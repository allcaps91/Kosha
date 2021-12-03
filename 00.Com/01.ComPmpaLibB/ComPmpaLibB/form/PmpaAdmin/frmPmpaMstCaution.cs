using ComLibB;
using System;
using System.Data;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스



/// <summary>
/// Description : 특별고객 빌드
/// Author : 박병규
/// Create Date : 2017.06.15
/// </summary>
/// <history>
/// </history>

namespace ComPmpaLibB
{
    public partial class frmPmpaMasterCaution : Form
    {
        clsUser CU = new clsUser();
        ComQuery CQ = new ComQuery();
        clsSpread SPR = new clsSpread();
        clsPmpaQuery CPQ = new clsPmpaQuery();

        public frmPmpaMasterCaution()
        {
            InitializeComponent();
            setEvent();
        }

        private void setEvent()
        {
            this.Load += new EventHandler(eForm_Load);

            this.dtpFdate.ValueChanged += new EventHandler(eForm_Clear);
            this.dtpTdate.ValueChanged += new EventHandler(eForm_Clear);

            this.dtpFdate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpTdate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtCnt.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtYYYY.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
        }

        private void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.dtpFdate && e.KeyChar == (Char)13) { dtpTdate.Focus(); }
            if (sender == this.dtpTdate && e.KeyChar == (Char)13) { txtCnt.Focus(); }
            if (sender == this.txtCnt && e.KeyChar == (Char)13) { btnSearch.Focus(); }
            if (sender == this.txtYYYY && e.KeyChar == (Char)13) { btnSave.Focus(); }
        }

        private void eForm_Clear(object sender, EventArgs e)
        {
            ComFunc.SetAllControlClear(pnlBody);
        }

        private void eForm_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            ComFunc.SetAllControlClear(pnlTop);
            ComFunc.SetAllControlClear(pnlBody);
            ComFunc.SetAllControlClear(pnlBottom);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Get_DataLoad();
        }






        private void Get_DataLoad()
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intCnt = 0;


            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) { return; }     //권한확인

            Cursor.Current = Cursors.WaitCursor;
            ComFunc.SetAllControlClear(pnlBody);

            intCnt = Convert.ToInt32(txtCnt.Text.Trim());

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO, COUNT(PANO) CNT";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER";
            SQL += ComNum.VBLF + "  WHERE 1 = 1";
            SQL += ComNum.VBLF + "    AND BDATE >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND BDATE <= TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND CHOJAE IN ('1','2') ";                                        //초진, 초진심야
            SQL += ComNum.VBLF + "    AND BI IN ('11','12','13') ";                                     //공단, 직장, 지역
            SQL += ComNum.VBLF + "    AND DEPTCODE NOT IN ('ER','NP','RM','DT') ";                      //응급의료센터, 정신건강의학과, 재활의학과, 치과
            SQL += ComNum.VBLF + "    AND (GBGAMEK IS NULL OR GBGAMEK IN ('00','51','41','55') ) ";     //감액무, 신자감액, 자원봉사자, 계약처
            SQL += ComNum.VBLF + "    AND PANO NOT IN(select PANO from ADMIN.mid_summary where tmodel = '5' GROUP BY PANO)";  //사망자 제외
            SQL += ComNum.VBLF + "    AND PANO NOT IN(select PANO from ADMIN.BAS_OCSMEMO_MID WHERE  GBN IN('0', '2')AND(DDate IS NULL OR DDate = '') GROUP BY PANO)"; //블랙리스트 제외
            SQL += ComNum.VBLF + "  GROUP BY PANO";
            SQL += ComNum.VBLF + "  HAVING COUNT(PANO) >= " + intCnt + " ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
            
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssList_Sheet1.RowCount = Dt.Rows.Count;
            ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            String strPtno = string.Empty;

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                strPtno = Dt.Rows[i]["PANO"].ToString().Trim();

                Get_BasPatient(clsDB.DbCon, i,strPtno);

                ssList_Sheet1.Cells[i, 0].Text = "";
                ssList_Sheet1.Cells[i, 1].Text = strPtno;
                ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["CNT"].ToString().Trim();
            }
            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;
        }

        private void Get_BasPatient(PsmhDb pDbCon, int intRow, string strPtno)
        {
            DataTable DtPat = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SNAME, JUMIN1, JUMIN2, JUMIN3, TEL, HPHONE, HPHONE2, LASTDATE,";
            SQL += ComNum.VBLF + "        BUILDNO, ZIPCODE1 || ZIPCODE2 AS ZIPCODE, JUSO";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
            SQL += ComNum.VBLF + "  WHERE 1 = 1";
            SQL += ComNum.VBLF + "    AND PANO ='" + strPtno + "' ";
            SqlErr = clsDB.GetDataTable(ref DtPat, SQL, clsDB.DbCon);

            if (DtPat.Rows.Count != 0)
            {
                ssList_Sheet1.Cells[intRow, 2].Text = DtPat.Rows[0]["SNAME"].ToString().Trim();
                ssList_Sheet1.Cells[intRow, 4].Text = DtPat.Rows[0]["JUMIN1"].ToString().Trim() + "-" + VB.Left( clsAES.DeAES(DtPat.Rows[0]["JUMIN3"].ToString().Trim()),1) + "******"; ;
                ssList_Sheet1.Cells[intRow, 5].Text = DtPat.Rows[0]["TEL"].ToString().Trim();
                ssList_Sheet1.Cells[intRow, 6].Text = DtPat.Rows[0]["HPHONE"].ToString().Trim();
                ssList_Sheet1.Cells[intRow, 7].Text = DtPat.Rows[0]["HPHONE2"].ToString().Trim();
                ssList_Sheet1.Cells[intRow, 8].Text = DtPat.Rows[0]["LASTDATE"].ToString().Trim();

                if (DtPat.Rows[0]["BUILDNO"].ToString().Trim() != "")
                {
                    ssList_Sheet1.Cells[intRow, 9].Text = CQ.Read_RoadJuso(clsDB.DbCon, DtPat.Rows[0]["BUILDNO"].ToString().Trim()) + " " + DtPat.Rows[0]["JUSO"].ToString().Trim();
                }
                else
                {
                    ssList_Sheet1.Cells[intRow, 9].Text = CQ.Read_Juso(clsDB.DbCon, DtPat.Rows[0]["ZIPCODE"].ToString().Trim()) + " " + DtPat.Rows[0]["JUSO"].ToString().Trim();
                }
            }

            DtPat.Dispose();
            DtPat = null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            string strChk = string.Empty;
            string strYear = string.Empty;

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) { return; }     //권한확인

            strYear = txtYYYY.Text.Trim();
            if (VB.Len(strYear) != 4)
            {
                ComFunc.MsgBox("대상연도를 넣고 빌드하십시오");
                txtYYYY.Focus();
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
             
            clsDB.setBeginTran(clsDB.DbCon);

            string strPtno = string.Empty;
            string strSname = string.Empty;
            string strRowid = string.Empty;

            try
            {
                for (int i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    strChk = ssList_Sheet1.Cells[i, 0].Text.Trim();

                    if (strChk != "")
                    {
                        strPtno = ssList_Sheet1.Cells[i, 1].Text.Trim();
                        strSname = ssList_Sheet1.Cells[i, 2].Text.Trim();

                        //기존자료 체크
                        DataTable Dt = new DataTable();

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT ROWID ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SVIP_MST ";
                        SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                        SQL += ComNum.VBLF + "    AND PANO = '" + strPtno + "' ";
                        SQL += ComNum.VBLF + "    AND (DELDATE IS NULL OR DELDATE = '') ";
                        SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                        if (Dt.Rows.Count > 0)
                            strRowid = Dt.Rows[0]["ROWID"].ToString().Trim();
                        else
                            strRowid = "";

                        Dt.Dispose();
                        Dt = null;

                        if (strRowid == "")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "BAS_SVIP_MST";
                            SQL += ComNum.VBLF + "       (PANO, SNAME, YEAR, JUMSU, GUBUN, ENTDATE, ENTDATE2, ENTSABUN)";
                            SQL += ComNum.VBLF + " VALUES('" + strPtno + "',";
                            SQL += ComNum.VBLF + "        '" + strSname + "',";
                            SQL += ComNum.VBLF + "        '" + strYear + "',";
                            SQL += ComNum.VBLF + "        0, '00', SYSDATE, SYSDATE,";
                            SQL += ComNum.VBLF + "        '" + clsType.User.Sabun + "')";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            //후불대상자자동등록
                            CQ.AUTO_MASTER_INSERT(clsDB.DbCon, strPtno, strSname, "감사고객 자동저장");

                            //환자마스터 UPDATE
                            SQL = "";
                            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT ";
                            SQL += ComNum.VBLF + "    SET GB_SVIP   = 'Y' ";
                            SQL += ComNum.VBLF + "  WHERE PANO      = '" + strPtno + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                    }

                    ssList_Sheet1.Cells[i, 0].Text = "";
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("특별고객 자료가 빌드되었습니다.", "알림");
                Cursor.Current = Cursors.Default;
            }

            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //컬럼헤더가 체크박스일 경우 전체선택 여부
        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            bool allChecked = false;

            if (e.ColumnHeader == false) { return; }
            if (e.Column != 0) { return; }

            if (ssList.Sheets[0].ColumnHeader.Cells[0,0].Value == null || Convert.ToBoolean(ssList.Sheets[0].ColumnHeader.Cells[0,0].Value) == false)
                allChecked = false;
            else
                allChecked = true;
            
            if (allChecked == true)
            {
                for (i = 0; i < ssList.Sheets[0].RowCount; i++)
                {
                    ssList.Sheets[0].Cells[i, 0].Value = false;
                }
                ssList.Sheets[0].ColumnHeader.Cells[0, 0].Value = false;
            }
            else
            {
                for (i = 0; i < ssList.Sheets[0].RowCount; i++)
                {
                    ssList.Sheets[0].Cells[i, 0].Value = true;
                }
                ssList.Sheets[0].ColumnHeader.Cells[0, 0].Value = true;
            }
        }
    }
}
