using DiscussionService.Models;

namespace DiscussionService.ViewModels;

public class QuestionsViewModel
{
    public Question Question { get; set; }
    public int AnswersCount { get; set; }
    public string AuthorFullname { get; set; }
}