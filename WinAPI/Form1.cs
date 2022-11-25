using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Management;
using Microsoft.VisualBasic;

namespace TaskManager
{
    public partial class Form1 : Form
    {
        private List<Process> processes = null;
        public Form1()
        {
            InitializeComponent();
        }
        private void GetProcesses()
        {
            processes.Clear();
            processes = Process.GetProcesses().ToList<Process>();
        }
        private void RefreshProcessesList()
        {
            listView1.Items.Clear();
            double memSize = 0;
            foreach (Process p in processes)
            {
                memSize = 0;
                PerformanceCounter pc = new PerformanceCounter();
                pc.CategoryName = "Process";
                pc.CounterName = "Working Set - Private";
                pc.InstanceName = p.ProcessName;
                memSize = (double)pc.NextValue() / (1000 * 1000);
                string[] row = new string[] { p.ProcessName.ToString(), Math.Round(memSize, 1).ToString() };
                listView1.Items.Add(new ListViewItem(row));
                pc.Close();
                pc.Dispose();
            }
            Text = "Запущено процессов:" + processes.Count.ToString();
        }
        private void RefreshProcessesList(List<Process> processes, string keyword)
        {
            listView1.Items.Clear();
            double memSize = 0;
            foreach (Process p in processes)
            {
                memSize = 0;
                PerformanceCounter pc = new PerformanceCounter();
                pc.CategoryName = "Process";
                pc.CounterName = "Working Set - Private";
                pc.InstanceName = p.ProcessName;
                memSize = (double)pc.NextValue() / (1000 * 1000);
                string[] row = new string[] { p.ProcessName.ToString(), Math.Round(memSize, 1).ToString() };
                listView1.Items.Add(new ListViewItem(row));
                pc.Close();
                pc.Dispose();
            }
            Text = "Запущено процессов" + keyword + ":" + processes.Count.ToString();
        }
        private void KillProcess(Process process)
        {
            process.Kill();
            process.WaitForExit();
        }
        private void KillProcessAndChildren(int pid)
        {
            if (pid == 0)
            {
                return;
            }
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                "Select * FromWin32_Process Where ParentProcessID" + pid);
            ManagementObjectCollection objectCollection = searcher.Get();
            foreach (ManagementObject obj in objectCollection)
            {
                KillProcessAndChildren(Convert.ToInt32(obj["ProcessID"]));

            }
            try
            {
                Process p = Process.GetProcessById(pid);
                p.Kill();
                p.WaitForExit();
            }
            catch (ArgumentException) { }

        }

        private int GetParentProcessID(Process p)
        {
            int parentID = 0;
            try
            {
                ManagementObject managementObject = new ManagementObject("win32_process.handle'" + p.Id + "'");
                managementObject.Get();
                parentID = Convert.ToInt32(managementObject["ParentProcessId"]);

            }
            catch (Exception)
            {

            }
            return parentID;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            processes = new List<Process>();
            GetProcesses();
            RefreshProcessesList();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            GetProcesses();
            RefreshProcessesList();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems[0] != null)
                {
                    Process processToKill = processes.Where((x) => x.ProcessName == listView1.SelectedItems[0].SubItems[0].Text).ToList()[0];
                    KillProcess(processToKill);
                    GetProcesses();
                    RefreshProcessesList();
                }

            }
            catch (Exception) { }


        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems[0] != null)
                {
                    Process processToKill = processes.Where((x) => x.ProcessName == listView1.SelectedItems[0].SubItems[0].Text).ToList()[0];
                    KillProcessAndChildren(GetParentProcessID(processToKill));
                    GetProcesses();
                    RefreshProcessesList();
                }

            }
            catch (Exception) { }

        }

        private void завершитьДеревоПроцессовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems[0] != null)
                {
                    Process processToKill = processes.Where((x) => x.ProcessName == listView1.SelectedItems[0].SubItems[0].Text).ToList()[0];
                    KillProcessAndChildren(GetParentProcessID(processToKill));
                    GetProcesses();
                    RefreshProcessesList();
                }

            }
            catch (Exception) { }

        }

        private void запуститьЗадачуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Interaction.InputBox("Введите имя программы", "Запуск новой задачи");
            try
            {
                Process.Start(path);
            }
            catch (Exception) { }
        }
    }
}
