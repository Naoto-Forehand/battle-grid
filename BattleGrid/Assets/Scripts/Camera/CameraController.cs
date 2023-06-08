using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed;
    [SerializeField]
    private float _movementTime;

    [SerializeField]
    private Vector3 _newPosition;
    private Vector3 _startingPosition;

    private void Awake()
    {
        _startingPosition = new Vector2(transform.position.x,transform.position.y);
        _newPosition = _startingPosition;
    }

    private void LateUpdate()
    {
        HandleMovementInput();
    }

    void HandleMovementInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            _newPosition += (transform.up * _movementSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _newPosition += (transform.right * -_movementSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _newPosition += (transform.right * _movementSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            _newPosition += (transform.up * -_movementSpeed);
        }

        transform.position = Vector3.Lerp(transform.position,_newPosition,Time.fixedDeltaTime * _movementTime);
    }
}
