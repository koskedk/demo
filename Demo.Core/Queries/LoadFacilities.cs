using System.Collections.Generic;
using MediatR;

namespace Demo.Core.Queries
{
    public class LoadFacilities:IRequest<IEnumerable<Facility>>
    {
        
    }
}