using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChallengeBackend
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public short Age { get; set; }
        public short Weight { get; set; }
        public string Lore { get; set; }
        [MaxLength]
        public byte[] Photo { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
    }
}
