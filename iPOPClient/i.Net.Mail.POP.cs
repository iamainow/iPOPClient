namespace i
{
	namespace Net
	{
		namespace Mail
		{
			namespace POP
			{
				public static class Message
				{
					public enum Status:byte
					{
						OK,
						ERROR,
						UNKNOWN,
					}
					public static Status GetStatus(string S)
					{
						switch(S[0])
						{
							case '+':
								return Status.OK;
							case '-':
								return Status.ERROR;
							default:
								return Status.UNKNOWN;
						}
					}
					public static Status GetStatus(byte[] B)
					{
						return Message.GetStatus(System.Text.Encoding.ASCII.GetString(B));
					}
				}
				namespace Status
				{
					public enum Global:byte
					{
						CONNECTED,
						DISCONNECTED,
					}
					public enum Local:byte
					{
						CONNECT,
						USER,
						PASS,
						STAT,
						QUIT,
						DISCONNECT,
						NONE,
					}
				}
				public class Client
				{
					public event System.Action<Status.Local> LocalStatusChanged;
					public event System.Action<Status.Global> GlobalStatusChanged;
					public bool ACTIVE;
					public int RECEIVE_TIMEOUT;
					private int RECEIVE_SLEEP;
					private Status.Global GLOBAL_STATUS;
					private Status.Local LOCAL_STATUS;
					public Status.Global global_status
					{
						get
						{
							return this.GLOBAL_STATUS;
						}
						set
						{
							this.GLOBAL_STATUS=value;
							if(this.GlobalStatusChanged!=null)
							{
								this.GlobalStatusChanged(this.GLOBAL_STATUS);
							}
						}
					}
					public Status.Local local_status
					{
						get
						{
							return this.LOCAL_STATUS;
						}
						set
						{
							this.LOCAL_STATUS=value;
							if(this.LocalStatusChanged!=null)
							{
								this.LocalStatusChanged(this.LOCAL_STATUS);
							}
						}
					}
					public System.Net.Sockets.TcpClient CLIENT;
					//
					public Client(int receive_timeout)
					{
						this.ACTIVE=true;
						this.RECEIVE_TIMEOUT=receive_timeout;
						this.RECEIVE_SLEEP=1;
						this.CLIENT=null;
						this.global_status=Status.Global.DISCONNECTED;
						this.local_status=Status.Local.NONE;
					}
					public string Connect(string Server,int Port)
					{
						this.ACTIVE=true;
						this.global_status=Status.Global.CONNECTED;
						this.local_status=Status.Local.CONNECT;
						this.CLIENT=new System.Net.Sockets.TcpClient();
						this.CLIENT.Client.NoDelay=true;
						this.CLIENT.SendBufferSize=256;
						this.CLIENT.ReceiveBufferSize=256;
						this.CLIENT.Connect(Server,Port);
						return this.ListenString();
					}
					public string Connect(string Server)
					{
						return this.Connect(Server,110);
					}
					public void DisConnect()
					{
						try
						{
							this.local_status=Status.Local.DISCONNECT;
							this.CLIENT.GetStream().Close();
							this.CLIENT.Close();
							this.CLIENT=null;
							this.global_status=Status.Global.DISCONNECTED;
							this.local_status=Status.Local.NONE;
						}
						catch(System.Exception)
						{
						}
					}
					public void SendString(string String,bool Enter)
					{
						if(Enter)
						{
							this.SendByte(System.Text.Encoding.ASCII.GetBytes(String+System.Environment.NewLine));
						}
						else
						{
							this.SendByte(System.Text.Encoding.ASCII.GetBytes(String));
						}
					}
					public void SendByte(byte[] Bytes)
					{
						if(this.ACTIVE==true)
						{
							if(this.CLIENT.Connected)
							{
								this.CLIENT.GetStream().Write(Bytes,0,Bytes.Length);
							}
							else
							{
								throw new System.Exception("Client Disconnected");
							}
						}
						else
						{
							throw new System.Exception("Active Disable");
						}
					}
					public string ListenString()
					{
						return System.Text.Encoding.ASCII.GetString(this.ListenByte());
					}
					public byte[] ListenByte()
					{
						System.DateTime Start=System.DateTime.Now;
						do
						{
							if(this.ACTIVE==false)
							{
								throw new System.Exception("Active Disable");
							}
							if((System.DateTime.Now-Start).TotalMilliseconds>=this.RECEIVE_TIMEOUT)
							{
								throw new System.Exception("RECEIVE_TIMEOUT");
							}
							System.Threading.Thread.Sleep(this.RECEIVE_SLEEP);
						}
						while(this.CLIENT.Available==0);
						byte[] B=new byte[this.CLIENT.Available];
						int RealSize=this.CLIENT.GetStream().Read(B,0,B.Length);
						if(RealSize<B.Length)
						{
							System.Array.Resize<byte>(ref B,RealSize);
							System.Windows.Forms.MessageBox.Show("Client.ListenByte");
						}
						return B;
					}
					public string USER(string User)
					{
						this.local_status=Status.Local.USER;
						this.SendString("user "+User,true);
						string S=this.ListenString();
						if(Message.GetStatus(S)!=Message.Status.OK)
						{
							throw new System.Exception("Message Status not OK in Client.USER");
						}
						return S;
					}
					public string PASS(string Password)
					{
						this.local_status=Status.Local.PASS;
						this.SendString("pass "+Password,true);
						string S=this.ListenString();
						if(Message.GetStatus(S)!=Message.Status.OK)
						{
							throw new System.Exception("Message Status not OK in Client.PASS");
						}
						return S;
					}
					public string STAT()
					{
						this.local_status=Status.Local.STAT;
						this.SendString("stat",true);
						string S=this.ListenString();
						if(Message.GetStatus(S)!=Message.Status.OK)
						{
							throw new System.Exception("Message Status not OK in Client.STAT");
						}
						return S;
					}
					public string QUIT()
					{
						this.local_status=Status.Local.QUIT;
						this.SendString("quit",true);
						string S=this.ListenString();
						if(Message.GetStatus(S)!=Message.Status.OK)
						{
							throw new System.Exception("Message Status not OK in Client.QUIT");
						}
						return S;
					}
				}
				public class Manager
				{
					public class MailStatus
					{
						public string Log;
						public int OldMailCount;
						public int OldMailSize;
						public System.DateTime OldDT;
						public int NewMailCount;
						public int NewMailSize;
						public System.DateTime NewDT;
						public bool Valid()
						{
							return (this.OldDT!=System.DateTime.MinValue&&this.NewDT!=System.DateTime.MinValue);
						}
					}
					public string DATA=System.Windows.Forms.Application.StartupPath+@"\data.bin";
					public string LOG=System.Windows.Forms.Application.StartupPath+@"\log.txt";
					public event System.Action<int,int> ProgressChanged;
					public i.Net.Mail.POP.Client CLIENT;
					protected Manager(i.Net.Mail.POP.Client client,string data,string log)
					{
						this.CLIENT=client;
						this.DATA=data;
						this.LOG=log;
					}
					public Manager(int receive_timeout,string data,string log):this(new Client(receive_timeout),data,log)
					{
					}
					protected void Progress(int Progress,int Overall)
					{
						if(this.ProgressChanged!=null)
						{
							this.ProgressChanged(Progress,Overall);
						}
					}
					public string CheckMail(string Server,string User,string Password)
					{
						string Log=string.Empty;
						this.Progress(0,6);
						Log+=this.CLIENT.Connect(Server);
						this.Progress(1,6);
						Log+=this.CLIENT.USER(User);
						this.Progress(2,6);
						Log+=this.CLIENT.PASS(Password);
						this.Progress(3,6);
						Log+=this.CLIENT.STAT();
						this.Progress(4,6);
						Log+=this.CLIENT.QUIT();
						this.Progress(5,6);
						this.CLIENT.DisConnect();
						this.Progress(6,6);
						return Log;
					}
					public string CheckMail(string Server,string User,string Password,out int MailCount,out int MailSize)
					{
						string Log=this.CheckMail(Server,User,Password);
						var R1=Log.Split(new string[] { System.Environment.NewLine },System.StringSplitOptions.None);
						var R2=R1[3].Split(' ');
						MailCount=int.Parse(R2[1]);
						MailSize=int.Parse(R2[2]);
						return Log;
					}
					public void Save(System.IO.BinaryWriter BW,MailStatus MS)
					{
						try
						{
							BW.Write(MS.NewMailCount);
							BW.Write(MS.NewMailSize);
							BW.Write(MS.NewDT.ToBinary());
							BW.Close();
						}
						finally
						{
							BW.Close();
						}
					}
					public void Load(System.IO.BinaryReader BR,MailStatus MS)
					{
						try
						{
							MS.OldMailCount=BR.ReadInt32();
							MS.OldMailSize=BR.ReadInt32();
							MS.OldDT=System.DateTime.FromBinary(BR.ReadInt64());
							BR.Close();
						}
						finally
						{
							BR.Close();
						}
					}
					public void Log(System.DateTime DT,string Text)
					{
						System.IO.File.AppendAllText(this.LOG,System.Environment.NewLine+"LOG: "+DT.ToLongDateString()+" // "+DT.ToLongTimeString()+System.Environment.NewLine+Text);
					}
					public MailStatus Check(string Server,string User,string Password)
					{
						MailStatus MS=new MailStatus();
						MS.NewDT=System.DateTime.Now;
						string S=this.CheckMail(Server,User,Password,out MS.NewMailCount,out MS.NewMailSize);
						this.Log(System.DateTime.Now,S);
						System.IO.BinaryReader BR=null;
						BR=new System.IO.BinaryReader(System.IO.File.Open(this.DATA,System.IO.FileMode.OpenOrCreate));
						try
						{
							if(System.IO.File.Exists(this.DATA))
							{
								this.Load(BR,MS);
							}
							else
							{
								System.IO.File.Create(this.DATA);
							}
						}
						catch(System.Exception)
						{
						}
						this.Save(new System.IO.BinaryWriter(System.IO.File.Open(this.DATA,System.IO.FileMode.Create)),MS);
						return MS;
					}
				}
			}
		}
	}
}