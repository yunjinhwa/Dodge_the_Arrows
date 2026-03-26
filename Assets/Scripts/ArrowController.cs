using UnityEngine;

public class ArrowController : FallingObjectBase
{
    public enum ArrowMoveType
    {
        Straight,
        Wave
    }

    [Header("Damage")]
    [SerializeField] private float damageAmount = 0.1f;

    [Header("Move Type")]
    [SerializeField] private ArrowMoveType moveType = ArrowMoveType.Straight;

    [Header("Wave Movement")]
    [SerializeField] private float swayAmplitude = 1.2f;   // 좌우 흔들리는 폭
    [SerializeField] private float swayFrequency = 2.5f;   // 흔들리는 속도
    [SerializeField] private bool randomPhaseOnSpawn = true;

    private float baseX;
    private float phaseOffset;
    private float elapsed;

    protected override void Awake()
    {
        base.Awake();
        baseX = transform.position.x;

        if (randomPhaseOnSpawn)
            phaseOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    protected override void Update()
    {
        if (GameStateManager.Instance.CurrentState != GameState.IsPlaying)
            return;

        if (moveType == ArrowMoveType.Wave)
        {
            elapsed += Time.deltaTime;

            float offsetX = Mathf.Sin(elapsed * swayFrequency + phaseOffset) * swayAmplitude;
            Vector3 pos = transform.position;
            pos.x = baseX + offsetX;
            transform.position = pos;
        }

        // Straight면 x를 건드리지 않고 아래로만 이동
        base.Update();
    }

    protected override void OnHitPlayer()
    {
        if (Director == null)
            return;

        Director.PauseScore();
        Director.DamagePlayer(damageAmount);
    }
}