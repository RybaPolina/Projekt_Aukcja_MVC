﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Auctions.Data;
using Auctions.Models;
using Auctions.Data.Services;
using System.Security.Claims;

namespace Auctions.Controllers
{
    public class ListingsController : Controller
    {
        private readonly IListingsService _listingsService;
        private readonly IBidsService _bidsService;
        private readonly ICommentsService _commentsService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ListingsController(IListingsService listingsService, IWebHostEnvironment webHostEnvironment, IBidsService bidsService, ICommentsService commentsService)
        {
            _listingsService = listingsService;
            _webHostEnvironment = webHostEnvironment;
            _bidsService = bidsService;
            _commentsService = commentsService;
        }

        // GET: Listings
        public async Task<IActionResult> Index(int? pageNumber, string searchString)
        {
            var applicationDbContext = _listingsService.GetAll();
            int pageSize = 3;
            if (!string.IsNullOrEmpty(searchString))
            {
                applicationDbContext = applicationDbContext.Where(a => a.Title.Contains(searchString));
                return View(await PaginatedList<Listing>.CreateAsync(applicationDbContext.Where(l => l.IsSold == false).AsNoTracking(), pageNumber ?? 1, pageSize));
            }
            return View(await PaginatedList<Listing>.CreateAsync(applicationDbContext.Where(l => l.IsSold == false).AsNoTracking(), pageNumber ?? 1, pageSize));

        }

        

        // GET: Listings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = await _listingsService.GetById(id);
            if (listing == null)
            {
                return NotFound();
            }

            return View(listing);
        }


        // GET: Listings/Create
        public IActionResult Create()
        {
            return View();
        }

        

       // POST: Listings/Create
       // To protect from overposting attacks, enable the specific properties you want to bind to.
       // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       [HttpPost]
       [ValidateAntiForgeryToken]
       public async Task<IActionResult> Create(ListingVM listing)
       {
           if (listing.Image != null)
           {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                string fileName = listing.Image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    listing.Image.CopyTo(fileStream);
                }

                var listObj = new Listing
                {
                    Title = listing.Title,
                    Description = listing.Description,
                    Price = listing.Price,
                    IdentityUserId = listing.IdentityUserId,
                    ImagePath = fileName,
                };
                await _listingsService.Add(listObj);
                return RedirectToAction("Index");
           }
           return View(listing);
       }

        [HttpPost]
        public async Task<ActionResult> AddBid([Bind("Id, Price, ListingId, IdentityUserId")] Bid bid)
        {
            if (ModelState.IsValid)
            {
                await _bidsService.Add(bid);
            }
            var listing = await _listingsService.GetById(bid.ListingId);
            listing.Price = bid.Price;
            await _listingsService.SaveChanges();

            return View("Details", listing);
        }

        public async Task<ActionResult> CloseBidding(int id)
        {
            var listing = await _listingsService.GetById(id);

            if (listing == null)
            {
                return NotFound();
            }

            listing.IsSold = true;
            await _listingsService.SaveChanges();
            return View("Details", listing);
        }

        
        [HttpPost]

        public async Task<ActionResult> AddComment([Bind("Id, Content, ListingId, IdentityUserId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                await _commentsService.Add(comment);
            }
            var listing = await _listingsService.GetById(comment.ListingId);
            return View("Details", listing);
        }

        // GET: My Listings
        public async Task<IActionResult> MyListings(int? pageNumber)
        {
            var applicationDbContext = _listingsService.GetAll();
            int pageSize = 3;
            
            return View("Index", await PaginatedList<Listing>.CreateAsync(applicationDbContext.Where(l => l.IdentityUserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: My Bids
        public async Task<IActionResult> MyBids(int? pageNumber)
        {
            var applicationDbContext = _bidsService.GetAll();
            int pageSize = 3;

            return View(await PaginatedList<Bid>.CreateAsync(applicationDbContext.Where(l => l.IdentityUserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Listings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var listing = await _listingsService.GetById(id);
            if (listing == null || listing.IdentityUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Unauthorized();

            var viewModel = new ListingVM
            {
                Id = listing.Id,
                Title = listing.Title,
                Description = listing.Description,
                Price = listing.Price,
                IdentityUserId = listing.IdentityUserId,
                IsSold = listing.IsSold
            };
            return View(viewModel);
        }


        // POST: Listings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ListingVM listing)
        {
            if (id != listing.Id) return NotFound();

            var existingListing = await _listingsService.GetById(id);
            if (existingListing == null || existingListing.IdentityUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Unauthorized();

            existingListing.Title = listing.Title;
            existingListing.Description = listing.Description;
            existingListing.Price = listing.Price;

            await _listingsService.Update(existingListing);
            return RedirectToAction(nameof(Index));
        }


        // GET: Listings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var listing = await _listingsService.GetById(id);
            if (listing == null || listing.IdentityUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Unauthorized();

            return View(listing);
        }

        // POST: Listings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listing = await _listingsService.GetById(id);
            if (listing == null || listing.IdentityUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Unauthorized();

            await _listingsService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
