using ConsoleTables;
using Microsoft.Data.SqlClient;

namespace adonet_db_videogame
{
    public class Program
    {
        public const string stringaDiConnessione = "Data Source=localhost;Initial Catalog=videogames;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

        static void Main(string[] args)
        {
            ConsoleTable tableVideogames = new(
                                "Name", "Overview", "Software House Id", "Release Date", "Created At", "Updated At"
                                );
            Console.WriteLine("Welcome to Videogame Manager! v1.0.0");
            while (true)
            {
                try
                {
                    Console.WriteLine("\nEnter a command:");
                    Console.WriteLine("1. Insert a new videogame");
                    Console.WriteLine("2. Search for a videogame by ID");
                    Console.WriteLine("3. Search for videogames by name");
                    Console.WriteLine("4. Delete a videogame");
                    Console.WriteLine("5. Exit");

                    int command = int.Parse(Console.ReadLine());
                    if (command > 5 || command < 1) throw new Exception("ERROR: invalid command");
                    switch (command)
                    {
                        // Insert new videogame
                        case 1:
                            string name;
                            string overview;
                            long softwareHouseId = -1;
                            DateTime releaseDate;

                            // name
                            while (true)
                            {
                                try
                                {
                                    Console.Write("Enter the name of the videogame: ");
                                    name = Console.ReadLine();
                                    if (string.IsNullOrWhiteSpace(name)) throw new Exception("ERROR: name is invalid");
                                    break;
                                } catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }

                            // overview
                            while (true)
                            {
                                try
                                {
                                    Console.Write("Enter the overview of the videogame: ");
                                    overview = Console.ReadLine();
                                    if (string.IsNullOrWhiteSpace(overview)) throw new Exception("ERROR: overview is invalid");
                                    break;
                                } catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }

                            // softwareHouseId
                            while (true)
                            {
                                try
                                {
                                    Console.WriteLine("Select a software house from the list below: ");
                                    ConsoleTable tableHouses = new("ID", "Name", "Country");
                                    using SqlConnection connection = new(stringaDiConnessione);
                                    connection.Open();
                                    SqlCommand cmd = new("SELECT * FROM software_houses;", connection);
                                    // Esegui la query e leggi il risultato
                                    SqlDataReader reader = cmd.ExecuteReader();
                                    if (!reader.HasRows) throw new Exception("ERROR: Software Houses not found");
                                    while (reader.Read())
                                    {
                                        tableHouses.AddRow(reader.GetInt64(0), reader.GetString(1), reader.GetString(4));
                                    }
                                    Console.WriteLine(tableHouses);
                                    Console.Write("ID: ");
                                    softwareHouseId = long.Parse(Console.ReadLine());
                                    if (!VideogameManager.IsSoftwareHouse(softwareHouseId)) throw new Exception("ERROR: Software House not found");
                                    break;
                                } catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }

                            // release date
                            while (true)
                            {
                                try
                                {
                                    Console.Write("Enter the release date of the videogame: ");
                                    DateTime input = DateTime.Parse(Console.ReadLine());
                                    if (input < DateTime.Parse("1/1/1753") || input > DateTime.MaxValue) throw new Exception("ERROR: release date must be between 1/1/1753 and 31/12/9999");
                                    releaseDate = input;
                                    break;
                                } catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }

                            VideogameManager.InsertNewVideogame(name, overview, softwareHouseId, releaseDate);
                            tableVideogames.AddRow(
                                name.Length > 25 ? name.Substring(0, 25) + "..." : name, 
                                overview.Length > 10 ? overview.Substring(0, 10) + "..." : overview, 
                                softwareHouseId, 
                                releaseDate.ToString("dd/MM/yyyy"), 
                                DateTime.Now, 
                                DateTime.Now
                                );
                            Console.WriteLine("Your videogames: ");
                            Console.WriteLine(tableVideogames);
                            break;

                        // search for a videogame by ID
                        case 2:
                            while (true)
                            {
                                try
                                {
                                    Console.Write("Enter the ID of the videogame: ");
                                    long id = long.Parse(Console.ReadLine());
                                    Videogame videogame = VideogameManager.GetVideogameById(id);
                                    ConsoleTable videogameId = new("Name", "Overview", "Software House ID", "Release Date", "Created At", "Updated At");
                                    videogameId.AddRow(
                                        videogame.Name.Length > 25 ? videogame.Name.Substring(0, 25) + "..." : videogame.Name, 
                                        videogame.Overview.Length > 10 ? videogame.Overview.Substring(0, 10) + "..." : videogame.Overview, 
                                        videogame.SoftwareHouseId, 
                                        videogame.ReleaseDate.ToString("dd/MM/yyyy"), 
                                        videogame.CreatedAt, 
                                        videogame.UpdatedAt
                                        );
                                    Console.WriteLine($"Result for videogame ID: {id}");
                                    Console.WriteLine(videogameId);
                                    break;
                                } catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                            break;

                        // search for videogames by name
                        case 3:
                            while (true)
                            {
                                try
                                {
                                    Console.Write("Enter the name of the videogame: ");
                                    string input = Console.ReadLine();
                                    List<Videogame> videogames = VideogameManager.GetVideogamesByName(input);
                                    if (videogames.Count == 0) throw new Exception("ERROR: Videogames not found");
                                    ConsoleTable videogameName = new("ID", "Name", "Overview", "Software House ID", "Release Date", "Created At", "Updated At");
                                    foreach (Videogame videogame in videogames)
                                        videogameName.AddRow(
                                            videogame.Id, 
                                            videogame.Name.Length > 25 ? videogame.Name.Substring(0, 25) + "..." : videogame.Name,
                                            videogame.Overview.Length > 10 ? videogame.Overview.Substring(0, 10) + "..." : videogame.Overview, 
                                            videogame.SoftwareHouseId,
                                            videogame.ReleaseDate.ToString("dd/MM/yyyy"), 
                                            videogame.CreatedAt, 
                                            videogame.UpdatedAt
                                            );
                                    Console.WriteLine($"Result for videogame name: {input}");
                                    Console.WriteLine(videogameName);
                                    break;
                                } catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                            break;

                        // delete a videogame
                        case 4:
                            while (true)
                            {
                                try
                                {
                                    Console.Write("Enter the ID of the videogame: ");
                                    long id = long.Parse(Console.ReadLine());
                                    Console.Write($"Are you sure you want to delete <{VideogameManager.GetVideogameById(id).Name}>? [y/n] ");
                                    if (Console.ReadLine().ToLower() == "y") VideogameManager.DeleteVideogame(id);
                                    break;
                                } catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                            break;

                        // exit
                        case 5:
                            Environment.Exit(0);
                            break;

                        default:
                            break;

                    }
                } catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
