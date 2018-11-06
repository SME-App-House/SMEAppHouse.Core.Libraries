using System.Collections.Generic;
using SMEAppHouse.Core.GHClientLib.Model;
using SMEAppHouse.Core.RestSharpClientLib;

namespace SMEAppHouse.Core.GHClientLib.Api.Interfaces
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IMatrixApi : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// Matrix API
        /// </summary>
        /// <remarks>
        /// The Matrix API is part of the GraphHopper Directions API and with this API you can calculate many-to-many distances, times or routes a lot more efficient than calling the Routing API multiple times. In the Routing API we support multiple points, so called &#39;via points&#39;, which results in one route being calculated. The Matrix API results in NxM routes or more precise NxM weights, distances or times being calculated but is a lot faster compared to NxM single requests. The most simple example is a tourist trying to decide which pizza is close to him instead of using beeline distance she can calculate a 1x4 matrix. Or a delivery service in the need of often big NxN matrices to solve vehicle routing problems. E.g. the GraphHopper Route Optimization API uses the Matrix API under the hood to achieve this. 
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="point">Get your key at graphhopper.com</param>
        /// <param name="fromPoint">Specifiy multiple points for which the weight-, route-, time- or distance-matrix should be calculated. In this case the starts are identical to the destinations. If there are N points, then NxN entries will be calculated. The order of the point parameter is important. Specify at least three points. Cannot be used together with from_point or to_point. Is a string with the format latitude,longitude. (optional)</param>
        /// <param name="toPoint">The starting points for the routes. E.g. if you want to calculate the three routes A-&amp;gt;1, A-&amp;gt;2, A-&amp;gt;3 then you have one from_point parameter and three to_point parameters. Is a string with the format latitude,longitude. (optional)</param>
        /// <param name="outArray">The destination points for the routes. Is a string with the format latitude,longitude. (optional)</param>
        /// <param name="vehicle">Specifies which arrays should be included in the response. Specify one or more of the following options &#39;weights&#39;, &#39;times&#39;, &#39;distances&#39;. To specify more than one array use e.g. out_array&#x3D;times&amp;out_array&#x3D;distances. The units of the entries of distances are meters, of times are seconds and of weights is arbitrary and it can differ for different vehicles or versions of this API. (optional)</param>
        /// <param name="vehicle">The vehicle for which the route should be calculated. Other vehicles are foot, small_truck etc (optional, default to car)</param>
        /// <returns>MatrixResponse</returns>
        MatrixResponse MatrixGet (string key, List<string> point = null, List<string> fromPoint = null, List<string> toPoint = null, List<string> outArray = null, string vehicle = null);

        /// <summary>
        /// Matrix API
        /// </summary>
        /// <remarks>
        /// The Matrix API is part of the GraphHopper Directions API and with this API you can calculate many-to-many distances, times or routes a lot more efficient than calling the Routing API multiple times. In the Routing API we support multiple points, so called &#39;via points&#39;, which results in one route being calculated. The Matrix API results in NxM routes or more precise NxM weights, distances or times being calculated but is a lot faster compared to NxM single requests. The most simple example is a tourist trying to decide which pizza is close to him instead of using beeline distance she can calculate a 1x4 matrix. Or a delivery service in the need of often big NxN matrices to solve vehicle routing problems. E.g. the GraphHopper Route Optimization API uses the Matrix API under the hood to achieve this. 
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="point">Get your key at graphhopper.com</param>
        /// <param name="fromPoint">Specifiy multiple points for which the weight-, route-, time- or distance-matrix should be calculated. In this case the starts are identical to the destinations. If there are N points, then NxN entries will be calculated. The order of the point parameter is important. Specify at least three points. Cannot be used together with from_point or to_point. Is a string with the format latitude,longitude. (optional)</param>
        /// <param name="toPoint">The starting points for the routes. E.g. if you want to calculate the three routes A-&amp;gt;1, A-&amp;gt;2, A-&amp;gt;3 then you have one from_point parameter and three to_point parameters. Is a string with the format latitude,longitude. (optional)</param>
        /// <param name="outArray">The destination points for the routes. Is a string with the format latitude,longitude. (optional)</param>
        /// <param name="vehicle">Specifies which arrays should be included in the response. Specify one or more of the following options &#39;weights&#39;, &#39;times&#39;, &#39;distances&#39;. To specify more than one array use e.g. out_array&#x3D;times&amp;out_array&#x3D;distances. The units of the entries of distances are meters, of times are seconds and of weights is arbitrary and it can differ for different vehicles or versions of this API. (optional)</param>
        /// <param name="vehicle">The vehicle for which the route should be calculated. Other vehicles are foot, small_truck etc (optional, default to car)</param>
        /// <returns>ApiResponse of MatrixResponse</returns>
        ApiResponse<MatrixResponse> MatrixGetWithHttpInfo (string key, List<string> point = null, List<string> fromPoint = null, List<string> toPoint = null, List<string> outArray = null, string vehicle = null);
        /// <summary>
        /// Matrix API Post
        /// </summary>
        /// <remarks>
        /// The GET request has an URL length limitation, which hurts for many locations per request. In those cases use a HTTP POST request with JSON data as input. The only parameter in the URL will be the key which stays in the URL. Both request scenarios are identically except that all singular parameter names are named as their plural for a POST request. 
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body">Get your key at graphhopper.com</param>
        /// <param name="body"> (optional)</param>
        /// <returns>MatrixResponse</returns>
        MatrixResponse MatrixPost (string key, MatrixRequest body = null);

        /// <summary>
        /// Matrix API Post
        /// </summary>
        /// <remarks>
        /// The GET request has an URL length limitation, which hurts for many locations per request. In those cases use a HTTP POST request with JSON data as input. The only parameter in the URL will be the key which stays in the URL. Both request scenarios are identically except that all singular parameter names are named as their plural for a POST request. 
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body">Get your key at graphhopper.com</param>
        /// <param name="body"> (optional)</param>
        /// <returns>ApiResponse of MatrixResponse</returns>
        ApiResponse<MatrixResponse> MatrixPostWithHttpInfo (string key, MatrixRequest body = null);
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Matrix API
        /// </summary>
        /// <remarks>
        /// The Matrix API is part of the GraphHopper Directions API and with this API you can calculate many-to-many distances, times or routes a lot more efficient than calling the Routing API multiple times. In the Routing API we support multiple points, so called &#39;via points&#39;, which results in one route being calculated. The Matrix API results in NxM routes or more precise NxM weights, distances or times being calculated but is a lot faster compared to NxM single requests. The most simple example is a tourist trying to decide which pizza is close to him instead of using beeline distance she can calculate a 1x4 matrix. Or a delivery service in the need of often big NxN matrices to solve vehicle routing problems. E.g. the GraphHopper Route Optimization API uses the Matrix API under the hood to achieve this. 
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="point">Get your key at graphhopper.com</param>
        /// <param name="fromPoint">Specifiy multiple points for which the weight-, route-, time- or distance-matrix should be calculated. In this case the starts are identical to the destinations. If there are N points, then NxN entries will be calculated. The order of the point parameter is important. Specify at least three points. Cannot be used together with from_point or to_point. Is a string with the format latitude,longitude. (optional)</param>
        /// <param name="toPoint">The starting points for the routes. E.g. if you want to calculate the three routes A-&amp;gt;1, A-&amp;gt;2, A-&amp;gt;3 then you have one from_point parameter and three to_point parameters. Is a string with the format latitude,longitude. (optional)</param>
        /// <param name="outArray">The destination points for the routes. Is a string with the format latitude,longitude. (optional)</param>
        /// <param name="vehicle">Specifies which arrays should be included in the response. Specify one or more of the following options &#39;weights&#39;, &#39;times&#39;, &#39;distances&#39;. To specify more than one array use e.g. out_array&#x3D;times&amp;out_array&#x3D;distances. The units of the entries of distances are meters, of times are seconds and of weights is arbitrary and it can differ for different vehicles or versions of this API. (optional)</param>
        /// <param name="vehicle">The vehicle for which the route should be calculated. Other vehicles are foot, small_truck etc (optional, default to car)</param>
        /// <returns>Task of MatrixResponse</returns>
        System.Threading.Tasks.Task<MatrixResponse> MatrixGetAsync (string key, List<string> point = null, List<string> fromPoint = null, List<string> toPoint = null, List<string> outArray = null, string vehicle = null);

        /// <summary>
        /// Matrix API
        /// </summary>
        /// <remarks>
        /// The Matrix API is part of the GraphHopper Directions API and with this API you can calculate many-to-many distances, times or routes a lot more efficient than calling the Routing API multiple times. In the Routing API we support multiple points, so called &#39;via points&#39;, which results in one route being calculated. The Matrix API results in NxM routes or more precise NxM weights, distances or times being calculated but is a lot faster compared to NxM single requests. The most simple example is a tourist trying to decide which pizza is close to him instead of using beeline distance she can calculate a 1x4 matrix. Or a delivery service in the need of often big NxN matrices to solve vehicle routing problems. E.g. the GraphHopper Route Optimization API uses the Matrix API under the hood to achieve this. 
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="point">Get your key at graphhopper.com</param>
        /// <param name="fromPoint">Specifiy multiple points for which the weight-, route-, time- or distance-matrix should be calculated. In this case the starts are identical to the destinations. If there are N points, then NxN entries will be calculated. The order of the point parameter is important. Specify at least three points. Cannot be used together with from_point or to_point. Is a string with the format latitude,longitude. (optional)</param>
        /// <param name="toPoint">The starting points for the routes. E.g. if you want to calculate the three routes A-&amp;gt;1, A-&amp;gt;2, A-&amp;gt;3 then you have one from_point parameter and three to_point parameters. Is a string with the format latitude,longitude. (optional)</param>
        /// <param name="outArray">The destination points for the routes. Is a string with the format latitude,longitude. (optional)</param>
        /// <param name="vehicle">Specifies which arrays should be included in the response. Specify one or more of the following options &#39;weights&#39;, &#39;times&#39;, &#39;distances&#39;. To specify more than one array use e.g. out_array&#x3D;times&amp;out_array&#x3D;distances. The units of the entries of distances are meters, of times are seconds and of weights is arbitrary and it can differ for different vehicles or versions of this API. (optional)</param>
        /// <param name="vehicle">The vehicle for which the route should be calculated. Other vehicles are foot, small_truck etc (optional, default to car)</param>
        /// <returns>Task of ApiResponse (MatrixResponse)</returns>
        System.Threading.Tasks.Task<ApiResponse<MatrixResponse>> MatrixGetAsyncWithHttpInfo (string key, List<string> point = null, List<string> fromPoint = null, List<string> toPoint = null, List<string> outArray = null, string vehicle = null);
        /// <summary>
        /// Matrix API Post
        /// </summary>
        /// <remarks>
        /// The GET request has an URL length limitation, which hurts for many locations per request. In those cases use a HTTP POST request with JSON data as input. The only parameter in the URL will be the key which stays in the URL. Both request scenarios are identically except that all singular parameter names are named as their plural for a POST request. 
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body">Get your key at graphhopper.com</param>
        /// <param name="body"> (optional)</param>
        /// <returns>Task of MatrixResponse</returns>
        System.Threading.Tasks.Task<MatrixResponse> MatrixPostAsync (string key, MatrixRequest body = null);

        /// <summary>
        /// Matrix API Post
        /// </summary>
        /// <remarks>
        /// The GET request has an URL length limitation, which hurts for many locations per request. In those cases use a HTTP POST request with JSON data as input. The only parameter in the URL will be the key which stays in the URL. Both request scenarios are identically except that all singular parameter names are named as their plural for a POST request. 
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body">Get your key at graphhopper.com</param>
        /// <param name="body"> (optional)</param>
        /// <returns>Task of ApiResponse (MatrixResponse)</returns>
        System.Threading.Tasks.Task<ApiResponse<MatrixResponse>> MatrixPostAsyncWithHttpInfo (string key, MatrixRequest body = null);
        #endregion Asynchronous Operations
    }
}