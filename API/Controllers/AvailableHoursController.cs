using API.Contracts.AvailableHours;
using Application.AvailableHours;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    public class AvailableHoursController : BaseApiController
    {
        private readonly IMapper _mapper;

        public AvailableHoursController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CalculateOpeningHours([FromBody] GenerateAvailableHoursDto body)
        {
            var result = await Mediator.Send(new GetAll.Query
            {
                ServiceId = new Guid(body.ServiceId),
                From = body.From,
                To = body.To
            });

            return HandleCreateResponse<List<Interval>, List<Interval>>(result);
        }
    }
}
