using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Skill_PlungeAttack : SkillBase
{
    private GameObject _shockwavePrefab;
    private CinemachineImpulseSource _impulseSource;

    public override void Initialize()
    {
        base.Initialize();
        Cooldown = 5f;
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public override bool Activate()
    {
        if (!base.Activate())
        {
            return false;
        }

        // TODO 스킬 애니메이션
        GameManager.Instance.player._rigidbody.velocity = new Vector2(0, -transform.localScale.y * 30f);
        GameManager.Instance.player._animator.SetTrigger(AnimatorHash.Plunge);
        return true;
    }

    public void Shockwaves()
    {
        CameraManager.Instance.CameraShake(_impulseSource, 1f);
        _shockwavePrefab = Resources.Load<GameObject>("Skills/PlungeAttack");
        Vector3 playerPosition = transform.position;
        float distanceBetweenShockwaves = 1.0f;

        Vector3 leftShockwavePosition = new Vector3(playerPosition.x - distanceBetweenShockwaves, playerPosition.y + 0.55f, playerPosition.z);
        GameObject leftShockwave = Instantiate(_shockwavePrefab, leftShockwavePosition, Quaternion.identity);
        Rigidbody2D leftShockwaveRb = leftShockwave.GetComponent<Rigidbody2D>();

        Vector3 rightShockwavePosition = new Vector3(playerPosition.x + distanceBetweenShockwaves, playerPosition.y + 0.55f, playerPosition.z);
        GameObject rightShockwave = Instantiate(_shockwavePrefab, rightShockwavePosition, Quaternion.Euler(0, 180, 0));
        Rigidbody2D rightShockwaveRb = rightShockwave.GetComponent<Rigidbody2D>();

        if (leftShockwaveRb != null && rightShockwaveRb != null)
        {
            float speed = 30f;
            leftShockwaveRb.velocity = new Vector2(-speed, 0);
            rightShockwaveRb.velocity = new Vector2(speed, 0);
        }
    }
}
