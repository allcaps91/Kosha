using ComBase;
using System;
using System.Data;
using System.Windows.Forms;
using FarPoint.Win.Spread;

namespace ComLibB
{
    public partial class frmOcsCpBasicRegist : Form, MainFormMessage
    {
       
        public frmOcsCpBasicRegist()
        {
            InitializeComponent();
            setEvent();
        }

        public frmOcsCpBasicRegist(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }


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

        void setEvent()
        {
            
            this.ssCp.CellClick += new CellClickEventHandler(Spread_Click);
           
        }
        void Spread_Click(object sender, CellClickEventArgs e)
        {
            clsSpread CS = new clsSpread();
            ComFunc CF = new ComFunc();
          

            if (sender == this.ssCp)
            {
                if (e.ColumnHeader == true)
                {
                    CS.setSpdSort(ssCp, e.Column, true);
                    return;
                }


            }
        }

        private void frmOcsCpBasicRegist_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            GetDataCpCode();
        }

        /// <summary>
        /// CP기초코드 정보 가져오기
        /// </summary>
        private void GetDataCpCode()
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            cboCP.Items.Clear();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + " GRPCD";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BASCD";
                SQL = SQL + ComNum.VBLF + "WHERE GRPCDB = 'CP관리' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY GRPCD";
                SQL = SQL + ComNum.VBLF + "ORDER BY DECODE(GRPCD, 'CP코드관리', 1, 'CP동의서', 2, 'CP제외기준', 3, 'CP중단사유', 4, 'CP지표', 5, 'CP지표참조', 6, 7) ASC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboCP.Items.Add(dt.Rows[i]["GRPCD"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                cboCP.SelectedIndex = 0;
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

        private void cboCP_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetSearchData();
        }

        void GetSearchData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssCp_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + " GRPCD, BASCD, APLFRDATE, APLENDDATE, BASNAME, BASNAME1,";
                SQL = SQL + ComNum.VBLF + " VFLAG1, VFLAG2, VFLAG3, VFLAG4, ";
                SQL = SQL + ComNum.VBLF + " NFLAG1, NFLAG2, NFLAG3, NFLAG4, ";
                SQL = SQL + ComNum.VBLF + " REMARK, REMARK1, ";
                SQL = SQL + ComNum.VBLF + " USECLS,";
                SQL = SQL + ComNum.VBLF + " DISPSEQ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BASCD";
                SQL = SQL + ComNum.VBLF + "WHERE GRPCDB = 'CP관리' ";
                SQL = SQL + ComNum.VBLF + "  AND GRPCD = '" + cboCP.Text + "'";
                SQL = SQL + ComNum.VBLF + "ORDER BY DISPSEQ";

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


                ssCp_Sheet1.RowCount = dt.Rows.Count + 1;
                ssCp_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                ssCp_Sheet1.Cells[ssCp_Sheet1.RowCount - 1, 14].Text = "9998-12-31";
                ssCp_Sheet1.Cells[ssCp_Sheet1.RowCount - 1, 16].Text = "C" ;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssCp_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                    ssCp_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                    ssCp_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BASNAME1"].ToString().Trim();
                    ssCp_Sheet1.Cells[i, 3].Text = dt.Rows[i]["VFLAG1"].ToString().Trim();
                    ssCp_Sheet1.Cells[i, 4].Text = dt.Rows[i]["VFLAG2"].ToString().Trim();
                    ssCp_Sheet1.Cells[i, 5].Text = dt.Rows[i]["VFLAG3"].ToString().Trim();
                    ssCp_Sheet1.Cells[i, 6].Text = dt.Rows[i]["VFLAG4"].ToString().Trim();
                    ssCp_Sheet1.Cells[i, 7].Text = dt.Rows[i]["NFLAG1"].ToString().Trim();
                    ssCp_Sheet1.Cells[i, 8].Text = dt.Rows[i]["NFLAG2"].ToString().Trim();
                    ssCp_Sheet1.Cells[i, 9].Text = dt.Rows[i]["NFLAG3"].ToString().Trim();
                    ssCp_Sheet1.Cells[i, 10].Text = dt.Rows[i]["NFLAG4"].ToString().Trim();
                    ssCp_Sheet1.Cells[i, 11].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                    ssCp_Sheet1.Cells[i, 12].Text = dt.Rows[i]["REMARK1"].ToString().Trim();
                    ssCp_Sheet1.Cells[i, 13].Text = ComFunc.FormatStrToDate(dt.Rows[i]["APLFRDATE"].ToString().Trim(),"D");
                    ssCp_Sheet1.Cells[i, 14].Text = ComFunc.FormatStrToDate(dt.Rows[i]["APLENDDATE"].ToString().Trim(), "D");
                    ssCp_Sheet1.Cells[i, 15].Text = dt.Rows[i]["USECLS"].ToString().Trim();
                    ssCp_Sheet1.Cells[i, 16].Text = "U";
                }
                ssCp_Sheet1.RowCount = dt.Rows.Count + 20;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ssCp_Sheet1.RowCount == 0)
            {
                ComFunc.MsgBox("저장할 데이타가 없습니다.");
                return;
            }

            if (SaveData() == true)
            {
                GetDataCpCode();
            }

            return;
        }

        private bool SaveData()
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return false; //권한 확인

            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

                for (i = 0; i < ssCp_Sheet1.RowCount; i++)
                {
                    if (ssCp_Sheet1.Cells[i, 16].Text.Trim() == "U") //신규등록이 아니면
                    {
                        //if (Convert.ToBoolean(ssCp_Sheet1.Cells[i, 15].Value) == true) //사용으로 체크한 경우만
                        //{
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " INSERT INTO KOSMOS_PMPA.BAS_BASCD_DEL ( ";
                            SQL = SQL + ComNum.VBLF + "      GRPCDB,GRPCD,BASCD,APLFRDATE, ";
                            SQL = SQL + ComNum.VBLF + "      APLENDDATE,BASNAME,BASNAME1,VFLAG1, ";
                            SQL = SQL + ComNum.VBLF + "      VFLAG2,VFLAG3,VFLAG4,NFLAG1, ";
                            SQL = SQL + ComNum.VBLF + "      NFLAG2,NFLAG3,NFLAG4,REMARK, ";
                            SQL = SQL + ComNum.VBLF + "      REMARK1,INPDATE,INPTIME,USECLS, ";
                            SQL = SQL + ComNum.VBLF + "      DISPSEQ,DELDATE,DELSABUN )";
                            SQL = SQL + ComNum.VBLF + " SELECT ";
                            SQL = SQL + ComNum.VBLF + "      GRPCDB,GRPCD,BASCD,APLFRDATE, ";
                            SQL = SQL + ComNum.VBLF + "      APLENDDATE,BASNAME,BASNAME1,VFLAG1, ";
                            SQL = SQL + ComNum.VBLF + "      VFLAG2,VFLAG3,VFLAG4,NFLAG1, ";
                            SQL = SQL + ComNum.VBLF + "      NFLAG2,NFLAG3,NFLAG4,REMARK, ";
                            SQL = SQL + ComNum.VBLF + "      REMARK1,INPDATE,INPTIME,USECLS, ";
                            SQL = SQL + ComNum.VBLF + "      DISPSEQ, SYSDATE, '" + clsType.User.Sabun + "'";
                            SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_BASCD ";
                            SQL = SQL + ComNum.VBLF + "  WHERE GRPCDB = 'CP관리' ";
                            SQL = SQL + ComNum.VBLF + "    AND GRPCD = '" + cboCP.Text + "'";
                            SQL = SQL + ComNum.VBLF + "    AND BASCD = '" + ssCp_Sheet1.Cells[i, 0].Text.Trim() + "'";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return false;
                            }

                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "BAS_BASCD SET ";
                            SQL = SQL + ComNum.VBLF + "    BASCD = '" + ssCp_Sheet1.Cells[i, 0].Text.Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "    BASNAME = '" + ssCp_Sheet1.Cells[i, 1].Text.Trim() + "',";
                            SQL = SQL + ComNum.VBLF + "    BASNAME1 = '" + ssCp_Sheet1.Cells[i, 2].Text.Trim() + "',";
                            SQL = SQL + ComNum.VBLF + "    VFLAG1 = '" + ssCp_Sheet1.Cells[i, 3].Text.Trim() + "',";
                            SQL = SQL + ComNum.VBLF + "    VFLAG2 = '" + ssCp_Sheet1.Cells[i, 4].Text.Trim() + "',";
                            SQL = SQL + ComNum.VBLF + "    VFLAG3 = '" + ssCp_Sheet1.Cells[i, 5].Text.Trim() + "',";
                            SQL = SQL + ComNum.VBLF + "    VFLAG4 = '" + ssCp_Sheet1.Cells[i, 6].Text.Trim() + "',";
                            SQL = SQL + ComNum.VBLF + "    NFLAG1 = '" + ssCp_Sheet1.Cells[i, 7].Text.Trim() + "',";
                            SQL = SQL + ComNum.VBLF + "    NFLAG2 = '" + ssCp_Sheet1.Cells[i, 8].Text.Trim() + "',";
                            SQL = SQL + ComNum.VBLF + "    NFLAG3 = '" + ssCp_Sheet1.Cells[i, 9].Text.Trim() + "',";
                            SQL = SQL + ComNum.VBLF + "    NFLAG4 = '" + ssCp_Sheet1.Cells[i, 10].Text.Trim() + "',";
                            SQL = SQL + ComNum.VBLF + "    REMARK = '" + ssCp_Sheet1.Cells[i, 11].Text.Trim() + "',";
                            SQL = SQL + ComNum.VBLF + "    REMARK1 = '" + ssCp_Sheet1.Cells[i, 12].Text.Trim() + "',";
                            SQL = SQL + ComNum.VBLF + "    APLFRDATE = '" + ssCp_Sheet1.Cells[i, 13].Text.Trim().Replace("-", "") + "',";
                            SQL = SQL + ComNum.VBLF + "    APLENDDATE = '" + ssCp_Sheet1.Cells[i, 14].Text.Trim().Replace("-", "") + "',";
                            SQL = SQL + ComNum.VBLF + "    USECLS = '" + (Convert.ToBoolean(ssCp_Sheet1.Cells[i, 15].Value) == true ? "1" : "0") + "', ";
                            SQL = SQL + ComNum.VBLF + "    DISPSEQ = " + i.ToString() + ", ";
                            SQL = SQL + ComNum.VBLF + "    INPDATE = '" + VB.Left(strCurDateTime, 8) + "',";
                            SQL = SQL + ComNum.VBLF + "    INPTIME = '" + VB.Right(strCurDateTime, 6) + "'";
                            SQL = SQL + ComNum.VBLF + "WHERE GRPCDB = 'CP관리' ";
                            SQL = SQL + ComNum.VBLF + "  AND GRPCD = '" + cboCP.Text + "'";
                            SQL = SQL + ComNum.VBLF + "  AND BASCD = '" + ssCp_Sheet1.Cells[i, 0].Text.Trim() + "'";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                        //}
                        //else
                        //{
                            //저장 시 삭제 기능은 실수로 삭제할 가능성이 있기 때문에 없앰(2021-03-11)
                            //SQL = "DELETE " + ComNum.DB_PMPA + "BAS_BASCD";
                            //SQL = SQL + ComNum.VBLF + "WHERE GRPCDB = 'CP관리' ";
                            //SQL = SQL + ComNum.VBLF + "  AND GRPCD = '" + cboCP.Text + "'";
                            //SQL = SQL + ComNum.VBLF + "  AND BASCD = '" + ssCp_Sheet1.Cells[i, 0].Text.Trim() + "'";

                            //SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            //if (SqlErr != "")
                            //{
                            //    clsDB.setRollbackTran(clsDB.DbCon);
                            //    ComFunc.MsgBox(SqlErr);
                            //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            //    Cursor.Current = Cursors.Default;
                            //    return false;
                            //}
                        //}
                    }
                    else
                    {
                        if (Convert.ToBoolean(ssCp_Sheet1.Cells[i, 15].Value) == true) //사용으로 체크한 경우만
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "BAS_BASCD ";
                            SQL = SQL + ComNum.VBLF + " ( ";
                            SQL = SQL + ComNum.VBLF + "   GRPCDB, GRPCD, BASCD, BASNAME, BASNAME1, ";
                            SQL = SQL + ComNum.VBLF + "   VFLAG1, VFLAG2, VFLAG3, VFLAG4,  ";
                            SQL = SQL + ComNum.VBLF + "   NFLAG1, NFLAG2, NFLAG3, NFLAG4,  ";
                            SQL = SQL + ComNum.VBLF + "   REMARK, REMARK1, APLFRDATE, APLENDDATE, ";
                            SQL = SQL + ComNum.VBLF + "   INPDATE, INPTIME, USECLS, DISPSEQ              ";
                            SQL = SQL + ComNum.VBLF + " )";
                            SQL = SQL + ComNum.VBLF + "VALUES ( ";
                            SQL = SQL + ComNum.VBLF + "     'CP관리', "; //GRPCDB
                            SQL = SQL + ComNum.VBLF + "     '" + cboCP.Text.Trim() + "', "; //GRPCD
                            SQL = SQL + ComNum.VBLF + "     '" + ssCp_Sheet1.Cells[i, 0].Text.Trim() + "', "; //BASCD
                            SQL = SQL + ComNum.VBLF + "     '" + ssCp_Sheet1.Cells[i, 1].Text.Trim() + "', "; //BASNAME
                            SQL = SQL + ComNum.VBLF + "     '" + ssCp_Sheet1.Cells[i, 2].Text.Trim() + "', "; //BASNAME1
                            SQL = SQL + ComNum.VBLF + "     '" + ssCp_Sheet1.Cells[i, 3].Text.Trim() + "', "; //VFLAG1
                            SQL = SQL + ComNum.VBLF + "     '" + ssCp_Sheet1.Cells[i, 4].Text.Trim() + "', "; //VFLAG2
                            SQL = SQL + ComNum.VBLF + "     '" + ssCp_Sheet1.Cells[i, 5].Text.Trim() + "', "; //VFLAG3
                            SQL = SQL + ComNum.VBLF + "     '" + ssCp_Sheet1.Cells[i, 6].Text.Trim() + "', "; //VFLAG4
                            SQL = SQL + ComNum.VBLF + "     '" + ssCp_Sheet1.Cells[i, 7].Text.Trim() + "', "; //NFLAG1
                            SQL = SQL + ComNum.VBLF + "     '" + ssCp_Sheet1.Cells[i, 8].Text.Trim() + "', "; //NFLAG2
                            SQL = SQL + ComNum.VBLF + "     '" + ssCp_Sheet1.Cells[i, 9].Text.Trim() + "', "; //NFLAG3
                            SQL = SQL + ComNum.VBLF + "     '" + ssCp_Sheet1.Cells[i, 10].Text.Trim() + "', "; //NFLAG4
                            SQL = SQL + ComNum.VBLF + "     '" + ssCp_Sheet1.Cells[i, 11].Text.Trim() + "', "; //REMARK
                            SQL = SQL + ComNum.VBLF + "     '" + ssCp_Sheet1.Cells[i, 12].Text.Trim() + "', "; //REMARK1
                            SQL = SQL + ComNum.VBLF + "     '" + ssCp_Sheet1.Cells[i, 13].Text.Trim().Replace("-", "") + "', "; //APLFRDATE
                            SQL = SQL + ComNum.VBLF + "     '" + ssCp_Sheet1.Cells[i, 14].Text.Trim().Replace("-", "") + "', "; //APLENDDATE
                            SQL = SQL + ComNum.VBLF + "     '" + VB.Left(strCurDateTime, 8) + "', "; //INPDATE
                            SQL = SQL + ComNum.VBLF + "     '" + VB.Right(strCurDateTime, 6) + "', "; //INPTIME
                            SQL = SQL + ComNum.VBLF + "     '" + (Convert.ToBoolean(ssCp_Sheet1.Cells[i, 15].Value) == true ? "1" : "0") + "', "; //USECLS
                            SQL = SQL + ComNum.VBLF + "     '" + i.ToString() + "' "; //DISPSEQ
                            SQL = SQL + ComNum.VBLF + " ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ssCp_Sheet1.RowCount += 1;
            ssCp_Sheet1.Cells[ssCp_Sheet1.RowCount - 1, 14].Text = "9998-12-31";
            ssCp_Sheet1.Cells[ssCp_Sheet1.RowCount - 1, 16].Text = "C";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int i = 0;
            string strCPName = "";
            string strCPCode = "";
            string strSDate = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수


            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인


            if (cboCP.Text.Trim() == "CP코드관리")
            {
                strCPCode = ssCp_Sheet1.Cells[ssCp_Sheet1.ActiveRowIndex, 0].Text.Trim();
                strCPName = ssCp_Sheet1.Cells[ssCp_Sheet1.ActiveRowIndex, 1].Text.Trim();
                strSDate = ssCp_Sheet1.Cells[ssCp_Sheet1.ActiveRowIndex, 13].Text.Trim();

                if (ComFunc.MsgBoxQ("◆" + strCPName + "(" + strCPCode + ")" + ComNum.VBLF + ComNum.VBLF + "삭제 후 복구는 불가능 합니다. 해당 코드를 삭제하시겠습니까?") == DialogResult.Yes)
                {
                    if (Convert.ToDateTime(strSDate) <= Convert.ToDateTime(clsPublic.GstrSysDate))
                    {
                        ComFunc.MsgBox("적용일자 이후는 삭제가 불가능합니다.");
                        return;
                    }

                    Cursor.Current = Cursors.WaitCursor;
                    clsDB.setBeginTran(clsDB.DbCon);

                    try
                    {

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " INSERT INTO KOSMOS_PMPA.BAS_BASCD_DEL ( ";
                        SQL = SQL + ComNum.VBLF + "      GRPCDB,GRPCD,BASCD,APLFRDATE, ";
                        SQL = SQL + ComNum.VBLF + "      APLENDDATE,BASNAME,BASNAME1,VFLAG1, ";
                        SQL = SQL + ComNum.VBLF + "      VFLAG2,VFLAG3,VFLAG4,NFLAG1, ";
                        SQL = SQL + ComNum.VBLF + "      NFLAG2,NFLAG3,NFLAG4,REMARK, ";
                        SQL = SQL + ComNum.VBLF + "      REMARK1,INPDATE,INPTIME,USECLS, ";
                        SQL = SQL + ComNum.VBLF + "      DISPSEQ,DELDATE,DELSABUN )";
                        SQL = SQL + ComNum.VBLF + " SELECT ";
                        SQL = SQL + ComNum.VBLF + "      GRPCDB,GRPCD,BASCD,APLFRDATE, ";
                        SQL = SQL + ComNum.VBLF + "      APLENDDATE,BASNAME,BASNAME1,VFLAG1, ";
                        SQL = SQL + ComNum.VBLF + "      VFLAG2,VFLAG3,VFLAG4,NFLAG1, ";
                        SQL = SQL + ComNum.VBLF + "      NFLAG2,NFLAG3,NFLAG4,REMARK, ";
                        SQL = SQL + ComNum.VBLF + "      REMARK1,INPDATE,INPTIME,USECLS, ";
                        SQL = SQL + ComNum.VBLF + "      DISPSEQ, SYSDATE, '" + clsType.User.Sabun + "'";
                        SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_BASCD ";
                        SQL += ComNum.VBLF + " WHERE GRPCDB = 'CP관리' ";
                        SQL += ComNum.VBLF + "   AND GRPCD = 'CP코드관리' ";
                        SQL += ComNum.VBLF + "   AND BASCD = '" + strCPCode + "' ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }



                        SQL = " DELETE KOSMOS_PMPA.BAS_BASCD ";
                        SQL += ComNum.VBLF + " WHERE GRPCDB = 'CP관리' ";
                        SQL += ComNum.VBLF + "   AND GRPCD = 'CP코드관리' ";
                        SQL += ComNum.VBLF + "   AND BASCD = '" + strCPCode + "' ";
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
                        ComFunc.MsgBox("저장하였습니다.");
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

                    GetSearchData();

                    return;
                }
            }

            if (ssCp_Sheet1.Cells[ssCp_Sheet1.RowCount - 1, 1].Text.Trim() != "")
            {
                ComFunc.MsgBox("기존 저장된 Data 입니다. 삭제 할 수 없습니다.");
                return;
            }

            ssCp_Sheet1.RowCount -= 1;
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (ssCp_Sheet1.NonEmptyRowCount == 0) return;

            if (ssCp_Sheet1.ActiveRowIndex == 0) return;

            ssCp_Sheet1.SwapRange(ssCp_Sheet1.ActiveRowIndex, 0, ssCp_Sheet1.ActiveRowIndex - 1, 0, 1, ssCp_Sheet1.ColumnCount, false);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (ssCp_Sheet1.NonEmptyRowCount == 0) return;

            ssCp_Sheet1.SwapRange(ssCp_Sheet1.ActiveRowIndex, 0, ssCp_Sheet1.ActiveRowIndex + 1, 0, 1, ssCp_Sheet1.ColumnCount, false);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmOcsCpBasicRegist_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frmOcsCpBasicRegist_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }
    }
}
