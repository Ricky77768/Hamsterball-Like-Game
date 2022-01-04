using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    // Timer
    public int levelID = 1;
    public float maxTime = 100f;
    public Canvas UI;
    public Text timerS;
    public Text timerMS;
    public Text extraTime;
    public Text recordTime;
    public Text newRecordText;
    public RawImage timeDeco;

    private float timeRemain;
    private float bestTime;
    private bool levelActive = false;
    private bool bonusActive;
    private Vector3 bonusOrigin;
    private Vector3 bonusNew;
    private Color lerpedYellow;
    private Color lerpedRed;

    // Start is called before the first frame update
    void Start() {
        timeRemain = maxTime;
        bestTime = PlayerPrefs.GetFloat("level" + levelID + "_time");
        recordTime.text = "Best: " + bestTime.ToString() + "s";
        timerS.text = ((int) maxTime).ToString();
        timerMS.text = "0";
        lerpedYellow = Color.yellow;
        lerpedRed = Color.red;
        newRecordText.enabled = false;
        extraTime.enabled = false;
        bonusActive = false;

        // Set animation Vector3
        bonusOrigin = extraTime.rectTransform.anchoredPosition;
        extraTime.rectTransform.anchoredPosition += new Vector2(-40f, 0);
        bonusNew = extraTime.rectTransform.anchoredPosition;
        extraTime.rectTransform.anchoredPosition += new Vector2(40f, 0);
    }

    // Update is called once per frame
    void Update() {
        if (!levelActive) { return; }

        int secondsRemain = (int)timeRemain;
        timeRemain -= Time.deltaTime;
        if (timeRemain <= 0f) {
            timeRemain = 0;
            levelActive = false;
            UI.GetComponent<GameController>().endGame();
        }
        timerS.text = secondsRemain.ToString("00");
        timerMS.text = ((int)((timeRemain - secondsRemain) * 10)).ToString();

        if (bonusActive) { return; }

        if (timeRemain < 10f) {
            lerpedRed = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 1f));
            timerS.color = lerpedRed;
            timerMS.color = lerpedRed;
            timeDeco.color = lerpedRed;
        } else if (timeRemain < 30f) {
            lerpedYellow = Color.Lerp(Color.white, Color.yellow, Mathf.PingPong(Time.time, 1f));
            timerS.color = lerpedYellow;
            timerMS.color = lerpedYellow;
            timeDeco.color = lerpedYellow;
        } else {
            timerS.color = Color.white;
            timerMS.color = Color.white;
            timeDeco.color = Color.white;
        }
    }

    public void toggleTimer(bool state) {
        levelActive = state;
    }

    public void updateBestTime() {
        if (timeRemain > bestTime) {
            newRecordText.enabled = true;
            PlayerPrefs.SetFloat("level" + levelID + "_time", Mathf.Floor(timeRemain * 10f) / 10f);
        }
    }

    // Add time to clock with animation
    public void bonusTime(int amount) {
        StartCoroutine(addTime(amount));
    }

    IEnumerator addTime(int amount) {
        timeRemain += amount;
        bonusActive = true;
        extraTime.text = "+" + amount + "s!";
        extraTime.enabled = true;

        for (int i = 0; i < 50; i++) {
            Color c = new Color((float)(0.4 + 0.012 * i), 1f, (float)(0.4 + 0.012 * i), 1f);
            extraTime.color = c;
            timerS.color = c;
            timerMS.color = c;
            timeDeco.color = c;
            yield return new WaitForSeconds(0.01f);
        }
        bonusActive = false;
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 100; i++) {
            float t = i / 100f;
            t = 1 - Mathf.Pow(1 - t, 2);
            extraTime.color = new Color(1f, 1f, 1f, (float)(1 - 0.01 * i));
            extraTime.rectTransform.anchoredPosition = Vector3.Lerp(bonusOrigin, bonusNew, t);
            yield return new WaitForSeconds(0.002f);
        }
        extraTime.enabled = false;
        extraTime.rectTransform.anchoredPosition = bonusOrigin;
    }
}
