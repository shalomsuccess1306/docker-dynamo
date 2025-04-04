using CommonOps;
using Docker_Dynamo.Models;

namespace DockerDynamo.Services
{
    public interface IAuthServices
    {
      Task<StandardResponse<string>> loginUser(LoginModel loginModel);
        Task<StandardResponse<UserDTO>> RegisterUser(UserDTO user);

    }
}
