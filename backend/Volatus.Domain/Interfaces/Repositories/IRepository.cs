using System.Linq.Expressions;
using Volatus.Domain.View;
using Volatus.Domain.Interfaces.Entities;

namespace Volatus.Domain.Interfaces.Repositories;

public interface IRepository<Entity> where Entity : IEntity
{
    bool Exists(Guid? id);
    List<Entity> Get(Expression<Func<Entity, bool>> linq = null);
    Entity GetById(Guid? id);
    IQueryable<Entity> Query();
    List<Entity> ExecuteQuery(IQueryable<Entity> query, PaginationParams @params, Expression<Func<Entity, IComparable>> orderBy = null);
    List<EntityViewModel> ExecuteQuery<EntityViewModel>(IQueryable<Entity> query, PaginationParams @params, Expression<Func<Entity, EntityViewModel>> converter, Expression<Func<Entity, IComparable>> orderBy = null);

    Entity Insert(Entity entity);
    List<Entity> InsertMany(List<Entity> entities);

    void Reload(Entity entityToUpdate);
    void Update(Entity entity);
    void UpdateMany(List<Entity> entitiesToUpdate);

    void Delete(Guid id);
    void Delete(Entity entityToDelete);
    void DeleteMany(List<Entity> entities);
}