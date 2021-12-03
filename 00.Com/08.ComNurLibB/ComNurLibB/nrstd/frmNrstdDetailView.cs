using System;
using System.Data;
using System.Windows.Forms;
using ComBase;
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmNrstdDetailView.cs
    /// Description     : 간호 지표 상세 조회
    /// Author          : 안정수
    /// Create Date     : 2018-01-27    
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 Frm간호지표상세조회.frm(Frm간호지표상세조회) 폼 frmNrstdDetailView.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\nurse\nrstd\Frm간호지표상세조회.frm(Frm간호지표상세조회) >> frmNrstdDetailView.cs 폼이름 재정의" />
    public partial class frmNrstdDetailView : Form, MainFormMessage
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

        public frmNrstdDetailView(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmNrstdDetailView()
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

                dtpFDate.Text = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-7).ToShortDateString();
                dtpTDate.Text = clsPublic.GstrSysDate;

                opt2.Checked = true;

                ss낙상.Dock = DockStyle.Fill;
                ss투약오류.Dock = DockStyle.Fill;
                ss욕창.Dock = DockStyle.Fill;

                Set_Init();
                panTitle.Visible = false;
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
        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;


            strHeader = "";
            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, false, false, false);

            if (ss투약오류.Visible == true)
            {
                SPR.setSpdPrint(ss투약오류, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
            }

            if (ss낙상.Visible == true)
            {
                SPR.setSpdPrint(ss낙상, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
            }

            if (ss욕창.Visible == true)
            {
                SPR.setSpdPrint(ss욕창, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
            }
        }

        void Set_Init()
        {
            cboGubun.Items.Add("전체");
            cboGubun.Items.Add("투약오류");
            cboGubun.Items.Add("욕창");
            cboGubun.Items.Add("낙상");
            cboGubun.SelectedIndex = 0;

            ssList1.ActiveSheet.Rows.Count = 0;

            CLEAR_ERROR_TUYAK();
            CLEAR_ERROR_BRADEN();
            CLEAR_ERROR_FALL();


            cboWard.Items.Clear();

            cboWard_SET();

            if (ComQuery.NURSE_System_Manager_Check(Convert.ToInt64(clsType.User.Sabun)) == true)
            {
                cboWard.Enabled = true;
            }


            eGetData();
        }

        private void cboWard_SET()
        {
            int i = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  WardCode, WardName";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_WARD";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND WARDCODE NOT IN ('IU','NP','2W','NR','IQ','ER') ";
            SQL = SQL + ComNum.VBLF + "     AND USED = 'Y'  ";
            SQL += ComNum.VBLF + "ORDER BY WardCode ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            cboWard.Items.Clear();
            cboWard.Items.Add("전체");

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                }
            }

            cboWard.Items.Add("SICU");
            cboWard.Items.Add("MICU");
            cboWard.Items.Add("ER");

            dt.Dispose();
            dt = null;

            cboWard.SelectedIndex = 0;

            if (VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "") != "")
            {
                for (i = 0; i < cboWard.Items.Count; i++)
                {
                    if (cboWard.Items[i].ToString() == VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", ""))
                    {
                        cboWard.SelectedIndex = i;
                        cboWard.Enabled = false;
                        break;
                    }
                }
            }
        }


        void CLEAR_ERROR_TUYAK()
        {
            int i = 0;
            int j = 0;

            for (i = 4; i <= 8; i++)
            {
                ss투약오류.ActiveSheet.Cells[i, 2].Text = "";
                ss투약오류.ActiveSheet.Cells[i, 4].Text = "";
                ss투약오류.ActiveSheet.Cells[i, 6].Text = "";
            }

            ss투약오류.ActiveSheet.Cells[51, 1].Text = "";

            ss투약오류.ActiveSheet.Cells[27, 2].Text = "";
            ss투약오류.ActiveSheet.Cells[34, 4].Text = "";
            ss투약오류.ActiveSheet.Cells[17, 6].Text = "";
            ss투약오류.ActiveSheet.Cells[25, 6].Text = "";
            ss투약오류.ActiveSheet.Cells[31, 6].Text = "";
            ss투약오류.ActiveSheet.Cells[38, 6].Text = "";
            ss투약오류.ActiveSheet.Cells[14, 8].Text = "";
            ss투약오류.ActiveSheet.Cells[22, 8].Text = "";
            ss투약오류.ActiveSheet.Cells[38, 8].Text = "";

            //보고일, 부서, 보고자
            ss투약오류.ActiveSheet.Cells[49, 3].Text = "";
            ss투약오류.ActiveSheet.Cells[49, 8].Text = "";

            //J.항목
            ss투약오류.ActiveSheet.Cells[35, 1].Text = "";

            //K, L
            ss투약오류.ActiveSheet.Cells[41, 1].Text = "";
            ss투약오류.ActiveSheet.Cells[41, 5].Text = "";

            //체크박스
            for (i = 14; i <= 16; i++)
            {
                ss투약오류.ActiveSheet.Cells[i, 1].Text = "";
            }

            for (i = 18; i <= 27; i++)
            {
                if (i != 26)
                {
                    ss투약오류.ActiveSheet.Cells[i, 1].Text = "";
                }
            }

            for (i = 13; i <= 33; i++)
            {
                if (i != 15 && i != 17 && i != 20 && i != 22 && i != 28 && i != 30 && i != 32)
                {
                    ss투약오류.ActiveSheet.Cells[i, 3].Text = "";
                }
            }

            for (i = 13; i <= 38; i++)
            {
                if (i != 18 && i != 26 && i != 32 && i != 35)
                {
                    ss투약오류.ActiveSheet.Cells[i, 5].Text = "";
                }
            }

            for (i = 13; i <= 38; i++)
            {
                if (i != 15 && i != 26)
                {
                    ss투약오류.ActiveSheet.Cells[i, 7].Text = "";
                }
            }

            //M, L
            for (i = 45; i <= 48; i += 2)
            {
                for (j = 1; j <= 7; j += 2)
                {
                    ss투약오류.ActiveSheet.Cells[i, j].Text = "";
                }
            }
        }

        void CLEAR_ERROR_BRADEN()
        {
            int i = 0;
            txtDiag.Text = "";
            txtp_Jumsu.Text = "";
            txtPetc.Text = "";

            ComFunc.SetAllControlClear(groupBox6);
            ComFunc.SetAllControlClear(groupBox7);
            ComFunc.SetAllControlClear(groupBox8);
            ComFunc.SetAllControlClear(groupBox9);

            for (i = 2; i <= 10; i++)
            {
                ss욕창.ActiveSheet.Cells[6, i].Text = "";
            }

            for (i = 10; i <= 13; i++)
            {
                ss욕창.ActiveSheet.Cells[i, 4].Text = "";
            }

            for (i = 17; i <= 19; i++)
            {
                ss욕창.ActiveSheet.Cells[i, 2].Text = "";
            }

            for (i = 23; i <= 25; i++)
            {
                ss욕창.ActiveSheet.Cells[i, 2].Text = "";
            }

            for (i = 29; i <= 34; i++)
            {
                ss욕창.ActiveSheet.Cells[i, 2].Text = "";
            }

            ss욕창.ActiveSheet.Cells[36, 9].Text = "";
        }

        void CLEAR_ERROR_FALL()
        {
            int i = 0;

            //SS낙상.Sheet = 1

            for (i = 4; i <= 7; i++)
            {
                ss낙상.ActiveSheet.Cells[i, 2].Text = "";
                ss낙상.ActiveSheet.Cells[i, 4].Text = "";
                ss낙상.ActiveSheet.Cells[i, 6].Text = "";
                ss낙상.ActiveSheet.Cells[i, 9].Text = "";
            }

            ss낙상.ActiveSheet.Cells[9, 2].Text = "";
            ss낙상.ActiveSheet.Cells[9, 4].Text = "";

            ss낙상.ActiveSheet.Cells[10, 2].Text = "";
            ss낙상.ActiveSheet.Cells[10, 3].Text = "";
            ss낙상.ActiveSheet.Cells[10, 5].Text = "";
            ss낙상.ActiveSheet.Cells[10, 6].Text = "";
            ss낙상.ActiveSheet.Cells[10, 8].Text = "";

            ss낙상.ActiveSheet.Cells[11, 2].Text = "";
            ss낙상.ActiveSheet.Cells[11, 3].Text = "";
            ss낙상.ActiveSheet.Cells[11, 6].Text = "";
            ss낙상.ActiveSheet.Cells[11, 8].Text = "";

            ss낙상.ActiveSheet.Cells[12, 3].Text = "";
            ss낙상.ActiveSheet.Cells[12, 5].Text = "";
            ss낙상.ActiveSheet.Cells[12, 7].Text = "";

            ss낙상.ActiveSheet.Cells[13, 3].Text = "";
            ss낙상.ActiveSheet.Cells[13, 4].Text = "";
            ss낙상.ActiveSheet.Cells[13, 5].Text = "";
            ss낙상.ActiveSheet.Cells[13, 6].Text = "";
            ss낙상.ActiveSheet.Cells[13, 7].Text = "";
            ss낙상.ActiveSheet.Cells[13, 9].Text = "";

            ss낙상.ActiveSheet.Cells[14, 3].Text = "";
            ss낙상.ActiveSheet.Cells[14, 4].Text = "";
            ss낙상.ActiveSheet.Cells[14, 5].Text = "";
            ss낙상.ActiveSheet.Cells[14, 6].Text = "";
            ss낙상.ActiveSheet.Cells[14, 9].Text = "";

            ss낙상.ActiveSheet.Cells[15, 4].Text = "";

            ss낙상.ActiveSheet.Cells[17, 2].Text = "";

            ss낙상.ActiveSheet.Cells[19, 1].Text = "";
            ss낙상.ActiveSheet.Cells[19, 3].Text = "";
            ss낙상.ActiveSheet.Cells[19, 8].Text = "";
            ss낙상.ActiveSheet.Cells[19, 9].Text = "";

            ss낙상.ActiveSheet.Cells[20, 1].Text = "";
            ss낙상.ActiveSheet.Cells[20, 3].Text = "";
            ss낙상.ActiveSheet.Cells[20, 7].Text = "";

            ss낙상.ActiveSheet.Cells[21, 1].Text = "";
            ss낙상.ActiveSheet.Cells[21, 2].Text = "";
            ss낙상.ActiveSheet.Cells[21, 7].Text = "";

            //낙상장소
            ss낙상.ActiveSheet.Cells[23, 1].Text = "";
            ss낙상.ActiveSheet.Cells[23, 2].Text = "";
            ss낙상.ActiveSheet.Cells[23, 3].Text = "";
            ss낙상.ActiveSheet.Cells[23, 4].Text = "";
            ss낙상.ActiveSheet.Cells[23, 5].Text = "";

            ss낙상.ActiveSheet.Cells[24, 1].Text = "";
            ss낙상.ActiveSheet.Cells[24, 2].Text = "";
            ss낙상.ActiveSheet.Cells[24, 3].Text = "";
            ss낙상.ActiveSheet.Cells[24, 4].Text = "";
            ss낙상.ActiveSheet.Cells[24, 6].Text = "";

            ss낙상.ActiveSheet.Cells[25, 8].Text = "";
            ss낙상.ActiveSheet.Cells[25, 9].Text = "";

            //침대 낙상시
            ss낙상.ActiveSheet.Cells[26, 3].Text = "";
            ss낙상.ActiveSheet.Cells[26, 4].Text = "";
            ss낙상.ActiveSheet.Cells[26, 8].Text = "";
            ss낙상.ActiveSheet.Cells[26, 9].Text = "";

            ss낙상.ActiveSheet.Cells[27, 3].Text = "";
            ss낙상.ActiveSheet.Cells[27, 4].Text = "";
            ss낙상.ActiveSheet.Cells[27, 8].Text = "";
            ss낙상.ActiveSheet.Cells[27, 9].Text = "";

            ss낙상.ActiveSheet.Cells[28, 3].Text = "";
            ss낙상.ActiveSheet.Cells[28, 4].Text = "";

            ss낙상.ActiveSheet.Cells[29, 3].Text = "";
            ss낙상.ActiveSheet.Cells[29, 4].Text = "";
            ss낙상.ActiveSheet.Cells[29, 5].Text = "";

            ss낙상.ActiveSheet.Cells[30, 2].Text = "";

            //미끄러지거나 넘어진경우
            ss낙상.ActiveSheet.Cells[32, 3].Text = "";
            ss낙상.ActiveSheet.Cells[32, 4].Text = "";
            ss낙상.ActiveSheet.Cells[32, 5].Text = "";
            ss낙상.ActiveSheet.Cells[32, 7].Text = "";

            ss낙상.ActiveSheet.Cells[33, 3].Text = "";
            ss낙상.ActiveSheet.Cells[33, 4].Text = "";
            ss낙상.ActiveSheet.Cells[33, 5].Text = "";
            ss낙상.ActiveSheet.Cells[33, 7].Text = "";

            ss낙상.ActiveSheet.Cells[34, 3].Text = "";
            ss낙상.ActiveSheet.Cells[34, 4].Text = "";

            ss낙상.ActiveSheet.Cells[36, 1].Text = "";
            ss낙상.ActiveSheet.Cells[36, 4].Text = "";

            for (i = 37; i <= 43; i++)
            {
                ss낙상.ActiveSheet.Cells[i, 5].Text = "";
                ss낙상.ActiveSheet.Cells[i, 7].Text = "";
            }

            //IPDNO
            ss낙상.ActiveSheet.Cells[45, 1].Text = "";

            //발생일자
            ss낙상.ActiveSheet.Cells[45, 2].Text = "";
        }

        void eGetData()
        {
            int i = 0;
            int nREAD = 0;

            ssList1.ActiveSheet.Rows.Count = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  GUBUN, ACTDATE, INDATE, PANO, SEX, AGE, DEPTCODE, WARDCODE, MAX(RO) RO, SNAME";
            SQL += ComNum.VBLF + "FROM  ( ";
            SQL += ComNum.VBLF + "      SELECT";
            SQL += ComNum.VBLF + "          '욕창' GUBUN,  TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE, TO_CHAR(B.INDATE,'YYYY-MM-DD') INDATE,";
            SQL += ComNum.VBLF + "          A.PANO, A.SEX, A.AGE, A.DEPTCODE, A.WARDCODE, A.ROWID RO, B.SNAME";
            SQL += ComNum.VBLF + "      FROM " + ComNum.DB_PMPA + "NUR_PRESSURE_SORE A, " + ComNum.DB_PMPA + "IPD_NEW_MASTER B";
            SQL += ComNum.VBLF + "      WHERE 1=1";
            SQL += ComNum.VBLF + "          AND A.IPDNO = B.IPDNO";

            if (opt1.Checked == true)
            {
                SQL += ComNum.VBLF + "      AND A.ACTDATE >= TO_DATE('" + dtpFDate.Text + " 00:00','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "      AND A.ACTDATE <= TO_DATE('" + dtpTDate.Text + " 23:59','YYYY-MM-DD HH24:MI')";
            }

            else if (opt2.Checked == true)
            {
                SQL += ComNum.VBLF + "      AND B.INDATE >= TO_DATE('" + dtpFDate.Text + " 00:00','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "      AND B.INDATE <= TO_DATE('" + dtpTDate.Text + " 23:59','YYYY-MM-DD HH24:MI')";
            }

            SQL += ComNum.VBLF + "      UNION ALL";
            SQL += ComNum.VBLF + "      SELECT";
            SQL += ComNum.VBLF + "          '낙상' GUBUN, TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE, TO_CHAR(B.INDATE,'YYYY-MM-DD') INDATE,";
            SQL += ComNum.VBLF + "          A.PANO, A.SEX, A.AGE, A.DEPTCODE, A.WARDCODE, A.ROWID RO, B.SNAME";
            SQL += ComNum.VBLF + "      FROM " + ComNum.DB_PMPA + "NUR_FALL_REPORT A, " + ComNum.DB_PMPA + "IPD_NEW_MASTER B";
            SQL += ComNum.VBLF + "      WHERE 1=1";
            SQL += ComNum.VBLF + "          AND A.IPDNO = B.IPDNO";

            if (opt1.Checked == true)
            {
                SQL += ComNum.VBLF + "      AND A.ACTDATE >= TO_DATE('" + dtpFDate.Text + " 00:00','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "      AND A.ACTDATE <= TO_DATE('" + dtpTDate.Text + " 23:59','YYYY-MM-DD HH24:MI')";
            }

            else if (opt2.Checked == true)
            {
                SQL += ComNum.VBLF + "      AND B.INDATE >= TO_DATE('" + dtpFDate.Text + " 00:00','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "      AND B.INDATE <= TO_DATE('" + dtpTDate.Text + " 23:59','YYYY-MM-DD HH24:MI')";
            }

            SQL += ComNum.VBLF + "      UNION ALL";
            SQL += ComNum.VBLF + "      SELECT";
            SQL += ComNum.VBLF + "        '투약오류' GUBUN , TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE, TO_CHAR(B.INDATE,'YYYY-MM-DD') INDATE,";
            SQL += ComNum.VBLF + "        A.PANO, A.SEX, A.AGE, A.DEPTCODE, A.WARDCODE, A.ROWID RO, B.SNAME";
            SQL += ComNum.VBLF + "      FROM " + ComNum.DB_PMPA + "NUR_STD_DRUG A, " + ComNum.DB_PMPA + "IPD_NEW_MASTER B";
            SQL += ComNum.VBLF + "      WHERE 1=1";
            SQL += ComNum.VBLF + "          AND A.IPDNO = B.IPDNO";

            if (opt1.Checked == true)
            {
                SQL += ComNum.VBLF + "      AND A.ACTDATE >= TO_DATE('" + dtpFDate.Text + " 00:00','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "      AND A.ACTDATE <= TO_DATE('" + dtpTDate.Text + " 23:59','YYYY-MM-DD HH24:MI')";
            }

            else if (opt2.Checked == true)
            {
                SQL += ComNum.VBLF + "      AND B.INDATE >= TO_DATE('" + dtpFDate.Text + " 00:00','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "      AND B.INDATE <= TO_DATE('" + dtpTDate.Text + " 23:59','YYYY-MM-DD HH24:MI')";
            }

            SQL += ComNum.VBLF + "      ) ";
            SQL += ComNum.VBLF + "WHERE 1=1";

            if (cboGubun.SelectedItem.ToString().Trim() != "전체")
            {
                SQL += ComNum.VBLF + "  AND GUBUN = '" + cboGubun.SelectedItem.ToString().Trim() + "'";
            }

            if (cboWard.Text.Trim() != "전체")
            {
                SQL += ComNum.VBLF + "      AND WARDCODE = '" + cboWard.Text.Trim() + "'";
            }

            SQL += ComNum.VBLF + "GROUP BY GUBUN, ACTDATE, INDATE, PANO, SEX, AGE, DEPTCODE, WARDCODE, SNAME";

            if (opt1.Checked == true)
            {
                SQL += ComNum.VBLF + "ORDER BY ACTDATE, PANO";
            }

            else if (opt2.Checked == true)
            {
                SQL += ComNum.VBLF + "ORDER BY ACTDATE, PANO";
            }

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                nREAD = dt.Rows.Count;

                if (nREAD > 0)
                {
                    ssList1.ActiveSheet.Rows.Count = nREAD;
                    for (i = 0; i < nREAD; i++)
                    {
                        ssList1.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                        ssList1.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                        ssList1.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        ssList1.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList1.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["SEX"].ToString().Trim() + "/" + dt.Rows[i]["AGE"].ToString().Trim();
                        ssList1.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList1.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList1.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                        ssList1.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["RO"].ToString().Trim();
                    }
                }
            }

            dt.Dispose();
            dt = null;
        }

        void ssList1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            string strROWID = "";
            string strGubun = "";

            if (e.Row < 0)
            {
                return;
            }

            strGubun = ssList1.ActiveSheet.Cells[e.Row, 0].Text;
            strROWID = ssList1.ActiveSheet.Cells[e.Row, 8].Text;

            ss낙상.Visible = false;
            ss욕창.Visible = false;
            ss투약오류.Visible = false;

            CLEAR_ERROR_TUYAK();
            CLEAR_ERROR_FALL();
            CLEAR_ERROR_BRADEN();

            switch (strGubun)
            {
                case "투약오류":
                    ss투약오류.Visible = true;
                    ss욕창.Visible = false;
                    ss낙상.Visible = false;
                    READ_ERROR_TUYAK(strROWID);
                    break;

                case "낙상":
                    ss투약오류.Visible = false;
                    ss욕창.Visible = false;
                    ss낙상.Visible = true;
                    READ_ERROR_FALL(strROWID);
                    break;

                case "욕창":
                    ss투약오류.Visible = false;
                    ss욕창.Visible = true;
                    ss낙상.Visible = false;
                    READ_ERROR_BRADEN(strROWID);
                    break;
            }

        }

        void READ_ERROR_TUYAK(string argROWID)
        {
            int i = 0;

            string strTemp = "";

            CLEAR_ERROR_TUYAK();

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  PANO,IPDNO,TO_CHAR(ActDate,'YYYY-MM-DD HH24:MI') ActDate,TO_CHAR(BDate,'YYYY-MM-DD HH24:MI') BDate, ";
            SQL += ComNum.VBLF + "  TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,";
            SQL += ComNum.VBLF + "  JEPDATE,PANO,IPDNO,SNAME,SEX,AGE,WARDCODE,ROOMCODE,DEPTCODE,";
            SQL += ComNum.VBLF + "  BPLACE,BNAME,DIAG,DRCODE1,DRCODE2,DRUG1,DRUG2,DRUG2_ETC,DRUG3,DRUG3_ETC,";
            SQL += ComNum.VBLF + "  DRUGOP,DRUGOP_ETC,DRUGCPR,DRUGCPR_ETC,DRUGBLOOD,DRUGBLOOD_ETC,DRUGAIRWAY,";
            SQL += ComNum.VBLF + "  DRUGAIRWAY_ETC,DRUGCARE,DRUGCARE_ETC,DRUGTOOL,DRUGTOOL1_ETC,DRUGTOOL2_ETC,";
            SQL += ComNum.VBLF + "  DRUG7,DRUG8,DRUG9,DRUG10,DRUG11,DRUG12,DRUG13,DRUG14,DRUG15,DRUG16,DRUG17,DRUG18,";
            SQL += ComNum.VBLF + "  DRUG18_ETC , DRUG_J, DRUG_K, DRUG_L, DRUG_M, DRUG_N, BUSENAME,";
            SQL += ComNum.VBLF + "  Jumsu1,Jumsu2,Jumsu3,Jumsu4,Jumsu5,Jumsu6,Jumsu7,Jumsu8,Jumsu9,Jumsu10,Total,Remark,";
            SQL += ComNum.VBLF + "  ENTSABUN,ROWID";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_STD_DRUG";
            SQL += ComNum.VBLF + "WHERE ROWID = '" + argROWID + "' ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                ss투약오류.ActiveSheet.Cells[4, 2].Text = dt.Rows[0]["DeptCode"].ToString().Trim();
                ss투약오류.ActiveSheet.Cells[4, 4].Text = dt.Rows[0]["WardCode"].ToString().Trim();
                ss투약오류.ActiveSheet.Cells[4, 6].Text = dt.Rows[0]["DIAG"].ToString().Trim();

                ss투약오류.ActiveSheet.Cells[5, 2].Text = dt.Rows[0]["Pano"].ToString().Trim();
                ss투약오류.ActiveSheet.Cells[5, 6].Text = dt.Rows[0]["ActDate"].ToString().Trim();

                ss투약오류.ActiveSheet.Cells[6, 2].Text = dt.Rows[0]["SName"].ToString().Trim();
                ss투약오류.ActiveSheet.Cells[6, 6].Text = dt.Rows[0]["BDate"].ToString().Trim();

                ss투약오류.ActiveSheet.Cells[7, 2].Text = dt.Rows[0]["Sex"].ToString().Trim();
                ss투약오류.ActiveSheet.Cells[7, 4].Text = dt.Rows[0]["Age"].ToString().Trim();
                ss투약오류.ActiveSheet.Cells[7, 6].Text = dt.Rows[0]["BPlace"].ToString().Trim();

                ss투약오류.ActiveSheet.Cells[8, 2].Text = dt.Rows[0]["DrCode1"].ToString().Trim();
                ss투약오류.ActiveSheet.Cells[8, 4].Text = dt.Rows[0]["DrCode2"].ToString().Trim();
                ss투약오류.ActiveSheet.Cells[8, 6].Text = dt.Rows[0]["BName"].ToString().Trim();

                //.Col = 2: .Text = ArgIpdNo --> ?? 변수 없음
                ss투약오류.ActiveSheet.Cells[51, 2].Text = dt.Rows[0]["WardCode"].ToString().Trim();
                ss투약오류.ActiveSheet.Cells[51, 3].Text = dt.Rows[0]["RoomCode"].ToString().Trim();

                //내역
                strTemp = dt.Rows[0]["Drug1"].ToString().Trim();

                //투약전 발견
                for (i = 0; i <= strTemp.Length - 1; i++)
                {
                    if (VB.Mid(strTemp, i + 1, 1) == "1")
                    {
                        ss투약오류.ActiveSheet.Cells[i + 14, 1].Text = "True";
                    }
                }

                strTemp = dt.Rows[0]["Drug2"].ToString().Trim();

                //투약후 발견
                for (i = 0; i <= strTemp.Length - 1; i++)
                {
                    if (VB.Mid(strTemp, i + 1, 1) == "1")
                    {
                        if (i == 8)
                        {
                            ss투약오류.ActiveSheet.Cells[i + 18 + 1, 1].Text = "True";
                        }

                        else
                        {
                            ss투약오류.ActiveSheet.Cells[i + 18, 1].Text = "True";
                        }
                    }
                }

                ss투약오류.ActiveSheet.Cells[27, 2].Text = dt.Rows[0]["Drug2_Etc"].ToString().Trim();

                //투약 후 발견된 오류
                strTemp = dt.Rows[0]["Drug3"].ToString().Trim();

                ss투약오류.ActiveSheet.Cells[13, 3].Text = VB.Mid(strTemp, 1, 1);

                ss투약오류.ActiveSheet.Cells[14, 3].Text = VB.Mid(strTemp, 2, 1);

                ss투약오류.ActiveSheet.Cells[16, 3].Text = VB.Mid(strTemp, 3, 1);

                ss투약오류.ActiveSheet.Cells[18, 3].Text = VB.Mid(strTemp, 4, 1);

                ss투약오류.ActiveSheet.Cells[19, 3].Text = VB.Mid(strTemp, 5, 1);

                ss투약오류.ActiveSheet.Cells[21, 3].Text = VB.Mid(strTemp, 6, 1);

                ss투약오류.ActiveSheet.Cells[23, 3].Text = VB.Mid(strTemp, 7, 1);

                ss투약오류.ActiveSheet.Cells[24, 3].Text = VB.Mid(strTemp, 8, 1);

                ss투약오류.ActiveSheet.Cells[25, 3].Text = VB.Mid(strTemp, 9, 1);

                ss투약오류.ActiveSheet.Cells[26, 3].Text = VB.Mid(strTemp, 10, 1);

                ss투약오류.ActiveSheet.Cells[27, 3].Text = VB.Mid(strTemp, 11, 1);

                ss투약오류.ActiveSheet.Cells[29, 3].Text = VB.Mid(strTemp, 12, 1);

                ss투약오류.ActiveSheet.Cells[31, 3].Text = VB.Mid(strTemp, 13, 1);

                ss투약오류.ActiveSheet.Cells[33, 3].Text = VB.Mid(strTemp, 14, 1);
                ss투약오류.ActiveSheet.Cells[33, 4].Text = dt.Rows[0]["Drug3_Etc"].ToString().Trim();

                strTemp = dt.Rows[0]["DrugOp"].ToString().Trim();

                //수술관련
                for (i = 0; i <= strTemp.Length - 1; i++)
                {
                    if (VB.Mid(strTemp, i + 1, 1) == "1")
                    {
                        ss투약오류.ActiveSheet.Cells[i + 13, 5].Text = "True";
                    }
                }

                ss투약오류.ActiveSheet.Cells[17, 6].Text = dt.Rows[0]["DrugOp_Etc"].ToString().Trim();

                strTemp = dt.Rows[0]["DrugCPR"].ToString().Trim();

                //CPR
                for (i = 0; i <= strTemp.Length - 1; i++)
                {
                    if (VB.Mid(strTemp, i + 1, 1) == "1")
                    {
                        ss투약오류.ActiveSheet.Cells[i + 19, 5].Text = "True";
                    }
                }

                ss투약오류.ActiveSheet.Cells[25, 6].Text = dt.Rows[0]["DrugCPR_Etc"].ToString().Trim();

                strTemp = dt.Rows[0]["DrugBlood"].ToString().Trim();

                //수혈관련
                for (i = 0; i <= strTemp.Length - 1; i++)
                {
                    if (VB.Mid(strTemp, i + 1, 1) == "1")
                    {
                        ss투약오류.ActiveSheet.Cells[i + 27, 5].Text = "True";
                    }
                }

                ss투약오류.ActiveSheet.Cells[31, 6].Text = dt.Rows[0]["DrugBlood_Etc"].ToString().Trim();

                //AirWay
                strTemp = dt.Rows[0]["DrugAirway"].ToString().Trim();

                ss투약오류.ActiveSheet.Cells[33, 5].Text = VB.Mid(strTemp, 1, 1);
                ss투약오류.ActiveSheet.Cells[34, 5].Text = VB.Mid(strTemp, 2, 1);
                ss투약오류.ActiveSheet.Cells[36, 5].Text = VB.Mid(strTemp, 3, 1);
                ss투약오류.ActiveSheet.Cells[37, 5].Text = VB.Mid(strTemp, 4, 1);

                ss투약오류.ActiveSheet.Cells[38, 5].Text = VB.Mid(strTemp, 5, 1);
                ss투약오류.ActiveSheet.Cells[38, 6].Text = dt.Rows[0]["DrugAirway_Etc"].ToString().Trim();

                strTemp = dt.Rows[0]["DrugCare"].ToString().Trim();

                //Care
                for (i = 0; i <= strTemp.Length - 1; i++)
                {
                    if (VB.Mid(strTemp, i + 1, 1) == "1")
                    {
                        ss투약오류.ActiveSheet.Cells[i + 13, 7].Text = "True";
                    }
                }

                ss투약오류.ActiveSheet.Cells[14, 8].Text = dt.Rows[0]["DrugCare_Etc"].ToString().Trim();

                strTemp = dt.Rows[0]["DrugTool"].ToString().Trim();

                //Care
                for (i = 0; i <= strTemp.Length - 1; i++)
                {
                    if (VB.Mid(strTemp, i + 1, 1) == "1")
                    {
                        ss투약오류.ActiveSheet.Cells[i + 16, 7].Text = "True";
                    }
                }

                ss투약오류.ActiveSheet.Cells[19, 8].Text = dt.Rows[0]["DrugTool1_Etc"].ToString().Trim();
                ss투약오류.ActiveSheet.Cells[22, 8].Text = dt.Rows[0]["DrugTool2_Etc"].ToString().Trim();

                ss투약오류.ActiveSheet.Cells[24, 7].Text = dt.Rows[0]["Drug7"].ToString().Trim();
                ss투약오류.ActiveSheet.Cells[25, 7].Text = dt.Rows[0]["Drug8"].ToString().Trim();

                strTemp = dt.Rows[0]["Drug9"].ToString().Trim();
                //Care
                for (i = 0; i <= strTemp.Length - 1; i++)
                {
                    if (VB.Mid(strTemp, i + 1, 1) == "1")
                    {
                        ss투약오류.ActiveSheet.Cells[i + 27, 7].Text = "True";
                    }
                }

                //화상
                ss투약오류.ActiveSheet.Cells[30, 7].Text = dt.Rows[0]["Drug10"].ToString().Trim();
                ss투약오류.ActiveSheet.Cells[31, 7].Text = dt.Rows[0]["Drug11"].ToString().Trim();
                ss투약오류.ActiveSheet.Cells[32, 7].Text = dt.Rows[0]["Drug12"].ToString().Trim();
                ss투약오류.ActiveSheet.Cells[33, 7].Text = dt.Rows[0]["Drug13"].ToString().Trim();
                ss투약오류.ActiveSheet.Cells[34, 7].Text = dt.Rows[0]["Drug14"].ToString().Trim();
                ss투약오류.ActiveSheet.Cells[35, 7].Text = dt.Rows[0]["Drug15"].ToString().Trim();
                ss투약오류.ActiveSheet.Cells[36, 7].Text = dt.Rows[0]["Drug16"].ToString().Trim();
                ss투약오류.ActiveSheet.Cells[37, 7].Text = dt.Rows[0]["Drug17"].ToString().Trim();

                ss투약오류.ActiveSheet.Cells[38, 7].Text = dt.Rows[0]["Drug18"].ToString().Trim();
                ss투약오류.ActiveSheet.Cells[38, 8].Text = dt.Rows[0]["Drug18_Etc"].ToString().Trim();

                //J, K, L
                ss투약오류.ActiveSheet.Cells[35, 1].Text = dt.Rows[0]["Drug_J"].ToString().Trim();

                ss투약오류.ActiveSheet.Cells[41, 1].Text = dt.Rows[0]["Drug_K"].ToString().Trim();
                ss투약오류.ActiveSheet.Cells[41, 5].Text = dt.Rows[0]["Drug_L"].ToString().Trim();

                //M, N
                ss투약오류.ActiveSheet.Cells[45, 1].Text = VB.Mid(dt.Rows[0]["Drug_M"].ToString().Trim(), 1, 1);
                ss투약오류.ActiveSheet.Cells[45, 3].Text = VB.Mid(dt.Rows[0]["Drug_M"].ToString().Trim(), 2, 1);

                ss투약오류.ActiveSheet.Cells[46, 1].Text = VB.Mid(dt.Rows[0]["Drug_M"].ToString().Trim(), 3, 1);
                ss투약오류.ActiveSheet.Cells[46, 3].Text = VB.Mid(dt.Rows[0]["Drug_M"].ToString().Trim(), 4, 1);

                ss투약오류.ActiveSheet.Cells[47, 1].Text = VB.Mid(dt.Rows[0]["Drug_M"].ToString().Trim(), 5, 1);

                ss투약오류.ActiveSheet.Cells[45, 5].Text = VB.Mid(dt.Rows[0]["Drug_N"].ToString().Trim(), 1, 1);
                ss투약오류.ActiveSheet.Cells[45, 7].Text = VB.Mid(dt.Rows[0]["Drug_N"].ToString().Trim(), 2, 1);

                ss투약오류.ActiveSheet.Cells[46, 5].Text = VB.Mid(dt.Rows[0]["Drug_N"].ToString().Trim(), 3, 1);
                ss투약오류.ActiveSheet.Cells[46, 7].Text = VB.Mid(dt.Rows[0]["Drug_N"].ToString().Trim(), 4, 1);

                ss투약오류.ActiveSheet.Cells[47, 5].Text = VB.Mid(dt.Rows[0]["Drug_N"].ToString().Trim(), 5, 1);

                //보고정보
                ss투약오류.ActiveSheet.Cells[49, 3].Text = dt.Rows[0]["JepDate"].ToString().Trim();
                ss투약오류.ActiveSheet.Cells[49, 6].Text = dt.Rows[0]["BuseName"].ToString().Trim();
                ss투약오류.ActiveSheet.Cells[49, 8].Text = CF.Read_SabunName(clsDB.DbCon, dt.Rows[0]["EntSabun"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;
        }

        void READ_ERROR_FALL(string argROWID)
        {
            int k = 0;

            CLEAR_ERROR_FALL();

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  PANO,IPDNO,TO_CHAR(ActDate,'YYYY-MM-DD HH24:MI') ActDate,TO_CHAR(ActDate,'YYYY-MM-DD') BDate,";
            SQL += ComNum.VBLF + "  TO_CHAR(SeekDate,'YYYY-MM-DD HH24:MI') SeekDate,";
            SQL += ComNum.VBLF + "  TO_CHAR(ReturnDate,'YYYY-MM-DD HH24:MI') ReturnDate, SNAME,SEX,AGE,DIAGNOSYS,ROOMCODE,DEPTCODE,EntSabun, ";
            SQL += ComNum.VBLF + "  WardCode,DIAGNOSYS,ENTDATE, ";
            SQL += ComNum.VBLF + "  EtcName, Weight,Height, Nur_Fall1,Nur_Fall2,Nur_Fall3,Nur_Fall3_Etc, ";
            SQL += ComNum.VBLF + "  Nur_Fall4,Nur_Fall4_Etc,Nur_Fall5,Nur_Fall6,Nur_Fall6_Etc,Nur_Fall7,Nur_Fall8,Nur_Fall9,Nur_Fall10, ";
            SQL += ComNum.VBLF + "  Nur_Fall11,Nur_Fall11_Etc,Nur_Fall12,Nur_Fall13,Nur_Fall14,Nur_Fall15,Nur_Fall16,Nur_Fall17,Nur_Fall18,";
            SQL += ComNum.VBLF + "  Nur_Fall19,Nur_Fall20,Nur_Fall21,Nur_Fall22,Nur_Fall23,Nur_Fall24,Nur_Fall25,Nur_Fall26,Nur_Fall27,ROWID, PRTYN";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_FALL_REPORT";
            SQL += ComNum.VBLF + "WHERE ROWID = '" + argROWID + "' ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                ss낙상.ActiveSheet.Cells[4, 2].Text = dt.Rows[0]["SName"].ToString().Trim();
                ss낙상.ActiveSheet.Cells[4, 4].Text = dt.Rows[0]["Sex"].ToString().Trim() + "/" + dt.Rows[0]["Age"].ToString().Trim();
                ss낙상.ActiveSheet.Cells[4, 6].Text = dt.Rows[0]["ActDate"].ToString().Trim();

                ss낙상.ActiveSheet.Cells[5, 2].Text = dt.Rows[0]["Pano"].ToString().Trim();
                ss낙상.ActiveSheet.Cells[5, 6].Text = dt.Rows[0]["SeekDate"].ToString().Trim();

                ss낙상.ActiveSheet.Cells[6, 2].Text = dt.Rows[0]["WardCode"].ToString().Trim();
                ss낙상.ActiveSheet.Cells[6, 4].Text = dt.Rows[0]["DeptCode"].ToString().Trim();
                ss낙상.ActiveSheet.Cells[6, 6].Text = dt.Rows[0]["ReturnDate"].ToString().Trim();

                ss낙상.ActiveSheet.Cells[7, 2].Text = dt.Rows[0]["DIAGNOSYS"].ToString().Trim();
                ss낙상.ActiveSheet.Cells[7, 6].Text = CF.Read_SabunName(clsDB.DbCon, dt.Rows[0]["EntSabun"].ToString().Trim());
                ss낙상.ActiveSheet.Cells[7, 9].Text = dt.Rows[0]["EtcName"].ToString().Trim();

                //환자관련사항
                ss낙상.ActiveSheet.Cells[9, 2].Text = dt.Rows[0]["Weight"].ToString().Trim();
                ss낙상.ActiveSheet.Cells[9, 4].Text = dt.Rows[0]["Height"].ToString().Trim();

                ss낙상.ActiveSheet.Cells[10, 2].Text = VB.Mid(dt.Rows[0]["Nur_Fall1"].ToString().Trim(), 1, 1);
                ss낙상.ActiveSheet.Cells[10, 3].Text = VB.Mid(dt.Rows[0]["Nur_Fall1"].ToString().Trim(), 2, 1);
                ss낙상.ActiveSheet.Cells[10, 5].Text = VB.Mid(dt.Rows[0]["Nur_Fall1"].ToString().Trim(), 3, 1);
                ss낙상.ActiveSheet.Cells[10, 6].Text = VB.Mid(dt.Rows[0]["Nur_Fall1"].ToString().Trim(), 4, 1);
                ss낙상.ActiveSheet.Cells[10, 8].Text = VB.Mid(dt.Rows[0]["Nur_Fall1"].ToString().Trim(), 5, 1);

                ss낙상.ActiveSheet.Cells[11, 2].Text = VB.Mid(dt.Rows[0]["Nur_Fall2"].ToString().Trim(), 1, 1);
                ss낙상.ActiveSheet.Cells[11, 3].Text = VB.Mid(dt.Rows[0]["Nur_Fall2"].ToString().Trim(), 2, 1);
                ss낙상.ActiveSheet.Cells[11, 6].Text = VB.Mid(dt.Rows[0]["Nur_Fall2"].ToString().Trim(), 3, 1);
                ss낙상.ActiveSheet.Cells[11, 8].Text = VB.Mid(dt.Rows[0]["Nur_Fall2"].ToString().Trim(), 4, 1);

                if (dt.Rows[0]["Nur_Fall3"].ToString().Trim() == "1")
                {
                    ss낙상.ActiveSheet.Cells[12, 3].Text = "True";
                }

                else
                {
                    ss낙상.ActiveSheet.Cells[12, 7].Text = "True";
                }

                ss낙상.ActiveSheet.Cells[12, 5].Text = dt.Rows[0]["Nur_Fall3_Etc"].ToString().Trim();

                ss낙상.ActiveSheet.Cells[13, 3].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 1, 1);
                ss낙상.ActiveSheet.Cells[13, 4].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 2, 1);
                ss낙상.ActiveSheet.Cells[13, 5].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 3, 1);
                ss낙상.ActiveSheet.Cells[13, 6].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 4, 1);
                ss낙상.ActiveSheet.Cells[13, 7].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 5, 1);
                ss낙상.ActiveSheet.Cells[13, 9].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 6, 1);

                ss낙상.ActiveSheet.Cells[14, 3].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 1, 1);
                ss낙상.ActiveSheet.Cells[14, 4].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 2, 1);
                ss낙상.ActiveSheet.Cells[14, 5].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 3, 1);
                ss낙상.ActiveSheet.Cells[14, 6].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 4, 1);
                ss낙상.ActiveSheet.Cells[14, 9].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 5, 1);

                ss낙상.ActiveSheet.Cells[15, 4].Text = dt.Rows[0]["Nur_Fall4_Etc"].ToString().Trim();

                ss낙상.ActiveSheet.Cells[17, 2].Text = dt.Rows[0]["Nur_Fall5"].ToString().Trim();

                //낙상유형
                ss낙상.ActiveSheet.Cells[19, 1].Text = VB.Mid(dt.Rows[0]["Nur_Fall6"].ToString().Trim(), 1, 1);
                ss낙상.ActiveSheet.Cells[19, 3].Text = VB.Mid(dt.Rows[0]["Nur_Fall6"].ToString().Trim(), 2, 1);

                ss낙상.ActiveSheet.Cells[20, 1].Text = VB.Mid(dt.Rows[0]["Nur_Fall6"].ToString().Trim(), 3, 1);
                ss낙상.ActiveSheet.Cells[20, 3].Text = VB.Mid(dt.Rows[0]["Nur_Fall6"].ToString().Trim(), 4, 1);

                ss낙상.ActiveSheet.Cells[21, 1].Text = VB.Mid(dt.Rows[0]["Nur_Fall6"].ToString().Trim(), 5, 1);
                ss낙상.ActiveSheet.Cells[21, 2].Text = dt.Rows[0]["Nur_Fall6_Etc"].ToString().Trim();

                //낙상장소
                ss낙상.ActiveSheet.Cells[23, 1].Text = VB.Mid(dt.Rows[0]["Nur_Fall7"].ToString().Trim(), 1, 1);
                ss낙상.ActiveSheet.Cells[23, 2].Text = VB.Mid(dt.Rows[0]["Nur_Fall7"].ToString().Trim(), 2, 1);
                ss낙상.ActiveSheet.Cells[23, 3].Text = VB.Mid(dt.Rows[0]["Nur_Fall7"].ToString().Trim(), 3, 1);
                ss낙상.ActiveSheet.Cells[23, 4].Text = VB.Mid(dt.Rows[0]["Nur_Fall7"].ToString().Trim(), 4, 1);

                ss낙상.ActiveSheet.Cells[24, 1].Text = VB.Mid(dt.Rows[0]["Nur_Fall7"].ToString().Trim(), 5, 1);
                ss낙상.ActiveSheet.Cells[24, 2].Text = VB.Mid(dt.Rows[0]["Nur_Fall7"].ToString().Trim(), 6, 1);
                ss낙상.ActiveSheet.Cells[24, 3].Text = VB.Mid(dt.Rows[0]["Nur_Fall7"].ToString().Trim(), 7, 1);
                ss낙상.ActiveSheet.Cells[24, 4].Text = VB.Mid(dt.Rows[0]["Nur_Fall7"].ToString().Trim(), 8, 1);


                //침상낙상시
                if (dt.Rows[0]["Nur_Fall8"].ToString().Trim() == "1")
                {
                    ss낙상.ActiveSheet.Cells[26, 3].Text = "True";
                }

                else
                {
                    ss낙상.ActiveSheet.Cells[26, 4].Text = "True";
                }

                if (dt.Rows[0]["Nur_Fall9"].ToString().Trim() == "1")
                {
                    ss낙상.ActiveSheet.Cells[27, 3].Text = "True";
                }

                else
                {
                    ss낙상.ActiveSheet.Cells[27, 4].Text = "True";
                }

                if (dt.Rows[0]["Nur_Fall10"].ToString().Trim() == "1")
                {
                    ss낙상.ActiveSheet.Cells[28, 3].Text = "True";
                }

                else
                {
                    ss낙상.ActiveSheet.Cells[28, 4].Text = "True";
                }

                if (dt.Rows[0]["Nur_Fall11"].ToString().Trim() == "1")
                {
                    ss낙상.ActiveSheet.Cells[29, 3].Text = "True";
                }

                else
                {
                    ss낙상.ActiveSheet.Cells[29, 4].Text = "True";
                }

                ss낙상.ActiveSheet.Cells[30, 2].Text = dt.Rows[0]["Nur_Fall11_Etc"].ToString().Trim();

                //미끄러짐?
                if (dt.Rows[0]["Nur_Fall12"].ToString().Trim() == "1")
                {
                    ss낙상.ActiveSheet.Cells[32, 3].Text = "True";
                }

                else
                {
                    ss낙상.ActiveSheet.Cells[32, 4].Text = "True";
                }

                if (dt.Rows[0]["Nur_Fall13"].ToString().Trim() == "1")
                {
                    ss낙상.ActiveSheet.Cells[33, 3].Text = "True";
                }

                else
                {
                    ss낙상.ActiveSheet.Cells[33, 4].Text = "True";
                }

                if (dt.Rows[0]["Nur_Fall14"].ToString().Trim() == "1")
                {
                    ss낙상.ActiveSheet.Cells[34, 3].Text = "True";
                }

                else
                {
                    ss낙상.ActiveSheet.Cells[34, 4].Text = "True";
                }

                //간호중재
                if (dt.Rows[0]["Nur_Fall15"].ToString().Trim() == "1")
                {
                    ss낙상.ActiveSheet.Cells[19, 8].Text = "True";
                }

                else
                {
                    ss낙상.ActiveSheet.Cells[19, 9].Text = "True";
                }


                ss낙상.ActiveSheet.Cells[20, 7].Text = dt.Rows[0]["Nur_Fall16"].ToString().Trim();
                ss낙상.ActiveSheet.Cells[21, 7].Text = dt.Rows[0]["Nur_Fall17"].ToString().Trim();
                ss낙상.ActiveSheet.Cells[23, 5].Text = dt.Rows[0]["Nur_Fall18"].ToString().Trim();
                ss낙상.ActiveSheet.Cells[24, 6].Text = dt.Rows[0]["Nur_Fall19"].ToString().Trim();

                if (dt.Rows[0]["Nur_Fall20"].ToString().Trim() == "1")
                {
                    ss낙상.ActiveSheet.Cells[25, 8].Text = "True";
                }

                else
                {
                    ss낙상.ActiveSheet.Cells[25, 9].Text = "True";
                }

                if (dt.Rows[0]["Nur_Fall21"].ToString().Trim() == "1")
                {
                    ss낙상.ActiveSheet.Cells[26, 8].Text = "True";
                }

                else
                {
                    ss낙상.ActiveSheet.Cells[26, 9].Text = "True";
                }

                if (dt.Rows[0]["Nur_Fall22"].ToString().Trim() == "1")
                {
                    ss낙상.ActiveSheet.Cells[27, 8].Text = "True";
                }

                else
                {
                    ss낙상.ActiveSheet.Cells[27, 9].Text = "True";
                }

                ss낙상.ActiveSheet.Cells[29, 5].Text = dt.Rows[0]["Nur_Fall23"].ToString().Trim();

                //낙상결과
                ss낙상.ActiveSheet.Cells[32, 5].Text = VB.Mid(dt.Rows[0]["Nur_Fall24"].ToString().Trim(), 1, 1);
                ss낙상.ActiveSheet.Cells[32, 7].Text = VB.Mid(dt.Rows[0]["Nur_Fall24"].ToString().Trim(), 2, 1);

                ss낙상.ActiveSheet.Cells[33, 5].Text = VB.Mid(dt.Rows[0]["Nur_Fall24"].ToString().Trim(), 3, 1);
                ss낙상.ActiveSheet.Cells[33, 8].Text = VB.Mid(dt.Rows[0]["Nur_Fall24"].ToString().Trim(), 4, 1);

                //환자의 신체적 손상 및 치료
                for (k = 37; k <= 43; k++)
                {
                    if (VB.Mid(dt.Rows[0]["Nur_Fall25"].ToString().Trim(), k - 36, 1) == "1")
                    {
                        ss낙상.ActiveSheet.Cells[k, 5].Text = "True";
                    }

                    else
                    {
                        ss낙상.ActiveSheet.Cells[k, 5].Text = "False";
                    }
                }

                for (k = 37; k <= 42; k++)
                {
                    if (VB.Mid(dt.Rows[0]["Nur_Fall26"].ToString().Trim(), k - 36, 1) == "1")
                    {
                        ss낙상.ActiveSheet.Cells[k, 7].Text = "True";
                    }

                    else
                    {
                        ss낙상.ActiveSheet.Cells[k, 7].Text = "False";
                    }
                }

                //낙상발생 상활 기술s\


                ss낙상.ActiveSheet.Cells[36, 1].Text = dt.Rows[0]["Nur_Fall27"].ToString().Trim();

                ss낙상.ActiveSheet.Cells[45, 1].Text = dt.Rows[0]["IpdNo"].ToString().Trim();
                ss낙상.ActiveSheet.Cells[45, 2].Text = dt.Rows[0]["BDate"].ToString().Trim();
                ss낙상.ActiveSheet.Cells[45, 3].Text = dt.Rows[0]["ENTDATE"].ToString().Trim();
                ss낙상.ActiveSheet.Cells[45, 4].Text = dt.Rows[0]["PRTYN"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
        }

        void READ_ERROR_BRADEN(string argROWID)
        {
            int k = 0;
            int nTOT1 = 0;
            string strTemp = "";

            CLEAR_ERROR_BRADEN();

            CheckBox[] ChkP_1 = new CheckBox[10] { ChkP_10, ChkP_11, ChkP_12, ChkP_13, ChkP_14,
                                                   ChkP_15, ChkP_16, ChkP_17, ChkP_18, ChkP_19};

            CheckBox[] ChkP_2 = new CheckBox[14] { ChkP_20, ChkP_21, ChkP_22, ChkP_23, ChkP_24,
                                                   ChkP_25, ChkP_26, ChkP_27, ChkP_28, ChkP_29,
                                                   ChkP_210, ChkP_211, ChkP_212, ChkP_213};

            CheckBox[] ChkP_3 = new CheckBox[9] { ChkP_30, ChkP_31, ChkP_32, ChkP_33, ChkP_34,
                                                  ChkP_35, ChkP_36, ChkP_37, ChkP_38};

            TextBox[] Txt_Pstep = new TextBox[10] {txt_Pstep0, txt_Pstep1, txt_Pstep2, txt_Pstep3, txt_Pstep4,
                                                   txt_Pstep5, txt_Pstep6, txt_Pstep7, txt_Pstep8,  txt_Pstep9};

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  PANO,IPDNO,TO_CHAR(ActDate,'YYYY-MM-DD') ActDate,TO_CHAR(InDate,'YYYY-MM-DD')InDate,";
            SQL += ComNum.VBLF + "  SNAME,SEX,AGE,DIAGNOSYS,ROOMCODE,DEPTCODE,EntSabun,";
            SQL += ComNum.VBLF + "  GRADE,TOTAL,P_BALBUI,P_BALBUI_etc,P_STEP,P_HAPBUNG,P_PROGRESS,P_YOIN,P_PRE,REMARK,WardCode,ROWID, ENTDATE, PRTYN ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_PRESSURE_SORE";
            SQL += ComNum.VBLF + "WHERE ROWID = '" + argROWID + "'";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                ss욕창.ActiveSheet.Cells[6, 2].Text = dt.Rows[0]["InDate"].ToString().Trim();
                ss욕창.ActiveSheet.Cells[6, 3].Text = dt.Rows[0]["ActDate"].ToString().Trim();
                ss욕창.ActiveSheet.Cells[6, 4].Text = CF.DATE_ILSU(clsDB.DbCon, dt.Rows[0]["ActDate"].ToString().Trim(), dt.Rows[0]["InDate"].ToString().Trim()).ToString();
                ss욕창.ActiveSheet.Cells[6, 5].Text = dt.Rows[0]["Pano"].ToString().Trim();
                ss욕창.ActiveSheet.Cells[6, 6].Text = dt.Rows[0]["SName"].ToString().Trim();
                ss욕창.ActiveSheet.Cells[6, 7].Text = dt.Rows[0]["Sex"].ToString().Trim() + "/" + dt.Rows[0]["Age"].ToString().Trim();
                ss욕창.ActiveSheet.Cells[6, 8].Text = dt.Rows[0]["DeptCode"].ToString().Trim();
                ss욕창.ActiveSheet.Cells[6, 9].Text = dt.Rows[0]["RoomCode"].ToString().Trim();
                ss욕창.ActiveSheet.Cells[6, 10].Text = dt.Rows[0]["Grade"].ToString().Trim();

                txtDiag.Text = dt.Rows[0]["DIAGNOSYS"].ToString().Trim();

                //등록일의 최근값
                txtp_Jumsu.Text = nTOT1.ToString();
                txtPetc.Text = dt.Rows[0]["P_BALBUI_etc"].ToString().Trim();

                for (k = 0; k <= VB.I(dt.Rows[0]["P_BALBUI"].ToString().Trim(), "^^") - 1; k++)
                {
                    if (VB.Pstr(dt.Rows[0]["P_BALBUI"].ToString().Trim(), "^^", k + 1) == "1")
                    {
                        ChkP_1[k].Checked = true;
                    }

                    if (String.Compare(VB.Pstr(dt.Rows[0]["P_STEP"].ToString().Trim(), "^^", k + 1), "0") > 0)
                    {
                        Txt_Pstep[k].Text = VB.Pstr(dt.Rows[0]["P_STEP"].ToString().Trim(), "^^", k + 1);
                    }
                }

                if (dt.Rows[0]["P_HAPBUNG"].ToString().Trim() == "Y")
                {
                    opt_P20.Checked = true;
                }

                else
                {
                    opt_P21.Checked = true;
                }

                if (dt.Rows[0]["P_PROGRESS"].ToString().Trim() == "1")
                {
                    Opt_P30.Checked = true;
                }

                else if (dt.Rows[0]["P_PROGRESS"].ToString().Trim() == "2")
                {
                    Opt_P31.Checked = true;
                }

                else if (dt.Rows[0]["P_PROGRESS"].ToString().Trim() == "3")
                {
                    Opt_P32.Checked = true;
                }

                else if (dt.Rows[0]["P_PROGRESS"].ToString().Trim() == "4")
                {
                    Opt_P33.Checked = true;
                }

                for (k = 0; k <= VB.I(dt.Rows[0]["P_YOIN"].ToString().Trim(), "^^") - 1; k++)
                {
                    if (VB.Pstr(dt.Rows[0]["P_YOIN"].ToString().Trim(), "^^", k + 1) == "1")
                    {
                        ChkP_2[k].Checked = true;
                    }
                }

                for (k = 0; k <= VB.I(dt.Rows[0]["P_PRE"].ToString().Trim(), "^^") - 1; k++)
                {
                    if (VB.Pstr(dt.Rows[0]["P_PRE"].ToString().Trim(), "^^", k + 1) == "1")
                    {
                        ChkP_3[k].Checked = true;
                    }
                }

                ss욕창.ActiveSheet.Cells[3, 7].Text = dt.Rows[0]["ENTDATE"].ToString().Trim();

                ss욕창.ActiveSheet.Cells[10, 4].Text = VB.Space(3) + txtDiag.Text.Trim();


                if (opt_P20.Checked == true)
                {
                    ss욕창.ActiveSheet.Cells[11, 4].Text = "유";
                }

                else
                {
                    ss욕창.ActiveSheet.Cells[11, 4].Text = "무";
                }

                ss욕창.ActiveSheet.Cells[12, 4].Text = txtp_Jumsu.Text.Trim();


                if (Opt_P30.Checked == true)
                {
                    ss욕창.ActiveSheet.Cells[13, 4].Text = "완쾌";

                }

                else if (Opt_P31.Checked == true)
                {
                    ss욕창.ActiveSheet.Cells[13, 4].Text = "악화";

                }

                else if (Opt_P32.Checked == true)
                {
                    ss욕창.ActiveSheet.Cells[13, 4].Text = "사망";

                }

                else if (Opt_P33.Checked == true)
                {
                    ss욕창.ActiveSheet.Cells[13, 4].Text = "퇴원";

                }

                //발생부위, 단계
                strTemp = "";
                for (k = 0; k <= 4; k++)
                {
                    if (ChkP_1[k].Checked == true)
                    {
                        strTemp = strTemp + k + 1 + "." + ChkP_1[k].Text + "(√) 단계 [" + Txt_Pstep[k].Text + "], ";
                    }

                    else
                    {
                        strTemp = strTemp + k + 1 + "." + ChkP_1[k].Text + "(  ) 단계 [ ], ";
                    }
                }
                ss욕창.ActiveSheet.Cells[17, 2].Text = VB.Space(3) + strTemp;

                strTemp = "";
                for (k = 5; k <= 8; k++)
                {
                    if (ChkP_1[k].Checked == true)
                    {
                        strTemp = strTemp + k + 1 + "." + ChkP_1[k].Text + "(√) 단계 [" + Txt_Pstep[k].Text + "], ";
                    }

                    else
                    {
                        strTemp = strTemp + k + 1 + "." + ChkP_1[k].Text + "(  ) 단계 [ ], ";
                    }
                }

                ss욕창.ActiveSheet.Cells[18, 2].Text = VB.Space(3) + strTemp;

                strTemp = "";
                if (ChkP_1[9].Checked == true)
                {
                    strTemp = strTemp + k + 1 + "." + ChkP_1[k].Text + "{ " + txtPetc.Text + " }" + "(√) 단계 [" + Txt_Pstep[k].Text + "], ";
                }

                ss욕창.ActiveSheet.Cells[19, 2].Text = VB.Space(3) + strTemp;

                //요인
                strTemp = "";
                for (k = 0; k <= 5; k++)
                {
                    if (ChkP_2[k].Checked == true)
                    {
                        strTemp = strTemp + k + 1 + "." + ChkP_2[k].Text + "(√), ";
                    }

                    else
                    {
                        strTemp = strTemp + k + 1 + "." + ChkP_2[k].Text + "( ), ";
                    }
                }
                ss욕창.ActiveSheet.Cells[23, 2].Text = VB.Space(3) + strTemp;

                strTemp = "";
                for (k = 6; k <= 11; k++)
                {
                    if (ChkP_2[k].Checked == true)
                    {
                        strTemp = strTemp + k + 1 + "." + ChkP_2[k].Text + "(√), ";
                    }

                    else
                    {
                        strTemp = strTemp + k + 1 + "." + ChkP_2[k].Text + "( ), ";
                    }
                }
                ss욕창.ActiveSheet.Cells[24, 2].Text = VB.Space(3) + strTemp;

                strTemp = "";
                for (k = 12; k <= 13; k++)
                {
                    if (ChkP_2[k].Checked == true)
                    {
                        strTemp = strTemp + k + 1 + "." + ChkP_2[k].Text + "(√), ";
                    }

                    else
                    {
                        strTemp = strTemp + k + 1 + "." + ChkP_2[k].Text + "( ), ";
                    }
                }
                ss욕창.ActiveSheet.Cells[25, 2].Text = VB.Space(3) + strTemp;

                strTemp = "";
                for (k = 0; k <= 3; k++)
                {
                    if (ChkP_3[k].Checked == true)
                    {
                        strTemp = strTemp + k + 1 + "." + ChkP_3[k].Text + "(√), ";
                    }

                    else
                    {
                        strTemp = strTemp + k + 1 + "." + ChkP_3[k].Text + "( ), ";
                    }
                }
                ss욕창.ActiveSheet.Cells[29, 2].Text = VB.Space(3) + strTemp;

                strTemp = "";
                for (k = 4; k <= 8; k++)
                {
                    if (ChkP_3[k].Checked == true)
                    {
                        strTemp = strTemp + k + 1 + "." + ChkP_3[k].Text + "(√), ";
                    }

                    else
                    {
                        strTemp = strTemp + k + 1 + "." + ChkP_3[k].Text + "( ), ";
                    }
                }
                ss욕창.ActiveSheet.Cells[30, 2].Text = VB.Space(3) + strTemp;

                ss욕창.ActiveSheet.Cells[36, 9].Text = "조사자 : " + CF.Read_SabunName(clsDB.DbCon, dt.Rows[0]["EntSabun"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;
        }
    }
}
