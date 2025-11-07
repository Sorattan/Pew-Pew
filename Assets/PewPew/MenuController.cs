using UnityEngine.UI;
using UnityEditor.SettingsManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject K;
    public GameObject Panel;
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

    void Setting(GameObject kapanacakolan, GameObject acilacakolan, GameObject panel)
    {
        kapanacakolan.SetActive(false);
        acilacakolan.SetActive(true);
        panel.SetActive(false);
        Smenu.SetActive(true);
    }

    public void Settings()
    {
        Setting(K, Smenu, Panel);
    }

    private void Update()
    {
        SensK = Sldr.value;

        PlayerPrefs.SetFloat("Sens", SensK);

        print(PlayerPrefs.GetFloat("Sens"));
    }

    public void GeriD()
    {
        Panel.SetActive(true);
        Smenu.SetActive(false);
        K.SetActive(true);
    }


}


