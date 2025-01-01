using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube : MonoBehaviour
{
    public Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
    }

    public void MoveToOriginalPosition(float speed)
    {
        StartCoroutine(MoveToPosition(originalPosition, speed));
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition, float speed)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition; // Устанавливаем точную позицию в конце
    }
}
