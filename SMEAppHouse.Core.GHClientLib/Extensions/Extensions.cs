using System.Collections.Generic;
using System.Linq;
using GeoCoordinatePortable;
using SMEAppHouse.Core.GHClientLib.Model;

namespace SMEAppHouse.Core.GHClientLib.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static double GetDistance(this LngLatPoint from, LngLatPoint to)
        {
            var sCoord = new GeoCoordinate(from.Lat, from.Lng);
            var eCoord = new GeoCoordinate(to.Lat, to.Lng);
            return sCoord.GetDistanceTo(eCoord);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public static LngLatPoint ToLngLatPoint(this Location loc)
        {
            return new LngLatPoint()
            {
                Lng = loc.Lon ?? 0,
                Lat = loc.Lat ?? 0
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public static Address ToAddress(this Location loc)
        {
            return new Address(loc.LocationId, loc.LocationId, loc.Lon, loc.Lat);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coordArr"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        public static LngLatPoint GetLngLatPoint(this ResponseCoordinatesArray coordArr, int idx)
        {
            return new LngLatPoint()
            {
                Lng = coordArr[idx][0] ?? 0,
                Lat = coordArr[idx][1] ?? 0
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseCoordinates"></param>
        /// <returns></returns>
        public static List<RoutePoint> ToRoutePoints(this ResponseCoordinates responseCoordinates)
        {
            var rtPts = new List<RoutePoint>();
            responseCoordinates.Coordinates.ForEach(r =>
            {
                var rtPt = new RoutePoint("", new List<double[]>
                {
                    new[]
                    {
                        r[0] ?? 0,
                        r[1] ?? 0
                    }
                });
                rtPts.Add(rtPt);
            });
            return rtPts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public static IEnumerable<LngLatPoint> ToListOfLngLatPts(this List<double[]> coords)
        {
            var result = coords.Select(p => new LngLatPoint(p[0], p[1]));
            return result;
        }

        public static double[] ToDblArr(this LngLatPoint lngLatPoint)
        {
            return new[] { lngLatPoint.Lng, lngLatPoint.Lat };
        }

        public static List<double[]> ToListOfDblArr(this List<LngLatPoint> lngLatPoints)
        {
            return lngLatPoints.Select(p => p.ToDblArr()).ToList();
        }


    }
}
