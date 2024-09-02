using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FirstWebService.Users;
using FirstWebServiceData.Models.Entity;

namespace TheFirstWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }
        [HttpPost]
        public IActionResult AddUser(User user)
        {
            if (user == null)
            {
                return BadRequest("User object is null.");
            }

            _userService.AddUser(user);
            return CreatedAtAction(nameof(GetById), new { Id = user.id }, user);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User user)
        {
            if (user == null)
            {
                return BadRequest("User object is null.");
            }

            // Veritabanında kullanıcıyı kontrol et
            var existingUser = _userService.GetUserById(id); // Kullanıcıyı veritabanından al
            if (existingUser == null)
            {
                return NotFound(); // Kullanıcı bulunamazsa 404 döndür
            }

            // Gelen verileri mevcut kullanıcı ile güncelle
            existingUser.Name = user.Name;
            existingUser.LastName = user.LastName;
            existingUser.PhoneNumber = user.PhoneNumber;

            _userService.UpdateUser(existingUser); // Güncellenmiş kullanıcıyı kaydet
            return NoContent();

        }
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            _userService.DeleteUser(id);
            return NoContent();
        }
    }
}
