using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mini_game
{
    public class gift
    {
        public int X, Y, W, H, time;
        Bitmap img = new Bitmap("gift.png");
    }

    public class MapGenerator
    {
        public int[,] map;
        public int stoneWidth;
        public int stoneHeight;

        public MapGenerator(int row, int col)
        {
            map = new int[row, col];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = 1;
                }
            }
            stoneWidth = 540 / col;
            stoneHeight = 150 / row;
        }

        public void setBrickValue(int value, int row, int col)
        {
            map[row, col] = value;
        }
    }

    public partial class Form1 : Form
    {
        Bitmap off;
        Timer timer = new Timer();
        Random random = new Random();
        Color ballColor = Color.Green;
        public MapGenerator mapSize;
        List<gift> LGift = new List<gift>();

        public bool play = false, flagGift = false, flagGiftTacken = false, flagBullet = false;
        public int racketX = 300; 
        public int ballPosX = 340, ballPosY = 400, ballDirX, ballDirY, ballW = 20, ballH = 20, ballSpead = 5;
        public int score, totalStones = 50, ctTick, bulletY = 540, bulletX;             
        
        public Form1()
        {
            mapSize = new MapGenerator(5, 10);
            this.Width = 710;
            this.Height = 610;
            Load += Form1_Load;
            Paint += Form1_Paint;
            KeyDown += Form1_KeyDown;
            timer.Interval = 10;
            timer.Tick += Timer_Tick;
            CenterToScreen();
        }

        void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(e.Graphics);
        }

        void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            ballDirX = getRandomNumberForX();
            ballDirY = getRandomNumberForY();
            timer.Start();
            DrawDubb(this.CreateGraphics());
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            ctTick++;
            if (play)
            {
                Rectangle ball = new Rectangle(ballPosX, ballPosY, 20, 20);
                Rectangle racket = new Rectangle(racketX, 550, 100, 8);
                if (ball.IntersectsWith(racket))
                {
                    ballDirY = -ballDirY;
                }

                for (int i = 0; i < mapSize.map.GetLength(0); i++)
                {
                    for (int j = 0; j < mapSize.map.GetLength(1); j++)
                    {
                        if (mapSize.map[i, j] > 0)
                        {
                            int brickX = j * mapSize.stoneWidth + 80;
                            int brickY = i * mapSize.stoneHeight + 50;
                            int stoneWidth = mapSize.stoneWidth;
                            int stoneHeight = mapSize.stoneHeight;

                            Rectangle brickRect = new Rectangle(brickX, brickY, stoneWidth, stoneHeight);
                            Rectangle ballRect = new Rectangle(ballPosX, ballPosY, 20, 20);
                            
                            if (ballRect.IntersectsWith(brickRect))
                            {
                                mapSize.setBrickValue(0, i, j);
                                totalStones--;
                                score += 5;

                                if (ballPosX + 19 <= brickRect.X || ballPosX + 1 >= brickRect.X + brickRect.Width)
                                {
                                    ballDirX = -ballDirX;
                                }
                                else
                                {
                                    ballDirY = -ballDirY;
                                }
                                break;
                            }

                            Rectangle bullet1 = new Rectangle(bulletX, bulletY, 10, 10);
                            Rectangle bullet2 = new Rectangle(bulletX + 90, bulletY, 10, 10);
                            if (bullet1.IntersectsWith(brickRect) || bullet2.IntersectsWith(brickRect))
                            {
                                mapSize.setBrickValue(0, i, j);
                                totalStones--;
                                score += 5;
                                flagBullet = false;
                            }
                        }
                    }
                }

                ballPosX += ballDirX;
                ballPosY += ballDirY;

                /* left */
                if (ballPosX < 0) { ballDirX = -ballDirX;}
                /* top */
                if (ballPosY < 0) { ballDirY = -ballDirY;}
                /* right */
                if (ballPosX > 670) { ballDirX = -ballDirX;}

                /* gift */
                createGifts();
                if(LGift[0].time == ctTick)
                {                    
                    flagGift = true;
                    LGift[0].X = this.Width / 2 - 20;
                }
                if(flagGift)
                {
                    LGift[0].Y += 5;                 
                }

                Rectangle gift = new Rectangle(LGift[0].X, LGift[0].Y, new Bitmap("gift.png").Width, new Bitmap("gift.png").Height);
                if (racket.IntersectsWith(gift))
                {
                    flagGift = false;
                    flagGiftTacken = true;
                }

                if(flagBullet)
                {
                    bulletY -= 5;
                }
            }
            DrawDubb(this.CreateGraphics());
        }


        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                if (racketX >= 600)
                {
                    racketX = 600;
                }
                else
                {
                    play = true;
                    racketX += 20;
                }
            }
            if (e.KeyCode == Keys.Left)
            {
                if (racketX < 10)
                {
                    racketX = 10;
                }
                else
                {
                    play = true;
                    racketX -= 20;
                }
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (!play)
                {
                    play = true;
                    setUpGame();                    
                    DrawDubb(this.CreateGraphics());
                }
            }

            if(e.KeyCode == Keys.Space)
            {
                if(flagGiftTacken)
                {
                    flagBullet = true;
                    bulletY = 540;
                    bulletX = racketX;
                }
            }
        }

        

        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }

        void DrawScene(Graphics g)
        {
            g.Clear(Color.Black);

            for (int i = 0; i < mapSize.map.GetLength(0); i++)
            {
                for (int j = 0; j < mapSize.map.GetLength(1); j++)
                {
                    if (mapSize.map[i, j] > 0)
                    {
                        SolidBrush stoneB = new SolidBrush(Color.White);
                        g.FillRectangle(stoneB, j * mapSize.stoneWidth + 90, i * mapSize.stoneHeight + 60, mapSize.stoneWidth, mapSize.stoneHeight);

                        Pen stoneP = new Pen(Brushes.Black, 4);
                        g.DrawRectangle(stoneP, j * mapSize.stoneWidth + 90, i * mapSize.stoneHeight + 60, mapSize.stoneWidth, mapSize.stoneHeight);
                    }
                }
            }

            /*border*/
            SolidBrush bordeB = new SolidBrush(ballColor);
            g.FillRectangle(bordeB, 0, 0, 3, 592);
            g.FillRectangle(bordeB, 0, 0, 692, 3);
            g.FillRectangle(bordeB, 691, 1, 3, 592);


            /*score*/
            Font scoreFont = new Font("Arial", 25);
            SolidBrush scoreB = new SolidBrush(Color.White);
            g.DrawString("Score: " + score + "/250", scoreFont, scoreB, 490, 10);

            /*racket*/
            SolidBrush racketB = new SolidBrush(ballColor);
            g.FillRectangle(racketB, racketX, 550, 100, 8);

            /* start game */
            if (play)
            {
                SolidBrush ballB2 = new SolidBrush(Color.Green);
                g.FillEllipse(ballB2, ballPosX, ballPosY, 20, 20);
            }
            else
            {
                SolidBrush ballB1 = new SolidBrush(Color.Black);
                g.FillEllipse(ballB1, ballPosX, ballPosY, 20, 20);

                //start game
                Font startFont = new Font("Arial", 15);
                SolidBrush startB = new SolidBrush(Color.Yellow);
                g.DrawString("Press Enter to Start The Game!", startFont, startB, this.Width /2 - 150, 350);
            }

            /* ball features*/

            if (score >= 50 && score < 100)
            {
                ballColor = Color.Yellow;
                SolidBrush scoreB1 = new SolidBrush(ballColor);
                g.FillEllipse(scoreB1, ballPosX, ballPosY, 25, 25);
            }
            else if (score >= 100 && score < 150)
            {
                ballColor = Color.Orange;
                SolidBrush scoreB1 = new SolidBrush(ballColor);
                g.FillEllipse(scoreB1, ballPosX, ballPosY, 30, 30);
            }
            else if (score >= 150)
            {
                ballColor = Color.Red;
                SolidBrush scoreB1 = new SolidBrush(ballColor);
                g.FillEllipse(scoreB1, ballPosX, ballPosY, 35, 35);
            }

            /* winner */
            if (totalStones <= 0)
            {
                play = false;
                flagGiftTacken = false;
                flagBullet = false;
                ballDirX = 0;
                ballDirY = 0;

                g.Clear(Color.Black);

                Font winFont = new Font("Arial", 30);
                SolidBrush winB = new SolidBrush(Color.Green);
                g.DrawString("You Win! Score: " + score, winFont, winB, this.Height / 2 - 150, this.Height / 2 - 150);


                Font restartFont = new Font("Arial", 20);
                SolidBrush restartB = new SolidBrush(Color.Red);
                g.DrawString("Press Enter to Restart..", restartFont, restartB, this.Height / 2 - 100, this.Height / 2 - 60);
            }

            /* game over */
            if (ballPosY > 570)
            {
                play = false;
                flagGiftTacken = false;
                flagBullet = false;
                ballDirX = 0;
                ballDirY = 0;

                g.Clear(Color.Black);

                Font overFont = new Font("Arial", 30);
                SolidBrush overB = new SolidBrush(Color.Red);
                g.DrawString("Game Over! Score: " + score, overFont, overB, this.Height/2 - 150, this.Height/2 - 150);

                Font restartFont = new Font("Arial", 20);
                SolidBrush restatB = new SolidBrush(Color.Yellow);
                g.DrawString("Press Enter to Restart..", restartFont, restatB, this.Height / 2 - 100, this.Height / 2 - 60);

            }

            /* gift */
            if(flagGift)
            {                
                g.DrawImage(new Bitmap("gift.png"), LGift[0].X, LGift[0].Y);
            }

            if(flagGiftTacken)
            {
                SolidBrush bulletsB = new SolidBrush(Color.White);
                g.FillRectangle(bulletsB, racketX, 540, 10, 10);
                g.FillRectangle(bulletsB, racketX + 90, 540, 10, 10);
            }

            if(flagBullet)
            {
                SolidBrush bulletsB = new SolidBrush(Color.White);
                g.FillRectangle(bulletsB, bulletX, bulletY, 10, 10);
                g.FillRectangle(bulletsB, bulletX + 90, bulletY, 10, 10);
            }
            
        }

        public int getRandomNumberForY()
        {           
            int max = -1;
            int min = -5;
            int randomNumber = min + random.Next(max - min + 1);
            return randomNumber;
        }

        public int getRandomNumberForX()
        {
            int max = -1;
            int min = -3;
            int randomNumber = min + random.Next(max - min + 1);
            return randomNumber;
        }

        public void setUpGame()
        {
            mapSize = new MapGenerator(5, 10);
            flagGiftTacken = false;
            flagBullet = false;
            racketX = 310;
            ballPosX = 290;
            ballPosY = 350;
            ballDirX = getRandomNumberForX();
            ballDirY = getRandomNumberForY();
            totalStones = 50;
            score = 0;
            ballColor = Color.Green;
        }

        public void createGifts()
        {
            int randomNumber = random.Next(300);
            gift pnn = new gift();
            pnn.time = randomNumber;
            LGift.Add(pnn);
        }
    }
}
