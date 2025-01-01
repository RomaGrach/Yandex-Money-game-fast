using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask targetLayerMask;
    public string targetTag;
    public string cubePartTag = "кубок парт"; // Тег для объектов, которые требуют обращения к родителю
    public Vector3 newPosition; // Новая позиция по оси Z
    public float moveSpeed = 5f; // Скорость перемещения
    public float returnSpeed = 5f; // Скорость возврата
    private GameObject targetedObject;
    private Vector3 originalPosition;
    private bool isDragging;
    private bool isMovingToZ;

    private Vector3 laserStartPosition;
    private Vector3 laserEndPosition;
    private bool laserShot;

    void Update()
    {
        // Стрельба лазером при нажатии кнопки мыши
        if (Input.GetMouseButtonDown(0))
        {
            ShootLaser();
        }

        // Постепенное перемещение объекта по оси Z
        if (isMovingToZ && targetedObject != null)
        {
            float step = moveSpeed * Time.deltaTime; // Скорость перемещения
            Vector3 targetPosition = new Vector3(targetedObject.transform.position.x, targetedObject.transform.position.y, newPosition.z);
            targetedObject.transform.position = Vector3.MoveTowards(targetedObject.transform.position, targetPosition, step);

            // Проверка, достиг ли объект целевой позиции по оси Z
            if (targetedObject.transform.position.z == newPosition.z)
            {
                isMovingToZ = false;
            }
        }

        // Перемещение объекта при перемещении курсора мыши
        if (isDragging && targetedObject != null)
        {
            MoveObjectWithCursor();
        }

        // Освобождение объекта при отпускании кнопки мыши
        if (Input.GetMouseButtonUp(0))
        {
            ReleaseObject();
        }
    }

    void ShootLaser()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        laserStartPosition = transform.position;
        laserEndPosition = ray.GetPoint(100);
        laserShot = true;

        if (Physics.Raycast(ray, out hit, 100, targetLayerMask))
        {
            laserEndPosition = hit.point; // Конечная позиция лазера при попадании
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag(targetTag) || hitObject.CompareTag(cubePartTag))
            {
                targetedObject = hitObject.CompareTag(cubePartTag) ? hitObject.transform.parent.gameObject : hitObject;

                cube cubeScript = targetedObject.GetComponent<cube>();
                if (cubeScript != null)
                {
                    originalPosition = cubeScript.originalPosition;
                }
                else
                {
                    originalPosition = targetedObject.transform.position;
                }

                isMovingToZ = true; // Начинаем постепенное перемещение по оси Z
                isDragging = true;  // Начинаем перемещение курсором
            }
        }
    }

    void MoveObjectWithCursor()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, targetedObject.transform.position.z));
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 point = ray.GetPoint(distance);
            targetedObject.transform.position = new Vector3(point.x, point.y, targetedObject.transform.position.z);
        }
    }

    void ReleaseObject()
    {
        isDragging = false;
        isMovingToZ = false; // Останавливаем любое движение по оси Z

        if (targetedObject != null)
        {
            cube cubeScript = targetedObject.GetComponent<cube>();
            if (cubeScript != null)
            {
                cubeScript.MoveToOriginalPosition(returnSpeed);
            }
            else
            {
                StartCoroutine(MoveToOriginalPosition());
            }
        }
    }

    IEnumerator MoveToOriginalPosition()
    {
        float step = returnSpeed * Time.deltaTime; // Скорость перемещения
        while (Vector3.Distance(targetedObject.transform.position, originalPosition) > 0.01f)
        {
            targetedObject.transform.position = Vector3.MoveTowards(targetedObject.transform.position, originalPosition, step);
            yield return null;
        }
        targetedObject.transform.position = originalPosition; // Устанавливаем точную позицию в конце
    }

    // Открытая функция для установки объекта в открытое положение
    public void OpenObject()
    {
        // В этом примере функция не делает ничего особенного, 
        // но вы можете добавить любую логику здесь, которая необходима для вашего сценария
    }

    void OnDrawGizmos()
    {
        if (laserShot)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(laserStartPosition, laserEndPosition);
            laserShot = false; // Сбрасываем флаг после визуализации
        }
    }
}
