namespace WebPracticeBackend.App.Models;

public class TopicComment {
	public string Id { get; set; }
	public string TopicId { get; set; }
	public Topic Topic { get; set; }
	public string Content { get; set; }
	public string UserId { get; set; }
	public User User { get; set; }
}