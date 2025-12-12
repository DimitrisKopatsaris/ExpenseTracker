using AutoMapper;
using ExpenseTracker.Application.DTOs.Accounts;
using ExpenseTracker.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;

    public AccountsController(IAccountService accountService, IMapper mapper)
    {
        _accountService = accountService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAll()
    {
        var accounts = await _accountService.GetAllAsync();
        var dtos = _mapper.Map<List<AccountDto>>(accounts);
        return Ok(dtos);
    }

    [HttpPost]
    public async Task<ActionResult<AccountDto>> Create(CreateAccountDto dto)
    {
        var account = await _accountService.CreateAsync(dto.Name, dto.StartingBalance);
        var result = _mapper.Map<AccountDto>(account);

        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }
}
