using System;

namespace Demo.SharedKernel.Tests.TestData
{
   public class Car:Entity<Guid>
    {
        public string Name { get; set; }

        public Car()
        {
        }

        public Car(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"{Name} ({Id})";
        }
    }
}