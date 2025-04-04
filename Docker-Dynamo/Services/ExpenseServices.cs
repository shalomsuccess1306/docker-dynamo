using CommonOps;
using Docker_Dynamo.Data;
using Docker_Dynamo.Models;
using DockerDynamo.Models;
using Microsoft.EntityFrameworkCore;

namespace DockerDynamo.Services
{
    public class ExpenseServices : IExpenseServices
    {
        private readonly AppDbContext _context;
        public ExpenseServices(AppDbContext context)
        {

            _context = context;

        }
        public async Task<StandardResponse<dynamic>> AddExpense(AddExpenseRequest request)
        {
            var groupMembers = await _context.GroupMembers
                                               .Where(gm => gm.GroupId == request.GroupID)
                                               .ToListAsync();
            if (groupMembers.Count == 0)
                return StandardResponse<dynamic>.ErrorMessage("No members in the group");
            var expense = new Expense
            {
                GroupId = request.GroupID,
                PayerId = request.UserId,
                Amount = request.Amount,
                Description = request.Description,
                DateCreated = request.DateCreated
            };
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            decimal shareAmount = expense.Amount / groupMembers.Count;
            foreach (var member in groupMembers)
            {
                _context.ExpenseParticipants.Add(new ExpenseParticipant
                {
                    ExpenseId = expense.Id,
                    UserId = member.UserId,
                    SharedAmount = shareAmount,
                    IsSettled = false
                });
            }
            await _context.SaveChangesAsync();
            return StandardResponse<dynamic>.SuccessMessage("Expense added successfully", expense);
        }

        public async Task<StandardResponse<dynamic>> GetIndividualExpensesFromGroup(int groupId)
        {
            var expenses = await _context.Expenses
                       .Where(e => e.GroupId == groupId)
                       .Include(e => e.Participants)
                           .ThenInclude(es => es.User) 
                        .OrderByDescending(e => e.DateCreated)
                        .ToListAsync();

            if(expenses.Any()) return StandardResponse<dynamic>.ErrorMessage("No expenses found for this group.");

            var result = expenses.Select(expense => new
            {
                ExpenseID = expense.Id,
                PaidByUserID = expense.PayerId,
                 PaidByUserName = _context.Users
                .Where(u => u.Id == expense.PayerId)
                .Select(u => u.Name)
                .FirstOrDefault(),
                Amounts = expense.Amount,
                Descriptions = expense.Description,
                Created = expense.DateCreated,
                Shares = expense.Participants.Select(share => new
                {
                    UserID = share.UserId,
                    UserName = share.User.Name,
                    ShareAmount = share.SharedAmount,
                    IsSettledDebt = share.IsSettled
                }).ToList()
            });
            return StandardResponse<dynamic>.SuccessMessage("", expenses);

        }

        public async Task<StandardResponse<dynamic>> GetUserExpenseDetails(int userId)
        {
                var user = await _context.Users
                                        .Where(u => u.Id == userId)
                                        .Select(u => new
                                        {
                                            UserID = u.Id,
                                            UserName = u.Name,
                                            EmailAddress = u.Email
                                        })
                                        .FirstOrDefaultAsync();
            if (user == null) return StandardResponse<dynamic>.ErrorMessage("User Not Found");

            // Get total amount user has been assigned (settled + unsettled)
            decimal totalExpenses = await _context.ExpenseParticipants
                .Where(es => es.UserId == userId)
                .SumAsync(es => es.SharedAmount);

            // Get total amount user has settled
            decimal totalSettled = await _context.ExpenseParticipants
                                                .Where(es => es.UserId == userId && es.IsSettled) 
                                                .SumAsync(es => es.SharedAmount);
            if (totalSettled == 0) return StandardResponse<dynamic>.SuccessMessage($"Not outstanding debt for this user :{user.UserName}", totalSettled);

            // Calculate unsettled amount
            decimal totalUnsettled = totalExpenses - totalSettled;




            return StandardResponse<dynamic>.SuccessMessage("",(new {User = user,TotalExpense = totalExpenses, TotalSettled = totalSettled, TotalUnSettled = totalUnsettled }));
        }

        public async Task<StandardResponse<dynamic>> GetTotalGroupExpenses(int groupId)
        {
            var group = await _context.Groups
                                   .Where(g => g.Id == groupId)
                                   .Select(g => new
                                   {
                                       GroupID = g.Id,
                                       GroupName = g.Name
                                   })
                                   .FirstOrDefaultAsync();
            if (group == null) return StandardResponse<dynamic>.ErrorMessage("Group not found");

            // get total expenses in the group
            decimal totalExpenses = await _context.Expenses
                                                .Where(e => e.Id == groupId)
                                                .SumAsync(e => e.Amount);
            // Get the number of expenses recorded
            int expenseCount = await _context.Expenses
                .Where(e => e.Id == groupId)
                .CountAsync();


            return StandardResponse<dynamic>.SuccessMessage("",(new {Group =group, TotalExpenses = totalExpenses, TotalRecordedExpense = expenseCount}));
        }
    }
}
