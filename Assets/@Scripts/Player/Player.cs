using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamagable
{
    public int _maxHp = 10;
    public int _Hp;
    public int _damage = 5;
    public bool IsHit
    {
        get
        {
            return _animator.GetBool(AnimatorHash.Hurt);
        }
        private set
        {
            _animator.SetBool(AnimatorHash.Hurt, value);
        }
    }
    public bool Invincible
    {
        get => _invincible;
        set
        {
            _invincible = value;
            if (_invincible)
            {
                if (_coInvincible != null) StopCoroutine(_coInvincible);
                _coInvincible = StartCoroutine(InvincibleTimer(_invincibilityTime));
            }
        }
    }
    private bool _invincible = false;
    private Coroutine _coInvincible;
    private float _invincibilityTime = 1f;

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        private set
        {
            _isAlive = value;
            _animator.SetBool(AnimatorHash.IsAlive, value);
        }
    }

    private bool _isAlive = true;

    //Skill
    private List<SkillBase> _skills = new();

    public Animator _animator;
    public Rigidbody2D _rigidbody;
    public PlayerInputController _controller;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _controller = GetComponent<PlayerInputController>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _Hp = _maxHp;
        GameManager.Instance.player = this;
    }

    #region Set

    private void SetSkill()
    {
        if(ItemManager.Instance.HasItem(ItemType.Skill, 0))
        {
            _skills.Add(this.AddComponent<Skill_SwordAuror>());
        }
        if (ItemManager.Instance.HasItem(ItemType.Skill, 1))
        {
            _skills.Add(this.AddComponent<Skill_PlungeAttack>());
        }

        for (int i = 0; i < _skills.Count; i++)
        {
            _skills[i].Initialize();
        }
    }

    #endregion

    public void GetDamaged(int damage, Transform target)
    {
        if (Invincible == false && IsAlive)
        {
            Invincible = true;
            StartCoroutine(FlashPlayer());
            IsHit = true;
            GameManager.Instance.player._Hp -= damage;
            StartCoroutine(ResetHurtAnimation());
            StartCoroutine(Knockback(target));

            if (GameManager.Instance.player._Hp <= 0)
            {
                GameManager.Instance.player._Hp = 0;
                OnDie();
            }
        }
    }

    private IEnumerator Knockback(Transform target)
    {
        float direction = Mathf.Sign(target.position.x - transform.position.x);
        Vector2 knockbackDirection = new Vector2(-direction, 1f).normalized;
        _rigidbody.velocity = knockbackDirection * 5f;
        yield return new WaitForSeconds(0.2f);
        _rigidbody.velocity = Vector2.zero;
    }

    private IEnumerator ResetHurtAnimation()
    {
        yield return new WaitForSeconds(0.3f);
        IsHit = false;
    }

    private IEnumerator InvincibleTimer(float invincibilityTime)
    {
        yield return new WaitForSeconds(invincibilityTime);
        Invincible = false;
    }

    // TODO 연속해서 맞을경우 알파값이 낮은 상태로 고정되는 버그 있음 애니메이션에 적용해서 이부분은 없앨수 있을듯
    private IEnumerator FlashPlayer()
    {
        float flashSpeed = 0.1f;
        Color originalColor = GetComponent<SpriteRenderer>().color;
        while (Invincible)
        {
            Color flashColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.1f);
            GetComponent<SpriteRenderer>().color = flashColor;
            yield return new WaitForSeconds(flashSpeed);
            GetComponent<SpriteRenderer>().color = originalColor;
            yield return new WaitForSeconds(flashSpeed);
        }
        yield break;
        //GetComponent<SpriteRenderer>().color = originalColor; //알파값을 마지막에 원래 색으로 변경하는 코드 추가해서 버그 수정
    }


    private void OnDie()
    {
        IsAlive = false;
        _playerInput.enabled = false;
        _animator.SetTrigger(AnimatorHash.Dead);
        StartCoroutine(OnGameOverUI());
    }

    private IEnumerator OnGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        UIManager.Instance.SetFixedUI(false);
        UIManager.Instance.OpenPopupUI(PopupType.GameOver);
        yield return new WaitForSeconds(1f);
    }

    public void OnContinue()
    {
        transform.position = new Vector3(263f, 0f, 0f);
        IsAlive = true;
        _playerInput.enabled = true;
        _Hp = _maxHp;
        UIManager.Instance.SetFixedUI(true);
        UIManager.Instance.ClosePopupUI(PopupType.GameOver);
    }
}
