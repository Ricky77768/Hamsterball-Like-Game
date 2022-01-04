using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour {
    public GameObject mainCamera;
    public GameObject playButton;
    public GameObject retryButton;
    public GameObject optionButton;
    public GameObject levelSelectButton;
    public GameObject exitButton;
    public GameObject optionsMenu;
    public GameObject controlsSubmenu;
    public GameObject displaySubmenu;
    public GameObject soundSubMenu;
    public GameObject mouseButton;
    public GameObject keyboardButton;
    public GameObject sfxEmitter;
    public Animator transition;
    public AudioSource backgroundMusic;
    public AudioSource clickSoundEffect;
    public Text resolutionText;
    public Slider volumeSlider;
    public Slider sfxSlider;
    public Slider sensitivitySlider;
    public RawImage cursor;
    public int cursorSize;

    private static float sensitivity = 1f;
    private bool menuDisabled = false;
    private static bool mouseEnabled = true;
    private static bool keyboardEnabled = true;
    private int defaultWidth;
    private int defaultHeight;
    private Vector3 originalCursorPos;
    private Vector2 keyMove = Vector2.zero;
    private Vector2 mouseMove = Vector2.zero;
    private Vector2 overallMove = Vector2.zero;

    void Start() {
        Time.timeScale = 1;
        optionsMenu.SetActive(false);
        originalCursorPos = cursor.rectTransform.anchoredPosition;
        defaultWidth = Screen.currentResolution.width;
        defaultHeight = Screen.currentResolution.height;
        PlayerPrefs.SetInt("setting_width", defaultWidth);
        PlayerPrefs.SetInt("setting_height", defaultHeight);
        importPlayerSettings();
    }

    void Update() {
        if (keyboardEnabled) {
            keyMove.x = Input.GetAxis("Horizontal");
            keyMove.y = Input.GetAxis("Vertical");
            if (keyMove.magnitude > 1f) {
                keyMove.Normalize();
            }
        }
        
        if (mouseEnabled) {
            mouseMove.x = Input.GetAxis("Mouse X") * sensitivity;
            mouseMove.y = Input.GetAxis("Mouse Y") * sensitivity;
            if (mouseMove.magnitude > 1f) {
                mouseMove.Normalize();
            }
        }
        overallMove = keyMove + mouseMove;
        if (overallMove.magnitude > 1f) {
            overallMove.Normalize();
        }
        cursor.rectTransform.anchoredPosition = originalCursorPos + new Vector3(overallMove.x, overallMove.y, 0) * cursorSize;
    }

    public void importPlayerSettings() {
        if (PlayerPrefs.HasKey("setting_mouse_enabled")) {
            setMouse(PlayerPrefs.GetInt("setting_mouse_enabled"));
        }

        if (PlayerPrefs.HasKey("setting_mouse_sensitivity")) {
            sensitivitySlider.value = PlayerPrefs.GetFloat("setting_mouse_sensitivity");
            sensitivity = PlayerPrefs.GetFloat("setting_mouse_sensitivity");
        }

        if (PlayerPrefs.HasKey("setting_keyboard_enabled")) {
            setKeyboard(PlayerPrefs.GetInt("setting_keyboard_enabled"));
        }

        if (PlayerPrefs.HasKey("setting_resolution_index")) {
            changeResolution(PlayerPrefs.GetInt("setting_resolution_index"));
        }

        if (PlayerPrefs.HasKey("setting_volume")) {
            volumeSlider.value = PlayerPrefs.GetFloat("setting_volume");
            backgroundMusic.volume = volumeSlider.value / 100f;
        }

        if (PlayerPrefs.HasKey("setting_sfx")) {
            sfxSlider.value = PlayerPrefs.GetFloat("setting_sfx");
            clickSoundEffect.volume = sfxSlider.value / 100f;
        }
    }

    public void updateInGameControls() {
        GameObject ball = GameObject.FindGameObjectWithTag("Player");
        if (ball != null) {
            ball.GetComponent<BallController>().updateControls();
        } else {
            Debug.Log("ERROR: updateInGameControls() goofed");
        }
    }

    public void playClickSFX() {
        clickSoundEffect.Play();
    }

    public void fadeMusic() {
        StartCoroutine(Utilities.audioFade(backgroundMusic, 0.5f, 0));
    }

    public void startLevelSelect() {
        menuDisabled = true;
        StartCoroutine(loadLevelSelect());
    }

    IEnumerator loadLevelSelect() {
        transition.SetTrigger("startTransition");
        yield return new WaitForSecondsRealtime(0.75f);
        SceneManager.LoadScene("Level Select");
    }

    public void endGame() {
        if (menuDisabled) { return; }
        Application.Quit();
    }

    public void showCredits() {
        Application.OpenURL("https://github.com/Ricky77768/Hamsterball-Like-Game/blob/main/Credits.md");
    }

    public void showOptions() {
        if (menuDisabled) { return; }
        playButton.SetActive(false);
        retryButton.SetActive(false);
        optionButton.SetActive(false);
        levelSelectButton.SetActive(false);
        exitButton.SetActive(false);
        optionsMenu.SetActive(true);
        switchToControlMenu();
    }

    public void hideOptions() {
        playButton.SetActive(true);
        retryButton.SetActive(true);
        optionButton.SetActive(true);
        levelSelectButton.SetActive(true);
        exitButton.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void switchToControlMenu() {
        controlsSubmenu.SetActive(true);
        displaySubmenu.SetActive(false);
        soundSubMenu.SetActive(false);
    }

    public void switchToDisplayMenu() {
        controlsSubmenu.SetActive(false);
        displaySubmenu.SetActive(true);
        soundSubMenu.SetActive(false);
    }

    public void switchToSoundMenu() {
        controlsSubmenu.SetActive(false);
        displaySubmenu.SetActive(false);
        soundSubMenu.SetActive(true);
    }

    public void changeResolution(int index) {
        int width = defaultWidth;
        int height = defaultHeight;
        bool isFullscreen = false;

        switch (index) {
            case 1:
                width = 1920;
                height = 1080;
                break;
            case 2:
                width = 1366;
                height = 768;
                break;
            case 3:
                width = 1280;
                height = 720;
                break;
            case 4:
                width = 1680;
                height = 1050;
                break;
            case 5:
                width = 1440;
                height = 900;
                break;
            case 6:
                width = 1280;
                height = 800;
                break;
            case 7:
                width = 1024;
                height = 768;
                break;
            case 8:
                width = 800;
                height = 600;
                break;
            case 9:
                width = 640;
                height = 480;
                break;
            case 10:
                isFullscreen = true;
                break;
            default:
                Debug.Log("ERROR: changeResolution() goofed");
                break;
        }

        PlayerPrefs.SetInt("setting_resolution_index", index);
        Screen.SetResolution(width, height, isFullscreen);
        if (isFullscreen) {
            resolutionText.text = "Current Resolution: " + width + " x " + height + " (Fullscreen)";
        } else {
            resolutionText.text = "Current Resolution: " + width + " x " + height;
        }
    }

    public void setVolume() {
        PlayerPrefs.SetFloat("setting_volume", volumeSlider.value);
        Text t = volumeSlider.GetComponentsInChildren<Text>()[0];
        t.text = volumeSlider.value.ToString();
        if (backgroundMusic != null) {
            backgroundMusic.volume = volumeSlider.value / 100f;
        }
    }

    public void setSFX() {
        PlayerPrefs.SetFloat("setting_sfx", sfxSlider.value);
        Text t = sfxSlider.GetComponentsInChildren<Text>()[0];
        t.text = sfxSlider.value.ToString();
        if (sfxEmitter != null) {
            AudioSource[] sfxs = sfxEmitter.GetComponentsInChildren<AudioSource>();

            foreach (AudioSource obj in sfxs) {
                obj.volume = sfxSlider.value / 100f;
            }
        }
    }

    public void setSensitivity() {
        PlayerPrefs.SetFloat("setting_mouse_sensitivity", sensitivitySlider.value);
        Text t = sensitivitySlider.GetComponentsInChildren<Text>()[0];
        t.text = (Mathf.Round(sensitivitySlider.value * 10) / 10.0f).ToString();
        sensitivity = sensitivitySlider.value;
    }

    public void toggleMouse() {
        if (mouseEnabled && !keyboardEnabled) { return; }

        mouseEnabled = !mouseEnabled;
        PlayerPrefs.SetInt("setting_mouse_enabled", (mouseEnabled) ? 1 : 0);
        Text t = mouseButton.GetComponentsInChildren<Text>()[0];
        if (mouseEnabled) {
            t.text = "Mouse: On";
        } else {
            t.text = "Mouse: Off";
        }
    }

    public void toggleKeyboard() {
        if (!mouseEnabled && keyboardEnabled) { return; }

        keyboardEnabled = !keyboardEnabled;
        PlayerPrefs.SetInt("setting_keyboard_enabled", (keyboardEnabled) ? 1 : 0);
        Text t = keyboardButton.GetComponentsInChildren<Text>()[0];
        if (keyboardEnabled) {
            t.text = "Keyboard: On";
        } else {
            t.text = "Keyboard: Off";
        }
    }

    private void setMouse(int x) {
        mouseEnabled = (x == 1);
        Text t = mouseButton.GetComponentsInChildren<Text>()[0];
        if (mouseEnabled) {
            t.text = "Mouse: On";
        } else {
            t.text = "Mouse: Off";
        }
    }

    private void setKeyboard(int x) {
        keyboardEnabled = (x == 1);
        Text t = keyboardButton.GetComponentsInChildren<Text>()[0];
        if (keyboardEnabled) {
            t.text = "Keyboard: On";
        } else {
            t.text = "Keyboard: Off";
        }
    }
}