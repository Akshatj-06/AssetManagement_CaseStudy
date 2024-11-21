using AssetManagementWebApplication.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

public class AssetManagementContext : DbContext
{
	public AssetManagementContext(DbContextOptions<AssetManagementContext> options)
	  : base(options)
	{
	}
	public DbSet<User> Users { get; set; }
	public DbSet<Asset> Assets { get; set; }
	public DbSet<AssetAllocation> AssetAllocations { get; set; }
	public DbSet<ServiceRequest> ServiceRequests { get; set; }
	public DbSet<AuditRequest> AuditRequests { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Configure precision for AssetValue
		modelBuilder.Entity<Asset>()
			.Property(a => a.AssetValue)
			.HasPrecision(18, 2); // Precision: 18 total digits, 2 after the decimal point

		// One-to-Many: AssetAllocation -> Asset
		modelBuilder.Entity<AssetAllocation>()
			.HasOne(a => a.Asset)
			.WithMany(b => b.Allocations)
			.HasForeignKey(a => a.AssetId);

		// One-to-Many: AssetAllocation -> User
		modelBuilder.Entity<AssetAllocation>()
			.HasOne(a => a.User)
			.WithMany(b => b.Allocations)
			.HasForeignKey(a => a.UserId);

		// One-to-Many: ServiceRequest -> Asset
		modelBuilder.Entity<ServiceRequest>()
			.HasOne(s => s.Asset)
			.WithMany(a => a.ServiceRequests)
			.HasForeignKey(s => s.AssetId);

		// One-to-Many: ServiceRequest -> User
		modelBuilder.Entity<ServiceRequest>()
			.HasOne(s => s.User)
			.WithMany(u => u.ServiceRequests)
			.HasForeignKey(s => s.UserId);

		// One-to-Many: AuditRequest -> Admin (User)
		modelBuilder.Entity<AuditRequest>()
			.HasOne(a => a.Admin)
			.WithMany()
			.HasForeignKey(a => a.AdminId)
			.OnDelete(DeleteBehavior.Restrict);

		// One-to-Many: AuditRequest -> User
		modelBuilder.Entity<AuditRequest>()
			.HasOne(a => a.User)
			.WithMany(u => u.AuditRequests)
			.HasForeignKey(a => a.UserId);
	}

}

