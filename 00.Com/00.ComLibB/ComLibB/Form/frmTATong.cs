using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmTATong
    /// File Name : frmTATong.cs
    /// Title or Description : TA입원/외래인원 통계
    /// Author : 박창욱
    /// Create Date : 2017-06-01
    /// Update Histroy : 
    ///     06-12 박창욱 : clsDB의 변경에 대응하여 수정
    /// </summary>
    /// <history>  
    /// VB\busanid11.frm(FrmTATong) -> frmTATong.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\busanid\busanid11.frm(FrmTATong)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\basic\busanid\\busanid.vbp
    /// </vbp>
    public partial class frmTATong : Form
    {
        public frmTATong()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 50;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";


            //Print Head 지정
            strFont1 = "/fn\"SYSTEM\" /fz\"20\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strFont2 = "/fn\"SYSTEM\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strHead1 = strHead1 + "/c" + cboDate.SelectedItem.ToString().Trim();

            if (rdoPersonnel.Checked == true)
            {
                if (rdoInpatient.Checked == true)
                {
                    strHead1 = strHead1 + " TA 인원(입원) ";
                    if (rdoSil.Checked == true)
                    {
                        strHead2 = "/n/l/f2" + "인쇄일자 : " + clsPublic.GstrSysDate + " (실인원)";
                    }
                    else
                    {
                        strHead2 = "/n/l/f2" + "인쇄일자 : " + clsPublic.GstrSysDate + " (연인원)";
                    }
                }
                else
                {
                    strHead1 = strHead1 + " TA 인원(외래) ";
                    if (rdoSil.Checked == true)
                    {
                        strHead2 = "/n/l/f2" + "인쇄일자 : " + clsPublic.GstrSysDate + " (실인원)";
                    }
                    else
                    {
                        strHead2 = "/n/l/f2" + "인쇄일자 : " + clsPublic.GstrSysDate + " (연인원)";
                    }
                }
            }
            else
            {
                if (rdoInpatient.Checked == true)
                {
                    strHead1 = strHead1 + " TA 수익(입원) ";
                }
                else
                {
                    strHead1 = strHead1 + " TA 수익(외래) ";
                }
                strHead2 = "/n/l/f2" + "인쇄일자 : " + clsPublic.GstrSysDate;
            }

            //Print Body
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = true;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView.PrintSheet(0);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int nRead = 0;
            int i = 0;
            int j = 0;
            string strYYMM = "";
            string strYYMM1 = "";
            string strYYMM2 = "";
            string strYear = "";
            string strIO = "";
            int[] nSumGa = new int[12];
            int nSumSe = 0;
            int nTotal = 0;
            int nCnt = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            strYYMM1 = VB.Left(cboDate.SelectedItem.ToString().Trim(), 4) + "01";
            strYYMM2 = VB.Left(cboDate.SelectedItem.ToString().Trim(), 4) + "12";
            strYear = VB.Left(cboDate.SelectedItem.ToString().Trim(), 4);
            strIO = "O";
            if (rdoInpatient.Checked == true)
            {
                strIO = "I";
            }

            try
            {
                if (rdoPersonnel.Checked == true)    //인원통계
                {
                    if (rdoSil.Checked == true)  //인원 실인원
                    {
                        SQL = "";
                        SQL = " SELECT";
                        for (i = 1; i < 13; i++)
                        {
                            SQL = SQL + " COUNT(CASE WHEN YYMM = '" + strYear + VB.Format(i, "00") + "' THEN GELCODE END) CNT" + i + ", ";
                        }
                        SQL = SQL + ComNum.VBLF + " MIANAME, GELCODE FROM MIR_TAID A, BAS_MIA B ";
                        SQL = SQL + ComNum.VBLF + " WHERE IPDOPD  = '" + strIO + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND YYMM >= '" + strYYMM1 + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND YYMM <= '" + strYYMM2 + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND TRIM(GELCODE) = TRIM(MIACODE) ";
                        SQL = SQL + ComNum.VBLF + " GROUP BY GELCODE,  MIANAME";
                        SQL = SQL + ComNum.VBLF + " HAVING ( ";
                        strYYMM = strYear + "01";
                        SQL = SQL + ComNum.VBLF + " COUNT(CASE WHEN YYMM = '" + strYYMM + "' THEN GELCODE END) > 0 ";
                        for (i = 2; i < 13; i++)
                        {
                            strYYMM = strYear + VB.Format(i, "00");
                            SQL = SQL + ComNum.VBLF + " OR COUNT(CASE WHEN YYMM = '" + strYYMM + "' THEN GELCODE END) > 0 ";
                        }
                        SQL = SQL + ComNum.VBLF + " ) ORDER BY GELCODE ";
                    }
                    else   //인원 연인원
                    {
                        SQL = "";
                        SQL = " SELECT";
                        for (i = 1; i < 13; i++)
                        {
                            SQL = SQL + ComNum.VBLF + " SUM(CASE WHEN YYMM = '" + strYear + VB.Format(i, "00") + "' THEN JINILSU END) CNT" + i + ", ";
                        }
                        SQL = SQL + ComNum.VBLF + " GELCODE, MIANAME FROM MIR_TAID A, BAS_MIA B ";
                        SQL = SQL + ComNum.VBLF + " WHERE IPDOPD  = '" + strIO + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND YYMM >= '" + strYYMM1 + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND YYMM <= '" + strYYMM2 + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND TRIM(GELCODE) = TRIM(MIACODE) ";
                        SQL = SQL + ComNum.VBLF + " GROUP BY GELCODE, MIANAME  ";
                        SQL = SQL + ComNum.VBLF + " HAVING ( ";
                        strYYMM = strYear + "01";
                        SQL = SQL + ComNum.VBLF + " SUM(CASE WHEN YYMM = '" + strYYMM + "' THEN JINILSU END) > 0 ";
                        for (i = 2; i < 13; i++)
                        {
                            strYYMM = strYear + VB.Format(i, "00");
                            SQL = SQL + ComNum.VBLF + " OR SUM(CASE WHEN YYMM = '" + strYYMM + "' THEN JINILSU END) > 0 ";
                        }
                        SQL = SQL + ComNum.VBLF + " ) ORDER BY GELCODE, MIANAME ";
                    }
                }
                else
                {
                    return;
                }

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nSumSe = 0;
                nTotal = 0;
                for (i = 0; i < 12; i++)
                {
                    nSumGa[i] = 0;
                }

                nRead = dt.Rows.Count;
                ssView_Sheet1.RowCount = nRead;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < nRead - 1; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["MIANAME"].ToString().Trim();
                    for (j = 2; j < 14; j++)
                    {
                        if (dt.Rows[i]["CNT" + (j - 1)].ToString().Trim() != "")
                        {
                            nCnt = Convert.ToInt32(dt.Rows[i]["CNT" + (j - 1)].ToString().Trim());
                        }
                        else
                        {
                            nCnt = 0;
                        }
                        ssView_Sheet1.Cells[i, j - 1].Text = VB.Format(nCnt, "##,##0");
                        nSumSe = nSumSe + nCnt;
                        nSumGa[j - 2] = nSumGa[j - 2] + nCnt;
                    }
                    ssView_Sheet1.Cells[i, 13].Text = VB.Format(nSumSe, "##,##0");
                    nSumSe = 0;
                }

                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "합      계";
                for (i = 1; i < 13; i++)
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, i].Text = VB.Format(nSumGa[i - 1], "##,##0");
                    nTotal = nTotal + nSumGa[i - 1];
                }
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = VB.Format(nTotal, "##,##0 ");

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        private void frmTATong_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int nYY = 0;
            int i = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            nYY = (int)(VB.Val(VB.Left(clsPublic.GstrSysDate, 4)));

            for (i = 1; i < 21; i++)
            {
                cboDate.Items.Add(VB.Format(nYY, "0000년"));
                nYY = nYY - 1;
            }

            cboDate.SelectedIndex = 0;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
