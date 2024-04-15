

using CoreApi.DTOs.User;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        // User Token
        CreateMap<UserInfo, UserTokenReadDto>().ReverseMap();
    }
}