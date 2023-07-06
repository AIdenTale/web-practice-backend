using Microsoft.AspNetCore.Identity;

namespace WebPracticeBackend.App.Models; 

public class User : IdentityUser {
	public ICollection<Topic> Topics { get; set; }
	public ICollection<TopicComment> TopicComments { get; set; }
}