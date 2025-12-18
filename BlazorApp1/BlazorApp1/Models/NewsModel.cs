using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace AdminPanel.Models;

[Table("news")]
public class NewsModel : BaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("title")]
    public string Title { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}