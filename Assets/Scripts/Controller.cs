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
#region variables
    static private float xLimit = 1090f; // ワークエリアの限界から10mm控えてある.
    static private float yLimit = 2340f;
    Values val = new Values();

    private bool isReady;
    private bool isMoving;
    private float prevVal;

    public AnimationCurve curve;
    private float moveDistX;
    private float moveDistY;
    private bool xFlag;
    private bool yFlag = true;
    private float estimatedTime;
#endregion

    void Start()
    {
        UnityEngine.Application.runInBackground = true;        
        StartCoroutine(HomingProcess());     
    }
#region EdingCNCFunctions
    private IEnumerator HomingProcess()
    {
        // デバッグ用.本番はバッチファイルで遅延させる.
        yield return new WaitForSeconds(10f);

        // ホーミングの前にペン先を上げる？
        //this.OpenMDI();
        //SendKeys.SendWait("M8" + "{ENTER}");
        //SendKeys.SendWait("{F12}");
        //yield return new WaitForSeconds(10f);

        this.HomeAllAxis();
        yield return new WaitForSeconds(1f);
        // 最大で2'30"はかかる.        
        //yield return new WaitForSeconds(180f);

        this.OpenMDI();
        this.MoveToStartPos();
    }

    private void HomeAllAxis()
    {
        Debug.Log("Homing all axis...");
        SendKeys.SendWait("{F1}");
        SendKeys.SendWait("{F2}");
        SendKeys.SendWait("{F8}");
        SendKeys.SendWait("{F12}");
        val.CurrXPos = xLimit;
        val.CurrYPos = 0f;
    }

    private void OpenMDI()
    {
        SendKeys.SendWait("{F6}");
    }

    private void MoveToStartPos()
    {
        //SendKeys.SendWait("M8" + "{ENTER}");
        SendKeys.SendWait("G1 X1000 Y10 F3200" + "{ENTER}");
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
        // 移動完了したら次のOSCを受け取る.
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
    }

    /// <summary>
    /// 次の座標を計算し、MDIにキーストロークを送信する.
    /// </summary>
    /// <param name="meditationScore">リラックス度合</param>
    public void VisualizeMeditation(string meditationScore)
    {
        if (!isReady || isMoving)
            return;

        var score = float.Parse(meditationScore);

        // ヘッドセット未装着.
        if(Mathf.Abs((int)score - (int)prevVal) < 2)
            return;

        moveDistX = score * curve.Evaluate(score / 100) * 0.5f;
        moveDistY = (100 - (score * curve.Evaluate(score / 100))) * 0.01f;
        val.FeedRate = (int)(score * curve.Evaluate(score / 100)) * 100 + 1200;

        val.NextXPos = xFlag ? val.CurrXPos + moveDistX : val.CurrXPos - moveDistX;
        val.NextYPos = yFlag ? val.CurrYPos + moveDistY : val.CurrYPos - moveDistY;

        // 移動にかかる時間を予測.
        var dist = Mathf.Sqrt(Mathf.Pow(val.NextXPos - val.CurrXPos, 2) + Mathf.Pow(val.NextYPos - val.CurrYPos, 2));
        estimatedTime = (dist * 100f) / val.FeedRate;
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

        if (val.NextYPos > yLimit || val.NextYPos < 10f)
        {
            val.NextXPos = Random.Range(20f, 990f);
            val.NextYPos = val.CurrYPos;
            yFlag = !yFlag;
        }
    }
}