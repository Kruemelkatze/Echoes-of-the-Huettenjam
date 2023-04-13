using UnityEngine;

namespace Helpers
{
    public class ActivateChildrenOnAwake : MonoBehaviour
    {
        private void Awake()
        {
            var children = transform.childCount;
            for (int i = 0; i < children; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
