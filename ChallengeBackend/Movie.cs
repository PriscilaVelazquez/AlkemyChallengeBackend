using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChallengeBackend
{
    public class Movie
    {
        public int Id { get; set; }
        public byte[] Photo { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public short Rate { get; set; }
        public virtual Genre Genre { get; set; }
        [MaxLength]
        public virtual ICollection<Character> Characters { get; set; }
    }
}
