using PostOfficeDomain.Model;
using PostOfficeInfrastructure.Services;

namespace PostOfficeInfrastructure.Services
{
    public class AutorDataPortServiceFactory : IDataPortServiceFactory<Parcel>
    {
        private readonly DbpostOfficeContext _context;
        public AutorDataPortServiceFactory(DbpostOfficeContext context)
        {
            _context = context;
        }
        public IImportService<Parcel> GetImportService(string contentType)
        {
            if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new ParcelImportService(_context);
            }
            throw new NotImplementedException($"No import service implemented for movies with content type {contentType}");
        }
        public IExportService<Parcel> GetExportService(string contentType)
        {
            if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new ParcelExportService(_context);
            }
            throw new NotImplementedException($"No export service implemented for movies with content type {contentType}");
        }
    }

}
