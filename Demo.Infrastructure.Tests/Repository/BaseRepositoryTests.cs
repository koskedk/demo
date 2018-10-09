using System;
using System.Collections.Generic;
using System.Linq;
using Demo.Infrastructure.Tests.TestData;
using Demo.SharedKernel.Tests.TestData;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Demo.Infrastructure.Tests.Repository
{
    [TestFixture]
    public class BaseRepositoryTests
    {
        private DbContextOptions<CarContext> _options;
        private CarContext _context;
        private CarRepository _carRepository;
        private List<Car> _cars;

        [OneTimeSetUp]
        public void Init()
        {
            _cars = Builder<Car>.CreateListOfSize(2).Build().ToList();
           
            _options = new DbContextOptionsBuilder<CarContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [SetUp]
        public void SetUp()
        {
            _context = new CarContext(_options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _context.AddRange(_cars);
            _context.SaveChanges();
            
            _carRepository = new CarRepository(_context);
        }

        [Test]
        public void should_Get_All()
        {
            var cars = _carRepository.GetAllAsync().Result;
            Assert.True(cars.Any());
            foreach (var car in cars)
                Console.WriteLine(car);
        }
        
        [Test]
        public void should_SaveOrUpdate_New()
        {
            var newCar=new Car("Skoda");
            _carRepository.CreateOrUpdateAsync(newCar).Wait();
            _carRepository.SaveAsync().Wait();
            
            var cars = _carRepository.GetAllAsync().Result;
            Assert.True(cars.Any(x=>x.Id==newCar.Id));
            Console.WriteLine(newCar);
        }
        
        [Test]
        public void should_SaveOrUpdate_Edit()
        {
            var car = _cars.First();
            car.Name = "XXX";
            _carRepository.CreateOrUpdateAsync(car).Wait();
            _carRepository.SaveAsync().Wait();
            
            var cars = _carRepository.GetAllAsync().Result;
            var savedCar = cars.FirstOrDefault(x => x.Id == car.Id);
            Assert.AreEqual("XXX",savedCar.Name);
            Console.WriteLine(savedCar);
        }
        
        [Test]
        public void should_Remove()
        {
            var id = _cars.Last().Id;
            _carRepository.RemoveAsync(id).Wait();
            _carRepository.SaveAsync().Wait();
            
            var cars = _carRepository.GetAllAsync().Result;
            Assert.False(cars.Any(x=>x.Id.Equals(id)));
        }
        
    }
}