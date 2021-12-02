using ComBase;
using ComSupLibB.Com;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComSupLibB.SupPthl
{
    public partial class frmComSupPthlSet01 : Form, MainFormMessage
    {
        /// <summary>
        /// Class Name      : ComSupLibB.SupPthl
        /// File Name       : frmComSupPthlSet01.cs
        /// Description     : 병리과 FNA 결과입력
        /// Author          : 안정수
        /// Create Date     : 2018-08-07
        /// Update History  : 
        /// </summary>

        #region 클래스 선언 및 etc....

        clsSpread methodSpd = new clsSpread();
        clsMethod method = new clsMethod();
        clsComSup sup = new clsComSup();
        clsSpread CS = new clsSpread();

        public delegate void SendText(string strText);
        public event SendText rSendText;

        #endregion


        #region MainFormMessage InterFace

        public MainFormMessage mCallForm = null;

        public void MsgActivedForm(Form frm)
        {

        }

        public void MsgUnloadForm(Form frm)
        {

        }

        public void MsgFormClear()
        {

        }

        public void MsgSendPara(string strPara)
        {

        }

        #endregion

        public frmComSupPthlSet01()
        {
            InitializeComponent();
            setEvent();
        }

        public frmComSupPthlSet01(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }
        void setCtrlData()
        {
            //setCombo();
        }

        void setCtrlInit()
        {
            // clsCompuInfo.SetComputerInfo();
            // DataTable dt = ComQuery.Select_BAS_PCCONFIG(clsDB.DbCon, clsCompuInfo.gstrCOMIP, "프로그램PC세팅", "프로그램위치", "접수프로그램위치_내시경실");

            // if (ComFunc.isDataTableNull(dt) == false)
            // {
            // //설정세팅
            // }
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Resize += new EventHandler(eFormResize);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);

            this.btnOK.Click += new EventHandler(eBtnSave);

        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
                //            				
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등				

                //툴팁
                setCtrlData();

                //설정정보 체크
                setCtrlInit();
            }

        }

        void eFormResize(object sender, EventArgs e)
        {
            //setCtrlProgress();
        }

        void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }            
        }

        void eBtnSave(object sender, EventArgs e)
        {

            if (sender == this.btnOK)
            {
                if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }

                else
                {
                    eSaveData(); 
                }
            }

        }

        void eSaveData()
        {
            string s = ""; 
            string[] s2 = new string[6];

            int i = 0;

            #region 1번 항목

            if (ssFNA.ActiveSheet.Cells[1, 0].Text.Trim() == "True" || ssFNA.ActiveSheet.Cells[2, 0].Text.Trim() == "True" || ssFNA.ActiveSheet.Cells[3, 0].Text.Trim() == "True")
            {
                s2[0] = ssFNA.ActiveSheet.Cells[0, 0].Text + "\r\n";
            }

            if(ssFNA.ActiveSheet.Cells[1, 0].Text.Trim() == "True")
            {
                s2[0] += "　　" +  VB.Mid(ssFNA.ActiveSheet.Cells[1, 1].Text.Trim(), 4, ssFNA.ActiveSheet.Cells[1, 1].Text.Trim().Length) + "\r\n";
            }
            if (ssFNA.ActiveSheet.Cells[2, 0].Text.Trim() == "True")
            {
                s2[0] += "　　" + VB.Mid(ssFNA.ActiveSheet.Cells[2, 1].Text.Trim(), 4, ssFNA.ActiveSheet.Cells[2, 1].Text.Trim().Length) + "\r\n";
            }
            if (ssFNA.ActiveSheet.Cells[3, 0].Text.Trim() == "True")
            {
                s2[0] += "　　" + VB.Mid(ssFNA.ActiveSheet.Cells[3, 1].Text.Trim(), 4, ssFNA.ActiveSheet.Cells[3, 1].Text.Trim().Length) + "\r\n";
            }
            #endregion

            #region 2번 항목

            if (ssFNA.ActiveSheet.Cells[5, 0].Text.Trim() == "True" || ssFNA.ActiveSheet.Cells[6, 0].Text.Trim() == "True" || ssFNA.ActiveSheet.Cells[7, 0].Text.Trim() == "True" || ssFNA.ActiveSheet.Cells[8, 0].Text.Trim() == "True")
            {
                s2[1] = ssFNA.ActiveSheet.Cells[4, 0].Text + "\r\n";
            }            

            if (ssFNA.ActiveSheet.Cells[5, 0].Text.Trim() == "True")
            {
                s2[1] += "　　" + VB.Mid(ssFNA.ActiveSheet.Cells[5, 1].Text.Trim(), 4, ssFNA.ActiveSheet.Cells[5, 1].Text.Trim().Length) + "\r\n";
            }
            if (ssFNA.ActiveSheet.Cells[6, 0].Text.Trim() == "True")
            {
                s2[1] += "　　" + VB.Mid(ssFNA.ActiveSheet.Cells[6, 1].Text.Trim(), 4, ssFNA.ActiveSheet.Cells[6, 1].Text.Trim().Length) + "\r\n";
            }
            if (ssFNA.ActiveSheet.Cells[7, 0].Text.Trim() == "True")
            {
                s2[1] += "　　" + VB.Mid(ssFNA.ActiveSheet.Cells[7, 1].Text.Trim(), 4, ssFNA.ActiveSheet.Cells[7, 1].Text.Trim().Length) + "\r\n";
            }
            if (ssFNA.ActiveSheet.Cells[8, 0].Text.Trim() == "True")
            {
                s2[1] += "　　" + VB.Mid(ssFNA.ActiveSheet.Cells[8, 1].Text.Trim(), 4, ssFNA.ActiveSheet.Cells[8, 1].Text.Trim().Length) + "\r\n";
            }
            #endregion

            #region 3번 항목
            if (ssFNA.ActiveSheet.Cells[9, 0].Text.Trim() == "True")
            {
                s2[2] = ssFNA.ActiveSheet.Cells[9, 1].Text + "\r\n";
            }
            #endregion

            #region 4번 항목
            if (ssFNA.ActiveSheet.Cells[11, 0].Text.Trim() == "True" || ssFNA.ActiveSheet.Cells[12, 0].Text.Trim() == "True")
            {
                s2[3] = ssFNA.ActiveSheet.Cells[10, 0].Text + "\r\n";
            }

            if (ssFNA.ActiveSheet.Cells[11, 0].Text.Trim() == "True")
            {
                s2[3] += "　　" + VB.Mid(ssFNA.ActiveSheet.Cells[11, 1].Text.Trim(), 4, ssFNA.ActiveSheet.Cells[11, 1].Text.Trim().Length) + "\r\n";
            }
            if (ssFNA.ActiveSheet.Cells[12, 0].Text.Trim() == "True")
            {
                s2[3] += "　　" + VB.Mid(ssFNA.ActiveSheet.Cells[12, 1].Text.Trim(), 4, ssFNA.ActiveSheet.Cells[12, 1].Text.Trim().Length) + "\r\n";
            }
            #endregion

            #region 5번 항목
            if (ssFNA.ActiveSheet.Cells[14, 0].Text.Trim() == "True" || ssFNA.ActiveSheet.Cells[15, 0].Text.Trim() == "True" || ssFNA.ActiveSheet.Cells[16, 0].Text.Trim() == "True" || ssFNA.ActiveSheet.Cells[17, 0].Text.Trim() == "True" || ssFNA.ActiveSheet.Cells[18, 0].Text.Trim() == "True")
            {
                s2[4] = ssFNA.ActiveSheet.Cells[13, 0].Text + "\r\n";
            }

            if (ssFNA.ActiveSheet.Cells[14, 0].Text.Trim() == "True")
            {
                s2[4] += "　　" + VB.Mid(ssFNA.ActiveSheet.Cells[14, 1].Text.Trim(), 4, ssFNA.ActiveSheet.Cells[14, 1].Text.Trim().Length) + "\r\n";
            }
            if (ssFNA.ActiveSheet.Cells[15, 0].Text.Trim() == "True")
            {
                s2[4] += "　　" + VB.Mid(ssFNA.ActiveSheet.Cells[15, 1].Text.Trim(), 4, ssFNA.ActiveSheet.Cells[15, 1].Text.Trim().Length) + "\r\n";
            }
            if (ssFNA.ActiveSheet.Cells[16, 0].Text.Trim() == "True")
            {
                s2[4] += "　　" + VB.Mid(ssFNA.ActiveSheet.Cells[16, 1].Text.Trim(), 4, ssFNA.ActiveSheet.Cells[16, 1].Text.Trim().Length) + "\r\n";
            }
            if (ssFNA.ActiveSheet.Cells[17, 0].Text.Trim() == "True")
            {
                s2[4] += "　　" + VB.Mid(ssFNA.ActiveSheet.Cells[17, 1].Text.Trim(), 4, ssFNA.ActiveSheet.Cells[17, 1].Text.Trim().Length) + "\r\n";
            }
            if (ssFNA.ActiveSheet.Cells[18, 0].Text.Trim() == "True")
            {
                s2[4] += "　　" + VB.Mid(ssFNA.ActiveSheet.Cells[18, 1].Text.Trim(), 4, ssFNA.ActiveSheet.Cells[18, 1].Text.Trim().Length) + "\r\n";
            }
            #endregion

            #region 6번 항목
            if (ssFNA.ActiveSheet.Cells[20, 0].Text.Trim() == "True" || ssFNA.ActiveSheet.Cells[21, 0].Text.Trim() == "True" || ssFNA.ActiveSheet.Cells[22, 0].Text.Trim() == "True" || ssFNA.ActiveSheet.Cells[23, 0].Text.Trim() == "True" || ssFNA.ActiveSheet.Cells[24, 0].Text.Trim() == "True")
            {
                s2[5] = ssFNA.ActiveSheet.Cells[19, 0].Text + "\r\n";
            }

            if (ssFNA.ActiveSheet.Cells[20, 0].Text.Trim() == "True")
            {
                s2[5] += "　　" + VB.Mid(ssFNA.ActiveSheet.Cells[20, 1].Text.Trim(), 4, ssFNA.ActiveSheet.Cells[20, 1].Text.Trim().Length) + "\r\n";
            }
            if (ssFNA.ActiveSheet.Cells[21, 0].Text.Trim() == "True")
            {
                s2[5] += "　　" + VB.Mid(ssFNA.ActiveSheet.Cells[21, 1].Text.Trim(), 4, ssFNA.ActiveSheet.Cells[21, 1].Text.Trim().Length) + "\r\n";
            }
            if (ssFNA.ActiveSheet.Cells[22, 0].Text.Trim() == "True")
            {
                s2[5] += "　　" + VB.Mid(ssFNA.ActiveSheet.Cells[22, 1].Text.Trim(), 4, ssFNA.ActiveSheet.Cells[22, 1].Text.Trim().Length) + "\r\n";
            }
            if (ssFNA.ActiveSheet.Cells[23, 0].Text.Trim() == "True")
            {
                s2[5] += "　　" + VB.Mid(ssFNA.ActiveSheet.Cells[23, 1].Text.Trim(), 4, ssFNA.ActiveSheet.Cells[23, 1].Text.Trim().Length) + "\r\n";
            }
            if (ssFNA.ActiveSheet.Cells[24, 0].Text.Trim() == "True")
            {
                s2[5] += "　　" + VB.Mid(ssFNA.ActiveSheet.Cells[24, 1].Text.Trim(), 4, ssFNA.ActiveSheet.Cells[24, 1].Text.Trim().Length) + "\r\n";
            }
            #endregion

            s += s2[0];
            s += s2[1];
            s += s2[2];
            s += s2[3];
            s += s2[4];
            s += s2[5];

            //델리게이트
            if(s != "")
            {
                rSendText(s);
            }

            this.Hide();
        }

    }
}
