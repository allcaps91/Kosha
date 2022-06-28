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
    public partial class FrmExcelUpload1 : Form
    {
        private HcSiteWorkerService hcSiteWorkerService;
        private HcSiteWorkerRepository hcSiteWorkerRepository;
        private int[] FnCol = new int[1000];

        public FrmExcelUpload1()
        {
            InitializeComponent();
            hcSiteWorkerService = new HcSiteWorkerService();
            hcSiteWorkerRepository = new HcSiteWorkerRepository();
            Screen_Set();
        }

        private void Screen_Set()
        {
            int i = 0;
            string strTitle = "";

            TxtLtdcode.Text = "";

            //회사관계자 로그인
            if (clsType.User.LtdUser != "")
            {
                TxtLtdcode.Text = clsType.User.LtdUser + "." + clsType.User.JobName;
                TxtLtdcode.Enabled = false;
                button1.Enabled = false;
            }

            for (i = 0; i < 1000; i++)
            {
                FnCol[i] = 0;
            }

            btnJob1.Enabled = true;
            btnJob2.Enabled = false;
            btnJob3.Enabled = false;
            btnJob4.Enabled = false;
            btnJob5.Enabled = false;

            SS1_Sheet1.RowCount = 0;
            SSConv_Sheet1.RowCount = 0;
            SSConv_Sheet1.RowCount = SS1_Sheet1.ColumnCount;

            for (i = 0; i < SS1_Sheet1.ColumnCount; i++)
            {
                strTitle = SS1_Sheet1.ColumnHeader.Cells[0, i].Value.ToString();
                SSConv_Sheet1.Cells[i, 0].Text = strTitle;
                SSConv_Sheet1.Cells[i, 1].Value = "";
            }
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
                SSExcel.ActiveSheet.RowCount = 0;
                SSExcel.ActiveSheet.OpenExcel(strFileName, 0);
                btnJob5.Enabled = true;

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
            string strTemp = "";

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

                    for (j = 0; j < SSConv_Sheet1.RowCount; j++)
                    {
                        if (FnCol[j] > 0)
                        {
                            strData = SSExcel_Sheet1.Cells[i, FnCol[j] - 1].Text.ToString();
                            if (strData != "")
                            {
                                //생년월일 형식변경
                                if (j==1 && VB.Len(strData)==10)  //1961-03-01
                                {
                                    strTemp = VB.Mid(strData, 3, 2) + VB.Mid(strData, 6, 2) + VB.Right(strData, 2);
                                    strData = strTemp;
                                }
                                if (j == 1 && VB.Len(strData) == 8)  //19610301
                                {
                                    strTemp = VB.Mid(strData, 3, 2) + VB.Mid(strData, 5, 2) + VB.Right(strData, 2);
                                    strData = strTemp;
                                }

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
            string strID = "";
            long nLtdCode = 0;
            string strName = "";
            string strBirth = "";
            string strBuse = "";
            string strJik = "";
            string strSabun = "";
            string strEND_DATE = "";
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            nLtdCode = long.Parse(VB.Pstr(TxtLtdcode.Text, ".", 1));
            HC_SITE_WORKER worker = new HC_SITE_WORKER();

            for (int i = 0; i < SS1_Sheet1.RowCount; i++)
            {
                strName = SS1_Sheet1.Cells[i, 0].Text.ToString();
                strBirth = SS1_Sheet1.Cells[i, 1].Text.ToString();
                if (VB.Len(strBirth) > 6) strBirth = VB.Left(strBirth, 6);
                strBuse = SS1_Sheet1.Cells[i, 2].Text.ToString();
                strEND_DATE = SS1_Sheet1.Cells[i, 5].Text.ToString();
                if (strName=="")
                {
                    ComFunc.MsgBox((i+1) + "번줄 이름이 공란입니다.", "오류");
                    return;
                }
                if (strBirth.IsNumeric()==false)
                {
                    ComFunc.MsgBox((i + 1) + "번줄 생년월일이 숫자가 아닙니다.", "오류");
                    return;
                }
                if (strBirth=="000000" || strBirth=="123456" || VB.Len(strBirth)!=6)
                {
                    ComFunc.MsgBox((i + 1) + "번줄 생년월일이 이상합니다.", "오류");
                    return;
                }

                if (strEND_DATE!="")
                {
                    if (VB.Len(strEND_DATE)!=10 || VB.Mid(strEND_DATE,5,1)!="-")
                    {
                        ComFunc.MsgBox((i + 1) + "번줄 퇴사일자 형식이 오류입니다.(ex:1993-12-24)", "오류");
                        return;
                    }
                }
            }

            for (int i = 0; i < SS1_Sheet1.RowCount; i++)
            {
                strName = SS1_Sheet1.Cells[i, 0].Text.ToString();
                strBirth = SS1_Sheet1.Cells[i, 1].Text.ToString();
                if (VB.Len(strBirth) > 6) strBirth = VB.Left(strBirth, 6);
                strBuse = SS1_Sheet1.Cells[i, 2].Text.ToString();
                strJik = SS1_Sheet1.Cells[i, 3].Text.ToString();
                strSabun = SS1_Sheet1.Cells[i, 4].Text.ToString();
                strEND_DATE = SS1_Sheet1.Cells[i, 5].Text.ToString();

                // 사원이 없으면 신규등록, 있으면 업데이트
                worker.ID = "";
                worker.SITEID = nLtdCode;
                worker.NAME = strName;
                worker.DEPT = strBuse;
                worker.WORKER_ROLE = strJik;
                worker.SABUN = strSabun;
                worker.TEL = "";
                worker.ISDELETED = "N";
                worker.JUMIN = strBirth;
                worker.PANO = 0;
                worker.PTNO = "";
                worker.IPSADATE = "";
                worker.END_DATE = strEND_DATE;
                HC_SITE_WORKER saved = hcSiteWorkerRepository.FindOneByBirth(nLtdCode, strName, strBirth);
                if (saved == null)
                {
                    saved = hcSiteWorkerRepository.Insert(worker);
                }
                else
                {
                    saved = hcSiteWorkerRepository.Update(worker);
                }

            }
            ComFunc.MsgBox("서버로 전송 완료", "알림");
            Screen_Set();
        }

        private void button1_Click(object sender, EventArgs e)
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
                    if (bOK == false && strHead == "이름")
                    {
                        if (strData == "성명") bOK = true;
                    }
                    if (bOK == false && strHead == "생년월일")
                    {
                        if (strData == "주민등록") bOK = true;
                        if (strData == "주민번호") bOK = true;
                        if (strData == "주민등록번호") bOK = true;
                    }
                    if (bOK == false && strHead == "부서")
                    {
                        if (strData == "소속") bOK = true;
                        if (strData == "공정") bOK = true;
                        if (strData == "공정(부서)") bOK = true;
                    }
                    if (bOK == false && strHead == "직책")
                    {
                        if (strData == "직위") bOK = true;
                    }
                    if (bOK == false && strHead == "사원번호")
                    {
                        if (strData == "사번") bOK = true;
                        if (strData == "직번") bOK = true;

                    }
                    if (bOK == false && strHead == "퇴사일자")
                    {
                        if (strData == "퇴직일") bOK = true;
                        if (strData == "퇴사일") bOK = true;
                        if (strData == "퇴사일자") bOK = true;
                        if (strData == "퇴직일자") bOK = true;
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

        private void SSConv_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }
    }
}
