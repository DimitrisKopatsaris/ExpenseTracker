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
            // ---------- Accounts ----------
            CreateMap<Account, AccountDto>();

            CreateMap<CreateAccountDto, Account>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.CreatedAtUtc, opt => opt.Ignore())
                .ForMember(d => d.Expenses, opt => opt.Ignore());

            // ---------- Categories ----------
            CreateMap<Category, CategoryDto>();

            CreateMap<CreateCategoryDto, Category>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.CreatedAtUtc, opt => opt.Ignore())
                .ForMember(d => d.Expenses, opt => opt.Ignore());

            // ---------- Expenses ----------
            CreateMap<Expense, ExpenseDto>()
                .ForMember(d => d.AccountName, opt => opt.MapFrom(s => s.Account.Name))
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.Name))
                .ForMember(d => d.CategoryType, opt => opt.MapFrom(s => s.Category.Type));

            CreateMap<CreateExpenseDto, Expense>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.CreatedAtUtc, opt => opt.Ignore())
                .ForMember(d => d.Account, opt => opt.Ignore())
                .ForMember(d => d.Category, opt => opt.Ignore());

            // Optional but recommended if you have UpdateExpenseDto
            CreateMap<UpdateExpenseDto, Expense>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.CreatedAtUtc, opt => opt.Ignore())
                .ForMember(d => d.Account, opt => opt.Ignore())
                .ForMember(d => d.Category, opt => opt.Ignore());
        }
    }
}
