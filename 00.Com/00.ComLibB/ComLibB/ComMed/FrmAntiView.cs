using ComBase; //기본 클래스
using ComBase.Controls;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Description : 제한항생제 의뢰현황
/// Author : 이상훈
/// Create Date : 2017.07.24
/// </summary>
/// <history>
/// </history>
/// <seealso cref="FrmAntiView_nrinfo"/>

namespace ComLibB
{
    public partial class FrmAntiView : Form
    {
        string SQL;
        DataTable dt = null;
        string SqlErr = "";
        //string strReturn = "";
        //int intRowAffected = 0; //변경된 Row 받는 변수

        string FstrYakGuk= "";
        string strSort;

        string FstrWard;
        string FstrPtno;


        clsSpread SP = new clsSpread();

        public FrmAntiView()
        {
            InitializeComponent();
        }

        public FrmAntiView(string strWard, string strPtno = "")
        {
            InitializeComponent();
            FstrWard = strWard;
            FstrPtno = strPtno;
        }

        private void FrmAntiView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)//폼 권한 조회
            {
                this.Close();
                return;
            }

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            //this.Location = new Point(10, 30);

            ssList_Sheet1.Columns.Get(13).Visible = false;
            
            dtpFrDate.Value = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>().AddDays(-14);
            dtpToDate.Value = dtpFrDate.Value.AddDays(14);

            txtJepCode.Text = "";
            txtPanoSearch.Text = "";
            rtxtResult.Rtf = "";

            SP.Spread_All_Clear(ssItem);
            SP.Spread_All_Clear(ssList);

            //항생제 Spread Set
            fn_AntiCodeSet();
            //진료과 Combo Set
            clsVbfunc.SetComboDept(clsDB.DbCon, cboDept, "1", 1);
            //병동 Combo Set
            cboWard.Items.Clear();
            cboWard.Items.Add("**.전체");
            clsVbfunc.SetWardCodeCombo(clsDB.DbCon, cboWard, "2", false, 1);
            cboWard.SelectedIndex = 0;

            if (FstrWard != null)
            {
                if (FstrWard.Trim() != "")
                {
                    for (int i = 0; i < cboWard.Items.Count; i++)
                    {
                        cboWard.SelectedIndex = i;
                        if (FstrWard.Trim() == VB.Left(cboWard.Text, 2).Trim())
                        {
                            cboWard.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }

            if (FstrPtno != null)
            {
                if (FstrPtno.Trim() != "")
                {
                    txtPanoSearch.Text = FstrPtno.Trim();
                }
            }

            btnSearch_Click(btnSearch, e);

            /*
            if (clsPublic.GnJobSabun == 36540 || clsPublic.GnJobSabun == 41710)
            {
                btnTong.Visible = true;
            }
            */
        }

        void fn_AntiCodeSet()
        {
            try
            {
                SQL = "";
                SQL += " SELECT A.SUNEXT, A.SUNAMEK                                                                                 \r";
                SQL += "   FROM ADMIN.BAS_SUN A                                                                               \r";
                SQL += "      , ADMIN.BAS_SUT B                                                                               \r";
                SQL += "  WHERE SUCODE IN (SELECT JEPCODE FROM ADMIN.DRUG_MASTER2 WHERE SUB  IN (02, 07) AND ETCBUN1 = 'Y')    \r";
                SQL += "    AND A.SUNEXT = B.SUCODE                                                                                 \r";
                SQL += "    AND (B.DELDATE IS NULL OR B.DELDATE <> '')                                                              \r";
                SQL += "  ORDER BY  A.SUNEXT                                                                                        \r";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    clsDB.DataTableToSpdRow(dt, ssItem, 0, true);
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            { 
                SQL = "";
                SQL += " SELECT SABUN FROM ADMIN.INSA_MST                  \r";
                SQL += " WHERE SABUN = '" + clsType.User.Sabun.Trim() + "'      \r";
                SQL += "   AND BUSE = '044101'                                  \r";
                SQL += "   AND TOIDAY IS NULL                                   \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    FstrYakGuk = "OK";
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

            rtxtResult.Text = "";
            SP.Spread_All_Clear(ssList);

            try
            {
                SQL = "";
                SQL += " SELECT                                                                                                         \r";
                //SQL += "        ADMIN.FC_OCS_DOCTOR_DRNAME(A.SABUN) DRNAME                     \r";
                SQL += "        (SELECT D1.DRNAME FROM ADMIN.OCS_DOCTOR D1 WHERE D1.SABUN = A.SABUN) AS DRNAME                     \r";
                SQL += "      , TO_CHAR(A.SDATE,'YY/MM/DD')  SDATE                                                                      \r";
                SQL += "      , to_char(A.PANO, 'FM00000000') PANO                                                                      \r";
                SQL += "      , B.SNAME                                                                                                 \r";
                SQL += "      , B.DEPTCODE                                                                                              \r";
                SQL += "      , B.WARDCODE                                                                                              \r";
                SQL += "      , B.ROOMCODE                                                                                              \r";
                //SQL += "      , ADMIN.FC_BAS_USER_USERNAME(a.SSABUN) DRNAME1                   \r";
                SQL += "      , (SELECT D1.DRNAME FROM ADMIN.OCS_DOCTOR D1 WHERE D1.SABUN = A.SSABUN) DRNAME1                      \r";
                SQL += "      , TO_CHAR(A.OKDATE,'YY/MM/DD') OKDATE                                                                     \r";   
                SQL += "      , TO_CHAR(A.EXDATE,'YY/MM/DD') EXDATE                                                                     \r";
                SQL += "      , DECODE ( A.STATE, '1', '승인','2','보류','3','불가','') STATE                                             \r";
                SQL += "      , '[' || TRIM(D.SUCODE) || '] ' || D.ORDERNAMES                                                           \r";
                SQL += "      , TO_CHAR(B.INDATE,'YY/MM/DD') INDATE                                                                     \r";
                SQL += "      , a.ROWID                                                                                                 \r";
                SQL += "   FROM ADMIN.OCS_ANTI_MST    A                                                                            \r";
                SQL += "      , ADMIN.IPD_NEW_MASTER B                                                                            \r";
                SQL += "      , ADMIN.OCS_ORDERCODE   D                                                                            \r";
                SQL += "  WHERE A.SDATE >=TO_DATE('" + dtpFrDate.Text + "','YYYY-MM-DD')                                                \r";
                SQL += "    AND A.SDATE <=TO_DATE('" + dtpToDate.Text + "','YYYY-MM-DD')                                                \r";
                SQL += "    AND A.PANO = B.PANO                                                                                         \r";
                SQL += "    AND A.IPDNO = B.IPDNO                                                                                       \r";
                SQL += "    AND A.ORDERCODE = D.ORDERCODE                                                                               \r";
                if (txtJepCode.Text.Trim() != "")
                {
                    SQL += "    AND D.SUCODE like '%' || '" + txtJepCode.Text + "' || '%'                                               \r";
                }
                if (VB.Left(cboDept.Text, 2) != "**")
                {
                    SQL += "    AND b.DeptCode = '" + VB.Left(cboDept.Text.Trim(), 2) + "'                                              \r";
                }
                if (VB.Left(cboWard.Text, 2) != "**")
                {
                    SQL += "    AND b.WardCode = '" + VB.Left(cboWard.Text.Trim(), 2) + "'                                              \r";
                }
                if (rdoGbn2.Checked == true)
                {
                    SQL += "    AND A.STATE = '1'                                                                                       \r";
                }
                else if (rdoGbn3.Checked == true)
                {
                    SQL += "    AND A.STATE = '2'                                                                                       \r";
                }
                else if (rdoGbn4.Checked == true)
                {
                    SQL += "    AND A.STATE = '3'                                                                                       \r";
                }
                else if (rdoGbn5.Checked == true)
                {
                    SQL += "    AND (A.STATE IS NULL OR A.STATE = '')                                                                   \r";
                }
                if (rdoJW.Checked == true)
                {
                    SQL += "    AND B.ACTDATE IS NULL                                                                                   \r";
                }
                else if (rdoTW.Checked == true)
                {
                    SQL += "    AND B.ACTDATE IS NOT NULL                                                                               \r";
                }
                if (txtPanoSearch.Text.Trim() != "")
                {
                    if (VB.IsNumeric(txtPanoSearch.Text.Trim()))
                    {
                        SQL += "    AND A.PANO = '" + txtPanoSearch.Text.Trim() + "'                                                    \r";
                    }
                    else
                    {
                        SQL += "    AND B.SNAME = '" + txtPanoSearch.Text.Trim() + "'                                                   \r";
                    }
                }
                SQL += "  ORDER BY A.SDATE,a.Pano,a.SuCode                                                                              \r";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    clsDB.DataTableToSpdRow(dt, ssList, 0, true);

                    //ssList_Sheet1.RowCount = dt.Rows.Count;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        
                        //ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                        //ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                        //ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                        //ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                        //ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                        //ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                        //ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                        //ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                        //ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                        //ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                        //ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                        //ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                        //ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                        //ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                        //ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ActDate"].ToString().Trim();






                        if (dt.Rows[i]["STATE"].ToString().Trim() == "불가")
                        {
                            ssList.ActiveSheet.Cells[i, 0, i, ssList.ActiveSheet.ColumnCount - 1].ForeColor = Color.Red;
                        }
                        else if (dt.Rows[i]["STATE"].ToString().Trim() == "보류")
                        {
                            ssList.ActiveSheet.Cells[i, 0, i, ssList.ActiveSheet.ColumnCount - 1].ForeColor = Color.Black;
                        }
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

        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strRowid;
            string strAnti;

            if (e.ColumnHeader == true)
            {
                if (strSort == "" || strSort == "A")
                {
                    ssList.ActiveSheet.AutoSortColumn(e.Column, true, false);
                    strSort = "D";
                }
                else
                {
                    ssList.ActiveSheet.AutoSortColumn(e.Column, false, false);
                    strSort = "A";
                }
                return;
            }

            strRowid = ssList.ActiveSheet.Cells[e.Row, 13].Text.Trim();

            try
            {
                SQL = "";
                SQL += " SELECT PANO                                                        \r";
                SQL += "      , SDATE                                                       \r";
                SQL += "      , REMARK                                                      \r";
                SQL += "      , OKDATE                                                      \r";
                SQL += "      , TO_CHAR(ExDate,'YYYY-MM-DD') EXDATE                         \r";
                SQL += "      , DECODE(STATE, '1','승인', '2','보류','3','불가') STATE      \r";
                SQL += "      , A.SUCODE                                                    \r";
                SQL += "      , TOREMARK                                                    \r";
                SQL += "      , b.SUNAMEK                                                   \r";
                SQL += "   FROM ADMIN.OCS_ANTI_MST A                                   \r";
                SQL += "      , ADMIN.BAS_SUN     B                                   \r";
                SQL += "  WHERE a.ROWID = '" + strRowid + "'                                \r";
                SQL += "    AND A.SUCODE = B.SUNEXT(+)                                      \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strAnti = "";
                    strAnti += "[제한항생제] ---------------------------------------" + "\r";
                    strAnti += "요 청 일 : " + dt.Rows[0]["SDATE"].ToString().Trim() + "\r";
                    strAnti += "승 인 일 : " + dt.Rows[0]["OKDATE"].ToString().Trim() + "\r";
                    strAnti += "종 료 일 : " + dt.Rows[0]["EXDATE"].ToString().Trim() + "\r";
                    strAnti += "약    제 : " + dt.Rows[0]["SUCODE"].ToString().Trim() + " " + dt.Rows[0]["SUNAMEK"].ToString().Trim() + "\r\r";
                    strAnti += "의뢰내용 : " + dt.Rows[0]["Remark"].ToString().Trim() + "\r";
                    strAnti += "☞결    과 : " + dt.Rows[0]["State"].ToString().Trim() + "\r\r";
                    strAnti += "☞회신내용 : " + dt.Rows[0]["TOREMARK"].ToString().Trim() + "\r";

                    rtxtResult.Text = strAnti;

                    //2019-06-24 김해수 이민주썜요청으로 내용도 인쇄 밑에 스프레드에 뿌려주기
                    ssList1.ActiveSheet.Cells[0, 0].Text = dt.Rows[0]["PANO"].ToString().Trim();
                    ssList1.ActiveSheet.Cells[0, 1].Text = ssList.ActiveSheet.Cells[e.Row, 3].Text.Trim();
                    ssList1.ActiveSheet.Cells[0, 2].Text = ssList.ActiveSheet.Cells[e.Row, 4].Text.Trim();
                    ssList1.ActiveSheet.Cells[0, 3].Text = dt.Rows[0]["SDATE"].ToString().Trim();
                    ssList1.ActiveSheet.Cells[0, 4].Text = dt.Rows[0]["OKDATE"].ToString().Trim();
                    ssList1.ActiveSheet.Cells[0, 5].Text = dt.Rows[0]["EXDATE"].ToString().Trim();

                    ssList1.ActiveSheet.Cells[0, 6].Text = dt.Rows[0]["SUCODE"].ToString().Trim() + "\n" + dt.Rows[0]["SUNAMEK"].ToString().Trim();
                    ssList1.ActiveSheet.Cells[0, 7].Text = dt.Rows[0]["Remark"].ToString().Trim();
                    ssList1.ActiveSheet.Cells[0, 8].Text = dt.Rows[0]["State"].ToString().Trim();

                    if(dt.Rows[0]["TOREMARK"].ToString().Trim() == "")
                    {
                        ssList1.ActiveSheet.Cells[0, 9].Text = " ";
                    }
                    else
                    {
                        ssList1.ActiveSheet.Cells[0, 9].Text = dt.Rows[0]["TOREMARK"].ToString().Trim();
                    }
                }
                else
                {
                    rtxtResult.Text = "";
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            ssList.ActiveSheet.SetColumnVisible(12, false);
            ssList.ActiveSheet.SetColumnVisible(13, false);

            string strTitle = "";
            string strTitle1 = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            strTitle = "제한항균제 의뢰 승인 현황";
            strTitle1 = "\r\n\r\n" + "  의뢰일 : " + dtpFrDate.Text + " 부터 " + dtpToDate.Text + " 까지" + VB.Space(38) + "인쇄일자 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "\r\n";
            strTitle1 += VB.Space(1) + "---------------------------------------------------------------------------------------------------------";
            strTitle1 += "\r\n";
            
            strHeader = SP.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SP.setSpdPrint_String(strTitle1, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strFooter = VB.Space(1) + "---------------------------------------------------------------------------------------------------------";
            strFooter += "\r\n" + VB.Space(85) + "포항성모병원";

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 35, 35, 20, 20);
            setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Portrait, FarPoint.Win.Spread.PrintType.All, 0, 0, true, true, true, true, true, false, false);

            SP.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            MessageBox.Show("출력 되었습니다", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ssList.ActiveSheet.SetColumnVisible(12, true);
            ssList.ActiveSheet.SetColumnVisible(13, true);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtJepCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtJepCode.Text != "")
                {
                    btnSearch_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("검색하실 약품코드를 입력하세요!!!", "약품코드 입력 누락", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtJepCode.Focus();
                    return;
                }
            }

        }

        private void ssItem_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            txtJepCode.Text = ssItem.ActiveSheet.Cells[e.Row, 0].Text.Trim();

            btnSearch_Click(btnSearch, e);
        }

        private void ssItem_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                if (strSort == "" || strSort == "A")
                {
                    ssItem.ActiveSheet.AutoSortColumn(e.Column, true, false);
                    strSort = "D";
                }
                else
                {
                    ssItem.ActiveSheet.AutoSortColumn(e.Column, false, false);
                    strSort = "A";
                }
                return;
            }

            ssItem.ActiveSheet.Cells[0, 0, ssItem.ActiveSheet.RowCount - 1, ssItem.ActiveSheet.ColumnCount - 1].BackColor = Color.White;

            ssItem.ActiveSheet.Cells[e.Row, 0, e.Row, ssItem.ActiveSheet.ColumnCount - 1].BackColor = Color.Aqua;
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strPano = "";
            string strSName = "";
            string strWard = "";
            string strRoom = "";
            string strInDate = "";
            string strRowId = "";
            string strStatus = "";

            if (e.RowHeader == true)
            {
                SP.setSpdSort(ssList, e.Column, true);
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }

            strPano = ssList.ActiveSheet.Cells[e.Row, 2].Text;
            strSName = ssList.ActiveSheet.Cells[e.Row, 3].Text;
            strWard = ssList.ActiveSheet.Cells[e.Row, 5].Text;
            strRoom = ssList.ActiveSheet.Cells[e.Row, 6].Text;
            strInDate = ssList.ActiveSheet.Cells[e.Row, 12].Text;
            strRowId = ssList.ActiveSheet.Cells[e.Row, 13].Text;

            if (FstrYakGuk == "OK")//승인
            {
                strStatus = "승인";
            }
            else
            {
                strStatus = ssList.ActiveSheet.Cells[e.Row, 10].Text;
            }

            using (FrmAnti f = new FrmAnti(strPano, strSName, strWard, strRoom, strInDate, strRowId, strStatus))
            {
                f.ShowDialog(this);
            }
        }

        private void btnTong_Click(object sender, EventArgs e)
        {
            using (FrmAntiTong frm = new FrmAntiTong())
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog mDlg = new SaveFileDialog())
            {
                mDlg.InitialDirectory = Application.StartupPath;
                mDlg.Filter = "Excel files (*.xls)|*.xls|Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                mDlg.FilterIndex = 1;
                if (mDlg.ShowDialog() == DialogResult.OK)
                {
                    ssList.SaveExcel(mDlg.FileName,
                    FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
                }
            }
        }

        private void txtPanoSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            txtJepCode.Focus(); 
        }

        private void btnPrint1_Click(object sender, EventArgs e)
        {
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            ssList.ActiveSheet.SetColumnVisible(12, false);
            ssList.ActiveSheet.SetColumnVisible(13, false);

            string strTitle = "";
            string strTitle1 = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            strTitle = "제한항균제 의뢰 내용";
            //strTitle1 = "\r\n\r\n" + "  의뢰일 : " + dtpFrDate.Text + " 부터 " + dtpToDate.Text + " 까지" + VB.Space(38) + "인쇄일자 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "\r\n";
            strTitle1 += VB.Space(1) + "---------------------------------------------------------------------------------------------------------";
            strTitle1 += "\r\n";

            strHeader = SP.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SP.setSpdPrint_String(strTitle1, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 35, 35, 20, 20);
            setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Landscape, FarPoint.Win.Spread.PrintType.All, 0, 0, true, true, true, true, true, true, false);
            SP.setSpdPrint(ssList1, PrePrint, setMargin, setOption, strHeader, strFooter);

            MessageBox.Show("출력 되었습니다", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ssList.ActiveSheet.SetColumnVisible(12, true);
            ssList.ActiveSheet.SetColumnVisible(13, true);

        }
    }
}
