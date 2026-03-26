using UnityEngine;
using UnityEngine.UI;

public class ArrowGenerator : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;
    float span = 1.0f;
    float delta = 0;

    [SerializeField] private float minSpan = 0.2f;
    [SerializeField] private float maxSpan = 2.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetNextSpan();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameStateManager.Instance.CurrentState == GameState.IsPlaying)
        {
            this.delta += Time.deltaTime;
            SetNextSpan();

            if (this.delta > span)
            {
                this.delta = 0;
                GameObject go = Instantiate(arrowPrefab);
                int px = Random.Range(-6, 7);
                go.transform.position = new Vector3(px, 7, 0);
            }
        }
    }

    void SetNextSpan()
    {
        float a = Mathf.Min(minSpan, maxSpan);
        float b = Mathf.Max(minSpan, maxSpan);
        this.span = Random.Range(a, b);
    }
}
