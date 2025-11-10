# 🎮 [Pew-Pew]  
_Yazılım Geliştirme Laboratuvarı 1 Proje Raporu_

## Hazırlayanlar
Ömer Faruk Güler\
Ömer Faruk Sarı\
Emirhan Bıkmaz


## 🎯 Proje Tanıtımı
Bu proje, **Unity oyun motoru** kullanılarak geliştirilen bir TPS oyunudur.  
Oyunun amacı, oyuncuya strateji geliştirerek görevleri tamamlamayı ve yeni çözümler üretme becerisini geliştirmeyi hedeflemektedir. 

**Temel Özellikler:**
- Tek bölümlü oynanış
- Pause (duraklatma) menüsü
- Setting (ayarlar) menüsü
- Hayatta kalma mücadelesi
- Basit AI ve düşman davranışları
- NPC ile konuşma ve bilgi alma


## 🎯 Amaç ve Hedefler
Bu oyunun geliştirilmesindeki amaç:
-  Oyun programlama mantığını ve Unity C# temellerini öğrenmek .
-  Sahne geçişleri, UI sistemleri, event yönetimi gibi Unity bileşenlerinde pratik yapmak.
-  Git temellerini kullanarak farklı branch'ler  üzerinden ilerleyerek ortak proje yürütmek.
-  Unity gibi oyun programlarını genel olarak nasıl geliştirildiğini öğrenmek.


## ⚙️ Kullanılan Teknolojiler
| Teknoloji | Kullanım Amacı |
|------------|----------------|
| **Unity** | Oyun motoru |
| **C# (MonoBehaviour)** | Oyun mantığının yazılması |
| **Git & GitHub** | Versiyon kontrolü ve ekip çalışması |
| **VS Code / Rider / Visual Studio** | Kod editörü |


## 🕹️ Oyun Mekanikleri ve Blok Diyagram

**Temel Oyun Döngüsü:**
1. Oyuncu oyuna başlar.
2. NPC oyun hakkında bilgilendirme yapar.
4. Alandaki düşmanların tamamını öldürür.
5. Oyuncu ya da bütün düşmanlar ölürse oyun biter.
6. Oyuncu “Retry” veya “Main Menu” seçenekleriyle ilerleyebilir.


**Blok Diyagram:**
> Aşağıdaki oyunun Ana Menü'sü görünmektedir.
![alt text](https://r.resimlink.com/bOuMp.png)

1. Start butonuna basılınca oyun başlar.
2. Quit butonuna basılınca oyundan çıkılır.
3. Settings butonuna basılınca oyun ayarları bölümüne yönlendirir.

> Oyun durdurulunca açılan ekran görünmektedir.
![alt text](https://r.resimlink.com/lt2zP.png)

1. Menu butonuna basınca ana menüye yönlendirilir.
2. Continue butonuna basınca oyun devam eder.
3. Quit butonuna basınca oyundan çıkılır.


## 🖥️ Oyun Ekranları ve Arayüz Tasarımı

> Ana karakter ölünce açılan ekran görünmektedir.
![alt text](https://r.resimlink.com/jcNXsBplfTGD.png)

Burada görmüş olduğunuz görüntü ana karakterin girmiş olduğu çatışmada öldükten sonra karşısında botların da bulunduğu bir anlık bir görüntüdür.
1. Try Again butonuna tıklayınca oyun tekrardan başlar.


## 📚 Literatür Taraması ve Karşılaştırma
Bu bölümde literatürdeki benzer oyun veya projelere yer verilmelidir.

**Örnek:**
-[Kaynak 1] “Development of a 3D Shooter Game Using Unity Engine” – IEEE, 2021 \
Bu çalışmada, Unity oyun motoru kullanılarak birinci şahıs kamera açısına sahip 3D nişancı türünde bir oyun geliştirilmiştir. Oyunda temel olarak oyuncunun belirli hedefleri vurması ve bölüm içinde ilerlemesi amaçlanmıştır. Proje, Unity’nin fizik sistemi, ışıklandırma ve kullanıcı arayüzü bileşenlerini kullanarak oyun mekaniği oluşturmuştur.

-[Kaynak 2] “Design and Implementation of a Simple 2D Platformer Game” – ResearchGate, 2020 \
Bu çalışmada temel zıplama ve hareket etme mekanikleri üzerine kurulu bir 2D platform oyunu geliştirilmiştir. Karakter kontrolü, çarpışma (collision) tespiti, skor artırma ve sahne geçişleri gibi modüller C# diliyle Unity üzerinde kodlanmıştır.

-[Kaynak 3] “Simple Shooter” – GitHub Open Source Project (Brackeys, 2022) \
Açık kaynaklı bu proje, Unity üzerinde temel nişancı mekaniğini öğretmeyi amaçlamaktadır. Oyunda sahne geçişleri, menü sistemi, hedef objeleri ve skor takibi bulunmaktadır. Kod yapısı eğitim amaçlı sade tutulmuştur.

-[Kaynak 4] “Game Development for Beginners with Unity” – Unity Learn Platform (2023) \
Unity’nin resmi eğitim platformunda yer alan bu projede, kullanıcıya sahne geçişleri, UI menüsü ve temel input yönetimi gibi konular öğretilmektedir. Proje modüler yapıdadır ve “Pause/Resume” gibi menü işlevlerini içerir.

⚖️ Karşılaştırma

Bizim projemiz, yukarıdaki çalışmalardan aşağıdaki yönlerle farklılık göstermektedir:

Tek Sahne Yapısı:
Diğer örneklerde birden fazla bölüm veya seviye bulunurken, bizim projemiz tek sahne (tek bölüm) üzerinde sade bir oyun döngüsü içerir.

Kullanıcı Arayüzü (UI) Basitliği:
Literatürdeki oyunlar genellikle karmaşık menü sistemlerine sahipken, biz sade ve işlevsel bir Start / Pause / Quit menü yapısı tasarladık.

Takım Çalışması ve GitHub Kullanımı:
Çoğu geliştirme çalışması bireysel örneklerden oluşurken, bu proje çoklu geliştirici ile Git ve Github versiyon kontrolü kullanılarak yürütülmüştür.

Kendi Script ve Fonksiyonlarımız:\
Menü kontrolü `(MenuController)`, sahne geçişleri `(SceneManager.LoadScene())`, ve duraklatma işlevleri `(PauseMenu)` tamamen ekip tarafından yazılmış özgün script’lerle sağlanmıştır.


## 🧱 Kullanılan Yazılım Mimarileri ve Teknikler
- **Event-Driven Yapı:** Butonlar ve input olayları `OnClick()`, `GetKeyDown()` gibi event’lerle yönetilmiştir.  
- **Modüler Kodlama:** Menü, karakter kontrolü, skor sistemi ayrı script’ler halindedir.  
- **Scene Yönetimi:** Unity’nin `SceneManager` sınıfı kullanılarak sahne geçişleri yapılmıştır.  
- **Prefab Kullanımı:** Tekrarlayan nesneler prefab olarak tanımlanmıştır.


## ⚠️ Karşılaşılan Zorluklar ve Çözümler
| Zorluk | Çözüm |
|---------|--------|
| Merge conflict hataları | Git üzerinde `--theirs` / `--ours` yöntemiyle çözüldü |
| Pause menüsünün sürekli açık kalması | `Cursor.visible` ve `SetActive(false)` ayarları düzenlendi |
| Sahne geçişlerinin otomatik başlaması | `StartGame()` buton kontrolü eklendi |
| Unity LSP (Language Server) hataları | Kod dosyalarında `using System.Collections` yazım hatası düzeltildi |
|NPC objesi NavMesh Agent bileşeniyle etkileşime girmeme hataları| Çözüm olarak NavMesh Surface yeniden bake edilip, EnemyAI script’inde agent.SetDestination() çağrısı güncellendi.|
|Ana karakterin silah tutuş pozisyonu bozuk görünmesi|Target objesi RightHand transform’una yeniden tanımlandı.|


## 🔄 Proje Süreci ve GitHub Kullanımı
- Her üye kendi **branch’inde** geliştirme yapmıştır.  
- `feature/`, `main/`,  gibi branch yapısı kullanılmıştır.  
- Merge işlemleri `pull request` ile yapılmıştır.  
- Commit mesajları açıklayıcıdır:  
  - Shoot Only When Holding Gun
  - Login Page Second Commit
-Gibi açıklamalarla commitler yapılmıştır.


## 🧠 Sonuç ve Kazanımlar
Bu proje sayesinde:
- Unity oyun geliştirme temelleri öğrenildi.
- Takım çalışmasında Git ve versiyon kontrol sistemleri etkin kullanıldı.
- Hata yönetimi, UI tasarımı ve kullanıcı etkileşimi konusunda deneyim kazanıldı.


## 📚 Kaynakça
1. https://docs.unity3d.com/  
2. https://www.youtube.com/watch?v=MMBQQiJrO_0&t=759s
3. https://www.youtube.com/playlist?list=PLX_yguE0Oa8QmfmFiMM9_heLBeSA6sNKx 
4. https://www.youtube.com/@CodeMonkeyUnity
5. https://www.youtube.com/watch?v=ydhpu4j7fIk
6. https://www.youtube.com/watch?v=L-81sc7Alx4
7. https://github.com/llamacademy/scriptable-object-based-guns?tab=readme-ov-file#readme
8. https://www.uopeople.edu/blog/what-is-unity-game-development/
