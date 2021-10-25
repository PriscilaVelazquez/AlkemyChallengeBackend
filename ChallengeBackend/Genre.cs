using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChallengeBackend
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [MaxLength]
        public byte[] Photo { get; set; }
        public virtual ICollection<Movie> Films { get; set; }
    }
}
