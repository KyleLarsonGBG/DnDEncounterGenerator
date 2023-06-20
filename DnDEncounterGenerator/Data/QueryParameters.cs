using System.Linq.Expressions;
using DnDEncounterGenerator.Data.Models.Entity;
using static System.Linq.Expressions.Expression;

namespace DnDEncounterGenerator.Data;

public class QueryParameters<TEntity> : IQueryParameters<TEntity>
    where TEntity : IEntity
{
    private readonly List<Expression<Func<TEntity, bool>>> _expressions = new List<Expression<Func<TEntity, bool>>>();

    public void WithFilter(Expression<Func<TEntity, bool>> filter)
    {
        _expressions.Add(filter);
    }

    public Expression<Func<TEntity, bool>> BuildExpression()
    {
        if (_expressions.Count <= 0)
            throw new ArgumentNullException("No filters present");

        var expression = _expressions[0];

        if(_expressions.Count > 1)
            AndAlso(expression, AddExpression(_expressions[1]));

        return expression;
    }

    private Expression AddExpression(Expression<Func<TEntity, bool>> otherExpression)
    {
        var index = _expressions.IndexOf(otherExpression);

        if (index == _expressions.Count) 
            return AndAlso(otherExpression, _expressions[index]);

        return AndAlso(otherExpression, AddExpression(_expressions[index + 1]));

    }
}