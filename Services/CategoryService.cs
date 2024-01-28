using ErrorOr;

public class CategoryService : ICategoryService
{
    private readonly ProdSyncContext _context;

    public CategoryService(ProdSyncContext context)
    {
        _context = context;
    }

    public List<Category> GetCategories()
    {
        return _context.Categories.ToList();
    }
    public ErrorOr<Category> CreateCategory(Category category)
    {

        var c = _context.Categories.SingleOrDefault(x => x.Code == category.Code);

        if (c != null)
        {
            return Errors.Category.CategoryCodeExists;
        }

        //Generate Guids and Set Dates
        var d = DateTime.UtcNow;

        category.Id = Guid.NewGuid();
        category.CreatedDate = d;
        category.LastModifiedDate = d;

        //Add Guids To Context
        // _context.Categories.Add(category);
        // _context.SaveChanges();
        return category;
    }
}