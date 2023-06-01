using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateWaves : MonoBehaviour
{
    private LineRenderer renderer;
    public AnimationCurve xCurve;
    public AnimationCurve yCurve;
    private float nextXPos;
    private float nextYPos;
    private float currXPos;
    private float currYPos;
    private bool xFlag;
    private bool keyPressed;
    
    void Start()
    {
        renderer = gameObject.GetComponent<LineRenderer>();
        
        renderer.startWidth = 0.1f;
        renderer.positionCount = 512;
        this.DrawWave();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !keyPressed)
        {
            this.Reset();
            this.DrawWave();
            keyPressed = false;
        }
    }

    private void DrawWave()
    {
        for(int i = 0; i < 512; i++)
        {
            var score = Random.Range(0f, 100f);
            var moveDistX = score * xCurve.Evaluate(score / 100) * 0.01f;
            var moveDistY = 1f;//(score * xCurve.Evaluate(score / 100));

            nextXPos = xFlag ? currXPos + moveDistX : currXPos - moveDistX;
            nextYPos = currYPos + moveDistY;
            
            renderer.SetPosition(i, new Vector2(nextYPos, nextXPos));

            xFlag = !xFlag;
            currXPos = nextXPos;
            currYPos = nextYPos;
        }
    }

    private void Reset()
    {
            for(int i = 0; i < 512; i++)
                renderer.SetPosition(i, Vector2.zero);
            currXPos = 0f;
            currYPos = 0f;
            nextXPos = 0f;
            nextYPos = 0f;
            renderer.positionCount = 512;
    }
}
