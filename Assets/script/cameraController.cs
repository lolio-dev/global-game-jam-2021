using UnityEngine;

using CommonsDebug;
using CommonsHelper;

public class cameraController : MonoBehaviour
{
    public Camera cam;

    public GameObject player1;
    public GameObject player2;
    public Transform bottomLimitTransform;
    public Transform leftLimitTransform;
    public Transform rightLimitTransform;


    [Header("Parameters")]

    [Range(1f, 10f)]
    public float distanceAroundCharacter = 1f;

    [Tooltip("Minimum span in X direction that the camera can view (also affects orthographic size)")]
    public float minSpanX = 5f;

    [Tooltip("Maximum span in X direction that the camera can view (also affects orthographic size)")]
    public float maxSpanX = 30f;


    /* Tracked state */

    public float camPositionX;
    public float camPositionY;


    void LateUpdate()
    {
        // Get bounding rectangle containing both characters, with margin (distanceAroundCharacter)
        Rect playerCharacterRect1 = GetCharacterViewRect(player1.transform.position);
        Rect playerCharacterRect2 = GetCharacterViewRect(player2.transform.position);
        Rect bothCharactersMinimumBoundingRect = RectUtil.MBR(playerCharacterRect1, playerCharacterRect2);

        #if UNITY_EDITOR
        DebugUtil.DrawRect(bothCharactersMinimumBoundingRect, Color.yellow);
        #endif

        // Find envelope that covers target camera view rect, with wanted aspect ratio (we only care about its height)
        Vector2 fixedAspectRatioEnvelope = ComputeFixedAspectRatioEnvelopeSize(bothCharactersMinimumBoundingRect.width, bothCharactersMinimumBoundingRect.height);

        #if UNITY_EDITOR
        Rect debugEnvelope = new Rect(bothCharactersMinimumBoundingRect.center - fixedAspectRatioEnvelope / 2f, fixedAspectRatioEnvelope);
        DebugUtil.DrawRect(debugEnvelope, Color.cyan);
        #endif

        Vector2 envelopeExtent = fixedAspectRatioEnvelope / 2f;

        // Orthographic size should be half height = vertical extent of envelope
        cam.orthographicSize = envelopeExtent.y;

        // Place camera so it sees this envelope, by moving it to envelope/target rect center
        // But also clamp camera Y so that its view rect bottom is clamped to the bottom limit
        camPositionX = bothCharactersMinimumBoundingRect.center.x;
        camPositionY = bothCharactersMinimumBoundingRect.center.y;

        // Apply bottom, left and right limits
        // Note: if camera envelope is too wide, left and right limits cannot be simultaneously verified
        // but as long as the level is wide enough, it shouldn't happen. Else, just set max span X for safety.
        if (bottomLimitTransform != null)
        {
            camPositionY = Mathf.Max(camPositionY, bottomLimitTransform.position.y + envelopeExtent.y);
        }
        if (leftLimitTransform != null)
        {
            camPositionX = Mathf.Max(camPositionX, leftLimitTransform.position.x + envelopeExtent.x);
        }
        if (rightLimitTransform != null)
        {
            camPositionX = Mathf.Min(camPositionX, rightLimitTransform.position.x - envelopeExtent.x);
        }

        transform.position = new Vector3(camPositionX, camPositionY, -10);
    }

    /// Return the view rectangle of a character in world coords, from its position and view range in all 4 directions
    private Rect GetCharacterViewRect (Vector2 characterPosition)
    {
        // Rectangle of half-extent D -> bottom-left corner is at (-D, -D) of center, extent = 2 * D
        return new Rect(characterPosition - distanceAroundCharacter * Vector2.one, 2 * distanceAroundCharacter * Vector2.one);
    }

    private Vector2 ComputeFixedAspectRatioEnvelopeSize (float originalWidth, float originalHeight)
    {
        float minimumViewAspectRatio = originalWidth / originalHeight;

        float viewWidth;
        float viewHeight;
        if (minimumViewAspectRatio > cam.aspect) {
            // target view is "too wide", use target width (clamped) as reference to find out height
            viewWidth = Mathf.Clamp(originalWidth, minSpanX, maxSpanX);
            viewHeight = viewWidth / cam.aspect;
        } else {
            // target view is "too high", set view height to the target height (except if min/max width is reached)
            viewHeight = originalHeight;
            viewWidth = originalHeight * cam.aspect;
            if (viewWidth < minSpanX || viewWidth > maxSpanX) {
                viewWidth = Mathf.Clamp(viewWidth, minSpanX, maxSpanX);
                viewHeight = viewWidth / cam.aspect;
            }
        }
        return new Vector2(viewWidth, viewHeight);
    }
}
