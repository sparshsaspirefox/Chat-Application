using ChatHubApi.Models;
using ChatHubApi.Models.GroupsModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatHubApi.Context
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {

     

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasOne(s => s.Sender)
                .WithMany()
                .HasForeignKey(s => s.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(s => s.Receiver)
                .WithMany()
                .HasForeignKey(s => s.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
               .HasOne(s => s.Sender)
               .WithMany()
               .HasForeignKey(s => s.SenderId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(s => s.Receiver)
                .WithMany()
                .HasForeignKey(s => s.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FriendShip>()
               .HasOne(s => s.Friend)
               .WithMany()
               .HasForeignKey(s => s.FriendId)
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<FriendShip>()
              .HasOne(s => s.User)
              .WithMany()
              .HasForeignKey(s => s.UserId)
              .OnDelete(DeleteBehavior.Restrict);

            //group
            modelBuilder.Entity<GroupMessage>()
             .HasOne(s => s.Group)
             .WithMany()
             .HasForeignKey(s => s.GroupId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GroupMessage>()
             .HasOne(s => s.Sender)
             .WithMany()
             .HasForeignKey(s => s.SenderId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserGroupMatching>()
             .HasOne(s => s.Group)
             .WithMany()
             .HasForeignKey(s => s.GroupId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserGroupMatching>()
             .HasOne(s => s.User)
             .WithMany()
             .HasForeignKey(s => s.UserId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
             .HasOne(s => s.Group)
             .WithMany()
             .HasForeignKey(s => s.GroupId)
             .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(modelBuilder);

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<FriendShip> FriendShips { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        // group
        public DbSet<GroupMessage> GroupMessages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroupMatching> UserGroupMatchings { get; set; }
    }
}
