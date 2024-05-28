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
            worksheet.Cell(rowIndex, columnIndex++).Value = parcel.SenderId.ToString();
            worksheet.Cell(rowIndex, columnIndex++).Value = parcel.ReciverId.ToString();
            worksheet.Cell(rowIndex, columnIndex++).Value = parcel.DeparturePointsId.ToString();
            worksheet.Cell(rowIndex, columnIndex++).Value = parcel.DeliveryPointsId.ToString();
            worksheet.Cell(rowIndex, columnIndex++).Value = parcel.Price.ToString();
            worksheet.Cell(rowIndex, columnIndex++).Value = parcel.StatusId.ToString();
            worksheet.Cell(rowIndex, columnIndex++).Value = parcel.CurrentLocationId.ToString();
            worksheet.Cell(rowIndex, columnIndex++).Value = parcel.DeliveryAddress;
        }

        private void WriteAutors(IXLWorksheet worksheet, ICollection<Parcel> autors)
        {
            WriteHeader(worksheet);
            int rowIndex = 2;
            foreach (var autor in autors)
            {
                WriteAutor(worksheet, autor, rowIndex);
                rowIndex++;
            }
        }

        public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanWrite)
            {
                throw new ArgumentException("Input stream is not writable");
            }

            var autors = await _context.Parcels.ToListAsync(cancellationToken);
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Parcels");
            WriteAutors(worksheet, autors);
            workbook.SaveAs(stream);
        }
    }
}
