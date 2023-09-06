using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class Player : MonoBehaviour {

    private Camera camera;
    private Rigidbody rigidbody;

    void Start() {
        camera = Camera.main;
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update() {
        Move();
        Look();
    }

    /// <summary>
    /// Aqui n�s movemos o personagem em cada eixo.
    /// A f�rmula ser� sempre: Vetor_velocidade = Vetor(Eixo X normalizado {-1, 1} por Input, 0, Eixo Y normalizado {-1, 1} por Input) * Velocidade_Media
    /// </summary>
    private void Move() {
        Vector3 velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Constants.WALK_SPEED;
        rigidbody.velocity = velocity;
    }

    /// <summary>
    /// Aqui n�s fazemos o personagem olhar para o cursor do mouse.
    /// A f�rmula ser� a seguinte: Novo_angulo_Y = ArcTan(distancia ou diferen�a entre mouse e personagem) * 180 / Num_Pi
    /// Lembrando que o resultado da fun��o Arco-Tangente sempre ser� em radianos, logo � por isso que precisamos da convers�o para graus...
    /// </summary>
    private void Look() 
    {
        Vector3 position = camera.WorldToScreenPoint(transform.position);
        Vector3 difference = Input.mousePosition - position;
        float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, -angle, 0f);
    }
}
