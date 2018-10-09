using System;
using Demo.SharedKernel.Tests.TestData;
using NUnit.Framework;

namespace Demo.SharedKernel.Tests
{
    [TestFixture]
    public class EntityTests
    {
        [Test]
        public void should_have_default_Id_Value()
        {
            var subaru=new Car("Subaru");
            var toyota= new Car("Toyota");
            
            Assert.AreNotEqual(subaru.Id,Guid.Empty);
            Assert.AreNotEqual(toyota.Id,Guid.Empty);
            Assert.AreNotEqual(subaru,toyota);
            
            Console.WriteLine(subaru);
            Console.WriteLine(toyota);
        }
    }
}