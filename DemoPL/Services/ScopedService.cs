using System;

namespace DemoPL.Services
{
    public class ScopedService : IScopedService
    {
        public Guid Guid { get ; set ; }

        public ScopedService()
        {
            Guid = Guid.NewGuid();
        }

        public override string ToString()
        {
            return Guid.ToString();
        }

        public string GetGuid()
        {
            return Guid.ToString();
        }
    }
}
