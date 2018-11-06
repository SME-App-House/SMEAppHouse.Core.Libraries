using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using SMEAppHouse.Core.AppMgt.AppCfgs.Interfaces;

namespace SMEAppHouse.Core.AppMgt.AppCfgs.Validator
{
    /// <summary>
    /// https://andrewlock.net/adding-validation-to-strongly-typed-configuration-objects-in-asp-net-core/
    /// </summary>
    public class AppConfigValidationStartupFilter : IStartupFilter
    {
        private readonly IEnumerable<IAppConfig> _validatableObjects;
        public AppConfigValidationStartupFilter(IEnumerable<IAppConfig> validatableObjects)
        {
            _validatableObjects = validatableObjects;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            foreach (var validatableObject in _validatableObjects)
            {
                validatableObject.Validate();
            }

            //don't alter the configuration
            return next;
        }
    }
}