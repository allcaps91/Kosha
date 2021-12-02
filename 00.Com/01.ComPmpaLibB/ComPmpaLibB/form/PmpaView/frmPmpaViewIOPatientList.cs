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
    /// File Name       : frmPmpaViewIOPatientList.cs
    /// Description     : 입원,외래환자명단 조회
    /// Author          : 안정수
    /// Create Date     : 2017-10-19
    /// Update History  : 2017-11-17
    /// <history>       
    /// d:\psmh\OPD\wonmok\wonmok04.frm(FrmWonMok04) => frmPmpaViewIOPatientList.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\wonmok\wonmok04.frm(FrmWonMok04)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewIOPatientList : Form
    {
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        public frmPmpaViewIOPatientList()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnCancel.Click += new EventHandler(eBtnEvent);

            this.optGbn0.CheckedChanged += new EventHandler(eOpt_Changed);
            this.optGbn1.CheckedChanged += new EventHandler(eOpt_Changed);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등 

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            optGbn0.Checked = true;

            ComboWard_SET();
        }

        void ComboWard_SET()
        {
            int i = 0;
            int j = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  WardCode, WardName";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_WARD";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND WARDCODE NOT IN ('IU','NP','2W','NR','DR','IQ','ER')";
            SQL += ComNum.VBLF + "ORDER BY WardCode";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            cboWard.Items.Clear();
            cboWard.Items.Add("전체");

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                }
            }

            cboWard.Items.Add("SICU");
            cboWard.Items.Add("MICU");

            dt.Dispose();
            dt = null;

            cboWard.SelectedIndex = 0;
        }

        void eOpt_Changed(object sender, EventArgs e)
        {
            if (sender == this.optGbn0)
            {
                groupBox1.Visible = true;
            }

            else if (sender == this.optGbn1)
            {
                groupBox1.Visible = false;
            }
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnCancel)
            {
                CS.Spread_All_Clear(ssList);
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
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
            ssList.ActiveSheet.Cells[0, ssList_Sheet1.Columns.Count - 1].Text = "　";


            #endregion

            if (optGbn0.Checked == true)
            {
                strTitle = "입원환자 LIST";
            }

            else
            {
                strTitle = "외래환자 LIST";
            }


            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력시간:" + VB.Now().ToString() + VB.Space(15) + "PAGE:" + "/p", new Font("굴림체", 11), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 70, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, false, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            #endregion
        }

        void eGetData()
        {
            int i = 0;
            int j = 0;

            string strBi = "";
            string strBName = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;
            Cursor.Current = Cursors.WaitCursor;

            string strJumin1 = "";
            string strJumin2 = "";
            string strJumin3 = "";

            string strTemp = "";

            CS.Spread_All_Clear(ssList);

            if (optGbn0.Checked == true)
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT";
                SQL += ComNum.VBLF + "  a.WardCode,a.RoomCode,a.Pano,a.Bi,a.Sname,a.Pname,a.Sex,a.Age,a.IPDNO, A.GBGAMEK,";
                SQL += ComNum.VBLF + "  TO_CHAR(a.InDate, 'YYYY-MM-DD') InDate,a.DeptCode,b.DrName,a.Religion, ";
                SQL += ComNum.VBLF + "  c.Tel,c.HPhone,c.Jumin1 , c.Jumin2, Jumin3, ZipCode1 || ZipCode2 ZipCode,Juso";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a, " + ComNum.DB_PMPA + "BAS_DOCTOR b, " + ComNum.DB_PMPA + "BAS_PATIENT c";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND a.DrCode = b.DrCode(+)";
                SQL += ComNum.VBLF + "      AND (a.OutDate IS NULL OR a.OutDate>=TO_DATE('" + Convert.ToDateTime(dtpDate.Text).AddDays(1).ToShortDateString() + "','YYYY-MM-DD'))";
                SQL += ComNum.VBLF + "      AND a.IpwonTime < TO_DATE('" + Convert.ToDateTime(dtpDate.Text).AddDays(1).ToShortDateString() + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND a.Amset4 <> '3' ";
                SQL += ComNum.VBLF + "      AND a.Pano < '90000000'";
                SQL += ComNum.VBLF + "      AND a.Pano <> '81000004'";
                SQL += ComNum.VBLF + "      AND a.Pano = c.Pano(+)";
                SQL += ComNum.VBLF + "      AND a.Jdate =to_date('1900-01-01','YYYY-MM-DD')";

                if (cboWard.SelectedItem.ToString() != "전체")
                {
                    if (cboWard.SelectedItem.ToString() == "SICU")
                    {
                        SQL += ComNum.VBLF + "  AND a.RoomCode ='233'";
                    }
                    else if (cboWard.SelectedItem.ToString() == "MICU")
                    {
                        SQL += ComNum.VBLF + "  AND a.RoomCode ='234'";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "  AND a.WARDCODE ='" + cboWard.SelectedItem.ToString() + "'";
                    }

                }

                SQL += ComNum.VBLF + "ORDER BY a.RoomCode,a.SName";
            }

            else
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT";
                SQL += ComNum.VBLF + "  a.Pano,a.Bi,a.Sname,a.Sex,a.Age, A.GBGAMEK,";
                SQL += ComNum.VBLF + "  TO_CHAR(a.ActDate, 'YYYY-MM-DD') ActDate,a.DeptCode,b.DrName,";
                SQL += ComNum.VBLF + "  c.Tel,c.HPhone,c.Jumin1 || c.Jumin2 Jumin,ZipCode1 || ZipCode2 ZipCode,Juso";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER a, " + ComNum.DB_PMPA + "BAS_DOCTOR b, " + ComNum.DB_PMPA + "BAS_PATIENT c";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND a.DrCode = b.DrCode(+)";
                SQL += ComNum.VBLF + "      AND a.Pano = c.Pano(+)";
                SQL += ComNum.VBLF + "      AND a.ActDate=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "ORDER BY a.SName,a.Pano";
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

                if (dt.Rows.Count > 0)
                {
                    ssList_Sheet1.Rows.Count = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                        if (optGbn0.Checked == true)
                        {
                            ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                            ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["RoomCode"].ToString().Trim();

                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT";
                            SQL += ComNum.VBLF + "  PANO,DIAGNOSIS";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_MASTER";
                            SQL += ComNum.VBLF + "WHERE 1=1";
                            SQL += ComNum.VBLF + "      AND IPDNO =" + dt.Rows[i]["IPDNO"].ToString().Trim() + " ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                ssList_Sheet1.Cells[i, 8].Text = dt1.Rows[0]["DIAGNOSIS"].ToString().Trim();
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }

                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Age"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DrName"].ToString().Trim();

                        ssList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["Tel"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["HPhone"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 11].Text = CF.READ_BAS_Mail(clsDB.DbCon, dt.Rows[i]["ZipCode"].ToString().Trim()) + " " + dt.Rows[i]["Juso"].ToString().Trim();

                        if (optGbn0.Checked == true)
                        {
                            strJumin1 = dt.Rows[i]["Jumin1"].ToString().Trim();
                            strJumin2 = dt.Rows[i]["Jumin2"].ToString().Trim();
                            strJumin3 = dt.Rows[i]["Jumin3"].ToString().Trim();

                            strJumin2 = clsAES.DeAES(strJumin3);

                            strJumin3 = clsAES.AES(strJumin1 + strJumin2);
                        }

                        //2017-11-17, 안정수
                        //외래로 조회시 Jumin1 칼럼이 없으므로, Jumin칼럼의 값을 저장하여 조회
                        else
                        {
                            strTemp = dt.Rows[i]["Jumin"].ToString().Trim();                            
                        }

                        

                        //if(dt.Rows[i]["pano"].ToString().Trim() == "06478183")
                        //{
                        //    i = i;
                        //}

                        if (dt.Rows[i]["GBGAMEK"].ToString().Trim() != "00" && dt.Rows[i]["GBGAMEK"].ToString().Trim() != "")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT";
                            SQL += ComNum.VBLF + "  GAMMESSAGE, GAMSABUN";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_GAMF";
                            SQL += ComNum.VBLF + "WHERE 1=1";   
                                                   
                            if(optGbn0.Checked == true)
                            {
                                SQL += ComNum.VBLF + "      AND gamjumin3 = '" + strJumin3 + "'";
                            }

                            else if (optGbn1.Checked == true)
                            {
                                SQL += ComNum.VBLF + "      AND gamjumin  = '" + strTemp + "'";
                            }

                            SQL += ComNum.VBLF + "      AND gamout is null";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                strBName = "";

                                if (dt1.Rows[0]["GAMSABUN"].ToString().Trim() != "")
                                {
                                    strBName = READ_BUSE(dt1.Rows[0]["GAMSABUN"].ToString().Trim());
                                }

                                if (strBName != "")
                                {
                                    ssList_Sheet1.Cells[i, 12].Text = strBName + ComNum.VBLF + dt1.Rows[0]["GAMMESSAGE"].ToString().Trim();
                                }
                                else
                                {
                                    ssList_Sheet1.Cells[i, 12].Text = dt1.Rows[0]["GAMMESSAGE"].ToString().Trim();
                                }
                            }

                            else
                            {
                                ssList_Sheet1.Cells[i, 12].Text = CF.Read_Bcode_Name(clsDB.DbCon, "BAS_감액코드명", dt.Rows[i]["GBGAMEK"].ToString().Trim());
                            }
                            ssList_Sheet1.SetRowHeight(i, Convert.ToInt32(ssList_Sheet1.GetPreferredRowHeight(i)) + 5);
                            dt1.Dispose();
                            dt1 = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }


        private string READ_BUSE(string strSABUN)
        {
            DataTable DtFunc = new DataTable();
            string SQL = String.Empty;
            string SqlErr = String.Empty;
            string strVal = String.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT A.NAME ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BUSE A, KOSMOS_ADM.INSA_MST B ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "    AND A.BUCODE = B.BUSE ";
                SQL += ComNum.VBLF + "    AND (B.SABUN = '" + VB.Right(strSABUN, 5) + "' ";
                SQL += ComNum.VBLF + "    OR B.SABUN = '" + strSABUN + "') ";
                SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, clsDB.DbCon);

                if (DtFunc.Rows.Count == 0)
                {
                    DtFunc.Dispose();
                    DtFunc = null;
                    return strVal;
                }

                if (DtFunc.Rows.Count == 1)
                    strVal = DtFunc.Rows[0]["NAME"].ToString().Trim();
                else
                    strVal = "";

                DtFunc.Dispose();
                DtFunc = null;

                return strVal;
            }
            catch (Exception ex)
            {
                strVal = "";
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return strVal;
            }
        }

    }
}
