using General;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Banana : Clue
{
    public override void Interact()
    {
        Hub.Get<ClueCanvas>().Win();
    }
}