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


namespace ComLibB
{
    public partial class frmPatientSearch2 : Form
    {
        string GstrHelpCode = "";

        public frmPatientSearch2()
        {
            InitializeComponent();
        }

        private void frmPatientSearch2_Load(object sender, EventArgs e)
        {
            btnCancel1.Enabled = false;
            btnCancel2.Enabled = false;
            btnCancel3.Enabled = false;

            ComFunc.ReadSysDate(clsDB.DbCon);
        }

        private DataTable PATIENT_SQL()
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            SQL = "  SELECT  ";
            SQL = SQL + " Pano,Sname,Sex,Jumin1,Jumin2,Jumin3,Pname,Bi,Tel,TO_CHAR(LastDate,'YY-MM-DD') Lday,DeptCode ";
            SQL = SQL + " FROM BAS_PATIENT ";
            switch (superTabControl1.SelectedTabIndex)
            {
                case 0:
                    SQL = SQL + " WHERE Sname Like '" + txtSname.Text + "%' ";
                    break;
                case 1:
                    if (VB.Trim(txtJumin1.Text) != "")
                    {
                        SQL = SQL + " WHERE Jumin1 = '" + txtJumin1.Text + "'";
                        if (VB.Trim(txtJumin2.Text) != "")
                        {
                            SQL = SQL + " and Jumin3 = '" + clsAES.AES(VB.Trim(txtJumin2.Text)) + "' ";
                        }
                    }
                    else
                    {
                        if (VB.Trim(txtJumin2.Text) != "")
                        {
                            SQL = SQL + " WHERE Jumin3 = '" + clsAES.AES(VB.Trim(txtJumin2.Text)) + "'";
                        }
                    }
                    break;
                case 2:
                    SQL = SQL + " WHERE PANO = '" + txtBarCode.Text.Trim() + "' ";
                    break;
            }
            SQL = SQL + " ORDER BY Sname,Jumin1,Jumin2 ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return null;
            }

            return dt;
        }

        private void btnSearch1_Click(object sender, EventArgs e)
        {
            if (txtSname.Text.Length >= 2)
            {
                ChartNo_Display();
            }
            txtSname.Focus();
            txtSname.SelectAll();
        }

        private void txtSname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtSname.Text.Length >= 2)
                {
                    ChartNo_Display();
                }
                txtSname.Focus();
                txtSname.SelectAll();
            }
        }

        private void ChartNo_Display()
        {
            int i = 0;
            DataTable dt = null;            
            string strJumin1 = "";
            string strJumin2 = "";

            ssIpd_Sheet1.RowCount = 0;

            try
            {                
                dt = PATIENT_SQL();
                
                if (dt.Rows.Count > 0)
                {
                    ssIpd_Sheet1.RowCount = dt.Rows.Count;
                    ssIpd_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssIpd_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssIpd_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                        ssIpd_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        strJumin1 = dt.Rows[i]["Jumin1"].ToString().Trim();
                        if (dt.Rows[i]["Jumin3"].ToString().Trim() != "")
                        {
                            strJumin2 = clsAES.DeAES(dt.Rows[i]["Jumin3"].ToString().Trim());
                        }
                        else
                        {
                            strJumin2 = dt.Rows[i]["Jumin2"].ToString().Trim();
                        }
                        ssIpd_Sheet1.Cells[i, 3].Text = strJumin1 + "-" + strJumin2;
                        ssIpd_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Pname"].ToString().Trim();
                        ssIpd_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Bi"].ToString().Trim();
                        ssIpd_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Tel"].ToString().Trim();
                        ssIpd_Sheet1.Cells[i, 7].Text = dt.Rows[i]["LDay"].ToString().Trim();
                        ssIpd_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;                
                ComFunc.MsgBox(ex.Message);
            }

            ss99_Sheet1.RowCount = 0;
        }

        private void txtJumin1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtJumin1.Text.Length >= 6)
                {
                    ChartNo_Display();
                }
                txtJumin1.Focus();
                txtJumin1.SelectAll();
            }
        }

        private void txtJumin2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtJumin2.Text.Length == 7)
                {
                    ChartNo_Display();
                }
                txtJumin2.Focus();
                txtJumin2.SelectAll();
            }
        }

        private void btnSearch2_Click(object sender, EventArgs e)
        {
            if (txtJumin1.Text.Length >= 6)
            {
                ChartNo_Display();
            }
            txtJumin1.Focus();
            txtJumin1.SelectAll();

            if (txtJumin2.Text.Length == 7)
            {
                ChartNo_Display();
            }
            txtJumin2.Focus();
            txtJumin2.SelectAll();
        }

        private void txtBarCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtBarCode.Text = VB.Left(txtBarCode.Text, 8);
                txtBarCode.Text = ComFunc.SetAutoZero(txtBarCode.Text, 8);
                ChartNo_Display();
                txtBarCode.Focus();
                txtBarCode.SelectAll();
            }
        }

        private void btnSearch3_Click(object sender, EventArgs e)
        {
            txtBarCode.Text = VB.Left(txtBarCode.Text, 8);
            txtBarCode.Text = ComFunc.SetAutoZero(txtBarCode.Text, 8);
            ChartNo_Display();
            txtBarCode.Focus();
            txtBarCode.SelectAll();
        }

        private void Read_RESV_Disp(string strPano)
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                ss99_Sheet1.RowCount = 0;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT SName,Pano,DeptCode,DrCode,TO_CHAR(Date3,'YYYY-MM-DD') RDate,TO_CHAR(Date3,'HH24:MI') RTime ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_RESERVED_NEW";
                SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND Date3 >= TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "   AND TRANSDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND RETDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY Date3,DeptCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ss99_Sheet1.RowCount = dt.Rows.Count;
                    ss99_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss99_Sheet1.Cells[i, 0].Text = dt.Rows[i]["RDate"].ToString().Trim();
                        ss99_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RTime"].ToString().Trim();
                        ss99_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ss99_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ss99_Sheet1.Cells[i, 4].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void superTabControl1_SelectedTabChanged(object sender, DevComponents.DotNetBar.SuperTabStripSelectedTabChangedEventArgs e)
        {
            ssIpd_Sheet1.RowCount = 0;
            ss99_Sheet1.RowCount = 0;
            switch (superTabControl1.SelectedTabIndex)
            {
                case 0:
                    txtSname.Text = "";
                    txtSname.Focus();
                    break;
                case 1:
                    txtJumin1.Text = "";
                    txtJumin2.Text = "";
                    txtJumin1.Focus();
                    break;
                case 2:
                    txtBarCode.Text = "";
                    txtBarCode.Focus();
                    break;
            }
        }

        private void ssIpd_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            string strPANO = "";
            string strSname = "";

            if (e.Row < 0) return;
            if (e.Column == 9)
            {
                strPANO = ssIpd_Sheet1.Cells[e.Row, 0].Text;
                strSname = ssIpd_Sheet1.Cells[e.Row, 1].Text;

                if (strPANO != "")
                {
                    prtBarCode(strPANO, strSname);
                }

                if (superTabControl1.SelectedTabIndex == 2)
                {
                    txtBarCode.Text = "";
                    txtBarCode.Focus();
                }
            }
        }

        private void prtBarCode(string strPano, string strSName)
        {
            string Prdata = "";
            string mstrPrintName = "혈액환자정보";
            string strPrintName1 = "";
            string strPrintName2 = "";

            clsPrint CP = new clsPrint();

            try
            {
                strPrintName1 = clsPrint.gGetDefaultPrinter();
                strPrintName2 = CP.getPrinter_Chk(mstrPrintName.ToUpper());

                if (strPrintName2 == "")
                {
                    ComFunc.MsgBox("프린터 설정 오류입니다. 의료정보과에 연락바랍니다.");
                    return;
                }


                Prdata = "^XA";
                Prdata = Prdata + "^LH0,0^FS";
                Prdata = Prdata + "^SEE:UHANGUL.DAT^FS";
                Prdata = Prdata + "^CW1,E:KFONT3.FNT^FS";
                Prdata = Prdata + "^FO85,50^BY2,2:1";                                     //' 바코드 인쇄 (10자리)
                Prdata = Prdata + "^B3N,N,80,N,N";                  //'Barcode Type: Code 39 (SubSets A,B and C)
                Prdata = Prdata + "^FD" + VB.Trim(strPano) + "^FS";
                Prdata = Prdata + "^FO90,140^CI26^A1N,25,25";
                Prdata = Prdata + "^FD" + strPano + " / " + VB.Left(strSName, 1) + "O" + VB.Right(strSName, 1) + "^FS";
                Prdata = Prdata + "^XZ";

                String s = Prdata;
                
                if (s.Length > 10)
                {
                    try
                    {
                        ComPrintApi.SendStringToPrinter(strPrintName2, s);
                    }
                    catch (Exception ex)
                    {
                        ComFunc.MsgBox(ex.ToString());
                        return;
                    }
                }
                else
                {
                    return;
                }                
            }
            catch (Exception Ex)
            {
                System.Windows.Forms.MessageBox.Show(Ex.ToString());
                return;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssIpd_Click(object sender, EventArgs e)
        {
            
        }

        private void ssIpd_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            Read_RESV_Disp(ssIpd_Sheet1.Cells[e.Row, 0].Text);
        }

        private void ssIpd_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strPANO = "";
            string strSname = "";
            string strSex = "";
            string strBirth = "";
            string strJumin = "";
            
            string strDeptCode = "";
            string strDrCode = "";
            string strSex2 = "";


            GstrHelpCode = "";

            if (ssIpd_Sheet1.Cells[e.Row, 0].Text == "") return;
            GstrHelpCode = ssIpd_Sheet1.Cells[e.Row, 0].Text + "{}";    //'등록번호
            strPANO = ssIpd_Sheet1.Cells[e.Row, 0].Text;

            GstrHelpCode = GstrHelpCode + ssIpd_Sheet1.Cells[e.Row, 1].Text + "{}";    //'성명
            strSname = ssIpd_Sheet1.Cells[e.Row, 1].Text;

            GstrHelpCode = GstrHelpCode + ssIpd_Sheet1.Cells[e.Row, 2].Text + "{}";    //'성별
            if (ssIpd_Sheet1.Cells[e.Row, 2].Text == "M") strSex = "남";
            if (ssIpd_Sheet1.Cells[e.Row, 2].Text == "F") strSex = "여";

            GstrHelpCode = GstrHelpCode + ssIpd_Sheet1.Cells[e.Row, 3].Text + "{}";    //'주민번호
            strJumin = ssIpd_Sheet1.Cells[e.Row, 3].Text;

            switch (VB.Mid(strJumin, 8, 1))
            {
                case "1":
                case "2":
                case "5":
                case "6":
                    strBirth = "19";
                    break;
                case "3":
                case "4":
                case "7":
                case "8":
                    strBirth = "20";
                    break;
                case "9":
                case "0":
                    strBirth = "18";
                    break;
            }
            strBirth = strBirth + VB.Left(VB.Trim(strJumin), 2) + "." + VB.Mid(VB.Trim(strJumin), 4, 2) + "." + VB.Mid(VB.Trim(strJumin), 5, 2);
            GstrHelpCode = GstrHelpCode + ssIpd_Sheet1.Cells[e.Row, 5].Text + "{}";     //'환자종류
            GstrHelpCode = GstrHelpCode + ssIpd_Sheet1.Cells[e.Row, 6].Text + "{}";     //'전화번호


            if (e.Column == 10)
            {
                ComFunc.ReadSysDate(clsDB.DbCon);

                clsPublic.GstrRetValue = strPANO;
                frmSelectJupsuList frmSelectJupsuListX = new frmSelectJupsuList();
                frmSelectJupsuListX.StartPosition = FormStartPosition.CenterParent;
                frmSelectJupsuListX.ShowDialog();

                strDeptCode = VB.Trim(VB.Pstr(clsPublic.GstrRetValue, "^^", 1));
                strDrCode = VB.Trim(VB.Pstr(clsPublic.GstrRetValue, "^^", 2));

                if (strDeptCode == "")
                {
                    ComFunc.MsgBox("당일접수 내역을 선택후 작업하세요!!");
                    return;
                }
                else
                {
                    if (strSex == "남")
                    {
                        strSex2 = "M";
                    }
                    else
                    {
                        strSex2 = "F";
                    }

                    //'xray 수동접수
                    INSERT_PACS_CD_ORDER(strPANO, strSname, strSex2, strJumin, strBirth, strDeptCode, strDrCode);

                    CD_SHELL(strPANO, strSname, strSex, strBirth);
                }
            }
        }

        private bool INSERT_PACS_CD_ORDER(string ArgPano, string ArgSName, string ArgSex, string ArgJumin, string ArgBirth, string ArgDept, string ArgDrCode)
        {
            int nPacsNo = 0;
            int nAge = 0;
            string strPacsNo = "";

            bool rtnVal = false;
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                nAge = (int)VB.Val(clsVbfunc.READ_AGE_GESAN(clsDB.DbCon, ArgPano));


                SQL = " SELECT PANO ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.XRAY_DETAIL";
                SQL = SQL + ComNum.VBLF + "  WHERE BDATE =TRUNC(SYSDATE)";
                SQL = SQL + ComNum.VBLF + "   AND XCODE ='PACS-C' ";
                SQL = SQL + ComNum.VBLF + "   AND GB_MANUAL='Y' ";
                SQL = SQL + ComNum.VBLF + "   AND Remark ='자동생성' ";
                SQL = SQL + ComNum.VBLF + "   AND DeptCode ='" + ArgDept + "' ";
                SQL = SQL + ComNum.VBLF + "   AND Pano ='" + ArgPano + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    
                    nPacsNo = READ_PACSNO_2();  //'PACS용 Accession Number를 만듬
                    strPacsNo = clsPublic.GstrSysDate.Replace("-", "") + VB.Format(nPacsNo, "0000").ToString();


                    SQL = "  INSERT INTO XRAY_DETAIL(Enterdate, IpdOpd,GbReserved, SeekDate,BDate, Pano,";
                    SQL = SQL + ComNum.VBLF + " Sname,Sex,Age,Deptcode, Drcode,WardCode, Roomcode, Xcode,Qty,";
                    SQL = SQL + ComNum.VBLF + " ExID,XJong,XSubCode, XrayRoom,   Gbngt, Remark,PacsNo, OrderCode,OrderName , ";
                    SQL = SQL + ComNum.VBLF + " AGREE,GbSTS,GB_MANUAL,Gbinfo) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + "  SYSDATE,'O','7',SYSDATE,TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + " '" + ArgPano + "','" + ArgSName + "','" + ArgSex + "'," + nAge + ",'" + ArgDept + "',";
                    SQL = SQL + ComNum.VBLF + " '" + ArgDrCode + "','','','PACS-C',1,0,'9','01','X','',";
                    SQL = SQL + ComNum.VBLF + " '자동생성','" + strPacsNo + "','0060463','Pacs에 저장(CD)', '','1','Y','' )";
                        
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }



                    SQL = " INSERT INTO XRAY_PACSSEND (EntDate,PacsNo,SendGbn,Pano,SName,";
                    SQL = SQL + ComNum.VBLF + " Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode,";
                    SQL = SQL + ComNum.VBLF + " XCode,OrderCode,SeekDate,Remark,XRayRoom,XRayName,Gbinfo) ";
                    SQL = SQL + ComNum.VBLF + " VALUES (SYSDATE,'" + strPacsNo + "','1','" + ArgPano + "','" + ArgSName + "', ";
                    SQL = SQL + ComNum.VBLF + "  '" + ArgSex + "'," + nAge + ",'O','" + ArgDept + "','" + ArgDrCode + "',";
                    SQL = SQL + ComNum.VBLF + "  '','','9', ";
                    SQL = SQL + ComNum.VBLF + "  '01','PACS-C','0060463',";
                    SQL = SQL + ComNum.VBLF + "  SYSDATE,'자동생성', ";
                    SQL = SQL + ComNum.VBLF + "  'X','Pacs에 저장(CD)','') ";


                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }                    
                }
                
                clsDB.setCommitTran(clsDB.DbCon);
                //ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void CD_SHELL(string ArgPano, string ArgSName, string ArgSex, string ArgBirth)
        {
            string strCom = "";
            
            if (ArgPano == "") return;
            if (VB.Len(ArgPano) != 8) return;
            
            strCom = @"D:\영상CDTEST\DICOMCS.EXE " + "" + ArgPano + "" + " " + "" + ArgSName + "" + " " + "" + ArgSex + "" + " " + "" + ArgBirth + "";
            VB.Shell(strCom, "vbMaximizedFocus");
            
            //'    영상CD관련 인터페이스 내역
            //'
            //'타입은 실행파일 뒤에 "고객번호" "고객명" "성별" "생년월일"을 붙여서 실행하시면 됩니다.
            //'
            //'아래는 그에 대한 예제입니다.
            //'
            //'patientid:  고객번호
            //'patientName:  이름
            //'patientSex:  성별
            //'PatientBirth: 생년월일
            //'
            //'예) DicomCS.exe "81000004" "테스트" "남" "2016.01.01"
            //'                 고객번호   이름    성별   생년월일
        }

        private int READ_PACSNO_2()
        {
            int rtnVal = -1;  
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "SELECT SEQ_PACSNO.NEXTVAL NextPacsNo FROM DUAL";                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = (int)VB.Val(dt.Rows[0]["NextPacsNo"].ToString());
                }                
                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

    }
}
