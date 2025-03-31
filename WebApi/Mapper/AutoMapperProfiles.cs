using ApplicationCore.Models;
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
            CreateMap<QuizItemUserAnswer, FeedbackAnswerDto>()
                .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.QuizItem.Question))
                .ForMember(dest => dest.Answer, opt => opt.MapFrom(src => src.Answer))
                .ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect()));

            CreateMap<(int QuizId, int UserId, List<QuizItemUserAnswer> Feedback, int TotalQuestions), FeedbackDto>()
                .ForMember(dest => dest.QuizId, opt => opt.MapFrom(src => src.QuizId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.TotalQuestions, opt => opt.MapFrom(src => src.TotalQuestions))
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Feedback));
        }
    }
}