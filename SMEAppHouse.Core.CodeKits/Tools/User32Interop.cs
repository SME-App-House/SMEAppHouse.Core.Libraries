using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace SMEAppHouse.Core.CodeKits.Tools
{
    public static class User32Interop
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static TimeSpan GetLastInput()
        {
            var plii = new LASTINPUTINFO();
            plii.cbSize = (uint)Marshal.SizeOf(plii);

            if (GetLastInputInfo(ref plii))
                return TimeSpan.FromMilliseconds(Environment.TickCount - plii.dwTime);
            else
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }
    }

    ///
    /// Usage:
    /*
     * public partial class MyForm : Form
{
  Timer activityTimer = new Timer();
  TimeSpan activityThreshold = TimeSpan.FromMinutes(2);
  bool cursorHidden = false;

  public Form1()
  {
    InitializeComponent();

    activityTimer.Tick += activityWorker_Tick;
    activityTimer.Interval = 100;
    activityTimer.Enabled = true;
  }

  void activityWorker_Tick(object sender, EventArgs e)
  {
    bool shouldHide = User32Interop.GetLastInput() > activityThreshold;
    if (cursorHidden != shouldHide)
    {
      if (shouldHide)
        Cursor.Hide();
      else
        Cursor.Show();

      cursorHidden = shouldHide;
    }
  }
}
     */
}
