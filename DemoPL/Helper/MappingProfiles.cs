using AutoMapper;
using DemoDAL.Models;
using DemoPL.ViewModels;

namespace DemoPL.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<EmployeeViewModel,Employee>().ReverseMap();

        }
    }
}
