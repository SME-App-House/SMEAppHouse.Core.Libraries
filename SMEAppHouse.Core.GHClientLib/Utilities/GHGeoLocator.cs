using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using SMEAppHouse.Core.GHClientLib.Model;

namespace SMEAppHouse.Core.GHClientLib.Utilities
{
    public class GHGeoLocator
    {
        private readonly string _apiKey;

        public string LastResponse { get; private set; } = "";
        public uint TimeoutSec { get; set; } = 10;

        public GHGeoLocator(string key)
        {
            _apiKey = key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public HitsResponse Geolocate(string location)
        {
            HitsResponse hits = null;
            var url = $"https://graphhopper.com/api/1/geocode?locale=en&debug=true&key={_apiKey}&q={location.Replace(' ', '+')}";

            var request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null) return null;

            request.Proxy = null; // Performance hack!
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response != null && response.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"Server error (HTTP {response.StatusCode}: {response.StatusDescription}).");

                if (response == null) return null;
                var stream = response.GetResponseStream();
                var memStream = new MemoryStream();
                if (stream == null) return null;
                stream.CopyTo(memStream);
                memStream.Seek(0, SeekOrigin.Begin);
                var jsonSerializer = new DataContractJsonSerializer(typeof(HitsResponse));
                var objResponse = jsonSerializer.ReadObject(memStream);
                hits = objResponse as HitsResponse;
                CreateLastResponse(memStream);

                memStream.Close();
                stream.Close();
            }

            return hits;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="latLngs"></param>
        /// <returns></returns>
        public List<List<Tuple<double, double>>> GetDistanceTimeMatrix(List<Tuple<string, string>> latLngs)
        {
            var distTimes = new List<List<Tuple<double, double>>>();
            MatrixResponse matrix = null;
            var url = "https://graphhopper.com/api/1/matrix?type=json&vehicle=car&debug=true&out_array=times&out_array=distances&key=";
            url += _apiKey;

            url = latLngs.Aggregate(url, (current, latLng) => current + ("&point=" + latLng.Item1) + "," + latLng.Item2);

            if (WebRequest.Create(url) is HttpWebRequest request)
            {
                request.Proxy = null; // Performance hack!
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response != null && response.StatusCode != HttpStatusCode.OK)
                        throw new Exception($"Server error (HTTP {response.StatusCode}: {response.StatusDescription}).");

                    if (response != null)
                    {
                        var stream = response.GetResponseStream();
                        var memStream = new MemoryStream();
                        if (stream != null)
                        {
                            stream.CopyTo(memStream);
                            memStream.Seek(0, SeekOrigin.Begin);
                            var jsonSerializer = new DataContractJsonSerializer(typeof(MatrixResponse));
                            var objResponse = jsonSerializer.ReadObject(memStream);
                            matrix = objResponse as MatrixResponse;
                            CreateLastResponse(memStream);

                            memStream.Close();
                            stream.Close();
                        }
                    }
                }
            }

            // time and distance arrays have all the same length
            if (matrix == null) return distTimes;
            for (var i = 0; i < matrix.Distances.Count(); i++)
            {
                var row = new List<Tuple<double, double>>();
                for (var k = 0; k < matrix.Distances[i].Count(); k++)
                {
                    var dist = matrix.Distances[i][k];
                    var time = matrix.Times[i][k];
                    var distTime = new Tuple<double, double>(Convert.ToDouble(dist ?? 0), Convert.ToDouble(time ?? 0));

                    row.Add(distTime);
                }

                distTimes.Add(row);
            }

            return distTimes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="latLngs"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public List<int> OptimizeRoute(List<Tuple<string, string>> latLngs, Tuple<string, string> start)
        {
            var optimizedOrder = new List<int>();
            var routeRequest = new RouteOptimizationRequest();
            var url = $"https://graphhopper.com/api/1/vrp/optimize?key={_apiKey}";

            RouteOptimizationResponse routeResponse = null;

            // create and assign the vehicle defining the point of origin
            var vehicle = new RouteOptimizationVehicle
            {
                StartAddress = new RouteOptimizationAddress
                {
                    LocationId = "start",
                    //Lat = double.Parse(start.Item1.Replace('.', ',')),
                    //Lon = double.Parse(start.Item2.Replace('.', ','))
                    Lat = double.Parse(start.Item1),
                    Lon = double.Parse(start.Item2)
                },
                VehicleId = "car"
            };
            routeRequest.Vehicles = new[] { vehicle };

            // define the services that will specify the end points
            var svcEndpoints = new RouteOptimizationServiceEndPoint[latLngs.Count];
            for (var i = 0; i < latLngs.Count; i++)
            {
                var svcEndpoint = new RouteOptimizationServiceEndPoint
                {
                    Id = i.ToString(),
                    Name = i.ToString(),
                    Address = new RouteOptimizationAddress
                    {
                        LocationId = i.ToString(),
                        //Lat = double.Parse(latLngs[i].Item1.Replace('.', ',')),
                        //Lon = double.Parse(latLngs[i].Item2.Replace('.', ','))
                        Lat = double.Parse(latLngs[i].Item1),
                        Lon = double.Parse(latLngs[i].Item2)
                    }
                };

                svcEndpoints[i] = svcEndpoint;
            }
            routeRequest.Services = svcEndpoints;


            var request = WebRequest.Create(url);// as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Proxy = null; // Performance hack!

            var jsonSerializer = new DataContractJsonSerializer(typeof(RouteOptimizationRequest));
            var outStream = new MemoryStream();

            jsonSerializer.WriteObject(outStream, routeRequest);
            outStream.Seek(0, SeekOrigin.Begin);
            outStream.CopyTo(request.GetRequestStream());

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response != null)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception($"Server error (HTTP {response.StatusCode}: {response.StatusDescription}).");

                    using (var stream = response.GetResponseStream())
                    {
                        if (stream == null)
                            return optimizedOrder;

                        var memStream = new MemoryStream();
                        stream.CopyTo(memStream);
                        memStream.Seek(0, SeekOrigin.Begin);
                        var jsonDeserializer = new DataContractJsonSerializer(typeof(RouteOptimizationResponse));
                        var objResponse = jsonDeserializer.ReadObject(memStream);
                        routeResponse = objResponse as RouteOptimizationResponse;
                        CreateLastResponse(memStream);

                        memStream.Close();
                        stream.Close();
                    }
                }
            }

            if (routeResponse == null)
                return optimizedOrder;

            url = $"https://graphhopper.com/api/1/vrp/solution/{routeResponse.JobId}?key={_apiKey}";

            for (var i = 0; i < (TimeoutSec * 2); i++)
            {
                request = WebRequest.Create(url);// as HttpWebRequest;
                request.Method = "GET";
                request.Proxy = null; // Performance hack!

                using (var response = request.GetResponse())// as HttpWebResponse)
                {
                    var httpResponse = (HttpWebResponse)response;
                    if (httpResponse.StatusCode != HttpStatusCode.OK)
                        throw new Exception($"Server error (HTTP {httpResponse.StatusCode}: {httpResponse.StatusDescription}).");

                    using (var stream = httpResponse.GetResponseStream())
                    {
                        var memStream = new MemoryStream();
                        if (stream != null)
                        {
                            stream.CopyTo(memStream);
                            memStream.Seek(0, SeekOrigin.Begin);
                            var jsonDeserializer = new DataContractJsonSerializer(typeof(RouteOptimizationResponse));
                            var objResponse = jsonDeserializer.ReadObject(memStream);
                            routeResponse = objResponse as RouteOptimizationResponse;
                            CreateLastResponse(memStream);

                            memStream.Close();
                            stream.Close();
                        }
                    }
                }

                if (routeResponse == null)
                    continue;

                if (!routeResponse.Status.Equals("finished"))
                    System.Threading.Thread.Sleep(500);

                if (!routeResponse.Solution.Routes.Any())
                    continue;

                foreach (var activity in routeResponse.Solution.Routes[0].Activities)
                {
                    if ("start" == activity.LocationId)
                        continue;

                    var id = 0;
                    if (int.TryParse(activity.LocationId, out id))
                        optimizedOrder.Add(id);
                }

                break;

            }

            return optimizedOrder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        private void CreateLastResponse(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var sb = new StringBuilder();
            var buf = new byte[8192];
            var count = 0;
            do
            {
                count = stream.Read(buf, 0, buf.Length);
                if (count != 0)
                {
                    sb.Append(Encoding.UTF8.GetString(buf, 0, count)); // just hardcoding UTF8 here
                }
            } while (count > 0);

            LastResponse = sb.ToString();
        }
    }
}
