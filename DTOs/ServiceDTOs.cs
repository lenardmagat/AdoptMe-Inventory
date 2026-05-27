using System.ComponentModel.DataAnnotations;
namespace inventory.DTOs;
public record ItemLibraryData
(
    [Required]string PetName,
    [Required]string PetHashidId
);
public record CreateInventoryDetails
(
    [Required]string InventoryName
);
public record UpsertItemDetails(
    [Required] string InventoryId,
    [Required] string StorageId, 
    [Required][Range(1, int.MaxValue, ErrorMessage = "At least 1.")] int Quantity,
    [Required][Range(0, double.MaxValue, ErrorMessage = "Invalid Price.")] double Price,
    [Required] Categories? Category 
);

public class UserInventoryData
{
    public string InventoryId {get; set;} = null!;
    public string InventoryName {get; set;} = null!;
    public string DateCreate {get; set;} = null!;
}
public class UserInventoryItemData
{
    public string ItemId {get; set;} = null!;
    public string StorageId {get; set;} = null!;
    public int Quantity {get; set;}
    public double Price {get; set;}
    public string Category {get;set;} = null!;
    public string ItemName {get; set;} = null!;
    public string Username {get; set;} = null!;
}
public class DeleteItem
{
    public string ItemId {get; set;} = null!;
    public string InventoryId {get; set;} = null!;
}

public class DeleteInventory
{
    public string InventoryId {get; set;} = null!;
}