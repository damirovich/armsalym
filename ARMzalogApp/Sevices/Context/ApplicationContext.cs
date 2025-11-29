namespace ARMzalogApp.Sevices.Context;

public sealed class ApplicationContext : IApplicationContext
{
    public int? CurrentZvPozn { get; private set; }
    public int? CurrentTypeClient { get; private set; }
    public string? CurrentPin { get; private set; }

    public void SetContext(int zvPozn, int typeClient, string pin)
    {
        CurrentZvPozn = zvPozn;
        CurrentTypeClient = typeClient;
        CurrentPin = pin;
    }

    public void Clear()
    {
        CurrentZvPozn = null;
        CurrentTypeClient = null;
        CurrentPin = null;
    }
}