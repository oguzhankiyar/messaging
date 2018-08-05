using Microsoft.EntityFrameworkCore;
using OK.Messaging.Common.Entities;
using OK.Messaging.Common.Enumerations;

namespace OK.Messaging.DataAccess.EntityFramework.DataContexts
{
    public class MessagingDataContext : DbContext
    {
        public virtual DbSet<ActivityEntity> Activities { get; set; }

        public virtual DbSet<ActivityTypeEntity> ActivityTypes { get; set; }

        public virtual DbSet<LogEntity> Logs { get; set; }

        public virtual DbSet<MessageEntity> Messages { get; set; }

        public virtual DbSet<UserBlockEntity> UserBlocks { get; set; }

        public virtual DbSet<UserEntity> Users { get; set; }

        public MessagingDataContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Build Activities Table

            modelBuilder.Entity<ActivityEntity>()
                        .ToTable("Activities")
                        .HasKey(x => x.Id);

            modelBuilder.Entity<ActivityEntity>()
                        .Property(x => x.Id)
                        .ValueGeneratedOnAdd()
                        .IsRequired();

            modelBuilder.Entity<ActivityEntity>()
                        .Property(x => x.UserId)
                        .IsRequired();

            modelBuilder.Entity<ActivityEntity>()
                        .HasOne(x => x.User)
                        .WithMany()
                        .HasForeignKey(nameof(ActivityEntity.UserId))
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ActivityEntity>()
                        .Property(x => x.TypeId)
                        .IsRequired();

            modelBuilder.Entity<ActivityEntity>()
                        .Property(x => x.Description)
                        .IsRequired();

            modelBuilder.Entity<ActivityEntity>()
                        .Property(x => x.IsDeleted)
                        .IsRequired();

            modelBuilder.Entity<ActivityEntity>()
                        .Property(x => x.CreatedDate)
                        .IsRequired();

            modelBuilder.Entity<ActivityEntity>()
                        .Property(x => x.UpdatedDate)
                        .IsRequired();

            #endregion

            #region Build ActivityTypes Table

            modelBuilder.Entity<ActivityTypeEntity>()
                        .ToTable("ActivityTypes")
                        .HasKey(x => x.Id);

            modelBuilder.Entity<ActivityTypeEntity>()
                        .Property(x => x.Id)
                        .ValueGeneratedOnAdd()
                        .IsRequired();

            modelBuilder.Entity<ActivityTypeEntity>()
                        .Property(x => x.Description)
                        .IsRequired();

            modelBuilder.Entity<ActivityTypeEntity>()
                        .HasData(
                            new ActivityTypeEntity() { Id = (int)ActivityTypeEnum.SuccessLogin, Description = ActivityTypeEnum.SuccessLogin.ToString() },
                            new ActivityTypeEntity() { Id = (int)ActivityTypeEnum.InvalidLogin, Description = ActivityTypeEnum.InvalidLogin.ToString() },
                            new ActivityTypeEntity() { Id = (int)ActivityTypeEnum.BlockUser, Description = ActivityTypeEnum.BlockUser.ToString() },
                            new ActivityTypeEntity() { Id = (int)ActivityTypeEnum.UnblockUser, Description = ActivityTypeEnum.UnblockUser.ToString() }
                        );

            #endregion

            #region Build Logs Table

            modelBuilder.Entity<LogEntity>()
                        .ToTable("Logs")
                        .HasKey(x => x.Id);

            modelBuilder.Entity<LogEntity>()
                        .Property(x => x.Id)
                        .ValueGeneratedOnAdd()
                        .IsRequired();

            modelBuilder.Entity<LogEntity>()
                        .Property(x => x.Level)
                        .IsRequired();

            modelBuilder.Entity<LogEntity>()
                        .Property(x => x.Thread)
                        .IsRequired();

            modelBuilder.Entity<LogEntity>()
                        .Property(x => x.Channel)
                        .IsRequired();

            modelBuilder.Entity<LogEntity>()
                        .Property(x => x.RequestId)
                        .IsRequired();

            modelBuilder.Entity<LogEntity>()
                        .Property(x => x.UserId)
                        .IsRequired(false);

            modelBuilder.Entity<LogEntity>()
                        .HasOne(x => x.User)
                        .WithMany()
                        .HasForeignKey(nameof(LogEntity.UserId))
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LogEntity>()
                        .Property(x => x.Source)
                        .IsRequired();

            modelBuilder.Entity<LogEntity>()
                        .Property(x => x.Message)
                        .IsRequired();

            modelBuilder.Entity<LogEntity>()
                        .Property(x => x.Data)
                        .IsRequired(false);

            modelBuilder.Entity<LogEntity>()
                        .Property(x => x.Exception)
                        .IsRequired(false);

            modelBuilder.Entity<LogEntity>()
                        .Property(x => x.IPAddress)
                        .IsRequired(false);

            modelBuilder.Entity<LogEntity>()
                        .Property(x => x.UserAgent)
                        .IsRequired(false);

            modelBuilder.Entity<LogEntity>()
                        .Property(x => x.CreatedDate)
                        .IsRequired();

            modelBuilder.Entity<LogEntity>()
                        .Property(x => x.UpdatedDate)
                        .IsRequired();

            #endregion

            #region Build Messages Table

            modelBuilder.Entity<MessageEntity>()
                        .ToTable("Messages")
                        .HasKey(x => x.Id);

            modelBuilder.Entity<MessageEntity>()
                        .Property(x => x.Id)
                        .ValueGeneratedOnAdd()
                        .IsRequired();

            modelBuilder.Entity<MessageEntity>()
                        .Property(x => x.FromUserId)
                        .IsRequired();

            modelBuilder.Entity<MessageEntity>()
                        .HasOne(x => x.FromUser)
                        .WithMany()
                        .HasForeignKey(nameof(MessageEntity.FromUserId))
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MessageEntity>()
                        .Property(x => x.ToUserId)
                        .IsRequired();

            modelBuilder.Entity<MessageEntity>()
                        .HasOne(x => x.ToUser)
                        .WithMany()
                        .HasForeignKey(nameof(MessageEntity.ToUserId))
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MessageEntity>()
                        .Property(x => x.Content)
                        .IsRequired();

            modelBuilder.Entity<MessageEntity>()
                        .Property(x => x.IsDeleted)
                        .IsRequired();

            modelBuilder.Entity<MessageEntity>()
                        .Property(x => x.CreatedDate)
                        .IsRequired();

            modelBuilder.Entity<MessageEntity>()
                        .Property(x => x.UpdatedDate)
                        .IsRequired();

            #endregion

            #region Build UserBlocks Table

            modelBuilder.Entity<UserBlockEntity>()
                        .ToTable("UserBlocks")
                        .HasKey(x => x.Id);

            modelBuilder.Entity<UserBlockEntity>()
                        .Property(x => x.Id)
                        .ValueGeneratedOnAdd()
                        .IsRequired();

            modelBuilder.Entity<UserBlockEntity>()
                        .Property(x => x.UserId)
                        .IsRequired();

            modelBuilder.Entity<UserBlockEntity>()
                        .HasOne(x => x.User)
                        .WithMany()
                        .HasForeignKey(nameof(UserBlockEntity.UserId))
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserBlockEntity>()
                        .Property(x => x.BlockedUserId)
                        .IsRequired();

            modelBuilder.Entity<UserBlockEntity>()
                        .HasOne(x => x.BlockedUser)
                        .WithMany()
                        .HasForeignKey(nameof(UserBlockEntity.BlockedUserId))
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserBlockEntity>()
                        .Property(x => x.IsDeleted)
                        .IsRequired();

            modelBuilder.Entity<UserBlockEntity>()
                        .Property(x => x.CreatedDate)
                        .IsRequired();

            modelBuilder.Entity<UserBlockEntity>()
                        .Property(x => x.UpdatedDate)
                        .IsRequired();

            #endregion

            #region Build Users Table

            modelBuilder.Entity<UserEntity>()
                        .ToTable("Users")
                        .HasKey(x => x.Id);

            modelBuilder.Entity<UserEntity>()
                        .Property(x => x.Id)
                        .ValueGeneratedOnAdd()
                        .IsRequired();

            modelBuilder.Entity<UserEntity>()
                        .Property(x => x.Username)
                        .IsRequired();

            modelBuilder.Entity<UserEntity>()
                        .Property(x => x.Password)
                        .IsRequired();

            modelBuilder.Entity<UserEntity>()
                        .Property(x => x.FullName)
                        .IsRequired();

            modelBuilder.Entity<UserEntity>()
                        .Property(x => x.IsDeleted)
                        .IsRequired();

            modelBuilder.Entity<UserEntity>()
                        .Property(x => x.CreatedDate)
                        .IsRequired();

            modelBuilder.Entity<UserEntity>()
                        .Property(x => x.UpdatedDate)
                        .IsRequired();

            #endregion
        }
    }
}