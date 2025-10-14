using TurmaMaisA.Models;
using TurmaMaisA.Persistence;
using TurmaMaisA.Repositories.Shared;

namespace TurmaMaisA.Repositories.Organizations
{
    public class OrganizationRepository : BaseRepository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(AppDbContext context)
            : base(context)
        { }
    }
}
