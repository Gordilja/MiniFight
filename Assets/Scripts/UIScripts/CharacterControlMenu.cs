using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControlMenu : MonoBehaviour
{
    public List<GameObject> characterList;
    public GameObject selectedCharacter;

    private bool characterClicked = false;

    void Update()
    {
        bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out raycastHit, 100f, LayerMask.GetMask("Player")) && !characterClicked)
                characterClicked = true;

        }
        else if (Input.GetMouseButtonUp(0) && characterClicked) 
            characterClicked = false;

        if (!isOverUI && Input.GetMouseButton(0) && characterClicked)
            RotateChar();

        if (Input.GetKeyDown("1"))
            SwitchActiveChar(0);
        

        if (Input.GetKeyDown("2"))
            SwitchActiveChar(1);
    }

    private void RotateChar() 
    {
        var Yaxys = selectedCharacter.transform.rotation.y - Input.mousePosition.x;

        var rotate = Quaternion.Euler(0, Yaxys, 0);
        selectedCharacter.transform.rotation = rotate;
    }


    private void SwitchActiveChar(int id) 
    {
        ResetCharRotation();
        selectedCharacter.SetActive(false);
        selectedCharacter = characterList[id];
        selectedCharacter.SetActive(true);
    }

    private void ResetCharRotation()
    {
        selectedCharacter.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}
