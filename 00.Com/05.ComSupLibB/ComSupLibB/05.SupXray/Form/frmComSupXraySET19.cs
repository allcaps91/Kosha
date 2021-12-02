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

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXraySET19.cs
    /// Description     : 폐암 판독문
    /// Author          : 안정수 
    /// Create Date     : 2019-07-25
    /// Update History  : 
    /// </summary>    
    /// 
    public partial class frmComSupXraySET19 : Form
    {
        #region 클래스 선언 및 etc....

        clsSpread methodSpd = new clsSpread();
        clsMethod method = new clsMethod();
        clsComSup sup = new clsComSup();
        clsSpread CS = new clsSpread();
        clsComSupXraySQL cSQL = new clsComSupXraySQL();

        clsComSupXrayRead.cHic_Xray_Result cHic_Xray_Result = null;  //저장

        public delegate void SendText(string strText);
        public event SendText rSendText;

        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;

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

        public frmComSupXraySET19(MainFormMessage pform, clsComSupXrayRead.cHic_Xray_Result argCls)
        {
            InitializeComponent();
            this.mCallForm = pform;
            cHic_Xray_Result = argCls;
            setEvent();
        }

        public frmComSupXraySET19(clsComSupXrayRead.cHic_Xray_Result argCls)
        {
            InitializeComponent();
            setEvent();
            cHic_Xray_Result = argCls;

            if (cHic_Xray_Result.strResult1.Length < 1000)
            { 
                txtResult.Text = cHic_Xray_Result.strResult1;
            }
            else
            {
                txtResult.Text = cHic_Xray_Result.strResult1 + cHic_Xray_Result.strResult2;
            }

            if(cHic_Xray_Result.PANCHK == "1")
            {
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }
        }

        void setCtrlData()
        {
          
        }

        void setCtrlInit()
        {
    
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Resize += new EventHandler(eFormResize);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
     
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnSave);
        }

        void setCombo()
        {
           
        }

        void setTxtTip()
        {
         
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

                //툴팁
                setTxtTip();

                setCtrlData();

                //설정정보 체크
                setCtrlInit();
            }

        }

        void eFormResize(object sender, EventArgs e)
        {
            
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
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            if (sender == this.btnSave)
            {
                if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                {
                    return; //권한 확인 
                }

                else 
                {
                    dt = cSQL.sel_Xray_Detail_SeekDate(clsDB.DbCon, cHic_Xray_Result.PTNO, cHic_Xray_Result.JEPDATE,false);

                    if(dt.Rows.Count > 0 && dt.Rows[0]["SEEKDATE"].ToString().Trim() != "")
                    {
                        cHic_Xray_Result.PASTCT = "2";
                        cHic_Xray_Result.PASTCTYYYY = VB.Left(dt.Rows[0]["SEEKDATE"].ToString().Trim(), 4);
                        cHic_Xray_Result.PASTCTMM = VB.Mid(dt.Rows[0]["SEEKDATE"].ToString().Trim(), 6, 2);
                    }
                    else
                    {
                        cHic_Xray_Result.PASTCT = "1";
                    }

                    dt.Dispose();
                    dt = null;

                    clsDB.setBeginTran(clsDB.DbCon);                    

                    SqlErr = cSQL.up_Hic_Xray_Result(clsDB.DbCon, cHic_Xray_Result, ref intRowAffected);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon); 
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    rSendText("ok");

                    this.Close();
                    return;
                }
            }

        }

    }
}
