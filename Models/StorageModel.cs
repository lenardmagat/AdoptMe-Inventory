using System.ComponentModel.DataAnnotations;
namespace inventory.Models;
public class Storage
{
    [Key]
    public int StorageId {get; set;}
    public required string ItemName {get; set;}
    public required bool Status {get; set;}

}