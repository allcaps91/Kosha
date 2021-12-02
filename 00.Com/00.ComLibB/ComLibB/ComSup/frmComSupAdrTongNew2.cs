using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmComSupAdrTongNew2 : Form
    {
        public frmComSupAdrTongNew2()
        {
            InitializeComponent();
        }

        private void frmComSupAdrTongNew2_Load(object sender, EventArgs e)
        {
            ////폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            dtpSDATE.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddMonths(-1);
            dtpEDATE.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            ssView_Sheet1.RowCount = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<Dictionary<string, object>> dt = null;
            
            for(int i2 = 0; i2 < tabControl1.TabCount; i2++)
            {
                FpSpread spd = tabControl1.TabPages[i2].Controls[0] as FpSpread;
                switch (i2)
                {
                    case 0:
                        dt = GetData1();
                        break;
                    case 1:
                        dt = GetData2_1();
                        break;
                    case 2:
                        dt = GetData2_2();
                        break;
                    case 3:
                        dt = GetData2_3();
                        break;
                    case 4:
                        dt = GetData3();
                        break;
                    case 5:
                        dt = GetData4();
                        break;
                    case 6:
                        dt = GetData5();
                        break;
                    case 7:
                        dt = GetData6();
                        break;
                    case 8:
                        dt = GetData7();
                        break;
                }


                if (dt == null || spd == null)
                    return;

                if (spd.Equals(fpSpread4) == false)
                {
                    spd.ActiveSheet.RowCount = 0;
                    spd.ActiveSheet.RowCount = dt.Count;
                    spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                }
                else
                {
                    spd.ActiveSheet.Cells[0, 1, spd.ActiveSheet.RowCount - 1, spd.ActiveSheet.ColumnCount - 1].Text = "";
                }


                for (int i = 0; i < dt.Count; i++)
                {
                    #region 데이터
                    switch (i2)
                    {
                        case 0:
                            spd.ActiveSheet.Cells[i, 0].Text = dt[i]["YYYYMM"].To<string>();
                            spd.ActiveSheet.Cells[i, 1].Text = (dt[i]["O_CNT"].To<int>(0) + dt[i]["I_CNT"].To<int>(0) + dt[i]["ETC_CNT"].To<int>(0)).ToString();
                            spd.ActiveSheet.Cells[i, 2].Text = dt[i]["I_CNT"].To<string>();
                            spd.ActiveSheet.Cells[i, 3].Text = dt[i]["O_CNT"].To<string>();
                            spd.ActiveSheet.Cells[i, 4].Text = dt[i]["ETC_CNT"].To<string>();
                            break;
                        case 1:
                        case 2:
                        case 3:
                            spd.ActiveSheet.Cells[i, 0].Text = dt[i]["DEPTCODE"].To<string>();
                            spd.ActiveSheet.Cells[i, 1].Text = dt[i]["SORT"].To<string>();
                            break;
                        case 4:
                            int ROW_NUM = dt[i]["ROW_NUM"].To<int>(0);
                            spd.ActiveSheet.Cells[i, 1].Text = (dt[ROW_NUM]["GBN1"].To<int>() + dt[ROW_NUM]["GBN2"].To<int>()).ToString();
                            spd.ActiveSheet.Cells[i, 2].Text = dt[ROW_NUM]["GBN1"].To<string>();
                            spd.ActiveSheet.Cells[i, 3].Text = dt[ROW_NUM]["GBN2"].To<string>();
                            break;
                        case 5:
                            spd.ActiveSheet.Cells[i, 0].Text = dt[i]["SUNGBUN"].To<string>();
                            spd.ActiveSheet.Cells[i, 1].Text = dt[i]["SUNGBUN_NM"].To<string>();
                            spd.ActiveSheet.Cells[i, 2].Text = dt[i]["CNT"].To<string>();
                            break;
                        case 6:
                            spd.ActiveSheet.Cells[i, 0].Text = dt[i]["SUCODE"].To<string>();
                            spd.ActiveSheet.Cells[i, 1].Text = dt[i]["JEPNAMEK"].To<string>();
                            spd.ActiveSheet.Cells[i, 2].Text = dt[i]["CNT"].To<string>();
                            break;
                        case 7:
                            spd.ActiveSheet.Cells[i, 0].Text = dt[i]["NAME"].To<string>();
                            spd.ActiveSheet.Cells[i, 1].Text = dt[i]["CNT"].To<string>();
                            break;
                        case 8:
                            spd.ActiveSheet.Cells[i, 0].Text = dt[i]["SUCODE"].To<string>();
                            spd.ActiveSheet.Cells[i, 1].Text = dt[i]["SUNAMEK"].To<string>();
                            spd.ActiveSheet.Cells[i, 2].Text = dt[i]["SUNGBUN_NM"].To<string>();
                            spd.ActiveSheet.Cells[i, 3].Text = dt[i]["LIST"].To<string>();
                            spd.ActiveSheet.Cells[i, 4].Text = dt[i]["CNT"].To<string>();

                            spd.ActiveSheet.Rows[i].Height = spd.ActiveSheet.Rows[i].GetPreferredHeight() + 16;
                            break;
                    }
                    #endregion
                }
            }
        }
        
        private void btnExcel_Click(object sender, EventArgs e)
        {
            FpSpread spd = tabControl1.SelectedTab.Controls[0] as FpSpread;
            using (clsSpread CS = new clsSpread())
            {
                CS.ExportToXLS(spd);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            FpSpread spd = tabControl1.SelectedTab.Controls[0] as FpSpread;

            string strFont1 = "";
            string strHead1 = "약물이상반응모니터링 통계";
            string strFont2 = "";
            string strHead2 = "";


            spd.ActiveSheet.PrintInfo.AbortMessage = "프린터 중입니다.";
            //spd.ActiveSheet.PrintInfo.ZoomFactor = 0.7f;
            //spd.ActiveSheet.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation./*Landscape*/;
            spd.ActiveSheet.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            spd.ActiveSheet.PrintInfo.Margin.Top = 40;
            spd.ActiveSheet.PrintInfo.Margin.Bottom = 20;
            spd.ActiveSheet.PrintInfo.Margin.Header = 10;
            spd.ActiveSheet.PrintInfo.ShowColor = false;
            spd.ActiveSheet.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            spd.ActiveSheet.PrintInfo.ShowBorder = true;
            spd.ActiveSheet.PrintInfo.ShowGrid = true;
            spd.ActiveSheet.PrintInfo.ShowShadows = false;
            spd.ActiveSheet.PrintInfo.UseMax = true;
            spd.ActiveSheet.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            spd.ActiveSheet.PrintInfo.UseSmartPrint = false;
            spd.ActiveSheet.PrintInfo.ShowPrintDialog = false;
            spd.ActiveSheet.PrintInfo.Preview = false;
            spd.PrintSheet(spd.ActiveSheetIndex);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 기간(월)별 보고 건수
        /// </summary>
        /// <returns></returns>
        private List<Dictionary<string, object>> GetData1()
        {
            
            MParameter mParameter = new MParameter();
            mParameter.AppendSql("WITH TONG_DATA AS                                                                                                          ");
            mParameter.AppendSql("(                                                                                                                          ");
            mParameter.AppendSql("SELECT TO_CHAR(WDATE, 'YYYYMM') YYYYMM                                                                                     ");
            mParameter.AppendSql("	,	CASE WHEN PATIENT_BUN IN ('입원', '외래') THEN PATIENT_BUN ELSE '기타' END GUBUN                                       ");
            mParameter.AppendSql("	,	COUNT(PTNO) CNT                                                                                                      ");
            mParameter.AppendSql("  FROM KOSMOS_ADM.DRUG_ADR1                                                                                                ");
            mParameter.AppendSql(" WHERE WDATE >= TO_DATE(:SDATE)                                                                                            ");
            mParameter.AppendSql("   AND WDATE <= TO_DATE(:EDATE)                                                                                            ");
            mParameter.AppendSql(" GROUP BY TO_CHAR(WDATE, 'YYYYMM'), CASE WHEN PATIENT_BUN IN ('입원', '외래') THEN PATIENT_BUN ELSE '기타' END               ");
            mParameter.AppendSql(" )                                                                                                                         ");
            mParameter.AppendSql(" ,	PIVOT_DATA AS                                                                                                        ");
            mParameter.AppendSql(" (                                                                                                                         ");
            mParameter.AppendSql(" SELECT *                                                                                                                  ");
            mParameter.AppendSql("   FROM TONG_DATA                                                                                                          ");
            mParameter.AppendSql("    PIVOT                                                                                                                  ");
            mParameter.AppendSql("	 (                                                                                                                       ");
            mParameter.AppendSql("	 	SUM(CNT)                                                                                                             ");
            mParameter.AppendSql("	   FOR GUBUN  IN ('외래' AS O_CNT, '입원'  AS I_CNT, '기타'  AS ETC_CNT)                                                   ");
            mParameter.AppendSql("	 )                                                                                                                       ");
            mParameter.AppendSql(" )                                                                                                                         ");
            mParameter.AppendSql(" SELECT YYYYMM                                                                                                             ");
            mParameter.AppendSql("	,	NVL(O_CNT, 0) +  NVL(I_CNT, 0) + NVL(ETC_CNT, 0) AS ALL_CNT                                                          ");
            mParameter.AppendSql(" 	,	O_CNT                                                                                                                ");
            mParameter.AppendSql(" 	,	I_CNT                                                                                                                ");
            mParameter.AppendSql(" 	,	ETC_CNT                                                                                                              ");
            mParameter.AppendSql("   FROM PIVOT_DATA                                                                                                         ");
            mParameter.AppendSql("   UNION ALL                                                                                                               ");
            mParameter.AppendSql(" SELECT '합계' AS YYYYMM                                                                                                    ");
            mParameter.AppendSql(" 	,   NVL(SUM(O_CNT), 0) +  NVL(SUM(I_CNT), 0) + NVL(SUM(ETC_CNT), 0) AS ALL_CNT                                           ");
            mParameter.AppendSql(" 	,	SUM(O_CNT) O_CNT                                                                                                     ");         
            mParameter.AppendSql(" 	,	SUM(I_CNT) I_CNT                                                                                                     ");          
            mParameter.AppendSql(" 	,	SUM(ETC_CNT) ETC_CNT                                                                                                 ");            
            mParameter.AppendSql("   FROM PIVOT_DATA                                                                                                         ");
            mParameter.AppendSql("  ORDER BY YYYYMM                                                                                                          ");
           
            mParameter.Add("SDATE", dtpSDATE.Value.ToString("yyyy-MM-dd"));
            mParameter.Add("EDATE", dtpEDATE.Value.ToString("yyyy-MM-dd"));

            return clsDB.ExecuteReader(mParameter, clsDB.DbCon);
        }

        /// <summary>
        /// 진료과별 보고 건수(건수 내림차순)
        /// </summary>
        /// <returns></returns>
        private List<Dictionary<string, object>> GetData2_1()
        {
            MParameter mParameter = new MParameter();
            mParameter.AppendSql("WITH DEPT_COUNT AS                                                                                                        ");
            mParameter.AppendSql("(                                                                                                                         ");
            mParameter.AppendSql("SELECT  DEPTCODE                                                                                                          ");
            mParameter.AppendSql("      , COUNT(DEPTCODE) AS SORT                                                                                           ");
            mParameter.AppendSql("  FROM KOSMOS_ADM.DRUG_ADR1                                                                                               ");
            mParameter.AppendSql(" WHERE WDATE >= TO_DATE(:SDATE)                                                                                           ");
            mParameter.AppendSql("   AND WDATE <= TO_DATE(:EDATE)                                                                                           ");
            mParameter.AppendSql(" GROUP BY DEPTCODE                                                                                                        ");
            mParameter.AppendSql(")                                                                                                                         ");
            mParameter.AppendSql("SELECT DEPTCODE                                                                                                           ");
            mParameter.AppendSql("    ,  SORT                                                                                                               ");
            mParameter.AppendSql("    ,  0 AS SORT2                                                                                                               ");
            mParameter.AppendSql("  FROM DEPT_COUNT                                                                                                         ");
            mParameter.AppendSql(" UNION ALL                                                                                                                ");
            mParameter.AppendSql("SELECT '계' AS DEPTCODE                                                                                                   ");
            mParameter.AppendSql("    ,  SUM(SORT) AS SORT                                                                                                               ");
            mParameter.AppendSql("    ,  1 AS SORT2                                                                                                               ");
            mParameter.AppendSql("  FROM DEPT_COUNT                                                                                                         ");
            mParameter.AppendSql(" ORDER BY SORT2, SORT DESC                                                                                                       ");

            mParameter.Add("SDATE", dtpSDATE.Value.ToString("yyyy-MM-dd"));
            mParameter.Add("EDATE", dtpEDATE.Value.ToString("yyyy-MM-dd"));

            return clsDB.ExecuteReader(mParameter, clsDB.DbCon);
        }

        /// <summary>
        /// 내과 보고 건수(건수 내림차순)
        /// </summary>
        /// <returns></returns>
        private List<Dictionary<string, object>> GetData2_2()
        {
            MParameter mParameter = new MParameter();
            mParameter.AppendSql("WITH DEPT_COUNT AS                                                                                                        ");
            mParameter.AppendSql("(                                                                                                                         ");
            mParameter.AppendSql("SELECT  DEPTCODE                                                                                                          ");
            mParameter.AppendSql("      , COUNT(DEPTCODE) AS SORT                                                                                           ");
            mParameter.AppendSql("  FROM KOSMOS_ADM.DRUG_ADR1                                                                                               ");
            mParameter.AppendSql(" WHERE WDATE >= TO_DATE(:SDATE)                                                                                           ");
            mParameter.AppendSql("   AND WDATE <= TO_DATE(:EDATE)                                                                                           ");
            mParameter.AppendSql("   AND DEPTCODE LIKE 'M%'                                                                                                 ");
            mParameter.AppendSql(" GROUP BY DEPTCODE                                                                                                        ");
            mParameter.AppendSql(")                                                                                                                         ");

            mParameter.AppendSql("SELECT DEPTCODE                                                                                                           ");
            mParameter.AppendSql("    ,  SORT                                                                                                               ");
            mParameter.AppendSql("    ,  0 AS SORT2                                                                                                         ");
            mParameter.AppendSql("  FROM DEPT_COUNT                                                                                                         ");
            mParameter.AppendSql(" UNION ALL                                                                                                                ");
            mParameter.AppendSql("SELECT '계' AS DEPTCODE                                                                                                    ");
            mParameter.AppendSql("    ,  SUM(SORT) AS SORT                                                                                                  ");
            mParameter.AppendSql("    ,  1 AS SORT2                                                                                                         ");
            mParameter.AppendSql("  FROM DEPT_COUNT                                                                                                         ");
            mParameter.AppendSql(" ORDER BY SORT2, SORT DESC, DEPTCODE DESC                                                                                 ");

            mParameter.Add("SDATE", dtpSDATE.Value.ToString("yyyy-MM-dd"));
            mParameter.Add("EDATE", dtpEDATE.Value.ToString("yyyy-MM-dd"));

            return clsDB.ExecuteReader(mParameter, clsDB.DbCon);
        }

        /// <summary>
        /// 진료과별 보고 건수(내과통합)
        /// </summary>
        /// <returns></returns>
        private List<Dictionary<string, object>> GetData2_3()
        {
            MParameter mParameter = new MParameter();
            mParameter.AppendSql("WITH DEPT_COUNT AS                                                                                                         ");
            mParameter.AppendSql("(                                                                                                                          ");
            mParameter.AppendSql("SELECT	CASE WHEN DEPTCODE LIKE 'M%' THEN 'MD' ELSE DEPTCODE END DEPTCODE                                                ");
            mParameter.AppendSql("	    ,	COUNT(DEPTCODE) AS SORT                                                                                          ");
            mParameter.AppendSql("  FROM KOSMOS_ADM.DRUG_ADR1                                                                                                ");
            mParameter.AppendSql(" WHERE WDATE >= TO_DATE(:SDATE)                                                                                            ");
            mParameter.AppendSql("   AND WDATE <= TO_DATE(:EDATE)                                                                                            ");
            mParameter.AppendSql("--   AND DEPTCODE LIKE 'M%'                                                                                                ");
            mParameter.AppendSql(" GROUP BY CASE WHEN DEPTCODE LIKE 'M%' THEN 'MD' ELSE DEPTCODE END                                                         ");
            mParameter.AppendSql(")                                                                                                                          ");

            mParameter.AppendSql("SELECT DEPTCODE                                                                                                           ");
            mParameter.AppendSql("    ,  SORT                                                                                                               ");
            mParameter.AppendSql("    ,  0 AS SORT2                                                                                                         ");
            mParameter.AppendSql("  FROM DEPT_COUNT                                                                                                         ");
            mParameter.AppendSql(" UNION ALL                                                                                                                ");
            mParameter.AppendSql("SELECT '계' AS DEPTCODE                                                                                                    ");
            mParameter.AppendSql("    ,  SUM(SORT) AS SORT                                                                                                  ");
            mParameter.AppendSql("    ,  1 AS SORT2                                                                                                         ");
            mParameter.AppendSql("  FROM DEPT_COUNT                                                                                                         ");
            mParameter.AppendSql(" ORDER BY SORT2, SORT DESC, DEPTCODE DESC                                                                                 ");


            mParameter.Add("SDATE", dtpSDATE.Value.ToString("yyyy-MM-dd"));
            mParameter.Add("EDATE", dtpEDATE.Value.ToString("yyyy-MM-dd"));

            return clsDB.ExecuteReader(mParameter, clsDB.DbCon);
        }

        /// <summary>
        /// 연령별, 성별 보고 건수
        /// </summary>
        /// <returns></returns>
        private List<Dictionary<string, object>> GetData3()
        {
            MParameter mParameter = new MParameter();
            mParameter.AppendSql(" WITH AGE_COUNT AS                                                                     ");
            mParameter.AppendSql("(                                                                                      ");
            mParameter.AppendSql("SELECT TO_NUMBER(SUBSTR(AGESEX, 0, INSTR(AGESEX, '/') - 1)) AS AGE                     ");
            mParameter.AppendSql("	,	TRIM(SUBSTR(AGESEX, INSTR(AGESEX, '/') + 1)) AS SEX                              ");
            mParameter.AppendSql("  FROM kosmos_adm.DRUG_ADR1                                                            ");
            mParameter.AppendSql(" WHERE WDATE >= TO_DATE(:SDATE)                                                        ");
            mParameter.AppendSql("   AND WDATE <= TO_DATE(:EDATE)                                                        ");
            mParameter.AppendSql("   AND AGESEX LIKE '%/%'                                                               ");
            mParameter.AppendSql(")                                                                                      ");
            mParameter.AppendSql(",	SUM_GBN AS                                                                           ");
            mParameter.AppendSql("(                                                                                      ");
            mParameter.AppendSql("	SELECT CASE WHEN AGE >= 0 AND AGE <= 9 AND SEX = 'M' THEN 0                          ");
            mParameter.AppendSql("				WHEN AGE >= 10 AND AGE <= 19 AND SEX = 'M' THEN 1                        ");
            mParameter.AppendSql("				WHEN AGE >= 20 AND AGE <= 29 AND SEX = 'M' THEN 2                        ");
            mParameter.AppendSql("				WHEN AGE >= 30 AND AGE <= 39 AND SEX = 'M' THEN 3                        ");
            mParameter.AppendSql("				WHEN AGE >= 40 AND AGE <= 49 AND SEX = 'M' THEN 4                        ");
            mParameter.AppendSql("				WHEN AGE >= 50 AND AGE <= 59 AND SEX = 'M' THEN 5                        ");
            mParameter.AppendSql("				WHEN AGE >= 60 AND AGE <= 69 AND SEX = 'M' THEN 6                        ");
            mParameter.AppendSql("				WHEN AGE >= 70 AND AGE <= 79 AND SEX = 'M' THEN 7                        ");
            mParameter.AppendSql("				WHEN AGE >= 80 AND AGE <= 89 AND SEX = 'M' THEN 8                        ");
            mParameter.AppendSql("				WHEN AGE >= 90 AND AGE <= 99 AND SEX = 'M' THEN 9                        ");
            mParameter.AppendSql("				WHEN AGE >= 100 AND SEX = 'M' THEN 10                                    ");
            mParameter.AppendSql("			END GBN                                                                      ");
            mParameter.AppendSql("		, CASE WHEN AGE >= 0 AND AGE <= 9 AND SEX = 'F' THEN 0                           ");
            mParameter.AppendSql("				WHEN AGE >= 10 AND AGE <= 19 AND SEX = 'F' THEN 1                        ");
            mParameter.AppendSql("				WHEN AGE >= 20 AND AGE <= 29 AND SEX = 'F' THEN 2                        ");
            mParameter.AppendSql("				WHEN AGE >= 30 AND AGE <= 39 AND SEX = 'F' THEN 3                        ");
            mParameter.AppendSql("				WHEN AGE >= 40 AND AGE <= 49 AND SEX = 'F' THEN 4                        ");
            mParameter.AppendSql("				WHEN AGE >= 50 AND AGE <= 59 AND SEX = 'F' THEN 5                        ");
            mParameter.AppendSql("				WHEN AGE >= 60 AND AGE <= 69 AND SEX = 'F' THEN 6                        ");
            mParameter.AppendSql("				WHEN AGE >= 70 AND AGE <= 79 AND SEX = 'F' THEN 7                        ");
            mParameter.AppendSql("				WHEN AGE >= 80 AND AGE <= 89 AND SEX = 'F' THEN 8                        ");
            mParameter.AppendSql("				WHEN AGE >= 90 AND AGE <= 99 AND SEX = 'F' THEN 9                        ");
            mParameter.AppendSql("				WHEN AGE >= 100 AND SEX = 'F' THEN 10                                    ");
            mParameter.AppendSql("			END GBN2                                                                     ");
            mParameter.AppendSql("	  FROM AGE_COUNT                                                                     ");
            mParameter.AppendSql(")                                                                                      ");
            mParameter.AppendSql(",	ROW_DATA AS                                                                          ");
            mParameter.AppendSql("(                                                                                      ");
            mParameter.AppendSql("SELECT LEVEL AS ROW_NUM                                                                ");
            mParameter.AppendSql("  FROM DUAL A                                                                          ");
            mParameter.AppendSql("CONNECT BY LEVEL <= 11                                                                 ");
            mParameter.AppendSql(")                                                                                      ");
            mParameter.AppendSql("SELECT (A.ROW_NUM - 1) AS ROW_NUM                                                      ");
            mParameter.AppendSql("	, (SELECT COUNT(GBN) FROM SUM_GBN WHERE GBN = A.ROW_NUM - 1) AS GBN1                 ");
            mParameter.AppendSql("	, (SELECT COUNT(GBN2) FROM SUM_GBN WHERE GBN2 = A.ROW_NUM - 1) AS GBN2               ");
            mParameter.AppendSql("  FROM ROW_DATA A                                                                      ");
            mParameter.AppendSql(" UNION ALL                                                                             ");
            mParameter.AppendSql("SELECT 11 AS ROW_NUM                                                                   ");
            mParameter.AppendSql("	, (SELECT COUNT(SEX) FROM AGE_COUNT WHERE SEX = 'M') AS GBN1                         ");
            mParameter.AppendSql("	, (SELECT COUNT(SEX) FROM AGE_COUNT WHERE SEX = 'F') AS GBN2                         ");
            mParameter.AppendSql("  FROM DUAL A                                                                          ");

            mParameter.Add("SDATE", dtpSDATE.Value.ToString("yyyy-MM-dd"));
            mParameter.Add("EDATE", dtpEDATE.Value.ToString("yyyy-MM-dd"));

            return clsDB.ExecuteReader(mParameter, clsDB.DbCon);
        }

        /// <summary>
        /// 이상반응 의심약물의 효능별 분류 
        /// </summary>
        /// <returns></returns>
        private List<Dictionary<string, object>> GetData4()
        {
            MParameter mParameter = new MParameter();                                                                                                                           
                                                                                                                                                                                
           mParameter.AppendSql("WITH ADR_SEQNO AS                                                                                                                             ");
           mParameter.AppendSql("(                                                                                                                                              ");
           mParameter.AppendSql(" 	SELECT SEQNO                                                                                                                                ");
           mParameter.AppendSql("	  FROM KOSMOS_ADM.DRUG_ADR1                                                                                                                 ");
           mParameter.AppendSql("	 WHERE WDATE >= TO_DATE(:SDATE)                                                                                                             ");
           mParameter.AppendSql("	   AND WDATE <= TO_DATE(:EDATE)                                                                                                             ");
           mParameter.AppendSql(")                                                                                                                                              ");
           mParameter.AppendSql(", SUNGBUN_DATA AS                                                                                                                              ");
           mParameter.AppendSql("(                                                                                                                                              ");
           mParameter.AppendSql("SELECT	B.SUNGBUN                                                                                                                               ");
           mParameter.AppendSql("	,	COUNT(B.SUNGBUN) CNT                                                                                                                    ");
           mParameter.AppendSql("  FROM KOSMOS_ADM.DRUG_ADR1_ORDER A                                                                                                            ");
           mParameter.AppendSql(" INNER JOIN KOSMOS_ADM.DRUG_JEP B                                                                                                              ");
           mParameter.AppendSql("   ON A.SUCODE = B.JEPCODE                                                                                                                     ");
           mParameter.AppendSql(" WHERE SEQNO  IN                                                                                                                               ");
           mParameter.AppendSql(" (                                                                                                                                             ");
           mParameter.AppendSql("    SELECT SEQNO                                                                                                                               ");
           mParameter.AppendSql("     FROM ADR_SEQNO                                                                                                                            ");
           mParameter.AppendSql(" )                                                                                                                                             ");
           mParameter.AppendSql(" GROUP BY B.SUNGBUN                                                                                                                            ");
           mParameter.AppendSql(" UNION ALL                                                                                                                                     ");
           mParameter.AppendSql("SELECT	B.SUNGBUN                                                                                                                               ");
           mParameter.AppendSql("	,	COUNT(B.SUNGBUN) CNT                                                                                                                    ");
           mParameter.AppendSql("  FROM KOSMOS_ADM.DRUG_ADR1_ORDER_JO A                                                                                                         ");
           mParameter.AppendSql(" INNER JOIN KOSMOS_ADM.DRUG_JEP B                                                                                                              ");
           mParameter.AppendSql("   ON A.SUCODE = B.JEPCODE                                                                                                                     ");
           mParameter.AppendSql(" WHERE SEQNO  IN                                                                                                                               ");
           mParameter.AppendSql(" (                                                                                                                                             ");
           mParameter.AppendSql("   SELECT SEQNO                                                                                                                                ");
           mParameter.AppendSql("     FROM ADR_SEQNO                                                                                                                            ");
           mParameter.AppendSql(" )                                                                                                                                             ");
           mParameter.AppendSql(" GROUP BY B.SUNGBUN                                                                                                                            ");
           mParameter.AppendSql(")                                                                                                                                              ");
           mParameter.AppendSql("SELECT SUNGBUN                                                                                                                                 ");
           mParameter.AppendSql("	,	(SELECT CLASSNAME FROM KOSMOS_PMPA.BAS_CLASS WHERE CLASSCODE = A.SUNGBUN) AS SUNGBUN_NM                                                 ");
           mParameter.AppendSql("	,	CNT                                                                                                                                     ");
           mParameter.AppendSql("	,	0 AS SORT                                                                                                                               ");
           mParameter.AppendSql("  FROM SUNGBUN_DATA A                                                                                                                          ");
           mParameter.AppendSql(" UNION ALL                                                                                                                                     ");
           mParameter.AppendSql("SELECT '합계' AS SUNGBUN                                                                                                                        ");
           mParameter.AppendSql("	,	'' AS SUNGBUN_NM                                                                                                                        ");
           mParameter.AppendSql("	,	SUM(CNT) AS CNT                                                                                                                         ");
           mParameter.AppendSql("	,	1 AS SORT	                                                                                                                            ");
           mParameter.AppendSql("  FROM SUNGBUN_DATA A                                                                                                                          ");
           mParameter.AppendSql(" ORDER BY SORT, CNT DESC, SUNGBUN                                                                                                              ");

            mParameter.Add("SDATE", dtpSDATE.Value.ToString("yyyy-MM-dd"));
            mParameter.Add("EDATE", dtpEDATE.Value.ToString("yyyy-MM-dd"));

            return clsDB.ExecuteReader(mParameter, clsDB.DbCon);
        }

        /// <summary>
        /// X선조영제 약품별 보고건수(내림차순) 건수, 같은경우 코드 
        /// </summary>
        /// <returns></returns>
        private List<Dictionary<string, object>> GetData5()
        {
            MParameter mParameter = new MParameter();
            mParameter.AppendSql("WITH JO_DATA AS                                                                           ");
            mParameter.AppendSql("(                                                                                         ");
            mParameter.AppendSql("SELECT SUCODE                                                                             ");
            mParameter.AppendSql("	,	(SELECT JEPNAME FROM KOSMOS_ADM.DRUG_JEP WHERE JEPCODE = A.SUCODE) AS JEPNAMEK      ");
            mParameter.AppendSql("	,	COUNT(SUCODE) CNT                                                                   ");
            mParameter.AppendSql("     FROM KOSMOS_ADM.DRUG_ADR1_ORDER_JO A                                                 ");
            mParameter.AppendSql("    WHERE SEQNO IN                                                                        ");
            mParameter.AppendSql("    (                                                                                     ");
            mParameter.AppendSql("	   SELECT SEQNO                                                                         ");
            mParameter.AppendSql("	     FROM KOSMOS_ADM.DRUG_ADR1                                                          ");
            mParameter.AppendSql("	    WHERE WDATE >= TO_DATE(:SDATE)                                                      ");
            mParameter.AppendSql("		  AND WDATE <= TO_DATE(:EDATE)                                                      ");
            mParameter.AppendSql("    )                                                                                     ");
            mParameter.AppendSql("     AND SUCODE IS NOT NULL                                                               ");
            mParameter.AppendSql("  GROUP BY SUCODE                                                                         ");
            mParameter.AppendSql(")                                                                                         ");
            mParameter.AppendSql("SELECT SUCODE                                                                             ");
            mParameter.AppendSql("	,	JEPNAMEK                                                                            ");
            mParameter.AppendSql("	,	CNT                                                                                 ");
            mParameter.AppendSql("  FROM JO_DATA                                                                            ");
            mParameter.AppendSql(" ORDER BY CNT DESC, SUCODE                                                                ");

            mParameter.Add("SDATE", dtpSDATE.Value.ToString("yyyy-MM-dd"));
            mParameter.Add("EDATE", dtpEDATE.Value.ToString("yyyy-MM-dd"));

            return clsDB.ExecuteReader(mParameter, clsDB.DbCon);
        }

        /// <summary>
        /// 이상반응 증상별 분류  
        /// </summary>
        /// <returns></returns>
        private List<Dictionary<string, object>> GetData6()
        {
            MParameter mParameter = new MParameter();
            mParameter.AppendSql("WITH ADR_COUNT AS                                                                                                           ");
            mParameter.AppendSql("(                                                                                                                           ");
            mParameter.AppendSql(" SELECT B1.NAME                                                                                                             ");
            mParameter.AppendSql(" 	,	COUNT(B1.NAME) CNT                                                                                                    ");
            mParameter.AppendSql("   FROM KOSMOS_PMPA.BAS_BCODE B1                                                                                            ");
            mParameter.AppendSql("  INNER JOIN                                                                                                                ");
            mParameter.AppendSql("(SELECT * FROM                                                                                                              ");
            mParameter.AppendSql("(SELECT SEQNO,                                                                                                              ");
            mParameter.AppendSql("        RACT_A1, RACT_A2, RACT_A3, RACT_A4, RACT_A5, RACT_A6,                                                               ");
            mParameter.AppendSql("        RACT_B1, RACT_B2, RACT_B3, RACT_B4, RACT_B5, RACT_B6, RACT_B7, RACT_B8, RACT_B9,                                    ");
            mParameter.AppendSql("        RACT_C1, RACT_C2, RACT_C3, RACT_C4, RACT_C5, RACT_C6, RACT_C7, RACT_C8,                                             ");
            mParameter.AppendSql("        RACT_D1, RACT_D2, RACT_D3, RACT_D4, RACT_D5, RACT_D6, RACT_D7,                                                      ");
            mParameter.AppendSql("        RACT_E1, RACT_E2, RACT_E3, RACT_E4, RACT_E5, RACT_E6, RACT_E7,                                                      ");
            mParameter.AppendSql("        RACT_F1, RACT_F2,                                                                                                   ");
            mParameter.AppendSql("        RACT_G1, RACT_G2, RACT_G3,                                                                                          ");
            mParameter.AppendSql("        RACT_H1, RACT_H2, RACT_H3, RACT_H4,                                                                                 ");
            mParameter.AppendSql("        RACT_I1, RACT_I2, RACT_I3, RACT_I4,                                                                                 ");
            mParameter.AppendSql("        RACT_J1, RACT_J2, RACT_J3, RACT_J4, RACT_J5, RACT_J6, RACT_J7, RACT_J8, RACT_J9, RACT_J10,                          ");
            mParameter.AppendSql("        RACT_J11, RACT_J12, RACT_J13, RACT_J14, RACT_J15, RACT_J16,                                                         ");
            mParameter.AppendSql("        RACT_K1, RACT_K2, RACT_K3, RACT_K4, RACT_K5, RACT_K6, RACT_K7,                                                      ");
            mParameter.AppendSql("        RACT_L1, RACT_L2, RACT_L3                                                                                           ");
            mParameter.AppendSql("    FROM KOSMOS_ADM.DRUG_ADR1 T1                                                                                            ");
            mParameter.AppendSql("    WHERE T1.SEQNO IN                                                                                                       ");
            mParameter.AppendSql("    (                                                                                                                       ");
            mParameter.AppendSql("    	SELECT SEQNO                                                                                                          ");
            mParameter.AppendSql("		  FROM KOSMOS_ADM.DRUG_ADR1                                                                                           ");
            mParameter.AppendSql("		 WHERE WDATE >= TO_DATE(:SDATE)                                                                                       ");
            mParameter.AppendSql("		   AND WDATE <= TO_DATE(:EDATE)                                                                                       ");
            mParameter.AppendSql("    )                                                                                                                       ");
            mParameter.AppendSql("    )TEMP                                                                                                                   ");
            mParameter.AppendSql("UNPIVOT(VAL FOR CODE IN(RACT_A1, RACT_A2, RACT_A3, RACT_A4, RACT_A5, RACT_A6,                                               ");
            mParameter.AppendSql("                        RACT_B1, RACT_B2, RACT_B3, RACT_B4, RACT_B5, RACT_B6, RACT_B7, RACT_B8, RACT_B9,                    ");
            mParameter.AppendSql("                        RACT_C1, RACT_C2, RACT_C3, RACT_C4, RACT_C5, RACT_C6, RACT_C7, RACT_C8,                             ");
            mParameter.AppendSql("                        RACT_D1, RACT_D2, RACT_D3, RACT_D4, RACT_D5, RACT_D6, RACT_D7,                                      ");
            mParameter.AppendSql("                        RACT_E1, RACT_E2, RACT_E3, RACT_E4, RACT_E5, RACT_E6, RACT_E7,                                      ");
            mParameter.AppendSql("                        RACT_F1, RACT_F2,                                                                                   ");
            mParameter.AppendSql("                        RACT_G1, RACT_G2, RACT_G3,                                                                          ");
            mParameter.AppendSql("                        RACT_H1, RACT_H2, RACT_H3, RACT_H4,                                                                 ");
            mParameter.AppendSql("                        RACT_I1, RACT_I2, RACT_I3, RACT_I4,                                                                 ");
            mParameter.AppendSql("                        RACT_J1, RACT_J2, RACT_J3, RACT_J4, RACT_J5,                                                        ");
            mParameter.AppendSql("                        RACT_J6, RACT_J7, RACT_J8, RACT_J9, RACT_J10,                                                       ");
            mParameter.AppendSql("                        RACT_J11, RACT_J12, RACT_J13, RACT_J14, RACT_J15, RACT_J16,                                         ");
            mParameter.AppendSql("                        RACT_K1, RACT_K2, RACT_K3, RACT_K4, RACT_K5, RACT_K6, RACT_K7,                                      ");
            mParameter.AppendSql("                        RACT_L1, RACT_L2, RACT_L3))) B2                                                                     ");
            mParameter.AppendSql("    ON B1.CODE = B2.CODE                                                                                                    ");
            mParameter.AppendSql("WHERE B1.GUBUN = 'ETC_ADR_이상반응'                                                                                          ");
            mParameter.AppendSql("    AND B2.VAL = '1'                                                                                                        ");
            mParameter.AppendSql("GROUP BY B1.NAME                                                                                                            ");
            mParameter.AppendSql(")                                                                                                                           ");
            mParameter.AppendSql("SELECT NAME                                                                                                                 ");
            mParameter.AppendSql("	,	CNT                                                                                                                   ");
            mParameter.AppendSql("  FROM ADR_COUNT                                                                                                            ");
            mParameter.AppendSql(" ORDER BY CNT DESC, NAME                                                                                                    ");
            
            mParameter.Add("SDATE", dtpSDATE.Value.ToString("yyyy-MM-dd"));
            mParameter.Add("EDATE", dtpEDATE.Value.ToString("yyyy-MM-dd"));

            return clsDB.ExecuteReader(mParameter, clsDB.DbCon);
        }

        /// <summary>
        /// 7. 의심약물 효능별 세부 내용(효능별 분류 내림차순=>약품코드별 보고건수 내림차순)
        /// </summary>
        /// <returns></returns>
        private List<Dictionary<string, object>> GetData7()
        {
            MParameter mParameter = new MParameter();
            mParameter.AppendSql("WITH ADR_SEQNO AS                                                                                                                              ");
            mParameter.AppendSql("(                                                                                                                                              ");
            mParameter.AppendSql(" 	SELECT SEQNO                                                                                                                                 ");
            mParameter.AppendSql("	  FROM KOSMOS_ADM.DRUG_ADR1                                                                                                                  ");
            mParameter.AppendSql("	 WHERE WDATE >= TO_DATE(:SDATE)                                                                                                              ");
            mParameter.AppendSql("	   AND WDATE <= TO_DATE(:EDATE)                                                                                                              ");
            mParameter.AppendSql(")                                                                                                                                              ");
            mParameter.AppendSql(",	ORDER_DATA AS                                                                                                                                ");
            mParameter.AppendSql("(                                                                                                                                              ");
            mParameter.AppendSql("	SELECT	SUCODE                                                                                                                               ");
            mParameter.AppendSql("		,	SUNAMEK                                                                                                                              ");
            mParameter.AppendSql("		,	SEQNO                                                                                                                                ");
            mParameter.AppendSql("		,	(SELECT SUNGBUN FROM KOSMOS_ADM.DRUG_JEP WHERE JEPCODE = A.SUCODE) AS SUNGBUN		                                                 ");
            mParameter.AppendSql("	  FROM KOSMOS_ADM.DRUG_ADR1_ORDER A	                                                                                                         ");
            mParameter.AppendSql("	WHERE SUCODE IS NOT NULL                                                                                                                     ");
            mParameter.AppendSql("	  AND EXISTS                                                                                                                                 ");
            mParameter.AppendSql("	  (                                                                                                                                          ");
            mParameter.AppendSql("	  	 SELECT 1                                                                                                                                ");
            mParameter.AppendSql("	  	   FROM ADR_SEQNO SUB                                                                                                                    ");
            mParameter.AppendSql("	      WHERE SUB.SEQNO = A.SEQNO                                                                                                              ");
            mParameter.AppendSql("	  )                                                                                                                                          ");
            mParameter.AppendSql("	 UNION ALL                                                                                                                                   ");
            mParameter.AppendSql("	SELECT	SUCODE                                                                                                                               ");
            mParameter.AppendSql("		,	SUNAMEK                                                                                                                              ");
            mParameter.AppendSql("		,	SEQNO                                                                                                                                ");
            mParameter.AppendSql("		,	(SELECT SUNGBUN FROM KOSMOS_ADM.DRUG_JEP WHERE JEPCODE = A.SUCODE) AS SUNGBUN                                                        ");
            mParameter.AppendSql("	  FROM KOSMOS_ADM.DRUG_ADR1_ORDER_JO A	                                                                                                     ");
            mParameter.AppendSql("	WHERE SUCODE IS NOT NULL                                                                                                                     ");
            mParameter.AppendSql("	  AND EXISTS                                                                                                                                 ");
            mParameter.AppendSql("	  (                                                                                                                                          ");
            mParameter.AppendSql("	  	 SELECT 1                                                                                                                                ");
            mParameter.AppendSql("	  	   FROM ADR_SEQNO SUB                                                                                                                    ");
            mParameter.AppendSql("	      WHERE SUB.SEQNO = A.SEQNO                                                                                                              ");
            mParameter.AppendSql("	  )                                                                                                                                          ");
            mParameter.AppendSql(")                                                                                                                                              ");
            mParameter.AppendSql(", ADR_LIST AS                                                                                                                                  ");
            mParameter.AppendSql("(                                                                                                                                              ");
            mParameter.AppendSql(" SELECT SUCODE                                                                                                                                 ");
            mParameter.AppendSql("	,	TO_CHAR(WM_CONCAT(DISTINCT B1.NAME)) AS LIST                                                                                             ");
            mParameter.AppendSql("   FROM KOSMOS_PMPA.BAS_BCODE B1                                                                                                               ");
            mParameter.AppendSql("  INNER JOIN                                                                                                                                   ");
            mParameter.AppendSql("(SELECT * FROM                                                                                                                                 ");
            mParameter.AppendSql("	(SELECT SEQNO,                                                                                                                               ");
            mParameter.AppendSql("	        RACT_A1, RACT_A2, RACT_A3, RACT_A4, RACT_A5, RACT_A6,                                                                                ");
            mParameter.AppendSql("	        RACT_B1, RACT_B2, RACT_B3, RACT_B4, RACT_B5, RACT_B6, RACT_B7, RACT_B8, RACT_B9,                                                     ");
            mParameter.AppendSql("	        RACT_C1, RACT_C2, RACT_C3, RACT_C4, RACT_C5, RACT_C6, RACT_C7, RACT_C8,                                                              ");
            mParameter.AppendSql("	        RACT_D1, RACT_D2, RACT_D3, RACT_D4, RACT_D5, RACT_D6, RACT_D7,                                                                       ");
            mParameter.AppendSql("	        RACT_E1, RACT_E2, RACT_E3, RACT_E4, RACT_E5, RACT_E6, RACT_E7,                                                                       ");
            mParameter.AppendSql("	        RACT_F1, RACT_F2,                                                                                                                    ");
            mParameter.AppendSql("	        RACT_G1, RACT_G2, RACT_G3,                                                                                                           ");
            mParameter.AppendSql("	        RACT_H1, RACT_H2, RACT_H3, RACT_H4,                                                                                                  ");
            mParameter.AppendSql("	        RACT_I1, RACT_I2, RACT_I3, RACT_I4,                                                                                                  ");
            mParameter.AppendSql("	        RACT_J1, RACT_J2, RACT_J3, RACT_J4, RACT_J5, RACT_J6, RACT_J7, RACT_J8, RACT_J9, RACT_J10,                                           ");
            mParameter.AppendSql("	        RACT_J11, RACT_J12, RACT_J13, RACT_J14, RACT_J15, RACT_J16,                                                                          ");
            mParameter.AppendSql("	        RACT_K1, RACT_K2, RACT_K3, RACT_K4, RACT_K5, RACT_K6, RACT_K7,                                                                       ");
            mParameter.AppendSql("	        RACT_L1, RACT_L2, RACT_L3                                                                                                            ");
            mParameter.AppendSql("	    FROM KOSMOS_ADM.DRUG_ADR1 T1                                                                                                             ");
            mParameter.AppendSql("	   WHERE SEQNO IN                                                                                                                            ");
            mParameter.AppendSql("	   (                                                                                                                                         ");
            mParameter.AppendSql("	   		SELECT SEQNO                                                                                                                         ");
            mParameter.AppendSql("	   		  FROM ADR_SEQNO                                                                                                                     ");
            mParameter.AppendSql("	   )                                                                                                                                         ");
            mParameter.AppendSql("	)TEMP                                                                                                                                        ");
            mParameter.AppendSql("UNPIVOT(VAL FOR CODE IN(RACT_A1, RACT_A2, RACT_A3, RACT_A4, RACT_A5, RACT_A6,                                                                  ");
            mParameter.AppendSql("                        RACT_B1, RACT_B2, RACT_B3, RACT_B4, RACT_B5, RACT_B6, RACT_B7, RACT_B8, RACT_B9,                                       ");
            mParameter.AppendSql("                        RACT_C1, RACT_C2, RACT_C3, RACT_C4, RACT_C5, RACT_C6, RACT_C7, RACT_C8,                                                ");
            mParameter.AppendSql("                        RACT_D1, RACT_D2, RACT_D3, RACT_D4, RACT_D5, RACT_D6, RACT_D7,                                                         ");
            mParameter.AppendSql("                        RACT_E1, RACT_E2, RACT_E3, RACT_E4, RACT_E5, RACT_E6, RACT_E7,                                                         ");
            mParameter.AppendSql("                        RACT_F1, RACT_F2,                                                                                                      ");
            mParameter.AppendSql("                        RACT_G1, RACT_G2, RACT_G3,                                                                                             ");
            mParameter.AppendSql("                        RACT_H1, RACT_H2, RACT_H3, RACT_H4,                                                                                    ");
            mParameter.AppendSql("                        RACT_I1, RACT_I2, RACT_I3, RACT_I4,                                                                                    ");
            mParameter.AppendSql("                        RACT_J1, RACT_J2, RACT_J3, RACT_J4, RACT_J5,                                                                           ");
            mParameter.AppendSql("                        RACT_J6, RACT_J7, RACT_J8, RACT_J9, RACT_J10,                                                                          ");
            mParameter.AppendSql("                        RACT_J11, RACT_J12, RACT_J13, RACT_J14, RACT_J15, RACT_J16,                                                            ");
            mParameter.AppendSql("                        RACT_K1, RACT_K2, RACT_K3, RACT_K4, RACT_K5, RACT_K6, RACT_K7,                                                         ");
            mParameter.AppendSql("                        RACT_L1, RACT_L2, RACT_L3))) B2                                                                                        ");
            mParameter.AppendSql("    ON B1.CODE = B2.CODE                                                                                                                       ");
            mParameter.AppendSql("  INNER JOIN ORDER_DATA B3                                                                                                                     ");
            mParameter.AppendSql("     ON B2.SEQNO = B3.SEQNO                                                                                                                    ");
            mParameter.AppendSql("WHERE B1.GUBUN = 'ETC_ADR_이상반응'                                                                                                             ");
            mParameter.AppendSql("    AND B2.VAL = '1'                                                                                                                           ");
            mParameter.AppendSql("GROUP BY B3.SUCODE                                                                                                                             ");
            mParameter.AppendSql(")                                                                                                                                              ");
            mParameter.AppendSql(", SUNGBUN_DATA AS                                                                                                                              ");
            mParameter.AppendSql("(                                                                                                                                              ");
            mParameter.AppendSql("SELECT	B.SUNGBUN                                                                                                                            ");
            mParameter.AppendSql("	,	COUNT(B.SUNGBUN) CNT                                                                                                                     ");
            mParameter.AppendSql("  FROM KOSMOS_ADM.DRUG_ADR1_ORDER A                                                                                                            ");
            mParameter.AppendSql(" INNER JOIN KOSMOS_ADM.DRUG_JEP B                                                                                                              ");
            mParameter.AppendSql("   ON A.SUCODE = B.JEPCODE                                                                                                                     ");
            mParameter.AppendSql(" WHERE SEQNO  IN                                                                                                                               ");
            mParameter.AppendSql(" (                                                                                                                                             ");
            mParameter.AppendSql("    SELECT SEQNO                                                                                                                               ");
            mParameter.AppendSql("     FROM ADR_SEQNO                                                                                                                            ");
            mParameter.AppendSql(" )                                                                                                                                             ");
            mParameter.AppendSql(" GROUP BY B.SUNGBUN                                                                                                                            ");
            mParameter.AppendSql(" UNION ALL                                                                                                                                     ");
            mParameter.AppendSql("SELECT	B.SUNGBUN                                                                                                                            ");
            mParameter.AppendSql("	,	COUNT(B.SUNGBUN) CNT                                                                                                                     ");
            mParameter.AppendSql("  FROM KOSMOS_ADM.DRUG_ADR1_ORDER_JO A                                                                                                         ");
            mParameter.AppendSql(" INNER JOIN KOSMOS_ADM.DRUG_JEP B                                                                                                              ");
            mParameter.AppendSql("   ON A.SUCODE = B.JEPCODE                                                                                                                     ");
            mParameter.AppendSql(" WHERE SEQNO  IN                                                                                                                               ");
            mParameter.AppendSql(" (                                                                                                                                             ");
            mParameter.AppendSql("   SELECT SEQNO                                                                                                                                ");
            mParameter.AppendSql("     FROM ADR_SEQNO                                                                                                                            ");
            mParameter.AppendSql(" )                                                                                                                                             ");
            mParameter.AppendSql(" GROUP BY B.SUNGBUN                                                                                                                            ");
            mParameter.AppendSql(")                                                                                                                                              ");
            mParameter.AppendSql("SELECT	A.SUNGBUN                                                                                                                            ");
            mParameter.AppendSql("	,	(SELECT CNT FROM SUNGBUN_DATA WHERE SUNGBUN = A.SUNGBUN) AS SUNGBUN_CNT                                                                  ");
            mParameter.AppendSql("	,	A.SUCODE                                                                                                                                 ");
            mParameter.AppendSql("	,	A.SUNAMEK                                                                                                                                ");
            mParameter.AppendSql("	,	REPLACE(C.SNAME, '/', ',') AS SUNGBUN_NM                                                                                                 ");
            mParameter.AppendSql("	,	D.LIST		                                                                                                                             ");
            mParameter.AppendSql("	,	COUNT(A.SUCODE) CNT                                                                                                                      ");
            mParameter.AppendSql("  FROM ORDER_DATA A                                                                                                                            ");
            mParameter.AppendSql(" INNER JOIN KOSMOS_OCS.OCS_DRUGINFO_NEW C                                                                                                      ");
            mParameter.AppendSql("    ON A.SUCODE = C.SUNEXT                                                                                                                     ");
            mParameter.AppendSql(" INNER JOIN ADR_LIST D                                                                                                                         ");
            mParameter.AppendSql("    ON A.SUCODE = D.SUCODE                                                                                                                     ");
            mParameter.AppendSql(" GROUP BY A.SUNGBUN, A.SUCODE, A.SUNAMEK, C.SNAME, D.LIST                                                                                      ");
            mParameter.AppendSql(" ORDER BY SUNGBUN_CNT DESC, CNT DESC, SUCODE                                                                                                   ");

            mParameter.Add("SDATE", dtpSDATE.Value.ToString("yyyy-MM-dd"));
            mParameter.Add("EDATE", dtpEDATE.Value.ToString("yyyy-MM-dd"));

            return clsDB.ExecuteReader(mParameter, clsDB.DbCon);
        }

    }
}
