using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Test461
{
    public partial class Form1 : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
    (
        int nLeftRect, // x-coordinate of upper-left corner
        int nTopRect, // y-coordinate of upper-left corner
        int nRightRect, // x-coordinate of lower-right corner
        int nBottomRect, // y-coordinate of lower-right corner
        int nWidthEllipse, // height of ellipse
        int nHeightEllipse // width of ellipse
     );

        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

        private bool m_aeroEnabled;                     // variables for box shadow
        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;

        public struct MARGINS                           // struct for box shadow
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }

        private const int WM_NCHITTEST = 0x84;          // variables for dragging the form
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();

                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW;

                return cp;
            }
        }

        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0;
                DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:                        // box shadow
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 1,
                            rightWidth = 1,
                            topHeight = 1
                        };
                        DwmExtendFrameIntoClientArea(this.Handle, ref margins);

                    }
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);

            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT)     // drag the form
                m.Result = (IntPtr)HTCAPTION;

        }


        private NoFocusCuesButton Button1;
        private NoFocusCuesButton Button2;

        public Form1()
        {
            InitializeComponent();

            Button1 = new NoFocusCuesButton();
            Button1.TabIndex = 0;
            Button1.TabStop = true;
            Button1.Location = new Point(50, 10);
            Button1.Size = new Size(100, 50);
            Button1.FlatStyle = FlatStyle.Flat;
            Button1.BackColor = Color.FromArgb(200, 200, 200);
            Button1.FlatAppearance.MouseOverBackColor = Button1.BackColor;
            Button1.FlatAppearance.MouseDownBackColor = Color.FromArgb(170, 170, 170);
            Button1.FlatAppearance.BorderSize = 0;
            Button1.Font = new Font("Calibri", 14);
            Button1.ForeColor = Color.Black;
            Button1.Text = "Button1";
            Button1.TextAlign = ContentAlignment.MiddleCenter;
            Button1.GotFocus += Button_GotFocus;
            Button1.MouseEnter += Button_MouseEnter;
            Button1.MouseDown += Button_MouseDown;
            Button1.MouseUp += Button_MouseUp;
            Button1.MouseLeave += Button_MouseLeave;
            Button1.LostFocus += Button_LostFocus;

            Button2 = new NoFocusCuesButton();
            Button2.TabIndex = 1;
            Button2.TabStop = true;
            Button2.Location = new Point(50, 70);
            Button2.Size = new Size(100, 50);
            Button2.FlatStyle = FlatStyle.Flat;
            Button2.BackColor = Color.FromArgb(200, 200, 200);
            Button2.FlatAppearance.MouseOverBackColor = Button2.BackColor;
            Button2.FlatAppearance.MouseDownBackColor = Color.FromArgb(170, 170, 170);
            Button2.FlatAppearance.BorderSize = 0;
            Button2.Font = new Font("Calibri", 14);
            Button2.ForeColor = Color.Black;
            Button2.Text = "Button2";
            Button2.TextAlign = ContentAlignment.MiddleCenter;
            Button2.GotFocus += Button_GotFocus;
            Button2.MouseEnter += Button_MouseEnter;
            Button2.MouseDown += Button_MouseDown;
            Button2.MouseUp += Button_MouseUp;
            Button2.MouseLeave += Button_MouseLeave;
            Button2.LostFocus += Button_LostFocus;

            this.Controls.Add(Button1);
            this.Controls.Add(Button2);
        }

        private void Button_GotFocus(object sender, EventArgs e)
        {
            Button Sender = sender as Button;
            for (int i = 0; i < 6; i++)
            {
                Thread.Sleep(1);
                Sender.FlatAppearance.MouseOverBackColor = Color.FromArgb(Sender.BackColor.R - 5, Sender.BackColor.G - 5, Sender.BackColor.B - 5);
                Sender.BackColor = Sender.FlatAppearance.MouseOverBackColor;
                Sender.Invalidate();
            }
        }

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            Button Sender = sender as Button;
            for (int i = 0; i < 6; i++)
            {
                Thread.Sleep(1);
                Sender.Invalidate();
            }
        }

        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            Button Sender = sender as Button;
            for (int i = 0; i < 6; i++)
            {
                Thread.Sleep(1);
                Sender.FlatAppearance.MouseDownBackColor = Color.FromArgb(Sender.FlatAppearance.MouseOverBackColor.R - 5, Sender.FlatAppearance.MouseOverBackColor.G - 5, Sender.FlatAppearance.MouseOverBackColor.B - 5);
                Sender.FlatAppearance.MouseOverBackColor = Sender.FlatAppearance.MouseDownBackColor;
                Sender.Invalidate();
            }
        }

        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            Button Sender = sender as Button;
            for (int i = 0; i < 6; i++)
            {
                Thread.Sleep(1);
                Sender.FlatAppearance.MouseOverBackColor = Color.FromArgb(Sender.FlatAppearance.MouseDownBackColor.R + 5, Sender.FlatAppearance.MouseDownBackColor.G + 5, Sender.FlatAppearance.MouseDownBackColor.B + 5);
                Sender.FlatAppearance.MouseDownBackColor = Sender.FlatAppearance.MouseOverBackColor;
                Sender.Invalidate();
            }
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            Button Sender = sender as Button;
            for (int i = 0; i < 6; i++)
            {
                Thread.Sleep(1);
                Sender.BackColor = Color.FromArgb(Sender.FlatAppearance.MouseOverBackColor.R + 5, Sender.FlatAppearance.MouseOverBackColor.G + 5, Sender.FlatAppearance.MouseOverBackColor.B + 5);
                Sender.FlatAppearance.MouseOverBackColor = Sender.BackColor;
                Sender.Invalidate();
            }
        }

        private void Button_LostFocus(object sender, EventArgs e)
        {
            Button Sender = sender as Button;
            for (int i = 0; i < 6; i++)
            {
                Thread.Sleep(1);
                Sender.BackColor = Color.FromArgb(Sender.FlatAppearance.MouseOverBackColor.R + 5, Sender.FlatAppearance.MouseOverBackColor.G + 5, Sender.FlatAppearance.MouseOverBackColor.B + 5);
                Sender.FlatAppearance.MouseOverBackColor = Sender.BackColor;
                Sender.Invalidate();
            }
        }
    }
}
