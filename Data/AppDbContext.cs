using System;
using Microsoft.EntityFrameworkCore;
using RunGroupSocialMedia.Models;

namespace RunGroupSocialMedia.Data
{
	public class AppDbContext : DbContext
	{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
			 
		}

        public DbSet<Race> Races { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}

