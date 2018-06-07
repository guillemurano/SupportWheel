using System.Collections.Generic;

namespace SupportWheel.Api.Services
{
    public interface ICrudService<TEntity> where TEntity : class
    {
        void Create(TEntity entity);

        TEntity GetById(long id);

        IEnumerable<TEntity> GetAll();
        
        void Update(TEntity entity);
        
        void Delete(long id);
    }
}