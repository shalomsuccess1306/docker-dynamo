using CommonOps;
using Docker_Dynamo.Data;
using Docker_Dynamo.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace DockerDynamo.Services
{
    public class GroupServices : IGroupServices
    {
        private readonly AppDbContext _context;
        public GroupServices(AppDbContext context)
        {
            _context = context;   
        }
        public async Task<StandardResponse<dynamic>> AddMemberToGroup(GroupMemberDTO model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
            if (user == null) return StandardResponse<dynamic>.ErrorMessage("This user is not does not have an account.");
            if (model.GroupId <= 0 || model.GroupId == null)
                return StandardResponse<dynamic>.ErrorMessage("Group Id cannot be null or less than zero");
            var groups = await _context.Groups.FindAsync(model.GroupId);
            if (groups == null) return StandardResponse<dynamic>.ErrorMessage("Group not found, Invalid groupId");

            // check if user is already in the group.

            bool isAlreadyMember = await _context.GroupMembers
             .AnyAsync(gm => gm.GroupId == model.GroupId && gm.UserId == model.UserId);
            if (isAlreadyMember) return StandardResponse<dynamic>.ErrorMessage("This user is already a member of this group");

            var newGroupMember = new GroupMember
            {
                UserId = model.UserId!.Value,
                GroupId = model.GroupId.Value
            };
            _context.GroupMembers.Add(newGroupMember);
            await _context.SaveChangesAsync();
            return StandardResponse<dynamic>.SuccessMessage("Member added successfully", newGroupMember);
        }

        public async Task<StandardResponse<dynamic>> CreateGroup(GroupDTO model)
        {
            if(string.IsNullOrEmpty(model.GroupName) || model.UserId == null || model.UserId <= 0)
                return StandardResponse<dynamic>.ErrorMessage("Group name and creator are required.");
            var group = new Groups();
            group.Name = model.GroupName;
            await _context.SaveChangesAsync();

            var groupMember = new GroupMember();
            groupMember.UserId = (int)model.UserId;
            groupMember.GroupId = group.Id;
            await _context.SaveChangesAsync();
            return StandardResponse<dynamic>.SuccessMessage("Group successfully created.", model);


        }

        public async Task<StandardResponse<Groups>> GetGroupById(int groupId)
        {
            var group = await _context.Groups
           .Include(g => g.Members)
           .Include(g => g.Expenses)
           .FirstOrDefaultAsync(g => g.Id == groupId);
            if (group == null) return StandardResponse<Groups>.ErrorMessage("Group Not Found");

            return StandardResponse<Groups>.SuccessMessage("",group);
        }

    }
}
