using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;

namespace ComEmrBase
{
    public partial class frmActingRecord : Form
    {
        private string mstrWardCode;

        public frmActingRecord()
        {
            InitializeComponent();
        }

        private void frmActingRecord_Load(object sender, EventArgs e)
        {
            if (VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "") != "")
            {
                mstrWardCode = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "");
            }

            clsVbfunc.SetWardCodeCombo(clsDB.DbCon, cboWard, "1", true, 2);

            cboWard.Text = mstrWardCode;

            cboJob.Items.Clear();
            cboJob.Items.Add("1.재원자명단");
            cboJob.Items.Add("2.당일입원자");
            cboJob.Items.Add("3.퇴원예고자");
            cboJob.Items.Add("4.당일퇴원자");
            cboJob.Items.Add("5.중증도미분류");
            cboJob.Items.Add("6.수술예정자");
            cboJob.Items.Add("7.진단명 누락자");
            cboJob.Items.Add("A.응급실경유입원(1-3일전)");
            cboJob.Items.Add("B.재원기간 7-14일 환자");
            cboJob.Items.Add("C.재원기간 3-7일 환자");
            cboJob.Items.Add("D.어제퇴원자");
            cboJob.SelectedIndex = 0;

            lblName.Text = "";

            btnSearch.PerformClick();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            string strJob = "";
            string strPriDate = "";
            string strToDate = "";
            string strNextDate = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strJob = VB.Left(cboJob.Text, 1);

            strPriDate = DateTime.Now.AddDays(-1).ToShortDateString();
            strToDate = DateTime.Now.ToShortDateString();
            strNextDate = DateTime.Now.AddDays(-1).ToShortDateString();

            SQL = "SELECT M.WardCode,M.RoomCode,M.Pano,M.SName,M.Sex,M.Age,M.Bi,M.PName,";
            SQL = SQL + ComNum.VBLF + " TO_CHAR(M.InDate,'YYYY-MM-DD') InDate,M.Ilsu,M.IpdNo,M.GbSts,";
            SQL = SQL + ComNum.VBLF + " TO_CHAR(M.OutDate,'YYYY-MM-DD') OutDate,";
            SQL = SQL + ComNum.VBLF + " M.DeptCode,M.DrCode,D.DrName,M.AmSet1,M.AmSet4,M.AmSet6,M.AmSet7 ";
            SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_PMPA.IPD_NEW_MASTER  M, ";
            SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_PATIENT P, ";
            SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_DOCTOR  D ";
            switch (cboWard.Text)
            {
                case "전체": SQL = SQL + ComNum.VBLF + "WHERE M.WardCode>' ' "; break;
                case "MICU": SQL = SQL + ComNum.VBLF + "WHERE M.RoomCode='234' "; break;
                case "SICU": SQL = SQL + ComNum.VBLF + "WHERE M.RoomCode='233' "; break;
                case "ND":
                case "NR": SQL = SQL + ComNum.VBLF + "WHERE M.WardCode IN ('ND','IQ','NR') "; break;
                default: SQL = SQL + ComNum.VBLF + "WHERE M.WardCode='" + VB.Trim(cboWard.Text) + "' "; break;
            }
            //If GnJobSabun <> 4349 Then SQL = SQL + ComNum.VBLF + "  AND M.Pano<>'81000004' "

            //작업분류
            if (strJob == "1") //재원자
            {
                SQL = SQL + ComNum.VBLF + " AND (M.OutDate IS NULL OR M.OutDate>=TO_DATE('" + strToDate + "','YYYY-MM-DD')) ";
                SQL = SQL + ComNum.VBLF + " AND M.IpwonTime < TO_DATE('" + strNextDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND M.Pano < '90000000' ";
                //''SQL = SQL + ComNum.VBLF + " AND M.Pano <> '81000004' "
                SQL = SQL + ComNum.VBLF + " AND M.GbSTS <> '9' ";
            }
            else if (strJob == "2") //당일입원자
            {
                SQL = SQL + ComNum.VBLF + "  AND M.InDate >= TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND M.IpwonTime >= TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND M.IpwonTime < TO_DATE('" + strNextDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND M.Pano < '90000000' ";
                SQL = SQL + ComNum.VBLF + "  AND M.Pano <> '81000004' ";
                SQL = SQL + ComNum.VBLF + "  AND M.GbSTS <> '9' ";
            }
            else if (strJob == "3") //퇴원예고
            {
                SQL = SQL + ComNum.VBLF + " AND (M.ActDate IS NULL OR M.ActDate>=TRUNC(SYSDATE)) ";
                SQL = SQL + ComNum.VBLF + " AND M.GbSts NOT IN ('7','9') ";
                SQL = SQL + ComNum.VBLF + " AND M.OutDate = TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND (M.ROutDate IS NULL OR M.ROutDate>=TRUNC(SYSDATE) ) ";
            }
            else if (strJob == "4")  //당일퇴원
            {
                SQL = SQL + ComNum.VBLF + " AND M.OutDate=TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + " AND M.GbSTS = '7' "; //퇴원수납완료
            }
            else if (strJob == "6") //수술예정자
            {
                SQL = SQL + ComNum.VBLF + " AND M.ActDate IS NULL ";
                SQL = SQL + ComNum.VBLF + " AND M.GbSts IN ('0','2')  ";
                SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
            }
            else if (strJob == "A") //응급실경유입원 1-3일전
            {
                SQL = SQL + ComNum.VBLF + " AND (M.ActDate IS NULL OR M.ActDate=TRUNC(SYSDATE)) ";
                SQL = SQL + ComNum.VBLF + " AND (M.Ilsu >= 1 AND M.Ilsu<=3) ";
                SQL = SQL + ComNum.VBLF + " AND M.AmSet7 IN ('3','4','5') ";
                SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                SQL = SQL + ComNum.VBLF + " AND M.ROutDate>=TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + " AND M.GbSTS <> '9' ";
            }
            else if (strJob == "B") //재원기간 7-14일 환자
            {
                SQL = SQL + ComNum.VBLF + " AND (M.ActDate IS NULL OR M.ActDate=TRUNC(SYSDATE)) ";
                SQL = SQL + ComNum.VBLF + " AND (M.Ilsu>=7 AND M.Ilsu<=14) ";
                SQL = SQL + ComNum.VBLF + " AND M.GbSTS <> '9' ";
            }
            else if (strJob == "C") //재원기간 3-7일 환자
            {
                SQL = SQL + ComNum.VBLF + " AND (M.ActDate IS NULL OR M.ActDate=TRUNC(SYSDATE)) ";
                SQL = SQL + ComNum.VBLF + " AND (M.Ilsu>=3 AND M.Ilsu<=7) ";
                SQL = SQL + ComNum.VBLF + " AND M.GbSTS <> '9' ";
            }
            else if (strJob == "D") //어제퇴원자
            {
                SQL = SQL + ComNum.VBLF + " AND M.OutDate=TRUNC(SYSDATE-1) ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " AND M.GbSts IN ('0','2')  ";
            }
            SQL = SQL + ComNum.VBLF + "  AND M.Pano=P.Pano(+) ";
            SQL = SQL + ComNum.VBLF + "  AND M.DrCode=D.DrCode(+) ";
            //SORT
            SQL = SQL + ComNum.VBLF + "ORDER BY M.RoomCode,M.SName, M.Indate DESC  ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            ssList.ActiveSheet.Rows.Count = 0;
            ssList.ActiveSheet.Rows.Count = dt.Rows.Count;
            ssList.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssList.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["RoomCode"].ToString();
                ssList.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString();
                ssList.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["Sname"].ToString();
                ssList.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["Age"].ToString();
                ssList.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["Sex"].ToString();
                ssList.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["DrName"].ToString();
            }

            dt.Dispose();
            dt = null;
        }

        private void btnActing_Click(object sender, EventArgs e)
        {
            if (!(ssList.ActiveSheet.ActiveRow.Index >= 0))
            {
                return;
            }

            string strPtno = "";
            string strName = "";
            string strBun = "";

            strPtno = ssList.ActiveSheet.Cells[ssList.ActiveSheet.ActiveRowIndex, 1].Text;
            strName = ssList.ActiveSheet.Cells[ssList.ActiveSheet.ActiveRowIndex, 2].Text;

            lblName.Text = strPtno + " " + strName;
            strBun = "";

            if (chkBun0.Checked == true) { strBun = "'11','12'"; }
            if (chkBun0.Checked == true) { strBun = strBun + (strBun == "" ? "" : ",") + "'20'"; }

            Read_Ocs_Iorder_act1(strPtno, strBun);
            ssActing.Enabled = true;
        }

        private void Read_Ocs_Iorder_act1(string argPtno, string argBun)
        {
            string sPtnoOld = "";
            string sChartDateOld = "";
            string sCodeOld = "";

            string sPtnoNew = "";
            string sChartDateNew = "";
            string sCodeNew = "";

            string sORDNAME = "";
            string sORDSUB = "";
            string sORDERNAME = "";

            string sACTTIME1 = "";
            string sFINDACT = "";
            string sACTNAME = "";

            string sDOSNAME = "";
            string sGBDiV = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = " ";
            SQL = SQL + ComNum.VBLF + "  SELECT M.Pano, O.OrderCode, ";
            SQL = SQL + ComNum.VBLF + "         C.OrderNames AS ORDNAME, C.OrderName AS ORDSUB, O.Contents,  ";
            SQL = SQL + ComNum.VBLF + "         O.BContents,  O.RealQty,    O.DosCode,    D.DosName,   D.GBDIV, ";
            SQL = SQL + ComNum.VBLF + "         O.ACTSABUN, T.NAME AS ACTUSER,  TO_CHAR(O.ACTTIME,'YYYY-MM-DD') AS ACTDATE1,";
            SQL = SQL + ComNum.VBLF + "         TO_CHAR(O.ACTTIME,'HH24MI') AS ACTTIME1,";
            SQL = SQL + ComNum.VBLF + "         CASE WHEN (TO_CHAR(O.ACTTIME,'HH24MI') >= '0000' AND TO_CHAR(O.ACTTIME,'HH24MI') < '0100') THEN '00'";
            SQL = SQL + ComNum.VBLF + "             WHEN (TO_CHAR(O.ACTTIME,'HH24MI') >= '0100' AND TO_CHAR(O.ACTTIME,'HH24MI') < '0200') THEN '01'";
            SQL = SQL + ComNum.VBLF + "             WHEN (TO_CHAR(O.ACTTIME,'HH24MI') >= '0200' AND TO_CHAR(O.ACTTIME,'HH24MI') < '0300') THEN '02'";
            SQL = SQL + ComNum.VBLF + "             WHEN (TO_CHAR(O.ACTTIME,'HH24MI') >= '0300' AND TO_CHAR(O.ACTTIME,'HH24MI') < '0400') THEN '03'";
            SQL = SQL + ComNum.VBLF + "             WHEN (TO_CHAR(O.ACTTIME,'HH24MI') >= '0400' AND TO_CHAR(O.ACTTIME,'HH24MI') < '0500') THEN '04'";
            SQL = SQL + ComNum.VBLF + "             WHEN (TO_CHAR(O.ACTTIME,'HH24MI') >= '0500' AND TO_CHAR(O.ACTTIME,'HH24MI') < '0600') THEN '05'";
            SQL = SQL + ComNum.VBLF + "             WHEN (TO_CHAR(O.ACTTIME,'HH24MI') >= '0600' AND TO_CHAR(O.ACTTIME,'HH24MI') < '0700') THEN '06'";
            SQL = SQL + ComNum.VBLF + "             WHEN (TO_CHAR(O.ACTTIME,'HH24MI') >= '0700' AND TO_CHAR(O.ACTTIME,'HH24MI') < '0800') THEN '07'";
            SQL = SQL + ComNum.VBLF + "             WHEN (TO_CHAR(O.ACTTIME,'HH24MI') >= '0800' AND TO_CHAR(O.ACTTIME,'HH24MI') < '0900') THEN '08'";
            SQL = SQL + ComNum.VBLF + "             WHEN (TO_CHAR(O.ACTTIME,'HH24MI') >= '0900' AND TO_CHAR(O.ACTTIME,'HH24MI') < '1000') THEN '09'";
            SQL = SQL + ComNum.VBLF + "             WHEN (TO_CHAR(O.ACTTIME,'HH24MI') >= '1000' AND TO_CHAR(O.ACTTIME,'HH24MI') < '1100') THEN '10'";
            SQL = SQL + ComNum.VBLF + "             WHEN (TO_CHAR(O.ACTTIME,'HH24MI') >= '1100' AND TO_CHAR(O.ACTTIME,'HH24MI') < '1200') THEN '11'";
            SQL = SQL + ComNum.VBLF + "             WHEN (TO_CHAR(O.ACTTIME,'HH24MI') >= '1200' AND TO_CHAR(O.ACTTIME,'HH24MI') < '1300') THEN '12'";
            SQL = SQL + ComNum.VBLF + "             WHEN (TO_CHAR(O.ACTTIME,'HH24MI') >= '1300' AND TO_CHAR(O.ACTTIME,'HH24MI') < '1400') THEN '13'";
            SQL = SQL + ComNum.VBLF + "             WHEN (TO_CHAR(O.ACTTIME,'HH24MI') >= '1400' AND TO_CHAR(O.ACTTIME,'HH24MI') < '1500') THEN '14'";
            SQL = SQL + ComNum.VBLF + "             WHEN (TO_CHAR(O.ACTTIME,'HH24MI') >= '1500' AND TO_CHAR(O.ACTTIME,'HH24MI') < '1600') THEN '15'";
            SQL = SQL + ComNum.VBLF + "             WHEN (TO_CHAR(O.ACTTIME,'HH24MI') >= '1600' AND TO_CHAR(O.ACTTIME,'HH24MI') < '1700') THEN '16'";
            SQL = SQL + ComNum.VBLF + "             WHEN (TO_CHAR(O.ACTTIME,'HH24MI') >= '1700' AND TO_CHAR(O.ACTTIME,'HH24MI') < '1800') THEN '17'";
            SQL = SQL + ComNum.VBLF + "             WHEN (TO_CHAR(O.ACTTIME,'HH24MI') >= '1800' AND TO_CHAR(O.ACTTIME,'HH24MI') < '1900') THEN '18'";
            SQL = SQL + ComNum.VBLF + "             WHEN (TO_CHAR(O.ACTTIME,'HH24MI') >= '1900' AND TO_CHAR(O.ACTTIME,'HH24MI') < '2000') THEN '19'";
            SQL = SQL + ComNum.VBLF + "             WHEN (TO_CHAR(O.ACTTIME,'HH24MI') >= '2000' AND TO_CHAR(O.ACTTIME,'HH24MI') < '2100') THEN '20'";
            SQL = SQL + ComNum.VBLF + "             WHEN (TO_CHAR(O.ACTTIME,'HH24MI') >= '2100' AND TO_CHAR(O.ACTTIME,'HH24MI') < '2200') THEN '21'";
            SQL = SQL + ComNum.VBLF + "             WHEN (TO_CHAR(O.ACTTIME,'HH24MI') >= '2200' AND TO_CHAR(O.ACTTIME,'HH24MI') < '2300') THEN '22'";
            SQL = SQL + ComNum.VBLF + "             WHEN (TO_CHAR(O.ACTTIME,'HH24MI') >= '2300' AND TO_CHAR(O.ACTTIME,'HH24MI') < '2400') THEN '23'";
            SQL = SQL + ComNum.VBLF + "         END FINDACT";
            SQL = SQL + ComNum.VBLF + "  FROM   KOSMOS_OCS.OCS_IORDER_ACT O, ";
            SQL = SQL + ComNum.VBLF + "         KOSMOS_PMPA.IPD_NEW_MASTER  M,";
            SQL = SQL + ComNum.VBLF + "         KOSMOS_PMPA.BAS_PATIENT P, ";
            SQL = SQL + ComNum.VBLF + "         KOSMOS_OCS.OCS_ORDERCODE           C,";
            SQL = SQL + ComNum.VBLF + "         KOSMOS_OCS.OCS_ODOSAGE             D,";
            SQL = SQL + ComNum.VBLF + "         KOSMOS_OCS.OCS_DOCTOR              N,";
            SQL = SQL + ComNum.VBLF + "         KOSMOS_PMPA.BAS_SUN     S,";
            SQL = SQL + ComNum.VBLF + "         KOSMOS_PMPA.BAS_PASS    T";
            SQL = SQL + ComNum.VBLF + "  WHERE  O.BDate >= TO_DATE('" + dtpDate1.Text + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "  AND  O.BDate <= TO_DATE('" + dtpDate2.Text + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "  AND  O.Ptno = '" + argPtno + "' ";
            SQL = SQL + ComNum.VBLF + "  AND  O.Bun IN ( '11','12','20' ) ";
            SQL = SQL + ComNum.VBLF + "  AND  (O.GbPRN IN  NULL OR O.GbPRN <> 'P') ";
            SQL = SQL + ComNum.VBLF + "  AND    O.GbPRN <>'S' ";
            SQL = SQL + ComNum.VBLF + "  AND   (O.GbStatus    = ' ' OR O.GbStatus IS NULL)    ";
            SQL = SQL + ComNum.VBLF + "  AND   (O.GbStatus  <> 'D' AND O.GbStatus <> 'D-')    ";
            SQL = SQL + ComNum.VBLF + "  AND    O.Ptno       =  M.Pano           ";
            SQL = SQL + ComNum.VBLF + "  AND    M.GBSTS IN ('0','2')              ";
            SQL = SQL + ComNum.VBLF + "  AND    M.OUTDATE IS NULL";
            SQL = SQL + ComNum.VBLF + "  AND    O.Ptno       =  P.Pano(+)        ";
            SQL = SQL + ComNum.VBLF + "  AND    O.SlipNo     =  C.SlipNo(+)      ";
            SQL = SQL + ComNum.VBLF + "  AND    O.OrderCode  =  C.OrderCode(+)   ";
            SQL = SQL + ComNum.VBLF + "  AND    (C.SendDept  !=  'N' OR C.SendDept IS NULL)   ";
            SQL = SQL + ComNum.VBLF + "  AND    O.DosCode    =  D.DosCode(+)     ";
            SQL = SQL + ComNum.VBLF + "  AND    O.DRCODE      =  N.SABUN(+)      ";
            SQL = SQL + ComNum.VBLF + "  AND    O.SUCODE = S.SUNEXT(+) ";
            SQL = SQL + ComNum.VBLF + "  AND    TO_NUMBER(O.ACTSABUN) = T.IDNUMBER (+)";
            SQL = SQL + ComNum.VBLF + "  AND    T.PROGRAMID = '        '";
            SQL = SQL + ComNum.VBLF + "  ORDER  BY M.Pano, O.ACTDATE,  o.bun,  O.SlipNo, O.ORDERCODE";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            ssActing.ActiveSheet.Rows.Count = 0;
            int nRow = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sPtnoNew = dt.Rows[i]["Pano"].ToString().Trim();
                sChartDateNew = dt.Rows[i]["ACTDATE1"].ToString().Trim();
                sCodeNew = dt.Rows[i]["OrderCode"].ToString().Trim();
                sORDNAME = dt.Rows[i]["ORDNAME"].ToString().Trim();
                sORDSUB = dt.Rows[i]["ORDSUB"].ToString().Trim();
                sORDERNAME = sORDERNAME + ComNum.VBLF + "(" + sORDSUB + ")";

                sACTTIME1 = dt.Rows[i]["ACTTIME1"].ToString().Trim();
                sFINDACT = dt.Rows[i]["FINDACT"].ToString().Trim();
                sACTNAME = dt.Rows[i]["ACTUSER"].ToString().Trim();

                sDOSNAME = dt.Rows[i]["DOSNAME"].ToString().Trim();
                sGBDiV = dt.Rows[i]["GBDIV"].ToString().Trim();

                if (sPtnoNew != sPtnoOld)
                {
                    ssActing.ActiveSheet.Rows.Count += 1;
                    nRow = ssActing.ActiveSheet.Rows.Count - 1;
                    ssActing.ActiveSheet.Cells[nRow, 0].Text = sPtnoNew;
                    ssActing.ActiveSheet.Cells[nRow, 1].Text = sChartDateNew;
                    ssActing.ActiveSheet.Cells[nRow, 2].Text = sCodeNew;
                    ssActing.ActiveSheet.Cells[nRow, 3].Text = sORDERNAME;
                    ssActing.ActiveSheet.Cells[nRow, 4].Text = sDOSNAME;
                    ssActing.ActiveSheet.Cells[nRow, 5].Text = sGBDiV;

                    sPtnoOld = sPtnoNew;
                    sChartDateOld = sChartDateNew;
                    sCodeOld = sCodeNew;
                }
                else
                {
                    if (sChartDateNew != sChartDateOld)
                    {
                        ssActing.ActiveSheet.Rows.Count += 1;
                        nRow = ssActing.ActiveSheet.Rows.Count - 1;

                        ssActing.ActiveSheet.Cells[nRow, 0].Text = sPtnoNew;
                        ssActing.ActiveSheet.Cells[nRow, 1].Text = sChartDateNew;
                        ssActing.ActiveSheet.Cells[nRow, 2].Text = sCodeNew;
                        ssActing.ActiveSheet.Cells[nRow, 3].Text = sORDERNAME;
                        ssActing.ActiveSheet.Cells[nRow, 4].Text = sDOSNAME;
                        ssActing.ActiveSheet.Cells[nRow, 5].Text = sGBDiV;

                        sPtnoOld = sPtnoNew;
                        sChartDateOld = sChartDateNew;
                        sCodeOld = sCodeNew;
                    }
                    else
                    {
                        if (sCodeNew != sCodeOld)
                        {
                            ssActing.ActiveSheet.Rows.Count += 1;
                            nRow = ssActing.ActiveSheet.Rows.Count - 1;

                            ssActing.ActiveSheet.Cells[nRow, 0].Text = sPtnoNew;
                            ssActing.ActiveSheet.Cells[nRow, 1].Text = sChartDateNew;
                            ssActing.ActiveSheet.Cells[nRow, 2].Text = sCodeNew;
                            ssActing.ActiveSheet.Cells[nRow, 3].Text = sORDERNAME;
                            ssActing.ActiveSheet.Cells[nRow, 4].Text = sDOSNAME;
                            ssActing.ActiveSheet.Cells[nRow, 5].Text = sGBDiV;

                            sPtnoOld = sPtnoNew;
                            sChartDateOld = sChartDateNew;
                            sCodeOld = sCodeNew;
                        }
                    }
                }

                //해당 셀을 찾아서 시간/액팅자 표시
                if (ssActing.ActiveSheet.Cells[nRow, 6].Text != "")
                {
                    ssActing.ActiveSheet.Cells[nRow, 6].Text += " " + sACTNAME + "(" + sACTTIME1 + ")";
                }
                else
                {
                    ssActing.ActiveSheet.Cells[nRow, 6].Text += sACTNAME + "(" + sACTTIME1 + ")";
                }

                ssActing.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
            }

            dt.Dispose();
            dt = null;
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDate.Checked == true)
            {
                dtpDate1.Text = DateTime.Now.ToShortDateString();
                dtpDate1.Visible = true;
            }
            else
            {
                dtpDate1.Visible = false;
            }
        }
    }
}
