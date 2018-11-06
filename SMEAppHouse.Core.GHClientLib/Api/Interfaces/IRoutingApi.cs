using System.Collections.Generic;
using SMEAppHouse.Core.GHClientLib.Model;
using SMEAppHouse.Core.RestSharpClientLib;

namespace SMEAppHouse.Core.GHClientLib.Api.Interfaces
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IRoutingApi : IApiAccessor
    {
        #region Synchronous Operations

        /// <summary>
        /// Routing Request
        /// </summary>
        /// <remarks>
        /// The GraphHopper Routing API allows to calculate route and implement navigation via the turn instructions
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="pointsEncoded">Specify multiple points for which the route should be calculated. The order is important. Specify at least two points.</param>
        /// <param name="key">IMPORTANT- TODO - currently you have to pass false for the swagger client - Have not found a way to force add a parameter. If &#x60;false&#x60; the coordinates in &#x60;point&#x60; and &#x60;snapped_waypoints&#x60; are returned as array using the order [lon,lat,elevation] for every point. If &#x60;true&#x60; the coordinates will be encoded as string leading to less bandwith usage. You&#39;ll need a special handling for the decoding of this string on the client-side. We provide open source code in [Java](https://github.com/graphhopper/graphhopper/blob/d70b63660ac5200b03c38ba3406b8f93976628a6/web/src/main/java/com/graphhopper/http/WebHelper.java#L43) and [JavaScript](https://github.com/graphhopper/graphhopper/blob/d70b63660ac5200b03c38ba3406b8f93976628a6/web/src/main/webapp/js/ghrequest.js#L139). It is especially important to use no 3rd party client if you set &#x60;elevation&#x3D;true&#x60;!</param>
        /// <param name="locale">Get your key at graphhopper.com</param>
        /// <param name="instructions">The locale of the resulting turn instructions. E.g. &#x60;pt_PT&#x60; for Portuguese or &#x60;de&#x60; for German (optional)</param>
        /// <param name="vehicle">If instruction should be calculated and returned (optional)</param>
        /// <param name="elevation">The vehicle for which the route should be calculated. Other vehicles are foot, small_truck, ... (optional)</param>
        /// <param name="calcPoints">If &#x60;true&#x60; a third dimension - the elevation - is included in the polyline or in the GeoJson. If enabled you have to use a modified version of the decoding method or set points_encoded to &#x60;false&#x60;. See the points_encoded attribute for more details. Additionally a request can fail if the vehicle does not support elevation. See the features object for every vehicle. (optional)</param>
        /// <param name="pointHint">If the points for the route should be calculated at all printing out only distance and time. (optional)</param>
        /// <param name="chDisable">Optional parameter. Specifies a hint for each &#x60;point&#x60; parameter to prefer a certain street for the closest location lookup. E.g. if there is an address or house with two or more neighboring streets you can control for which street the closest location is looked up. (optional)</param>
        /// <param name="weighting">Use this parameter in combination with one or more parameters of this table (optional)</param>
        /// <param name="edgeTraversal">Which kind of &#39;best&#39; route calculation you need. Other option is &#x60;shortest&#x60; (e.g. for &#x60;vehicle&#x3D;foot&#x60; or &#x60;bike&#x60;), &#x60;short_fastest&#x60; if time and distance is expensive e.g. for &#x60;vehicle&#x3D;truck&#x60; (optional)</param>
        /// <param name="algorithm">Use &#x60;true&#x60; if you want to consider turn restrictions for bike and motor vehicles. Keep in mind that the response time is roughly 2 times slower. (optional)</param>
        /// <param name="heading">The algorithm to calculate the route. Other options are &#x60;dijkstra&#x60;, &#x60;astar&#x60;, &#x60;astarbi&#x60;, &#x60;alternative_route&#x60; and &#x60;round_trip&#x60; (optional)</param>
        /// <param name="headingPenalty">Favour a heading direction for a certain point. Specify either one heading for the start point or as many as there are points. In this case headings are associated by their order to the specific points. Headings are given as north based clockwise angle between 0 and 360 degree. This parameter also influences the tour generated with &#x60;algorithm&#x3D;round_trip&#x60; and force the initial direction. (optional)</param>
        /// <param name="passThrough">Penalty for omitting a specified heading. The penalty corresponds to the accepted time delay in seconds in comparison to the route without a heading. (optional)</param>
        /// <param name="roundTripDistance">If &#x60;true&#x60; u-turns are avoided at via-points with regard to the &#x60;heading_penalty&#x60;. (optional)</param>
        /// <param name="roundTripSeed">If &#x60;algorithm&#x3D;round_trip&#x60; this parameter configures approximative length of the resulting round trip (optional)</param>
        /// <param name="alternativeRouteMaxPaths">If &#x60;algorithm&#x3D;round_trip&#x60; this parameter introduces randomness if e.g. the first try wasn&#39;t good. (optional)</param>
        /// <param name="alternativeRouteMaxWeightFactor">If &#x60;algorithm&#x3D;alternative_route&#x60; this parameter sets the number of maximum paths which should be calculated. Increasing can lead to worse alternatives. (optional)</param>
        /// <param name="alternativeRouteMaxShareFactor">If &#x60;algorithm&#x3D;alternative_route&#x60; this parameter sets the factor by which the alternatives routes can be longer than the optimal route. Increasing can lead to worse alternatives. (optional)</param>
        /// <param name="avoid">If &#x60;algorithm&#x3D;alternative_route&#x60; this parameter specifies how much alternatives routes can have maximum in common with the optimal route. Increasing can lead to worse alternatives. (optional)</param>
        /// <param name="details">comma separate list to avoid certain roads. You can avoid motorway, ferry, tunnel or track (optional)</param>
        /// <param name="details"></param>
        /// <returns>RouteResponse</returns>
        RouteResponse RouteGet(List<string> point, bool? pointsEncoded, string key, string locale = null, bool? instructions = null, string vehicle = null, bool? elevation = null, bool? calcPoints = null, List<string> pointHint = null, bool? chDisable = null, string weighting = null, bool? edgeTraversal = null, string algorithm = null, int? heading = null, int? headingPenalty = null, bool? passThrough = null, int? roundTripDistance = null, long? roundTripSeed = null, int? alternativeRouteMaxPaths = null, int? alternativeRouteMaxWeightFactor = null, int? alternativeRouteMaxShareFactor = null, string avoid = null, string details = null);

        /// <summary>
        /// Routing Request
        /// </summary>
        /// <remarks>
        /// The GraphHopper Routing API allows to calculate route and implement navigation via the turn instructions
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="pointsEncoded">Specify multiple points for which the route should be calculated. The order is important. Specify at least two points.</param>
        /// <param name="key">IMPORTANT- TODO - currently you have to pass false for the swagger client - Have not found a way to force add a parameter. If &#x60;false&#x60; the coordinates in &#x60;point&#x60; and &#x60;snapped_waypoints&#x60; are returned as array using the order [lon,lat,elevation] for every point. If &#x60;true&#x60; the coordinates will be encoded as string leading to less bandwith usage. You&#39;ll need a special handling for the decoding of this string on the client-side. We provide open source code in [Java](https://github.com/graphhopper/graphhopper/blob/d70b63660ac5200b03c38ba3406b8f93976628a6/web/src/main/java/com/graphhopper/http/WebHelper.java#L43) and [JavaScript](https://github.com/graphhopper/graphhopper/blob/d70b63660ac5200b03c38ba3406b8f93976628a6/web/src/main/webapp/js/ghrequest.js#L139). It is especially important to use no 3rd party client if you set &#x60;elevation&#x3D;true&#x60;!</param>
        /// <param name="locale">Get your key at graphhopper.com</param>
        /// <param name="instructions">The locale of the resulting turn instructions. E.g. &#x60;pt_PT&#x60; for Portuguese or &#x60;de&#x60; for German (optional)</param>
        /// <param name="vehicle">If instruction should be calculated and returned (optional)</param>
        /// <param name="elevation">The vehicle for which the route should be calculated. Other vehicles are foot, small_truck, ... (optional)</param>
        /// <param name="calcPoints">If &#x60;true&#x60; a third dimension - the elevation - is included in the polyline or in the GeoJson. If enabled you have to use a modified version of the decoding method or set points_encoded to &#x60;false&#x60;. See the points_encoded attribute for more details. Additionally a request can fail if the vehicle does not support elevation. See the features object for every vehicle. (optional)</param>
        /// <param name="pointHint">If the points for the route should be calculated at all printing out only distance and time. (optional)</param>
        /// <param name="chDisable">Optional parameter. Specifies a hint for each &#x60;point&#x60; parameter to prefer a certain street for the closest location lookup. E.g. if there is an address or house with two or more neighboring streets you can control for which street the closest location is looked up. (optional)</param>
        /// <param name="weighting">Use this parameter in combination with one or more parameters of this table (optional)</param>
        /// <param name="edgeTraversal">Which kind of &#39;best&#39; route calculation you need. Other option is &#x60;shortest&#x60; (e.g. for &#x60;vehicle&#x3D;foot&#x60; or &#x60;bike&#x60;), &#x60;short_fastest&#x60; if time and distance is expensive e.g. for &#x60;vehicle&#x3D;truck&#x60; (optional)</param>
        /// <param name="algorithm">Use &#x60;true&#x60; if you want to consider turn restrictions for bike and motor vehicles. Keep in mind that the response time is roughly 2 times slower. (optional)</param>
        /// <param name="heading">The algorithm to calculate the route. Other options are &#x60;dijkstra&#x60;, &#x60;astar&#x60;, &#x60;astarbi&#x60;, &#x60;alternative_route&#x60; and &#x60;round_trip&#x60; (optional)</param>
        /// <param name="headingPenalty">Favour a heading direction for a certain point. Specify either one heading for the start point or as many as there are points. In this case headings are associated by their order to the specific points. Headings are given as north based clockwise angle between 0 and 360 degree. This parameter also influences the tour generated with &#x60;algorithm&#x3D;round_trip&#x60; and force the initial direction. (optional)</param>
        /// <param name="passThrough">Penalty for omitting a specified heading. The penalty corresponds to the accepted time delay in seconds in comparison to the route without a heading. (optional)</param>
        /// <param name="roundTripDistance">If &#x60;true&#x60; u-turns are avoided at via-points with regard to the &#x60;heading_penalty&#x60;. (optional)</param>
        /// <param name="roundTripSeed">If &#x60;algorithm&#x3D;round_trip&#x60; this parameter configures approximative length of the resulting round trip (optional)</param>
        /// <param name="alternativeRouteMaxPaths">If &#x60;algorithm&#x3D;round_trip&#x60; this parameter introduces randomness if e.g. the first try wasn&#39;t good. (optional)</param>
        /// <param name="alternativeRouteMaxWeightFactor">If &#x60;algorithm&#x3D;alternative_route&#x60; this parameter sets the number of maximum paths which should be calculated. Increasing can lead to worse alternatives. (optional)</param>
        /// <param name="alternativeRouteMaxShareFactor">If &#x60;algorithm&#x3D;alternative_route&#x60; this parameter sets the factor by which the alternatives routes can be longer than the optimal route. Increasing can lead to worse alternatives. (optional)</param>
        /// <param name="avoid">If &#x60;algorithm&#x3D;alternative_route&#x60; this parameter specifies how much alternatives routes can have maximum in common with the optimal route. Increasing can lead to worse alternatives. (optional)</param>
        /// <param name="details">comma separate list to avoid certain roads. You can avoid motorway, ferry, tunnel or track (optional)</param>
        /// <param name="details">Optional parameter to retrieve path details.</param>
        /// <returns>ApiResponse of RouteResponse</returns>
        ApiResponse<RouteResponse> RouteGetWithHttpInfo(List<string> point, bool? pointsEncoded, string key, string locale = null, bool? instructions = null, string vehicle = null, bool? elevation = null, bool? calcPoints = null, List<string> pointHint = null, bool? chDisable = null, string weighting = null, bool? edgeTraversal = null, string algorithm = null, int? heading = null, int? headingPenalty = null, bool? passThrough = null, int? roundTripDistance = null, long? roundTripSeed = null, int? alternativeRouteMaxPaths = null, int? alternativeRouteMaxWeightFactor = null, int? alternativeRouteMaxShareFactor = null, string avoid = null, string details = null);
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Routing Request
        /// </summary>
        /// <remarks>
        /// The GraphHopper Routing API allows to calculate route and implement navigation via the turn instructions
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="pointsEncoded">Specify multiple points for which the route should be calculated. The order is important. Specify at least two points.</param>
        /// <param name="key">IMPORTANT- TODO - currently you have to pass false for the swagger client - Have not found a way to force add a parameter. If &#x60;false&#x60; the coordinates in &#x60;point&#x60; and &#x60;snapped_waypoints&#x60; are returned as array using the order [lon,lat,elevation] for every point. If &#x60;true&#x60; the coordinates will be encoded as string leading to less bandwith usage. You&#39;ll need a special handling for the decoding of this string on the client-side. We provide open source code in [Java](https://github.com/graphhopper/graphhopper/blob/d70b63660ac5200b03c38ba3406b8f93976628a6/web/src/main/java/com/graphhopper/http/WebHelper.java#L43) and [JavaScript](https://github.com/graphhopper/graphhopper/blob/d70b63660ac5200b03c38ba3406b8f93976628a6/web/src/main/webapp/js/ghrequest.js#L139). It is especially important to use no 3rd party client if you set &#x60;elevation&#x3D;true&#x60;!</param>
        /// <param name="locale">Get your key at graphhopper.com</param>
        /// <param name="instructions">The locale of the resulting turn instructions. E.g. &#x60;pt_PT&#x60; for Portuguese or &#x60;de&#x60; for German (optional)</param>
        /// <param name="vehicle">If instruction should be calculated and returned (optional)</param>
        /// <param name="elevation">The vehicle for which the route should be calculated. Other vehicles are foot, small_truck, ... (optional)</param>
        /// <param name="calcPoints">If &#x60;true&#x60; a third dimension - the elevation - is included in the polyline or in the GeoJson. If enabled you have to use a modified version of the decoding method or set points_encoded to &#x60;false&#x60;. See the points_encoded attribute for more details. Additionally a request can fail if the vehicle does not support elevation. See the features object for every vehicle. (optional)</param>
        /// <param name="pointHint">If the points for the route should be calculated at all printing out only distance and time. (optional)</param>
        /// <param name="chDisable">Optional parameter. Specifies a hint for each &#x60;point&#x60; parameter to prefer a certain street for the closest location lookup. E.g. if there is an address or house with two or more neighboring streets you can control for which street the closest location is looked up. (optional)</param>
        /// <param name="weighting">Use this parameter in combination with one or more parameters of this table (optional)</param>
        /// <param name="edgeTraversal">Which kind of &#39;best&#39; route calculation you need. Other option is &#x60;shortest&#x60; (e.g. for &#x60;vehicle&#x3D;foot&#x60; or &#x60;bike&#x60;), &#x60;short_fastest&#x60; if time and distance is expensive e.g. for &#x60;vehicle&#x3D;truck&#x60; (optional)</param>
        /// <param name="algorithm">Use &#x60;true&#x60; if you want to consider turn restrictions for bike and motor vehicles. Keep in mind that the response time is roughly 2 times slower. (optional)</param>
        /// <param name="heading">The algorithm to calculate the route. Other options are &#x60;dijkstra&#x60;, &#x60;astar&#x60;, &#x60;astarbi&#x60;, &#x60;alternative_route&#x60; and &#x60;round_trip&#x60; (optional)</param>
        /// <param name="headingPenalty">Favour a heading direction for a certain point. Specify either one heading for the start point or as many as there are points. In this case headings are associated by their order to the specific points. Headings are given as north based clockwise angle between 0 and 360 degree. This parameter also influences the tour generated with &#x60;algorithm&#x3D;round_trip&#x60; and force the initial direction. (optional)</param>
        /// <param name="passThrough">Penalty for omitting a specified heading. The penalty corresponds to the accepted time delay in seconds in comparison to the route without a heading. (optional)</param>
        /// <param name="roundTripDistance">If &#x60;true&#x60; u-turns are avoided at via-points with regard to the &#x60;heading_penalty&#x60;. (optional)</param>
        /// <param name="roundTripSeed">If &#x60;algorithm&#x3D;round_trip&#x60; this parameter configures approximative length of the resulting round trip (optional)</param>
        /// <param name="alternativeRouteMaxPaths">If &#x60;algorithm&#x3D;round_trip&#x60; this parameter introduces randomness if e.g. the first try wasn&#39;t good. (optional)</param>
        /// <param name="alternativeRouteMaxWeightFactor">If &#x60;algorithm&#x3D;alternative_route&#x60; this parameter sets the number of maximum paths which should be calculated. Increasing can lead to worse alternatives. (optional)</param>
        /// <param name="alternativeRouteMaxShareFactor">If &#x60;algorithm&#x3D;alternative_route&#x60; this parameter sets the factor by which the alternatives routes can be longer than the optimal route. Increasing can lead to worse alternatives. (optional)</param>
        /// <param name="avoid">If &#x60;algorithm&#x3D;alternative_route&#x60; this parameter specifies how much alternatives routes can have maximum in common with the optimal route. Increasing can lead to worse alternatives. (optional)</param>
        /// <param name="avoid">comma separate list to avoid certain roads. You can avoid motorway, ferry, tunnel or track (optional)</param>
        /// <returns>Task of RouteResponse</returns>
        System.Threading.Tasks.Task<RouteResponse> RouteGetAsync(List<string> point, bool? pointsEncoded, string key, string locale = null, bool? instructions = null, string vehicle = null, bool? elevation = null, bool? calcPoints = null, List<string> pointHint = null, bool? chDisable = null, string weighting = null, bool? edgeTraversal = null, string algorithm = null, int? heading = null, int? headingPenalty = null, bool? passThrough = null, int? roundTripDistance = null, long? roundTripSeed = null, int? alternativeRouteMaxPaths = null, int? alternativeRouteMaxWeightFactor = null, int? alternativeRouteMaxShareFactor = null, string avoid = null);

        /// <summary>
        /// Routing Request
        /// </summary>
        /// <remarks>
        /// The GraphHopper Routing API allows to calculate route and implement navigation via the turn instructions
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="pointsEncoded">Specify multiple points for which the route should be calculated. The order is important. Specify at least two points.</param>
        /// <param name="key">IMPORTANT- TODO - currently you have to pass false for the swagger client - Have not found a way to force add a parameter. If &#x60;false&#x60; the coordinates in &#x60;point&#x60; and &#x60;snapped_waypoints&#x60; are returned as array using the order [lon,lat,elevation] for every point. If &#x60;true&#x60; the coordinates will be encoded as string leading to less bandwith usage. You&#39;ll need a special handling for the decoding of this string on the client-side. We provide open source code in [Java](https://github.com/graphhopper/graphhopper/blob/d70b63660ac5200b03c38ba3406b8f93976628a6/web/src/main/java/com/graphhopper/http/WebHelper.java#L43) and [JavaScript](https://github.com/graphhopper/graphhopper/blob/d70b63660ac5200b03c38ba3406b8f93976628a6/web/src/main/webapp/js/ghrequest.js#L139). It is especially important to use no 3rd party client if you set &#x60;elevation&#x3D;true&#x60;!</param>
        /// <param name="locale">Get your key at graphhopper.com</param>
        /// <param name="instructions">The locale of the resulting turn instructions. E.g. &#x60;pt_PT&#x60; for Portuguese or &#x60;de&#x60; for German (optional)</param>
        /// <param name="vehicle">If instruction should be calculated and returned (optional)</param>
        /// <param name="elevation">The vehicle for which the route should be calculated. Other vehicles are foot, small_truck, ... (optional)</param>
        /// <param name="calcPoints">If &#x60;true&#x60; a third dimension - the elevation - is included in the polyline or in the GeoJson. If enabled you have to use a modified version of the decoding method or set points_encoded to &#x60;false&#x60;. See the points_encoded attribute for more details. Additionally a request can fail if the vehicle does not support elevation. See the features object for every vehicle. (optional)</param>
        /// <param name="pointHint">If the points for the route should be calculated at all printing out only distance and time. (optional)</param>
        /// <param name="chDisable">Optional parameter. Specifies a hint for each &#x60;point&#x60; parameter to prefer a certain street for the closest location lookup. E.g. if there is an address or house with two or more neighboring streets you can control for which street the closest location is looked up. (optional)</param>
        /// <param name="weighting">Use this parameter in combination with one or more parameters of this table (optional)</param>
        /// <param name="edgeTraversal">Which kind of &#39;best&#39; route calculation you need. Other option is &#x60;shortest&#x60; (e.g. for &#x60;vehicle&#x3D;foot&#x60; or &#x60;bike&#x60;), &#x60;short_fastest&#x60; if time and distance is expensive e.g. for &#x60;vehicle&#x3D;truck&#x60; (optional)</param>
        /// <param name="algorithm">Use &#x60;true&#x60; if you want to consider turn restrictions for bike and motor vehicles. Keep in mind that the response time is roughly 2 times slower. (optional)</param>
        /// <param name="heading">The algorithm to calculate the route. Other options are &#x60;dijkstra&#x60;, &#x60;astar&#x60;, &#x60;astarbi&#x60;, &#x60;alternative_route&#x60; and &#x60;round_trip&#x60; (optional)</param>
        /// <param name="headingPenalty">Favour a heading direction for a certain point. Specify either one heading for the start point or as many as there are points. In this case headings are associated by their order to the specific points. Headings are given as north based clockwise angle between 0 and 360 degree. This parameter also influences the tour generated with &#x60;algorithm&#x3D;round_trip&#x60; and force the initial direction. (optional)</param>
        /// <param name="passThrough">Penalty for omitting a specified heading. The penalty corresponds to the accepted time delay in seconds in comparison to the route without a heading. (optional)</param>
        /// <param name="roundTripDistance">If &#x60;true&#x60; u-turns are avoided at via-points with regard to the &#x60;heading_penalty&#x60;. (optional)</param>
        /// <param name="roundTripSeed">If &#x60;algorithm&#x3D;round_trip&#x60; this parameter configures approximative length of the resulting round trip (optional)</param>
        /// <param name="alternativeRouteMaxPaths">If &#x60;algorithm&#x3D;round_trip&#x60; this parameter introduces randomness if e.g. the first try wasn&#39;t good. (optional)</param>
        /// <param name="alternativeRouteMaxWeightFactor">If &#x60;algorithm&#x3D;alternative_route&#x60; this parameter sets the number of maximum paths which should be calculated. Increasing can lead to worse alternatives. (optional)</param>
        /// <param name="alternativeRouteMaxShareFactor">If &#x60;algorithm&#x3D;alternative_route&#x60; this parameter sets the factor by which the alternatives routes can be longer than the optimal route. Increasing can lead to worse alternatives. (optional)</param>
        /// <param name="avoid">If &#x60;algorithm&#x3D;alternative_route&#x60; this parameter specifies how much alternatives routes can have maximum in common with the optimal route. Increasing can lead to worse alternatives. (optional)</param>
        /// <param name="avoid">comma separate list to avoid certain roads. You can avoid motorway, ferry, tunnel or track (optional)</param>
        /// <returns>Task of ApiResponse (RouteResponse)</returns>
        System.Threading.Tasks.Task<ApiResponse<RouteResponse>> RouteGetAsyncWithHttpInfo(List<string> point, bool? pointsEncoded, string key, string locale = null, bool? instructions = null, string vehicle = null, bool? elevation = null, bool? calcPoints = null, List<string> pointHint = null, bool? chDisable = null, string weighting = null, bool? edgeTraversal = null, string algorithm = null, int? heading = null, int? headingPenalty = null, bool? passThrough = null, int? roundTripDistance = null, long? roundTripSeed = null, int? alternativeRouteMaxPaths = null, int? alternativeRouteMaxWeightFactor = null, int? alternativeRouteMaxShareFactor = null, string avoid = null);
        #endregion Asynchronous Operations
    }
}