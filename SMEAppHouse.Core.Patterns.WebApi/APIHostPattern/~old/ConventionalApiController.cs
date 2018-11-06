// ***********************************************************************
// Assembly         : SMED.Core.Patterns.WebApi
// Author           : jcman
// Created          : 07-04-2018
//
// Last Modified By : jcman
// Last Modified On : 08-03-2018
// ***********************************************************************
// <copyright file="WebAPIServiceHostBase.cs" company="">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SMED.Core.Patterns.EF.ModelComposite;
using SMED.Core.Patterns.Repo.Base;

// ReSharper disable InheritdocConsiderUsage

namespace SMED.Core.Patterns.WebApi.APIHostPattern
{
    public abstract class ConventionalApiControllerExt : Controller
    {
        protected abstract IActionResult Execute(Func<IActionResult> executeAction);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPk"></typeparam>
    [Route("api/[controller]")]
    public class ConventionalApiController<TEntity, TPk> : ConventionalApiControllerExt, IConventionalApiController<TEntity, TPk>
        where TEntity : class, IIdentifiableEntity<TPk>
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IRepositorySync<TEntity, TPk> Repository { get; set; }

        #region constructors

        public ConventionalApiController(IRepositorySync<TEntity, TPk> repository) //: this(null, repository)
        {
            this.Repository = repository;
        }

        #endregion

        #region implemented methods

        /// <summary>
        /// Execute an action defined with ActionResult
        /// </summary>
        /// <param name="executeAction"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Create single entity of <see cref="TEntity"/>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("CreateSingle")]
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
            });
            return result;
        }

        /// <inheritdoc cref="IConventionalApiController{TEntity,TPk}"/>
        /// <summary>
        /// Creates multiple of <see cref="TEntity"/> supplied with the API call
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        [HttpPost("CreateMany")]
        public virtual IActionResult CreateMany(List<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonOfEntity"></param>
        /// <returns></returns>
        [HttpPost("CreateFromJson")]
        public virtual IActionResult CreateFromJson(object jsonOfEntity)
        {
            var jsonString = jsonOfEntity.ToString();
            var targetEntity = JsonConvert.DeserializeObject<TEntity>(jsonString);
            return CreateSingle(targetEntity);
        }

        /// <summary>
        /// Updates the values in the entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut("UpdateSingle")]
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
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        [HttpPut("UpdateMany")]
        public virtual IActionResult UpdateMany(List<TEntity> entities)
        {
            if (entities == null)
                return BadRequest($"Entity {nameof(entities)} received is null.");

            return Execute(() =>
            {
                try
                {
                    entities.ForEach(e => { UpdateSingle(e); });
                }
                catch (Exception ex)
                {
                    return BadRequest($"Operation error occured for {nameof(entities)}: {ex.Message}");
                }
                return Ok(entities);
                //return Content(HttpStatusCode.OK, "Entity updated.");
            });
        }

        /// <summary>
        /// Update the values of the entity serialized from the json input supplied
        /// </summary>
        /// <param name="jsonOfEntity"></param>
        /// <returns></returns>
        [HttpPut("UpdateFromJson")]
        public virtual IActionResult UpdateFromJson(object jsonOfEntity)
        {
            var jsonString = jsonOfEntity.ToString();
            var targetEntity = JsonConvert.DeserializeObject<TEntity>(jsonString);
            return UpdateSingle(targetEntity);
        }

        /// <summary>
        /// Removes the entity using the id supplied
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[HttpDelete("api/[controller]/[action]")]
        [HttpDelete("RemoveById")]
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

        /// <summary>
        /// Remove all records in this entity table
        /// </summary>
        /// <returns></returns>
        [HttpDelete("RemoveAll")]
        public virtual IActionResult RemoveAll()
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
        [HttpGet("GetById")]
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

        /// <summary>
        /// Return all the records kept in this entity table
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public virtual IActionResult GetAll()
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
        [HttpGet("GetAllWithEntities")]
        public virtual IActionResult GetAllWithEntities(string entitiesToInclude)
        {
            return Execute(() =>
            {
                var all = Repository.GetAll(entitiesToInclude);

                // Returns an OkNegotiatedContentResult
                return Ok(all);
            });
        }

        /// <summary>
        /// Retrieves the value defined with headername
        /// </summary>
        /// <param name="headerName"></param>
        /// <returns></returns>
        [HttpGet("GetHeader")]
        public virtual string GetHeader(string headerName)
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
        [HttpGet("GetLocalDateTime")]
        public virtual DateTime GetLocalDateTime([FromBody]DateTime utcDateTime, [FromBody]int timeZoneOffset)
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
        [HttpGet("ConvertToClientDateTime")]
        public virtual DateTime ConvertToClientDateTime(DateTime utcDateTime, int timeZoneOffset)
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