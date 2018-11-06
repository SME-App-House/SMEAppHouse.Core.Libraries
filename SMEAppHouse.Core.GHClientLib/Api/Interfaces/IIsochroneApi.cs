using SMEAppHouse.Core.GHClientLib.Model;
using SMEAppHouse.Core.RestSharpClientLib;

namespace SMEAppHouse.Core.GHClientLib.Api.Interfaces
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IIsochroneApi : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// Isochrone Request
        /// </summary>
        /// <remarks>
        /// The GraphHopper Isochrone API allows calculating an isochrone of a locations means to calculate &#39;a line connecting points at which a vehicle arrives at the same time,&#39; see [Wikipedia](http://en.wikipedia.org/wiki/Isochrone_map). It is also called **reachability** or **walkability**. 
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="key">Specify the start coordinate</param>
        /// <param name="timeLimit">Get your key at graphhopper.com</param>
        /// <param name="distanceLimit">Specify which time the vehicle should travel. In seconds. (optional, default to 600)</param>
        /// <param name="vehicle">Specify which distance the vehicle should travel. In meter. (optional, default to -1)</param>
        /// <param name="buckets">Possible vehicles are bike, car, foot and [more](https://graphhopper.com/api/1/docs/supported-vehicle-profiles/) (optional, default to car)</param>
        /// <param name="reverseFlow">For how many sub intervals an additional polygon should be calculated. (optional, default to 1)</param>
        /// <param name="reverseFlow">If &#x60;false&#x60; the flow goes from point to the polygon, if &#x60;true&#x60; the flow goes from the polygon \&quot;inside\&quot; to the point. Example usage for &#x60;false&#x60;&amp;#58; *How many potential customer can be reached within 30min travel time from your store* vs. &#x60;true&#x60;&amp;#58; *How many customers can reach your store within 30min travel time.* (optional, default to false)</param>
        /// <returns>IsochroneResponse</returns>
        IsochroneResponse IsochroneGet (string point, string key, int? timeLimit = null, int? distanceLimit = null, string vehicle = null, int? buckets = null, bool? reverseFlow = null);

        /// <summary>
        /// Isochrone Request
        /// </summary>
        /// <remarks>
        /// The GraphHopper Isochrone API allows calculating an isochrone of a locations means to calculate &#39;a line connecting points at which a vehicle arrives at the same time,&#39; see [Wikipedia](http://en.wikipedia.org/wiki/Isochrone_map). It is also called **reachability** or **walkability**. 
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="key">Specify the start coordinate</param>
        /// <param name="timeLimit">Get your key at graphhopper.com</param>
        /// <param name="distanceLimit">Specify which time the vehicle should travel. In seconds. (optional, default to 600)</param>
        /// <param name="vehicle">Specify which distance the vehicle should travel. In meter. (optional, default to -1)</param>
        /// <param name="buckets">Possible vehicles are bike, car, foot and [more](https://graphhopper.com/api/1/docs/supported-vehicle-profiles/) (optional, default to car)</param>
        /// <param name="reverseFlow">For how many sub intervals an additional polygon should be calculated. (optional, default to 1)</param>
        /// <param name="reverseFlow">If &#x60;false&#x60; the flow goes from point to the polygon, if &#x60;true&#x60; the flow goes from the polygon \&quot;inside\&quot; to the point. Example usage for &#x60;false&#x60;&amp;#58; *How many potential customer can be reached within 30min travel time from your store* vs. &#x60;true&#x60;&amp;#58; *How many customers can reach your store within 30min travel time.* (optional, default to false)</param>
        /// <returns>ApiResponse of IsochroneResponse</returns>
        ApiResponse<IsochroneResponse> IsochroneGetWithHttpInfo (string point, string key, int? timeLimit = null, int? distanceLimit = null, string vehicle = null, int? buckets = null, bool? reverseFlow = null);
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Isochrone Request
        /// </summary>
        /// <remarks>
        /// The GraphHopper Isochrone API allows calculating an isochrone of a locations means to calculate &#39;a line connecting points at which a vehicle arrives at the same time,&#39; see [Wikipedia](http://en.wikipedia.org/wiki/Isochrone_map). It is also called **reachability** or **walkability**. 
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="key">Specify the start coordinate</param>
        /// <param name="timeLimit">Get your key at graphhopper.com</param>
        /// <param name="distanceLimit">Specify which time the vehicle should travel. In seconds. (optional, default to 600)</param>
        /// <param name="vehicle">Specify which distance the vehicle should travel. In meter. (optional, default to -1)</param>
        /// <param name="buckets">Possible vehicles are bike, car, foot and [more](https://graphhopper.com/api/1/docs/supported-vehicle-profiles/) (optional, default to car)</param>
        /// <param name="reverseFlow">For how many sub intervals an additional polygon should be calculated. (optional, default to 1)</param>
        /// <param name="reverseFlow">If &#x60;false&#x60; the flow goes from point to the polygon, if &#x60;true&#x60; the flow goes from the polygon \&quot;inside\&quot; to the point. Example usage for &#x60;false&#x60;&amp;#58; *How many potential customer can be reached within 30min travel time from your store* vs. &#x60;true&#x60;&amp;#58; *How many customers can reach your store within 30min travel time.* (optional, default to false)</param>
        /// <returns>Task of IsochroneResponse</returns>
        System.Threading.Tasks.Task<IsochroneResponse> IsochroneGetAsync (string point, string key, int? timeLimit = null, int? distanceLimit = null, string vehicle = null, int? buckets = null, bool? reverseFlow = null);

        /// <summary>
        /// Isochrone Request
        /// </summary>
        /// <remarks>
        /// The GraphHopper Isochrone API allows calculating an isochrone of a locations means to calculate &#39;a line connecting points at which a vehicle arrives at the same time,&#39; see [Wikipedia](http://en.wikipedia.org/wiki/Isochrone_map). It is also called **reachability** or **walkability**. 
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="key">Specify the start coordinate</param>
        /// <param name="timeLimit">Get your key at graphhopper.com</param>
        /// <param name="distanceLimit">Specify which time the vehicle should travel. In seconds. (optional, default to 600)</param>
        /// <param name="vehicle">Specify which distance the vehicle should travel. In meter. (optional, default to -1)</param>
        /// <param name="buckets">Possible vehicles are bike, car, foot and [more](https://graphhopper.com/api/1/docs/supported-vehicle-profiles/) (optional, default to car)</param>
        /// <param name="reverseFlow">For how many sub intervals an additional polygon should be calculated. (optional, default to 1)</param>
        /// <param name="reverseFlow">If &#x60;false&#x60; the flow goes from point to the polygon, if &#x60;true&#x60; the flow goes from the polygon \&quot;inside\&quot; to the point. Example usage for &#x60;false&#x60;&amp;#58; *How many potential customer can be reached within 30min travel time from your store* vs. &#x60;true&#x60;&amp;#58; *How many customers can reach your store within 30min travel time.* (optional, default to false)</param>
        /// <returns>Task of ApiResponse (IsochroneResponse)</returns>
        System.Threading.Tasks.Task<ApiResponse<IsochroneResponse>> IsochroneGetAsyncWithHttpInfo (string point, string key, int? timeLimit = null, int? distanceLimit = null, string vehicle = null, int? buckets = null, bool? reverseFlow = null);
        #endregion Asynchronous Operations
    }
}