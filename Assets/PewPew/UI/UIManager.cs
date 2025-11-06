using UnityEngine;
using UnityEngine.SceneManagement; // Sahne yönetimi için bu kütüphane şart!
using UnityEngine.UI; // UI elemanları için bu kütüphane şart!

public class UIManager : MonoBehaviour
{
    // Inspector'dan GameOverPanel'imizi buraya sürükleyeceğiz
    public GameObject gameOverPanel;

    // Bu fonksiyon, GameOverPanel'i aktif hale getirecek
    public void ShowGameOverScreen()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    // Bu fonksiyon, butona tıklandığında çalışacak
    public void RestartGame()
    {
        // O anda aktif olan sahneyi yeniden yükler
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}