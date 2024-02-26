using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCInteraction_Constructor_Event : NPCInteraction
{
    // 조우하면 이제부터 마을에서 건축가 NPC를 만날 수 있게 됩니다.
    [SerializeField] private int _chatID_start;
    [SerializeField] private int _chatID_end;

    public override IEnumerator Interact(PlayerInput input)
    {
        StartInteract(input);

        var chatDatas = ChatManager.Instance.GetChatData(_chatID_start);
        yield return StartCoroutine(_chatBoxUI.StartChat(chatDatas));

        // Item docs ref.
        ItemManager.Instance.AddItem(ItemType.NPC, 1);
        ItemManager.Instance.UseItem(ItemType.NPC, 5, 1);
        transform.parent.Find("Sprite").gameObject.SetActive(false);

        chatDatas = ChatManager.Instance.GetChatData(_chatID_end);
        yield return StartCoroutine(_chatBoxUI.StartChat(chatDatas));

        EndInteract(input);

        // TODO 해결 전 임시 코드
        transform.parent.gameObject.SetActive(false);
    }
}