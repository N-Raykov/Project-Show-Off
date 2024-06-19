using UnityEngine;
using UnityEditor;

//Kinda like Unity's [Range(float,float)] slider, but it takes a sounds clip and adjusts the slider's values to 0, clip.length
public class RangeFromAudioClipAttribute : PropertyAttribute
{
    public string AudioSourceFieldName { get; private set; }

    public RangeFromAudioClipAttribute(string audioSourceFieldName)
    {
        AudioSourceFieldName = audioSourceFieldName;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(RangeFromAudioClipAttribute))]
public class RangeFromAudioClipDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        RangeFromAudioClipAttribute rangeAttribute = (RangeFromAudioClipAttribute)attribute;
        SerializedProperty audioSourceProperty = property.serializedObject.FindProperty(rangeAttribute.AudioSourceFieldName);

        if (audioSourceProperty != null && audioSourceProperty.objectReferenceValue is AudioSource audioSource)
        {
            if (audioSource.clip != null)
            {
                float max = audioSource.clip.length;
                property.floatValue = EditorGUI.Slider(position, label, property.floatValue, 0, max);
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "AudioSource has no clip assigned");
            }
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Invalid AudioSource reference");
        }
    }
}
#endif

