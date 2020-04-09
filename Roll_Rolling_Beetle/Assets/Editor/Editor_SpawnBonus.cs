using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(SpawnBonus))]
[CanEditMultipleObjects]
public class Editor_SpawnBonus : Editor
{
    //This is the value of the Slider
    float m_Value;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SpawnBonus spawner = (SpawnBonus)target;

        if (GUILayout.Button("Spawn PoopTest"))
        {
            spawner.generateBonus();
        }

        if(GUILayout.Button("Erase All"))
        {
            spawner.deletePoop();
        }
    }
}
