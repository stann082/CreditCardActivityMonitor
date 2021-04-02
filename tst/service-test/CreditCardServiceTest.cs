using common;
using domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using service;
using System;

namespace service_test
{
    [TestClass]
    public class CreditCardServiceTest
    {

        [TestInitialize]
        public void SetUp()
        {
            string[] logArgs = new string[] { "--no-console-log", "--no-file-log" };
            ApplicationLogger.Singleton.Initialize(string.Empty, logArgs);

            Context = CreateDbContext();
            Service = new CreditCardService(Context);
        }

        private CreditCardContext Context;
        private ICreditCardService Service;

        [TestMethod]
        public void BlueSkyTest_AddActivities()
        {
            // pre-conditions
            Assert.AreEqual(0, Context.cardactivity.Local.Count);

            // exercise
            CardActivity cardActivity = new CardActivity();
            cardActivity.Amount = 2;
            cardActivity.Description = "Test description";
            cardActivity.Category = "test category";
            cardActivity.Date = DateTime.Today;
            Service.AddActivities(new CardActivity[] { cardActivity });

            // post-conditions
            Assert.AreEqual(1, Context.cardactivity.Local.Count);
        }

        [TestMethod]
        public void BlueSkyTest_AddActivities_SkipDuplicate()
        {
            // set-up
            CardActivity cardActivity1 = new CardActivity();
            cardActivity1.Amount = 2;
            cardActivity1.Description = "Test description";
            cardActivity1.Category = "test category";
            cardActivity1.Date = DateTime.Today;
            Service.AddActivities(new CardActivity[] { cardActivity1 });

            // pre-conditions
            Assert.AreEqual(1, Context.cardactivity.Local.Count);

            // exercise
            CardActivity cardActivity2 = new CardActivity();
            cardActivity2.Amount = 25;
            cardActivity2.Description = "New description";
            cardActivity2.Category = "New category";
            cardActivity2.Date = DateTime.Today;
            Service.AddActivities(new CardActivity[] { cardActivity1, cardActivity2 });

            // post-conditions
            Assert.AreEqual(2, Context.cardactivity.Local.Count);
        }

        #region Helper Methods

        private CreditCardContext CreateDbContext()
        {
            DbContextOptionsBuilder<CreditCardContext> builder = new DbContextOptionsBuilder<CreditCardContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            builder.EnableSensitiveDataLogging();
            return new CreditCardContext(builder.Options);
        }

        #endregion

    }
}
