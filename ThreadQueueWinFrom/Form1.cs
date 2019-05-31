using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreadQueueWinFrom
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        public delegate void InvokeMsg0(DownLoadFile x);
        public void ShowOneStartMsg(DownLoadFile x)
        {
            if (this.tb5.InvokeRequired)
            {
                InvokeMsg0 msgCallback = new InvokeMsg0(ShowOneStartMsg);
                tb5.Invoke(msgCallback, new object[] { x });
            }
            else
            {

                tb5.Text += x.FileName + " begin!" + Environment.NewLine;
            }
        }



        public delegate void InvokeMsg2(CompetedEventArgs args);
        public void ShowAllDoneMsg(CompetedEventArgs args)
        {
            if (this.tb5.InvokeRequired)
            {
                InvokeMsg2 msgCallback = new InvokeMsg2(ShowAllDoneMsg);
                tb5.Invoke(msgCallback, new object[] { args });
            }
            else
            {
                tb5.Text += "完成率：" + Convert.ToString(args.CompetedPrecent) + "%  All Job finished!" + Environment.NewLine;
            }
        }



        public delegate void InvokeMsg1(DownLoadFile x, CompetedEventArgs args);
        public void ShowOneDoneMsg(DownLoadFile x, CompetedEventArgs args)
        {
            if (this.tb5.InvokeRequired)
            {
                InvokeMsg1 msgCallback = new InvokeMsg1(ShowOneDoneMsg);
                tb5.Invoke(msgCallback, new object[] { x, args });
            }
            else
            {

                tb5.Text += x.FileName + " finished!" + "  完成率：" + Convert.ToString(args.CompetedPrecent) + "%  " + Environment.NewLine;
            }
        }



        private void button1_Click_1(object sender, EventArgs e)
        {
            DownLoadFile fd1 = new DownLoadFile();
            fd1.FileName = "myfile.txt";
            DownLoadFile fd2 = new DownLoadFile();
            fd2.FileName = "myfile2.txt";
            DownLoadFile fd3 = new DownLoadFile();
            fd3.FileName = "myfile3.txt";

            DownLoadFile fd4 = new DownLoadFile();
            fd4.FileName = "myfile4.txt";
            DownLoadFile fd5 = new DownLoadFile();
            fd5.FileName = "myfile5.txt";


            List<DownLoadFile> Quefd = new List<DownLoadFile>();
            Quefd.Add(fd1);
            //Quefd.Add(fd2);
            //Quefd.Add(fd3);
            //Quefd.Add(fd4);
            //Quefd.Add(fd5);
            QueueThreadBase thfd = new QueueThreadBase(Quefd);
            thfd.OneJobStart += ShowOneStartMsg;
            thfd.OneCompleted += ShowOneDoneMsg;
            thfd.AllCompleted += ShowAllDoneMsg;
            thfd.Start();
        }
    }
}
