// /*
// Created by Darsan
// */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ColorGroup : ScriptableObject,IEnumerable<Color>
{
    public static ColorGroup Default => Resources.Load<ColorGroup>(nameof(ColorGroup));

    [SerializeField] private List<Color> _colors = new List<Color>();

#if UNITY_EDITOR
    [UnityEditor.MenuItem("MyGames/Colors")]
    public static void Open()
    {
        GamePlayEditorManager.OpenScriptableAtDefault<ColorGroup>();
    }
#endif
    public IEnumerator<Color> GetEnumerator()
    {
        return _colors.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}