using ComBase;
using ComBase.Controls;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrBaseEmrCertify : Form, FormEmrMessage
    {
        #region 스프레드
        private enum spdList
        {
            /// <summary>
            /// 체크박스
            /// </summary>
            CheckBox,
            /// <summary>
            /// 입원/외래 
            /// </summary>
            INOUTCLS,
            /// <summary>
            /// 등록번호
            /// </summary>
            PTNO,
            /// <summary>
            /// 환자이름
            /// </summary>
            SNAME,
            /// <summary>
            /// 과
            /// </summary>
            MEDDEPTCD,
            /// <summary>
            /// 작성날짜
            /// </summary>
            CHARTDATE,
            /// <summary>
            /// 작성시간
            /// </summary>
            CHARTTIME,
            /// <summary>
            /// 기록지이름
            /// </summary>
            FORMNAME,
            /// <summary>
            /// 외래(내원일), 입원(입원일)
            /// </summary>
            MEDFRDATE,
            /// <summary>
            /// EMRNO
            /// </summary>
            EMRNO,
            /// <summary>
            /// HISTORYNO
            /// </summary>
            EMRNOHIS,
            /// <summary>
            /// 기록지번호
            /// </summary>
            FORMNO,
            /// <summary>
            /// 기록지 업데이트번호
            /// </summary>
            UPDATENO,
            /// <summary>
            /// 저장 결과
            /// </summary>
            RESULT
        }
        #endregion

        #region FormEmrMessage
        public void MsgSave(string strSaveFlag)
        {
        }

        public void MsgDelete()
        {
        }

        public void MsgClear()
        {
        }

        public void MsgPrint()
        {
        }
        #endregion

        #region 생성자
        public frmEmrBaseEmrCertify()
        {
            InitializeComponent();
        }
        #endregion

        /// <summary>
        /// 임시저장 폼
        /// </summary>
        Form frmEmrForm = null;

        #region 폼 이벤트

        private void frmEmrBaseEmrCertify_Load(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetSearchData();
        }                

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData())
            {
                //ComFunc.MsgBoxEx(this, "저장되었습니다.");
                GetSearchData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region 함수

        private bool SaveData()
        {
            #region 변수
            bool rtnVal = false;
            SheetView sheetView = tabControl1.SelectedIndex == 0 ? ssTemp_Sheet1 : ssCertifyErr_Sheet1;
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty;
            //frmEmrChartNew frmEmrForm = null;
            #endregion

            if (sheetView.RowCount == 0)
                return rtnVal;

            if (tabControl1.SelectedIndex == 0)
            {
                ComFunc.MsgBoxEx(this, "임시저장 하신 차트는 항목 클릭후 차트화면에서 저장해주세요.");
                return rtnVal;
            }

            //clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                for(int i = 0; i < sheetView.RowCount; i++)
                {
                    if (sheetView.Cells[i, 0].Text.Trim().Equals("True"))
                    {
                        if (clsCertWork.Cert_Check(clsDB.DbCon, clsType.User.Sabun) == true)
                        {
                            string strPTNO = ssTemp_Sheet1.Cells[i, (int)spdList.PTNO].Text.Trim();
                            string strInOutCls = ssTemp_Sheet1.Cells[i, (int)spdList.INOUTCLS].Text.Trim();
                            string strMedDeptCd = ssTemp_Sheet1.Cells[i, (int)spdList.MEDDEPTCD].Text.Trim();
                            string strMedFrDate = ssTemp_Sheet1.Cells[i, (int)spdList.MEDFRDATE].Text.Trim();
                            string strFormNo = ssTemp_Sheet1.Cells[i, (int)spdList.FORMNO].Text.Trim();
                            string strUpdateNo = ssTemp_Sheet1.Cells[i, (int)spdList.UPDATENO].Text.Trim();

                            string strEmrNo = ssTemp_Sheet1.Cells[i, (int)spdList.EMRNO].Text.Trim();
                            sheetView.Cells[i, (int)spdList.RESULT].Text =  clsEmrQuery.SaveEmrCert(clsDB.DbCon, VB.Val(strEmrNo)) ? "성공" : "실패";
                        }
                    }
                }

                //clsDB.setCommitTran(clsDB.DbCon);
                rtnVal = true;
            }
            catch (Exception ex)
            {
                //clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return rtnVal;
        }

        private void GetSearchData()
        {
            ssTemp_Sheet1.RowCount = 0;
            ssCertifyErr_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            #region 쿼리 
            StringBuilder SQL = new StringBuilder();
            #region 전자인증시 에러
            SQL.AppendLine("SELECT '에러' AS GBN, EMRNO, EMRNOHIS, A.INOUTCLS, PTNO, P.SNAME, MEDFRDATE, CHARTUSEID, CHARTDATE, CHARTTIME, A.FORMNO, A.UPDATENO, B.FORMNAME");
            SQL.AppendLine("  FROM ADMIN.AEMRCHARTMST A");
            SQL.AppendLine("    INNER JOIN ADMIN.AEMRFORM B");
            SQL.AppendLine("       ON A.FORMNO = B.FORMNO");
            SQL.AppendLine("      AND A.UPDATENO = B.UPDATENO");
            SQL.AppendLine("    INNER JOIN ADMIN.BAS_PATIENT P");
            SQL.AppendLine("       ON A.PTNO = P.PANO");
            SQL.AppendLine("WHERE CHARTUSEID = '" + clsType.User.IdNumber + "'");
            SQL.AppendLine("  AND CHARTDATE > CHR(0)");
            SQL.AppendLine("  AND A.FORMNO > 0");
            SQL.AppendLine("  AND SAVECERT = '1'");
            SQL.AppendLine("  AND CERTNO IS NULL");
            SQL.AppendLine("  AND SUBSTR(A.PTNO, 0, 4) <> '8100'");
            #endregion

            #region 임시저장
            SQL.AppendLine("UNION ALL");
            SQL.AppendLine("SELECT '임시저장' AS GBN, EMRNO, EMRNOHIS, A.INOUTCLS, PTNO, P.SNAME, MEDFRDATE, CHARTUSEID, CHARTDATE, CHARTTIME, A.FORMNO, A.UPDATENO, B.FORMNAME");
            SQL.AppendLine("  FROM ADMIN.AEMRCHARTMST A");
            SQL.AppendLine("    INNER JOIN ADMIN.AEMRFORM B");
            SQL.AppendLine("       ON A.FORMNO = B.FORMNO");
            SQL.AppendLine("      AND A.UPDATENO = B.UPDATENO");
            SQL.AppendLine("    INNER JOIN ADMIN.BAS_PATIENT P");
            SQL.AppendLine("       ON A.PTNO = P.PANO");
            SQL.AppendLine("WHERE CHARTUSEID = '" + clsType.User.IdNumber + "'");
            SQL.AppendLine("  AND CHARTDATE > CHR(0)");
            SQL.AppendLine("  AND A.FORMNO > 0");
            SQL.AppendLine("  AND SAVECERT = '0'");
            SQL.AppendLine("  AND SUBSTR(A.PTNO, 0, 4) <> '8100'");
            #endregion

            SQL.AppendLine("ORDER BY INOUTCLS, CHARTDATE, PTNO");
            #endregion

            DataTable dt = null;
            string SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon);
                ComFunc.MsgBoxEx(this, SqlErr);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SheetView sheetView = dt.Rows[i]["GBN"].ToString().Trim().Equals("에러") ? ssCertifyErr_Sheet1 : ssTemp_Sheet1;

                    sheetView.RowCount += 1;
                    sheetView.Cells[sheetView.RowCount - 1, (int) spdList.INOUTCLS].Text = dt.Rows[i]["INOUTCLS"].ToString().Trim();
                    sheetView.Cells[sheetView.RowCount - 1, (int) spdList.PTNO].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                    sheetView.Cells[sheetView.RowCount - 1, (int) spdList.SNAME].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    sheetView.Cells[sheetView.RowCount - 1, (int) spdList.CHARTDATE].Text = dt.Rows[i]["CHARTDATE"].ToString().Trim().To<int>().ToString("0000-00-00");
                    sheetView.Cells[sheetView.RowCount - 1, (int) spdList.CHARTTIME].Text = dt.Rows[i]["CHARTTIME"].ToString().Trim().Substring(0, 4).To<int>().ToString("00:00");
                    sheetView.Cells[sheetView.RowCount - 1, (int) spdList.FORMNAME].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                    sheetView.Cells[sheetView.RowCount - 1, (int) spdList.MEDFRDATE].Text = dt.Rows[i]["MEDFRDATE"].ToString().Trim();
                    sheetView.Cells[sheetView.RowCount - 1, (int) spdList.EMRNO].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                    sheetView.Cells[sheetView.RowCount - 1, (int)spdList.EMRNOHIS].Text = dt.Rows[i]["EMRNOHIS"].ToString().Trim();
                    sheetView.Cells[sheetView.RowCount - 1, (int) spdList.FORMNO].Text = dt.Rows[i]["FORMNO"].ToString().Trim();
                    sheetView.Cells[sheetView.RowCount - 1, (int)spdList.UPDATENO].Text = dt.Rows[i]["UPDATENO"].ToString().Trim();
                }
            }

            dt.Dispose();
            Cursor.Current = Cursors.Default;
        }
        #endregion

        private void ssTemp_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (frmEmrForm != null)
            {
                frmEmrForm.Dispose();
                frmEmrForm = null;
            }

            string strPTNO = ssTemp_Sheet1.Cells[e.Row, (int)spdList.PTNO].Text.Trim();
            string strInOutCls = ssTemp_Sheet1.Cells[e.Row, (int)spdList.INOUTCLS].Text.Trim();
            string strMedDeptCd = ssTemp_Sheet1.Cells[e.Row, (int)spdList.MEDDEPTCD].Text.Trim();
            string strMedFrDate = ssTemp_Sheet1.Cells[e.Row, (int)spdList.MEDFRDATE].Text.Trim();
            string strFormNo = ssTemp_Sheet1.Cells[e.Row, (int)spdList.FORMNO].Text.Trim();
            string strUpdateNo = ssTemp_Sheet1.Cells[e.Row, (int)spdList.UPDATENO].Text.Trim();
            string strEmrNo = ssTemp_Sheet1.Cells[e.Row, (int)spdList.EMRNO].Text.Trim();

            EmrPatient tAcp = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPTNO, strInOutCls, strMedFrDate, strInOutCls.Equals("I") ? "" : strMedDeptCd);
            EmrForm tForm = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, strFormNo, strUpdateNo);

            //입원 환자이고 과가 다를경우 과 강제로 수정
            if (strInOutCls.Equals("I") && strMedDeptCd.Equals(tAcp.medDeptCd) == false)
            {
                tAcp.medDeptCd = strMedDeptCd;
            }

            if (tForm.FmFORMTYPE == "4") //개발자가 만든것
            {
                //clsFormMap.EmrFormMapping("MHENRINS", strNameSpace, frmFORM.FmPROGFORMNAME, strFormNo, strUpdateNo, pEmrPatient, strEmrNo, "W");
                frmEmrForm = clsEmrFormMap.EmrFormMappingEx(tForm.FmPROGFORMNAME, tForm.FmFORMNO.ToString(), tForm.FmUPDATENO.ToString(), tAcp, strEmrNo, "V", this);
            }
            //else if (tForm.FmFORMTYPE == "3") //Flow
            //{
            //    frmEmrForm = new frmEmrChartFlowOld(tForm.FmFORMNO.ToString(), tForm.FmUPDATENO.ToString(), tAcp, strEmrNo, "V", "", strInOutCls, this);
            //    //ActiveFormView = new frmEmrPrintFlowSheet(tForm.FmFORMNO.ToString(), tForm.FmUPDATENO.ToString(), tAcp, strEmrNo, "V", strVal, this);
            //}
            else if (tForm.FmFORMTYPE == "2") //전자동의서
            {
                frmEmrForm = new frmEmrChartNew(tForm.FmFORMNO.ToString(), tForm.FmUPDATENO.ToString(), tAcp, strEmrNo, "V", this);
            }
            else if (tForm.FmFORMTYPE == "1") //동의서
            {
                frmEmrForm = new frmEmrChartNew(tForm.FmFORMNO.ToString(), tForm.FmUPDATENO.ToString(), tAcp, strEmrNo, "V", this);
            }
            else if (tForm.FmFORMTYPE == "0") //정형화 서식
            {
                frmEmrForm = new frmEmrChartNew(tForm.FmFORMNO.ToString(), tForm.FmUPDATENO.ToString(), tAcp, strEmrNo, "V", this);
            }

            frmEmrForm.TopLevel = false;
            frmEmrForm.Parent = panForm;
            frmEmrForm.FormBorderStyle = FormBorderStyle.None;
            frmEmrForm.ControlBox = false;
            frmEmrForm.Show();
        }

        private void frmEmrBaseEmrCertify_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmEmrForm != null)
            {
                frmEmrForm.Dispose();
                frmEmrForm = null;
            }
        }

        private void ssCertifyErr_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader)
            {
                clsSpread.gSpdSortRow(ssCertifyErr, e.Column);
                return;
            }
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            ssCertifyErr_Sheet1.Cells[0, 0, ssCertifyErr_Sheet1.RowCount - 1, 0].Text = chkAll.Checked.ToString();
        }
    }
}
