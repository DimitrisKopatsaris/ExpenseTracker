using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace ExpenseTracker.Api.Controllers; // asp.net core automatically looks for controllers inside .api.controllers namespaces when i call app.MapControllers() in program.cs

[ApiController] //it tells that this class handles HTTP API requests, and enables automatic transformation from JSON request bodies into C# objects
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Account>>> GetAll()
    {
        var accounts = await _accountService.GetAllAsync();
        return Ok(accounts);
    }
        
        
    public record CreateAccountRequest(string Name, decimal StartingBalance); //this defines what the client must send in the request JSON body. 

    [HttpPost]
    public async Task<ActionResult<Account>> Create(CreateAccountRequest req) //takes the JSON body that the client  enterd and created a C# object named req
    {
        var account = await _accountService.CreateAsync(req.Name,req.StartingBalance);
        return CreatedAtAction(nameof(GetAll), new { id = account.Id }, account); //returns http 201 created, includes new resource info like the Id.
    }
}
