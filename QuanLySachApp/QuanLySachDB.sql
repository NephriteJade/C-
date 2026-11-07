CREATE DATABASE IF NOT EXISTS QuanLySachDB CHARACTER SET utf8mb4;
USE QuanLySachDB;

-- 1. Tạo bảng TheLoai
CREATE TABLE IF NOT EXISTS TheLoai (
    MaLoai VARCHAR(10) PRIMARY KEY,
    TenLoai VARCHAR(100) NOT NULL
);

-- 2. Tạo bảng Sach
CREATE TABLE IF NOT EXISTS Sach (
    MaSach VARCHAR(20) PRIMARY KEY,
    TenSach VARCHAR(150) NOT NULL,
    NgayNhap DATETIME NOT NULL,
    SoLuong INT NOT NULL,
    DonGia DECIMAL(18,2) NOT NULL,
    MaLoai VARCHAR(10) NOT NULL,
    FOREIGN KEY (MaLoai) REFERENCES TheLoai(MaLoai)
);

-- 3. Nạp dữ liệu ban đầu
INSERT IGNORE INTO TheLoai VALUES ('TH', 'Toán học'), ('VL', 'Vật lý'), ('HH', 'Hóa học');
INSERT IGNORE INTO Sach VALUES 
('S001', 'Giải tích 1', '2026-10-01', 10, 150000, 'TH'),
('S002', 'Đại số tuyến tính', '2026-10-03', 20, 170000, 'TH'),
('S003', 'Vật lý 1', '2026-10-02', 10, 120000, 'VL'),
('S004', 'Hóa học 1', '2026-10-01', 10, 100000, 'HH'),
('S005', 'Hóa học 2', '2026-10-02', 15, 150000, 'HH');
