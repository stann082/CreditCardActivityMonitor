using domain;
using service;
using System.Collections.Generic;

namespace service_test.mock
{
    public class MockCreditCardService : ICreditCardService
    {

        #region Helper Methods

        public MockCreditCardService()
        {
            Activities = new List<CardActivity>();
        }

        #endregion

        #region Properties

        public List<CardActivity> Activities { get; set; }

        #endregion

        #region Public Methods

        public void AddActivities(CardActivity[] cardActivities)
        {
            Activities.AddRange(cardActivities);
        }

        #endregion

    }
}
