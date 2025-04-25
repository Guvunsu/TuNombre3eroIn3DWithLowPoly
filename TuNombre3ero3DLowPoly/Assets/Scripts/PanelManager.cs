using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;


public class PanelManager : MonoBehaviour {
    #region Variables
    private bool paused = false;
    public string menuSceneName = "Menu"; //Menu escena del Menu ;P

    public GameObject pausePanel, panelVictoria;
    public GameObject panelUI;


    #endregion Variables

    #region LocalMethods
    void Start() {
        panelVictoria.SetActive(false);
        pausePanel.SetActive(false);
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            if (paused) {
                ResumeGame();
            }
        }
    }

    #endregion Local Methods

    #region Funciones para Panel Victoria
    // agreggar panel de victoria por un setactive(true); y sus botones y activacion de la escena
    // agregar un tag al jugador en un gameobject vacio el trigger para la activacion del winCondition cuando el vato lo toque
    public void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            panelVictoria.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    #endregion Funciones para Panel Victoria

    #region Ui Mensaje
    public void CerrarMensaje() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            panelUI.SetActive(false);
        }
    }

    #endregion Ui Mensaje

    #region Funciones para el panel de pausa
    //public void ActivationPanel(InputAction.CallbackContext value) {
    //    if (value.performed && paused) {
    //        ResumeGame();
    //    } else {
    //        PauseGame();
    //    }
    //}
    public void PauseGame() {
        paused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ResumeGame() {
        paused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void RetryGame() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoToMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
    public void GoToExit() {
        //// lo mismo pero para el editor, cuando estas en unity
        //UnityEditor.EditorApplication.isPlaying = false;

        // para el jugador en in game
        Application.Quit();
    }

    #endregion Funciones para el panel de pausa
}
