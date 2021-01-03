using ASCOM.Alpaca.Simulators;
using System.Threading.Tasks;

namespace ASCOM.Alpaca
{
    public interface IUserService
    {
        Task<bool> Authenticate(string username, string password);
    }

    public class UserService : IUserService
    {
        //ToDo, don't hard code support multiple users
        public async Task<bool> Authenticate(string username, string password)
        {
            return await Task.Run(() =>
            {
                try
                {
                    return username == ServerSettings.UserName && Hash.Validate(ServerSettings.Password, password);
                }
                catch
                {
                    return false;
                }
            }

            );
        }
    }
}