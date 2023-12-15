using Application.CompanyActions;
using MediatR;

namespace API.Service;

public class PermissionHelper
{
    private readonly IMediator _mediator;

    public PermissionHelper(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<bool> IsCompanyOwner(Guid businessProfileId, Guid companyId)
    {
        var company = await _mediator.Send(new GetOne.Query{Id = companyId});
        if (company.Value == null)
            return false;

        return company.Value.BusinessProfileId == businessProfileId;
    }
}