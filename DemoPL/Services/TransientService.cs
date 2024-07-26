using System;

namespace DemoPL.Services
{
    public class TransientService : ITransientService
    {
        public Guid Guid { get ; set ; }

        public TransientService()
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
