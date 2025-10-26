using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;

public class MCGhostController : MonoBehaviour
{

    [Header("Movement")]
    public float moveSpeed = 5f;
    private Vector3 _input;
    private Rigidbody _rb;
    private Vector3 _moveDirection;
    private bool canMove = true;
    private bool hasTeleported = false;
    private Vector3 MCStartPosition;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.isKinematic = false;
        _rb.freezeRotation = true;
        MCStartPosition = transform.position;
    }

    void Start()
    {
 
    }

    void Update()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.state == State.Dialogue)
            {
                if (!hasTeleported)
                {
                    TeleportTo(MCStartPosition, Quaternion.identity);
                    hasTeleported = true;
                }
                canMove = false;
            }
            else
            {
                canMove = true;
                hasTeleported = false;
            }
        }

        if (!canMove) return;
        GatherInput();
        Rotate();
    }

    void FixedUpdate()
    {
        if (!canMove) return;
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

    public void LockMovement()
    {
        canMove = false;
        _rb.linearVelocity = Vector3.zero;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }
    
    public void TeleportTo(Vector3 position, Quaternion rotation)
    {
        _rb.position = position;
        _rb.rotation = rotation;
        _rb.linearVelocity = Vector3.zero;
    }
}
