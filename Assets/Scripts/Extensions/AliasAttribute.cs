using UnityEditor;
using UnityEngine;

namespace Extensions.Editor
{
    public class AliasAttribute : PropertyAttribute
    {
        public string NewName { get; }

        public AliasAttribute(string name)
        {
            NewName = name;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(AliasAttribute))]
    public class AliasAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, new GUIContent((attribute as AliasAttribute).NewName));
        }
    }
#endif
}