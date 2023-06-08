using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] blockObjects;
    public float transitionDuration;
    private Block[] blocks;
    private int hoverBlock;
    private int selectedBlock;
    private float moveTime;
    private float moveEndTime;
    private bool active;
    private bool levelComplete;

    // Start is called before the first frame update
    void Start()
    {
        blocks = new Block[blockObjects.Length];
        for (int i = 0; i < blockObjects.Length; i++) blocks[i] = blockObjects[i].GetComponent<Block>();
        hoverBlock = -1;
        selectedBlock = -1;
        moveTime = 0;
        moveEndTime = transitionDuration;
        active = true;
        levelComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (moveTime > 0)
        {
            moveTime -= Time.deltaTime;
            if (moveTime <= 0)
            {
                List<Block> ignited = new List<Block>();
                foreach (Block b in blocks) if (b.GetIgnited()) ignited.Add(b);
                if (ignited.Count > 0)
                {
                    List<Block> newIgnited = new List<Block>();
                    foreach (Block b in ignited)
                    {
                        b.SetSelectable(true);
                        bool c1 = b.GetCondition();
                        int x1 = c1 ? b.alternateX : b.defaultX;
                        int y1 = c1 ? b.alternateY : b.defaultY;
                        foreach (Block o in blocks)
                        {
                            if (!o.GetIgnited())
                            {
                                bool c2 = o.GetCondition();
                                int x2 = c2 ? o.alternateX : o.defaultX;
                                int y2 = c2 ? o.alternateY : o.defaultY;
                                bool selectable = x1 == x2 && Mathf.Abs(y1 - y2) == 1 || Mathf.Abs(x1 - x2) == 1 && y1 == y2;
                                if (selectable)
                                {
                                    o.SetIgnited(true);
                                    newIgnited.Add(o);
                                }
                                o.SetSelectable(selectable);
                            }
                        }
                    }
                    foreach (Block b in newIgnited) b.UpdateCondition();
                }
                else levelComplete = true;
            }
            return;
        }
        else if (moveEndTime > 0)
        {
            moveEndTime -= Time.deltaTime;
            return;
        }
        if (!active) return;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider == null)
        {
            if (hoverBlock >= 0) blocks[hoverBlock].SetHover(false);
            hoverBlock = -1;
        }
        else
        {
            Block b = hit.collider.GetComponent<Block>();
            int newHoverBlock = -1;
            for (int i = 0; i < blocks.Length; i++) if (b == blocks[i]) newHoverBlock = i;
            if (hoverBlock != newHoverBlock)
            {
                if (hoverBlock >= 0) blocks[hoverBlock].SetHover(false);
                if (newHoverBlock >= 0) blocks[newHoverBlock].SetHover(true);
                hoverBlock = newHoverBlock;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedBlock < 0 && hoverBlock >= 0 && blocks[hoverBlock].GetSelectable())
            {
                foreach (Block b in blocks) b.SetSelectable(false);
                bool up = true;
                bool down = true;
                bool left = true;
                bool right = true;
                bool c1 = blocks[hoverBlock].GetCondition();
                int x1 = c1 ? blocks[hoverBlock].alternateX : blocks[hoverBlock].defaultX;
                int y1 = c1 ? blocks[hoverBlock].alternateY : blocks[hoverBlock].defaultY;
                int count = 0;
                do
                {
                    count++;
                    bool pUp = up;
                    bool pDown = down;
                    bool pLeft = left;
                    bool pRight = right;
                    up = false;
                    down = false;
                    left = false;
                    right = false;
                    foreach (Block b in blocks)
                    {
                        if (b.GetIgnited())
                        {
                            bool c2 = b.GetCondition();
                            int x2 = c2 ? b.alternateX : b.defaultX;
                            int y2 = c2 ? b.alternateY : b.defaultY;
                            if (pUp && x1 == x2 && y1 + count == y2)
                            {
                                b.SetSelectable(true);
                                up = true;
                            }
                            else if (pDown && x1 == x2 && y1 - count == y2)
                            {
                                b.SetSelectable(true);
                                down = true;
                            }
                            else if (pLeft && x1 - count == x2 && y1 == y2)
                            {
                                b.SetSelectable(true);
                                left = true;
                            }
                            else if (pRight && x1 + count == x2 && y1 == y2)
                            {
                                b.SetSelectable(true);
                                right = true;
                            }
                        }
                    }
                }
                while (up || down || left || right);
                if(count > 1)
                {
                    blocks[hoverBlock].SetSelected(true);
                    selectedBlock = hoverBlock;
                }
                else
                {
                    blocks[hoverBlock].SetIgnited(false);
                    moveTime = transitionDuration;
                }
            }
            else if (selectedBlock >= 0)
            {
                if (hoverBlock >= 0 && blocks[hoverBlock].GetSelectable())
                {
                    bool c1 = blocks[selectedBlock].GetCondition();
                    int x1 = c1 ? blocks[selectedBlock].alternateX : blocks[selectedBlock].defaultX;
                    int y1 = c1 ? blocks[selectedBlock].alternateY : blocks[selectedBlock].defaultY;
                    bool c2 = blocks[hoverBlock].GetCondition();
                    int x2 = c2 ? blocks[hoverBlock].alternateX : blocks[hoverBlock].defaultX;
                    int y2 = c2 ? blocks[hoverBlock].alternateY : blocks[hoverBlock].defaultY;
                    List<Block> extinguished = new List<Block>();
                    foreach (Block b in blocks)
                    {
                        bool c3 = b.GetCondition();
                        int x3 = c3 ? b.alternateX : b.defaultX;
                        int y3 = c3 ? b.alternateY : b.defaultY;
                        if (x3 >= Mathf.Min(x1, x2) && x3 <= Mathf.Max(x1, x2) && y3 >= Mathf.Min(y1, y2) && y3 <= Mathf.Max(y1, y2))
                        {
                            b.SetIgnited(false);
                            extinguished.Add(b);
                        }
                    }
                    foreach(Block b in extinguished) b.UpdateCondition();
                    moveTime = transitionDuration;
                }
                blocks[selectedBlock].SetSelected(false);
                selectedBlock = -1;
                foreach (Block b in blocks) b.SetSelectable(b.GetIgnited());
            }
        }
    }

    public bool GetLevelComplete()
    {
        return levelComplete;
    }

    public void SetActive(bool a)
    {
        active = a;
    }
}
