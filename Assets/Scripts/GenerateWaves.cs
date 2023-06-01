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
    
    void Start()
    {
        renderer = gameObject.GetComponent<LineRenderer>();
        
        renderer.SetWidth(0.5f, 0.5f); //(start, end)
        renderer.SetVertexCount(512);        
        //renderer.SetPosition(0, Vector2.zero);

        for(int i = 0; i < 512; i++)
        {
            var score = Random.Range(0f, 100f);
            var moveDistX = score * xCurve.Evaluate(score / 100) * 0.01f;
            var moveDistY = 0.1f;//(score * xCurve.Evaluate(score / 100));

            nextXPos = xFlag ? currXPos + moveDistX : currXPos - moveDistX;
            nextYPos = currYPos + moveDistY;
            
            renderer.SetPosition(i, new Vector2(nextYPos, nextXPos));

            xFlag = !xFlag;
            currXPos = nextXPos;
            currYPos = nextYPos;
        }
    }
}
