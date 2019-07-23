
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClimberCookie.DAL;

namespace ClimberCookie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        [Route("login/{name}/{word}")]
        [HttpGet]
        public string login(string name, string word)
        {
            Access.Login(name, word);
            return "success";

        }

        [Route("signup/{name}/{word}")]
        [HttpGet]
        public string signup(string name, string word)
        {
            Access.CreateNew(name, word);
            return "success";
        }
    }
}