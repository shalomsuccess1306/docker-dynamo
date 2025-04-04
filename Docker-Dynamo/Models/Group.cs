namespace Docker_Dynamo.Models
{
    public class Groups
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GroupMember> Members { get; set; }
        public List<Expense> Expenses { get; set; }
    }

    public class GroupDTO
    {
        public string GroupName { get; set; }
        public int? UserId { get; set; }
    }

}
