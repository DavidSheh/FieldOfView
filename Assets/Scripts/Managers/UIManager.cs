using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : Singleton<UIManager> 
{
    public bool inEditMode { get; private set; }

    private Button _btnEdit, _btnFinishPoly, _btnReset;
    private Text _txtEdit, _txtFinishPoly, _txtReset;
    private PlayerController _player;


    public void Awake()
    {
        _btnEdit = GameObject.Find("btnEdit").GetComponent<Button>();
        _btnFinishPoly = GameObject.Find("btnFinishPoly").GetComponent<Button>();
        _btnReset = GameObject.Find("btnReset").GetComponent<Button>();

        _txtEdit = GameObject.Find("txtEdit").GetComponent<Text>();
        _txtFinishPoly = GameObject.Find("txtFinishPoly").GetComponent<Text>();
        _txtReset = GameObject.Find("txtReset").GetComponent<Text>(); 

        _player = GameObject.Find("Player").GetComponent<PlayerController>();

        inEditMode = false;
        _SetEditMode();
    }


    public void btnEditClick()
    {
        inEditMode = !inEditMode;

        _SetEditMode();
    }


    public void btnFinishPolyClick()
    {
        _player.EndMesh();
    }


    public void btnResetClick()
    {
        _player.ResetAll();
        inEditMode = !inEditMode;
        _SetEditMode();
    }


    private void _SetEditMode()
    {
        if (inEditMode)
        {
            _player.StartMesh();
        }
        else
        {
            _player.EndMesh();
        }

        _txtEdit.text = (inEditMode ? "Finish editing" : "Edit");
        _btnFinishPoly.gameObject.SetActive(inEditMode);
        _btnReset.gameObject.SetActive(inEditMode);        
    }
}
