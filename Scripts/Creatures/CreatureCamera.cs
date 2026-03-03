using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Converts the collection of sprites for a creature into a flat texture that is then applied to an image, such as for UI elements. Could also be used for characters.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CreatureCamera : MonoBehaviour
{
    [SerializeField, Tooltip("Where should the rendered texture be displayed?")]
    private Image target;

    [SerializeField, Tooltip("Size in pixels of the texture to be rendered.")]
    private Vector3Int imageDimensions = new(64, 64, 1);

    [SerializeField]
    private Shader shader;

    private void Awake()
    {
        // get the camera that this component is attached to
        Camera camera = GetComponent<Camera>();

        // create a texture using the specified dimensions
        RenderTexture texture = new(imageDimensions.x, imageDimensions.y, imageDimensions.z)
        {
            filterMode = FilterMode.Point
        };

        // tell the camera to send its output to the created texture
        camera.targetTexture = texture;

        // create a new material using the texture
        Material material = new(shader)
        {
            mainTexture = texture
        };

        // assign the created material to the target output
        target.material = material;
    }
}
