using Extensions;
using UnityEngine;

public class LimitToSpriteBounds : MonoBehaviour
{
    [SerializeField] private SpriteRenderer targetSprite;

    private Camera _cam;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
    }

    private void OnDrawGizmosSelected()
    {
        if (!targetSprite)
            return;

        Gizmos.color = Color.red;
        var tb = targetSprite.bounds;
        Gizmos.DrawWireCube(tb.center, tb.extents * 2);
    }

    void LateUpdate()
    {
        if (!targetSprite || !_cam)
            return;

        var b = _cam.OrthographicBounds();
        var tb = targetSprite.bounds;

        if (b.min.x < tb.min.x)
        {
            b.center += new Vector3(tb.min.x - b.min.x, 0, 0);
        }

        if (b.max.x > tb.max.x)
        {
            b.center -= new Vector3(b.max.x - tb.max.x, 0, 0);
        }

        if (b.min.y < tb.min.y)
        {
            b.center += new Vector3(0, tb.min.y - b.min.y, 0);
        }

        if (b.max.y > tb.max.y)
        {
            b.center -= new Vector3(0, b.max.y - tb.max.y, 0);
        }

        transform.position = b.center;
    }
}