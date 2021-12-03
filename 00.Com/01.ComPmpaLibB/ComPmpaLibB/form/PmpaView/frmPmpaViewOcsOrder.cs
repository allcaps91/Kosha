using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewOcsOrder.cs
    /// Description     : OCS 오더 미수납자 조회
    /// Author          : 안정수
    /// Create Date     : 2017-08-30
    /// Update History  : 2017-11-02
    /// <history>       
    /// d:\psmh\OPD\olrepa\OLREPA13.FRM(frmOCSorder) => frmPmpaViewOcsOrder.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\OLREPA13.FRM(frmOCSorder)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewOcsOrder : Form
    {
        ComFunc CF = null;
        clsSpread CS = null;

        string mstrJobName = "";
        string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

        string[] strGwaName = new string[30];

        int[,] nGwaNum = new int[15, 21];
        double[,] nGwaPay = new double[15, 21];

        public frmPmpaViewOcsOrder()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewOcsOrder(string GstrJobName)
        {
            InitializeComponent();
            setEvent();
            mstrJobName = GstrJobName;
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
            CF = new ComFunc();
            CS = new clsSpread();

            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등            

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;            

            optGB0.Checked = true;
            optOCS0.Checked = true;
            optSunap0.Checked = true;
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
                //
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }

                eGetData();
            }

            else if (sender == this.btnPrint)
            {
                //
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
            string strSubTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ssList_Sheet1.Rows.Count == 0)
            {
                return;
            }

            string JobDate = dtpDate.Text;
            btnPrint.Enabled = false;


            strTitle = "OCS 오더 미수납자 명단";
            if (mstrJobName != "")
            {
                strSubTitle = "작성자 : " + mstrJobName + "\r\n" + "작업일자: " + JobDate;
            }

            else
            {
                strSubTitle = "작업일자: " + JobDate;
            }

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 60, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, true, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            btnPrint.Enabled = true;
        }

        void eGetData()
        {
            ssListClear();
            ssListBuild();
        }

        void ssListClear()
        {
            CS.Spread_All_Clear(ssList);
        }

        void ssListBuild()
        {
            clsSpread CS = new clsSpread();
            int i = 0;
            int nCount = 0;

            string strCode = "";
            string strGbinfor = "";
            string strOrdercode = "";

            int strNal = 0;

            string strPano = "";
            string strRes = "";
            string strOK = "";

            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = "";

            CS.Spread_All_Clear(ssList);

            if (optOCS0.Checked == true)
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                                                        ";
                SQL += ComNum.VBLF + "  A.ORDERCODE, A.DEPTCODE, A.GBINFO, C.DRNAME, B.SNAME, A.PTNO,                                               ";
                SQL += ComNum.VBLF + "  B.SEX, B.JUMIN1,B.JUMIN2 , B.TEL, A.SUCODE, A.NAL , A.GBINFO,a.Res                                          ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OORDER A, " + ComNum.DB_PMPA + "BAS_PATIENT B, " + ComNum.DB_MED + "OCS_DOCTOR C, " + ComNum.DB_PMPA + "OPD_MASTER D";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                                                     ";
                SQL += ComNum.VBLF + "      AND A.BDATE =TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')                                               ";
                SQL += ComNum.VBLF + "      AND A.GBSUNAP NOT IN ('1','2')                                                                          ";
                SQL += ComNum.VBLF + "      AND A.SUCODE IS NOT NULL                                                                                ";
                SQL += ComNum.VBLF + "      AND A.SEQNO <> '0'                                                                                      ";
                SQL += ComNum.VBLF + "      AND A.PTNO <>'81000004'                                                                                 ";
                SQL += ComNum.VBLF + "      AND A.OrderCode <> 'NSA'                                                                                ";  //2009-09-07 윤조연 추가
                SQL += ComNum.VBLF + "      AND (SUBSTR(A.SUCODE,1,2) NOT IN ('$$','##' ) or  decode(D.JIN,'L','',A.SUCODE) IN ('$$12','$$13' ))                                                             ";
                if (optGB0.Checked == true)
                {
                    SQL += ComNum.VBLF + "  AND A.BUN >='26' AND A.BUN <='40'                                                                       "; //처치, 수술
                }
                if (optSunap1.Checked == true)
                {
                    SQL += ComNum.VBLF + "  AND  a.GBAUTOSEND2 ='2'                                                                                 ";
                }
                else if (optSunap2.Checked == true)
                {
                    SQL += ComNum.VBLF + "  AND  a.Ptno in ( SELECT PANO FROM KOSMOS_PMPA.OPD_MASTER                                                ";
                    SQL += ComNum.VBLF + "                    WHERE ACTDATE =TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')                           ";
                    SQL += ComNum.VBLF + "                    AND GBGAMEK IN ('11')  )                                                              ";
                }

                SQL += ComNum.VBLF + "      AND A.DRCODE = C.DRCODE(+)                                                                              ";
                SQL += ComNum.VBLF + "      AND A.PTNO = B.PANO(+)                                                                                  ";
                SQL += ComNum.VBLF + "      AND A.PTNO = D.PANO(+)                                                                              ";
                SQL += ComNum.VBLF + "      AND A.BDATE = D.BDATE(+)                                                                                  ";
                SQL += ComNum.VBLF + "      AND A.DEPTCODE = D.DEPTCODE(+)                                                                                  ";
                SQL += ComNum.VBLF + "ORDER BY A.DEPTCODE, A.PTNO                                                                                   ";
            }

            else
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                                                        ";
                SQL += ComNum.VBLF + "  A.ORDERCODE, A.DEPTCODE, A.GBINFO, C.DRNAME, B.SNAME, A.PTNO,                                               ";
                SQL += ComNum.VBLF + "  B.SEX, B.JUMIN1,B.JUMIN2 , B.TEL, A.SUCODE, A.NAL,'0' Res                                                   ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_iORDER A, " + ComNum.DB_PMPA + "BAS_PATIENT B, " + ComNum.DB_MED + "OCS_DOCTOR C";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                                                     ";
                SQL += ComNum.VBLF + "      AND A.BDATE =TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')                                               ";
                SQL += ComNum.VBLF + "      AND A.GBAct = '*'                                                                                       ";
                SQL += ComNum.VBLF + "      AND A.SUCODE IS NOT NULL                                                                                ";
                SQL += ComNum.VBLF + "      AND A.SEQNO <> '0'                                                                                      ";
                SQL += ComNum.VBLF + "      AND A.GbIOE ='E'                                                                                        ";
                SQL += ComNum.VBLF + "      AND A.PTNO <>'81000004'                                                                                 ";
                SQL += ComNum.VBLF + "      AND A.OrderCode <> 'NSA'                                                                                ";  //2009-09-07 윤조연 추가
                if (optGB0.Checked == true)
                {
                    SQL += ComNum.VBLF + "  AND A.BUN >='26' AND A.BUN <='40'                                                                       "; //처치, 수술
                }
                SQL += ComNum.VBLF + "      AND A.DRCODE = C.DRCODE(+)                                                                              ";
                SQL += ComNum.VBLF + "      AND A.PTNO = B.PANO(+)                                                                                  ";
                SQL += ComNum.VBLF + "ORDER BY 2,7                                                                                                  ";
            }

            try
            {

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            if (dt.Rows.Count > 0)
            {
               // nCount = 0;
                

                strPano = "";
                strRes = "";

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strOK = "";
                    ssList_Sheet1.Rows.Count += 1;

                    if (optSunap2.Checked == true)
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT ";
                        SQL += ComNum.VBLF + "  PANO";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_AUTO_MST";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND Pano ='" + dt.Rows[i]["PTNO"].ToString().Trim() + "'";
                        SQL += ComNum.VBLF + "      AND DELDATE IS NULL";
                        SQL += ComNum.VBLF + "      AND GUBUN ='1'";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count == 0)
                        {
                            dt1.Dispose();
                            dt1 = null;
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            strOK = "OK";
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }
                    else
                    {
                        strOK = "OK";
                    }

                    if (strOK == "OK")
                    {
                        if (strPano != dt.Rows[i]["PTNO"].ToString().Trim())
                        {
                            nCount += 1;
                            strPano = dt.Rows[i]["PTNO"].ToString().Trim();

                            ssList_Sheet1.Cells[nCount - 1, 0].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                            ssList_Sheet1.Cells[nCount - 1, 1].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                            ssList_Sheet1.Cells[nCount - 1, 2].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                            ssList_Sheet1.Cells[nCount - 1, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                            ssList_Sheet1.Cells[nCount - 1, 4].Text = dt.Rows[i]["SEX"].ToString().Trim() + "/" +
                                                                      ComFunc.AgeCalcEx(dt.Rows[i]["JUMIN1"].ToString().Trim() + dt.Rows[i]["JUMIN2"].ToString().Trim(), CurrentDate);
                            ssList_Sheet1.Cells[nCount - 1, 5].Text = dt.Rows[i]["TEL"].ToString().Trim();
                        }

                        strGbinfor = dt.Rows[i]["GBINFO"].ToString().Trim();
                        strOrdercode = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                        strNal = Convert.ToInt32(VB.Val(dt.Rows[i]["Nal"].ToString().Trim()));
                        strCode = "(" + strNal + ")" + dt.Rows[i]["SUCODE"].ToString().Trim();
                        strRes = "";

                        if (dt.Rows[i]["Res"].ToString().Trim() == "1")
                        {
                            strRes = "[예약검사]";
                        }

                        if (dt.Rows[i]["gbinfo"].ToString().Trim() != "")
                        {
                            //2021-07-28 매핑수가 적용
                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT";
                            SQL += ComNum.VBLF + "  SUCODE";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_SUBCODE";
                            SQL += ComNum.VBLF + "WHERE 1=1";
                            SQL += ComNum.VBLF + "      AND ORDERCODE ='" + strOrdercode + "'";
                            SQL += ComNum.VBLF + "      AND SUBNAME = '" + strGbinfor + "'";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                clsOrdFunction OF = new clsOrdFunction();

                                strCode = "(" + strNal + ")" + dt1.Rows[0]["SUCODE"].ToString().Trim();
                                //if (clsType.User.Sabun == "21403")
                                if (CF.Read_Bcode_Name(clsDB.DbCon, "SUNAP_수가매핑시행여부", "USE") == "Y")
                                {
                                    strCode = "(" + strNal + ")" + OF.Mapping_SuCode(clsDB.DbCon, strOrdercode, dt1.Rows[0]["SUCODE"].ToString().Trim(), strGbinfor,
                                                                       dtpDate.Text.Trim(), dt.Rows[i]["DEPTCODE"].ToString().Trim());
                                }
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }

                        ssList_Sheet1.Cells[nCount - 1, 6].Text += " " + strCode + " " + strRes;

                        ssList_Sheet1.Rows[nCount - 1].Height = ssList_Sheet1.Rows[nCount - 1].GetPreferredHeight() + 5;
                    }
                }



            }
            ssList_Sheet1.Rows.Count = nCount;

            dt.Dispose();
            dt = null;

            btnPrint.Visible = true;
            btnPrint.Focus();


        }

    }
}
