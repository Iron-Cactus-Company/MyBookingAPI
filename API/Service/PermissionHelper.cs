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
    public async Task<bool> IsCompanyOwner(string businessProfileId, string companyId)
    {
        var company = await _mediator.Send(new GetOne.Query{Id = new Guid(companyId)});
        if (company.Value == null)
            return false;

        return company.Value.BusinessProfileId.ToString() == businessProfileId;
    }
    public async Task<bool> IsCompanyOwner(Guid businessProfileId, string companyId)
    {
        var company = await _mediator.Send(new GetOne.Query{Id = new Guid(companyId)});
        if (company.Value == null)
            return false;

        return company.Value.BusinessProfileId.ToString() == businessProfileId.ToString();
    }
    public async Task<bool> IsCompanyOwner(string businessProfileId, Guid companyId)
    {
        var company = await _mediator.Send(new GetOne.Query{Id = companyId});
        if (company.Value == null)
            return false;

        return company.Value.BusinessProfileId.ToString() == businessProfileId;
    }
}