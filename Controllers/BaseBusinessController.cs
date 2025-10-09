using HMS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseBusinessController<T1, T2, T3> : ControllerBase
        where T1 : class
        where T2 : class
        where T3 : IBaseBusinessService<T1, T2>
    {
        public T3 ser = default!;
        public string UserId { get { return GetUserId(); } }
        private string GetUserId()
        {
            string? m = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(m)) return m;
            throw new Exception("Error In Read UserId From Token");
        }

        [HttpPost]
        public virtual async Task<IActionResult> Insert(T2 model)
        {
            var obj = await ser.Insert(model, UserId);
            if (obj == null) return BadRequest();
            return Ok(obj);
        }

        [HttpPut]
        public virtual async Task<IActionResult> Edit(T2 model)
        {
            var obj = await ser.Edit(model, UserId);
            if (obj == null) return BadRequest();
            return Ok(obj);
        }
        [HttpDelete]
        public virtual async Task<IActionResult> Remove(int id)
        {
            var obj = await ser.Remove(id, UserId);
            if (obj == false) return BadRequest();
            return Ok(obj);
        }
    }
}
