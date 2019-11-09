using Service;

namespace Interface
{
    public class ConsoleApplication
    {

        #region Constructors

        public ConsoleApplication(ICreditCardService creditCardService)
        {
            CreditCardService = creditCardService;
        }

        #endregion

        #region Properties

        private ICreditCardService CreditCardService { get; set; }

        #endregion

        #region Application Starting Point

        public void Run()
        {
            CreditCardActivityProcessor processor = new CreditCardActivityProcessor(CreditCardService);
            processor.Process();
        }

        #endregion

    }
}
