using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViWork.DAL;
using ViWork.WebApp.Authentication;
using ViWork.WebApp.Models.SchemaViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ViWork.WebApp.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme)]
    public class SchemaController : Controller
    {
        SchemaGateaway _schemaGateaway;

        public SchemaController(SchemaGateaway schemaGateaway)
        {
            _schemaGateaway = schemaGateaway;
        }

        [HttpGet("GetSchemaById/{userId}")]
        public async Task<IActionResult> GetSchema(int userId)
        {
            IEnumerable<SchemaData>  result = await _schemaGateaway.FindById(userId);
            return Ok(result);
        }

        [HttpGet("{name}", Name = "GetSchemaByName")]
        public async Task<IActionResult> GetSchemaByName( string schemaName)
        {
            SchemaData result = await _schemaGateaway.FindByName(schemaName);
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetSchemaGroup")]
        public async Task<IActionResult> GetSchemaGroup(int schemaId)
        {
            GroupData result = await _schemaGateaway.FindSchemaGroup(schemaId);
            return Ok(result);
        }

        [HttpPost("CreateSchema")]
        public async Task<IActionResult> AddSchema([FromBody] SchemaViewModel model)
        {

            Result result = await _schemaGateaway.AddSchema(model.SchemaName, model.GroupId);
            return Ok(result);
        }


        [HttpDelete("DeleteSchemaById/{schemaId}")]
        public async Task<IActionResult> DeleteGroup(int schemaId)
        {
            Result result = await _schemaGateaway.DeleteSchema(schemaId);
            return this.CreateResult(result);
        }


    }
}
