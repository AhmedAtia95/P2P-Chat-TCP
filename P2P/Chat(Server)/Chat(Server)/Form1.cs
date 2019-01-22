using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat_Server_
{
    public partial class FormServer : Form
    {
        public FormServer()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            threadReceive = new Thread(new ThreadStart(ReceivedByServer));
            threadReceive.Start();
        }
        //============================================================Receive================================================================================
        Thread threadReceive;
        void ReceivedByServer()
        {
            //hn3ml object socketReceive yst2bl bl TCP
            Socket socketReceive = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            int portReceive = 4995;             //El port elle hyst2bl 3leh
            IPEndPoint iPEndPointReceive = new IPEndPoint(IPAddress.Parse("127.0.0.1"), portReceive);
            socketReceive.Bind(iPEndPointReceive);
            socketReceive.Listen(10);      //Listen 3shan ysm3 el 7aga elle gaya w yst2blha
            while (true)
            {
                Socket temp = null;
                try
                {
                    //byst2bl el msg elle gaya
                    temp = socketReceive.Accept();
                    byte[] messageReceivedByServer = new byte[100];
                    int sizeOfReceivedMessage = temp.Receive(messageReceivedByServer, SocketFlags.None);
                    string str = Encoding.ASCII.GetString(messageReceivedByServer);
                    labelShow.Text += "\r\nClient: " + str;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + ex.StackTrace + "\n" + ex.HelpLink + "\n" + ex.InnerException
                            + "\n" + ex.Source + "\n" + ex.TargetSite);
                }
                finally
                {
                    temp.Close();
                }
            }
        }
        //================================================================================================================================================
        private void buttonSend_Click(object sender, EventArgs e)
        {
        }

        private void buttonSend_Click_1(object sender, EventArgs e)
        {
            int portSend = 1411;   //El port elle hyb3t 3lih
            IPEndPoint iPEndPointSend = new IPEndPoint(IPAddress.Parse("127.0.0.1"), portSend);
            Socket socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string messageTextBox = textBoxMessage.Text;
            byte[] messageSentFromServer;
            try
            {
                socketSend.Connect(iPEndPointSend);
                messageSentFromServer = Encoding.ASCII.GetBytes(messageTextBox);
                socketSend.Send(messageSentFromServer, SocketFlags.None);
                labelShow.Text += "\r\nServer: " + messageTextBox;
                textBoxMessage.Text = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace + "\n" + ex.HelpLink + "\n" + ex.InnerException
                        + "\n" + ex.Source + "\n" + ex.TargetSite);
            }
            finally
            {
                socketSend.Close();
            }
        }

        private void textBoxMessage_TextChanged(object sender, EventArgs e)
        {
            this.buttonSend.Enabled = !string.IsNullOrWhiteSpace(this.textBoxMessage.Text);
        }
        //============================================================Receive================================================================================
    }
}