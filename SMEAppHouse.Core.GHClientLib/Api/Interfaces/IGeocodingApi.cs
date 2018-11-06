using SMEAppHouse.Core.GHClientLib.Model;
using SMEAppHouse.Core.RestSharpClientLib;

namespace SMEAppHouse.Core.GHClientLib.Api.Interfaces
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IGeocodingApi : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// Execute a Geocoding request
        /// </summary>
        /// <remarks>
        /// This endpoint provides forward and reverse geocoding. For more details, review the official documentation at: https://graphhopper.com/api/1/docs/geocoding/ 
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="key">Get your key at graphhopper.com</param>
        /// <param name="q">If you do forward geocoding, then this would be a textual description of the address you are looking for (optional)</param>
        /// <param name="locale">Display the search results for the specified locale. Currently French (fr), English (en), German (de) and Italian (it) are supported. If the locale wasn&#39;t found the default (en) is used. (optional)</param>
        /// <param name="limit">Specify the maximum number of returned results (optional)</param>
        /// <param name="reverse">Set to true to do a reverse Geocoding request, see point parameter (optional)</param>
        /// <param name="point">The location bias in the format &#39;latitude,longitude&#39; e.g. point&#x3D;45.93272,11.58803 (optional)</param>
        /// <param name="provider">Can be either, default, nominatim, opencagedata (optional)</param>
        /// <returns>GeocodingResponse</returns>
        GeocodingResponse GeocodeGet (string key, string q = null, string locale = null, int? limit = null, bool? reverse = null, string point = null, string provider = null);

        /// <summary>
        /// Execute a Geocoding request
        /// </summary>
        /// <remarks>
        /// This endpoint provides forward and reverse geocoding. For more details, review the official documentation at: https://graphhopper.com/api/1/docs/geocoding/ 
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="key">Get your key at graphhopper.com</param>
        /// <param name="q">If you do forward geocoding, then this would be a textual description of the address you are looking for (optional)</param>
        /// <param name="locale">Display the search results for the specified locale. Currently French (fr), English (en), German (de) and Italian (it) are supported. If the locale wasn&#39;t found the default (en) is used. (optional)</param>
        /// <param name="limit">Specify the maximum number of returned results (optional)</param>
        /// <param name="reverse">Set to true to do a reverse Geocoding request, see point parameter (optional)</param>
        /// <param name="point">The location bias in the format &#39;latitude,longitude&#39; e.g. point&#x3D;45.93272,11.58803 (optional)</param>
        /// <param name="provider">Can be either, default, nominatim, opencagedata (optional)</param>
        /// <returns>ApiResponse of GeocodingResponse</returns>
        ApiResponse<GeocodingResponse> GeocodeGetWithHttpInfo (string key, string q = null, string locale = null, int? limit = null, bool? reverse = null, string point = null, string provider = null);
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Execute a Geocoding request
        /// </summary>
        /// <remarks>
        /// This endpoint provides forward and reverse geocoding. For more details, review the official documentation at: https://graphhopper.com/api/1/docs/geocoding/ 
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="key">Get your key at graphhopper.com</param>
        /// <param name="q">If you do forward geocoding, then this would be a textual description of the address you are looking for (optional)</param>
        /// <param name="locale">Display the search results for the specified locale. Currently French (fr), English (en), German (de) and Italian (it) are supported. If the locale wasn&#39;t found the default (en) is used. (optional)</param>
        /// <param name="limit">Specify the maximum number of returned results (optional)</param>
        /// <param name="reverse">Set to true to do a reverse Geocoding request, see point parameter (optional)</param>
        /// <param name="point">The location bias in the format &#39;latitude,longitude&#39; e.g. point&#x3D;45.93272,11.58803 (optional)</param>
        /// <param name="provider">Can be either, default, nominatim, opencagedata (optional)</param>
        /// <returns>Task of GeocodingResponse</returns>
        System.Threading.Tasks.Task<GeocodingResponse> GeocodeGetAsync (string key, string q = null, string locale = null, int? limit = null, bool? reverse = null, string point = null, string provider = null);

        /// <summary>
        /// Execute a Geocoding request
        /// </summary>
        /// <remarks>
        /// This endpoint provides forward and reverse geocoding. For more details, review the official documentation at: https://graphhopper.com/api/1/docs/geocoding/ 
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="key">Get your key at graphhopper.com</param>
        /// <param name="q">If you do forward geocoding, then this would be a textual description of the address you are looking for (optional)</param>
        /// <param name="locale">Display the search results for the specified locale. Currently French (fr), English (en), German (de) and Italian (it) are supported. If the locale wasn&#39;t found the default (en) is used. (optional)</param>
        /// <param name="limit">Specify the maximum number of returned results (optional)</param>
        /// <param name="reverse">Set to true to do a reverse Geocoding request, see point parameter (optional)</param>
        /// <param name="point">The location bias in the format &#39;latitude,longitude&#39; e.g. point&#x3D;45.93272,11.58803 (optional)</param>
        /// <param name="provider">Can be either, default, nominatim, opencagedata (optional)</param>
        /// <returns>Task of ApiResponse (GeocodingResponse)</returns>
        System.Threading.Tasks.Task<ApiResponse<GeocodingResponse>> GeocodeGetAsyncWithHttpInfo (string key, string q = null, string locale = null, int? limit = null, bool? reverse = null, string point = null, string provider = null);
        #endregion Asynchronous Operations
    }
}