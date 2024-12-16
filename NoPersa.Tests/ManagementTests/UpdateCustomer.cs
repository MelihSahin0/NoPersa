using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NoPersa.Tests.DatabaseMemory;
using NoPersa.Tests.Misc;
using NoPersaService.Controllers;
using NoPersaService.Database;
using NoPersaService.DTOs.Management.RA;
using NoPersaService.Models;
using NoPersaService.Util;

namespace NoPersa.Tests.ManagementTests
{
    [TestClass]
    public class UpdateCustomer : ITest
    {
        private NoPersaDbContext context;
        private CustomerManagementController controller;
        private IMapper mapper;
        private ILogger<CustomerManagementController> logger;

        [TestInitialize]
        public void Setup()
        {
            DotNetEnv.Env.Load(@"..\..\..\..\.env");

            var services = new ServiceCollection();
            ProgramBuilder.RegisterAutoMapperProfiles(services);
            ProgramBuilder.RegisterFluentValidations(services);
            services.AddDbContext<NoPersaDbContext>(opt => opt.UseSqlite("DataSource=:memory:"));
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            context = serviceProvider.GetRequiredService<NoPersaDbContext>();
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            mapper = serviceProvider.GetRequiredService<IMapper>();
            logger = new Mock<ILogger<CustomerManagementController>>().Object;
            controller = new CustomerManagementController(logger, context, mapper);

            SeedTestData();
        }

        public void SeedTestData()
        {
            context.MonthlyOverviews.AddRange(StaticMonthlyOverview.GetMonthlyOverviews(DateTime.Today.Year, DateTime.Today.Month));
            context.SaveChanges();

            var dailyOverviews = StaticDailyOverviews.GetDailyOverview1(context.MonthlyOverviews.First(m => m.Id == 1));
            context.DailyOverviews.AddRange(dailyOverviews);
            context.SaveChanges();

            context.LightDiets.AddRange(StaticLightDiets.GetLightDiets());
            context.FoodWishes.AddRange(StaticFoodWishes.GetFoodWishes());
            context.BoxContents.AddRange(StaticBoxContents.GetBoxContents());
            context.PortionSizes.AddRange(StaticPortionSizes.GetPortionSizes());
            context.SaveChanges();

            List<CustomersLightDiet> customersLightDiets = [];
            customersLightDiets.AddRange(StaticCustomersLightDiets.GetCustomersLightDiets());
            context.BulkInsert(customersLightDiets);

            List<CustomersFoodWish> customersFoodWishes = [];
            customersFoodWishes.AddRange(StaticCustomersFoodWishes.GetCustomersFoodWishes());
            context.BulkInsert(customersFoodWishes);

            List<CustomersMenuPlan> customersMenuPlans = [];
            customersMenuPlans.AddRange(StaticCustomersMenuPlan.GetCustomersMenuPlan());
            context.BulkInsert(customersMenuPlans);

            context.BulkInsert(StaticDeliveryLocation.GetDeliveryLocations());
        }

        [TestMethod]
        [TestOrder(1)]
        public void UpdateCustomers()
        {
            List<Customer> customers = [.. context.Customers.AsNoTracking().Include(c => c.MonthlyOverviews).ThenInclude(m => m.DailyOverviews)
                                                                           .Include(w => w.Workdays).Include(h => h.Holidays)
                                                                           .Include(dl => dl.DeliveryLocation)
                                                                           .Include(a => a.Article)];
            Customer customer = customers.FirstOrDefault(c => c.Id == 1)!;
            customer.Name = "Customer 0";
            Article article = context.Articles.First(a => a.Id == 2);
            customer.ArticleId = article.Id;
            customer.Article = article;
            customer.CustomersLightDiets.Add(StaticCustomersLightDiets.GetCustomersLightDiets().First(c => c.CustomerId == customer.Id));
            customer.CustomersFoodWish.Add(StaticCustomersFoodWishes.GetCustomersFoodWishes().First(c => c.CustomerId == customer.Id));         

            List<LightDiet> lightDiets = [.. context.LightDiets.AsNoTracking()];
            List<FoodWish> foodWishes = [.. context.FoodWishes.AsNoTracking()];
            List<BoxContent> boxContents = [.. context.BoxContents.AsNoTracking()];
            List<PortionSize> portionSizes = [.. context.PortionSizes.AsNoTracking()];
            List<Article> articles = [.. context.Articles.AsNoTracking()];

            DTOCustomerOverview customerOverview = mapper.Map<DTOCustomerOverview>(customer);
            foreach (var lightDiet in customerOverview.LightDietOverviews ?? [])
            {
                lightDiet.Selected = true;
            }
            foreach (var foodWish in customerOverview.FoodWishesOverviews ?? [])
            {
                foodWish.Selected = true;
            }

            controller.UpdateCustomer(customerOverview);
            Customer dbCustomer = context.Customers.FirstOrDefault(c => c.Id == 1)!;

            Assert.AreEqual(customer.Name, dbCustomer.Name);
            Assert.AreEqual(lightDiets.Count, context.LightDiets.AsNoTracking().Count());
            Assert.AreEqual(foodWishes.Count, context.FoodWishes.AsNoTracking().Count());
            Assert.AreEqual(boxContents.Count, context.BoxContents.AsNoTracking().Count());
            Assert.AreEqual(portionSizes.Count, context.PortionSizes.AsNoTracking().Count());
            Assert.AreEqual(customer.CustomersLightDiets.Count, dbCustomer.CustomersLightDiets.Count);
            Assert.AreEqual(customer.CustomersFoodWish.Count, dbCustomer.CustomersFoodWish.Count);
            Assert.AreEqual(boxContents.Count, dbCustomer.CustomerMenuPlans.Count);
            Assert.AreEqual(articles.Count, context.Articles.AsNoTracking().Count());
        }

        [TestMethod]
        [TestOrder(1)]
        public void InsertCustomers()
        {
            List<Customer> customers = [.. context.Customers.AsNoTracking().Include(c => c.MonthlyOverviews).ThenInclude(m => m.DailyOverviews)
                                                                           .Include(w => w.Workdays).Include(h => h.Holidays)
                                                                           .Include(cld => cld.CustomersLightDiets)
                                                                           .Include(cmp => cmp.CustomerMenuPlans)];

            Customer customer = customers.FirstOrDefault(c => c.Id == 1)!;
            customer.Name = "Customer new";
            customer.Id = 0;

            List<LightDiet> lightDiets = [.. context.LightDiets.AsNoTracking()];
            List<FoodWish> foodWishes = [.. context.FoodWishes.AsNoTracking()];
            List<BoxContent> boxContents = [.. context.BoxContents.AsNoTracking()];
            List<PortionSize> portionSizes = [.. context.PortionSizes.AsNoTracking()];
            List<Article> articles = [.. context.Articles.AsNoTracking()];

            controller.InsertCustomer(mapper.Map<DTOCustomerOverview>(customer));
            Customer dbCustomer = context.Customers.FirstOrDefault(c => c.Name == "Customer new")!;

            Assert.AreEqual(customer.Name, dbCustomer.Name);
            Assert.AreEqual(lightDiets.Count, context.LightDiets.AsNoTracking().Count());
            Assert.AreEqual(foodWishes.Count, context.FoodWishes.AsNoTracking().Count());
            Assert.AreEqual(boxContents.Count, context.BoxContents.AsNoTracking().Count());
            Assert.AreEqual(portionSizes.Count, context.PortionSizes.AsNoTracking().Count());
            Assert.AreEqual(boxContents.Count, dbCustomer.CustomerMenuPlans.Count);
            Assert.AreEqual(articles.Count, context.Articles.AsNoTracking().Count());
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Database.CloseConnection();
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
