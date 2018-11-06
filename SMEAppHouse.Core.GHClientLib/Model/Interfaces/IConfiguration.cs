using System;

namespace SMEAppHouse.Core.GHClientLib.Model.Interfaces
{
    public interface IConfiguration
    {
        Guid Uid { get; }

        IConfiguration ConfigurationMember { get; set; }
    }
}