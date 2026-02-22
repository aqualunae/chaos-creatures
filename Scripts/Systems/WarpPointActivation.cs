using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpPointActivation : MonoBehaviour
{
    [SerializeField, Tooltip("Location that the warping object will warp to when this is activated.")]
    private WarpPoint target;

    [SerializeField, Tooltip("Variable that tells the player where to load when entering a new scene.")]
    private WarpPointVariable entrancePoint;

    [SerializeField, Tooltip("When true, warp on collision. When false, you can pair this with an interaction field, such as a door.")]
    private bool warpOnCollision = true;

    [SerializeField, Tooltip("Player reference. Only required when using an interaction field.")]
    private GameObjectVariable playerRef;

    // the object moving from the location this script is attached to to the target
    // in theory, this could be an npc, but right now only player warping is implemented
    private GameObject objectWarping;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // when an object enters the warp point
        // check if we are warping on collision, if there is a target to warp to, and if the colliding object can move
        if (warpOnCollision && target != null && collision.gameObject.TryGetComponent(out Mover mover))
        {
            // if yes, set the colliding object as the object that is warping and warp it
            objectWarping = mover.gameObject;
            WarpToTarget();
        }
    }

    public void WarpToTarget()
    {
        // if this function was called without setting an object warping
        // and the script has access to the player reference
        // assume the player is the object warping
        if (objectWarping == null && playerRef != null)
        {
            objectWarping = playerRef.Value;
        }

        // if the object warping is the player
        if (objectWarping.CompareTag("Player"))
        {
            // if they're warping to a location that isn't in this scene
            if (!target.SceneName.Equals(SceneManager.GetActiveScene().name))
            {
                // set the entrance point to the target and load that scene
                entrancePoint.Value = target;
                SceneManager.LoadScene(target.SceneName, LoadSceneMode.Single);
            }
            else
            {
                // if they're warping to a target on the same scene
                // just change their position
                objectWarping.transform.position = target.Position;
            }
        }

        // set the object warping to null so that the collision trigger's assignment isn't permanent
        objectWarping = null;
    }
}
