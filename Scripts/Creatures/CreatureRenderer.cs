using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class CreatureRenderer : MonoBehaviour
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
