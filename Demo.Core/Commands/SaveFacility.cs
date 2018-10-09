using MediatR;

namespace Demo.Core.Commands
{
    public class SaveFacility:IRequest
    {
        public int Id { get; }
        public string Name { get; }

        public SaveFacility(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}