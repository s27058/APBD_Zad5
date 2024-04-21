namespace WebApplication1.Animals;

public interface IAnimalService
{
    public IEnumerable<Animal> GetAllAnimals(string orderBy);
    public bool AddAnimal(CreateAnimalDTO dto);
    public bool CheckIfAnimalExists(int id);
    public bool UpdateAnimal(int idAnimal, UpdateAnimalDTO dto);
    public bool DeleteAnimal(int id);
}

public class AnimalService : IAnimalService
{
    private readonly IAnimalRepository _animalRepository;
    public AnimalService(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }
    
    public IEnumerable<Animal> GetAllAnimals(string orderBy)
    {
        return _animalRepository.FetchAllAnimals(orderBy);
    }

    public bool AddAnimal(CreateAnimalDTO dto)
    {
        return _animalRepository.CreateAnimal(dto.Name, dto.Description, dto.Category, dto.Area);
    }

    public bool CheckIfAnimalExists(int id)
    {
        return _animalRepository.CheckIfAnimalExists(id);
    }

    public bool UpdateAnimal(int idAnimal, UpdateAnimalDTO dto)
    {
        return _animalRepository.UpdateAnimal(idAnimal, dto.Name, dto.Description, dto.Category, dto.Area);
    }

    public bool DeleteAnimal(int id)
    {
        return _animalRepository.DeleteAnimal(id);
    }
}