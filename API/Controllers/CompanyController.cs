using API.Contracts.Company;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class CompanyController : BaseApiController
    {
        
        private readonly IMapper _mapper;
        
        public CompanyController(IMapper mapper)
        {
            _mapper = mapper;
        }
        
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
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
            
            var companyResponse = _mapper.Map<CompanyResponseObject>(createCompanyDto);
            companyResponse.Id = createdCompanyId;

            return CreatedAtAction(nameof(Get), new { id = createdCompanyId }, companyResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCompanyDto updateCompanyDto)
        {
            var companyResponse = _mapper.Map<CompanyResponseObject>(updateCompanyDto);
            companyResponse.Id = updateCompanyDto.Id;
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
