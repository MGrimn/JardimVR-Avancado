using UnityEngine;

/// <summary>
/// Gerador automático da cena VR.
/// Anexe este script a um GameObject vazio chamado "SceneBuilder" na sua cena.
/// Ao pressionar Play, ele criará automaticamente todos os 5 objetos necessários:
///  1. Plano de chão
///  2. Casa (cubo grande)
///  3. Árvore (cilindro + esfera)
///  4. Banco (conjunto de cubos)
///  5. Rocha decorativa (esfera achatada)
///  6. Poste de luz (cilindro + esfera amarela)
/// 
/// ATENÇÃO: Este script é auxiliar. Você pode criar os objetos manualmente no Editor
/// se preferir maior controle visual.
/// </summary>
public class SceneBuilder : MonoBehaviour
{
    [Header("Configurações da Cena")]
    [Tooltip("Gerar objetos automaticamente ao iniciar?")]
    public bool autoGenerate = true;

    [Header("Materiais (opcional - deixe vazio para usar cores padrão)")]
    public Material groundMaterial;
    public Material houseMaterial;
    public Material treeTrunkMaterial;
    public Material treeLeavesMaterial;
    public Material benchMaterial;
    public Material rockMaterial;

    void Start()
    {
        if (autoGenerate)
            GenerateScene();
    }

    /// <summary>Gera todos os elementos da cena</summary>
    public void GenerateScene()
    {
        Debug.Log("[SceneBuilder] Gerando cena VR...");

        // 1. Chão
        CreateGround();

        // 2. Casa
        CreateHouse(new Vector3(8f, 0f, 5f));

        // 3. Árvores
        CreateTree(new Vector3(-4f, 0f, 3f));
        CreateTree(new Vector3(-6f, 0f, -2f));
        CreateTree(new Vector3(3f,  0f, -5f));

        // 4. Banco
        CreateBench(new Vector3(0f, 0f, 3f));

        // 5. Rocha
        CreateRock(new Vector3(-2f, 0f, -3f));

        // 6. Poste de luz
        CreateLampPost(new Vector3(4f, 0f, 0f));

        Debug.Log("[SceneBuilder] Cena gerada com sucesso! 6 tipos de objetos criados.");
    }

    // ─────────────────────────────────────────
    //  CRIADORES DE OBJETOS
    // ─────────────────────────────────────────

    private void CreateGround()
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Chao_Principal";
        ground.transform.position = Vector3.zero;
        ground.transform.localScale = new Vector3(5f, 1f, 5f); // 50x50 unidades

        SetColor(ground, new Color(0.3f, 0.6f, 0.2f)); // Verde grama
        if (groundMaterial != null)
            ground.GetComponent<Renderer>().material = groundMaterial;

        // Organizar na hierarquia
        MoveToFolder(ground, "Ambiente");
        Debug.Log("[SceneBuilder] ✔ Chão criado");
    }

    private void CreateHouse(Vector3 position)
    {
        // Container pai
        GameObject houseRoot = new GameObject("Casa");
        houseRoot.transform.position = position;
        MoveToFolder(houseRoot, "Estruturas");

        // Corpo da casa
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body.name = "Casa_Corpo";
        body.transform.SetParent(houseRoot.transform);
        body.transform.localPosition = new Vector3(0f, 2f, 0f);
        body.transform.localScale = new Vector3(6f, 4f, 5f);
        SetColor(body, new Color(0.9f, 0.85f, 0.75f)); // Bege
        if (houseMaterial != null) body.GetComponent<Renderer>().material = houseMaterial;

        // Telhado (pirâmide com cubo rotacionado)
        GameObject roof = GameObject.CreatePrimitive(PrimitiveType.Cube);
        roof.name = "Casa_Telhado";
        roof.transform.SetParent(houseRoot.transform);
        roof.transform.localPosition = new Vector3(0f, 4.5f, 0f);
        roof.transform.localScale = new Vector3(7f, 1.5f, 6f);
        roof.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        SetColor(roof, new Color(0.6f, 0.2f, 0.1f)); // Vermelho tijolo

        // Porta
        GameObject door = GameObject.CreatePrimitive(PrimitiveType.Cube);
        door.name = "Casa_Porta";
        door.transform.SetParent(houseRoot.transform);
        door.transform.localPosition = new Vector3(0f, 0.9f, 2.51f);
        door.transform.localScale = new Vector3(1f, 1.8f, 0.1f);
        SetColor(door, new Color(0.4f, 0.25f, 0.1f)); // Marrom madeira

        // Janela esquerda
        CreateWindow(houseRoot.transform, new Vector3(-1.8f, 2.2f, 2.51f));
        // Janela direita
        CreateWindow(houseRoot.transform, new Vector3(1.8f, 2.2f, 2.51f));

        Debug.Log("[SceneBuilder] ✔ Casa criada");
    }

    private void CreateWindow(Transform parent, Vector3 localPos)
    {
        GameObject window = GameObject.CreatePrimitive(PrimitiveType.Cube);
        window.name = "Casa_Janela";
        window.transform.SetParent(parent);
        window.transform.localPosition = localPos;
        window.transform.localScale = new Vector3(0.8f, 0.8f, 0.1f);
        SetColor(window, new Color(0.5f, 0.75f, 1f)); // Azul vidro
    }

    private void CreateTree(Vector3 position)
    {
        GameObject treeRoot = new GameObject("Arvore");
        treeRoot.transform.position = position;
        MoveToFolder(treeRoot, "Vegetacao");

        // Tronco
        GameObject trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        trunk.name = "Arvore_Tronco";
        trunk.transform.SetParent(treeRoot.transform);
        trunk.transform.localPosition = new Vector3(0f, 1.2f, 0f);
        trunk.transform.localScale = new Vector3(0.3f, 1.2f, 0.3f);
        SetColor(trunk, new Color(0.45f, 0.28f, 0.1f)); // Marrom
        if (treeTrunkMaterial != null) trunk.GetComponent<Renderer>().material = treeTrunkMaterial;

        // Copa
        GameObject leaves = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        leaves.name = "Arvore_Copa";
        leaves.transform.SetParent(treeRoot.transform);
        leaves.transform.localPosition = new Vector3(0f, 3.2f, 0f);
        leaves.transform.localScale = new Vector3(2f, 2f, 2f);
        SetColor(leaves, new Color(0.15f, 0.5f, 0.1f)); // Verde escuro
        if (treeLeavesMaterial != null) leaves.GetComponent<Renderer>().material = treeLeavesMaterial;

        Debug.Log("[SceneBuilder] ✔ Árvore criada em " + position);
    }

    private void CreateBench(Vector3 position)
    {
        GameObject benchRoot = new GameObject("Banco_Jardim");
        benchRoot.transform.position = position;
        MoveToFolder(benchRoot, "Mobiliario");

        Color woodColor = new Color(0.55f, 0.35f, 0.15f);

        // Assento
        GameObject seat = GameObject.CreatePrimitive(PrimitiveType.Cube);
        seat.name = "Banco_Assento";
        seat.transform.SetParent(benchRoot.transform);
        seat.transform.localPosition = new Vector3(0f, 0.55f, 0f);
        seat.transform.localScale = new Vector3(1.8f, 0.1f, 0.6f);
        SetColor(seat, woodColor);
        if (benchMaterial != null) seat.GetComponent<Renderer>().material = benchMaterial;

        // Encosto
        GameObject backrest = GameObject.CreatePrimitive(PrimitiveType.Cube);
        backrest.name = "Banco_Encosto";
        backrest.transform.SetParent(benchRoot.transform);
        backrest.transform.localPosition = new Vector3(0f, 0.85f, -0.25f);
        backrest.transform.localScale = new Vector3(1.8f, 0.6f, 0.08f);
        SetColor(backrest, woodColor);

        // Pé esquerdo
        CreateBenchLeg(benchRoot.transform, new Vector3(-0.75f, 0.25f, 0f), woodColor);
        // Pé direito
        CreateBenchLeg(benchRoot.transform, new Vector3( 0.75f, 0.25f, 0f), woodColor);

        Debug.Log("[SceneBuilder] ✔ Banco criado");
    }

    private void CreateBenchLeg(Transform parent, Vector3 localPos, Color color)
    {
        GameObject leg = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leg.name = "Banco_Pe";
        leg.transform.SetParent(parent);
        leg.transform.localPosition = localPos;
        leg.transform.localScale = new Vector3(0.08f, 0.5f, 0.6f);
        SetColor(leg, color);
    }

    private void CreateRock(Vector3 position)
    {
        GameObject rock = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        rock.name = "Rocha_Decorativa";
        rock.transform.position = position + new Vector3(0f, 0.3f, 0f);
        rock.transform.localScale = new Vector3(0.8f, 0.5f, 0.7f);
        rock.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        SetColor(rock, new Color(0.5f, 0.5f, 0.5f)); // Cinza pedra
        if (rockMaterial != null) rock.GetComponent<Renderer>().material = rockMaterial;
        MoveToFolder(rock, "Decoracao");
        Debug.Log("[SceneBuilder] ✔ Rocha criada");
    }

    private void CreateLampPost(Vector3 position)
    {
        GameObject lampRoot = new GameObject("Poste_Luz");
        lampRoot.transform.position = position;
        MoveToFolder(lampRoot, "Mobiliario");

        // Haste
        GameObject pole = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        pole.name = "Poste_Haste";
        pole.transform.SetParent(lampRoot.transform);
        pole.transform.localPosition = new Vector3(0f, 2f, 0f);
        pole.transform.localScale = new Vector3(0.1f, 2f, 0.1f);
        SetColor(pole, new Color(0.2f, 0.2f, 0.2f)); // Preto

        // Lâmpada (esfera amarela)
        GameObject lamp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        lamp.name = "Poste_Lampada";
        lamp.transform.SetParent(lampRoot.transform);
        lamp.transform.localPosition = new Vector3(0f, 4.2f, 0f);
        lamp.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        SetColor(lamp, new Color(1f, 0.95f, 0.4f)); // Amarelo

        // Adiciona luz pontual
        Light pointLight = lamp.AddComponent<Light>();
        pointLight.type = LightType.Point;
        pointLight.color = new Color(1f, 0.9f, 0.6f);
        pointLight.intensity = 2f;
        pointLight.range = 10f;

        Debug.Log("[SceneBuilder] ✔ Poste de luz criado");
    }

    // ─────────────────────────────────────────
    //  UTILITÁRIOS
    // ─────────────────────────────────────────

    /// <summary>Define a cor de um objeto por material temporário</summary>
    private void SetColor(GameObject obj, Color color)
    {
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend != null)
        {
            // Cria um material padrão (URP ou Built-in)
            Material mat = new Material(Shader.Find("Standard"));
            if (mat == null) mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.color = color;
            rend.material = mat;
        }
    }

    /// <summary>Move um objeto para uma pasta (GameObject vazio) na hierarquia</summary>
    private void MoveToFolder(GameObject obj, string folderName)
    {
        GameObject folder = GameObject.Find("_" + folderName);
        if (folder == null)
        {
            folder = new GameObject("_" + folderName);
            folder.transform.SetParent(this.transform);
        }
        obj.transform.SetParent(folder.transform);
    }
}
