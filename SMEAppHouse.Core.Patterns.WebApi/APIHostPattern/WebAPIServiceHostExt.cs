using System;
using Microsoft.AspNetCore.Mvc;

namespace SMEAppHouse.Core.Patterns.WebApi.APIHostPattern
{
    public abstract class WebAPIServiceHostExt : Controller
    {
        protected abstract IActionResult Execute(Func<IActionResult> executeAction);
    }
}