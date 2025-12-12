using AutoMapper;
using ExpenseTracker.Application.DTOs.Accounts;
using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Domain.Entities;
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

    [HttpGet("{id:int}")] 
    public async Task<ActionResult<AccountDto>> GetById(int id)
    {
        var account = await _accountService.GetByIdAsync(id);
        if (account is null) return NotFound();
        
        return Ok(_mapper.Map<AccountDto>(account));
    }


    [HttpPost]
    public async Task<ActionResult<AccountDto>> Create(CreateAccountDto dto)
    {
        try 
        {
            var account = await _accountService.CreateAsync(dto.Name, dto.StartingBalance);
            var result = _mapper.Map<AccountDto>(account); 
            return CreatedAtAction(nameof(GetById),new {id = result.Id,result});
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new {message = ex.Message});
        }
        catch (ArgumentException ex)
        {
            return ValidationProblem(ex.Message);
        }
        
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<AccountDto>> Update(int id, UpdateAccountDto dto)
    {
        try
        {
            var updated = await _accountService.UpdateAsync(id, dto.Name, dto.StartingBalance);
            if ( updated is null) return NotFound();

            return Ok(_mapper.Map<AccountDto>(updated));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new {message = ex.Message});
        }
        catch (ArgumentException ex)
        {
            return ValidationProblem(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _accountService.DeleteAsync(id);
            if(!deleted) return NotFound();

            return NoContent();
        }
        catch(InvalidOperationException ex) 
        {
            return Conflict(new {message = ex.Message});
        }
    }
}
