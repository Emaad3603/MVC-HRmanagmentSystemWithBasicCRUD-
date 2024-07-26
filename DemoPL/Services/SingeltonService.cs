using System;

namespace DemoPL.Services
{
    public class SingeltonService : ISingeltonService
    {
        public Guid Guid { get ; set ; }

        public SingeltonService()
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
