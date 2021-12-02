using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComEmrBase
{

    /// <summary>
    /// Class Name      : ComEmrBase
    /// File Name       : frmEmrBaseSympOld
    /// Description     : 미비통계
    /// Author          : 박웅규
    /// Create Date     : 2018-05-10
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// TODO : 폼 호출, 엑셀저장
    /// </history>
    /// <seealso cref= "PSMHVB\\Ocs\OpdOcs\Oorder\mtsoorder.vbp(PSMHVB\mtsEmr\frmTextEmrMibi.frm) >> frmEmrBaseTextEmrMibiOld.cs 폼이름 재정의" />
    public partial class frmEmrBaseTextEmrMibiOld : Form
    {
        Form mCallForm = null;
        string mPTNO = "";
        string mInDate = "";
        string mOutDate = "";

        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        //기록지 관련 : 작성된 기록지 호출
        public delegate void SetMibiPat(string strPtNo, string strINDATE, string strOutDate);
        public event SetMibiPat rSetMibiPat;

        public frmEmrBaseTextEmrMibiOld()
        {
            InitializeComponent();
        }

        public frmEmrBaseTextEmrMibiOld(Form pCallForm, string strPtNo, string strINDATE, string strOutDate)
        {
            InitializeComponent();
            mCallForm = pCallForm;
            mPTNO = strPtNo;
            mInDate = strINDATE;
            mOutDate = strOutDate;
        }

        private void frmEmrBaseTextEmrMibiOld_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssMiBi_Sheet1.RowCount = 0;

            GetMibiList();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) //권한 확인
            {
                return;
            }
            GetMibiList();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                rEventClosed();
            }
            else
            {
                this.Close();
            }
        }

        private void GetMibiList()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            //int j = 0;
            string strPtno1 = "";
            //string strPtno2 = "";
            string strMedEndDate = "";

            float mHisMaxRowHeightOld = 0;
            float mHisMaxRowHeightNew = 0;

            ssMiBi_Sheet1.RowCount = 0;

            SQL = " SELECT A.PTNO, B.SNAME AS PTNAME, A.MIBIINDATE, A.MIBIINTIME,";
            SQL = SQL + ComNum.VBLF + "        A.MEDFRDATE, A.MEDENDDATE, A.MIBIGRP, A.MIBICD, A.MIBIRMK ";
            SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.EMRMIBI A, KOSMOS_PMPA.BAS_PATIENT B ";
            if (clsType.User.DeptCode == "MD" || clsType.User.IdNumber == "31606" || clsType.User.IdNumber == "34241" )
            {
                SQL = SQL + ComNum.VBLF + "    WHERE A.MEDDEPTCD IN ('MG','MP','MN','ME','MC','MR','MD','MI') ";
            }
            else if (clsType.User.IdNumber == "1367")
            {
                SQL = SQL + ComNum.VBLF + "    WHERE A.MEDDEPTCD IN ('GS','HU') ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "    WHERE A.MEDDEPTCD IN ('" + clsType.User.DeptCode + "') ";
            }
            SQL = SQL + ComNum.VBLF + "    AND A.MEDDRCD = '" + clsType.User.IdNumber + "' ";
            SQL = SQL + ComNum.VBLF + "    AND A.MIBIFNDATE IS NULL";
            SQL = SQL + ComNum.VBLF + "    AND A.MIBICLS = 1";
            SQL = SQL + ComNum.VBLF + "    AND A.PTNO = B.PANO ";
            SQL = SQL + ComNum.VBLF + "    ORDER BY B.SNAME, A.MEDFRDATE, A.MIBIGRP, MIBIRMK DESC, MIBICD ";

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
                dt.Dispose();
                dt = null;
                ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (strPtno1 != dt.Rows[i]["PTNO"].ToString().Trim() || strMedEndDate != dt.Rows[i]["MEDENDDATE"].ToString().Trim())
                {
                    if (i != 0)
                    {
                        ssMiBi_Sheet1.SetRowHeight(ssMiBi_Sheet1.RowCount - 1, (int)mHisMaxRowHeightOld);
                    }

                    if (i != 0)
                    {
                        //윗줄의 미비 현황을 체크 한다
                        if (ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 12].Text.Trim() != "")
                        {
                            if (MiBiStatusCheck(strPtno1, "A", strMedEndDate) == true)
                            {
                                ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 12].Text = "(완료)" + ComNum.VBLF + ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 12].Text.Trim();
                                ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 12].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                                ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 12].ForeColor = Color.Blue;
                            }
                        }
                        if (ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 13].Text.Trim() != "")
                        {
                            if (MiBiStatusCheck(strPtno1, "B", strMedEndDate) == true)
                            {
                                ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 13].Text = "(완료)" + ComNum.VBLF + ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 12].Text.Trim();
                                ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 13].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                                ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 13].ForeColor = Color.Blue;
                            }
                        }
                        if (ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 14].Text.Trim() != "")
                        {
                            if (MiBiStatusCheck(strPtno1, "B", strMedEndDate) == true)
                            {
                                ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 14].Text = "(완료)" + ComNum.VBLF + ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 12].Text.Trim();
                                ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 14].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                                ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 14].ForeColor = Color.Blue;
                            }
                        }
                        if (ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 15].Text.Trim() != "")
                        {
                            if (MiBiStatusCheck(strPtno1, "C", strMedEndDate) == true)
                            {
                                ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 15].Text = "(완료)" + ComNum.VBLF + ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 12].Text.Trim();
                                ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 15].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                                ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 15].ForeColor = Color.Blue;
                            }
                        }
                        if (ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 16].Text.Trim() != "")
                        {
                            if (MiBiStatusCheck(strPtno1, "D", strMedEndDate) == true)
                            {
                                ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 16].Text = "(완료)" + ComNum.VBLF + ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 12].Text.Trim();
                                ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 16].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                                ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 16].ForeColor = Color.Blue;
                            }
                        }
                    }

                    ssMiBi_Sheet1.RowCount = ssMiBi_Sheet1.RowCount + 1;
                    mHisMaxRowHeightOld = 0;
                    mHisMaxRowHeightNew = 0;
                    ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                    ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNAME"].ToString().Trim();
                    ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 2].Text = ComFunc.FormatStrToDate(dt.Rows[i]["MEDFRDATE"].ToString().Trim(), "D");
                    ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 3].Text = ComFunc.FormatStrToDate(dt.Rows[i]["MEDENDDATE"].ToString().Trim(), "D");

                    strPtno1 = dt.Rows[i]["PTNO"].ToString().Trim();
                    strMedEndDate = dt.Rows[i]["MEDENDDATE"].ToString().Trim();
                }

                switch (dt.Rows[i]["MIBIGRP"].ToString().Trim())
                {
                    case "A":
                        if (ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 12].Text.Trim() != "")
                        {
                            ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 12].Text = GetMiBiNm(dt.Rows[i]["MIBICD"].ToString().Trim(), dt.Rows[i]["MIBIRMK"].ToString().Trim());
                        }
                        else
                        {
                            ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 12].Text = ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 12].Text + ComNum.VBLF + GetMiBiNm(dt.Rows[i]["MIBICD"].ToString().Trim(), dt.Rows[i]["MIBIRMK"].ToString().Trim());
                        }
                        ssMiBi_Sheet1.SetRowHeight(ssMiBi_Sheet1.RowCount - 1, Convert.ToInt32(ssMiBi_Sheet1.GetPreferredRowHeight(ssMiBi_Sheet1.RowCount - 1)) + 25);

                        mHisMaxRowHeightNew = ssMiBi_Sheet1.Rows[ssMiBi_Sheet1.RowCount - 1].Height;
                        if (mHisMaxRowHeightNew > mHisMaxRowHeightOld)
                        {
                            mHisMaxRowHeightOld = mHisMaxRowHeightNew;
                        }
                        break;
                    case "B":
                        if (ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 13].Text.Trim() != "")
                        {
                            ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 13].Text = GetMiBiNm(dt.Rows[i]["MIBICD"].ToString().Trim(), dt.Rows[i]["MIBIRMK"].ToString().Trim());
                        }
                        else
                        {
                            ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 13].Text = ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 12].Text + ComNum.VBLF + GetMiBiNm(dt.Rows[i]["MIBICD"].ToString().Trim(), dt.Rows[i]["MIBIRMK"].ToString().Trim());
                        }
                        ssMiBi_Sheet1.SetRowHeight(ssMiBi_Sheet1.RowCount - 1, Convert.ToInt32(ssMiBi_Sheet1.GetPreferredRowHeight(ssMiBi_Sheet1.RowCount - 1)) + 25);

                        mHisMaxRowHeightNew = ssMiBi_Sheet1.Rows[ssMiBi_Sheet1.RowCount - 1].Height;
                        if (mHisMaxRowHeightNew > mHisMaxRowHeightOld)
                        {
                            mHisMaxRowHeightOld = mHisMaxRowHeightNew;
                        }
                        break;
                    case "C":
                        if (ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 14].Text.Trim() != "")
                        {
                            ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 14].Text = GetMiBiNm(dt.Rows[i]["MIBICD"].ToString().Trim(), dt.Rows[i]["MIBIRMK"].ToString().Trim());
                        }
                        else
                        {
                            ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 14].Text = ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 12].Text + ComNum.VBLF + GetMiBiNm(dt.Rows[i]["MIBICD"].ToString().Trim(), dt.Rows[i]["MIBIRMK"].ToString().Trim());
                        }
                        ssMiBi_Sheet1.SetRowHeight(ssMiBi_Sheet1.RowCount - 1, Convert.ToInt32(ssMiBi_Sheet1.GetPreferredRowHeight(ssMiBi_Sheet1.RowCount - 1)) + 25);

                        mHisMaxRowHeightNew = ssMiBi_Sheet1.Rows[ssMiBi_Sheet1.RowCount - 1].Height;
                        if (mHisMaxRowHeightNew > mHisMaxRowHeightOld)
                        {
                            mHisMaxRowHeightOld = mHisMaxRowHeightNew;
                        }
                        break;
                    case "D":
                        if (ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 15].Text.Trim() != "")
                        {
                            ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 15].Text = GetMiBiNm(dt.Rows[i]["MIBICD"].ToString().Trim(), dt.Rows[i]["MIBIRMK"].ToString().Trim());
                        }
                        else
                        {
                            ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 15].Text = ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 12].Text + ComNum.VBLF + GetMiBiNm(dt.Rows[i]["MIBICD"].ToString().Trim(), dt.Rows[i]["MIBIRMK"].ToString().Trim());
                        }
                        ssMiBi_Sheet1.SetRowHeight(ssMiBi_Sheet1.RowCount - 1, Convert.ToInt32(ssMiBi_Sheet1.GetPreferredRowHeight(ssMiBi_Sheet1.RowCount - 1)) + 25);

                        mHisMaxRowHeightNew = ssMiBi_Sheet1.Rows[ssMiBi_Sheet1.RowCount - 1].Height;
                        if (mHisMaxRowHeightNew > mHisMaxRowHeightOld)
                        {
                            mHisMaxRowHeightOld = mHisMaxRowHeightNew;
                        }
                        break;
                    case "E":
                        if (ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 16].Text.Trim() != "")
                        {
                            ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 16].Text = GetMiBiNm(dt.Rows[i]["MIBICD"].ToString().Trim(), dt.Rows[i]["MIBIRMK"].ToString().Trim());
                        }
                        else
                        {
                            ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 16].Text = ssMiBi_Sheet1.Cells[ssMiBi_Sheet1.RowCount - 1, 12].Text + ComNum.VBLF + GetMiBiNm(dt.Rows[i]["MIBICD"].ToString().Trim(), dt.Rows[i]["MIBIRMK"].ToString().Trim());
                        }
                        ssMiBi_Sheet1.SetRowHeight(ssMiBi_Sheet1.RowCount - 1, Convert.ToInt32(ssMiBi_Sheet1.GetPreferredRowHeight(ssMiBi_Sheet1.RowCount - 1)) + 25);

                        mHisMaxRowHeightNew = ssMiBi_Sheet1.Rows[ssMiBi_Sheet1.RowCount - 1].Height;
                        if (mHisMaxRowHeightNew > mHisMaxRowHeightOld)
                        {
                            mHisMaxRowHeightOld = mHisMaxRowHeightNew;
                        }
                        break;
                }
            }

            dt.Dispose();
            dt = null;
        }

        private bool MiBiStatusCheck(string strPtNo , string strMiBiGPR , string strMedEndDate)
        {
            //int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT MIBIGRP";
            SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.EMRMIBI  ";
            SQL = SQL + ComNum.VBLF + "WHERE PTNO = '" + strPtNo + "'";
            SQL = SQL + ComNum.VBLF + "  AND MEDENDDATE = '" + strMedEndDate + "'";
            if (clsType.User.DeptCode == "MD")
            {
                SQL = SQL + ComNum.VBLF + "    AND MEDDEPTCD IN ('MG','MP','MN','ME','MC','MR','MD','MI') ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "    AND MEDDEPTCD IN ('" + clsType.User.DeptCode + "') ";
            }
            SQL = SQL + ComNum.VBLF + "  AND MEDDRCD = '" + clsType.User.IdNumber + "'";
            SQL = SQL + ComNum.VBLF + "  AND MIBIFNDATE IS NULL";
            SQL = SQL + ComNum.VBLF + "  AND MIBICLS = 1";
            SQL = SQL + ComNum.VBLF + "  AND MIBIGRP = '" + strMiBiGPR + "'";
            SQL = SQL + ComNum.VBLF + "  AND CONCAT(MIBIINDATE, MIBIINTIME) <= CONCAT(WRITEDATE, WRITETIME)";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return false;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return false;
            }
            dt.Dispose();
            dt = null;
            return true;

        }

        private string GetMiBiNm(string strMibiCd, string strMibiRmk)
        {
            string rGetMiBiNm = "";
            switch (strMibiCd)
            {
                case "A01":
                    rGetMiBiNm = "누락";
                    break;
                case "A02":
                    rGetMiBiNm = "주진단명";
                    break;
                case "A03":
                    rGetMiBiNm = "부진단명";
                    break;
                case "A04":
                    rGetMiBiNm = "퇴원시환자상태";
                    break;
                case "A05":
                    rGetMiBiNm = "주수술(처치)명";
                    break;
                case "A06":
                    rGetMiBiNm = "기타수술(처치)명";
                    break;
                case "A07":
                    rGetMiBiNm = "퇴원형태";
                    break;
                case "A08":
                    rGetMiBiNm = "C/C. P/I";
                    break;
                case "A09":
                    rGetMiBiNm = "검사결과";
                    break;
                case "A10":
                    rGetMiBiNm = "추후치료계획";
                    break;
                case "A11":
                    rGetMiBiNm = "(" + strMibiRmk + ")";
                    break;


                case "B01":
                    rGetMiBiNm = "누락";
                    break;
                case "B02":
                    rGetMiBiNm = "의료행위 전 작성여부";
                    break;
                case "B03":
                    rGetMiBiNm = "환자인적사항";
                    break;
                case "B04":
                    rGetMiBiNm = "목적필요성 및 방법";
                    break;
                case "B05":
                    rGetMiBiNm = "발생할 수 있는 문제";
                    break;
                case "B06":
                    rGetMiBiNm = "대안 및 미시행결과";
                    break;
                case "B07":
                    rGetMiBiNm = "설명의사 서명";
                    break;
                case "B08":
                    rGetMiBiNm = "동의권자 서명";
                    break;
                case "B09":
                    rGetMiBiNm = "동의권자서명사유";
                    break;
                case "B10":
                    rGetMiBiNm = "설명일시";
                    break;
                case "B11":
                    rGetMiBiNm = "(" + strMibiRmk + ")";
                    break;
                case "C01":
                    rGetMiBiNm = "누락";
                    break;
                case "C02":
                    rGetMiBiNm = "C/C";
                    break;
                case "C03":
                    rGetMiBiNm = "P/I";
                    break;
                case "C04":
                    rGetMiBiNm = "P/H";
                    break;
                case "C05":
                    rGetMiBiNm = "PE";
                    break;
                case "C06":
                    rGetMiBiNm = "ROS";
                    break;
                case "C07":
                    rGetMiBiNm = "Imp";
                    break;
                case "C08":
                    rGetMiBiNm = "Plan";
                    break;
                case "C09":
                    rGetMiBiNm = "(" + strMibiRmk + ")";
                    break;

                case "D01":
                    rGetMiBiNm = "누락";
                    break;
                case "D02":
                    rGetMiBiNm = "횟수부족(3일1회작성)";
                    break;
                case "D03":
                    rGetMiBiNm = "특수검사기록";
                    break;
                case "D04":
                    rGetMiBiNm = "처치및시술기록";
                    break;
                case "D05":
                    rGetMiBiNm = "전과기록";
                    break;
                case "D06":
                    rGetMiBiNm = "수술후환자상태";
                    break;
                case "D07":
                    rGetMiBiNm = "퇴원지시";
                    break;
                case "D08":
                    rGetMiBiNm = "(" + strMibiRmk + ")";
                    break;
                case "D09":
                    rGetMiBiNm = "SOAP항목";
                    break;
                case "D10":
                    rGetMiBiNm = "의학적재평가";
                    break;

                case "E01":
                    rGetMiBiNm = "누락";
                    break;
                case "E02":
                    rGetMiBiNm = "환자정보";
                    break;
                case "E03":
                    rGetMiBiNm = "수술전진단";
                    break;
                case "E04":
                    rGetMiBiNm = "수술후진단";
                    break;
                case "E05":
                    rGetMiBiNm = "수술명";
                    break;
                case "E06":
                    rGetMiBiNm = "마취방법";
                    break;
                case "E07":
                    rGetMiBiNm = "수술관찰소견";
                    break;
                case "E08":
                    rGetMiBiNm = "수술절차";
                    break;
                case "E10":
                    rGetMiBiNm = "조직검체표본";
                    break;
                case "E11":
                    rGetMiBiNm = "배액";
                    break;
                case "E12":
                    rGetMiBiNm = "출혈량";
                    break;
                case "E13":
                    rGetMiBiNm = "시술기록지";
                    break;
                case "E09":
                    rGetMiBiNm = "(" + strMibiRmk + ")";
                    break;
            }
            return rGetMiBiNm;
        }

        private void ssMiBi_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                return;
            }
            if (ssMiBi_Sheet1.RowCount == 0)
            {
                return;
            }

            ssMiBi_Sheet1.Cells[0, 0, ssMiBi_Sheet1.RowCount - 1, ssMiBi_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssMiBi_Sheet1.Cells[e.Row, 0, e.Row, ssMiBi_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
        }

        private void ssMiBi_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                return;
            }
            if (ssMiBi_Sheet1.RowCount == 0)
            {
                return;
            }

            if (mCallForm == null) return;

            if (mCallForm.Name != "frmTextEmrMain") return;

            mPTNO = ssMiBi_Sheet1.Cells[e.Row, 0].Text.Trim();
            mInDate = ssMiBi_Sheet1.Cells[e.Row, 2].Text.Trim();
            mOutDate = ssMiBi_Sheet1.Cells[e.Row, 3].Text.Trim();

            rSetMibiPat(mPTNO, mInDate, mOutDate);
        }
    }
}

