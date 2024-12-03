using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PanelManager : MonoBehaviour {

    #region Variables
    private bool paused = false;  // Para saber si el juego está en pausa
    public string menuSceneName = "Menu"; //Menu escena del Menu ;P
    public GameObject pausePanel, panelVictoria;           // Panel de pausa que se activará y desactivará
    #endregion Variables

    #region LocalMethods
    private void Start() {
        // Inicializamos los paneles (asegurándonos que el panel de victoria está desactivado)
        panelVictoria.SetActive(false);
        pausePanel.SetActive(false);  // Inicialmente el panel de pausa está oculto
    }
    void Update() {
        // Activar o desactivar el menú de pausa cuando el jugador presiona "P"
        if (Input.GetKeyDown(KeyCode.P)) {
            if (paused) {
                ResumeGame();

            } else {
                PauseGame();

            }

        }
    }
    #endregion Local Methods

    #region Funciones para Panel Victoria
    // agreggar panel de victoria por un setactive(true); y sus botones y activacion de la escena
    // agregar un tag al jugador en un gameobject vacio el trigger para la activacion del winCondition cuando el vato lo toque


    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") || other.CompareTag("Win"))
            if (panelVictoria != null) {

                panelVictoria = null;
                panelVictoria.SetActive(true);
            }

    }
    #endregion Funciones para Panel Victoria

    #region Funciones para el panel de pausa
    // Activa el menú de pausa
    public void PauseGame() {
        paused = true;
        pausePanel.SetActive(true);  // Activar el panel de pausa
        Time.timeScale = 0f;         // Detiene el tiempo del juego
    }

    // Desactiva el menú de pausa
    public void ResumeGame() {
        paused = false;
        pausePanel.SetActive(false);  // Desactivar el panel de pausa
        Time.timeScale = 1f;          // Reanuda el tiempo del juego
    }
    // Reinicia el nivel actual
    public void RetryGame() {
        Time.timeScale = 1f;  // indicarle que se reanuda desde el 1er momento del juego
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    // Carga la escena del menú principal
    public void GoToMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
    // Salir del juego
    public void GoToExit() {
        // lo mismo pero para el editor, cuando estas en unity
        UnityEditor.EditorApplication.isPlaying = false;
        // para el jugador en in game
        Application.Quit();
    }

    #endregion Funciones para el panel de pausa
}
