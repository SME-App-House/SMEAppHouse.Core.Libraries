// ***********************************************************************
// Assembly         : BNS.TEDSvc.DA
// Author           : jcman
// Created          : 08-02-2018
//
// Last Modified By : jcman
// Last Modified On : 08-02-2018
// ***********************************************************************
// <copyright file="AppDbContextBase.cs" company="">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SMED.Core.Patterns.EF.StrategyForDBCtxt
{
    public class AppDbContextExt : DbContext
    {
        public AppDbContextExt(DbContextOptions options)
            : base(options)
        {    
        }

        public override int SaveChanges()
        {
            var entities = (from entry in ChangeTracker.Entries()
                            where entry.State == EntityState.Modified || entry.State == EntityState.Added
                            select entry.Entity);

            var validationResults = new List<ValidationResult>();
            if (entities.Any(entity => !Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults)))
            {
                throw new ValidationException(); //or do whatever you want
            }
            return base.SaveChanges();
        }

    }
}