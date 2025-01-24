// /*
// Created by Darsan
// */

using System.Collections.Generic;
using UnityEngine;

public class PathTile : MonoBehaviour
{
    private Vector2 _direction;

    [SerializeField] private List<Transform> _openPoints = new List<Transform>();

    public IEnumerable<Transform> OpenPoints => _openPoints;



    public float Size
    {
        get => transform.localScale.x;
        set => transform.localScale = value * Vector3.one;
    }

    public Color Color
    {
        set
        {
            foreach (var spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
            {
                spriteRenderer.color = value;
            }
        }
    }

    public Vector2 Direction
    {
        get => _direction;
        set
        {
            _direction = value;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.Cross(Vector3.forward, value));
        }
    }
}