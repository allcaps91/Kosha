using ComLibB;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using System.Collections.Generic;

/// <summary>
/// Description : 포스코검사 의뢰자 관리
/// Author : 박병규
/// Create Date : 2017.07.20
/// </summary>
/// <history>
/// 예약현황 기존 6개월기간에서 3개월로 변경 : 전혜경담당자
/// </history>
/// <seealso cref="frmPoscoResMain.frm"/> 


namespace ComPmpaLibB
{
    public partial class frmPmpaEntryExamRes : Form
    {
        clsSpread CS = null;
        ComFunc CF = null;
        ComQuery CQ = null;
        clsPmpaFunc CPF = null;
        clsPmpaQuery CPQ = null;
        clsPmpaPb CPB = null;
        clsOrdFunction OF = null;

        DataTable Dt = new DataTable();
        string SQL = string.Empty;
        string SqlErr = string.Empty;
        int intRowCnt = 0;

        string FstrRowID = string.Empty;
        string FstrGkiho = string.Empty;
        string FstrAuto = string.Empty;
        string Fvalue = string.Empty;
        
        public frmPmpaEntryExamRes()
        {
            InitializeComponent();
            setParam();
        }

        private void setParam()
        {
            this.Load += new EventHandler(eFrm_Load);

            //LostFocus 이벤트
            this.txtPtno.LostFocus += new EventHandler(LostFocusEvent);

            this.dtpEtime1.LostFocus += new EventHandler(LostFocusEvent);
            this.dtpEtime2.LostFocus += new EventHandler(LostFocusEvent);
            this.dtpEtime3.LostFocus += new EventHandler(LostFocusEvent);
            this.dtpEtime4.LostFocus += new EventHandler(LostFocusEvent);
            this.dtpEtime5.LostFocus += new EventHandler(LostFocusEvent);
            this.dtpEtime6.LostFocus += new EventHandler(LostFocusEvent);
            this.dtpEtime7.LostFocus += new EventHandler(LostFocusEvent);
            this.dtpEtime8.LostFocus += new EventHandler(LostFocusEvent);
            this.dtpEtime9.LostFocus += new EventHandler(LostFocusEvent);
            this.dtpEtime10.LostFocus += new EventHandler(LostFocusEvent);
            this.dtpEtime11.LostFocus += new EventHandler(LostFocusEvent);
            this.dtpEtime12.LostFocus += new EventHandler(LostFocusEvent);
            this.dtpEtime13.LostFocus += new EventHandler(LostFocusEvent);
            this.dtpEtime14.LostFocus += new EventHandler(LostFocusEvent);
            this.dtpEtime15.LostFocus += new EventHandler(LostFocusEvent);
            this.dtpEtime16.LostFocus += new EventHandler(LostFocusEvent);
            this.dtpEtime17.LostFocus += new EventHandler(LostFocusEvent);
            this.dtpEtime18.LostFocus += new EventHandler(LostFocusEvent);


            //KeyPress 이벤트
            this.txtPtno.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtSname.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtJumin1.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtJumin2.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.cboSex.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtTel.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtHphone.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtPostCode.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtJuso2.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtRoadDong.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.dtpJdate.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.dtpCdate.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.cboSingu.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtBuse.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtSabun.KeyPress += new KeyPressEventHandler(KeyPressEvent);

            this.dtpEdate1.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.dtpEdate2.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.dtpEdate3.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.dtpEdate4.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.dtpEdate5.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.dtpEdate6.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.dtpEdate7.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.dtpEdate8.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.dtpEdate9.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.dtpEdate10.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.dtpEdate11.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.dtpEdate12.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.dtpEdate13.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.dtpEdate14.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.dtpEdate15.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.dtpEdate16.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.dtpEdate17.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.dtpEdate18.KeyPress += new KeyPressEventHandler(KeyPressEvent);


            this.txtRemark1.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtRemark2.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtRemark3.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtRemark4.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtRemark5.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtRemark6.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtRemark7.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtRemark8.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtRemark9.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtRemark10.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtRemark11.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtRemark12.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtRemark13.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtRemark14.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtRemark15.KeyPress += new KeyPressEventHandler(KeyPressEvent);
        }

        private void LostFocusEvent(object sender, EventArgs e)
        {
            DataTable DtPat = new DataTable();

            if (sender == this.txtPtno)
            {
                txtPtno.Text = ComFunc.SetAutoZero(txtPtno.Text, 8);

                DtPat = CPF.Get_BasPatient(clsDB.DbCon, txtPtno.Text);

                if (DtPat.Rows.Count > 0)
                {
                    txtSname.Text = DtPat.Rows[0]["SNAME"].ToString().Trim();
                    txtJumin1.Text = DtPat.Rows[0]["JUMIN1"].ToString().Trim();
                    txtJumin2.Text = clsAES.DeAES(DtPat.Rows[0]["JUMIN3"].ToString().Trim());
                    txtTel.Text = DtPat.Rows[0]["TEL"].ToString().Trim();
                    txtHphone.Text = DtPat.Rows[0]["HPHONE"].ToString().Trim();

                    txtBuildNo.Text = DtPat.Rows[0]["BuildNo"].ToString().Trim();

                    if (DtPat.Rows[0]["BuildNo"].ToString().Trim() != "")
                    {
                        txtPostCode.Text = DtPat.Rows[0]["ZipCode3"].ToString().Trim();
                        txtJuso1.Text = CQ.Read_RoadJuso(clsDB.DbCon, txtBuildNo.Text.Trim());
                        txtJuso2.Text = DtPat.Rows[0]["RoadDetail"].ToString().Trim();
                        chkRoad.Checked = true;
                    }
                    else
                    {
                        txtPostCode.Text = DtPat.Rows[0]["ZipCode1"].ToString().Trim() + DtPat.Rows[0]["ZipCode2"].ToString().Trim();
                        txtJuso1.Text = CQ.Read_Juso(clsDB.DbCon, txtPostCode.Text.Trim());
                        txtJuso2.Text = DtPat.Rows[0]["Juso"].ToString().Trim();
                        chkRoad.Checked = false;

                        if (DtPat.Rows[0]["Road"].ToString().Trim() == "Y") { chkRoad.Checked = true; }
                    }

                    txtRoadDong.Text = DtPat.Rows[0]["RoadDong"].ToString().Trim();

                    switch (VB.Left(txtJumin2.Text.Trim(), 1))
                    {
                        case "1":
                        case "3":
                        case "5":
                        case "7":
                        case "0":
                            cboSex.SelectedIndex = 0;
                            break;

                        default:
                            cboSex.SelectedIndex = 1;
                            break;
                    }

                }

                DtPat.Dispose();
                DtPat = null;

                //포스코 접수가 있으면 정보르르 가져옴
                DtPat = CPF.Get_PoscoPatient(clsDB.DbCon, txtPtno.Text);

                if (DtPat.Rows.Count > 0)
                {
                    txtSabun.Text = DtPat.Rows[0]["Sabun"].ToString().Trim();
                    txtBuse.Text = DtPat.Rows[0]["Buse"].ToString().Trim();

                    if (DtPat.Rows[0]["Juso"].ToString().Trim() != "")
                        txtJuso2.Text = DtPat.Rows[0]["Juso"].ToString().Trim();
                }

                DtPat.Dispose();
                DtPat = null;
            }

            //if (sender == this.dtpEtime1) { CPB.CHECK_TIME(dtpEtime1); }
            //if (sender == this.dtpEtime2) { CPB.CHECK_TIME(dtpEtime2); }
            //if (sender == this.dtpEtime3) { CPB.CHECK_TIME(dtpEtime3); }
            //if (sender == this.dtpEtime4) { CPB.CHECK_TIME(dtpEtime4); }
            //if (sender == this.dtpEtime5) { CPB.CHECK_TIME(dtpEtime5); }
            //if (sender == this.dtpEtime6) { CPB.CHECK_TIME(dtpEtime6); }
            //if (sender == this.dtpEtime7) { CPB.CHECK_TIME(dtpEtime7); }
            //if (sender == this.dtpEtime8) { CPB.CHECK_TIME(dtpEtime8); }
            //if (sender == this.dtpEtime9) { CPB.CHECK_TIME(dtpEtime9); }
            //if (sender == this.dtpEtime10) { CPB.CHECK_TIME(dtpEtime10); }
            //if (sender == this.dtpEtime11) { CPB.CHECK_TIME(dtpEtime11); }
            //if (sender == this.dtpEtime12) { CPB.CHECK_TIME(dtpEtime12); }
            //if (sender == this.dtpEtime13) { CPB.CHECK_TIME(dtpEtime13); }
            //if (sender == this.dtpEtime14) { CPB.CHECK_TIME(dtpEtime14); }

        }

        private void KeyPressEvent(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtPtno && e.KeyChar == (Char)13)
            {
                txtPtno.Text = ComFunc.SetAutoZero(txtPtno.Text, 8);
                txtSname.Focus();
            }

            if (sender == this.txtSname && e.KeyChar == (Char)13) { txtJumin1.Focus(); }
            if (sender == this.txtJumin1 && e.KeyChar == (Char)13) { txtJumin2.Focus(); }
            if (sender == this.txtJumin2 && e.KeyChar == (Char)13) { cboSex.Focus(); }
            if (sender == this.cboSex && e.KeyChar == (Char)13) { txtTel.Focus(); }
            if (sender == this.txtTel && e.KeyChar == (Char)13) { txtHphone.Focus(); }
            if (sender == this.txtHphone && e.KeyChar == (Char)13) { txtPostCode.Focus(); }
            if (sender == this.txtPostCode && e.KeyChar == (Char)13) { txtJuso2.Focus(); }
            if (sender == this.txtJuso2 && e.KeyChar == (Char)13) { txtRoadDong.Focus(); }
            if (sender == this.txtRoadDong && e.KeyChar == (Char)13) { dtpJdate.Focus(); }
            if (sender == this.dtpJdate && e.KeyChar == (Char)13) { dtpCdate.Focus(); }
            if (sender == this.dtpCdate && e.KeyChar == (Char)13) { cboSingu.Focus(); }
            if (sender == this.cboSingu && e.KeyChar == (Char)13) { txtBuse.Focus(); }
            if (sender == this.txtBuse && e.KeyChar == (Char)13) { txtSabun.Focus(); }
            if (sender == this.txtSabun && e.KeyChar == (Char)13) { txtJobName.Focus(); }

            if (sender == this.dtpEdate1 && e.KeyChar == (Char)13) { txtRemark1.Focus(); }
            if (sender == this.dtpEdate2 && e.KeyChar == (Char)13) { txtRemark2.Focus(); }
            if (sender == this.dtpEdate3 && e.KeyChar == (Char)13) { txtRemark3.Focus(); }
            if (sender == this.dtpEdate4 && e.KeyChar == (Char)13) { txtRemark4.Focus(); }
            if (sender == this.dtpEdate5 && e.KeyChar == (Char)13) { txtRemark5.Focus(); }
            if (sender == this.dtpEdate6 && e.KeyChar == (Char)13) { txtRemark6.Focus(); }
            if (sender == this.dtpEdate7 && e.KeyChar == (Char)13) { txtRemark7.Focus(); }
            if (sender == this.dtpEdate8 && e.KeyChar == (Char)13) { txtRemark8.Focus(); }
            if (sender == this.dtpEdate9 && e.KeyChar == (Char)13) { txtRemark9.Focus(); }
            if (sender == this.dtpEdate10 && e.KeyChar == (Char)13) { txtRemark10.Focus(); }
            if (sender == this.dtpEdate11 && e.KeyChar == (Char)13) { txtRemark11.Focus(); }
            if (sender == this.dtpEdate12 && e.KeyChar == (Char)13) { txtRemark12.Focus(); }
            if (sender == this.dtpEdate13 && e.KeyChar == (Char)13) { txtRemark13.Focus(); }
            if (sender == this.dtpEdate14 && e.KeyChar == (Char)13) { txtRemark14.Focus(); }
            if (sender == this.dtpEdate15 && e.KeyChar == (Char)13) { txtRemark15.Focus(); }
            if (sender == this.dtpEdate16 && e.KeyChar == (Char)13) { txtRemark16.Focus(); }
            if (sender == this.dtpEdate17 && e.KeyChar == (Char)13) { txtRemark17.Focus(); }
            if (sender == this.dtpEdate18 && e.KeyChar == (Char)13) { txtRemark18.Focus(); }
          

            if (sender == this.txtRemark1 && e.KeyChar == (Char)13) { txtMsg1.Focus(); }
            if (sender == this.txtRemark2 && e.KeyChar == (Char)13) { txtMsg2.Focus(); }
            if (sender == this.txtRemark3 && e.KeyChar == (Char)13) { txtMsg3.Focus(); }
            if (sender == this.txtRemark4 && e.KeyChar == (Char)13) { txtMsg4.Focus(); }
            if (sender == this.txtRemark5 && e.KeyChar == (Char)13) { txtMsg5.Focus(); }
            if (sender == this.txtRemark6 && e.KeyChar == (Char)13) { txtMsg6.Focus(); }
            if (sender == this.txtRemark7 && e.KeyChar == (Char)13) { txtMsg7.Focus(); }
            if (sender == this.txtRemark8 && e.KeyChar == (Char)13) { txtMsg8.Focus(); }
            if (sender == this.txtRemark9 && e.KeyChar == (Char)13) { txtMsg9.Focus(); }
            if (sender == this.txtRemark10 && e.KeyChar == (Char)13) { txtMsg10.Focus(); }
            if (sender == this.txtRemark11 && e.KeyChar == (Char)13) { txtMsg11.Focus(); }
            if (sender == this.txtRemark12 && e.KeyChar == (Char)13) { txtMsg12.Focus(); }
            if (sender == this.txtRemark13 && e.KeyChar == (Char)13) { txtMsg13.Focus(); }
            if (sender == this.txtRemark14 && e.KeyChar == (Char)13) { txtMsg14.Focus(); }
            if (sender == this.txtRemark15 && e.KeyChar == (Char)13) { txtMsg15.Focus(); }
            if (sender == this.txtRemark16 && e.KeyChar == (Char)13) { txtMsg16.Focus(); }
            if (sender == this.txtRemark17 && e.KeyChar == (Char)13) { txtMsg17.Focus(); }
            if (sender == this.txtRemark18 && e.KeyChar == (Char)13) { txtMsg18.Focus(); }



        }

        private void eFrm_Load(object sender, EventArgs e)
        {
            CS = new clsSpread();
            CF = new ComFunc();
            CQ = new ComQuery();
            CPF = new clsPmpaFunc();
            CPQ = new clsPmpaQuery();
            CPB = new clsPmpaPb();
            OF = new clsOrdFunction();

            ComboBox[] cboDept;

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            dtpFDate.Text = clsPublic.GstrSysDate;
            dtpTDate.Text = clsPublic.GstrSysDate;

            CS.Spread_Clear_Range(ssList, 0, 1, ssList_Sheet1.RowCount, ssList_Sheet1.ColumnCount);
            CS.Spread_All_Clear(ssSub);

            txtBigo.Text = "";

            eFrm_Clear();

            cboSex.Items.Clear();
            cboSex.Items.Add("M.남자");
            cboSex.Items.Add("F.여자");
            cboSex.SelectedIndex = -1;

            cboSingu.Items.Clear();
            cboSingu.Items.Add("1.신환");
            cboSingu.Items.Add("2.구환");
            cboSingu.SelectedIndex = -1;

            cboDept = new ComboBox[] { cboDept1, cboDept2, cboDept3, cboDept4, cboDept5, cboDept6, cboDept7, cboDept8, cboDept9, cboDept10, cboDept11, cboDept12, cboDept13, cboDept14, cboDept15, cboDept16, cboDept17, cboDept18 };

            for (int i = 0; i < 18; i++)
            {
                cboDept[i].Items.Clear();
                cboDept[i].Items.Add(" ");
            }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT DEPTCODE, DEPTNAMEK ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
            SQL += ComNum.VBLF + "  ORDER BY PRINTRANKING ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                for (int j = 0; j < 18; j++)
                    cboDept[j].Items.Add(Dt.Rows[i]["DEPTCODE"].ToString().Trim());
            }

            for (int i = 0; i < 18; i++)
                cboDept[i].SelectedIndex = 0;

            Dt.Dispose();
            Dt = null;

            //예약현황
            Get_ResBoard();
            //포스코예약 공지사항 가져오기
            txtBigo.Text = CPF.READ_POSCO_MSG(clsDB.DbCon, "전체", "01");

            FstrRowID = "";

        }

        private void eFrm_Clear()
        {
            TextBox[] dtpEdate;

            //기본사항
            ComFunc.SetAllControlClear(grbBasic);

            cboSex.SelectedIndex = -1;
            cboSingu.SelectedIndex = -1;

            //검사예약일자
            ComFunc.SetAllControlClear(grbRes);

            dtpEdate = new TextBox[] { dtpEdate1, dtpEdate2, dtpEdate3, dtpEdate4, dtpEdate5, dtpEdate6, dtpEdate7, dtpEdate8, dtpEdate9, dtpEdate10, dtpEdate11, dtpEdate12, dtpEdate13, dtpEdate14, dtpEdate15, dtpEdate16, dtpEdate17, dtpEdate18 };

            //for (int i = 0; i < 14; i++)
            //{
            //    dtpEdate[i].Checked = false;
            //}

            //dtpResult.Checked = false;

            rdoBio0.Checked = false;
            rdoBio1.Checked = false;
            rdoBio2.Checked = false;

            btnNew.Enabled = true;
            btnSave.Enabled = false;
            btnDelete.Enabled = false;

            FstrRowID = "";
            FstrGkiho = "";
            FstrAuto = "";

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Get_ResList();
        }

        private void Get_ResBoard()
        {
            string strPtno = string.Empty;
            string strFdate = string.Empty;
            string strTdate = string.Empty;
            string strExDate = string.Empty;
            int nRow = 0;
            int nCol = 0;
            string strOK = "(N)";

            //string[,] strData = new string[19, 90];
            string[,] strData = new string[19, 210];

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) { return; }     //권한확인

            //strFdate = Convert.ToDateTime(VB.DateAdd("d", -5, dtpFDate.Text).ToString("yyyy-MM-dd")).ToString();
            //strTdate = Convert.ToDateTime(VB.DateAdd("d", 85, dtpFDate.Text).ToString("yyyy-MM-dd")).ToString();

            strFdate = dtpFDate.Value.AddDays(-4).ToShortDateString();
            //strTdate = dtpFDate.Value.AddDays(85).ToShortDateString();
            strTdate = dtpFDate.Value.AddDays(205).ToShortDateString();

            for (int i = 0; i < 19; i++)
            {
                //for (int j = 0; j < 90; j++)
                for (int j = 0; j < 210; j++)
                    strData[i, j] = "";
            }

            //날짜SET
            //for (int i = 0; i < 90; i++)
            for (int i = 0; i < 210; i++)
                strData[0, i] = VB.DateAdd("d", i, strFdate).ToString("yyyy-MM-dd");

            Cursor.Current = Cursors.WaitCursor;
            CS.Spread_Clear_Range(ssList, 0, 1, ssList_Sheet1.RowCount, ssList_Sheet1.ColumnCount);
            CS.setSpdCellColor(ssList, 0, 1, ssList_Sheet1.RowCount - 1, ssList_Sheet1.ColumnCount - 1, Color.White);
            CS.Spread_All_Clear(ssSub);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT '1' GBN , PANO, SNAME,  ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES1,'YYYY-MM-DD HH24:MI') EXDATE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1";
            SQL += ComNum.VBLF + "    AND EXAMRES1 >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND EXAMRES1  < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GUBUN ='01' ";
            SQL += ComNum.VBLF + "  UNION ALL ";
            SQL += ComNum.VBLF + " SELECT '2' GBN , PANO, SNAME,  ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES8,'YYYY-MM-DD HH24:MI') EXDATE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1";
            SQL += ComNum.VBLF + "    AND EXAMRES8 >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND EXAMRES8  < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GUBUN ='01' ";
            SQL += ComNum.VBLF + "  UNION ALL ";
            SQL += ComNum.VBLF + " SELECT '3' GBN , PANO, SNAME,  ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES2,'YYYY-MM-DD HH24:MI') EXDATE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1";
            SQL += ComNum.VBLF + "    AND EXAMRES2 >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND EXAMRES2  < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GUBUN ='01' ";
            SQL += ComNum.VBLF + "  UNION ALL ";
            SQL += ComNum.VBLF + " SELECT '4' GBN , PANO, SNAME,  ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES3,'YYYY-MM-DD HH24:MI') EXDATE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1";
            SQL += ComNum.VBLF + "    AND EXAMRES3 >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND EXAMRES3  < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GUBUN ='01' ";
            SQL += ComNum.VBLF + "  UNION ALL ";
            SQL += ComNum.VBLF + " SELECT '5' GBN , PANO, SNAME,  ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES6,'YYYY-MM-DD HH24:MI') EXDATE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1";
            SQL += ComNum.VBLF + "    AND EXAMRES6 >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND EXAMRES6  < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GUBUN ='01' ";
            SQL += ComNum.VBLF + "  UNION ALL ";
            SQL += ComNum.VBLF + " SELECT '6' GBN , PANO, SNAME,  ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES7,'YYYY-MM-DD HH24:MI') EXDATE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1";
            SQL += ComNum.VBLF + "    AND EXAMRES7 >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND EXAMRES7  < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GUBUN ='01' ";
            SQL += ComNum.VBLF + "  UNION ALL ";
            SQL += ComNum.VBLF + " SELECT '7' GBN , PANO, SNAME,  ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES9,'YYYY-MM-DD HH24:MI') EXDATE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1";
            SQL += ComNum.VBLF + "    AND EXAMRES9 >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND EXAMRES9  < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GUBUN ='01' ";
            SQL += ComNum.VBLF + "  UNION ALL ";
            SQL += ComNum.VBLF + " SELECT '8' GBN , PANO, SNAME,  ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES10,'YYYY-MM-DD HH24:MI') EXDATE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1";
            SQL += ComNum.VBLF + "    AND EXAMRES10 >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND EXAMRES10  < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GUBUN ='01' ";
            SQL += ComNum.VBLF + "  UNION ALL ";
            SQL += ComNum.VBLF + " SELECT '9' GBN , PANO, SNAME,  ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES11,'YYYY-MM-DD HH24:MI') EXDATE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1";
            SQL += ComNum.VBLF + "    AND EXAMRES11 >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND EXAMRES11  < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GUBUN ='01' ";
            SQL += ComNum.VBLF + "  UNION ALL ";
            SQL += ComNum.VBLF + " SELECT '10' GBN , PANO, SNAME,  ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES12,'YYYY-MM-DD HH24:MI') EXDATE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1";
            SQL += ComNum.VBLF + "    AND EXAMRES12 >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND EXAMRES12  < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GUBUN ='01' ";
            SQL += ComNum.VBLF + "  UNION ALL ";
            SQL += ComNum.VBLF + " SELECT '11' GBN , PANO, SNAME,  ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES13,'YYYY-MM-DD HH24:MI') EXDATE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1";
            SQL += ComNum.VBLF + "    AND EXAMRES13 >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND EXAMRES13  < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GUBUN ='01' ";
            SQL += ComNum.VBLF + "  UNION ALL ";
            SQL += ComNum.VBLF + " SELECT '12' GBN , PANO, SNAME,  ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES14,'YYYY-MM-DD HH24:MI') EXDATE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1";
            SQL += ComNum.VBLF + "    AND EXAMRES14 >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND EXAMRES14  < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GUBUN ='01' ";
            SQL += ComNum.VBLF + "  UNION ALL ";
            SQL += ComNum.VBLF + " SELECT '13' GBN , PANO, SNAME,  ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES15,'YYYY-MM-DD HH24:MI') EXDATE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1";
            SQL += ComNum.VBLF + "    AND EXAMRES15 >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND EXAMRES15  < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GUBUN ='01' ";
            SQL += ComNum.VBLF + "  UNION ALL ";
            SQL += ComNum.VBLF + " SELECT '14' GBN , PANO, SNAME,  ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES16,'YYYY-MM-DD HH24:MI') EXDATE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1";
            SQL += ComNum.VBLF + "    AND EXAMRES16 >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND EXAMRES16  < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GUBUN ='01' ";
            SQL += ComNum.VBLF + "  UNION ALL ";
            SQL += ComNum.VBLF + " SELECT '15' GBN , PANO, SNAME,  ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES17,'YYYY-MM-DD HH24:MI') EXDATE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1";
            SQL += ComNum.VBLF + "    AND EXAMRES17 >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND EXAMRES17  < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GUBUN ='01' ";
            SQL += ComNum.VBLF + "  UNION ALL ";
            SQL += ComNum.VBLF + " SELECT '16' GBN , PANO, SNAME,  ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES18,'YYYY-MM-DD HH24:MI') EXDATE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1";
            SQL += ComNum.VBLF + "    AND EXAMRES18 >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND EXAMRES18  < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GUBUN ='01' ";
            SQL += ComNum.VBLF + "  UNION ALL ";
            SQL += ComNum.VBLF + " SELECT '17' GBN , PANO, SNAME,  ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES19,'YYYY-MM-DD HH24:MI') EXDATE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1";
            SQL += ComNum.VBLF + "    AND EXAMRES19 >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND EXAMRES19  < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GUBUN ='01' ";
            SQL += ComNum.VBLF + "  UNION ALL ";
            SQL += ComNum.VBLF + " SELECT '18' GBN , PANO, SNAME,  ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES20,'YYYY-MM-DD HH24:MI') EXDATE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1";
            SQL += ComNum.VBLF + "    AND EXAMRES20 >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND EXAMRES20  < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GUBUN ='01' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            //ssList_Sheet1.RowCount = Dt.Rows.Count;
            //ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                strPtno = string.Format("{0:00000000}", Dt.Rows[i]["PANO"].ToString().Trim());
                strExDate = Dt.Rows[i]["EXDATE"].ToString().Trim();
                nRow = Convert.ToInt32(Dt.Rows[i]["GBN"].ToString().Trim());
                nCol = Convert.ToInt32(CF.DATE_ILSU(clsDB.DbCon, VB.Left(strExDate, 10), strFdate));

                strOK = CPF.RETURN_EXAM_END(clsDB.DbCon, strPtno, VB.Left(strExDate, 10), nRow);

                if (strData[nRow, nCol] == "")
                    strData[nRow, nCol] += strOK + Dt.Rows[i]["SNAME"].ToString().Trim() + VB.Mid(strExDate, 12, 5);
                else
                    strData[nRow, nCol] += ComNum.VBLF + strOK + Dt.Rows[i]["SNAME"].ToString().Trim() + VB.Mid(strExDate, 12, 5);
            }

            Dt.Dispose();
            Dt = null;

            //for (int i = 0; i < 90; i++)
            for (int i = 0; i < 210; i++)
            {
                switch (clsVbfunc.GetYoIl(strData[0, i]))
                {
                    case "토요일":
                        CS.setSpdCellColor(ssList, 0, i + 1, ssList_Sheet1.RowCount - 1, i + 1, Color.FromArgb(193, 193, 255));
                        ssList_Sheet1.Columns[i + 1].Width = 100;
                        break;

                    case "일요일":
                        CS.setSpdCellColor(ssList, 0, i + 1, ssList_Sheet1.RowCount - 1, i + 1, Color.FromArgb(255, 217, 217));
                        ssList_Sheet1.Columns[i + 1].Width = 1;
                        break;


                    default:
                        ssList_Sheet1.Columns[i + 1].Width = 100;
                        break;
                }

                if (CPF.CHECK_HOLYDAY(clsDB.DbCon, strData[0, i]) == "OK")
                {
                    CS.setSpdCellColor(ssList, 0, i + 1, ssList_Sheet1.RowCount - 1, i + 1, Color.FromArgb(255, 217, 217));
                    ssList_Sheet1.Columns[i + 1].Width = 1;
                }

                ssList_Sheet1.Cells[0, i + 1].Text = strData[0, i];     //날짜
                ssList_Sheet1.Cells[1, i + 1].Text = strData[1, i];     //복부초음파
                ssList_Sheet1.Cells[2, i + 1].Text = strData[2, i];     //여성자궁검사
                ssList_Sheet1.Cells[3, i + 1].Text = strData[3, i];     //위 내시경
                ssList_Sheet1.Cells[4, i + 1].Text = strData[4, i];     //위 내시경(수면)
                ssList_Sheet1.Cells[5, i + 1].Text = strData[5, i];     //대장 내시경(수면)
                ssList_Sheet1.Cells[6, i + 1].Text = strData[6, i];     //흉부CT
                ssList_Sheet1.Cells[7, i + 1].Text = strData[7, i];     //뇌CT
                ssList_Sheet1.Cells[8, i + 1].Text = strData[8, i];     //경추CT
                ssList_Sheet1.Cells[9, i + 1].Text = strData[9, i];     //요추CT
                ssList_Sheet1.Cells[10, i + 1].Text = strData[10, i];   //심장CT
                ssList_Sheet1.Cells[11, i + 1].Text = strData[11, i];   //심장 초음파
                ssList_Sheet1.Cells[12, i + 1].Text = strData[12, i];   //경동맥 초음파
                ssList_Sheet1.Cells[13, i + 1].Text = strData[13, i];   //뇌혈류 초음파
                ssList_Sheet1.Cells[14, i + 1].Text = strData[14, i];   //여성유방검진
                ssList_Sheet1.Cells[15, i + 1].Text = strData[15, i];   //무릎 MRI
                ssList_Sheet1.Cells[16, i + 1].Text = strData[16, i];   //뇌 MRI
                ssList_Sheet1.Cells[17, i + 1].Text = strData[17, i];   //경추 MRI
                ssList_Sheet1.Cells[18, i + 1].Text = strData[18, i];   //요추 MRI
            }

            Cursor.Current = Cursors.Default;
        }

        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true) return;

            rdoDate1.Checked = true;
            dtpFDate.Text = ssList_Sheet1.Cells[0, e.Column].Text;
            dtpTDate.Text = ssList_Sheet1.Cells[0, e.Column].Text;

            Get_ResList();
        }

        private void Get_ResList()
        {
            DataTable DtPf = new DataTable();
            DataTable Dt1 = new DataTable();
            DataTable Dt2 = new DataTable();

            string strPtno = string.Empty;
            string strFdate = string.Empty;
            string strTdate = string.Empty;
            string strRowID = string.Empty;
            int[] nCnt = new int[2];



            strFdate = dtpFDate.Text;
            strTdate = dtpTDate.Text;

            eFrm_Clear();

            Cursor.Current = Cursors.WaitCursor;
            CS.Spread_All_Clear(ssSub);

            SQL = "";
            if (chkName.Checked == true)
            {
                SQL += ComNum.VBLF + "SELECT * FROM ( ";
            }
            SQL += ComNum.VBLF + " SELECT TO_CHAR(JDATE,'YYYY-MM-DD') JDATE, SNAME, JUMIN1, ";
            SQL += ComNum.VBLF + "        JUMIN3 ,Gubun, ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND GUBUN  ='01' ";

            if (rdoDate0.Checked == true)
            {
                SQL += ComNum.VBLF + "   AND JDATE  >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND JDATE  <= TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
            }
            else
            {
                SQL += ComNum.VBLF + "    AND (EXAMRES1    >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND EXAMRES1      < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) ";
                SQL += ComNum.VBLF + "     OR (EXAMRES2    >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND EXAMRES2      < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) ";
                SQL += ComNum.VBLF + "     OR (EXAMRES3    >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND EXAMRES3      < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) ";
                SQL += ComNum.VBLF + "     OR (EXAMRES4    >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND EXAMRES4      < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) ";
                SQL += ComNum.VBLF + "     OR (EXAMRES6    >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND EXAMRES6      < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) ";
                SQL += ComNum.VBLF + "     OR (EXAMRES7    >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND EXAMRES7      < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) ";
                SQL += ComNum.VBLF + "     OR (EXAMRES8    >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND EXAMRES8      < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) ";
                SQL += ComNum.VBLF + "     OR (EXAMRES9    >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND EXAMRES9      < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) ";
                SQL += ComNum.VBLF + "     OR (EXAMRES10   >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND EXAMRES10     < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) ";
                SQL += ComNum.VBLF + "     OR (EXAMRES11   >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND EXAMRES11     < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) ";
                SQL += ComNum.VBLF + "     OR (EXAMRES12   >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND EXAMRES12     < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) ";
                SQL += ComNum.VBLF + "     OR (EXAMRES13   >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND EXAMRES13     < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) ";
                SQL += ComNum.VBLF + "     OR (EXAMRES14   >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND EXAMRES14     < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) ";
                SQL += ComNum.VBLF + "     OR (EXAMRES15   >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND EXAMRES15     < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) ";
                SQL += ComNum.VBLF + "     OR (EXAMRES16   >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND EXAMRES16     < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) ";
                SQL += ComNum.VBLF + "     OR (EXAMRES17   >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND EXAMRES17     < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) ";
                SQL += ComNum.VBLF + "     OR (EXAMRES18   >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND EXAMRES18     < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) ";
                SQL += ComNum.VBLF + "     OR (EXAMRES19   >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND EXAMRES19    < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) ";
                SQL += ComNum.VBLF + "     OR (EXAMRES20   >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND EXAMRES20    < TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) ";

            }

            if (rdoGb1.Checked == true) { SQL += ComNum.VBLF + " AND STOPFLAG = 'Y' "; }
            if (rdoGb2.Checked == true) { SQL += ComNum.VBLF + " AND STOPFLAG IS NULL "; }


            if (chkName.Checked == true)
            {
                SQL += ComNum.VBLF + ") ";
                SQL += ComNum.VBLF + "WHERE SNAME = '" + txtSearchName.Text + "'";
            }
            SQL += ComNum.VBLF + " ORDER BY JDATE, SNAME ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssSub_Sheet1.RowCount = Dt.Rows.Count;
            ssSub_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                ssSub_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["JDATE"].ToString().Trim();
                ssSub_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["SNAME"].ToString().Trim();
                ssSub_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["JUMIN1"].ToString().Trim() + "-" + clsAES.DeAES(Dt.Rows[i]["JUMIN3"].ToString().Trim());
                ssSub_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
                strRowID = Dt.Rows[i]["ROWID"].ToString().Trim();

                DtPf = CPF.Get_Jumin_BasPatient(clsDB.DbCon, Dt.Rows[i]["JUMIN1"].ToString().Trim(), Dt.Rows[i]["JUMIN3"].ToString().Trim());
                strPtno = DtPf.Rows[0]["PANO"].ToString().Trim();
                DtPf.Dispose();
                DtPf = null;

                nCnt[0] = 0;
                nCnt[1] = 0;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(EXAMRES1,'YYYY-MM-DD') EXAMRES1 ";
                SQL += ComNum.VBLF + "       ,TO_CHAR(EXAMRES2,'YYYY-MM-DD') EXAMRES2 ";
                SQL += ComNum.VBLF + "       ,TO_CHAR(EXAMRES3,'YYYY-MM-DD') EXAMRES3 ";
                SQL += ComNum.VBLF + "       ,TO_CHAR(EXAMRES4,'YYYY-MM-DD') EXAMRES4 ";
                SQL += ComNum.VBLF + "       ,TO_CHAR(EXAMRES6,'YYYY-MM-DD') EXAMRES6 ";
                SQL += ComNum.VBLF + "       ,TO_CHAR(EXAMRES7,'YYYY-MM-DD') EXAMRES7 ";
                SQL += ComNum.VBLF + "       ,TO_CHAR(EXAMRES8,'YYYY-MM-DD') EXAMRES8 ";
                SQL += ComNum.VBLF + "       ,TO_CHAR(EXAMRES16,'YYYY-MM-DD') EXAMRES16 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
                SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                SQL += ComNum.VBLF + "    AND ROWID = '" + strRowID + "' ";
                SQL += ComNum.VBLF + "    AND GUBUN = '01' ";
                SqlErr = clsDB.GetDataTable(ref Dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (Dt1.Rows.Count > 0)
                {
                    if (string.Compare(Dt1.Rows[0]["EXAMRES1"].ToString().Trim(), strFdate) >= 0 &&
                        string.Compare(Dt1.Rows[0]["EXAMRES1"].ToString().Trim(), strTdate) <= 0)
                    {
                        nCnt[0] += 1; //초음파

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT PANO ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL ";
                        SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                        SQL += ComNum.VBLF + "    AND PANO      = '" + strPtno + "' ";
                        SQL += ComNum.VBLF + "    AND XJONG     = '3' ";
                        SQL += ComNum.VBLF + "    AND GBEND     = '1' ";
                        SQL += ComNum.VBLF + "    AND SEEKDATE  >= TO_DATE('" + strFdate + "' ,'YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND SEEKDATE  <  TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "' ,'YYYY-MM-DD') ";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, clsDB.DbCon);

                        if (Dt2.Rows.Count > 0) { nCnt[1] += 1; }

                        Dt2.Dispose();
                        Dt2 = null;
                    }

                    if (string.Compare(Dt1.Rows[0]["EXAMRES2"].ToString().Trim(), strFdate) >= 0 &&
                        string.Compare(Dt1.Rows[0]["EXAMRES2"].ToString().Trim(), strTdate) <= 0)
                    {
                        nCnt[0] += 1; //위내시경

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT COUNT(PTNO) CNT ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "ENDO_JUPMST ";
                        SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                        SQL += ComNum.VBLF + "    AND PTNO      = '" + strPtno + "' ";
                        SQL += ComNum.VBLF + "    AND RDATE     >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND RDATE     <  TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND GBSUNAP   <> '*' ";
                        SQL += ComNum.VBLF + "    AND GBJOB     = '2' ";
                        SQL += ComNum.VBLF + "    AND RESULTDATE IS NOT NULL";
                        SQL += ComNum.VBLF + "    AND OrderCode IN ('00440110', '00440120') ";
                        SQL += ComNum.VBLF + "  GROUP BY Ptno ";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, clsDB.DbCon);

                        if (Dt2.Rows.Count > 0) { nCnt[1] += Convert.ToInt32(Dt2.Rows[0]["CNT"].ToString().Trim()); }

                        Dt2.Dispose();
                        Dt2 = null;
                    }

                    if (string.Compare(Dt1.Rows[0]["EXAMRES8"].ToString().Trim(), strFdate) >= 0 &&
                        string.Compare(Dt1.Rows[0]["EXAMRES8"].ToString().Trim(), strTdate) <= 0)
                    {
                        nCnt[0] += 1; //위장조영

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT COUNT(PTNO) CNT ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "ENDO_JUPMST ";
                        SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                        SQL += ComNum.VBLF + "    AND RDATE     >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND RDATE     <  TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND PTNO      = '" + strPtno + "' ";
                        SQL += ComNum.VBLF + "    AND GBSUNAP   <> '*' ";
                        SQL += ComNum.VBLF + "    AND GBJOB     = '3' ";
                        SQL += ComNum.VBLF + "    AND RESULTDATE IS NOT NULL ";
                        SQL += ComNum.VBLF + "    AND OrderCode = '00440913' ";
                        SQL += ComNum.VBLF + "  GROUP BY Ptno ";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, clsDB.DbCon);

                        if (Dt2.Rows.Count > 0) { nCnt[1] += Convert.ToInt32(Dt2.Rows[0]["CNT"].ToString().Trim()); }

                        Dt2.Dispose();
                        Dt2 = null;

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT COUNT(PANO) CNT ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL ";
                        SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                        SQL += ComNum.VBLF + "    AND PANO      = '" + strPtno + "' ";
                        SQL += ComNum.VBLF + "    AND XJONG     = '2' ";
                        SQL += ComNum.VBLF + "    AND XCODE     = 'HA010' ";
                        SQL += ComNum.VBLF + "    AND SEEKDATE  >= TO_DATE('" + strFdate + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "    AND SEEKDATE  <  TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "    AND SENDDATE IS NOT NULL";
                        SQL += ComNum.VBLF + "  GROUP BY Pano ";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, clsDB.DbCon);

                        if (Dt2.Rows.Count > 0) { nCnt[1] += Convert.ToInt32(Dt2.Rows[0]["CNT"].ToString().Trim()); }

                        Dt2.Dispose();
                        Dt2 = null;
                    }

                    if (string.Compare(Dt1.Rows[0]["EXAMRES3"].ToString().Trim(), strFdate) >= 0 &&
                        string.Compare(Dt1.Rows[0]["EXAMRES3"].ToString().Trim(), strTdate) <= 0)
                    {
                        nCnt[0] += 1; //수면위내시경

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT COUNT(PTNO) CNT ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "ENDO_JUPMST ";
                        SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                        SQL += ComNum.VBLF + "    AND RDATE     >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND RDATE     <  TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND PTNO      = '" + strPtno + "' ";
                        SQL += ComNum.VBLF + "    AND GBSUNAP   <> '*' ";
                        SQL += ComNum.VBLF + "    AND GBJOB     = '2' ";
                        SQL += ComNum.VBLF + "    AND RESULTDATE IS NOT NULL";
                        SQL += ComNum.VBLF + "    AND OrderCode = '00440120' ";
                        SQL += ComNum.VBLF + "  GROUP BY Ptno ";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, clsDB.DbCon);

                        if (Dt2.Rows.Count > 0) { nCnt[1] += Convert.ToInt32(Dt2.Rows[0]["CNT"].ToString().Trim()); }

                        Dt2.Dispose();
                        Dt2 = null;
                    }

                    if (string.Compare(Dt1.Rows[0]["EXAMRES4"].ToString().Trim(), strFdate) >= 0 &&
                        string.Compare(Dt1.Rows[0]["EXAMRES4"].ToString().Trim(), strTdate) <= 0)
                    {
                        nCnt[0] += 1; //대장경

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT COUNT(PTNO) CNT ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "ENDO_JUPMST ";
                        SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                        SQL += ComNum.VBLF + "    AND RDATE     >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND RDATE     <  TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND PTNO      = '" + strPtno + "' ";
                        SQL += ComNum.VBLF + "    AND GBSUNAP   <> '*' ";
                        SQL += ComNum.VBLF + "    AND GBJOB     = '3' ";
                        SQL += ComNum.VBLF + "    AND RESULTDATE IS NOT NULL";
                        SQL += ComNum.VBLF + "    AND OrderCode IN ( '00440180','00440165','00440160' ) ";
                        SQL += ComNum.VBLF + "  GROUP BY Ptno ";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, clsDB.DbCon);

                        if (Dt2.Rows.Count > 0) { nCnt[1] += Convert.ToInt32(Dt2.Rows[0]["CNT"].ToString().Trim()); }

                        Dt2.Dispose();
                        Dt2 = null;
                    }

                    if (string.Compare(Dt1.Rows[0]["EXAMRES6"].ToString().Trim(), strFdate) >= 0 &&
                        string.Compare(Dt1.Rows[0]["EXAMRES6"].ToString().Trim(), strTdate) <= 0)
                    {
                        nCnt[0] += 1; //수면대장경

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT COUNT(PTNO) CNT ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "ENDO_JUPMST ";
                        SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                        SQL += ComNum.VBLF + "    AND RDATE     >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND RDATE     <  TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND PTNO      = '" + strPtno + "' ";
                        SQL += ComNum.VBLF + "    AND GBSUNAP   <> '*' ";
                        SQL += ComNum.VBLF + "    AND GBJOB     = '3' ";
                        SQL += ComNum.VBLF + "    AND RESULTDATE IS NOT NULL";
                        SQL += ComNum.VBLF + "    AND OrderCode IN ( 'OO440916','00440913','00440160') ";
                        SQL += ComNum.VBLF + "  GROUP BY Ptno ";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, clsDB.DbCon);

                        if (Dt2.Rows.Count > 0) { nCnt[1] += Convert.ToInt32(Dt2.Rows[0]["CNT"].ToString().Trim()); }

                        Dt2.Dispose();
                        Dt2 = null;
                    }

                    if (string.Compare(Dt1.Rows[0]["EXAMRES7"].ToString().Trim(), strFdate) >= 0 &&
                        string.Compare(Dt1.Rows[0]["EXAMRES7"].ToString().Trim(), strTdate) <= 0)
                    {
                        nCnt[0] += 1; //CT

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT PANO ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL ";
                        SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                        SQL += ComNum.VBLF + "    AND PANO      = '" + strPtno + "' ";
                        SQL += ComNum.VBLF + "    AND SEEKDATE  >= TO_DATE('" + strFdate + "' ,'YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND SEEKDATE  <  TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "' ,'YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND XJONG     = '4' ";
                        SQL += ComNum.VBLF + "    AND GBEND     = '1' ";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, clsDB.DbCon);

                        if (Dt2.Rows.Count > 0) { nCnt[1] += 1; }

                        Dt2.Dispose();
                        Dt2 = null;
                    }

                    if (string.Compare(Dt1.Rows[0]["EXAMRES16"].ToString().Trim(), strFdate) >= 0 &&
                        string.Compare(Dt1.Rows[0]["EXAMRES16"].ToString().Trim(), strTdate) <= 0)
                    {
                        nCnt[0] += 1; //유방초음파

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT PANO ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL ";
                        SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                        SQL += ComNum.VBLF + "    AND PANO      = '" + strPtno + "' ";
                        SQL += ComNum.VBLF + "    AND SEEKDATE  >= TO_DATE('" + strFdate + "' ,'YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND SEEKDATE  <  TO_DATE('" + VB.DateAdd("D", 1, strTdate).ToString("yyyy-MM-dd") + "' ,'YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND XJONG     = '2' ";
                        SQL += ComNum.VBLF + "    AND GBEND     = '1' ";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, clsDB.DbCon);

                        if (Dt2.Rows.Count > 0) { nCnt[1] += 1; }

                        Dt2.Dispose();
                        Dt2 = null;
                    }
                }

                Dt1.Dispose();
                Dt1 = null;

                ssSub_Sheet1.Cells[i, 3].Text = nCnt[1] + "/" + nCnt[0];

                if (nCnt[0] == nCnt[1])
                    CS.setSpdCellColor(ssSub, i, 0, i, ssSub_Sheet1.ColumnCount - 1, Color.FromArgb(128, 255, 128));
                else
                    CS.setSpdCellColor(ssSub, i, 0, i, ssSub_Sheet1.ColumnCount - 1, Color.FromArgb(255, 255, 255));

            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;
        }

        private void ssSub_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            DataTable DtPat = new DataTable();

            string strCheck = string.Empty;

            TextBox[] txtRemark;
            TextBox[] txtMsg;
            ComboBox[] cboDept;
            CheckBox[] chkOpd;
            CheckBox[] chkHic;

            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }

            txtRemark = new TextBox[] { txtRemark1, txtRemark2, txtRemark3, txtRemark4, txtRemark5, txtRemark6, txtRemark7, txtRemark8, txtRemark9, txtRemark10, txtRemark11, txtRemark12, txtRemark13, txtRemark14, txtRemark15 , txtRemark16 , txtRemark17 , txtRemark18 , txtRemark19 };
            txtMsg = new TextBox[] { txtMsg1, txtMsg2, txtMsg3, txtMsg4, txtMsg5, txtMsg6, txtMsg7, txtMsg8, txtMsg9, txtMsg10, txtMsg11, txtMsg12, txtMsg13, txtMsg14 , txtMsg15 , txtMsg16 , txtMsg17 , txtMsg18 };
            cboDept = new ComboBox[] { cboDept1, cboDept2, cboDept3, cboDept4, cboDept5, cboDept6, cboDept7, cboDept8, cboDept9, cboDept10, cboDept11, cboDept12, cboDept13, cboDept14 , cboDept15 , cboDept16 , cboDept17 , cboDept18 };
            chkOpd = new CheckBox[] { chkOpd1, chkOpd2, chkOpd3, chkOpd4, chkOpd5, chkOpd6, chkOpd7, chkOpd8, chkOpd9, chkOpd10, chkOpd11, chkOpd12, chkOpd13, chkOpd14 , chkOpd15 , chkOpd16 , chkOpd17 , chkOpd18 };
            chkHic = new CheckBox[] { chkHic1, chkHic2, chkHic3, chkHic4, chkHic5, chkHic6, chkHic7, chkHic8, chkHic9, chkHic10, chkHic11, chkHic12, chkHic13, chkHic14, chkHic15, chkHic16, chkHic17, chkHic18 };

            eFrm_Clear();

            FstrRowID = ssSub_Sheet1.Cells[e.Row, 4].Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(JDATE, 'YYYY-MM-DD') JDATE, SINGU, SNAME, ";
            SQL += ComNum.VBLF + "        SABUN, PANO, STOPFLAG, ";
            SQL += ComNum.VBLF + "        SEX, PANO, REMARK_BIOPSY, ";
            SQL += ComNum.VBLF + "        BIOPSY_GBN, JUMIN1, JUMIN3, ";
            SQL += ComNum.VBLF + "        TEL, HPHONE, ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES1,'YYYY-MM-DD HH24:MI') EXAM1, ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES2,'YYYY-MM-DD HH24:MI') EXAM2, ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES3,'YYYY-MM-DD HH24:MI') EXAM3, ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES4,'YYYY-MM-DD HH24:MI') EXAM4, ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES6,'YYYY-MM-DD HH24:MI') EXAM6, ";
            SQL += ComNum.VBLF + "        EXAM5, ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES7,'YYYY-MM-DD HH24:MI') EXAM7, ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES8,'YYYY-MM-DD HH24:MI') EXAM8, ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES9,'YYYY-MM-DD HH24:MI') EXAM9, ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES10,'YYYY-MM-DD HH24:MI') EXAM10, ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES11,'YYYY-MM-DD HH24:MI') EXAM11, ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES12,'YYYY-MM-DD HH24:MI') EXAM12, ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES13,'YYYY-MM-DD HH24:MI') EXAM13, ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES14,'YYYY-MM-DD HH24:MI') EXAM14, ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES15,'YYYY-MM-DD HH24:MI') EXAM15, ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES16,'YYYY-MM-DD HH24:MI') EXAM16, ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES17,'YYYY-MM-DD HH24:MI') EXAM17, ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES18,'YYYY-MM-DD HH24:MI') EXAM18, ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES19,'YYYY-MM-DD HH24:MI') EXAM19, ";
            SQL += ComNum.VBLF + "        TO_CHAR(EXAMRES20,'YYYY-MM-DD HH24:MI') EXAM20, ";
            SQL += ComNum.VBLF + "        TO_CHAR(RESULT1,'YYYY-MM-DD') RESULT1, ";
            SQL += ComNum.VBLF + "        TO_CHAR(RESULT2,'YYYY-MM-DD') RESULT2, ";
            SQL += ComNum.VBLF + "        TO_CHAR(RESULT3,'YYYY-MM-DD') RESULT3, ";
            SQL += ComNum.VBLF + "        TO_CHAR(RESULT4,'YYYY-MM-DD') RESULT4, ";
            SQL += ComNum.VBLF + "        TO_CHAR(RESULT5,'YYYY-MM-DD') RESULT5, ";
            SQL += ComNum.VBLF + "        TO_CHAR(RESULT6,'YYYY-MM-DD') RESULT6, ";
            SQL += ComNum.VBLF + "        TO_CHAR(RESULT7,'YYYY-MM-DD') RESULT7, ";
            SQL += ComNum.VBLF + "        TO_CHAR(CDATE, 'YYYY-MM-DD') CDATE, ";
            SQL += ComNum.VBLF + "        BUSE, REMARK, JOBNAME, ";
            SQL += ComNum.VBLF + "        JUSO, Dept, OPD, ";
            SQL += ComNum.VBLF + "        HIC, SendMSG ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND ROWID = '" + FstrRowID + "'";
            SQL += ComNum.VBLF + "    AND GUBUN = '01' ";
            SQL += ComNum.VBLF + "  ORDER BY JDATE, SNAME ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            #region //기본사항 DISPLAY
            txtPtno.Text = Dt.Rows[0]["PANO"].ToString().Trim();  //등록번호
            clsPrint.GstrPoscoPrtPtno = Dt.Rows[0]["PANO"].ToString().Trim();

            txtSname.Text = Dt.Rows[0]["SNAME"].ToString().Trim();  //환자성명
            clsPrint.GstrPoscoPrtName = Dt.Rows[0]["SNAME"].ToString().Trim();

            txtJumin1.Text = Dt.Rows[0]["JUMIN1"].ToString().Trim();    //주민번호
            txtJumin2.Text = clsAES.DeAES(Dt.Rows[0]["JUMIN3"].ToString().Trim());
            clsPrint.GstrPoscoPrtJumin = Dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + clsAES.DeAES(Dt.Rows[0]["JUMIN3"].ToString().Trim());

            if (Dt.Rows[0]["SEX"].ToString().Trim() == "M")   //성별
                cboSex.Text = "M.남자";
            else
                cboSex.Text = "F.여자";

            txtTel.Text = Dt.Rows[0]["TEL"].ToString().Trim();  //전화번호
            txtJuso2.Text = Dt.Rows[0]["JUSO"].ToString().Trim();  //주소

            DtPat = CPF.Get_BasPatient(clsDB.DbCon, txtPtno.Text.Trim());

            if (DtPat.Rows.Count > 0)
            {
                if (DtPat.Rows[0]["BUILDNO"].ToString().Trim() != "")
                {
                    txtPostCode.Text = DtPat.Rows[0]["ZIPCODE3"].ToString().Trim();
                    txtJuso1.Text = CQ.Read_RoadJuso(clsDB.DbCon, DtPat.Rows[0]["BUILDNO"].ToString().Trim());
                    txtJuso2.Text = DtPat.Rows[0]["RoadDetail"].ToString().Trim();
                    chkRoad.Checked = true;
                }
                else
                {
                    txtPostCode.Text = DtPat.Rows[0]["ZIPCODE1"].ToString().Trim() + DtPat.Rows[0]["ZIPCODE2"].ToString().Trim();
                    txtJuso1.Text = CQ.Read_Juso(clsDB.DbCon, txtPostCode.Text);
                    txtRoadDong.Text = DtPat.Rows[0]["RoadDong"].ToString().Trim();
                    chkRoad.Checked = false;
                    if (DtPat.Rows[0]["Road"].ToString().Trim() == "Y") { chkRoad.Checked = true; }
                }

                txtHphone.Text = DtPat.Rows[0]["HPHONE"].ToString().Trim();
            }

            DtPat.Dispose();
            DtPat = null;


            dtpJdate.Text = Dt.Rows[0]["JDATE"].ToString().Trim(); //접수일자
            clsPrint.GstrPoscoPrtJDate = Dt.Rows[0]["JDATE"].ToString().Trim();
            dtpCdate.Text = Dt.Rows[0]["CDATE"].ToString().Trim(); //의뢰일자

            if (Dt.Rows[0]["SINGU"].ToString().Trim() == "1")   //신/구환
                cboSingu.Text = "1.신환";
            else
                cboSingu.Text = "2.구환";

            txtBuse.Text = Dt.Rows[0]["BUSE"].ToString().Trim();  //부서명
            clsPrint.GstrPoscoPrtBuse = Dt.Rows[0]["BUSE"].ToString().Trim();

            txtSabun.Text = Dt.Rows[0]["SABUN"].ToString().Trim();  //사원번호
            clsPrint.GstrPoscoPrtJikbun = Dt.Rows[0]["SABUN"].ToString().Trim();

            txtJobName.Text = Dt.Rows[0]["JOBNAME"].ToString().Trim();  //작업자

            chkStop.Checked = false;    //처리완료
            if (Dt.Rows[0]["STOPFLAG"].ToString().Trim() == "Y") { chkStop.Checked = true; }

            #endregion

            #region //검사예약 DISPLAY
            //복부초음파
            if (VB.IsDate(Dt.Rows[0]["EXAM1"].ToString().Trim()) == true)
            {
                dtpEdate1.Text = VB.Left(Dt.Rows[0]["EXAM1"].ToString().Trim(), 10);
                dtpEtime1.Text = VB.Right(Dt.Rows[0]["EXAM1"].ToString().Trim(), 5);
            }
            if (VB.Left(Dt.Rows[0]["EXAM1"].ToString().Trim(), 10) != "")
            {
                //dtpEdate1.Checked = true;
                strCheck = "OK";
            }
            //여성자궁검사
            if (VB.IsDate(Dt.Rows[0]["EXAM8"].ToString().Trim()) == true)
            {
                dtpEdate2.Text = VB.Left(Dt.Rows[0]["EXAM8"].ToString().Trim(), 10);
                dtpEtime2.Text = VB.Right(Dt.Rows[0]["EXAM8"].ToString().Trim(), 5);
            }
            if (VB.Left(Dt.Rows[0]["EXAM8"].ToString().Trim(), 10) != "")
            {
                //dtpEdate2.Checked = true;
                strCheck = "OK";
            }
            //위 내시경
            if (VB.IsDate(Dt.Rows[0]["EXAM2"].ToString().Trim()) == true)
            {
                dtpEdate3.Text = VB.Left(Dt.Rows[0]["EXAM2"].ToString().Trim(), 10);
                dtpEtime3.Text = VB.Right(Dt.Rows[0]["EXAM2"].ToString().Trim(), 5);
            }
            if (VB.Left(Dt.Rows[0]["EXAM2"].ToString().Trim(), 10) != "")
            {
                //dtpEdate3.Checked = true;
                strCheck = "OK";
            }
            //위 내시경(수면)
            if (VB.IsDate(Dt.Rows[0]["EXAM3"].ToString().Trim()) == true)
            {
                dtpEdate4.Text = VB.Left(Dt.Rows[0]["EXAM3"].ToString().Trim(), 10);
                dtpEtime4.Text = VB.Right(Dt.Rows[0]["EXAM3"].ToString().Trim(), 5);
            }
            if (VB.Left(Dt.Rows[0]["EXAM3"].ToString().Trim(), 10) != "")
            {
                //dtpEdate4.Checked = true;
                strCheck = "OK";
            }
            //대장 내시경(수면)
            if (VB.IsDate(Dt.Rows[0]["EXAM6"].ToString().Trim()) == true)
            {
                dtpEdate5.Text = VB.Left(Dt.Rows[0]["EXAM6"].ToString().Trim(), 10);
                dtpEtime5.Text = VB.Right(Dt.Rows[0]["EXAM6"].ToString().Trim(), 5);
            }
            if (VB.Left(Dt.Rows[0]["EXAM6"].ToString().Trim(), 10) != "")
            {
                //dtpEdate5.Checked = true;
                strCheck = "OK";
            }
            //흉부CT
            if (VB.IsDate(Dt.Rows[0]["EXAM7"].ToString().Trim()) == true)
            {
                dtpEdate6.Text = VB.Left(Dt.Rows[0]["EXAM7"].ToString().Trim(), 10);
                dtpEtime6.Text = VB.Right(Dt.Rows[0]["EXAM7"].ToString().Trim(), 5);
            }
            if (VB.Left(Dt.Rows[0]["EXAM7"].ToString().Trim(), 10) != "")
            {
                //dtpEdate6.Checked = true;
                strCheck = "OK";
            }
            //뇌CT
            if (VB.IsDate(Dt.Rows[0]["EXAM9"].ToString().Trim()) == true)
            {
                dtpEdate7.Text = VB.Left(Dt.Rows[0]["EXAM9"].ToString().Trim(), 10);
                dtpEtime7.Text = VB.Right(Dt.Rows[0]["EXAM9"].ToString().Trim(), 5);
            }
            if (VB.Left(Dt.Rows[0]["EXAM9"].ToString().Trim(), 10) != "")
            {
                //dtpEdate7.Checked = true;
                strCheck = "OK";
            }
            //경추CT
            if (VB.IsDate(Dt.Rows[0]["EXAM10"].ToString().Trim()) == true)
            {
                dtpEdate8.Text = VB.Left(Dt.Rows[0]["EXAM10"].ToString().Trim(), 10);
                dtpEtime8.Text = VB.Right(Dt.Rows[0]["EXAM10"].ToString().Trim(), 5);
            }
            if (VB.Left(Dt.Rows[0]["EXAM10"].ToString().Trim(), 10) != "")
            {
                //dtpEdate8.Checked = true;
                strCheck = "OK";
            }
            //요추CT
            if (VB.IsDate(Dt.Rows[0]["EXAM11"].ToString().Trim()) == true)
            {
                dtpEdate9.Text = VB.Left(Dt.Rows[0]["EXAM11"].ToString().Trim(), 10);
                dtpEtime9.Text = VB.Right(Dt.Rows[0]["EXAM11"].ToString().Trim(), 5);
            }
            if (VB.Left(Dt.Rows[0]["EXAM11"].ToString().Trim(), 10) != "")
            {
                //dtpEdate9.Checked = true;
                strCheck = "OK";
            }
            //심장CT
            if (VB.IsDate(Dt.Rows[0]["EXAM12"].ToString().Trim()) == true)
            {
                dtpEdate10.Text = VB.Left(Dt.Rows[0]["EXAM12"].ToString().Trim(), 10);
                dtpEtime10.Text = VB.Right(Dt.Rows[0]["EXAM12"].ToString().Trim(), 5);
            }
            if (VB.Left(Dt.Rows[0]["EXAM12"].ToString().Trim(), 10) != "")
            {
                //dtpEdate10.Checked = true;
                strCheck = "OK";
            }
            //심장초음파
            if (VB.IsDate(Dt.Rows[0]["EXAM13"].ToString().Trim()) == true)
            {
                dtpEdate11.Text = VB.Left(Dt.Rows[0]["EXAM13"].ToString().Trim(), 10);
                dtpEtime11.Text = VB.Right(Dt.Rows[0]["EXAM13"].ToString().Trim(), 5);
            }
            if (VB.Left(Dt.Rows[0]["EXAM13"].ToString().Trim(), 10) != "")
            {
                //dtpEdate11.Checked = true;
                strCheck = "OK";
            }
            //경동맥초음파
            if (VB.IsDate(Dt.Rows[0]["EXAM14"].ToString().Trim()) == true)
            {
                dtpEdate12.Text = VB.Left(Dt.Rows[0]["EXAM14"].ToString().Trim(), 10);
                dtpEtime12.Text = VB.Right(Dt.Rows[0]["EXAM14"].ToString().Trim(), 5);
            }
            if (VB.Left(Dt.Rows[0]["EXAM14"].ToString().Trim(), 10) != "")
            {
                //dtpEdate12.Checked = true;
                strCheck = "OK";
            }
            //뇌혈류초음파
            if (VB.IsDate(Dt.Rows[0]["EXAM15"].ToString().Trim()) == true)
            {
                dtpEdate13.Text = VB.Left(Dt.Rows[0]["EXAM15"].ToString().Trim(), 10);
                dtpEtime13.Text = VB.Right(Dt.Rows[0]["EXAM15"].ToString().Trim(), 5);
            }
            if (VB.Left(Dt.Rows[0]["EXAM15"].ToString().Trim(), 10) != "")
            {
                //dtpEdate13.Checked = true;
                strCheck = "OK";
            }
            //여성유방검진
            if (VB.IsDate(Dt.Rows[0]["EXAM16"].ToString().Trim()) == true)
            {
                dtpEdate14.Text = VB.Left(Dt.Rows[0]["EXAM16"].ToString().Trim(), 10);
                dtpEtime14.Text = VB.Right(Dt.Rows[0]["EXAM16"].ToString().Trim(), 5);
            }
            if (VB.Left(Dt.Rows[0]["EXAM16"].ToString().Trim(), 10) != "")
            {
                //dtpEdate14.Checked = true;
                strCheck = "OK";
            }

            //무릎 MRI 
            if (VB.IsDate(Dt.Rows[0]["EXAM17"].ToString().Trim()) == true)
            {
                dtpEdate15.Text = VB.Left(Dt.Rows[0]["EXAM17"].ToString().Trim(), 10);
                dtpEtime15.Text = VB.Right(Dt.Rows[0]["EXAM17"].ToString().Trim(), 5);
            }
            if (VB.Left(Dt.Rows[0]["EXAM17"].ToString().Trim(), 10) != "")
            {
                //dtpEdate14.Checked = true;
                strCheck = "OK";
            }

            //뇌 MRI 
            if (VB.IsDate(Dt.Rows[0]["EXAM18"].ToString().Trim()) == true)
            {
                dtpEdate16.Text = VB.Left(Dt.Rows[0]["EXAM18"].ToString().Trim(), 10);
                dtpEtime16.Text = VB.Right(Dt.Rows[0]["EXAM18"].ToString().Trim(), 5);
            }
            if (VB.Left(Dt.Rows[0]["EXAM18"].ToString().Trim(), 10) != "")
            {
                //dtpEdate14.Checked = true;
                strCheck = "OK";
            }

            //경추 MRI
            if (VB.IsDate(Dt.Rows[0]["EXAM19"].ToString().Trim()) == true)
            {
                dtpEdate17.Text = VB.Left(Dt.Rows[0]["EXAM19"].ToString().Trim(), 10);
                dtpEtime17.Text = VB.Right(Dt.Rows[0]["EXAM19"].ToString().Trim(), 5);
            }
            if (VB.Left(Dt.Rows[0]["EXAM19"].ToString().Trim(), 10) != "")
            {
                //dtpEdate14.Checked = true;
                strCheck = "OK";
            }

            //요추 MRI 
            if (VB.IsDate(Dt.Rows[0]["EXAM20"].ToString().Trim()) == true)
            {
                dtpEdate18.Text = VB.Left(Dt.Rows[0]["EXAM20"].ToString().Trim(), 10);
                dtpEtime18.Text = VB.Right(Dt.Rows[0]["EXAM20"].ToString().Trim(), 5);
            }
            if (VB.Left(Dt.Rows[0]["EXAM20"].ToString().Trim(), 10) != "")
            {
                //dtpEdate14.Checked = true;
                strCheck = "OK";
            }



            dtpResult.Text = Dt.Rows[0]["RESULT5"].ToString().Trim();

            //참고사항
            for (int i = 0; i < VB.Split(Dt.Rows[0]["REMARK"].ToString().Trim(), "^^").Length - 1; i++)
                txtRemark[i].Text = VB.Pstr(Dt.Rows[0]["REMARK"].ToString().Trim(), "^^", i + 1);

            //전달사항
            for (int i = 0; i < VB.Split(Dt.Rows[0]["SendMsg"].ToString().Trim(), "^^").Length - 1; i++)
                txtMsg[i].Text = VB.Pstr(Dt.Rows[0]["SendMsg"].ToString().Trim(), "^^", i + 1);

            //접수내용
            for (int i = 0; i < VB.Split(Dt.Rows[0]["DEPT"].ToString().Trim(), "^^").Length - 1; i++)
                cboDept[i].Text = VB.Pstr(Dt.Rows[0]["DEPT"].ToString().Trim(), "^^", i + 1);

            for (int i = 0; i < VB.Split(Dt.Rows[0]["OPD"].ToString().Trim(), "^^").Length - 1; i++)
            {
                if (VB.Pstr(Dt.Rows[0]["OPD"].ToString().Trim(), "^^", i + 1) == "0")
                    chkOpd[i].Checked = false;
                else
                    chkOpd[i].Checked = true;
            }

            for (int i = 0; i < VB.Split(Dt.Rows[0]["HIC"].ToString().Trim(), "^^").Length - 1; i++)
            {
                if (VB.Pstr(Dt.Rows[0]["HIC"].ToString().Trim(), "^^", i + 1) == "0")
                    chkHic[i].Checked = false;
                else
                    chkHic[i].Checked = true;
            }


            txtBioRemark.Text = Dt.Rows[0]["REMARK_BIOPSY"].ToString().Trim();
            switch (Dt.Rows[0]["BIOPSY_GBN"].ToString().Trim())
            {
                case "1":
                    rdoBio0.Checked = true;
                    break;
                case "2":
                    rdoBio1.Checked = true;
                    break;
                case "3":
                    rdoBio2.Checked = true;
                    break;
            }

            chkStop.Checked = false;
            if (Dt.Rows[0]["STOPFLAG"].ToString().Trim() == "Y") { chkStop.Checked = true; }

            chkBiopsy.Checked = false;
            if (Dt.Rows[0]["EXAM5"].ToString().Trim() == "Y") { chkBiopsy.Checked = true; }

            txtTel.Text = Dt.Rows[0]["TEL"].ToString().Trim();
            txtJuso2.Text = Dt.Rows[0]["JUSO"].ToString().Trim();
            #endregion

            Dt.Dispose();
            Dt = null;




            //'주소통합
            SQL = " SELECT  ZIPCODE1,ZIPCODE2,JUSO,JICODE,Hphone,Road,RoadDong,ZIPCODE3,BUILDNO,ROADDETAIL ";
            SQL += ComNum.VBLF + "  FROM ADMIN.BAS_PATIENT ";
            SQL += ComNum.VBLF + "   WHERE PANO ='" + txtPtno.Text + "' ";

            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count > 0)
            {
                txtBuildNo.Text = Dt.Rows[0]["BUILDNO"].ToString().Trim();

                if (Dt.Rows[0]["BUILDNO"].ToString().Trim() != "")
                {
                    txtPostCode.Text = Dt.Rows[0]["ZIPCODE3"].ToString().Trim();
                    txtJuso1.Text = CQ.Read_RoadJuso(clsDB.DbCon, Dt.Rows[0]["BUILDNO"].ToString().Trim());
                    txtJuso2.Text = Dt.Rows[0]["RoadDetail"].ToString().Trim();
                    chkRoad.Checked = true;
                }
                else
                {
                    txtPostCode.Text = Dt.Rows[0]["ZIPCODE1"].ToString().Trim() + Dt.Rows[0]["ZIPCODE2"].ToString().Trim();
                    txtJuso1.Text = CQ.Read_Juso(clsDB.DbCon, txtPostCode.Text);
                    txtJuso2.Text = Dt.Rows[0]["Juso"].ToString().Trim();
                    txtRoadDong.Text = Dt.Rows[0]["RoadDong"].ToString().Trim();
                    chkRoad.Checked = false;
                    if (Dt.Rows[0]["Road"].ToString().Trim() == "Y")
                    {
                        chkRoad.Checked = true;
                    }
                }

                txtHphone.Text = Dt.Rows[0]["HPHONE"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;

            btnNew.Enabled = false;
            btnSave.Enabled = true;
            btnDelete.Enabled = true;
        }

        //공지사항 저장 버튼
        private void btnSaveAlert_Click(object sender, EventArgs e)
        {
            CPQ.UPDATE_POSCO_MSG(clsDB.DbCon, "전체", "01", clsVbfunc.QuotationChange(txtBigo.Text));
        }

        //등록번호 검색 버튼
        private void btnPtnoHelp_Click(object sender, EventArgs e)
        {
            DataTable DtPat = new DataTable();

            clsPublic.GstrHelpCode = "";

            frmPatientSearch frm = new frmPatientSearch();
            frm.ShowDialog();
            OF.fn_ClearMemory(frm);


            if (clsPublic.GstrHelpCode != "")
            {
                txtPtno.Text = VB.Pstr(clsPublic.GstrHelpCode, "{}", 1);
                txtSname.Text = VB.Pstr(clsPublic.GstrHelpCode, "{}", 2);

                if (VB.Pstr(clsPublic.GstrHelpCode, "{}", 3) == "M")
                    cboSex.SelectedIndex = 0;
                else
                    cboSex.SelectedIndex = 1;

                txtJumin1.Text = VB.Pstr(VB.Pstr(clsPublic.GstrHelpCode, "{}", 4), "-", 1);
                txtJumin2.Text = VB.Pstr(VB.Pstr(clsPublic.GstrHelpCode, "{}", 4), "-", 2);
                txtTel.Text = VB.Pstr(clsPublic.GstrHelpCode, "{}", 6);

                //기존정보 가져오기
                DtPat = CPF.Get_BasPatient(clsDB.DbCon, txtPtno.Text.Trim());

                if (DtPat.Rows.Count > 0)
                {
                    txtTel.Text = DtPat.Rows[0]["TEL"].ToString().Trim();
                    txtHphone.Text = DtPat.Rows[0]["HPHONE"].ToString().Trim();
                    txtBuildNo.Text = DtPat.Rows[0]["BUILDNO"].ToString().Trim();

                    if (DtPat.Rows[0]["BUILDNO"].ToString().Trim() != "")
                    {
                        txtPostCode.Text = DtPat.Rows[0]["ZIPCODE3"].ToString().Trim();
                        txtJuso1.Text = CQ.Read_RoadJuso(clsDB.DbCon, DtPat.Rows[0]["BUILDNO"].ToString().Trim());
                        txtJuso2.Text = DtPat.Rows[0]["RoadDetail"].ToString().Trim();
                        chkRoad.Checked = true;
                    }
                    else
                    {
                        txtPostCode.Text = DtPat.Rows[0]["ZIPCODE1"].ToString().Trim() + DtPat.Rows[0]["ZIPCODE2"].ToString().Trim();
                        txtJuso1.Text = CQ.Read_Juso(clsDB.DbCon, txtPostCode.Text);
                        txtJuso2.Text = DtPat.Rows[0]["Juso"].ToString().Trim();
                        txtRoadDong.Text = DtPat.Rows[0]["RoadDong"].ToString().Trim();
                        chkRoad.Checked = false;
                        if (DtPat.Rows[0]["Road"].ToString().Trim() == "Y") { chkRoad.Checked = true; }
                    }
                }

                DtPat.Dispose();
                DtPat = null;

                //포스코 접수정보 가져오기
                DtPat = CPF.Get_PoscoPatient(clsDB.DbCon, txtPtno.Text.Trim());

                if (DtPat.Rows.Count > 0)
                {
                    if (txtTel.Text.Trim() == "") { txtTel.Text = DtPat.Rows[0]["TEL"].ToString().Trim(); }
                    if (txtHphone.Text.Trim() == "") { txtHphone.Text = DtPat.Rows[0]["HPHONE"].ToString().Trim(); }

                    txtSabun.Text = DtPat.Rows[0]["Sabun"].ToString().Trim();
                    txtBuse.Text = DtPat.Rows[0]["Buse"].ToString().Trim();

                    if (DtPat.Rows[0]["Juso"].ToString().Trim() != "")
                        txtJuso2.Text = DtPat.Rows[0]["Juso"].ToString().Trim();
                }

                DtPat.Dispose();
                DtPat = null;

                cboSingu.Focus();
            }
        }

        //자격조회 버튼
        private void btnNhic_Click(object sender, EventArgs e)
        {
            GET_NHIC();
        }

        private void GET_NHIC()
        {
            string strSname = string.Empty;
            string strJumin1 = string.Empty;
            string strJumin2 = string.Empty;

            DataTable DtPat = new DataTable();

            if (txtPtno.Text.Trim() == "") { return; }

            DtPat = CPF.Get_BasPatient(clsDB.DbCon, txtPtno.Text.Trim());

            if (DtPat.Rows.Count > 0)
            {
                strSname = DtPat.Rows[0]["SNAME"].ToString().Trim();
                strJumin1 = DtPat.Rows[0]["JUMIN1"].ToString().Trim();
                strJumin2 = clsAES.DeAES(DtPat.Rows[0]["JUMIN3"].ToString().Trim());

                //clsPublic.GstrHelpCode = txtPtno.Text.Trim() + "," + "MD" + DtPat.Rows[0]["SNAME"].ToString().Trim() + ",";
                //clsPublic.GstrHelpCode += DtPat.Rows[0]["JUMIN1"].ToString().Trim() + clsAES.DeAES(DtPat.Rows[0]["JUMIN3"].ToString().Trim()) + ",";
                //clsPublic.GstrHelpCode += clsPublic.GstrSysDate;
            }

            DtPat.Dispose();
            DtPat = null;

            frmPmpaCheckNhic frm = new frmPmpaCheckNhic(txtPtno.Text, "MD", strSname, strJumin1, strJumin2, clsPublic.GstrSysDate, "");
            frm.StartPosition = FormStartPosition.Manual;
            frm.Location = new Point(10, 10);
            frm.ShowDialog();
            OF.fn_ClearMemory(frm);


            FstrGkiho = "";

            if (clsPublic.GstrHelpCode != "")
            {
                if (VB.Pstr(VB.Pstr(clsPublic.GstrHelpCode, ";", 2), ".", 1) != "7" && VB.Pstr(VB.Pstr(clsPublic.GstrHelpCode, ";", 2), ".", 1) != "8")
                {

                    FstrGkiho = VB.Pstr(clsPublic.GstrHelpCode, ";", 5);
                    FstrGkiho = VB.Left(FstrGkiho, 1) + "-" + VB.Mid(FstrGkiho, 2, FstrGkiho.Length);

                    if (FstrGkiho.Length < 12) { FstrGkiho = ""; }
                }
            }

            clsPublic.GstrHelpCode = "";
        }

        //후불대상 등록 버튼
        private void btnAuto_Click(object sender, EventArgs e)
        {
            AUTO_MST_INSERT();
        }

        private void AUTO_MST_INSERT()
        {
            TextBox[] dtpEdate;
            string strRowID = string.Empty;

            dtpEdate = new TextBox[] { dtpEdate1, dtpEdate2, dtpEdate3, dtpEdate4, dtpEdate5, dtpEdate6, dtpEdate7, dtpEdate8, dtpEdate9, dtpEdate10, dtpEdate11, dtpEdate12, dtpEdate13, dtpEdate14, dtpEdate15, dtpEdate16, dtpEdate17, dtpEdate18 };

            if (txtPtno.Text.Trim() == "") { return; }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " DELETE " + ComNum.DB_PMPA + "BAS_AUTO_MST ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND PANO      = '" + txtPtno.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "    AND GUBUN     = '1' ";
                SQL += ComNum.VBLF + "    AND GBPOSCO   = '2' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }


            for (int i = 0; i < 18; i++)
            {
                if (dtpEdate[i].Text != "")
                {
                    clsDB.setBeginTran(clsDB.DbCon);

                    strRowID = "";

                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT ROWID ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_AUTO_MST ";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND PANO      = '" + txtPtno.Text.Trim() + "' ";
                    SQL += ComNum.VBLF + "    AND GUBUN     ='1' ";
                    SQL += ComNum.VBLF + "    AND SDate     = TO_DATE('" + dtpEdate[i].Text + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND GBPOSCO   = '2' ";
                    SQL += ComNum.VBLF + "    AND (DelDate IS NULL OR DelDate ='') ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (Dt.Rows.Count > 0)
                        strRowID = Dt.Rows[0]["ROWID"].ToString().Trim();

                    Dt.Dispose();
                    Dt = null;


                    if (strRowID == "")
                    {
                        try
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "BAS_AUTO_MST ";
                            SQL += ComNum.VBLF + "        (PANO, SNAME, GUBUN, ";
                            SQL += ComNum.VBLF + "         SDATE, EDATE, DELDATE,";
                            SQL += ComNum.VBLF + "         ENTSABUN, ENTDATE, ENTDATE2, ";
                            SQL += ComNum.VBLF + "         REMARK, GBPOSCO ) ";
                            SQL += ComNum.VBLF + " VALUES ('" + txtPtno.Text.Trim() + "', ";
                            SQL += ComNum.VBLF + "         '" + txtSname.Text.Trim() + "', ";
                            SQL += ComNum.VBLF + "         '1', ";
                            SQL += ComNum.VBLF + "         TO_DATE('" + dtpEdate[i].Text + "','YYYY-MM-DD'), ";
                            SQL += ComNum.VBLF + "         TO_DATE('" + dtpEdate[i].Text + "','YYYY-MM-DD'), ";
                            SQL += ComNum.VBLF + "         '', ";
                            SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
                            SQL += ComNum.VBLF + "         SYSDATE, ";
                            SQL += ComNum.VBLF + "         SYSDATE, ";
                            SQL += ComNum.VBLF + "         '포스코위탁검사', ";
                            SQL += ComNum.VBLF + "         '2') ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                        }
                        catch (Exception ex)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                    else
                    {
                        try
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "BAS_AUTO_MST_HIS  ";
                            SQL += ComNum.VBLF + "        (PANO, SNAME, GUBUN, ";
                            SQL += ComNum.VBLF + "         DEPTCODE, SDATE, EDATE, ";
                            SQL += ComNum.VBLF + "         DELDATE, ENTDATE, ENTDATE2, ";
                            SQL += ComNum.VBLF + "         ENTSABUN, REMARK, GBPOSCO)   ";
                            SQL += ComNum.VBLF + "  SELECT PANO, SNAME, GUBUN, ";
                            SQL += ComNum.VBLF + "         DEPTCODE, SDATE, EDATE, ";
                            SQL += ComNum.VBLF + "         DELDATE, ENTDATE, ENTDATE2, ";
                            SQL += ComNum.VBLF + "         ENTSABUN, REMARK, GBPOSCO ";
                            SQL += ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "BAS_AUTO_MST ";
                            SQL += ComNum.VBLF + "   WHERE ROWID = '" + strRowID + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            SQL = "";
                            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_AUTO_MST ";
                            SQL += ComNum.VBLF + "    SET ENTSABUN  = '" + clsType.User.IdNumber + "', ";
                            SQL += ComNum.VBLF + "        ENTDATE2  = SYSDATE, ";
                            SQL += ComNum.VBLF + "        REMARK    = '포스코위탁검사' ";
                            SQL += ComNum.VBLF + "  WHERE ROWID     = '" + strRowID + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                }
            }

            Cursor.Current = Cursors.Default;

            if (FstrAuto != "OK") { ComFunc.MsgBox("일괄후불대상 등록완료!", "작업완료"); }
        }

        private void btnPost_Click(object sender, EventArgs e)
        {
            Fvalue = "";

            frmSearchRoadAdd frm = new frmSearchRoadAdd();
            frm.rSetGstrValue += new frmSearchRoadAdd.SetGstrValue(ePost_value);
            frm.ShowDialog();
            OF.fn_ClearMemory(frm);

            if (Fvalue != "")
            {
                txtPostCode.Text = VB.Left(VB.Pstr(Fvalue, "|", 1), 3);
                txtPostCode.Text += VB.Mid(VB.Pstr(Fvalue, "|", 1), 4, 2);

                txtJuso1.Text = VB.Pstr(Fvalue, "|", 2).Trim();
                chkRoad.Checked = true;

                txtBuildNo.Text = VB.Pstr(Fvalue, "|", 5).Trim();
                txtJuso2.Text = "";
            }
            else
            {
                txtBuildNo.Text = "";
            }
            txtJuso2.Focus();
        }

        private void ePost_value(string GstrValue)
        {
            Fvalue = GstrValue;
        }

        //신규 버튼
        private void btnNew_Click(object sender, EventArgs e)
        {
            eFrm_Clear();
            btnSave.Enabled = true;
            btnDelete.Enabled = false;
            txtPtno.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string strRemark = string.Empty;
            string strDept = string.Empty;
            string strOpd = string.Empty;
            string strHic = string.Empty;
            string strMsg = string.Empty;
            string strBioGb = string.Empty;

            TextBox[] dtpEdate;
            TextBox[] dtpEtime;
            TextBox[] txtRemark;
            TextBox[] txtMsg;
            ComboBox[] cboDept;
            CheckBox[] chkOpd;
            CheckBox[] chkHic;
            Label[] lblName;

            dtpEdate = new TextBox[] { dtpEdate1, dtpEdate2, dtpEdate3, dtpEdate4, dtpEdate5, dtpEdate6, dtpEdate7, dtpEdate8, dtpEdate9, dtpEdate10, dtpEdate11, dtpEdate12, dtpEdate13, dtpEdate14 , dtpEdate15 , dtpEdate16 , dtpEdate17 , dtpEdate18 };
            dtpEtime = new TextBox[] { dtpEtime1, dtpEtime2, dtpEtime3, dtpEtime4, dtpEtime5, dtpEtime6, dtpEtime7, dtpEtime8, dtpEtime9, dtpEtime10, dtpEtime11, dtpEtime12, dtpEtime13, dtpEtime14 , dtpEtime15 , dtpEtime16 , dtpEtime17 , dtpEtime18 };
            txtRemark = new TextBox[] { txtRemark1, txtRemark2, txtRemark3, txtRemark4, txtRemark5, txtRemark6, txtRemark7, txtRemark8, txtRemark9, txtRemark10, txtRemark11, txtRemark12, txtRemark13, txtRemark14, txtRemark15, txtRemark16, txtRemark17, txtRemark18, txtRemark19 };
            txtMsg = new TextBox[] { txtMsg1, txtMsg2, txtMsg3, txtMsg4, txtMsg5, txtMsg6, txtMsg7, txtMsg8, txtMsg9, txtMsg10, txtMsg11, txtMsg12, txtMsg13, txtMsg14 , txtMsg15, txtMsg16 , txtMsg17 , txtMsg18};
            cboDept = new ComboBox[] { cboDept1, cboDept2, cboDept3, cboDept4, cboDept5, cboDept6, cboDept7, cboDept8, cboDept9, cboDept10, cboDept11, cboDept12, cboDept13, cboDept14, cboDept15, cboDept16, cboDept17, cboDept18 };
            chkOpd = new CheckBox[] { chkOpd1, chkOpd2, chkOpd3, chkOpd4, chkOpd5, chkOpd6, chkOpd7, chkOpd8, chkOpd9, chkOpd10, chkOpd11, chkOpd12, chkOpd13, chkOpd14 , chkOpd15, chkOpd16, chkOpd17, chkOpd18 };
            chkHic = new CheckBox[] { chkHic1, chkHic2, chkHic3, chkHic4, chkHic5, chkHic6, chkHic7, chkHic8, chkHic9, chkHic10, chkHic11, chkHic12, chkHic13, chkHic14 , chkHic15 , chkHic16 , chkHic17 , chkHic18};
            lblName = new Label[] { lblName1, lblName2, lblName3, lblName4, lblName5, lblName6, lblName7, lblName8, lblName9, lblName10, lblName11, lblName12, lblName13, lblName14 , lblName14 , lblName14 , lblName14 , lblName14 };

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) { return; }     //권한확인

            GET_NHIC();

            FstrAuto = "OK";

            if (FstrGkiho == "")
            {
                ComFunc.MsgBox("건강보험 자격조회중 증번호가 없습니다.", "외래접수불가");
                txtPtno.Focus();
                return;
            }

            if (txtJobName.Text.Trim() == "")
            {
                ComFunc.MsgBox("작업자명을 입력하시기 바랍니다.", "확인");
                txtJobName.Focus();
                return;
            }

            for (int i = 0; i < 18; i++)
            {
                if (chkOpd[i].Checked == true)
                {
                    if (cboDept[i].Text.Trim() == "")
                    {
                        ComFunc.MsgBox("외래예약 대상자이나, 진료과가 공란입니다.", "확인");
                        return;
                    }
                    else
                    {
                        if (dtpEdate[i].Text.Trim() == "" || dtpEtime[i].Text.Trim() == "")
                        {
                            ComFunc.MsgBox("외래예약 대상자이나, 진료일자가 비선택 되었습니다.", "확인");
                            return;
                        }
                        else
                        {
                            if (CPF.READ_DOCTOR_SCHEDULE(clsDB.DbCon, cboDept[i].Text, dtpEdate[i].Text, dtpEtime[i].Text) == "")
                            {
                                clsPublic.GstrMsgTitle = "진료스케줄 확인요망";
                                clsPublic.GstrMsgList = "예약일자에 해당의사 스케쥴이 없습니다!" + '\r';
                                clsPublic.GstrMsgList += "예약진료과 : " + cboDept[i].Text + '\r';
                                clsPublic.GstrMsgList += "예약일자   : " + dtpEdate[i].Text + '\r';
                                clsPublic.GstrMsgList += "예약시각   : " + dtpEtime[i].Text + '\r';

                                ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
                                return;
                            }
                        }
                    }
                }
            }

            for (int i = 0; i <= 18; i++)
            {
                strRemark += txtRemark[i].Text + "^^";
            }
            for (int i = 0; i < 18; i++)
            {
                strMsg += txtMsg[i].Text + "^^";
                strDept += VB.Left(cboDept[i].Text, 2) + "^^";

                if (chkOpd[i].Checked == true)
                    strOpd += "1" + "^^";
                else
                    strOpd += "0" + "^^";

                if (chkHic[i].Checked == true)
                    strHic += "1" + "^^";
                else
                    strHic += "0" + "^^";
            }

            if (rdoBio0.Checked == true)
                strBioGb = "1";
            else if (rdoBio1.Checked == true)
                strBioGb = "2";
            else if (rdoBio2.Checked == true)
                strBioGb = "3";


            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (FstrRowID == "")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
                    SQL += ComNum.VBLF + "       (JDATE, PANO, SNAME, ";
                    SQL += ComNum.VBLF + "        SEX, JUMIN1, JUMIN3, ";
                    SQL += ComNum.VBLF + "        SINGU, SABUN, TEL, ";
                    SQL += ComNum.VBLF + "        HPHONE, CDate, EXAMRES1, ";
                    SQL += ComNum.VBLF + "        EXAMRES2,  EXAMRES3,  EXAMRES6, ";
                    SQL += ComNum.VBLF + "        EXAMRES7,  EXAMRES8,  EXAMRES9, ";
                    SQL += ComNum.VBLF + "        EXAMRES10, EXAMRES11, EXAMRES12, ";
                    SQL += ComNum.VBLF + "        EXAMRES13, EXAMRES14, EXAMRES15, ";
                    SQL += ComNum.VBLF + "        EXAMRES16, EXAMRES17,EXAMRES18,EXAMRES19,EXAMRES20 , ENTDATE, STOPFLAG, ";
                    SQL += ComNum.VBLF + "        EXAM5, BUSE, Remark, ";
                    SQL += ComNum.VBLF + "        JOBNAME, Juso, GUBUN, ";
                    SQL += ComNum.VBLF + "        REMARK_BIOPSY, BIOPSY_GBN, Dept, ";
                    SQL += ComNum.VBLF + "        OPD, HIC, SendMsg ) ";
                    SQL += ComNum.VBLF + "VALUES (TO_DATE('" + dtpJdate.Text + "','YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "        '" + txtPtno.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        '" + txtSname.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        '" + VB.Left(cboSex.Text, 1) + "', ";
                    SQL += ComNum.VBLF + "        '" + txtJumin1.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        '" + clsAES.AES(txtJumin2.Text.Trim()) + "', ";
                    SQL += ComNum.VBLF + "        '" + VB.Left(cboSingu.Text, 1) + "', ";
                    SQL += ComNum.VBLF + "        '" + txtSabun.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        '" + txtTel.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        '" + txtHphone.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        TO_DATE('" + dtpCdate.Text + "','YYYY-MM-DD'), ";

                    if (dtpEdate[0].Text != "")
                        SQL += ComNum.VBLF + "    TO_DATE('" + dtpEdate[0].Text + " " + dtpEtime[0].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    '', ";

                    if (dtpEdate[2].Text != "")
                        SQL += ComNum.VBLF + "    TO_DATE('" + dtpEdate[2].Text + " " + dtpEtime[2].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    '', ";

                    if (dtpEdate[3].Text != "")
                        SQL += ComNum.VBLF + "    TO_DATE('" + dtpEdate[3].Text + " " + dtpEtime[3].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    '', ";

                    if (dtpEdate[4].Text != "")
                        SQL += ComNum.VBLF + "    TO_DATE('" + dtpEdate[4].Text + " " + dtpEtime[4].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    '', ";

                    if (dtpEdate[5].Text != "")
                        SQL += ComNum.VBLF + "    TO_DATE('" + dtpEdate[5].Text + " " + dtpEtime[5].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    '', ";

                    if (dtpEdate[1].Text != "")
                        SQL += ComNum.VBLF + "    TO_DATE('" + dtpEdate[1].Text + " " + dtpEtime[1].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    '', ";

                    if (dtpEdate[6].Text != "")
                        SQL += ComNum.VBLF + "    TO_DATE('" + dtpEdate[6].Text + " " + dtpEtime[6].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    '', ";

                    if (dtpEdate[7].Text != "")
                        SQL += ComNum.VBLF + "    TO_DATE('" + dtpEdate[7].Text + " " + dtpEtime[7].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    '', ";

                    if (dtpEdate[8].Text != "")
                        SQL += ComNum.VBLF + "    TO_DATE('" + dtpEdate[8].Text + " " + dtpEtime[8].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    '', ";

                    if (dtpEdate[9].Text != "")
                        SQL += ComNum.VBLF + "    TO_DATE('" + dtpEdate[9].Text + " " + dtpEtime[9].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    '', ";

                    if (dtpEdate[10].Text != "")
                        SQL += ComNum.VBLF + "    TO_DATE('" + dtpEdate[10].Text + " " + dtpEtime[10].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    '', ";

                    if (dtpEdate[11].Text != "")
                        SQL += ComNum.VBLF + "    TO_DATE('" + dtpEdate[11].Text + " " + dtpEtime[11].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    '', ";

                    if (dtpEdate[12].Text != "")
                        SQL += ComNum.VBLF + "    TO_DATE('" + dtpEdate[12].Text + " " + dtpEtime[12].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    '', ";

                    if (dtpEdate[13].Text != "")
                        SQL += ComNum.VBLF + "    TO_DATE('" + dtpEdate[13].Text + " " + dtpEtime[13].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    '', ";

                    if (dtpEdate[14].Text != "")
                        SQL += ComNum.VBLF + "    TO_DATE('" + dtpEdate[14].Text + " " + dtpEtime[14].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    '', ";

                    if (dtpEdate[15].Text != "")
                        SQL += ComNum.VBLF + "    TO_DATE('" + dtpEdate[15].Text + " " + dtpEtime[15].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    '', ";

                    if (dtpEdate[16].Text != "")
                        SQL += ComNum.VBLF + "    TO_DATE('" + dtpEdate[16].Text + " " + dtpEtime[16].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    '', ";

                    if (dtpEdate[17].Text != "")
                        SQL += ComNum.VBLF + "    TO_DATE('" + dtpEdate[17].Text + " " + dtpEtime[17].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    '', ";

                    SQL += ComNum.VBLF + "        SYSDATE, ";

                    if (chkStop.Checked == true)
                        SQL += ComNum.VBLF + "    'Y',";
                    else
                        SQL += ComNum.VBLF + "    '',  ";

                    if (chkBiopsy.Checked == true)
                        SQL += ComNum.VBLF + "    'Y', ";
                    else
                        SQL += ComNum.VBLF + "    '' , ";

                    SQL += ComNum.VBLF + "        '" + txtBuse.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        '" + strRemark + "', ";
                    SQL += ComNum.VBLF + "        '" + txtJobName.Text.Trim() + "',";
                    SQL += ComNum.VBLF + "        '" + txtJuso2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        '01', ";
                    SQL += ComNum.VBLF + "        '" + txtBioRemark.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        '" + strBioGb + "', ";
                    SQL += ComNum.VBLF + "        '" + strDept + "', ";
                    SQL += ComNum.VBLF + "        '" + strOpd + "', ";
                    SQL += ComNum.VBLF + "        '" + strHic + "', ";
                    SQL += ComNum.VBLF + "        '" + strMsg + "' ";
                    SQL += ComNum.VBLF + "       )";
                }
                else
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " UPDATE BAS_PATIENT_POSCO ";
                    SQL += ComNum.VBLF + "    SET JDATE     = TO_DATE('" + dtpJdate.Text + "','YYYY-MM-DD') ,";
                    SQL += ComNum.VBLF + "        CDATE     = TO_DATE('" + dtpCdate.Text + "','YYYY-MM-DD') ,";
                    SQL += ComNum.VBLF + "        PANO      = '" + txtPtno.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        SNAME     = '" + txtSname.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        SEX       = '" + VB.Left(cboSex.Text, 1) + "', ";
                    SQL += ComNum.VBLF + "        JUMIN1    = '" + txtJumin1.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        JUMIN3    = '" + clsAES.AES(txtJumin2.Text.Trim()) + "', ";
                    SQL += ComNum.VBLF + "        SINGU     = '" + VB.Left(cboSingu.Text, 1) + "', ";
                    SQL += ComNum.VBLF + "        SABUN     = '" + txtSabun.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        TEL       = '" + txtTel.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        HPHONE    = '" + txtHphone.Text.Trim() + "', ";

                    if (dtpEdate[0].Text != "")
                        SQL += ComNum.VBLF + "    EXAMRES1  = TO_DATE('" + dtpEdate[0].Text + " " + dtpEtime[0].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    EXAMRES1  = '', ";

                    if (dtpEdate[2].Text != "")
                        SQL += ComNum.VBLF + "    EXAMRES2  = TO_DATE('" + dtpEdate[2].Text + " " + dtpEtime[2].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    EXAMRES2  = '', ";

                    if (dtpEdate[3].Text != "")
                        SQL += ComNum.VBLF + "    EXAMRES3  = TO_DATE('" + dtpEdate[3].Text + " " + dtpEtime[3].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    EXAMRES3  = '', ";

                    if (dtpEdate[4].Text != "")
                        SQL += ComNum.VBLF + "    EXAMRES6  = TO_DATE('" + dtpEdate[4].Text + " " + dtpEtime[4].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    EXAMRES6  = '', ";

                    if (dtpEdate[5].Text != "")
                        SQL += ComNum.VBLF + "    EXAMRES7  = TO_DATE('" + dtpEdate[5].Text + " " + dtpEtime[5].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    EXAMRES7  = '', ";

                    if (dtpEdate[1].Text != "")
                        SQL += ComNum.VBLF + "    EXAMRES8  = TO_DATE('" + dtpEdate[1].Text + " " + dtpEtime[1].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    EXAMRES8  = '', ";

                    if (dtpEdate[6].Text != "")
                        SQL += ComNum.VBLF + "    EXAMRES9  = TO_DATE('" + dtpEdate[6].Text + " " + dtpEtime[6].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    EXAMRES9  = '', ";

                    if (dtpEdate[7].Text != "")
                        SQL += ComNum.VBLF + "    EXAMRES10  = TO_DATE('" + dtpEdate[7].Text + " " + dtpEtime[7].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    EXAMRES10  = '', ";

                    if (dtpEdate[8].Text != "")
                        SQL += ComNum.VBLF + "    EXAMRES11  = TO_DATE('" + dtpEdate[8].Text + " " + dtpEtime[8].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    EXAMRES11  = '', ";

                    if (dtpEdate[9].Text != "")
                        SQL += ComNum.VBLF + "    EXAMRES12  = TO_DATE('" + dtpEdate[9].Text + " " + dtpEtime[9].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    EXAMRES12  = '', ";

                    if (dtpEdate[10].Text != "")
                        SQL += ComNum.VBLF + "    EXAMRES13  = TO_DATE('" + dtpEdate[10].Text + " " + dtpEtime[10].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    EXAMRES13  = '', ";

                    if (dtpEdate[11].Text != "")
                        SQL += ComNum.VBLF + "    EXAMRES14  = TO_DATE('" + dtpEdate[11].Text + " " + dtpEtime[11].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    EXAMRES14  = '', ";

                    if (dtpEdate[12].Text != "")
                        SQL += ComNum.VBLF + "    EXAMRES15  = TO_DATE('" + dtpEdate[12].Text + " " + dtpEtime[12].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    EXAMRES15  = '', ";

                    if (dtpEdate[13].Text != "")
                        SQL += ComNum.VBLF + "    EXAMRES16  = TO_DATE('" + dtpEdate[13].Text + " " + dtpEtime[13].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    EXAMRES16  = '', ";

                    if (dtpEdate[14].Text != "")
                        SQL += ComNum.VBLF + "    EXAMRES17  = TO_DATE('" + dtpEdate[14].Text + " " + dtpEtime[14].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    EXAMRES17  = '', ";

                    if (dtpEdate[15].Text != "")
                        SQL += ComNum.VBLF + "    EXAMRES18  = TO_DATE('" + dtpEdate[15].Text + " " + dtpEtime[15].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    EXAMRES18  = '', ";

                    if (dtpEdate[16].Text != "")
                        SQL += ComNum.VBLF + "    EXAMRES19  = TO_DATE('" + dtpEdate[16].Text + " " + dtpEtime[16].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    EXAMRES19  = '', ";

                    if (dtpEdate[17].Text != "")
                        SQL += ComNum.VBLF + "    EXAMRES20  = TO_DATE('" + dtpEdate[17].Text + " " + dtpEtime[17].Text + "','YYYY-MM-DD HH24:MI'), ";
                    else
                        SQL += ComNum.VBLF + "    EXAMRES20  = '', ";

                    SQL += ComNum.VBLF + "        ENTDATE   = SYSDATE, ";
                    SQL += ComNum.VBLF + "        RESULT5   = TO_DATE('" + dtpResult.Text + "','YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "        Remark    = '" + strRemark + "', ";
                    SQL += ComNum.VBLF + "        SendMsg   = '" + strMsg + "', ";

                    if (chkStop.Checked == true)
                        SQL += ComNum.VBLF + "    STOPFLAG  = 'Y', ";
                    else
                        SQL += ComNum.VBLF + "    STOPFLAG  = '',  ";

                    if (chkBiopsy.Checked == true)
                        SQL += ComNum.VBLF + "    EXAM5     = 'Y', ";
                    else
                        SQL += ComNum.VBLF + "    EXAM5     = '',  ";

                    SQL += ComNum.VBLF + "        REMARK_BIOPSY = '" + txtBioRemark.Text.Trim() + "',  ";
                    SQL += ComNum.VBLF + "        BIOPSY_GBN    = '" + strBioGb + "', ";
                    SQL += ComNum.VBLF + "        BUSE          = '" + txtBuse.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        Juso          = '" + txtJuso2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        JOBNAME       = '" + txtJobName.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        DEPT          = '" + strDept + "', ";
                    SQL += ComNum.VBLF + "        OPD           = '" + strOpd + "', ";
                    SQL += ComNum.VBLF + "        HIC           ='" + strHic + "' ";
                    SQL += ComNum.VBLF + "  WHERE ROWID         = '" + FstrRowID + "' ";
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //#region 환자인적사항 변경 내역 백업
                //Dictionary<string, string> dict = new Dictionary<string, string>();
                //#endregion

                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT SET ";

                if (chkRoad.Checked == false)
                {
                    SQL += ComNum.VBLF + "    ZipCode1  = '" + VB.Left(txtPostCode.Text.Trim(), 3) + "', ";
                    SQL += ComNum.VBLF + "    ZipCode2  = '" + VB.Right(txtPostCode.Text.Trim(), 3) + "', ";
                    SQL += ComNum.VBLF + "    Juso      = '" + txtJuso2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "    Road      = 'N', ";
                    SQL += ComNum.VBLF + "    RoadDong  = '', ";

                    //dict.Add("ZIPCODE1", VB.Left(txtPostCode.Text.Trim(), 3));
                    //dict.Add("ZIPCODE2", VB.Right(txtPostCode.Text.Trim(), 3));
                    //dict.Add("JUSO", txtJuso2.Text.Trim());
                }
                else
                {
                    SQL += ComNum.VBLF + "    ZipCode1  = '', ";
                    SQL += ComNum.VBLF + "    ZipCode2  = '', ";
                    SQL += ComNum.VBLF + "    ZipCode3  = '" + txtPostCode.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "    RoadDetail = '" + txtJuso2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "    Juso      = '" + txtJuso1.Text.Trim() + " " + txtJuso2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "    BuildNo   = '" + txtBuildNo.Text.Trim() + "',  ";
                    SQL += ComNum.VBLF + "    Road      = 'Y', ";
                    SQL += ComNum.VBLF + "    RoadDong  = '" + txtRoadDong.Text.Trim() + "', ";

                    //dict.Add("ROADDETAIL", VB.Left(txtPostCode.Text.Trim(), 3));
                    //dict.Add("JUSO", VB.Right(txtPostCode.Text.Trim(), 3));
                    //dict.Add("BUILDNO", txtJuso2.Text.Trim());

                }
                //dict.Add("HPHONE", txtHphone.Text.Trim());
                //CF.INSERT_BAS_PATIENT_HIS(txtPtno.Text.Trim(), dict);

                SQL += ComNum.VBLF + "        Hphone    = '" + txtHphone.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "  WHERE PANO      = '" + txtPtno.Text.Trim() + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }


                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장되었습니다.", "알림");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            strDept = "";
            strRemark = "";

            for (int i = 0; i < 18; i++)
            {
                if (chkOpd[i].Checked == true && cboDept[i].Text.Trim() != "")
                {
                    strDept = cboDept[i].Text.Trim();

                    //당일 이전 내용은 예약안됨. 익일부터 예약수정 가능
                    if (string.Compare(dtpEdate[i].Text, clsPublic.GstrSysDate) > 0)
                    {
                        for (int j = 0; j < 18; j++)
                        {
                            if (strDept == cboDept[j].Text.Trim())
                            {
                                strRemark += lblName[j].Text.Trim() + "/";
                                strRemark += txtMsg[j].Text.Trim() + ",";
                            }
                            //전화예약MASTER
                            CPQ.TELRESV_INSERT(clsDB.DbCon, txtPtno.Text.Trim(), txtSname.Text.Trim(), cboDept[i].Text, dtpEdate[i].Text, dtpEtime[i].Text, VB.Format(i, "00"), FstrGkiho, strRemark);
                        }
                    }
                }
            }

            AUTO_MST_INSERT();
            eFrm_Clear();
            Get_ResList();
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            string value = "";

            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) { return; }     //권한확인

            if (FstrRowID == "") { return; }

            if (clsPmpaPb.InputBox("삭제사유", "삭제사유를 입력바랍니다.:", ref value) == DialogResult.OK)
            {
                if (value == "") { ComFunc.MsgBox("삭제사유가 공란입니다.", "확인"); return; }

                Cursor.Current = Cursors.WaitCursor;

                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO_DTL ";
                    SQL += ComNum.VBLF + "        (JDATE , PANO, sName, ";
                    SQL += ComNum.VBLF + "         Sex, JUMIN1, JUMIN3, ";
                    SQL += ComNum.VBLF + "         SINGU, SABUN, TEL, ";
                    SQL += ComNum.VBLF + "         HPHONE, EXAMRES1, EXAMRES2, ";
                    SQL += ComNum.VBLF + "         EXAMRES3, EXAMRES4, ENTDATE, ";
                    SQL += ComNum.VBLF + "         STOPFLAG, RESULT1, RESULT2, ";
                    SQL += ComNum.VBLF + "         RESULT3, RESULT4, RESULT5, ";
                    SQL += ComNum.VBLF + "         EXAM5, EXAMRES6, RESULT6, ";
                    SQL += ComNum.VBLF + "         EXAMRES7, RESULT7, BUSE, ";
                    SQL += ComNum.VBLF + "         JOBNAME, REMARK, EXAMRES8, ";
                    SQL += ComNum.VBLF + "         JUSO, DELDATE, DELSAYU, ";
                    SQL += ComNum.VBLF + "         GUBUN, REMARK_BIOPSY, BIOPSY_GBN, ";
                    SQL += ComNum.VBLF + "         EXAMRES9, EXAMRES10, EXAMRES11, ";
                    SQL += ComNum.VBLF + "         EXAMRES12, EXAMRES13, EXAMRES14, ";
                    SQL += ComNum.VBLF + "         EXAMRES15, EXAMRES16) ";
                    SQL += ComNum.VBLF + "  SELECT JDATE , PANO, sName, ";
                    SQL += ComNum.VBLF + "         Sex, JUMIN1, JUMIN3, ";
                    SQL += ComNum.VBLF + "         SINGU, SABUN, TEL, ";
                    SQL += ComNum.VBLF + "         HPHONE, EXAMRES1, EXAMRES2, ";
                    SQL += ComNum.VBLF + "         EXAMRES3, EXAMRES4, ENTDATE, ";
                    SQL += ComNum.VBLF + "         STOPFLAG, RESULT1, RESULT2, ";
                    SQL += ComNum.VBLF + "         RESULT3, RESULT4, RESULT5, ";
                    SQL += ComNum.VBLF + "         EXAM5, EXAMRES6, RESULT6, ";
                    SQL += ComNum.VBLF + "         EXAMRES7, RESULT7, BUSE, ";
                    SQL += ComNum.VBLF + "         JOBNAME, REMARK, EXAMRES8, ";
                    SQL += ComNum.VBLF + "         JUSO, TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD'), '" + value + "' DELSAYU, ";
                    SQL += ComNum.VBLF + "         GUBUN, REMARK_BIOPSY, BIOPSY_GBN, ";
                    SQL += ComNum.VBLF + "         EXAMRES9, EXAMRES10, EXAMRES11, ";
                    SQL += ComNum.VBLF + "         EXAMRES12, EXAMRES13, EXAMRES14, ";
                    SQL += ComNum.VBLF + "         EXAMRES15, EXAMRES16 ";
                    SQL += ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
                    SQL += ComNum.VBLF + "    WHERE 1       = 1 ";
                    SQL += ComNum.VBLF + "      AND ROWID   = '" + FstrRowID + "'";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = "";
                    SQL += ComNum.VBLF + " DELETE " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND ROWID     = '" + FstrRowID + "'";
                    SQL += ComNum.VBLF + "    AND GUBUN     ='01' ";  //포스코대상
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }


                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("삭제되었습니다..", "알림");
                    Cursor.Current = Cursors.Default;
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }

            eFrm_Clear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void mnuTelJepsu_Click(object sender, EventArgs e)
        {
            frmSupReturnJepsuView frm = new frmSupReturnJepsuView();
            //frmPmpaViewResJepsuTel frm = new ComPmpaLibB.frmPmpaViewResJepsuTel();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
            OF.fn_ClearMemory(frm);

        }

        private void mnuPrint_Click(object sender, EventArgs e)
        {
            frmPmpaPrintPosco frm = new ComPmpaLibB.frmPmpaPrintPosco();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
            OF.fn_ClearMemory(frm);

        }

        private void mnuSchedule_Click(object sender, EventArgs e)
        {
            frmDoctPlan4 frm = new frmDoctPlan4();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
            OF.fn_ClearMemory(frm);
        }

        private void mnuDeleteList_Click(object sender, EventArgs e)
        {
            frmPmpaViewPoscoReqDel frm = new frmPmpaViewPoscoReqDel();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
            OF.fn_ClearMemory(frm);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            eFrm_Clear();
        }

        private void dtpEdate_DoubleClick(object sender, EventArgs e)
        {
            if (((TextBox)sender).ReadOnly == true) return;

            if (((TextBox)sender).Text != "")
            {
                clsPublic.GstrCalDate = ((TextBox)sender).Text;
            }

            frmCalendar2 frmCalendarX = new frmCalendar2();
            frmCalendarX.StartPosition = FormStartPosition.CenterParent;
            frmCalendarX.ShowDialog();

            ((TextBox)sender).Text = clsPublic.GstrCalDate;
            clsPublic.GstrCalDate = "";
        }

        private void txtTimeKeyPress(object sender, KeyPressEventArgs e)
        {
            // 숫자, 백스페이스, DEL 키를 제외한 입력제한
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == Convert.ToChar(Keys.Delete)))
            {
                e.Handled = true;
            }
        }

        private void txtTime_Leave(object sender, EventArgs e)
        {
            string strValue = ((TextBox)sender).Text.Replace(":", "");
            string strMsg = "입력 데이터를 다시 확인해주세요.";

            if (strValue == "") return;

            if (strValue.Length == 4)
            {
                if (Convert.ToInt32(VB.Left(strValue, 2)) > 24)
                {
                    ComFunc.MsgBox(strMsg);
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Focus();
                    return;
                }
                if (Convert.ToInt32(VB.Right(strValue, 2)) > 60)
                {
                    ComFunc.MsgBox(strMsg);
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Focus();
                    return;
                }

                ((TextBox)sender).Text = VB.Left(strValue, 2) + ":" + VB.Right(strValue, 2);
                CPB.CHECK_TIME(((TextBox)sender));
            }
            else
            {
                ComFunc.MsgBox(strMsg);
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Focus();
            }
        }

        private void mnuSearchBoard_Click(object sender, EventArgs e)
        {            
            Get_ResBoard();
        }

        private void btnExit2_Click(object sender, EventArgs e)
        {
            panNhic.Visible = false;
        }

        private void mnuNhic_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (txtJumin1.Text.Trim() != "" && txtJumin2.Text.Trim() != "")
            {
                Cursor.Current = Cursors.WaitCursor;
                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    SQL = "Delete From WORK_NHIC WHERE Jumin2 = '" + clsAES.AES(txtJumin1.Text + txtJumin2.Text) + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;

                    Hic_Nhic_View();

                    //'위암-위내시경,수면
                    if (dtpEdate3.Text != "") txtRemark2.Text = txtRemark2.Text + " " + clsOiGuide_nhic.THNV.strhCan1;
                    if (dtpEdate4.Text != "") txtRemark3.Text = txtRemark3.Text + " " + clsOiGuide_nhic.THNV.strhCan1;
                    //'간암-복부초음파
                    //if (dtpEdate1.Text != "") txtRemark1.Text = txtRemark1.Text + " " + clsOiGuide_nhic.THNV.strhLiver;
                    if (dtpEdate1.Text != "") txtRemark1.Text = txtRemark1.Text + " " + clsOiGuide_nhic.THNV.strhCan4;
                    
                    //'자궁암-여성자궁검사
                    if (dtpEdate2.Text != "") txtRemark7.Text = txtRemark7.Text + " " + clsOiGuide_nhic.THNV.strhCan5;
                    //'유방암-여성유방검진
                    if (dtpEdate14.Text != "") txtRemark15.Text = txtRemark15.Text + " " + clsOiGuide_nhic.THNV.strhCan2;
                    return;
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(ex.Message);
                    Cursor.Current = Cursors.Default;
                    return;
                }                
            }
        }

        private void Hic_Nhic_View()
        {
            string strTemp = "";
            string strTemp2 = "";
            string strGKiho = "";
            string strAmTemp = "";
            string strNhicChk = "";
            string strYEAR = "";

            string strJong = "";
            string strGubun = "";
            int[] nGbAm = new int[5];
            int i = 0;
            int nRead = 0;
            int nAge = 0;

            

            //txtRowid.Text = "";

            for (i = 0; i < 5; i++)
            {
                nGbAm[i] = 0;
            }

            for (i = 1; i < 31; i++)
            {
                switch (i)
                {
                    case 11:
                    case 21:
                    case 22:
                        continue;
                    default:
                        break;
                }
                ss10_Sheet1.Cells[i, 1].Text = "";
            }
            ss10_Sheet1.Cells[31, 0].Text = "";

            nAge = (int)VB.Val(clsVbfunc.READ_HIC_AGE_GESAN2(VB.Trim(txtJumin1.Text) + VB.Trim(txtJumin2.Text)));
            
            strYEAR = VB.Trim(VB.Left(clsPublic.GstrSysDate, 4));
            strJong = ((nAge == 40) || (nAge == 66) ? "35" : "31");

            strGubun = "N";
            if (strJong == "35")
            {
                strGubun = "Y";
            }

            CHECK_NHIC_HISTORY(strGubun, ref strNhicChk);
            
            //'자격읽기 성공한 자료가 없다면 ...(10.03.16) 자동자격조회함
            if (strNhicChk == "Y")
            {
                //'자동자격조회
                if (txtSname.Text != "")
                {
                    clsPublic.GstrRetValue = txtSname.Text + "^^" + txtJumin1.Text + txtJumin2.Text + "^^" + strJong + "^^" + VB.Trim(txtPtno.Text) + "^^";
                    frmNHicSub frmNHicSubX = new frmNHicSub();   
                    frmNHicSubX.StartPosition = FormStartPosition.CenterParent;
                    frmNHicSubX.ShowDialog();

                    CHECK_NHIC_HISTORY(strGubun, ref strNhicChk);
                    clsPublic.GstrRetValue = "";
                }
            }

            //'자격내역읽기
            clsOiGuide_nhic.HIC_NHIC_READ(ss10);

            panNhic.Visible = true;
        }

        private void CHECK_NHIC_HISTORY(string strGubun, ref string strNhicChk)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                //'최근 검색한 자료가 있으면 다시 검색을 안함
                SQL = "SELECT * FROM ADMIN.WORK_NHIC ";
                SQL = SQL + ComNum.VBLF + " WHERE Jumin2='" + clsAES.AES(VB.Trim(txtJumin1.Text) + VB.Trim(txtJumin2.Text)) + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SNAME='" + VB.Trim(txtSname.Text) + "' ";
                SQL = SQL + ComNum.VBLF + "   AND Gubun='H' "; //'건강검진 대상자
                SQL = SQL + ComNum.VBLF + "   AND TRUNC(CTime) >= TRUNC(SYSDATE-3) ";
                SQL = SQL + ComNum.VBLF + "   AND GBSTS ='1' ";
                if (strGubun == "N")
                {
                    SQL = SQL + ComNum.VBLF + "  AND (GJong IS NULL OR GJong NOT IN ('35','41','42','43','44','45','46' ) )";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND (GJong IS NULL OR GJong IN ('35','41','42','43','44','45','46' ) )";
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY CTIME DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Rel"].ToString().Trim() != "" && dt.Rows[0]["Year"].ToString().Trim() != "")
                    {

                        //'인적정보
                        ss10_Sheet1.Cells[1, 1].Text = txtSname.Text;
                        ss10_Sheet1.Cells[2, 1].Text = txtJumin1.Text + txtJumin2.Text;
                        ss10_Sheet1.Cells[3, 1].Text = dt.Rows[0]["Rel"].ToString().Trim();
                        ss10_Sheet1.Cells[4, 1].Text = dt.Rows[0]["Year"].ToString().Trim();
                        ss10_Sheet1.Cells[5, 1].Text = dt.Rows[0]["Trans"].ToString().Trim();
                        ss10_Sheet1.Cells[6, 1].Text = dt.Rows[0]["GKiho"].ToString().Trim();
                        ss10_Sheet1.Cells[7, 1].Text = dt.Rows[0]["Jisa"].ToString().Trim();
                        ss10_Sheet1.Cells[8, 1].Text = dt.Rows[0]["BDate"].ToString().Replace(".", "-").Trim();
                        ss10_Sheet1.Cells[9, 1].Text = dt.Rows[0]["Cancer53"].ToString().Trim();    //'관할보건소
                        ss10_Sheet1.Cells[10, 1].Text = dt.Rows[0]["Kiho"].ToString().Trim();


                        //'검사정보
                        ss10_Sheet1.Cells[12, 1].Text = dt.Rows[0]["First"].ToString().Trim();
                        ss10_Sheet1.Cells[13, 1].Text = dt.Rows[0]["EKG"].ToString().Trim();
                        ss10_Sheet1.Cells[14, 1].Text = dt.Rows[0]["Liver2"].ToString().Trim();
                        ss10_Sheet1.Cells[15, 1].Text = dt.Rows[0]["Liver"].ToString().Trim();
                        ss10_Sheet1.Cells[16, 1].Text = dt.Rows[0]["Cancer11"].ToString().Trim() + "(" + dt.Rows[0]["Cancer12"].ToString().Trim() + ")";
                        ss10_Sheet1.Cells[17, 1].Text = dt.Rows[0]["Cancer21"].ToString().Trim() + "(" + dt.Rows[0]["Cancer22"].ToString().Trim() + ")";
                        ss10_Sheet1.Cells[18, 1].Text = dt.Rows[0]["Cancer31"].ToString().Trim() + "(" + dt.Rows[0]["Cancer32"].ToString().Trim() + ")";
                        ss10_Sheet1.Cells[19, 1].Text = dt.Rows[0]["Cancer41"].ToString().Trim() + "(" + dt.Rows[0]["Cancer42"].ToString().Trim() + ")";
                        ss10_Sheet1.Cells[20, 1].Text = dt.Rows[0]["Cancer51"].ToString().Trim() + "(" + dt.Rows[0]["Cancer52"].ToString().Trim() + ")";


                        //'수검정보
                        ss10_Sheet1.Cells[23, 1].Text = dt.Rows[0]["GBCHK01"].ToString().Trim() + " " + dt.Rows[0]["GBCHK01_Name"].ToString().Trim() + "";
                        ss10_Sheet1.Cells[24, 1].Text = dt.Rows[0]["GBCHK02"].ToString().Trim() + " " + dt.Rows[0]["GBCHK02_Name"].ToString().Trim() + "";
                        ss10_Sheet1.Cells[25, 1].Text = dt.Rows[0]["GBCHK03"].ToString().Trim() + " " + dt.Rows[0]["GBCHK03_Name"].ToString().Trim() + "";
                        ss10_Sheet1.Cells[26, 1].Text = dt.Rows[0]["GBCHK04"].ToString().Trim() + " " + dt.Rows[0]["GBCHK04_Name"].ToString().Trim() + "";
                        ss10_Sheet1.Cells[27, 1].Text = dt.Rows[0]["GBCHK05"].ToString().Trim() + " " + dt.Rows[0]["GBCHK05_Name"].ToString().Trim() + "";
                        ss10_Sheet1.Cells[28, 1].Text = dt.Rows[0]["GBCHK06"].ToString().Trim() + " " + dt.Rows[0]["GBCHK06_Name"].ToString().Trim() + "";
                        ss10_Sheet1.Cells[29, 1].Text = dt.Rows[0]["GBCHK07"].ToString().Trim() + " " + dt.Rows[0]["GBCHK07_Name"].ToString().Trim() + "";
                        ss10_Sheet1.Cells[30, 1].Text = dt.Rows[0]["GBCHK08"].ToString().Trim() + " " + dt.Rows[0]["GBCHK08_Name"].ToString().Trim() + "";
                    }                    
                }
                else
                {
                    strNhicChk = "Y";
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }            
        }

    }
}
