using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PostOfficeDomain.Model;
using PostOfficeInfrastructure;
using PostOfficeInfrastructure.Services;

namespace PostOfficeInfrastructure.Controllers
{
    public class ParcelsController : Controller
    {
        private readonly DbpostOfficeContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public ParcelsController(DbpostOfficeContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Parcels
        public async Task<IActionResult> Index()
        {
            if (!_signInManager.IsSignedIn(User))
                return RedirectToAction("Register", "Account");

            Client client = await GetClientAsync();

            var dbpostOfficeContext = _context.Parcels
                .Where(x => x.ReciverId == client.Id || x.SenderId == client.Id)
                .Include(p => p.CurrentLocation)
                .Include(p => p.DeliveryPoints)
                .Include(p => p.DeparturePoints)
                .Include(p => p.Reciver)
                .Include(p => p.Sender)
                .Include(p => p.Status);

            return View(await dbpostOfficeContext.ToListAsync());
        }

        [Authorize(Roles = "worker")]
        public async Task<IActionResult> ParcelList()
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
            var parcel = await _context.Parcels
                .Where(x => x.Id == id)
                .Include(p => p.CurrentLocation)
                .Include(p => p.DeliveryPoints)
                .Include(p => p.DeparturePoints)
                .Include(p => p.Reciver)
                .Include(p => p.Sender)
                .Include(p => p.Status)
                .FirstOrDefaultAsync();

            if (parcel == null)
            {
                return NotFound();
            }

            return View(parcel);
        }

        // GET: Parcels/Create
        public async Task<IActionResult> CreateAsync()
        {
            if (!_signInManager.IsSignedIn(User))
                return RedirectToAction("Register", "Account");

            Client client = await GetClientAsync();

            ViewData["DeliveryPointsId"] = new SelectList(_context.PostalFacilitys, "Id", "Address");
            ViewData["DeparturePointsId"] = new SelectList(_context.PostalFacilitys, "Id", "Address");
            ViewData["ReciverId"] = new SelectList(_context.Clients.Where(x => x.Id != client.Id).ToList(), "Id", "ContactNumber");
            return View();
        }

        // POST: Parcels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Info,Weight,SenderId,ReciverId,DeparturePointsId,DeliveryPointsId,Price,StatusId,CurrentLocationId,DeliveryAddress,Id")] Parcel parcel)
        {
            if (!_signInManager.IsSignedIn(User))
                return RedirectToAction("Register", "Account");

            Client client = await GetClientAsync();

            ProcessTheParcel(parcel, client);

            //if (ModelState.IsValid)   //Незнаю чого не робить, все перепробував(  Вже немає сил терпіти ці пекельні борошна
            if (IsValid(parcel))
            {
                _context.Add(parcel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeliveryPointsId"] = new SelectList(_context.PostalFacilitys.Where(x => x.WeightRestrictions >= parcel.Weight).ToList(), "Id", "Address", parcel.DeliveryPointsId);
            ViewData["DeparturePointsId"] = new SelectList(_context.PostalFacilitys.Where(x => x.WeightRestrictions >= parcel.Weight).ToList(), "Id", "Address", parcel.DeparturePointsId);
            ViewData["ReciverId"] = new SelectList(_context.Clients.Where(x => x.Id != client.Id).ToList(), "Id", "ContactNumber", parcel.ReciverId);
            return View(parcel);
        }

        // GET: Parcels/Edit/5
        [Authorize(Roles = "worker")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parcel = await _context.Parcels
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (parcel == null)
            {
                return NotFound();
            }

            ViewData["DeliveryPointsId"] = new SelectList(_context.PostalFacilitys, "Id", "Address", parcel.DeliveryPointsId);
            ViewData["DeparturePointsId"] = new SelectList(_context.PostalFacilitys, "Id", "Address", parcel.DeparturePointsId);
            ViewData["CurrentLocationId"] = new SelectList(_context.PostalFacilitys, "Id", "Address", parcel.CurrentLocationId);
            ViewData["ReciverId"] = new SelectList(_context.Clients, "Id", "ContactNumber", parcel.ReciverId);
            ViewData["SenderId"] = new SelectList(_context.Clients, "Id", "ContactNumber", parcel.SenderId);
            ViewData["StatusId"] = new SelectList(_context.ParcelStatuses, "Id", "Status", parcel.StatusId);
            return View(parcel);
        }

        // POST: Parcels/Edit/5
        [HttpPost]
        [Authorize(Roles = "worker")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Info,Weight,SenderId,ReciverId,DeparturePointsId,DeliveryPointsId,Price,StatusId,CurrentLocationId,DeliveryAddress")] Parcel parcel)
        {
            if (id != parcel.Id)
            {
                return NotFound();
            }

            if (IsValid(parcel))
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
                return RedirectToAction(nameof(ParcelList));
            }

            ViewData["DeliveryPointsId"] = new SelectList(_context.PostalFacilitys, "Id", "Address", parcel.DeliveryPointsId);
            ViewData["DeparturePointsId"] = new SelectList(_context.PostalFacilitys, "Id", "Address", parcel.DeparturePointsId);
            ViewData["ReciverId"] = new SelectList(_context.Clients, "Id", "ContactNumber", parcel.ReciverId);
            return View(parcel);
        }

        // GET: Parcels/Delete/5
        [Authorize(Roles = "worker")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parcel = await _context.Parcels
                .Where(x => x.Id == id)
                .Include(p => p.CurrentLocation)
                .Include(p => p.DeliveryPoints)
                .Include(p => p.DeparturePoints)
                .Include(p => p.Reciver)
                .Include(p => p.Sender)
                .Include(p => p.Status)
                .FirstOrDefaultAsync();

            if (parcel == null)
            {
                return NotFound();
            }

            return View(parcel);
        }

        // POST: Parcels/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "worker")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!_signInManager.IsSignedIn(User))
                return RedirectToAction("Register", "Account");

            var parcel = await _context.Parcels.FindAsync(id);
            _context.Parcels.Remove(parcel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private void ProcessTheParcel(Parcel parcel, Client client)
        {
            parcel.SenderId = client.Id;
            parcel.StatusId = _context.ParcelStatuses.Where(x => x.Status == "Очікується відправника").First().Id;
            parcel.Price = CalculatePrice(parcel);
            parcel.CurrentLocationId = parcel.DeparturePointsId;
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

        private async Task<Client> GetClientAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var client = _context.Clients
                .Where(x => x.ContactNumber == user.ContactNumber)
                .First();

            return client;
        }
        private bool ParcelExists(int id)
        {
            return _context.Parcels.Any(e => e.Id == id);
        }

        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile fileExcel, CancellationToken cancellationToken)
        {
            var factory = new AutorDataPortServiceFactory(_context);
            var importService = factory.GetImportService(fileExcel.ContentType);

            using var stream = fileExcel.OpenReadStream();
            await importService.ImportFromStreamAsync(stream, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Export([FromQuery] string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    CancellationToken cancellationToken = default)
        {
            var factory = new AutorDataPortServiceFactory(_context);
            var exportService = factory.GetExportService(contentType);

            var memoryStream = new MemoryStream();

            await exportService.WriteToAsync(memoryStream, cancellationToken);

            await memoryStream.FlushAsync(cancellationToken);
            memoryStream.Position = 0;

            return new FileStreamResult(memoryStream, contentType)
            {
                FileDownloadName = $"parcel_{DateTime.UtcNow.ToShortDateString()}.xlsx"
            };
        }
    }
}