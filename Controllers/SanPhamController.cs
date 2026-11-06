
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quản_Lý_KhoHàng.Models;
using Quản_Lý_KhoHàng.Data;

public class SanPhamController : Controller
{
    private readonly CoSoDuLieuKho _context;

    public SanPhamController(CoSoDuLieuKho context)
    {
        _context = context;
    }

    // GET: SANPHAMS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.SanPham.ToListAsync());
    }

    // GET: SANPHAMS/Details/5
    public async Task<IActionResult> Details(int? masanpham)
    {
        if (masanpham == null)
        {
            return NotFound();
        }

        var sanpham = await _context.SanPham
            .FirstOrDefaultAsync(m => m.MaSanPham == masanpham);
        if (sanpham == null)
        {
            return NotFound();
        }

        return View(sanpham);
    }

    // GET: SANPHAMS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: SANPHAMS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MaSanPham,TenSanPham,GiaBan,SoLuongTon,TonKhoToiThieu,MoTa,MaDanhMuc,MaHang,LichSuNhapXuat,MaDanhMucNavigation,MaHangNavigation")] SanPham sanpham)
    {
        if (ModelState.IsValid)
        {
            _context.Add(sanpham);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(sanpham);
    }

    // GET: SANPHAMS/Edit/5
    public async Task<IActionResult> Edit(int? masanpham)
    {
        if (masanpham == null)
        {
            return NotFound();
        }

        var sanpham = await _context.SanPham.FindAsync(masanpham);
        if (sanpham == null)
        {
            return NotFound();
        }
        return View(sanpham);
    }

    // POST: SANPHAMS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? masanpham, [Bind("MaSanPham,TenSanPham,GiaBan,SoLuongTon,TonKhoToiThieu,MoTa,MaDanhMuc,MaHang,LichSuNhapXuat,MaDanhMucNavigation,MaHangNavigation")] SanPham sanpham)
    {
        if (masanpham != sanpham.MaSanPham)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(sanpham);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SanPhamExists(sanpham.MaSanPham))
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
        return View(sanpham);
    }

    // GET: SANPHAMS/Delete/5
    public async Task<IActionResult> Delete(int? masanpham)
    {
        if (masanpham == null)
        {
            return NotFound();
        }

        var sanpham = await _context.SanPham
            .FirstOrDefaultAsync(m => m.MaSanPham == masanpham);
        if (sanpham == null)
        {
            return NotFound();
        }

        return View(sanpham);
    }

    // POST: SANPHAMS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? masanpham)
    {
        var sanpham = await _context.SanPham.FindAsync(masanpham);
        if (sanpham != null)
        {
            _context.SanPham.Remove(sanpham);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SanPhamExists(int? masanpham)
    {
        return _context.SanPham.Any(e => e.MaSanPham == masanpham);
    }
}
