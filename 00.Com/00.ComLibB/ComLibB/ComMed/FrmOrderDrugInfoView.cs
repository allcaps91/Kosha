using ComBase; //기본 클래스
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Description : 오더내역조회
    /// Author : 이상훈
    /// Create Date : 2017.10.11
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref="Frm오더내역보기.frm"/>
    public partial class FrmOrderDrugInfoView : Form
    {
        string strPano;

        string FstrSName;
        string FstrJumin;

        string SQL;
        DataTable dt = null;
        string SqlErr = "";     //에러문 받는 변수

        clsSpread SP = new clsSpread();

        public FrmOrderDrugInfoView(string sPano)
        {
            InitializeComponent();

            strPano = sPano;
        }

        private void FrmOrderView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등            

            //ssList_Sheet1.Columns.Get(0).Visible = false;
            txtPano.Text = strPano.Trim();
            fn_ReadBas_Patient(txtPano.Text);
            ssList.ActiveSheet.RowCount = 0;
            lblInfo.Text = "특수약제 사용 내역 조회(100일전 자료포함-외래,입원,지참약)";

            fn_READ_SLIP_AntiBloodMed(txtPano.Text.Trim(), -100);

            //SP.Spread_All_Clear(ssList);
            //SP.Spread_All_Clear(ssListDtl);
            fn_Screen_Clear();

            picPhoto.Visible = false;
        }

        void fn_READ_SLIP_AntiBloodMed(string sPano, int nNal)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += " SELECT 'IO' AS IO,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,DEPTCODE,'항혈전' AS Gbn_1     \r";
                SQL += "   FROM ADMIN.BAS_PATIENT_ANTITHROMBOTIC                                      \r";
                SQL += "  WHERE PANO = '" + sPano + "'                                                      \r";
                SQL += "    AND BDATE >= TO_DATE('" + DateTime.Parse(clsPublic.GstrSysDate).AddDays(nNal).ToShortDateString() + "','YYYY-MM-DD') \r";
                SQL += "    AND Gubun = '01'                                                                \r";  //항혈전
                SQL += "  GROUP BY TO_CHAR(BDATE,'YYYY-MM-DD'),DEPTCODE                                     \r";
                SQL += "  UNION ALL                                                                         \r";
                SQL += " SELECT 'IO' AS IO,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,DEPTCODE,'항혈전' AS Gbn_1     \r";
                SQL += "   FROM ADMIN.OPD_SLIP                                                        \r";
                SQL += "  WHERE PANO = '" + sPano + "'                                                      \r";
                SQL += "    AND ACTDATE =TRUNC(SYSDATE)                                                     \r";  //위의자료 형성 하루지난다음날 발생때문
                SQL += "    AND TRIM(SUNEXT) IN ( SELECT TRIM(JEPCODE)                                      \r";
                SQL += "                            FROM ADMIN.DRUG_SETCODE                            \r";
                SQL += "                           WHERE (DELDATE IS NULL or DelDate ='')                   \r";
                SQL += "                             AND GUBUN = '13' )                                     \r";
                SQL += "  GROUP BY TO_CHAR(BDATE,'YYYY-MM-DD'), DEPTCODE                                    \r";
                SQL += "  UNION ALL                                                                         \r";
                SQL += " SELECT 'IO' AS IO,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,DEPTCODE,'항혈전' AS Gbn_1     \r";
                SQL += "   FROM ADMIN.IPD_NEW_SLIP                                                    \r";
                SQL += "  WHERE PANO = '" + sPano + "'                                                      \r";
                SQL += "    AND ACTDATE = TRUNC(SYSDATE)                                                    \r";  //위의자료 형성 하루지난다음날 발생때문
                SQL += "    AND TRIM(SUNEXT) IN ( SELECT TRIM(JEPCODE)                                      \r";
                SQL += "                            FROM ADMIN.DRUG_SETCODE                            \r";
                SQL += "                           WHERE (DELDATE IS NULL or DelDate ='')                   \r";
                SQL += "                             AND GUBUN = '13' )                                     \r";
                SQL += "  GROUP BY TO_CHAR(BDATE,'YYYY-MM-DD'),DEPTCODE                                     \r";
                SQL += "  UNION ALL                                                                         \r";
                SQL += " SELECT '자가' AS IO, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,'XX' as DEPTCODE ,'항혈전' AS Gbn_1    \r";
                SQL += "   FROM ADMIN.DRUG_HOISLIP                                                     \r";
                SQL += "  WHERE PANO ='" + sPano + "'                                                       \r";
                SQL += "    AND TRUNC(BDATE) >= TO_DATE('" + DateTime.Parse(clsPublic.GstrSysDate).AddDays(nNal).ToShortDateString() + "','YYYY-MM-DD')\r";
                SQL += "    AND BLOOD = '1'                                                                 \r";
                SQL += "  GROUP BY TO_CHAR(BDATE,'YYYY-MM-DD')                                              \r";
                SQL += "  UNION ALL                                                                         \r";
                SQL += " SELECT 'IO' AS IO,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,DEPTCODE,'면역억제' AS Gbn_1   \r";
                SQL += "   FROM  ADMIN.BAS_PATIENT_ANTITHROMBOTIC                                     \r";
                SQL += "  WHERE PANO = '" + sPano + "'                                                      \r";
                SQL += "    AND BDATE >= TO_DATE('" + DateTime.Parse(clsPublic.GstrSysDate).AddDays(nNal).ToShortDateString() + "','YYYY-MM-DD') \r";
                SQL += "    AND Gubun = '02'                                                                \r";  //항혈전
                SQL += "  GROUP BY TO_CHAR(BDATE,'YYYY-MM-DD'),DEPTCODE                                     \r";
                SQL += "  UNION ALL                                                                         \r";
                SQL += " SELECT 'IO' AS IO,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,DEPTCODE,'면역억제' AS Gbn_1   \r";
                SQL += "   FROM ADMIN.OPD_SLIP                                                        \r";
                SQL += "  WHERE PANO = '" + sPano + "'                                                      \r";
                SQL += "    AND ACTDATE = TRUNC(SYSDATE)                                                    \r";  //위의자료 형성 하루지난다음날 발생때문
                SQL += "    AND TRIM(SUNEXT) IN ( SELECT TRIM(JEPCODE)  FROM ADMIN.DRUG_SPECIAL_JEPCODE  WHERE SEQNO = 7 )  \r";
                SQL += "  GROUP BY TO_CHAR(BDATE,'YYYY-MM-DD'),DEPTCODE                                     \r";
                SQL += "  UNION ALL                                                                         \r";
                SQL += " SELECT 'IO' AS IO,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,DEPTCODE,'면역억제' AS Gbn_1   \r";
                SQL += "   FROM ADMIN.IPD_NEW_SLIP                                                    \r";
                SQL += "  WHERE PANO = '" + sPano + "'                                                      \r";
                SQL += "    AND ACTDATE = TRUNC(SYSDATE)                                                    \r";  //위의자료 형성 하루지난다음날 발생때문
                SQL += "    AND TRIM(SUNEXT) IN ( SELECT TRIM(JEPCODE)                                      \r";
                SQL += "                            FROM ADMIN.DRUG_SPECIAL_JEPCODE                    \r";
                SQL += "                           WHERE SEQNO = 7 )                                        \r";
                SQL += "  GROUP BY TO_CHAR(BDATE,'YYYY-MM-DD'),DEPTCODE                                     \r";
                SQL += "  ORDER BY BDATE DESC                                                               \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssList.ActiveSheet.RowCount = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    clsDB.DataTableToSpdRow(dt, ssList, 0, true);
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void fn_Screen_Clear()
        {
            txtDrugHName.Text = "";
            txtDrugEName.Text = "";
            txtDrugSName.Text = "";
            txtDrugHoo.Text = "";
            rtxtInfo.Rtf = "";
        }

        void fn_ReadBas_Patient(string sPano)
        {
            string strJumin2;

            try
            {
                SQL = "";
                SQL += " SELECT SName,Jumin1,Jumin2,Jumin3      \r";
                SQL += "   FROM ADMIN.BAS_PATIENT         \r";
                SQL += "  WHERE Pano = '" + sPano + "'          \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["JUMIN3"].ToString() != "")
                    {
                        strJumin2 = clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                    }
                    else
                    {
                        strJumin2 = dt.Rows[0]["JUMIN2"].ToString().Trim();
                    }
                    FstrSName = dt.Rows[0]["SNAME"].ToString().Trim();
                    FstrJumin = dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + strJumin2;

                    lblSName.Text = FstrSName + " [" + FstrJumin + "]";
                }
                else
                {
                    lblSName.Text = "";
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lblInfo.Text = "특수약제 사용 내역 조회(100일전 자료포함-외래,입원,지참약)";

            SP.Spread_All_Clear(ssList);
            SP.Spread_All_Clear(ssListDtl);
            fn_Screen_Clear();

            fn_READ_SLIP_AntiBloodMed(txtPano.Text.Trim(), -100);
        }

        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strGUBUN;
            string strBDATE;
            string strDeptCode;
            string strGbn;

            picPhoto.Visible = false;

            fn_Screen_Clear();

            if (ssList.ActiveSheet.RowCount == 0) return;
            
            if (e.ColumnHeader == true)
            {
                SP.setSpdSort(ssList, e.Column, true);
                return;
            }

            strGUBUN = ssList.ActiveSheet.Cells[e.Row, 0].Text.Trim();
            strBDATE = DateTime.Parse(ssList.ActiveSheet.Cells[e.Row, 1].Text.Trim()).ToShortDateString();
            strDeptCode = ssList.ActiveSheet.Cells[e.Row, 2].Text.Trim();
            strGbn = ssList.ActiveSheet.Cells[e.Row, 3].Text.Trim();

            if (chkAll.Checked == true)
            {
                strGUBUN = "전체";
            }

            fn_READ_SLIP_AntiBloodMed_View(strGbn, strGUBUN, txtPano.Text.Trim(), strBDATE, strDeptCode);
        }

        void fn_READ_SLIP_AntiBloodMed_View(string ArgGbn, string ArgGubun, string ArgPano, string ArgBDate, string ArgDeptCode)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            SP.Spread_All_Clear(ssListDtl);

            try
            {
                SQL = "";
                if (ArgGbn == "항혈전")
                {
                    if (ArgGubun == "IO")
                    {   
                        SQL  = " SELECT '외래약' AS IO, TO_CHAR(a.BDATE,'YYYY-MM-DD') BDATE,a.SuCode,a.DEPTCODE,b.SuNamek   \r";
                        SQL += "      , a.Qty,a.Nal,a.Div,a.DosCode                                                         \r";
                        SQL += "      , (SELECT DOSNAME FROM ADMIN.OCS_ODOSAGE WHERE DOSCODE = a.DOSCODE AND ROWNUM = 1) DOSNAME \r";
                        SQL += "      , (SELECT GBDIV FROM ADMIN.OCS_ODOSAGE WHERE DOSCODE = a.DOSCODE AND ROWNUM = 1) GBDIV \r";
                        SQL += "   FROM ADMIN.OPD_SLIP a                                                              \r";
                        SQL += "      , ADMIN.BAS_SUN  b                                                              \r";
                        SQL += "  WHERE a.SuCode=b.SuNext(+)                                                                \r";
                        SQL += "    AND a.PANO = '" + ArgPano + "'                                                          \r";
                        SQL += "    AND a.BDATE =TO_DATE('" + ArgBDate + "' ,'YYYY-MM-DD')                                  \r";
                        SQL += "    AND a.DEPTCODE ='" + ArgDeptCode + "'                                                   \r";
                        SQL += "    AND TRIM(a.SUCODE) IN ( SELECT TRIM(JEPCODE)                                            \r";
                        SQL += "                              FROM ADMIN.DRUG_SETCODE                                  \r";
                        SQL += "                             WHERE (DELDATE IS NULL or DelDate ='')                         \r";
                        SQL += "                               AND GUBUN = '13' )                                           \r";
                        SQL += "  UNION ALL                                                                                 \r";
                        SQL += " SELECT '입원약' AS IO, TO_CHAR(a.BDATE,'YYYY-MM-DD') BDATE,a.SuCode,a.DEPTCODE,b.SuNamek   \r";
                        SQL += "      , a.Qty,a.Nal,a.Div, '' AS DosCode                                                    \r";
                        SQL += "      , '' DOSNAME                                                                          \r";
                        SQL += "      , '' GBDIV                                                                            \r";
                        SQL += "   FROM ADMIN.IPD_NEW_SLIP a                                                          \r";
                        SQL += "      , ADMIN.BAS_SUN      b                                                          \r";
                        SQL += "  WHERE a.SuCode = b.SuNext(+)                                                              \r";
                        SQL += "    AND a.PANO = '" + ArgPano + "'                                                          \r";
                        SQL += "    AND a.BDATE = TO_DATE('" + ArgBDate + "' ,'YYYY-MM-DD')                                 \r";
                        SQL += "    AND a.DEPTCODE = '" + ArgDeptCode + "'                                                  \r";
                        SQL += "    AND TRIM(a.SUCODE) IN ( SELECT TRIM(JEPCODE)                                            \r";
                        SQL += "                              FROM ADMIN.DRUG_SETCODE                                  \r";
                        SQL += "                             WHERE (DELDATE IS NULL or DelDate ='')                         \r";
                        SQL += "                               AND GUBUN = '13' )                                           \r";
                        SQL += "  ORDER BY 1 DESC                                                                           \r";
                    }
                    else if (ArgGubun == "자가")
                    {
                        SQL =  " SELECT '자가약' AS IO, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,Remark1 SuNamek ,Remark2 SuCode   \r";
                        SQL += "      , '' AS Qty, '' AS Nal, '' AS DeptCode ,'' as DosCode                                 \r";
                        SQL += "      , '' DOSNAME                                                                          \r";
                        SQL += "      , '' GBDIV                                                                            \r";
                        SQL += "   FROM ADMIN.DRUG_HOISLIP                                                             \r";
                        SQL += "  WHERE PANO = '" + ArgPano + "'                                                            \r";
                        SQL += "    AND TRUNC(BDATE) =TO_DATE('" + ArgBDate + "' ,'YYYY-MM-DD')                             \r";
                        SQL += "    AND BLOOD = '1'                                                                         \r";
                        SQL += "  ORDER BY BDATE DESC                                                                       \r";
                    }
                    else if (ArgGubun == "전체")
                    {
                        SQL =  " SELECT '외래약' AS IO, TO_CHAR(a.BDATE,'YYYY-MM-DD') BDATE,a.SuCode,a.DEPTCODE,b.SuNamek   \r";
                        SQL += "      , a.Qty,a.Nal,a.Div,a.DosCode                                                         \r";
                        SQL += "      , (SELECT DOSNAME FROM ADMIN.OCS_ODOSAGE WHERE DOSCODE = a.DOSCODE AND ROWNUM = 1) DOSNAME \r";
                        SQL += "      , (SELECT GBDIV FROM ADMIN.OCS_ODOSAGE WHERE DOSCODE = a.DOSCODE AND ROWNUM = 1) GBDIV \r";
                        SQL += "  FROM ADMIN.OPD_SLIP a                                                               \r";
                        SQL += "     , ADMIN.BAS_SUN  b                                                               \r";
                        SQL += "   WHERE a.SuCode = b.SuNext(+)                                                             \r";
                        SQL += "    AND a.PANO = '" + ArgPano + "'                                                          \r";
                        SQL += "    AND a.BDATE >=TO_DATE('" + DateTime.Parse(clsPublic.GstrSysDate).AddDays(-100).ToShortDateString() + "' ,'YYYY-MM-DD') \r";
                        SQL += "    AND TRIM(a.SUCODE) IN ( SELECT TRIM(JEPCODE)                                            \r";
                        SQL += "                              FROM ADMIN.DRUG_SETCODE                                  \r";
                        SQL += "                             WHERE (DELDATE IS NULL or DelDate ='')                         \r";
                        SQL += "                               AND GUBUN = '13' )                                           \r";
                        SQL += "  UNION ALL                                                                                 \r";
                        SQL += " SELECT '입원약' AS IO, TO_CHAR(a.BDATE,'YYYY-MM-DD') BDATE,a.SuCode,a.DEPTCODE,b.SuNamek   \r";
                        SQL += "      , a.Qty,a.Nal,a.Div, '' AS DosCode                                                    \r";
                        SQL += "      , '' DOSNAME                                                                          \r";
                        SQL += "      , '' GBDIV                                                                            \r";
                        SQL += "  FROM ADMIN.IPD_NEW_SLIP a                                                           \r";
                        SQL += "     , ADMIN.BAS_SUN      b                                                           \r";
                        SQL += "   WHERE a.SuCode = b.SuNext(+)                                                             \r";
                        SQL += "    AND a.PANO = '" + ArgPano + "'                                                          \r";
                        SQL += "    AND a.BDATE >= TO_DATE('" + DateTime.Parse(clsPublic.GstrSysDate).AddDays(-100).ToShortDateString() + "' ,'YYYY-MM-DD') \r";
                        SQL += "    AND TRIM(a.SUCODE) IN ( SELECT TRIM(JEPCODE)  FROM ADMIN.DRUG_SETCODE  WHERE (DELDATE IS NULL or DelDate ='')   AND GUBUN = '13' ) \r";
                        SQL += "   UNION ALL                                                                                \r";
                        SQL += "  SELECT '자가약' AS IO, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,Remark2 SuCode, '' AS DeptCode   \r";
                        SQL += "       , Remark1 SuNamek ,0 AS Qty, 0 AS Nal, 0 AS Div, '' AS DosCode                       \r";
                        SQL += "      , '' DOSNAME                                                                          \r";
                        SQL += "      , '' GBDIV                                                                            \r";
                        SQL += "    FROM ADMIN.DRUG_HOISLIP                                                            \r";
                        SQL += "   WHERE PANO ='" + ArgPano + "'                                                            \r";
                        SQL += "     AND TRUNC(BDATE) >= TO_DATE('" + DateTime.Parse(clsPublic.GstrSysDate).AddDays(-100).ToShortDateString() + "' ,'YYYY-MM-DD') \r";
                        SQL += "     AND BLOOD = '1'                                                                        \r";
                        SQL += "   ORDER BY 2 DESC                                                                          \r";
                    }
                }
                else if (ArgGbn == "면역억제")
                {
                    if (ArgGubun == "IO")
                    {
                        SQL =  " SELECT '외래약' AS IO, TO_CHAR(a.BDATE,'YYYY-MM-DD') BDATE,a.SuCode,a.DEPTCODE,b.SuNamek   \r";
                        SQL += "      , a.Qty,a.Nal,a.Div,a.DosCode                                                         \r";
                        SQL += "      , (SELECT DOSNAME FROM ADMIN.OCS_ODOSAGE WHERE DOSCODE = a.DOSCODE AND ROWNUM = 1) DOSNAME \r";
                        SQL += "      , (SELECT GBDIV FROM ADMIN.OCS_ODOSAGE WHERE DOSCODE = a.DOSCODE AND ROWNUM = 1) GBDIV \r";
                        SQL += "   FROM ADMIN.OPD_SLIP a                                                              \r";
                        SQL += "      , ADMIN.BAS_SUN  b                                                              \r";
                        SQL += "  WHERE a.SuCode=b.SuNext(+)                                                                \r";
                        SQL += "    AND a.PANO = '" + ArgPano + "'                                                          \r";
                        SQL += "    AND a.BDATE = TO_DATE('" + ArgBDate + "' ,'YYYY-MM-DD')                                 \r";
                        SQL += "    AND a.DEPTCODE ='" + ArgDeptCode + "'                                                   \r";
                        SQL += "    AND TRIM(a.SUCODE) IN ( SELECT TRIM(JEPCODE)                                            \r";
                        SQL += "                              FROM ADMIN.DRUG_SPECIAL_JEPCODE                          \r";
                        SQL += "                             WHERE SEQNO = 7  )                                             \r";
                        SQL += "  UNION ALL                                                                                 \r";
                        SQL += " SELECT '입원약' AS IO, TO_CHAR(a.BDATE,'YYYY-MM-DD') BDATE,a.SuCode,a.DEPTCODE,b.SuNamek   \r";
                        SQL += "      , a.Qty,a.Nal,a.Div, '' AS DosCode                                                    \r";
                        SQL += "      , '' DOSNAME                                                                          \r";
                        SQL += "      , '' GBDIV                                                                            \r";
                        SQL += "   FROM ADMIN.IPD_NEW_SLIP a                                                          \r";
                        SQL += "      , ADMIN.BAS_SUN      b                                                          \r";
                        SQL += "  WHERE a.SuCode = b.SuNext(+)                                                              \r";
                        SQL += "    AND a.PANO ='" + ArgPano + "'                                                           \r";
                        SQL += "    AND a.BDATE = TO_DATE('" + ArgBDate + "' ,'YYYY-MM-DD')                                 \r";
                        SQL += "    AND a.DEPTCODE = '" + ArgDeptCode + "'                                                  \r";
                        SQL += "    AND TRIM(a.SUCODE) IN ( SELECT TRIM(JEPCODE)  FROM ADMIN.DRUG_SPECIAL_JEPCODE  WHERE SEQNO = 7) \r";
                        SQL += "  ORDER BY 1 DESC \r";
                    }
                    else if (ArgGubun == "전체")
                    {
                        SQL =  " SELECT '외래약' AS IO, TO_CHAR(a.BDATE,'YYYY-MM-DD') BDATE,a.SuCode,a.DEPTCODE,b.SuNamek   \r";
                        SQL += "      , a.Qty,a.Nal,a.Div,a.DosCode                                                         \r";
                        SQL += "      , (SELECT DOSNAME FROM ADMIN.OCS_ODOSAGE WHERE DOSCODE = a.DOSCODE AND ROWNUM = 1) DOSNAME \r";
                        SQL += "      , (SELECT GBDIV FROM ADMIN.OCS_ODOSAGE WHERE DOSCODE = a.DOSCODE AND ROWNUM = 1) GBDIV \r";
                        SQL += "   FROM ADMIN.OPD_SLIP a                                                              \r";
                        SQL += "      , ADMIN.BAS_SUN  b                                                              \r";
                        SQL += "   WHERE a.SuCode = b.SuNext(+)                                                             \r";
                        SQL += "    AND a.PANO = '" + ArgPano + "'                                                          \r";
                        SQL += "    AND a.BDATE >= TO_DATE('" + DateTime.Parse(clsPublic.GstrSysDate).AddDays(-100).ToShortDateString() + "' ,'YYYY-MM-DD') \r";
                        SQL += "    AND TRIM(a.SUCODE) IN ( SELECT TRIM(JEPCODE)                                            \r";
                        SQL += "                              FROM ADMIN.DRUG_SPECIAL_JEPCODE                          \r";
                        SQL += "                             WHERE SEQNO = 7  )                                             \r";
                        SQL += "  UNION ALL                                                                                 \r";
                        SQL += " SELECT '입원약' AS IO, TO_CHAR(a.BDATE,'YYYY-MM-DD') BDATE,a.SuCode,a.DEPTCODE,b.SuNamek   \r";
                        SQL += "      , a.Qty,a.Nal,a.Div, '' AS DosCode                                                    \r";
                        SQL += "      , '' DOSNAME                                                                          \r";
                        SQL += "      , '' GBDIV                                                                            \r";
                        SQL += "   FROM ADMIN.IPD_NEW_SLIP a                                                          \r";
                        SQL += "      , ADMIN.BAS_SUN      b                                                          \r";
                        SQL += "  WHERE a.SuCode=b.SuNext(+)                                                                \r";
                        SQL += "    AND a.PANO = '" + ArgPano + "'                                                          \r";
                        SQL += "    AND a.BDATE >= TO_DATE('" + DateTime.Parse(clsPublic.GstrSysDate).AddDays(-100).ToShortDateString() + "' ,'YYYY-MM-DD') \r";
                        SQL += "    AND TRIM(a.SUCODE) IN ( SELECT TRIM(JEPCODE)                                            \r";
                        SQL += "                              FROM ADMIN.DRUG_SPECIAL_JEPCODE                          \r";
                        SQL += "                             WHERE SEQNO = 7  )                                             \r";
                        SQL += "  ORDER BY 2 DESC                                                                           \r";
                    }
                }
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssListDtl.ActiveSheet.RowCount = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ssListDtl.ActiveSheet.Cells[i, 0].Text = ArgGbn;
                        ssListDtl.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["IO"].ToString();
                        ssListDtl.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["BDATE"].ToString();
                        ssListDtl.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["DEPTCODE"].ToString();
                        ssListDtl.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["QTY"].ToString();
                        ssListDtl.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["NAL"].ToString();
                        ssListDtl.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["SUCODE"].ToString();
                        ssListDtl.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["DOSNAME"].ToString();
                        ssListDtl.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["GBDIV"].ToString();
                        ssListDtl.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["SUNAMEK"].ToString();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void ssListDtl_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strSucode;
            string strEtc;
            string strRemark;
            string strAllRemark = "";

            string strFile;
            string strHostFile;
            string strHost;

            //MemoryStream stream = null;
            //Bitmap image = null;
            //byte[] b = null;

            fn_Screen_Clear();

            if (e.ColumnHeader == true)
            {
                SP.setSpdSort(ssListDtl, e.Column, true);
                return;
            }

            strSucode = ssListDtl.ActiveSheet.Cells[e.Row, 6].Text;
            strEtc = ssListDtl.ActiveSheet.Cells[e.Row, 1].Text;

            picPhoto.Visible = false;

            if (strEtc == "자가약")
            {
                MessageBox.Show("자가약은 사진정보가 없습니다.");
            }

            try
            {
                SQL = "";
                SQL += " SELECT SuNext,Jong,BunCode,HName,EName,SName,Unit,JeHeng                           \r";
                SQL += "      , JEHENG2, JEHENG3_1, JEHENG3_2, Jeyak, EffEct                                \r";
                SQL += "      , Remark011,Remark012,Remark013,Remark014,Remark015                           \r";
                SQL += "      , Remark021,Remark022,Remark023,Remark024,Remark025                           \r";
                SQL += "      , Remark031,Remark032,Remark033,Remark034,Remark035                           \r";
                SQL += "      , Remark041,Remark042,Remark043,Remark044,Remark045                           \r";
                SQL += "      , Remark051,Remark052,Remark053,Remark054,Remark055                           \r";
                SQL += "      , Remark061,Remark062,Remark063,Remark064,Remark065                           \r";
                SQL += "      , Remark071,Remark072,Remark073,Remark074,Remark075                           \r";
                SQL += "      , Remark081,Remark082,Remark083,Remark084,Remark085                           \r";
                SQL += "      , Remark091,Remark092,Remark093,Remark094,Remark095                           \r";
                SQL += "      , Remark101,Remark102,Remark103,Remark104,Remark105                           \r";
                SQL += "      , Remark111,Remark112,Remark113,Remark114,Remark115                           \r";
                SQL += "      , Remark121,Remark122,Remark123,Remark124,Remark125                           \r";
                SQL += "      , Remark131,Remark132,Remark133,Remark134,Remark135                           \r";
                SQL += "      , Remark141,Remark142,Remark143,Remark144,Remark145                           \r";
                SQL += "      , Remark151,Remark152,Remark153,Remark154,Remark155                           \r";
                SQL += "      , Remark161,Remark162,Remark163,Remark164,Remark165                           \r";
                SQL += "      , Remark171,Remark172,Remark173,Remark174,Remark175                           \r";
                SQL += "      , Remark181,Remark182,Remark183,Remark184,Remark185                           \r";
                SQL += "      , Remark191,Remark192,Remark193,Remark194,Remark195                           \r";
                SQL += "      , Image_YN, ROWID, DRBUN                                                      \r";
                SQL += "      , TO_CHAR(ENTDATE,'YYYY-MM-DD') ENTDATE, TO_CHAR(SDATE,'YYYY-MM-DD') SDATE    \r";
                SQL += "      , POWDER, PCLSCODE, CAUTION, CAUTION_STRING, METFORMIN                        \r";
                SQL += "  FROM ADMIN.OCS_DRUGINFO_NEW                                                  \r";
                SQL += " WHERE SuNext = '" + strSucode.Trim() + "'                                          \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtDrugHName.Text = dt.Rows[0]["HNAME"].ToString().Trim();
                    txtDrugEName.Text = dt.Rows[0]["ENAME"].ToString().Trim();
                    txtDrugSName.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    txtDrugHoo.Text = dt.Rows[0]["EFFECT"].ToString().Trim();

                    for (int i = 1; i <= 19; i++)
                    {
                        strRemark = dt.Rows[0]["REMARK" + string.Format("{0:00}", i) + "1"].ToString().TrimEnd();
                        strRemark += dt.Rows[0]["REMARK" + string.Format("{0:00}", i) + "2"].ToString().TrimEnd();
                        strRemark += dt.Rows[0]["REMARK" + string.Format("{0:00}", i) + "3"].ToString().TrimEnd();
                        strRemark += dt.Rows[0]["REMARK" + string.Format("{0:00}", i) + "4"].ToString().TrimEnd();
                        strRemark += dt.Rows[0]["REMARK" + string.Format("{0:00}", i) + "5"].ToString().TrimEnd();

                        if (strRemark != "")
                        {
                            switch (i)
                            {
                                case 1: strAllRemark += "【성분함량】" + "\r\n" + strRemark + "\r\n\r\n";
                                    break;
                                case 2: strAllRemark += "【약리작용】" + "\r\n" + strRemark + "\r\n\r\n";
                                    break;
                                case 3: strAllRemark += "【적응증】" + "\r\n" + strRemark + "\r\n\r\n";
                                    break;
                                case 4: strAllRemark += "【용법용량】" + "\r\n" + strRemark + "\r\n\r\n";
                                    break;
                                case 5: strAllRemark += "【부작용금기】" + "\r\n" + strRemark + "\r\n\r\n";
                                    break;
                                case 6: strAllRemark += "【임산부】" + "\r\n" + strRemark + "\r\n\r\n";
                                    break;
                                case 7: strAllRemark += "【약물동력】" + "\r\n" + strRemark + "\r\n\r\n";
                                    break;
                                case 8: strAllRemark += "【상호작용】" + "\r\n" + strRemark + "\r\n\r\n";
                                    break;
                                case 9: strAllRemark += "【저장방법】" + "\r\n" + strRemark + "\r\n\r\n";
                                    break;
                                case 10: strAllRemark += "【참고사항】" + "\r\n" + strRemark + "\r\n\r\n";
                                    break;
                                case 11: strAllRemark += "【복약지도】" + "\r\n" + strRemark + "\r\n\r\n";
                                    break;
                                case 12: strAllRemark += "【보험관련】" + "\r\n" + strRemark + "\r\n\r\n";
                                    break;
                                case 13: strAllRemark += "【약효분류】" + "\r\n" + strRemark + "\r\n\r\n";
                                    break;
                                case 14: strAllRemark += "【금기】" + "\r\n" + strRemark + "\r\n\r\n";
                                    break;
                                case 15: strAllRemark += "【신중투여】" + "\r\n" + strRemark + "\r\n\r\n";
                                    break;
                                case 16: strAllRemark += "【이상반응】" + "\r\n" + strRemark + "\r\n\r\n";
                                    break;
                                case 17: strAllRemark += "【일반적주의】" + "\r\n" + strRemark + "\r\n\r\n";
                                    break;
                                case 18: strAllRemark += "【고령자】" + "\r\n" + strRemark + "\r\n\r\n";
                                    break;
                                case 19: strAllRemark += "【과량투여시 처치】" + "\r\n" + strRemark + "\r\n\r\n";
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    rtxtInfo.Text = strAllRemark;
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            
            Dir_Check(@"C:\PSMHEXE\YAK_IMAGE\");

            strFile = @"C:\PSMHEXE\YAK_IMAGE\" + strSucode.Trim().Replace("/", "__").ToUpper();
            strHostFile = "/data/YAK_IMAGE/" + strSucode.Trim().Replace("/", "__").ToUpper();
            strHost = "/data/YAK_IMAGE/";

            using (Ftpedt FtpedtX = new Ftpedt())
            {
                if (FtpedtX.FtpDownload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strFile, strHostFile, strHost) == true)
                {
                    picPhoto.Visible = true;
                    using (MemoryStream mem = new MemoryStream(File.ReadAllBytes(strFile)))
                    {
                        picPhoto.Image = Image.FromStream(mem);
                    }

                    //picPhoto.Load(strFile);
                    picPhoto.SizeMode = PictureBoxSizeMode.StretchImage;
                    //picPhoto.Refresh();
                }
            }
        }

        private void txtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.txtPano.Text != "" && e.KeyChar == (char)13)
            {
                e.Handled = true;
                txtPano.Text = string.Format("{0:00000000}", int.Parse(txtPano.Text));
                fn_ReadBas_Patient(txtPano.Text);
            }
        }

        void Dir_Check(string sDirPath, string sExe = "*.jpg")
        {
            DirectoryInfo Dir = new DirectoryInfo(sDirPath);
            if (Dir.Exists == false)
            {
                Dir.Create();
            }
            else
            {
                FileInfo[] File = Dir.GetFiles(sExe, SearchOption.AllDirectories);

                foreach (FileInfo file in File)
                {
                    file.Delete();
                }
            }
        }
    }
}
