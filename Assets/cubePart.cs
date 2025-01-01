using UnityEngine;

public class cubePart : MonoBehaviour
{
    public bool hitEmptyCell = false; // Булева переменная, чтобы отметить попадание в пустую клетку
    public float laserDistance = 10f; // Дистанция лазера
    public LayerMask targetLayerMask; // Слой для цели

    void Update()
    {
        // Стрельба лазером назад каждый кадр
        ShootLaserBackwards();
    }

    void ShootLaserBackwards()
    {
        Vector3 direction = transform.forward; // Направление назад
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, laserDistance, targetLayerMask))
        {
            GameObject hitObject = hit.collider.gameObject;
            Debug.Log(hitObject.tag);
            pole poleScript = hitObject.GetComponent<pole>();
            if (poleScript != null)
            {
                // Активируем компонент на объекте клетки
                poleScript.ActivateComponent();

                // Если клетка имеет статус пустая, отметим это
                if (poleScript.isEmpty)
                {
                    hitEmptyCell = true;
                }
            }
        }

        // Визуализация лазера с помощью Gizmos (для наглядности)
        Debug.DrawRay(transform.position, direction * laserDistance, Color.red);
    }
}
