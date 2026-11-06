
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quản_Lý_KhoHàng.Models;
using Quản_Lý_KhoHàng.Data;

public class HangSanXuatController : Controller
{
    private readonly CoSoDuLieuKho _context;

    public HangSanXuatController(CoSoDuLieuKho context)
    {
        _context = context;
    }

    // GET: HANGSANXUATS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.HangSanXuat.ToListAsync());
    }

    // GET: HANGSANXUATS/Details/5
    public async Task<IActionResult> Details(int? mahang)
    {
        if (mahang == null)
        {
            return NotFound();
        }

        var hangsanxuat = await _context.HangSanXuat
            .FirstOrDefaultAsync(m => m.MaHang == mahang);
        if (hangsanxuat == null)
        {
            return NotFound();
        }

        return View(hangsanxuat);
    }

    // GET: HANGSANXUATS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: HANGSANXUATS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MaHang,TenHang,QuocGia,SanPham")] HangSanXuat hangsanxuat)
    {
        if (ModelState.IsValid)
        {
            _context.Add(hangsanxuat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(hangsanxuat);
    }

    // GET: HANGSANXUATS/Edit/5
    public async Task<IActionResult> Edit(int? mahang)
    {
        if (mahang == null)
        {
            return NotFound();
        }

        var hangsanxuat = await _context.HangSanXuat.FindAsync(mahang);
        if (hangsanxuat == null)
        {
            return NotFound();
        }
        return View(hangsanxuat);
    }

    // POST: HANGSANXUATS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? mahang, [Bind("MaHang,TenHang,QuocGia,SanPham")] HangSanXuat hangsanxuat)
    {
        if (mahang != hangsanxuat.MaHang)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(hangsanxuat);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HangSanXuatExists(hangsanxuat.MaHang))
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
        return View(hangsanxuat);
    }

    // GET: HANGSANXUATS/Delete/5
    public async Task<IActionResult> Delete(int? mahang)
    {
        if (mahang == null)
        {
            return NotFound();
        }

        var hangsanxuat = await _context.HangSanXuat
            .FirstOrDefaultAsync(m => m.MaHang == mahang);
        if (hangsanxuat == null)
        {
            return NotFound();
        }

        return View(hangsanxuat);
    }

    // POST: HANGSANXUATS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? mahang)
    {
        var hangsanxuat = await _context.HangSanXuat.FindAsync(mahang);
        if (hangsanxuat != null)
        {
            _context.HangSanXuat.Remove(hangsanxuat);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool HangSanXuatExists(int? mahang)
    {
        return _context.HangSanXuat.Any(e => e.MaHang == mahang);
    }
}
