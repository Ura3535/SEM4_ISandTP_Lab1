using ClosedXML.Excel;
using PostOfficeDomain.Model;
using Microsoft.EntityFrameworkCore;
using PostOfficeInfrastructure;
using PostOfficeInfrastructure.Services;

namespace PostOfficeInfrastructure.Services
{
    public class ParcelImportService : IImportService<Parcel>
    {
        private readonly DbpostOfficeContext _context;
        // реалізація AddMovieAsync та інших методів

        public ParcelImportService(DbpostOfficeContext context)
        {
            _context = context;
        }

        public async Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanRead)
            {
                throw new ArgumentException("Дані не можуть бути прочитані", nameof(stream));
            }

            using (XLWorkbook workBook = new XLWorkbook(stream))
            {
                foreach (IXLWorksheet worksheet in workBook.Worksheets)
                {
                    foreach (var row in worksheet.RowsUsed().Skip(1))
                    {
                        await AddParcelAsync(row, cancellationToken);
                    }
                }
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task AddParcelAsync(IXLRow row, CancellationToken cancellationToken)
        {
            Parcel parcel = new Parcel
            {
                Info = row.Cell(1).Value.ToString(),
                Weight = double.Parse(row.Cell(2).Value.ToString()),
                SenderId = int.Parse(row.Cell(3).Value.ToString()),
                ReciverId = int.Parse(row.Cell(4).Value.ToString()),
                DeparturePointsId = int.Parse(row.Cell(5).Value.ToString()),
                DeliveryPointsId = int.Parse(row.Cell(6).Value.ToString()),
                Price = int.Parse(row.Cell(7).Value.ToString()),
                StatusId = int.Parse(row.Cell(8).Value.ToString()),
                CurrentLocationId = int.Parse(row.Cell(9).Value.ToString()),
                DeliveryAddress = row.Cell(10).Value.ToString()
            };

            _context.Parcels.Add(parcel);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
