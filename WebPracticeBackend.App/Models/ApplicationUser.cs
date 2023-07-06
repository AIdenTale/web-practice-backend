using Microsoft.AspNetCore.Identity;

namespace WebPracticeBackend.App.Models; 

public class ApplicationUser : IdentityUser {
	public int Id { get; set; }
	public string Name { get; set; }
}