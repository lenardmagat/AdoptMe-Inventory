using System.ComponentModel.DataAnnotations;
namespace inventory.Models;
public enum Roles
{
    Admin,
    User
}
public class User
{
    [Key]
    public int UserId {get; set;}
    [MaxLength(12)]
    public required string Username {get;set;}
    public required string HashedPassword {get;set;}
    public required Roles Role{get; set;}
    public bool Status{get; set;}
    public ICollection<UserInventory> Items { get; set; } = new List<UserInventory>();
}