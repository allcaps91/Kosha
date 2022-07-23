using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Utils;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using HC.Core.Common.Interface;
using HC.Core.Dto;
using HC.Core.Model;
using HC.Core.Service;
using HC.Core.Repository;
using HC.OSHA.Model;
using HC.OSHA.Service;
using HC_Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_OSHA
{
    public partial class FrmExcelUpload8 : Form
    {
        private int[] FnCol = new int[1000];
        private int FnHeadRow=-1;

        public FrmExcelUpload8()
        {
            InitializeComponent();
            Screen_Set();
        }

        private void Screen_Set()
        {
            int i = 0;
            int nYear = 0;
            string strTitle = "";
            nYear = Int32.Parse(DateTime.Now.ToString("yyyy"));

            //회사관계자 로그인
            if (clsType.User.LtdUser != "")
            {
            }

            for (i = 0; i < 1000; i++)
            {
                FnCol[i] = 0;
            }

            if (cboYear.Items.Count == 0)
            {
                for (i = 0; i < 5; i++)
                {
                    cboYear.Items.Add(nYear.ToString());
                    nYear--;
                }
            }
            cboYear.SelectedIndex = 0;

            SS1_Sheet1.RowCount = 0;
            SSConv_Sheet1.RowCount = 0;
            SSConv_Sheet1.RowCount = SS1_Sheet1.ColumnCount;

            for (i = 0; i < SS1_Sheet1.ColumnCount; i++)
            {
                strTitle = SS1_Sheet1.ColumnHeader.Cells[0, i].Value.ToString();
                SSConv_Sheet1.Cells[i, 0].Text = strTitle;
            }

            btnJob1.Enabled = true;
            btnJob2.Enabled = false;
            btnJob3.Enabled = false;
            btnJob4.Enabled = false;
            btnJob5.Enabled = false;
        }

        private void btnJob1_Click(object sender, EventArgs e)
        {
            string strFileName = "";
            int nBlankLine = 0;
            int nRowCount = 0;
            bool bBlank = false;

            if (cboYear.Text.Trim() == "") { ComFunc.MsgBox("작업년도가 공란입니다."); return; }

            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                strFileName = dialog.FileName;
                SSExcel.ActiveSheet.RowCount = 0;
                SSExcel.ActiveSheet.OpenExcel(strFileName, 0);
                btnJob4.Enabled = true;

                // 공란만 있는 Row는 제거
                for (int i = 0; i < SSExcel_Sheet1.RowCount; i++)
                {
                    bBlank = true;
                    for (int j = 0; j < SSExcel_Sheet1.ColumnCount; j++)
                    {
                        if (SSExcel_Sheet1.Cells[i, j].Text != "")
                        {
                            bBlank = false;
                            nRowCount = i;
                            break;
                        }
                    }
                    if (bBlank == true)
                    {
                        nBlankLine++;
                        if (nBlankLine > 5)
                        {
                            SSExcel_Sheet1.RowCount = nRowCount + 1;
                            break;
                        }
                    }
                }
            }
            btnJob5.Enabled = true;

        }

        private void btnJob5_Click(object sender, EventArgs e)
        {
            string strHead = "";
            string strData = "";
            int nCnt = 0;
            bool bOK = false;

            //1~5번 중 제목줄을 찾기
            FnHeadRow = -1;
            for (int i = 0; i < 5; i++)
            {
                nCnt = 0;
                strHead = SSExcel_Sheet1.Cells[i, 0].Text.ToString() + "{}";
                strHead += SSExcel_Sheet1.Cells[i, 1].Text.ToString() + "{}";
                strHead += SSExcel_Sheet1.Cells[i, 2].Text.ToString() + "{}";
                strHead += SSExcel_Sheet1.Cells[i, 3].Text.ToString() + "{}";
                if (VB.InStr(strHead, "사업장명") > 0) nCnt++;
                if (VB.InStr(strHead, "사업자번호") > 0) nCnt++;
                if (nCnt==2)
                {
                    FnHeadRow = i;
                    break;
                }
            }
            if (FnHeadRow==-1) { ComFunc.MsgBox("엑셀파일에 제목줄이 없습니다.", "오류"); return; }

            // 변환정보 Clear
            for (int i = 0; i < SSConv_Sheet1.RowCount; i++)
            {
                SSConv_Sheet1.Cells[i, 1].Value = "";
                SSConv_Sheet1.Cells[i, 2].Value = "";
            }

            //엑셀파일에서 표준파일 헤드정보를 찾음
            for (int i = 0; i < SSConv_Sheet1.RowCount; i++)
            {
                strHead = VB.Replace(SSConv_Sheet1.Cells[i, 0].Text.Trim(), " ", "");
                for (int j = 0; j < SSExcel_Sheet1.ColumnCount; j++)
                {
                    bOK = false;
                    strData = VB.Replace(SSExcel_Sheet1.Cells[FnHeadRow, j].Text.ToString(), " ", "");
                    strData = VB.Replace(strData, "\n", "");
                    if (strHead == strData) bOK = true;
                    if (bOK == false && strHead == "사업장명")
                    {
                        if (VB.InStr(strData, "업체명") > 0) bOK = true;
                        if (VB.InStr(strData, "거래처명") > 0) bOK = true;
                    }
                    if (bOK == false && strHead == "사업자번호")
                    {
                        if (VB.InStr(strData, "등록번호") > 0) bOK = true;
                    }
                    if (bOK == false && strHead == "보건담당자")
                    {
                        if (VB.InStr(strData, "담당자") > 0) bOK = true;
                    }
                    if (bOK == false && strHead == "담당자직위")
                    {
                        if (VB.InStr(strData, "직위") > 0) bOK = true;
                        if (VB.InStr(strData, "직책") > 0) bOK = true;
                    }
                    if (bOK == false && strHead == "담당자휴대폰번호")
                    {
                        if (VB.InStr(strData, "휴대폰") > 0) bOK = true;
                        if (VB.InStr(strData, "폰번호") > 0) bOK = true;
                    }
                    if (bOK == false && strHead == "개시일자")
                    {
                        if (VB.InStr(strData, "계약일") > 0) bOK = true;
                    }
                    if (bOK == false && strHead == "대행단가")
                    {
                        if (VB.InStr(strData, "단가") > 0) bOK = true;
                    }

                    if (bOK == true)
                    {
                        SSConv_Sheet1.Cells[i, 1].Value = (j + 1);
                        break;
                    }
                }
            }
            btnJob4.Enabled = true;
        }

        private void btnJob4_Click(object sender, EventArgs e)
        {
            string strData = "";
            int nCol = 0;

            for (int i = 0; i < SSConv_Sheet1.RowCount; i++)
            {
                SSConv_Sheet1.Cells[i, 2].Value = "";
            }

            for (int i = 0; i < SSConv_Sheet1.RowCount; i++)
            {
                if (SSConv_Sheet1.Cells[i, 1].Text.Trim() != "")
                {
                    nCol = Int32.Parse(SSConv_Sheet1.Cells[i, 1].Value.ToString());
                    if (nCol > 0)
                    {
                        strData = SSExcel_Sheet1.Cells[FnHeadRow+1, nCol - 1].Text.ToString();
                        SSConv_Sheet1.Cells[i, 2].Text = strData;
                    }

                }
            }
            btnJob2.Enabled = true;
        }

        private void btnJob2_Click(object sender, EventArgs e)
        {
            int i;
            int j;
            string strData = "";
            int nRow = 0;
            bool isBlankLine = false;
            bool bMultyLine = false;
            string strNewData = "";

            //변경값을 변수에 저장
            for (i = 0; i < SSConv_Sheet1.RowCount; i++)
            {
                FnCol[i] = 0;
                if (SSConv_Sheet1.Cells[i, 1].Text.Trim() != "")
                {
                    FnCol[i] = Int32.Parse(SSConv_Sheet1.Cells[i, 1].Value.ToString());
                }
            }

            SS1_Sheet1.RowCount = 0;
            SS1_Sheet1.RowCount = SSExcel_Sheet1.RowCount;
            nRow = 0;
            for (i = (FnHeadRow+1); i < SSExcel_Sheet1.RowCount; i++)
            {
                isBlankLine = true;
                for (j = 0; j < SSExcel_Sheet1.ColumnCount; j++)
                {
                    if (SSExcel_Sheet1.Cells[i, j].Text.ToString() != "")
                    {
                        isBlankLine = false;
                        break;
                    }
                }

                // 공란줄은 제외
                if (isBlankLine == false)
                {
                    // 엑셀파일의 처음 2칼럼 모두 공란이면 연속 Data로 처리
                    bMultyLine = true;
                    if (SSExcel_Sheet1.Cells[i, 0].Text.Trim() != "") bMultyLine = false;
                    if (SSExcel_Sheet1.Cells[i, 1].Text.Trim() != "") bMultyLine = false;

                    for (j = 0; j < SSConv_Sheet1.RowCount; j++)
                    {
                        if (FnCol[j] > 0)
                        {
                            strData = SSExcel_Sheet1.Cells[i, FnCol[j] - 1].Text.ToString();
                            if (strData != "")
                            {
                                if (bMultyLine == true)
                                {
                                    // 유해인자
                                    strNewData = SS1_Sheet1.Cells[nRow - 1, j].Text.ToString();
                                    strNewData = strNewData + "," + ComNum.VBLF + strData;
                                    SS1_Sheet1.Cells[nRow - 1, j].Text = strNewData;
                                }
                                else
                                {
                                    SS1_Sheet1.Cells[nRow, j].Text = strData;
                                }
                            }
                        }
                    }
                }
                if (bMultyLine == false) nRow++;
            }
            SS1_Sheet1.RowCount = nRow;

            // 셀의 높이를 조정
            for (i = 0; i < SS1_Sheet1.RowCount; i++)
            {
                SS1_Sheet1.Rows[i].Height = SS1_Sheet1.Rows[i].GetPreferredHeight();
            }

            btnJob3.Enabled = true;
        }

        private void btnJob3_Click(object sender, EventArgs e)
        {
            long nLtdCode = 0;
            long nEstimate = 0;

            string SQL = "";
            string SqlErr = "";
            string strYear = "";

            string strData = "";
            string strSangho = "";
            string strSAUPNO = "";
            string strDAEPYO = "";
            string strUPTAE = "";
            string strJONGMOK = "";
            string strTEL = "";
            string strFAX = "";
            string strBONAME = "";
            string strBOJIK = "";
            string strHtel = "";
            string strGYEDATE = "";
            string strMAILCODE = "";
            string strJUSO = "";
            string strJUSODETAIL = "";
            long nPrice = 0;
            int nInwon1 = 0;
            int nInwon2 = 0;
            int nInwon3 = 0;
            int nInwon4 = 0;
            int nTotInwon = 0;

            int intRowAffected = 0;
            DataTable dt = null;
            bool bOK = true;

            strYear = cboYear.Text.Trim();

            //필수정보 누락 여부 점검
            for (int i = 0; i < SS1_Sheet1.RowCount; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    strData = SS1_Sheet1.Cells[i, j].Text.ToString().Trim();
                    if (strData == "")
                    {
                        ComFunc.MsgBox((i + 1) + "번줄 필수정보가 누락되어 작업 불가", "알림");
                        return;
                    }
                }
            }

            // 자료를 1건씩 업로드함
            for (int i = 0; i < SS1_Sheet1.RowCount; i++)
            {
                strSangho = SS1_Sheet1.Cells[i, 0].Text.ToString().Trim();
                strSAUPNO = SS1_Sheet1.Cells[i, 1].Text.ToString().Trim();
                strDAEPYO = SS1_Sheet1.Cells[i, 2].Text.ToString().Trim();
                strUPTAE = SS1_Sheet1.Cells[i, 3].Text.ToString().Trim();
                strJONGMOK = SS1_Sheet1.Cells[i, 4].Text.ToString().Trim();
                strTEL = SS1_Sheet1.Cells[i, 5].Text.ToString().Trim();
                strFAX = SS1_Sheet1.Cells[i, 6].Text.ToString().Trim();
                strBONAME = SS1_Sheet1.Cells[i, 7].Text.ToString().Trim();
                strBOJIK = SS1_Sheet1.Cells[i, 8].Text.ToString().Trim();
                strHtel = SS1_Sheet1.Cells[i, 9].Text.ToString().Trim();
                strGYEDATE = SS1_Sheet1.Cells[i, 10].Text.ToString().Trim();
                nPrice = 0;
                strData = SS1_Sheet1.Cells[i, 11].Text.ToString().Trim();
                if (strData != "") nPrice = long.Parse(strData);
                nInwon1 = 0;
                strData = SS1_Sheet1.Cells[i, 12].Text.ToString().Trim();
                if (strData != "") nInwon1 = int.Parse(strData);
                nInwon2 = 0;
                strData = SS1_Sheet1.Cells[i, 13].Text.ToString().Trim();
                if (strData != "") nInwon2 = int.Parse(strData);
                nInwon3 = 0;
                strData = SS1_Sheet1.Cells[i, 14].Text.ToString().Trim();
                if (strData != "") nInwon3 = int.Parse(strData);
                nInwon4 = 0;
                strData = SS1_Sheet1.Cells[i, 15].Text.ToString().Trim();
                if (strData != "") nInwon4 = int.Parse(strData);
                nTotInwon = nInwon1 + nInwon2 + nInwon3 + nInwon4;

                strMAILCODE = SS1_Sheet1.Cells[i, 16].Text.ToString().Trim();
                strJUSO = SS1_Sheet1.Cells[i, 17].Text.ToString().Trim();
                strJUSODETAIL = SS1_Sheet1.Cells[i, 18].Text.ToString().Trim();

                //상호로 거래처가 이미 등록되었지 확인
                SQL = "SELECT CODE FROM HIC_LTD ";
                SQL = SQL + "WHERE SANGHO='" + strSangho + "' ";
                SQL = SQL + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                bOK = true;
                if (dt.Rows.Count > 0) bOK = false;
                dt.Dispose();
                dt = null;

                //거래처코드가 등록않된것만 DB등록함
                if (bOK == true)
                {
                    nLtdCode = GET_NewLtdNo();

                    //거래처코드에 신규등록
                    SQL = "INSERT INTO HIC_LTD (CODE,SANGHO,NAME,SAUPNO,UPTAE,JONGMOK,DAEPYO,TEL,FAX,";
                    SQL = SQL + "BONAME,BOJIK,HTEL,GYEDATE,MAILCODE,JUSO,JUSODETAIL,GBDAEHANG,SWLICENSE) ";
                    SQL = SQL + "VALUES (" + nLtdCode + ",'" + strSangho + "','" + strSangho + "','";
                    SQL = SQL + strSAUPNO + "','" + strUPTAE + "','" + strJONGMOK + "','";
                    SQL = SQL + strDAEPYO + "','" + strTEL + "','" + strFAX + "','" + strBONAME + "','";
                    SQL = SQL + strBOJIK + "','" + strHtel + "','" + strGYEDATE + "','";
                    SQL = SQL + strMAILCODE + "','" + strJUSO + "','" + strJUSODETAIL + "','Y','";
                    SQL = SQL + clsType.HosInfo.SwLicense + "') ";
                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("거래처코드에 신규등록시 오류가 발생함", "알림");
                        bOK = false;
                    }
                }

                //HIC_OSHA_SITE 등록
                if (bOK == true)
                {
                    SQL = "INSERT INTO HIC_OSHA_SITE (ID,ISACTIVE,TASKNAME,PARENTSITE_ID,";
                    SQL = SQL + "HASCHILD,ISPARENTCHARGE,ISQUARTERCHARGE,MODIFIED,MODIFIEDUSER,";
                    SQL = SQL + "CREATED,CREATEDUSER,SWLICENSE) ";
                    SQL = SQL + "VALUES (" + nLtdCode + ",'Y','사업장 등록',0,'N','Y','N',SYSDATE,'";
                    SQL = SQL + clsType.User.Sabun + "',SYSDATE,'" + clsType.User.Sabun + "','";
                    SQL = SQL + clsType.HosInfo.SwLicense + "') ";
                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("HIC_OSHA_SITE 신규등록시 오류가 발생함", "알림");
                        bOK = false;
                    }
                }

                string strStartDate = strYear + "-01-01";
                string strEndDate = strYear + "-12-31";
                if (VB.Left(strGYEDATE,4)== strYear) strStartDate = strGYEDATE;
                nEstimate = 0;

                //HIC_OSHA_ESTIMATE 자동 생성
                if (bOK == true)
                {
                    nEstimate = GET_SeqNextVal("HC_OSHA_ESTIMATE_ID_SEQ");

                    SQL = "INSERT INTO HIC_OSHA_ESTIMATE (ID,OSHA_SITE_ID,ESTIMATEDATE,STARTDATE,WORKERTOTALCOUNT,";
                    SQL += " OFFICIALFEE,SITEFEE,MONTHLYFEE,WHITEMALE,WHITEFEMALE,BLUEMALE,BLUEFEMALE,";
                    SQL += " ISDELETED,CREATED,CREATEDUSER,MODIFIED,MODIFIEDUSER,SWLICENSE) ";
                    SQL += "VALUES (" + nEstimate + "," + nLtdCode + ",'";
                    SQL += strStartDate + "','" + strStartDate + "'," + nTotInwon + ",0," + nPrice + ",0,";
                    SQL += nInwon1 + "," + nInwon2 + "," + nInwon3 + "," + nInwon4 + ",'N',SYSDATE,'";
                    SQL += clsType.User.Sabun + "',SYSDATE,'" + clsType.User.Sabun + "','";
                    SQL += clsType.HosInfo.SwLicense + "') ";
                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("견적정보에 신규등록시 오류가 발생함", "알림");
                        bOK = false;
                    }
                }

                //HIC_OSHA_CONTRACT 자동 생성
                if (bOK == true)
                {
                    SQL = "INSERT INTO HIC_OSHA_CONTRACT (ESTIMATE_ID,OSHA_SITE_ID,CONTRACTDATE,WORKERTOTALCOUNT,";
                    SQL += " WORKERWHITEMALECOUNT,WORKERWHITEFEMALECOUNT,WORKERBLUEMALECOUNT,";
                    SQL += " WORKERBLUEFEMALECOUNT,MANAGEWORKERCOUNT,";
                    SQL += " VISITWEEK,VISITDAY,COMMISSION,DECLAREDAY,CONTRACTSTARTDATE,CONTRACTENDDATE,";
                    SQL += " POSITION,ISDELETED,MODIFIED,MODIFIEDUSER,CREATED,CREATEDUSER,SWLICENSE) ";
                    SQL += "VALUES (" + nEstimate + "," + nLtdCode + ",'" + strStartDate + "',";
                    SQL += nTotInwon + "," + nInwon1 + "," + nInwon2 + "," + nInwon3 + "," + nInwon4 + ",";
                    SQL += nTotInwon + ",'둘째','화'," + nPrice + ",'" + strGYEDATE + "','";
                    SQL += strStartDate + "','" + strEndDate + "','0','N',SYSDATE,'";
                    SQL += clsType.User.Sabun + "',SYSDATE,'" + clsType.User.Sabun + "','";
                    SQL += clsType.HosInfo.SwLicense + "') ";
                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("계약정보에 신규등록시 오류가 발생함", "알림");
                        bOK = false;
                    }
                }
            }

            ComFunc.MsgBox("서버로 전송 완료", "알림");
            Screen_Set();
        }

        //신규 거래처코드 찾기
        private long GET_NewLtdNo()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt1 = null;
            long nLtdno = 0;

            for (int i = 0; i < 2000; i++)
            {
                nLtdno = GET_SeqNextVal("HC_LTD_SEQ");

                //거래처가 등록되었는지 확인(라이선스 상관없이 점검)
                SQL = "SELECT CODE FROM HIC_LTD ";
                SQL = SQL + "WHERE CODE='" + nLtdno.ToString() + "' ";
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                if (dt1.Rows.Count == 0) break;
                dt1.Dispose();
                dt1 = null;
            }
            dt1.Dispose();
            dt1 = null;

            return nLtdno;
        }

        private long GET_SeqNextVal(string strSeqName)
        {
            long nID = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            //시컨스 찾기
            SQL = "SELECT " + strSeqName + ".NEXTVAL AS SeqNo FROM DUAL";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            nID = 0;
            if (dt.Rows.Count > 0) nID = long.Parse(dt.Rows[0]["SeqNo"].ToString().Trim());
            dt.Dispose();
            dt = null;

            return nID;
        }

    }
}
