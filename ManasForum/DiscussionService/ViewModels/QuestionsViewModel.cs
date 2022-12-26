using DiscussionService.Models;
using UserService.Models;

namespace DiscussionService.ViewModels;

public class QuestionsViewModel
{
    public Question Question { get; set; }
    public int AnswersCount { get; set; }
    public Account Author { get; set; }
}