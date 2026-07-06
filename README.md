# Subway Surfers WebGL - GitHub Actions Build

## Bilgisayar Gereksinimleri
**HIÇBIR SEY KURMANA GEREK YOK!** GitHub'in sunucularinda derleniyor.
Senin E1-1200 islemcin bu isleme hic karismiyor.

---

## ADIM 1: GitHub Hesabi Olustur (5 dk)

1. https://github.com/signup adresine git
2. E-posta, sifre, kullanici adi gir
3. E-postana gelen dogrulama kodunu gir

---

## ADIM 2: Yeni Repo Olustur (2 dk)

1. GitHub'da sag ustteki "+" butonuna tikla
2. "New repository" sec
3. Repository name: `subway-surfers-webgl`
4. "Create repository" de

---

## ADIM 3: Dosyalari Yukle (5 dk)

1. Repo sayfasinda "Add file" > "Upload files"
2. ZIP icindeki TUM dosyalari sec ve surukle-birak
3. "Commit changes" de

---

## ADIM 4: Unity Lisansi Ekle (ONEMLI!)

GitHub Actions Unity derlemek icin lisans gerekiyor.

### 4a. Unity ID Olustur
1. https://id.unity.com/ adresine git
2. Ucretsiz hesap olustur (Personal plan)

### 4b. GitHub Secrets Ekle
1. Repo sayfasinda "Settings" > "Secrets and variables" > "Actions"
2. "New repository secret" de ve sirayla ekle:

| Secret Name | Nasil Alirsin |
|-------------|---------------|
| `UNITY_EMAIL` | Unity ID e-postan |
| `UNITY_PASSWORD` | Unity ID sifren |
| `UNITY_LICENSE` | Asagidaki adimlari takip et |

### 4c. Lisans Kodunu Alma (Terminalde)

Lubuntu'da terminal ac ve calistir:

```bash
# Unity Editor Docker image indir ve lisans al
docker run --rm   -e "UNITY_EMAIL=epostan@ornek.com"   -e "UNITY_PASSWORD=sifren"   -v "$(pwd):/project"   unityci/editor:ubuntu-2022.3.20f1-webgl-3   /opt/Unity/Editor/Unity   -batchmode -nographics -quit   -logFile /dev/stdout   -username "epostan@ornek.com"   -password "sifren"   -serial ""   -returnlicense
```

Bu komut calistiktan sonra, bulundugun klasorde bir lisans dosyasi olusur.
Icerigini kopyala ve GitHub'da `UNITY_LICENSE` secret olarak ekle.

**VEYA** daha kolay yol:
- Windows'ta bir arkadasinin PC'sinde Unity Hub ac
- "Manage licenses" > "Activate with license" > Personal sec
- Cikis yap, lisans dosyasi olusur
- Dosyayi ac, icerigini kopyala
- GitHub'a `UNITY_LICENSE` secret olarak yapistir

---

## ADIM 5: Build'i Baslat (1 dk)

1. GitHub repo sayfasinda "Actions" sekmesine tikla
2. Sol tarafta "Build Unity WebGL" workflow'u goreceksin
3. Uzerine tikla, sagda "Run workflow" butonu var
4. "Run workflow" de
5. Yesil tik gelene kadar bekle (10-20 dk)

---

## ADIM 6: Sonucu Al (1 dk)

Build bittiginde:
1. "Actions" sekmesinde yesil tik goreceksin
2. Asagida "Artifacts" bolumunde "WebGL-Build" indirme linki var
3. Indir ve ZIP'i ac
4. Icinde `index.html` var - tarayicida ac ve oyna!

---

## SORUN GIDERME

### "License activation failed" hatasi
- UNITY_LICENSE secret'i eksik veya yanlis
- Adim 4c'yi tekrarla, dogru lisans kodunu al

### Build 1 saatten uzun suruyor
- Ilk build 15-20 dk surer, sonrakiler 5-10 dk
- GitHub Actions ucretsiz plan: ayda 2000 dk limit

### "No files were found with the provided path" hatasi
- Build basarisiz olmus demek
- Actions sekmesinde kirmizi X'e tikla, log'u oku

### Oyun acilmiyor
- `index.html` dosyasini dogrudan cift tiklama CALISMAZ
- Bir HTTP sunucusu lazim:
  ```bash
  # Python ile basit sunucu
  python3 -m http.server 8000
  # Tarayicida: http://localhost:8000
  ```

---

## HIZLI KONTROL LISTESI

- [ ] GitHub hesabi olusturuldu
- [ ] Repo olusturuldu (`subway-surfers-webgl`)
- [ ] Dosyalar yuklendi
- [ ] Unity ID olusturuldu (https://id.unity.com/)
- [ ] GitHub Secrets eklendi (UNITY_EMAIL, UNITY_PASSWORD, UNITY_LICENSE)
- [ ] Actions > Build Unity WebGL > Run workflow
- [ ] Build basarili (yesil tik)
- [ ] Artifact indirildi
- [ ] python3 -m http.server ile test edildi
