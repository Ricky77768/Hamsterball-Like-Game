using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour {
    public GameObject mainCamera;
    public GameObject particles;
    public GameObject ballTrail;
    public GameObject ballDizzyFX1;
    public GameObject ballDizzyFX2;
    public Canvas UI;
    public float maxAngularVelocity = 100f;
    public float rotationSpeed = 60f;
    public float groundSpeed = 1200f;
    public float aerialSpeed = 600f;
    public float dizzySpeed = 600f;
    public float dizzyDuration = 1.5f;
    public float gravityForce = 350f;
    public float slopeSlideForce = 3000f;
    public float drag = 1.2f;
    public float restDrag = 4.5f;
    public float dragIncreaseTime = 0.25f; // rolling -> rest (in seconds)
    public float dragDecreaseTime = 0.15f; // rest -> rolling (in seconds)
    public float dragTriggerValue = 0.2f; // 0-1 for horizontal/vertical input

    private CameraFollow ballCamera;
    private Rigidbody ball;
    private Vector3 moveForce;
    private Vector3 fallStartPos;
    private Vector3 fallCurPos;
    private Vector3 respawnPos;
    private Vector2 keyMove;
    private Vector2 mouseMove;
    private Vector2 overallMove;
    private float curDrag;
    private float dragFactor;
    private float dizzyTimer = 0f;
    private float collisionAngle;
    private float fallDistance;
    private float sensitivity = 10f;
    private bool isActive = false;
    private bool isRespawning = false;
    private bool isGrounded = false;
    private bool isPrevGrounded = false;
    private bool breakUponContact = false;
    private bool dizzyUponContact = false;
    private bool mouseEnabled = true;
    private bool keyboardEnabled = true;
    private int ignoreRaycast = ~(1 << 2);

    // For test
    public Text fallText;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        ballCamera = mainCamera.GetComponent<CameraFollow>();
        ball = GetComponent<Rigidbody>();
        ball.freezeRotation = true;
        ballDizzyFX1.GetComponent<ParticleSystem>().Stop();
        ballDizzyFX2.GetComponent<ParticleSystem>().Stop();
        ball.maxAngularVelocity = maxAngularVelocity;
        fallStartPos = ball.position;
        fallCurPos = ball.position;
        updateControls();
    }

    void Update() {
        particles.transform.position = transform.position;

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
        moveForce = new Vector3(overallMove.x, 0, overallMove.y);
        moveForce = Quaternion.Euler(0, 45, 0) * moveForce;
        
        // Disable input when level is finished
        if (!isActive || isRespawning) {
            moveForce = Vector3.zero;
        } else {
            moveForce.Normalize();
        }

        isGrounded = Physics.SphereCast(transform.position, 0.2f, Vector3.down, out RaycastHit hit, 0.6f);
        collisionAngle = Vector3.Angle(Vector3.up, hit.normal);
        Vector3 left = Vector3.Cross(hit.normal, Vector3.up);
        Vector3 slope = Vector3.Cross(hit.normal, left);
        CheckSurroundings(Physics.OverlapSphere(transform.position, 0.6f, ignoreRaycast));

        // Ball rotation
        if (ball.velocity.x > 0) {
            ball.transform.Rotate(Quaternion.Euler(0, 90, 0) * new Vector3(ball.velocity.x, 0, 0), rotationSpeed * ball.velocity.x * Time.deltaTime, Space.World);
        } else {
            ball.transform.Rotate(Quaternion.Euler(0, 270, 0) * new Vector3(ball.velocity.x, 0, 0), rotationSpeed * ball.velocity.x * Time.deltaTime, Space.World);
        }
        if (ball.velocity.z > 0) {
            ball.transform.Rotate(Quaternion.Euler(0, 90, 0) * new Vector3(0, 0, ball.velocity.z), rotationSpeed * ball.velocity.z * Time.deltaTime, Space.World);
        } else {
            ball.transform.Rotate(Quaternion.Euler(0, 270, 0) * new Vector3(0, 0, ball.velocity.z), rotationSpeed * ball.velocity.z * Time.deltaTime, Space.World);
        }
        
        // Slope & Faster stopping when no input      
        if (Mathf.Abs(overallMove.x) < dragTriggerValue && Mathf.Abs(overallMove.y) < dragTriggerValue && isGrounded) {
            ball.AddForce(slope * slopeSlideForce * Time.deltaTime);
            dragFactor += (1 / dragIncreaseTime) * Time.deltaTime;   
        } else {
            ball.AddForce(0.2f * slope * slopeSlideForce * Time.deltaTime);
            dragFactor -= (1 / dragDecreaseTime) * Time.deltaTime;
        }
        dragFactor = Mathf.Clamp(dragFactor, 0f, 1f) * (1 - collisionAngle / 100);
        curDrag = Mathf.Lerp(drag, restDrag, dragFactor);
        ball.drag = curDrag;

        // Gravity
        if (!isGrounded) {
            ball.AddForce(Physics.gravity * gravityForce * Time.deltaTime);
        }

        // Gravity - Calculate fall distance
        if (!isGrounded && isPrevGrounded) {
            fallStartPos = ball.position;
        } else if (!isGrounded && !isPrevGrounded) {
            fallCurPos = ball.position;
            fallDistance = fallStartPos.y - fallCurPos.y;
        }

        // Test
        if (isGrounded != isPrevGrounded) {
            fallText.text = fallDistance.ToString(); 
        }

        // Apply correct speed based on ball state
        if (dizzyTimer > 0f) {
            dizzyTimer -= Time.deltaTime;
            ball.AddForce(moveForce * dizzySpeed * Time.deltaTime);
        } else if (isGrounded) {
            ball.AddForce(moveForce * groundSpeed * Time.deltaTime);
            if (ballDizzyFX1.GetComponent<ParticleSystem>().isPlaying) {
                ballDizzyFX1.GetComponent<ParticleSystem>().Stop();
                ballDizzyFX2.GetComponent<ParticleSystem>().Stop();
            }
        } else {
            ball.AddForce(moveForce * aerialSpeed * Time.deltaTime);
            if (ballDizzyFX1.GetComponent<ParticleSystem>().isPlaying) {
                ballDizzyFX1.GetComponent<ParticleSystem>().Stop();
                ballDizzyFX2.GetComponent<ParticleSystem>().Stop();
            }
        }

        // Enter dizzy state
        if (dizzyUponContact && isGrounded && dizzyTimer < 0.5f * dizzyDuration) {
            UI.GetComponent<GameController>().playBallDizzy();
            ballDizzyFX1.GetComponent<ParticleSystem>().Play();
            ballDizzyFX2.GetComponent<ParticleSystem>().Play();
            dizzyTimer = dizzyDuration;
            dizzyUponContact = false;
        }

        // Ball breaks
        if (isGrounded && breakUponContact) {
            StartCoroutine(RespawnBall(true));
        }

        isPrevGrounded = isGrounded;

        if (isActive && Input.GetKeyDown(KeyCode.Escape)) {
            UI.GetComponent<GameController>().pauseGame();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            StartCoroutine(RespawnBall(true));
        }

        // TEST
        if (Input.GetKeyDown(KeyCode.Keypad0)) {
            UI.GetComponent<TimerController>().bonusTime(5);
        }

        // TEST
        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            UI.GetComponent<GameController>().completeGame();
        }

        // TEST
        if (Input.GetKeyDown(KeyCode.Keypad2)) {
            UI.GetComponent<GameController>().endGame();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!breakUponContact && other.gameObject.CompareTag("Checkpoint")) {
            respawnPos = other.transform.position;
        }

        if (isActive && !breakUponContact && other.gameObject.CompareTag("Finish Trigger")) {
            UI.GetComponent<GameController>().completeGame();
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Break Zone")) {
            if (fallDistance > other.GetComponent<CheckBreak>().breakDistance) {
                breakUponContact = true;
                dizzyUponContact = false;
            } else if (fallDistance > other.GetComponent<CheckBreak>().dizzyDistance) {
                dizzyUponContact = true;
            }
        }

        if (other.gameObject.CompareTag("Fallout Trigger")) {
            StartCoroutine(RespawnBall(false));
        }
    }

    public void toggleBallControls(bool state) {
        isActive = state;
    }

    public void updateControls() {
        if (PlayerPrefs.HasKey("setting_mouse_enabled")) {
            mouseEnabled = (PlayerPrefs.GetInt("setting_mouse_enabled") == 1);
        }

        if (PlayerPrefs.HasKey("setting_mouse_sensitivity")) {
            sensitivity = PlayerPrefs.GetFloat("setting_mouse_sensitivity");
        }

        if (PlayerPrefs.HasKey("setting_keyboard_enabled")) {
            keyboardEnabled = (PlayerPrefs.GetInt("setting_keyboard_enabled") == 1);
        }
    }

    IEnumerator RespawnBall(bool isBreaking) {
        if (isRespawning) { yield break; }
        isRespawning = true;

        ballCamera.disableCamera();
        ballTrail.GetComponent<ParticleSystem>().Stop();

        if (isBreaking) {
            ball.velocity = Vector3.zero;
            ballDizzyFX1.SetActive(false);
            ballDizzyFX2.SetActive(false);
            StartCoroutine(ball.GetComponent<BallShatter>().SplitMesh());
            UI.GetComponent<GameController>().playBallBreak();
        }
        yield return new WaitForSeconds(1);
        ballCamera.enableCamera();
        transform.position = respawnPos;
        transform.rotation = Quaternion.identity;
        ball.velocity = Vector3.zero;
        ball.angularVelocity = Vector3.zero;
        fallStartPos = ball.position;
        fallCurPos = ball.position;
        dizzyUponContact = false;
        breakUponContact = false;
        isRespawning = false;
        fallDistance = 0;
        dizzyTimer = 0;

        yield return new WaitForSeconds(0.3f);
        ballTrail.GetComponent<ParticleSystem>().Play();
        
        if (isBreaking) {
            ballDizzyFX1.SetActive(true);
            ballDizzyFX2.SetActive(true);
        }
    }

    private void CheckSurroundings(Collider[] hits) {
        foreach (var collider in hits) { 
            if (collider.transform == null) { return; }
            Bumper bp = collider.transform.GetComponent<Bumper>();

            if (bp != null && bp.getCD() <= 0f) {
                Physics.Linecast(transform.position, bp.transform.position, out RaycastHit contactPoint);
                ball.velocity = Vector3.Reflect(ball.velocity, contactPoint.normal);
                ball.AddForce(ball.velocity.normalized * bp.bumperStrength);
                bp.addCD();
            }
        }
    }
}
