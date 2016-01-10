using UnityEngine;
using System.Collections;

public class client : MonoBehaviour
{
	//要连接的服务器地址
	string IP = "127.0.0.1";
	//要连接的端口
	int Port = 10100;
	//聊天信息
	string Message = "";
	//声明一个二维向量 
	Vector2 Sc;

	void OnGUI()
	{
		//端类型的状态
		switch (Network.peerType)
		{
			//禁止客户端连接运行, 服务器未初始化
			case NetworkPeerType.Disconnected:
				StartConnect();
				break;
			//运行于服务器端
			case NetworkPeerType.Server:
				break;
			//运行于客户端
			case NetworkPeerType.Client:
				OnClient();
				break;
			//正在尝试连接到服务器
			case NetworkPeerType.Connecting:
				break;
		}
	}

	void StartConnect()
	{
		if (GUILayout.Button("连接服务器"))
		{
			NetworkConnectionError error = Network.Connect(IP, Port);
			//连接状态
			switch (error)
			{
				case NetworkConnectionError.NoError:
					break;
				default:
					Debug.Log("客户端错误" + error);
					break;
			}
		}
	}

	void OnClient()
	{
		//创建开始滚动视图
		Sc = GUILayout.BeginScrollView(Sc, GUILayout.Width(280), GUILayout.Height(400));
		//绘制纹理, 显示内容
		GUILayout.Box(Message);
		//文本框
		Message = GUILayout.TextArea(Message);
		if (GUILayout.Button("发送"))
		{
			//发送给接收的函数, 模式为全部, 参数为信息
			GetComponent<NetworkView>().RPC("ReciveMessage", RPCMode.All, Message);
		}
		//结束滚动视图, 注意, 与开始滚动视图成对出现
		GUILayout.EndScrollView();

	}

	//接收请求的方法. 注意要在上面添加[RPC]
	[RPC]
	void ReciveMessage(string msg, NetworkMessageInfo info)
	{
		//刚从网络接收的数据的相关信息,会被保存到NetworkMessageInfo这个结构中
		Message = "发送端" + info.sender + "消息" + msg;
	}

}