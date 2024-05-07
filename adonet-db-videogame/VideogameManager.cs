using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace adonet_db_videogame
{
    public static class VideogameManager
    {
        public static void InsertNewVideogame(string name, string overview, long softwareHouseId, DateTime releaseDate)
        {
            try
            {
                Validation(name, overview, softwareHouseId, releaseDate);
                using SqlConnection connection = new(Program.stringaDiConnessione);
                SqlCommand command = new(
                    @"INSERT INTO Videogames (name, overview, software_house_id, release_date, created_at, updated_at) 
                VALUES (@name, @overview, @softwareHouseId, @releaseDate, @createdAt, @updatedAt)", connection
                    );
                connection.Open();
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@overview", overview);
                command.Parameters.AddWithValue("@softwareHouseId", softwareHouseId);
                command.Parameters.AddWithValue("@releaseDate", releaseDate);
                command.Parameters.AddWithValue("@createdAt", DateTime.Now);
                command.Parameters.AddWithValue("@updatedAt", DateTime.Now);
                command.ExecuteNonQuery();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static Videogame GetVideogameById(long id)
        {

            using SqlConnection connection = new(Program.stringaDiConnessione);
            // validation
            if (id <= 0) throw new Exception("ID must be positive");
            connection.Open();
            SqlCommand command = new("SELECT * FROM videogames WHERE id=@id", connection);
            command.Parameters.AddWithValue("@id", id);
            // Esegui la query e leggi il risultato
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Videogame output = new(
                reader.GetInt64(0), // Leggi l'ID dal primo campo 
                reader.GetString(1), // Leggi il campo 'name' dal secondo campo 
                reader.GetString(2), // Leggi il campo 'overview' dal terzo campo 
                reader.GetDateTime(3), // Leggi il campo 'release_date' dal quinto campo 
                reader.GetDateTime(4), // Leggi il campo 'updated_at' dal sesto campo 
                reader.GetDateTime(5), // Leggi il campo 'created_at' dal settimo campo 
                reader.GetInt64(6) // Leggi il campo 'software_house_id' dal quarto campo 
                );
                reader.Close();
                return output;
            }
            else
            {
                throw new Exception("Videogame not found");
            }
        }

        public static List<Videogame> GetVideogamesByName(string name)
        {
            // validation
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("name is invalid");

            using SqlConnection connection = new(Program.stringaDiConnessione);
            connection.Open();
            SqlCommand command = new("SELECT * FROM videogames WHERE name LIKE @name", connection);
            command.Parameters.AddWithValue("@name", "%" + name + "%");
            SqlDataReader reader = command.ExecuteReader();

            List<Videogame> output = new();

            while (reader.Read())
            {
                Videogame videogame = new(
                reader.GetInt64(0), // Leggi l'ID dal primo campo
                reader.GetString(1), // Leggi il campo 'name' dal secondo campo
                reader.GetString(2), // Leggi il campo 'overview' dal terzo campo
                reader.GetDateTime(3), // Leggi il campo 'release_date' dal quinto campo
                reader.GetDateTime(4), // Leggi il campo 'updated_at' dal sesto campo
                reader.GetDateTime(5), // Leggi il campo 'created_at' dal settimo campo
                reader.GetInt64(6) // Leggi il campo 'software_house_id' dal quarto campo
                );
                output.Add(videogame);
            }

            return output;
        }

        public static void DeleteVideogame(long id)
        {
            // validation
            if (id <= 0) throw new Exception("ID must be positive");

            using SqlConnection connection = new(Program.stringaDiConnessione);
            connection.Open();
            SqlCommand command = new("DELETE FROM videogames WHERE id=@id", connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            Console.WriteLine("Videogame deleted");
        }

        private static void Validation(string name, string overview, long softwareHouseId, DateTime releaseDate)
        {
            try
            {
                // validation
                if (typeof(DateTime) != releaseDate.GetType()) throw new Exception("value of releaseDate is not a DateTime");
                if (string.IsNullOrWhiteSpace(name)) throw new Exception("name is invalid");
                if (string.IsNullOrWhiteSpace(overview)) throw new Exception("overview is invalid");
                if (!IsSoftwareHouse(softwareHouseId)) throw new Exception("Software house not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static bool IsSoftwareHouse(long id)
        {
            try
            {
                using SqlConnection connection = new(Program.stringaDiConnessione);
                connection.Open();
                SqlCommand command = new(
                    @"SELECT * FROM software_houses WHERE id=@id", connection
                    );
                command.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return true;
                } else
                {
                    throw new Exception("SoftwareHouse not found");
                }
            } catch (Exception)
            {
                return false;
            }

        }
    }
}
