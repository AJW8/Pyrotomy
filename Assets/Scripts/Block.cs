using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject rim;
    public GameObject face;
    public GameObject[] conditioned;
    public Color selectableColour;
    public Color hoverColour;
    public Color selectedColour;
    public Color flameColour1;
    public Color flameColour2;
    public int defaultX;
    public int defaultY;
    public int alternateX;
    public int alternateY;
    public float transitionDuration;
    public float flameAnimationDuration;
    private Block[] conditionedBlocks;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer rimSR;
    private SpriteRenderer faceSR;
    private bool ignited;
    private bool condition;
    private bool selectable;
    private bool hover;
    private bool selected;
    private float ignitionFactor;
    private float conditionFactor;
    private float flameAnimationTime;

    // Start is called before the first frame update
    void Start()
    {
        conditionedBlocks = new Block[conditioned.Length];
        for (int i = 0; i < conditioned.Length; i++) conditionedBlocks[i] = conditioned[i].GetComponent<Block>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = selectableColour;
        rimSR = rim.GetComponent<SpriteRenderer>();
        faceSR = face.GetComponent<SpriteRenderer>();
        rimSR.color = flameColour1;
        transform.position = new Vector3(defaultX, defaultY, transform.position.z);
        ignited = true;
        condition = false;
        selectable = true;
        hover = false;
        selected = false;
        ignitionFactor = 0;
        conditionFactor = 0;
        flameAnimationTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        flameAnimationTime += Time.deltaTime;
        if(flameAnimationTime >= flameAnimationDuration) flameAnimationTime -= flameAnimationDuration;
        if (ignited && ignitionFactor < transitionDuration)
        {
            ignitionFactor += Time.deltaTime;
            if (ignitionFactor > transitionDuration) ignitionFactor = transitionDuration;
        }
        else if (!ignited && ignitionFactor > 0)
        {
            ignitionFactor -= Time.deltaTime;
            if (ignitionFactor < 0) ignitionFactor = 0;
        }
        if (ignitionFactor > 0)
        {
            Color flameColour = flameColour1 + (flameColour2 - flameColour1) * (1 - Mathf.Cos(Mathf.PI * 2f * flameAnimationTime / flameAnimationDuration)) / 2f;
            rimSR.color = faceSR.color + (flameColour - faceSR.color) * ignitionFactor / transitionDuration;
        }
        if (condition && conditionFactor < transitionDuration)
        {
            conditionFactor += Time.deltaTime;
            if (conditionFactor > transitionDuration) conditionFactor = transitionDuration;
            transform.position = new Vector3(defaultX + (alternateX - defaultX) * (1 - Mathf.Cos(Mathf.PI * conditionFactor / transitionDuration)) / 2f, defaultY + (alternateY - defaultY) * (1 - Mathf.Cos(Mathf.PI * conditionFactor / transitionDuration)) / 2f, transform.position.z);
        }
        else if (!condition && conditionFactor > 0)
        {
            conditionFactor -= Time.deltaTime;
            if (conditionFactor < 0) conditionFactor = 0;
            transform.position = new Vector3(defaultX + (alternateX - defaultX) * (1 - Mathf.Cos(Mathf.PI * conditionFactor / transitionDuration)) / 2f, defaultY + (alternateY - defaultY) * (1 - Mathf.Cos(Mathf.PI * conditionFactor / transitionDuration)) / 2f, transform.position.z);
        }
    }

    public bool GetIgnited()
    {
        return ignited;
    }

    public bool GetCondition()
    {
        return condition;
    }

    public bool GetSelectable()
    {
        return selectable;
    }

    public void SetIgnited(bool i)
    {
        ignited = i;
    }

    public void UpdateCondition()
    {
        foreach (Block b in conditionedBlocks) b.SetCondition(!ignited);
    }

    void SetCondition(bool c)
    {
        condition = c;
    }

    public void SetSelectable(bool s)
    {
        if (s) spriteRenderer.color = hover ? hoverColour : selectableColour;
        else
        {
            Color c = spriteRenderer.color;
            c.a = 0;
            spriteRenderer.color = c;
        }
        selectable = s;
    }

    public void SetHover(bool h)
    {
        if (selectable) spriteRenderer.color = h ? hoverColour : selectableColour;
        hover = h;
    }

    public void SetSelected(bool s)
    {
        if (selected != s)
        {
            if (selectable) spriteRenderer.color = hover ? hoverColour : selectableColour;
            else if (s) spriteRenderer.color = selectedColour;
            else
            {
                Color c = spriteRenderer.color;
                c.a = 0;
                spriteRenderer.color = c;
            }
            selected = s;
        }
    }
}
