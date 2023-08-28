using static System.Runtime.InteropServices.JavaScript.JSType;
using TaskManagement.Models.DTO.User;
using TaskManagement.Models;
using AutoMapper;

namespace TaskManagement
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
