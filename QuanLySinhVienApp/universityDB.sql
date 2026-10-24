CREATE DATABASE universityDB;
USE universityDB;
CREATE TABLE Departments (
	dept_id INT PRIMARY KEY AUTO_INCREMENT,
    dept_code VARCHAR(50) NOT NULL UNIQUE,
    dept_name VARCHAR(255) NOT NULL,
    location VARCHAR(100),
    phone VARCHAR(20) CHECK (phone REGEXP '^\\+[0-9]+$' OR phone IS NULL),
    established INT CHECK (established BETWEEN 1900 AND 2100),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
);
CREATE TABLE Instructors (
	instructor_id INT PRIMARY KEY AUTO_INCREMENT,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    dept_id INT NOT NULL,
    hire_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    degree VARCHAR (20) CHECK (degree IN ('Cử nhân', 'Thạc sĩ', 'Tiến sĩ', 'Phó giáo sư', 'Giáo sư')),
    salary REAL CHECK (salary > 0),
    FOREIGN KEY (dept_id) REFERENCES Departments(dept_id) ON DELETE RESTRICT
);
CREATE TABLE Students (
	student_id INT PRIMARY KEY AUTO_INCREMENT,
    student_code VARCHAR(20) NOT NULL UNIQUE,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email VARCHAR(50) NOT NULL UNIQUE,
    dob DATE NOT NULL,
    dept_id INT NOT NULL,
    enrollment_year INT NOT NULL,
    gpa DECIMAL(3,2) DEFAULT 0.0 CHECK (gpa BETWEEN 0.0 AND 4.0),
    status VARCHAR(20) DEFAULT 'active' CHECK (status IN ('active', 'graduated', 'suspended', 'leave')),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (dept_id) REFERENCES Departments(dept_id) ON DELETE RESTRICT
);
CREATE TABLE Rooms (
	room_id INT PRIMARY KEY AUTO_INCREMENT,
    building VARCHAR(50) NOT NULL,
    room_number VARCHAR(100) NOT NULL,
    capacity INT NOT NULL CHECK (capacity > 0),
    room_type VARCHAR(20) DEFAULT 'lecture' CHECK (room_type IN ('lecture', 'lab', 'seminar', 'auditorium')),
    UNIQUE (building, room_number)
);
CREATE TABLE Courses (
	course_id INT PRIMARY KEY AUTO_INCREMENT,
    course_code VARCHAR(50) NOT NULL UNIQUE,
    title VARCHAR(50) NOT NULL,
    credits INT NOT NULL CHECK (credits IN (1,2,3,4,5)),
    dept_id INT NOT NULL,
    description VARCHAR(100),
    is_active INT DEFAULT 1 CHECK (is_active IN (0,1)),
    FOREIGN KEY (dept_id) REFERENCES Departments(dept_id) ON DELETE RESTRICT
);
CREATE TABLE Prerequisites (
	course_id INT NOT NULL,
    prereq_id INT NOT NULL,
    PRIMARY KEY (course_id, prereq_id),
    FOREIGN KEY (course_id) REFERENCES Courses(course_id) ON DELETE CASCADE,
    FOREIGN KEY (prereq_id) REFERENCES Courses(course_id) ON DELETE CASCADE,
    CHECK (course_id <> prereq_id)
);
CREATE TABLE Sections (
	section_id INT PRIMARY KEY AUTO_INCREMENT,
    course_id INT NOT NULL,
    instructor_id INT NOT NULL,
    room_id INT NOT NULL,
    semester VARCHAR(3) NOT NULL CHECK (semester IN ('HK1', 'HK2', 'HK3')),
    academic_year VARCHAR(20) NOT NULL,
    schedule_days TIME NOT NULL,
    start_time TIME NOT NULL,
    end_time VARCHAR(50) NOT NULL,
    max_enrollment INT NOT NULL CHECK (max_enrollment > 0),
    current_count INT DEFAULT 0 CHECK (current_count >= 0),
    CHECK (end_time > start_time),
    CHECK (current_count <= max_enrollment),
    FOREIGN KEY (course_id) REFERENCES Courses(course_id) ON DELETE RESTRICT,
    FOREIGN KEY (instructor_id) REFERENCES Instructors(instructor_id) ON DELETE RESTRICT,
    FOREIGN KEY (room_id) REFERENCES Rooms(room_id) ON DELETE RESTRICT,
    UNIQUE (instructor_id, semester, academic_year, schedule_days, start_time)
);
CREATE TABLE Enrollments (
	enrollment_id INT PRIMARY KEY AUTO_INCREMENT,
    student_id INT NOT NULL,
    section_id INT NOT NULL,
    enroll_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    status VARCHAR(20) DEFAULT 'registered' CHECK (status IN ('registered', 'completed', 'dropped', 'failed')),
    UNIQUE (student_id, section_id),
    FOREIGN KEY (student_id) REFERENCES Students(student_id) ON DELETE CASCADE,
    FOREIGN KEY (section_id) REFERENCES Sections(section_id) ON DELETE RESTRICT
);
CREATE TABLE Grades (
	enrollment_id INT PRIMARY KEY,
    score DECIMAL(10,0) NOT NULL CHECK (score BETWEEN 0 AND 10),
    grade_letter VARCHAR(1) GENERATED ALWAYS AS (
		CASE
			WHEN score >= 8.5 THEN 'A'
            WHEN score >= 7.0 THEN 'B'
            WHEN score >= 5.5 THEN 'C'
            WHEN score >= 4.0 THEN 'D'
            ELSE 'F'
		END
	) STORED,
    grade_points DECIMAL(2,1) GENERATED ALWAYS AS (
		CASE
			WHEN score >= 8.5 THEN 4.0
            WHEN score >= 7.0 THEN 3.0
            WHEN score >= 5.5 THEN 2.0
            WHEN score >= 4.0 THEN 1.0
            ELSE 0.0
		END
	) STORED,
    graded_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    notes VARCHAR(100),
    FOREIGN KEY (enrollment_id) REFERENCES Enrollments(enrollment_id) ON DELETE CASCADE
);
CREATE INDEX idx_students_name ON Students (last_name, first_name);
CREATE INDEX idx_students_dept ON Students (dept_id);
CREATE INDEX idx_sections_course ON Sections(course_id, semester, academic_year);
CREATE INDEX idx_enrollments_student ON Enrollments(student_id);
CREATE INDEX idx_enrollments_section ON Enrollments(section_id);
CREATE INDEX idx_courses_dept ON Courses(dept_id);

DELIMITER //
CREATE TRIGGER trg_enroll_increment
AFTER INSERT ON Enrollments
FOR EACH ROW
BEGIN
	UPDATE Sections
    SET current_count = current_count + 1
    WHERE section_id = NEW.section_id;
END//

CREATE TRIGGER trg_enrollment_decrement
AFTER UPDATE ON Enrollments
FOR EACH ROW
BEGIN
	IF NEW.status = 'dropped' AND OLD.status = 'registered' THEN
		UPDATE Sections
		SET current_count = current_count - 1
		WHERE section_id = NEW.section_id;
	END IF;
END//

CREATE TRIGGER trg_update_gpa
AFTER INSERT ON Grades
FOR EACH ROW
BEGIN
	DECLARE v_student_id INT;
    SELECT student_id INTO v_student_id 
    FROM Enrollments 
    WHERE enrollment_id = NEW.enrollment_id;
	UPDATE Students
    SET gpa = (
		SELECT ROUND(SUM(g.grade_points * c.credits) / SUM(c.credits), 2)
        FROM Grades g
        JOIN Enrollments e ON e.Enrollment_id = g.enrollment_id
        JOIN Sections s ON s.section_id = e.section_id
        JOIN Courses c ON c.course_id = s.course_id
        WHERE e.student_id = v_student_id AND e.status = 'completed'
	)
    WHERE student_id = v_student_id;
END//

CREATE TRIGGER trg_update_enrollment_status
AFTER INSERT ON Grades
FOR EACH ROW
BEGIN
	UPDATE Enrollments
    SET status = CASE
		WHEN NEW.score >= 4.0 THEN 'completed'
        ELSE 'failed'
	END
    WHERE enrollment_id = NEW.enrollment_id;
END//

CREATE TRIGGER trg_check_capacity
BEFORE INSERT ON enrollments
FOR EACH ROW
BEGIN
    DECLARE v_current_count INT;
    DECLARE v_max_enrollment INT;
    SELECT current_count, max_enrollment 
    INTO v_current_count, v_max_enrollment
    FROM Sections
    WHERE section_id = NEW.section_id;
    IF v_current_count >= v_max_enrollment THEN
        SIGNAL SQLSTATE '45000' 
        SET MESSAGE_TEXT = 'Lỗi: Lớp học đã đủ sinh viên (hết chỗ)';
    END IF;
END//
DELIMITER ;

INSERT INTO Departments(dept_code, dept_name, location, phone, established) VALUES
('CS', 'Khoa học Máy tính', 'Tòa A', '+84241234567', 1995),
('SE', 'Kỹ thuật Phần mềm', 'Tòa A', '+84241234568', 2000),
('IS', 'Hệ thống Thông tin', 'Tòa B', '+84241234569', 2005),
('NET', 'Mạng máy tínhh', 'Tòa B', '+84241234570', 2010),
('DS', 'Khoa học Dữ liệu', 'Tòa C', '+84241234571', 2018);
INSERT INTO Instructors(first_name, last_name, email, dept_id, hire_date, degree, salary) VALUES
('An', 'Nguyễn Văn', 'an.vn@uni.edu.vn', 1, '2010-09-01', 'Tiến sĩ', 25000000),
('Bình', 'Trần Thị', 'binh.tt@uni.edu.vn', 1, '2015-09-01', 'Thạc sĩ', 18000000),
('Cường', 'Lê Văn', 'cuong.lv@uni.edu.vn', 2, '2008-09-01', 'Phó Giáo sư', 32000000),
('Dung', 'Phạm Thị', 'dung.pt@uni.edu.vn', 2, '2017-03-01', 'Tiến sĩ', 22000000),
('Em', 'Hoàng Văn', 'em.hv@uni.edu.vn', 3, '2012-09-01', 'Tiến sĩ', 24000000),
('Phương', 'Vũ Thị', 'phuong.vt@uni.edu.vn', 4, '2018-09-01', 'Thạc sĩ', 17000000),
('Giang', 'Đỗ Văn', 'giang.dv@uni.edu.vn', 5, '2020-09-01', 'Tiến sĩ', 21000000),
('Hà', 'Bùi Thị', 'ha.bt@uni.edu.vn', 1, '2019-03-01', 'Giáo sư', 35000000);
INSERT INTO Rooms (building, room_number, capacity, room_type) VALUES
('Tòa A', 'Phòng 101', 50, 'lecture'),
('Tòa A', 'Phòng 102', 50, 'lecture'),
('Tòa B', 'Phòng 201', 30, 'lab'),
('Tòa B', 'Phòng 202', 30, 'lab'),
('Tòa C', 'Phòng 301', 120, 'auditorium'),
('Tòa C', 'Phòng 302', 45, 'lecture'),
('Tòa D', 'Phòng 401', 25, 'seminar'),
('Tòa D', 'Phòng 402', 25, 'seminar'),
('Tòa E', 'Phòng 501', 60, 'lecture'),
('Tòa E', 'Phòng 502', 35, 'lab');
INSERT INTO Courses (course_code, title, credits, dept_id, description) VALUES
('CS101', 'Lập trình C cơ bản', 3, 1, 'Học về cú pháp C và tư duy lập trình'),
('CS102', 'Cấu trúc dữ liệu và Giải thuật', 4, 1, 'Các loại cấu trúc dữ liệu phổ biến'),
('SE201', 'Nhập môn Kỹ thuật phần mềm', 3, 2, 'Quy trình phát triển phần mềm'),
('SE202', 'Thiết kế hướng đối tượng', 3, 2, 'Học về OOP và UML'),
('IS301', 'Cơ sở dữ liệu', 3, 3, 'Thiết kế và truy vấn SQL'),
('IS302', 'Phân tích hệ thống thông tin', 3, 3, 'Quy trình phân tích nghiệp vụ'),
('NET401', 'Mạng máy tính cơ bản', 3, 4, 'Mô hình OSI và giao thức mạng'),
('NET402', 'An toàn bảo mật thông tin', 3, 4, 'Các kỹ thuật mã hóa và bảo vệ hệ thống'),
('DS501', 'Nhập môn Khoa học dữ liệu', 4, 5, 'Xử lý dữ liệu với Python/R'),
('DS502', 'Học máy cơ bản', 4, 5, 'Các thuật toán Machine Learning cơ bản');
INSERT INTO Prerequisites (course_id, prereq_id) VALUES
(2, 1), -- Cấu trúc dữ liệu (2) cần Lập trình C (1)
(4, 3), -- Thiết kế hướng đối tượng (4) cần Nhập môn SE (3)
(6, 5), -- Phân tích hệ thống (6) cần Cơ sở dữ liệu (5)
(8, 7), -- An toàn bảo mật (8) cần Mạng máy tính (7)
(10, 9), -- Học máy (10) cần Khoa học dữ liệu (9)
(9, 1), -- Khoa học dữ liệu (9) cần Lập trình C (1)
(10, 2); -- Học máy (10) cần Cấu trúc dữ liệu (2)
INSERT INTO Students (student_id, student_code, first_name, last_name, email, dob, dept_id, enrollment_year, gpa, status) VALUES
(1, 'SV202301', 'Anh', 'Nguyễn Tuấn', 'anh.nt@uni.edu.vn', '2005-05-15', 1, 2023, 0.0, 'active'),
(2, 'SV202302', 'Bảo', 'Lê Gia', 'bao.lg@uni.edu.vn', '2005-08-20', 1, 2023, 0.0, 'active'),
(3, 'SV202303', 'Chi', 'Phạm Linh', 'chi.pl@uni.edu.vn', '2005-02-10', 2, 2023, 0.0, 'active'),
(4, 'SV202304', 'Dũng', 'Trần Anh', 'dung.ta@uni.edu.vn', '2005-11-30', 2, 2023, 0.0, 'active'),
(5, 'SV202401', 'An', 'Hoàng Ngọc', 'an.hn@uni.edu.vn', '2006-01-25', 3, 2024, 0.0, 'active'),
(6, 'SV202402', 'Hà', 'Vũ Việt', 'ha.vv@uni.edu.vn', '2006-04-12', 3, 2024, 0.0, 'active'),
(7, 'SV202403', 'Linh', 'Đặng Mai', 'linh.dm@uni.edu.vn', '2006-07-18', 4, 2024, 0.0, 'active'),
(8, 'SV202404', 'Nam', 'Bùi Xuân', 'nam.bx@uni.edu.vn', '2006-09-05', 4, 2024, 0.0, 'active'),
(9, 'SV202405', 'Minh', 'Ngô Quang', 'minh.nq@uni.edu.vn', '2006-12-22', 5, 2024, 0.0, 'active'),
(10, 'SV202406', 'Hương', 'Đỗ Thanh', 'huong.dt@uni.edu.vn', '2006-03-08', 5, 2024, 0.0, 'active'),
(11, 'SV202411', 'Tuấn', 'Lý Anh', 'tuan.la@uni.edu.vn', '2006-05-20', 1, 2024, 0.0, 'active'),
(12, 'SV202412', 'Sơn', 'Phan Thanh', 'son.pt@uni.edu.vn', '2006-10-15', 2, 2024, 0.0, 'active'),
(13, 'SV202305', 'Thảo', 'Trương Minh', 'thao.tm@uni.edu.vn', '2005-06-11', 3, 2023, 0.0, 'active'),
(14, 'SV202306', 'Hiền', 'Nguyễn Thu', 'hien.nt@uni.edu.vn', '2005-12-25', 4, 2023, 0.0, 'active'),
(15, 'SV202307', 'Quang', 'Lê Hữu', 'quang.lh@uni.edu.vn', '2005-09-09', 5, 2023, 0.0, 'active'),
(16, 'SV202201', 'Yến', 'Phạm Thị', 'yen.pt@uni.edu.vn', '2004-01-14', 1, 2022, 0.0, 'active'),
(17, 'SV202202', 'Huy', 'Vũ Đức', 'huy.vd@uni.edu.vn', '2004-03-30', 2, 2022, 0.0, 'active'),
(18, 'SV202203', 'Mai', 'Trần Tuyết', 'mai.tt@uni.edu.vn', '2004-07-22', 3, 2022, 0.0, 'active'),
(19, 'SV202204', 'Tùng', 'Hoàng Sơn', 'tung.hs@uni.edu.vn', '2004-10-05', 4, 2022, 0.0, 'active'),
(20, 'SV202205', 'Ngọc', 'Đặng Kim', 'ngoc.dk@uni.edu.vn', '2004-08-18', 5, 2022, 0.0, 'active');
INSERT INTO Sections (section_id, course_id, instructor_id, room_id, semester, academic_year, schedule_days, start_time, end_time, max_enrollment, current_count) VALUES
(1, 1, 1, 1, 'HK1', '2024-2025', '08:00:00', '08:00:00', '10:00:00', 40, 0),
(2, 2, 2, 2, 'HK1', '2024-2025', '10:00:00', '10:00:00', '12:00:00', 40, 0),
(3, 3, 3, 3, 'HK1', '2024-2025', '13:00:00', '13:00:00', '15:00:00', 40, 0), 
(4, 4, 4, 4, 'HK1', '2024-2025', '15:00:00', '15:00:00', '17:00:00', 40, 0),
(5, 5, 5, 5, 'HK1', '2024-2025', '08:00:00', '08:00:00', '10:00:00', 5, 0),
(6, 6, 6, 6, 'HK2', '2024-2025', '10:00:00', '10:00:00', '12:00:00', 35, 0),
(7, 7, 7, 7, 'HK2', '2024-2025', '13:00:00', '13:00:00', '15:00:00', 35, 0),
(8, 8, 8, 8, 'HK2', '2024-2025', '15:00:00', '15:00:00', '17:00:00', 35, 0),
(9, 9, 1, 9, 'HK3', '2024-2025', '08:00:00', '08:00:00', '11:00:00', 30, 0),
(10, 10, 2, 10, 'HK3', '2024-2025', '13:00:00', '13:00:00', '16:00:00', 30, 0);
INSERT INTO Enrollments (student_id, section_id, enroll_date, status) VALUES
(1, 1, '2024-09-01', 'registered'), (1, 2, '2024-09-01', 'registered'), (1, 3, '2024-09-01', 'registered'),
(2, 1, '2024-09-01', 'registered'), (2, 2, '2024-09-01', 'registered'), (2, 5, '2024-09-01', 'registered'),
(3, 3, '2024-09-02', 'registered'), (3, 4, '2024-09-02', 'registered'), (3, 5, '2024-09-02', 'registered'),
(4, 1, '2024-09-02', 'registered'), (4, 4, '2024-09-02', 'registered'), (4, 6, '2024-09-02', 'registered'),
(5, 5, '2024-09-03', 'registered'), (5, 6, '2024-09-03', 'registered'), (5, 7, '2024-09-03', 'registered'),
(6, 2, '2024-09-03', 'registered'), (6, 7, '2024-09-03', 'registered'),
(7, 3, '2024-09-04', 'registered'), (7, 8, '2024-09-04', 'registered'),
(8, 4, '2024-09-04', 'registered'), (8, 9, '2024-09-04', 'registered'),
(9, 5, '2024-09-05', 'registered'), (9, 10, '2024-09-05', 'registered'),
(10, 6, '2024-09-05', 'registered'), (10, 1, '2024-09-05', 'registered'),
(13, 2, '2024-09-06', 'registered'), (13, 8, '2024-09-06', 'registered'),
(14, 3, '2024-09-06', 'registered'), (14, 9, '2024-09-06', 'registered'),
(15, 4, '2024-09-07', 'registered'), (15, 10, '2024-09-07', 'registered'),
(16, 1, '2024-09-07', 'registered'), (17, 2, '2024-09-07', 'registered'),
(18, 3, '2024-09-08', 'registered'), (19, 4, '2024-09-08', 'registered'),
(20, 6, '2024-09-08', 'registered'), (20, 7, '2024-09-08', 'registered'),
(12, 1, '2024-09-08', 'registered'); -- Sinh viên 12 đăng ký lớp 1
CREATE VIEW vw_student_transcript AS
SELECT
	s.student_code AS ma_sv,
    CONCAT(s.last_name, ' ', s.first_name) AS ho_ten,
    d.dept_name AS khoa,
    c.course_code AS ma_hoc_phan,
    c.title AS ten_hoc_phan,
    c.credits AS so_tin_chi,
    sec.semester AS hoc_ky,
    sec.academic_year AS nam_hoc,
    g.score AS diem_so,
    g.grade_letter AS diem_chu,
    g.grade_points AS diem_4,
    e.status AS trang_thai
FROM Students s
JOIN Departments d ON d.dept_id = s.dept_id
JOIN Enrollments e ON e.student_id = s.student_id
JOIN Sections sec ON sec.section_id = e.section_id
JOIN Courses c ON c.course_id = sec.course_id
LEFT JOIN Grades g ON g.enrollment_id = e.enrollment_id;

CREATE VIEW vw_student_ranking AS
SELECT
	ROW_NUMBER() OVER (ORDER BY s.gpa DESC) AS thu_hang,
    s.student_code AS ma_sv,
	CONCAT(s.last_name, ' ', s.first_name) AS ho_ten,
	d.dept_name AS khoa,
    s.enrollment_year AS nien_khoa,
    ROUND(s.gpa, 2) AS gpa,
    CASE
		WHEN s.gpa >= 3.6 THEN 'Xuất sắc'
        WHEN s.gpa >= 3.2 THEN 'Giỏi'
        WHEN s.gpa >= 2.5 THEN 'Khá'
        WHEN s.gpa >= 2.0 THEN 'Trung bình'
        ELSE 'Yếu'
	END AS xep_loai
FROM Students s
JOIN Departments d ON d.dept_id = s.dept_id
WHERE s.gpa > 0;

-- =========================================================
-- 8.1. Transaction 1 — Đăng ký học phần thành công
-- =========================================================
SELECT '=== TRANSACTION 1: Đăng ký học phần ===' AS '';
START TRANSACTION;
SELECT 
    CONCAT('Sĩ số section 3 trước: ', current_count, '/', max_enrollment) AS 'Trạng thái'
FROM sections WHERE section_id = 3;
INSERT INTO enrollments (student_id, section_id, enroll_date) 
VALUES (11, 3, CURDATE());
SELECT 
    CONCAT('Sĩ số section 3 sau: ', current_count, '/', max_enrollment) AS 'Trạng thái'
FROM sections WHERE section_id = 3;
COMMIT;
SELECT '-> COMMIT thành công!' AS '';
-- =========================================================
-- 8.2. Transaction 2 — Nhập điểm và GPA tự cập nhật
-- =========================================================
SELECT '=== TRANSACTION 2: Nhập điểm ===' AS '';
START TRANSACTION;
SELECT 'GPA trước:' AS '', gpa FROM students WHERE student_id = 11;
INSERT INTO grades (enrollment_id, score, graded_date)
SELECT e.enrollment_id, 8.5, NOW()
FROM enrollments e
JOIN sections sec ON sec.section_id = e.section_id
WHERE e.student_id = 11 AND sec.course_id = 3; -- Giả sử course_id của section 3 là 3
SELECT 'GPA sau:' AS '', gpa FROM students WHERE student_id = 11;
COMMIT;
-- =========================================================
-- 8.3. Transaction 3 — Demo ROLLBACK khi lớp đầy
-- =========================================================
SELECT '=== TRANSACTION 3: Demo ROLLBACK ===' AS '';
START TRANSACTION;
UPDATE sections SET current_count = max_enrollment WHERE section_id = 5;
SAVEPOINT sp_enroll;
INSERT INTO enrollments (student_id, section_id, enroll_date) VALUES (12, 5, CURDATE());
ROLLBACK TO sp_enroll;
ROLLBACK;
SELECT '-> ROLLBACK - Đăng ký thất bại vì lớp đầy' AS '';
-- =========================================================
-- 8.4. Transaction 4 — Hủy đăng ký
-- =========================================================
SELECT '=== TRANSACTION 4: Hủy đăng ký ===' AS '';
START TRANSACTION;
UPDATE enrollments 
SET status = 'dropped' 
WHERE student_id = 11 AND section_id = 3;
SELECT 
    CONCAT('Sĩ số sau khi hủy: ', current_count, '/', max_enrollment) AS 'Trạng thái'
FROM sections WHERE section_id = 3;
COMMIT;