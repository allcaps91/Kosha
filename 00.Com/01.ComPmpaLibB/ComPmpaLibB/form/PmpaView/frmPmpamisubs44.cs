using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-09-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= d:\psmh\misu\misubs\misubs44.frm" >> frmPmpamisubs44.cs 폼이름 재정의" />

    public partial class frmPmpamisubs44 : Form
    {
        //clsSpread.SpdPrint_Margin setMargin;
        //clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaFunc CPF = new clsPmpaFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        string GstrRetValue = "";

        string FstrYYMM = "";
        string FstrIpdOpd = "";
        //double FnCurrRow = 0;

        public frmPmpamisubs44()
        {
            InitializeComponent();
        }

        public frmPmpamisubs44(string strRetValue)
        {
            InitializeComponent();
            GstrRetValue = strRetValue;
        }

        private void frmPmpamisubs44_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            label1.Text = "";
            SS1_Sheet1.Columns[6].Visible = true;
            SS1_Sheet1.Columns[7].Visible = true;

            FstrYYMM = VB.Left(GstrRetValue, 6);
            FstrIpdOpd = VB.Right(GstrRetValue, 1);

            label1.Text = label1.Text + "(" + VB.Left(FstrYYMM, 4) + "/" + VB.Right(FstrYYMM, 2) + ") ";

            if (FstrIpdOpd == "I")
                this.Text = this.Text + "(입원)";
            else
                this.Text = this.Text + "(외래)";

            Screen_Display();

        }

        private void Screen_Display()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            SS1_Sheet1.RowCount = 0;
            SS1_Sheet1.RowCount = 20;
            try
            {
                //'기존에 등록된 자료를 Display
                SQL = "";
                SQL = "SELECT Tewon,BiGbn,BalAmt,MisuAmt,Remark,ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM MISU_BALJOJENG ";
                SQL = SQL + ComNum.VBLF + "WHERE YYMM='" + FstrYYMM + "' ";
                SQL = SQL + ComNum.VBLF + "  AND IpdOpd='" + FstrIpdOpd + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Tewon,BiGbn ";

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


                SS1_Sheet1.Rows.Count = dt.Rows.Count;
                SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SS1_Sheet1.Cells[i, 0].Text = "";
                    SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Remark"].ToString().Trim();

                    if (Convert.ToDouble(dt.Rows[i]["BalAmt"].ToString().Trim()) != 0)
                    {
                        SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BalAmt"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 3].Text = "1";
                    }
                    else if (Convert.ToDouble(dt.Rows[i]["MisuAmt"].ToString().Trim()) != 0)
                    {
                        SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["MisuAmt"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 3].Text = "2";
                    }
                    else
                    {
                        SS1_Sheet1.Cells[i, 2].Text = "";
                        SS1_Sheet1.Cells[i, 3].Text = "";

                    }
                    SS1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Tewon"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["BiGbn"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 7].Text = "";

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

        private void btnSpace_Click(object sender, EventArgs e)
        {
            SS1_Sheet1.Rows.Add(SS1_Sheet1.RowCount - 1, 1);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Save() == true)
            {
                Screen_Display();
            }
            else
            {
                return;
            }
        }

        private bool Save()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;
            string strDel = "";
            string strRemark = "";
            double nBalAmt = 0;
            double nMisuAmt = 0;
            string strAmtGbn = "";
            string strTewon = "";
            string strBiGbn = "";
            string strROWID = "";
            string strChange = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            for (i = 1; i <= SS1_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
            {
                if (Convert.ToBoolean(SS1_Sheet1.Cells[i - 1, 0].Value) == true)
                {
                    strDel = "1";
                }
                else if (Convert.ToBoolean(SS1_Sheet1.Cells[i - 1, 0].Value) == false)
                {
                    strDel = "0";
                }

                strRemark = VB.RTrim(SS1_Sheet1.Cells[i - 1, 1].Text);
                nBalAmt = VB.Val(SS1_Sheet1.Cells[i - 1, 2].Text);
                strAmtGbn = SS1_Sheet1.Cells[i - 1, 3].Text;
                strTewon = SS1_Sheet1.Cells[i - 1, 4].Text;
                strBiGbn = SS1_Sheet1.Cells[i - 1, 5].Text;
                strROWID = SS1_Sheet1.Cells[i - 1, 6].Text;

                if (strDel != "1")
                {
                    if (nBalAmt != 0)
                    {
                        if (strAmtGbn != "1" && strAmtGbn != "2")
                        {
                            ComFunc.MsgBox(i + 1 + "번줄 금액 구분이 오류입니다", "오류");
                            return rtnVal;
                        }
                    }
                    if (VB.Len(strTewon) != 1 || string.Compare(strTewon, "1") < 0 || string.Compare(strTewon, "3") > 0)
                    {
                        ComFunc.MsgBox(i + 1 + "번줄 금액 구분이 오류입니다", "오류");
                        return rtnVal;
                    }
                    if (VB.Len(strBiGbn) != 1 || string.Compare(strBiGbn, "1") < 0 || string.Compare(strBiGbn, "4") > 0)
                    {
                        ComFunc.MsgBox(i + 1 + "번줄 금액 구분이 오류입니다", "오류");
                        return rtnVal;
                    }
                }
            }

            try
            {
                for (i = 1; i <= SS1_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                {
                    if (Convert.ToBoolean(SS1_Sheet1.Cells[i - 1, 0].Value) == true)
                    {
                        strDel = "1";
                    }
                    else if (Convert.ToBoolean(SS1_Sheet1.Cells[i - 1, 0].Value) == false)
                    {
                        strDel = "0";
                    }

                    strRemark = VB.RTrim(SS1_Sheet1.Cells[i - 1, 1].Text);

                    if (strAmtGbn == "1")
                    {
                        nBalAmt = VB.Val(SS1_Sheet1.Cells[i - 1, 2].Text);
                        nMisuAmt = 0;
                    }
                    else
                    {
                        nBalAmt = 0;
                        nMisuAmt = VB.Val(SS1_Sheet1.Cells[i - 1, 2].Text);
                    }
                    strAmtGbn = SS1_Sheet1.Cells[i - 1, 3].Text;
                    strTewon = SS1_Sheet1.Cells[i - 1, 4].Text;
                    strBiGbn = SS1_Sheet1.Cells[i - 1, 5].Text;
                    strROWID = SS1_Sheet1.Cells[i - 1, 6].Text;
                    strChange = SS1_Sheet1.Cells[i - 1, 7].Text;



                    SQL = "";
                    if (strDel == "1")
                    {
                        if (strROWID != "")
                        {
                            SQL = "DELETE MISU_BALJOJENG WHERE ROWID='" + strROWID + "' ";
                        }
                    }
                    else
                    {
                        if (strROWID == "")
                        {
                            SQL = "INSERT INTO MISU_BALJOJENG (YYMM,IpdOpd,SeqNo,Tewon,BiGbn,";
                            SQL = SQL + ComNum.VBLF + "BalAmt,MisuAmt,Remark,EntSabun,EntDate) VALUES ('";
                            SQL = SQL + ComNum.VBLF + FstrYYMM + "','" + FstrIpdOpd + "'," + i + ",'" + strTewon + "','";
                            SQL = SQL + ComNum.VBLF + strBiGbn + "'," + nBalAmt + "," + nMisuAmt + ",'";
                            SQL = SQL + ComNum.VBLF + strRemark + "'," + clsPublic.GnJobSabun + ",SYSDATE) ";
                        }
                        else
                        {
                            SQL = "UPDATE MISU_BALJOJENG SET ";
                            SQL = SQL + ComNum.VBLF + "SeqNo=" + i + ",";
                            SQL = SQL + ComNum.VBLF + "Tewon='" + strTewon + "',";
                            SQL = SQL + ComNum.VBLF + "BiGbn='" + strBiGbn + "',";
                            SQL = SQL + ComNum.VBLF + "BalAmt=" + nBalAmt + ",";
                            SQL = SQL + ComNum.VBLF + "MisuAmt=" + nMisuAmt + ",";
                            SQL = SQL + ComNum.VBLF + "Remark='" + strRemark + "' ";

                            if (strChange == "Y")
                            {
                                SQL = SQL + ComNum.VBLF + ",EntSabun=" + clsPublic.GnJobSabun + ",";
                                SQL = SQL + ComNum.VBLF + " EntDate=SYSDATE ";
                            }
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID='" + strROWID + "' ";
                        }
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
                //clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
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

        private void SS1_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            switch (e.Column)
            {
                case 2:
                    label1.Text = "조정사유 또는 참고(메모)사항을 입력하세요.";
                    break;
                case 3:
                    label1.Text = "발생미수조정액 또는 미수발생액 조정액을 입력하세요";
                    break;
                case 4:
                    label1.Text = "1.발생미수조정 2.미수발생액 조정";
                    break;
                case 5:
                    label1.Text = "1.퇴원청구 2.중간청구 3.응급입원";
                    break;
                case 6:
                    label1.Text = "1.보험 2.보호 3.산재 4.자보";
                    break;
                default:
                    label1.Text = "";
                    break;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
