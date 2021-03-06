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
    public partial class SolutionApi : ISolutionApi
    {
        private ExceptionFactory _exceptionFactory = (name, response) => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionApi"/> class.
        /// </summary>
        /// <returns></returns>
        public SolutionApi(string basePath)
        {
            Configuration = new RestSharpClientLib.Configuration { BasePath = basePath };

            ExceptionFactory = RestSharpClientLib.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public SolutionApi(RestSharpClientLib.Configuration configuration = null)
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
            set => _exceptionFactory = value;
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
        /// Return the solution associated to the jobId This endpoint returns the solution of a large problems. You can fetch it with the job_id, you have been sent. 
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="key">your API key</param>
        /// <param name="jobId">Request solution with jobId</param>
        /// <returns>Response</returns>
        public Response GetSolution (string key, string jobId)
        {
             var localVarResponse = GetSolutionWithHttpInfo(key, jobId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Return the solution associated to the jobId This endpoint returns the solution of a large problems. You can fetch it with the job_id, you have been sent. 
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="key">your API key</param>
        /// <param name="jobId">Request solution with jobId</param>
        /// <returns>ApiResponse of Response</returns>
        public ApiResponse< Response > GetSolutionWithHttpInfo (string key, string jobId)
        {
            // verify the required parameter 'key' is set
            if (key == null)
                throw new ApiException(400, "Missing required parameter 'key' when calling SolutionApi->GetSolution");
            // verify the required parameter 'jobId' is set
            if (jobId == null)
                throw new ApiException(400, "Missing required parameter 'jobId' when calling SolutionApi->GetSolution");

            var localVarPath = "/vrp/solution/{jobId}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new List<KeyValuePair<string, string>>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            var localVarHttpContentTypes = new string[] {
                "application/json"
            };
            var localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            var localVarHttpHeaderAccepts = new string[] {
                "application/json"
            };
            var localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (jobId != null) localVarPathParams.Add("jobId", Configuration.ApiClient.ParameterToString(jobId)); // path parameter
            if (key != null) localVarQueryParams.AddRange(Configuration.ApiClient.ParameterToKeyValuePairs("", "key", key)); // query parameter


            // make the HTTP request
            var localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            var localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                var exception = ExceptionFactory("GetSolution", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Response>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (Response) Configuration.ApiClient.Deserialize(localVarResponse, typeof(Response)));
        }

        /// <summary>
        /// Return the solution associated to the jobId This endpoint returns the solution of a large problems. You can fetch it with the job_id, you have been sent. 
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="key">your API key</param>
        /// <param name="jobId">Request solution with jobId</param>
        /// <returns>Task of Response</returns>
        public async System.Threading.Tasks.Task<Response> GetSolutionAsync (string key, string jobId)
        {
             var localVarResponse = await GetSolutionAsyncWithHttpInfo(key, jobId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Return the solution associated to the jobId This endpoint returns the solution of a large problems. You can fetch it with the job_id, you have been sent. 
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="key">your API key</param>
        /// <param name="jobId">Request solution with jobId</param>
        /// <returns>Task of ApiResponse (Response)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Response>> GetSolutionAsyncWithHttpInfo (string key, string jobId)
        {
            // verify the required parameter 'key' is set
            if (key == null)
                throw new ApiException(400, "Missing required parameter 'key' when calling SolutionApi->GetSolution");
            // verify the required parameter 'jobId' is set
            if (jobId == null)
                throw new ApiException(400, "Missing required parameter 'jobId' when calling SolutionApi->GetSolution");

            var localVarPath = "/vrp/solution/{jobId}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new List<KeyValuePair<string, string>>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            var localVarHttpContentTypes = new string[] {
                "application/json"
            };
            var localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            var localVarHttpHeaderAccepts = new string[] {
                "application/json"
            };
            var localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (jobId != null) localVarPathParams.Add("jobId", Configuration.ApiClient.ParameterToString(jobId)); // path parameter
            if (key != null) localVarQueryParams.AddRange(Configuration.ApiClient.ParameterToKeyValuePairs("", "key", key)); // query parameter


            // make the HTTP request
            var localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            var localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                var exception = ExceptionFactory("GetSolution", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<Response>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (Response) Configuration.ApiClient.Deserialize(localVarResponse, typeof(Response)));
        }

    }
}
