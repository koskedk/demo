using MediatR;

namespace Demo.Core.Commands
{
    public class DeleteFacility:IRequest
    {
        public int Id { get; }
      
        public DeleteFacility(int id)
        {
            Id = id;
        }
    }
}