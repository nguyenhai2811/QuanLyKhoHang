using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Quản_Lý_KhoHàng.Models;

[Index("SoDienThoai", Name = "UQ_NhaCungCap_SoDienThoai", IsUnique = true)]
public partial class NhaCungCap
{
    [Key]
    public int MaNhaCungCap { get; set; }

    [StringLength(200)]
    public string TenNhaCungCap { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string SoDienThoai { get; set; } = null!;

    [StringLength(254)]
    public string? Email { get; set; }

    [StringLength(500)]
    public string DiaChi { get; set; } = null!;

    [InverseProperty("MaNhaCungCapNavigation")]
    public virtual ICollection<LichSuNhapXuat> LichSuNhapXuat { get; set; } = new List<LichSuNhapXuat>();
}
