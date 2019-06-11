using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SMEAppHouse.Core.Patterns.Repo.UnitOfWork;

namespace SMEAppHouse.Core.Patterns.Repo.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class UnitOfWorkServiceCollectionExtentions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<IGenericUnitOfWork<Guid>, UnitOfWorkBase<TContext, Guid>>();
            services.AddScoped<IGenericUnitOfWork<TContext, Guid>, UnitOfWorkBase<TContext, Guid>>();

            services.AddScoped<IGenericUnitOfWork<int>, UnitOfWorkBase<TContext, int>>();
            services.AddScoped<IGenericUnitOfWork<TContext, int>, UnitOfWorkBase<TContext, int>>();

            services.AddScoped<IGenericUnitOfWork<long>, UnitOfWorkBase<TContext, long>>();
            services.AddScoped<IGenericUnitOfWork<TContext, long>, UnitOfWorkBase<TContext, long>>();


            services.AddScoped<UnitOfWork.GuidPKBasedVariation.IUnitOfWork, UnitOfWork.GuidPKBasedVariation.UnitOfWork<TContext>>();
            services.AddScoped<UnitOfWork.IntPKBasedVariation.IUnitOfWork, UnitOfWork.IntPKBasedVariation.UnitOfWork<TContext>>();
            services.AddScoped<UnitOfWork.LongPKBasedVariation.IUnitOfWork, UnitOfWork.LongPKBasedVariation.UnitOfWork<TContext>>();

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TContext1"></typeparam>
        /// <typeparam name="TContext2"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddUnitOfWork<TContext1, TContext2>(this IServiceCollection services)
            where TContext1 : DbContext
            where TContext2 : DbContext
        {
            services.AddScoped<IGenericUnitOfWork<Guid>, UnitOfWorkBase<TContext1, Guid>>();
            services.AddScoped<IGenericUnitOfWork<Guid>, UnitOfWorkBase<TContext2, Guid>>();
            services.AddScoped<IGenericUnitOfWork<TContext1, Guid>, UnitOfWorkBase<TContext1, Guid>>();
            services.AddScoped<IGenericUnitOfWork<TContext2, Guid>, UnitOfWorkBase<TContext2, Guid>>();

            services.AddScoped<IGenericUnitOfWork<int>, UnitOfWorkBase<TContext1, int>>();
            services.AddScoped<IGenericUnitOfWork<int>, UnitOfWorkBase<TContext2, int>>();
            services.AddScoped<IGenericUnitOfWork<TContext1, int>, UnitOfWorkBase<TContext1, int>>();
            services.AddScoped<IGenericUnitOfWork<TContext2, int>, UnitOfWorkBase<TContext2, int>>();

            services.AddScoped<IGenericUnitOfWork<long>, UnitOfWorkBase<TContext1, long>>();
            services.AddScoped<IGenericUnitOfWork<long>, UnitOfWorkBase<TContext2, long>>();
            services.AddScoped<IGenericUnitOfWork<TContext1, long>, UnitOfWorkBase<TContext1, long>>();
            services.AddScoped<IGenericUnitOfWork<TContext2, long>, UnitOfWorkBase<TContext2, long>>();

            services.AddScoped<UnitOfWork.GuidPKBasedVariation.IUnitOfWork, UnitOfWork.GuidPKBasedVariation.UnitOfWork<TContext1>>();
            services.AddScoped<UnitOfWork.IntPKBasedVariation.IUnitOfWork, UnitOfWork.IntPKBasedVariation.UnitOfWork<TContext1>>();
            services.AddScoped<UnitOfWork.LongPKBasedVariation.IUnitOfWork, UnitOfWork.LongPKBasedVariation.UnitOfWork<TContext1>>();

            services.AddScoped<UnitOfWork.GuidPKBasedVariation.IUnitOfWork, UnitOfWork.GuidPKBasedVariation.UnitOfWork<TContext2>>();
            services.AddScoped<UnitOfWork.IntPKBasedVariation.IUnitOfWork, UnitOfWork.IntPKBasedVariation.UnitOfWork<TContext2>>();
            services.AddScoped<UnitOfWork.LongPKBasedVariation.IUnitOfWork, UnitOfWork.LongPKBasedVariation.UnitOfWork<TContext2>>();

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TContext1"></typeparam>
        /// <typeparam name="TContext2"></typeparam>
        /// <typeparam name="TContext3"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddUnitOfWork<TContext1, TContext2, TContext3>(
            this IServiceCollection services)
            where TContext1 : DbContext
            where TContext2 : DbContext
            where TContext3 : DbContext
        {
            services.AddScoped<IGenericUnitOfWork<Guid>, UnitOfWorkBase<TContext1, Guid>>();
            services.AddScoped<IGenericUnitOfWork<Guid>, UnitOfWorkBase<TContext2, Guid>>();
            services.AddScoped<IGenericUnitOfWork<Guid>, UnitOfWorkBase<TContext3, Guid>>();
            services.AddScoped<IGenericUnitOfWork<TContext1, Guid>, UnitOfWorkBase<TContext1, Guid>>();
            services.AddScoped<IGenericUnitOfWork<TContext2, Guid>, UnitOfWorkBase<TContext2, Guid>>();
            services.AddScoped<IGenericUnitOfWork<TContext3, Guid>, UnitOfWorkBase<TContext3, Guid>>();

            services.AddScoped<IGenericUnitOfWork<int>, UnitOfWorkBase<TContext1, int>>();
            services.AddScoped<IGenericUnitOfWork<int>, UnitOfWorkBase<TContext2, int>>();
            services.AddScoped<IGenericUnitOfWork<int>, UnitOfWorkBase<TContext3, int>>();
            services.AddScoped<IGenericUnitOfWork<TContext1, int>, UnitOfWorkBase<TContext1, int>>();
            services.AddScoped<IGenericUnitOfWork<TContext2, int>, UnitOfWorkBase<TContext2, int>>();
            services.AddScoped<IGenericUnitOfWork<TContext3, int>, UnitOfWorkBase<TContext3, int>>();

            services.AddScoped<IGenericUnitOfWork<long>, UnitOfWorkBase<TContext1, long>>();
            services.AddScoped<IGenericUnitOfWork<long>, UnitOfWorkBase<TContext2, long>>();
            services.AddScoped<IGenericUnitOfWork<long>, UnitOfWorkBase<TContext3, long>>();
            services.AddScoped<IGenericUnitOfWork<TContext1, long>, UnitOfWorkBase<TContext1, long>>();
            services.AddScoped<IGenericUnitOfWork<TContext2, long>, UnitOfWorkBase<TContext2, long>>();
            services.AddScoped<IGenericUnitOfWork<TContext3, long>, UnitOfWorkBase<TContext3, long>>();


            services.AddScoped<UnitOfWork.GuidPKBasedVariation.IUnitOfWork, UnitOfWork.GuidPKBasedVariation.UnitOfWork<TContext1>>();
            services.AddScoped<UnitOfWork.IntPKBasedVariation.IUnitOfWork, UnitOfWork.IntPKBasedVariation.UnitOfWork<TContext1>>();
            services.AddScoped<UnitOfWork.LongPKBasedVariation.IUnitOfWork, UnitOfWork.LongPKBasedVariation.UnitOfWork<TContext1>>();

            services.AddScoped<UnitOfWork.GuidPKBasedVariation.IUnitOfWork, UnitOfWork.GuidPKBasedVariation.UnitOfWork<TContext2>>();
            services.AddScoped<UnitOfWork.IntPKBasedVariation.IUnitOfWork, UnitOfWork.IntPKBasedVariation.UnitOfWork<TContext2>>();
            services.AddScoped<UnitOfWork.LongPKBasedVariation.IUnitOfWork, UnitOfWork.LongPKBasedVariation.UnitOfWork<TContext2>>();

            services.AddScoped<UnitOfWork.GuidPKBasedVariation.IUnitOfWork, UnitOfWork.GuidPKBasedVariation.UnitOfWork<TContext3>>();
            services.AddScoped<UnitOfWork.IntPKBasedVariation.IUnitOfWork, UnitOfWork.IntPKBasedVariation.UnitOfWork<TContext3>>();
            services.AddScoped<UnitOfWork.LongPKBasedVariation.IUnitOfWork, UnitOfWork.LongPKBasedVariation.UnitOfWork<TContext3>>();

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TContext1"></typeparam>
        /// <typeparam name="TContext2"></typeparam>
        /// <typeparam name="TContext3"></typeparam>
        /// <typeparam name="TContext4"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddUnitOfWork<TContext1, TContext2, TContext3, TContext4>(
            this IServiceCollection services)
            where TContext1 : DbContext
            where TContext2 : DbContext
            where TContext3 : DbContext
            where TContext4 : DbContext
        {
            services.AddScoped<IGenericUnitOfWork<Guid>, UnitOfWorkBase<TContext1, Guid>>();
            services.AddScoped<IGenericUnitOfWork<Guid>, UnitOfWorkBase<TContext2, Guid>>();
            services.AddScoped<IGenericUnitOfWork<Guid>, UnitOfWorkBase<TContext3, Guid>>();
            services.AddScoped<IGenericUnitOfWork<Guid>, UnitOfWorkBase<TContext4, Guid>>();
            services.AddScoped<IGenericUnitOfWork<TContext1, Guid>, UnitOfWorkBase<TContext1, Guid>>();
            services.AddScoped<IGenericUnitOfWork<TContext2, Guid>, UnitOfWorkBase<TContext2, Guid>>();
            services.AddScoped<IGenericUnitOfWork<TContext3, Guid>, UnitOfWorkBase<TContext3, Guid>>();
            services.AddScoped<IGenericUnitOfWork<TContext4, Guid>, UnitOfWorkBase<TContext4, Guid>>();

            services.AddScoped<IGenericUnitOfWork<int>, UnitOfWorkBase<TContext1, int>>();
            services.AddScoped<IGenericUnitOfWork<int>, UnitOfWorkBase<TContext2, int>>();
            services.AddScoped<IGenericUnitOfWork<int>, UnitOfWorkBase<TContext3, int>>();
            services.AddScoped<IGenericUnitOfWork<int>, UnitOfWorkBase<TContext4, int>>();
            services.AddScoped<IGenericUnitOfWork<TContext1, int>, UnitOfWorkBase<TContext1, int>>();
            services.AddScoped<IGenericUnitOfWork<TContext2, int>, UnitOfWorkBase<TContext2, int>>();
            services.AddScoped<IGenericUnitOfWork<TContext3, int>, UnitOfWorkBase<TContext3, int>>();
            services.AddScoped<IGenericUnitOfWork<TContext4, int>, UnitOfWorkBase<TContext4, int>>();

            services.AddScoped<IGenericUnitOfWork<long>, UnitOfWorkBase<TContext1, long>>();
            services.AddScoped<IGenericUnitOfWork<long>, UnitOfWorkBase<TContext2, long>>();
            services.AddScoped<IGenericUnitOfWork<long>, UnitOfWorkBase<TContext3, long>>();
            services.AddScoped<IGenericUnitOfWork<long>, UnitOfWorkBase<TContext4, long>>();
            services.AddScoped<IGenericUnitOfWork<TContext1, long>, UnitOfWorkBase<TContext1, long>>();
            services.AddScoped<IGenericUnitOfWork<TContext2, long>, UnitOfWorkBase<TContext2, long>>();
            services.AddScoped<IGenericUnitOfWork<TContext3, long>, UnitOfWorkBase<TContext3, long>>();
            services.AddScoped<IGenericUnitOfWork<TContext4, long>, UnitOfWorkBase<TContext4, long>>();

            services.AddScoped<UnitOfWork.GuidPKBasedVariation.IUnitOfWork, UnitOfWork.GuidPKBasedVariation.UnitOfWork<TContext1>>();
            services.AddScoped<UnitOfWork.IntPKBasedVariation.IUnitOfWork, UnitOfWork.IntPKBasedVariation.UnitOfWork<TContext1>>();
            services.AddScoped<UnitOfWork.LongPKBasedVariation.IUnitOfWork, UnitOfWork.LongPKBasedVariation.UnitOfWork<TContext1>>();

            services.AddScoped<UnitOfWork.GuidPKBasedVariation.IUnitOfWork, UnitOfWork.GuidPKBasedVariation.UnitOfWork<TContext2>>();
            services.AddScoped<UnitOfWork.IntPKBasedVariation.IUnitOfWork, UnitOfWork.IntPKBasedVariation.UnitOfWork<TContext2>>();
            services.AddScoped<UnitOfWork.LongPKBasedVariation.IUnitOfWork, UnitOfWork.LongPKBasedVariation.UnitOfWork<TContext2>>();

            services.AddScoped<UnitOfWork.GuidPKBasedVariation.IUnitOfWork, UnitOfWork.GuidPKBasedVariation.UnitOfWork<TContext3>>();
            services.AddScoped<UnitOfWork.IntPKBasedVariation.IUnitOfWork, UnitOfWork.IntPKBasedVariation.UnitOfWork<TContext3>>();
            services.AddScoped<UnitOfWork.LongPKBasedVariation.IUnitOfWork, UnitOfWork.LongPKBasedVariation.UnitOfWork<TContext3>>();

            services.AddScoped<UnitOfWork.GuidPKBasedVariation.IUnitOfWork, UnitOfWork.GuidPKBasedVariation.UnitOfWork<TContext4>>();
            services.AddScoped<UnitOfWork.IntPKBasedVariation.IUnitOfWork, UnitOfWork.IntPKBasedVariation.UnitOfWork<TContext4>>();
            services.AddScoped<UnitOfWork.LongPKBasedVariation.IUnitOfWork, UnitOfWork.LongPKBasedVariation.UnitOfWork<TContext4>>();

            return services;
        }
    }
}