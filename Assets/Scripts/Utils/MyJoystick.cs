using UnityEngine;
using UnityEngine.EventSystems;

public class MyJoystick : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    /// <summary>
    /// Normalized vector.
    /// </summary>
    public Vector2 Direction { get; private set; }
    public bool interactible = true;
    public float Horizontal => Direction.x;
    public float Vertical => Direction.y;

    public System.Action onDragBegin;
    public System.Action onPointerUp;

    public float InactiveZoneSqr
    {
        get { return inactiveZone; }
        set { inactiveZone = Mathf.Pow(value, 2); }
    }

    [SerializeField, Min(0)] float inactiveZone = 0;


    [SerializeField] protected RectTransform background = null;
    [SerializeField] RectTransform handle = null;
    private RectTransform baseRect = null;

    //[SerializeField] RectTransform baseRect = null;

    private Canvas canvas;
    private Camera cam;

    Vector2 radius;

    protected virtual void Start()
    {
        InactiveZoneSqr = inactiveZone;

        canvas = GetComponentInParent<Canvas>();
        baseRect = GetComponent<RectTransform>();
        if (canvas == null)
            MyDebug.LogError("The Joystick is not placed inside a canvas");

        Vector2 center = new Vector2(0.5f, 0.5f);
        background.pivot = center;
        handle.anchorMin = center;
        handle.anchorMax = center;
        handle.pivot = center;
        handle.anchoredPosition = Vector2.zero;

        radius = background.sizeDelta / 2;
    }


    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (!interactible)
            return;
        onDragBegin?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!interactible)
            return;

        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            cam = canvas.worldCamera;
        //else
        //    Mydebug.LogError("canvas.renderMode != RenderMode.ScreenSpaceCamera");


        Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
        Direction = (eventData.position - position) / (radius * canvas.scaleFactor);
        HandleInput(Direction.sqrMagnitude);
        handle.anchoredPosition = Direction * radius;
    }

    public void HandleInput(float sqrMagnitude)
    {
        if (sqrMagnitude > InactiveZoneSqr)
        {
            if (sqrMagnitude > 1)
            {
                Direction = Direction.normalized;
            }
        }
        else
        {
            Direction = Vector2.zero;
        }
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (interactible)
            onPointerUp?.Invoke();

        Direction = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition) {
        Vector2 localPoint = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out localPoint)) {
            Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;
            return localPoint - (background.anchorMax * baseRect.sizeDelta) + pivotOffset;
        }
        return Vector2.zero;
    }
}
