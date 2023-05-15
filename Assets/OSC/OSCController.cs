using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOSC;

public class OSCController : MonoBehaviour {

    private OSCHandler handler;
    private OSCReceiver receiver;
    private string address = "192.168.0.136";
	private int sendPort = 8888;
    public int receivePort = 8888;
    public Controller controller;
    //public bool isSenderActive;

	void Start () {
        handler = OSCHandler.Instance;
        handler.Init("Max", address, sendPort);
        handler.transform.SetParent(gameObject.transform);
		receiver = new OSCReceiver();
		receiver.Open(receivePort);

        //if (isSenderActive)
            //StartCoroutine(this.EmulateSender());
    }	
	
	void Update () {
		if(receiver.hasWaitingMessages()) {
			OSCMessage msg = receiver.getNextMessage();

            // データ形式によって振り分け
            var b = (msg.Address == "/EEG" || msg.Address == "/BandPower") ? true : false;
            ReceiveMessages(msg.Data, b);
        }
    }
    /*
    private IEnumerator EmulateSender()
    {
        var cachedWait = new WaitForSeconds(1f);
        while (true) // このGameObjectが有効な間は
        {
            this.SendMessages(address, "/bandpower ");
            yield return cachedWait;
        }
    }
    */
    public void SendMessages(string address, string value) {
        handler.SendMessageToClient("Max", address, value);
    }

    private void ReceiveMessages(List<object> data, bool hasMultipleData) {
        /*
        送られてくるデータ形式

        /eeg
            [0]EEG生データ（String、1データごとに「;」で分けられる） [1]タイムスタンプ（String）
            例: "value0;value1;value2;value3;" "YYYY - MM - ddTHH:mm: ssZZZ"

        /bandpower
            [0]データ（String、1データごとに「;」で分けられる） [1]タイムスタンプ（String）
            例: "alpha;beta;theta;delta;gamma" "YYYY - MM - ddTHH:mm: ssZZZ"

        /attention
            [0]attentionスコア (Float) [1]タイムスタンプ（String）
            例: 100f "YYYY - MM - ddTHH:mm: ssZZZ"

        /meditation
            [0]meditationスコア (Float)　[1]タイムスタンプ （String）
            例: 100f "YYYY - MM - ddTHH:mm: ssZZZ"
        */

        //bool hasMultipleData = data[0].ToString().Contains(";");
        if (hasMultipleData)
        {
            string[] waves = data[0].ToString().Split(";");
            controller.SetNextCoord(waves[0], waves[1], waves[2], waves[3], waves[4]);
            //for (int i = 0; i < waves.Length; i++)
                //Debug.Log(i + ": " + waves[i]);
        }
        else
        {
            //controller.SetNextCoord(data[0], 0, 0);
            Debug.Log(data[0]);
        }
		
	}
}