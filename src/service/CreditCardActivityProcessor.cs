using Domain;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Service
{
    public class CreditCardActivityProcessor
    {

        #region Constructors

        public CreditCardActivityProcessor(ICreditCardService service)
        {
            Service = service;
        }

        #endregion

        #region Public Methods

        public void Process(string[] postedActivityFiles)
        {
            ProcessPostedPayments(postedActivityFiles);
            InsertData();
        }

        #endregion

        #region Properties

        private ILogger<CreditCardActivityProcessor> Logger { get { return ApplicationLogger.CreateLogger<CreditCardActivityProcessor>(); } }
        private List<CardActivityModel> PostedPayments { get; set; }
        private ICreditCardService Service { get; set; }

        #endregion

        #region Helper Methods

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
            Logger.LogInformation("Processing payments...");

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
                        activityModel.Description = $"{activityText[2]}, {activityText[3]}".Trim('"');
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
