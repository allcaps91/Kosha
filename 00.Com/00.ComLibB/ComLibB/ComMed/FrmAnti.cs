using ComBase;
using ComBase.Controls;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Description : 항균제 사용 신청
    /// Author : 이상훈
    /// Create Date : 2018.03.05
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref="FrmAnti.frm"/>
    public partial class FrmAnti : Form
    {
        string strPano   = string.Empty;
        string strSName  = string.Empty;
        string strWard   = string.Empty;
        string strRoom   = string.Empty;
        string strInDate = string.Empty;
        string strRowId  = string.Empty;
        string strStatus = string.Empty;
        //string FstrRowId = "";

        string SQL;
        DataTable dt = null;
        DataTable dt1 = null;
        DataTable dt2 = null;
        DataTable dt3 = null;
        string SqlErr = string.Empty;     //에러문 받는 변수
        int intRowAffected = 0; //변경된 Row 받는 변수

        clsSpread SP = new clsSpread();
        clsOrdFunction OF = new clsOrdFunction();

        public FrmAnti(string sPano, string sSName, string sWard, string sRoom, string sInDate, string sRowId, string sStatus)
        {
            InitializeComponent();

            strPano = sPano;
            strSName = sSName;
            strWard = sWard;
            strRoom = sRoom;
            strInDate = Convert.ToDateTime(sInDate).ToString("yyyy-MM-dd");
            strRowId = sRowId;
            clsOrdFunction.GstrANTI_ROWID = sRowId;
            strStatus = sStatus;
        }

        public FrmAnti()
        {
            InitializeComponent();
        }


        private void FrmAnti_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등

            this.Location = new Point(100, 100);

            fn_Screen_Clear();

            if (strPano.NotEmpty())
            {
                txtPano.Text = strPano;
                txtSName.Text = strSName;
                txtWard.Text = strWard;
                txtRoom.Text = strRoom;

                //fn_Read_Vital();

                if (strStatus.NotEmpty())
                {
                    MessageBox.Show("승인, 불가인 경우는 조회만 가능 합니다.", "수정 불가", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    btnRegist.Enabled = false;
                    btnDelete.Enabled = false;
                }
                else
                {
                    btnRegist.Enabled = true;
                    btnDelete.Enabled = true;
                }

                if (clsOrdFunction.GstrAntiSucode.NotEmpty())
                {
                    ssOrder.ActiveSheet.RowCount = 1;

                    ssOrder.ActiveSheet.Cells[0, 0].Text = clsOrdFunction.GstrAntiSucode;
                    ssOrder.ActiveSheet.Cells[0, 1].Text = clsOrdFunction.GstrAntiOrderName;
                    ssOrder.ActiveSheet.Cells[0, 2].Text = clsOrdFunction.GstrAntiPlusName;
                    ssOrder.ActiveSheet.Cells[0, 3].Text = clsOrdFunction.GnAntiContents.ToString();
                    ssOrder.ActiveSheet.Cells[0, 4].Text = clsOrdFunction.GnAnitRealQty.ToString();
                    ssOrder.ActiveSheet.Cells[0, 5].Text = clsOrdFunction.GnAntiDiv.ToString();
                    ssOrder.ActiveSheet.Cells[0, 6].Text = clsOrdFunction.GnAntiNal.ToString();
                    ssOrder.ActiveSheet.Cells[0, 7].Text = clsOrdFunction.GstrAntiDosCode;
                    ssOrder.ActiveSheet.Cells[0, 8].Text = clsOrdFunction.GstrAntiOrderCode;
                    fn_Data_Read(txtPano.Text, strInDate, strRowId);
                }
                else
                {
                    fn_Data_Read(txtPano.Text, strInDate, strRowId);
                }
            }
            else
            {
                txtPano.Text = clsOrdFunction.Pat.PtNo;
                txtSName.Text = clsOrdFunction.Pat.sName;
                txtWard.Text = clsOrdFunction.Pat.WardCode;
                txtRoom.Text = clsOrdFunction.Pat.RoomCode;

                //fn_Read_Vital();

                if (clsOrdFunction.GstrAntiSucode.NotEmpty())
                {
                    ssOrder.ActiveSheet.RowCount = 1;

                    ssOrder.ActiveSheet.Cells[0, 0].Text = clsOrdFunction.GstrAntiSucode;
                    ssOrder.ActiveSheet.Cells[0, 1].Text = clsOrdFunction.GstrAntiOrderName;
                    ssOrder.ActiveSheet.Cells[0, 2].Text = clsOrdFunction.GstrAntiPlusName;
                    ssOrder.ActiveSheet.Cells[0, 3].Text = clsOrdFunction.GnAntiContents.ToString();
                    ssOrder.ActiveSheet.Cells[0, 4].Text = clsOrdFunction.GnAnitRealQty.ToString();
                    ssOrder.ActiveSheet.Cells[0, 5].Text = clsOrdFunction.GnAntiDiv.ToString();
                    ssOrder.ActiveSheet.Cells[0, 6].Text = clsOrdFunction.GnAntiNal.ToString();
                    ssOrder.ActiveSheet.Cells[0, 7].Text = clsOrdFunction.GstrAntiDosCode;
                    ssOrder.ActiveSheet.Cells[0, 8].Text = clsOrdFunction.GstrAntiOrderCode;
                }

                fn_Data_Read(clsOrdFunction.Pat.PtNo, clsOrdFunction.Pat.INDATE, "");

                clsOrdFunction.GstrANTI_ROWID = "";
            }
        }

        /// <summary>
        /// 신장/체중/체온
        /// </summary>
        void fn_Read_Vital()
        {   
            try
            {
                if (string.IsNullOrEmpty(clsOrdFunction.Pat.INDATE)) clsOrdFunction.Pat.INDATE = string.Empty;

                //HEIGHT
                SQL = "";
                SQL += " SELECT CHARTDATE, CHARTTIME, HEIGHT                                \r";
                SQL += "   FROM \r";
                SQL += "   (\r";
                SQL += " SELECT A.CHARTDATE, A.CHARTTIME, EXTRACTVALUE(A.CHARTXML, '//it11') AS HEIGHT                                \r";
                SQL += "   FROM ADMIN.EMRXML A, ADMIN.EMRXMLMST B                                 \r";
                SQL += "  WHERE a.EMRNO = b.EMRNO                                                           \r";
                SQL += "    AND B.FORMNO = 1562                                                             \r";
                SQL += "    AND B.PTNO = '" + txtPano.Text + "'                                             \r";
                SQL += "    AND A.CHARTDATE >= '" + clsOrdFunction.Pat.INDATE.Replace("-", "") + "'         \r"; 
                SQL += "    AND A.CHARTDATE <= TO_CHAR(SYSDATE, 'YYYYMMDD')                                 \r";
                SQL += "    AND TRIM(EXTRACTVALUE(A.CHARTXML, '//it11')) IS NOT NULL                        \r";
                SQL += " UNION ALL                                                                          \r";
                SQL += " SELECT A.CHARTDATE, A.CHARTTIME, B.ITEMVALUE AS HEIGHT                             \r";
                SQL += "   FROM ADMIN.AEMRCHARTMST A                                                   \r";
                SQL += "     INNER JOIN ADMIN.AEMRCHARTROW B                                           \r";
                SQL += "        ON A.EMRNO    = B.EMRNO                                                     \r";
                SQL += "       AND A.EMRNOHIS = B.EMRNOHIS                                                  \r";
                SQL += "       AND B.ITEMCD = 'I0000000002'                                                 \r";
                SQL += "       AND B.ITEMVALUE > CHR(0)                                                     \r";
                SQL += "  WHERE A.FORMNO = 3150                                                             \r";
                SQL += "    AND A.PTNO = '" + txtPano.Text + "'                                             \r";
                SQL += "    AND A.MEDFRDATE = '" + clsOrdFunction.Pat.INDATE.Replace("-", "") + "'          \r";
                //SQL += "    AND A.CHARTDATE >= '" + clsOrdFunction.Pat.INDATE.Replace("-", "") + "'         \r";
                //SQL += "    AND A.CHARTDATE <= TO_CHAR(SYSDATE, 'YYYYMMDD')                                 \r";
                SQL += "   )\r";
                SQL += "  ORDER BY (CHARTDATE || CHARTTIME) DESC                                       \r";

                SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    txtHeight.Text = dt1.Rows[0]["HEIGHT"].ToString().Trim();

                    //2020-09-15 안정수 추가 
                    //측정불가라고 입력되는 경우, IPD_NEW_MASTER에 있는 신장, 체중을 가져온다
                    if (txtHeight.Text.Trim().Contains("측정"))
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT TO_CHAR(HEIGHT) AS HEIGHT";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "  AND PANO = '" + txtPano.Text + "'";
                        SQL += ComNum.VBLF + "  AND ((OUTDATE IS NULL AND JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD'))";
                        SQL += ComNum.VBLF + "      OR (INDATE <= TO_DATE('" + clsOrdFunction.Pat.INDATE + " 23:59', 'YYYY-MM-DD HH24:MI')";
                        SQL += ComNum.VBLF + "          AND OUTDATE >= TO_DATE('" + clsOrdFunction.Pat.INDATE + " 00:00', 'YYYY-MM-DD HH24:MI')))";
                        SQL += ComNum.VBLF + "  AND GBSTS NOT IN ('9')";

                        SqlErr = clsDB.GetDataTableREx(ref dt3, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if(dt3.Rows.Count > 0)
                        {
                            txtHeight.Text = dt3.Rows[0]["HEIGHT"].ToString().Trim();
                        }

                        dt3.Dispose();
                        dt3 = null;
                    }
                }
                else
                {
                    txtHeight.Text = "";
                }

                dt1.Dispose();
                dt1 = null;

                //WEIGHT
                SQL = "";
                SQL += " SELECT CHARTDATE, CHARTTIME, WEIGHT                                                \r";
                SQL += "   FROM                                                                             \r";
                SQL += "   (                                                                                \r";
                SQL += " SELECT A.CHARTDATE, A.CHARTTIME, EXTRACTVALUE(A.CHARTXML, '//it10') AS WEIGHT      \r";
                SQL += "   FROM ADMIN.EMRXML A, ADMIN.EMRXMLMST B                                 \r";
                SQL += "  WHERE a.EMRNO = b.EMRNO                                                           \r";
                SQL += "    AND B.FORMNO = 1562                                                             \r";
                SQL += "    AND B.PTNO = '" + txtPano.Text + "'                                             \r";
                SQL += "    AND A.CHARTDATE >= '" + clsOrdFunction.Pat.INDATE.Replace("-", "") + "'         \r";
                SQL += "    AND A.CHARTDATE <= TO_CHAR(SYSDATE, 'YYYYMMDD')                                 \r";
                SQL += "    AND TRIM(EXTRACTVALUE(A.CHARTXML, '//it10')) IS NOT NULL                        \r";
                SQL += " UNION ALL                                                                          \r";
                SQL += " SELECT A.CHARTDATE, A.CHARTTIME, B.ITEMVALUE AS WEIGHT                             \r";
                SQL += "   FROM ADMIN.AEMRCHARTMST A                                                   \r";
                SQL += "     INNER JOIN ADMIN.AEMRCHARTROW B                                           \r";
                SQL += "        ON A.EMRNO    = B.EMRNO                                                     \r";
                SQL += "       AND A.EMRNOHIS = B.EMRNOHIS                                                  \r";
                SQL += "       AND B.ITEMNO = 'I0000000418'                                                 \r";
                SQL += "       AND B.ITEMVALUE > CHR(0)                                                     \r";
                SQL += "  WHERE A.FORMNO = 3150                                                             \r";
                SQL += "    AND A.PTNO = '" + txtPano.Text + "'                                             \r";
                SQL += "    AND A.MEDFRDATE = '" + clsOrdFunction.Pat.INDATE.Replace("-", "") + "'          \r";
                //SQL += "    AND A.CHARTDATE >= '" + clsOrdFunction.Pat.INDATE.Replace("-", "") + "'         \r";
                //SQL += "    AND A.CHARTDATE <= TO_CHAR(SYSDATE, 'YYYYMMDD')                                               \r";
                SQL += "   )                                                                                \r";
                SQL += "  ORDER BY (CHARTDATE || CHARTTIME) DESC                                            \r";

                SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    txtWeight.Text = dt1.Rows[0]["WEIGHT"].ToString().Trim();

                    //2020-09-15 안정수 추가 
                    //측정불가라고 입력되는 경우, IPD_NEW_MASTER에 있는 신장, 체중을 가져온다
                    if (txtWeight.Text.Trim().Contains("측정"))
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT TO_CHAR(WEIGHT) AS WEIGHT";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "  AND PANO = '" + txtPano.Text + "'";
                        SQL += ComNum.VBLF + "  AND ((OUTDATE IS NULL AND JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD'))";
                        SQL += ComNum.VBLF + "      OR (INDATE <= TO_DATE('" + clsOrdFunction.Pat.INDATE + " 23:59', 'YYYY-MM-DD HH24:MI')";
                        SQL += ComNum.VBLF + "          AND OUTDATE >= TO_DATE('" + clsOrdFunction.Pat.INDATE + " 00:00', 'YYYY-MM-DD HH24:MI')))";
                        SQL += ComNum.VBLF + "  AND GBSTS NOT IN ('9')";

                        SqlErr = clsDB.GetDataTableREx(ref dt3, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt3.Rows.Count > 0)
                        {
                            txtHeight.Text = dt3.Rows[0]["WEIGHT"].ToString().Trim();
                        }

                        dt3.Dispose();
                        dt3 = null;
                    }
                }
                else
                {
                    txtWeight.Text = "";
                }

                dt1.Dispose();
                dt1 = null;

                //BODYTEMP
                SQL = "";
                SQL = "";
                SQL += " SELECT CHARTDATE, CHARTTIME, BDTEMP                                                \r";
                SQL += "   FROM                                                                             \r";
                SQL += "   (                                                                                \r";
                SQL += " SELECT A.CHARTDATE, A.CHARTTIME, EXTRACTVALUE(A.CHARTXML, '//it8') AS BDTEMP       \r";
                SQL += "   FROM ADMIN.EMRXML A, ADMIN.EMRXMLMST B                                 \r";
                SQL += "  WHERE a.EMRNO = b.EMRNO                                                           \r";
                SQL += "    AND B.FORMNO = 1562                                                             \r";
                SQL += "    AND B.PTNO = '" + txtPano.Text + "'                                             \r";
                SQL += "    AND A.CHARTDATE >= '" + clsOrdFunction.Pat.INDATE.Replace("-", "") + "'         \r";
                SQL += "    AND A.CHARTDATE <= TO_CHAR(SYSDATE, 'YYYYMMDD')                                 \r";
                SQL += "    AND TRIM(EXTRACTVALUE(A.CHARTXML, '//it8')) IS NOT NULL                         \r";
                SQL += " UNION ALL                                                                          \r";
                SQL += " SELECT A.CHARTDATE, A.CHARTTIME, B.ITEMVALUE AS BDTEMP                             \r";
                SQL += "   FROM ADMIN.AEMRCHARTMST A                                                   \r";
                SQL += "     INNER JOIN ADMIN.AEMRCHARTROW B                                           \r";
                SQL += "        ON A.EMRNO    = B.EMRNO                                                     \r";
                SQL += "       AND A.EMRNOHIS = B.EMRNOHIS                                                  \r";
                SQL += "       AND B.ITEMNO = 'I0000001811'                                                 \r";
                SQL += "       AND B.ITEMVALUE > CHR(0)                                                     \r";
                SQL += "  WHERE A.FORMNO = 3150                                                             \r";
                SQL += "    AND A.PTNO = '" + txtPano.Text + "'                                             \r";
                SQL += "    AND A.MEDFRDATE = '" + clsOrdFunction.Pat.INDATE.Replace("-", "") + "'          \r";
                //SQL += "    AND A.CHARTDATE >= '" + clsOrdFunction.Pat.INDATE.Replace("-", "") + "'         \r";
                //SQL += "    AND A.CHARTDATE <= TO_CHAR(SYSDATE, 'YYYYMMDD')                                               \r";
                SQL += "   )                                                                                \r";
                SQL += "  ORDER BY (CHARTDATE || CHARTTIME) DESC                                            \r";

                SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    txtBdTemp.Text = dt1.Rows[0]["BDTEMP"].ToString().Trim();
                }
                else
                {
                    txtBdTemp.Text = "";
                }

                dt1.Dispose();
                dt1 = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void fn_Data_Read(string sPano, string sInDate, string strRowId)
        {
            string strSuCode = "";

            //상병
            try
            {
                SQL = "";
                SQL += " SELECT A.ILLCODE, B.ILLNAMEK                                   \r";
                SQL += "   FROM ADMIN.OCS_IILLS A                                  \r";
                SQL += "      , ADMIN.BAS_ILLS B                                  \r";
                SQL += "  WHERE A.ILLCODE  = B.ILLCODE(+)                               \r";
                SQL += "    AND A.PTNO     = '" + sPano + "'                            \r";
                SQL += "    AND A.BDATE   >= TO_DATE('" + sInDate + "','YYYY-MM-DD')    \r";
                SQL += "    AND B.IllCLASS = '1'                                        \r";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    ssDiagno_Sheet1.RowCount = 0;
                    ssDiagno_Sheet1.RowCount = dt1.Rows.Count;
                    ssDiagno_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        ssDiagno.ActiveSheet.Cells[i, 0].Text = dt1.Rows[i]["ILLCODE"].ToString();
                        ssDiagno.ActiveSheet.Cells[i, 1].Text = dt1.Rows[i]["ILLNAMEK"].ToString();
                    }
                }

                dt1.Dispose();
                dt1 = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            //수술
            try
            {
                SQL = "";
                SQL += " SELECT TO_CHAR(OPDATE,'YYYY-MM-DD') OPDATE, OPTITLE        \r";
                SQL += "   FROM ADMIN.ORAN_MASTER                             \r";
                SQL += "  WHERE PANO = '" + sPano + "'                              \r";
                SQL += "    AND OPDATE >= TO_DATE('" + sInDate + "','YYYY-MM-DD')   \r";
                SQL += "  ORDER BY OPDATE DESC                                      \r";
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    ssOp_Sheet1.RowCount = 0;
                    ssOp_Sheet1.RowCount = dt1.Rows.Count;
                    ssOp_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        ssOp.ActiveSheet.Cells[i, 0].Text = dt1.Rows[i]["OPDATE"].ToString();
                        ssOp.ActiveSheet.Cells[i, 1].Text = dt1.Rows[i]["OPTITLE"].ToString();
                    }
                }

                dt1.Dispose();
                dt1 = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            //검사결과
            fn_Exam_Read(sPano, sInDate);

            //기타 항생제 내역
            try
            {
                SQL = "";
                SQL += " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE           \r";
                SQL += "      , A.SUNEXT, SUM(QTY * NAL) QTY, B.SUNAMEK     \r";
                SQL += "   FROM ADMIN.IPD_NEW_SLIP A                  \r";
                SQL += "      , ADMIN.BAS_SUN      B                  \r";
                SQL += "  WHERE A.PANO = '" + sPano + "'                    \r";
                SQL += "    AND A.BDATE >=TRUNC(SYSDATE - 30)               \r";
                //SQL += "    AND A.SUNEXT LIKE 'W-%'                         \r";
                //SQL += "    AND TRIM(a.SuCode) IN ( SELECT TRIM(CODE) FROM ADMIN.BAS_BCODE WHERE GUBUN ='OCS_항생제코드' AND (DELDATE IS NULL OR DELDATE ='')  )  \r";
                //2021-06-24 조회 기준 약품마스터로 변경
                SQL += "    AND A.SUCODE IN ( SELECT JEPCODE FROM ADMIN.DRUG_MASTER2 WHERE SUB  IN (02, 07))  \r";
                SQL += "    AND A.SUNEXT = B.SUNEXT(+)                      \r";
                SQL += "  GROUP BY BDATE, A.SUNEXT, B.SUNAMEK               \r";
                SQL += "  ORDER BY BDATE DESC, 2                            \r";
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    ssEtc_Sheet1.RowCount = 0;
                    ssEtc_Sheet1.RowCount = dt1.Rows.Count;
                    ssEtc_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        ssEtc.ActiveSheet.Cells[i, 0].Text = dt1.Rows[i]["BDATE"].ToString();
                        ssEtc.ActiveSheet.Cells[i, 1].Text = dt1.Rows[i]["SUNEXT"].ToString();
                        ssEtc.ActiveSheet.Cells[i, 2].Text = dt1.Rows[i]["SUNAMEK"].ToString();
                        ssEtc.ActiveSheet.Cells[i, 3].Text = dt1.Rows[i]["QTY"].ToString();
                    }
                }

                dt1.Dispose();
                dt1 = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            try
            {
                ssAntiHis_Sheet1.RowCount = 0;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(SDATE, 'YYYY-MM-DD') AS SDATE, ";
                SQL = SQL + ComNum.VBLF + "     GB_INIT, GB_CULTURE, GB_INFECT, REMARK, ";
                SQL = SQL + ComNum.VBLF + "     HEIGHT, WEIGHT, TEMPER, LEANWEIGHT, GB_SCR, GB_ECCR, ";
                SQL = SQL + ComNum.VBLF + "     GB_SERIOUS, GB_IMM, GB_PREG, GB_ANTI, GB_ANTINAME, ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ANTI_MST";
                SQL = SQL + ComNum.VBLF + "    WHERE PANO = '" + sPano + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    ssAntiHis_Sheet1.RowCount = dt1.Rows.Count;
                    ssAntiHis_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        ssAntiHis_Sheet1.Cells[i, 0].Text = dt1.Rows[i]["SDATE"].ToString().Trim();
                        ssAntiHis_Sheet1.Cells[i, 1].Text = (dt1.Rows[i]["GB_INIT"].ToString().Trim() == "1" ? "초치료" : dt1.Rows[i]["GB_INIT"].ToString().Trim() == "2" ? "연장치료": "재의뢰");
                        ssAntiHis_Sheet1.Cells[i, 2].Text = (dt1.Rows[i]["GB_CULTURE"].ToString().Trim() == "1" ? "유" : "무");
                        ssAntiHis_Sheet1.Cells[i, 3].Text = (dt1.Rows[i]["GB_INFECT"].ToString().Trim() == "1" ? "유" : "무");
                        ssAntiHis_Sheet1.Cells[i, 4].Text = dt1.Rows[i]["REMARK"].ToString().Trim();
                        ssAntiHis_Sheet1.Cells[i, 5].Text = dt1.Rows[i]["HEIGHT"].ToString().Trim();
                        ssAntiHis_Sheet1.Cells[i, 6].Text = dt1.Rows[i]["WEIGHT"].ToString().Trim();
                        ssAntiHis_Sheet1.Cells[i, 7].Text = dt1.Rows[i]["TEMPER"].ToString().Trim();
                        ssAntiHis_Sheet1.Cells[i, 8].Text = dt1.Rows[i]["LEANWEIGHT"].ToString().Trim();
                        ssAntiHis_Sheet1.Cells[i, 9].Text = dt1.Rows[i]["GB_SCR"].ToString().Trim();
                        ssAntiHis_Sheet1.Cells[i, 10].Text = dt1.Rows[i]["GB_ECCR"].ToString().Trim();
                        ssAntiHis_Sheet1.Cells[i, 11].Text = (dt1.Rows[i]["GB_SERIOUS"].ToString().Trim() == "1" ? "경미증" : "중증");
                        ssAntiHis_Sheet1.Cells[i, 12].Text = (dt1.Rows[i]["GB_IMM"].ToString().Trim() == "1" ? "정상" : "비정상");
                        ssAntiHis_Sheet1.Cells[i, 13].Text = (dt1.Rows[i]["GB_PREG"].ToString().Trim() == "1" ? "유" : "무");
                        ssAntiHis_Sheet1.Cells[i, 14].Text = (dt1.Rows[i]["GB_ANTI"].ToString().Trim() == "1" ? "유" : "무");
                        ssAntiHis_Sheet1.Cells[i, 15].Text = dt1.Rows[i]["GB_ANTINAME"].ToString().Trim();
                        ssAntiHis_Sheet1.Cells[i, 16].Text = dt1.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                dt1.Dispose();
                dt1 = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            //ANTI 정보
            if (strRowId == "")
            {
                fn_Read_Vital();
                return;
            }

            try
            {
                SQL = "";
                SQL += " SELECT A.*, TO_CHAR(A.SDATE,'YYYY-MM-DD') SDATE, TO_CHAR(A.OKDATE,'YYYY-MM-DD') OKDATE, a.DosCode  \r";
                SQL += "      , TO_CHAR(A.EXDATE,'YYYY-MM-DD') EXDATE, B.WARDCODE, B.ROOMCODE, A.PANO, B.SNAME              \r";
                SQL += "      , DECODE ( A.STATE, '1', '승인','2','보류','') STATE,  D.ORDERNAME, D.SUCODE                   \r";
                SQL += "      , TO_CHAR(B.INDATE,'YYYY-MM-DD') INDATE, E.DOSNAME                                            \r";
                SQL += "   FROM ADMIN.OCS_ANTI_MST    A                                                                \r";
                SQL += "      , ADMIN.IPD_NEW_MASTER B                                                                \r";
                SQL += "      , ADMIN.OCS_ORDERCODE   D                                                                \r";
                SQL += "      , ADMIN.OCS_ODOSAGE     E                                                                \r";
                SQL += "  WHERE A.ROWID = '" + strRowId + "'                                                                \r";
                SQL += "    AND A.IPDNO = B.IPDNO                                                                           \r";
                SQL += "    AND B.ACTDATE IS NULL                                                                           \r"; //재원자만
                SQL += "    AND A.ORDERCODE = D.ORDERCODE                                                                   \r";
                SQL += "    AND A.DOSCODE = E.DOSCODE(+)                                                                    \r";
                SQL += "  ORDER BY A.SDATE                                                                                  \r";
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    ssOrder_Sheet1.RowCount = 0;
                    ssOrder_Sheet1.RowCount = dt1.Rows.Count;
                    ssOrder_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    //의뢰사유
                    if (dt1.Rows[0]["GB_INIT"].ToString() == "1") rdoGubun1.Checked = true;
                    if (dt1.Rows[0]["GB_INIT"].ToString() == "2") rdoGubun2.Checked = true;
                    if (dt1.Rows[0]["GB_INIT"].ToString() == "3") rdoGubun3.Checked = true;

                    if (dt1.Rows[0]["GB_INFECT"].ToString() == "1") rdoInfect1.Checked = true;
                    if (dt1.Rows[0]["GB_INFECT"].ToString() == "2") rdoInfect2.Checked = true;

                    if (dt1.Rows[0]["GB_CULTURE"].ToString() == "1") rdoCulture1.Checked = true;
                    if (dt1.Rows[0]["GB_CULTURE"].ToString() == "2") rdoCulture2.Checked = true;

                    rtxtRemark.Text = dt1.Rows[0]["REMARK"].ToString().Trim();

                    //환자정보
                    txtHeight.Text = dt1.Rows[0]["HEIGHT"].ToString().Trim();
                    txtWeight.Text = dt1.Rows[0]["WEIGHT"].ToString().Trim();
                    txtBdTemp.Text = dt1.Rows[0]["TEMPER"].ToString().Trim();
                    txtLeanWeight.Text = dt1.Rows[0]["LEANWEIGHT"].ToString().Trim();
                    txtSCR.Text = dt1.Rows[0]["GB_SCR"].ToString().Trim();
                    txtCCR.Text = dt1.Rows[0]["GB_ECCR"].ToString().Trim();

                    if (dt1.Rows[0]["GB_SERIOUS"].ToString().Trim() == "1") rdoSerious1.Checked = true;
                    if (dt1.Rows[0]["GB_SERIOUS"].ToString().Trim() == "2") rdoSerious2.Checked = true;

                    if (dt1.Rows[0]["GB_IMM"].ToString().Trim() == "1") rdoImm1.Checked = true;
                    if (dt1.Rows[0]["GB_IMM"].ToString().Trim() == "2") rdoImm2.Checked = true;

                    if (dt1.Rows[0]["GB_PREG"].ToString().Trim() == "1") rdoPreg1.Checked = true;
                    if (dt1.Rows[0]["GB_PREG"].ToString().Trim() == "2") rdoPreg2.Checked = true;

                    if (dt1.Rows[0]["GB_ANTI"].ToString().Trim() == "1") rdoAnti1.Checked = true;
                    if (dt1.Rows[0]["GB_ANTI"].ToString().Trim() == "2") rdoAnti2.Checked = true;

                    txtAnti.Text = dt1.Rows[0]["GB_ANTINAME"].ToString().Trim();

                    //항생제
                    ssOrder.ActiveSheet.Cells[0, 0].Text = dt1.Rows[0]["SUCODE"].ToString().Trim();
                    ssOrder.ActiveSheet.Cells[0, 1].Text = dt1.Rows[0]["ORDERNAME"].ToString().Trim();
                    ssOrder.ActiveSheet.Cells[0, 2].Text = dt1.Rows[0]["DOSNAME"].ToString().Trim();
                    ssOrder.ActiveSheet.Cells[0, 3].Text = dt1.Rows[0]["BCONTENTS"].ToString().Trim();
                    ssOrder.ActiveSheet.Cells[0, 4].Text = dt1.Rows[0]["CONTENTS"].ToString().Trim();
                    ssOrder.ActiveSheet.Cells[0, 5].Text = dt1.Rows[0]["QTY"].ToString().Trim();
                    ssOrder.ActiveSheet.Cells[0, 6].Text = dt1.Rows[0]["NAL"].ToString().Trim();
                    ssOrder.ActiveSheet.Cells[0, 7].Text = dt1.Rows[0]["ORDERCODE"].ToString().Trim();
                    ssOrder.ActiveSheet.Cells[0, 8].Text = dt1.Rows[0]["DOSCODE"].ToString().Trim();
                    ssOrder.ActiveSheet.Cells[0, 9].Text = dt1.Rows[0]["ORDERNO"].ToString().Trim();

                    dt1.Dispose();
                    dt1 = null;
                }
                else
                {
                    dt1.Dispose();
                    dt1 = null;
                    fn_Read_Vital();
                }
                
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            //심사정보
            strSuCode = ssOrder.ActiveSheet.Cells[0, 0].Text.Trim();

            try
            {
                SQL = "";
                SQL += " SELECT JONG, REMARK                        \r";
                SQL += "   FROM ADMIN.BAS_SIMSAINFOR_WARD     \r";
                SQL += "  WHERE SUNEXT = '" + strSuCode + "'        \r";
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    rtxtSimsa.Rtf = dt1.Rows[0]["REMARK"].ToString().Replace("`", "'");
                }

                dt1.Dispose();
                dt1 = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void fn_Exam_Read(string sPano, string sInDate)
        {
            //string sDate = "";
            string sSpecNo = "";
            //string sCompare = "";
            string sRef = "";
            //string sFDate = "";
            //string sTDate = "";
            //string sWsCode = "";   //WS약어
            string sResultDate = "";   //결과일자
            string sStatus = "";   //상태
            string sResult = "";   //결과
            string strOK = "";   //Display여부
            //string sFootNote = "";   //FootNote
            int nCNT = 0;
            int nCnt1 = 0;
            int iRow = 0;

            string strMasterCode = "";
            string strRESULTDATE  = "";
            string strDisplayOK = "";

            ssExam.ActiveSheet.GetColumnMerge(0);

            SP.Spread_All_Clear(ssExam);

            try
            {
                SQL = "";
                SQL += " SELECT a.sex, a.Age, TO_CHAR(A.RESULTDATE,'YYYY-MM-DD') RESULTDATE, R.Status, R.MasterCode     \r";
                SQL += "      , R.SubCode, R.Result, R.Refer, R.Panic                                                   \r";
                SQL += "      , R.Delta, R.Unit, R.SeqNo, M.ExamName                                                    \r";
                SQL += "      , R.MASTERCODE                                                                            \r";
                SQL += "   FROM ADMIN.EXAM_SPECMST A,  ADMIN.Exam_ResultC R, ADMIN.Exam_Master M         \r";
                SQL += "  WHERE A. SpecNo= R.SPECNO                                                                     \r";
                SQL += "    AND A.PANO = '" + sPano + "'                                                                \r";
                SQL += "    AND A.BDATE >=TO_DATE('" + sInDate + "','YYYY-MM-DD')                                       \r";
                SQL += "    AND R.STATUS = 'V'                                                                          \r";
                SQL += "    AND R.SubCode = M.MasterCode(+)                                                             \r";
                SQL += "    AND R.MasterCode IN ('HR013','HR014','HR012','HR011','HR01','CR34','CR35','CR41','CR42',    \r";
                SQL += "                         'HC030','MI35','MI37')                                                 \r";
                SQL += "  ORDER BY DECODE( R.MASTERCODE, 'HR013', 1,'HR014', 1, 'HR012', 1, 'HR011',1,'HR01',1,'CR34'   \r";
                SQL += "        , 2,'CR35',3,'CR41',4,'CR42',5,'HC030',6,'MI35',7, 'MI37',7, 9)                         \r";
                SQL += "        , A.BDATE DESC, R.MASTERCODE, A.SPECNO, R.SeqNo                                         \r";
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count == 0) return;

                nCNT = dt1.Rows.Count;
                //sCompare = "";
                iRow = 0;
                strDisplayOK = "NO";


                if (dt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        if (strMasterCode != dt1.Rows[i]["MASTERCODE"].ToString())
                        {
                            strDisplayOK = "OK";
                            strMasterCode = dt1.Rows[i]["MASTERCODE"].ToString();
                            strRESULTDATE = dt1.Rows[i]["RESULTDATE"].ToString().Trim();
                        }
                        else
                        {
                            if (strRESULTDATE != dt1.Rows[i]["RESULTDATE"].ToString().Trim())
                            {
                                strDisplayOK = "NO";
                            }
                            else
                            {
                                strRESULTDATE = dt1.Rows[i]["RESULTDATE"].ToString().Trim();
                                strDisplayOK = "OK";
                            }
                        }

                        if (strDisplayOK == "OK")
                        {
                            sResultDate = dt1.Rows[i]["RESULTDATE"].ToString().Trim();
                            sStatus = dt1.Rows[i]["STATUS"].ToString().Trim();
                            sResult = dt1.Rows[i]["RESULT"].ToString().Trim();

                            if (sStatus == "H")
                            {
                                strOK = "OK";
                            }
                            else if (sStatus == "V")
                            {
                                strOK = "OK";
                                if (sResult == "")
                                {
                                    strOK = "OK";
                                }
                                if (dt1.Rows[i]["MASTERCODE"].ToString() == dt1.Rows[i]["SUBCODE"].ToString())
                                {
                                    strOK = "OK";
                                }
                            }
                            else
                            {
                                strOK = "OK";
                                sResult = "◈검사중◈";
                            }

                            //Foot Note를 READ
                            SQL = "";
                            SQL += " SELECT FootNote                                                                    \r";
                            SQL += "   FROM ADMIN.Exam_ResultCf                                                    \r";
                            SQL += "  WHERE SpecNo = '" + sSpecNo + "'                                                  \r";
                            SQL += "    AND SeqNo =  '" + VB.Val(dt1.Rows[i]["SeqNo"].ToString()).ToString("00") + "' \r";
                            SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            nCnt1 = dt2.Rows.Count;

                            if (nCnt1 > 0)
                            {
                                strOK = "OK";
                            }
                            dt2.Dispose();
                            dt2 = null;

                            if (strOK == "OK")
                            {
                                ssExam_Sheet1.RowCount = ssExam_Sheet1.RowCount + 1;
                                ssExam_Sheet1.SetRowHeight(ssExam_Sheet1.RowCount - 1, ComNum.SPDROWHT);
                                //iRow += 1;
                                //if (iRow > ssExam.ActiveSheet.RowCount)
                                //{
                                    //ssExam.ActiveSheet.RowCount = iRow + 20;
                                    ssExam.ActiveSheet.Cells[ssExam_Sheet1.RowCount - 1, 0].Text = sResultDate;
                                    ssExam.ActiveSheet.Cells[ssExam_Sheet1.RowCount - 1, 1].Text = dt1.Rows[i]["EXAMNAME"].ToString(); //검사명
                                    ssExam.ActiveSheet.Cells[ssExam_Sheet1.RowCount - 1, 2].Text = sResult;                           //결과치
                                    ssExam.ActiveSheet.Cells[ssExam_Sheet1.RowCount - 1, 3].Text = dt1.Rows[i]["REFER"].ToString();    //참고치
                                    ssExam.ActiveSheet.Cells[ssExam_Sheet1.RowCount - 1, 4].Text = dt1.Rows[i]["UNIT"].ToString();     //결과단위

                                    sRef = fn_Reference(dt1.Rows[i]["SUBCODE"].ToString(), dt1.Rows[i]["AGE"].ToString(), dt1.Rows[i]["SEX"].ToString());
                                    ssExam.ActiveSheet.Cells[ssExam_Sheet1.RowCount - 1, 5].Text = VB.Pstr(sRef, "~", 1);
                                    ssExam.ActiveSheet.Cells[ssExam_Sheet1.RowCount - 1, 6].Text = VB.Pstr(sRef, "~", 2);
                                //}
                            }

                            if (nCnt1 > 0)
                            {
                                //Foot Note를 READ
                                SQL = "";
                                SQL += " SELECT FootNote                                                                    \r";
                                SQL += "   FROM ADMIN.Exam_ResultCf                                                    \r";
                                SQL += "  WHERE SpecNo = '" + sSpecNo + "'                                                  \r";
                                SQL += "    AND SeqNo =  '" + VB.Val(dt1.Rows[i]["SeqNo"].ToString()).ToString("00") + "' \r";
                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }

                                nCnt1 = dt2.Rows.Count;

                                if (nCnt1 > 0)
                                {
                                    for (int k = 0; k < nCnt1; k++)
                                    {
                                        ssExam_Sheet1.RowCount = ssExam_Sheet1.RowCount + 1;
                                        ssExam_Sheet1.SetRowHeight(ssExam_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                                        //iRow += 1;
                                        //if (iRow > ssExam.ActiveSheet.RowCount) ssExam.ActiveSheet.RowCount = iRow + 20;
                                        ssExam.ActiveSheet.Cells[ssExam_Sheet1.RowCount - 1, 1].Text = "  " + dt2.Rows[k]["FOOTNOTE"].ToString();
                                        ssExam.ActiveSheet.Cells[ssExam_Sheet1.RowCount - 1, 1].ForeColor = Color.Blue;
                                    }
                                }
                                dt2.Dispose();
                                dt2 = null;
                            }
                        }
                    }
                }
                dt1.Dispose();
                dt1 = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            if (iRow > 21) ssExam.ActiveSheet.RowCount = iRow;
        }

        string fn_Reference(string strSubCode, string nAge, string strSex)
        {
            string strRtn = "";

            string sCode = "";
            string sNormal = "";
            string sSex = "";
            string sAgeFrom = "";
            string sAgeTo = "";
            string sRefValFrom = "";
            string sRefValTo = "";

            string sAllReference = "";
            string sReference = "";

            long kk = 0;

            try
            {
                SQL = "";
                SQL += " SELECT MasterCode, Normal, Sex, AgeFrom, AgeTo, RefvalFrom, RefvalTo   \r";
                SQL += "   FROM ADMIN.Exam_Master_Sub                                      \r";
                SQL += "  WHERE MasterCode = '" + strSubCode + "'                               \r";
                SQL += "    AND Gubun = '41'                                                    \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strRtn;
                }

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sCode = dt.Rows[i]["MASTERCODE"].ToString().Trim();
                        sNormal = dt.Rows[i]["NORMAL"].ToString().Trim();
                        sSex = dt.Rows[i]["SEX"].ToString().Trim();
                        sAgeFrom = dt.Rows[i]["AGEFROM"].ToString().Trim();
                        sAgeTo = dt.Rows[i]["AGETO"].ToString().Trim();
                        sRefValFrom = dt.Rows[i]["REFVALFROM"].ToString().Trim();
                        sRefValTo = dt.Rows[i]["REFVALTO"].ToString().Trim();

                        sAllReference += sCode + "|" + sNormal + "|" + sSex + "|" + sAgeFrom + "|" + sAgeTo + "|" + sRefValFrom + "|" + sRefValTo + "|" + "|";

                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strRtn;
            }

            sReference = sAllReference.Replace(strSubCode, "^");

            kk = VB.L(sReference, "^");

            if (kk == 1) return "";

            for (int j = 2; j <= kk; j++)
            {
                sNormal = VB.Split(VB.Split(sReference, "^")[j - 1].ToString(), "|")[1];
                sSex = VB.Split(VB.Split(sReference, "^")[j - 1].ToString(), "|")[2];
                sAgeFrom = VB.Split(VB.Split(sReference, "^")[j - 1].ToString(), "|")[3];
                sAgeTo = VB.Split(VB.Split(sReference, "^")[j - 1].ToString(), "|")[4];
                sRefValFrom = VB.Split(VB.Split(sReference, "^")[j - 1].ToString(), "|")[5];
                sRefValTo = VB.Split(VB.Split(sReference, "^")[j - 1].ToString(), "|")[6];

                if (sNormal != "")
                {
                    strRtn = sNormal;
                    return strRtn;
                }

                if (sSex == "" || sSex == strSex)
                {
                    if (sAgeFrom != "" && sAgeTo != "")
                    {
                        if (VB.Val(nAge) >= VB.Val(sAgeFrom) && VB.Val(nAge) <= VB.Val(sAgeTo))
                        {
                            strRtn = sRefValFrom + " ~ " + sRefValTo;
                            return strRtn;
                        }
                    }
                }
            }

            return strRtn;
        }

        void fn_Screen_Clear()
        {
            //<환자정보>--------------------------------------
            txtHeight.Text = "";
            txtWeight.Text = "";
            txtBdTemp.Text = "";
            txtLeanWeight.Text = "";
            txtSCR.Text = "";
            txtCCR.Text = "";
            txtAnti.Text = "";

            rdoSerious1.Checked = false;
            rdoSerious2.Checked = false;

            rdoImm1.Checked = false;
            rdoImm2.Checked = false;

            rdoPreg1.Checked = false;
            rdoPreg2.Checked = false;

            rdoAnti1.Checked = false;
            rdoAnti2.Checked = false;

            //< 안티오더 > ----------------------------------
            SP.Spread_All_Clear(ssOrder);

            //<의뢰사유>----------------------------------
            rdoCulture1.Checked = false;
            rdoCulture2.Checked = false;
            rdoInfect1.Checked = false;
            rdoInfect2.Checked = false;

            rtxtRemark.Text = "";

            //<진단상병>----------------------------------
            SP.Spread_All_Clear(ssDiagno);

            //검사결과 > ----------------------------------
            SP.Spread_All_Clear(ssExam);

            //<심사기준>----------------------------------
            rtxtSimsa.Text = "";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            string strGB_INIT = string.Empty;
            string strGB_INFECT = string.Empty;
            string strGB_CLUTURE = string.Empty;
            string strGB_SERIOUS = string.Empty;
            string strGB_IMM = string.Empty;
            string strGB_PREG = string.Empty;
            string strGB_ANTI = string.Empty;
            string strORDERCODE = string.Empty;
            string strSuCode = string.Empty;
            long nContents = 0;
            long nBContents = 0;
            long nQty = 0;
            long nNal = 0;
            string strDOSCODE = string.Empty;

            strGB_INIT = string.Empty;

            if (rdoGubun1.AutoCheck == true) strGB_INIT = "1";
            if (rdoGubun2.AutoCheck == true) strGB_INIT = "2";
            if (rdoGubun3.AutoCheck == true) strGB_INIT = "3";

            if (strGB_INIT == "")
            {
                MessageBox.Show("의뢰사유 구분를 선택해주세요 (Inital , Continue)", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                rdoGubun1.Focus();
                return;
            }

            strGB_INFECT = "";
            if (rdoInfect1.Checked == true) strGB_INFECT = "1";
            if (rdoInfect2.Checked == true) strGB_INFECT = "2";

            if (strGB_INFECT == "")
            {
                MessageBox.Show("감염증거 를 선택해주세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                rdoInfect1.Focus();
                return;
            }

            strGB_CLUTURE = "";
            if (rdoCulture1.Checked == true) strGB_CLUTURE = "1";
            if (rdoCulture2.Checked == true) strGB_CLUTURE = "2";

            if (strGB_CLUTURE == "")
            {
                MessageBox.Show("CLUTURE 유/무를 선택해주세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                rdoCulture1.Focus();
                return;
            }

            if (txtHeight.Text == "")
            {
                MessageBox.Show("신장을 입력 해주세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtHeight.Focus();
                return;
            }
            else if (VB.IsNumeric(txtHeight.Text) == false)
            {
                MessageBox.Show("숫자만 입력 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtHeight.Focus();
                return;
            }

            if (txtWeight.Text == "")
            {
                MessageBox.Show("체중을 입력 해주세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtWeight.Focus();
                return;
            }
            else if (VB.IsNumeric(txtWeight.Text) == false)
            {
                MessageBox.Show("숫자만 입력 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtWeight.Focus();
                return;
            }

            if (txtBdTemp.Text == "")
            {
                MessageBox.Show("체온을 입력 해주세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBdTemp.Focus();
                return;
            }
            else if (VB.IsNumeric(txtBdTemp.Text) == false)
            {
                MessageBox.Show("숫자만 입력 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBdTemp.Focus();
                return;
            }

            strGB_SERIOUS = "";
            if (rdoSerious1.Checked == true) strGB_SERIOUS = "1";
            if (rdoSerious2.Checked == true) strGB_SERIOUS = "2";

            if (strGB_SERIOUS == "")
            {
                MessageBox.Show("중증동구분를 선택 해주세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                rdoSerious1.Focus();
                return;
            }

            strGB_IMM = "";
            if (rdoImm1.Checked == true) strGB_IMM = "1";
            if (rdoImm2.Checked == true) strGB_IMM = "2";

            if (strGB_IMM == "")
            {
                MessageBox.Show("면역상태 구분을 선택 해주세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                rdoImm1.Focus();
                return;
            }

            strGB_PREG = "";
            if (rdoPreg1.Checked == true) strGB_PREG = "1";
            if (rdoPreg2.Checked == true) strGB_PREG = "2";

            if (strGB_PREG == "")
            {
                MessageBox.Show("임신 유/무 구분을 선택 해주세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                rdoPreg1.Focus();
                return;
            }

            strGB_ANTI = "";
            if (rdoAnti1.Checked == true) strGB_ANTI = "1";
            if (rdoAnti2.Checked == true) strGB_ANTI = "2";

            if (strGB_ANTI == "")
            {
                MessageBox.Show("항생제 과민 반응 구분을 선택 해주세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                rdoAnti1.Focus();
                return;
            }

            strSuCode = ssOrder.ActiveSheet.Cells[0, 0].Text;
            nContents = long.Parse(ssOrder.ActiveSheet.Cells[0, 3].Text);
            nBContents = long.Parse(ssOrder.ActiveSheet.Cells[0, 4].Text);
            nQty = long.Parse(ssOrder.ActiveSheet.Cells[0, 5].Text);
            nNal = long.Parse(ssOrder.ActiveSheet.Cells[0, 6].Text);
            strDOSCODE = ssOrder.ActiveSheet.Cells[0, 7].Text;
            strORDERCODE = ssOrder.ActiveSheet.Cells[0, 8].Text;

            clsOrdFunction.GstrAntSave = "";

            if (clsOrdFunction.GstrANTI_ROWID.NotEmpty())
            {
                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    SQL = "";
                    SQL += " UPDATE ADMIN.OCS_ANTI_MST                                                     \r";
                    SQL += "    SET SABUN       = '" + string.Format("{0:00000}", clsType.User.Sabun.Trim()) + "' \r";
                    SQL += "      , GB_INIT     = '" + strGB_INIT + "'                                          \r";
                    SQL += "      , GB_INFECT   = '" + strGB_INFECT + "'                                        \r";
                    SQL += "      , GB_CULTURE  = '" + strGB_CLUTURE + "'                                       \r";
                    SQL += "      , REMARK      = '" + rtxtRemark.Text.Replace("'", "`") + "'                   \r";
                    SQL += "      , HEIGHT      = '" + txtHeight.Text + "'                                      \r";
                    SQL += "      , WEIGHT      = '" + txtWeight.Text + "'                                      \r";
                    SQL += "      , TEMPER      = '" + txtBdTemp.Text + "'                                      \r";
                    SQL += "      , LEANWEIGHT  = '" + txtLeanWeight.Text + "'                                  \r";
                    SQL += "      , GB_SCR      = '" + txtSCR.Text + "'                                         \r";
                    SQL += "      , GB_ECCR     = '" + txtCCR.Text + "'                                         \r";
                    SQL += "      , GB_SERIOUS  = '" + strGB_SERIOUS + "'                                       \r";
                    SQL += "      , GB_IMM      = '" + strGB_IMM + "'                                           \r";
                    SQL += "      , GB_PREG     = '" + strGB_PREG + "'                                          \r";
                    SQL += "      , GB_ANTI     = '" + strGB_ANTI + "'                                          \r";
                    SQL += "      , GB_ANTINAME = '" + txtAnti.Text + "'                                        \r";
                    SQL += "  WHERE ROWID       = '" + clsOrdFunction.GstrANTI_ROWID + "'                       \r";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsOrdFunction.GstrAntSave = "";
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr + " 의뢰서 등록시 오류가 발생되었습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    clsDB.setCommitTran(clsDB.DbCon);
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }
            }
            else
            {
                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    SQL = "";
                    SQL += " INSERT INTO ADMIN.OCS_ANTI_MST (SABUN, IPDNO, PANO, SDATE, OKDATE, EXDATE                         \r";
                    SQL += "      , SSABUN, STATE, GB_INIT, GB_INFECT, GB_CULTURE, REMARK, HEIGHT, WEIGHT, TEMPER,  LEANWEIGHT      \r";
                    SQL += "      , GB_SCR, GB_ECCR, GB_SERIOUS, GB_IMM, GB_PREG, GB_ANTI, GB_ANTINAME, ORDERCODE                   \r";
                    SQL += "      , SUCODE, CONTENTS, BCONTENTS, QTY, NAL, DOSCODE, ORDERNO,GUBUN,ENTDATE,BDate)                    \r";
                    SQL += " VALUES (                                                                                               \r";
                    SQL += "        '" + string.Format("{0:00000}", clsType.User.Sabun.Trim()) + "' , '" + clsOrdFunction.Pat.IPDNO + "' \r";
                    SQL += "      , '" + clsOrdFunction.Pat.PtNo + "', TO_DATE('" + clsOrdFunction.GstrBDate + "','YYYY-MM-DD')     \r";
                    //SQL += "      , '','','',''                                                                                     \r";
                    //2020-02-24 전산업무 2020-1620
                    //SQL += "      , SYSDATE, TRUNC(SYSDATE) + 6, '7790', '1'                                                        \r";
                    //2020-04-14 안정수, 자동승인 해제 
                    SQL += "      ,      '', TRUNC(SYSDATE) + 6, '', ''                                                             \r";
                    SQL += "      , '" + strGB_INIT + "','" + strGB_INFECT + "', '" + strGB_CLUTURE + "'                            \r";
                    SQL += "      ,'" + rtxtRemark.Text.Replace("'", "`") + "'                                                      \r";
                    SQL += "      , '" + txtHeight.Text + "', '" + txtWeight.Text + "' , '" + txtBdTemp.Text + "'                   \r";
                    SQL += "      , '" + txtLeanWeight.Text + "', '" + txtSCR.Text + "','" + txtCCR.Text + "'                       \r";
                    SQL += "      ,'" + strGB_SERIOUS + "' ,'" + strGB_IMM + "'                                                     \r";
                    SQL += "      , '" + strGB_PREG + "','" + strGB_ANTI + "', '" + txtAnti.Text + "'                               \r";
                    SQL += "      , '" + strORDERCODE + "','" + strSuCode + "','" + nContents + "','" + nBContents + "'             \r";
                    SQL += "      , '" + nQty + "','" + nNal + "', '" + strDOSCODE + "', '0', 'I'                                   \r";
                    SQL += "      , SYSDATE,TO_DATE('" + clsOrdFunction.GstrBDate + "','YYYY-MM-DD')  )                             \r";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsOrdFunction.GstrAntSave = "";
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr + " 의뢰서 등록시 오류가 발생되었습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    clsOrdFunction.GstrAntSave = "Y";
                    clsDB.setCommitTran(clsDB.DbCon);
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }
            }

            MessageBox.Show("등록완료!!!");
            this.Close();
            return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            fn_Screen_Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (clsOrdFunction.GstrANTI_ROWID == "")
            {
                fn_Screen_Clear();
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += " INSERT INTO ADMIN.OCS_ANTI_MST_HIS ( WRTNO , SABUN, IPDNO, Pano, sDate, OKDATE    \r";
                SQL += "      , EXDATE, SSABUN, State, GB_INIT, GB_INFECT                                       \r";
                SQL += "       , GB_CULTURE,REMARK,HEIGHT,WEIGHT,TEMPER,LEANWEIGHT,GB_SCR,GB_ECCR,GB_SERIOUS    \r";
                SQL += "      , GB_IMM,GB_PREG,GB_ANTI,GB_ANTINAME,ORDERCODE,SUCODE,CONTENTS,BCONTENTS,QTY      \r";
                SQL += "      , NAL,DOSCODE, ORDERNO, SMSDATE, SMSDATE2, TOREMARK, Gubun, EntDate,EDIT_GBN )    \r";
                SQL += "  SELECT WRTNO,SABUN,IPDNO,PANO,SDATE,OKDATE,EXDATE,SSABUN,STATE,GB_INIT,GB_INFECT      \r";
                SQL += "      , GB_CULTURE,REMARK,HEIGHT,WEIGHT,TEMPER,LEANWEIGHT,GB_SCR,GB_ECCR,GB_SERIOUS     \r";
                SQL += "      , GB_IMM,GB_PREG,GB_ANTI,GB_ANTINAME,ORDERCODE,SUCODE,CONTENTS,BCONTENTS,QTY      \r";
                SQL += "      , NAL,DOSCODE, ORDERNO, SMSDATE, SMSDATE2, TOREMARK, Gubun, EntDate, 'D'          \r";
                SQL += "   FROM ADMIN.OCS_ANTI_MST                                                         \r";
                SQL += "  WHERE ROWID = '" + clsOrdFunction.GstrANTI_ROWID + "'                                \r";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsOrdFunction.GstrAntSave = "";
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr + " 의뢰서 삭제시 오류가 발생되었습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                SQL = "";
                SQL += " DELETE ADMIN.OCS_ANTI_MST WHERE ROWID = '" + clsOrdFunction.GstrANTI_ROWID + "'  \r";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsOrdFunction.GstrAntSave = "";
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr + " 의뢰서 삭제시 오류가 발생되었습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                MessageBox.Show("삭제완료!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            this.Close();
        }

        private void ssAntiHis_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            btnDelete.Enabled = true;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(SDATE, 'YYYY-MM-DD') AS SDATE, ";
                SQL = SQL + ComNum.VBLF + "     GB_INIT, GB_CULTURE, GB_INFECT, REMARK, ";
                SQL = SQL + ComNum.VBLF + "     HEIGHT, WEIGHT, TEMPER, LEANWEIGHT, GB_SCR, GB_ECCR, ";
                SQL = SQL + ComNum.VBLF + "     GB_SERIOUS, GB_IMM, GB_PREG, GB_ANTI, GB_ANTINAME, ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ANTI_MST";
                SQL = SQL + ComNum.VBLF + "    WHERE ROWID = '" + ssAntiHis_Sheet1.Cells[e.Row, 16].Text.Trim() + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    switch (dt.Rows[0]["GB_INIT"].ToString().Trim())
                    {
                        case "1": rdoGubun1.Checked = true; break;
                        case "2": rdoGubun2.Checked = true; break;
                        case "3": rdoGubun3.Checked = true; break;
                    }

                    switch (dt.Rows[0]["GB_CULTURE"].ToString().Trim())
                    {
                        case "1": rdoCulture1.Checked = true; break;
                        case "2": rdoCulture1.Checked = true; break;
                    }

                    switch (dt.Rows[0]["GB_INFECT"].ToString().Trim())
                    {
                        case "1": rdoInfect1.Checked = true; break;
                        case "2": rdoInfect2.Checked = true; break;
                    }

                    rtxtRemark.Text = dt.Rows[0]["REMARK"].ToString().Trim();

                    if (txtHeight.Text.Trim() == "") { txtHeight.Text = dt.Rows[0]["HEIGHT"].ToString().Trim(); }
                    if (txtWeight.Text.Trim() == "") { txtWeight.Text = dt.Rows[0]["WEIGHT"].ToString().Trim(); }
                    if (txtBdTemp.Text.Trim() == "") { txtBdTemp.Text = dt.Rows[0]["TEMPER"].ToString().Trim(); }

                    txtLeanWeight.Text = dt.Rows[0]["LEANWEIGHT"].ToString().Trim();
                    txtSCR.Text = dt.Rows[0]["GB_SCR"].ToString().Trim();
                    txtCCR.Text = dt.Rows[0]["GB_ECCR"].ToString().Trim();

                    switch (dt.Rows[0]["GB_SERIOUS"].ToString().Trim())
                    {
                        case "1": rdoSerious1.Checked = true; break;
                        case "2": rdoSerious2.Checked = true; break;
                    }

                    switch (dt.Rows[0]["GB_IMM"].ToString().Trim())
                    {
                        case "1": rdoImm1.Checked = true; break;
                        case "2": rdoImm2.Checked = true; break;
                    }

                    switch (dt.Rows[0]["GB_PREG"].ToString().Trim())
                    {
                        case "1": rdoPreg1.Checked = true; break;
                        case "2": rdoPreg2.Checked = true; break;
                    }

                    switch (dt.Rows[0]["GB_ANTI"].ToString().Trim())
                    {
                        case "1": rdoAnti1.Checked = true; break;
                        case "2": rdoAnti2.Checked = true; break;
                    }

                    txtAnti.Text = dt.Rows[0]["GB_ANTINAME"].ToString().Trim();
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

        private void txtHeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                txtWeight.Focus();
            }
        }

        private void txtWeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                txtBdTemp.Focus();
            }
        }

        private void txtBdTemp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                txtLeanWeight.Focus();
            }
        }

        private void txtLeanWeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                txtSCR.Focus();
            }
        }

        private void txtSCR_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                txtCCR.Focus();
            }
        }

        private void txtCCR_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                rdoSerious1.Focus();
            }
        }

        private void btnEtcResult_Click(object sender, EventArgs e)
        {
            using (frmViewResult f = new frmViewResult(txtPano.Text, "MTSOORDER"))
            {
                f.StartPosition = FormStartPosition.CenterScreen;
                f.ShowDialog(this);
            }
        }
    }
}
