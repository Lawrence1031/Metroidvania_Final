using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapEditorWindow : EditorWindow
{
    [MenuItem("Editor/MapEditor")]
    static public void Init()
    {
        MapEditorWindow window = GetWindow<MapEditorWindow>(typeof(Square));
        window.minSize = new Vector2(100, 100);
        window.maxSize = new Vector2(500, 500);
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Box(Resources.Load<Texture2D>("Slime00"), GUILayout.Width(50), GUILayout.Height(50));

        if (GUILayout.Button("��ư"))
        {
            Debug.Log("��ư ����");
        }
        if (GUILayout.RepeatButton("�ݺ� ��ư"))
        {
            Debug.Log("�ݺ� ��ư ����");
        }
        if (EditorGUILayout.DropdownButton(new GUIContent("��� �ٿ� ��ư"), FocusType.Keyboard))
        {
            Debug.Log("��� �ٿ� ��ư ����");
        }
    }
}
