using System;
using System.Collections.Generic;
using System.Linq;
using SMEAppHouse.Core.CodeKits.Tools;
using SMEAppHouse.Core.GHClientLib.Api;
using SMEAppHouse.Core.GHClientLib.Extensions;
using SMEAppHouse.Core.GHClientLib.Model;

namespace SMEAppHouse.Core.GHClientLib.Utilities
{
    public class GHRouter
    {

        private readonly VrpApi _vrpApiInstance;
        private readonly SolutionApi _slnApiInstance;
        private readonly RoutingApi _rtngApiInstance;
        private readonly string _apiKey;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        public GHRouter(string apiKey)
        {
            _apiKey = apiKey;

            _vrpApiInstance = new VrpApi();
            _rtngApiInstance = new RoutingApi();
            _slnApiInstance = new SolutionApi();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locations"></param>
        /// <param name="vehicle"></param>
        /// <param name="optimize"></param>
        /// <param name="calcPoints"></param>
        /// <param name="pointsEncoded"></param>
        /// <returns></returns>
        public Tuple<List<Activity>, List<RoutePoint>> Calculate(List<Location> locations
            , Vehicle vehicle
            , bool optimize = false
            , bool calcPoints = true
            , bool pointsEncoded = true)
        {
            var traceMsg = new List<string>();
            try
            {
                if (optimize)
                {
                    traceMsg.Add(string.Format("Calculate:", "OPTIMIZE"));

                    // remove origin and final points as we only 
                    // need to optimize the job sites in between.
                    locations.RemoveAt(0);
                    locations.RemoveAt(locations.Count - 1);

                    var response = CalculateRouteOptimize(locations
                            , vehicle
                            , pointsEncoded
                            , calcPoints);

                    var route = (((response ?? throw new InvalidOperationException("Response is null. No route solution returned."))?.Solution ??
                                  throw new InvalidOperationException("No route solution returned.")).Routes ??
                                 throw new InvalidOperationException("No route solution returned.")).FirstOrDefault() ??
                                        throw new InvalidOperationException("No route solution returned.");

                    var wpCoords = route.Points;
                    return new Tuple<List<Activity>, List<RoutePoint>>(route.Activities, wpCoords);
                }
                else
                {
                    traceMsg.Add(string.Format("Calculate:", "NON-OPTIMIZE"));
                    Exception ex = null;
                    RouteResponse response = null;
                    RetryCodeKit.LoopAction(() =>
                    {
                        traceMsg.Add(string.Format("Calculate:", "BEFORE_CalculateRoute"));
                        response = CalculateRoute(locations
                            , vehicle: vehicle.VehicleType.Profile?.ToString() ?? "car"
                            , pointsEncoded: pointsEncoded
                            , calcPoints: calcPoints
                            , details: "time");
                        traceMsg.Add(string.Format("Calculate:", "AFTER_CalculateRoute"));
                        return response != null;
                    }, ref ex, true, 10, 5000);

                    if (response == null)
                        throw new Exception("GHRouteNonOptim> For some reason GH fails to cater to route request.", ex);
                    else if (ex != null)
                        throw ex;
                    else
                    {
                        try
                        {
                            traceMsg.Add(string.Format("Calculate:", "BEFORE_GettingRouteData"));
                            var actvAndWpCoords = CalculateActivitiesAndWpCoords(response.Paths[0], locations);
                            var activities = actvAndWpCoords.Item1.ToList();
                            var routePoints = actvAndWpCoords.Item2; //rtngResponse.Paths[0].Points.ToRoutePoints();
                            return new Tuple<List<Activity>, List<RoutePoint>>(activities, routePoints);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }

                }
            }
            catch (Exception exception)
            {
                //var xMsg = "Exception when calling SolutionApi.GetSolution: " + exception.Message;
                //var auditDat = new
                //{
                //    /*
                //     List<Location> locations
                //                    , Vehicle vehicle
                //                    , bool optimize = false
                //                    , bool calcPoints = true
                //                    , bool pointsEncoded = true
                //     */
                //};
                //var ghCalcEx = new GHCalculationException(xMsg, exception, auditDat);
                //throw ghCalcEx;

                exception.Data.Add("Trace", traceMsg);
                throw new Exception("Exception when calling SolutionApi.GetSolution", exception);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="locations"></param>
        /// <returns></returns>
        private static Tuple<IEnumerable<Activity>, List<RoutePoint>> CalculateActivitiesAndWpCoords(RouteResponsePath path, IReadOnlyList<Location> locations)
        {
            var activities = new List<Activity>();
            var points = path.Points;
            var snappedPts = path.SnappedWaypoints;
            var times = path.Details.Times;
            var wpCoords = new List<RoutePoint>();

            Func<Location, ResponseCoordinatesArray, int, Tuple<int, LngLatPoint>> findClosest = (loc, coords, skpIdx) =>
            {
                try
                {
                    var lc = coords
                                        .Skip(skpIdx)
                                        .OrderBy(p => loc.ToLngLatPoint()
                                                            .GetDistance(new LngLatPoint()
                                                            {
                                                                Lng = p[0] ?? 0,
                                                                Lat = p[1] ?? 0
                                                            }))
                                        .FirstOrDefault();
                    var idx = coords.FindIndex(p => p.Equals(lc));
                    return new Tuple<int, LngLatPoint>(idx, new LngLatPoint()
                    {
                        Lng = lc[0] ?? 0,
                        Lat = lc[1] ?? 0
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            };

            Func<int, int, ResponseCoordinatesArray, Tuple<double, long, RoutePoint>> computeTotTimeAndDistance = (frmIdx, toIdx, coords) =>
            {
                try
                {
                    var src = coords.Skip(frmIdx).Take(toIdx - frmIdx).ToArray();
                    var dist = 0.0;
                    var wps = new RoutePoint();

                    for (var i = 0; i < src.Count(); i++)
                    {
                        var ths = src[i];
                        wps.Coordinates.Add(new[] { ths[0] ?? 0.0, ths[1] ?? 0.0 });

                        if (i == 0) continue;
                        var prv = src[i - 1];

                        // compute for the distance
                        var srcPt = new LngLatPoint(prv[0], prv[1]);
                        var dstPt = new LngLatPoint(ths[0], ths[1]);
                        dist += srcPt.GetDistance(dstPt);
                    }

                    // compute total travel time
                    var tmSrc = times.Where(p => p[0] >= frmIdx && p[1] < toIdx);
                    var totSecs = tmSrc.Sum(s => s[2]);

                    return new Tuple<double, long, RoutePoint>(dist, totSecs ?? 0, wps);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            };

            Tuple<int, LngLatPoint> lstClosest = null;

            for (var i = 0; i < locations.Count; i++)
            {
                try
                {
                    var loc = locations[i];

                    if (i == 0)
                        activities.Add(new Activity(Activity.TypeEnum.Start, loc.LocationId, loc.LocationId));
                    else
                    {
                        var closest = findClosest(loc, points.Coordinates, lstClosest?.Item1 ?? 0);
                        var tupDistTimeCoords = computeTotTimeAndDistance(lstClosest?.Item1 ?? 0, closest.Item1, points.Coordinates);
                        var type = (i == (locations.Count - 1)) ? Activity.TypeEnum.End : Activity.TypeEnum.Service;
                        activities.Add(new Activity(id: loc.LocationId,
                                                    type: type,
                                                    locationId: loc.LocationId,
                                                    arrTime: tupDistTimeCoords.Item2 / 1000,
                                                    drivingTime: tupDistTimeCoords.Item2 / 1000,
                                                    endTime: tupDistTimeCoords.Item2 / 1000,
                                                    waitingTime: 0,
                                                    distance: tupDistTimeCoords.Item1));

                        wpCoords.Add(tupDistTimeCoords.Item3);
                        lstClosest = closest;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
            return new Tuple<IEnumerable<Activity>, List<RoutePoint>>(activities, wpCoords);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locations"></param>
        /// <param name="vehicle"></param>
        /// <param name="pointsEncoded"></param>
        /// <param name="locale"></param>
        /// <param name="instructions"></param>
        /// <param name="elevation"></param>
        /// <param name="calcPoints"></param>
        /// <param name="pointHint"></param>
        /// <param name="chDisable"></param>
        /// <param name="weighting"></param>
        /// <param name="edgeTraversal"></param>
        /// <param name="algorithm"></param>
        /// <param name="heading"></param>
        /// <param name="headingPenalty"></param>
        /// <param name="passThrough"></param>
        /// <param name="roundTripDistance"></param>
        /// <param name="roundTripSeed"></param>
        /// <param name="alternativeRouteMaxPaths"></param>
        /// <param name="alternativeRouteMaxWeightFactor"></param>
        /// <param name="alternativeRouteMaxShareFactor"></param>
        /// <param name="avoid"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        private RouteResponse CalculateRoute(IEnumerable<Location> locations, string vehicle = null,
                                            bool? pointsEncoded = true,
                                            string locale = null,
                                            bool? instructions = null,
                                            bool? elevation = null,
                                            bool? calcPoints = null,
                                            List<string> pointHint = null,
                                            bool? chDisable = null,
                                            string weighting = null,
                                            bool? edgeTraversal = null,
                                            string algorithm = null,
                                            int? heading = null,
                                            int? headingPenalty = null,
                                            bool? passThrough = null,
                                            int? roundTripDistance = null,
                                            long? roundTripSeed = null,
                                            int? alternativeRouteMaxPaths = null,
                                            int? alternativeRouteMaxWeightFactor = null,
                                            int? alternativeRouteMaxShareFactor = null,
                                            string avoid = null,
                                            string details = null)
        {
            try
            {
                var rtPoints = locations.Select(p => p.Lat + "," + p.Lon).ToList();
                var response = _rtngApiInstance.RouteGet(rtPoints, pointsEncoded, _apiKey
                                                        , locale, instructions, vehicle
                                                        , elevation, calcPoints, pointHint
                                                        , chDisable, weighting, edgeTraversal
                                                        , algorithm, heading, headingPenalty
                                                        , passThrough, roundTripDistance, roundTripSeed
                                                        , alternativeRouteMaxPaths, alternativeRouteMaxWeightFactor
                                                        , alternativeRouteMaxShareFactor, avoid, details);
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locations"></param>
        /// <param name="vehicle"></param>
        /// <param name="pointsEncoded"></param>
        /// <param name="calcPoints"></param>
        /// <returns></returns>
        private Response CalculateRouteOptimize(List<Location> locations
                                    , Vehicle vehicle
                                    , bool pointsEncoded = true
                                    , bool calcPoints = true)
        {

            // comnpose the body for the request

            var clientServices = new List<Service>();

            try
            {
                locations.ForEach(loc =>
                {
                    var address = new Address(loc.LocationId, $"servicing-{loc.LocationId}", loc.Lon, loc.Lat);
                    var srvc = new Service(Id: address.LocationId,
                                            Name: address.Name,
                                            Type: loc.ServiceType,
                                            Address: address);
                    clientServices.Add(srvc);
                });

                var routingCfg = new Model.Configuration()
                {
                    Routing = new Routing(calcPoints: calcPoints,
                                            considerTraffic: true,
                                            networkDataProvider: Routing.NetworkDataProviderEnum.Openstreetmap)
                };

                var body = new Request(Vehicles: new List<Vehicle>() { vehicle },
                    VehicleTypes: new List<VehicleType>() { vehicle.VehicleType },
                    Configuration: routingCfg,
                    Services: clientServices);

                // Request | Request object that contains the problem to be solved
                var jobId = _vrpApiInstance.PostVrp(_apiKey, body);

                // Return the solution associated to the jobId

                Exception ex = null;
                Response response = null;

                RetryCodeKit.LoopAction(() =>
                {
                    response = _slnApiInstance.GetSolution(_apiKey, jobId._JobId);
                    return response?.Status != null && 
                           response.Solution?.Routes != null && 
                           response.Solution.Routes.Any() && 
                           (response != null && response?.Status.Value != Response.StatusEnum.Finished);
                }, ref ex, true, 10, 5000);

                if (ex != null)
                    throw ex;

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
