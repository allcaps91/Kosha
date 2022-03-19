using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Utils;
using ComBase.Mvc.Enums;
using ComHpcLibB.Model;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using HC.Core.Common.Util;
using HC.Core.Dto;
using HC.Core.Model;
using HC.Core.Repository;
using HC.Core.Service;
using HC.Core.Common.Interface;
using HC.OSHA.Dto;
using HC.OSHA.Dto.StatusReport;
using HC.OSHA.Repository;
using HC.OSHA.Repository.StatusReport;
using HC.OSHA.Service.StatusReport;
using HC_Core;
using HC_Core.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Data;

namespace HC_OSHA
{
    public partial class FrmSaupjang : Form
    {
        private HcUserService hcUsersService;

        public FrmSaupjang()
        {
            InitializeComponent();
            hcUsersService = new HcUserService();
            Screen_Set();
        }

        private void Screen_Set()
        {
            int i = 0;
            int nYear = Int32.Parse(DateTime.Now.ToString("yyyy"));

            if (cboYear.Items.Count == 0)
            {
                for (i = 0; i < 5; i++)
                {
                    cboYear.Items.Add(nYear.ToString());
                    nYear--;
                }
            }
            cboYear.SelectedIndex = 0;

            cboJob.Items.Add("전체");
            cboJob.Items.Add("신규");
            cboJob.Items.Add("해지");
            cboJob.SelectedIndex = 0;

            List<HC_USER> OSHAUsers = hcUsersService.GetOsha();
            CboManager.SetItems(OSHAUsers, "Name", "UserId", "전체", "", AddComboBoxPosition.Top);
            CboManager.SetValue(CommonService.Instance.Session.UserId);

            //SSList.Initialize(new SpreadOption() { IsRowSelectColor = false, RowHeightAuto = true, RowHeaderVisible = true, ColumnHeaderHeight = 40 });
            //SSList.AddColumnText("코드", "", 50, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("사업장명", "", 120, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = false, Aligen = CellHorizontalAlignment.Left });
            //SSList.AddColumnText("의사", "", 60, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = false, Aligen = CellHorizontalAlignment.Center });
            //SSList.AddColumnText("간호사", "", 60, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = false, Aligen = CellHorizontalAlignment.Center });
            //SSList.AddColumnText("산업위생", "", 65, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = false, Aligen = CellHorizontalAlignment.Center });
            //SSList.AddColumnText("보건관리자", "", 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = false, WordWrap = false, Aligen = CellHorizontalAlignment.Left });
            //SSList.AddColumnText("계약일", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("계약시작일", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("계약종료일", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true, Aligen = CellHorizontalAlignment.Center });
            //SSList.AddColumnText("해지일", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true, Aligen = CellHorizontalAlignment.Center });
            //SSList.AddColumnText("선임일", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("계약단가", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true, Aligen = CellHorizontalAlignment.Center });
            //SSList.AddColumnText("근로자수", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true, Aligen = CellHorizontalAlignment.Center });
            //SSList.AddColumnText("위치", "", 90, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = false, Aligen = CellHorizontalAlignment.Center });
            //SSList.AddColumnText("건물소유", "", 40, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = false, Aligen = CellHorizontalAlignment.Center });
            //SSList.AddColumnText("교대", "", 40, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = false, Aligen = CellHorizontalAlignment.Center });
            //SSList.AddColumnText("노조", "", 40, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = false, Aligen = CellHorizontalAlignment.Center });
            //SSList.AddColumnText("생산", "", 40, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = false, Aligen = CellHorizontalAlignment.Center });
            //SSList.AddColumnText("측정", "", 40, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = false, Aligen = CellHorizontalAlignment.Center });
            //SSList.AddColumnText("근골격계", "", 40, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = false, Aligen = CellHorizontalAlignment.Center });
            //SSList.AddColumnText("청력보존", "", 40, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = false, Aligen = CellHorizontalAlignment.Center });
            //SSList.AddColumnText("뇌심혈관", "", 40, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = false, Aligen = CellHorizontalAlignment.Center });
            //SSList.AddColumnText("특별물질", "", 40, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = false, Aligen = CellHorizontalAlignment.Center });
            //SSList.AddColumnText("위원회", "", 40, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = false, Aligen = CellHorizontalAlignment.Center });
            //SSList.AddColumnText("밀폐공간", "", 40, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = false, Aligen = CellHorizontalAlignment.Center });
            //SSList.AddColumnText("스트레스", "", 40, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = false, Aligen = CellHorizontalAlignment.Center });
            //SSList.AddColumnText("우편번호", "", 65, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("주 소", "", 130, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = false, Aligen = CellHorizontalAlignment.Left });
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            string strName = TxtName.Text.Trim();
            string strJob = cboJob.Text.Trim();

            if (cboYear.Text.Trim() == "")
            {
                ComFunc.MsgBox("작업년도를 선택하세요");
                return;
            }
            string strManager = CboManager.GetValue();

            Cursor.Current = Cursors.WaitCursor;

            SSList.ActiveSheet.RowCount = 0;

            SQL = "SELECT A.*,B.SANGHO,B.MAILCODE,B.JUSO,C.NAME AS UserName1,D.NAME AS UserName2,E.NAME AS UserName3 ";
            SQL = SQL + ComNum.VBLF + " FROM HIC_OSHA_CONTRACT A, HIC_LTD B,HIC_USERS C,HIC_USERS D,HIC_USERS E ";
            SQL = SQL + ComNum.VBLF + "WHERE A.ISDELETED='N' "; //삭제 제외
            if (strJob == "신규")
            {
                SQL = SQL + ComNum.VBLF + "  AND A.CONTRACTDATE>='" + cboYear.Text + "-01-01' ";
                SQL = SQL + ComNum.VBLF + "  AND A.CONTRACTDATE<='" + cboYear.Text + "-12-31' ";
            }
            else if(strJob == "해지")
            {
                SQL = SQL + ComNum.VBLF + "  AND A.TERMINATEDATE>='" + cboYear.Text + "-01-01' ";
                SQL = SQL + ComNum.VBLF + "  AND A.TERMINATEDATE<='" + cboYear.Text + "-12-31' ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "  AND A.CONTRACTENDDATE>='" + cboYear.Text + "-01-01' ";
                SQL = SQL + ComNum.VBLF + "  AND A.CONTRACTSTARTDATE<='" + cboYear.Text + "-12-31' ";
            }
            // 회사이름으로 검색
            if (strName != "") SQL = SQL + ComNum.VBLF + "  AND A.NAME LIKE '%" + strName + "%' ";
            //관리자별
            if (strManager != "")
            {
                SQL = SQL + ComNum.VBLF + "  AND (MANAGEDOCTOR='" + strManager + "' ";
                SQL = SQL + ComNum.VBLF + "   OR  MANAGENURSE='" + strManager + "' ";
                SQL = SQL + ComNum.VBLF + "   OR  MANAGEENGINEER='" + strManager + "') ";
            }
            SQL = SQL + ComNum.VBLF + "  AND A.OSHA_SITE_ID=B.CODE(+) ";
            SQL = SQL + ComNum.VBLF + "  AND A.MANAGEDOCTOR=C.USERID(+) ";
            SQL = SQL + ComNum.VBLF + "  AND A.MANAGENURSE=D.USERID(+) ";
            SQL = SQL + ComNum.VBLF + "  AND A.MANAGEENGINEER=E.USERID(+) ";
            SQL = SQL + ComNum.VBLF + "  AND A.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "  AND B.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "  AND C.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "  AND D.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "  AND E.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY B.SANGHO,A.OSHA_SITE_ID ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                SSList.ActiveSheet.RowCount = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SSList.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["OSHA_SITE_ID"].ToString().Trim();
                    SSList.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["SANGHO"].ToString().Trim();
                    SSList.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["UserName1"].ToString().Trim();
                    SSList.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["UserName2"].ToString().Trim();
                    SSList.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["UserName3"].ToString().Trim();
                    SSList.ActiveSheet.Cells[i, 5].Text = GET_BogenName(dt.Rows[i]["ESTIMATE_ID"].ToString().Trim());
                    SSList.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["CONTRACTDATE"].ToString().Trim();
                    SSList.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["CONTRACTSTARTDATE"].ToString().Trim();
                    SSList.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["CONTRACTENDDATE"].ToString().Trim();
                    SSList.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["TERMINATEDATE"].ToString().Trim();
                    SSList.ActiveSheet.Cells[i, 10].Text = dt.Rows[i]["DECLAREDAY"].ToString().Trim(); //선임일
                    SSList.ActiveSheet.Cells[i, 11].Text = VB.Format(dt.Rows[i]["COMMISSION"], "#,##0");
                    SSList.ActiveSheet.Cells[i, 12].Text = VB.Format(dt.Rows[i]["WORKERTOTALCOUNT"], "#,##0");
                    //위치
                    if (dt.Rows[i]["POSITION"].ToString().Trim() == "0") SSList.ActiveSheet.Cells[i, 13].Text = "공단";
                    if (dt.Rows[i]["POSITION"].ToString().Trim() == "1") SSList.ActiveSheet.Cells[i, 13].Text = "논공";
                    if (dt.Rows[i]["POSITION"].ToString().Trim() == "2") SSList.ActiveSheet.Cells[i, 13].Text = "도심";
                    if (dt.Rows[i]["POSITION"].ToString().Trim() == "3") SSList.ActiveSheet.Cells[i, 13].Text = "기타";
                    //건물소유
                    if (dt.Rows[i]["BUILDINGTYPE"].ToString().Trim() == "0") SSList.ActiveSheet.Cells[i, 14].Text = "자가";
                    if (dt.Rows[i]["BUILDINGTYPE"].ToString().Trim() == "1") SSList.ActiveSheet.Cells[i, 14].Text = "임대";
                    //교대
                    if (dt.Rows[i]["ISROTATION"].ToString().Trim() == "0") SSList.ActiveSheet.Cells[i, 15].Text = "유";
                    if (dt.Rows[i]["ISROTATION"].ToString().Trim() == "1") SSList.ActiveSheet.Cells[i, 15].Text = "무";
                    //교대
                    if (dt.Rows[i]["ISLABOR"].ToString().Trim() == "0") SSList.ActiveSheet.Cells[i, 16].Text = "유";
                    if (dt.Rows[i]["ISLABOR"].ToString().Trim() == "1") SSList.ActiveSheet.Cells[i, 16].Text = "무";
                    //생산
                    if (dt.Rows[i]["ISPRODUCTTYPE"].ToString().Trim() == "0") SSList.ActiveSheet.Cells[i, 17].Text = "독립";
                    if (dt.Rows[i]["ISPRODUCTTYPE"].ToString().Trim() == "1") SSList.ActiveSheet.Cells[i, 17].Text = "하청";

                    SSList.ActiveSheet.Cells[i, 18].Text = dt.Rows[i]["ISWEM"].ToString().Trim(); //측정
                    SSList.ActiveSheet.Cells[i, 19].Text = dt.Rows[i]["ISSKELETON"].ToString().Trim(); //근골격계
                    SSList.ActiveSheet.Cells[i, 20].Text = dt.Rows[i]["ISEARPROGRAM"].ToString().Trim(); //청력보존
                    SSList.ActiveSheet.Cells[i, 21].Text = dt.Rows[i]["ISBRAINTEST"].ToString().Trim(); //뇌심혈관
                    SSList.ActiveSheet.Cells[i, 22].Text = dt.Rows[i]["ISSPECIAL"].ToString().Trim(); //특별물질
                    SSList.ActiveSheet.Cells[i, 23].Text = dt.Rows[i]["ISCOMMITTEE"].ToString().Trim(); //위원회
                    SSList.ActiveSheet.Cells[i, 24].Text = dt.Rows[i]["ISSPACEPROGRAM"].ToString().Trim(); //밀폐공간
                    SSList.ActiveSheet.Cells[i, 25].Text = dt.Rows[i]["ISSTRESS"].ToString().Trim(); //스트레스
                    SSList.ActiveSheet.Cells[i, 26].Text = dt.Rows[i]["MAILCODE"].ToString().Trim(); //우편번호
                    SSList.ActiveSheet.Cells[i, 27].Text = dt.Rows[i]["JUSO"].ToString().Trim(); //주 소

                    SSList_Sheet1.Rows[i].Height = SSList_Sheet1.Rows[i].GetPreferredHeight() + 10;
                }
            }
            dt.Dispose();
            dt = null;

            Cursor.Current = Cursors.Default;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            SpreadPrint sp = new SpreadPrint(SSList, PrintStyle.STANDARD_REPORT);
            sp.Title = cboYear.Text.Trim() + "년 사업장 현황";
            //sp.Period = "회사명: " + TxtLtdcode.Text + "          " + "근로자: " + TxtName.Text;
            sp.orientation = PrintOrientation.Landscape;
            sp.Execute();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            bool bOk = SSList.SaveExcel("c:\\temp\\사업장현황.xls", FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
            {
                if (bOk == true)
                    ComFunc.MsgBox("Temp 폴더에 엑셀파일이 생성이 되었습니다.", "확인");
                else
                    ComFunc.MsgBox("엑셀파일 생성에 오류가 발생 하였습니다.", "확인");
            }
        }

        //회사 보건관리자 이름 찾기
        private string GET_BogenName(string ESTIMATE_ID)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt2 = null;
            string strName = "";
            string email = string.Empty;

            SQL = "";
            SQL = "SELECT NAME,TEL,HP FROM HIC_OSHA_CONTRACT_MANAGER ";
            SQL = SQL + ComNum.VBLF + "WHERE ESTIMATE_ID=" + ESTIMATE_ID + " ";
            SQL = SQL + ComNum.VBLF + "  AND WORKER_ROLE='HEALTH_ROLE' ";
            SQL = SQL + ComNum.VBLF + "  AND ISDELETED='N' ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
            strName = "";
            if (dt2.Rows.Count > 0)
            {
                strName = dt2.Rows[0]["NAME"].ToString().Trim();
                if (dt2.Rows[0]["HP"].ToString().Trim()!="")
                {
                    strName += "," + dt2.Rows[0]["HP"].ToString().Trim();
                }
                else if (dt2.Rows[0]["TEL"].ToString().Trim()!="")
                {
                    strName += "," + dt2.Rows[0]["TEL"].ToString().Trim();
                }
            }

            dt2.Dispose();
            dt2 = null;

            return strName;
        }
    }
}
