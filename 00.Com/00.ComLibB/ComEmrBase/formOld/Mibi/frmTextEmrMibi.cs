using ComBase;
using FarPoint.Win.Spread;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmTextEmrMibi : Form
    {
        /// <summary>
        /// Class Name      : ComEmrBase
        /// File Name       : frmTextEmrMibi
        /// Description     : 개인별 미비 현황조회
        /// Author          : 이현종
        /// Create Date     : 2019-07-27
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// </history>
        /// <seealso cref= " PSMH\mtsEmr(frmTextEmrMibi.frm) >> frmTextEmrMibi.cs 폼이름 재정의" />
        /// 

        //미비 환자 정보 전달
        public delegate void EventMiBiUserSend(string strPtNo, string strInDate, string strOutDate);
        public event EventMiBiUserSend rEventMiBiUserSend;

        //창종료 이벤트
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        Font font = new Font("굴림체", 9f, FontStyle.Regular);

        public frmTextEmrMibi()
        {
            InitializeComponent();
        }

        private void frmTextEmrMibi_Load(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetMibiList();
        }

        #region 미비 조회 함수
        void GetMibiList()
        {
            string strPtno1 = string.Empty;
            string strMedEndDate = string.Empty;
            int intCol = 0;

            spMibi_Sheet1.RowCount = 0;

            string SQL     = string.Empty;
            string SqlErr  = string.Empty;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT A.PTNO, B.SNAME AS PTNAME, A.MIBIINDATE, A.MIBIINTIME,";
                SQL += ComNum.VBLF + "        A.MEDFRDATE, A.MEDENDDATE, A.MIBIGRP, A.MIBICD, A.MIBIRMK, A.ERGB ";
                SQL += ComNum.VBLF + "    FROM ADMIN.EMRMIBI A, ADMIN.BAS_PATIENT B ";
                if(clsType.User.DeptCode == "MD" || clsType.User.Sabun == "31606" || clsType.User.Sabun == "34241")
                {
                    SQL += ComNum.VBLF + "    WHERE A.MEDDEPTCD IN ('MG','MP','MN','ME','MC','MR','MD','MI','MO') ";
                }
                else if(clsType.User.IdNumber == "1367")
                {
                    SQL += ComNum.VBLF + "    WHERE A.MEDDEPTCD IN ('GS','HU') ";
                }
                else
                {
                    SQL += ComNum.VBLF + "    WHERE A.MEDDEPTCD IN ('" + clsType.User.DeptCode + "') ";
                }

                SQL += ComNum.VBLF + "    AND A.MEDDRCD = '" + clsType.User.IdNumber + "' ";
                SQL += ComNum.VBLF + "    AND A.MIBIFNDATE IS NULL";
                SQL += ComNum.VBLF + "    AND A.MIBICLS = 1";
                SQL += ComNum.VBLF + "    AND A.PTNO = B.PANO ";
                SQL += ComNum.VBLF + "    ORDER BY B.SNAME, A.MEDFRDATE, A.MIBIGRP, MIBIRMK DESC, MIBICD ";

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
                    Cursor.Current = Cursors.Default;
                    return;
                }

                int intRow = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string strMibiGrp = dt.Rows[i]["MIBIGRP"].ToString().Trim();

                    if (strMibiGrp == "A")
                    {
                        intCol = 12;
                    }
                    else if (strMibiGrp == "B")
                    {
                        intCol = 13;
                    }
                    else if (strMibiGrp == "C")
                    {
                        intCol = 14;
                    }
                    else if (strMibiGrp == "D")
                    {
                        intCol = 15;
                    }
                    else if (strMibiGrp == "E")
                    {
                        intCol = 16;
                    }


                    if (strPtno1 != dt.Rows[i]["PTNO"].ToString().Trim() ||
                        strMedEndDate != dt.Rows[i]["MEDENDDATE"].ToString().Trim())
                    {
                        spMibi_Sheet1.RowCount += 1;
                        intRow = spMibi_Sheet1.RowCount - 1;

                        #region 미비 현황을 체크 한다
                        if (MiBiStatusCheck(dt.Rows[i]["PTNO"].ToString().Trim(),
                            dt.Rows[i]["MIBIGRP"].ToString().Trim(),
                            dt.Rows[i]["MEDENDDATE"].ToString().Trim()))
                        {
                            spMibi_Sheet1.Cells[intRow, intCol].Text = "(완료)" +
                                spMibi_Sheet1.Cells[intRow, intCol].Text;
                            spMibi_Sheet1.Cells[intRow, intCol].Font = font;
                            spMibi_Sheet1.Cells[intRow, intCol].ForeColor = Color.Blue;
                            spMibi_Sheet1.Rows[intRow].Height = spMibi_Sheet1.Rows[intRow].GetPreferredHeight() + 10;
                        }
                        #endregion

                        strPtno1 = dt.Rows[i]["PTNO"].ToString().Trim();
                        strMedEndDate = dt.Rows[i]["MEDENDDATE"].ToString().Trim();

                        spMibi_Sheet1.Cells[intRow, 0].Text = strPtno1;
                        spMibi_Sheet1.Cells[intRow, 1].Text = dt.Rows[i]["PTNAME"].ToString().Trim();
                        spMibi_Sheet1.Cells[intRow, 2].Text = dt.Rows[i]["MEDFRDATE"].ToString().Trim();
                        spMibi_Sheet1.Cells[intRow, 3].Text = dt.Rows[i]["MEDENDDATE"].ToString().Trim();
                        if(dt.Rows[i]["ERGB"].ToString().Trim() == "1")
                        {
                            spMibi_Sheet1.Cells[intRow, 3].Text =  spMibi_Sheet1.Cells[intRow, 3].Text + ComNum.VBLF + "(ER)";
                            spMibi_Sheet1.Cells[intRow, 3].ForeColor = Color.Red;
                        }
                    }
                    else
                    {
                        intRow = intRow = spMibi_Sheet1.RowCount - 1;

                        #region 미비 현황을 체크 한다
                        if (MiBiStatusCheck(dt.Rows[i]["PTNO"].ToString().Trim(),
                            dt.Rows[i]["MIBIGRP"].ToString().Trim(),
                            dt.Rows[i]["MEDENDDATE"].ToString().Trim()))
                        {
                            if (spMibi_Sheet1.Cells[intRow, intCol].Text.IndexOf("(완료)") == -1)
                            {
                                spMibi_Sheet1.Cells[intRow, intCol].Text = "(완료)" +
                                spMibi_Sheet1.Cells[intRow, intCol].Text;
                                spMibi_Sheet1.Cells[intRow, intCol].Font = font;
                                spMibi_Sheet1.Cells[intRow, intCol].ForeColor = Color.Blue;
                                spMibi_Sheet1.Rows[intRow].Height = spMibi_Sheet1.Rows[intRow].GetPreferredHeight() + 10;
                            }
                        }
                        #endregion
                    }

                    switch (strMibiGrp)
                    {
                        case "A":
                        case "B":
                        case "C":
                        case "D":
                        case "E":

                            spMibi_Sheet1.Cells[intRow, intCol].Text =
                            spMibi_Sheet1.Cells[intRow, intCol].Text == "" ?
                            GetMiBiNm(dt.Rows[i]["MIBICD"].ToString().Trim(), dt.Rows[i]["MIBIRMK"].ToString().Trim())
                            :
                            spMibi_Sheet1.Cells[intRow, intCol].Text + ComNum.VBLF +
                            GetMiBiNm(dt.Rows[i]["MIBICD"].ToString().Trim(), dt.Rows[i]["MIBIRMK"].ToString().Trim());


                            spMibi_Sheet1.Rows[intRow].Height =
                            spMibi_Sheet1.Rows[intRow].GetPreferredHeight() + 10;
                            break;
                    }
                }


                #region 미비 현황을 체크 한다
                if (MiBiStatusCheck(dt.Rows[dt.Rows.Count - 1]["PTNO"].ToString().Trim(),
                    dt.Rows[dt.Rows.Count - 1]["MIBIGRP"].ToString().Trim(),
                    dt.Rows[dt.Rows.Count - 1]["MEDENDDATE"].ToString().Trim()))
                {
                    if(spMibi_Sheet1.Cells[intRow, intCol].Text.IndexOf("(완료)") == -1)
                    {
                        spMibi_Sheet1.Cells[intRow, intCol].Text = "(완료)" + ComNum.VBLF +
                        spMibi_Sheet1.Cells[intRow, intCol].Text;
                        spMibi_Sheet1.Cells[intRow, intCol].Font = font;
                        spMibi_Sheet1.Cells[intRow, intCol].ForeColor = Color.Blue;
                        spMibi_Sheet1.Rows[intRow].Height = spMibi_Sheet1.Rows[intRow].GetPreferredHeight() + 10;
                    }
                }
                #endregion

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


        bool MiBiStatusCheck(string strPtNo, string strMiBiGPR, string strMedEndDate)
        {
            bool rtnVal = false;
            string SQL = string.Empty;
            string sqlErr = string.Empty;
            OracleDataReader reader = null;

            try
            {
                SQL = "SELECT MIBIGRP";
                SQL += ComNum.VBLF + "FROM ADMIN.EMRMIBI  ";
                SQL += ComNum.VBLF + "WHERE PTNO = '" + strPtNo + "'";
                SQL += ComNum.VBLF + "  AND MEDENDDATE = '" + strMedEndDate + "'";

                if(clsType.User.DeptCode == "MD")
                {
                    SQL += ComNum.VBLF + "    AND MEDDEPTCD IN ('MG','MP','MN','ME','MC','MR','MD','MI') ";
                }
                else
                {
                    SQL += ComNum.VBLF + "    AND MEDDEPTCD IN ('" + clsType.User.DeptCode + "') ";
                }

                SQL += ComNum.VBLF + "  AND MEDDRCD = '" + clsType.User.IdNumber + "'";
                SQL += ComNum.VBLF + "  AND MIBIFNDATE IS NULL";
                SQL += ComNum.VBLF + "  AND MIBICLS = 1";
                SQL += ComNum.VBLF + "  AND MIBIGRP = '" + strMiBiGPR + "'";
                SQL += ComNum.VBLF + "  AND CONCAT(MIBIINDATE, MIBIINTIME) <= CONCAT(WRITEDATE, WRITETIME)";

                sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = true;
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return rtnVal;

        }

        /// <summary>
        /// 미비 이름 반환
        /// </summary>
        /// <param name="strMibiCd"></param>
        /// <param name="strMibiRmk"></param>
        /// <returns></returns>
        private string GetMiBiNm(string strMibiCd, string strMibiRmk)
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
                //A12~13 신규로 추가된 항목임.
                case "A12":
                    strMibiCd = "*경과요약";
                    break;
                case "A13":
                    strMibiCd = "*삭제";
                    break;
                //20-06-25 - POA 항목 추가로 추가됨
                case "A14":
                    strMibiCd = "*POA";
                    break;
                //'Case "A11": GetMiBiNm = "＊기타(" & strMibiRmk & ")"

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

        #endregion
        private void btnExit_Click(object sender, EventArgs e)
        {
            if(rEventClosed != null)
            {
                rEventClosed();
            }
        }
        private void FrmTextEmrMibi_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (rEventClosed != null)
            {
                font.Dispose();
                rEventClosed();
            }
        }

        private void ssMiBiList_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (spMibi_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                return;
            }

            if(rEventMiBiUserSend !=null)
            {
                string strPtno = spMibi_Sheet1.Cells[e.Row, 0].Text.Trim();
                string strInDate  = spMibi_Sheet1.Cells[e.Row, 2].Text.Trim();
                string strOutDate = spMibi_Sheet1.Cells[e.Row, 3].Text.Trim().Replace("(ER)", "").Replace(ComNum.VBLF, "");

                rEventMiBiUserSend(strPtno, strInDate, strOutDate);
            }
        }

        private void ssMiBiList_CellClick(object sender, CellClickEventArgs e)
        {
            if (spMibi_Sheet1.RowCount == 0) return;

            if (e.Row == -1) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(spMibi, e.Column);
                return;
            }
        }
    }
}
