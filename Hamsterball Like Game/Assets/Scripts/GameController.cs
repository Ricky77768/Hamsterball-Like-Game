using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour {

    public GameObject ball;
    public GameObject mainCamera;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject levelCompleteMenu;
    public GameObject countdownObjects;
    public Canvas UI;
    public Text fpsDisplay;
    public Text numberCountdown;
    public Animator transition;
    public AudioSource countdown;
    public AudioSource ballDizzy;
    public AudioSource ballBreak;
    public AudioSource backgroundMusic;
    public AudioSource victorySound;
    public AudioSource defeatSound;
    private bool menuDisabled = false;
    private float timer;

    void Start() {
        Time.timeScale = 1;
        gameOverMenu.SetActive(false);
        levelCompleteMenu.SetActive(false);
        StartCoroutine(countDown());
    }

    void Update() {
        if (Time.unscaledTime > timer) {
            int fps = (int) (1f / Time.unscaledDeltaTime);
            fpsDisplay.text = "FPS: " + fps;
            timer = Time.unscaledTime + 0.5f;
        }
    }

    IEnumerator countDown() {
        countdown.Play();
        numberCountdown.text = "3";
        yield return new WaitForSeconds(1f);
        numberCountdown.text = "2";
        yield return new WaitForSeconds(1f);
        numberCountdown.text = "1";
        yield return new WaitForSeconds(1f);
        numberCountdown.text = "Go!";
        ball.GetComponent<BallController>().toggleBallControls(true);
        if (GetComponent<TimerController>() != null) {
            GetComponent<TimerController>().toggleTimer(true);
        }
        yield return new WaitForSeconds(1f);
        countdownObjects.SetActive(false);
    }

    public void pauseGame() {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void resumeGame() {
        if (menuDisabled) { return; }
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void endGame() {
        Cursor.lockState = CursorLockMode.None;
        ball.GetComponent<BallController>().toggleBallControls(false);
        gameOverMenu.SetActive(true);
        defeatSound.Play();
        StartCoroutine(Utilities.audioFade(backgroundMusic, 2f, PlayerPrefs.GetFloat("setting_volume") / 300f));
    }

    public void completeGame() {
        Cursor.lockState = CursorLockMode.None;
        ball.GetComponent<BallController>().toggleBallControls(false);
        UI.GetComponent<TimerController>().toggleTimer(false);
        UI.GetComponent<TimerController>().updateBestTime();
        levelCompleteMenu.SetActive(true);
        victorySound.Play();
        StartCoroutine(Utilities.audioFade(backgroundMusic, 2f, PlayerPrefs.GetFloat("setting_volume") / 300f));
    }

    public void reloadLevel() {
        if (menuDisabled) { return; }
        menuDisabled = true;
        Time.timeScale = 1;
        UI.GetComponent<TimerController>().toggleTimer(false);
        StartCoroutine(restartLevel());
    }

    IEnumerator restartLevel() {
        transition.SetTrigger("startTransition");
        yield return new WaitForSecondsRealtime(0.75f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void returnToLevelSelect() {
        if (menuDisabled) { return; }
        menuDisabled = true;
        Time.timeScale = 1;
        UI.GetComponent<TimerController>().toggleTimer(false);
        StartCoroutine(loadLevelSelect());
    }

    IEnumerator loadLevelSelect() {
        transition.SetTrigger("startTransition");
        yield return new WaitForSecondsRealtime(0.75f);
        SceneManager.LoadScene("Level Select");
    }

    public void returnToMainMenu() {
        if (menuDisabled) { return; }
        menuDisabled = true;
        Time.timeScale = 1;
        UI.GetComponent<TimerController>().toggleTimer(false);
        StartCoroutine(loadMainMenu());
    }

    IEnumerator loadMainMenu() {
        transition.SetTrigger("startTransition");
        yield return new WaitForSecondsRealtime(0.75f);
        SceneManager.LoadScene("Main Menu");
    }

    public void playBallDizzy() {
        if (!ballDizzy.isPlaying) {
            ballDizzy.Play();
        }
    }

    public void playBallBreak() {
        if (!ballBreak.isPlaying) {
            ballBreak.Play();
        }
    }
}