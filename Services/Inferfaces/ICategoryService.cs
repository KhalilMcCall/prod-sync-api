
using ErrorOr;

public interface ICategoryService
{
    List<Category> GetCategories();
    ErrorOr<Category> CreateCategory(Category category);
}