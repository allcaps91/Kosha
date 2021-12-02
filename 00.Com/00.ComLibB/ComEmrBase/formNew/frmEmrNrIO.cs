using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;

namespace ComEmrBase
{
    //frmEmrNF00300004
    public partial class frmEmrNrIO : Form, EmrChartForm
    {
        ContextMenu PopupMenu = null;
        int mCol = -1;

        #region // 폼에 사용하는 변수를 코딩하는 부분
        private const string mDirection = "H";   //기록지 작성방향(H: 옆으로, V:아래로)
        private const int mintTCol = 3;  //해드 칼럼수
        private const int mintTRow = 4;  //해드 로수
        private const int mintBRow = 3;  //밑줄
        private const int mintColW_I = 90;  //밑줄
        private const int mintColW_V = 60;  //밑줄
        public const string mstrFormNameGb = "섭취배설항목";
        public const string mstrFormNameWard = "섭취배설병동";
        private const string mJOBGB = "IO";
        #endregion

        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        public FormEmrMessage mEmrCallForm;

        public string mstrFormNo = "";
        public string mstrUpdateNo = "0";
        public string mstrFormText = "";
        public EmrPatient p = null;
        public string mstrEmrNo = "0";
        public string mstrMode = "W";
        private mtsPanel15.TransparentPanel panEditLock = null;

        #endregion

        #region // TopMenu관련 선언
        private usFormTopMenu usFormTopMenuEvent;
        private usTimeSet usTimeSetEvent;
        public ComboBox mMaskBox = null;
        #endregion

        #region // 상용구 관련 모듈
        private Control mControl = null;
        private frmEmrMacrowordProg frmMacrowordProgEvent;

        private Control mCalControl = null; //달력 띄우기
        private frmEmrCaledar frmEmrCaledarEvent;

        private FarPoint.Win.Spread.FpSpread ssMacroWord;
        private FarPoint.Win.Spread.SheetView ssMacroWord_Sheet1;

        //스프래드를 생성한다.
        private void pAddMacroSpd()
        {
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color333635194368298125000", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text397635194368298125000", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();

            ssMacroWord = new FarPoint.Win.Spread.FpSpread();
            ssMacroWord_Sheet1 = new FarPoint.Win.Spread.SheetView();

            // ssMacroWord
            // 
            this.ssMacroWord.AccessibleDescription = "";
            this.ssMacroWord.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssMacroWord.Location = new System.Drawing.Point(3, 6);
            this.ssMacroWord.Name = "ssMacroWord";
            namedStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            textCellType1.MaxLength = 32000;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            this.ssMacroWord.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2});
            this.ssMacroWord.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssMacroWord_Sheet1});
            this.ssMacroWord.Size = new System.Drawing.Size(62, 20);
            this.ssMacroWord.TabIndex = 105;
            this.ssMacroWord.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssMacroWord.TextTipAppearance = tipAppearance1;
            this.ssMacroWord.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssMacroWord.Visible = false;
            this.ssMacroWord.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssMacroWord_CellDoubleClick);
            this.ssMacroWord.Leave += new System.EventHandler(this.ssMacroWord_Leave);
            // 
            // ssMacroWord_Sheet1
            // 
            this.ssMacroWord_Sheet1.Reset();
            this.ssMacroWord_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssMacroWord_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            ssMacroWord_Sheet1.ColumnCount = 2;
            ssMacroWord_Sheet1.RowCount = 10;
            this.ssMacroWord_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMacroWord_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMacroWord_Sheet1.ColumnHeader.Visible = false;
            this.ssMacroWord_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMacroWord_Sheet1.DefaultStyleName = "Text397635194368298125000";
            this.ssMacroWord_Sheet1.GrayAreaBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssMacroWord_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssMacroWord_Sheet1.RowHeader.Visible = false;
            this.ssMacroWord_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;

            Controls.Add(this.ssMacroWord);
        }

        private void pAddEventToText(Control objParent)
        {
            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control control in controls)
            {
                if (control is TextBox)
                {
                    string strTag = "";
                    if (((TextBox)control).Tag != null)
                    {
                        strTag = ((TextBox)control).Tag.ToString();
                        if (strTag == "DATE")
                        {
                            ((TextBox)control).KeyDown += new System.Windows.Forms.KeyEventHandler(pCalTextBox_KeyDown);
                            ((TextBox)control).Click += new System.EventHandler(pCalTextBox_Click);
                            ((TextBox)control).DoubleClick += new System.EventHandler(pCalTextBox_DoubleClick);
                        }
                        else
                        {
                            ((TextBox)control).KeyDown += new System.Windows.Forms.KeyEventHandler(pTextBox_KeyDown);
                            ((TextBox)control).Click += new System.EventHandler(pTextBox_Click);
                            ((TextBox)control).DoubleClick += new System.EventHandler(pTextBox_DoubleClick);
                        }
                    }
                    else
                    {
                        ((TextBox)control).KeyDown += new System.Windows.Forms.KeyEventHandler(pTextBox_KeyDown);
                        ((TextBox)control).Click += new System.EventHandler(pTextBox_Click);
                        ((TextBox)control).DoubleClick += new System.EventHandler(pTextBox_DoubleClick);
                    }
                }
            }
        }
        //=================
        private void pCalTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            mCalControl = (TextBox)sender;
            ssMacroWord.Visible = false;
        }
        private void pCalTextBox_Click(object sender, EventArgs e)
        {
            mCalControl = (TextBox)sender;
            ssMacroWord.Visible = false;
        }
        private void pCalTextBox_DoubleClick(object sender, EventArgs e)
        {
            mCalControl = (TextBox)sender;
            ssMacroWord.Visible = false;

            if (frmEmrCaledarEvent == null)
            {
                frmEmrCaledarEvent = new frmEmrCaledar();
                frmEmrCaledarEvent.rSetClalendaInfo += new frmEmrCaledar.SetClalendaInfo(frmEmrCaledarEvent_SetClalendaInfo);
                frmEmrCaledarEvent.rEventClosed += new frmEmrCaledar.EventClosed(frmEmrCaledarEvent_EventClosed);
            }
            frmEmrCaledarEvent.ShowDialog();
        }

        private void frmEmrCaledarEvent_SetClalendaInfo(string strDate)
        {
            frmEmrCaledarEvent.Close();
            frmEmrCaledarEvent = null;

            if (strDate.Trim() == "")
            {
                return;
            }
            mCalControl.Text = strDate;
        }

        private void frmEmrCaledarEvent_EventClosed()
        {
            frmEmrCaledarEvent.Close();
            frmEmrCaledarEvent = null;
        }
        //=================

        private void pTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            mControl = (TextBox)sender;
            ssMacroWord.Visible = false;
        }

        private void pTextBox_Click(object sender, EventArgs e)
        {
            if (((TextBox)sender).Focused == false)
            {
                return;
            }
            mControl = (TextBox)sender;
            ssMacroWord.Visible = false;

            clsEmrFunc.DspControl(clsDB.DbCon, this, panChart, mstrFormNo, ssMacroWord, mControl);
        }

        private void pTextBox_DoubleClick(object sender, EventArgs e)
        {
            mControl = (TextBox)sender;
            ssMacroWord.Visible = false;

            pLoadMacro(mControl);
            return;
        }

        private void pLoadMacro(Control mControl)
        {
            string strConIndex = "";
            strConIndex = clsXML.IsArryCon(mControl);

            if (frmMacrowordProgEvent == null)
            {
                frmMacrowordProgEvent = new frmEmrMacrowordProg(mControl.Name, strConIndex, mstrFormNo, "200", mstrFormText);
                frmMacrowordProgEvent.rSetMacro += new frmEmrMacrowordProg.SetMacro(frmMacrowordProgEvent_SetMacro);
                frmMacrowordProgEvent.rEventClosed += new frmEmrMacrowordProg.EventClosed(frmMacrowordProgEvent_EventClosed);
            }
            frmMacrowordProgEvent.ShowDialog();
        }

        private void frmMacrowordProgEvent_SetMacro(string strCtlName, string strMacrono, string strMacro, string strCtlNameIdx)
        {
            string strConIndex = "";
            strConIndex = clsXML.IsArryCon(mControl);

            frmMacrowordProgEvent.Close();
            frmMacrowordProgEvent = null;

            //strMacro = VB.Replace(strMacro, "\n", "\r\n", 1, -1, CompareMethod.Text);

            if (clsEmrPublic.gstrMcrAddFlag == "1")
            {
                mControl.Text = mControl.Text + " " + strMacro;
                ((TextBox)mControl).Select(((TextBox)mControl).Text.Length, 0);
                ((TextBox)mControl).Focus();
            }
            else
            {
                mControl.Text = strMacro;
                ((TextBox)mControl).Select(((TextBox)mControl).Text.Length, 0);
                ((TextBox)mControl).Focus();
            }
        }

        private void frmMacrowordProgEvent_EventClosed()
        {
            frmMacrowordProgEvent.Close();
            frmMacrowordProgEvent = null;
        }

        private void ssMacroWord_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ssMacroWord.Visible = false;
            string strMacro = ssMacroWord_Sheet1.Cells[e.Row, 1].Text;
            strMacro = VB.Replace(VB.Replace(strMacro, "<pre>", ""), "</pre>", "");
            //strMacro = VB.Replace(strMacro, "\r", "\r\n", 1, -1, CompareMethod.Text);

            if (clsEmrPublic.gstrMcrAddFlag == "1")
            {
                mControl.Text = mControl.Text + " " + strMacro;
                ((TextBox)mControl).Select(((TextBox)mControl).Text.Length, 0);
                ((TextBox)mControl).Focus();
            }
            else
            {
                mControl.Text = strMacro;
                ((TextBox)mControl).Select(((TextBox)mControl).Text.Length, 0);
                ((TextBox)mControl).Focus();
            }
        }

        private void ssMacroWord_Leave(object sender, EventArgs e)
        {
            ssMacroWord.Visible = false;
        }

        #endregion

        #region // TopMenu관련 이벤트 처리 함수

        private void usFormTopMenuEvent_SetTimeCheckShow(ComboBox mkText)
        {
            mMaskBox = mkText;
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
            usTimeSetEvent = new usTimeSet();
            usTimeSetEvent.rSetTime += new usTimeSet.SetTime(usTimeSetEvent_SetTime);
            usTimeSetEvent.rEventClosed += new usTimeSet.EventClosed(usTimeSetEvent_EventClosed);
            this.Controls.Add(usTimeSetEvent);
            usTimeSetEvent.Top = mMaskBox.Top - 5;
            usTimeSetEvent.Left = mMaskBox.Left;
            usTimeSetEvent.BringToFront();
        }

        private void usTimeSetEvent_SetTime(string strText)
        {
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
            usFormTopMenuEvent.txtMedFrTime.Text = strText;
        }

        private void usTimeSetEvent_EventClosed()
        {
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
        }

        private void usFormTopMenuEvent_SetSave(string strFrDate, string strFrTime)
        {
            pSaveData("1"); //인증저장
        }
        private void usFormTopMenuEvent_SetSaveTemp(string strFrDate, string strFrTime)
        {
            pSaveData("0"); //임시저장
        }

        private void usFormTopMenuEvent_SetDel(string strFrDate, string strFrTime)
        {
            pDelData();
        }
        private void usFormTopMenuEvent_SetClear()
        {
            mstrEmrNo = "0";
            pClearForm();
        }
        private void usFormTopMenuEvent_SetPrint()
        {
            pPrintForm();
        }
        private void usFormTopMenuEvent_EventClosed()
        {
            //아무것도 하지 않는다.
        }

        #endregion

        #region // 폼에 사용하는 공통 이벤트

        /// <summary>
        /// 폼이 Close 될때 필요한 것들
        /// </summary>
        private void pUnloadForm()
        {
            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control control in controls)
            {
                if (control is PictureBox)
                {
                    //임시폴더를 삭제한다.
                    string mstrFoldJob = "";
                    string mstrFoldBase = "";

                    clsEmrFunc.CheckImageJobFold(ref mstrFoldJob, ref mstrFoldBase, mstrFormNo, mstrUpdateNo, mstrEmrNo, ((PictureBox)control).Name);
                    clsEmrFunc.DeleteImageJobFold(mstrFoldJob, mstrFoldBase);
                }
            }
        }

        /// <summary>
        /// 이미지 이벤트
        /// </summary>
        /// <param name="objParent"></param>
        private void pAddEventImage(Control objParent)
        {
            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control control in controls)
            {
                if (control is PictureBox)
                {
                    ((PictureBox)control).DoubleClick += new System.EventHandler(PictureBox_DoubleClick);
                }
            }
        }

        /// <summary>
        /// 체크박스, 레디오 버튼
        /// </summary>
        /// <param name="objParent"></param>
        private void pAddEventCheckAndRdio(Control objParent)
        {
            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control control in controls)
            {
                if (control is RadioButton)
                {
                    ((RadioButton)control).CheckedChanged += new System.EventHandler(RadioButton_CheckedChanged);
                }
                if (control is CheckBox)
                {
                    ((CheckBox)control).CheckedChanged += new System.EventHandler(CheckBox_CheckedChanged);
                }
            }
        }

        /// <summary>
        /// 버튼 이벤트
        /// </summary>
        /// <param name="objParent"></param>
        private void pAddEventButton(Control objParent)
        {
            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control control in controls)
            {
                if (control is Button)
                {
                    ((Button)control).Click += new System.EventHandler(Button_Click);
                }
            }
        }

        private void PictureBox_DoubleClick(object sender, EventArgs e)
        {
            string strTag = "";

            if (((PictureBox)sender).Tag != null)
            {
                strTag = ((PictureBox)sender).Tag.ToString();
            }

            clsEmrFunc.SetImageEvent(this, (PictureBox)sender, strTag, mstrFormNo, mstrUpdateNo, mstrMode, mstrEmrNo, mEmrCallForm);
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Tag == null)
            {
                return;
            }

            string strTag = ((RadioButton)sender).Tag.ToString();

            if (strTag.Trim() == "") return;

            if (((RadioButton)sender).Checked == true)
            {
                clsEmrFunc.SetControlEvent(clsDB.DbCon, this, (RadioButton)sender, strTag, p, usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd"));
            }
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Tag == null)
            {
                return;
            }

            string strTag = ((CheckBox)sender).Tag.ToString();

            if (strTag.Trim() == "") return;

            clsEmrFunc.SetControlEvent(clsDB.DbCon, this, (CheckBox)sender, strTag, p, usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd"));
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Tag == null)
            {
                return;
            }

            string strTag = ((Button)sender).Tag.ToString();

            if (strTag.Trim() == "") return;

            clsEmrFunc.SetControlEvent(clsDB.DbCon, this, (Button)sender, strTag, p, usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd"));
        }

        private void pSetEmrInfo()
        {
            //clsEmrFunc.usBtnHide(usFormTopMenuEvent);

            ////권한에 따라서 버튼을 세팅을 한다. 
            //clsEmrFunc.usBtnShowReg(usFormTopMenuEvent);

            //if (clsType.gEmrUseInfo.AuAWRITE == "1")
            //{
            //    SetEditLock(this, panChart, true);
            //}
            //else
            //{
            //    SetEditLock(this, panChart, false);
            //}
            //EMRNO가 있으면 기록 정보를 세팅을 한다.
            pLoadEmrChartInfo();
        }

        /// <summary>
        /// 기록지 수정 가능여부
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="panChartX"></param>
        /// <param name="EditYn"></param>
        public void SetEditLock(Control frm, Panel panChartX, bool EditYn)
        {
            if (EditYn == false)
            {
                if (panEditLock == null)
                {
                    panEditLock = new mtsPanel15.TransparentPanel();
                    frm.Controls.Add(panEditLock);
                }
                panEditLock.Top = panChartX.Top;
                panEditLock.Left = panChartX.Left;
                if (panChartX.Width < 700)
                {
                    panEditLock.Width = panChartX.Width;
                }
                else
                {
                    panEditLock.Width = 680;
                }
                panEditLock.Height = CalcChartPanelHeight(panChartX); //panChart가 부모인 자식 panel의 높이를 계산한다.
                panEditLock.BringToFront();
            }
            else
            {
                if (panEditLock != null)
                {
                    panEditLock.Dispose();
                    panEditLock = null;
                }
            }
        }

        private int CalcChartPanelHeight(Panel panChartX)
        {
            int rtnVal = 0;

            Control[] controls = null;
            controls = ComFunc.GetAllControls(panChartX);

            foreach (Control objControl in controls)
            {
                if (objControl is Panel)
                {
                    if (((Panel)objControl).Visible == true)
                    {
                        if (objControl.Parent == panChartX)
                        {
                            rtnVal = rtnVal + ((Panel)objControl).Height;
                        }
                    }
                }
            }
            return rtnVal;
        }

        #endregion

        #region //외부에서 이벤트 받아서 처리 클리어, 저장, 삭제, 프린터
        public void SetChartHisMsg(string strEmrNo, string strOldGb)
        {
            return;
        }

        public double SaveDataMsg(string strFlag)
        {
            double dblEmrNo = pSaveData(strFlag);
            return dblEmrNo;
        }

        public bool DelDataMsg()
        {
            return pDelData();
        }

        public void ClearFormMsg()
        {
            mstrEmrNo = "0";
            pClearForm();
        }

        public int PrintFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;
            if (strPRINTFLAG == "N")
            {
                using (frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption())
                {
                    frmEmrPrintOptionX.StartPosition = FormStartPosition.CenterParent;
                    frmEmrPrintOptionX.ShowDialog();

                }
            }

            if (clsFormPrint.mstrPRINTFLAG == "-1")
            {
                return rtnVal;
            }

            if (clsEmrQuery.SaveEmrXmlPrnYnForm(clsDB.DbCon, mstrEmrNo, "0") == false)
            {
                return rtnVal;
            }

            rtnVal = clsFormPrint.PrintFormLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            return rtnVal;
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            int rtnVal = clsFormPrint.PrintToTifFileLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            return rtnVal;
        }
        /// <summary>
        /// 환자 받아서 기록지를 초기화 한다.
        /// </summary>
        public void gPatientinfoRecive(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;

            //폼을 클리어하고 기록지 작성 정보등을 갱신한다.
            pClearForm();
            //기록지 정보를 세팅한다.
            pSetEmrInfo();
        }

        /// <summary>
        /// 폼이 로드할때 초기 세팅을 한다
        /// </summary>
        public void pInitForm()
        {
            //이미지 작성
            pAddEventImage(this);
            //텍스트 박스에 상용구 이벤트를 세팅한다
            pAddEventToText(this);
            //Radio 패널 열기 닫기
            pAddEventCheckAndRdio(this);
            //Button에 이벤트 달기
            pAddEventButton(this);

            //스프래드를 생성한다.
            pAddMacroSpd();

            //----TopMenu관련 이벤트 생성 및 선언
            usFormTopMenuEvent = new usFormTopMenu();
            //usBtnShow(usFormTopMenuEvent, "mbtnSave");
            usFormTopMenuEvent.rSetTimeCheckShow += new usFormTopMenu.SetTimeCheckShow(usFormTopMenuEvent_SetTimeCheckShow);
            usFormTopMenuEvent.rSetSave += new usFormTopMenu.SetSave(usFormTopMenuEvent_SetSave);
            usFormTopMenuEvent.rSetSaveTemp += new usFormTopMenu.SetSaveTemp(usFormTopMenuEvent_SetSaveTemp);
            usFormTopMenuEvent.rSetDel += new usFormTopMenu.SetDel(usFormTopMenuEvent_SetDel);
            usFormTopMenuEvent.rSetClear += new usFormTopMenu.SetClear(usFormTopMenuEvent_SetClear);
            usFormTopMenuEvent.rSetPrint += new usFormTopMenu.SetPrint(usFormTopMenuEvent_SetPrint);
            usFormTopMenuEvent.rEventClosed += new usFormTopMenu.EventClosed(usFormTopMenuEvent_EventClosed);

            this.Controls.Add(usFormTopMenuEvent);
            usFormTopMenuEvent.Parent = this.panTopMenu;
            usFormTopMenuEvent.Dock = DockStyle.Fill;
            //--------------------------
            pClearForm();
            pSetEmrInfo();

            clsEmrChart.SetChartHead(clsDB.DbCon, usFormTopMenuEvent, p);
        }

        /// <summary>
        /// 폼별 특수한 초기화세팅이 필요할 경우 코딩.
        /// </summary>
        public void pInitFormSpc()
        {

        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        public double pSaveData(string strFlag)
        {
            //if (VB.Val(mstrEmrNo) != 0)
            //{
            //    if (clsEmrQuery.IsChangeAuth(mstrEmrNo, clsType.User.IdNumber) == false) return 0;
            //}

            double dblEmrNo = 0;
            //if (VB.Val(mstrEmrNo) != 0)
            //{
            //    if (MessageBox.Show(new Form() { TopMost = true }, "기존 내용을 변경하시겠습니까?", "EMR", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            //    {
            //        return dblEmrNo;
            //    }
            //}

            //dblEmrNo = pSaveEmrData(strFlag);
            //if (dblEmrNo == 0)
            //{
            //    MessageBox.Show(new Form() { TopMost = true }, "저장중 에러가 발생했습니다.");
            //}
            //else
            //{
            //    MessageBox.Show(new Form() { TopMost = true }, "저장하였습니다.");
            //    mstrEmrNo = Convert.ToString(dblEmrNo);
            //    pSetEmrInfo();
            //    mEmrCallForm.MsgSave(strFlag);
            //}

            //돌면서 저장을 한다.

            if (SaveTimeSet() == false)
            {
                ComFunc.MsgBoxEx(this, "시간 저장중 에러가 발생했습니다.");
                return dblEmrNo;
            }

            int i = 0;
            int j = 0;

            for (i = 3; i < ssChart_Sheet1.Columns.Count; i++)
            {

                string[] arryEMRNO = null;
                string[] arryITEMCD = null;
                string[] arryITEMNO = null;
                string[] arryITEMINDEX = null;
                string[] arryITEMTYPE = null;
                string[] arryITEMVALUE = null;
                string[] arryITEMVALUE1 = null;
                string[] arryDSPSEQ = null;

                if (ssChart_Sheet1.Cells[ssChart_Sheet1.RowCount - 1, i].Text == "Y")
                {
                    string strEmrNo = Convert.ToString(VB.Val(ssChart_Sheet1.Cells[ssChart_Sheet1.Rows.Count - 4, i].Text.Trim()));
                    string strChartDate = dtpFrDate.Value.ToString("yyyyMMdd");// ssChart_Sheet1.Cells[0, i].Text.Trim().Replace("-", "");
                    string strChartTime = ssChart_Sheet1.Cells[2, i].Text.Trim().Replace(":", "") + "00";
                    string strCHARTUSEID = "";
                    string strCOMPUSEID = "";
                    string strSAVEGB = "0";
                    string strFORMGB = "0";

                    if (VB.Val(strEmrNo) > 0)
                    {
                        strCHARTUSEID = ssChart_Sheet1.Cells[ssChart_Sheet1.Rows.Count - 3, i].Text.Trim();
                        strCOMPUSEID = ssChart_Sheet1.Cells[ssChart_Sheet1.Rows.Count - 3, i].Text.Trim();
                    }
                    else
                    {
                        strCHARTUSEID = clsType.User.IdNumber;
                        strCOMPUSEID = clsType.User.IdNumber;
                    }

                    //합계를 구한다.
                    SumIntakeOutPutRow(i);

                    for (j = 4; j < ssChart_Sheet1.RowCount - 4; j++)
                    {
                        string strITEMCD = ssChart_Sheet1.Cells[j, 0].Text.Trim();
                        string[] strItem = VB.Split(ssChart_Sheet1.Cells[j, 0].Text.Trim(), "_");
                        string strITEMNO = "";
                        string strITEMINDEX = "-1";
                        if (strItem.Length > 0)
                        {
                            strITEMNO = strItem[0].Trim();
                        }
                        if (strItem.Length > 1)
                        {
                            strITEMINDEX = strItem[1].Trim();
                        }
                        string strDSPSEQ = "0";
                        string strITEMTYPE = "TEXT";
                        string strITEMVALUE = ssChart_Sheet1.Cells[j, i].Text.Trim();
                        string strITEMVALUE1 = "";

                        if (arryEMRNO == null)
                        {
                            arryEMRNO = new string[0];
                            arryITEMCD = new string[0];
                            arryITEMNO = new string[0];
                            arryITEMINDEX = new string[0];
                            arryITEMTYPE = new string[0];
                            arryITEMVALUE = new string[0];
                            arryITEMVALUE1 = new string[0];
                            arryDSPSEQ = new string[0];
                        }

                        Array.Resize<string>(ref arryEMRNO, arryEMRNO.Length + 1);
                        Array.Resize<string>(ref arryITEMCD, arryITEMCD.Length + 1);
                        Array.Resize<string>(ref arryITEMNO, arryITEMNO.Length + 1);
                        Array.Resize<string>(ref arryITEMINDEX, arryITEMINDEX.Length + 1);
                        Array.Resize<string>(ref arryITEMTYPE, arryITEMTYPE.Length + 1);
                        Array.Resize<string>(ref arryITEMVALUE, arryITEMVALUE.Length + 1);
                        Array.Resize<string>(ref arryITEMVALUE1, arryITEMVALUE1.Length + 1);
                        Array.Resize<string>(ref arryDSPSEQ, arryDSPSEQ.Length + 1);

                        arryEMRNO[arryEMRNO.Length - 1] = "0";
                        arryITEMCD[arryEMRNO.Length - 1] = strITEMCD;
                        arryITEMNO[arryEMRNO.Length - 1] = strITEMNO;
                        arryITEMINDEX[arryEMRNO.Length - 1] = strITEMINDEX;
                        arryITEMTYPE[arryEMRNO.Length - 1] = strITEMTYPE;
                        arryITEMVALUE[arryEMRNO.Length - 1] = strITEMVALUE;
                        arryITEMVALUE1[arryEMRNO.Length - 1] = strITEMVALUE1;
                        arryDSPSEQ[arryEMRNO.Length - 1] = strDSPSEQ;
                    }

                    string strSAVECERT = "1";

                    if (clsEmrQuery.SaveFlowChart(clsDB.DbCon, p, mstrFormNo, mstrUpdateNo, strEmrNo, strChartDate, strChartTime, 
                        strCHARTUSEID, strCOMPUSEID, strSAVEGB, strSAVECERT, strFORMGB,
                        arryITEMCD, arryITEMNO, arryITEMINDEX, arryITEMTYPE, arryITEMVALUE, arryDSPSEQ, arryITEMVALUE1
                        ) == 0)
                    {
                        //에러임
                    }
                }
            }

            return dblEmrNo;
        }

        private void SumIntakeOutPutRow(int Col)
        {
            int j = 0;
            int intIntakeRow = -1;
            int intOutPutRow = -1;
            int intIntakeSum = 0;
            int intOutPutSum = 0;

            string strSql = "";
            DataTable dt = null;
            //Intake
            //OutPut
            for (j = 4; j < ssChart_Sheet1.RowCount - 4; j++)
            {
                string strITEMCD = ssChart_Sheet1.Cells[j, 0].Text.Trim();

                if ((strITEMCD == "I0000030622") || (strITEMCD == "I0000030623"))
                {
                    if (strITEMCD == "I0000030622")
                    {
                        intIntakeRow = j;
                    }
                    else if (strITEMCD == "I0000030623")
                    {
                        intOutPutRow = j;
                    }
                }
                else
                {
                    strSql = "";
                    strSql = strSql + "SELECT VFLAG1";
                    strSql = strSql + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD ";
                    strSql = strSql + ComNum.VBLF + "WHERE BSNSCLS = '기록지관리'";
                    strSql = strSql + ComNum.VBLF + "    AND UNITCLS = '섭취배설항목'";
                    strSql = strSql + ComNum.VBLF + "    AND BASCD = '" + strITEMCD + "'";


                    string sqlErr = clsDB.GetDataTable(ref dt, strSql, clsDB.DbCon);
                    if (sqlErr.Length > 0)
                    {
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["VFLAG1"].ToString().Trim() == "01.섭취")
                        {
                            intIntakeSum = intIntakeSum + Convert.ToInt32(VB.Val(ssChart_Sheet1.Cells[j, Col].Text.Trim()));
                        }
                        else if (dt.Rows[0]["VFLAG1"].ToString().Trim() == "11.배설")
                        {
                            intOutPutSum = intOutPutSum + Convert.ToInt32(VB.Val(ssChart_Sheet1.Cells[j, Col].Text.Trim()));
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }
            }

            if (intIntakeRow >= 0)
            {
                ssChart_Sheet1.Cells[intIntakeRow, Col].Text = intIntakeSum.ToString();
            }

            if (intOutPutRow >= 0)
            {
                ssChart_Sheet1.Cells[intOutPutRow, Col].Text = intOutPutSum.ToString();
            }

        }

        private void SumIntakeOutPutRowTot(int TotCol)
        {
            int Col = 0;
            int Row = 0;
            int intSum = 0;

            string strSql = "";
            DataTable dt = null;

            for (Row = 4; Row < ssChart_Sheet1.RowCount - 4; Row++)
            {
                string strITEMCD = ssChart_Sheet1.Cells[Row, 0].Text.Trim();
                intSum = 0;

                strSql = "";
                strSql = strSql + "SELECT VFLAG1";
                strSql = strSql + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD ";
                strSql = strSql + ComNum.VBLF + "WHERE BSNSCLS = '기록지관리'";
                strSql = strSql + ComNum.VBLF + "    AND UNITCLS = '섭취배설항목'";
                strSql = strSql + ComNum.VBLF + "    AND BASCD = '" + strITEMCD + "'";
                strSql = strSql + ComNum.VBLF + "    AND VFLAG1 IN ('01.섭취', '02.섭취', '11.배설', '12.배설', '09.섭취', '19.배설')";

                string sqlErr = clsDB.GetDataTable(ref dt, strSql, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    //for (Col = 3; Col < TotCol; Col++)
                    //{
                    //    if (ssChart_Sheet1.Cells[3, Col].Text.Trim() != "합계")
                    //    {
                    //        intSum = intSum + Convert.ToInt32(VB.Val(ssChart_Sheet1.Cells[Row, Col].Text.Trim()));
                    //    }
                    //}
                    for (Col = TotCol - 1; Col >= 3; Col--)
                    {
                        if (ssChart_Sheet1.Cells[3, Col].Text.Trim() == "합계")
                        {
                            break;
                        }
                        else
                        {
                            intSum = intSum + Convert.ToInt32(VB.Val(ssChart_Sheet1.Cells[Row, Col].Text.Trim()));
                        }
                    }
                    ssChart_Sheet1.Cells[Row, TotCol].Text = intSum.ToString();
                }
                dt.Dispose();
                dt = null;
            }

        }

        private bool SaveTimeSet()
        {
            bool rtnVal = false;
            int i = 0;
            int RowAffected = 0;
            string strSql = string.Empty;
            string sqlErr = string.Empty;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

                strSql = "";
                strSql = "DELETE ";
                strSql = strSql + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALTIME";
                strSql = strSql + ComNum.VBLF + "WHERE ACPNO = " + p.acpNo;
                strSql = strSql + ComNum.VBLF + "    AND JOBGB = '" + mJOBGB + "'";
                strSql = strSql + ComNum.VBLF + "    AND CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";

                sqlErr = clsDB.ExecuteNonQuery(strSql, ref RowAffected, clsDB.DbCon);
                if(sqlErr.Length > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return rtnVal;
                }

                for (i = 3; i < ssChart_Sheet1.Columns.Count; i++)
                {
                    strSql = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALTIME ";
                    strSql = strSql + ComNum.VBLF + "(ACPNO, JOBGB , CHARTDATE, TIMEVALUE, SUBGB, WRITEDATE, WRITETIME, WRITEUSEID)";
                    strSql = strSql + ComNum.VBLF + "VALUES (";
                    strSql = strSql + ComNum.VBLF + "" + p.acpNo + ",";
                    strSql = strSql + ComNum.VBLF + "'" + mJOBGB + "',";
                    strSql = strSql + ComNum.VBLF + "'" + dtpFrDate.Value.ToString("yyyyMMdd") + "',";
                    strSql = strSql + ComNum.VBLF + "'" + ssChart_Sheet1.Cells[2, i].Text.Trim().Replace(":", "") + "',";
                    if (ssChart_Sheet1.Cells[3, i].Text.Trim() == "합계")
                    {
                        strSql = strSql + ComNum.VBLF + "'1',";
                    }
                    else
                    {
                        strSql = strSql + ComNum.VBLF + "'0',";
                    }
                    strSql = strSql + ComNum.VBLF + "'" + VB.Left(strCurDateTime, 8) + "',";
                    strSql = strSql + ComNum.VBLF + "'" + VB.Right(strCurDateTime, 6) + "',";
                    strSql = strSql + ComNum.VBLF + "'" + clsType.User.IdNumber + "'";
                    strSql = strSql + ComNum.VBLF + ")";

                    sqlErr = clsDB.ExecuteNonQuery(strSql, ref RowAffected, clsDB.DbCon);
                    if (sqlErr.Length > 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, sqlErr);
                        return rtnVal;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                //MessageBox.Show(new Form() { TopMost = true }, "저장 하였습니다.");
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                return rtnVal;
            }
        }

        #endregion

        #region // 기록지 클리어, 저장, 삭제, 프린터

        /// <summary>
        /// 화면 정리
        /// </summary>
        public void pClearForm()
        {
            ////모든 컨트롤을 초기화 한다.
            //ComFunc.SetAllControlClearEx(this);
            ////시간 세팅을 한다.
            //string strCurDateTime = clsComQuery.CurrentDateTime("A");
            //usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D"));
            //usFormTopMenuEvent.txtMedFrTime.Text = ComFunc.FormatStrToDate(VB.Mid(strCurDateTime, 9, 4), "M");
            //usFormTopMenuEvent.dtMedFrDate.Enabled = true;
            //usFormTopMenuEvent.txtMedFrTime.Enabled = true;
            //if (clsType.gEmrUseInfo.AuAWRITE == "1")
            //{
            //    SetEditLock(this, panChart, true);
            //}
            //else
            //{
            //    SetEditLock(this, panChart, false);
            //}


            pClearFormExcept();
        }

        /// <summary>
        /// 폼별로 EMR 작성 내역을 화면에 보여준다.
        /// </summary>
        private void pLoadEmrChartInfo()
        {
            LoadEmrChartInfoOne();
            LoadEmrChartInfoTot();
            LoadTotal();
        }

        private void LoadTotal()
        {
            txtNightPreIn.Text = "";
            txtTotalPreIn.Text = "";
            txtDayIn.Text = "";
            txtEveIn.Text = "";
            txtNightIn.Text = "";
            txtTotalIn.Text = "";

            txtNightPreOut.Text = "";
            txtTotalPreOut.Text = "";
            txtDayOut.Text = "";
            txtEveOut.Text = "";
            txtNightOut.Text = "";
            txtTotalOut.Text = "";

            int i = 0;
            string strSql = "";
            DataTable dt = null;

            string strPreDate = dtpFrDate.Value.AddDays(-1).ToString("yyyyMMdd");
            string strToDate = dtpFrDate.Value.ToString("yyyyMMdd");
            string strNextDate = dtpFrDate.Value.AddDays(+1).ToString("yyyyMMdd");

            strSql = "";
            strSql = strSql + ComNum.VBLF + " SELECT A.EMRNO, A.CHARTDATE, A.CHARTTIME,  ";
            strSql = strSql + ComNum.VBLF + "                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE ";
            strSql = strSql + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A  ";
            strSql = strSql + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B ";
            strSql = strSql + ComNum.VBLF + "    ON  B.EMRNO = A.EMRNO ";
            strSql = strSql + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BC  ";
            strSql = strSql + ComNum.VBLF + "    ON BC.BASCD = B.ITEMCD  ";
            strSql = strSql + ComNum.VBLF + "   AND BC.BSNSCLS = '기록지관리'  ";
            strSql = strSql + ComNum.VBLF + "    AND BC.UNITCLS = '섭취배설항목' ";
            strSql = strSql + ComNum.VBLF + "    AND BC.BASCD IN ('I0000030622', 'I0000030623')  ";
            strSql = strSql + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
            strSql = strSql + ComNum.VBLF + "  AND A.FORMNO = " + mstrFormNo;
            strSql = strSql + ComNum.VBLF + "  AND A.CHARTDATE >= '" + dtpFrDate.Value.AddDays(-1).ToString("yyyyMMdd") + "' ";
            strSql = strSql + ComNum.VBLF + "  AND A.CHARTDATE <= '" + dtpFrDate.Value.AddDays(+1).ToString("yyyyMMdd") + "' ";
            strSql = strSql + ComNum.VBLF + "  AND A.CHARTUSEID <> '합계' ";
            strSql = strSql + ComNum.VBLF + "ORDER BY A.CHARTDATE ASC , A.CHARTTIME ASC ";

            string sqlErr = clsDB.GetDataTable(ref dt, strSql, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["CHARTDATE"].ToString().Trim() == strPreDate)
                {
                    //전일 Total
                    if (VB.Val(dt.Rows[i]["CHARTTIME"].ToString().Trim()) > 60000)
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030622")
                        {
                            txtTotalPreIn.Text = (VB.Val(txtTotalPreIn.Text) + VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())).ToString();
                        }
                        else if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030623")
                        {
                            txtTotalPreOut.Text = (VB.Val(txtTotalPreOut.Text) + VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())).ToString();
                        }
                    }

                    //전일 Night
                    if (VB.Val(dt.Rows[i]["CHARTTIME"].ToString().Trim()) > 220000)
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030622")
                        {
                            txtNightPreIn.Text = (VB.Val(txtNightPreIn.Text) + VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())).ToString();
                        }
                        else if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030623")
                        {
                            txtNightPreOut.Text = (VB.Val(txtNightPreOut.Text) + VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())).ToString();
                        }
                    }
                }

                if (dt.Rows[i]["CHARTDATE"].ToString().Trim() == strToDate)
                {
                    if (VB.Val(dt.Rows[i]["CHARTTIME"].ToString().Trim()) <= 60000)
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030622")
                        {
                            txtNightPreIn.Text = (VB.Val(txtNightPreIn.Text) + VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())).ToString();
                        }
                        else if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030623")
                        {
                            txtNightPreOut.Text = (VB.Val(txtNightPreOut.Text) + VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())).ToString();
                        }
                    }
                    else if ((VB.Val(dt.Rows[i]["CHARTTIME"].ToString().Trim()) > 60000) && (VB.Val(dt.Rows[i]["CHARTTIME"].ToString().Trim()) <= 140000))
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030622")
                        {
                            txtDayIn.Text = (VB.Val(txtDayIn.Text) + VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())).ToString();
                        }
                        else if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030623")
                        {
                            txtDayOut.Text = (VB.Val(txtDayOut.Text) + VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())).ToString();
                        }
                    }
                    else if ((VB.Val(dt.Rows[i]["CHARTTIME"].ToString().Trim()) > 140000) && (VB.Val(dt.Rows[i]["CHARTTIME"].ToString().Trim()) <= 220000))
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030622")
                        {
                            txtEveIn.Text = (VB.Val(txtEveIn.Text) + VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())).ToString();
                        }
                        else if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030623")
                        {
                            txtEveOut.Text = (VB.Val(txtEveOut.Text) + VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())).ToString();
                        }
                    }
                    else if (VB.Val(dt.Rows[i]["CHARTTIME"].ToString().Trim()) > 220000)
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030622")
                        {
                            txtNightIn.Text = (VB.Val(txtNightIn.Text) + VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())).ToString();
                        }
                        else if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030623")
                        {
                            txtNightOut.Text = (VB.Val(txtNightOut.Text) + VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())).ToString();
                        }
                    }
                }


                if (dt.Rows[i]["CHARTDATE"].ToString().Trim() == strNextDate)
                {
                    if (VB.Val(dt.Rows[i]["CHARTTIME"].ToString().Trim()) < 60000)
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030622")
                        {
                            txtNightIn.Text = (VB.Val(txtNightIn.Text) + VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())).ToString();
                        }
                        else if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030623")
                        {
                            txtNightOut.Text = (VB.Val(txtNightOut.Text) + VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())).ToString();
                        }
                    }
                }
            }

            //txtTotalPreIn.Text = (VB.Val(txtNightPreIn.Text)).ToString();
            //txtTotalPreOut.Text = (VB.Val(txtNightPreOut.Text)).ToString();

            txtTotalIn.Text = (VB.Val(txtDayIn.Text) + VB.Val(txtEveIn.Text) + VB.Val(txtNightIn.Text)).ToString();
            txtTotalOut.Text = (VB.Val(txtDayOut.Text) + VB.Val(txtEveOut.Text) + VB.Val(txtNightOut.Text)).ToString();

        }

        private void LoadEmrChartInfoOne()
        {
            string strSql = "";
            DataTable dt = null;

            strSql = "";
            strSql = strSql + ComNum.VBLF + " SELECT A.EMRNO, A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID, A.PRNTYN, ";
            strSql = strSql + ComNum.VBLF + "                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE,";
            strSql = strSql + ComNum.VBLF + "                U.USENAME";
            strSql = strSql + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
            strSql = strSql + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
            strSql = strSql + ComNum.VBLF + "    ON  B.EMRNO = A.EMRNO";
            strSql = strSql + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BC ";
            strSql = strSql + ComNum.VBLF + "    ON BC.BASCD = B.ITEMCD ";
            strSql = strSql + ComNum.VBLF + "   AND BC.BSNSCLS = '기록지관리' ";
            strSql = strSql + ComNum.VBLF + "    AND BC.UNITCLS = '" + mstrFormNameGb + "' ";
            strSql = strSql + ComNum.VBLF + "LEFT OUTER JOIN  " + ComNum.DB_EMR + "AVIEWEMRUSER U";
            strSql = strSql + ComNum.VBLF + "    ON  U.USEID = A.CHARTUSEID";
            if (p.inOutCls == "I")
            {
                strSql = strSql + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
            }
            else
            {
                strSql = strSql + ComNum.VBLF + "WHERE A.PTNO = '" + p.ptNo + "'";
            }
            strSql = strSql + ComNum.VBLF + "  AND A.FORMNO = " + mstrFormNo;
            strSql = strSql + ComNum.VBLF + "  AND A.UPDATENO = " + mstrUpdateNo;
            strSql = strSql + ComNum.VBLF + "  AND A.CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
            strSql = strSql + ComNum.VBLF + "  AND A.CHARTUSEID <> '합계'";
            strSql = strSql + ComNum.VBLF + "ORDER BY A.CHARTDATE ASC , A.CHARTTIME ASC , A.EMRNO, BC.BASVAL";

            Cursor.Current = Cursors.WaitCursor;

            string sqlErr = clsDB.GetDataTable(ref dt, strSql, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            int i = 0;
            int j = 0;
            int k = 0;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                for (j = 3; j < ssChart_Sheet1.Columns.Count; j++)
                {
                    if (ssChart_Sheet1.Cells[3, j].Text.Trim() == "합계")
                    {
                        continue;
                    }
                    if (ssChart_Sheet1.Cells[2, j].Text.Trim() == ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"))
                    {
                        ssChart_Sheet1.Cells[0, j].Text = ComFunc.FormatStrToDate(dt.Rows[i]["CHARTDATE"].ToString().Trim(), "D");
                        ssChart_Sheet1.Cells[1, j].Text = clsEmrFunc.DutyGet(ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"));
                        ssChart_Sheet1.Cells[2, j].Text = ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M");
                        if (dt.Rows[i]["CHARTUSEID"].ToString().Trim() == "합계")
                        {
                            ssChart_Sheet1.Cells[3, j].Text = dt.Rows[i]["CHARTUSEID"].ToString().Trim();
                        }
                        else
                        {
                            ssChart_Sheet1.Cells[3, j].Text = dt.Rows[i]["USENAME"].ToString().Trim();
                        }


                        ssChart_Sheet1.Cells[ssChart_Sheet1.Rows.Count - 4, j].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                        ssChart_Sheet1.Cells[ssChart_Sheet1.Rows.Count - 3, j].Text = dt.Rows[i]["CHARTUSEID"].ToString().Trim();
                        ssChart_Sheet1.Cells[ssChart_Sheet1.Rows.Count - 2, j].Text = dt.Rows[i]["PRNTYN"].ToString().Trim();
                        if (dt.Rows[i]["CHARTUSEID"].ToString().Trim() == "합계")
                        {
                            ssChart_Sheet1.Cells[4, j, ssChart_Sheet1.Rows.Count - 5, j].Locked = true;
                        }
                        else
                        {
                            if (dt.Rows[i]["CHARTUSEID"].ToString().Trim() == clsType.User.IdNumber)
                            {
                                ssChart_Sheet1.Cells[4, j, ssChart_Sheet1.Rows.Count - 5, j].Locked = false;
                            }
                            else
                            {
                                ssChart_Sheet1.Cells[4, j, ssChart_Sheet1.Rows.Count - 5, j].Locked = true;
                            }
                        }

                        for (k = 4; k < ssChart_Sheet1.RowCount - 4; k++)
                        {
                            if (dt.Rows[i]["ITEMCD"].ToString().Trim() == ssChart_Sheet1.Cells[k, 0].Text.Trim())
                            {
                                ssChart_Sheet1.Cells[k, j].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                                //i = i + 1;
                                //if (i >= dt.Rows.Count) break;
                            }
                        }
                    }
                    if (i >= dt.Rows.Count) break;
                }
                if (i >= dt.Rows.Count) break;
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void LoadEmrChartInfoTot()
        {
            string strSql = "";
            DataTable dt = null;

            strSql = strSql + ComNum.VBLF + " SELECT A.EMRNO, A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID, A.PRNTYN, ";
            strSql = strSql + ComNum.VBLF + "                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE,";
            strSql = strSql + ComNum.VBLF + "                U.USENAME";
            strSql = strSql + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
            strSql = strSql + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
            strSql = strSql + ComNum.VBLF + "    ON  B.EMRNO = A.EMRNO";
            strSql = strSql + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BC ";
            strSql = strSql + ComNum.VBLF + "    ON BC.BASCD = B.ITEMCD ";
            strSql = strSql + ComNum.VBLF + "   AND BC.BSNSCLS = '기록지관리' ";
            strSql = strSql + ComNum.VBLF + "    AND BC.UNITCLS = '" + mstrFormNameGb + "' ";
            strSql = strSql + ComNum.VBLF + "LEFT OUTER JOIN  " + ComNum.DB_EMR + "AVIEWEMRUSER U";
            strSql = strSql + ComNum.VBLF + "    ON  U.USEID = A.CHARTUSEID";
            if (p.inOutCls == "I")
            {
                strSql = strSql + ComNum.VBLF + "WHERE A.ACPNO = '" + p.acpNo + "'";
            }
            else
            {
                strSql = strSql + ComNum.VBLF + "WHERE A.PTNO = '" + p.ptNo + "'";
            }
            strSql = strSql + ComNum.VBLF + "  AND A.FORMNO = " + mstrFormNo;
            strSql = strSql + ComNum.VBLF + "  AND A.UPDATENO = " + mstrUpdateNo;
            strSql = strSql + ComNum.VBLF + "  AND A.CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
            strSql = strSql + ComNum.VBLF + "  AND A.CHARTUSEID = '합계'";
            strSql = strSql + ComNum.VBLF + "ORDER BY A.CHARTDATE ASC , A.CHARTTIME ASC , A.EMRNO, BC.BASVAL";

            Cursor.Current = Cursors.WaitCursor;

            string sqlErr = clsDB.GetDataTable(ref dt, strSql, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            int i = 0;
            int j = 0;
            int k = 0;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                for (j = 3; j < ssChart_Sheet1.Columns.Count; j++)
                {
                    if (ssChart_Sheet1.Cells[3, j].Text.Trim() != "합계")
                    {
                        continue;
                    }

                    if (ssChart_Sheet1.Cells[2, j].Text.Trim() == ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"))
                    {
                        ssChart_Sheet1.Cells[0, j].Text = ComFunc.FormatStrToDate(dt.Rows[i]["CHARTDATE"].ToString().Trim(), "D");
                        ssChart_Sheet1.Cells[1, j].Text = clsEmrFunc.DutyGet(ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"));
                        ssChart_Sheet1.Cells[2, j].Text = ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M");
                        if (dt.Rows[i]["CHARTUSEID"].ToString().Trim() == "합계")
                        {
                            ssChart_Sheet1.Cells[3, j].Text = dt.Rows[i]["CHARTUSEID"].ToString().Trim();
                        }
                        else
                        {
                            ssChart_Sheet1.Cells[3, j].Text = dt.Rows[i]["USENAME"].ToString().Trim();
                        }


                        ssChart_Sheet1.Cells[ssChart_Sheet1.Rows.Count - 4, j].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                        ssChart_Sheet1.Cells[ssChart_Sheet1.Rows.Count - 3, j].Text = dt.Rows[i]["CHARTUSEID"].ToString().Trim();
                        ssChart_Sheet1.Cells[ssChart_Sheet1.Rows.Count - 2, j].Text = dt.Rows[i]["PRNTYN"].ToString().Trim();
                        if (dt.Rows[i]["CHARTUSEID"].ToString().Trim() == "합계")
                        {
                            ssChart_Sheet1.Cells[4, j, ssChart_Sheet1.Rows.Count - 5, j].Locked = true;
                        }
                        else
                        {
                            if (dt.Rows[i]["CHARTUSEID"].ToString().Trim() == clsType.User.IdNumber)
                            {
                                ssChart_Sheet1.Cells[4, j, ssChart_Sheet1.Rows.Count - 5, j].Locked = false;
                            }
                            else
                            {
                                ssChart_Sheet1.Cells[4, j, ssChart_Sheet1.Rows.Count - 5, j].Locked = true;
                            }
                        }

                        for (k = 4; k < ssChart_Sheet1.RowCount - 4; k++)
                        {
                            if (dt.Rows[i]["ITEMCD"].ToString().Trim() == ssChart_Sheet1.Cells[k, 0].Text.Trim())
                            {
                                ssChart_Sheet1.Cells[k, j].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                                //i = i + 1;
                                //if (i >= dt.Rows.Count) break;
                            }
                        }
                    }
                    if (i >= dt.Rows.Count) break;
                }
                if (i >= dt.Rows.Count) break;
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        private double pSaveEmrData(string strFlag)
        {
            double dblEmrNo = 0;
            //bool blnCert = false;
            //string strChartDate = VB.Format(usFormTopMenuEvent.dtMedFrDate.Value, "yyyyMMdd");
            //string strChartTime = VB.Replace(usFormTopMenuEvent.txtMedFrTime.Text, ":", "", 1);

            //dblEmrNo = clsEmrQuery.SaveChartMst(p, this, false, this,
            //                                                    mstrFormNo, mstrUpdateNo, mstrEmrNo, strChartDate, strChartTime,
            //                                                    clsType.User.IdNumber, clsType.User.IdNumber, strFlag, "0");

            //if (dblEmrNo != 0)
            //{
            //    if (strFlag == "1")
            //    {
            //        //전자인증
            //        if (clsType.gHosInfo.strEmrCertUseYn == "1")
            //        {
            //            blnCert = clsEmrQuery.SaveDataAEMRCHARTCERTY(this, false, this, dblEmrNo, null);
            //            if (blnCert == false)
            //            {
            //                MessageBox.Show(new Form() { TopMost = true }, "인증중 에러가 발생했습니다." + ComNum.VBLF + "추후 인증을 실시해 주시기 바랍니다.");
            //            }
            //        }
            //    }
            //}
            return dblEmrNo;
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <returns></returns>
        public bool pDelData()
        {
            if (VB.Val(mstrEmrNo) == 0)
            {
                return false;
            }

            if (VB.Val(mstrEmrNo) != 0)
            {
                if (clsEmrQuery.IsChangeAuth(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == false) return false;
            }

            if (clsXML.gDeleteEmrXml(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == true)
            {
                mstrEmrNo = "0";
                pClearForm();
                mEmrCallForm.MsgDelete();
            }
            return true;
        }

        public bool pDelData(string strUseId)
        {
            if (VB.Val(mstrEmrNo) == 0)
            {
                return false;
            }

            if (VB.Val(mstrEmrNo) != 0)
            {
                if (clsEmrQuery.IsChangeAuthCopy(clsDB.DbCon, mstrEmrNo, strUseId) == false) return false;
            }

            if (clsXML.gDeleteEmrXml(clsDB.DbCon, mstrEmrNo, strUseId) == true)
            {
                mstrEmrNo = "0";
                pClearForm();
                mEmrCallForm.MsgDelete();
            }
            return true;
        }

        /// <summary>
        /// 기록지를 출력한다.
        /// </summary>
        public void pPrintForm()
        {
            using (frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption())
            {
                frmEmrPrintOptionX.StartPosition = FormStartPosition.CenterParent;
                frmEmrPrintOptionX.ShowDialog();
            }

            if (clsFormPrint.mstrPRINTFLAG == "-1")
            {
                return;
            }

            if (clsEmrQuery.SaveEmrXmlPrnYnForm(clsDB.DbCon, mstrEmrNo, "0") == false)
            {
                return;
            }

            clsFormPrint.PrintFormLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
        }
        #endregion


        //==========================================================================================//
        //=============================== 아래부터 코딩을 하면 됨 ===================================//
        //=========================================================================================//


        private void mbtnUpdate_Click(object sender, EventArgs e)
        {
            using (frmVitalSet frmVitalSetX = new frmVitalSet(p, mstrFormNameGb, mstrFormNo, mstrUpdateNo))
            {
                frmVitalSetX.StartPosition = FormStartPosition.CenterParent;
                frmVitalSetX.ShowDialog();
            }

            pClearFormExcept();
            pLoadEmrChartInfo();
        }

        public frmEmrNrIO()
        {
            InitializeComponent();
        }

        public frmEmrNrIO(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
        }

        private void frmEmrNrIO_Load(object sender, EventArgs e)
        {
            //pInitForm();
            //SetUserAut();
            //QueryChartList();
            //pClearFormExcept();
            ssView_Sheet1.RowCount = 0;

            if (VB.Val(mstrEmrNo) != 0)
            {
                clsEmrFunc.SetMedFrEndDate(clsDB.DbCon, mstrEmrNo, p, dtpFrDate, null);
            }
            else
            {
                dtpFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            }
        }

        private void ClearTot()
        {
            txtNightPreIn.Text = "";
            txtTotalPreIn.Text = "";
            txtDayIn.Text = "";
            txtEveIn.Text = "";
            txtNightIn.Text = "";
            txtTotalIn.Text = "";

            txtNightPreOut.Text = "";
            txtTotalPreOut.Text = "";
            txtDayOut.Text = "";
            txtEveOut.Text = "";
            txtNightOut.Text = "";
            txtTotalOut.Text = "";
        }

        /// <summary>
        /// 클리어하고 폼별로 별요한것 기본 세팅
        /// </summary>
        private void pClearFormExcept()
        {
            //일자별 등록된 것이 있는지 파악해서 있으면 세팅을 하고
            //없으면 기본을 가지고 세팅을 한다.
            int i = 0;
            string strSql = string.Empty;
            DataTable dt = null;

            ClearTot();
            SetTopRow();

            string strBASEXNAME = "";
            int intS = 0;

            Cursor.Current = Cursors.WaitCursor;

            strSql = "SELECT B.BASCD, B.BASNAME, B.BASEXNAME, B.BASVAL, A.CHARTDATE ";
            strSql = strSql + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
            strSql = strSql + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
            strSql = strSql + ComNum.VBLF + "    ON B.BASCD = A.ITEMCD";
            strSql = strSql + ComNum.VBLF + "    AND B.BSNSCLS = '기록지관리'";
            strSql = strSql + ComNum.VBLF + "    AND B.UNITCLS = '" + mstrFormNameGb + "'";
            strSql = strSql + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B1";
            strSql = strSql + ComNum.VBLF + "    ON B1.BASCD = B.BASEXNAME";
            strSql = strSql + ComNum.VBLF + "    AND B1.BSNSCLS = '기록지관리'";
            strSql = strSql + ComNum.VBLF + "    AND B1.UNITCLS = '섭취배설그룹'";
            strSql = strSql + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
            strSql = strSql + ComNum.VBLF + "    AND A.JOBGB = '" + mJOBGB + "'";
            //strSql = strSql + ComNum.VBLF + "    AND A.CHARTDATE <= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
            strSql = strSql + ComNum.VBLF + "    AND A.CHARTDATE = (SELECT MAX(CHARTDATE) AS CHARTDATE ";
            strSql = strSql + ComNum.VBLF + "                                        FROM " + ComNum.DB_EMR + "AEMRBVITALSET ";
            strSql = strSql + ComNum.VBLF + "                                        WHERE ACPNO = " + p.acpNo;
            strSql = strSql + ComNum.VBLF + "                                            AND JOBGB = '" + mJOBGB + "'";
            strSql = strSql + ComNum.VBLF + "                                            AND CHARTDATE <= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "')";
            strSql = strSql + ComNum.VBLF + "ORDER BY B1.BASVAL, B.BASVAL";

            string sqlErr = clsDB.GetDataTable(ref dt, strSql, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssChart_Sheet1.RowCount = ssChart_Sheet1.RowCount + 1;
                    ssChart_Sheet1.SetRowHeight(ssChart_Sheet1.RowCount - 1, ComNum.SPDROWHT);
                    clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                    clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", true);
                    clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 1, 2, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);

                    ssChart_Sheet1.Cells[ssChart_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                    if (strBASEXNAME != dt.Rows[i]["BASEXNAME"].ToString().Trim())
                    {
                        ssChart_Sheet1.Cells[ssChart_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                        if (i != 0)
                        {
                            ssChart_Sheet1.AddSpanCell(intS, 1, ssChart_Sheet1.RowCount - 1 - intS, 1);
                            ssChart_Sheet1.SetRowHeight(intS, Convert.ToInt32(ssChart_Sheet1.GetPreferredRowHeight(intS)) + 10);
                        }
                        intS = ssChart_Sheet1.RowCount - 1;
                    }
                    strBASEXNAME = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                    ssChart_Sheet1.Cells[ssChart_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                    if (ssChart_Sheet1.ColumnCount > i)
                    {
                        ssChart_Sheet1.SetColumnWidth(2, Convert.ToInt32(ssChart_Sheet1.GetPreferredColumnWidth(i)) + 4);
                    }
                }
                dt.Dispose();
                dt = null;
                ssChart_Sheet1.AddSpanCell(intS, 1, ssChart_Sheet1.RowCount - intS, 1);
                ssChart_Sheet1.SetRowHeight(intS, Convert.ToInt32(ssChart_Sheet1.GetPreferredRowHeight(intS)) + 10);

                SetButtonRow();
                SetTimeSet();
                return;
            }

            dt.Dispose();
            dt = null;

            strSql = "";
            strSql = "SELECT A.BASVAL, A.BASCD, A.BASNAME, A.BASEXNAME ";
            strSql = strSql + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD A";
            strSql = strSql + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B1";
            strSql = strSql + ComNum.VBLF + "    ON B1.BASCD = A.BASEXNAME";
            strSql = strSql + ComNum.VBLF + "    AND B1.BSNSCLS = '기록지관리'";
            strSql = strSql + ComNum.VBLF + "    AND B1.UNITCLS = '섭취배설그룹'";
            strSql = strSql + ComNum.VBLF + "WHERE A.BSNSCLS = '기록지관리'";
            strSql = strSql + ComNum.VBLF + "    AND A.UNITCLS = '" + mstrFormNameWard + "'";
            strSql = strSql + ComNum.VBLF + "ORDER BY B1.BASVAL, A.BASVAL";

            sqlErr = clsDB.GetDataTable(ref dt, strSql, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssChart_Sheet1.RowCount = ssChart_Sheet1.RowCount + 1;
                ssChart_Sheet1.SetRowHeight(ssChart_Sheet1.RowCount - 1, ComNum.SPDROWHT);
                clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", true);
                clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 1, 2, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                ssChart_Sheet1.Cells[ssChart_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                if (strBASEXNAME != dt.Rows[i]["BASEXNAME"].ToString().Trim())
                {
                    ssChart_Sheet1.Cells[ssChart_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                    if (i != 0)
                    {
                        ssChart_Sheet1.AddSpanCell(intS, 1, ssChart_Sheet1.RowCount - 1 - intS, 1);
                        ssChart_Sheet1.SetRowHeight(intS, Convert.ToInt32(ssChart_Sheet1.GetPreferredRowHeight(intS)) + 10);
                    }
                    intS = ssChart_Sheet1.RowCount - 1;
                }
                strBASEXNAME = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                ssChart_Sheet1.Cells[ssChart_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                if (ssChart_Sheet1.ColumnCount > i)
                {
                    ssChart_Sheet1.SetColumnWidth(2, Convert.ToInt32(ssChart_Sheet1.GetPreferredColumnWidth(i)) + 4);
                }
            }
            dt.Dispose();
            dt = null;
            ssChart_Sheet1.AddSpanCell(intS, 1, ssChart_Sheet1.RowCount - intS, 1);
            ssChart_Sheet1.SetRowHeight(intS, Convert.ToInt32(ssChart_Sheet1.GetPreferredRowHeight(intS)) + 10);
            Cursor.Current = Cursors.Default;

            SetButtonRow();
            SetTimeSet();
            GetData();
        }

        /// <summary>
        /// TODO: 포항성모 처방 연동 필요
        /// </summary>
        private void GetData()
        {
            ssView_Sheet1.RowCount = 0;

            if (p == null) return;
            if (VB.Val(p.acpNo) == 0) return;
            return;

            string strSql = "";
            string strODate = "";
            string strQueryDept = "";
            int i = 0;
            DataTable dt = null;

            //if (intRow < 0)
            //{
            //    ComFunc.MsgBoxEx(this, " 전 처방이 선택 되지 않았습니다..");
            //    return;
            //}
            Cursor.Current = Cursors.WaitCursor;

            strODate = dtpFrDate.Value.ToString("yyyy-MM-dd");
            strQueryDept = p.medDeptCd;
            //INPUT_STATUS : 0: 정규, 1: 추가, 2,3:DC
            //ORDER_KEY : PK
            strSql = " SELECT DISTINCT  ";
            strSql = strSql + ComNum.VBLF + "    A.ORDER_KEY,  A.INPUT_STATUS, ";
            strSql = strSql + ComNum.VBLF + "    TO_CHAR(A.ORDER_DATE,'YYYY-MM-DD') AS ORDER_DATE,  A.CLINICAL_DEPT, A.ITEM_CODE, A.ITEM_NAME, A.QTY_OF_ORDER, A.DOSAGE_OF_ORDER, A.DOSAGE_CODE, A.FREQUENCY_OF_ORDER, A.VALUE_OF_FREQUENCY,  A.DURATION_OF_ORDER, A.SEQ_NO ";
            strSql = strSql + ComNum.VBLF + "    , B.CLASS_OF_ORDER, B.S_CODE, B.DELETE_FLAG, B.SHAPE, B.ATC,  B.CHK_OUT  ";
            strSql = strSql + ComNum.VBLF + "    , C.S_DRUGCODE  , C.S_SUGBB , C.S_SUGBBO , C.S_SUGBS ,C.S_SUGBT , C.S_DELDATE, C.S_SUDATE ,C.I_DRUG100  DRUG100,  C.OCS_REMARK    ";
            strSql = strSql + ComNum.VBLF + "    , D.CHKFLAG  ";
            strSql = strSql + ComNum.VBLF + "    , U.DR_NAME  ";
            strSql = strSql + ComNum.VBLF + " FROM IPD_ORDER_DMC A ";
            strSql = strSql + ComNum.VBLF + " LEFT OUTER JOIN OPDIPD_MEDICINES_DMC B ";
            strSql = strSql + ComNum.VBLF + "    ON A.ITEM_CODE = B.ITEM_CODE ";
            strSql = strSql + ComNum.VBLF + "    AND B.CLASS_OF_ORDER NOT IN ('7','88','99')";
            strSql = strSql + ComNum.VBLF + " LEFT OUTER JOIN    MED_PMPA.BAS_SUMAST_DMC C ";
            strSql = strSql + ComNum.VBLF + "    ON B.S_CODE    = C.S_CODE ";
            strSql = strSql + ComNum.VBLF + " LEFT OUTER JOIN OPDIPD_PASSWORD_DMC U ";
            strSql = strSql + ComNum.VBLF + "    ON A.DR_CODE = U.DR_CODE ";
            strSql = strSql + ComNum.VBLF + " LEFT OUTER JOIN   OPDIPD_OCS_SLIP_DMC D  ";
            strSql = strSql + ComNum.VBLF + "    ON A.ITEM_CODE = D.ITEM_CODE ";
            strSql = strSql + ComNum.VBLF + " WHERE PATIENT_NO = '" + p.ptNo + "'   ";
            //if (strQueryDept == "RF" || strQueryDept == "PU" || strQueryDept == "HO" || strQueryDept == "KH" ||
            //    strQueryDept == "GI" || strQueryDept == "ED" || strQueryDept == "CV" || strQueryDept == "CO")
            //{
            //    strSql = strSql + ComNum.VBLF + "           AND A.CLINICAL_DEPT IN ( 'IM', '" + strQueryDept.Trim() + "'  )  ";
            //}
            //else
            //{
            //    strSql = strSql + ComNum.VBLF + "   AND A.CLINICAL_DEPT = '" + strQueryDept.Trim() + "'   ";
            //}
            strSql = strSql + ComNum.VBLF + "   AND A.ORDER_DATE >= TO_DATE('" + ComFunc.FormatStrToDate(p.medFrDate, "D") + "', 'yyyy-MM-dd')  ";
            strSql = strSql + ComNum.VBLF + "   AND A.ORDER_DATE <= TO_DATE('" + strODate + "', 'yyyy-MM-dd')  ";
            strSql = strSql + ComNum.VBLF + "   AND ( (A.DOSAGE_OF_ORDER = '" + strODate + "') OR (A.ORDER_DATE + A.DURATION_OF_ORDER) > TO_DATE('" + strODate + "', 'yyyy-MM-dd')  ";
            strSql = strSql + ComNum.VBLF + "       ) ";
            //strSql = strSql + ComNum.VBLF + "   AND A.CLASS_OF_ORDER IN ('21', '22', '23')";
            strSql = strSql + ComNum.VBLF + "   AND A.GROUP_OF_ORDER IN ('3', '5')";
            strSql = strSql + ComNum.VBLF + "ORDER BY ORDER_DATE, A.DOSAGE_CODE, CLASS_OF_ORDER, DOSAGE_OF_ORDER,    A.SEQ_NO ";

            string sqlErr = clsDB.GetDataTable(ref dt, strSql, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count == 0)
            {

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }
            ssView_Sheet1.RowCount = dt.Rows.Count;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            //MovePreorderedItemsToSpread
            string strMix = "";
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ORDER_DATE"].ToString().Trim();
                ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ITEM_NAME"].ToString().Trim();

                if ((dt.Rows[i]["INPUT_STATUS"].ToString().Trim() == "2") || (dt.Rows[i]["INPUT_STATUS"].ToString().Trim() == "3")) // DC
                {
                    ssView_Sheet1.Cells[i, 1].Text = "[D/C] " + ssView_Sheet1.Cells[i, 1].Text.Trim();
                }
                else
                {
                    if (dt.Rows[i]["INPUT_STATUS"].ToString().Trim() == "1") // ADD
                    {
                        ssView_Sheet1.Cells[i, 1].Text = "추] " + ssView_Sheet1.Cells[i, 1].Text.Trim();
                    }
                }

                if (VB.Val(dt.Rows[i]["QTY_OF_ORDER"].ToString().Trim()) < 0)
                {
                    ssView_Sheet1.Cells[i, 2].Text = "0";
                }
                else
                {
                    ssView_Sheet1.Cells[i, 2].Text = VB.Val(dt.Rows[i]["QTY_OF_ORDER"].ToString().Trim()).ToString();
                }
                ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DOSAGE_OF_ORDER"].ToString().Trim();
                ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["FREQUENCY_OF_ORDER"].ToString().Trim();

                ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["VALUE_OF_FREQUENCY"].ToString().Trim();
                ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DURATION_OF_ORDER"].ToString().Trim();
                ssView_Sheet1.Cells[i, 7].Text = "";
                ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["CLINICAL_DEPT"].ToString().Trim();
                ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DR_NAME"].ToString().Trim();
                ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["ORDER_KEY"].ToString().Trim();
                ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["CLASS_OF_ORDER"].ToString().Trim();
                if ((dt.Rows[i]["CLASS_OF_ORDER"].ToString().Trim() == "11") || (dt.Rows[i]["CLASS_OF_ORDER"].ToString().Trim() == "21") || (dt.Rows[i]["CLASS_OF_ORDER"].ToString().Trim() == "22") || (dt.Rows[i]["CLASS_OF_ORDER"].ToString().Trim() == "23"))
                {
                    ssView_Sheet1.Cells[i, 1].ForeColor = Color.Blue;
                    //ssView_Sheet1.Cells[i, 7].Text = GetActInfo(dt.Rows[i]["ORDER_KEY"].ToString().Trim());
                    //if (ssView_Sheet1.Cells[i, 7].Text.Trim() != "")
                    //{
                    //    ssView_Sheet1.SetRowHeight(i, Convert.ToInt32(ssView_Sheet1.GetPreferredRowHeight(i)) + 10);
                    //}
                    //else
                    //{
                    //    ssView_Sheet1.Cells[i, 7].Text = SetActDefault(dt.Rows[i]["CLASS_OF_ORDER"].ToString().Trim(), dt.Rows[i]["DOSAGE_CODE"].ToString().Trim(), dt.Rows[i]["FREQUENCY_OF_ORDER"].ToString().Trim());
                    //}

                    if ((dt.Rows[i]["CLASS_OF_ORDER"].ToString().Trim() == "21") || (dt.Rows[i]["CLASS_OF_ORDER"].ToString().Trim() == "22") || (dt.Rows[i]["CLASS_OF_ORDER"].ToString().Trim() == "23"))
                    {
                        if (strMix != "")
                        {
                            if (strMix == dt.Rows[i]["DOSAGE_CODE"].ToString().Trim())
                            {
                                ssView_Sheet1.Cells[i, 1].Text = "┃ " + ssView_Sheet1.Cells[i, 1].Text.Trim();
                            }
                            else
                            {
                                ssView_Sheet1.Cells[i - 1, 1].Text = ssView_Sheet1.Cells[i - 1, 1].Text.Trim().Replace("┃", "┗");
                                strMix = "";
                                if ((VB.Left(dt.Rows[i]["DOSAGE_CODE"].ToString(), 5) == "FLIM-") || (VB.Left(dt.Rows[i]["DOSAGE_CODE"].ToString(), 4) == "IMX-"))
                                {
                                    strMix = dt.Rows[i]["DOSAGE_CODE"].ToString().Trim();
                                    ssView_Sheet1.Cells[i, 1].Text = "┏" + ssView_Sheet1.Cells[i, 1].Text.Trim();
                                }
                            }
                        }
                        else
                        {
                            if ((VB.Left(dt.Rows[i]["DOSAGE_CODE"].ToString(), 5) == "FLIM-") || (VB.Left(dt.Rows[i]["DOSAGE_CODE"].ToString(), 4) == "IMX-"))
                            {
                                strMix = dt.Rows[i]["DOSAGE_CODE"].ToString().Trim();
                                ssView_Sheet1.Cells[i, 1].Text = "┏" + ssView_Sheet1.Cells[i, 1].Text.Trim();
                            }
                        }
                    }
                    else
                    {
                        if (strMix != "") ssView_Sheet1.Cells[i - 1, 1].Text = ssView_Sheet1.Cells[i - 1, 1].Text.Trim().Replace("┃", "┗");
                        strMix = "";
                    }
                }
                else
                {
                    if (strMix != "") ssView_Sheet1.Cells[i - 1, 1].Text = ssView_Sheet1.Cells[i - 1, 1].Text.Trim().Replace("┃", "┗");
                    strMix = "";
                }

                if ((dt.Rows[i]["INPUT_STATUS"].ToString().Trim() == "2") || (dt.Rows[i]["INPUT_STATUS"].ToString().Trim() == "3")) // DC
                {
                    ssView_Sheet1.Cells[i, 1].ForeColor = Color.Red;
                }
            }
            if (strMix != "")
            {
                ssView_Sheet1.Cells[i - 1, 1].Text = "┗ " + ssView_Sheet1.Cells[i - 1, 1].Text.Trim();
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void InitSpdSet()
        {
            ssChart_Sheet1.RowCount = 0;
            ssChart_Sheet1.ColumnCount = 0;

            ssChart_Sheet1.RowCount = mintTRow;
            ssChart_Sheet1.ColumnCount = mintTCol;
            ssChart_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            ssChart_Sheet1.SetColumnWidth(-1, mintColW_I);
            ssChart_Sheet1.SetColumnWidth(1, 60);
            ssChart_Sheet1.Columns[0].Visible = false;
            ssChart_Sheet1.Rows[0].Visible = false;
            ssChart_Sheet1.Rows[1].Visible = false;
        }

        private void SetTopRow()
        {
            InitSpdSet();

            clsSpread.SetTypeAndValue(ssChart_Sheet1, 0, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssChart_Sheet1, 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssChart_Sheet1, 2, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssChart_Sheet1, 3, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);

            clsSpread.SetTypeAndValue(ssChart_Sheet1, 0, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "작성일자", false);
            ssChart_Sheet1.Cells[0, 1, 0, 2].BackColor = Color.LightBlue;
            ssChart_Sheet1.AddSpanCell(0, 1, 1, 2);
            clsSpread.SetTypeAndValue(ssChart_Sheet1, 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "Duty", false);
            ssChart_Sheet1.Cells[1, 1, 1, 2].BackColor = Color.LightBlue;
            ssChart_Sheet1.AddSpanCell(1, 1, 1, 2);
            clsSpread.SetTypeAndValue(ssChart_Sheet1, 2, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "작성시간", false);
            ssChart_Sheet1.Cells[2, 1, 2, 2].BackColor = Color.LightBlue;
            ssChart_Sheet1.AddSpanCell(2, 1, 1, 2);
            clsSpread.SetTypeAndValue(ssChart_Sheet1, 3, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "작성자", false);
            ssChart_Sheet1.Cells[3, 1, 3, 2].BackColor = Color.LightBlue;
            ssChart_Sheet1.AddSpanCell(3, 1, 1, 2);
        }

        private void SetButtonRow()
        {
            ssChart_Sheet1.RowCount = ssChart_Sheet1.RowCount + 1;
            clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "EMRNO", false);
            clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);

            ssChart_Sheet1.RowCount = ssChart_Sheet1.RowCount + 1;
            clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "USEID", false);
            clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);

            ssChart_Sheet1.RowCount = ssChart_Sheet1.RowCount + 1;
            clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "PRNTYN", false);
            clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssChart_Sheet1.RowCount = ssChart_Sheet1.RowCount + 1;
            clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "CHANG", false);
            clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssChart_Sheet1.Rows[ssChart_Sheet1.RowCount - 4].Visible = false;
            ssChart_Sheet1.Rows[ssChart_Sheet1.RowCount - 3].Visible = false;
            ssChart_Sheet1.Rows[ssChart_Sheet1.RowCount - 2].Visible = false;
            ssChart_Sheet1.Rows[ssChart_Sheet1.RowCount - 1].Visible = false;
        }

        private void SetTimeSet()
        {
            int i = 0;
            string strSql = string.Empty;
            string sqlErr = string.Empty;

            DataTable dt = null;
            int j = 0;

            strSql = "";
            strSql = "SELECT TIMEVALUE, SUBGB ";
            strSql = strSql + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALTIME";
            strSql = strSql + ComNum.VBLF + "WHERE ACPNO = " + p.acpNo;
            strSql = strSql + ComNum.VBLF + "    AND JOBGB = '" + mJOBGB + "'";
            strSql = strSql + ComNum.VBLF + "    AND CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
            strSql = strSql + ComNum.VBLF + "ORDER BY TO_NUMBER(TIMEVALUE), TO_NUMBER(SUBGB)";

            sqlErr = clsDB.GetDataTable(ref dt, strSql, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssChart_Sheet1.Columns.Count = ssChart_Sheet1.Columns.Count + 1;
                    ssChart_Sheet1.SetColumnWidth(ssChart_Sheet1.Columns.Count - 1, mintColW_V);

                    clsSpread.SetTypeAndValue(ssChart_Sheet1, 0, ssChart_Sheet1.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    ssChart_Sheet1.Cells[0, ssChart_Sheet1.Columns.Count - 1].BackColor = Color.LightBlue;
                    clsSpread.SetTypeAndValue(ssChart_Sheet1, 1, ssChart_Sheet1.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    ssChart_Sheet1.Cells[1, ssChart_Sheet1.Columns.Count - 1].BackColor = Color.LightBlue;
                    clsSpread.SetTypeAndValue(ssChart_Sheet1, 2, ssChart_Sheet1.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate(dt.Rows[i]["TIMEVALUE"].ToString().Trim(), "M"), false);
                    ssChart_Sheet1.Cells[2, ssChart_Sheet1.Columns.Count - 1].BackColor = Color.LightBlue;
                    clsSpread.SetTypeAndValue(ssChart_Sheet1, 3, ssChart_Sheet1.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    ssChart_Sheet1.Cells[3, ssChart_Sheet1.Columns.Count - 1].BackColor = Color.LightBlue;

                    if (dt.Rows[i]["SUBGB"].ToString().Trim() == "1")
                    {
                        ssChart_Sheet1.Cells[3, ssChart_Sheet1.Columns.Count - 1].Text = "합계";
                    }

                    for (j = 4; j < ssChart_Sheet1.RowCount - 4; j++)
                    {
                        clsSpread.SetTypeAndValue(ssChart_Sheet1, j, ssChart_Sheet1.Columns.Count - 1, "TypeText", false, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                        if ((ssChart_Sheet1.Cells[j, 0].Text.Trim() == "I0000030622") || (ssChart_Sheet1.Cells[j, 0].Text.Trim() == "I0000030623"))
                        {
                            ssChart_Sheet1.Cells[j, 0, j, ssChart_Sheet1.ColumnCount - 1].BackColor = Color.LightYellow;
                        }
                    }
                    clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 4, ssChart_Sheet1.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 3, ssChart_Sheet1.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 2, ssChart_Sheet1.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 1, ssChart_Sheet1.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                }
                dt.Dispose();
                dt = null;
                return;
            }
            dt.Dispose();
            dt = null;

            for (i = 0; i < 24; i++)
            {
                ssChart_Sheet1.Columns.Count = ssChart_Sheet1.Columns.Count + 1;
                ssChart_Sheet1.SetColumnWidth(ssChart_Sheet1.Columns.Count - 1, mintColW_V);

                clsSpread.SetTypeAndValue(ssChart_Sheet1, 0, ssChart_Sheet1.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                ssChart_Sheet1.Cells[0, ssChart_Sheet1.Columns.Count - 1].BackColor = Color.LightBlue;
                clsSpread.SetTypeAndValue(ssChart_Sheet1, 1, ssChart_Sheet1.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                ssChart_Sheet1.Cells[1, ssChart_Sheet1.Columns.Count - 1].BackColor = Color.LightBlue;
                clsSpread.SetTypeAndValue(ssChart_Sheet1, 2, ssChart_Sheet1.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.SetAutoZero(i.ToString().Trim(), 2) + ":00", false);
                ssChart_Sheet1.Cells[2, ssChart_Sheet1.Columns.Count - 1].BackColor = Color.LightBlue;
                clsSpread.SetTypeAndValue(ssChart_Sheet1, 3, ssChart_Sheet1.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                ssChart_Sheet1.Cells[3, ssChart_Sheet1.Columns.Count - 1].BackColor = Color.LightBlue;

                for (j = 4; j < ssChart_Sheet1.RowCount - 4; j++)
                {
                    clsSpread.SetTypeAndValue(ssChart_Sheet1, j, ssChart_Sheet1.Columns.Count - 1, "TypeText", false, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                    if ((ssChart_Sheet1.Cells[j, 0].Text.Trim() == "I0000030622") || (ssChart_Sheet1.Cells[j, 0].Text.Trim() == "I0000030623"))
                    {
                        ssChart_Sheet1.Cells[j, 0, j, ssChart_Sheet1.ColumnCount - 1].BackColor = Color.LightYellow;
                    }
                }
                clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 4, ssChart_Sheet1.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 3, ssChart_Sheet1.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 2, ssChart_Sheet1.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 1, ssChart_Sheet1.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            }

            //섭취량, 배설량은 노란색으로

        }

        private void ssChart_EditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column < 3) return;
            if (e.Row < 4) return;
            if (e.Row > ssChart_Sheet1.RowCount - 4) return;

            ssChart_Sheet1.Cells[ssChart_Sheet1.RowCount - 1, e.Column].Text = "Y";
            //합계를 구한다.
            SumIntakeOutPutRow(e.Column);
        }

        private void mbtnSaveAll_Click(object sender, EventArgs e)
        {
            pSaveData("1"); //인증저장
            pClearFormExcept();
            pLoadEmrChartInfo();
        }

        private void mbtnSearchAll_Click(object sender, EventArgs e)
        {
            pClearFormExcept();
            pLoadEmrChartInfo();
        }

        private void dtpFrDate_ValueChanged(object sender, EventArgs e)
        {
            pClearFormExcept();
            pLoadEmrChartInfo();
        }

        private void mbtnBefore_Click(object sender, EventArgs e)
        {
            dtpFrDate.Value = dtpFrDate.Value.AddDays(-1);
        }

        private void mbtnNext_Click(object sender, EventArgs e)
        {
            dtpFrDate.Value = dtpFrDate.Value.AddDays(+1);
        }

        private void ssChart_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }

        private void ssChart_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            PopupMenu = null;
            ssChart.ContextMenu = null;

            if (ssChart_Sheet1.Columns.Count <= 3) return;

            if (e.Column < 3) return;

            for (i = 3; i < ssChart_Sheet1.Columns.Count; i++)
            {
                ssChart_Sheet1.Cells[0, i, ssChart_Sheet1.Rows.Count - 1, i].Font = new Font("굴림", 9, FontStyle.Regular);
            }
            ssChart_Sheet1.Cells[0, e.Column, ssChart_Sheet1.Rows.Count - 1, e.Column].Font = new Font("굴림", 9, FontStyle.Bold);

            //합계 추가, 삭제
            if (e.Button == MouseButtons.Left) return;
            if (e.Column < 4) return;

            string strCHARTUSEID = ssChart_Sheet1.Cells[3, e.Column].Text.Trim();
            mCol = e.Column;
            if (strCHARTUSEID == "합계")
            {
                PopupMenu = new ContextMenu();
                PopupMenu.MenuItems.Add("합계 삭제", new System.EventHandler(SubMenuClick));
                ssChart.ContextMenu = PopupMenu; // 입력
            }
            else
            {
                PopupMenu = new ContextMenu();
                PopupMenu.MenuItems.Add("합계 추가", new System.EventHandler(SubMenuClick));
                ssChart.ContextMenu = PopupMenu; // 입력
            }
        }

        private void SubMenuClick(object sender, EventArgs e)
        {
            string strJob = ((MenuItem)sender).Text.Trim();
            string strTime = ssChart_Sheet1.Cells[2, mCol].Text.Trim();

            if (strJob == "합계 추가")
            {
                TotAdd(strTime);
            }
            else if (strJob == "합계 삭제")
            {
                TotDel(strTime);
            }
        }

        private void TotDel(string strTime)
        {
            int intSelCol = mCol;

            //EMRNO가 있는지 확인
            string strEmrNo = Convert.ToString(VB.Val(ssChart_Sheet1.Cells[ssChart_Sheet1.Rows.Count - 4, intSelCol].Text.Trim()));

            if (ssChart_Sheet1.Cells[3, intSelCol].Text.Trim() == "합계")
            {
                if (VB.Val(strEmrNo) == 0)
                {
                    ssChart_Sheet1.Columns[intSelCol].Remove();
                    if (SaveTimeSet() == false)
                    {
                        ComFunc.MsgBoxEx(this, "시간 저장중 에러가 발생했습니다.");
                    }
                    pClearFormExcept();
                    pLoadEmrChartInfo();
                    return;
                }
                mstrEmrNo = strEmrNo;
                if (pDelData("합계") == true)
                {
                    ssChart_Sheet1.Columns[intSelCol].Remove();
                    if (SaveTimeSet() == false)
                    {
                        ComFunc.MsgBoxEx(this, "시간 저장중 에러가 발생했습니다.");
                    }
                    pClearFormExcept();
                    pLoadEmrChartInfo();
                }
                return;
            }
            mstrEmrNo = "0";
        }

        private void TotAdd(string strTime)
        {
            int j = 0;
            int intSelCol = 0;
            int intTime = 0;
            intTime = Convert.ToInt32(VB.Val(strTime.Replace(":", "")));

            if (intTime <= 0) return;

            if (mCol == ssChart_Sheet1.ColumnCount - 1)
            {
                ssChart_Sheet1.ColumnCount = ssChart_Sheet1.ColumnCount + 1;
            }
            else
            {
                ssChart_Sheet1.AddColumns(mCol + 1, 1);
            }
            intSelCol = mCol + 1;

            ssChart_Sheet1.Columns[intSelCol].Width = mintColW_V;

            clsSpread.SetTypeAndValue(ssChart_Sheet1, 0, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssChart_Sheet1.Cells[0, intSelCol].BackColor = Color.LightBlue;
            clsSpread.SetTypeAndValue(ssChart_Sheet1, 1, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssChart_Sheet1.Cells[1, intSelCol].BackColor = Color.LightBlue;
            clsSpread.SetTypeAndValue(ssChart_Sheet1, 2, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssChart_Sheet1.Cells[2, intSelCol].BackColor = Color.LightBlue;
            ssChart_Sheet1.Cells[2, intSelCol].Text = strTime;
            clsSpread.SetTypeAndValue(ssChart_Sheet1, 3, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssChart_Sheet1.Cells[3, intSelCol].BackColor = Color.LightBlue;
            for (j = 4; j < ssChart_Sheet1.RowCount - 4; j++)
            {
                clsSpread.SetTypeAndValue(ssChart_Sheet1, j, intSelCol, "TypeText", false, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                if ((ssChart_Sheet1.Cells[j, 0].Text.Trim() == "I0000030622") || (ssChart_Sheet1.Cells[j, 0].Text.Trim() == "I0000030623"))
                {
                    ssChart_Sheet1.Cells[j, 0, j, ssChart_Sheet1.ColumnCount - 1].BackColor = Color.LightYellow;
                }
            }
            clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 4, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 3, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 2, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 1, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);

            ssChart_Sheet1.Cells[3, intSelCol].Text = "합계";
            ssChart_Sheet1.Cells[ssChart_Sheet1.Rows.Count - 3, intSelCol].Text = "합계";

            if (SaveTimeSet() == false)
            {
                ComFunc.MsgBoxEx(this, "시간 저장중 에러가 발생했습니다.");
            }

            pSaveDataTot(intSelCol);
            pClearFormExcept();
            pLoadEmrChartInfo();
        }

        private void mbtnDelete_Click(object sender, EventArgs e)
        {
            DeleteOne();
        }

        private void mbtnDeleteAll_Click(object sender, EventArgs e)
        {
            int intSelCol = 0;
            if (ssChart_Sheet1.Columns.Count <= 3) return;

            if (ssChart_Sheet1.ActiveColumnIndex < 3) return;
            intSelCol = ssChart_Sheet1.ActiveColumnIndex;
            string strTime = ssChart_Sheet1.Cells[2, intSelCol].Text.Trim();

            if (VB.Right(strTime, 2) == "00")
            {
                DeleteOne();
            }
            else
            {
                DeleteAll();
            }
        }

        private void DeleteOne()
        {
            int intSelCol = 0;

            if (ssChart_Sheet1.Columns.Count <= 3) return;

            if (ssChart_Sheet1.ActiveColumnIndex < 3) return;
            intSelCol = ssChart_Sheet1.ActiveColumnIndex;

            if (ssChart_Sheet1.Cells[3, intSelCol].Text.Trim() == "합계") return;

            //EMRNO가 있는지 확인
            string strEmrNo = Convert.ToString(VB.Val(ssChart_Sheet1.Cells[ssChart_Sheet1.Rows.Count - 4, intSelCol].Text.Trim()));

            if (VB.Val(strEmrNo) == 0)
            {
                for (int k = 4; k < ssChart_Sheet1.RowCount - 4; k++)
                {
                    ssChart_Sheet1.Cells[k, intSelCol].Text = "";
                }
                return;
            }
            mstrEmrNo = strEmrNo;
            if (pDelData() == true)
            {
                pClearFormExcept();
                pLoadEmrChartInfo();
            }
            mstrEmrNo = "0";
        }

        private void DeleteAll()
        {
            int intSelCol = 0;

            if (ssChart_Sheet1.Columns.Count <= 3) return;

            if (ssChart_Sheet1.ActiveColumnIndex < 3) return;
            intSelCol = ssChart_Sheet1.ActiveColumnIndex;

            //EMRNO가 있는지 확인
            string strEmrNo = Convert.ToString(VB.Val(ssChart_Sheet1.Cells[ssChart_Sheet1.Rows.Count - 4, intSelCol].Text.Trim()));

            if (ssChart_Sheet1.Cells[3, intSelCol].Text.Trim() == "합계")
            {
                return;
            }

            if (VB.Val(strEmrNo) == 0)
            {
                ssChart_Sheet1.Columns[intSelCol].Remove();
                if (SaveTimeSet() == false)
                {
                    ComFunc.MsgBoxEx(this, "시간 저장중 에러가 발생했습니다.");
                }
                pClearFormExcept();
                pLoadEmrChartInfo();
                return;
            }
            mstrEmrNo = strEmrNo;
            if (pDelData() == true)
            {
                ssChart_Sheet1.Columns[intSelCol].Remove();
                if (SaveTimeSet() == false)
                {
                    ComFunc.MsgBoxEx(this, "시간 저장중 에러가 발생했습니다.");
                }
                pClearFormExcept();
                pLoadEmrChartInfo();
            }
            mstrEmrNo = "0";
        }

        private void mbtnInsert_Click(object sender, EventArgs e)
        {
            int i = 0;
            int j = 0;
            int intSelCol = 0;
            int intTime = 0;
            intTime = Convert.ToInt32(VB.Val(txtTime.Text.Replace(":", "")));

            if (intTime <= 0) return;
            if (intTime > 2400) return;

            if (intTime < Convert.ToInt32(VB.Val(ssChart_Sheet1.Cells[2, 3].Text.Replace(":", ""))))
            {
                intSelCol = 3;
                ssChart_Sheet1.AddColumns(intSelCol, 1);
            }
            else
            {
                for (i = 3; i < ssChart_Sheet1.Columns.Count; i++)
                {
                    if (intTime > Convert.ToInt32(VB.Val(ssChart_Sheet1.Cells[2, i].Text.Replace(":", ""))))
                    {
                        if (i + 1 == ssChart_Sheet1.Columns.Count)
                        {
                            break;
                        }
                        else
                        {
                            if (intTime < Convert.ToInt32(VB.Val(ssChart_Sheet1.Cells[2, i + 1].Text.Replace(":", ""))))
                            {
                                intSelCol = i;
                                break;
                            }
                        }
                    }
                }
                if (intSelCol == 0)
                {
                    ssChart_Sheet1.Columns.Count = ssChart_Sheet1.Columns.Count + 1;
                    intSelCol = ssChart_Sheet1.Columns.Count - 1;
                }
                else
                {
                    intSelCol = intSelCol + 1;
                    ssChart_Sheet1.AddColumns(intSelCol, 1);
                }
            }

            ssChart_Sheet1.Columns[intSelCol].Width = mintColW_V;

            clsSpread.SetTypeAndValue(ssChart_Sheet1, 0, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssChart_Sheet1.Cells[0, intSelCol].BackColor = Color.LightBlue;
            clsSpread.SetTypeAndValue(ssChart_Sheet1, 1, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssChart_Sheet1.Cells[1, intSelCol].BackColor = Color.LightBlue;
            clsSpread.SetTypeAndValue(ssChart_Sheet1, 2, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssChart_Sheet1.Cells[2, intSelCol].BackColor = Color.LightBlue;
            ssChart_Sheet1.Cells[2, intSelCol].Text = txtTime.Text;
            clsSpread.SetTypeAndValue(ssChart_Sheet1, 3, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssChart_Sheet1.Cells[3, intSelCol].BackColor = Color.LightBlue;
            for (j = 4; j < ssChart_Sheet1.RowCount - 4; j++)
            {
                clsSpread.SetTypeAndValue(ssChart_Sheet1, j, intSelCol, "TypeText", false, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                if ((ssChart_Sheet1.Cells[j, 0].Text.Trim() == "I0000030622") || (ssChart_Sheet1.Cells[j, 0].Text.Trim() == "I0000030623"))
                {
                    ssChart_Sheet1.Cells[j, 0, j, ssChart_Sheet1.ColumnCount - 1].BackColor = Color.LightYellow;
                }
            }
            clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 4, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 3, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 2, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 1, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);

            if (SaveTimeSet() == false)
            {
                ComFunc.MsgBoxEx(this, "시간 저장중 에러가 발생했습니다.");
            }
        }

        private void mbtnInsertTot_Click(object sender, EventArgs e)
        {
            int i = 0;
            int j = 0;
            int intSelCol = 0;
            int intTime = 0;
            intTime = Convert.ToInt32(VB.Val(txtTimeTot.Text.Replace(":", "")));

            if (intTime <= 0) return;

            if (intTime < Convert.ToInt32(VB.Val(ssChart_Sheet1.Cells[2, 3].Text.Replace(":", ""))))
            {
                intSelCol = 3;
                ssChart_Sheet1.AddColumns(intSelCol, 1);
            }
            else
            {
                for (i = 3; i < ssChart_Sheet1.Columns.Count; i++)
                {
                    if (intTime > Convert.ToInt32(VB.Val(ssChart_Sheet1.Cells[2, i].Text.Replace(":", ""))))
                    {
                        if (i + 1 == ssChart_Sheet1.Columns.Count)
                        {
                            break;
                        }
                        else
                        {
                            if (intTime < Convert.ToInt32(VB.Val(ssChart_Sheet1.Cells[2, i + 1].Text.Replace(":", ""))))
                            {
                                intSelCol = i;
                                break;
                            }
                        }
                    }
                }
                if (intSelCol == 0)
                {
                    ssChart_Sheet1.Columns.Count = ssChart_Sheet1.Columns.Count + 1;
                    intSelCol = ssChart_Sheet1.Columns.Count - 1;
                }
                else
                {
                    intSelCol = intSelCol + 1;
                    ssChart_Sheet1.AddColumns(intSelCol, 1);
                }
            }

            ssChart_Sheet1.Columns[intSelCol].Width = mintColW_V;

            clsSpread.SetTypeAndValue(ssChart_Sheet1, 0, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssChart_Sheet1.Cells[0, intSelCol].BackColor = Color.LightBlue;
            clsSpread.SetTypeAndValue(ssChart_Sheet1, 1, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssChart_Sheet1.Cells[1, intSelCol].BackColor = Color.LightBlue;
            clsSpread.SetTypeAndValue(ssChart_Sheet1, 2, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssChart_Sheet1.Cells[2, intSelCol].BackColor = Color.LightBlue;
            ssChart_Sheet1.Cells[2, intSelCol].Text = txtTimeTot.Text;
            clsSpread.SetTypeAndValue(ssChart_Sheet1, 3, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssChart_Sheet1.Cells[3, intSelCol].BackColor = Color.LightBlue;
            for (j = 4; j < ssChart_Sheet1.RowCount - 4; j++)
            {
                clsSpread.SetTypeAndValue(ssChart_Sheet1, j, intSelCol, "TypeText", false, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                if ((ssChart_Sheet1.Cells[j, 0].Text.Trim() == "I0000030622") || (ssChart_Sheet1.Cells[j, 0].Text.Trim() == "I0000030623"))
                {
                    ssChart_Sheet1.Cells[j, 0, j, ssChart_Sheet1.ColumnCount - 1].BackColor = Color.LightYellow;
                }
            }
            clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 4, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 3, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 2, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssChart_Sheet1, ssChart_Sheet1.RowCount - 1, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);

            if (SaveTimeSet() == false)
            {
                ComFunc.MsgBoxEx(this, "시간 저장중 에러가 발생했습니다.");
            }

            //기존 내용이 있으면 합계를 바로 저장을 한다.
            //USEID = IOTOT

            pSaveDataTot(intSelCol);
            pClearFormExcept();
            pLoadEmrChartInfo();

        }

        public double pSaveDataTot(int intSelCol)
        {
            double dblEmrNo = 0;

            int i = intSelCol;
            int j = 0;

            string[] arryEMRNO = null;
            string[] arryITEMCD = null;
            string[] arryITEMNO = null;
            string[] arryITEMINDEX = null;
            string[] arryITEMTYPE = null;
            string[] arryITEMVALUE = null;
            string[] arryITEMVALUE1 = null;
            string[] arryDSPSEQ = null;

            //if (ssChart_Sheet1.Cells[ssChart_Sheet1.RowCount - 1, i].Text == "Y")
            //{
            string strEmrNo = Convert.ToString(VB.Val(ssChart_Sheet1.Cells[ssChart_Sheet1.Rows.Count - 4, i].Text.Trim()));
            string strChartDate = dtpFrDate.Value.ToString("yyyyMMdd");// ssChart_Sheet1.Cells[0, i].Text.Trim().Replace("-", "");
            string strChartTime = ssChart_Sheet1.Cells[2, i].Text.Trim().Replace(":", "") + "00";
            string strCHARTUSEID = "합계";
            string strCOMPUSEID = "합계";
            string strSAVEGB = "0";
            string strFORMGB = "0";

            //if (VB.Val(strEmrNo) > 0)
            //{
            //    strCHARTUSEID = ssChart_Sheet1.Cells[ssChart_Sheet1.Rows.Count - 3, i].Text.Trim();
            //    strCOMPUSEID = ssChart_Sheet1.Cells[ssChart_Sheet1.Rows.Count - 3, i].Text.Trim();
            //}
            //else
            //{
            //    strCHARTUSEID = clsType.User.IdNumber;
            //    strCOMPUSEID = clsType.User.IdNumber;
            //}

            SumIntakeOutPutRowTot(intSelCol);

            for (j = 4; j < ssChart_Sheet1.RowCount - 4; j++)
            {
                string strITEMCD = ssChart_Sheet1.Cells[j, 0].Text.Trim();
                string[] strItem = VB.Split(ssChart_Sheet1.Cells[j, 0].Text.Trim(), "_");
                string strITEMNO = "";
                string strITEMINDEX = "-1";
                if (strItem.Length > 0)
                {
                    strITEMNO = strItem[0].Trim();
                }
                if (strItem.Length > 1)
                {
                    strITEMINDEX = strItem[1].Trim();
                }
                string strDSPSEQ = "0";
                string strITEMTYPE = "TEXT";
                string strITEMVALUE = ssChart_Sheet1.Cells[j, i].Text.Trim();
                string strITEMVALUE1 = "";

                if (arryEMRNO == null)
                {
                    arryEMRNO = new string[0];
                    arryITEMCD = new string[0];
                    arryITEMNO = new string[0];
                    arryITEMINDEX = new string[0];
                    arryITEMTYPE = new string[0];
                    arryITEMVALUE = new string[0];
                    arryITEMVALUE1 = new string[0];
                    arryDSPSEQ = new string[0];
                }

                Array.Resize<string>(ref arryEMRNO, arryEMRNO.Length + 1);
                Array.Resize<string>(ref arryITEMCD, arryITEMCD.Length + 1);
                Array.Resize<string>(ref arryITEMNO, arryITEMNO.Length + 1);
                Array.Resize<string>(ref arryITEMINDEX, arryITEMINDEX.Length + 1);
                Array.Resize<string>(ref arryITEMTYPE, arryITEMTYPE.Length + 1);
                Array.Resize<string>(ref arryITEMVALUE, arryITEMVALUE.Length + 1);
                Array.Resize<string>(ref arryITEMVALUE1, arryITEMVALUE1.Length + 1);
                Array.Resize<string>(ref arryDSPSEQ, arryDSPSEQ.Length + 1);

                arryEMRNO[arryEMRNO.Length - 1] = "0";
                arryITEMCD[arryEMRNO.Length - 1] = strITEMCD;
                arryITEMNO[arryEMRNO.Length - 1] = strITEMNO;
                arryITEMINDEX[arryEMRNO.Length - 1] = strITEMINDEX;
                arryITEMTYPE[arryEMRNO.Length - 1] = strITEMTYPE;
                arryITEMVALUE[arryEMRNO.Length - 1] = strITEMVALUE;
                arryITEMVALUE1[arryEMRNO.Length - 1] = strITEMVALUE1;
                arryDSPSEQ[arryEMRNO.Length - 1] = strDSPSEQ;
            }

            string strSAVECERT = "1";
            if (clsEmrQuery.SaveFlowChart(clsDB.DbCon, p, mstrFormNo, mstrUpdateNo, strEmrNo, strChartDate, strChartTime, 
                strCHARTUSEID, strCOMPUSEID, strSAVEGB, strSAVECERT, strFORMGB,
                arryITEMCD, arryITEMNO, arryITEMINDEX, arryITEMTYPE, arryITEMVALUE, arryDSPSEQ, arryITEMVALUE1
                ) == 0)
            {
                //에러임
            }
            //}

            return dblEmrNo;
        }

        private void ssChart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                clsSpread.gSpreadEnter(ssChart);
            }
        }

        public void SetUserFormMsg(double dblMACRONO)
        {
            
        }

        public bool SaveUserFormMsg(double dblMACRONO)
        {
            return false;
        }
    }
}
