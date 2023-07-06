namespace WebPracticeBackend.App.Models; 

public class Topic {
	public string Id { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string UserId { get; set; }
	public DateTime DateOfCreation { get; set; }
	public ICollection<TopicComment> Comments { get; set; }
	public User User { get; set; }
}