using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Quản_Lý_KhoHàng.Data;
using Quản_Lý_KhoHàng.Models;

namespace Quản_Lý_KhoHàng.Controllers;

public class LichSuNhapXuatController : Controller
{
    private readonly CoSoDuLieuKho _context;
    public LichSuNhapXuatController(CoSoDuLieuKho context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.LichSuNhapXuat.Include(x => x.MaSanPhamNavigation).Include(x => x.MaNhaCungCapNavigation).AsNoTracking().OrderByDescending(x => x.NgayGiaoDich).ToListAsync());

    public async Task<IActionResult> Details(long? id)
    {
        if (id == null) return NotFound();
        var item = await _context.LichSuNhapXuat.Include(x => x.MaSanPhamNavigation).Include(x => x.MaNhaCungCapNavigation).AsNoTracking().FirstOrDefaultAsync(x => x.MaGiaoDich == id);
        return item == null ? NotFound() : View(item);
    }

    public async Task<IActionResult> Create() { await LoadLookups(); return View(); }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MaSanPham,MaNhaCungCap,LoaiGiaoDich,SoLuong,NgayGiaoDich,GhiChu")] LichSuNhapXuat item)
    {
        Validate(item);
        if (ModelState.IsValid)
        {
            _context.LichSuNhapXuat.Add(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        await LoadLookups(item.MaSanPham, item.MaNhaCungCap);
        return View(item);
    }

    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null) return NotFound();
        var item = await _context.LichSuNhapXuat.FindAsync(id);
        if (item == null) return NotFound();
        await LoadLookups(item.MaSanPham, item.MaNhaCungCap);
        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, [Bind("MaGiaoDich,MaSanPham,MaNhaCungCap,LoaiGiaoDich,SoLuong,NgayGiaoDich,GhiChu")] LichSuNhapXuat item)
    {
        if (id != item.MaGiaoDich) return NotFound();
        Validate(item);
        if (ModelState.IsValid)
        {
            try { _context.Update(item); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.LichSuNhapXuat.AnyAsync(x => x.MaGiaoDich == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        await LoadLookups(item.MaSanPham, item.MaNhaCungCap);
        return View(item);
    }

    public async Task<IActionResult> Delete(long? id)
    {
        if (id == null) return NotFound();
        var item = await _context.LichSuNhapXuat.Include(x => x.MaSanPhamNavigation).Include(x => x.MaNhaCungCapNavigation).AsNoTracking().FirstOrDefaultAsync(x => x.MaGiaoDich == id);
        return item == null ? NotFound() : View(item);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        var item = await _context.LichSuNhapXuat.FindAsync(id);
        if (item == null) return NotFound();
        _context.LichSuNhapXuat.Remove(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private void Validate(LichSuNhapXuat item)
    {
        if (item.MaSanPham <= 0 || !_context.SanPham.Any(x => x.MaSanPham == item.MaSanPham)) ModelState.AddModelError(nameof(item.MaSanPham), "Vui lòng chọn sản phẩm hợp lệ.");
        if (item.MaNhaCungCap.HasValue && !_context.NhaCungCap.Any(x => x.MaNhaCungCap == item.MaNhaCungCap)) ModelState.AddModelError(nameof(item.MaNhaCungCap), "Nhà cung cấp không hợp lệ.");
        if (item.LoaiGiaoDich is not ("NHAP" or "XUAT")) ModelState.AddModelError(nameof(item.LoaiGiaoDich), "Loại giao dịch phải là NHAP hoặc XUAT.");
        if (item.SoLuong <= 0) ModelState.AddModelError(nameof(item.SoLuong), "Số lượng phải lớn hơn 0.");
        if (item.GhiChu?.Length > 1000) ModelState.AddModelError(nameof(item.GhiChu), "Ghi chú tối đa 1000 ký tự.");
    }

    private async Task LoadLookups(int? sanPhamId = null, int? nhaCungCapId = null)
    {
        var products = await _context.SanPham.AsNoTracking().OrderBy(x => x.TenSanPham).ToListAsync();
        var suppliers = await _context.NhaCungCap.AsNoTracking().OrderBy(x => x.TenNhaCungCap).ToListAsync();
        ViewData["MaSanPham"] = new SelectList(products, "MaSanPham", "TenSanPham", sanPhamId);
        ViewData["MaNhaCungCap"] = new SelectList(suppliers, "MaNhaCungCap", "TenNhaCungCap", nhaCungCapId);
    }
}
