using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.EntityFrameworkCore;
using PlayProjectify.ApiService.Data;
using PlayProjectify.ApiService.Models.DTO;
using PlayProjectify.ApiService.Models.Mapping;
using PlayProjectify.ServiceDefaults;

namespace PlayProjectify.ApiService.Services;

public interface ICustomerService
{
    Task<ProjectifyServiceResult<IEnumerable<CustomerDto>>> GetAll(CancellationToken cancellationToken);
    Task<ProjectifyServiceResult<CustomerDto>> Get(Guid id, CancellationToken cancellationToken);
    Task<ProjectifyServiceResult<CustomerDto>> Patch(Guid id, JsonPatchDocument<PatchCustomerDto> patchDoc, CancellationToken ct);
}

public sealed class CustomerService : ICustomerService
{
    private readonly AppDbContext _dbContext;
    public CustomerService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<ProjectifyServiceResult<IEnumerable<CustomerDto>>> GetAll(CancellationToken cancellationToken)
    {
        return await _dbContext.Customers.Take(10)
            .Select(p => p.ToDto())
            .ToListAsync(cancellationToken);
    }

    public async Task<ProjectifyServiceResult<CustomerDto>> Get(Guid id, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (customer is not null)
            return customer.ToDto();

        return ProjectifyServiceResult<CustomerDto>.NotFound($"Order with ID {id} not found.");
    }

    public async Task<ProjectifyServiceResult<CustomerDto>> Patch(Guid id, JsonPatchDocument<PatchCustomerDto> patchDoc, CancellationToken ct)
    {
        var customer = await _dbContext.Customers.FindAsync([id], ct);
        if (customer is null)
            return ProjectifyServiceResult<CustomerDto>.NotFound($"Customer {id} not found.");

        // Map entity → DTO
        var dto = new PatchCustomerDto
        {
            PhoneNumber = customer.PhoneNumber,
            CompanyName = customer.CompanyName,
            Notes = customer.Notes,
            PreferredCurrency = customer.PreferredCurrency,
            PreferredLanguage = customer.PreferredLanguage,
            MarketingOptIn = customer.MarketingOptIn,
            IsActive = customer.IsActive
        };

        // Apply patch operations onto the DTO
        patchDoc.ApplyTo(dto);

        // Map DTO → entity
        customer.PhoneNumber = dto.PhoneNumber;
        customer.CompanyName = dto.CompanyName;
        customer.Notes = dto.Notes;
        customer.PreferredCurrency = dto.PreferredCurrency ?? customer.PreferredCurrency;
        customer.PreferredLanguage = dto.PreferredLanguage ?? customer.PreferredLanguage;
        customer.MarketingOptIn = dto.MarketingOptIn ?? customer.MarketingOptIn;
        customer.IsActive = dto.IsActive ?? customer.IsActive;
        customer.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(ct);
        return customer.ToDto();
    }
}