using System.Reflection.Metadata;
using AccountManager.Data;
using AccountManager.Dto;
using Dapper;
using Npgsql;

namespace AccountManager.Repository;

public class RepositoryAccounts : IRepositoryAccount
{
    public async Task<int> AddAsync(AccountDto account)
    {
        int result;
        await using (var connection = new NpgsqlConnection(Connection.GetConnectingString()))
        {
            result = await connection.ExecuteAsync(
                "INSERT INTO \"Accounts\" (\"IdCustomer\", \"Name\", \"Email\", \"Login\", \"Password\") VALUES (@idCustomer,@name,@email,@login,@password)",
                new
                {
                    idCustomer = account.IdCustomer,
                    name = account.Name,
                    email = account.Email,
                    login = account.Login,
                    password = account.Password
                });
        }
        
        return result;
    }


    public async Task<IEnumerable<AccountDto>?> GetItemsAsync(string? searchValue)
    {
        IEnumerable<AccountDto>? result;

        await using (var connection = new NpgsqlConnection(Connection.GetConnectingString()))
        {
            result = await connection.QueryAsync<AccountDto>("SELECT * FROM \"Accounts\"");
        }

        return result;
    }
    public async Task<IEnumerable<AccountDto>?> GetItemsAsync(string? searchValue, int? idFind)
    {
        IEnumerable<AccountDto>? result;

        await using (var connection = new NpgsqlConnection(Connection.GetConnectingString()))
        {
            result = await connection.QueryAsync<AccountDto>($"SELECT * FROM \"Accounts\" WHERE \"IdCustomer\" = {idFind}");
        }

        return result;
    }
    
    public async Task<int> GetIdForNameAsync(string name)
    {
        return -1;
    }

    public async Task<int> UpdateAsync(AccountDto updateItem)
    {
        int result;
        await using (var connection = new NpgsqlConnection(Connection.GetConnectingString()))
        {
            result = await connection.ExecuteAsync(
                "UPDATE \"Accounts\" SET \"IdCustomer\" = @idCustomer, \"Name\" = @name, \"Email\" = @email, \"Login\" = @login, \"Password\" = @password WHERE \"Id\" = @id",
                new
                {
                    id = updateItem.Id,
                    idCustomer = updateItem.IdCustomer,
                    name = updateItem.Name,
                    email = updateItem.Email,
                    login = updateItem.Login,
                    password = updateItem.Password
                });
        }
        
        return result;
    }

    public async Task<int> DeleteAsync(int id)
    {
        int result = -1;
        await using (var connection = new NpgsqlConnection(Connection.GetConnectingString()))
        {
            result = await connection.ExecuteAsync($"DELETE FROM \"Accounts\" WHERE \"Id\"={id}");
        }
        return result;
    }

    public async Task<int> GetAccountForCustomerId(int id)
    {
        int result = -1;
        await using (var connection = new NpgsqlConnection(Connection.GetConnectingString()))
        {
            result = await connection.QueryFirstOrDefaultAsync<int>(
                $"SELECT count(\"Login\") FROM \"Accounts\" WHERE \"IdCustomer\" = {id}");
        }

        return result;
    }

    
}