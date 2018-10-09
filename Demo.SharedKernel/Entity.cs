using System;

namespace Demo.SharedKernel
{
    public abstract class Entity<TId>
    {
        public virtual TId Id { get; set; }

        protected Entity()
        {
            if (typeof(TId) == typeof(Guid))
                Id = (TId)(object)Guid.NewGuid();
        }

        public Entity(TId id)
        {
            Id = id;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Entity<TId>;

            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;

            if (Equals(Id, default(TId)) || Equals(other.Id, default(TId)))
                return false;

            return Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}