using System.ComponentModel.DataAnnotations;

namespace WebAPIAutores1.DTOs
{
    public class EditarAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
