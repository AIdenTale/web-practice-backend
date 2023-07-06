using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebPracticeBackend.App.Models;

namespace WebPracticeBackend.App.Data {
	public class ApplicationDbContext : ApiAuthorizationDbContext<User> {
		public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
			: base(options, operationalStoreOptions) {
		}
		public DbSet<Topic> Topics { get; set; }
		public DbSet<TopicComment> TopicComments { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<TopicComment>()
				.HasOne(comment => comment.Topic)
				.WithMany(topic => topic.Comments)
				.HasForeignKey(comment => comment.TopicId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Topic>()
				.HasOne(topic => topic.User)
				.WithMany(user => user.Topics)
				.HasForeignKey(topic => topic.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<TopicComment>()
				.HasOne(comment => comment.User)
				.WithMany(user => user.TopicComments)
				.HasForeignKey(comment => comment.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<TopicComment>()
				.HasKey(comment => comment.Id);

			modelBuilder.Entity<Topic>()
				.HasKey(topic => topic.Id);
		}
	}
}