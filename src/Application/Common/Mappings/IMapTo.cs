using AutoMapper;

namespace Application.Common.Mappings;

public interface IMapTo<T>
{
    void Mapping(Profile profile) => profile.CreateMap(GetType(), typeof(T));
}