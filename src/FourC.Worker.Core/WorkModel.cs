using System;
using System.ComponentModel.DataAnnotations;

namespace FourC.Worker.Core
{
    public class WorkModel
    {
        [Required, StringLength(100)]
        public string User { get; set; }
        [Required]
        public DateTime Timestamp { get; set; }
        [Required, StringLength(100)]
        public string Content { get; set; }

        public override string ToString()
        {
            return $"{Content} from {User} at {Timestamp}";
        }
    }
}