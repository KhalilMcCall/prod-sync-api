public class Category
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public DateTime LastModifiedDate { get; set; }
}