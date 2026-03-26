using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 8f;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (GameStateManager.Instance.CurrentState != GameState.IsPlaying)
            return;

        float horizontal = GetHorizontalInput();
        if (Mathf.Abs(horizontal) > 0.01f)
        {
            transform.Translate(Vector3.right * horizontal * moveSpeed * Time.deltaTime, Space.World);
        }

        ClampPosition();
    }

    public void ResetPosition()
    {
        Vector3 pos = transform.position;
        pos.x = 0f;
        transform.position = pos;
    }

    private float GetHorizontalInput()
    {
        float input = 0f;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            input -= 1f;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            input += 1f;

        switch (GameStateManager.Instance.CurrentButton)
        {
            case ButtonState.Left:
                input -= 1f;
                break;
            case ButtonState.Right:
                input += 1f;
                break;
        }

        return Mathf.Clamp(input, -1f, 1f);
    }

    private void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;
    }
}