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
    public partial class FrmExcelUpload5 : Form
    {
        private int[] FnCol = new int[1000];
        public FrmExcelUpload5()
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

            TxtLtdcode.Text = "";

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

        private void BtnSearchSite_Click(object sender, EventArgs e)
        {
            SiteListForm form = new SiteListForm();

            HC_SITE_VIEW siteView = form.Search(TxtLtdcode.Text);
            if (siteView == null)
            {
                DialogResult result = form.ShowDialog();
                siteView = form.SelectedSite;
            }
            else
            {
                form.Close();
            }

            if (siteView != null)
            {
                TxtLtdcode.Text = siteView.ID.ToString() + "." + siteView.NAME;
            }

        }

        private void btnJob1_Click(object sender, EventArgs e)
        {
            string strFileName = "";
            int nBlankLine = 0;
            int nRowCount = 0;
            bool bBlank = false;

            if (TxtLtdcode.Text.Trim() == "") { ComFunc.MsgBox("회사코드가 공란입니다."); return; }
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
            int nCol = 0;
            bool bOK = false;

            // 엑셀파일 1번줄이 제목인지 확인
            strHead = SSExcel_Sheet1.Cells[0, 0].Text.ToString();
            strHead += SSExcel_Sheet1.Cells[0, 1].Text.ToString();
            if (strHead == "") { ComFunc.MsgBox("엑셀파일 1번줄 제목줄이 아님", "오류"); return; }

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
                    strData = VB.Replace(SSExcel_Sheet1.Cells[0, j].Text.ToString(), " ", "");
                    strData = VB.Replace(strData, "\n", "");
                    if (strHead == strData) bOK = true;
                    if (bOK == false && strHead == "취급공정")
                    {
                        if (strData == "공정명") bOK = true;
                    }
                    if (bOK == false && strHead == "취급량(Kg/월)")
                    {
                        if (VB.InStr(strData,"취급량")>0) bOK = true;
                    }
                    if (bOK == false && strHead == "밀폐국소배기상태")
                    {
                        if (VB.InStr(strData, "밀폐국소") > 0) bOK = true;
                        if (VB.InStr(strData, "국소배기") > 0) bOK = true;
                    }
                    if (bOK == false && strHead == "보호구지급착용상태")
                    {
                        if (VB.InStr(strData, "보호구") > 0) bOK = true;
                        if (VB.InStr(strData, "착용상태") > 0) bOK = true;
                    }
                    if (bOK == false && strHead == "MSDS게시또는비치여부")
                    {
                        if (VB.InStr(strData, "게시") > 0) bOK = true;
                        if (VB.InStr(strData, "비치여부") > 0) bOK = true;
                    }
                    if (bOK == false && strHead == "경고표시부착여부")
                    {
                        if (VB.InStr(strData, "경고표시") > 0) bOK = true;
                        if (VB.InStr(strData, "표시부착") > 0) bOK = true;
                        if (VB.InStr(strData, "부착여부") > 0) bOK = true;
                    }
                    if (bOK == false && strHead == "MSDS교육실시여부")
                    {
                        if (VB.InStr(strData, "교육실시") > 0) bOK = true;
                        if (VB.InStr(strData, "실시여부") > 0) bOK = true;
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
                        strData = SSExcel_Sheet1.Cells[1, nCol - 1].Text.ToString();
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

            for (i = 1; i < SSExcel_Sheet1.RowCount; i++)
            {
                isBlankLine = true;
                for (j = 0; j < SSConv_Sheet1.RowCount; j++)
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
                    // 엑셀파일의 처음 3칼럼 모두 공란이면 연속 Data로 처리
                    bMultyLine = true;
                    if (SSExcel_Sheet1.Cells[i, 0].Text.Trim() != "") bMultyLine = false;
                    if (SSExcel_Sheet1.Cells[i, 1].Text.Trim() != "") bMultyLine = false;
                    if (SSExcel_Sheet1.Cells[i, 2].Text.Trim() != "") bMultyLine = false;

                    for (j = 0; j < SSConv_Sheet1.RowCount; j++)
                    {
                        if (FnCol[j] > 0)
                        {
                            strData = SSExcel_Sheet1.Cells[i, FnCol[j] - 1].Text.ToString();
                            if (strData != "")
                            {
                                if (bMultyLine == true)
                                {
                                    strNewData = SS1_Sheet1.Cells[nRow - 1, j].Text.ToString();
                                    strNewData = strNewData + ComNum.VBLF + strData;
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
            long nID = 0;
            long nLtdCode = 0;
            long nEstimate = 0;

            string SQL = "";
            string SqlErr = "";
            string strYear = "";

            string strTASKNAME = "";
            string strTASKTYPE = "";
            string strNAME = "";
            string strUSAGE = "";
            string strQTY = "";
            string strEXPOSURE = "";
            string strCOSENESS = "";
            string strPROTECTION = "";
            string strISMSDSPUBLISH = "";
            string strISALET = "";
            string strISMSDSEDUCATION = "";

            int intRowAffected = 0;
            int nSS4_Rows = 0;
            DataTable dt = null;

            nLtdCode = long.Parse(VB.Pstr(TxtLtdcode.Text, ".", 1));
            strYear = cboYear.Text.Trim();

            //계약번호를 찾기
            SQL = "SELECT ESTIMATE_ID FROM HIC_OSHA_CONTRACT ";
            SQL = SQL + ComNum.VBLF + "WHERE OSHA_SITE_ID=" + nLtdCode + " ";
            SQL = SQL + ComNum.VBLF + "  AND CONTRACTSTARTDATE<='" + strYear + "-12-31' ";
            SQL = SQL + ComNum.VBLF + "  AND CONTRACTENDDATE>='" + strYear + "-01-01' ";
            SQL = SQL + ComNum.VBLF + "  AND ISDELETED='N' ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            nEstimate = 0;
            if (dt.Rows.Count > 0)
            {
                nEstimate = long.Parse(dt.Rows[0]["ESTIMATE_ID"].ToString().Trim());
            }
            dt.Dispose();
            dt = null;

            if (nEstimate == 0)
            {
                ComFunc.MsgBox("계약정보를 찾지 못하여 작업 불가", "알림");
                Cursor.Current = Cursors.Default;
                return;
            }

            //기존의 자료를 삭제함
            SQL = "DELETE FROM HIC_OSHA_CARD16 ";
            SQL = SQL + "WHERE SITE_ID=" + nLtdCode + " ";
            SQL = SQL + "  AND YEAR='" + strYear + "' ";
            SQL = SQL + "  AND SWLICENSE='" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("기존 자료 삭제시 오류가 발생함", "알림");
                Cursor.Current = Cursors.Default;
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            // DB에 저장
            for (int i = 0; i < SS1_Sheet1.RowCount; i++)
            {
                strTASKNAME = SS1_Sheet1.Cells[i, 0].Text.ToString().Trim();
                strTASKTYPE = SS1_Sheet1.Cells[i, 1].Text.ToString().Trim();
                strNAME = SS1_Sheet1.Cells[i, 2].Text.ToString().Trim();
                strQTY = SS1_Sheet1.Cells[i, 3].Text.ToString().Trim();
                strUSAGE = SS1_Sheet1.Cells[i, 4].Text.ToString().Trim();
                strEXPOSURE = SS1_Sheet1.Cells[i, 5].Text.ToString().Trim();
                strCOSENESS = SS1_Sheet1.Cells[i, 6].Text.ToString().Trim();
                strPROTECTION = SS1_Sheet1.Cells[i, 7].Text.ToString().Trim();
                strISMSDSPUBLISH = SS1_Sheet1.Cells[i, 8].Text.ToString().Trim();
                strISALET = SS1_Sheet1.Cells[i, 9].Text.ToString().Trim();
                strISMSDSEDUCATION = SS1_Sheet1.Cells[i, 10].Text.ToString().Trim();

                if (strNAME != "")
                {
                    nID = GET_SeqNextVal("HC_OSHA_CARD_ID_SEQ");

                    SQL = "INSERT INTO HIC_OSHA_CARD16 (ID, SITE_ID,ESTIMATE_ID,YEAR,TASKNAME,";
                    SQL = SQL + " TASKTYPE,NAME,USAGE,QTY,EXPOSURE,COSENESS,PROTECTION,ISMSDSPUBLISH,";
                    SQL = SQL + " ISALET,ISMSDSEDUCATION,MODIFIED,MODIFIEDUSER, ";
                    SQL = SQL + " CREATED,CREATEDUSER,SWLICENSE) VALUES (";
                    SQL = SQL + nID + "," + nLtdCode + "," + nEstimate + ",'";
                    SQL = SQL + strYear + "','" + strTASKNAME + "','" + strTASKTYPE + "','";
                    SQL = SQL + strNAME + "','" + strUSAGE + "','" + strQTY + "','";
                    SQL = SQL + strEXPOSURE + "','" + strCOSENESS + "','" + strPROTECTION + "','";
                    SQL = SQL + strISMSDSPUBLISH + "','" + strISALET + "','" + strISMSDSEDUCATION + "',";
                    SQL = SQL + "SYSDATE,'" + clsType.User.Sabun + "',SYSDATE,'" + clsType.User.Sabun;
                    SQL = SQL + "','" + clsType.HosInfo.SwLicense + "') ";
                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("자료 신규등록시 오류가 발생함", "알림");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
            }
            ComFunc.MsgBox("서버로 전송 완료", "알림");
            Screen_Set();
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
