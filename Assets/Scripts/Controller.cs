using System.Windows.Forms;
using UnityEngine;
public class Values
{
    private float currXPos;
    private float currYPos;
    private float nextXPos;
    private float nextYPos;
    private int feedRate;
    private bool zUp = false;

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
    Values val = new Values();
    public ConsoleToGUI console;
    private int numCommands;

    void Start()
    {
        UnityEngine.Application.runInBackground = true;
        //this.HomeAllAxis();
        // ホーミング後
        // this.OpenMDI();
    }

    private void HomeAllAxis()
    {
        SendKeys.SendWait("{F1}");
        SendKeys.SendWait("{F2}");
        SendKeys.SendWait("{F8}");
        SendKeys.SendWait("{F12}");
        val.CurrXPos = 1100f;
        val.CurrYPos = 0f;
    }

    private void OpenMDI()
    {
        SendKeys.SendWait("{F6}");
    }
    private void ClearMDI()
    {
        SendKeys.SendWait("^{A}");
        SendKeys.SendWait("{DELETE}");
        SendKeys.SendWait("{ENTER}");
    }

    public void SetNextCoord(int xPos, int yPos, int feedRate)
    {
        val.NextXPos = xPos;
        val.NextYPos = Random.Range(0f, 2400f);
        val.FeedRate = Random.Range(300, 5400);
        /*
        // 移動にかかる時間を予測
        var dist = Mathf.Sqrt(Mathf.Pow(val.NextXPos - val.CurXPos, 2) + Mathf.Pow(val.NextYPos - val.CurYPos, 2));
        estimatedTime = (dist * 60f) / val.FeedRate;
        */
        val.CurrXPos = val.NextXPos;
        val.CurrYPos = val.NextYPos;
        
        Debug.Log("G1 X" + val.NextXPos.ToString() + " Y" + val.NextYPos.ToString() + " F" + val.FeedRate.ToString() + "{ENTER}");
        if (numCommands < 10)
        {
            numCommands++;
        }
        else
        {
            console.ClearLog();
            this.ClearMDI();
            numCommands = 0;
        }
        //SendKeys.SendWait("G1 X" + val.NextXPos.ToString() + " Y" + val.NextYPos.ToString() + " F" + val.FeedRate.ToString() + "{ENTER}");
    }
}