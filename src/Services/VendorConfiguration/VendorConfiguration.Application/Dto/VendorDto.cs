#pragma warning disable CS8618

namespace VendorConfiguration.Application.Dto
{
    public class VendorDto
    {
        public int VendorId { get; set; }
        public string VendorKey { get; set; }
        public string VendorName { get; set; }
        public bool IsEnabled { get; set; }
    }
}
