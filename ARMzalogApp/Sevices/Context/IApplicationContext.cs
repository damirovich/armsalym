namespace ARMzalogApp.Sevices.Context;

public interface IApplicationContext
{
    int? CurrentZvPozn { get; }
    int? CurrentTypeClient { get; }
    string? CurrentPin { get; }

    void SetContext(int zvPozn, int typeClient, string pin);
    void Clear();
}
