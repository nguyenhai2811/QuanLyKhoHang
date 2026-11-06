using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quản_Lý_KhoHàng.Data;
using Quản_Lý_KhoHàng.Models;

namespace Quản_Lý_KhoHàng.Controllers;

public class HangSanXuatController : Controller
{
    private readonly CoSoDuLieuKho _context;

    public HangSanXuatController(CoSoDuLieuKho context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.HangSanXuat.AsNoTracking().ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var item = await _context.HangSanXuat.AsNoTracking().FirstOrDefaultAsync(x => x.MaHang == id);
        return item == null ? NotFound() : View(item);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TenHang,QuocGia")] HangSanXuat item)
    {
        ValidateRequired(item);
        if (!ModelState.IsValid) return View(item);
        _context.HangSanXuat.Add(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var item = await _context.HangSanXuat.FindAsync(id);
        return item == null ? NotFound() : View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("MaHang,TenHang,QuocGia")] HangSanXuat item)
    {
        if (id != item.MaHang) return NotFound();
        ValidateRequired(item);
        if (!ModelState.IsValid) return View(item);
        try { _context.Update(item); await _context.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.HangSanXuat.AnyAsync(x => x.MaHang == id)) return NotFound();
            throw;
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var item = await _context.HangSanXuat.AsNoTracking().FirstOrDefaultAsync(x => x.MaHang == id);
        return item == null ? NotFound() : View(item);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _context.HangSanXuat.FindAsync(id);
        if (item == null) return NotFound();
        if (await _context.SanPham.AnyAsync(x => x.MaHang == id))
        {
            ModelState.AddModelError(string.Empty, "Không thể xóa hãng đang có sản phẩm.");
            return View("Delete", item);
        }
        _context.HangSanXuat.Remove(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private void ValidateRequired(HangSanXuat item)
    {
        if (string.IsNullOrWhiteSpace(item.TenHang)) ModelState.AddModelError(nameof(item.TenHang), "Vui lòng nhập tên hãng.");
        else if (item.TenHang.Length > 150) ModelState.AddModelError(nameof(item.TenHang), "Tên hãng tối đa 150 ký tự.");
        if (string.IsNullOrWhiteSpace(item.QuocGia)) ModelState.AddModelError(nameof(item.QuocGia), "Vui lòng nhập quốc gia.");
        else if (item.QuocGia.Length > 100) ModelState.AddModelError(nameof(item.QuocGia), "Quốc gia tối đa 100 ký tự.");
        if (!string.IsNullOrWhiteSpace(item.TenHang) && _context.HangSanXuat.Any(x => x.TenHang == item.TenHang && x.MaHang != item.MaHang))
            ModelState.AddModelError(nameof(item.TenHang), "Tên hãng đã tồn tại.");
    }
}
