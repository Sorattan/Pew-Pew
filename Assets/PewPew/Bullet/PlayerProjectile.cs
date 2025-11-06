using UnityEngine;
public class PlayerProjectile : MonoBehaviour
{
    public int damage = 25;

    private void OnCollisionEnter(Collision collision)
    {
        // --- HATA AYIKLAMA KODU ---
        // Merminin ilk çarptığı objenin adını konsola yazdır
        Debug.Log("PlayerBullet şuna çarptı: " + collision.gameObject.name);
        // ---

        // Bir bota mı çarptık?
        AITarget bot = collision.gameObject.GetComponentInParent<AITarget>();
        if (bot != null)
        {
            // Bot'un TakeDamage fonksiyonunu çağır
            bot.TakeDamage(damage);
        }

        // Mermi neye çarparsa çarpsın kendini yok etsin
        Destroy(gameObject);
    }
}