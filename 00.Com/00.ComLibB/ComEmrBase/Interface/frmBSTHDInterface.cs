using System;
using System.Data;
using System.Windows.Forms;
using ComBase;
using ComBase.Controls;
using Oracle.ManagedDataAccess.Client;

namespace ComEmrBase
{
    /// <summary>
    /// BST Interface
    /// </summary>
    public partial class frmBSTHDInterface : Form
    {
        EmrPatient AcpEmr = null;

        public frmBSTHDInterface()
        {
            InitializeComponent();
        }

        private void frmBSTHDInterface_Load(object sender, EventArgs e)
        {
            ssChart_Sheet1.RowCount = 0;
            GetBSTList();
        }

        #region 함수

     

        /// <summary>
        /// BST 검사 결과를 가지고 온다.
        /// </summary>
        void GetBSTList()
        {
            DataTable dt = null;
            string SQL = string.Empty;

            try
            {
                ssChart_Sheet1.RowCount = 0;


                SQL = "SELECT S.SPECNO, S.PANO, S.SNAME, S.AGE, S.SEX, B.MEASURE_DT, B.VALUE,  TO_CHAR(B.EMR, 'YYYY-MM-DD') EMR";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_SPECMST  S  ";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN ADMIN.EXAM_INTERFACE_BST B";
                SQL = SQL + ComNum.VBLF + "      ON S.SPECNO = B.PATIENT_ID";                
                SQL = SQL + ComNum.VBLF + " WHERE S.BDATE = TO_DATE('" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "     AND MEASURE_DT >= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "000000'";
                SQL = SQL + ComNum.VBLF + "     AND MEASURE_DT <= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "235959'";
                SQL = SQL + ComNum.VBLF + "     AND B.WARD LIKE '%HD%'";
                SQL = SQL + ComNum.VBLF + " ORDER BY MEASURE_DT DESC";

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt == null)
                    return;

                if (dt.Rows.Count > 0)
                {
                    ssChart_Sheet1.RowCount = dt.Rows.Count;
                    ssChart_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DateTime dtpDT = DateTime.ParseExact(dt.Rows[i]["MEASURE_DT"].ToString(), "yyyyMMddHHmmss", null);

                        ssChart_Sheet1.Cells[i, 0].Locked = !string.IsNullOrWhiteSpace(dt.Rows[i]["EMR"].ToString().Trim());

                        ssChart_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SPECNO"].ToString().Trim();
                        ssChart_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssChart_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssChart_Sheet1.Cells[i, 4].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        ssChart_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        ssChart_Sheet1.Cells[i, 6].Text = dtpDT.ToString("yyyy-MM-dd");
                        ssChart_Sheet1.Cells[i, 7].Text = dtpDT.ToString("HH:mm");
                        ssChart_Sheet1.Cells[i, 7].Tag = dtpDT.ToString("yyyyMMddHHmmss");
                        ssChart_Sheet1.Cells[i, 8].Text = dt.Rows[i]["VALUE"].ToString().Trim();
                        ssChart_Sheet1.Cells[i, 9].Text = dt.Rows[i]["EMR"].ToString().Trim();
                        
                    }
                }
                dt.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, "BST Interface GetBSTList()", clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        /// <summary>
        /// 해당일자에 BST 검사 결과 갯수를 가져온다.
        /// </summary>
        private void GetBstCount()
        {
            DataTable dt = null;
            string SQL = string.Empty;

            try
            {
                ssChart_Sheet1.RowCount = 0;

            
                //SQL = "SELECT MEASURE_DT, B.VALUE, S.SPECNO, TO_CHAR(B.EMR, 'YYYY-MM-DD') EMR";
                SQL = "SELECT 1";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_INTERFACE_BST B";
                SQL = SQL + ComNum.VBLF + "WHERE MEASURE_DT >= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "000000'";
                SQL = SQL + ComNum.VBLF + "  AND MEASURE_DT <= '" + dtpExamDate.Value.ToString("yyyyMMdd") + "235959'";
                SQL = SQL + ComNum.VBLF + "  AND B.WARD LIKE '%HD%'";
                SQL = SQL + ComNum.VBLF + "  AND B.KIND IS NULL";
                

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt == null)
                    return;

                if (dt.Rows.Count > 0)
                {
                    Text = dtpExamDate.Value.ToString("yyyy-MM-dd") + " 일자의 총 검사 갯수 : " + dt.Rows.Count;
                }
                dt.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, "BST Interface GetBstCount()", clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

    

        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveData()
        {
            bool rtnVal = true;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;
            //OracleDataReader reader = null;

            EmrForm ppForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, "1572");
            EmrPatient AcpEmr2 = null;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {

                for(int i = 0; i < ssChart_Sheet1.RowCount; i ++)
                {
                    if (string.IsNullOrEmpty(ssChart_Sheet1.Cells[i, 9].Text.Trim()) == true && ssChart_Sheet1.Cells[i, 0].Text.Trim().Equals("True"))
                    {
                        AcpEmr2 = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, ssChart_Sheet1.Cells[i, 2].Text.Trim(), "O", ssChart_Sheet1.Cells[i, 7].Tag.ToString().Substring(0, 8), "HD");

                        if (AcpEmr2 == null)
                        {
                            ComFunc.MsgBoxEx(this, "환자 접수내역을 찾을 수 없습니다.");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return false;
                        }
                        if (AcpEmr2.age.IndexOf("개월") != -1)
                        {
                            AcpEmr2.age = "0";
                        }

                        double dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "GETEMRXMLNO"));
                        double dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO"));

                        string strChartDate = ssChart_Sheet1.Cells[i, 7].Tag.ToString().Substring(0, 8);
                        string strChartTime = ssChart_Sheet1.Cells[i, 7].Tag.ToString().Substring(8, 6);

                        #region 해당 날짜/시간에 작성된 기록지 있는지 확인
                        //SQL = "SELECT 1 ";
                        //SQL += ComNum.VBLF + "FROM ADMIN.AEMRCHARTMST";
                        //SQL += ComNum.VBLF + "WHERE MEDFRDATE = '" + AcpEmr.medFrDate + "'";
                        //SQL += ComNum.VBLF + "  AND FORMNO = 1572";
                        //SQL += ComNum.VBLF + "  AND CHARTDATE = '" + strChartDate + "'";
                        //SQL += ComNum.VBLF + "  AND CHARTTIME = '" + strChartTime + "'";

                        //SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                        //if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        //{
                        //    ComFunc.MsgBoxEx(this, "기록지 작성여부 확인 도중 에러가 발생했습니다.");
                        //    clsDB.setRollbackTran(clsDB.DbCon);
                        //    Cursor.Current = Cursors.Default;
                        //    return false;
                        //}

                        //if (reader.HasRows)
                        //{
                        //    reader.Dispose();
                        //    continue;
                        //}

                        //reader.Dispose();
                        #endregion

                        #region //저장 CHRATMAST
                        string strSaveFlag = string.Empty;
                        DateTime dtpSys = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));

                        if (clsEmrQuery.SaveChartMstOnly(clsDB.DbCon, dblEmrNoNew, dblEmrHisNo, AcpEmr2, ppForm.FmFORMNO.ToString(), ppForm.FmUPDATENO.ToString(),
                                            strChartDate, strChartTime,
                                            clsType.User.IdNumber, clsType.User.IdNumber, "1", "1", "0",
                                            dtpSys.ToString("yyyyMMdd"), dtpSys.ToString("HHmmss"), strSaveFlag) == false)
                        {
                            ComFunc.MsgBoxEx(this, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                        else
                        {

                            #region  //저장 CHARTROW
                            string strItemCd = "I0000009122"; //Glucose;
                            string ItemValue = ssChart_Sheet1.Cells[i, 8].Text.Trim();

                            SQL = string.Empty;
                            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                            SQL += ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                            SQL += ComNum.VBLF + "VALUES (";
                            SQL += ComNum.VBLF + dblEmrNoNew.ToString() + ",";    //EMRNO
                            SQL += ComNum.VBLF + dblEmrHisNo.ToString() + ",";    //EMRNOHIS
                            SQL += ComNum.VBLF + "'" + strItemCd + "',";   //ITEMCD
                            SQL += ComNum.VBLF + "'" + (strItemCd.IndexOf("_") != -1 ? strItemCd.Split('_')[0] : strItemCd) + "',"; //ITEMNO
                            SQL += ComNum.VBLF + "-1,"; //ITEMINDEX
                            SQL += ComNum.VBLF + "'TEXT',";   //ITEMTYPE
                            SQL += ComNum.VBLF + "'" + ItemValue + "',";   //ITEMVALUE
                            SQL += ComNum.VBLF + "0,";   //DSPSEQ
                            SQL += ComNum.VBLF + "'',";   //ITEMVALUE1
                            SQL += ComNum.VBLF + "'" + clsType.User.IdNumber + "',";   //INPUSEID
                            SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'YYYYMMDD'), ";   //INPDATE
                            SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'HH24MISS') ";   //INPTIME
                            SQL += ComNum.VBLF + ")";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBoxEx(this, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                                return false;
                            }
                            #endregion //CHARTROW

                            #region 인터페이스 테이블 EMR 연동 시간 업데이트.
                            if (RowAffected > 0)
                            {
                                SQL = string.Empty;
                                SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_MED + "EXAM_INTERFACE_BST ";
                                SQL += ComNum.VBLF + "SET ";
                                SQL += ComNum.VBLF + "EMR = SYSDATE";
                                SQL += ComNum.VBLF + "WHERE PATIENT_ID  = '" + ssChart_Sheet1.Cells[i, 1].Text.ToString().Trim() + "'";   //PATIENT_ID
                                SQL += ComNum.VBLF + "  AND MEASURE_DT = '" + ssChart_Sheet1.Cells[i, 7].Tag.ToString() + "'";   //MEASURE_DT
                                                                                                                                 //SQL += ComNum.VBLF + "  AND VALUE      = '" + ItemValue + "'";   //MEASURE_DT

                                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBoxEx(this, "인터페이스 테이블에 차트 시간을 기록 하는 도중 에러가 발생했습니다.");
                                    return false;
                                }

                                #region 전자인증
                                if (clsCertWork.Cert_Check(clsDB.DbCon, clsType.User.Sabun) == true)
                                {
                                    clsEmrQuery.SaveEmrCert(clsDB.DbCon, dblEmrNoNew, false);
                                }
                                #endregion

                          
                            }
                            #endregion
                        }
                        #endregion //저장 CHRATMAST
                    } 
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch(Exception ex)
            {
                ComFunc.MsgBoxEx(this, "당뇨기록지를 저장 하는 도중 에러가 발생했습니다.");
                clsDB.SaveSqlErrLog(ex.Message, "BSTInterface - SaveData", clsDB.DbCon);
                clsDB.setRollbackTran(clsDB.DbCon);
                return false;
            }

            return rtnVal;
        }


        #endregion



        private void btnSearch_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            GetBstCount();
            GetBSTList();
            Cursor.Current = Cursors.Default;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (SaveData())
            {
                MessageBox.Show("저장이 완료 되었습니다.");
                GetBSTList();

            }
            Cursor.Current = Cursors.Default;
        }

      


        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

  

    }
}
