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
    /// File Name       : frmJinNameView.cs
    /// Description     : 외래진료/퇴원자 명단조회
    /// Author          : 안정수
    /// Create Date     : 2018-02-06
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 nrtong41.frm(FrmJinNameView) 폼 frmJinNameView.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\nurse\nrtong\nrtong41.frm(FrmJinNameView) >> frmJinNameView.cs 폼이름 재정의" />
    public partial class frmJinNameView : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;
        //DataTable dt1 = null;
        DataTable dt2 = null;

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

        public frmJinNameView(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmJinNameView()
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

            //this.btnSearch.Click += new EventHandler(eBtnSearch);

            this.btnPrint.Click += new EventHandler(eBtnPrint);

            //this.eControl.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            //this.eControl.LostFocus += new EventHandler(eControl_LostFocus);
            //this.eControl.GotFocus += new EventHandler(eControl_GotFocus);
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

                Set_Init();
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

        void Set_Init()
        {
            int i = 0;
            string strData = "";

            cboDept.Items.Clear();
            cboDept.Items.Add("**.전체과");

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  DeptCode,DeptNameK";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ClinicDept";
            SQL += ComNum.VBLF + "ORDER BY PrintRanking";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strData = dt.Rows[i]["DeptCode"].ToString().Trim() + ".";
                    strData += dt.Rows[i]["DeptNameK"].ToString().Trim();
                    cboDept.Items.Add(strData);
                }
            }

            dt.Dispose();
            dt = null;

            cboDept.SelectedIndex = 0;

            dtpDate.Text = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-1).ToShortDateString();
            SCREEN_CLEAR();
        }

        void SCREEN_CLEAR()
        {
            ssList.ActiveSheet.Cells[0, 0, ssList.ActiveSheet.Rows.Count - 1, ssList.ActiveSheet.Columns.Count - 1].Text = "";

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

            if (optIO0.Checked == true)
            {
                strTitle = dtpDate.Text + "일 " + "외래진료 명단";
            }

            else if (optIO1.Checked == true)
            {
                strTitle = dtpDate.Text + "일 " + "퇴원자 명단";
            }

            strSubTitle = "인쇄일자 : " + clsPublic.GstrSysDate;


            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String("\r\n", new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, true, true, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        void eGetData()
        {
            int i = 0;
            //int j = 0;
            int nREAD = 0;
            int nRow = 0;

            string strPano = "";
            string strDept = "";
            string strNowDate = "";

            btnView.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            strDept = VB.Left(cboDept.SelectedItem.ToString().Trim(), 2);
            strNowDate = dtpDate.Text;

            if (optIO0.Checked == true)
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  Pano,Sname,DeptCode,DrCode,' ' WardCode";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_Master";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND ActDate = TO_DATE('" + strNowDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND Jin <> '2'";

                if (strDept != "**")
                {
                    SQL += ComNum.VBLF + "  AND DeptCode ='" + strDept + "'";
                }

                SQL += ComNum.VBLF + "ORDER BY DeptCode,DrCode,Sname";
            }

            else
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  Pano,Sname,DeptCode,DrCode,WardCode";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND ActDate = TO_DATE('" + strNowDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND OUTDATE IS NOT NULL ";

                if (strDept != "**")
                {
                    SQL += ComNum.VBLF + "  AND DeptCode = '" + strDept + "'";
                }

                SQL += ComNum.VBLF + "ORDER BY DeptCode,DrCode,Sname";
            }

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                btnCancel.Enabled = true;
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                btnCancel.Enabled = true;
                return;
            }

            if (dt.Rows.Count > 0)
            {
                //ssList.ActiveSheet.Rows.Count = dt.Rows.Count; ;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nREAD += 1;
                    nRow += 1;

                    if (nRow > ssList.ActiveSheet.Rows.Count)
                    {
                        ssList.ActiveSheet.Rows.Count = nRow;
                    }

                    //환자주소 READ
                    strPano = dt.Rows[i]["Pano"].ToString().Trim();

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  a.Tel,a.Juso,b.MailJuso";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT a, " + ComNum.DB_PMPA + "BAS_MAIL b";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND a.Pano = '" + strPano + "'";
                    SQL += ComNum.VBLF + "      AND (a.ZipCode1 || a.ZipCode2) = b.MailCode";

                    SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        btnCancel.Enabled = true;
                        return;
                    }

                    //if (dt2.Rows.Count == 0)
                    //{
                    //    dt2.Dispose();
                    //    dt2 = null;
                    //    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    //    //return;
                    //}

                    ssList.ActiveSheet.Cells[nRow - 1, 0].Text = strPano.PadLeft(8, '0');
                    ssList.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 3].Text = dt.Rows[i]["Deptcode"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, 5].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();

                    if (dt2.Rows.Count > 0)
                    {
                        ssList.ActiveSheet.Cells[nRow - 1, 2].Text = dt2.Rows[0]["Tel"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 6].Text = dt2.Rows[0]["MailJuso"].ToString().Trim() + " " + dt2.Rows[0]["Juso"].ToString().Trim();
                    }

                    dt2.Dispose();
                    dt2 = null;

                    //의사성명 READ
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  DrName";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_DOCTOR";
                    SQL += ComNum.VBLF + "WHERE DrCode = '" + dt.Rows[i]["DrCode"].ToString().Trim() + "'";

                    SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        btnCancel.Enabled = true;
                        return;
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        ssList.ActiveSheet.Cells[nRow - 1, 4].Text = dt2.Rows[0]["DrName"].ToString().Trim();
                    }

                    dt2.Dispose();
                    dt2 = null;
                }
            }

            dt.Dispose();
            dt = null;

            Cursor.Current = Cursors.Default;

            btnCancel.Enabled = true;
            btnPrint.Enabled = true;
        }

        void btnCancel_Click()
        {
            SCREEN_CLEAR();
            dtpDate.Focus();
        }


    }
}
