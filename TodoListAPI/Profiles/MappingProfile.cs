using AutoMapper;
using TodoListAPI.Models;
using TodoListAPI.Models.DTO;

namespace TodoListAPI.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TodoItem, TodoItemDTO>();
            CreateMap<CreateTodoItemDTO, TodoItem>();
            CreateMap<UpdateTodoItemDTO, TodoItem>();

            CreateMap<Priority, PriorityDTO>();
            CreateMap<CreatePriorityDTO, Priority>();
            CreateMap<UpdatePriorityDTO, Priority>();
        }
    }
}