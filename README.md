# Büyük Ölçekli E-Ticaret Uygulaması (ASP.NET Core Web API)

Bu proje, full-stack bir e-ticaret sisteminin backend servislerini barındıran, kurumsal mimari prensiplerine uygun olarak geliştirilmiş bir Web API uygulamasıdır. Proje, monolitik karmaşadan uzak, gevşek bağlı (loosely coupled) ve genişletilebilir bir altyapıyı deneyimlemek amacıyla inşa edilmiştir.

---

## 🏗️ Mimari Yapı ve Tasarım Desenleri (Architecture & Patterns)

Uygulamanın back-end mimarisi, sürdürülebilirliği artırmak ve katmanlar arası bağımlılıkları minimize etmek için **SoC (Separation of Concerns)** prensibine göre tasarlanmıştır:

* **ONION Architecture:** Uygulama çekirdeği (Domain ve Application) tamamen dış dünyadan soyutlanmış; veritabanı (Persistence) ve dış servisler (Infrastructure) birer eklenti gibi dış katmanda konumlandırılmıştır.
* **CQRS Pattern (Command Query Responsibility Segregation):** Veri okuma (Query) ve veri yazma/güncelleme (Command) operasyonları tamamen birbirinden ayrılmıştır.
* **MediatR Pattern:** CQRS operasyonlarının yönetiminde, bileşenler arası doğrudan bağımlılığı engelleyen ve istekleri ilgili Handler sınıflarına yönlendiren *Mediator* deseni uygulanmıştır.
* **Repository Pattern:** Veritabanı CRUD işlemleri, kod tekrarını önlemek ve soyutlama sağlamak amacıyla generic bir repository yapısıyla ele alınmıştır.

---

## 🛠️ Teknolojik Altyapı & Kullanılan Araçlar

* **Framework:** .NET 8.0 / 7.0 Web API
* **ORM:** Entity Framework Core (PostgreSQL / MsSQL - Code First)
* **Güvenlik & Kimlik Doğrulama:** Microsoft Identity altyapısı, JWT (JSON Web Token) & Refresh Token mekanizması, Role-Based ve Endpoint-Based yetkilendirme politikaları (Policies).
* **Real-Time İletişim:** SignalR entegrasyonu ile anlık sepet, sipariş ve stok değişimlerinin istemciye push edilmesi.
* **Hata Yönetimi & Loglama:** Global Exception Handler middleware yapısı ve merkezi loglama mimarisi.

---

## 📦 Kurulum ve Çalıştırma

1. Projeyi yerel bilgisayarınıza klonlayın.
2. Ana dizinde bulunan `appsettings.json.template` dosyasının adını `appsettings.json` olarak değiştirin ve kendi yerel veritabanı Connection String bilgilerinizi girin.
3. Package Manager Console veya terminal üzerinden migration'ları veritabanına uygulayın:
   ```bash
   dotnet ef database update
