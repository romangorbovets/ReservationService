using System.Linq.Expressions;

namespace ReservationService.Domain.Specifications;

public class OrSpecification<T> : Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    public OrSpecification(Specification<T> left, Specification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override Expression<Func<T, bool>> Criteria
    {
        get
        {
            var leftExpression = _left.Criteria;
            var rightExpression = _right.Criteria;

            var parameter = Expression.Parameter(typeof(T), "x");
            var leftBody = ReplaceParameter(leftExpression.Body, leftExpression.Parameters[0], parameter);
            var rightBody = ReplaceParameter(rightExpression.Body, rightExpression.Parameters[0], parameter);
            var combinedBody = Expression.OrElse(leftBody, rightBody);

            return Expression.Lambda<Func<T, bool>>(combinedBody, parameter);
        }
    }

    private static Expression ReplaceParameter(Expression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        return new ParameterReplacer(oldParameter, newParameter).Visit(expression);
    }

    private class ParameterReplacer : ExpressionVisitor
    {
        private readonly ParameterExpression _oldParameter;
        private readonly ParameterExpression _newParameter;

        public ParameterReplacer(ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            _oldParameter = oldParameter;
            _newParameter = newParameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _oldParameter ? _newParameter : base.VisitParameter(node);
        }
    }
}