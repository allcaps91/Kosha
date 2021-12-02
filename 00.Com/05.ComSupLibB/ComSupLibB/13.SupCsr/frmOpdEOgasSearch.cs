using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;


namespace ComSupLibB
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-02-23
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\nurse\nrinfo\nrinfo.vbp\Frm공급실EOGas.frm" >> frmEOgasSearch.cs 폼이름 재정의" />

    public partial class frmOpdEOgasSearch : Form
    {
        
        string strDTP = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
        string GstrWardCode = File.Exists(@"c:\cmc\gumebuse.dat") ? File.ReadAllText(@"c:\cmc\gumebuse.dat").Split('.')[0] : clsType.User.BuseCode;
        string GstrWardName = File.Exists(@"c:\cmc\gumebuse.dat") ? File.ReadAllText(@"c:\cmc\gumebuse.dat").Split('.')[1] : clsType.User.BuseCode;
        public frmOpdEOgasSearch()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            //프린트 버튼
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            //if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            strTitle = GstrWardName + "병동 공급실 E.O 가스 조회";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업일자 : " + TxtDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return;//권한 확인
            //}
            ////권한부여 버튼
            //if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            //{
            //    return;//권한 확인
            //}
            //if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            //{
            //    return;//권한 확인
            //}

            if (SAVE() == false)
            {
                return;
            }

            Search();
            CHK_ReqList();
        }

        bool SAVE()
        {

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;
            string strDel = "";
            string strBDATE = "";
            string strRDate = "";
            string strRDate1 = "";
            string strRName = "";
            string strChange = "";
            string strROWID = "";
            double nQty = 0;

            Cursor.Current = Cursors.WaitCursor;

            GstrWardCode = VB.Pstr(ComboWard.Text, ".", 2);
            strBDATE = strDTP;
            strRDate = TxtDate.Text.Trim();

            if (string.Compare(strBDATE, strRDate) > 0)
            {
                ComFunc.MsgBox("등록일이 현재일 보다 작습니다.", "확인");
                return rtnVal;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i <= SS1_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                {
                    strDel = Convert.ToBoolean(SS1_Sheet1.Cells[i, 0].Value) == true ? "1" : "0";
                    strRName = SS1_Sheet1.Cells[i, 2].Text.Trim();
                    nQty = VB.Val(SS1_Sheet1.Cells[i, 3].Text.Replace(",", ""));
                    strRDate1 = SS1_Sheet1.Cells[i, 4].Text.Trim();
                    strChange = SS1_Sheet1.Cells[i, 7].Text.Trim();
                    strROWID = SS1_Sheet1.Cells[i, 8].Text.Trim();

                    SQL = "";
                    if (strROWID == "")
                    {
                        if (strRName != "" && (strDel == "" || strDel == "0"))
                        {
                            if (nQty == 0)
                            {
                                ComFunc.MsgBox("물품 갯수를 입력하세요." + ComNum.VBLF + SqlErr, "확인");
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }

                            SQL = "INSERT INTO KOSMOS_ADM.CSR_REQ_EO (BDATE,BuCode,REQNAME,REQ,JobSabun) ";
                            SQL = SQL + ComNum.VBLF + " VALUES (  TO_DATE('" + strRDate + "','YYYY-MM-DD') , ";
                            SQL = SQL + ComNum.VBLF + " '" + GstrWardCode + "', '" + strRName + "', " + nQty + ", ";
                            SQL = SQL + ComNum.VBLF + "  " + clsType.User.Sabun + "   ) ";
                        }
                    }
                    else
                    {
                        if (strDel == "1")
                        {
                            if (strRDate1 != "")
                            {
                                ComFunc.MsgBox("반환된것은 삭제가 불가능합니다." + ComNum.VBLF + SqlErr, "확인");
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }

                            SQL = "DELETE KOSMOS_ADM.CSR_REQ_EO ";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID + "' ";
                        }
                        else
                        {
                            if (strChange == "Y")
                            {
                                if (strRDate1 != "")
                                {
                                    ComFunc.MsgBox("반환된것은 수정이 불가능합니다." + ComNum.VBLF + SqlErr, "확인");
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }
                                if (nQty == 0)
                                {
                                    ComFunc.MsgBox("물품 갯수를 입력하세요." + ComNum.VBLF + SqlErr, "확인");
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }

                                SQL = "UPDATE KOSMOS_ADM.CSR_REQ_EO SET REQNAME='" + strRName + "', ";
                                SQL = SQL + ComNum.VBLF + " REQ=" + nQty + ", ";
                                //'SQL = SQL & " RDATE=TO_DATE('" & strRdate & "','YYYY - MM - DD') , "
                                SQL = SQL + ComNum.VBLF + " JobSabun=" + clsType.User.Sabun + "  ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID='" + strROWID + "' ";
                            }
                        }
                    }

                    if (SQL != "")
                    {
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("UPDATE시 오류가 발생하였습니다." + ComNum.VBLF + SqlErr, "확인");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }

                    SS1_Sheet1.Cells[i, 0].Text = "";
                }


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

        private void Search()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            SS1_Sheet1.RowCount = 0;
            SS1_Sheet1.RowCount = 50;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate,BUCODE,REQNAME,REQ,TO_CHAR(RDate,'YYYY-MM-DD') RDate,JobSabun,RSabun,ROWID, ";
                SQL = SQL + ComNum.VBLF + "KOSMOS_OCS.FC_BAS_PASS_NAME(JobSabun) AS JobSabun_NAME,KOSMOS_OCS.FC_BAS_PASS_NAME(RSabun) AS RSabun_NAME";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "CSR_REQ_EO ";
                SQL = SQL + ComNum.VBLF + "  WHERE BuCode ='" + GstrWardCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDate =TO_DATE('" + (TxtDate.Text).Trim() + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SS1_Sheet1.Rows.Count = dt.Rows.Count + 50;
                    SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["Rdate"].ToString().Trim() != "")
                        {
                            SS1_Sheet1.Cells[i, 0].Value = true;
                        }

                        SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["REQNAME"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["REQ"].ToString().Replace(",", "");
                        SS1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["RDate"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["JobSabun_NAME"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["RSabun_NAME"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void CHK_ReqList()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            SS2_Sheet1.RowCount = 0;

            GstrWardCode = VB.Pstr(ComboWard.Text, ".", 2);
            if (GstrWardCode == "")
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate, SUM(Req) ReqCnt ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.CSR_REQ_EO ";
                SQL = SQL + ComNum.VBLF + "  WHERE BuCode ='" + GstrWardCode + "' ";
                if (chkEO.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND RDate IS NULL ";
                }
                SQL = SQL + ComNum.VBLF + " GROUP BY BDate ";
                SQL = SQL + ComNum.VBLF + " ORDER BY BDate DESC ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SS2_Sheet1.Rows.Count = dt.Rows.Count;
                    SS2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SS2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDate"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ReqCnt"].ToString().Trim();
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return;//권한 확인
            //}

            Search();
            CHK_ReqList();
        }

        private void frmOpdEOgasSearch_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ComboWard_SET();

            // GstrWardCodeCSR = P(Trim(Frm공급실물품청구.ComboWard.Text), ".", 2)

            SS1_Sheet1.Columns[7].Visible = false;
            SS1_Sheet1.Columns[8].Visible = false;

            TxtDate.Text = strDTP;

            CHK_ReqList();

        }

        private void ComboWard_SET()
        {
            int i = 0;
            //int j = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intIdxCbo = 0;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  BuCode,Name";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BUSE";
                SQL += ComNum.VBLF + "WHERE ORDFLAG = 'Y' ";
                if (GstrWardCode == "033102" || GstrWardCode == "033103")
                {
                    SQL += ComNum.VBLF + "AND BuCode IN ('033102','033103') ";
                }
                else if (GstrWardCode == "033113" || GstrWardCode == "033125")
                {
                    SQL += ComNum.VBLF + "AND BuCode IN ('033113','033125') ";
                }
                else if (GstrWardCode == "033122" || GstrWardCode == "033126")
                {
                    SQL += ComNum.VBLF + "AND BuCode IN ('033122','033126') ";
                }
                else if (GstrWardCode == "033123" || GstrWardCode == "033104")
                {
                    SQL += ComNum.VBLF + "AND BuCode IN ('033123','033104') ";
                }
                else if (GstrWardCode == "033118" || GstrWardCode == "033108")
                {
                    SQL += ComNum.VBLF + "AND BuCode IN ('033118','033108') ";
                }
                else if (GstrWardCode == "055101")
                {
                    SQL += ComNum.VBLF + "AND BuCode IN ('055101','044510','044520','100570') ";
                }
                else if (GstrWardCode == "056102")
                {
                    SQL += ComNum.VBLF + " AND BuCode in ('011101','011102','011103','011104','011105','011106','011109','011110', ";
                    SQL += ComNum.VBLF + " '011111','011112','011113','011114','011117','011116','011124','011129','033140','056102','056104','056101', ";
                    SQL += ComNum.VBLF + " '100130','100070','100090','100150','100110', '011150')            ";
                }
                else if (GstrWardCode != "******")
                {
                    SQL += ComNum.VBLF + "AND BuCode = '" + GstrWardCode + "' ";
                }
                SQL += ComNum.VBLF + "ORDER BY BuCode";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ComboWard.Items.Clear();

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ComboWard.Items.Add(dt.Rows[i]["Name"].ToString().Trim() + "." + dt.Rows[i]["BuCode"].ToString().Trim());
                }

                for (i = 0; i < ComboWard.Items.Count; i++)
                {
                    if (VB.Pstr(ComboWard.Items[i].ToString(), ".", 2) == GstrWardCode)
                    {
                        ComboWard.SelectedIndex = i;
                        ComboWard.Enabled = false;
                        intIdxCbo = i + 1;
                        break;
                    }
                }
                if(intIdxCbo == 0)
                {
                    MessageBox.Show("EO Gas 를 등록 할수 없는 부서입니다. 전산정보팀에 8337 문의 바랍니다");
                    this.Close();
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void SS2_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //권한부여 버튼
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return;//권한 확인
            //}

            TxtDate.Text = SS2_Sheet1.Cells[e.Row, 0].Text;

            Search();
            CHK_ReqList();
        }
    }
}
