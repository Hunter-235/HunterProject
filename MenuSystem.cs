using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.XR.WSA;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class MenuSystem : MonoBehaviour {

    //
    public static bool GameIsPaused = false;
    
    //
    public Text text_sfx, text_music;
    public GameObject handle_sfx, handle_music, pauseMenuUI;
    public AudioMixer MyAudioMixer;

    //
    public Dropdown resolutionDropdown;
    Resolution[] resolutions;

    // Inputs
    private Keyboard keyboard;              // Keyboard
    private Gamepad pad;                    // XBOX Controller
    private float key_esc, start_button;    // Keys

    private void Start() {
        StartResolution();
    }

    private void Update() {
        Input();
        if ((key_esc >= 1.0f) || (start_button >= 1.0f)) {
            if (GameIsPaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    /** SALTO DE ESCENAS **/
    public void NewGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void MainMenu() {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void TestMap() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
    }

    public void QuitGame() {
        Application.Quit();
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - // 

    /** SONIDO (Musica y Sfx) **/
    public void SetMusicVolume(float music) {
        RectTransform rect_music = handle_music.GetComponent<RectTransform>();
        float music_pc = rect_music.anchorMin.x * 100.0f;
        text_music.text = music_pc.ToString("0") + "%";
        MyAudioMixer.SetFloat("Mixer_MusicVolume", music);
    }
    public void SetSFXVolume(float sfx) {
        RectTransform rect_sfx = handle_sfx.GetComponent<RectTransform>();
        float sfx_pc = rect_sfx.anchorMin.x * 100.0f;
        text_sfx.text = sfx_pc.ToString("0") + "%";
        MyAudioMixer.SetFloat("Mixer_SFXVolume", sfx);
    }

    /** GRAFICOS **/
    private void StartResolution () {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height) {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex) {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void setAntiAliasingLevel(int AntiAliasQuality) {
        // QualitySettings.antiAliasing = (AntiAliasQuality);
    }

    public void SetFullScreen (bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - // 

    // Menu Pausa
    public void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
    }

    void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        GameIsPaused = true;
    }

    // Inputs
    private void Input() {

        // Gamepad pad = InputSystem.GetDevice<Gamepad>();
        Keyboard keyboard = InputSystem.GetDevice<Keyboard>();

        key_esc = keyboard.escapeKey.ReadValue();               // Tecla E
        // start_button = pad.startButton.ReadValue();             // Button B

    }

}
