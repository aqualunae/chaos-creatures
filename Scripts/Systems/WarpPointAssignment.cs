using UnityEngine;

[ExecuteInEditMode]
public class WarpPointAssignment : MonoBehaviour
{
    [SerializeField, Tooltip("Scriptable Object that will be used to access this location.")]
    private WarpPoint warpPoint;

    [SerializeField, Tooltip("Set the exit point outside of the collider so that the warps don't ricochet.")]
    private GameObject exitPoint;

    // start not awake so that the scene is loaded before this is called
    private void Start()
    {
        #if UNITY_EDITOR

        // if for whatever reason the scene or the object is not loaded, don't touch the warp point
        if (string.IsNullOrEmpty(gameObject.scene.name) || !gameObject.activeInHierarchy)
        {
            return;
        }

        if (Application.isEditor)
        {
            warpPoint.Position = exitPoint.transform.position;
            warpPoint.SceneName = gameObject.scene.name;
        }

        #endif
    }
}
