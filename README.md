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

## ADIM 4: GitHub Secrets Ekle (2 dk)

1. Repo sayfasinda "Settings" > "Secrets and variables" > "Actions"
2. "New repository secret" de ve sirayla ekle:

| Secret Name | Deger |
|-------------|-------|
| `UNITY_EMAIL` | Herhangi bir e-posta adresi (ornek@gmail.com) |
| `UNITY_PASSWORD` | Herhangi bir sifre (12345678) |

**NOT:** Bu gercek Unity hesabi DEGIL. GitHub Actions'in calismasi icin bos yer tutucu gerekiyor. Unity Personal lisans ucretsiz ve otomatik aktive olur.

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
- [ ] GitHub Secrets eklendi (UNITY_EMAIL, UNITY_PASSWORD - herhangi deger)
- [ ] Actions > Build Unity WebGL > Run workflow
- [ ] Build basarili (yesil tik)
- [ ] Artifact indirildi
- [ ] python3 -m http.server ile test edildi
