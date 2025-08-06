using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Volatus.Domain.View;
using Volatus.Domain.Interfaces.Entities;
using Volatus.Domain.Interfaces.Repositories;

namespace Volatus.Data.Repositories;

public class Repository<Entity> : IRepository<Entity>
    where Entity : class, IEntity
{
    protected readonly DbContext _context;
    private readonly DbSet<Entity> _table;

    public Repository(DbContext context)
    {
        _context = context;
        _table = _context.Set<Entity>();
    }

    public virtual Entity Insert(Entity entity)
    {
        _table.Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public virtual List<Entity> InsertMany(List<Entity> entities)
    {
        if (entities == null || !entities.Any())
            return entities;

        _table.AddRange(entities);
        _context.SaveChanges();
        return entities;
    }

    public virtual void Delete(Guid id)
    {
        Entity entityToDelete = _table.Find(id);
        Delete(entityToDelete);
    }

    public virtual void DeleteMany(List<Entity> entities)
    {
        if (entities == null || !entities.Any())
            return;

        _table.RemoveRange(entities);
        _context.SaveChanges();
    }

    public virtual void Delete(Entity entityToDelete)
    {
        if (_context.Entry(entityToDelete).State == EntityState.Detached)
            _table.Attach(entityToDelete);
        
        _table.Remove(entityToDelete);
        _context.SaveChanges();
    }

    public virtual void Update(Entity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public virtual void UpdateMany(List<Entity> entitiesToUpdate)
    {
        if (entitiesToUpdate == null || !entitiesToUpdate.Any())
            return;
        
        _context.UpdateRange(entitiesToUpdate);
        _context.SaveChanges();
    }

    public virtual void Reload(Entity entityToUpdate)
    {
        _context.Entry(entityToUpdate).Reload();
    }

    public IQueryable<Entity> Query()
    {
        return _table.AsNoTracking().AsQueryable();
    }
    
    private static IQueryable<Entity> PaginateQuery(IQueryable<Entity> query, PaginationParams @params, Expression<Func<Entity, IComparable>> orderBy = null)
    {
        if (orderBy != null)
        {
            if (@params.SortDirection == "desc")
                query = query.OrderByDescending(orderBy);
            else
                query = query.OrderBy(orderBy);
        }

        var entities = query.Skip((@params.PageNumber - 1) * @params.PageSize).Take(@params.PageSize);
        @params.Count = entities.Count();

        if ((@params.Count != @params.PageSize) || query.Count() <= (@params.PageSize * @params.PageNumber))
            @params.isLastPage = true;

        return entities;
    }

    public virtual List<Entity> ExecuteQuery(IQueryable<Entity> query, PaginationParams @params, Expression<Func<Entity, IComparable>> orderBy = null)
    {
        return PaginateQuery(query, @params, orderBy)
                .ToList();
    }

    public virtual List<EntityViewModel> ExecuteQuery<EntityViewModel>(IQueryable<Entity> query, PaginationParams @params, Expression<Func<Entity, EntityViewModel>> converter, Expression<Func<Entity, IComparable>> orderBy = null)
    {
        return PaginateQuery(query, @params, orderBy)
                .Select(converter)
                .ToList();
    }

    public virtual List<Entity> Get(Expression<Func<Entity, bool>> linq = null)
    {
        return (linq != null) ? _table.AsNoTracking().Where(linq).ToList() : _table.AsNoTracking().ToList();
    }

    public virtual Entity GetById(Guid? id)
    {
        return (id.HasValue) ? _table.Find(id) : null;
    }

    public bool Exists(Guid? id)
    {
        return (id.HasValue) && (_table.Find(id) != null);
    }
}