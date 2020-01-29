using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace labirint
{
    public partial class Form1 : Form
    {
        Image head = Image.FromFile(@"RES\head.png");
        Image grass = Image.FromFile(@"RES\grass.png");
        Image apple = Image.FromFile(@"RES\apple.png");
        Image tail = Image.FromFile(@"RES\tail.png");

        Bitmap bitmap = new Bitmap(100, 100);

        Pen BlackPen = new Pen(Color.Black);
        Pen GrayPen = new Pen(Color.Gray);
        Pen GreenPen = new Pen(Color.Green);
        Pen RedPen = new Pen(Color.Red);
        Pen SeaGreenPen = new Pen(Color.SeaGreen);

        Brush BlackBrush = new SolidBrush(Color.Black);
        Brush GrayBrush = new SolidBrush(Color.Gray);
        Brush GreenBrush = new SolidBrush(Color.Green);
        Brush RedBrush = new SolidBrush(Color.Red);
        Brush SeaGreenBrush = new SolidBrush(Color.SeaGreen);

        Random random = new Random();

        System.Media.SoundPlayer UDIED = new System.Media.SoundPlayer(@"RES\DED.wav");
        System.Media.SoundPlayer SIZEPlUS = new System.Media.SoundPlayer(@"RES\LVLUP.wav");

        int[,] map = new int[10, 10];
        int[] where = new int[2];
        int RX, RY, size, razmer, howRender, mashtab = 0;
        string move;
        bool can = false;

        Queue<int> partX = new Queue<int>();

        Queue<int> partY = new Queue<int>();


        void NextHwost()
        {
            //score
            label1.Text = "Your score" + "\n" + Convert.ToString(size);

            //clear
            for (int i = 0; i < razmer; i++)
            {
                for (int j = 0; j < razmer; j++)
                {
                    if (map[i, j] == 1)
                    {
                        map[i, j] = 0;
                    }
                }
            }

            //queue
            partX.Enqueue(where[0]);
            partY.Enqueue(where[1]);
            if (howRender > size)
            {
                partX.Dequeue();
                partY.Dequeue();
                howRender--;
            }
            howRender++;

            //addtomap
            for (int x = 0; x < partX.Count(); x++)
            {
                map[partX.ToArray()[x], partY.ToArray()[x]] = 1;
            }
        }

        void Uupdate()
        {
            Graphics graphics = Graphics.FromImage(bitmap);

            graphics.Clear(Color.White);

            graphics.DrawRectangle(BlackPen, 0, 0, razmer * mashtab, razmer * mashtab);
            graphics.FillRectangle(BlackBrush, 0, 0, razmer * mashtab, razmer * mashtab);

            for (int x = 0; x < razmer * mashtab; x += 1 * mashtab)
            {
                for (int z = 0; z < razmer * mashtab; z += 1 * mashtab)
                {
                    switch (map[z / mashtab, x / mashtab])
                    {
                        case 0:
                            graphics.DrawRectangle(GrayPen, x, z, mashtab, mashtab);
                            graphics.FillRectangle(GrayBrush, x, z, mashtab, mashtab);
                            graphics.DrawImage(grass, x, z, mashtab, mashtab);
                            break;
                        case 1:
                            graphics.DrawRectangle(GreenPen, x, z, mashtab, mashtab);
                            graphics.FillRectangle(GreenBrush, x, z, mashtab, mashtab);
                            graphics.DrawImage(tail, x, z, mashtab, mashtab);
                            break;
                        case 2:
                            graphics.DrawRectangle(RedPen, x, z, mashtab, mashtab);
                            graphics.FillRectangle(RedBrush, x, z, mashtab, mashtab);
                            graphics.DrawImage(apple, x, z, mashtab, mashtab);
                            break;
                    }
                }

            }

            graphics.DrawImage(head, where[1] * mashtab, where[0] * mashtab, mashtab, mashtab);

            pictureBox2.Image = bitmap;
        }

        void toomuch()
        {

            partX.Clear();
            partY.Clear();

            this.BackColor = Color.FromArgb(0, 0, 0);
            pictureBox1.Visible = true;
            UDIED.Play();
            label2.Visible = false;
            timer1.Enabled = false;
            timer2.Enabled = false;
            label1.Visible = false;
            button1.Enabled = true;
            pictureBox2.Visible = false;
        }

        void goLeft()
        {
            if (where[1] == 0 || map[where[0], where[1] - 1] == 1)
            {
                toomuch();
            }
            else
            {
                if (map[where[0], where[1] - 1] == 2)
                {
                    size++;
                    SIZEPlUS.Play();
                }
                map[where[0], where[1]] = 0;
                map[where[0], where[1] - 1] = 1;
                where[1] -= 1;
            }
            NextHwost();
        }

        void goUp()
        {
            if (where[0] == 0 || map[where[0] - 1, where[1]] == 1)
            {
                toomuch();
            }
            else
            {
                if (map[where[0] - 1, where[1]] == 2)
                {
                    size++;
                    SIZEPlUS.Play();
                }
                map[where[0], where[1]] = 0;
                map[where[0] - 1, where[1]] = 1;
                where[0] -= 1;
            }
            NextHwost();
        }

        void goDown()
        {
            if (where[0] == razmer - 1 || map[where[0] + 1, where[1]] == 1)
            {
                toomuch();
            }
            else
            {
                if (map[where[0] + 1, where[1]] == 2)
                {
                    size++;
                    SIZEPlUS.Play();
                }
                map[where[0], where[1]] = 0;
                map[where[0] + 1, where[1]] = 1;
                where[0] += 1;
            }
            NextHwost();
        }

        void goRight()
        {
            if (where[1] == razmer - 1 || map[where[0], where[1] + 1] == 1)
            {
                toomuch();
            }
            else
            {
                if (map[where[0], where[1] + 1] == 2)
                {
                    size++;
                    SIZEPlUS.Play();
                }
                map[where[0], where[1]] = 0;
                map[where[0], where[1] + 1] = 1;
                where[1] += 1;
            }
            NextHwost();
        }

        public void Razmer()
        {
            razmer = 10;
            map = new int[razmer, razmer];

            bitmap = new Bitmap(razmer * mashtab, razmer * mashtab);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            partX = new Queue<int>();
            partY = new Queue<int>();
            map = new int[10, 10];
            mashtab = 50;
            Razmer();

            size = 0;
            howRender = 0;

            pictureBox2.Visible = true;
            label2.Visible = true;
            timer1.Enabled = true;
            timer2.Enabled = true;
            pictureBox1.Visible = false;

            this.BackColor = Color.FromArgb(255, 255, 255);
            move = "right";
            label1.Visible = true;

            map[0, 0] = 1;
            where[0] = 0;
            where[1] = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 1000 - (size * 10);
            timer2.Interval = 5000 - (size * 10);
            switch (move)
            {
                case "up":
                    goUp();
                    break;
                case "down":
                    goDown();
                    break;
                case "right":
                    goRight();
                    break;
                case "left":
                    goLeft();
                    break;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            can = true;
            while (can)
            {
                RX = random.Next(0, razmer);
                RY = random.Next(0, razmer);
                if (map[RX, RY] == 0)
                {
                    can = false;
                }
                else
                {
                    can = true;
                }
            }
            can = true;
            map[RX, RY] = 2;
        }
        private void Button1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                case (char)Keys.W:
                    move = "up";
                    break;

                case (char)Keys.D:
                    move = "right";
                    break;

                case (char)Keys.A:
                    move = "left";
                    break;

                case (char)Keys.S:
                    move = "down";
                    break;

                default:
                    break;
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            Uupdate();
        }
    }
}
