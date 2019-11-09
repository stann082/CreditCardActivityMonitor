using Domain;
using Microsoft.Extensions.Logging;

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
                count++;
                Context.CardActivity.Add(newActivity);
            }

            Logger.LogInformation($"Added {count} card activity items...Skipping duplicate items");
            Context.SaveChanges();
        }

        #endregion

    }
}
