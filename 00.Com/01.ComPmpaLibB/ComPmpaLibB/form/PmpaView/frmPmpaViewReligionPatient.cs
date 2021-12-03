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
    /// File Name       : frmPmpaViewReligionPatient.cs
    /// Description     : 종교별 환자 조회
    /// Author          : 안정수
    /// Create Date     : 2017-10-18
    /// Update History  : 
    /// <history>       
    /// d:\psmh\OPD\wonmok\wonmok03.frm(FrmWonMok03) => frmPmpaViewReligionPatient.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\wonmok\wonmok03.frm(FrmWonMok03)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewReligionPatient : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();
        int mnJobSabun = 0;

        public frmPmpaViewReligionPatient()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewReligionPatient(int GnJobSabun)
        {
            InitializeComponent();
            setEvent();
            mnJobSabun = GnJobSabun;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnCancel.Click += new EventHandler(eBtnEvent);
            this.btnSave.Click += new EventHandler(eBtnEvent);
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

            optSort0.Checked = true;

            Set_Combo();
        }

        void Set_Combo()
        {
            cboReligion.Items.Clear();

            cboReligion.Items.Add("1.천주교");
            cboReligion.Items.Add("2.개신교");
            cboReligion.Items.Add("3.불  교");
            cboReligion.Items.Add("4.천도교");
            cboReligion.Items.Add("5.유  교");
            cboReligion.Items.Add("6.무  교");
            cboReligion.Items.Add("9.기  타");

            cboReligion.SelectedIndex = 0;

            Load_Bi_IDs();
        }

        void Load_Bi_IDs()
        {
            clsPmpaPb.strBis[11] = "공단";
            clsPmpaPb.strBis[12] = "직장";
            clsPmpaPb.strBis[13] = "지역";
            clsPmpaPb.strBis[14] = "";
            clsPmpaPb.strBis[15] = "";

            clsPmpaPb.strBis[21] = "보호1";
            clsPmpaPb.strBis[22] = "보호2";
            clsPmpaPb.strBis[23] = "보호3";
            clsPmpaPb.strBis[24] = "행려";
            clsPmpaPb.strBis[25] = "";

            clsPmpaPb.strBis[31] = "산재";
            clsPmpaPb.strBis[32] = "공상";
            clsPmpaPb.strBis[33] = "산재공상";
            clsPmpaPb.strBis[34] = "";
            clsPmpaPb.strBis[35] = "";

            clsPmpaPb.strBis[41] = "공단100%";
            clsPmpaPb.strBis[42] = "직장100%";
            clsPmpaPb.strBis[43] = "지역100%";
            clsPmpaPb.strBis[44] = "가족계획";
            clsPmpaPb.strBis[45] = "보험계약";

            clsPmpaPb.strBis[51] = "일반";
            clsPmpaPb.strBis[52] = "TA 보험";
            clsPmpaPb.strBis[53] = "계약";
            clsPmpaPb.strBis[54] = "미확인";
            clsPmpaPb.strBis[55] = "TA 일반";
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnCancel)
            {
                cboReligion.Focus();
                ssList_Sheet1.Rows.Count = 0;
            }

            else if (sender == this.btnSave)
            {
                if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eSaveData();
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


            #endregion

            strTitle = "입원환자 LIST";
            string strTitle2 = "조회구분: " + cboReligion.SelectedItem.ToString();

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strTitle2, new Font("굴림체", 13, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력시간:" + VB.Now().ToString() + VB.Space(15) + "PAGE:" + "/p", new Font("굴림체", 11), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 70, 30);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, false, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            #endregion
        }

        void eSaveData()
        {
            int i = 0;
            string strROWID = "";
            string strCHANGE = "";

            string strSERENAME = "";
            string strBONDANG = "";
            string strPANO = "";

            string strOK = "";

            strOK = "OK";

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;


            clsDB.setBeginTran(clsDB.DbCon);

            for (i = 0; i < ssList_Sheet1.Rows.Count; i++)
            {
                strPANO = ssList_Sheet1.Cells[i, 0].Text.Trim();
                strSERENAME = ssList_Sheet1.Cells[i, 2].Text.Trim();
                strBONDANG = ssList_Sheet1.Cells[i, 7].Text.Trim();
                strCHANGE = ssList_Sheet1.Cells[i, 13].Text.Trim();
                strROWID = ssList_Sheet1.Cells[i, 14].Text.Trim();

                if (strCHANGE == "Y")
                {
                    if (strROWID == "")
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "ETC_WONMOK_SIN";
                        SQL += ComNum.VBLF + "(PANO, SERENAME, BONDANG, WRITEDATE, WRITESABUN)";
                        SQL += ComNum.VBLF + "VALUES(";
                        SQL += ComNum.VBLF + "'" + strPANO + "',            ";
                        SQL += ComNum.VBLF + "'" + strSERENAME + "',        ";
                        SQL += ComNum.VBLF + "'" + strBONDANG + "',         ";
                        SQL += ComNum.VBLF + "SYSDATE, " + mnJobSabun + "  )";
                    }
                    else
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "ETC_WONMOK_SIN " + "SET";
                        SQL += ComNum.VBLF + "SERENAME = '" + strSERENAME + "',";
                        SQL += ComNum.VBLF + "BONDANG = '" + strBONDANG + "' ";
                        SQL += ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'";
                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        strOK = "NO";
                        break;
                    }
                }
            }

            if (strOK == "NO")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 중 에러 발생");
            }
            else
            {
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                eGetData();
            }
        }

        void eGetData()
        {
            int i = 0;
            int j = 0;
            string strBi = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            CS.Spread_All_Clear(ssList);

            ssList_Sheet1.Rows.Count = 0;

            if (VB.Left(cboReligion.SelectedItem.ToString(), 1) == "1")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT";
                SQL += ComNum.VBLF + "  WardCode,RoomCode,I.Pano,Bi,Sname,Pname,Sex,Age,E.SERENAME, E.BONDANG, E.ROWID,";
                SQL += ComNum.VBLF + "  TO_CHAR(InDate, 'yyyy-mm-dd') InDate,DeptCode,DrName,Religion, G.GAMSNAME, G.GAMROMAN";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER i, " + ComNum.DB_PMPA + "BAS_DOCTOR K, ";
                SQL += ComNum.VBLF + ComNum.DB_PMPA + "ETC_WONMOK_SIN E, " + ComNum.DB_PMPA + "BAS_GAMFSINGA G";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND i.DrCode = k.DrCode";
                SQL += ComNum.VBLF + "      AND i.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND I.PANO = E.PANO(+)";
                SQL += ComNum.VBLF + "      AND I.PANO = G.GAMPANO(+)";
                SQL += ComNum.VBLF + "      AND i.GBSTS  = '0'";
                SQL += ComNum.VBLF + "      AND (RELIGION  = '1' OR RELIGION IS NULL)";
            }

            else
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT";
                SQL += ComNum.VBLF + "  WardCode,RoomCode,I.Pano,Bi,Sname,Pname,Sex,Age,E.SERENAME, E.BONDANG, E.ROWID,";
                SQL += ComNum.VBLF + "  TO_CHAR(InDate, 'yyyy-mm-dd') InDate,DeptCode,DrName,Religion";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER i, " + ComNum.DB_PMPA + "BAS_DOCTOR K, ";
                SQL += ComNum.VBLF + ComNum.DB_PMPA + "ETC_WONMOK_SIN E, " + ComNum.DB_PMPA + "BAS_GAMFSINGA G";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND i.DrCode = k.DrCode";
                SQL += ComNum.VBLF + "      AND i.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND I.PANO = E.PANO(+)";
                SQL += ComNum.VBLF + "      AND I.PANO = G.GAMPANO(+)";
                SQL += ComNum.VBLF + "      AND i.GBSTS  = '0'";
                if (VB.Left(cboReligion.SelectedItem.ToString(), 1) == "9")
                {
                    SQL += ComNum.VBLF + "  AND RELIGION NOT IN ('1','2','3','4','5','6')";
                }
                else
                {
                    SQL += ComNum.VBLF + "  AND RELIGION = '" + VB.Left(cboReligion.SelectedItem.ToString(), 1) + "'";
                }
            }

            if (optSort0.Checked == true)
            {
                SQL += ComNum.VBLF + "ORDER BY ROOMCODE,SNAME";
            }

            else if (optSort1.Checked == true)
            {
                SQL += ComNum.VBLF + "ORDER BY INDATE, SNAME";
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
                    //ssList_Sheet1.Rows.Count = dt.Rows.Count + 15;
                    ssList_Sheet1.Rows.Count = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strBi = dt.Rows[i]["Bi"].ToString().Trim();

                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SERENAME"].ToString().Trim();

                        if (ssList_Sheet1.Cells[i, 2].Text == "" && VB.Left(cboReligion.SelectedItem.ToString(), 1) == "1")
                        {
                            ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["GAMSNAME"].ToString().Trim();
                        }

                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = clsPmpaPb.strBis[Convert.ToInt32(strBi)];

                        switch (dt.Rows[i]["Religion"].ToString().Trim())
                        {
                            case "1":
                                ssList_Sheet1.Cells[i, 6].Text = "1.천주교";
                                break;
                            case "2":
                                ssList_Sheet1.Cells[i, 6].Text = "2.개신교";
                                break;
                            case "3":
                                ssList_Sheet1.Cells[i, 6].Text = "3.불　교";
                                break;
                            case "4":
                                ssList_Sheet1.Cells[i, 6].Text = "4.천도교";
                                break;
                            case "5":
                                ssList_Sheet1.Cells[i, 6].Text = "5.유　교";
                                break;
                            case "6":
                                ssList_Sheet1.Cells[i, 6].Text = "6.무　교";
                                break;

                            default:
                                ssList_Sheet1.Cells[i, 6].Text = "9.기　타";
                                break;
                        }

                        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["BONDANG"].ToString().Trim();
                        if (ssList_Sheet1.Cells[i, 7].Text == "" && VB.Left(cboReligion.SelectedItem.ToString(), 1) == "1")
                        {
                            ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["GAMROMAN"].ToString().Trim();
                        }

                        ssList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["Age"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["InDate"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 11].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 12].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 14].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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

        }

        void ssList_Change(object sender, ChangeEventArgs e)
        {
            int a = 0;

            a = ssList_Sheet1.ActiveRowIndex;
            ssList_Sheet1.Cells[a, 13].Text = "Y";

            ssList_Sheet1.Rows[a].ForeColor = Color.Red;
        }

        void ssList_LeaveCell(object sender, LeaveCellEventArgs e)
        {
            int i = 0;
            int nREAD = 0;
            int nRow = 0;

            string strPANO = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (ssList_Sheet1.Cells[e.Row, 14].Text != "")
            {
                return;
            }

            if (e.Column == 0)
            {
                strPANO = ComFunc.SetAutoZero(ssList_Sheet1.Cells[e.Row, 0].Text, 8);
            }

            for (i = 0; i < ssList_Sheet1.Rows.Count; i++)
            {
                if (i == nRow)
                {

                }

                else
                {
                    if (ssList_Sheet1.Cells[i, 0].Text == strPANO && ssList_Sheet1.Cells[i, 0].Text != "")
                    {
                        ComFunc.MsgBox("입력하신 환자번호는 이미 등록이 되어 있습니다. 상단에서 찾아보시기 바랍니다.");
                    }
                }
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  SNAME, PANO, WARDCODE, ROOMCODE,";
            SQL += ComNum.VBLF + "  TO_CHAR(INDATE,'YYYY-MM-DD') INDATE, DEPTCODE, DRCODE, BI, AGE, SEX";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
            SQL += ComNum.VBLF + "WHERE PANO = '" + strPANO + "'";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssList_Sheet1.Cells[e.Row, 0].Text = dt.Rows[0]["PANO"].ToString().Trim();
                    ssList_Sheet1.Cells[e.Row, 1].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ssList_Sheet1.Cells[e.Row, 3].Text = dt.Rows[0]["WARDCODE"].ToString().Trim();
                    ssList_Sheet1.Cells[e.Row, 4].Text = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    ssList_Sheet1.Cells[e.Row, 5].Text = dt.Rows[0]["BI"].ToString().Trim();
                    ssList_Sheet1.Cells[e.Row, 8].Text = dt.Rows[0]["AGE"].ToString().Trim();
                    ssList_Sheet1.Cells[e.Row, 9].Text = dt.Rows[0]["SEX"].ToString().Trim();
                    ssList_Sheet1.Cells[e.Row, 10].Text = dt.Rows[0]["INDATE"].ToString().Trim();
                    ssList_Sheet1.Cells[e.Row, 11].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    ssList_Sheet1.Cells[e.Row, 12].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[0]["DRCODE"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

        }
    }
}
