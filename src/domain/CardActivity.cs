using System;

namespace Domain
{
    public class CardActivity
    {

        #region Constructors

        public CardActivity()
        {
            Id = Guid.NewGuid().ToString();
            Amount = 0.0;
            Category = string.Empty;
            Date = DateTime.MinValue;
            Description = string.Empty;
        }

        #endregion

        #region Properties

        public double Amount { get; set; }

        public string Category { get; set; }

        public DateTime Date { get; set; }
        public string Description { get; set; }

        public string Id { get; set; }

        public ActivityType Type { get; set; }

        #endregion

        #region Public Methods

        public void CopyFrom(CardActivityModel model)
        {
            Date = model.Date;
            Amount = Math.Abs(model.Amount);
            Category = model.Category;
            Description = model.Description;
            Type = model.Amount >= 0 ? ActivityType.Purchase : ActivityType.Payment;
        }

        #endregion

        #region Override Methods

        public override string ToString()
        {
            return $"{Date.ToShortDateString()} - {Description} - {Category} - {Amount}";
        }

        #endregion

    }
}
