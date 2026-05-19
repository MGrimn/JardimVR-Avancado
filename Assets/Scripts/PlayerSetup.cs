using UnityEngine;

/// <summary>
/// Configurador do Player para VR (Meta Quest) e PC.
/// Monta automaticamente a estrutura correta do Player:
///
///   [PlayerRoot]          <- Rigidbody, PCMovement (este script)
///     └── [CameraHolder]  <- Altura dos olhos
///           └── [MainCamera]
///
/// Como usar:
///  1. Crie um GameObject vazio e renomeie para "Player"
///  2. Adicione este script nele
///  3. Pressione Play — a estrutura será criada automaticamente
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerSetup : MonoBehaviour
{
    [Header("Configurações do Player")]
    [Tooltip("Altura do player em metros")]
    public float playerHeight = 1.8f;

    [Tooltip("Largura/raio do colisor")]
    public float playerRadius = 0.4f;

    [Tooltip("Velocidade de caminhada")]
    public float walkSpeed = 3f;

    [Tooltip("Velocidade de corrida")]
    public float runSpeed = 6f;

    void Awake()
    {
        SetupCollider();
        SetupRigidbody();
        SetupCamera();
        SetupMovement();
        PositionPlayer();
    }

    private void SetupCollider()
    {
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        col.height = playerHeight;
        col.radius = playerRadius;
        col.center = new Vector3(0f, playerHeight / 2f, 0f);
    }

    private void SetupRigidbody()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Evita que o player role
        rb.mass = 70f;
        rb.linearDamping = 5f;
    }

    private void SetupCamera()
    {
        // Verifica se já existe câmera filha
        Camera existingCam = GetComponentInChildren<Camera>();
        if (existingCam != null) return;

        // Cria o CameraHolder na altura dos olhos
        GameObject cameraHolder = new GameObject("CameraHolder");
        cameraHolder.transform.SetParent(this.transform);
        cameraHolder.transform.localPosition = new Vector3(0f, playerHeight - 0.15f, 0f);

        // Cria a câmera
        GameObject camObj = new GameObject("MainCamera");
        camObj.transform.SetParent(cameraHolder.transform);
        camObj.transform.localPosition = Vector3.zero;
        camObj.transform.localRotation = Quaternion.identity;

        Camera cam = camObj.AddComponent<Camera>();
        cam.tag = "MainCamera";
        cam.fieldOfView = 75f;
        cam.nearClipPlane = 0.01f;

        // AudioListener na câmera
        camObj.AddComponent<AudioListener>();

        Debug.Log("[PlayerSetup] Câmera criada em: " + cameraHolder.transform.localPosition);
    }

    private void SetupMovement()
    {
        // Adiciona ou configura o PCMovement
        PCMovement movement = GetComponent<PCMovement>();
        if (movement == null)
            movement = gameObject.AddComponent<PCMovement>();

        movement.walkSpeed = walkSpeed;
        movement.runSpeed = runSpeed;

        // Atribui a câmera ao script de movimento
        Camera cam = GetComponentInChildren<Camera>();
        if (cam != null)
            movement.cameraTransform = cam.transform;

        // Adiciona ou configura o PlayerInteraction
        PlayerInteraction interaction = GetComponent<PlayerInteraction>();
        if (interaction == null)
            interaction = gameObject.AddComponent<PlayerInteraction>();
            
        if (cam != null)
            interaction.cameraTransform = cam.transform;
    }

    private void PositionPlayer()
    {
        // Posiciona o player ligeiramente acima do chão para evitar clipping
        transform.position = new Vector3(0f, 0.05f, 0f);
    }
}
