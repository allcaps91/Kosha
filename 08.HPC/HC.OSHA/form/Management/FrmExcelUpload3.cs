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
                SSConv_Sheet1.Cells[i, 1].Value = (i + 1);
            }

            btnJob1.Enabled = true;
            btnJob2.Enabled = false;
            btnJob3.Enabled = false;
            btnJob4.Enabled = false;
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
            string strID = "";
            long nLtdCode = 0;
            string strGongjeng = "";
            string strName = "";
            string strBirth = "";
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

            int intRowAffected = 0;

            nLtdCode = long.Parse(VB.Pstr(TxtLtdcode.Text, ".", 1));
            strYear = cboYear.Text.Trim();
            strBangi = VB.Pstr(cboBangi.Text, ".", 1);

            HC_SITE_WORKER worker = new HC_SITE_WORKER();

            for (int i = 0; i < SS1_Sheet1.RowCount; i++)
            {
                strGongjeng = SS1_Sheet1.Cells[i, 0].Text.ToString();
                strName = SS1_Sheet1.Cells[i, 1].Text.ToString();
                strSex = SS1_Sheet1.Cells[i, 2].Text.ToString();
                strAge = SS1_Sheet1.Cells[i, 3].Text.ToString();
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

                // 뇌심혈관 결과 DB에 저장
                if (Exist_Ltd_Result3(nLtdCode, strYear,strBangi, strID) == false)
                {
                    try
                    {
                        SQL = "";
                        SQL += " INSERT INTO HIC_LTD_RESULT3 (SITEID,YEAR,BANGI,ID,NAME,BIRTH,JINDATE, ";
                        SQL += " SEX,AGE,GUNSOK,YUHE,JIPYO,GGUBUN,SOGEN,SAHU,UPMU,JOBSABUN,ENTTIME,SWLICENSE) ";
                        SQL += " VALUES (" + nLtdCode + ",'" + strYear + "','" + strBangi + "','";
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
                        ComFunc.MsgBox(ex.Message);
                        return;
                    }
                }
            }
            ComFunc.MsgBox("서버로 전송 완료", "알림");
            Screen_Set();
        }

        // 년도,반기 및 사원번호로 금년도 등록여부 점검
        bool Exist_Ltd_Result3(long nLtdCode, string strYear,string strBangi, string strID)
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
    }
}


