using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMzalogApp.Sevices
{
    public interface IAccountRepository
    {
        Task<string> UpdateAccountStatus(string otNom, int status, string ot_uid);
        Task<int> GetAccountStatus(string userName);
    }
}
