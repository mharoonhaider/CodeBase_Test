using Codebase_Test.Context;
using Codebase_Test.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using System;

namespace Codebase_Test.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly UserContext _context;

        public RegisterController(UserContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> RegisterUser(UserModel userInfo)
        {
            try
            {
                var alreadyExist = await GetUser(userInfo.EmailAddress);
                if (alreadyExist == null)
                {
                    await _context.UserInfo.AddAsync(userInfo);
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetString("phoneOTP_" + userInfo.Id, GenerateRandomNo().ToString());
                    HttpContext.Session.SetString("emailOTP_" + userInfo.Id, GenerateRandomNo().ToString());
                }
                else
                {
                    HttpContext.Session.SetString("phoneOTP_" + alreadyExist.Id, GenerateRandomNo().ToString());
                    HttpContext.Session.SetString("emailOTP_" + alreadyExist.Id, GenerateRandomNo().ToString());
                }
                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        public async Task<UserModel> GetUser(string email)
        {
            var user = await _context.UserInfo.FindAsync(email);
            return user;
        }

        [HttpPut]
        public async Task<ActionResult> VerifyPhone(string UserId, string otp)
        {
            try
            {
                if (!otp.Equals(null))
                {
                    if (otp == HttpContext.Session.GetString("phoneOTP_" + UserId))
                    {
                        var user = await _context.UserInfo.FindAsync(UserId);
                        if (user != null)
                        {
                            HttpContext.Session.Remove("phoneOTP_" + UserId);
                            user.PhoneVerified = true;
                            _context.Entry(user).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                            return Ok(user);
                        }
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                return NotFound();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        public async Task<ActionResult> VerifyEmail(string UserId, string otp)
        {
            try
            {
                if (!otp.Equals(null))
                {
                    if (otp == HttpContext.Session.GetString("emailOTP_" + UserId))
                    {
                        var user = await _context.UserInfo.FindAsync(UserId);
                        if (user != null)
                        {
                            HttpContext.Session.Remove("emailOTP_" + UserId);
                            user.EmailVerified = true;
                            _context.Entry(user).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                            return Ok(user);
                        }
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                return NotFound();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        [NonAction]
        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
        [HttpPost]
        public async Task<ActionResult> CreateAccountPin(string UserId, string pin)
        {
            try
            {
                if (!pin.Equals(null))
                {
                    var addedPIN = HttpContext?.Session?.GetString("accountPIN" + UserId);
                    if (addedPIN != null)
                    {
                        if (addedPIN == pin)
                        {
                            var user = await _context.UserInfo.FindAsync(UserId);
                            if (user != null)
                            {
                                HttpContext.Session.Remove("accountPIN" + UserId);
                                user.AccountPin = pin;
                                _context.Entry(user).State = EntityState.Modified;
                                await _context.SaveChangesAsync();
                                return Ok(user);
                            }
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    else
                    {
                        HttpContext.Session.SetString("accountPIN" + UserId, pin);
                        return Ok(UserId);
                    }
                }
                return NotFound();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
