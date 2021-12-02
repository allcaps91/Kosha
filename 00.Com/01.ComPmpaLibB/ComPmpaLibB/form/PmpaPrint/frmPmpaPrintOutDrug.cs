using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComEmrBase; //기본 EMR 클래스 : 선택적으로
using ComLibB;
using ComPmpaLibB;
using static ComPmpaLibB.clsDrugPrint;
using Newtonsoft.Json.Linq;
using System.Net;

/// <summary>
/// Description : 원외처방전 재발행
/// Author : 박병규
/// Create Date : 2017.11.16
/// </summary>
/// <history>
/// </history>
/// <seealso cref="Frm원외처방전재발행(Frm원외처방전재발행.frm)"/>

namespace ComPmpaLibB
{
    public partial class frmPmpaPrintOutDrug : Form
    {
        clsPmpaQuery CPQ = null;
        clsPmpaPrint CPP = null;
        clsOrdFunction OF = null;

        DataTable Dt = new DataTable();
        string SQL = "";
        string SqlErr = "";

        public frmPmpaPrintOutDrug()
        {
            InitializeComponent();
            setParam();
        }

        private void setParam()
        {
            this.Load += new EventHandler(eFrm_Load);

            this.dtpBdate.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);

            this.txtPtno.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.txtPart.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);

            this.btnSearch.Click += new EventHandler(eCtl_Click);//조회
            this.btnChangeNo.Click += new EventHandler(eCtl_Click);//재정산처방전 번호맞춤
            this.btnPrintRe.Click += new EventHandler(eCtl_Click);//선택재발행
            this.btnSend.Click += new EventHandler(eCtl_Click);//선택재발행
            this.btnExit.Click += new EventHandler(eCtl_Click);
        }

        private void eCtl_Click(object sender, EventArgs e)
        {
            if (sender == this.btnSearch)
                READ_DATA(clsDB.DbCon);
            else if (sender == this.btnChangeNo)
                CHANGE_PROCESS();
            else if (sender == this.btnPrintRe)
                REPRINT_PROCESS(clsDB.DbCon);
            else if (sender == this.btnSend)
                REPRINT_PROCESS_Send(clsDB.DbCon);

            else if (sender == this.btnExit)
                this.Close();

        }
        //TODM.strPano.Trim(), TODM.strBDate, TODM.strDeptCode.Trim(), TODM.dblSlipNo, TODM.strName.Trim()
        private string Push_send(string sDT, string sPatno, string sBDate,  string sDept, string sSlipno, string sName )
        {
            string rtnVal = "";
            try
            {
                //push type 32 고정
                string sStep = "32";
                string sStep1 = "50";
                string h_id = "HSPTL-0001";
            //서버정보
           // string url = "http://221.157.239.4:8888/MPService/inside/addTrans";
            //string url = "https://mp.pohangsmh.co.kr/MPService/inside/addTrans";
              string url = "https://mp.pohangsmh.co.kr/u2m/api/extern/ex-payment";
                string sReturn;

                //string sInputJson = "{ \"h_id\":\"" + h_id + "\", ";
                //string sInputJson = "";
                //sInputJson = sInputJson + " { \"mpDate\": \"" + sBDate + "\", ";
                //sInputJson = sInputJson + " \"mpPatNo\": \"" + sPatno + "\", ";
                //sInputJson = sInputJson + " \"mpDept\": \"" + sDept + "\", ";
                //sInputJson = sInputJson + " \"mpPresc\": \"" + sSlipno + "\", ";
                //sInputJson = sInputJson + " \"mpStep\": \"" + sStep + "\", ";
                //sInputJson = sInputJson + " \"stepChk\": \"" + sStep + "\" }";

                string sInputJson = "";
                sInputJson = sInputJson + " { \"mpDate\": \"" + sBDate + "\", ";
                sInputJson = sInputJson + " \"mpPatNo\": \"" + sPatno + "\", ";
                sInputJson = sInputJson + " \"mpDept\": \"" + sDept + "\", ";
                sInputJson = sInputJson + " \"mpPresc\": \"" + sSlipno + "\", ";
                sInputJson = sInputJson + " \"mpStep\": \"" + sStep + "\", ";
                sInputJson = sInputJson + " \"stepChk\": \"" + sStep1 + "\" }";



                //send data print
                string strSendData = "send data=" + sInputJson;

                // JSON 데이터 전송
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.Method = "POST";
                using (StreamWriter stream = new StreamWriter(request.GetRequestStream()))
                {
                    stream.Write(sInputJson);
                    stream.Flush();
                    stream.Close();
                }

                // JSON 데이터 전송 결과 학인하기 
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    sReturn = reader.ReadToEnd();
                }

                if (sReturn == "0000")
                {
                    rtnVal = "Push성공";
                }
                else
                {
                    rtnVal = "Push실패";
                }

                //결과값 Print
                //string strReceiveData = sReturn;

                ////결과값 json 으로 parsing
                //JObject jobj = JObject.Parse(sReturn); //문자를 객체화

                //string rtn_value = null;

                //if (jobj != null && jobj["rtn_cd"] != null)
                //{
                //    rtn_value = jobj["rtn_cd"].ToString();

                //    if (rtn_value == "0000")
                //    {
                //        rtnVal = "Push성공" + ", rtn=" + rtn_value;
                //    }
                //    else
                //    {
                //        rtnVal = "Push실패" + ", rtn=" + rtn_value;
                //    }

                //}
                //else
                //{
                //    rtnVal = "Push실패" + ", rtn=" + rtn_value;
                //}
            }
            catch (Exception ex)
            {
                //에러처리
                rtnVal = "에러발생 : " + ex.ToString();
            }

            return rtnVal;
        }

        private void REPRINT_PROCESS_Send(PsmhDb pDbCon)
        {
            string strMsg = "";

            string strPtno = string.Empty;
            string strName = string.Empty;
            string strSlipNo = string.Empty;
            string strDept = string.Empty;
            string strSunap = string.Empty;
            string strBdate = string.Empty;

            int intPage = 0;
            string strSlipDate = dtpBdate.Text;

            clsDrugPrint cDP = new ComPmpaLibB.clsDrugPrint();

            FarPoint.Win.Spread.FpSpread ssSpread = null;
            FarPoint.Win.Spread.SheetView ssSpread_Sheet1 = null;

            clsPmpaPb.GstrPrintData = "";
            Type_OutDrugMst TODM = new Type_OutDrugMst();
            cDP.Clear_Type_OutDrugMst(TODM);

            //cDP.SetDrugPrint(ref ssSpread, ref ssSpread_Sheet1);
            //ComFunc.Delay(500);

            for (int i = 0; i < ssList_Sheet1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(ssList_Sheet1.Cells[i, 0].Value) == true)
                {
                    ssList_Sheet1.Cells[i, 0].Value = false;
                    strPtno = ssList_Sheet1.Cells[i, 1].Text;
                    strName = ssList_Sheet1.Cells[i, 2].Text;
                    strDept = ssList_Sheet1.Cells[i, 3].Text;
                    strBdate = ssList_Sheet1.Cells[i, 4].Text;
                    strSlipNo = strBdate.Replace("-", "")  + Convert.ToInt32(ssList_Sheet1.Cells[i, 5].Text).ToString("00000");

                 
                    strSunap = ssList_Sheet1.Cells[i, 7].Text;
                   
                    clsPublic.GstrRetValue = strPtno + "^^" + "외래^^" + strSlipDate + "~" + strSlipDate + "^^" + "03^^";

                    frmSetPrintInfo frm = new frmSetPrintInfo();
                    frm.ShowDialog();
                    //OF.fn_ClearMemory(frm);


                    if (VB.Pstr(clsPublic.GstrRetValue, "^^", 1).Trim() != "OK") { return; }
                    intPage = Convert.ToInt32(VB.Pstr(clsPublic.GstrRetValue, "^^", 2));
                    clsPublic.GstrRetValue = "";

                    if (strSunap == "Y")
                    {
                        
                        try
                        {
                            //LogWrite("푸쉬요청 >> " + TODM.strPano.Trim() + "/" + TODM.strDeptCode.Trim());

                          
                                strMsg = Push_send(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), strPtno, strBdate, strDept, strSlipNo, strName);
                                ComFunc.MsgBox(strMsg);
                          

                            //LogWrite("푸쉬결과 >> " + TODM.strPano.Trim() + "/" + TODM.strDeptCode.Trim() + ComNum.VBLF + strMsg);
                        }
                        catch { }
                    }
                    else
                    {
                        ComFunc.MsgBox("원무수납 발행만 재발행 가능함.", "알림");
                    }
                }
            }
        }
        private void LogWrite(string str)
        {
            string FilePath = @"C:\Push_Log\Log_" + DateTime.Today.ToString("yyyyMMdd") + ".log";
            string DirPath = @"C:\Push_Log";
            string temp;

            DirectoryInfo di = new DirectoryInfo(DirPath);
            FileInfo fi = new FileInfo(FilePath);

            try
            {
                if (!di.Exists) Directory.CreateDirectory(DirPath);
                if (!fi.Exists)
                {
                    using (StreamWriter sw = new StreamWriter(FilePath))
                    {
                        temp = string.Format("[{0}] {1}", DateTime.Now, str);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(FilePath))
                    {
                        temp = string.Format("[{0}] {1}", DateTime.Now, str);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        /// <summary>
        /// 선택재발행
        /// </summary>
        private void REPRINT_PROCESS(PsmhDb pDbCon)
        {
            string strPtno = string.Empty;
            string strSunap = string.Empty;
            int intSlipNo = 0;
            int intPage = 0;
            string strSlipDate = dtpBdate.Text;

            clsDrugPrint cDP = new ComPmpaLibB.clsDrugPrint();

            FarPoint.Win.Spread.FpSpread ssSpread = null;
            FarPoint.Win.Spread.SheetView ssSpread_Sheet1 = null;

            clsPmpaPb.GstrPrintData = "";
            Type_OutDrugMst TODM = new Type_OutDrugMst();
            cDP.Clear_Type_OutDrugMst(TODM);

            //cDP.SetDrugPrint(ref ssSpread, ref ssSpread_Sheet1);
            //ComFunc.Delay(500);

            for (int i = 0; i < ssList_Sheet1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(ssList_Sheet1.Cells[i, 0].Value) == true)
                {
                    ssList_Sheet1.Cells[i, 0].Value = false;
                    strPtno = ssList_Sheet1.Cells[i, 1].Text;
                    intSlipNo = Convert.ToInt32(ssList_Sheet1.Cells[i, 5].Text);
                    strSunap = ssList_Sheet1.Cells[i, 7].Text;

                    clsPublic.GstrRetValue = strPtno + "^^" + "외래^^" + strSlipDate + "~" + strSlipDate + "^^" + "03^^";

                    frmSetPrintInfo frm = new frmSetPrintInfo();
                    frm.ShowDialog();
                    //OF.fn_ClearMemory(frm);


                    if (VB.Pstr(clsPublic.GstrRetValue, "^^", 1).Trim() != "OK") { return; }
                    intPage = Convert.ToInt32(VB.Pstr(clsPublic.GstrRetValue, "^^", 2));
                    clsPublic.GstrRetValue = "";

                    if (strSunap == "Y")
                    {
                        DeleteBarCodeFIle();

                        cDP.READ_OutDrugMst(pDbCon, strSlipDate, intSlipNo, strPtno, ref TODM);
                        
                        cDP.SetDrugPrint(ref ssSpread, ref ssSpread_Sheet1);
                        cDP.ClearDrugPrint(ssSpread, ssSpread_Sheet1);

                        cDP.OutDRUG_Slip_Print(pDbCon, ssSpread, ssSpread_Sheet1, TODM, TODM.strPano, TODM.strBDate, TODM.dblSlipNo, "OK", "LASER", false, "1", clsPmpaPrint.DRUG_PRINTER_NAME);

                        ComFunc.Delay(500);

                        cDP.SetDrugPrint(ref ssSpread, ref ssSpread_Sheet1);
                        cDP.ClearDrugPrint(ssSpread, ssSpread_Sheet1);

                        cDP.OutDRUG_Slip_Print(pDbCon, ssSpread, ssSpread_Sheet1, TODM, TODM.strPano, TODM.strBDate, TODM.dblSlipNo, "OK", "LASER", false, "2", clsPmpaPrint.DRUG_PRINTER_NAME);

                    }
                    else
                    {
                        ComFunc.MsgBox("원무수납 발행만 재발행 가능함.", "알림");
                    }
                }
            }
        }

        private void DeleteBarCodeFIle()
        {
            try
            {
                if (File.Exists("C:\\PSMHEXE\\YAK_BARCODE.BMP") == true)
                {
                    File.Delete("C:\\PSMHEXE\\YAK_BARCODE.BMP");
                    ComFunc.Delay(500);
                }
            }
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }
        /// <summary>
        /// 재정산처방전 번호맞춤
        /// </summary>
        private void CHANGE_PROCESS()
        {
            int intCnt = 0;

            for (int i = 0; i < ssList_Sheet1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(ssList_Sheet1.Cells[i, 0].Value) == true)
                {
                    intCnt += 1;
                    if (intCnt > 1)
                    {
                        ComFunc.MsgBox("재정산처방전 번호맞춤 선택은 한가지만 가능함.", "알림");
                        return;
                    }
                }
            }

            if (intCnt == 0)
            {
                ComFunc.MsgBox("재정산번호를 변경할 처방전이 선택되지 않음.", "처방전선택");
                return;
            }

            clsPublic.GstrHelpCode = "";
            for (int i = 0; i < ssList_Sheet1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(ssList_Sheet1.Cells[i, 0].Value) == true)
                {
                    clsPublic.GstrHelpCode = ssList_Sheet1.Cells[i, 14].Text;
                    clsPublic.GstrHelpCode += "|" + ssList_Sheet1.Cells[i, 1].Text;
                    clsPublic.GstrHelpCode += "|" + ssList_Sheet1.Cells[i, 3].Text;

                    frmPmpaEntryOutDrugNo frmNo = new ComPmpaLibB.frmPmpaEntryOutDrugNo();
                    frmNo.ShowDialog();
                    OF.fn_ClearMemory(frmNo);


                    clsPublic.GstrHelpCode = "";
                    return;
                }
            }
        }

        /// <summary>
        /// 원외처방전 마스터에서 조회.
        /// </summary>
        private void READ_DATA(PsmhDb pDbCon)
        {
            Cursor.Current = Cursors.WaitCursor;
            ComFunc.SetAllControlClear(pnlBody);

            if (txtPtno.Text.Trim() == "") { return; }

            txtPtno.Text = string.Format("{0:D8}", Convert.ToInt32(txtPtno.Text));

            //대기자 자료
            SQL = "";
            SQL += ComNum.VBLF + " SELECT a.Pano, b.Sname, a.DeptCode,                          --등록번호,수진자명,진료과";
            SQL += ComNum.VBLF + "        TO_CHAR(a.BDate,'YYYY-MM-DD') BDate,                  --발생일자";
            SQL += ComNum.VBLF + "        TO_CHAR(a.SlipDate,'YYYY-MM-DD') SlipDate,            --원외처방발급일자";
            SQL += ComNum.VBLF + "        TO_CHAR(a.EntDate,'YYYY-MM-DD HH24:MI') EntDate,      --등록시각(외래수납시각)";
            SQL += ComNum.VBLF + "        TO_CHAR(a.PrtDate,'YYYY-MM-DD HH24:MI') PrtDate,      --인쇄시각";
            SQL += ComNum.VBLF + "        a.SlipNo, a.Part, a.PrtBun,                           --원외처방전번호,작업조,위치";
            SQL += ComNum.VBLF + "        a.ChkPrt, a.GbAuto, a.GbV252,  a.GbV352,                       --인쇄처리구분(대상기본:Y,처리:P),자동출력,V252여부";
            SQL += ComNum.VBLF + "        KOSMOS_OCS.FC_ETC_TUYAKNO(TO_CHAR(a.SlipDate,'YYYY-MM-DD'), a.SlipNo, a.Pano) TUNO, ";
            SQL += ComNum.VBLF + "        KOSMOS_OCS.FC_OPD_CHOJAE(TO_CHAR(a.SlipDate,'YYYY-MM-DD'), a.Pano,a.DeptCode) CHOJAE, ";
            SQL += ComNum.VBLF + "        a.ROWID ";                
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OUTDRUGMST a,                --원외처방전 마스터테이블";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_Patient b                   --환자기본 마스터테이블";
            SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
            SQL += ComNum.VBLF + "    AND a.SlipDate    = TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND a.FLAG        = 'P' ";  //출력된것만

            if (rdoAuto1.Checked == true)
                SQL += ComNum.VBLF + "AND a.GbAuto      = 'Y' ";  //자동발행건만
            else if (rdoAuto2.Checked == true)
                SQL += ComNum.VBLF + "AND (a.GbAuto IS NULL OR a.GbAuto <>'Y') ";

            SQL += ComNum.VBLF + "    AND a.Pano        = b.Pano(+) ";

            if (txtPtno.Text.Trim() != "")
                SQL += ComNum.VBLF + "AND a.Pano        = '" + txtPtno.Text.Trim() + "' ";

            if (txtPart.Text.Trim() != "")
                SQL += ComNum.VBLF + "AND a.Part        = '" + txtPart.Text.Trim() + "' ";
            SQL += ComNum.VBLF + "ORDER BY a.SlipNo DESC ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            ssList_Sheet1.RowCount = Dt.Rows.Count;
            ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                ssList_Sheet1.Cells[i, 0].Value = false;
                ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["PANO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["SNAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["DeptCode"].ToString().Trim();
                ssList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["BDate"].ToString().Trim();
                ssList_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["SlipNo"].ToString().Trim();
                ssList_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["Part"].ToString().Trim();
                ssList_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["GbAuto"].ToString().Trim();
                ssList_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["GbV252"].ToString().Trim();
                ssList_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["PrtBun"].ToString().Trim();
                ssList_Sheet1.Cells[i, 10].Text = Dt.Rows[i]["TUNO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 11].Text = Dt.Rows[i]["EntDate"].ToString().Trim();
                ssList_Sheet1.Cells[i, 12].Text = Dt.Rows[i]["PrtDate"].ToString().Trim();
                ssList_Sheet1.Cells[i, 13].Text = Dt.Rows[i]["CHOJAE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 14].Text = Dt.Rows[i]["SlipDate"].ToString().Trim();
                ssList_Sheet1.Cells[i, 15].Text = Dt.Rows[i]["GbV352"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;
        }


        private void eCtl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.dtpBdate && e.KeyChar == (Char)13)
                txtPtno.Focus();
            else if (sender == this.txtPtno && e.KeyChar == (Char)13)
            {
                if (txtPtno.Text != "")
                    txtPtno.Text = string.Format("{0:D8}", Convert.ToInt32(txtPtno.Text));

                txtPart.Focus();
            }
            else if (sender == this.txtPart && e.KeyChar == (Char)13)
                btnSearch.Focus();
        }

        private void eFrm_Load(object sender, EventArgs e)
        {
            CPQ = new clsPmpaQuery();
            CPP = new ComPmpaLibB.clsPmpaPrint();
            OF = new clsOrdFunction();

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            ComFunc.SetAllControlClear(pnlTop);
            dtpBdate.Text = clsPublic.GstrSysDate;
            txtPart.Text = clsType.User.IdNumber;
        }
    }
}
