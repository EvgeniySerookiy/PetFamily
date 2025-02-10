using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.PetContext;
using PetFamily.Domain.VolunteerContext;

namespace PetFamily.Infrastructure;

public class ApplicationDbContex : DbContext
{
    private const string DATABASE = "Database";
    
    private readonly IConfiguration _configuration;
    public DbSet<Volunteer> Volunteers  => Set<Volunteer>(); 
    public DbSet<Pet> Pets => Set<Pet>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString(DATABASE));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    public ApplicationDbContex(IConfiguration configuration)
    {
         _configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContex).Assembly); 
    }

    private ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => {builder.AddConsole();});
    }
    
}