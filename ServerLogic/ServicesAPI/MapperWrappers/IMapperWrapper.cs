﻿using System.Collections.Generic;

namespace ServicesAPI.MapperWrappers
{
    /// <summary>
    /// An interface wrapper for IMapper from Automaper.
    /// <typeparamref name="TEntity"/> - a type of entity.
    /// <typeparamref name="TDTO"/> - a type of DTO.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDTO"></typeparam>
    public interface IMapperWrapper<TEntity, TDTO>
        where TEntity : class
        where TDTO : class
    {
        /// <summary>
        /// Map an entity from <paramref name="dto"/>
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public TEntity MapEntity(TDTO dto);
        /// <summary>
        /// Map a dto from <paramref name="entity"/>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TDTO MapModel(TEntity entity);
        /// <summary>
        /// Map an IEnumerable(<typeparamref name="TEntity"/>) from <paramref name="dtos"/>
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> MapEntities(IEnumerable<TDTO> dtos);
        /// <summary>
        /// Map an IEnumerable(<typeparamref name="TDTO"/>) from <paramref name="entities"/>
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public IEnumerable<TDTO> MapModels(IEnumerable<TEntity> entities);
    }
}
