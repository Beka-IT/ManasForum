using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using UserService.Models;

namespace DiscussionService.Models;

public class Question
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
    [Required]
    public int AuthorId { get; set; }
    [DefaultValue(0)]
    public int Views { get; set; }
    [Required]
    public DateTime PublicationDate { get; set; }
}