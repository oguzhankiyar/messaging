using Microsoft.EntityFrameworkCore;
using OK.Messaging.Common.Entities;
using OK.Messaging.Core.Repositories;
using OK.Messaging.DataAccess.EntityFramework.DataContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OK.Messaging.DataAccess.EntityFramework.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly MessagingDataContext _dataContext;

        private DbSet<TEntity> Entities
        {
            get
            {
                return _dataContext.Set<TEntity>();
            }
        }

        public BaseRepository(MessagingDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<TEntity> FindAll()
        {
            return Entities.Where(x => x.IsDeleted == false);
        }

        public IEnumerable<TEntity> FindMany(Expression<Func<TEntity, bool>> predicate)
        {
            return Entities.Where(x => x.IsDeleted == false).Where(predicate);
        }

        public TEntity FindOne(Expression<Func<TEntity, bool>> predicate)
        {
            return Entities.Where(x => x.IsDeleted == false).FirstOrDefault(predicate);
        }

        public TEntity Insert(TEntity entity)
        {
            entity.IsDeleted = false;
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;

            _dataContext.Entry(entity).State = EntityState.Added;
            _dataContext.SaveChanges();

            return entity;
        }

        public bool Update(TEntity entity)
        {
            entity.UpdatedDate = DateTime.Now;

            _dataContext.Entry(entity).State = EntityState.Modified;

            return _dataContext.SaveChanges() > 0;
        }

        public bool Remove(int id)
        {
            TEntity entity = FindOne(x => x.Id == id);

            entity.IsDeleted = true;
            entity.UpdatedDate = DateTime.Now;

            _dataContext.Entry(entity).State = EntityState.Modified;

            return _dataContext.SaveChanges() > 0;
        }
    }
}