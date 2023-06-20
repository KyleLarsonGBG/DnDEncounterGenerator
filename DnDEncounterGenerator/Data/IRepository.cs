using DnDEncounterGenerator.Data.Models.Entity;

namespace DnDEncounterGenerator.Data
{
    public interface IRepository<TEntity>
        where TEntity : IEntity
    {
        Task<bool> AddEntity(TEntity entity);
        Task<bool> AddEntities(IEnumerable<TEntity> entities);
        Task<bool> UpdateEntity(TEntity entity);
        Task<IEnumerable<TEntity>> GetEntities(IQueryParameters<TEntity> queryParams);
        Task<IEnumerable<TEntity>> ListEntities();
        Task<object> GetEntity(Guid entityId);
        Task<bool> DeleteEntity(Guid entityId);
    }
}
