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
using ComDbB;
using ComNurLibB;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedIpdNr
    /// File Name       : FrmCommonKnowList.cs
    /// Description     : 간호부 공지사항
    /// Author          : 안정수
    /// Create Date     : 2018-02-26
    /// Update History  : 
    /// TODO : FrmCommonKnowViewInfect 폼 구현 필요
    /// </summary>
    /// <history>  
    /// 기존 FrmCommonKnowList.frm(FrmCommonKnowList) 폼 FrmCommonKnowList.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\mtsEmr\CADEX\FrmCommonKnowList.frm(FrmCommonKnowList) >> FrmCommonKnowList.cs 폼이름 재정의" />
    public partial class frmCommonKnowList : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsIpdNr CIN = new clsIpdNr();

        #endregion


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

        public frmCommonKnowList(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmCommonKnowList()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnClose.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);

            this.txtSearch.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            //this.eControl.LostFocus += new EventHandler(eControl_LostFocus);
            //this.eControl.GotFocus += new EventHandler(eControl_GotFocus);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                ComFunc.ReadSysDate(clsDB.DbCon);

                Set_Init();
            }
        }

        void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }

            else if (sender == this.btnClose)
            {
                //본사이즈
                //this.Height = 619; 
                this.Height = 411;
                btnClose.Visible = false;
            }

            else if (sender == this.btnSearch)
            {
                eGetData();
            }
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtSearch)
            {
                if (e.KeyChar == 13)
                {
                    eGetData();
                }
            }
        }

        void Set_Init()
        {
            int i = 0;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            this.Height = 411;
            btnClose.Visible = false;

            ReadCKNOW();


            try
            {

                ssDrug.ActiveSheet.Rows.Count = 0;

                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  CDATE, GUBUN";
                SQL += ComNum.VBLF + "FROM( ";
                SQL += ComNum.VBLF + "      SELECT TO_CHAR(WRITEDATE, 'YYYY-MM-DD') CDATE, '신약사용안내' GUBUN";
                SQL += ComNum.VBLF + "      FROM KOSMOS_ADM.DRUG_INFO01";
                SQL += ComNum.VBLF + "      WHERE (CDATE >= TRUNC(SYSDATE) OR CDATE IS NULL)";
                SQL += ComNum.VBLF + "      UNION ALL";
                SQL += ComNum.VBLF + "      SELECT TO_CHAR(WRITEDATE, 'YYYY-MM-DD') CDATE, '사용중단약품안내' GUBUN";
                SQL += ComNum.VBLF + "      FROM KOSMOS_ADM.DRUG_INFO02";
                SQL += ComNum.VBLF + "      WHERE (CDATE >= TRUNC(SYSDATE) OR CDATE IS NULL)";
                SQL += ComNum.VBLF + "      UNION ALL";
                SQL += ComNum.VBLF + "      SELECT TO_CHAR(WRITEDATE, 'YYYY-MM-DD') CDATE, '용도변경약품안내' GUBUN";
                SQL += ComNum.VBLF + "      FROM KOSMOS_ADM.DRUG_INFO04";
                SQL += ComNum.VBLF + "      WHERE (CDATE >= TRUNC(SYSDATE) OR CDATE IS NULL)";
                SQL += ComNum.VBLF + "      UNION ALL";
                SQL += ComNum.VBLF + "      SELECT TO_CHAR(WRITEDATE, 'YYYY-MM-DD') CDATE, '약품코드변경안내' GUBUN";
                SQL += ComNum.VBLF + "      FROM KOSMOS_ADM.DRUG_INFO03";
                SQL += ComNum.VBLF + "      WHERE (CDATE >= TRUNC(SYSDATE) OR CDATE IS NULL))";
                SQL += ComNum.VBLF + "GROUP BY CDATE, GUBUN";
                SQL += ComNum.VBLF + "ORDER BY CDATE DESC, GUBUN";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssDrug.ActiveSheet.Rows.Count = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssDrug.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["CDATE"].ToString().Trim();
                        ssDrug.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
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
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        void eGetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            if (txtSearch.Text.Trim() == "")
            {
                ReadCKNOW();
            }

            else
            {
                ReadCKNOW("검색");
            }
        }

        void ReadCKNOW(string arg = "")
        {
            int i = 0;
            int j = 0;
            string strIPSADAY = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  IPSADAY";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST";
                SQL += ComNum.VBLF + "WHERE SABUN = '" + ComFunc.SetAutoZero(clsType.User.Sabun, 5) + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strIPSADAY = dt.Rows[0]["IPSADAY"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                ssList0.ActiveSheet.Rows.Count = 0;
                ssList1.ActiveSheet.Rows.Count = 0;

                for (j = 0; j <= 1; j++)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  /*+rule*/  A.WRTNO, TO_CHAR(A.CDATE, 'YYYY-MM-DD') CDATE, A.BUCODE, A.TITLE, A.DELDATE, A.REPEAT, A.IMPORTANCE, B.SABUN, A.PHARMACY, '간호' GUBUN";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMR_CADEX_CKNOW A, (";
                    SQL += ComNum.VBLF + "                                                  SELECT WRTNO, SABUN";
                    SQL += ComNum.VBLF + "                                                  FROM KOSMOS_EMR.EMR_CADEX_CKNOW_CLICK";
                    SQL += ComNum.VBLF + "                                                  WHERE SABUN = " + clsType.User.Sabun;
                    SQL += ComNum.VBLF + "                                                  GROUP BY WRTNO, SABUN ) B";
                    SQL += ComNum.VBLF + "WHERE 1=1";

                    if (rdoNurse.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND PHARMACY IS NULL  ";
                    }
                    else if (rdoSd.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND PHARMACY = '1' AND PH_SIGN1_SABUN IS NOT NULL ";
                    }

                    if (arg == "검색")
                    {
                        SQL += ComNum.VBLF + "  AND UPPER(TITLE) LIKE '%" + txtSearch.Text.Trim().ToUpper() + "%' ";
                    }

                    else
                    {
                        SQL += ComNum.VBLF + "  AND (A.DELDATE >= TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') OR A.DELDATE IS NULL)";
                    }


                    SQL += ComNum.VBLF + "      AND A.WRTNO = B.WRTNO(+)";

                    if (j == 1)
                    {
                        SQL += ComNum.VBLF + "  AND A.CDATE >= TO_DATE('" + VB.Left(strIPSADAY, 10) + "','YYYY-MM-DD')";
                    }

                    if (j == 0)
                    {
                        SQL += ComNum.VBLF + "  AND A.BUCODE = '033100'";
                    }

                    else if (j == 1)
                    {
                        if (clsType.User.BuseCode == "033109" || clsType.User.BuseCode == "100490")
                        {
                            SQL += ComNum.VBLF + "AND A.BUCODE IN ('033109','100490') ";
                        }
                        else if (clsType.User.BuseCode != "")
                        {
                            SQL += ComNum.VBLF + "AND A.BUCODE = '" + clsType.User.BuseCode + "'";
                        }
                    }

                    SQL += ComNum.VBLF + "      AND (PHARMACY IS NULL OR (PHARMACY = '1' AND PH_SIGN1_SABUN IS NOT NULL))";
                    SQL += ComNum.VBLF + "      AND A.TRUNCDATE IS NULL";

                    if (j == 0)
                    {
                        SQL += ComNum.VBLF + "UNION ALL";
                        SQL += ComNum.VBLF + "SELECT ";
                        SQL += ComNum.VBLF + "  TO_NUMBER(A.STRWRTNO) WRTNO, TO_CHAR(SDATE,'YYYY-MM-DD') CDATE, '101758' BUCODE,  '(감염)' || TITLE TITLE, ";
                        SQL += ComNum.VBLF + "  EDATE DELDATE, '0' REPEAT, '1' IMPORTANCE, B.SABUN, '' PHARMACY, '감염' GUBUN";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_INFECT_MSG A, ( ";
                        SQL += ComNum.VBLF + "                                              SELECT TO_NUMBER(STRWRTNO) STRWRTNO, SABUN";
                        SQL += ComNum.VBLF + "                                              FROM KOSMOS_EMR.EMR_CADEX_CKNOW_CLICK_INFECT";
                        SQL += ComNum.VBLF + "                                              WHERE SABUN = " + clsType.User.Sabun;
                        SQL += ComNum.VBLF + "                                              GROUP BY STRWRTNO, SABUN) B";
                        SQL += ComNum.VBLF + "WHERE 1=1";

                        if (arg == "검색")
                        {
                            SQL += ComNum.VBLF + "  AND UPPER(TITLE) LIKE '%" + txtSearch.Text.Trim().ToUpper() + "%' ";
                        }

                        else
                        {
                            //SQL += ComNum.VBLF + "  AND A.EDATE > TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')";
                            SQL += ComNum.VBLF + "  AND decode(A.EDATE,to_date('2999-12-31','yyyy-mm-dd'),A.SDATE + 30,A.EDATE) > TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')";

                        }
                        SQL += ComNum.VBLF + "      AND A.VIEW1 IN ('0','2')";
                        SQL += ComNum.VBLF + "      AND A.GUBUN = '0'";
                        SQL += ComNum.VBLF + "      AND A.STRWRTNO = B.STRWRTNO(+)";
                    }

                    SQL += ComNum.VBLF + "ORDER BY REPEAT DESC, IMPORTANCE DESC, CDATE DESC, WRTNO DESC";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        FarPoint.Win.Spread.SheetView[] Spd = new FarPoint.Win.Spread.SheetView[2] { ssList0.ActiveSheet, ssList1.ActiveSheet };

                        Spd[j].Rows.Count = dt.Rows.Count;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            Application.DoEvents();

                            Spd[j].Cells[i, 0].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                            Spd[j].Cells[i, 1].Text = dt.Rows[i]["CDATE"].ToString().Trim();
                            Spd[j].Cells[i, 2].Text = Read_Name(dt.Rows[i]["BUCODE"].ToString().Trim());
                            Spd[j].Cells[i, 3].Text = dt.Rows[i]["PHARMACY"].ToString().Trim() == "1" ? "(★약)" + dt.Rows[i]["TITLE"].ToString().Trim() : "" + dt.Rows[i]["TITLE"].ToString().Trim();
                            Spd[j].Cells[i, 5].Text = dt.Rows[i]["DELDATE"].ToString().Trim();
                            Spd[j].Cells[i, 8].Text = dt.Rows[i]["SABUN"].ToString().Trim() != "" ? "확인" : "미확인";
                            Spd[j].Cells[i, 9].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                            Spd[j].Cells[i, 10].Text = dt.Rows[i]["GUBUN"].ToString().Trim();

                            if (dt.Rows[i]["IMPORTANCE"].ToString().Trim() == "1")
                            {
                                Spd[j].Rows[i].ForeColor = Color.Red;
                            }

                            else if (dt.Rows[i]["REPEAT"].ToString().Trim() == "1")
                            {
                                Spd[j].Rows[i].ForeColor = Color.Red;
                            }

                            else
                            {
                                Spd[j].Rows[i].ForeColor = Color.Black;
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }

        }

        public string Read_Name(string argBuCode)
        {
            string strData = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT  ";
                SQL = SQL + ComNum.VBLF + "      NAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BUSE ";
                SQL = SQL + ComNum.VBLF + " WHERE BUCODE = '" + argBuCode + "'";


                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");

                    return "";
                }

                if (dt.Rows.Count > 0)
                {
                    strData = dt.Rows[0]["NAME"].ToString().Trim();
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

            return strData;
        }

        void ssDrug_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strDate = "";

            if (e.Row < 0)
            {
                return;
            }

            strDate = ssDrug.ActiveSheet.Cells[e.Row, 0].Text;

            switch (ssDrug.ActiveSheet.Cells[e.Row, 1].Text)
            {
                case "신약사용안내":
                    READ_SS1(strDate);
                    break;

                case "사용중단약품안내":
                    READ_SS2(strDate);
                    break;

                case "용도변경약품안내":
                    READ_SS4(strDate);
                    break;

                case "약품코드변경안내":
                    READ_SS3(strDate);
                    break;
            }

            this.Height = 619;
            btnClose.Visible = true;
        }

        void ssList0_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (e.Row < 0)
            {
                return;
            }
            
            if (ssList0.ActiveSheet.Cells[e.Row, 10].Text.Trim() == "감염")
            {
                frmCommonKnowViewInfect frmCommonKnowViewInfectX = new frmCommonKnowViewInfect(ssList0.ActiveSheet.Cells[e.Row, 9].Text.Trim());
                frmCommonKnowViewInfectX.StartPosition = FormStartPosition.CenterParent;
                frmCommonKnowViewInfectX.ShowDialog();
                frmCommonKnowViewInfectX = null;

                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "INSERT INTO KOSMOS_EMR.EMR_CADEX_CKNOW_CLICK_INFECT(STRWRTNO, SABUN, VDATE) VALUES( ";
                    SQL += ComNum.VBLF + ssList0.ActiveSheet.Cells[e.Row, 9].Text.Trim() + ", " + VB.Val(clsType.User.Sabun) + ", SYSDATE)";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    eGetData();
                }

                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }
            else
            {
                frmNurseNotice ff = new frmNurseNotice(this, ssList0.ActiveSheet.Cells[e.Row, 9].Text.Trim());
                ff.StartPosition = FormStartPosition.CenterParent;
                ff.ShowDialog();
                ff = null;

                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "INSERT INTO KOSMOS_EMR.EMR_CADEX_CKNOW_CLICK(WRTNO, SABUN, VDATE) VALUES(  ";
                    SQL += ComNum.VBLF + VB.Val(ssList0.ActiveSheet.Cells[e.Row, 9].Text.Trim()) + ", " + VB.Val(clsType.User.Sabun) + ", SYSDATE)";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    
                    clsDB.setCommitTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    eGetData();
                }

                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

            }

        }

        void ssList1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (e.Row < 0)
            {
                return;
            }

            if (ssList1.ActiveSheet.Cells[e.Row, 10].Text.Trim() == "감염")
            {
                frmCommonKnowViewInfect frmCommonKnowViewInfectX = new frmCommonKnowViewInfect(ssList1.ActiveSheet.Cells[e.Row, 9].Text.Trim());
                frmCommonKnowViewInfectX.StartPosition = FormStartPosition.CenterParent;
                frmCommonKnowViewInfectX.ShowDialog();
                frmCommonKnowViewInfectX = null;

                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "INSERT INTO KOSMOS_EMR.EMR_CADEX_CKNOW_CLICK_INFECT(STRWRTNO, SABUN, VDATE) VALUES( ";
                    SQL += ComNum.VBLF + ssList1.ActiveSheet.Cells[e.Row, 9].Text.Trim() + ", " + VB.Val(clsType.User.Sabun) + ", SYSDATE)";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                }

                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }
            else
            {
                frmNurseNotice ff = new frmNurseNotice(this, ssList1.ActiveSheet.Cells[e.Row, 9].Text.Trim());
                ff.StartPosition = FormStartPosition.CenterParent;
                ff.ShowDialog();
                ff = null;

                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "INSERT INTO KOSMOS_EMR.EMR_CADEX_CKNOW_CLICK(WRTNO, SABUN, VDATE) VALUES(  ";
                    SQL += ComNum.VBLF + VB.Val(ssList1.ActiveSheet.Cells[e.Row, 9].Text.Trim()) + ", " + VB.Val(clsType.User.Sabun) + ", SYSDATE)";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                }

                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

            }
        }

        void READ_SS1(string argDATE)
        {
            int i = 0;
            int nRead = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            ss1.ActiveSheet.Rows.Count = 0;


            try
            {

                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  JEPCODE, REMARK1, REMARK2, REMARK2_CHECK,";
                SQL += ComNum.VBLF + "  REMARK3, REMARK3_ETC, BIGO, ROWID";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_INFO01";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND WRITEDATE >= TO_DATE('" + argDATE + " 00:00','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "      AND WRITEDATE <= TO_DATE('" + argDATE + " 23:59','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "      AND (CDATE >= TRUNC(SYSDATE) OR CDATE IS NULL)";
                SQL += ComNum.VBLF + "ORDER BY WRITEDATE DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;
                    ss1.ActiveSheet.Rows.Count = nRead;

                    for (i = 0; i < nRead; i++)
                    {
                        ss1.ActiveSheet.Cells[i, 0].Text = READ_JEP_INFO(dt.Rows[i]["JEPCODE"].ToString().Trim());

                        switch (dt.Rows[i]["REMARK1"].ToString().Trim())
                        {
                            case "1":
                                ss1.ActiveSheet.Cells[i, 1].Text = "원내외혼용";
                                break;

                            case "2":
                                ss1.ActiveSheet.Cells[i, 1].Text = "원내전용";
                                break;

                            case "3":
                                ss1.ActiveSheet.Cells[i, 1].Text = "원외전용";
                                break;

                            default:
                                ss1.ActiveSheet.Cells[i, 1].Text = "";
                                break;
                        }

                        if (dt.Rows[i]["REMARK2_CHECK"].ToString().Trim() == "1")
                        {
                            ss1.ActiveSheet.Cells[i, 2].Text = "추후 알림";
                        }

                        else
                        {
                            ss1.ActiveSheet.Cells[i, 2].Text = VB.Left(dt.Rows[i]["REMARK2"].ToString().Trim(), 10);
                        }

                        switch (dt.Rows[i]["REMARK3"].ToString().Trim())
                        {
                            case "1":
                                ss1.ActiveSheet.Cells[i, 3].Text = "유사/동종약 없음";
                                break;

                            case "2":
                                ss1.ActiveSheet.Cells[i, 3].Text = "제형/품목 추가" + "\r\n" + "\r\n" + "  => 유사/동종약 : " + dt.Rows[i]["REMARK3_ETC"].ToString().Trim();
                                break;

                            case "3":
                                ss1.ActiveSheet.Cells[i, 3].Text = "기존약 대체" + "\r\n" + "\r\n" + "  => 기존약 : " + dt.Rows[i]["REMARK3_ETC"].ToString().Trim();
                                break;

                            default:
                                ss1.ActiveSheet.Cells[i, 3].Text = "";
                                break;
                        }

                        ss1.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["BIGO"].ToString().Trim();
                        ss1.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }

                    ss1.Visible = true;
                    ss2.Visible = false;
                    ss3.Visible = false;
                    ss4.Visible = false;

                    this.Height = 619;
                    btnClose.Visible = true;
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
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }

        }

        void READ_SS2(string argDATE)
        {
            int i = 0;
            int nRead = 0;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            try
            {
                ss2.ActiveSheet.Rows.Count = 0;

                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  JEPCODE, REMARK1, REMARK1_CHECK, REMARK2,";
                SQL += ComNum.VBLF + "  REMARK2_ETC, REMARK3, REMARK3_ETC, BIGO, ROWID, REMARK4 ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_INFO02";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND WRITEDATE >= TO_DATE('" + argDATE + " 00:00','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "      AND WRITEDATE <= TO_DATE('" + argDATE + " 23:59','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "      AND (CDATE >= TRUNC(SYSDATE) OR CDATE IS NULL)";
                SQL += ComNum.VBLF + "ORDER BY WRITEDATE DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;
                    ss2.ActiveSheet.Rows.Count = nRead;

                    for (i = 0; i < nRead; i++)
                    {
                        ss2.ActiveSheet.Cells[i, 0].Text = READ_JEP_INFO(dt.Rows[i]["JEPCODE"].ToString().Trim());

                        switch (dt.Rows[i]["REMARK4"].ToString().Trim())
                        {
                            case "1":
                                ss2.ActiveSheet.Cells[i, 1].Text = "원내외혼용";
                                break;

                            case "2":
                                ss2.ActiveSheet.Cells[i, 1].Text = "원내전용";
                                break;

                            case "3":
                                ss2.ActiveSheet.Cells[i, 1].Text = "원외전용";
                                break;

                            default:
                                ss2.ActiveSheet.Cells[i, 1].Text = "";
                                break;
                        }

                        if (dt.Rows[i]["REMARK1_CHECK"].ToString().Trim() == "1")
                        {
                            ss2.ActiveSheet.Cells[i, 2].Text = "추후 알림";
                        }

                        else
                        {
                            ss2.ActiveSheet.Cells[i, 2].Text = VB.Left(dt.Rows[i]["REMARK1"].ToString().Trim(), 10);
                        }

                        switch (dt.Rows[i]["REMARK2"].ToString().Trim())
                        {
                            case "1":
                                ss2.ActiveSheet.Cells[i, 3].Text = "약사위원회 결과";
                                break;

                            case "2":
                                ss2.ActiveSheet.Cells[i, 3].Text = "생산중단";
                                break;

                            case "3":
                                ss2.ActiveSheet.Cells[i, 3].Text = "일시품절";
                                break;

                            case "4":
                                ss2.ActiveSheet.Cells[i, 3].Text = "보험코드 삭제";
                                break;

                            case "5":
                                ss2.ActiveSheet.Cells[i, 3].Text = "기타 : " + dt.Rows[i]["REMARK2_ETC"].ToString().Trim();
                                break;

                            default:
                                ss2.ActiveSheet.Cells[i, 3].Text = "";
                                break;
                        }

                        switch (dt.Rows[i]["REMARK3"].ToString().Trim())
                        {
                            case "1":
                                ss2.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["REMARK3_ETC"].ToString().Trim();
                                break;

                            case "2":
                                ss2.ActiveSheet.Cells[i, 4].Text = "없음";
                                break;

                            case "3":
                                ss2.ActiveSheet.Cells[i, 4].Text = "추후 알림";
                                break;

                            default:
                                ss2.ActiveSheet.Cells[i, 4].Text = "";
                                break;
                        }

                        ss2.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["BIGO"].ToString().Trim();
                        ss2.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }

                    ss1.Visible = false;
                    ss2.Visible = true;
                    ss3.Visible = false;
                    ss4.Visible = false;

                    this.Height = 619;
                    btnClose.Visible = true;
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
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        void READ_SS3(string argDATE)
        {
            int i = 0;
            int nRead = 0;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            try
            {
                ss3.ActiveSheet.Rows.Count = 0;

                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  JEPCODE, REMARK1, REMARK2, REMARK2_CHECK,";
                SQL += ComNum.VBLF + "  REMARK3, REMARK3_ETC, BIGO, ROWID ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_INFO03";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND WRITEDATE >= TO_DATE('" + argDATE + " 00:00','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "      AND WRITEDATE <= TO_DATE('" + argDATE + " 23:59','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "      AND (CDATE >= TRUNC(SYSDATE) OR CDATE IS NULL)";
                SQL += ComNum.VBLF + "ORDER BY WRITEDATE DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;
                    ss3.ActiveSheet.Rows.Count = nRead;

                    for (i = 0; i < nRead; i++)
                    {
                        ss3.ActiveSheet.Cells[i, 0].Text = READ_JEP_INFO(dt.Rows[i]["JEPCODE"].ToString().Trim());
                        ss3.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["REMARK1"].ToString().Trim();


                        if (dt.Rows[i]["REMARK2_CHECK"].ToString().Trim() == "1")
                        {
                            ss3.ActiveSheet.Cells[i, 2].Text = "추후 알림";
                        }

                        else
                        {
                            ss3.ActiveSheet.Cells[i, 2].Text = VB.Left(dt.Rows[i]["REMARK2"].ToString().Trim(), 10);
                        }

                        switch (dt.Rows[i]["REMARK3"].ToString().Trim())
                        {
                            case "1":
                                ss3.ActiveSheet.Cells[i, 3].Text = "표준코드 변경";
                                break;

                            case "2":
                                ss3.ActiveSheet.Cells[i, 3].Text = "함량 변경";
                                break;

                            case "3":
                                ss3.ActiveSheet.Cells[i, 3].Text = "기타 : " + dt.Rows[i]["REMARK3_ETC"].ToString().Trim();
                                break;

                            default:
                                ss3.ActiveSheet.Cells[i, 3].Text = "";
                                break;
                        }

                        ss3.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["BIGO"].ToString().Trim();
                        ss3.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }

                    ss1.Visible = false;
                    ss2.Visible = false;
                    ss3.Visible = true;
                    ss4.Visible = false;


                    this.Height = 619;
                    btnClose.Visible = true;
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
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        void READ_SS4(string argDATE)
        {
            int i = 0;
            int nRead = 0;


            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            try
            {
                ss4.ActiveSheet.Rows.Count = 0;

                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  JEPCODE, REMARK1, REMARK1_CHECK, REMARK2,";
                SQL += ComNum.VBLF + "  REMARK2_ETC, REMARK3, REMARK3_ETC, BIGO, ROWID  ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_INFO04";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND WRITEDATE >= TO_DATE('" + argDATE + " 00:00','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "      AND WRITEDATE <= TO_DATE('" + argDATE + " 23:59','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "      AND (CDATE >= TRUNC(SYSDATE) OR CDATE IS NULL)";
                SQL += ComNum.VBLF + "ORDER BY WRITEDATE DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;
                    ss4.ActiveSheet.Rows.Count = nRead;

                    for (i = 0; i < nRead; i++)
                    {
                        ss4.ActiveSheet.Cells[i, 0].Text = READ_JEP_INFO(dt.Rows[i]["JEPCODE"].ToString().Trim());

                        if (dt.Rows[i]["REMARK2_CHECK"].ToString().Trim() == "1")
                        {
                            ss4.ActiveSheet.Cells[i, 1].Text = "추후 알림";
                        }

                        else
                        {
                            ss4.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["REMARK1"].ToString().Trim();
                        }

                        switch (dt.Rows[i]["REMARK2"].ToString().Trim())
                        {
                            case "1":
                                ss4.ActiveSheet.Cells[i, 2].Text = "원내외혼용";
                                break;

                            case "2":
                                ss4.ActiveSheet.Cells[i, 2].Text = "원내전용";
                                break;

                            case "3":
                                ss4.ActiveSheet.Cells[i, 2].Text = "원외전용";
                                break;

                            default:
                                ss4.ActiveSheet.Cells[i, 2].Text = "";
                                break;
                        }

                        switch (dt.Rows[i]["REMARK3"].ToString().Trim())
                        {
                            case "1":
                                ss4.ActiveSheet.Cells[i, 3].Text = "원내외혼용";
                                break;

                            case "2":
                                ss4.ActiveSheet.Cells[i, 3].Text = "원내전용";
                                break;

                            case "3":
                                ss4.ActiveSheet.Cells[i, 3].Text = "원외전용";
                                break;

                            default:
                                ss4.ActiveSheet.Cells[i, 3].Text = "";
                                break;
                        }

                        ss4.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["BIGO"].ToString().Trim();
                        ss4.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ss4.ActiveSheet.Cells[i, 6].Text = VB.Left(dt.Rows[i]["WRITEDATE"].ToString().Trim(), 10);
                    }

                    ss1.Visible = false;
                    ss2.Visible = false;
                    ss3.Visible = false;
                    ss4.Visible = true;


                    this.Height = 619;
                    btnClose.Visible = true;
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
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }

        }

        public string READ_JEP_INFO(string argJEPCODE)
        {
            string rtnVal = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            try
            {
                rtnVal = "◈약품코드:" + argJEPCODE;

                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  SUNEXT, HNAME, JEHENG, REMARK011, REMARK012, REMARK013";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUGINFO";
                SQL += ComNum.VBLF + "WHERE SUNEXT = '" + argJEPCODE + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal += "\r\n" + "\r\n" +
                        "◈약품명:" + dt.Rows[0]["HNAME"].ToString().Trim() + "\r\n" + "\r\n" +
                        "◈함량:" + dt.Rows[0]["REMARK011"].ToString().Trim() + dt.Rows[0]["REMARK012"].ToString().Trim()
                        + dt.Rows[0]["REMARK013"].ToString().Trim() + "\r\n" + "\r\n" +
                        "◈제형:" + dt.Rows[0]["JEHENG"].ToString().Trim();
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
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        private void rdoSd_CheckedChanged(object sender, EventArgs e)
        {
            eGetData();
        }


    }
}
