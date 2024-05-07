using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adonet_db_videogame
{
    public class Videogame(long id, string name, string overview, DateTime releaseDate, DateTime createdAt, DateTime updatedAt, long softwareHouseId)
    {
        public long Id { get; } = id;
        public string Name { get; set; } = name;
        public string Overview { get; set; } = overview;
        public DateTime ReleaseDate { get; } = releaseDate;
        public DateTime CreatedAt { get; } = createdAt;
        public DateTime UpdatedAt { get; set; } = updatedAt;
        public long SoftwareHouseId { get; } = softwareHouseId;
    }
}
