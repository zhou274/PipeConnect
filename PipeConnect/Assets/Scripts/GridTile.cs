// /*
// Created by Darsan
// */

using System.Linq;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _pathEnd;

    private float _size;
    private bool _hasEnd;
    private int _color;

    public float Size
    {
        get => _size;
        set
        {
            _size = value;
            transform.localScale = Vector3.one * value;
        }
    }

    public Vector2Int Coordinate { get; set; }

    public bool HasEnd
    {
        get => _hasEnd;
        set
        {
            _pathEnd.gameObject.SetActive(value);
            _hasEnd = value;
        }
    }

    public int Color
    {
        get => _color;
        set
        {
            _pathEnd.color = ColorGroup.Default.ElementAt(value);
            _color = value;
        }
    }
}

