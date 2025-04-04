namespace Docker_Dynamo.Models
{
    public class ExpenseParticipant
    {
        public int ExpenseId { get; set; }
        public int UserId { get; set; }
        public Expense Expense { get; set; }
        public User User { get; set; }
        public decimal SharedAmount { get; set; }
        public bool IsSettled { get; set; }
    }
}
