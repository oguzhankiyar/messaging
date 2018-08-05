using OK.Messaging.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OK.Messaging.Core.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        IEnumerable<TEntity> FindAll();

        IEnumerable<TEntity> FindMany(Expression<Func<TEntity, bool>> predicate);

        TEntity FindOne(Expression<Func<TEntity, bool>> predicate);

        TEntity Insert(TEntity entity);

        bool Update(TEntity entity);

        bool Remove(int id);
    }
}