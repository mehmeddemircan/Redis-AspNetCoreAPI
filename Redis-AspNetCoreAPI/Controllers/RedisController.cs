using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Redis_AspNetCoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisController(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        [HttpGet("set")]
        public IActionResult SetValue(string key, string value)
        {
            var db = _redis.GetDatabase();
            db.StringSet(key, value);
            return Ok($"Key '{key}' set to '{value}'");
        }

        [HttpGet("get")]
        public IActionResult GetValue(string key)
        {
            var db = _redis.GetDatabase();
            var value = db.StringGet(key);
            return Ok($"Key '{key}' has value '{value}'");
        }

        [HttpDelete("delete")]
        public IActionResult DeleteKey(string key)
        {
            var db = _redis.GetDatabase();
            bool deleted = db.KeyDelete(key);
            if (deleted)
            {
                return Ok($"Key '{key}' deleted successfully.");
            }
            else
            {
                return NotFound($"Key '{key}' not found.");
            }
        }

        [HttpPut("update")]
        public IActionResult UpdateValue(string key, string value)
        {
            var db = _redis.GetDatabase();
            if (db.KeyExists(key))
            {
                db.StringSet(key, value);
                return Ok($"Key '{key}' updated to '{value}'");
            }
            else
            {
                return NotFound($"Key '{key}' not found.");
            }
        }
    }
}
