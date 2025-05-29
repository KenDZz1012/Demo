using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Lib.Keycloak
{
    public interface IKeycloakService
    {
        Task<string> GetAccessTokenAsync();
        Task CreateUserAsync(KeycloakUserDto userDto);
        Task AssignRoleAsync(string userId, string roleName);
        Task UpdatePasswordAsync(string userID, string newPassword);
        Task<string?> GetUserIdByUsernameAsync(string username);
    }
}
