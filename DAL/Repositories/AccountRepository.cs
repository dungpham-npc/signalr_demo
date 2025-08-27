using DAL.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly FunewsmanagementContext _context;

    public AccountRepository(FunewsmanagementContext context)
    {
        _context = context;
    }
    
    public SystemAccount CreateSystemAccount(SystemAccount systemAccount)
    {
        _context.SystemAccounts.Add(systemAccount);
        _context.SaveChanges();
        return systemAccount;
    }
    
    public SystemAccount GetSystemAccountById(int? id)
    {
        return _context.SystemAccounts.Find(id)!;
    }
    
    public SystemAccount GetSystemAccountByEmail(string email)
    {
        return _context.SystemAccounts.FirstOrDefault(a => a.AccountEmail == email)!;
    }

    public IEnumerable<SystemAccount> GetSystemAccounts()
    {
        return _context.SystemAccounts.ToList();
    }
    
    public SystemAccount UpdateSystemAccount(SystemAccount systemAccount, int id)
    {
        if (systemAccount.AccountId != id) return null!;
        var existingAccount = _context.SystemAccounts.Find(id);
        if (existingAccount == null) return null!;

        _context.Entry(existingAccount).CurrentValues.SetValues(systemAccount);
        _context.SaveChanges();
        return existingAccount;
    }

    
    public void DeleteSystemAccountById(int id)
    {
        var account = _context.SystemAccounts.Find(id);
        if (account == null) return;
        _context.SystemAccounts.Remove(account);
        _context.SaveChanges();
    }
}