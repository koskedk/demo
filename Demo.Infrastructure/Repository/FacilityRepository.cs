using Demo.Core;
using Demo.Core.Interfaces;

namespace Demo.Infrastructure.Repository
{
    public class FacilityRepository:BaseRepository<Facility,int>,IFacilityRepository
    {
        public FacilityRepository(DemoContext context) : base(context)
        {
        }
    }
}