using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class DataContext : DbContext{
    public DataContext(DbContextOptions options) : base(options){}

    public DbSet<BusinessProfile> BusinessProfile { get; set; }
    public DbSet<Company> Company { get; set; }
    
    public DbSet<OpeningHours> OpeningHours { get; set; }
    
    public DbSet<ExceptionHours> ExceptionHours { get; set; }
    
    public DbSet<Service> Service { get; set; }
    
    public DbSet<Booking> Booking { get; set; }
}