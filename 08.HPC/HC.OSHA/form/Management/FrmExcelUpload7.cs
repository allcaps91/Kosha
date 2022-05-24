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
using System.Threading;
using HC.Core.BaseCode.MSDS.Dto;

namespace HC_OSHA
{
    public partial class FrmExcelUpload7 : Form
    {
        private KoshaMsdsService koshaMsdsService;

        private int[] FnCol = new int[1000];
        private int startRow = 0;
        private int nMaxDataCol = 0;
        private int nMaxDataRow = 0;

        public FrmExcelUpload7()
        {
            InitializeComponent();
            koshaMsdsService = new KoshaMsdsService();
            Screen_Set();
        }

        private void Screen_Set()
        {
            int i = 0;
            string strTitle = "";

            TxtLtdcode.Text = "";

            for (i = 0; i < 1000; i++)
            {
                FnCol[i] = 0;
            }

            SS1_Sheet1.RowCount = 0;
            SSConv_Sheet1.RowCount = 0;
            SSConv_Sheet1.RowCount = SS1_Sheet1.ColumnCount;

            for (i = 0; i < SS1_Sheet1.ColumnCount - 2; i++)
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

            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                strFileName = dialog.FileName;
                SSExcel.ActiveSheet.RowCount = 0;
                //SSExcel.ActiveSheet.OpenExcel(strFileName, 0);
                SSExcel.OpenExcel(strFileName,0);
            }
        }

        private void BtnSearchSite_Click(object sender, EventArgs e)
        {

        }

        private void btnJob1_Click_1(object sender, EventArgs e)
        {
            string strFileName = "";

            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                strFileName = dialog.FileName;
                SSExcel.ActiveSheet.RowCount = 0;
                SSExcel.OpenExcel(strFileName, 0);
                btnJob4.Enabled = true;
            }
            btnJob5.Enabled = true;
        }

        private void BtnSearchSite_Click_1(object sender, EventArgs e)
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

        private void btnJob5_Click(object sender, EventArgs e)
        {
            string strHead = "";
            string strData = "";
            int nCol = 0;
            bool bOK = false;

            //해당 시트의 시작 제목줄을 찾기
            startRow = -1;
            for (int i = 0; i < 20; i++)
            {
                strHead = "";
                for (int j = 0; j < 10; j++)
                {
                    strHead += SSExcel.ActiveSheet.Cells[i, j].Text.Trim() + ",";
                }
                if (VB.InStr(strHead, "공정") > 0 && VB.InStr(strHead, "제품명") > 0 && VB.InStr(strHead, "취급자") > 0)
                {
                    startRow = i;
                    break;
                }
            }
            if (startRow == -1) { ComFunc.MsgBox("엑셀파일 제목줄 찾기 오류", "오류"); return; }

            //제목줄의 마지막 칼럼 찾기
            nMaxDataCol = -1;
            for (int i = 0; i < 30; i++)
            {
                if (i < SSExcel.ActiveSheet.ColumnCount)
                {
                    if (SSExcel.ActiveSheet.Cells[startRow, i].Text.Trim() != "")
                    {
                        nMaxDataCol = i;
                    }
                }
            }

            //해당 시트의 마지막 자료 줄번호 찾기
            nMaxDataRow = startRow + 2;
            for (int i = startRow + 2; i < 1000; i++)
            {
                strHead = "";
                for (int j = 0; j < 10; j++)
                {
                    strHead += SSExcel.ActiveSheet.Cells[i, j].Text.Trim();
                }
                if (strHead=="")
                {
                    nMaxDataRow = i;
                    break;
                }
            }

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
                for (int j = 0; j <= nMaxDataCol; j++)
                {
                    bOK = false;
                    strData = VB.Replace(SSExcel.ActiveSheet.Cells[startRow, j].Text.ToString(), " ", "");
                    strData = VB.Replace(strData, "\n", "");
                    if (strHead == strData) bOK = true;
                    if (bOK == false && strHead == "공정")
                    {
                        if (VB.InStr(strData, "공정") > 0) bOK = true;
                        if (VB.InStr(strData, "사업소") > 0) bOK = true;
                    }
                    if (bOK == false && strHead == "제품명")
                    {
                        if (VB.InStr(strData, "물질명") > 0) bOK = true;
                    }
                    if (bOK == false && strHead == "사용용도")
                    {
                        if (VB.InStr(strData, "용도") > 0) bOK = true;
                    }
                    if (bOK == false && strHead == "취급량(월)")
                    {
                        if (VB.InStr(strData, "취급량") > 0) bOK = true;
                        if (VB.InStr(strData, "사용량") > 0) bOK = true;
                    }
                    if (bOK == false && strHead == "구성성분")
                    {
                        if (VB.InStr(strData, "구성성분") > 0) bOK = true;
                        if (VB.InStr(strData, "함유물질") > 0) bOK = true;
                    }
                    if (bOK == false && strHead == "CASNO")
                    {
                        if (VB.InStr(strData, "CAS") > 0) bOK = true;
                    }
                    if (bOK == false && strHead == "함유량(%)")
                    {
                        if (VB.InStr(strData, "함유량") > 0) bOK = true;
                    }
                    if (bOK == false && strHead == "관리대상물질")
                    {
                        if (VB.InStr(strData, "관리대상") > 0) bOK = true;
                    }
                    if (bOK == false && strHead == "특검대상물질")
                    {
                        if (VB.InStr(strData, "특검대상") > 0) bOK = true;
                    }
                    if (bOK == false && strHead == "작업환경측정")
                    {
                        if (VB.InStr(strData, "측정") > 0) bOK = true;
                        if (VB.InStr(strData, "환경") > 0) bOK = true;
                    }

                    if (bOK == true)
                    {
                        SSConv_Sheet1.Cells[i, 1].Value = (j + 1);
                        break;
                    }
                }
            }
            btnJob4.Enabled = true;
            SS1_Sheet1.RowCount = 0;
            SS1_Sheet1.RowCount = 20;
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
                        strData = SSExcel.ActiveSheet.Cells[startRow + 2, nCol - 1].Text.ToString();
                        SSConv_Sheet1.Cells[i, 2].Text = strData;
                    }
                }
            }
            btnJob2.Enabled = true;
            SS1_Sheet1.RowCount = 0;
            SS1_Sheet1.RowCount = 20;
        }

        private void btnJob2_Click(object sender, EventArgs e)
        {
            int i;
            int j;
            string strData = "";
            string strPan = "";
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
            SS1_Sheet1.RowCount = SSExcel.ActiveSheet.RowCount;
            nRow = -1;
            for (i = startRow + 1; i <= nMaxDataRow; i++)
            {
                isBlankLine = true;
                for (j = 0; j <= nMaxDataCol; j++)
                {
                    if (SSExcel.ActiveSheet.Cells[i, j].Text.ToString() != "")
                    {
                        isBlankLine = false;
                        break;
                    }
                }

                // 공란줄은 제외
                if (isBlankLine == false)
                {
                    nRow++;
                    for (j = 0; j < SSConv_Sheet1.RowCount; j++)
                    {
                        if (FnCol[j] > 0)
                        {
                            strData = SSExcel.ActiveSheet.Cells[i, FnCol[j] - 1].Text.ToString();
                            if (strData != "")
                            {
                                SS1_Sheet1.Cells[nRow, j].Text = strData;
                            }
                        }
                    }
                }
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
            string strCurrDate = DateTime.Now.ToString("yyyy-MM-dd");

            string SQL = "";
            string SqlErr = "";

            string strGong = "";
            string strJepum = "";
            string strUsage = "";
            string strMontyQty = "";

            bool bOk = false;
            string strMsdsid = "";
            string strID = "";
            string strChemid = "";
            string strName = "";
            string strCASNO = "";
            string strQty = "";
            string strTemp = "";
            string strMsdsError = "";

            int intRowAffected = 0;
            DataTable dt = null;

            nLtdCode = long.Parse(VB.Pstr(TxtLtdcode.Text, ".", 1));

            //MSDS 누락분 신규등록
            strMsdsError = "";
            for (int i = 0; i < SS1_Sheet1.RowCount; i++)
            {
                strMsdsid = SS1_Sheet1.Cells[i, 12].Text.Trim();
                if (strMsdsid == "")
                {
                    strName = SS1_Sheet1.Cells[i, 5].Text.Trim();
                    strCASNO = SS1_Sheet1.Cells[i, 6].Text.Trim();
                    strID = "";

                    bOk = true;
                    if (VB.Len(strCASNO)<=4) bOk = false;
                    if (strName == "필요없음") bOk = false;
                    if (strName == "영업비밀") bOk = false;
                    if (strName == "조회안됨") bOk = false;
                    if (strCASNO == "필요없음") bOk = false;

                    //msds 정보가 있는지 점검
                    if (bOk == true)
                    {
                        SQL = "SELECT ID,CHEMID,NAME FROM HIC_MSDS ";
                        SQL = SQL + ComNum.VBLF + "WHERE CASNO='" + strCASNO + "' ";
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                        strID = "";
                        strChemid = "";
                        if (dt.Rows.Count > 0)
                        {
                            strID = dt.Rows[0]["ID"].ToString().Trim();
                            strChemid = dt.Rows[0]["CHEMID"].ToString().Trim();
                        }
                        dt.Dispose();
                        dt = null;
                        
                        //msds 정보가 없으면 신규 등록함
                        if (strID == "")
                        {
                            strTemp = HIC_MSDS_Insert(strCASNO);
                            if (strTemp == "")
                            {
                                strMsdsError += (i + 1) + "번줄 CASNO:" + strCASNO + " " + strName;
                                strMsdsError += " KOSHA MSDS 정보 찾기 실패" + "\n";
                            } else {
                                strID = VB.Pstr(strTemp, "{}", 1);
                                strChemid = VB.Pstr(strTemp, "{}", 2);
                            }

                        }

                        //msds 정보가 등록되었으면 CHEMID,MSDSID를 저장 
                        if (strID != "") {
                            for (int j=i;j< SS1_Sheet1.RowCount; j++)
                            {
                                strTemp = SS1_Sheet1.Cells[j, 6].Text.Trim();
                                if (strTemp == strCASNO)
                                {
                                    SS1_Sheet1.Cells[j, 11].Text = strChemid;
                                    SS1_Sheet1.Cells[j, 12].Text = strID;
                                }
                            }
                        }

                     }

                }

            }

            //미등록 자료 등록
            strMsdsError = "";
            for (int i = 0; i < SS1_Sheet1.RowCount; i++)
            {
                strGong = SS1_Sheet1.Cells[i, 0].Text.Trim();
                strJepum = SS1_Sheet1.Cells[i, 1].Text.Trim();
                strUsage = SS1_Sheet1.Cells[i, 3].Text.Trim();
                strMontyQty = SS1_Sheet1.Cells[i, 4].Text.Trim();

                bOk = false;
                if (strJepum != "")
                {
                    SQL = "SELECT ID FROM HIC_SITE_PRODUCT ";
                    SQL = SQL + ComNum.VBLF + "WHERE SITE_ID=" + nLtdCode + " ";
                    SQL = SQL + ComNum.VBLF + "  AND PRODUCTNAME='" + strJepum + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND PROCESS='" + strGong + "' ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    strID = "";
                    if (dt.Rows.Count > 0) strID = dt.Rows[0]["ID"].ToString().Trim();
                    dt.Dispose();
                    dt = null;

                    if (strID == "") bOk = true; 
                }

                //신규등록
                if (bOk == true)
                {
                    nID = GET_SeqNextVal("HC_SITE_PRODUCT_ID_SEQ");

                    SQL = "INSERT INTO HIC_SITE_PRODUCT (ID, SITE_ID,PRODUCTNAME,PROCESS,USAGE,";
                    SQL = SQL + " MONTHLYAMOUNT,REVISIONDATE,MODIFIED,MODIFIEDUSER,";
                    SQL = SQL + " CREATED,CREATEDUSER,SWLICENSE) VALUES (";
                    SQL = SQL + nID + "," + nLtdCode + ",'" + strJepum + "','";
                    SQL = SQL + strGong + "','" + strUsage + "','" + strMontyQty + "','" + strCurrDate + "',";
                    SQL = SQL + "SYSDATE,'" + clsType.User.Sabun + "',SYSDATE,'" + clsType.User.Sabun;
                    SQL = SQL + "','" + clsType.HosInfo.SwLicense + "') ";
                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        strMsdsError += strJepum + " PRODUCT DB에 저장 오류" + "\n";
                        ComFunc.MsgBox("HIC_SITE_PRODUCT 자료 등록시 오류가 발생함", "알림");
                        Cursor.Current = Cursors.Default;
                    }

                    for (int j=i; j < SS1_Sheet1.RowCount; j++)
                    {
                        strCASNO = SS1_Sheet1.Cells[j, 6].Text.Trim();
                        strQty = SS1_Sheet1.Cells[j, 7].Text.Trim();

                        SQL = "INSERT INTO HIC_SITE_PRODUCT_MSDS (SITE_PRODUCT_ID,CASNO,QTY,SWLICENSE) VALUES (";
                        SQL = SQL + nID + ",'" + strCASNO + "','" + strQty + "',";
                        SQL = SQL + "'" + clsType.HosInfo.SwLicense + "') ";
                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            strMsdsError += strName + " PRODUCT_MSDS DB에 저장 오류" + "\n";
                            ComFunc.MsgBox("HIC_SITE_PRODUCT_MSDS 자료 등록시 오류가 발생함", "알림");
                            Cursor.Current = Cursors.Default;
                        }

                        //다음줄에 제품명이 있으면 HIC_SITE_PRODUCT_MSDS 등록 중지
                        if ((j+1) < SS1_Sheet1.RowCount)
                        {
                            strJepum = SS1_Sheet1.Cells[j + 1, 1].Text.Trim();
                            if (strJepum != "") break;
                        }
                    }
                }
            }

            if (strMsdsError=="")
            {
                ComFunc.MsgBox("서버로 전송 완료", "알림");
            }
            else
            {
                ComFunc.MsgBox(strMsdsError, "전송오류");
            }
        }

        // MSDS DB 신규 등록
        private string HIC_MSDS_Insert(string strCasno)
        {
            long nID = 0;
            string strChemid = "";
            string strName = "";
            string strReturn = "";

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            List<KoshaMsds> list = koshaMsdsService.SearchKoshaMsds(false, strCasno);
            Thread.Sleep(500);
            if (list == null || list.Count==0) { Cursor.Current = Cursors.Default; return ""; }

            strChemid = list[0].ChemId;
            strName = list[0].ChemNameKor;

            HC_MSDS dto = koshaMsdsService.GetKoshaRule(strChemid);
            Thread.Sleep(500);
            if (dto == null) { Cursor.Current = Cursors.Default; return ""; }

            dto.GHS_PICTURE = koshaMsdsService.GetGHSPicture(strChemid);

            nID = GET_SeqNextVal("HC_MSDS_ID_SEQ");

            SQL = "INSERT INTO HIC_MSDS (ID,CHEMID,NAME,CASNO,EXPOSURE_MATERIAL,WEM_MATERIAL,SPECIALHEALTH_MATERIAL,";
            SQL = SQL + " MANAGETARGET_MATERIAL,SPECIALMANAGE_MATERIAL,STANDARD_MATERIAL,";
            SQL = SQL + " PERMISSION_MATERIAL,PSM_MATERIAL,GHS_PICTURE,MODIFIED,CREATED) VALUES (";
            SQL = SQL + nID + ",'" + strChemid + "','" + strName + "','";
            SQL = SQL + strCasno + "','" + dto.EXPOSURE_MATERIAL + "','" + dto.WEM_MATERIAL + "','";
            SQL = SQL + dto.SPECIALHEALTH_MATERIAL + "','" + dto.MANAGETARGET_MATERIAL + "','" + dto.SPECIALMANAGE_MATERIAL + "','";
            SQL = SQL + dto.STANDARD_MATERIAL + "','" + dto.PERMISSION_MATERIAL + "','" + dto.PSM_MATERIAL + "','";
            SQL = SQL + dto.GHS_PICTURE + "',SYSDATE,SYSDATE) ";
            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "") if (list == null) { Cursor.Current = Cursors.Default; return ""; }

            Cursor.Current = Cursors.Default;
            strReturn = nID + "{}" + strChemid + "{}";

            return strReturn;
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

        private void FrmExcelUpload7_Load(object sender, EventArgs e)
        {

        }
    }
}
