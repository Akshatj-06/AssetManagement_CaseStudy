using System.ComponentModel.DataAnnotations;

namespace AssetManagementWebApplication.Model
{
	public class AuditRequest
	{
		[Key]
		public int AuditId { get; set; }
		public int AdminId { get; set; }
		public int UserId { get; set; }
		public required string AuditStatus { get; set; }
		public DateTime AuditDate { get; set; } = DateTime.UtcNow;

		public User? Admin { get; set; }
		public User? User { get; set; }
	}
}
