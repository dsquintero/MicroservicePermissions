using MediatR;
using MicroservicePermissions.Application.Features.PermissionTypes.Commands.CreatePermissionType;
using MicroservicePermissions.Application.Features.PermissionTypes.Commands.DeletePermissionType;
using MicroservicePermissions.Application.Features.PermissionTypes.Commands.UpdatePermissionType;
using MicroservicePermissions.Application.Features.PermissionTypes.Queries.GetAllPermissionTypes;
using MicroservicePermissions.Application.Features.PermissionTypes.Queries.GetPermissionTypeById;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicePermissions.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePermissionTypeCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePermissionTypeCommand command)
        {
            if (id != command.Id)
                return BadRequest("El ID de la ruta no coincide con el del cuerpo.");

            var result = await _mediator.Send(command);
            if (!result)
                return NotFound($"No se encontró el tipo de permiso con ID {id}");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeletePermissionTypeCommand(id));
            if (!result)
                return NotFound($"No se encontró un tipo de permiso con ID {id}");

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetPermissionTypeByIdQuery(id));
            if (result == null)
                return NotFound($"No se encontró tipo de permiso con ID {id}");

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllPermissionTypesQuery());
            return Ok(result);
        }
    }
}
