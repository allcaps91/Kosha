using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ErForm
    /// File Name       : frmHuDetail.cs
    /// Description     : 타병원 이송환자 세부명단
    /// Author          : 이현종
    /// Create Date     : 2018-04-19
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrer\frmHuDetail.frm(nrer18.frm) >> frmHuDetail.cs 폼이름 재정의" />
    /// 
    public partial class frmHuDetail : Form
    {
        private bool bolSort = false;

        public frmHuDetail()
        {
            InitializeComponent();
        }

        private void frmHuDetail_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ss1_Sheet1.RowCount = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            dtpDate1.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetSearchData();
            SUM_TOTAL();
        }

        void GetSearchData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            ss1_Sheet1.RowCount = 0;

            try
            {
                SQL = " SELECT TO_CHAR(a.JDATE,'MM/DD') JDate,a.PANO,a.DEPTCODE,a.DRNAME,a.SEX,a.AGE,a.SINGU,a.BI,a.WARDCODE,a.ROOM,a.STUDY,a.DISEASE,a.KTASLEVL ,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(a.INTIME,'YYYYMMDDHH24MI') INTIME  ,OUTTIME,OUTGBN,NURSE,INGBN,INCARNO,CANCELGBN,CANCELREMARK,HUSONGNAME,a.GbTrans,a.GbTransT, ";
                //'후송 체크(수용능력 확인, 응급환자 진료의뢰서,  전원동의) 0:flase 1:true
                SQL = SQL + ComNum.VBLF + " a.HUSONGSAYU , a.GBFUNERAL, a.REMARK, a.MAILCODE, a.JUSO,b.SNAME,a.HUSONG_CK, A.TRANSDIRECTOR, A.ROWID ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_ER_PATIENT a, KOSMOS_PMPA.BAS_PATIENT b";
                SQL = SQL + ComNum.VBLF + " WHERE a.JDATE >= TO_DATE('" + dtpDate.Text.Trim() + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND a.JDATE <= TO_DATE('" + dtpDate1.Text.Trim() + "' ,'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND a.OUTGBN  ='6'   ";
                SQL = SQL + ComNum.VBLF + "   AND a.PANO=b.PANO(+) ";
                SQL = SQL + ComNum.VBLF + " ORDER BY a.Jdate";
                SQL = SQL + ComNum.VBLF + "";

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

                ss1_Sheet1.RowCount = dt.Rows.Count;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1_Sheet1.Cells[i, 0].Text = (i+1).ToString();
                    ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["JDATE"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["AGE"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["KTASLEVL"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim() + "/" + dt.Rows[i]["DRNAME"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["TRANSDIRECTOR"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 15].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 7 + 1].Text = READ_HUSONGSAYU(dt.Rows[i]["pano"].ToString().Trim(),
                                                                  VB.Left(dt.Rows[i]["INTIME"].ToString().Trim(), 8),
                                                                  VB.Right(dt.Rows[i]["INTIME"].ToString().Trim(), 4));

                    if (VB.Mid(dt.Rows[i]["HUSONG_CK"].ToString().Trim(), 1, 1) == "1")
                    {
                        ss1_Sheet1.Cells[i, 8 + 1].Text = "확인";
                    }
                    else
                    {
                        ss1_Sheet1.Cells[i, 8 + 1].Text = " ";
                    }

                    ss1_Sheet1.Cells[i, 9 + 1].Text = dt.Rows[i]["HUSONGNAME"].ToString().Trim();

                    switch(dt.Rows[i]["GBTrans"].ToString().Trim())
                    {
                        case "1":
                            ss1_Sheet1.Cells[i, 10 + 1].Text = "본원";
                            break;
                        case "2":
                            ss1_Sheet1.Cells[i, 10 + 1].Text = "129";
                            break;
                    }

                    switch (dt.Rows[i]["GbTransT"].ToString().Trim())
                    {
                        case "1":
                            ss1_Sheet1.Cells[i, 11 + 1].Text = "의사";
                            break;
                        case "2":
                            ss1_Sheet1.Cells[i, 11 + 1].Text = "간호사";
                            break;
                        case "3":
                            ss1_Sheet1.Cells[i, 11 + 1].Text = "응급구조사";
                            break;
                    }

                    if (VB.Mid(dt.Rows[i]["HUSONG_CK"].ToString().Trim(), 3, 1) == "1")
                    {
                        ss1_Sheet1.Cells[i, 12 + 1].Text = "확인";
                    }
                    else
                    {
                        ss1_Sheet1.Cells[i, 12 + 1].Text = " ";
                    }

                    if (VB.Mid(dt.Rows[i]["HUSONG_CK"].ToString().Trim(), 2, 1) == "1")
                    {
                        ss1_Sheet1.Cells[i, 13 + 1].Text = "확인";
                    }
                    else
                    {
                        ss1_Sheet1.Cells[i, 13 + 1].Text = " ";
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void SUM_TOTAL()
        {
            int iTemp = 0;
            //초기화
            for (int i = 0; i < ssTotal_Sheet1.ColumnCount; i++)
            {
                ssTotal_Sheet1.Cells[0, i].Text = "0";
            }
            //합계
            for (int i = 0; i < ss1_Sheet1.Rows.Count; i++)
            {
                if (ss1_Sheet1.Cells[i, 7 + 1].Text != "")
                {
                    iTemp = (int)VB.Val(ss1_Sheet1.Cells[i, 7 + 1].Text.Split('.')[0]);
                }

                ssTotal_Sheet1.Cells[0, (iTemp % 20) - 1].Text = ((int)VB.Val(ssTotal_Sheet1.Cells[0, (iTemp % 20) - 1].Text) + 1).ToString();
            }

            for (int i = 0; i < 8; i++)
            {
                ssTotal_Sheet1.Cells[0, 8].Text = ((int)VB.Val(ssTotal_Sheet1.Cells[0, 8].Text) + (int)VB.Val(ssTotal_Sheet1.Cells[0, i].Text)).ToString();
            }
        }

        string READ_HUSONGSAYU(string ArgPano, string ArgInDate, string argINTIME)
        {
            string returnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = " SELECT ";
                SQL = SQL + ComNum.VBLF + " CODE || '.' || NAME AS NAME " ;
                SQL = SQL + ComNum.VBLF + " FROM NUR_ER_EMIHPTMI a,BAS_BCODE b " ;
                SQL = SQL + ComNum.VBLF + " WHERE SEQNO IN ( SELECT MAX(SEQNO) FROM NUR_ER_EMIHPTMI " ;
                SQL = SQL + ComNum.VBLF + "                  WHERE PTMIIDNO = '" + ArgPano + "' " ;
                SQL = SQL + ComNum.VBLF + "                    AND PTMIINDT = '" + ArgInDate + "' " ;
                SQL = SQL + ComNum.VBLF + "                    AND PTMIINTM = '" + argINTIME + "') " ;
                SQL = SQL + ComNum.VBLF + "   AND PTMIIDNO = '" + ArgPano + "' " ;
                SQL = SQL + ComNum.VBLF + "   AND PTMIINDT = '" + ArgInDate + "' " ;
                SQL = SQL + ComNum.VBLF + "   AND PTMIINTM = '" + argINTIME + "' " ;
                SQL = SQL + ComNum.VBLF + "   AND GUBUN  = 'EMI_응급진료결과'  " ;
                SQL = SQL + ComNum.VBLF + "   AND CODE  like '2%'  " ;
                SQL = SQL + ComNum.VBLF + "   AND CODE  =  ptmiemrt  " ;

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return returnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return returnVal;
                }

                returnVal = dt.Rows[0]["NAME"].ToString().Trim();

                dt.Dispose();
                dt = null;
                return returnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return returnVal;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            SETPRINT();
        }

        void SETPRINT()
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = VB.Space(35) + "타병원 이송환자 세부명단";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("system", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("인쇄일자: " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + ComNum.VBLF +
                                               "응급실장: ", new Font("system", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50 , 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, false, true, false, true, false);

            CS.setSpdPrint(ss1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ss1_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ss1, e.Column, ref bolSort, true);
                return;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            int i = 0;

            string strRowid = "";
            string strTransDirector = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                for (i = 0; i < ss1.ActiveSheet.Rows.Count; i++)
                {

                    strTransDirector = ss1.ActiveSheet.Cells[i, 7].Text.Trim();
                    strRowid = ss1.ActiveSheet.Cells[i, 15].Text.Trim();

                    SQL = " UPDATE KOSMOS_PMPA.NUR_ER_PATIENT SET TRANSDIRECTOR = '" + strTransDirector + "' ";
                    SQL += ComNum.VBLF + " WHERE ROWID = '" + strRowid + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                //clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                btnSearch.PerformClick();

                return ;
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
}
