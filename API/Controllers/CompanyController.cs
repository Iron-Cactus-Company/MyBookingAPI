using API.Attributes;
using API.Contracts.Company;
using API.Service;
using Application.CompanyActions;
using Application.Core;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
        
        [OffsetPaginator]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetMany([FromQuery] string name)
        {
            var limit = (int)HttpContext.Items["limit"];
            var page = (int)HttpContext.Items["page"];
            
            var result = name.IsNullOrEmpty() ?
                await Mediator.Send(new GetMany.Query{ Options = new ReadOptions{ Limit = limit, PageNumber = page} }) :
                await Mediator.Send(new FindByName.Query { Name = name, Options = new ReadOptions { Limit = limit, PageNumber = page } });
            
            return HandleReadOneResponse<List<Company>, List<CompanyResponseObject>>(result);
        }
        
        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await Mediator.Send(new GetOne.Query{ Id = id});
            return HandleReadOneResponse<Company, CompanyResponseObject>(result);
        }
        

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCompanyDto createCompanyDto)
        {
            var result = await Mediator.Send(new Create.Command
            {
                Company = _mapper.Map<Company>(createCompanyDto)
            });
            
            return HandleCreateResponse<Company, CompanyResponseObject>(result);
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
            
            return HandleUpdateResponse(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (! await _permissionHelper.IsCompanyOwner(GetLoggedUserId(), id))
                return Unauthorized();
            
            var result = await Mediator.Send(new Delete.Command{ Id = id });
            
            return HandleDeleteResponse(result);
        }
    }
}
