using UnityEngine;
using UnityEditor;
using AYellowpaper.SerializedCollections;

public class RespawnPointDictionary : MonoBehaviour
{
    [SerializedDictionary("Checkpoints", "RespawnPoints")]
    [SerializeField] private SerializedDictionary<Transform, Transform> respawnPoints;

    public void UpdateDictionary()
    {
        //This part makes sure that the assigned value is the correct respawn point for the checkpoint
        Checkpoint[] respawnPointsArray = FindObjectsOfType<Checkpoint>();
        foreach (Checkpoint checkpoint in respawnPointsArray)
        {
            if (checkpoint != null && checkpoint.respawnPoint != null && respawnPoints.ContainsKey(checkpoint.gameObject.transform))
            {
                checkpoint.respawnPoint = respawnPoints[checkpoint.gameObject.transform];
            }
        }

        respawnPoints.Clear();

        foreach (Checkpoint Checkpoint in respawnPointsArray)
        {
            if (Checkpoint != null)
            {
                if (Checkpoint.respawnPoint != null)
                {
                    respawnPoints.Add(Checkpoint.gameObject.transform, Checkpoint.respawnPoint);
                }
            }
        }
    }
}

//Adds the Update Dictionary button in the inspector
#if UNITY_EDITOR
[CustomEditor(typeof(RespawnPointDictionary))]
public class RespawnPointDictionaryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RespawnPointDictionary script = (RespawnPointDictionary)target;
        if (GUILayout.Button("Update Dictionary"))
        {
            script.UpdateDictionary();
        }
    }
}
#endif