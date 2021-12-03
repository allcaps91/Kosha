using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : PC환경설정
/// Author : 김형범
/// Create Date : 2017.06.28
/// </summary>
/// <history>
/// App.EXEName, Print Print DB로 관리
/// </history>
namespace ComLibB
{
    /// <summary> PC환경설정 </summary>
    public partial class frmPcSET : Form
    {
        int GnJobSabun = 0;
        int nx = 0;
        int nY = 0;

        /// <summary> PC환경설정 </summary>
        public frmPcSET()
        {
            InitializeComponent();
        }

        public frmPcSET(int intJobSabun, int x, int y)
        {
            InitializeComponent();

            GnJobSabun = intJobSabun;
            nx = x;
            nY = y;
        }

        ////TODO: App.EXEName "xuagfa"c
        void frmPcSET_Load(object sender, EventArgs e)
        {


            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등


            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int i = 0;

            txtIpAddress.Text = "";
            cboWard.Items.Clear();
            cboDept.Items.Clear();
            cboDoct.Items.Clear();
            txtBuCode.Text = "";
            txtBuName.Text = "";
            txtnX.Text = "0";
            txtnY.Text = "0";

            //TODO: App.EXEName
            //if (App.EXEName == "xuagfa")
            //{
            //    clsQuery.READ_PC_CONFIG(1);     //IP 주소 는 INI 파일읽어서 처리
            //}
            //else
            //{
            //    clsQuery.READ_PC_CONFIG(2);     //IP 주소는 REGEDIT  읽어서 처리
            //}

            clsQuery.READ_PC_CONFIG(clsDB.DbCon);

            cboJob.Items.Clear();
            cboJob.Items.Add("1.등록번호별");
            cboJob.Items.Add("2.당일외래접수");
            cboJob.Items.Add("3.재원환자");
            cboJob.Items.Add("5.응급실재원자");
            cboJob.Items.Add("6.당일입원");
            cboJob.Items.Add("7.종합검진");
            cboJob.Items.Add("8.일반검진");
            cboJob.Items.Add("9.촬영일자별");
            cboJob.Items.Add("A.영상누락자");
            cboJob.Items.Add("B.초음파검사");
            cboJob.Items.Add("C.수술예정자");
            cboJob.SelectedIndex = 0;

            if (clsType.PC_CONFIG.Job != "")
            {
                ComFunc.ComboFind(cboJob, "L", 1, clsType.PC_CONFIG.Job);
            }

            try
            {
                //병동코드
                SQL = "";
                SQL = "SELECT WardCode ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_WARD";
                SQL = SQL + ComNum.VBLF + " ORDER BY WardCode ";

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
                cboWard.Items.Clear();
                cboWard.Items.Add("**");

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                cboWard.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }


            //진료과 ComboBox SET
            SQL = "";
            SQL = "SELECT DeptCode  ";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
            SQL = SQL + ComNum.VBLF + " WHERE DeptCode NOT IN ('II','PT') ";
            SQL = SQL + ComNum.VBLF + " ORDER BY PrintRanking ";

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

            cboDept.Items.Clear();
            cboDept.Items.Add("**");

            for (i = 0; i < dt.Rows.Count; i++)
            {
                cboDept.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;

            txtIpAddress.Text = clsType.PC_CONFIG.IPAddress;

            // 없을경우 regedit 에서 ip read

            if (txtIpAddress.Text.Trim() == "")
            {
                txtIpAddress.Text = clsCompuInfo.gstrCOMIP;
            }

            if (clsType.PC_CONFIG.BuseGbn == "1")
            {
                rdoBuse0.Checked = true;
            }
            else if (clsType.PC_CONFIG.BuseGbn == "2")
            {
                rdoBuse1.Checked = true;
            }
            else
            {
                rdoBuse2.Checked = true;
            }

            cboWard.Text = clsType.PC_CONFIG.WardCode;

            if (clsType.PC_CONFIG.DeptCode != "")
            {
                for (i = 0; i < cboDept.Items.Count; i++)
                {
                    if (cboDept.Items[i].ToString().Trim() == clsType.PC_CONFIG.DeptCode.Trim())
                    {
                        cboDept.SelectedIndex = i;
                        break;
                    }

                }

                cboDept_SelectedIndexChanged(sender, e);

                //의사코드 SET

                //for (i = 0; i < cboDept.SelectedIndex; i++)
                //{
                //    if (VB.Left(cboDept.SelectedIndex.ToString(), 4) == clsType.PC_CONFIG.DeptCode)
                //    {
                //        cboDept.SelectedIndex = i;
                //    }
                //}

                ComFunc.ComboFind(cboDoct, "L", 4, clsType.PC_CONFIG.DrCode);
            }

            txtBuCode.Text = clsType.PC_CONFIG.BuCode;
            txtBuName.Text = ReadBuseName(clsType.PC_CONFIG.BuCode);

            switch (clsType.PC_CONFIG.PacsSW)
            {
                case "임상용":
                    rdoPacs0.Checked = true;
                    break;
                case "Web1000":
                    rdoPacs1.Checked = true;
                    break;
                case "판독용":
                    rdoPacs2.Checked = true;
                    break;
                case "사용불가":
                    rdoPacs3.Checked = true;
                    break;
            }

            switch (clsType.PC_CONFIG.CrtSize)
            {
                case "800x600":
                    rdoSize0.Checked = true;
                    break;
                case "1024x768":
                    rdoSize1.Checked = true;
                    break;
                case "1280x1024":
                    rdoSize2.Checked = true;
                    break;
            }

            chkUser.Checked = false;

            if (clsType.PC_CONFIG.PcUserYN == "Y")
            {
                chkUser.Checked = true;
            }

            txtWebUser.Text = clsType.PC_CONFIG.PacsID;
            txtWebPass.Text = clsType.PC_CONFIG.PacsPass;
            txtRemark.Text = clsType.PC_CONFIG.Remark;


            //사용중인 PC의 OS Ver.
            cboOS.Items.Clear();
            cboOS.Items.Add("1.MS-DOS");
            cboOS.Items.Add("2.Windows 3.1");
            cboOS.Items.Add("3.Windows 98");
            cboOS.Items.Add("4.Windows 2000");
            cboOS.Items.Add("5.Windows XP");
            cboOS.SelectedIndex = 0;

            if (clsType.PC_CONFIG.OS_Ver != "")
            {
                ComFunc.ComboFind(cboOS, "L", 1, VB.Left(clsType.PC_CONFIG.OS_Ver, 1));
            }

            //바코드 설정
            if (clsType.PC_CONFIG.BarCode == "1")
            {
                rdoBar0.Checked = true; //기존 시리얼방식
            }
            else if (clsType.PC_CONFIG.BarCode == "2")
            {
                rdoBar1.Checked = true; //Z4M USB 드라이브방식
            }
            else if (clsType.PC_CONFIG.BarCode == "3")
            {
                rdoBar2.Checked = true; //GX420
            }
            else if (clsType.PC_CONFIG.BarCode == "4")
            {
                rdoBar3.Checked = true; //clp621
            }

            txtnX.Text = clsType.PC_CONFIG.nx.ToString();
            txtnY.Text = clsType.PC_CONFIG.nY.ToString();

            txtGx420dX.Text = clsType.PC_CONFIG.GX420D_X.ToString();
            txtGx420dY.Text = clsType.PC_CONFIG.GX420D_Y.ToString();
        }

        //부서명 READ
        string ReadBuseName(string ArgCode)
        {
            string strVal = "";

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return strVal; //권한 확인
            }

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = "SELECT Sname ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_BUSE ";
                SQL = SQL + ComNum.VBLF + " WHERE BuCode = '" + VB.Val(ArgCode).ToString("000000") + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return strVal;
                }

                if (dt.Rows.Count == 1)
                {
                    strVal = dt.Rows[0]["Sname"].ToString().Trim();
                }
                else
                {
                    strVal = "";
                }

                dt.Dispose();
                dt = null;

                return strVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strVal;
            }
        }

        //TODO: Print DB로 관리
        void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string strBuseGbn = "";
            string strPacs = "";
            string strCrtSize = "";
            string strUserYN = "";
            string strROWID = "";
            string strBarCode = "";

            string strGx420d = "";
            int intGx420d_X = 0;
            int intGx420d_Y = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int intRowAffected = 0;

            txtIpAddress.Text = txtIpAddress.Text.Trim();

            if (txtIpAddress.Text == "")
            {
                ComFunc.MsgBox("PC의 IP번호가 공란입니다.", "확인");
                return;
            }

            strBuseGbn = "";

            if (rdoBuse0.Checked == true)
            {
                strBuseGbn = "1";
            }

            if (rdoBuse1.Checked == true)
            {
                strBuseGbn = "2";
            }

            if (rdoBuse2.Checked == true)
            {
                strBuseGbn = "3";
            }

            if (strBuseGbn == "")
            {
                ComFunc.MsgBox("사용부서를 선택하지 않았습니다.", "확인");
                return;
            }

            txtBuCode.Text = txtBuCode.Text.Trim();

            if (txtBuCode.Text == "")
            {
                ComFunc.MsgBox("부서코드를 입력하지 않았습니다.", "확인");
                return;
            }

            if (cboJob.Text == "")
            {
                ComFunc.MsgBox("작업방법을 선택하지 않았습니다.", "확인");
                return;
            }

            strPacs = "";

            if (rdoPacs0.Checked == true)
            {
                strPacs = "임상용";
            }

            if (rdoPacs1.Checked == true)
            {
                strPacs = "Web1000";
            }

            if (rdoPacs2.Checked == true)
            {
                strPacs = "판독용";
            }

            if (rdoPacs3.Checked == true)
            {
                strPacs = "사용불가";
            }

            if (strPacs == "")
            {
                ComFunc.MsgBox("PACS S/W 설치 구분을 선택하지 않았습니다,", "확인");
                return;
            }

            if (rdoSize0.Checked == true)
            {
                strCrtSize = "800x600";
            }

            if (rdoSize1.Checked == true)
            {
                strCrtSize = "1024x768";
            }

            if (rdoSize2.Checked == true)
            {
                strCrtSize = "1280x1024";
            }

            strUserYN = "N";

            if (chkUser.Checked == true)
            {
                strUserYN = "Y";
            }

            if (rdoBar0.Checked == true)
            {
                strBarCode = "1";
            }

            if (rdoBar1.Checked == true)
            {
                strBarCode = "2";
            }

            strGx420d = "0";

            if (rdoBar2.Checked == true)
            {
                strGx420d = "1";
                strBarCode = "3";
            }

            if (rdoBar3.Checked == true)
            {
                strBarCode = "4";
            }

            intGx420d_X = Convert.ToInt32(txtGx420dX.Text);
            intGx420d_Y = Convert.ToInt32(txtGx420dY.Text);

            //TODO: DB로 관리
            //Open "C:\PC_CONFIG.ini" For Output As #1
            //Print #1, "IPAddress="; TxtIpAddress.Text
            //Print #1, "부서성격="; strBuseGbn
            //Print #1, "병동="; Trim(ComboWard.Text)
            //Print #1, "진료과="; Trim(ComboDept.Text)
            //Print #1, "의사="; Trim(ComboDoct.Text)
            //Print #1, "부서명="; TxtBuCode.Text; ";"; PanelBuName.Caption
            //Print #1, "PACS_SW="; strPacs
            //Print #1, "화면크기="; strCrtSize
            //Print #1, "작업방법="; Trim(ComboJob.Text)
            //Print #1, "고정User="; strUserYN
            //Print #1, "고정ID="; Trim(TxtWebUser.Text)
            //Print #1, "고정Pass="; Trim(TxtWebPass.Text)
            //Print #1, "참고사항="; Trim(TxtRemark.Text)
            //Print #1, "PC_OS="; Trim(ComboOS.Text)
            //Print #1, "BarCode="; strBarCode
            //Close #1

            //이미 자료가 있는지 Check
            try
            {
                SQL = "";
                SQL = "SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "ETC_PCCONFIG ";
                SQL = SQL + ComNum.VBLF + " WHERE IpAddress='" + txtIpAddress.Text.Trim() + "' ";

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

                strROWID = dt.Rows[0]["ROWID"].ToString().Trim();

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                if (strROWID == "")
                {
                    if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                    {
                        return; //권한 확인
                    }

                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_PCCONFIG ";
                    SQL = SQL + ComNum.VBLF + "(IPADDRESS,BUSEGBN,WARDCODE,DEPTCODE,DRCODE,BUCODE,OS_VER,PACSSW,CRTSIZE,";
                    SQL = SQL + ComNum.VBLF + " Job,PcUserYN,PacsID,PacsPass,Remark,ENTDATE,ENTSABUN, BarCode , NX, NY, GX420D, GX420D_X, GX420D_Y) VALUES ('";
                    SQL = SQL + ComNum.VBLF + txtIpAddress.Text.Trim() + "','" + strBuseGbn + "','" + VB.Left(cboWard.Text, 2).Trim() + "','";
                    SQL = SQL + ComNum.VBLF + cboDept.Text.Trim() + "','" + VB.Left(cboDoct.Text, 4).Trim() + "','";
                    SQL = SQL + ComNum.VBLF + txtBuCode.Text.Trim() + "','" + cboOS.Text.Trim() + "','";
                    SQL = SQL + ComNum.VBLF + strPacs + "','" + strCrtSize + "','" + VB.Left(cboJob.Text, 1).Trim() + "','";
                    SQL = SQL + ComNum.VBLF + strUserYN + "','" + txtWebUser.Text.Trim() + "','" + txtWebPass.Text.Trim() + "','";
                    SQL = SQL + ComNum.VBLF + txtRemark.Text.Trim() + "',SYSDATE," + GnJobSabun + " ,  '" + strBarCode + "' , ";
                    SQL = SQL + ComNum.VBLF + " '" + nx + "', '" + nY + "', '" + strGx420d + "', '" + intGx420d_X + "', '" + intGx420d_Y + "'   ) ";
                }
                else
                {
                    if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                    {
                        return; //권한 확인
                    }

                    SQL = "";
                    SQL = "UPDATE " + ComNum.DB_PMPA + "ETC_PCCONFIG SET ";
                    SQL = SQL + ComNum.VBLF + "BuseGbn = '" + strBuseGbn + "',";
                    SQL = SQL + ComNum.VBLF + "WardCode = '" + VB.Left(cboWard.Text, 2).Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "DeptCode = '" + cboDept.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "DrCode = '" + VB.Left(cboDoct.Text, 4).Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "BuCode = '" + txtBuCode.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "OS_Ver = '" + cboOS.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "PacsSW = '" + strPacs + "',";
                    SQL = SQL + ComNum.VBLF + "CrtSize = '" + strCrtSize + "',";
                    SQL = SQL + ComNum.VBLF + "Job = '" + VB.Left(cboJob.Text, 1).Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "PcUserYN = '" + strUserYN + "',";
                    SQL = SQL + ComNum.VBLF + "PacsID = '" + txtWebUser.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "PacsPass = '" + txtWebPass.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "Remark = '" + txtRemark.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "EntDate = SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + "EntSabun = " + GnJobSabun + ", ";
                    SQL = SQL + ComNum.VBLF + "BarCode = '" + strBarCode + "' , ";
                    SQL = SQL + ComNum.VBLF + " NX = '" + txtnX.Text + "', ";
                    SQL = SQL + ComNum.VBLF + " NY = '" + txtnY.Text + "', ";
                    SQL = SQL + ComNum.VBLF + "  GX420D = '" + strGx420d + "', ";
                    SQL = SQL + ComNum.VBLF + "  GX420D_X = '" + intGx420d_X + "' ,";
                    SQL = SQL + ComNum.VBLF + "  GX420D_Y = '" + intGx420d_Y + "' ";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            clsQuery.READ_PC_CONFIG(clsDB.DbCon); //다시 변경한 내용을 변수에 저장함

            this.Close();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void txtBuCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void txtBuCode_Leave(object sender, EventArgs e)
        {
            txtBuCode.Text = VB.UCase(txtBuCode.Text).Trim();

            if (txtBuCode.Text == "")
            {
                txtBuName.Text = "";
                return;
            }

            txtBuCode.Text = BuseName2Code(txtBuCode.Text.Trim());

            txtBuName.Text = ReadBuseName(txtBuCode.Text);

            if (txtBuName.Text == "")
            {
                ComFunc.MsgBox("사용부서 코드가 오류 입니다.", "오류");
                txtBuCode.Text = "";
                txtBuCode.Focus();
                return;
            }
        }

        //부서명으로 부서코드 찾기
        string BuseName2Code(string ArgName)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            string strVal = "";

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return strVal; //권한 확인
            }

            //찾을코드는 반드시 2자이상
            if (VB.Len(ArgName) < 2)
            {
                strVal = ArgName;
                return strVal;
            }
            //부서코드는 숫자로만 구성됨
            if (VB.IsNumeric(ArgName) == true)
            {
                strVal = ArgName;
                return strVal;
            }

            try
            {
                SQL = "";
                SQL = "SELECT BuCode ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_BUSE ";
                SQL = SQL + ComNum.VBLF + " WHERE SName LIKE '" + ArgName + "%' ";
                SQL = SQL + ComNum.VBLF + "   AND DelDate IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND Jas='*' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["BuCode"].ToString().Trim();
                }
                else
                {
                    strVal = "";
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            return strVal;
        }

        void txtIpAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            string strDeptCode = "";
            string strDoct = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strDeptCode = cboDept.Text.Trim();

            if (strDeptCode == "")
            {
                cboDept.Text = "";
                return;
            }

            try
            {
                //해당과의 의사를 Setting
                SQL = "";
                SQL = "SELECT DrCode,DrName ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_DOCTOR";
                SQL = SQL + ComNum.VBLF + " WHERE Tour = 'N' ";
                SQL = SQL + ComNum.VBLF + "   AND (DrDept1='" + strDeptCode + "' OR DrDept2='" + strDeptCode + "') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY PrintRanking ";

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

                cboDoct.Items.Clear();
                cboDoct.Items.Add("****.전체");

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strDoct = dt.Rows[i]["DrCode"].ToString().Trim() + ".";
                    strDoct = strDoct + dt.Rows[i]["DrName"].ToString().Trim();
                    cboDoct.Items.Add(strDoct);
                }

                cboDoct.SelectedIndex = 0;

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            SendKeys.Send("{Tab}");

        }

        void cboDept_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void cboDoct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void cboWard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cboDept.Focus();
            }
        }
    }
}
