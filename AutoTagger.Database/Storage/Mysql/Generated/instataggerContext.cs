using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AutoTagger.Database
{
    public partial class InstataggerContext : DbContext
    {
        public InstataggerContext()
        {
        }

        public InstataggerContext(DbContextOptions<InstataggerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Blacklist> Blacklist { get; set; }
        public virtual DbSet<Debug> Debug { get; set; }
        public virtual DbSet<Itags> Itags { get; set; }
        public virtual DbSet<Locations> Locations { get; set; }
        public virtual DbSet<Mtags> Mtags { get; set; }
        public virtual DbSet<PhotoItagRel> PhotoItagRel { get; set; }
        public virtual DbSet<Photos> Photos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var ip   = Environment.GetEnvironmentVariable("instatagger_mysql_ip");
                var user = Environment.GetEnvironmentVariable("instatagger_mysql_user");
                var pw   = Environment.GetEnvironmentVariable("instatagger_mysql_pw");
                var db   = Environment.GetEnvironmentVariable("instatagger_mysql_db");
                optionsBuilder.UseMySql($"Server={ip};User Id={user};Password={pw};Database={db};TreatTinyAsBoolean=false");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blacklist>(entity =>
            {
                entity.ToTable("blacklist");

                entity.HasIndex(e => e.Id)
                    .HasName("id")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(40)");

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasColumnName("reason")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.Table)
                    .IsRequired()
                    .HasColumnName("table")
                    .HasColumnType("varchar(10)");
            });

            modelBuilder.Entity<Debug>(entity =>
            {
                entity.ToTable("debug");

                entity.HasIndex(e => e.Id)
                    .HasName("id")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Created)
                    .HasColumnName("created")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Data)
                    .IsRequired()
                    .HasColumnName("data")
                    .HasColumnType("text");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)");
            });

            modelBuilder.Entity<Itags>(entity =>
            {
                entity.HasKey(e => e.Name);

                entity.ToTable("itags");

                entity.HasIndex(e => e.Name)
                    .HasName("name")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.OnBlacklist)
                    .HasColumnName("onBlacklist")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Posts)
                    .HasColumnName("posts")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RefCount)
                    .HasColumnName("refCount")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Updated)
                    .HasColumnName("updated")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
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
                    .HasColumnType("int(11)");

                entity.Property(e => e.Created)
                    .HasColumnName("created")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.HasPublicPage)
                    .HasColumnName("has_public_page")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.InstaId)
                    .HasColumnName("insta_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Lat)
                    .HasColumnName("lat")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Lng)
                    .IsRequired()
                    .HasColumnName("lng")
                    .HasColumnType("varchar(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("text");

                entity.Property(e => e.ProfilePicUrl)
                    .IsRequired()
                    .HasColumnName("profile_pic_url")
                    .HasColumnType("text");

                entity.Property(e => e.Slug)
                    .IsRequired()
                    .HasColumnName("slug")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Mtags>(entity =>
            {
                entity.ToTable("mtags");

                entity.HasIndex(e => e.Id)
                    .HasName("id")
                    .IsUnique();

                entity.HasIndex(e => e.Name)
                    .HasName("name");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(30)");

                entity.Property(e => e.OnBlacklist)
                    .HasColumnName("onBlacklist")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Score)
                    .HasColumnName("score")
                    .HasColumnType("float(11,9)");

                entity.Property(e => e.Shortcode)
                    .IsRequired()
                    .HasColumnName("shortcode")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnName("source")
                    .HasColumnType("varchar(30)");
            });

            modelBuilder.Entity<PhotoItagRel>(entity =>
            {
                entity.ToTable("photo_itag_rel");

                entity.HasIndex(e => e.Itag)
                    .HasName("itagId");

                entity.HasIndex(e => e.Shortcode)
                    .HasName("shortcode");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Itag)
                    .IsRequired()
                    .HasColumnName("itag")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Shortcode)
                    .IsRequired()
                    .HasColumnName("shortcode")
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<Photos>(entity =>
            {
                entity.HasKey(e => e.Shortcode);

                entity.ToTable("photos");

                entity.HasIndex(e => e.LocationId)
                    .HasName("rel_photos_location");

                entity.HasIndex(e => e.Shortcode)
                    .HasName("imgId")
                    .IsUnique();

                entity.Property(e => e.Shortcode)
                    .HasColumnName("shortcode")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Created)
                    .HasColumnName("created")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
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
                    .HasColumnType("text");

                entity.Property(e => e.Likes)
                    .HasColumnName("likes")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("location_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Posts)
                    .HasColumnName("posts")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ThumbUrl)
                    .IsRequired()
                    .HasColumnName("thumbUrl")
                    .HasColumnType("text");

                entity.Property(e => e.Uploaded)
                    .HasColumnName("uploaded")
                    .HasColumnType("timestamp");

                entity.Property(e => e.User)
                    .IsRequired()
                    .HasColumnName("user")
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("rel_photos_location");
            });
        }
    }
}
