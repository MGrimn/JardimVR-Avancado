using UnityEngine;
// Usando a API clássica (UnityEngine.Input) — compatível com "Both" em Active Input Handling.
// Não requer o pacote com.unity.inputsystem instalado.

/// <summary>
/// Controlador de movimentação para PC.
/// Requer: Edit → Project Settings → Player → Active Input Handling = "Both"
///
/// Controles:
/// - WASD ou Setas : Mover
/// - Mouse         : Rotacionar câmera
/// - Espaço        : Pular
/// - Shift Esq     : Correr
/// - ESC           : Liberar/travar cursor
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PCMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [Tooltip("Velocidade base de caminhada")]
    public float walkSpeed = 3f;

    [Tooltip("Velocidade ao correr (Shift)")]
    public float runSpeed = 6f;

    [Tooltip("Força do pulo")]
    public float jumpForce = 4f;

    [Header("Configurações da Câmera")]
    [Tooltip("Transform da câmera principal — preencha no Inspector")]
    public Transform cameraTransform;

    [Tooltip("Sensibilidade do mouse")]
    public float mouseSensitivity = 2f;

    [Tooltip("Limite máximo de rotação vertical (graus)")]
    public float maxVerticalAngle = 80f;

    // ── Internos ──────────────────────────────
    private Rigidbody rb;
    private float verticalRotation = 0f;
    private bool isGrounded = false;
    private bool cursorLocked = true;

    // ─────────────────────────────────────────
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Câmera: se não atribuída no Inspector, busca a Main Camera
        if (cameraTransform == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
                cameraTransform = mainCam.transform;
            else
                Debug.LogWarning("[PCMovement] Câmera não encontrada! Atribua no Inspector.");
        }

        LockCursor(true);
        Debug.Log("[PCMovement] Pronto! WASD=mover | Mouse=olhar | Espaço=pular | ESC=cursor");
    }

    // ─────────────────────────────────────────
    void Update()
    {
        HandleCameraRotation();
        HandleJump();
        HandleCursorLock();
    }

    void FixedUpdate()
    {
        HandleMovement();
        CheckGrounded();
    }

    // ─────────────────────────────────────────
    //  ROTAÇÃO DA CÂMERA
    // ─────────────────────────────────────────
    private void HandleCameraRotation()
    {
        if (!cursorLocked) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotação horizontal do corpo do player
        transform.Rotate(0f, mouseX, 0f);

        // Rotação vertical da câmera (limitada para não virar de cabeça pra baixo)
        verticalRotation -= mouseY;
        verticalRotation  = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);

        if (cameraTransform != null)
            cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    // ─────────────────────────────────────────
    //  MOVIMENTO WASD
    // ─────────────────────────────────────────
    private void HandleMovement()
    {
        float horizontal  = Input.GetAxisRaw("Horizontal"); // A/D
        float vertical    = Input.GetAxisRaw("Vertical");   // W/S
        bool  isRunning   = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Direção relativa ao olhar do player
        Vector3 direction = (transform.right * horizontal + transform.forward * vertical).normalized;

        // Aplica velocidade mantendo a componente Y (gravidade/pulo)
        Vector3 targetVelocity = direction * currentSpeed;
        targetVelocity.y = rb.linearVelocity.y;

        rb.linearVelocity = targetVelocity;
    }

    // ─────────────────────────────────────────
    //  PULO
    // ─────────────────────────────────────────
    private void HandleJump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    // ─────────────────────────────────────────
    //  VERIFICAR CHÃO
    // ─────────────────────────────────────────
    private void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.15f);
    }

    // ─────────────────────────────────────────
    //  TRAVA / LIBERA O CURSOR
    // ─────────────────────────────────────────
    private void HandleCursorLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            LockCursor(!cursorLocked);
    }

    private void LockCursor(bool shouldLock)
    {
        cursorLocked     = shouldLock;
        Cursor.lockState = shouldLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible   = !shouldLock;
    }

    // ─────────────────────────────────────────
    //  GIZMOS (debug visual no Editor)
    // ─────────────────────────────────────────
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 1.15f);
    }
}
