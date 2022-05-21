using AccountManager.Dto;

namespace AccountManager.Repository;

public interface IRepositoryAccount : IRepository<AccountDto>
{
    Task<int> GetAccountForCustomerId(int id);
    Task<IEnumerable<AccountDto>?> GetItemsAsync(string? searchValue, int? idFind);
}