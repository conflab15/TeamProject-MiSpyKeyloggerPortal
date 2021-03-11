using System;
using System.ComponentModel.DataAnnotations;

namespace MiSpyWebAppMVC.Models
{
    public class LoggedEvent
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Event { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public bool HasRead { get; set; }
    }
}
