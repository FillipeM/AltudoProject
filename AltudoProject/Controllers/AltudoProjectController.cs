using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AltudoProjectModels;
using AltudoProjectServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AltudoProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AltudoProjectController : ControllerBase
    {
        private readonly IAltudoProjectService service;

        public AltudoProjectController(IAltudoProjectService _service)
        {
            this.service = _service;
        }
        [HttpGet]
        public async Task<ActionResult<AltudoModel>> ProcessaURL([FromQueryAttribute] string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return new BadRequestObjectResult("URL can't be empty");
            }

            return new ActionResult<AltudoModel>( await service.processaURL(url));
        }
    }
}
