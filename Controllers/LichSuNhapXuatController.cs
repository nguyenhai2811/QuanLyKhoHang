
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quản_Lý_KhoHàng.Models;
using Quản_Lý_KhoHàng.Data;

public class LichSuNhapXuatController : Controller
{
    private readonly CoSoDuLieuKho _context;

    public LichSuNhapXuatController(CoSoDuLieuKho context)
    {
        _context = context;
    }

    // GET: LICHSUNHAPXUATS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.LichSuNhapXuat.ToListAsync());
    }

    // GET: LICHSUNHAPXUATS/Details/5
    public async Task<IActionResult> Details(long? magiaodich)
    {
        if (magiaodich == null)
        {
            return NotFound();
        }

        var lichsunhapxuat = await _context.LichSuNhapXuat
            .FirstOrDefaultAsync(m => m.MaGiaoDich == magiaodich);
        if (lichsunhapxuat == null)
        {
            return NotFound();
        }

        return View(lichsunhapxuat);
    }

    // GET: LICHSUNHAPXUATS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: LICHSUNHAPXUATS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MaGiaoDich,MaSanPham,MaNhaCungCap,LoaiGiaoDich,SoLuong,NgayGiaoDich,GhiChu,MaNhaCungCapNavigation,MaSanPhamNavigation")] LichSuNhapXuat lichsunhapxuat)
    {
        if (ModelState.IsValid)
        {
            _context.Add(lichsunhapxuat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(lichsunhapxuat);
    }

    // GET: LICHSUNHAPXUATS/Edit/5
    public async Task<IActionResult> Edit(long? magiaodich)
    {
        if (magiaodich == null)
        {
            return NotFound();
        }

        var lichsunhapxuat = await _context.LichSuNhapXuat.FindAsync(magiaodich);
        if (lichsunhapxuat == null)
        {
            return NotFound();
        }
        return View(lichsunhapxuat);
    }

    // POST: LICHSUNHAPXUATS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long? magiaodich, [Bind("MaGiaoDich,MaSanPham,MaNhaCungCap,LoaiGiaoDich,SoLuong,NgayGiaoDich,GhiChu,MaNhaCungCapNavigation,MaSanPhamNavigation")] LichSuNhapXuat lichsunhapxuat)
    {
        if (magiaodich != lichsunhapxuat.MaGiaoDich)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(lichsunhapxuat);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LichSuNhapXuatExists(lichsunhapxuat.MaGiaoDich))
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
        return View(lichsunhapxuat);
    }

    // GET: LICHSUNHAPXUATS/Delete/5
    public async Task<IActionResult> Delete(long? magiaodich)
    {
        if (magiaodich == null)
        {
            return NotFound();
        }

        var lichsunhapxuat = await _context.LichSuNhapXuat
            .FirstOrDefaultAsync(m => m.MaGiaoDich == magiaodich);
        if (lichsunhapxuat == null)
        {
            return NotFound();
        }

        return View(lichsunhapxuat);
    }

    // POST: LICHSUNHAPXUATS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long? magiaodich)
    {
        var lichsunhapxuat = await _context.LichSuNhapXuat.FindAsync(magiaodich);
        if (lichsunhapxuat != null)
        {
            _context.LichSuNhapXuat.Remove(lichsunhapxuat);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool LichSuNhapXuatExists(long? magiaodich)
    {
        return _context.LichSuNhapXuat.Any(e => e.MaGiaoDich == magiaodich);
    }
}
