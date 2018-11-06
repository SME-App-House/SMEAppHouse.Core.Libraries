using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SMED.Core.Patterns.Entities;
using SMED.Core.Patterns.Repo.Synchronous;

// ReSharper disable InheritdocConsiderUsage

namespace SMED.Core.WebAPI.Patterns.APIHostPattern
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPk"></typeparam>
    public class WebAPIServiceHostBase<TEntity, TPk> : Controller, IWebAPIServiceHost<TEntity, TPk>
        where TEntity : class, IIdentifiableEntity<TPk>
    {
        public readonly IConfiguration Configuration;

        /// <summary>
        /// Middle layer for data operations
        /// </summary>
        public IRepository<TEntity, TPk> Repository { get; set; }

        #region constructors

        public WebAPIServiceHostBase(IRepository<TEntity, TPk> repository) : this(repository, null)
        {
        }

        /// <summary>
        /// Initiates this base class
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="configuration"></param>
        public WebAPIServiceHostBase(IRepository<TEntity, TPk> repository, IConfiguration configuration)
        {
            this.Repository = repository;
            this.Configuration = configuration;
        }

        #endregion

        #region implemented methods


        /// <summary>
        /// Creates the entity supplied with the API call
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual IActionResult Create(TEntity entity)
        {
            if (entity == null)
                return BadRequest($"Entity {nameof(entity)} received is null.");

            var result = Execute(() =>
            {
                try
                {
                    entity = Repository.Create(entity, true);
                }
                catch (Exception)
                {
                    throw;
                }

                return Ok(entity);
                //return Content(HttpStatusCode.OK, $"Entity added or saved: [ID:{entity.Id}]");
            });
            return result;
        }

        [HttpPost("Many")]
        public IActionResult Create(List<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create the entity serialized from the json input supplied.
        /// </summary>
        /// <param name="jsonOfEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateFromJson(object jsonOfEntity)
        {
            var jsonString = jsonOfEntity.ToString();
            var targetEntity = JsonConvert.DeserializeObject<TEntity>(jsonString);
            return Create(targetEntity);
        }

        /// <summary>
        /// Updates the values in the entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut]
        public virtual IActionResult Update(TEntity entity)
        {
            if (entity == null)
                return BadRequest($"Entity {nameof(entity)} received is null.");


            return Execute(() =>
            {
                try
                {
                    Repository.Update(entity, true);
                }
                catch (Exception ex)
                {
                    return BadRequest($"Operation error occured for {nameof(entity)}: {ex.Message}");
                }
                return Ok(entity);
                //return Content(HttpStatusCode.OK, "Entity updated.");

            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        [HttpPut("Many")]
        public IActionResult Update(List<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update the values of the entity serialized from the json input supplied
        /// </summary>
        /// <param name="jsonOfEntity"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateFromJson(object jsonOfEntity)
        {
            var jsonString = jsonOfEntity.ToString();
            var targetEntity = JsonConvert.DeserializeObject<TEntity>(jsonString);
            return Update(targetEntity);
        }

        /// <summary>
        /// Removes the entity using the id supplied
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("ById")]
        public virtual IActionResult Remove(TPk id)
        {
            return Execute(() =>
            {
                var applicationUser = Repository.Get(id);

                if (applicationUser == null)
                {
                    // Returns a NotFoundResult
                    //return Content(HttpStatusCode.NotFound, "Entity not found.");
                    return NotFound(id);
                }

                Repository.Remove(applicationUser, true);

                return Ok("Success");
            });
        }

        /// <summary>
        /// Remove all records in this entity table
        /// </summary>
        /// <returns></returns>
        [HttpPut("All")]
        public virtual IActionResult Remove()
        {
            return Execute(() =>
            {
                var entities = Repository.GetAll();
                var enumerable = entities as TEntity[] ?? entities.ToArray();

                if (!enumerable.Any())
                    return Ok("Success");

                var copy = new List<TEntity>(enumerable);
                copy.ForEach(p =>
                {
                    Repository.Remove(x => x.Id.Equals(p.Id), true);
                });

                // Returns an OkNegotiatedContentResult
                return Ok("Success");
            });
        }

        /// <summary>
        /// Get the number of records kept in this entity table
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual IActionResult Count()
        {
            return Execute(() => Ok(Repository.GetAll()?.ToList().Count() ?? 0));
        }

        /// <summary>
        /// Retrieves the id supplied from the url query string
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ById")]
        public virtual IActionResult Get(TPk id)
        {
            return Execute(() =>
            {
                var result = Repository.GetAll()?.FirstOrDefault(p => Equals(p.Id, id));

                // Returns a NotFoundResult
                if (result == null) return NotFound();

                // Returns an OkNegotiatedContentResult
                return Ok(result);
            });
        }

        /// <summary>
        /// Return all the records kept in this entity table
        /// </summary>
        /// <returns></returns>
        [HttpGet("All")]
        public virtual IActionResult Get()
        {
            return Execute(() =>
            {
                var all = Repository.GetAll();

                // Returns an OkNegotiatedContentResult
                return Ok(all);
            });
        }

        /// <summary>
        /// Return all the records kept in this entity table and include those from the entities supplied
        /// </summary>
        /// <returns></returns>
        [HttpGet("SelectEntities")]
        public virtual IActionResult Get(string entitiesToInclude)
        {
            return Execute(() =>
            {
                var all = Repository.GetAll(entitiesToInclude);

                // Returns an OkNegotiatedContentResult
                return Ok(all);
            });
        }

        /// <summary>
        /// Implements a get method for the web api with sets of conditions against the target entity type
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/formats-and-model-binding/parameter-binding-in-aspnet-web-api
        /// Important: [FromBody] must be prefixed inside web method 
        /// definition as part of the complexQuery parameter
        /// </summary>
        /// <param name="whereStr"></param>
        /// <param name="entityParam"></param>
        /// <returns></returns>
        [HttpGet("Conditional")]
        public IActionResult Get([FromBody]string whereStr, [FromBody]TEntity entityParam)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves the value defined with headername
        /// </summary>
        /// <param name="headerName"></param>
        /// <returns></returns>
        [HttpGet("GetHeader")]
        public string GetHeader(string headerName)
        {
            return Request.Headers.TryGetValue(headerName, out var headerValues)
                ? headerValues.FirstOrDefault()
                : "";
        }

        /// <summary>
        /// Gets the equivalent datetime of the specified date based on the timezone supplied.
        /// </summary>
        /// <param name="timeZoneOffset"></param>
        /// <param name="utcDateTime"></param>
        /// <returns></returns>
        [HttpGet]
        public DateTime GetLocalDateTime([FromBody]DateTime utcDateTime, [FromBody]int timeZoneOffset)
        {
            try
            {
                var utcOffset = TimeSpan.FromMinutes(timeZoneOffset);
                return utcDateTime.Add(utcOffset);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        /// <summary>
        /// Converts the equivalent datetime of the specified date based on the timezone supplied.
        /// </summary>
        /// <param name="utcDateTime"></param>
        /// <param name="timeZoneOffset"></param>
        /// <returns></returns>
        [HttpGet]
        public DateTime ConvertToClientDateTime(DateTime utcDateTime, int timeZoneOffset)
        {
            try
            {
                var utcOffset = TimeSpan.FromMinutes(timeZoneOffset);
                return utcDateTime.Add(utcOffset);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

        #region private methods

        /// <summary>
        /// Execute an action defined with ActionResult
        /// </summary>
        /// <param name="executeAction"></param>
        /// <returns></returns>
        //[HttpPost]
        private IActionResult Execute(Func<IActionResult> executeAction)
        {
            try
            {
                return executeAction();
            }
            catch (AggregateException ae)
            {
                var errs = ae.Flatten().InnerExceptions.Aggregate(string.Empty, (current, innerException) => current + innerException.Message);
                throw new Exception($"Errors: {errs}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


    }
}




#region others

///// <summary>
///// 
///// </summary>
///// <param name="conditions"></param>
///// <param name="entitiesToInclude"></param>
///// <returns></returns>
//public virtual IActionResult Get(Expression<Func<TEntity, bool>> conditions, string entitiesToInclude)
//{
//    return Execute(() =>
//    {
//        var all = Repository.GetAll(conditions, null, 0, entitiesToInclude);

//        // Returns an OkNegotiatedContentResult
//        return Ok(all);
//    });
//}



//#region associated user context

//private ApplicationUser _member;

//public ApplicationUserManager UserManager => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

//public string UserIdentityId
//{
//    get
//    {
//        var claimsIdent = (ClaimsIdentity)HttpContext.Current.User.Identity;
//        //string username = identity.Claims.First().Value;
//        var user = UserManager.FindByName(claimsIdent.Name);
//        return user.Id;
//    }
//}

//public ApplicationUser UserRecord
//{
//    get
//    {
//        if (_member != null)
//            return _member;

//        _member = UserManager.FindByEmail(Thread.CurrentPrincipal.Identity.Name);
//        return _member;
//    }
//    set => _member = value;
//}

//#endregion

#endregion