using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOSC;

public class OSCController : MonoBehaviour {

    private OSCHandler handler;
    private OSCReceiver receiver;
    private string address = "172.16.202.207";
	private int sendPort = 7400;
    public int receivePort = 6666;
    public Controller controller;

	void Start () {
        handler = OSCHandler.Instance;
        handler.Init("Max", address, sendPort);
        handler.transform.SetParent(gameObject.transform);
		receiver = new OSCReceiver();
		receiver.Open(receivePort);
	}	
	
	void Update () {
		if(receiver.hasWaitingMessages()) {            
			OSCMessage msg = receiver.getNextMessage();
            ReceiveMessages(msg.Data);
        }
    }
	
    public void SendMessages(string address, float value) {
        handler.SendMessageToClient("Max", address, value);
    }

    private void ReceiveMessages(List<object> data) {
		for(int i = 0; i < data.Count; i++) {

            Debug.Log(data[0].ToString());
            //controller.SetNextCoord(num, 0, 0);
		}
	}
}