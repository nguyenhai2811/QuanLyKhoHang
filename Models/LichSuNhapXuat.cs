using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Quản_Lý_KhoHàng.Models;

[Index("MaNhaCungCap", Name = "IX_LichSuNhapXuat_MaNhaCungCap")]
[Index("MaSanPham", "NgayGiaoDich", Name = "IX_LichSuNhapXuat_SanPham_Ngay")]
public partial class LichSuNhapXuat
{
    [Key]
    public long MaGiaoDich { get; set; }

    public int MaSanPham { get; set; }

    public int? MaNhaCungCap { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string LoaiGiaoDich { get; set; } = null!;

    public int SoLuong { get; set; }

    [Precision(0)]
    public DateTime NgayGiaoDich { get; set; }

    [StringLength(1000)]
    public string? GhiChu { get; set; }

    [ForeignKey("MaNhaCungCap")]
    [InverseProperty("LichSuNhapXuat")]
    public virtual NhaCungCap? MaNhaCungCapNavigation { get; set; }

    [ForeignKey("MaSanPham")]
    [InverseProperty("LichSuNhapXuat")]
    public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
}
