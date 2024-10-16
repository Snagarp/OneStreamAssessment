using AutoMapper;
using VendorConfiguration.Application.Commands;
using VendorConfiguration.Application.Dto;
using VendorConfiguration.Domain.AggregatesModel.CountryAggregate;

namespace VendorConfiguration.Api.Infrastructure.Mappers
{
    /// <summary>
    /// Profile configuration for AutoMapper that defines the mappings between domain models and DTOs.
    /// </summary>
    public class ConfigurationProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationProfile"/> class and configures the mappings.
        /// </summary>
        public ConfigurationProfile()
        {
            // Mapping between Country and CountryDto
            CreateMap<Country, CountryDto>()
                .ForMember(dst => dst.CountryCode2, opt => opt.MapFrom(src => src.Iso3166CountryCode2))
                .ForMember(dst => dst.CountryCode3, opt => opt.MapFrom(src => src.Iso3166CountryCode3));

            // Mapping between CreateCountryCommand and Country
            CreateMap<CreateCountryCommand, Country>()
                .ConstructUsing(x => new Country(x.RequestData.Iso3166CountryCode2, x.RequestData.Iso3166CountryCode3, x.RequestData.CountryName))
                .ForAllMembers(opt => opt.Ignore());

            // Mapping between ModifyCountryCommand and Country
            CreateMap<ModifyCountryCommand, Country>()
                .ForMember(dst => dst.DomainEvents, opt => opt.Ignore())
                .ForMember(dst => dst.Iso3166CountryCode2, opt => opt.Ignore())
                .ForMember(dst => dst.Iso3166CountryCode3, opt => opt.Ignore())
                .ForMember(dst => dst.CountryName, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dst, mbr) => mbr is not null));
        }
    }
}
