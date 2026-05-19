using UnityEngine;

/// <summary>
/// Sistema de interação do jogador. Lança um Raycast a partir da câmera
/// para detectar objetos com o script InteractableObject.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    [Header("Configurações de Interação")]
    public float interactionDistance = 5f;
    public Transform cameraTransform;

    void Start()
    {
        if (cameraTransform == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
                cameraTransform = mainCam.transform;
            else
                Debug.LogWarning("[PlayerInteraction] Câmera não encontrada! Atribua no Inspector.");
        }
    }

    void Update()
    {
        // Interagir com clique esquerdo do mouse ou tecla 'E'
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void Interact()
    {
        if (cameraTransform == null) return;

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            // Tenta achar o InteractableObject comum (lâmpadas, etc)
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                interactable.OnInteract();
                return;
            }

            // Tenta achar a porta animada
            DoorController door = hit.collider.GetComponentInParent<DoorController>(); // Busca também no pai, útil para dobradiças
            if (door != null)
            {
                door.ToggleDoor();
                return;
            }
        }
    }
}
