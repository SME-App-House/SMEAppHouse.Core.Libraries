// ***********************************************************************
// Assembly         : SMED.Core.Patterns.WebApi
// Author           : jcman
// Created          : 07-04-2018
//
// Last Modified By : jcman
// Last Modified On : 07-04-2018
// ***********************************************************************
// <copyright file="UserGrant.cs" company="">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace SMED.Core.Patterns.WebApi.Models
{
    public class UserGrant
    {
        public string Username { get; set; }
        public string AccessToken { get; set; }
    }
}
