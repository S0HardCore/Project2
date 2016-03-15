using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        class ConsolePrototype
        {
            public Boolean Enabled;

            static private string consoleString;
            static private string consolePrevString;
            static private string consoleLog;

            static private readonly Rectangle CONSOLE_REGION = new Rectangle (700, 990, 520, 50);

            public string getString() {  return consoleString; }

            public string getPrevString() {  return consolePrevString; }

            public string getLog() { return consoleLog; }

            public int getLength() { return consoleString.Length; }

            public void setString(string String) { consoleString = String; }

            public void setPrevString(string String) { consolePrevString = String; }

            public void setLog(string String) { consoleLog = String; }

            public Rectangle getRegion() { return CONSOLE_REGION; }

            public void applyCommand()
            {
                consoleString = consoleString.Trim();
                if (!String.IsNullOrEmpty(consoleString))
                    if (consoleString.Length > 8)
                        if (consoleString[0].Equals('W') && consoleString[5].Equals('H'))
                        {
                            string TS1, TS2;
                            int TI1 = currentResolution.Width, TI2 = currentResolution.Height;
                            TS1 = consoleString.Substring(1, 4);
                            TS2 = consoleString.Substring(6);
                            Int32.TryParse(TS1, out TI1);
                            Int32.TryParse(TS2, out TI2);
                            currentResolution.Width = TI1;
                            currentResolution.Height = TI1;
                            if (currentResolution.Width > 1920)
                                currentResolution.Width = 1920;
                            else
                                if (currentResolution.Width < 1000)
                                    currentResolution.Width = 1000;
                            if (currentResolution.Height > 1080)
                                currentResolution.Height = 1080;
                            else
                                if (currentResolution.Height < 600)
                                    currentResolution.Height = 600;
                            consoleLog = currentResolution.Width + ";" + currentResolution.Height + " done.";
                        }
                        else consoleLog = "Error.";
                    else consoleLog = "Error.";

                switch (consoleString)
                {
                    case "CLEAR":
                    case "RESET":
                        mouseOffset = new Point(0, 0);
                        consoleLog = "Reset done.";
                        break;
                    case "RES":
                    case "DEFRES":
                        currentResolution.Width = 1920;
                        currentResolution.Height = 1080;
                        consoleLog = "1920;1080 done.";
                        break;
                    //case ""
                }
                consolePrevString = consoleString;
                if (consoleString.Length > 0)
                    consoleString = consoleString.Remove(0);
            }
        }

        ConsolePrototype 
            Console = new ConsolePrototype();
        Font
            Verdana13 = new Font("Verdana", 13),
            Verdana11 = new Font("Verdana", 11);
        Timer
            timer = new Timer();
        static readonly int
            MAX_OFFSET_X = 1000,
            MAX_OFFSET_Y = 1000;
        static Point
            mouseOffset = new Point(0, 0),
            mousePosition = new Point(960, 540);
        static Size
            currentResolution = new Size(1920, 1080);
        Boolean
            mDown;

        public Form1()
        {
            InitializeComponent();
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pMouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pMouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pMouseUp);
            this.Size = new Size(1920, 1080);
            Cursor.Position = new Point(mousePosition.X, mousePosition.Y);
            this.Paint += new PaintEventHandler(pDraw);
            this.KeyDown += new KeyEventHandler(pKeyDown);
            this.KeyUp += new KeyEventHandler(pKeyUp);
            timer.Interval = 1;
            timer.Tick += new EventHandler(pUpdate);
            timer.Start();
            Cursor.Hide();
        }

        void pMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                mDown = true;
        }

        void pMouseUp(object sender, MouseEventArgs e)
        {
            mDown = false;
        }

        void pMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (mouseOffset.X > -MAX_OFFSET_X)
                    mouseOffset.X += e.X - mousePosition.X;
                else
                    mouseOffset.X = -MAX_OFFSET_X;
                if (mouseOffset.X < MAX_OFFSET_X)
                    mouseOffset.X += e.X - mousePosition.X;
                else
                    mouseOffset.X = MAX_OFFSET_X;
                if (mouseOffset.Y > -MAX_OFFSET_Y)
                    mouseOffset.Y += e.Y - mousePosition.Y;
                else
                    mouseOffset.Y = -MAX_OFFSET_Y;
                if (mouseOffset.Y < MAX_OFFSET_Y)
                    mouseOffset.Y += e.Y - mousePosition.Y;
                else
                    mouseOffset.Y = MAX_OFFSET_Y;
            }
            mousePosition.X = e.X;
            mousePosition.Y = e.Y;
        }

        void pKeyDown(object sender, KeyEventArgs e)
        {
            if (Console.Enabled)
            {
                if ((e.KeyData >= Keys.A && e.KeyData <= Keys.Z) ||
                    (e.KeyData >= Keys.D0 && e.KeyData <= Keys.D9))
                {
                    string temp = Console.getString();
                    temp += (char)e.KeyValue;
                    Console.setString(temp);
                }
                switch (e.KeyData)
                {
                    case Keys.Back:
                        if (Console.getLength() > 0)
                        {
                            string temp = Console.getString();
                            int tempint = Console.getLength() - 1;
                            temp = temp.Substring(0, tempint);
                            Console.setString(temp);
                        }
                        break;
                    case Keys.Enter:
                        if (!String.IsNullOrEmpty(Console.getString()))
                            Console.applyCommand();
                        break;
                }
            }
        }



        void pKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    Application.Exit();
                    break;
                case Keys.Tab:
                    if (!Console.Enabled)
                        Console.Enabled = true;
                    else
                    {
                        Console.Enabled = false;
                        if (Console.getLength() > 0)
                        {
                            string temp = Console.getString();
                            temp = temp.Remove(0);
                            Console.setString(temp);
                        }
                    }
                    break;
            }
        }

        void pUpdate(object sender, EventArgs e)
        {
            if (!mDown)
                Cursor.Position = new Point(960, 540);
            Invalidate();
        }

        Boolean TriangleContainsPoint(Point[] TP, Point PP)
        {
            Point P1 = TP[0], P2 = TP[1], P3 = TP[2];
            int a = (P1.X - PP.X) * (P2.Y - P1.Y) - (P2.X - P1.X) * (P1.Y - PP.Y);
            int b = (P2.X - PP.X) * (P3.Y - P2.Y) - (P3.X - P2.X) * (P2.Y - PP.Y);
            int c = (P3.X - PP.X) * (P1.Y - P3.Y) - (P1.X - P3.X) * (P3.Y - PP.Y);

            if ((a >= 0 && b >= 0 && c >= 0) || (a <= 0 && b <= 0 && c <= 0))
                return true;
            else
                return false;
        }

        void pDraw(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            #region Resolution
            g.TranslateTransform(mouseOffset.X, mouseOffset.Y);
            float a = (float)(currentResolution.Width / 1920f);
            float b = (float)(currentResolution.Height / 1080f);
            g.ScaleTransform(a, b);
            //if (this.Size.Width < currentResolution.Width || this.Size.Height < currentResolution.Height)
                //this.Size = new Size(currentResolution.Width, currentResolution.Height);
            #endregion

            Point[] TP = new Point[] { new Point (960, -20), new Point (-20, 1100), new Point (1940, 1100) };
            g.FillPolygon(Brushes.CornflowerBlue, TP);
            g.DrawRectangle(Pens.DarkSlateBlue, -200, -400, 300, 700);
            g.ResetTransform();

            g.DrawString("CurPos:\n" + mousePosition.ToString() + "\nOffset: \n" + mouseOffset.ToString() + "\nString:" + Console.getString() + "\nMonitorRes:\n" + Screen.PrimaryScreen.Bounds.Size.ToString() + "\nCurRes:\n" + currentResolution.ToString(), Verdana11, Brushes.Black, 0, 0);

            if (Console.Enabled)
            #region Console
            {
                g.FillRectangle(Brushes.DimGray, Console.getRegion());
                g.DrawRectangle(Pens.Black, Console.getRegion());
                g.DrawLine(Pens.Black, new Point(700, 1011), new Point(1220, 1011));
                g.DrawString("Console: ", Verdana13, Brushes.Black, 700, 1015);
                g.DrawString(Console.getLog(), Verdana13, Brushes.Black, Console.getRegion().Location);
                StringFormat format1 = new StringFormat(StringFormatFlags.NoClip);
                format1.Alignment = StringAlignment.Center;
                g.DrawString(Console.getPrevString(), Verdana13, Brushes.Black, new RectangleF(800, 990, 420, 20), format1);
                g.DrawString(Console.getString(), Verdana13, Brushes.Black, 775, 1015);
            }
            #endregion
        }
    }
}
