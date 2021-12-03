using System;
using System.Data;
using System.Windows.Forms;
using ComBase;

namespace ComEmrBase
{
    /// <summary>
    /// BST Interface
    /// </summary>
    public partial class frmAutoBSTInterface : Form
    {
        EmrPatient AcpEmr = null;
        int nCnt = 0;

        public frmAutoBSTInterface()
        {
            InitializeComponent();
        }

        private void frmBSTInterface_Load(object sender, EventArgs e)
        {
            ss1_Sheet1.RowCount = 0;
        }

        #region 함수
        /// <summary>
        /// 일괄 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveDataAll()
        {

            bool rtnVal = false;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;
            int AllRowAffected = 0;
            DataTable dt = null;

            EmrForm ppForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, "1572");

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = "SELECT B.NURSE_ID, MEASURE_DT, B.VALUE, S.SPECNO, S.PANO, TO_CHAR(I.INDATE, 'YYYYMMDD') MEDFRDATE, I.DEPTCODE, TO_CHAR(B.EMR, 'YYYY-MM-DD') EMR, I.WARDCODE";
                SQL = SQL + ComNum.VBLF + " , (SELECT TO_CHAR(SYSDATE, 'YYYY-MM-DD') FROM DUAL) SYSDATE";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_INTERFACE_BST B  ";
                //SQL = SQL + ComNum.VBLF + "   INNER JOIN ADMIN.EXAM_SPECMST S ";
                //SQL = SQL + ComNum.VBLF + "      ON S.SPECNO = B.PATIENT_ID";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN ADMIN.IPD_NEW_MASTER I ";
                SQL = SQL + ComNum.VBLF + "      ON (I.JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD') OR I.JDATE = TRUNC(SYSDATE))";
                SQL = SQL + ComNum.VBLF + "     AND I.GBSTS <> '7'";
                SQL = SQL + ComNum.VBLF + "     AND I.PANO = B.PANO";
                SQL = SQL + ComNum.VBLF + "WHERE MEASURE_DT >= TO_CHAR(SYSDATE - 1, 'YYYYMMDD') || '000000'";
                SQL = SQL + ComNum.VBLF + "  AND MEASURE_DT <= TO_CHAR(SYSDATE, 'YYYYMMDD') || '235900'";
                SQL = SQL + ComNum.VBLF + "  AND B.WARD IN (I.WARDCODE || 'W1', I.WARDCODE || 'W2')";
                SQL = SQL + ComNum.VBLF + "  AND B.EMR IS NULL";
                SQL = SQL + ComNum.VBLF + "ORDER BY MEASURE_DT DESC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return false;
                }

                if (dt.Rows.Count == 0)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return false;
                }


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim(), "I", dt.Rows[i]["MEDFRDATE"].ToString().Trim(), dt.Rows[i]["DEPTCODE"].ToString().Trim());
                    if (AcpEmr == null)
                    {
                        //ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                        //clsDB.setRollbackTran(clsDB.DbCon);
                        continue;
                    }

                 
                    //20200810093944

                    string strChartDate = dt.Rows[i]["MEASURE_DT"].ToString().Substring(0, 8);
                    string strChartTime = dt.Rows[i]["MEASURE_DT"].ToString().Substring(8, 6);

                    double dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "GETEMRXMLNO"));
                    double dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO"));

                    string SaveIdNumber = dt.Rows[i]["NURSE_ID"].ToString().Trim();
                    string CertIdNumber = dt.Rows[i]["NURSE_ID"].ToString().Trim().PadLeft(5, '0');

                    #region //저장 CHRATMAST
                    string strSaveFlag = string.Empty;
                    DateTime dtpSys = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));

                    if (clsEmrQuery.SaveChartMstOnly(clsDB.DbCon, dblEmrNoNew, dblEmrHisNo, AcpEmr, ppForm.FmFORMNO.ToString(), ppForm.FmUPDATENO.ToString(),
                                        strChartDate, strChartTime,
                                        SaveIdNumber, SaveIdNumber, "1", "1", "0",
                                        dtpSys.ToString("yyyyMMdd"), dtpSys.ToString("HHmmss"), strSaveFlag) == false)
                    {
                        ComFunc.MsgBoxEx(this, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    #endregion //저장 CHRATMAST

                    #region  //저장 CHARTROW
                    string strItemCd = "I0000009122"; //Glucose;
                    string ItemValue = dt.Rows[i]["VALUE"].ToString();

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
                    SQL += ComNum.VBLF + "'" + SaveIdNumber + "',";   //INPUSEID
                    SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'YYYYMMDD'), ";   //INPDATE
                    SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'HH24MISS') ";   //INPTIME
                    SQL += ComNum.VBLF + ")";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                        return false;
                    }

                    AllRowAffected += RowAffected;
                    #endregion //CHARTROW

                    #region 인터페이스 테이블 EMR 연동 시간 업데이트.
                    SQL = string.Empty;
                    SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_MED + "EXAM_INTERFACE_BST ";
                    SQL += ComNum.VBLF + "SET ";
                    SQL += ComNum.VBLF + "EMR = SYSDATE";
                    SQL += ComNum.VBLF + "WHERE PATIENT_ID = '" + dt.Rows[i]["SPECNO"].ToString() + "'";   //PATIENT_ID
                    SQL += ComNum.VBLF + "  AND MEASURE_DT = '" + dt.Rows[i]["MEASURE_DT"].ToString() + "'";   //MEASURE_DT

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, "인터페이스 테이블에 차트 시간을 기록 하는 도중 에러가 발생했습니다.");
                        return false;
                    }
                    #endregion

                    #region 전자인증
                    bool blnCert = true;
                    if (dblEmrNoNew > 0)
                    {
                        if (clsCertWork.Cert_Check(clsDB.DbCon, CertIdNumber) == true)
                        {
                            blnCert = clsEmrQuery.SaveEmrCert(clsDB.DbCon, dblEmrNoNew, false);
                        }
                    }
                    #endregion

                    #region 2020-08-28 안정수, 오더데이터 발생 추가 
                    if (clsEmrQuery.Ins_OCS_IORDER_BST(AcpEmr, dt.Rows[i]["SYSDATE"].ToString(),dt.Rows[i]["MEASURE_DT"].ToString()) == false)
                    {
                        ComFunc.MsgBoxEx(this, "OCS_IORDER(BST) 저장 중 에러가 발생했습니다.");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    #endregion

                    ss1_Sheet1.RowCount += 1;
                    ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 0].Text = DateTime.ParseExact(dt.Rows[i]["MEASURE_DT"].ToString(), "yyyyMMdd", null).ToString("yyyy-MM-dd HH:mm");
                    ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["NURSE_ID"].ToString().Trim();
                    ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 3].Text = "Y";
                }

                dt.Dispose();

                clsDB.setCommitTran(clsDB.DbCon);
                //ComFunc.MsgBoxEx(this, "총 " + AllRowAffected + "개의 기록지가 저장되었습니다.");
            }
            catch (Exception ex)
            {
                ss1_Sheet1.RowCount += 1;
                ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 0].Text = DateTime.Now.ToLongDateString();
                ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 1].Text = "에러 발생";
                ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 2].Text = "에러 발생";
                ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 3].Text = ex.Message;

                //ComFunc.MsgBoxEx(this, "당뇨기록지를 저장 하는 도중 에러가 발생했습니다.");
                clsDB.SaveSqlErrLog(ex.Message, "BSTInterface - SaveData", clsDB.DbCon);
                clsDB.setRollbackTran(clsDB.DbCon);
            }

            return rtnVal;
        }

        #endregion

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            lblCnt.Text = nCnt.ToString();
            nCnt += 1;

            if (nCnt >= 5)
            {
                timer1.Stop();
                SaveDataAll();
                timer1.Start();

                nCnt = 0;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ss1_Sheet1.RowCount = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
    }
}
