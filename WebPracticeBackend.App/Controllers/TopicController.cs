using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebPracticeBackend.App.Data;
using WebPracticeBackend.App.Models;
using WebPracticeBackend.App.Requests;
using WebPracticeBackend.App.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebPracticeBackend.App.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TopicController : ControllerBase {
	private readonly ApplicationDbContext _context;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public TopicController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) {
		_context = context;
		_httpContextAccessor = httpContextAccessor;
	}


	[HttpGet]
	[Route("{id}")]
	public async Task<IActionResult> GetTopic(string id) {
		var result = await _context.Topics.FindAsync(id);
		return result == null ? NotFound() : new OkObjectResult(result);
	}


	[HttpPost]
	[Route("create")]
	public async Task<ActionResult> CreateTopic([FromBody] TopicRequest topic) {
		var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

		if (userId == null) {
			return new ForbidResult();
		}

		var item = new Topic {
			Title = topic.Title,
			Description = topic.Description,
			UserId = userId
		};

		_context.Topics.Add(item);

		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(GetTopic), new { id = item.Id }, new TopicResponse { Title = item.Title, Description = item.Description, Id = item.Id, UserId = userId });
	}

	[HttpGet]
	[Route("all")]
	public async Task<ActionResult<PaginatedResponse<Topic>>> GetAllTopics([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0) {
		var totalTopics = await _context.Topics
			.LongCountAsync();

		var topicOnPage = await _context.Topics
			.Skip(pageSize * pageIndex)
			.Take(pageSize)
			.ToListAsync();

		return new PaginatedResponse<Topic>(pageIndex, pageSize, totalTopics, topicOnPage);
	}

	[HttpPut]
	[Route("{id}")]
	public async Task<ActionResult> UpdateTopic(string id, [FromBody] TopicRequest newTopicData) {
		var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

		if (userId == null) {
			return new ForbidResult();
		}

		var topic = await _context.Topics.SingleOrDefaultAsync(t => t.Id == id);

		if (topic == null) {
			return NotFound(new { Message = $"Topic with id {id} not found." });
		}

		if (topic.UserId != userId) {
			return new ForbidResult();
		}

		topic.Title = newTopicData.Title;
		topic.Description = newTopicData.Description;
		_context.Topics.Update(topic);
		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(GetTopic), new { id = topic.Id }, new TopicResponse { Title = newTopicData.Title, Description = newTopicData.Description, Id = topic.Id, UserId = userId });
	}

	[HttpDelete]
	[Route("{id}")]
	public async Task<ActionResult> DeleteTopic(string id) {
		var topic = _context.Topics.SingleOrDefault(t => t.Id == id);
		var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

		if (userId == null) {
			return new ForbidResult();
		}

		if (topic == null) {
			return NotFound();
		}

		if (topic.UserId != userId) {
			return new ForbidResult();
		}

		_context.Topics.Remove(topic);
		await _context.SaveChangesAsync();

		return NoContent();
	}

	[HttpGet]
	[Route("{topicId}/comment/all")]
	public async Task<ActionResult<PaginatedResponse<TopicComment>>> GetAllComments([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0) {
		var totalComments = await _context.TopicComments
			.LongCountAsync();

		var commentsOnPage = await _context.TopicComments
			.Skip(pageSize * pageIndex)
			.Take(pageSize)
			.ToListAsync();

		return new PaginatedResponse<TopicComment>(pageIndex, pageSize, totalComments, commentsOnPage);
	}

	[HttpGet]
	[Route("{topicId}/comment/{id}")]
	public async Task<IActionResult> GetComment(string topicId, string id) {
		var result = await _context.TopicComments.FindAsync(id);
		return result == null ? NotFound() : new OkObjectResult(result);
	}

	[HttpPost]
	[Route("{topicId}/comment/create")]
	public async Task<ActionResult> CreateComment(string topicId, [FromBody] TopicCommentRequest comment) {
		var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

		if (userId == null) {
			return new ForbidResult();
		}

		var item = new TopicComment {
			Content = comment.Content,
			TopicId = topicId,
			UserId = userId
		};

		_context.TopicComments.Add(item);

		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(GetComment), new { id = item.Id }, new TopicCommentResponse { Content = item.Content, Id = item.Id, TopicId = item.TopicId, UserId = userId });
	}

	[HttpPut]
	[Route("{topicId}/comment/{id}")]
	public async Task<ActionResult> UpdateComment(string topicId, string id, [FromBody] TopicCommentRequest newTopicCommentData) {
		var comment = await _context.TopicComments.SingleOrDefaultAsync(c => c.Id == id && c.TopicId == topicId);
		var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

		if (comment == null) {
			return NotFound(new { Message = $"Comment with id {id} not found in topic {topicId}." });
		}

		if (comment.UserId != userId) {
			return new ForbidResult();
		}

		comment.Content = newTopicCommentData.Content;
		_context.TopicComments.Update(comment);
		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, new TopicCommentResponse { Content = newTopicCommentData.Content, Id = comment.Id, TopicId = comment.TopicId, UserId = userId });
	}

	[HttpDelete]
	[Route("{topicId}/comment/{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> DeleteComment(string topicId, string id) {
		var comment = _context.TopicComments.SingleOrDefault(x => x.Id == id);
		var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

		if (userId == null) {
			return new ForbidResult();
		}

		if (comment == null) {
			return NotFound();
		}

		if (comment.UserId != userId) {
			return new ForbidResult();
		}

		_context.TopicComments.Remove(comment);

		await _context.SaveChangesAsync();

		return NoContent();
	}
}