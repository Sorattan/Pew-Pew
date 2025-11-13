using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject K;
    public GameObject Smenu;
    public Slider Sldr;

    public float SensK;
    
    public void Cikis()
    {
        Application.Quit();
    }
    public void Başla()
    {
        SceneManager.LoadScene(1);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    void Setting(GameObject kapanacakolan, GameObject acilacakolan)
    {
        kapanacakolan.SetActive(false);
        acilacakolan.SetActive(true);
        Smenu.SetActive(true);
    }

    public void Settings()
    {
        Setting(K, Smenu);
    }

    private void Update()
    {
        if (Sldr != null)
        {

            SensK = Sldr.value;

            PlayerPrefs.SetFloat("Sens", SensK);

            print(PlayerPrefs.GetFloat("Sens"));
        }
    }

    public void GeriD()
    {
        Smenu.SetActive(false);
        K.SetActive(true);
    }


}


