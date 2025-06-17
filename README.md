# Project Skolar

Project Skolar, ASP.NET Core ve SQL Server kullanılarak geliştirilen bir çevrimiçi sınıf yönetim sistemidir. Bu proje, eğitimciler ve öğrenciler arasında etkileşimli bir öğrenme ortamı sağlamayı hedefler. JWT tabanlı kimlik doğrulama, çok katmanlı mimari ve DTO kullanımı gibi modern yazılım geliştirme tekniklerini içerir.

## 🚀 Özellikler

* **Kullanıcı Rolleri:** Öğrenci ve Eğitmen rolleriyle kullanıcı yönetimi.
* **Müfredat Yönetimi:** Konu ve alt konu yapısıyla esnek müfredat oluşturma.
* **Ödev Sistemi:** Dosya yükleme destekli ödev verme ve notlandırma.
* **JWT Kimlik Doğrulama:** Güvenli API erişimi için JWT tabanlı kimlik doğrulama.
* **Çok Katmanlı Mimari:** Controller, Service, Repository katmanlarıyla temiz kod yapısı.
* **DTO Kullanımı:** Veri transferi için Data Transfer Object (DTO) yapıları.

## 🛠️ Teknoloji Yığını

* **Backend:** ASP.NET Core
* **Veritabanı:** Microsoft SQL Server
* **ORM:** Entity Framework Core
* **Kimlik Doğrulama:** JWT (JSON Web Token)
* **Dokümantasyon:** Swagger (Swashbuckle.AspNetCore)

## 🧱 Mimari Yapı

Proje, aşağıdaki katmanlardan oluşan çok katmanlı bir mimariye sahiptir:

1. **Controller Katmanı:** HTTP isteklerini alır ve uygun servis metodlarını çağırır.
2. **Service Katmanı:** İş mantığını içerir ve veri erişim katmanıyla iletişim kurar.
3. **Repository Katmanı:** Veritabanı işlemlerini gerçekleştirir.
4. **DTO'lar:** Veri transferi için kullanılır, model ile istemci arasındaki veri yapısını tanımlar.

## 🔐 JWT Entegrasyonu

JWT, kullanıcıların kimlik doğrulamasını sağlamak için kullanılır. `JWTHelper` sınıfı, token oluşturma ve çözümleme işlemlerini yönetir. Token, kullanıcı bilgilerini içerir ve API isteklerinde kimlik doğrulama için kullanılır.

## 🗄️ Veritabanı Tasarımı

Veritabanı, aşağıdaki ana tabloları içerir:

* **Users:** Kullanıcı bilgileri ve rolleri.
* **Participants:** Öğrencilerin sınıflara katılım bilgileri.
* **Assignments:** Eğitmenler tarafından verilen ödevler.
* **SubmittedAssignments:** Öğrencilerin teslim ettiği ödevler.
* **Topics:** Müfredat konuları.
* **TopicNotes:** Konu notları ve içerikleri.
* **TopicNoteBlocks:** Farklı içerik türlerini destekleyen not blokları (paragraf, liste, görsel vb.).

## 📦 Kurulum

1. Bu repoyu klonlayın:

   ```bash
   git clone https://github.com/kullaniciadi/project-skolar.git
   ```

2. Gerekli NuGet paketlerini yükleyin:

   ```bash
   dotnet restore
   ```

3. Veritabanını oluşturun ve bağlantı dizesini `appsettings.json` dosyasına ekleyin.

4. Veritabanı şemasını EF Core Scaffold ile içeri aktarın:

   ```bash
   dotnet ef dbcontext scaffold "Your_Connection_String" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --context-dir Data --context YourDbContextName --force
   ```

   > **Not:** Bu proje, mevcut bir veritabanı şemasından model sınıfları ve `DbContext` oluşturmak için EF Core'un *Scaffold-DbContext* komutunu kullanır. Bu nedenle, `dotnet ef database update` komutu kullanılmaz.

5. Uygulamayı çalıştırın:

   ```bash
   dotnet run
   ```

## 📄 API Dokümantasyonu

Swagger Arayüzünden (/swagger/index.html) tüm API endpointleri detaylıca incelenebilir.

## ✒️ Projeye Ait İlerlemeyi <Devlog> adlı Medium platformundaki yazı serimden takip edebilirsiniz.

🌐 [**\<Devlog> Project-Skolar #1 — Tech Stack, Kurulumlar ve Başlangıç**](https://ugurcankzuit.medium.com/devlog-project-skolar-1-tech-stack-kurulumlar-ba%C5%9Flang%C4%B1%C3%A7-d6383ddd1698)
🌐 [**\<Devlog> Project-Skolar #2 — N-Katmanlı Mimari, JWT Entegrasyonu ve API’de Veri Akışı (DTO)**](https://medium.com/@ugurcankzuit/devlog-project-skolar-2-n-katmanl%C4%B1-mimari-jwt-entegrasyonu-ve-apide-veri-ak%C4%B1%C5%9F%C4%B1-dto-e3f9851a8724)
🌐 [**\<Devlog> Project-Skolar #3 — Google OAuth, Tasarım ve Next.js**](https://medium.com/p/ad9e560da48c)

