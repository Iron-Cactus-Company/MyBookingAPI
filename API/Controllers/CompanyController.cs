using API.Contracts.Company;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class CompanyController : BaseApiController
    {
        
        // private readonly IMapper _mapper;
        //
        // public CompanyController(IMapper mapper)
        // {
        //     _mapper = mapper;
        // }
        
        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            // TODO: Implement logic to get a company by ID asynchronously
            var value = $"Value {id}";
            return Ok(value);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // TODO: Implement logic to get all companies asynchronously
            return Ok("List of all companies");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCompanyDto createCompanyDto)
        {
            // TODO: Implement logic to add a company asynchronously
            var createdCompanyId = Guid.NewGuid().ToString();
            
            var companyResponse = new CompanyResponseObject
            {
                Id = createdCompanyId,
                Name = createCompanyDto.Name,
                Address = createCompanyDto.Address,
                Phone = createCompanyDto.Phone,
                Email = createCompanyDto.Email,
                Description = createCompanyDto.Description,
                AcceptingTimeText = createCompanyDto.AcceptingTimeText,
                CancellingTimeMessage = createCompanyDto.CancellingTimeMessage
            };

            return CreatedAtAction(nameof(Get), new { id = createdCompanyId }, companyResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCompanyDto updateCompanyDto)
        {
            // dynamic companyResponse = new ExpandoObject();

            CompanyResponseObject companyResponse = new CompanyResponseObject
            {
                Id = updateCompanyDto.Id,
                Name = updateCompanyDto.Name,
                Address = updateCompanyDto.Address,
                Phone = updateCompanyDto.Phone,
                Email = updateCompanyDto.Email,
                Description = updateCompanyDto.Description,
                AcceptingTimeText = updateCompanyDto.AcceptingTimeText,
                CancellingTimeMessage = updateCompanyDto.CancellingTimeMessage
            };
            
            // var companyResponse = _mapper.Map<CompanyResponseObject>(updateCompanyDto);
            // TODO: Implement logic to update a company asynchronously
            return Ok(companyResponse);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            // TODO: Implement logic to delete a company asynchronously
            return NoContent();
        }
    }
}
