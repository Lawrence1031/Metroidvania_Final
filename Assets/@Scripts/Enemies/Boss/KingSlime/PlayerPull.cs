using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPull : MonoBehaviour
{
    private bool _isPlayerEnter;
    private Rigidbody2D _playerRigidbody;
    private Transform _transform;
    private Vector2 _pullDirection;

    private void Awake()
    {
        _isPlayerEnter = false;
        _pullDirection = Vector2.zero;
        _transform = GetComponentInParent<Transform>();
    }

    private void FixedUpdate()
    {
        if(_isPlayerEnter)
        {
            _playerRigidbody.transform.position += (Vector3) _pullDirection * 0.21f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            _isPlayerEnter = true;
            _playerRigidbody = other.GetComponent<Rigidbody2D>();
            _pullDirection =
            other.transform.position.x - _transform.position.x > 0 ?
            Vector2.left : Vector2.right;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            _isPlayerEnter = false;
            _playerRigidbody = null;
            _pullDirection = Vector2.zero;
        }
    }
}
