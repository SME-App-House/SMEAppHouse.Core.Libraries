

// ReSharper disable InheritdocConsiderUsage

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMEAppHouse.Core.Patterns.EF.ModelComposite;
using SMEAppHouse.Core.Patterns.Repo.Base;

namespace SMEAppHouse.Core.Patterns.WebApi.APIHostPattern
{
    [EnableCors("SiteCorsPolicy")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WebAPIServiceHost<TEntity, TPk> : WebAPIServiceHostExt, IWebAPIServiceHost<TEntity, TPk>
        where TEntity : class, IIdentifiableEntity<TPk>
    {
        ///<inheritdoc cref="IIdentifiableEntity{TPk}"/>
        /// <summary>
        /// blah! blah! blah! blah! blah! blah! blah! blah! blah! blah! blah! blah! blah! 
        /// </summary>
        public IRepositorySync<TEntity, TPk> Repository { get; set; }

        #region constructors

        /// <summary>
        /// Initiates this base class
        /// </summary>
        /// <param name="repository"></param>
        protected WebAPIServiceHost(IRepositorySync<TEntity, TPk> repository)
        {
            this.Repository = repository;
        }

        #endregion

        #region  IWebAPIServiceHost implementations

        /// <inheritdoc cref="IIdentifiableEntity{TPk}"/>
        /// <summary>
        /// blah! blah! blah! blah! blah! blah! blah! blah! blah! blah! blah! blah! 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[Action]")]
        public virtual IActionResult CreateSingle(TEntity entity)
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

        [HttpPost]
        [Route("[Action]")]
        public virtual IActionResult CreateMany(List<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("[Action]")]
        public virtual IActionResult CreateFromJson(object jsonOfEntity)
        {
            var jsonString = jsonOfEntity.ToString();
            var targetEntity = JsonConvert.DeserializeObject<TEntity>(jsonString);
            return CreateSingle(targetEntity);
        }

        [HttpPut]
        [Route("[Action]")]
        public virtual IActionResult UpdateSingle(TEntity entity)
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

        [HttpPut]
        [Route("[Action]")]
        public virtual IActionResult UpdateMany(List<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("[Action]")]
        public virtual IActionResult UpdateFromJson(object jsonOfEntity)
        {
            var jsonString = jsonOfEntity.ToString();
            var targetEntity = JsonConvert.DeserializeObject<TEntity>(jsonString);
            return UpdateSingle(targetEntity);
        }

        [HttpDelete]
        [Route("[Action]")]
        public virtual IActionResult RemoveById(TPk id)
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

        [HttpDelete]
        [Route("[Action]")]
        public virtual IActionResult RemoveMany(TPk[] ids)
        {
            return Execute(() =>
            {
                Repository.Remove(p => ids.Contains(p.Id), true);
                return Ok("Success");
            });
        }

        [HttpDelete]
        [Route("[Action]")]
        public virtual IActionResult Zap()
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

        [HttpGet]
        [Route("[Action]")]
        public virtual IActionResult Count()
        {
            return Execute(() => Ok(Repository.GetAll()?.ToList().Count() ?? 0));
        }

        [HttpGet]
        [Route("[Action]")]
        public virtual IActionResult GetById(TPk id)
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

        [HttpGet]
        [Route("[Action]")]
        public virtual IActionResult GetAll()
        {
            return Execute(() =>
            {
                var all = Repository.GetAll();

                // Returns an OkNegotiatedContentResult
                return Ok(all);
            });
        }

        [HttpGet]
        [Route("[Action]")]
        public virtual IActionResult GetAndEntities(string entitiesToInclude)
        {
            return Execute(() =>
            {
                var all = Repository.GetAll(entitiesToInclude);

                // Returns an OkNegotiatedContentResult
                return Ok(all);
            });
        }

        [HttpGet]
        [Route("[Action]")]
        public virtual IActionResult GetAndConditional([FromBody]string whereStr, [FromBody]TEntity entityParam)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("[Action]")]
        public virtual string GetHeader(string headerName)
        {
            return Request.Headers.TryGetValue(headerName, out var headerValues)
                ? headerValues.FirstOrDefault()
                : "";
        }

        [HttpGet]
        [Route("[Action]")]
        public virtual DateTime GetLocalTZ(DateTime utcDateTime, int timeZoneOffset)
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

        [HttpGet]
        [Route("[Action]")]
        public virtual DateTime ToClientTZ(DateTime utcDateTime, int timeZoneOffset)
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

        protected override IActionResult Execute(Func<IActionResult> executeAction)
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