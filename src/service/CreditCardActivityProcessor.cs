using common;
using domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace service
{
    public class CreditCardActivityProcessor
    {

        #region Constants


        #endregion

        #region Constructors

        public CreditCardActivityProcessor(ICreditCardService service)
        {
            Service = service;
        }

        #endregion

        #region Properties

        private string ArchiveDir { get; set; }
        private string ArchiveRootDir => DirectoryProvider.Singleton.ArchiveRootDir;

        private string DownloadsDir => DirectoryProvider.Singleton.DownloadsDir;

        private IApplicationLogger Logger => ApplicationLogger.Singleton;

        private List<CardActivityModel> PostedPayments { get; set; }

        private ICreditCardService Service { get; set; }

        #endregion

        #region Public Methods

        public void Process()
        {
            if (!Directory.Exists(DownloadsDir))
            {
                Logger.LogError(DownloadsDir);
                return;
            }

            string[] activityFiles = Directory.GetFiles(DownloadsDir, "Discover-*.csv");
            if (activityFiles.Length == 0)
            {
                Logger.LogWarn("No pending or posted activity files were found...Exiting the app");
                return;
            }

            ProcessPostedPayments(activityFiles);
            InsertData();

            InitializeArchiveDirectory();
            foreach (string activityFile in activityFiles)
            {
                Logger.LogInfo($"Moving {activityFile}");
                ArchiveFile(activityFile);
            }
        }

        #endregion

        #region Helper Methods

        private void ArchiveFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            string fileName = Path.GetFileName(filePath);
            File.Move(filePath, Path.Combine(ArchiveDir, fileName));
        }

        private void InitializeArchiveDirectory()
        {
            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            ArchiveDir = Path.Combine(ArchiveRootDir, timeStamp);
            if (Directory.Exists(ArchiveDir))
            {
                return;
            }

            Directory.CreateDirectory(ArchiveDir);
        }

        private CardActivity CreateActivity(CardActivityModel model)
        {
            CardActivity cardActivity = new CardActivity();
            cardActivity.CopyFrom(model);
            return cardActivity;
        }

        private void InsertData()
        {
            IEnumerable<CardActivityModel> allModels = PostedPayments.OrderBy(pp => pp.Date);
            if (!allModels.Any())
            {
                return;
            }

            IEnumerable<CardActivity> cardActivities = allModels.Select(m => CreateActivity(m));
            Service.AddActivities(cardActivities.ToArray());
        }

        private void ProcessPostedPayments(string[] postedActivityFiles)
        {
            Logger.LogInfo("Processing payments...");

            PostedPayments = new List<CardActivityModel>();

            foreach (string recentActivityFile in postedActivityFiles)
            {
                string[] lines = File.ReadAllLines(recentActivityFile);
                foreach (string line in lines.Skip(1))
                {
                    string[] activityText = line.Split(',');
                    CardActivityModel activityModel = new CardActivityModel();

                    if (activityText.Length == 5)
                    {
                        activityModel.RawDate = activityText[1];
                        activityModel.Description = activityText[2].Trim('"');
                        activityModel.RawAmount = activityText[3];
                        activityModel.Category = activityText[4].Trim('"');
                    }
                    else if (activityText.Length > 5)
                    {
                        activityModel.RawDate = activityText[1];
                        activityModel.Description = $"{activityText[2]},{activityText[3]}".Trim('"');
                        activityModel.RawAmount = activityText[4];
                        activityModel.Category = activityText[5].Trim('"');
                    }

                    PostedPayments.Add(activityModel);
                }
            }
        }

        #endregion

    }
}
