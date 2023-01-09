using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels
{
  public class RegisterViewModel
  {
    [Required]
    // validates that it's an email
    [EmailAddress]
    // specifies that the field name should be displayed as "Email Address" not Email
    [Display(Name = "Email Address")]
    public string Email { get; set; }

    [Required]
    // specifies that field should be formatted as a password
    [DataType(DataType.Password)]

    // Regex to check that a password contains 
    // 6 chars: a capital & lowercase letter, a number, and a special character
    // Commented out while in development

    // [RegularExpression("^(?=.*[a-z])(?=.*A-Z)(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&\]{6,}$", ErrorMessage = "Your password must contain at least six characters, a capital letter, a lowercase letter, a number, and a special character.")]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    // first argument specifies what to compare it to
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
  }
}