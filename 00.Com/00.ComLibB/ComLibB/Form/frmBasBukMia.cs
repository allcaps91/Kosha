using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmBasBukMia
    /// File Name : frmBasBukMia.cs
    /// Title or Description : 기관조합,거래처 페이지
    /// Author : 박성완
    /// Create Date : 2017-06-14
    /// <history> 
    /// 2017-06-20 frmMail, frmCodehelp에서 데이터 받아오는 부분 추가.
    /// 2017-06-26 텍스트박스 substring 관련 오류 수정
    /// 2017-06-28 이벤트 메소드 정리 및 테스트 작업
    /// </history>
    /// </summary>
    public partial class frmBasBukMia : Form
    {
        #region 필드 선언
        private FrmCodehelp frmCodehelpX;
        private frmMail frmMailX;

        string strClass = "";

        string GstrHelpCode = "";

        #endregion

        #region 메소드
        void Display()
        {
            int nCount1 = 0;
            string strCombo1 = "";

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "SELECT MiaCode, MiaClass, MiaName, MiaTel, MiaDetail,";
                SQL = SQL + ComNum.VBLF + " MiaMisu, MiaJuso, TO_CHAR(DelDate, 'yyyy-MM-dd') DelDate,GbCity,Kiho, ";
                SQL = SQL + ComNum.VBLF + " MiaGubun, TO_CHAR(MiaBdate,'YYYY - MM - DD') MiaBdate, GAMEK ";
                SQL = SQL + ComNum.VBLF + " MIAREMARK , GBCHA , EDITA , GBWON ";
                SQL = SQL + ComNum.VBLF + " FROM BAS_MIA ";
                SQL = SQL + ComNum.VBLF + "WHERE MiaCode = '" + txtMiaCode.Text + "' ";

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
                    return;
                }

                btnSave.Enabled = true;

                dtpDeldate.Text = dt.Rows[0]["DelDate"].ToString().Trim();
                txtMiaName.Text = dt.Rows[0]["MiaName"].ToString().Trim();
                txtTelName.Text = dt.Rows[0]["MiaTel"].ToString().Trim();
                strCombo1 = dt.Rows[0]["MiaClass"].ToString().Trim();
                txtRemark.Text = dt.Rows[0]["MIAREMARK"].ToString().Trim();
                nCount1 = cboMiaClass.Items.Count;
                for (int i = 0; i <= nCount1 - 1; i++)
                {
                    strClass = cboMiaClass.Items[i].ToString().Substring(0, 2);
                    if (strCombo1 == strClass)
                    {
                        cboMiaClass.Text = cboMiaClass.Items[i].ToString();
                        break;
                    }
                }
                //상세분류
                strCombo1 = dt.Rows[0]["MiaDetail"].ToString().Trim();
                nCount1 = cboDetailClass.Items.Count;
                for (int i = 0; i <= nCount1 - 1; i++)
                {
                    strClass = cboDetailClass.Items[i].ToString().Substring(0, 2);
                    if (strCombo1 == strClass)
                    {
                        cboDetailClass.Text = cboDetailClass.Items[i].ToString();
                    }
                }
                //시구지역구분
                strCombo1 = dt.Rows[0]["GbCity"].ToString().Trim();
                nCount1 = cboGbCity.Items.Count;
                for (int i = 0; i <= nCount1 - 1; i++)
                {
                    strClass = cboGbCity.Items[i].ToString().Substring(0, 1);
                    if (strCombo1 == strClass)
                    {
                        cboGbCity.Text = cboGbCity.Items[i].ToString();
                        break;
                    }
                }

                txtMailCode.Text = VB.Left(dt.Rows[0]["MiaJuso"].ToString().Trim(), 3) + VB.Mid(dt.Rows[0]["MiaJuso"].ToString().Trim(), 4, 3);
                txtMiaJuso1.Text = VB.Mid(dt.Rows[0]["MiaJuso"].ToString().Trim(), 7, 13);
                txtMiaJuso2.Text = VB.Mid(dt.Rows[0]["MiaJuso"].ToString().Trim(), 21, 40);

                txtKiho.Text = dt.Rows[0]["Kiho"].ToString().Trim();

                chkGbCha.Checked = dt.Rows[0]["GBCHA"].ToString().Trim() == "*" ? true : false;
                chkWon.Checked = dt.Rows[0]["GBWON"].ToString().Trim() == "*" ? true : false;

                txtEDITA.Text = dt.Rows[0]["EDITA"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void Screen_Clear()
        {
            txtMiaCode.Text = ""; dtpDeldate.Text = "";
            cboMiaClass.Text = ""; cboDetailClass.Text = "";
            txtMiaName.Text = ""; txtTelName.Text = "";
            cboGbCity.Text = ""; txtMailCode.Text = "";
            txtMiaJuso.Text = ""; cboDetailClass.Text = ""; cboDetailClass1.Text = "";
            txtMiaJuso1.Text = ""; ss1_Sheet1.Rows.Count = 0;
            txtMiaJuso2.Text = ""; txtKiho.Text = "";
            txtRemark.Text = ""; txtEDITA.Text = "";

            btnSave.Enabled = false; btnCancel.Enabled = false;
            btnView.Enabled = true;
            btnPrint.Enabled = false; btnShow.Enabled = false;
            txtKiho.Enabled = true;

            chkGbCha.Checked = false;
            chkWon.Checked = false;
        }

        bool SaveData()
        {
            string strMiaJuso = "";
            string strDelDate = "";
            string strGbcha = "";
            string strWon = "";

            if (cboDetailClass.Text == "")
            {
                MessageBox.Show(" 상세분류항목 입력 후 등록하세요 오류!! ", "확인");
                cboDetailClass.BackColor = Color.FromArgb(255, 200, 200);
                cboDetailClass.Focus();
                return false;
            }

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                strGbcha = chkGbCha.Checked == true ? "*" : "";
                strWon = chkWon.Checked == true ? "*" : "";

                SQL = "SELECT MiaCode  FROM BAS_MIA";
                SQL = SQL + ComNum.VBLF + " WHERE MiaCode = '" + txtMiaCode.Text.Trim() + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                strDelDate = dtpDeldate.Text;
                strMiaJuso = VB.Left(txtMailCode.Text.Trim(), 3) + VB.Right(txtMailCode.Text.Trim(), 3);
                strMiaJuso += (txtMiaJuso1.Text + VB.Space(20)).Substring(0, 20);
                strMiaJuso += (txtMiaJuso2.Text + VB.Space(40)).Substring(0, 40);

                if (dt.Rows.Count == 0)
                {
                    if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인
                    //Insert
                    SQL = "INSERT INTO BAS_MIA";
                    SQL = SQL + ComNum.VBLF + " (MiaCode, DelDate, MiaClass, MiaDetail, MiaName,";
                    SQL = SQL + ComNum.VBLF + " MiaTel, GbCity, MiaJuso, KIHO, miaremark, Gbcha, EDITA, GBWon )";
                    SQL = SQL + ComNum.VBLF + " VALUES(";
                    SQL = SQL + ComNum.VBLF + " '" + txtMiaCode.Text.Trim() + "',";                 //조합코드
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strDelDate.Trim() + "','YYYY-MM-DD'),"; //삭제일자
                    SQL = SQL + ComNum.VBLF + " '" + cboMiaClass.Text.Trim().Substring(0, 2) + "',";     //조합구분
                    SQL = SQL + ComNum.VBLF + " '" + cboDetailClass.Text.Trim().Substring(0, 2) + "',";  //상세분류
                    SQL = SQL + ComNum.VBLF + " '" + VB.Trim(txtMiaName.Text) + "',";                 //조합명
                    SQL = SQL + ComNum.VBLF + " '" + VB.Trim(txtTelName.Text) + "',";                 //전화번호
                    SQL = SQL + ComNum.VBLF + " '" + VB.Left(VB.Trim(cboGbCity.Text) + VB.Space(1), 1) + "',";       //시.군지역 구분
                    SQL = SQL + ComNum.VBLF + " '" + VB.RTrim(strMiaJuso) + "', '" + VB.Trim(txtKiho.Text) + "',";  //우편번호및 주소
                    SQL = SQL + ComNum.VBLF + " '" + VB.Trim(txtRemark.Text) + "' , '" + strGbcha + "' , '" + VB.Trim(txtEDITA.Text) + "', '" + strWon + "' )";
                }
                else
                {
                    if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return false; //권한 확인
                    SQL = "UPDATE   BAS_MIA SET";
                    SQL = SQL + ComNum.VBLF + " MiaCode = '" + VB.Trim(txtMiaCode.Text) + "',";         //조합코드
                    SQL = SQL + ComNum.VBLF + " DelDate = TO_DATE('" + VB.Trim(dtpDeldate.Text) + "','YYYY-MM-DD'),"; //삭제일자
                    SQL = SQL + ComNum.VBLF + " MiaClass = '" + VB.Left(VB.Trim(cboMiaClass.Text), 2) + "',";    //조합구분
                    SQL = SQL + ComNum.VBLF + " MiaDetail = '" + VB.Left(VB.Trim(cboDetailClass.Text), 2) + "',"; //상세분류
                    SQL = SQL + ComNum.VBLF + " MiaName = '" + VB.Trim(txtMiaName.Text) + "',";  //조합명
                    SQL = SQL + ComNum.VBLF + " MiaTel = '" + VB.Trim(txtTelName.Text) + "',";  //전화번호
                    SQL = SQL + ComNum.VBLF + " GbCity = '" + VB.Left(VB.Trim(cboGbCity.Text), 1) + "',";      //시.군지역 구분
                    SQL = SQL + ComNum.VBLF + " MiaJuso = '" + VB.RTrim(strMiaJuso) + "', ";   //우편번호 주소
                    SQL = SQL + ComNum.VBLF + " KIHO = '" + VB.Trim(txtKiho.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + " MIAREMARK = '" + VB.Trim(txtRemark.Text) + "' ,";
                    SQL = SQL + ComNum.VBLF + " GBCHA = '" + strGbcha + "' , ";
                    SQL = SQL + ComNum.VBLF + " EDITA  = '" + VB.Trim(txtEDITA.Text) + "',   ";
                    SQL = SQL + ComNum.VBLF + " GBWON = '" + strWon + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE  MiaCode = '" + txtMiaCode.Text.Trim() + "'";
                }

                dt.Dispose();
                dt = null;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Screen_Clear();
                txtMiaCode.Focus();
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        bool ViewData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인

            string strClass1 = "";
            string strChoice = "";

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            if (optReferance0.Checked == true)
            {
                strChoice = "11";
            }
            else if (optReferance1.Checked == true)
            {
                strChoice = "12";
            }
            else if (optReferance2.Checked == true)
            {
                strChoice = "13";
            }
            else if (optReferance3.Checked == true)
            {
                strChoice = "20";
            }
            else
            {
                strChoice = "90";
            }

            try
            {
                ss1_Sheet1.Rows.Count = 0;

              //  strClass1 = cboDetailClass1.Text.Substring(0, 2);

                SQL = "SELECT MiaCode,MiaName,MiaDetail,MiaTel,MiaJuso,MiaClass,ROWID, Kiho, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(DELDATE,'YYYY-MM-DD') DELDATE";
                SQL = SQL + ComNum.VBLF + " FROM BAS_MIA ";
                SQL = SQL + ComNum.VBLF + "WHERE MiaClass = '" + strChoice + "' ";

                if (optSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY MiaCode ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY MiaName ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return false;
                }

                ss1_Sheet1.RowCount = dt.Rows.Count;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["MiaCode"].ToString();
                    ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["MiaName"].ToString();
                    ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["MiaDetail"].ToString();
                    ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DELDATE"].ToString();
                    ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Miajuso"].ToString();
                    ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["KIHO"].ToString();
                    ss1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ROWID"].ToString();
                }

                btnPrint.Enabled = true;
                btnShow.Enabled = true;
                btnView.Enabled = true;

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        #endregion

        #region 이벤트

        /// <summary>
        /// 컨트롤들의 CR 시 Tab 이벤트는 디자이너에
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{Tab}");
            }
        }

        /// <summary>
        /// txtMiaCode에 값 반환 시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMiaCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtMiaCode.Text = txtMiaCode.Text.Trim().ToUpper();
                if (txtMiaCode.Text == "") { txtMiaCode.Focus(); return; }

                Display();
                SendKeys.Send("{Tab}");
            }
        }

        void setEvent()
        {
            //로드 이벤트
            Load += (sender, e) =>
            {
                if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                //콤보박스 아이템 셋팅
                string[] GbCityItems =
                {
                "1.시,구지역",
                "0.기타"
                 };

                string[] MiaClassItems =
                {
                "11.공단",
                "12.공단",
                "13.지역",
                "20.보호",
                "90.거래처"
                };

                string[] DetailClassItems =
                {
                "01.각지구구조합", "02.대기업조합", "11.초등학교", "12.중학교", "13.고등학교",
                "14.전문대, 대학","15.특수학교", "21.군.공무원", "31.경찰.소방", "32.시,군,구청",
                "33.법무공무원", "34.교육공무원", "35.전화우체국", "36.철도,원호,세무", "41.기타",
                "71.서울",  "72.부산", "73.대구", "74.인천", "75.광주",
                "76.대전","77.울산", "81.경기", "82.강원", "83.충북",
                "84.충남", "85.경북", "86.경남", "87.전북", "88.전남",
                "89.제주", "90.세종특별자치시","92.보험", "99.계약처"
                };

                foreach (string item in MiaClassItems)
                {
                    cboMiaClass.Items.Add(item);
                }

                foreach (string item in DetailClassItems)
                {
                    cboDetailClass.Items.Add(item);
                    cboDetailClass1.Items.Add(item);
                }

                foreach (string item in GbCityItems)
                {
                    cboGbCity.Items.Add(item);
                }
                Screen_Clear();
            };

            cboDetailClass.Leave += (sender, e) =>
            {
                if (cboDetailClass.Text.Substring(0, 2) == "99")
                {
                    txtKiho.Enabled = true;
                }
                else
                {
                    txtKiho.Enabled = false;
                }
            };
            txtMiaCode.Leave += (sender, e) =>
            {
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
            };
            ss1.CellDoubleClick += (sender, e) =>
            {
                txtMiaCode.Text = ss1_Sheet1.Cells[e.Row, 0].Text.Trim();
                txtMiaCode_KeyPress(txtMiaCode, new KeyPressEventArgs((char)Keys.Enter));
            };

            #region Click 이벤트

            cboDetailClass1.Click += (sender, e) => { btnView.Enabled = true; };
            btnView.Click += (sender, e) => { if (ViewData() == false) return; };
            btnSave.Click += (sender, e) => { if (SaveData() == false) return; };
            btnCancel.Click += (sender, e) => { Screen_Clear(); txtMiaCode.Focus(); };          
            btnExit.Click += (sender, e) => { this.Close(); };
            btnPrint.Click += (sender, e) => 
            {
                clsSpread SPR = new clsSpread();
                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                strTitle = "조 합 기 호  코 드 집";

                strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

                strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                SPR.setSpdPrint(ss1, PrePrint, setMargin, setOption, strHeader, strFooter);
            };
            btnGam.Click += (sender, e) => 
            {
                if (clsType.User.Sabun != "4349" && clsType.User.Sabun != "19684" && clsType.User.Sabun != "20175" && clsType.User.Sabun != "38358" && clsType.User.Sabun != "45432" && clsType.User.Sabun != "45442" && clsType.User.Sabun != "11701"  )
                {
                    MessageBox.Show("작업권한이 있는 사번만 작업가능합니다.", "확인");
                    return;
                }
                frmContractorDecreaseCriterion frm = new frmContractorDecreaseCriterion();
                frm.Show();
            };
            btnMia.Click += (sender, e) => 
            {
                GstrHelpCode = "MIA";

                if (frmCodehelpX != null)
                {
                    frmCodehelpX.Dispose();
                    frmCodehelpX = null;
                }

                frmCodehelpX = new FrmCodehelp(GstrHelpCode);
                frmCodehelpX.rSetHelpName += (strHelpCode) => 
                {
                    if (strHelpCode != "")
                    {
                        txtMiaCode.Text = strHelpCode;
                        txtMiaCode_KeyPress(txtMiaCode, new KeyPressEventArgs((char)Keys.Enter));
                    }
                };
                frmCodehelpX.rEventClosed += () => 
                {
                    if (frmCodehelpX != null)
                    {
                        frmCodehelpX.Dispose();
                        frmCodehelpX = null;
                    }
                };
                frmCodehelpX.Show();
            };
            btnMiaHelp.Click += (sender, e) =>
            {
                //중복 폼 방지
                if (frmMailX != null)
                {
                    frmMailX.Dispose();
                    frmMailX = null;
                }

                frmMailX = new frmMail();
                //자식폼 과의 데이터 연결 
                frmMailX.SendEvent += (string rtnValue) =>
                {
                    txtMailCode.Text = VB.Left(rtnValue, 3) + "-" + VB.Mid(rtnValue, 4, 3);
                    txtMiaJuso.Text = VB.Mid(rtnValue, 9, 50);
                };
                frmMailX.rEventClosed += () =>
                {
                    if (frmMailX != null)
                    {
                        frmMailX.Dispose();
                        frmMailX = null;
                    }
                };
                frmMailX.Show();
            };
            btnShow.Click += (sender, e) =>
            {
                ss1_Sheet1.Rows.Count = 0;
                Screen_Clear();
                txtMiaCode.Focus();
            };

            #endregion
        }

        #endregion

        //생성자
        public frmBasBukMia()
        {
            InitializeComponent();

            //이벤트 
            setEvent();
        }

        private void frmBasBukMia_Load (object sender , EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등
        }

        private void dtpDeldate_TextChanged(object sender, EventArgs e)
        {

        }
        private void Calendar_Date_Select(Control ArgText)
        {
            clsOrdFunction OF = null;

            clsPublic.GstrCalDate = VB.Trim(ArgText.Text);
            if (VB.Len(VB.Trim(ArgText.Text)) != 10)
            {
                clsPublic.GstrCalDate = clsPublic.GstrSysDate;
            }

            frmCalendar frmCalendarX = new frmCalendar();
            frmCalendarX.ShowDialog();
          //  OF.fn_ClearMemory(frmCalendarX);

            if (VB.Len(clsPublic.GstrCalDate) == 10)
            {
                ArgText.Text = clsPublic.GstrCalDate;
            }
        }

        private void dtpDeldate_DoubleClick(object sender, EventArgs e)
        {
            if (sender == this.dtpDeldate)
                Calendar_Date_Select(dtpDeldate);
        }
    }
}
