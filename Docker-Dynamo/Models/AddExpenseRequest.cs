namespace DockerDynamo.Models
{
    public class AddExpenseRequest
    {
        public int GroupID { get; set; }  
        public int UserId { get; set; }
        public decimal Amount { get; set; } 
        public string? Description { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow; 
    }
}
