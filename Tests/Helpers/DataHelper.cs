using Core.Entities.Concrete;
using Core.Utilities.Security.Hashing;
using Entities.Concrete;

namespace Tests.Helpers;

public static class DataHelper
{
    public static Trainer GetTrainer(string name = "Test")
    {
        HashingHelper.CreatePasswordHash("123456", out var passwordSalt, out var passwordHash);
        return new Trainer
        {
            Id = 1,
            FullName = $"{name} Trainer",
            Email = $"{name.ToLower()}@trainer.com",
            CitizenId = 12345678910,
            Status = true,
            BirthDate = new DateTime(1985, 1, 1),
            Gender = 1,
            MobilePhones = "5551234567",
            Address = "Trainer Address",
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = "trainer",
            Specialization = "Swimming",
            Sessions = new List<Session>()
        };
    }

    public static Student GetStudent(string name = "Test")
    {
        HashingHelper.CreatePasswordHash("123456", out var passwordSalt, out var passwordHash);
        return new Student
        {
            Id = 1,
            FullName = $"{name} Student",
            Email = $"{name.ToLower()}@student.com",
            CitizenId = 12345678911,
            Status = true,
            BirthDate = new DateTime(2010, 1, 1),
            Gender = 1,
            MobilePhones = "5551234568",
            Address = "Student Address",
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = "student",
            BranchId = 1,
            ParentId = 1,
            StudentSessions = new List<StudentSession>(),
            Payments = new List<Payment>(),
            ProgressRecords = new List<Progress>()
        };
    }

    public static School GetSchool(string name = "Test")
    {
        HashingHelper.CreatePasswordHash("123456", out var passwordSalt, out var passwordHash);
        return new School
        {
            Id = 1,
            FullName = $"{name} School Admin",
            Email = $"{name.ToLower()}@school.com",
            CitizenId = 12345678912,
            Status = true,
            BirthDate = new DateTime(1980, 1, 1),
            Gender = 1,
            MobilePhones = "5551234569",
            Address = "School Address",
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = "school",
            SchoolName = $"{name} School",
            SchoolAddress = "School Main Address",
            SchoolPhone = "5551234570",
            Branches = new List<Branch>()
        };
    }

    public static Parent GetParent(string name = "Test")
    {
        HashingHelper.CreatePasswordHash("123456", out var passwordSalt, out var passwordHash);
        return new Parent
        {
            Id = 1,
            FullName = $"{name} Parent",
            Email = $"{name.ToLower()}@parent.com",
            CitizenId = 12345678913,
            Status = true,
            BirthDate = new DateTime(1975, 1, 1),
            Gender = 1,
            MobilePhones = "5551234571",
            Address = "Parent Address",
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = "parent",
            Children = new List<Student>()
        };
    }

    public static Branch GetBranch(string name = "Test")
    {
        HashingHelper.CreatePasswordHash("123456", out var passwordSalt, out var passwordHash);
        return new Branch
        {
            Id = 1,
            FullName = $"{name} Branch Admin",
            Email = $"{name.ToLower()}@branch.com",
            CitizenId = 12345678914,
            Status = true,
            BirthDate = new DateTime(1982, 1, 1),
            Gender = 1,
            MobilePhones = "5551234572",
            Address = "Branch Admin Address",
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = "branch",
            BranchName = $"{name} Branch",
            BranchAddress = "Branch Address",
            BranchPhone = "5551234573",
            SchoolId = 1,
            Sessions = new List<Session>(),
            Students = new List<Student>()
        };
    }

    public static Admin GetAdmin(string name = "Test")
    {
        HashingHelper.CreatePasswordHash("123456", out var passwordSalt, out var passwordHash);
        return new Admin
        {
            Id = 1,
            FullName = $"{name} Admin",
            Email = $"{name.ToLower()}@admin.com",
            CitizenId = 12345678915,
            Status = true,
            BirthDate = new DateTime(1980, 1, 1),
            Gender = 1,
            MobilePhones = "5551234574",
            Address = "Admin Address",
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = "admin"
        };
    }

    public static List<T> GetList<T>(Func<string, T> getMethod, int count = 5) where T : User
    {
        var list = new List<T>();
        for (var i = 1; i <= count; i++)
        {
            var item = getMethod($"Test{i}");
            item.Id = i;
            list.Add(item);
        }
        return list;
    }
}