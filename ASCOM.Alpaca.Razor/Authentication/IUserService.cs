using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.Alpaca
{
    public interface IUserService
    {
        Task<bool> Authenticate(string username, string password);

        bool UseAuth
        {
            get;
        }
    }
}
