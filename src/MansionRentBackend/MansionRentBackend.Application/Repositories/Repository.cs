﻿using MansionRentBackend.Domain.Entities;
using MansionRentBackend.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace MansionRentBackend.Application.Repositories;

public abstract class Repository<TEntity, TKey>
    : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    protected DbContext _dbContext;
    protected DbSet<TEntity> _dbSet;
    protected int CommandTimeout { get; set; }

    public Repository(DbContext context)
    {
        CommandTimeout = 300;
        _dbContext = context;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public virtual async Task Add(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual async Task Remove(TKey id)
    {
        var entityToDelete = await _dbSet.FindAsync(id);
        await Remove(entityToDelete);
    }

    public virtual async Task Remove(TEntity entityToDelete)
    {
        if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            _dbSet.Attach(entityToDelete);

        _dbSet.Remove(entityToDelete);
    }

    public virtual async Task Remove(Expression<Func<TEntity, bool>> filter)
    {
        _dbSet.RemoveRange(_dbSet.Where(filter));
    }

    public virtual async Task Edit(TEntity entityToUpdate)
    {
        if (_dbContext.Entry(entityToUpdate).State == EntityState.Detached)
            _dbSet.Attach(entityToUpdate);

        _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
    }

    public virtual async Task<int> GetCount(Expression<Func<TEntity, bool>> filter = null)
    {
        IQueryable<TEntity> query = _dbSet;
        var count = 0;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        count = await query.CountAsync();
        return count;
    }

    public virtual async Task<IList<TEntity>> Get(Expression<Func<TEntity, bool>> filter, string includeProperties = "")
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        return query.ToList();
    }

    public virtual async Task<IList<TEntity>> GetAll()
    {
        return _dbSet.ToList();
    }

    public virtual async Task<TEntity> GetById(TKey id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<(IList<TEntity> data, int total, int totalDisplay)> Get
        (Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "", int pageIndex = 1, int pageSize = 10, bool isTrackingOff = false)
    {
        IQueryable<TEntity> query = _dbSet;
        var total = await query.CountAsync();
        var totalDisplay = total;

        if (filter != null)
        {
            query = query.Where(filter);
            totalDisplay = await query.CountAsync();
        }

        foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            var result = orderBy(query).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            if (isTrackingOff)
                return (result.AsNoTracking().ToList(), total, totalDisplay);
            else
                return (result.ToList(), total, totalDisplay);
        }
        else
        {
            var result = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            if (isTrackingOff)
                return (result.AsNoTracking().ToList(), total, totalDisplay);
            else
                return (result.ToList(), total, totalDisplay);
        }
    }

    public async Task<(IList<TEntity> data, int total, int totalDisplay)> GetDynamic(Expression<Func<TEntity, bool>> filter = null,
        string orderBy = null, string includeProperties = "", int pageIndex = 1, int pageSize = 10, bool isTrackingOff = false)
    {
        IQueryable<TEntity> query = _dbSet;
        var total = await query.CountAsync();
        var totalDisplay = await query.CountAsync();

        if (filter != null)
        {
            query = query.Where(filter);
            totalDisplay = await query.CountAsync();
        }

        foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            var result = query.OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            if (isTrackingOff)
                return (result.AsNoTracking().ToList(), total, totalDisplay);
            else
                return (result.ToList(), total, totalDisplay);
        }
        else
        {
            var result = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            if (isTrackingOff)
                return (result.AsNoTracking().ToList(), total, totalDisplay);
            else
                return (result.ToList(), total, totalDisplay);
        }
    }
}