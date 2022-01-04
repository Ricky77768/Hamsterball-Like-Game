using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelSelectController : MonoBehaviour {
    public Text levelName;
    public Text levelDiffTime;
    public Text totalSavedTime;
    public Text lockedSymbol;
    public Text lockedPrompt;
    public Animator transition;
    public AudioSource backgroundMusic;
    public AudioSource clickSoundEffect;
    public RawImage background;
    public GameObject playButton;
    public Texture[] backgroundImages;
    public GameObject[] levels;

    private bool firstFrame = true;
    private bool playDisabled = false;
    private bool menuDisabled = false;
    private static float totalTime = 0f;
    private static int currentSelectedLevelID;

    // Start is called before the first frame update
    void Start() {
        Time.timeScale = 1;
        if (PlayerPrefs.HasKey("setting_volume")) {
            backgroundMusic.volume = PlayerPrefs.GetFloat("setting_volume") / 100f;
        }

        if (PlayerPrefs.HasKey("setting_sfx")) {
            clickSoundEffect.volume = PlayerPrefs.GetFloat("setting_sfx") / 100f;
        }
        lockedSymbol.color = Color.clear;
        lockedPrompt.color = Color.clear;
    }

    void Update() {
        if (firstFrame) {
            changeLevelID(1);
            firstFrame = false;
        }
    }

    public void playClickSFX() {
        clickSoundEffect.Play();
    }

    public int getLevelID() {
        return currentSelectedLevelID;
    }

    public float getTotalTime() {
        return totalTime;
    }

    public void resetData() {
        StartCoroutine(clearData());
    }

    IEnumerator clearData() {
        foreach (GameObject level in levels) {
            level.GetComponent<LevelButtons>().clearTime();
        }
        transition.SetTrigger("startTransition");
        yield return new WaitForSecondsRealtime(0.75f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void changeLevelID (int id) {
        currentSelectedLevelID = id;
        updateShownLevel(id);
    }

    private void updateShownLevel(int id) {
        string text = "Sample Text";

        switch (id) {
            case 1:
                levelName.text = "The Beginning";
                text = "Tutorial";
                break;
            case 2:
                levelName.text = "The Forest";
                text = "Easy";
                break;
            case 3:
                levelName.text = "The Desert";
                text = "Easy";
                break;
            case 4:
                levelName.text = "The Tundra";
                text = "Normal";
                break;
            case 5:
                levelName.text = "The Volcano";
                text = "Normal";
                break;
            case 6:
                levelName.text = "The Neon";
                text = "Hard";
                break;
            case 7:
                levelName.text = "The Night";
                text = "Hard";
                break;
            case 8:
                levelName.text = "The Graveyard";
                text = "Expert";
                break;
            case 9:
                levelName.text = "The Space";
                text = "Expert";
                break;
            case 10:
                levelName.text = "The End";
                text = "???";
                break;
            default:
                Debug.Log("updateShownLevel() goofed");
                break;
        }
        levelDiffTime.text = text + " | Best Save: " + levels[id - 1].GetComponent<LevelButtons>().getTime() + "s";
        background.texture = backgroundImages[id - 1];

        if (levels[id - 1].GetComponent<LevelButtons>().getLocked()) {
            playDisabled = true;
            playButton.GetComponent<Button>().interactable = false;
            playButton.GetComponent<CanvasGroup>().alpha = 0.3f;
            background.color = Color.grey;
            lockedPrompt.text = "Reach " + levels[id - 1].GetComponent<LevelButtons>().requiredTime + "s to unlock!";
            lockedSymbol.color = Color.yellow;
            lockedPrompt.color = Color.yellow;
        } else {
            playDisabled = false;
            playButton.GetComponent<Button>().interactable = true;
            playButton.GetComponent<CanvasGroup>().alpha = 1f;
            background.color = Color.white;
            lockedSymbol.color = Color.clear;
            lockedPrompt.color = Color.clear;
        }

        float total = 0f;
        foreach (GameObject level in levels) {
            total += level.GetComponent<LevelButtons>().getTime();
        }
        totalTime = total;
        totalSavedTime.text = totalTime.ToString();
    }

    public void fadeMusic() {
        StartCoroutine(Utilities.audioFade(backgroundMusic, 0.5f, 0));
    }

    public void startLevel() {
        if (menuDisabled || playDisabled) { return; }
        menuDisabled = true;
        StartCoroutine(loadLevel(currentSelectedLevelID));
    }

    IEnumerator loadLevel(int level) {
        transition.SetTrigger("startTransition");
        yield return new WaitForSecondsRealtime(0.75f);
        SceneManager.LoadScene("Level " + level);
    }

    // TESTING ONLY
    public void startDebug() {
        if (menuDisabled || playDisabled) { return; }
        menuDisabled = true;
        StartCoroutine(loadDebug());
    }

    // TESTING ONLY
    IEnumerator loadDebug() {
        transition.SetTrigger("startTransition");
        yield return new WaitForSecondsRealtime(0.75f);
        SceneManager.LoadScene("Test");
    }

    public void returnToMainMenu() {
        menuDisabled = true;
        StartCoroutine(loadMainMenu());
    }
    IEnumerator loadMainMenu() {
        transition.SetTrigger("startTransition");
        yield return new WaitForSecondsRealtime(0.75f);
        SceneManager.LoadScene("Main Menu");
    }

}
