using System;
using System.Collections.Generic;
using Application.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Application.Data;

public partial class ModelContext : IdentityDbContext<Account, IdentityRole<int>, int>
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Anime> Animes { get; set; }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Badge> Badges { get; set; }

    public virtual DbSet<Character> Characters { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Listelement> Listelements { get; set; }

    public virtual DbSet<Manga> Mangas { get; set; }

    public virtual DbSet<Medium> Media { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Studio> Studios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PKACCOUNT");

            entity.ToTable("ACCOUNT");

            entity.HasIndex(e => e.Email, "SYS_C00191762").IsUnique();

            entity.HasIndex(e => e.UserName, "SYS_C00191763").IsUnique();

            entity.Property(e => e.Id)
                .HasPrecision(10)
                .HasColumnName("Id");
            entity.Property(e => e.Accountprivilege)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'n' ")
                .HasColumnName("ACCOUNTPRIVILEGE");
            entity.Property(e => e.Createdate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("DATE")
                .HasColumnName("CREATEDATE");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Email");
            entity.Property(e => e.Imagelink)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("IMAGELINK");
            entity.Property(e => e.UserName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("UserName");
            entity.Property(e => e.NormalizedUserName)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("NormalizedUserName");
            entity.Property(e => e.NormalizedEmail)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("NormalizedEmail");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PasswordHash");
            entity.Property(e => e.SecurityStamp)
                .HasColumnName("SecurityStamp");
            entity.Property(e => e.ConcurrencyStamp)
                .HasColumnName("ConcurrencyStamp");


            entity.HasMany(d => d.Accountid1s).WithMany(p => p.Accountid2s)
                .UsingEntity<Dictionary<string, object>>(
                    "Friend",
                    r => r.HasOne<Account>().WithMany()
                        .HasForeignKey("Accountid1")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FKUSER1"),
                    l => l.HasOne<Account>().WithMany()
                        .HasForeignKey("Accountid2")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FKUSER2"),
                    j =>
                    {
                        j.HasKey("Accountid1", "Accountid2").HasName("PKFRIEND");
                        j.ToTable("FRIEND");
                        j.IndexerProperty<int>("Accountid1")
                            .HasPrecision(10)
                            .HasColumnName("ACCOUNTID1");
                        j.IndexerProperty<int>("Accountid2")
                            .HasPrecision(10)
                            .HasColumnName("ACCOUNTID2");
                    });

            entity.HasMany(d => d.Accountid2s).WithMany(p => p.Accountid1s)
                .UsingEntity<Dictionary<string, object>>(
                    "Friend",
                    r => r.HasOne<Account>().WithMany()
                        .HasForeignKey("Accountid2")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FKUSER2"),
                    l => l.HasOne<Account>().WithMany()
                        .HasForeignKey("Accountid1")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FKUSER1"),
                    j =>
                    {
                        j.HasKey("Accountid1", "Accountid2").HasName("PKFRIEND");
                        j.ToTable("FRIEND");
                        j.IndexerProperty<int>("Accountid1")
                            .HasPrecision(10)
                            .HasColumnName("ACCOUNTID1");
                        j.IndexerProperty<int>("Accountid2")
                            .HasPrecision(10)
                            .HasColumnName("ACCOUNTID2");
                    });

            entity.HasMany(d => d.Badgenames).WithMany(p => p.Accounts)
                .UsingEntity<Dictionary<string, object>>(
                    "Userbadge",
                    r => r.HasOne<Badge>().WithMany()
                        .HasForeignKey("Badgename")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FKBADGEUSER"),
                    l => l.HasOne<Account>().WithMany()
                        .HasForeignKey("Accountid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FKUSERBADGE"),
                    j =>
                    {
                        j.HasKey("Accountid", "Badgename").HasName("PKUSERBADGE");
                        j.ToTable("USERBADGE");
                        j.IndexerProperty<int>("Accountid")
                            .HasPrecision(10)
                            .HasColumnName("ACCOUNTID");
                        j.IndexerProperty<string>("Badgename")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("BADGENAME");
                    });

            entity.HasMany(d => d.Characters).WithMany(p => p.Accounts)
                .UsingEntity<Dictionary<string, object>>(
                    "Usercharacter",
                    r => r.HasOne<Character>().WithMany()
                        .HasForeignKey("Characterid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FKCHARACTERUSER"),
                    l => l.HasOne<Account>().WithMany()
                        .HasForeignKey("Accountid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FKUSERCHARACTER"),
                    j =>
                    {
                        j.HasKey("Accountid", "Characterid").HasName("PKUSERCHARACTER");
                        j.ToTable("USERCHARACTER");
                        j.IndexerProperty<int>("Accountid")
                            .HasPrecision(10)
                            .HasColumnName("ACCOUNTID");
                        j.IndexerProperty<long>("Characterid")
                            .HasPrecision(12)
                            .HasColumnName("CHARACTERID");
                    });
        });

        modelBuilder.Entity<Anime>(entity =>
        {
            entity.HasKey(e => e.Mediumid).HasName("PKANIMEID");

            entity.ToTable("ANIME");

            entity.Property(e => e.Mediumid)
                .HasPrecision(10)
                .ValueGeneratedNever()
                .HasColumnName("MEDIUMID");
            entity.Property(e => e.Studioname)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("STUDIONAME");
            entity.Property(e => e.Type)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasDefaultValueSql("'TV'")
                .HasColumnName("TYPE");

            entity.HasOne(d => d.Medium).WithOne(p => p.Anime)
                .HasForeignKey<Anime>(d => d.Mediumid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKANIMEID");

            entity.HasOne(d => d.StudionameNavigation).WithMany(p => p.Animes)
                .HasForeignKey(d => d.Studioname)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKNAMESTUDIO");
        });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PKAUTHOR");

            entity.ToTable("AUTHOR");

            entity.Property(e => e.Id)
                .HasPrecision(5)
                .HasColumnName("ID");
            entity.Property(e => e.Image)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("IMAGE");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Wikipedialink)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("WIKIPEDIALINK");
        });

        modelBuilder.Entity<Badge>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PKBADGE");

            entity.ToTable("BADGE");

            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Backgroundcolor)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("'ffffff' ")
                .HasColumnName("BACKGROUNDCOLOR");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Namecolor)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("'000000' ")
                .HasColumnName("NAMECOLOR");
        });

        modelBuilder.Entity<Character>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PKCHARACTER");

            entity.ToTable("CHARACTER");

            entity.Property(e => e.Id)
                .HasPrecision(12)
                .HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Image)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("IMAGE");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PKGENRE");

            entity.ToTable("GENRE");

            entity.Property(e => e.Name)
                .HasMaxLength(24)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<Listelement>(entity =>
        {
            entity.HasKey(e => new { e.Accountid, e.Mediumid }).HasName("PKLISTELEMENT");

            entity.ToTable("LISTELEMENT");

            entity.Property(e => e.Accountid)
                .HasPrecision(10)
                .HasColumnName("ACCOUNTID");
            entity.Property(e => e.Mediumid)
                .HasPrecision(10)
                .HasColumnName("MEDIUMID");
            entity.Property(e => e.Finishdate)
                .HasColumnType("DATE")
                .HasColumnName("FINISHDATE");
            entity.Property(e => e.Finishednumber)
                .HasPrecision(4)
                .HasDefaultValueSql("0 ")
                .HasColumnName("FINISHEDNUMBER");
            entity.Property(e => e.Mediumcomment)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("MEDIUMCOMMENT");
            entity.Property(e => e.Rating)
                .HasPrecision(2)
                .HasDefaultValueSql("NULL ")
                .HasColumnName("RATING");
            entity.Property(e => e.Startdate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("DATE")
                .HasColumnName("STARTDATE");
            entity.Property(e => e.Status)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasDefaultValueSql("'Watching' ")
                .HasColumnName("STATUS");

            entity.HasOne(d => d.Account).WithMany(p => p.Listelements)
                .HasForeignKey(d => d.Accountid)
                .HasConstraintName("FKELEMENTACCOUNTID");

            entity.HasOne(d => d.Medium).WithMany(p => p.Listelements)
                .HasForeignKey(d => d.Mediumid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKELEMENTMEDIUM");
        });

        modelBuilder.Entity<Manga>(entity =>
        {
            entity.HasKey(e => e.Mediumid).HasName("PKMANGAID");

            entity.ToTable("MANGA");

            entity.Property(e => e.Mediumid)
                .HasPrecision(10)
                .ValueGeneratedNever()
                .HasColumnName("MEDIUMID");
            entity.Property(e => e.Authorid)
                .HasPrecision(5)
                .HasColumnName("AUTHORID");
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("'Manga' ")
                .HasColumnName("TYPE");

            entity.HasOne(d => d.Author).WithMany(p => p.Mangas)
                .HasForeignKey(d => d.Authorid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKAUTHORID");

            entity.HasOne(d => d.Medium).WithOne(p => p.Manga)
                .HasForeignKey<Manga>(d => d.Mediumid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKMANGAID");
        });

        modelBuilder.Entity<Medium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PKMEDIUM");

            entity.ToTable("MEDIUM");

            entity.Property(e => e.Id)
                .HasPrecision(10)
                .HasColumnName("ID");
            entity.Property(e => e.Count)
                .HasPrecision(4)
                .HasDefaultValueSql("0 ")
                .HasColumnName("COUNT");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Poster)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("POSTER");
            entity.Property(e => e.Publishdate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("DATE")
                .HasColumnName("PUBLISHDATE");
            entity.Property(e => e.Status)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasDefaultValueSql("'Not finished'\n    ")
                .HasColumnName("STATUS");
            entity.Property(e => e.Type)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("TYPE");

            entity.HasMany(d => d.Characters).WithMany(p => p.Media)
                .UsingEntity<Dictionary<string, object>>(
                    "Mediumcharacter",
                    r => r.HasOne<Character>().WithMany()
                        .HasForeignKey("Characterid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FKCHARACTERMEDIUM"),
                    l => l.HasOne<Medium>().WithMany()
                        .HasForeignKey("Mediumid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FKMEDIUMCHARACTER"),
                    j =>
                    {
                        j.HasKey("Mediumid", "Characterid").HasName("PKMEDIUMCHARACTER");
                        j.ToTable("MEDIUMCHARACTER");
                        j.IndexerProperty<int>("Mediumid")
                            .HasPrecision(10)
                            .HasColumnName("MEDIUMID");
                        j.IndexerProperty<long>("Characterid")
                            .HasPrecision(12)
                            .HasColumnName("CHARACTERID");
                    });

            entity.HasMany(d => d.Genrenames).WithMany(p => p.Idmedia)
                .UsingEntity<Dictionary<string, object>>(
                    "Mediumgenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("Genrename")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FKGENREMEDIUM"),
                    l => l.HasOne<Medium>().WithMany()
                        .HasForeignKey("Idmedium")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FKMEDIUMGENRE"),
                    j =>
                    {
                        j.HasKey("Idmedium", "Genrename").HasName("PKMEDIUMGENRE");
                        j.ToTable("MEDIUMGENRE");
                        j.IndexerProperty<int>("Idmedium")
                            .HasPrecision(10)
                            .HasColumnName("IDMEDIUM");
                        j.IndexerProperty<string>("Genrename")
                            .HasMaxLength(24)
                            .IsUnicode(false)
                            .HasColumnName("GENRENAME");
                    });

            entity.HasMany(d => d.Idmedium1s).WithMany(p => p.Idmedium2s)
                .UsingEntity<Dictionary<string, object>>(
                    "Mediumconnection",
                    r => r.HasOne<Medium>().WithMany()
                        .HasForeignKey("Idmedium1")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FKMEDIUMCONNECTION1"),
                    l => l.HasOne<Medium>().WithMany()
                        .HasForeignKey("Idmedium2")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FKMEDIUMCONNECTION2"),
                    j =>
                    {
                        j.HasKey("Idmedium1", "Idmedium2").HasName("PKMEDIUMCONNECTION");
                        j.ToTable("MEDIUMCONNECTION");
                        j.IndexerProperty<int>("Idmedium1")
                            .HasPrecision(10)
                            .HasColumnName("IDMEDIUM1");
                        j.IndexerProperty<int>("Idmedium2")
                            .HasPrecision(10)
                            .HasColumnName("IDMEDIUM2");
                    });

            entity.HasMany(d => d.Idmedium2s).WithMany(p => p.Idmedium1s)
                .UsingEntity<Dictionary<string, object>>(
                    "Mediumconnection",
                    r => r.HasOne<Medium>().WithMany()
                        .HasForeignKey("Idmedium2")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FKMEDIUMCONNECTION2"),
                    l => l.HasOne<Medium>().WithMany()
                        .HasForeignKey("Idmedium1")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FKMEDIUMCONNECTION1"),
                    j =>
                    {
                        j.HasKey("Idmedium1", "Idmedium2").HasName("PKMEDIUMCONNECTION");
                        j.ToTable("MEDIUMCONNECTION");
                        j.IndexerProperty<int>("Idmedium1")
                            .HasPrecision(10)
                            .HasColumnName("IDMEDIUM1");
                        j.IndexerProperty<int>("Idmedium2")
                            .HasPrecision(10)
                            .HasColumnName("IDMEDIUM2");
                    });
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => new { e.Accountid, e.Mediumid }).HasName("PKREVIEW");

            entity.ToTable("REVIEW");

            entity.Property(e => e.Accountid)
                .HasPrecision(10)
                .HasColumnName("ACCOUNTID");
            entity.Property(e => e.Mediumid)
                .HasPrecision(10)
                .HasColumnName("MEDIUMID");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Feeling)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasDefaultValueSql("'Recommended' ")
                .HasColumnName("FEELING");
            entity.Property(e => e.Postdate)
                .HasDefaultValueSql("CURRENT_DATE ")
                .HasColumnType("DATE")
                .HasColumnName("POSTDATE");

            entity.HasOne(d => d.Account).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.Accountid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKUSERREVIEW");

            entity.HasOne(d => d.Medium).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.Mediumid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKREVIEWMEDIUM");
        });

        modelBuilder.Entity<Studio>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PKSTUDIO");

            entity.ToTable("STUDIO");

            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Wikipedialink)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("WIKIPEDIALINK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
