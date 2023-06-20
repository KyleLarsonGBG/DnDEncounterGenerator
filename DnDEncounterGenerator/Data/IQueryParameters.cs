using System.Linq.Expressions;
using DnDEncounterGenerator.Data.Models.Entity;

namespace DnDEncounterGenerator.Data;

public interface IQueryParameters<TEntity>
    where TEntity : IEntity
{
    void WithFilter(Expression<Func<TEntity, bool>> filter);
    Expression<Func<TEntity, bool>> BuildExpression();
}