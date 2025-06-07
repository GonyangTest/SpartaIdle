using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    UIManager _uiManager;
    private void Start()
    {
        _uiManager = UIManager.Instance;
        GameStart();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (_uiManager.IsUIActive(UIType.Shop))
            {
                _uiManager.CloseUI(UIType.Shop);
            }
            else
            {
                _uiManager.OpenUI(UIType.Shop);
            }
        }
    }

    void GameStart()
    {
        _uiManager.OpenUI(UIType.Main);
    }

    void GameOver()
    {
    }
}
