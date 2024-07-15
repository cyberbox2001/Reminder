using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;


namespace Reminder
{
    public partial class MainFrm : Form
    {
        WorkFrm wrkFrm;
        IConfigurationBuilder builder;
        IConfigurationRoot root;
        public MainFrm()
        {
            InitializeComponent();
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            builder = new ConfigurationBuilder().AddJsonFile("jsconfig.json", false, true);
            root = builder.Build();
            numWrkTime.Value = decimal.Parse(root["numWrkTime"]);
            numRstTime.Value = decimal.Parse(root["numRstTime"]);
            ckBoxInput.Checked = bool.Parse(root["ckBoxInput"]);
        }
       
        public void doStart()
        {
            try
            {
                bool input_flag;

                if (this.ckBoxInput.Checked)
                {
                    input_flag = true;
                }
                else
                {
                    input_flag = false;
                }

                int wrkTime = (int)this.numWrkTime.Value;
                int rstTime = (int)this.numRstTime.Value;
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm is WorkFrm)
                    {
                        frm.Close();

                    }
                }
                wrkFrm = new WorkFrm(wrkTime, rstTime, input_flag);
                var appSetDTO = new appSetDTO();
                appSetDTO.numWrkTime = numWrkTime.Value.ToString();
                appSetDTO.numRstTime = numRstTime.Value.ToString();
                appSetDTO.ckBoxInput = ckBoxInput.Checked.ToString();

                File.WriteAllText("jsconfig.json", JsonConvert.SerializeObject(appSetDTO));

                wrkFrm.Show();
                //MainFrm.Visible = false;
                this.Visible = false;
            }
            catch  
            {

                 
            }
            
        }
        private void Btn_start_Click(object sender, EventArgs e)
        {
            doStart();
        }

        private void 主窗体ToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            try
            {
                wrkFrm.Close();
            }
            catch 
            {
            }
        }

        private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {            
            //取消关闭窗口
            e.Cancel = true;
            //最小化主窗口
            this.WindowState = FormWindowState.Minimized;
            this.Visible = false;
            //不在系统任务栏显示主窗口图标
            this.ShowInTaskbar = false;
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            System.Environment.Exit(0);
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }
        public class appSetDTO
        {
            public string numWrkTime { set; get; }
            public string numRstTime { set; get; }
            public string ckBoxInput { set; get; }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is RestFrm)
                {
                    frm.Close();
                    
                }
            }
            RestFrm restFrm = new RestFrm(int.Parse(numRstTime.Value.ToString()), int.Parse(numWrkTime.Value.ToString()), ckBoxInput.Checked);
            restFrm.ShowDialog();
        }
    }
}
