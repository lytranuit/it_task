
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using Vue.Models;
using System.ComponentModel.DataAnnotations;
using workflow.Areas.V1.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DiagnosticAdapter;
using System.Data.Common;

namespace Vue.Data
{
	public class ItContext : DbContext
	{
		private IActionContextAccessor actionAccessor;
		private UserManager<UserModel> UserManager;
		public ItContext(DbContextOptions<ItContext> options, UserManager<UserModel> UserMgr, IActionContextAccessor ActionAccessor) : base(options)
		{
			actionAccessor = ActionAccessor;
			UserManager = UserMgr;

		}

		public DbSet<ProjectStatusModel> ProjectStatusModel { get; set; }
		public DbSet<TaskModel> TaskModel { get; set; }
		public DbSet<TaskCommentModel> TaskCommentModel { get; set; }
		public DbSet<TaskCommentFileModel> TaskCommentFileModel { get; set; }
		public DbSet<TaskAssigneeModel> TaskAssigneeModel { get; set; }
		public DbSet<TaskFavoriteModel> TaskFavoriteModel { get; set; }
		public DbSet<ProjectModel> ProjectModel { get; set; }
		public DbSet<ProjectAssigneeModel> ProjectAssigneeModel { get; set; }
		public DbSet<AuditTrailsModel> AuditTrailsModel { get; set; }

		public DbSet<UserModel> UserModel { get; set; }
		public DbSet<UserRoleModel> UserRoleModel { get; set; }

		public DbSet<EventModel> EventModel { get; set; }

		//public DbSet<User2Model> User2Model { get; set; }
		public DbSet<EmailModel> EmailModel { get; set; }
		public DbSet<TokenModel> TokenModel { get; set; }
		public DbSet<UserManagerModel> UserManagerModel { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserRoleModel>().ToTable("AspNetUserRoles").HasKey(table => new
			{
				table.RoleId,
				table.UserId
			});
			modelBuilder.Entity<TaskAssigneeModel>().ToTable("taskAssignee").HasKey(table => new
			{
				table.taskId,
				table.userId
			});
			modelBuilder.Entity<TaskFavoriteModel>().ToTable("taskFavorite").HasKey(table => new
			{
				table.taskId,
				table.userId
			});
			modelBuilder.Entity<ProjectAssigneeModel>().ToTable("projectAssignee").HasKey(table => new
			{
				table.projectId,
				table.userId
			});

			modelBuilder.Entity<UserModel>().HasMany(e => e.list_users).WithOne(e => e.user).HasForeignKey(e => e.userId);

			modelBuilder.Entity<UserModel>().HasOne(e => e.userreport).WithOne(e => e.userManager).HasForeignKey<UserManagerModel>(e => e.userManagerId);


			//modelBuilder.Entity<UserManagerModel>().HasOne(e => e.user);

		}
		public override int SaveChanges()
		{
			OnBeforeSaveChanges();
			return base.SaveChanges();
		}
		private void OnBeforeSaveChanges()
		{
			ChangeTracker.DetectChanges();
			var auditEntries = new List<AuditEntry>();
			var user_http = actionAccessor.ActionContext.HttpContext.User;
			var user_id = UserManager.GetUserId(user_http);
			var changes = ChangeTracker.Entries();
			foreach (var entry in changes)
			{
				if (entry.Entity is AuditTrailsModel || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
					continue;

				var auditEntry = new AuditEntry(entry);
				auditEntry.TableName = entry.Entity.GetType().Name;
				auditEntry.UserId = user_id;
				auditEntries.Add(auditEntry);
				foreach (var property in entry.Properties)
				{

					string propertyName = property.Metadata.Name;
					if (property.Metadata.IsPrimaryKey())
					{
						auditEntry.KeyValues[propertyName] = property.CurrentValue;
						continue;
					}
					switch (entry.State)
					{
						case EntityState.Added:
							auditEntry.AuditType = AuditType.Create;
							auditEntry.NewValues[propertyName] = property.CurrentValue;
							break;
						case EntityState.Deleted:
							auditEntry.AuditType = AuditType.Delete;
							auditEntry.OldValues[propertyName] = property.OriginalValue;
							break;
						case EntityState.Modified:
							if (property.IsModified)
							{
								var Original = entry.GetDatabaseValues().GetValue<object>(propertyName);
								var Current = property.CurrentValue;
								if (JsonConvert.SerializeObject(Original) == JsonConvert.SerializeObject(Current))
									continue;
								auditEntry.ChangedColumns.Add(propertyName);
								auditEntry.AuditType = AuditType.Update;
								auditEntry.OldValues[propertyName] = Original;
								auditEntry.NewValues[propertyName] = Current;

							}
							break;
					}

				}
			}
			foreach (var auditEntry in auditEntries)
			{
				AuditTrailsModel.Add(auditEntry.ToAudit());
			}
		}

		protected override void ConfigureConventions(ModelConfigurationBuilder builder)
		{
		}
	}
	public class FilterIdRaw
	{
		[Key]
		public int ItemId { get; set; }
		public string Name { get; set; }
	}
	public class SizeRaw
	{
		[Key]
		public long Size { get; set; }
	}
	public class CommandInterceptor
	{
		[DiagnosticName("Microsoft.EntityFrameworkCore.Database.Command.CommandExecuting")]
		public void OnCommandExecuting(DbCommand command, DbCommandMethod executeMethod, Guid commandId, Guid connectionId, bool async, DateTimeOffset startTime)
		{
			var secondaryDatabaseName = "OrgData";
			var schemaName = "dbo";
			var list_talbe = new List<string>()
			{
				"AspNetUsers","AspNetUserRoles","emails","Token"
			};
			//var tableName = "AspNetUsers";
			foreach (var tableName in list_talbe)
			{
				command.CommandText = command.CommandText.Replace($" [{tableName}]", $" [{schemaName}].[{tableName}]")
													 .Replace($" [{schemaName}].[{tableName}]", $" [{secondaryDatabaseName}].[{schemaName}].[{tableName}]");
			}


		}
	}
}
