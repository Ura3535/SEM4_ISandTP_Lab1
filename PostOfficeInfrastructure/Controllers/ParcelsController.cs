using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PostOfficeDomain.Model;
using PostOfficeInfrastructure;

namespace PostOfficeInfrastructure.Controllers
{
    public class ParcelsController : Controller
    {
        private readonly DbpostOfficeContext _context;

        public ParcelsController(DbpostOfficeContext context)
        {
            _context = context;
        }

        // GET: Parcels
        public async Task<IActionResult> Index()
        {
            var dbpostOfficeContext = _context.Parcels
                .Include(p => p.CurrentLocation)
                .Include(p => p.DeliveryPoints)
                .Include(p => p.DeparturePoints)
                .Include(p => p.Reciver)
                .Include(p => p.Sender)
                .Include(p => p.Status);
            return View(await dbpostOfficeContext.ToListAsync());
        }

        // GET: Parcels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parcel = await _context.Parcels
                .Include(p => p.CurrentLocation)
                .Include(p => p.DeliveryPoints)
                .Include(p => p.DeparturePoints)
                .Include(p => p.Reciver)
                .Include(p => p.Sender)
                .Include(p => p.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parcel == null)
            {
                return NotFound();
            }

            return View(parcel);
        }

        // GET: Parcels/Create
        public IActionResult Create()
        {
            ViewData["DeliveryPointsId"] = new SelectList(_context.PostalFacilitys, "Id", "Address");
            ViewData["DeparturePointsId"] = new SelectList(_context.PostalFacilitys, "Id", "Address");
            ViewData["ReciverId"] = new SelectList(_context.Clients, "Id", "ContactNumber");
            ViewData["SenderId"] = new SelectList(_context.Clients, "Id", "ContactNumber");
            return View();
        }

        // POST: Parcels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Info,Weight,SenderId,ReciverId,DeparturePointsId,DeliveryPointsId,Price,StatusId,CurrentLocationId,DeliveryAddress,Id")] Parcel parcel)
        {
            //ModelState.Clear();
            ProcessTheParcel(parcel);
            //TryValidateModel(parcel);

            //if (ModelState.IsValid)   //Незнаю чого не робить, вже все перепробував(  Вже немає сил терпіти ці пекельні борошна
            if (IsValid(parcel))
            {
                _context.Add(parcel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeliveryPointsId"] = new SelectList(_context.PostalFacilitys.Where(x => x.WeightRestrictions >= parcel.Weight).ToList(), "Id", "Address", parcel.DeliveryPointsId);
            ViewData["DeparturePointsId"] = new SelectList(_context.PostalFacilitys.Where(x => x.WeightRestrictions >= parcel.Weight).ToList(), "Id", "Address", parcel.DeparturePointsId);
            ViewData["ReciverId"] = new SelectList(_context.Clients, "Id", "ContactNumber", parcel.ReciverId);
            ViewData["SenderId"] = new SelectList(_context.Clients, "Id", "ContactNumber", parcel.SenderId);
            return View(parcel);
        }

        private void ProcessTheParcel(Parcel parcel)
        {
            parcel.StatusId = _context.ParcelStatuses.Where(x => x.Status == "Очікується відправника").First().Id;
            parcel.Price = CalculatePrice(parcel);
            parcel.CurrentLocationId = parcel.DeliveryPointsId;
        }
        private int CalculatePrice(Parcel parcel)
        {
            const int standartPrice = 30;
            const int courierDeliveryPrice = 50;
            return (parcel.DeliveryAddress == null ? courierDeliveryPrice : 0) + standartPrice + (int)(parcel.Weight * 10);
        }
        private bool IsValid(Parcel parsel)
        {
            return parsel.DeparturePointsId != parsel.DeliveryPointsId
                && parsel.SenderId != parsel.ReciverId
                && parsel.Info.Length < 50
                && parsel.Weight > 0
                && _context.PostalFacilitys
                .Where(x => x.Id == parsel.DeliveryPointsId || x.Id == parsel.DeparturePointsId)
                .All(x => x.WeightRestrictions >= parsel.Weight);
        }
        private bool ParcelExists(int id)
        {
            return _context.Parcels.Any(e => e.Id == id);
        }
    }
}
