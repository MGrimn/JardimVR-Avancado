using UnityEngine;
using System.Collections;

/// <summary>
/// Controlador independente para abrir e fechar portas.
/// Ele cria automaticamente uma "dobradiça" virtual na quina para rodar corretamente.
/// </summary>
public class DoorController : MonoBehaviour
{
    [Header("Configurações da Porta")]
    [Tooltip("O ângulo Y que a porta deve ficar quando aberta")]
    public float openAngle = -90f;
    [Tooltip("A velocidade da animação de abrir/fechar")]
    public float rotationSpeed = 3f;
    
    [Tooltip("Lado da dobradiça (1 = Direita, -1 = Esquerda). Mude se a porta abrir pro lado errado.")]
    public float hingeSide = -1f;

    private float closedAngle;
    private bool isOpen = false;
    private Coroutine animationCoroutine;
    
    // A dobradiça real que vai girar
    private Transform hingeTransform;

    void Start()
    {
        // 1. Cria um objeto vazio para atuar como dobradiça no mundo
        GameObject hinge = new GameObject(gameObject.name + "_Hinge");
        hinge.transform.SetParent(transform.parent);
        
        // 2. Calcula a posição da quina baseada na escala atual do objeto (assumindo eixo central)
        float xOffset = (transform.localScale.x / 2f) * hingeSide;
        
        // 3. Posiciona a dobradiça exatamente na quina (borda) da porta
        hinge.transform.position = transform.position + transform.right * xOffset;
        hinge.transform.rotation = transform.rotation;
        
        // 4. Faz a porta se tornar "filha" dessa dobradiça
        transform.SetParent(hinge.transform);
        
        // Guardamos a referência da dobradiça. Agora ela é quem vai girar!
        hingeTransform = hinge.transform;
        
        // Salva a rotação inicial (fechada) no eixo Y da dobradiça
        closedAngle = hingeTransform.localEulerAngles.y;
    }

    /// <summary>
    /// Alterna o estado da porta entre aberta e fechada.
    /// </summary>
    public void ToggleDoor()
    {
        isOpen = !isOpen;

        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        animationCoroutine = StartCoroutine(AnimateDoor(isOpen ? openAngle : closedAngle));
        Debug.Log("[DoorController] Porta " + (isOpen ? "Abrindo" : "Fechando") + " através da dobradiça virtual");
    }

    private IEnumerator AnimateDoor(float targetAngle)
    {
        // Pega a rotação alvo preservando X e Z originais da DOBRADIÇA
        Quaternion targetRot = Quaternion.Euler(hingeTransform.localEulerAngles.x, targetAngle, hingeTransform.localEulerAngles.z);

        // Interpola suavemente até o ângulo alvo
        while (Quaternion.Angle(hingeTransform.localRotation, targetRot) > 0.1f)
        {
            hingeTransform.localRotation = Quaternion.Slerp(hingeTransform.localRotation, targetRot, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        hingeTransform.localRotation = targetRot;
    }
}
