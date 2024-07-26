using System;

namespace DemoPL.Services
{
    public interface ITransientService
    {
        public Guid Guid { get; set; }

        string GetGuid();
    }
}
