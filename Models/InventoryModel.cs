using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using inventory.Models;
public enum Categories
{
    None = 0,
    Normal = 1,
    Neon = 2,
    MegaNeon = 3
}
public class InventoryItem
    
{
    [Key]
    public int ItemId {get; set;}
    [Range(1, int.MaxValue ,ErrorMessage = "Quantity cannot be negative.")]
    public required int quantity {get; set;}
    [Range(0.0, double.MaxValue, ErrorMessage = "Price cannot be negative")]
    public required double price {get; set;}
    public Categories category {get; set;}
    public required int UserInventoryId{get; set;}
    [ForeignKey(nameof(UserInventoryId))]
    public UserInventory Owner {get; set;} = null!;
    public required int StorageId {get; set;}
    [ForeignKey(nameof(StorageId))]
    public Storage ItemAttributes {get; set;} = null!;
}