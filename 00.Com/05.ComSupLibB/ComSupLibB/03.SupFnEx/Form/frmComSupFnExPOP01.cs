using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System;
using System.Windows.Forms;

namespace ComSupLibB.SupFnEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupFnEx
    /// File Name       : frmComSupFnExPOP01.cs
    /// Description     : 기능검사 달력+시간 
    /// Author          : 윤조연
    /// Create Date     : 2017-07-18
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= ">> frmComSupFnExPOP01.cs 폼이름 재정의" />
    public partial class frmComSupFnExPOP01 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic();

        public delegate void SendMsg(string[] str);
        public event SendMsg rSendMsg;

        string gPart = "";//사용파트
        string gSTS = "";//작업상태
        string[] msgX = null;
        string[] argfrm = new string[Enum.GetValues(typeof(clsComSupFnEx.enmfrmComSupFnExPOP01)).Length];

        
        #endregion

        public frmComSupFnExPOP01(string[] argfrm)
        {
            InitializeComponent();

            
            gPart = argfrm[(int)clsComSupFnEx.enmfrmComSupFnExPOP01.Part];
            gSTS = argfrm[(int)clsComSupFnEx.enmfrmComSupFnExPOP01.STS];
            dtpDate.Text = argfrm[(int)clsComSupFnEx.enmfrmComSupFnExPOP01.RDate];
            dtpTime.Text = argfrm[(int)clsComSupFnEx.enmfrmComSupFnExPOP01.RTime];

            if (gPart =="Ends")
            {
                btnCTime.Visible = false;
                dtpTime.Visible = false;
            }

            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
                        
            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnSet.Click += new EventHandler(eBtnEvent);

            this.btnPop.Click += new EventHandler(eBtnEvent);
            
            this.btnCTime.Click += new EventHandler(eBtnEvent);                      

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
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                read_sysdate();

                eBtnEvent(btnPop, e);

            }
            
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                msgX = new string[Enum.GetValues(typeof(clsComSupFnEx.enmRDateTime)).Length];
                msgX[(int)clsComSupFnEx.enmRDateTime.STS] = gSTS;
                msgX[(int)clsComSupFnEx.enmRDateTime.OK] = "NO";
                

                this.Close();
            }
            else if (sender == this.btnCTime)
            {
                read_sysdate();

                dtpDate.Text = cpublic.strSysDate;
                dtpTime.Text = VB.Left(cpublic.strSysTime,5);

            }
            else if (sender == this.btnPop)
            {
                //this.dtpDate.Focus();
                //SendKeys.Send("{F4}");
                if(clsType.User.Sabun == "06416")
                { }
                else
                {
                    dtpDate.Select();
                    SendKeys.Send("%{DOWN}");
                }
                

            }
            else if (sender == this.btnSet)
            {
                msgX = new string[Enum.GetValues(typeof(clsComSupFnEx.enmRDateTime)).Length];

                if (gPart =="FnEx")
                {
                    msgX[(int)clsComSupFnEx.enmRDateTime.Part] = gPart;
                    msgX[(int)clsComSupFnEx.enmRDateTime.STS] = gSTS;
                    msgX[(int)clsComSupFnEx.enmRDateTime.OK] = "OK";
                    msgX[(int)clsComSupFnEx.enmRDateTime.RDate] = dtpDate.Text.Trim();
                    msgX[(int)clsComSupFnEx.enmRDateTime.RTime] = dtpTime.Text.Trim();
                    msgX[(int)clsComSupFnEx.enmRDateTime.RDateTime] = dtpDate.Text.Trim() + " " + dtpTime.Text.Trim();
                }
                else if (gPart == "Ends")
                {
                    msgX[(int)clsComSupFnEx.enmRDateTime.Part] = gPart;
                    msgX[(int)clsComSupFnEx.enmRDateTime.STS] = gSTS;
                    msgX[(int)clsComSupFnEx.enmRDateTime.OK] = "OK";
                    msgX[(int)clsComSupFnEx.enmRDateTime.RDate] = dtpDate.Text.Trim();
                    msgX[(int)clsComSupFnEx.enmRDateTime.RTime] = dtpTime.Text.Trim();
                    msgX[(int)clsComSupFnEx.enmRDateTime.RDateTime] = dtpDate.Text.Trim() + " " + dtpTime.Text.Trim();
                }
                else if (gPart == "Xray")
                {
                    msgX[(int)clsComSupFnEx.enmRDateTime.Part] = gPart;
                    msgX[(int)clsComSupFnEx.enmRDateTime.STS] = gSTS;
                    msgX[(int)clsComSupFnEx.enmRDateTime.OK] = "OK";
                    msgX[(int)clsComSupFnEx.enmRDateTime.RDate] = dtpDate.Text.Trim();
                    msgX[(int)clsComSupFnEx.enmRDateTime.RTime] = dtpTime.Text.Trim();
                    msgX[(int)clsComSupFnEx.enmRDateTime.RDateTime] = dtpDate.Text.Trim() + " " + dtpTime.Text.Trim();
                }
                else
                {
                    msgX[(int)clsComSupFnEx.enmRDateTime.Part] = gPart;
                    msgX[(int)clsComSupFnEx.enmRDateTime.STS] = gSTS;
                    msgX[(int)clsComSupFnEx.enmRDateTime.OK] = "OK";
                    msgX[(int)clsComSupFnEx.enmRDateTime.RDate] = dtpDate.Text.Trim();
                    msgX[(int)clsComSupFnEx.enmRDateTime.RTime] = dtpTime.Text.Trim();
                    msgX[(int)clsComSupFnEx.enmRDateTime.RDateTime] = dtpDate.Text.Trim() + " " + dtpTime.Text.Trim();
                }
                                
                rSendMsg(msgX);

                this.Close();

            }

        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

    }
}
