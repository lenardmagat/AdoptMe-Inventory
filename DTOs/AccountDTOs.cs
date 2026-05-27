using System.ComponentModel.DataAnnotations;
namespace inventory.DTOs;
public record AccountCredentials
(
    [Required] string Name,
    [Required] string Password
);
public record ChangePasswordCredentials(
    [Required] string Password,
    [Required] string NewPassword
);