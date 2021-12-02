using ComLibB;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


namespace ComPmpaLibB
{
    public partial class frmPmpaIpdResvView : Form
    {
        private string PrintName = "접수증";
        private string FstrJob = "";
        private string FstrRet = "";
        private string FstrDRG = "";
        private string FstrSel = "";
        private string FstrDSC = "";
        private string FstrDeptName = "";
        private string FstrPano = "";
        private string FstrSname = "";
        private string FstrJumin1 = "";
        private string FstrJumin2 = "";
        private string FstrBDATE = "";
        private string FstrDrname = "";
        private string FstrDrCode = "";
        private string FstrBI = "";
        private string FstrSex = "";
        private string FstrRDate = "";
        private string FstrDeptCode = "";
        private int FnAge = 0;

        clsPrint clsPrt = new clsPrint();
        //clsComSup sup = new clsComSup();

        public frmPmpaIpdResvView()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPmpaIpdResvView_Load(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            SetCtrl(cboDept);
            dtpDate1.Text = clsPublic.GstrSysDate;
            dtpDate2.Text = clsPublic.GstrSysDate;
        }

        private void SetCtrl(ComboBox cbo)
        {
            int i = 0;

            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            SS1.ActiveSheet.ClearRange(0, 0, SS1_Sheet1.Rows.Count, SS1_Sheet1.ColumnCount, false);
            SS1_Sheet1.Rows.Count = 0;

            cbo.Items.Clear();
            cbo.Items.Add("**");

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT PrintRanking,DeptCode ";
                SQL = SQL + ComNum.VBLF + "   From " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1 ";
                //TODO : 순환참조 GstrEmrViewDoct
                //SQL = SQL + ComNum.VBLF + "    AND DeptCode IN ( ";
                //SQL = SQL + ComNum.VBLF + "        SELECT DrDept1 FROM BAS_DOCTOR WHERE DrCode IN(" + clsOpdNr.GstrEmrViewDoct + ") ";
                //SQL = SQL + ComNum.VBLF + "        GROUP BY DrDept1 ) ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY PrintRanking ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;

                }

                if (Dt.Rows.Count == 0)
                {
                    cbo.SelectedIndex = 0;
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                for (i = 0; i < Dt.Rows.Count; i++)
                {
                    cbo.Items.Add(Dt.Rows[i]["DeptCode"].ToString().Trim());
                }

                Dt.Dispose();
                Dt = null;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, clsDB.DbCon);
                return;
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Screen_Display();
        }

        private void Screen_Display()
        {
            int i = 0, nRead = 0, nRow = 0;

            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            SS1.ActiveSheet.ClearRange(0, 0, SS1_Sheet1.Rows.Count, SS1_Sheet1.ColumnCount, false);
            SS1_Sheet1.Rows.Count = 0;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT  a.PANO,a.SName, a.DeptCode, a.DrCode, a.WardCode, a.RoomCode, a.INSIL, a.Remark, a.GBSMS, a.GBCHK,a.GbWard, ";
                SQL += ComNum.VBLF + "         b.Tel,b.HPhone,b.Jumin1 || '-' || b.Jumin2 JuminNo,b.Jumin3,a.ROWID,a.GBSPC,a.GBDSC,a.GbDRG,";
                SQL += ComNum.VBLF + "         TO_CHAR(a.REDATE,'YYYY-MM-DD') REDATE,TO_CHAR(a.CDATE,'YYYY-MM-DD') CDATE, TO_CHAR(a.SDATE,'YYYY-MM-DD') SDATE  ";
                SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_RESERVED a, ";
                SQL += ComNum.VBLF + "       " + ComNum.DB_PMPA + "BAS_PATIENT b  ";
                SQL += ComNum.VBLF + " WHERE 1 = 1";
                SQL += ComNum.VBLF + "   AND a.Pano=b.Pano(+) ";
                if (rdoGbn1.Checked == true)
                {
                    SQL += ComNum.VBLF + "  AND SDATE >=TO_DATE('" + dtpDate1.Text + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "  AND SDATE < TO_DATE('" + VB.Format(Convert.ToDateTime(dtpDate2.Text).AddDays(1), "yyyy-MM-dd") + "','YYYY-MM-DD')";
                }
                else
                {
                    SQL += ComNum.VBLF + "  AND ReDATE >=TO_DATE('" + dtpDate1.Text + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "  AND ReDATE <=TO_DATE('" + dtpDate2.Text + "','YYYY-MM-DD')";
                }
                if (cboDept.Text != "" && cboDept.Text != "**")
                {
                    SQL += ComNum.VBLF + "  AND a.DeptCode ='" + VB.Left(cboDept.Text.Trim(), 2) + "' ";
                }

                if (chkDel.Checked == true)
                {
                    SQL += ComNum.VBLF + "   AND GBCHK ='1' ";
                }
                else
                {
                    SQL += ComNum.VBLF + "   AND ( GBCHK IS NULL OR GBCHK <>'1' ) ";
                }
                SQL += ComNum.VBLF + "   ORDER BY SDATE ";


                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                nRead = Dt.Rows.Count;

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;

                }

                if (nRead == 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                for (i = 0; i < nRead; i++)
                {
                    nRow += 1;
                    if (nRow > SS1_Sheet1.Rows.Count)
                    {
                        SS1_Sheet1.Rows.Count = nRow;
                    }
                    SS1_Sheet1.Cells[i, 0].Text = VB.Format(VB.Val(Dt.Rows[i]["PANO"].ToString()), "00000000");
                    SS1_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["SName"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["JuminNo"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["Tel"].ToString().Trim();
                    if (Dt.Rows[i]["HPhone"].ToString().Trim() != "")
                    {
                        SS1_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["HPhone"].ToString().Trim();
                    }
                    SS1_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["SDate"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["ReDate"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["DeptCode"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 8].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, Dt.Rows[i]["DrCode"].ToString().Trim());
                    SS1_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["CDate"].ToString().Trim();

                    if (Dt.Rows[i]["CDate"].ToString().Trim() != "")
                    {
                        SS1_Sheet1.Cells[i, 9].BackColor = Color.FromArgb(128, 255, 128);
                    }
                    SS1_Sheet1.Cells[i, 10].Text = Dt.Rows[i]["Remark"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 11].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 12].Text = Dt.Rows[i]["DrCode"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 13].Text = Dt.Rows[i]["GbDRG"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 14].Text = Dt.Rows[i]["GbSPC"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 15].Text = Dt.Rows[i]["GbDSC"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 16].Text = Dt.Rows[i]["GbWard"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                return;
            }
        }

        private void 입원예약증출력ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BarCode_Print("", "");
        }

        private void 임의DRG입원예약증출력ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BarCode_Print("", "DRG");
        }

        private void 임의선택진료입원예약증출력ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BarCode_Print("", "SPC");
        }

        private void 임의DSC입원예약증출력ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BarCode_Print("", "DSC");
        }

        private void 입원예약증출력입원일선택ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strDate = "";

            BarCode_Print(strDate, "");
        }
        
        private void SS1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0)
            {
                return;
            }

            FstrRet = "";

            FstrRet += SS1_Sheet1.Cells[e.Row, 0].Text.Trim() + "^^";      //등록번호
            FstrRet += SS1_Sheet1.Cells[e.Row, 1].Text.Trim() + "^^";      //환자명
            FstrRet += SS1_Sheet1.Cells[e.Row, 5].Text.Trim() + "^^";      //진료일자
            FstrRet += SS1_Sheet1.Cells[e.Row, 6].Text.Trim() + "^^";      //희망일자
            FstrRet += SS1_Sheet1.Cells[e.Row, 7].Text.Trim() + "^^";      //진료과
            FstrRet += SS1_Sheet1.Cells[e.Row, 8].Text.Trim() + "^^";      //진료의사
            FstrRet += SS1_Sheet1.Cells[e.Row, 12].Text.Trim() + "^^";      //의사코드
            FstrRet += SS1_Sheet1.Cells[e.Row, 10].Text.Trim() + "^^";      //참고사항

        }

        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strRowid = "";
            string strTemp = "";
            string strReDate = "";
            string strPano = "";
            string strDept = "";

            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            strRowid = SS1_Sheet1.Cells[e.Row, 11].Text.Trim();
            strTemp = "등록번호:" + SS1_Sheet1.Cells[e.Row, 0].Text.Trim();
            strTemp += " 성명:" + SS1_Sheet1.Cells[e.Row, 1].Text.Trim();
            strPano = SS1_Sheet1.Cells[e.Row, 0].Text.Trim();

            if (e.Column == 1)
            {
                //TODO : EMR Viewer 실행 함수구현 필요
                //Dim CallTextView As clsCallEmrView
                //Set CallTextView = New clsCallEmrView
                //Call CallTextView.EXECUTE_TextEmrView(strPano, GnJobSabun)
                //Set CallTextView = Nothing
                //Exit Sub
            }
            else if (e.Column == 2)
            {
                strPano = SS1_Sheet1.Cells[e.Row, 0].Text.Trim();
                strDept = SS1_Sheet1.Cells[e.Row, 7].Text.Trim();
                
                //TODO : 순환참조
                //frmMemo f = new frmMemo(strPano, strDept);
                //f.ShowDialog();
                
                
                return;
            }
            else
            {
                if (strRowid != "")
                {
                    if (e.Column == 6)
                    {
                        if (ComFunc.MsgBoxQ(strTemp + ComNum.VBLF + "선택한 자료의 입원희망일을 변경하시겠습니까?", "취소확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                        {
                            strReDate = "";
                            strReDate = VB.InputBox("희망 입원일을 변경하십시오. 예)2999-12-31", "희망예약일변경", clsPublic.GstrSysDate);

                            if (VB.Len(strReDate) == 10)
                            {

                                clsDB.setBeginTran(clsDB.DbCon);

                                SQL = "";
                                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "IPD_RESERVED ";
                                SQL += ComNum.VBLF + "    SET ReDate = TO_DATE('" + strReDate + "','YYYY-MM-DD') ";
                                SQL += ComNum.VBLF + "  WHERE ROWID ='" + strRowid + "' ";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                    return;
                                }

                                clsDB.setCommitTran(clsDB.DbCon);
                                Screen_Display();
                            }
                            else
                            {
                                ComFunc.MsgBox("날짜형식을 확인하세요.", "변경실패");
                            }
                        }
                    }
                    else
                    {
                        if (ComFunc.MsgBoxQ(strTemp + ComNum.VBLF + "선택한 자료를 취소하시겠습니까?", "취소확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                        {

                            clsDB.setBeginTran(clsDB.DbCon);

                            SQL = "";
                            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "IPD_RESERVED ";
                            SQL += ComNum.VBLF + "    SET GBCHK = '1' ";
                            SQL += ComNum.VBLF + "  WHERE ROWID ='" + strRowid + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                return;
                            }

                            clsDB.setCommitTran(clsDB.DbCon);
                            Screen_Display();

                        }
                    }
                }
            }
        }

        void BarCode_Print(string strDate, string strJob)
        {
            frmJepsuPrint frmJepsuPrintX = null;

            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = "";

            FstrPano = VB.Format(VB.Val(VB.SinglePiece(FstrRet, "^^", 1).Trim()), "00000000");
            FstrSname = VB.SinglePiece(FstrRet, "^^", 2).Trim();
            FstrBDATE = VB.SinglePiece(FstrRet, "^^", 3).Trim();

            if (strDate == "")
            {
                FstrRDate = VB.SinglePiece(FstrRet, "^^", 4).Trim();
            }
            else
            {
                FstrRDate = strDate;
            }

            FstrDeptCode = VB.SinglePiece(FstrRet, "^^", 5).Trim();
            FstrDrname = VB.SinglePiece(FstrRet, "^^", 6).Trim();
            FstrDrCode = VB.Format(VB.Val(VB.SinglePiece(FstrRet, "^^", 7).Trim()), "0000");

            if (ComFunc.MsgBoxQ("선택하신 " + FstrSname + "님 입원예약증을 출력하시겠습니까?", "취소확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            FstrDRG = ""; FstrSel = ""; FstrDSC = "";

            #region Data Set
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT JUMIN1, JUMIN2, SEX,SName,Bi ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL += ComNum.VBLF + "  WHERE PANO = '" + FstrPano + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (Dt.Rows.Count > 0)
                {
                    FstrJumin1 = Dt.Rows[0]["JUMIN1"].ToString().Trim();
                    FstrJumin2 = Dt.Rows[0]["JUMIN2"].ToString().Trim();
                    FstrSex = Dt.Rows[0]["SEX"].ToString().Trim();
                    FstrSname = Dt.Rows[0]["SName"].ToString().Trim();

                    FnAge = ComFunc.AgeCalc(clsDB.DbCon, FstrJumin1 + FstrJumin2);
                    FstrBI = Dt.Rows[0]["Bi"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT BI,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, AGE ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL += ComNum.VBLF + "  WHERE PANO = '" + FstrPano + "' ";
                SQL += ComNum.VBLF + "    AND DEPTCODE = '" + FstrDeptCode + "' ";
                SQL += ComNum.VBLF + "    AND BDATE =TO_DATE('" + FstrBDATE + "','YYYY-MM-DD') ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (Dt.Rows.Count > 0)
                {
                    FstrBI = Dt.Rows[0]["BI"].ToString().Trim();
                    FnAge = Convert.ToInt16(Dt.Rows[0]["AGE"].ToString());
                }

                Dt.Dispose();
                Dt = null;

                FstrBI = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_환자종류", FstrBI);

                SQL = "";
                SQL += ComNum.VBLF + " SELECT SUCODE ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OORDER ";
                SQL += ComNum.VBLF + "  WHERE PTNO = '" + FstrPano + "' ";
                SQL += ComNum.VBLF + "    AND DEPTCODE = '" + FstrDeptCode + "' ";
                SQL += ComNum.VBLF + "    AND BDATE =TO_DATE('" + FstrBDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND SUCODE ='$$DRG' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (Dt.Rows.Count > 0) { FstrDRG = "Y"; }

                Dt.Dispose();
                Dt = null;

                FstrDeptName = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, FstrDeptCode);

                if (strJob != "")
                {
                    switch (strJob)
                    {
                        case "DRG":
                            FstrJob = "Y/N/N";
                            break;
                        case "SPC":
                            FstrJob = "N/Y/N";
                            break;
                        case "DSC":
                            FstrJob = "N/N/Y";
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
            #endregion 

            if (frmJepsuPrintX == null)
            {
                frmJepsuPrintX = new frmJepsuPrint();
            }

            frmJepsuPrintX.OpdNr_Print("IPD", FstrPano, FstrDeptCode, FstrBDATE, FstrRDate, FstrDrCode, FstrJob, "접수증");

        }
    }
}
