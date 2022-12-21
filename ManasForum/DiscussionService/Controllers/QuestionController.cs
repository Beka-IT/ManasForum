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
            string authorFullname = _context.Accounts.FirstOrDefault(a => a.Id == question.AuthorId).Fullname;
            
            int answersCount = _context.Answers.Where(a => a.QuestionId == question.Id).Count();
            
            result.Add(
                new QuestionsViewModel()
                {
                    Question = question,
                    AnswersCount = answersCount,
                    AuthorFullname = authorFullname
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
            string authorFullname = _context.Accounts.FirstOrDefault(a => a.Id == question.AuthorId).Fullname;
            
            int answersCount = _context.Answers.Where(a => a.QuestionId == question.Id).Count();
            
            result.Add(
                new QuestionsViewModel()
                {
                    Question = question,
                    AnswersCount = answersCount,
                    AuthorFullname = authorFullname
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