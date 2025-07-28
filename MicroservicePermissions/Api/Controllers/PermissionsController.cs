using MediatR;
using MicroservicePermissions.Application.Features.Permissions.Commands.CreatePermission;
using MicroservicePermissions.Application.Features.Permissions.Commands.DeletePermission;
using MicroservicePermissions.Application.Features.Permissions.Commands.UpdatePermission;
using MicroservicePermissions.Application.Features.Permissions.Queries.GetAllPermissions;
using MicroservicePermissions.Application.Features.Permissions.Queries.GetPermissionById;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicePermissions.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreatePermissionCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePermissionCommand command)
        {
            if (id != command.Id)
                return BadRequest("El ID de la ruta no coincide con el del cuerpo.");

            var result = await _mediator.Send(command);
            if (!result)
                return NotFound($"No se encontró el permiso con ID {id}");

            return NoContent(); // 204
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeletePermissionCommand(id));
            if (!result)
                return NotFound($"No se encontró un permiso con ID {id}");

            return NoContent(); // 204
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetPermissionByIdQuery(id));
            if (result == null)
                return NotFound($"No se encontró permiso con ID {id}");

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllPermissionsQuery());
            return Ok(result);
        }

    }
}
