using common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using service;
using service_test.mock;
using System;
using System.IO;
using System.Text;

namespace domain
{
    [TestClass]
    public class CreditCardActivityProcessorTest
    {

        [TestInitialize]
        public void SetUp()
        {
            ApplicationLogger.Singleton = new MockApplicationLogger();
            DirectoryProvider.Singleton = new MockDirectoryProvider();

            Service = new MockCreditCardService();
            Processor = new CreditCardActivityProcessor(Service);
        }

        private string DownloadsDir => DirectoryProvider.Singleton.DownloadsDir;
        private CreditCardActivityProcessor Processor;
        private MockCreditCardService Service;

        #region Blue Sky Tests

        [TestMethod]
        public void BlueSkyTest_Process()
        {
            // set-up
            InitializeFiles();

            // pre-conditions
            Assert.AreEqual(0, Service.Activities.Count);

            // exercise
            Processor.Process();

            // post-conditions
            Assert.AreEqual(3, Service.Activities.Count);

            Assert.AreEqual(new DateTime(2019, 11, 6), Service.Activities[0].Date);
            Assert.AreEqual("HEB, #4543", Service.Activities[0].Description);
            Assert.AreEqual("Supermarkets", Service.Activities[0].Category);
            Assert.AreEqual(43.26, Service.Activities[0].Amount);
            Assert.AreEqual(ActivityType.Purchase, Service.Activities[0].Type);

            Assert.AreEqual(new DateTime(2019, 11, 6), Service.Activities[1].Date);
            Assert.AreEqual("INTERNET PAYMENT", Service.Activities[1].Description);
            Assert.AreEqual("Payments and Credits", Service.Activities[1].Category);
            Assert.AreEqual(1000, Service.Activities[1].Amount);
            Assert.AreEqual(ActivityType.Payment, Service.Activities[1].Type);

            Assert.AreEqual(new DateTime(2019, 11, 7), Service.Activities[2].Date);
            Assert.AreEqual("Amazon", Service.Activities[2].Description);
            Assert.AreEqual("Merchandise", Service.Activities[2].Category);
            Assert.AreEqual(15.27, Service.Activities[2].Amount);
            Assert.AreEqual(ActivityType.Purchase, Service.Activities[2].Type);
        }

        [TestMethod]
        public void BlueSkyTest_Process_NoFilesToProcess()
        {
            // pre-conditions
            Assert.AreEqual(0, Service.Activities.Count);

            // exercise
            Processor.Process();

            // post-conditions
            Assert.AreEqual(0, Service.Activities.Count);
        }

        #endregion

        #region Helper Methods

        private void EnsureDirectoryExists()
        {
            if (Directory.Exists(DownloadsDir))
            {
                return;
            }

            Directory.CreateDirectory(DownloadsDir);
        }

        private void InitializeFiles()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Trans. Date,Post Date,Description,Amount,Category");
            sb.AppendLine("11/07/2019,11/07/2019,\"Amazon\",15.27,\"Merchandise\"");
            sb.AppendLine("11/06/2019,11/06/2019,\"HEB, #4543\",43.26,\"Supermarkets\"");
            sb.AppendLine("11/06/2019,11/06/2019,\"INTERNET PAYMENT\",-1000.00,\"Payments and Credits\"");

            EnsureDirectoryExists();

            string mockFilePath = Path.Combine(DownloadsDir, "Discover-RecentActivity.csv");
            File.WriteAllText(mockFilePath, sb.ToString());
        }

        #endregion

    }
}
