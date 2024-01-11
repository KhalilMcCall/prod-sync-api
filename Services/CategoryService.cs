public class CategoryService : ICategoryService
{
    private readonly ProdSyncContext _context;

    public CategoryService(ProdSyncContext context)
    {
        Console.WriteLine("Categories Service Created!");
        _context = context;
    }

    public List<Category> GetCategories()
    {
        return _context.Categories.ToList();
    }
    public void CreateCategory(Category category)
    {

        //Generate Guids and Set Dates
        var d = DateTime.UtcNow;

        category.Id = Guid.NewGuid();
        category.CreatedDate = d;
        category.LastModifiedDate = d;

        //Add Guids To Context
        _context.Categories.Add(category);
        _context.SaveChanges();
    }
}