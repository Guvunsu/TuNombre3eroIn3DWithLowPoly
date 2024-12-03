using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    // A VER SI ME DA TIEMPO DE AGREGARSELO
    // HACERLE SI LO HAGO :
    // CONTANDOR DE VIDAS O INTENTOS DE RESPAWN 
    //IGUALARLO DE QUE SI LLEVA TANTAS VECES MUERTO 
    //SETACTIVE PANEL ACTIVADO Y REFERENCIAR ESTE SCRIPT ES ESE OTRO 

    //public bool losePanelIsOpen = false;
    public bool winPanelIsOpen = false;

    public bool isGameWin = false;
    // private bool isGameLose = false;

    void Start() {
        // losePanelIsOpen = false;
        isGameWin = false;
    }

    public bool IsGameWin {
        get => isGameWin;
        set => isGameWin = value;
    }

    //public bool IsGameLose {
    //    get => IsGameLose;
    //    set => isGameLose = value;


    //}
    private void Awake() {

        //sirve para que no me destrruyan las escenas de los juegos y los llama

        if (Instance == null) {
            Instance = this;
            // music();
        } else {
            Destroy(this);
        }
        DontDestroyOnLoad(this.gameObject);

    }
    public void sceneSwitch(string sceneName) {

        //esto me hace cambiar de UI´s 

        // losePanelIsOpen = false;
        winPanelIsOpen = false;
        isGameWin = false;
        //isGameLose = false;
        SceneManager.LoadScene(sceneName);
    }
    //SI ME DA TIEMPO LO AGREGO AL GAMEPLAY
    //public void activateLosePanel() {
    //    //esta sirve para activar la escena de muerte/derrota cuando hayas perdido

    //    losePanelIsOpen = true;
    //}

    public void activateWinPanel() {
        winPanelIsOpen = true;
    }


}
