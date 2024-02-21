using System;
using System.Collections.Generic;

public class PlayerStatus
{
    public Dictionary<PlayerStatusType, float> Stats {get; private set;}

    public PlayerStatus(PlayerStatusData data)
    {
        // 초기화용 생성자
        Stats = new Dictionary<PlayerStatusType, float>()
        {
            {PlayerStatusType.HP, data.HP},
            {PlayerStatusType.Damage, data.Damage},
            {PlayerStatusType.Mana, data.Mana},
            {PlayerStatusType.ManaRegenerate, data.ManaRegenerate},
            {PlayerStatusType.AttackSpeed, data.AttackSpeed}
        };
    }

    public void AddStat(PlayerStatusType type, float value)
    {
        // 게임 특성상 복잡한 연산이 필요하지 않아, 단순한 형태로 만들었습니다.
        Stats[type] += value;
    }

    public PlayerStatusData GetSaveData()
    {
        PlayerStatusData data = new PlayerStatusData()
        {
            HP = Stats[PlayerStatusType.HP],
            Damage = Stats[PlayerStatusType.Damage],
            Mana = Stats[PlayerStatusType.Mana],
            ManaRegenerate = Stats[PlayerStatusType.ManaRegenerate],
            AttackSpeed = Stats[PlayerStatusType.AttackSpeed]
        };
        return data;
    }
}

[Serializable]
public enum PlayerStatusType
{
    None,
    HP,
    Damage,
    Mana,
    ManaRegenerate,
    AttackSpeed
}

[Serializable]
public class PlayerStatusData
{
    // TODO => Defalut value 여기서 설정하면 될 듯??
    public float HP;
    public float Damage;
    public float Mana;
    public float ManaRegenerate;
    public float MovementSpeed;
    public float AttackSpeed;
}
