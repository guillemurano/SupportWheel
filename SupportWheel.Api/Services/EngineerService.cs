using System.Collections.Generic;
using System.Linq;
using SupportWheel.Api.Models;
using SupportWheel.Api.Repositories;

namespace SupportWheel.Api.Services
{
    public class EngineerService : IEngineerService
    {
        private IRepository<Engineer> _repository;
        
        public EngineerService(IRepository<Engineer> repository)
        {
            _repository = repository;
        }
        
        public void Create(Engineer entity)
        {
            _repository.Insert(entity);
            _repository.SaveChanges();
        }

        public void Update(Engineer entity)
        {
            _repository.Update(entity);
            _repository.SaveChanges();
        }

        public void Delete(long id)
        {
            var e =_repository.GetById(id);
            if (e != null) 
            {
                _repository.Delete(e);
                _repository.SaveChanges();
            }
        }

        public IEnumerable<Engineer> GetAll()
        {
            return _repository.Get().ToList();
        }

        public Engineer GetById(long id)
        {
            return _repository.GetById(id);
        }
    }
}