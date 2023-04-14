#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class StoryText : MonoBehaviour
{
    [SerializeField] private ClueBlank[] blanks;

    void Start()
    {
        blanks = GetComponentsInChildren<ClueBlank>();
    }

    public bool IsFulfilled()
    {
        foreach (var blank in blanks)
        {
            var isFullfilled = blank.IsFulfilled();
            Debug.Log($"{blank.gameObject.name} is fulfilled: {isFullfilled}");
            if (!isFullfilled)
            {
                return false;
            }
        }

        return true;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(StoryText))]
    private class StoryTextEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!(target is StoryText script))
                return;

            if (GUILayout.Button("Check if fulfilled"))
            {
                Debug.Log($"Story is fulfilled: {script.IsFulfilled()}");
            }
        }
    }
#endif
}