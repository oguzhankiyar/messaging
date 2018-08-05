using System.Collections.Generic;

namespace OK.Messaging.Core.Mapping
{
    public interface IMapper
    {
        TTo Map<TFrom, TTo>(TFrom from);

        List<TTo> MapList<TFrom, TTo>(List<TFrom> from);
    }
}