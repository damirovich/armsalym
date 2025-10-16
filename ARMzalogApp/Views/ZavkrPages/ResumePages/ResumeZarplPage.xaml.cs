namespace ARMzalogApp.Views.ZavkrPages.ResumePages;

public partial class ResumeZarplPage : ContentPage
{
    private string _pozn;
    public ResumeZarplPage(string pozn)
	{
		InitializeComponent();
        _pozn = pozn;
        poznLabel.Text = _pozn;

        BindingContext = this;

    }
}