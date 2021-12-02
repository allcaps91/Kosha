using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Media;
using System.Speech.Synthesis;
using System.Threading;
using System.Windows.Forms;

namespace HC_IF
{
    public partial class frmHcCall_DID :Form
    {
        long FnPcNo = 0;
        string FstrJobName = "";

        SoundPlayer player = null;

        HIC_WAIT nHW = null;
        HicWaitService hicWaitService = null;
        SpeechSynthesizer speechSynthesizer = null;

        Label[] flblCall = new Label[4];
        Label[] flblWait = new Label[3];

        public frmHcCall_DID()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.SetOutputToDefaultAudioDevice();
            speechSynthesizer.SelectVoice("Microsoft Heami Desktop");

            //speechSynthesizer.SelectVoice("Microsoft David Desktop");
            //speechSynthesizer.SelectVoice("Microsoft Zira Desktop");
            //speechSynthesizer.SelectVoice("Microsoft Mark Desktop");

            nHW = new HIC_WAIT();
            hicWaitService = new HicWaitService();

            flblCall[0] = lblCall0;
            flblCall[1] = lblCall1;
            flblCall[2] = lblCall2;
            flblCall[3] = lblCall3;

            flblWait[0] = lblWait0;
            flblWait[1] = lblWait1;
            flblWait[2] = lblWait2;

            TabParent_Set();
        }

        private void TabParent_Set()
        {
            lblLogo.Parent = pic_Main;
            lblLogo.BackColor = Color.Transparent;
            lblLogo.BringToFront();

            for (int i = 0; i < 4; i++)
            {
                flblCall[i].Parent = pic_Main;
                flblCall[i].BackColor = Color.Transparent;
            }

            for (int i = 0; i < 3; i++)
            {
                flblWait[i].Parent = pic_Main;
                flblWait[i].BackColor = Color.Transparent;
            }

            label3.Parent = pic_Main;
            label4.Parent = pic_Main;
            label5.Parent = pic_Main;
            label6.Parent = pic_Main;
            label7.Parent = pic_Main;
            label8.Parent = pic_Main;
            label9.Parent = pic_Main;
            label10.Parent = pic_Main;

            label3.BackColor = Color.Transparent;
            label4.BackColor = Color.Transparent;
            label5.BackColor = Color.Transparent;
            label6.BackColor = Color.Transparent;
            label7.BackColor = Color.Transparent;
            label8.BackColor = Color.Transparent;
            label9.BackColor = Color.Transparent;
            label10.BackColor = Color.Transparent;
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.timer1.Tick += new EventHandler(eTimer_Timer);
            this.lblLogo.DoubleClick += new EventHandler(eLblDblClick);
        }

        private void eLblDblClick(object sender, EventArgs e)
        {
            timer1.Stop();
            this.Close();
            return;
        }

        private void eTimer_Timer(object sender, EventArgs e)
        {
            try
            {
                if (sender == timer1)
                {
                    Timer1_timer();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   
        }

        private void Timer1_timer()
        {
            string strWav = "";
            panCall.Visible = false;

            try
            {
                timer1.Stop();

                nHW = hicWaitService.GetItemByJobDateGbDisplay(DateTime.Now.ToShortDateString(), "N");

                if (!nHW.IsNullOrEmpty())
                {
                    if (nHW.GBDISPLAY2 == "N" && !nHW.GBJOB.IsNullOrEmpty())
                    {
                        FnPcNo = nHW.PCNO.To<long>(0);

                        FstrJobName = "";
                        switch (nHW.GBJOB)
                        {
                            case "0": FstrJobName = "준비중"; break;
                            case "1": FstrJobName = VB.Format(nHW.SEQNO, "#0"); break;
                            case "2": FstrJobName = "상담중"; break;
                            case "3": FstrJobName = "PC고장"; break;
                            case "4": FstrJobName = "옆창구"; break;
                            case "5": FstrJobName = "업무종료"; break;
                            default: break;
                        }

                        if (!hicWaitService.UpDateDisplay("Y", DateTime.Now.ToShortDateString(), nHW.PCNO))
                        {
                            MessageBox.Show("호출정보 갱신 오류", "확인");
                            return;
                        }

                        if (nHW.SNAME.IsNullOrEmpty())
                        {
                            panCall.Text = nHW.SEQNO + "번 고객님" + ComNum.VBLF + ComNum.VBLF;
                        }
                        else
                        {
                            panCall.Text = nHW.SEQNO + "번(" + nHW.SNAME + "님)" + ComNum.VBLF + ComNum.VBLF;
                        }

                        panCall.Text += FnPcNo + "번창구로 오십시오.";
                        panCall.Visible = true;

                        flblCall[FnPcNo - 1].Text = FstrJobName;

                        Application.DoEvents();

                        //딩동 소리(대기순번 호출시만 표시)
                        if (!nHW.IsNullOrEmpty() && nHW.GBJOB == "1" && nHW.SEQNO > 0)
                        {
                            strWav = "DingDong.wav";
                            player.SoundLocation = @"C:\" + strWav;
                            player.LoadAsync();
                            Thread.Sleep(500);

                            speechSynthesizer.Speak(nHW.SEQNO + "  번 " + nHW.SNAME + "님," + FnPcNo + "  번창구로 오십시오.");
                        }

                        panCall.Visible = false;

                    }
                }

                //대기 인원수를 읽음
                HIC_WAIT nHWTCNT = hicWaitService.GetCountItemByJobDateGbJob(DateTime.Now.ToShortDateString(), "1");
                if (!nHWTCNT.IsNullOrEmpty())
                {
                    lblWait0.Text = nHWTCNT.HEACNT.To<string>("0");
                    lblWait1.Text = nHWTCNT.YEYAKCNT.To<string>("0");
                    lblWait2.Text = (nHWTCNT.CNT - nHWTCNT.YEYAKCNT - nHWTCNT.HEACNT).To<string>("0");
                }

                Application.DoEvents();

                timer1.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void eFormLoad(object sender, EventArgs e)
        {
            player = new SoundPlayer();
            player.LoadCompleted += new AsyncCompletedEventHandler(player_LoadCompleted);

            panCall.Visible = false;

            lblCall0.Text = "";
            lblCall1.Text = "";
            lblCall2.Text = "";
            lblCall3.Text = "";

            lblWait0.Text = "";
            lblWait1.Text = "";
            lblWait2.Text = "";

            //어제 이전의 자료는 삭제함
            string strDate = DateTime.Now.AddDays(-2).ToShortDateString();

            hicWaitService.DeleteByJobDate(strDate);

        }

        private void player_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (player.IsLoadCompleted)
            {
                player.Play();
            }
        }
    }
}
