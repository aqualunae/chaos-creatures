using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Coroutine movementCoroutine;
    private Vector3 direction3;

    public void Move(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            return;
        }

        direction3 = new Vector3(direction.x, direction.y) * 0.32f;
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
        movementCoroutine = StartCoroutine(Movement());
    }

    private IEnumerator Movement()
    {
        Vector3 originalPosition = transform.position;
        while (Vector2.Distance(transform.position, originalPosition + direction3) > 0.1f)
        {
            transform.Translate(speed * Time.deltaTime * direction3);
            yield return new WaitForEndOfFrame();
        }
        transform.position = originalPosition + direction3;
        yield return null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StopCoroutine(movementCoroutine);
        // prevent collision from disabling movement permanently
    }
}
