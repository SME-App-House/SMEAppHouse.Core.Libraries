/* 
 * GraphHopper Directions API
 *
 * You use the GraphHopper Directions API to add route planning, navigation and route optimization to your software. E.g. the Routing API has turn instructions and elevation data and the Route Optimization API solves your logistic problems and supports various constraints like time window and capacity restrictions. Also it is possible to get all distances between all locations with our fast Matrix API.
 *
 * OpenAPI spec version: 1.0.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RestSharp;
using SMEAppHouse.Core.GHClientLib.Api.Interfaces;
using SMEAppHouse.Core.GHClientLib.Model;
using SMEAppHouse.Core.RestSharpClientLib;

namespace SMEAppHouse.Core.GHClientLib.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public partial class GeocodingApi : IGeocodingApi
    {
        private ExceptionFactory _exceptionFactory = (name, response) => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeocodingApi"/> class.
        /// </summary>
        /// <returns></returns>
        public GeocodingApi(string basePath)
        {
            Configuration = new RestSharpClientLib.Configuration { BasePath = basePath };

            ExceptionFactory = RestSharpClientLib.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeocodingApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public GeocodingApi(RestSharpClientLib.Configuration configuration = null)
        {
            if (configuration == null) // use the default one in Configuration
                Configuration = RestSharpClientLib.Configuration.Default;
            else
                Configuration = configuration;

            ExceptionFactory = RestSharpClientLib.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        public string GetBasePath()
        {
            return Configuration.ApiClient.RestClient.BaseUrl.ToString();
        }

        /// <summary>
        /// Sets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        [Obsolete("SetBasePath is deprecated, please do 'Configuration.ApiClient = new ApiClient(\"http://new-path\")' instead.")]
        public void SetBasePath(string basePath)
        {
            // do nothing
        }

        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        /// <value>An instance of the Configuration</value>
        public RestSharpClientLib.Configuration Configuration {get; set;}

        /// <summary>
        /// Provides a factory method hook for the creation of exceptions.
        /// </summary>
        public ExceptionFactory ExceptionFactory
        {
            get
            {
                if (_exceptionFactory != null && _exceptionFactory.GetInvocationList().Length > 1)
                {
                    throw new InvalidOperationException("Multicast delegate for ExceptionFactory is unsupported.");
                }
                return _exceptionFactory;
            }
            set { _exceptionFactory = value; }
        }

        /// <summary>
        /// Gets the default header.
        /// </summary>
        /// <returns>Dictionary of HTTP header</returns>
        [Obsolete("DefaultHeader is deprecated, please use Configuration.DefaultHeader instead.")]
        public IDictionary<string, string> DefaultHeader()
        {
            return new ReadOnlyDictionary<string, string>(Configuration.DefaultHeader);
        }

        /// <summary>
        /// Add default header.
        /// </summary>
        /// <param name="key">Header field name.</param>
        /// <param name="value">Header field value.</param>
        /// <returns></returns>
        [Obsolete("AddDefaultHeader is deprecated, please use Configuration.AddDefaultHeader instead.")]
        public void AddDefaultHeader(string key, string value)
        {
            Configuration.AddDefaultHeader(key, value);
        }

        /// <summary>
        /// Execute a Geocoding request This endpoint provides forward and reverse geocoding. For more details, review the official documentation at: https://graphhopper.com/api/1/docs/geocoding/ 
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="key">Get your key at graphhopper.com</param>
        /// <param name="q">If you do forward geocoding, then this would be a textual description of the address you are looking for (optional)</param>
        /// <param name="locale">Display the search results for the specified locale. Currently French (fr), English (en), German (de) and Italian (it) are supported. If the locale wasn&#39;t found the default (en) is used. (optional)</param>
        /// <param name="limit">Specify the maximum number of returned results (optional)</param>
        /// <param name="reverse">Set to true to do a reverse Geocoding request, see point parameter (optional)</param>
        /// <param name="point">The location bias in the format &#39;latitude,longitude&#39; e.g. point&#x3D;45.93272,11.58803 (optional)</param>
        /// <param name="provider">Can be either, default, nominatim, opencagedata (optional)</param>
        /// <returns>GeocodingResponse</returns>
        public GeocodingResponse GeocodeGet (string key, string q = null, string locale = null, int? limit = null, bool? reverse = null, string point = null, string provider = null)
        {
             ApiResponse<GeocodingResponse> localVarResponse = GeocodeGetWithHttpInfo(key, q, locale, limit, reverse, point, provider);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Execute a Geocoding request This endpoint provides forward and reverse geocoding. For more details, review the official documentation at: https://graphhopper.com/api/1/docs/geocoding/ 
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="key">Get your key at graphhopper.com</param>
        /// <param name="q">If you do forward geocoding, then this would be a textual description of the address you are looking for (optional)</param>
        /// <param name="locale">Display the search results for the specified locale. Currently French (fr), English (en), German (de) and Italian (it) are supported. If the locale wasn&#39;t found the default (en) is used. (optional)</param>
        /// <param name="limit">Specify the maximum number of returned results (optional)</param>
        /// <param name="reverse">Set to true to do a reverse Geocoding request, see point parameter (optional)</param>
        /// <param name="point">The location bias in the format &#39;latitude,longitude&#39; e.g. point&#x3D;45.93272,11.58803 (optional)</param>
        /// <param name="provider">Can be either, default, nominatim, opencagedata (optional)</param>
        /// <returns>ApiResponse of GeocodingResponse</returns>
        public ApiResponse< GeocodingResponse > GeocodeGetWithHttpInfo (string key, string q = null, string locale = null, int? limit = null, bool? reverse = null, string point = null, string provider = null)
        {
            // verify the required parameter 'key' is set
            if (key == null)
                throw new ApiException(400, "Missing required parameter 'key' when calling GeocodingApi->GeocodeGet");

            var localVarPath = "/geocode";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new List<KeyValuePair<string, string>>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json"
            };
            string localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (q != null) localVarQueryParams.AddRange(Configuration.ApiClient.ParameterToKeyValuePairs("", "q", q)); // query parameter
            if (locale != null) localVarQueryParams.AddRange(Configuration.ApiClient.ParameterToKeyValuePairs("", "locale", locale)); // query parameter
            if (limit != null) localVarQueryParams.AddRange(Configuration.ApiClient.ParameterToKeyValuePairs("", "limit", limit)); // query parameter
            if (reverse != null) localVarQueryParams.AddRange(Configuration.ApiClient.ParameterToKeyValuePairs("", "reverse", reverse)); // query parameter
            if (point != null) localVarQueryParams.AddRange(Configuration.ApiClient.ParameterToKeyValuePairs("", "point", point)); // query parameter
            if (provider != null) localVarQueryParams.AddRange(Configuration.ApiClient.ParameterToKeyValuePairs("", "provider", provider)); // query parameter
            if (key != null) localVarQueryParams.AddRange(Configuration.ApiClient.ParameterToKeyValuePairs("", "key", key)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("GeocodeGet", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<GeocodingResponse>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (GeocodingResponse) Configuration.ApiClient.Deserialize(localVarResponse, typeof(GeocodingResponse)));
        }

        /// <summary>
        /// Execute a Geocoding request This endpoint provides forward and reverse geocoding. For more details, review the official documentation at: https://graphhopper.com/api/1/docs/geocoding/ 
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="key">Get your key at graphhopper.com</param>
        /// <param name="q">If you do forward geocoding, then this would be a textual description of the address you are looking for (optional)</param>
        /// <param name="locale">Display the search results for the specified locale. Currently French (fr), English (en), German (de) and Italian (it) are supported. If the locale wasn&#39;t found the default (en) is used. (optional)</param>
        /// <param name="limit">Specify the maximum number of returned results (optional)</param>
        /// <param name="reverse">Set to true to do a reverse Geocoding request, see point parameter (optional)</param>
        /// <param name="point">The location bias in the format &#39;latitude,longitude&#39; e.g. point&#x3D;45.93272,11.58803 (optional)</param>
        /// <param name="provider">Can be either, default, nominatim, opencagedata (optional)</param>
        /// <returns>Task of GeocodingResponse</returns>
        public async System.Threading.Tasks.Task<GeocodingResponse> GeocodeGetAsync (string key, string q = null, string locale = null, int? limit = null, bool? reverse = null, string point = null, string provider = null)
        {
             ApiResponse<GeocodingResponse> localVarResponse = await GeocodeGetAsyncWithHttpInfo(key, q, locale, limit, reverse, point, provider);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Execute a Geocoding request This endpoint provides forward and reverse geocoding. For more details, review the official documentation at: https://graphhopper.com/api/1/docs/geocoding/ 
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="key">Get your key at graphhopper.com</param>
        /// <param name="q">If you do forward geocoding, then this would be a textual description of the address you are looking for (optional)</param>
        /// <param name="locale">Display the search results for the specified locale. Currently French (fr), English (en), German (de) and Italian (it) are supported. If the locale wasn&#39;t found the default (en) is used. (optional)</param>
        /// <param name="limit">Specify the maximum number of returned results (optional)</param>
        /// <param name="reverse">Set to true to do a reverse Geocoding request, see point parameter (optional)</param>
        /// <param name="point">The location bias in the format &#39;latitude,longitude&#39; e.g. point&#x3D;45.93272,11.58803 (optional)</param>
        /// <param name="provider">Can be either, default, nominatim, opencagedata (optional)</param>
        /// <returns>Task of ApiResponse (GeocodingResponse)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<GeocodingResponse>> GeocodeGetAsyncWithHttpInfo (string key, string q = null, string locale = null, int? limit = null, bool? reverse = null, string point = null, string provider = null)
        {
            // verify the required parameter 'key' is set
            if (key == null)
                throw new ApiException(400, "Missing required parameter 'key' when calling GeocodingApi->GeocodeGet");

            var localVarPath = "/geocode";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new List<KeyValuePair<string, string>>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json"
            };
            string localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (q != null) localVarQueryParams.AddRange(Configuration.ApiClient.ParameterToKeyValuePairs("", "q", q)); // query parameter
            if (locale != null) localVarQueryParams.AddRange(Configuration.ApiClient.ParameterToKeyValuePairs("", "locale", locale)); // query parameter
            if (limit != null) localVarQueryParams.AddRange(Configuration.ApiClient.ParameterToKeyValuePairs("", "limit", limit)); // query parameter
            if (reverse != null) localVarQueryParams.AddRange(Configuration.ApiClient.ParameterToKeyValuePairs("", "reverse", reverse)); // query parameter
            if (point != null) localVarQueryParams.AddRange(Configuration.ApiClient.ParameterToKeyValuePairs("", "point", point)); // query parameter
            if (provider != null) localVarQueryParams.AddRange(Configuration.ApiClient.ParameterToKeyValuePairs("", "provider", provider)); // query parameter
            if (key != null) localVarQueryParams.AddRange(Configuration.ApiClient.ParameterToKeyValuePairs("", "key", key)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("GeocodeGet", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<GeocodingResponse>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (GeocodingResponse) Configuration.ApiClient.Deserialize(localVarResponse, typeof(GeocodingResponse)));
        }

    }
}
