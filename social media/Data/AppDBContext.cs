using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using social_media.Data.Models;

namespace social_media.Data
{
    public class AppDBContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Report> Reports { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .Property(p => p.FullName)
                .HasColumnType("nvarchar(500)");

            modelBuilder.Entity<User>()
                .Property(p => p.UserName)
                .HasColumnType("nvarchar(500)");

            modelBuilder.Entity<User>()
                .HasMany(u => u.Stories)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Like>()
                .HasKey(p => new { p.UserId, p.PostId });

            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(p => p.Likes)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Comment>()
                .HasOne(l => l.User)
                .WithMany(p => p.Comments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Favorite>()
                .HasKey(p => new { p.UserId, p.PostId });

            modelBuilder.Entity<Favorite>()
                .HasOne(l => l.User)
                .WithMany(p => p.Favorites)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Favorite>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Favorites)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.Cascade);




            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");


            modelBuilder.Entity<FriendRequest>()
                .HasOne(p => p.Sender)
                .WithMany()
                .HasForeignKey(p => p.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FriendRequest>()
                .HasOne(p => p.Reciver)
                .WithMany()
                .HasForeignKey(p => p.ReciverId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Friendship>()
                .HasOne(p => p.Sender)
                .WithMany()
                .HasForeignKey(p => p.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Friendship>()
                .HasOne(p => p.Reciver)
                .WithMany()
                .HasForeignKey(p => p.ReciverId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Report>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Report>()
                .HasOne(l => l.User)
                .WithMany(p => p.Reports)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Report>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Reports)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<User>()
            //    .HasIndex(p => p.Email);
            modelBuilder.Entity<ChatMessage>()
                .HasOne(p => p.Sender)
                .WithMany()
                .HasForeignKey(p => p.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChatMessage>()
                .HasOne(p => p.Reciver)
                .WithMany()
                .HasForeignKey(p => p.ReciverId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
