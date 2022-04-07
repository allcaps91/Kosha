using ComBase;
using ComDbB;
using ComHpcLibB;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using HC_Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HC_OSHA.StatusReport
{
    public partial class FrmWorkerJiindan : Form
    {
        private string Send_Data = "";
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
            string strChartDate = "";
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
            strChartDate = VB.Pstr(Send_Data, "{}", 4);
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

            strGBJEKHAP = SSList_Sheet1.Cells[14, 4].Text.Trim();
            strJEKHAP = SSList_Sheet1.Cells[15, 2].Text.Trim();

            strGBSAHU = SSList_Sheet1.Cells[16, 4].Text.Trim();
            strSAHU = SSList_Sheet1.Cells[17, 2].Text.Trim();

            if (strAGE=="") { ComFunc.MsgBox("나이가 공란입니다.", "오류"); return; }

            if (OLD_ID == 0)
            {
                nID = GET_SeqNextVal("HC_OSHA_CARD_ID_SEQ");

                SQL = "INSERT INTO HIC_OSHA_WORKER_JINDAN (ID,SITE_ID,CHARTDATE,WORKER_ID,";
                SQL += " NAME,SEX,AGE,BIRTH,HISTORY,GBJEKHAP,JEKHAP,GBSAHU,SAHU,ISDELETED,";
                SQL += " MODIFIED,MODIFIEDUSER,CREATED,CREATEDUSER,SWLICENSE) VALUES (";
                SQL = SQL + nID + "," + strSITE_ID + ",'" + strChartDate + "','";
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
            btnExcel.Enabled = true;

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
            string strChartDate = VB.Pstr(Send_Data, "{}", 4);
            string worker_id = VB.Pstr(Send_Data, "{}", 5);
            string worker_name = VB.Pstr(Send_Data, "{}", 6);
            string worker_gender = VB.Pstr(Send_Data, "{}", 7);
            string worker_age = VB.Pstr(Send_Data, "{}", 8);
            if (VB.Pstr(Send_Data, "{}", 9)!="") ISDOCTOR = "Y";

            SSList_Sheet1.Cells[0, 1].Text = worker_name;
            SSList_Sheet1.Cells[0, 7].Text = worker_gender;
            SSList_Sheet1.Cells[0, 9].Text = worker_age;
            SSList_Sheet1.Cells[0, 13].Text = SITE_NAME;

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
            Data_Display(SITE_ID, worker_id, strChartDate);

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
            btnExcel.Enabled = false;

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

                    SSList_Sheet1.Cells[14, 4].Text = strGBJEKHAP;
                    SSList_Sheet1.Cells[15, 2].Text = strJEKHAP;

                    SSList_Sheet1.Cells[16, 4].Text = strGBSAHU;
                    SSList_Sheet1.Cells[17, 2].Text = strSAHU;

                    BtnPrint.Enabled = true;
                    btnExcel.Enabled = true;
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

                        if (OLD_ID==0)
                        {
                            nYear = Int32.Parse(dt.Rows[i]["YEAR"].ToString().Trim());
                            if (nYear >= nGYear)
                            {
                                if (strGong == "") strGong = dt.Rows[i]["GONGJENG"].ToString().Trim();
                                if (strGunsok == "") strGunsok = dt.Rows[i]["GUNSOK"].ToString().Trim();
                                if (strYuhe == "") strYuhe = dt.Rows[i]["YUHE"].ToString().Trim();
                                if (strUpmu == "") strUpmu = dt.Rows[i]["UPMU"].ToString().Trim();
                                if (dt.Rows[i]["SOGEN"].ToString().Trim()!="정상") strSogen += dt.Rows[i]["SOGEN"].ToString().Trim() + ",";
                                if (dt.Rows[i]["SAHU"].ToString().Trim()!="필요없음") strSahu += dt.Rows[i]["SAHU"].ToString().Trim() + ",";
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
                    SSList_Sheet1.Cells[13, 2].Text = strSogen;
                    SSList_Sheet1.Cells[14, 4].Text = strUpmu;
                    SSList_Sheet1.Cells[17, 2].Text = strSahu;
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
            btnExcel.Enabled = false;

            ComFunc.MsgBox("삭제 완료", "알림");
        }
    }
}
