using ComBase;
using ComBase.Mvc.Utils;
using ComDbB;
using ComHpcLibB;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using HC_Core;
using HC.Core.Common.Util;
using HC.Core.Dto;
using HC.Core.Model;
using HC.Core.Repository;
using HC.Core.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace HC_OSHA.StatusReport
{
    public partial class FrmWorkerJiindan : Form
    {
        private string Send_Data = "";
        private string CHART_DATE = "";
        private long OLD_ID = 0;

        public FrmWorkerJiindan()
        {
            InitializeComponent();
        }

        private void SSList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";

            long nID = 0;
            string strSITE_ID = "";
            string strWORKER_ID = "";
            string strNAME = "";
            string strBIRTH = "";
            string strSEX = "";
            string strAGE = "";
            string strHISTORY = "";
            string strGBJEKHAP = "";
            string strJEKHAP = "";
            string strGBSAHU = "";
            string strSAHU = "";

            int intRowAffected = 0;
            DataTable dt = null;

            strSITE_ID = VB.Pstr(Send_Data, "{}", 1);
            CHART_DATE = VB.Pstr(Send_Data, "{}", 4);
            strWORKER_ID = VB.Pstr(Send_Data, "{}", 5);
            strNAME = SSList_Sheet1.Cells[0, 1].Text.Trim();
            strBIRTH = SSList_Sheet1.Cells[0, 4].Text.Trim();
            strSEX = SSList_Sheet1.Cells[0, 7].Text.Trim();
            strAGE = SSList_Sheet1.Cells[0, 9].Text.Trim();

            strHISTORY = SSList_Sheet1.Cells[2, 2].Text.Trim() + "{}";    //1.회사명1
            strHISTORY += SSList_Sheet1.Cells[2, 4].Text.Trim() + "{}";   //2.업무내용1
            strHISTORY += SSList_Sheet1.Cells[2, 10].Text.Trim() + "{}";  //3.유해인자1
            strHISTORY += SSList_Sheet1.Cells[2, 14].Text.Trim() + "{}";  //4.근무기간1
            strHISTORY += SSList_Sheet1.Cells[3, 2].Text.Trim() + "{}";   //5.회사명2
            strHISTORY += SSList_Sheet1.Cells[3, 4].Text.Trim() + "{}";   //6.업무내용2 
            strHISTORY += SSList_Sheet1.Cells[3, 10].Text.Trim() + "{}";  //7.유해인자2
            strHISTORY += SSList_Sheet1.Cells[3, 14].Text.Trim() + "{}";  //8.근무기간2
            strHISTORY += SSList_Sheet1.Cells[4, 2].Text.Trim() + "{}";   //9.회사명3
            strHISTORY += SSList_Sheet1.Cells[4, 4].Text.Trim() + "{}";   //10.업무내용3
            strHISTORY += SSList_Sheet1.Cells[4, 10].Text.Trim() + "{}";  //11.유해인자3
            strHISTORY += SSList_Sheet1.Cells[4, 14].Text.Trim() + "{}";  //12.근무기간3
            strHISTORY += SSList_Sheet1.Cells[5, 2].Text.Trim() + "{}";   //13.과거병력
            strHISTORY += SSList_Sheet1.Cells[7, 2].Text.Trim() + "{}";   //14.진단명1
            strHISTORY += SSList_Sheet1.Cells[7, 12].Text.Trim() + "{}";  //15.진단일1
            strHISTORY += SSList_Sheet1.Cells[8, 2].Text.Trim() + "{}";   //16.진단명2
            strHISTORY += SSList_Sheet1.Cells[8, 12].Text.Trim() + "{}";  //17.진단일2 
            strHISTORY += SSList_Sheet1.Cells[9, 4].Text.Trim() + "{}";   //18.현재작업(부서및공정명)
            strHISTORY += SSList_Sheet1.Cells[10, 4].Text.Trim() + "{}";  //19.현재작업(유해인자)
            strHISTORY += SSList_Sheet1.Cells[11, 2].Text.Trim() + "{}";  //20.현재작업(작업내용)
            strHISTORY += SSList_Sheet1.Cells[13, 2].Text.Trim() + "{}";  //21.소견

            strGBJEKHAP = "";
            if (SSList.ActiveSheet.Cells[14, 2].Text == "True") strGBJEKHAP = "가";
            if (SSList.ActiveSheet.Cells[14, 8].Text == "True") strGBJEKHAP = "나";
            if (SSList.ActiveSheet.Cells[15, 2].Text == "True") strGBJEKHAP = "다";
            if (SSList.ActiveSheet.Cells[15, 8].Text == "True") strGBJEKHAP = "라";
            strJEKHAP = SSList_Sheet1.Cells[16, 2].Text.Trim();

            strGBSAHU = "";
            if (SSList.ActiveSheet.Cells[17, 2].Text == "True") strGBSAHU += "1,";
            if (SSList.ActiveSheet.Cells[17, 2].Text == "True") strGBSAHU += "1,";
            if (SSList.ActiveSheet.Cells[17, 4].Text == "True") strGBSAHU += "2,";
            if (SSList.ActiveSheet.Cells[17, 6].Text == "True") strGBSAHU += "3,";
            if (SSList.ActiveSheet.Cells[17, 9].Text == "True") strGBSAHU += "4,";
            if (SSList.ActiveSheet.Cells[17, 11].Text == "True") strGBSAHU += "5,";
            if (SSList.ActiveSheet.Cells[17, 13].Text == "True") strGBSAHU += "6,";
            if (SSList.ActiveSheet.Cells[18, 2].Text == "True") strGBSAHU += "7,";
            if (SSList.ActiveSheet.Cells[18, 4].Text == "True") strGBSAHU += "8,";
            if (SSList.ActiveSheet.Cells[18, 6].Text == "True") strGBSAHU += "9,";

            strSAHU = SSList_Sheet1.Cells[19, 2].Text.Trim();

            if (strAGE == "") { ComFunc.MsgBox("나이가 공란입니다.", "오류"); return; }
            if (strGBJEKHAP == "") { ComFunc.MsgBox("업무적합성 구분을 선택 안함", "오류"); return; }
            if (strGBSAHU == "") { ComFunc.MsgBox("사후관리 구분을 선택 안함", "오류"); return; }

            if (OLD_ID == 0)
            {
                nID = GET_SeqNextVal("HC_OSHA_CARD_ID_SEQ");

                SQL = "INSERT INTO HIC_OSHA_WORKER_JINDAN (ID,SITE_ID,CHARTDATE,WORKER_ID,";
                SQL += " NAME,SEX,AGE,BIRTH,HISTORY,GBJEKHAP,JEKHAP,GBSAHU,SAHU,ISDELETED,";
                SQL += " MODIFIED,MODIFIEDUSER,CREATED,CREATEDUSER,SWLICENSE) VALUES (";
                SQL = SQL + nID + "," + strSITE_ID + ",'" + CHART_DATE + "','";
                SQL = SQL + strWORKER_ID + "','" + strNAME + "','" + strSEX + "'," + strAGE + ",'";
                SQL = SQL + strBIRTH + "','" + strHISTORY + "','" + strGBJEKHAP + "','" + strJEKHAP + "','";
                SQL = SQL + strGBSAHU + "','" + strSAHU + "','N',";
                SQL = SQL + "SYSDATE,'" + clsType.User.Sabun + "',SYSDATE,'" + clsType.User.Sabun;
                SQL = SQL + "','" + clsType.HosInfo.SwLicense + "') ";
            }
            else
            {
                SQL = "UPDATE HIC_OSHA_WORKER_JINDAN SET ";
                SQL += " NAME='" + strNAME + "',";
                SQL += " SEX='" + strSEX + "',";
                SQL += " AGE=" + strAGE + ",";
                SQL += " BIRTH='" + strBIRTH + "',";
                SQL += " HISTORY='" + strHISTORY + "',";
                SQL += " GBJEKHAP='" + strGBJEKHAP + "',";
                SQL += " JEKHAP='" + strJEKHAP + "',";
                SQL += " GBSAHU='" + strGBSAHU + "',";
                SQL += " SAHU='" + strSAHU + "',";
                SQL += " MODIFIED=SYSDATE,";
                SQL += " MODIFIEDUSER='" + clsType.User.Sabun + "' ";
                SQL += "WHERE ID=" + OLD_ID + " ";
            }
            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("자료 저장시 오류가 발생함", "알림");
                return;
            }

            if (OLD_ID == 0) OLD_ID = nID;

            BtnPrint.Enabled = true;
            btnPdf.Enabled = true;

            ComFunc.MsgBox("저장 완료", "알림");

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

        public void Set_Data(string strData)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            OLD_ID = 0;
            Send_Data = strData;

            string SITE_ID = VB.Pstr(Send_Data, "{}", 1);
            string SITE_NAME = VB.Pstr(Send_Data, "{}", 2);
            string ISDOCTOR = "N";
            CHART_DATE = VB.Pstr(Send_Data, "{}", 4);
            string worker_id = VB.Pstr(Send_Data, "{}", 5);
            string worker_name = VB.Pstr(Send_Data, "{}", 6);
            string worker_gender = VB.Pstr(Send_Data, "{}", 7);
            string worker_age = VB.Pstr(Send_Data, "{}", 8);
            if (VB.Pstr(Send_Data, "{}", 9) != "") ISDOCTOR = "Y";

            SSList_Sheet1.Cells[0, 1].Text = worker_name;
            SSList_Sheet1.Cells[0, 7].Text = worker_gender;
            SSList_Sheet1.Cells[0, 9].Text = worker_age;
            SSList_Sheet1.Cells[0, 12].Text = SITE_NAME;

            //생년월일을 찾아 표시함
            SQL = "SELECT JUMIN FROM HIC_SITE_WORKER ";
            SQL = SQL + ComNum.VBLF + " WHERE SWLicense='" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "   AND SITEID = '" + SITE_ID + "' ";
            SQL = SQL + ComNum.VBLF + "   AND ID='" + worker_id + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                SSList_Sheet1.Cells[0, 4].Text = dt.Rows[0]["JUMIN"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;

            //업무적합성평가 Display
            Data_Display(SITE_ID, worker_id, CHART_DATE);

            //사후관리소견서
            SSHealthCheck_Show(worker_id);

            if (ISDOCTOR == "Y") btnSave.Enabled = true;
            if (ISDOCTOR == "Y" && OLD_ID > 0) btnDelete.Enabled = true;
        }

        //업무적합성평가 Display
        void Data_Display(string site_id, string worker_id, string chartDate)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strHISTORY = "";
            string strGBJEKHAP = "";
            string strJEKHAP = "";
            string strGBSAHU = "";
            string strSAHU = "";

            OLD_ID = 0;
            BtnPrint.Enabled = false;
            btnPdf.Enabled = false;

            try
            {
                SQL = "SELECT * FROM HIC_OSHA_WORKER_JINDAN ";
                SQL = SQL + ComNum.VBLF + " WHERE SWLicense='" + clsType.HosInfo.SwLicense + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ISDELETED='N' ";
                SQL = SQL + ComNum.VBLF + "   AND SITE_ID=" + site_id + " ";
                SQL = SQL + ComNum.VBLF + "   AND WORKER_ID='" + worker_id + "' ";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE>='" + VB.Left(chartDate, 4) + "-01-01' ";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE<='" + VB.Left(chartDate, 4) + "-12-31' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0)
                {
                    OLD_ID = long.Parse(dt.Rows[0]["ID"].ToString().Trim());
                    CHART_DATE = dt.Rows[0]["CHARTDATE"].ToString().Trim();
                    SSList_Sheet1.Cells[0, 1].Text = dt.Rows[0]["NAME"].ToString().Trim();
                    SSList_Sheet1.Cells[0, 4].Text = dt.Rows[0]["BIRTH"].ToString().Trim();
                    SSList_Sheet1.Cells[0, 7].Text = dt.Rows[0]["SEX"].ToString().Trim();
                    SSList_Sheet1.Cells[0, 9].Text = dt.Rows[0]["AGE"].ToString().Trim();

                    strHISTORY = dt.Rows[0]["HISTORY"].ToString().Trim();
                    strGBJEKHAP = dt.Rows[0]["GBJEKHAP"].ToString().Trim();
                    strJEKHAP = dt.Rows[0]["JEKHAP"].ToString().Trim();
                    strGBSAHU = dt.Rows[0]["GBSAHU"].ToString().Trim();
                    strSAHU = dt.Rows[0]["SAHU"].ToString().Trim();

                    SSList_Sheet1.Cells[2, 2].Text = VB.Pstr(strHISTORY, "{}", 1);
                    SSList_Sheet1.Cells[2, 4].Text = VB.Pstr(strHISTORY, "{}", 2);
                    SSList_Sheet1.Cells[2, 10].Text = VB.Pstr(strHISTORY, "{}", 3);
                    SSList_Sheet1.Cells[2, 14].Text = VB.Pstr(strHISTORY, "{}", 4);
                    SSList_Sheet1.Cells[3, 2].Text = VB.Pstr(strHISTORY, "{}", 5);
                    SSList_Sheet1.Cells[3, 4].Text = VB.Pstr(strHISTORY, "{}", 6);
                    SSList_Sheet1.Cells[3, 10].Text = VB.Pstr(strHISTORY, "{}", 7);
                    SSList_Sheet1.Cells[3, 14].Text = VB.Pstr(strHISTORY, "{}", 8);
                    SSList_Sheet1.Cells[4, 2].Text = VB.Pstr(strHISTORY, "{}", 9);
                    SSList_Sheet1.Cells[4, 4].Text = VB.Pstr(strHISTORY, "{}", 10);
                    SSList_Sheet1.Cells[4, 10].Text = VB.Pstr(strHISTORY, "{}", 11);
                    SSList_Sheet1.Cells[4, 14].Text = VB.Pstr(strHISTORY, "{}", 12);

                    SSList_Sheet1.Cells[5, 2].Text = VB.Pstr(strHISTORY, "{}", 13);

                    SSList_Sheet1.Cells[7, 2].Text = VB.Pstr(strHISTORY, "{}", 14);
                    SSList_Sheet1.Cells[7, 12].Text = VB.Pstr(strHISTORY, "{}", 15);
                    SSList_Sheet1.Cells[8, 2].Text = VB.Pstr(strHISTORY, "{}", 16);
                    SSList_Sheet1.Cells[8, 12].Text = VB.Pstr(strHISTORY, "{}", 17);
                    SSList_Sheet1.Cells[9, 4].Text = VB.Pstr(strHISTORY, "{}", 18);
                    SSList_Sheet1.Cells[10, 4].Text = VB.Pstr(strHISTORY, "{}", 19);
                    SSList_Sheet1.Cells[11, 2].Text = VB.Pstr(strHISTORY, "{}", 20);

                    SSList_Sheet1.Cells[13, 2].Text = VB.Pstr(strHISTORY, "{}", 21);

                    if (strGBJEKHAP == "가") SSList.ActiveSheet.Cells[14, 2].Text = "True";
                    if (strGBJEKHAP == "나") SSList.ActiveSheet.Cells[14, 8].Text = "True";
                    if (strGBJEKHAP == "다") SSList.ActiveSheet.Cells[15, 2].Text = "True";
                    if (strGBJEKHAP == "라") SSList.ActiveSheet.Cells[15, 8].Text = "True";

                    SSList_Sheet1.Cells[16, 2].Text = strJEKHAP;

                    if (VB.InStr(strGBSAHU, "1,") > 0) SSList.ActiveSheet.Cells[17, 2].Text = "True";
                    if (VB.InStr(strGBSAHU, "2,") > 0) SSList.ActiveSheet.Cells[17, 4].Text = "True";
                    if (VB.InStr(strGBSAHU, "3,") > 0) SSList.ActiveSheet.Cells[17, 6].Text = "True";
                    if (VB.InStr(strGBSAHU, "4,") > 0) SSList.ActiveSheet.Cells[17, 9].Text = "True";
                    if (VB.InStr(strGBSAHU, "5,") > 0) SSList.ActiveSheet.Cells[17, 11].Text = "True";
                    if (VB.InStr(strGBSAHU, "6,") > 0) SSList.ActiveSheet.Cells[17, 13].Text = "True";
                    if (VB.InStr(strGBSAHU, "7,") > 0) SSList.ActiveSheet.Cells[18, 2].Text = "True";
                    if (VB.InStr(strGBSAHU, "8,") > 0) SSList.ActiveSheet.Cells[18, 4].Text = "True";
                    if (VB.InStr(strGBSAHU, "9,") > 0) SSList.ActiveSheet.Cells[18, 6].Text = "True";
                    SSList_Sheet1.Cells[19, 2].Text = strSAHU;

                    BtnPrint.Enabled = true;
                    btnPdf.Enabled = true;
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                ComFunc.MsgBox(ex.Message);
            }
        }

        //질병유소견자 
        private void SSHealthCheck_Show(string strWorkId)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int i = 0;

            string strGong = "";
            string strGunsok = "";
            string strYuhe = "";
            string strSogen = "";
            string strSahu = "";
            string strUpmu = "";
            int nYear = 0;
            int nGYear = Int32.Parse(VB.Left(VB.Pstr(Send_Data, "{}", 4), 4)) - 1;

            SSHealthCheck_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "SELECT GONGJENG,NAME,SEX,AGE,GUNSOK,YUHE,GGUBUN,SOGEN,SAHU,";
                SQL = SQL + ComNum.VBLF + " UPMU,YEAR,JINDATE,JIPYO  ";
                SQL = SQL + ComNum.VBLF + "  FROM HIC_LTD_RESULT3 ";
                SQL = SQL + ComNum.VBLF + " WHERE SWLicense='" + clsType.HosInfo.SwLicense + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ID='" + strWorkId + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY YEAR DESC,JINDATE DESC,JONG ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0)
                {
                    SSHealthCheck_Sheet1.RowCount = dt.Rows.Count;
                    SSHealthCheck_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SSHealthCheck_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GONGJENG"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 1].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 3].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 4].Text = dt.Rows[i]["GUNSOK"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 5].Text = dt.Rows[i]["YUHE"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 6].Text = dt.Rows[i]["JIPYO"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 7].Text = dt.Rows[i]["GGUBUN"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 8].Text = dt.Rows[i]["SOGEN"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 9].Text = dt.Rows[i]["SAHU"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 10].Text = dt.Rows[i]["UPMU"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 11].Text = dt.Rows[i]["YEAR"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 12].Text = VB.Right(dt.Rows[i]["JINDATE"].ToString().Trim(), 5);
                        SSHealthCheck_Sheet1.Rows[i].Height = SSHealthCheck_Sheet1.Rows[i].GetPreferredHeight();

                        if (OLD_ID == 0)
                        {
                            nYear = Int32.Parse(dt.Rows[i]["YEAR"].ToString().Trim());
                            if (nYear >= nGYear)
                            {
                                if (strGong == "") strGong = dt.Rows[i]["GONGJENG"].ToString().Trim();
                                if (strGunsok == "") strGunsok = dt.Rows[i]["GUNSOK"].ToString().Trim();
                                if (strYuhe == "") strYuhe = dt.Rows[i]["YUHE"].ToString().Trim();
                                if (strUpmu == "") strUpmu = dt.Rows[i]["UPMU"].ToString().Trim();
                                //if (dt.Rows[i]["SOGEN"].ToString().Trim()!="정상") strSogen += dt.Rows[i]["SOGEN"].ToString().Trim() + ",";
                                if (dt.Rows[i]["SAHU"].ToString().Trim() != "필요없음") strSahu += dt.Rows[i]["SAHU"].ToString().Trim() + ",";
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                if (OLD_ID == 0)
                {
                    strSogen = VB.Replace(strSogen, "\r\n", " ");
                    strSahu = VB.Replace(strSahu, "\r\n", " ");
                    SSList_Sheet1.Cells[2, 2].Text = VB.Pstr(Send_Data, "{}", 2);
                    SSList_Sheet1.Cells[2, 4].Text = strGong;
                    SSList_Sheet1.Cells[2, 14].Text = strGunsok;
                    SSList_Sheet1.Cells[9, 4].Text = strGong;
                    SSList_Sheet1.Cells[10, 4].Text = strYuhe;
                    //SSList_Sheet1.Cells[13, 2].Text = strSogen;
                    //SSList_Sheet1.Cells[17, 2].Text = strSahu;
                    //업무적합성 구분
                    if (strUpmu == "가") SSList.ActiveSheet.Cells[14, 2].Text = "True";
                    if (strUpmu == "나") SSList.ActiveSheet.Cells[14, 8].Text = "True";
                    if (strUpmu == "다") SSList.ActiveSheet.Cells[15, 2].Text = "True";
                    if (strUpmu == "라") SSList.ActiveSheet.Cells[15, 8].Text = "True";
                    //사후관리
                    if (VB.InStr(strSahu, "건강상담") > 0) SSList.ActiveSheet.Cells[17, 4].Text = "True";
                    if (VB.InStr(strSahu, "보호구") > 0) SSList.ActiveSheet.Cells[17, 6].Text = "True";
                    if (VB.InStr(strSahu, "추적") > 0) SSList.ActiveSheet.Cells[17, 9].Text = "True";
                    if (VB.InStr(strSahu, "치료") > 0) SSList.ActiveSheet.Cells[17, 11].Text = "True";
                    if (VB.InStr(strSahu, "시간단축") > 0) SSList.ActiveSheet.Cells[17, 13].Text = "True";
                    if (VB.InStr(strSahu, "작업전환") > 0) SSList.ActiveSheet.Cells[18, 2].Text = "True";
                    if (VB.InStr(strSahu, "근로금지") > 0) SSList.ActiveSheet.Cells[18, 4].Text = "True";
                    if (VB.InStr(strSahu, "직업병") > 0) SSList.ActiveSheet.Cells[18, 6].Text = "True";
                }

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;

            SQL = "DELETE FROM HIC_OSHA_WORKER_JINDAN ";
            SQL += "WHERE ID=" + OLD_ID + " ";
            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("자료 삭제시 오류가 발생함", "알림");
                return;
            }

            OLD_ID = 0;

            BtnPrint.Enabled = false;
            btnPdf.Enabled = false;

            ComFunc.MsgBox("삭제 완료", "알림");
        }

        private void FrmWorkerJiindan_Load(object sender, EventArgs e)
        {

        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            SSPrint_Load();

            SpreadPrint sp = new SpreadPrint(SSPrint, PrintStyle.STANDARD_REPORT);
            sp.Execute();

        }

        private void SSPrint_Load()
        {
            int nCol = 0;
            int nRow = 0;
            string strData = "";
            string strChartDate = "";
            string strJekhap1 = "";
            string strJekhap2 = "";
            string strSahu1 = "";
            string strSahu2 = "";

            string fileName = @"C:\HealthSoft\엑셀서식\업무적합성평가양식.xlsx";
            SSPrint.ActiveSheet.OpenExcel(fileName, 0);
            Thread.Sleep(500);

            strChartDate = VB.Pstr(CHART_DATE, "-", 1) + "년  " + VB.Pstr(CHART_DATE, "-", 2) + "월  ";
            strChartDate += VB.Pstr(CHART_DATE, "-", 3) + "일";

            strJekhap1 = "";
            strJekhap2 = "";
            if (SSList.ActiveSheet.Cells[14, 2].Text == "True")
            {
                strJekhap1 = "  ■ 현재 조건하에서 현재업무 가능  □ 일정 조건하에서 현재 업무 가능";
                strJekhap2 = "  □ 한시적으로 현재 업무 불가      □ 영구적으로 현재업무 불가";
            }
            else if (SSList.ActiveSheet.Cells[14, 8].Text == "True")
            {
                strJekhap1 = "  □ 현재 조건하에서 현재업무 가능  ■ 일정 조건하에서 현재 업무 가능";
                strJekhap2 = "  □ 한시적으로 현재 업무 불가      □ 영구적으로 현재업무 불가";

            }
            else if (SSList.ActiveSheet.Cells[15, 2].Text == "True")
            {
                strJekhap1 = "  □ 현재 조건하에서 현재업무 가능  □ 일정 조건하에서 현재 업무 가능";
                strJekhap2 = "  ■ 한시적으로 현재 업무 불가      □ 영구적으로 현재업무 불가";
            }
            else if (SSList.ActiveSheet.Cells[15, 8].Text == "True")
            {
                strJekhap1 = "  □ 현재 조건하에서 현재업무 가능  □ 일정 조건하에서 현재 업무 가능";
                strJekhap2 = "  □ 한시적으로 현재 업무 불가      ■ 영구적으로 현재업무 불가";
            }

            strSahu1 = "";
            strSahu2 = "";

            if (SSList.ActiveSheet.Cells[17, 2].Text == "True")
            {
                strSahu1 = "   ■ 필요없음  ";
            }
            else
            {
                strSahu1 = "   □ 필요없음  ";
            }

            if (SSList.ActiveSheet.Cells[17, 4].Text == "True")
            {
                strSahu1 += "   ■ 건강상담  ";
            }
            else
            {
                strSahu1 += "   □ 건강상담  ";
            }

            if (SSList.ActiveSheet.Cells[17, 6].Text == "True")
            {
                strSahu1 += "   ■ 보호구지급 및 착용지도  ";
            }
            else
            {
                strSahu1 += "   □ 보호구지급 및 착용지도  ";
            }

            if (SSList.ActiveSheet.Cells[17, 9].Text == "True")
            {
                strSahu1 += "   ■ 추적검사  ";
            }
            else
            {
                strSahu1 += "   □ 추적검사  ";
            }

            if (SSList.ActiveSheet.Cells[17, 11].Text == "True")
            {
                strSahu1 += "   ■ 근무 중 치료  ";
            }
            else
            {
                strSahu1 += "   □ 근무 중 치료  ";
            }

            if (SSList.ActiveSheet.Cells[17, 13].Text == "True")
            {
                strSahu2 += "   ■ 근로시간단축  ";
            }
            else
            {
                strSahu2 += "   □ 근로시간단축  ";
            }

            if (SSList.ActiveSheet.Cells[18, 2].Text == "True")
            {
                strSahu2 += "   ■ 작업전환  ";
            }
            else
            {
                strSahu2 += "   □ 작업전환  ";
            }

            if (SSList.ActiveSheet.Cells[18, 4].Text == "True")
            {
                strSahu2 += "   ■ 근로금지 및 제한  ";
            }
            else
            {
                strSahu2 += "   □ 근로금지 및 제한  ";
            }

            if (SSList.ActiveSheet.Cells[18, 6].Text == "True")
            {
                strSahu2 += "   ■ 직업병확진의뢰 안내  ";
            }
            else
            {
                strSahu2 += "   □ 직업병확진의뢰 안내  ";
            }

            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    strData = SSPrint.ActiveSheet.Cells[i, j].Text.Trim();
                    if (VB.Left(strData, 1) == "{")
                    {
                        if (VB.InStr(strData, ",") > 0)
                        {
                            nRow = Int32.Parse(VB.Pstr(VB.Pstr(strData, ",", 1), "{", 2));
                            nCol = Int32.Parse(VB.Pstr(VB.Pstr(strData, ",", 2), "}", 1));
                            SSPrint.ActiveSheet.Cells[i, j].Text = SSList.ActiveSheet.Cells[nRow, nCol].Text;
                        }
                        else
                        {
                            if (strData == "{적합1}") SSPrint.ActiveSheet.Cells[i, j].Text = strJekhap1;
                            if (strData == "{적합2}") SSPrint.ActiveSheet.Cells[i, j].Text = strJekhap2;
                            if (strData == "{사후1}") SSPrint.ActiveSheet.Cells[i, j].Text = strSahu1;
                            if (strData == "{사후2}") SSPrint.ActiveSheet.Cells[i, j].Text = strSahu2;
                            if (strData == "{발급일}") SSPrint.ActiveSheet.Cells[i, j].Text = strChartDate;
                        }
                    }
                }
            }
            Thread.Sleep(500);

        }

        private void btnMail_Click(object sender, EventArgs e)
        {
            string SITE_ID = VB.Pstr(Send_Data, "{}", 1);
            string SITE_NAME = VB.Pstr(Send_Data, "{}", 2);
            string strNAME = SSList_Sheet1.Cells[0, 1].Text.Trim();
            long siteid = 0;

            try
            {
                SSPrint_Load();

                Cursor.Current = Cursors.WaitCursor;
                string title = "업무적합성평가_" + SITE_NAME + "_" + strNAME;
                string pdfFileName = @"C:\\HealthSoft\pdfprint" + "\\" + title + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                SpreadPrint print = new SpreadPrint(SSPrint, PrintStyle.STANDARD_APPROVAL, false);
                print.ExportPDFNoWait(pdfFileName, SSPrint.ActiveSheet);
                Thread.Sleep(2000);

                HcCodeService codeService = new HcCodeService();
                HC_CODE sendMailAddress = codeService.FindActiveCodeByGroupAndCode("OSHA_MANAGER", "mail", "OSHA");

                EstimateMailForm form = new EstimateMailForm();
                siteid = long.Parse(SITE_ID);
                form.set_SiteId(siteid);
                form.GetMailForm().SenderMailAddress = sendMailAddress.CodeName;
                form.GetMailForm().AttachmentsList.Add(pdfFileName);

                string strMailList = GetEmail(SITE_ID);
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

        private string GetEmail(string site_id)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            string strEMail = "";
            string strList = "";
            string email = string.Empty;
            string strNow = DateTime.Now.ToString("yyyy-MM-dd");

            SQL = "";
            SQL = "SELECT NAME,EMAIL FROM HIC_OSHA_CONTRACT_MANAGER ";
            SQL = SQL + ComNum.VBLF + "WHERE ESTIMATE_ID IN (SELECT ESTIMATE_ID FROM HIC_OSHA_CONTRACT ";
            SQL = SQL + ComNum.VBLF + "      WHERE OSHA_SITE_ID=" + site_id + " ";
            SQL = SQL + ComNum.VBLF + "        AND CONTRACTSTARTDATE<='" + strNow + "' ";
            SQL = SQL + ComNum.VBLF + "        AND CONTRACTENDDATE>='" + strNow + "' ";
            SQL = SQL + ComNum.VBLF + "        AND ISDELETED='N') ";
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

        private void btnPdf_Click(object sender, EventArgs e)
        {
            string SITE_ID = VB.Pstr(Send_Data, "{}", 1);
            string SITE_NAME = VB.Pstr(Send_Data, "{}", 2);
            string strNAME = SSList_Sheet1.Cells[0, 1].Text.Trim();

            SSPrint_Load();

            Cursor.Current = Cursors.WaitCursor;
            string title = "업무적합성평가_" + SITE_NAME + "_" + strNAME;
            string pdfFileName = @"C:\Temp\" + title + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
            SpreadPrint print = new SpreadPrint(SSPrint, PrintStyle.STANDARD_APPROVAL, false);
            print.ExportPDFNoWait(pdfFileName, SSPrint.ActiveSheet);
            Thread.Sleep(2000);

            ComFunc.MsgBox("Temp 폴더에 저장되었습니다.", "알림");
        }
    }
}
