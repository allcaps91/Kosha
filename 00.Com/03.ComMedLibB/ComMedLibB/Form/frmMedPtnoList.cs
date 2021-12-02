using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComMedLibB
{
    /// <summary>
    /// Class Name      : MedOrder
    /// File Name       : frmMedPtnoList.cs
    /// Description     : 환자현황
    /// Author          : 이정현
    /// Create Date     : 2018-05-15
    /// <history> 
    /// 환자현황
    /// </history>
    /// <seealso>
    /// PSMH\Ocs\ipdocs\iorder\mtsiorder\FrmPtnoList.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\Ocs\ipdocs\iorder\mtsiorder.vbp
    /// </vbp>
    /// </summary>
    public partial class frmMedPtnoList : Form
    {
        clsOrdFunction OF = new clsOrdFunction();

        private string GstrGBIO = "";
        private string GstrDeptCode = "";
        private string GstrPrtGb = "";

        public frmMedPtnoList()
        {
            InitializeComponent();
        }

        public frmMedPtnoList(string strGBIO, string strDeptCode)
        {
            InitializeComponent();

            GstrGBIO = strGBIO;
            GstrDeptCode = strDeptCode;
        }

        private void frmMedPtnoList_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ssList_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 0;

            if (GstrGBIO == "I")
            {
                clsVbfunc.SetComboDept(clsDB.DbCon, cboDept, "0", 2);

                GstrPrtGb = "NO";
                rdoSort1.Checked = true;

                if (GstrDeptCode.Trim() == "")
                {
                    cboDept.SelectedIndex = 0;
                }
                else
                {
                    for (int i = 0; i < cboDept.Items.Count; i++)
                    {
                        if (cboDept.Items[i].ToString().Trim() == GstrDeptCode.Trim())
                        {
                            cboDept.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            else
            {
                GstrPrtGb = "NO";

                cboDept.Items.Add(GstrDeptCode.Trim());
                cboDept.SelectedIndex = 0;

                rdoSort0.Checked = true;
            }

            panel1.Visible = false;   
            
            if(clsType.User.IdNumber == "50880" || clsType.User.IdNumber == "41827")
            {
                panICU.Visible = true;
            }
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdoSort2.Checked == false) { return; }

            GetList();
        }

        private void rdoSort_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoSort0.Checked == true || rdoSort1.Checked == true)
            {
                ssList.Visible = false;
            }
            else if (rdoSort2.Checked == true)
            {
                ssList.Visible = true;

                GetList();
            }
        }

        private void GetList()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssList_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     DrName, DrCode";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "     WHERE DrDept1 = '" + cboDept.Text.Trim() + "'  ";
                SQL = SQL + ComNum.VBLF + "         AND Tour <> 'Y' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY PrintRanking ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssList_Sheet1.RowCount = dt.Rows.Count;
                    ssList_Sheet1.Cells[0, 0, ssList_Sheet1.RowCount - 1, ssList_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
                    ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
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
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        void GetData()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string oldRoom = "";
            string strDate = "";
            string strWard1 = "";
            string strWard2 = "";
            int cntTong3 = 0;
            int cntIpd = 0;
            int cntAdm = 0;
            int cntDis = 0;

            int nAddDay = 0;

            double[] nBi = new double[6];

            FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();
            txt.Multiline = true;

            Cursor.Current = Cursors.WaitCursor;

            GstrPrtGb = "NO";
            strDate = clsOrdFunction.GstrBDate;

            ssView_Sheet1.RowCount = 0;

            txtRowHeight.Text = ComNum.SPDROWHT.ToString();

            try
            {
                #region GoSub Tong1_Disp

                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].ColumnSpan = 11;

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "     ▶ 구분별통계";

                GstrPrtGb = "YES";

                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].ColumnSpan = 11;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].CellType = txt;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].HorizontalAlignment = CellHorizontalAlignment.Left;

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "          ◈ 재원현황 : "
                                                                           + "입원-" + cntAdm + VB.Space(11 - VB.Len(cntAdm.ToString()))
                                                                           + "퇴원-" + cntDis + VB.Space(11 - VB.Len(cntDis.ToString()))
                                                                           + "총원-" + cntIpd;

                #endregion

                #region GoSub Tong2_Disp

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     Bi, Count(Bi) AS Cnt ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";

                if (rdoSort0.Checked == true || rdoSort1.Checked == true)
                {
                    if (cboDept.Text.Trim() == "MD")
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE DeptCode IN ('MG', 'MC', 'MP', 'ME', 'MN', 'MR', 'MI') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE DeptCode = '" + VB.Left(cboDept.Text, 2).Trim() + "' ";
                    }
                }
                else if (rdoSort2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "     WHERE DeptCode  = '" + VB.Left(cboDept.Text, 2).Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND DrCode IN (";

                    for (i = 0; i < ssList_Sheet1.RowCount; i++)
                    {
                        if (ssList_Sheet1.Cells[i, 0].BackColor == ComNum.SPSELCOLOR)
                        {
                            SQL = SQL + "'" + ssList_Sheet1.Cells[i, 1].Text.Trim() + "', ";
                        }
                    }

                    SQL = SQL + "'') ";
                }

                //SQL = SQL + ComNum.VBLF + "         AND GBSTS ='0'";
                SQL = SQL + ComNum.VBLF + "         AND (GBSTS = '0' OR OUTDate = TRUNC(SYSDATE))";
                SQL = SQL + ComNum.VBLF + "         AND PANO <>'81000004'";

                //2020-05-07 안정수 추가 
                if(optICU0.Checked == true)
                {
                    
                }
                else if (optICU1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND SUBSTR(ROOMCODE, 1, 2) = '33' ";
                }
                else if (optICU2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND SUBSTR(ROOMCODE, 1, 2) = '35' ";
                }

                SQL = SQL + ComNum.VBLF + "GROUP BY Bi ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (GetBi(dt.Rows[i]["BI"].ToString().Trim()))
                        {
                            case "건강보험": nBi[0] = nBi[0] + VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); break;
                            case "자보": nBi[1] = nBi[1] + VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); break;
                            case "산재": nBi[2] = nBi[2] + VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); break;
                            case "일반": nBi[3] = nBi[3] + VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); break;
                            case "의료급여": nBi[4] = nBi[4] + VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); break;
                            case "기타": nBi[5] = nBi[5] + VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()); break;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].ColumnSpan = 11;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].CellType = txt;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].HorizontalAlignment = CellHorizontalAlignment.Left;

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "          ◈ 재원현황 : "
                                                                           + "건강보험-" + nBi[0] + VB.Space(11 - VB.Len(nBi[0].ToString()))
                                                                           + "자보-" + nBi[1] + VB.Space(11 - VB.Len(nBi[1].ToString()))
                                                                           + "산재-" + nBi[2] + VB.Space(11 - VB.Len(nBi[2].ToString()))
                                                                           + "일반-" + nBi[3] + VB.Space(11 - VB.Len(nBi[3].ToString()))
                                                                           + "의료급여-" + nBi[4] + VB.Space(11 - VB.Len(nBi[4].ToString()))
                                                                           + "기타-" + nBi[5];

                #endregion

                #region GoSub Tong3_Disp

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     WardCode, Count(WardCode) AS Cnt ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";

                if (rdoSort0.Checked == true || rdoSort1.Checked == true)
                {
                    if (cboDept.Text == "MD")
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE DeptCode IN ('MG','MC','MP','ME','MN','MR','MI')  ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE DeptCode = '" + VB.Left(cboDept.Text, 2).Trim() + "' ";
                    }
                }
                else if (rdoSort2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "     WHERE DeptCode = '" + VB.Left(cboDept.Text, 2).Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND DrCode IN (";

                    for (i = 0; i < ssList_Sheet1.RowCount; i++)
                    {
                        if (ssList_Sheet1.Cells[i, 0].BackColor == ComNum.SPSELCOLOR)
                        {
                            SQL = SQL + "'" + ssList_Sheet1.Cells[i, 1].Text.Trim() + "', ";
                        }
                    }

                    SQL = SQL + "'')";
                }

                //2020-05-07 안정수 추가 
                if (optICU0.Checked == true)
                {

                }
                else if (optICU1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND SUBSTR(ROOMCODE, 1, 2) = '33' ";
                }
                else if (optICU2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND SUBSTR(ROOMCODE, 1, 2) = '35' ";
                }

                //SQL = SQL + ComNum.VBLF + "         AND GBSTS ='0'";
                SQL = SQL + ComNum.VBLF + "         AND (GBSTS = '0' OR OUTDate = TRUNC(SYSDATE))";
                SQL = SQL + ComNum.VBLF + "         AND PANO <>'81000004'";
                SQL = SQL + ComNum.VBLF + "GROUP BY WardCode";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    cntTong3 = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i <= 6)
                        {
                            strWard1 = strWard1 + dt.Rows[i]["WardCode"].ToString().Trim() + "-" + dt.Rows[i]["Cnt"].ToString().Trim() + VB.Space(13 - VB.Len(dt.Rows[i]["Cnt"].ToString().Trim()));
                        }
                        else
                        {
                            strWard2 = strWard2 + dt.Rows[i]["WardCode"].ToString().Trim() + "-" + dt.Rows[i]["Cnt"].ToString().Trim() + VB.Space(13 - VB.Len(dt.Rows[i]["Cnt"].ToString().Trim()));
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].ColumnSpan = 11;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].CellType = txt;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].HorizontalAlignment = CellHorizontalAlignment.Left;

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "          ◈ 병동현황 : " + strWard1;

                if (cntTong3 > 7)
                {
                    ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                    ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].ColumnSpan = 11;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].CellType = txt;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].HorizontalAlignment = CellHorizontalAlignment.Left;

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "          ◈ 병동현황 : " + strWard2;
                }

                #endregion

                #region GoSub IpdMaster_Disp

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     M.RoomCode, m.WardCode, m.BedNum, M.SName, TO_CHAR(M.InDate,'YYYY-MM-DD') AS InDate, ";
                SQL = SQL + ComNum.VBLF + "     M.Pano, M.Sex, M.Age, P.Jumin1, P.Jumin2, ";
                SQL = SQL + ComNum.VBLF + "     D.DrName, M.Bi, M.ilsu ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER  M, ";
                SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_PATIENT P, ";
                SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_DOCTOR  D  ";

                if (rdoSort0.Checked == true || rdoSort1.Checked == true)
                {
                    if (cboDept.Text == "MD")
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE M.DeptCode IN ('MG', 'MC', 'MP', 'ME', 'MN', 'MR', 'MI') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE M.DeptCode = '" + VB.Left(cboDept.Text, 2).Trim() + "' ";
                    }
                }
                else if (rdoSort2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "     WHERE M.DeptCode = '" + VB.Left(cboDept.Text, 2).Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND M.DrCode IN (";

                    for (i = 0; i < ssList_Sheet1.RowCount; i++)
                    {
                        if (ssList_Sheet1.Cells[i, 0].BackColor == ComNum.SPSELCOLOR)
                        {
                            SQL = SQL + "'" + ssList_Sheet1.Cells[i, 1].Text.Trim() + "', ";
                        }
                    }

                    SQL = SQL + "'')";
                }

                if (clsType.User.Sabun == "38732")
                {
                    //2019-05-28 박창욱 : 김영민 과장 요구. 원무 퇴원 상관없이 퇴원환자 목록에서 제외
                    SQL = SQL + ComNum.VBLF + "         AND M.OUTDATE IS NULL ";
                }
                else
                {
                    // 전산업무의뢰서 2020-1519
                    //SQL = SQL + ComNum.VBLF + "         AND (M.ACTDATE IS NULL AND M.GBSTS NOT IN ('6')) ";  //전체 쿼리 마추기위해서 아래로 변경함
                    SQL = SQL + ComNum.VBLF + "     AND M.ACTDATE IS NULL                                    ";  //전체 쿼리 마추기위해서 아래로 변경함                    
                    SQL = SQL + ComNum.VBLF + "     AND M.GBSTS IN ('0','2','3','4','5')                     "; //'允가퇴원 제외 심사계요청(2005-08-24)                    
                }
                //SQL = SQL + ComNum.VBLF + "         AND (M.GBSTS = '0' OR M.OUTDate = TRUNC(SYSDATE))";
                SQL = SQL + ComNum.VBLF + "         AND M.PANO <> '81000004' ";
                SQL = SQL + ComNum.VBLF + "         AND M.Pano = P.Pano(+) ";
                SQL = SQL + ComNum.VBLF + "         AND M.DrCode = D.DrCode ";

                //2020-05-07 안정수 추가 
                if (optICU0.Checked == true)
                {

                }
                else if (optICU1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND SUBSTR(M.RoomCode, 1, 2) = '33' ";
                }
                else if (optICU2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND SUBSTR(M.RoomCode, 1, 2) = '35' ";
                }

                if (rdoSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY DECODE(WARDCODE, '33', 1, '35', 2,  '4H', 3, '40', 4, '50', 5, '60', 6, '70', 7, '80', 8, '53', 9, '55', 10, '63', 11, '65', 12, '73', 13, '75', 14, '83', 15, 16), M.RoomCode, M.SName ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY DECODE(WARDCODE, '33', 1, '35', 2,  '4H', 3, '40', 4, '50', 5, '60', 6, '70', 7, '80', 8, '53', 9, '55', 10, '63', 11, '65', 12, '73', 13, '75', 14, '83', 15, 16), M.RoomCode, REGEXP_REPLACE(BEDNUM, '[0-9]') , to_number(REGEXP_REPLACE(BEDNUM, '[^0-9]')), M.DrCode, M.SName ";

                    //정규식 변경
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    cntIpd = dt.Rows.Count;

                    ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                    ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].ColumnSpan = 11;

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "     ▶ 재원자현황";

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                        ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                        if (oldRoom != dt.Rows[i]["ROOMCODE"].ToString().Trim())
                        {
                            if (dt.Rows[i]["WARDCODE"].ToString().Trim() == "32"
                                || dt.Rows[i]["WARDCODE"].ToString().Trim() == "33"
                                || dt.Rows[i]["WARDCODE"].ToString().Trim() == "35")
                            {
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim() + "-" + OF.READ_IPD_BED_NUMBER(dt.Rows[i]["WARDCODE"].ToString().Trim(), dt.Rows[i]["BEDNUM"].ToString().Trim());
                            }
                            else
                            {
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                            }

                            oldRoom = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text;
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = Convert.ToDateTime(dt.Rows[i]["INDATE"].ToString().Trim()).ToString("MM/dd");
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["AGE"].ToString().Trim();

                        if (VB.Val(dt.Rows[i]["AGE"].ToString().Trim()) <= 3)
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = ComFunc.AgeCalcEx_Zero(dt.Rows[i]["JUMIN1"].ToString().Trim() + dt.Rows[i]["JUMIN2"].ToString().Trim(), ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) + "m";
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["DRNAME"].ToString().Trim();

                        if (clsOrdFunction.GstrAnatCode != "OS")
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = (VB.Val(dt.Rows[i]["ILSU"].ToString().Trim()) + 1 + VB.Val(txtIlsu.Text)).ToString();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = VB.Left(Get_IllName(dt.Rows[i]["PANO"].ToString().Trim()), 52);
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["BI"].ToString().Trim();
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = Get_OpDate(dt.Rows[i]["PANO"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                ssView_Sheet1.Cells[1, 0].Text = "          ◈ 재원현황 : "
                                                  + "입원-" + cntAdm + VB.Space(11 - VB.Len(cntAdm.ToString()))
                                                  + "퇴원-" + cntDis + VB.Space(11 - VB.Len(cntDis.ToString()))
                                                  + "총원-" + cntIpd;

                #endregion

                #region GoSub Admission_Disp

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     M.RoomCode, M.SName, TO_CHAR(M.InDate,'YYYY-MM-DD') AS InDate, ";
                SQL = SQL + ComNum.VBLF + "     M.Pano, M.Sex, M.Age, P.Jumin1, P.Jumin2, ";
                SQL = SQL + ComNum.VBLF + "     D.DrName, M.Bi, M.ilsu  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER  M, ";
                SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_PATIENT P, ";
                SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_DOCTOR  D  ";

                if (rdoSort0.Checked == true || rdoSort1.Checked == true)
                {
                    if (cboDept.Text.Trim() == "MD")
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE M.DeptCode IN ('MG', 'MC', 'MP', 'ME', 'MN', 'MR', 'MI') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE M.DeptCode = '" + VB.Left(cboDept.Text, 2).Trim() + "' ";
                    }
                }
                else if (rdoSort2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "     WHERE M.DeptCode = '" + VB.Left(cboDept.Text, 2).Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND M.DrCode IN (";

                    for (i = 0; i < ssList_Sheet1.RowCount; i++)
                    {
                        if (ssList_Sheet1.Cells[i, 0].BackColor == ComNum.SPSELCOLOR)
                        {
                            SQL = SQL + "'" + ssList_Sheet1.Cells[i, 1].Text.Trim() + "', ";
                        }
                    }

                    SQL = SQL + "'') ";
                }

                SQL = SQL + ComNum.VBLF + "         AND TO_CHAR(M.InDate,'YYYY-MM-DD') = '" + strDate + "' ";
                SQL = SQL + ComNum.VBLF + "         AND (M.GBSTS = '0' OR M.OUTDate = TRUNC(SYSDATE))";
                SQL = SQL + ComNum.VBLF + "         AND M.PANO <>'81000004' ";
                SQL = SQL + ComNum.VBLF + "         AND M.Pano = P.Pano(+) ";
                SQL = SQL + ComNum.VBLF + "         AND M.DrCode = D.DrCode ";

                //2020-05-07 안정수 추가 
                if (optICU0.Checked == true)
                {

                }
                else if (optICU1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND SUBSTR(M.RoomCode, 1, 2) = '33' ";
                }
                else if (optICU2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND SUBSTR(M.RoomCode, 1, 2) = '35' ";
                }

                if (rdoSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY DECODE(WARDCODE, '33', 1, '35', 2,  '4H', 3, '40', 4, '50', 5, '60', 6, '70', 7, '80', 8, '53', 9, '55', 10, '63', 11, '65', 12, '73', 13, '75', 14, '83', 15, 16), M.RoomCode, M.SName";
                }
                else if (rdoSort1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY DECODE(WARDCODE, '33', 1, '35', 2,  '4H', 3, '40', 4, '50', 5, '60', 6, '70', 7, '80', 8, '53', 9, '55', 10, '63', 11, '65', 12, '73', 13, '75', 14, '83', 15, 16), M.RoomCode, M.DeptCode, M.DrCode, M.SName ";
                }
                else if (rdoSort2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY DECODE(WARDCODE, '33', 1, '35', 2,  '4H', 3, '40', 4, '50', 5, '60', 6, '70', 7, '80', 8, '53', 9, '55', 10, '63', 11, '65', 12, '73', 13, '75', 14, '83', 15, 16), M.RoomCode, M.DrCode, M.SName ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    cntAdm = dt.Rows.Count;

                    ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                    ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].ColumnSpan = 11;

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "     ▶ 당일 입원";

                    oldRoom = "";

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                        ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                        if (oldRoom != dt.Rows[i]["ROOMCODE"].ToString().Trim())
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                            oldRoom = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = Convert.ToDateTime(dt.Rows[i]["INDATE"].ToString().Trim()).ToString("MM/dd");
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["AGE"].ToString().Trim();

                        if (VB.Val(dt.Rows[i]["AGE"].ToString().Trim()) <= 3)
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = ComFunc.AgeCalcEx_Zero(dt.Rows[i]["JUMIN1"].ToString().Trim() + dt.Rows[i]["JUMIN2"].ToString().Trim(), ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) + "m";
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        //ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = "Ad#" + (VB.Val(dt.Rows[i]["ILSU"].ToString().Trim()) + 1 + VB.Val(txtIlsu.Text));
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = (VB.Val(dt.Rows[i]["ILSU"].ToString().Trim()) + 1 + VB.Val(txtIlsu.Text)).ToString();

                        //2020-05-26 추가 
                        if (clsOrdFunction.GstrAnatCode != "OS")
                        {
                            //ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = (VB.Val(dt.Rows[i]["ILSU"].ToString().Trim()) + 1 + VB.Val(txtIlsu.Text)).ToString();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = VB.Left(Get_IllName(dt.Rows[i]["PANO"].ToString().Trim()), 52);
                            //ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["BI"].ToString().Trim();
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                ssView_Sheet1.Cells[1, 0].Text = "          ◈ 재원현황 : "
                                                  + "입원-" + cntAdm + VB.Space(11 - VB.Len(cntAdm.ToString()))
                                                  + "퇴원-" + cntDis + VB.Space(11 - VB.Len(cntDis.ToString()))
                                                  + "총원-" + cntIpd;

                #endregion

                #region GoSub Discharge_Disp

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     M.RoomCode, M.SName, TO_CHAR(M.InDate,'YYYY-MM-DD') AS InDate, ";
                SQL = SQL + ComNum.VBLF + "     M.Pano, M.Sex, M.Age, P.Jumin1, P.Jumin2, ";
                SQL = SQL + ComNum.VBLF + "     D.DrName, M.Bi, M.ilsu ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER M, ";
                SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_PATIENT P, ";
                SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_DOCTOR D ";

                if (rdoSort0.Checked == true || rdoSort1.Checked == true)
                {
                    if (cboDept.Text.Trim() == "MD")
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE M.DeptCode IN ('MG','MC','MP','ME','MN','MR','MI')  ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE M.DeptCode = '" + VB.Left(cboDept.Text, 2).Trim() + "' ";
                    }
                }
                else if (rdoSort2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "     WHERE M.DeptCode = '" + VB.Left(cboDept.Text, 2).Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND M.DrCode IN (";

                    for (i = 0; i < ssList_Sheet1.RowCount; i++)
                    {
                        if (ssList_Sheet1.Cells[i, 0].BackColor == ComNum.SPSELCOLOR)
                        {
                            SQL = SQL + "'" + ssList_Sheet1.Cells[i, 1].Text.Trim() + "', ";
                        }
                    }

                    SQL = SQL + "'') ";
                }

                SQL = SQL + ComNum.VBLF + "         AND M.OUTDate = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND M.PANO <> '81000004' ";
                SQL = SQL + ComNum.VBLF + "         AND M.Pano = P.Pano(+) ";
                SQL = SQL + ComNum.VBLF + "         AND M.DrCode = D.DrCode ";

                //2020-05-07 안정수 추가 
                if (optICU0.Checked == true)
                {

                }
                else if (optICU1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND SUBSTR(M.RoomCode, 1, 2) = '33' ";
                }
                else if (optICU2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND SUBSTR(M.RoomCode, 1, 2) = '35' ";
                }

                if (rdoSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY DECODE(WARDCODE, '33', 1, '35', 2,  '4H', 3, '40', 4, '50', 5, '60', 6, '70', 7, '80', 8, '53', 9, '55', 10, '63', 11, '65', 12, '73', 13, '75', 14, '83', 15, 16), M.RoomCode, M.SName ";
                }
                else if (rdoSort1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY DECODE(WARDCODE, '33', 1, '35', 2,  '4H', 3, '40', 4, '50', 5, '60', 6, '70', 7, '80', 8, '53', 9, '55', 10, '63', 11, '65', 12, '73', 13, '75', 14, '83', 15, 16), M.DeptCode, M.DrCode, M.SName ";
                }
                else if (rdoSort2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY DECODE(WARDCODE, '33', 1, '35', 2,  '4H', 3, '40', 4, '50', 5, '60', 6, '70', 7, '80', 8, '53', 9, '55', 10, '63', 11, '65', 12, '73', 13, '75', 14, '83', 15, 16), M.DrCode,   M.SName ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    cntDis = dt.Rows.Count;

                    ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                    ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].ColumnSpan = 11;

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "     ▶ 당일 퇴원";

                    oldRoom = "";

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                        ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                        if (oldRoom != dt.Rows[i]["ROOMCODE"].ToString().Trim())
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                            oldRoom = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = Convert.ToDateTime(dt.Rows[i]["INDATE"].ToString().Trim()).ToString("MM/dd");
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["AGE"].ToString().Trim();

                        if (VB.Val(dt.Rows[i]["AGE"].ToString().Trim()) <= 3)
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = ComFunc.AgeCalcEx_Zero(dt.Rows[i]["JUMIN1"].ToString().Trim() + dt.Rows[i]["JUMIN2"].ToString().Trim(), ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) + "m";
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        //ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = "Ad#" + (VB.Val(dt.Rows[i]["ILSU"].ToString().Trim()) + 1 + VB.Val(txtIlsu.Text));
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = (VB.Val(dt.Rows[i]["ILSU"].ToString().Trim()) + 1 + VB.Val(txtIlsu.Text)).ToString();

                        //2020-05-26 추가 
                        if (clsOrdFunction.GstrAnatCode != "OS")
                        {
                            //ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = (VB.Val(dt.Rows[i]["ILSU"].ToString().Trim()) + 1 + VB.Val(txtIlsu.Text)).ToString();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = VB.Left(Get_IllName(dt.Rows[i]["PANO"].ToString().Trim()), 52);
                            //ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["BI"].ToString().Trim();
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                ssView_Sheet1.Cells[1, 0].Text = "          ◈ 재원현황 : "
                                                  + "입원-" + cntAdm + VB.Space(11 - VB.Len(cntAdm.ToString()))
                                                  + "퇴원-" + cntDis + VB.Space(11 - VB.Len(cntDis.ToString()))
                                                  + "총원-" + cntIpd;

                #endregion

                #region GoSub Operation_Disp

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     M.RoomCode, M.SName, TO_CHAR(M.InDate,'YYYY-MM-DD') InDate, ";
                SQL = SQL + ComNum.VBLF + "     M.Pano, M.Sex, M.Age, P.Jumin1, P.Jumin2, ";
                SQL = SQL + ComNum.VBLF + "     D.DrName, M.Bi, O.OpIll, M.ilsu  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER M, ";
                SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_PATIENT P, ";
                SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_DOCTOR D, ";
                SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_MED + "OCS_OPSCHE O  ";
                if (txtIlsu.Text.Trim() != "")
                {
                    nAddDay = int.Parse(VB.Right(txtIlsu.Text, 1));
                    SQL = SQL + ComNum.VBLF + "     WHERE O.OpDate = TO_DATE('" + strDate + "','YYYY-MM-DD') + " + nAddDay + "";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     WHERE O.OpDate = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                }
                SQL = SQL + ComNum.VBLF + "         AND O.Pano = M.Pano ";

                if (rdoSort0.Checked == true || rdoSort1.Checked == true)
                {
                    if (cboDept.Text == "MD")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND M.DeptCode IN ('MG','MC','MP','ME','MN','MR','MI')  ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "         AND M.DeptCode = '" + VB.Left(cboDept.Text, 2).Trim() + "' ";
                    }
                }
                else if (rdoSort2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND M.DeptCode = '" + VB.Left(cboDept.Text, 2).Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND M.DrCode IN (";

                    for (i = 0; i < ssList_Sheet1.RowCount; i++)
                    {
                        if (ssList_Sheet1.Cells[i, 0].BackColor == ComNum.SPSELCOLOR)
                        {
                            SQL = SQL + "'" + ssList_Sheet1.Cells[i, 1].Text.Trim() + "', ";
                        }
                    }

                    SQL = SQL + "'') ";
                }

                SQL = SQL + ComNum.VBLF + "         AND (M.GBSTS = '0' OR M.OUTDate = TRUNC(SYSDATE))";
                SQL = SQL + ComNum.VBLF + "         AND M.PANO <> '81000004' ";
                SQL = SQL + ComNum.VBLF + "         AND M.Pano = P.Pano(+) ";
                SQL = SQL + ComNum.VBLF + "         AND M.DrCode = D.DrCode ";

                //2020-05-07 안정수 추가 
                if (optICU0.Checked == true)
                {

                }
                else if (optICU1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND SUBSTR(M.RoomCode, 1, 2) = '33' ";
                }
                else if (optICU2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND SUBSTR(M.RoomCode, 1, 2) = '35' ";
                }

                if (rdoSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY DECODE(M.WARDCODE, '33', 1, '35', 2,  '4H', 3, '40', 4, '50', 5, '60', 6, '70', 7, '80', 8, '53', 9, '55', 10, '63', 11, '65', 12, '73', 13, '75', 14, '83', 15, 16), M.RoomCode, M.SName ";
                }
                else if (rdoSort1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY DECODE(M.WARDCODE, '33', 1, '35', 2,  '4H', 3, '40', 4, '50', 5, '60', 6, '70', 7, '80', 8, '53', 9, '55', 10, '63', 11, '65', 12, '73', 13, '75', 14, '83', 15, 16), M.DeptCode, M.DrCode, M.SName ";
                }
                else if (rdoSort2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY DECODE(M.WARDCODE, '33', 1, '35', 2,  '4H', 3, '40', 4, '50', 5, '60', 6, '70', 7, '80', 8, '53', 9, '55', 10, '63', 11, '65', 12, '73', 13, '75', 14, '83', 15, 16), M.DrCode, M.SName ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                    ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].ColumnSpan = 11;

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "     ▶ 수술 사항";

                    oldRoom = "";

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                        ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                        if (oldRoom != dt.Rows[i]["ROOMCODE"].ToString().Trim())
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                            oldRoom = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = Convert.ToDateTime(dt.Rows[i]["INDATE"].ToString().Trim()).ToString("MM/dd");
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["AGE"].ToString().Trim();

                        if (VB.Val(dt.Rows[i]["AGE"].ToString().Trim()) <= 3)
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = ComFunc.AgeCalcEx_Zero(dt.Rows[i]["JUMIN1"].ToString().Trim() + dt.Rows[i]["JUMIN2"].ToString().Trim(), ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) + "m";
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = (VB.Val(dt.Rows[i]["ILSU"].ToString().Trim()) + 1 + VB.Val(txtIlsu.Text)).ToString();
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion

                if (rdoC1.Checked == true)
                {
                    if (chkReceive.Checked == true)
                    {
                        #region GoSub ConsultTo_Disp

                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     M.RoomCode, M.SName, TO_CHAR(M.InDate,'YYYY-MM-DD') InDate, ";
                        SQL = SQL + ComNum.VBLF + "     M.Pano, M.Sex, M.Age, P.Jumin1, P.Jumin2, ";
                        SQL = SQL + ComNum.VBLF + "     D.DrName, M.Bi, T.FrDeptCode, M.ILSU ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER M, ";
                        SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_PATIENT P, ";
                        SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_DOCTOR D, ";
                        SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_MED + "OCS_ITRANSFER T ";

                        if (rdoSort0.Checked == true || rdoSort1.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "     WHERE T.ToDeptCode = '" + VB.Left(cboDept.Text, 2).Trim() + "' ";
                        }
                        else if (rdoSort2.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "     WHERE T.ToDeptCode = '" + VB.Left(cboDept.Text, 2).Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND T.ToDrCode IN (";

                            for (i = 0; i < ssList_Sheet1.RowCount; i++)
                            {
                                if (ssList_Sheet1.Cells[i, 0].BackColor == ComNum.SPSELCOLOR)
                                {
                                    SQL = SQL + "'" + ssList_Sheet1.Cells[i, 1].Text.Trim() + "', ";
                                }
                            }

                            SQL = SQL + ComNum.VBLF + "'') ";
                        }

                        SQL = SQL + ComNum.VBLF + "         AND T.GbConfirm <> '*' ";
                        SQL = SQL + ComNum.VBLF + "         AND T.Ptno = M.Pano ";
                        //SQL = SQL + ComNum.VBLF + "         AND M.GBSTS = '0' ";
                        SQL = SQL + ComNum.VBLF + "         AND (M.GBSTS = '0' OR M.OUTDate = TRUNC(SYSDATE))";
                        SQL = SQL + ComNum.VBLF + "         AND M.PANO <> '81000004'";
                        SQL = SQL + ComNum.VBLF + "         AND M.Pano = P.Pano(+) ";
                        SQL = SQL + ComNum.VBLF + "         AND M.DrCode = D.DrCode ";

                        //2020-03-20 안정수, 컨설트 미회신건만 보이도록 추가 (전산의뢰 - 1697 )
                        if (clsOrdFunction.GstrDrCode == "0301")
                        {
                            SQL = SQL + ComNum.VBLF + "     AND T.GBFLAG = '1'";
                            SQL = SQL + ComNum.VBLF + "     AND (T.GbDEL    <> '*'  OR T.GBDEL IS NULL)  ";
                            SQL = SQL + ComNum.VBLF + "     AND (T.GBCONFIRM IN ( ' ','','T') OR T.GBCONFIRM IS NULL )";
                            SQL = SQL + ComNum.VBLF + "     AND (T.GBSEND IS NULL OR T.GBSEND =' ' )";
                        }

                        //2020-05-07 안정수 추가 
                        if (optICU0.Checked == true)
                        {

                        }
                        else if (optICU1.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "         AND SUBSTR(M.RoomCode, 1, 2) = '33' ";
                        }
                        else if (optICU2.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "         AND SUBSTR(M.RoomCode, 1, 2) = '35' ";
                        }

                        if (rdoSort0.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "ORDER BY M.RoomCode, M.SName ";
                        }
                        else if (rdoSort1.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "ORDER BY M.DeptCode, M.DrCode, M.SName ";
                        }
                        else if (rdoSort2.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "ORDER BY M.DrCode, M.SName ";
                        }

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                            ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].ColumnSpan = 11;

                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].HorizontalAlignment = CellHorizontalAlignment.Left;
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "     ▶ Consult 온것";

                            oldRoom = "";

                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                                ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                                if (oldRoom != dt.Rows[i]["ROOMCODE"].ToString().Trim())
                                {
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                                    oldRoom = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                                }

                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = Convert.ToDateTime(dt.Rows[i]["INDATE"].ToString().Trim()).ToString("MM/dd");
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["PANO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["SEX"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["AGE"].ToString().Trim();

                                if (VB.Val(dt.Rows[i]["AGE"].ToString().Trim()) <= 3)
                                {
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = ComFunc.AgeCalcEx_Zero(dt.Rows[i]["JUMIN1"].ToString().Trim() + dt.Rows[i]["JUMIN2"].ToString().Trim(), ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) + "m";
                                }

                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = (VB.Val(dt.Rows[i]["ILSU"].ToString().Trim()) + 1 + VB.Val(txtIlsu.Text)).ToString();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = dt.Rows[i]["FRDEPTCODE"].ToString().Trim() + "부터";
                            }
                        }

                        dt.Dispose();
                        dt = null;

                        #endregion
                    }
                    if (chkSend.Checked == true)
                    {
                        #region GoSub ConsultFr_Disp

                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     M.RoomCode, M.SName, TO_CHAR(M.InDate,'YYYY-MM-DD') InDate, ";
                        SQL = SQL + ComNum.VBLF + "     M.Pano, M.Sex, M.Age, P.Jumin1, P.Jumin2, ";
                        SQL = SQL + ComNum.VBLF + "     D.DrName, M.Bi, T.ToDeptCode, M.ilsu ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER  M, ";
                        SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_PATIENT P, ";
                        SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_DOCTOR D, ";
                        SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_MED + "OCS_ITRANSFER T  ";

                        if (rdoSort1.Checked == true || rdoSort0.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "     WHERE T.FrDeptCode = '" + VB.Left(cboDept.Text, 2).Trim() + "' ";
                        }
                        else if (rdoSort2.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "     WHERE T.FrDeptCode = '" + VB.Left(cboDept.Text, 2).Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND T.FrDrCode IN (";

                            for (i = 0; i < ssList_Sheet1.RowCount; i++)
                            {
                                if (ssList_Sheet1.Cells[i, 0].BackColor == ComNum.SPSELCOLOR)
                                {
                                    SQL = SQL + "'" + ssList_Sheet1.Cells[i, 1].Text.Trim() + "', ";
                                }
                            }

                            SQL = SQL + "'') ";
                        }

                        SQL = SQL + ComNum.VBLF + "         AND T.GbConfirm <> '*' ";
                        SQL = SQL + ComNum.VBLF + "         AND T.Ptno = M.Pano ";
                        //SQL = SQL + ComNum.VBLF + "         AND M.GBSTS = '0' ";
                        SQL = SQL + ComNum.VBLF + "         AND (M.GBSTS = '0' OR M.OUTDate = TRUNC(SYSDATE))";
                        SQL = SQL + ComNum.VBLF + "         AND M.PANO <> '81000004'";
                        SQL = SQL + ComNum.VBLF + "         AND M.Pano = P.Pano(+) ";
                        SQL = SQL + ComNum.VBLF + "         AND M.DrCode = D.DrCode ";

                        //2020-03-20 안정수, 컨설트 미회신건만 보이도록 추가 (전산의뢰 - 1697 )
                        if (clsOrdFunction.GstrDrCode == "0301")
                        {
                            SQL = SQL + ComNum.VBLF + "     AND T.GBFLAG = '1'";
                            SQL = SQL + ComNum.VBLF + "     AND (T.GbDEL    <> '*'  OR T.GBDEL IS NULL)  ";
                            SQL = SQL + ComNum.VBLF + "     AND (T.GBCONFIRM IN ( ' ','','T') OR T.GBCONFIRM IS NULL )";
                            SQL = SQL + ComNum.VBLF + "     AND (T.GBSEND IS NULL OR T.GBSEND =' ' )";
                        }

                        //2020-05-07 안정수 추가 
                        if (optICU0.Checked == true)
                        {

                        }
                        else if (optICU1.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "         AND SUBSTR(M.RoomCode, 1, 2) = '33' ";
                        }
                        else if (optICU2.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "         AND SUBSTR(M.RoomCode, 1, 2) = '35' ";
                        }

                        if (rdoSort0.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "ORDER BY M.RoomCode, M.SName ";
                        }
                        else if (rdoSort1.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "ORDER BY M.DeptCode, M.DrCode, M.SName ";
                        }
                        else if (rdoSort2.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "ORDER BY M.DrCode, M.SName ";
                        }

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                            ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].ColumnSpan = 11;

                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].HorizontalAlignment = CellHorizontalAlignment.Left;
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "     ▶ Consult 보낸것";

                            oldRoom = "";

                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                                ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                                if (oldRoom != dt.Rows[i]["ROOMCODE"].ToString().Trim())
                                {
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                                    oldRoom = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                                }

                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = Convert.ToDateTime(dt.Rows[i]["INDATE"].ToString().Trim()).ToString("MM/dd");
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["PANO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["SEX"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["AGE"].ToString().Trim();

                                if (VB.Val(dt.Rows[i]["AGE"].ToString().Trim()) <= 3)
                                {
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = ComFunc.AgeCalcEx_Zero(dt.Rows[i]["JUMIN1"].ToString().Trim() + dt.Rows[i]["JUMIN2"].ToString().Trim(), ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) + "m";
                                }

                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = (VB.Val(dt.Rows[i]["ILSU"].ToString().Trim()) + 1 + VB.Val(txtIlsu.Text)).ToString();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = dt.Rows[i]["TODEPTCODE"].ToString().Trim() + "(으)로";
                            }
                        }

                        dt.Dispose();
                        dt = null;

                        #endregion
                    }
                }

                #region GoSub Leave_Notice

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     M.RoomCode, M.SName, TO_CHAR(M.InDate,'YYYY-MM-DD') InDate, ";
                SQL = SQL + ComNum.VBLF + "     M.Pano, M.Sex, M.Age, P.Jumin1, P.Jumin2, ";
                SQL = SQL + ComNum.VBLF + "     D.DrName, M.Bi, M.ilsu ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER  M, ";
                SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_PATIENT P, ";
                SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_DOCTOR D  ";

                if (rdoSort1.Checked == true || rdoSort0.Checked == true)
                {
                    if (cboDept.Text == "MD")
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE M.DeptCode IN ('MG','MC','MP','ME','MN','MR','MI')  ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE M.DeptCode = '" + VB.Left(cboDept.Text, 2).Trim() + "' ";
                    }
                }
                else if (rdoSort2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "     WHERE M.DeptCode = '" + VB.Left(cboDept.Text, 2).Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND M.DrCode IN (";

                    for (i = 0; i < ssList_Sheet1.RowCount; i++)
                    {
                        if (ssList_Sheet1.Cells[i, 0].BackColor == ComNum.SPSELCOLOR)
                        {
                            SQL = SQL + "'" + ssList_Sheet1.Cells[i, 1].Text.Trim() + "', ";
                        }
                    }

                    SQL = SQL + "'')";
                }

                //SQL = SQL + ComNum.VBLF + "         AND M.ACTDATE IS NULL ";  //전체 쿼리 마추기위해서 아래로 변경함
                SQL = SQL + ComNum.VBLF + "         AND (M.GBSTS = '0' OR M.OUTDate = TRUNC(SYSDATE))";
                SQL = SQL + ComNum.VBLF + "         AND M.ROUTDATE IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + "         AND M.PANO <>'81000004'";
                SQL = SQL + ComNum.VBLF + "         AND M.Pano = P.Pano(+) ";
                SQL = SQL + ComNum.VBLF + "         AND M.DrCode = D.DrCode ";

                //2020-05-07 안정수 추가 
                if (optICU0.Checked == true)
                {

                }
                else if (optICU1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND SUBSTR(M.RoomCode, 1, 2) = '33' ";
                }
                else if (optICU2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND SUBSTR(M.RoomCode, 1, 2) = '35' ";
                }

                if (rdoSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY M.RoomCode, M.SName ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY M.RoomCode,M.DrCode, M.SName ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    cntIpd = dt.Rows.Count;

                    ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                    ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].ColumnSpan = 11;

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "     ▶ 퇴원예고자현황";

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                        ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                        if (oldRoom != dt.Rows[i]["ROOMCODE"].ToString().Trim())
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                            oldRoom = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text;
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = Convert.ToDateTime(dt.Rows[i]["INDATE"].ToString().Trim()).ToString("MM/dd");
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["AGE"].ToString().Trim();

                        if (VB.Val(dt.Rows[i]["AGE"].ToString().Trim()) <= 3)
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = ComFunc.AgeCalcEx_Zero(dt.Rows[i]["JUMIN1"].ToString().Trim() + dt.Rows[i]["JUMIN2"].ToString().Trim(), ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) + "m";
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["DRNAME"].ToString().Trim();

                        if (clsOrdFunction.GstrAnatCode != "OS")
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = (VB.Val(dt.Rows[i]["ILSU"].ToString().Trim()) + 1 + VB.Val(txtIlsu.Text)).ToString();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = VB.Left(Get_IllName(dt.Rows[i]["PANO"].ToString().Trim()), 52);
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["BI"].ToString().Trim();
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = Get_OpDate(dt.Rows[i]["PANO"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion

                //ssList_Sheet1.Cells[0, ssList_Sheet1.ColumnCount - 1, ssList_Sheet1.RowCount - 1, ssList_Sheet1.ColumnCount - 1].Text = " ";

                Cursor.Current = Cursors.Default;
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
                Cursor.Current = Cursors.Default;
            }
        }

        private string GetBi(string strBi)
        {
            string rtnVal = "";

            switch (strBi)
            {
                case "11": case "12": case "13": case "32": case "41": case "42": case "43": case "44":
                    rtnVal = "건강보험";
                    break;
                case "52": case "55":
                    rtnVal = "자보";
                    break;
                case "31": case "33":
                    rtnVal = "산재";
                    break;
                case "51": case "54":
                    rtnVal = "일반";
                    break;
                case "21": case "22": case "23": case "24":
                    rtnVal = "의료급여";
                    break;
                default:
                    rtnVal = "기타";
                    break;
            }

            return rtnVal;
        }

        private string Get_IllName(string strPtNo)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     O.IllCode, B.IllNameE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IILLS O, " + ComNum.DB_PMPA + "BAS_ILLS B ";
                SQL = SQL + ComNum.VBLF + "     WHERE O.Ptno = '" + strPtNo + "' ";
                //SQL = SQL + ComNum.VBLF + "         AND O.DEPTCODE = '" + cboDept.Text.Trim() + "'";
                //SQL = SQL + ComNum.VBLF + "         AND O.Main = '*' ";
                SQL = SQL + ComNum.VBLF + "         AND O.SEQNO IN ('0', '1') ";
                SQL = SQL + ComNum.VBLF + "         AND O.IllCode = B.IllCode ";
                SQL = SQL + ComNum.VBLF + "         AND B.IllCLASS = '1' ";

                SQL = SQL + ComNum.VBLF + "ORDER BY O.EntDate DESC, O.SEQNO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     O.IllCode, B.IllNameE ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IILLS O, " + ComNum.DB_PMPA + "BAS_ILLS B ";
                    SQL = SQL + ComNum.VBLF + "     WHERE O.Ptno = '" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND O.DEPTCODE = '" + cboDept.Text.Trim() + "'";
                    SQL = SQL + ComNum.VBLF + "         AND O.SEQNO IN('0', '1') ";
                    SQL = SQL + ComNum.VBLF + "         AND O.IllCode = B.IllCode ";
                    SQL = SQL + ComNum.VBLF + "         AND B.IllCLASS = '1' ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY O.EntDate DESC, O.SEQNO ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        return rtnVal;
                    }
                }

                rtnVal = dt.Rows[0]["ILLNAMEE"].ToString().Trim();

                dt.Dispose();
                dt = null;

                return rtnVal;
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
                return rtnVal;
            }
        }

        private string Get_OpDate(string strPtNo)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(OpDate,'MM/DD') AS OpDate";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OPSCHE ";
                SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + strPtNo.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY OpDate DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        rtnVal += dt.Rows[i]["OPDATE"].ToString().Trim() + " ";
                    }
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
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
                return rtnVal;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            int nAddDay = 0;
            string strDate;

            if (GstrPrtGb == "NO") { return; }
            if (ComFunc.MsgBoxQ("검색된 자료를 출력하시겠습니까?", "확인", MessageBoxDefaultButton.Button1)
                == System.Windows.Forms.DialogResult.No) { return; }

            string strFont1 = "";
            string strHead1 = "";
            string strFont2 = "";
            string strHead2 = "";

            for (int i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                ssView.ActiveSheet.Rows[i].BackColor = ComNum.SPDESELCOLOR;
            }

            if (txtIlsu.Text.Trim() != "")
            {
                nAddDay = int.Parse(VB.Right(txtIlsu.Text, 1));
            }

            if (nAddDay > 0)
            {
                strDate = DateTime.Parse(clsOrdFunction.GstrBDate).AddDays(nAddDay).ToShortDateString();
            }
            else
            {
                strDate = clsOrdFunction.GstrBDate;
            }

            //ssView_Sheet1.Columns[7].Width = 200;
            //ssView_Sheet1.Columns[7].Visible = false;
            //ssView_Sheet1.Columns[8].Visible = false;
            //ssView_Sheet1.Columns[9].Visible = false;
            //ssView_Sheet1.Columns[10].Visible = false;
            //ssView_Sheet1.Columns[10].Visible = true;

            strFont1 = "/fn\"맑은 고딕\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"맑은 고딕\" /fz\"11\" /fb1 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + "환자 LIST" + "/f1/n";
            strHead2 = "/l/f2" + "진료과목 : " + VB.Left(cboDept.Text, 2).Trim() + "/f2/n";
            strHead2 += "/l/f2" + VB.Space(11) + "조회일자 : " + strDate
                + VB.Space(5) + "출력시간 : " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-")
                + VB.Space(35) + "Page : /p" + "/f2/n";

            //ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Top = 20;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView_Sheet1.PrintInfo.Margin.Header = 10;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView_Sheet1.PrintInfo.UseSmartPrint = false;
            ssView_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssView_Sheet1.PrintInfo.Preview = false;
            ssView.PrintSheet(0);

            Application.DoEvents();
            Task.Delay(3000);

            //ssView_Sheet1.Columns[7].Width = 250;
            //ssView_Sheet1.Columns[7].Visible = true;
            //ssView_Sheet1.Columns[8].Visible = true;
            //ssView_Sheet1.Columns[9].Visible = true;
            //ssView_Sheet1.Columns[10].Visible = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssList_CellClick(object sender, CellClickEventArgs e)
        {
            if (ssList_Sheet1.Cells[e.Row, 0].BackColor == ComNum.SPDESELCOLOR)
            {
                ssList_Sheet1.Cells[e.Row, 0, e.Row, 1].BackColor = ComNum.SPSELCOLOR;
            }
            else
            {
                ssList_Sheet1.Cells[e.Row, 0, e.Row, 1].BackColor = ComNum.SPDESELCOLOR;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            int nRow = 0;

            for (int i = 0; i < ssView.ActiveSheet.RowCount; i++)
            {
                if (VB.IsNumeric(ssView_Sheet1.Cells[i, 3].Text))
                {
                    nRow = i - 1;
                    break;
                }
                i++;
            }

            if (chk1.Checked == true) 
            {
                ssView.ActiveSheet.Cells[nRow, 8, ssView.ActiveSheet.RowCount - 1, 8].Text = "";
            }

            if (chk2.Checked == true)
            {
                ssView.ActiveSheet.Cells[nRow, 9, ssView.ActiveSheet.RowCount - 1, 9].Text = "";
            }

            if (chk3.Checked == true)
            {
                ssView.ActiveSheet.Cells[nRow, 10, ssView.ActiveSheet.RowCount - 1, 10].Text = "";
            }

            if (chk4.Checked == true)
            {
                ssView.ActiveSheet.Cells[nRow, 6, ssView.ActiveSheet.RowCount - 1, 6].Text = "";
            }
        }

        private void txtRowHeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            //2019-06-24 전산업무의뢰서 2019-741
            bool isSelect = false;

            if (e.KeyChar == (char)13)
            {
                for (int i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    if (ssView_Sheet1.Rows[i].BackColor == ComNum.SPSELCOLOR)
                    {
                        isSelect = true;
                    }
                }

                if (isSelect == true)
                {
                    for (int i = 0; i < ssView_Sheet1.RowCount; i++)
                    {
                        if (ssView_Sheet1.Rows[i].BackColor == ComNum.SPSELCOLOR)
                        {
                            ssView_Sheet1.Rows[i].Height = int.Parse(txtRowHeight.Text);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < ssView_Sheet1.RowCount; i++)
                    {
                        ssView_Sheet1.Rows[i].Height = int.Parse(txtRowHeight.Text);
                    }
                }

                for (int i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    ssView.ActiveSheet.Rows[i].BackColor = ComNum.SPDESELCOLOR;
                }
            }
        }

        private void ssView_CellClick(object sender, CellClickEventArgs e)
        {                      
        }

        private void ssView_MouseUp(object sender, MouseEventArgs e)
        {
            int i = 0;
            FarPoint.Win.Spread.Model.CellRange cr;
            cr = ssView.ActiveSheet.GetSelection(0);
            if (cr == null)
            {
                return;
            }

            //2019-06-24 전산업무의뢰서 2019-741
            for (i = cr.Row; i < cr.Row + cr.RowCount; i++)
            {
                if (ssView.ActiveSheet.Rows[i].BackColor == ComNum.SPSELCOLOR)
                {
                    ssView.ActiveSheet.Rows[i].BackColor = ComNum.SPDESELCOLOR;
                }
                else
                {
                    ssView.ActiveSheet.Rows[i].BackColor = ComNum.SPSELCOLOR;
                }
            }            
        }

        void rdoC0_CheckedChanged(object sender, EventArgs e)
        {
            if(rdoC0.Checked == false)
            {
                return;
            }
            else if (rdoC0.Checked == true)
            {
                panel1.Visible = false;
                chkReceive.Checked = false;
                chkSend.Checked = false;
            }
        }

        void rdoC1_CheckedChanged(object sender, EventArgs e)
        {
            if(rdoC1.Checked == false)
            {
                panel1.Visible = false;
                chkReceive.Checked = false;
                chkSend.Checked = false;
            }
            else if(rdoC1.Checked == true)
            {
                panel1.Visible = true;
                chkReceive.Checked = true;
                chkSend.Checked = true;
            }
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            if(cboDept.Text.Trim() != "MD")
            {
                cboDept.Text = "MD";
            }

            GetData();
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            if (cboDept.Text.Trim() != "MD")
            {
                cboDept.Text = "MD";
            }

            GetData();
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            if (cboDept.Text.Trim() != "MD")
            {
                cboDept.Text = "MD";
            }

            GetData();
        }
    }
}
