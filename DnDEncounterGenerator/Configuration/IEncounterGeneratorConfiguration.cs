namespace DnDEncounterGenerator.Configuration
{
    public interface IEncounterGeneratorConfiguration
    {
        string? ConnectionString { get; set; }

        string? DatabaseName { get; set; }

        string? CollectionName { get; set; }
    }
}
