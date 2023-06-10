using System;
using System.Collections;
using System.Windows.Forms;
using UnityEngine;

public class CNCController : MonoBehaviour
{
    public string fileName = "test.gcode";
    public int waitForEding = 60;
    public int waitForHoming = 60;
    public int waitForLoading = 10;
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
        yield return new WaitForSeconds(waitForEding);

        this.HomeAllAxis();
        yield return new WaitForSeconds(waitForHoming);

        this.LoadGCode();
        yield return new WaitForSeconds(waitForLoading);
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
}
