using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс для улучшения зданий
/// </summary>
public class TowerUpdater : MonoBehaviour
{
    // Кнопки
    [SerializeField] private GameObject _lvlupButton;
    [SerializeField] private GameObject _cancelButton;

   
    [SerializeField] private TowerBase currentTower; // Текущее здание которое хотим улучшить
    public LayerMask layerMask;  // Слои на которые бьудет реагировать луч


    private void Update()
    {
        // При нажатии на клавишу мышик, при условии что кликаем по зданию появляются кнопки улучений
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100, layerMask))
            {
                if (hit.transform.gameObject.IsInLayer(layerMask))
                {
                    if (hit.transform.gameObject.GetComponent<TowerBase>().isBuild)
                    {
                        currentTower = hit.transform.gameObject.GetComponent<TowerBase>();
                        _lvlupButton.SetActive(true);
                        _cancelButton.SetActive(true);
                    }

                }
            }

        }

    }

    /// <summary>
    /// Улучшить текущего здания
    /// </summary>
    public void UpdateTower() {
        currentTower.UpTower();
        Reset();
    }

    /// <summary>
    /// Закнчить улучшения
    /// </summary>
    public void Reset()
    {
        currentTower = null;
        _lvlupButton.SetActive(false);
        _cancelButton.SetActive(false);
    }
}
