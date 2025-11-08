using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    public int damage = 10; 

    /**
     * Şimdi hasar verme kodunu buraya ekliyoruz.
     */
    private void OnCollisionEnter(Collision collision)
{
    // 1. Konsola çarptığı şeyi yaz
    Debug.Log("BotBullet (OnCollisionEnter) şuna çarptı: " + collision.gameObject.name);

    // 2. 'PlayerHealth' script'ini ara
    PlayerHealth player = collision.gameObject.GetComponentInParent<PlayerHealth>();

    // 3. Script'i bulup bulamadığını KONTROL ET
    if (player != null)
    {
        // EĞER SİSTEM ÇALIŞIYORSA, BU MESAJI GÖRMELİYİZ
        Debug.Log("PlayerHealth script'i BAŞARIYLA BULUNDU. Hasar veriliyor.");
        player.TakeDamage(damage);
    }
    else
    {
        
        Debug.LogWarning("PlayerHealth script'i BULUNAMADI! 'GetComponentInParent' başarısız oldu. 'PlayerArmature' objende 'PlayerHealth.cs' script'i ekli mi?");
    }
    
    // 4. Mermi neye çarparsa çarpsın kendini yok et
    Destroy(gameObject);
}
}