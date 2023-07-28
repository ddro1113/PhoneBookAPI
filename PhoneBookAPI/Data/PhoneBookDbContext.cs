using Microsoft.EntityFrameworkCore;
using PhoneBookAPI.Models;

namespace PhoneBookAPI.Data;

public class PhoneBookDbContext : DbContext
{
    public PhoneBookDbContext(DbContextOptions<PhoneBookDbContext> options) : base(options)
    {

    }

    public DbSet<Contact> Contacts { get; set; }

    public DbSet<Group> Groups { get; set; }
}
