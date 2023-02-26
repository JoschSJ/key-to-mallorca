using System.Text.Json.Serialization;

namespace key_to_mallorca_wasm.Models;

public class SpeciesList
{
    public List<PlantSpecies> Plant { get; set; }
    public List<BirdSpecies> Bird { get; set; }
}
public class PlantSpecies
{
    public string Group { get; set; }
    public string Family { get; set; }
    public string Genus { get; set; }
    [JsonPropertyName("species")] public string Species { get; set; }
    [JsonPropertyName("Commonname")] public string CommonName { get; set; }
    public string Description { get; set; }
    public string Photo1 { get; set; }
    public string Credit1 { get; set; }
    public string Photo2 { get; set; }
    public string Credit2 { get; set; }
    public string Photo3 { get; set; }
    public string Credit3 { get; set; }
    public string Subfamily { get; set; }
}

public class BirdSpecies
{
    public string Order { get; set; }
    public string Family { get; set; }
    public string Commonname { get; set; }
    public string Genus { get; set; }
    public string species { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string Photo1 { get; set; }
    public string Credit1 { get; set; }
    public string Photo2 { get; set; }
    public string Credit2 { get; set; }
}
