using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

// FIX: Namespace must be BlazorApp1.Models to match your project
namespace BlazorApp1.Models;

[Table("reports")]
public class ReportModel : BaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("description")]
    public string? Description { get; set; } // Added ? to fix warning

    [Column("location")]
    public string? Location { get; set; }    // Added ? to fix warning

    [Column("student_id")]
    public string? StudentId { get; set; }   // Added ? to fix warning

    [Column("image_url")]
    public string? ImageUrl { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("status")]
    public string Status { get; set; } = "Pending";
}