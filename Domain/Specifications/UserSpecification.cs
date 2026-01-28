using ReservationService.Domain.Entities;
using System.Linq.Expressions;

namespace ReservationService.Domain.Specifications;

public class UserSpecification : Specification<User>
{
    private readonly string _email;

    public UserSpecification(string email)
    {
        _email = email ?? throw new ArgumentNullException(nameof(email));
    }

    public override Expression<Func<User, bool>> Criteria => u => u.Email == _email;
}