using Microsoft.EntityFrameworkCore;

namespace QRService.Models.DBModels
{
    public partial class qrserviceContext : DbContext
    {
        public qrserviceContext()
        {
        }

        public qrserviceContext(DbContextOptions<qrserviceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Qrcodes> Qrcodes { get; set; }
        public virtual DbSet<Qrcodesadvertisments> Qrcodesadvertisments { get; set; }
        public virtual DbSet<Qrcodesusers> Qrcodesusers { get; set; }
        public virtual DbSet<Redeemauthorities> Redeemauthorities { get; set; }
        public virtual DbSet<Redeemedcodes> Redeemedcodes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=MySQL@123;database=qrservice", x => x.ServerVersion("8.0.20-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Qrcodes>(entity =>
            {
                entity.HasKey(e => e.QrCodeId)
                    .HasName("PRIMARY");

                entity.ToTable("qrcodes");

                entity.Property(e => e.QrCodeId).HasColumnName("qrCodeId");

                entity.Property(e => e.Details)
                    .HasColumnName("details")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ExpieryDate)
                    .HasColumnName("expieryDate")
                    .HasColumnType("date");

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("imageUrl")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.InstitutionId).HasColumnName("institutionId");

                entity.Property(e => e.RedeemLimit).HasColumnName("redeemLimit");

                entity.Property(e => e.ResourceName)
                    .HasColumnName("resourceName")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Qrcodesadvertisments>(entity =>
            {
                entity.ToTable("qrcodesadvertisments");

                entity.HasIndex(e => e.QrCodeId)
                    .HasName("qrCodeId");

                entity.Property(e => e.QrCodesAdvertismentsId).HasColumnName("qrCodesAdvertismentsId");

                entity.Property(e => e.AdvertismentId).HasColumnName("advertismentId");

                entity.Property(e => e.QrCodeId).HasColumnName("qrCodeId");

                entity.HasOne(d => d.QrCode)
                    .WithMany(p => p.Qrcodesadvertisments)
                    .HasForeignKey(d => d.QrCodeId)
                    .HasConstraintName("qrcodesadvertisments_ibfk_1");
            });

            modelBuilder.Entity<Qrcodesusers>(entity =>
            {
                entity.ToTable("qrcodesusers");

                entity.HasIndex(e => e.QrCodeId)
                    .HasName("qrCodeId");

                entity.Property(e => e.QrCodesUsersId).HasColumnName("qrCodesUsersId");

                entity.Property(e => e.QrCodeId).HasColumnName("qrCodeId");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.QrCode)
                    .WithMany(p => p.Qrcodesusers)
                    .HasForeignKey(d => d.QrCodeId)
                    .HasConstraintName("qrcodesusers_ibfk_1");
            });

            modelBuilder.Entity<Redeemauthorities>(entity =>
            {
                entity.ToTable("redeemauthorities");

                entity.Property(e => e.RedeemAuthoritiesId).HasColumnName("redeemAuthoritiesId");

                entity.Property(e => e.AuthorityId).HasColumnName("authorityId");

                entity.Property(e => e.InstitutionId).HasColumnName("institutionId");

                entity.Property(e => e.Pin)
                    .HasColumnName("pin")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Redeemedcodes>(entity =>
            {
                entity.HasKey(e => e.RedeemedCodeId)
                    .HasName("PRIMARY");

                entity.ToTable("redeemedcodes");

                entity.HasIndex(e => e.QrCodeId)
                    .HasName("qrCodeId");

                entity.Property(e => e.RedeemedCodeId).HasColumnName("redeemedCodeId");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("date");

                entity.Property(e => e.QrCodeId).HasColumnName("qrCodeId");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.QrCode)
                    .WithMany(p => p.Redeemedcodes)
                    .HasForeignKey(d => d.QrCodeId)
                    .HasConstraintName("redeemedcodes_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
