using Microsoft.EntityFrameworkCore;
using My.QuickCampus.Entities;

namespace My.QuickCampus.Data
{
    public class SqliteDbContext : DbContext
    {
        public SqliteDbContext(DbContextOptions<SqliteDbContext> options)
          : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            #region Entity config
            builder.Entity<Student>(entity =>
               {
                   entity.ToTable("Student");

                   entity.HasKey(u => u.StudentId)
                       .HasName("PK_Student_StudentId");

                   // NO AUTO-INCREMENT for GradeId

                   entity.Property(u => u.Name)
                     .IsRequired()
                     .HasMaxLength(50);

                   entity.Property(u => u.DisplayName)
                      .IsRequired()
                      .HasMaxLength(100);

               });



            builder.Entity<Grade>(entity =>
            {
                entity.ToTable("Grade");

                entity.HasKey(u => u.GradeId)
                    .HasName("PK_Grade_GradeId");

                // NO AUTO-INCREMENT for GradeId

                entity.HasIndex(u => u.StudentId)
                    .HasDatabaseName("IX_Grade_StudentId");

                entity.Property(u => u.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(u => u.Section)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            builder.Entity<Homework>(entity =>
            {
                entity.ToTable("Homework");

                entity.HasKey(u => u.HomeworkId)
                    .HasName("PK_Homework_HomeworkId");

                // auto-increment for HomeworkId
                entity.Property(u => u.HomeworkId)
                   .IsRequired()
                   .ValueGeneratedOnAdd();

                entity.Property(u => u.QuickCampusId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(u => u.Title)
                    .IsRequired()
                    .HasMaxLength(100);

            });

            builder.Entity<Assignment>(entity =>
            {
                entity.ToTable("Assignment");

                // auto-increment for AssignmentId
                entity.HasKey(u => u.AssignmentId)
                    .HasName("PK_Assignment_AssignmentId");

                entity.Property(u => u.AssignmentId)
                  .IsRequired()
                  .ValueGeneratedOnAdd();

                entity.Property(u => u.QuickCampusId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(u => u.Title)
                    .IsRequired()
                    .HasMaxLength(100);

            });

            builder.Entity<MediaFile>(entity =>
            {
                entity.ToTable("MediaFile");

                entity.HasKey(u => u.MediaFileId)
                    .HasName("PK_MediaFile_MediaFileId");

                // auto-increment for MediaFileId
                entity.Property(u => u.MediaFileId)
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                entity.Property(u => u.FileExtension)
                   .IsRequired()
                   .HasMaxLength(10);

                entity.Property(u => u.QuickCampusFileName)
                   .IsRequired()
                   .HasMaxLength(200);

                entity.Property(u => u.MediaType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(u => u.FileName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(u => u.FilePath)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            builder.Entity<QuickCampusSync>(entity =>
            {
                entity.ToTable("QuickCampusSync");

                // auto-increment for AssignmentId
                entity.HasKey(u => u.QuickCampusSyncId)
                    .HasName("PK_QuickCampusSync_QuickCampusSyncId");

                entity.Property(u => u.QuickCampusSyncId)
                  .IsRequired()
                  .ValueGeneratedOnAdd();

            });


            #endregion


            builder.Entity<Grade>()
              .HasOne(entity => entity.Student)
              .WithMany(user => user.Grades)
              .HasForeignKey(entity => entity.StudentId) // Explicit foreign key name added
              .HasConstraintName("FK_Grade_Student_StudentId")
              .IsRequired();

            builder.Entity<Assignment>()
              .HasOne(entity => entity.Grade)
              .WithMany(his => his.Assignments)
              .HasForeignKey(entity => entity.GradeId) // Explicit foreign key name added
              .HasConstraintName("FK_Assignment_Grade_GradeId")
              .IsRequired(false);

            builder.Entity<Homework>()
              .HasOne(entity => entity.Grade)
              .WithMany(his => his.Homeworks)
              .HasForeignKey(entity => entity.GradeId) // Explicit foreign key name added
              .HasConstraintName("FK_Homework_Grade_GradeId")
              .IsRequired(false);

            builder.Entity<MediaFile>()
               .HasOne(entity => entity.Homework)
              .WithMany(his => his.MediaFiles)
              .HasForeignKey(entity => entity.MediaFileId) // Explicit foreign key name added
              .HasConstraintName("FK_MediaFile_Homework_MediaFileId")
              .IsRequired();

            builder.Entity<MediaFile>()
             .HasOne(entity => entity.Assignment)
            .WithMany(his => his.MediaFiles)
            .HasForeignKey(entity => entity.MediaFileId) // Explicit foreign key name added
            .HasConstraintName("FK_MediaFile_Assignment_MediaFileId")
            .IsRequired();
        }


        public DbSet<Student> Students { get; set; }

        public DbSet<Grade> Grades { get; set; }

        public DbSet<Homework> Homeworks { get; set; }

        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<MediaFile> MediaFiles { get; set; }

        public DbSet<QuickCampusSync> QuickCampusSyncs { get; set; }

    }
}
