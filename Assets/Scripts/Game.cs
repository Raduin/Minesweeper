using System.Collections.Generic;
using UnityEngine;

public enum ButtonImage { Joy, Sad, Wonder, Win, JoyPressed, Menu, MenuPressed, }

public class Game : MonoBehaviour
{
    public const int maxWidthBorder = 30;
    public const int maxHeightBorder = 16;
    
    public int width = 16;
    public int height = 16;
    public int mineCount = 32;
    public bool questionMark;
    public float scrollSpeed = 0.2f;
    public float catchTime = 0.4f;            //duration of time for mouse doubleclick method LeftDoubleClickMouseDetection()
    public float timeForFieldMoving = 0.2f;   //time of mouse immobility whereupon the field will not move automatically with damping

    public bool beginnerOn;                   //Flag if "Begginer" game type is active (for timescore) 
    public bool intermediateOn;               //Flag if "Intermediate" game type is active (for timescore)
    public bool expertOn;                     //Flag if "Expert" game type is active (for timescore)
    public GameObject winWindow;              //To enable/disable win window
    public GameObject winWindowShadow;        //To enable/disable win windowShadow

    //Score
    [HideInInspector] public string beginnerName;
    [HideInInspector] public int beginnerTime;
    [HideInInspector] public string intermediateName;
    [HideInInspector] public int intermediateTime;
    [HideInInspector] public string expertName;
    [HideInInspector] public int expertTime;

    private int timer;
    private int mineCounter;
    private Field field;
    private Hud hud;
    private Border border;
    private BackgroundField backgroundField;
    private Cell[,] state;
    private bool gameover;
    [SerializeField] private GameObject maskField;

    private float lastClickTime;              //first click time for mouse doubleclick method LeftDoubleClickMouseDetection()
    private int secondCounter;                //The counter of seconds from real time for method IncreaseTimer()
    private bool firstClick;                  //Flag for starting timer;
    private bool reset60;                     //Flag for resetting the counter of second for method IncreaseTimer()
    private Vector3Int firstCellLmb;          //Data of the cell that was clicked first before releasing with Left mouse button
    private Vector3Int firstCellLRmb;         //Data of the cell that was clicked first before releasing with Left+Right mouse button
    private Vector3 firstCellMmb;             //Data of the cell that was clicked first before releasing with Middle mouse button
    private bool currentCellLmb;              //Flag for matching with first clicked cell before releasing with Left mouse button
    private bool currentCellLRmb;             //Flag for matching with first clicked cell before releasing with Left+Right mouse button
    private bool currentCellMmb;              //Flag for matching with first clicked cell before releasing with Middle mouse button
    private bool mouseButtonsPressed;         //Flag for pressing both mouse buttons together
    private Vector3 deltafieldMoving;         //Start delta coordinates for field moving speed
    private bool fieldMoving;                 //Flag to start field moving
    private float timeForFieldMovingCounter;  //time of mouse immobility



    private void OnValidate()
    {
        mineCount = Mathf.Clamp(mineCount, 0, width * height);
    }

    private void Awake()
    {
        field = GetComponentInChildren<Field>();
        hud = GetComponentInChildren<Hud>();
        border = GetComponentInChildren<Border>();
        backgroundField = GetComponentInChildren<BackgroundField>();

        maskField = Instantiate(maskField);
        LoadScore();
    }

    //private void Start()
    //{

    //}

        public void NewGame()
    {
        field.FieldMap.ClearAllTiles();
        hud.HudMap.ClearAllTiles();
        border.BorderMap.ClearAllTiles();

        gameover = false;
        mineCounter = mineCount;
        timer = 0;
        state = new Cell[width, height];
        reset60 = true;
        firstClick = false;

        GenerateCells();
        GenerateMines();
        GenerateNumbers();

        SetMainCamera(width, height);

        Vector3Int backgroundFieldPosition = backgroundField.BackgroundFieldMap.WorldToCell(Camera.main.transform.position);
        backgroundField.DrawBackgroundField(backgroundFieldPosition);

        border.DrawBorder(width, height);
        hud.Clock(width, height, mineCount, timer);
        hud.DrawButtonUp(width, height, ButtonImage.Joy);
        hud.DrawButtonDown(width, height, ButtonImage.Menu);

        field.transform.position = transform.position;
        field.transform.localScale = transform.localScale;
        FieldInBorder(width, height);

        SetMaskField(width, height);

        field.Draw(state);
    }

        private void GenerateCells()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = new Cell();
                cell.position = new Vector3Int(x, y, 0);
                cell.type = Cell.Type.Empty;
                state[x, y] = cell;
            }
        }
    }

    private void GenerateMines()
    {
        for (int i = 0; i < mineCount; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            if (state[x, y].type == Cell.Type.Mine) {
                i--;
            }
            else
            {
                state[x, y].type = Cell.Type.Mine;
            }
        }

    }

    private void GenerateNumbers()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (state[x, y].type == Cell.Type.Mine) {
                    continue;
                }

                state[x, y].number = CountMines(x, y);

                if (state[x, y].number > 0) {
                    state[x, y].type = Cell.Type.Number;
                }
            }
        }
    }

    private int CountMines(int cellX, int cellY)
    {
        int count = 0;

        for (int adjacentX = -1; adjacentX <= 1; adjacentX++)
        {
            for (int adjacentY = -1; adjacentY <= 1; adjacentY++)
            {
                if (adjacentX == 0 && adjacentY == 0) {
                    continue;
                }

                int x = cellX + adjacentX;
                int y = cellY + adjacentY;

                if (x < 0 || x >= width || y < 0 || y >= height) {
                    continue;
                }
                if (state[x, y].type == Cell.Type.Mine) {
                    count++;
                }
            }
        }
        return count;
    }

    private void FixedUpdate()
    {
        //moving field with damping
        if (fieldMoving)
        {
            field.transform.Translate(deltafieldMoving * 5);
            deltafieldMoving = new Vector3(deltafieldMoving.x / 1.1f, deltafieldMoving.y / 1.1f, 0f);
            if (System.Math.Abs(deltafieldMoving.x) < 0.001 && System.Math.Abs(deltafieldMoving.y) < 0.001) fieldMoving = false;
            FieldInBorder(width, height);
        }
    }

    private void Update()
    {
        if (winWindow.activeInHierarchy) return;
        if (Input.GetKeyDown(KeyCode.R)) NewGame();
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            width = 8;
            height = 8;
            mineCount = 10;
            beginnerOn = true;
            intermediateOn = false;
            expertOn = false;
            NewGame();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            width = 16;
            height = 16;
            mineCount = 40;
            beginnerOn = false;
            intermediateOn = true;
            expertOn = false;
            NewGame();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            width = 30;
            height = 16;
            mineCount = 99;
            beginnerOn = false;
            intermediateOn = false;
            expertOn = true;
            NewGame();
        }
        //Field scaling
        if (Input.mouseScrollDelta.y != 0)
        {
            if ((Input.mouseScrollDelta.y > 0 && field.transform.localScale.x <= 4f && field.transform.localScale.y <= 4f)
                || (Input.mouseScrollDelta.y < 0 && field.transform.localScale.x > 1f && field.transform.localScale.y > 1f))
            {
                Vector3 cellPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                field.transform.localScale = new Vector3(field.transform.localScale.x + Input.mouseScrollDelta.y * scrollSpeed, field.transform.localScale.y + Input.mouseScrollDelta.y * scrollSpeed, 0f);

                float currentVectorX = field.transform.position.x - cellPosition.x;
                float currentVectorY = field.transform.position.y - cellPosition.y;
                float newVectorX;
                float newVectorY;

                if (Input.mouseScrollDelta.y > 0)
                {
                    newVectorX = currentVectorX / (field.transform.localScale.x - scrollSpeed) * field.transform.localScale.x;
                    newVectorY = currentVectorY / (field.transform.localScale.y - scrollSpeed) * field.transform.localScale.y;
                }
                else
                {
                    newVectorX = currentVectorX / (field.transform.localScale.x + scrollSpeed) * field.transform.localScale.x;
                    newVectorY = currentVectorY / (field.transform.localScale.y + scrollSpeed) * field.transform.localScale.y;
                }
                field.transform.position = new Vector3(field.transform.position.x + newVectorX - currentVectorX, field.transform.position.y + newVectorY - currentVectorY, 0f);

                FieldInBorder(width, height);
            }
        }

        //Field moving
        if (Input.GetMouseButton(2))
        {
            fieldMoving = false;
            Vector3 cellPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (cellPosition.x < 0 || cellPosition.x >= width || cellPosition.y < 0 || cellPosition.y >= height) return;
            if (!currentCellMmb)
            {
                firstCellMmb = cellPosition;
                currentCellMmb = true;
            }
            if (cellPosition != firstCellMmb)
            {
                field.transform.position = new Vector3((field.transform.position.x + cellPosition.x - firstCellMmb.x), (field.transform.position.y + cellPosition.y - firstCellMmb.y), 0f);
                FieldInBorder(width, height);
                deltafieldMoving = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0f);

                firstCellMmb = cellPosition;
                timeForFieldMovingCounter = 0f;
            }
            else timeForFieldMovingCounter += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(2)) 
        {
            if (timeForFieldMovingCounter < timeForFieldMoving) fieldMoving = true;
            currentCellMmb = false;
            timeForFieldMovingCounter = 0f;
        }

        if (!gameover)
        {
            if (Input.GetMouseButton(0))
                if (!mouseButtonsPressed)
                {
                    Vector3Int cellPosition = MousePositionCurrent();

                    if (!currentCellLmb)
                    {
                        firstCellLmb = cellPosition;
                        currentCellLmb = true;
                    }

                    if (cellPosition != firstCellLmb)
                    {
                        if (firstCellLmb.x >= 0 && firstCellLmb.x < width && firstCellLmb.y >= 0 && firstCellLmb.y < height)
                        {
                            if (!state[firstCellLmb.x, firstCellLmb.y].revealed && !state[firstCellLmb.x, firstCellLmb.y].flagged)
                            {
                                if (state[firstCellLmb.x, firstCellLmb.y].questionMark) field.DrawOneCellQuestionMark(firstCellLmb);
                                else field.DrawOneCellUnknown(firstCellLmb);
                            }
                            hud.DrawButtonUp(width, height, ButtonImage.Joy);
                        }
                        firstCellLmb = cellPosition;
                    }
                    if (cellPosition.x >= 0 && cellPosition.x < width && cellPosition.y >= 0 && cellPosition.y < height)
                        if (!state[cellPosition.x, cellPosition.y].revealed && !state[cellPosition.x, cellPosition.y].flagged)
                        {
                            if (!MouseOutField(width, height)) hud.DrawButtonUp(width, height, ButtonImage.Wonder);
                            if (state[cellPosition.x, cellPosition.y].questionMark) field.DrawOneCellQuestionMarkDown(cellPosition);
                            else field.DrawOneCellEmpty(cellPosition);
                        }
                }

            if (Input.GetMouseButtonDown(1))
            {
                if (!Input.GetMouseButton(0)) Flag();
                mouseButtonsPressed = false;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (firstCellLmb.x >= 0 && firstCellLmb.x < width && firstCellLmb.y >= 0 && firstCellLmb.y < height)
                {
                    if (!state[firstCellLmb.x, firstCellLmb.y].revealed && !state[firstCellLmb.x, firstCellLmb.y].flagged)
                    {
                        if (state[firstCellLmb.x, firstCellLmb.y].questionMark) field.DrawOneCellQuestionMark(firstCellLmb);
                        else field.DrawOneCellUnknown(firstCellLmb);
                    }
                }

                hud.DrawButtonUp(width, height, ButtonImage.Joy);
                if (!Input.GetMouseButton(1) && !mouseButtonsPressed) Reveal();
                currentCellLmb = false;
                currentCellLRmb = false;
                mouseButtonsPressed = false;
            }

            if ((Input.GetMouseButtonUp(0) & Input.GetMouseButton(1)) || (Input.GetMouseButton(0) & Input.GetMouseButtonUp(1)) || (Input.GetMouseButtonUp(0) && Input.GetMouseButtonUp(1)) || LeftDoubleClickMouseDetection())
            {
                if (firstCellLmb.x >= 0 && firstCellLmb.x < width && firstCellLmb.y >= 0 && firstCellLmb.y < height)
                {
                    if (!state[firstCellLmb.x, firstCellLmb.y].revealed && !state[firstCellLmb.x, firstCellLmb.y].flagged)
                    {
                        if (state[firstCellLmb.x, firstCellLmb.y].questionMark) field.DrawOneCellQuestionMark(firstCellLmb);
                        else field.DrawOneCellUnknown(firstCellLmb);
                    }
                }
                Vector3Int cellPosition = MousePositionCurrent();

                if (cellPosition.x >= 0 && cellPosition.x < width && cellPosition.y >= 0 && cellPosition.y < height)
                {
                    hud.DrawButtonUp(width, height, ButtonImage.Joy);
                    for (int adjacentX = -1; adjacentX <= 1; adjacentX++)
                    {
                        for (int adjacentY = -1; adjacentY <= 1; adjacentY++)
                        {
                            int x = firstCellLRmb.x + adjacentX;
                            int y = firstCellLRmb.y + adjacentY;

                            if (x < 0 || x >= width || y < 0 || y >= height)
                            {
                                continue;
                            }
                            if (!state[x, y].revealed && !state[x, y].flagged)
                            {
                                if (state[x, y].questionMark) field.DrawOneCellQuestionMark(new Vector3Int(x, y, 0));
                                else field.DrawOneCellUnknown(new Vector3Int(x, y, 0));
                            }
                        }
                    }
                    AutoReveal();
                    currentCellLmb = false;
                    currentCellLRmb = false;
                }
            }

            if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
            {
                Vector3Int cellPosition = MousePositionCurrent();
                mouseButtonsPressed = true;

                if (!currentCellLRmb)
                {
                    firstCellLRmb = cellPosition;
                    currentCellLRmb = true;
                }

                if (cellPosition != firstCellLRmb)
                {
                    if (firstCellLRmb.x >= 0 && firstCellLRmb.x < width && firstCellLRmb.y >= 0 && firstCellLRmb.y < height)
                    {
                        for (int adjacentX = -1; adjacentX <= 1; adjacentX++)
                        {
                            for (int adjacentY = -1; adjacentY <= 1; adjacentY++)
                            {
                                int x = firstCellLRmb.x + adjacentX;
                                int y = firstCellLRmb.y + adjacentY;

                                if (x < 0 || x >= width || y < 0 || y >= height)
                                {
                                    continue;
                                }
                                if (!state[x, y].revealed && !state[x, y].flagged)
                                {
                                    if (state[x, y].questionMark) field.DrawOneCellQuestionMark(new Vector3Int(x, y, 0));
                                    else field.DrawOneCellUnknown(new Vector3Int(x, y, 0));
                                 }
                            }
                        }
                        hud.DrawButtonUp(width, height, ButtonImage.Joy);
                    }
                    firstCellLRmb = cellPosition;
                }
                if (cellPosition.x >= 0 && cellPosition.x < width && cellPosition.y >= 0 && cellPosition.y < height)
                    for (int adjacentX = -1; adjacentX <= 1; adjacentX++)
                    {
                        for (int adjacentY = -1; adjacentY <= 1; adjacentY++)
                        {
                            int x = cellPosition.x + adjacentX;
                            int y = cellPosition.y + adjacentY;

                            if (x < 0 || x >= width || y < 0 || y >= height)
                            {
                                continue;
                            }
                            if (!state[x, y].revealed && !state[x, y].flagged)
                             {
                                if (!MouseOutField(width, height)) hud.DrawButtonUp(width, height, ButtonImage.Wonder);
                                if (state[x, y].questionMark) field.DrawOneCellQuestionMarkDown(new Vector3Int(x, y, 0));
                                else field.DrawOneCellEmpty(new Vector3Int(x, y, 0));
                            }
                        }
                    }
            }
        }
        
        if (firstClick == true ) timer += IncreaseTimer();
    }

    private void Flag()
    {
        if (MouseOutField(width, height)) return;
        Vector3Int cellPosition = MousePositionCurrent();

        if (cellPosition.x < 0 || cellPosition.x >= width || cellPosition.y < 0 || cellPosition.y >= height) {
            return;
        }
        if (state[cellPosition.x, cellPosition.y].revealed) {
            return;
        }

         if (state[cellPosition.x, cellPosition.y].flagged)
        {
            mineCounter += 1;
            hud.Clock(width, height, mineCounter, timer);
        }
        else
        {
            mineCounter = mineCounter -= 1;
            hud.Clock(width, height, mineCounter, timer);
        }
         if (questionMark)
        {
            if (state[cellPosition.x, cellPosition.y].flagged)
            {
                state[cellPosition.x, cellPosition.y].flagged = false;
                state[cellPosition.x, cellPosition.y].questionMark = true;
            }
            else if (state[cellPosition.x, cellPosition.y].questionMark)
                state[cellPosition.x, cellPosition.y].questionMark = false;
            else state[cellPosition.x, cellPosition.y].flagged = true;
        }
        else state[cellPosition.x, cellPosition.y].flagged = !state[cellPosition.x, cellPosition.y].flagged;
        field.DrawOneCell(state[cellPosition.x, cellPosition.y]);

        firstClick = true;
        secondCounter = System.DateTime.Now.Second;
    }

    private void Reveal()
    {
        if (MouseOutField(width, height)) return;
        Vector3Int cellPosition = MousePositionCurrent();

        if (cellPosition.x < 0 || cellPosition.x >= width || cellPosition.y < 0 || cellPosition.y >= height) {
            return;
        }

        if (state[cellPosition.x, cellPosition.y].revealed || state[cellPosition.x, cellPosition.y].flagged) {
            return;
        }

        state[cellPosition.x, cellPosition.y].revealed = true;

        switch (state[cellPosition.x, cellPosition.y].type)
        {
            case Cell.Type.Mine:
                state[cellPosition.x, cellPosition.y].exploded = true;
                Explode();
                break;

            case Cell.Type.Empty:
                Flood(cellPosition.x, cellPosition.y);

                break;

            case Cell.Type.Number:

                break;
        }

        field.DrawOneCell(state[cellPosition.x, cellPosition.y]);
        CheckWinCondition();
        firstClick = true;
        secondCounter = System.DateTime.Now.Second;
    }

    private void AutoReveal()
    {
        if (MouseOutField(width, height)) return;
        Vector3Int cellPosition = MousePositionCurrent();

        if (cellPosition.x < 0 || cellPosition.x >= width || cellPosition.y < 0 || cellPosition.y >= height) {
            return;
        }
        if (!state[cellPosition.x, cellPosition.y].revealed || state[cellPosition.x, cellPosition.y].type == Cell.Type.Empty) {
            return;
        }

        if (state[cellPosition.x, cellPosition.y].number == CountFlagsAround(cellPosition.x, cellPosition.y))
        {
            for (int adjacentX = -1; adjacentX <= 1; adjacentX++)
            {
                for (int adjacentY = -1; adjacentY <= 1; adjacentY++)
                {
                    if (adjacentX == 0 && adjacentY == 0)
                    {
                        continue;
                    }

                    int x = cellPosition.x + adjacentX;
                    int y = cellPosition.y + adjacentY;

                    if (x < 0 || x >= width || y < 0 || y >= height)
                    {
                        continue;
                    }
                    if (!state[x, y].revealed && !state[x, y].flagged)
                    {
                        switch (state[x, y].type)
                        {
                            case Cell.Type.Mine:
                                state[x, y].exploded = true;
                                Explode();
                                break;

                            case Cell.Type.Empty:
                                Flood(x, y);

                                break;

                            case Cell.Type.Number:

                                break;
                        }
                        state[x, y].revealed = true;
                        field.DrawOneCell(state[x, y]);
                        CheckWinCondition();
                        firstClick = true;
                        secondCounter = System.DateTime.Now.Second;
                    }
                }
            }
        }
    }

    private int CountFlagsAround(int cellX, int cellY)
    {
        int count = 0;

        for (int adjacentX = -1; adjacentX <= 1; adjacentX++)
        {
            for (int adjacentY = -1; adjacentY <= 1; adjacentY++)
            {
                if (adjacentX == 0 && adjacentY == 0) {
                    continue;
                }

                int x = cellX + adjacentX;
                int y = cellY + adjacentY;

                if (x < 0 || x >= width || y < 0 || y >= height) {
                    continue;
                }
                if (state[x, y].flagged) {
                    count++;
                }
            }
        }

        return count;
    }
    private void Explode()
    {
        gameover = true;
        hud.DrawButtonUp(width, height, ButtonImage.Sad);

        for (int x = 0; x < width; x++)
        {
            for (int y =0; y < height; y++)
            {
                if (state[x, y].flagged && state[x, y].type != Cell.Type.Mine)
                {
                    state[x, y].revealed = true;
                    state[x, y].mineWrong = true;
                    field.DrawOneCell(state[x, y]);
                }
                if (state[x,y].type == Cell.Type.Mine)
                {
                    state[x,y].revealed = true;
                    field.DrawOneCell(state[x,y]);
                }
            }
        }
    }

    private void Flood(int cellX, int cellY)
    {
        bool endFlood = false;
        Queue<int> emptyX = new (0);
        Queue<int> emptyY = new (0);

        while (!endFlood)
        {
            state[cellX, cellY].revealed = true;
            field.DrawOneCell(state[cellX, cellY]);

            for (int adjacentX = -1; adjacentX <= 1; adjacentX++)
            {
                for (int adjacentY = -1; adjacentY <= 1; adjacentY++)
                {
                    if (adjacentX == 0 && adjacentY == 0)
                    {
                        continue;
                    }
                    int x = cellX + adjacentX;
                    int y = cellY + adjacentY;
                    if (x < 0 || x >= width || y < 0 || y >= height)
                    {
                        continue;
                    }
                    if (state[x, y].type == Cell.Type.Mine || state[x, y].revealed)
                    {
                        continue;
                    }
                    if (state[x, y].type == Cell.Type.Number)
                    {
                        state[x, y].revealed = true;
                        field.DrawOneCell(state[x, y]);
                    }
                    if (state[x, y].type == Cell.Type.Empty)
                    {
                        state[x, y].revealed = true;
                        emptyX.Enqueue(x);
                        emptyY.Enqueue(y);
                    }
                }
            }

            if (emptyX.Count > 0) 
            {
                cellX = emptyX.Dequeue();
                cellY = emptyY.Dequeue();
            }
            else endFlood = true;
        }
    }

    private void CheckWinCondition()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!state[x,y].revealed && state[x,y].type != Cell.Type.Mine) {
                    return; // no win
                }
            }
        }

        gameover = true;
        hud.DrawButtonUp(width, height, ButtonImage.Win);
        hud.Clock(width, height, 0, timer);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (state[x, y].type == Cell.Type.Mine)
                {
                    state[x, y].flagged = true;
                    field.DrawOneCell(state[x, y]);
                }
            }
        }
        if (beginnerOn && timer < beginnerTime) 
        {
            beginnerTime = timer;
            winWindow.SetActive(true);
            winWindowShadow.SetActive(true);
        }
        if (intermediateOn && timer < intermediateTime) 
        {
            intermediateTime = timer;
            winWindow.SetActive(true);
            winWindowShadow.SetActive(true);
        }
        if (expertOn && timer < expertTime) 
        {
            expertTime = timer;
            winWindow.SetActive(true);
            winWindowShadow.SetActive(true);
        }
    }

    private bool LeftDoubleClickMouseDetection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastClickTime < catchTime) return true;
            lastClickTime = Time.time;
        }
        return false;
    }
     private int IncreaseTimer()
    {
        if (System.DateTime.Now.Second == 0 && reset60 == true && timer <= 999 && !gameover)
        {
            secondCounter = 0;
            hud.Clock(width, height, mineCounter, timer);
            reset60 = false;
            return 1;
        }
        if ((secondCounter + 1) == System.DateTime.Now.Second && timer <= 999 && !gameover)
        {
            secondCounter = System.DateTime.Now.Second;
            hud.Clock(width, height, mineCounter, timer);
            reset60 = true;
            return 1;
        }
        return 0;
    }

    private bool MouseOutField(int width, int height)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellBorder = border.BorderMap.WorldToCell(worldPosition);
        if (width > maxWidthBorder) width = maxWidthBorder;
        if (height > maxHeightBorder) height = maxHeightBorder;
        if (cellBorder.x < 0 || cellBorder.x >= width || cellBorder.y < 0 || cellBorder.y >= height) return true;
        else return false;
    }

    private Vector3Int MousePositionCurrent()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = field.FieldMap.WorldToCell(worldPosition);
        return cellPosition;
    }
    //Return field in the border
    private void FieldInBorder(int width, int height)
    {
        
        if (field.transform.position.x >= 0)
        {
            field.transform.position = new Vector3(0f, field.transform.position.y, 0f);
            border.FieldBorderMarkLeft(height, false);
        }
        else border.FieldBorderMarkLeft(height, true);

        if (field.transform.position.y >= 0)
        {
            field.transform.position = new Vector3(field.transform.position.x, 0f, 0f);
            border.FieldBorderMarkDown(width, false);
        }
        else border.FieldBorderMarkDown(width, true);

        if (field.transform.localScale.x == 1 && field.transform.localScale.y == 1)
        {
            if (width > maxWidthBorder)
                if (field.transform.position.x <= maxWidthBorder - width)
                {
                    field.transform.position = new Vector3(maxWidthBorder - width, field.transform.position.y, 0f);
                    border.FieldBorderMarkRight(width, height, false);
                }
                else border.FieldBorderMarkRight(width, height, true);
            else 
            {
                field.transform.position = new Vector3(transform.position.x, field.transform.position.y, 0f);
                border.FieldBorderMarkRight(width, height, false);
            }

            if (height > maxHeightBorder)
                if (field.transform.position.y <= maxHeightBorder - height)
                {
                    field.transform.position = new Vector3(field.transform.position.x, maxHeightBorder - height, 0f);
                    border.FieldBorderMarkUp(width, height, false);
                }
                else border.FieldBorderMarkUp(width, height, true);
            else 
            {
                border.FieldBorderMarkUp(width, height, false);
                field.transform.position = new Vector3(field.transform.position.x, transform.position.y, 0f); 
            }

            if (field.transform.position.x == 0) border.FieldBorderMarkLeft(height, false);
            if (field.transform.position.y == 0) border.FieldBorderMarkDown(width, false);
        }
        else
        {
            if (width > maxWidthBorder)
                if (field.transform.position.x <= -width * (field.transform.localScale.x) + maxWidthBorder)
                {
                    field.transform.position = new Vector3(-width * (field.transform.localScale.x) + maxWidthBorder, field.transform.position.y, 0f);
                    border.FieldBorderMarkRight(width, height, false);
                }
                else border.FieldBorderMarkRight(width, height, true);
            else
            {
                if (field.transform.position.x <= -width * (field.transform.localScale.x - 1))
                {
                    field.transform.position = new Vector3(-width * (field.transform.localScale.x - 1), field.transform.position.y, 0f);
                    border.FieldBorderMarkRight(width, height, false);
                }
                else border.FieldBorderMarkRight(width, height, true);
            }

            if (height > maxHeightBorder)
                if (field.transform.position.y <= -height * (field.transform.localScale.y) + maxHeightBorder)
                {
                    field.transform.position = new Vector3(field.transform.position.x, -height * (field.transform.localScale.y) + maxHeightBorder, 0f);
                    border.FieldBorderMarkUp(width, height, false);
                }
                else border.FieldBorderMarkUp(width, height, true);
            else
            {
                if (field.transform.position.y <= -height * (field.transform.localScale.y - 1))
                {
                    field.transform.position = new Vector3(field.transform.position.x, -height * (field.transform.localScale.y - 1), 0f);
                    border.FieldBorderMarkUp(width, height, false);
                }
                else border.FieldBorderMarkUp(width, height, true);
            }

            if (field.transform.position.x == 0) border.FieldBorderMarkLeft(height, false);
            if (field.transform.position.y == 0) border.FieldBorderMarkDown(width, false);
        }
    }

    private void SetMainCamera(int width, int height)
    {
        if (width > maxWidthBorder) width = maxWidthBorder;
        if (height > maxHeightBorder) height = maxHeightBorder;
        Camera.main.transform.position = new Vector3(width / 2f, height / 2f, -11f);
    }

    private void SetMaskField(int width, int height)
    {
        if (width > maxWidthBorder) width = maxWidthBorder;
        if (height > maxHeightBorder) height = maxHeightBorder;
        maskField.transform.position = new Vector3(width / 2f, height / 2f, 0f);
        maskField.transform.localScale = new Vector3(width, height, 0f);
    }

    public void LoadScore()
    {
        if (PlayerPrefs.HasKey("BeginnerName")) 
        { 
            beginnerName = PlayerPrefs.GetString("BeginnerName");
            beginnerTime = PlayerPrefs.GetInt("BeginnerTime");
        }
        else
        {
            beginnerName = "Anonymouse";
            beginnerTime = 999;
        }
        if (PlayerPrefs.HasKey("IntermediateName")) 
        {
            intermediateName = PlayerPrefs.GetString("IntermediateName");
            intermediateTime = PlayerPrefs.GetInt("IntermediateTime");
        }
        else
        {
            intermediateName = "Anonymouse";
            intermediateTime = 999;
        }
        if (PlayerPrefs.HasKey("ExpertName")) 
        {
            expertName = PlayerPrefs.GetString("ExpertName");
            expertTime = PlayerPrefs.GetInt("ExpertTime");
        }
        else
        {
            expertName = "Anonymouse";
            expertTime = 999;
        }
    }
}