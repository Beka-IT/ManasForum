using DiscussionService.Data;
using DiscussionService.Models;
using DiscussionService.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public IEnumerable<QuestionsViewModel> GetPopularQuestions()
    {
        var result = new List<QuestionsViewModel>();
        var questions =  _context.Questions
            .OrderByDescending(q => q.Id)
            .Take(6);

        foreach (var question in questions)
        {
            var author = _context.Accounts.FirstOrDefault(a => a.Id == question.AuthorId);
            
            int answersCount = _context.Answers.Where(a => a.QuestionId == question.Id).Count();
            
            result.Add(
                new QuestionsViewModel()
                {
                    Question = question,
                    AnswersCount = answersCount,
                    Author = author
                });
        }
        
        return result;
    }

    [HttpPost("AddQuestion")]
    public async Task<IActionResult> AddQuestion(Question newQuestion)
    {
        if (newQuestion.PublicationDate == null || newQuestion.Title == null || newQuestion.Description == null ||
            newQuestion.AuthorId == null)
        {
            return BadRequest();
        }
        
        await _context.Questions.AddAsync(newQuestion);
        
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("FindQuestions")]
    public IEnumerable<QuestionsViewModel> FindQuestions(string title)
    {
        var result = new List<QuestionsViewModel>();
        
        var questions = _context.Questions.ToList()
            .Where(q => q.Title.ToLower().Contains(title.ToLower()) || q.Description.ToLower().Contains(title.ToLower()))
            .Take(6);
        
        foreach (var question in questions)
        {
            var author = _context.Accounts.FirstOrDefault(a => a.Id == question.AuthorId);
            
            int answersCount = _context.Answers.Where(a => a.QuestionId == question.Id).Count();
            
            result.Add(
                new QuestionsViewModel()
                {
                    Question = question,
                    AnswersCount = answersCount,
                    Author = author
                });
        }
        
        return result;
    }

    [HttpPost("Answer")]
    public async Task AddAnswer(Answer newAnswer)
    {
        if (newAnswer != null)
        {
            newAnswer.PublicationDate = DateTime.Today;
            await _context.AddAsync(newAnswer);
            await _context.SaveChangesAsync();
        }
    }

    [HttpGet("GetQuestionsByAuthorId")]
    public IEnumerable<QuestionsViewModel> GetQuestionsByAuthorId(int authorId)
    {
        var result = new List<QuestionsViewModel>();
        
        var questions = _context.Questions.ToList()
            .Where(q => q.AuthorId == authorId)
            .Take(6);
        
        foreach (var question in questions)
        {
            var author = _context.Accounts.FirstOrDefault(a => a.Id == question.AuthorId);
            
            int answersCount = _context.Answers.Where(a => a.QuestionId == question.Id).Count();
            
            result.Add(
                new QuestionsViewModel()
                {
                    Question = question,
                    AnswersCount = answersCount,
                    Author = author
                });
        }
        
        return result;
    }

    [HttpGet("GetQuestion")]
    public async Task<QuestionPageViewModel> GetQuestion(int id, int userId)
    {
        var result = new QuestionPageViewModel();

        result.Question = _context.Questions.FirstOrDefault(q => q.Id == id);

        if (result.Question.AuthorId != userId)
        {
            result.Question.Views++;
            await _context.SaveChangesAsync();
        }
        
        result.AuthorFullname = _context.Accounts.FirstOrDefault(a => a.Id == result.Question.AuthorId).Fullname;

        result.Answers = await GetAnswers(id);
        
        return result;
    }
    

    [HttpGet("GetAllMembers")]
    public IEnumerable<AccountViewModel> GetAllMembers()
    {
        var accounts = _context.Accounts.ToList();
        var questions = _context.Questions.ToList();
        var answers = _context.Answers.ToList();

        var accountResult = new List<AccountViewModel>();

        foreach (var account in accounts)
        {
            accountResult.Add(
                new AccountViewModel()
                {
                    Account = account,
                    AnswersCount = answers.Where(a => a.AuthorId == account.Id).Count(),
                    QuestionCounts = answers.Where(a=> a.AuthorId == account.Id).Count(),
                }
            );
            accountResult[accountResult.Count - 1].Activity = accountResult[accountResult.Count - 1].AnswersCount * 2 +
                                                              accountResult[accountResult.Count - 1].QuestionCounts;
        }
        
        return accountResult;
    }

    private async Task<IEnumerable<AnswerViewModel>> GetAnswers(int id)
    {
        var answers = new List<AnswerViewModel>();
        
        await _context.Answers.Where(a => a.QuestionId == id).ForEachAsync(ans =>
        {
            var newAnswerViewModel = new AnswerViewModel();
            newAnswerViewModel.Answer = ans;
            newAnswerViewModel.AuthorFullname = _context.Accounts.FirstOrDefault(a => a.Id == ans.AuthorId)?.Fullname;
            answers.Add(newAnswerViewModel);
        });
        
        return answers;
    }
}