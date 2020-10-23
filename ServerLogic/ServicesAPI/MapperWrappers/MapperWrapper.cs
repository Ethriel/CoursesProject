using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesAPI.MapperWrappers
{
    public class MapperWrapper<TEntity, TDTO> : IMapperWrapper<TEntity, TDTO>
        where TEntity : class
        where TDTO : class
    {
        private readonly IMapper mapper;

        public MapperWrapper(IMapper mapper)
        {
            this.mapper = mapper;
        }
        public IEnumerable<TEntity> MapEntities(IEnumerable<TDTO> dtos)
        {
            return mapper.Map<IEnumerable<TDTO>, IEnumerable<TEntity>>(dtos);
        }

        public IEnumerable<TDTO> MapModels(IEnumerable<TEntity> entities)
        {
            return mapper.Map<IEnumerable<TEntity>, IEnumerable<TDTO>>(entities);
        }

        public TEntity MapEntity(TDTO dto)
        {
            return mapper.Map<TDTO, TEntity>(dto);
        }

        public TDTO MapModel(TEntity entity)
        {
            return mapper.Map<TEntity, TDTO>(entity);
        }
    }
}
