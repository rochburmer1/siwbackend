using AutoMapper;
using ApplicationCore.Models.QuizAggregate;
using WebApi.Dto;

namespace WebApi.Mapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<QuizItem, QuizItemDto>()
                .ForMember(
                    q => q.Options,
                    op => op.MapFrom(i => new List<string>(i.IncorrectAnswers) { i.CorrectAnswer })
                );

            CreateMap<Quiz, QuizDto>()
                .ForMember(
                    q => q.Items,
                    op => op.MapFrom(i => i.Items)
                );
            
        }
    }
}