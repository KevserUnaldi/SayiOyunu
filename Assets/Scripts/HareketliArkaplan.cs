using UnityEngine;

public class HareketliArkaplan : MonoBehaviour
{
    public float renkDegisimHizi = 0.3f;    
    public float parlaklik = 0.8f;        
    public float doygunluk = 0.3f;        
    private Material arkaplanMaterial;
    private float zaman;

    void Start()
    {
        arkaplanMaterial = new Material(Shader.Find("Unlit/Color"));
        GetComponent<Renderer>().material = arkaplanMaterial;
    }

    void Update()
    {
        zaman += Time.deltaTime * renkDegisimHizi;
        
        
        Color yeniRenk = new Color(
            Mathf.Sin(zaman) * doygunluk + parlaklik,           
            Mathf.Sin(zaman + 2.0944f) * doygunluk + parlaklik, 
            Mathf.Sin(zaman + 4.1888f) * doygunluk + parlaklik  
        );
        
        arkaplanMaterial.color = yeniRenk;
    }
}