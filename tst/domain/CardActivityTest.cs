using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace domain
{
    [TestClass]
    public class CardActivityTest
    {

        [TestInitialize]
        public void SetUp()
        {
            CardActivity = new CardActivity();
        }

        private CardActivity CardActivity;

        [TestMethod]
        public void BlueSkyTest()
        {
            // pre-conditions
            Assert.AreEqual(0.0, CardActivity.Amount);
            Assert.AreEqual(string.Empty, CardActivity.Category);
            Assert.AreEqual(DateTime.MinValue, CardActivity.Date);
            Assert.AreEqual(string.Empty, CardActivity.Description);

            // exercise
            CardActivity.Amount = 5.45;
            CardActivity.Category = "New Category";
            CardActivity.Date = DateTime.Today;
            CardActivity.Description = "test activity";

            // post-conditions
            Assert.AreEqual(5.45, CardActivity.Amount);
            Assert.AreEqual("New Category", CardActivity.Category);
            Assert.AreEqual(DateTime.Today, CardActivity.Date);
            Assert.AreEqual("test activity", CardActivity.Description);
        }

    }
}
