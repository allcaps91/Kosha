using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmSupDrstDrugListNew : Form
    {
        private string[] strJong = new string[0];

        public frmSupDrstDrugListNew()
        {
            InitializeComponent();
        }

        private void frmSupDrstDrugListNew_Load(object sender, EventArgs e)
        {
            ////폼 권한 조회
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}

            ////폼 기본값 세팅 등
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            cboGubun.Text = "";
            cboGubun.Items.Clear();
            cboGubun.Items.Add("전체                        00");
            cboGubun.Items.Add("경구약                      10");
            cboGubun.Items.Add("주사약                      20");
            cboGubun.Items.Add("외용약                      31");
            cboGubun.Items.Add("기타외용약                  32");
            cboGubun.Items.Add("안약                        33");
            cboGubun.Items.Add("가글제                      34");
            cboGubun.Items.Add("패취제                      35");
            cboGubun.Items.Add("분무제                      36");
            cboGubun.Items.Add("항문좌제                    37");
            cboGubun.Items.Add("부인과정제                  38");
            cboGubun.Items.Add("마취약                      41");
            cboGubun.Items.Add("방사선용제                  42");
            cboGubun.Items.Add("인공신장관류용제            43");
            cboGubun.Items.Add("투약관련 소모품             44");

            for (int i = 0; i < cboGubun.Items.Count; i++)
            {
                if (VB.Right(cboGubun.Items[i].ToString(), 2) != "00")
                {
                    Array.Resize<string>(ref strJong, strJong.Length + 1);
                    strJong[strJong.Length - 1] = VB.Right(cboGubun.Items[i].ToString(), 2) + "." + VB.Left(cboGubun.Items[i].ToString(), 10).Trim();
                }
            }

            cboGubun.SelectedIndex = 0;

            ssView_Sheet1.RowCount = 0;

            rdoSort0.Checked = true;
            rdoGu0.Checked = true;
        }

        private void rdoSort_CheckedChanged(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.Columns[0, ssView_Sheet1.ColumnCount - 1].Visible = false;

            switch (VB.Right(((RadioButton)sender).Name, 1))
            {
                case "0":
                case "1":
                    ssView_Sheet1.Columns[0].Visible = true;
                    ssView_Sheet1.Columns[1].Visible = true;
                    ssView_Sheet1.Columns[4].Visible = true;
                    ssView_Sheet1.Columns[5].Visible = true;
                    ssView_Sheet1.Columns[6].Visible = true;
                    break;
                case "2":
                    ssView_Sheet1.Columns[0].Visible = true;
                    ssView_Sheet1.Columns[1].Visible = true;
                    ssView_Sheet1.Columns[5].Visible = true;
                    ssView_Sheet1.Columns[6].Visible = true;
                    ssView_Sheet1.Columns[7].Visible = true;
                    break;
                case "3":
                    ssView_Sheet1.Columns[2].Visible = true;
                    ssView_Sheet1.Columns[3].Visible = true;
                    ssView_Sheet1.Columns[4].Visible = true;
                    ssView_Sheet1.Columns[5].Visible = true;
                    ssView_Sheet1.Columns[6].Visible = true;
                    break;
                case "4":
                    ssView_Sheet1.Columns[0].Visible = true;
                    ssView_Sheet1.Columns[2].Visible = true;
                    ssView_Sheet1.Columns[3].Visible = true;
                    ssView_Sheet1.Columns[4].Visible = true;
                    ssView_Sheet1.Columns[7].Visible = true;
                    ssView_Sheet1.Columns[8].Visible = true;
                    ssView_Sheet1.Columns[9].Visible = true;
                    ssView_Sheet1.Columns[10].Visible = true;
                    ssView_Sheet1.Columns[11].Visible = true;
                    break;
                case "5":
                    ssView_Sheet1.Columns[0].Visible = true;
                    ssView_Sheet1.Columns[2].Visible = true;
                    ssView_Sheet1.Columns[3].Visible = true;
                    ssView_Sheet1.Columns[12].Visible = true;
                    ssView_Sheet1.Columns[13].Visible = true;
                    break;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            int k = 0;

            string strGubun = "";
            //string strDrName = "";
            //string strOldName = "";
            string strCheck = "";
            string strRdo = "";
            string strOldGubun = "";
            string strNewGubun = "";

            ssView_Sheet1.RowCount = 0;

            strGubun = VB.Right(cboGubun.Text, 2);
            strCheck = chkOK.Checked == true ? "ENAME" : "HNAME";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     a.SUNEXT, a.JONG, a." + strCheck + ", a.SNAME, a.UNIT, A.HNAME AS HNAME1, ";
                SQL = SQL + ComNum.VBLF + "     B.SUGBJ, A.ENAME AS ENAME1, a.EFFECT, a.JeYak, A.BUNCODE, A.SDATE, B.BAMT";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUGINFO_NEW A, " + ComNum.DB_PMPA + "BAS_SUT B ";
                SQL = SQL + ComNum.VBLF + "     WHERE b.SuCode = a.SuNext ";
                SQL = SQL + ComNum.VBLF + "         AND b.DelDate IS NULL ";

                //구분에서 전체를 클릭할때
                if (strGubun != "00")
                {
                    SQL = SQL + ComNum.VBLF + "         AND a.Jong = '" + strGubun + "' ";
                }

                //원내, 원외, 원내외혼합
                if (rdoGu0.Checked == true)                   //원내
                {
                    SQL = SQL + ComNum.VBLF + "         AND b.SuGbJ NOT IN ('1') ";
                }
                else if (rdoGu1.Checked == true)              //원외
                {
                    SQL = SQL + ComNum.VBLF + "         AND b.SuGbJ = '1' ";
                }

                //정렬
                if (rdoSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY SuNext ";
                    strRdo = "0";
                }
                else if (rdoSort1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY Jong, " + strCheck;
                    strRdo = "1";
                }
                else if (rdoSort2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY BunCode, jong, " + strCheck;
                    strRdo = "2";
                }
                else if (rdoSort3.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY Sname, jong, " + strCheck;
                    strRdo = "3";
                }
                else if (rdoSort5.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY SUNEXT, " + strCheck;
                    strRdo = "5";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY BunCode, Sname, jong," + strCheck;
                    strRdo = "4";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strOldGubun = dt.Rows[0]["JONG"].ToString().Trim();

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                        ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                        switch (strRdo)
                        {
                            case "0":
                            case "1":
                                if (strRdo == "1" && strGubun == "00")
                                {
                                    if (strOldGubun != strNewGubun)
                                    {
                                        for (k = 0; k < strJong.Length; k++)
                                        {
                                            if (strOldGubun == VB.Left(strJong[k], 2))
                                            {
                                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = VB.Mid(strJong[k], 4, strJong[k].Length);
                                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Font = new Font("맑은 고딕", 12F, FontStyle.Bold);

                                                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                                                ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);
                                                break;
                                            }
                                        }
                                    }
                                }

                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i][strCheck].ToString().Trim() + " (" + dt.Rows[i]["Sname"].ToString().Trim() + ")";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["Unit"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["Effect"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["JeYak"].ToString().Trim();

                                strNewGubun = strOldGubun;
                                break;
                            case "2":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i][strCheck].ToString().Trim() + " (" + dt.Rows[i]["Sname"].ToString().Trim() + ")";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["Effect"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["JeYak"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["BunCode"].ToString().Trim();
                                break;
                            case "3":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i][strCheck].ToString().Trim() + " (" + dt.Rows[i]["SuNext"].ToString().Trim() + ")";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["Sname"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["Unit"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["Effect"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["JeYak"].ToString().Trim();
                                break;
                            case "4":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["HNAME1"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();

                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["UNIT"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["BUNCODE"].ToString().Trim();

                                for (k = 0; k < strJong.Length; k++)
                                {
                                    if (dt.Rows[i]["JONG"].ToString().Trim() == VB.Left(strJong[k], 2))
                                    {
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = VB.Mid(strJong[k], 4, strJong[k].Length);
                                        break;
                                    }
                                }

                                switch (dt.Rows[i]["SUGBJ"].ToString().Trim())
                                {
                                    case "1":
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = "원외처방전용";
                                        break;
                                    case "2":
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = "입원처방전용";
                                        break;
                                    case "3":
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = "원내외혼용";
                                        break;
                                    case "4":
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = "원내만전용(입원+외래)";
                                        break;
                                    default:
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = "";
                                        break;
                                }
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = VB.Val(dt.Rows[i]["BAMT"].ToString().Trim()).ToString("###,###,###");

                                if (dt.Rows[i]["SDATE"].ToString().Trim() != "")
                                {
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = Convert.ToDateTime(dt.Rows[i]["SDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                                }
                                break;
                            case "5":
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["Sname"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Text = READ_KEEPGUBUN(dt.Rows[i]["SUNEXT"].ToString().Trim());
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = READ_BRIGHTGUBUN(dt.Rows[i]["SUNEXT"].ToString().Trim());

                                Application.DoEvents();
                                break;
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

        private string READ_KEEPGUBUN(string strSuCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SAVETEMP ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_MASTER2 ";
                SQL = SQL + ComNum.VBLF + "     WHERE JEPCODE = '" + strSuCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["SAVETEMP"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (rtnVal != "") { return rtnVal; }

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     KEEPGUBUN ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_JEP ";
                SQL = SQL + ComNum.VBLF + "     WHERE JEPCODE = '" + strSuCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    switch (dt.Rows[0]["KEEPGUBUN"].ToString().Trim())
                    {
                        case "1": rtnVal = "실온"; break;
                        case "2": rtnVal = "냉장"; break;
                        case "3": rtnVal = "냉동"; break;
                        case "4": rtnVal = "저온"; break;
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

        private string READ_BRIGHTGUBUN(string strSuCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SAVEBRIGHT ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_MASTER2 ";
                SQL = SQL + ComNum.VBLF + "     WHERE JEPCODE = '" + strSuCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["SAVEBRIGHT"].ToString().Trim() == "1")
                    {
                        rtnVal = "○";
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
            //if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strHead1 = "";
            string strFont2 = "";
            string strHead2 = "";

            string strRdoGu = "";
            string strGubun = VB.Right(cboGubun.Text, 2);

            if (rdoGu0.Checked == true)
            {
                strRdoGu = rdoGu0.Text;
            }
            else if (rdoGu1.Checked == true)
            {
                strRdoGu = rdoGu1.Text;
            }
            else if (rdoGu2.Checked == true)
            {
                strRdoGu = rdoGu2.Text;
            }

            strFont1 = "/fn\"맑은 고딕\" /fz\"15\" /fb0 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"맑은 고딕\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + "<< " + VB.Left(cboGubun.Text, 20).Trim() + " >>" + "/f1/n";
            strHead2 = "/l/f2" + strRdoGu + "약품 ☞ 출력일자 : " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + "/f2/n";

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";

            if (rdoSort0.Checked == true || rdoSort1.Checked == true)
            {
                ssView_Sheet1.PrintInfo.ZoomFactor = 0.95f;
            }
            else if (rdoSort4.Checked == true)
            {
                ssView_Sheet1.PrintInfo.ZoomFactor = 0.8f;
            }

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
            ssView_Sheet1.PrintInfo.ShowPrintDialog = true;
            ssView_Sheet1.PrintInfo.Preview = false;
            ssView.PrintSheet(0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
