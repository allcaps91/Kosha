using ComBase; //기본 클래스
using System;
using System.Windows.Forms;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupInfc
    /// File Name       : frmComSupInfcSEND02.cs
    /// Description     : 병원체 검사 결과 상세내역 선택 창
    /// Author          : 김홍록
    /// Create Date     : 2018-05-17
    /// Update History  : 
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref= "d:\psmh\Frm병원체검사선택창.frm" />
    public partial class frmComSupLbExSEND02 : Form
    {
        

        public delegate void PSMH_RETURN_VALUE(string strEXAMTYPE, string strEXAMTYPE_NM, string strEXAMWAY, string strEXAMWAY_NM, string strEXAMTYPEETC, string strEXAMWAYETC);
        public event PSMH_RETURN_VALUE ePsmhReturnValue;

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }
            else
            {
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등
                setCtrl();
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
        }

        string gStrEXAMTYPE = "";
        string gStrEXAMWAY  = "";

        string gStrEXAMTYPE_NM = "";
        string gStrEXAMWAY_NM = "";

        string gStrEXAMTYPEETC = "";
        string gStrEXAMWAYETC = "";

        /// <summary>생성자</summary>
        public frmComSupLbExSEND02(string strEXAMTYPE, string strEXAMWAY, string strEXAMTYPEETC, string strEXAMWAYETC)
        {
            InitializeComponent();

            this.gStrEXAMTYPE       = strEXAMTYPE;
            this.gStrEXAMWAY        = strEXAMWAY;

            this.gStrEXAMTYPE_NM    = "";
            this.gStrEXAMWAY_NM     = "";


            this.gStrEXAMTYPEETC    = strEXAMTYPEETC;
            this.gStrEXAMWAYETC     = strEXAMWAYETC;

            setEvent();
        }

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnSend);

        }

        void eBtnSend(object sender, EventArgs e)
        {
            this.gStrEXAMTYPE = "";

            this.gStrEXAMTYPE += this.chk_11.Checked == true ? "01," : "";
            this.gStrEXAMTYPE += this.chk_12.Checked == true ? "02," : "";
            this.gStrEXAMTYPE += this.chk_13.Checked == true ? "03," : "";
            this.gStrEXAMTYPE += this.chk_14.Checked == true ? "04," : "";
            this.gStrEXAMTYPE += this.chk_15.Checked == true ? "05," : "";
            this.gStrEXAMTYPE += this.chk_19.Checked == true ? "99," : "";

            if (string.IsNullOrEmpty(this.gStrEXAMTYPE) == false)
            {
                this.gStrEXAMTYPE = this.gStrEXAMTYPE.Substring(0, this.gStrEXAMTYPE.Length - 1);

                if (this.gStrEXAMTYPE.IndexOf("99") > -1 && string.IsNullOrEmpty(this.gStrEXAMTYPEETC) == true)
                {
                    ComFunc.MsgBox("검체 유형 중 '기타'를 선택하셨을 경우 상세내역을 입력하시기 바랍니다.");
                    
                    return;
                }
            }
            else
            {
                ComFunc.MsgBox("선택된 검체 유형이 없습니다. 유형을 선택하시기 바랍니다.");
                return;
            }

            this.gStrEXAMWAY = "";

            this.gStrEXAMWAY += this.chk21.Checked == true ? "01," : "";
            this.gStrEXAMWAY += this.chk22.Checked == true ? "02," : "";
            this.gStrEXAMWAY += this.chk23.Checked == true ? "03," : "";
            this.gStrEXAMWAY += this.chk24.Checked == true ? "04," : "";
            this.gStrEXAMWAY += this.chk29.Checked == true ? "99," : "";

            if (string.IsNullOrEmpty(this.gStrEXAMWAY) == false)
            {
                this.gStrEXAMWAY = this.gStrEXAMWAY.Substring(0, this.gStrEXAMWAY.Length - 1);

                if (this.gStrEXAMWAY.IndexOf("99") > -1 && string.IsNullOrEmpty(this.gStrEXAMWAYETC) == true)
                {
                    ComFunc.MsgBox("검체 유형 중 '기타'를 선택하셨을 경우 상세내역을 입력하시기 바랍니다.");
                    return;
                                       
                }
            }
            else
            {
                ComFunc.MsgBox("선택된 검사 방법이 없습니다. 방법을 선택하시기 바랍니다.");
                return;
            }

            this.gStrEXAMTYPE_NM += this.chk_11.Checked == true ? "혈액," : "";
            this.gStrEXAMTYPE_NM += this.chk_12.Checked == true ? "체액," : "";
            this.gStrEXAMTYPE_NM += this.chk_13.Checked == true ? "소변," : "";
            this.gStrEXAMTYPE_NM += this.chk_14.Checked == true ? "대변," : "";
            this.gStrEXAMTYPE_NM += this.chk_15.Checked == true ? "객담," : "";
            this.gStrEXAMTYPE_NM += this.chk_19.Checked == true ? this.gStrEXAMTYPEETC + "," : "";

            this.gStrEXAMTYPE_NM = this.gStrEXAMTYPE_NM.Substring(0, this.gStrEXAMTYPE_NM.Length - 1);

            this.gStrEXAMWAY_NM += this.chk21.Checked == true ? "분리동정," : "";
            this.gStrEXAMWAY_NM += this.chk22.Checked == true ? "PCR검사," : "";
            this.gStrEXAMWAY_NM += this.chk23.Checked == true ? "항체항원검사," : "";
            this.gStrEXAMWAY_NM += this.chk24.Checked == true ? "간이진단키트," : "";
            this.gStrEXAMWAY_NM += this.chk29.Checked == true ? this.gStrEXAMWAYETC + "," : "";

            this.gStrEXAMWAY_NM = this.gStrEXAMWAY_NM.Substring(0, this.gStrEXAMWAY_NM.Length - 1);

            ePsmhReturnValue += new PSMH_RETURN_VALUE(ePSMH_RETURN_VALUE);
            this.ePsmhReturnValue(this.gStrEXAMTYPE, this.gStrEXAMTYPE_NM, this.gStrEXAMWAY, this.gStrEXAMWAY_NM, this.gStrEXAMTYPEETC, this.gStrEXAMWAYETC);

            this.Close();
        }

        void ePSMH_RETURN_VALUE(string strEXAMTYPE, string strEXAMTYPE_NM, string strEXAMWAY, string strEXAMWAY_NM, string strEXAMTYPEETC, string strEXAMWAYETC)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }           
        }

        void setCtrl()
        {
            setCtrlCheck();
        }

        void setCtrlCheck()
        {
            this.chk_11.Checked = false;
            this.chk_12.Checked = false;
            this.chk_13.Checked = false;
            this.chk_14.Checked = false;
            this.chk_15.Checked = false;
            this.chk_19.Checked = false;
            this.txt_1.Text     = string.Empty;

            this.chk21.Checked = false;
            this.chk22.Checked = false;
            this.chk23.Checked = false;
            this.chk24.Checked = false;
            this.chk29.Checked = false;
            this.txt_2.Text = string.Empty;

            this.txt_1.Text = "";
            this.txt_2.Text = "";

            //---------------------------------------

            if (string.IsNullOrEmpty(this.gStrEXAMTYPE) == false)
            {
                string[] examType = this.gStrEXAMTYPE.Split(',');

                for (int i = 0; i < examType.Length; i++)
                {
                    #region examTyhpe

                    if (examType[i].Equals("01") == true)
                    {
                        this.chk_11.Checked = true;
                    }
                    else if (examType[i].Equals("02") == true)
                    {
                        this.chk_12.Checked = true;
                    }
                    else if (examType[i].Equals("03") == true)
                    {
                        this.chk_13.Checked = true;
                    }
                    else if (examType[i].Equals("04") == true)
                    {
                        this.chk_14.Checked = true;
                    }
                    else if (examType[i].Equals("05") == true)
                    {
                        this.chk_15.Checked = true;
                    }
                    else if (examType[i].Equals("99") == true)
                    {
                        this.chk_19.Checked = true;
                        this.txt_1.Text = this.gStrEXAMTYPEETC.Trim();
                    }
                    #endregion
                }
            }

            if (string.IsNullOrEmpty(this.gStrEXAMWAY) == false)
            {
                string[] examWay = this.gStrEXAMWAY.Split(',');

                for (int i = 0; i < examWay.Length; i++)
                {
                    if (examWay[i].Equals("01") == true)
                    {
                        this.chk21.Checked = true;
                    }
                    else if (examWay[i].Equals("02") == true)
                    {
                        this.chk22.Checked = true;
                    }
                    else if (examWay[i].Equals("03") == true)
                    {
                        this.chk23.Checked = true;
                    }
                    else if (examWay[i].Equals("04") == true)
                    {
                        this.chk24.Checked = true;
                    }
                    else if (examWay[i].Equals("99") == true)
                    {
                        this.chk29.Checked = true;
                        this.txt_2.Text = this.gStrEXAMWAYETC.Trim();
                    }

                }
            }

        }









    }
}
