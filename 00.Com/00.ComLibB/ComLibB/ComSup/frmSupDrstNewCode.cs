using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : SupDrst
    /// File Name       : frmSupDrstNewCode.cs
    /// Description     : 원내약품 수가결정 요청서
    /// Author          : 이정현
    /// Create Date     : 2017-11-08
    /// <history> 
    /// 원내약품 수가결정 요청서
    /// </history>
    /// <seealso>
    /// PSMH\drug\drcode\frmNewCode.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drcode\drcode.vbp
    /// </vbp>
    /// </summary>
    public partial class frmSupDrstNewCode : Form
    {
        #region //ENUM 선언
        enum ColumnCert
        {
            chk,            // 체크
            Contents,       // 내용
            Bun,            // 분류
            Company,        // 제약회사
            EdiCode,        // 표준코드
            SuCode,         // 수가코드
            DosCode,        // 용법(기본검체)
            ODosCode,       // 외래용법
            IDosCode,       // 입원용법
            HName,          // 명칭(한글)
            EName,          // 명칭(영문)
            SungBun,        // 성분 
            JeHyeng,        // 제형분류            
            Unit1,          // 약제용량
            Unit2,          // 용량단위
            Unit4,          // 부피
            Unit3,          // 제형
            MultiContent,   // 복합성분함량
            Cost,           // 약가
            J,              // J항
            F,              // F항
            O,              // O항
            U,              // U항
            Title,          // Title
            FollowUp,       // FollowUp
            ItemCD,         // ItemCD
            OcsSungbun,     // 성분명(SLIP관리)
            TuyakPath,      // 투약경로
            TuyakPath_Etc,  // 투약경로 기타
            SDATE,          // 시행일자
            BIGO,           // 비고
            WRTNO,
            ROWID,
            DUMMY1,
            DUMMY2
        };
        #endregion

        string GstrGrade = "";
        string GstrWRTNO = "";
        string GstrJDATE = "";
        string GstrYDATE = "";
        string GstrSDATE = "";
        string GstrJNAME = "";

        string[] GstrBCode = new string[0];

        public frmSupDrstNewCode()
        {
            InitializeComponent();
            setEvent();
        }

        void frmSupDrstNewCode_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ChkBuse();

            if (GstrGrade == "")
            {
                ComFunc.MsgBox("사용자 정보가 부정확합니다. 종료합니다.");
                this.Close();
                return;
            }

            dtpSDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-7);
            dtpEDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            Init();
            InitControlEnabled();
            GetData();

            setSpread();

            //2014-06-14 김순옥과장,정희정주임 만 허용
            //2019-10-28 13635 추가
            //2020-10-30               이현정수녀님                    정희정팀장                        김해수                           이민주
            if (clsType.User.Sabun == "50773" || clsType.User.Sabun == "15273" || clsType.User.Sabun == "45316" || clsType.User.Sabun == "13635")
            {
                panNewOrderSave.Visible = true;
            }
            else
            {
                //
                this.ssView.Font = new System.Drawing.Font("맑은 고딕", 9F);
                this.ssCert.Font = new System.Drawing.Font("맑은 고딕", 9F);

            }
        }

        void setEvent()
        {
            this.ssCert.KeyDown += new KeyEventHandler(eSpreadKeyDown);
        }

        void setSpread()
        {
            //sheet grid line set : border none  // gridelines   color 211,211,211
            ssCert_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlLightLight, System.Drawing.Color.Black);
            ssCert_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlLightLight, System.Drawing.Color.Black);
        }

        void eSpreadKeyDown(object sender, KeyEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (e.KeyCode == Keys.Enter)
            {
                clsSpread.gSpreadEnter_NextCol(o);
            }

        }

        void ChkBuse()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strBuse = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     BUSE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST ";
                SQL = SQL + ComNum.VBLF + "     WHERE SABUN = '" + ComFunc.LPAD(clsType.User.Sabun, 5, "0") + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strBuse = dt.Rows[0]["BUSE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                switch (strBuse)
                {
                    case "044101":
                    case "044100":
                        GstrGrade = "YAK";
                        lblTitle.Text = "하단부에 약품의 상세내역을 작성해 주시기 바랍니다.";
                        break;
                    //case "077502":
                    case "077501":
                    case "077405":
                    case "078200":
                    case "078201":
                        GstrGrade = "SIM";
                        lblTitle.Text = "본원에서 사용하는 의약품에 대하여 다음과 같이 요청하오니 처리하여 주시기 바랍니다.";
                        break;
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            if (clsType.User.IdNumber == "19423")
            {
                GstrGrade = "SIM";
            }

        }

        void Init()
        {
            ssView_Sheet1.RowCount = 0;
            ssCert_Sheet1.RowCount = 0;

            txtSend.Text = "";
            txtRequest.Text = "";
            GstrWRTNO = "";

            SetComboSpread();
        }

        void SetComboSpread()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     NAME";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'DRUG_수가결정내용' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY NAME";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    GstrBCode = new string[dt.Rows.Count];

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        GstrBCode[i] = dt.Rows[i]["NAME"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void InitControlEnabled()
        {
            switch (GstrGrade)
            {
                case "YAK":
                    btnNew.Enabled = true;
                    btnSelDelete.Enabled = true;
                    btnCert.Enabled = true;
                    btnCertAll.Enabled = false;
                    txtSend.Enabled = true;
                    txtRequest.Enabled = false;
                    btnBCode.Enabled = true;
                    btnYak.Enabled = true;
                    break;
                case "SIM":
                    btnNew.Enabled = false;
                    btnSelDelete.Enabled = false;
                    btnCert.Enabled = false;
                    btnCertAll.Enabled = true;
                    txtSend.Enabled = false;
                    txtRequest.Enabled = true;
                    btnBCode.Enabled = false;
                    btnYak.Enabled = false;
                    break;
            }
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strBun = "";

            Init();

            if (rdoGubun1.Checked == true)
            {
                strBun = "1";
            }
            else if (rdoGubun2.Checked == true)
            {
                strBun = "2";
            }
            else if (rdoGubun3.Checked == true)
            {
                strBun = "3";
            }

            #region Call ReadMst(strBun)

            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     WRTNO, TO_CHAR(SENDDATE, 'YYYY-MM-DD') AS SENDDATE, SENDCONTEXT, SENDSABUN, CERT, ";
                SQL = SQL + ComNum.VBLF + "     REQCONTEXT, TO_CHAR(REQDATE,'YYYY-MM-DD HH24:MI') AS REQDATE, TO_CHAR(SIGNDATE,'YYYY-MM-DD HH24:MI') AS SIGNDATE, SIGNSABUN, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(SENDDATE,'YYYY-MM-DD HH24:MI') AS SENDDATE2 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_JEP_REQ ";

                switch (strBun)
                {
                    case "1":
                        SQL = SQL + ComNum.VBLF + "     WHERE CERT IS NULL";
                        break;
                    case "2":
                        SQL = SQL + ComNum.VBLF + "     WHERE SENDDATE >= TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND SENDDATE <  TO_DATE('" + dtpEDate.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        break;
                    case "3":
                        SQL = SQL + ComNum.VBLF + "     WHERE REQDATE >= TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND REQDATE <= TO_DATE('" + dtpEDate.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        break;
                }

                if (GstrGrade == "SIM")
                {
                    SQL = SQL + ComNum.VBLF + "         AND SIGNSABUN IS NOT NULL ";
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY WRTNO ASC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SENDDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = (dt.Rows[i]["SIGNSABUN"].ToString().Trim() == "" ? "미결재" : "결재완료");
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SENDCONTEXT"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = (dt.Rows[i]["REQDATE"].ToString().Trim() == "" ? "미확인" : "확인완료");
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["REQCONTEXT"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["SIGNDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["REQDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["SENDDATE2"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 10].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["SENDSABUN"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            #endregion
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            //ssPrintOld();
            ssPrintNew();
        }

        private void ssPrintOld()
        {
            string strFont1 = "";
            string strHead1 = "";
            string strFont2 = "";
            string strHead2 = "";

            strFont1 = "/fn\"맑은 고딕\" /fz\"17\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"맑은 고딕\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + "원내약품 수가결정 요청서" + "/f1/n";
            strHead2 = "/l/f2" + "※ 문서번호 : " + VB.Left(GstrJDATE, 4) + "-" + GstrWRTNO + "/f2/n";
            strHead2 = strHead2 + "/l/f2" + VB.Space(11) + "※ 작성일자 : " + GstrJDATE + "/f2/n";
            strHead2 = strHead2 + "/l/f2" + VB.Space(11) + "※ 작 성 자 : " + GstrJNAME + "/f2/n";
            strHead2 = strHead2 + "/l/f2" + VB.Space(11) + "※ 약제팀장확인 : " + GstrYDATE + "/f2/n";
            strHead2 = strHead2 + "/l/f2" + VB.Space(11) + "※ 보험심사팀님확인 : " + GstrSDATE + "/f2/n";

            ssCert_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssCert_Sheet1.PrintInfo.ZoomFactor = 0.5f;
            ssCert_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssCert_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssCert_Sheet1.PrintInfo.Margin.Top = 90;
            ssCert_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssCert_Sheet1.PrintInfo.Margin.Header = 10;
            ssCert_Sheet1.PrintInfo.ShowColor = false;
            ssCert_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssCert_Sheet1.PrintInfo.ShowBorder = true;
            ssCert_Sheet1.PrintInfo.ShowGrid = true;
            ssCert_Sheet1.PrintInfo.ShowShadows = false;
            ssCert_Sheet1.PrintInfo.UseMax = true;
            ssCert_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssCert_Sheet1.PrintInfo.UseSmartPrint = false;
            ssCert_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssCert_Sheet1.PrintInfo.Preview = false;
            ssCert.PrintSheet(0);
        }

        void btnBCode_Click(object sender, EventArgs e)
        {
            frmBCode frm = new frmBCode("DRUG_수가결정내용", "내용을 입력하세요");
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();

            GetData();
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            if (GstrWRTNO == "")
            {
                ComFunc.MsgBox("삭제할 신청서를 선택해주십시오.");
                return;
            }

            if (ComFunc.MsgBoxQ("신청서 삭제를 계속 진행하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "DELETE " + ComNum.DB_ERP + "DRUG_JEP_REQ ";
                SQL = SQL + ComNum.VBLF + "     WHERE WRTNO = " + GstrWRTNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = "DELETE " + ComNum.DB_ERP + "DRUG_JEP_REQ_DETAIL ";
                SQL = SQL + ComNum.VBLF + "     WHERE WRTNO = " + GstrWRTNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                GetData();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void rdoGubun_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                GetData();
            }
        }

        void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

            //border 라인 설정후 
            FarPoint.Win.ComplexBorder complexBorder1 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));

            GstrWRTNO = "";

            if (e.RowHeader == true || e.ColumnHeader == true) { return; }

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            GstrWRTNO = ssView_Sheet1.Cells[e.Row, 6].Text.Trim();

            txtSend.Text = ssView_Sheet1.Cells[e.Row, 3].Text.Trim();
            txtRequest.Text = ssView_Sheet1.Cells[e.Row, 5].Text.Trim();

            GstrYDATE = ssView_Sheet1.Cells[e.Row, 7].Text.Trim();
            GstrSDATE = ssView_Sheet1.Cells[e.Row, 8].Text.Trim();
            GstrJDATE = ssView_Sheet1.Cells[e.Row, 9].Text.Trim();
            GstrJNAME = ssView_Sheet1.Cells[e.Row, 10].Text.Trim();

            if (ssView_Sheet1.Cells[e.Row, 4].Text.Trim() == "확인완료")
            {
                btnCert.Enabled = false;
                btnSelDelete.Enabled = false;
                btnDelete.Enabled = false;
            }
            else
            {
                btnCert.Enabled = true;
                btnSelDelete.Enabled = true;

                if (GstrGrade == "YAK")
                {
                    btnDelete.Enabled = true;
                }
                else
                {
                    btnDelete.Enabled = false;
                }
            }

            #region Call ReadSub(fstrWRTNO)

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            byte[] a = new byte[0];
            int intHeight = 0;

            ssCert_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "select";
                SQL = SQL + ComNum.VBLF + "     WRTNO, CERT, CONTENTS, BUN, ";
                SQL = SQL + ComNum.VBLF + "     LTDNAME, BCODE, JEPCODE, JEPNAME, ";
                SQL = SQL + ComNum.VBLF + "     UNIT1, PRICE, PART_J, PART_F, TUYAKPATH, TUYAKPATH_ETC, ";
                SQL = SQL + ComNum.VBLF + "     PART_O, PART_U, SDATE, BIGO, ROWID, ";
                SQL = SQL + ComNum.VBLF + "     UNIT4, OCS_ITEM1, OCS_ITEM2, OCS_ITEM3, OCS_ITEM4, ODOSCODE, IDOSCODE, ";
                SQL = SQL + ComNum.VBLF + "     DOSCODE, JEPENAME, SUNGBUN, UNIT2, UNIT3, JEHYUNG,SEND, ";
                SQL = SQL + ComNum.VBLF + "     OCS_SUNGBUN, MULTI_CONTENT ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_JEP_REQ_DETAIL";
                SQL = SQL + ComNum.VBLF + "     WHERE WRTNO = " + GstrWRTNO;
                SQL = SQL + ComNum.VBLF + "ORDER BY CONTENTS ASC, JEPNAME ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssCert_Sheet1.RowCount = dt.Rows.Count;
                    ssCert_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    clsSpread.gSpreadComboDataSetEx(ssCert, 0, (int)ColumnCert.Contents, ssCert_Sheet1.RowCount - 1, (int)ColumnCert.Contents, GstrBCode);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        clsSpread.gSpreadComboListFind(ssCert, i, (int)ColumnCert.Contents, 10, dt.Rows[i]["CONTENTS"].ToString().Trim());

                        ssCert_Sheet1.Cells[i, (int)ColumnCert.Bun].Text = dt.Rows[i]["BUN"].ToString().Trim();
                        ssCert_Sheet1.Cells[i, (int)ColumnCert.Company].Text = dt.Rows[i]["LTDNAME"].ToString().Trim();
                        ssCert_Sheet1.Cells[i, (int)ColumnCert.EdiCode].Text = dt.Rows[i]["BCODE"].ToString().Trim();
                        ssCert_Sheet1.Cells[i, (int)ColumnCert.SuCode].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                        ssCert_Sheet1.Cells[i, (int)ColumnCert.DosCode].Text = dt.Rows[i]["DOSCODE"].ToString().Trim();
                        ssCert_Sheet1.Cells[i, (int)ColumnCert.ODosCode].Text = dt.Rows[i]["ODOSCODE"].ToString().Trim();
                        ssCert_Sheet1.Cells[i, (int)ColumnCert.IDosCode].Text = dt.Rows[i]["IDOSCODE"].ToString().Trim();
                        ssCert_Sheet1.Cells[i, (int)ColumnCert.HName].Text = dt.Rows[i]["JEPNAME"].ToString().Trim();
                        ssCert_Sheet1.Cells[i, (int)ColumnCert.EName].Text = dt.Rows[i]["JEPENAME"].ToString().Trim();
                        ssCert_Sheet1.Cells[i, (int)ColumnCert.SungBun].Text = dt.Rows[i]["SUNGBUN"].ToString().Trim();

                        clsSpread.gSpreadComboFind(ssCert, i, (int)ColumnCert.JeHyeng, 5, dt.Rows[i]["JEHYUNG"].ToString().Trim());
                        if (ssCert_Sheet1.Cells[i, (int)ColumnCert.JeHyeng].Text == "-1")
                        {
                            ssCert_Sheet1.Cells[i, (int)ColumnCert.JeHyeng].Text = dt.Rows[i]["JEHYUNG"].ToString().Trim();
                        }
                        
                        ssCert_Sheet1.Cells[i, (int)ColumnCert.Unit1].Text = dt.Rows[i]["UNIT1"].ToString().Trim();

                        clsSpread.gSpreadComboFind(ssCert, i, (int)ColumnCert.Unit2, 3, dt.Rows[i]["UNIT2"].ToString().Trim());
                        if (ssCert_Sheet1.Cells[i, (int)ColumnCert.Unit2].Text == "-1")
                        {
                            ssCert_Sheet1.Cells[i, (int)ColumnCert.Unit2].Text = dt.Rows[i]["UNIT2"].ToString().Trim();
                        }

                        clsSpread.gSpreadComboFind(ssCert, i, (int)ColumnCert.Unit3, 4, dt.Rows[i]["UNIT3"].ToString().Trim());
                        if (ssCert_Sheet1.Cells[i, (int)ColumnCert.Unit3].Text == "-1")
                        {
                            ssCert_Sheet1.Cells[i, (int)ColumnCert.Unit3].Text = dt.Rows[i]["UNIT3"].ToString().Trim();
                        }

                        ssCert_Sheet1.Cells[i, (int)ColumnCert.Unit4].Text = dt.Rows[i]["UNIT4"].ToString().Trim();

                        ssCert_Sheet1.Cells[i, (int)ColumnCert.Cost].Text = dt.Rows[i]["PRICE"].ToString().Trim();
                        ssCert_Sheet1.Cells[i, (int)ColumnCert.J].Text = dt.Rows[i]["PART_J"].ToString().Trim();
                        ssCert_Sheet1.Cells[i, (int)ColumnCert.F].Text = dt.Rows[i]["PART_F"].ToString().Trim();
                        ssCert_Sheet1.Cells[i, (int)ColumnCert.O].Text = dt.Rows[i]["PART_O"].ToString().Trim();
                        ssCert_Sheet1.Cells[i, (int)ColumnCert.U].Text = dt.Rows[i]["PART_U"].ToString().Trim();

                        ssCert_Sheet1.Cells[i, (int)ColumnCert.Title].Text = dt.Rows[i]["OCS_ITEM2"].ToString().Trim();
                        ssCert_Sheet1.Cells[i, (int)ColumnCert.FollowUp].Text = dt.Rows[i]["OCS_ITEM3"].ToString().Trim();
                        ssCert_Sheet1.Cells[i, (int)ColumnCert.ItemCD].Text = dt.Rows[i]["OCS_ITEM4"].ToString().Trim();

                        //2019-08-19 전산업무의뢰서 2019-898
                        ssCert_Sheet1.Cells[i, (int)ColumnCert.TuyakPath].Text = dt.Rows[i]["TUYAKPATH"].ToString().Trim();
                        ssCert_Sheet1.Cells[i, (int)ColumnCert.TuyakPath_Etc].Text = dt.Rows[i]["TUYAKPATH_ETC"].ToString().Trim();

                        //2019-10-14 전산업무의뢰서 2019-839
                        ssCert_Sheet1.Cells[i, (int)ColumnCert.OcsSungbun].Text = dt.Rows[i]["OCS_SUNGBUN"].ToString().Trim();
                        ssCert_Sheet1.Cells[i, (int)ColumnCert.MultiContent].Text = dt.Rows[i]["MULTI_CONTENT"].ToString().Trim();

                        ssCert_Sheet1.Cells[i, (int)ColumnCert.SDATE].Text = dt.Rows[i]["SDATE"].ToString().Trim();
                        ssCert_Sheet1.Cells[i, (int)ColumnCert.BIGO].Text = dt.Rows[i]["BIGO"].ToString().Trim();
                        ssCert_Sheet1.Cells[i, (int)ColumnCert.WRTNO].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                        ssCert_Sheet1.Cells[i, (int)ColumnCert.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssCert_Sheet1.Cells[i, (int)ColumnCert.DUMMY1].Text = "";

                        ssCert_Sheet1.Cells[i, (int)ColumnCert.DUMMY2].Text = dt.Rows[i]["SEND"].ToString().Trim();

                        

                        intHeight = 0;

                        a = Encoding.Default.GetBytes(ssCert_Sheet1.Cells[i, (int)ColumnCert.HName].Text);

                        if (intHeight < Convert.ToInt32(a.Length / 12))
                        {
                            intHeight = Convert.ToInt32(a.Length / 12);
                        }

                        if (ssCert_Sheet1.Rows[i].Height < ComNum.SPDROWHT + (intHeight * 18))
                        {
                            ssCert_Sheet1.SetRowHeight(i, ComNum.SPDROWHT + (intHeight * 18));
                        }

                        a = Encoding.Default.GetBytes(ssCert_Sheet1.Cells[i, (int)ColumnCert.SungBun].Text);

                        if (intHeight < Convert.ToInt32(a.Length / 12))
                        {
                            intHeight = Convert.ToInt32(a.Length / 12);
                        }

                        if (ssCert_Sheet1.Rows[i].Height < ComNum.SPDROWHT + (intHeight * 22))
                        {
                            ssCert_Sheet1.SetRowHeight(i, ComNum.SPDROWHT + (intHeight * 22));
                        }

                        a = Encoding.Default.GetBytes(ssCert_Sheet1.Cells[i, (int)ColumnCert.BIGO].Text);

                        if (intHeight < Convert.ToInt32(a.Length / 10))
                        {
                            intHeight = Convert.ToInt32(a.Length / 10);
                        }

                        if (ssCert_Sheet1.Rows[i].Height < ComNum.SPDROWHT + (intHeight * 22))
                        {
                            ssCert_Sheet1.SetRowHeight(i, ComNum.SPDROWHT + (intHeight * 22));
                        }

                        //2020-01-06 처방명2
                        a = Encoding.Default.GetBytes(ssCert_Sheet1.Cells[i, (int)ColumnCert.OcsSungbun].Text);

                        if (intHeight < Convert.ToInt32(a.Length / 12))
                        {
                            intHeight = Convert.ToInt32(a.Length / 12);
                        }

                        if (ssCert_Sheet1.Rows[i].Height < ComNum.SPDROWHT + (intHeight * 22))
                        {
                            ssCert_Sheet1.SetRowHeight(i, ComNum.SPDROWHT + (intHeight * 22));
                        }

                        ssCert_Sheet1.Columns.Get(0).Border = complexBorder1;
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            #endregion
        }

        void btnNew_Click(object sender, EventArgs e)
        {
            Init();

            btnCert.Enabled = true;
            btnSelDelete.Enabled = true;

            txtSend.Focus();

            ssCert_Sheet1.RowCount = ssCert_Sheet1.RowCount + 1;
            ssCert_Sheet1.SetRowHeight(ssCert_Sheet1.RowCount - 1, ComNum.SPDROWHT);

            clsSpread.gSpreadComboDataSetEx1(ssCert, ssCert_Sheet1.RowCount - 1, (int)ColumnCert.Contents, ssCert_Sheet1.RowCount - 1, (int)ColumnCert.Contents, GstrBCode, true);
        }

        void btnCert_Click(object sender, EventArgs e)
        {
            if (GstrGrade == "SIM") { return; }

            if (SaveData() == true)
            {
                GetData();
            }
        }

        bool SaveData()
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strCONTENTS = "";
            string strBun = "";
            string strLTDNAME = "";
            string strBCode = "";
            string strJEPCODE = "";
            string strJepName = "";
            string strUNIT1 = "";
            string strUNIT2 = "";
            string strUNIT3 = "";
            string strUNIT4 = "";

            string strPART_J = "";
            string strPART_F = "";
            string strPART_O = "";
            string strPART_U = "";
            string strSDate = "";
            string strBigo = "";
            string strPrice = "";
            string strWRTNO = "";
            string strROWID = "";
            string strChange = "";
            string strDOSCODE = "";
            string strODOSCODE = "";
            string strIDOSCODE = "";
            string strJepEName = "";
            string strSUNGBUN = "";
            string strJeHyung = "";

            string strTITLE = "";
            string strFOLLOWUP = "";
            string strITEMCD = "";

            //2019-08-19 전산업무의뢰서 2019-898
            string strTUYAKPATH = "";
            string strTUYAKPATH_ETC = "";

            string strSEND = "";
            string strREQUEST = "";

            //2019-08-19 전산업무의뢰서 2019-837, 839
            string strOCS_SUNGBUN = "";
            string strMULTI_CONTENT = "";

            strSEND = txtSend.Text.Trim();
            strREQUEST = txtRequest.Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (GstrWRTNO == "")
                {
                    GstrWRTNO = ComQuery.GetSequencesNo(clsDB.DbCon, ComNum.DB_ERP.Replace(".", ""), "SEQ_DRUGJEPREQ").ToString();

                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_JEP_REQ";
                    SQL = SQL + ComNum.VBLF + "     (WRTNO, SENDDATE, SENDCONTEXT, SENDSABUN)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         " + GstrWRTNO + ", ";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSEND + "', ";
                    SQL = SQL + ComNum.VBLF + "         " + clsType.User.Sabun;
                    SQL = SQL + ComNum.VBLF + "     )";
                }
                else
                {
                    SQL = "";
                    SQL = "UPDATE " + ComNum.DB_ERP + "DRUG_JEP_REQ";
                    SQL = SQL + ComNum.VBLF + "     SET ";
                    SQL = SQL + ComNum.VBLF + "         SENDCONTEXT = '" + strSEND + "' ";
                    SQL = SQL + ComNum.VBLF + "WHERE WRTNO = " + GstrWRTNO;
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                strWRTNO = GstrWRTNO;

                for (i = 0; i < ssCert_Sheet1.RowCount; i++)
                {
                    strCONTENTS = ssCert_Sheet1.Cells[i, (int)ColumnCert.Contents].Text.Trim();
                    strBun = ssCert_Sheet1.Cells[i, (int)ColumnCert.Bun].Text.Trim();
                    strLTDNAME = ssCert_Sheet1.Cells[i, (int)ColumnCert.Company].Text.Trim();
                    strBCode = ssCert_Sheet1.Cells[i, (int)ColumnCert.EdiCode].Text.Trim();
                    strJEPCODE = ssCert_Sheet1.Cells[i, (int)ColumnCert.SuCode].Text.Trim();
                    strDOSCODE = ssCert_Sheet1.Cells[i, (int)ColumnCert.DosCode].Text.Trim();
                    strODOSCODE = ssCert_Sheet1.Cells[i, (int)ColumnCert.ODosCode].Text.Trim();
                    strIDOSCODE = ssCert_Sheet1.Cells[i, (int)ColumnCert.IDosCode].Text.Trim();
                    strJepName = ssCert_Sheet1.Cells[i, (int)ColumnCert.HName].Text.Trim();
                    strJepEName = ssCert_Sheet1.Cells[i, (int)ColumnCert.EName].Text.Trim();
                    strSUNGBUN = ssCert_Sheet1.Cells[i, (int)ColumnCert.SungBun].Text.Trim();
                    strJeHyung = ssCert_Sheet1.Cells[i, (int)ColumnCert.JeHyeng].Text.Trim();
                    strUNIT1 = ssCert_Sheet1.Cells[i, (int)ColumnCert.Unit1].Text.Trim();
                    strUNIT2 = ssCert_Sheet1.Cells[i, (int)ColumnCert.Unit2].Text.Trim();
                    strUNIT3 = ssCert_Sheet1.Cells[i, (int)ColumnCert.Unit3].Text.Trim();
                    strUNIT4 = ssCert_Sheet1.Cells[i, (int)ColumnCert.Unit4].Text.Trim();
                    strPrice = ssCert_Sheet1.Cells[i, (int)ColumnCert.Cost].Text.Trim();
                    strPART_J = ssCert_Sheet1.Cells[i, (int)ColumnCert.J].Text.Trim();
                    strPART_F = ssCert_Sheet1.Cells[i, (int)ColumnCert.F].Text.Trim();
                    strPART_O = ssCert_Sheet1.Cells[i, (int)ColumnCert.O].Text.Trim();
                    strPART_U = ssCert_Sheet1.Cells[i, (int)ColumnCert.U].Text.Trim();

                    strTITLE = ssCert_Sheet1.Cells[i, (int)ColumnCert.Title].Text.Trim();
                    strFOLLOWUP = ssCert_Sheet1.Cells[i, (int)ColumnCert.FollowUp].Text.Trim();
                    strITEMCD = ssCert_Sheet1.Cells[i, (int)ColumnCert.ItemCD].Text.Trim();

                    //2019-08-19 전산업무의뢰서 2019-898
                    strTUYAKPATH = ssCert_Sheet1.Cells[i, (int)ColumnCert.TuyakPath].Text.Trim();
                    strTUYAKPATH_ETC = ssCert_Sheet1.Cells[i, (int)ColumnCert.TuyakPath_Etc].Text.Trim();

                    strSDate = ssCert_Sheet1.Cells[i, (int)ColumnCert.SDATE].Text.Trim();
                    strBigo = ssCert_Sheet1.Cells[i, (int)ColumnCert.BIGO].Text.Trim();
                    strROWID = ssCert_Sheet1.Cells[i, (int)ColumnCert.ROWID].Text.Trim();
                    strChange = ssCert_Sheet1.Cells[i, (int)ColumnCert.DUMMY1].Text.Trim();
                    
                    //2019-08-19 전산업무의뢰서 2019-837, 839
                    strOCS_SUNGBUN = ssCert_Sheet1.Cells[i, (int)ColumnCert.OcsSungbun].Text.Trim();
                    strMULTI_CONTENT = ssCert_Sheet1.Cells[i, (int)ColumnCert.MultiContent].Text.Trim();

                    //2020-05-20, 복사붙여넣기 했을시 수정플래그에 데이터 안들어가는 문제로 조건 추가함
                    if(strChange == "" && strSDate != "")
                    {
                        strChange = "Y";
                    }

                    if (strChange == "Y")
                    {
                        if (strROWID == "")
                        {
                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_JEP_REQ_DETAIL";
                            SQL = SQL + ComNum.VBLF + "     (WRTNO, Contents, Bun, LTDNAME, BCODE, JepCode, JepName, ";
                            SQL = SQL + ComNum.VBLF + "     UNIT1, PART_J, PART_F, PART_O, PART_U, sDate, BIGO, Price, DOSCODE, ";
                            SQL = SQL + ComNum.VBLF + "     UNIT4, OCS_ITEM1, OCS_ITEM2, OCS_ITEM3, OCS_ITEM4, ODOSCODE, IDOSCODE,";
                            SQL = SQL + ComNum.VBLF + "     TUYAKPATH, TUYAKPATH_ETC, JEPENAME, SUNGBUN, UNIT2, UNIT3, JEHYUNG,";
                            SQL = SQL + ComNum.VBLF + "     OCS_SUNGBUN, MULTI_CONTENT)";
                            SQL = SQL + ComNum.VBLF + "VALUES";
                            SQL = SQL + ComNum.VBLF + "     (";
                            SQL = SQL + ComNum.VBLF + "         " + strWRTNO + ", ";
                            SQL = SQL + ComNum.VBLF + "         '" + strCONTENTS + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strBun + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strLTDNAME + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strBCode + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strJEPCODE + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strJepName + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strUNIT1 + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strPART_J + "',";
                            SQL = SQL + ComNum.VBLF + "         '" + strPART_F + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strPART_O + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strPART_U + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strSDate + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strBigo + "',";
                            SQL = SQL + ComNum.VBLF + "         '" + strPrice + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strDOSCODE + "', ";

                            SQL = SQL + ComNum.VBLF + "         '" + strUNIT4 + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strDOSCODE + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strTITLE + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strFOLLOWUP + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strITEMCD + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strODOSCODE + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strIDOSCODE + "', ";

                            //2019-08-19 전산업무의뢰서 2019-898
                            SQL = SQL + ComNum.VBLF + "         '" + strTUYAKPATH + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strTUYAKPATH_ETC + "', ";

                            SQL = SQL + ComNum.VBLF + "         '" + strJepEName + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strSUNGBUN + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strUNIT2 + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strUNIT3 + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strJeHyung + "', ";

                            //2019-08-19 전산업무의뢰서 2019-837, 839
                            SQL = SQL + ComNum.VBLF + "         '" + strOCS_SUNGBUN + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strMULTI_CONTENT + "' ";
                            SQL = SQL + ComNum.VBLF + "     )";
                        }
                        else
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_ERP + "DRUG_JEP_REQ_DETAIL";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         Contents = '" + strCONTENTS + "',";
                            SQL = SQL + ComNum.VBLF + "         Bun = '" + strBun + "',";
                            SQL = SQL + ComNum.VBLF + "         LTDNAME = '" + strLTDNAME + "',";
                            SQL = SQL + ComNum.VBLF + "         BCODE = '" + strBCode + "',";
                            SQL = SQL + ComNum.VBLF + "         JepCode = '" + strJEPCODE + "',";
                            SQL = SQL + ComNum.VBLF + "         JepName = '" + strJepName + "',";
                            SQL = SQL + ComNum.VBLF + "         UNIT1 = '" + strUNIT1 + "',";
                            SQL = SQL + ComNum.VBLF + "         PART_J = '" + strPART_J + "',";
                            SQL = SQL + ComNum.VBLF + "         PART_F = '" + strPART_F + "',";
                            SQL = SQL + ComNum.VBLF + "         PART_O = '" + strPART_O + "',";
                            SQL = SQL + ComNum.VBLF + "         PART_U = '" + strPART_U + "',";
                            SQL = SQL + ComNum.VBLF + "         JEHYUNG = '" + strJeHyung + "', ";
                            SQL = SQL + ComNum.VBLF + "         sDate = '" + strSDate + "',";
                            SQL = SQL + ComNum.VBLF + "         BIGO = '" + strBigo + "',";
                            SQL = SQL + ComNum.VBLF + "         Price = '" + strPrice + "',";
                            SQL = SQL + ComNum.VBLF + "         DOSCODE = '" + strDOSCODE + "',";

                            SQL = SQL + ComNum.VBLF + "         UNIT4 = '" + strUNIT4 + "', ";
                            SQL = SQL + ComNum.VBLF + "         OCS_ITEM1 = '" + strDOSCODE + "', ";
                            SQL = SQL + ComNum.VBLF + "         OCS_ITEM2 = '" + strTITLE + "', ";
                            SQL = SQL + ComNum.VBLF + "         OCS_ITEM3 = '" + strFOLLOWUP + "', ";
                            SQL = SQL + ComNum.VBLF + "         OCS_ITEM4 = '" + strITEMCD + "', ";
                            SQL = SQL + ComNum.VBLF + "         ODOSCODE = '" + strODOSCODE + "', ";
                            SQL = SQL + ComNum.VBLF + "         IDOSCODE = '" + strIDOSCODE + "', ";

                            //2019-08-19 전산업무의뢰서 2019-898
                            SQL = SQL + ComNum.VBLF + "         TUYAKPATH = '" + strTUYAKPATH + "', ";
                            SQL = SQL + ComNum.VBLF + "         TUYAKPATH_ETC = '" + strTUYAKPATH_ETC + "', ";
                            
                            SQL = SQL + ComNum.VBLF + "         JEPENAME = '" + strJepEName + "',";
                            SQL = SQL + ComNum.VBLF + "         SUNGBUN = '" + strSUNGBUN + "',";
                            SQL = SQL + ComNum.VBLF + "         UNIT2 = '" + strUNIT2 + "', ";
                            SQL = SQL + ComNum.VBLF + "         UNIT3 = '" + strUNIT3 + "', ";

                            //2019-08-19 전산업무의뢰서 2019-837, 839
                            SQL = SQL + ComNum.VBLF + "         OCS_SUNGBUN = '" + strOCS_SUNGBUN + "', ";
                            SQL = SQL + ComNum.VBLF + "         MULTI_CONTENT = '" + strMULTI_CONTENT + "' ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                        }

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        void btnSelDelete_Click(object sender, EventArgs e)
        {
            if (GstrGrade == "SIM") { return; }

            if (DelData() == true)
            {
                GetData();
            }
        }

        bool DelData()
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strROWID = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssCert_Sheet1.RowCount; i++)
                {
                    strROWID = ssCert_Sheet1.Cells[i, (int)ColumnCert.ROWID].Text.Trim();

                    if (Convert.ToBoolean(ssCert_Sheet1.Cells[i, (int)ColumnCert.chk].Value) == true)
                    {
                        SQL = "";
                        SQL = "DELETE " + ComNum.DB_ERP + "DRUG_JEP_REQ_DETAIL";
                        SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + strROWID + "'";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        void btnYak_Click(object sender, EventArgs e)
        {
            if (clsType.User.Sabun != "27111")
            {
                ComFunc.MsgBox("약제팀 승인은 약제팀장님만 가능합니다.");
                return;
            }

            if (ComFunc.MsgBoxQ("해당 요청서를 보험심사팀로 전송합니다.", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                if (YakData() == true)
                {
                    GetData();
                }
            }
        }

        bool YakData()
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string strMsg = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_ERP + "DRUG_JEP_REQ";
                SQL = SQL + ComNum.VBLF + "     SET ";
                SQL = SQL + ComNum.VBLF + "         SIGNSABUN = " + ComFunc.LPAD(clsType.User.Sabun, 5, "0") + ", ";
                SQL = SQL + ComNum.VBLF + "         SIGNDATE = SYSDATE ";
                SQL = SQL + ComNum.VBLF + "WHERE WRTNO = " + GstrWRTNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                strMsg = "[약제팀]수가결정 요청서가 등록이 되었으니 처리하여 주시기 바랍니다.";

                //'2012-03-27 약제팀 승인하면 심사팀장, 정희정 쌤에게 문자 전송
                //2020-05-20 안정수, 심사팀장 제외 
                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "     (SYSDATE, '99999999', '이민주', '010-4523-3885', '32', '', '', SYSDATE, '0542608051', '', ";
                SQL = SQL + ComNum.VBLF + "         '" + strMsg + "') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, DeptCode, DrCode, RTime, RetTel, SendTime, SendMsg)";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "     (SYSDATE, '99999999', '정희정', '010-9376-3233', '32', '', '', SYSDATE, '0542608051', '', ";
                SQL = SQL + ComNum.VBLF + "         '" + strMsg + "') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                } 

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        void btnAdd_Click(object sender, EventArgs e)
        {
            ssCert_Sheet1.RowCount = ssCert_Sheet1.RowCount + 1;
            ssCert_Sheet1.SetRowHeight(ssCert_Sheet1.RowCount - 1, ComNum.SPDROWHT);

            clsSpread.gSpreadComboDataSetEx(ssCert, ssCert_Sheet1.RowCount - 1, (int)ColumnCert.Contents, ssCert_Sheet1.RowCount - 1, (int)ColumnCert.Contents, GstrBCode);
        }

        void ssCert_EditModeOff(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string strJEPCODE = "";
            int intHeight = 0;
            byte[] a;

            ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.DUMMY1].Text = "Y";

            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";

            try
            {
                intHeight = VB.Split(ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, ssCert_Sheet1.ActiveColumnIndex].Text, ComNum.VBLF).Length;

                a = Encoding.Default.GetBytes(ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, ssCert_Sheet1.ActiveColumnIndex].Text);

                if (intHeight < Convert.ToInt32(a.Length / 12))
                {
                    intHeight = Convert.ToInt32(a.Length / 12);
                }

                if (ssCert_Sheet1.Rows[ssCert_Sheet1.ActiveRowIndex].Height < ComNum.SPDROWHT + (intHeight * 22))
                {
                    ssCert_Sheet1.SetRowHeight(ssCert_Sheet1.ActiveRowIndex, ComNum.SPDROWHT + (intHeight * 22));
                }

                if (ssCert_Sheet1.ActiveColumnIndex == (int)ColumnCert.SuCode)
                {
                    ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.SuCode].Text = ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.SuCode].Text.ToUpper().Trim();

                    strJEPCODE = ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.SuCode].Text;

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.BAMT, B.BCODE ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUT a, " + ComNum.DB_PMPA + "BAS_SUN b ";
                    SQL = SQL + ComNum.VBLF + "     WHERE a.SuNext = '" + strJEPCODE + "'  ";
                    SQL = SQL + ComNum.VBLF + "         AND a.SuNext = b.SuNext(+) ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.Cost].Text = dt.Rows[0]["BAMT"].ToString().Trim();
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.EdiCode].Text = dt.Rows[0]["BCODE"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    #region 2021-1110 전산업무 의뢰서 추가
                    ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.BIGO].Text = "";

                    SQL = "";
                    SQL = "SELECT '청구자-' || TRIM(B.DEPTCODE) || '/' || TRIM(B.DRNAME) AS DRNAME ";
                    SQL = SQL + ComNum.VBLF + "  FROM ADMIN.DRUG_MASTER2 A       ";
                    SQL = SQL + ComNum.VBLF + " INNER JOIN ADMIN.OCS_DOCTOR B";
                    SQL = SQL + ComNum.VBLF + "    ON A.CHUNGGUDR = B.DRCODE";
                    SQL = SQL + ComNum.VBLF + " WHERE A.JEPCODE = '" + strJEPCODE + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.BIGO].Text = dt.Rows[0]["DRNAME"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                    #endregion

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     JEYAK, HNAME, ENAME, SNAME, BUNCODE ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUGINFO_NEW ";
                    SQL = SQL + ComNum.VBLF + "     WHERE SUNEXT = '" + strJEPCODE + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.Bun].Text = dt.Rows[0]["BUNCODE"].ToString().Trim();
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.Company].Text = dt.Rows[0]["JEYAK"].ToString().Trim();
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.HName].Text = dt.Rows[0]["HNAME"].ToString().Trim();
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.EName].Text = dt.Rows[0]["ENAME"].ToString().Trim();
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.SungBun].Text = dt.Rows[0]["SNAME"].ToString().Trim();

                        a = Encoding.Default.GetBytes(ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.SungBun].Text);

                        if (intHeight < Convert.ToInt32(a.Length / 12))
                        {
                            intHeight = Convert.ToInt32(a.Length / 12);
                        }

                        if (ssCert_Sheet1.Rows[ssCert_Sheet1.ActiveRowIndex].Height < ComNum.SPDROWHT + (intHeight * 22))
                        {
                            ssCert_Sheet1.SetRowHeight(ssCert_Sheet1.ActiveRowIndex, ComNum.SPDROWHT + (intHeight * 22));
                        }
                    }

                    dt.Dispose();
                    dt = null;




                    SQL = "SELECT ";
                    SQL = SQL + ComNum.VBLF + "      JEPNAMEK, JEPNAMEE, BOKGIBUN, SUIP, ";
                    SQL = SQL + ComNum.VBLF + "      SUGA_ITEM1, SUGA_ITEM2, SUGA_ITEM3, SUGA_ITEM4, SUGA_ITEM5, SUGA_ITEM6, SUGA_ITEM7, SUGA_ITEM8, ";
                    SQL = SQL + ComNum.VBLF + "      OCS_ITEM1, OCS_ITEM2, OCS_ITEM3, OCS_ITEM4, OCS_ITEM5, OCS_ITEM6, BOHUMCODE, ";
                    SQL = SQL + ComNum.VBLF + "      TUYAKPATH_20_1, TUYAKPATH_20_2, TUYAKPATH_20_3, TUYAKPATH_20_4, TUYAKPATH_20_ETC,  ";
                    SQL = SQL + ComNum.VBLF + "      DECODE(SUGABUN, '11', '1.경구약', '20', '2.주사약', '12', '3.외용약') SUGABUN, ";
                    SQL = SQL + ComNum.VBLF + "      (SELECT SUNGBUN ";
                    SQL = SQL + ComNum.VBLF + "         FROM ADMIN.DRUG_MASTER1_SUNGBUN ";
                    SQL = SQL + ComNum.VBLF + "        WHERE JEPCODE = '" + strJEPCODE + "' ";
                    SQL = SQL + ComNum.VBLF + "          AND RANKING = '1' AND ROWNUM = 1) SUNGBUN ";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.DRUG_MASTER1 A ";
                    SQL = SQL + ComNum.VBLF + " INNER JOIN  ADMIN.DRUG_MASTER2 B ";
                    SQL = SQL + ComNum.VBLF + "       ON A.JEPCODE = B.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + "WHERE A.JEPCODE = '" + strJEPCODE + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["BOKGIBUN"].ToString().Trim() != "")
                        {
                            ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.Bun].Text = dt.Rows[0]["BOKGIBUN"].ToString().Trim();
                        }
                        if (dt.Rows[0]["SUIP"].ToString().Trim() != "")
                        {
                            ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.Company].Text = dt.Rows[0]["SUIP"].ToString().Trim();
                        }
                        if (dt.Rows[0]["JEPNAMEK"].ToString().Trim() != "")
                        {
                            ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.HName].Text = dt.Rows[0]["JEPNAMEK"].ToString().Trim();
                        }
                        if (dt.Rows[0]["JEPNAMEE"].ToString().Trim() != "")
                        {
                            ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.EName].Text = dt.Rows[0]["JEPNAMEE"].ToString().Trim();
                        }
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.EdiCode].Text = dt.Rows[0]["BOHUMCODE"].ToString().Trim();
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.DosCode].Text = dt.Rows[0]["OCS_ITEM1"].ToString().Trim(); //용법
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.ODosCode].Text = dt.Rows[0]["OCS_ITEM5"].ToString().Trim(); //용법
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.IDosCode].Text = dt.Rows[0]["OCS_ITEM6"].ToString().Trim(); //용법
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.SungBun].Text = dt.Rows[0]["SUNGBUN"].ToString().Trim();
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.JeHyeng].Text = dt.Rows[0]["SUGABUN"].ToString().Trim();
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.Unit1].Text = dt.Rows[0]["SUGA_ITEM1"].ToString().Trim();
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.Unit2].Text = dt.Rows[0]["SUGA_ITEM2"].ToString().Trim();
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.Unit3].Text = dt.Rows[0]["SUGA_ITEM4"].ToString().Trim();
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.Unit4].Text = dt.Rows[0]["SUGA_ITEM3"].ToString().Trim();
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.J].Text = dt.Rows[0]["SUGA_ITEM5"].ToString().Trim(); //J항
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.F].Text = dt.Rows[0]["SUGA_ITEM6"].ToString().Trim(); //F항
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.O].Text = dt.Rows[0]["SUGA_ITEM7"].ToString().Trim(); //O항
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.U].Text = dt.Rows[0]["SUGA_ITEM8"].ToString().Trim(); //U항
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.Title].Text = dt.Rows[0]["OCS_ITEM2"].ToString().Trim();
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.FollowUp].Text = dt.Rows[0]["OCS_ITEM3"].ToString().Trim();
                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.ItemCD].Text = dt.Rows[0]["OCS_ITEM4"].ToString().Trim();

                        //2019-08-19 전산업무의뢰서 2019-898
                        string strTuyakPath_Temp = "";

                        if (dt.Rows[0]["TUYAKPATH_20_1"].ToString().Trim() == "1")
                        {
                            if (strTuyakPath_Temp != "")
                            {
                                strTuyakPath_Temp = strTuyakPath_Temp + ",";
                            }
                            strTuyakPath_Temp = strTuyakPath_Temp = "IM";
                        }
                        else if (dt.Rows[0]["TUYAKPATH_20_2"].ToString().Trim() == "1")
                        {
                            if (strTuyakPath_Temp != "")
                            {
                                strTuyakPath_Temp = strTuyakPath_Temp + ",";
                            }
                            strTuyakPath_Temp = strTuyakPath_Temp = "IV";
                        }
                        else if (dt.Rows[0]["TUYAKPATH_20_3"].ToString().Trim() == "1")
                        {
                            if (strTuyakPath_Temp != "")
                            {
                                strTuyakPath_Temp = strTuyakPath_Temp + ",";
                            }
                            strTuyakPath_Temp = strTuyakPath_Temp = "IV Infusion";
                        }
                        else if (dt.Rows[0]["TUYAKPATH_20_4"].ToString().Trim() == "1")
                        {
                            if (strTuyakPath_Temp != "")
                            {
                                strTuyakPath_Temp = strTuyakPath_Temp + ",";
                            }
                            strTuyakPath_Temp = strTuyakPath_Temp = "SC";
                        }
                        else if (dt.Rows[0]["TUYAKPATH_20_ETC"].ToString().Trim() != "")
                        {
                            if (strTuyakPath_Temp != "")
                            {
                                strTuyakPath_Temp = strTuyakPath_Temp + ",";
                            }
                            strTuyakPath_Temp = strTuyakPath_Temp = "기타";
                            ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.TuyakPath_Etc].Text = dt.Rows[0]["TUYAKPATH_20_ETC"].ToString().Trim();
                        }

                        ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.TuyakPath].Text = strTuyakPath_Temp;


                        a = Encoding.Default.GetBytes(ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.SungBun].Text);

                        if (intHeight < Convert.ToInt32(a.Length / 12))
                        {
                            intHeight = Convert.ToInt32(a.Length / 12);
                        }

                        if (ssCert_Sheet1.Rows[ssCert_Sheet1.ActiveRowIndex].Height < ComNum.SPDROWHT + (intHeight * 22))
                        {
                            ssCert_Sheet1.SetRowHeight(ssCert_Sheet1.ActiveRowIndex, ComNum.SPDROWHT + (intHeight * 22));
                        }
                    }

                    dt.Dispose();
                    dt = null;



                    // 2019-10-14 멀티성분 함량
                    // 전산업무의뢰서 2019-837
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT B.QTY, B.UNIT FROM ADMIN.DRUG_MASTER2 A ";
                    SQL = SQL + ComNum.VBLF + "INNER JOIN  ADMIN.DRUG_MASTER1_SUNGBUN B       ";
                    SQL = SQL + ComNum.VBLF + "    ON A.JEPCODE = B.JEPCODE                        ";
                    SQL = SQL + ComNum.VBLF + "WHERE A.SUGABUN IN('11', '12')                      ";
                    SQL = SQL + ComNum.VBLF + "  AND A.JEPCODE = '" + strJEPCODE + "'              ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY RANKING                                    ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        string strMultiTmp = "";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            strMultiTmp = strMultiTmp + dt.Rows[i]["QTY"].ToString().Trim() + dt.Rows[i]["UNIT"].ToString().Trim() + "/";
                        }

                        //ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.MultiContent].Text = VB.Mid(strMultiTmp, 0, strMultiTmp.Length - 1);

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT SUNGBUN2                          ";
                        SQL = SQL + ComNum.VBLF + "  FROM ADMIN.DRUG_MASTER1           ";
                        SQL = SQL + ComNum.VBLF + " WHERE JEPCODE = '" + strJEPCODE + "'    ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.MultiContent].Text = strMultiTmp + dt1.Rows[0]["SUNGBUN2"].ToString().Trim();
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }

                    dt.Dispose();
                    dt = null;

                    // 2019-10-14 성분명(SLIP관리)
                    // 전산업무의뢰서 2019-839
                    string strTemp1 = "";
                    string strTemp2 = "";
                    string strTemp3 = "";
                    string strJ = "";
                    string strOrderName = "";
                    string strOrderNameS = "";

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT ORDERNAMES FROM ADMIN.OCS_ORDERCODE ";
                    SQL = SQL + ComNum.VBLF + " WHERE ORDERCODE = '" + strJEPCODE + "'         ";
                    
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strOrderNameS = dt.Rows[0]["ORDERNAMES"].ToString().Trim();
                    }
                    else
                    {
                        strTemp1 = ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.Unit1].Text.Trim();  //규격1(용량)
                        strTemp2 = ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.Unit2].Text.Trim();  //규격2(용량단위)
                        strTemp3 = ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.Unit3].Text.Trim();  //규격3(제형)
                        strJ = ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.J].Text.Trim();  //J항
                        strOrderName = strTemp1 + strTemp2 + "/" + strTemp3;    //오더명칭

                        strTemp1 = ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.EName].Text.Trim();
                        strTemp2 = ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.SungBun].Text.Trim();

                        strOrderNameS = strTemp1 + "(" + strTemp2 + ")";
                        strOrderNameS = strOrderNameS.Replace("\n", " ").Replace("\r", "");

                        //add
                        if (strJ == "1")
                        {
                            strOrderNameS = "(원외)" + strOrderNameS.Trim();
                        }
                        if (strOrderNameS != "")
                        { 
                            string s = string.Empty;
                            s = strOrderNameS.Trim();
                            int intLenTot = (int)ComFunc.GetWordByByte(s);

                            if (intLenTot > 44)
                            {
                                strOrderNameS = ComFunc.GetMidStr(s, 0, 44);
                                strOrderNameS.Trim();
                            }
                        }

                        //전산업무의뢰서 2019-1369
                        if (ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.MultiContent].Text == "")
                        {
                            ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.MultiContent].Text = strOrderName;
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.OcsSungbun].Text = strOrderNameS.Replace("(원외)", "");

                    //2020-01-06 처방명2
                    a = Encoding.Default.GetBytes(ssCert_Sheet1.Cells[ssCert_Sheet1.ActiveRowIndex, (int)ColumnCert.OcsSungbun].Text);

                    if (intHeight < Convert.ToInt32(a.Length / 12))
                    {
                        intHeight = Convert.ToInt32(a.Length / 12);
                    }

                    if (ssCert_Sheet1.Rows[ssCert_Sheet1.ActiveRowIndex].Height < ComNum.SPDROWHT + (intHeight * 22))
                    {
                        ssCert_Sheet1.SetRowHeight(ssCert_Sheet1.ActiveRowIndex, ComNum.SPDROWHT + (intHeight * 22));
                    }
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void btnCertAll_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (ComFunc.MsgBoxQ("해당 요청서를 접수&처리합니다.", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_ERP + "DRUG_JEP_REQ";
                SQL = SQL + ComNum.VBLF + "     SET ";
                SQL = SQL + ComNum.VBLF + "         CERT = '1', ";
                SQL = SQL + ComNum.VBLF + "         REQCONTEXT = '" + txtRequest.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         REQDATE = SYSDATE ";
                SQL = SQL + ComNum.VBLF + "WHERE WRTNO = " + GstrWRTNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                GetData();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void btnNewOrderSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;
            int i = 0;
            int intCnt = 0;

            string strNew = "";
            string strROWID = "";
            string strSuga = "";
            string strBun = "";     //분류
            string strBun2 = "";    //수가분류
            string strOrderName = "";       //오더명
            string strOrderNameS = "";      //약명칭
            string strSname = "";   //성분명
            string strTITLE = "";   //타이틀
            string strDOSCODE = ""; //용법
            double dblSeqNo = 0;    //SEQNO
            string strSlipNo = "";
            string strDisRGB = "";
            string strJ = "";//j항이 1일 경우 (외래) 붙이기       '2018-01-25 >>2019-01-29 추가
            string strTemp1 = "";
            string strTemp2 = "";
            string strTemp3 = "";


            string strODOSCODE = "";
            string strIDOSCODE = "";
            string strFOLLOWUP = "";
            string strITEMCD = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssCert_Sheet1.RowCount; i++)
                {
                    strSuga = ssCert_Sheet1.Cells[i, (int)ColumnCert.SuCode].Text.Trim();

                    if (Convert.ToBoolean(ssCert_Sheet1.Cells[i, (int)ColumnCert.chk].Value) == true)
                    {
                        intCnt++;

                        if (ssCert_Sheet1.Cells[i, (int)ColumnCert.DUMMY2].Text.Trim() == "Y")
                        {
                            ComFunc.MsgBox(strSuga + "는 이미 오더판넬에 등록되었습니다!!"
                                            + ComNum.VBLF + "제외후 작업을 하세요!!");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }

                        //기존오더체크
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     SuNext";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN ";
                        SQL = SQL + ComNum.VBLF + "     WHERE SUNEXT ='" + strSuga + "'";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt.Rows.Count == 0)
                        {
                            ComFunc.MsgBox(strSuga + "는 수가코드 등록이 되어 있지 않습니다..!!"
                                            + ComNum.VBLF + "수가등록작업후 하세요!!");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            dt.Dispose();
                            dt = null;
                            return;
                        }

                        dt.Dispose();
                        dt = null;

                        //기존오더체크
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     SLIPNO";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ORDERCODE";
                        SQL = SQL + ComNum.VBLF + "     WHERE ORDERCODE ='" + strSuga + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            ComFunc.MsgBox(strSuga + "는 이미 오더판넬에 등록되었습니다!!"
                                            + ComNum.VBLF + "제외후 작업을 하세요!!");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            dt.Dispose();
                            dt = null;
                            return;
                        }

                        dt.Dispose();
                        dt = null;
                    }
                }

                if (intCnt > 0)
                {
                    if (ComFunc.MsgBoxQ(intCnt + "개의 선택한 신규약제를 오더판넬에 등재 하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                    {
                        for (i = 0; i < ssCert_Sheet1.RowCount; i++)
                        {
                            if (Convert.ToBoolean(ssCert_Sheet1.Cells[i, (int)ColumnCert.chk].Value) == true)
                            {
                                strROWID = ssCert_Sheet1.Cells[i, (int)ColumnCert.ROWID].Text.Trim();
                                strNew = ssCert_Sheet1.Cells[i, (int)ColumnCert.DUMMY2].Text.Trim();

                                strBun = VB.Left(ssCert_Sheet1.Cells[i, (int)ColumnCert.JeHyeng].Text.Trim(), 1);
                                strTemp1 = ssCert_Sheet1.Cells[i, (int)ColumnCert.Unit1].Text.Trim();  //규격1(용량)
                                strTemp2 = ssCert_Sheet1.Cells[i, (int)ColumnCert.Unit2].Text.Trim();  //규격2(용량단위)
                                strTemp3 = ssCert_Sheet1.Cells[i, (int)ColumnCert.Unit3].Text.Trim();  //규격3(제형)

                                strJ = ssCert_Sheet1.Cells[i, (int)ColumnCert.J].Text.Trim();  //J항

                                strOrderName = strTemp1 + strTemp2 + "/" + strTemp3;    //오더명칭

                                //전산업무의뢰서 2019-837 복합성분 용량
                                if (ssCert_Sheet1.Cells[i, (int)ColumnCert.MultiContent].Text.Trim() != "")
                                {
                                    strOrderName = ssCert_Sheet1.Cells[i, (int)ColumnCert.MultiContent].Text.Trim();    //오더명칭
                                }

                                strTemp1 = ssCert_Sheet1.Cells[i, (int)ColumnCert.EName].Text.Trim();
                                strTemp2 = ssCert_Sheet1.Cells[i, (int)ColumnCert.SungBun].Text.Trim();

                                strOrderNameS = strTemp1 + "(" + strTemp2 + ")";
                                strOrderNameS = strOrderNameS.Replace("\n", " ").Replace("\r", "");

                                //add
                                if (strJ == "1")
                                {
                                    strOrderNameS = "(원외)" + strOrderNameS.Trim();
                                }
                                if (strOrderNameS != "")
                                {
                                    string s = string.Empty;
                                    s = strOrderNameS.Trim();
                                    int intLenTot = (int)ComFunc.GetWordByByte(s);

                                    if (intLenTot > 44)
                                    {
                                        strOrderNameS = ComFunc.GetMidStr(s, 0, 44);
                                        strOrderNameS.Trim();
                                    }
                                }
                                //

                                //2019-11-08 성분명(SLIP관리) 전산업무의뢰서 2019-839
                                if (ssCert_Sheet1.Cells[i, (int)ColumnCert.OcsSungbun].Text.Trim() != "")
                                {
                                    strOrderNameS = ssCert_Sheet1.Cells[i, (int)ColumnCert.OcsSungbun].Text.Trim();
                                    
                                    if (strJ == "1")
                                    {
                                        strOrderNameS = "(원외)" + strOrderNameS.Trim();
                                    }

                                    if (strOrderNameS != "")
                                    {
                                        string s = string.Empty;
                                        s = strOrderNameS.Trim();
                                        int intLenTot = (int)ComFunc.GetWordByByte(s);

                                        if (intLenTot > 44)
                                        {
                                            strOrderNameS = ComFunc.GetMidStr(s, 0, 44);
                                            strOrderNameS.Trim();
                                        }
                                    }
                                }

                                strSname = ssCert_Sheet1.Cells[i, (int)ColumnCert.SungBun].Text.Trim();   //성분명
                                strTITLE = ssCert_Sheet1.Cells[i, (int)ColumnCert.Title].Text.Trim();  //타이틀

                                strSuga = ssCert_Sheet1.Cells[i, (int)ColumnCert.SuCode].Text.Trim();    //약코드
                                strDOSCODE = ssCert_Sheet1.Cells[i, (int)ColumnCert.DosCode].Text.Trim(); //용법

                                strODOSCODE = ssCert_Sheet1.Cells[i, (int)ColumnCert.ODosCode].Text.Trim(); //외래용법
                                strIDOSCODE = ssCert_Sheet1.Cells[i, (int)ColumnCert.IDosCode].Text.Trim(); //입원용법

                                strFOLLOWUP = ssCert_Sheet1.Cells[i, (int)ColumnCert.FollowUp].Text.Trim(); //FollowUp
                                strITEMCD = ssCert_Sheet1.Cells[i, (int)ColumnCert.ItemCD].Text.Trim(); //ItemCD

                                strSlipNo = "0000";
                                strDisRGB = "800000";
                                strBun2 = "";

                                switch (strBun)
                                {
                                    case "1":
                                        strSlipNo = "0003";  //경구
                                        strDisRGB = "800000";
                                        strBun2 = "11";
                                        break;
                                    case "2":
                                        strSlipNo = "0005"; //주사
                                        strDisRGB = "8000";
                                        strBun2 = "20";
                                        break;
                                    case "3":
                                        strSlipNo = "0004"; //외용
                                        strDisRGB = "800000";
                                        strBun2 = "12";
                                        break;
                                }

                                dblSeqNo = 0;

                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     (MAX(SEQNO) + 1) AS SEQNO";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ORDERCODE";
                                SQL = SQL + ComNum.VBLF + "     WHERE SLIPNO ='" + strSlipNo + "' ";

                                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }
                                if (dt.Rows.Count > 0)
                                {
                                    dblSeqNo = VB.Val(dt.Rows[0]["SEQNO"].ToString().Trim());
                                }

                                dt.Dispose();
                                dt = null;

                                if (dblSeqNo > 0)
                                {
                                    SQL = "";
                                    SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_ORDERCODE ";
                                    SQL = SQL + ComNum.VBLF + "     (Slipno, Seqno, OrderCode, OrderName, Qty, Nal, DispHeader, DispSpace,  ";
                                    SQL = SQL + ComNum.VBLF + "     DispRGB, GbBoth, GbInfo, GbInput, GbQty, GbDosage, SpecCode, SuCode, ";
                                    SQL = SQL + ComNum.VBLF + "     Bun, GbIMIV, NextCode, SendDept, OrderNameS, ItemCD, SubRate, GbSub, GbGume, ODosCode, IDosCode)";
                                    SQL = SQL + ComNum.VBLF + "VALUES";
                                    SQL = SQL + ComNum.VBLF + "     (";
                                    SQL = SQL + ComNum.VBLF + "         '" + strSlipNo + "', ";
                                    SQL = SQL + ComNum.VBLF + "         " + dblSeqNo + ", ";
                                    SQL = SQL + ComNum.VBLF + "         '" + strSuga + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + strOrderName + "', ";
                                    SQL = SQL + ComNum.VBLF + "         1, 1, ";
                                    SQL = SQL + ComNum.VBLF + "         '" + strTITLE + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '0', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + strDisRGB + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '0', '0', '1', ";
                                    SQL = SQL + ComNum.VBLF + "         '1', '1', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + strDOSCODE + "', ";    //기본검체
                                    SQL = SQL + ComNum.VBLF + "         '" + strSuga + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + strBun2 + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '0', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + strFOLLOWUP + "', ";  // FollowUp
                                    SQL = SQL + ComNum.VBLF + "         '', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + strOrderNameS + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + strITEMCD + "', ";  // ITEMCD
                                    SQL = SQL + ComNum.VBLF + "         '', '0', '0', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + strODOSCODE + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + strIDOSCODE + "' ";
                                    SQL = SQL + ComNum.VBLF + "     )";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                }

                                ssCert_Sheet1.Cells[i, (int)ColumnCert.chk].Value = false;
                            }
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("작업이 완료 되었습니다..오더판넬을 조회하여 확인하세요!!");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog mDlg = new SaveFileDialog();
            mDlg.InitialDirectory = Application.StartupPath;
            mDlg.Filter = "Excel files (*.xls)|*.xls|Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            mDlg.FilterIndex = 1;

            if (mDlg.ShowDialog() == DialogResult.OK)
            {
                ssCert.SaveExcel(mDlg.FileName,
                FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
            }
        }

        private void ssPrintNew()
        {
            string strFont1 = "";
            string strHead1 = "";
            string strFont2 = "";
            string strHead2 = "";

            int intHeight = 0;
            byte[] a;

            strFont1 = "/fn\"맑은 고딕\" /fz\"17\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"맑은 고딕\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + "원내약품 수가결정 요청서" + "/f1/n";
            strHead2 = "/l/f2" + "※ 문서번호 : " + VB.Left(GstrJDATE, 4) + "-" + GstrWRTNO + "/f2/n";
            strHead2 = strHead2 + "/l/f2" + VB.Space(11) + "※ 작성일자 : " + GstrJDATE + "/f2/n";
            strHead2 = strHead2 + "/l/f2" + VB.Space(11) + "※ 작 성 자 : " + GstrJNAME + "/f2/n";
            strHead2 = strHead2 + "/l/f2" + VB.Space(11) + "※ 약제팀장확인 : " + GstrYDATE + "/f2/n";
            strHead2 = strHead2 + "/l/f2" + VB.Space(11) + "※ 보험심사팀님확인 : " + GstrSDATE + "/f2/n";

            
            ssPrint_Sheet1.RowCount = 0;

            if (ssCert_Sheet1.RowCount == 0) return;

            for (int i = 0; i < ssCert_Sheet1.RowCount; i++)
            {
                ssPrintAddRow();

                int iRow1 = ssPrint_Sheet1.RowCount - 3;
                int iRow2 = ssPrint_Sheet1.RowCount - 1;

                ssPrint_Sheet1.Cells[iRow1, 0].Text = (i + 1).ToString();
                ssPrint_Sheet1.Cells[iRow1, 1].Text = ssCert_Sheet1.Cells[i, 1].Text;       //내용
                ssPrint_Sheet1.Cells[iRow1, 2].Text = ssCert_Sheet1.Cells[i, 2].Text;       //분류
                ssPrint_Sheet1.Cells[iRow1, 3].Text = ssCert_Sheet1.Cells[i, 3].Text;       //제약회사
                ssPrint_Sheet1.Cells[iRow1, 4].Text = ssCert_Sheet1.Cells[i, 4].Text;       //표준코드
                ssPrint_Sheet1.Cells[iRow1, 5].Text = ssCert_Sheet1.Cells[i, 5].Text;       //약코드
                ssPrint_Sheet1.Cells[iRow1, 6].Text = ssCert_Sheet1.Cells[i, 6].Text;       //기본용법
                ssPrint_Sheet1.Cells[iRow1, 7].Text = ssCert_Sheet1.Cells[i, 7].Text;       //외래용법
                ssPrint_Sheet1.Cells[iRow1, 8].Text = ssCert_Sheet1.Cells[i, 8].Text;       //입원용법
                ssPrint_Sheet1.Cells[iRow1, 9].Text = ssCert_Sheet1.Cells[i, 9].Text;       //약품명 (한글)
                ssPrint_Sheet1.Cells[iRow1, 10].Text = ssCert_Sheet1.Cells[i, 10].Text;       //약품명 (영문)
                ssPrint_Sheet1.Cells[iRow1, 11].Text = ssCert_Sheet1.Cells[i, 11].Text;       //성분명
                ssPrint_Sheet1.Cells[iRow1, 12].Text = ssCert_Sheet1.Cells[i, 12].Text;       //제형
                ssPrint_Sheet1.Cells[iRow1, 13].Text = ssCert_Sheet1.Cells[i, 13].Text;       //약제용량
                ssPrint_Sheet1.Cells[iRow1, 14].Text = ssCert_Sheet1.Cells[i, 14].Text;       //용량단위
                ssPrint_Sheet1.Cells[iRow1, 15].Text = ssCert_Sheet1.Cells[i, 15].Text;       //부피(ml)
                
                ssPrint_Sheet1.Cells[iRow2, 1].Text = ssCert_Sheet1.Cells[i, 16].Text;       //제형
                ssPrint_Sheet1.Cells[iRow2, 2].Text = ssCert_Sheet1.Cells[i, 17].Text;       //복합성분함량
                ssPrint_Sheet1.Cells[iRow2, 3].Text = ssCert_Sheet1.Cells[i, 18].Text;       //약가
                ssPrint_Sheet1.Cells[iRow2, 4].Text = ssCert_Sheet1.Cells[i, 19].Text;       //J항
                ssPrint_Sheet1.Cells[iRow2, 5].Text = ssCert_Sheet1.Cells[i, 20].Text;       //F항
                ssPrint_Sheet1.Cells[iRow2, 6].Text = ssCert_Sheet1.Cells[i, 21].Text;       //O항
                ssPrint_Sheet1.Cells[iRow2, 7].Text = ssCert_Sheet1.Cells[i, 22].Text;       //U항
                ssPrint_Sheet1.Cells[iRow2, 8].Text = ssCert_Sheet1.Cells[i, 23].Text;       //일용량 단위
                ssPrint_Sheet1.Cells[iRow2, 9].Text = ssCert_Sheet1.Cells[i, 24].Text;       //일용량
                ssPrint_Sheet1.Cells[iRow2, 10].Text = ssCert_Sheet1.Cells[i, 25].Text;       //ITEMCD
                ssPrint_Sheet1.Cells[iRow2, 11].Text = ssCert_Sheet1.Cells[i, 26].Text;       //성분명(SLIP관리)
                ssPrint_Sheet1.Cells[iRow2, 12].Text = ssCert_Sheet1.Cells[i, 27].Text;       //투약경로
                ssPrint_Sheet1.Cells[iRow2, 13].Text = ssCert_Sheet1.Cells[i, 28].Text;       //기타투약경로
                ssPrint_Sheet1.Cells[iRow2, 14].Text = ssCert_Sheet1.Cells[i, 29].Text;       //시행일
                ssPrint_Sheet1.Cells[iRow2, 15].Text = ssCert_Sheet1.Cells[i, 30].Text;       //기타사항

                intHeight = 0;
                for (int j = 1; j <= 28; j++)
                {
                    a = Encoding.Default.GetBytes(ssPrint_Sheet1.Cells[iRow1, (int)ColumnCert.SungBun].Text);

                    if (intHeight < Convert.ToInt32(a.Length / 8))
                    {
                        intHeight = Convert.ToInt32(a.Length / 8);
                    }

                    if (ssPrint_Sheet1.Rows[iRow1].Height < ComNum.SPDROWHT + (intHeight * 22))
                    {
                        ssPrint_Sheet1.SetRowHeight(iRow1, ComNum.SPDROWHT + (intHeight * 22));
                    }
                }

                intHeight = 0;
                for (int j = 1; j <= 28; j++)
                {
                    a = Encoding.Default.GetBytes(ssPrint_Sheet1.Cells[iRow2, (int)ColumnCert.SungBun].Text);

                    if (intHeight < Convert.ToInt32(a.Length / 8))
                    {
                        intHeight = Convert.ToInt32(a.Length / 8);
                    }

                    if (ssPrint_Sheet1.Rows[iRow2].Height < ComNum.SPDROWHT + (intHeight * 22))
                    {
                        ssPrint_Sheet1.SetRowHeight(iRow2, ComNum.SPDROWHT + (intHeight * 22));
                    }
                }
            }
            
            
            ssPrint_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssPrint_Sheet1.PrintInfo.ZoomFactor = 0.78f;
            ssPrint_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssPrint_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssPrint_Sheet1.PrintInfo.Margin.Top = 90;
            ssPrint_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssPrint_Sheet1.PrintInfo.Margin.Header = 10;
            ssPrint_Sheet1.PrintInfo.ShowColor = false;
            ssPrint_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssPrint_Sheet1.PrintInfo.ShowBorder = true;
            ssPrint_Sheet1.PrintInfo.ShowGrid = true;
            ssPrint_Sheet1.PrintInfo.ShowShadows = false;
            ssPrint_Sheet1.PrintInfo.UseMax = true;
            ssPrint_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssPrint_Sheet1.PrintInfo.UseSmartPrint = false;
            ssPrint_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssPrint_Sheet1.PrintInfo.Preview = false;
            ssPrint.PrintSheet(0);
        }

        private void ssPrintAddRow()
        {
            #region // 스프레드 디자인 객체생성

            FarPoint.Win.ComplexBorder complexBorder1 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder2 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder3 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder4 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder5 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder6 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder7 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder8 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder9 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder complexBorder10 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder11 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder12 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder13 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder14 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder15 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder16 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType15 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder17 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThickLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType16 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder18 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType17 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder19 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType18 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder20 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType19 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder21 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType20 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder22 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType21 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder23 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType22 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder24 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType23 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder25 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType24 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder26 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType25 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder27 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType26 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder28 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType27 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder29 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType28 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder30 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType29 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder31 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType30 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder32 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder complexBorder33 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType31 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder34 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType32 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder35 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType33 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder36 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType34 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder37 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType35 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder38 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType36 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder39 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType37 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder40 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType38 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder41 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType39 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder42 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType40 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder43 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType41 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder44 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType42 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder45 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType43 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder46 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType44 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder47 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType45 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder48 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType46 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder49 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThickLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType47 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder50 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThickLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType48 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder51 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThickLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType49 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder52 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThickLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType50 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder53 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThickLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType51 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder54 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThickLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType52 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder55 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThickLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType53 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder56 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThickLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType54 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder57 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThickLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType55 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder58 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThickLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType56 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder59 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThickLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType57 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder60 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThickLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType58 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder61 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThickLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType59 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder62 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThickLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType60 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder63 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThickLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType61 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder64 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThickLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType62 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType63 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType64 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType65 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType66 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType67 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType68 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType69 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType70 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType71 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType72 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType73 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType74 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType75 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType76 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType77 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color384636457549440364391", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Static484636457549440634407", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType78 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static683636457549440684409");
            FarPoint.Win.Spread.CellType.TextCellType textCellType79 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.Spread.CellType.TextCellType textCellType80 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType81 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType82 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType83 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType84 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType85 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("BorderEx488636457553693987684", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder65 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Text618636457553694057688", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder66 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType86 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle6 = new FarPoint.Win.Spread.NamedStyle("CheckBox798636457553694067689");
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType1 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle7 = new FarPoint.Win.Spread.NamedStyle("ComboEx852636457553694087690");
            FarPoint.Win.Spread.CellType.ComboBoxCellType comboBoxCellType1 = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle8 = new FarPoint.Win.Spread.NamedStyle("Text940636457553694107691");
            FarPoint.Win.Spread.CellType.TextCellType textCellType87 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle9 = new FarPoint.Win.Spread.NamedStyle("Text1040636457553694127692");
            FarPoint.Win.Spread.CellType.TextCellType textCellType88 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle10 = new FarPoint.Win.Spread.NamedStyle("Text1095636457553694137693");
            FarPoint.Win.Spread.CellType.TextCellType textCellType89 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle11 = new FarPoint.Win.Spread.NamedStyle("Text1139636457553694147694");
            FarPoint.Win.Spread.CellType.TextCellType textCellType90 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle12 = new FarPoint.Win.Spread.NamedStyle("ComboEx1296636457553694177695");
            FarPoint.Win.Spread.CellType.ComboBoxCellType comboBoxCellType2 = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle13 = new FarPoint.Win.Spread.NamedStyle("ComboEx1447636457553694197696");
            FarPoint.Win.Spread.CellType.ComboBoxCellType comboBoxCellType3 = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle14 = new FarPoint.Win.Spread.NamedStyle("ComboEx1578636457553694217698");
            FarPoint.Win.Spread.CellType.ComboBoxCellType comboBoxCellType4 = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle15 = new FarPoint.Win.Spread.NamedStyle("Text1648636457553694237699");
            FarPoint.Win.Spread.CellType.TextCellType textCellType91 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle16 = new FarPoint.Win.Spread.NamedStyle("Static2060636457553694307703");
            FarPoint.Win.Spread.CellType.TextCellType textCellType92 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance2 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType2 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.ComboBoxCellType comboBoxCellType5 = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType93 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType94 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType95 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType96 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType97 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType98 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType99 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType100 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType101 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType102 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.ComboBoxCellType comboBoxCellType6 = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType103 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.ComboBoxCellType comboBoxCellType7 = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            FarPoint.Win.Spread.CellType.ComboBoxCellType comboBoxCellType8 = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType104 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType105 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType106 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType107 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType108 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType109 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType110 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.ComboBoxCellType comboBoxCellType9 = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType111 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType112 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType113 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType114 = new FarPoint.Win.Spread.CellType.TextCellType();


            //FarPoint.Win.ComplexBorder complexBorder1 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder2 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder3 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder4 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder5 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder6 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder7 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder8 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder9 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.ComplexBorder complexBorder10 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder11 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder12 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder13 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder14 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder15 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder16 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType15 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder17 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType16 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder18 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType17 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder19 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType18 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder20 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType19 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder21 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType20 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder22 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType21 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder23 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType22 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder24 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType23 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder25 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType24 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder26 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType25 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder27 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType26 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder28 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType27 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder29 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType28 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder30 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType29 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder31 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType30 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder32 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType31 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder33 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType32 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder34 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType33 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder35 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType34 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder36 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType35 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder37 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType36 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder38 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType37 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder39 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType38 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder40 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType39 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder41 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType40 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder42 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType41 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder43 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType42 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder44 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType43 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder45 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType44 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder46 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType45 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder47 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType46 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder48 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType47 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder49 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType48 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder50 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType49 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder51 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType50 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder52 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType51 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder53 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType52 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder54 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType53 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder55 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType54 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder56 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType55 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder57 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType56 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder58 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType57 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder59 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType58 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.ComplexBorder complexBorder60 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            //FarPoint.Win.Spread.CellType.TextCellType textCellType59 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.Spread.CellType.TextCellType textCellType60 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.Spread.CellType.TextCellType textCellType61 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.Spread.CellType.TextCellType textCellType62 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.Spread.CellType.TextCellType textCellType63 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.Spread.CellType.TextCellType textCellType64 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.Spread.CellType.TextCellType textCellType65 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.Spread.CellType.TextCellType textCellType66 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.Spread.CellType.TextCellType textCellType67 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.Spread.CellType.TextCellType textCellType68 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.Spread.CellType.TextCellType textCellType69 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.Spread.CellType.TextCellType textCellType70 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.Spread.CellType.TextCellType textCellType71 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.Spread.CellType.TextCellType textCellType72 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.Spread.CellType.TextCellType textCellType73 = new FarPoint.Win.Spread.CellType.TextCellType();
            //FarPoint.Win.Spread.CellType.TextCellType textCellType74 = new FarPoint.Win.Spread.CellType.TextCellType();
            #endregion


            int iRow1 = 0;
            int iRow2 = 0;

            ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 4;
            iRow1 = ssPrint_Sheet1.RowCount - 4;
            iRow2 = ssPrint_Sheet1.RowCount - 2;

            this.ssPrint_Sheet1.Cells.Get(iRow1, 0).Border = complexBorder1;
            textCellType1.Multiline = true;
            textCellType1.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 0).CellType = textCellType1;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 0).Value = "번호";
            this.ssPrint_Sheet1.Cells.Get(iRow1, 1).Border = complexBorder2;
            textCellType2.Multiline = true;
            textCellType2.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 1).CellType = textCellType2;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 1).Value = "내용";
            this.ssPrint_Sheet1.Cells.Get(iRow1, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 2).Border = complexBorder3;
            textCellType3.Multiline = true;
            textCellType3.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 2).CellType = textCellType3;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 2).Value = "분류";
            this.ssPrint_Sheet1.Cells.Get(iRow1, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 3).Border = complexBorder4;
            textCellType4.Multiline = true;
            textCellType4.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 3).CellType = textCellType4;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 3).Value = "제약회사";
            this.ssPrint_Sheet1.Cells.Get(iRow1, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 4).Border = complexBorder5;
            textCellType5.Multiline = true;
            textCellType5.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 4).CellType = textCellType5;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 4).Value = "표준코드";
            this.ssPrint_Sheet1.Cells.Get(iRow1, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 5).Border = complexBorder6;
            textCellType6.Multiline = true;
            textCellType6.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 5).CellType = textCellType6;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 5).Value = "약코드";
            this.ssPrint_Sheet1.Cells.Get(iRow1, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 6).Border = complexBorder7;
            textCellType7.Multiline = true;
            textCellType7.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 6).CellType = textCellType7;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 6).Value = "기본용법";
            this.ssPrint_Sheet1.Cells.Get(iRow1, 6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 7).Border = complexBorder8;
            textCellType8.Multiline = true;
            textCellType8.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 7).CellType = textCellType8;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 7).Value = "외래용법";
            this.ssPrint_Sheet1.Cells.Get(iRow1, 7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 8).Border = complexBorder9;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 8).Value = "입원용법";
            this.ssPrint_Sheet1.Cells.Get(iRow1, 9).Border = complexBorder10;
            textCellType9.Multiline = true;
            textCellType9.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 9).CellType = textCellType9;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 9).Value = "약품명(한글)";
            this.ssPrint_Sheet1.Cells.Get(iRow1, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 10).Border = complexBorder11;
            textCellType10.Multiline = true;
            textCellType10.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 10).CellType = textCellType10;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 10).Value = "약품명(영문)";
            this.ssPrint_Sheet1.Cells.Get(iRow1, 10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 11).Border = complexBorder12;
            textCellType11.Multiline = true;
            textCellType11.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 11).CellType = textCellType11;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 11).Value = "성분명";
            this.ssPrint_Sheet1.Cells.Get(iRow1, 11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 12).Border = complexBorder13;
            textCellType12.Multiline = true;
            textCellType12.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 12).CellType = textCellType12;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 12).Value = "제형";
            this.ssPrint_Sheet1.Cells.Get(iRow1, 12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 13).Border = complexBorder14;
            textCellType13.Multiline = true;
            textCellType13.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 13).CellType = textCellType13;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 13).Value = "약제용량";
            this.ssPrint_Sheet1.Cells.Get(iRow1, 13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 14).Border = complexBorder15;
            textCellType14.Multiline = true;
            textCellType14.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 14).CellType = textCellType14;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 14).Value = "용량단위";
            this.ssPrint_Sheet1.Cells.Get(iRow1, 14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 15).Border = complexBorder16;
            textCellType15.Multiline = true;
            textCellType15.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 15).CellType = textCellType15;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1, 15).Value = "부피(ml)";
            this.ssPrint_Sheet1.Cells.Get(iRow1, 15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 0).Border = complexBorder17;
            textCellType16.Multiline = true;
            textCellType16.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 0).CellType = textCellType16;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 0).RowSpan = 3;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 0).Value = "1";
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 1).Border = complexBorder18;
            textCellType17.Multiline = true;
            textCellType17.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 1).CellType = textCellType17;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 2).Border = complexBorder19;
            textCellType18.Multiline = true;
            textCellType18.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 2).CellType = textCellType18;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 3).Border = complexBorder20;
            textCellType19.Multiline = true;
            textCellType19.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 3).CellType = textCellType19;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 4).Border = complexBorder21;
            textCellType20.Multiline = true;
            textCellType20.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 4).CellType = textCellType20;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 5).Border = complexBorder22;
            textCellType21.Multiline = true;
            textCellType21.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 5).CellType = textCellType21;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 6).Border = complexBorder23;
            textCellType22.Multiline = true;
            textCellType22.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 6).CellType = textCellType22;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 7).Border = complexBorder24;
            textCellType23.Multiline = true;
            textCellType23.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 7).CellType = textCellType23;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 8).Border = complexBorder25;
            textCellType24.Multiline = true;
            textCellType24.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 8).CellType = textCellType24;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 9).Border = complexBorder26;
            textCellType25.Multiline = true;
            textCellType25.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 9).CellType = textCellType25;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 10).Border = complexBorder27;
            textCellType26.Multiline = true;
            textCellType26.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 10).CellType = textCellType26;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 11).Border = complexBorder28;
            textCellType27.Multiline = true;
            textCellType27.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 11).CellType = textCellType27;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 12).Border = complexBorder29;
            textCellType28.Multiline = true;
            textCellType28.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 12).CellType = textCellType28;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 13).Border = complexBorder30;
            textCellType29.Multiline = true;
            textCellType29.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 13).CellType = textCellType29;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 14).Border = complexBorder31;
            textCellType30.Multiline = true;
            textCellType30.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 14).CellType = textCellType30;
            this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 15).Border = complexBorder32;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 0).Border = complexBorder33;
            textCellType31.Multiline = true;
            textCellType31.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 0).CellType = textCellType31;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 1).Border = complexBorder34;
            textCellType32.Multiline = true;
            textCellType32.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 1).CellType = textCellType32;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 1).Value = "제형";
            this.ssPrint_Sheet1.Cells.Get(iRow2, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 2).Border = complexBorder35;
            textCellType33.Multiline = true;
            textCellType33.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 2).CellType = textCellType33;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 2).Value = "처방명1";
            this.ssPrint_Sheet1.Cells.Get(iRow2, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 3).Border = complexBorder36;
            textCellType34.Multiline = true;
            textCellType34.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 3).CellType = textCellType34;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 3).Value = "약가";
            this.ssPrint_Sheet1.Cells.Get(iRow2, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 4).Border = complexBorder37;
            textCellType35.Multiline = true;
            textCellType35.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 4).CellType = textCellType35;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 4).Value = "J항";
            this.ssPrint_Sheet1.Cells.Get(iRow2, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 5).Border = complexBorder38;
            textCellType36.Multiline = true;
            textCellType36.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 5).CellType = textCellType36;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 5).Value = "F항";
            this.ssPrint_Sheet1.Cells.Get(iRow2, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 6).Border = complexBorder39;
            textCellType37.Multiline = true;
            textCellType37.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 6).CellType = textCellType37;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 6).Value = "O항";
            this.ssPrint_Sheet1.Cells.Get(iRow2, 6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 7).Border = complexBorder40;
            textCellType38.Multiline = true;
            textCellType38.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 7).CellType = textCellType38;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 7).Value = "U항";
            this.ssPrint_Sheet1.Cells.Get(iRow2, 7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 8).Border = complexBorder41;
            textCellType39.Multiline = true;
            textCellType39.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 8).CellType = textCellType39;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 8).Value = "일용량단위";
            this.ssPrint_Sheet1.Cells.Get(iRow2, 8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 9).Border = complexBorder42;
            textCellType40.Multiline = true;
            textCellType40.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 9).CellType = textCellType40;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 9).Value = "일용량";
            this.ssPrint_Sheet1.Cells.Get(iRow2, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 10).Border = complexBorder43;
            textCellType41.Multiline = true;
            textCellType41.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 10).CellType = textCellType41;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 10).Value = "ITEM CD";
            this.ssPrint_Sheet1.Cells.Get(iRow2, 10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 11).Border = complexBorder44;
            textCellType42.Multiline = true;
            textCellType42.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 11).CellType = textCellType42;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 11).Value = "처방명2";
            this.ssPrint_Sheet1.Cells.Get(iRow2, 11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 12).Border = complexBorder45;
            textCellType43.Multiline = true;
            textCellType43.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 12).CellType = textCellType43;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 12).Value = "투약경로";
            this.ssPrint_Sheet1.Cells.Get(iRow2, 12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 13).Border = complexBorder46;
            textCellType44.Multiline = true;
            textCellType44.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 13).CellType = textCellType44;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 13).Value = "기타투약경로";
            this.ssPrint_Sheet1.Cells.Get(iRow2, 13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 14).Border = complexBorder47;
            textCellType45.Multiline = true;
            textCellType45.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 14).CellType = textCellType45;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 14).Value = "시행일";
            this.ssPrint_Sheet1.Cells.Get(iRow2, 14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 15).Border = complexBorder48;
            textCellType46.Multiline = true;
            textCellType46.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 15).CellType = textCellType46;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2, 15).Value = "기타사항";
            this.ssPrint_Sheet1.Cells.Get(iRow2, 15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 0).Border = complexBorder49;
            textCellType47.Multiline = true;
            textCellType47.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 0).CellType = textCellType47;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 1).Border = complexBorder50;
            textCellType48.Multiline = true;
            textCellType48.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 1).CellType = textCellType48;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 2).Border = complexBorder51;
            textCellType49.Multiline = true;
            textCellType49.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 2).CellType = textCellType49;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 3).Border = complexBorder52;
            textCellType50.Multiline = true;
            textCellType50.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 3).CellType = textCellType50;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 4).Border = complexBorder53;
            textCellType51.Multiline = true;
            textCellType51.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 4).CellType = textCellType51;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 5).Border = complexBorder54;
            textCellType52.Multiline = true;
            textCellType52.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 5).CellType = textCellType52;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 6).Border = complexBorder55;
            textCellType53.Multiline = true;
            textCellType53.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 6).CellType = textCellType53;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 7).Border = complexBorder56;
            textCellType54.Multiline = true;
            textCellType54.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 7).CellType = textCellType54;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 8).Border = complexBorder57;
            textCellType55.Multiline = true;
            textCellType55.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 8).CellType = textCellType55;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 9).Border = complexBorder58;
            textCellType56.Multiline = true;
            textCellType56.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 9).CellType = textCellType56;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 10).Border = complexBorder59;
            textCellType57.Multiline = true;
            textCellType57.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 10).CellType = textCellType57;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 11).Border = complexBorder60;
            textCellType58.Multiline = true;
            textCellType58.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 11).CellType = textCellType58;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 12).Border = complexBorder61;
            textCellType59.Multiline = true;
            textCellType59.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 12).CellType = textCellType59;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 13).Border = complexBorder62;
            textCellType60.Multiline = true;
            textCellType60.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 13).CellType = textCellType60;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 14).Border = complexBorder63;
            textCellType61.Multiline = true;
            textCellType61.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 14).CellType = textCellType61;
            this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 15).Border = complexBorder64;
            this.ssPrint_Sheet1.ColumnHeader.Visible = false;
            this.ssPrint_Sheet1.Columns.Get(0).CellType = textCellType62;
            this.ssPrint_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Columns.Get(0).Width = 39F;
            this.ssPrint_Sheet1.Columns.Get(1).CellType = textCellType63;
            this.ssPrint_Sheet1.Columns.Get(1).Width = 100F;
            this.ssPrint_Sheet1.Columns.Get(2).CellType = textCellType64;
            this.ssPrint_Sheet1.Columns.Get(2).Width = 100F;
            this.ssPrint_Sheet1.Columns.Get(3).CellType = textCellType65;
            this.ssPrint_Sheet1.Columns.Get(3).Width = 100F;
            this.ssPrint_Sheet1.Columns.Get(4).CellType = textCellType66;
            this.ssPrint_Sheet1.Columns.Get(4).Width = 85F;
            this.ssPrint_Sheet1.Columns.Get(5).CellType = textCellType67;
            this.ssPrint_Sheet1.Columns.Get(5).Width = 85F;
            this.ssPrint_Sheet1.Columns.Get(6).CellType = textCellType68;
            this.ssPrint_Sheet1.Columns.Get(6).Width = 85F;
            this.ssPrint_Sheet1.Columns.Get(7).CellType = textCellType69;
            this.ssPrint_Sheet1.Columns.Get(7).Width = 85F;
            this.ssPrint_Sheet1.Columns.Get(8).CellType = textCellType70;
            this.ssPrint_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Columns.Get(8).Width = 85F;
            this.ssPrint_Sheet1.Columns.Get(9).CellType = textCellType71;
            this.ssPrint_Sheet1.Columns.Get(9).Width = 100F;
            this.ssPrint_Sheet1.Columns.Get(10).CellType = textCellType72;
            this.ssPrint_Sheet1.Columns.Get(10).Width = 100F;
            this.ssPrint_Sheet1.Columns.Get(11).CellType = textCellType73;
            this.ssPrint_Sheet1.Columns.Get(11).Width = 100F;
            this.ssPrint_Sheet1.Columns.Get(12).CellType = textCellType74;
            this.ssPrint_Sheet1.Columns.Get(12).Width = 100F;
            this.ssPrint_Sheet1.Columns.Get(13).CellType = textCellType75;
            this.ssPrint_Sheet1.Columns.Get(13).Width = 100F;
            this.ssPrint_Sheet1.Columns.Get(14).CellType = textCellType76;
            this.ssPrint_Sheet1.Columns.Get(14).Width = 85F;
            this.ssPrint_Sheet1.Columns.Get(15).CellType = textCellType77;
            this.ssPrint_Sheet1.Columns.Get(15).Width = 100F;
            this.ssPrint_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssPrint_Sheet1.RowHeader.Visible = false;
            this.ssPrint_Sheet1.Rows.Get(iRow1).Height = 22F;
            this.ssPrint_Sheet1.Rows.Get(iRow1 + 1).Height = 22F;
            this.ssPrint_Sheet1.Rows.Get(iRow2).Height = 22F;
            this.ssPrint_Sheet1.Rows.Get(iRow2 + 1).Height = 22F;
            this.ssPrint_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;







            //this.ssPrint_Sheet1.Cells.Get(iRow1, 0).Border = complexBorder1;
            //textCellType1.Multiline = true;
            //textCellType1.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 0).CellType = textCellType1;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 0).Value = "번호";
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 1).Border = complexBorder2;
            //textCellType2.Multiline = true;
            //textCellType2.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 1).CellType = textCellType2;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 1).Value = "내용";
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 2).Border = complexBorder3;
            //textCellType3.Multiline = true;
            //textCellType3.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 2).CellType = textCellType3;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 2).Value = "분류";
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 3).Border = complexBorder4;
            //textCellType4.Multiline = true;
            //textCellType4.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 3).CellType = textCellType4;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 3).Value = "제약회사";
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 4).Border = complexBorder5;
            //textCellType5.Multiline = true;
            //textCellType5.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 4).CellType = textCellType5;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 4).Value = "표준코드";
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 5).Border = complexBorder6;
            //textCellType6.Multiline = true;
            //textCellType6.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 5).CellType = textCellType6;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 5).Value = "약코드";
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 6).Border = complexBorder7;
            //textCellType7.Multiline = true;
            //textCellType7.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 6).CellType = textCellType7;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 6).Value = "기본용법";
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 7).Border = complexBorder8;
            //textCellType8.Multiline = true;
            //textCellType8.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 7).CellType = textCellType8;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 7).Value = "외래용법";
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 8).Border = complexBorder9;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 8).Value = "입원용법";
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 9).Border = complexBorder10;
            //textCellType9.Multiline = true;
            //textCellType9.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 9).CellType = textCellType9;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 9).Value = "약품명(한글)";
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 10).Border = complexBorder11;
            //textCellType10.Multiline = true;
            //textCellType10.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 10).CellType = textCellType10;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 10).Value = "약품명(영문)";
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 11).Border = complexBorder12;
            //textCellType11.Multiline = true;
            //textCellType11.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 11).CellType = textCellType11;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 11).Value = "성분명";
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 12).Border = complexBorder13;
            //textCellType12.Multiline = true;
            //textCellType12.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 12).CellType = textCellType12;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 12).Value = "제형";
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 13).Border = complexBorder14;
            //textCellType13.Multiline = true;
            //textCellType13.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 13).CellType = textCellType13;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 13).Value = "약제용량";
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 14).Border = complexBorder15;
            //textCellType14.Multiline = true;
            //textCellType14.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 14).CellType = textCellType14;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 14).Value = "용량단위";
            //this.ssPrint_Sheet1.Cells.Get(iRow1, 14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 0).Border = complexBorder16;
            //textCellType15.Multiline = true;
            //textCellType15.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 0).CellType = textCellType15;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 0).RowSpan = 3;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 0).Value = "1";
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 1).Border = complexBorder17;
            //textCellType16.Multiline = true;
            //textCellType16.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 1).CellType = textCellType16;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 2).Border = complexBorder18;
            //textCellType17.Multiline = true;
            //textCellType17.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 2).CellType = textCellType17;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 3).Border = complexBorder19;
            //textCellType18.Multiline = true;
            //textCellType18.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 3).CellType = textCellType18;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 4).Border = complexBorder20;
            //textCellType19.Multiline = true;
            //textCellType19.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 4).CellType = textCellType19;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 5).Border = complexBorder21;
            //textCellType20.Multiline = true;
            //textCellType20.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 5).CellType = textCellType20;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 6).Border = complexBorder22;
            //textCellType21.Multiline = true;
            //textCellType21.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 6).CellType = textCellType21;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 7).Border = complexBorder23;
            //textCellType22.Multiline = true;
            //textCellType22.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 7).CellType = textCellType22;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 8).Border = complexBorder24;
            //textCellType23.Multiline = true;
            //textCellType23.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 8).CellType = textCellType23;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 9).Border = complexBorder25;
            //textCellType24.Multiline = true;
            //textCellType24.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 9).CellType = textCellType24;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 10).Border = complexBorder26;
            //textCellType25.Multiline = true;
            //textCellType25.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 10).CellType = textCellType25;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 11).Border = complexBorder27;
            //textCellType26.Multiline = true;
            //textCellType26.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 11).CellType = textCellType26;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 12).Border = complexBorder28;
            //textCellType27.Multiline = true;
            //textCellType27.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 12).CellType = textCellType27;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 13).Border = complexBorder29;
            //textCellType28.Multiline = true;
            //textCellType28.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 13).CellType = textCellType28;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 14).Border = complexBorder30;
            //textCellType29.Multiline = true;
            //textCellType29.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow1 + 1, 14).CellType = textCellType29;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 0).Border = complexBorder31;
            //textCellType30.Multiline = true;
            //textCellType30.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 0).CellType = textCellType30;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 1).Border = complexBorder32;
            //textCellType31.Multiline = true;
            //textCellType31.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 1).CellType = textCellType31;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 1).Value = "부피(ml)";
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 2).Border = complexBorder33;
            //textCellType32.Multiline = true;
            //textCellType32.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 2).CellType = textCellType32;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 2).Value = "제형";
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 3).Border = complexBorder34;
            //textCellType33.Multiline = true;
            //textCellType33.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 3).CellType = textCellType33;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 3).Value = "약가";
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 4).Border = complexBorder35;
            //textCellType34.Multiline = true;
            //textCellType34.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 4).CellType = textCellType34;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 4).Value = "J항";
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 5).Border = complexBorder36;
            //textCellType35.Multiline = true;
            //textCellType35.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 5).CellType = textCellType35;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 5).Value = "F항";
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 6).Border = complexBorder37;
            //textCellType36.Multiline = true;
            //textCellType36.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 6).CellType = textCellType36;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 6).Value = "O항";
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 7).Border = complexBorder38;
            //textCellType37.Multiline = true;
            //textCellType37.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 7).CellType = textCellType37;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 7).Value = "U항";
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 8).Border = complexBorder39;
            //textCellType38.Multiline = true;
            //textCellType38.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 8).CellType = textCellType38;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 8).Value = "일용량단위";
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 9).Border = complexBorder40;
            //textCellType39.Multiline = true;
            //textCellType39.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 9).CellType = textCellType39;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 9).Value = "일용량";
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 10).Border = complexBorder41;
            //textCellType40.Multiline = true;
            //textCellType40.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 10).CellType = textCellType40;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 10).Value = "ITEM CD";
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 11).Border = complexBorder42;
            //textCellType41.Multiline = true;
            //textCellType41.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 11).CellType = textCellType41;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 11).Value = "투약경로";
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 12).Border = complexBorder43;
            //textCellType42.Multiline = true;
            //textCellType42.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 12).CellType = textCellType42;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 12).Value = "기타투약경로";
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 13).Border = complexBorder44;
            //textCellType43.Multiline = true;
            //textCellType43.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 13).CellType = textCellType43;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 13).Value = "시행일";
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 14).Border = complexBorder45;
            //textCellType44.Multiline = true;
            //textCellType44.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 14).CellType = textCellType44;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 14).Value = "기타사항";
            //this.ssPrint_Sheet1.Cells.Get(iRow2, 14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 0).Border = complexBorder46;
            //textCellType45.Multiline = true;
            //textCellType45.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 0).CellType = textCellType45;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 1).Border = complexBorder47;
            //textCellType46.Multiline = true;
            //textCellType46.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 1).CellType = textCellType46;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 2).Border = complexBorder48;
            //textCellType47.Multiline = true;
            //textCellType47.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 2).CellType = textCellType47;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 3).Border = complexBorder49;
            //textCellType48.Multiline = true;
            //textCellType48.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 3).CellType = textCellType48;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 4).Border = complexBorder50;
            //textCellType49.Multiline = true;
            //textCellType49.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 4).CellType = textCellType49;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 5).Border = complexBorder51;
            //textCellType50.Multiline = true;
            //textCellType50.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 5).CellType = textCellType50;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 6).Border = complexBorder52;
            //textCellType51.Multiline = true;
            //textCellType51.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 6).CellType = textCellType51;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 7).Border = complexBorder53;
            //textCellType52.Multiline = true;
            //textCellType52.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 7).CellType = textCellType52;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 8).Border = complexBorder54;
            //textCellType53.Multiline = true;
            //textCellType53.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 8).CellType = textCellType53;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 9).Border = complexBorder55;
            //textCellType54.Multiline = true;
            //textCellType54.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 9).CellType = textCellType54;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 10).Border = complexBorder56;
            //textCellType55.Multiline = true;
            //textCellType55.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 10).CellType = textCellType55;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 11).Border = complexBorder57;
            //textCellType56.Multiline = true;
            //textCellType56.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 11).CellType = textCellType56;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 12).Border = complexBorder58;
            //textCellType57.Multiline = true;
            //textCellType57.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 12).CellType = textCellType57;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 13).Border = complexBorder59;
            //textCellType58.Multiline = true;
            //textCellType58.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 13).CellType = textCellType58;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 14).Border = complexBorder60;
            //textCellType59.Multiline = true;
            //textCellType59.WordWrap = true;
            //this.ssPrint_Sheet1.Cells.Get(iRow2 + 1, 14).CellType = textCellType59;
            //this.ssPrint_Sheet1.Columns.Get(0).CellType = textCellType60;
            //this.ssPrint_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(0).Width = 39F;
            //this.ssPrint_Sheet1.Columns.Get(1).CellType = textCellType61;
            //this.ssPrint_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(1).Width = 150F;
            //this.ssPrint_Sheet1.Columns.Get(2).CellType = textCellType62;
            //this.ssPrint_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(2).Width = 50F;
            //this.ssPrint_Sheet1.Columns.Get(3).CellType = textCellType63;
            //this.ssPrint_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(3).Width = 100F;
            //this.ssPrint_Sheet1.Columns.Get(4).CellType = textCellType64;
            //this.ssPrint_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(4).Width = 85F;
            //this.ssPrint_Sheet1.Columns.Get(5).CellType = textCellType65;
            //this.ssPrint_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(5).Width = 85F;
            //this.ssPrint_Sheet1.Columns.Get(6).CellType = textCellType66;
            //this.ssPrint_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(6).Width = 85F;
            //this.ssPrint_Sheet1.Columns.Get(7).CellType = textCellType67;
            //this.ssPrint_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(7).Width = 85F;
            //this.ssPrint_Sheet1.Columns.Get(8).CellType = textCellType68;
            //this.ssPrint_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(8).Width = 85F;
            //this.ssPrint_Sheet1.Columns.Get(9).CellType = textCellType69;
            //this.ssPrint_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(9).Width = 100F;
            //this.ssPrint_Sheet1.Columns.Get(10).CellType = textCellType70;
            //this.ssPrint_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(10).Width = 100F;
            //this.ssPrint_Sheet1.Columns.Get(11).CellType = textCellType71;
            //this.ssPrint_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(11).Width = 100F;
            //this.ssPrint_Sheet1.Columns.Get(12).CellType = textCellType72;
            //this.ssPrint_Sheet1.Columns.Get(12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(12).Width = 100F;
            //this.ssPrint_Sheet1.Columns.Get(13).CellType = textCellType73;
            //this.ssPrint_Sheet1.Columns.Get(13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(13).Width = 100F;
            //this.ssPrint_Sheet1.Columns.Get(14).CellType = textCellType74;
            //this.ssPrint_Sheet1.Columns.Get(14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //this.ssPrint_Sheet1.Columns.Get(14).Width = 85F;
            //this.ssPrint_Sheet1.RowHeader.Columns.Default.Resizable = false;
            //this.ssPrint_Sheet1.Rows.Get(iRow1).Height = 28F;
            //this.ssPrint_Sheet1.Rows.Get(iRow1 + 1).Height = 22F;
            //this.ssPrint_Sheet1.Rows.Get(iRow2).Height = 28F;
            //this.ssPrint_Sheet1.Rows.Get(iRow2 + 1).Height = 22F;
            //this.ssPrint_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
        }

        private void btnPrintNew_Click(object sender, EventArgs e)
        {
            string strFont1 = "";
            string strHead1 = "";
            string strFont2 = "";
            string strHead2 = "";

            int intHeight = 0;
            byte[] a;

            strFont1 = "/fn\"맑은 고딕\" /fz\"17\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"맑은 고딕\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + "원내약품 수가결정 요청서" + "/f1/n";
            strHead2 = "/l/f2" + "※ 문서번호 : " + VB.Left(GstrJDATE, 4) + "-" + GstrWRTNO + "/f2/n";
            strHead2 = strHead2 + "/l/f2" + VB.Space(11) + "※ 작성일자 : " + GstrJDATE + "/f2/n";
            strHead2 = strHead2 + "/l/f2" + VB.Space(11) + "※ 작 성 자 : " + GstrJNAME + "/f2/n";
            strHead2 = strHead2 + "/l/f2" + VB.Space(11) + "※ 약제팀장확인 : " + GstrYDATE + "/f2/n";
            strHead2 = strHead2 + "/l/f2" + VB.Space(11) + "※ 보험심사팀님확인 : " + GstrSDATE + "/f2/n";


            ssPrintSummary_Sheet1.RowCount = 0;
            ssPrintSummary_Sheet1.RowCount = ssCert_Sheet1.RowCount;

            if (ssCert_Sheet1.RowCount == 0) return;

            for (int i = 0; i < ssCert_Sheet1.RowCount; i++)
            {                                
                ssPrintSummary_Sheet1.Cells[i, 0].Text = ssCert_Sheet1.Cells[i, 1].Text;       //내용
                ssPrintSummary_Sheet1.Cells[i, 1].Text = ssCert_Sheet1.Cells[i, 2].Text;       //분류
                ssPrintSummary_Sheet1.Cells[i, 2].Text = ssCert_Sheet1.Cells[i, 3].Text;       //제약회사
                ssPrintSummary_Sheet1.Cells[i, 3].Text = ssCert_Sheet1.Cells[i, 4].Text;       //표준코드
                ssPrintSummary_Sheet1.Cells[i, 4].Text = ssCert_Sheet1.Cells[i, 5].Text;       //약코드                
                ssPrintSummary_Sheet1.Cells[i, 5].Text = ssCert_Sheet1.Cells[i, 9].Text;       //약품명 (한글)
                ssPrintSummary_Sheet1.Cells[i, 6].Text = ssCert_Sheet1.Cells[i, 10].Text;       //약품명 (영문)
                ssPrintSummary_Sheet1.Cells[i, 7].Text = ssCert_Sheet1.Cells[i, 11].Text;       //성분명
                ssPrintSummary_Sheet1.Cells[i, 8].Text = ssCert_Sheet1.Cells[i, 12].Text;       //제형분류
                ssPrintSummary_Sheet1.Cells[i, 9].Text = ssCert_Sheet1.Cells[i, 13].Text;       //약제용량
                ssPrintSummary_Sheet1.Cells[i, 10].Text = ssCert_Sheet1.Cells[i, 14].Text;       //용량단위
                ssPrintSummary_Sheet1.Cells[i, 11].Text = ssCert_Sheet1.Cells[i, 15].Text;       //부피(ml)

                ssPrintSummary_Sheet1.Cells[i, 12].Text = ssCert_Sheet1.Cells[i, 16].Text;       //제형                
                ssPrintSummary_Sheet1.Cells[i, 13].Text = ssCert_Sheet1.Cells[i, 18].Text;       //약가
                ssPrintSummary_Sheet1.Cells[i, 14].Text = ssCert_Sheet1.Cells[i, 19].Text;       //J항
                ssPrintSummary_Sheet1.Cells[i, 15].Text = ssCert_Sheet1.Cells[i, 20].Text;       //F항
                ssPrintSummary_Sheet1.Cells[i, 16].Text = ssCert_Sheet1.Cells[i, 21].Text;       //O항
                ssPrintSummary_Sheet1.Cells[i, 17].Text = ssCert_Sheet1.Cells[i, 22].Text;       //U항

                ssPrintSummary_Sheet1.Cells[i, 18].Text = ssCert_Sheet1.Cells[i, 29].Text;       //시행일
                ssPrintSummary_Sheet1.Cells[i, 19].Text = ssCert_Sheet1.Cells[i, 30].Text == "" ? " " : ssCert_Sheet1.Cells[i, 30].Text;       //기타사항

                intHeight = 0;
                for (int j = 0; j < 20; j++)
                {
                    a = Encoding.Default.GetBytes(ssPrintSummary_Sheet1.Cells[i, j].Text);

                    if (intHeight < Convert.ToInt32(a.Length / 8))
                    {
                        intHeight = Convert.ToInt32(a.Length / 8);
                    }

                    if (ssPrintSummary_Sheet1.Rows[i].Height < ComNum.SPDROWHT + (intHeight * 22))
                    {
                        ssPrintSummary_Sheet1.SetRowHeight(i, ComNum.SPDROWHT + (intHeight * 22));
                    }
                }
            }

            ssPrintSummary_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssPrintSummary_Sheet1.PrintInfo.ZoomFactor = 0.92f;
            ssPrintSummary_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssPrintSummary_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssPrintSummary_Sheet1.PrintInfo.Margin.Top = 90;
            ssPrintSummary_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssPrintSummary_Sheet1.PrintInfo.Margin.Header = 10;
            ssPrintSummary_Sheet1.PrintInfo.ShowColor = false;
            //ssPrintSummary_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssPrintSummary_Sheet1.PrintInfo.ShowBorder = true;
            ssPrintSummary_Sheet1.PrintInfo.ShowGrid = true;
            ssPrintSummary_Sheet1.PrintInfo.ShowShadows = false;
            ssPrintSummary_Sheet1.PrintInfo.UseMax = true;
            ssPrintSummary_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssPrintSummary_Sheet1.PrintInfo.UseSmartPrint = false;
            ssPrintSummary_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssPrintSummary_Sheet1.PrintInfo.Preview = false;
            ssPrintSummary.PrintSheet(0);
        }       
    }
}
