namespace DnDEncounterGenerator.Configuration;

public class EncounterGeneratorConfiguration : IEncounterGeneratorConfiguration
{
    public string? ConnectionString { get; set; }
    public string? DatabaseName { get; set; }
    public string? CollectionName { get; set; }
}