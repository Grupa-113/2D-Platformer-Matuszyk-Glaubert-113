using System;
using UnityEngine;

public class GeneratedPlatforms : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;
    const int PLATFORMS_NUM = 10;

    GameObject[] platforms;
    Vector3[] positions;

    [SerializeField] private float radius = 5.0f;
    private float currentAngle = 0;
    [SerializeField] private float speed = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        platforms = new GameObject[PLATFORMS_NUM];
        positions = new Vector3[PLATFORMS_NUM];

        float angle, x, y;

        for (int i = 0; i < platforms.Length; i++)
        {
            angle = 2 * Mathf.PI * i / PLATFORMS_NUM;

            x = radius * Mathf.Cos(angle);
            y = radius * Mathf.Sin(angle);

            positions[i] = transform.position + new Vector3(x, y, 0);
            platforms[i] = Instantiate(platformPrefab, positions[i], Quaternion.identity);
            platforms[i].transform.SetParent(transform);
        }


    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentAngle += speed * Time.deltaTime;

        float angle, x, y;

        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            angle = currentAngle + (2 * Mathf.PI * i / PLATFORMS_NUM);

            x = radius * Mathf.Cos(angle);
            y = radius * Mathf.Sin(angle);

            positions[i] = transform.position + new Vector3(x, y, 0);
            platforms[i].transform.position = positions[i];
        }
    }
}
