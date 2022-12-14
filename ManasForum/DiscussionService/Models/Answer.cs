using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DiscussionService.Models;

public class Answer
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int QuestionId { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public int AuthorId { get; set; }
    [Required]
    public DateTime PublicationDate { get; set; }
    public bool IsViewed { get; set; }
    
}