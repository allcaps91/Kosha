using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedIpdNr
    /// File Name       : frmEducationTargetList.cs
    /// Description     : 교육 대상자 명단
    /// Author          : 이현종
    /// Create Date     : 2018-05-17
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\insa\insa2\insa2_22.frm(insa2_22.frm) >> frmEducationTargetList.cs 폼이름 재정의" />	
    public partial class frmEducationTargetList : Form, MainFormMessage
    {
        string mPara1 = "";

        #region MainFormMessage InterFace

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

        #endregion

        string Show_yn = "";
        int nIndex = 0;
        string[] strCertify = new string[3];
        //string FstrBuseChk = "";
        
        public frmEducationTargetList()
        {
            InitializeComponent();
        }

        public frmEducationTargetList(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }

        public frmEducationTargetList(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
        }

        private void frmEducationTargetList_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            Show_yn = "n";

            txtTitle.Text = "";
            txtSNAME.Text = "";

            ComFunc.ReadSysDate(clsDB.DbCon);
            dtpTDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);

            SET_BUSE();
        }

        void SET_BUSE()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strFDate = dtpFDate.Text.Trim();
            string strTDate = dtpTDate.Text.Trim();

            string strName = "";
            string strBuCode = "";

            ssBuse_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "  SELECT A.BUCODE, A.NAME, A.DEPT_ID, A.DEPT_ID_UP, C.GRP, C.ROWID";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_BUSE A, KOSMOS_ADM.INSA_MST B, KOSMOS_ADM.INSA_EDUBUSE_GROUP C";
                SQL += ComNum.VBLF + "  WHERE A.INSA = '*'";
                SQL += ComNum.VBLF + "    AND A.BUCODE = B.BUSE";
                SQL += ComNum.VBLF + "    AND A.BUCODE = C.BUCODE(+)";
                if (chkJae.Checked == true)
                {
                    SQL += ComNum.VBLF + "   AND B.KUNDAY <= TO_DATE('" + strTDate + "', 'YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "   AND (B.TOIDAY >= TO_DATE('" + strFDate + "', 'YYYY-MM-DD') OR B.TOIDAY IS NULL)";
                }
                else
                {
                    SQL += ComNum.VBLF + " AND   KUNDAY >= TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + " AND   KUNDAY <= TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                }

                if (mPara1 == "NRSTD")
                {
                    SQL = SQL + ComNum.VBLF + " AND A.BUCODE IN (";
                    SQL = SQL + ComNum.VBLF + "       SELECT MATCH_CODE ";
                    SQL = SQL + ComNum.VBLF + "         FROM KOSMOS_PMPA.NUR_CODE ";
                    SQL = SQL + ComNum.VBLF + "        WHERE GUBUN = '2' AND GBUSE='Y' ";
                    SQL = SQL + ComNum.VBLF + "          AND MATCH_CODE IS NOT NULL) ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " AND   SUBSTR(A.BUCODE, 1, 4) <> '0992' ";   //'용역 부서 조회 안되게
                }

                SQL += ComNum.VBLF + "  GROUP BY A.BUCODE, A.NAME, A.DEPT_ID, A.DEPT_ID_UP, C.GRP, C.ROWID";
                SQL += ComNum.VBLF + "  ORDER BY C.GRP DESC, A.BUCODE, A.NAME";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

                ssBuse_Sheet1.RowCount = dt.Rows.Count;
                ssBuse_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strName = dt.Rows[i]["Name"].ToString().Trim();
                    strBuCode = dt.Rows[i]["Bucode"].ToString().Trim();

                    cboFrbuse.Items.Add(strName + VB.Space(20) + strBuCode);
                    cboToBuse.Items.Add(strName + VB.Space(20) + strBuCode);

                    ssBuse_Sheet1.Cells[i, 1].Text = strName;
                    ssBuse_Sheet1.Cells[i, 2].Text = strBuCode;
                    ssBuse_Sheet1.Cells[i, 3].Text = dt.Rows[i]["GRP"].ToString().Trim();
                    ssBuse_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                cboFrbuse.SelectedIndex = 0;
                cboToBuse.SelectedIndex = 0;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            GetSearchData();
        }

        void GetSearchData()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int i = 0;
            int j = 0;
            string strBuse = "";
            string strGroup = "";
            string strFDate = dtpFDate.Text.Trim();
            string strTDate = dtpTDate.Text.Trim();

            vaSpread1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                for (i = 0; i < ssBuse_Sheet1.RowCount; i++)
                {
                    if (ssBuse_Sheet1.Cells[i, 0].Value != null && (bool)ssBuse_Sheet1.Cells[i, 0].Value == true)
                    {
                        strGroup = ssBuse_Sheet1.Cells[i, 3].Text.Trim();
                        if (strGroup != "")
                        {
                            strBuse = "";
                            for (j = i; j < ssBuse_Sheet1.RowCount; j++)
                            {
                                if (ssBuse_Sheet1.Cells[j, 3].Text.Trim() == strGroup)
                                {
                                    strBuse += "'" + ssBuse_Sheet1.Cells[j, 2].Text.Trim() + "', ";
                                }
                                else
                                {
                                    i = j - 1;
                                    break;
                                }
                            }
                            if (j >= ssBuse_Sheet1.RowCount-1)
                            {
                                i = j - 1;
                            }
                            strBuse = VB.Mid(strBuse, 1, strBuse.Length - 2);
                        }
                        else
                        {
                            strBuse = ssBuse_Sheet1.Cells[i, 2].Text.Trim();
                        }

                        SQL = " SELECT KOSMOS_PMPA.BAS_BUSE.Name BuseName, Sabun, KorName, Buse, C.Name JikName, A.JIKJONG, ";
                        SQL += ComNum.VBLF + " TO_CHAR(IpsaDay,'YYYY-MM-DD') IpsaDay, TO_CHAR(BalDay,'YYYY-MM-DD') BalDay, ";
                        SQL += ComNum.VBLF + " ZipCode1, ZipCode2, Juso, Tel, SEX, A.JUMIN3 AJUMIN3, A.BIRTHDAY ";
                        SQL += ComNum.VBLF + " FROM  KOSMOS_ADM.INSA_MST A, KOSMOS_ADM.INSA_CODE C, KOSMOS_PMPA.BAS_BUSE ";
                        SQL += ComNum.VBLF + " WHERE Buse = Bucode(+) ";
                        SQL += ComNum.VBLF + " AND   Jik = C.Code(+) ";
                        SQL += ComNum.VBLF + " AND   C.Gubun = '2' ";
                        SQL += ComNum.VBLF + " AND   Buse IN (" + strBuse + ") ";
                        if (chkJae.Checked == true) //기간내 재직자
                        {
                            SQL += ComNum.VBLF + "   AND A.KUNDAY <= TO_DATE('" + strTDate + "', 'YYYY-MM-DD')";
                            SQL += ComNum.VBLF + "   AND (A.TOIDAY >= TO_DATE('" + strFDate + "', 'YYYY-MM-DD') OR A.TOIDAY IS NULL)";
                        }
                        else
                        {
                            SQL += ComNum.VBLF + " AND   KUNDAY >= TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                            SQL += ComNum.VBLF + " AND   KUNDAY <= TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                        }

                        if (rdoJikwon0.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND A.Sabun <= '600000' ";
                        }
                        else if (rdoJikwon1.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND A.Sabun >= '800001' ";
                            SQL += ComNum.VBLF + " AND   Substr(A.Buse, 1, 4) <> '0992' "; //'용역직원 제외
                        }
                        else
                        {
                            SQL += ComNum.VBLF + " AND   Substr(A.Buse, 1, 4) <> '0992' "; //'용역직원 제외
                        }

                        if (chkToi.Checked == false && chkJae.Checked == false)
                        {
                            SQL += ComNum.VBLF + " AND   Toiday is null ";
                        }

                        if (rdoSex0.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND SEX = '1' ";
                        }
                        else if (rdoSex1.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND SEX = '2' ";
                        }

                        switch (nIndex)
                        {
                            case 0:
                                SQL += ComNum.VBLF + " ORDER BY BAS_BUSE.REPORT_ARRAY, Buse, C.SORT, A.Jik, Sabun ";
                                break;
                            case 1:
                                SQL += ComNum.VBLF + " ORDER BY Jik, Buse, Sabun ";
                                break;
                            case 2:
                                SQL += ComNum.VBLF + " ORDER BY Sabun ";
                                break;
                            case 3:
                                SQL += ComNum.VBLF + " ORDER BY A.BIRTHDAY ";
                                break;
                            case 4:
                                SQL += ComNum.VBLF + " ORDER BY KorName ";
                                break;
                            case 5:
                                SQL += ComNum.VBLF + " ORDER BY JIKJONG, BAS_BUSE.REPORT_ARRAY, BUSE, C.SORT, JIK, SABUN  ";
                                break;
                        }

                        vaSpread1_Sheet1.RowCount = 0;

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            for (j = 0; j < dt.Rows.Count; j++)
                            {
                                vaSpread1_Sheet1.RowCount = vaSpread1_Sheet1.RowCount + 1;
                                vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 0].Text =
                                    dt.Rows[j]["BuseName"].ToString();
                                vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 1].Text =
                                    SABUN_ASTA(dt.Rows[j]["Sabun"].ToString(), chkASTA.Checked == true ? "1" : "0");
                                vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 2].Text =
                                    dt.Rows[j]["KorName"].ToString().Trim();
                                vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 3].Text =
                                    dt.Rows[j]["JikName"].ToString();
                                vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 4].Text =
                                    READ_JIKJONG(dt.Rows[j]["JIKJONG"].ToString());
                                if (dt.Rows[j]["SEX"].ToString() == "1")
                                {
                                    vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 5].Text = "남";
                                }
                                else
                                {
                                    vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 5].Text = "여";
                                }
                                vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 6].Text = " ";
                                vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 7].Text = " ";
                            }
                        }
                        dt.Dispose();
                        dt = null;
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        string READ_JIKJONG(string arg)
        {
            string returnVal = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT NAME FROM KOSMOS_ADM.INSA_CODE ";
                SQL += ComNum.VBLF + " WHERE GUBUN = '3' ";
                SQL += ComNum.VBLF + "  AND CODE = '" + arg + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return returnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return returnVal;
                }

                returnVal = dt.Rows[0]["NAME"].ToString().Trim();
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return returnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return returnVal;
            }
        }

        string SABUN_ASTA(string arg, string arg2)
        {
            string returnVal = "";
            if (arg2 == "1")
            {
                arg = arg.Trim();
                returnVal = VB.Mid(arg, 1, arg.Length - 2) + "**";
            }
            else
            {
                returnVal = arg;
            }
            return returnVal;
        }


        private void btnPrint_Click(object sender, EventArgs e)
        {
            SET_PRINT();
        }

        void SET_PRINT()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int i = 0;
            int j = 0;
            string strBuse = "";
            string strGroup = "";
            string strFDate = dtpFDate.Text.Trim();
            string strTDate = dtpTDate.Text.Trim();
            //string strBuChk1 = ""; //'관리과
            //string strBuChk2 = ""; //'원무과

            vaSpread1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (chkGubun.Checked == true)
                {

                    for (i = 0; i < ssBuse_Sheet1.RowCount; i++)
                    {
                        if (ssBuse_Sheet1.Cells[i, 0].Value != null && (bool)ssBuse_Sheet1.Cells[i, 0].Value == true)
                        {
                            strGroup = ssBuse_Sheet1.Cells[i, 3].Text.Trim();
                            if (strGroup != "")
                            {
                                strBuse = "";
                                for (j = i; j < ssBuse_Sheet1.RowCount; j++)
                                {
                                    if (ssBuse_Sheet1.Cells[j, 3].Text.Trim() == strGroup)
                                    {
                                        strBuse += "'" + ssBuse_Sheet1.Cells[j, 2].Text.Trim() + "', ";
                                    }
                                    else
                                    {
                                        i = j - 1;
                                        break;
                                    }
                                }
                                if (j >= ssBuse_Sheet1.RowCount-1)
                                {
                                    i = j - 1;
                                }
                                strBuse = VB.Mid(strBuse, 1, strBuse.Length - 2);
                            }
                            else
                            {
                                strBuse = ssBuse_Sheet1.Cells[i, 2].Text.Trim();
                            }

                            SQL = " SELECT KOSMOS_PMPA.BAS_BUSE.Name BuseName, Sabun, KorName, Buse, C.Name JikName, A.JIKJONG, ";
                            SQL += ComNum.VBLF + " TO_CHAR(IpsaDay,'YYYY-MM-DD') IpsaDay, TO_CHAR(BalDay,'YYYY-MM-DD') BalDay, ";
                            SQL += ComNum.VBLF + " ZipCode1, ZipCode2, Juso, Tel, SEX, A.JUMIN3 AJUMIN3, A.BIRTHDAY ";
                            SQL += ComNum.VBLF + " FROM  KOSMOS_ADM.INSA_MST A, KOSMOS_ADM.INSA_CODE C, KOSMOS_PMPA.BAS_BUSE ";
                            SQL += ComNum.VBLF + " WHERE Buse = Bucode(+) ";
                            SQL += ComNum.VBLF + " AND   Jik = C.Code(+) ";
                            SQL += ComNum.VBLF + " AND   C.Gubun = '2' ";
                            SQL += ComNum.VBLF + " AND   Buse IN (" + strBuse + ") ";
                            if (chkJae.Checked == true) //기간내 재직자
                            {
                                SQL += ComNum.VBLF + "   AND A.KUNDAY <= TO_DATE('" + strTDate + "', 'YYYY-MM-DD')";
                                SQL += ComNum.VBLF + "   AND (A.TOIDAY >= TO_DATE('" + strFDate + "', 'YYYY-MM-DD') OR A.TOIDAY IS NULL)";
                            }
                            else
                            {
                                SQL += ComNum.VBLF + " AND   KUNDAY >= TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                                SQL += ComNum.VBLF + " AND   KUNDAY <= TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                            }

                            if (rdoJikwon0.Checked == true)
                            {
                                SQL += ComNum.VBLF + " AND A.Sabun <= '600000' ";
                            }
                            else if (rdoJikwon1.Checked == true)
                            {
                                SQL += ComNum.VBLF + " AND A.Sabun >= '800001' ";
                                SQL += ComNum.VBLF + " AND   Substr(A.Buse, 1, 4) <> '0992' "; //'용역직원 제외
                            }
                            else
                            {
                                SQL += ComNum.VBLF + " AND   Substr(A.Buse, 1, 4) <> '0992' "; //'용역직원 제외
                            }

                            if (chkToi.Checked == false && chkJae.Checked == false)
                            {
                                SQL += ComNum.VBLF + " AND   Toiday is null ";
                            }

                            if (rdoSex0.Checked == true)
                            {
                                SQL += ComNum.VBLF + " AND SEX = '1' ";
                            }
                            else if (rdoSex1.Checked == true)
                            {
                                SQL += ComNum.VBLF + " AND SEX = '2' ";
                            }

                            switch (nIndex)
                            {
                                case 0:
                                    SQL += ComNum.VBLF + " ORDER BY BAS_BUSE.REPORT_ARRAY, Buse, C.SORT, A.Jik, Sabun ";
                                    break;
                                case 1:
                                    SQL += ComNum.VBLF + " ORDER BY Jik, Buse, Sabun ";
                                    break;
                                case 2:
                                    SQL += ComNum.VBLF + " ORDER BY Sabun ";
                                    break;
                                case 3:
                                    SQL += ComNum.VBLF + " ORDER BY A.BIRTHDAY ";
                                    break;
                                case 4:
                                    SQL += ComNum.VBLF + " ORDER BY KorName ";
                                    break;
                                case 5:
                                    SQL += ComNum.VBLF + " ORDER BY JIKJONG, BAS_BUSE.REPORT_ARRAY, BUSE, C.SORT, JIK, SABUN  ";
                                    break;
                            }

                            vaSpread1_Sheet1.RowCount = 0;

                            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            if (dt.Rows.Count > 0)
                            {
                                for (j = 0; j < dt.Rows.Count; j++)
                                {
                                    vaSpread1_Sheet1.RowCount = vaSpread1_Sheet1.RowCount + 1;
                                    vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 0].Text =
                                        dt.Rows[j]["BuseName"].ToString();
                                    vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 1].Text =
                                        SABUN_ASTA(dt.Rows[j]["Sabun"].ToString(), chkASTA.Checked == true ? "1" : "0");
                                    vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 2].Text =
                                        dt.Rows[j]["KorName"].ToString().Trim();
                                    vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 3].Text =
                                        dt.Rows[j]["JikName"].ToString();
                                    vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 4].Text =
                                        READ_JIKJONG(dt.Rows[j]["JIKJONG"].ToString());
                                    if (dt.Rows[j]["SEX"].ToString() == "1")
                                    {
                                        vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 5].Text = "남";
                                    }
                                    else
                                    {
                                        vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 5].Text = "여";
                                    }
                                    vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 6].Text = " ";
                                    vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 7].Text = " ";

                                    if (chkPrt.Checked == true)
                                    {
                                        Show_yn = "y";
                                        Print_Sheet();
                                        vaSpread1_Sheet1.RowCount = 0;
                                    }
                                }
                            }

                            if (chkPrt.Checked == false)
                            {
                                Show_yn = "y";
                                if (dt.Rows.Count > 0)
                                {
                                    Print_Sheet();
                                }
                                vaSpread1_Sheet1.RowCount = 0;
                            }

                            dt.Dispose();
                            dt = null;

                        }
                    }
                }
                else
                {
                    for (i = 0; i < ssBuse_Sheet1.RowCount; i++)
                    {
                        if (ssBuse_Sheet1.Cells[i, 0].Value != null && (bool)ssBuse_Sheet1.Cells[i, 0].Value == true)
                        {
                            strBuse += "'" + ssBuse_Sheet1.Cells[j, 2].Text.Trim() + "', ";
                        }
                    }

                    if (strBuse == "")
                    {
                        ComFunc.MsgBox("부서를 선택하십시오.");
                        return;
                    }

                    strBuse = VB.Mid(strBuse, 1, strBuse.Length - 2);


                    SQL = " SELECT KOSMOS_PMPA.BAS_BUSE.Name BuseName, Sabun, KorName, Buse, C.Name JikName, A.JIKJONG, ";
                    SQL += ComNum.VBLF + " TO_CHAR(IpsaDay,'YYYY-MM-DD') IpsaDay, TO_CHAR(BalDay,'YYYY-MM-DD') BalDay, ";
                    SQL += ComNum.VBLF + " ZipCode1, ZipCode2, Juso, Tel, SEX, A.JUMIN3 AJUMIN3, A.BIRTHDAY ";
                    SQL += ComNum.VBLF + " FROM  KOSMOS_ADM.INSA_MST A, KOSMOS_ADM.INSA_CODE C, KOSMOS_PMPA.BAS_BUSE ";
                    SQL += ComNum.VBLF + " WHERE Buse = Bucode(+) ";
                    SQL += ComNum.VBLF + " AND   Jik = C.Code(+) ";
                    SQL += ComNum.VBLF + " AND   C.Gubun = '2' ";
                    SQL += ComNum.VBLF + " AND   Buse IN (" + strBuse + ") ";
                    if (chkJae.Checked == true) //기간내 재직자
                    {
                        SQL += ComNum.VBLF + "   AND A.KUNDAY <= TO_DATE('" + strTDate + "', 'YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "   AND (A.TOIDAY >= TO_DATE('" + strFDate + "', 'YYYY-MM-DD') OR A.TOIDAY IS NULL)";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + " AND   KUNDAY >= TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + " AND   KUNDAY <= TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                    }

                    if (rdoJikwon0.Checked == true)
                    {
                        SQL += ComNum.VBLF + " AND A.Sabun <= '600000' ";
                    }
                    else if (rdoJikwon1.Checked == true)
                    {
                        SQL += ComNum.VBLF + " AND A.Sabun >= '800001' ";
                        SQL += ComNum.VBLF + " AND   Substr(A.Buse, 1, 4) <> '0992' "; //'용역직원 제외
                    }
                    else
                    {
                        SQL += ComNum.VBLF + " AND   Substr(A.Buse, 1, 4) <> '0992' "; //'용역직원 제외
                    }

                    if (chkToi.Checked == false && chkJae.Checked == false)
                    {
                        SQL += ComNum.VBLF + " AND   Toiday is null ";
                    }

                    if (rdoSex0.Checked == true)
                    {
                        SQL += ComNum.VBLF + " AND SEX = '1' ";
                    }
                    else if (rdoSex1.Checked == true)
                    {
                        SQL += ComNum.VBLF + " AND SEX = '2' ";
                    }

                    switch (nIndex)
                    {
                        case 0:
                            SQL += ComNum.VBLF + " ORDER BY BAS_BUSE.REPORT_ARRAY, Buse, C.SORT, A.Jik, Sabun ";
                            break;
                        case 1:
                            SQL += ComNum.VBLF + " ORDER BY Jik, Buse, Sabun ";
                            break;
                        case 2:
                            SQL += ComNum.VBLF + " ORDER BY Sabun ";
                            break;
                        case 3:
                            SQL += ComNum.VBLF + " ORDER BY A.BIRTHDAY ";
                            break;
                        case 4:
                            SQL += ComNum.VBLF + " ORDER BY KorName ";
                            break;
                        case 5:
                            SQL += ComNum.VBLF + " ORDER BY JIKJONG, BAS_BUSE.REPORT_ARRAY, BUSE, C.SORT, JIK, SABUN  ";
                            break;
                    }

                    vaSpread1_Sheet1.RowCount = 0;

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (j = 0; j < dt.Rows.Count; j++)
                        {
                            vaSpread1_Sheet1.RowCount = vaSpread1_Sheet1.RowCount + 1;
                            vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 0].Text =
                                dt.Rows[j]["BuseName"].ToString();
                            vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 1].Text =
                                SABUN_ASTA(dt.Rows[j]["Sabun"].ToString(), chkASTA.Checked == true ? "1" : "0");
                            vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 2].Text =
                                dt.Rows[j]["KorName"].ToString().Trim();
                            vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 3].Text =
                                dt.Rows[j]["JikName"].ToString();
                            vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 4].Text =
                                READ_JIKJONG(dt.Rows[j]["JIKJONG"].ToString());
                            if (dt.Rows[j]["SEX"].ToString() == "1")
                            {
                                vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 5].Text = "남";
                            }
                            else
                            {
                                vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 5].Text = "여";
                            }
                            vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 6].Text = " ";
                            vaSpread1_Sheet1.Cells[vaSpread1_Sheet1.RowCount - 1, 7].Text = " ";

                            if (chkPrt.Checked == true)
                            {
                                Show_yn = "y";
                                Print_Sheet();
                                vaSpread1_Sheet1.RowCount = 0;
                            }
                        }
                    }

                    if (chkPrt.Checked == false)
                    {
                        Show_yn = "y";
                        if (dt.Rows.Count > 0)
                        {
                            Print_Sheet();
                        }
                        vaSpread1_Sheet1.RowCount = 0;
                    }

                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }

        void Print_Sheet()
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return; //권한 확인

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (Show_yn == "n")
            {
                GetSearchData();
                if (Show_yn == "n")
                    return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = txtTitle.Text.Trim() + ComNum.VBLF;

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            if (dtpDate.Checked == true)
            {
                strHeader += CS.setSpdPrint_String("◈ 교육일시: " + dtpDate.Text.Trim(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }

            if (txtSNAME.Text.Trim() != "")
            {
                strHeader += CS.setSpdPrint_String("◈ 강 사 명 : " + txtSNAME.Text.Trim(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 60, 30, 30, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, false, false, false, 1.2f);

            CS.setSpdPrint(vaSpread1, PrePrint, setMargin, setOption, strHeader, strFooter);

            Cursor.Current = Cursors.Default;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            int i = 0;
            for (i = 0; i < ssBuse_Sheet1.RowCount; i++)
            {
                ssBuse_Sheet1.Cells[i, 0].Value = true;
            }
        }

        private void btnDisAll_Click(object sender, EventArgs e)
        {
            int i = 0;
            for (i = 0; i < ssBuse_Sheet1.RowCount; i++)
            {
                ssBuse_Sheet1.Cells[i, 0].Value = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                return; //권한 확인

            if (Save_Data() == false)
                return;

            SET_BUSE();
        }

        bool Save_Data()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string strROWID = "";
            string strGRP = "";
            string strBuse = "";

            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssBuse_Sheet1.RowCount; i++)
                {
                    strBuse = ssBuse_Sheet1.Cells[i, 2].Text.Trim();
                    strGRP = ssBuse_Sheet1.Cells[i, 3].Text.Trim();
                    strROWID = ssBuse_Sheet1.Cells[i, 4].Text.Trim();

                    if (strROWID != "")
                    {
                        SQL = " UPDATE KOSMOS_ADM.INSA_EDUBUSE_GROUP ";
                        SQL += ComNum.VBLF + " SET GRP = '" + strGRP + "'";
                        SQL += ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";
                    }
                    else
                    {
                        SQL = " INSERT INTO KOSMOS_ADM.INSA_EDUBUSE_GROUP(BUCODE, GRP) VALUES (";
                        SQL += ComNum.VBLF + "'" + strBuse + "','" + strGRP + "') ";
                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
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
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void rdoSort0_CheckedChanged(object sender, EventArgs e)
        {
            Show_yn = "n";
            nIndex = (int)VB.Val(VB.Right(((RadioButton)sender).Name, 1));
        }

        private void dtpFDate_ValueChanged(object sender, EventArgs e)
        {
            SET_BUSE();
        }

        private void dtpTDate_ValueChanged(object sender, EventArgs e)
        {
            SET_BUSE();
        }

        private void chkJae_CheckedChanged(object sender, EventArgs e)
        {
            if (chkJae.Checked == true)
            {
                dtpTDate.Enabled = false;
            }
            else
            {
                dtpTDate.Enabled = true;
            }
            SET_BUSE();
        }

        private void chkJikjong_CheckedChanged(object sender, EventArgs e)
        {
            if (chkJikjong.Checked == true)
            {
                vaSpread1_Sheet1.Columns[4].Visible = true;

            }
            else
            {
                vaSpread1_Sheet1.Columns[4].Visible = false;
            }
        }
    }
}
