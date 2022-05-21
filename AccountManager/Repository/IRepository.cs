using AccountManager.Dto;

namespace AccountManager.Repository;

public interface IRepository<T>
{
    Task<int> AddAsync(T item);
    Task<IEnumerable<T>?> GetItemsAsync(string? searchValue);
    Task<int> GetIdForNameAsync(string name);
    Task<int> UpdateAsync(T updateItem);
    Task<int> DeleteAsync(int id);
}