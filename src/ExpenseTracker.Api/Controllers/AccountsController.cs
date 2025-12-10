using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Controllers; // asp.net core automatically looks for controllers inside .api.controllers namespaces when i call app.MapControllers() in program.cs

[ApiController] //it tells that this class handles HTTP API requests, and enables automatic transformation from JSON request bodies into C# objects
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly ExpenseTrackerDbContext _db;
    public AccountsController(ExpenseTrackerDbContext db) => _db = db; //everytime i call an endpoint, asp.net gives my controller a connection to the sql database

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Account>>> GetAll()
        => Ok(await _db.Accounts.AsNoTracking().ToListAsync()); //it is simple because account does not have any relationships, it simply takes the account data that the user gave, unlikely the expense controller, where the expense includes account and category and i have to include and select manually in order to control what the JSON will look like

    public record CreateAccountRequest(string Name, decimal StartingBalance); //this defines what the client must send in the request JSON body. 

    [HttpPost]
    public async Task<ActionResult<Account>> Create(CreateAccountRequest req) //takes the JSON body that the client  enterd and created a C# object named req
    {
        var account = new Account { Name = req.Name, StartingBalance = req.StartingBalance }; //create a new instance in memory 
        _db.Accounts.Add(account); //to be inserted 
        await _db.SaveChangesAsync(); //EF looks at all tracked entinties and runs the sql insert command to add it into the database, and then sql server creates a new row in the account table, generates a new Id and returns that Id to EF Core.
        return CreatedAtAction(nameof(GetAll), new { id = account.Id }, account); //returns http 201 created, includes new resource info like the Id.
    }
}
