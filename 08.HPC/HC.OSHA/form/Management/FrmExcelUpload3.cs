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
    public partial class FrmExcelUpload3 : Form
    {
        private HcSiteWorkerService hcSiteWorkerService;
        private HcSiteWorkerRepository hcSiteWorkerRepository;
        private int[] FnCol = new int[1000];

        public FrmExcelUpload3()
        {
            InitializeComponent();
            hcSiteWorkerService = new HcSiteWorkerService();
            hcSiteWorkerRepository = new HcSiteWorkerRepository();
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

            if (cboBangi.Items.Count == 0)
            {
                cboBangi.Items.Add("전체");
                cboBangi.Items.Add("상반기");
                cboBangi.Items.Add("하반기");
            }
            cboBangi.SelectedIndex = 0;

            if (cboJong.Items.Count == 0)
            {
                cboJong.Items.Add("특수");
                cboJong.Items.Add("일반");
                cboJong.Items.Add("배치전");
                cboJong.Items.Add("배치후");
            }
            cboJong.SelectedIndex = 0;

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

            if (TxtLtdcode.Text.Trim() == "") { ComFunc.MsgBox("회사코드가 공란입니다."); return; }
            if (cboYear.Text.Trim() == "") { ComFunc.MsgBox("검진년도가 공란입니다."); return; }
            if (cboBangi.Text.Trim() == "") { ComFunc.MsgBox("반기 구분이 공란입니다."); return; }
            if (cboJong.Text.Trim() == "") { ComFunc.MsgBox("검진종류 구분이 공란입니다."); return; }

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
            SS1_Sheet1.RowCount = SSExcel_Sheet1.RowCount;

            for (i = 1; i < SSExcel_Sheet1.RowCount; i++)
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
                                    // 판정에 중복 표시된것 제외
                                    if (j == 7)
                                    {
                                        strNewData = SS1_Sheet1.Cells[nRow - 1, j].Text.ToString();
                                        if (VB.InStr(strNewData, strData) == 0)
                                        {
                                            if (strData == "A")
                                            {
                                                if (strNewData == "") strNewData = strData;
                                            }
                                            else if (strNewData=="A")
                                            {
                                                strNewData = strData;
                                            }
                                            else
                                            {
                                                strNewData = strNewData + "," + strData;
                                            }
                                            SS1_Sheet1.Cells[nRow - 1, j].Text = strNewData;
                                        }
                                    }
                                    else
                                    {
                                        if (strData == "[특]정상" || strData== "0.필요없음")
                                        {
                                            strNewData = SS1_Sheet1.Cells[nRow - 1, j].Text.ToString();
                                            if (VB.InStr(strNewData, strData) == 0)
                                            {
                                                strNewData = SS1_Sheet1.Cells[nRow - 1, j].Text.ToString();
                                                strNewData = strNewData + "," + ComNum.VBLF + strData;
                                                SS1_Sheet1.Cells[nRow - 1, j].Text = strNewData;
                                            }

                                        }
                                        else if (strData == "정상" || strData == "필요없음")
                                        {
                                            strNewData = SS1_Sheet1.Cells[nRow - 1, j].Text.ToString();
                                            if (VB.InStr(strNewData, strData) == 0)
                                            {
                                                strNewData = SS1_Sheet1.Cells[nRow - 1, j].Text.ToString();
                                                strNewData = strNewData + "," + ComNum.VBLF + strData;
                                                SS1_Sheet1.Cells[nRow - 1, j].Text = strNewData;
                                            }

                                        }

                                        else
                                        {
                                            strNewData = SS1_Sheet1.Cells[nRow - 1, j].Text.ToString();
                                            strNewData = strNewData + "," + ComNum.VBLF + strData;
                                            SS1_Sheet1.Cells[nRow - 1, j].Text = strNewData;
                                        }
                                    }
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
            string strID = "";
            long nLtdCode = 0;
            string strGongjeng = "";
            string strName = "";
            string strBirth = "";
            string strJong = "";
            string SQL = "";
            string SqlErr = "";
            string strYear = "";
            string strBangi = "";
            string strJinDate = "";
            string strAge = "";
            string strSex = "";
            string strGunsok = "";
            string strYuhe = "";
            string strJipyo = "";
            string strGGubun = "";
            string strSogen = "";
            string strSahu = "";
            string strUpmu = "";
            string strMsg = "";
            
            int intRowAffected = 0;
            int nSS4_Rows = 0;
            DataTable dt = null;

            nLtdCode = long.Parse(VB.Pstr(TxtLtdcode.Text, ".", 1));
            strYear = cboYear.Text.Trim();
            strBangi = VB.Pstr(cboBangi.Text, ".", 1);
            strJong = cboJong.Text.Trim();

            HC_SITE_WORKER worker = new HC_SITE_WORKER();

            strYuhe = SS1_Sheet1.Cells[1, 5].Text.ToString().Trim();
            if (strJong == "일반" && strYuhe != "")
            {
                ComFunc.MsgBox("일반인데 유해인자가 있음", "오류");
                return;
            }
            if (strJong == "특수" && strYuhe == "")
            {
                ComFunc.MsgBox("특수인데 유해인자가 없음", "오류");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            // 검진일자, 생년월일 자동 찾기
            strMsg = "";
            for (int i = 0; i < SS1_Sheet1.RowCount; i++)
            {
                strName = SS1_Sheet1.Cells[i, 1].Text.ToString().Trim();
                strSex = SS1_Sheet1.Cells[i, 2].Text.ToString().Trim();
                strAge = SS1_Sheet1.Cells[i, 3].Text.ToString().Trim();
                strJinDate = SS1_Sheet1.Cells[i, 11].Text.ToString().Trim();
                strBirth = SS1_Sheet1.Cells[i, 12].Text.ToString().Trim();

                if (strName != "" && strSex != "" && strAge != "")
                {
                    //생년월일이 공란인것만 찾음
                    if (strBirth == "")
                    {
                        SQL = "";
                        SQL = "SELECT ID,BIRTH,JINDATE FROM HIC_LTD_RESULT2 ";
                        SQL = SQL + ComNum.VBLF + "WHERE SITEID=" + nLtdCode + " ";
                        SQL = SQL + ComNum.VBLF + "  AND YEAR = '" + strYear + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND NAME='" + strName + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND SEX = '" + strSex + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND AGE = " + strAge + " ";
                        SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                        if (dt.Rows.Count > 0)
                        {
                            SS1_Sheet1.Cells[i, 11].Text = dt.Rows[0]["JINDATE"].ToString().Trim();
                            SS1_Sheet1.Cells[i, 12].Text = dt.Rows[0]["BIRTH"].ToString().Trim();
                            strJinDate = SS1_Sheet1.Cells[i, 11].Text.ToString();
                            strBirth = SS1_Sheet1.Cells[i, 12].Text.ToString();
                        }
                        dt.Dispose();
                        dt = null;

                        // 직원정보에서 동명이인이 없으면 생년월일을 자동 가져옴
                        if (strBirth == "")
                        {
                            SQL = "SELECT ID,JUMIN FROM HIC_SITE_WORKER ";
                            SQL = SQL + ComNum.VBLF + "WHERE SITEID=" + nLtdCode + " ";
                            SQL = SQL + ComNum.VBLF + "  AND NAME='" + strName + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND JUMIN <> '000000' ";
                            SQL = SQL + ComNum.VBLF + "  AND ISDELETED='N' ";
                            SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
                            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                            if (dt.Rows.Count > 1)
                            {
                                nSS4_Rows++;
                                strMsg = strMsg + (i + 1).ToString() + "번줄 " + strName + " " + strSex + " ";
                                strMsg = strMsg + strAge + " 동명이인이 있어 저장 불가" + ComNum.VBLF;
                            }
                            else if (dt.Rows.Count == 1)
                            {
                                SS1_Sheet1.Cells[i, 12].Text = dt.Rows[0]["JUMIN"].ToString().Trim();
                                strBirth = SS1_Sheet1.Cells[i, 12].Text.ToString();
                            }
                            else if (dt.Rows.Count == 0)
                            {
                                nSS4_Rows++;
                                strMsg = strMsg + (i + 1).ToString() + "번줄 " + strName + " " + strSex + " ";
                                strMsg = strMsg + strAge + " 직원명단에 없어 저장 불가" + ComNum.VBLF;
                            }
                            dt.Dispose();
                            dt = null;
                        }
                    }
                }
            }

            // 등록번호를 찾지 못하면 오류 처리
            if (strMsg != "")
            {
                string sMsg = "";
                sMsg = "정상적인 자료는 업로드를 하시겠습니까?" + "\r\n";
                sMsg += "-< 오류내역 >-" + "\r\n";
                sMsg += strMsg;

                Cursor.Current = Cursors.Default;
                if (MessageBox.Show(sMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }

            //오류 보관용 시트를 Clear
            SS4_Sheet1.RowCount = 0;
            SS4_Sheet1.RowCount = nSS4_Rows;
            nSS4_Rows = 0;

            for (int i = 0; i < SS1_Sheet1.RowCount; i++)
            {
                strGongjeng = SS1_Sheet1.Cells[i, 0].Text.ToString();
                strName = SS1_Sheet1.Cells[i, 1].Text.ToString().Trim();
                strSex = SS1_Sheet1.Cells[i, 2].Text.ToString().Trim();
                strAge = SS1_Sheet1.Cells[i, 3].Text.ToString().Trim();
                strGunsok = SS1_Sheet1.Cells[i, 4].Text.ToString();
                strYuhe = SS1_Sheet1.Cells[i, 5].Text.ToString();
                strJipyo = SS1_Sheet1.Cells[i, 6].Text.ToString();
                strGGubun = SS1_Sheet1.Cells[i, 7].Text.ToString();
                strSogen = SS1_Sheet1.Cells[i, 8].Text.ToString();
                strSahu = SS1_Sheet1.Cells[i, 9].Text.ToString();
                strUpmu = SS1_Sheet1.Cells[i, 10].Text.ToString();
                strJinDate = SS1_Sheet1.Cells[i, 11].Text.ToString();
                strBirth = SS1_Sheet1.Cells[i, 12].Text.ToString();
                if (VB.Len(strBirth) > 6) strBirth = VB.Left(strBirth, 6);
                if (strSex == "M") strSex = "남";
                if (strSex == "F") strSex = "여";
                if (strAge == "") strAge = Age_Gesan1(strBirth);

                // 전송오류는 별도 엑셀파일로 저장하기 위해 별도 보관함
                if (strBirth=="")
                {
                    for (int j=0; j < SS1_Sheet1.ColumnCount; j++)
                    {
                        SS4_Sheet1.Cells[nSS4_Rows, j].Text=SS1_Sheet1.Cells[i, j].Text;
                    }
                    nSS4_Rows++;
                }
                else 
                {
                    // 사원이 없으면 신규등록함
                    worker.ID = "";
                    worker.SITEID = nLtdCode;
                    worker.NAME = strName;
                    worker.DEPT = "";
                    worker.TEL = "";
                    worker.ISDELETED = "N";
                    worker.JUMIN = strBirth;
                    worker.PANO = 0;
                    worker.PTNO = "";
                    worker.WORKER_ROLE = "EMP_ROLE";
                    worker.IPSADATE = "";
                    HC_SITE_WORKER saved = hcSiteWorkerRepository.FindOneByBirth(nLtdCode, strName, strBirth);
                    if (saved == null)
                    {
                        saved = hcSiteWorkerRepository.Insert(worker);
                    }
                    else
                    {
                        worker.ID = saved.ID;
                    }

                    strID = worker.ID;

                    // 유질환자 사후관리
                    if (Exist_Ltd_Result3(nLtdCode, strYear,strJong, strBangi, strID) == false)
                    {
                        try
                        {
                            SQL = "";
                            SQL += " INSERT INTO HIC_LTD_RESULT3 (SITEID,JONG,YEAR,BANGI,ID,NAME,BIRTH,JINDATE, ";
                            SQL += " SEX,AGE,GUNSOK,YUHE,JIPYO,GGUBUN,SOGEN,SAHU,UPMU,JOBSABUN,ENTTIME,SWLICENSE) ";
                            SQL += " VALUES (" + nLtdCode + ",'" + strJong + "','" + strYear + "','" + strBangi + "','";
                            SQL += strID + "','" + strName + "','" + strBirth + "','" + strJinDate + "','";
                            SQL += strSex + "'," + strAge + ",'" + strGunsok + "','" + strYuhe + "','";
                            SQL += strJipyo + "','" + strGGubun + "','" + strSogen + "','" + strSahu + "','";
                            SQL += strUpmu + "','" + clsType.User.IdNumber + "',";
                            SQL += "SYSTIMESTAMP,'" + clsType.HosInfo.SwLicense + "') ";
                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("유질환자 사후관리 등록 실패", "알림");
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox(ex.Message);
                            return;
                        }
                    }
                }
            }
            Cursor.Current = Cursors.Default;
            if (strMsg == "")
            {
                ComFunc.MsgBox("서버로 전송 완료", "알림");
            }
            else
            {
                string strPath = @"C:\Temp\업로드오류_";
                strPath += VB.Pstr(TxtLtdcode.Text, ".", 2) + "_" + cboYear.Text.Trim() + "_";
                strPath += cboJong.Text.Trim() + ".xls";
                SS4.SaveExcel(strPath, FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders | FarPoint.Excel.ExcelSaveFlags.DataOnly);
                ComFunc.MsgBox("전송 오류 내역은 Temp 폴더에 저장됨", "알림");
            }
            Screen_Set();
        }

        // 년도,반기 및 사원번호로 금년도 등록여부 점검
        bool Exist_Ltd_Result3(long nLtdCode, string strYear,string strJong, string strBangi, string strID)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT ID  FROM HIC_LTD_RESULT3 ";
                SQL = SQL + ComNum.VBLF + "WHERE SITEID=" + nLtdCode + " ";
                SQL = SQL + ComNum.VBLF + "  AND ID='" + strID + "' ";
                SQL = SQL + ComNum.VBLF + "  AND YEAR = '" + strYear + "' ";
                SQL = SQL + ComNum.VBLF + "  AND Jong = '" + strJong + "' ";
                SQL = SQL + ComNum.VBLF + "  AND Bangi = '" + strBangi + "' ";
                SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;
                    return true;
                }
                else
                {
                    dt.Dispose();
                    dt = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    return false;
                }

                ComFunc.MsgBox(ex.Message);
                return false;
            }
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
                    if (bOK==false && strHead == "공정")
                    {
                        if (strData == "공정(부서)") bOK = true;
                    }
                    if (bOK == false && strHead == "근속연수")
                    {
                        if (strData == "근속년수") bOK = true;
                    }
                    if (bOK == false && strHead == "검진소견")
                    {
                        if (strData == "건강진단결과") bOK = true;
                    }

                    if (bOK == false && strHead == "사후관리소견")
                    {
                        if (strData == "사후관리내용") bOK = true;
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

        // 생년월일 6자리로 오늘 기준 나이를 계산
        // 근로자는 신생아는 없기 때문에 생년월일 6자리로 나이를 간이 계산함
        private string Age_Gesan1(string strBirth)
        {
            int nAge = 0;
            //생년월일이 오류이면 0살을 Return
            if (strBirth == "" || strBirth == "000000" || strBirth == "123456")
            {
                return "0";
            }

            int nYear1 = Int32.Parse(DateTime.Now.ToString("yyyy"));
            int nYear2 = Int32.Parse(VB.Left(strBirth, 2));
            nAge = nYear1 - (2000 + nYear2);
            if (nAge < 0) nAge = nYear1 - (1900 + nYear2);

            return nAge.ToString();
        }

    }

}


