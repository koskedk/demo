using System;
using Demo.Infrastructure.Repository;
using Demo.SharedKernel.Tests.TestData;

namespace Demo.Infrastructure.Tests.TestData
{
    public class CarRepository:BaseRepository<Car,Guid>, ICarRepository
    {
        public CarRepository(CarContext context) : base(context)
        {
        }
    }
}