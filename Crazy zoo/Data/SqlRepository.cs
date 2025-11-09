using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Crazy_zoo.Modules;
using Crazy_zoo.Animals;

namespace Crazy_zoo.Data
{
    public class SqlRepository<T> : IRepository<T> where T : Animal, new()
    {
        private readonly string _connectionString;

        public SqlRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(T item)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = new SqlCommand(
                @"INSERT INTO Animals (Name, Species, Age, EnclosureId)
                  VALUES (@Name, @Species, @Age, @EnclosureId);
                  SELECT SCOPE_IDENTITY();", connection);

            cmd.Parameters.AddWithValue("@Name", item.Name);
            cmd.Parameters.AddWithValue("@Species", item.Species);
            cmd.Parameters.AddWithValue("@Age", item.Age);
            cmd.Parameters.AddWithValue("@EnclosureId", item.EnclosureId.HasValue ? (object)item.EnclosureId.Value : DBNull.Value);

            item.Id = Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Remove(T item)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = new SqlCommand("DELETE FROM Animals WHERE Id = @Id", connection);
            cmd.Parameters.AddWithValue("@Id", item.Id);
            cmd.ExecuteNonQuery();
        }

        private T CreateAnimalFromRecord(SqlDataReader reader)
        {
            string species = reader["Species"].ToString()!;
            Animal animal = species switch
            {
                "Lion" => new Lion(),
                "Sheep" => new Sheep(),
                "Parrot" => new Parrot(),
                "Unicorn" => new Unicorn(),
                "Dragon" => new Dragon(),
                "Capybara" => new Capybara(),
                "Dolphin" => new Dolphin(),
                "Shark" => new Shark(),
                "Whale" => new Whale(),
                _ => new CustomAnimal()
            };

            animal.Id = Convert.ToInt32(reader["Id"]);
            animal.Name = reader["Name"].ToString()!;
            animal.Species = species;
            animal.Age = Convert.ToInt32(reader["Age"]);
            animal.EnclosureId = reader["EnclosureId"] != DBNull.Value ? (int?)Convert.ToInt32(reader["EnclosureId"]) : null;

            if (animal is T tAnimal)
                return tAnimal;
            else
                throw new InvalidCastException($"Cannot cast {animal.GetType().Name} to {typeof(T).Name}");
        }



        public IEnumerable<T> GetAll()
        {
            var list = new List<T>();
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = new SqlCommand("SELECT Id, Name, Species, Age, EnclosureId FROM Animals", connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                list.Add(CreateAnimalFromRecord(reader));

            return list;
        }

        public T? Find(Func<T, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }
    }
}
