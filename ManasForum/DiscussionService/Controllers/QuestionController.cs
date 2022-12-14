using DiscussionService.Data;
using DiscussionService.Models;
using DiscussionService.ViewModels;
using Microsoft.AspNetCore.Mvc;
using UserService.Models;

namespace DiscussionService.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionController : ControllerBase
{
    private readonly DiscussionServiceContext _context;
    private const int PageSize = 2;

    public QuestionController(DiscussionServiceContext context)
    {
        _context = context;
    }

    [HttpGet("PopularQuestions")]
    public IEnumerable<PopularQuestionsViewModel> GetPopularQuestions()
    {
        var result = new List<PopularQuestionsViewModel>();
        var questions =  _context.Questions
            .OrderByDescending(q => q.Views)
            .Take(6);

        foreach (var question in questions)
        {
            string authorFullname = _context.Accounts.FirstOrDefault(a => a.Id == question.AuthorId).Fullname;
            
            int answersCount = _context.Answers.Where(a => a.QuestionId == question.Id).Count();
            
            result.Add(
                new PopularQuestionsViewModel()
                {
                    Question = question,
                    AnswersCount = answersCount,
                    AuthorFullname = authorFullname
                });
        }
        
        return result;
    }
}