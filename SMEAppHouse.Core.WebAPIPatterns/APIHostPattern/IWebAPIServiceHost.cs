using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SMED.Core.Patterns.Entities;
using SMED.Core.Patterns.Repo.Synchronous;

namespace SMED.Core.WebAPI.Patterns.APIHostPattern
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
        /// 
        /// </summary>
        IRepository<TEntity, TPk> Repository { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        IActionResult Create(TEntity entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        IActionResult Create(List<TEntity> entities);

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
        IActionResult Update(TEntity entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        IActionResult Update(List<TEntity> entities);

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
        IActionResult Remove(TPk id);

        /// <summary>
        /// 
        /// </summary>
        IActionResult Remove();

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
        IActionResult Get(TPk id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IActionResult Get();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entitiesToInclude"></param>
        /// <returns></returns>
        IActionResult Get(string entitiesToInclude);

        /// <summary>
        /// Important: [FromBody] must be prefixed inside web method 
        /// definition as part of the complexQuery parameter    
        /// </summary>
        /// <param name="whereStr"></param>
        /// <param name="entityParam"></param>
        /// <returns></returns>
        IActionResult Get([FromBody]string whereStr, [FromBody]TEntity entityParam);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerName"></param>
        /// <returns></returns>
        string GetHeader(string headerName);

        DateTime GetLocalDateTime([FromBody] DateTime utcDateTime, [FromBody] int timeZoneOffset);
        DateTime ConvertToClientDateTime(DateTime utcDateTime, int timeZoneOffset);
    }


}

