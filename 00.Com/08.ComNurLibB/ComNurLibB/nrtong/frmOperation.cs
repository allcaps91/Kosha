using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB;
using ComBase;
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmOperation.cs
    /// Description     : 수술실 일지
    /// Author          : 안정수
    /// Create Date     : 2018-02-06
    /// 사용안함
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 nrtong17.frm(FrmOperation) 폼 frmOperation.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\nurse\nrtong\nrtong17.frm(FrmOperation) >> frmOperation.cs 폼이름 재정의" />
    public partial class frmOperation : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();


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

        public frmOperation(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmOperation()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnView.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);

            this.dtpDate.LostFocus += new EventHandler(eControl_LostFocus);

            this.btnPrint.Click += new EventHandler(eBtnPrint);

            this.dtpDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);                        
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
                ComFunc.ReadSysDate(clsDB.DbCon);
                dtpDate.Text = clsPublic.GstrSysDate;
            }
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

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }

            else if (sender == this.btnCancel)
            {
                btnCancel_Click();
            }
        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }
        }

        void eControl_LostFocus(object sender, EventArgs e)
        {
            if(sender == this.dtpDate)
            {
                if (!VB.IsDate(dtpDate.Text))
                {
                    dtpDate.Text = clsPublic.GstrSysDate;
                    dtpDate.Focus();
                }

                if(String.Compare(dtpDate.Text, clsPublic.GstrSysDate) > 0)
                {
                    dtpDate.Text = clsPublic.GstrSysDate;
                }
            }
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(sender == this.dtpDate)
            {
                if(e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

        void btnCancel_Click()
        {
            dtpDate.Enabled = true;
            dtpDate.Focus();
            btnView.Enabled = true;
            btnCancel.Enabled = false;
            btnPrint.Enabled = false;
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            string PrintDate = "";
            string JobDate = "";
            string JobMan = "";

            PrintDate = clsPublic.GstrSysDate;
            JobDate = dtpDate.Text;
            JobMan = clsType.User.JobName;

            string strLine1 = VB.Space(30) + "┌─┬────┬────┬────┬────┐ " + "\n";
            string strLine2 = VB.Space(30) + "│결│임상감독│간호부장│행정처장│병 원 장│ " + "\n";
            string strLine3 = VB.Space(30) + "│　├────┼────┼────┼────┤ " + "\n";
            string strLine4 = VB.Space(30) + "│　│　　　　│　　　　│　　　　│　　　　│ " + "\n";
            string strLine5 = VB.Space(30) + "│재│　　　　│　　　　│　　　  │　　　  │ " + "\n";
            string strLine6 = VB.Space(30) + "└─┴────┴────┴────┴────┘ " + "\n";
       

            strTitle = "(" + JobDate + ")  수  술  실   일  지";
            strSubTitle = "작업년월: " + JobDate;

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strHeader += SPR.setSpdPrint_String(strLine1, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine2, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine3, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine4, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine5, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += SPR.setSpdPrint_String(strLine6, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);

            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("바탕체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);            

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 55, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);        
        }

        void eGetData()
        {
            int i = 0;

            string strDate = "";
            //string strDRCODE = "";
            //string strOpposition = "";
            //string strDrname = "";
            int RowCount = 0;

            strDate = dtpDate.Text;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  PANO,SNAME,DEPTCODE,AGE,SEX,OPTIMEFROM,OPTIMETO";
            SQL += ComNum.VBLF + "  ,DIAGNOSIS,OPTITLE,OPDOCT1,ANGBN, ANDOCT1,OPNURSE,ANNURSE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ORAN_MASTER";
            SQL += ComNum.VBLF + "WHERE OPDATE =TO_DATE('" + strDate + "','YYYY-MM-DD')";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당일자에 수술예약자가 없습니다.");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                ssList.ActiveSheet.Rows.Count = RowCount;

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    ssList.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim() + "";
                    ssList.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["Deptcode"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["optimefrom"].ToString().Trim() + dt.Rows[i]["optimefrom"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["diagnosis"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["optitle"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["angbn"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["opdoct1"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["andoct1"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["opnurse"].ToString().Trim() + "/" + dt.Rows[i]["annurse"].ToString().Trim();                    
                }
            }

            dt.Dispose();
            dt = null;
        }
    }
}
