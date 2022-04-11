using CreITBuy.Core.Contracts;
using CreITBuy.Core.Services;
using CreITBuy.Core.ViewModels.JobRequest;
using CreITBuy.Infrastructure.Data.Common;
using CreITBuy.Infrastructure.Data.Models;
using CreITBuy.Infrastructure.Data.Models.Enums;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreITBuy.Test
{
    public class JobRequestServiceTests
    {
        private ServiceProvider serviceProvider;
        private IJobRequestService jobRequestService;
        private InMemoryDbContext dbContext;
        private IRepo repo;
        [SetUp]
        public void Setup()
        {

            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
               .AddSingleton(sp => dbContext.CreateContext())
               .AddSingleton<IRepo, Repo>()
               .AddSingleton<IJobRequestService, JobRequestService>()
               .BuildServiceProvider();

            var repo = serviceProvider.GetService<IRepo>();

            this.repo = repo;
            jobRequestService = new JobRequestService(repo,new ValidationService());

        }
        [Test]
        public void AddTest()
        {
            var user1 = new User()
            {
                UserName = "Berbatov2000",
                LiveIn = "Bulgaria, Sofia",
                PasswordHash = "AQAAAAEAACcQAAAAEI94JA2QBE2Eb0avL0VafpCfd7n0eubhaNSmBf4Y5MoAetMwMoVot8Pmj9aHM6u1/g==",
                Email = "setolan@abv.bg",
                NormalizedEmail = "SETOLAN@ABV.BG",
                Job = Jobs.Developer,
                EmailConfirmed = true,
                LockoutEnabled = false,
            };
            var user2 = new User()
            {
                UserName = "Berbatov3000",
                LiveIn = "Bulgaria, Sofia",
                PasswordHash = "AQAAAAEAAffgfAAEI94JA2QBE2Eb0avL0VafpCfd7n0eubhaNSmBf4Y5MoAetMwMoVot8Pmj9adHM6u1/g==",
                Email = "setolan2@abv.bg",
                NormalizedEmail = "SETOLAN@ABV2.BG",
                Job = Jobs.Developer,
                EmailConfirmed = true,
                LockoutEnabled = false,
            };
            var jobRequest = new JobRequestViewModel()
            {
                Theme = "asfimmaf",
                Description = " annjfuiuqbawfbwfb fubwuebfb ub ubu bu biub ub ubub ub ub bobswobaf obobn onn n"
            };
            (bool isAdded, string errors) = jobRequestService.AddRequest(user1, user2, jobRequest);

            Assert.IsTrue(isAdded);
            Assert.IsNull(errors);
            Assert.AreEqual(repo.All<JobRequest>().Count(), 1);

            (bool isAdded2, string errors2) = jobRequestService.AddRequest(null, null, jobRequest);

            Assert.IsFalse(isAdded2);
            Assert.IsNotEmpty(errors2);
        }

        [Test]
        public void RemoveTest()
        {
            var user1 = new User()
            {
                UserName = "Berbatov2000",
                LiveIn = "Bulgaria, Sofia",
                PasswordHash = "AQAAAAEAACcQAAAAEI94JA2QBE2Eb0avL0VafpCfd7n0eubhaNSmBf4Y5MoAetMwMoVot8Pmj9aHM6u1/g==",
                Email = "setolan@abv.bg",
                NormalizedEmail = "SETOLAN@ABV.BG",
                Job = Jobs.Developer,
                EmailConfirmed = true,
                LockoutEnabled = false,
            };
            var user2 = new User()
            {
                UserName = "Berbatov3000",
                LiveIn = "Bulgaria, Sofia",
                PasswordHash = "AQAAAAEAAffgfAAEI94JA2QBE2Eb0avL0VafpCfd7n0eubhaNSmBf4Y5MoAetMwMoVot8Pmj9adHM6u1/g==",
                Email = "setolan2@abv.bg",
                NormalizedEmail = "SETOLAN@ABV2.BG",
                Job = Jobs.Developer,
                EmailConfirmed = true,
                LockoutEnabled = false,
            };
            var jobRequest = new JobRequestViewModel()
            {
                Theme = "asfimmaf",
                Description = " annjfuiuqbawfbwfb fubwuebfb ub ubu bu biub ub ubub ub ub bobswobaf obobn onn n"
            };
            jobRequestService.AddRequest(user1, user2, jobRequest);

            string requestId = repo.All<JobRequest>().FirstOrDefault().Id;
            (bool isRemoved, string errors)  = jobRequestService.Remove(requestId);
            Assert.IsTrue(isRemoved);
            Assert.IsNull(errors);
            Assert.AreEqual(repo.All<JobRequest>().Count(), 0);

            (bool isRemoved2, string errors2) = jobRequestService.Remove("asgfagags");
            Assert.IsFalse(isRemoved2);
            Assert.IsNotNull(errors2);
        }
    }
}
