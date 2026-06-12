using System.ComponentModel.DataAnnotations;

public class EditProfileVM
{
    [Required]
    public string FullName { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }
}