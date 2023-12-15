using API.Contracts.Company;
using API.Service;
using Application.CompanyActions;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class CompanyController : BaseApiController
    {
        
        private readonly IMapper _mapper;
        private readonly PermissionHelper _permissionHelper;

        public CompanyController(IMapper mapper, PermissionHelper permissionHelper)
        {
            _mapper = mapper;
            _permissionHelper = permissionHelper;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetMany()
        {
            var result = await Mediator.Send(new GetMany.Query());
            var serializedResult = _mapper.Map<IList<CompanyResponseObject>>(result.Value);
            
            return Ok(serializedResult);
        }
        
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await Mediator.Send(new GetOne.Query{ Id = id});
            if (result.Value == null)
            {
                return NotFound();
            }
            var serializedResult = _mapper.Map<CompanyResponseObject>(result.Value);
            return Ok(serializedResult);
        }
        

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCompanyDto createCompanyDto)
        {
            // todo result returns empty value 
            var result = await Mediator.Send(new Create.Command
            {
                Company = _mapper.Map<Company>(createCompanyDto)
            });
            if (!result.IsSuccess)
            {
                return Conflict();
            }
            return Ok();

            // return CreatedAtAction(nameof(Get), new { id = result.value.Id }, companyResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCompanyDto updateCompanyDto)
        {
            if (! await _permissionHelper.IsCompanyOwner(GetLoggedUserId(), updateCompanyDto.Id))
                return Unauthorized();
            
            var result = await Mediator.Send(new Update.Command
            {
                Company = _mapper.Map<Company>(updateCompanyDto)
            });
            
            if (!result.IsSuccess)
            {
                return Conflict();
            }
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (! await _permissionHelper.IsCompanyOwner(GetLoggedUserId(), id))
                return Unauthorized();
            
            //todo fix internal error 500 when notfound
            var result = await Mediator.Send(new Delete.Command{ Id = id });
            if (!result.IsSuccess)
            {
                return Conflict();
            }
            return NoContent();
        }
    }
}
