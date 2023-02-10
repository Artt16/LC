using LC.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LC.Data
{
	public class LCDbContext:DbContext
	{
		public LCDbContext() => Database.EnsureCreated();
		public DbSet<LinkModel> Links { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseMySql("server=localhost;user=root;password=artt16130;database=linkscuttertest;",
                new MariaDbServerVersion(new Version(10, 0, 3)));
			}
		}
	}
}
