using ComBase; //기본 클래스
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// File Name       : FrmMedDocMsgBox..cs
    /// Description     : 메시지 전달 사항
    /// Author          : 이상훈
    /// Create Date     : 2017-05
    /// <history>       
    /// </history>
    /// <seealso>
    /// Ocs\FrmDoctMsgBox.frm
    /// </seealso>
    /// </summary>
    public partial class FrmMedDocMsgBox : Form
    {
        clsOrdFunction od = new clsOrdFunction();

        string strMsg;
        string strMsg3;

        public FrmMedDocMsgBox()
        {
            InitializeComponent();
        }

        public FrmMedDocMsgBox(string sMsg, string sMsg3)
        {
            InitializeComponent();

            strMsg = sMsg;
            strMsg3 = sMsg3;
        }

        private void btnBlack_Click(object sender, EventArgs e)
        {
            int i = 0;
            int nStart = 0;
            int nEnd = 0;

            RText.Rtf = strMsg;

            nStart = 0;
            nEnd = 0;

            for (int I = 0; I < RText.Text.Length; I++)
            {
                if (nStart == 0 && VB.Mid(RText.Text, I, 1) == "☞")
                {
                    nStart = I - 1;
                }

                if (nStart != 0)
                {
                    if (VB.Mid(RText.Text, i, 1) == "\n")
                    {
                        nEnd = i - 1 - nStart;
                    }
                }

                if (nStart != 0 && nEnd != 0)
                {
                    RText.SelectionStart = nStart;
                    RText.SelectionLength = nEnd;
                    RText.SelectionColor = Color.FromArgb(255, 0, 0);
                    nStart = 0; nEnd = 0;
                }
            }

            RText.SelectionStart = i;
            btnBlack.Visible = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmDocMsgBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            strMsg = "";
        }

        private void FrmDocMsgBox_Load(object sender, EventArgs e)
        {
            int i;
            int nStart = 0;
            int nEnd = 0;

            btnBlack.Visible = false;

            //if (ComQuery.isFormAuth(this) == false) //폼 권한 조회
            //{
            //    this.Close();
            //    return;
            //}
            //ComFunc.SetFormInit(this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            if (VB.Left(strMsg, 11) == "<골다공증 검사결과>")
            {
                this.Text = "골다공증 검사결과 확인!!";

                RText.Text = strMsg;

                for (i = 0; i < RText.Text.Length; i++)
                {
                    if (nStart == 0 && VB.Mid(RText.Text, i, 1) == "☞")
                    {
                        nStart = i - 1;
                    }

                    if (nStart != 0)
                    {
                        if (VB.Mid(RText.Text, i, 1) == "")
                        {
                            nEnd = i - 1 - nStart;
                        }
                    }

                    if (nStart != 0 && nEnd != 0)
                    {
                        RText.SelectionStart = nStart;
                        RText.SelectionLength = nEnd;
                        RText.SelectionColor = Color.FromArgb(255, 0, 0);
                        nStart = 0; nEnd = 0;
                    }
                }
                RText.SelectionStart = i;
            }
            else if (strMsg3 == "<블랙리스트>")
            {
                this.Text = "Information!!";

                Font f = new Font("굴림체", 9);
                this.Font = f;

                this.Width = 470;
                this.Height = 430;

                btnBlack.Top = 63;
                btnBlack.Left = 1;

                btnBlack.BringToFront();

                RText.Width = 467;
                RText.Height = 300;
                
                btnExit.Left = 375;
                btnExit.Top = 305;

                btnExit.Size = new Size(72, 75);

                this.BackColor = Color.FromArgb(0, 0, 0);
                RText.BackColor = Color.FromArgb(255, 255, 255);

                Lbl_Black.Top = 305;
                Lbl_Black.Left = 1;
                Lbl_Black.Visible = true;

                RText.Text = "☞자세한내용은 클릭후확인!!";

                RText.SelectionStart = 0;
                btnBlack.Visible = true;
            }
            else if (VB.Left(strMsg, 15) == "[ Allergy  정보 ]")
            {
                RText.Text = strMsg;
                //RText.Rtf = strMsg;
                nStart = 0; nEnd = 0;

                for (i = 0; i < RText.Text.Length; i++)
                {
                    if (nStart == 0 && VB.Mid(RText.Text, i, 1) == "☞")
                    {
                        nStart = i - 1;
                    }

                    if (nStart != 0)
                    {
                        if (VB.Mid(RText.Text, i, 1) == "\n")
                        {
                            nEnd = i - 1 - nStart;
                        }
                    }

                    if (nStart != 0 && nEnd != 0)
                    {
                        RText.SelectionStart = nStart;
                        RText.SelectionLength = nEnd;
                        RText.SelectionColor = Color.FromArgb(255, 0, 0);
                        nStart = 0; nEnd = 0;
                    }
                }

                RText.SelectionStart = i;
            }
            else if (strMsg3 == "동시처방 불가  알림")
            {
                this.Text += " 동시처방 불가  알림";
            }
            else if (VB.Left(strMsg3, 10) == "심사과 메시지 전달")
            {
                //string sMsg = VB.Mid(strMsg3, 11, strMsg3.Length - 1);
                string sMsg = strMsg3;

                this.Text = sMsg;
                
                //RText.Rtf = strMsg;
                RText.Rtf = VB.Replace(strMsg, "`", "'"); 

                //RText.Text += "\r\n\r\n" + sMsg;
                //nStart = 0; nEnd = 0;

                //for (i = 0; i < RText.Text.Length; i++)
                //{
                //    if (nStart == 0 && VB.Mid(RText.Text, i, 1) == "☞")
                //    {
                //        nStart = i - 1;
                //    }

                //    if (nStart != 0)
                //    {
                //        if (VB.Mid(RText.Text, i, 1) == "\r")
                //        {
                //            nEnd = i - 1 - nStart;
                //        }
                //    }

                //    if (nStart != 0 && nEnd != 0)
                //    {
                //        RText.SelectionStart = nStart;
                //        RText.SelectionLength = nEnd;
                //        RText.SelectionColor = Color.FromArgb(255, 0, 0);
                //        nStart = 0; nEnd = 0;
                //    } 
                //}
                //RText.SelectionStart = i;
            }
            else if (VB.Left(strMsg3, 7) == "[제한항생제]")
            {
                string sMsg = VB.Mid(strMsg3, 8, strMsg3.Length - 1);

                RText.Text = strMsg;

                RText.Text += "\r\n\r\n" + sMsg;
                nStart = 0; nEnd = 0;

                for (i = 0; i < RText.Text.Length; i++)
                {
                    if (nStart == 0 && VB.Mid(RText.Text, i, 1) == "☞")
                    {
                        nStart = i - 1;
                    }

                    if (nStart != 0)
                    {
                        if (VB.Mid(RText.Text, i, 1) == "\r")
                        {
                            nEnd = i - 1 - nStart;
                        }
                    }

                    if (nStart != 0 && nEnd != 0)
                    {
                        RText.SelectionStart = nStart;
                        RText.SelectionLength = nEnd;
                        RText.SelectionColor = Color.FromArgb(255, 0, 0);
                        nStart = 0; nEnd = 0;
                    }
                }
                RText.SelectionStart = i;
            }
            else
            {
                RText.Text = strMsg;
                //RText.Rtf = strMsg;
                nStart = 0; nEnd = 0;

               //2021-02-15 김찬욱과장 요청으로 글씨크기 기본보다 작게..
               if(clsType.User.IdNumber == "45684" && strMsg3 == "동일 성분 의약품 중복 처방 시스템") 
               {
                    Font f = new Font("맑은고딕", 12, FontStyle.Bold);
                    RText.Font = f;
               }

                for (i = 0; i < RText.Text.Length; i++)
                {
                    if (nStart == 0 && VB.Mid(RText.Text, i, 1) == "☞")
                    {
                        nStart = i - 1;
                    }

                    if (nStart != 0)
                    {
                        if (VB.Mid(RText.Text, i, 1) == "\r")
                        {
                            nEnd = i - 1 - nStart;
                        }
                    }

                    if (nStart != 0 && nEnd != 0)
                    {
                        RText.SelectionStart = nStart;
                        RText.SelectionLength = nEnd;
                        RText.SelectionColor = Color.FromArgb(255, 0, 0);
                        nStart = 0; nEnd = 0;
                    }
                }
                RText.SelectionStart = i;
            }

            btnExit.Focus();
            btnExit.Select();
        }
    }
}
