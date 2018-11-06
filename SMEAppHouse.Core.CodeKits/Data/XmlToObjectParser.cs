using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace SMEAppHouse.Core.CodeKits.Data
{
    public static class XmlToObjectParser
    {
        public static dynamic ParseFromXml(string xml)
        {
            IDictionary<string, object> parsedObject = new ExpandoObject();

            var rootElement = XElement.Parse(xml);

            var root = CreateChildElement(rootElement);

            parsedObject.Add(rootElement.Name.LocalName, root);

            return parsedObject;
        }

        private static dynamic CreateChildElement(XElement parent)
        {
            if (!parent.Attributes().Any() && !parent.Elements().Any())
                return null;

            IDictionary<string, object> child = new ExpandoObject();

            parent.Attributes().ToList().ForEach(attr =>
            {
                child.Add(attr.Name.LocalName, attr.Value);

                if (!child.ContainsKey("NodeName"))
                    if (attr.Parent != null) child.Add("NodeName", attr.Parent.Name.LocalName);
            });

            parent.Elements().ToList().ForEach(childElement =>
            {
                var grandChild = CreateChildElement(childElement);

                if (grandChild != null)
                {
                    string nodeName = grandChild.NodeName;
                    if (child.ContainsKey(nodeName) && child[nodeName].GetType() != typeof(List<dynamic>))
                    {
                        var firstValue = child[nodeName];
                        child[nodeName] = new List<dynamic>();
                        ((dynamic)child[nodeName]).Add(firstValue);
                        ((dynamic)child[nodeName]).Add(grandChild);
                    }
                    else if (child.ContainsKey(nodeName) && child[nodeName].GetType() == typeof(List<dynamic>))
                    {
                        ((dynamic)child[nodeName]).Add(grandChild);
                    }
                    else
                    {
                        child.Add(childElement.Name.LocalName, CreateChildElement(childElement));
                        if (!child.ContainsKey("NodeName"))
                            child.Add("NodeName", parent.Name.LocalName);
                    }
                }
                else
                {
                    if (child.ContainsKey(childElement.Name.LocalName))
                    {
                        var firstValue = child[childElement.Name.LocalName];
                        child[childElement.Name.LocalName] = new List<dynamic>();
                        ((List<dynamic>)child[childElement.Name.LocalName]).Add(firstValue);
                        ((List<dynamic>)child[childElement.Name.LocalName]).Add(childElement.Value);
                    }
                    else
                    {
                        child.Add(childElement.Name.LocalName, childElement.Value);
                    }

                    if (!child.ContainsKey("NodeName"))
                        child.Add("NodeName", parent.Name.LocalName);
                }
            });

            return child;
        }

        public static XElement SerializeToXml(this object o)
        {
            var t = o.GetType();

            var extraTypes = t.GetProperties()
                .Where(p => p.PropertyType.IsInterface)
                .Select(p => p.GetValue(o, null).GetType())
                .ToArray();

            var serializer = new DataContractSerializer(t, extraTypes);
            var sw = new StringWriter();
            var xw = new XmlTextWriter(sw);
            serializer.WriteObject(xw, o);
            return XElement.Parse(sw.ToString());
        }
    }
}
