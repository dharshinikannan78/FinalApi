using Microsoft.EntityFrameworkCore;
using RegistrationApllication.Modal;

namespace RegistrationApllication.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }
        public DbSet<RegistrationModelClass> RegistrationDetail { get; set; }

        public DbSet<UserModelClass> AdminLogin { get; set; }

        public DbSet<FileAttachmentModelClass> FileAttachment { get; set; }

        public DbSet<TokenRequest> TokenDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegistrationModelClass>().ToTable("candidatedetails");
            modelBuilder.Entity<UserModelClass>().ToTable("userlogin");
            modelBuilder.Entity<FileAttachmentModelClass>().ToTable("attachments");
            modelBuilder.Entity<TokenRequest>().ToTable("tokendetails");


        }
    }
}
