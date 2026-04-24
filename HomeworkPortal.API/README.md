🚀 Homework Portal API & MVC UI

Modern, güvenli ve ölçeklenebilir bir Ödev Yönetim Sistemi (LMS).
Bu proje, gerçek dünya senaryolarını hedefleyerek kurumsal backend + güvenli UI client yaklaşımıyla geliştirilmiştir.

📌 Proje Amacı

Bu sistemin amacı:

Öğretmenlerin ders oluşturması ve ödev ataması
Öğrencilerin ödevleri görüntüleyip teslim etmesi
Öğretmenlerin teslimleri değerlendirip not vermesi
Tüm sürecin güvenli, tutarlı ve performanslı yönetilmesi

Kısaca:

“Google Classroom benzeri bir sistemin backend + UI mimarisini kurmak”

🧠 Mimari Yaklaşım

Bu proje sıradan CRUD değildir.

Aşağıdaki prensipler uygulanmıştır:

Clean Architecture
Layered Architecture
SOLID Principles
Separation of Concerns
Katmanlar:
API (Controller Layer)
Service Layer (Business Logic)
Repository Layer
Database (EF Core / SQL Server)
MVC UI (Dumb Client)
🛠️ Kullanılan Teknolojiler
Backend
ASP.NET Core Web API
Entity Framework Core (Code First)
SQL Server
ASP.NET Identity
JWT Authentication
AutoMapper
Frontend
ASP.NET Core MVC
jQuery AJAX
Bootstrap 5
Production Tools
Serilog (Structured Logging)
Prometheus (Metrics)
Rate Limiting
Global Exception Middleware
Polly (Retry Mechanism)
🔐 Güvenlik Özellikleri
JWT tabanlı kimlik doğrulama
Role-based authorization (Admin / Teacher / Student)
Ownership kontrolü (Data-level security)
Rate limiting (DDoS koruması)
Security headers (XSS, Clickjacking)
User Secrets (config güvenliği)
⚙️ Temel Özellikler
👨‍🏫 Teacher
Ders oluşturma
Ödev oluşturma
Teslimleri görüntüleme
Not verme (Concurrency kontrollü)
👨‍🎓 Student
Derse kayıt olma
Ödevleri görüntüleme
Dosya yükleme
Notları görüntüleme
🛠 Admin
Kullanıcı yönetimi
Rol atama
Sistem kontrolü
🧱 Kritik Mühendislik Çözümleri

Bu projeyi “farklı” yapan kısım burası:

✔ Soft Delete (Global Query Filter)

Veriler fiziksel silinmez → veri kaybı önlenir

✔ Concurrency Control (RowVersion)

Aynı veri üzerinde çakışma → otomatik engellenir

✔ Transaction Boundary

Core işlem ile side-effect ayrılmıştır
→ sistem kırılmaz

✔ Unique Constraint (Race Condition çözümü)

Çift submission → DB seviyesinde engellenir

✔ Pagination (Performans)

Büyük veri → RAM’e yüklenmez

✔ Global Exception Handling

Tüm hatalar standart JSON formatında döner

🔔 Notification Sistemi
In-App notification (DB tabanlı)
Batch insert (AddRange)
Retry mekanizması (Polly)
📁 Dosya Yükleme Sistemi
IFormFile ile gerçek upload
5MB limit
Extension whitelist
Path traversal koruması
GUID ile güvenli dosya isimlendirme
📡 API Özellikleri
REST prensiplerine büyük ölçüde uygun
DTO tabanlı veri transferi
Status code disiplini (200, 400, 401, 403, 409)
Pagination destekli endpointler
🎨 UI Özellikleri
AJAX tabanlı iletişim
Token Session’da saklanır
Global unauthorized handler
Role-based dinamik menü
Tek sayfa → rol bazlı görünüm
🧪 Test & Dayanıklılık
Race condition testleri
Concurrency testleri
Unauthorized erişim testleri
Stress testleri