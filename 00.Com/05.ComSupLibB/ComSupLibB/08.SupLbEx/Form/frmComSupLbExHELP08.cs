using ComBase; //기본 클래스
using ComSupLibB.Com;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupLbEx
    /// File Name       : frmComSupLbExHELP08.cs
    /// Description     : 진단검사의학과 HELP
    /// Author          : 김홍록
    /// Create Date     : 2018-03-22
    /// Update History  : 
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref= "exmain\FrmMsg.frm" />
    public partial class frmComSupLbExHELP08: Form
    { 

        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsComSupLbEx lbEX = new clsComSupLbEx();

        List<string> arr = null;

        public delegate void PSMH_RETURN_VALUE(string DEPTCODE);
        public event PSMH_RETURN_VALUE ePsmhReturnValue;

        public frmComSupLbExHELP08()
        {
            InitializeComponent();
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
            }

            setCtrl();
        }

        void setCtrl()
        {
            setCtrlCombo();

            setCtrlInit();
        }

        void setCtrlInit()
        {
            string strDEPT = comSql.sel_BAS_PCCONFIG(clsDB.DbCon, clsParam.EXAM_DEPT);

            bool isFalse = false;

            if (string.IsNullOrEmpty(strDEPT) == false)
            {
                for (int i = 0; i < this.arr.Count; i++)
                {
                    if (method.getGubunText(this.arr[i],".") == strDEPT)
                    {
                        this.cbo_DEPT.Text = this.arr[i];
                        isFalse = true;
                        break;
                    }
                }
            }

            if (isFalse == false)
            {
                this.cbo_DEPT.TabIndex = 0;
            }
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

                string strDEPTCODE = method.getGubunText(this.cbo_DEPT.Text, ".");

                SqlErr = comSql.del_BAS_PCCONFIG(clsDB.DbCon, clsParam.EXAM_DEPT, ref intRowAffected);                
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    method.setTranction(clsDB.DbCon, true, SqlErr, SQL, chkRow);
                    return;
                }

                SqlErr = comSql.ins_BAS_PCCONFIG(clsDB.DbCon, clsParam.EXAM_DEPT, strDEPTCODE, ref intRowAffected);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    method.setTranction(clsDB.DbCon, true, SqlErr, SQL, chkRow);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;

                this.ePsmhReturnValue += new PSMH_RETURN_VALUE(ePSMH_VALUE);
                this.ePsmhReturnValue(strDEPTCODE);

                this.Close();

            }
            catch (Exception)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 문제 발생");
            }
        }

        void setCtrlCombo()
        {
            arr = lbEX.set_EXAM_DEPT();

            if (arr.Count > 0)
            {
                method.setCombo_View(this.cbo_DEPT, arr, clsParam.enmComParamComboType.None);
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
