using ComBase;
using ComBase.Controls;
using ComDbB;
using FarPoint.Win.Spread;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : SupDrst
    /// File Name       : frmSupDrstDrugMedicineList.cs
    /// Description     : 의약품 목록
    /// Author          : 이정현
    /// Create Date     : 2017-11-29
    /// <history> 
    /// 의약품 목록
    /// </history>
    /// <seealso>
    /// PSMH\drug\drinfo\Frm의약품목록.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drinfo\drinfo.vbp
    /// </vbp>
    /// </summary>
    public partial class frmSupDrstDrugMedicineList : Form
    {
        private FarPoint.Win.Spread.FpSpread ssSpread = null;

        public frmSupDrstDrugMedicineList()
        {
            InitializeComponent();
            setEvent();
        }

        private void frmSupDrstDrugMedicineList_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}

            ////폼 기본값 세팅 등
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y");

            ssSpread = ssView;
            ssView.Visible = true;
            ssView.Dock = DockStyle.Fill;

            //기존
            //cboGubun.Text = "";
            //cboGubun.Items.Clear();
            //cboGubun.Items.Add("A.마약류 목록");
            //cboGubun.Items.Add("B.고위험 의약품 목록");
            //cboGubun.Items.Add("C.고주의 의약품 목록");
            //cboGubun.Items.Add("D.필요 시 처방(P.R.N) 목록");
            //cboGubun.Items.Add("E.항혈전제 목록");
            //cboGubun.Items.Add("F.면역억제제");
            //cboGubun.Items.Add("G.구두/전화 처방 금지 의약품 목록");

            //2019-05-10 변경            
            cboGubun.Text = "";
            cboGubun.Items.Clear();
            cboGubun.Items.Add("A.마약류");
            cboGubun.Items.Add("B.고위험 의약품");
            cboGubun.Items.Add("C.고주의 의약품");
            cboGubun.Items.Add("D.필요시 처방(P.R.N)");
            cboGubun.Items.Add("E.항혈전제");
            cboGubun.Items.Add("F.면역억제제");
            cboGubun.Items.Add("G.구두/전화 처방 금지 의약품 목록");
            cboGubun.Items.Add("H.제한항생제");

            cboGubun2.Text = "";
            cboGubun2.Items.Clear();
            cboGubun2.Items.Add("*.전체");

            cboGubun3.Text = "";
            cboGubun3.Items.Clear();
            cboGubun3.Items.Add("*.전체");

            cboBun.Text = "";
            cboBun.Items.Clear();
        }

        void setEvent()
        {
            this.ssView.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssSame_Jusa.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssSame_Mouse.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssSame_Out.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssSimilar_Exterior.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssSimilar_Med.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssSimilar_Pronounce.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (sender == ssView)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(o, e.Column);
                    return;
                }
                if (e.RowHeader == true)
                {
                    return;
                }
            }                

        }

        private void cboGubun_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboBun.Text = "";
            cboBun.Items.Clear();
            
            cboGubun2.Text = "";
            cboGubun2.Items.Clear();

            cboGubun3.Text = "";
            cboGubun3.Items.Clear();

            switch (VB.Left(cboGubun.Text, 1))
            {
                case "A":
                    cboGubun2.Items.Add("*.전체");
                    cboGubun2.Items.Add("1.마약");
                    cboGubun2.Items.Add("2.향정신성의약품");
                    cboGubun3.Items.Add("*.전체");
                    cboBun.Items.Clear();
                    cboBun.Items.Add("전체");
                    cboBun.Items.Add("주사");
                    cboBun.Items.Add("경구");
                    cboBun.Items.Add("외용");
                    cboBun.SelectedIndex = 0;
                    break;
                case "B":
                    //기존
                    //cboGubun2.Items.Add("*.전체");
                    //cboGubun2.Items.Add("1.항암제 류");
                    //cboGubun2.Items.Add("2.고농도 전해질 류");
                    //cboGubun2.Items.Add("3.헤파린 주사제(고분자량 헤파린)");
                    //cboGubun2.Items.Add("4.인슐린 주사류");
                    //cboGubun3.Items.Add("*.전체");

                    //2019-05-10 수정
                    SETCOMBO_BASBCODEDRUG(clsDB.DbCon, cboGubun2, "A001", "0", 1);

                    //cboGubun2.Items.Add("*.전체");
                    //cboGubun2.Items.Add("1.중등도 진정 의약품");
                    //cboGubun2.Items.Add("2.항암제");
                    //cboGubun2.Items.Add("3.고농도 전해질 제제");
                    //cboGubun2.Items.Add("4.주사용 항혈전제");
                    //cboGubun2.Items.Add("5.주사용 인슐린 제제");
                    //cboGubun2.Items.Add("6.조영제");
                    cboGubun3.Items.Add("*.전체");
                    
                    break;
                case "C":
                    SETCOMBO_BASBCODEDRUG(clsDB.DbCon, cboGubun2, "A002", "0", 0);
                    //cboGubun2.Items.Add("1.냉장/냉동 보관이 필요한 의약품(백신제외)");
                    //cboGubun2.Items.Add("2.차광이 필요한 약품");
                    //cboGubun2.Items.Add("3.혼동주의 의약품");
                    //cboGubun2.Items.Add("4.백신");
                    cboGubun3.Items.Add("*.전체");
                    break;
                case "D":
                    cboGubun2.Items.Add("*.세부항목없음");
                    cboGubun3.Items.Add("*.세부항목없음");

                    cboBun.Items.Clear();
                    cboBun.Items.Add("전체");
                    cboBun.Items.Add("주사");
                    cboBun.Items.Add("경구");
                    cboBun.Items.Add("외용");
                    cboBun.SelectedIndex = 0;

                    break;
                case "E":
                    cboGubun2.Items.Add("*.세부항목없음");
                    cboGubun3.Items.Add("*.세부항목없음");

                    cboBun.Items.Clear();
                    cboBun.Items.Add("전체");
                    cboBun.Items.Add("주사");
                    cboBun.Items.Add("경구");
                    cboBun.Items.Add("외용");
                    cboBun.SelectedIndex = 0;

                    break;
                case "F":
                    cboGubun2.Items.Add("*.세부항목없음");
                    cboGubun3.Items.Add("*.세부항목없음");

                    cboBun.Items.Clear();
                    cboBun.Items.Add("전체");
                    cboBun.Items.Add("주사");
                    cboBun.Items.Add("경구");
                    cboBun.Items.Add("외용");
                    cboBun.SelectedIndex = 0;

                    break;
                case "G":
                    cboGubun2.Items.Add("*.세부항목없음");
                    cboGubun3.Items.Add("*.세부항목없음");
                    cboBun.Items.Clear();
                    cboBun.Items.Add("전체");
                    cboBun.Items.Add("주사");
                    cboBun.Items.Add("경구");
                    cboBun.Items.Add("외용");
                    cboBun.SelectedIndex = 0;
                    break;

                case "H":
                    SETCOMBO_Antibiotic(clsDB.DbCon, cboGubun2);

                    cboBun.Items.Clear();
                    cboBun.Items.Add("전체");
                    cboBun.Items.Add("주사");
                    cboBun.Items.Add("경구");
                    cboBun.Items.Add("외용");
                    cboBun.SelectedIndex = 0;
                    break;
            }

            cboGubun2.SelectedIndex = 0;
            cboGubun3.SelectedIndex = 0;
        }

        private void cboGubun2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strGubun = VB.Left(cboGubun.Text, 1);
            string strGubun2 = "";

            if ((strGubun == "B" || strGubun == "C") && VB.Left(cboGubun2.Text, 1) != "*")
            {
                strGubun2 = cboGubun2.Text.Split(',')[1];
            }
            else
            {
                strGubun2 = VB.Left(cboGubun2.Text, 1);
            }
            

            cboGubun3.Text = "";
            cboGubun3.Items.Clear();

            switch(strGubun)
            {
                case "A":
                case "D":
                case "E":
                case "F":
                case "G":
                case "H":
                    cboGubun3.Items.Add("*.세부항목없음");
                    break;
                //case "H":
                //    SETCOMBO_Antibiotic(clsDB.DbCon, cboGubun2, cboGubun2.SelectedItem.ToString());
                //    break;
                case "B":
                    cboBun.Items.Clear();
                    cboGubun3.Items.Add("*.세부항목없음");

                    //if (strGubun2 == "9714")
                    //{
                    //    cboBun.Items.Add("전체");
                    //    cboBun.Items.Add("주사");
                    //    cboBun.Items.Add("경구");
                    //    cboBun.SelectedIndex = 0;
                    //}
                    
                    cboBun.Items.Clear();
                    cboBun.Items.Add("전체");
                    cboBun.Items.Add("주사");
                    cboBun.Items.Add("경구");
                    cboBun.Items.Add("외용");
                    cboBun.SelectedIndex = 0;

                    break;
                case "C":
                    cboBun.Text = "";
                    cboBun.Items.Clear();

                    switch (strGubun2)
                    {
                        case "10721":
                            cboGubun3.Items.Add("*.전체");
                            cboGubun3.Items.Add("1.냉장 보관 의약품");
                            cboGubun3.Items.Add("2.냉동 보관 의약품");

                            cboBun.Items.Add("전체");
                            cboBun.Items.Add("주사");
                            cboBun.Items.Add("경구");
                            cboBun.Items.Add("외용");
                            cboBun.SelectedIndex = 0;
                            break;
                        case "10720":
                            cboGubun3.Items.Add("*.EFFECT세부항목없음");

                            cboBun.Items.Add("전체");
                            cboBun.Items.Add("주사");
                            cboBun.Items.Add("경구");
                            cboBun.Items.Add("외용");
                            cboBun.SelectedIndex = 0;
                            break;
                        case "9719":
                            SETCOMBO_BASBCODEDRUG(clsDB.DbCon, cboGubun3, "A002", "9719", 0);
                            //cboGubun3.Items.Add("1.유사의약품코드(code-alike)");
                            //cboGubun3.Items.Add("2.유사외관(look-alike)");
                            //cboGubun3.Items.Add("3.유사발음(sound-alike)");
                            //cboGubun3.Items.Add("4.동일성분/다함량 의약품");
                            break;
                        case "10722":
                            cboGubun3.Items.Add("*.세부항목없음");

                            cboBun.Items.Add("전체");
                            cboBun.Items.Add("주사");
                            cboBun.Items.Add("경구");
                            cboBun.Items.Add("외용");
                            cboBun.SelectedIndex = 0;
                            break;
                    }
                    break;
            }

            cboGubun3.SelectedIndex = 0;
        }

        private void cboGubun3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (VB.Left(cboGubun.Text, 1) == "C" && VB.Left(cboGubun2.Text, 1) == "3")
            {
                cboBun.Text = "";
                cboBun.Items.Clear();

                //if (VB.Left(cboGubun3.Text, 1) == "4")
                if (cboGubun3.Text.Split(',')[1] == "9777")
                {
                    cboBun.Items.Add("전체");
                    cboBun.Items.Add("주사");
                    cboBun.Items.Add("경구");
                    cboBun.Items.Add("외용");
                    cboBun.SelectedIndex = 0;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string strGubun2 = VB.Left(cboGubun2.Text, 1);
            string strGubun3 = VB.Left(cboGubun3.Text, 1);

            panMsg.Visible = true;
            ssView.Visible = false;
            ssView.Dock = DockStyle.None;
            ssSimilar_Exterior.Visible = false;
            ssSimilar_Exterior.Dock = DockStyle.None;
            ssSimilar_Pronounce.Visible = false;
            ssSimilar_Pronounce.Dock = DockStyle.None;
            ssSimilar_Med.Visible = false;
            ssSimilar_Med.Dock = DockStyle.None;
            ssSame_Jusa.Visible = false;
            ssSame_Jusa.Dock = DockStyle.None;
            ssSame_Mouse.Visible = false;
            ssSame_Mouse.Dock = DockStyle.None;
            ssSame_Out.Visible = false;
            ssSame_Out.Dock = DockStyle.None;
            ssSpread = null;

            switch (VB.Left(cboGubun.Text, 1))
            {
                case "A": READ_DATA_A(strGubun2); break;
                case "B": READ_DATA_B(strGubun2); break;
                case "C": READ_DATA_C(strGubun2, strGubun3); break;
                case "D": READ_DATA_D(strGubun2); break;
                case "E": READ_DATA_E(); break;
                case "F": READ_DATA_F(); break;
                case "G": READ_DATA_G(); break;
                case "H": READ_DATA_H(); break;
            }

            // 화면상의 정렬표시 Clear
            ssView.ActiveSheet.ColumnHeader.Cells[0, 0, 0, ssView.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;

        }

        //마약류 목록
        private void READ_DATA_A(string strGubun)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            ssView.Visible = true;
            ssSpread = ssView;

            ssView_Sheet1.Columns[0].Visible     = false;           //분류
            ssView_Sheet1.Columns[0 + 1].Visible = true;            //성분
            ssView_Sheet1.Columns[1 + 1].Visible = true;            //의약품코드
            ssView_Sheet1.Columns[2 + 1].Visible = true;            //의약품명
            ssView_Sheet1.Columns[3 + 1].Visible = true;            //함량/단위
            ssView_Sheet1.Columns[4 + 1].Visible = true;            //보관온도
            ssView_Sheet1.Columns[5 + 1].Visible = true;            //차광여부
            ssView_Sheet1.Columns[6 + 1].Visible = false;           //효능
            ssView_Sheet1.Columns[7 + 1].Visible = false;           //유사의약품
            ssView_Sheet1.Columns[8 + 1].Visible = false;           //유사의약품명
            ssView_Sheet1.Columns[9 + 1].Visible = false;           //색상/모양
            ssView_Sheet1.Columns[10 + 1].Visible = false;          //투여경로
            ssView_Sheet1.Columns[11 + 1].Visible = false;          //1회 최대용량
            ssView_Sheet1.Columns[12 + 1].Visible = false;          //1일 최대용량
            ssView_Sheet1.Columns[13 + 1].Visible = false;          //적응증
            ssView_Sheet1.Columns[14 + 1].Visible = false;          //실시기준(공통)
            ssView_Sheet1.Columns[15 + 1].Visible = false;          //복지부분료
            ssView_Sheet1.Columns[16 + 1].Visible = false;          //분류명칭

            ssView.Dock = DockStyle.Fill;
            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     'A' AS CLASS, A.SNAME, A.SUNEXT, A.HNAME, A.UNIT, B.SAVETEMP, DECODE(B.SAVEBRIGHT, '1', '차광', '') AS SAVEBRIGHT, B.SAVEBRIGHTETC";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUGINFO_NEW A, " + ComNum.DB_ERP + "DRUG_MASTER2 B, " + ComNum.DB_ERP + "DRUG_JEP C";
                SQL = SQL + ComNum.VBLF + "     WHERE A.SUNEXT = B.JEPCODE(+)";
                SQL = SQL + ComNum.VBLF + "         AND A.SUNEXT = C.JEPCODE ";

                switch (strGubun)
                {
                    case "1":
                        SQL = SQL + ComNum.VBLF + "         AND C.CHENGGU = '09'";
                        break;
                    case "2":
                        SQL = SQL + ComNum.VBLF + "         AND C.CHENGGU = '08'";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "         AND C.CHENGGU IN ('09','08')";
                        break;
                }

                switch (cboBun.Text.Trim())
                {
                    case "경구":
                        SQL = SQL + ComNum.VBLF + "         AND C.CHENGBUN IN ('92', '82')";
                        break;
                    case "외용":
                        SQL = SQL + ComNum.VBLF + "         AND C.CHENGBUN IN ('93', '83')";
                        break;
                    case "주사":
                        SQL = SQL + ComNum.VBLF + "         AND C.CHENGBUN IN ('91', '81')";
                        break;
                }

                SQL = SQL + ComNum.VBLF + "         AND EXISTS";
                SQL = SQL + ComNum.VBLF + "                 (SELECT * FROM " + ComNum.DB_PMPA + "BAS_SUT S";
                SQL = SQL + ComNum.VBLF + "                     WHERE DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "                         AND (S.SUGBJ IN ('2', '3', '4') OR (S.BUN = '23' AND S.SUGBJ = '0'))";
                SQL = SQL + ComNum.VBLF + "                         AND A.SUNEXT = S.SUNEXT)";
                SQL = SQL + ComNum.VBLF + "ORDER BY SUNEXT, CHENGGU, CHENGBUN";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0 + 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1 + 1].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2 + 1].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3 + 1].Text = dt.Rows[i]["UNIT"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4 + 1].Text = dt.Rows[i]["SAVETEMP"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5 + 1].Text = dt.Rows[i]["SAVEBRIGHT"].ToString().Trim();
                    }
                }
                
                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        //위험 의약품 목록
        private void READ_DATA_B(string strGubun)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            ssView.Visible = true;
            ssSpread = ssView;

            ssView_Sheet1.Columns[0].Visible = false;           //분류
            ssView_Sheet1.Columns[0 + 1].Visible = true;            //성분
            ssView_Sheet1.Columns[1 + 1].Visible = true;            //의약품코드
            ssView_Sheet1.Columns[2 + 1].Visible = true;            //의약품명
            ssView_Sheet1.Columns[3 + 1].Visible = true;            //함량/단위
            ssView_Sheet1.Columns[4 + 1].Visible = true;            //보관온도
            ssView_Sheet1.Columns[5 + 1].Visible = true;            //차광여부
            ssView_Sheet1.Columns[6 + 1].Visible = false;           //효능
            ssView_Sheet1.Columns[7 + 1].Visible = false;           //유사의약품
            ssView_Sheet1.Columns[8 + 1].Visible = false;           //유사의약품명
            ssView_Sheet1.Columns[9 + 1].Visible = false;           //색상/모양
            ssView_Sheet1.Columns[10 + 1].Visible = false;          //투여경로
            ssView_Sheet1.Columns[11 + 1].Visible = false;          //1회 최대용량
            ssView_Sheet1.Columns[12 + 1].Visible = false;          //1일 최대용량
            ssView_Sheet1.Columns[13 + 1].Visible = false;          //적응증
            ssView_Sheet1.Columns[14 + 1].Visible = false;          //실시기준(공통)
            ssView_Sheet1.Columns[15 + 1].Visible = false;          //복지부분료
            ssView_Sheet1.Columns[16 + 1].Visible = false;          //분류명칭

            ssView.Dock = DockStyle.Fill;
            ssView_Sheet1.RowCount = 0;

            try
            {
                #region //기존
                //SQL = "";
                //SQL = "SELECT";
                //SQL = SQL + ComNum.VBLF + "     'B' AS CLASS, A.SNAME, A.SUNEXT, A.HNAME, A.UNIT, B.SAVETEMP, DECODE(B.SAVEBRIGHT, '1', '차광', '') AS SAVEBRIGHT, B.SAVEBRIGHTETC";
                //SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUGINFO_NEW A, " + ComNum.DB_ERP + "DRUG_MASTER2  B, " + ComNum.DB_ERP + "DRUG_BUN_LIST C";
                //SQL = SQL + ComNum.VBLF + "     WHERE A.SUNEXT = B.JEPCODE(+)";
                //SQL = SQL + ComNum.VBLF + "         AND A.SUNEXT = C.JEPCODE";

                //switch (strGubun)
                //{
                //    case "*":
                //        SQL = SQL + ComNum.VBLF + "         AND C.GUBUN IN ('01','02','03','04') ";
                //        break;
                //    case "1":
                //        SQL = SQL + ComNum.VBLF + "         AND C.GUBUN = '01'";

                //        switch (cboBun.Text.Trim())
                //        {
                //            case "주사":
                //                SQL = SQL + ComNum.VBLF + "         AND C.BUN = '20'";
                //                break;
                //            case "경구":
                //                SQL = SQL + ComNum.VBLF + "         AND C.BUN = '11'";
                //                break;
                //        }
                //        break;
                //    case "2":
                //        SQL = SQL + ComNum.VBLF + "         AND C.GUBUN = '02'";
                //        break;
                //    case "3":
                //        SQL = SQL + ComNum.VBLF + "         AND C.GUBUN = '03'";
                //        break;
                //    case "4":
                //        SQL = SQL + ComNum.VBLF + "         AND C.GUBUN = '04'";
                //        break;
                //}

                //SQL = SQL + ComNum.VBLF + "         AND EXISTS";
                //SQL = SQL + ComNum.VBLF + "                 (SELECT * FROM " + ComNum.DB_PMPA + "BAS_SUT S";
                //SQL = SQL + ComNum.VBLF + "                     WHERE DELDATE IS NULL";
                //SQL = SQL + ComNum.VBLF + "                         AND S.SUGBJ IN ('2', '3', '4')";
                //SQL = SQL + ComNum.VBLF + "                         AND A.SUNEXT = S.SUNEXT)";
                //SQL = SQL + ComNum.VBLF + "ORDER BY GUBUN, SUNEXT, C.GUBUN, C.BUN";
                #endregion

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DISTINCT ";
                SQL = SQL + ComNum.VBLF + "         C.CODE, C.MCODE, C.MNAME, C.CODE01,  ";
                SQL = SQL + ComNum.VBLF + "         'B' AS CLASS, A.SNAME, A.SUNEXT, A.HNAME, A.UNIT, B.SAVETEMP,  ";
                SQL = SQL + ComNum.VBLF + "         DECODE(B.SAVEBRIGHT, '1', '차광', '') AS SAVEBRIGHT, B.SAVEBRIGHTETC ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_DRUGINFO_NEW A ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_ADM.DRUG_MASTER2 B ";
                SQL = SQL + ComNum.VBLF + "     ON A.SUNEXT = B.JEPCODE(+) ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN  (SELECT B.CODE, B.MCODE, B.MNAME, M.CODE01 ";
                SQL = SQL + ComNum.VBLF + "                FROM KOSMOS_ADM.DRUG_MASTER5 M ";
                SQL = SQL + ComNum.VBLF + "              INNER JOIN KOSMOS_PMPA.BAS_BCODE_DRUG B ";
                SQL = SQL + ComNum.VBLF + "                 ON M.GUBUN = B.MCODE ";
                SQL = SQL + ComNum.VBLF + "                 AND CODE = 'A001' ";
                SQL = SQL + ComNum.VBLF + "                 AND DELDATE IS NULL) C ";
                SQL = SQL + ComNum.VBLF + "     ON TRIM(A.SUNEXT) = TRIM(C.CODE01) ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1 ";

                switch (cboBun.Text.Trim())
                {
                    case "전체":
                        SQL = SQL + ComNum.VBLF + "         AND B.SUGABUN IN ('11','12','20','23')";
                        break;
                    case "주사":
                        SQL = SQL + ComNum.VBLF + "         AND B.SUGABUN IN ('20','23')";
                        break;
                    case "경구":
                        SQL = SQL + ComNum.VBLF + "         AND B.SUGABUN IN ('11')";
                        break;
                    case "외용":
                        SQL = SQL + ComNum.VBLF + "         AND B.SUGABUN IN ('12')";
                        break;
                }

                switch (strGubun)
                {
                    case "*":
                        SQL = SQL + ComNum.VBLF + "         AND SUBSTR(MNAME, 1, 1) IN ('1','2','3','4','5','6') ";
                        break;                    
                    case "2":
                        SQL = SQL + ComNum.VBLF + "         AND SUBSTR(MNAME, 1, 1) = '2' ";

                        //switch (cboBun.Text.Trim())
                        //{
                        //    case "주사":
                        //        SQL = SQL + ComNum.VBLF + "         AND B.SUGABUN = '20'";
                        //        break;
                        //    case "경구":
                        //        SQL = SQL + ComNum.VBLF + "         AND B.SUGABUN = '11'";
                        //        break;
                        //}
                        break;
                    case "1":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                        SQL = SQL + ComNum.VBLF + "         AND SUBSTR(MNAME, 1, 1) = '"+ strGubun + "' ";
                        break;                    
                }
                                
                SQL = SQL + ComNum.VBLF + "  AND EXISTS ";
                SQL = SQL + ComNum.VBLF + "      (SELECT * FROM KOSMOS_PMPA.BAS_SUT S ";
                SQL = SQL + ComNum.VBLF + "          WHERE DELDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "              AND S.SUGBJ IN('0', '2', '3', '4') ";
                SQL = SQL + ComNum.VBLF + "              AND A.SUNEXT = S.SUNEXT)         ";
                SQL = SQL + ComNum.VBLF + " ORDER BY MNAME, CODE01 ";
                                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0 + 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1 + 1].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2 + 1].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3 + 1].Text = dt.Rows[i]["UNIT"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4 + 1].Text = dt.Rows[i]["SAVETEMP"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5 + 1].Text = dt.Rows[i]["SAVEBRIGHT"].ToString().Trim();
                    }
                }
                                
                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        //고주의 의약품 목록
        private void READ_DATA_C(string strGubun1, string strGubun2)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            //string strJepCode = "";
            string strPCode = "";

            int iRow = 0;
            int iCol = 0;

            Cursor.Current = Cursors.WaitCursor;

            ssView.Visible = true;
            ssSpread = ssView;

            ssSame_Sungbun.Visible = false;


            ssView_Sheet1.Columns[0].Visible = false;               //분류
            ssView_Sheet1.Columns[0 + 1].Visible = true;            //성분
            ssView_Sheet1.Columns[1 + 1].Visible = true;            //의약품코드
            ssView_Sheet1.Columns[2 + 1].Visible = true;            //의약품명
            ssView_Sheet1.Columns[3 + 1].Visible = true;            //함량/단위
            ssView_Sheet1.Columns[4 + 1].Visible = true;            //보관온도
            ssView_Sheet1.Columns[5 + 1].Visible = true;            //차광여부
            ssView_Sheet1.Columns[6 + 1].Visible = false;           //효능
            ssView_Sheet1.Columns[7 + 1].Visible = false;           //유사의약품
            ssView_Sheet1.Columns[8 + 1].Visible = false;           //유사의약품명
            ssView_Sheet1.Columns[9 + 1].Visible = false;           //색상/모양
            ssView_Sheet1.Columns[10 + 1].Visible = false;          //투여경로
            ssView_Sheet1.Columns[11 + 1].Visible = false;          //1회 최대용량
            ssView_Sheet1.Columns[12 + 1].Visible = false;          //1일 최대용량
            ssView_Sheet1.Columns[13 + 1].Visible = false;          //적응증
            ssView_Sheet1.Columns[14 + 1].Visible = false;          //실시기준(공통)
            ssView_Sheet1.Columns[15 + 1].Visible = false;          //복지부분료
            ssView_Sheet1.Columns[16 + 1].Visible = false;          //분류명칭

            ssView.Dock = DockStyle.Fill;
            ssView_Sheet1.RowCount = 0;

            if (strGubun1 == "*")
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("세부분류를 선택하십시오.");
                return;
            }

            try
            {
                switch (strGubun1)
                {
                    case "1":
                    case "2":
                        #region //기존
                        //SQL = "";
                        //SQL = "SELECT";
                        //SQL = SQL + ComNum.VBLF + "     'C' AS CLASS, A.SNAME, A.SUNEXT, A.HNAME, A.UNIT, B.SAVETEMP, DECODE(B.SAVEBRIGHT, '1', '차광', '') AS SAVEBRIGHT, B.SAVEBRIGHTETC";
                        //SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUGINFO_NEW A, " + ComNum.DB_ERP + "DRUG_MASTER2 B, " + ComNum.DB_ERP + "DRUG_BUN_LIST C, " + ComNum.DB_PMPA + "BAS_SUT D";
                        //SQL = SQL + ComNum.VBLF + "     WHERE A.SUNEXT = B.JEPCODE(+)";
                        //SQL = SQL + ComNum.VBLF + "         AND A.SUNEXT = C.JEPCODE ";
                        //SQL = SQL + ComNum.VBLF + "         AND C.GUBUN = '10'";
                        //SQL = SQL + ComNum.VBLF + "         AND A.SUNEXT = D.SUNEXT";

                        //switch (cboBun.Text.Trim())
                        //{
                        //    case "전체":
                        //        SQL = SQL + ComNum.VBLF + "         AND D.BUN IN ('11','12','20','23')";
                        //        break;
                        //    case "주사":
                        //        SQL = SQL + ComNum.VBLF + "         AND D.BUN IN ('20','23')";
                        //        break;
                        //    case "경구":
                        //        SQL = SQL + ComNum.VBLF + "         AND D.BUN IN ('11')";
                        //        break;
                        //    case "외용":
                        //        SQL = SQL + ComNum.VBLF + "         AND D.BUN IN ('12')";
                        //        break;
                        //}


                        //SQL = SQL + ComNum.VBLF + "         AND A.SUNEXT NOT IN";
                        //SQL = SQL + ComNum.VBLF + "                         (SELECT";
                        //SQL = SQL + ComNum.VBLF + "                             JEPCODE ";
                        //SQL = SQL + ComNum.VBLF + "                         FROM " + ComNum.DB_ERP + "DRUG_BUN_LIST ";
                        //SQL = SQL + ComNum.VBLF + "                             WHERE GUBUN IN ('01', '02', '03', '04'))";
                        //SQL = SQL + ComNum.VBLF + "ORDER BY SUNEXT,  D.BUN";
                        #endregion

                        //2019-05-10 수정
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT ";
                        SQL = SQL + ComNum.VBLF + "         C.CODE, C.MCODE, C.MNAME, C.CODE01, ";
                        SQL = SQL + ComNum.VBLF + "         'B' AS CLASS, A.SNAME, A.SUNEXT, A.HNAME, A.UNIT, B.SAVETEMP, ";
                        SQL = SQL + ComNum.VBLF + "         DECODE(B.SAVEBRIGHT, '1', '차광', '') AS SAVEBRIGHT, B.SAVEBRIGHTETC";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_DRUGINFO_NEW A";
                        SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_ADM.DRUG_MASTER2 B";
                        SQL = SQL + ComNum.VBLF + "     ON A.SUNEXT = B.JEPCODE(+)";
                        SQL = SQL + ComNum.VBLF + " INNER JOIN  (SELECT B.CODE, B.MCODE, B.MNAME, M.CODE01";
                        SQL = SQL + ComNum.VBLF + "                FROM KOSMOS_ADM.DRUG_MASTER5 M";
                        SQL = SQL + ComNum.VBLF + "              INNER JOIN KOSMOS_PMPA.BAS_BCODE_DRUG B";
                        SQL = SQL + ComNum.VBLF + "                 ON M.GUBUN = B.MCODE";
                        SQL = SQL + ComNum.VBLF + "                 AND CODE = 'A002') C";
                        SQL = SQL + ComNum.VBLF + "     ON TRIM(A.SUNEXT) = TRIM(C.CODE01)";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "  AND SUBSTR(MNAME, 1, 1) = '"+ strGubun1 + "'";

                        switch (strGubun2)
                        {
                            case "1":
                                SQL = SQL + ComNum.VBLF + "  AND SUBSTR(MNAME, 3, 1) = '1'";
                                break;
                            case "2":
                                SQL = SQL + ComNum.VBLF + "  AND SUBSTR(MNAME, 3, 1) = '2'";
                                break;
                        }
                                         
                        switch (cboBun.Text.Trim())
                        {
                            case "전체":
                                SQL = SQL + ComNum.VBLF + "         AND B.SUGABUN IN ('11','12','20','23')";
                                break;
                            case "주사":
                                SQL = SQL + ComNum.VBLF + "         AND B.SUGABUN IN ('20','23')";
                                break;
                            case "경구":
                                SQL = SQL + ComNum.VBLF + "         AND B.SUGABUN IN ('11')";
                                break;
                            case "외용":
                                SQL = SQL + ComNum.VBLF + "         AND B.SUGABUN IN ('12')";
                                break;
                        }
                        SQL = SQL + ComNum.VBLF + "  AND EXISTS";
                        SQL = SQL + ComNum.VBLF + "      (SELECT * FROM KOSMOS_PMPA.BAS_SUT S";
                        SQL = SQL + ComNum.VBLF + "          WHERE DELDATE IS NULL";
                        //SQL = SQL + ComNum.VBLF + "              AND S.SUGBJ IN('2', '3', '4')";
                        SQL = SQL + ComNum.VBLF + "              AND S.SUGBJ IN('0', '2', '3', '4')";
                        SQL = SQL + ComNum.VBLF + "              AND A.SUNEXT = S.SUNEXT)       ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY MNAME, CODE01";
                        
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            ssView_Sheet1.RowCount = dt.Rows.Count;
                            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                ssView_Sheet1.Cells[i, 0 + 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                                ssView_Sheet1.Cells[i, 1 + 1].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                                ssView_Sheet1.Cells[i, 2 + 1].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                                ssView_Sheet1.Cells[i, 3 + 1].Text = dt.Rows[i]["UNIT"].ToString().Trim();
                                ssView_Sheet1.Cells[i, 4 + 1].Text = dt.Rows[i]["SAVETEMP"].ToString().Trim();
                                ssView_Sheet1.Cells[i, 5 + 1].Text = dt.Rows[i]["SAVEBRIGHT"].ToString().Trim();
                            }
                        }

                        dt.Dispose();
                        dt = null;
                        break;
                    #region //기존
                    //case "2":
                    //    SQL = "";
                    //    SQL = "SELECT";
                    //    SQL = SQL + ComNum.VBLF + "     'C' AS CLASS, A.SNAME, A.SUNEXT, A.HNAME, A.UNIT, B.SAVETEMP, DECODE(B.SAVEBRIGHT, '1', '차광', '') AS SAVEBRIGHT, B.SAVEBRIGHTETC";
                    //    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUGINFO_NEW A, " + ComNum.DB_ERP + "DRUG_MASTER2 B, " + ComNum.DB_ERP + "DRUG_BUN_LIST C, " + ComNum.DB_PMPA + "BAS_SUT D";
                    //    SQL = SQL + ComNum.VBLF + "     WHERE A.SUNEXT = B.JEPCODE";
                    //    SQL = SQL + ComNum.VBLF + "         AND A.SUNEXT = C.JEPCODE";
                    //    SQL = SQL + ComNum.VBLF + "         AND A.SUNEXT = D.SUNEXT";
                    //    SQL = SQL + ComNum.VBLF + "         AND C.GUBUN = '11'";

                    //    switch (cboBun.Text.Trim())
                    //    {
                    //        case "전체":
                    //            SQL = SQL + ComNum.VBLF + "         AND D.BUN IN ('11','12','20','23')";
                    //            break;
                    //        case "주사":
                    //            SQL = SQL + ComNum.VBLF + "         AND D.BUN IN ('20','23')";
                    //            break;
                    //        case "경구":
                    //            SQL = SQL + ComNum.VBLF + "         AND D.BUN IN ('11')";
                    //            break;
                    //        case "외용":
                    //            SQL = SQL + ComNum.VBLF + "         AND D.BUN IN ('12')";
                    //            break;
                    //    }

                    //    SQL = SQL + ComNum.VBLF + "ORDER BY A.SUNEXT, D.BUN";

                    //    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    //    if (SqlErr != "")
                    //    {
                    //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    //        return;
                    //    }
                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        ssView_Sheet1.RowCount = dt.Rows.Count;
                    //        ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    //        for (i = 0; i < dt.Rows.Count; i++)
                    //        {
                    //            ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    //            ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                    //            ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                    //            ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["UNIT"].ToString().Trim();
                    //            ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SAVETEMP"].ToString().Trim();
                    //            ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SAVEBRIGHT"].ToString().Trim();
                    //        }
                    //    }

                    //    dt.Dispose();
                    //    dt = null;
                    //    break;
                    #endregion
                    case "3":
                        panMsg.Visible = false;
                        strPCode = cboGubun3.Text.Split(',')[1];
                        switch (strPCode)
                        {                            
                            case "9774":    // 유사코드
                                ssView.Dock = DockStyle.None;
                                ssView.Visible = false;
                                ssSimilar_Med.Dock = DockStyle.Fill;
                                ssSimilar_Med.Visible = true;

                                // 2019-05-10 수정

                                ssSimilar_Med.ActiveSheet.RowCount = 0;

                                //SQL = "";
                                //SQL = SQL + ComNum.VBLF + "SELECT ";
                                //SQL = SQL + ComNum.VBLF + "        B.CODE, B.MCODE, B.MNAME,  ";
                                //SQL = SQL + ComNum.VBLF + "        M.CODE01, ";
                                //SQL = SQL + ComNum.VBLF + "       (SELECT HNAME FROM KOSMOS_OCS.OCS_DRUGINFO_NEW M1 ";
                                //SQL = SQL + ComNum.VBLF + "        WHERE TRIM(M1.SUNEXT) = TRIM(CODE01)) AS NAME01, ";
                                //SQL = SQL + ComNum.VBLF + "        M.CODE02, ";
                                //SQL = SQL + ComNum.VBLF + "       (SELECT HNAME FROM KOSMOS_OCS.OCS_DRUGINFO_NEW M1 ";
                                //SQL = SQL + ComNum.VBLF + "        WHERE TRIM(M1.SUNEXT) = TRIM(CODE02)) AS NAME02, ";
                                //SQL = SQL + ComNum.VBLF + "        M.CODE03, ";
                                //SQL = SQL + ComNum.VBLF + "       (SELECT HNAME FROM KOSMOS_OCS.OCS_DRUGINFO_NEW M1 ";
                                //SQL = SQL + ComNum.VBLF + "        WHERE TRIM(M1.SUNEXT) = TRIM(CODE03)) AS NAME03, ";
                                //SQL = SQL + ComNum.VBLF + "        M.CODE04, ";
                                //SQL = SQL + ComNum.VBLF + "       (SELECT HNAME FROM KOSMOS_OCS.OCS_DRUGINFO_NEW M1 ";
                                //SQL = SQL + ComNum.VBLF + "        WHERE TRIM(M1.SUNEXT) = TRIM(CODE04)) AS NAME04 ";
                                //SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.DRUG_MASTER5 M ";
                                //SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.BAS_BCODE_DRUG B ";
                                //SQL = SQL + ComNum.VBLF + "   ON M.GUBUN = B.MCODE ";
                                //SQL = SQL + ComNum.VBLF + "   AND CODE = 'A002' ";
                                //SQL = SQL + ComNum.VBLF + "   AND SUBSTR(MNAME, 1, 1) = '3' ";
                                //SQL = SQL + ComNum.VBLF + "   AND SUBSTR(MNAME, 3, 1) = '1' ";
                                //SQL = SQL + ComNum.VBLF + "   AND EXISTS";
                                //SQL = SQL + ComNum.VBLF + "       (SELECT * FROM KOSMOS_PMPA.BAS_SUT S";
                                //SQL = SQL + ComNum.VBLF + "           WHERE DELDATE IS NULL";
                                //SQL = SQL + ComNum.VBLF + "               AND S.SUGBJ IN('2', '3', '4')";
                                //SQL = SQL + ComNum.VBLF + "               AND M.CODE01 = TRIM(S.SUNEXT))       ";
                                //SQL = SQL + ComNum.VBLF + " ORDER BY MNAME, CODE01";

                                SQL = "";
                                SQL = SQL + ComNum.VBLF + "SELECT                                                   ";
                                SQL = SQL + ComNum.VBLF + "     CLASS, GRP, JEPCODE, HNAME                          ";
                                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.DRUG_MASTER_SETCODE A                   ";
                                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_OCS.OCS_DRUGINFO_NEW B                ";
                                SQL = SQL + ComNum.VBLF + "     on A.JEPCODE = B.SUNEXT                             ";
                                SQL = SQL + ComNum.VBLF + " WHERE CLASS = '9774'                                    ";
                                SQL = SQL + ComNum.VBLF + " AND GRP IN(SELECT                                       ";
                                SQL = SQL + ComNum.VBLF + "                 GRP                                     ";
                                SQL = SQL + ComNum.VBLF + "             FROM KOSMOS_ADM.DRUG_MASTER_SETCODE A       ";
                                SQL = SQL + ComNum.VBLF + "             INNER JOIN KOSMOS_OCS.OCS_DRUGINFO_NEW B    ";
                                SQL = SQL + ComNum.VBLF + "                 on A.JEPCODE = B.SUNEXT                 ";
                                SQL = SQL + ComNum.VBLF + "             WHERE CLASS = '9774'                        ";
                                SQL = SQL + ComNum.VBLF + "             AND EXISTS                                  ";
                                SQL = SQL + ComNum.VBLF + "                 (SELECT * FROM KOSMOS_PMPA.BAS_SUT S    ";
                                SQL = SQL + ComNum.VBLF + "                     WHERE DELDATE IS NULL               ";
                                SQL = SQL + ComNum.VBLF + "                         AND S.SUGBJ IN('2', '3', '4')   ";
                                SQL = SQL + ComNum.VBLF + "                         AND A.JEPCODE = S.SUNEXT)       ";
                                SQL = SQL + ComNum.VBLF + "             GROUP BY GRP                                ";
                                SQL = SQL + ComNum.VBLF + "             HAVING COUNT(GRP) > 1 )                     ";
                                SQL = SQL + ComNum.VBLF + " AND EXISTS                                              ";
                                SQL = SQL + ComNum.VBLF + "     (SELECT * FROM KOSMOS_PMPA.BAS_SUT S                ";
                                SQL = SQL + ComNum.VBLF + "         WHERE DELDATE IS NULL                           ";
                                SQL = SQL + ComNum.VBLF + "             AND S.SUGBJ IN('2', '3', '4')               ";
                                SQL = SQL + ComNum.VBLF + "             AND A.JEPCODE = S.SUNEXT)                   ";
                                SQL = SQL + ComNum.VBLF + " ORDER BY CLASS, GRP, JEPCODE                            ";

                                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    Cursor.Current = Cursors.Default;
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }
                                if (dt.Rows.Count > 0)
                                {
                                    string strGrp = "";
                                    iRow = 0;
                                    iCol = 0;
                                    
                                    for (i = 0; i < dt.Rows.Count; i++)
                                    {
                                        if (dt.Rows[i]["GRP"].ToString().Trim() != strGrp)
                                        {
                                            ssSimilar_Med_Sheet1.RowCount = ssSimilar_Med_Sheet1.RowCount + 1;
                                            ssSimilar_Med_Sheet1.SetRowHeight(iRow, ComNum.SPDROWHT);

                                            strGrp = dt.Rows[i]["GRP"].ToString().Trim();
                                            ssSimilar_Med_Sheet1.Cells[iRow, 0].Text = (iRow + 1).ToString();
                                            ssSimilar_Med_Sheet1.Cells[iRow, 1].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                                            ssSimilar_Med_Sheet1.Cells[iRow, 2].Text = dt.Rows[i]["HNAME"].ToString().Trim();

                                            iRow = iRow + 1;
                                            iCol = 1;
                                        }
                                        else
                                        {                                            
                                            switch (iCol)
                                            {
                                                case 1:
                                                    ssSimilar_Med_Sheet1.Cells[iRow - 1, 3].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                                                    ssSimilar_Med_Sheet1.Cells[iRow - 1, 4].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                                                    break;
                                                case 2:
                                                    ssSimilar_Med_Sheet1.Cells[iRow - 1, 5].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                                                    ssSimilar_Med_Sheet1.Cells[iRow - 1, 6].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                                                    break;
                                                case 3:
                                                    ssSimilar_Med_Sheet1.Cells[iRow - 1, 7].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                                                    ssSimilar_Med_Sheet1.Cells[iRow - 1, 8].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                                                    break;
                                            }

                                            iCol = iCol + 1;
                                        }                                        
                                    }                                    
                                }

                                dt.Dispose();
                                dt = null;

                                ssSpread = ssSimilar_Med;
                                break;
                            case "9775":    //유사외관
                                ssView.Dock = DockStyle.None;
                                ssView.Visible = false;
                                ssSimilar_Exterior.Dock = DockStyle.Fill;
                                ssSimilar_Exterior.Visible = true;
                                ssSpread = ssSimilar_Exterior;

                                #region //기존
                                //for (i = 0; i < ssSimilar_Exterior_Sheet1.RowCount; i++)
                                //{
                                //    strJepCode = ssSimilar_Exterior_Sheet1.Cells[i, 1].Text.Trim();

                                //    SQL = "";
                                //    SQL = "SELECT";
                                //    SQL = SQL + ComNum.VBLF + "     JEHENG, JEHENG2, JEHENG3_1, JEHENG3_2";
                                //    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUGINFO_NEW ";
                                //    SQL = SQL + ComNum.VBLF + "     WHERE SUNEXT = '" + strJepCode + "' ";

                                //    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                                //    if (SqlErr != "")
                                //    {
                                //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                //        return;
                                //    }
                                //    if (dt.Rows.Count > 0)
                                //    {
                                //        ssSimilar_Exterior_Sheet1.Cells[i, 3].Text = "";
                                //        ssSimilar_Exterior_Sheet1.Cells[i, 3].Text = dt.Rows[0]["JEHENG2"].ToString().Trim();
                                //        ssSimilar_Exterior_Sheet1.Cells[i, 4].Text = "";

                                //        if (dt.Rows[0]["JEHENG3_1"].ToString().Trim() != "")
                                //        {
                                //            ssSimilar_Exterior_Sheet1.Cells[i, 4].Text = ssSimilar_Exterior_Sheet1.Cells[i, 4].Text + "앞)" + dt.Rows[0]["JEHENG3_1"].ToString().Trim();
                                //        }

                                //        if (dt.Rows[0]["JEHENG3_2"].ToString().Trim() != "")
                                //        {
                                //            ssSimilar_Exterior_Sheet1.Cells[i, 4].Text = ssSimilar_Exterior_Sheet1.Cells[i, 4].Text + ComNum.VBLF + "뒤)" + dt.Rows[0]["JEHENG3_2"].ToString().Trim();
                                //        }
                                //    }

                                //    dt.Dispose();
                                //    dt = null;
                                //}

                                //for (i = 0; i < ssSimilar_Exterior_Sheet1.RowCount; i++)
                                //{
                                //    strJepCode = ssSimilar_Exterior_Sheet1.Cells[i, 5].Text.Trim();

                                //    SQL = "";
                                //    SQL = "SELECT";
                                //    SQL = SQL + ComNum.VBLF + "     JEHENG, JEHENG2, JEHENG3_1, JEHENG3_2";
                                //    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUGINFO_NEW ";
                                //    SQL = SQL + ComNum.VBLF + "     WHERE SUNEXT = '" + strJepCode + "' ";

                                //    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                                //    if (SqlErr != "")
                                //    {
                                //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                //        return;
                                //    }
                                //    if (dt.Rows.Count > 0)
                                //    {
                                //        ssSimilar_Exterior_Sheet1.Cells[i, 7].Text = "";
                                //        ssSimilar_Exterior_Sheet1.Cells[i, 7].Text = dt.Rows[0]["JEHENG2"].ToString().Trim();
                                //        ssSimilar_Exterior_Sheet1.Cells[i, 8].Text = "";

                                //        if (dt.Rows[0]["JEHENG3_1"].ToString().Trim() != "")
                                //        {
                                //            ssSimilar_Exterior_Sheet1.Cells[i, 8].Text = ssSimilar_Exterior_Sheet1.Cells[i, 8].Text + "앞)" + dt.Rows[0]["JEHENG3_1"].ToString().Trim();
                                //        }

                                //        if (dt.Rows[0]["JEHENG3_2"].ToString().Trim() != "")
                                //        {
                                //            ssSimilar_Exterior_Sheet1.Cells[i, 8].Text = ssSimilar_Exterior_Sheet1.Cells[i, 8].Text + ComNum.VBLF + "뒤)" + dt.Rows[0]["JEHENG3_2"].ToString().Trim();
                                //        }
                                //    }

                                //    dt.Dispose();
                                //    dt = null;
                                //}

                                //for (i = 0; i < ssSimilar_Exterior_Sheet1.RowCount; i++)
                                //{
                                //    strJepCode = ssSimilar_Exterior_Sheet1.Cells[i, 9].Text.Trim();

                                //    SQL = "";
                                //    SQL = "SELECT";
                                //    SQL = SQL + ComNum.VBLF + "     JEHENG, JEHENG2, JEHENG3_1, JEHENG3_2";
                                //    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUGINFO_NEW ";
                                //    SQL = SQL + ComNum.VBLF + "     WHERE SUNEXT = '" + strJepCode + "' ";

                                //    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                                //    if (SqlErr != "")
                                //    {
                                //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                //        return;
                                //    }
                                //    if (dt.Rows.Count > 0)
                                //    {
                                //        ssSimilar_Exterior_Sheet1.Cells[i, 11].Text = "";
                                //        ssSimilar_Exterior_Sheet1.Cells[i, 11].Text = dt.Rows[0]["JEHENG2"].ToString().Trim();
                                //        ssSimilar_Exterior_Sheet1.Cells[i, 12].Text = "";

                                //        if (dt.Rows[0]["JEHENG3_1"].ToString().Trim() != "")
                                //        {
                                //            ssSimilar_Exterior_Sheet1.Cells[i, 12].Text = ssSimilar_Exterior_Sheet1.Cells[i, 12].Text + "앞)" + dt.Rows[0]["JEHENG3_1"].ToString().Trim();
                                //        }

                                //        if (dt.Rows[0]["JEHENG3_2"].ToString().Trim() != "")
                                //        {
                                //            ssSimilar_Exterior_Sheet1.Cells[i, 12].Text = ssSimilar_Exterior_Sheet1.Cells[i, 12].Text + ComNum.VBLF + "뒤)" + dt.Rows[0]["JEHENG3_2"].ToString().Trim();
                                //        }
                                //    }

                                //    dt.Dispose();
                                //    dt = null;
                                //}
                                #endregion

                                // 2019-05-10 수정
                                ssSimilar_Exterior_Sheet1.RowCount = 0;

                                #region // 사용안함
                                //SQL = "";
                                //SQL = SQL + ComNum.VBLF + " SELECT                                                                                     ";
                                //SQL = SQL + ComNum.VBLF + "          B.CODE, B.MCODE, B.MNAME,                                                         ";
                                //SQL = SQL + ComNum.VBLF + "          M.CODE01,                                                                         ";
                                //SQL = SQL + ComNum.VBLF + "          (SELECT BCODE FROM KOSMOS_PMPA.BAS_SUN                                            ";
                                //SQL = SQL + ComNum.VBLF + "            WHERE TRIM(SUNEXT) = CODE01) AS EDICODE01,                                      ";
                                //SQL = SQL + ComNum.VBLF + "          (SELECT HNAME FROM KOSMOS_OCS.OCS_DRUGINFO_NEW M1                                 ";
                                //SQL = SQL + ComNum.VBLF + "          WHERE TRIM(M1.SUNEXT) = CODE01) AS NAME01,                                        ";
                                //SQL = SQL + ComNum.VBLF + "          (SELECT JEHENG2 FROM KOSMOS_OCS.OCS_DRUGINFO_NEW M1                               ";
                                //SQL = SQL + ComNum.VBLF + "                         WHERE TRIM(M1.SUNEXT) = CODE01) AS SHAPE01,                        ";
                                //SQL = SQL + ComNum.VBLF + "          (SELECT '앞)' || JEHENG3_1 || CHR(13) || CHR(10) || '뒤)' || JEHENG3_2            ";
                                //SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_OCS.OCS_DRUGINFO_NEW M1 WHERE TRIM(M1.SUNEXT) = CODE01)  AS REMARK01, ";
                                //SQL = SQL + ComNum.VBLF + "          M.CODE02,                                                                         ";
                                //SQL = SQL + ComNum.VBLF + "          (SELECT BCODE FROM KOSMOS_PMPA.BAS_SUN                                            ";
                                //SQL = SQL + ComNum.VBLF + "            WHERE TRIM(SUNEXT) = CODE02) AS EDICODE02,                                      ";
                                //SQL = SQL + ComNum.VBLF + "          (SELECT HNAME FROM KOSMOS_OCS.OCS_DRUGINFO_NEW M1                                 ";
                                //SQL = SQL + ComNum.VBLF + "          WHERE TRIM(M1.SUNEXT) = CODE02) AS NAME02,                                        ";
                                //SQL = SQL + ComNum.VBLF + "          (SELECT JEHENG2 FROM KOSMOS_OCS.OCS_DRUGINFO_NEW M1                               ";
                                //SQL = SQL + ComNum.VBLF + "                         WHERE TRIM(M1.SUNEXT) = CODE02) AS SHAPE02,                        ";
                                //SQL = SQL + ComNum.VBLF + "          (SELECT '앞)' || JEHENG3_1 || CHR(13) || CHR(10) || '뒤)' || JEHENG3_2            ";
                                //SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_OCS.OCS_DRUGINFO_NEW M1 WHERE TRIM(M1.SUNEXT) = CODE02)  AS REMARK02, ";
                                //SQL = SQL + ComNum.VBLF + "          M.CODE03,                                                                         ";
                                //SQL = SQL + ComNum.VBLF + "          (SELECT BCODE FROM KOSMOS_PMPA.BAS_SUN                                            ";
                                //SQL = SQL + ComNum.VBLF + "            WHERE TRIM(SUNEXT) = CODE03) AS EDICODE03,                                      ";
                                //SQL = SQL + ComNum.VBLF + "          (SELECT HNAME FROM KOSMOS_OCS.OCS_DRUGINFO_NEW M1                                 ";
                                //SQL = SQL + ComNum.VBLF + "          WHERE TRIM(M1.SUNEXT) = CODE03) AS NAME03,                                        ";
                                //SQL = SQL + ComNum.VBLF + "          (SELECT JEHENG2 FROM KOSMOS_OCS.OCS_DRUGINFO_NEW M1                               ";
                                //SQL = SQL + ComNum.VBLF + "                         WHERE TRIM(M1.SUNEXT) = CODE03) AS SHAPE03,                        ";
                                //SQL = SQL + ComNum.VBLF + "          (SELECT '앞)' || JEHENG3_1 || CHR(13) || CHR(10) || '뒤)' || JEHENG3_2            ";
                                //SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_OCS.OCS_DRUGINFO_NEW M1 WHERE TRIM(M1.SUNEXT) = CODE03)  AS REMARK03, ";
                                //SQL = SQL + ComNum.VBLF + "          M.CODE04,                                                                         ";
                                //SQL = SQL + ComNum.VBLF + "          (SELECT BCODE FROM KOSMOS_PMPA.BAS_SUN                                            ";
                                //SQL = SQL + ComNum.VBLF + "            WHERE TRIM(SUNEXT) = CODE04) AS EDICODE04,                                      ";
                                //SQL = SQL + ComNum.VBLF + "          (SELECT HNAME FROM KOSMOS_OCS.OCS_DRUGINFO_NEW M1                                 ";
                                //SQL = SQL + ComNum.VBLF + "          WHERE TRIM(M1.SUNEXT) = CODE04) AS NAME04,                                        ";
                                //SQL = SQL + ComNum.VBLF + "          (SELECT JEHENG2 FROM KOSMOS_OCS.OCS_DRUGINFO_NEW M1                               ";
                                //SQL = SQL + ComNum.VBLF + "                         WHERE TRIM(M1.SUNEXT) = CODE04) AS SHAPE04,                        ";
                                //SQL = SQL + ComNum.VBLF + "          (SELECT '앞)' || JEHENG3_1 || CHR(13) || CHR(10) || '뒤)' || JEHENG3_2            ";
                                //SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_OCS.OCS_DRUGINFO_NEW M1 WHERE TRIM(M1.SUNEXT) = CODE04)  AS REMARK04  ";
                                //SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_ADM.DRUG_MASTER5 M                                                          ";
                                //SQL = SQL + ComNum.VBLF + "  INNER JOIN KOSMOS_PMPA.BAS_BCODE_DRUG B                                                   ";
                                //SQL = SQL + ComNum.VBLF + "     ON M.GUBUN = B.MCODE                                                                   ";
                                //SQL = SQL + ComNum.VBLF + "     AND CODE = 'A002'                                                                      ";
                                //SQL = SQL + ComNum.VBLF + "     AND SUBSTR(MNAME, 1, 1) = '3'                                                          ";
                                //SQL = SQL + ComNum.VBLF + "     AND SUBSTR(MNAME, 3, 1) = '2'                                                          ";
                                //SQL = SQL + ComNum.VBLF + "   AND EXISTS";
                                //SQL = SQL + ComNum.VBLF + "       (SELECT * FROM KOSMOS_PMPA.BAS_SUT S";
                                //SQL = SQL + ComNum.VBLF + "           WHERE DELDATE IS NULL";
                                //SQL = SQL + ComNum.VBLF + "               AND S.SUGBJ IN('2', '3', '4')";
                                //SQL = SQL + ComNum.VBLF + "               AND M.CODE01 = TRIM(S.SUNEXT))       ";
                                //SQL = SQL + ComNum.VBLF + "  ORDER BY TRIM(CODE01)                                                                     ";
                                #endregion

                                SQL = "";
                                SQL = SQL + ComNum.VBLF + "SELECT                                                                                   ";
                                SQL = SQL + ComNum.VBLF + "     CLASS, TO_NUMBER(GRP) AS GRP, JEPCODE, HNAME, BCODE AS EDICODE, JEHENG2 AS SHAPE,   ";
                                SQL = SQL + ComNum.VBLF + "     '앞)' || JEHENG3_1 || CHR(13) || CHR(10) || '뒤)' || JEHENG3_2 AS REMARK            ";
                                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.DRUG_MASTER_SETCODE A                                                   ";
                                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_OCS.OCS_DRUGINFO_NEW B                                                ";
                                SQL = SQL + ComNum.VBLF + "     on A.JEPCODE = B.SUNEXT                                                             ";
                                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_PMPA.BAS_SUN C                                                        ";
                                SQL = SQL + ComNum.VBLF + "     ON A.JEPCODE = C.SUNEXT                                                             ";
                                SQL = SQL + ComNum.VBLF + " WHERE CLASS = '9775'                                                                    ";
                                SQL = SQL + ComNum.VBLF + "   AND GRP IN(SELECT                                                                     ";
                                SQL = SQL + ComNum.VBLF + "                     GRP                                                                 ";
                                SQL = SQL + ComNum.VBLF + "                 FROM KOSMOS_ADM.DRUG_MASTER_SETCODE A                                   ";
                                SQL = SQL + ComNum.VBLF + "                 INNER JOIN KOSMOS_OCS.OCS_DRUGINFO_NEW B                                ";
                                SQL = SQL + ComNum.VBLF + "                     on A.JEPCODE = B.SUNEXT                                             ";
                                SQL = SQL + ComNum.VBLF + "                 INNER JOIN KOSMOS_PMPA.BAS_SUN C                                        ";
                                SQL = SQL + ComNum.VBLF + "                     ON A.JEPCODE = C.SUNEXT                                             ";
                                SQL = SQL + ComNum.VBLF + "                 WHERE CLASS = '9775'                                                    ";
                                SQL = SQL + ComNum.VBLF + "                   AND EXISTS(SELECT * FROM KOSMOS_PMPA.BAS_SUT S                        ";
                                SQL = SQL + ComNum.VBLF + "                           WHERE DELDATE IS NULL                                         ";
                                SQL = SQL + ComNum.VBLF + "                               AND S.SUGBJ IN('2', '3', '4')                             ";
                                SQL = SQL + ComNum.VBLF + "                               AND A.JEPCODE = S.SUNEXT)                                 ";
                                SQL = SQL + ComNum.VBLF + "                 GROUP BY GRP                                                            ";
                                SQL = SQL + ComNum.VBLF + "                 HAVING COUNT(GRP) > 1 )                                                 ";
                                SQL = SQL + ComNum.VBLF + "   AND EXISTS (SELECT * FROM KOSMOS_PMPA.BAS_SUT S                                       ";
                                SQL = SQL + ComNum.VBLF + "           WHERE DELDATE IS NULL                                                         ";
                                SQL = SQL + ComNum.VBLF + "               AND S.SUGBJ IN('2', '3', '4')                                             ";
                                SQL = SQL + ComNum.VBLF + "               AND A.JEPCODE = S.SUNEXT)                                                 ";
                                SQL = SQL + ComNum.VBLF + " ORDER BY CLASS, GRP, JEPCODE                                                            ";

                                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    Cursor.Current = Cursors.Default;
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }
                                if (dt.Rows.Count > 0)
                                {
                                    string strGrp = "";
                                    iRow = 0;
                                    iCol = 0;
                                    
                                    for ( i = 0; i < dt.Rows.Count; i++)
                                    {
                                        if (dt.Rows[i]["GRP"].ToString().Trim() != strGrp)
                                        {
                                            ssSimilar_Exterior_Sheet1.RowCount = ssSimilar_Exterior_Sheet1.RowCount + 1;
                                            ssSimilar_Exterior_Sheet1.SetRowHeight(iRow, ComNum.SPDROWHT * 2);

                                            strGrp = dt.Rows[i]["GRP"].ToString().Trim();
                                            ssSimilar_Exterior_Sheet1.Cells[iRow, 0].Text = (iRow + 1).ToString();
                                            ssSimilar_Exterior_Sheet1.Cells[iRow, 1].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                                            ssSimilar_Exterior_Sheet1.Cells[iRow, 2].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                                            READ_MEDICINE_IMG(dt.Rows[i]["EDICODE"].ToString().Trim(), ssSimilar_Exterior, iRow, 3);
                                            ssSimilar_Exterior_Sheet1.Cells[iRow, 4].Text = dt.Rows[i]["SHAPE"].ToString().Trim();
                                            ssSimilar_Exterior_Sheet1.Cells[iRow, 5].Text = dt.Rows[i]["REMARK"].ToString().Trim();

                                            iRow = iRow + 1;
                                            iCol = 1;
                                        }
                                        else
                                        {
                                            switch (iCol)
                                            {
                                                case 1:
                                                    ssSimilar_Exterior_Sheet1.Cells[iRow - 1, 6].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                                                    ssSimilar_Exterior_Sheet1.Cells[iRow - 1, 7].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                                                    READ_MEDICINE_IMG(dt.Rows[i]["EDICODE"].ToString().Trim(), ssSimilar_Exterior, iRow - 1, 8);
                                                    ssSimilar_Exterior_Sheet1.Cells[iRow - 1, 9].Text = dt.Rows[i]["SHAPE"].ToString().Trim();
                                                    ssSimilar_Exterior_Sheet1.Cells[iRow - 1, 10].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                                                    break;
                                                case 2:
                                                    ssSimilar_Exterior_Sheet1.Cells[iRow - 1, 11].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                                                    ssSimilar_Exterior_Sheet1.Cells[iRow - 1, 12].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                                                    READ_MEDICINE_IMG(dt.Rows[i]["EDICODE"].ToString().Trim(), ssSimilar_Exterior, iRow - 1, 13);
                                                    ssSimilar_Exterior_Sheet1.Cells[iRow - 1, 14].Text = dt.Rows[i]["SHAPE"].ToString().Trim();
                                                    ssSimilar_Exterior_Sheet1.Cells[iRow - 1, 15].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                                                    break;
                                                //case 3:
                                                //    ssSimilar_Exterior_Sheet1.Cells[iRow - 1, 16].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                                                //    ssSimilar_Exterior_Sheet1.Cells[iRow - 1, 17].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                                                //    READ_MEDICINE_IMG(dt.Rows[i]["EDICODE"].ToString().Trim(), ssSimilar_Exterior, iRow - 1, 18);
                                                //    ssSimilar_Exterior_Sheet1.Cells[iRow - 1, 19].Text = dt.Rows[i]["SHAPE"].ToString().Trim();
                                                //    ssSimilar_Exterior_Sheet1.Cells[iRow - 1, 20].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                                                //    break;                                                
                                            }

                                            iCol = iCol + 1;
                                        }                                          
                                    }
                                }

                                dt.Dispose();
                                dt = null;
                                break;
                            case "9776":    //유사이름
                                ssView.Dock = DockStyle.None;
                                ssView.Visible = false;
                                ssSimilar_Pronounce.Dock = DockStyle.Fill;
                                ssSimilar_Pronounce.Visible = true;
                                ssSpread = ssSimilar_Pronounce;

                                // 2019-05-10 수정
                                ssSimilar_Pronounce_Sheet1.RowCount = 0;


                                //SQL = SQL + ComNum.VBLF + " SELECT  ";
                                //SQL = SQL + ComNum.VBLF + "         B.CODE, B.MCODE, B.MNAME, ";
                                //SQL = SQL + ComNum.VBLF + "         M.CODE01, ";
                                //SQL = SQL + ComNum.VBLF + "         (SELECT HNAME FROM KOSMOS_OCS.OCS_DRUGINFO_NEW M1 ";
                                //SQL = SQL + ComNum.VBLF + "         WHERE TRIM(M1.SUNEXT) = CODE01) AS NAME01, ";
                                //SQL = SQL + ComNum.VBLF + "         M.CODE02, ";
                                //SQL = SQL + ComNum.VBLF + "         (SELECT HNAME FROM KOSMOS_OCS.OCS_DRUGINFO_NEW M1 ";
                                //SQL = SQL + ComNum.VBLF + "         WHERE TRIM(M1.SUNEXT) = CODE02) AS NAME02, ";
                                //SQL = SQL + ComNum.VBLF + "         M.CODE03, ";
                                //SQL = SQL + ComNum.VBLF + "         (SELECT HNAME FROM KOSMOS_OCS.OCS_DRUGINFO_NEW M1 ";
                                //SQL = SQL + ComNum.VBLF + "         WHERE TRIM(M1.SUNEXT) = CODE03) AS NAME03, ";
                                //SQL = SQL + ComNum.VBLF + "         M.CODE04, ";
                                //SQL = SQL + ComNum.VBLF + "         (SELECT HNAME FROM KOSMOS_OCS.OCS_DRUGINFO_NEW M1 ";
                                //SQL = SQL + ComNum.VBLF + "         WHERE TRIM(M1.SUNEXT) = CODE04) AS NAME04, ";
                                //SQL = SQL + ComNum.VBLF + "         M.CODE05, ";
                                //SQL = SQL + ComNum.VBLF + "         (SELECT HNAME FROM KOSMOS_OCS.OCS_DRUGINFO_NEW M1 ";
                                //SQL = SQL + ComNum.VBLF + "         WHERE TRIM(M1.SUNEXT) = CODE05) AS NAME05 ";
                                //SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_ADM.DRUG_MASTER5 M ";
                                //SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_PMPA.BAS_BCODE_DRUG B ";
                                //SQL = SQL + ComNum.VBLF + "    ON M.GUBUN = B.MCODE ";
                                //SQL = SQL + ComNum.VBLF + "    AND CODE = 'A002' ";
                                //SQL = SQL + ComNum.VBLF + "    AND SUBSTR(MNAME, 1, 1) = '3' ";
                                //SQL = SQL + ComNum.VBLF + "    AND SUBSTR(MNAME, 3, 1) = '3' ";
                                //SQL = SQL + ComNum.VBLF + "   AND EXISTS";
                                //SQL = SQL + ComNum.VBLF + "       (SELECT * FROM KOSMOS_PMPA.BAS_SUT S";
                                //SQL = SQL + ComNum.VBLF + "           WHERE DELDATE IS NULL";
                                //SQL = SQL + ComNum.VBLF + "               AND S.SUGBJ IN('2', '3', '4')";
                                //SQL = SQL + ComNum.VBLF + "               AND M.CODE01 = TRIM(S.SUNEXT))       ";
                                //SQL = SQL + ComNum.VBLF + " ORDER BY TRIM(CODE01) ";

                                SQL = "";                                
                                SQL = SQL + ComNum.VBLF + "SELECT                                                       ";
                                SQL = SQL + ComNum.VBLF + "     CLASS, GRP, JEPCODE, HNAME                              ";
                                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.DRUG_MASTER_SETCODE A                       ";
                                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_OCS.OCS_DRUGINFO_NEW B                    ";
                                SQL = SQL + ComNum.VBLF + "     on A.JEPCODE = B.SUNEXT                                 ";
                                SQL = SQL + ComNum.VBLF + " WHERE CLASS = '9776'                                        ";
                                SQL = SQL + ComNum.VBLF + " AND GRP IN(SELECT                                           ";
                                SQL = SQL + ComNum.VBLF + "                 GRP                                         ";
                                SQL = SQL + ComNum.VBLF + "             FROM KOSMOS_ADM.DRUG_MASTER_SETCODE A           ";
                                SQL = SQL + ComNum.VBLF + "             INNER JOIN KOSMOS_OCS.OCS_DRUGINFO_NEW B        ";
                                SQL = SQL + ComNum.VBLF + "                 on A.JEPCODE = B.SUNEXT                     ";
                                SQL = SQL + ComNum.VBLF + "             WHERE CLASS = '9776'                            ";
                                SQL = SQL + ComNum.VBLF + "             AND EXISTS                                      ";
                                SQL = SQL + ComNum.VBLF + "                 (SELECT * FROM KOSMOS_PMPA.BAS_SUT S        ";
                                SQL = SQL + ComNum.VBLF + "                     WHERE DELDATE IS NULL                   ";
                                SQL = SQL + ComNum.VBLF + "                         AND S.SUGBJ IN('2', '3', '4')       ";
                                SQL = SQL + ComNum.VBLF + "                         AND A.JEPCODE = S.SUNEXT)           ";
                                SQL = SQL + ComNum.VBLF + "             GROUP BY GRP                                    ";
                                SQL = SQL + ComNum.VBLF + "             HAVING COUNT(GRP) > 1)                          ";
                                SQL = SQL + ComNum.VBLF + " AND EXISTS                                                  ";
                                SQL = SQL + ComNum.VBLF + "     (SELECT * FROM KOSMOS_PMPA.BAS_SUT S                    ";
                                SQL = SQL + ComNum.VBLF + "         WHERE DELDATE IS NULL                               ";
                                SQL = SQL + ComNum.VBLF + "             AND S.SUGBJ IN('2', '3', '4')                   ";
                                SQL = SQL + ComNum.VBLF + "             AND A.JEPCODE = S.SUNEXT)                       ";
                                SQL = SQL + ComNum.VBLF + " ORDER BY CLASS, GRP, JEPCODE                                ";

                                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    Cursor.Current = Cursors.Default;
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }
                                if (dt.Rows.Count > 0)
                                {
                                    string strGrp = "";
                                    iRow = 0;
                                    iCol = 0;

                                    for (i = 0; i < dt.Rows.Count; i++)
                                    {
                                        if (dt.Rows[i]["GRP"].ToString().Trim() != strGrp)
                                        {
                                            ssSimilar_Pronounce_Sheet1.RowCount = ssSimilar_Pronounce_Sheet1.RowCount + 1;
                                            ssSimilar_Pronounce_Sheet1.SetRowHeight(iRow, ComNum.SPDROWHT);

                                            strGrp = dt.Rows[i]["GRP"].ToString().Trim();
                                            ssSimilar_Pronounce_Sheet1.Cells[iRow, 0].Text = (iRow + 1).ToString();
                                            ssSimilar_Pronounce_Sheet1.Cells[iRow, 1].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                                            ssSimilar_Pronounce_Sheet1.Cells[iRow, 2].Text = dt.Rows[i]["HNAME"].ToString().Trim();

                                            iRow = iRow + 1;
                                            iCol = 1;
                                        }
                                        else
                                        {
                                            switch (iCol)
                                            {
                                                case 1:
                                                    ssSimilar_Pronounce_Sheet1.Cells[iRow - 1, 3].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                                                    ssSimilar_Pronounce_Sheet1.Cells[iRow - 1, 4].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                                                    break;
                                                case 2:
                                                    ssSimilar_Pronounce_Sheet1.Cells[iRow - 1, 5].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                                                    ssSimilar_Pronounce_Sheet1.Cells[iRow - 1, 6].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                                                    break;
                                                case 3:
                                                    ssSimilar_Pronounce_Sheet1.Cells[iRow - 1, 7].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                                                    ssSimilar_Pronounce_Sheet1.Cells[iRow - 1, 8].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                                                    break;
                                                case 4:
                                                    ssSimilar_Pronounce_Sheet1.Cells[iRow - 1, 9].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                                                    ssSimilar_Pronounce_Sheet1.Cells[iRow - 1, 10].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                                                    break;
                                            }

                                            iCol = iCol + 1;
                                        }
                                    }                                    
                                }

                                dt.Dispose();
                                dt = null;
                                break;
                            case "9777":    // 동일성분/다함량 의약품
                                ssView.Dock = DockStyle.None;
                                ssView.Visible = false;

                                //switch (cboBun.Text.Trim())
                                //{
                                //    case "주사":
                                //        ssSame_Jusa.Dock = DockStyle.Fill;
                                //        ssSame_Jusa.Visible = true;
                                //        ssSpread = ssSame_Jusa;
                                //        break;
                                //    case "경구":
                                //        ssSame_Mouse.Dock = DockStyle.Fill;
                                //        ssSame_Mouse.Visible = true;
                                //        ssSpread = ssSame_Mouse;
                                //        break;
                                //    case "외용":
                                //        ssSame_Out.Dock = DockStyle.Fill;
                                //        ssSame_Out.Visible = true;
                                //        ssSpread = ssSame_Out;
                                //        break;
                                //}

                                ssSame_Sungbun_Sheet1.RowCount = 0;
                                ssSame_Sungbun.Dock = DockStyle.Fill;
                                ssSame_Sungbun.Visible = true;
                                ssSpread = ssSame_Sungbun;

                                string strGrpName = "";
                                iRow = 0;
                                iCol = 0;
                                

                                #region
                                //SQL = SQL + ComNum.VBLF + " SELECT                                                          ";
                                //SQL = SQL + ComNum.VBLF + "     M.GUBUN2,                                                   ";
                                //SQL = SQL + ComNum.VBLF + "     M.CODE01, C1.HNAME AS NAME01,                               ";
                                //SQL = SQL + ComNum.VBLF + "     (SELECT BCODE FROM KOSMOS_PMPA.BAS_SUN                      ";
                                //SQL = SQL + ComNum.VBLF + "      WHERE TRIM(SUNEXT) = M.CODE01) AS EDICODE01,               ";
                                //SQL = SQL + ComNum.VBLF + "     (SELECT HAMYANG1 || HAMYANG2 FROM KOSMOS_ADM.DRUG_MASTER1   ";
                                //SQL = SQL + ComNum.VBLF + "       WHERE TRIM(JEPCODE) = M.CODE01) AS HAMYANG01,             ";
                                //SQL = SQL + ComNum.VBLF + "     M.CODE02, C2.HNAME AS NAME02,                               ";
                                //SQL = SQL + ComNum.VBLF + "     (SELECT BCODE FROM KOSMOS_PMPA.BAS_SUN                      ";
                                //SQL = SQL + ComNum.VBLF + "      WHERE TRIM(SUNEXT) = M.CODE02) AS EDICODE02,               ";
                                //SQL = SQL + ComNum.VBLF + "     (SELECT HAMYANG1 || HAMYANG2 FROM KOSMOS_ADM.DRUG_MASTER1   ";
                                //SQL = SQL + ComNum.VBLF + "       WHERE TRIM(JEPCODE) = M.CODE02) AS HAMYANG02,             ";
                                //SQL = SQL + ComNum.VBLF + "     M.CODE03, C3.HNAME AS NAME03,                               ";
                                //SQL = SQL + ComNum.VBLF + "     (SELECT BCODE FROM KOSMOS_PMPA.BAS_SUN                      ";
                                //SQL = SQL + ComNum.VBLF + "      WHERE TRIM(SUNEXT) = M.CODE03) AS EDICODE03,               ";
                                //SQL = SQL + ComNum.VBLF + "     (SELECT HAMYANG1 || HAMYANG2 FROM KOSMOS_ADM.DRUG_MASTER1   ";
                                //SQL = SQL + ComNum.VBLF + "       WHERE TRIM(JEPCODE) = M.CODE03) AS HAMYANG03,             ";
                                //SQL = SQL + ComNum.VBLF + "     M.CODE04, C4.HNAME AS NAME04,                               ";
                                //SQL = SQL + ComNum.VBLF + "     (SELECT BCODE FROM KOSMOS_PMPA.BAS_SUN                      ";
                                //SQL = SQL + ComNum.VBLF + "      WHERE TRIM(SUNEXT) = M.CODE04) AS EDICODE04,               ";
                                //SQL = SQL + ComNum.VBLF + "     (SELECT HAMYANG1 || HAMYANG2 FROM KOSMOS_ADM.DRUG_MASTER1   ";
                                //SQL = SQL + ComNum.VBLF + "       WHERE TRIM(JEPCODE) = M.CODE04) AS HAMYANG04,             ";
                                //SQL = SQL + ComNum.VBLF + "     M.CODE05, C5.HNAME AS NAME05,                               ";
                                //SQL = SQL + ComNum.VBLF + "     (SELECT BCODE FROM KOSMOS_PMPA.BAS_SUN                      ";
                                //SQL = SQL + ComNum.VBLF + "      WHERE TRIM(SUNEXT) = M.CODE05) AS EDICODE05,               ";
                                //SQL = SQL + ComNum.VBLF + "     (SELECT HAMYANG1 || HAMYANG2 FROM KOSMOS_ADM.DRUG_MASTER1   ";
                                //SQL = SQL + ComNum.VBLF + "       WHERE TRIM(JEPCODE) = M.CODE05) AS HAMYANG05              ";
                                //SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.DRUG_MASTER5 M                                  ";
                                //SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_ADM.DRUG_MASTER2 B                            ";
                                //SQL = SQL + ComNum.VBLF + "     ON TRIM(M.CODE01) = TRIM(B.JEPCODE)                         ";
                                //SQL = SQL + ComNum.VBLF + " LEFT OUTER JOIN KOSMOS_OCS.OCS_DRUGINFO_NEW C1                  ";
                                //SQL = SQL + ComNum.VBLF + "     ON TRIM(M.CODE01) = TRIM(C1.SUNEXT)                         ";
                                //SQL = SQL + ComNum.VBLF + " LEFT OUTER JOIN KOSMOS_OCS.OCS_DRUGINFO_NEW C2                  ";
                                //SQL = SQL + ComNum.VBLF + "     ON TRIM(M.CODE02) = TRIM(C2.SUNEXT)                         ";
                                //SQL = SQL + ComNum.VBLF + " LEFT OUTER JOIN KOSMOS_OCS.OCS_DRUGINFO_NEW C3                  ";
                                //SQL = SQL + ComNum.VBLF + "     ON TRIM(M.CODE03) = TRIM(C3.SUNEXT)                         ";
                                //SQL = SQL + ComNum.VBLF + " LEFT OUTER JOIN KOSMOS_OCS.OCS_DRUGINFO_NEW C4                  ";
                                //SQL = SQL + ComNum.VBLF + "     ON TRIM(M.CODE04) = TRIM(C4.SUNEXT)                         ";
                                //SQL = SQL + ComNum.VBLF + " LEFT OUTER JOIN KOSMOS_OCS.OCS_DRUGINFO_NEW C5                  ";
                                //SQL = SQL + ComNum.VBLF + "     ON TRIM(M.CODE05) = TRIM(C5.SUNEXT)                         ";
                                //SQL = SQL + ComNum.VBLF + " WHERE M.GUBUN = '9777'                                          ";

                                //switch (cboBun.Text.Trim())
                                //{
                                //    case "주사":
                                //        SQL = SQL + ComNum.VBLF + "     AND B.SUGABUN = '20'                                ";                             
                                //        break;
                                //    case "경구":
                                //        SQL = SQL + ComNum.VBLF + "     AND B.SUGABUN = '11'                                ";
                                //        break;
                                //    case "외용":
                                //        SQL = SQL + ComNum.VBLF + "     AND B.SUGABUN = '12'                                ";
                                //        break;
                                //}

                                //SQL = SQL + ComNum.VBLF + "   AND EXISTS";
                                //SQL = SQL + ComNum.VBLF + "       (SELECT * FROM KOSMOS_PMPA.BAS_SUT S";
                                //SQL = SQL + ComNum.VBLF + "           WHERE DELDATE IS NULL";
                                //SQL = SQL + ComNum.VBLF + "               AND S.SUGBJ IN('2', '3', '4')";
                                //SQL = SQL + ComNum.VBLF + "               AND M.CODE01 = TRIM(S.SUNEXT))       ";
                                #endregion

                                SQL = "";
                                SQL = SQL + ComNum.VBLF + " SELECT                                                              ";
                                SQL = SQL + ComNum.VBLF + "      SNAME GRP, A.GRPNAME, B.SUNEXT, BCODE AS EDICODE,              ";
                                SQL = SQL + ComNum.VBLF + "      HNAME, HAMYANG1 || HAMYANG2  AS HAMYANG                        ";
                                SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_OCS.OCS_DRUGINFO_COMPONENT A                         ";
                                SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_OCS.OCS_DRUGINFO_NEW B                         ";
                                SQL = SQL + ComNum.VBLF + "      ON A.SUNEXT = B.SUNEXT                                         ";
                                SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_PMPA.BAS_SUN C                                 ";
                                SQL = SQL + ComNum.VBLF + "      ON A.SUNEXT = C.SUNEXT                                         ";
                                SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_ADM.DRUG_MASTER1 D                             ";
                                SQL = SQL + ComNum.VBLF + "      ON TRIM(B.SUNEXT) = TRIM(D.JEPCODE)                            ";
                                SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_ADM.DRUG_MASTER2 E                             ";
                                SQL = SQL + ComNum.VBLF + "      ON TRIM(B.SUNEXT) = TRIM(E.JEPCODE)                            ";
                                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                                        ";
                                SQL = SQL + ComNum.VBLF + "  AND GRPNAME NOT IN(SELECT GRPNAME                                ";
                                SQL = SQL + ComNum.VBLF + "                        FROM KOSMOS_OCS.OCS_DRUGINFO_COMPONENT A     ";
                                SQL = SQL + ComNum.VBLF + "                          INNER JOIN KOSMOS_OCS.OCS_DRUGINFO_NEW B   ";
                                SQL = SQL + ComNum.VBLF + "                            ON A.SUNEXT = B.SUNEXT                   ";
                                SQL = SQL + ComNum.VBLF + "                          INNER JOIN KOSMOS_PMPA.BAS_SUN C           ";
                                SQL = SQL + ComNum.VBLF + "                            ON A.SUNEXT = C.SUNEXT                   ";
                                SQL = SQL + ComNum.VBLF + "                          INNER JOIN KOSMOS_ADM.DRUG_MASTER1 D       ";
                                SQL = SQL + ComNum.VBLF + "                            ON TRIM(B.SUNEXT) = TRIM(D.JEPCODE)      ";
                                SQL = SQL + ComNum.VBLF + "                          INNER JOIN KOSMOS_ADM.DRUG_MASTER2 E       ";
                                SQL = SQL + ComNum.VBLF + "                            ON TRIM(B.SUNEXT) = TRIM(E.JEPCODE)      ";
                                SQL = SQL + ComNum.VBLF + "                        WHERE EXISTS                                 ";
                                SQL = SQL + ComNum.VBLF + "                          (SELECT * FROM KOSMOS_PMPA.BAS_SUT S       ";
                                SQL = SQL + ComNum.VBLF + "                              WHERE DELDATE IS NULL                  ";
                                SQL = SQL + ComNum.VBLF + "                                  AND S.SUGBJ IN('2', '3', '4')      ";
                                SQL = SQL + ComNum.VBLF + "                                  AND A.SUNEXT = S.SUNEXT)           ";
                                SQL = SQL + ComNum.VBLF + "                         GROUP BY GRPNAME                            ";
                                SQL = SQL + ComNum.VBLF + "                         HAVING COUNT(GRPNAME) <= 1)                 ";
                                    
                                switch (cboBun.Text.Trim())
                                {
                                    case "주사":
                                        SQL = SQL + ComNum.VBLF + "     AND E.SUGABUN = '20'                                ";
                                        break;
                                    case "경구":
                                        SQL = SQL + ComNum.VBLF + "     AND E.SUGABUN = '11'                                ";
                                        break;
                                    case "외용":
                                        SQL = SQL + ComNum.VBLF + "     AND E.SUGABUN = '12'                                ";
                                        break;
                                }
                                
                                SQL = SQL + ComNum.VBLF + " AND EXISTS                                                      ";
                                SQL = SQL + ComNum.VBLF + "     (SELECT * FROM KOSMOS_PMPA.BAS_SUT S                        ";
                                SQL = SQL + ComNum.VBLF + "         WHERE DELDATE IS NULL                                   ";
                                SQL = SQL + ComNum.VBLF + "             AND S.SUGBJ IN('2', '3', '4')                       ";
                                SQL = SQL + ComNum.VBLF + "             AND A.SUNEXT = S.SUNEXT)                            ";                            
                                SQL = SQL + ComNum.VBLF + " ORDER BY GRPNAME, SUNEXT                                        ";

                                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    Cursor.Current = Cursors.Default;
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }

                                if (dt.Rows.Count > 0)
                                {                                    
                                    for (i = 0; i < dt.Rows.Count; i++)
                                    {
                                        if (strGrpName != dt.Rows[i]["GRPNAME"].ToString().Trim())
                                        {
                                            ssSame_Sungbun_Sheet1.RowCount = ssSame_Sungbun_Sheet1.RowCount + 1;
                                            ssSame_Sungbun_Sheet1.SetRowHeight(-1, (ComNum.SPDROWHT * 2));

                                            strGrpName = dt.Rows[i]["GRPNAME"].ToString().Trim();
                                            ssSame_Sungbun_Sheet1.Cells[iRow, 0].Text = (iRow + 1).ToString();
                                            ssSame_Sungbun_Sheet1.Cells[iRow, 1].Text = dt.Rows[i]["GRPNAME"].ToString().Trim();

                                            READ_MEDICINE_IMG(dt.Rows[i]["EDICODE"].ToString().Trim(), ssSame_Sungbun, iRow, 2);
                                            ssSame_Sungbun_Sheet1.Cells[iRow, 3].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                                            ssSame_Sungbun_Sheet1.Cells[iRow, 4].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                                            ssSame_Sungbun_Sheet1.Cells[iRow, 5].Text = dt.Rows[i]["HAMYANG"].ToString().Trim();

                                            iRow = iRow + 1;
                                            iCol = 1;
                                        }
                                        else
                                        {
                                            switch (iCol)
                                            {
                                                case 1:
                                                    READ_MEDICINE_IMG(dt.Rows[i]["EDICODE"].ToString().Trim(), ssSame_Sungbun, iRow - 1, 6);
                                                    ssSame_Sungbun_Sheet1.Cells[iRow - 1, 7].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                                                    ssSame_Sungbun_Sheet1.Cells[iRow - 1, 8].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                                                    ssSame_Sungbun_Sheet1.Cells[iRow - 1, 9].Text = dt.Rows[i]["HAMYANG"].ToString().Trim();
                                                    break;
                                                case 2:
                                                    READ_MEDICINE_IMG(dt.Rows[i]["EDICODE"].ToString().Trim(), ssSame_Sungbun, iRow - 1, 10);
                                                    ssSame_Sungbun_Sheet1.Cells[iRow - 1, 11].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                                                    ssSame_Sungbun_Sheet1.Cells[iRow - 1, 12].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                                                    ssSame_Sungbun_Sheet1.Cells[iRow - 1, 13].Text = dt.Rows[i]["HAMYANG"].ToString().Trim();
                                                    break;
                                                case 3:
                                                    READ_MEDICINE_IMG(dt.Rows[i]["EDICODE"].ToString().Trim(), ssSame_Sungbun, iRow - 1, 14);
                                                    ssSame_Sungbun_Sheet1.Cells[iRow - 1, 15].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                                                    ssSame_Sungbun_Sheet1.Cells[iRow - 1, 16].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                                                    ssSame_Sungbun_Sheet1.Cells[iRow - 1, 17].Text = dt.Rows[i]["HAMYANG"].ToString().Trim();
                                                    break;
                                                case 4:
                                                    READ_MEDICINE_IMG(dt.Rows[i]["EDICODE"].ToString().Trim(), ssSame_Sungbun, iRow - 1, 18);
                                                    ssSame_Sungbun_Sheet1.Cells[iRow - 1, 19].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                                                    ssSame_Sungbun_Sheet1.Cells[iRow - 1, 20].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                                                    ssSame_Sungbun_Sheet1.Cells[iRow - 1, 21].Text = dt.Rows[i]["HAMYANG"].ToString().Trim();
                                                    break;
                                            }

                                            iCol = iCol + 1;
                                        }                                        
                                    }
                                }
                                dt.Dispose();
                                dt = null;                                
                                break;
                        }
                        break;
                    case "4":
                        // 백신
                        ssView_Sheet1.Columns[0 + 1].Visible = false;           //성분
                        ssView_Sheet1.Columns[1 + 1].Visible = true;            //의약품코드
                        ssView_Sheet1.Columns[2 + 1].Visible = true;            //의약품명
                        ssView_Sheet1.Columns[3 + 1].Visible = true;            //함량/단위
                        ssView_Sheet1.Columns[4 + 1].Visible = true;            //보관온도
                        ssView_Sheet1.Columns[5 + 1].Visible = true;            //차광여부
                        ssView_Sheet1.Columns[6 + 1].Visible = true;            //효능
                        ssView_Sheet1.Columns[7 + 1].Visible = false;           //유사의약품
                        ssView_Sheet1.Columns[8 + 1].Visible = false;           //유사의약품명
                        ssView_Sheet1.Columns[9 + 1].Visible = false;           //색상/모양
                        ssView_Sheet1.Columns[10 + 1].Visible = false;          //투여경로
                        ssView_Sheet1.Columns[11 + 1].Visible = false;          //1회 최대용량
                        ssView_Sheet1.Columns[12 + 1].Visible = false;          //1일 최대용량
                        ssView_Sheet1.Columns[13 + 1].Visible = false;          //적응증
                        ssView_Sheet1.Columns[14 + 1].Visible = false;          //실시기준(공통)
                        ssView_Sheet1.Columns[15 + 1].Visible = false;          //복지부분료
                        ssView_Sheet1.Columns[16 + 1].Visible = false;          //분류명칭

                        SQL = "";
                        #region // 사용안함
                        //SQL = "SELECT";
                        //SQL = SQL + ComNum.VBLF + "     'C' AS CLASS, A.EFFECT, A.SUNEXT, A.HNAME, A.UNIT, B.SAVETEMP, DECODE(B.SAVEBRIGHT, '1', '차광', '') AS SAVEBRIGHT, B.SAVEBRIGHTETC";
                        //SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUGINFO_NEW A, " + ComNum.DB_ERP + "DRUG_MASTER2 B, " + ComNum.DB_ERP + "DRUG_BUN_LIST C";
                        //SQL = SQL + ComNum.VBLF + "     WHERE A.SUNEXT = B.JEPCODE(+)";
                        //SQL = SQL + ComNum.VBLF + "         AND A.SUNEXT = C.JEPCODE";
                        //SQL = SQL + ComNum.VBLF + "         AND C.GUBUN = '05'";
                        #endregion

                        SQL = SQL + ComNum.VBLF + " SELECT ";
                        SQL = SQL + ComNum.VBLF + "     'C' AS CLASS, A.EFFECT, A.SUNEXT, A.HNAME, A.UNIT, B.SAVETEMP,  ";
                        SQL = SQL + ComNum.VBLF + "     DECODE(B.SAVEBRIGHT, '1', '차광', '') AS SAVEBRIGHT, B.SAVEBRIGHTETC ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_DRUGINFO_NEW A, KOSMOS_ADM.DRUG_MASTER2 B, KOSMOS_ADM.DRUG_MASTER5 C ";
                        SQL = SQL + ComNum.VBLF + "      WHERE A.SUNEXT = B.JEPCODE(+) ";
                        SQL = SQL + ComNum.VBLF + "          AND TRIM(A.SUNEXT) = C.CODE01 ";
                        SQL = SQL + ComNum.VBLF + "          AND C.GUBUN = '10722' ";
                        SQL = SQL + ComNum.VBLF + "          AND C.DELDATE IS NULL ";
                        
                        switch (cboBun.Text.Trim())
                        {
                            case "전체":
                                SQL = SQL + ComNum.VBLF + "         AND B.SUGABUN IN ('11','12','20','23')";
                                break;
                            case "주사":
                                SQL = SQL + ComNum.VBLF + "         AND B.SUGABUN IN ('20','23')";
                                break;
                            case "경구":
                                SQL = SQL + ComNum.VBLF + "         AND B.SUGABUN IN ('11')";
                                break;
                            case "외용":
                                SQL = SQL + ComNum.VBLF + "         AND B.SUGABUN IN ('12')";
                                break;
                        }

                        SQL = SQL + ComNum.VBLF + "   AND EXISTS";
                        SQL = SQL + ComNum.VBLF + "       (SELECT * FROM KOSMOS_PMPA.BAS_SUT S";
                        SQL = SQL + ComNum.VBLF + "           WHERE DELDATE IS NULL";
                        SQL = SQL + ComNum.VBLF + "               AND S.SUGBJ IN('2', '3', '4')";
                        SQL = SQL + ComNum.VBLF + "               AND A.SUNEXT = S.SUNEXT)       ";

                        SQL = SQL + ComNum.VBLF + "ORDER BY EFFECT, SUNEXT";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            ssView_Sheet1.RowCount = dt.Rows.Count;
                            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                ssView_Sheet1.Cells[i, 1 + 1].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                                ssView_Sheet1.Cells[i, 2 + 1].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                                ssView_Sheet1.Cells[i, 3 + 1].Text = dt.Rows[i]["UNIT"].ToString().Trim();
                                ssView_Sheet1.Cells[i, 4 + 1].Text = dt.Rows[i]["SAVETEMP"].ToString().Trim();
                                ssView_Sheet1.Cells[i, 5 + 1].Text = dt.Rows[i]["SAVEBRIGHT"].ToString().Trim();
                                ssView_Sheet1.Cells[i, 6 + 1].Text = dt.Rows[i]["EFFECT"].ToString().Trim();
                            }
                        }

                        dt.Dispose();
                        dt = null;
                        break;
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
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        //위험 의약품 목록
        private void READ_DATA_D(string strGubun)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            ssView.Visible = true;
            ssSpread = ssView;

            ssView_Sheet1.Columns[0].Visible = false;               //분류
            ssView_Sheet1.Columns[0 + 1].Visible = true;            //성분
            ssView_Sheet1.Columns[1 + 1].Visible = true;            //의약품코드
            ssView_Sheet1.Columns[2 + 1].Visible = true;            //의약품명
            ssView_Sheet1.Columns[3 + 1].Visible = true;            //함량/단위
            ssView_Sheet1.Columns[4 + 1].Visible = false;           //보관온도
            ssView_Sheet1.Columns[5 + 1].Visible = false;           //차광여부
            ssView_Sheet1.Columns[6 + 1].Visible = false;           //효능
            ssView_Sheet1.Columns[7 + 1].Visible = false;           //유사의약품
            ssView_Sheet1.Columns[8 + 1].Visible = false;           //유사의약품명
            ssView_Sheet1.Columns[9 + 1].Visible = false;           //색상/모양
            ssView_Sheet1.Columns[10 + 1].Visible = true;           //투여경로
            ssView_Sheet1.Columns[11 + 1].Visible = true;           //1회 최대용량
            ssView_Sheet1.Columns[12 + 1].Visible = true;           //1일 최대용량
            ssView_Sheet1.Columns[13 + 1].Visible = true;           //적응증
            ssView_Sheet1.Columns[14 + 1].Visible = true;           //실시기준(공통)
            ssView_Sheet1.Columns[15 + 1].Visible = false;          //복지부분료
            ssView_Sheet1.Columns[16 + 1].Visible = false;          //분류명칭

            ssView.Dock = DockStyle.Fill;
            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     F.HNAME, F.SNAME, A.SUCODE, A.ORDERNAME, F.UNIT, ";
                SQL = SQL + ComNum.VBLF + "     MAXQTY_1TIME_CONTENTS, MAXQTY_1TIME_QTY, MAXQTY_1TIME_UNIT, MAXQTY_1DAY_CONTENTS, MAXQTY_1DAY_UNIT, ";
                SQL = SQL + ComNum.VBLF + "     MAXQTY_CNT, MAXQTY_EFFECT, MAXQTY_GUBUN1, MAXQTY_GUBUN2";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ORDERCODE a, " + ComNum.DB_PMPA + "BAS_SUT b, " + ComNum.DB_ERP + "DRUG_MASTER3 c, " + ComNum.DB_ERP + "DRUG_MASTER4 d, " + ComNum.DB_MED + "OCS_DRUGINFO_NEW f, " + ComNum.DB_ERP + "DRUG_MASTER2 g";
                SQL = SQL + ComNum.VBLF + "     WHERE a.SuCode = b.SuCode(+)";
                SQL = SQL + ComNum.VBLF + "         AND a.SuCode = f.SuNext(+)";
                SQL = SQL + ComNum.VBLF + "         AND TRIM(a.SuCODE) = TRIM(c.JEPCODE)";
                SQL = SQL + ComNum.VBLF + "         AND c.JEPCODE = d.JEPCODE";
                SQL = SQL + ComNum.VBLF + "         AND c.JEPCODE = g.JEPCODE";
                SQL = SQL + ComNum.VBLF + "         AND c.PRN = '1'";
                SQL = SQL + ComNum.VBLF + "         AND a.Slipno IN ( '0003','0004','0005')";
                SQL = SQL + ComNum.VBLF + "         AND a.Seqno <> 0";
                SQL = SQL + ComNum.VBLF + "         AND a.SendDept <> 'N'";
                SQL = SQL + ComNum.VBLF + "         AND (a.GbSub <> '1' OR a.GbSub IS NULL)";
                SQL = SQL + ComNum.VBLF + "         AND (b.DelDate IS NULL OR b.DelDate = '')";

                switch (cboBun.Text.Trim())
                {
                    case "전체":
                        SQL = SQL + ComNum.VBLF + "         AND g.SUGABUN IN ('11','12','20','23')";
                        break;
                    case "주사":
                        SQL = SQL + ComNum.VBLF + "         AND g.SUGABUN IN ('20','23')";
                        break;
                    case "경구":
                        SQL = SQL + ComNum.VBLF + "         AND g.SUGABUN IN ('11')";
                        break;
                    case "외용":
                        SQL = SQL + ComNum.VBLF + "         AND g.SUGABUN IN ('12')";
                        break;
                }

                SQL = SQL + ComNum.VBLF + "   AND EXISTS";
                SQL = SQL + ComNum.VBLF + "       (SELECT * FROM KOSMOS_PMPA.BAS_SUT S";
                SQL = SQL + ComNum.VBLF + "           WHERE DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "               AND S.SUGBJ IN('2', '3', '4')";
                SQL = SQL + ComNum.VBLF + "               AND A.SUCODE = S.SUNEXT)       ";

                SQL = SQL + ComNum.VBLF + "ORDER BY F.HNAME";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0 + 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1 + 1].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2 + 1].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3 + 1].Text = dt.Rows[i]["UNIT"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 10 + 1].Text = Read_PRN_IV_CHK(dt.Rows[i]["SUCODE"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 11 + 1].Text = dt.Rows[i]["MAXQTY_1TIME_CONTENTS"].ToString().Trim() + dt.Rows[i]["MAXQTY_1TIME_UNIT"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 12 + 1].Text = dt.Rows[i]["MAXQTY_1DAY_CONTENTS"].ToString().Trim() + dt.Rows[i]["MAXQTY_1DAY_UNIT"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 13 + 1].Text = dt.Rows[i]["MAXQTY_GUBUN1"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 14 + 1].Text = dt.Rows[i]["MAXQTY_GUBUN2"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private string Read_PRN_IV_CHK(string strSuCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     a.Bun,";
                SQL = SQL + ComNum.VBLF + "     g.TUYAKPATH_11_1, g.TUYAKPATH_11_2, g.TUYAKPATH_11_ETC, ";
                SQL = SQL + ComNum.VBLF + "     g.TUYAKPATH_12_1, g.TUYAKPATH_12_2, g.TUYAKPATH_12_3, ";
                SQL = SQL + ComNum.VBLF + "     g.TUYAKPATH_12_4, g.TUYAKPATH_12_5, g.TUYAKPATH_12_6, g.TUYAKPATH_12_ETC, ";
                SQL = SQL + ComNum.VBLF + "     g.TUYAKPATH_20_1, g.TUYAKPATH_20_2, g.TUYAKPATH_20_3, g.TUYAKPATH_20_4, g.TUYAKPATH_20_ETC";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ORDERCODE a, " + ComNum.DB_ERP + "DRUG_MASTER1 g ";
                SQL = SQL + ComNum.VBLF + "     WHERE TRIM(a.SuCODE) = TRIM(g.JEPCODE) ";
                SQL = SQL + ComNum.VBLF + "         AND JEPCODE = '" + strSuCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    switch (dt.Rows[0]["BUN"].ToString().Trim())
                    {
                        case "11":
                            if (dt.Rows[0]["TUYAKPATH_11_1"].ToString().Trim() == "1") { rtnVal = rtnVal + "경구,"; }
                            if (dt.Rows[0]["TUYAKPATH_11_2"].ToString().Trim() == "1") { rtnVal = rtnVal + "설하,"; }

                            //2021-02-08 추가
                            if ((dt.Rows[0]["TUYAKPATH_11_1"].ToString().Trim() == "0"
                                && dt.Rows[0]["TUYAKPATH_11_2"].ToString().Trim() == "0") && dt.Rows[0]["TUYAKPATH_11_ETC"].ToString().Trim() != "")
                            {
                                rtnVal = rtnVal + dt.Rows[0]["TUYAKPATH_11_ETC"].ToString().Trim() +","; 
                            } 
                            break;
                        case "12":
                            if (dt.Rows[0]["TUYAKPATH_12_1"].ToString().Trim() == "1") { rtnVal = rtnVal + "피부,"; }
                            if (dt.Rows[0]["TUYAKPATH_12_2"].ToString().Trim() == "1") { rtnVal = rtnVal + "눈,"; }
                            if (dt.Rows[0]["TUYAKPATH_12_3"].ToString().Trim() == "1") { rtnVal = rtnVal + "코,"; }
                            if (dt.Rows[0]["TUYAKPATH_12_4"].ToString().Trim() == "1") { rtnVal = rtnVal + "직장,"; }
                            if (dt.Rows[0]["TUYAKPATH_12_5"].ToString().Trim() == "1") { rtnVal = rtnVal + "폐,"; }
                            if (dt.Rows[0]["TUYAKPATH_12_6"].ToString().Trim() == "1") { rtnVal = rtnVal + "질,"; }
                            break;
                        case "20":
                            if (dt.Rows[0]["TUYAKPATH_20_1"].ToString().Trim() == "1") { rtnVal = rtnVal + "IM,"; }
                            if (dt.Rows[0]["TUYAKPATH_20_2"].ToString().Trim() == "1") { rtnVal = rtnVal + "IV,"; }
                            if (dt.Rows[0]["TUYAKPATH_20_3"].ToString().Trim() == "1") { rtnVal = rtnVal + "IV infusion,"; }
                            if (dt.Rows[0]["TUYAKPATH_20_4"].ToString().Trim() == "1") { rtnVal = rtnVal + "SC,"; }
                            break;
                    }
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        //마약류 목록
        private void READ_DATA_E()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssView.Visible = true;
            ssSpread = ssView;

            ssView_Sheet1.Columns[0].Visible = false;               //분류
            ssView_Sheet1.Columns[0 + 1].Visible = true;            //성분
            ssView_Sheet1.Columns[1 + 1].Visible = true;            //의약품코드
            ssView_Sheet1.Columns[2 + 1].Visible = true;            //의약품명
            ssView_Sheet1.Columns[3 + 1].Visible = true;            //함량/단위
            ssView_Sheet1.Columns[4 + 1].Visible = true;            //보관온도
            ssView_Sheet1.Columns[5 + 1].Visible = true;            //차광여부
            ssView_Sheet1.Columns[6 + 1].Visible = false;           //효능
            ssView_Sheet1.Columns[7 + 1].Visible = false;           //유사의약품
            ssView_Sheet1.Columns[8 + 1].Visible = false;           //유사의약품명
            ssView_Sheet1.Columns[9 + 1].Visible = false;           //색상/모양
            ssView_Sheet1.Columns[10 + 1].Visible = false;          //투여경로
            ssView_Sheet1.Columns[11 + 1].Visible = false;          //1회 최대용량
            ssView_Sheet1.Columns[12 + 1].Visible = false;          //1일 최대용량
            ssView_Sheet1.Columns[13 + 1].Visible = false;          //적응증
            ssView_Sheet1.Columns[14 + 1].Visible = false;          //실시기준(공통)
            ssView_Sheet1.Columns[15 + 1].Visible = false;          //복지부분료
            ssView_Sheet1.Columns[16 + 1].Visible = false;          //분류명칭

            ssView.Dock = DockStyle.Fill;
            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     'E' AS CLASS, A.SNAME, A.SUNEXT, A.HNAME, A.UNIT, B.SAVETEMP, DECODE(B.SAVEBRIGHT, '1', '차광', '') AS SAVEBRIGHT";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUGINFO_NEW A, " + ComNum.DB_ERP + "DRUG_MASTER2 B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.SUNEXT = B.JEPCODE(+)";

                switch (cboBun.Text.Trim())
                {
                    case "전체":
                        SQL = SQL + ComNum.VBLF + "         AND B.SUGABUN IN ('11','12','20','23')";
                        break;
                    case "주사":
                        SQL = SQL + ComNum.VBLF + "         AND B.SUGABUN IN ('20','23')";
                        break;
                    case "경구":
                        SQL = SQL + ComNum.VBLF + "         AND B.SUGABUN IN ('11')";
                        break;
                    case "외용":
                        SQL = SQL + ComNum.VBLF + "         AND B.SUGABUN IN ('12')";
                        break;
                }

                SQL = SQL + ComNum.VBLF + "         AND A.SUNEXT IN";
                SQL = SQL + ComNum.VBLF + "                     (SELECT";
                SQL = SQL + ComNum.VBLF + "                         JEPCODE";
                SQL = SQL + ComNum.VBLF + "                     From " + ComNum.DB_ERP + "DRUG_SETCODE";
                SQL = SQL + ComNum.VBLF + "                         WHERE GUBUN = '13'";
                SQL = SQL + ComNum.VBLF + "                             AND (DELDATE IS NULL or DelDate = ''))";
                SQL = SQL + ComNum.VBLF + "         AND EXISTS";
                SQL = SQL + ComNum.VBLF + "                 (SELECT * FROM " + ComNum.DB_PMPA + "BAS_SUT S";
                SQL = SQL + ComNum.VBLF + "                     WHERE DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "                         AND S.SUGBJ IN ('2', '3', '4')";
                SQL = SQL + ComNum.VBLF + "                         AND A.SUNEXT = S.SUNEXT)";
                SQL = SQL + ComNum.VBLF + "ORDER BY SUNEXT ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0 + 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1 + 1].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2 + 1].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3 + 1].Text = dt.Rows[i]["UNIT"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4 + 1].Text = dt.Rows[i]["SAVETEMP"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5 + 1].Text = dt.Rows[i]["SAVEBRIGHT"].ToString().Trim();
                    }
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        //마약류 목록
        private void READ_DATA_F()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssView.Visible = true;
            ssSpread = ssView;

            ssView_Sheet1.Columns[0].Visible = false;               //분류
            ssView_Sheet1.Columns[0 + 1].Visible = true;            //성분
            ssView_Sheet1.Columns[1 + 1].Visible = true;            //의약품코드
            ssView_Sheet1.Columns[2 + 1].Visible = true;            //의약품명
            ssView_Sheet1.Columns[3 + 1].Visible = true;            //함량/단위
            ssView_Sheet1.Columns[4 + 1].Visible = true;            //보관온도
            ssView_Sheet1.Columns[5 + 1].Visible = true;            //차광여부
            ssView_Sheet1.Columns[6 + 1].Visible = false;           //효능
            ssView_Sheet1.Columns[7 + 1].Visible = false;           //유사의약품
            ssView_Sheet1.Columns[8 + 1].Visible = false;           //유사의약품명
            ssView_Sheet1.Columns[9 + 1].Visible = false;           //색상/모양
            ssView_Sheet1.Columns[10 + 1].Visible = false;          //투여경로
            ssView_Sheet1.Columns[11 + 1].Visible = false;          //1회 최대용량
            ssView_Sheet1.Columns[12 + 1].Visible = false;          //1일 최대용량
            ssView_Sheet1.Columns[13 + 1].Visible = false;          //적응증
            ssView_Sheet1.Columns[14 + 1].Visible = false;          //실시기준(공통)
            ssView_Sheet1.Columns[15 + 1].Visible = false;          //복지부분료
            ssView_Sheet1.Columns[16 + 1].Visible = false;          //분류명칭

            ssView.Dock = DockStyle.Fill;
            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     'E' CLASS, A.SNAME, A.SUNEXT, A.HNAME, A.UNIT, B.SAVETEMP, DECODE(B.SAVEBRIGHT, '1', '차광', '') SAVEBRIGHT";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUGINFO_NEW A, " + ComNum.DB_ERP + "DRUG_MASTER2 B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.SUNEXT = B.JEPCODE(+)";

                switch (cboBun.Text.Trim())
                {
                    case "전체":
                        SQL = SQL + ComNum.VBLF + "         AND B.SUGABUN IN ('11','12','20','23')";
                        break;
                    case "주사":
                        SQL = SQL + ComNum.VBLF + "         AND B.SUGABUN IN ('20','23')";
                        break;
                    case "경구":
                        SQL = SQL + ComNum.VBLF + "         AND B.SUGABUN IN ('11')";
                        break;
                    case "외용":
                        SQL = SQL + ComNum.VBLF + "         AND B.SUGABUN IN ('12')";
                        break;
                }

                SQL = SQL + ComNum.VBLF + "         AND A.SUNEXT IN";
                SQL = SQL + ComNum.VBLF + "                     (SELECT";
                SQL = SQL + ComNum.VBLF + "                         JEPCODE";
                SQL = SQL + ComNum.VBLF + "                     From " + ComNum.DB_ERP + "DRUG_SPECIAL_JEPCODE";
                SQL = SQL + ComNum.VBLF + "                         WHERE SEQNO = 7)";
                SQL = SQL + ComNum.VBLF + "         AND EXISTS";
                SQL = SQL + ComNum.VBLF + "                 (SELECT * FROM " + ComNum.DB_PMPA + "BAS_SUT S";
                SQL = SQL + ComNum.VBLF + "                     WHERE DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "                         AND S.SUGBJ IN ('2', '3', '4')";
                SQL = SQL + ComNum.VBLF + "                         AND A.SUNEXT = S.SUNEXT)";
                SQL = SQL + ComNum.VBLF + "ORDER BY SUNEXT ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0 + 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1 + 1].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2 + 1].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3 + 1].Text = dt.Rows[i]["UNIT"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4 + 1].Text = dt.Rows[i]["SAVETEMP"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5 + 1].Text = dt.Rows[i]["SAVEBRIGHT"].ToString().Trim();
                    }
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void READ_DATA_H()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssView.Visible = true;
            ssSpread = ssView;

            ssView_Sheet1.Columns[0].Visible  = true;           //성분
            ssView_Sheet1.Columns[0 + 1].Visible = true;           //성분
            ssView_Sheet1.Columns[1 + 1].Visible = true;           //의약품코드
            ssView_Sheet1.Columns[2 + 1].Visible  = true;           //의약품명
            ssView_Sheet1.Columns[3 + 1].Visible  = true;           //함량/단위
            ssView_Sheet1.Columns[4 + 1].Visible  = true;           //보관온도
            ssView_Sheet1.Columns[5 + 1].Visible  = true;           //차광여부
            ssView_Sheet1.Columns[6 + 1].Visible  = false;          //효능
            ssView_Sheet1.Columns[7 + 1].Visible  = false;          //유사의약품
            ssView_Sheet1.Columns[8 + 1].Visible  = false;          //유사의약품명
            ssView_Sheet1.Columns[9 + 1].Visible  = false;          //색상/모양
            ssView_Sheet1.Columns[10 + 1].Visible = false;          //투여경로
            ssView_Sheet1.Columns[11 + 1].Visible = false;          //1회 최대용량
            ssView_Sheet1.Columns[12 + 1].Visible = false;          //1일 최대용량
            ssView_Sheet1.Columns[13 + 1].Visible = false;          //적응증
            ssView_Sheet1.Columns[14 + 1].Visible = false;          //실시기준(공통)
            ssView_Sheet1.Columns[15 + 1].Visible = false;          //복지부분료
            ssView_Sheet1.Columns[16 + 1].Visible = false;          //분류명칭

            ssView.Dock = DockStyle.Fill;
            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "  SELECT                                                                                                               ";
                SQL = SQL + ComNum.VBLF + "     A.JEPCODE                                                                                   ";
                SQL = SQL + ComNum.VBLF + ",    A.JEPNAMEK                                                                                  ";
                SQL = SQL + ComNum.VBLF + ",    A.JEPNAMEE                                                                                  ";
                SQL = SQL + ComNum.VBLF + ",    (SELECT LISTAGG(SUNGBUN, ', ')  WITHIN GROUP(ORDER BY RANKING) FROM KOSMOS_ADM.DRUG_MASTER1_SUNGBUN WHERE JEPCODE = A.JEPCODE) AS SUNGBUN  ";
                SQL = SQL + ComNum.VBLF + ",    (SELECT UNIT FROM KOSMOS_OCS.OCS_DRUGINFO_NEW WHERE SUNEXT = A.JEPCODE) AS UNIT             ";
                SQL = SQL + ComNum.VBLF + ",    B.EFFECT                                                                                    ";
                SQL = SQL + ComNum.VBLF + ",    B.BOKGIBUN                                                                                  ";
                SQL = SQL + ComNum.VBLF + ",    B.SAVETEMP                                                                                  ";
                SQL = SQL + ComNum.VBLF + ",    DECODE(B.SAVEBRIGHT, '1', '차광', '') SAVEBRIGHT                                             ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "DRUG_MASTER1 A                                                        ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_ERP + "DRUG_MASTER2 B                                                   ";
                SQL = SQL + ComNum.VBLF + "   ON A.JEPCODE = B.JEPCODE                                                                      ";
                SQL = SQL + ComNum.VBLF + "  AND B.ETCBUN1 = 'Y'                                                                            ";
                SQL = SQL + ComNum.VBLF + "  AND EXISTS                                                                                     ";
                SQL = SQL + ComNum.VBLF + "  (                                                                                              ";
                SQL = SQL + ComNum.VBLF + "     SELECT 1                                                                                    ";
                SQL = SQL + ComNum.VBLF + "       FROM " + ComNum.DB_PMPA + "BAS_SUT                                                        ";
                SQL = SQL + ComNum.VBLF + "      WHERE DELDATE IS NULL                                                                      ";
                SQL = SQL + ComNum.VBLF + "        AND SUNEXT = B.JEPCODE                                                                   ";
                SQL = SQL + ComNum.VBLF + "  )                                                                                              ";

                if (cboGubun2.SelectedIndex == 0)
                {

                }
                else if (cboGubun2.SelectedItem.ToString().NotEmpty())
                {
                    SQL = SQL + ComNum.VBLF + "  AND B.EFFECT  = '" + cboGubun2.SelectedItem.ToString() + "'                                             ";
                }
                else if (cboGubun2.SelectedItem.ToString().IsNullOrEmpty())
                {
                    SQL = SQL + ComNum.VBLF + "  AND B.EFFECT  IS NULL                                             ";

                }

                switch (cboBun.Text.Trim())
                {
                    case "경구":
                        SQL = SQL + ComNum.VBLF + "  AND B.SUGABUN = '11'";
                        break;
                    case "주사":
                        SQL = SQL + ComNum.VBLF + "  AND B.SUGABUN IN ('20', '23')";
                        break;
                    case "외용":
                        SQL = SQL + ComNum.VBLF + "  AND B.SUGABUN = '12'";
                        break;
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY B.EFFECT, A.JEPCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0 + 0].Text = dt.Rows[i]["EFFECT"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 0 + 1].Text = dt.Rows[i]["SUNGBUN"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1 + 1].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2 + 1].Text = dt.Rows[i]["JEPNAMEK"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3 + 1].Text = dt.Rows[i]["UNIT"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4 + 1].Text = dt.Rows[i]["SAVETEMP"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5 + 1].Text = dt.Rows[i]["SAVEBRIGHT"].ToString().Trim();
                    }
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        //구두처방 불가
        private void READ_DATA_G()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssView.Visible = true;
            ssSpread = ssView;



            ssView_Sheet1.Columns[0].Visible = false;               //분류
            ssView_Sheet1.Columns[0 + 1].Visible = false;            //성분
            ssView_Sheet1.Columns[1 + 1].Visible = true;            //의약품코드
            ssView_Sheet1.Columns[2 + 1].Visible = true;            //의약품명
            ssView_Sheet1.Columns[3 + 1].Visible = false;            //함량/단위
            ssView_Sheet1.Columns[4 + 1].Visible = false;            //보관온도
            ssView_Sheet1.Columns[5 + 1].Visible = false;            //차광여부
            ssView_Sheet1.Columns[6 + 1].Visible = true;           //효능
            ssView_Sheet1.Columns[7 + 1].Visible = false;           //유사의약품
            ssView_Sheet1.Columns[8 + 1].Visible = false;           //유사의약품명
            ssView_Sheet1.Columns[9 + 1].Visible = false;           //색상/모양
            ssView_Sheet1.Columns[10 + 1].Visible = false;          //투여경로
            ssView_Sheet1.Columns[11 + 1].Visible = false;          //1회 최대용량
            ssView_Sheet1.Columns[12 + 1].Visible = false;          //1일 최대용량
            ssView_Sheet1.Columns[13 + 1].Visible = false;          //적응증
            ssView_Sheet1.Columns[14 + 1].Visible = false;          //실시기준(공통)
            ssView_Sheet1.Columns[15 + 1].Visible = true;          //복지부분료
            ssView_Sheet1.Columns[16 + 1].Visible = true;          //분류명칭

            ssView.Dock = DockStyle.Fill;
            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     'E' CLASS, A.SNAME, A.SUNEXT, A.HNAME, A.EFFECT, A.BUNCODE ";
                SQL = SQL + ComNum.VBLF + "    , (SELECT CLASSNAME FROM KOSMOS_PMPA.BAS_CLASS WHERE CLASSCODE = A.BUNCODE) AS BUN_NM";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUGINFO_NEW A, " + ComNum.DB_ERP + "DRUG_MASTER4 B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.SUNEXT = B.JEPCODE";
                SQL = SQL + ComNum.VBLF + "         AND B.NOT_VERBAL = '1'";
                SQL = SQL + ComNum.VBLF + "         AND EXISTS";
                SQL = SQL + ComNum.VBLF + "                 (SELECT * FROM " + ComNum.DB_PMPA + "BAS_SUT S";
                SQL = SQL + ComNum.VBLF + "                     WHERE DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "                         AND S.SUGBJ IN ('2', '3', '4')";

                switch (cboBun.Text.Trim())
                {
                    case "경구":
                        SQL = SQL + ComNum.VBLF + "                         AND S.BUN = '11'";
                        break;
                    case "주사":
                        SQL = SQL + ComNum.VBLF + "                         AND S.BUN IN ('20', '23')";
                        break;
                    case "외용":
                        SQL = SQL + ComNum.VBLF + "                         AND S.BUN = '12'";
                        break;
                }

                SQL = SQL + ComNum.VBLF + "                         AND A.SUNEXT = S.SUNEXT)";
                SQL = SQL + ComNum.VBLF + "ORDER BY SUNEXT ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {                            
                        ssView_Sheet1.Cells[i, 1 + 1].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2 + 1].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6 + 1].Text = dt.Rows[i]["EFFECT"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 15 + 1].Text = dt.Rows[i]["BUNCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 16 + 1].Text = dt.Rows[i]["BUN_NM"].ToString().Trim();
                    }
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
        
        private string READ_BAS_Class(PsmhDb pDbCon, string strCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            if (strCode.Trim() == "") { return rtnVal; }

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     ClassName";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLASS";
                SQL = SQL + ComNum.VBLF + "     WHERE ClassCode = '" + strCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["ClassName"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

                return rtnVal;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            if (ssSpread == null) { return; }
            if (ssSpread.ActiveSheet.RowCount == 0) { return; }

            string strFont1 = "";
            string strHead1 = "";
            string strFont2 = "";
            string strHead2 = "";
            string strTitle = "";
            bool bolOrientation = false;
            float flotZoom = 1f;

            switch (ssSpread.Name)
            {
                case "ssView":
                    strTitle = VB.Mid(cboGubun.Text, 3, cboGubun.Text.Length);
                    bolOrientation = false;
                    flotZoom = 1f;
                    break;
                case "ssSimilar_Exterior":
                    strTitle = "유사외관";
                    bolOrientation = true;
                    flotZoom = 0.9f;
                    break;
                case "ssSimilar_Pronounce":
                    strTitle = "유사발음";
                    bolOrientation = true;
                    flotZoom = 1f;
                    break;
                case "ssSimilar_Med":
                    strTitle = "유사의약품";
                    bolOrientation = true;
                    flotZoom = 1f;
                    break;
                case "ssSame_Jusa":
                    strTitle = "동일성분_주사";
                    bolOrientation = true;
                    flotZoom = 1f;
                    break;
                case "ssSame_Mouse":
                    strTitle = "동일성분_경구";
                    bolOrientation = true;
                    flotZoom = 0.8f;
                    break;
                case "ssSame_Out":
                    strTitle = "동일성분_외용";
                    bolOrientation = true;
                    flotZoom = 1f;
                    break;
            }

            //2019-08-02 의약품 목록 출력 시 제목 수정(데레사 수녀님 요청사항)
            strTitle = cboGubun.Text.Split('.')[1].Trim();
            if (VB.Left(cboGubun2.Text, 1) != "*")
            {
                strTitle = strTitle + "-" + cboGubun2.Text.Split(',')[0].Trim();
            }
            if (VB.Left(cboGubun3.Text, 1) != "*")
            {
                strTitle = strTitle + "-" + cboGubun3.Text.Split(',')[0].Trim();
            }

            
            strTitle = strTitle + " 목록 " + (cboBun.Text != "" ? "[" + cboBun.Text + "]" : "");

            
            strFont1 = "/fn\"맑은 고딕\" /fz\"15\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"맑은 고딕\" /fz\"12\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + strTitle + "/f1/n";
            strHead2 = "/l/f2" + "출력일시 : " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-") + "/f2/n";

            ssSpread.ActiveSheet.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssSpread.ActiveSheet.PrintInfo.ZoomFactor = flotZoom;

            if (bolOrientation == false)
            {
                ssSpread.ActiveSheet.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            }
            else
            {
                ssSpread.ActiveSheet.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            }
            
            ssSpread.ActiveSheet.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssSpread.ActiveSheet.PrintInfo.Margin.Top = 20;
            ssSpread.ActiveSheet.PrintInfo.Margin.Bottom = 20;
            ssSpread.ActiveSheet.PrintInfo.Margin.Header = 10;
            ssSpread.ActiveSheet.PrintInfo.ShowColor = true;
            ssSpread.ActiveSheet.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssSpread.ActiveSheet.PrintInfo.ShowBorder = true;
            ssSpread.ActiveSheet.PrintInfo.ShowGrid = true;
            ssSpread.ActiveSheet.PrintInfo.ShowShadows = false;
            ssSpread.ActiveSheet.PrintInfo.UseMax = true;
            ssSpread.ActiveSheet.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssSpread.ActiveSheet.PrintInfo.UseSmartPrint = false;            
            ssSpread.ActiveSheet.PrintInfo.ShowPrintDialog = true;
            ssSpread.ActiveSheet.PrintInfo.Preview = false;
            ssSpread.PrintSheet(0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void READ_MEDICINE_IMG(string strEdiCode, FpSpread o, int iRow, int iCol)
        {            
            if (strEdiCode == "") return;

            string strFDRUGCD = READ_FDRUGCD(strEdiCode);

            if (strFDRUGCD != "")
            {
                o.ActiveSheet.Cells[iRow, iCol].Value = READ_IMG(strFDRUGCD);
            }            
        }

        private Image READ_IMG(string strDifKey)
        {
            Image img = null;

            if (strDifKey == "") { return img; }

            Dir_Check(@"c:\cmc\ocsexe\dif");

            string strLocal = "";
            string strPath = "";
            string strHost = "";
            string strImgFileName = "";

            Ftpedt FtpedtX = new Ftpedt();

            if (FtpedtX.FtpConnetBatch("192.168.100.33", "pcnfs", "pcnfs1") == false)
            {
                ComFunc.MsgBox("FTP Server Connect ERROR !!!", "오류");
                return img;
            }

            strImgFileName = strDifKey + ".jpg";

            strLocal = "c:\\cmc\\ocsexe\\dif\\" + strImgFileName;

            strPath = "/pcnfs/firstdis/" + strImgFileName;
            strHost = "/pcnfs/firstdis";

            FileInfo f = new FileInfo(strLocal);

            if (f.Exists == true)
            {
                img = Image.FromFile(strLocal);
            }
            else
            {
                if (FtpedtX.FtpDownloadEx("192.168.100.33", "pcnfs", "pcnfs1", strLocal, strPath, strHost) == true)
                {
                    img = Image.FromFile(strLocal);
                }
            }

            FtpedtX.FtpDisConnetBatch();
            FtpedtX = null;

            return img;
        }

        private void Dir_Check(string sDirPath, string sExe = "*.jpg")
        {
            DirectoryInfo Dir = new DirectoryInfo(sDirPath);

            if (Dir.Exists == false)
            {
                Dir.Create();
            }
            else
            {
                FileInfo[] File = Dir.GetFiles(sExe, SearchOption.AllDirectories);

                foreach (FileInfo file in File)
                {
                    //file.Delete();
                }
            }
        }

        private string READ_FDRUGCD(string strEDICODE)
        {
            string rtnVal = "";

            if (strEDICODE == "") return rtnVal;

            OracleCommand cmd = new OracleCommand();
            PsmhDb pDbCon = clsDB.DbCon;
            OracleDataReader reader = null;
            DataTable dt = new DataTable();

            string pSearchType = "06";
            string pKeyword = strEDICODE;
            string pScope = "02";


            cmd.Connection = pDbCon.Con;
            cmd.InitialLONGFetchSize = 1000;
            cmd.CommandText = "KOSMOS_DRUG.up_DrugSearch";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("pSearchType", OracleDbType.Varchar2, 9, pSearchType, ParameterDirection.Input);
            cmd.Parameters.Add("pKeyword", OracleDbType.Varchar2, 9, pKeyword, ParameterDirection.Input);
            cmd.Parameters.Add("pScope", OracleDbType.Varchar2, 9, pScope, ParameterDirection.Input);

            cmd.Parameters.Add("V_CUR", OracleDbType.RefCursor, ParameterDirection.Output);

            reader = cmd.ExecuteReader();

            dt.Load(reader);
            reader.Dispose();
            reader = null;

            cmd.Dispose();
            cmd = null;

            if (dt == null)
            {
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["FdrugCd"].ToString().Trim();
            }

            return rtnVal;
        }

        private void SETCOMBO_BASBCODEDRUG(PsmhDb pDbCon, ComboBox cbo, string strCode, string strPCode, int Flag = 0)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
                        
            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "       MNAME, MCODE ";
                SQL = SQL + ComNum.VBLF + "  FROM BAS_BCODE_DRUG ";
                SQL = SQL + ComNum.VBLF + " WHERE CODE = '"+ strCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND PCODE = '"+ strPCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DELGB = 0 ";
                SQL = SQL + ComNum.VBLF + "ORDER BY DISPSEQ ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                cbo.Text = "";
                cbo.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    if (Flag == 1)
                    {
                        cbo.Items.Add("*.전체");
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cbo.Items.Add(dt.Rows[i]["MNAME"].ToString().Trim() + VB.Space(50) + "," + dt.Rows[i]["MCODE"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                return;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

                return;
            }
        }

        private void SETCOMBO_Antibiotic(PsmhDb pDbCon, ComboBox cbo)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.EFFECT                     ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.DRUG_MASTER2 A     ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_ADM.DRUG_JEP B    ";
                SQL = SQL + ComNum.VBLF + "    ON A.JEPCODE = B.JEPCODE         ";
                SQL = SQL + ComNum.VBLF + " WHERE A.ETCBUN1 = 'Y'               ";
                SQL = SQL + ComNum.VBLF + " GROUP BY A.EFFECT                   ";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.EFFECT                   ";
             

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                cbo.Text = "";
                cbo.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    cbo.Items.Add("전체");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cbo.Items.Add(dt.Rows[i]["EFFECT"].ToString());
                    }
                }

                dt.Dispose();
                dt = null;

                return;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

                return;
            }
        }
    }
}
