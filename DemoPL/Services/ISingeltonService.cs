using System;

namespace DemoPL.Services
{
    public interface ISingeltonService
    {
        public Guid Guid { get; set; }

        string GetGuid();
    }
}
