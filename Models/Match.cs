using System;
using System.ComponentModel.DataAnnotations;

namespace Jorfofbasket.Models
{
    public class Match
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom de l'équipe adverse est obligatoire")]
        [Display(Name = "Équipe adverse")]
        public string EquipeAdverse { get; set; }

        [Required(ErrorMessage = "La date du match est obligatoire")]
        [DataType(DataType.Date)]
        [Display(Name = "Date du match")]
        public DateTime DateMatch { get; set; }

        [Display(Name = "Score équipe locale")]
        public int? ScoreEquipe { get; set; }

        [Display(Name = "Score adverse")]
        public int? ScoreAdverse { get; set; }

        [Display(Name = "Lieu du match")]
        public string Lieu { get; set; }

        [Display(Name = "Statut du match")]
        public string Statut { get; set; } = "Prévu"; // Par défaut "Prévu"
    }
}
