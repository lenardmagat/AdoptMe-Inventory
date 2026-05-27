using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace inventory.Models;
public class UserInventory
{
    [Key]
    public int InventoryId {get; set;}
    public int UserId {get;set;}
    [MaxLength(12)]
    public required string InventoryName {get; set;}
    public required bool InvetoryStatus {get; set;}
    public required string DateCreated {get; set;}
    [ForeignKey("UserId")]
    public User Owner {get; set;} = null!; 
    public ICollection<InventoryItem> Items { get; set; } = new List<InventoryItem>();
}