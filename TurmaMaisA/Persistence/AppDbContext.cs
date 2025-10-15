using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TurmaMaisA.Models;
using TurmaMaisA.Models.Shared;
using TurmaMaisA.Persistence.Interfaces;

namespace TurmaMaisA.Persistence
{
    public class AppDbContext : IdentityUserContext<User>, IUnitOfWork
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public Guid OrganizationId { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetOrganizationIdOnAddedEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetOrganizationIdOnAddedEntities()
        {
            var addedEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added);

            foreach (var entry in addedEntities)
            {
                if (entry.Entity is IMustHaveOrganizationId entityWithOrganization)
                {
                    if (entityWithOrganization.OrganizationId == Guid.Empty)
                    {
                        entityWithOrganization.OrganizationId = OrganizationId;
                    }
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IMustHaveOrganizationId).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(ConvertToExp(entityType.ClrType));
                }
            }

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.RA)
                .IsUnique();

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.Cpf)
                .IsUnique();
        }

        private LambdaExpression ConvertToExp(Type type)
        {
            var parameter = Expression.Parameter(type, "e");
            var body = Expression.Equal(
                Expression.Property(Expression.Convert(parameter, typeof(IMustHaveOrganizationId)), "OrganizationId"),
                Expression.Property(Expression.Constant(this), "OrganizationId")
            );
            return Expression.Lambda(body, parameter);
        }
    }
}
