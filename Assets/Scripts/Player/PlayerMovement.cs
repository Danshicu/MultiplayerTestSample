using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader;

    [SerializeField] private Transform _bodyTransform;

    [SerializeField] private Rigidbody2D _rigidbody;

    [SerializeField] private float movementSpeed = 4f;

    [SerializeField] private float turningRate = 30f;

    private Vector2 _movementInput;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        _inputReader.MoveEvent += HandleMove;
    }

    private void HandleMove(Vector2 input)
    {
        _movementInput = input;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        _inputReader.MoveEvent -= HandleMove;
    }

    private void Move()
    {
        
    }

    private void FixedUpdate()
    {
        if(!IsOwner) return;

        _rigidbody.velocity = (Vector2)_bodyTransform.up * _movementInput.y * movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner) return;
        float zRotation = _movementInput.x * -turningRate * Time.deltaTime;
        _bodyTransform.Rotate(0f,0f, zRotation);
    }
}
