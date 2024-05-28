using PostOfficeDomain.Model;

namespace PostOfficeInfrastructure.Services
{
    public interface IExportService<TEntity>
 where TEntity : Entity
    {
        Task WriteToAsync(Stream stream, CancellationToken cancellationToken);
    }

}
