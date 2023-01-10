using LoginProject.Data;
using LoginProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace LoginProject.Controllers
{
    public class UserController : Controller
    { 
        private readonly AppDbContext _db;
        public UserController(AppDbContext db)
        {
            _db = db; 
        }
   
        public IActionResult Index()
        {
            IEnumerable<User> objUsers = _db.Users.ToList();
            return View(objUsers);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                _db.Add(user);
                _db.SaveChanges();
                TempData["success"] = "User created successfully";
                return RedirectToAction("Index", "User");
            }
            return View();
        } public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            var un= user.Username;
            var ps= user.Password;
            var validUser = _db.Users.Where(s => s.Username.Equals(un) && s.Password.Equals(ps)).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if(validUser == null)
                {
                    TempData["notuser"] = "Invalid User";
                    return View();
                }
                else
                {
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Username),
                        new Claim("OtherProperties", "Example Role")

                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh= true,
                        IsPersistent= user.KeepLoggedIn,
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
                    TempData["loggedin"] = "User logged in successfully";
                    return RedirectToAction("Index", "Home");
                }
                
            }
            ViewData["ValidateMessage"] = "user not found";
            return View();
        } public IActionResult Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var foundUser = _db.Users.Find(id);
            if(foundUser== null)
            {
                return NotFound();
            }
            return View(foundUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(User user)
        {
            if (ModelState.IsValid)
            {
                _db.Users.Remove(user);
                _db.SaveChanges();
                TempData["deleted"] = "User deleted";
                return RedirectToAction("Index", "User");
            }

            return View();
        }public IActionResult Update(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var foundUser = _db.Users.Find(id);
            if(foundUser== null)
            {
                return NotFound();
            }
            return View(foundUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(User user)
        {
            if (ModelState.IsValid)
            {
                _db.Users.Remove(user);
                _db.SaveChanges();
                TempData["deleted"] = "User deleted";
                return RedirectToAction("Index", "User");
            }

            return View();
        }

    }
}
