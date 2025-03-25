using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCMProject
{
    public class Movie
    {
        public int MovieID { get; set; } // Primary key
        public string? Title { get; set; } // Nullable
        public string? Genre { get; set; } // Nullable
        public int? ReleaseYear { get; set; } // Nullable for optional fields
        public string? Director { get; set; } // Nullable
        public decimal? Rating { get; set; } // Nullable for optional fields
        public bool Watched { get; set; } // Default is false
    }
}
