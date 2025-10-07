using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MCGhostController : MonoBehaviour
{

    [Header("Movement")]
    public float moveSpeed = 5f;
    private Vector3 _input;
    private Rigidbody _rb;
    private Vector3 _moveDirection;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.isKinematic = false;
        _rb.freezeRotation = true;
    }


    void Update()
    {
        GatherInput();
        Rotate();
    }

    void FixedUpdate()
    {
        Move();
    }
    void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        _input = Vector3.ClampMagnitude(_input, 1);
    }

    void Move()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        _moveDirection = (cameraForward * _input.z + cameraRight * _input.x).normalized;
        Vector3 newPosition = transform.position + _moveDirection * moveSpeed * Time.deltaTime;
        _rb.MovePosition(newPosition);

    }

    void Rotate()
    {
        if (_moveDirection.sqrMagnitude > 0.01f) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);
            targetRotation *= Quaternion.Euler(0, 75f, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}
