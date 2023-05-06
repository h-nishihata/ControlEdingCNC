using System.Windows.Forms;
using UnityEngine;
public class Values
{
    private float curXPos;
    private float curYPos;
    private float nextXPos;
    private float nextYPos;

    private int feedRate;
    public float CurXPos
    {
        set { this.curXPos = value; }
        get { return this.curXPos; }
    }
    public float CurYPos
    {
        set { this.curYPos = value; }
        get { return this.curYPos; }
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
    public float count = 10f;
    private float estimatedTime;
    bool flag;

    private bool zUp = false;
    Values val = new Values();

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
        val.CurXPos = 1100f;
        val.CurYPos = 0f;
    }

    private void OpenMDI()
    {
        SendKeys.SendWait("{F12}");
        SendKeys.SendWait("{F6}");
    }
    private void ClearMDI()
    {
        SendKeys.SendWait("^{A}");
        SendKeys.SendWait("{DELETE}");
        SendKeys.SendWait("{ENTER}");
    }

    void Update()
    {
        /*
        if (count > 0f)
            count -= Time.deltaTime;

        if ((count <= 0f) && !flag)
        {
            this.SetNextCoord();
            flag = true;
        }
        */        
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
        val.CurXPos = val.NextXPos;
        val.CurYPos = val.NextYPos;
        Debug.Log("G1 X" + val.NextXPos.ToString() + " Y" + val.NextYPos.ToString() + " F" + val.FeedRate.ToString() + "{ENTER}");
        //SendKeys.SendWait("G1 X" + val.NextXPos.ToString() + " Y" + val.NextYPos.ToString() + " F" + val.FeedRate.ToString() + "{ENTER}");
    }
}