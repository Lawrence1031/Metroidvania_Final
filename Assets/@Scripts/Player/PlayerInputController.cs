using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : CharacterController
{
    #region Properties

    private Vector2 _movementDirection = Vector2.zero;
    private Vector2 _aimDirection = Vector2.right;
    private Rigidbody2D _rigidbody;
    private Player _player;
    private float _timeSinceLastAttack = float.MaxValue;
    protected bool _isAttacking { get; set; }
    private bool _isGrounded;
    private float _jumpPowar = 5f;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _player = GetComponent<Player>();
        _isGrounded = true;
    }

    private void Start()
    {
        OnMoveEvent += Move;
        OnLookEvent += OnAim;
    }

    protected override void Update()
    {
        base.Update();
        //AttackDelay();
    }

    #endregion


    #region Move
    public void OnMove(InputValue value)
    {
        Vector2 moveInput = value.Get<Vector2>().normalized;
        CallMoveEvent(moveInput);
    }

    private void FixedUpdate()
    {
        ApplyMovment(_movementDirection);
        CheckGrounded();
    }

    private void Move(Vector2 direction)
    {
        _movementDirection = direction;
    }

    private void ApplyMovment(Vector2 direction)
    {
        direction *= 5f; //�÷��̾� speed
        //direction += KnockbackDirection;
        _rigidbody.velocity = direction;
    }

    //public Vector2 KnockbackDirection = Vector2.zero;
    #endregion

    #region Look
    public void OnLook(InputValue value)
    {
        Vector2 newAim = value.Get<Vector2>();
        CallLookEvent(newAim);
    }

    public void OnAim(Vector2 newAimDirection)
    {
        _aimDirection = newAimDirection.normalized;
    }
    #endregion

    #region Jump

    public void OnJump()
    {
        if (_isGrounded)
        {
            _rigidbody.AddForce(Vector2.up * _jumpPowar, ForceMode2D.Impulse);
            _isGrounded = false; // ���� �ִ� ���¸� ������Ʈ�մϴ�.
            Debug.Log("Jump");
        }
    }

    private void CheckGrounded()
    {
        // �÷��̾ ���� �ִ����� �����ϱ� ���� ����ĳ��Ʈ �Ǵ� �ٸ� �� üũ ��Ŀ������ �����մϴ�.
        // �ܼ�ȭ�� ����, �÷��̾��� y-��ġ�� ���� ������ ���� �ִ� ������ �����մϴ�.
        float groundCheckDistance = 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance);

        // �� üũ ����� ������� ���� �ִ��� ���θ� ������Ʈ�մϴ�.
        _isGrounded = hit.collider != null;
    }
    #endregion

    #region Dash

    public void OnDash()
    {
        Debug.Log("Dash");

    }

    #endregion

    #region Attack

    public void OnAttack()
    {
        Debug.Log("Attack");

    }

    #endregion
}
