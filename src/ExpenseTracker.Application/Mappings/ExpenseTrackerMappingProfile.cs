using AutoMapper;
using ExpenseTracker.Application.DTOs.Accounts;
using ExpenseTracker.Application.DTOs.Categories;
using ExpenseTracker.Application.DTOs.Expenses;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Mappings
{
    public class ExpenseTrackerMappingProfile : Profile
    {
        public ExpenseTrackerMappingProfile()
        {
            // Accounts
            CreateMap<Account, AccountDto>();
            CreateMap<CreateAccountDto, Account>();

            // Categories
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryDto, Category>();

            // Expenses
            CreateMap<Expense, ExpenseDto>()
                .ForMember(dest => dest.AccountName,
                    opt => opt.MapFrom(src => src.Account.Name))
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.CategoryType,
                    opt => opt.MapFrom(src => src.Category.Type));

            CreateMap<CreateExpenseDto, Expense>();
        }
    }
}
