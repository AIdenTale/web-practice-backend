namespace WebPracticeBackend.App.Models; 

public class TopicComment {
	public int Id { get; set; }
	public int TopicId { get; set; }
	public Topic topic { get; set; }
	public string Content { get; set; }
	public int CreatorId { get; set; }
}