using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public enum MoveDirection
{
    Left,
    Right,
    Up,
    Down
}

public enum GAMESTATE
{
    Playing,
    waiting
}




public class GameManager_8 : MonoBehaviour
{


    //touchControls 
    float pressTime = 0.0f;
    Vector2 pressStartPos = Vector2.zero;
    bool isSwipe;
    float minSwipeDistance = 50f;
    float maxSwipeTime = 1.5f;



    //ScoreTimer

    public Text Score;
    Bettr_Encryption.Encrypt score;
    public Text Timer;
    Bettr_Encryption.Encrypt timer;


    //CellREFERENCE

    Cell_8[,] allCells = new Cell_8[4, 4];
    List<Cell_8> emptyCells = new List<Cell_8>();                 // to do encryption list 
    List<Cell_8[]> cellRows = new List<Cell_8[]>();
    List<Cell_8[]> cellCols = new List<Cell_8[]>();
    [Range(0, 2f)]



    public float delay;
    private bool movemade;
    private bool[] linemoving = new bool[4] { true, true, true, true };
    public GAMESTATE state;


    public GameObject gameover;    // gameover  enable

    // AUdio_Clips
    public AudioClip[] Event_Audio;




    void Start()
    {
        
        InitCell();
        InitCellRowAndCols();
        AddNewCell();
        score = new Bettr_Encryption.Encrypt(0);
        timer = new Bettr_Encryption.Encrypt(180);
    }

    void Update()
    {

        Score.text = "" + score;        //score to ScoreText

        if (!CanMove())

        {
            gameover.SetActive(true);
            Debug.Log("gameover");
        }




        //for Touchconntrols :-

        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        isSwipe = true;
                        pressTime = Time.time;
                        pressStartPos = touch.position;
                        break;
                    case TouchPhase.Canceled:
                        isSwipe = false;
                        break;
                    case TouchPhase.Ended:
                        float fingerTime = Time.time - pressTime;
                        float fingerDir = (touch.position - pressStartPos).magnitude;
                        if (isSwipe && fingerTime < maxSwipeTime && fingerDir > minSwipeDistance)
                        {
                            Vector2 dir = touch.position - pressStartPos;
                            Vector2 swipType = Vector2.zero;
                            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                            {
                                swipType = Vector2.right * Mathf.Sign(dir.x);
                            }
                            else
                                swipType = Vector2.up * Mathf.Sign(dir.y);
                            if (swipType.x != 0)
                            {
                                if (swipType.x > 0)
                                {
                                    Move(MoveDirection.Right);
                                }
                                else
                                    Move(MoveDirection.Left);
                            }
                            if (swipType.y != 0)
                            {
                                if (swipType.y > 0)
                                {
                                    Move(MoveDirection.Up);
                                }
                                else
                                    Move(MoveDirection.Down);
                            }
                        }
                        break;
                }
            }
        }

        //for Key Board Controls :-


        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(MoveDirection.Left);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(MoveDirection.Right);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(MoveDirection.Up);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(MoveDirection.Down);
        }

    }




    void InitCell()              //Cells Initilisation
    {
        Cell_8[] cellList = FindObjectsOfType<Cell_8>();
        foreach (Cell_8 c in cellList)
        {
            c.Number = 0;
            allCells[c.row, c.col] = c;
            emptyCells.Add(c);
        }
    }

    void InitCellRowAndCols()       //Storing Cell in rows and coloumn
    {
        cellCols.Add(new Cell_8[] { allCells[0, 0], allCells[1, 0], allCells[2, 0], allCells[3, 0] });
        cellCols.Add(new Cell_8[] { allCells[0, 1], allCells[1, 1], allCells[2, 1], allCells[3, 1] });
        cellCols.Add(new Cell_8[] { allCells[0, 2], allCells[1, 2], allCells[2, 2], allCells[3, 2] });
        cellCols.Add(new Cell_8[] { allCells[0, 3], allCells[1, 3], allCells[2, 3], allCells[3, 3] });

        cellRows.Add(new Cell_8[] { allCells[0, 0], allCells[0, 1], allCells[0, 2], allCells[0, 3] });
        cellRows.Add(new Cell_8[] { allCells[1, 0], allCells[1, 1], allCells[1, 2], allCells[1, 3] });
        cellRows.Add(new Cell_8[] { allCells[2, 0], allCells[2, 1], allCells[2, 2], allCells[2, 3] });
        cellRows.Add(new Cell_8[] { allCells[3, 0], allCells[3, 1], allCells[3, 2], allCells[3, 3] });
    }

    void AddNewCell()                      //Adding new cell
    {
        if (emptyCells.Count > 0)
        {
            int index = Random.Range(0, emptyCells.Count);
            int random = Random.Range(0, 10);
            if (emptyCells[index].Number == 0)
            {

                if (random == 0)
                    emptyCells[index].Number = 4;
                else
                    emptyCells[index].Number = 2;
             

            }

           

            emptyCells[index].AnimationAppear();

            emptyCells.RemoveAt(index);
        }
    }

    void ClearEmptyCellList()                    //clearing empty cells 
    {
        emptyCells.Clear();
        foreach (Cell_8 c in allCells)
        {
            if (c.Number == 0)
            {
                emptyCells.Add(c);
            }
        }
    }


    bool DownMove(Cell_8[] lines)                         //movingDown 
    {
        for (int i = 0; i < lines.Length - 1; i++)
        {

         



          //Merging 
            if (lines[i].Number != 0 && lines[i].Number == lines[i + 1].Number &&
                !lines[i].addNumber && !lines[i + 1].addNumber)
            {
                lines[i].Number = lines[i].Number * 2;

                //audio events
                switch (lines[i].Number)
                {
                    case 16384:
                        Callback(4);
                        break;

                    case 8192:
                        Callback(4);
                        break;
                    case 4096:
                        Callback(4);
                        break;
                    case 2048:
                        Callback(4);
                        break;
                    case 1024:
                        Callback(4);
                        break;
                    case 512:
                        Callback(3);
                        break;
                    case 256:
                        Callback(3);
                        break;
                    case 128:
                        Callback(3);
                        break;
                    case 64:
                        Callback(3);
                        break;
                    case 32:
                        Callback(2);
                        break;
                    case 16:
                        Callback(2);
                        break;

                    case 8:
                        Callback(2);
                        break;

                    case 4:
                        Callback(2);
                        break;


                }
                lines[i + 1].Number = 0;
                lines[i].addNumber = true;
                lines[i].AnimationMerge();


                emptyCells.Add(lines[i + 1]);
                score += new Bettr_Encryption.Encrypt(lines[i].Number);               //Score Updater
                return true;




            }
            if (lines[i].Number == 0 && lines[i + 1].Number != 0)
            {







                

                lines[i].Number = lines[i + 1].Number;
               

                lines[i + 1].Number = 0;

                return true;

            }
        }
        return false;
    }



    private bool Audio_Calls = false;
    void Callback(int id)
    {
        Debug.Log(id);
        if (Audio_Calls == false)
        {
            Audio_Calls = true;
            StartCoroutine(Audio_Handler(id));
        }

    }
    IEnumerator Audio_Handler(int id){
        yield return new WaitForSeconds(0.04f);
        GameObject.FindWithTag("TAG_1").transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(Event_Audio[id - 1]);
        Audio_Calls = false;
    }

    bool UpMove(Cell_8[] lines)                  // moving up
    {
        for (int i = lines.Length - 1; i > 0; i--)
        {
            if (lines[i].Number == 0 && lines[i - 1].Number != 0)
            {

                lines[i].Number = lines[i - 1].Number;
                lines[i - 1].Number = 0;
                
                return true;
            }
            //merging condition
            if (lines[i].Number != 0 && lines[i].Number == lines[i - 1].Number &&
                !lines[i].addNumber && !lines[i - 1].addNumber)
            {

                lines[i].Number = lines[i].Number * 2;

                //audioevents
                switch(lines[i].Number)
                {
                    case 16384:
                        Callback(4);
                        break;
     
                    case 8192:
                        Callback(4);
                        break;
                    case 4096:
                        Callback(4);
                        break;
                    case 2048:
                        Callback(4);
                        break;
                    case 1024:
                        Callback(4);
                        break;
                    case 512:
                        Callback(3);
                        break;
                    case 256:
                        Callback(3);
                        break;
                    case 128:
                        Callback(3);
                        break;
                    case 64:
                        Callback(3);
                        break;
                    case 32:
                        Callback(2);
                        break;
                    case 16 :
                        Callback(2);
                        break;

                    case 8:
                        Callback(2);
                        break;

                    case 4:
                        Callback(2);
                        break;


                }

                lines[i - 1].Number = 0;
                lines[i].addNumber = true;
                lines[i].AnimationMerge();
                emptyCells.Add(lines[i - 1]);
                score += new Bettr_Encryption.Encrypt(lines[i].Number);              //Score Updater
                return true;
            }
            if (lines[i].Number == 0 && lines[i - 1].Number != 0)
            {




                lines[i].Number = lines[i - 1].Number;
                lines[i - 1].Number = 0;
              
                return true;
            }
        }
        return false;
    }


    //gameover Condition
    bool CanMove()
    {
        if (emptyCells.Count > 0)
            return true;
        else
        {
            for (int i = 0; i < cellCols.Count; i++)
            {
                for (int j = 0; j < cellRows.Count - 1; j++)
                {
                    if (allCells[j, i].Number == allCells[j + 1, i].Number)
                        return true;
                }
            }
            for (int i = 0; i < cellRows.Count; i++)
            {
                for (int j = 0; j < cellCols.Count - 1; j++)
                {
                    if (allCells[i, j].Number == allCells[i, j + 1].Number)
                        return true;
                }
            }
        }
        return false;
    }



    //Moving Cells innto MoveDirection
    public void Move(MoveDirection move)
    {

        foreach (Cell_8 c in allCells)
        {
            c.ResetAddNumberFlag();
        }

        movemade = false;
        if (delay > 0)
        {
            StartCoroutine(MoveCoroutine(move));

        }
        else
        {


            for (int i = 0; i < cellRows.Count; i++)
            {
                switch (move)
                {
                    case MoveDirection.Down:
                        while (UpMove(cellCols[i]))
                        {

                            movemade = true;

                        }
                        break;
                    case MoveDirection.Left:
                        while (DownMove(cellRows[i]))
                        {

                            movemade = true;
                        }
                        break;
                    case MoveDirection.Right:
                        while (UpMove(cellRows[i]))
                        {

                            movemade = true;
                        }
                        break;
                    case MoveDirection.Up:
                        while (DownMove(cellCols[i]))
                        {

                            movemade = true;

                        }
                        break;
                }
            }
            ClearEmptyCellList();
            AddNewCell();

        }

    

    }

    //delaying effect of movement 
    IEnumerator MoveCoroutine(MoveDirection move)
    {
        state = GAMESTATE.waiting;


        switch (move)
        {
            case MoveDirection.Down:
                    for (int i = 0; i < cellCols.Count; i++)
                    StartCoroutine(UpMoveCoroutine(cellCols[i],i));
                        break;
           
            case MoveDirection.Left:


                    for (int i = 0; i < cellRows.Count; i++)
                    StartCoroutine(DownMoveCoroutine(cellRows[i],i));
                        break;

            case MoveDirection.Right:

                    for (int i = 0; i < cellRows.Count; i++)
                    StartCoroutine(UpMoveCoroutine(cellRows[i],i));
                        break;

            case MoveDirection.Up:

                    for (int i = 0; i < cellCols.Count; i++)
                    StartCoroutine(DownMoveCoroutine(cellCols[i],i));
                        break;
          }


        // Wait until move is completed in all lines

        while (!(linemoving[0] && linemoving[1] && linemoving[2] && linemoving[3]))
        {
            yield return null;


        }
        if (movemade)
        {
            ClearEmptyCellList();
            AddNewCell();

        }
        
         state = GAMESTATE.Playing;
        StopAllCoroutines();
    
    }


    IEnumerator UpMoveCoroutine(Cell_8[] cells,int index)
    {
        linemoving[index] = false;

        while(UpMove(cells))
        {
            movemade = true;
            yield return new WaitForSeconds(delay);
        }
        linemoving[index] = true;

    }



    IEnumerator DownMoveCoroutine(Cell_8[] cells,int index)
    {
        linemoving[index] = false;
        while(DownMove(cells))
        {
            movemade = true;
            yield return new WaitForSeconds(delay);
        }
        linemoving[index] = true;

    }


   

    







}