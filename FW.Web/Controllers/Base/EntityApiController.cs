using FW.BusinessLogic.Contracts;
using FW.Domain;
using FW.Web.Clients.Interfaces;
using FW.Web.RpcClients.Interfaces;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FW.Web.Controllers.Base;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public abstract class EntityApiController<T, K> : ControllerBase 
    where T : class
    where K : class
    ,IEntity
{
    protected IBaseClient<T, K> Client { get; }
    protected EntityApiController(IBaseClient<T, K> Client) => this.Client = Client;
    protected Guid UserId => Guid.Parse(User.FindFirst(JwtClaimTypes.Subject).Value);
    [HttpGet("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest();

        var response = await Client.Get(id);

        if (response.Id == Guid.Empty)
            return NotFound();

        return Ok(response);
    }

    [HttpGet("[action]/{Skip}:{Take})")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<IActionResult> GetPage(int Skip, int Take)
    {
        var response = await Client.GetPage(Skip, Take);

        if (!response.Any())
            return NotFound();

        return Ok(response);
    }
    
    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<IActionResult> GetAll()
    {
        var response = await Client.GetAll();

        if (!response.Any())
            return NotFound();

        return Ok(response);
    }

    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Count()
    {
        var response = await Client.Count();

        if (response == null)
            return NotFound();

        return Ok(response);
    }
    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<IActionResult> Add(T model)
    {
        if (model == null)
            return BadRequest();

        var response = await Client.Create(model,UserId);

        if (response.Status == StatusResult.Error)
            return StatusCode(StatusCodes.Status500InternalServerError);

        return Ok(response);
    }

    [HttpPut("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<IActionResult> Edit(Guid id, T model)
    {
        if (id == Guid.Empty || model== null)
            return BadRequest();

        var response = await Client.Update(id, model,UserId);

        if (response.Status == StatusResult.NotFound)
            return NotFound();

        return Ok(response);
    }

    [HttpDelete("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<IActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest();

        var response = await Client.Delete(id);

        if (response.Status == StatusResult.NotFound)
            return NotFound();

        return Ok(response);
    }
}
