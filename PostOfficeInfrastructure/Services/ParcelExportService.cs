using PostOfficeDomain.Model;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using PostOfficeInfrastructure.Services;
using PostOfficeInfrastructure;

namespace PostOfficeInfrastructure.Services
{
    public class ParcelExportService : IExportService<Parcel>
    {
        private static readonly IReadOnlyList<string> HeaderNames =
        [
            "Інфо",
            "Вага",
            "Відправник",
            "Отримувач",
            "Точка відправки",
            "Точка отримання",
            "Вартість",
            "Статус",
            "Місце перебування",
            "Адреса доставки"
        ];

        private readonly DbpostOfficeContext _context;

        public ParcelExportService(DbpostOfficeContext context)
        {
            _context = context;
        }

        private static void WriteHeader(IXLWorksheet worksheet)
        {
            for (int columnIndex = 0; columnIndex < HeaderNames.Count; columnIndex++)
            {
                worksheet.Cell(1, columnIndex + 1).Value = HeaderNames[columnIndex];
            }
            worksheet.Row(1).Style.Font.Bold = true;
        }

        private void WriteAutor(IXLWorksheet worksheet, Parcel parcel, int rowIndex)
        {
            var columnIndex = 1;
            worksheet.Cell(rowIndex, columnIndex++).Value = parcel.Info;
            worksheet.Cell(rowIndex, columnIndex++).Value = parcel.Weight.ToString();
            worksheet.Cell(rowIndex, columnIndex++).Value = $"{parcel.Sender.Name} (тел. {parcel.Sender.ContactNumber})";
            worksheet.Cell(rowIndex, columnIndex++).Value = $"{parcel.Reciver.Name} (тел. {parcel.Reciver.ContactNumber})";
            worksheet.Cell(rowIndex, columnIndex++).Value = $"{parcel.DeparturePoints.Name}; адреса: {parcel.DeparturePoints.Address}";
            worksheet.Cell(rowIndex, columnIndex++).Value = $"{parcel.DeliveryPoints.Name}; адреса: {parcel.DeliveryPoints.Address}";
            worksheet.Cell(rowIndex, columnIndex++).Value = parcel.Price.ToString();
            worksheet.Cell(rowIndex, columnIndex++).Value = parcel.Status.Status;
            worksheet.Cell(rowIndex, columnIndex++).Value = $"{parcel.CurrentLocation.Name}; адреса: {parcel.CurrentLocation.Address}";
            worksheet.Cell(rowIndex, columnIndex++).Value = parcel.DeliveryAddress;
        }

        private void WriteAutors(IXLWorksheet worksheet, ICollection<Parcel> parcels)
        {
            WriteHeader(worksheet);
            int rowIndex = 2;
            foreach (var parcel in parcels)
            {
                WriteAutor(worksheet, parcel, rowIndex);
                rowIndex++;
            }
        }

        public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanWrite)
            {
                throw new ArgumentException("Input stream is not writable");
            }

            var parcels = await _context.Parcels
                .Include(p => p.CurrentLocation)
                .Include(p => p.DeliveryPoints)
                .Include(p => p.DeparturePoints)
                .Include(p => p.Reciver)
                .Include(p => p.Sender)
                .Include(p => p.Status)
                .ToListAsync(cancellationToken);

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Parcels");
            WriteAutors(worksheet, parcels);
            workbook.SaveAs(stream);
        }
    }
}
