
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quản_Lý_KhoHàng.Models;
using Quản_Lý_KhoHàng.Data;

namespace Quản_Lý_KhoHàng.Controllers;

public class DanhMucController : Controller
{
    private readonly CoSoDuLieuKho _context;

    public DanhMucController(CoSoDuLieuKho context)
    {
        _context = context;
    }

    // GET: DANHMUCS
    public async Task<IActionResult> Index(string? searchString)
        => await Search(searchString);

    [HttpGet]
    public async Task<IActionResult> Search(string? searchString)
    {
        ViewData["CurrentFilter"] = searchString;
        var query = _context.DanhMuc.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(searchString))
            query = query.Where(x => x.TenDanhMuc.Contains(searchString) || (x.MoTa != null && x.MoTa.Contains(searchString)) || x.MaDanhMuc.ToString() == searchString);
        return View("Search", await query.OrderBy(x => x.TenDanhMuc).ToListAsync());
    }

    // GET: DANHMUCS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var danhmuc = await _context.DanhMuc
            .FirstOrDefaultAsync(m => m.MaDanhMuc == id);
        if (danhmuc == null)
        {
            return NotFound();
        }

        return View(danhmuc);
    }

    // GET: DANHMUCS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: DANHMUCS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TenDanhMuc,MoTa")] DanhMuc danhmuc)
    {
        if (string.IsNullOrWhiteSpace(danhmuc.TenDanhMuc))
        {
            ModelState.AddModelError(
                 nameof(DanhMuc.TenDanhMuc),
                 "Vui lòng nhập tên danh mục.");
        }
        else if (danhmuc.TenDanhMuc.Length > 150)
        {
            ModelState.AddModelError(nameof(DanhMuc.TenDanhMuc), "Tên danh mục tối đa 150 ký tự.");
        }
        else
        {
            var daTonTai = await _context.DanhMuc
                .AnyAsync(d => d.TenDanhMuc == danhmuc.TenDanhMuc);

            if (daTonTai)
            {
                ModelState.AddModelError(
                    nameof(DanhMuc.TenDanhMuc),
                    "Tên danh mục đã tồn tại.");
            }
        }

        if (ModelState.IsValid)
        {
            _context.Add(danhmuc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(danhmuc);
    }

    // GET: DANHMUCS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var danhmuc = await _context.DanhMuc.FindAsync(id);
        if (danhmuc == null)
        {
            return NotFound();
        }
        return View(danhmuc);
    }

    // POST: DANHMUCS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("MaDanhMuc,TenDanhMuc,MoTa")] DanhMuc danhmuc)
    {
        if (id != danhmuc.MaDanhMuc)
        {
            return NotFound();
        }
        if (string.IsNullOrWhiteSpace(danhmuc.TenDanhMuc))
            ModelState.AddModelError(nameof(DanhMuc.TenDanhMuc), "Vui lòng nhập tên danh mục.");
        else if (danhmuc.TenDanhMuc.Length > 150)
            ModelState.AddModelError(nameof(DanhMuc.TenDanhMuc), "Tên danh mục tối đa 150 ký tự.");

        var daTonTai = !string.IsNullOrWhiteSpace(danhmuc.TenDanhMuc) && await _context.DanhMuc.AnyAsync(d =>
            d.TenDanhMuc == danhmuc.TenDanhMuc && d.MaDanhMuc != danhmuc.MaDanhMuc);

        if (daTonTai)
        {
            ModelState.AddModelError(
                nameof(DanhMuc.TenDanhMuc),
                "Tên danh mục đã tồn tại.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(danhmuc);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await DanhMucExists(danhmuc.MaDanhMuc))
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
        return View(danhmuc);
    }

    // GET: DANHMUCS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var danhmuc = await _context.DanhMuc
            .FirstOrDefaultAsync(m => m.MaDanhMuc == id);
        if (danhmuc == null)
        {
            return NotFound();
        }

        return View(danhmuc);
    }

    // POST: DANHMUCS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var danhmuc = await _context.DanhMuc.FindAsync(id);
        if (danhmuc == null)
        {
            return NotFound();
        }
        var coSanPham = await _context.SanPham
         .AnyAsync(s => s.MaDanhMuc == id);

        if (coSanPham)
        {
            ModelState.AddModelError(
                "",
                "Không thể xóa danh mục đang có sản phẩm.");

            return View("Delete", danhmuc);
        }
        try
        {
            _context.DanhMuc.Remove(danhmuc);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is Microsoft.Data.SqlClient.SqlException
            { Number: 547 })
        {
            ModelState.AddModelError(
                "",
                "Không thể xóa danh mục vì đang có dữ liệu liên kết.");

            return View("Delete", danhmuc);
        }
     
    }

    private Task<bool> DanhMucExists(int id)
    {
        return _context.DanhMuc.AnyAsync(e => e.MaDanhMuc == id);
    }
}
