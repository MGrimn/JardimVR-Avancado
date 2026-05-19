

**Aluno(a):** Gustavo Davyd de Oliveira Mota

---

## 1. Apresentando o Seu Projeto
O projeto **"Jardim de Meditação VR"** é um ambiente interativo criado na Unity utilizando o Meta XR SDK. Ele simula um jardim externo imersivo com objetos 3D (casa, árvores, banco, rocha, poste de luz). O ambiente possui movimentação pelo PC (teclado e mouse) e interações específicas como um sistema de iluminação inteligente e portas interativas, que funcionam através de mecânicas de Raycast.

## 2. Contexto e Objetivos
**Contexto:** Saúde Mental e Bem-estar (Entretenimento/Educação).
**Objetivo:** Este ambiente representa um espaço de relaxamento e foco dentro do Metaverso. Muitas vezes, em ambientes virtuais de trabalho ou estudo, os usuários sofrem de exaustão digital. O "Jardim de Meditação" serve como um refúgio virtual onde o usuário pode entrar, interagir com o ambiente (ligar/desligar a luz ambiente do poste, abrir/fechar a porta da cabana) e aproveitar um momento de descompressão.

## 3. Processo de Criação e Dificuldades
**Processo de Criação:**
- **Construção da Cena:** Utilizei Unity Primitives para gerar os objetos, organizando hierarquicamente os elementos e aplicando cores baseadas em materiais URP/Standard.
- **Movimentação:** Adaptei o script de movimentação para que funcione nativamente no PC, garantindo que o projeto pudesse ser testado sem a necessidade imediata dos óculos VR, focando na flexibilidade.
- **Interação:** Desenvolvi um sistema de `PlayerInteraction.cs` utilizando Raycast (acionado pelo MouseClick ou tecla 'E'). Ele se conecta com:
  1. `InteractableObject.cs`: Cuida da alternância de luzes (ligar/desligar).
  2. `DoorController.cs`: Um script independente (anexado aos assets de porta já existentes na cena) que gerencia de forma autônoma a animação rotacional (abrir e fechar portas de forma suave através de `Coroutines`).

**Principais Dificuldades e Soluções:**
- *Conflitos de Input:* Tive dificuldades iniciais com o "New Input System" do Meta SDK conflitando com a movimentação tradicional. Resolvi isso ajustando o **Active Input Handling** para "Both" nas configurações do projeto, permitindo o uso da API clássica (`UnityEngine.Input`) de forma segura e fluida.
- *Interações 3D no PC vs VR:* Criar uma interação que fizesse sentido tanto no PC quanto no VR exigiu adaptar o clique do mouse no centro da tela. Usei `Raycast` a partir do centro da câmera, que é uma abordagem facilmente portável para interações baseadas em olhar (Gaze) ou ponteiros VR no futuro.

---

## Estrutura de Diretórios (Destaques)
- `/Assets/Scripts/PCMovement.cs`: Controla o movimento via teclado/mouse.
- `/Assets/Scripts/PlayerSetup.cs`: Monta o Rig do Player e configura a câmera automaticamente.
- `/Assets/Scripts/PlayerInteraction.cs` e `InteractableObject.cs`: Cuidam da lógica da atividade interativa (Luz e eventos gerais).
- `/Assets/Scripts/DoorController.cs`: Script independente e reutilizável para rotacionar/animar portas dos assets da cena quando interagidos.
