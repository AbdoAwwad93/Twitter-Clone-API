using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwitterClone_API.DataAccess.Repo;
using TwitterClone_API.Models.AppModels;

namespace TwitterClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        private readonly IRepository<AppUser> repository;

        public TestController(IRepository<AppUser> repository)
        {
            this.repository=repository;
        }
        [HttpGet]
        
        public ActionResult display()
        {
            var list = repository.GetAll();
            return Ok(list);
        }
    }
}
