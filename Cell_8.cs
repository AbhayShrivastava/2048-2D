using UnityEngine;
using UnityEngine.UI;

public class Cell_8 : MonoBehaviour
{

    [HideInInspector]
    public bool addNumber = false;
   
    public int row, col;
    int number;
    Text cellText;
    Image BgImage;
    

   
   
    private Animator anim;


    public int Number
    {
        get
        {
            return number;
        }
        set
        {
            number = value;
            if (number== 0)
            {
                SetCellHide();
            }
            else
            {
               ChangeCellStyle(number);

                SetCellVisual();
            }
        }
    }

    private void Awake()
    {
        BgImage = transform.Find("Panel").GetComponent<Image>();
        cellText = GetComponentInChildren<Text>();
        anim = GetComponent<Animator>();
        

    }
  
    public void AnimationMerge(){

        anim.SetTrigger("Merge");
    }

    public void AnimationAppear()
    {
        anim.SetTrigger("appear");
    }



    void GetCellStyle(int index)
    {
        int i = index % 12;
       
        BgImage.color = CellStyleHolder_8.instance.cellStyle[i].cellColor;
        cellText.color = CellStyleHolder_8.instance.cellStyle[i].textColor;
        cellText.text = CellStyleHolder_8.instance.cellStyle[i].number.ToString();
        cellText.fontSize = 140 - i / 12 * 20;
    }

    void ChangeCellStyle(int index)
    {
        if (index > 32768)
            index = index / 32678;
        switch (index)
        {
            case 2:
                GetCellStyle(0);
                break;
            case 4:
                GetCellStyle(1);
                break;
            case 8:
                GetCellStyle(2);
                break;
            case 16:
                GetCellStyle(3);
                break;
            case 32:
                GetCellStyle(4);
                break;
            case 64:
                GetCellStyle(5);
                break;
            case 128:
                GetCellStyle(6);
                break;
            case 256:
                GetCellStyle(7);
                break;
            case 512:
                GetCellStyle(8);
                break;
            case 1024:
                GetCellStyle(9);
                break;
            case 2048:
                GetCellStyle(10);
                break;
            case 4096:
                GetCellStyle(11);
                break;
            
            case 8192 :
                GetCellStyle(12);
                break;

            case 16384 :
                GetCellStyle(13);
                break;
           default:
                Debug.LogError("invalid number!");
                break;
       
        
        
        }
    
    
    
    }



    void SetCellVisual()
    {
        cellText.enabled = true;
        BgImage.enabled = true;
    }

    void SetCellHide()
    {
        cellText.enabled = false;
        BgImage.enabled = false;
    }



    public void ResetAddNumberFlag()
    {
        addNumber = false;
    }



}
