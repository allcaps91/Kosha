using ComBase;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : EmrJob
    /// File Name       : frmEmrJobConvPano
    /// Description     : 등록번호 일괄 변경작업(Ver 2018-11-28)
    /// Author          : 이현종
    /// Create Date     : 2020-02-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= "PSMH\mid\midchdl\FrmMain(Conv_Pano.frm) >> frmEmrJobConvPano.cs 폼이름 재정의" />
    public partial class frmEmrJobConvPano : Form, MainFormMessage
    {
        #region //MainFormMessage
        string mPara1 = string.Empty;
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

        #region 서브폼
        private List<Form> subFormList = null;
        #endregion

        bool LoadActiveForm = false;

        private BackgroundWorker worker;
        OracleDataReader WorkerReader = null;


        public frmEmrJobConvPano()
        {
            InitializeComponent();
        }

        public frmEmrJobConvPano(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;

        }

        public frmEmrJobConvPano(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
        }

        private void frmEmrJobConvPano_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            btnSave.Enabled = false;
            btnSearch.Enabled = false;

            subFormList = new List<Form>();
            SSTables_Sheet1.RowCount = 0;

            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.RunWorkerAsync();

            //GetSearchData2();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            GetSearchData2();
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // 에러가 있는지 체크
            if (e.Error != null)
            {
                ComFunc.MsgBoxEx(this, e.Error.Message, "Error");
                return;
            }

            if (WorkerReader != null)
            {
                if (WorkerReader.HasRows)
                {
                    while (WorkerReader.Read())
                    {
                        SSTables_Sheet1.RowCount += 1;
                        SSTables_Sheet1.Cells[SSTables_Sheet1.RowCount - 1, 0].Text = WorkerReader.GetValue(0).ToString().Trim();
                        SSTables_Sheet1.Cells[SSTables_Sheet1.RowCount - 1, 1].Text = WorkerReader.GetValue(1).ToString().Trim();
                        SSTables_Sheet1.Cells[SSTables_Sheet1.RowCount - 1, 2].Text = WorkerReader.GetValue(2).ToString().Trim();
                        SSTables_Sheet1.Cells[SSTables_Sheet1.RowCount - 1, 3].Text = WorkerReader.GetValue(3).ToString().Trim();
                    }
                }

                WorkerReader.Dispose();
            }

            label1.Text = "데이터 조회가 완료되었습니다 사용하시면 됩니다!";
            label1.ForeColor = System.Drawing.Color.Navy;
            btnSave.Enabled   = true;
            btnSearch.Enabled = true;
        }

        private void GetSearchData2()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }


            #region 변수
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            #endregion

            Cursor.Current = Cursors.WaitCursor;

            #region 서브쿼리 2
            //SQL = " SELECT b.Owner,a.table_name,a.Column_name, A.DATA_TYPE";
            //SQL += ComNum.VBLF + " FROM ALL_TAB_COLUMNS a, SYS.DBA_TABLES b, ETC_LAYOUT C";
            //SQL += ComNum.VBLF + " Where a.table_name = b.table_name";
            //SQL += ComNum.VBLF + " AND A.TABLE_NAME = C.TBLNAME";
            //SQL += ComNum.VBLF + " AND C.TBLGBN = '1'";
            //SQL += ComNum.VBLF + " and a.column_name in ('PANO','PTNO','PTMIIDNO','DGDCIDNO','DGOTIDNO','TRPTIDNO','OPPTIDNO','STRMIDNO') ";//   'PATID 기록실 차트 관련 테이블 제외함 2010-09-06
            //SQL += ComNum.VBLF + " AND A.DATA_TYPE IN ('CHAR', 'VARCHAR2') ";
            //SQL += ComNum.VBLF + " AND A.TABLE_NAME NOT IN ('HIC_WORK_RESULTC','HIC_WORK_SPECMST', 'ENDO_JUPMST','ENDO_JUSAMST','ENDO_REMARK','BAS_PATIENT', ";
            //SQL += ComNum.VBLF + "                          'XRAY_DETAIL','HIC_XRAY_RESULT','XRAY_RESULTNEW','XRAY_PACSSEND','XRAY_DETAIL_DEL','XRAY_NEWIPWON_NOT','XRAY_RESULT','XRAY_TONGWORK','EXAM_RESULTC','ETC_PANO_HIS','EMRXMLHISTORY' ";// '2013-07-17;
            //SQL += ComNum.VBLF + "                          , 'AEMRCHARTMSTHIS') ";// '2020-04-10 신규 EMR히스토리 테이블;
            //SQL += ComNum.VBLF + " GROUP BY b.Owner,a.table_name,a.Column_name, A.DATA_TYPE ";
            //SQL += ComNum.VBLF + " order by owner ";

            SQL = " SELECT a.Owner,a.table_name,a.Column_name, A.DATA_TYPE";
            SQL += ComNum.VBLF + " FROM ALL_TAB_COLUMNS a,  ETC_LAYOUT C";
            SQL += ComNum.VBLF + " Where A.TABLE_NAME = C.TBLNAME";
            SQL += ComNum.VBLF + " AND C.TBLGBN = '1'  ";
            SQL += ComNum.VBLF + " AND global_stats = 'YES' ";
            SQL += ComNum.VBLF + " and a.column_name in ('PANO','PTNO','PTMIIDNO','DGDCIDNO','DGOTIDNO','TRPTIDNO','OPPTIDNO','STRMIDNO') ";//   'PATID 기록실 차트 관련 테이블 제외함 2010-09-06
            SQL += ComNum.VBLF + " AND A.DATA_TYPE IN ('CHAR', 'VARCHAR2') ";
            SQL += ComNum.VBLF + " AND A.TABLE_NAME NOT IN ('HIC_WORK_RESULTC','HIC_WORK_SPECMST', 'ENDO_JUPMST','ENDO_JUSAMST','ENDO_REMARK','BAS_PATIENT', ";
            SQL += ComNum.VBLF + "                          'XRAY_DETAIL','HIC_XRAY_RESULT','XRAY_RESULTNEW','XRAY_PACSSEND','XRAY_DETAIL_DEL','XRAY_NEWIPWON_NOT','XRAY_RESULT','XRAY_TONGWORK','EXAM_RESULTC','ETC_PANO_HIS','EMRXMLHISTORY' ";// '2013-07-17;
            SQL += ComNum.VBLF + "                          , 'AEMRCHARTMSTHIS') ";// '2020-04-10 신규 EMR히스토리 테이블;
            SQL += ComNum.VBLF + " GROUP BY a.Owner,a.table_name,a.Column_name, A.DATA_TYPE ";
            SQL += ComNum.VBLF + " order by owner ";




            SqlErr = GetAdoRs(ref WorkerReader, SQL);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, SqlErr);
                return;
            }

            //if (reader.HasRows)
            //{
            //    while (reader.Read())
            //    {
            //        SSTables_Sheet1.RowCount += 1;
            //        SSTables_Sheet1.Cells[SSTables_Sheet1.RowCount - 1, 0].Text = reader.GetValue(0).ToString().Trim();
            //        SSTables_Sheet1.Cells[SSTables_Sheet1.RowCount - 1, 1].Text = reader.GetValue(1).ToString().Trim();
            //        SSTables_Sheet1.Cells[SSTables_Sheet1.RowCount - 1, 2].Text = reader.GetValue(2).ToString().Trim();
            //        SSTables_Sheet1.Cells[SSTables_Sheet1.RowCount - 1, 3].Text = reader.GetValue(3).ToString().Trim();
            //    }
            //}

            //reader.Dispose();
            #endregion

            Cursor.Current = Cursors.Default;
        }

        public static string GetAdoRs(ref OracleDataReader reader, string SQL)
        {

            try
            {
                using (OracleCommand cmd = clsDB.DbCon.Con.CreateCommand())
                {
                    cmd.InitialLONGFetchSize = -1;
                    cmd.CommandText = SQL;
                    cmd.CommandTimeout = 60;

                    reader = cmd.ExecuteReader();
                }

                return string.Empty;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                return sqlExc.Message;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return ex.Message;
            }
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Close();
        }



        #region 서브폼 이벤트
        private void mnu_Click(object sender, EventArgs e)
        {
            SubForm_View((sender as ToolStripButton).Tag.ToString());
        }

        private void frmSubForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form frm = subFormList.Find(f => f.Equals(sender));
            if (frm != null)
            {
                frm.Dispose();
                subFormList.Remove(frm);
            }
        }
        /// <summary>
        /// 서브폼 열기
        /// </summary>
        /// <param name="SubForm"></param>
        private void SubForm_View(string SubFormName, string DllNm = "ComLibB")
        {
            Form frm = subFormList.Find(f => f.Name.Equals(SubFormName));
            if (frm == null)
            {
                Assembly assem = Assembly.LoadFrom(DllNm + ".dll");
                Form objForm = null;

                Type t = assem.GetType(DllNm + "." + SubFormName);
                objForm = (Form)Activator.CreateInstance(t);

                objForm.StartPosition = FormStartPosition.CenterParent;
                objForm.FormClosed += frmSubForm_FormClosed;
                objForm.Show(this);

                subFormList.Add(objForm);
            }
            else
            {
                frm.BringToFront();
                frm.Show();
            }
        }
        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetSearchData();
        }

        private void GetSearchData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }

            progressBar1.Maximum = SSTables_Sheet1.RowCount;
            progressBar1.Value = 0;
            
            SS1_Sheet1.RowCount = 0;
            SS2_Sheet1.Cells[0, 0].Text = "";

            if (string.IsNullOrWhiteSpace(TxtPano.Text.Trim()))
                return;

            #region 변수
            string strPano = VB.Val(TxtPano.Text.Trim()).ToString("00000000");
            OracleDataReader reader = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            #endregion

            if (strPano.Equals("07063405") || strPano.Equals("06761899"))
            {
                ComFunc.MsgBoxEx(this, "작업불가.. 임시로 막음(심사과장+전산실)");
                return;
            }

            for (int i = 0; i < SSTables_Sheet1.NonEmptyRowCount; i++)
            {
   

                string strUser = SSTables_Sheet1.Cells[i, 0].Text.Trim();
                string strTables = SSTables_Sheet1.Cells[i, 1].Text.Trim();
                string strPanoGbn = SSTables_Sheet1.Cells[i, 2].Text.Trim();
                SSTables_Sheet1.Cells[i, 4].Text = "";

                string strTableOK = (strUser).ToUpper() + "." + (strTables).ToUpper();
                strTables = (strTables).ToUpper();
                strPanoGbn = (strPanoGbn).ToUpper();

                if (strTableOK.Equals("KOSMOS_OCS.EXAM_SPECMST"))
                {
                    #region 서브쿼리 1
                    SQL = " SELECT '" + strTableOK + "' AS Tables, " + strPanoGbn + " AS Pano, ROWID ";
                    SQL += ComNum.VBLF + " FROM " + strTableOK + " ";
                    SQL += ComNum.VBLF + "  WHERE " + strPanoGbn + " ='" + strPano + "' ";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        ComFunc.MsgBoxEx(this, SqlErr);
                        return;
                    }

                    if (reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            SS1_Sheet1.RowCount += 1;
                            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 0].Text = reader.GetValue(1).ToString().Trim();
                            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 1].Text = strTableOK;
                            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 2].Text = strPanoGbn;
                            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 3].Text = reader.GetValue(2).ToString().Trim();
                            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 4].Text = "SELECT * FROM " + strTableOK + " WHERE " + strPanoGbn + " in  ('06691239','06063545')";
                        }
                    }

                    reader.Dispose();
                    #endregion

                    #region 서브쿼리 2
                    SQL = " SELECT 'KOSMOS_OCS.EXAM_RESULTC' AS Tables, " + strPanoGbn + " AS Pano, ROWID ";
                    SQL += ComNum.VBLF + " FROM KOSMOS_OCS.EXAM_RESULTC ";
                    SQL += ComNum.VBLF + "  WHERE SPECNO IN (SELECT SPECNO FROM KOSMOS_OCS.EXAM_SPECMST ";
                    SQL += ComNum.VBLF + "                      WHERE PANO = '" + strPano + "' )";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        ComFunc.MsgBoxEx(this, SqlErr);
                        return;
                    }

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            SS1_Sheet1.RowCount += 1;
                            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 0].Text = reader.GetValue(1).ToString().Trim();
                            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 1].Text = "KOSMOS_OCS.EXAM_RESULTC";
                            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 2].Text = "PANO";
                            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 3].Text = reader.GetValue(2).ToString().Trim();
                            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 4].Text = "SELECT * FROM KOSMOS_OCS.EXAM_RESULTC WHERE " + strPanoGbn + " in  ('06691239','06063545')";
                        }
                    }

                    reader.Dispose();
                    #endregion
                }
                else
                {
                    #region 서브쿼리
                    SQL = " SELECT '" + strTableOK + "' AS Tables, " + strPanoGbn + " AS Pano, ROWID ";
                    SQL += ComNum.VBLF + " FROM " + strTableOK + " ";
                    SQL += ComNum.VBLF + "  WHERE " + strPanoGbn + " ='" + strPano + "' ";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        ComFunc.MsgBoxEx(this, SqlErr);
                        return;
                    }

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            SS1_Sheet1.RowCount += 1;
                            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 0].Text = reader.GetValue(1).ToString().Trim();
                            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 1].Text = strTableOK;
                            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 2].Text = strPanoGbn;
                            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 3].Text = reader.GetValue(2).ToString().Trim();
                            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 4].Text = "SELECT * FROM " + strTableOK + " WHERE " + strPanoGbn + " in  ('06691239','06063545')";
                        }
                    }

                    reader.Dispose();
                    #endregion
                }

                Application.DoEvents();
                progressBar1.Value += 1;
            }


            #region 방사선과
            SQL = " SELECT PANO FROM KOSMOS_PMPA.XRAY_DETAIL ";
            SQL += ComNum.VBLF + " WHERE PANO = '" + TxtPano.Text + "' ";

            SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                ComFunc.MsgBoxEx(this, SqlErr);
                return;
            }

            if (reader.HasRows)
            {
                SS2_Sheet1.Cells[0, 0].Text = "방사선,";
        }

            reader.Dispose();
            #endregion

            #region 내시경건
            SQL = " SELECT PTNO FROM KOSMOS_OCS.ENDO_JUPMST ";
            SQL += ComNum.VBLF + " WHERE PTNO = '" + TxtPano.Text + "' ";

            SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                ComFunc.MsgBoxEx(this, SqlErr);
                return;
            }

            if (reader.HasRows)
            {
                SS2_Sheet1.Cells[0, 0].Text += "내시경,";
            }

            reader.Dispose();
            #endregion

            #region ECHO
            SQL = " SELECT PTNO FROM KOSMOS_OCS.ETC_JUPMST ";
            SQL += ComNum.VBLF + " WHERE PTNO = '" + TxtPano.Text + "' ";
            SQL += ComNum.VBLF + "   AND GBJOB = '3'";
            SQL += ComNum.VBLF + "   AND GUBUN IN ('3','9','10','11','22') ";

            SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                ComFunc.MsgBoxEx(this, SqlErr);
                return;
            }

            if (reader.HasRows)
            {
                SS2_Sheet1.Cells[0, 0].Text += "심장검사,";
            }

            reader.Dispose();
            #endregion


            #region HR chest건
            SQL = " SELECT PTNO FROM KOSMOS_PMPA.HIC_XRAY_RESULT";
            SQL += ComNum.VBLF + " WHERE PTNO = '" + TxtPano.Text + "' ";

            SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                ComFunc.MsgBoxEx(this, SqlErr);
                return;
            }

            if (reader.HasRows)
            {
                SS2_Sheet1.Cells[0, 0].Text += "HR chest,";
            }

            reader.Dispose();
            #endregion

            //'2014-11-26 LOCK 설정
            clsLockCheck.GstrLockPtno = TxtPano.Text.Trim();
            clsLockCheck.GstrLockRemark = clsType.User.IdNumber + " " + clsType.User.UserName + "님이 이중챠트 작업중 입니다";

            if (clsLockCheck.IpdOcs_Lock_Insert_NEW().Equals("NO"))
                return;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return;
            }

            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtNewPano.Text.Trim()))
            {
                ComFunc.MsgBoxEx(this, "변경할 등록번호가 없습니다");
                return;
            }

            if (string.IsNullOrWhiteSpace(lblName.Text.Trim()))
            {
                ComFunc.MsgBoxEx(this, "변경할 이름이 없습니다");
                return;
            }

            if (ComFunc.MsgBoxQEx(this, TxtPano.Text + "-->" + TxtNewPano.Text + " 등록번호변경 작업을 시작하시겠습니까?") == DialogResult.No)
                return;

            #region 변수
            OracleDataReader reader = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            #endregion

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                int RowAffected = 0;

                for (int i = 0; i < SS1_Sheet1.NonEmptyRowCount; i++)
                {
                    string strPano = SS1_Sheet1.Cells[i, 0].Text.Trim();
                    string strTable = SS1_Sheet1.Cells[i, 1].Text.Trim();
                    string strColumn = SS1_Sheet1.Cells[i, 2].Text.Trim();
                    string strROWID = SS1_Sheet1.Cells[i, 3].Text.Trim();

                    if (VB.Left(strTable.ToUpper(), 23).Equals("KOSMOS_PMPA.NUR_ER_EMIH"))
                    {
                        //20-04-10 전산실 한기호 선생님 요청으로 주석처리
                        //#region insert
                        //SQL = " INSERT INTO " + strTable;
                        //SQL += ComNum.VBLF + " SELECT * FROM " + strTable;
                        //SQL += ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        //SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                        //if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        //{
                        //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        //    clsDB.setRollbackTran(clsDB.DbCon);
                        //    return;
                        //}
                        //#endregion

                        //#region UPDATE
                        //SQL = " UPDATE " + strTable + " SET ";
                        //SQL += ComNum.VBLF + strColumn + "= '" + TxtNewPano.Text.Trim() + "', ";
                        //SQL += ComNum.VBLF + "  GBSEND = NULL ";
                        //SQL += ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        //SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                        //if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        //{
                        //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        //    clsDB.setRollbackTran(clsDB.DbCon);
                        //    return;
                        //}
                        //#endregion

                        //if (strTable.Trim().ToUpper().Equals("KOSMOS_PMPA.NUR_ER_EMIHPTMI"))
                        //{
                        //    string strINDT = string.Empty;
                        //    string strINTM = string.Empty;

                        //    SQL = " SELECT PTMIINDT, PTMIINTM ";
                        //    SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI ";
                        //    SQL += ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        //    SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                        //    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        //    {
                        //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        //        clsDB.setRollbackTran(clsDB.DbCon);
                        //        return;
                        //    }

                        //    if (reader.HasRows && reader.Read())
                        //    {
                        //        strINDT = reader.GetValue(0).ToString().Trim();
                        //        strINTM = reader.GetValue(1).ToString().Trim();
                        //    }

                        //    reader.Dispose();


                        //    SQL = " UPDATE KOSMOS_PMPA.NUR_ER_EMIHPTMI SET ";
                        //    SQL += ComNum.VBLF + " PTMISTAT = 'D',";
                        //    SQL += ComNum.VBLF + " GBSEND = NULL ";
                        //    SQL += ComNum.VBLF + " WHERE PTMIIDNO = '" + TxtPano.Text.Trim() + "' ";
                        //    SQL += ComNum.VBLF + "   AND PTMIINDT = '" + strINDT + "' ";
                        //    SQL += ComNum.VBLF + "   AND PTMIINTM = '" + strINTM + "' ";

                        //    SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                        //    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        //    {
                        //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        //        clsDB.setRollbackTran(clsDB.DbCon);
                        //        return;
                        //    }
                        //}
                    }
                    else
                    {
                        SQL = " UPDATE " + strTable + " SET ";
                        SQL += ComNum.VBLF + strColumn + "= '" + (TxtNewPano.Text).Trim() + "' ";
                        SQL += ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                    }

                    SQL = " INSERT INTO ETC_PANO_HIS (BDATE,TO_PANO,PANO,TABLE_NAME,TABLE_ROWID) VALUES ( ";
                    SQL += ComNum.VBLF + " SYSDATE, '" + TxtPano.Text + "', '" + TxtNewPano.Text + "', ";
                    SQL += ComNum.VBLF + " '" + strTable + "', '" + strROWID + "') ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }

                #region 환자인적사항 변경 내역 백업
                ComFunc CF1 = new ComFunc();
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("SNAME", "이중차트");
                dict.Add("JUMIN2", "1000000");
                dict.Add("JUMIN3", clsAES.AES("1000000"));
                CF1.INSERT_BAS_PATIENT_HIS((TxtPano.Text).Trim(), dict);
                #endregion

                #region BAS_PATIENT UPDATE1
                SQL = " UPDATE BAS_PATIENT SET SNAME = '이중차트', ";
                SQL += ComNum.VBLF + " JUMIN2 = '1000000', ";
                SQL += ComNum.VBLF + " Jumin3 ='" + clsAES.AES("1000000") + "' ";// '2013-05-16
                SQL += ComNum.VBLF + " WHERE PANO  = '" + (TxtPano.Text).Trim() + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }
                #endregion

                #region HEA_PATIENT UPDATE(2015-04-07 건진센터 이양재과장 요청으로 종검/일반건진 환자마스타도 같이 변경함)
                SQL = " UPDATE HEA_PATIENT SET SNAME = '이중차트'";
                SQL += ComNum.VBLF + " WHERE PTNO  = '" + (TxtPano.Text).Trim() + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                SQL = " UPDATE HIC_PATIENT SET SNAME = '이중차트'";
                SQL += ComNum.VBLF + " WHERE PTNO  = '" + (TxtPano.Text).Trim() + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }
                #endregion

                #region 2010-12-06 단순 환자마스타만 변경해도 히스토리 남김
                if (SS1_Sheet1.NonEmptyRowCount == 0)
                {
                    SQL = " INSERT INTO ETC_PANO_HIS (BDATE,TO_PANO,PANO,TABLE_NAME) VALUES ( ";
                    SQL += " SYSDATE, '" + TxtPano.Text + "', '" + TxtNewPano.Text + "', ";
                    SQL += " '기본변경정보' ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                }
                #endregion

                clsDB.setCommitTran(clsDB.DbCon);

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                clsDB.setRollbackTran(clsDB.DbCon);
            }

            ComFunc.MsgBoxEx(this, "대단히 수고했습니다..!!", "확인");
            SS1_Sheet1.RowCount = 0;
        }

        private void TxtPano_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtPano.Text.Trim()))
                return;


        }

        private void TxtNewPano_Leave(object sender, EventArgs e)
        {
            //TxtNewPano.Text = VB.Val(TxtNewPano.Text.Trim()).ToString("00000000");
        }

        private void TxtNewPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                TxtNewPano.Text = VB.Val(TxtNewPano.Text.Trim()).ToString("00000000");

                lblName.Text = clsVbfunc.GetPatientName(clsDB.DbCon, TxtNewPano.Text);

                if (string.IsNullOrWhiteSpace(lblName.Text))
                {
                    ComFunc.MsgBoxEx(this, "등록번호가 없습니다. 등록번호를 다시 확인 요망!!");
                    lblName.Text = "외래환자X";
                }
            }
        }

        private void frmEmrJobConvPano_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (worker != null)
            {
                worker.CancelAsync();
                worker.Dispose();
                worker = null;
            }

            if (string.IsNullOrWhiteSpace(TxtPano.Text.Trim()) == false)
            {
                clsLockCheck.IpdOcs_Lock_Delete_NEW(TxtPano.Text.Trim());
            }

            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }

        private void frmEmrJobConvPano_Activated(object sender, EventArgs e)
        {
            if (LoadActiveForm == false)
            {
                //ComFunc.Delay(500);
                //GetSearchData2();
                //LoadActiveForm = true;
            }

            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void TxtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (SSTables_Sheet1.RowCount == 0)
                return;

            TxtPano.Text = VB.Val(TxtPano.Text.Trim()).ToString("00000000");

            lblNameOLD.Text = clsVbfunc.GetPatientName(clsDB.DbCon, TxtPano.Text);
        }

        private void TxtPano_Click(object sender, EventArgs e)
        {
            lblNameOLD.Text = "";
        }

        private void TxtNewPano_Click(object sender, EventArgs e)
        {
            lblName.Text = "";

        }
    }
}
