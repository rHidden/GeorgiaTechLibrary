using GeorgiaTechLibrary.DbContext;
using GeorgiaTechLibrary.DTOs;
using GeorgiaTechLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace DbContextNamespace
{
    public class GTLDbContext: DbContext
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
            modelBuilder.Entity<BookDTO>().HasKey(b => b.ISBN);
            modelBuilder.Entity<DigitalItemDTO>().HasKey(d => d.Id);
            modelBuilder.Entity<LibraryDTO>().HasKey(l => l.Name);
            modelBuilder.Entity<BookInstanceDTO>().HasKey(bi => bi.Id);
            modelBuilder.Entity<UserDTO>().HasKey(u => u.SSN);
            modelBuilder.Entity<LoanDTO>().HasKey(u => u.LoanId);
            modelBuilder.Entity<StaffDTO>().HasKey(s => s.UserSSN);
            modelBuilder.Entity<MemberDTO>().HasKey(m => m.UserSSN);

            modelBuilder.Entity<DigitalItemLibraryDTO>().HasNoKey();
        }

        public DbSet<BookDTO> Book { get; set; }
        public DbSet<BookInstanceDTO> BookInstance { get; set; }
        public DbSet<DigitalItemDTO> DigitalItem { get; set; }
        public DbSet<DigitalItemLibraryDTO> DigitalItemLibrary { get; set; }
        public DbSet<LibraryDTO> Library { get; set; }
        public DbSet<LoanDTO> Loan { get; set; }
        public DbSet<MemberDTO> Member { get; set; }
        public DbSet<StaffDTO> Staff { get; set; }
        public DbSet<UserDTO> User { get; set; }
    }
}
