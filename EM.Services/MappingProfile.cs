using AutoMapper;
using EM.Core.Models.Entities;
using EM.Models;
using EM.Models.Entities;
using System.Xml.Linq;

namespace EM.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AspNetUser, RegisterViewModel>().ReverseMap();
            CreateMap<AspNetUser, UserModel>().ReverseMap();
            CreateMap<EmployeeMetadata, Employee>().ReverseMap();
            CreateMap<Core.Models.Entities.EmployeeEducation, EmployeeEducationMetadata>().ReverseMap();
            CreateMap<EmployeeEducationType, EmployeeEducationTypeViewModel>().ReverseMap();
            CreateMap<EmployeeBankAccountMetadata, Core.Models.Entities.EmployeeBankAccount>().ReverseMap();
            CreateMap<EmployeeExperieceMetadata, Core.Models.Entities.EmployeeExperiece>().ReverseMap();
            CreateMap<EmployeeITExperienceMetadata, EM.Core.Models.Entities.EmployeeITExperience>().ReverseMap();
            CreateMap<BatchMetadata, EM.Core.Models.Entities.Batch>().ReverseMap();
            CreateMap<TeamMetadata, EM.Core.Models.Entities.Team>().ReverseMap();
            CreateMap<EmployeeResignationMetadata, Core.Models.Entities.EmployeeResignation>().ReverseMap();
            CreateMap<CompanyMetadata, Core.Models.Entities.Company>().ReverseMap();
            CreateMap<UserRoleViewModel, UserRole>().ReverseMap();
            CreateMap<RoleViewModel, AspNetRole>().ReverseMap();
            CreateMap<CompanyMetadata, EM.Core.Models.Entities.Company>().ReverseMap();
            CreateMap<CompanyDocumentsTypeMetadata, EM.Core.Models.Entities.CompanyDocumentsType>().ReverseMap();
            CreateMap<CompanyDocumentMetadata, EM.Core.Models.Entities.CompanyDocument>()/*
                    .ForMember(dest => dest.CompanyDocumentsType, opt => opt.MapFrom(src => src.CompanyDocumentsType == null ? new CompanyDocumentsTypeMetadata() : src.CompanyDocumentsType))
                    .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company == null ? new CompanyMetadata() : src.Company))
                    .ForMember(dest => dest.CreatedByNavigation, opt => opt.MapFrom(src => src.CreatedByNavigation == null ? new UserModel() : src.CreatedByNavigation))*/
                    .ReverseMap();


            //CreateMap<AspNetUserRole, UserRoleViewModel>().ReverseMap();
        }
    }
}
