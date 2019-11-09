using Domain;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Service
{
    public class CreditCardService : ICreditCardService
    {

        #region Constructors

        public CreditCardService(CreditCardContext context)
        {
            Context = context;
            Context.Database.EnsureCreated();
        }

        #endregion

        #region Properties

        private CreditCardContext Context { get; set; }
        private ILogger<CreditCardService> Logger { get { return ApplicationLogger.CreateLogger<CreditCardService>(); } }

        #endregion

        #region Public Methods

        public void AddActivities(CardActivity[] newCardActivities)
        {
            int count = 0;
            foreach (CardActivity newActivity in newCardActivities)
            {
                // guard clause - skip the existing entry
                CardActivity existingCardActivity = Context.CardActivity.FirstOrDefault(ca => Matches(newActivity, ca));
                if (existingCardActivity != null)
                {
                    continue;
                }

                count++;
                Context.CardActivity.Add(newActivity);
            }

            Logger.LogInformation($"Added {count} new credit card activity items... Skipped {newCardActivities.Length - count} duplicate items");
            Context.SaveChanges();
        }

        #endregion

        #region Helper Methods

        private bool Matches(CardActivity newCardActivity, CardActivity existingCardActivity)
        {
            return newCardActivity.Date == existingCardActivity.Date &&
                   newCardActivity.Category == existingCardActivity.Category &&
                   newCardActivity.Amount == existingCardActivity.Amount &&
                   newCardActivity.Description == existingCardActivity.Description;
        }

        #endregion

    }
}
