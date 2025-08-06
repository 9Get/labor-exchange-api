using AutoMapper;
using LaborExchangeApi.Dtos.CategoryDtos;
using LaborExchangeApi.Dtos.CompanyDtos;
using LaborExchangeApi.Dtos.ResumeDtos;
using LaborExchangeApi.Dtos.UnemployedDtos;
using LaborExchangeApi.Dtos.VacancyDtos;
using LaborExchangeApi.Models;

namespace LaborExchangeApi.Properties
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<Company, CompanyDto>();
            CreateMap<Resume, ResumeDto>();
            CreateMap<Unemployed, UnemployedDto>();
            CreateMap<Vacancy, VacancyDto>();

            CreateMap<CreateCategoryDto, Category>();
            CreateMap<CreateCompanyDto, Company>();
            CreateMap<CreateResumeDto, Resume>();
            CreateMap<CreateUnemployedDto, Unemployed>();
            CreateMap<CreateVacancyDto, Vacancy>();

            CreateMap<UpdateCategoryDto, Category>();
            CreateMap<UpdateCompanyDto, Company>();
            CreateMap<UpdateResumeDto, Resume>();
            CreateMap<UpdateUnemployedDto, Unemployed>();
            CreateMap<UpdateVacancyDto, Vacancy>();
    
            CreateMap<Company, PatchCompanyDto>();
            CreateMap<Resume, PatchResumeDto>();
            CreateMap<Unemployed, PatchUnemployedDto>();
            CreateMap<Vacancy, PatchVacancyDto>();

            CreateMap<PatchCompanyDto, Company>();
            CreateMap<PatchResumeDto, Resume>();
            CreateMap<PatchUnemployedDto, Unemployed>();
            CreateMap<PatchVacancyDto, Vacancy>();
        }
    }
}