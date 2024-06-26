﻿using System;
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
    public class PostalFacilitiesController : Controller
    {
        private readonly DbpostOfficeContext _context;

        public PostalFacilitiesController(DbpostOfficeContext context)
        {
            _context = context;
        }

        // GET: PostalFacilities
        public async Task<IActionResult> Index()
        {
            var dbpostOfficeContext = _context.PostalFacilitys
                .Where(x => x.FacilityType.Type == "Поштомат" || x.FacilityType.Type == "Приймальне відділення")
                .Include(p => p.FacilityType);
            return View(await dbpostOfficeContext.ToListAsync());
        }

        // GET: PostalFacilities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postalFacility = await _context.PostalFacilitys
                .Where(x => x.FacilityType.Type == "Поштомат" || x.FacilityType.Type == "Приймальне відділення")
                .Include(p => p.FacilityType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postalFacility == null)
            {
                return NotFound();
            }

            return View(postalFacility);
        }
    }
}
