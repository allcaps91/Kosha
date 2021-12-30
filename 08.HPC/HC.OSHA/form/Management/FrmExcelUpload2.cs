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
    public partial class FrmExcelUpload2 : Form
    {
        private HcSiteWorkerService hcSiteWorkerService;
        private HcSiteWorkerRepository hcSiteWorkerRepository;
        private int[] FnCol = new int[1000];

        public FrmExcelUpload2()
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

            for (i = 0; i < 5; i++)
            {
                cboYear.Items.Add(nYear.ToString());
                nYear--;
            }
            cboYear.SelectedIndex = 0;

            cboBangi.Items.Add("전체");
            cboBangi.Items.Add("상반기");
            cboBangi.Items.Add("하반기");
            cboBangi.SelectedIndex = 0;

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
            int nCol=0;

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

            TxtLtdcode.Text = "";
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
            int nRow = 0;
            bool isBlankLine = false;
            bool bMultyLine = false;
            string strNewData = "";

            //변경값을 변수에 저장
            for (i=0; i < SSConv_Sheet1.RowCount; i++)
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
                    if (SSExcel_Sheet1.Cells[i, 0].Text.Trim() !="") bMultyLine = false;
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
                                    strNewData = SS1_Sheet1.Cells[nRow-1, j].Text.ToString();
                                    strNewData = strNewData + ComNum.VBLF + strData;
                                    SS1_Sheet1.Cells[nRow-1, j].Text = strNewData;
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

        // 서버 DB에 저장
        private void btnJob3_Click(object sender, EventArgs e)
        {
            string strID = "";
            long nLtdCode = 0;
            string strBuse = "";
            string strName = "";
            string strBirth = "";
            string SQL = "";
            string SqlErr = "";
            string strYear = "";
            string strJinDate = "";
            string strHosName = "";
            string strAge = "";
            string strSex = "";
            string strResult = "";
            int intRowAffected = 0;

            nLtdCode = long.Parse(VB.Pstr(TxtLtdcode.Text, ".", 1));
            strYear = cboYear.Text.Trim();
            HC_SITE_WORKER worker = new HC_SITE_WORKER();

            for (int i=0; i < SS1_Sheet1.RowCount; i++)
            {
                strBuse = SS1_Sheet1.Cells[i, 0].Text.ToString();
                strName = SS1_Sheet1.Cells[i, 1].Text.ToString();
                strBirth = SS1_Sheet1.Cells[i, 2].Text.ToString();
                if (VB.Len(strBirth) > 6) strBirth = VB.Left(strBirth, 6);

                // 구분자를 {$}로 결과를 한개로 묶음
                strResult = "";
                for (int j = 7; j < SS1_Sheet1.ColumnCount; j++) 
                {
                    strResult += SS1_Sheet1.Cells[i, j].Text.ToString().Trim() + "{$}";
                }

                // 사원이 없으면 신규등록함
                worker.ID = "";
                worker.SITEID = nLtdCode;
                worker.NAME = strName;
                worker.DEPT = strBuse;
                worker.TEL = "";
                worker.ISDELETED = "N";
                worker.JUMIN = strBirth;
                worker.PANO = 0;
                worker.PTNO = "";
                worker.WORKER_ROLE = "EMP_ROLE";
                worker.IPSADATE = "";
                HC_SITE_WORKER saved = hcSiteWorkerRepository.FindOneByBirth(nLtdCode,strName,strBirth);
                if (saved == null)
                {
                    saved = hcSiteWorkerRepository.Insert(worker);
                }
                else
                {
                    worker.ID = saved.ID;
                }

                strID = worker.ID;
                strJinDate = SS1_Sheet1.Cells[i, 3].Text.ToString().Trim();
                strHosName = SS1_Sheet1.Cells[i, 4].Text.ToString().Trim();
                strAge = SS1_Sheet1.Cells[i, 5].Text.ToString().Trim();
                strSex = SS1_Sheet1.Cells[i, 6].Text.ToString().Trim();
                if (strSex == "M") strSex = "남";
                if (strSex == "F") strSex = "여";

                // 뇌심혈관 결과 DB에 저장
                if (Exist_Ltd_Result2(nLtdCode,strYear,strID)==false)
                {
                    try
                    {
                        SQL = "";
                        SQL += " INSERT INTO HIC_LTD_RESULT2 (SITEID,ID,BUSE,NAME,BIRTH,YEAR,JINDATE, ";
                        SQL += " HOSNAME,AGE,SEX,RESULT,JOBSABUN,ENTTIME,SWLICENSE) ";
                        SQL += " VALUES (" + nLtdCode + ",'" + strID + "','" + strBuse + "','" + strName + "','";
                        SQL +=  strBirth + "','" + strYear + "','";
                        SQL +=  strJinDate + "','" + strHosName + "', "; //검진일,검진병원
                        SQL +=  strAge + ",'" + strSex + "','";  //나이,성별
                        SQL +=  strResult + "','" + clsType.User.IdNumber + "',";
                        SQL +=  "SYSTIMESTAMP,'" + clsType.HosInfo.SwLicense + "') ";
                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("뇌심혈관 결과 등록 실패", "알림");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        ComFunc.MsgBox(ex.Message);
                        return;
                    }
                }
            }
            ComFunc.MsgBox("서버로 전송 완료", "알림");
            Screen_Set();
        }

        // 년도 및 사원번호로 금년도 등록여부 점검
        bool Exist_Ltd_Result2(long nLtdCode, string strYear, string strID)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT ID  FROM HIC_LTD_RESULT2 ";
                SQL = SQL + ComNum.VBLF + "WHERE SITEID=" + nLtdCode + " ";
                SQL = SQL + ComNum.VBLF + "  AND ID='" + strID + "' ";
                SQL = SQL + ComNum.VBLF + "  AND YEAR = '" + strYear + "' ";
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

        private void FrmExcelUpload2_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lblLTD02_Click(object sender, EventArgs e)
        {

        }

        private void TxtLtdcode_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void cboYear_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void cboBangi_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SSExcel_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void SS1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void SSConv_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
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
            if (strHead == "") { ComFunc.MsgBox("엑셀파일 1번줄이 제목줄이 아님", "오류"); return; }

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
                for (int j=0;j<SSExcel_Sheet1.ColumnCount;j++)
                {
                    bOK = false;
                    strData = VB.Replace(SSExcel_Sheet1.Cells[0, j].Text.ToString()," ","");
                    if (strHead == strData) bOK = true;
                    if (bOK==false && strHead == "소속")
                    {
                        if (strData == "부서명") bOK = true;
                        if (strData == "부서") bOK = true;
                    }
                    if (bOK == false && strHead == "검진일")
                    {
                        if (strData == "검진일자") bOK = true;
                        if (strData == "검사일") bOK = true;
                    }
                    if (bOK == false && strHead == "생년월일")
                    {
                        if (strData == "주민번호") bOK = true;
                    }
                    if (bOK == false && strHead == "허리둘레")
                    {
                        if (strData == "복부둘레") bOK = true;
                    }
                    if (bOK == false && strHead == "수축기혈압")
                    {
                        if (strData == "혈압(최고)1차") bOK = true;
                        if (strData == "혈압") bOK = true;
                    }
                    if (bOK == false && strHead == "이완기혈압")
                    {
                        if (strData == "혈압(최저)1차") bOK = true;
                    }
                    if (bOK == false && strHead == "혈당")
                    {
                        if (strData == "공복혈당") bOK = true;
                    }
                    if (bOK == false && strHead == "총콜레스테롤")
                    {
                        if (strData == "T-Chol") bOK = true;
                    }
                    if (bOK == false && strHead == "HDL")
                    {
                        if (strData == "HDL콜레스테롤") bOK = true;
                        if (strData == "HDL-C") bOK = true;
                    }
                    if (bOK == false && strHead == "LDL")
                    {
                        if (strData == "LDL-콜레스테롤") bOK = true;
                        if (strData == "LDL-C") bOK = true;
                    }
                    if (bOK == false && strHead == "중성지방")
                    {
                        if (strData == "트리글리세라이드") bOK = true;
                        if (strData == "TG") bOK = true;
                    }
                    if (bOK == false && strHead == "BMI")
                    {
                        if (strData == "체질량지수") bOK = true;
                    }
                    if (bOK == false && strHead == "단백뇨")
                    {
                        if (strData == "요단백(단백뇨)(10-1)") bOK = true;
                    }
                    if (bOK == false && strHead == "사구체여과율(GFR)")
                    {
                        if (strData == "GFR") bOK = true;
                    }
                    if (bOK == false && strHead == "흉부X선")
                    {
                        if (strData == "흉부-X선") bOK = true;
                        if (strData == "ChestPA") bOK = true;
                        if (strData == "흉부") bOK = true;
                    }
                    if (bOK == false && strHead == "10년이내심뇌혈관발병확률(%)")
                    {
                        if (strData == "향후10년이내에심뇌혈관질환이발생활확률") bOK = true;
                        if (strData == "사유") bOK = true;
                    }
                    if (bOK == false && strHead == "최종평가")
                    {
                        if (strData == "소견") bOK = true;
                    }
                    if (bOK==true)
                    {
                        SSConv_Sheet1.Cells[i, 1].Value = (j+1);
                        break;
                    }
                }
            }
            btnJob4.Enabled = true;
        }
    }

}
