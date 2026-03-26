using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private float minX = -8f;
    private float maxX = 8f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameStateManager.Instance.CurrentState == GameState.IsPlaying)
        {
            // 왼쪽 화살표 OR A가 눌렸을 때
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || GameStateManager.Instance.CurrentButton == ButtonState.Left)
            {
                //transform.Translate(-2, 0, 0);              // 왼쪽으로 '2'움직인다.
                transform.Translate(speed * Time.deltaTime * Vector3.left, Space.World);
            }

            // 오른쪽 화살표 OR D가 눌렸을 때
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || GameStateManager.Instance.CurrentButton == ButtonState.Right)
            {
                //transform.Translate(2, 0, 0);              // 오른쪽으로 '2'움직인다.
                transform.Translate(speed * Time.deltaTime * Vector3.right, Space.World);
            }

            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            transform.position = pos;
        }
    }
}
