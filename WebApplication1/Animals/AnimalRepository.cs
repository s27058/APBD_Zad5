using System.Data.SqlClient;

namespace WebApplication1.Animals;

public interface IAnimalRepository
{
    public IEnumerable<Animal> FetchAllAnimals(string orderBy);
    public bool CreateAnimal(string name, string? description, string category, string area);
    public bool CheckIfAnimalExists(int id);
    public bool UpdateAnimal(int id, string name, string? description, string category, string area);
    public bool DeleteAnimal(int id);
}

public class AnimalRepository : IAnimalRepository
{
    private readonly IConfiguration _configuration;
    public AnimalRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public IEnumerable<Animal> FetchAllAnimals(string orderBy)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();

        var safeOrderBy = new string[] { "name", "description",
            "category", "area" }.Contains(orderBy) ? orderBy : "name";
        using var command = new SqlCommand($"SELECT Id, Name, Description, Category, Area FROM Animals ORDER BY {safeOrderBy} ASC", connection);
        using var reader = command.ExecuteReader();
        
        var animals = new List<Animal>();
        while (reader.Read())
        {
            var animal = new Animal()
            {
                Id = (int)reader["Id"],
                Name = reader["Name"].ToString()!,
                Description = reader["Description"].ToString(),
                Category = reader["Category"].ToString()!,
                Area = reader["Area"].ToString()!
            };
            animals.Add(animal);
        }

        return animals;
    }

    public bool CreateAnimal(string name, string? description, string category, string area)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();
        
        using var command = new SqlCommand("INSERT INTO Animals (Name, Description, Category, Area) VALUES (@name, @description, @category, @area)", connection);
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@description", description is null ? DBNull.Value : description);
        command.Parameters.AddWithValue("@category", category);
        command.Parameters.AddWithValue("@area", area);
        var affectedRows = command.ExecuteNonQuery();
        return affectedRows == 1;
    }

    public bool CheckIfAnimalExists(int id)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();
        
        using var command = new SqlCommand($"SELECT * FROM Animals WHERE Id=@id", connection);
        command.Parameters.AddWithValue("@id", id);
        using var reader = command.ExecuteReader();
        return reader.HasRows;
    }
    
    public bool UpdateAnimal(int id, string name, string? description, string category, string area)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();
        
        using var command = new SqlCommand("UPDATE Animals SET (Name = @name, Description = @description, Category = @category, Area = @area) WHERE Id = @id", connection);
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@description", description);
        command.Parameters.AddWithValue("@category", category);
        command.Parameters.AddWithValue("@area", area);
        var affectedRows = command.ExecuteNonQuery();
        return affectedRows == 1;
    }
    
    public bool DeleteAnimal(int id)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();
        
        using var command = new SqlCommand("DELETE Animals WHERE Id = @id", connection);
        command.Parameters.AddWithValue("@id", id);
        var affectedRows = command.ExecuteNonQuery();
        return affectedRows == 1;
    }
}