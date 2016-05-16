using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using task2.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace task2.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AccountsController : Controller
    {
        public static List<Account> Accounts = new List<Account>()
        {
            new Account() { Id = 1, Name = "Alice", Description = "Some girl" },
            new Account() { Id = 2, Name = "Bob", Description = "Some boy" }
        };

       
        [HttpGet]
        public IEnumerable<Account> GetAll()
        {
            return Accounts;
        }

        
        [HttpGet("{id}", Name="Get")]
        public IActionResult GetById(int id)
        {
            var account = Accounts.FirstOrDefault(x => x.Id == id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(account);
        }

        
        [HttpPost("create")]
        public IActionResult Create([FromBody]Account account)
        {
            var id = Accounts.Max(x => x.Id);
            account.Id = id + 1;
            Accounts.Add(account);
            return CreatedAtRoute("Get", new { controller = "Accounts", id = account.Id }, account);
        }

        
        [HttpPut("update/{id}", Name="update")]
        public IActionResult Update(int id, [FromBody] Account value)
        {
            if (string.IsNullOrEmpty(value.Name))
            {
                return new BadRequestResult();
            }

            var account = Accounts.FirstOrDefault(x => x.Id == id);
            if (account == null)
            {
                return HttpNotFound();
            }
            account.Description = value.Description;
            account.Name = value.Name;
            return new NoContentResult();
        }

        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var account = Accounts.FirstOrDefault(x => x.Id == id);
            if (account != null)
            {
                Accounts.Remove(account);
                return Ok();
            }
            return HttpNotFound("No such user");
        }
    }
}
