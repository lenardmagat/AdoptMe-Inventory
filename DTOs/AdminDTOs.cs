using System.ComponentModel.DataAnnotations;
namespace inventory.DTOs;
public record Item
(
    [Required] string ItemName
);
public record StorageItems(
    [Required]
    [MinLength(1, ErrorMessage = "Must contain at least 1 item")]
    List<Item> Items
);
public record ItemHashids(
    [Required]
    [MinLength(1, ErrorMessage = "Must contain at least 1 item")]
    List<string> Items
);