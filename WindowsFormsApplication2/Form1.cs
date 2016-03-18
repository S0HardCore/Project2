using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        #region Constants
        static readonly int
            MAX_OFFSET_X = 1000,
            MAX_OFFSET_Y = 1000,
            HEXAGON_WIDTH = 96,
            HEXAGON_HEIGHT = 84,
            HEXAGON_MARGIN = 4;
        #endregion

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
                            consoleLog = currentResolution.Width + "x" + currentResolution.Height + " done.";
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
                    case "RESOLUTION":
                    case "DEFAULTRESOLUTION":
                    case "DEFRESOLUTION":
                    case "DEFAULTRES":
                    case "RES":
                    case "DEFRES":
                        currentResolution.Width = 1920;
                        currentResolution.Height = 1080;
                        consoleLog = "1920x1080 done.";
                        break;
                    //case ""
                }
                consolePrevString = consoleString;
                if (consoleString.Length > 0)
                    consoleString = consoleString.Remove(0);
            }
        }

        #region Hexagon
        static readonly Point[]
            HexagonPosition = new Point[7]
            {
                new Point(100 + HEXAGON_WIDTH / 4, 200 + (HEXAGON_HEIGHT + HEXAGON_MARGIN )/ 2),
                new Point(200, 200),
                new Point(200 + 3 * HEXAGON_WIDTH / 4 + HEXAGON_MARGIN, 200 + (HEXAGON_HEIGHT + HEXAGON_MARGIN) / 2),
                new Point(100 + HEXAGON_WIDTH / 4, 200 + 3 * (HEXAGON_HEIGHT + HEXAGON_MARGIN) / 2),
                new Point(200, 200 + (HEXAGON_HEIGHT + HEXAGON_MARGIN) * 2),
                new Point(200 + 3 * HEXAGON_WIDTH / 4 + HEXAGON_MARGIN, 200 + 3 * (HEXAGON_HEIGHT + HEXAGON_MARGIN) / 2),
                new Point(200, 200 + HEXAGON_HEIGHT + 4)
            };
        Boolean[]
            HexagonHover = new Boolean[7],
            HexagonKeys = new Boolean[7];
        List<GraphicsPath> Hexagon = new List<GraphicsPath>()
            {
                new GraphicsPath(),
                new GraphicsPath(),
                new GraphicsPath(),
                new GraphicsPath(),
                new GraphicsPath(),
                new GraphicsPath(),
                new GraphicsPath()
            };
        Color[] HexagonColors = new Color[7]
            {
                Color.FromArgb(101, 176, 49),
                Color.FromArgb(255, 254, 52),
                Color.FromArgb(251, 153, 2),
                Color.FromArgb(3, 146, 206),
                Color.FromArgb(135, 1, 176),
                Color.FromArgb(254, 39, 19),
                Color.Gray
            };
        string[] HexagonText = new string[7] { "Q", "W", "E", "A", "S", "D", "Space" };
        string HexagonString = "";
        #endregion

        ConsolePrototype 
            Console = new ConsolePrototype();
        Font
            Verdana16 = new Font("Verdana", 16),
            Verdana13 = new Font("Verdana", 13),
            Verdana11 = new Font("Verdana", 11);
        Timer
            timer = new Timer();
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
            //Cursor.Hide();
            for (int a = 0; a < 7; ++a)
            {
                Hexagon[a].AddPolygon(BuildHexagon(HexagonPosition[a]));
            }
        }

        void pMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mDown = true;
                Point temp = ConvertClickPoint(e.X, e.Y);
                for (int q = 0; q < 7; ++q)
                {
                    HexagonHover[q] = Hexagon[q].IsVisible(temp);
                    if (HexagonHover[q])
                        if (q != 6)
                            HexagonString += HexagonText[q];
                        else
                            HexagonString = "";
                    }
            }

        }

        void pMouseUp(object sender, MouseEventArgs e)
        {
            mDown = false;
            Point temp = ConvertClickPoint(e.X, e.Y);
            for (int q = 0; q < 7; ++q)
            {
                HexagonHover[q] = false;
                //if (HexagonHover[q])
                //    if (q != 6)
                //        HexagonString += HexagonText[q];
                //    else
                //        HexagonString = "";
            }
        }

        void pMouseMove(object sender, MouseEventArgs e)
        {
            Point temp = ConvertClickPoint(e.X, e.Y);
            if (e.Button == MouseButtons.Left)
            //for (int q = 0; q < 7; ++q)
            //{
            //    HexagonHover[q] = Hexagon[q].IsVisible(temp);
            //    if (HexagonHover[q])
            //        if (q != 6)
            //            HexagonString += HexagonText[q];
            //        else
            //            HexagonString = "";
            //}

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
            /*if (e.X != mousePosition.X && e.Y != mousePosition.Y)
            {
                int qw = mousePosition.X - e.X,
                    we = mousePosition.Y - e.Y;
                qw = e.X - qw;
                we = e.Y - we;
                Cursor.Position = new Point(qw, we);
            }*/
            mousePosition.X = e.X;
            mousePosition.Y = e.Y;
        }

        Point ConvertClickPoint(int eX, int eY)
        {
            int a = (int)((eX - mouseOffset.X) / GetResolutionRatio().Width),
                b = (int)((eY - mouseOffset.Y) / GetResolutionRatio().Height);
            Point temp = new Point(a, b);
            return temp;
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
            else
            {
                for (int a = 0; a < 6; ++a)
                    if ((char)e.KeyValue == HexagonText[a][0])
                        HexagonHover[a] = true;
                if (e.KeyData == Keys.Space)
                    HexagonHover[6] = true;
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
                        if (!String.IsNullOrEmpty(Console.getString()))
                        {
                            string temp = Console.getString();
                            temp = temp.Remove(0);
                            Console.setString(temp);
                        }
                    }
                    break;
            }
            for (int a = 0; a < 6; ++a)
                if ((char)e.KeyValue == HexagonText[a][0])
                {
                    HexagonHover[a] = false;
                    HexagonString += (char)e.KeyValue;
                }
            if (e.KeyData == Keys.Space)
            {
                HexagonHover[6] = false;
                HexagonString = "";
            }
        }

        void pUpdate(object sender, EventArgs e)
        {
            //if (!mDown)
                //Cursor.Position = new Point(960, 540);
            //mouseOffset.X +=2;
            Invalidate();
        }

        SizeF GetResolutionRatio()
        {
            float a = (float)(currentResolution.Width / 1920f);
            float b = (float)(currentResolution.Height / 1080f);
            return new SizeF(a, b);
        }

        Point[] BuildHexagon(Point start)
        {
            int width = HEXAGON_WIDTH, height = HEXAGON_HEIGHT;
            Point[] HexPoints = new Point[6];
            HexPoints[0] = new Point(start.X + width / 4, start.Y);
            HexPoints[1] = new Point(start.X + 3 * width / 4, start.Y);
            HexPoints[2] = new Point(start.X + width, start.Y + height / 2);
            HexPoints[3] = new Point(start.X + 3 * width / 4, start.Y + height);
            HexPoints[4] = new Point(start.X + width / 4, start.Y + height);
            HexPoints[5] = new Point(start.X, start.Y + height / 2);
            return HexPoints;
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
            g.ScaleTransform(GetResolutionRatio().Width, GetResolutionRatio().Height);
            //if (this.Size.Width < currentResolution.Width || this.Size.Height < currentResolution.Height)
                //this.Size = new Size(currentResolution.Width, currentResolution.Height);
            #endregion

            Point[] TP = new Point[] { new Point (960, -20), new Point (-20, 1100), new Point (1940, 1100) };
            g.FillPolygon(Brushes.CornflowerBlue, TP);
            g.DrawRectangle(Pens.DarkSlateBlue, -200, -400, 300, 700);
            

            g.DrawPolygon(Pens.Black, TP);

            for (int a = 0; a < 7; ++a)
            {
                if (HexagonHover[a])
                    g.FillPath(new SolidBrush(HexagonColors[a]), Hexagon[a]);
                else
                    g.DrawPath(Pens.Black, Hexagon[a]);
                Point TPS = HexagonPosition[a];
                TPS.Offset(36 - (a == 6 ? 22 : a == 1 ? 2 : 0), 30);
                g.DrawString(HexagonText[a], Verdana16, Brushes.Black, TPS);
            }
            g.ResetTransform();

            g.DrawString("Cursor Position:\n" + mousePosition.ToString() + "\nOffset: \n" + mouseOffset.ToString() + "\nConsole String:" + Console.getString() + "\nHexagon String:" + HexagonString + "\nMonitor Resolution:\n" + Screen.PrimaryScreen.Bounds.Size.ToString() + "\nApp Resolution:\n" + currentResolution.ToString(), Verdana11, Brushes.Black, 0, 0);

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
