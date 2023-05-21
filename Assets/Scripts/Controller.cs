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
    static private float xLimit = 1090f;
    static private float yLimit = 2340f;
    Values val = new Values();

    private bool isReady;
    private bool isMoving;
    private int numCommands;

    private float estimatedTime;
    public ConsoleToGUI console;

    private bool xFlag, yFlag;
    private float prevVal;

    void Start()
    {
        UnityEngine.Application.runInBackground = true;        
        StartCoroutine(HomingProcess());        
    }
#region EdingCNCFunctions
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
        yield return new WaitForSeconds(10f);

        //this.OpenMDI();
        //SendKeys.SendWait("M8" + "{ENTER}");
        //SendKeys.SendWait("{F12}");
        //yield return new WaitForSeconds(10f);

        this.HomeAllAxis();
        yield return new WaitForSeconds(10f);
        // 最大で2'30"はかかる.        
        //yield return new WaitForSeconds(180f);

        this.OpenMDI();
        this.MoveToCenter();
    }

    private void OpenMDI()
    {
        SendKeys.SendWait("{F6}");
    }

    private void MoveToCenter()
    {
        //SendKeys.SendWait("M8" + "{ENTER}");
        SendKeys.SendWait("G0 X550 Y1175" + "{ENTER}");
        //移動後にペン先を下ろす.
        isReady = true;
        val.CurrXPos = 550f;
        val.CurrYPos = 1175f;
    }

    private void ClearMDI()
    {
        SendKeys.SendWait("^{A}");
        SendKeys.SendWait("{DELETE}");
        SendKeys.SendWait("{ENTER}");
    }
#endregion

    private void Update()
    {
        /*
        // [TO DO] OSCを指定フレーム数分無視するのではなく、移動完了したら受け取るようにしたい.
        if(estimatedTime > 0f)
        {
            isMoving = true;
            estimatedTime -= Time.deltaTime;
        }
        else
        {
            isMoving = false;
            estimatedTime = 0f;
        }
        */
    }

    /// <summary>
    /// /BandPowerを使って描く場合.
    /// </summary>
    /// <param name="xPos">α波をX座標に置換</param>
    /// <param name="yPos">β波をY座標に置換</param>
    /// <param name="xDir">θ波をX軸の方向に置換</param>
    /// <param name="yDir">Δ波をY軸の方向に置換</param>
    /// <param name="feedRate">Γ波をF値に置換</param>
    public void DrawRandomLines(string xPos, string yPos, string xDir, string yDir, string feedRate)
    {
        if(!isReady)
            return;

        if (numCommands < 15)
        {
            // OSCが送られてくる頻度を調節.
            numCommands++;
        }
        else
        {
            // 現在地から移動する量.
            var moveDistX = float.Parse(xPos) > 40f ? 200f : float.Parse(xPos) * 5f;
            var moveDistY = float.Parse(yPos) > 40f ? 200f : float.Parse(yPos) * 5f;

            // +/-方向どちらに移動するかを決め, 次の座標として設定する.
            val.NextXPos = float.Parse(xDir) > 0.5f ? val.CurrXPos + moveDistX : val.CurrXPos - moveDistX;
            val.NextYPos = float.Parse(yDir) > 0.5f ? val.CurrYPos + moveDistY : val.CurrYPos - moveDistY;
            val.FeedRate = float.Parse(feedRate) < 0.1f ? (int)(float.Parse(feedRate) * 50000f) : 5000;

            this.CheckWorkAreaLimit(moveDistX, moveDistY);

            /*

            */
            
            // MDIへキーストロークを送信.
            SendKeys.SendWait("G1 X" + val.NextXPos.ToString() + " Y" + val.NextYPos.ToString() + " F" + val.FeedRate.ToString() + "{ENTER}");

            val.CurrXPos = val.NextXPos;
            val.CurrYPos = val.NextYPos;

            numCommands = 0;
            //this.ClearMDI();
            //console.ClearLog();
        }
    }

    /// <summary>
    /// /Meditationを使って描く場合.
    /// </summary>
    /// <param name="meditationScore">リラックス度合</param>
    public void VisualizeMeditation(string meditationScore)
    {
        if (!isReady || isMoving)
            return;

        var score = float.Parse(meditationScore);
        if(Mathf.Abs((int)score - (int)prevVal) < 1)
            return;

        Debug.Log(score);
        var moveDistX = score * score * 0.05f;
        var moveDistY = (10000 - score * score) * 0.005f;
        var feedRate = (int)((100 - score) * 120) + 2000;

        val.NextXPos = xFlag ? val.CurrXPos + moveDistX : val.CurrXPos - moveDistX;
        val.NextYPos = yFlag ? val.CurrYPos + moveDistY : val.CurrYPos - moveDistY;
        val.FeedRate = feedRate;

        // 移動にかかる時間を予測.
        //var dist = Mathf.Sqrt(Mathf.Pow(val.NextXPos - val.CurrXPos, 2) + Mathf.Pow(val.NextYPos - val.CurrYPos, 2));
        //estimatedTime = (dist * 60f) / val.FeedRate;
        this.CheckWorkAreaLimit(moveDistX, moveDistY);

        SendKeys.SendWait("G1 X" + val.NextXPos.ToString() + " Y" + val.NextYPos.ToString() + " F" + val.FeedRate.ToString() + "{ENTER}");
        val.CurrXPos = val.NextXPos;
        val.CurrYPos = val.NextYPos;
        prevVal = score;
        xFlag = !xFlag;      
    }

    /// <summary>
    /// ワークエリアからはみ出さないように調節.
    /// </summary>
    /// <param name="distX">X方向の移動量</param>
    /// <param name="distY">Y方向の移動量</param>
    void CheckWorkAreaLimit(float distX, float distY)
    {
        if (val.NextXPos > xLimit)
            val.NextXPos = val.CurrXPos - distX;
        if (val.NextXPos < 10)
            val.NextXPos = val.CurrXPos + distX;

        if (val.NextYPos > yLimit-10f || val.NextYPos < 10f)
        {
            val.NextXPos = Random.Range(20f, 990f);
            val.NextYPos = val.CurrYPos;
            yFlag = !yFlag;
        }
    }
}