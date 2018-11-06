

// ReSharper disable InheritdocConsiderUsage

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMED.Core.Patterns.EF.ModelComposite;
using SMED.Core.Patterns.Repo.Base;

namespace SMED.Core.Patterns.WebApi.APIHostPattern
{
    /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPk"></typeparam>
        [Produces("application/json")]
    [Route("api/[controller]")]
    public abstract class WebAPIServiceHostBase<TEntity, TPk> : WebAPIServiceHostExt, IWebAPIServiceHost<TEntity, TPk>
        where TEntity : class, IIdentifiableEntity<TPk>
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IRepositorySync<TEntity, TPk> Repository { get; set; }

        #region constructors

        /// <summary>
        /// Initiates this base class
        /// </summary>
        /// <param name="repository"></param>
        protected WebAPIServiceHostBase(IRepositorySync<TEntity, TPk> repository)
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
           
        }


        /// <summary>
        /// Creates the entity supplied with the API call
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("CreateSingle")]
        //[Route("CreateSingle")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public virtual IActionResult CreateSingle(TEntity entity)
        {
           
        }

        [HttpPost]
        [Route("CreateMany")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult CreateMany(List<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create the entity serialized from the json input supplied.
        /// </summary>
        /// <param name="jsonOfEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateFromJson")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult CreateFromJson(object jsonOfEntity)
        {

        }

        /// <summary>
        /// Updates the values in the entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateSingle")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual IActionResult UpdateSingle(TEntity entity)
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateMany")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult UpdateMany(List<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update the values of the entity serialized from the json input supplied
        /// </summary>
        /// <param name="jsonOfEntity"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateFromJson")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult UpdateFromJson(object jsonOfEntity)
        {
           
        }

        /// <summary>
        /// Removes the entity using the id supplied
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Route("RemoveById")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual IActionResult RemoveById(TPk id)
        {
            
        }

        /// <summary>
        /// Remove all records in this entity table
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        //[Route("RemoveAll")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual IActionResult Zap()
        {
           
        }

        /// <summary>
        /// Get the number of records kept in this entity table
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Route("GetCount")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual IActionResult Count()
        {
            
        }

        /// <summary>
        /// Retrieves the id supplied from the url query string
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Route("GetById")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual IActionResult Get(TPk id)
        {
           
        }

        /// <summary>
        /// Return all the records kept in this entity table
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual IActionResult GetAll()
        {
           
        }

        /// <summary>
        /// Return all the records kept in this entity table and include those from the entities supplied
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetWithEntities")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual IActionResult Get(string entitiesToInclude)
        {
            
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
        [HttpGet]
        [Route("GetConditional")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Get([FromBody]string whereStr, [FromBody]TEntity entityParam)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves the value defined with headername
        /// </summary>
        /// <param name="headerName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetHeader")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public string GetHeader(string headerName)
        {
           
        }

        /// <summary>
        /// Gets the equivalent datetime of the specified date based on the timezone supplied.
        /// </summary>
        /// <param name="timeZoneOffset"></param>
        /// <param name="utcDateTime"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ToLocalTZ")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public DateTime GetLocalDateTime([FromBody]DateTime utcDateTime, [FromBody]int timeZoneOffset)
        {
           

        }

        /// <summary>
        /// Converts the equivalent datetime of the specified date based on the timezone supplied.
        /// </summary>
        /// <param name="utcDateTime"></param>
        /// <param name="timeZoneOffset"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ToClientTZ")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public DateTime ConvertToClientDateTime(DateTime utcDateTime, int timeZoneOffset)
        {
            
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