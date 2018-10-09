using Demo.SharedKernel;

namespace Demo.Core
{
    public class Facility : Entity<int>
    {
        public string Name { get; set; }

        private Facility()
        {
            
        }

        public Facility(int id, string name) : base(id)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"{Id}-{Name}";
        }
    }
}