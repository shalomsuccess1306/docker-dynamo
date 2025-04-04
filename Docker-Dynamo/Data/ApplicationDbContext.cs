using Docker_Dynamo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Docker_Dynamo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Groups> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseParticipant> ExpenseParticipants { get; set; }

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupMember>().HasKey(gm => new { gm.GroupId, gm.UserId });
            modelBuilder.Entity<ExpenseParticipant>().HasKey(ep => new { ep.ExpenseId, ep.UserId });
        }
    }
}
