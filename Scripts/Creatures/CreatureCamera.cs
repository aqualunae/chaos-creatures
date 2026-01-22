using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Converts the collection of sprites for a creature into a flat texture that is then applied to an image, such as for UI elements. Could also be used for characters.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CreatureCamera : MonoBehaviour
{
    [SerializeField]
    private Image target;

    private void Awake()
    {
        Camera camera = GetComponent<Camera>();
        RenderTexture texture = new RenderTexture(64, 64, 1);
        texture.filterMode = FilterMode.Point;
        camera.targetTexture = texture;
        Material material = new Material(Shader.Find("Universal Render Pipeline/2D/Sprite-Unlit-Default"));
        material.mainTexture = texture;
        target.material = material;
    }
}
