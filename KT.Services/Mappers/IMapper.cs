using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using KT.DTOs;

namespace KT.Services.Mappers
{
	public interface IMapper<TDto, TEntity> where TDto: BaseDto where TEntity: EntityObject
	{
		TDto Map(TEntity entity);

		TEntity Map(TDto dto);

		IEnumerable<TDto> Map(IEnumerable<TEntity> collection);

		EntityCollection<TEntity> Map(IEnumerable<TDto> dtoList);
	}
}
