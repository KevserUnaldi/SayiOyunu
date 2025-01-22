using UnityEngine;
using System.Collections;
using TMPro;

public class KupScript : MonoBehaviour
{
    public int sayi;
    private bool secili = false;
    private Material originalMaterial;
    private MeshRenderer meshRenderer;
    private TextMeshPro numberText;
    
    private Vector3 baslangicPozisyonu;
    private bool animasyonDevamEdiyor = false;
    private float animasyonHizi = 2f;
    private float ziplamaYuksekligi = 3f;
    private float dusmeHizi = 15f;

    void Start()
    {
        Debug.Log("KupScript Start başladı");
        
        meshRenderer = GetComponent<MeshRenderer>();
        if(meshRenderer != null)
        {
            originalMaterial = meshRenderer.material;
        }
        
        numberText = GetComponentInChildren<TextMeshPro>();
        if(numberText == null)
        {
            Debug.LogError($"TextMeshPro component'i bulunamadı! GameObject: {gameObject.name}");
            TextMeshPro[] textComponents = GetComponentsInChildren<TextMeshPro>();
            Debug.Log($"Bulunan TextMeshPro sayısı: {textComponents.Length}");
            foreach(var text in textComponents)
            {
                Debug.Log($"Bulunan text: {text.gameObject.name}");
            }
        }
        else
        {
            Debug.Log($"TextMeshPro bulundu. Mevcut text: {numberText.text}");
        }
        
        
        baslangicPozisyonu = transform.position;
    }

    void OnMouseDown()
    {
        if(!secili && !animasyonDevamEdiyor)
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if(gameManager != null)
            {
                gameManager.KupSecildi(this);
            }
        }
    }

    public void SayiyiAyarla(int yeniSayi)
    {
        sayi = yeniSayi;
        Debug.Log($"SayiyiAyarla çağrıldı. Yeni sayı: {yeniSayi}");
        
        if(numberText == null)
        {
            numberText = GetComponentInChildren<TextMeshPro>();
            Debug.Log("TextMeshPro yeniden aranıyor...");
        }
        
        if(numberText != null)
        {
            numberText.text = sayi.ToString();
            Debug.Log($"Sayı ayarlandı. Text: {numberText.text}");
        }
        else
        {
            Debug.LogError($"TextMeshPro hala bulunamadı! GameObject: {gameObject.name}");
        }
    }

    public void Sec()
    {
        secili = true;
        if(meshRenderer != null)
        {
            meshRenderer.material.color = Color.yellow;
        }
    }

    public void Kullan()
    {
        secili = false;
        if(meshRenderer != null)
        {
            meshRenderer.material.color = Color.gray;
        }
        GetComponent<Collider>().enabled = false;
    }

    public void SecimIptal()
    {
        secili = false;
        if(meshRenderer != null && originalMaterial != null)
        {
            meshRenderer.material = originalMaterial;
        }
    }

    public bool SeciliMi()
    {
        return secili;
    }

    public void KazanmaAnimasyonu()
    {
        if (!animasyonDevamEdiyor)
        {
            baslangicPozisyonu = transform.position;
            StartCoroutine(KazanmaAnimasyonuCoroutine());
        }
    }

    private IEnumerator KazanmaAnimasyonuCoroutine()
    {
        animasyonDevamEdiyor = true;
        float gecenZaman = 0;
        float maxYukseklik = ziplamaYuksekligi;
        Vector3 baslangicOlcek = transform.localScale;

        while (gecenZaman < 1.5f)
        {
            gecenZaman += Time.deltaTime * animasyonHizi;
            
            
            float yukseklik = maxYukseklik * (1 - (gecenZaman - 0.5f) * (gecenZaman - 0.5f));
            yukseklik = Mathf.Max(0, yukseklik);
            
            
            float olcekFaktoru = 1 + (0.2f * Mathf.Sin(gecenZaman * 4 * animasyonHizi));
            transform.localScale = baslangicOlcek * olcekFaktoru;
            
            transform.position = baslangicPozisyonu + new Vector3(0, yukseklik, 0);
            transform.Rotate(Vector3.up, 360 * Time.deltaTime * animasyonHizi);
            
            yield return null;
        }

        transform.position = baslangicPozisyonu;
        transform.localScale = baslangicOlcek;
        transform.rotation = Quaternion.identity;
        animasyonDevamEdiyor = false;
    }

    public void KaybetmeAnimasyonu()
    {
        if (!animasyonDevamEdiyor)
        {
            baslangicPozisyonu = transform.position;
            StartCoroutine(KaybetmeAnimasyonuCoroutine());
        }
    }

    private IEnumerator KaybetmeAnimasyonuCoroutine()
    {
        animasyonDevamEdiyor = true;
        float gecenZaman = 0;
        Vector3 baslangicOlcek = transform.localScale;
        float dusmeYuksekligi = 5f;
        
        
        while (gecenZaman < 0.3f)
        {
            gecenZaman += Time.deltaTime * animasyonHizi;
            float yukseklik = Mathf.Sin(gecenZaman * Mathf.PI * 2 * animasyonHizi) * 0.5f;
            transform.position = baslangicPozisyonu + new Vector3(0, yukseklik, 0);
            yield return null;
        }
        
        
        float dusmeZamani = 0;
        Vector3 dusmeBaslangic = transform.position;
        
        while (dusmeZamani < 1f)
        {
            dusmeZamani += Time.deltaTime * animasyonHizi;
            float dusmeY = dusmeYuksekligi * (-dusmeZamani * dusmeZamani);
            
            transform.position = dusmeBaslangic + new Vector3(0, dusmeY, 0);
            transform.Rotate(Vector3.right, dusmeHizi * Time.deltaTime * 90 * animasyonHizi);
            
            yield return null;
        }

        
        transform.position = baslangicPozisyonu;
        transform.rotation = Quaternion.identity;
        transform.localScale = baslangicOlcek * 0.8f;
        
        yield return new WaitForSeconds(0.1f);
        transform.localScale = baslangicOlcek;
        
        animasyonDevamEdiyor = false;
    }
}