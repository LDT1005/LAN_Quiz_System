# 📝 Hệ thống Thi Trắc Nghiệm LAN (Classroom Quiz)

Hệ thống cho phép giáo viên tổ chức thi trắc nghiệm trong mạng nội bộ (LAN), phát đề đồng loạt và chấm điểm thời gian thực. Dự án được xây dựng để chạy trong môi trường giả lập VMware.

## 🛠 Công cụ & Môi trường
- **Ngôn ngữ:** C# (.NET Framework 4.7.2+).
- **IDE:** Visual Studio 2022.
- **Thư viện:** Newtonsoft.Json (Xử lý chuỗi dữ liệu), System.Net.Sockets.
- **Giả lập:** VMware (Cấu hình mạng Host-only).

## 📂 Cấu trúc dự án
- **Quiz.Shared**: Thư viện chứa các Class dùng chung như `Question`, `Packet`, `DataHelper`.
- **Quiz.Server**: Ứng dụng dành cho giáo viên (Quản lý kết nối, phát đề, bảng điểm).
- **Quiz.Client**: Ứng dụng dành cho học sinh (Đăng nhập, làm bài, nhận thông báo).

## ✨ Chức năng chính
- **Kết nối TCP đa luồng:** Hỗ trợ nhiều máy ảo kết nối cùng lúc.
- **Broadcast Đề thi:** Gửi đề thi dạng JSON tới tất cả Client đồng loạt.
- **Chấm điểm thời gian thực:** Tự động so khớp đáp án và hiển thị lên Dashboard giáo viên.
- **Thông báo UDP Multicast:** Gửi thông báo khẩn cấp dạng Popup tới toàn bộ lớp học.
- **Xử lý TCP Framing:** Kỹ thuật Header 4-byte giúp tránh lỗi dính gói tin.

## 👥 Phân công nhiệm vụ
- **Lại Đức Thành (Manager & Core):** Xử lý logic mạng TCP/UDP, thiết kế Data Model, Serialization, Quản lý Git.
- **Thành viên UI/UX:** Thiết kế giao diện WinForms, Logic xử lý Client, Timer đếm ngược.

## 🚀 Hướng dẫn cài đặt
1. Clone dự án: `git clone https://github.com/LDT1005/LAN_Quiz_System.git`
2. Mở file `.sln` bằng Visual Studio 2022.
3. Restore NuGet Packages (Newtonsoft.Json).
4. Build Solution và chạy Server trên máy thật, Client trên máy ảo VMware.