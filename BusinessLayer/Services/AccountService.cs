using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BusinessLayer.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public AccountService(IAccountRepository accountRepository, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _accountRepository = accountRepository;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    public SystemAccount Authenticate(string email, string password)
    {
        var adminEmail = _configuration["Admin:Email"];
        var adminPassword = _configuration["Admin:Password"];
        var adminName = _configuration["Admin:Name"] ?? "Admin";

        if (email == adminEmail && password == adminPassword)
        {
            var adminAccount = new SystemAccount
            {
                AccountId = -1,
                AccountEmail = adminEmail,
                AccountName = adminName,
                AccountRole = "3",
                AccountPassword = adminPassword
            };

            _httpContextAccessor.HttpContext?.Session.SetString("UserEmail", adminAccount.AccountEmail);
            _httpContextAccessor.HttpContext?.Session.SetString("UserName", adminAccount.AccountName);
            _httpContextAccessor.HttpContext?.Session.SetString("UserRole", adminAccount.AccountRole);
            _httpContextAccessor.HttpContext?.Session.SetInt32("AccountId", adminAccount.AccountId);

            return adminAccount;
        }

        var account = _accountRepository.GetSystemAccountByEmail(email);

        if (account == null || account.AccountPassword != password)
        {
            return null;
        }

        _httpContextAccessor.HttpContext?.Session.SetString("UserEmail", account.AccountEmail);
        _httpContextAccessor.HttpContext?.Session.SetString("UserName", account.AccountName);
        _httpContextAccessor.HttpContext?.Session.SetString("UserRole", account.AccountRole);
        _httpContextAccessor.HttpContext?.Session.SetInt32("AccountId", account.AccountId);

        return account;
    }
    public SystemAccount GetAccountById(int? id)
    {
        return _accountRepository.GetSystemAccountById(id);
    }

    public IEnumerable<SystemAccount> GetAccounts()
    {
        return _accountRepository.GetSystemAccounts();
    }

    public SystemAccount CreateAccount(SystemAccount account)
    {
        var accountToCreate = new SystemAccount()
        {
            AccountName = account.AccountName,
            AccountEmail = account.AccountEmail,
            AccountRole = account.AccountRole,
            AccountPassword = account.AccountPassword
        };

        return _accountRepository.CreateSystemAccount(accountToCreate);
    }

    public SystemAccount UpdateAccount(SystemAccount account, int id)
    {
        if (account.AccountId != id) return null!;
        var existingAccount = _accountRepository.GetSystemAccountById(id);

        if (existingAccount.AccountName != account.AccountName)
        {
            existingAccount.AccountName = account.AccountName;
        }

        if (existingAccount.AccountEmail != account.AccountEmail)
        {
            existingAccount.AccountEmail = account.AccountEmail;
        }

        if (existingAccount.AccountRole != account.AccountRole)
        {
            existingAccount.AccountRole = account.AccountRole;
        }

        if (existingAccount.AccountPassword != account.AccountPassword)
        {
            existingAccount.AccountPassword = account.AccountPassword;
        }


        return _accountRepository.UpdateSystemAccount(account, account.AccountId);
    }

    public void DeleteAccount(int accountId)
    {
        _accountRepository.DeleteSystemAccountById(accountId);
    }
}