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
            var dbpostOfficeContext = _context.Parcels.Include(p => p.CurrentLocation).Include(p => p.DeliveryPoints).Include(p => p.DeparturePoints).Include(p => p.Reciver).Include(p => p.Sender).Include(p => p.Status);
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
            ViewData["CurrentLocationId"] = new SelectList(_context.PostalFacilitys, "Id", "Address");
            ViewData["DeliveryPointsId"] = new SelectList(_context.PostalFacilitys, "Id", "Address");
            ViewData["DeparturePointsId"] = new SelectList(_context.PostalFacilitys, "Id", "Address");
            ViewData["ReciverId"] = new SelectList(_context.Clients, "Id", "ContactNumber");
            ViewData["SenderId"] = new SelectList(_context.Clients, "Id", "ContactNumber");
            ViewData["StatusId"] = new SelectList(_context.ParcelStatuses, "Id", "Status");
            return View();
        }

        // POST: Parcels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Info,Weight,SenderId,ReciverId,DeparturePointsId,DeliveryPointsId,Price,StatusId,CurrentLocationId,DeliveryAddress,Id")] Parcel parcel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parcel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CurrentLocationId"] = new SelectList(_context.PostalFacilitys, "Id", "Address", parcel.CurrentLocationId);
            ViewData["DeliveryPointsId"] = new SelectList(_context.PostalFacilitys, "Id", "Address", parcel.DeliveryPointsId);
            ViewData["DeparturePointsId"] = new SelectList(_context.PostalFacilitys, "Id", "Address", parcel.DeparturePointsId);
            ViewData["ReciverId"] = new SelectList(_context.Clients, "Id", "ContactNumber", parcel.ReciverId);
            ViewData["SenderId"] = new SelectList(_context.Clients, "Id", "ContactNumber", parcel.SenderId);
            ViewData["StatusId"] = new SelectList(_context.ParcelStatuses, "Id", "Status", parcel.StatusId);
            return View(parcel);
        }

        // GET: Parcels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parcel = await _context.Parcels.FindAsync(id);
            if (parcel == null)
            {
                return NotFound();
            }
            ViewData["CurrentLocationId"] = new SelectList(_context.PostalFacilitys, "Id", "Address", parcel.CurrentLocationId);
            ViewData["DeliveryPointsId"] = new SelectList(_context.PostalFacilitys, "Id", "Address", parcel.DeliveryPointsId);
            ViewData["DeparturePointsId"] = new SelectList(_context.PostalFacilitys, "Id", "Address", parcel.DeparturePointsId);
            ViewData["ReciverId"] = new SelectList(_context.Clients, "Id", "ContactNumber", parcel.ReciverId);
            ViewData["SenderId"] = new SelectList(_context.Clients, "Id", "ContactNumber", parcel.SenderId);
            ViewData["StatusId"] = new SelectList(_context.ParcelStatuses, "Id", "Status", parcel.StatusId);
            return View(parcel);
        }

        // POST: Parcels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Info,Weight,SenderId,ReciverId,DeparturePointsId,DeliveryPointsId,Price,StatusId,CurrentLocationId,DeliveryAddress,Id")] Parcel parcel)
        {
            if (id != parcel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parcel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParcelExists(parcel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CurrentLocationId"] = new SelectList(_context.PostalFacilitys, "Id", "Address", parcel.CurrentLocationId);
            ViewData["DeliveryPointsId"] = new SelectList(_context.PostalFacilitys, "Id", "Address", parcel.DeliveryPointsId);
            ViewData["DeparturePointsId"] = new SelectList(_context.PostalFacilitys, "Id", "Address", parcel.DeparturePointsId);
            ViewData["ReciverId"] = new SelectList(_context.Clients, "Id", "ContactNumber", parcel.ReciverId);
            ViewData["SenderId"] = new SelectList(_context.Clients, "Id", "ContactNumber", parcel.SenderId);
            ViewData["StatusId"] = new SelectList(_context.ParcelStatuses, "Id", "Status", parcel.StatusId);
            return View(parcel);
        }

        // GET: Parcels/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Parcels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parcel = await _context.Parcels.FindAsync(id);
            if (parcel != null)
            {
                _context.Parcels.Remove(parcel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParcelExists(int id)
        {
            return _context.Parcels.Any(e => e.Id == id);
        }
    }
}
