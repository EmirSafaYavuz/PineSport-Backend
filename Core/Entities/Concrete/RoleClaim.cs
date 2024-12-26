namespace Core.Entities.Concrete;

public class RoleClaim : IEntity
{
    public int RoleId { get; set; }
    public int ClaimId { get; set; }
}