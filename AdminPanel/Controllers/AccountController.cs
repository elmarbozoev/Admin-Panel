using AdminPanel.Models;
using AdminPanel.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Controllers
{
    public class AccountController : Controller
    {
        //мы получаем сервис по управлению пользователями - UserManager и сервис SignInManager,
        //который позволяет аутентифицировать пользователя и устанавливать или удалять его куки.
        private readonly UserManager<User> _userManager;// управление пользователем
        private readonly SignInManager<User> _signInManager; //управление логином (куки)


        //создаем констркутор AccountController
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //метод будет отображать форму для регистрации
        public IActionResult Register()
        {
            return View();
        }

        //метод Post
        //из формы(Views) будут приходить сюда данные, которые будт обрабатывать сервер
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model) {
            //проверяем, все ли поля формы заполнены коректно 
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email, Year = model.Year };
                // добавляем пользователя в БД
                var result = await _userManager.CreateAsync(user, model.Password);//В качестве параметра передается
                                                                                  //сам пользователь и его пароль.
                //если ползователь добавился в БД, то временно(до первого закрытия вкладки) сохраняем его данные в куки
                //и автоматически попадаем на старницу в качечтве зарегистрированного пользователя
                if (result.Succeeded)
                {
                    // установка куки
                    //await _signInManager.SignInAsync(user, false);
                    //return RedirectToAction("Index", "Home");
                    // Генерация токена 
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    // Url.Action - хэлпер, перенаправляет наши УРЛ-ки ConfirmEmail - action, Account - controller
                    var callback = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    EmailService emailService = new EmailService();
             //Admin123!
                        await emailService.SendEmailAsync(model.Email,
                            "Подтверждение вашего аккаунта",
                            $"Подтвердите вашу регистрацию, перейдя по <a href='{callback}'>Ссылке</a>");
                    
                    return Content("Для завершения регистрации проверьте вашу почту");

                }
                //если по какой-то причине не удалось произвести запись в БД, то выводим ошибку
                else
                {
                    foreach (var error in result.Errors)//result.Errors - реестр ошибок в Identity
                    {
                        //добавляет модель ошибки на страницу Identity для отобрадения ошибко
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        // Когда пользователь запрашивает URL-адрес с ограниченным доступом, он перенаправляется
        // по адресу /Account/Login со строкой запроса, содержащей адрес страницы с ограниченным доступом
        public IActionResult Login(string? returnUrl=null)
        {
            return View(new LoginViewModel { ReturnUrl=returnUrl});
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user != null)
                {
                    if(!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError("", "Вы не подтвердили регистрацию");
                        return View(model);
                    }
                }
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password,model.RememberMe, false); 
                if(result.Succeeded)
                {
                    //проверить принадлежит  ли url приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl) ) {
                        //тут
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильно введены логин или ппроль");
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
       
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            // Существует ли такой пользователь или токен
            if (userId == null || code == null)
            {
                return View("Error");
            }

            // поиск данного пользователя
            var user = await _userManager.FindByIdAsync(userId);

            // проверка на наличие такого пользователя
            if(user == null)
            {
                return View("Error");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> Index() => View(await _userManager.Users.ToListAsync());

        [HttpGet]
        public async Task<IActionResult> ChangePassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is not null)
            {
                var model = new ChangePasswordViewModel() { Id = user.Id, Email = user.Email };
                return View(model);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User is not found");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user is not null)
                {
                    IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User is not found");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is not null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User is not found");
            }
            return RedirectToAction("Index");
        }
    }
}
