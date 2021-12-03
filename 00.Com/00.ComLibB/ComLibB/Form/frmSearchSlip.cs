using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComLibB
{
    /// <summary> 수가코드별 처방내역 조회 </summary>
    public partial class frmSearchSlip : Form
    {
        string GstrJobName = string.Empty; //global
        string GstrHelpCode = string.Empty; //global

        private ComLibB.frmYAKHelp frmYakHelpX = null;

        /// <summary> 수가코드별 처방내역 조회 </summary>
        public frmSearchSlip()
        {
            InitializeComponent();
            setParam();
        }
        private void setParam()
        {
            this.ssView2.Change += new ChangeEventHandler(Spread_Change);
        }
        private void Spread_Change(object sender, ChangeEventArgs e)
        {
            string strCode = string.Empty;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                if (e.Column != 1)
                {
                    return;
                }

                ssView2_Sheet1.Cells[e.Row, e.Column].Text = ssView2_Sheet1.Cells[e.Row, e.Column].Text.ToUpper();
                strCode = ssView2_Sheet1.Cells[e.Row, e.Column].Text.Trim();

                if (strCode == "")
                {
                    return;
                }

                SQL = " SELECT SuNameK FROM BAS_SUN ";
                SQL += ComNum.VBLF + "WHERE SuNext = '" + strCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView2_Sheet1.Cells[e.Row, 2].Text = dt.Rows[0]["SuNameK"].ToString().Trim();

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
        void frmSearchSlip_Load(object sender, EventArgs e)
        {
           // if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
           // ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            int i = 0;
            string strDrCode = string.Empty;
            string strSabun = string.Empty;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                btnCancel.Enabled = false;
                //btnPrint.Enabled = false;

                SQL = "";
                SQL = " SELECT DEPTCODE,DEPTNAMEK FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                cboDept.Items.Clear();
                cboDept.Items.Add("**.전체과");

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim() + "," + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                cboDept.SelectedIndex = 0;

                cboDrCode.Items.Clear();
                cboDrCode.Items.Add("****.전체의사");
                cboDrCode.SelectedIndex = 0;

                txtPano.Text = "";
                txtSName.Text = "";
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
           // if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
          //  {
          //      return; //권한 확인
          //  }

            int i = 0;
            int intREAD = 0;
            int intEndFLAG = 0;
            int intRow = 0;

            string strDel = string.Empty;
            string strCode = string.Empty;
            string strTemp = string.Empty;

            string strNewData = string.Empty;
            string strOldData = string.Empty;
            string strSuCode = string.Empty;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            DataTable dt1 = null;
            string strSql = string.Empty;

            try
            {
                strSuCode = "";

                for (i = 0; i < ssView2_Sheet1.RowCount; i++)
                {
                    strDel = ssView2_Sheet1.Cells[i, 0].Text;
                    strCode = ssView2_Sheet1.Cells[i, 1].Text;

                    if (strDel != "True" && strCode != "")
                    {
                        strSuCode = strSuCode + "'" + strCode + "',";
                    }
                }

                if (txtDaiCode.Text != "")
                {
                    SQL = "";
                    SQL = "  SELECT SUNEXT FROM BAS_SUN WHERE DAICODE = '" + txtDaiCode.Text.Trim() + "'  ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        return;
                    }

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strSuCode = strSuCode + "'" + dt.Rows[i]["SUNEXT"].ToString().Trim() + "',";
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strSuCode == "")
                {
                    ComFunc.MsgBox("조회할 수가코드가 공란입니다", "확인");
                    btnCancel.Enabled = true;
                    return;
                }

                strSuCode = VB.Left(strSuCode, VB.Len(strSuCode) - 1);

                btnSearch.Enabled = false;

                Cursor.Current = Cursors.WaitCursor;

                strTemp = "";

                if (optJob0.Checked == true || optJob1.Checked == true)
                {
                    SQL = "";
                    SQL = " SELECT  /*+index(a INDEX_IPDNEWSL5)*/ SuNext,Pano,Bi,WardCode,DeptCode,DrCode, ";
                }
                else
                {
                    SQL = "";
                    SQL = " SELECT SuNext,Pano,Bi,WardCode,DeptCode,DrCode, ";
                }

                SQL = SQL + ComNum.VBLF + "       TO_CHAR(Bdate,'yyyy-mm-dd') BIlja,SUM(Qty *Nal) QTY,GbSelf,GbGisul, ";
                SQL = SQL + ComNum.VBLF + "       SUM(BaseAmt)/ COUNT(*) BASEAMT ,SUM(Amt1+Amt2) Amt ";

                if (optJob0.Checked == true || optJob1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " FROM IPD_NEW_SLIP a ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " FROM OPD_SLIP ";
                }

                SQL = SQL + ComNum.VBLF + " WHERE ActDate >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','yyyy-mm-dd') ";
                SQL = SQL + ComNum.VBLF + "   AND SuNext IN (" + strSuCode + ") ";

                if (optBi1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND BUN IN ('11','12') ";
                }

                if (optBi2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND BUN IN ('20') ";
                }

                SQL = SQL + ComNum.VBLF + "   AND BDate >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','yyyy-mm-dd') ";
                SQL = SQL + ComNum.VBLF + "   AND BDate <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','yyyy-mm-dd') ";

                if (optBi1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND BI IN ('11','12','13') ";
                }
                else if (optBi2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND BI IN ('21','22') ";
                }
                else if (optBi3.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND BI IN ('31') ";
                }
                else if (optBi4.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND BI IN ('52','55') ";
                }
                else if (optBi5.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND BI IN ('51') ";
                }
                else if (optBi6.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND BI IN ('11','12','13','21','22') ";
                }

                if (optF1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND GbSelf ='0' ";
                }
                else if (optF2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND GbSelf ='1' ";
                }
                else if (optF3.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND GbSelf ='2' ";
                }
                else if (optF4.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND GbSelf IN ('1','2') ";
                }

                if (VB.Left(cboDept.Text, 2) != "**")
                {
                    SQL = SQL + ComNum.VBLF + " AND DEPTCODE = '" + VB.Left(cboDept.Text, 2) + "'";
                }

                if (VB.Left(cboDrCode.Text, 4) != "****")
                {
                    SQL = SQL + ComNum.VBLF + " AND DRCODE = '" + VB.Left(cboDrCode.Text, 4) + "' ";
                }

                if (txtPano.Text.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND  PANO = '" + txtPano.Text + "'";
                }

                SQL += ComNum.VBLF + " GROUP BY SuNext , Pano, Bi, WardCode, DeptCode, DrCode, ";
                SQL += ComNum.VBLF + "       BDATE, GbSelf,GbGisul ";

                if (chkQty.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " HAVING SUM(Qty *Nal) > 0 ";
                }

                if (optJob3.Checked == true)
                {
                    strSql = VB.Replace(SQL, "OPD_SLIP", "IPD_NEW_SLIP");
                    SQL = SQL + ComNum.VBLF + " UNION ALL " + strSql;
                }
                else
                {
                    if (optSort0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "ORDER BY SuNext,Bdate,Pano ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "ORDER BY SuNext,Pano,Bdate ";
                    }
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView1_Sheet1.RowCount = 0;

                intEndFLAG = 1;
                intRow = 0;
                strOldData = "";
                strNewData = "";

                intREAD = dt.Rows.Count;
                ssView1_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (optSort0.Checked == true)
                    {
                        strNewData = dt.Rows[i]["SuNext"].ToString().Trim();
                        strNewData = strNewData + dt.Rows[i]["BIlja"].ToString().Trim();
                    }
                    else
                    {
                        strNewData = dt.Rows[i]["SuNext"].ToString().Trim();
                        strNewData = strNewData + dt.Rows[i]["Pano"].ToString().Trim();
                    }

                    if (intRow > ssView1_Sheet1.RowCount)
                    {
                        ssView1_Sheet1.RowCount = intRow;
                    }

                    if (optJob3.Checked == true)
                    {
                        ssView1_Sheet1.Cells[intRow, 0].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                        ssView1_Sheet1.Cells[intRow, 1].Text = dt.Rows[i]["Bilja"].ToString().Trim();
                    }

                    if (strNewData != strOldData)
                    {
                        if (optSort0.Checked == true)
                        {
                            ssView1_Sheet1.Cells[intRow, 0].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                            ssView1_Sheet1.Cells[intRow, 1].Text = dt.Rows[i]["Bilja"].ToString().Trim();
                            ssView1_Sheet1.Cells[intRow, 2].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            SnameDisplay(dt, i, intRow);
                        }
                        else
                        {
                            ssView1_Sheet1.Cells[intRow, 0].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                            ssView1_Sheet1.Cells[intRow, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            SnameDisplay(dt, i, intRow);
                            ssView1_Sheet1.Cells[intRow, 3].Text = dt.Rows[i]["BIlja"].ToString().Trim();
                        }

                        strOldData = strNewData;
                    }
                    else
                    {
                        if (optSort0.Checked == true)
                        {
                            ssView1_Sheet1.Cells[intRow, 1].Text = dt.Rows[i]["Bilja"].ToString().Trim();
                            ssView1_Sheet1.Cells[intRow, 2].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            SnameDisplay(dt, i, intRow);
                        }
                        else
                        {
                            ssView1_Sheet1.Cells[intRow, 3].Text = dt.Rows[i]["BIlja"].ToString().Trim();
                        }
                    }

                    ssView1_Sheet1.Cells[intRow, 4].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                    ssView1_Sheet1.Cells[intRow, 5].Text = dt.Rows[i]["Bi"].ToString().Trim();
                    ssView1_Sheet1.Cells[intRow, 6].Text = dt.Rows[i]["DeptCode"].ToString().Trim();

                    SQL = "";
                    SQL = " SELECT DrName FROM BAS_DOCTOR ";
                    SQL = SQL + ComNum.VBLF + "WHERE DrCode = '" + dt.Rows[i]["DrCode"].ToString().Trim() + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count == 0)
                    {
                        dt1.Dispose();
                        dt1 = null;
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        return;
                    }

                    ssView1_Sheet1.Cells[intRow, 7].Text = dt1.Rows[0]["DrName"].ToString().Trim();

                    dt1.Dispose();
                    dt1 = null;

                 //   ssView1_Sheet1.Cells[intRow, 8].Text = VB.Format(dt.Rows[i]["Qty"].ToString().Trim(), "###,##0.0");
                  //  ssView1_Sheet1.Cells[intRow, 10].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                 //   ssView1_Sheet1.Cells[intRow, 11].Text = VB.Format(dt.Rows[i]["BaseAmt"].ToString().Trim(), "###,###,##0");
                //    ssView1_Sheet1.Cells[intRow, 12].Text = VB.Format(dt.Rows[i]["Amt"].ToString().Trim(), "###,###,##0");



                    ssView1_Sheet1.Cells[intRow, 8].Text = String.Format("{0:#,##0}", dt.Rows[i]["Qty"]);
                    ssView1_Sheet1.Cells[intRow, 10].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                    ssView1_Sheet1.Cells[intRow, 11].Text = String.Format("{0:#,##0}", dt.Rows[i]["BaseAmt"]);
                    ssView1_Sheet1.Cells[intRow, 12].Text = String.Format("{0:#,##0}", dt.Rows[i]["Amt"]);



                    intRow += 1;
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

                btnCancel.Enabled = true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        void SnameDisplay(DataTable dt, int i, int intRow)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt1 = null;

            try
            {
                SQL = "";
                SQL = " SELECT Sname FROM BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt1.Rows.Count == 0)
                {
                    dt1.Dispose();
                    dt1 = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (optSort0.Checked == true)
                {
                    ssView1_Sheet1.Cells[intRow, 3].Text = dt1.Rows[0]["Sname"].ToString().Trim();
                }
                else
                {
                    ssView1_Sheet1.Cells[intRow, 2].Text = dt1.Rows[0]["Sname"].ToString().Trim();
                }

                dt1.Dispose();
                dt1 = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            ssView1_Sheet1.RowCount = 0;

            btnCancel.Enabled = false;
            //btnPrint.Enabled = false;
            btnSearch.Enabled = true;
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            string strFont1 = string.Empty;
            string strFont2 = string.Empty;
            string strHead = string.Empty;
            string strHead1 = string.Empty;
            string strHead2 = string.Empty;
            string Headtitle = string.Empty;
            string JobDate = string.Empty;
            string JobMan = string.Empty;

            JobMan = GstrJobName;

            strFont1 = "/fn\"바탕체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs0 "; 
            strFont2 = "/fn\"바탕체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs0";

            strHead = "              수가코드별 처방 내역";

            strHead1 = "/f1" + strHead;
            strHead2 = "/l/f2" + "작업기간 : " + dtpFDate.Value.ToString("yyyy-MM-dd") + " ==> " + dtpTDate.Value.ToString("yyyy-MM-dd") + "/n";
            strHead2 += "/l/f2" + "인쇄일자 : " + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            ssView1_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n/n" + strFont2 + strHead2;
            ssView1_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView1_Sheet1.PrintInfo.Margin.Top = 50;
            ssView1_Sheet1.PrintInfo.Margin.Bottom = 200;
            ssView1_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView1_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView1_Sheet1.PrintInfo.ShowBorder = true;
            ssView1_Sheet1.PrintInfo.ShowColor = false;
            ssView1_Sheet1.PrintInfo.ShowGrid = true;
            ssView1_Sheet1.PrintInfo.ShowShadows = false;
            ssView1_Sheet1.PrintInfo.UseMax = false;
            ssView1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView1.PrintSheet(0);
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {

            int i = 0;
            string strDept = string.Empty;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                strDept = VB.Left(cboDept.Text.Trim(), 2);

                cboDrCode.Items.Clear();

                if (strDept == "")
                {
                    return;
                }

                SQL = " SELECT DRCODE, DRNAME FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE TOUR = 'N' ";

                if (strDept != "**")
                {
                    SQL += ComNum.VBLF + "   AND DRDEPT1  = '" + strDept + "' ";
                }

                SQL += ComNum.VBLF + " ORDER BY PRINTRANKING ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                //if (dt.Rows.Count == 0)
                //{
                //    dt.Dispose();
                //    dt = null;
                //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                //    return;
                //}

                if (dt.Rows.Count > 0)
                {
                    cboDrCode.Items.Add("****.전체");
                }
                else
                {
                    ComFunc.MsgBox("진료과장이 없습니다", "확인");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboDrCode.Items.Add(dt.Rows[i]["DRCODE"].ToString().Trim() + "," + dt.Rows[i]["DRNAME"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                cboDrCode.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        void optSort_CheckedChanged(object sender, EventArgs e)
        {
            if (optSort0.Checked == true)
            {
                ssView1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "발생일자";
                ssView1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "등록번호";
                ssView1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "성  명";
            }
            else if (optSort1.Checked == true)
            {
                ssView1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "등록번호";
                ssView1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "성  명";
                ssView1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "발생일자";
            }
        }

        void ssView2_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
          

            string strCode = string.Empty;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                if (e.Column != 1)
                {
                    return;
                }

                ssView2_Sheet1.Cells[e.Row, e.Column].Text = ssView2_Sheet1.Cells[e.Row, e.Column].Text.ToUpper();
                strCode = ssView2_Sheet1.Cells[e.Row, e.Column].Text.Trim();

                if (strCode == "")
                {
                    return;
                }

                SQL = " SELECT SuNameK FROM BAS_SUN ";
                SQL += ComNum.VBLF + "WHERE SuNext = '" + strCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView2_Sheet1.Cells[e.Row, 2].Text = dt.Rows[0]["SuNameK"].ToString().Trim();

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

        //TODO: FrmYAKHelp svn업데이트 후 frmYAKHelp Show
        void txtDaiCode_DoubleClick(object sender, EventArgs e)
        {
            GstrHelpCode = "";

            //FrmYAKHelp frm = new FrmYAKHelp();
            //frm.Show();

            //if (GstrHelpCode != "")
            //{
            //    txtDaiCode.Text = GstrHelpCode;
            //    txtDaiCode1.Text = ReadDaicodeName(txtDaiCode.Text);
            //    GstrHelpCode = "";
            //    SendKeys.Send("{Tab}");
            //}

            if (frmYakHelpX != null)
            {
                frmYakHelpX.Dispose();
                frmYakHelpX = null;
            }

            frmYakHelpX = new ComLibB.frmYAKHelp();

            frmYakHelpX.rSetHelpCode += new ComLibB.frmYAKHelp.SetHelpCode(GetHelpCode);
            frmYakHelpX.rEventClosed += Frm_rEventClosed;
            frmYakHelpX.ShowDialog();
        }

        private void Frm_rEventClosed()
        {
            //닫는 이벤트 내용
            if (frmYakHelpX != null)
            {
                frmYakHelpX.Dispose();
                frmYakHelpX = null;
            }
        }

        private string Read_DaiCodeName(PsmhDb dbCon, string argCode)
        {
            string rtnVal = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (VB.Val(argCode) == 0)
            {
                rtnVal = "";
                return rtnVal;
            }
            SQL = "SELECT ClassName FROM BAS_CLASS ";
            SQL = SQL + "WHERE ClassCode=" + VB.Val(argCode) + " ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, dbCon);

            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["ClassName"].ToString().Trim();
            }
            else
            {
                rtnVal = "** ERROR ** ";
            }
            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        private void GetHelpCode(string strHelpCode)
        {
            string HelpCode = strHelpCode;

            if (HelpCode != "")
            {
                txtDaiCode.Text = HelpCode;
                txtDaiCode1.Text = Read_DaiCodeName(clsDB.DbCon, txtDaiCode.Text);
            }
        }

        string ReadDaicodeName(string ArgCode)
        {
            string strVal = string.Empty;
            string ArgReturn = string.Empty;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;


            try
            {
                if (VB.Val(ArgCode) == 0)
                {
                    strVal = "";
                    return strVal;
                }


                SQL = "";
                SQL = "SELECT ClassName FROM BAS_CLASS ";
                SQL += ComNum.VBLF + "WHERE ClassCode= '" + VB.Val(ArgCode) + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }
                //if (dt.Rows.Count == 0)
                //{
                //    dt.Dispose();
                //    dt = null;
                //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                //    return;
                //}

                if (dt.Rows.Count > 0)
                {
                    ArgReturn = dt.Rows[0]["ClassName"].ToString().Trim();
                }
                else
                {
                    ArgReturn = "** ERROR **";
                }

                dt.Dispose();
                dt = null;

                strVal = ArgReturn;
                return strVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return strVal;
            }
        }

        void txtDaiCode_Leave(object sender, EventArgs e)
        {
            txtDaiCode.Text = ReadDaicodeName(txtDaiCode.Text);
        }

        void dtpFDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            txtPano.Text = ComFunc.SetAutoZero(txtPano.Text,8);
            txtSName.Text = clsVbfunc.GetPatientName(clsDB.DbCon, txtPano.Text);

            if (txtSName.Text == "")
            {
                ComFunc.MsgBox("해당 등록번호는 없는 번호입니다.", "확인");
                return;
            }
        }

        void dtpTDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }
    }
}
