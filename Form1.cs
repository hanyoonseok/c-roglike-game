using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Final
{
    public partial class Form1 : Form
    {
        Random r = new Random(DateTime.Now.Millisecond);
        public Form1()
        {
            InitializeComponent();
            //Info[] info = new Info[10];
        }
        Form3 form3 = new Form3(); // 1스테이지
        Form4 form4 = new Form4();
        Form5 form5 = new Form5();
        Form6 form6 = new Form6();
        Form7 form7 = new Form7();
        Form8 form8 = new Form8();
        Form9 form9 = new Form9();
        Form10 form10 = new Form10();
        Form11 form11 = new Form11();
        Form12 form12 = new Form12(); // 10스테이지
        int stgno = 0; //info index용 stgnum
        int exitS = 0; //출구가 있는 스테이지 넘버
        Info[] info = new Info[10];

        //Button[,] button;
        public void setStage(Form form, int first)
        {
            info[stgno].gameRow = (int)r.Next(3, 21); //세로로 커지는거
            info[stgno].gameCol = (int)r.Next(3, 21); //가로로 커지는거

            FlowLayoutPanel flp = new FlowLayoutPanel();
            info[stgno].button = new Button[info[stgno].gameRow, info[stgno].gameCol];
            
            form.KeyPreview = true;
            form.KeyDown += new KeyEventHandler(movePlayer);
            form.Size = new Size(info[stgno].gameCol * 32, info[stgno].gameRow * 32 + 70);
            flp.Size = new Size(info[stgno].gameCol * 26+5, info[stgno].gameRow * 26+30);

            for(int i=0; i< info[stgno].gameRow; i++)
            {
                for(int j=0; j< info[stgno].gameCol; j++)
                {
                    info[stgno].button[i, j] = new Button();
                    info[stgno].button[i, j].FlatStyle = FlatStyle.Flat;
                    info[stgno].button[i, j].Size = new Size(20, 20);
                    info[stgno].button[i, j].Tag = 0;
                    flp.Controls.Add(info[stgno].button[i, j]);
                }
            }
            setPortal(form, info[stgno].button, info[stgno].gameRow, info[stgno].gameCol);
            setMonster(form, info[stgno].button, info[stgno].gameRow, info[stgno].gameCol);

            info[stgno].stg = new Label();
            info[stgno].life = new Label();
            info[stgno].stg.Size = new Size(35, 30);
            info[stgno].life.Size = new Size(35, 30);
            info[stgno].stg.Font = new Font("굴림", 8);
            info[stgno].life.Font = new Font("굴림", 8);
            info[stgno].stg.Text = "Stg:" + (stgno+1).ToString();
            info[stgno].life.Text = "Life:" + Global.lifePoint.ToString();

            flp.Controls.Add(info[stgno].stg);
            flp.Controls.Add(info[stgno].life);
            form.Controls.Add(flp);

        }
        
        private void setPlayer(Form form, Button[,] button, int col, int row) //처음에 세팅할 때
        {
            int co = (int)r.Next(0, col);
            int ro = (int)r.Next(0, row);
            if ((int)info[0].button[co, ro].Tag==2 && (int)info[0].button[co, ro].Tag == 3 && (int)info[0].button[co, ro].Tag == 4)
                setPlayer(form, button, col, row);
            else
            {
                info[0].playerCol = co;
                info[0].playerRow = ro;

                button[co, ro].Text = "■";
                button[co, ro].Tag = 1;
            }

        }
        
        private void setPortal(Form form, Button[,] button, int col, int row)
        {
            int portalNum = (int)r.Next(1, 5); //1~4 무작위 수 
            int[] rand1 = new int[portalNum]; //포탈 무작위 행
            int[] rand2 = new int[portalNum]; //포탈 무작위 열
            Global.stgList.RemoveAt(index:stgno); //현재 위치한 스테이지를 제외하고 난수 생성 위해
            int[] nextStg = new int[portalNum];
            for(int i=0; i<portalNum; i++) //현재 스테이지 제외하고 무작위로 스테이지받음
            {
                int a = (int)r.Next(0, Global.stgList.Count);
                nextStg[i] = Global.stgList[a];
            }
            //MessageBox.Show(nextStg.ToString());
            for (int i = 0; i < portalNum; i++)
            {
                rand1[i] = (int)r.Next(0, col);
                rand2[i] = (int)r.Next(0, row);
                for (int j = 0; j < i; j++)
                {
                    if (rand1[j] == rand1[i] && rand2[j] == rand2[i])
                    {
                        i--;
                        break;
                    }
                    else if ((int)button[rand1[i], rand2[i]].Tag != 0)
                    {
                        i--;
                        break;
                    }
                }
            }
            for (int i = 0; i < portalNum; i++)
            {
                    button[rand1[i], rand2[i]].Text = "●";
                    button[rand1[i], rand2[i]].Tag = 2;
                    button[rand1[i], rand2[i]].Name = nextStg[i].ToString();    
            }
            Global.stgList.Insert(index: stgno, item:stgno);
        }
        private int search(int randM, int[] arr, Button[,] button, int randPortal) //플레이어 배치 시 포탈이나 몬스터 타일을 제외시킴
        {
            switch (randM)
            {
                case 0:
                    if (arr[randPortal] - 1 >= 0 && (int)button[arr[randPortal] - 1, arr[randPortal + 4]].Tag != 2 && (int)button[arr[randPortal] - 1, arr[randPortal + 4]].Tag != 3)
                    {
                        button[arr[randPortal] - 1, arr[randPortal + 4]].Text = "■";
                        button[arr[randPortal] - 1, arr[randPortal + 4]].Tag = 1;
                        info[stgno].playerCol = arr[randPortal] - 1;
                        info[stgno].playerRow = arr[randPortal + 4];
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                    break;
                case 1:
                    if (arr[randPortal + 4] + 1 < info[stgno].gameCol && (int)button[arr[randPortal], arr[randPortal + 4] + 1].Tag != 2 && (int)button[arr[randPortal], arr[randPortal + 4] + 1].Tag != 3)
                    {
                        button[arr[randPortal], arr[randPortal + 4] + 1].Text = "■";
                        button[arr[randPortal], arr[randPortal + 4] + 1].Tag = 1;
                        info[stgno].playerCol = arr[randPortal];
                        info[stgno].playerRow = arr[randPortal + 4] + 1;
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                    break;
                case 2: //남
                    if (arr[randPortal] + 1 < info[stgno].gameRow && (int)button[arr[randPortal] + 1, arr[randPortal + 4]].Tag != 2 && (int)button[arr[randPortal] + 1, arr[randPortal + 4]].Tag != 3)
                    {
                        button[arr[randPortal] + 1, arr[randPortal + 4]].Text = "■";
                        button[arr[randPortal] + 1, arr[randPortal + 4]].Tag = 1;
                        info[stgno].playerCol = arr[randPortal] + 1;
                        info[stgno].playerRow = arr[randPortal + 4];
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                    break;
                case 3: //서
                    if (arr[randPortal + 4] - 1 >= 0 && (int)button[arr[randPortal], arr[randPortal + 4] - 1].Tag != 2 && (int)button[arr[randPortal], arr[randPortal + 4] - 1].Tag != 3)
                    {
                        button[arr[randPortal], arr[randPortal + 4] - 1].Text = "■";
                        button[arr[randPortal], arr[randPortal + 4] - 1].Tag = 1;
                        info[stgno].playerCol = arr[randPortal];
                        info[stgno].playerRow = arr[randPortal + 4] - 1;
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                    break;
            }
            return 0;
        }
        private void PortalPlayer(int a)
        {
            List<Button> btnlist = new List<Button>(); //포탈인 버튼을 담을 리스트
            int[] arr = new int[8]; //포탈인 버튼의 row,col을 담음
            int cnt = 0; //추적용 인덱스
            int randMove = (int)r.Next(0, 4); //포탈 중심으로 어느 방향으로 움직일지
            int randPortal = (int)r.Next(0, btnlist.Count); //리스트에 담아둔 포탈중 무작위 인덱스

            for (int i = 0; i < info[a].gameRow; i++)
            {
                for (int j = 0; j < info[a].gameCol; j++)
                {
                    if ((int)info[a].button[i, j].Tag == 2) //해당 스테이지의 포탈 버튼을 찾음
                    {
                        btnlist.Add(info[a].button[i, j]); //btnlist에 추가
                        arr[cnt] = i; // 포탈 수가 최대 4개니까 0~3은 row, 4~7은 col
                        arr[cnt + 4] = j; //
                    }
                }
            }
            //int randM, int [] arr, Button [,] button, int randPortal
            switch (randMove)
            {
                case 0: //북
                    if (search(randMove, arr, info[a].button, randPortal) != 1)
                    {
                        search(randMove, arr, info[a].button, randPortal);
                    }

                    break;
                case 1: //동
                    if (search(randMove, arr, info[a].button, randPortal) != 1)
                    {
                        search(randMove, arr, info[a].button, randPortal);
                    }
                    break;
                case 2: //남
                    if (search(randMove, arr, info[a].button, randPortal) != 1)
                    {
                        search(randMove, arr, info[a].button, randPortal);
                    }
                    break;
                case 3: //서
                    if (search(randMove, arr, info[a].button, randPortal) != 1)
                    {
                        search(randMove, arr, info[a].button, randPortal);
                    }
                    break;
            }
        }
        private void setMonster(Form form, Button[,] button, int col, int row)
        {
            int idx = col * row / 5;
            int monsterNum = (int)r.Next(0, idx); //0~버튼수의 20% 무작위 수 
            int[] rand1 = new int[monsterNum]; //몬스터 무작위 행
            int[] rand2 = new int[monsterNum]; //몬스터 무작위 열
            void re()
            {
                for (int i = 0; i < monsterNum; i++)
                {
                    rand1[i] = (int)r.Next(0, col);
                    rand2[i] = (int)r.Next(0, row);
                    for (int j = 0; j < i; j++)
                    {
                        if (rand1[j] == rand1[i] && rand2[j]== rand2[i])
                        {
                            i--;
                            break;
                        }
                        else if((int)button[rand1[i], rand2[i]].Tag != 0)
                        {
                            i--;
                            break;
                        }
                    }
                }
            }
            re();
            
            for (int i = 0; i < monsterNum; i++)
            {
                    button[rand1[i], rand2[i]].Text = "◈";
                    button[rand1[i], rand2[i]].Tag = 3;
            }
        }
        private void setExit()
        {
            int a = (int)r.Next(1, stgno + 1); //출구가 생성될 시작 스테이지를 제외한 무작위 스테이지
            int rand1 = (int)r.Next(0, info[a].gameRow);
            int rand2 = (int)r.Next(0, info[a].gameCol);
            int b;

            if ((int)info[a].button[rand1, rand2].Tag != 0)
            {
                setExit();
            }
            else
            {
                info[a].button[rand1, rand2].Tag = 4;
                info[a].button[rand1, rand2].Text = "◎";
            }
            void re()
            {
                b = (int)r.Next(1, stgno + 1); //출구로 향하는 포탈이 반드시 하나는 존재
            }
            re();
            if (b == a) //출구가 있는 스테이지에 생성됨을 막음
                re();
            else
            {
                for (int i = 0; i < info[b].gameRow; i++)
                {
                    for (int j = 0; j < info[b].gameCol; j++)
                    {
                        if((int)info[b].button[i,j].Tag==2)
                        {
                            info[b].button[i, j].Name = a.ToString();
                            break;
                        }
                    }
                }
            }
        }
        private void exitForm(int a)
        {
            if (a == 0)
            {
                form3.Visible = false;
                form3.Hide();
            }
            else if (a == 1)
            {
                form4.Visible = false;
                form4.Hide();
            }
            else if (a == 2)
            {
                form5.Visible = false;
                form5.Hide();
            }
            else if (a == 3)
            {
                form6.Visible = false;
                form6.Hide();
            }
            else if (a == 4)
            {
                form7.Visible = false;
                form7.Hide();
            }
            else if (a == 5)
            {
                form8.Visible = false;
                form8.Hide();
            }
            else if (a == 6)
            {
                form9.Visible = false;
                form9.Hide();
            }
            else if (a == 7)
            {
                form10.Visible = false;
                form10.Hide();
            }
            else if (a == 8)
            {
                form11.Visible = false;
                form11.Hide();
            }
            else if (a == 9)
            {
                form12.Visible = false;
                form12.Hide();
            }
        }
        private void InitInfo()
        {
            info[stgno].stg.Text = "Stg:" + (stgno+1).ToString();
            info[stgno].life.Text = "Life:" + Global.lifePoint.ToString();
        }
        private void movePlayer(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    if(info[stgno].playerCol -1 >=0)
                    {
                        if ((int)info[stgno].button[info[stgno].playerCol - 1, info[stgno].playerRow].Tag != 3 ) //일반 타일이거나 포탈이거나 탈출구면
                        {
                            
                            if ((int)info[stgno].button[info[stgno].playerCol - 1, info[stgno].playerRow].Tag == 2)//포탈일 때
                            {
                                int a = stgno;
                                stgno= Convert.ToInt32(info[stgno].button[info[stgno].playerCol - 1, info[stgno].playerRow].Name);
                                switch (stgno)
                                {
                                    case 0:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form3.ShowDialog();
                                        form3.Show();
                                        form3.Visible = true;
                                        break;
                                    case 1:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form4.ShowDialog();
                                        form4.Show();
                                        form4.Visible = true;
                                        break;
                                    case 2:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form5.ShowDialog();
                                        form5.Show();
                                        form5.Visible = true;
                                        break;
                                    case 3:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form6.ShowDialog();
                                        form6.Show();
                                        form6.Visible = true;
                                        break;
                                    case 4:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form7.ShowDialog();
                                        form7.Show();
                                        form7.Visible = true;
                                        break;
                                    case 5:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form8.ShowDialog();
                                        form8.Show();
                                        form8.Visible = true;
                                        break;
                                    case 6:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form9.ShowDialog();
                                        form9.Show();
                                        form9.Visible = true;
                                        break;
                                    case 7:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form10.ShowDialog();
                                        form10.Show();
                                        form10.Visible = true;
                                        break;
                                    case 8:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form11.ShowDialog();
                                        form11.Show();
                                        form11.Visible = true;
                                        break;
                                    case 9:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form12.ShowDialog();
                                        form12.Show();
                                        form12.Visible = true;
                                        break;
                                }
                            }
                            else if((int)info[stgno].button[info[stgno].playerCol - 1, info[stgno].playerRow].Tag == 4)
                            {
                                Form2 form2 = new Form2("Win!", "Conguratulation...!");
                                this.Visible = false;
                                form2.ShowDialog();
                            }
                            else //일반 타일일 때
                            {
                                info[stgno].button[info[stgno].playerCol, info[stgno].playerRow].Text = "";
                                info[stgno].button[info[stgno].playerCol, info[stgno].playerRow].Tag = 0;
                                info[stgno].button[info[stgno].playerCol - 1, info[stgno].playerRow].Text = "■";
                                info[stgno].button[info[stgno].playerCol - 1, info[stgno].playerRow].Tag = 1;
                                info[stgno].playerCol--;
                            }
                        }
                        moveMonster();
                    }
                    InitInfo();
                    break;
                case Keys.S:
                    if (info[stgno].playerCol + 1 < info[stgno].gameRow)
                    {
                        if ((int)info[stgno].button[info[stgno].playerCol + 1, info[stgno].playerRow].Tag == 0 || (int)info[stgno].button[info[stgno].playerCol + 1, info[stgno].playerRow].Tag == 2 || (int)info[stgno].button[info[stgno].playerCol + 1, info[stgno].playerRow].Tag == 4)
                        {
                            if ((int)info[stgno].button[info[stgno].playerCol + 1, info[stgno].playerRow].Tag == 2)//포탈일 때
                            {
                                int a = stgno;
                                stgno = Convert.ToInt32(info[stgno].button[info[stgno].playerCol + 1, info[stgno].playerRow].Name);
                                switch (stgno)
                                {
                                    case 0:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form3.ShowDialog();
                                        form3.Visible = true;
                                        break;
                                    case 1:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form4.ShowDialog();
                                        form4.Visible = true;
                                        break;
                                    case 2:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form5.ShowDialog();
                                        form5.Visible = true;
                                        break;
                                    case 3:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form6.ShowDialog();
                                        form6.Visible = true;
                                        break;
                                    case 4:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form7.ShowDialog();
                                        form7.Visible = true;
                                        break;
                                    case 5:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form8.ShowDialog();
                                        form8.Visible = true;
                                        break;
                                    case 6:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form9.ShowDialog();
                                        form9.Visible = true;
                                        break;
                                    case 7:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form10.ShowDialog();
                                        form10.Visible = true;
                                        break;
                                    case 8:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form11.ShowDialog();
                                        form11.Visible = true;
                                        break;
                                    case 9:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form12.ShowDialog();
                                        form12.Visible = true;
                                        break;
                                }
                            }
                            else if ((int)info[stgno].button[info[stgno].playerCol + 1, info[stgno].playerRow].Tag == 4)
                            {
                                Form2 form2 = new Form2("Win!", "Conguratulation...!");
                                this.Visible = false;
                                form2.ShowDialog();
                            }
                            else
                            {
                                info[stgno].button[info[stgno].playerCol, info[stgno].playerRow].Text = "";
                                info[stgno].button[info[stgno].playerCol, info[stgno].playerRow].Tag = 0;
                                info[stgno].button[info[stgno].playerCol + 1, info[stgno].playerRow].Text = "■";
                                info[stgno].button[info[stgno].playerCol + 1, info[stgno].playerRow].Tag = 1;
                                info[stgno].playerCol++;
                            }
                        }
                        moveMonster();
                    }
                    InitInfo();
                    break;
                case Keys.A:
                    if (info[stgno].playerRow - 1 >= 0)
                    {
                        if ((int)info[stgno].button[info[stgno].playerCol, info[stgno].playerRow - 1].Tag == 0 || (int)info[stgno].button[info[stgno].playerCol, info[stgno].playerRow - 1].Tag == 2 || (int)info[stgno].button[info[stgno].playerCol, info[stgno].playerRow - 1].Tag == 4)
                        {
                            
                            if ((int)info[stgno].button[info[stgno].playerCol , info[stgno].playerRow-1].Tag == 2)//포탈일 때
                            {
                                int a = stgno;
                                stgno = Convert.ToInt32(info[stgno].button[info[stgno].playerCol, info[stgno].playerRow-1].Name);
                                switch (stgno)
                                {
                                    case 0:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form3.ShowDialog();
                                        form3.Visible = true;
                                        break;
                                    case 1:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form4.ShowDialog();
                                        form4.Visible = true;
                                        break;
                                    case 2:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form5.ShowDialog();
                                        form5.Visible = true;
                                        break;
                                    case 3:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form6.ShowDialog();
                                        form6.Visible = true;
                                        break;
                                    case 4:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form7.ShowDialog();
                                        form7.Visible = true;
                                        break;
                                    case 5:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form8.ShowDialog();
                                        form8.Visible = true;
                                        break;
                                    case 6:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form9.ShowDialog();
                                        form9.Visible = true;
                                        break;
                                    case 7:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form10.ShowDialog();
                                        form10.Visible = true;
                                        break;
                                    case 8:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form11.ShowDialog();
                                        form11.Visible = true;
                                        break;
                                    case 9:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form12.ShowDialog();
                                        form12.Visible = true;
                                        break;
                                }
                            }
                            else if ((int)info[stgno].button[info[stgno].playerCol, info[stgno].playerRow-1].Tag == 4)
                            {
                                Form2 form2 = new Form2("Win!", "Conguratulation...!");
                                this.Visible = false;
                                form2.ShowDialog();
                            }
                            else
                            {
                                info[stgno].button[info[stgno].playerCol, info[stgno].playerRow].Text = "";
                                info[stgno].button[info[stgno].playerCol, info[stgno].playerRow].Tag = 0;
                                info[stgno].button[info[stgno].playerCol, info[stgno].playerRow - 1].Text = "■";
                                info[stgno].button[info[stgno].playerCol, info[stgno].playerRow - 1].Tag = 1;
                                info[stgno].playerRow--;
                            }
                        }
                        moveMonster();
                    }
                    InitInfo();
                    break;
                case Keys.D:
                    if (info[stgno].playerRow +1 < info[stgno].gameCol)
                    {
                        if ((int)info[stgno].button[info[stgno].playerCol, info[stgno].playerRow + 1].Tag == 0 || (int)info[stgno].button[info[stgno].playerCol, info[stgno].playerRow + 1].Tag == 2 || (int)info[stgno].button[info[stgno].playerCol, info[stgno].playerRow + 1].Tag == 4)
                        {
                            if ((int)info[stgno].button[info[stgno].playerCol, info[stgno].playerRow+1].Tag == 2)//포탈일 때
                            {
                                int a = stgno;
                                stgno = Convert.ToInt32(info[stgno].button[info[stgno].playerCol, info[stgno].playerRow+1].Name);
                                switch (stgno)
                                {
                                    case 0:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form3.ShowDialog();
                                        form3.Visible = true;
                                        break;
                                    case 1:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form4.ShowDialog();
                                        form4.Visible = true;
                                        break;
                                    case 2:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form5.ShowDialog();
                                        form5.Visible = true;
                                        break;
                                    case 3:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form6.ShowDialog();
                                        form6.Visible = true;
                                        break;
                                    case 4:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form7.ShowDialog();
                                        form7.Visible = true;
                                        break;
                                    case 5:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form8.ShowDialog();
                                        form8.Visible = true;
                                        break;
                                    case 6:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form9.ShowDialog();
                                        form9.Visible = true;
                                        break;
                                    case 7:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form10.ShowDialog();
                                        form10.Visible = true;
                                        break;
                                    case 8:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form11.ShowDialog();
                                        form11.Visible = true;
                                        break;
                                    case 9:
                                        exitForm(a);
                                        PortalPlayer(stgno);
                                        //form12.ShowDialog();
                                        form12.Visible = true;
                                        break;
                                }
                            }
                            else if ((int)info[stgno].button[info[stgno].playerCol, info[stgno].playerRow+1].Tag == 4)
                            {
                                Form2 form2 = new Form2("Win!", "Conguratulation...!");
                                this.Visible = false;
                                form2.ShowDialog();
                            }
                            else
                            {
                                info[stgno].button[info[stgno].playerCol, info[stgno].playerRow].Text = "";
                                info[stgno].button[info[stgno].playerCol, info[stgno].playerRow].Tag = 0;
                                info[stgno].button[info[stgno].playerCol, info[stgno].playerRow + 1].Text = "■";
                                info[stgno].button[info[stgno].playerCol, info[stgno].playerRow + 1].Tag = 1;
                                info[stgno].playerRow++;
                            } 
                        }
                        moveMonster();
                    }
                    InitInfo();
                    break;
                    //MessageBox.Show(stgno.ToString());
            }
        }
        
        private void moveMonster()
        {
            for (int i = 0; i < info[stgno].gameRow; i++) //게임판에 대해서 검사
            {
                for (int j = 0; j < info[stgno].gameCol; j++)
                {
                    int randMove = (int)r.Next(0, 8);
                    if ((int)info[stgno].button[i, j].Tag == 3) //만약 버튼이 몬스터태그이면 실행
                    {
                        switch(randMove) //이동할 무작위 위치
                        {
                            case 0: //북
                                if (i-1 >=0 && (int)info[stgno].button[i-1,j].Tag != 2 && (int)info[stgno].button[i - 1, j].Tag != 3 && (int)info[stgno].button[i - 1, j].Tag != 4)
                                {
                                    if ((int)info[stgno].button[i - 1, j].Tag == 1)
                                    {
                                        Global.lifePoint--;
                                        info[stgno].life.Text = "Life:" + Global.lifePoint.ToString();
                                        if(Global.lifePoint==0)
                                        {
                                            Form2 form2 = new Form2("Lose!", "don't be disappointed...!");
                                            this.Visible=false;
                                            form2.ShowDialog();
                                        }
                                    } 
                                    else
                                    {
                                        info[stgno].button[i, j].Text = "";
                                        info[stgno].button[i, j].Tag = 0;
                                        info[stgno].button[i - 1, j].Text = "◈";
                                        info[stgno].button[i - 1, j].Tag = 3;
                                        break;
                                    }
                                }
                                break;
                            case 1: //북동
                                if (i-1>=0 && j+1 < info[stgno].gameCol && (int)info[stgno].button[i - 1, j+1].Tag != 2 && (int)info[stgno].button[i - 1, j + 1].Tag != 3 && (int)info[stgno].button[i - 1, j + 1].Tag != 4)
                                {
                                    if ((int)info[stgno].button[i - 1, j + 1].Tag == 1)
                                    {
                                        Global.lifePoint--;
                                        info[stgno].life.Text = "Life:" + Global.lifePoint.ToString();
                                        if (Global.lifePoint == 0)
                                        {
                                            Form2 form2 = new Form2("Lose!", "don't be disappointed...!");
                                            this.Hide();
                                            form2.ShowDialog();
                                        }
                                    }
                                    else
                                    {
                                        info[stgno].button[i, j].Text = "";
                                        info[stgno].button[i, j].Tag = 0;
                                        info[stgno].button[i - 1, j + 1].Text = "◈";
                                        info[stgno].button[i - 1, j + 1].Tag = 3;
                                        break;
                                    }
                                }
                                break;
                            case 2: //동
                                if (j + 1 < info[stgno].gameCol && (int)info[stgno].button[i, j+1].Tag != 2 && (int)info[stgno].button[i, j + 1].Tag != 3 && (int)info[stgno].button[i, j + 1].Tag != 4)
                                {
                                    if ((int)info[stgno].button[i, j + 1].Tag == 1)
                                    {   Global.lifePoint--;
                                        info[stgno].life.Text = "Life:" + Global.lifePoint.ToString();
                                        if (Global.lifePoint == 0)
                                        {
                                            Form2 form2 = new Form2("Lose!", "don't be disappointed...!");
                                            this.Hide();
                                            form2.ShowDialog();

                                        }
                                    }
                                    else
                                    {
                                        info[stgno].button[i, j].Text = "";
                                        info[stgno].button[i, j].Tag = 0;
                                        info[stgno].button[i, j + 1].Text = "◈";
                                        info[stgno].button[i, j + 1].Tag = 3;
                                        break;
                                    }
                                }
                                break;
                            case 3: //남동
                                if (i + 1 < info[stgno].gameRow && j + 1 < info[stgno].gameCol && (int)info[stgno].button[i + 1, j+1].Tag != 2 && (int)info[stgno].button[i + 1, j + 1].Tag != 3 && (int)info[stgno].button[i + 1, j + 1].Tag != 4)
                                {
                                    if ((int)info[stgno].button[i + 1, j + 1].Tag == 1)
                                    {
                                        Global.lifePoint--;
                                        info[stgno].life.Text = "Life:" + Global.lifePoint.ToString();
                                        if (Global.lifePoint == 0)
                                        {
                                            Form2 form2 = new Form2("Lose!", "don't be disappointed...!");
                                            this.Hide();
                                            form2.ShowDialog();

                                        }
                                    }
                                    else
                                    {
                                        info[stgno].button[i, j].Text = "";
                                        info[stgno].button[i, j].Tag = 0;
                                        info[stgno].button[i + 1, j + 1].Text = "◈";
                                        info[stgno].button[i + 1, j + 1].Tag = 3;
                                        break;
                                    }
                                }
                                break;
                            case 4: //남
                                if (i + 1 < info[stgno].gameRow && (int)info[stgno].button[i + 1, j].Tag != 2 && (int)info[stgno].button[i + 1, j].Tag != 3 && (int)info[stgno].button[i + 1, j].Tag != 4)
                                {
                                    if ((int)info[stgno].button[i + 1, j].Tag == 1)
                                    {
                                        Global.lifePoint--;
                                        info[stgno].life.Text = "Life:" + Global.lifePoint.ToString();
                                        if (Global.lifePoint == 0)
                                        {
                                            Form2 form2 = new Form2("Lose!", "don't be disappointed...!");
                                            this.Hide();
                                            form2.ShowDialog();

                                        }
                                    }
                                    else
                                    {
                                        info[stgno].button[i, j].Text = "";
                                        info[stgno].button[i, j].Tag = 0;
                                        info[stgno].button[i + 1, j].Text = "◈";
                                        info[stgno].button[i + 1, j].Tag = 3;
                                        break;
                                    }
                                }
                                break;
                            case 5: //남서
                                if (i + 1 < info[stgno].gameRow && j -1 >= 0 && (int)info[stgno].button[i + 1, j-1].Tag != 2 && (int)info[stgno].button[i + 1, j - 1].Tag != 3 && (int)info[stgno].button[i + 1, j - 1].Tag != 4)
                                {
                                    if ((int)info[stgno].button[i + 1, j - 1].Tag == 1)
                                    {
                                        Global.lifePoint--;
                                        info[stgno].life.Text = "Life:" + Global.lifePoint.ToString();
                                        if (Global.lifePoint == 0)
                                        {
                                            Form2 form2 = new Form2("Lose!", "don't be disappointed...!");
                                            this.Hide();
                                            form2.ShowDialog();

                                        }
                                    }
                                    else
                                    {
                                        info[stgno].button[i, j].Text = "";
                                        info[stgno].button[i, j].Tag = 0;
                                        info[stgno].button[i + 1, j - 1].Text = "◈";
                                        info[stgno].button[i + 1, j - 1].Tag = 3;
                                        break;
                                    }
                                }
                                break;
                            case 6: //서
                                if (j - 1 >= 0 && (int)info[stgno].button[i, j-1].Tag != 2 && (int)info[stgno].button[i, j - 1].Tag != 3 && (int)info[stgno].button[i, j - 1].Tag != 4)
                                {
                                    if ((int)info[stgno].button[i, j - 1].Tag == 1)
                                    {
                                        Global.lifePoint--;
                                        info[stgno].life.Text = "Life:" + Global.lifePoint.ToString();
                                        if (Global.lifePoint == 0)
                                        {
                                            Form2 form2 = new Form2("Lose!", "don't be disappointed...!");
                                            this.Hide();
                                            form2.ShowDialog();

                                        }
                                    }
                                    else
                                    {
                                        info[stgno].button[i, j].Text = "";
                                        info[stgno].button[i, j].Tag = 0;
                                        info[stgno].button[i, j - 1].Text = "◈";
                                        info[stgno].button[i, j - 1].Tag = 3;
                                        break;
                                    }
                                }
                                break;
                            case 7: //북서
                                if (i - 1 >= 0 && j - 1 >= 0 && (int)info[stgno].button[i - 1, j - 1].Tag != 2 && (int)info[stgno].button[i - 1, j - 1].Tag != 3 && (int)info[stgno].button[i - 1, j - 1].Tag != 4)
                                {
                                    if ((int)info[stgno].button[i - 1, j - 1].Tag == 1)
                                    {
                                        Global.lifePoint--;
                                        info[stgno].life.Text = "Life:" + Global.lifePoint.ToString();
                                        if (Global.lifePoint == 0)
                                        {
                                            Form2 form2 = new Form2("Lose!", "don't be disappointed...!");
                                            this.Hide();
                                            form2.ShowDialog();

                                        }
                                    }
                                    else
                                    {
                                        info[stgno].button[i, j].Text = "";
                                        info[stgno].button[i, j].Tag = 0;
                                        info[stgno].button[i - 1, j - 1].Text = "◈";
                                        info[stgno].button[i - 1, j - 1].Tag = 3;
                                        break;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
        }
        private void Start(int a)
        {
            if (a == 2)
            {
                form4.Show();
                form4.Visible = false;
            }
            else if (a == 3)
            {
                form4.Show();
                form4.Visible = false;
                form5.Show();
                form5.Visible = false;
            }
            else if (a == 4)
            {
                form4.Show();
                form4.Visible = false;
                form5.Show();
                form5.Visible = false;
                form6.Show();
                form6.Visible = false;
            }
            else if (a == 5)
            {
                form4.Show();
                form4.Visible = false;
                form5.Show();
                form5.Visible = false;
                form6.Show();
                form6.Visible = false;
                form7.Show();
                form7.Visible = false;
            }
            else if (a == 6)
            {
                form4.Show();
                form4.Visible = false;
                form5.Show();
                form5.Visible = false;
                form6.Show();
                form6.Visible = false;
                form7.Show();
                form7.Visible = false;
                form8.Show();
                form8.Visible = false;
            }
            else if (a == 7)
            {
                form4.Show();
                form4.Visible = false;
                form5.Show();
                form5.Visible = false;
                form6.Show();
                form6.Visible = false;
                form7.Show();
                form7.Visible = false;
                form8.Show();
                form8.Visible = false;
                form9.Show();
                form9.Visible = false;
            }
            else if (a == 8)
            {
                form4.Show();
                form4.Visible = false;
                form5.Show();
                form5.Visible = false;
                form6.Show();
                form6.Visible = false;
                form7.Show();
                form7.Visible = false;
                form8.Show();
                form8.Visible = false;
                form9.Show();
                form9.Visible = false;
                form10.Show();
                form10.Visible = false;
            }
            else if (a == 9)
            {
                form4.Show();
                form4.Visible = false;
                form5.Show();
                form5.Visible = false;
                form6.Show();
                form6.Visible = false;
                form7.Show();
                form7.Visible = false;
                form8.Show();
                form8.Visible = false;
                form9.Show();
                form9.Visible = false;
                form10.Show();
                form10.Visible = false;
                form11.Show();
                form11.Visible = false;
            }
            else if (a == 10)
            {
                form4.Show();
                form4.Visible = false;
                form5.Show();
                form5.Visible = false;
                form6.Show();
                form6.Visible = false;
                form7.Show();
                form7.Visible = false;
                form8.Show();
                form8.Visible = false;
                form9.Show();
                form9.Visible = false;
                form10.Show();
                form10.Visible = false;
                form11.Show();
                form11.Visible = false;
                form12.Show();
                form12.Visible = false;
            }
        }
        private void btn_start_Click(object sender, EventArgs e)
        {
            int StageNum = (int)r.Next(2, 11); //2~10 랜덤 스테이지 수
            //StageNum = 2;
            //MessageBox.Show(StageNum.ToString());
            for (int i = 0; i < StageNum; i++)
            {
                info[i] = new Info(0, 0, 0, 0, 0);
                Global.stgList.Add(i);
            }
            switch (StageNum)
            {
                case 2:
                    setStage(form3,0);
                    stgno++;
                    setStage(form4,1);
                    break;
                case 3:
                    setStage(form3,0);
                    stgno++;
                    setStage(form4,1);
                    stgno++;
                    setStage(form5,1);
                    break;
                case 4:
                    setStage(form3,0);
                    stgno++;
                    setStage(form4,1);
                    stgno++;
                    setStage(form5,1);
                    stgno++;
                    setStage(form6,1);
                    break;
                case 5:
                    setStage(form3,0);
                    stgno++;
                    setStage(form4,1);
                    stgno++;
                    setStage(form5,1);
                    stgno++;
                    setStage(form6,1);
                    stgno++;
                    setStage(form7,1);
                    break;
                case 6:
                    setStage(form3, 0);
                    stgno++;
                    setStage(form4, 1);
                    stgno++;
                    setStage(form5, 1);
                    stgno++;
                    setStage(form6, 1);
                    stgno++;
                    setStage(form7, 1);
                    stgno++;
                    setStage(form8, 1);
                    break;
                case 7:
                    setStage(form3, 0);
                    stgno++;
                    setStage(form4, 1);
                    stgno++;
                    setStage(form5, 1);
                    stgno++;
                    setStage(form6, 1);
                    stgno++;
                    setStage(form7, 1);
                    stgno++;
                    setStage(form8, 1);
                    stgno++;
                    setStage(form9, 1);
                    break;
                case 8:
                    setStage(form3, 0);
                    stgno++;
                    setStage(form4, 1);
                    stgno++;
                    setStage(form5, 1);
                    stgno++;
                    setStage(form6, 1);
                    stgno++;
                    setStage(form7, 1);
                    stgno++;
                    setStage(form8, 1);
                    stgno++;
                    setStage(form9, 1);
                    stgno++;
                    setStage(form10, 1);
                    break;
                case 9:
                    setStage(form3, 0);
                    stgno++;
                    setStage(form4, 1);
                    stgno++;
                    setStage(form5, 1);
                    stgno++;
                    setStage(form6, 1);
                    stgno++;
                    setStage(form7, 1);
                    stgno++;
                    setStage(form8, 1);
                    stgno++;
                    setStage(form9, 1);
                    stgno++;
                    setStage(form10, 1);
                    stgno++;
                    setStage(form11, 1);
                    break;
                case 10:
                    setStage(form3, 0);
                    stgno++;
                    setStage(form4, 1);
                    stgno++;
                    setStage(form5, 1);
                    stgno++;
                    setStage(form6, 1);
                    stgno++;
                    setStage(form7, 1);
                    stgno++;
                    setStage(form8, 1);
                    stgno++;
                    setStage(form9, 1);
                    stgno++;
                    setStage(form10, 1);
                    stgno++;
                    setStage(form11, 1);
                    stgno++;
                    setStage(form12, 1);
                    break;
            }
            //MessageBox.Show(Global.stgList.Count.ToString());
            setExit();
            this.Visible = false;
            this.Hide();
            stgno = 0;
            Start(StageNum);
            setPlayer(form3, info[0].button, info[0].gameRow, info[0].gameCol);
            form3.ShowDialog();
            
            //this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}

