using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Quản_Lý_KhoHàng.Data;
using Quản_Lý_KhoHàng.Models;

namespace Quản_Lý_KhoHàng.Controllers;

public class SanPhamController : Controller
{
    private readonly CoSoDuLieuKho _context;
    public SanPhamController(CoSoDuLieuKho context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.SanPham.Include(x => x.MaDanhMucNavigation).Include(x => x.MaHangNavigation).AsNoTracking().ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var item = await _context.SanPham.Include(x => x.MaDanhMucNavigation).Include(x => x.MaHangNavigation).AsNoTracking().FirstOrDefaultAsync(x => x.MaSanPham == id);
        return item == null ? NotFound() : View(item);
    }

    public async Task<IActionResult> Create() { await LoadLookups(); return View(); }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TenSanPham,GiaBan,SoLuongTon,TonKhoToiThieu,MoTa,MaDanhMuc,MaHang")] SanPham item)
    {
        Validate(item);
        if (ModelState.IsValid)
        {
            _context.SanPham.Add(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        await LoadLookups(item.MaDanhMuc, item.MaHang);
        return View(item);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var item = await _context.SanPham.FindAsync(id);
        if (item == null) return NotFound();
        await LoadLookups(item.MaDanhMuc, item.MaHang);
        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("MaSanPham,TenSanPham,GiaBan,SoLuongTon,TonKhoToiThieu,MoTa,MaDanhMuc,MaHang")] SanPham item)
    {
        if (id != item.MaSanPham) return NotFound();
        Validate(item);
        if (ModelState.IsValid)
        {
            try { _context.Update(item); await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.SanPham.AnyAsync(x => x.MaSanPham == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        await LoadLookups(item.MaDanhMuc, item.MaHang);
        return View(item);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var item = await _context.SanPham.Include(x => x.MaDanhMucNavigation).Include(x => x.MaHangNavigation).AsNoTracking().FirstOrDefaultAsync(x => x.MaSanPham == id);
        return item == null ? NotFound() : View(item);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _context.SanPham.FindAsync(id);
        if (item == null) return NotFound();
        if (await _context.LichSuNhapXuat.AnyAsync(x => x.MaSanPham == id))
        {
            ModelState.AddModelError(string.Empty, "Không thể xóa sản phẩm đã có lịch sử nhập xuất.");
            await _context.Entry(item).Reference(x => x.MaDanhMucNavigation).LoadAsync();
            await _context.Entry(item).Reference(x => x.MaHangNavigation).LoadAsync();
            return View("Delete", item);
        }
        _context.SanPham.Remove(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private void Validate(SanPham item)
    {
        if (string.IsNullOrWhiteSpace(item.TenSanPham)) ModelState.AddModelError(nameof(item.TenSanPham), "Vui lòng nhập tên sản phẩm.");
        else if (item.TenSanPham.Length > 200) ModelState.AddModelError(nameof(item.TenSanPham), "Tên sản phẩm tối đa 200 ký tự.");
        if (item.GiaBan < 0) ModelState.AddModelError(nameof(item.GiaBan), "Giá bán không được âm.");
        if (item.SoLuongTon < 0) ModelState.AddModelError(nameof(item.SoLuongTon), "Số lượng tồn không được âm.");
        if (item.TonKhoToiThieu < 0) ModelState.AddModelError(nameof(item.TonKhoToiThieu), "Tồn kho tối thiểu không được âm.");
        if (item.MaDanhMuc <= 0 || !_context.DanhMuc.Any(x => x.MaDanhMuc == item.MaDanhMuc)) ModelState.AddModelError(nameof(item.MaDanhMuc), "Vui lòng chọn danh mục hợp lệ.");
        if (item.MaHang <= 0 || !_context.HangSanXuat.Any(x => x.MaHang == item.MaHang)) ModelState.AddModelError(nameof(item.MaHang), "Vui lòng chọn hãng hợp lệ.");
        if (item.MoTa?.Length > 2000) ModelState.AddModelError(nameof(item.MoTa), "Mô tả tối đa 2000 ký tự.");
    }

    private async Task LoadLookups(int? danhMucId = null, int? hangId = null)
    {
        ViewData["MaDanhMuc"] = new SelectList(await _context.DanhMuc.AsNoTracking().OrderBy(x => x.TenDanhMuc).ToListAsync(), "MaDanhMuc", "TenDanhMuc", danhMucId);
        ViewData["MaHang"] = new SelectList(await _context.HangSanXuat.AsNoTracking().OrderBy(x => x.TenHang).ToListAsync(), "MaHang", "TenHang", hangId);
    }
}
