using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Data/QuestSO")]
public class QuestSO : ScriptableObject, IHasID
{
    [SerializeField] private int _ID;
    [SerializeField] private List<RequiredCondition> _requiredConditions;
    [SerializeField] private List<InternalItemData> _requiredItems;
    [SerializeField] private List<InternalItemData> _rewards;

    public int ID { get {return _ID;} set { } }
    public List<RequiredCondition> RequiredConditions => _requiredConditions;
    public List<InternalItemData> RequriedItems => _requiredItems;
    public List<InternalItemData> Rewards => _rewards;

}

public enum QuestStatus
{
    NotAccept,
    Accept,
    Complete
}