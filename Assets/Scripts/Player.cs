using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController character;
    private Vector3 direction;

    [SerializeField] private float gravity = 9.81f * 2f;
    [SerializeField] private float minJumpForce = 5f;
    [SerializeField] private float maxJumpForce = 15f;
    [SerializeField] private float chargeRate = 10f;

    private float jumpCharge;
    private bool isCharging;
    private bool canJump;

    private void Awake()
    {
        character = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        ResetPlayerState();
    }

    private void Update()
    {
        if (character.isGrounded)
        {
            HandleGroundedState();
        }
        else
        {
            ApplyGravity();
        }

        character.Move(direction * Time.deltaTime);
    }

    private void ResetPlayerState()
    {
        direction = Vector3.zero;
        jumpCharge = 0f;
        isCharging = false;
        canJump = false;
    }

    private void HandleGroundedState()
    {
        if (!canJump)
        {
            PrepareForJump();
        }

        if (IsJumpButtonHeld())
        {
            ChargeJump();
        }

        if (isCharging && IsJumpButtonReleased())
        {
            PerformJump();
        }
    }

    private void PrepareForJump()
    {
        direction.y = -1f; // Oyuncunun yerde kalmasını sağlar
        canJump = true;
    }

    private void ChargeJump()
    {
        isCharging = true;
        jumpCharge += chargeRate * Time.deltaTime;
        jumpCharge = Mathf.Clamp(jumpCharge, minJumpForce, maxJumpForce);
    }

    private void PerformJump()
    {
        MusicManager.instance.PlayJumpSound();
        direction.y = jumpCharge;
        ResetJumpState();
    }

    private void ResetJumpState()
    {
        jumpCharge = 0f;
        isCharging = false;
        canJump = false;
    }

    private void ApplyGravity()
    {
        direction.y -= gravity * Time.deltaTime; // Yerçekimini uygula
    }

    private bool IsJumpButtonHeld()
    {
        // Zıplama için klavye (Boşluk), fare veya dokunmatik giriş kontrolü
        return Input.GetKey(KeyCode.Space) ||
               Input.GetMouseButton(0) ||
               IsTouchHeld();
    }

    private bool IsJumpButtonReleased()
    {
        // Zıplama tuşunun bırakıldığını kontrol eder
        return Input.GetKeyUp(KeyCode.Space) ||
               Input.GetMouseButtonUp(0) ||
               IsTouchReleased();
    }

    private bool IsTouchHeld()
    {
        // Dokunmatik ekranda basılı tutma durumunu kontrol eder
        return Input.touchCount > 0 &&
               Input.GetTouch(0).phase == TouchPhase.Stationary;
    }

    private bool IsTouchReleased()
    {
        // Dokunmatik ekranda parmağın kaldırıldığını kontrol eder
        return Input.touchCount > 0 &&
               (Input.GetTouch(0).phase == TouchPhase.Ended ||
                Input.GetTouch(0).phase == TouchPhase.Canceled);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            GameManager.instance.GameOver();
        }
    }
}