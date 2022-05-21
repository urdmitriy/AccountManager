using AccountManager.Data;
using AccountManager.Dto;
using AccountManager.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AccountManager.Pages;

public class AccountPageRazor : ComponentBase
{
    protected  IEnumerable<CustomerDto>? Customers;
    protected  IEnumerable<AccountDto>? Accounts;
    protected bool IsShowModalEditAccount;
    protected bool IsShowModalEditCustomer;
    protected bool IsShowModalError;
    protected string? Message;
    protected AccountDto? CurrentAccount = new ();
    protected CustomerDto? CurrentCustomer = new ();
    private readonly IRepositoryAccount _repositoryAccount = new RepositoryAccounts();
    private readonly IRepositoryCustomers _repositoryCustomers = new RepositoryCustomers();

    [Inject]
    private IJSRuntime _jsRuntime { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Customers = await _repositoryCustomers.GetItemsAsync(null);
        Accounts = new List<AccountDto>();
    }
    

    protected async Task UpdateAccounts()
    {
        Accounts = await _repositoryAccount.GetItemsAsync(null, CurrentCustomer?.Id);
    }

    protected void ShowEditAccountWindow(int id)
    {
        CurrentAccount = Accounts?.FirstOrDefault(x => x.Id == id);
        IsShowModalEditAccount = true;
    }
    protected void ShowEditCustomerWindow(int id)
    {
        if (id==0) return;
        CurrentCustomer = Customers?.FirstOrDefault(x => x.Id == id);
        IsShowModalEditCustomer = true;
    }
    protected void ShowAddAccountWindow()
    {
        CurrentAccount = new AccountDto();
        IsShowModalEditAccount = true;
    }
    protected void ShowAddCustomerWindow()
    {
        CurrentCustomer = new CustomerDto();
        IsShowModalEditCustomer = true;
    }
    protected async Task<int> UpdateAccountAsync(AccountDto? account)
    {
        int result;
        if (account == null) return 0;
        if (account.Id != 0)
        {
            result = await _repositoryAccount.UpdateAsync(account);
        }
        else
        {
            result = await _repositoryAccount.AddAsync(account);
        }

        Accounts = await _repositoryAccount.GetItemsAsync(null, CurrentCustomer?.Id);
        IsShowModalEditAccount = false;
        return result;
    }
    protected async Task<int> UpdateCustomerAsync(CustomerDto? customer)
    {
        int result;
        if (customer == null) return 0;
        if (customer.Id != 0)
        {
            result = await _repositoryCustomers.UpdateAsync(customer);
        }
        else
        {
            result = await _repositoryCustomers.AddAsync(customer);
        }

        IsShowModalEditCustomer = false;
        return result;
    }
    protected async Task<int> UpdatePasswordAccountAsync(AccountDto account)
    {
        var req = await _jsRuntime.InvokeAsync<bool>("confirm", "Подтверждаете изменение пароля?");
        if (!req) return 0;
        account.Password = Service.GeneratePassword(6);
        var result = await _repositoryAccount.UpdateAsync(account);
        Accounts = await _repositoryAccount.GetItemsAsync(null,CurrentCustomer?.Id);
        return result;
    }

    protected async Task<int> DeleteCustomerAsync(int id)
    {
        var result=-1;
        if (id == 0) return 0;
        var count = await _repositoryAccount.GetAccountForCustomerId(id);
        if (count == 0)
        {
            result = await _repositoryCustomers.DeleteAsync(id);
        }
        else
        {
            ShowMessage("У этой организации есть аккаунты, организацию с аккаунтами удалить нельзя");
        }
        
        return result;
    }
    protected async Task<int> DeleteAccountAsync(int id)
    {
        var req = await _jsRuntime.InvokeAsync<bool>("confirm", "Подтверждаете удаление аккаунта?");
        if (!req) return 0;
        int result=-1;
        if (id == 0) return 0;
        
        result = await _repositoryAccount.DeleteAsync(id);
        IsShowModalEditAccount = false;
        Accounts = await _repositoryAccount.GetItemsAsync(null, CurrentCustomer?.Id);
        return result;
    }

    private void ShowMessage(string message)
    {
        Message = message;
        IsShowModalError = true;
    }
}