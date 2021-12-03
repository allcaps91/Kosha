using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmNurMibi : Form
    {
        //public delegate void CloseEvent();
        //public event CloseEvent rClosed;

        string strWard = "";

        public frmNurMibi()
        {
            InitializeComponent();
        }

        public frmNurMibi(string strWard)
        {
            InitializeComponent();
            this.strWard = strWard;
        }


        private void frmNurMibi_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            Set_WardCode();

            if (NURSE_Manager_Check(clsType.User.Sabun) == true) cboWard.Enabled = true;

            ssList_Sheet1.RowCount = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
        }

        void Set_WardCode()
        {
            int sIndex = -1;
            DataTable dt = null;

            cboWard.Items.Add("전체");

            string SQL = " SELECT NAME WARDCODE, MATCH_CODE";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CODE";
            SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '2'";
            SQL = SQL + ComNum.VBLF + "   AND SUBUSE = '1'";
            SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING";

            string SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                    if (dt.Rows[i]["MATCH_CODE"].ToString().Trim().Equals(clsType.User.BuseCode))
                    {
                        sIndex = i;
                    }
                }
            }

            dt.Dispose();

            cboWard.Items.Add("SICU");
            cboWard.Items.Add("MICU");
            cboWard.Items.Add("HD");
            cboWard.Items.Add("ER");
            cboWard.Items.Add("RA");
            cboWard.Items.Add("TTE"); //'심장초음파

            cboWard.SelectedIndex = sIndex == -1 ? cboWard.SelectedIndex : sIndex + 1;
        }

        bool NURSE_Manager_Check(string argSABUN)
        {

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "SELECT Code FROM ADMIN.BAS_BCODE ";
                SQL += ComNum.VBLF + " WHERE Gubun='NUR_간호부관리자사번' ";
                SQL += ComNum.VBLF + "   AND Code=" + argSABUN + " ";
                SQL += ComNum.VBLF + "   AND DELDATE IS NULL    ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;

                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                if (VB.Val(dt.Rows[0]["Code"].ToString().Trim()) > 0)
                {
                    return true;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return false;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            GetSearchData();
        }

        void GetSearchData()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt2 = null;

            ComFunc.ReadSysDate(clsDB.DbCon);

            ssList_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "  SELECT M.WardCode,M.RoomCode,M.Pano,M.SName,M.Sex,M.Age,M.Bi,M.PName,M.DEPTCODE,";
                SQL += ComNum.VBLF + "  TRUNC(M.InDate) AS InDate, TRUNC(M.OutDate) AS OutDate, N.OUTTIME";
                SQL += ComNum.VBLF + "  FROM   ADMIN.IPD_NEW_MASTER  M,";
                SQL += ComNum.VBLF + "         ADMIN.BAS_PATIENT P,";
                SQL += ComNum.VBLF + "         ADMIN.BAS_DOCTOR D,";
                SQL += ComNum.VBLF + "         ADMIN.NUR_MASTER N";
                if(cboWard.Text == "전체")
                {
                    SQL += ComNum.VBLF + " WHERE M.WardCode>' '";
                }
                else
                {
                    if(cboWard.Text == "SICU")
                    {
                        SQL += ComNum.VBLF + " WHERE M.WARDCODE = 'IU' ";
                        SQL += ComNum.VBLF + "   AND M.ROOMCODE = '233' ";
                    }
                    else if (cboWard.Text == "MICU")
                    {
                        SQL += ComNum.VBLF + " WHERE M.WARDCODE = 'IU' ";
                        SQL += ComNum.VBLF + "   AND M.ROOMCODE = '234' ";
                    }
                    else if (cboWard.Text == "NR" || cboWard.Text == "ND")
                    {
                        SQL += ComNum.VBLF + " WHERE M.WARDCODE IN ('NR','ND','IQ') ";
                    }
                    else 
                    {
                        SQL += ComNum.VBLF + " WHERE M.WARDCODE = '" + cboWard.Text + "' ";
                    }
                }

                // '    SQL = SQL & vbCr & "  AND (M.ActDate IS NULL OR M.ActDate>=TRUNC(SYSDATE))"
                SQL += ComNum.VBLF + "  AND M.GbSts NOT IN ('9')";
                SQL += ComNum.VBLF + "  AND M.OutDate = TO_DATE('" + dtpDate.Text.Trim() + "','YYYY-MM-DD')";
            //'    SQL = SQL & vbCr & "  AND (M.ROutDate IS NULL OR M.ROutDate>=TRUNC(SYSDATE) )"
                SQL += ComNum.VBLF + "   AND M.Pano=P.Pano(+)";
                SQL += ComNum.VBLF + "   AND M.DrCode=D.DrCode(+)";
                SQL += ComNum.VBLF + "   AND M.PANO = N.PANO";
                SQL += ComNum.VBLF + "      AND M.IPDNO = N.IPDNO ";
                SQL += ComNum.VBLF + "  ORDER BY M.WARDCODE, M.RoomCode,M.SName, M.Indate DESC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssList_Sheet1.RowCount = dt.Rows.Count;
                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    if(dt.Rows[i]["INDATE"].ToString().Trim() != "")
                    {
                        ssList_Sheet1.Cells[i, 6].Text = ComFunc.FormatStrToDateTime(dt.Rows[i]["INDATE"].ToString().Trim(), "D");
                    }

                    if (dt.Rows[i]["OUTDATE"].ToString().Trim() != "")
                    {
                        ssList_Sheet1.Cells[i, 7].Text = ComFunc.FormatStrToDateTime(dt.Rows[i]["OUTDATE"].ToString().Trim(), "D");
                    }
                     ssList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["OUTTIME"].ToString().Trim();

                    if (dt.Rows[i]["INDATE"].ToString().Trim() != "")
                    {
                        SQL = " SELECT SABUN ";
                        SQL += ComNum.VBLF + " FROM ADMIN.EMR_NURMIBI ";
                        SQL += ComNum.VBLF + " WHERE PTNO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                        SQL += ComNum.VBLF + "   AND INDATE = TO_DATE('" + ComFunc.FormatStrToDateTime(dt.Rows[i]["INDATE"].ToString().Trim(),"D") + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "   ORDER BY CDATE DESC ";

                        SqlErr = clsDB.GetDataTableREx(ref dt2, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dt2.Rows.Count > 0)
                        {
                            ssList_Sheet1.Cells[i, 0].Value = true;
                            ssList_Sheet1.Cells[i, 9].Text = clsVbfunc.GetInSaName(clsDB.DbCon, ComFunc.SetAutoZero(dt2.Rows[0]["SABUN"].ToString().Trim(), 5));
                            ssList_Sheet1.Rows[i].ForeColor = System.Drawing.Color.FromArgb(0, 0, 255);
                        }
                        else
                        {
                            ssList_Sheet1.Cells[i, 0].Value = false;
                            ssList_Sheet1.Cells[i, 9].Text = "";
                            ssList_Sheet1.Rows[i].ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
                        }

                        dt2.Dispose();
                        dt2 = null;
                    }

                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSearchCert_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            GetCertData();
        }

        void GetCertData()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strPtNo   = "";
            string strInDate = "";
            string strOutDate = "";

            int strChartTime = 0;
            //string strDAY       = "";
            //string strEVE = "";

            int i = 0;
            int j = 0;

            string strChart1D          = ""; //'간호기록 965
            string strChart1E          = ""; //
            string strChart2D          = ""; //'기본간호활동 1575
            string strChart2E          = ""; //
            string strChart3D          = ""; //'신경정신과 간호활동  1576
            string strChart3E          = ""; //
            string strChart4D          = ""; //'강박간호활동 1574
            string strChart4E          = ""; //
            string strChart5D          = ""; //'활력측정 1562
            string strChart5E          = ""; //
            string strChart6D          = ""; //'퇴원간호 966
            string strChart6E          = ""; //
            string strChart7D          = ""; //'당뇨기록지 1572
            string strChart7E          = ""; //
            string strChart8D          = ""; //'통증재평가기록지 1547
            string strChart8E          = ""; //
            string strChart9D          = ""; //'상처간호기록지 1725
            string strChart9E          = ""; //
            string strChart10D         = ""; //'욕창간호기록지 1573
            string strChart10E = ""; //

            //int nRandom = new Random().Next(0, 9);

            panWait.Visible = true;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                for(i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    strPtNo = ssList_Sheet1.Cells[i, 3].Text.Trim();
                    strInDate = ssList_Sheet1.Cells[i, 6].Text.Trim().Replace("-", "");
                    strOutDate = ssList_Sheet1.Cells[i, 7].Text.Trim().Replace("-", "");
                    Application.DoEvents();

                    SQL = " SELECT CHARTDATE, CHARTTIME, EMRNO, FORMNO ";
                    SQL += ComNum.VBLF + " FROM ADMIN.EMRXMLMST ";
                    SQL += ComNum.VBLF + " WHERE MEDFRDATE = '" + strInDate + "' ";
                    SQL += ComNum.VBLF + "   AND PTNO = '" + strPtNo + "' ";
                    SQL += ComNum.VBLF + "   AND CHARTDATE = '" + strOutDate + "' ";

                    if (strWard == "SICU" || strWard == "MICU")
                    {
                        SQL += ComNum.VBLF + "   AND FORMNO IN ('965','1976','1576','1574','1562','966','1572','1547','1725','1573') ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   AND FORMNO IN ('965','1575','1576','1574','1562','966','1572','1547','1725','1573') ";
                    }


                    #region 신규
                    SQL += ComNum.VBLF + "UNION ALL ";
                    SQL += ComNum.VBLF + " SELECT CHARTDATE, CHARTTIME, EMRNO, FORMNO ";
                    SQL += ComNum.VBLF + " FROM ADMIN.AEMRCHARTMST ";
                    SQL += ComNum.VBLF + " WHERE MEDFRDATE = '" + strInDate + "' ";
                    SQL += ComNum.VBLF + "   AND PTNO = '" + strPtNo + "' ";
                    SQL += ComNum.VBLF + "   AND CHARTDATE = '" + strOutDate + "' ";
                    SQL += ComNum.VBLF + "   AND FORMNO IN (965, 1575, 3150, 966, 1572, 1547, 1725, 1573) ";

                    #endregion

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if(dt.Rows.Count > 0)
                    {
                        for(j = 0; j < dt.Rows.Count; j++)
                        {
                            Application.DoEvents();
                            strChartTime = 0;
                            switch(dt.Rows[j]["FORMNO"].ToString().Trim())
                            {
                                case "965":
                                    strChartTime =(int) VB.Val(VB.Left(dt.Rows[j]["CHARTTIME"].ToString().Trim(), 4));
                                    if(strChartTime >= 0900 && strChartTime < 1500) strChart1D = "OK";
                                    if (strChartTime >= 1500) strChart1E = "OK";
                                    break;

                                case "1575":
                                case "1976":
                                    strChartTime = (int)VB.Val(VB.Left(dt.Rows[j]["CHARTTIME"].ToString().Trim(), 4));
                                    if (strChartTime >= 0900 && strChartTime < 1500) strChart2D = "OK";
                                    if (strChartTime >= 1500) strChart2E = "OK";
                                    break;

                                case "1576":
                                    strChartTime = (int)VB.Val(VB.Left(dt.Rows[j]["CHARTTIME"].ToString().Trim(), 4));
                                    if (strChartTime >= 0900 && strChartTime < 1500) strChart3D = "OK";
                                    if (strChartTime >= 1500) strChart3E = "OK";
                                    break;

                                case "1574":
                                    strChartTime = (int)VB.Val(VB.Left(dt.Rows[j]["CHARTTIME"].ToString().Trim(), 4));
                                    if (strChartTime >= 0900 && strChartTime < 1500) strChart4D = "OK";
                                    if (strChartTime >= 1500) strChart4E = "OK";
                                    break;
                                case "1562":
                                case "3150":
                                    strChartTime = (int)VB.Val(VB.Left(dt.Rows[j]["CHARTTIME"].ToString().Trim(), 4));
                                    if (strChartTime >= 0900 && strChartTime < 1500) strChart5D = "OK";
                                    if (strChartTime >= 1500) strChart5E = "OK";
                                    break;
                                case "966":
                                    strChartTime = (int)VB.Val(VB.Left(dt.Rows[j]["CHARTTIME"].ToString().Trim(), 4));
                                    if (strChartTime >= 0900 && strChartTime < 1500) strChart6D = "OK";
                                    if (strChartTime >= 1500) strChart6E = "OK";
                                    break;
                                case "1572":
                                    strChartTime = (int)VB.Val(VB.Left(dt.Rows[j]["CHARTTIME"].ToString().Trim(), 4));
                                    if (strChartTime >= 0900 && strChartTime < 1500) strChart7D = "OK";
                                    if (strChartTime >= 1500) strChart7E = "OK";
                                    break;

                                case "1547":
                                    strChartTime = (int)VB.Val(VB.Left(dt.Rows[j]["CHARTTIME"].ToString().Trim(), 4));
                                    if (strChartTime >= 0900 && strChartTime < 1500) strChart8D = "OK";
                                    if (strChartTime >= 1500) strChart8E = "OK";
                                    break;

                                case "1725":
                                    strChartTime = (int)VB.Val(VB.Left(dt.Rows[j]["CHARTTIME"].ToString().Trim(), 4));
                                    if (strChartTime >= 0900 && strChartTime < 1500) strChart9D = "OK";
                                    if (strChartTime >= 1500) strChart9E = "OK";
                                    break;

                                case "1573":
                                    strChartTime = (int)VB.Val(VB.Left(dt.Rows[j]["CHARTTIME"].ToString().Trim(), 4));
                                    if (strChartTime >= 0900 && strChartTime < 1500) strChart10D = "OK";
                                    if (strChartTime >= 1500) strChart10E = "OK";
                                    break;
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    if (strChart1D == "OK") ssList_Sheet1.Cells[i, 10].Text =  "작성완료";
                    if (strChart1E == "OK") ssList_Sheet1.Cells[i, 11].Text =  "작성완료";
                    if (strChart2D == "OK") ssList_Sheet1.Cells[i, 12].Text =  "작성완료";
                    if (strChart2E == "OK") ssList_Sheet1.Cells[i, 13].Text =  "작성완료";
                    if (strChart3D == "OK") ssList_Sheet1.Cells[i, 14].Text =  "작성완료";
                    if (strChart3E == "OK") ssList_Sheet1.Cells[i, 15].Text =  "작성완료";
                    if (strChart4D == "OK") ssList_Sheet1.Cells[i, 16].Text =  "작성완료";
                    if (strChart4E == "OK") ssList_Sheet1.Cells[i, 17].Text =  "작성완료";
                    if (strChart5D == "OK") ssList_Sheet1.Cells[i, 18].Text =  "작성완료";
                    if (strChart5E == "OK") ssList_Sheet1.Cells[i, 19].Text =  "작성완료";
                    if (strChart6D == "OK") ssList_Sheet1.Cells[i, 20].Text =  "작성완료";
                    if (strChart6E == "OK") ssList_Sheet1.Cells[i, 20].Text =  "작성완료";
                    if (strChart7D == "OK") ssList_Sheet1.Cells[i, 21].Text =  "작성완료";
                    if (strChart7E == "OK") ssList_Sheet1.Cells[i, 22].Text =  "작성완료";
                    if (strChart8D == "OK") ssList_Sheet1.Cells[i, 23].Text =  "작성완료";
                    if (strChart8E == "OK") ssList_Sheet1.Cells[i, 24].Text =  "작성완료";
                    if (strChart9D == "OK") ssList_Sheet1.Cells[i, 25].Text =  "작성완료";
                    if (strChart9E == "OK") ssList_Sheet1.Cells[i, 26].Text = "작성완료";
                    if (strChart10D == "OK") ssList_Sheet1.Cells[i, 27].Text = "작성완료";
                    if (strChart10E == "OK") ssList_Sheet1.Cells[i, 28].Text = "작성완료";

                    strChart1D = "";                strChart1E = "";
                    strChart2D = "";                strChart2E = "";
                    strChart3D = "";                strChart3E = "";
                    strChart4D = "";                strChart4E = "";
                    strChart5D = "";                strChart5E = "";
                    strChart6D = "";                strChart6E = "";
                    strChart7D = "";                strChart7E = "";
                    strChart8D = "";                strChart8E = "";
                    strChart9D = "";                strChart9E = "";
                    strChart10D = "";               strChart10E = "";
                }

                panWait.Visible = false;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            if(Set_Verity() == false) return;
            GetSearchData();
        }

        bool Set_Verity()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            int i = 0;
            string strPtNo = "";
            string strInDate = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                for(i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    if((bool) ssList_Sheet1.Cells[i, 0].Value == true)
                    {
                        strPtNo = ssList_Sheet1.Cells[i, 3].Text.Trim();
                        strInDate = ssList_Sheet1.Cells[i, 6].Text.Trim();

                        SQL = " INSERT INTO ADMIN.EMR_NURMIBI(PTNO, INDATE, CDATE, SABUN) VALUES(";
                        SQL += ComNum.VBLF + "'" + strPtNo + "',TO_DATE('" + strInDate + "','YYYY-MM-DD'), SYSDATE, " + clsType.User.Sabun + ") ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
