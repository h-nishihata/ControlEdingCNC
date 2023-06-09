﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOSC;

public class OSCController : MonoBehaviour {

    private OSCHandler handler;
    private OSCReceiver receiver;
    private string address = "192.168.0.136";
	private int sendPort = 8888;
    public int receivePort = 8888;
    public LiveWaveGenerator controller;

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
    
    public void SendMessages(string address, string value) {
        handler.SendMessageToClient("Max", address, value);
    }

    private void ReceiveMessages(List<object> data) {
        /*
        送られてくるデータ形式(今回使用するのは/Attention).

        /EEG
            [0]EEG生データ（String、1データごとに「;」で分けられる） [1]タイムスタンプ（String）
            例: "value0;value1;value2;value3;" "YYYY - MM - ddTHH:mm: ssZZZ"

        /BandPower
            [0]データ（String、1データごとに「;」で分けられる） [1]タイムスタンプ（String）
            例: "alpha;beta;theta;delta;gamma" "YYYY - MM - ddTHH:mm: ssZZZ"

        /Attention
            [0]attentionスコア (Float) [1]タイムスタンプ（String）
            例: 100f "YYYY - MM - ddTHH:mm: ssZZZ"

        /Meditation
            [0]meditationスコア (Float)　[1]タイムスタンプ （String）
            例: 100f "YYYY - MM - ddTHH:mm: ssZZZ"
        */

        controller.VisualizeMeditation(data[0].ToString());
	}
}