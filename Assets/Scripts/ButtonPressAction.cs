using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPressAction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private ButtonState buttonState;

    public void OnPointerDown(PointerEventData eventData)
    {
        GameStateManager.Instance.SetButtonState(buttonState);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameStateManager.Instance.SetButtonState(ButtonState.None);
    }
}