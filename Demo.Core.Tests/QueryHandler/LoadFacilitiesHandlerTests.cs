using System;
using System.Linq;
using Demo.Core.Interfaces;
using Demo.Core.Queries;
using Demo.Core.QueryHandler;
using Demo.Infrastructure;
using Demo.Infrastructure.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Demo.Core.Tests.QueryHandler
{
    [TestFixture]
    public class LoadFacilitiesHandlerTests
    {
        private ServiceProvider _serviceProvider;
        private IMediator _mediator;
        private DemoContext _context;

        [OneTimeSetUp]
        public void Init()
        {      
            _serviceProvider = new ServiceCollection()
                .AddDbContext<DemoContext>(o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()))
                .AddScoped<IFacilityRepository, FacilityRepository>()
                .AddMediatR(typeof(LoadFacilitiesHandler))
                .BuildServiceProvider();

            _context = _serviceProvider.GetService<DemoContext>();
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
           
            _mediator = _serviceProvider.GetService<IMediator>();
        }
        
        [Test]
        public void should_load_facilities()
        {
            var facilities = _mediator.Send(new LoadFacilities()).Result.ToList();
            Assert.True(facilities.Any());

            foreach (var facility in facilities)
                Console.WriteLine(facility);
        }
    }
}