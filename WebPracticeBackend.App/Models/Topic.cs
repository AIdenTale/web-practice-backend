namespace WebPracticeBackend.App.Models; 

public class Topic {
	public int id { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public int CreatorId { get; set; }
	public DateTime DateOfCreation { get; set; }
}