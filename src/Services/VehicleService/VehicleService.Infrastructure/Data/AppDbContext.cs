using Vehicle.Domain.Entity.Authentication;
using Vehicle.Domain.Entity.VehicleEntity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Vehicle.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Domain.Entity.VehicleEntity.Vehicle> Vehicles { get; set; }
        public DbSet<VehicleBrand> VehicleBrands { get; set; }
        public DbSet<VehicleOwner> VehicleOwners { get; set; }
    }
}
