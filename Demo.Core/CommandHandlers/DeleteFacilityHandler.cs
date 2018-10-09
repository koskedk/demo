using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.Core.Commands;
using Demo.Core.Interfaces;
using MediatR;
using Serilog;

namespace Demo.Core.CommandHandlers
{
    public class DeleteFacilityHandler:IRequestHandler<DeleteFacility>
    {
        private readonly IFacilityRepository _repository;

        public DeleteFacilityHandler(IFacilityRepository repository)
        {
            _repository = repository;
        }


        public async Task<Unit> Handle(DeleteFacility request, CancellationToken cancellationToken)
        {
            try
            {
                await _repository.RemoveAsync(request.Id);
                await _repository.SaveAsync();
            }
            catch (Exception e)
            {
                Log.Error(e,"Error on DeleteFacility");
                throw;
            }

            return Unit.Value;
        }
    }
}