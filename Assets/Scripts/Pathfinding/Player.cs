using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float moveSpeedMultiplier;
    [SerializeField] private float rotationSmoothMultiplier;
    private void Update()
    {
        //Procesado de input
        Vector2 inputVector = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x -= 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x += 1;
        }
        inputVector = inputVector.normalized;

        //Cálculo del movimiento
        Vector3 movementVector = new(inputVector.x, 0f, inputVector.y);
        transform.position += moveSpeedMultiplier * Time.deltaTime * movementVector;

        //Rotación del personaje
        transform.forward = Vector3.Slerp(transform.forward, movementVector, Time.deltaTime*rotationSmoothMultiplier);
    }
}
