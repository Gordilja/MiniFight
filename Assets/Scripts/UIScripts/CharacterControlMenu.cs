using System.Collections.Generic;
using UnityEngine;

public class CharacterControlMenu : MonoBehaviour
{
    public static CharacterControlMenu Instance;
    public List<GameObject> characterList;
    public GameObject selectedCharacter;
    private Animator animator;

    private bool characterClicked = false;
    private bool loadingStart = false;

    private void Awake()
    {
        Instance = this;
        // Change from game data
        selectedCharacter = characterList[0];
        animator = selectedCharacter.GetComponent<Animator>();
        animator.Play("Idle");
    }

    void Update()
    {
        if (!GameManager.Instance.StartGame)
        {
            MenuControls();
        }
        else if (GameManager.Instance.StartGame && !loadingStart)
        {
            animator.Play("Run");
            selectedCharacter.transform.rotation = Quaternion.Euler(0, 90, 0);
            loadingStart = true;
        }
    }

    private void MenuControls() 
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
        animator = selectedCharacter.GetComponent<Animator>();
    }

    private void ResetCharRotation()
    {
        selectedCharacter.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}
