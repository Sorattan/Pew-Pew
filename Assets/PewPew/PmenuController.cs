using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PmenuController : MonoBehaviour
{
    bool Pmenu;

    public GameObject PauseScreen;
    
    private void Start(){
        Pmenu = false;
    }
    private void Update(){
        if(Pmenu== false){

        }

        if(Input.GetKeyDown(KeyCode.Escape)){
            Pmenu = true;
        }

        if(Pmenu){
            PauseScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if(Input.GetKeyDown(KeyCode.Escape)){
                Pmenu= false;

            }
        }
    }

    public void Devam(){
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        PauseScreen.SetActive(false);
    }
    
}
