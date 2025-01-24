// /*
// Created by Darsan
// */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using MyGame;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public static event Action LevelCompleted;

    [SerializeField] private AudioClip _winClip;
    [SerializeField] private Board _board;

    public LevelGroup LevelGroup { get; private set; }
    public Level Level { get; private set; }

    


    public State CurrentState { get; private set; } = State.None;
    public bool HasUndo => _board.HasUndo;

    private void Awake()
    {
        Instance = this;
        var loadGameData = GameManager.LoadGameData;
        LevelGroup = loadGameData.LevelGroup;
        Level = loadGameData.Level;
        LoadLevel();
        CurrentState = State.Playing;
        _board.Completed += OverTheGame;
    }

    private void LoadLevel()
    {
       _board.SetUp(Level.grid,Level.paths);
    }

    public void OnClickUndo()
    {
        if(CurrentState!=State.Playing )
            return;
        
        _board.Undo();

    }

    private void Update()
    {

        if(CurrentState != State.Playing)
            return;

      
    }




    private void OverTheGame()
    {
        if(CurrentState!=State.Playing)
            return;

        PlayClipIfCan(_winClip);
        CurrentState = State.Over;
      
        ResourceManager.CompleteLevel(LevelGroup.id,Level.no);
        LevelCompleted?.Invoke();
    }

    private void PlayClipIfCan(AudioClip clip,float volume=0.35f)
    {
        if(!AudioManager.IsSoundEnable || clip ==null)
            return;
        AudioSource.PlayClipAtPoint(clip,Camera.main.transform.position,volume);
    }



    public enum State
    {
        None,Playing,Over
    }

}

[Serializable]
public class LevelGroup:IEnumerable<Level>
{
    public string id;
    public string name;

    public List<Level> levels;

    public IEnumerator<Level> GetEnumerator()
    {
        return levels?.GetEnumerator() ?? Enumerable.Empty<Level>().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}


[Serializable]
public struct Level
{
    public int no;
    public int grid;
    public List<PathData> paths;
}

[Serializable]
public struct PathData : IEnumerable<Vector2Int>
{
    public List<Vector2Int> points;
    public int color;


    public IEnumerator<Vector2Int> GetEnumerator()
    {
        return points.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}