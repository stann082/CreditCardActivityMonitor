using common;
using domain;
using System.Linq;

namespace service
{
    public class CreditCardService : AbstractBase, ICreditCardService
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
        private IApplicationLogger Logger { get { return GetApplicationLogger<CreditCardService>(); } }

        #endregion

        #region Public Methods

        public void AddActivities(CardActivity[] cardActivities)
        {
            int count = 0;
            foreach (CardActivity newActivity in cardActivities)
            {
                // guard clause - skip the existing entry
                CardActivity existingCardActivity = Context.cardactivity.FirstOrDefault(ca => Matches(newActivity, ca));
                if (existingCardActivity != null)
                {
                    continue;
                }

                count++;
                Context.cardactivity.Add(newActivity);
            }

            Logger.LogInfo($"Added {count} new credit card activity items...Skipped {cardActivities.Length - count} duplicate items");
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
