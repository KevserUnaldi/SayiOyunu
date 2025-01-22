using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    
    public Text HedefText;
    public Text SonucText;
    public Text durumText;
    public Text puanText;
    public Text enYuksekPuanText;
    public Button baslaButton;
    public Button yenidenBaslatButton;
    public Button kolayButton;
    public Button ortaButton;
    public Button zorButton;
    

    public Button toplamaButton;
    public Button cikarmaButton;
    public Button carpmaButton;
    public Button bolmeButton;
    
    
    public GameObject kupPrefab;
    public Transform kuplerinParenti;

    
    [Header("Ses Efektleri")]
    public AudioSource audioSource;
    public AudioClip tiklamaSesi;
    public AudioClip basarmaSesi;
    public AudioClip kaybetmeSesi;
    
    
    private int hedefSayi;
    private int hamle;
    private int puan = 0;
    private int enYuksekPuan = 0;
    private int zorlukSeviyesi = 1;
    private List<KupScript> kuplar = new List<KupScript>();
    public bool oyunBitti;
    

    private int secilenSayi1 = -1;
    private int secilenSayi2 = -1;
    private KupScript secilenKup1;
    private KupScript secilenKup2;
    private string secilenIslem = "";
    
    
    private readonly int[] seviyeKupSayilari = { 3, 4, 7 }; 

    void Start()
    {
        
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        
        enYuksekPuan = PlayerPrefs.GetInt("EnYuksekPuan", 0);
        EnYuksekPuaniGoster();
        
    
        hamle = 0;
        
    
        HedefText.gameObject.SetActive(false);
        SonucText.gameObject.SetActive(false);
        durumText.gameObject.SetActive(false);
        puanText.gameObject.SetActive(false);
        yenidenBaslatButton.gameObject.SetActive(false);
        
        
        IslemButonlariniGoster(false);
        
        
        kolayButton.gameObject.SetActive(false);
        ortaButton.gameObject.SetActive(false);
        zorButton.gameObject.SetActive(false);
        
        
        baslaButton.gameObject.SetActive(true);
        
        
        enYuksekPuanText.gameObject.SetActive(true);
        
        
        baslaButton.onClick.AddListener(ZorlukSeciminiGoster);
        yenidenBaslatButton.onClick.AddListener(YenidenBaslat);
        kolayButton.onClick.AddListener(() => ZorlukSec(1));
        ortaButton.onClick.AddListener(() => ZorlukSec(2));
        zorButton.onClick.AddListener(() => ZorlukSec(3));
        
        
        toplamaButton.onClick.AddListener(() => IslemSec("+"));
        cikarmaButton.onClick.AddListener(() => IslemSec("-"));
        carpmaButton.onClick.AddListener(() => IslemSec("×"));
        bolmeButton.onClick.AddListener(() => IslemSec("÷"));
    }

    private void SesEfektiCal(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    void ZorlukSeciminiGoster()
    {
        baslaButton.gameObject.SetActive(false);
        kolayButton.gameObject.SetActive(true);
        ortaButton.gameObject.SetActive(true);
        zorButton.gameObject.SetActive(true);
    }

    void ZorlukSec(int seviye)
    {
        zorlukSeviyesi = seviye;
        kolayButton.gameObject.SetActive(false);
        ortaButton.gameObject.SetActive(false);
        zorButton.gameObject.SetActive(false);
        OyunuBaslat();
    }

    void OyunuBaslat()
    {
        
        oyunBitti = false;
        hamle = 0;
        secilenSayi1 = -1;
        secilenSayi2 = -1;
        secilenKup1 = null;
        secilenKup2 = null;
        secilenIslem = "";
        
        
        HedefText.gameObject.SetActive(true);
        SonucText.gameObject.SetActive(true);
        durumText.gameObject.SetActive(true);
        puanText.gameObject.SetActive(true);
        yenidenBaslatButton.gameObject.SetActive(false);
        
        
        IslemButonlariniGoster(false);
        
    
        KupleriOlustur();
        
        
        HedefSayiyiBelirle();
        
    
        durumText.text = "Bir sayı seçin";
        durumText.color = Color.black;
    }

    void KupleriOlustur()
    {
    
        foreach(Transform child in kuplerinParenti)
        {
            Destroy(child.gameObject);
        }
        kuplar.Clear();

        int kupSayisi = seviyeKupSayilari[zorlukSeviyesi - 1];
        float aralik = 2f;
        float baslangicX = -(kupSayisi - 1) * aralik / 2f;
        
        for(int i = 0; i < kupSayisi; i++)
        {
            Vector3 pozisyon = new Vector3(baslangicX + i * aralik, -2f, 0);
            GameObject yeniKup = Instantiate(kupPrefab, pozisyon, Quaternion.identity, kuplerinParenti);
            KupScript kupScript = yeniKup.GetComponent<KupScript>();
            
        
            int minSayi = 1;
            int maxSayi = zorlukSeviyesi == 1 ? 10 : 
                         zorlukSeviyesi == 2 ? 15 : 20;
            
            
            int rastgeleSayi = Random.Range(minSayi, maxSayi + 1);
            kupScript.SayiyiAyarla(rastgeleSayi);
            
            kuplar.Add(kupScript);
        }

        KupleriDuzenle();
    }

    void HedefSayiyiBelirle()
    {
        
        List<int> mevcutSayilar = new List<int>();
        foreach(var kup in kuplar)
        {
            mevcutSayilar.Add(kup.sayi);
        }

    
        List<int> olasiSonuclar = new List<int>();
        
        
        olasiSonuclar.AddRange(mevcutSayilar);
        
        
        for(int i = 0; i < mevcutSayilar.Count; i++)
        {
            for(int j = i + 1; j < mevcutSayilar.Count; j++)
            {
                
                olasiSonuclar.Add(mevcutSayilar[i] + mevcutSayilar[j]);
                
                
                olasiSonuclar.Add(Mathf.Abs(mevcutSayilar[i] - mevcutSayilar[j]));
                
                
                olasiSonuclar.Add(mevcutSayilar[i] * mevcutSayilar[j]);
                
                
                if(mevcutSayilar[j] != 0 && mevcutSayilar[i] % mevcutSayilar[j] == 0)
                    olasiSonuclar.Add(mevcutSayilar[i] / mevcutSayilar[j]);
                if(mevcutSayilar[i] != 0 && mevcutSayilar[j] % mevcutSayilar[i] == 0)
                    olasiSonuclar.Add(mevcutSayilar[j] / mevcutSayilar[i]);
            }
        }
        
        
        olasiSonuclar = olasiSonuclar.Distinct().ToList();
        olasiSonuclar.Sort();
        
        
        int hedefIndex;
        switch(zorlukSeviyesi)
        {
            case 1: 
                hedefIndex = Random.Range(0, olasiSonuclar.Count / 3);
                break;
            case 2: 
                hedefIndex = Random.Range(olasiSonuclar.Count / 3, 2 * olasiSonuclar.Count / 3);
                break;
            case 3: 
                hedefIndex = Random.Range(2 * olasiSonuclar.Count / 3, olasiSonuclar.Count);
                break;
            default:
                hedefIndex = Random.Range(0, olasiSonuclar.Count);
                break;
        }
        
        hedefSayi = olasiSonuclar[hedefIndex];
        HedefText.text = "Hedef: " + hedefSayi.ToString();
        SonucText.text = "Sonuç: 0";
    }

    void KupleriDuzenle()
    {
        float aralik = 2f;
        float baslangicX = -(kuplar.Count - 1) * aralik / 2f;
        
        for(int i = 0; i < kuplar.Count; i++)
        {
            Vector3 yeniPozisyon = new Vector3(baslangicX + i * aralik, -2f, 0);
            kuplar[i].transform.position = yeniPozisyon;
        }
    }

    public void KupSecildi(KupScript kup)
    {
        if(oyunBitti) return;
        
        if(kup.sayi == hedefSayi)
        {
            SesEfektiCal(tiklamaSesi);
            kup.Sec();
            OyunuBitir("Tebrikler!\nHedef sayıya ulaştınız!", Color.green);
            return;
        }
        
        if(secilenSayi1 == -1)
        {
            SesEfektiCal(tiklamaSesi);
            secilenSayi1 = kup.sayi;
            secilenKup1 = kup;
            kup.Sec();
            durumText.text = "İşlem seçin";
            IslemButonlariniGoster(true);
        }
        else if(secilenIslem != "" && secilenSayi2 == -1)
        {
            SesEfektiCal(tiklamaSesi);
            secilenSayi2 = kup.sayi;
            secilenKup2 = kup;
            kup.Sec();
            IslemYap();
        }
    }

    void IslemSec(string islem)
    {
        secilenIslem = islem;
        IslemButonlariniGoster(false);
        durumText.text = "İkinci sayıyı seçin";
    }

    void IslemButonlariniGoster(bool goster)
    {
        toplamaButton.gameObject.SetActive(goster);
        cikarmaButton.gameObject.SetActive(goster);
        carpmaButton.gameObject.SetActive(goster);
        bolmeButton.gameObject.SetActive(goster);
    }

    void IslemYap()
    {
        if(secilenSayi1 != -1 && secilenSayi2 != -1 && secilenIslem != "")
        {
            int sonuc = 0;
            bool gecerliIslem = true;
            
            switch(secilenIslem)
            {
                case "+":
                    sonuc = secilenSayi1 + secilenSayi2;
                    break;
                case "-":
                    sonuc = secilenSayi1 - secilenSayi2;
                    break;
                case "×":
                    sonuc = secilenSayi1 * secilenSayi2;
                    break;
                case "÷":
                    if(secilenSayi2 != 0 && secilenSayi1 % secilenSayi2 == 0)
                    {
                        sonuc = secilenSayi1 / secilenSayi2;
                    }
                    else
                    {
                        gecerliIslem = false;
                        durumText.text = "Geçersiz bölme işlemi!\nBaşka bir işlem seçin";
                        durumText.color = Color.red;
                    }
                    break;
            }
            
            if(gecerliIslem)
            {
                hamle++;
                secilenKup1.Kullan();
                secilenKup2.Kullan();
                
            
                GameObject yeniKup = Instantiate(kupPrefab, Vector3.zero, Quaternion.identity, kuplerinParenti);
                KupScript yeniKupScript = yeniKup.GetComponent<KupScript>();
                yeniKupScript.SayiyiAyarla(sonuc);
                kuplar.Add(yeniKupScript);
                
            
                kuplar.Remove(secilenKup1);
                kuplar.Remove(secilenKup2);
                
                
                KupleriDuzenle();
                
                
                if(sonuc == hedefSayi)
                {
                    OyunuBitir("Tebrikler!\nHedef sayıya ulaştınız!", Color.green);
                }
                else if(TumKuplerKullanildiMi())
                {
                    OyunuBitir("Kaybettiniz!", Color.red);
                }
                else
                {
                    durumText.text = "Devam edin!\nYeni sayı seçin";
                }
                
                SonucText.text = "Sonuç: " + sonuc.ToString();
            }
            
            
            secilenSayi1 = -1;
            secilenSayi2 = -1;
            secilenKup1 = null;
            secilenKup2 = null;
            secilenIslem = "";
        }
    }

    bool TumKuplerKullanildiMi()
    {
        return kuplar.Count <= 1;
    }

    void OyunuBitir(string mesaj, Color renk)
    {
        oyunBitti = true;
        
        if(renk == Color.green)
        {
            SesEfektiCal(basarmaSesi);
            foreach(var kup in kuplar)
            {
                kup.KazanmaAnimasyonu();
            }
            
            int kazanilanPuan = 10;
            
            if(hamle <= 2) kazanilanPuan += 15;
            else if(hamle <= 3) kazanilanPuan += 10;
            else if(hamle <= 4) kazanilanPuan += 5;
            
            kazanilanPuan += (zorlukSeviyesi - 1) * 5;
            
            puan += kazanilanPuan;
            mesaj += $"\nKazanılan Puan: {kazanilanPuan}";
            
            if(puan > enYuksekPuan)
            {
                enYuksekPuan = puan;
                PlayerPrefs.SetInt("EnYuksekPuan", enYuksekPuan);
                mesaj += "\nYeni En Yüksek Puan!";
                EnYuksekPuaniGoster();
            }
        }
        else
        {
            SesEfektiCal(kaybetmeSesi);
            foreach(var kup in kuplar)
            {
                kup.KaybetmeAnimasyonu();
            }
        }
        
        durumText.text = mesaj;
        durumText.color = renk;
        yenidenBaslatButton.gameObject.SetActive(true);
        IslemButonlariniGoster(false);
        
        PuaniGoster();
    }

    void YenidenBaslat()
    {
        ZorlukSeciminiGoster();
    }

    void PuaniGoster()
    {
        puanText.text = "Puan: " + puan;
    }

    void EnYuksekPuaniGoster()
    {
        enYuksekPuanText.text = "En Yüksek: " + enYuksekPuan;
    }

    public int ZorlukSeviyesiniAl()
    {
        return zorlukSeviyesi;
    }
}