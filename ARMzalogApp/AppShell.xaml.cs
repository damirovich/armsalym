using ARMzalogApp.Views;
using ARMzalogApp.Views.ZavkrPages;
using ARMzalogApp.Views.DogkrPages;
using ARMzalogApp.Views.ZavkrPages.ResumePages;

namespace ARMzalogApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutingPages();
        }
        private void RegisterRoutingPages()
        {
            Routing.RegisterRoute("SetPinPage", typeof(SetPinPage));
            Routing.RegisterRoute("PinVerificationPage", typeof(PinVerificationPage));
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("RegistPage", typeof(RegistPage));
            //Routing.RegisterRoute("ChosePage", typeof(ChosePage));
            Routing.RegisterRoute("HomePage", typeof(HomePage));
            Routing.RegisterRoute("ListDogkr", typeof(ListDogkr)); 
            Routing.RegisterRoute("NewZavkrPage", typeof(NewZavkrPage));
            Routing.RegisterRoute("CreateLoanZarp", typeof(CreateLoanZarp));
            Routing.RegisterRoute("OpiuPage", typeof(OpiuPage));
            Routing.RegisterRoute("ViewKredDog", typeof(ViewKredDog));
            Routing.RegisterRoute("Forma3Page", typeof(Forma3Page));
            Routing.RegisterRoute("UkzPage", typeof(UkzPage));
            Routing.RegisterRoute("AnketaPage", typeof(AnketaPage));
            Routing.RegisterRoute("ResumeZarplPage", typeof(ResumeZarplPage));
        }
    }
}
