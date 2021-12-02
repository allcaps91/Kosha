using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComLibB;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewDocumentList.cs
    /// Description     : 재증명서List
    /// Author          : 안정수
    /// Create Date     : 2017-10-02
    /// Update History  : 
    /// <history>       
    /// 쿼리 조건부분에서 PAT.Ptno 어디서 받아오는지 알수가 없으므로 조건에서 제외하고 구현함
    /// 이상훈 수정(FstrPano 추가 : 생성자 Parameter)
    /// d:\psmh\OPD\jengsan\Frm재증명서LIST.frm(Frm재증명서LIST) => frmPmpaViewDocumentList.cs 으로 변경함
    /// 2019-02-18 기존 HISTORY_PRINT 함수 HISTORY_PRINT_OLD 로 변경후 새로 구현함 KMC
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\jengsan\Frm재증명서LIST.frm(Frm재증명서LIST)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewDocumentList : Form
    {
        string FstrPano;
        
        public frmPmpaViewDocumentList(string sPano )
        {
            InitializeComponent();

            FstrPano = sPano;

            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등                  

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            Set_Combo();
            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

            dtpFDate.Text = Convert.ToDateTime(CurrentDate).AddDays(-7).ToShortDateString();
            dtpTDate.Text = CurrentDate;

            btnView_Click();
        }

        void Set_Combo()
        {
            cboPart.Items.Clear();

            cboPart.Items.Add("**.전체");
            cboPart.Items.Add("00.진료사실증명서");
            cboPart.Items.Add("01.진단서");
            cboPart.Items.Add("02.상해진단서");
            cboPart.Items.Add("03.병사용진단서");
            cboPart.Items.Add("05.사망진단서");
            cboPart.Items.Add("08.소견서");
            cboPart.Items.Add("18.진료의뢰서");
            cboPart.Items.Add("19.장애인증명서");
            cboPart.Items.Add("20.장애진단서");
            cboPart.Items.Add("26.의료급여의뢰서");
            cboPart.Items.Add("27.응급환자진료의뢰서");
            cboPart.Items.Add("29.근로능력평가진단서");
            cboPart.SelectedIndex = 0;
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                btnView_Click();

            }

            else if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;


            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;


            #endregion

            strTitle = "재증명서 List";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 11), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 130, 50);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, false, false, true, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;


            #endregion
        }

        void btnView_Click()
        {
            HISTORY_PRINT();
        }

        void HISTORY_PRINT_OLD()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int i = 0;
            int intHeight = 0;

            ssList_Sheet1.Rows.Count = 0;
            
            try
            {
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     MCCLASS, SEQNO, PANO, " + ComNum.DB_MED + "FC_BAS_PATIENT_SNAME(PANO) AS SNAME, 'O' AS IPDOPD, ";
                SQL = SQL + ComNum.VBLF + "     DEPTCODE, SINNAME, SINSAYU, BIGO, TO_CHAR(SEQDATE,'YYYY-MM-DD') AS BDATE, TO_CHAR(REPRINT, 'YYYY-MM-DD') AS PRTDATE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU_REPRINT";
                SQL = SQL + ComNum.VBLF + "     WHERE REPRINT >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "         AND REPRINT <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";

                if (VB.Left(cboPart.Text, 2) != "**")
                {
                    SQL = SQL + ComNum.VBLF + "         AND MCCLASS = '" + VB.Left(cboPart.Text, 2) + "' ";
                }

                SQL = SQL + ComNum.VBLF + "         AND PANO = '" + FstrPano + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SEQNO DESC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssList_Sheet1.RowCount = dt.Rows.Count;
                    ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["IPDOPD"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["BDATE"].ToString().Trim() + "(";

                        switch (dt.Rows[i]["MCCLASS"].ToString().Trim())
                        {
                            case "00": ssList_Sheet1.Cells[i, 5].Text += "진료사실증명서"; break;
                            case "01": ssList_Sheet1.Cells[i, 5].Text += "진단서"; break;
                            case "08": ssList_Sheet1.Cells[i, 5].Text += "소견서"; break;
                            case "18": ssList_Sheet1.Cells[i, 5].Text += "진료의뢰서"; break;
                            case "26": ssList_Sheet1.Cells[i, 5].Text += "의료급여의뢰서"; break;
                            case "27": ssList_Sheet1.Cells[i, 5].Text += "응급환자진료의뢰서"; break;
                            case "02": ssList_Sheet1.Cells[i, 5].Text += "상해진단서"; break;
                            case "03": ssList_Sheet1.Cells[i, 5].Text += "병사용진단서"; break;
                            case "05": ssList_Sheet1.Cells[i, 5].Text += "사망진단서"; break;
                            case "19": ssList_Sheet1.Cells[i, 5].Text += "장애인증명서"; break;
                            case "20": ssList_Sheet1.Cells[i, 5].Text += "장애진단서"; break;
                            case "29": ssList_Sheet1.Cells[i, 5].Text += "근로능력평가진단서"; break;
                        }

                        ssList_Sheet1.Cells[i, 5].Text += ")";

                        ssList_Sheet1.Cells[i, 6].Text = "신청자:" + dt.Rows[i]["SINNAME"].ToString().Trim()
                                        + ComNum.VBLF + "신청사유:" + dt.Rows[i]["SINSAYU"].ToString().Trim();

                        intHeight = 2;
                        ssList_Sheet1.SetRowHeight(i, ComNum.SPDROWHT + (intHeight * 15));

                        if (dt.Rows[i]["BIGO"].ToString().Trim() != "")
                        {
                            ssList_Sheet1.Cells[i, 6].Text += ComNum.VBLF + "비고:" + dt.Rows[i]["BIGO"].ToString().Trim();

                            intHeight = 3;
                            ssList_Sheet1.SetRowHeight(i, ComNum.SPDROWHT + (intHeight * 15));
                        }

                        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["PRTDATE"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void HISTORY_PRINT()
        {
            string SQL = "", SQL1 = "", SQL2 = "", SQL3 = "", SQL4 = "", SQL5 = "";
            string SQL6 = "", SQL7 = "", SQL8 = "", SQL9 = "", SQL10 = "", SQL11 = "", SQL12 = "";
            string SqlErr = "";
            DataTable dt = null;
            int i = 0;

            string strGubun = VB.Left(cboPart.Text, 2);
            string strDate  = dtpFDate.Value.ToString("yyyy-MM-dd");
            string StrDate2 = dtpTDate.Value.ToString("yyyy-MM-dd");

            ssList_Sheet1.Rows.Count = 0;

            try
            {
                SQL1 = " SELECT A.SEQNO, A.PANO, A.IPDOPD, A.DEPTCODE, A.REMARK, A.BDATE, B.SNAME, A.PRTDATE  \r\n";
                SQL1 += "  FROM " + ComNum.DB_PMPA + "ETC_WONSELU A, " + ComNum.DB_PMPA + "BAS_PATIENT B  \r\n";
                SQL1 += " WHERE A.ACTDATE >= TO_DATE('" + strDate + "','YYYY-MM-DD')    \r\n";
                SQL1 += "   AND A.ACTDATE <= TO_DATE('" + StrDate2 + "','YYYY-MM-DD')    \r\n";
                SQL1 += "   AND A.PANO = B.PANO(+)   \r\n";
                SQL1 += "   AND B.pano = '" + FstrPano + "'  \r\n";
                
                SQL2 = " SELECT A.SEQNO, B.PTNO PANO, '' IPDOPD, B.DEPTCODE, '진단서' REMARK, '' BDATE, B.SNAME, A.SEQDATE PRTDATE  \r\n";
                SQL2 += "  FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI01 B  \r\n";
                SQL2 += " WHERE a.MCNO(+) = b.MCNO  \r\n";
                SQL2 += "   AND A.MCCLASS(+) = '01'  \r\n";
                SQL2 += "   AND B.LSDATE >= TO_DATE('" + strDate + "','YYYY-MM-DD')    \r\n";
                SQL2 += "   AND B.LSDATE <= TO_DATE('" + StrDate2 + "','YYYY-MM-DD')    \r\n";
                SQL2 += "   AND B.ptno = '" + FstrPano + "'  \r\n";
                
                SQL3 = " SELECT A.SEQNO, B.PTNO PANO, '' IPDOPD, B.DEPTCODE, '소견서' REMARK, '' BDATE, B.SNAME, A.SEQDATE PRTDATE  \r\n";
                SQL3 += "  FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI08 B  \r\n";
                SQL3 += " WHERE a.MCNO(+) = b.MCNO  \r\n";
                SQL3 += "   AND A.MCCLASS(+) = '08'  \r\n";
                SQL3 += "   AND B.LSDATE >= TO_DATE('" + strDate + "','YYYY-MM-DD')    \r\n";
                SQL3 += "   AND B.LSDATE <= TO_DATE('" + StrDate2 + "','YYYY-MM-DD')    \r\n";
                SQL3 += "   AND B.ptno = '" + FstrPano + "'  \r\n";
                
                SQL4 = " SELECT A.SEQNO, B.PTNO PANO, '' IPDOPD, B.DEPTCODE, '진료의뢰서' REMARK, '' BDATE, B.SNAME, A.SEQDATE PRTDATE  \r\n";
                SQL4 += "  FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI18 B  \r\n";
                SQL4 += " WHERE a.MCNO(+) = b.MCNO  \r\n";
                SQL4 += "   AND A.MCCLASS(+) = '18'  \r\n";
                SQL4 += "   AND B.LSDATE >= TO_DATE('" + strDate + "','YYYY-MM-DD')    \r\n";
                SQL4 += "   AND B.LSDATE <= TO_DATE('" + StrDate2 + "','YYYY-MM-DD')    \r\n";
                SQL4 += "   AND B.ptno = '" + FstrPano + "'  \r\n";
                
                SQL5 = " SELECT A.SEQNO, B.PTNO PANO, '' IPDOPD, '' DEPTCODE, '의료급여의뢰서' REMARK, '' BDATE, B.SNAME, A.SEQDATE PRTDATE  \r\n";
                SQL5 += "  FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI26 B  \r\n";
                SQL5 += " WHERE a.MCNO(+) = b.MCNO  \r\n";
                SQL5 += "   AND A.MCCLASS(+) = '26'  \r\n";
                SQL5 += "   AND B.BALDATE >= TO_DATE('" + strDate + "','YYYY-MM-DD')    \r\n";
                SQL5 += "   AND B.BALDATE <= TO_DATE('" + StrDate2 + "','YYYY-MM-DD')    \r\n";
                SQL5 += "   AND B.ptno = '" + FstrPano + "'  \r\n";
                
                SQL6 = " SELECT A.SEQNO, B.PTNO PANO, '' IPDOPD, B.DEPTCODE, '응급환자진료의뢰서' REMARK, '' BDATE, B.SNAME, A.SEQDATE PRTDATE  \r\n";
                SQL6 += "  FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI27 B  \r\n";
                SQL6 += " WHERE a.MCNO(+) = b.MCNO  \r\n";
                SQL6 += "   AND A.MCCLASS(+) = '27'  \r\n";
                SQL6 += "   AND B.LSDATE >= TO_DATE('" + strDate + "','YYYY-MM-DD')    \r\n";
                SQL6 += "   AND B.LSDATE <= TO_DATE('" + StrDate2 + "','YYYY-MM-DD')    \r\n";
                SQL6 += "   AND B.ptno = '" + FstrPano + "'  \r\n";
                
                //add 상해,병사,사망,장애증명,장애진단
                SQL7 = " SELECT A.SEQNO, B.PTNO PANO, '' IPDOPD, B.DEPTCODE, '상해진단서' REMARK, '' BDATE, B.SNAME, A.SEQDATE PRTDATE  \r\n";
                SQL7 += "  FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI02 B  \r\n";
                SQL7 += " WHERE a.MCNO(+) = b.MCNO  \r\n";
                SQL7 += "   AND A.MCCLASS (+)= '02'  \r\n";
                SQL7 += "   AND B.LSDATE >= TO_DATE('" + strDate + "','YYYY-MM-DD')    \r\n";
                SQL7 += "   AND B.LSDATE <= TO_DATE('" + StrDate2 + "','YYYY-MM-DD')    \r\n";
                SQL7 += "   AND B.ptno = '" + FstrPano + "'  \r\n";
                
                SQL8 = " SELECT A.SEQNO, B.PTNO PANO, '' IPDOPD, B.DEPTCODE, '병사용진단서' REMARK, '' BDATE, B.SNAME, A.SEQDATE PRTDATE  \r\n";
                SQL8 += "  FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI03 B  \r\n";
                SQL8 += " WHERE a.MCNO (+)= b.MCNO  \r\n";
                SQL8 += "   AND A.MCCLASS (+)= '03'  \r\n";
                SQL8 += "   AND B.LSDATE >= TO_DATE('" + strDate + "','YYYY-MM-DD')    \r\n";
                SQL8 += "   AND B.LSDATE <= TO_DATE('" + StrDate2 + "','YYYY-MM-DD')    \r\n";
                SQL8 += "   AND B.ptno = '" + FstrPano + "'  \r\n";
                
                SQL9 = " SELECT A.SEQNO, B.PTNO PANO, '' IPDOPD, B.DEPTCODE, '사망진단서' REMARK, '' BDATE, B.SNAME, A.SEQDATE PRTDATE  \r\n";
                SQL9 += "  FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI05 B  \r\n";
                SQL9 += " WHERE a.MCNO (+)= b.MCNO  \r\n";
                SQL9 += "   AND A.MCCLASS (+)= '05'  \r\n";
                SQL9 += "   AND B.LSDATE >= TO_DATE('" + strDate + "','YYYY-MM-DD')    \r\n";
                SQL9 += "   AND B.LSDATE <= TO_DATE('" + StrDate2 + "','YYYY-MM-DD')    \r\n";
                SQL9 += "   AND B.ptno = '" + FstrPano + "'  \r\n";
                
                SQL10 = " SELECT A.SEQNO, B.PTNO PANO, '' IPDOPD,  (SELECT DRDEPT1 FROM " + ComNum.DB_PMPA + "BAS_DOCTOR C WHERE B.DRCODE = C.DRCODE) DEPTCODE,  \r\n";
                SQL10 += " '장애인증명서' REMARK, '' BDATE, B.TNAME SNAME, A.SEQDATE PRTDATE  \r\n";
                SQL10 += "  FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI19 B  \r\n";
                SQL10 += " WHERE a.MCNO = b.MCNO  \r\n";
                SQL10 += "   AND A.MCCLASS = '19'  \r\n";
                SQL10 += "   AND B.LSDATE >= TO_DATE('" + strDate + "','YYYY-MM-DD')    \r\n";
                SQL10 += "   AND B.LSDATE <= TO_DATE('" + StrDate2 + "','YYYY-MM-DD')    \r\n";
                SQL10 += "   AND B.ptno = '" + FstrPano + "'  \r\n";
                
                SQL11 = " SELECT A.SEQNO, B.PTNO PANO, '' IPDOPD, B.DEPTCODE, '장애진단서' REMARK, '' BDATE, NAME SNAME, A.SEQDATE PRTDATE  \r\n";
                SQL11 += "  FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI22 B  \r\n";
                SQL11 += " WHERE a.MCNO(+) = b.MCNO  \r\n";
                SQL11 += "   AND A.MCCLASS (+)= '22'  \r\n";
                SQL11 += "   AND B.LSDATE >= TO_DATE('" + strDate + "','YYYY-MM-DD')    \r\n";
                SQL11 += "   AND B.LSDATE <= TO_DATE('" + StrDate2 + "','YYYY-MM-DD')    \r\n";
                SQL11 += "   AND B.ptno = '" + FstrPano + "'  \r\n";
                
                SQL12 = " SELECT A.SEQNO, B.PTNO PANO, '' IPDOPD, B.DEPTCODE, '근로능력평가진단서' REMARK, '' BDATE, SNAME, A.SEQDATE PRTDATE  \r\n";
                SQL12 += "  FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI29 B  \r\n";
                SQL12 += " WHERE a.MCNO (+)= b.MCNO  \r\n";
                SQL12 += "   AND A.MCCLASS (+)= '29'  \r\n";
                SQL12 += "   AND B.LSDATE >= TO_DATE('" + strDate + "','YYYY-MM-DD')    \r\n";
                SQL12 += "   AND B.LSDATE <= TO_DATE('" + StrDate2 + "','YYYY-MM-DD')    \r\n";
                SQL12 += "   AND B.ptno = '" + FstrPano + "'  \r\n";

                switch (strGubun)
                {
                    case "**":
                        SQL = SQL1 + ComNum.VBLF + " UNION ALL " + ComNum.VBLF + 
                              SQL2 + ComNum.VBLF + " UNION ALL " + ComNum.VBLF + 
                              SQL3 + ComNum.VBLF + " UNION ALL " + ComNum.VBLF + 
                              SQL4 + ComNum.VBLF + " UNION ALL " + ComNum.VBLF + 
                              SQL5 + ComNum.VBLF + " UNION ALL " + ComNum.VBLF + 
                              SQL6 + ComNum.VBLF + " UNION ALL " + ComNum.VBLF + 
                              SQL7 + ComNum.VBLF + " UNION ALL " + ComNum.VBLF + 
                              SQL8 + ComNum.VBLF + " UNION ALL " + ComNum.VBLF + 
                              SQL9 + ComNum.VBLF + " UNION ALL " + ComNum.VBLF + 
                              SQL10 + ComNum.VBLF + " UNION ALL " + ComNum.VBLF + 
                              SQL11 + ComNum.VBLF + " UNION ALL " + ComNum.VBLF + SQL12; 
                        break;
                    case "00": SQL = SQL1; break;
                    case "01": SQL = SQL2; break;
                    case "08": SQL = SQL3; break;
                    case "18": SQL = SQL4; break;
                    case "26": SQL = SQL5; break;
                    case "27": SQL = SQL6; break;
                    case "02": SQL = SQL7; break;
                    case "03": SQL = SQL8; break;
                    case "05": SQL = SQL9; break;
                    case "19": SQL = SQL10; break;
                    case "20": SQL = SQL11; break;
                    case "29": SQL = SQL12; break;
                    default:  break;
                }

                SQL += ComNum.VBLF + "  ORDER BY SEQNO DESC ";
                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssList_Sheet1.RowCount = dt.Rows.Count;
                    ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["IPDOPD"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["PRTDATE"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

    }
}
