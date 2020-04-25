using System;
using Microsoft.EntityFrameworkCore;

namespace Instaq.Database.Storage.Mysql.Generated
{
    public partial class InstaqProdContext : DbContext
    {
        private string connectionString;

        public InstaqProdContext()
        {
            this.connectionString = "";
        }

        public InstaqProdContext(DbContextOptions<InstaqProdContext> options)
            : base(options)
        {
            this.connectionString = "";
        }

        public InstaqProdContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public virtual DbSet<Blacklist> Blacklist { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Debug> Debug { get; set; }
        public virtual DbSet<Feedback> Feedback { get; set; }
        public virtual DbSet<Itags> Itags { get; set; }
        public virtual DbSet<Locations> Locations { get; set; }
        public virtual DbSet<Mtags> Mtags { get; set; }
        public virtual DbSet<PhotoItagRel> PhotoItagRel { get; set; }
        public virtual DbSet<Photos> Photos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured && !String.IsNullOrEmpty(this.connectionString))
            {
                optionsBuilder.UseMySql(this.connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blacklist>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Name })
                    .HasName("PRIMARY");

                entity.ToTable("blacklist");

                entity.HasIndex(e => e.Id)
                    .HasName("id")
                    .IsUnique();

                entity.HasIndex(e => e.Name)
                    .HasName("name")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_bin");

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasColumnName("reason")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Table)
                    .IsRequired()
                    .HasColumnName("table")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.CustomerId })
                    .HasName("PRIMARY");

                entity.ToTable("customer");

                entity.HasIndex(e => e.CustomerId)
                    .HasName("customer_id");

                entity.HasIndex(e => e.Id)
                    .HasName("id")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Created)
                    .HasColumnName("created")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.FeedbackCount)
                    .HasColumnName("feedback_count")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Infos)
                    .IsRequired()
                    .HasColumnName("infos")
                    .HasColumnType("varchar(60)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.PhotosCount)
                    .HasColumnName("photos_count")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Debug>(entity =>
            {
                entity.ToTable("debug");

                entity.HasIndex(e => e.CustomerId)
                    .HasName("debugCustomerId");

                entity.HasIndex(e => e.Id)
                    .HasName("id")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Created)
                    .HasColumnName("created")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .HasColumnName("customer_id")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Data)
                    .IsRequired()
                    .HasColumnName("data")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Deleted).HasColumnName("deleted");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("feedback");

                entity.HasIndex(e => e.CustomerId)
                    .HasName("feedbackCustomerId");

                entity.HasIndex(e => e.Id)
                    .HasName("id")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Created)
                    .HasColumnName("created")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .HasColumnName("customer_id")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Data)
                    .IsRequired()
                    .HasColumnName("data")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DebugId)
                    .HasColumnName("debug_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Itags>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PRIMARY");

                entity.ToTable("itags");

                entity.HasIndex(e => e.Name)
                    .HasName("name")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_bin");

                entity.Property(e => e.OnBlacklist).HasColumnName("onBlacklist");

                entity.Property(e => e.Posts)
                    .HasColumnName("posts")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RefCount)
                    .HasColumnName("refCount")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Updated)
                    .HasColumnName("updated")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<Locations>(entity =>
            {
                entity.ToTable("locations");

                entity.HasIndex(e => e.Id)
                    .HasName("id")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Created)
                    .HasColumnName("created")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.HasPublicPage).HasColumnName("has_public_page");

                entity.Property(e => e.InstaId)
                    .HasColumnName("insta_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Lat)
                    .HasColumnName("lat")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Lng)
                    .IsRequired()
                    .HasColumnName("lng")
                    .HasColumnType("varchar(11)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ProfilePicUrl)
                    .IsRequired()
                    .HasColumnName("profile_pic_url")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Slug)
                    .IsRequired()
                    .HasColumnName("slug")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Mtags>(entity =>
            {
                entity.ToTable("mtags");

                entity.HasIndex(e => e.Id)
                    .HasName("id")
                    .IsUnique();

                entity.HasIndex(e => e.Name)
                    .HasName("name");

                entity.HasIndex(e => e.Shortcode)
                    .HasName("shortcode");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OnBlacklist).HasColumnName("onBlacklist");

                entity.Property(e => e.Score)
                    .HasColumnName("score")
                    .HasColumnType("float(11,9)");

                entity.Property(e => e.Shortcode)
                    .IsRequired()
                    .HasColumnName("shortcode")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnName("source")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<PhotoItagRel>(entity =>
            {
                entity.HasKey(e => new { e.Shortcode, e.Itag })
                    .HasName("PRIMARY");

                entity.ToTable("photo_itag_rel");

                entity.HasIndex(e => e.Itag)
                    .HasName("itagId");

                entity.HasIndex(e => e.Shortcode)
                    .HasName("shortcode");

                entity.Property(e => e.Shortcode)
                    .HasColumnName("shortcode")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Itag)
                    .HasColumnName("itag")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_bin");
            });

            modelBuilder.Entity<Photos>(entity =>
            {
                entity.HasKey(e => e.Shortcode)
                    .HasName("PRIMARY");

                entity.ToTable("photos");

                entity.HasIndex(e => e.Created)
                    .HasName("created");

                entity.HasIndex(e => e.LocationId)
                    .HasName("rel_photos_location");

                entity.HasIndex(e => e.Shortcode)
                    .HasName("imgId")
                    .IsUnique();

                entity.HasIndex(e => e.Status)
                    .HasName("status");

                entity.Property(e => e.Shortcode)
                    .HasColumnName("shortcode")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Created)
                    .HasColumnName("created")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Follower)
                    .HasColumnName("follower")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Following)
                    .HasColumnName("following")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LargeUrl)
                    .IsRequired()
                    .HasColumnName("largeUrl")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Likes)
                    .HasColumnName("likes")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("location_id")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Posts)
                    .HasColumnName("posts")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ThumbUrl)
                    .IsRequired()
                    .HasColumnName("thumbUrl")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Uploaded)
                    .HasColumnName("uploaded")
                    .HasColumnType("timestamp");

                entity.Property(e => e.User)
                    .IsRequired()
                    .HasColumnName("user")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("rel_photos_location");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
