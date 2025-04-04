using CommonOps;
using Docker_Dynamo.Models;
using DockerDynamo.Models;

namespace DockerDynamo.Services
{
    public interface IExpenseServices
    {
        Task<StandardResponse<dynamic>> AddExpense(AddExpenseRequest expense);
        Task<StandardResponse<dynamic>> GetIndividualExpensesFromGroup(int groupId);
        Task<StandardResponse<dynamic>> GetTotalGroupExpenses(int groupId);
        Task<StandardResponse<dynamic>> GetUserExpenseDetails(int userId);

    }
}
