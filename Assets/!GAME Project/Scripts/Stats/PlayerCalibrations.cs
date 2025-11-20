using UnityEngine;
using UnityEngine.UI;

public class PlayerCalibrations : MonoBehaviour
{
    [SerializeField] private PlayerSetUp playerSetUp;

    [SerializeField] private Dropdown bentInDrop;
    [SerializeField] private Dropdown bentOutDrop;
    [SerializeField] private Dropdown rotationUpDrop;
    [SerializeField] private Dropdown rotationDownDrop;


    public void Start()
    {
        Debug.Log(bentInDrop.value);
    }




    public void SetPlayerSetUp()
    {

    }


    public void UpdateBentROMIn()
    {

       
         switch (bentInDrop.value)
         {
             case 0:
                    playerSetUp.bentROMIn = 20f;
                    break;
             case 1:
                    playerSetUp.bentROMIn = 25f;
                    break;
             case 2:
                    playerSetUp.bentROMIn = 35f;
                    break;
             case 3:
                    playerSetUp.bentROMIn = 45f;
                    break;
        }
    }

}
