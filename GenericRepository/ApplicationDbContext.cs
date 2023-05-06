using GenericRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace GenericRepository
{
	public class ApplicationDbContext:DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
		public DbSet<Person> People { get; set; }
	}
}
