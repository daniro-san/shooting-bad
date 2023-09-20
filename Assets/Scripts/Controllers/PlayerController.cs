using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class PlayerController : MonoBehaviour 
{
    // Propriedade públicas configuraveis no editor
    public float walkSpeed = 5.0f;
    public float cameraThreshold = 5.0f;

    // Propriedades privadas
    private Camera camera = null;
    private Rigidbody rigidbody = null;

    // Propriedades relacionadas a "Camera Boom"
    private bool isCameraMoving = false;
    private Vector3 playerInitialPosition = Vector3.zero;
    private Vector3 cameraInitialPosition = Vector3.zero;

    void Start() 
    {
        // Obtendo componentes e outras entidades
        camera = Camera.main;
        rigidbody = GetComponent<Rigidbody>();

        // Inicializando propriedades do "Camera Boom"
        playerInitialPosition = transform.position;
        cameraInitialPosition = camera.transform.position;
    }

    void Update() 
    {
        Move();
        CameraBoom();
        Look();
    }

    /// <summary>
    /// Aqui nós movemos o personagem em cada eixo.
    /// <para>A fórmula será sempre: Vetor_velocidade = Vetor(Eixo X normalizado {-1, 1} por Input, 0, Eixo Y normalizado {-1, 1} por Input) * Velocidade_Media</para>
    /// </summary>
    private void Move()
    {
        Vector3 velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * walkSpeed;
        rigidbody.velocity = velocity;
    }

    /// <summary>
    /// Aplica o movimento e efeito de "camera boom".
    /// <para>Para isso, nós cálculamos a distância de movimento que o jogador andou.</para>
    /// <para>Em seguida e se a distância for maior que o cameraThreshold (limiar), nós aplicamos a seguinte fórmula:</para>
    /// 
    /// <para>Posição_Nova_Camera = Posição_Antiga_Camera - Vetor_Normalizado_de_Direção * Velocidade_Media * Frame_Time</para>
    /// 
    /// <para>Veja mais: https://pt.wikipedia.org/wiki/Normal_(geometria)</para>
    /// 
    /// </summary>
    private void CameraBoom() 
    {
        float playerDistanceOffset = Vector3.Distance(transform.position, playerInitialPosition);

        // Checando limiar de distância do jogador... Se for maior, nós movemos a câmera.
        if (!isCameraMoving && playerDistanceOffset > cameraThreshold) 
        {
            cameraInitialPosition = camera.transform.position;
            isCameraMoving = true;
        } 
        else 
        {
            // Aplicando a fórmula de movimento
            Vector3 originalVector = new Vector3(
                playerInitialPosition.x - transform.position.x,
                0f,
                playerInitialPosition.z - transform.position.z
            );

            Vector3 normalizedVector = new Vector3(originalVector.x, 0f, originalVector.z).normalized;
            Vector3 cameraPosition = camera.transform.position - normalizedVector * walkSpeed * Time.deltaTime;
            camera.transform.position = cameraPosition;

            // Checando se a distância navegada pela câmera é maior ou igual a distância navegada pelo jogador
            float cameraDistanceOffset = Vector3.Distance(camera.transform.position, cameraInitialPosition);
            if (cameraDistanceOffset >= playerDistanceOffset) 
            {
                isCameraMoving = false;
                playerInitialPosition = transform.position;
            }
        }
    }

    /// <summary>
    /// Aqui nós fazemos o personagem olhar para o cursor do mouse.
    /// <para>A fórmula será a seguinte: Novo_angulo_Y = ArcTan(distancia ou diferença entre mouse e personagem) * 180 / Num_Pi</para>
    /// <para>Lembrando que o resultado da função Arco-Tangente sempre será em radianos, logo é por isso que precisamos da conversão para graus...</para>
    /// </summary>
    private void Look() 
    {
        Vector3 position = camera.WorldToScreenPoint(transform.position);
        Vector3 difference = Input.mousePosition - position;
        float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, -angle, 0f);
    }
}
