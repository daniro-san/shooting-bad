using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour {

    private Camera camera;

    void Start() {
        camera = Camera.main;
    }

    void Update() {
        Move();
        Look();
    }

    /// <summary>
    /// Aqui nós movemos o personagem em cada eixo.
    /// A fórmula será sempre: Novo_eixo = tempo_do_frame * velocidade_de_mov + Eixo_antigo * (+1 ou -1),
    /// onde -1 ou +1 servirá para atualizar o espaço ou em outras palavras "mover para frente" ou "mover para trás".
    /// </summary>
    private void Move() {
        Vector3 position = transform.position;

        if (Input.GetKey(KeyCode.W)) {
            position.z += Time.deltaTime * Constants.WALK_SPEED;
        }

        if (Input.GetKey(KeyCode.S)) {
            position.z -= Time.deltaTime * Constants.WALK_SPEED;
        }

        if (Input.GetKey(KeyCode.D)) {
            position.x += Time.deltaTime * Constants.WALK_SPEED;
        }

        if (Input.GetKey(KeyCode.A)) {
            position.x -= Time.deltaTime * Constants.WALK_SPEED;
        }

        Vector3 difference = position - transform.position;
        camera.transform.position += difference;
        transform.position = position;
    }

    /// <summary>
    /// Aqui nós fazemos o personagem olhar para o cursor do mouse.
    /// A fórmula será a seguinte: Novo_angulo_Y = Tan(distancia ou diferença entre mouse e personagem) * 180 / Num_Pi
    /// </summary>
    private void Look() {
        Vector3 position = camera.WorldToScreenPoint(transform.position);
        Vector3 difference = Input.mousePosition - position;
        float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, -angle, 0f);
    }

    private struct Constants {
        public static float WALK_SPEED = 5.0f;
    }
}
