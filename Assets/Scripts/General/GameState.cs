using System;

[Serializable]
public enum GameState
{
    Starting = 0,
    Playing = 5,
    Paused = 10,
    Changing = 15,
    PlayerDied = 20,
    Finished = 30,
}