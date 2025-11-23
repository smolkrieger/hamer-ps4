using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class JoystickControll : MonoBehaviour, IDragHandler, IEventSystemHandler, IPointerUpHandler, IPointerDownHandler
{
	private Image joystickBorder;

	private Image joystickCircle;

	private Vector2 inputVector;

	private CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis;

	private CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis;

	public string horizontalAxisName = "Horizontal";

	public string verticalAxisName = "Vertical";

	private void OnEnable()
	{
		CreateVirtualAxes();
	}

	private void CreateVirtualAxes()
	{
		m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
		CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
		m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
		CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
	}

	private void Start()
	{
		joystickBorder = GetComponent<Image>();
		joystickCircle = base.transform.GetChild(0).GetComponent<Image>();
	}

	public virtual void OnPointerDown(PointerEventData stick)
	{
		OnDrag(stick);
	}

	public virtual void OnPointerUp(PointerEventData stick)
	{
		inputVector = Vector2.zero;
		UpdateAxis(inputVector);
		joystickCircle.rectTransform.anchoredPosition = Vector2.zero;
	}

	public virtual void OnDrag(PointerEventData stick)
	{
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBorder.rectTransform, stick.position, stick.pressEventCamera, out var localPoint))
		{
			localPoint.x /= joystickBorder.rectTransform.sizeDelta.x;
			localPoint.y /= joystickBorder.rectTransform.sizeDelta.y;
			inputVector = new Vector2(localPoint.x, localPoint.y);
			inputVector = ((inputVector.magnitude > 1f) ? inputVector.normalized : inputVector);
			joystickCircle.rectTransform.anchoredPosition = new Vector2(inputVector.x * (joystickBorder.rectTransform.sizeDelta.x / 2f), inputVector.y * (joystickBorder.rectTransform.sizeDelta.y / 2f));
		}
		UpdateAxis(inputVector);
	}

	private void UpdateAxis(Vector2 axis)
	{
		m_HorizontalVirtualAxis.Update(axis.x);
		m_VerticalVirtualAxis.Update(axis.y);
	}

	private void OnDisable()
	{
		m_HorizontalVirtualAxis.Remove();
		m_VerticalVirtualAxis.Remove();
	}
}
