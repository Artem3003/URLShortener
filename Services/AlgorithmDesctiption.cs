namespace URLShortener.Services;

public class AlgorithmDescriptionService
{
    private string _description = "This is the initial description of the URL shortener algorithm.";

    public string GetDescription() => _description;

    public void SetDescription(string newDescription)
    {
        _description = newDescription;
    }
}