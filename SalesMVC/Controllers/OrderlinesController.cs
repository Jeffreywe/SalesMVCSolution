using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesMVC.Data;
using SalesMVC.Models;

namespace SalesMVC.Controllers
{
    public class OrderlinesController : Controller
    {
        private readonly SalesMVCContext _context;

        public OrderlinesController(SalesMVCContext context)
        {
            _context = context;
        }

        // Recalculate ordertotal
        private async Task<IActionResult> RecalculateOrderTotal(int orderId) {
            var order = _context.Orders.Find(orderId);

            order.Total = (from ol in _context.Orderlines
                           join o in _context.Orders
                           on ol.OrderId equals o.Id
                           where ol.OrderId == orderId
                           select new {
                               LineTotal = ol.Quantity * ol.Price
                           }).Sum(x => x.LineTotal);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // GET: Orderlines
        public async Task<IActionResult> Index()
        {
            var salesMVCContext = _context.Orderlines.Include(o => o.Order);
            return View(await salesMVCContext.ToListAsync());
        }

        // GET: Orderlines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderline = await _context.Orderlines
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderline == null)
            {
                return NotFound();
            }

            return View(orderline);
        }

        // GET: Orderlines/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Description");
            return View();
        }

        // POST: Orderlines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Product,Quantity,Price,LineTotal,OrderId")] Orderline orderline)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderline);
                await _context.SaveChangesAsync();
                await RecalculateOrderTotal(orderline.OrderId);//recalc
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Description", orderline.OrderId);
            return View(orderline);
        }

        // GET: Orderlines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderline = await _context.Orderlines.FindAsync(id);
            if (orderline == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Description", orderline.OrderId);
            return View(orderline);
        }

        // POST: Orderlines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Product,Quantity,Price,LineTotal,OrderId")] Orderline orderline)
        {
            if (id != orderline.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderline);
                    await _context.SaveChangesAsync();
                    await RecalculateOrderTotal(orderline.OrderId);//recalc
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderlineExists(orderline.Id))
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Description", orderline.OrderId);
            return View(orderline);
        }

        // GET: Orderlines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderline = await _context.Orderlines
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderline == null)
            {
                return NotFound();
            }

            return View(orderline);
        }

        // POST: Orderlines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderline = await _context.Orderlines.FindAsync(id);
            _context.Orderlines.Remove(orderline);
            await _context.SaveChangesAsync();
            await RecalculateOrderTotal(orderline.OrderId);//recalc
            return RedirectToAction(nameof(Index));
        }

        private bool OrderlineExists(int id)
        {
            return _context.Orderlines.Any(e => e.Id == id);
        }
    }
}
