using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTower : MonoBehaviour
{
    [SerializeField] private GameObject _сancelButton; // Кнопка выхода

    [SerializeField] private TowerBase _towerCatapult; // Префаб катапульты обычной
    [SerializeField] private TowerBase _towerFrozzenCatapult; // Префаб катапульты с заморозкой

    [SerializeField] private TowerBase _currentTower; // Текущее здание

    [SerializeField] private Camera _camera;

    // Для подстветки можно или нельзя установит здание
    [SerializeField] private LayerMask _layersSuccess; // Поверхности с которыми взаомодейстуем
    [SerializeField] private LayerMask _layersFailed; // Поверхности на которых можем установить
    
    // Поверхзность на которй можно установит или нельзя
    [SerializeField] private Material _SuccessBuild;
    [SerializeField] private Material _FailedBuild;

    // При клике на кнопку строительства показывает здание
    public void ShowTower(TowerBase tower) {
        _сancelButton.SetActive(true);
        if (_currentTower) Destroy(_currentTower.gameObject);
        _currentTower = Instantiate(tower);
    }

    // Установка обычной катапульты
    public void ShowTowerCatapult() {
        ShowTower(_towerCatapult);
    }

    // Установка катапульты с заморозкой
    public void ShowTowerFrozzenCatapult() {
        ShowTower(_towerFrozzenCatapult);
    }

    // Закончить строительство
    public void Cancel() {
        _сancelButton.SetActive(false);
        Destroy(_currentTower.gameObject);
        _currentTower = null;
    }

    // Установить здание
    public void Build() {
        if (_currentTower.price > GameMannager.Instance.gameController.money)
        {
            GameMannager.Instance.gameController.messageShower.ShowMessage("Не хватает денег");
            return;
        }

        GameMannager.Instance.gameController.ChangeMoney(-_currentTower.price);

        _currentTower.Build();
        _currentTower.ResetMaterial();

        _currentTower = null;
        _сancelButton.SetActive(false);
    }


    private void Update()
    {
        // Луч для проверки можно ли установить здание
        if (_currentTower)
        {
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100, _layersSuccess))
            {
                _currentTower.transform.position = hit.transform.position + new Vector3(0,1,0);

                if (hit.transform.gameObject.IsInLayer(_layersFailed))
                {
                    _currentTower.SetMaterial(_FailedBuild);
                }
                else {
                    _currentTower.SetMaterial(_SuccessBuild);
                    if (Input.GetMouseButtonDown(0))
                    {
                        Build();
                    }
                }
            }
        }
    }


}
