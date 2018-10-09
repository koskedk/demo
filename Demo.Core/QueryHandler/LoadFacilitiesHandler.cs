using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.Core.Interfaces;
using Demo.Core.Queries;
using MediatR;
using Serilog;

namespace Demo.Core.QueryHandler
{
    public class LoadFacilitiesHandler:IRequestHandler<LoadFacilities,IEnumerable<Facility>>
    {
        private readonly IFacilityRepository _facilityRepository;

        public LoadFacilitiesHandler(IFacilityRepository facilityRepository)
        {
            _facilityRepository = facilityRepository;
        }

        public async Task<IEnumerable<Facility>> Handle(LoadFacilities request, CancellationToken cancellationToken)
        {
            try
            {
                var facilities=await _facilityRepository.GetAllAsync();
                return facilities;
            }
            catch (Exception e)
            {
                Log.Error(e,"Error on DeleteFacility");
                throw;
            }
        }
    }
}