using System;
using System.ComponentModel.DataAnnotations;

namespace MvcBooks.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Título")]
        public string Title { get; set; } = null!;

        [Required]
        [Display(Name = "Autor")]
        public string Author { get; set; } = null!;

        [Display(Name = "Género Literario")]
        public string Genre { get; set; } = null!;

        [Display(Name = "Año de Publicación")]
        public int Year { get; set; }

        [Display(Name = "Editorial")]
        public string Publisher { get; set; } = null!;

        [Display(Name = "ISBN")]
        public string ISBN { get; set; } = null!;

        // --- Soft-delete fields ---
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
