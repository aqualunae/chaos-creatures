using UnityEngine;

[CreateAssetMenu(fileName = "Warp Point ", menuName = "World/Warp Point")]
public class WarpPoint : ScriptableObject
{
    private Vector3 position;
    private string sceneName;

    public Vector3 Position
    {
        get => position;
        set => position = value;
    }

    public string SceneName
    {
        get => sceneName;
        set => sceneName = value;
    }
}
