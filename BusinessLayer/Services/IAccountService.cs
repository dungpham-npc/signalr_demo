using DAL.Models;

namespace BusinessLayer.Services;

public interface IAccountService
{
    SystemAccount Authenticate(string email, string password);

    SystemAccount GetAccountById(int? id);

    IEnumerable<SystemAccount> GetAccounts();

    SystemAccount UpdateAccount(SystemAccount systemAccount, int id);

    SystemAccount CreateAccount(SystemAccount systemAccount);

    void DeleteAccount(int id);

}