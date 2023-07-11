using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
	[Route("GetTopic/{id}")]
    public async Task<IActionResult> GetTopic(string id) {
		var result = await _context.Topics.FindAsync(id);
		return result == null ? NotFound() : new OkObjectResult(result);
	}


	[HttpPost("create")]
	public void CreateTopic([FromBody] string value) {
	}

	[HttpGet("all")]
	public async Task<ActionResult<IEnumerable<Topic>>> GetAllTopics()
	{
		var topics = await _context.Topics.ToListAsync();
		return Ok(topics);
	}

    [HttpPut("{id}")]
	public async Task<ActionResult> UpdateTopic(string id, [FromBody] string value)
	{
		var topic = await _context.Topics.SingleOrDefaultAsync(t => t.Id == id);

		if (topic == null)
		{
			return NotFound(new { Message = $"Topic with id {id} not found." });
		}
		
		topic.Title = value;
		topic.Description = value;
		_context.Topics.Update(topic);
		await _context.SaveChangesAsync();

		return NoContent();
	}

    [HttpDelete("{id}")]
	public async Task<ActionResult> DeleteTopic(string id)
	{
		var topic = _context.Topics.SingleOrDefault(t => t.Id == id);

		if (topic == null)
		{
			return NotFound();
		}

		_context.Topics.Remove(topic);
		await _context.SaveChangesAsync();

		return NoContent();
	}


    [HttpGet("{topicId}/comment/all")]
	public async Task<ActionResult<IEnumerable<TopicComment>>> GetAllComments(string topicId)
	{
		var comments = await _context.TopicComments.Where(c => c.TopicId == topicId).ToListAsync();
		return Ok(comments);
	}

    [HttpGet("{topicId}/comment/{id}")]
	public async Task<IActionResult> GetComment(string topicId, string id) {
		var result = await _context.TopicComments.FindAsync(id);
		return result == null ? NotFound() : new OkObjectResult(result);
	}

	[HttpPost]
	[Route("{topicId}/comment/create")]
    public async Task<ActionResult> Post(string topicId, [FromBody] CreateTopicCommentRequest comment) {
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
	public async Task<ActionResult> UpdateComment(string topicId, string id, [FromBody] string value)
	{
		var comment = await _context.TopicComments.SingleOrDefaultAsync(c => c.Id == id && c.TopicId == topicId);

		if (comment == null)
		{
			return NotFound(new { Message = $"Comment with id {id} not found in topic {topicId}." });
		}
		
		comment.Content = value;
		_context.TopicComments.Update(comment);
		await _context.SaveChangesAsync();

        return NoContent();
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