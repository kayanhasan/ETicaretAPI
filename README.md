# Büyük Ölçekli E-Ticaret Uygulaması (ASP.NET Core Web API)

Bu proje, full-stack bir e-ticaret sisteminin backend servislerini barındıran, kurumsal mimari prensiplerine (Enterprise Architecture) uygun olarak geliştirilmiş bir Web API uygulamasıdır. Proje, monolitik karmaşadan uzak, gevşek bağlı (loosely coupled), yüksek performanslı ve genişletilebilir bir altyapıyı deneyimlemek amacıyla inşa edilmiştir.

---

## 🏗️ Mimari Yapı ve Tasarım Desenleri (Architecture & Patterns)

Uygulamanın back-end mimarisi, sürdürülebilirliği artırmak ve katmanlar arası bağımlılıkları minimize etmek için **SoC (Separation of Concerns)** prensibine göre tasarlanmıştır:

* **ONION Architecture:** Uygulama çekirdeği (Domain ve Application) tamamen dış dünyadan soyutlanmış; veritabanı (Persistence) ve dış servisler (Infrastructure) birer eklenti gibi dış katmanda konumlandırılmıştır.
* **CQRS Pattern (Command Query Responsibility Segregation):** Veri okuma (Query) ve veri yazma/güncelleme (Command) operasyonları tamamen birbirinden ayrılmıştır.
* **MediatR Pattern:** CQRS operasyonlarının yönetiminde, bileşenler arası doğrudan bağımlılığı engelleyen ve istekleri ilgili Handler sınıflarına yönlendiren *Mediator* deseni uygulanmıştır.
* **Pipeline Behavior (Cross-Cutting Concerns):** İsteklerin işlenmesi sürecinde araya giren (Interceptor) yapılar kurularak Validasyon ve Önbellekleme (Caching) gibi operasyonlar handler sınıflarından soyutlanmıştır.
* **Repository Pattern:** Veritabanı CRUD işlemleri, kod tekrarını önlemek ve soyutlama sağlamak amacıyla generic bir repository yapısıyla ele alınmıştır.
* **Event-Driven Architecture (EDA):** Sistem içerisindeki süreçlerin (Örn: Sipariş oluşturma/onaylama) asenkron olarak yürümesi ve mikroservis altyapılarına hazır olması için olay güdümlü mimari modelleri uygulanmıştır.

---

## 🛠️ Teknolojik Altyapı & Kullanılan Araçlar

* **Framework:** .NET 8.0 / 7.0 Web API
* **ORM:** Entity Framework Core (PostgreSQL / MsSQL - Code First)
* **Dağıtık Önbellekleme (Distributed Caching):** High-Performance veri okuma süreçleri için **Redis** entegrasyonu sağlanmıştır. Sayfalanmış sorgular için dinamik key yönetimi ve veri tabanında değişiklik olduğunda ilgili önbellek kümesini temizleyen **Cache Invalidation (Pattern-Based)** mekanizması kurgulanmıştır.
* **Mesaj Kuyruğu & Asenkron Yönetim (Message Broker):** Sisteme yük getiren ağır iş süreçlerinin (Örn: Sipariş bildirim e-postaları) asenkron yönetimi amacıyla **RabbitMQ** ve soyutlama katmanı olarak **MassTransit** entegre edilmiştir.
* **Güvenlik & Kimlik Doğrulama:** Microsoft Identity altyapısı, JWT (JSON Web Token) & Refresh Token mekanizması, Role-Based ve Endpoint-Based yetkilendirme politikaları (Policies).
* **Real-Time İletişim:** SignalR entegrasyonu ile anlık sepet, sipariş ve stok değişimlerinin istemciye push edilmesi.
* **Hata Yönetimi & Loglama:** Global Exception Handler middleware yapısı ve merkezi loglama mimarisi.

---

## 📦 Kurulum ve Çalıştırma

1. Projeyi yerel bilgisayarınıza klonlayın.
2. Projenin bağımlılıkları olan **Redis** ve **RabbitMQ** servislerini ayağa kaldırın (Docker üzerinde varsayılan portlarda çalıştırılması önerilir):
   ```bash
   # Redis çalıştırmak için
   docker run -d --name eticaret_redis -p 6379:6379 redis
   
   # RabbitMQ (Management Paneli dahil) çalıştırmak için
   docker run -d --name eticaret_rabbitmq -p 5672:5672 -p 15172:15672 rabbitmq:3-management
