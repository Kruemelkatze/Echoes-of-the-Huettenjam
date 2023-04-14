using UnityEngine;

public class ClueBlank : MonoBehaviour
{
    [SerializeField] private string shouldBeText = "Clueless";
    [SerializeField] private DraggableClue installedClue;

    public bool IsFulfilled()
    {
        return installedClue && TextRoughlyEquals(installedClue.Text, shouldBeText);
    }

    public static bool TextRoughlyEquals(string text1, string text2)
    {
        return text1?.ToLower().Replace(" ", "").Trim() == text2?.ToLower().Replace(" ", "").Trim();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}