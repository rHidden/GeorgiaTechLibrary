using DataAccess.DAO.DAOIntefaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class GTLDbContext : DbContext
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public GTLDbContext(IDatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionFactory.CreateConnection());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasKey(b => b.ISBN);
            modelBuilder.Entity<DigitalItem>().HasKey(d => d.Id);
            modelBuilder.Entity<Library>().HasKey(l => l.Name);
            modelBuilder.Entity<BookInstance>().HasKey(bi => bi.Id);
            modelBuilder.Entity<User>().HasKey(u => u.SSN);
            modelBuilder.Entity<Loan>().HasKey(u => u.Id);
            modelBuilder.Entity<Staff>().HasKey(s => s.SSN);
            modelBuilder.Entity<Member>().HasKey(m => m.SSN);
            modelBuilder.Entity<Address>().HasNoKey();
        }

        public DbSet<Book> Book { get; set; }
        public DbSet<BookInstance> BookInstance { get; set; }
        public DbSet<DigitalItem> DigitalItem { get; set; }
        public DbSet<Library> Library { get; set; }
        public DbSet<Loan> Loan { get; set; }
        public DbSet<Member> Member { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<User> User { get; set; }
    }
}
