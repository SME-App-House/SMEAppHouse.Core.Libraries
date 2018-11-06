using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SMEAppHouse.Core.AppMgt.AppCfgs.Interfaces;

namespace SMEAppHouse.Core.AppMgt.AppCfgs
{
    public static class Extensions
    {
        /// <summary>
        /// Inject AppIdentitySettings so that others can use too
        /// https://stackoverflow.com/a/46940811
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="sectionName"></param>
        public static void LoadAppConfig<T>(this IServiceCollection services, IConfiguration configuration, string sectionName) where T : class, IAppConfig
        {
            var settingsSection = configuration.GetSection(sectionName);
            services.Configure<T>(settingsSection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration"></param>
        /// <param name="sectionName"></param>
        public static T GetAppConfig<T>(this IConfiguration configuration, string sectionName) where T : class
        {
            var settingsSection = configuration.GetSection(sectionName);
            var settings = settingsSection.Get<T>();
            return settings;
        }
    }
}