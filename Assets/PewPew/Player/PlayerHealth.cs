using UnityEngine;
using UnityEngine.UI; 
using StarterAssets;

public class PlayerHealth : MonoBehaviour
{
    // === DEĞİŞKENLER ===
    public int maxHealth = 100; // Maksimum can
    private int currentHealth;  // Mevcut can
    public Image healthBarFill;
    private Animator animator;

    // Opsiyonel: Inspector'dan bir UI Image (Resim) sürükleyebilirsiniz
    // public Image healthBar; 

    
    // === TEMEL FONKSİYONLAR ===

    // Oyun başladığında çalışır
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();


        if (healthBarFill != null)
    {
        healthBarFill.fillAmount = 1f; // '1f' tam dolu demektir
    }
    }

    // Dışarıdan hasar almak için çağrılan public fonksiyon
    public void TakeDamage(int damage)
    {
        // Eğer can 0'ın altındaysa (zaten ölmüşse) bu fonksiyondan çık
        if (currentHealth <= 0) return;

        // Canı azalt
        currentHealth -= damage;

        // Konsola mevcut canı yaz (test için)
        Debug.Log("Oyuncu Canı: " + currentHealth);
       if (healthBarFill != null)
    {
        // Mevcut canı, maksimum cana bölerek 0-1 arası bir değer bul (float)
        healthBarFill.fillAmount = (float)currentHealth / maxHealth;
    }

        // Eğer can 0 veya daha az ise Ölme fonksiyonunu çağır
        if (currentHealth <= 0)
        {
            PlayerDie();
        }
    }

    // Can 0'a ulaştığında çalışır
    private void PlayerDie()
{
    Debug.Log("OYUNCU ÖLDÜ!");

    
    // Eğer bir Animator bulduysak, "Die" trigger'ını ateşle
    if (animator != null)
    {
        animator.SetTrigger("Die");
    }
    // ---

    // UIManager'ı bul ve "Öldün" ekranını göstermesini söyle
    UIManager uiManager = FindFirstObjectByType<UIManager>();
    if (uiManager != null)
    {
        uiManager.ShowGameOverScreen();
    }

    // Oyuncunun hareket etmesini ve ateş etmesini durdur
    GetComponent<ThirdPersonController>().enabled = false;
    GetComponent<ThirdPersonShooterController>().enabled = false;

    // Farenin kilidini aç ve görünür yap
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

    this.enabled = false; 
}

    

}