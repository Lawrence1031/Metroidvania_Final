using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    #region Fields
    protected bool _isDeadEnded;
    protected Transform _spriteTransform;
    #endregion
    public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _spriteTransform = stateMachine.transform.Find("Sprite");
        stateMachine.EventReceiver.OnDeadEnded += EndState;
    }
    #region IState
    public override void OnStateEnter()
    {
        _isDeadEnded = false;
        var attackComponents = _stateMachine.transform.Find("Sprite").GetComponentsInChildren<EnemyBodyAttackSystem>();
        _stateMachine.gameObject.layer = LayerMask.NameToLayer("EnemyDead");
        foreach (var component in attackComponents)
        {
            component.Collider.enabled = false;
        }
        _animator.SetTrigger(AnimatorHash.Dead);
        PlayDeadSound();
        GenerateDeadEffect();
    }

    public override void OnStateExit()
    {
    }

    public override void OnStateStay()
    {
        if(_isDeadEnded)
        {
            DropManager.Instance.DropItem(_stateMachine.EnemyData.DropTableIndex, _spriteTransform.position);
            _stateMachine.gameObject.SetActive(false);
            _stateMachine.DisappearMonster();
        }
    }
    #endregion
    
    private void GenerateDeadEffect()
    {
        GameObject effect = Resources.Load<GameObject>("Enemies/Effects/EnemyDeadEffect");
        Transform transform = GameObject.Instantiate(effect).GetComponent<Transform>();
        transform.localPosition = _spriteTransform.position;
    }

    private void EndState()
    {
        _isDeadEnded = true;
    }

    private void PlayDeadSound()
    {
        SFX.Instance.PlayOneShot("EnemyDeadSound", 0.3f);
    }
}
