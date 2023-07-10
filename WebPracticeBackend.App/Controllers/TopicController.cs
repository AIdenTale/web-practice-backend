using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebPracticeBackend.App.Data;
using WebPracticeBackend.App.Models;
using WebPracticeBackend.App.Requests;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebPracticeBackend.App.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TopicController : ControllerBase {
	private readonly ApplicationDbContext _context;
	private readonly UserManager<User> _userManager;

	public TopicController(ApplicationDbContext context, UserManager<User> userManager) {
		_context = context;
		_userManager = userManager;
	}


	[HttpGet("{id}")]
	public async Task<IActionResult> GetTopic(string id) {
		var result = await _context.Topics.FindAsync(id);
		return result == null ? NotFound() : new OkObjectResult(result);
	}


	[HttpPost("create")]
	public void CreateTopic([FromBody] string value) {
	}

	[HttpGet("all")]
	public void GetAllTopics([FromBody] string value) {
	}

	[HttpPut("{id}")]
	public void UpdateTopic(string id, [FromBody] string value) {
	}

	[HttpDelete("{id}")]
	public void DeleteTopic(string id) {
	}


	[HttpGet("{topicId}/comment/all")]
	public void GetAllComments([FromBody] string value) {
	}

	[HttpGet("{topicId}/comment/{id}")]
	public async Task<IActionResult> GetComment(string topicId, string id) {
		var result = await _context.TopicComments.FindAsync(id);
		return result == null ? NotFound() : new OkObjectResult(result);
	}

	[HttpPost("{topicId}/comment/create")]
	public async Task<ActionResult> CreateComment(string topicId, [FromBody] CreateTopicCommentRequest comment) {
		var user = _userManager.FindByNameAsync(User.Identity?.Name);

        /*var item = new TopicComment {
			Content = comment.Content,
			TopicId = topicId,
			/*UserId = user.Id#1#
		};

		_context.TopicComments.Add(item);

		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(ItemByIdAsync), new { id = item.Id }, null);*/

		return BadRequest();
	}

	[HttpPut("{topicId}/comment/{id}")]
	public void UpdateComment(string topicId, [FromBody] string value, string id) {
	}

	[HttpDelete("{topicId}/comment/{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> DeleteComment(string topicId, string id) {
		var product = _context.TopicComments.SingleOrDefault(x => x.Id == id);

		if (product == null) {
			return NotFound();
		}

		_context.TopicComments.Remove(product);

		await _context.SaveChangesAsync();

		return NoContent();
	}
}