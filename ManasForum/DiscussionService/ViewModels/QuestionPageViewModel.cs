using DiscussionService.Models;

namespace DiscussionService.ViewModels;

public class QuestionPageViewModel
{
    public Question Question { get; set; }
    public string AuthorFullname { get; set; }
    public IEnumerable<AnswerViewModel> Answers { get; set; }
}