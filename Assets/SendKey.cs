using UnityEngine;
using System.Windows.Forms;


public class SendKey : MonoBehaviour
{
   //[DllImport("user32.dll", CharSet = CharSet.Auto)]
   //public static extern IntPtr GetActiveWindow();
   //IntPtr hWnd;

    public float count;
    bool flag;

    void Start()
    {
        //hWnd = GetActiveWindow();
        UnityEngine.Application.runInBackground = true;
    }

    void Update()
    {
        count += Time.deltaTime;
        if ((count > 10f) && !flag)
        {
            flag = true;
            SendKeys.SendWait("{UP}");
            Debug.Log("sent");
        }
    }
}
