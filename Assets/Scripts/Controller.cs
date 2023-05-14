using System.Collections;
using System.Windows.Forms;
using UnityEngine;
public class Values
{
    private float currXPos;
    private float currYPos;
    private float nextXPos;
    private float nextYPos;
    private int feedRate;

    public float CurrXPos
    {
        set { this.currXPos = value; }
        get { return this.currXPos; }
    }
    public float CurrYPos
    {
        set { this.currYPos = value; }
        get { return this.currYPos; }
    }
    public float NextXPos 
    {
        set{ this.nextXPos = value; }
        get{ return this.nextXPos; }
    }
    public float NextYPos
    {
        set { this.nextYPos = value; }
        get { return this.nextYPos; }
    }
    public int FeedRate
    {
        set { this.feedRate = value; }
        get { return this.feedRate; }
    }
}

public class Controller : MonoBehaviour
{
    //private bool isReady;
    static private float yLimit = 2350f;
    static private float xLimit = 1100f;

    Values val = new Values();

    public ConsoleToGUI console;
    private int numCommands;

    void Start()
    {
        UnityEngine.Application.runInBackground = true;        
        StartCoroutine(HomingProcess());        
    }

    private void HomeAllAxis()
    {
        SendKeys.SendWait("{F1}");
        SendKeys.SendWait("{F2}");
        SendKeys.SendWait("{F8}");
        SendKeys.SendWait("{F12}");
        val.CurrXPos = xLimit;
        val.CurrYPos = 0f;
    }

    private IEnumerator HomingProcess()
    {
        this.OpenMDI();
        SendKeys.SendWait("M8" + "{ENTER}");
        yield return new WaitForSeconds(10f);

        this.HomeAllAxis();
        yield return new WaitForSeconds(10f);

        this.OpenMDI();
        this.MoveToCenter();
    }

    private void OpenMDI()
    {
        SendKeys.SendWait("{F6}");
    }

    private void MoveToCenter()
    {
        SendKeys.SendWait("M8" + "{ENTER}");
        SendKeys.SendWait("G0 X550 Y1175" + "{ENTER}");
        //移動後に下ろす
        //isReady = true;
    }
    private void ClearMDI()
    {
        SendKeys.SendWait("^{A}");
        SendKeys.SendWait("{DELETE}");
        SendKeys.SendWait("{ENTER}");
    }

    public void SetNextCoord(string xPos, string yPos, string xDir, string yDir, string feedRate)
    {
        // if(!isReady)
            // return;

        val.NextXPos = float.Parse(xDir) > 10 ? val.CurrXPos + float.Parse(xPos) : val.CurrXPos - float.Parse(xPos);
        val.NextYPos = float.Parse(yDir) > 10 ? val.CurrYPos + float.Parse(yPos) : val.CurrYPos - float.Parse(yPos);
        val.FeedRate = (int)(float.Parse(feedRate) * 100);

        if (val.NextXPos > xLimit)
            val.NextXPos = val.CurrXPos - float.Parse(xPos);
        if (val.NextYPos > yLimit)
            val.NextYPos = val.CurrYPos - float.Parse(yPos);
        if (val.NextXPos < 0)
            val.NextXPos = val.CurrXPos + float.Parse(xPos);
        if (val.NextYPos < 0)
            val.NextYPos = val.CurrYPos + float.Parse(yPos);

        /*
        // 移動にかかる時間を予測
        var dist = Mathf.Sqrt(Mathf.Pow(val.NextXPos - val.CurXPos, 2) + Mathf.Pow(val.NextYPos - val.CurYPos, 2));
        estimatedTime = (dist * 60f) / val.FeedRate;
        */

        SendKeys.SendWait("G1 X" + val.NextXPos.ToString() + " Y" + val.NextYPos.ToString() + " F" + val.FeedRate.ToString() + "{ENTER}");
        //Debug.Log("G1 X" + val.NextXPos.ToString() + " Y" + val.NextYPos.ToString() + " F" + val.FeedRate.ToString() + "{ENTER}");

        val.CurrXPos = val.NextXPos;
        val.CurrYPos = val.NextYPos;

        if (numCommands < 10)
        {
            numCommands++;
        }
        else
        {
            //console.ClearLog();
            this.ClearMDI();
            numCommands = 0;
        }
    }
}