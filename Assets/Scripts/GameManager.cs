using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Canvas _menuCanvas = null;

    public PlayerMoveInput _playerMoveInput;

    public UiInventory _UIInventory;

    
    public void OnJump()
    {
        _playerMoveInput.OnJump();
    }
    
    public void OnPause()
    {
        Debug.Log("pause");

        if(_menuCanvas.enabled)
        {
            resume();
        }
        else
        {
            pause();
        }
        
    }

    private void pause()
    {
        Time.timeScale = 0;
        _UIInventory.UpdateInventoryView();
        _menuCanvas.enabled = true;
    }

    private void resume()
    {
        Time.timeScale = 1;
        _menuCanvas.enabled = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
