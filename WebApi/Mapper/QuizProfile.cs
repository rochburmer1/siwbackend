using ApplicationCore.Models.QuizAggregate;
using AutoMapper;
using WebAPI.Controllers;
using WebApi.Dto;

public class QuizProfile : Profile
{
    public QuizProfile()
    {
        CreateMap<NewQuizDto, Quiz>();
    }
}