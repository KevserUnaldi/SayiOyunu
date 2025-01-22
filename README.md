# Sayı Oyunu 🎮

Sayı Oyunu, oyuncuların verilen hedef sayıya ulaşmak için sayılar ve işlemlerle çalıştığı, eğitici ve eğlenceli bir Unity oyunudur. Proje üç temel script (GameManager, KupScript, TestScript) ve oyun mekaniğini kapsayan özelliklerle tasarlanmıştır.

## 📋 Özellikler
- Oyuncular, rastgele oluşturulmuş hedef sayıya ulaşmak için toplama, çıkarma, çarpma ve bölme işlemlerini kullanır.
- Farklı zorluk seviyeleri (Kolay, Orta, Zor).
- Animasyonlu kazanç ve kayıp durumları.
- En yüksek puan kaydı ve ses efektleri.

## 🎮 Kullanım
1. Oyunu başlatın ve bir zorluk seviyesi seçin.
2. Verilen hedef sayıya ulaşmak için küplerin üzerindeki sayıları ve işlemleri kullanın.
3. Doğru hamleleri yaparak hedef sayıya ulaştığınızda oyunu kazanın!

## 🛠️ Proje Yapısı
Proje, aşağıdaki scriptlerden oluşmaktadır:

### 1. `GameManager.cs`
- Oyunun ana mantığını kontrol eder.
- Hedef sayıyı belirler ve kullanıcı etkileşimlerini işler.
- Küplerin dinamik olarak oluşturulmasını sağlar.
- Kazanma ve kaybetme durumlarını yönetir.

### 2. `KupScript.cs`
- Her küpün davranışını kontrol eder.
- Küp seçimleri, animasyonlar ve etkileşimleri içerir.

### 3. `TestScript.cs`
- Test amaçlı temel bir scripttir.

## 🛠️ Kurulum
1. Bu projeyi bilgisayarınıza klonlayın:
   ```bash
   git clone https://github.com/kullanici-adi/sayi-oyunu.git
