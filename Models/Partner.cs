using System.ComponentModel.DataAnnotations;

namespace Jorfofbasket.Models
{
    public class Partner
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Titre FR")]
        public string TitleFr { get; set; }

        [Required]
        [Display(Name = "Titre EN")]
        public string TitleEn { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [Display(Name = "Photo")]
        public string PhotoPath { get; set; }
    }
}
