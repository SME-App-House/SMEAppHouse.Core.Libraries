using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMED.Core.Patterns.Entities;

// ReSharper disable InheritdocConsiderUsage

namespace SMED.Core.WebAPI.Patterns.APIHostPattern
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class HttpActionResultWrapper<TEntity> : IActionResult
        where TEntity : IEntity
    {
        private readonly TEntity _value;
        readonly HttpRequestMessage _request;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="request"></param>
        public HttpActionResultWrapper(TEntity value, HttpRequestMessage request)
        {
            _value = value;
            _request = request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var json = JsonConvert.SerializeObject(_value);
            var response = new HttpResponseMessage()
            {
                Content = new StringContent(json),
                RequestMessage = _request
            };
            return Task.FromResult(response);
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}