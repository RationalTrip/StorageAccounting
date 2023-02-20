using Microsoft.EntityFrameworkCore;
using StorageAccounting.Application.Repositories;
using StorageAccounting.Database.Extensions;
using StorageAccounting.Domain.Common;
using StorageAccounting.Domain.Exceptions.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace StorageAccounting.Database.Repositories
{
    public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        private readonly DbContext _context;

        public Repository(DbContext context) => _context = context;

        protected abstract Expression<Func<TEntity, TKey>> PrimaryKey { get; }

        public virtual async Task<Result<TEntity>> CreateAsync(TEntity entity, CancellationToken token)
        {
            _context.Set<TEntity>().Add(entity);

            await _context.SaveChangesAsync(token);

            return entity;
        }

        public virtual async Task<Result<IEnumerable<TEntity>>> GetAllAsync(int? start,
            int? size,
            CancellationToken token) =>
            await _context.Set<TEntity>()
                    .OptionalPagination(PrimaryKey, start, size)
                    .AsNoTracking()
                    .ToListAsync(token);
        public async Task<int> GetCountAsync(CancellationToken token) =>
            await _context.Set<TEntity>().CountAsync(token);
        public virtual async Task<Result<TEntity>> GetById(TKey id, CancellationToken token)
        {
            var entity = await _context.FindAsync<TEntity>(id, token);

            if (entity is null)
                return EntityNotFoundResult(id);

            return entity!;
        }

        public virtual async Task<bool> IsExistsAsync(TKey id, CancellationToken token) =>
            await _context.Set<TEntity>()
                    .AnyAsync(PrimaryKey.EqualTo(id), token);

        public virtual async Task<Result<TEntity>> UpdateAsync(TEntity entity, CancellationToken token)
        {
            _context.Update(entity);

            await _context.SaveChangesAsync(token);

            return entity;
        }

        public async Task<Result> RemoveAsync(TKey id, CancellationToken token)
        {
            var linesRemoved = await _context.Set<TEntity>()
                .Where(PrimaryKey.EqualTo(id))
                .ExecuteDeleteAsync(token);

            if (linesRemoved > 0)
                return Result.Success;

            return EntityNotFoundResult(id)
                .AsFaultResult();
        }

        protected static Result<TEntity> EntityNotFoundResult(TKey notExistedId) =>
            new Result<TEntity>(new EntityNotFoundException(notExistedId?.ToString()!, typeof(TEntity).Name));

        protected static Result<TEntity> UniqueValueAlreadyExistsResult(string value,
            string propertyName,
            string existedId) =>
            new Result<TEntity>(new UniqueValueAlreadyExistsException(value, propertyName, typeof(TEntity).Name, existedId));
    }
}
