using General;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Banana : Clue
{
    public override bool Interact()
    {
        Hub.Get<ClueCanvas>().Win();
        return true;
    }
}