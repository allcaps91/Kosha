using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : EmrJob
    /// File Name       : frmEmrJobstatisticsTewon
    /// Description     : 미비 현황 통계(퇴원자)
    /// Author          : 전상원
    /// Create Date     : 2018-04-30
    /// Update History  : 2018-07-06(이현종)
    /// </summary>
    /// <history>  
    /// TODO : 폼 호출, 엑셀저장
    /// </history>
    /// <seealso cref= "\emr\BITNIXCHART\bitnixChart\bitnixChart.vbg(frmMibiStatistics.frm) >> frmEmrJobstatisticsTewon.cs 폼이름 재정의" />
    public partial class frmEmrJobMiBiStatisticsTewon : Form
    {
        private bool MedicalInfoTeam = false;
        private string strPano = string.Empty;

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

        public frmEmrJobMiBiStatisticsTewon(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }

        public frmEmrJobMiBiStatisticsTewon(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
        }

        public frmEmrJobMiBiStatisticsTewon()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 의료정보팀 통계용
        /// </summary>
        /// <param name="MedicalInfoTeam"></param>
        public frmEmrJobMiBiStatisticsTewon(bool MedicalInfoTeam)
        {
            InitializeComponent();
            this.MedicalInfoTeam = MedicalInfoTeam;
        }

        /// <summary>
        /// 의료정보팀 차트복사용
        /// 19-07-23 요구사항으로 추가.
        /// </summary>
        /// <param name="Pano"></param>
        public frmEmrJobMiBiStatisticsTewon(string Pano)
        {
            InitializeComponent();
            strPano = Pano;
        }


        private void frmEmrJobstatisticsTewon_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); 
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등

            FormClear(); //폼 클리어
            SET_COMBO_DEPT();

            //차트복사에서 미비현황 조회 하기 위해서 추가함
            //19-07-23
            if (strPano.Length > 0)
            {
                rdotabssView0.Checked = true;
                rdotabssView1.Checked = false;
                rdoSearchType0.Checked = true;
                panMibi2.Dispose();
                panMibi2 = null;
                ssStatisticsView1.Dispose();
                ssStatisticsView1 = null;
                #region 필요한 칼럼만 보이게 수정
                ssStatisticsView0_Sheet1.Columns[0, ssStatisticsView0_Sheet1.ColumnCount - 1].Visible = false;
                ssStatisticsView0_Sheet1.Columns[0, 6].Visible = true;
                ssStatisticsView0_Sheet1.Columns[9].Visible = true;
                ssStatisticsView0_Sheet1.Columns[13, 17].Visible = true;
                #endregion

                txtSearch.Text = strPano;
                GetPatientInfoSearch();
                btnSearch.PerformClick();
                return;
            }

            if (MedicalInfoTeam)
            {
                rdotabssView0.Checked = false;
                rdotabssView1.Checked = true;
                rdoSearchType4.Checked = true;
                dtpMiBiToDate2.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(-2);
                dtpMiBiFrDate2.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(-2);
                panMibi1.Dispose();
                panMibi1 = null;
                ssStatisticsView0.Dispose();
                ssStatisticsView0 = null;
                GetMibiStatistics2();
                return;
            }

            rdotabssView0.Checked = true;
            rdotabssView1.Checked = false;
            rdoSearchType3.Checked = true;
        }

        public void SetNewMiBi(string strPano)
        {
            txtSearch.Text = strPano;
            GetPatientInfoSearch();
            btnSearch.PerformClick();
        }

        private void FormClear()
        {
            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control ctl in controls)
            {
                if (ctl is TextBox)
                {
                    ((TextBox)ctl).Text = "";
                }
                else if (ctl is DateTimePicker)
                {
                    ((DateTimePicker)ctl).Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
                }
                else if (ctl is ComboBox)
                {
                    ((ComboBox)ctl).Items.Clear();
                }
                else if (ctl is CheckBox)
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl is FpSpread)
                {
                    ((FpSpread)ctl).ActiveSheet.RowCount = 0;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 조회
            }

            if (rdotabssView0.Checked == true)
            {
                GetMibiList(); //미비리스트 조회
            }
            else if (rdotabssView1.Checked == true)
            {
                if (rdoSearchType3.Checked == true)
                {
                    GetMibiStatistics1(); //미비 서식지별 조회
                }
                else if (rdoSearchType4.Checked == true)
                {
                    GetMibiStatistics2(); //미비 의사별 조회
                }
                else if (rdoSearchType9.Checked == true)
                {
                    GetMibiStatistics2_1(); //MD 통계 조회
                }
                else if (rdoSearchType8.Checked == true)
                {
                    GetMibiStatistics4(); //미비 과별 조회
                }
                else if (rdoSearchType5.Checked == true)
                {
                    GetMibiStatistics3(); //미비 월별통계 조회
                }
                else if (rdoSearchType10.Checked == true)
                {
                    GetMibiStatistics5(); //미비 누적
                }

                if(rdoSearchType5.Checked == false) setSSRowMaxHeight(ssStatisticsView1);
            }
        }

        /// <summary>
        /// 미비리스트 조회
        /// </summary>
        private void GetMibiList()
        {
            int i = 0;
            int nCol = 0;
            string strPtno1 = "";

            double nREAD = 0;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            double mHisMaxRowHeightOld = 0;
            double mHisMaxRowHeightNew = 0;

            string strPtno = "";
            string strMedFrDate = "";
            string strMedEndDate = "";
            string strMedDrCd = "";
            string strTabNo = "";
            string strMiBiFrDate = "";
            string strMiBiToDate = "";

            if (rdoSearchType0.Checked == true)
            {
                strPtno = (txtSearch.Text).Trim();
            }
            else if (rdoSearchType1.Checked == true)
            {
                strMedDrCd =txtSearch.Text.Trim();
            }
            else if (rdoSearchType2.Checked == true || rdoSearchType6.Checked == true || rdoSearchType7.Checked == true)
            {
                strMedDrCd = (txtSearch.Text).Trim();
                strMiBiFrDate = (dtpMiBiFrDate.Value).ToString("yyyyMMdd");
                strMiBiToDate = (dtpMiBiToDate.Value).ToString("yyyyMMdd");
            }

            ssStatisticsView0_Sheet1.RowCount = 0;

            //On Error GoTo ErrS

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DISTINCT M.WardCode,M.RoomCode,M.Pano,M.SName AS PTNAME,M.Sex,M.Age,D.NAME,";
                SQL = SQL + ComNum.VBLF + "             TO_CHAR(M.InDate,'YYYY-MM-DD') InDate,";
                SQL = SQL + ComNum.VBLF + "             TO_CHAR(M.OutDate,'YYYY-MM-DD') OutDate,";
                SQL = SQL + ComNum.VBLF + "             A.PTNO, A.MEDFRDATE, A.MEDENDDATE, A.MEDDEPTCD, A.MEDDRCD, A.MIBICLS, ";
                SQL = SQL + ComNum.VBLF + "             A.MIBITAB, A.MIBIGRP , A.MIBICD, A.MIBIRMK, A.MIBIFNDATE, M.DRCODE";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRMIBI A, " + ComNum.DB_PMPA + "BAS_PATIENT P,";
                SQL = SQL + ComNum.VBLF + "           " + ComNum.DB_PMPA + "IPD_NEW_MASTER  M, " + ComNum.DB_EMR + "EMR_USERT  D";
                SQL = SQL + ComNum.VBLF + "  WHERE A.PTNO = P.PANO";
                SQL = SQL + ComNum.VBLF + "  AND A.PTNO = M.PANO";

                if (chkER.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND M.AMSET7 IN ('3','4','5') ";
                }
                if (chkPOA.Checked == true)
                {
                    // SQL = SQL + ComNum.VBLF + "  AND A.MIBICD IN ('A14') ";
                    SQL = SQL + ComNum.VBLF + " AND EXISTS(select* from ADMIN.EMRMIBI AA where MIBICD IN ('A14') and AA.MIBICLS > 0  and aa.PTNO = a.PTNO and aa.MEDDRCD = a.MEDDRCD   and aa.MEDENDDATE = a.MEDENDDATE  ) ";

                }

                SQL = SQL + ComNum.VBLF + "  AND A.MEDENDDATE = TO_CHAR(M.OUTDATE, 'YYYYMMDD')";
                SQL = SQL + ComNum.VBLF + "  AND A.MEDDRCD = D.USERID";

                if(strPano.Length == 0)
                {
                    if (rdoSearchType6.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.MIBICLS = 1 ";
                    }
                    else if (rdoSearchType7.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.MIBICLS = 2 ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.MIBICLS > 0 ";
                    }
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.MIBICLS = 1 ";
                }
             

                if (strMedDrCd.Length > 0)
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.MEDDRCD = '" + strMedDrCd + "'";
                }
                if (rdoSearchType0.Checked == true && txtSearch.TextLength > 0)
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.PTNO = '" + strPtno + "'";
                }
                if (rdoSearchType2.Checked == true || rdoSearchType6.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.MEDENDDATE BETWEEN '" + strMiBiFrDate + "' AND '" + strMiBiToDate + "'";
                }
                else if (rdoSearchType7.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.MIBIFNDATE BETWEEN '" + strMiBiFrDate + "' AND '" + strMiBiToDate + "'";
                }
                if (VB.Left(cboDept.Text, 2) != "**")
                {
                    SQL = SQL + ComNum.VBLF + " AND A.MEDDEPTCD = '" + VB.Right(cboDept.Text, 2) + "' ";
                }
                SQL = SQL + ComNum.VBLF + " GROUP BY M.WardCode,M.RoomCode,M.Pano,M.SName,M.Sex,M.Age,D.NAME,";
                SQL = SQL + ComNum.VBLF + "                  TO_CHAR(M.InDate,'YYYY-MM-DD'),";
                SQL = SQL + ComNum.VBLF + "                  TO_CHAR(M.OutDate,'YYYY-MM-DD'),";
                SQL = SQL + ComNum.VBLF + "                  A.PTNO, A.MEDFRDATE, A.MEDENDDATE, A.MEDDEPTCD, A.MEDDRCD, A.MIBICLS,";
                SQL = SQL + ComNum.VBLF + "                  A.MIBITAB , A.MIBIGRP, A.MIBICD, A.MIBIRMK, A.MIBIFNDATE, M.DRCODE";
                //SQL = SQL + ComNum.VBLF + " ORDER BY M.SName, A.MEDDRCD, A.MEDENDDATE DESC, A.MEDFRDATE, A.MIBITAB, A.MIBIGRP, MIBICD";
                SQL = SQL + ComNum.VBLF + " ORDER BY M.SName, A.MEDDRCD, A.MEDENDDATE , MIBIRMK DESC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nREAD = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    progressBar1.Value = 0;
                    progressBar1.Maximum = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        Application.DoEvents();
                        progressBar1.Value = (i + 1);

                        if (strPtno1 != dt.Rows[i]["PTNO"].ToString().Trim() || strMedDrCd != dt.Rows[i]["MEDDRCD"].ToString().Trim() || strTabNo != dt.Rows[i]["MIBITAB"].ToString().Trim() || strMedFrDate != dt.Rows[i]["MEDFRDATE"].ToString().Trim() || strMedEndDate != dt.Rows[i]["MEDENDDATE"].ToString().Trim())
                        {
                            ssStatisticsView0_Sheet1.RowCount = ssStatisticsView0_Sheet1.RowCount + 1;

                            strPtno1 = dt.Rows[i]["PTNO"].ToString().Trim();
                            strTabNo = dt.Rows[i]["MIBITAB"].ToString().Trim();
                            strMedDrCd = dt.Rows[i]["MEDDRCD"].ToString().Trim();
                            strMedFrDate = dt.Rows[i]["MEDFRDATE"].ToString().Trim();
                            strMedEndDate = dt.Rows[i]["MEDENDDATE"].ToString().Trim();

                            ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                            ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNAME"].ToString().Trim();
                            ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["MEDFRDATE"].ToString().Trim();
                            ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["MEDENDDATE"].ToString().Trim();
                            ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["MEDDEPTCD"].ToString().Trim();
                            ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["NAME"].ToString().Trim();
                            ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, 6].Text = ReadBASDoctor(dt.Rows[i]["DRCODE"].ToString().Trim());
                            ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                            ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                            ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["MEDDEPTCD"].ToString().Trim();
                            ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, 10].Text = dt.Rows[i]["MEDDRCD"].ToString().Trim();
                            ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["SEX"].ToString().Trim();
                            ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, 12].Text = dt.Rows[i]["AGE"].ToString().Trim();
                            ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, 18].Text = dt.Rows[i]["MIBITAB"].ToString().Trim();
                            ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, 19].Text = dt.Rows[i]["MIBIFNDATE"].ToString().Trim();
                            ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, 20].Text = ReadJewonIlsu(dt.Rows[i]["PTNO"].ToString().Trim(), dt.Rows[i]["MEDENDDATE"].ToString().Trim());

                        }
                        else
                        {

                        }

                        switch (dt.Rows[i]["MIBIGRP"].ToString().Trim())
                        {
                            case "A":
                                nCol = 13;
                                if (VB.Trim(ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, nCol].Text) == "")
                                {
                                    ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, nCol].Text = GetMiBiNm(dt.Rows[i]["MIBICD"].ToString().Trim(), dt.Rows[i]["MIBIRMK"].ToString().Trim());
                                }
                                else
                                {
                                    ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, nCol].Text = ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, nCol].Text + ComNum.VBLF + GetMiBiNm(dt.Rows[i]["MIBICD"].ToString().Trim(), dt.Rows[i]["MIBIRMK"].ToString().Trim());
                                }

                                ssStatisticsView0_Sheet1.SetRowHeight(ssStatisticsView0_Sheet1.RowCount - 1, Convert.ToInt32(ssStatisticsView0_Sheet1.GetPreferredRowHeight(ssStatisticsView0_Sheet1.RowCount - 1)) + 10);
                                mHisMaxRowHeightNew = ssStatisticsView0_Sheet1.GetRowHeight(ssStatisticsView0_Sheet1.RowCount - 1);
                                if (mHisMaxRowHeightNew > mHisMaxRowHeightOld)
                                {
                                    mHisMaxRowHeightOld = mHisMaxRowHeightNew;
                                }
                                break;
                            case "B":
                                nCol = 14;
                                if (VB.Trim(ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, nCol].Text) == "")
                                {
                                    ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, nCol].Text = GetMiBiNm(dt.Rows[i]["MIBICD"].ToString().Trim(), dt.Rows[i]["MIBIRMK"].ToString().Trim());
                                }
                                else
                                {
                                    ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, nCol].Text = ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, nCol].Text + ComNum.VBLF + GetMiBiNm(dt.Rows[i]["MIBICD"].ToString().Trim(), dt.Rows[i]["MIBIRMK"].ToString().Trim());
                                }

                                ssStatisticsView0_Sheet1.SetRowHeight(ssStatisticsView0_Sheet1.RowCount - 1, Convert.ToInt32(ssStatisticsView0_Sheet1.GetPreferredRowHeight(ssStatisticsView0_Sheet1.RowCount - 1)) + 10);
                                mHisMaxRowHeightNew = ssStatisticsView0_Sheet1.GetRowHeight(ssStatisticsView0_Sheet1.RowCount - 1);
                                if (mHisMaxRowHeightNew > mHisMaxRowHeightOld)
                                {
                                    mHisMaxRowHeightOld = mHisMaxRowHeightNew;
                                }
                                break;
                            case "C":
                                nCol = 15;
                                if (VB.Trim(ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, nCol].Text) == "")
                                {
                                    ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, nCol].Text = GetMiBiNm(dt.Rows[i]["MIBICD"].ToString().Trim(), dt.Rows[i]["MIBIRMK"].ToString().Trim());
                                }
                                else
                                {
                                    ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, nCol].Text = ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, nCol].Text + ComNum.VBLF + GetMiBiNm(dt.Rows[i]["MIBICD"].ToString().Trim(), dt.Rows[i]["MIBIRMK"].ToString().Trim());
                                }

                                ssStatisticsView0_Sheet1.SetRowHeight(ssStatisticsView0_Sheet1.RowCount - 1, Convert.ToInt32(ssStatisticsView0_Sheet1.GetPreferredRowHeight(ssStatisticsView0_Sheet1.RowCount - 1)) + 10);
                                mHisMaxRowHeightNew = ssStatisticsView0_Sheet1.GetRowHeight(ssStatisticsView0_Sheet1.RowCount - 1);
                                if (mHisMaxRowHeightNew > mHisMaxRowHeightOld)
                                {
                                    mHisMaxRowHeightOld = mHisMaxRowHeightNew;
                                }
                                break;
                            case "D":
                                nCol = 16;
                                if (VB.Trim(ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, nCol].Text) == "")
                                {
                                    ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, nCol].Text = GetMiBiNm(dt.Rows[i]["MIBICD"].ToString().Trim(), dt.Rows[i]["MIBIRMK"].ToString().Trim());
                                }
                                else
                                {
                                    ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, nCol].Text = ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, nCol].Text + ComNum.VBLF + GetMiBiNm(dt.Rows[i]["MIBICD"].ToString().Trim(), dt.Rows[i]["MIBIRMK"].ToString().Trim());
                                }

                                ssStatisticsView0_Sheet1.SetRowHeight(ssStatisticsView0_Sheet1.RowCount - 1, Convert.ToInt32(ssStatisticsView0_Sheet1.GetPreferredRowHeight(ssStatisticsView0_Sheet1.RowCount - 1)) + 10);
                                mHisMaxRowHeightNew = ssStatisticsView0_Sheet1.GetRowHeight(ssStatisticsView0_Sheet1.RowCount - 1);
                                if (mHisMaxRowHeightNew > mHisMaxRowHeightOld)
                                {
                                    mHisMaxRowHeightOld = mHisMaxRowHeightNew;
                                }
                                break;
                            case "E":
                                nCol = 17;
                                if (VB.Trim(ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, nCol].Text) == "")
                                {
                                    ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, nCol].Text = GetMiBiNm(dt.Rows[i]["MIBICD"].ToString().Trim(), dt.Rows[i]["MIBIRMK"].ToString().Trim());
                                }
                                else
                                {
                                    ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, nCol].Text = ssStatisticsView0_Sheet1.Cells[ssStatisticsView0_Sheet1.RowCount - 1, nCol].Text + ComNum.VBLF + GetMiBiNm(dt.Rows[i]["MIBICD"].ToString().Trim(), dt.Rows[i]["MIBIRMK"].ToString().Trim());
                                }

                                ssStatisticsView0_Sheet1.SetRowHeight(ssStatisticsView0_Sheet1.RowCount - 1, Convert.ToInt32(ssStatisticsView0_Sheet1.GetPreferredRowHeight(ssStatisticsView0_Sheet1.RowCount - 1)) + 10);
                                mHisMaxRowHeightNew = ssStatisticsView0_Sheet1.GetRowHeight(ssStatisticsView0_Sheet1.RowCount - 1);
                                if (mHisMaxRowHeightNew > mHisMaxRowHeightOld)
                                {
                                    mHisMaxRowHeightOld = mHisMaxRowHeightNew;
                                }
                                break;
                        }

                        if (dt.Rows[i]["MIBICLS"].ToString().Trim() == "1")
                        {
                            vaForeColor(ssStatisticsView0, ssStatisticsView0_Sheet1.RowCount - 1, nCol, ssStatisticsView0_Sheet1.RowCount - 1, nCol, Color.Red);
                        }
                        else if (dt.Rows[i]["MIBICLS"].ToString().Trim() == "2")
                        {
                            vaForeColor(ssStatisticsView0, ssStatisticsView0_Sheet1.RowCount - 1, nCol, ssStatisticsView0_Sheet1.RowCount - 1, nCol, Color.Blue);
                        }
                    }
                }

                setSSRowMaxHeight(ssStatisticsView0);

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void GetMibiStatistics1()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strMiBiFrDate2 = "";
            string strMiBiToDate2 = "";

            string strMiBiGrp = "";
            string strMedDeptCd = "";

            Font font = null;

            int intSpanEndRow = 0;
            int intSpanStartRow = 0;

            strMiBiFrDate2 = (dtpMiBiFrDate2).Value.ToString("yyyyMMdd");
            strMiBiToDate2 = (dtpMiBiToDate2).Value.ToString("yyyyMMdd");

            if (rdoSearchType3.Checked == true)
            {
                if (VB.Len(cboSearchType.Text) > 0)
                {
                    strMiBiGrp = VB.Right(cboSearchType.Text, 1);
                }
            }
            else if (rdoSearchType4.Checked == true)
            {
                if (VB.Len(cboSearchType.Text) > 0)
                {
                    strMedDeptCd = VB.Right(cboSearchType.Text, 2);
                }
            }

            ssStatisticsView1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DECODE(GROUPING(MIBIGRP), 1, '<< 총 계 >>', DECODE(GROUPING(MEDDEPTCD), 1, '', MIBIGRP)) AS MIBIGRP,";
                SQL = SQL + ComNum.VBLF + "              DECODE(GROUPING(MIBIGRP), 1, '', DECODE(GROUPING(MEDDEPTCD), 1, '<< 소 계 >>', MEDDEPTCD)) AS MEDDEPTCD,";
                SQL = SQL + ComNum.VBLF + "              SUM(MIBICNT) AS MIBICNT, SUM(BALCNT) AS BALCNT";
                SQL = SQL + ComNum.VBLF + " From (";
                SQL = SQL + ComNum.VBLF + "              SELECT MIBIGRP, MEDDEPTCD, COUNT(MIBICLS) AS MIBICNT, 0 BALCNT";
                SQL = SQL + ComNum.VBLF + "              From " + ComNum.DB_EMR + "EMRMIBI";
                SQL = SQL + ComNum.VBLF + "              WHERE MEDENDDATE BETWEEN '" + strMiBiFrDate2 + "' AND '" + strMiBiToDate2 + "'";
                SQL = SQL + ComNum.VBLF + "                  AND MIBICLS = 1";
                if (VB.Left(cboDept.Text, 2) != "**")
                {
                    SQL = SQL + ComNum.VBLF + " AND MEDDEPTCD = '" + VB.Right(cboDept.Text, 2) + "' ";
                }
                if (VB.Len(strMiBiGrp) > 0)
                {
                    SQL = SQL + ComNum.VBLF + "                  AND MIBIGRP = '" + strMiBiGrp + "'";
                }
                SQL = SQL + ComNum.VBLF + "              GROUP BY MIBIGRP, MEDDEPTCD";
                SQL = SQL + ComNum.VBLF + "              Union All";
                SQL = SQL + ComNum.VBLF + "              SELECT MIBIGRP, MEDDEPTCD, 0 AS MIBICNT, COUNT(MIBICLS) BALCNT";
                SQL = SQL + ComNum.VBLF + "              From " + ComNum.DB_EMR + "EMRMIBI";
                SQL = SQL + ComNum.VBLF + "              WHERE MEDENDDATE BETWEEN '" + strMiBiFrDate2 + "' AND '" + strMiBiToDate2 + "'";
                SQL = SQL + ComNum.VBLF + "                  AND MIBICLS IN (1, 2)";
                if (VB.Left(cboDept.Text, 2) != "**")
                {
                    SQL = SQL + ComNum.VBLF + " AND MEDDEPTCD = '" + VB.Right(cboDept.Text, 2) + "' ";
                }
                if (VB.Len(strMiBiGrp) > 0)
                {
                    SQL = SQL + ComNum.VBLF + "                  AND MIBIGRP = '" + strMiBiGrp + "'";
                }
                SQL = SQL + ComNum.VBLF + "              GROUP BY MIBIGRP, MEDDEPTCD";
                SQL = SQL + ComNum.VBLF + " )";
                SQL = SQL + ComNum.VBLF + " GROUP BY ROLLUP(MIBIGRP, MEDDEPTCD)";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                    return;
                }

                strMiBiGrp = (dt.Rows[0]["MIBIGRP"].ToString().Trim());

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssStatisticsView1_Sheet1.RowCount = ssStatisticsView1_Sheet1.RowCount + 1;

                    if (dt.Rows[i]["MIBIGRP"].ToString().Trim() != "" && dt.Rows[i]["MEDDEPTCD"].ToString().Trim() != "")
                    {
                        ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 0].Text = GetMiBiTitleNm(dt.Rows[i]["MIBIGRP"].ToString().Trim());
                        ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["MEDDEPTCD"].ToString().Trim();
                    }
                    else
                    {
                        if (dt.Rows[i]["MEDDEPTCD"].ToString().Trim() != "")
                        {
                            ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["MEDDEPTCD"].ToString().Trim();
                            font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
                            ssStatisticsView1_Sheet1.Rows[ssStatisticsView1_Sheet1.RowCount - 1].Font = font;

                            vaBackColor(ssStatisticsView1, ssStatisticsView1_Sheet1.RowCount - 1, 0, ssStatisticsView1_Sheet1.RowCount - 1, ssStatisticsView1_Sheet1.ColumnCount - 1, Color.FromArgb(235, 235, 235));

                            ssStatisticsView1_Sheet1.Rows[ssStatisticsView1_Sheet1.RowCount - 1].HorizontalAlignment = CellHorizontalAlignment.Center;
                        }
                        else if (dt.Rows[i]["MIBIGRP"].ToString().Trim() != "")
                        {
                            ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["MIBIGRP"].ToString().Trim();
                            font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
                            ssStatisticsView1_Sheet1.Rows[ssStatisticsView1_Sheet1.RowCount - 1].Font = font;

                            vaBackColor(ssStatisticsView1, ssStatisticsView1_Sheet1.RowCount - 1, 0, ssStatisticsView1_Sheet1.RowCount - 1, ssStatisticsView1_Sheet1.ColumnCount - 1, Color.FromArgb(236, 217, 255));

                            ssStatisticsView1_Sheet1.Rows[ssStatisticsView1_Sheet1.RowCount - 1].HorizontalAlignment = CellHorizontalAlignment.Center;
                        }
                    }

                    ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["MIBICNT"].ToString().Trim();
                    ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["BALCNT"].ToString().Trim();

                    if (dt.Rows[i]["MIBIGRP"].ToString().Trim() == strMiBiGrp)
                    {
                        intSpanEndRow = intSpanEndRow + 1;
                    }
                    else
                    {
                        strMiBiGrp = dt.Rows[i]["MIBIGRP"].ToString().Trim();
                        ssStatisticsView1_Sheet1.AddSpanCell(intSpanStartRow, 0, intSpanEndRow, 1);
                        intSpanStartRow = ssStatisticsView1_Sheet1.RowCount - 1;
                        intSpanEndRow = 1;
                    }

                }

                for (i = 0; i < ssStatisticsView1_Sheet1.RowCount; i++)
                {
                    if (ssStatisticsView1_Sheet1.Cells[i, 0].Text.IndexOf("<") != -1)
                    {
                        ssStatisticsView1_Sheet1.AddSpanCell(i, 0, 1, 2);
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void GetMibiStatistics2()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strMiBiFrDate2 = "";
            string strMiBiToDate2 = "";

            Font font = null;

            strMiBiFrDate2 = (dtpMiBiFrDate2.Value).ToString("yyyyMMdd");
            strMiBiToDate2 = (dtpMiBiToDate2.Value).ToString("yyyyMMdd");

            ssStatisticsView1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            //MC 심장내과
            //ME 내분비내과
            //MG 소화기내과
            //MN 신장내과
            //MP 호흡기내과과
            //MR 류마티스내과

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT DECODE(GROUPING(DEPTNAME), 1, '<< 총 계 >>', DECODE(GROUPING(DEPTNAME),1,'', DEPTNAME)) AS DEPTNAME,";
                SQL = SQL + ComNum.VBLF + "             DECODE(GROUPING(DEPTNAME),1,'', DECODE(GROUPING(USENAME), 1, '<< 소 계 >>', MAX(MEDDEPTCD))) AS DEPTCD,";
                SQL = SQL + ComNum.VBLF + "             DECODE(GROUPING(DEPTNAME),1,'', USENAME) AS DRNAME,";
                SQL = SQL + ComNum.VBLF + "             SUM(MIBICNT) AS MIBICNT,";
                SQL = SQL + ComNum.VBLF + "             SUM(BALCNT) As BALCNT,";
                SQL = SQL + ComNum.VBLF + "          (";
                SQL = SQL + ComNum.VBLF + "              SELECT";
                SQL = SQL + ComNum.VBLF + "              PRINTRANK";
                SQL = SQL + ComNum.VBLF + "              FROM ADMIN.MID_DEPT";
                SQL = SQL + ComNum.VBLF + "              WHERE DEPTNAMEK = DEPTNAME";
                SQL = SQL + ComNum.VBLF + "          ) As SORTNO";
                SQL = SQL + ComNum.VBLF + "From (";
                if (chkCheck.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "              SELECT /*+INDEX(ADMIN.emr_treatt  index_indate*/A.DEPTNAME, A.MEDENDDATE, A.MEDDEPTCD, A.USENAME,";
                    SQL = SQL + ComNum.VBLF + "                          NVL(A.MIBIBALCNT,0) AS BALCNT, NVL(B.MIBICNT, 0) AS MIBICNT";
                    SQL = SQL + ComNum.VBLF + "              From";
                    SQL = SQL + ComNum.VBLF + "              (SELECT C.DEPTNAMEK DEPTNAME, A.MEDENDDATE, A.MEDDEPTCD, B.USENAME,";
                    SQL = SQL + ComNum.VBLF + "                  COUNT(DISTINCT A.PTNO) AS MIBIBALCNT";
                    SQL = SQL + ComNum.VBLF + "              From " + ComNum.DB_EMR + "EMRMIBI A, " + ComNum.DB_EMR + "VIEWBUSER B, " + ComNum.DB_PMPA + "MID_DEPT C, " + ComNum.DB_EMR + "EMR_TREATT D";
                    SQL = SQL + ComNum.VBLF + "              WHERE MEDENDDATE BETWEEN '" + strMiBiFrDate2 + "' AND '" + strMiBiToDate2 + "'";
                    SQL = SQL + ComNum.VBLF + "                  AND A.PTNO = D.PATID";
                    SQL = SQL + ComNum.VBLF + "                  AND A.MEDFRDATE = D.INDATE";
                    SQL = SQL + ComNum.VBLF + "                  AND D.CLASS = 'I'";
                    SQL = SQL + ComNum.VBLF + "                  AND A.MIBICLS IN (1,2)";
                    SQL = SQL + ComNum.VBLF + "                  AND D.DOCCODE = B.USEID";
                    SQL = SQL + ComNum.VBLF + "                  AND D.CLINCODE = C.DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + "              GROUP BY A.MEDENDDATE, A.MEDDEPTCD, B.DEPTNAME, B.USENAME, C.DEPTNAMEK) A,";
                    SQL = SQL + ComNum.VBLF + "              (SELECT C.DEPTNAMEK DEPTNAME, A.MEDENDDATE, A.MEDDEPTCD, B.USENAME,";
                    SQL = SQL + ComNum.VBLF + "                  COUNT(DISTINCT A.PTNO) AS MIBICNT";
                    SQL = SQL + ComNum.VBLF + "              From " + ComNum.DB_EMR + "EMRMIBI A, " + ComNum.DB_EMR + "VIEWBUSER B, " + ComNum.DB_PMPA + "MID_DEPT C, " + ComNum.DB_EMR + "EMR_TREATT D";
                    SQL = SQL + ComNum.VBLF + "              WHERE MEDENDDATE BETWEEN '" + strMiBiFrDate2 + "' AND '" + strMiBiToDate2 + "'";
                    SQL = SQL + ComNum.VBLF + "                  AND A.PTNO = D.PATID";

                    SQL = SQL + ComNum.VBLF + "                  AND A.MEDFRDATE = D.INDATE";
                    SQL = SQL + ComNum.VBLF + "                  AND D.CLASS = 'I'";
                    SQL = SQL + ComNum.VBLF + "                  AND A.MIBICLS IN (1)";
                    SQL = SQL + ComNum.VBLF + "                  AND D.DOCCODE = B.USEID";
                    SQL = SQL + ComNum.VBLF + "                  AND D.CLINCODE = C.DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + "              GROUP BY A.MEDENDDATE, A.MEDDEPTCD, B.DEPTNAME, B.USENAME, C.DEPTNAMEK) B";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "              SELECT A.DEPTNAME, A.MEDENDDATE, A.MEDDEPTCD, A.USENAME,";
                    SQL = SQL + ComNum.VBLF + "                          NVL(A.MIBIBALCNT,0) AS BALCNT, NVL(B.MIBICNT, 0) AS MIBICNT";
                    SQL = SQL + ComNum.VBLF + "              From";
                    SQL = SQL + ComNum.VBLF + "              (SELECT C.DEPTNAMEK DEPTNAME, A.MEDENDDATE, A.MEDDEPTCD, B.USENAME,";
                    SQL = SQL + ComNum.VBLF + "                  COUNT(DISTINCT A.PTNO) AS MIBIBALCNT";
                    SQL = SQL + ComNum.VBLF + "              From " + ComNum.DB_EMR + "EMRMIBI A, " + ComNum.DB_EMR + "VIEWBUSER B, " + ComNum.DB_PMPA + "MID_DEPT C";
                    SQL = SQL + ComNum.VBLF + "              WHERE MEDENDDATE BETWEEN '" + strMiBiFrDate2 + "' AND '" + strMiBiToDate2 + "'";
                    SQL = SQL + ComNum.VBLF + "                  AND A.MIBICLS IN (1,2)";
                    SQL = SQL + ComNum.VBLF + "    AND (DECODE(A.MEDDEPTCD, 'MC', 'MD', 'MP','MD','MN','MD','ME','MD','MR','MD','MC','MD','MG','MD','MI','MD','MO','MD','HU','GS','DM', A.MEDDEPTCD) = B.DEPTCD OR A.MEDDEPTCD = B.DEPTCD)";
                    SQL = SQL + ComNum.VBLF + "                  AND A.MEDDRCD = B.USEID";
                    SQL = SQL + ComNum.VBLF + "                  AND A.MEDDEPTCD = C.DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + "              GROUP BY A.MEDENDDATE, A.MEDDEPTCD, B.DEPTNAME, B.USENAME, C.DEPTNAMEK) A,";
                    SQL = SQL + ComNum.VBLF + "              (SELECT C.DEPTNAMEK DEPTNAME, A.MEDENDDATE, A.MEDDEPTCD, B.USENAME,";
                    SQL = SQL + ComNum.VBLF + "                  COUNT(DISTINCT A.PTNO) AS MIBICNT";
                    SQL = SQL + ComNum.VBLF + "              From " + ComNum.DB_EMR + "EMRMIBI A, " + ComNum.DB_EMR + "VIEWBUSER B, " + ComNum.DB_PMPA + "MID_DEPT C";
                    SQL = SQL + ComNum.VBLF + "              WHERE MEDENDDATE BETWEEN '" + strMiBiFrDate2 + "' AND '" + strMiBiToDate2 + "'";
                    SQL = SQL + ComNum.VBLF + "                  AND A.MIBICLS IN (1)";
                    SQL = SQL + ComNum.VBLF + "    AND (DECODE(A.MEDDEPTCD, 'MC', 'MD', 'MP','MD','MN','MD','ME','MD','MR','MD','MC','MD','MG','MD','MI','MD','MO','MD','HU','GS','DM', A.MEDDEPTCD) = B.DEPTCD OR A.MEDDEPTCD = B.DEPTCD)";
                    SQL = SQL + ComNum.VBLF + "                  AND A.MEDDRCD = B.USEID";
                    SQL = SQL + ComNum.VBLF + "                  AND A.MEDDEPTCD = C.DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + "              GROUP BY A.MEDENDDATE, A.MEDDEPTCD, B.DEPTNAME, B.USENAME, C.DEPTNAMEK) B";
                }
                SQL = SQL + ComNum.VBLF + "              WHERE A.DEPTNAME = B.DEPTNAME(+)";
                SQL = SQL + ComNum.VBLF + "                  AND A.MEDDEPTCD = B.MEDDEPTCD(+)";
                SQL = SQL + ComNum.VBLF + "                  AND A.USENAME = B.USENAME(+)";
                SQL = SQL + ComNum.VBLF + "                  AND A.MEDENDDATE = B.MEDENDDATE(+)";
                if (VB.Left(cboDept.Text, 2) != "**")
                {
                    SQL = SQL + ComNum.VBLF + " AND A.MEDDEPTCD = '" + VB.Right(cboDept.Text, 2) + "' ";
                }
                SQL = SQL + ComNum.VBLF + " )";
                SQL = SQL + ComNum.VBLF + "GROUP BY ROLLUP(DEPTNAME, USENAME)";
                SQL = SQL + ComNum.VBLF + "ORDER BY SORTNO, DEPTCD DESC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssStatisticsView1_Sheet1.RowCount = ssStatisticsView1_Sheet1.RowCount + 1;

                    if (dt.Rows[i]["DEPTNAME"].ToString().Trim() != "" && dt.Rows[i]["DRNAME"].ToString().Trim() != "")
                    {
                        ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 0].Text = VB.Space(2) + "⊙" + VB.Space(2) + dt.Rows[i]["DEPTNAME"].ToString().Trim() + VB.Space(1) + "(" + dt.Rows[i]["DEPTCD"].ToString().Trim() + ")";
                        ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                    }
                    else
                    {
                        if (dt.Rows[i]["DEPTCD"].ToString().Trim() != "")
                        {
                            ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["DEPTCD"].ToString().Trim();
                            font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
                            ssStatisticsView1_Sheet1.Rows[ssStatisticsView1_Sheet1.RowCount - 1].Font = font;

                            vaBackColor(ssStatisticsView1, ssStatisticsView1_Sheet1.RowCount - 1, 0, ssStatisticsView1_Sheet1.RowCount - 1, ssStatisticsView1_Sheet1.ColumnCount - 1, Color.FromArgb(235, 235, 235));

                            ssStatisticsView1_Sheet1.Rows[ssStatisticsView1_Sheet1.RowCount - 1].HorizontalAlignment = CellHorizontalAlignment.Center;
                        }
                        else if (dt.Rows[i]["DEPTNAME"].ToString().Trim() != "")
                        {
                            ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["DEPTNAME"].ToString().Trim();
                            font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
                            ssStatisticsView1_Sheet1.Rows[ssStatisticsView1_Sheet1.RowCount - 1].Font = font;

                            vaBackColor(ssStatisticsView1, ssStatisticsView1_Sheet1.RowCount - 1, 0, ssStatisticsView1_Sheet1.RowCount - 1, ssStatisticsView1_Sheet1.ColumnCount - 1, Color.FromArgb(236, 217, 255));

                            ssStatisticsView1_Sheet1.Rows[ssStatisticsView1_Sheet1.RowCount - 1].HorizontalAlignment = CellHorizontalAlignment.Center;
                        }
                    }

                    if (ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 0].Text.IndexOf("소 계") != -1)
                    {
                        ssStatisticsView1_Sheet1.AddSpanCell(ssStatisticsView1_Sheet1.RowCount - 1, 0, 1, 2);
                    }

                    ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["MIBICNT"].ToString().Trim();
                    ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["BALCNT"].ToString().Trim();
                }

                ssStatisticsView1_Sheet1.AddSpanCell(ssStatisticsView1_Sheet1.RowCount - 1, 0, 1, 2);

                dt.Dispose();
                dt = null;

                lblMibi.Text = MedicalInfoTeam ? "총 발생 건수 : " +
                    VB.Val(ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 3].Text.Trim()) : "";

                lblMibi.Visible = MedicalInfoTeam;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void GetMibiStatistics2_1()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strMiBiFrDate2 = "";
            string strMiBiToDate2 = "";

            string strNAME_OLD = "";

            Font font = null;

            strMiBiFrDate2 = (dtpMiBiFrDate2.Value).ToString("yyyyMMdd");
            strMiBiToDate2 = (dtpMiBiToDate2.Value).ToString("yyyyMMdd");

            ssStatisticsView1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            //MC 심장내과
            //ME 내분비내과
            //MG 소화기내과
            //MN 신장내과
            //MP 호흡기내과과
            //MR 류마티스내

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT DECODE(GROUPING(DEPTNAME), 1, '<< 총 계 >>', DECODE(GROUPING(DEPTNAME),1,'', DEPTNAME)) AS DEPTNAME,";
                SQL = SQL + ComNum.VBLF + "             DECODE(GROUPING(USENAME),1,'', DECODE(GROUPING(DEPTNAME), 1, '<< 소 계 >>', MAX(USENAME))) AS DEPTCD,";
                SQL = SQL + ComNum.VBLF + "             DECODE(GROUPING(DEPTNAME),1,'', USENAME) AS DRNAME,";
                SQL = SQL + ComNum.VBLF + "             SUM(MIBICNT) AS MIBICNT,";
                SQL = SQL + ComNum.VBLF + "             SUM(BALCNT) As BALCNT";
                //SQL = SQL + ComNum.VBLF + "             SUM(BALCNT) As BALCNT,";
                //SQL = SQL + ComNum.VBLF + "          Case DEPTNAME";
                //SQL = SQL + ComNum.VBLF + "              WHEN '내과' THEN 1";
                //SQL = SQL + ComNum.VBLF + "              WHEN '심장내과' THEN 2";
                //SQL = SQL + ComNum.VBLF + "              WHEN '내분비내과' THEN 3";
                //SQL = SQL + ComNum.VBLF + "              WHEN '소화기내과' THEN 4";
                //SQL = SQL + ComNum.VBLF + "              WHEN '신장내과' THEN 5";
                //SQL = SQL + ComNum.VBLF + "              WHEN '호흡기내과' THEN 6";
                //SQL = SQL + ComNum.VBLF + "              WHEN '류마티스내과' THEN 7";
                //SQL = SQL + ComNum.VBLF + "              WHEN '감염내과' THEN 8";
                //SQL = SQL + ComNum.VBLF + "              WHEN '종양내과' THEN 9";
                //SQL = SQL + ComNum.VBLF + "              WHEN '외    과' THEN 10";
                //SQL = SQL + ComNum.VBLF + "              WHEN '소아청소년과' THEN 11";
                //SQL = SQL + ComNum.VBLF + "              WHEN '산부인과' THEN 12";
                //SQL = SQL + ComNum.VBLF + "              WHEN '정형외과' THEN 13";
                //SQL = SQL + ComNum.VBLF + "              WHEN '신경외과' THEN 14";
                //SQL = SQL + ComNum.VBLF + "              WHEN '이비인후과' THEN 15";
                //SQL = SQL + ComNum.VBLF + "              WHEN '비뇨기과' THEN 16";
                //SQL = SQL + ComNum.VBLF + "              WHEN '안    과' THEN 17";
                //SQL = SQL + ComNum.VBLF + "              WHEN '정신건강의학과' THEN 18";
                //SQL = SQL + ComNum.VBLF + "              WHEN '신경과' THEN 19";
                //SQL = SQL + ComNum.VBLF + "              WHEN '치    과' THEN 20";
                //SQL = SQL + ComNum.VBLF + "              WHEN '흉부외과' THEN 21";
                //SQL = SQL + ComNum.VBLF + "              WHEN '재활의학과' THEN 22";
                //SQL = SQL + ComNum.VBLF + "              WHEN '응급의료센터' THEN 23";
                //SQL = SQL + ComNum.VBLF + "              WHEN '영상의학과' THEN 24";
                //SQL = SQL + ComNum.VBLF + "              ELSE 25";
                //SQL = SQL + ComNum.VBLF + "          END As SORTNO";
                SQL = SQL + ComNum.VBLF + "From (";
                SQL = SQL + ComNum.VBLF + "              SELECT A.DEPTNAME, A.MEDENDDATE, A.MEDDEPTCD, A.USENAME,";
                SQL = SQL + ComNum.VBLF + "                          NVL(A.MIBIBALCNT,0) AS BALCNT, NVL(B.MIBICNT, 0) AS MIBICNT";
                SQL = SQL + ComNum.VBLF + "              From";
                SQL = SQL + ComNum.VBLF + "              (SELECT C.DEPTNAMEK DEPTNAME, A.MEDENDDATE, A.MEDDEPTCD, B.USENAME,";
                SQL = SQL + ComNum.VBLF + "                  COUNT(DISTINCT A.PTNO) AS MIBIBALCNT";
                SQL = SQL + ComNum.VBLF + "              From " + ComNum.DB_EMR + "EMRMIBI A, " + ComNum.DB_EMR + "VIEWBUSER B, " + ComNum.DB_PMPA + "MID_DEPT C";
                SQL = SQL + ComNum.VBLF + "              WHERE MEDENDDATE BETWEEN '" + strMiBiFrDate2 + "' AND '" + strMiBiToDate2 + "'";
                SQL = SQL + ComNum.VBLF + "                  AND A.MIBICLS IN (1,2)";
                SQL = SQL + ComNum.VBLF + "                  AND DECODE(A.MEDDEPTCD, 'MC', 'MD', 'MP','MD','MN','MD','ME','MD','MR','MD','MC','MD','MG','MD','MI','MD','MO','MD',A.MEDDEPTCD) = B.DEPTCD";
                SQL = SQL + ComNum.VBLF + "                  AND A.MEDDRCD = B.USEID";
                SQL = SQL + ComNum.VBLF + "                  AND A.MEDDEPTCD = C.DEPTCODE ";
                SQL = SQL + ComNum.VBLF + "                  AND A.MEDDEPTCD IN ('MC','MG','MP','MN','ME','MR','MD','MI','MO')";
                SQL = SQL + ComNum.VBLF + "              GROUP BY A.MEDENDDATE, A.MEDDEPTCD, B.DEPTNAME, B.USENAME, C.DEPTNAMEK) A,";
                SQL = SQL + ComNum.VBLF + "              (SELECT C.DEPTNAMEK DEPTNAME, A.MEDENDDATE, A.MEDDEPTCD, B.USENAME,";
                SQL = SQL + ComNum.VBLF + "                  COUNT(DISTINCT A.PTNO) AS MIBICNT";
                SQL = SQL + ComNum.VBLF + "              From " + ComNum.DB_EMR + "EMRMIBI A, " + ComNum.DB_EMR + "VIEWBUSER B, " + ComNum.DB_PMPA + "MID_DEPT C";
                SQL = SQL + ComNum.VBLF + "              WHERE MEDENDDATE BETWEEN '" + strMiBiFrDate2 + "' AND '" + strMiBiToDate2 + "'";
                SQL = SQL + ComNum.VBLF + "                  AND A.MIBICLS IN (1)";
                SQL = SQL + ComNum.VBLF + "                  AND DECODE(A.MEDDEPTCD, 'MC', 'MD', 'MP','MD','MN','MD','ME','MD','MR','MD','MC','MD','MG','MD','MI','MD', 'MO','MD',A.MEDDEPTCD) = B.DEPTCD";
                SQL = SQL + ComNum.VBLF + "                  AND A.MEDDRCD = B.USEID";
                SQL = SQL + ComNum.VBLF + "                  AND A.MEDDEPTCD = C.DEPTCODE ";
                SQL = SQL + ComNum.VBLF + "              GROUP BY A.MEDENDDATE, A.MEDDEPTCD, B.DEPTNAME, B.USENAME, C.DEPTNAMEK) B";
                SQL = SQL + ComNum.VBLF + "              WHERE A.DEPTNAME = B.DEPTNAME(+)";
                SQL = SQL + ComNum.VBLF + "                  AND A.MEDDEPTCD = B.MEDDEPTCD(+)";
                SQL = SQL + ComNum.VBLF + "                  AND A.USENAME = B.USENAME(+)";
                SQL = SQL + ComNum.VBLF + "                  AND A.MEDENDDATE = B.MEDENDDATE(+)";
                SQL = SQL + ComNum.VBLF + "                  AND A.MEDDEPTCD IN ('MC','MG','MP','MN','ME','MR','MD','MI','MO')";
                if (VB.Left(cboDept.Text, 2) != "**")
                {
                    SQL = SQL + ComNum.VBLF + " AND A.MEDDEPTCD = '" + VB.Right(cboDept.Text, 2) + "' ";
                }
                SQL = SQL + ComNum.VBLF + " )";
                SQL = SQL + ComNum.VBLF + "GROUP BY ROLLUP(USENAME, DEPTNAME)";
                SQL = SQL + ComNum.VBLF + "ORDER BY USENAME, DEPTNAME DESC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssStatisticsView1_Sheet1.RowCount = ssStatisticsView1_Sheet1.RowCount + 1;

                    if (dt.Rows[i]["DEPTNAME"].ToString().Trim() != "" && dt.Rows[i]["DRNAME"].ToString().Trim() != "")
                    {
                        ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 1].Text = VB.Space(2) + "⊙" + VB.Space(2) + dt.Rows[i]["DEPTNAME"].ToString().Trim();
                        if (strNAME_OLD != dt.Rows[i]["DRNAME"].ToString().Trim())
                        {
                            strNAME_OLD = dt.Rows[i]["DRNAME"].ToString().Trim();
                            ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 0].Text = strNAME_OLD;
                        }
                    }
                    else
                    {
                        if (dt.Rows[i]["DEPTCD"].ToString().Trim() != "")
                        {
                            ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["DEPTCD"].ToString().Trim();
                            font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
                            ssStatisticsView1_Sheet1.Rows[ssStatisticsView1_Sheet1.RowCount - 1].Font = font;

                            vaBackColor(ssStatisticsView1, ssStatisticsView1_Sheet1.RowCount - 1, 0, ssStatisticsView1_Sheet1.RowCount - 1, ssStatisticsView1_Sheet1.ColumnCount - 1, Color.FromArgb(235, 235, 235));

                            ssStatisticsView1_Sheet1.Rows[ssStatisticsView1_Sheet1.RowCount - 1].HorizontalAlignment = CellHorizontalAlignment.Center;
                        }
                        else if (dt.Rows[i]["DEPTNAME"].ToString().Trim() != "")
                        {
                            ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["DEPTNAME"].ToString().Trim();
                            font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
                            ssStatisticsView1_Sheet1.Rows[ssStatisticsView1_Sheet1.RowCount - 1].Font = font;

                            vaBackColor(ssStatisticsView1, ssStatisticsView1_Sheet1.RowCount - 1, 0, ssStatisticsView1_Sheet1.RowCount - 1, ssStatisticsView1_Sheet1.ColumnCount - 1, Color.FromArgb(236, 217, 255));

                            ssStatisticsView1_Sheet1.Rows[ssStatisticsView1_Sheet1.RowCount - 1].HorizontalAlignment = CellHorizontalAlignment.Center;
                        }
                    }

                    if (ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 0].Text.IndexOf("소 계") != -1)
                    {
                        ssStatisticsView1_Sheet1.AddSpanCell(ssStatisticsView1_Sheet1.RowCount - 1, 0, 1, 2);
                    }

                    ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["MIBICNT"].ToString().Trim();
                    ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["BALCNT"].ToString().Trim();
                }

                ssStatisticsView1_Sheet1.AddSpanCell(ssStatisticsView1_Sheet1.RowCount - 1, 0, 1, 2);
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void GetMibiStatistics3()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strMiBiFrDate2 = dtpMiBiFrDate2.Value.ToString("yyyyMM") + "01";
            string strMiBiToDate2 = dtpMiBiFrDate2.Value.ToString("yyyyMM") + DateTime.DaysInMonth(dtpMiBiFrDate2.Value.Year, dtpMiBiFrDate2.Value.Month);

            int Row = 0;

            double dVar1 = 0;
            double dVar2 = 0;

            Font font = null;

            ssStatisticsView1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            //MC 심장내과
            //ME 내분비내과
            //MG 소화기내과
            //MN 신장내과
            //MP 호흡기내과과
            //MR 류마티스내



            try
            {
                SQL = "";
                if (VB.Val(strMiBiFrDate2) >= 20101001)
                {
                    SQL = SQL + ComNum.VBLF + " SELECT '내   과' DEPTNAMEK, 'MD' TDEPT, 0 AS ENDCNT, 1 AS PRINTRANK";
                    SQL = SQL + ComNum.VBLF + " FROM DUAL ";
                    SQL = SQL + ComNum.VBLF + " UNION ALL ";
                }
                if (VB.Val(strMiBiFrDate2) >= 20170401 && VB.Val(strMiBiFrDate2) <= 20170401)
                {
                    SQL = SQL + ComNum.VBLF + " SELECT '감염내과' DEPTNAMEK, 'MI' TDEPT, 0 AS ENDCNT, 8 AS PRINTRANK";
                    SQL = SQL + ComNum.VBLF + " FROM DUAL ";
                    SQL = SQL + ComNum.VBLF + " UNION ALL ";
                }
                SQL = SQL + ComNum.VBLF + " SELECT B.DEPTNAMEK, A.TDEPT,  DECODE(TDEPT,'MD',0,COUNT(A.PANO))  AS ENDCNT, B.PRINTRANK";
                SQL = SQL + ComNum.VBLF + " From " + ComNum.DB_PMPA + "MID_SUMMARY A,";
                SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_PMPA + "MID_DEPT B";
                SQL = SQL + ComNum.VBLF + " WHERE TO_CHAR(A.OUTDATE,'YYYYMMDD') BETWEEN '" + strMiBiFrDate2 + "' AND '" + strMiBiToDate2 + "'";
                SQL = SQL + ComNum.VBLF + "      AND A.TDept = B.Deptcode";
                SQL = SQL + ComNum.VBLF + " GROUP BY B.DEPTNAMEK, A.TDEPT, PRINTRANK";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANK";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssStatisticsView1_Sheet1.RowCount = ssStatisticsView1_Sheet1.RowCount + 1;

                    ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["TDEPT"].ToString().Trim();

                    if (dt.Rows[i]["TDEPT"].ToString().Trim() == "PD")
                    {
                        ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 1].Text = "소아청소년과";
                    }
                    else if (dt.Rows[i]["TDEPT"].ToString().Trim() == "NP")
                    {
                        ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 1].Text = "정신건강의학과";
                    }
                    else
                    {
                        ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["DEPTNAMEK"].ToString().Trim();
                    }

                    ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["ENDCNT"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.DEPTNAME, A.MEDDEPTCD,";
                SQL = SQL + ComNum.VBLF + " SUM(NVL(A.MIBIBALCNT,0)) AS BALCNT,";
                SQL = SQL + ComNum.VBLF + " SUM(NVL(B.MIBICNT, 0)) AS MIBICNT";
                SQL = SQL + ComNum.VBLF + " From";
                SQL = SQL + ComNum.VBLF + " (SELECT C.DEPTNAMEK DEPTNAME, A.MEDDEPTCD, A.MEDDRCD, A.MEDENDDATE, B.USENAME,";
                SQL = SQL + ComNum.VBLF + " COUNT(DISTINCT A.PTNO) AS MIBIBALCNT";
                SQL = SQL + ComNum.VBLF + " From " + ComNum.DB_EMR + "EMRMIBI A, " + ComNum.DB_EMR + "VIEWBUSER B, " + ComNum.DB_PMPA + "MID_DEPT C";
                SQL = SQL + ComNum.VBLF + " WHERE MEDENDDATE BETWEEN '" + strMiBiFrDate2 + "' AND '" + strMiBiToDate2 + "'";
                SQL = SQL + ComNum.VBLF + " AND A.MIBICLS IN (1,2)";
                SQL = SQL + ComNum.VBLF + " AND (A.MEDDEPTCD = B.DEPTCD OR DECODE(A.MEDDEPTCD, 'MC', 'MD', 'MP','MD','MN','MD','ME','MD','MR','MD','MC','MD','MG','MD','MI','MD','MO','MD','HU','GS',A.MEDDEPTCD) = B.DEPTCD)";
                SQL = SQL + ComNum.VBLF + " AND A.MEDDRCD = B.USEID";
                SQL = SQL + ComNum.VBLF + " AND A.MEDDEPTCD = C.DEPTCODE";
                SQL = SQL + ComNum.VBLF + " GROUP BY  C.DEPTNAMEK, A.MEDDEPTCD, A.MEDDRCD, A.MEDENDDATE, B.DEPTNAME, B.USENAME) A,";
                SQL = SQL + ComNum.VBLF + " (SELECT C.DEPTNAMEK DEPTNAME, A.MEDDEPTCD, A.MEDDRCD,  A.MEDENDDATE, B.USENAME, ";
                SQL = SQL + ComNum.VBLF + " COUNT(DISTINCT A.PTNO) AS MIBICNT";
                SQL = SQL + ComNum.VBLF + " From " + ComNum.DB_EMR + "EMRMIBI A, " + ComNum.DB_EMR + "VIEWBUSER B, " + ComNum.DB_PMPA + "MID_DEPT C";
                SQL = SQL + ComNum.VBLF + " WHERE MEDENDDATE BETWEEN '" + strMiBiFrDate2 + "' AND '" + strMiBiToDate2 + "'";
                SQL = SQL + ComNum.VBLF + " AND A.MIBICLS IN (1)";
                SQL = SQL + ComNum.VBLF + " AND (A.MEDDEPTCD = B.DEPTCD OR DECODE(A.MEDDEPTCD, 'MC', 'MD', 'MP','MD','MN','MD','ME','MD','MR','MD','MC','MD','MG','MD','MI','MD','MO','MD','HU','GS',A.MEDDEPTCD) = B.DEPTCD)";
                SQL = SQL + ComNum.VBLF + " AND A.MEDDRCD = B.USEID";
                SQL = SQL + ComNum.VBLF + " AND A.MEDDEPTCD = C.DEPTCODE";
                SQL = SQL + ComNum.VBLF + " GROUP BY  C.DEPTNAMEK, A.MEDDEPTCD, A.MEDDRCD, A.MEDENDDATE, B.DEPTNAME, B.USENAME) B";
                SQL = SQL + ComNum.VBLF + " WHERE A.DEPTNAME = B.DEPTNAME(+)";
                SQL = SQL + ComNum.VBLF + " AND A.MEDDEPTCD = B.MEDDEPTCD(+)";
                SQL = SQL + ComNum.VBLF + " AND A.MEDENDDATE = B.MEDENDDATE(+)";
                SQL = SQL + ComNum.VBLF + " AND A.MEDDRCD = B.MEDDRCD(+)";
                if (VB.Left(cboDept.Text, 2) != "**")
                {
                    SQL = SQL + ComNum.VBLF + " AND A.MEDDEPTCD = '" + VB.Right(cboDept.Text, 2) + "' ";
                }
                SQL = SQL + ComNum.VBLF + " GROUP BY A.DEPTNAME, A.MEDDEPTCD";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Row = RowAddCheck(dt.Rows[i]["MEDDEPTCD"].ToString().Trim());

                    if (Row == -1)
                    {
                        ssStatisticsView1_Sheet1.RowCount = ssStatisticsView1_Sheet1.RowCount + 1;

                        ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["MEDDEPTCD"].ToString().Trim();
                        ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["DEPTNAME"].ToString().Trim();
                        ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["BALCNT"].ToString().Trim();
                        ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["MIBICNT"].ToString().Trim();
                    }
                    else
                    {
                        ssStatisticsView1_Sheet1.Cells[Row, 3].Text = dt.Rows[i]["BALCNT"].ToString().Trim();
                        ssStatisticsView1_Sheet1.Cells[Row, 5].Text = dt.Rows[i]["MIBICNT"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                //월별 누적 통계
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DEPTNAME, MEDDEPTCD, SUM(NUMIBICNT) MUMIBICNT";
                SQL = SQL + ComNum.VBLF + " FROM (SELECT B.DEPTNAME, A.MEDDEPTCD,";
                SQL = SQL + ComNum.VBLF + "      COUNT(DISTINCT A.PTNO) AS NUMIBICNT";
                SQL = SQL + ComNum.VBLF + "      From " + ComNum.DB_EMR + "EMRMIBI A, " + ComNum.DB_EMR + "VIEWBUSER B";
                SQL = SQL + ComNum.VBLF + "      WHERE A.MIBICLS IN (1)";
                SQL = SQL + ComNum.VBLF + "          AND A.MEDENDDATE <= '" + strMiBiToDate2 + "'";
                SQL = SQL + ComNum.VBLF + "          AND DECODE(A.MEDDEPTCD, 'MC', 'MD', 'MP','MD','MN','MD','ME','MD','MR','MD','MC','MD','MG','MD','MI','MD','MO','MD','HU','GS',A.MEDDEPTCD) = B.DEPTCD";
                SQL = SQL + ComNum.VBLF + "          AND A.MEDDRCD = B.USEID";
                SQL = SQL + ComNum.VBLF + "      GROUP BY  A.MEDDEPTCD, A.MEDDRCD, A.MEDENDDATE, B.DEPTNAME)";
                SQL = SQL + ComNum.VBLF + " GROUP  BY DEPTNAME, MEDDEPTCD";
                SQL = SQL + ComNum.VBLF + "  ORDER BY MEDDEPTCD ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Row = RowAddCheck(dt.Rows[i]["MEDDEPTCD"].ToString().Trim());

                    if (Row == -1)
                    {
                        ssStatisticsView1_Sheet1.RowCount = ssStatisticsView1_Sheet1.RowCount + 1;

                        ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["MEDDEPTCD"].ToString().Trim();
                        ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["DEPTNAME"].ToString().Trim();
                        ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["MUMIBICNT"].ToString().Trim();
                    }
                    else
                    {
                        ssStatisticsView1_Sheet1.Cells[Row, 6].Text = dt.Rows[i]["MUMIBICNT"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                for (i = 0; i < ssStatisticsView1_Sheet1.RowCount; i++)
                {

                    dVar1 = VB.Val(ssStatisticsView1_Sheet1.Cells[i, 3].Text);
                    dVar2 = VB.Val(ssStatisticsView1_Sheet1.Cells[i, 2].Text);

                    if (dVar2 > 0)
                    {
                        ssStatisticsView1_Sheet1.Cells[i, 4].Text = VB.Mid(((dVar1 / dVar2) * 100).ToString().Trim(), 1, 4);
                    }

                    ssStatisticsView1_Sheet1.Cells[i, 6].Text = ssStatisticsView1_Sheet1.Cells[i, 6].Text == "" ? "0" : ssStatisticsView1_Sheet1.Cells[i, 6].Text;
                }

                ssStatisticsView1_Sheet1.RowCount = ssStatisticsView1_Sheet1.RowCount + 1;

                font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
                ssStatisticsView1_Sheet1.Rows[ssStatisticsView1_Sheet1.RowCount - 1].Font = font;
                ssStatisticsView1_Sheet1.Rows[ssStatisticsView1_Sheet1.RowCount - 1].BackColor = Color.FromArgb(255, 215, 255);

                Row = ssStatisticsView1_Sheet1.NonEmptyRowCount;

                ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 2].Formula = "SUM(C1:C" + Row + ")";
                ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 3].Formula = "SUM(D1:D" + Row + ")";

                dVar1 = (double) (ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 3].Value);
                dVar2 = (double) (ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 2].Value);

                ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 4].Text = string.Format("{0:0.#}", ((dVar1 / dVar2) * 100)) + "%";
                ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 5].Formula = "SUM(F1:F" + Row + ")";
                ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 6].Formula = "SUM(G1:G" + Row + ")";
                ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 1].Text = "합          계";

                ssStatisticsView1_Sheet1.SetRowHeight(-1, 25);

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void GetMibiStatistics4()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strMiBiFrDate2 = "";
            string strMiBiToDate2 = "";

            int Row = 0;

            Font font = null;

            strMiBiFrDate2 = (dtpMiBiFrDate2.Value).ToString("yyyyMMdd");
            strMiBiToDate2 = (dtpMiBiToDate2.Value).ToString("yyyyMMdd");

            ssStatisticsView1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT D.DEPTNAMEK, MAX(MEDDEPTCD) AS MEDDEPTCD,";
                SQL = SQL + ComNum.VBLF + "              SUM(MIBICNT) AS MIBICNT,";
                SQL = SQL + ComNum.VBLF + "              SUM(BALCNT) As BALCNT,";
                SQL = SQL + ComNum.VBLF + "         PRINTRANK ";
                SQL = SQL + ComNum.VBLF + " From (";
                SQL = SQL + ComNum.VBLF + "              SELECT A.DEPTNAME, A.MEDENDDATE, A.MEDDEPTCD, A.USENAME,";
                SQL = SQL + ComNum.VBLF + "                          NVL(A.MIBIBALCNT,0) AS BALCNT, NVL(B.MIBICNT, 0) AS MIBICNT";
                SQL = SQL + ComNum.VBLF + "              From";
                SQL = SQL + ComNum.VBLF + "              (SELECT ";
                SQL = SQL + ComNum.VBLF + "                  b.DEPTNAME, A.MEDENDDATE, A.MEDDEPTCD, B.USENAME,";
                SQL = SQL + ComNum.VBLF + "                  COUNT(DISTINCT A.PTNO) AS MIBIBALCNT";
                SQL = SQL + ComNum.VBLF + "              From " + ComNum.DB_EMR + "EMRMIBI A, " + ComNum.DB_EMR + "VIEWBUSER B";
                SQL = SQL + ComNum.VBLF + "              WHERE MEDENDDATE BETWEEN '" + strMiBiFrDate2 + "' AND '" + strMiBiToDate2 + "'";
                SQL = SQL + ComNum.VBLF + "                  AND A.MIBICLS IN (1,2)";
                SQL = SQL + ComNum.VBLF + "                  AND (DECODE(A.MEDDEPTCD, 'MC', 'MD', 'MP','MD','MN','MD','ME','MD','MR','MD','MC','MD','MG','MD','MI','MD','MO','MD','HU','GS',A.MEDDEPTCD) = B.DEPTCD  OR A.MEDDEPTCD = B.DEPTCD)";
                SQL = SQL + ComNum.VBLF + "                  AND A.MEDDRCD = B.USEID";
                SQL = SQL + ComNum.VBLF + "              GROUP BY A.MEDENDDATE, A.MEDDEPTCD, B.DEPTNAME, B.USENAME) A,";
                SQL = SQL + ComNum.VBLF + "              (SELECT ";
                SQL = SQL + ComNum.VBLF + "                   b.DEPTNAME, a.MedEndDate, a.MedDeptCd, b.USENAME, ";
                SQL = SQL + ComNum.VBLF + "                  COUNT(DISTINCT A.PTNO) AS MIBICNT";
                SQL = SQL + ComNum.VBLF + "              From " + ComNum.DB_EMR + "EMRMIBI A, " + ComNum.DB_EMR + "VIEWBUSER B";
                SQL = SQL + ComNum.VBLF + "              WHERE MEDENDDATE BETWEEN '" + strMiBiFrDate2 + "' AND '" + strMiBiToDate2 + "'";
                SQL = SQL + ComNum.VBLF + "                  AND A.MIBICLS IN (1)";
                SQL = SQL + ComNum.VBLF + "                  AND (DECODE(A.MEDDEPTCD, 'MC', 'MD', 'MP','MD','MN','MD','ME','MD','MR','MD','MC','MD','MG','MD','MI','MD','MO','MD','HU','GS',A.MEDDEPTCD) = B.DEPTCD  OR A.MEDDEPTCD = B.DEPTCD)";
                SQL = SQL + ComNum.VBLF + "                  AND A.MEDDRCD = B.USEID";
                SQL = SQL + ComNum.VBLF + "              GROUP BY A.MEDENDDATE, A.MEDDEPTCD, B.DEPTNAME, B.USENAME) B";
                SQL = SQL + ComNum.VBLF + "              WHERE A.DEPTNAME = B.DEPTNAME(+)";
                SQL = SQL + ComNum.VBLF + "                  AND A.MEDDEPTCD = B.MEDDEPTCD(+)";
                SQL = SQL + ComNum.VBLF + "                  AND A.USENAME = B.USENAME(+)";
                SQL = SQL + ComNum.VBLF + "                  AND A.MEDENDDATE = B.MEDENDDATE(+)";
                if (VB.Left(cboDept.Text, 2) != "**")
                {
                    SQL = SQL + ComNum.VBLF + " AND A.MEDDEPTCD = '" + VB.Right(cboDept.Text, 2) + "' ";
                }
                SQL = SQL + ComNum.VBLF + " ) G";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_PMPA + "MID_DEPT D";
                SQL = SQL + ComNum.VBLF + "  ON D.DEPTCODE = G.MEDDEPTCD";
                SQL = SQL + ComNum.VBLF + "GROUP BY D.DEPTNAMEK, G.MEDDEPTCD, PRINTRANK";
                SQL = SQL + ComNum.VBLF + "ORDER BY PRINTRANK";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                    return;
                }

                Row = 1;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssStatisticsView1_Sheet1.RowCount = ssStatisticsView1_Sheet1.RowCount + 1;

                    ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 0].Text = VB.Space(5) + "⊙" + VB.Space(5) + dt.Rows[i]["DEPTNAMEK"].ToString().Trim() + VB.Space(1) + "(" + dt.Rows[i]["MEDDEPTCD"].ToString().Trim() + ")";
                    ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["MIBICNT"].ToString().Trim();
                    ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["BALCNT"].ToString().Trim();
                }

                ssStatisticsView1_Sheet1.RowCount = ssStatisticsView1_Sheet1.RowCount + 1;

                font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
                ssStatisticsView1_Sheet1.Rows[ssStatisticsView1_Sheet1.RowCount - 1].Font = font;
                ssStatisticsView1_Sheet1.Rows[ssStatisticsView1_Sheet1.RowCount - 1].BackColor = Color.FromArgb(255, 215, 255);

                Row = dt.Rows.Count;

                ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 1].Formula = "SUM(B1:B" + Row + ")";
                ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 2].Formula = "SUM(C1:C" + Row + ")";
                ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 0].Text = "합          계";

                ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 0].HorizontalAlignment = CellHorizontalAlignment.Center;

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void GetMibiStatistics5()
        {
            int i = 0;
            int j = 0;
            double nCnt = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strMiBiFrDate2 = "";
            string strMiBiToDate2 = "";

            strMiBiFrDate2 = (dtpMiBiFrDate2.Value).ToString("yyyyMMdd");
            strMiBiToDate2 = (dtpMiBiToDate2.Value).ToString("yyyyMMdd");

            ssStatisticsView1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT  USENAME AS DRNAME,";
                SQL = SQL + ComNum.VBLF + "                SUM(MIBICNT) AS MIBICNT,";
                SQL = SQL + ComNum.VBLF + "                SUM(BALCNT) As BALCNT";
                SQL = SQL + ComNum.VBLF + "   From ( SELECT A.USEID, A.MEDENDDATE, A.USENAME,";
                SQL = SQL + ComNum.VBLF + "                             NVL(A.MIBIBALCNT,0) AS BALCNT, NVL(B.MIBICNT, 0) AS MIBICNT";
                SQL = SQL + ComNum.VBLF + "                 From ( SELECT A.MEDDRCD USEID, A.MEDENDDATE, B.USENAME, COUNT(DISTINCT A.PTNO) AS MIBIBALCNT";
                SQL = SQL + ComNum.VBLF + "                 From " + ComNum.DB_EMR + "EMRMIBI A, " + ComNum.DB_EMR + "VIEWBUSER B";
                SQL = SQL + ComNum.VBLF + "                WHERE MEDENDDATE BETWEEN '" + strMiBiFrDate2 + "' AND '" + strMiBiToDate2 + "'";
                SQL = SQL + ComNum.VBLF + "                     AND A.MIBICLS IN (1,2)";
                SQL = SQL + ComNum.VBLF + "                     AND A.MEDDRCD = B.USEID";
                SQL = SQL + ComNum.VBLF + "                 GROUP BY A.MEDDRCD, A.MEDENDDATE, B.DEPTNAME, B.USENAME";
                SQL = SQL + ComNum.VBLF + "                 ) A, ( SELECT A.MEDDRCD USEID, A.MEDENDDATE, B.USENAME, COUNT(DISTINCT A.PTNO) AS MIBICNT";
                SQL = SQL + ComNum.VBLF + "                 From " + ComNum.DB_EMR + "EMRMIBI A, " + ComNum.DB_EMR + "VIEWBUSER B";
                SQL = SQL + ComNum.VBLF + "                WHERE MEDENDDATE BETWEEN '" + strMiBiFrDate2 + "' AND '" + strMiBiToDate2 + "'";
                SQL = SQL + ComNum.VBLF + "                     AND A.MIBICLS IN (1)";
                SQL = SQL + ComNum.VBLF + "                     AND A.MEDDRCD = B.USEID";
                SQL = SQL + ComNum.VBLF + "                 GROUP BY A.MEDDRCD, A.MEDENDDATE, B.DEPTNAME, B.USENAME) B";
                SQL = SQL + ComNum.VBLF + "                 WHERE A.USEID = B.USEID(+)";
                SQL = SQL + ComNum.VBLF + "                     AND A.MEDENDDATE = B.MEDENDDATE(+) )";
                SQL = SQL + ComNum.VBLF + "   GROUP BY USENAME";
                SQL = SQL + ComNum.VBLF + "   ORDER BY DRNAME ASC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssStatisticsView1_Sheet1.RowCount = ssStatisticsView1_Sheet1.RowCount + 1;

                    //if (dt.Rows[i]["DRNAME"].ToString().Trim() == "조선영")
                    //{
                    //    Row = Row;
                    //}

                    ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                    ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["MIBICNT"].ToString().Trim();
                    ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["BALCNT"].ToString().Trim();
                    ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 3].Text = READ_AccumMibiCase(dt.Rows[i]["DRNAME"].ToString().Trim(), strMiBiToDate2).ToString().Trim();
                }

                ssStatisticsView1_Sheet1.RowCount = ssStatisticsView1_Sheet1.RowCount + 1;

                ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, 0].Text = "총  계";

                for (i = 1; i < ssStatisticsView1_Sheet1.ColumnCount; i++)
                {
                    nCnt = 0;

                    for (j = 0; j < ssStatisticsView1_Sheet1.RowCount - 1; j++)
                    {
                        nCnt = nCnt + (int)VB.Val(ssStatisticsView1_Sheet1.Cells[j, i].Text);
                    }

                    ssStatisticsView1_Sheet1.Cells[ssStatisticsView1_Sheet1.RowCount - 1, i].Text = nCnt.ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private int READ_AccumMibiCase(string ArgName, string ArgDate, string argUSEID = "")
        {
            int i = 0;
            int nCnt = 0;

            int rtnVal = 0;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            //if (ArgName == "권기훈")
            //{
            //    ArgName = ArgName;
            //}

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT COUNT(MUMIBICNT) MUMIBICNT FROM (";
                SQL = SQL + ComNum.VBLF + "   SELECT COUNT(DISTINCT(PTNO)) MUMIBICNT, MEDFRDATE";
                SQL = SQL + ComNum.VBLF + "        From ADMIN.EMRMIBI A, ADMIN.VIEWBUSER B, ADMIN.IPD_NEW_MASTER  C";
                SQL = SQL + ComNum.VBLF + "         WHERE A.MIBICLS IN (1)";
                SQL = SQL + ComNum.VBLF + "             AND A.MEDENDDATE <= '" + ArgDate + "'";
                SQL = SQL + ComNum.VBLF + "             AND A.MEDDRCD = B.USEID";
                SQL = SQL + ComNum.VBLF + "             AND A.MEDENDDATE = TO_CHAR(C.OUTDATE, 'YYYYMMDD')";
                SQL = SQL + ComNum.VBLF + "             AND A.PTNO = C.PANO";
                SQL = SQL + ComNum.VBLF + "             AND C.GBSTS <> '9'";
                SQL = SQL + ComNum.VBLF + "             AND B.USENAME = '" + ArgName + "'";
                SQL = SQL + ComNum.VBLF + "  GROUP BY PTNO, MEDFRDATE) ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                    return rtnVal;
                }

                if (dt.Rows.Count == 1)
                {
                    rtnVal = Convert.ToInt32(dt.Rows[0]["MUMIBICNT"].ToString().Trim());
                }
                else
                {
                    nCnt = 0;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nCnt = nCnt + Convert.ToInt32(dt.Rows[0]["MUMIBICNT"].ToString().Trim());
                    }

                    rtnVal = nCnt;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            return rtnVal;
        }

        private int RowAddCheck(string strMedDeptCd)
        {
            int refRow = -1;
            int refCol = -1;

            ssStatisticsView1.Search(ssStatisticsView1.ActiveSheetIndex, strMedDeptCd, false, false, false, false, 0, 0, ref refRow, ref refCol);

            return refRow;
            //int i = 0;

            //for (i = 0; i < ssStatisticsView1_Sheet1.RowCount; i++)
            //{
            //    if (ssStatisticsView1_Sheet1.Cells[i, 0].Text.Trim() == strMedDeptCd)
            //    {
            //        return i;
            //    }
            //}

            //return -1;
        }

        private string GetMiBiTitleNm(string strMiBiGrp)
        {
            switch (strMiBiGrp.Trim())
            {
                case "A":
                    strMiBiGrp = "입퇴원요약지";
                    break;
                case "B":
                    strMiBiGrp = "동의서";
                    break;
                case "C":
                    strMiBiGrp = "입원기록지";
                    break;
                case "D":
                    strMiBiGrp = "경과기록지";
                    break;
                case "E":
                    strMiBiGrp = "수술기록지";
                    break;
            }

            return strMiBiGrp;
        }

        /// <summary>
        /// 환자정보 조회
        /// </summary>
        private void GetPatientInfoSearch()
        {
            //환자정보조회
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strPtno = (rdotabssView0.Checked ? txtSearch : txtSearch2).Text.Trim();

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (VB.IsNumeric(strPtno) != true)
                {
                    ComFunc.MsgBoxEx(this, "환자조회는 등록번호만 조회 가능합니다.", "확인요망");
                }
                else if (VB.IsNumeric(strPtno) == true)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT A.PTNO, A.PTNAME, A.SSNO1, A.SSNO2, A.BIRTHDATE, A.TEL1, A.CELPHNO, ";
                    SQL = SQL + ComNum.VBLF + "              A.ZIPCD,  A.ZIPCDSEQNO, A.ADDR, A.ACPCHKFLAG, A.REMARK, A.EMAIL";
                    SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "VIEWBPT A ";
                    SQL = SQL + ComNum.VBLF + " WHERE A.PTNO = '" + strPtno + "'";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                        return;
                    }

                    (rdotabssView0.Checked ? txtSearch : txtSearch2).Text = strPtno;
                    (rdotabssView0.Checked ? lblName : lblName2).Text = dt.Rows[0]["PTNAME"].ToString().Trim();

                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void SET_COMBO_DEPT()
        {
            int iC = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            cboDept.Items.Clear();
            cboDept.Items.Add("**.전체");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //SQL = "SELECT DEPTCODE, DEPTNAMEK FROM ADMIN.BAS_CLINICDEPT";
                //SQL = SQL + ComNum.VBLF + "WHERE DEPTCODE NOT IN('OC', 'II','R6','HR','TO','PT','HC','OM','LM')";
                //SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING";

                SQL = "SELECT DEPTCODE, DEPTNAMEK FROM ADMIN.MID_DEPT";
                SQL = SQL + ComNum.VBLF + "WHERE DEPTCODE NOT IN('OC', 'II','R6','HR','TO','PT','HC','OM','LM')";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANK";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (iC = 0; iC < dt.Rows.Count; iC++)
                    {
                        cboDept.Items.Add(dt.Rows[iC]["DEPTNAMEK"].ToString().Trim() + "." + dt.Rows[iC]["DEPTCODE"].ToString().Trim());
                    }
                }

                cboDept.SelectedIndex = 0;

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void txtSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            lblName.Text = "";
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            if (rdoSearchType0.Checked == true)
            {
                txtSearch.Text = VB.Val(txtSearch.Text).ToString("00000000");
                GetPatientInfoSearch();
            }
            else
            {
                if (VB.Trim(txtSearch.Text) == "")
                {
                    txtSearch.Text = "";
                    lblName.Text = "";
                }
                else
                {
                    Cursor.Current = Cursors.WaitCursor;

                    try
                    {
                        SQL = "";
                        SQL = "SELECT USERID, PASSWD, NAME, AUTH, PRINTAUTH, EDATE, CLINCODE, NPVIEW " + " ";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMR_USERT ";
                        SQL = SQL + ComNum.VBLF + " WHERE (USERID = '" + VB.UCase(VB.Trim(txtSearch.Text)) + "'  or REPLACE(NAME,' ') = '" + VB.UCase(VB.Trim(txtSearch.Text)) + "' ) ";

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt.Rows.Count == 0)
                        {
                            dt.Dispose();
                            dt = null;
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBoxEx(this, "존재하지 않는 사용자입니다.");
                            txtSearch.Text = "";
                            lblName.Text = "";
                            return;
                        }
                        else if (dt.Rows.Count == 1)
                        {
                            txtSearch.Text = dt.Rows[0]["UserID"].ToString().Trim();
                            lblName.Text = dt.Rows[0]["NAME"].ToString().Trim();
                            txtSearch.Focus();
                        }
                        else if (dt.Rows.Count > 0)
                        {
                            GetDoctorInfo();
                            txtSearch.Focus();
                        }
                        txtSearch.Focus();

                        dt.Dispose();
                        dt = null;
                        Cursor.Current = Cursors.Default;
                    }
                    catch (Exception ex)
                    {
                        if (dt != null)
                        {
                            dt.Dispose();
                            dt = null;
                        }

                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBoxEx(this, ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    }
                }
            }
        }

        /// <summary>
        /// 의사정보 가져오기
        /// </summary>
        void GetDoctorInfo()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "SELECT USERID, PASSWD, NAME, AUTH, PRINTAUTH, EDATE, CLINCODE, NPVIEW ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMR_USERT ";
                SQL = SQL + ComNum.VBLF + " WHERE (USERID = '" + txtSearch.Text.Trim().ToUpper() + "'  or REPLACE(NAME,' ') = '" + txtSearch.Text.Trim().ToUpper() + "' ) ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    ComFunc.MsgBoxEx(this, "존재하지 않는 사용자입니다.");
                    lblName.Text = "";
                    txtSearch.Text = "";
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }
                else if (dt.Rows.Count == 1)
                {
                    lblName.Text = dt.Rows[0]["NAME"].ToString().Trim();
                    txtSearch.Text = dt.Rows[0]["UserID"].ToString().Trim();
                }
                else
                {
                    Form frmDocSelect = new Form();
                    FpSpread spd = new FpSpread();
                    SheetView sheet = new SheetView();
                    FarPoint.Win.Spread.CellType.TextCellType txtCellType = new FarPoint.Win.Spread.CellType.TextCellType();
                    txtCellType.Static = true;
                        
                    spd.Sheets.Add(sheet);
                    spd.HorizontalScrollBarPolicy = ScrollBarPolicy.Never;
                    spd.ActiveSheet.ColumnCount = 2;
                    spd.ActiveSheet.Columns[0].CellType = txtCellType;
                    spd.ActiveSheet.Columns[0].HorizontalAlignment = CellHorizontalAlignment.Center;
                    spd.ActiveSheet.Columns[0].Label = "아이디";
                    spd.ActiveSheet.Columns[0].Width = 60;
                    spd.ActiveSheet.Columns[1].CellType = txtCellType;
                    spd.ActiveSheet.Columns[1].Label = "이름";
                    spd.ActiveSheet.Columns[1].Width = 70;
                    spd.ActiveSheet.Columns[1].VerticalAlignment = CellVerticalAlignment.Center;
                    spd.ActiveSheet.RowCount = 0;
                    spd.Dock = DockStyle.Fill;
                    spd.CellClick += Spd_CellClick;
                    spd.CellDoubleClick += Spd_CellDoubleClick;
                    spd.ActiveSheet.RowCount = dt.Rows.Count;
                    spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        spd.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["UserID"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["NAME"].ToString().Trim();
                    }

                    frmDocSelect.Controls.Add(spd);
                    frmDocSelect.Width = 188;
                    frmDocSelect.Height = 134;
                    frmDocSelect.StartPosition = FormStartPosition.Manual;
                    frmDocSelect.Location = new Point(Width - 552, 280);
                    frmDocSelect.FormBorderStyle = FormBorderStyle.None;
                    frmDocSelect.ControlBox = false;
                    frmDocSelect.ShowDialog();
                    frmDocSelect.Dispose();

                    spd.Dispose();
                    sheet.Dispose();
                    txtCellType.Dispose();
                    spd = null;
                    sheet = null;
                    txtCellType = null;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void Spd_CellClick(object sender, CellClickEventArgs e)
        {
            FpSpread spd = (FpSpread)sender;
            if (spd.ActiveSheet.RowCount == 0) return;

            if (e.Row == -1) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(spd, e.Column);
                return;
            }

            spd.ActiveSheet.Cells[0, 0, spd.ActiveSheet.RowCount - 1, spd.ActiveSheet.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            spd.ActiveSheet.Cells[e.Row, 0, e.Row, spd.ActiveSheet.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
            spd = null;
        }

        private void Spd_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            FpSpread spd = (FpSpread)sender;
            if (spd.ActiveSheet.RowCount == 0) return;

            if (e.Row == -1) return;

            txtSearch.Text = spd.ActiveSheet.Cells[e.Row, 0].Text.Trim();
            lblName.Text = spd.ActiveSheet.Cells[e.Row, 1].Text.Trim();

            spd.Parent.Dispose();
            spd = null;
        }

        #region 주석처리
        //private string GetMiBiNm(string strMibiCd, string strMibiRmk)
        //{
        //    string rtnVal = "";

        //    switch (strMibiCd)
        //    {
        //        case "A01":
        //            rtnVal = "＊누락";
        //            break;
        //        case "A02":
        //            rtnVal = "＊주진단명";
        //            break;
        //        case "A03":
        //            rtnVal = "＊기타진단명";
        //            break;
        //        case "A04":
        //            rtnVal = "＊퇴원시환자상태";
        //            break;
        //        case "A05":
        //            rtnVal = "＊주수술(처치)명";
        //            break;
        //        case "A06":
        //            rtnVal = "＊기타수술(처치)명";
        //            break;
        //        case "A07":
        //            rtnVal = "＊퇴원형태";
        //            break;
        //        case "A08":
        //            rtnVal = "＊C/C. P/I";
        //            break;
        //        case "A09":
        //            rtnVal = "＊검사결과";
        //            break;
        //        case "A10":
        //            rtnVal = "＊추후치료계획";
        //            break;
        //        case "A11":
        //            rtnVal = strMibiRmk;
        //            break;

        //        case "B01":
        //            rtnVal = "*누락";
        //            break;
        //        case "B02":
        //            rtnVal = "*환자의 인적사항(등록번호,성명,나이,셩별,진료과)";
        //            break;
        //        case "B03":
        //            rtnVal = "*진단명";
        //            break;
        //        case "B04":
        //            rtnVal = "*환자상태 또는 특이사항";
        //            break;
        //        case "B05":
        //            rtnVal = "*예정된 의료행위 종류 (수술 및 시술명)";
        //            break;
        //        case "B06":
        //            rtnVal = "*예정된 의료행위의 목적 및 필요성";
        //            break;
        //        case "B07":
        //            rtnVal = "*예정된 의료행위 방법";
        //            break;
        //        case "B08":
        //            rtnVal = "*회복과 관련하여 발생 할 수 있는 문제";
        //            break;
        //        case "B09":
        //            rtnVal = "*예정된 의료행위 이외의 시행 가능한 다른 방법";
        //            break;
        //        case "B10":
        //            rtnVal = "*예정된 의료행위가 시행되지 않았을 때의 결과";
        //            break;
        //        case "B11":
        //            rtnVal = "*설명의사의 서명";
        //            break;
        //        case "B12":
        //            rtnVal = "*환자의 서명 혹은 동의권자의 자필서명";
        //            break;
        //        case "B13":
        //            rtnVal = "*환자가 서명 할 수 없어 동의권자가 서명한 사유";
        //            break;
        //        case "B14":
        //            rtnVal = "*동의서 작성일시";
        //            break;

        //        case "C01":
        //            rtnVal = "＊누락";
        //            break;
        //        case "C02":
        //            rtnVal = "＊C/C";
        //            break;
        //        case "C03":
        //            rtnVal = "＊P/I";
        //            break;
        //        case "C04":
        //            rtnVal = "＊P/H";
        //            break;
        //        case "C05":
        //            rtnVal = "＊PE";
        //            break;
        //        case "C06":
        //            rtnVal = "＊ROS";
        //            break;
        //        case "C07":
        //            rtnVal = "＊Imp";
        //            break;
        //        case "C08":
        //            rtnVal = "＊Plan";
        //            break;
        //        case "C09":
        //            rtnVal = strMibiRmk;
        //            break;

        //        case "D01":
        //            rtnVal = "＊누락";
        //            break;
        //        case "D02":
        //            rtnVal = "＊횟수부족";
        //            break;
        //        case "D03":
        //            rtnVal = "＊특수검사기록";
        //            break;
        //        case "D04":
        //            rtnVal = "＊처치및시술기록";
        //            break;
        //        case "D05":
        //            rtnVal = "＊전과기록";
        //            break;
        //        case "D06":
        //            rtnVal = "＊수술기록";
        //            break;
        //        case "D07":
        //            rtnVal = "＊퇴원지시";
        //            break;
        //        case "D08":
        //            rtnVal = strMibiRmk;
        //            break;


        //        case "E01":
        //            rtnVal = "＊누락";
        //            break;
        //        case "E02":
        //            rtnVal = "＊환자정보";
        //            break;
        //        case "E03":
        //            rtnVal = "＊수술전진단";
        //            break;
        //        case "E04":
        //            rtnVal = "＊수술후진단";
        //            break;
        //        case "E05":
        //            rtnVal = "＊수술명";
        //            break;
        //        case "E06":
        //            rtnVal = "＊마취방법";
        //            break;
        //        case "E07":
        //            rtnVal = "＊수술관찰소견";
        //            break;
        //        case "E08":
        //            rtnVal = "＊수술절차";
        //            break;
        //        case "E09":
        //            rtnVal = strMibiRmk;
        //            break;
        //    }

        //    return rtnVal;
        //}
        #endregion


        string GetMiBiNm(string strMibiCd, string strMibiRmk)
        {
            switch (strMibiCd)
            {
                case "A01":
                    strMibiCd = "＊누락";
                    break;
                case "A02":
                    strMibiCd = "＊주진단명";
                    break;
                case "A03":
                    strMibiCd = "＊부진단명";
                    break;
                case "A04":
                    strMibiCd = "＊퇴원시환자상태";
                    break;
                case "A05":
                    strMibiCd = "＊주수술(처치)명";
                    break;
                case "A06":
                    strMibiCd = "＊기타수술(처치)명";
                    break;
                case "A07":
                    strMibiCd = "＊퇴원형태";
                    break;
                case "A08":
                    strMibiCd = "＊C/C. P/I";
                    break;
                case "A09":
                    strMibiCd = "＊검사결과";
                    break;
                case "A10":
                    strMibiCd = "＊추후치료계획";
                    break;
                case "A11":
                    strMibiCd = strMibiRmk;
                    break;
                case "A12":
                    strMibiCd = "*경과요약";
                    break;
                case "A13":
                    strMibiCd = "*삭제";
                    break;
                case "A14":
                    strMibiCd = "*POA";
                    break;                //'Case "A11": GetMiBiNm = "＊기타(" & strMibiRmk & ")"

                //''        Case "B01": GetMiBiNm = "＊누락"
                //''        Case "B02": GetMiBiNm = "＊병명 및 수술명"
                //''        Case "B03": GetMiBiNm = "＊수술내용설명"
                //''        Case "B04": GetMiBiNm = "＊날짜"
                //''        Case "B05": GetMiBiNm = "＊부작용"
                //''        Case "B06": GetMiBiNm = "＊보호자서명"
                //''        Case "B07": GetMiBiNm = "＊의사서명"
                //''        Case "B08": GetMiBiNm = strMibiRmk
                //''        'Case "B08": GetMiBiNm = "＊기타(" & strMibiRmk & ")"
                case "B01":
                    strMibiCd = "*누락";
                    break;
                case "B02":
                    strMibiCd = "*의료행위 전 작성여부";
                    break;
                case "B03":
                    strMibiCd = "*환자인적사항";
                    break;
                case "B04":
                    strMibiCd = "*목적 필요성 및 방법";
                    break;
                case "B05":
                    strMibiCd = "*발생 할 수 있는 문제";
                    break;
                case "B06":
                    strMibiCd = "*대안 및 미시행결과";
                    break;
                case "B07":
                    strMibiCd = "*설명의사서명";
                    break;
                case "B08":
                    strMibiCd = "*동의권자서명";
                    break;
                case "B09":
                    strMibiCd = "*동의권자서명사유";
                    break;
                case "B10":
                    strMibiCd = "*설명일시";
                    break;
                case "B11":
                    strMibiCd = strMibiRmk;
                    break;

                case "C01":
                    strMibiCd = "＊누락";
                    break;
                case "C02":
                    strMibiCd = "＊C/C";
                    break;
                case "C03":
                    strMibiCd = "＊P/I";
                    break;
                case "C04":
                    strMibiCd = "＊P/H";
                    break;
                case "C05":
                    strMibiCd = "＊PE";
                    break;
                case "C06":
                    strMibiCd = "＊ROS";
                    break;
                case "C07":
                    strMibiCd = "＊Imp";
                    break;
                case "C08":
                    strMibiCd = "＊Plan";
                    break;
                case "C09":
                    strMibiCd = strMibiRmk;
                    break;
                case "C10":
                    strMibiCd = "＊삭제";
                    break;
                //'Case "C09": GetMiBiNm = "＊기타(" & strMibiRmk & ")"
                case "D01":
                    strMibiCd = "＊누락";
                    break;
                case "D02":
                    strMibiCd = "＊횟수부족(3일1회기준작성)";
                    break;
                case "D03":
                    strMibiCd = "＊특수검사기록";
                    break;
                case "D04":
                    strMibiCd = "＊처치및시술기록";
                    break;
                case "D05":
                    strMibiCd = "＊전과기록";
                    break;
                //'Case "D06": GetMiBiNm = "＊수술기록"             ; break;
                case "D06":
                    strMibiCd = "＊수술후환자상태";
                    break;
                case "D07":
                    strMibiCd = "＊퇴원지시";
                    break;
                case "D09":
                    strMibiCd = "＊SOAP항목";
                    break;
                case "D10":
                    strMibiCd = "＊의학적재평가";
                    break;
                case "D11":
                    strMibiCd = "＊전출";
                    break;
                case "D12":
                    strMibiCd = "＊전입";
                    break;
                case "D08":
                    strMibiCd = strMibiRmk;
                    break;
                //'Case "D08": GetMiBiNm = "＊기타(" & strMibiRmk & ")"
                case "E01":
                    strMibiCd = "＊누락";
                    break;
                case "E02":
                    strMibiCd = "＊환자정보";
                    break;
                case "E03":
                    strMibiCd = "＊수술전진단";
                    break;
                case "E04":
                    strMibiCd = "＊수술후진단";
                    break;
                case "E05":
                    strMibiCd = "＊수술명";
                    break;
                case "E06":
                    strMibiCd = "＊마취방법";
                    break;
                case "E07":
                    strMibiCd = "＊수술관찰소견";
                    break;
                case "E08":
                    strMibiCd = "＊수술절차";
                    break;
                case "E10":
                    strMibiCd = "＊조직검체표본";
                    break;
                case "E11":
                    strMibiCd = "＊배액";
                    break;
                case "E12":
                    strMibiCd = "＊출혈량";
                    break;
                case "E13":
                    strMibiCd = "＊시술기록지";
                    break;
                case "E09":
                    strMibiCd = strMibiRmk;
                    break;
                    //'Case "E09": GetMiBiNm = "＊기타(" & strMibiRmk & ")"
            }
            return strMibiCd;
        }

        //TODO: frm특정수가조회 에 있는 READ_BAS_Doctor 함수 임시로 만들어 사용
        private string ReadBASDoctor(string argDrCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";
            DataTable dt = null;

            if (argDrCode.Trim() == "")
            {
                rtnVal = "";
            }
            try
            {

                SQL = "";
                SQL += ComNum.VBLF + "SELECT DrName FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL += ComNum.VBLF + " WHERE DrCode='" + argDrCode + "' ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["DrName"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
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

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private string ReadJewonIlsu(string argPTNO, string argOUTDATE)
        {
            string rtnVal = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT ILSU ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "     AND OUTDATE = TO_DATE('" + argOUTDATE + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["ILSU"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void SpreadSetting(string strIndex)
        {
            //스프레드 세팅
            FarPoint.Win.Spread.CellType.NumberCellType numberCell = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCell = new FarPoint.Win.Spread.CellType.TextCellType();
            textCell.Static = true;
            numberCell.Static = true;
            numberCell.NullDisplay = "0";

            if (VB.IsNumeric(strIndex) == false) strIndex = VB.Right(strIndex, 1);

            switch (strIndex)
            {
                case "3": //서식지별
                    //스프레드 설정
                    ssStatisticsView1_Sheet1.RowCount = 0;
                    //서식지
                    ssStatisticsView1_Sheet1.Columns[0].Width = 183;
                    ssStatisticsView1_Sheet1.Columns[0].CellType = textCell;
                    ssStatisticsView1_Sheet1.Columns[0].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssStatisticsView1_Sheet1.Columns[0].VerticalAlignment = CellVerticalAlignment.Center;
                    //진료과
                    ssStatisticsView1_Sheet1.Columns[1].Width = 183;
                    ssStatisticsView1_Sheet1.Columns[1].CellType = textCell;
                    ssStatisticsView1_Sheet1.Columns[1].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssStatisticsView1_Sheet1.Columns[1].VerticalAlignment = CellVerticalAlignment.Center;
                    //미비건수
                    ssStatisticsView1_Sheet1.Columns[2].Width = 183;
                    ssStatisticsView1_Sheet1.Columns[2].CellType = textCell;
                    ssStatisticsView1_Sheet1.Columns[2].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssStatisticsView1_Sheet1.Columns[2].VerticalAlignment = CellVerticalAlignment.Center;
                    //발생건수
                    ssStatisticsView1_Sheet1.Columns[3].Width = 183;
                    ssStatisticsView1_Sheet1.Columns[3].CellType = textCell;
                    ssStatisticsView1_Sheet1.Columns[3].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssStatisticsView1_Sheet1.Columns[3].VerticalAlignment = CellVerticalAlignment.Center;

                    //Headers명 설정
                    ssStatisticsView1_Sheet1.Columns[0].Label = "서식지";
                    ssStatisticsView1_Sheet1.Columns[1].Label = "진료과";
                    ssStatisticsView1_Sheet1.Columns[2].Label = "미비건수";
                    ssStatisticsView1_Sheet1.Columns[3].Label = "발생건수";

                    ssStatisticsView1_Sheet1.Columns[0].Visible = true;
                    ssStatisticsView1_Sheet1.Columns[1].Visible = true;
                    ssStatisticsView1_Sheet1.Columns[2].Visible = true;
                    ssStatisticsView1_Sheet1.Columns[3].Visible = true;
                    ssStatisticsView1_Sheet1.Columns[4].Visible = false;
                    ssStatisticsView1_Sheet1.Columns[5].Visible = false;
                    ssStatisticsView1_Sheet1.Columns[6].Visible = false;
                    break;

                case "4": //의사별
                    ssStatisticsView1_Sheet1.RowCount = 0;

                    //스프레드 설정

                    //진료과
                    ssStatisticsView1_Sheet1.Columns[0].Width = 183;
                    ssStatisticsView1_Sheet1.Columns[0].CellType = textCell;
                    ssStatisticsView1_Sheet1.Columns[0].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssStatisticsView1_Sheet1.Columns[0].VerticalAlignment = CellVerticalAlignment.Center;
                    //진료의사
                    ssStatisticsView1_Sheet1.Columns[1].Width = 183;
                    ssStatisticsView1_Sheet1.Columns[1].CellType = textCell;
                    ssStatisticsView1_Sheet1.Columns[1].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssStatisticsView1_Sheet1.Columns[1].VerticalAlignment = CellVerticalAlignment.Center;
                    //미비건수
                    ssStatisticsView1_Sheet1.Columns[2].Width = 183;
                    ssStatisticsView1_Sheet1.Columns[2].CellType = textCell;
                    ssStatisticsView1_Sheet1.Columns[2].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssStatisticsView1_Sheet1.Columns[2].VerticalAlignment = CellVerticalAlignment.Center;
                    //발생건수
                    ssStatisticsView1_Sheet1.Columns[3].Width = 183;
                    ssStatisticsView1_Sheet1.Columns[3].CellType = textCell;
                    ssStatisticsView1_Sheet1.Columns[3].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssStatisticsView1_Sheet1.Columns[3].VerticalAlignment = CellVerticalAlignment.Center;

                    //Headers명 설정
                    ssStatisticsView1_Sheet1.Columns[0].Label = "진료과";
                    ssStatisticsView1_Sheet1.Columns[1].Label = "진료의사";
                    ssStatisticsView1_Sheet1.Columns[2].Label = "미비건수";
                    ssStatisticsView1_Sheet1.Columns[3].Label = "발생건수";

                    ssStatisticsView1_Sheet1.Columns[0].Visible = true;
                    ssStatisticsView1_Sheet1.Columns[1].Visible = true;
                    ssStatisticsView1_Sheet1.Columns[2].Visible = true;
                    ssStatisticsView1_Sheet1.Columns[3].Visible = true;
                    ssStatisticsView1_Sheet1.Columns[4].Visible = false;
                    ssStatisticsView1_Sheet1.Columns[5].Visible = false;
                    ssStatisticsView1_Sheet1.Columns[6].Visible = false;
                    break;

                case "10": //누적
                    ssStatisticsView1_Sheet1.RowCount = 0;

                    //스프레드 설정

                    //진료과
                    ssStatisticsView1_Sheet1.Columns[0].Width = 183;
                    ssStatisticsView1_Sheet1.Columns[0].CellType = textCell;
                    ssStatisticsView1_Sheet1.Columns[0].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssStatisticsView1_Sheet1.Columns[0].VerticalAlignment = CellVerticalAlignment.Center;
                    //진료의사
                    ssStatisticsView1_Sheet1.Columns[1].Width = 183;
                    ssStatisticsView1_Sheet1.Columns[1].CellType = numberCell;
                    ssStatisticsView1_Sheet1.Columns[1].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssStatisticsView1_Sheet1.Columns[1].VerticalAlignment = CellVerticalAlignment.Center;
                    numberCell.DecimalPlaces = 0;
                    //미비건수
                    ssStatisticsView1_Sheet1.Columns[2].Width = 183;
                    ssStatisticsView1_Sheet1.Columns[2].CellType = numberCell;
                    ssStatisticsView1_Sheet1.Columns[2].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssStatisticsView1_Sheet1.Columns[2].VerticalAlignment = CellVerticalAlignment.Center;
                    numberCell.DecimalPlaces = 0;
                    //발생건수
                    ssStatisticsView1_Sheet1.Columns[3].Width = 183;
                    ssStatisticsView1_Sheet1.Columns[3].CellType = numberCell;
                    ssStatisticsView1_Sheet1.Columns[3].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssStatisticsView1_Sheet1.Columns[3].VerticalAlignment = CellVerticalAlignment.Center;
                    numberCell.DecimalPlaces = 0;

                    //Headers명 설정
                    ssStatisticsView1_Sheet1.Columns[0].Label = "진료의사";
                    ssStatisticsView1_Sheet1.Columns[1].Label = "미비건수";
                    ssStatisticsView1_Sheet1.Columns[2].Label = "발생건수";
                    ssStatisticsView1_Sheet1.Columns[3].Label = "누적미비건수";

                    ssStatisticsView1_Sheet1.Columns[0].Visible = true;
                    ssStatisticsView1_Sheet1.Columns[1].Visible = true;
                    ssStatisticsView1_Sheet1.Columns[2].Visible = true;
                    ssStatisticsView1_Sheet1.Columns[3].Visible = true;
                    ssStatisticsView1_Sheet1.Columns[4].Visible = false;
                    ssStatisticsView1_Sheet1.Columns[5].Visible = false;
                    ssStatisticsView1_Sheet1.Columns[6].Visible = false;
                    break;

                case "8": //진료과별
                    ssStatisticsView1_Sheet1.RowCount = 0;

                    //스프레드 설정

                    //진료과
                    ssStatisticsView1_Sheet1.Columns[0].Width = 245;
                    ssStatisticsView1_Sheet1.Columns[0].CellType = textCell;
                    ssStatisticsView1_Sheet1.Columns[0].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssStatisticsView1_Sheet1.Columns[0].VerticalAlignment = CellVerticalAlignment.Center;
                    //미비건수
                    ssStatisticsView1_Sheet1.Columns[1].Width = 245;
                    ssStatisticsView1_Sheet1.Columns[1].CellType = numberCell;
                    ssStatisticsView1_Sheet1.Columns[1].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssStatisticsView1_Sheet1.Columns[1].VerticalAlignment = CellVerticalAlignment.Center;
                    numberCell.DecimalPlaces = 0;
                    numberCell.ShowSeparator = true;
                    //발생건수
                    ssStatisticsView1_Sheet1.Columns[2].Width = 245;
                    ssStatisticsView1_Sheet1.Columns[2].CellType = numberCell;
                    ssStatisticsView1_Sheet1.Columns[2].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssStatisticsView1_Sheet1.Columns[2].VerticalAlignment = CellVerticalAlignment.Center;
                    numberCell.DecimalPlaces = 0;
                    numberCell.ShowSeparator = true;

                    //Headers명 설정
                    ssStatisticsView1_Sheet1.Columns[0].Label = "진료과";
                    ssStatisticsView1_Sheet1.Columns[1].Label = "미비건수";
                    ssStatisticsView1_Sheet1.Columns[2].Label = "발생건수";

                    ssStatisticsView1_Sheet1.Columns[0].Visible = true;
                    ssStatisticsView1_Sheet1.Columns[1].Visible = true;
                    ssStatisticsView1_Sheet1.Columns[2].Visible = true;
                    ssStatisticsView1_Sheet1.Columns[3].Visible = false;
                    ssStatisticsView1_Sheet1.Columns[4].Visible = false;
                    ssStatisticsView1_Sheet1.Columns[5].Visible = false;
                    ssStatisticsView1_Sheet1.Columns[6].Visible = false;
                    break;

                case "9":
                    ssStatisticsView1_Sheet1.RowCount = 0;

                    //스프레드 설정

                    //진료과
                    ssStatisticsView1_Sheet1.Columns[0].Width = 183;
                    ssStatisticsView1_Sheet1.Columns[0].CellType = textCell;
                    ssStatisticsView1_Sheet1.Columns[0].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssStatisticsView1_Sheet1.Columns[0].VerticalAlignment = CellVerticalAlignment.Center;
                    //진료의사
                    ssStatisticsView1_Sheet1.Columns[1].Width = 183;
                    ssStatisticsView1_Sheet1.Columns[1].CellType = textCell;
                    ssStatisticsView1_Sheet1.Columns[1].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssStatisticsView1_Sheet1.Columns[1].VerticalAlignment = CellVerticalAlignment.Center;
                    //미비건수
                    ssStatisticsView1_Sheet1.Columns[2].Width = 183;
                    ssStatisticsView1_Sheet1.Columns[2].CellType = textCell;
                    ssStatisticsView1_Sheet1.Columns[2].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssStatisticsView1_Sheet1.Columns[2].VerticalAlignment = CellVerticalAlignment.Center;
                    //발생건수
                    ssStatisticsView1_Sheet1.Columns[3].Width = 183;
                    ssStatisticsView1_Sheet1.Columns[3].CellType = textCell;
                    ssStatisticsView1_Sheet1.Columns[3].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssStatisticsView1_Sheet1.Columns[3].VerticalAlignment = CellVerticalAlignment.Center;

                    //Headers명 설정
                    ssStatisticsView1_Sheet1.Columns[0].Label = "진료의사";
                    ssStatisticsView1_Sheet1.Columns[1].Label = "진료과";
                    ssStatisticsView1_Sheet1.Columns[2].Label = "미비건수";
                    ssStatisticsView1_Sheet1.Columns[3].Label = "발생건수";

                    ssStatisticsView1_Sheet1.Columns[0].Visible = true;
                    ssStatisticsView1_Sheet1.Columns[1].Visible = true;
                    ssStatisticsView1_Sheet1.Columns[2].Visible = true;
                    ssStatisticsView1_Sheet1.Columns[3].Visible = true;
                    ssStatisticsView1_Sheet1.Columns[4].Visible = false;
                    ssStatisticsView1_Sheet1.Columns[5].Visible = false;
                    ssStatisticsView1_Sheet1.Columns[6].Visible = false;
                    break;

                case "5": // 월별
                    textCell.NullDisplay = "0";
                    ssStatisticsView1_Sheet1.RowCount = 0;
                    //스프레드 설정

                    //진료과코드
                    ssStatisticsView1_Sheet1.Columns[0].Width = 175;
                    ssStatisticsView1_Sheet1.Columns[0].CellType = textCell;
                    ssStatisticsView1_Sheet1.Columns[0].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssStatisticsView1_Sheet1.Columns[0].VerticalAlignment = CellVerticalAlignment.Center;
                    ssStatisticsView1_Sheet1.Columns[0].Visible = false;
                    //진료과
                    ssStatisticsView1_Sheet1.Columns[1].Width = 175;
                    ssStatisticsView1_Sheet1.Columns[1].CellType = textCell;
                    ssStatisticsView1_Sheet1.Columns[1].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssStatisticsView1_Sheet1.Columns[1].VerticalAlignment = CellVerticalAlignment.Center;
                    //퇴원건수
                    ssStatisticsView1_Sheet1.Columns[2].Width = 190;
                    ssStatisticsView1_Sheet1.Columns[2].CellType = numberCell;
                    ssStatisticsView1_Sheet1.Columns[2].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssStatisticsView1_Sheet1.Columns[2].VerticalAlignment = CellVerticalAlignment.Center;
                    numberCell.DecimalPlaces = 0;
                    numberCell.ShowSeparator = true;
                    //미비발생건수
                    ssStatisticsView1_Sheet1.Columns[3].Width = 190;
                    ssStatisticsView1_Sheet1.Columns[3].CellType = numberCell;
                    ssStatisticsView1_Sheet1.Columns[3].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssStatisticsView1_Sheet1.Columns[3].VerticalAlignment = CellVerticalAlignment.Center;
                    numberCell.DecimalPlaces = 0;
                    numberCell.ShowSeparator = true;
                    //발생율
                    ssStatisticsView1_Sheet1.Columns[4].Width = 165;
                    ssStatisticsView1_Sheet1.Columns[4].CellType = textCell;
                    ssStatisticsView1_Sheet1.Columns[4].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssStatisticsView1_Sheet1.Columns[4].VerticalAlignment = CellVerticalAlignment.Center;

                    //미비건수
                    ssStatisticsView1_Sheet1.Columns[5].Width = 190;
                    ssStatisticsView1_Sheet1.Columns[5].CellType = numberCell;
                    ssStatisticsView1_Sheet1.Columns[5].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssStatisticsView1_Sheet1.Columns[5].VerticalAlignment = CellVerticalAlignment.Center;
                    numberCell.DecimalPlaces = 0;
                    numberCell.ShowSeparator = true;
                    //누적 미비건수
                    ssStatisticsView1_Sheet1.Columns[6].Width = 190;
                    ssStatisticsView1_Sheet1.Columns[6].CellType = numberCell;
                    ssStatisticsView1_Sheet1.Columns[6].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssStatisticsView1_Sheet1.Columns[6].VerticalAlignment = CellVerticalAlignment.Center;
                    numberCell.DecimalPlaces = 0;
                    numberCell.ShowSeparator = true;

                    //Headers명 설정
                    ssStatisticsView1_Sheet1.Columns[0].Label = "진료과코드";
                    ssStatisticsView1_Sheet1.Columns[1].Label = "진료과";
                    ssStatisticsView1_Sheet1.Columns[2].Label = VB.Format(DateTime.Today, "MM") + "월 퇴원건수";
                    ssStatisticsView1_Sheet1.Columns[3].Label = VB.Format(DateTime.Today, "MM") + "월 미비발생 건수";
                    ssStatisticsView1_Sheet1.Columns[4].Label = "발생율(%)";
                    ssStatisticsView1_Sheet1.Columns[5].Label = VB.Format(DateTime.Today, "MM") + "월 미비건수";
                    ssStatisticsView1_Sheet1.Columns[6].Label = "누적 미비건수";

                    ssStatisticsView1_Sheet1.Columns[0].Visible = false;
                    ssStatisticsView1_Sheet1.Columns[1].Visible = true;
                    ssStatisticsView1_Sheet1.Columns[2].Visible = true;
                    ssStatisticsView1_Sheet1.Columns[3].Visible = true;
                    ssStatisticsView1_Sheet1.Columns[4].Visible = true;
                    ssStatisticsView1_Sheet1.Columns[5].Visible = true;
                    ssStatisticsView1_Sheet1.Columns[6].Visible = true;
                    break;
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            FpSpread ssObject = rdotabssView0.Checked ? ssStatisticsView0 : ssStatisticsView1;
            SaveFileDialog mDlg = new SaveFileDialog();
            mDlg.InitialDirectory = Application.StartupPath;
            mDlg.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
            mDlg.FilterIndex = 1;
            if (mDlg.ShowDialog() == DialogResult.OK)
            {
                ssObject.SaveExcel(mDlg.FileName,
                FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders | FarPoint.Excel.ExcelSaveFlags.DataOnly);
            }

            mDlg.Dispose();
            mDlg = null;
        }

        private void vaForeColor(FpSpread vaSS, int Row, int Col, int Row2, int Col2, Color argColor)
        {
            vaSS.ActiveSheet.Cells[Row, Col, Row2, Col2].ForeColor = argColor;
        }

        private void vaBackColor(FpSpread vaSS, int Row, int Col, int Row2, int Col2, Color argColor)
        {
            vaSS.ActiveSheet.Cells[Row, Col, Row2, Col2].BackColor = argColor;
        }

        private void rdoSearchType_CheckedChanged(object sender, EventArgs e)
        {
            chkCheck.Visible = rdoSearchType1.Checked || rdoSearchType4.Checked;
            SpreadSetting((VB.Right((sender as RadioButton).Name, 2)));
            progressBar1.Value = 0;
            progressBar1.Maximum = 0;

            if(sender == rdoSearchType2 || sender == rdoSearchType3 || sender == rdoSearchType4 ||
               sender == rdoSearchType5 || sender == rdoSearchType8 || sender == rdoSearchType9 ||
               sender == rdoSearchType10)
            {
                dtpMiBiFrDate2.Value = MedicalInfoTeam ? Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(-3) : Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                dtpMiBiToDate2.Value = MedicalInfoTeam ? Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(-3) : Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            }

            ssStatisticsView0_Sheet1.RowCount = 0;
            ssStatisticsView1_Sheet1.RowCount = 0;

            if (rdoSearchType0.Checked == true || rdoSearchType1.Checked == true)
            {
                //dtpMiBiFrDate.Enabled = false;
                //dtpMiBiToDate.Enabled = false;

                dtpMiBiFrDate.Visible = false;
                dtpMiBiToDate.Visible = false;

                txtSearch.SetBounds(513, 5, 103, 25);
                lblName.SetBounds(618, 5, 103, 25);

                txtSearch.Text = "";
                lblName.Text = "";
            }

            if (rdoSearchType2.Checked == true || rdoSearchType6.Checked == true || rdoSearchType7.Checked == true)
            {

                //dtpMiBiFrDate.Enabled = true;
                //dtpMiBiToDate.Enabled = true;
                dtpMiBiFrDate.Visible = true;
                dtpMiBiToDate.Visible = true;

                txtSearch.SetBounds(724, 5, 103, 25);
                lblName.SetBounds(830, 5, 103, 25);
            }

            if (rdoSearchType3.Checked == true)
            {
                txtSearch2.Visible = false;
                lblName2.Visible = false;
                cboSearchType.Visible = true;
                cboSearchType.Items.Clear();

                cboSearchType.Items.Add("");
                cboSearchType.Items.Add("입퇴원요약지" + VB.Space(50) + "A");
                cboSearchType.Items.Add("동의서" + VB.Space(50) + "B");
                cboSearchType.Items.Add("입원기록지" + VB.Space(50) + "C");
                cboSearchType.Items.Add("경과기록지" + VB.Space(50) + "D");
                cboSearchType.Items.Add("수술기록지" + VB.Space(50) + "E");

                dtpMiBiFrDate2.ShowUpDown = false;
                dtpMiBiFrDate2.Width = 103;
                dtpMiBiToDate2.Visible = true;
                txtSearch2.Visible = false;
                lblName2.Visible = false;
                btnUp.Visible = false;
                btnDown.Visible = false;
            }

            if (rdoSearchType4.Checked == true || rdoSearchType8.Checked == true || rdoSearchType9.Checked == true || rdoSearchType10.Checked == true)
            {
                txtSearch2.Visible = false;
                lblName2.Visible = false;
                cboSearchType.Visible = false;
                dtpMiBiFrDate2.Width = 103;
                dtpMiBiFrDate2.ShowUpDown = false;
                dtpMiBiToDate2.Visible = true;
                btnUp.Visible = false;
                btnDown.Visible = false;
            }

            if (rdoSearchType5.Checked == true)
            {
                cboSearchType.Items.Clear();
                txtSearch2.Visible = true;
                lblName2.Visible = true;
                cboSearchType.Visible = false;

                btnUp.Visible = true;
                btnDown.Visible = true;

                dtpMiBiFrDate2.Width = 75;
                dtpMiBiFrDate2.ShowUpDown = true;
                dtpMiBiToDate2.Visible = false;
                txtSearch2.Visible = false;
                lblName2.Visible = false;
            }

            chkCheck.Visible = (rdoSearchType1.Checked || rdoSearchType4.Checked);
        }

        private void setSSRowMaxHeight(FpSpread ssSpread)
        {
            int i = 0;

            for (i = 0; i < ssSpread.ActiveSheet.RowCount; i++)
            {
                //Application.DoEvents();
                ssSpread.ActiveSheet.SetRowHeight(i, (int) ssSpread.ActiveSheet.GetPreferredRowHeight(i) + 4);
            }
        }

        private void rdotabssView_CheckedChanged(object sender, EventArgs e)
        {
            chkER.Visible = rdotabssView0.Checked;
            Font font = new Font("맑은고딕", 10f, FontStyle.Bold);

            if (rdotabssView0.Checked == true)
            {
                rdotabssView0.Font = font;
                font = new Font("맑은고딕", 9.75f, FontStyle.Regular);
                rdotabssView1.Font = font;
                rdotabssView0.BackColor = Color.RoyalBlue;
                rdotabssView1.BackColor = Color.White;
                rdotabssView0.ForeColor = Color.White;
                rdotabssView1.ForeColor = Color.Black;
                panMibi2.Dock = DockStyle.None;
                panSpread2.Dock = DockStyle.None;
                panMibi2.Visible = false;
                panSpread2.Visible = false;

                panMibi1.Visible = true;
                panSpread1.Visible = true;
                panMibi1.Dock = DockStyle.Fill;
                panSpread1.Dock = DockStyle.Fill;
            }

            if (rdotabssView1.Checked == true)
            {
                rdotabssView1.Font = font;
                font = new Font("맑은고딕", 9.75f, FontStyle.Regular);
                rdotabssView0.Font = font;
                rdotabssView1.BackColor = Color.RoyalBlue;
                rdotabssView0.BackColor = Color.White;
                rdotabssView1.ForeColor = Color.White;
                rdotabssView0.ForeColor = Color.Black;
                panMibi1.Dock = DockStyle.None;
                panSpread1.Dock = DockStyle.None;
                panMibi1.Visible = false;
                panSpread1.Visible = false;

                panMibi2.Visible = true;
                panSpread2.Visible = true;
                panMibi2.Dock = DockStyle.Fill;
                panSpread2.Dock = DockStyle.Fill;
            }

            font.Dispose();
            font = null;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }

            string strMiBiFrDate = "";
            string strMiBiToDate = "";
            string strMiBiFrDate2 = "";
            string strMiBiToDate2 = "";
            FpSpread ssObject = null;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            PrintOrientation Pot = PrintOrientation.Portrait;
            bool bRowHeaderVisible = true;
  
            if (rdotabssView0.Checked == true)
            {
                ssObject = ssStatisticsView0;
                strMiBiFrDate = (dtpMiBiFrDate.Value).ToString("yyyy년MM월dd일");
                strMiBiToDate = (dtpMiBiToDate.Value).ToString("yyyy년MM월dd일");
            }
            else if (rdotabssView1.Checked == true)
            {
                ssObject = ssStatisticsView1;
                strMiBiFrDate2 = (dtpMiBiFrDate2.Value).ToString("yyyy년MM월dd일");
                strMiBiToDate2 = (dtpMiBiToDate2.Value).ToString("yyyy년MM월dd일");
            }

            btnPrint.Enabled = false;

            string strSysDateTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A");
            DateTime dtSysDate = Convert.ToDateTime(strSysDateTime);

            if (rdotabssView0.Checked == true)
            {
                if (rdoSearchType0.Checked == true)
                {
                    strTitle = "(환자별) 미비현황";

                    strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                    strHeader += CS.setSpdPrint_String("출력일자 : " + dtSysDate.ToString("yyyy년MM월dd일 HH시mm분") + " 출력자 : " + VB.Trim(clsType.User.Sabun), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);
                }
                else if (rdoSearchType1.Checked == true)
                {
                    strTitle = "(의사별) 미비현황";

                    strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                    strHeader += CS.setSpdPrint_String("출력일자 : " + dtSysDate.ToString("yyyy년MM월dd일 HH시mm분") + " 출력자 : " + VB.Trim(clsType.User.Sabun), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);
                }
                else if (rdoSearchType2.Checked == true)
                {
                    strTitle = "(기간별) 미비현황";

                    strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                    strHeader += CS.setSpdPrint_String("조회기간 : " + strMiBiFrDate + " ~ " + strMiBiToDate, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);
                    strHeader += CS.setSpdPrint_String("출력일자 : " + dtSysDate.ToString("yyyy년 MM월 dd일") + " 출력자 : " + clsType.User.Sabun, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, false);
                }
                else if (rdoSearchType6.Checked == true)
                {
                    strTitle = "(미비) 현황";

                    strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                    strHeader += CS.setSpdPrint_String("조회기간 : " + strMiBiFrDate + " ~ " + strMiBiToDate, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);
                    strHeader += CS.setSpdPrint_String("출력일자 : " + dtSysDate.ToString("yyyy년 MM월 dd일") + " 출력자 : " + clsType.User.Sabun, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, false);
                }
                else if (rdoSearchType7.Checked == true)
                {
                    strTitle = "(완료) 현황";

                    strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                    strHeader += CS.setSpdPrint_String("조회기간 : " + strMiBiFrDate + " ~ " + strMiBiToDate, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);
                    strHeader += CS.setSpdPrint_String("출력일자 : " + dtSysDate.ToString("yyyy년 MM월 dd일") + " 출력자 : " + clsType.User.Sabun, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, false);
                }
                Pot = PrintOrientation.Landscape;
            }

            if (rdotabssView1.Checked == true)
            {
                if (rdoSearchType3.Checked == true)
                {
                    strTitle = "(서식지별) 미비통계";

                    strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                    strHeader += CS.setSpdPrint_String("조회기간 : " + strMiBiFrDate2 + " ~ " + strMiBiToDate2, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);
                }
                else if (rdoSearchType4.Checked == true)
                {
                    strTitle = "(의사별) 미비통계";

                    strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                    strHeader += CS.setSpdPrint_String("조회기간 : " + strMiBiFrDate2 + " ~ " + strMiBiToDate2, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);

                    bRowHeaderVisible = false;
                }
                else if (rdoSearchType9.Checked == true)
                {
                    strTitle = "(MD) 미비통계";

                    strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                    strHeader += CS.setSpdPrint_String("조회기간 : " + strMiBiFrDate2 + " ~ " + strMiBiToDate2, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);
                }
                else if (rdoSearchType10.Checked == true)
                {
                    strTitle = "(의사별) 미비누적건수";

                    strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                    strHeader += CS.setSpdPrint_String("조회기간 : " + strMiBiFrDate2 + " ~ " + strMiBiToDate2, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);

                }
                else if (rdoSearchType5.Checked == true)
                {
                    strTitle = "(월별) 미비통계";

                    strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                    strHeader += CS.setSpdPrint_String("조회기간 : " + dtpMiBiFrDate2.Value.ToString("yyyy년MM월01일") + " ~ " + dtpMiBiFrDate2.Value.ToString("yyyy년MM월") +  DateTime.DaysInMonth(dtpMiBiFrDate2.Value.Year, dtpMiBiFrDate2.Value.Month) + "일", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);
                    Pot = PrintOrientation.Landscape;
                    bRowHeaderVisible = false;
                }
                else if (rdoSearchType8.Checked == true)
                {
                    strTitle = "(과별 ) 미비통계";

                    strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                    strHeader += CS.setSpdPrint_String("조회기간 : " + strMiBiFrDate2 + " ~ " + strMiBiToDate2, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);
                }
            }

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, bRowHeaderVisible ? 20 : 50, 10);
            setOption = new clsSpread.SpdPrint_Option(Pot, PrintType.All, 0, 0, true, bRowHeaderVisible, true, true, false, false, false);

            CS.setSpdPrint(ssObject, PrePrint, setMargin, setOption, strHeader, strFooter);
            CS = null;

            btnPrint.Enabled = true;
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (ssStatisticsView1_Sheet1.ActiveRowIndex <= 1)
            {
                return;
            }

            if (ssStatisticsView1_Sheet1.ActiveRowIndex > ssStatisticsView1_Sheet1.NonEmptyRowCount)
            {
                return;
            }

            ssStatisticsView1_Sheet1.RowCount = ssStatisticsView1_Sheet1.RowCount + 1;

            MoveSlip(ssStatisticsView1_Sheet1.ActiveRowIndex, ssStatisticsView1_Sheet1.ActiveRowIndex - 1, ssStatisticsView1);
            ssStatisticsView1_Sheet1.RowCount = ssStatisticsView1_Sheet1.RowCount - 1;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (ssStatisticsView1_Sheet1.ActiveRowIndex < 1)
            {
                return;
            }

            if (ssStatisticsView1_Sheet1.ActiveRowIndex >= ssStatisticsView1_Sheet1.NonEmptyRowCount)
            {
                return;
            }

            ssStatisticsView1_Sheet1.RowCount = ssStatisticsView1_Sheet1.RowCount + 1;

            MoveSlip(ssStatisticsView1_Sheet1.ActiveRowIndex, ssStatisticsView1_Sheet1.ActiveRowIndex + 2, ssStatisticsView1);
            ssStatisticsView1_Sheet1.RowCount = ssStatisticsView1_Sheet1.RowCount - 1;
        }

        private void MoveSlip(int Row, int NewRow, FpSpread sSpread)
        {

            sSpread.ActiveSheet.SwapRange(Row, 0, NewRow, 0, 1, sSpread.ActiveSheet.ColumnCount, false);


            //sSpread.Row = NewRow
            //sSpread.Action = 7


            //If Row < NewRow Then
            //    sSpread.Col = -1:       sSpread.Row = Row
            //    sSpread.Col2 = -1:      sSpread.Row2 = Row
            //    sSpread.DestCol = 1:    sSpread.DestRow = NewRow
            //    sSpread.Action = 20
            //    sSpread.Row = Row
            //    sSpread.Action = 5
            //    sSpread.Col = sSpread.ActiveCol:    sSpread.Row = NewRow - 1
            //    sSpread.Action = 0
            //Else
            //    sSpread.Col = -1:       sSpread.Row = Row + 1
            //    sSpread.Col2 = -1:      sSpread.Row2 = Row + 1
            //    sSpread.DestCol = 1:    sSpread.DestRow = NewRow
            //    sSpread.Action = 20
            //    sSpread.Row = Row + 1
            //    sSpread.Action = 5
            //    sSpread.Col = sSpread.ActiveCol:    sSpread.Row = NewRow
            //    sSpread.Action = 0
            //End If


            //If sSpread.MaxRows = sSpread.DataRowCnt Then sSpread.MaxRows = sSpread.DataRowCnt + 1
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCommand_Click(object sender, EventArgs e)
        {
            if(File.Exists("C:\\의사미비현황.XLS"))
            {
                File.Delete("C:\\의사미비현황.XLS");
            }

            if (ssStatisticsView0.SaveExcel("C:\\의사미비현황.XLS", FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders))
            {
                ComFunc.MsgBoxEx(this, "C드라이브의 의사미비현황.xls를 확인하십시요.", "확인");
            }
            else
            {
                ComFunc.MsgBoxEx(this, "파일 출력실패", "실패");
            }
        }

        private string gSave_Excel(string strFileName, FpSpread SS)
        {
            string rtnVal = "";
            //TODO: 엑셀저장
            //    Dim f       As Integer
            //    Dim i       As Integer
            //    Dim j       As Integer
            //    Dim OneRec  As String '그리드 한행의 내용을 가지고 있는 변수


            //    f = FreeFile()
            //    OneRec = ""
            //    On Error GoTo Err
            //    Open strFileName For Output As #f


            //    'Data를 출력함
            //    gSave_Excel = "T"

            //    For i = 0 To SS.MaxRows

            //        For j = 1 To SS.MaxCols
            //            SS.Row = i: SS.Col = j
            //            If SS.ColHidden = False Then '스레드 보이는 그대로 엑셀에서 뿌려주기
            //                If j = SS.MaxCols Then
            //                    OneRec = OneRec & CStr(SS.Text)              ' 맨 마지막 Column
            //                Else
            //                    OneRec = OneRec & Replace(CStr(SS.Text), vbCrLf, " ") & Chr(9)
            //                End If
            //            End If
            //        Next j

            //        Print #f, OneRec
            //        OneRec = ""

            //    Next i

            //    Close #f

            //    MsgBox "엑셀저장작업이 완료되었습니다", vbOKOnly, "작업성공"
            //    Exit Function


            //Err:
            //            MsgBox "  엑셀파일이 열려있습니다. " & vbLf & _
            //               " 확인 후 다시 저장하세요.", vbCritical
            //        gSave_Excel = "F"

            return rtnVal;
        }

        private void txtSearch2_Click(object sender, EventArgs e)
        {
            txtSearch2.Text = "";
            lblName2.Text = "";
        }

        private void txtSearch2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return)
            {
                return;
            }

            txtSearch2.Text = VB.Val(txtSearch2.Text).ToString("00000000");
            GetPatientInfoSearch();
        }

        private void dtpMiBiFrDate2_ValueChanged(object sender, EventArgs e)
        {
            if (rdoSearchType5.Checked)
            {
                ssStatisticsView1_Sheet1.Columns[2].Label = dtpMiBiFrDate2.Value.ToString("M월") + " 퇴원건수";
                ssStatisticsView1_Sheet1.Columns[3].Label = dtpMiBiFrDate2.Value.ToString("M월") + " 미비발생 건수";
                ssStatisticsView1_Sheet1.Columns[5].Label = dtpMiBiFrDate2.Value.ToString("M월") + " 미비건수";
            }
        }

        private void ssStatisticsView0_CellClick(object sender, CellClickEventArgs e)
        {
            if (ssStatisticsView0_Sheet1.RowCount == 0) return;

            if (e.Row == -1) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssStatisticsView0, e.Column);
                return;
            }
        }

        private void ssStatisticsView1_CellClick(object sender, CellClickEventArgs e)
        {
            if (ssStatisticsView1_Sheet1.RowCount == 0) return;

            if (e.Row == -1) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssStatisticsView1, e.Column);
                return;
            }
        }

        private void cboSearchType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lblName2_Click(object sender, EventArgs e)
        {

        }

        private void txtSearch2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dtpMiBiToDate2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void frmEmrJobMiBiStatisticsTewon_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }

        private void frmEmrJobMiBiStatisticsTewon_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }
    }
}
