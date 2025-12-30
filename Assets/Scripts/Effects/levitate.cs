using UnityEngine;

public class SmoothLevitate : MonoBehaviour
{
    [Range(0.01f, 20.0f)][SerializeField] protected float moveSpeed = 2.0f;
    [SerializeField] protected float moveRange = 1.0f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * moveSpeed) * moveRange;

        transform.position = new Vector3(startPos.x, startPos.y + newY, startPos.z);
    }
}