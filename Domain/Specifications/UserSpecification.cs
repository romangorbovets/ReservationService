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

    public override Expression<Func<User, bool>> Criteria
    {
        get
        {
            
            var email = _email;
            return u => u.Email == email;
        }
    }
}