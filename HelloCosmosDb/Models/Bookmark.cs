using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace HelloCosmosDb.Models
{
public class Bookmark
{
    [Key]
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }   
    public string Url { get; set; }
    public string CreatedOn { get; set; } = DateTime.UtcNow.ToString();
}
}