using UnityEngine;

public class ForcePad : MonoBehaviour {
    public Vector3 launchForce;
    public float launchCoolDown;
    private float CDleft = 0f;
    
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (CDleft > 0f) {
            CDleft -= Time.deltaTime;
        }
    }

    public float getCD() {
        return CDleft;
    }

    public void addCD() {
        CDleft += launchCoolDown;
    }
}
