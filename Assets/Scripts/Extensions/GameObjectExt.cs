using General;
using UnityEngine;

namespace Extensions
{
    public static class GameObjectExt
    {
        public static void SetLayerRecursively(this GameObject go, int layer)
        {
            go.layer = layer;
            var t = go.transform;
            for (int i = 0; i < t.childCount; i++)
            {
                SetLayerRecursively(t.GetChild(i).gameObject, layer);
            }
        }

        public static bool HasPlayerTag(this GameObject go)
        {
            return go.CompareTag(Constants.Namings.Player);
        }

        public static bool IsParentOrEqual(this GameObject parent, GameObject potentialChild)
        {
            if (!potentialChild)
                return false;

            if (potentialChild == parent)
                return true;

            var obj = potentialChild.transform;
            while ((bool) (obj = obj.transform.parent))
            {
                if (parent == obj.gameObject)
                    return true;
            }

            return false;
        }

        public static bool IsPrefab(this GameObject go)
        {
            // https://answers.unity.com/questions/1730693/how-to-detect-if-prefab-was-added-to-the-scene-in.html
            var hasNoSceneName = string.IsNullOrEmpty(go.scene.name);

            if (hasNoSceneName)
                return true;

#if UNITY_EDITOR
            if (UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() != null)
            {
                return true;
            }
#endif
            return false;
        }
    }
}