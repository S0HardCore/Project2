﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        #region Constants
        enum ZombieTypes
        {
            NORMAL = 0,
            BIG = 1,
            HUGE = 2,
        }
        enum ZombieActions
        {
            WALK = 0,
            STAND = 1,
            ATTACK = 2,
        }
        enum WeaponType
        {
            CURSOR = 0,
            SNIPER_RIFLE = 1,
            EXPLOSION = 2,
            LIGHTNING = 3,
            FIRE = 4,
            ICE = 5,
        }
        const int
            MAX_OFFSET_X = 1000,
            MAX_OFFSET_Y = 1000,
            HEXAGON_WIDTH = 96,
            HEXAGON_HEIGHT = 84,
            HEXAGON_MARGIN = 4,
            ZOMBIE_WIDTH = 81,
            ZOMBIE_HEIGHT = 62,
            BIG_ZOMBIE_WIDTH = 147,
            BIG_ZOMBIE_HEIGHT = 192,
            HUGE_ZOMBIE_WIDTH = 242,
            HUGE_ZOMBIE_HEIGHT = 206,
            EXPLOSION_SIZE = 96,
            EXPLOSION_FRAMES = 15,
            LIGHTNING_FRAMES = 7,
            LIGHTNING_WIDTH = 64,
            LIGHTNING_HEIGHT = 256,
            WEAPONS_COUNT = 6;
        static readonly int[]
            SPELL_COOLDOWNS = new int[WEAPONS_COUNT] { 0, 20, 60, 45, 30, 30 };
        static readonly String[]
            DI_WEAPON_NAMES = new String[WEAPONS_COUNT]
            {
                "None",
                "Sniper Rifle",
                "Explosion",
                "Lightning",
                "Fire",
                "Ice"
            };
        readonly string[] HexagonText = new string[7] { "Q", "W", "E", "A", "S", "D", "Space" };
        static readonly Size
            ZOMBIE_SIZE = new Size(ZOMBIE_WIDTH, ZOMBIE_HEIGHT),
            BIG_ZOMBIE_SIZE = new Size(BIG_ZOMBIE_WIDTH, BIG_ZOMBIE_HEIGHT),
            HUGE_ZOMBIE_SIZE = new Size(HUGE_ZOMBIE_WIDTH, HUGE_ZOMBIE_HEIGHT),
            LIGHTNING_SIZE = new Size(LIGHTNING_WIDTH, LIGHTNING_HEIGHT);
        static readonly Font
            Verdana16 = new Font("Verdana", 16),
            Verdana13 = new Font("Verdana", 13),
            Verdana11 = new Font("Verdana", 11),
            Verdana9 = new Font("Verdana", 9);
        readonly Rectangle SPELL_SLOT_RECTANGLE = new Rectangle(650, 930, 100, 100);
        static Bitmap
            iZombie = Properties.Resources.zombie,
            iZombieIdle = Properties.Resources.zombie_idle,
            iBigZombie = Properties.Resources.big_zombie,
            iBigZombieIdle = Properties.Resources.big_zombie_idle,
            iHugeZombie = Properties.Resources.huge_zombie,
            iHugeZombieIdle = Properties.Resources.huge_zombie_idle,
            iCursor = Properties.Resources.cursor,
            iCrossHair = Properties.Resources.crosshair,
            iSniperRifle = Properties.Resources.sniper_rifle,
            iLightningEffect = Properties.Resources.lightning,
            iExplosionEffect = Properties.Resources.explosion,
            iLightningIcon = Properties.Resources.lightning_icon,
            iExplosionIcon = Properties.Resources.explosion_icon;
        static readonly Color[] HexagonColors = new Color[7]
            {
                Color.FromArgb(101, 176, 49),
                Color.FromArgb(255, 254, 52),
                Color.FromArgb(251, 153, 2),
                Color.FromArgb(3, 146, 206),
                Color.FromArgb(135, 1, 176),
                Color.FromArgb(254, 39, 19),
                Color.DarkGray
            };
        static readonly Color[]
            HealthColor = new Color[]
            {
                Color.DarkGreen,
                Color.Green,
                Color.YellowGreen,
                Color.Orange,
                Color.Red
            };
        static readonly StringFormat
            TextFormatCenter = new StringFormat(StringFormatFlags.NoClip);
        readonly static Point[]
            HexagonPosition = new Point[7]
            {
                new Point(810 + HEXAGON_WIDTH / 4, 780 + (HEXAGON_HEIGHT + HEXAGON_MARGIN )/ 2),
                new Point(910, 780),
                new Point(910 + 3 * HEXAGON_WIDTH / 4 + HEXAGON_MARGIN, 780 + (HEXAGON_HEIGHT + HEXAGON_MARGIN) / 2),
                new Point(810 + HEXAGON_WIDTH / 4, 780 + 3 * (HEXAGON_HEIGHT + HEXAGON_MARGIN) / 2),
                new Point(910, 780 + (HEXAGON_HEIGHT + HEXAGON_MARGIN) * 2),
                new Point(910 + 3 * HEXAGON_WIDTH / 4 + HEXAGON_MARGIN, 780 + 3 * (HEXAGON_HEIGHT + HEXAGON_MARGIN) / 2),
                new Point(910, 780 + HEXAGON_HEIGHT + 4)
            };
        #endregion

        class ConsolePrototype
        {
            public Boolean Enabled;

            static private string consoleString;
            static private string consolePrevString;
            static private string consoleLog;

            static private Rectangle CONSOLE_REGION;

            public string getString() { return consoleString; }

            public string getPrevString() { return consolePrevString; }

            public string getLog() { return consoleLog; }

            public int getLength() { return consoleString.Length; }

            public void setString(string String) { consoleString = String; }

            public void setPrevString(string String) { consolePrevString = String; }

            public void setLog(string String) { consoleLog = String; }

            public Rectangle getRegion()
            {
                CONSOLE_REGION = new Rectangle(0, 0, 520, 50);
                return CONSOLE_REGION;
            }

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
                            currentResolution.Height = TI2;
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
                    case "PAUSE":
                        isPaused = true;
                        Enabled = false;
                        break;
                    case "OUTPUT":
                    case "DEBUG":
                    case "INFORMATION":
                    case "INFO":
                    case "DEBUGINFO":
                    case "DEBUGINFORMATION":
                        if (showDI)
                        {
                            showDI = false;
                            consoleLog = "Debug output is disabled.";
                        }
                        else
                        {
                            showDI = true;
                            consoleLog = "Debug output is enabled.";
                        }
                        break;
                    case "HITBOX":
                    case "HITBOXES":
                        if (showHitbox)
                        {
                            showHitbox = false;
                            consoleLog = "Hitboxes are disabled.";
                        }
                        else
                        {
                            showHitbox = true;
                            consoleLog = "Hitboxes are enabled.";
                        }
                        break;
                    case "FREEZE":
                    case "STOP":
                        consoleLog = "Zombies are freezed.";
                        zombieFreeze = true;
                        break;
                    case "UNFREEZE":
                    case "GO":
                        consoleLog = "Zombies are unfreezed.";
                        zombieFreeze = false;
                        break;
                    case "AUTO":
                    case "AUTORES":
                    case "AUTORESOLUTION":
                        currentResolution = Screen.PrimaryScreen.Bounds.Size;
                        consoleLog = "Automatically " + currentResolution.Width + "x" + currentResolution.Height + " done.";
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
                    case "HEALZOMBIE":
                    case "HEALZOMBIES":
                        for (int a = 0; a < zombieCount; ++a)
                            if (zUnits[a].isAlive())
                            {
                                int tempi = zUnits[a].getMaxHealth();
                                zUnits[a].setHealth(tempi);
                            }
                        if (zombieCount > 0)
                            consoleLog = zombieCount + " zombie" + (zombieCount > 1 ? "s are " : " is ") + "healed.";
                        else
                            consoleLog = "No zombies.";
                        break;
                    case "REFRESHSPELL":
                    case "REFRESHSPELLS":
                        for (int a = 0; a < CDT.Length; ++a)
                        {
                            CDT[a] = 0f;
                            WeaponUsed[a] = false;
                        }
                        for (int a = 0; a < zombieCount; ++a)
                            zUnits[a].setHitedStatus(false, -1);
                        consoleLog = "Spells refreshed.";
                        break;
                    //case ""
                }
                consolePrevString = consoleString;
                if (consoleString.Length > 0)
                    consoleString = consoleString.Remove(0);
            }
        }

        class Zombie
        {
            ZombieTypes ZombieType;
            ZombieActions ZombieAction;
            PointF Position;
            float Speed = 1;
            Boolean Alive = true;
            int Direction;
            int Health = 10;
            int MaxHealth = 10;
            Boolean[] HITED = new Boolean[WEAPONS_COUNT];

            public Zombie(int x, int y)
            {
                Position = new Point(x, y);
            }
            public Zombie(int x, int y, int _Direction):this(x, y)
            {
                Direction = _Direction;
            }

            public Zombie(int x, int y, int _Direction, ZombieTypes Type):this(x, y, _Direction)
            {
                ZombieType = Type;
                switch (Type)
                {
                    case ZombieTypes.HUGE:
                        Health = 50;
                        MaxHealth = 50;
                        break;
                    case ZombieTypes.BIG:
                        Health = 20;
                        MaxHealth = 20;
                        break;
                    case ZombieTypes.NORMAL:
                        Health = 10;
                        MaxHealth = 10;
                        break;
                }
            }
            
            public Zombie(int x, int y, int _Direction, ZombieTypes Type, ZombieActions Action):this(x, y, _Direction, Type)
            {
                ZombieAction = Action;
            }

            public ZombieTypes getZombieType()
            {
                return ZombieType;
            }

            public ZombieActions getZombieAction()
            {
                return ZombieAction;
            }

            public void setZombieAction(ZombieActions Action)
            {
                ZombieAction = Action;
            }

            public PointF getPosition()
            {
                return Position;
            }
            public int getX()
            {
                int X = (int)Position.X;
                return X;
            }
            public int getY()
            {
                int Y = (int)Position.Y;
                return Y;
            }
            public Rectangle getRectangle(Boolean actual, ZombieTypes Type)
            {
                Rectangle Region = new Rectangle((int)Position.X + 5, (int)Position.Y, ZOMBIE_WIDTH - 15, ZOMBIE_HEIGHT - 5);
                switch (Type)
                {
                    case ZombieTypes.NORMAL:
                        #region NORMAL
                        if (actual)
                            switch (Direction)
                            {
                                case 0:
                                    Region = new Rectangle((int)Position.X, (int)Position.Y + 10, ZOMBIE_WIDTH - 20, ZOMBIE_HEIGHT - 5);
                                    break;
                                case 1:
                                    Region = new Rectangle((int)Position.X + 5, (int)Position.Y, ZOMBIE_WIDTH - 15, ZOMBIE_HEIGHT - 5);
                                    break;
                                case 2:
                                    Region = new Rectangle((int)Position.X + 5, (int)Position.Y + 5, ZOMBIE_WIDTH - 15, ZOMBIE_HEIGHT - 5);
                                    break;
                                case 3:
                                    Region = new Rectangle((int)Position.X + 10, (int)Position.Y, ZOMBIE_WIDTH - 15, ZOMBIE_HEIGHT - 5);
                                    break;
                                default:
                                    break;
                            }
                        else
                            Region = new Rectangle(new Point((int)Position.X, (int)Position.Y), ZOMBIE_SIZE);
                        break;
                        #endregion 
                    case ZombieTypes.BIG:
                        #region BIG
                        if (actual)
                            switch (Direction)
                            {
                                case 0:
                                    Region = new Rectangle((int)Position.X + 15, (int)Position.Y + 15, BIG_ZOMBIE_WIDTH + 10, BIG_ZOMBIE_HEIGHT - 65);
                                    break;
                                case 1:
                                    Region = new Rectangle((int)Position.X + 5, (int)Position.Y + 10, BIG_ZOMBIE_WIDTH - 15, BIG_ZOMBIE_HEIGHT - 15);
                                    break;
                                case 2:
                                    Region = new Rectangle((int)Position.X + 20, (int)Position.Y, BIG_ZOMBIE_WIDTH + 20, BIG_ZOMBIE_HEIGHT - 55);
                                    break;
                                case 3:
                                    Region = new Rectangle((int)Position.X + 5, (int)Position.Y + 15, BIG_ZOMBIE_WIDTH - 15, BIG_ZOMBIE_HEIGHT - 30);
                                    break;
                                default:
                                    break;
                            }
                        else
                            Region = new Rectangle(new Point((int)Position.X, (int)Position.Y), BIG_ZOMBIE_SIZE);
                        break;
                        #endregion
                    case ZombieTypes.HUGE:
                        #region HUGE
                        if (actual)
                        {
                            Region = new Rectangle((int)Position.X, (int)Position.Y, HUGE_ZOMBIE_WIDTH, HUGE_ZOMBIE_HEIGHT);
                            switch (Direction)
                            {
                                case 0:
                                    Region.Inflate(-35, -10);
                                    Region.Offset(-20, 23);
                                    break;
                                case 1:
                                    Region.Inflate(-30, -6);
                                    Region.Offset(-15, 0);
                                    break;
                                case 2:
                                    Region.Inflate(-30, -6);
                                    Region.Offset(-15, 0);
                                    break;
                                case 3:
                                    Region.Inflate(-20, -6);
                                    Region.Offset(5, 0);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                            Region = new Rectangle(new Point((int)Position.X, (int)Position.Y), HUGE_ZOMBIE_SIZE);
                        break;
                    #endregion
                }
                return Region;
                #region RectangleRotateGovnocod
                {
                    
                }
                #endregion
            }
            public int getDirection()
            {
                return Direction;
            }
            public int getHealth()
            {
                return Health;
            }
            public int getMaxHealth()
            {
                return MaxHealth;
            }
            public Rectangle getHealthBar()
            {
                Rectangle Rect = new Rectangle((int)Position.X, (int)Position.Y - (Direction % 2 == 0 ? 0 : 10), Health * ZOMBIE_WIDTH / MaxHealth, 8);
                switch(ZombieType)
                {
                    case ZombieTypes.NORMAL:
                        Rect = new Rectangle((int)Position.X, (int)Position.Y - (Direction % 2 == 0 ? 0 : 10), Health * ZOMBIE_WIDTH / MaxHealth, 8);
                        break;
                    case ZombieTypes.BIG:
                        Rect = new Rectangle((int)Position.X + 15, (int)Position.Y - (Direction % 2 == 0 ? 0 : 10), Health * (BIG_ZOMBIE_WIDTH - 30) / MaxHealth, 8);
                        break;
                    case ZombieTypes.HUGE:
                        Rect = new Rectangle((int)Position.X + 30, (int)Position.Y - (Direction % 2 == 0 ? 0 : 10), Health * (HUGE_ZOMBIE_WIDTH - 80) / MaxHealth, 8);
                        break;
                }
                return Rect;
            }
            public SolidBrush getHealthBrush()
            {
                Color Color = HealthColor[0];
                float HealthRatio = 4 * Health / MaxHealth;
                if (Health > 0)
                    Color = HealthColor[4 - (int)HealthRatio];
                SolidBrush Brush = new SolidBrush(Color);
                return Brush;
            }
            public void setPosition(Point _Position)
            {
                Position = _Position;
            }
            public void setDirection(int _Direction)
            {
                Direction = _Direction;
            }
            public void setHealth(int _Health)
            {
                Health = _Health;
            }
            public void setHitedStatus(Boolean _Hited, int _index)
            {
                if (_index == -1)
                    for (int a = 0; a < WEAPONS_COUNT; ++a)
                        HITED[a] = false;
                else
                    HITED[_index] = false;
            }
            public void doDamage(int Damage)
            {
                if (Damage != -1)
                    Health -= Damage;
                else
                    Health = 0;
                if (Health < 1)
                    Alive = false;
            }
            public void doDamage(int Damage, WeaponType Source)
            {
                if (Source != WeaponType.CURSOR)
                    if (!HITED[(int)currentWeapon])
                    {
                        doDamage(Damage);
                        HITED[(int)currentWeapon] = true;
                    }
            }
            public Boolean isAlive()
            {
                return Alive;
            }
            public void setAlive(Boolean _Alive)
            {
                Alive = _Alive;
            }
            public void changePosition(int x, int y)
            {
                Position.X += x;
                Position.Y += y;
            }
            public void changePosition(float x, float y)
            {
                Position.X += x;
                Position.Y += y;
            }
            public void changePosition(Point p)
            {
                Position.X += p.X;
                Position.Y += p.Y;
            }
        };
        static List<Zombie> zUnits = new List<Zombie>();

        #region Hexagon
        Boolean[]
            HexagonHover = new Boolean[7];
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
        string HexagonString = "";
        static float[] CDT = new float[WEAPONS_COUNT] { 0, 0, 0, 0, 0, 0 };
        static Boolean[]
            WeaponUsed = new Boolean[WEAPONS_COUNT],
            WeaponEffect = new Boolean[WEAPONS_COUNT];
        Point[]
            WeaponEffectPoint = new Point[WEAPONS_COUNT];

        void applyCommand()
        {
            switch (HexagonString)
            {
                case "":
                    currentWeapon = WeaponType.CURSOR;
                    break;
                case "QEWDAS":
                    currentWeapon = WeaponType.SNIPER_RIFLE;
                    break;
                case "AASESA":
                    currentWeapon = WeaponType.EXPLOSION;
                    break;
                case "SWDEQA":
                    currentWeapon = WeaponType.LIGHTNING;
                    break;
            }
            HexagonString = "";
        }
        #endregion

        #region WeaponSystem
        static WeaponType currentWeapon;
        void CDTT(object sender, EventArgs e)
        {
            if (!isPaused)
            {
                for (int i = 0; i < CDT.Length; ++i)
                    if (WeaponUsed[i])
                        CDT[i] += 0.1f;
                for (int a = 0; a < WEAPONS_COUNT; ++a)
                    if (WeaponEffect[a])
                        switch (a)
                        {
                            case (int)WeaponType.EXPLOSION:
                                if (effectsFrames[(int)WeaponType.EXPLOSION] < EXPLOSION_FRAMES)
                                    effectsFrames[(int)WeaponType.EXPLOSION]++;
                                else
                                {
                                    effectsFrames[(int)WeaponType.EXPLOSION] = 0;
                                    WeaponEffect[a] = false;
                                }
                                break;
                            case (int)WeaponType.LIGHTNING:
                                if (effectsFrames[(int)WeaponType.LIGHTNING] < LIGHTNING_FRAMES)
                                    effectsFrames[(int)WeaponType.LIGHTNING]++;
                                else
                                {
                                    effectsFrames[(int)WeaponType.LIGHTNING] = 0;
                                    WeaponEffect[a] = false;
                                }
                                break;
                        }
                if (CDT[(int)currentWeapon] >= SPELL_COOLDOWNS[(int)currentWeapon])
                {
                    CDT[(int)currentWeapon] = 0;
                    foreach (Zombie TZ in zUnits)
                        TZ.setHitedStatus(false, (int)currentWeapon);
                    WeaponUsed[(int)currentWeapon] = false;
                }
            }
        }
        #endregion

        ConsolePrototype
            Console = new ConsolePrototype();
        static Timer
            updateTimer = new Timer(),
            cooldownTimer = new Timer();
        static Point
            mouseOffset = new Point(0, 0),
            mousePosition = new Point(960, 540);
        static Size
            currentResolution = new Size(1920, 1080);
        static Boolean
            isPaused, showDI, showHitbox, zombieFreeze = true;
        static int
            zombieFrames, zombieCount = 8, zombieTotalCount = 8;
        static int[,] map = new int[(MAX_OFFSET_Y * 2 + 1080) / 5, (MAX_OFFSET_X * 2 + 1920) / 5];
        static int[] effectsFrames = new int[WEAPONS_COUNT] { 0, 0, 0, 0, 0, 0 };
        private static int lastTick, lastFrameRate, frameRate;
        public static int CalculateFrameRate()
        {
            if (System.Environment.TickCount - lastTick >= 1000)
            {
                lastFrameRate = frameRate;
                frameRate = 0;
                lastTick = System.Environment.TickCount;
            }
            frameRate++;
            return lastFrameRate;
        }

        public Form1()
        {
            InitializeComponent();
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pMouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pMouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pMouseUp);
            this.Size = new Size(1920, 1080);
            Cursor.Position = new Point(mousePosition.X, mousePosition.Y);
            Cursor.Hide();
            this.Paint += new PaintEventHandler(pDraw);
            this.KeyDown += new KeyEventHandler(pKeyDown);
            this.KeyUp += new KeyEventHandler(pKeyUp);
            updateTimer.Interval = 1;
            updateTimer.Tick += new EventHandler(pUpdate);
            updateTimer.Start();
            cooldownTimer.Interval = 100;
            cooldownTimer.Tick += new EventHandler(CDTT);
            cooldownTimer.Start();
            for (int a = 0; a < map.GetLength(0); ++a)
                for (int b = 0; b < map.GetLength(1); ++b)
                    if (a % 100 == 0 | b % 100 == 0)//if ((a % 20 == 0 & b % 20 == 0) || (a % 20 == 10 & b % 20 == 10))
                        map[a, b] = 1;
            for (int a = 0; a < 7; ++a)
                Hexagon[a].AddPolygon(BuildHexagon(HexagonPosition[a]));
            for (int a = 0; a < 2; ++a)
                zUnits.Add(new Zombie(300 + a * 50, 640 + 80 * a, 0));
            for (int a = 0; a < 2; ++a)
                zUnits.Add(new Zombie(1800 - a * 50, 600 + 80 * a, 3));
            for (int a = 0; a < 2; ++a)
                zUnits.Add(new Zombie(560 + a * 30, 550 + 120 * a, 2, ZombieTypes.BIG));
            for (int a = 0; a < 2; ++a)
                zUnits.Add(new Zombie(60 + a * 160, 980 + a * 20, 1, ZombieTypes.HUGE));
            ImageAnimator.Animate(iZombie, this.pUpdate);
            ImageAnimator.Animate(iZombieIdle, this.pUpdate);
            ImageAnimator.Animate(iBigZombie, this.pUpdate);
            ImageAnimator.Animate(iBigZombieIdle, this.pUpdate);
            ImageAnimator.Animate(iHugeZombie, this.pUpdate);
            ImageAnimator.Animate(iHugeZombieIdle, this.pUpdate);
            TextFormatCenter.Alignment = StringAlignment.Center;
        }

        void pMouseDown(object sender, MouseEventArgs e)
        {
            Point temp = ConvertClickPoint(e.X, e.Y, false, true),
                temptwo = ConvertClickPoint(e.X, e.Y, true, true);
            if (e.Button == MouseButtons.Left)
            {
                for (int q = 0; q < 7; ++q)
                {
                    HexagonHover[q] = Hexagon[q].IsVisible(temp);
                    if (HexagonHover[q])
                        if (q != 6)
                            HexagonString += HexagonText[q];
                        else
                            applyCommand();
                }

                if (zombieTotalCount > 0)
                    for (int q = 0; q < zombieTotalCount; ++q)
                        if (!zUnits[q].isAlive())
                        {
                            zUnits[q].setAlive(false);
                        }
                        else
                            if (zUnits[q].getRectangle(true, zUnits[q].getZombieType()).Contains(temptwo))
                            {
                                switch (currentWeapon)
                                {
                                    case WeaponType.CURSOR:
                                        zUnits[q].doDamage(1);
                                        break;
                                    case WeaponType.SNIPER_RIFLE:
                                        if (CDT[(int)currentWeapon] == 0 && !WeaponUsed[(int)currentWeapon])
                                        {
                                            zUnits[q].doDamage(5);
                                            WeaponUsed[(int)currentWeapon] = true;
                                        }
                                        break;
                                    case WeaponType.EXPLOSION:
                                        if (CDT[(int)currentWeapon] == 0 && !WeaponUsed[(int)currentWeapon])
                                        {
                                            zUnits[q].doDamage(-1);
                                            Point offsetedPoint = new Point(temptwo.X - EXPLOSION_SIZE / 2, temptwo.Y - EXPLOSION_SIZE / 2);
                                            Point[] NearPoints = new Point[]
                                        {
                                            new Point(temptwo.X + EXPLOSION_SIZE / 2, temptwo.Y),
                                            new Point(temptwo.X - EXPLOSION_SIZE / 2, temptwo.Y),
                                            new Point(temptwo.X, temptwo.Y + EXPLOSION_SIZE / 2),
                                            new Point(temptwo.X, temptwo.Y - EXPLOSION_SIZE / 2),
                                            new Point(temptwo.X + EXPLOSION_SIZE / 2, temptwo.Y + EXPLOSION_SIZE / 2),
                                            new Point(temptwo.X - EXPLOSION_SIZE / 2, temptwo.Y - EXPLOSION_SIZE / 2),
                                            new Point(temptwo.X - EXPLOSION_SIZE / 2, temptwo.Y + EXPLOSION_SIZE / 2),
                                            new Point(temptwo.X + EXPLOSION_SIZE / 2, temptwo.Y - EXPLOSION_SIZE / 2)
                                        };
                                            WeaponEffect[(int)currentWeapon] = true;
                                            WeaponUsed[(int)currentWeapon] = true;
                                            WeaponEffectPoint[(int)currentWeapon] = offsetedPoint;
                                            foreach (Point TP in NearPoints)
                                                foreach (Zombie TZ in zUnits)
                                                    if (TZ.getRectangle(true, TZ.getZombieType()).Contains(TP))
                                                        TZ.doDamage(3, WeaponType.EXPLOSION);
                                        }
                                        break;
                                    case WeaponType.LIGHTNING:
                                        if (CDT[(int)currentWeapon] == 0 && !WeaponUsed[(int)currentWeapon])
                                        {
                                            zUnits[q].doDamage(4, WeaponType.LIGHTNING);
                                            List<Point> NearPoints = new List<Point>();
                                            for (int m = -256; m < 256; m += 50)
                                                if (m != -ZOMBIE_WIDTH || m != 0 || m != ZOMBIE_WIDTH)
                                                {
                                                    NearPoints.Add(new Point(temptwo.X + m, temptwo.Y));
                                                }
                                            for (int n = -256; n < 256; n += 50)
                                                if (n != -ZOMBIE_HEIGHT || n != 0 || n != ZOMBIE_HEIGHT)
                                                {
                                                    NearPoints.Add(new Point(temptwo.X, temptwo.Y + n));
                                                }
                                            WeaponEffect[(int)currentWeapon] = true;
                                            WeaponUsed[(int)currentWeapon] = true;
                                            WeaponEffectPoint[(int)currentWeapon] = new Point(temptwo.X, temptwo.Y);
                                            foreach (Point TP in NearPoints)
                                                foreach (Zombie TZ in zUnits)
                                                    if (TZ.getRectangle(true, TZ.getZombieType()).Contains(TP))
                                                        TZ.doDamage(2, WeaponType.LIGHTNING);
                                        }
                                        break;
                                }
                            }
            }
        }

        void pMouseUp(object sender, MouseEventArgs e)
        {
            for (int q = 0; q < 7; ++q)
            {
                HexagonHover[q] = false;
            }

        }

        void pMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
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

        Point ConvertClickPoint(int eX, int eY, Boolean WithOffset, Boolean WithRatio)
        {
            int a = (int)((eX - (WithOffset ? mouseOffset.X : 0)) / (WithRatio ? GetResolutionRatio().Width : 1)),
                b = (int)((eY - (WithOffset ? mouseOffset.Y : 0)) / (WithRatio ? GetResolutionRatio().Height : 1));
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
                    if (!Console.Enabled && !isPaused)
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
                case Keys.P:
                    if (!Console.Enabled)
                        if (isPaused)
                            isPaused = false;
                        else
                            isPaused = true;
                    break;
                case Keys.O:
                    if (!Console.Enabled)
                        if (showDI)
                            showDI = false;
                        else
                            showDI = true;
                    break;
            }
            for (int a = 0; a < 6; ++a)
                if ((char)e.KeyValue == HexagonText[a][0])
                {
                    if (HexagonHover[a])
                        HexagonString += (char)e.KeyValue;
                    HexagonHover[a] = false;
                }
            if (e.KeyData == Keys.Space && !Console.Enabled)
            {
                HexagonHover[6] = false;
                applyCommand();
                for (int a = 0; a < 6; ++a)
                {
                    if (HexagonHover[a])
                        HexagonString += HexagonText[a];
                    HexagonHover[a] = false;
                }
            }
        }

        void pUpdate(object sender, EventArgs e)
        {
            if (!isPaused)
            {
                if (!zombieFreeze)
                    if (zombieFrames < 29)
                    {
                        int RandomNumberForSlowSprite = new Random(DateTime.Now.Millisecond).Next(100);
                        if (RandomNumberForSlowSprite < 75)
                            zombieFrames++;
                    }
                    else
                        zombieFrames = 0;

                if (zombieCount > 0)
                {
                    zombieCount = 0;
                    for (int q = 0; q < zombieTotalCount; ++q)
                    {
                        if (zUnits[q].isAlive())
                        {
                            zombieCount++;
                            if (!zombieFreeze && zUnits[q].getZombieAction() == ZombieActions.WALK)
                            {
                                float x = zUnits[q].getX();
                                float y = zUnits[q].getY();
                                float sx = zUnits[q].getRectangle(true, zUnits[q].getZombieType()).Width;
                                float sy = zUnits[q].getRectangle(true, zUnits[q].getZombieType()).Height;
                                //ZombieStep(zUnits[q], x + 1920 + mouseOffset.X, y + 1080 + mouseOffset.Y, sx, sy, zUnits[q].getDirection(), 0);
                                //    }
                                switch (zUnits[q].getDirection())
                                {
                                    case 0:
                                        if (y > -MAX_OFFSET_Y && map[(int)((y - 1) / 5), (int)(x / 5)] == 0)
                                            zUnits[q].changePosition(0, -1);
                                        else
                                        {
                                            zUnits[q].setDirection(3);
                                            //zUnits[q].setZombieAction(ZombieActions.STAND);
                                        }
                                        break;
                                    case 1:
                                        if (x + sx < 1920 + MAX_OFFSET_X && map[(int)(y / 5), (int)((x + sx + 1) / 5)] == 0)
                                            zUnits[q].changePosition(1, 0);
                                        else
                                        {
                                            zUnits[q].setDirection(0);
                                            //zUnits[q].setZombieAction(ZombieActions.STAND);
                                        }
                                        break;
                                    case 2:
                                        if (y + sy < 1080 + MAX_OFFSET_Y && map[(int)((y + sy + 1) / 5), (int)(x / 5)] == 0)
                                            zUnits[q].changePosition(0, 1);
                                        else
                                        {
                                            zUnits[q].setDirection(1);
                                            //zUnits[q].setZombieAction(ZombieActions.STAND);
                                        }
                                        break;
                                    case 3:
                                        if (x > -MAX_OFFSET_X && map[(int)(y / 5), (int)((x - 1) / 5)] == 0)
                                            zUnits[q].changePosition(-1, 0);
                                        else
                                        {
                                            zUnits[q].setDirection(2);
                                            //zUnits[q].setZombieAction(ZombieActions.STAND);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }

                Invalidate();
            }
        }

        void ZombieStep(Zombie zUnit, float x, float y, float sx, float sy, int dir, int step)
        {
            int[,] DirList = new int[4, 3]
            {
                { 0, 1, 3 },
                { 1, 0, 2 },
                { 2, 1, 3 },
                { 3, 0, 2 }
            };

            zUnit.setDirection(dir);
            //Point TP = ZombieCanStep(zUnit, x, y, sx, sy, dir);
            //if (TP.X != 0 && TP.Y != 0)
            //    zUnit.changePosition(TP);
            //if (step < 2)
            //{
            //    step++;
            //    ZombieStep(zUnit, x, y, sx, sy, DirList[dir, step], step);
            //}
            switch (dir)
            {
                case 0:
                    if (y > -MAX_OFFSET_Y && map[(int)((y - 1) / 5), (int)(x / 5)] == 0 && map[(int)((y - 1) / 5), (int)((x + sx) / 5)] == 0)
                        zUnit.changePosition(0, -1);
                    else if (step < 2)
                    {
                        step++;
                        ZombieStep(zUnit, x, y, sx, sy, DirList[dir, step], step);
                        //zUnit.setDirection(3);
                        //zUnit.setZombieAction(ZombieActions.STAND);
                    }
                    break;
                case 1:
                    if (x + sx < 1920 + MAX_OFFSET_X && map[(int)(y / 5), (int)((x + sx + 1) / 5)] == 0 && map[(int)((y + sy) / 5), (int)((x + sx + 1) / 5)] == 0)
                        zUnit.changePosition(1, 0);
                    else if (step < 2)
                    {
                        step++;
                        ZombieStep(zUnit, x, y, sx, sy, DirList[dir, step], step);
                        //zUnit.setDirection(0);
                        //zUnit.setZombieAction(ZombieActions.STAND);
                    }
                    break;
                case 2:
                    if (y + sy < 1080 + MAX_OFFSET_Y && map[(int)((y + sy + 1) / 5), (int)(x / 5)] == 0 && map[(int)((y + sy + 1) / 5), (int)((x + sx) / 5)] == 0)
                        zUnit.changePosition(0, 1);
                    else if (step < 2)
                    {
                        step++;
                        ZombieStep(zUnit, x, y, sx, sy, DirList[dir, step], step);
                        //zUnit.setDirection(1);
                        //zUnit.setZombieAction(ZombieActions.STAND);
                    }
                    break;
                case 3:
                    if (x > -MAX_OFFSET_X && map[(int)(y / 5), (int)((x - 1) / 5)] == 0 && map[(int)((y + sy) / 5), (int)((x - 1) / 5)] == 0)
                        zUnit.changePosition(-1, 0);
                    else if (step < 2)
                    {
                        step++;
                        ZombieStep(zUnit, x, y, sx, sy, DirList[dir, step], step);
                        //zUnit.setDirection(2);
                        //zUnit.setZombieAction(ZombieActions.STAND);
                    }
                    break;
            }
        }

        Point ZombieCanStep(Zombie zUnit, float x, float y, float sx, float sy, int dir)
        {
            switch (dir)
            {
                case 0:
                    if (y > -MAX_OFFSET_Y && map[(int)((y - 1) / 5), (int)(x / 5)] == 0 && map[(int)((y - 1) / 5), (int)((x + sx) / 5)] == 0)
                        return new Point(0, -1);
                    break;
                case 1:
                    if (x + sx < 1920 + MAX_OFFSET_X && map[(int)(y / 5), (int)((x + sx + 1) / 5)] == 0 && map[(int)((y + sy) / 5), (int)((x + sx + 1) / 5)] == 0)
                        return new Point(1, 0);
                    break;
                case 2:
                    if (y + sy < 1080 + MAX_OFFSET_Y && map[(int)((y + sy + 1) / 5), (int)(x / 5)] == 0 && map[(int)((y + sy + 1) / 5), (int)((x + sx) / 5)] == 0)
                        return new Point(0, 1);
                    break;
                case 3:
                    if (x > -MAX_OFFSET_X && map[(int)(y / 5), (int)((x - 1) / 5)] == 0 && map[(int)((y + sy) / 5), (int)((x - 1) / 5)] == 0)
                        return new Point(-1, 0);
                    break;
            }
            return new Point(0, 0);
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

        void pDraw(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.TranslateTransform(mouseOffset.X, mouseOffset.Y);
            g.ScaleTransform(GetResolutionRatio().Width, GetResolutionRatio().Height);
            if (!zombieFreeze)
                ImageAnimator.UpdateFrames();

            String RightBlockInfo = "Count: " + zombieCount + "/" + zombieTotalCount + "\n";

            if (zombieTotalCount > 0)
                foreach (var q in zUnits)
                {
                    if (q.isAlive())
                    {
                        if (showDI)
                        {
                            RightBlockInfo += "Number: ";
                            RightBlockInfo += zUnits.LastIndexOf(q).ToString();
                            RightBlockInfo += "; Position: \n";
                            RightBlockInfo += q.getPosition().ToString();
                            RightBlockInfo += "\nHealth: ";
                            RightBlockInfo += q.getHealth();
                            RightBlockInfo += ";Type:";
                            RightBlockInfo += q.getZombieType().ToString();
                            RightBlockInfo += "\n-------------------------\n";
                        }

                        Bitmap temp = new Bitmap(iZombie);
                        switch (q.getZombieType())
                        {
                            case ZombieTypes.NORMAL:
                                if (q.getZombieAction() == ZombieActions.WALK)
                                    temp = RotateZombie(q, iZombie);
                                else
                                    temp = RotateZombie(q, iZombieIdle);
                                break;
                            case ZombieTypes.BIG:
                                if (q.getZombieAction() == ZombieActions.WALK)
                                    temp = RotateZombie(q, iBigZombie);
                                else
                                    temp = RotateZombie(q, iBigZombieIdle);
                                break;
                            case ZombieTypes.HUGE:
                                if (q.getZombieAction() == ZombieActions.WALK)
                                    temp = RotateZombie(q, iHugeZombie);
                                else
                                    temp = RotateZombie(q, iHugeZombieIdle);
                                break;
                        }
                        g.DrawImage(temp, q.getPosition());
                        g.FillRectangle(q.getHealthBrush(), q.getHealthBar());
                    }
                    if (showHitbox)
                        g.DrawRectangle(Pens.Black, q.getRectangle(true, q.getZombieType()));
                }

            g.ResetTransform();

            g.TranslateTransform(mouseOffset.X, mouseOffset.Y);
            g.ScaleTransform(GetResolutionRatio().Width, GetResolutionRatio().Height);
            #region Weapon
            for (int a = 0; a < WEAPONS_COUNT; ++a)
                if (WeaponEffect[a])
                {
                    switch (a)
                    {
                        case (int)WeaponType.EXPLOSION:
                            Rectangle effectRect = new Rectangle(WeaponEffectPoint[a], new Size(EXPLOSION_SIZE, EXPLOSION_SIZE));
                            g.DrawImage(iExplosionEffect, effectRect, new Rectangle(EXPLOSION_SIZE * effectsFrames[(int)WeaponType.EXPLOSION], 0, EXPLOSION_SIZE, EXPLOSION_SIZE), GraphicsUnit.Pixel);
                            if (showHitbox)
                            #region Hitbox
                            {
                                Point offsetedPoint = new Point(WeaponEffectPoint[a].X + EXPLOSION_SIZE / 2, WeaponEffectPoint[a].Y + EXPLOSION_SIZE / 2);
                                Point[] NearPoints = new Point[]
                                        {
                                            new Point(offsetedPoint.X + EXPLOSION_SIZE / 2, offsetedPoint.Y),
                                            new Point(offsetedPoint.X - EXPLOSION_SIZE / 2, offsetedPoint.Y),
                                            new Point(offsetedPoint.X, offsetedPoint.Y + EXPLOSION_SIZE / 2),
                                            new Point(offsetedPoint.X, offsetedPoint.Y - EXPLOSION_SIZE / 2),
                                            new Point(offsetedPoint.X + EXPLOSION_SIZE / 2, offsetedPoint.Y + EXPLOSION_SIZE / 2),
                                            new Point(offsetedPoint.X - EXPLOSION_SIZE / 2, offsetedPoint.Y - EXPLOSION_SIZE / 2),
                                            new Point(offsetedPoint.X - EXPLOSION_SIZE / 2, offsetedPoint.Y + EXPLOSION_SIZE / 2),
                                            new Point(offsetedPoint.X + EXPLOSION_SIZE / 2, offsetedPoint.Y - EXPLOSION_SIZE / 2)
                                        };
                                foreach (Point TP in NearPoints)
                                    g.FillRectangle(Brushes.Black, TP.X, TP.Y, 10, 10);
                            }
                            #endregion
                            break;
                        case (int)WeaponType.LIGHTNING:
                            Rectangle[] effectRects = new Rectangle[]{
                                new Rectangle(WeaponEffectPoint[a].X - LIGHTNING_WIDTH / 2, WeaponEffectPoint[a].Y - LIGHTNING_HEIGHT, LIGHTNING_WIDTH, LIGHTNING_HEIGHT),
                                new Rectangle(WeaponEffectPoint[a].X, WeaponEffectPoint[a].Y - LIGHTNING_WIDTH / 2, LIGHTNING_HEIGHT, LIGHTNING_WIDTH),
                                new Rectangle(WeaponEffectPoint[a].X - LIGHTNING_WIDTH / 2, WeaponEffectPoint[a].Y, LIGHTNING_WIDTH, LIGHTNING_HEIGHT),
                                new Rectangle(WeaponEffectPoint[a].X - LIGHTNING_HEIGHT, WeaponEffectPoint[a].Y - LIGHTNING_WIDTH / 2, LIGHTNING_HEIGHT, LIGHTNING_WIDTH),
                            };
                            if (showHitbox)
                            #region Hitbox
                            {
                                List<Point> NearPoints = new List<Point>();
                                for (int m = -256; m < 256; m += 50)
                                    if (m != -ZOMBIE_WIDTH || m != 0 || m != ZOMBIE_WIDTH)
                                    {
                                        NearPoints.Add(new Point(WeaponEffectPoint[a].X + m, WeaponEffectPoint[a].Y));
                                    }
                                for (int n = -256; n < 256; n += 50)
                                    if (n != -ZOMBIE_HEIGHT || n != 0 || n != ZOMBIE_HEIGHT)
                                    {
                                        NearPoints.Add(new Point(WeaponEffectPoint[a].X, WeaponEffectPoint[a].Y + n));
                                    }
                                foreach (Point TP in NearPoints)
                                    g.FillRectangle(Brushes.Black, TP.X, TP.Y, 10, 10);
                            }
                            #endregion
                            for (int b = 0; b < effectRects.Length; ++b)
                                if (b % 2 == 0)
                                    g.DrawImage(iLightningEffect, effectRects[b], new Rectangle(64 * effectsFrames[(int)WeaponType.LIGHTNING], 0, LIGHTNING_WIDTH, LIGHTNING_HEIGHT), GraphicsUnit.Pixel);
                                else
                                {
                                    Bitmap tempimg = new Bitmap(iLightningEffect);
                                    if (b == 1)
                                        tempimg.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                    else 
                                        tempimg.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                    g.DrawImage(tempimg, effectRects[b], new Rectangle(64 * effectsFrames[(int)WeaponType.LIGHTNING], 0, LIGHTNING_HEIGHT, LIGHTNING_WIDTH), GraphicsUnit.Pixel);
                                }
                            break;
                    }
                }
            g.ResetTransform();
            g.ScaleTransform(GetResolutionRatio().Width, GetResolutionRatio().Height);
            switch (currentWeapon)
            {
                case WeaponType.CURSOR:
                    break;
                case WeaponType.SNIPER_RIFLE:
                    g.DrawImage(iSniperRifle, SPELL_SLOT_RECTANGLE);
                    break;
                case WeaponType.EXPLOSION:
                    g.DrawImage(iExplosionIcon, SPELL_SLOT_RECTANGLE);
                    break;
                case WeaponType.LIGHTNING:
                    g.DrawImage(iLightningIcon, SPELL_SLOT_RECTANGLE);
                    break;
            }
            g.DrawRectangle(new Pen(Color.Goldenrod, 2), SPELL_SLOT_RECTANGLE);
            if (CDT[(int)currentWeapon] != 0 && currentWeapon != WeaponType.CURSOR)
            {
                g.FillPie(new SolidBrush(Color.FromArgb(90, 190, 190, 190)), SPELL_SLOT_RECTANGLE, -90, -6 * (SPELL_COOLDOWNS[(int)currentWeapon] - CDT[(int)currentWeapon]));
                string temps = Math.Round(SPELL_COOLDOWNS[(int)currentWeapon] - Math.Round(CDT[(int)currentWeapon], 1), 1).ToString();
                g.DrawString(temps, Verdana9, Brushes.LightGoldenrodYellow, SPELL_SLOT_RECTANGLE.X + (Double.Parse(temps) < 10 ? 73 : 67), SPELL_SLOT_RECTANGLE.Y + 84);
            }
            #endregion

            for (int a = 0; a < 7; ++a)
            {
                if (HexagonHover[a])
                    g.FillPath(new SolidBrush(HexagonColors[a]), Hexagon[a]);
                g.DrawPath(Pens.Black, Hexagon[a]);
                Point TPS = HexagonPosition[a];
                TPS.Offset(36 - (a == 6 ? 22 : a == 1 ? 2 : 0), 30);
                g.DrawString(HexagonText[a], Verdana16, Brushes.Black, TPS);
            }

            g.ResetTransform();
            if (Console.Enabled)
            #region Console
            {
                g.FillRectangle(Brushes.DimGray, Console.getRegion());
                g.DrawRectangle(Pens.Black, Console.getRegion());
                g.DrawLine(Pens.Black, new Point(0, 21), new Point(520, 21));
                g.DrawString("Console: ", Verdana13, Brushes.Black, 0, 25);
                g.DrawString(Console.getLog(), Verdana13, Brushes.Black, Console.getRegion().Location);
                g.DrawString(Console.getPrevString(), Verdana13, Brushes.Black, new RectangleF(100, 0, 420, 20), TextFormatCenter);
                g.DrawString(Console.getString(), Verdana13, Brushes.Black, 75, 25);
            }
            #endregion

            if (showDI)
            #region Debug Information
            {
                RightBlockInfo += "Spell Cooldowns:\n";
                for (int a = 1; a < CDT.Length; ++a)
                {
                    RightBlockInfo += DI_WEAPON_NAMES[a];
                    RightBlockInfo += " - ";
                    RightBlockInfo += Math.Round(CDT[a], 1);
                    RightBlockInfo += "\n";
                }
                g.DrawString(RightBlockInfo, Verdana11, Brushes.Black, currentResolution.Width - 200, 0);

                g.DrawString(map.GetLength(0) + ";" + map.GetLength(1) + "FPS: " + CalculateFrameRate().ToString() + "\nCursor Position:\n" + mousePosition.ToString() + "\nOffset: \n" + mouseOffset.ToString() + "\nConsole String:" + Console.getString() + "\nHexagon String:" + HexagonString + "\nMonitor Resolution:\n" + Screen.PrimaryScreen.Bounds.Size.ToString() + "\nApp Resolution:\n" + currentResolution.ToString() + "\nResolution Ratio:\n" + GetResolutionRatio().ToString(), Verdana11, Brushes.Black, 0, (Console.Enabled ? 50 : 0));
            }
            #endregion

            if (currentWeapon != WeaponType.SNIPER_RIFLE)
                g.DrawImage(iCursor, mousePosition);
            else
            {
                Point temp = mousePosition;
                temp.Offset(-iCrossHair.Width / 2, -iCrossHair.Height / 2);
                g.DrawImage(iCrossHair, temp);
            }
            if (showHitbox)
            {
                g.DrawRectangle(Pens.Black, new Rectangle(new Point(0, 0), currentResolution));

                g.ResetTransform();
                g.TranslateTransform(mouseOffset.X, mouseOffset.Y);
                g.ScaleTransform(GetResolutionRatio().Width, GetResolutionRatio().Height);

                for (int a = 0; a < map.GetLength(0); ++a)
                    for (int b = 0; b < map.GetLength(1); ++b)
                        if (map[a, b] == 1)
                            g.DrawRectangle(Pens.Black, new Rectangle(-MAX_OFFSET_X + a * 5, -MAX_OFFSET_Y + b * 5, 3, 3));
            }
        }

        private static Bitmap RotateZombie(Zombie q, Image image)
        {
            Bitmap temp = new Bitmap(image);
            switch (q.getDirection())
            {
                case 0:
                    temp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                case 1:
                    temp.RotateFlip(RotateFlipType.RotateNoneFlipNone);
                    break;
                case 2:
                    temp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case 3:
                    temp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    break;
            }
            return temp;
        }
    }
}