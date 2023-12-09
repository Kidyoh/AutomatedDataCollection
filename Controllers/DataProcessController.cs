using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutomatedDataCollectionApi.Services;

namespace AutomatedDataCollectionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataProcessController : ControllerBase
    {
        private readonly IDataProcessService _dataProcessService;

        public DataProcessController(IDataProcessService dataProcessService)
        {
            _dataProcessService = dataProcessService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> GetEndpoints()
        {
            List<string> endpoints = _dataProcessService.GetConfigFileEndPoints();
            return Ok(endpoints);
        }

        [HttpPost("add")]
        public async Task<ActionResult<string>> AddEndpoints([FromBody] JsonElement endpointsJson)
        {
            // Deserialize JSON array from request body
            List<string> endpoints = JsonSerializer.Deserialize<List<string>>(endpointsJson.ToString());

            string result = await _dataProcessService.AddConfigFileEndPoints(endpoints);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<ActionResult<string>> EditEndpoints([FromBody] JsonElement endpointsJson)
        {
            // Deserialize JSON array from request body
            List<string> endpoints = JsonSerializer.Deserialize<List<string>>(endpointsJson.ToString());

            string result = await _dataProcessService.EditConfigFileEndPoints(endpoints);
            return Ok(result);
        }


        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteEndpoint([FromBody] string endpointToDelete)
        {
            try
            {
                var result = await _dataProcessService.DeleteConfigFileEndPoints(new List<string> { endpointToDelete });
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle errors and return appropriate response
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
            }
        }

        [HttpPost("run-python")]
        public async Task<IActionResult> RunPythonScript()
        {
            string result = await _dataProcessService.RunPythonScript();

            return Ok(result);
        }
    }

}
