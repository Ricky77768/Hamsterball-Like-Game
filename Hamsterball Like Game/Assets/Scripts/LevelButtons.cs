using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LevelButtons : MonoBehaviour {
    
    public int levelID;
    public float requiredTime;
    public Text levelText;
    public GameObject UI;
    public Button icon;
    public Sprite normal;
    public Sprite selected;
    public Sprite locked;
    public Sprite lockSelected;

    private float levelTime;
    private bool isLocked;
    private bool isSelected;
    private bool firstFrame = true;

    void Start() {
        levelTime = PlayerPrefs.GetFloat("level" + levelID + "_time");
    }

    void Update() {
        if (firstFrame) {
            levelText.text = levelTime.ToString() + "s";
            firstFrame = false;
        }

        isLocked = UI.GetComponent<LevelSelectController>().getTotalTime() < requiredTime;
        isSelected = UI.GetComponent<LevelSelectController>().getLevelID() == levelID;
        if (isLocked && isSelected) {
            icon.image.sprite = lockSelected;
        } else if (isSelected) {
            icon.image.sprite = selected;
        } else if (isLocked) {
            icon.image.sprite = locked;
        } else {
            icon.image.sprite = normal;
        }
    }

    public float getTime() {
        return levelTime;
    }

    public bool getLocked() {
        return isLocked;
    }

    public void clearTime() {
        levelTime = 0f;
        PlayerPrefs.DeleteKey("level" + levelID + "_time");
    }
}
