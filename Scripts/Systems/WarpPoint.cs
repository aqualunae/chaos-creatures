using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Warp Point ", menuName = "World/Warp Point")]
public class WarpPoint : ScriptableObject
{
    [SerializeField]
    private Vector3 position;

    [SerializeField]
    private string sceneName;

    public Vector3 Position
    {
        get => position;
        set {
            position = value;
            # if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            # endif
        }
    }

    public string SceneName
    {
        get => sceneName;
        set {
            sceneName = value;
            # if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            # endif
        }
    }
}
