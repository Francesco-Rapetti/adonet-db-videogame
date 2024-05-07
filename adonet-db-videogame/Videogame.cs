using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace adonet_db_videogame
{
    internal class Videogame
    {
        public long Id { get; }
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                using SqlConnection connection = new(Program.stringaDiConnessione);
                try
                {
                    // validation
                    if (string.IsNullOrEmpty(value.ToString())) throw new Exception("name is not valid");

                    SqlCommand command = new(
                        @"UPDATE videogames SET name=@name WHERE id=@id;", connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@name", value);
                    command.Parameters.AddWithValue("@id", Id);
                    command.ExecuteNonQuery();
                    _name = value.ToString();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private string _overview;
        public string Overview
        {
            get => _overview;
            set
            {
                using SqlConnection connection = new(Program.stringaDiConnessione);
                try
                {
                    // validation
                    if (string.IsNullOrEmpty(value.ToString())) throw new Exception("overview is not valid");

                    SqlCommand command = new(
                        @"UPDATE videogames SET overview=@overview WHERE id=@id;", connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@overview", value);
                    command.Parameters.AddWithValue("@id", Id);
                    command.ExecuteNonQuery();
                    _overview = value.ToString();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public DateTime ReleaseDate { get; }
        public DateTime CreatedAt { get; }
        private DateTime _updatedAt;
        public DateTime UpdatedAt 
        { 
            get => _updatedAt;
            private set
            {
                using SqlConnection connection = new(Program.stringaDiConnessione);
                try
                {
                    // validation
                    if (typeof(DateTime) != value.GetType()) throw new Exception("value of updatedAt is not a DateTime");

                    SqlCommand command = new(
                        @"UPDATE Videogames SET updated_at=@updatedAt WHERE id=@id;", connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@updatedAt", value);
                    command.Parameters.AddWithValue("@id", Id);
                    command.ExecuteNonQuery();
                    _updatedAt = value;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } 
        }
        public long SoftwareHouseId { get; set; }


        public Videogame(string name, string overview, int softwareHouseId, DateTime releaseDate)
        { 
            try
            {
                // validation
                if (typeof(DateTime) != releaseDate.GetType()) throw new Exception("value of releaseDate is not a DateTime");
                if (string.IsNullOrEmpty(name)) throw new Exception("name is invalid");
                if (string.IsNullOrEmpty(overview)) throw new Exception("overview is invalid");
                if (!IsSoftwareHouse(softwareHouseId)) throw new Exception("Software house not found");

                Init(name, overview, softwareHouseId, releaseDate);
                using SqlConnection connection = new(Program.stringaDiConnessione);
                try
                {
                    connection.Open();
                    SqlCommand command = new("SELECT TOP 1 * FROM videogames ORDER BY id DESC", connection);
                    command.Parameters.AddWithValue("@name", name);
                    // Esegui la query e leggi il risultato
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        this.Id = reader.GetInt64(0); // Leggi l'ID dal primo campo 
                        this._name = reader.GetString(1); // Leggi il campo 'name' dal secondo campo 
                        this._overview = reader.GetString(2); // Leggi il campo 'overview' dal terzo campo 
                        this.ReleaseDate = reader.GetDateTime(3); // Leggi il campo 'release_date' dal quinto campo 
                        this._updatedAt = reader.GetDateTime(4); // Leggi il campo 'updated_at' dal sesto campo 
                        this.CreatedAt = reader.GetDateTime(5); // Leggi il campo 'created_at' dal settimo campo 
                        this.SoftwareHouseId = reader.GetInt64(6); // Leggi il campo 'software_house_id' dal quarto campo 
                        reader.Close();
                    }
                    else
                    {
                        throw new Exception("Videogioco non trovato");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void Init(string name, string overview, int softwareHouseId, DateTime releaseDate)
        {
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
            } catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                return false;
            }

        }
    }
}
