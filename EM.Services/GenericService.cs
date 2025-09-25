using AutoMapper;
using EM.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EM.Services
{
    
    public class GenericService<TDto, TEntity> : IGenericService<TDto, TEntity>
    where TDto : class
    where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly IMapper _mapper;

        public GenericService(IGenericRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }

        public async Task<TDto?> GetByIdAsync(object id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<TDto>(entity);
        }

        public async Task<TDto?> GetByUserIdAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = await _repository.GetByUserIdAsync(predicate);
            return _mapper.Map<TDto>(entity);
        }
        public async Task<IEnumerable<TDto>> GetByUserIdAsync(Expression<Func<TEntity, bool>> predicate, bool returnIenumerable)
        {
            var entities = await _repository.GetByUserIdAsync(predicate, returnIenumerable);
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }

        public async Task AddAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.AddAsync(entity);
            await _repository.SaveAsync();
        }

        public async Task UpdateAsync(object id, TDto dto)
        {
            var existingEntity = await _repository.GetByIdAsync(id);
            if (existingEntity == null)
                throw new Exception("Entity not found");

            _mapper.Map(dto, existingEntity); // maps changes onto existing entity
            _repository.Update(existingEntity);
            await _repository.SaveAsync();
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new Exception("Entity not found");

            _repository.Delete(entity);
            await _repository.SaveAsync();
        }
    }
    }
