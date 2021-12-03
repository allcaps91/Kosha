using ComBase;
using ComDbB;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : SupDrst
    /// File Name       : frmSupDrstTotalBanNapList.cs
    /// Description     : 통합 반납약 리스트
    /// Author          : 이정현
    /// Create Date     : 2017-09-19
    /// <history> 
    /// 통합 반납약 리스트
    /// </history>
    /// <seealso>
    /// PSMH\drug\drphar\Frm통합반납리스트.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drphar\drphar.vbp
    /// </vbp>
    /// </summary>
    public partial class frmSupDrstTotalBanNapList : Form
    {
        private string GstrWARDCODE = "";

        public frmSupDrstTotalBanNapList()
        {
            InitializeComponent();
        }

        public frmSupDrstTotalBanNapList(string strWARDCODE)
        {
            InitializeComponent();

            GstrWARDCODE = strWARDCODE;
        }

        private void frmSupDrstTotalBanNapList_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            FormClear();
        }

        private void FormClear()
        {
            dtpBanDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-1);

            chkSUM.Checked = true;

            ssPt_Sheet1.RowCount = 0;
            ssMed_Sheet1.RowCount = 0;

            if (GstrWARDCODE == "")
            {
                panOK.Visible = false;
            }
            else
            {
                panOK.Visible = true;
            }

            cboWard.Items.Clear();
            cboWard.Items.Add("전체");

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            try
            {
                SQL = "";
                SQL = "SELECT WARDCODE FROM " + ComNum.DB_PMPA + "BAS_WARD ";
                SQL = SQL + ComNum.VBLF + "     WHERE WARDCODE NOT IN ('IU','2W') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY WARDCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboWard.Items.Add(dt.Rows[i]["WARDCODE"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                cboWard.Items.Add("MICU");
                cboWard.Items.Add("SICU");
                cboWard.Items.Add("외래(EM)");
                cboWard.Items.Add("EN");
                cboWard.Items.Add("AN");
                cboWard.Items.Add("HD");
                cboWard.Items.Add("PC");
                cboWard.Items.Add("AG");
                cboWard.Items.Add("OP");

                cboWard.SelectedIndex = 0;

                if (GstrWARDCODE != "")
                {
                    ComFunc.ComboFind(cboWard, "L", 2, GstrWARDCODE);
                }
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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(tabControl1.SelectedIndex)
            {
                case 0:
                    chkSUM.Visible = true;
                    break;
                case 1:
                    chkSUM.Visible = false;
                    break;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strBanDate = "";
            string strWard_New = "";
            string strBun_New = "";
            string strBun_old = "";
            string strSuNext = "";
            string strSuName = "";
            string strSname = "";
            string strRoom = "";
            string strPano_New = "";
            string strPano_Old = "";
            string strExPress = "";

            double dblSeqNo = 0;
            double dblQty = 0;
            double dblQty_Sil = 0;  //실제 소모 개수 (주사약제만)

            int intCount = 0;
            int intPtCount = 0;
            
            try
            {
                strBanDate = dtpBanDate.Value.ToString("yyyy-MM-dd");

                #region GoSub SQL_PATIENT

                ssPt_Sheet1.RowCount = 0;

                if (Convert.ToDateTime(strBanDate) >= Convert.ToDateTime("2012-03-14"))
                {
                    //2012-03-14 경구약도 주사약과 같은 방식으로 넘어옴.(경구약 금액반환작업 하지 않음)
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     DECODE(P.WARDCODE, 'IU', DECODE(RTRIM(P.ROOMCODE), '233', 'SICU', '234', 'MICU'), P.WARDCODE) AS WARDCODE1,";
                    SQL = SQL + ComNum.VBLF + "     P.BUN, P.PTNO, P.SUCODE, ORDERNAMES, A.SNAME, P.ROOMCODE,";
                    SQL = SQL + ComNum.VBLF + "     TRUNC(SUM( DECODE(P.BCONTENTS,'0',P.REALQTY * P.NAL, P.CONTENTS / P.BCONTENTS * P.REALQTY * P.NAL)),2) AS SUM1,";
                    SQL = SQL + ComNum.VBLF + "     SUM(P.QTY * P.NAL) AS QTY";
                    SQL = SQL + ComNum.VBLF + "	 , CASE WHEN EXISTS (SELECT 1 FROM ADMIN.DRUG_WARDJEP WHERE JEPCODE = P.SUCODE AND BUN = '20' AND (TRIM(WARD) = DECODE(P.WARDCODE, 'IU', DECODE(RTRIM(P.ROOMCODE), '233', 'SICU', '234', 'MICU'), P.WARDCODE) OR WARD IS NULL)) THEN '(집계)' END ||";
                    SQL = SQL + ComNum.VBLF + "	   CASE WHEN EXISTS (SELECT 1 FROM ADMIN.DRUG_JEP WHERE JEPCODE = P.SUCODE AND SUB = '16' AND BUN = '2') THEN '★(향) ' END ||";

                    SQL = SQL + ComNum.VBLF + "	   CASE WHEN EXISTS (SELECT 1 FROM ADMIN.DRUG_JEP WHERE JEPCODE = P.SUCODE AND SUB = '17' AND BUN = '3') THEN '▼(마) ' END ||";
                    SQL = SQL + ComNum.VBLF + " 	(                                                                                                                            ";
                    SQL = SQL + ComNum.VBLF + "         SELECT LISTAGG(SHORTTEXT, '') WITHIN GROUP(ORDER BY JEPCODE) AS FORMNO                                                   ";
                    SQL = SQL + ComNum.VBLF + "           FROM ADMIN.DRUG_LAJEP                                                                                             ";
                    SQL = SQL + ComNum.VBLF + "          WHERE JEPCODE = P.SUCODE                                                                                                ";
                    SQL = SQL + ComNum.VBLF + "            AND LABUN = 'Y'                                                                                                       ";
                    SQL = SQL + ComNum.VBLF + "     ) AS GET_EXPRESS                                                                                                             ";

                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_PHARMACY P, " + ComNum.DB_MED + "OCS_ORDERCODE O, " + ComNum.DB_PMPA + "BAS_PATIENT A";
                    SQL = SQL + ComNum.VBLF + " WHERE P.ENTDATE = TO_DATE('" + strBanDate + "','YYYY-MM-DD')";

                    if (cboWard.Text.Trim() != "전체")
                    {
                        switch (cboWard.Text.Trim())
                        {
                            case "MICU":
                                SQL = SQL + ComNum.VBLF + "   AND ROOMCODE = '234'";
                                SQL = SQL + ComNum.VBLF + "   AND WARDCODE = 'IU'";
                                break;
                            case "SICU":
                                SQL = SQL + ComNum.VBLF + "   AND ROOMCODE = '233'";
                                SQL = SQL + ComNum.VBLF + "   AND WARDCODE = 'IU'";
                                break;
                            case "외래(EM)":
                                SQL = SQL + ComNum.VBLF + "   AND WARDCODE = 'ER'";
                                break;
                            default:
                                SQL = SQL + ComNum.VBLF + "   AND WARDCODE = '" + cboWard.Text.Trim() + "'";
                                break;
                        }
                    }

                    SQL = SQL + ComNum.VBLF + "   AND P.BUN IN ('20','12','11') ";
                    SQL = SQL + ComNum.VBLF + "   AND (P.GBOUT IN ('+', '-')  OR (P.GBOUT ='Y'  AND   P.NAL < 0))"; //2006-11-01 수정. 윤
                    SQL = SQL + ComNum.VBLF + "   AND P.GBTFLAG <> 'T'";   //약국요청 (JJY 2004-10-11)
                    SQL = SQL + ComNum.VBLF + "   AND P.SUCODE <> 'JAGA'"; //약국요청 (AJS 2020-08-18)
                    SQL = SQL + ComNum.VBLF + "   AND P.ORDERCODE = O.ORDERCODE";
                    SQL = SQL + ComNum.VBLF + "   AND P.PTNO = A.PANO";

                    #region 2021-11-15 NDC 취소 로직 추가
                    //SQL = SQL + ComNum.VBLF + "   AND (P.ORDERSITE <> 'CAN' OR P.ORDERSITE IS NULL) ";
                    #endregion

                    SQL = SQL + ComNum.VBLF + "GROUP BY DECODE(P.WARDCODE, 'IU', DECODE(RTRIM(P.ROOMCODE), '233', 'SICU', '234', 'MICU'), P.WARDCODE), ";
                    SQL = SQL + ComNum.VBLF + "                P.PTNO, A.SNAME, P.SUCODE, ORDERNAMES, ROOMCODE,P.BUN";
                    SQL = SQL + ComNum.VBLF + "ORDER BY 1, 2 DESC, 3";

           

                }
                else if (Convert.ToDateTime(strBanDate) >= Convert.ToDateTime("2008-07-27") && Convert.ToDateTime(strBanDate) <= Convert.ToDateTime("2012-03-13"))
                {
                    SQL = "";
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "";

                    SQL = SQL + ComNum.VBLF + "SELECT ";
                    SQL = SQL + ComNum.VBLF + "     DECODE(P.WARDCODE, 'IU', DECODE(RTRIM(P.ROOMCODE), '233', 'SICU', '234', 'MICU'), P.WARDCODE) AS WARDCODE1,";
                    SQL = SQL + ComNum.VBLF + "     P.BUN, P.PTNO, P.SUCODE, ORDERNAMES, A.SNAME, P.ROOMCODE,";
                    SQL = SQL + ComNum.VBLF + "     TRUNC(SUM(DECODE(P.BCONTENTS,'0',P.REALQTY * P.NAL, P.CONTENTS / P.BCONTENTS * P.REALQTY * P.NAL)),2) AS SUM1,";
                    //'====================================================
                    //'2010-01-04 김현욱 수정 실제 약품 소모 개수 표시 요청(0.5개 => 1개) 주사약제만,
                    //'약국 약처방전, 집계리스트, 반납리스트에 오더(또는 반환) 포함
                    SQL = SQL + ComNum.VBLF + "       SUM(P.QTY  * P.NAL) AS QTY";
                    //'====================================================
                    SQL = SQL + ComNum.VBLF + "	 , CASE WHEN EXISTS (SELECT 1 FROM ADMIN.DRUG_WARDJEP WHERE JEPCODE = P.SUCODE AND BUN = '20' AND (TRIM(WARD) = DECODE(P.WARDCODE, 'IU', DECODE(RTRIM(P.ROOMCODE), '233', 'SICU', '234', 'MICU'), P.WARDCODE) OR WARD IS NULL)) THEN '(집계)' END ||";
                    SQL = SQL + ComNum.VBLF + "	   CASE WHEN EXISTS (SELECT 1 FROM ADMIN.DRUG_JEP WHERE JEPCODE = P.SUCODE AND SUB = '16' AND BUN = '2') THEN '★(향) ' END ||";

                    SQL = SQL + ComNum.VBLF + "	   CASE WHEN EXISTS (SELECT 1 FROM ADMIN.DRUG_JEP WHERE JEPCODE = P.SUCODE AND SUB = '17' AND BUN = '3') THEN '▼(마) ' END ||";
                    SQL = SQL + ComNum.VBLF + " 	(                                                                                                                            ";
                    SQL = SQL + ComNum.VBLF + "         SELECT LISTAGG(SHORTTEXT, '') WITHIN GROUP(ORDER BY JEPCODE) AS FORMNO                                                   ";
                    SQL = SQL + ComNum.VBLF + "           FROM ADMIN.DRUG_LAJEP                                                                                             ";
                    SQL = SQL + ComNum.VBLF + "          WHERE JEPCODE = P.SUCODE                                                                                                ";
                    SQL = SQL + ComNum.VBLF + "            AND LABUN = 'Y'                                                                                                       ";
                    SQL = SQL + ComNum.VBLF + "     ) AS GET_EXPRESS                                                                                                             ";

                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_PHARMACY P, " + ComNum.DB_MED + "OCS_ORDERCODE O, " + ComNum.DB_PMPA + "BAS_PATIENT A";
                    SQL = SQL + ComNum.VBLF + " WHERE P.ENTDATE = TO_DATE('" + strBanDate + "','YYYY-MM-DD')";

                    if (cboWard.Text.Trim() != "전체")
                    {
                        switch (cboWard.Text.Trim())
                        {
                            case "MICU":
                                SQL = SQL + ComNum.VBLF + "   AND ROOMCODE = '234'";
                                SQL = SQL + ComNum.VBLF + "   AND WARDCODE = 'IU'";
                                break;
                            case "SICU":
                                SQL = SQL + ComNum.VBLF + "   AND ROOMCODE = '233'";
                                SQL = SQL + ComNum.VBLF + "   AND WARDCODE = 'IU'";
                                break;
                            case "외래(EM)":
                                SQL = SQL + ComNum.VBLF + "   AND WARDCODE = 'ER'";
                                break;
                            default:
                                SQL = SQL + ComNum.VBLF + "   AND WARDCODE = '" + cboWard.Text.Trim() + "'";
                                break;
                        }
                    }

                    SQL = SQL + ComNum.VBLF + "   AND P.BUN IN ('20','12') ";
                    SQL = SQL + ComNum.VBLF + "   AND (P.GBOUT IN ('+', '-') OR (P.GBOUT ='Y'  AND   P.NAL < 0))"; //'2006-11-01 수정. 윤
                    SQL = SQL + ComNum.VBLF + "   AND P.GBTFLAG <> 'T'"; //'약국요청 (JJY 2004-10-11)
                    SQL = SQL + ComNum.VBLF + "   AND P.ORDERCODE = O.ORDERCODE";
                    SQL = SQL + ComNum.VBLF + "   AND P.PTNO = A.PANO";
                    SQL = SQL + ComNum.VBLF + "GROUP BY DECODE(P.WARDCODE, 'IU', DECODE(RTRIM(P.ROOMCODE), '233', 'SICU', '234', 'MICU'), P.WARDCODE), ";
                    SQL = SQL + ComNum.VBLF + "                P.PTNO, A.SNAME, P.SUCODE, ORDERNAMES, ROOMCODE,P.BUN";
                    SQL = SQL + ComNum.VBLF + "UNION ALL ";
                    SQL = SQL + ComNum.VBLF + "SELECT ";
                    SQL = SQL + ComNum.VBLF + "     DECODE(P.WARDCODE, 'IU', DECODE(RTRIM(P.ROOMCODE), '233', 'SICU', '234', 'MICU'), P.WARDCODE) AS WARDCODE1,";
                    SQL = SQL + ComNum.VBLF + "     P.BUN, P.PTNO, P.SUCODE, ORDERNAMES, A.SNAME, P.ROOMCODE,";
                    SQL = SQL + ComNum.VBLF + "     TRUNC(SUM( DECODE(P.BCONTENTS,'0',P.REALQTY * P.NAL, P.CONTENTS / P.BCONTENTS * P.REALQTY * P.NAL)),2) AS SUM1,";
                    SQL = SQL + ComNum.VBLF + "     SUM(P.QTY  * P.NAL) AS QTY";
                    SQL = SQL + ComNum.VBLF + "	 , CASE WHEN EXISTS (SELECT 1 FROM ADMIN.DRUG_WARDJEP WHERE JEPCODE = P.SUCODE AND BUN = '20' AND (TRIM(WARD) = DECODE(P.WARDCODE, 'IU', DECODE(RTRIM(P.ROOMCODE), '233', 'SICU', '234', 'MICU'), P.WARDCODE) OR WARD IS NULL)) THEN '(집계)' END ||";
                    SQL = SQL + ComNum.VBLF + "	   CASE WHEN EXISTS (SELECT 1 FROM ADMIN.DRUG_JEP WHERE JEPCODE = P.SUCODE AND SUB = '16' AND BUN = '2') THEN '★(향) ' END ||";

                    SQL = SQL + ComNum.VBLF + "	   CASE WHEN EXISTS (SELECT 1 FROM ADMIN.DRUG_JEP WHERE JEPCODE = P.SUCODE AND SUB = '17' AND BUN = '3') THEN '▼(마) ' END ||";
                    SQL = SQL + ComNum.VBLF + " 	(                                                                                                                            ";
                    SQL = SQL + ComNum.VBLF + "         SELECT LISTAGG(SHORTTEXT, '') WITHIN GROUP(ORDER BY JEPCODE) AS FORMNO                                                   ";
                    SQL = SQL + ComNum.VBLF + "           FROM ADMIN.DRUG_LAJEP                                                                                             ";
                    SQL = SQL + ComNum.VBLF + "          WHERE JEPCODE = P.SUCODE                                                                                                ";
                    SQL = SQL + ComNum.VBLF + "            AND LABUN = 'Y'                                                                                                       ";
                    SQL = SQL + ComNum.VBLF + "     ) AS GET_EXPRESS                                                                                                             ";

                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_PHARMACY P, " + ComNum.DB_MED + "OCS_ORDERCODE O, " + ComNum.DB_PMPA + "BAS_PATIENT A";
                    SQL = SQL + ComNum.VBLF + " WHERE P.ENTDATE = TO_DATE('" + strBanDate + "','YYYY-MM-DD')";

                    if (cboWard.Text.Trim() != "전체")
                    {
                        switch (cboWard.Text.Trim())
                        {
                            case "MICU":
                                SQL = SQL + ComNum.VBLF + "   AND ROOMCODE = '234'";
                                SQL = SQL + ComNum.VBLF + "   AND WARDCODE = 'IU'";
                                break;
                            case "SICU":
                                SQL = SQL + ComNum.VBLF + "   AND ROOMCODE = '233'";
                                SQL = SQL + ComNum.VBLF + "   AND WARDCODE = 'IU'";
                                break;
                            case "외래(EM)":
                                SQL = SQL + ComNum.VBLF + "   AND WARDCODE = 'ER'";
                                break;
                            default:
                                SQL = SQL + ComNum.VBLF + "   AND WARDCODE = '" + cboWard.Text.Trim() + "'";
                                break;
                        }
                    }

                    SQL = SQL + ComNum.VBLF + "   AND P.BUN IN ('11')";
                    SQL = SQL + ComNum.VBLF + "   AND P.GBOUT IN ('+', '-') "; //'간호사 DC
                    SQL = SQL + ComNum.VBLF + "   AND P.GBTFLAG <> 'T'"; //약국요청 (JJY 2004-10-11)
                    SQL = SQL + ComNum.VBLF + "   AND P.ORDERCODE = O.ORDERCODE";
                    SQL = SQL + ComNum.VBLF + "   AND P.PTNO = A.PANO";
                    SQL = SQL + ComNum.VBLF + "GROUP BY DECODE(P.WARDCODE, 'IU', DECODE(RTRIM(P.ROOMCODE), '233', 'SICU', '234', 'MICU'), P.WARDCODE), ";
                    SQL = SQL + ComNum.VBLF + "                P.PTNO, A.SNAME, P.SUCODE, ORDERNAMES, ROOMCODE,P.BUN";
                    SQL = SQL + ComNum.VBLF + "ORDER BY 2, 1 DESC, 3";
                }
                else
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.BUN, A.SUNEXT AS SUCODE, B.SUNAMEK AS ORDERNAMES, SUM(A.QTY*A.NAL) AS SUM1 ,";
                    SQL = SQL + ComNum.VBLF + "     A.PANO AS PTNO, C.SNAME, D.WARDCODE, A.ROOMCODE,";
                    SQL = SQL + ComNum.VBLF + "     DECODE(D.WARDCODE,'IU',DECODE(RTRIM(A.ROOMCODE),'233','SICU','234','MICU','MICU'), D.WARDCODE) AS WARDCODE1";
                    SQL = SQL + ComNum.VBLF + "	 , CASE WHEN EXISTS (SELECT 1 FROM ADMIN.DRUG_WARDJEP WHERE JEPCODE = P.SUCODE AND BUN = '20' AND (TRIM(WARD) = DECODE(P.WARDCODE, 'IU', DECODE(RTRIM(P.ROOMCODE), '233', 'SICU', '234', 'MICU'), P.WARDCODE) OR WARD IS NULL)) THEN '(집계)' END ||";
                    SQL = SQL + ComNum.VBLF + "	   CASE WHEN EXISTS (SELECT 1 FROM ADMIN.DRUG_JEP WHERE JEPCODE = P.SUCODE AND SUB = '16' AND BUN = '2') THEN '★(향) ' END ||";

                    SQL = SQL + ComNum.VBLF + "	   CASE WHEN EXISTS (SELECT 1 FROM ADMIN.DRUG_JEP WHERE JEPCODE = P.SUCODE AND SUB = '17' AND BUN = '3') THEN '▼(마) ' END ||";
                    SQL = SQL + ComNum.VBLF + " 	(                                                                                                                            ";
                    SQL = SQL + ComNum.VBLF + "         SELECT LISTAGG(SHORTTEXT, '') WITHIN GROUP(ORDER BY JEPCODE) AS FORMNO                                                   ";
                    SQL = SQL + ComNum.VBLF + "           FROM ADMIN.DRUG_LAJEP                                                                                             ";
                    SQL = SQL + ComNum.VBLF + "          WHERE JEPCODE = P.SUCODE                                                                                                ";
                    SQL = SQL + ComNum.VBLF + "            AND LABUN = 'Y'                                                                                                       ";
                    SQL = SQL + ComNum.VBLF + "     ) AS GET_EXPRESS                                                                                                             ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP A,";
                    SQL = SQL + ComNum.VBLF + "      " + ComNum.DB_PMPA + "BAS_SUN B,";
                    SQL = SQL + ComNum.VBLF + "      " + ComNum.DB_PMPA + "BAS_PATIENT C,";
                    SQL = SQL + ComNum.VBLF + "      " + ComNum.DB_PMPA + "BAS_ROOM D";
                    SQL = SQL + ComNum.VBLF + " WHERE A.ACTDATE = TO_DATE('" + strBanDate + "','YYYY-MM-DD')  ";
                    SQL = SQL + ComNum.VBLF + "   AND A.PART = '!'";
                    SQL = SQL + ComNum.VBLF + "   AND A.NAL = -1 ";
                    SQL = SQL + ComNum.VBLF + "   AND A.PANO = C.PANO(+)";

                    if (cboWard.Text.Trim() != "전체")
                    {
                        switch (cboWard.Text.Trim())
                        {
                            case "MICU":
                                SQL = SQL + ComNum.VBLF + "   AND A.ROOMCODE = '234'";
                                break;
                            case "SICU":
                                SQL = SQL + ComNum.VBLF + "   AND A.ROOMCODE = '233'";
                                break;
                            default:
                                SQL = SQL + ComNum.VBLF + "   AND D.WARDCODE = '" + cboWard.Text.Trim() + "'  ";
                                break;
                        }
                    }

                    SQL = SQL + ComNum.VBLF + "   AND A.ROOMCODE = D.ROOMCODE(+)";
                    SQL = SQL + ComNum.VBLF + "   AND A.BUN IN ('11','12','20')";
                    SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT = B.SUNEXT";
                    SQL = SQL + ComNum.VBLF + "   AND A.GBSLIP <> 'T'";  //퇴원약 제외
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.BUN,A.SUNEXT,B.SUNAMEK,A.PANO,C.SNAME,";
                    SQL = SQL + ComNum.VBLF + "         D.WARDCODE,A.ROOMCODE";
                    SQL = SQL + ComNum.VBLF + "ORDER BY WARDCODE1,BUN DESC,A.PANO";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strWard_New = dt.Rows[i]["WARDCODE1"].ToString().Trim();
                        strBun_New = dt.Rows[i]["BUN"].ToString().Trim();

                        switch (strBun_New)
                        {
                            case "11":
                                strBun_New = "내복약";
                                break;
                            case "12":
                                strBun_New = "외용약";
                                break;
                            case "20":
                                strBun_New = "주사약";
                                break;
                        }

                        strSuNext = dt.Rows[i]["SUCODE"].ToString().Trim();
                        strSuName = clsVbfunc.READ_SugaName(clsDB.DbCon, dt.Rows[i]["SUCODE"].ToString().Trim());
                        strSname = dt.Rows[i]["SNAME"].ToString().Trim();
                        strPano_New = dt.Rows[i]["PTNO"].ToString().Trim();
                        dblQty = VB.Val(dt.Rows[i]["SUM1"].ToString().Trim());
                        strRoom = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        dblQty_Sil = VB.Val(dt.Rows[i]["QTY"].ToString().Trim());

                        strExPress = dt.Rows[i]["GET_EXPRESS"].ToString().Trim();
                        //strExPress = GET_EXPRESS(clsDB.DbCon, strSuNext);

                        //개수가
                        if (dblQty_Sil < 0)
                        {
                            if (strBun_New != strBun_old)
                            {
                                dblSeqNo = 0;
                                dblSeqNo++;

                                ssPt_Sheet1.RowCount = ssPt_Sheet1.RowCount + 1;
                                ssPt_Sheet1.SetRowHeight(ssPt_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                                if (strBun_old != "")
                                {
                                    ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - intCount - 1, 0].RowSpan = intCount;

                                    ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - intPtCount - 1, 1].RowSpan = intPtCount;
                                    ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - intPtCount - 1, 2].RowSpan = intPtCount;
                                    ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - intPtCount - 1, 3].RowSpan = intPtCount;
                                    ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - intPtCount - 1, 4].RowSpan = intPtCount;

                                    intCount = 0;
                                    intPtCount = 0;
                                }

                                ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - 1, 0].Text = strBun_New;
                                ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - 1, 1].Text = dblSeqNo.ToString();
                                ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - 1, 2].Text = strPano_New;
                                ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - 1, 3].Text = strSname;
                                ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - 1, 4].Text = strRoom;
                                ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - 1, 5].Text = " " + strSuNext;
                                ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - 1, 6].Text = " " + strExPress + strSuName;

                                //2016-03-18 실수량 표시 요청
                                if (dblQty != dblQty_Sil)
                                {
                                    ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - 1, 7].Text = dblQty + "(" + dblQty_Sil + ")";
                                }
                                else
                                {
                                    ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - 1, 7].Text = dblQty.ToString();
                                }

                                ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - 1, 8].Text = "";

                                strPano_Old = strPano_New;
                                strBun_old = strBun_New;

                                intCount++;
                                intPtCount++;
                            }
                            else
                            {
                                ssPt_Sheet1.RowCount = ssPt_Sheet1.RowCount + 1;
                                ssPt_Sheet1.SetRowHeight(ssPt_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                                if (strPano_New != strPano_Old)
                                {
                                    if (strPano_Old != "")
                                    {
                                        ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - intPtCount - 1, 1].RowSpan = intPtCount;
                                        ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - intPtCount - 1, 2].RowSpan = intPtCount;
                                        ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - intPtCount - 1, 3].RowSpan = intPtCount;
                                        ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - intPtCount - 1, 4].RowSpan = intPtCount;

                                        intPtCount = 0;
                                    }

                                    dblSeqNo++;
                                    ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - 1, 1].Text = dblSeqNo.ToString();
                                    ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - 1, 2].Text = strPano_New;
                                    ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - 1, 3].Text = strSname;
                                    ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - 1, 4].Text = strRoom;
                                }

                                ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - 1, 5].Text = " " + strSuNext;
                                ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - 1, 6].Text = " " + strExPress + strSuName;
                                
                                //2016-03-18 실수량 표시 요청
                                if (dblQty != dblQty_Sil)
                                {
                                    ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - 1, 7].Text = dblQty + "(" + dblQty_Sil + ")";
                                }
                                else
                                {
                                    ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - 1, 7].Text = dblQty.ToString();
                                }

                                ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - 1, 8].Text = "";

                                strPano_Old = strPano_New;
                                strBun_old = strBun_New;

                                intCount++;
                                intPtCount++;
                            }
                        }

                        Application.DoEvents();
                    }
                    
                    ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - intCount, 0].RowSpan = intCount;

                    ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - intPtCount, 1].RowSpan = intPtCount;
                    ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - intPtCount, 2].RowSpan = intPtCount;
                    ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - intPtCount, 3].RowSpan = intPtCount;
                    ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - intPtCount, 4].RowSpan = intPtCount;
                }

                dt.Dispose();
                dt = null;

                #endregion

                strBun_old = "";
                strBun_New = "";

                #region GoSub SQL_DRUG

                intCount = 0;
                intPtCount = 0;
                ssMed_Sheet1.RowCount = 0;

                if (Convert.ToDateTime(strBanDate) >= Convert.ToDateTime("2012-03-14"))
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     P.BUN, P.SUCODE, ORDERNAMES,";
                    SQL = SQL + ComNum.VBLF + "     TRUNC(SUM( DECODE(P.BCONTENTS,'0',P.REALQTY * P.NAL, P.CONTENTS / P.BCONTENTS * P.REALQTY * P.NAL)),2) AS SUM1,";
                    SQL = SQL + ComNum.VBLF + "     SUM(P.QTY * P.NAL) AS QTY ";
                    SQL = SQL + ComNum.VBLF + "	 , CASE WHEN EXISTS (SELECT 1 FROM ADMIN.DRUG_WARDJEP WHERE JEPCODE = P.SUCODE AND BUN = '20' AND WARD = '" + cboWard.Text.Trim() + "') THEN '(집계)' END ||";
                    SQL = SQL + ComNum.VBLF + "	   CASE WHEN EXISTS (SELECT 1 FROM ADMIN.DRUG_JEP WHERE JEPCODE = P.SUCODE AND SUB = '16' AND BUN = '2') THEN '★(향) ' END ||";
                    SQL = SQL + ComNum.VBLF + "	   CASE WHEN EXISTS (SELECT 1 FROM ADMIN.DRUG_JEP WHERE JEPCODE = P.SUCODE AND SUB = '17' AND BUN = '3') THEN '▼(마) ' END ||";
                    SQL = SQL + ComNum.VBLF + " 	(                                                                                                                            ";
                    SQL = SQL + ComNum.VBLF + "         SELECT LISTAGG(SHORTTEXT, '') WITHIN GROUP(ORDER BY JEPCODE) AS FORMNO                                                   ";
                    SQL = SQL + ComNum.VBLF + "           FROM ADMIN.DRUG_LAJEP                                                                                             ";
                    SQL = SQL + ComNum.VBLF + "          WHERE JEPCODE = P.SUCODE                                                                                                ";
                    SQL = SQL + ComNum.VBLF + "            AND LABUN = 'Y'                                                                                                       ";
                    SQL = SQL + ComNum.VBLF + "     ) AS GET_EXPRESS                                                                                                             ";

                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_PHARMACY P, " + ComNum.DB_MED + "OCS_ORDERCODE O, " + ComNum.DB_PMPA + "BAS_PATIENT A";
                    SQL = SQL + ComNum.VBLF + "     WHERE P.ENTDATE = TO_DATE('" + dtpBanDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "         AND P.BUN IN ('20', '12', '11')";
                    SQL = SQL + ComNum.VBLF + "         AND (P.GBOUT IN ('+', '-') OR (P.GBOUT ='Y' AND P.NAL < 0))";
                    SQL = SQL + ComNum.VBLF + "         AND P.GBTFLAG <> 'T'";
                    SQL = SQL + ComNum.VBLF + "         AND P.ORDERCODE = O.ORDERCODE";
                    SQL = SQL + ComNum.VBLF + "         AND P.PTNO = A.PANO";

                    #region 2021-11-15 NDC 취소 로직 추가
                    //SQL = SQL + ComNum.VBLF + "   AND (P.ORDERSITE <> 'CAN' OR P.ORDERSITE IS NULL) ";
                    #endregion

                    if (cboWard.Text.Trim() != "전체")
                    {
                        switch (cboWard.Text.Trim())
                        {
                            case "MICU":
                                SQL = SQL + ComNum.VBLF + "         AND ROOMCODE = '234' ";
                                SQL = SQL + ComNum.VBLF + "         AND WARDCODE = 'IU' ";
                                break;
                            case "SICU":
                                SQL = SQL + ComNum.VBLF + "         AND ROOMCODE = '233' ";
                                SQL = SQL + ComNum.VBLF + "         AND WARDCODE = 'IU' ";
                                break;
                            case "외래(EM)":
                                SQL = SQL + ComNum.VBLF + "         AND WARDCODE = 'ER' ";
                                break;
                            default:
                                SQL = SQL + ComNum.VBLF + "         AND WARDCODE = '" + cboWard.Text.Trim() + "' ";
                                break;
                        }
                    }

                    SQL = SQL + ComNum.VBLF + "GROUP BY P.BUN, P.SUCODE, ORDERNAMES";
                    SQL = SQL + ComNum.VBLF + "ORDER BY 1 DESC, 2, 3";
                }
                else
                {
                    //약 구분 별로 화면에 표시 2008-07-27 이전 자료 처리 안함.
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     P.BUN, P.SUCODE, ORDERNAMES,";
                    SQL = SQL + ComNum.VBLF + "     TRUNC(SUM( DECODE(P.BCONTENTS,'0',P.REALQTY * P.NAL, P.CONTENTS / P.BCONTENTS * P.REALQTY * P.NAL)),2) AS SUM1,";
                    SQL = SQL + ComNum.VBLF + "     SUM(P.QTY * P.NAL) QTY ";
                    SQL = SQL + ComNum.VBLF + "	 , CASE WHEN EXISTS (SELECT 1 FROM ADMIN.DRUG_WARDJEP WHERE JEPCODE = P.SUCODE AND BUN = '20' AND WARD = '" + cboWard.Text.Trim() + "') THEN '(집계)' END ||";
                    SQL = SQL + ComNum.VBLF + "	   CASE WHEN EXISTS (SELECT 1 FROM ADMIN.DRUG_JEP WHERE JEPCODE = P.SUCODE AND SUB = '16' AND BUN = '2') THEN '★(향) ' END ||";
                    SQL = SQL + ComNum.VBLF + "	   CASE WHEN EXISTS (SELECT 1 FROM ADMIN.DRUG_JEP WHERE JEPCODE = P.SUCODE AND SUB = '17' AND BUN = '3') THEN '▼(마) ' END ||";
                    SQL = SQL + ComNum.VBLF + " 	(                                                                                                                            ";
                    SQL = SQL + ComNum.VBLF + "         SELECT LISTAGG(SHORTTEXT, '') WITHIN GROUP(ORDER BY JEPCODE) AS FORMNO                                                   ";
                    SQL = SQL + ComNum.VBLF + "           FROM ADMIN.DRUG_LAJEP                                                                                             ";
                    SQL = SQL + ComNum.VBLF + "          WHERE JEPCODE = P.SUCODE                                                                                                ";
                    SQL = SQL + ComNum.VBLF + "            AND LABUN = 'Y'                                                                                                       ";
                    SQL = SQL + ComNum.VBLF + "     ) AS GET_EXPRESS                                                                                                             ";

                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_PHARMACY P, " + ComNum.DB_MED + "OCS_ORDERCODE O, " + ComNum.DB_PMPA + "BAS_PATIENT A";
                    SQL = SQL + ComNum.VBLF + "     WHERE P.ENTDATE = TO_DATE('" + dtpBanDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "         AND P.BUN IN ('20','12')";
                    SQL = SQL + ComNum.VBLF + "         AND (P.GBOUT IN ('+', '-') OR (P.GBOUT ='Y' AND P.NAL < 0))";
                    SQL = SQL + ComNum.VBLF + "         AND P.GBTFLAG <> 'T'";
                    SQL = SQL + ComNum.VBLF + "         AND P.ORDERCODE = O.ORDERCODE";
                    SQL = SQL + ComNum.VBLF + "         AND P.PTNO = A.PANO";

                    if (cboWard.Text.Trim() != "전체")
                    {
                        switch (cboWard.Text.Trim())
                        {
                            case "MICU":
                                SQL = SQL + ComNum.VBLF + " AND   ROOMCODE = '234' ";
                                SQL = SQL + ComNum.VBLF + " AND   WARDCODE = 'IU' ";
                                break;
                            case "SICU":
                                SQL = SQL + ComNum.VBLF + " AND   ROOMCODE = '233' ";
                                SQL = SQL + ComNum.VBLF + " AND   WARDCODE = 'IU' ";
                                break;
                            case "외래(EM)":
                                SQL = SQL + ComNum.VBLF + " AND   WARDCODE = 'ER' ";
                                break;
                            default:
                                SQL = SQL + ComNum.VBLF + " AND   WARDCODE = '" + cboWard.Text.Trim() + "' ";
                                break;
                        }
                    }

                    SQL = SQL + ComNum.VBLF + "GROUP BY P.BUN, P.SUCODE, ORDERNAMES";
                    SQL = SQL + ComNum.VBLF + "Union All";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     P.BUN, P.SUCODE, ORDERNAMES,";
                    SQL = SQL + ComNum.VBLF + "     TRUNC(SUM( DECODE(P.BCONTENTS,'0',P.REALQTY * P.NAL, P.CONTENTS / P.BCONTENTS * P.REALQTY * P.NAL)),2) AS SUM1,";
                    SQL = SQL + ComNum.VBLF + "     SUM(P.QTY * P.NAL) AS QTY ";
                    SQL = SQL + ComNum.VBLF + "	 , CASE WHEN EXISTS (SELECT 1 FROM ADMIN.DRUG_WARDJEP WHERE JEPCODE = P.SUCODE AND BUN = '20' AND WARD = '" + cboWard.Text.Trim() + "') THEN '(집계)' END ||";
                    SQL = SQL + ComNum.VBLF + "	   CASE WHEN EXISTS (SELECT 1 FROM ADMIN.DRUG_JEP WHERE JEPCODE = P.SUCODE AND SUB = '16' AND BUN = '2') THEN '★(향) ' END ||";
                    SQL = SQL + ComNum.VBLF + "	   CASE WHEN EXISTS (SELECT 1 FROM ADMIN.DRUG_JEP WHERE JEPCODE = P.SUCODE AND SUB = '17' AND BUN = '3') THEN '▼(마) ' END ||";
                    SQL = SQL + ComNum.VBLF + " 	(                                                                                                                            ";
                    SQL = SQL + ComNum.VBLF + "         SELECT LISTAGG(SHORTTEXT, '') WITHIN GROUP(ORDER BY JEPCODE) AS FORMNO                                                   ";
                    SQL = SQL + ComNum.VBLF + "           FROM ADMIN.DRUG_LAJEP                                                                                             ";
                    SQL = SQL + ComNum.VBLF + "          WHERE JEPCODE = P.SUCODE                                                                                                ";
                    SQL = SQL + ComNum.VBLF + "            AND LABUN = 'Y'                                                                                                       ";
                    SQL = SQL + ComNum.VBLF + "     ) AS GET_EXPRESS                                                                                                             ";

                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_PHARMACY P, " + ComNum.DB_MED + "OCS_ORDERCODE O, " + ComNum.DB_PMPA + "BAS_PATIENT A";
                    SQL = SQL + ComNum.VBLF + "     WHERE P.ENTDATE = TO_DATE('" + dtpBanDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "         AND P.BUN IN ('11')";
                    SQL = SQL + ComNum.VBLF + "         AND P.GBOUT IN ('+', '-')";
                    SQL = SQL + ComNum.VBLF + "         AND P.GBTFLAG <> 'T'";
                    SQL = SQL + ComNum.VBLF + "         AND P.ORDERCODE = O.ORDERCODE";
                    SQL = SQL + ComNum.VBLF + "         AND P.PTNO = A.PANO";

                    if (cboWard.Text.Trim() != "전체")
                    {
                        switch (cboWard.Text.Trim())
                        {
                            case "MICU":
                                SQL = SQL + ComNum.VBLF + "         AND ROOMCODE = '234' ";
                                SQL = SQL + ComNum.VBLF + "         AND WARDCODE = 'IU' ";
                                break;
                            case "SICU":
                                SQL = SQL + ComNum.VBLF + "         AND ROOMCODE = '233' ";
                                SQL = SQL + ComNum.VBLF + "         AND WARDCODE = 'IU' ";
                                break;
                            case "외래(EM)":
                                SQL = SQL + ComNum.VBLF + "         AND WARDCODE = 'ER' ";
                                break;
                            default:
                                SQL = SQL + ComNum.VBLF + "         AND WARDCODE = '" + cboWard.Text.Trim() + "' ";
                                break;
                        }
                    }

                    SQL = SQL + ComNum.VBLF + "GROUP BY P.BUN, P.SUCODE, ORDERNAMES";
                    SQL = SQL + ComNum.VBLF + "ORDER BY 1 DESC, 2, 3";
                }
                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strBun_New = dt.Rows[i]["BUN"].ToString().Trim();

                        switch (strBun_New)
                        {
                            case "11":
                                strBun_New = "내복약";
                                break;
                            case "12":
                                strBun_New = "외용약";
                                break;
                            case "20":
                                strBun_New = "주사약";
                                break;
                        }

                        strSuNext = dt.Rows[i]["SUCODE"].ToString().Trim();
                        strSuName = clsVbfunc.READ_SugaName(clsDB.DbCon, dt.Rows[i]["SUCODE"].ToString().Trim());
                        dblQty = VB.Val(dt.Rows[i]["SUM1"].ToString().Trim());
                        dblQty_Sil = VB.Val(dt.Rows[i]["QTY"].ToString().Trim());

                        strExPress = dt.Rows[i]["GET_EXPRESS"].ToString().Trim();
                        //strExPress = GET_EXPRESS(clsDB.DbCon, strSuNext);

                        if (dblQty_Sil < 0)
                        {
                            if (strBun_New == "주사약"  && chkSUM.Checked == true)
                            {
                                ssPt_Sheet1.RowCount = ssPt_Sheet1.RowCount + 1;
                                ssPt_Sheet1.SetRowHeight(ssPt_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                                ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - 1, 5].Text = " " + strSuNext;
                                ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - 1, 6].Text = " " + strExPress + strSuName;

                                if (dblQty != dblQty_Sil)
                                {
                                    ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - 1, 7].Text = dblQty + "(" + dblQty_Sil + ")";
                                }
                                else
                                {
                                    ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - 1, 7].Text = dblQty.ToString();
                                }

                                intPtCount++;
                            }

                            ssMed_Sheet1.RowCount = ssMed_Sheet1.RowCount + 1;
                            ssMed_Sheet1.SetRowHeight(ssMed_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                            if (strBun_New != strBun_old)
                            {
                                dblSeqNo = 0;
                                dblSeqNo++;

                                if (strBun_old != "")
                                {
                                    ssMed_Sheet1.Cells[ssMed_Sheet1.RowCount - intCount - 1, 0].RowSpan = intCount;
                                    
                                    intCount = 0;
                                }

                                ssMed_Sheet1.Cells[ssMed_Sheet1.RowCount - 1, 0].Text = strBun_New;
                            }

                            ssMed_Sheet1.Cells[ssMed_Sheet1.RowCount - 1, 1].Text = " " + strSuNext;
                            ssMed_Sheet1.Cells[ssMed_Sheet1.RowCount - 1, 2].Text = " " + strExPress + strSuName;

                            if (dblQty != dblQty_Sil)
                            {
                                ssMed_Sheet1.Cells[ssMed_Sheet1.RowCount - 1, 3].Text = dblQty + "(" + dblQty_Sil + ")";
                            }
                            else
                            {
                                ssMed_Sheet1.Cells[ssMed_Sheet1.RowCount - 1, 3].Text = dblQty.ToString();
                            }

                            strBun_old = strBun_New;
                            intCount++;
                        }

                        Application.DoEvents();
                    }

                    ssMed_Sheet1.Cells[ssMed_Sheet1.RowCount - intCount, 0].RowSpan = intCount;

                    if (chkSUM.Checked == true && intPtCount > 0)
                    {
                        ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - intPtCount, 0].RowSpan = intPtCount;
                        ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - intPtCount, 0].ColumnSpan = 5;
                        ssPt_Sheet1.Cells[ssPt_Sheet1.RowCount - intPtCount, 0].Text = "주사약 합계 조회";
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion
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

        private string GET_EXPRESS(PsmhDb pDbCon, string strJEPCODE)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";
            int i = 0;

            string strHyang = "";
            string strMayak = "";
            string strSHORTText = "";

            try
            {
                SQL = "";
                SQL = "SELECT SHORTTEXT FROM " + ComNum.DB_ERP + "DRUG_LAJEP";
                SQL = SQL + ComNum.VBLF + "     WHERE JEPCODE = '" + strJEPCODE + "' ";
                SQL = SQL + ComNum.VBLF + "         AND LABUN = 'Y'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strSHORTText = strSHORTText + dt.Rows[i]["SHORTTEXT"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                //마약 표시
                SQL = "";
                SQL = "SELECT JEPCODE FROM " + ComNum.DB_ERP + "DRUG_JEP ";
                SQL = SQL + ComNum.VBLF + "     WHERE JEPCODE = '" + strJEPCODE + "' ";
                SQL = SQL + ComNum.VBLF + "         AND SUB = '17'";
                SQL = SQL + ComNum.VBLF + "         AND BUN = '3' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strMayak = "▼(마) ";
                }

                dt.Dispose();
                dt = null;

                //항정주사약 조회
                SQL = "";
                SQL = "SELECT JEPCODE FROM " + ComNum.DB_ERP + "DRUG_JEP ";
                SQL = SQL + ComNum.VBLF + "     WHERE JEPCODE = '" + strJEPCODE + "' ";
                SQL = SQL + ComNum.VBLF + "         AND SUB = '16'";
                SQL = SQL + ComNum.VBLF + "         AND BUN = '2' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strHyang = "★(향) ";
                }

                dt.Dispose();
                dt = null;

                rtnVal = strHyang + strMayak + strSHORTText;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            FormClear();
        }

        private void btnPrintPt_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            PreViewAndPrint(ssPt);
        }

        private void btnPrintMed_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            PreViewAndPrint(ssMed);
        }

        private void PreViewAndPrint(FarPoint.Win.Spread.FpSpread ssSpread)
        {
            string strFont1 = "";
            string strHead1 = "";
            string strFont2 = "";
            string strHead2 = "";
            
            strFont1 = "/fn\"맑은 고딕\" /fz\"18\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"맑은 고딕\" /fz\"12\" /fb0 /fi0 /fu0 /fk0 /fs2";

            if (cboWard.Text.Trim() == "전체")
            {
                for (int i = 1; i < cboWard.Items.Count; i++)
                {
                    cboWard.SelectedIndex = i;

                    strHead1 = "/c/f1" + "병동별 반납약 대장" + "/f1/n";
                    strHead2 = "/l/f2" + "반납일자 : " + dtpBanDate.Value.ToString("yyyy-MM-dd") + VB.Space(5) + "병동 : " + cboWard.Text.Trim()
                        + VB.Space(10) + "반납인:____________     약제확인:____________" + "/f2/n";

                    Application.DoEvents();
                    Task.Delay(2000);
                    Application.DoEvents();
                    Task.Delay(2000);

                    GetData();
                    
                    if (ssSpread.ActiveSheet.RowCount > 0)
                    {
                        ssSpread.ActiveSheet.PrintInfo.Printer = "\\\\192.168.30.50\\주사집계";
                        ssSpread.ActiveSheet.PrintInfo.AbortMessage = "프린터 중입니다.";
                        ssSpread.ActiveSheet.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
                        ssSpread.ActiveSheet.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
                        ssSpread.ActiveSheet.PrintInfo.Margin.Top = 20;
                        ssSpread.ActiveSheet.PrintInfo.Margin.Header = 20;
                        ssSpread.ActiveSheet.PrintInfo.Margin.Bottom = 20;
                        ssSpread.ActiveSheet.PrintInfo.ShowColor = false;
                        ssSpread.ActiveSheet.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
                        ssSpread.ActiveSheet.PrintInfo.ShowBorder = true;
                        ssSpread.ActiveSheet.PrintInfo.ShowGrid = true;
                        ssSpread.ActiveSheet.PrintInfo.ShowShadows = false;
                        ssSpread.ActiveSheet.PrintInfo.UseMax = true;
                        ssSpread.ActiveSheet.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                        ssSpread.ActiveSheet.PrintInfo.UseSmartPrint = false;
                        ssSpread.ActiveSheet.PrintInfo.ShowPrintDialog = false;
                        ssSpread.ActiveSheet.PrintInfo.Preview = false;
                        ssSpread.PrintSheet(0);

                        Application.DoEvents();
                        Task.Delay(2000);
                        Application.DoEvents();
                        Task.Delay(2000);
                    }
                }
            }
            else
            {
                strHead1 = "/c/f1" + "병동별 반납약 대장" + "/f1/n";
                strHead2 = "/l/f2" + "반납일자 : " + dtpBanDate.Value.ToString("yyyy-MM-dd") + VB.Space(5) + "병동 : " + cboWard.Text.Trim()
                    + VB.Space(10) + "반납인:____________     약제확인:____________" + "/f2/n";

                ssSpread.ActiveSheet.PrintInfo.Printer = "\\\\192.168.30.50\\주사집계";
                ssSpread.ActiveSheet.PrintInfo.AbortMessage = "프린터 중입니다.";
                ssSpread.ActiveSheet.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
                ssSpread.ActiveSheet.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
                ssSpread.ActiveSheet.PrintInfo.Margin.Top = 20;
                ssSpread.ActiveSheet.PrintInfo.Margin.Header = 20;
                ssSpread.ActiveSheet.PrintInfo.Margin.Bottom = 20;
                ssSpread.ActiveSheet.PrintInfo.ShowColor = false;
                ssSpread.ActiveSheet.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
                ssSpread.ActiveSheet.PrintInfo.ShowBorder = true;
                ssSpread.ActiveSheet.PrintInfo.ShowGrid = true;
                ssSpread.ActiveSheet.PrintInfo.ShowShadows = false;
                ssSpread.ActiveSheet.PrintInfo.UseMax = true;
                ssSpread.ActiveSheet.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                ssSpread.ActiveSheet.PrintInfo.UseSmartPrint = false;
                ssSpread.ActiveSheet.PrintInfo.ShowPrintDialog = false;
                ssSpread.ActiveSheet.PrintInfo.Preview = false;
                ssSpread.PrintSheet(0);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }
    }
}
