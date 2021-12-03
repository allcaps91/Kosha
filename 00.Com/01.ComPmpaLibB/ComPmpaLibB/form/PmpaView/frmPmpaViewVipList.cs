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
    /// File Name       : frmPmpaViewVipList.cs
    /// Description     : 재원환자 VIP 명단 관리
    /// Author          : 안정수
    /// Create Date     : 2017-08-09
    /// Update History  : 2017-10-23
    /// 출력 부분 수정
    /// <history>       
    /// d:\psmh\Etc\csinfo\Frm재원환자_VIP명단.frm(Frm재원환자_VIP명단) => frmPmpaViewVipList.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\Etc\csinfo\Frm재원환자_VIP명단.frm(Frm재원환자_VIP명단)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewVipList : Form, MainFormMessage
    {
        string mstrJobName = "";
        ComFunc CF = new ComFunc();

        #region MainFormMessage InterFace

        public MainFormMessage mCallForm = null;

        public void MsgActivedForm(Form frm)
        {

        }

        public void MsgUnloadForm(Form frm)
        {

        }

        public void MsgFormClear()
        {

        }

        public void MsgSendPara(string strPara)
        {

        }

        #endregion

        public frmPmpaViewVipList(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmPmpaViewVipList()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewVipList(string JobName)
        {
            InitializeComponent();
            mstrJobName = JobName;
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);

            this.optGubun0.CheckedChanged += new EventHandler(eBtnEvent);
            this.optGubun1.CheckedChanged += new EventHandler(eBtnEvent);

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

            optGubun0.Checked = true;
        }

        void Set_Combo()
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0;

            ssList_Sheet1.Rows.Count = 0;

            //VIP
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                         ";
            SQL += ComNum.VBLF + "  Code,Name                                    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE           ";
            SQL += ComNum.VBLF + "WHERE 1=1                                      ";
            SQL += ComNum.VBLF + "      AND Gubun='BAS_VIP_구분코드2'            ";
            SQL += ComNum.VBLF + "      AND DELDATE IS NULL                      ";
            SQL += ComNum.VBLF + "ORDER BY Code                                  ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                }


                if (dt.Rows.Count > 0)
                {
                    cboVip.Items.Clear();
                    cboVip.Items.Add("**.전체");

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboVip.Items.Add(dt.Rows[i]["Code"].ToString().Trim() + "." + dt.Rows[i]["Name"].ToString().Trim());
                    }
                }
                else
                {
                    ComFunc.MsgBox("조회된 DATA가 존재하지 않습니다.");
                }
            }

            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;

            cboVip.SelectedIndex = 0;
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int i = 0;

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

            else if (sender == this.optGubun0)
            {
                // VIP
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                        ";
                SQL += ComNum.VBLF + "  Code,Name                                                                   ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE                                          ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
                SQL += ComNum.VBLF + "      AND GUBUN ='BAS_VIP_구분코드2'                                          ";
                SQL += ComNum.VBLF + "      AND DELDATE IS NULL                                                     ";
                SQL += ComNum.VBLF + "ORDER BY Code                                                                 ";

                try
                {
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    }

                    if (dt.Rows.Count > 0)
                    {
                        cboVip.Items.Clear();
                        cboVip.Items.Add("**.전체");
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            cboVip.Items.Add(dt.Rows[i]["Code"].ToString().Trim() + "." + dt.Rows[i]["Name"].ToString().Trim());
                        }
                    }
                }

                catch (System.Exception ex)
                {
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }

                dt.Dispose();
                dt = null;
                cboVip.SelectedIndex = 0;

            }

            else if (sender == this.optGubun1)
            {
                // VIP
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                        ";
                SQL += ComNum.VBLF + "  Code,Name                                                                   ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE                                          ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
                SQL += ComNum.VBLF + "      AND GUBUN ='BAS_VIP_구분코드'                                           ";
                SQL += ComNum.VBLF + "      AND DELDATE IS NULL                                                     ";
                SQL += ComNum.VBLF + "ORDER BY Code                                                                 ";

                try
                {
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    }

                    if (dt.Rows.Count > 0)
                    {
                        cboVip.Items.Clear();
                        cboVip.Items.Add("**.전체");
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            cboVip.Items.Add(dt.Rows[i]["Code"].ToString().Trim() + "." + dt.Rows[i]["Name"].ToString().Trim());
                        }
                    }
                }

                catch (System.Exception ex)
                {
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }

                dt.Dispose();
                dt = null;
                cboVip.SelectedIndex = 0;
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
            string strGuBun = "";
            bool PrePrint = true;

            strGuBun = VB.Pstr(cboVip.SelectedItem.ToString().Trim(), ".", 2);

            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;


            #endregion

            strTitle = "VIP 재원자명부(" + strGuBun + ")";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 120, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            #endregion
        }

        void eGetData()
        {
            int i = 0;
            int nRead = 0;
            int nRow = 0;

            string strJuso = "";
            string strGubun = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strGubun = VB.Left(cboVip.SelectedItem.ToString(), 2);

            ssList_Sheet1.Rows.Count = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  a.Pano,b.SName,a.Bi,a.DeptCode,a.DrCode,a.WardCode,a.RoomCode,a.Sex,a.GbGamek,                              ";
            SQL += ComNum.VBLF + "  a.Age,a.Religion,TO_CHAR(a.InDate,'YYYY-MM-DD') InDate,TO_CHAR(a.OutDate,'YYYY-MM-DD') OutDate,b.Jumin1,b.Jumin2,b.Jumin3,  ";
            SQL += ComNum.VBLF + "  b.ZipCode1,b.ZipCode2,b.Juso,b.Tel,b.Remark,a.IPDNO,b.Gb_VIP,b.Gb_VIP_Remark,b.Gb_VIP2,b.Gb_VIP2_Reamrk     ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a," + ComNum.DB_PMPA + "BAS_PATIENT b                               ";

            if (chkAll.Checked == true)
            {
                SQL += ComNum.VBLF + "WHERE 1=1                                                                                                 ";
                //SQL += ComNum.VBLF + "      AND  a.Age,a.Religion,TO_CHAR(a.InDate,'YYYY-MM-DD') InDate,                                        ";
                SQL += ComNum.VBLF + "      AND  a.OutDate IS NULL                                                                              ";
            }

            else
            {
                SQL += ComNum.VBLF + "WHERE 1=1                                                                                                 ";
                SQL += ComNum.VBLF + "      AND  a.InDate>=TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD')                                        ";
                SQL += ComNum.VBLF + "      AND  a.InDate<TO_DATE('" + Convert.ToDateTime(dtpTdate.Text).AddDays(1).ToShortDateString() + "','YYYY-MM-DD') ";

                if (chkOut.Checked == false)
                {
                    SQL += ComNum.VBLF + "      AND  ((A.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD')                                             ";
                    SQL += ComNum.VBLF + "      AND  a.OutDate IS NULL) OR a.OutDate =TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD'))            ";
                }
            }

            SQL += ComNum.VBLF + "      AND a.GbSTS <> '9'                                                                                      "; //입원취소는 제외
            SQL += ComNum.VBLF + "      AND a.Pano=b.Pano(+)                                                                                    ";

            if (optGubun0.Checked == true)
            {
                if (strGubun == "**")
                {
                    SQL += ComNum.VBLF + "      AND  TRIM(b.GB_VIP2) IN ( SELECT TRIM(CODE) FROM KOSMOS_PMPA.BAS_BCODE WHERE GUBUN ='BAS_VIP_구분코드2' AND DELDATE IS NULL ) ";
                }
                else
                {
                    SQL += ComNum.VBLF + "      AND  b.GB_VIP2 ='" + strGubun + "'                                                              ";
                }
            }

            else if (optGubun1.Checked == true)
            {
                if (strGubun == "**")
                {
                    SQL += ComNum.VBLF + "      AND  TRIM(b.GB_VIP) IN ( SELECT TRIM(CODE) FROM KOSMOS_PMPA.BAS_BCODE WHERE GUBUN ='BAS_VIP_구분코드' AND DELDATE IS NULL )   ";
                }
                else
                {
                    SQL += ComNum.VBLF + "      AND  b.GB_VIP ='" + strGubun + "'                                                               ";
                }
            }

            SQL += ComNum.VBLF + "      AND a.Pano <> '81000004'                                                                                ";
            SQL += ComNum.VBLF + "ORDER BY a.InDate DESC ,b.SName,a.Pano                                                                        ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;
                    nRow = 0;
                    ssList_Sheet1.Rows.Count = nRead;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nRow += 1;
                        if (nRow > ssList_Sheet1.Rows.Count)
                        {
                            ssList_Sheet1.Rows.Count = nRow;
                        }

                        ssList_Sheet1.Cells[nRow - 1, 0].Text = nRow.ToString();
                        ssList_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["InDate"].ToString().Trim();

                        ssList_Sheet1.Cells[nRow - 1, 4].Text = CF.Read_Bcode_Name(clsDB.DbCon, "BAS_환자종류", dt.Rows[i]["Bi"].ToString().Trim());
                        ssList_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 6].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                        ssList_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();

                        if (optGubun0.Checked == true)
                        {
                            ssList_Sheet1.Cells[nRow - 1, 9].Text = Read_VIP_Gubun("1", dt.Rows[i]["Gb_Vip2"].ToString().Trim());
                            ssList_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["Gb_VIP2_Reamrk"].ToString().Trim();
                        }
                        else if (optGubun1.Checked == true)
                        {
                            ssList_Sheet1.Cells[nRow - 1, 9].Text = Read_VIP_Gubun("2", dt.Rows[i]["Gb_Vip"].ToString().Trim());
                            ssList_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["Gb_VIP_Remark"].ToString().Trim();
                        }

                        ssList_Sheet1.Cells[nRow - 1, 11].Text = "";
                        ssList_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["GbGamek"].ToString().Trim();

                        if (dt.Rows[i]["OutDate"].ToString().Trim() != "")
                        {
                            ssList_Sheet1.Cells[nRow - 1, 13].Text = "Y";
                        }
                    }
                }
            }

            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;

            ssList_Sheet1.Rows.Count = nRow;

            btnPrint.Enabled = true;

        }

        public string Read_VIP_Gubun(string ArgJob, string ArgCode)
        {
            string rtnVal = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (ArgJob == "1")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                        ";
                SQL += ComNum.VBLF + "  NAME                                                                        ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE                                          ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
                SQL += ComNum.VBLF + "      AND GUBUN ='BAS_VIP_구분코드2'                                          ";
                SQL += ComNum.VBLF + "      AND TRIM(CODE) ='" + ArgCode + "'                                       ";
            }
            else if (ArgJob == "2")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                        ";
                SQL += ComNum.VBLF + "  NAME                                                                        ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE                                          ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
                SQL += ComNum.VBLF + "      AND GUBUN ='BAS_VIP_구분코드'                                           ";
                SQL += ComNum.VBLF + "      AND TRIM(CODE) ='" + ArgCode + "'                                       ";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["NAME"].ToString().Trim();
                }
            }

            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;
            return rtnVal;
        }

    }
}
