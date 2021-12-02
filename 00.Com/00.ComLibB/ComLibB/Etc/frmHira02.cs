using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmHira02
    /// Description     : 심평원자료-연령금기
    /// Author          : 이현종
    /// Create Date     : 2018-05-23
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= " \basic\busuga\busuga.vbp(FrmHira02) >> frmHira02.cs 폼이름 재정의" />
    public partial class frmHira02 : Form
    {
        public frmHira02()
        {
            InitializeComponent();
        }

        private void frmHira02_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");
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

            string strAnn = "";

            ss1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT C.SUNEXT,  C.SUNAMEK, A.GNL_NM_CD,  A.SPC_AGE,  A.SPC_AGE_UNIT, A.ADPT_FR_DT, A.ADPT_TO_DT, A.ANNCE_DT,  TO_CHAR(D.DELDATE,'YYYYMMDD') DELDATE  , F.OFFR_MSG, F.CND_CD  ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.HIRA_TBJBD44 A ,    KOSMOS_PMPA.EDI_SUGA B ,  KOSMOS_PMPA.BAS_SUN C , KOSMOS_PMPA.BAS_SUT D, KOSMOS_PMPA.HIRA_TBDUD230 F";
                SQL += ComNum.VBLF + "   WHERE A.GNL_NM_CD =  B.SCODE";
                SQL += ComNum.VBLF + "     AND B.CODE = C.BCODE";
                SQL += ComNum.VBLF + "     AND A.ADPT_FR_DT <= '" + ComQuery.CurrentDateTime(clsDB.DbCon, "D") + "' ";
                SQL += ComNum.VBLF + "     AND A.ADPT_TYPE = '0' ";
                SQL += ComNum.VBLF + "     AND C.SUNEXT =D.SUNEXT ";
                SQL += ComNum.VBLF + "     AND  B.CODE NOT IN  ";
                SQL += ComNum.VBLF + "          (SELECT B.MEDC_CD ";
                SQL += ComNum.VBLF + "          FROM KOSMOS_PMPA.HIRA_TBJBD47 B ";
                SQL += ComNum.VBLF + "          WHERE B.GNL_NM_CD = A.GNL_NM_CD)";
                SQL += ComNum.VBLF + "     AND A.SPC_AGE_UNIT || COALESCE(A.AGE_PRS_CND_CD,'') = F.INFM_CD";
                SQL += ComNum.VBLF + "     AND F.EXM_KND_CD = '01'";



                if(chkOPD.Checked == true)
                {
                    SQL += ComNum.VBLF + "   AND D.DELDATE IS NULL ";
                    SQL += ComNum.VBLF + "   AND A.ADPT_TO_DT ='99991231'";
                }

                if (rdoGB1.Checked == true)
                {
                    SQL += ComNum.VBLF + " AND F.CND_CD = '>='";
                }
                if (rdoGB2.Checked == true)
                {
                    SQL += ComNum.VBLF + " AND F.CND_CD = '<='";
                }
                if (rdoGB3.Checked == true)
                {
                    SQL += ComNum.VBLF + " AND F.CND_CD = '<'";
                }

                SQL += ComNum.VBLF + "   ORDER BY ANNCE_DT DESC , C.SUNEXT, A.ADPT_FR_DT DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
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

                for(int i =0; i < dt.Rows.Count; i++)
                {
                    if(strAnn != dt.Rows[i]["ANNCE_DT"].ToString().Trim())
                    {
                        clsSpread.gSpreadLineBoder(ss1, i, 0, i, ss1_Sheet1.ColumnCount - 1, Color.Blue, 1, false, false, false, true);
                        strAnn = dt.Rows[i]["ANNCE_DT"].ToString().Trim();
                    }


                    ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ANNCE_DT"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["GNL_NM_CD"].ToString().Trim();

                    ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SPC_AGE"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["OFFR_MSG"].ToString().Trim() + " " + dt.Rows[i]["CND_CD"].ToString().Trim();


                    ss1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ADPT_FR_DT"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ADPT_TO_DT"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 8].Text = ComFunc.FormatStrToDateTime(dt.Rows[i]["DelDate"].ToString().Trim(), "D");
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            Set_Print();
        }

        void Set_Print()
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            //'자료를 인쇄
            strTitle = "심평원자료- 연령금기 약제 LIST";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력 일자 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + VB.Space(82) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 30, 180, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, true, false, false);

            CS.setSpdPrint(ss1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            if (Set_Copy() == false) return;

            ComFunc.MsgBox("등록 완료");
        }

        bool Set_Copy()
        {
            //'항상 심평원과 1대일 동기화

            //기존자료 삭제
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            string strSuCode = "";
            string strFieldA = "";
            string strFieldB = "";
            string strDate = "";

            int i = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //기존자료 삭제
                SQL = "DELETE KOSMOS_PMPA.BAS_MSELF WHERE GUBUNA = '0' AND GUBUNB = '9' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                //'등록할 자료 읽기
                SQL = " SELECT C.SUNEXT,  C.SUNAMEK, A.GNL_NM_CD,  A.SPC_AGE,  A.SPC_AGE_UNIT, A.ADPT_FR_DT, A.ADPT_TO_DT, TO_CHAR(D.DELDATE,'YYYYMMDD') DELDATE  , F.OFFR_MSG, F.CND_CD  ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.HIRA_TBJBD44 A ,    KOSMOS_PMPA.EDI_SUGA B ,  KOSMOS_PMPA.BAS_SUN C , KOSMOS_PMPA.BAS_SUT D, KOSMOS_PMPA.HIRA_TBDUD230 F";
                SQL += ComNum.VBLF + "   WHERE A.GNL_NM_CD =  B.SCODE";
                SQL += ComNum.VBLF + "     AND B.CODE = C.BCODE";
                SQL += ComNum.VBLF + "     AND A.ADPT_FR_DT <= '" + ComQuery.CurrentDateTime(clsDB.DbCon, "D") + "' ";
                SQL += ComNum.VBLF + "     AND A.ADPT_TYPE = '0' "; 
                SQL += ComNum.VBLF + "     AND C.SUNEXT =D.SUNEXT ";
                SQL += ComNum.VBLF + "     AND  B.CODE NOT IN  ";
                SQL += ComNum.VBLF + "          (SELECT B.MEDC_CD ";
                SQL += ComNum.VBLF + "          FROM KOSMOS_PMPA.HIRA_TBJBD47 B ";
                SQL += ComNum.VBLF + "          WHERE B.GNL_NM_CD = A.GNL_NM_CD)";
                SQL += ComNum.VBLF + "     AND A.SPC_AGE_UNIT || COALESCE(A.AGE_PRS_CND_CD,'') = F.INFM_CD";
                SQL += ComNum.VBLF + "     AND F.EXM_KND_CD = '01'";
                SQL += ComNum.VBLF + "   AND D.DELDATE IS NULL ";
                SQL += ComNum.VBLF + "   AND A.ADPT_TO_DT ='99991231'";
                SQL += ComNum.VBLF + "   ORDER BY C.SUNEXT, A.ADPT_FR_DT DESC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                if(dt.Rows.Count > 0)
                {
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        strSuCode = dt.Rows[i]["SuNext"].ToString().Trim();

                        strFieldA = dt.Rows[i]["SPC_AGE"].ToString().Trim();

                        strFieldB = dt.Rows[i]["OFFR_MSG"].ToString().Trim().Replace(" ", "");
                        strDate = dt.Rows[i]["ADPT_FR_DT"].ToString().Trim();


                        SQL = "";
                        SQL = "INSERT INTO KOSMOS_PMPA.BAS_MSELF (SuCode,GubunA,GubunB,FieldA,FieldB,";
                        SQL = SQL + "EntDate) VALUES ('" + strSuCode + "','1','0','" + strFieldA + "','" + strFieldB + "',";
                        SQL = SQL + "TO_DATE('" + strDate + "','YYYY-MM-DD')) ";

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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
