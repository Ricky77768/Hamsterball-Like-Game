using UnityEngine;

public class Bumper : MonoBehaviour {
    public float bumperStrength = 10f;
    public float bumperCoolDown;
    private float CDleft = 0f;

    void Start() {
        
    }

    void Update() {
        if (CDleft > 0f) {
            CDleft -= Time.deltaTime;
        }
    }
    public float getCD() {
        return CDleft;
    }

    public void addCD() {
        CDleft += bumperCoolDown;
    }
}