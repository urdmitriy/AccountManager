using AccountManager.Data;
using AccountManager.Dto;
using Dapper;
using Npgsql;

namespace AccountManager.Repository;

public class RepositoryCustomers : IRepositoryCustomers
{
    public async Task<int> AddAsync(CustomerDto customer)
    {
        return -1;
    }

    public async Task<IEnumerable<CustomerDto>?> GetItemsAsync(string? searchValue)
    {
        IEnumerable<CustomerDto>? result;
        await using (var connection = new NpgsqlConnection(Connection.GetConnectingString()))
        {
            var sql = "SELECT * FROM \"Customers\"";
            result = await connection.QueryAsync<CustomerDto>(sql);
        }

        return result;
    }

    public async Task<int> GetIdForNameAsync(string name)
    {
        return -1;
    }

    public async Task<int> UpdateAsync(CustomerDto updateItem)
    {
        return -1;
    }

    public async Task<int> DeleteAsync(int id)
    {
        return -1;
    }


}