// /*
// Created by Darsan
// */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGroups : ScriptableObject, IEnumerable<LevelGroup>
{
    public static LevelGroups Default => Resources.Load<LevelGroups>(nameof(LevelGroups));

    [SerializeField]private List<LevelGroup> _groups = new List<LevelGroup>();

    public IEnumerator<LevelGroup> GetEnumerator()
    {
        return _groups.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Save()
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("MyGames/Level Groups")]
    public static void Open()
    {
        GamePlayEditorManager.OpenScriptableAtDefault<LevelGroups>();
    }
#endif
}