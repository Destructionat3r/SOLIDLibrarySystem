namespace SOLIDLibrarySystem
{
    public interface IInformation
    {
        string category { get; set; }
        string Title { get; set; }
        string Author { get; set; }
        string Publisher { get; set; }
        string DateOfPublication { get; set; }
        string ID { get; set; }
    }
}