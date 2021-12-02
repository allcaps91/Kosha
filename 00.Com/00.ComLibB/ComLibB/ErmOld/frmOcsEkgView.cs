using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmOcsEkgView.cs
    /// Description     : 수술실 대장
    /// Author          : 박창욱
    /// Create Date     : 2017-08-28
    /// Update History  : 
    /// </summary>
    /// <history>  
    ///
    /// </history>
    /// <seealso cref= "\Ocs\ekg\Frm수술대장_ekg.frm(Frm수술대장_ekg.frm) >> frmOcsEkgView.cs 폼이름 재정의" />	
    public partial class frmOcsEkgView : Form
    {
        string GstrPano = "";

        frmEmrViewer frmEmrViewerX = null;
        public frmOcsEkgView()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false)
                return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string SysDate = "";

            SysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            Cursor.Current = Cursors.WaitCursor;

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/n/n/f1/C 수 술 실 대 장" + "/n/n/n/n";
            strHead2 = "/l/f2" + "조회기간 : " + dtpFDate.Value.ToString("yyyy-MM-dd") + "~" + dtpTDate.Value.ToString("yyyy-MM-dd") + VB.Space(10) + "인쇄일자 : " + SysDate;

            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Left = 35;
            ssView_Sheet1.PrintInfo.Margin.Right = 0;
            ssView_Sheet1.PrintInfo.Margin.Top = 35;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 30;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRead = 0;
            int nRow = 0;
            string strOK = "";
            string strOpSize = "";
            string strOpSize2 = "";
            string strFDate = "";
            string strTDate = "";
            string strOpGbn = "";
            string strOpGbn2 = "";
            string strOpGbn3 = "";
            string strOpGbnX = "";
            string strAnGbn = "";

            strFDate = dtpFDate.Value.ToString("yyyy-MM-dd");
            strTDate = dtpTDate.Value.ToString("yyyy-MM-dd");

            if (cboGbn.Text.Trim() != "")
            {
                switch (VB.Left(cboGbn.Text, 1))
                {
                    case "1":
                        strOpGbn = "1";
                        break;            //정규수술
                    case "2":
                        strOpGbn = "2//,//3";
                        break;        //응급수실
                    case "3":
                        strOpGbn = "4";
                        break;            //통원수술
                    case "4":
                        strOpGbn2 = "4";
                        break;           //처치
                    case "5":
                        strOpGbn2 = "5";
                        break;           //교정
                    case "6":
                        strOpGbn2 = "6";
                        break;           //운동
                    case "7":
                        strOpGbn3 = "E/B";
                        break;         //조직검사시술
                    case "8":
                        strOpGbnX = "X";
                        break;           //취소건
                }
            }

            if (cboGbn2.Text.Trim() != "")
            {
                switch (VB.Left(cboGbn2.Text.Trim(), 1))
                {
                    case "1":
                        strAnGbn = "G";
                        break;                        //전신마취
                    case "2":
                        strAnGbn = "S','E','A";
                        break;                //부위마취전체
                    case "3":
                        strAnGbn = "L";
                        break;                        //국소마취
                    case "4":
                        strAnGbn = "S";
                        break;                        //부위마취-척추마취
                    case "5":
                        strAnGbn = "E";
                        break;                        //부위마취-경막외마취
                    case "6":
                        strAnGbn = "A";
                        break;                        //부위마취-상박신경총마취
                    case "7":
                        strAnGbn = "M','LV-A','LV-B','LV-C";
                        break;   //기타마취-mask,정맥-a,b,c
                }
            }

            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = " SELECT * ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "ORAN_MASTER ";
                SQL = SQL + ComNum.VBLF + "  WHERE OpDate >=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND OpDate <=TO_DATE('" + strTDate + "','YYYY-MM-DD') ";

                if (grbName.Visible == true)
                {
                    if (txtPano.Text != "")
                    {
                        SQL = SQL + "  AND PANO = '" + txtPano.Text + "' ";
                    }
                }
                else
                {
                    if (cboDept.Text.Trim() != "전체")
                    {
                        SQL = SQL + "   AND DeptCode = '" + cboDept.Text.Trim() + "' ";    //과
                        if (VB.Left(cboDr.Text, 4) != "****")
                        {
                            SQL = SQL + " AND DRCODE = '" + VB.Left(cboDr.Text, 4) + "' ";
                        }
                    }
                }

                if (strOpGbn != "")
                {
                    SQL = SQL + "   AND OpBun IN ('" + strOpGbn + "') ";  //수술구분
                }
                if (strAnGbn != "")
                {
                    SQL = SQL + "   AND AnGbn IN ('" + strAnGbn + "') "; //마취구분
                }
                if (strOpGbn3 != "")
                {
                    SQL = SQL + "   AND OpTitle IN ('" + strOpGbn3 + "') "; //마취구분
                }
                if (strOpGbnX == "X")
                {
                    SQL = SQL + "   AND OpCancel IS NOT NULL "; //수술취소
                }
                else
                {
                    SQL = SQL + "   AND Opcancel IS NULL  ";
                }

                if (rdoGbn2.Checked == true)
                {
                    SQL = SQL + " AND GbAngio = 'Y' ";
                }
                else
                {
                    SQL = SQL + " AND (GbAngio IS NULL OR GbAngio <> 'Y') ";
                }

                if (chkRed.Checked == true && chkComp.Checked == true)
                {
                    SQL = SQL + " AND (OpErr>='1' OR OpHapSayu IS NOT NULL) ";
                }
                else if (chkRed.Checked == true)
                {
                    SQL = SQL + " AND OpErr>='1' ";
                }
                else if (chkComp.Checked == true)
                {
                    SQL = SQL + " AND OpHapSayu IS NOT NULL ";
                }
                if (VB.Left(cboIO.Text, 1) == "2")
                {
                    SQL = SQL + " AND IpdOpd ='I' ";
                }
                else if (VB.Left(cboIO.Text, 1) == "3")
                {
                    SQL = SQL + " AND IpdOpd ='O' ";
                }

                SQL = SQL + "ORDER BY OpDate,OpTimeFrom ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nRead = dt.Rows.Count;
                ssView_Sheet1.RowCount = nRead;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < nRead; i++)
                {
                    strOK = "OK";
                    strOpSize = "";
                    strOpSize2 = "";

                    SQL = "";
                    SQL = " SELECT OpSize FROM " + ComNum.DB_PMPA + "ORAN_OPBUN ";
                    SQL = SQL + ComNum.VBLF + " WHERE Dept='" + dt.Rows[i]["DeptCode"].ToString().Trim() + "'  ";
                    SQL = SQL + ComNum.VBLF + " AND Bun=" + VB.Val(dt.Rows[i]["OpBun"].ToString().Trim()) + "  ";
                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        switch (dt1.Rows[0]["OpSize"].ToString().Trim())
                        {
                            case "1":
                                strOpSize = "대";
                                break;
                            case "2":
                                strOpSize = "중";
                                break;
                            case "3":
                                strOpSize = "소";
                                break;
                            case "4":
                                strOpSize2 = "처치";
                                break;
                            case "5":
                                strOpSize2 = "교정";
                                break;
                            case "6":
                                strOpSize2 = "운동";
                                break;
                            default:
                                strOpSize = "";
                                strOpSize2 = "";
                                break;
                        }
                    }

                    if (strOpGbn2 != "")
                    {
                        strOK = "";
                        switch (strOpGbn2)
                        {
                            case "4":
                                if (strOpSize2 == "처치")
                                {
                                    strOK = "OK";
                                }
                                break;
                            case "5":
                                if (strOpSize2 == "교정")
                                {
                                    strOK = "OK";
                                }
                                break;
                            case "6":
                                if (strOpSize2 == "운동")
                                {
                                    strOK = "OK";
                                }
                                break;
                        }
                    }

                    if (strOK == "OK")
                    {
                        nRow += 1;
                        ssView_Sheet1.Cells[nRow - 1, 0].Text = Convert.ToDateTime(dt.Rows[i]["OpDate"].ToString().Trim()).ToString("yyyy-MM-dd");
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["OpTimeFrom"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["Age"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                        ssView_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["DIAGNOSIS"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["OpTitle"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 11].Text = "";
                        switch (dt.Rows[i]["OpBun"].ToString().Trim())
                        {
                            case "1":
                                ssView_Sheet1.Cells[nRow - 1, 11].Text = "정규수술";
                                break;
                            case "2":
                                ssView_Sheet1.Cells[nRow - 1, 11].Text = "응급수술";
                                break;
                            case "3":
                                ssView_Sheet1.Cells[nRow - 1, 11].Text = "ER응급수술";
                                break;
                            case "4":
                                ssView_Sheet1.Cells[nRow - 1, 11].Text = "통원수술";
                                break;
                            case "A":
                                ssView_Sheet1.Cells[nRow - 1, 11].Text = "환자사유";
                                break;
                            case "B":
                                ssView_Sheet1.Cells[nRow - 1, 11].Text = "환자거부";
                                break;
                            case "C":
                                ssView_Sheet1.Cells[nRow - 1, 11].Text = "입원안함";
                                break;
                            case "X":
                                ssView_Sheet1.Cells[nRow - 1, 11].Text = "취소전체";
                                break;
                            case "O":
                                ssView_Sheet1.Cells[nRow - 1, 11].Text = "병원사유";
                                break;
                        }
                        ssView_Sheet1.Cells[nRow - 1, 12].Text = strOpSize2;
                        ssView_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["AnDoct1"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 14].Text = dt.Rows[i]["AnGbn"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 15].Text = strOpSize;

                        dt1.Dispose();
                        dt1 = null;
                        ssView_Sheet1.Cells[nRow - 1, 16].Text = "무";
                        if (dt.Rows[i]["OPCANCEL"].ToString().Trim() != "")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 16].Text = "유";
                        }
                        ssView_Sheet1.Cells[nRow - 1, 17].Text = dt.Rows[i]["OpErr"].ToString().Trim();
                        if (dt.Rows[i]["OpHapSayu"].ToString().Trim() == "")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 18].Text = "무";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[nRow - 1, 18].Text = "◎";
                        }
                        ssView_Sheet1.Cells[nRow - 1, 19].Text = dt.Rows[i]["OPSTIME"].ToString().Trim();
                    }
                }
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                dt.Dispose();
                dt = null;
                ssView_Sheet1.RowCount = nRow;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            clsVbfunc.SetDrCodeCombo(clsDB.DbCon, cboDr, VB.Left(cboDept.Text, 2), "1", 1, "");
        }

        private void frmOcsEkgView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            grbName.Location = new System.Drawing.Point(500, 7);

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string SysDate = "";

            SysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            dtpFDate.Value = Convert.ToDateTime(SysDate);
            dtpTDate.Value = Convert.ToDateTime(SysDate);

            cboGbn.Items.Clear();
            cboGbn.Items.Add(" ");
            cboGbn.Items.Add("1.정규수술대장");
            cboGbn.Items.Add("2.응급수술대장");
            cboGbn.Items.Add("3.통원수술대장");
            cboGbn.Items.Add("4.수술실처치대장");
            cboGbn.Items.Add("5.수술실운동대장");
            cboGbn.Items.Add("6.수술실교정대장");
            cboGbn.Items.Add("7.조직검사시술대장");
            cboGbn.Items.Add("8.수술취소대장");
            cboGbn.SelectedIndex = -1;

            cboGbn2.Items.Clear();
            cboGbn2.Items.Add(" ");
            cboGbn2.Items.Add("1.전신마취대장");
            cboGbn2.Items.Add("2.부위마취대장(전체)");
            cboGbn2.Items.Add("3.국소마취대장");
            cboGbn2.Items.Add("4.부위마취대장(척추마취)");
            cboGbn2.Items.Add("5.부위마취대장(경막외마취)");
            cboGbn2.Items.Add("6.부위마취대장(상박신경총마취)");
            cboGbn2.Items.Add("7.부위마취대장(기타)");
            cboGbn2.SelectedIndex = -1;

            cboIO.Items.Clear();
            cboIO.Items.Add(" ");
            cboIO.Items.Add("1.전체");
            cboIO.Items.Add("2.입원");
            cboIO.Items.Add("3.외래");
            cboIO.SelectedIndex = 0;

            try
            {
                //진료과 Combo SET
                SQL = "";
                SQL = "SELECT DeptCode FROM " + ComNum.DB_PMPA + "BAS_ClinicDept ";
                SQL = SQL + ComNum.VBLF + " WHERE DeptCode NOT IN ('II','HR','TO','R6','HD','PT','AN') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY PrintRanking ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                cboDept.Items.Clear();
                cboDept.Items.Add("전체");
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboDept.Items.Add(dt.Rows[i]["Deptcode"].ToString().Trim());
                }
                cboDept.SelectedIndex = 0;
                txtPano.Text = "";

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            //TODO 정영록 일딴 세팅 추후 과별로 변경
            rdoDept.Checked = true;

            cboDept.SelectedIndex = cboDept.Items.IndexOf("MC");
        }

        private void rdoDept_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoDept.Checked == true)
            {
                grbName.Visible = false;
            }
            else
            {
                grbName.Visible = true;
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strPano = "";
            string strOpDate = "";
            string strSayu = "";
            string strPara = "";

            if (e.Column == 3)
            {
                GstrPano = ssView_Sheet1.Cells[e.Row, 3].Text;
                strPara = "";

                if (frmEmrViewerX != null)
                {
                    frmEmrViewerX.SetNewPatient(GstrPano);
                    return;
                }

                frmEmrViewerX = new frmEmrViewer(GstrPano);
                frmEmrViewerX.rEventClosed += FrmEmrViewerX_rEventClosed;
                frmEmrViewerX.Show(this);
                ////clsVbEmr.EXECUTE_TextEmrView(GstrPano, clsType.User.Sabun, strPara);
                return;
            }

            if (e.Column != 18)
            {
                return;
            }

            if (ssView_Sheet1.Cells[e.Row, 18].Text == "")
            {
                return;
            }

            strOpDate = ssView_Sheet1.Cells[e.Row, 0].Text;
            strPano = ssView_Sheet1.Cells[e.Row, 3].Text;

            try
            {
                SQL = "";
                SQL = "SELECT OpHapSayu FROM " + ComNum.DB_PMPA + "ORAN_MASTER ";
                SQL = SQL + ComNum.VBLF + "WHERE Pano='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "  AND OpDate=TO_DATE('" + strOpDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND OpHapSayu IS NOT NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                strSayu = dt.Rows[0]["OpHapSayu"].ToString().Trim();

                dt.Dispose();
                dt = null;

                if (strSayu != "")
                {
                    ComFunc.MsgBox(strSayu, "합병증");
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void FrmEmrViewerX_rEventClosed()
        {
            if (frmEmrViewerX != null)
            {
                frmEmrViewerX.Dispose();
                frmEmrViewerX = null;
                return;
            }
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT PANO FROM BAS_PATIENT  WHERE PANO = '" + txtPano.Text + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("등록번호를 다시 확인해주세요.");
                    txtPano.Text = "";
                    return;
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void rdoPano_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void frmOcsEkgView_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (frmEmrViewerX != null)
            //{
            //    frmEmrViewerX.Dispose();
            //    frmEmrViewerX = null;
            //    return;
            //}
        }
    }
}
