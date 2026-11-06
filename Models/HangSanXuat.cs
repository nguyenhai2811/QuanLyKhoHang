using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Quản_Lý_KhoHàng.Models;

[Index("TenHang", Name = "UQ_HangSanXuat_TenHang", IsUnique = true)]
public partial class HangSanXuat
{
    [Key]
    public int MaHang { get; set; }

    [StringLength(150)]
    public string TenHang { get; set; } = null!;

    [StringLength(100)]
    public string QuocGia { get; set; } = null!;

    [InverseProperty("MaHangNavigation")]
    public virtual ICollection<SanPham> SanPham { get; set; } = new List<SanPham>();
}
