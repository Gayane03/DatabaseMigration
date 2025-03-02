using AutoMapper;
using BusinessLayer.Helper;
using SharedLibrary.DbModels.Request;
using SharedLibrary.RequestModels;

namespace BusinessLayer.Mapping
{
	public class MappingProfile: Profile
	{
        public MappingProfile()
        {
			CreateMap<RegistrationRequest, UserEmailRequest>()
		   .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

			CreateMap<RegistrationRequest, User>()
			.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
			.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
			.ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => (int)src.Country))
			//.ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
			.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
			.ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => PasswordHelper.HashAndStore(src.Password)));

			CreateMap<LoginRequest, LoginRequestDB>()
			.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
			.ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
		}
    }
}
