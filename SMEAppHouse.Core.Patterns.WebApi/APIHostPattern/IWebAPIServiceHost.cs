using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SMEAppHouse.Core.Patterns.EF.ModelComposite;
using SMEAppHouse.Core.Patterns.Repo.Base;

namespace SMEAppHouse.Core.Patterns.WebApi.APIHostPattern
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPk"></typeparam>
    public interface IWebAPIServiceHost<TEntity, TPk>
        where TEntity : class, IIdentifiableEntity<TPk>

    {
        /// <summary>
        /// Reference to the entity's data repository for handling backend data
        /// </summary>
        IRepositorySync<TEntity, TPk> Repository { get; set; }

        /// <summary>
        /// Create instance of the <see cref="TEntity"/>
        /// </summary>
        /// <param name="entity"></param>
        IActionResult CreateSingle(TEntity entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        IActionResult CreateMany(List<TEntity> entities);

        /// <summary>
        /// https://stackoverflow.com/questions/24129919/web-api-complex-parameter-properties-are-all-null/24131860#24131860
        /// </summary>
        /// <param name="jsonOfEntity"></param>
        /// <returns></returns>
        IActionResult CreateFromJson(object jsonOfEntity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        IActionResult UpdateSingle(TEntity entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        IActionResult UpdateMany(List<TEntity> entities);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonOfEntity"></param>
        /// <returns></returns>
        IActionResult UpdateFromJson(object jsonOfEntity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        IActionResult RemoveById(TPk id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IActionResult RemoveMany(TPk[] ids);

        /// <summary>
        /// Obliterate all of the records in this table or collection
        /// </summary>
        IActionResult Zap();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IActionResult Count();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IActionResult GetById(TPk id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IActionResult GetAll();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entitiesToInclude"></param>
        /// <returns></returns>
        IActionResult GetAndEntities(string entitiesToInclude);

        /// <summary>
        /// Important: [FromBody] must be prefixed inside web method 
        /// definition as part of the complexQuery parameter    
        /// </summary>
        /// <param name="whereStr"></param>
        /// <param name="entityParam"></param>
        /// <returns></returns>
        IActionResult GetAndConditional([FromBody]string whereStr, [FromBody]TEntity entityParam);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerName"></param>
        /// <returns></returns>
        string GetHeader(string headerName);

        DateTime GetLocalTZ([FromBody] DateTime utcDateTime, [FromBody] int timeZoneOffset);
        DateTime ToClientTZ(DateTime utcDateTime, int timeZoneOffset);
    }


}

