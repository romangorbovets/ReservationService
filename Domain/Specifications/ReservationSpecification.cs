using ReservationService.Domain.AggregateRoots;
using System.Linq.Expressions;

namespace ReservationService.Domain.Specifications;

public class ReservationSpecification : Specification<Reservation>
{
    private readonly Guid _id;

    public ReservationSpecification(Guid id)
    {
        _id = id;
    }

<<<<<<< HEAD
    public override Expression<Func<Reservation, bool>> Criteria
    {
        get
        {
           
            var id = _id;
            return r => r.Id == id;
        }
    }
=======
    public override Expression<Func<Reservation, bool>> Criteria => r => r.Id == _id;
>>>>>>> main
}