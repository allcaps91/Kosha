using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Threading;
using ComBase;

namespace ComPmpaLibB
{
    public partial class frmAutoInfomation : Form
    {
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        SoundPlayer player = null;

        int FnTimer1 = 0;
        int FnCnt_Touch1 = 0;
        int FnCnt_Touch2 = 0;

        string FstrGubun = "";
        string FstrPopGbn = "";
        string FstrFlag = "";
        
        public frmAutoInfomation()
        {
            InitializeComponent();
        }
        
        private void lblLogo_DoubleClick(object sender, EventArgs e)
        {
            //this.Close();
            Timer1.Enabled = false;
            rEventClosed();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            //this.Close();
            Timer1.Enabled = false;
            rEventClosed();
        }

        private void frmAutoInfomation_Load(object sender, EventArgs e)
        {
            player = new SoundPlayer();
            player.LoadCompleted += new AsyncCompletedEventHandler(player_LoadCompleted);

            lblLogo.Width = 130;
            lblLogo.Height = 106;
            lblLogo.Left = 18;
            lblLogo.Top = 38;
            lblLogo.Parent = pic_Main;

            Image_T1.Parent = pic_Main;
            Image_T2.Parent = pic_Main;

            Lbl_11_msg.Parent = picPrint1;
            Lbl_Last_msg.Parent = picPrint2;
            Lbl_Last_msg2.Parent = picPrint2;

            labelX3.Parent = picPrint3;
            Lbl_Main_msg.Parent = picPrint3;

            labelX1.Parent = picSystemChk;
            lblMsg.Parent = picSystemChk;

            Timer1.Enabled = false;

            string strTemp = "";
            string strTemp2 = "";
            string strTemp3 = "";
            string strTemp4 = "";

            panPrint1.Dock = DockStyle.Fill;
            panPrint2.Dock = DockStyle.Fill;
            panPrint3.Dock = DockStyle.Fill;
            panSystemChk.Dock = DockStyle.Fill;

            panPrint1.Visible = false;
            panPrint2.Visible = false;
            panPrint3.Visible = false;
            panSystemChk.Visible = false;

            progressBar1.Maximum = 10;
            progressBar1.Minimum = 1;

            FnCnt_Touch1 = 0;
            FnCnt_Touch2 = 0;

            strTemp = clsAuto.GstrAR_PopUpVariable;

            Lbl_Main_msg.Text = "";
            Lbl_11_msg.Text = "";
            Lbl_Last_msg.Text = "";
            Lbl_Last_msg2.Text = "";

            FnTimer1 = 0;
            FstrGubun = "";
            FstrPopGbn = "";

            if (strTemp != "")
            {
                strTemp2 = VB.Split(strTemp, "^^")[0];
                FstrGubun = strTemp2; // '작업구분 세팅
                strTemp3 = VB.Split(strTemp, "^^")[1];
                strTemp4 = VB.Split(strTemp, "^^")[2];
                Lbl_Main_msg.Text = strTemp3;
                Lbl_Last_msg.Text = strTemp3;
                Lbl_Last_msg2.Text = "영수증 및 처방전 출력이 완료 되었습니다.";

                if (strTemp2 != "")
                {
                    switch (strTemp2)
                    {
                        case "00":
                            panPrint3.Visible = true;
                            panPrint3.BringToFront();
                            Lbl_Main_msg.Text = strTemp3;
                            break;
                        case "01":

                            break;
                        case "10":
                            panPrint1.Visible = true;
                            panPrint1.BringToFront();
                            Lbl_11_msg.Text = strTemp3;
                            break;
                        case "11":
                            btnExit.Visible = false;
                            panPrint1.Visible = true;
                            panPrint1.BringToFront();
                            Lbl_11_msg.Text = strTemp3;
                            break;
                        case "12":
                            btnExit.Visible = false;
                            panPrint2.Visible = true;
                            panPrint2.BringToFront();
                            break;
                        case "20":
                            //'영수증만 있을경우
                            btnExit.Visible = false;
                            panPrint1.Visible = true;
                            panPrint1.BringToFront();
                            Lbl_11_msg.Text = strTemp3;
                            //'pic_print2
                            Lbl_Last_msg.Text = strTemp4;
                            //'2013-09-12
                            Lbl_Last_msg2.Text = "영수증 출력이 완료 되었습니다.";
                            break;
                        case "21":
                            //'원외만있을경우
                            btnExit.Visible = false;
                            panPrint1.Visible = true;
                            panPrint1.BringToFront();
                            Lbl_11_msg.Text = strTemp3;
                            //'pic_print2
                            Lbl_Last_msg.Text = strTemp4;
                            //'2013-09-12
                            Lbl_Last_msg2.Text = "처방전 출력이 완료 되었습니다.";
                            break;
                        case "22":
                            //'영수증+원외
                            btnExit.Visible = false;
                            panPrint1.Visible = true;
                            panPrint1.BringToFront();
                            Lbl_11_msg.Text = strTemp3;
                            //'pic_print2
                            Lbl_Last_msg.Text = strTemp4;
                            break;
                        case "99":
                            btnExit.Visible = false;
                            panSystemChk.Visible = true;
                            panSystemChk.BringToFront();
                            break;
                    }
                }
            }
            else
            {
                btnExit.Visible = false;
                panPrint3.Visible = true;
                panPrint3.BringToFront();
                Lbl_Main_msg.Text = "잠시 기다려주세요";
            }
            Timer1.Enabled = true;
        }

        private void player_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (player.IsLoadCompleted)
            {
                player.Play();
            }
        }

        private void Image_T1_Click(object sender, EventArgs e)
        {
            FnCnt_Touch1 = FnCnt_Touch1 + 1;
        }

        private void Image_T2_Click(object sender, EventArgs e)
        {
            FnCnt_Touch2 = FnCnt_Touch2 + 1;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {            
            string strWav = "";

            FnTimer1 = FnTimer1 + 1;

            if (FstrFlag != "1")
            {
                FstrFlag = "1";
                Lbl_11_msg.Visible = false;
                Lbl_Last_msg.Visible = false;
            }
            else
            {
                FstrFlag = "";
                Lbl_11_msg.Visible = true;
                Lbl_Last_msg.Visible = true;
            }

            if (progressBar1.Value < progressBar1.Maximum)
            {
                progressBar1.Value = progressBar1.Value + 1;
            }
            else
            {
                progressBar1.Value = 1;
            }
            
            if (FnTimer1 >= 20)
            {
                FnCnt_Touch1 = 0;
                FnCnt_Touch2 = 0;
            }

            if (FnCnt_Touch1 >= 1 && FnCnt_Touch2 >= 3)
            {
                clsDB.DisDBConnect(clsDB.DbCon);
                clsDbMySql.DisDBConnect();
                Application.Exit();
                return;
            }

            //'팝업메시지
            if (FstrGubun == "00")
            {
                if (FnTimer1 >= 6)
                {
                    FnTimer1 = 0;
                    //this.Close();
                    Timer1.Enabled = false;
                    rEventClosed();
                    return;
                }
            }
            else if (FstrGubun == "10")
            {
                if (FnTimer1 >= 20)
                {
                    FnTimer1 = 0;
                    //this.Close();
                    Timer1.Enabled = false;
                    rEventClosed();
                    return;
                }
            }
            else if (FstrGubun == "11")
            {
                if (FnTimer1 >= 40)
                {
                    FnTimer1 = 0;
                    //this.Close();
                    Timer1.Enabled = false;
                    rEventClosed();
                    return;
                }
            }
            else if (FstrGubun == "12")
            {
                if (FnTimer1 >= 40)
                {
                    FnTimer1 = 0;
                    //this.Close();
                    Timer1.Enabled = false;
                    rEventClosed();
                    return;
                }
            }
            else if (FstrGubun == "20")  //연속 메시지 영수증
            {
                //7초,10초, 9초 ==>> 7초있다가 음성나오고 10초있다가 창전환후 9초후 종료됨
                //2014-01-22  6초, 8초, 9초
                //2014-01-27  5초
                //2019-03-25  10초 12초
                if (FnTimer1 == 8 && FstrPopGbn == "")
                {
                    //2013-09-12 음성안내
                    strWav = "무인수납_영수증.wav";
                    player.SoundLocation = @"C:\cmc\exe\wav\" + strWav;
                    player.LoadAsync();                    
                    return;
                }

                if (FnTimer1 >= 8 && FstrPopGbn == "")  //10초 8초 5초
                {
                    FnTimer1 = 0;
                    FstrPopGbn = "Y";
                    panPrint2.Visible = true;
                    panPrint2.BringToFront();
                }
                else if (FnTimer1 >= 9 && FstrPopGbn == "Y")
                {
                    FnTimer1 = 0;
                    //this.Close();
                    Timer1.Enabled = false;
                    rEventClosed();
                    return;
                }
            }
            else if (FstrGubun == "21")  //연속 메시지 원외처방
            {
                //'            12초,15초,9초
                //'2014-01-22  10초, 12초, 9초
                //'2014-01-27  8초
                //2019-03-25   16초 16초 12초
                if (FnTimer1 == 13 && FstrPopGbn == "")
                {
                    strWav = "무인수납_처방전.wav";
                    player.SoundLocation = @"C:\cmc\exe\wav\" + strWav;
                    player.LoadAsync();                                        
                    return;
                }

                if (FnTimer1 >= 13 && FstrPopGbn == "")
                {
                    FnTimer1 = 0;
                    FstrPopGbn = "Y";
                    panPrint2.Visible = true;
                    panPrint2.BringToFront();
                }
                else if (FnTimer1 >= 9 && FstrPopGbn == "Y")
                {
                    FnTimer1 = 0;
                    //this.Close();
                    Timer1.Enabled = false;
                    rEventClosed();
                    return;
                }
            }
            else if (FstrGubun == "22")  //연속 메시지 영수증+원외처방
            {
                //2019-03-25   18초 18초
                if (FnTimer1 >= 15 && FstrPopGbn == "")
                {
                    FnTimer1 = 0;
                    FstrPopGbn = "Y";

                    strWav = "무인수납_영수처방전.wav";
                    player.SoundLocation = @"C:\cmc\exe\wav\" + strWav;
                    player.LoadAsync();
                    
                    panPrint2.Visible = true;
                    panPrint2.BringToFront();
                    return;
                }
                else if (FnTimer1 >= 9 && FstrPopGbn == "Y")
                {
                    FnTimer1 = 0;
                    //this.Close();
                    Timer1.Enabled = false;
                    rEventClosed();
                    return;
                }
            }
            else if (FstrGubun == "99") //장애루틴시 종료 없음
            {
                //this.Close();
                Timer1.Enabled = false;
                rEventClosed();
                return;
            }
            else
            {
                if (FnTimer1 >= 40)
                {
                    FnTimer1 = 0;
                    //this.Close();
                    Timer1.Enabled = false;
                    rEventClosed();
                    return;
                }
            }            
        }        
    }
}

