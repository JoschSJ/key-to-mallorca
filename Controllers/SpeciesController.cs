using key_to_mallorca_wasm.Models;
using key_to_mallorca_wasm.Services;
using Microsoft.AspNetCore.Components;

namespace key_to_mallorca_wasm.Controllers;

public class SpeciesController: ISpeciesDetails
{
    public string? Name => _plant != null ? $"{_plant.Genus} {_plant.Species}" : null;
    public string? CommonName => _plant?.CommonName;
    public string? Family => _plant?.Family;
    public string? Image => _plant?.Photo1;
    public string? ImageCaption => _plant?.Credit1;
    public string? Description => _plant?.Description;

    [Inject]
    private SpeciesService SpeciesService { get; set; }

    private SpeciesList? _speciesList;
    private PlantSpecies? _plant;

    public SpeciesController(SpeciesService speciesService)
    {
        SpeciesService = speciesService;
    }

    public async Task GetPlantSpecies(string name)
    {
        name = name.ToLower();
        var nameSplit = name.Split('_');
        _speciesList ??= await SpeciesService.GetSpeciesList();
        _plant = _speciesList.Plant.Find(
            o => o.Genus.ToLower() == nameSplit[0]
                 && o.Species.ToLower() == nameSplit[1]);
    }
}