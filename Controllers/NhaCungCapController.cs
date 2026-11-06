using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quản_Lý_KhoHàng.Data;
using Quản_Lý_KhoHàng.Models;

namespace Quản_Lý_KhoHàng.Controllers;

public class NhaCungCapController : Controller
{
    private readonly CoSoDuLieuKho _context;
    public NhaCungCapController(CoSoDuLieuKho context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.NhaCungCap.AsNoTracking().ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var item = await _context.NhaCungCap.AsNoTracking().FirstOrDefaultAsync(x => x.MaNhaCungCap == id);
        return item == null ? NotFound() : View(item);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TenNhaCungCap,SoDienThoai,Email,DiaChi")] NhaCungCap item)
    {
        Validate(item);
        if (!ModelState.IsValid) return View(item);
        _context.NhaCungCap.Add(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var item = await _context.NhaCungCap.FindAsync(id);
        return item == null ? NotFound() : View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("MaNhaCungCap,TenNhaCungCap,SoDienThoai,Email,DiaChi")] NhaCungCap item)
    {
        if (id != item.MaNhaCungCap) return NotFound();
        Validate(item);
        if (!ModelState.IsValid) return View(item);
        try { _context.Update(item); await _context.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.NhaCungCap.AnyAsync(x => x.MaNhaCungCap == id)) return NotFound();
            throw;
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var item = await _context.NhaCungCap.AsNoTracking().FirstOrDefaultAsync(x => x.MaNhaCungCap == id);
        return item == null ? NotFound() : View(item);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _context.NhaCungCap.FindAsync(id);
        if (item == null) return NotFound();
        if (await _context.LichSuNhapXuat.AnyAsync(x => x.MaNhaCungCap == id))
        {
            ModelState.AddModelError(string.Empty, "Không thể xóa nhà cung cấp đang được dùng trong lịch sử giao dịch.");
            return View("Delete", item);
        }
        _context.NhaCungCap.Remove(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private void Validate(NhaCungCap item)
    {
        if (string.IsNullOrWhiteSpace(item.TenNhaCungCap)) ModelState.AddModelError(nameof(item.TenNhaCungCap), "Vui lòng nhập tên nhà cung cấp.");
        else if (item.TenNhaCungCap.Length > 200) ModelState.AddModelError(nameof(item.TenNhaCungCap), "Tên nhà cung cấp tối đa 200 ký tự.");
        if (string.IsNullOrWhiteSpace(item.SoDienThoai)) ModelState.AddModelError(nameof(item.SoDienThoai), "Vui lòng nhập số điện thoại.");
        else if (item.SoDienThoai.Length > 20) ModelState.AddModelError(nameof(item.SoDienThoai), "Số điện thoại tối đa 20 ký tự.");
        if (item.Email?.Length > 254) ModelState.AddModelError(nameof(item.Email), "Email tối đa 254 ký tự.");
        if (string.IsNullOrWhiteSpace(item.DiaChi)) ModelState.AddModelError(nameof(item.DiaChi), "Vui lòng nhập địa chỉ.");
        else if (item.DiaChi.Length > 500) ModelState.AddModelError(nameof(item.DiaChi), "Địa chỉ tối đa 500 ký tự.");
        if (!string.IsNullOrWhiteSpace(item.SoDienThoai) && _context.NhaCungCap.Any(x => x.SoDienThoai == item.SoDienThoai && x.MaNhaCungCap != item.MaNhaCungCap))
            ModelState.AddModelError(nameof(item.SoDienThoai), "Số điện thoại đã được sử dụng.");
        if (!string.IsNullOrWhiteSpace(item.Email) && _context.NhaCungCap.Any(x => x.Email == item.Email && x.MaNhaCungCap != item.MaNhaCungCap))
            ModelState.AddModelError(nameof(item.Email), "Email đã được sử dụng.");
    }
}
