using System;
using System.Collections;
using System.Windows.Forms;
using UnityEngine;

public class CNCController : MonoBehaviour
{
    #region variables
    public string fileName = "test.gcode";
    private bool isReady;
    private bool isMoving;
    private int countToClearMDI;
#endregion
    void Start()
    {
        UnityEngine.Application.runInBackground = true;
        StartCoroutine(HomingProcess());
    }
    #region EdingCNCFunctions
    private IEnumerator HomingProcess()
    {
        // 本番はUnityアプリ → Edingの順で、バッチファイルで遅延させて立ち上げるので、その分(+余裕を持たせて)待機する.
        Debug.Log(DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ": Wainting for EdingCNC to start up...");
        yield return new WaitForSeconds(10f);

        // ホーミングの前にペン先を上げる？
        //this.OpenMDI();
        //SendKeys.SendWait("M8" + "{ENTER}");
        //SendKeys.SendWait("{F12}");
        //yield return new WaitForSeconds(10f);

        this.HomeAllAxis();
        yield return new WaitForSeconds(10f);
        // 最大で2'30"はかかる.        
        //yield return new WaitForSeconds(180f);

        this.LoadGCode();
        this.StartDrawing();
    }

    private void HomeAllAxis()
    {
        Debug.Log(DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ": Homing all axis...");
        SendKeys.SendWait("{F1}");
        SendKeys.SendWait("{F2}");
        SendKeys.SendWait("{F8}");
        SendKeys.SendWait("{F12}");
    }

    private void LoadGCode()
    {
        Debug.Log(DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ": Homing Complete.");
        SendKeys.SendWait("{F4}");
        SendKeys.SendWait("{F2}");
        SendKeys.SendWait(fileName);
        SendKeys.SendWait("{ENTER}");
        Debug.Log(DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ": Gcode Loaded.");
    }

    private void StartDrawing()
    {
        SendKeys.SendWait("{F4}");
        Debug.Log(DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ": Start Drawing !!");
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
    #endregion

    private void Update()
    {
    }
}
