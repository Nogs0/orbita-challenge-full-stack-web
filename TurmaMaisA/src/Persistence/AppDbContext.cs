using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TurmaMaisA.Models;
using TurmaMaisA.Models.Courses;
using TurmaMaisA.Models.Enrollments;
using TurmaMaisA.Models.Organizations;
using TurmaMaisA.Models.Shared;
using TurmaMaisA.Models.Users;
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
            SetIsDeletedOnDeletedEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetOrganizationIdOnAddedEntities()
        {
            var addedEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added && e.Entity is IMustHaveOrganizationId);

            foreach (var entry in addedEntities)
            {
                var entityMustHaveOrganizationId = (IMustHaveOrganizationId)entry.Entity;
                if (entityMustHaveOrganizationId.OrganizationId == Guid.Empty)
                {
                    entityMustHaveOrganizationId.OrganizationId = OrganizationId;
                }
            }
        }

        private void SetIsDeletedOnDeletedEntities()
        {
            var deletedEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Deleted && e.Entity is ISoftDelete);

            foreach (var entry in deletedEntities)
            {
                var entitySoftDelete = (ISoftDelete)entry.Entity;
                entitySoftDelete.DeletedAt = DateTime.Now;
                entry.State = EntityState.Modified;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");

                Expression? filterBody = null;

                if (typeof(IMustHaveOrganizationId).IsAssignableFrom(entityType.ClrType))
                {
                    // e.OrganizationId == this.OrganizationId
                    var tenantFilter = Expression.Equal(
                        Expression.Property(Expression.Convert(parameter, typeof(IMustHaveOrganizationId)), "OrganizationId"),
                        Expression.Property(Expression.Constant(this), "OrganizationId")
                    );
                    filterBody = tenantFilter;
                }

                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    // e.DeletedAt == null
                    var softDeleteFilter = Expression.Equal(
                        Expression.Property(Expression.Convert(parameter, typeof(ISoftDelete)), "DeletedAt"),
                        Expression.Constant(null)
                    );

                    if (filterBody != null)
                        filterBody = Expression.AndAlso(filterBody, softDeleteFilter);
                    else
                        filterBody = softDeleteFilter;
                }

                if (filterBody != null)
                {
                    var finalLambda = Expression.Lambda(filterBody, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(finalLambda);
                }
            }

            modelBuilder.Entity<Student>()
                .HasIndex(s => new { s.RA, s.Cpf, s.OrganizationId })
                .IsUnique();
        }
    }
}
