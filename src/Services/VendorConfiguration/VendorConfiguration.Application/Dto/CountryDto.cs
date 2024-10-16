#pragma warning disable CS8618

namespace VendorConfiguration.Application.Dto
{
    public record CountryDto
    {
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode2 { get; set; }
        public string CountryCode3 { get; set; }

    }
}
