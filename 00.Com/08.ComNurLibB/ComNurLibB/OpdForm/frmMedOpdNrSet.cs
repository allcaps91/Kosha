using System;
using System.Data;
using System.Windows.Forms;
using ComDbB;
using ComBase;
using ComLibB;

namespace ComNurLibB
{
    public partial class frmMedOpdNrSet : Form
    {
        clsOpdNr OpdWait = new clsOpdNr();//외래대기 환경설정

        public frmMedOpdNrSet()
        {
            InitializeComponent();
        }

        void frmMedOpdNrSet_Load(object sender, EventArgs e)
        {
            ssList3_Sheet1.Columns[5].Visible = false;
            ssList3_Sheet1.Columns[7].Visible = false;
            ssList3_Sheet1.Columns[8].Visible = false;
            ssList3_Sheet1.Columns[11].Visible = false;
            ssList3_Sheet1.Columns[12].Visible = false;
            ssList3_Sheet1.Columns[14].Visible = false;

            //환경설정 체크
            clsOpdNr.READ_OPDWAIT_DB(clsDB.DbCon);
            clsOpdNr.READ_OPDWAIT_INI(clsDB.DbCon);
            //clsOpdNr.READ_EMRPRT_INI();

            //
            ComboSet();

            //
            optPrt2.Checked = true; //인쇄안함 기본
            if (clsOpdNr.GbPrtYN == true) optPrt1.Checked = true;

            //panel1.Enabled = false;
            //if (READ_EDPS_USER_CHK(123) ==true ) panel1.Enabled = true; //editedit// TODO: yunjoyon 간호

            READ_SET12(ssList1_Sheet1,ssList2_Sheet1);

            READ_SET2(ssList3_Sheet1);

            READ_SET3(ssList4_Sheet1);



        }
        //1
        void READ_SET12(FarPoint.Win.Spread.SheetView Spd, FarPoint.Win.Spread.SheetView Spd2)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strDrCode = "";
            Spd.RowCount = 0;

            try
            {
                SQL = SQL + "SELECT b.PrintRanking,a.DrDept1,a.DrCode,a.DrName ";
                SQL = SQL + ComNum.VBLF + "   FROM  " + ComNum.DB_PMPA + "BAS_DOCTOR a, " + ComNum.DB_PMPA + "BAS_CLINICDEPT b  ";
                SQL = SQL + ComNum.VBLF + " WHERE a.DrDept1 NOT IN ('PT','TO','HR','AN','ER','R6')  ";
                SQL = SQL + ComNum.VBLF + "  AND a.DrDept1=b.DeptCode(+) ";
                SQL = SQL + ComNum.VBLF + " ORDER BY b.PrintRanking,a.DrName";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                Spd.RowCount = dt.Rows.Count;
                Spd.SetRowHeight(-1, ComNum.SPDROWHT);
                Spd2.RowCount = dt.Rows.Count;
                Spd2.SetRowHeight(-1, ComNum.SPDROWHT);

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strDrCode = dt.Rows[i]["DrCode"].ToString().Trim();

                        Spd.Cells[i, 0].Text = "";
                        if (VB.InStr(clsOpdNr.GstrEmrDoct, strDrCode) > 1)
                        {
                            Spd.Cells[i, 0].Text = "1";
                        }
                        Spd.Cells[i, 1].Text = dt.Rows[i]["DrDept1"].ToString().Trim();
                        Spd.Cells[i, 2].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        Spd.Cells[i, 3].Text = strDrCode;


                        Spd2.Cells[i, 0].Text = "";
                        if (VB.InStr(clsOpdNr.GstrXrayDoct, strDrCode) > 1)
                        {
                            Spd2.Cells[i, 0].Text = "1";
                        }
                        Spd2.Cells[i, 1].Text = dt.Rows[i]["DrDept1"].ToString().Trim();
                        Spd2.Cells[i, 2].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        Spd2.Cells[i, 3].Text = strDrCode;

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        //2
        void READ_SET2(FarPoint.Win.Spread.SheetView Spd)
        {
            int i = 0;
            int nRow = -1;
            DataTable dt = null;
            DataTable dt2 = null;
            DataTable dt3 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strDrCode = "";            

            Spd.RowCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + " SELECT b.PrintRanking,a.DrDept1,a.DrCode,a.DrName,a.EmrPrts3,a.ROWID ROWID2 ";
                SQL = SQL + ComNum.VBLF + "   FROM  " + ComNum.DB_PMPA + "BAS_DOCTOR a, " + ComNum.DB_PMPA + "BAS_CLINICDEPT b  ";
                if (chkAll.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE a.DrDept1 NOT IN ('PT','TO','HR','AN','ER','R6')  ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE a.DrDept1 IN  (" + clsOpdNr.AllDeptCode + ")";
                }
                SQL = SQL + ComNum.VBLF + "  AND a.DrDept1=b.DeptCode(+) ";
                SQL = SQL + ComNum.VBLF + " ORDER BY b.PrintRanking,a.DrName";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strDrCode = dt.Rows[i]["DrCode"].ToString().Trim();

                        SQL = "";
                        SQL = SQL + " SELECT B.SABUN FROM " + ComNum.DB_MED + "OCS_DOCTOR B ";
                        SQL = SQL + ComNum.VBLF + " WHERE b.DRCODE ='" + strDrCode + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND TDATE >=TRUNC(SYSDATE -30)  ";

                        SqlErr = clsDB.GetDataTableEx(ref dt3, SQL, clsDB.DbCon);

                        if (dt3.Rows.Count > 0)
                        {
                        
                            nRow++;
                            Spd.RowCount = Spd.RowCount + 1;
                            Spd.SetRowHeight(-1, ComNum.SPDROWHT);

                            Spd.Cells[nRow, 0].Text = "";
                            if (VB.InStr(clsOpdNr.GstrEmrViewDoct, strDrCode) > 1)
                            {
                                Spd.Cells[nRow, 0].Text = "1";
                            }
                            Spd.Cells[nRow, 1].Text = dt.Rows[i]["DrDept1"].ToString().Trim();
                            Spd.Cells[nRow, 2].Text = dt.Rows[i]["DrName"].ToString().Trim();
                            Spd.Cells[nRow, 3].Text = strDrCode;


                            SQL = "";
                            SQL = SQL + " SELECT DRCODE,ROWID FROM " + ComNum.DB_PMPA + "ETC_JINDISP_DOCT ";
                            SQL = SQL + ComNum.VBLF + " WHERE DRCODE ='" + strDrCode + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND Gubun ='00' ";

                            SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                            if (dt2.Rows.Count > 0)
                            {
                                Spd.Cells[nRow, 4].Text = "1";
                                Spd.Cells[nRow, 5].Text = dt2.Rows[0]["ROWID"].ToString().Trim();
                            }

                            dt2.Dispose();
                            dt2 = null;

                            //안내문구
                            SQL = "";
                            SQL = SQL + " SELECT DRCODE,Remark,ROWID FROM " + ComNum.DB_PMPA + "ETC_HTML";
                            SQL = SQL + ComNum.VBLF + " WHERE DRCODE ='" + strDrCode + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND Gubun ='R' ";

                            SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                            if (dt2.Rows.Count > 0)
                            {
                                Spd.Cells[nRow, 6].Text = dt2.Rows[0]["Remark"].ToString().Trim();
                                Spd.Cells[nRow, 8].Text = dt2.Rows[0]["ROWID"].ToString().Trim();
                            }

                            dt2.Dispose();
                            dt2 = null;


                            Spd.Cells[nRow, 10].Text = dt.Rows[i]["EmrPrts3"].ToString().Trim();
                            Spd.Cells[nRow, 11].Text = dt.Rows[i]["ROWID2"].ToString().Trim();
                            Spd.Cells[nRow, 12].Text = "";

                            //특검대상
                            SQL = "";
                            SQL = SQL + " SELECT DRCODE,ROWID FROM " + ComNum.DB_PMPA + "ETC_JINDISP_DOCT ";
                            SQL = SQL + ComNum.VBLF + " WHERE DRCODE ='" + strDrCode + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND Gubun ='01' ";

                            SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                            if (dt2.Rows.Count > 0)
                            {
                                Spd.Cells[nRow, 13].Text = "1";
                                Spd.Cells[nRow, 14].Text = dt2.Rows[0]["ROWID"].ToString().Trim();
                            }
                            dt2.Dispose();
                            dt2 = null;
                        }
                        dt3.Dispose();
                        dt3 = null;
                        
                    }
                }

                nRow++;
                Spd.RowCount = Spd.RowCount + 1;
                Spd.SetRowHeight(-1, ComNum.SPDROWHT);

                Spd.Cells[nRow , 0].Text = "";
                if (VB.InStr(clsOpdNr.GstrEmrViewDoct, "2103") > 1)
                {
                    Spd.Cells[nRow, 0].Text = "1";
                }
                Spd.Cells[nRow , 1].Text = "HU";
                Spd.Cells[nRow , 2].Text = "김병욱";
                Spd.Cells[nRow , 3].Text = "2103";


                SQL = "";
                SQL = SQL + " SELECT DRCODE,ROWID FROM KOSMOS_PMPA.ETC_JINDISP_DOCT ";
                SQL = SQL + ComNum.VBLF + " WHERE DRCODE ='2103' ";
                SQL = SQL + ComNum.VBLF + "  AND Gubun ='00' ";

                SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                if (dt2.Rows.Count > 0)
                {
                    Spd.Cells[nRow , 4].Text = "1";
                    Spd.Cells[nRow , 5].Text = dt2.Rows[0]["ROWID"].ToString().Trim();
                }

                dt2.Dispose();
                dt2 = null;
                
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        //3
        void READ_SET3(FarPoint.Win.Spread.SheetView Spd)
        {
            int i = 0;
            DataTable dt = null;
            //DataTable dt2 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strDeptCode = "";

            Spd.RowCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + " SELECT PrintRanking,DeptCode,DeptNameK  ";
                SQL = SQL + ComNum.VBLF + "  FROM  " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";            
                SQL = SQL + ComNum.VBLF + " WHERE DeptCode NOT IN ('PT','TO','HR','AN','ER','R6','II')  ";
                SQL = SQL + ComNum.VBLF + " ORDER BY PrintRanking,DeptCode  ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                Spd.RowCount = dt.Rows.Count;
                Spd.SetRowHeight(-1, ComNum.SPDROWHT);
            
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strDeptCode = dt.Rows[i]["DeptCode"].ToString().Trim();

                        Spd.Cells[i, 0].Text = "";
                        if (VB.InStr(clsOpdNr.AllDeptCode, strDeptCode) > 1)
                        {
                            Spd.Cells[i, 0].Text = "1";
                        }
                        Spd.Cells[i, 1].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        Spd.Cells[i, 2].Text = dt.Rows[i]["DeptNameK"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        bool READ_EDPS_USER_CHK(int argSabun)
        {
            bool rtnVal = false;
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + " SELECT CLASS,GRADE,CHARGE,PART  ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_PASS  ";
                SQL = SQL + ComNum.VBLF + " WHERE IdNumber = " + argSabun + " ";
                SQL = SQL + ComNum.VBLF + "  AND PROGRAMID =' ' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["GRADE"].ToString().Trim() == "EDPS")
                    {
                        rtnVal = true;
                    }
                    else
                    {
                        rtnVal = false;
                    }
                }
                else
                {
                    rtnVal = false;
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
            
        }

        void btnSet_Click(object sender, EventArgs e)
        {
            //저장
            if (clsType.User.Sabun == "23767" || clsType.User.Sabun == "21403" || clsType.User.BuseCode == "077501")
            {
                //진료의 설정
                SET1_SAVE(ssList1_Sheet1, ssList2_Sheet1);
                //진료과 설정
                SET3_SAVE(ssList4_Sheet1);
            }
            //안내문구 설정
            SET2_SAVE(ssList3_Sheet1);
            

            //환경설정 체크
            clsOpdNr.READ_OPDWAIT_DB(clsDB.DbCon);
            clsOpdNr.READ_OPDWAIT_INI(clsDB.DbCon);
            //clsOpdNr.READ_EMRPRT_INI();

            //
            ComboSet();

            //
            optPrt2.Checked = true; //인쇄안함 기본
            if (clsOpdNr.GbPrtYN == true) optPrt1.Checked = true;

            //panel1.Enabled = false;
            //if (READ_EDPS_USER_CHK(123) == true) panel1.Enabled = true; //editedit// TODO: yunjoyon 간호


            //조회
            READ_SET12(ssList1_Sheet1, ssList2_Sheet1);
            READ_SET2(ssList3_Sheet1);
            READ_SET3(ssList4_Sheet1);

        }

        void SET1_SAVE(FarPoint.Win.Spread.SheetView Spd, FarPoint.Win.Spread.SheetView Spd2)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strCHK = "";
            string strREC = "";
            string strXray = "";
            string strFlu = "";
            string strPrtYN = "";

            // EMRPRT_진료의사
            for (i = 0; i < Spd.RowCount; i++)
            {
                strCHK = Spd.Cells[i, 0].Text.ToString();
                if (strCHK == "True")
                {
                    strREC = strREC + "'" + Spd.Cells[i, 3].Text.ToString().Trim() + "',";
                }
            }
            if (strREC != "")
            {
                strREC = VB.Left(strREC, VB.Len(strREC) - 1);
            }


            // EMRPRT_XRAY
            for (i = 0; i < Spd2.RowCount; i++)
            {
                strCHK = Spd2.Cells[i, 0].Text.ToString();
                if (strCHK == "True")
                {
                    strXray = strXray + "'" + Spd2.Cells[i, 3].Text.ToString().Trim() + "',";
                }
            }
            if (strXray != "")
            {
                strXray = VB.Left(strXray, VB.Len(strXray) - 1);
            }
            

            // EMRPRT_인쇄
            strPrtYN = "N";
            if (optPrt1.Checked == true) strPrtYN = "Y";


            // EMRPRT_INFLUE
            strFlu = "N";

            #region //ini 파일저장 EMRPRT_path -> DB로 변경
            //string[] lines = { "의사:" + strREC, "인쇄:" + strPrtYN, "XRAY:" + strXray, "신종플루:" + strFlu };

            //try
            //{
            //    System.IO.File.WriteAllLines(clsOpdNr.EMRPRT_path, lines, System.Text.Encoding.Default);

            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show("파일저장시 오류!! " + ex.Message);
            //}
            #endregion


            // 글로벌 변수 세팅
            clsOpdNr.GstrEmrDoct = strREC;
            clsOpdNr.GstrXrayDoct = strXray;
            clsOpdNr.GbPrtYN = false;
            if (strPrtYN == "Y") clsOpdNr.GbPrtYN = true;

            clsOpdNr.gstrFluPrt = "";
            if (strFlu == "Y") clsOpdNr.gstrFluPrt = "Y";


            // DB 기초코드 세팅
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                // EMRPRT_진료의사
                SQL = "";
                SQL = SQL + ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PCCONFIG SET ";
                SQL = SQL + ComNum.VBLF + "     VALUEV = '" + VB.Replace(strREC,"'","") + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '외래간호' ";
                SQL = SQL + ComNum.VBLF + "   AND CODE = 'EMRPRT_진료의사' ";
                SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '" + clsCompuInfo.gstrCOMIP + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PCCONFIG SET ";
                SQL = SQL + ComNum.VBLF + "     VALUEV = '" + VB.Replace(strREC, "'", "") + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '외래간호' ";
                SQL = SQL + ComNum.VBLF + "   AND CODE = 'EMRPRT1_진료의사' ";
                SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '" + clsCompuInfo.gstrCOMIP + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                // EMRPRT_XRAY
                SQL = "";
                SQL = SQL + ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PCCONFIG SET ";
                SQL = SQL + ComNum.VBLF + "     VALUEV = '" + VB.Replace(strXray, "'", "") + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '외래간호' ";
                SQL = SQL + ComNum.VBLF + "   AND CODE = 'EMRPRT_XRAY' ";
                SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '" + clsCompuInfo.gstrCOMIP + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                // EMRPRT_인쇄
                SQL = "";
                SQL = SQL + ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PCCONFIG SET ";
                SQL = SQL + ComNum.VBLF + "     VALUEV = '" + strPrtYN + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '외래간호' ";
                SQL = SQL + ComNum.VBLF + "   AND CODE = 'EMRPRT_인쇄' ";
                SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '" + clsCompuInfo.gstrCOMIP + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                // EMRPRT_INFLUE
                SQL = "";
                SQL = SQL + ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PCCONFIG SET ";
                SQL = SQL + ComNum.VBLF + "     VALUEV = '" + strFlu + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '외래간호' ";
                SQL = SQL + ComNum.VBLF + "   AND CODE = 'EMRPRT_INFLUE' ";
                SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '" + clsCompuInfo.gstrCOMIP + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }            
        }

        bool SET2_SAVE(FarPoint.Win.Spread.SheetView Spd)
        {
            bool rtVal = false;
            int i = 0;
            string strCHK = "";
            string strREC = "";
            string strREC2 = "";
            string strREC3 = "";

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strDept = "", strDrCode = "", strRemark = "", strChange = "", strChange3 = "";
            //string strChange4 = "";
            string strROWID ="", strROWID2 = "", strROWID3 = "", strROWID4 = "";
            int nSeq = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //
                for (i = 0; i < Spd.RowCount; i++)
                {
                    strDept = Spd.Cells[i, 1].Text.ToString().Trim();
                    strDrCode = Spd.Cells[i, 3].Text.ToString().Trim();
                    strRemark = Spd.Cells[i, 6].Text.ToString().Trim();
                    strChange = Spd.Cells[i, 7].Text.ToString().Trim();
                    strROWID2 = Spd.Cells[i, 8].Text.ToString().Trim();

                    if (Spd.Cells[i, 10].Text.ToString().Trim() != "")
                    {
                        nSeq = Convert.ToInt32(Spd.Cells[i, 10].Text.ToString().Trim());
                    }
                    strROWID3 = Spd.Cells[i, 11].Text.ToString().Trim();
                    strChange3 = Spd.Cells[i, 12].Text.ToString().Trim();

                    if(strROWID2!="")
                    {
                        if(strChange =="Y")
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA +"ETC_HTML SET ";
                            SQL = SQL + ComNum.VBLF + "  REMARK ='" + strRemark + "' ";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID2 + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtVal;
                            }
                        }
                    }
                    else
                    {
                        if (strChange == "Y")
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "ETC_HTML ( DRCODE,REMARK,GUBUN )  VALUES (  ";
                            SQL = SQL + ComNum.VBLF + " '" + strDrCode + "','" + strRemark + "','R' ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtVal;
                            }
                        }
                    }

                    if(strDept=="NS" &&  strROWID3 !="" && strChange3 =="Y")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_DOCTOR SET ";
                        SQL = SQL + ComNum.VBLF + "  EMRPRTS3 =" + nSeq + " ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID3 + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return rtVal;
                        }
                    }                    
                }

            
            

                //
                for (i = 0; i < Spd.RowCount; i++)
                {
                    strCHK = Spd.Cells[i, 0].Text.ToString();
                    if (strCHK == "True")
                    {
                        strREC = strREC + "'" + Spd.Cells[i, 3].Text.ToString().Trim() + "',";
                    }

                    strREC2 = Spd.Cells[i, 3].Text.ToString().Trim();

                    strCHK = Spd.Cells[i, 4].Text.ToString();
                    strROWID = Spd.Cells[i, 5].Text.ToString();

                    strREC3 = Spd.Cells[i, 13].Text.ToString();
                    strROWID4 = Spd.Cells[i, 14].Text.ToString();


                    if (strCHK == "True")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "ETC_JINDISP_DOCT ( DRCODE,ENTSABUN,GUBUN,ENTDATE  )  VALUES (  ";
                        SQL = SQL + ComNum.VBLF + " '" + strREC2 + "',4349,'00',SYSDATE ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return rtVal;
                        }
                    }
                    else
                    {
                        if (strROWID != "")
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_PMPA + "ETC_JINDISP_DOCT ";
                            SQL = SQL + ComNum.VBLF + " WHERE DrCode ='" + strREC2 + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND Gubun ='00' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtVal;
                            }
                        }
                    }


                    strCHK = Spd.Cells[i, 13].Text.ToString();

                    if (strCHK == "True")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "ETC_JINDISP_DOCT ( DRCODE,ENTSABUN,GUBUN,ENTDATE  )  VALUES (  ";
                        SQL = SQL + ComNum.VBLF + " '" + strREC2 + "',4349,'01',SYSDATE ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return rtVal;
                        }
                    }
                    else
                    {
                        if (strROWID4 != "")
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_PMPA + "ETC_JINDISP_DOCT ";
                            SQL = SQL + ComNum.VBLF + " WHERE DrCode ='" + strREC2 + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND Gubun ='01' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtVal;
                            }
                        }
                    }

                }
                

                if (strREC != "")
                {
                    strREC = VB.Left(strREC, VB.Len(strREC) - 1);
                }


                #region //ini 파일저장 EMRPRT_path -> DB로 변경
                //string[] lines = { "의사:" + strREC };

                //try
                //{
                //    System.IO.File.WriteAllLines(clsOpdNr.EMRPRT_path1, lines, System.Text.Encoding.Default);

                //}
                //catch (Exception ex)
                //{

                //    MessageBox.Show("파일저장시 오류!! " + ex.Message);
                //}
                #endregion

                clsOpdNr.GstrEmrViewDoct = strREC;
                clsOpdNr.GstrEmrDoct = strREC;

                
                // EMRPRT1_진료의사
                //SQL = "";
                //SQL = SQL + ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PCCONFIG SET ";
                //SQL = SQL + ComNum.VBLF + "     VALUEV = '" + VB.Replace(strREC, "'", "") + "' ";
                //SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '외래간호' ";
                //SQL = SQL + ComNum.VBLF + "   AND CODE = 'EMRPRT1_진료의사' ";
                //SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '" + clsCompuInfo.gstrCOMIP + "'";

                //SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                //if (SqlErr != "")
                //{
                //    clsDB.setRollbackTran(clsDB.DbCon);
                //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                //    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                //    Cursor.Current = Cursors.Default;
                //    return rtVal;
                //}

                
                clsDB.setCommitTran(clsDB.DbCon);
                //ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }            
        }

        void SET3_SAVE(FarPoint.Win.Spread.SheetView Spd)
        {                     
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            int i = 0;
            string strREC = "";

            //
            for (i = 0; i < Spd.RowCount; i++)
            {
                if (Convert.ToBoolean(Spd.Cells[i, 0].Value) == true)
                {
                    strREC = strREC + "'" + Spd.Cells[i, 1].Text.ToString().Trim() + "',";
                }
            }
            if (strREC != "")
            {
                strREC = VB.Left(strREC, VB.Len(strREC) - 1);
            }
            

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PCCONFIG SET ";
                SQL = SQL + ComNum.VBLF + "     VALUEV = '" + VB.Replace(strREC,"'","") + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '외래간호' ";
                SQL = SQL + ComNum.VBLF + "   AND CODE = 'OPDWAIT_DEPT' ";                
                SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '" + clsCompuInfo.gstrCOMIP + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);                
                Cursor.Current = Cursors.Default;                
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }            
        }

        void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            READ_SET2(ssList3_Sheet1);
        }

        void ssList3_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            string strRemark="";

            if (cboRemark.Text.Trim() == "")
            {
                ComFunc.MsgBox("상용구를 선택후 클릭해 주세요.");
                return;
            }
            
            strRemark = cboRemark.SelectedItem.ToString().Trim();

            if(  e.Row>= 0 && e.Column ==9)
            {
                ssList3_Sheet1.Cells[e.Row, 6].Text = strRemark;
                ssList3_Sheet1.Cells[e.Row, 7].Text = "Y";
            }
        }

        void ssList3_EditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Row < 0) return;

            ssList3_Sheet1.Cells[e.Row, 7].Text = "Y";

        }

        void ComboSet()
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                cboRemark.Items.Clear();
                cboRemark.Items.Add("");
                cboRemark.SelectedIndex = 0;
                         
                SQL = SQL + "SELECT  NAME ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_BCODE  ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN ='ETC_진료현황판상용구' ";
                SQL = SQL + ComNum.VBLF + "  AND (DelDate IS NULL OR DelDate ='') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY CODE  ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboRemark.Items.Add(dt.Rows[i]["Name"].ToString().Trim());
                    }
                }
                
                dt.Dispose();
                dt = null;

                cboRemark.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
    }
}
