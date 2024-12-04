using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Fakes.Handlers.OperationClaims;
using Business.Fakes.Handlers.User;
using Business.Fakes.Handlers.UserClaims;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Helpers
{
    public static class OperationClaimCreatorMiddleware
    {
        public static async Task UseDbOperationClaimCreator(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var internalOperationClaimService = scope.ServiceProvider.GetService<IInternalOperationClaimService>();
            var internalUserService = scope.ServiceProvider.GetService<IInternalUserService>();
            var internalUserClaimService = scope.ServiceProvider.GetService<IInternalUserClaimService>();

            if (internalOperationClaimService == null || internalUserService == null || internalUserClaimService == null)
            {
                throw new Exception("Required services are not registered.");
            }

            // Ensure Operation Claims
            foreach (var claim in StaticOperationClaims)
            {
                var claimExists = await internalOperationClaimService.GetClaimByNameAsync(claim);
                if (!claimExists.Success || claimExists.Data == null)
                {
                    await internalOperationClaimService.CreateInternalClaimAsync(claim);
                }
            }

            // Ensure Roles and Users
            foreach (var role in StaticRoles)
            {
                var userEmail = $"{role.ToLower()}@example.com";
                var userPassword = $"{role}123!";
                var userFullName = $"{role} User";

                // Check or Create User
                var userResult = await internalUserService.GetUserByEmailAsync(userEmail);
                var userId = userResult.Data?.UserId ?? 0;

                if (userId == 0)
                {
                    var createUserResult = await internalUserService.RegisterInternalUserAsync(userEmail, userPassword, userFullName);
                    if (createUserResult.Success)
                    {
                        userId = createUserResult.Data.UserId;
                    }
                    else
                    {
                        Console.WriteLine($"Error creating user {userEmail}: {createUserResult.Message}");
                        continue;
                    }
                }

                // Ensure Role Claims
                if (RoleClaims.TryGetValue(role, out var claims))
                {
                    foreach (var claim in claims)
                    {
                        var userClaimResult = await internalUserClaimService.CheckUserClaimAsync(userId, claim);
                        if (!userClaimResult.Success || userClaimResult.Data == null)
                        {
                            await internalUserClaimService.AssignClaimToUserAsync(userId, claim);
                        }
                    }
                }
            }
        }

        private static readonly List<string> StaticRoles = new List<string>
        {
            "Admin",
            "School",
            "Trainer",
            "Parent",
            "Student"
        };

        private static readonly List<string> StaticOperationClaims = new List<string>
        {
            "ManageStudents",
            "ViewReports",
            "ManagePayments",
            "TrackProgress",
            "ManageBranches",
            "ManageTrainers",
            "ViewBranchReports"
        };

        private static readonly Dictionary<string, List<string>> RoleClaims = new Dictionary<string, List<string>>
        {
            {
                "Admin",
                new List<string>
                {
                    "ManageStudents", "ViewReports", "ManagePayments", "TrackProgress", "ManageBranches",
                    "ManageTrainers", "ViewBranchReports"
                }
            },
            { "School", new List<string> { "ManageStudents", "ViewBranchReports", "ManageTrainers" } },
            { "Trainer", new List<string> { "ManageStudents", "TrackProgress" } },
            { "Parent", new List<string> { "ViewReports", "TrackProgress" } },
            { "Student", new List<string> { "TrackProgress" } }
        };
    }
}