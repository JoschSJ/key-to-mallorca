namespace key_to_mallorca_wasm.Models;

public interface ISpeciesDetails
{
    string? Name { get; }
    string? CommonName { get; }
    string? Family { get; }
    string? Image { get; }
    string? ImageCaption { get; }
    string? Description { get; }

    Task GetPlantSpecies(string name);
    // add note
    // add to faves

}