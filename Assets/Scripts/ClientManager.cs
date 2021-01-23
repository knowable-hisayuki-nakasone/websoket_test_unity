using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Net;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ClientManager : MonoBehaviour
{
   public WebSocket ws;
   public Text chatText;
   public Button sendButton;
   public InputField messageInput;

   public Transform scrollContent;
   private Text cloneChatText = null;

   private static string chatMessage = "";

   //サーバへ、メッセージを送信する
   public void SendText()
   {
      ws.Send(messageInput.text);
      
   }

   void Update(){

      if(chatMessage == "") return;
   
      cloneChatText = Instantiate(chatText, Vector3.zero, Quaternion.identity, scrollContent);
      cloneChatText.text = chatMessage;
      cloneChatText.gameObject.transform.localPosition = Vector3.zero;
      chatMessage = "";
   }
   //サーバから受け取ったメッセージを、ChatTextに表示する
   public void RecvText(string text)
   {
      chatMessage = text + "\n";
   }

   //サーバの接続が切れたときのメッセージを、ChatTextに表示する
   public void RecvClose()
   {
      chatMessage = ("Close.");
   }

   void Start()
   {
      //接続処理。接続先サーバと、ポート番号を指定する
      ws = new WebSocket("ws://localhost:12345/");
      ws.Connect();

      chatText.text = "";

      //送信ボタンが押されたときに実行する処理「SendText」を登録する
      sendButton.onClick.AddListener(SendText);
      //サーバからメッセージを受信したときに実行する処理「RecvText」を登録する
      ws.OnMessage += (sender, e) => RecvText(e.Data);
      //サーバとの接続が切れたときに実行する処理「RecvClose」を登録する
      ws.OnClose += (sender, e) => RecvClose();
   }
}