using System;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using System.Drawing;
using System.Threading;
using System.Threading.Tasks;


public class SendKey : MonoBehaviour
{
    #region NativeMethods
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll", SetLastError = true)]
    public extern static void SendInput(int nInputs, Input[] pInputs, int cbsize);

    IntPtr hWnd;
    #endregion

    public string key = "{UP}";
    public float count = 10f;
    bool flag;

    void Start()
    {
        UnityEngine.Application.runInBackground = true;
    }

    void Update()
    {
        if (count > 0f)
            count -= Time.deltaTime;

        if ((count <= 0f) && !flag)
        {
            //hWnd = GetActiveWindow();
            SendKeys.SendWait(key);
            flag = true;
            Debug.Log("sent");
        }
    }
}
