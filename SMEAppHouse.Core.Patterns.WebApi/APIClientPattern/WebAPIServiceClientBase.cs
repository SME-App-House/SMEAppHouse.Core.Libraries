// ***********************************************************************
// Assembly         : SMEAppHouse.Core.Patterns.WebApi
// Author           : jcman
// Created          : 07-04-2018
//
// Last Modified By : jcman
// Last Modified On : 08-03-2018
// ***********************************************************************
// <copyright file="WebAPIServiceClientBase.cs" company="">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SMEAppHouse.Core.Patterns.EF.ModelComposite;

namespace SMEAppHouse.Core.Patterns.WebApi.APIClientPattern
{
    public abstract class WebApiServiceClientBase<TEntity, TIdType> : IWebAPIServiceClient<TEntity, TIdType>
        where TEntity : class, IEntity
    {
        public HttpClient HttpClient { get; private set; }

        public string BaseServiceAddress { get; private set; }

        /* Implementing "Template Method" design pattern*/

        protected string ServiceUrlForCreate => $"{ClientNameUrl}/Create";
        protected string ServiceUrlForUpdate => $"{ClientNameUrl}/Update";
        public string ServiceUrlForRemoveById => $"{ClientNameUrl}/RemoveById?id=[id]";
        public string ServiceUrlForRemoveAll => $"{ClientNameUrl}/RemoveAll";
        public string ServiceUrlForCount => $"{ClientNameUrl}/Count";
        public string ServiceUrlForGetById => $"{ClientNameUrl}/GetById?id=[id]";
        public string ServiceUrlForGetAll => $"{ClientNameUrl}/GetAll";
        public string ServiceUrlForGetAllWithEntities => $"{ClientNameUrl}/GetAll?entitiesToInclude=[entitiesToInclude]";


        #region constructors

        protected WebApiServiceClientBase(string baseSrvcAddress)
            : this(new HttpClient(), baseSrvcAddress)
        {
        }

        protected WebApiServiceClientBase(HttpClient client, string baseSrvcAddress)
        {
            HttpClient = client;
            BaseServiceAddress = baseSrvcAddress;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public string ClientNameUrl => $"api/v1/{GetType().Name.Replace("Client", "")}";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public TEntity Create(TEntity entity)
        {
            var url = $"{BaseServiceAddress}{ServiceUrlForCreate}";
            var http = new HttpClient();
            var json = JsonConvert.SerializeObject(entity);
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
            var task = http.SendAsync(request);

            task.ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    var err = t.Exception.Message;
                }
            }, TaskContinuationOptions.OnlyOnFaulted);

            return task.ContinueWith(innerTask =>
            {
                var response = innerTask.Result;
                json = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                    throw new System.Exception(json);

                var entityTarget = JsonConvert.DeserializeObject<TEntity>(json);
                return entityTarget;

            }, TaskContinuationOptions.OnlyOnRanToCompletion).Result;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public TEntity Update(TEntity entity)
        {
            var url = $"{BaseServiceAddress}{ServiceUrlForUpdate}";
            var json = JsonConvert.SerializeObject(entity);

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };

            var http = new HttpClient();
            var task = http.SendAsync(request);

            task.ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    var err = t.Exception.Message;
                }
            }, TaskContinuationOptions.OnlyOnFaulted);

            return task.ContinueWith(innerTask =>
            {
                var response = innerTask.Result;
                json = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                    throw new System.Exception(json);

                var entityTarget = JsonConvert.DeserializeObject<TEntity>(json);
                return entityTarget;

            }, TaskContinuationOptions.OnlyOnRanToCompletion).Result;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void RemoveById(TIdType id)
        {
            var url = $"{BaseServiceAddress}/{ServiceUrlForRemoveById.Replace("[id]", id.ToString())}";
            try
            {
                var response = HttpClient.DeleteAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    return;
                }

                var str = response.Content.ReadAsStringAsync().Result;
                throw new System.Exception(str);
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveAll()
        {
            var url = $"{BaseServiceAddress}/{ServiceUrlForRemoveAll}";

            //var queryString = HttpUtility.ParseQueryString(string.Empty);
            //foreach (var id in ids)
            //{
            //    queryString.Add("ids", id.ToString());
            //}
            //var response = _client.DeleteAsync(queryString.ToString());

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            var url = $"{BaseServiceAddress}{ServiceUrlForCount}";
            var task = HttpClient.GetStringAsync(url);

            task.ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    var err = t.Exception.Message;
                }
            }, TaskContinuationOptions.OnlyOnFaulted);

            return task.ContinueWith(innerTask =>
            {
                var json = innerTask.Result;
                return JsonConvert.DeserializeObject<int>(json);
            }, TaskContinuationOptions.OnlyOnRanToCompletion).Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity GetById(TIdType id)
        {
            var url = $"{BaseServiceAddress}{ServiceUrlForGetById}".Replace("[id]", id.ToString());
            var task = HttpClient.GetStringAsync(url);

            return task.ContinueWith(innerTask =>
            {
                var json = innerTask.Result;
                return JsonConvert.DeserializeObject<TEntity>(json);
            }).Result;
        }

        /// <summary>
        /// Returns all of the entities from the database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll()
        {
            var url = $"{BaseServiceAddress}{ServiceUrlForGetAll}";
            var task = HttpClient.GetStringAsync(url);

            task.ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    var err = t.Exception.Message;
                }
            }, TaskContinuationOptions.OnlyOnFaulted);

            return task.ContinueWith(innerTask =>
            {
                var json = innerTask.Result;
                var result=JsonConvert.DeserializeObject<IEnumerable<TEntity>>(json);
                return result;
            }, TaskContinuationOptions.OnlyOnRanToCompletion).Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAllWithEntities(params string[] entities)
        {
            var url = $"{BaseServiceAddress}{ServiceUrlForGetAllWithEntities}";
            var includes = string.Join(",", entities.ToArray());
            url = url.Replace("[entitiesToInclude]", includes);

            var task = HttpClient.GetStringAsync(url);

            task.ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    var err = t.Exception.Message;
                }
            }, TaskContinuationOptions.OnlyOnFaulted);

            return task.ContinueWith(innerTask =>
            {
                var json = innerTask.Result;
                return JsonConvert.DeserializeObject<IEnumerable<TEntity>>(json);
            }, TaskContinuationOptions.OnlyOnRanToCompletion).Result;
        }
    }
}
