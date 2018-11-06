using SMEAppHouse.Core.GHClientLib.Model;
using SMEAppHouse.Core.RestSharpClientLib;

namespace SMEAppHouse.Core.GHClientLib.Api.Interfaces
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface ISolutionApi : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// Return the solution associated to the jobId
        /// </summary>
        /// <remarks>
        /// This endpoint returns the solution of a large problems. You can fetch it with the job_id, you have been sent. 
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="key">your API key</param>
        /// <param name="jobId">Request solution with jobId</param>
        /// <returns>Response</returns>
        Response GetSolution (string key, string jobId);

        /// <summary>
        /// Return the solution associated to the jobId
        /// </summary>
        /// <remarks>
        /// This endpoint returns the solution of a large problems. You can fetch it with the job_id, you have been sent. 
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="key">your API key</param>
        /// <param name="jobId">Request solution with jobId</param>
        /// <returns>ApiResponse of Response</returns>
        ApiResponse<Response> GetSolutionWithHttpInfo (string key, string jobId);
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Return the solution associated to the jobId
        /// </summary>
        /// <remarks>
        /// This endpoint returns the solution of a large problems. You can fetch it with the job_id, you have been sent. 
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="key">your API key</param>
        /// <param name="jobId">Request solution with jobId</param>
        /// <returns>Task of Response</returns>
        System.Threading.Tasks.Task<Response> GetSolutionAsync (string key, string jobId);

        /// <summary>
        /// Return the solution associated to the jobId
        /// </summary>
        /// <remarks>
        /// This endpoint returns the solution of a large problems. You can fetch it with the job_id, you have been sent. 
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="key">your API key</param>
        /// <param name="jobId">Request solution with jobId</param>
        /// <returns>Task of ApiResponse (Response)</returns>
        System.Threading.Tasks.Task<ApiResponse<Response>> GetSolutionAsyncWithHttpInfo (string key, string jobId);
        #endregion Asynchronous Operations
    }
}