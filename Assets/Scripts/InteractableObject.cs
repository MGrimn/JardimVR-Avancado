using UnityEngine;

/// <summary>
/// Script base para objetos interagíveis no ambiente.
/// Quando o jogador interagir com o objeto, ele executa uma ação.
/// </summary>
[RequireComponent(typeof(Collider))]
public class InteractableObject : MonoBehaviour
{
    [Header("Configurações da Ação")]
    public bool isLamp = false;
    public bool isDoor = false;

    private Light attachedLight;
    private Renderer objRenderer;
    private Color originalColor;
    private bool stateOn = true;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        if (objRenderer != null)
        {
            originalColor = objRenderer.material.color;
        }

        if (isLamp)
        {
            attachedLight = GetComponent<Light>();
        }
    }

    public void OnInteract()
    {
        stateOn = !stateOn;

        if (isLamp)
        {
            if (attachedLight != null)
            {
                attachedLight.enabled = stateOn;
            }
            
            if (objRenderer != null)
            {
                objRenderer.material.color = stateOn ? new Color(1f, 0.95f, 0.4f) : Color.gray; // Amarelo ou Cinza
            }
            
            Debug.Log("[InteractableObject] Lâmpada alternada para: " + (stateOn ? "Ligada" : "Desligada"));
        }
        else if (isDoor)
        {
            if (objRenderer != null)
            {
                objRenderer.material.color = stateOn ? originalColor : new Color(0.2f, 0.8f, 0.2f); // Muda cor da porta para verde
            }
            Debug.Log("[InteractableObject] Porta interagida.");
        }
        else
        {
            // Ação padrão: muda cor aleatória
            if (objRenderer != null)
            {
                objRenderer.material.color = new Color(Random.value, Random.value, Random.value);
            }
            Debug.Log("[InteractableObject] Objeto interagido (cor aleatória).");
        }
    }
}
