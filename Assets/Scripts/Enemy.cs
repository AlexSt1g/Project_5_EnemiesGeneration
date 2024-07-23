using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _minHeightForLiving = -15f;

    private Vector3 _movementDirection;

    public event Action<Enemy> Died;    

    public void SetMovementDirection(Vector3 direction)
    {
        _movementDirection = direction;        
    }

    private void Update()
    {
        transform.Translate(_movementDirection * _moveSpeed * Time.deltaTime);        

        if (transform.position.y < _minHeightForLiving)        
            Died?.Invoke(this);        
    }
}
