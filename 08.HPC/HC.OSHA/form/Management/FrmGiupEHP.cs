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
using HC.OSHA.Repository;
using HC.OSHA.Dto;

namespace HC_OSHA
{
    public partial class FrmGiupEHP : Form
    {
        private long Selected_Ltd_id = 0;
        private HicMailRepository hicMailRepository;
        private HcUsersRepository hcUsersRepository;
        private Role USER_ROLE = Role.ENGINEER;
        private MailType MAIL_TYPE = MailType.STATUS_WORKER_DOCTOR;

        public FrmGiupEHP()
        {
            InitializeComponent();

            hicMailRepository = new HicMailRepository();
            hcUsersRepository = new HcUsersRepository();

            //  TODO : 메일 발송정보
            HC_USER user = hcUsersRepository.FindOne(clsType.User.Sabun);
            Role USER_ROLE = Role.ENGINEER;

            Screen_Set();
        }

        private void Screen_Set()
        {
            int i = 0;
            string strFileName = @"c:\\HealthSoft\\엑셀서식\\기업건강증진지수.xls";

            TxtLtdcode.Text = "";
            SSExcel.OpenExcel(strFileName);

            SS1_Clear();
            ExcelSheet_Clear();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnJob1_Click(object sender, EventArgs e)
        {
            string strFileName = "";
            int nBlankLine = 0;
            int nRowCount = 0;
            bool bBlank = false;

            if (TxtLtdcode.Text.Trim() == "") { ComFunc.MsgBox("회사코드가 공란입니다."); return; }

            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                strFileName = dialog.FileName;
                SSExcel.OpenExcel(strFileName);
            }
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
                Selected_Ltd_id = siteView.ID;
                SET_SSList();
            }
        }

        private void SET_SSList()
        {
            string strLtd = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strLtd = VB.Pstr(TxtLtdcode.Text, ".", 1).Trim();
            if (strLtd == "") { ComFunc.MsgBox("회사코드가 공란입니다."); return; }

            //계약번호를 찾기
            SQL = "SELECT WDATE,ID FROM HIC_OSHA_EHP ";
            SQL = SQL + ComNum.VBLF + "WHERE SITE_ID=" + strLtd + " ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY WDATE DESC ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            SSList_Sheet1.RowCount = 0;
            if (dt.Rows.Count > 0)
            {
                SSList_Sheet1.RowCount = dt.Rows.Count;
                for (int i=0; i<dt.Rows.Count; i++)
                {
                    SSList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["WDATE"].ToString().Trim();
                    SSList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ID"].ToString().Trim();
                }
            }
            dt.Dispose();
            dt = null;
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            string strPath = dialog.SelectedPath;

            string strLtdName = VB.Pstr(TxtLtdcode.Text.Trim(), ".", 2).Trim();
            if (strLtdName == "")
            {
                ComFunc.MsgBox("회사를 선택 않함", "오류");
                return;
            }

            bool bOk = SSExcel.SaveExcel(strPath + "\\기업건강증진지수_" + strLtdName + ".xls", FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
            {
                if (bOk == true)
                    ComFunc.MsgBox(strPath + " 폴더에 엑셀파일이 생성이 되었습니다.", "확인");
                else
                    ComFunc.MsgBox("엑셀파일 생성에 오류가 발생 하였습니다.", "확인");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            SET_SSList();
            SS1_Clear();
            ExcelSheet_Clear();
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

        private void SSList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            long nID = 0;
            string strData = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            nID = long.Parse(SSList_Sheet1.Cells[e.Row, 1].Text.Trim());
            if (nID == 0) return;

            //목록을 읽음
            SQL = "SELECT * FROM HIC_OSHA_EHP ";
            SQL = SQL + ComNum.VBLF + "WHERE ID=" + nID + " ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                SS1_Sheet1.Cells[0,0].Text = dt.Rows[0]["WDATE"].ToString().Trim();
                SS1_Sheet1.Cells[0, 1].Text = dt.Rows[0]["CNT01"].ToString().Trim();
                SS1_Sheet1.Cells[0, 2].Text = dt.Rows[0]["CNT02"].ToString().Trim();
                SS1_Sheet1.Cells[0, 3].Text = dt.Rows[0]["CNT03"].ToString().Trim();
                SS1_Sheet1.Cells[0, 4].Text = dt.Rows[0]["CNT04"].ToString().Trim();
                SS1_Sheet1.Cells[0, 5].Text = dt.Rows[0]["CNT05"].ToString().Trim();
                SS1_Sheet1.Cells[0, 6].Text = dt.Rows[0]["CNT06"].ToString().Trim();
                SS1_Sheet1.Cells[0, 7].Text = dt.Rows[0]["CNT07"].ToString().Trim();
                SS1_Sheet1.Cells[0, 8].Text = dt.Rows[0]["CNT08"].ToString().Trim();
                SS1_Sheet1.Cells[0, 9].Text = dt.Rows[0]["CNT09"].ToString().Trim();
                SS1_Sheet1.Cells[0, 10].Text = dt.Rows[0]["CNT10"].ToString().Trim();
                SS1_Sheet1.Cells[0, 11].Text = dt.Rows[0]["JEMSU"].ToString().Trim();
                SS1_Sheet1.Cells[0, 12].Text = dt.Rows[0]["ID"].ToString().Trim();

                strData = dt.Rows[0]["EXCELDATA"].ToString().Trim();
                SS1_Sheet1.Cells[0, 13].Text = strData;

                ExcelSheet_Clear();
                Sheet_2_Excel();

            }
            dt.Dispose();
            dt = null;

        }

        private void ExcelSheet_Clear()
        {
            //기업건강증진지수
            SSExcel_Sheet1.Cells[2, 0].Text = "";
            SSExcel_Sheet1.Cells[2, 6].Text = "";
            //① 근골격계질환 예방	
            SSExcel_Sheet1.Cells[5, 11].Text = "";
            SSExcel_Sheet1.Cells[5, 12].Text = "";
            SSExcel_Sheet1.Cells[5, 13].Text = "";
            //② 뇌심혈관질환 예방	
            SSExcel_Sheet1.Cells[6, 11].Text = "";
            SSExcel_Sheet1.Cells[6, 12].Text = "";
            SSExcel_Sheet1.Cells[6, 13].Text = "";
            //③ 직무스트레스 관리	
            SSExcel_Sheet1.Cells[7, 11].Text = "";
            SSExcel_Sheet1.Cells[7, 12].Text = "";
            SSExcel_Sheet1.Cells[7, 13].Text = "";
            //④ 생활습관 개선	
            SSExcel_Sheet1.Cells[8, 11].Text = "";
            SSExcel_Sheet1.Cells[8, 12].Text = "";
            SSExcel_Sheet1.Cells[8, 13].Text = "";

            for (int i = 2; i<=12; i++)
            {
                if (i!=10) SSExcel_Sheet1.Cells[15, i].Text = "";
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j <= 11; j++)
                {
                    SSExcel_Sheet1.Cells[21 + i, j + 1].Text = "";
                }
            }

            SSExcel_Sheet1.Cells[31, 2].Text = "";
            SSExcel_Sheet1.Cells[34, 2].Text = "";
        }

        private void SS1_Clear()
        {
            for (int i = 0; i < SS1_Sheet1.ColumnCount; i++)
            {
                SS1_Sheet1.Cells[0, i].Text = "";
            }
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            Sheet_2_Excel();
        }

        private void Sheet_2_Excel()
        {
            //기업 건강증진 지수: 상호
            SSExcel_Sheet1.Cells[2, 0].Text = "기업 건강증진 지수: " + VB.Pstr(TxtLtdcode.Text.Trim(), ".", 2).Trim();
            //근로자수
            SSExcel_Sheet1.Cells[15, 2].Text = SS1_Sheet1.Cells[0, 1].Text.Trim();
            //장년50세
            SSExcel_Sheet1.Cells[15, 3].Text = SS1_Sheet1.Cells[0, 2].Text.Trim();
            //장시간
            SSExcel_Sheet1.Cells[15, 4].Text = SS1_Sheet1.Cells[0, 3].Text.Trim();
            //교대(야간)
            SSExcel_Sheet1.Cells[15, 5].Text = SS1_Sheet1.Cells[0, 4].Text.Trim();
            //근골부담
            SSExcel_Sheet1.Cells[15, 6].Text = SS1_Sheet1.Cells[0, 5].Text.Trim();
            //고객응대
            SSExcel_Sheet1.Cells[15, 7].Text = SS1_Sheet1.Cells[0, 6].Text.Trim();
            //근골격계질환
            SSExcel_Sheet1.Cells[15, 8].Text = SS1_Sheet1.Cells[0, 7].Text.Trim();
            //뇌심질환
            SSExcel_Sheet1.Cells[15, 9].Text = SS1_Sheet1.Cells[0, 8].Text.Trim();
            //검진미수검
            SSExcel_Sheet1.Cells[15, 11].Text = SS1_Sheet1.Cells[0, 9].Text.Trim();
            //일반질병요관찰·유소견자
            SSExcel_Sheet1.Cells[15, 12].Text = SS1_Sheet1.Cells[0, 10].Text.Trim();

            //자료가 설정되었으면 다시 설정 안함
            if (SSExcel_Sheet1.Cells[21, 2].Text.Trim() == "")
            {
                string strTemp = SS1_Sheet1.Cells[0, 13].Text.Trim();
                if (strTemp == "")
                {
                    strTemp = "O,O,O,미실시,O,O,O,X,X,해당없음,미실시{}";
                    strTemp += "O,O,O,미실시,O,O,O,X,X,해당없음,미실시{}";
                    strTemp += "O,X,X,미실시,X,X,X,X,X,해당없음,미실시{}";
                    strTemp += "O,X,X,미실시,X,X,X,X,X,해당없음,미실시{}";
                }

                if (strTemp != "")
                {
                    string strDAT = "";

                    //[2단계] 건강증진 활동 실태 (기준: 최근 3년간을 기준으로 해당란에 O/X 표시)													
                    for (int i = 0; i < 4; i++)
                    {
                        strDAT = VB.Pstr(strTemp, "{}", (i + 1));
                        for (int j = 1; j <= 11; j++)
                        {
                            SSExcel_Sheet1.Cells[21 + i, j + 1].Text = VB.Pstr(strDAT, ",", j);
                        }
                    }

                }

            }
            if (SSExcel_Sheet1.Cells[31, 2].Text.Trim() == "") SSExcel_Sheet1.Cells[31, 2].Text = "업무위탁요원";
            if (SSExcel_Sheet1.Cells[34, 2].Text.Trim() == "") SSExcel_Sheet1.Cells[34, 2].Text = "건강증진(간호사 등)";

            btnSave.Enabled = true;
            if (SS1_Sheet1.Cells[0, 10].Text.Trim() != "") btnDelete.Enabled = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strJumin = "";
            string strYear = "";
            int nYear = 0;
            int nAge = 0;
            int nCnt1 = 0;
            int nCnt2 = 0;
            int nCnt3 = 0;
            int nCnt4 = 0;

            string strNow = DateTime.Now.ToString("yyyy-MM-dd");

            SS1_Clear();
            ExcelSheet_Clear();

            SS1_Sheet1.Cells[0, 0].Text = strNow;
            SS1_Sheet1.Cells[0, 1].Text = "0";
            SS1_Sheet1.Cells[0, 2].Text = "0";
            SS1_Sheet1.Cells[0, 3].Text = "0";
            SS1_Sheet1.Cells[0, 4].Text = "0";
            SS1_Sheet1.Cells[0, 5].Text = "0";
            SS1_Sheet1.Cells[0, 6].Text = "0";
            SS1_Sheet1.Cells[0, 7].Text = "0";
            SS1_Sheet1.Cells[0, 8].Text = "0";
            SS1_Sheet1.Cells[0, 9].Text = "0";
            SS1_Sheet1.Cells[0, 10].Text = "0";

            //최근 간호사 상태보고서에서 인원수를 찾음
            SQL = "SELECT VISITDATE,CURRENTWORKERCOUNT FROM HIC_OSHA_REPORT_NURSE ";
            SQL = SQL + ComNum.VBLF + "WHERE SITE_ID=" + Selected_Ltd_id + " ";
            SQL = SQL + ComNum.VBLF + "  AND ISDELETED='N' ";
            SQL = SQL + ComNum.VBLF + "  AND CURRENTWORKERCOUNT>0 ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY VISITDATE DESC ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0) SS1_Sheet1.Cells[0, 1].Text = dt.Rows[0]["CURRENTWORKERCOUNT"].ToString().Trim();
            dt.Dispose();
            dt = null;

            //50세이상 근로자수
            SQL = "SELECT JUMIN FROM HIC_SITE_WORKER ";
            SQL = SQL + ComNum.VBLF + "WHERE SITEID=" + Selected_Ltd_id + " ";
            SQL = SQL + ComNum.VBLF + "  AND ISDELETED='N' ";
            SQL = SQL + ComNum.VBLF + "  AND END_DATE IS NULL ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            nCnt1 = 0;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strJumin = dt.Rows[i]["JUMIN"].ToString().Trim();
                    if (VB.Len(strJumin)== 6 || VB.Len(strJumin) == 7)
                    {
                        if (Int32.Parse(VB.Left(strJumin, 2)) > Int32.Parse(VB.Mid(strNow, 3, 2)))
                        {
                            nAge = Int32.Parse(VB.Left(strNow, 4)) - Int32.Parse("19" + VB.Left(strJumin, 2));
                        }
                        else
                        {
                            nAge = Int32.Parse(VB.Left(strNow, 4)) - Int32.Parse("20" + VB.Left(strJumin, 2));
                        }
                    }
                    else
                    {
                        nAge = Int32.Parse(VB.Left(strNow, 4)) - Int32.Parse(VB.Left(strJumin, 4));
                    }
                    if (nAge >= 50) nCnt1++;
                }
            }
            dt.Dispose();
            dt = null;
            SS1_Sheet1.Cells[0, 2].Text = nCnt1.ToString(); //50세이장 근로자수

            // 금년, 작년 사후관리소견서 건수를 읽음
            nYear = Int32.Parse(VB.Left(strNow, 4)) - 1;
            strYear = nYear.ToString();

            SQL = "SELECT YEAR,COUNT(*) AS CNT FROM HIC_LTD_RESULT3 ";
            SQL = SQL + ComNum.VBLF + "WHERE SITEID=" + Selected_Ltd_id + " ";
            SQL = SQL + ComNum.VBLF + "  AND YEAR>='" + strYear + "' ";
            SQL = SQL + ComNum.VBLF + "  AND JONG IN ('특수','일반') ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "GROUP BY YEAR ";
            SQL = SQL + ComNum.VBLF + "ORDER BY YEAR DESC ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            strYear = "";
            nCnt1 = 0;
            if (dt.Rows.Count > 0)
            {
                strYear = dt.Rows[0]["YEAR"].ToString().Trim();
                nCnt1 = Int32.Parse(dt.Rows[0]["CNT"].ToString().Trim());
            }
            dt.Dispose();
            dt = null;

            //사후관리소견서 기준으로 50세이상, 야간근로, 유소견자 건수를 구함
            if (nCnt1 > 0)
            {
                SQL = "SELECT AGE,YUHE,GGUBUN FROM HIC_LTD_RESULT3 ";
                SQL = SQL + ComNum.VBLF + "WHERE SITEID=" + Selected_Ltd_id + " ";
                SQL = SQL + ComNum.VBLF + "  AND YEAR='" + strYear + "' ";
                SQL = SQL + ComNum.VBLF + "  AND JONG IN ('특수','일반') ";
                SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Age ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                nCnt1 = 0;
                nCnt2 = 0;
                nCnt3 = 0;
                if (dt.Rows.Count > 0)
                {
                    for (int i=0; i < dt.Rows.Count; i++)
                    {
                        if (Int32.Parse(dt.Rows[i]["AGE"].ToString().Trim()) > 50) nCnt1++;
                        if (VB.InStr(dt.Rows[i]["YUHE"].ToString().Trim(),"야간") > 0) nCnt2++; 
                        nCnt4 = 0;
                        if (VB.InStr(dt.Rows[i]["GGUBUN"].ToString().Trim(), "C1") > 0) nCnt4++;
                        if (VB.InStr(dt.Rows[i]["GGUBUN"].ToString().Trim(), "C2") > 0) nCnt4++;
                        if (VB.InStr(dt.Rows[i]["GGUBUN"].ToString().Trim(), "D1") > 0) nCnt4++;
                        if (VB.InStr(dt.Rows[i]["GGUBUN"].ToString().Trim(), "D2") > 0) nCnt4++;
                        if (VB.InStr(dt.Rows[i]["GGUBUN"].ToString().Trim(), "CN") > 0) nCnt4++;
                        if (VB.InStr(dt.Rows[i]["GGUBUN"].ToString().Trim(), "DN") > 0) nCnt4++;
                        if (nCnt4 > 0) nCnt3++;
                    }
                    SS1_Sheet1.Cells[0, 2].Text = nCnt1.ToString(); //50세이장 근로자수
                    SS1_Sheet1.Cells[0, 4].Text = nCnt2.ToString(); //야간
                    SS1_Sheet1.Cells[0, 10].Text = nCnt3.ToString(); //유소견자
                }
                dt.Dispose();
                dt = null;
            }

        }

        private void btnExcelUpdate_Click(object sender, EventArgs e)
        {

        }


        private void btnExcelGet_Click(object sender, EventArgs e)
        {
            Excel_2_Sheet();
        }

        //엑셀의 자료를 SS1 시트로 옮기기
        private void Excel_2_Sheet()
        {
            SS1_Sheet1.Cells[0, 1].Text = SSExcel_Sheet1.Cells[15, 2].Text.Trim();
            SS1_Sheet1.Cells[0, 2].Text = SSExcel_Sheet1.Cells[15, 3].Text.Trim();
            SS1_Sheet1.Cells[0, 3].Text = SSExcel_Sheet1.Cells[15, 4].Text.Trim();
            SS1_Sheet1.Cells[0, 4].Text = SSExcel_Sheet1.Cells[15, 5].Text.Trim();
            SS1_Sheet1.Cells[0, 5].Text = SSExcel_Sheet1.Cells[15, 6].Text.Trim();
            SS1_Sheet1.Cells[0, 6].Text = SSExcel_Sheet1.Cells[15, 7].Text.Trim();
            SS1_Sheet1.Cells[0, 7].Text = SSExcel_Sheet1.Cells[15, 8].Text.Trim();
            SS1_Sheet1.Cells[0, 8].Text = SSExcel_Sheet1.Cells[15, 9].Text.Trim();
            SS1_Sheet1.Cells[0, 9].Text = SSExcel_Sheet1.Cells[15, 11].Text.Trim();
            SS1_Sheet1.Cells[0, 10].Text = SSExcel_Sheet1.Cells[15, 12].Text.Trim();
            SS1_Sheet1.Cells[0, 11].Text = SSExcel_Sheet1.Cells[2, 6].Text.Trim();

            string strDAT = "";

            //[2단계] 건강증진 활동 실태 (기준: 최근 3년간을 기준으로 해당란에 O/X 표시)													
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j <= 11; j++)
                {
                    strDAT += SSExcel_Sheet1.Cells[21 + i, j + 1].Text.Trim() + ",";
                }
                strDAT += "{}";
            }
            strDAT += "{*}";

            strDAT += SSExcel_Sheet1.Cells[31, 2].Text.Trim() + "{}";
            strDAT += SSExcel_Sheet1.Cells[34, 2].Text.Trim() + "{*}";

            SS1_Sheet1.Cells[0, 13].Text = strDAT;

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnMail_Click(object sender, EventArgs e)
        {
            try
            {
                if (Selected_Ltd_id == 0)
                {
                    MessageUtil.Alert("사업장 정보가 없습니다");
                    return;
                }

                string strPath = "";

                Cursor.Current = Cursors.WaitCursor;
                HcCodeService codeService = new HcCodeService();
                HC_CODE pdfPath = codeService.FindActiveCodeByGroupAndCode("PDF_PATH", "OSHA_ESTIMATE", "OSHA");
                string title = "기업건강증진지수_" + VB.Pstr(TxtLtdcode.Text.Trim(), ".", 2).Trim();
                strPath = pdfPath.CodeName + "\\" + title + ".xls";
                bool bOk = SSExcel.SaveExcel(strPath, FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);

                Thread.Sleep(3000);

                HC_CODE sendMailAddress = codeService.FindActiveCodeByGroupAndCode("OSHA_MANAGER", "mail", "OSHA");
                EstimateMailForm form = new EstimateMailForm();
                form.set_SiteId(Selected_Ltd_id);
                form.GetMailForm().SenderMailAddress = sendMailAddress.CodeName;
                form.GetMailForm().AttachmentsList.Add(strPath);

                string strMailList = GetEmail();
                if (strMailList != "")
                {
                    for (int i = 1; i <= VB.L(strMailList, ","); i++)
                    {
                        form.GetMailForm().ReciverMailSddress.Add(VB.Pstr(strMailList, ",", i).Trim());
                    }
                    form.GetMailForm().RefreshReceiver();
                }
                form.GetMailForm().Subject = title;
                form.GetMailForm().Body = title;

                DialogResult result = form.ShowDialog();

                if (result == DialogResult.OK)
                {
                    HIC_OSHA_MAIL_SEND mail = new HIC_OSHA_MAIL_SEND
                    {
                        SITE_ID = Selected_Ltd_id,
                        SEND_TYPE = MAIL_TYPE.ToString(),
                        SEND_USER = clsType.User.Sabun
                    };

                    int res = hicMailRepository.Insert(mail);

                }
            }
            catch (Exception ex)
            {
                MessageUtil.Alert(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }

        private string GetEmail()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            string strEMail = "";
            string strList = "";
            string email = string.Empty;

            SQL = "";
            SQL = "SELECT NAME,EMAIL FROM HIC_OSHA_CONTRACT_MANAGER ";
            SQL = SQL + ComNum.VBLF + "WHERE ESTIMATE_ID IN (SELECT MAX(ESTIMATE_ID) AS ESTIMATE_ID ";
            SQL = SQL + ComNum.VBLF + "                        FROM HIC_OSHA_CONTRACT ";
            SQL = SQL + ComNum.VBLF + "                       WHERE OSHA_SITE_ID=" + Selected_Ltd_id + " ";
            SQL = SQL + ComNum.VBLF + "                         AND ESTIMATE_ID > 0 ) ";
            SQL = SQL + ComNum.VBLF + "  AND (EMAILSEND='Y' OR EMAILSEND='y') ";
            SQL = SQL + ComNum.VBLF + "  AND WORKER_ROLE='HEALTH_ROLE' ";
            SQL = SQL + ComNum.VBLF + "  AND EMAIL IS NOT NULL ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strEMail = dt.Rows[i]["EMAIL"].ToString().Trim();
                    if (strEMail != "")
                    {
                        if (strList == "")
                        {
                            strList = strEMail;
                        }
                        else
                        {
                            strList += "," + strEMail;
                        }
                    }
                }
            }
            dt.Dispose();
            dt = null;

            return strList;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            long nID = 0;
            string strWDate = "";
            double nJemsu = 0;
            long nCnt1 = 0;
            long nCnt2 = 0;
            long nCnt3 = 0;
            long nCnt4 = 0;
            long nCnt5 = 0;
            long nCnt6 = 0;
            long nCnt7 = 0;
            long nCnt8 = 0;
            long nCnt9 = 0;
            long nCnt10 = 0;
            string strDAT = "";

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Excel_2_Sheet();

            strWDate = SS1_Sheet1.Cells[0, 0].Text.Trim();
            if (SS1_Sheet1.Cells[0, 11].Text.Trim()!="") nJemsu = double.Parse(SS1_Sheet1.Cells[0, 11].Text.Trim());
            nCnt1 = long.Parse(SS1_Sheet1.Cells[0, 1].Text.Trim());
            nCnt2 = long.Parse(SS1_Sheet1.Cells[0, 2].Text.Trim());
            nCnt3 = long.Parse(SS1_Sheet1.Cells[0, 3].Text.Trim());
            nCnt4 = long.Parse(SS1_Sheet1.Cells[0, 4].Text.Trim());
            nCnt5 = long.Parse(SS1_Sheet1.Cells[0, 5].Text.Trim());
            nCnt6 = long.Parse(SS1_Sheet1.Cells[0, 6].Text.Trim());
            nCnt7 = long.Parse(SS1_Sheet1.Cells[0, 7].Text.Trim());
            nCnt8 = long.Parse(SS1_Sheet1.Cells[0, 8].Text.Trim());
            nCnt9 = long.Parse(SS1_Sheet1.Cells[0, 9].Text.Trim());
            nCnt10 = long.Parse(SS1_Sheet1.Cells[0, 10].Text.Trim());
            if (SS1_Sheet1.Cells[0, 12].Text.Trim()!= "") nID = long.Parse(SS1_Sheet1.Cells[0, 12].Text.Trim());
            strDAT = SS1_Sheet1.Cells[0, 13].Text.Trim();

            if (nID == 0)
            {
                nID = GET_SeqNextVal("HC_OSHA_EHP_ID_SEQ");

                SQL = "INSERT INTO HIC_OSHA_EHP (ID,SITE_ID,WDATE,JEMSU,CNT01,CNT02,CNT03,";
                SQL = SQL + " CNT04,CNT05,CNT06,CNT07,CNT08,CNT09,CNT10,EXCELDATA,MODIFIED,";
                SQL = SQL + " MODIFIEDUSER,CREATED,CREATEDUSER,SWLICENSE) VALUES (";
                SQL = SQL + nID + "," + Selected_Ltd_id + ",'" + strWDate + "',";
                SQL = SQL + nJemsu + "," + nCnt1 + "," + nCnt2 + "," + nCnt3 + ",";
                SQL = SQL + nCnt4 + "," + nCnt5 + "," + nCnt6 + "," + nCnt7 + ",";
                SQL = SQL + nCnt8 + "," + nCnt9 + "," + nCnt10 + ",'" + strDAT + "',";
                SQL = SQL + "SYSDATE,'" + clsType.User.Sabun + "',SYSDATE,'" + clsType.User.Sabun + "',";
                SQL = SQL + "'" + clsType.HosInfo.SwLicense + "') ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("HIC_OSHA_EHP 자료 등록시 오류가 발생함", "알림");
                    Cursor.Current = Cursors.Default;
                }
            }
            else
            {
                SQL = "UPDATE HIC_OSHA_EHP SET ";
                SQL = SQL + " WDATE = '" + strWDate + "', ";
                SQL = SQL + " JEMSU = " + nJemsu + ", ";
                SQL = SQL + " CNT01 = " + nCnt1 + ", ";
                SQL = SQL + " CNT02 = " + nCnt2 + ", ";
                SQL = SQL + " CNT03 = " + nCnt3 + ", ";
                SQL = SQL + " CNT04 = " + nCnt4 + ", ";
                SQL = SQL + " CNT05 = " + nCnt5 + ", ";
                SQL = SQL + " CNT06 = " + nCnt6 + ", ";
                SQL = SQL + " CNT07 = " + nCnt7 + ", ";
                SQL = SQL + " CNT08 = " + nCnt8 + ", ";
                SQL = SQL + " CNT09 = " + nCnt9 + ", ";
                SQL = SQL + " CNT10 = " + nCnt10 + ", ";
                SQL = SQL + " MODIFIED = SYSDATE, ";
                SQL = SQL + " MODIFIEDUSER = '" + clsType.User.Sabun + "' ";
                SQL = SQL + "WHERE ID=" + nID + " ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("HIC_OSHA_EHP 자료 변경시 오류가 발생함", "알림");
                    Cursor.Current = Cursors.Default;
                }
            }

            Search();

        }

        private void FrmGiupEHP_Load(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            long nID = 0;

            if (SS1_Sheet1.Cells[0, 12].Text.Trim() == "") return;

            nID = long.Parse(SS1_Sheet1.Cells[0, 12].Text.Trim());
            if (nID == 0) return;

            if (ComFunc.MsgBoxQ("정말로 삭제를 하시겠습니까?", "삭제여부", MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                SQL = "DELETE FROM HIC_OSHA_EHP ";
                SQL = SQL + "WHERE ID=" + nID + " ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("HIC_OSHA_EHP 자료 삭제시 오류가 발생함", "알림");
                    Cursor.Current = Cursors.Default;
                }
            }
            Search();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strData = "";
            string strNow = DateTime.Now.ToString("yyyy-MM-dd");
            string strLtd = VB.Pstr(TxtLtdcode.Text, ".", 1).Trim();
            if (strLtd == "") { ComFunc.MsgBox("회사코드가 공란입니다."); return; }

            SS1_Clear();
            ExcelSheet_Clear();

            SS1_Sheet1.Cells[0, 0].Text = strNow;
            SS1_Sheet1.Cells[0, 1].Text = "0";
            SS1_Sheet1.Cells[0, 2].Text = "0";
            SS1_Sheet1.Cells[0, 3].Text = "0";
            SS1_Sheet1.Cells[0, 4].Text = "0";
            SS1_Sheet1.Cells[0, 5].Text = "0";
            SS1_Sheet1.Cells[0, 6].Text = "0";
            SS1_Sheet1.Cells[0, 7].Text = "0";
            SS1_Sheet1.Cells[0, 8].Text = "0";
            SS1_Sheet1.Cells[0, 9].Text = "0";
            SS1_Sheet1.Cells[0, 10].Text = "0";
            SS1_Sheet1.Cells[0, 11].Text = "";
            SS1_Sheet1.Cells[0, 12].Text = "";

            //계약번호를 찾기
            SQL = "SELECT * FROM HIC_OSHA_EHP ";
            SQL = SQL + ComNum.VBLF + "WHERE SITE_ID=" + strLtd + " ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY WDATE DESC ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                SS1_Sheet1.Cells[0, 1].Text = dt.Rows[0]["CNT01"].ToString().Trim();
                SS1_Sheet1.Cells[0, 2].Text = dt.Rows[0]["CNT02"].ToString().Trim();
                SS1_Sheet1.Cells[0, 3].Text = dt.Rows[0]["CNT03"].ToString().Trim();
                SS1_Sheet1.Cells[0, 4].Text = dt.Rows[0]["CNT04"].ToString().Trim();
                SS1_Sheet1.Cells[0, 5].Text = dt.Rows[0]["CNT05"].ToString().Trim();
                SS1_Sheet1.Cells[0, 6].Text = dt.Rows[0]["CNT06"].ToString().Trim();
                SS1_Sheet1.Cells[0, 7].Text = dt.Rows[0]["CNT07"].ToString().Trim();
                SS1_Sheet1.Cells[0, 8].Text = dt.Rows[0]["CNT08"].ToString().Trim();
                SS1_Sheet1.Cells[0, 9].Text = dt.Rows[0]["CNT09"].ToString().Trim();
                SS1_Sheet1.Cells[0, 10].Text = dt.Rows[0]["CNT10"].ToString().Trim();
                SS1_Sheet1.Cells[0, 11].Text = dt.Rows[0]["JEMSU"].ToString().Trim();
                strData = dt.Rows[0]["EXCELDATA"].ToString().Trim();
                SS1_Sheet1.Cells[0, 13].Text = strData;

                ExcelSheet_Clear();
                Sheet_2_Excel();
            }
            else
            {
                ComFunc.MsgBox("최근 자료가 없습니다.");
            }
            dt.Dispose();
            dt = null;
        }
    }
}

