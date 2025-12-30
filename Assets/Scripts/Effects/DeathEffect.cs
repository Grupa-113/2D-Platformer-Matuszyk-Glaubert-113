using UnityEngine;

public class DeathEffect : MonoBehaviour
{

    [SerializeField] private float time = 2.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject,time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
