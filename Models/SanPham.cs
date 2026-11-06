using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Quản_Lý_KhoHàng.Models;

[Index("MaDanhMuc", Name = "IX_SanPham_MaDanhMuc")]
[Index("MaHang", Name = "IX_SanPham_MaHang")]
public partial class SanPham
{
    [Key]
    public int MaSanPham { get; set; }

    [StringLength(200)]
    public string TenSanPham { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal GiaBan { get; set; }

    public int SoLuongTon { get; set; }

    public int TonKhoToiThieu { get; set; }

    [StringLength(2000)]
    public string? MoTa { get; set; }

    public int MaDanhMuc { get; set; }

    public int MaHang { get; set; }

    [InverseProperty("MaSanPhamNavigation")]
    public virtual ICollection<LichSuNhapXuat> LichSuNhapXuat { get; set; } = new List<LichSuNhapXuat>();

    [ForeignKey("MaDanhMuc")]
    [InverseProperty("SanPham")]
    public virtual DanhMuc MaDanhMucNavigation { get; set; } = null!;

    [ForeignKey("MaHang")]
    [InverseProperty("SanPham")]
    public virtual HangSanXuat MaHangNavigation { get; set; } = null!;
}
