using DAL.Models;

namespace DAL.Repositories;

public interface IAccountRepository
{
    SystemAccount CreateSystemAccount(SystemAccount systemAccount);
    
    SystemAccount GetSystemAccountById(int? id);

    IEnumerable<SystemAccount> GetSystemAccounts();
    
    SystemAccount GetSystemAccountByEmail(string email);
    
    SystemAccount UpdateSystemAccount(SystemAccount systemAccount, int id);
    
    void DeleteSystemAccountById(int id);
}