using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    #region Properties

    public bool Iswalking
    {
        get
        {
            return _isWalking;
        }
        private set
        {
            _isWalking = value;
            _animator.SetBool(AnimatorHash.Walk, value);
        }
    }

    #endregion

    #region Fileds

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private TouchingDirection _touchingDirection;
    private Player _player;
    private bool _isFacingRight = true;
    private float _speed = 10f;
    private Vector2 _moveInput;

    private bool _isWalking = false;
    

    private bool _isAttacking = false;

    //Jump
    private float _jumpPower = 20f;
    public int _maxJump = 1;
    public int _jumpCount;

    //Gravity
    private float _baseGravity = 4f;
    private float _maxFallSpeed = 20f;
    private float _fallSpeedMultiplier = 2f;

    //Wall Slide
    private float _wallSlideSpeed = 0f;
    private bool _isWallSliding;

    public bool IsWallSliding { get 
        {
            return _isWallSliding; 
        } 
        private set
        {
            _isWallSliding = value;
            _animator.SetBool(AnimatorHash.WallSliding, value);
        }
    }

    // WallJump
    private bool _isWallJumping;
    private float _wallJumpDirection;
    private float _wallJumpTime = 0.3f;
    private float _wallJumpTimer;
    private Vector2 _wallJumpPower = new Vector2(5f, 15f);

    // Dash
    private bool _canDash = true;
    private bool _isDashing;
    private float _dashPower = 15f;
    private float _dashTime = 0.2f;
    private int _maxDash = 1;
    public int _dashCount;
    private TrailRenderer _trailRenderer;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _touchingDirection = GetComponent<TouchingDirection>();
        _trailRenderer = GetComponent<TrailRenderer>();
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();

        //임시 스킬 초기화. ==================================================================
        _SwordAuror = GetComponent<Skill_SwordAuror>();
        _PlungeAttack = GetComponent<Skill_PlungeAttack>();
        _SwordAuror.Initialize();
        _PlungeAttack.Initialize();
        //====================================================================================
    }

    private void FixedUpdate()
    {
        GroundCheck();
        WallSlide();
        WallJump();

        if (!_isWallJumping && !_isDashing && !_player.IsHit)
        {
            _rigidbody.velocity = new Vector2(_moveInput.x * _speed, _rigidbody.velocity.y);
            Flip();
        }

        if (!_isDashing)
        {
            Gravity();
        }

        _animator.SetFloat(AnimatorHash.yVelocity, _rigidbody.velocity.y);
    }

    #endregion

    #region Movement

    public void Move(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();

        Iswalking = _moveInput != Vector2.zero;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (_jumpCount > 0)
        {
            if (context.started && _touchingDirection.IsGrounded)
            {
                if (!_isWallJumping)
                {
                    _animator.SetTrigger(AnimatorHash.Jump);
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpPower);
                    _jumpCount--;
                }
            }
            else if (context.canceled)
            {
                if (!_isWallJumping)
                {
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
                    _jumpCount--;
                }
            }
        }

        //Wall Jump
        if (context.performed && _wallJumpTimer > 0f)
        {
            if (!_isWallJumping && _touchingDirection.IsWall)
            {
                _isWallJumping = true;
                _rigidbody.velocity = new Vector2(_wallJumpDirection * _wallJumpPower.x, _wallJumpPower.y);
                _wallJumpTimer = 0f;
                _animator.SetTrigger(AnimatorHash.WallJump);
                if (transform.localScale.x != _wallJumpDirection)
                {
                    _isFacingRight = !_isFacingRight;
                    Vector3 Is = transform.localScale;
                    Is.x *= -1f;
                    transform.localScale = Is;
                }

                Invoke(nameof(CancelWallJump), _wallJumpTime + 0.1f);
            }
        }
    }

    private void WallSlide()
    {
        if (!_touchingDirection.IsGrounded & _touchingDirection.IsWall & _moveInput.x != 0)
        {
            _isWallSliding = true;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, Mathf.Max(_rigidbody.velocity.y, -_wallSlideSpeed));
        }
        else
        {
            _isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (_isWallSliding)
        {
            _isWallJumping = false;
            _wallJumpDirection = -transform.localScale.x;
            _wallJumpTimer = _wallJumpTime;

            CancelInvoke(nameof(CancelWallJump));
        }
        else if (_wallJumpTimer > 0f)
        {
            _wallJumpTimer -= Time.deltaTime;
        }
    }

    private void CancelWallJump()
    {
        _isWallJumping = false;
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && _isAttacking == false)
        { 
            _isAttacking = true;
            _animator.SetTrigger(AnimatorHash.Attack);
            StartCoroutine(ResetAttackAnimation());
        }
    }

    // TODO 리펙토링 필요해 보임
    private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        _isAttacking = false;
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && _canDash == true)
        {
            StartCoroutine(CoDash());
        }
    }

    private IEnumerator CoDash()
    {
        if (_dashCount > 0)
        {
            _animator.SetTrigger(AnimatorHash.Dash);
            _canDash = false;
            _isDashing = true;
            _player.Invincible = true;
            float originalGravity = _baseGravity;
            _rigidbody.gravityScale = 0f;
            _rigidbody.velocity = new Vector2(transform.localScale.x * _dashPower, 0f);
            _trailRenderer.emitting = true;
            yield return new WaitForSeconds(_dashTime);
            _trailRenderer.emitting = false;
            _rigidbody.gravityScale = originalGravity;
            _isDashing = false;
            _dashCount--;
            yield return new WaitForSeconds(0.1f);
            _canDash = true;
        }
    }

    // 임시 ===================================================================
    private Skill_SwordAuror _SwordAuror;
    private Skill_PlungeAttack _PlungeAttack;
    // ========================================================================

    public void Skill(InputAction.CallbackContext context)
    {
        if (!Input.GetKey(KeyCode.DownArrow) && context.performed)
        {
            _SwordAuror.Activate();
        }
        else if (Input.GetKey(KeyCode.DownArrow) && !_touchingDirection.IsGrounded && context.performed)
        {
            _PlungeAttack.Activate();
            _player.Invincible = true;
        }
    }

    public void Interaction(InputAction.CallbackContext context)
    {
        //if (상호작용이 가능한 오브젝트 && context.performed)
        //{
        //    상호작용 오브젝트에서 실행될 메서드?
        //}
    }

    private void Gravity()
    {
        if (_rigidbody.velocity.y < 0)
        {
            _rigidbody.gravityScale = _baseGravity * _fallSpeedMultiplier;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, Mathf.Max(_rigidbody.velocity.y, -_maxFallSpeed));
        }
        else
        {
            _rigidbody.gravityScale = _baseGravity;
        }
    }

    
    

    #endregion

    #region Check

    private void GroundCheck()
    {
        if (_touchingDirection.IsGrounded)
        {
            _jumpCount = _maxJump;
            _dashCount = _maxDash;
        }
    }

    #endregion

    private void Flip()
    {
        if(_isAttacking == false)
        {
            if (_isFacingRight && _moveInput.x < 0 || !_isFacingRight && _moveInput.x > 0)
            {
                _isFacingRight = !_isFacingRight;
                Vector3 Is = transform.localScale;
                Is.x *= -1f;
                transform.localScale = Is;
            }
        }
    }

    #region Attribute Method

    // Controller에 필요없음
   

    #endregion
}
