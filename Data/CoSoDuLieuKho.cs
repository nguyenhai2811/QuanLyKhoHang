using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Quản_Lý_KhoHàng.Models;

namespace Quản_Lý_KhoHàng.Data;

public partial class CoSoDuLieuKho : DbContext
{
    public CoSoDuLieuKho(DbContextOptions<CoSoDuLieuKho> options)
        : base(options)
    {
    }

    public virtual DbSet<DanhMuc> DanhMuc { get; set; }

    public virtual DbSet<HangSanXuat> HangSanXuat { get; set; }

    public virtual DbSet<LichSuNhapXuat> LichSuNhapXuat { get; set; }

    public virtual DbSet<NhaCungCap> NhaCungCap { get; set; }

    public virtual DbSet<SanPham> SanPham { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LichSuNhapXuat>(entity =>
        {
            entity.Property(e => e.LoaiGiaoDich).UseCollation("Latin1_General_100_BIN2");
            entity.Property(e => e.NgayGiaoDich).HasDefaultValueSql("(sysutcdatetime())", "DF_LichSuNhapXuat_Ngay");

            entity.HasOne(d => d.MaNhaCungCapNavigation).WithMany(p => p.LichSuNhapXuat).HasConstraintName("FK_LichSuNhapXuat_NhaCungCap");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.LichSuNhapXuat)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LichSuNhapXuat_SanPham");
        });

        modelBuilder.Entity<NhaCungCap>(entity =>
        {
            entity.HasIndex(e => e.Email, "UQ_NhaCungCap_Email")
                .IsUnique()
                .HasFilter("([Email] IS NOT NULL)");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasOne(d => d.MaDanhMucNavigation).WithMany(p => p.SanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SanPham_DanhMuc");

            entity.HasOne(d => d.MaHangNavigation).WithMany(p => p.SanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SanPham_HangSanXuat");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
