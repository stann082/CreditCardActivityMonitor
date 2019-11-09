using System;

namespace Domain
{
    public class CardActivityModel
    {

        #region Constructors

        public CardActivityModel()
        {
            Category = "N/A";
            Description = string.Empty;
            RawAmount = string.Empty;
            RawDate = string.Empty;
        }

        #endregion

        #region Properties

        public double Amount { get { return ParseAmount(); } }

        public string Category { get; set; }

        public DateTime Date { get { return ParseDate(); } }
        public string Description { get; set; }

        public string RawAmount { get; set; }
        public string RawDate { get; set; }

        #endregion

        #region Overridden Methods

        public override string ToString()
        {
            return $"{Date.ToShortDateString()}: {Description} - ${Amount}";
        }

        #endregion

        #region Helper Methods

        private double ParseAmount()
        {
            if (!double.TryParse(RawAmount.Replace("$", string.Empty), out double value))
            {
                return 0;
            }

            return value;
        }

        private DateTime ParseDate()
        {
            if (!DateTime.TryParse(RawDate, out DateTime value))
            {
                return DateTime.MinValue;
            }

            return value;
        }

        #endregion

    }
}
