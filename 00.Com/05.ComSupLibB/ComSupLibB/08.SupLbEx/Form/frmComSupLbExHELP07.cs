using ComBase; //기본 클래스
using ComSupLibB.Com;
using System;
using System.Windows.Forms;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupLbEx
    /// File Name       : frmComSupHELP04.cs
    /// Description     : 진단검사의학과 HELP
    /// Author          : 김홍록
    /// Create Date     : 2018-02-13
    /// Update History  : 
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref= "exmain\FrmMsg.frm" />
    public partial class frmComSupLbExHELP07: Form
    {
        clsComSupLbExRsltSQL rsltSQL = new clsComSupLbExRsltSQL();

        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();

        string gStrPANO;       
        string gStrSNAME;


        public delegate void PSMH_RETURN_VALUE(string remaind);
        public event PSMH_RETURN_VALUE ePsmhReturnValue;


        public frmComSupLbExHELP07(string strPANO, string strSNAME)
        {
            InitializeComponent();

            this.gStrPANO       = strPANO;
            this.gStrSNAME      = strSNAME;

            setEvent();
        }

        void setEvent()
        {
            this.Load           += new EventHandler(eFormLoad);
            this.btnSave.Click  += new EventHandler(eBtnSave);

        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }
            else
            {
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                

                setCtrText();
            }
        }

        void setCtrText()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            this.lbl_PTINFO.Text = this.gStrPANO + "/" + this.gStrSNAME;
            this.txt_REMIND.Text = rsltSQL.sel_EXAM_PATIENT(clsDB.DbCon, this.gStrPANO);
        }

        void eBtnSave(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                string SqlErr = string.Empty;
                string SQL = string.Empty;
                int intRowAffected = 0;
                int chkRow = 0;

                Cursor.Current = Cursors.WaitCursor;

                SqlErr = rsltSQL.save_EXAM_PATIENT(clsDB.DbCon, this.gStrPANO, "","", this.txt_REMIND.Text.Trim(),true, ref intRowAffected, ref SQL);

                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    method.setTranction(clsDB.DbCon, true, SqlErr, SQL, chkRow);
                    return;
                }
               
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;

                this.ePsmhReturnValue += new PSMH_RETURN_VALUE(ePSMH_VALUE);
                this.ePsmhReturnValue(this.txt_REMIND.Text);

                this.Close();

            }
            catch (Exception)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 문제 발생");
            }


        }

        void ePSMH_VALUE(string remaind)
        {
        
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
