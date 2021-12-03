using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;

namespace ComLibB
{
    public partial class frmOcsCpGoalStaics : Form, MainFormMessage
    {
        int mListColCnt = 8;

        #region //MainFormMessage
        string mPara1 = "";
        public MainFormMessage mCallForm = null;
        public void MsgActivedForm(Form frm)
        {

        }
        public void MsgUnloadForm(Form frm)
        {

        }
        public void MsgFormClear()
        {

        }
        public void MsgSendPara(string strPara)
        {

        }
        #endregion //MainFormMessage


        public frmOcsCpGoalStaics()
        {
            InitializeComponent();
        }
        public frmOcsCpGoalStaics(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }

        private void frmOcsCpGoalStaics_Load(object sender, EventArgs e)
        {
            ////폼 권한 조회
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}

            ////폼 기본값 세팅 등
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            string strCurDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            dtpSDate.Value = Convert.ToDateTime(strCurDate);
            dtpEDate.Value = dtpSDate.Value;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}
            GetDataAll();
            GetDataViolAll();
            GetDataAvrAll();
            GetDataResultAll();
            GetDatListAll();
            GetDataEtcAll();
        }

        private void GetDataAll()
        {
            GetDataAllQuery(ssAll_All_Sheet1);
            GetDataAllQuery(ssAll_Stroke_Sheet1);
            GetDataAllQuery(ssAll_STMI_Sheet1);
            GetDataAllQuery(ssAll_Seizure_Sheet1);
            GetDataAllQuery(ssAll_UGI_Sheet1);
            GetDataAllQuery(ssAll_Double_Sheet1);
            GetDataAllQuery(ssAll_Broken_Sheet1);
        }

        private void GetDataAllQuery(FarPoint.Win.Spread.SheetView Spd)
        {
            Spd.Cells[0, 0].Text = "";
            Spd.Cells[0, 1].Text = "";
            Spd.Cells[0, 2].Text = "";


            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                string strSdate = dtpSDate.Value.ToString("yyyy-MM-dd");
                string strEdate = dtpEDate.Value.ToString("yyyy-MM-dd");

                #region //위반 Count
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    COUNT(X.CPNO) AS CNT ";
                SQL = SQL + ComNum.VBLF + "FROM ( ";
                SQL = SQL + ComNum.VBLF + "    SELECT  ";
                SQL = SQL + ComNum.VBLF + "        R.CPNO ";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.OCS_CP_RECORD R  ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.OCS_CP_VALUE V ";
                SQL = SQL + ComNum.VBLF + "        ON R.CPNO = V.CPNO ";
                SQL = SQL + ComNum.VBLF + "        AND V.VALGB = '0' ";
                SQL = SQL + ComNum.VBLF + "        AND V.CPRSTN > 0 ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.OCS_CP_SUB S ";
                SQL = SQL + ComNum.VBLF + "        ON V.CODE = S.CODE  ";
                SQL = SQL + ComNum.VBLF + "        AND S.CPCODE = R.CPCODE ";
                SQL = SQL + ComNum.VBLF + "        AND S.GUBUN = '06' ";
                SQL = SQL + ComNum.VBLF + "        AND S.TYPE = '시각' ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.NUR_ER_PATIENT N  ";
                SQL = SQL + ComNum.VBLF + "         ON R.PTNO = N.PANO  ";
                SQL = SQL + ComNum.VBLF + "         AND R.BDATE = N.JDATE  ";
                SQL = SQL + ComNum.VBLF + "         AND R.INTIME = N.INTIME  ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.BAS_BASCD B  ";
                SQL = SQL + ComNum.VBLF + "        ON R.CPCODE = B.BASCD  ";
                SQL = SQL + ComNum.VBLF + "        AND B.GRPCDB = 'CP관리'  ";
                SQL = SQL + ComNum.VBLF + "        AND B.GRPCD = 'CP코드관리'  ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.BAS_USER U ";
                SQL = SQL + ComNum.VBLF + "        ON R.STARTSABUN = U.IDNUMBER  ";
                SQL = SQL + ComNum.VBLF + "    WHERE 1 = 1  ";
                SQL = SQL + ComNum.VBLF + "         AND R.BDATE >= TO_DATE('" + strSdate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "         AND R.BDATE <= TO_DATE('" + strEdate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "        AND R.GBIO = 'E'  ";
                SQL = SQL + ComNum.VBLF + "        AND R.DROPDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "        AND R.CANCERDATE IS NULL ";
                if (Spd == ssAll_Stroke_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "         AND R.CPCODE = 'CPCODE0001' ";
                }
                else if (Spd == ssAll_STMI_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "         AND R.CPCODE = 'CPCODE0002' ";
                }
                else if (Spd == ssAll_Seizure_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "         AND R.CPCODE = 'CPCODE0003' ";
                }
                else if (Spd == ssAll_UGI_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "         AND R.CPCODE = 'CPCODE0004' ";
                }
                else if (Spd == ssAll_Double_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "         AND R.CPCODE = 'CPCODE0005' ";
                }
                else if (Spd == ssAll_Broken_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "         AND R.CPCODE = 'CPCODE0006' ";
                }
                SQL = SQL + ComNum.VBLF + "    GROUP BY R.CPNO ";
                SQL = SQL + ComNum.VBLF + ")X ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    Spd.Cells[0, 0].Text = dt.Rows[0]["CNT"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                #endregion //위반 Count

                #region //최종 Count
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    COUNT(R.CPCODE) AS CNT ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.OCS_CP_RECORD R  ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.NUR_ER_PATIENT N  ";
                SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PANO  ";
                SQL = SQL + ComNum.VBLF + "     AND R.BDATE = N.JDATE  ";
                SQL = SQL + ComNum.VBLF + "     AND R.INTIME = N.INTIME  ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.BAS_BASCD B  ";
                SQL = SQL + ComNum.VBLF + "    ON R.CPCODE = B.BASCD  ";
                SQL = SQL + ComNum.VBLF + "    AND B.GRPCDB = 'CP관리'  ";
                SQL = SQL + ComNum.VBLF + "    AND B.GRPCD = 'CP코드관리'  ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.BAS_USER U ";
                SQL = SQL + ComNum.VBLF + "    ON R.STARTSABUN = U.IDNUMBER  ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1  ";
                SQL = SQL + ComNum.VBLF + "    AND R.BDATE >= TO_DATE('" + strSdate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "    AND R.BDATE <= TO_DATE('" + strEdate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "    AND R.GBIO = 'E'  ";
                SQL = SQL + ComNum.VBLF + "    AND R.DROPDATE IS NULL  ";
                SQL = SQL + ComNum.VBLF + "    AND R.CANCERDATE IS NULL ";
                if (Spd == ssAll_Stroke_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND R.CPCODE = 'CPCODE0001' ";
                }
                else if (Spd == ssAll_STMI_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND R.CPCODE = 'CPCODE0002' ";
                }
                else if (Spd == ssAll_Seizure_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND R.CPCODE = 'CPCODE0003' ";
                }
                else if (Spd == ssAll_UGI_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND R.CPCODE = 'CPCODE0004' ";
                }
                else if (Spd == ssAll_Double_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND R.CPCODE = 'CPCODE0005' ";
                }
                else if (Spd == ssAll_Broken_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND R.CPCODE = 'CPCODE0006' ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    Spd.Cells[0, 1].Text = dt.Rows[0]["CNT"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                #endregion //최종 Count

                #region //Activation Count
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    COUNT(R.CPCODE) AS CNT ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.OCS_CP_RECORD R  ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.NUR_ER_PATIENT N  ";
                SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PANO  ";
                SQL = SQL + ComNum.VBLF + "     AND R.BDATE = N.JDATE  ";
                SQL = SQL + ComNum.VBLF + "     AND R.INTIME = N.INTIME  ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.BAS_BASCD B  ";
                SQL = SQL + ComNum.VBLF + "    ON R.CPCODE = B.BASCD  ";
                SQL = SQL + ComNum.VBLF + "    AND B.GRPCDB = 'CP관리'  ";
                SQL = SQL + ComNum.VBLF + "    AND B.GRPCD = 'CP코드관리'  ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.BAS_USER U ";
                SQL = SQL + ComNum.VBLF + "    ON R.STARTSABUN = U.IDNUMBER  ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1  ";
                SQL = SQL + ComNum.VBLF + "    AND R.BDATE >= TO_DATE('" + strSdate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "    AND R.BDATE <= TO_DATE('" + strEdate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "    AND R.GBIO = 'E'  ";
                if (Spd == ssAll_Stroke_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND R.CPCODE = 'CPCODE0001' ";
                }
                else if (Spd == ssAll_STMI_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND R.CPCODE = 'CPCODE0002' ";
                }
                else if (Spd == ssAll_Seizure_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND R.CPCODE = 'CPCODE0003' ";
                }
                else if (Spd == ssAll_UGI_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND R.CPCODE = 'CPCODE0004' ";
                }
                else if (Spd == ssAll_Double_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND R.CPCODE = 'CPCODE0005' ";
                }
                else if (Spd == ssAll_Broken_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND R.CPCODE = 'CPCODE0006' ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    Spd.Cells[0, 2].Text = dt.Rows[0]["CNT"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                Spd.Cells[0, 3].Text = ((VB.Val(Spd.Cells[0, 0].Text) / VB.Val(Spd.Cells[0, 1].Text)) * 100).ToString("##.##");
                Spd.Cells[0, 4].Text = ((VB.Val(Spd.Cells[0, 1].Text) / VB.Val(Spd.Cells[0, 2].Text)) * 100).ToString("##.##");

                #endregion //Activation Count
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void GetDataViolAll()
        {
            GetDataViolAllQuery(ssViol_All_Sheet1);
            GetDataViolSubQuery(ssViol_Stroke_Sheet1);
            GetDataViolSubQuery(ssViol_STMI_Sheet1);
            GetDataViolSubQuery(ssViol_Seizure_Sheet1);
            GetDataViolSubQuery(ssViol_UGI_Sheet1);
            GetDataViolSubQuery(ssViol_Double_Sheet1);
            GetDataViolSubQuery(ssViol_Broken_Sheet1);
        }

        private void GetDataViolAllQuery(FarPoint.Win.Spread.SheetView Spd)
        {
            Spd.RowCount = 0;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                string strSdate = dtpSDate.Value.ToString("yyyy-MM-dd");
                string strEdate = dtpEDate.Value.ToString("yyyy-MM-dd");

                #region //최종 Count
                SQL = " SELECT   ";
                SQL = SQL + ComNum.VBLF + "    R.CPCODE, B.BASNAME AS CPNAME, COUNT(R.CPCODE) AS CNT ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.OCS_CP_RECORD R   ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.NUR_ER_PATIENT N   ";
                SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PANO   ";
                SQL = SQL + ComNum.VBLF + "     AND R.BDATE = N.JDATE   ";
                SQL = SQL + ComNum.VBLF + "     AND R.INTIME = N.INTIME   ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.BAS_BASCD B   ";
                SQL = SQL + ComNum.VBLF + "    ON R.CPCODE = B.BASCD   ";
                SQL = SQL + ComNum.VBLF + "    AND B.GRPCDB = 'CP관리'   ";
                SQL = SQL + ComNum.VBLF + "    AND B.GRPCD = 'CP코드관리'   ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.BAS_USER U  ";
                SQL = SQL + ComNum.VBLF + "    ON R.STARTSABUN = U.IDNUMBER   ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1   ";
                SQL = SQL + ComNum.VBLF + "    AND R.BDATE >= TO_DATE('" + strSdate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "    AND R.BDATE <= TO_DATE('" + strEdate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "    AND R.GBIO = 'E'   ";
                SQL = SQL + ComNum.VBLF + "    AND R.DROPDATE IS NULL   ";
                SQL = SQL + ComNum.VBLF + "    AND R.CANCERDATE IS NULL  ";
                SQL = SQL + ComNum.VBLF + "GROUP BY R.CPCODE, B.BASNAME  ";
                SQL = SQL + ComNum.VBLF + "ORDER BY R.CPCODE  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }
                Spd.RowCount = dt.Rows.Count;
                Spd.SetRowHeight(-1, ComNum.SPDROWHT);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.Cells[i, 0].Text = dt.Rows[i]["CPNAME"].ToString().Trim();
                    Spd.Cells[i, 2].Text = dt.Rows[i]["CNT"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                #endregion

                #region //위반 Count

                SQL = " SELECT ";
                SQL = SQL + ComNum.VBLF + "    X.CPCODE, X.CPNAME, COUNT(X.CPNO) AS CNT ";
                SQL = SQL + ComNum.VBLF + "FROM ( ";
                SQL = SQL + ComNum.VBLF + "    SELECT    ";
                SQL = SQL + ComNum.VBLF + "        R.CPCODE, B.BASNAME AS CPNAME, R.CPNO ";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.OCS_CP_RECORD R    ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.OCS_CP_VALUE V   ";
                SQL = SQL + ComNum.VBLF + "        ON R.CPNO = V.CPNO   ";
                SQL = SQL + ComNum.VBLF + "        AND V.VALGB = '0'   ";
                SQL = SQL + ComNum.VBLF + "        AND V.CPRSTN > 0   ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.OCS_CP_SUB S   ";
                SQL = SQL + ComNum.VBLF + "        ON V.CODE = S.CODE    ";
                SQL = SQL + ComNum.VBLF + "        AND S.CPCODE = R.CPCODE   ";
                SQL = SQL + ComNum.VBLF + "        AND S.GUBUN = '06'   ";
                SQL = SQL + ComNum.VBLF + "        AND S.TYPE = '시각'   ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.NUR_ER_PATIENT N    ";
                SQL = SQL + ComNum.VBLF + "         ON R.PTNO = N.PANO    ";
                SQL = SQL + ComNum.VBLF + "         AND R.BDATE = N.JDATE    ";
                SQL = SQL + ComNum.VBLF + "         AND R.INTIME = N.INTIME    ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.BAS_BASCD B    ";
                SQL = SQL + ComNum.VBLF + "        ON R.CPCODE = B.BASCD    ";
                SQL = SQL + ComNum.VBLF + "        AND B.GRPCDB = 'CP관리'    ";
                SQL = SQL + ComNum.VBLF + "        AND B.GRPCD = 'CP코드관리'    ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.BAS_USER U   ";
                SQL = SQL + ComNum.VBLF + "        ON R.STARTSABUN = U.IDNUMBER    ";
                SQL = SQL + ComNum.VBLF + "    WHERE 1 = 1    ";
                SQL = SQL + ComNum.VBLF + "         AND R.BDATE >= TO_DATE('" + strSdate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "         AND R.BDATE <= TO_DATE('" + strEdate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "        AND R.GBIO = 'E'    ";
                SQL = SQL + ComNum.VBLF + "        AND R.DROPDATE IS NULL   ";
                SQL = SQL + ComNum.VBLF + "        AND R.CANCERDATE IS NULL   ";
                SQL = SQL + ComNum.VBLF + "    GROUP BY R.CPCODE, B.BASNAME , R.CPNO ";
                SQL = SQL + ComNum.VBLF + "    ) X ";
                SQL = SQL + ComNum.VBLF + "GROUP BY X.CPCODE, X.CPNAME ";
                SQL = SQL + ComNum.VBLF + "ORDER BY X.CPCODE   ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < Spd.RowCount; j++)
                    {
                        if (Spd.Cells[j, 0].Text.Trim() == dt.Rows[i]["CPNAME"].ToString().Trim())
                        {
                            Spd.Cells[j, 1].Text = dt.Rows[i]["CNT"].ToString().Trim();
                            Spd.Cells[j, 3].Text = ((VB.Val(Spd.Cells[j, 1].Text) / VB.Val(dt.Rows[i]["CNT"].ToString().Trim())) * 100).ToString("##.##");
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                #endregion //위반 Count

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void GetDataViolSubQuery(FarPoint.Win.Spread.SheetView Spd)
        {
            Spd.RowCount = 0;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                string strSdate = dtpSDate.Value.ToString("yyyy-MM-dd");
                string strEdate = dtpEDate.Value.ToString("yyyy-MM-dd");

                SQL = " SELECT ";
                SQL = SQL + ComNum.VBLF + "    CODE, BASNAME, DSPSEQ,  ";
                SQL = SQL + ComNum.VBLF + "    SUM(VIOL) AS VIOL, SUM(GOOD) AS GOOD, SUM(VIOL) + SUM(GOOD) AS TOTCNT "     ;
                SQL = SQL + ComNum.VBLF + "FROM ( "                                                                        ;
                SQL = SQL + ComNum.VBLF + "    SELECT   "                                                                  ;
                SQL = SQL + ComNum.VBLF + "        S.CODE, BS.BASNAME, S.DSPSEQ,  "                                        ;
                SQL = SQL + ComNum.VBLF + "        CASE  "                                                                 ;
                SQL = SQL + ComNum.VBLF + "            WHEN V.CPRSTN > 0 THEN 1 "                                          ;
                SQL = SQL + ComNum.VBLF + "            ELSE 0 "                                                            ;
                SQL = SQL + ComNum.VBLF + "        END AS VIOL , "                                                         ;
                SQL = SQL + ComNum.VBLF + "        CASE  "                                                                 ;
                SQL = SQL + ComNum.VBLF + "            WHEN V.CPRSTN <= 0 THEN 1 "                                         ;
                SQL = SQL + ComNum.VBLF + "            ELSE 0 "                                                            ;
                SQL = SQL + ComNum.VBLF + "        END AS GOOD   "                                                         ;
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.OCS_CP_RECORD R   "                                         ;
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.OCS_CP_VALUE V  "                                     ;
                SQL = SQL + ComNum.VBLF + "        ON R.CPNO = V.CPNO  "                                                   ;
                SQL = SQL + ComNum.VBLF + "        AND V.VALGB = '0'  "                                                    ;
                SQL = SQL + ComNum.VBLF + "        --AND V.CPRSTN > 0  "                                                   ;
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.OCS_CP_SUB S  "                                       ;
                SQL = SQL + ComNum.VBLF + "        ON V.CODE = S.CODE   "                                                  ;
                SQL = SQL + ComNum.VBLF + "        AND S.CPCODE = R.CPCODE  "                                              ;
                SQL = SQL + ComNum.VBLF + "        AND S.GUBUN = '06'  "                                                   ;
                SQL = SQL + ComNum.VBLF + "        AND S.TYPE = '시각'  "                                                  ;
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.NUR_ER_PATIENT N   "                                 ;
                SQL = SQL + ComNum.VBLF + "         ON R.PTNO = N.PANO   "                                                 ;
                SQL = SQL + ComNum.VBLF + "         AND R.BDATE = N.JDATE   "                                              ;
                SQL = SQL + ComNum.VBLF + "         AND R.INTIME = N.INTIME   "                                            ;
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.BAS_BASCD B   "                                      ;
                SQL = SQL + ComNum.VBLF + "        ON R.CPCODE = B.BASCD   "                                               ;
                SQL = SQL + ComNum.VBLF + "        AND B.GRPCDB = 'CP관리'   "                                             ;
                SQL = SQL + ComNum.VBLF + "        AND B.GRPCD = 'CP코드관리' "                                            ;
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.BAS_BASCD BS   "                                     ;
                SQL = SQL + ComNum.VBLF + "        ON S.CODE = BS.BASCD   "                                                ;
                SQL = SQL + ComNum.VBLF + "        AND BS.GRPCDB = 'CP관리'   "                                            ;
                SQL = SQL + ComNum.VBLF + "        AND BS.GRPCD = 'CP지표'     "                                           ;
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.BAS_USER U  "                                        ;
                SQL = SQL + ComNum.VBLF + "        ON R.STARTSABUN = U.IDNUMBER   "                                        ;
                SQL = SQL + ComNum.VBLF + "    WHERE 1 = 1   "                                                             ;
                SQL = SQL + ComNum.VBLF + "         AND R.BDATE >= TO_DATE('" + strSdate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "         AND R.BDATE <= TO_DATE('" + strEdate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "        AND R.GBIO = 'E'   "                                                    ;
                SQL = SQL + ComNum.VBLF + "        AND R.DROPDATE IS NULL  "                                               ;
                SQL = SQL + ComNum.VBLF + "        AND R.CANCERDATE IS NULL "                                              ;
                if (Spd == ssViol_Stroke_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "         AND R.CPCODE = 'CPCODE0001' ";
                }
                else if (Spd == ssViol_STMI_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "         AND R.CPCODE = 'CPCODE0002' ";
                }
                else if (Spd == ssViol_Seizure_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "         AND R.CPCODE = 'CPCODE0003' ";
                }
                else if (Spd == ssViol_UGI_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "         AND R.CPCODE = 'CPCODE0004' ";
                }
                else if (Spd == ssViol_Double_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "         AND R.CPCODE = 'CPCODE0005' ";
                }
                else if (Spd == ssViol_Broken_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "         AND R.CPCODE = 'CPCODE0006' ";
                }
                SQL = SQL + ComNum.VBLF + "    ) "                                                                         ;
                SQL = SQL + ComNum.VBLF + "GROUP BY CODE, BASNAME, DSPSEQ "                                                ;
                SQL = SQL + ComNum.VBLF + "ORDER BY DSPSEQ ";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }
                Spd.RowCount = dt.Rows.Count;
                Spd.SetRowHeight(-1, ComNum.SPDROWHT);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.Cells[i, 0].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                    Spd.Cells[i, 1].Text = dt.Rows[i]["VIOL"].ToString().Trim();
                    Spd.Cells[i, 2].Text = dt.Rows[i]["TOTCNT"].ToString().Trim();
                    Spd.Cells[i, 3].Text = ((VB.Val(dt.Rows[i]["VIOL"].ToString().Trim()) / VB.Val(dt.Rows[i]["TOTCNT"].ToString().Trim())) * 100).ToString("##.##");
                }
                dt.Dispose();
                dt = null;
                

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void GetDataAvrAll()
        {
            GetDataAvrSubQuery(ssAvr_Stroke_Sheet1);
            GetDataAvrSubQuery(ssAvr_STMI_Sheet1);
            GetDataAvrSubQuery(ssAvr_Seizure_Sheet1);
            GetDataAvrSubQuery(ssAvr_UGI_Sheet1);
            GetDataAvrSubQuery(ssAvr_Double_Sheet1);
            GetDataAvrSubQuery(ssAvr_Broken_Sheet1);
        }

        private void GetDataAvrSubQuery(FarPoint.Win.Spread.SheetView Spd)
        {
            Spd.RowCount = 0;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                string strSdate = dtpSDate.Value.ToString("yyyy-MM-dd");
                string strEdate = dtpEDate.Value.ToString("yyyy-MM-dd");

                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    CODE, BASNAME, DSPSEQ, CPVALUE, COUNT(CODE) AS TOTCNT, "           ;
                SQL = SQL + ComNum.VBLF + "    SUM(CPVALN) AS CPVALN, SUM(CPRSTN) AS CPRSTN ";
                SQL = SQL + ComNum.VBLF + "FROM ( "                                                            ;
                SQL = SQL + ComNum.VBLF + "    SELECT   "                                                      ;
                SQL = SQL + ComNum.VBLF + "        S.CODE, BS.BASNAME, S.DSPSEQ,    "                          ;
                SQL = SQL + ComNum.VBLF + "        S.CPVALUE, V.CPVALN, V.CPRSTN       ";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.OCS_CP_RECORD R    "                            ;
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.OCS_CP_VALUE V   "                        ;
                SQL = SQL + ComNum.VBLF + "        ON R.CPNO = V.CPNO   "                                      ;
                SQL = SQL + ComNum.VBLF + "        AND V.VALGB = '0'   "                                       ;
                SQL = SQL + ComNum.VBLF + "        --AND V.CPRSTN > 0   "                                      ;
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.OCS_CP_SUB S   "                          ;
                SQL = SQL + ComNum.VBLF + "        ON V.CODE = S.CODE    "                                     ;
                SQL = SQL + ComNum.VBLF + "        AND S.CPCODE = R.CPCODE   "                                 ;
                SQL = SQL + ComNum.VBLF + "        AND S.GUBUN = '06'   "                                      ;
                SQL = SQL + ComNum.VBLF + "        AND S.TYPE = '시각'   "                                     ;
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.NUR_ER_PATIENT N    "                    ;
                SQL = SQL + ComNum.VBLF + "         ON R.PTNO = N.PANO    "                                    ;
                SQL = SQL + ComNum.VBLF + "         AND R.BDATE = N.JDATE    "                                 ;
                SQL = SQL + ComNum.VBLF + "         AND R.INTIME = N.INTIME    "                               ;
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.BAS_BASCD B    "                         ;
                SQL = SQL + ComNum.VBLF + "        ON R.CPCODE = B.BASCD    "                                  ;
                SQL = SQL + ComNum.VBLF + "        AND B.GRPCDB = 'CP관리'    "                                ;
                SQL = SQL + ComNum.VBLF + "        AND B.GRPCD = 'CP코드관리'  "                               ;
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.BAS_BASCD BS    "                        ;
                SQL = SQL + ComNum.VBLF + "        ON S.CODE = BS.BASCD    "                                   ;
                SQL = SQL + ComNum.VBLF + "        AND BS.GRPCDB = 'CP관리'    "                               ;
                SQL = SQL + ComNum.VBLF + "        AND BS.GRPCD = 'CP지표'      "                              ;
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.BAS_USER U   "                           ;
                SQL = SQL + ComNum.VBLF + "        ON R.STARTSABUN = U.IDNUMBER    "                           ;
                SQL = SQL + ComNum.VBLF + "    WHERE 1 = 1    "                                                ;
                SQL = SQL + ComNum.VBLF + "         AND R.BDATE >= TO_DATE('" + strSdate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "         AND R.BDATE <= TO_DATE('" + strEdate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "        AND R.GBIO = 'E'    "                                       ;
                SQL = SQL + ComNum.VBLF + "        AND R.DROPDATE IS NULL   "                                  ;
                SQL = SQL + ComNum.VBLF + "        AND R.CANCERDATE IS NULL  "                                 ;
                if (Spd == ssAvr_Stroke_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "         AND R.CPCODE = 'CPCODE0001' ";
                }
                else if (Spd == ssAvr_STMI_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "         AND R.CPCODE = 'CPCODE0002' ";
                }
                else if (Spd == ssAvr_Seizure_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "         AND R.CPCODE = 'CPCODE0003' ";
                }
                else if (Spd == ssAvr_UGI_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "         AND R.CPCODE = 'CPCODE0004' ";
                }
                else if (Spd == ssAvr_Double_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "         AND R.CPCODE = 'CPCODE0005' ";
                }
                else if (Spd == ssAvr_Broken_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "         AND R.CPCODE = 'CPCODE0006' ";
                }
                SQL = SQL + ComNum.VBLF + "    )  "                                                            ;
                SQL = SQL + ComNum.VBLF + "GROUP BY CODE, BASNAME, DSPSEQ, CPVALUE  "                          ;
                SQL = SQL + ComNum.VBLF + "ORDER BY DSPSEQ  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }
                Spd.RowCount = dt.Rows.Count;
                Spd.SetRowHeight(-1, ComNum.SPDROWHT);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.Cells[i, 0].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                    Spd.Cells[i, 1].Text = dt.Rows[i]["CPVALUE"].ToString().Trim();
                    Spd.Cells[i, 2].Text = ((int)(VB.Val(dt.Rows[i]["CPVALN"].ToString().Trim()) / VB.Val(dt.Rows[i]["TOTCNT"].ToString().Trim()))).ToString();
                    if (dt.Rows[i]["TOTCNT"].ToString().Trim() != "0")
                    {
                        if(((int)(VB.Val(dt.Rows[i]["CPVALN"].ToString().Trim()) / VB.Val(dt.Rows[i]["TOTCNT"].ToString().Trim()))) > (int)VB.Val(dt.Rows[i]["CPVALUE"].ToString().Trim()))
                        {
                            Spd.Cells[i, 3].Text = "+" +  (((int)(VB.Val(dt.Rows[i]["CPVALN"].ToString().Trim()) / VB.Val(dt.Rows[i]["TOTCNT"].ToString().Trim()))) - (int)VB.Val(dt.Rows[i]["CPVALUE"].ToString().Trim())).ToString(); 
                        }
                        else
                        {
                            Spd.Cells[i, 3].Text = (((int)(VB.Val(dt.Rows[i]["CPVALN"].ToString().Trim()) / VB.Val(dt.Rows[i]["TOTCNT"].ToString().Trim()))) - (int)VB.Val(dt.Rows[i]["CPVALUE"].ToString().Trim())).ToString();
                        }

                        if (Spd.Cells[i, 3].Text.IndexOf("+") >= 0 )
                        {
                            Spd.Cells[i, 3].ForeColor = Color.Red;
                        }
                    }
                }
                dt.Dispose();
                dt = null;


            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void GetDataResultAll()
        {
            GetDataResultAllQuery(ssResult_All_Sheet1);
            GetDataResultAllQuery(ssResult_Stroke_Sheet1);
            GetDataResultAllQuery(ssResult_STMI_Sheet1);
            GetDataResultAllQuery(ssResult_Seizure_Sheet1);
            GetDataResultAllQuery(ssResult_UGI_Sheet1);
            GetDataResultAllQuery(ssResult_Double_Sheet1);
            GetDataResultAllQuery(ssResult_Broken_Sheet1);
        }

        private void GetDataResultAllQuery(FarPoint.Win.Spread.SheetView Spd)
        {
            Spd.Cells[0, 0].Text = "";
            Spd.Cells[0, 1].Text = "";
            Spd.Cells[0, 2].Text = "";
            Spd.Cells[0, 3].Text = "";
            Spd.Cells[0, 4].Text = "";
            Spd.Cells[0, 5].Text = "";
            Spd.Cells[0, 6].Text = "";
            Spd.Cells[0, 7].Text = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                string strSdate = dtpSDate.Value.ToString("yyyy-MM-dd");
                string strEdate = dtpEDate.Value.ToString("yyyy-MM-dd");

                #region //사례수
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    SUM(GHOME) AS GHOME, ";
                SQL = SQL + ComNum.VBLF + "    SUM(TRS) AS TRS, ";
                SQL = SQL + ComNum.VBLF + "    SUM(INCNT) AS INCNT, ";
                SQL = SQL + ComNum.VBLF + "    SUM(DOA) AS DOA, ";
                SQL = SQL + ComNum.VBLF + "    COUNT(*) AS ALLCNT ";

                SQL = SQL + ComNum.VBLF + " FROM (  ";
                SQL = SQL + ComNum.VBLF + "    SELECT   ";
                SQL = SQL + ComNum.VBLF + "        CASE   ";
                SQL = SQL + ComNum.VBLF + "            WHEN ADMIN.FC_GET_CP_RESULT(R.PTNO, TO_CHAR(R.INTIME, 'YYYYMMDD'), TO_CHAR(R.INTIME, 'HH24MI')) = '귀가' THEN 1  ";
                SQL = SQL + ComNum.VBLF + "            ELSE 0  ";
                SQL = SQL + ComNum.VBLF + "        END GHOME,  ";
                SQL = SQL + ComNum.VBLF + "        CASE   ";
                SQL = SQL + ComNum.VBLF + "            WHEN ADMIN.FC_GET_CP_RESULT(R.PTNO, TO_CHAR(R.INTIME, 'YYYYMMDD'), TO_CHAR(R.INTIME, 'HH24MI')) = '전원' THEN 1 ";
                SQL = SQL + ComNum.VBLF + "            ELSE 0  ";
                SQL = SQL + ComNum.VBLF + "        END TRS,   ";
                SQL = SQL + ComNum.VBLF + "        CASE   ";
                SQL = SQL + ComNum.VBLF + "            WHEN ADMIN.FC_GET_CP_RESULT(R.PTNO, TO_CHAR(R.INTIME, 'YYYYMMDD'), TO_CHAR(R.INTIME, 'HH24MI')) = '입원' THEN 1 ";
                SQL = SQL + ComNum.VBLF + "            ELSE 0  ";
                SQL = SQL + ComNum.VBLF + "        END INCNT,  ";
                SQL = SQL + ComNum.VBLF + "        CASE   ";
                SQL = SQL + ComNum.VBLF + "            WHEN ADMIN.FC_GET_CP_RESULT(R.PTNO, TO_CHAR(R.INTIME, 'YYYYMMDD'), TO_CHAR(R.INTIME, 'HH24MI')) = '사망' THEN 1 ";
                SQL = SQL + ComNum.VBLF + "            ELSE 0  ";
                SQL = SQL + ComNum.VBLF + "        END DOA , ";
                SQL = SQL + ComNum.VBLF + "        CASE   ";
                SQL = SQL + ComNum.VBLF + "            WHEN ADMIN.FC_GET_CP_RESULT(R.PTNO, TO_CHAR(R.INTIME, 'YYYYMMDD'), TO_CHAR(R.INTIME, 'HH24MI')) = '귀가' THEN 0 ";
                SQL = SQL + ComNum.VBLF + "            WHEN ADMIN.FC_GET_CP_RESULT(R.PTNO, TO_CHAR(R.INTIME, 'YYYYMMDD'), TO_CHAR(R.INTIME, 'HH24MI')) = '전원' THEN 0 ";
                SQL = SQL + ComNum.VBLF + "            WHEN ADMIN.FC_GET_CP_RESULT(R.PTNO, TO_CHAR(R.INTIME, 'YYYYMMDD'), TO_CHAR(R.INTIME, 'HH24MI')) = '입원' THEN 0 ";
                SQL = SQL + ComNum.VBLF + "            WHEN ADMIN.FC_GET_CP_RESULT(R.PTNO, TO_CHAR(R.INTIME, 'YYYYMMDD'), TO_CHAR(R.INTIME, 'HH24MI')) = '사망' THEN 0 ";
                SQL = SQL + ComNum.VBLF + "            ELSE 1 ";
                SQL = SQL + ComNum.VBLF + "        END ETC  ";

                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.OCS_CP_RECORD R   ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.NUR_ER_PATIENT N   ";
                SQL = SQL + ComNum.VBLF + "         ON R.PTNO = N.PANO   ";
                SQL = SQL + ComNum.VBLF + "         AND R.BDATE = N.JDATE   ";
                SQL = SQL + ComNum.VBLF + "         AND R.INTIME = N.INTIME   ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.BAS_BASCD B   ";
                SQL = SQL + ComNum.VBLF + "        ON R.CPCODE = B.BASCD   ";
                SQL = SQL + ComNum.VBLF + "        AND B.GRPCDB = 'CP관리'   ";
                SQL = SQL + ComNum.VBLF + "        AND B.GRPCD = 'CP코드관리'   ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.BAS_USER U  ";
                SQL = SQL + ComNum.VBLF + "        ON R.STARTSABUN = U.IDNUMBER   ";
                SQL = SQL + ComNum.VBLF + "    WHERE 1 = 1   ";
                SQL = SQL + ComNum.VBLF + "        AND R.BDATE >= TO_DATE('" + strSdate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "        AND R.BDATE <= TO_DATE('" + strEdate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "        AND R.GBIO = 'E'   ";
                SQL = SQL + ComNum.VBLF + "        AND R.DROPDATE IS NULL  ";
                SQL = SQL + ComNum.VBLF + "        AND R.CANCERDATE IS NULL ";
                if (Spd == ssResult_Stroke_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "             AND R.CPCODE = 'CPCODE0001' ";
                }
                else if (Spd == ssResult_STMI_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "             AND R.CPCODE = 'CPCODE0002' ";
                }
                else if (Spd == ssResult_Seizure_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "             AND R.CPCODE = 'CPCODE0003' ";
                }
                else if (Spd == ssResult_UGI_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "             AND R.CPCODE = 'CPCODE0004' ";
                }
                else if (Spd == ssResult_Double_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "             AND R.CPCODE = 'CPCODE0005' ";
                }
                else if (Spd == ssResult_Broken_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "             AND R.CPCODE = 'CPCODE0006' ";
                }
                SQL = SQL + ComNum.VBLF + "    ) ";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                Spd.Cells[0, 0].Text = dt.Rows[0]["GHOME"].ToString().Trim();
                Spd.Cells[0, 1].Text = dt.Rows[0]["TRS"].ToString().Trim();
                Spd.Cells[0, 2].Text = dt.Rows[0]["INCNT"].ToString().Trim();
                Spd.Cells[0, 3].Text = dt.Rows[0]["DOA"].ToString().Trim();

                int intAllCnt = 0;
                if (Spd == ssResult_All_Sheet1)
                {
                    intAllCnt = (int)VB.Val(ssAll_All_Sheet1.Cells[0, 1].Text);
                }
                else if (Spd == ssResult_Stroke_Sheet1)
                {
                    intAllCnt = (int)VB.Val(ssAll_Stroke_Sheet1.Cells[0, 1].Text);
                }
                else if (Spd == ssResult_STMI_Sheet1)
                {
                    intAllCnt = (int)VB.Val(ssAll_STMI_Sheet1.Cells[0, 1].Text);
                }
                else if (Spd == ssResult_Seizure_Sheet1)
                {
                    intAllCnt = (int)VB.Val(ssAll_Seizure_Sheet1.Cells[0, 1].Text);
                }
                else if (Spd == ssResult_UGI_Sheet1)
                {
                    intAllCnt = (int)VB.Val(ssAll_UGI_Sheet1.Cells[0, 1].Text);
                }
                else if (Spd == ssResult_Double_Sheet1)
                {
                    intAllCnt = (int)VB.Val(ssAll_Double_Sheet1.Cells[0, 1].Text);
                }
                else if (Spd == ssResult_Broken_Sheet1)
                {
                    intAllCnt = (int)VB.Val(ssAll_Broken_Sheet1.Cells[0, 1].Text);
                }

                Spd.Cells[0, 4].Text = ((VB.Val(dt.Rows[0]["GHOME"].ToString().Trim()) / intAllCnt) * 100).ToString("##.##");
                Spd.Cells[0, 5].Text = ((VB.Val(dt.Rows[0]["TRS"].ToString().Trim()) / intAllCnt) * 100).ToString("##.##");
                Spd.Cells[0, 6].Text = ((VB.Val(dt.Rows[0]["INCNT"].ToString().Trim()) / intAllCnt) * 100).ToString("##.##");
                Spd.Cells[0, 7].Text = ((VB.Val(dt.Rows[0]["DOA"].ToString().Trim()) / intAllCnt) * 100).ToString("##.##");

                dt.Dispose();
                dt = null;

                #endregion //사례수

                #region //사례비율
                
                #endregion //최종 Count

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void GetDatListAll()
        {
            GetDatListAllQuery(ssList_All_Sheet1);

            CpList_Head(ssList_Stroke_Sheet1);
            CpList_Head(ssList_STMI_Sheet1);
            CpList_Head(ssList_Seizure_Sheet1);
            CpList_Head(ssList_UGI_Sheet1);
            CpList_Head(ssList_Double_Sheet1);
            CpList_Head(ssList_Broken_Sheet1);
            GetDatListAllQuery(ssList_Stroke_Sheet1);
            GetDatListAllQuery(ssList_STMI_Sheet1);
            GetDatListAllQuery(ssList_Seizure_Sheet1);
            GetDatListAllQuery(ssList_UGI_Sheet1);
            GetDatListAllQuery(ssList_Double_Sheet1);
            GetDatListAllQuery(ssList_Broken_Sheet1);
        }

        private void GetDatListAllQuery(FarPoint.Win.Spread.SheetView Spd)
        {
            Spd.Rows.Count = 0;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                string strSdate = dtpSDate.Value.ToString("yyyy-MM-dd");
                string strEdate = dtpEDate.Value.ToString("yyyy-MM-dd");

                #region //List
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(R.BDATE, 'YYYY-MM-DD')  AS BDATE,";
                SQL = SQL + ComNum.VBLF + "    R.CPNO, R.CPCODE, R.PTNO, R.CPCODE, N.OUTGBN, ";
                SQL = SQL + ComNum.VBLF + "    B.BASNAME AS CPNAME,";
                SQL = SQL + ComNum.VBLF + "    P.SNAME,";
                SQL = SQL + ComNum.VBLF + "    R.AGE, R.SEX, ";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(R.INTIME, 'YYYY-MM-DD HH24:MI') AS INTIME  ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.OCS_CP_RECORD R  ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.BAS_PATIENT P  ";
                SQL = SQL + ComNum.VBLF + "     ON R.PTNO = P.PANO  ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.NUR_ER_PATIENT N  ";
                SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PANO  ";
                SQL = SQL + ComNum.VBLF + "     AND R.BDATE = N.JDATE  ";
                SQL = SQL + ComNum.VBLF + "     AND R.INTIME = N.INTIME  ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.BAS_BASCD B  ";
                SQL = SQL + ComNum.VBLF + "    ON R.CPCODE = B.BASCD  ";
                SQL = SQL + ComNum.VBLF + "    AND B.GRPCDB = 'CP관리'  ";
                SQL = SQL + ComNum.VBLF + "    AND B.GRPCD = 'CP코드관리'  ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.BAS_USER U ";
                SQL = SQL + ComNum.VBLF + "    ON R.STARTSABUN = U.IDNUMBER  ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1  ";
                SQL = SQL + ComNum.VBLF + "    AND R.BDATE >= TO_DATE('" + strSdate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "    AND R.BDATE <= TO_DATE('" + strEdate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "    AND R.GBIO = 'E'  ";
                SQL = SQL + ComNum.VBLF + "    AND R.DROPDATE IS NULL  ";
                SQL = SQL + ComNum.VBLF + "    AND R.CANCERDATE IS NULL ";
                if (Spd == ssList_Stroke_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND R.CPCODE = 'CPCODE0001' ";
                }
                else if (Spd == ssList_STMI_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND R.CPCODE = 'CPCODE0002' ";
                }
                else if (Spd == ssList_Seizure_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND R.CPCODE = 'CPCODE0003' ";
                }
                else if (Spd == ssList_UGI_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND R.CPCODE = 'CPCODE0004' ";
                }
                else if (Spd == ssList_Double_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND R.CPCODE = 'CPCODE0005' ";
                }
                else if (Spd == ssList_Broken_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND R.CPCODE = 'CPCODE0006' ";
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY R.BDATE, R.CPCODE";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    Spd.RowCount = dt.Rows.Count;
                    Spd.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Spd.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        Spd.Cells[i, 1].Text = dt.Rows[i]["CPNAME"].ToString().Trim();
                        Spd.Cells[i, 2].Text = dt.Rows[i]["PTNO"].ToString().Trim(); 
                        Spd.Cells[i, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        Spd.Cells[i, 4].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        Spd.Cells[i, 5].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        Spd.Cells[i, 6].Text = CpResult(dt.Rows[i]["CPNO"].ToString().Trim(), dt.Rows[i]["CPCODE"].ToString().Trim());
                        if (Spd.Cells[i, 6].Text.Trim() == "Y")
                        {
                            Spd.Cells[i, 6].ForeColor = Color.Red;
                        }
                        //Spd.Cells[i, 7].Text = JinResult(dt.Rows[i]["OUTGBN"].ToString().Trim());
                        Spd.Cells[i, 7].Text = JinResultQuery(dt.Rows[i]["PTNO"].ToString().Trim(), VB.Left(dt.Rows[i]["INTIME"].ToString().Trim(), 10).Replace("-", ""), VB.Right(dt.Rows[i]["INTIME"].ToString().Trim(), 5).Replace(":", ""));
                        if (Spd != ssList_All_Sheet1)
                        {
                            CpList_Value(dt.Rows[i]["CPNO"].ToString().Trim(), dt.Rows[i]["CPCODE"].ToString().Trim(), Spd, i);
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                #endregion //List
                
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void CpList_Head(FarPoint.Win.Spread.SheetView Spd)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Spd.ColumnCount = mListColCnt;
            FarPoint.Win.Spread.CellType.TextCellType rtnTextCellType = new FarPoint.Win.Spread.CellType.TextCellType();

            try
            {
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    S.TYPE, ";
                SQL = SQL + ComNum.VBLF + "    C.BASNAME  ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.OCS_CP_SUB S   ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.BAS_BASCD C   ";
                SQL = SQL + ComNum.VBLF + "    ON S.CODE = C.BASCD   ";
                SQL = SQL + ComNum.VBLF + "    AND C.GRPCDB = 'CP관리'   ";
                SQL = SQL + ComNum.VBLF + "    AND C.GRPCD = 'CP지표'  ";
                SQL = SQL + ComNum.VBLF + "WHERE 1=1";
                if (Spd == ssList_Stroke_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND S.CPCODE = 'CPCODE0001' ";
                }
                else if (Spd == ssList_STMI_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND S.CPCODE = 'CPCODE0002' ";
                }
                else if (Spd == ssList_Seizure_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND S.CPCODE = 'CPCODE0003' ";
                }
                else if (Spd == ssList_UGI_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND S.CPCODE = 'CPCODE0004' ";
                }
                else if (Spd == ssList_Double_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND S.CPCODE = 'CPCODE0005' ";
                }
                else if (Spd == ssList_Broken_Sheet1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND S.CPCODE = 'CPCODE0006' ";
                }
                SQL = SQL + ComNum.VBLF + "    AND S.GUBUN = '06'   ";
                SQL = SQL + ComNum.VBLF + "ORDER BY S.DSPSEQ ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }
                
                Spd.ColumnCount = mListColCnt + dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.Columns[mListColCnt + i].CellType = rtnTextCellType;
                    Spd.Columns[mListColCnt + i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    Spd.Columns[mListColCnt + i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    Spd.Columns[mListColCnt + i].Locked = true;
                    Spd.Columns[mListColCnt + i].Width = 120;
                    Spd.ColumnHeader.Cells[0, mListColCnt + i].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
                return ;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return ;
            }
        }

        private void CpList_Value(string pCPNO, string pCPCODE, FarPoint.Win.Spread.SheetView Spd, int Row)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    V.CODE, V.CPVALV, V.CPVALN, V.CPDFTN, V.CPRSTN, ";
                SQL = SQL + ComNum.VBLF + "    S.TYPE, S.CPVALUE, ";
                SQL = SQL + ComNum.VBLF + "    C.BASNAME  ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.OCS_CP_VALUE V   ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.OCS_CP_SUB S   ";
                SQL = SQL + ComNum.VBLF + "    ON V.CODE = S.CODE    ";
                SQL = SQL + ComNum.VBLF + "    AND S.CPCODE = '" + pCPCODE + "' ";
                SQL = SQL + ComNum.VBLF + "    AND S.GUBUN = '06'   ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.BAS_BASCD C   ";
                SQL = SQL + ComNum.VBLF + "    ON S.CODE = C.BASCD   ";
                SQL = SQL + ComNum.VBLF + "    AND C.GRPCDB = 'CP관리'   ";
                SQL = SQL + ComNum.VBLF + "    AND C.GRPCD = 'CP지표'  ";
                SQL = SQL + ComNum.VBLF + "WHERE V.CPNO = " + pCPNO;
                SQL = SQL + ComNum.VBLF + "ORDER BY S.DSPSEQ ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["TYPE"].ToString().Trim() == "구분")
                    {
                        Spd.Cells[Row, mListColCnt + i].Text = dt.Rows[i]["CPVALV"].ToString().Trim();
                    }
                    else
                    {
                        if (VB.Val(dt.Rows[i]["CPVALN"].ToString().Trim()) > VB.Val(dt.Rows[i]["CPDFTN"].ToString().Trim()))
                        {
                            Spd.Cells[Row, mListColCnt + i].ForeColor = Color.Red;
                        }
                        Spd.Cells[Row, mListColCnt + i].Text = dt.Rows[i]["CPVALN"].ToString().Trim() + " (" + dt.Rows[i]["CPDFTN"].ToString().Trim() + ")";

                        //Spd.Cells[Row, mListColCnt + i].Text = dt.Rows[i]["CPRSTN"].ToString().Trim();
                        //if (VB.Val(dt.Rows[i]["CPRSTN"].ToString().Trim()) > 0)
                        //{
                        //    Spd.Cells[Row, mListColCnt + i].ForeColor = Color.Red;
                        //}
                    }
                }
                dt.Dispose();
                dt = null;
                return;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        private string JinResult(string pFlag)
        {
            //1.입원 2.귀가 3.DOA 4.사망 5.취소 6.후송 7.DAMA 8.OPD(ER후외래로)
            string rtnVal = "";

            switch (pFlag)
            {
                case "1":
                    rtnVal = "입원";
                    break;
                case "2":
                    rtnVal = "귀가";
                    break;
                case "3":
                    rtnVal = "DOA";
                    break;
                case "4":
                    rtnVal = "사망";
                    break;
                case "5":
                    rtnVal = "취소";
                    break;
                case "6":
                    rtnVal = "후송";
                    break;
                case "7":
                    rtnVal = "DAMA";
                    break;
                case "8":
                    rtnVal = "OPD";
                    break;
            }

            return rtnVal;
        }

        private string CpResult(string pCPNO, string pCPCODE)
        {
            string rtnVal = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    V.CPNO  ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.OCS_CP_VALUE V   ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.OCS_CP_SUB S   ";
                SQL = SQL + ComNum.VBLF + "    ON V.CODE = S.CODE    ";
                SQL = SQL + ComNum.VBLF + "    AND S.CPCODE = '" + pCPCODE + "' ";
                SQL = SQL + ComNum.VBLF + "    AND S.GUBUN = '06'   ";
                SQL = SQL + ComNum.VBLF + "    AND S.TYPE = '시각'   ";
                SQL = SQL + ComNum.VBLF + "WHERE V.CPNO = " + pCPNO;
                SQL = SQL + ComNum.VBLF + "    AND V.VALGB = '0'   ";
                SQL = SQL + ComNum.VBLF + "    AND V.CPRSTN > 0 ";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnVal;
                }
                rtnVal = "Y";
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        private void GetDataEtcAll()
        {
            GetDataEtcQueryStroke();
            GetDataEtcQueryDouble();
            GetDataAllQueryBroken();
        }

        private void GetDataEtcQueryStroke()
        {
            ssStrokeTreat_Sheet1.Cells[0, 1].Text = "";
            ssStrokeTreat_Sheet1.Cells[0, 2].Text = "";
            ssStrokeTreat_Sheet1.Cells[1, 1].Text = "";
            ssStrokeTreat_Sheet1.Cells[1, 2].Text = "";

            int intTot = (int)VB.Val(ssAll_Stroke_Sheet1.Cells[0, 1].Text.Trim());
            
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                string strSdate = dtpSDate.Value.ToString("yyyy-MM-dd");
                string strEdate = dtpEDate.Value.ToString("yyyy-MM-dd");

                SQL = " SELECT    "                                                          ;
                SQL = SQL + ComNum.VBLF + "    COUNT(S.CODE) CNT        "                                  ;
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.OCS_CP_RECORD R     "                           ;
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.OCS_CP_VALUE V    "                       ;
                SQL = SQL + ComNum.VBLF + "    ON R.CPNO = V.CPNO    "                                     ;
                SQL = SQL + ComNum.VBLF + "    AND V.VALGB = '0'  "                                        ;
                SQL = SQL + ComNum.VBLF + "    AND V.CODE = 'CPPLN0005' "                                  ;
                SQL = SQL + ComNum.VBLF + "    AND V.CPETIME IS NOT NULL   "                               ;
                SQL = SQL + ComNum.VBLF + "    --AND V.CPRSTN > 0    "                                     ;
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.OCS_CP_SUB S    "                         ;
                SQL = SQL + ComNum.VBLF + "    ON V.CODE = S.CODE     "                                    ;
                SQL = SQL + ComNum.VBLF + "    AND S.CPCODE = R.CPCODE    "                                ;
                SQL = SQL + ComNum.VBLF + "    AND S.GUBUN = '06'    "                                     ;
                SQL = SQL + ComNum.VBLF + "    AND S.TYPE = '시각'    "                                    ;
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1     "                                               ;
                SQL = SQL + ComNum.VBLF + "     AND R.BDATE >= TO_DATE('" + strSdate + "','YYYY-MM-DD')   "      ;
                SQL = SQL + ComNum.VBLF + "     AND R.BDATE <= TO_DATE('" + strEdate + "','YYYY-MM-DD')   "      ;
                SQL = SQL + ComNum.VBLF + "     AND R.GBIO = 'E'     "                                     ;
                SQL = SQL + ComNum.VBLF + "    AND R.DROPDATE IS NULL    "                                 ;
                SQL = SQL + ComNum.VBLF + "    AND R.CANCERDATE IS NULL   "                                ;
                SQL = SQL + ComNum.VBLF + "     AND R.CPCODE = 'CPCODE0001' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssStrokeTreat_Sheet1.Cells[0, 1].Text = dt.Rows[0]["CNT"].ToString().Trim();
                    ssStrokeTreat_Sheet1.Cells[0, 2].Text = (((int)VB.Val(dt.Rows[0]["CNT"].ToString().Trim()) / intTot) * 100).ToString("##.##");
                }

                dt.Dispose();
                dt = null;

                SQL = " SELECT    ";
                SQL = SQL + ComNum.VBLF + "    COUNT(S.CODE) CNT        ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.OCS_CP_RECORD R     ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.OCS_CP_VALUE V    ";
                SQL = SQL + ComNum.VBLF + "    ON R.CPNO = V.CPNO    ";
                SQL = SQL + ComNum.VBLF + "    AND V.VALGB = '0'  ";
                SQL = SQL + ComNum.VBLF + "    AND V.CODE = 'CPPLN0006' ";
                SQL = SQL + ComNum.VBLF + "    AND V.CPETIME IS NOT NULL   ";
                SQL = SQL + ComNum.VBLF + "    --AND V.CPRSTN > 0    ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.OCS_CP_SUB S    ";
                SQL = SQL + ComNum.VBLF + "    ON V.CODE = S.CODE     ";
                SQL = SQL + ComNum.VBLF + "    AND S.CPCODE = R.CPCODE    ";
                SQL = SQL + ComNum.VBLF + "    AND S.GUBUN = '06'    ";
                SQL = SQL + ComNum.VBLF + "    AND S.TYPE = '시각'    ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1     ";
                SQL = SQL + ComNum.VBLF + "     AND R.BDATE >= TO_DATE('" + strSdate + "','YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "     AND R.BDATE <= TO_DATE('" + strEdate + "','YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "     AND R.GBIO = 'E'     ";
                SQL = SQL + ComNum.VBLF + "    AND R.DROPDATE IS NULL    ";
                SQL = SQL + ComNum.VBLF + "    AND R.CANCERDATE IS NULL   ";
                SQL = SQL + ComNum.VBLF + "     AND R.CPCODE = 'CPCODE0001' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssStrokeTreat_Sheet1.Cells[1, 1].Text = dt.Rows[0]["CNT"].ToString().Trim();
                    ssStrokeTreat_Sheet1.Cells[1, 2].Text = (((int)VB.Val(dt.Rows[0]["CNT"].ToString().Trim()) / intTot) * 100).ToString("##.##");
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void GetDataEtcQueryDouble()
        {
            ssDoubleTreat_Sheet1.Cells[0, 1].Text = "";
            ssDoubleTreat_Sheet1.Cells[0, 2].Text = "";
            ssDoubleTreat_Sheet1.Cells[1, 1].Text = "";
            ssDoubleTreat_Sheet1.Cells[1, 2].Text = "";
            ssDoubleTreat_Sheet1.Cells[2, 1].Text = "";
            ssDoubleTreat_Sheet1.Cells[2, 2].Text = "";

            int intTot = (int)VB.Val(ssAll_Double_Sheet1.Cells[0, 1].Text.Trim());

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                string strSdate = dtpSDate.Value.ToString("yyyy-MM-dd");
                string strEdate = dtpEDate.Value.ToString("yyyy-MM-dd");

                SQL = " SELECT    ";
                SQL = SQL + ComNum.VBLF + "    COUNT(S.CODE) CNT        ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.OCS_CP_RECORD R     ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.OCS_CP_VALUE V    ";
                SQL = SQL + ComNum.VBLF + "    ON R.CPNO = V.CPNO    ";
                SQL = SQL + ComNum.VBLF + "    AND V.VALGB = '0'  ";
                SQL = SQL + ComNum.VBLF + "    AND V.CODE = 'CPPLN0012' ";
                SQL = SQL + ComNum.VBLF + "    AND V.CPETIME IS NOT NULL   ";
                SQL = SQL + ComNum.VBLF + "    --AND V.CPRSTN > 0    ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.OCS_CP_SUB S    ";
                SQL = SQL + ComNum.VBLF + "    ON V.CODE = S.CODE     ";
                SQL = SQL + ComNum.VBLF + "    AND S.CPCODE = R.CPCODE    ";
                SQL = SQL + ComNum.VBLF + "    AND S.GUBUN = '06'    ";
                SQL = SQL + ComNum.VBLF + "    AND S.TYPE = '시각'    ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1     ";
                SQL = SQL + ComNum.VBLF + "     AND R.BDATE >= TO_DATE('" + strSdate + "','YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "     AND R.BDATE <= TO_DATE('" + strEdate + "','YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "     AND R.GBIO = 'E'     ";
                SQL = SQL + ComNum.VBLF + "    AND R.DROPDATE IS NULL    ";
                SQL = SQL + ComNum.VBLF + "    AND R.CANCERDATE IS NULL   ";
                SQL = SQL + ComNum.VBLF + "     AND R.CPCODE = 'CPCODE0005' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssDoubleTreat_Sheet1.Cells[0, 1].Text = dt.Rows[0]["CNT"].ToString().Trim();
                    ssDoubleTreat_Sheet1.Cells[0, 2].Text = ((VB.Val(dt.Rows[0]["CNT"].ToString().Trim()) / intTot) * 100).ToString("##.##");
                }

                dt.Dispose();
                dt = null;

                SQL = " SELECT    ";
                SQL = SQL + ComNum.VBLF + "    COUNT(S.CODE) CNT        ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.OCS_CP_RECORD R     ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.OCS_CP_VALUE V    ";
                SQL = SQL + ComNum.VBLF + "    ON R.CPNO = V.CPNO    ";
                SQL = SQL + ComNum.VBLF + "    AND V.VALGB = '0'  ";
                SQL = SQL + ComNum.VBLF + "    AND V.CODE = 'CPPLN0013' ";
                SQL = SQL + ComNum.VBLF + "    AND V.CPVALV = '정복실패'   ";
                SQL = SQL + ComNum.VBLF + "    --AND V.CPRSTN > 0    ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.OCS_CP_SUB S    ";
                SQL = SQL + ComNum.VBLF + "    ON V.CODE = S.CODE     ";
                SQL = SQL + ComNum.VBLF + "    AND S.CPCODE = R.CPCODE    ";
                SQL = SQL + ComNum.VBLF + "    AND S.GUBUN = '06'    ";
                SQL = SQL + ComNum.VBLF + "    AND S.TYPE = '구분'    ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1     ";
                SQL = SQL + ComNum.VBLF + "     AND R.BDATE >= TO_DATE('" + strSdate + "','YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "     AND R.BDATE <= TO_DATE('" + strEdate + "','YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "     AND R.GBIO = 'E'     ";
                SQL = SQL + ComNum.VBLF + "    AND R.DROPDATE IS NULL    ";
                SQL = SQL + ComNum.VBLF + "    AND R.CANCERDATE IS NULL   ";
                SQL = SQL + ComNum.VBLF + "     AND R.CPCODE = 'CPCODE0005' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssDoubleTreat_Sheet1.Cells[1, 1].Text = dt.Rows[0]["CNT"].ToString().Trim();
                    ssDoubleTreat_Sheet1.Cells[1, 2].Text = ((VB.Val(dt.Rows[0]["CNT"].ToString().Trim()) / intTot) * 100).ToString("##.##");
                }

                dt.Dispose();
                dt = null;

                SQL = " SELECT    ";
                SQL = SQL + ComNum.VBLF + "    COUNT(S.CODE) CNT        ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.OCS_CP_RECORD R     ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.OCS_CP_VALUE V    ";
                SQL = SQL + ComNum.VBLF + "    ON R.CPNO = V.CPNO    ";
                SQL = SQL + ComNum.VBLF + "    AND V.VALGB = '0'  ";
                SQL = SQL + ComNum.VBLF + "    AND V.CODE = 'CPPLN0013' ";
                SQL = SQL + ComNum.VBLF + "    AND V.CPVALV IN ('비정복타입', '정복실패')   ";
                SQL = SQL + ComNum.VBLF + "    --AND V.CPRSTN > 0    ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.OCS_CP_SUB S    ";
                SQL = SQL + ComNum.VBLF + "    ON V.CODE = S.CODE     ";
                SQL = SQL + ComNum.VBLF + "    AND S.CPCODE = R.CPCODE    ";
                SQL = SQL + ComNum.VBLF + "    AND S.GUBUN = '06'    ";
                SQL = SQL + ComNum.VBLF + "    AND S.TYPE = '구분'    ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1     ";
                SQL = SQL + ComNum.VBLF + "     AND R.BDATE >= TO_DATE('" + strSdate + "','YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "     AND R.BDATE <= TO_DATE('" + strEdate + "','YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "     AND R.GBIO = 'E'     ";
                SQL = SQL + ComNum.VBLF + "    AND R.DROPDATE IS NULL    ";
                SQL = SQL + ComNum.VBLF + "    AND R.CANCERDATE IS NULL   ";
                SQL = SQL + ComNum.VBLF + "     AND R.CPCODE = 'CPCODE0005' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssDoubleTreat_Sheet1.Cells[2, 1].Text = dt.Rows[0]["CNT"].ToString().Trim();
                    ssDoubleTreat_Sheet1.Cells[2, 2].Text = ((VB.Val(dt.Rows[0]["CNT"].ToString().Trim()) / intTot) * 100).ToString("##.##");
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void GetDataAllQueryBroken()
        {
            ssBrokenTreat_Sheet1.Cells[0, 1].Text = "";

            int intTot = (int)VB.Val(ssAll_Broken_Sheet1.Cells[0, 1].Text.Trim());

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                string strSdate = dtpSDate.Value.ToString("yyyy-MM-dd");
                string strEdate = dtpEDate.Value.ToString("yyyy-MM-dd");

                SQL = " SELECT    ";
                SQL = SQL + ComNum.VBLF + "    EMS_CALL_DATE, EMS_CALL_TIME, EMS_DATE, EMS_TIME        ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.OCS_CP_RECORD R     ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1     ";
                SQL = SQL + ComNum.VBLF + "    AND R.BDATE >= TO_DATE('" + strSdate + "','YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "    AND R.BDATE <= TO_DATE('" + strEdate + "','YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "    AND R.GBIO = 'E'     ";
                SQL = SQL + ComNum.VBLF + "    AND R.DROPDATE IS NULL    ";
                SQL = SQL + ComNum.VBLF + "    AND R.CANCERDATE IS NULL   ";
                SQL = SQL + ComNum.VBLF + "    AND R.CPCODE = 'CPCODE0006' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                long lngTImeTot = 0;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string strFullDateCall = ComFunc.FormatStrToDate(dt.Rows[i]["EMS_CALL_DATE"].ToString().Trim(), "D") + " " + ComFunc.FormatStrToDate(dt.Rows[i]["EMS_CALL_TIME"].ToString().Trim(), "M");
                        string strFullDate = ComFunc.FormatStrToDate(dt.Rows[i]["EMS_DATE"].ToString().Trim(), "D") + " " + ComFunc.FormatStrToDate(dt.Rows[i]["EMS_TIME"].ToString().Trim(), "M");

                        if (VB.IsDate(strFullDateCall) == true && VB.IsDate(strFullDate) == true)
                        {
                            long lngTIme = VB.DateDiff("n", strFullDateCall, strFullDate);
                            lngTImeTot = lngTImeTot + lngTIme;
                        }
                    }
                    ssBrokenTreat_Sheet1.Cells[0, 1].Text = ((int)(lngTImeTot / intTot)).ToString();
                }

                dt.Dispose();
                dt = null;
                
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private string JinResultQuery(string pPTNO, string pINDATE, string pINTIME)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            try
            {

                SQL = " SELECT ";
                SQL = SQL + ComNum.VBLF + "    PTMIEMRT ";
                SQL = SQL + ComNum.VBLF + "FROM NUR_ER_EMIHPTMI  ";
                SQL = SQL + ComNum.VBLF + "WHERE SEQNO IN ( SELECT MAX(SEQNO) FROM NUR_ER_EMIHPTMI  ";
                SQL = SQL + ComNum.VBLF + "              WHERE PTMIIDNO = '" + pPTNO + "'  ";
                SQL = SQL + ComNum.VBLF + "                AND PTMIINDT = '" + pINDATE + "'  ";
                SQL = SQL + ComNum.VBLF + "                AND PTMIINTM = '" + pINTIME + "')  ";
                SQL = SQL + ComNum.VBLF + "AND PTMIIDNO = '" + pPTNO + "'  ";
                SQL = SQL + ComNum.VBLF + "AND PTMIINDT = '" + pINDATE + "'  ";
                SQL = SQL + ComNum.VBLF + "AND PTMIINTM = '" + pINTIME + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnVal;
                }

                switch (VB.Left(dt.Rows[0]["PTMIEMRT"].ToString().Trim(), 1))
                {
                    case "1":
                        rtnVal = "귀가";
                        break;
                    case "2":
                        rtnVal = "전원";
                        break;
                    case "3":
                        rtnVal = "입원";
                        break;
                    case "4":
                        rtnVal = "사망";
                        break;
                    case "8":
                        rtnVal = "기타";
                        break;
                    case "9":
                        rtnVal = "기타";
                        break;
                    default:
                        rtnVal = "";
                        break;
                }

                if (dt.Rows[0]["PTMIEMRT"].ToString().Trim() != "")
                {
                    rtnVal = rtnVal + " (" + dt.Rows[0]["PTMIEMRT"].ToString().Trim() + ")";
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);

                return rtnVal;
            }
        }

        private void frmOcsCpGoalStaics_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frmOcsCpGoalStaics_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }
    }
}
