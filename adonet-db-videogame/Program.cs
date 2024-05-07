namespace adonet_db_videogame
{
    public class Program
    {
        public const string stringaDiConnessione = "Data Source=localhost;Initial Catalog=videogames;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

        static void Main(string[] args)
        {
            Videogame prova = new("ciao", "questa è una descrizione", 5, DateTime.Parse("12/4/2022"));
            Console.WriteLine(prova.Id);
            Console.WriteLine(prova.Name);
            Console.WriteLine(prova.Overview);
            Console.WriteLine(prova.SoftwareHouseId);
            Console.WriteLine(prova.ReleaseDate);
            Console.WriteLine(prova.CreatedAt);
            Console.WriteLine(prova.UpdatedAt);
        }
    }
}
