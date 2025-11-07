using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void Cikis(){
        Application.Quit();
    }
    public void Başla()
    {
        SceneManager.LoadScene(1);
    }
    public void MainMenu(){
        SceneManager.LoadScene(0);
    }
}

