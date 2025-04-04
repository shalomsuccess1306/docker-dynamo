namespace Docker_Dynamo.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public decimal Amount { get; set; }
        public int PayerId { get; set; }
        public string? Description { get; set; }
        public DateTime DateCreated { get; set; }
        public List<ExpenseParticipant>? Participants { get; set; }
    }
}
