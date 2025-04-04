using CommonOps;
using Docker_Dynamo.Models;

namespace DockerDynamo.Services
{
    public interface IGroupServices
    {
        Task<StandardResponse<dynamic>> CreateGroup(GroupDTO model);
        Task<StandardResponse<dynamic>> AddMemberToGroup(GroupMemberDTO model);
        Task<StandardResponse<Groups>> GetGroupById(int groupId);
    }
}
