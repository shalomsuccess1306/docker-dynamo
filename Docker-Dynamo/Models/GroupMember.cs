namespace Docker_Dynamo.Models
{
    public class GroupMember
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public Groups Group { get; set; }
        public User User { get; set; }
    }
    public class GroupMemberDTO
    {
        public int? GroupId { get; set; }
        public int? UserId { get; set; }
    }
}
