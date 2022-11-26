using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FakeUser.Infrastructure.Dtos;
using FakeUser.Infrastructure.Interfaces;
using FakeUser.Service.Services;

namespace FakeUserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakeUserController : Controller
    {

        private readonly IFakeUserService _fakeUserService;
        public FakeUserController(IFakeUserService FakeUserService)
        {
            FakeUserService = _fakeUserService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await _fakeUserService.GetAll();
            return Ok(users);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var user = await _fakeUserService.GetById(id);
            return Ok(user);
        }
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _fakeUserService.Delete(id);
            return Ok();
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateAsync(FakeUserDto userDto, int id)
        {
            await _fakeUserService.Update(userDto, id);
            return Ok();
        }
    }
}
