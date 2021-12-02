using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    public partial class frmMisuEdiView : Form
    {
        private double[,] FnTotAmt = new double[3, 10];
        private double strTotNCOVAAmt = 0; //국가재난지원금 총액
           

        public frmMisuEdiView()
        {
            InitializeComponent(); 
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMisuEdiView_Load(object sender, EventArgs e)
        {
            ComFunc CF = new ComFunc();

            CF.ComboMonth_Set(cboYYMM, 12);

            Screen_Clear();
        }

        private void Screen_Clear()
        {
            btnSearch.Enabled = true;
            grbIO.Enabled = true;
            grbYYMM.Enabled = true;
            grbJong.Enabled = true;
            lblMsg.Text = "";
            
            //Sheet Clear
            SS1.ActiveSheet.ClearRange(0, 0, SS1_Sheet1.Rows.Count, SS1_Sheet1.ColumnCount, false);
            SS1.ActiveSheet.ColumnHeader.Cells[0, 0, 0, SS1.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            SS1_Sheet1.Rows.Count = 0;
        }

        private void rdoJob1_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoJob1.Checked) { grbYYMM.Text = "접수월구분"; }
        }

        private void rdoJob2_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoJob2.Checked) { grbYYMM.Text = "진료월구분"; }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Screen_Clear();
            cboYYMM.Focus();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = "EDI접수증 조회 및 자료변경";
            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            if (rdoJob1.Checked)
            {
                strHeader += CS.setSpdPrint_String("접수월 : " + cboYYMM.Text + VB.Space(15),
                         new Font("굴림체", 11), clsSpread.enmSpdHAlign.Left, false, true);
            }
            else if (rdoJob2.Checked)
            {
                strHeader += CS.setSpdPrint_String("진료월 : " + cboYYMM.Text + VB.Space(15),
                         new Font("굴림체", 11), clsSpread.enmSpdHAlign.Left, false, true);
            }
            strHeader += CS.setSpdPrint_String("인쇄일자 : " + VB.Now().ToString() + VB.Space(15),
                         new Font("굴림체", 11), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 100, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataTable Dt = null;
            DataTable Dt1 = null;
            DataTable Dt2 = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0, j = 0, nRead = 0, nRow = 0;
            strTotNCOVAAmt = 0;


            string strYYMM = string.Empty;
            string strFDate = string.Empty;
            string strTdate = string.Empty;
            string strJong = string.Empty;
            string strNewData = string.Empty;
            string strOldData = string.Empty;

            long nJangAmt = 0, nDeaAmt = 0;

            ComFunc CF = new ComFunc();
            
            try
            {
                SS1.ActiveSheet.ClearRange(0, 0, SS1_Sheet1.Rows.Count, SS1_Sheet1.ColumnCount, false);
                SS1_Sheet1.Rows.Count = 0;

                btnSearch.Enabled = false;
                grbIO.Enabled = false;
                grbYYMM.Enabled = false;
                grbJong.Enabled = false;

                strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
                strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
                strTdate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);
                strJong = VB.Left(cboJong.Text, 1);

                for (i = 1; i < 3; i++)
                {
                    for (j = 1; j < 10; j++)
                    {
                        FnTotAmt[i, j] = 0;
                    }
                }
                #region SQL Query Set
                //자료조회
                if (strJong != "3")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT Johap,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,JepNo,IpdOpd,mirno edimirno,";
                    SQL += ComNum.VBLF + "        DTbun,YYMM,Week,MirGbn,SamQty,SamTAmt,SamJAmt,SamJangAmt, SAMGAMT, ";
                    SQL += ComNum.VBLF + "        SAMDRUGAMT, SamGAmt_TUBER, SAMUAMT100, SamDaebul,MirNo,DTHU,ROWID ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "EDI_JEPSU ";
                }
                else
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT '6' Johap,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,JepNo,IpdOpd,mirno edimirno, ";
                    SQL += ComNum.VBLF + "        '' DTbun,YYMM,Week,MirGbn,SamQty,SamTAmt,SamTAmt SamJAmt,0 SAMGAMT, 0 SAMDRUGAMT, ";
                    SQL += ComNum.VBLF + "        0 SamGAmt_TUBER, 0 SAMUAMT100, 0 SamJangAmt,0 SamDaebul,MirNo,DTHU,ROWID ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "EDI_JEPSU ";
                }
                if (rdoJob1.Checked)
                {
                    SQL += ComNum.VBLF + "  WHERE JepDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND JepDate<=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                }
                else if (rdoJob2.Checked)
                {
                    SQL += ComNum.VBLF + "  WHERE YYMM = '" + strYYMM + "' ";
                }
                SQL += ComNum.VBLF + "        AND BanDate IS NULL ";
                SQL += ComNum.VBLF + "        AND JepDate IS NOT NULL ";
                switch (strJong)
                {
                    case "1": SQL += ComNum.VBLF + " AND Johap <> '5' "; break;
                    case "2": SQL += ComNum.VBLF + " AND Johap = '5' ";  break;
                    default:
                        break;
                }

                if (rdoIO1.Checked)
                    SQL += ComNum.VBLF + "  AND IpdOpd='O' ";
                else if (rdoIO2.Checked)
                    SQL += ComNum.VBLF + "  AND IpdOpd='I' ";
                else
                    SQL += ComNum.VBLF + "  AND IpdOpd IN ('O','I') ";

                switch (VB.Left(cboA.Text, 1))
                {
                    case "*":   break;
                    case "A":   SQL += ComNum.VBLF + "  AND WEEK IN ('1','2','3','4','5','6') ";        break;
                    default:    SQL += ComNum.VBLF + "  AND WEEK = '" + VB.Left(cboA.Text, 1) + "' ";   break;
                }

                if (VB.Left(cboB.Text, 1) != "*")
                    SQL += ComNum.VBLF + "  AND MIRGBN = '" + VB.Left(cboB.Text, 1) + "' ";

                if (strJong == "3")
                    SQL += ComNum.VBLF + "ORDER BY JepDate,IpdOpd,JepNo ";
                else
                    SQL += ComNum.VBLF + "ORDER BY JepDate,IpdOpd,DTBun,JepNo ";
                #endregion

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                { 
                    ComFunc.MsgBox("조회중 문제가 발생했습니다"); 
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        strNewData = Dt.Rows[i]["Johap"].ToString().Trim();
                        if ((strOldData != strNewData) && strOldData != "")
                            CmdView_SubTotal(ref nRow);

                        nRow += 1;
                        if (nRow > SS1_Sheet1.Rows.Count)
                        {
                            SS1_Sheet1.Rows.Count = nRow;
                        }
                        
                        if (strOldData != strNewData)
                        {
                            switch (strNewData)
                            {
                                case "1": SS1_Sheet1.Cells[nRow - 1, 0].Text = "공단"; break;
                                case "2": SS1_Sheet1.Cells[nRow - 1, 0].Text = "직장"; break; 
                                case "3": SS1_Sheet1.Cells[nRow - 1, 0].Text = "지역"; break;
                                case "5": SS1_Sheet1.Cells[nRow - 1, 0].Text = "보호"; break;
                                case "6": SS1_Sheet1.Cells[nRow - 1, 0].Text = "산재"; break;
                                default:  SS1_Sheet1.Cells[nRow - 1, 0].Text = ""; break;
                            }
                            strOldData = strNewData;
                        }

                        SS1_Sheet1.Cells[nRow - 1, 1].Text = SS1.Text = Dt.Rows[i]["JepDate"].ToString().Trim(); 
                        SS1_Sheet1.Cells[nRow - 1, 2].Text = SS1.Text = Dt.Rows[i]["JepNo"].ToString().Trim();  
                        SS1_Sheet1.Cells[nRow - 1, 3].Text = SS1.Text = Dt.Rows[i]["IpdOpd"].ToString().Trim();

                        switch (Dt.Rows[i]["DTbun"].ToString().Trim())
                        {   
                            case "0": SS1_Sheet1.Cells[nRow - 1, 4].Text = "의과";   break; 
                            case "1": SS1_Sheet1.Cells[nRow - 1, 4].Text = "내과";   break; 
                            case "2":
                                if (Dt.Rows[i]["DTHU"].ToString().Trim() == "Y")
                                {
                                    SS1_Sheet1.Cells[nRow - 1, 4].Text = "HU"; break;
                                }
                                else
                                {
                                    SS1_Sheet1.Cells[nRow - 1, 4].Text = "외과"; break;
                                }
                            case "3": SS1_Sheet1.Cells[nRow - 1, 4].Text = "산소";   break; 
                            case "4": SS1_Sheet1.Cells[nRow - 1, 4].Text = "안이";   break; 
                            case "5": SS1_Sheet1.Cells[nRow - 1, 4].Text = "피비";   break; 
                            case "6": SS1_Sheet1.Cells[nRow - 1, 4].Text = "치과";   break; 
                            case "7": SS1_Sheet1.Cells[nRow - 1, 4].Text = "NP정액"; break; 
                            case "8": SS1_Sheet1.Cells[nRow - 1, 4].Text = "DRG";    break;    
                            default:  SS1_Sheet1.Cells[nRow - 1, 4].Text = ""; break;
                        }

                        SS1_Sheet1.Cells[nRow - 1, 5].Text = SS1.Text = Dt.Rows[i]["YYMM"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow - 1, 6].Text = SS1.Text = Dt.Rows[i]["Week"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow - 1, 7].Text = SS1.Text = Dt.Rows[i]["MirGbn"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow - 1, 8].Text = SS1.Text = VB.Val(Dt.Rows[i]["SamQty"].ToString().Trim()).ToString("###,###,###,##0");  
                        SS1_Sheet1.Cells[nRow - 1, 9].Text = SS1.Text = VB.Val(Dt.Rows[i]["SamTAmt"].ToString().Trim()).ToString("###,###,###,##0");
                        SS1_Sheet1.Cells[nRow - 1, 10].Text = SS1.Text = VB.Val(Dt.Rows[i]["SamJAmt"].ToString().Trim()).ToString("###,###,###,##0");
                        
                        nJangAmt = Convert.ToInt64(Dt.Rows[i]["SamJangAmt"].ToString()); 
                        nDeaAmt = Convert.ToInt64(Dt.Rows[i]["SamDaebul"].ToString());

                        SS1_Sheet1.Cells[nRow - 1, 11].Text = SS1.Text = VB.Format(nDeaAmt, "###,###,###,###,###");
                        SS1_Sheet1.Cells[nRow - 1, 12].Text = SS1.Text = VB.Format(nJangAmt, "###,###,###,###,###");
                        SS1_Sheet1.Cells[nRow - 1, 13].Text = SS1.Text = VB.Val(Dt.Rows[i]["SAMGAMT"].ToString().Trim()).ToString("###,###,###,##0");
                        SS1_Sheet1.Cells[nRow - 1, 14].Text = SS1.Text = VB.Val(Dt.Rows[i]["SAMGAMT_TUBER"].ToString().Trim()).ToString("###,###,###,##0");
                        SS1_Sheet1.Cells[nRow - 1, 15].Text = SS1.Text = VB.Val(Dt.Rows[i]["SAMDRUGAMT"].ToString().Trim()).ToString("###,###,###,##0");
                        SS1_Sheet1.Cells[nRow - 1, 16].Text = SS1.Text = VB.Val(Dt.Rows[i]["SAMUAMT100"].ToString().Trim()).ToString("###,###,###,##0");
                        SS1_Sheet1.Cells[nRow - 1, 17 + 1].Text = Dt.Rows[i]["MirNo"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow - 1, 18 + 1].Text = Dt.Rows[i]["ROWID"].ToString().Trim();

                        //2020-05-04 국가재난지원금(본인부담률)

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT nvl(SUM(EDIBAMT) ,0) EDIBAMT ";
                        SQL = SQL + ComNum.VBLF + "FROM (";
                        SQL = SQL + ComNum.VBLF + "SELECT A.EDIBAMT, A.SEQNO";
                        SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.MIR_INSID A, KOSMOS_PMPA.MIR_INSDTL B";
                        SQL = SQL + ComNum.VBLF + "WHERE A.WRTNO = B.WRTNO";
                        SQL = SQL + ComNum.VBLF + "AND A.EDIMIRNO = '" + Dt.Rows[i]["MirNo"].ToString().Trim() + "'";
                        SQL = SQL + ComNum.VBLF + "AND A.IPDOPD = '" + Dt.Rows[i]["IpdOpd"].ToString() + "'";
                        SQL = SQL + ComNum.VBLF + "AND B.SUNEXT IN ('NCOVA','NCOV-1','NCOV-P2','COV-FLU')";
                        SQL = SQL + ComNum.VBLF + "AND A.DRGCODE is null";
                        SQL = SQL + ComNum.VBLF + "AND A.EDIMIRNO > '0'";
                        //SQL = SQL + ComNum.VBLF + "AND A.UPCNT1 <> '9'";
                        SQL = SQL + ComNum.VBLF + "AND A.WRTNO != '6928057'";
                        SQL = SQL + ComNum.VBLF + "GROUP BY A.EDIBAMT, A.SEQNO";
                        SQL = SQL + ComNum.VBLF + ")";

                        SqlErr = clsDB.GetDataTable(ref Dt1, SQL, clsDB.DbCon);

                        SS1_Sheet1.Cells[nRow - 1, 17].Text = string.Format("{0:###,###,###,##0}", Dt1.Rows[0]["EDIBAMT"]);

                        strTotNCOVAAmt = strTotNCOVAAmt + Convert.ToDouble(Dt1.Rows[0]["EDIBAMT"]); 

                        Dt1.Dispose();
                        Dt1 = null;

                        //2016-06-07
                        if (strJong == "1" || strJong == "2")
                        { 
                            if (Dt.Rows[i]["IpdOpd"].ToString().Trim() == "I")
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " SELECT WRTNO FROM " + ComNum.DB_PMPA + "MIR_INSID ";
                                SQL += ComNum.VBLF + "  WHERE EDIMIRNO = '" + Dt.Rows[i]["EDIMIRNO"].ToString().Trim() + "' ";
                                SQL += ComNum.VBLF + "    AND DEPTCODE1 ='ER' ";
                                SQL += ComNum.VBLF + "    AND UPCNT1 <> '9' ";
                                SqlErr = clsDB.GetDataTable(ref Dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }

                                if (Dt2.Rows.Count > 0)
                                {
                                    SS1_Sheet1.Cells[nRow - 1, 1, nRow - 1, 13].ForeColor = Color.FromArgb(0, 0, 255);
                                }

                                Dt2.Dispose();
                                Dt2 = null;
                            }
                        }

                        //소계,합계에 ADD
                        FnTotAmt[1, 1] += Convert.ToInt16(Dt.Rows[i]["SamQty"].ToString()); 
                        FnTotAmt[1, 2] += Convert.ToInt64(Dt.Rows[i]["SamTAmt"].ToString()); 
                        FnTotAmt[1, 3] += Convert.ToInt64(Dt.Rows[i]["SamJAmt"].ToString());
                        FnTotAmt[1, 4] += nDeaAmt;
                        FnTotAmt[1, 5] += nJangAmt;
                        FnTotAmt[1, 6] += Convert.ToInt64(Dt.Rows[i]["SAMGAMT"].ToString()); 
                        FnTotAmt[1, 7] += Convert.ToInt64(Dt.Rows[i]["SAMGAMT_TUBER"].ToString()); 
                        FnTotAmt[1, 8] += Convert.ToInt64(Dt.Rows[i]["SAMDRUGAMT"].ToString());
                        FnTotAmt[1, 9] += Convert.ToInt64(Dt.Rows[i]["SAMUAMT100"].ToString()); 
                        FnTotAmt[2, 1] += Convert.ToInt64(Dt.Rows[i]["SamQty"].ToString()); 
                        FnTotAmt[2, 2] += Convert.ToInt64(Dt.Rows[i]["SamTAmt"].ToString());
                        FnTotAmt[2, 3] += Convert.ToInt64(Dt.Rows[i]["SamJAmt"].ToString());
                        FnTotAmt[2, 4] += nDeaAmt;
                        FnTotAmt[2, 5] += nJangAmt;
                        FnTotAmt[2, 6] += Convert.ToInt64(Dt.Rows[i]["SAMGAMT"].ToString());         
                        FnTotAmt[2, 7] += Convert.ToInt64(Dt.Rows[i]["SAMGAMT_TUBER"].ToString());   
                        FnTotAmt[2, 8] += Convert.ToInt64(Dt.Rows[i]["SAMDRUGAMT"].ToString());      
                        FnTotAmt[2, 9] += Convert.ToInt64(Dt.Rows[i]["SAMUAMT100"].ToString());           
                    }

                    CmdView_SubTotal(ref nRow);
                    CmdView_Total(nRow);
                }
                else
                {
                    Dt.Dispose();
                    Dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void CmdView_SubTotal(ref int nRow)
        {
            int j = 0;

            nRow += 1;
            if (nRow > SS1_Sheet1.Rows.Count)
            {
                SS1_Sheet1.Rows.Count = nRow;
            }
            SS1_Sheet1.Cells[nRow - 1, 4].Text = "**소계**";

            for (j = 1; j < 10; j++)
            {
                SS1_Sheet1.Cells[nRow - 1, j + 7].Text = VB.Format(FnTotAmt[1, j], "###,###,###,###");
                FnTotAmt[1, j] = 0;
            }
        }

        private void CmdView_Total(int nRow)
        {
            int j = 0;

            nRow += 1;
            if (nRow > SS1_Sheet1.Rows.Count)
            {
                SS1_Sheet1.Rows.Count = nRow;
            }
            SS1_Sheet1.Cells[nRow - 1, 4].Text = "**소계**";
            
            for (j = 1; j < 10; j++)
            {
                SS1_Sheet1.Cells[nRow - 1, j + 7].Text = VB.Format(FnTotAmt[2, j], "###,###,###,###");
                FnTotAmt[2, j] = 0;
            }

            SS1_Sheet1.Cells[nRow - 1, 17].Text = VB.Format(strTotNCOVAAmt, "###,###,###,###"); 
        }

        private void SS1_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Column == 6)
            {
                lblMsg.Text = "";
                lblMsg.Text = "주별구분 (1-5: 1주차~5주차 주별청구, 6.외래 및 퇴원 청구, 7.중간청구)";
            }
            else if (e.Column == 7)
            {
                lblMsg.Text = "";
                lblMsg.Text = "청구구분 (0.일반청구 1.보안(재)청구 2.추가청구 4.NP정액";
            }
            else
            {
                lblMsg.Text = "";
            }
        }

        private void SS1_EditModeOff(object sender, EventArgs e)
        { 
            string strROWID = "";
            string strJong = "";
            string strWeek = "";
            string strMirGbn = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            int strCol = 0;
            int strRow = 0;

            strCol= SS1.ActiveSheet.ActiveColumnIndex;
            strRow = SS1.ActiveSheet.ActiveRowIndex;

            if (strCol != 6 && strCol != 7)
            {
                return;
            }

            strWeek = SS1_Sheet1.Cells[strRow, 6].Text;
            strMirGbn = SS1_Sheet1.Cells[strRow, 7].Text;
            strROWID = SS1_Sheet1.Cells[strRow, 19].Text;
            strJong = VB.Left(cboJong.Text, 1);

            if (string.Compare(strWeek, "1") < 0 || string.Compare(strWeek, "7") > 0)
            {
                ComFunc.MsgBox("주별구분 (1-5: 1주차~5주차 주별청구, 6.외래 및 퇴원 청구, 7.중간청구)");
                return;
            }


            if (string.Compare(strMirGbn, "4") > 0)
            {
                ComFunc.MsgBox("청구구분 (0.일반청구 1.보안(재)청구 2.추가청구 4.NP정액");
                return;
            }
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (strJong == "3")
                {
                    SQL = "UPDATE EDI_SANJEPSU SET Week ='" + strWeek + "',";
                    SQL += " MirGbn = '" + strMirGbn + "'";
                    SQL += " WHERE ROWID = '" + strROWID + "'";
                }
                else
                {
                    SQL = "UPDATE EDI_JEPSU SET Week ='" + strWeek + "',";
                    SQL += " MirGbn = '" + strMirGbn + "'";
                    SQL += " WHERE ROWID = '" + strROWID + "'";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void SS1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (sender == this.SS1)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(o, e.Column); //sort 정렬 기능 
                    return;
                }
                if (e.RowHeader == true)
                {
                    return;
                }
            }
        }
    }
}
