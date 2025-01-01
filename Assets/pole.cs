using UnityEngine;

public class pole : MonoBehaviour
{
    public bool isEmpty = true; // Статус пустоты клетки
    public GameObject componentToActivate; // Компонент, который нужно активировать

    void Start()
    {
        // Изначально деактивируем компонент
        if (componentToActivate != null)
        {
            componentToActivate.SetActive(false);
        }
    }

    void Update()
    {
    }

    public void ActivateComponent()
    {
        // Активируем компонент
        if (componentToActivate != null)
        {
            componentToActivate.SetActive(true);
        }
    }
}
