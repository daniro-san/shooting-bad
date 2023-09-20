using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour 
{
    public float walkSpeed = 5.0f;

    private Camera camera = null;
    private Rigidbody rigidbody = null;
    private Animator animator;
    private AnimationDirectorController animationDirector;

    void Start() 
    {
        camera = Camera.main;
        rigidbody = GetComponent<Rigidbody>();
        animationDirector = GetComponent<AnimationDirectorController>();
        animator = GetComponent<Animator>();
    }

    void Update() 
    {
        Move();
        Look();
    }

    /// <summary>
    /// Aqui nós movemos o personagem em cada eixo.
    /// A fórmula será sempre: Vetor_velocidade = Vetor(Eixo X normalizado {-1, 1} por Input, 0, Eixo Y normalizado {-1, 1} por Input) * Velocidade_Media
    /// </summary>
    private void Move()
    {
        Vector3 velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * walkSpeed;

        animationDirector.Moving(animator, velocity.sqrMagnitude);

        rigidbody.velocity = velocity;
    }

    /// <summary>
    /// Aqui nós fazemos o personagem olhar para o cursor do mouse.
    /// A fórmula será a seguinte: Novo_angulo_Y = ArcTan(distancia ou diferença entre mouse e personagem) * 180 / Num_Pi
    /// Lembrando que o resultado da função Arco-Tangente sempre será em radianos, logo é por isso que precisamos da conversão para graus...
    /// </summary>
    private void Look() 
    {
        Vector3 position = camera.WorldToScreenPoint(transform.position);
        Vector3 difference = Input.mousePosition - position;
        float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, -angle, 0f);
    }
}
