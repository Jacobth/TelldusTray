using MetroFramework.Forms;
using System;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace TelldusTray
{
    public partial class MainForm : MetroForm
    {
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;

        public MainForm()
        {          
            InitializeComponent();

            this.BackColor = Color.FromArgb(64, 64, 64);
            this.Opacity = 0.85;
            this.ShowInTaskbar = false;
            this.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Size.Height * 0.7);
            this.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Size.Width * 0.3);
            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.Location = new Point(workingArea.Right - Size.Width, workingArea.Bottom - Size.Height);

            System.Diagnostics.Debug.Write("Height" + this.Height);
            System.Diagnostics.Debug.Write("Width" + this.Width);

            trayIcon = new NotifyIcon();
            trayIcon.Text = "Telldus";
            trayIcon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);

            this.Deactivate += new EventHandler(this.MainForm_Focus);
            this.Resize += new EventHandler(this.MainForm_Resize);
            this.trayIcon.MouseClick += new MouseEventHandler(this.NotifyIcon_MouseClick);
            this.FormClosing += new FormClosingEventHandler(this.MainForm_Resize);

            var list = Telldus.GetNames();
            metroGrid1.RowHeadersVisible = false;
            metroGrid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            metroGrid1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            metroGrid1.DataSource = list;

            Save.GetDevices();
            var saves = Save.GetNoIdDevices();

            metroGrid2.RowHeadersVisible = false;
            metroGrid2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            metroGrid2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            metroGrid2.DataSource = saves;

            metroGrid1.DoubleClick += new EventHandler(this.metroGrid1_DoubleClick);
            metroGrid2.DoubleClick += new EventHandler(this.metroGrid2_DoubleClick);

            this.WindowState = FormWindowState.Minimized;
        }

        private void metroGrid1_DoubleClick(object sender, EventArgs e)
        {
            string name = GetSelected();
            string action1 = "turnon";
            string action2 = "turnoff";
            int id = Telldus.GetId(name);

            Save.SaveDevice(name + " on", id, action1);
            Save.SaveDevice(name + " off", id, action2);

            Save.GetDevices();
            metroGrid2.DataSource = Save.GetNoIdDevices();
        }

        private void metroGrid2_DoubleClick(object sender, EventArgs e)
        {
            int index = metroGrid2.CurrentCell.RowIndex;

            if (index >= 0)
                Save.RemoveDevice(index);

            Save.GetDevices();
            metroGrid2.DataSource = Save.GetNoIdDevices();
        }

        private string GetSelected()
        {
            string str = metroGrid1.Rows[metroGrid1.SelectedRows[0].Index].Cells[0].Value.ToString();
            return str;
        }
  
        private void SetupTray()
        {
            trayMenu = new ContextMenu();

            var list = Save.GetDevices();

            foreach (var item in list)
            {
                trayMenu.MenuItems.Add(item.Name, OnMenuItem);
            }

            trayMenu.MenuItems.Add("-");
            trayMenu.MenuItems.Add("Open", OnOpen);
            trayMenu.MenuItems.Add("Exit", OnExit);
            trayIcon.ContextMenu = trayMenu;

            if (FormWindowState.Minimized == this.WindowState)
            {
                trayIcon.Visible = true;
                this.Hide();         
            }

            else if (FormWindowState.Normal == this.WindowState)
            {                                     
                trayIcon.Visible = false;
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Write("Lost focuss");
            SetupTray();
        }

        private void MainForm_Focus(object sender, EventArgs e)
        {
            trayMenu = new ContextMenu();

            var list = Save.GetDevices();

            foreach (var item in list)
            {
                trayMenu.MenuItems.Add(item.Name, OnMenuItem);
            }

            trayMenu.MenuItems.Add("-");
            trayMenu.MenuItems.Add("Open", OnOpen);
            trayMenu.MenuItems.Add("Exit", OnExit);
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;

            this.Hide();
            this.ShowInTaskbar = false;
        }

        private void OnOpen(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = false;
            trayIcon.Visible = false;
        }

        private void OnExit(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Write(sender.ToString());
            //System.Diagnostics.Debug.Write(());
            Application.Exit();
        }

        private void OnMenuItem(object sender, EventArgs e)
        {
            string[] arr = sender.ToString().Split(':');
            string name = arr[arr.Length - 1];
            string[] tmp = name.Split(' ');

            string deviceName = "";

            for (int i = 0; i < tmp.Length - 1; i++)
            {
                if(i != 0)
                {
                    deviceName += " " + tmp[i];
                }
                else
                {
                    deviceName += tmp[i];
                }
            }
            deviceName = deviceName.Substring(1, deviceName.Length - 1);

            string action = "turn" + tmp[tmp.Length - 1];
            int id = Telldus.GetId(deviceName);

            Telldus.DeviceAction(id, action);

            System.Diagnostics.Debug.Write(id);
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = false;
                this.FocusMe();
                trayIcon.Visible = false;
            }
        }
    }
}
