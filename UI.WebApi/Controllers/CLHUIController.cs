using Core.Application.Common.Interfaces;
using Core.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.WebApi.Middleware;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UI.WebApi.Controllers
{
    [Route("~/smw-api/[controller]")]
    [ApiController]
    public class CLHUIController : ControllerBase
    {
        private readonly ICLHUIService _CLHUIService;

        public CLHUIController(ICLHUIService pcLHUIService)
        {
            _CLHUIService = pcLHUIService;
        }

        [HttpGet]
        [Permission("statistical.clhuis")]
        public async Task<IActionResult> RunAlgorithmAsync(int pMinUtil, int? pMonth, int? pYear)
        {
            try
            {
                var result = await _CLHUIService.RunAlgorithm(pMinUtil, pMonth, pYear);

                return Ok(new { data = result});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("detail")]
        [Permission("statistical.clhuis")]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var result = await _CLHUIService.Detail(id);

                return Ok(new { data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
