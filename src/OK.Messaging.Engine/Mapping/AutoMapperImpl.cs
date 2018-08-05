using AutoMapper;
using System.Collections.Generic;
using IMessagingMapper = OK.Messaging.Core.Mapping.IMapper;

namespace OK.Messaging.Engine.Mapping
{
    public class AutoMapperImpl : IMessagingMapper
    {
        private readonly IMapper _mapper;

        public AutoMapperImpl(Profile mappingProfile)
        {
            MapperConfiguration config = new MapperConfiguration((cfg) =>
            {
                cfg.AddProfile(mappingProfile);
            });

            _mapper = new Mapper(config);
        }

        public TTo Map<TFrom, TTo>(TFrom from)
        {
            return _mapper.Map<TTo>(from);
        }

        public List<TTo> MapList<TFrom, TTo>(List<TFrom> from)
        {
            return _mapper.Map<List<TTo>>(from);
        }
    }
}