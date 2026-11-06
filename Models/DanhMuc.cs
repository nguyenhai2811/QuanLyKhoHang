using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Quản_Lý_KhoHàng.Models;

[Index("TenDanhMuc", Name = "UQ_DanhMuc_TenDanhMuc", IsUnique = true)]
public partial class DanhMuc
{
    [Key]
    public int MaDanhMuc { get; set; }

    [StringLength(150)]
    public string TenDanhMuc { get; set; } = null!;

    [StringLength(1000)]
    public string? MoTa { get; set; }

    [InverseProperty("MaDanhMucNavigation")]
    public virtual ICollection<SanPham> SanPham { get; set; } = new List<SanPham>();
}
