using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PostOfficeInfrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private record CountByYearResponseItem(string Year, int Count);

        private readonly DbpostOfficeContext _context;

        public ChartController(DbpostOfficeContext _context) 
        {
            this._context = _context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            List<object> res = new List<object>();
            res.Add(new[] { "Тип відділення", "Кількість відправлених посилок" });
            foreach (var c in _context.FacilityTypes.ToList())
            {
                res.Add(new object[] {
                    c.Type,
                    _context.Parcels
                        .Join(
                            _context.PostalFacilitys,
                            parcel => parcel.DeparturePointsId,
                            facility => facility.Id,
                            (parcel, facility) => new { parcel, facility }
                        )
                        .Where(pf => pf.facility.FacilityTypeId == c.Id)
                        .Count()
            });
            }

            return new JsonResult(res);
        }

        [HttpGet("JsonData/{Id}")]
        public JsonResult JsonData(int Id)
        {
            List<object> res = new List<object>();
            res.Add(new[] { "Кількість отриманих посилок", "Кількість відправлених посилок" });

            res.Add(new object[] {
                "Відправленні посилки",
            _context.Parcels
            .Where(parcel => parcel.DeparturePointsId == Id)
            .Count()
            });

            res.Add(new object[] {
                "Отримані посилки",
            _context.Parcels
            .Where(parcel => parcel.DeliveryPointsId == Id)
            .Count()
            });

            return new JsonResult(res);
        }

    }
}
