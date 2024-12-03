using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour {
    #region Variables

    [SerializeField] Button Gameplay, exit;

    #endregion Variables

    // Métodos para cambiar entre escenas
    #region Funciones Publicas 

    public void LoadMenuPrincipal() {
        SceneManager.LoadScene("Menu");
    }
    public void LoadGameplay() {
        SceneManager.LoadScene("Juego");
    }
    public void QuitGame() {

        // Si estás en el editor de Unity, detiene el modo de juego
        UnityEditor.EditorApplication.isPlaying = false;

        Application.Quit();
    }

    #endregion Funciones Publicas

}
