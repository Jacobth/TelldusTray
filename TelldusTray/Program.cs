using System;
using System.Drawing;
using System.Windows.Forms;

namespace TelldusTray
{
    static class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.Run(new MainForm());
        }

    }
}
