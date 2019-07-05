using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessProcess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAppPolling.Controllers
{
    public class OperacionController : Controller
    {
        private readonly IBusinessService _businessService;
        private readonly IMemoryCache _memoryCache;

        public OperacionController(IBusinessService businessService, IMemoryCache memoryCache)
        {
            _businessService = businessService;
            _memoryCache = memoryCache;
        }

        // GET: /<controller>/
        [HttpPost]
        [Route("Operacion/polling")]
        public IActionResult CargaManual()
        {
            string file = ".zip";

            string key = Guid.NewGuid().ToString();

            Task.Run(() => GuardaObtieneCache(null, file, key));

            //GuardaObtieneCache(null, file, key);

            var response = new Response<string>();

            response.Success = true;
            response.Item = key;

            return Ok(response);
        }

        [HttpPost]
        [Route("Operacion/polling/{message}")]
        public IActionResult CargaManual(string message)
        {
            string file = ".zip";

            string key = Guid.NewGuid().ToString();

            Task.Run(() => GuardaObtieneCache(message, file, key));

            return Ok(new Response<string> { Success = true, Item = key });
        }

        // GET: /<controller>/
        [HttpGet]
        [Route("Operacion/polling/{key}")]
        public IActionResult RecuperarCargaManual(string key)
        {
            var resultado =  GuardaObtieneCache(null, null, key);

            if (resultado == null)
                return NoContent();

            if (resultado == "Faulted")
                return StatusCode(500);

            return Ok(new Response<string>() { Item = resultado});
        }


        public string GuardaObtieneCache(string message, string file, string key)
        {
            var resultado = _memoryCache.Get<string>(key);

            if (resultado == null)
            {
                if (file != null)
                {
                    var timeExpirationPolicy = TimeSpan.FromMinutes(30);
                    try
                    {
                        resultado = new BusinessService().DoSomeLongRunningStuff(message);

                        _memoryCache.Set(key, resultado, timeExpirationPolicy);

                        return null;
                    }
                    catch (Exception)
                    {
                        _memoryCache.Set(key, resultado = "Faulted" , timeExpirationPolicy);
                    }
                }
            }
            else
                _memoryCache.Remove(key);

            return resultado;
        }
    }
}
