// ***********************************************************************
// Assembly         : SMED.Core.Patterns.WebApi
// Author           : jcman
// Created          : 07-04-2018
//
// Last Modified By : jcman
// Last Modified On : 08-03-2018
// ***********************************************************************
// <copyright file="IWebAPIServiceHost.cs" company="">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SMED.Core.Patterns.EF.ModelComposite;
using SMED.Core.Patterns.Repo.Base;

namespace SMED.Core.Patterns.WebApi.APIHostPattern
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPk"></typeparam>
    public interface IConventionalApiController<TEntity, TPk>
        where TEntity : class, IIdentifiableEntity<TPk>

    {
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="executeAction"></param>
        ///// <returns></returns>
        //IActionResult Execute(Func<IActionResult> executeAction); 

        /// <summary>
        /// Reference to the repository middle layer
        /// </summary>
        IRepositorySync<TEntity, TPk> Repository { get; set; }

        /// <summary>
        /// Creates single <see cref="TEntity"/> supplied with the API call
        /// </summary>
        /// <param name="entity"></param>
        IActionResult CreateSingle(TEntity entity);

        /// <summary>
        /// Creates multiple of <see cref="TEntity"/> supplied with the API call
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        IActionResult CreateMany(List<TEntity> entities);

        /// <summary>
        /// Create the entity serialized from the json input supplied.
        /// ref: https://stackoverflow.com/questions/24129919/web-api-complex-parameter-properties-are-all-null/24131860#24131860
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
        IActionResult RemoveAll();

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
        IActionResult GetAllWithEntities(string entitiesToInclude);

        ///// <summary>
        ///// Important: [FromBody] must be prefixed inside web method 
        ///// definition as part of the complexQuery parameter    
        ///// </summary>
        ///// <param name="whereStr"></param>
        ///// <param name="entityParam"></param>
        ///// <returns></returns>
        //IActionResult GetViaCondition([FromBody]string whereStr, [FromBody]TEntity entityParam);

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

