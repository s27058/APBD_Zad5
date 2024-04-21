using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Animals;

[ApiController]
[Route("/api/animals")]
public class AnimalController : ControllerBase
{
    private readonly IAnimalService _animalService;
    public AnimalController(IAnimalService animalService)
    {
        _animalService = animalService;
    }
    
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAllAnimals([FromQuery] string orderBy)
    {
        var animals = _animalService.GetAllAnimals(orderBy);
        return Ok(animals);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult CreateAnimal([FromBody] CreateAnimalDTO dto)
    {
        var success = _animalService.AddAnimal(dto);
        return success ? StatusCode(StatusCodes.Status201Created) : StatusCode(StatusCodes.Status409Conflict);
    }

    [HttpPut("{idAnimal:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult UpdateAnimal([FromRoute] int idAnimal, [FromBody] UpdateAnimalDTO dto)
    {
        if (!_animalService.CheckIfAnimalExists(idAnimal)) return StatusCode(StatusCodes.Status404NotFound);
        _animalService.UpdateAnimal(idAnimal, dto);
        return StatusCode(StatusCodes.Status204NoContent);
    }
    
    [HttpDelete("{idAnimal:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult DeleteAnimal([FromRoute] int idAnimal)
    {
        if (!_animalService.CheckIfAnimalExists(idAnimal)) return StatusCode(StatusCodes.Status404NotFound);
        _animalService.DeleteAnimal(idAnimal);
        return StatusCode(StatusCodes.Status204NoContent);
    }
}