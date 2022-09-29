using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SPANTask.Entities;
using SPANTask.Helpers;

namespace SPANTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]    
    public class PeopleController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DataHelper _dataHelper;

        public PeopleController(IConfiguration config)
        {
            _config = config;
            _dataHelper = new DataHelper(_config.GetConnectionString("SPANTask"), _config);
        }

        [HttpGet("load-data")]
        public IEnumerable<Person> LoadData()
        {
            return _dataHelper.LoadData();
        }

        [HttpPost("save-data")]
        public int SaveData(List<Person> people)
        {
            return _dataHelper.SaveData(people);
        }
    }
}
