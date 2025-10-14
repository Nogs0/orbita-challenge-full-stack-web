using TurmaMaisA.Models;
using TurmaMaisA.Persistence;
using TurmaMaisA.Persistence.Repositories.Shared;

namespace TurmaMaisA.Persistence.Repositories.Organizations
{
    public class OrganizationRepository : BaseRepository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(AppDbContext context)
            : base(context)
        { }
    }
}
