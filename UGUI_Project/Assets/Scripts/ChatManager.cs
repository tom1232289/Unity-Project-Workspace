using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour {
    // 公有引用
    public InputField m_InputField;
    public Text m_ChatWindowText;

    // 私有变量
    private string m_sIP = "192.168.1.105"; // 服务器IP
    private int m_iPort = 8881; // 服务器端口
    private Socket m_clientSocket;
    private Thread m_Thread;
    private byte[] m_data = new byte[1024];
    private string m_sData;

    private void Start() {
        ConnectToServer();
    }

    private void Update() {
        // 更新聊天窗口
        if (m_sData != "") {
            m_ChatWindowText.text += (m_sData + '\n');
            m_sData = "";
        }
        // 按下回车，发送消息
        if (Input.GetKey(KeyCode.Return)) {
            OnSendBtnClicked();
        }
    }

    private void ConnectToServer() {
        m_clientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        m_clientSocket.Connect(new IPEndPoint(IPAddress.Parse(m_sIP), m_iPort));
        // 创建一个线程用于接收消息
        m_Thread = new Thread(ReceiveMessage);
        m_Thread.Start();
    }

    private void SendMessageToServer(string msg) {
        m_clientSocket.Send(Encoding.UTF8.GetBytes(msg));
    }

    // 循环接收消息
    private void ReceiveMessage() {
        while (true) {
            if (!m_clientSocket.Connected) {
                break;
            }

            int iLength = m_clientSocket.Receive(m_data);
            m_sData = Encoding.UTF8.GetString(m_data, 0, iLength);
        }
    }

    public void OnSendBtnClicked() {
        SendMessageToServer(m_InputField.text);
        m_InputField.text = "";
        // 激活输入框
        m_InputField.ActivateInputField();
    }

    private void OnDestory() {
        m_clientSocket.Close();
    }
}
