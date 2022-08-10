using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace big_game
{
    public class CActor
    {
        public int X, Y, W, H;
        public Bitmap img;
        public Bitmap[] fireball = new Bitmap[3];
    }

    public class CHero
    {
        public int X, Y;
        public Bitmap[] idle    = new Bitmap[6];
        public Bitmap[] run     = new Bitmap[8];
        public Bitmap[] attack  = new Bitmap[8];
        public Bitmap[] attack2 = new Bitmap[8];
        public Bitmap[] jump    = new Bitmap[2];
        public Bitmap[] fall    = new Bitmap[2];
        public Bitmap[] die     = new Bitmap[7];


        public CHero()
        {
            for(int i=0;i<run.Length;i++)
            {
                run[i] = new Bitmap("hero_run_" + i + ".png");
                run[i] = new Bitmap(run[i], 400, 400);
            }

            for (int i = 0; i < 6; i++)
            {
                idle[i] = new Bitmap("tile00" + i + ".png");
                idle[i] = new Bitmap(idle[i], 400, 400);
            }

            for (int i = 0; i < 8; i++)
            {
                attack[i] = new Bitmap("hero_attack_" + i + ".png");
                attack[i] = new Bitmap(attack[i], 400, 400);

                attack2[i] = new Bitmap("hero_attack2_" + i + ".png");
                attack2[i] = new Bitmap(attack2[i], 400, 400);
            }

            for (int i = 0; i < 2; i++)
            {
                jump[i] = new Bitmap("hero_jump_" + i + ".png");
                jump[i] = new Bitmap(jump[i], 400, 400);

                fall[i] = new Bitmap("hero_fall_" + i + ".png");
                fall[i] = new Bitmap(fall[i], 400, 400);
            }

            for (int i = 0; i < 7; i++)
            {
                die[i] = new Bitmap("hero_die_" + i + ".png");
                die[i] = new Bitmap(die[i], 400, 400);
            }
        }
    }

    public class CWizard
    {
        public int X, Y;
        public Bitmap[] idle = new Bitmap[5];
        public Bitmap[] attack = new Bitmap[10];
        public Bitmap[] die = new Bitmap[9];
        public Bitmap[] fireball = new Bitmap[3];

        public CWizard()
        {
            for (int i = 0; i < 5; i++)
            {
                idle[i] = new Bitmap("wizard-idle-" + i + ".png");
                idle[i] = new Bitmap(idle[i], 250, 250);
            }

            for (int i = 0; i < 10; i++)
            {
                attack[i] = new Bitmap("wizard-fire-" + i + ".png");
                attack[i] = new Bitmap(attack[i], 250, 250);
            }

            for (int i = 0; i < 9; i++)
            {
                die[i] = new Bitmap("wizard-death-" + i + ".png");
                die[i] = new Bitmap(die[i], 250, 250);
            }

            for (int i = 0; i < 3; i++)
            {
                fireball[i] = new Bitmap("fireball-" + i + ".png");
                fireball[i] = new Bitmap(fireball[i], 80, 80);
            }
        }
    }

    public class CDemon
    {
        public int X, Y;
        public Bitmap[] idle = new Bitmap[6];
        public Bitmap[] attack = new Bitmap[18];

        public CDemon()
        {
            for (int i = 0; i < 6; i++)
            {
                idle[i] = new Bitmap("demon-idle-" + i + ".gif");
                idle[i] = new Bitmap(idle[i], 400, 400);
            }

            for (int i = 0; i < 18; i++)
            {
                attack[i] = new Bitmap("demon-attack-" + i + ".gif");
                attack[i] = new Bitmap(attack[i], 400, 400);
            }
        }
    }

    public class CGhoul
    {
        public int X, Y;
        public Bitmap[] right = new Bitmap[8];
        public Bitmap[] left = new Bitmap[8];
        public Bitmap[] die = new Bitmap[9];

        public CGhoul()
        {
            for (int i = 0; i < 8; i++)
            {
                right[i] = new Bitmap("burning-ghoul-right-" + i + ".gif");
                right[i] = new Bitmap(right[i], 200, 200);

                left[i] = new Bitmap("burning-ghoul-left-" + i + ".png");
                left[i] = new Bitmap(left[i], 200, 200);
            }

            for (int i = 0; i < 9; i++)
            {
                die[i] = new Bitmap("wizard-death-" + i + ".png");
                die[i] = new Bitmap(die[i], 200, 200);
            }
        }
    }

    public partial class Form1 : Form
    {
        Bitmap off, background = new Bitmap("background.bmp");        
        Timer T = new Timer();
        Random Rand = new Random();

        CHero Hero = new CHero();
        CWizard Wizard = new CWizard();
        CDemon Demon = new CDemon();
        CGhoul Ghoul = new CGhoul();
        CGhoul Ghoul2 = new CGhoul();

        List<CActor> LSingleB = new List<CActor>();
        List<CActor> LMap = new List<CActor>();
        List<CActor> LFireBall = new List<CActor>();

        CActor lightning = new CActor();
        CActor ladder = new CActor();

        bool heroRun, heroIdle = true, heroRight, heroLeft, heroAttack, heroJump, heroFast, heroDie, heroAttack2, heroIntersect, heroGravity, flagElevator, flagLight, flagUp, flagDown;
        bool wizardIdle = true, wizardAttack, wizardDie, wizardFireBall;
        bool demonIdle = true, demonAttack, demonDie, flagEnter;
        bool ghoulRight = true, ghoulLeft, ghoulDie,   ghoulRight2 = true, ghoulLeft2, ghoulDie2;

        int scroll, runCT, attackCT, idleCT, jumpCT, dieCT, attackCT2, lightCT;
        int wizardIdleCT, wizardAttackCT, wizardDieCT, fireballCT,ct, wizardDead = 0;
        int demonIdleCT, demonAttackCT, demonDead = 0;
        int gRightCT, gLeftCT, gDieCT, ghoulDead = 0, gRightCT2, gLeftCT2, gDieCT2, ghoulDead2 = 0, ghoulNum;

        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;
            this.Load += new EventHandler(Form1_Load);
            this.Paint += new PaintEventHandler(Form1_Paint);
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
            T.Tick += T_Tick;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right) 
            { 
                heroRight = false; 
                heroRun = false;
                heroIdle = true;
            }
            if (e.KeyCode == Keys.Left) 
            { 
                heroLeft = false;
                heroRun = false;
                heroIdle = true;
            }
            if (e.KeyCode == Keys.S)
            {
                heroFast = false;
            }           
        }

        private void T_Tick(object sender, EventArgs e)
        {

            bullet();
            enemyBullet();
            gravity();
            elevator();
            

            if(runCT == 8)
            {
                runCT = 0;
            }


            if(heroRight)
            {
                Rectangle heroRect = new Rectangle(Hero.X, Hero.Y, 231, 190);
                for (int j = 0; j < LMap.Count; j++)
                {
                    Rectangle mapRect = new Rectangle(LMap[j].X, LMap[j].Y, LMap[j].img.Width - 200, LMap[j].img.Height -200);
                    if (heroRect.IntersectsWith(mapRect))
                    {
                        heroIntersect = true;
                    }
                }

                if (heroIntersect == false)
                {
                    if (scroll < 4700)
                    {
                        if (heroFast)
                        {
                            scroll += 35;
                            for (int i = 0; i < LMap.Count; i++)
                            {
                                LMap[i].X -= 35;
                            }
                            Wizard.X -= 35;
                            Demon.X -= 35;
                            Ghoul.X -= 35;
                            Ghoul2.X -= 35;
                            ladder.X -= 35;
                        }
                        else
                        {
                            scroll += 20;
                            for (int i = 0; i < LMap.Count; i++)
                            {
                                LMap[i].X -= 20;
                            }
                            Wizard.X -= 20;
                            Demon.X -= 20;
                            Ghoul.X -= 20;
                            Ghoul2.X -= 20;
                            ladder.X -= 20;
                        }
                    }
                    else
                    {
                        Hero.X += 20;
                    }
                }                
            }
            else if(heroLeft)
            {
                if (scroll > 0)
                {
                    if (heroFast)
                    {
                        scroll -= 35;
                        for (int i = 0; i < LMap.Count; i++)
                        {
                            LMap[i].X += -35;
                        }
                        Wizard.X += 35;
                        Demon.X += 35;
                        Ghoul.X += 35;
                        Ghoul2.X += 35;
                        ladder.X += 35;
                    }
                    else
                    {
                        scroll -= 20;
                        for (int i = 0; i < LMap.Count; i++)
                        {
                            LMap[i].X += 20;
                        }
                        Wizard.X += 20;
                        Demon.X += 20;
                        Ghoul.X += 20;
                        Ghoul2.X += 20;
                        ladder.X += 20;
                    }
                }
                
            }

            if(heroAttack)
            {
                attackCT++;
                if(attackCT == 8)
                {
                    heroAttack = false;
                    attackCT = 0;
                    heroIdle = true;

                    CActor pnn = new CActor();
                    pnn.img = new Bitmap("Ebullet.png");
                    pnn.img = new Bitmap(pnn.img, 80, 45);
                    pnn.X = Hero.X + Hero.attack[7].Width - 100;
                    pnn.Y = Hero.Y + 150;
                    LSingleB.Add(pnn);
                }
            }

            if (heroAttack2)
            {
                attackCT2++;
                if (attackCT2 == 8)
                {
                    heroAttack2 = false;
                    attackCT2 = 0;
                    heroIdle = true;
                    flagLight = true;
                    ghoulNum = nearestEnemy();                   
                    DrawDubb(this.CreateGraphics());
                }
            }

            if (heroIdle)
            {
                idleCT++;
                if(idleCT == 6)
                {
                    idleCT = 0;
                }
            }

            if(heroJump)
            {
                jumpCT++;
                Hero.Y -= 100;
                if(jumpCT == 3)
                {
                    jumpCT = 0;
                    heroJump = false;
                    heroIdle = true;
                }
            }

            
            heroIntersect = false;

            if (wizardDead == 0)
            {
                if (wizardIdle)
                {
                    wizardIdleCT++;
                    if (wizardIdleCT == 5)
                    {
                        wizardIdleCT = 0;
                    }
                }

                if (scroll > 2000 && scroll < 2900)
                {
                    if (ct % 33 == 0)
                    {
                        wizardAttack = true;
                        wizardIdle = false;
                    }
                    ct++;
                    wizardAttackCT++;
                    if (wizardAttackCT == 11)
                    {
                        wizardAttackCT = 0;
                        wizardIdle = true;
                    }
                }
                else if (scroll > 2900)
                {
                    wizardAttack = false;
                    wizardIdle = true;
                }

                if (wizardAttack)
                {
                    wizardAttackCT++;
                    if (wizardAttackCT == 11)
                    {
                        wizardAttack = false;
                        wizardAttackCT = 0;
                        wizardIdle = true;
                        wizardFireBall = true;

                        CActor pnn = new CActor();
                        for (int i = 0; i < 3; i++)
                        {
                            pnn.fireball[i] = Wizard.fireball[i];
                        }
                        pnn.X = Wizard.X;
                        pnn.Y = Wizard.Y + 100;
                        LFireBall.Add(pnn);
                    }
                }

                if (wizardFireBall)
                {
                    fireballCT++;
                    if (fireballCT == 4)
                    {
                        fireballCT = 0;
                    }
                }
            } 
            if(wizardDie)
            {
                wizardDieCT++;
                wizardIdle = false;
                wizardAttack = false;
                if(wizardDieCT == 9)
                {
                    wizardDieCT = 0;
                    wizardDie = false;
                    wizardDead = 2;
                }
            }

            if (demonDead == 0)
            {
                if (demonIdle)
                {
                    demonIdleCT++;
                    if (demonIdleCT == 5)
                    {
                        demonIdleCT = 0;
                    }
                }

                if (scroll > 3300 && scroll < 3520)
                {
                    demonAttack = true;
                    demonIdle = false;
                    if (demonAttack)
                    {
                        demonAttackCT++;
                        if (demonAttackCT == 18)
                        {
                            demonAttackCT = 0;
                        }
                    }
                }
                else if (scroll > 3520 || scroll < 3300)
                {
                    demonAttack = false;
                    demonIdle = true;
                }

                if (demonDie == false && scroll >= 3410 && scroll <= 3510)
                {
                    heroDie = true;
                    heroIdle = false;
                }
            }

            if (ghoulDead == 0)
            {
                if (scroll < 1100)
                {
                    if (ghoulRight)
                    {
                        gRightCT++;
                        Ghoul.X += 15;
                        if (gRightCT == 8)
                        {
                            gRightCT = 0;
                        }
                        if (Ghoul.X >= 1080 - scroll)
                        {
                            gRightCT = 0;
                            ghoulRight = false;
                            ghoulLeft = true;
                        }
                    }

                    if (ghoulLeft)
                    {
                        gLeftCT++;
                        Ghoul.X -= 15;
                        if (gLeftCT == 8)
                        {
                            gLeftCT = 0;
                        }
                        if (Ghoul.X <= 600 - scroll)
                        {
                            gLeftCT = 0;
                            ghoulLeft = false;
                            ghoulRight = true;
                        }
                    }
                }
                else
                {
                    ghoulLeft = false;
                    ghoulRight = false;
                }
            }
            if (ghoulDie)
            {
                gDieCT++;
                ghoulRight = false;
                ghoulLeft = false;
                if (gDieCT == 9)
                {
                    gDieCT = 0;
                    ghoulDie = false;
                    ghoulDead = 2;
                }
            }

            //
            if (ghoulDead2 == 0)
            {
                if (scroll < 1900)
                {
                    if (ghoulRight2)
                    {
                        gRightCT2++;
                        Ghoul2.X += 15;
                        if (gRightCT2 == 8)
                        {
                            gRightCT2 = 0;
                        }
                        if (Ghoul2.X >= 1800 - scroll)
                        {
                            gRightCT2 = 0;
                            ghoulRight2 = false;
                            ghoulLeft2 = true;
                        }
                    }

                    if (ghoulLeft2)
                    {
                        gLeftCT2++;
                        Ghoul2.X -= 15;
                        if (gLeftCT2 == 8)
                        {
                            gLeftCT2 = 0;
                        }
                        if (Ghoul2.X <= 1300 - scroll)
                        {
                            gLeftCT2 = 0;
                            ghoulLeft2 = false;
                            ghoulRight2 = true;
                        }
                    }
                }
                else
                {
                    ghoulLeft2 = false;
                    ghoulRight2 = false;
                }
            }
            if (ghoulDie2)
            {
                gDieCT2++;
                ghoulRight2 = false;
                ghoulLeft2 = false;
                if (gDieCT2 == 9)
                {
                    gDieCT2 = 0;
                    ghoulDie2 = false;
                    ghoulDead2 = 2;
                }
            }
            //
            if(flagUp)
            {
                Hero.Y -= 50;
                if(Hero.Y <= ladder.Y - 220)
                {
                    flagUp = false;
                }
            }

            if(flagDown)
            {
                Hero.Y += 50;
                if (Hero.Y >= 420 /*|| heroGravity*//*Hero.Y >= ladder.Y + 150*/)
                {
                    flagDown = false;
                }

                /*Hero.Y >= 430*/
            }

            if (demonDie)
            {
                demonIdle = false;
                demonAttack = false;
                demonDie = false;
                demonDead = 2;
            }

            if (heroDie)
            {
                dieCT++;
                if (dieCT == 8)
                {
                    dieCT = 0;
                    heroDie = false;
                    heroIdle = true;
                }
            }



            DrawDubb(this.CreateGraphics());
            runCT++;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Right)
            {
                heroRun = true;
                heroRight = true;
                
            }
            else if(e.KeyCode == Keys.Left)
            {
                heroRun = true;
                heroLeft = true;
            }

            if(e.KeyCode == Keys.A)
            {
                heroAttack = true;
                heroIdle = false;
                heroRun = false;                
            }

            if(e.KeyCode == Keys.Space)
            {
                if (Hero.Y >= 430 || heroGravity)
                {
                    heroJump = true;
                    heroIdle = false;
                }
            }

            if(e.KeyCode == Keys.S)
            {
                heroFast = true;
            }

            if (e.KeyCode == Keys.D)
            {
                heroDie = true;
                heroIdle = false;
            }

            if (e.KeyCode == Keys.Z)
            {
                heroAttack2 = true;
                heroIdle = false;
                heroRun = false;
                if (ghoulLeft) { ghoulRight = false; }
                if (ghoulRight) { ghoulLeft = false; }
            }

            if(e.KeyCode == Keys.Up)
            {
                if (scroll > 3000 && scroll < 3150)
                {
                flagUp = true;
                }
            }

            if(e.KeyCode == Keys.Down)
            {
                if (scroll > 3000 && scroll < 3150)
                {
                    flagDown = true;
                }
            }

            if(e.KeyCode == Keys.Enter)
            {
                flagEnter = true;
            }
            //DrawDubb(this.CreateGraphics());
        }

        void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(e.Graphics);
        }

        void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(ClientSize.Width, ClientSize.Height);
            Hero.X = 100;
            Hero.Y = 430;

            Wizard.X = 3100;
            Wizard.Y = -30;

            Demon.X = 3650;
            Demon.Y = -120;

            Ghoul.X = 620;
            Ghoul.Y = 530;

            Ghoul2.X = 1800;
            Ghoul2.Y = 530;

            //CreateHero();
            CreateMap();
            T.Start();
            //PlayMusic("CityStomper.wav");
        }

        void CreateMap()
        {
            int ax = 500;
            for(int i=0;i<3;i++)
            {
                CActor pnn = new CActor();
                pnn.X = ax;
                pnn.Y = 545;
                pnn.img = new Bitmap("ground.png");
                LMap.Add(pnn);
                ax += 700;
            }

            CActor pnn1 = new CActor();
            pnn1.X = ax - 500;
            pnn1.Y = 335;
            pnn1.img = new Bitmap("ground1.png");
            LMap.Add(pnn1);

            pnn1 = new CActor();
            pnn1.X = ax - 120;
            pnn1.Y = 220;
            pnn1.img = new Bitmap("ground.png");
            LMap.Add(pnn1);

            ax -= 120;
            for (int i = 0; i < 3; i++)
            {
                pnn1 = new CActor();
                pnn1.X = ax + LMap[2].img.Width - 78;
                pnn1.Y = 203;
                pnn1.img = new Bitmap("ground1.png");
                LMap.Add(pnn1);
                ax += LMap[3].img.Width - 175;
            }

            int ay = 545;
            for (int i = 0; i < 5; i++)
            {
                CActor pnn = new CActor();
                pnn.X = ax + 500;
                pnn.Y = ay;
                pnn.img = new Bitmap("ground.png");
                LMap.Add(pnn);
                //ax += pnn.img.Width - 75;
                ay -= 80;
            }

            CActor pnn2 = new CActor();
            pnn2.X = ax + 350;
            pnn2.Y = 660;
            pnn2.img = new Bitmap("stone.bmp");
            LMap.Add(pnn2);

            lightning.img = new Bitmap("Lightning.png");
            lightning.img.MakeTransparent(lightning.img.GetPixel(0, 0));

            ladder.img = new Bitmap("ladder.png");
            ladder.img = new Bitmap(ladder.img , 100,350);
            ladder.img.MakeTransparent(ladder.img.GetPixel(0, 0));
            ladder.X = 3315;
            ladder.Y = 210;
        }

        void DrawScene(Graphics g)
        {
            g.Clear(Color.White);

            Rectangle rcDst = new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height + 510);
            Rectangle rcScr = new Rectangle(scroll, 0, this.ClientSize.Width, this.ClientSize.Height);
            g.DrawImage(background, rcDst, rcScr, GraphicsUnit.Pixel);

            SolidBrush B = new SolidBrush(Color.DarkRed);

            for (int i = 0; i < LSingleB.Count; i++)
            {
                g.DrawImage(LSingleB[i].img, LSingleB[i].X+=40, LSingleB[i].Y);
            }

            for (int i = 0; i < LFireBall.Count; i++)
            {
                g.DrawImage(LFireBall[i].fireball[0], LFireBall[i].X -= 40, LFireBall[i].Y);
            }

            if (heroRun && heroJump == false)
            {
                g.DrawImage(Hero.run[runCT], Hero.X, Hero.Y);
            }

            else if(heroIdle)
            {
                g.DrawImage(Hero.idle[idleCT], Hero.X, Hero.Y);
            }

            else if(heroAttack)
            {
                g.DrawImage(Hero.attack[attackCT -1], Hero.X, Hero.Y);
            }
            else if (heroAttack2)
            {
                g.DrawImage(Hero.attack2[attackCT2 - 1], Hero.X, Hero.Y);
            }
            else if (heroJump)
            {
                g.DrawImage(Hero.jump[jumpCT - 1], Hero.X, Hero.Y);
            }
            else if (heroDie)
            {
                g.DrawImage(Hero.die[dieCT - 1], Hero.X, Hero.Y);
            }

            for(int i=0;i<LMap.Count;i++)
            {
                g.DrawImage(LMap[i].img, LMap[i].X, LMap[i].Y);
            }

            if (wizardIdle)
            {
                g.DrawImage(Wizard.idle[wizardIdleCT], Wizard.X, Wizard.Y);
            }

            else if(wizardAttack)
            {
                g.DrawImage(Wizard.attack[wizardAttackCT - 1], Wizard.X, Wizard.Y);
            }

            else if(wizardDie)
            {
                g.DrawImage(Wizard.die[wizardDieCT], Wizard.X, Wizard.Y);
            }

            /*if(wizardFireBall)
            {
                g.DrawImage(LFireBall[0].fireball[fireballCT - 1], LFireBall[0].X -= 40, LFireBall[0].Y);
            }*/

            if(demonIdle)
            {
                g.DrawImage(Demon.idle[demonIdleCT], Demon.X + 150, Demon.Y);
            }

            if (demonAttack)
            {
                g.DrawImage(Demon.attack[demonAttackCT], Demon.X, Demon.Y);
            }

            if(ghoulRight)
            {
                g.DrawImage(Ghoul.right[gRightCT], Ghoul.X, Ghoul.Y);
            }
            else if(ghoulLeft)
            {
                g.DrawImage(Ghoul.left[gLeftCT], Ghoul.X, Ghoul.Y);
            }
            else if (ghoulDie)
            {
                g.DrawImage(Ghoul.die[gDieCT], Ghoul.X, Ghoul.Y);
            }

            //
            if (ghoulRight2)
            {
                g.DrawImage(Ghoul2.right[gRightCT2], Ghoul2.X, Ghoul2.Y);
            }
            else if (ghoulLeft2)
            {
                g.DrawImage(Ghoul2.left[gLeftCT2], Ghoul2.X, Ghoul2.Y);
            }
            else if (ghoulDie2)
            {
                g.DrawImage(Ghoul2.die[gDieCT2], Ghoul2.X, Ghoul2.Y);
            }
            //

            if (flagLight)
            {
                lightCT++;
                if (lightCT <= 5)
                {
                    g.DrawImage(lightning.img, lightning.X, lightning.Y);

                    if (ghoulNum == 1) { ghoulDie = true; }
                    if (ghoulNum == 2) { ghoulDie2 = true; }
                    ghoulLeft = false;
                    ghoulRight = false;
                }
                else
                {
                    lightCT = 0;
                    flagLight = false;
                }
            }

            g.DrawImage(ladder.img, ladder.X, ladder.Y);

            Font Fn = new Font("System", 20);
            g.DrawString("dx: " + scroll + "  " + "dy: " + Hero.Y + "" +
                "                                                                                                      " +
                "                              " + "Bullet: A | Lightning : Z | Fast: S", Fn, Brushes.Black, 0, 0);

            if(scroll > 4000)
            {
                g.Clear(Color.Black);
                Font win = new Font("System", 120);
                g.DrawString("You Win!", win, Brushes.Green, ClientSize.Width / 2 - 450, ClientSize.Height / 2 - 80);               
            }
        }

        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }

        void PlayMusic(string musicName)
        {
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + musicName;
            player.Play();
        }

        void bullet()
        {
            for (int i = 0; i < LSingleB.Count; i++)
            {
                if (LSingleB[i].X + LSingleB[i].img.Width > ClientSize.Width)
                {
                    LSingleB.Remove(LSingleB[i]);
                }

                else if (LSingleB[i].X > Wizard.X && LSingleB[i].X < Wizard.X + Wizard.idle[0].Width && LSingleB[i].Y > Wizard.Y && LSingleB[i].Y < Wizard.Y  + Wizard.idle[0].Width)
                {
                    LSingleB.Remove(LSingleB[i]);
                    wizardDie = true;
                }

                else if (LSingleB[i].X > Demon.X && LSingleB[i].X < Demon.X + Demon.idle[0].Width && LSingleB[i].Y > Demon.Y && LSingleB[i].Y < Demon.Y + Demon.idle[0].Width)
                {
                    LSingleB.Remove(LSingleB[i]);
                    demonDie = true;
                }

                else if (LSingleB[i].X > Ghoul.X && LSingleB[i].X < Ghoul.X + Ghoul.right[0].Width && LSingleB[i].Y > Ghoul.Y && LSingleB[i].Y < Ghoul.Y + Ghoul.right[0].Width)
                {
                    LSingleB.Remove(LSingleB[i]);
                    ghoulDie = true;
                }

                else if (LSingleB[i].X > Ghoul2.X && LSingleB[i].X < Ghoul2.X + Ghoul2.right[0].Width && LSingleB[i].Y > Ghoul2.Y && LSingleB[i].Y < Ghoul2.Y + Ghoul2.right[0].Width)
                {
                    LSingleB.Remove(LSingleB[i]);
                    ghoulDie2 = true;
                }
            }
        }

        void enemyBullet()
        {
            for (int i = 0; i < LFireBall.Count; i++)
            {
                if (LFireBall[i].X > Hero.X && LFireBall[i].X < Hero.X + Hero.idle[0].Width - 100 && LFireBall[i].Y > Hero.Y && LFireBall[i].Y < Hero.Y + Hero.idle[0].Width)
                {
                    LFireBall.Remove(LFireBall[i]);
                    heroDie = true;
                    heroIdle = false;
                }
            }
        }

        void elevator()
        {
            if (Hero.X + 200 > LMap[13].X && Hero.X - 470 < LMap[13].X + LMap[13].img.Width && LMap[13].Y > 240)
            {
                flagElevator = true;
            }
            else
            {
                if (LMap[13].Y > 670)
                {
                    LMap[13].Y += 50;
                }
            }

            if (flagElevator)
            {
                LMap[13].Y -= 50;
                Hero.Y -= 50;

                if (LMap[13].Y < 240)
                {
                    flagElevator = false;
                }
            }
        }

        int nearestEnemy()
        {
            int Enemy1 = Math.Abs(Hero.X - Ghoul.X);
            int Enemy2 = Math.Abs(Hero.X - Ghoul2.X);
            int num;
            if(Enemy1 < Enemy2)
            {
                lightning.X = Enemy1 + 90;
                num = 1;
            }
            else
            {
                lightning.X = Enemy2 + 90;
                num = 2;
            }

            if(ghoulDie)
            {
                num = 2;
            }
            if(ghoulDie2)
            {
                num = 1;
            }
            lightning.Y = -60;
            return num;
        }

        void gravity()
        {

            if(Hero.X + 200> LMap[0].X && Hero.X + 200 < LMap[0].X + LMap[0].img.Width)
            {
                if(Hero.Y + 300 < LMap[0].Y)
                {
                    Hero.Y += 30;
                }
                heroGravity = true;
            }
            else if (Hero.X + 200 > LMap[1].X && Hero.X + 200 < LMap[1].X + LMap[1].img.Width)
            {
                if (Hero.Y + 300 < LMap[1].Y)
                {
                    Hero.Y += 30;
                }
                heroGravity = true;
            }
            else if (Hero.X + 200 > LMap[2].X && Hero.X + 200 < LMap[2].X + LMap[2].img.Width)
            {
                if (Hero.Y + 300 < LMap[2].Y)
                {
                    Hero.Y += 30;
                }
                heroGravity = true;
            }
            else if (Hero.X + 200 > LMap[3].X && Hero.X + 300 < LMap[3].X + LMap[3].img.Width)
            {
                if (Hero.Y + 292 < LMap[3].Y)
                {
                    Hero.Y += 30;
                }
                heroGravity = true;
            }
            else if (Hero.X + 200 > LMap[4].X && Hero.X - 470 - ladder.img.Width< LMap[4].X + LMap[4].img.Width)
            {
                if (Hero.Y + 300 < LMap[4].Y)
                {
                    Hero.Y += 30;
                }
                heroGravity = true;
            }
            else if (Hero.X + 200 > LMap[13].X && Hero.X + 30< LMap[13].X + LMap[13].img.Width)
            {
                if (Hero.Y + 300 < LMap[13].Y)
                {
                    Hero.Y += 30;
                }
                heroGravity = true;
            }
            //
            /*else if (Hero.X> ladder.X && Hero.X <ladder.X + ladder.img.Width)
            {
                if (Hero.Y <ladder.Y)
                {
                    //Hero.Y += 30;
                }
                heroGravity = true;
            }*/
            else
            {
                heroGravity = false;
                if (Hero.Y < 430)
                {
                    Hero.Y += 30;
                }
            }
        }
    }
}
