using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Data;
using System.Globalization;
using Microsoft.Win32;
using ComDbB; //DB연결

namespace ComBase
{

    /// <summary>SetComputerInfo
    /// 전체 공통함수 모음
    /// </summary>
    public class ComFunc : MTSDisposable
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        //public string strSysDate; // clsPublic 으로 변경
        //public string strSysTime; // clsPublic 으로 변경

        /// <summary>
        /// 시스템 시간 날짜, 시간
        /// </summary>
        public static void ReadSysDate(PsmhDb pDbCon)
        {
            string SQL = "";    //Query문
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(SysDate, 'YYYY-MM-DD') SDate,";
                SQL += ComNum.VBLF + "        TO_CHAR(SysDate, 'HH24:MI') STime";
                SQL += ComNum.VBLF + "   FROM DUAL";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                }

                if (dt.Rows.Count > 0)
                {
                    clsPublic.GstrSysDate = dt.Rows[0]["SDate"].ToString().Trim();
                    clsPublic.GstrSysTime = dt.Rows[0]["STime"].ToString().Trim();
                }
                else
                {
                    clsPublic.GstrSysDate = DateTime.Now.ToString("yyyy-MM-dd");
                    clsPublic.GstrSysTime = DateTime.Now.ToString("HH24:MI");
                }

                clsPublic.GstrActDate = clsPublic.GstrSysDate;//발생일자를 회계일자로 만듬
                clsPublic.GstrBdate = clsPublic.GstrSysDate;
                clsPublic.GstrSysTomorrow = VB.DateAdd("D", 1, clsPublic.GstrSysDate).ToString("yyyy-MM-dd");

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return;
            }

        }

        /// <summary>
        /// 해당 문자열에서 Val숫자(바이트) 만큼 짤라서 넘겨준다
        /// </summary>
        /// <param name="Source">문자열</param>
        /// <param name="Val">자를 바이트 수</param>
        /// <returns></returns>
        public static List<string> GetByteString(string Source, int Val)
        {
            List<string> lstRtn = new List<string>();

            try
            {
                int intLen = 0;
                int intLenTot = (int)ComFunc.LenH(Source);

                if (intLenTot < Val)
                {
                    lstRtn.Add(Source);
                    return lstRtn;
                }

                string strTmp0 = ComFunc.GetMidStr(Source, 0, Val);
                for (int i = 0; i < intLenTot; i += Val)
                {
                    if (i > 0)
                    {
                        strTmp0 = ComFunc.GetMidStr(Source, intLen, intLenTot - intLen > Val ? Val : intLenTot - intLen);
                    }
                    if (VB.Right(strTmp0, 1) == "\r" || VB.Right(strTmp0, 1) == "?")
                    {
                        lstRtn.Add(ComFunc.GetMidStr(Source, intLen, intLenTot - intLen > (Val - 1) ? (Val - 1) : intLenTot - intLen));
                        intLen += (Val - 1);
                    }
                    else
                    {
                        lstRtn.Add(strTmp0);
                        intLen += Val;
                    }
                }

                if (intLen < intLenTot)
                {
                    lstRtn.Add(ComFunc.GetMidStr(Source, intLen, intLenTot - intLen));
                }

            }
            catch (Exception ex)
            {

            }

            return lstRtn;
        }

        /// <summary>
        /// 폼로드시 필요한 세팅을 한다
        /// </summary>
        /// <param name="frm">폼</param>
        /// <param name="pLog">로그저장</param>
        /// <param name="pSkin">스킨 적용</param>
        /// <param name="pAuth">권한 설정</param>
        public static void SetFormInit(PsmhDb pDbCon, Form frm, string pLog = "Y", string pSkin = "Y", string pAuth = "Y")
        {
            //폼 접속 로그를 저장한다.
            if (pLog == "Y")
            {
                ComQuery.SaveFormLog(pDbCon, frm);
            }

            //폼 타이틀을 입힌다.
            SetFormTitle(pDbCon, frm);

            //폼 스킨을 입힌다
            if (pSkin == "Y")
            {
                SetFormSkin(pDbCon, frm);
            }

            //폼 조회,저장 등 권한별 버튼 활성화
            if (pAuth == "Y")
            {
                ComQuery.SetFormJobAuth(pDbCon, frm);
            }
        }

        /// <summary>
        /// 폼 타이틀을 입힌다 
        /// 폼명(물리) + 담당자 + 전화번호
        /// <param name="objParent">폼</param>
        /// </summary>
        public static void SetFormTitle(PsmhDb pDbCon, Form pForm)
        {
            DataTable dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            //FORMNAME + ( FORMAUTH + ☎ FORMAUTHTEL ) + ( PROJECTNAME : VERSION ) + (사용자 + 컴퓨터 IP)
            try
            {
                //SQL = "";
                //SQL = " SELECT  ";
                //SQL = SQL + ComNum.VBLF + "    FORMNAME, FORMNAME1, FORMAUTH, FORMAUTHTEL ";
                //SQL = SQL + ComNum.VBLF + "FROM BAS_PROJECTFORM ";
                //SQL = SQL + ComNum.VBLF + "WHERE FORMNAME = '" + pForm.Name + "' ";
                //SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                //if (SqlErr != "")
                //{
                //    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                //    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                //    return;
                //}

                //if (dt.Rows.Count == 0)
                //{
                //    dt.Dispose();
                //    dt = null;
                //    return;
                //}
                //pForm.Text = dt.Rows[0]["FORMNAME"].ToString().Trim() + " (" + dt.Rows[0]["FORMAUTH"].ToString().Trim() + " : " + dt.Rows[0]["FORMAUTHTEL"].ToString().Trim() + ")";
                //dt.Dispose();
                //dt = null;

                //============================================================//
                //============================================================//

                if (pForm.Name == "frmSplash")
                {
                    return;
                }

                SQL = "";
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    PROJECTNAME, FORMNAME, FORMNAME1, FORMAUTH, FORMAUTHTEL ";
                SQL = SQL + ComNum.VBLF + "FROM BAS_PROJECTFORM ";
                SQL = SQL + ComNum.VBLF + "WHERE FORMNAME = '" + pForm.Name + "' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                string[] arryAssName = VB.Split(dt.Rows[0]["PROJECTNAME"].ToString().Trim(), ".");
                string strAssName = arryAssName[0];
                strAssName = strAssName + ".dll";

                string strPROJECTNAME = dt.Rows[0]["PROJECTNAME"].ToString().Trim();
                string strFORMNAME = dt.Rows[0]["FORMNAME"].ToString().Trim();
                string strFORMAUTH = dt.Rows[0]["FORMAUTH"].ToString().Trim();
                string strFORMAUTHTEL = dt.Rows[0]["FORMAUTHTEL"].ToString().Trim();
                dt.Dispose();
                dt = null;

                string strUpdateIniFile = @"C:\PSMHEXE\PSMHAutoUpdate.ini";
                clsIniFile myIniFile = new clsIniFile(strUpdateIniFile);
                double dblVerClt = myIniFile.ReadValue("DEFAULT_UPDATE_LIST", strAssName, 0);

                pForm.Text = strFORMNAME + " (" + strFORMAUTH + " ☎ " + strFORMAUTHTEL + ")"
                            + VB.Space(6) + " (" + strPROJECTNAME + " : Ver " + dblVerClt.ToString() + ")"
                            + VB.Space(10) + " 사용자:" + clsType.User.UserName + " (" + clsCompuInfo.gstrCOMIP + ")";

                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                return;
            }
        }

        /// <summary>
        /// 폼 스킨을 입힌다 
        /// 운영체제 마다 버튼 모양, 스프래드 생긴것이 달라서 통일화 하고자 함
        /// <param name="objParent">폼</param>
        /// </summary>
        public static void SetFormSkin(PsmhDb pDbCon, Form pForm)
        {
            string strClient = @"C:\PSMHEXE";
            if (File.Exists(strClient + "\\Icon\\psmhexe.ico") == true)
            {
                pForm.Icon = new System.Drawing.Icon(strClient + "\\Icon\\psmhexe.ico");
            }

            //3가지중에서 선택을 한다
            //기본, 3D, 사용자
            DataTable dt = null;
            string SQL = "";    //Query문
            SQL = "";
            SQL = SQL + ComNum.VBLF + "WHERE IDNUMBER = '" + clsType.User.IdNumber + "'";
            SQL = SQL + ComNum.VBLF + "     AND OPTIONGB = 'UserFormSkin' ";
            dt = ComQuery.Select_BAS_USEROPTION(pDbCon, SQL);

            if (dt == null)
            {
                SetFormSkinDefault(pForm);
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                SetFormSkinDefault(pForm);
                return;
            }

            int intNVALUE1 = 0;
            intNVALUE1 = (int)VB.Val(dt.Rows[0]["NVALUE1"].ToString().Trim());
            dt.Dispose();
            dt = null;

            if (intNVALUE1 == 1)
            {
                SetFormSkin3D(pForm);
            }
            else if (intNVALUE1 == 2)
            {
                SetFormSkinUser(pDbCon, pForm);
            }
            else
            {
                SetFormSkinDefault(pForm);
            }
        }

        /// <summary>
        /// 사용자정의 폼 스킨을 입힌다.
        /// </summary>
        /// <param name="pForm"></param>
        public static void SetFormSkinUser(PsmhDb pDbCon, Form pForm)
        {
            string SetlblTitle = "";
            string SetlblTitleColor = "";
            string SetpanTitle = "";
            string SetpanTitleColor = "";
            string SetlblTitleSub = "";
            string SetlblTitleSubColor = "";
            string SetpanTitleSub = "";
            string SetpanTitleSubColor = "";
            string SetSpread = "";
            string SetSpreadColor = "";
            string SetButton = "";

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문

            SQL = "";
            SQL = SQL + ComNum.VBLF + "WHERE IDNUMBER = '" + clsType.User.IdNumber + "'";
            SQL = SQL + ComNum.VBLF + "     AND OPTIONGB IN ('SetFormTitle', 'SetFormPanTitle', 'SetFormSubTitle', 'SetFormPanSubTitle', 'SetFormSpread', 'SetFormButton') ";
            dt = ComQuery.Select_BAS_USEROPTION(pDbCon, SQL);

            if (dt == null)
            {
                SetFormSkinDefault(pForm);
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                SetFormSkinDefault(pForm);
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["OPTIONGB"].ToString().Trim() == "SetFormTitle")
                {
                    SetlblTitle = (VB.Val(dt.Rows[i]["NVALUE1"].ToString().Trim())).ToString();
                    SetlblTitleColor = (VB.Val(dt.Rows[i]["NVALUE2"].ToString().Trim())).ToString();
                }
                if (dt.Rows[i]["OPTIONGB"].ToString().Trim() == "SetFormPanTitle")
                {
                    SetpanTitle = (VB.Val(dt.Rows[i]["NVALUE1"].ToString().Trim())).ToString();
                    SetpanTitleColor = (VB.Val(dt.Rows[i]["NVALUE2"].ToString().Trim())).ToString();
                }
                if (dt.Rows[i]["OPTIONGB"].ToString().Trim() == "SetFormSubTitle")
                {
                    SetlblTitleSub = (VB.Val(dt.Rows[i]["NVALUE1"].ToString().Trim())).ToString();
                    SetlblTitleSubColor = (VB.Val(dt.Rows[i]["NVALUE2"].ToString().Trim())).ToString();
                }
                if (dt.Rows[i]["OPTIONGB"].ToString().Trim() == "SetFormPanSubTitle")
                {
                    SetpanTitleSub = (VB.Val(dt.Rows[i]["NVALUE1"].ToString().Trim())).ToString();
                    SetpanTitleSubColor = (VB.Val(dt.Rows[i]["NVALUE2"].ToString().Trim())).ToString();
                }
                if (dt.Rows[i]["OPTIONGB"].ToString().Trim() == "SetFormSpread")
                {
                    SetSpread = (VB.Val(dt.Rows[i]["NVALUE1"].ToString().Trim())).ToString();
                    SetSpreadColor = (VB.Val(dt.Rows[i]["NVALUE2"].ToString().Trim())).ToString();
                }
                if (dt.Rows[i]["OPTIONGB"].ToString().Trim() == "SetFormButton")
                {
                    SetButton = (VB.Val(dt.Rows[i]["NVALUE1"].ToString().Trim())).ToString();
                }
            }
            dt.Dispose();
            dt = null;

            Control[] controls = GetAllControls(pForm);
            foreach (Control ctl in controls)
            {
                if (ctl is Button)
                {
                    if (SetButton == "1")
                    {
                        ((Button)ctl).FlatStyle = FlatStyle.System;
                    }
                    else
                    {
                        ((Button)ctl).FlatStyle = FlatStyle.Flat;
                        ((Button)ctl).ForeColor = Color.Black;
                        ((Button)ctl).FlatAppearance.BorderSize = 1;
                        ((Button)ctl).FlatAppearance.BorderColor = Color.Black;
                    }
                }
                if (ctl is FarPoint.Win.Spread.FpSpread)
                {
                    if (SetSpread == "1")
                    {
                        SetSpreadSkinOffice((FarPoint.Win.Spread.FpSpread)ctl);
                        SetSpreadSheetSkinColor(((FarPoint.Win.Spread.FpSpread)ctl).Sheets[0], Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(236)))), ((int)(((byte)(247))))), Color.Black);
                    }
                    else
                    {
                        SetSpreadSkinDefault((FarPoint.Win.Spread.FpSpread)ctl);
                        if (SetSpreadColor == "0")
                        {
                            SetSpreadSheetSkinColor(((FarPoint.Win.Spread.FpSpread)ctl).Sheets[0], Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237))))), Color.Black);  //Color.White
                        }
                        else
                        {
                            SetSpreadSheetSkinColor(((FarPoint.Win.Spread.FpSpread)ctl).Sheets[0], ColorTranslator.FromWin32((int)VB.Val(SetSpreadColor)), Color.Black);
                        }
                    }
                }
                if (ctl is Label)
                {
                    if (VB.Left(((Label)ctl).Name.Trim(), 8) == "lblTitle")
                    {
                        if (SetlblTitle == "0")
                        {
                            ((Label)ctl).ForeColor = Color.Black;
                        }
                        else
                        {
                            ((Label)ctl).ForeColor = ColorTranslator.FromWin32((int)VB.Val(SetlblTitleColor));
                        }
                    }
                    if (VB.Left(((Label)ctl).Name.Trim(), 11) == "lblTitleSub")
                    {
                        if (SetlblTitleSub == "0")
                        {
                            ((Label)ctl).ForeColor = Color.White;
                        }
                        else
                        {
                            ((Label)ctl).ForeColor = ColorTranslator.FromWin32((int)VB.Val(SetlblTitleSubColor));
                        }
                    }
                }
                if (ctl is Panel)
                {
                    if (VB.Left(((Panel)ctl).Name.Trim(), 8) == "panTitle")
                    {
                        if (SetpanTitle == "0")
                        {
                            ((Panel)ctl).BackColor = Color.White;
                        }
                        else
                        {
                            ((Panel)ctl).BackColor = ColorTranslator.FromWin32((int)VB.Val(SetpanTitleColor));
                        }
                    }
                    if (VB.Left(((Panel)ctl).Name.Trim(), 11) == "panTitleSub")
                    {
                        if (SetpanTitleSub == "0")
                        {
                            ((Panel)ctl).BackColor = Color.RoyalBlue;
                        }
                        else
                        {
                            ((Panel)ctl).BackColor = ColorTranslator.FromWin32((int)VB.Val(SetpanTitleSubColor));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 폼 컨트롤의 스킨 설정 : 기본
        /// </summary>
        /// <param name="pForm"></param>
        public static void SetFormSkinDefault(Form pForm)
        {
            Control[] controls = GetAllControls(pForm);
            foreach (Control ctl in controls)
            {
                if (ctl is Button)
                {
                    ((Button)ctl).FlatStyle = FlatStyle.Flat;
                    ((Button)ctl).BackColor = Color.WhiteSmoke;
                    ((Button)ctl).ForeColor = Color.Black;
                    ((Button)ctl).FlatAppearance.BorderSize = 1;
                    ((Button)ctl).FlatAppearance.BorderColor = Color.Black;
                }
                if (ctl is FarPoint.Win.Spread.FpSpread)
                {
                    SetSpreadSkinDefault((FarPoint.Win.Spread.FpSpread)ctl);
                    SetSpreadSheetSkinColor(((FarPoint.Win.Spread.FpSpread)ctl).Sheets[0], Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237))))), Color.Black);
                    //Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))))
                }
                if (ctl is Label)
                {
                    if (VB.Left(((Label)ctl).Name.Trim(), 8) == "lblTitle")
                    {
                        ((Label)ctl).ForeColor = Color.Black;
                    }
                    if (VB.Left(((Label)ctl).Name.Trim(), 11) == "lblTitleSub")
                    {
                        ((Label)ctl).ForeColor = Color.White;
                    }
                }
                if (ctl is Panel)
                {
                    if (VB.Left(((Panel)ctl).Name.Trim(), 8) == "panTitle")
                    {
                        ((Panel)ctl).BackColor = Color.White;
                    }
                    if (VB.Left(((Panel)ctl).Name.Trim(), 11) == "panTitleSub")
                    {
                        ((Panel)ctl).BackColor = Color.RoyalBlue;
                    }
                }
            }
        }

        /// <summary>
        /// 폼 컨트롤의 스킨 설정 : 3D
        /// </summary>
        /// <param name="pForm"></param>
        public static void SetFormSkin3D(Form pForm)
        {
            Control[] controls = GetAllControls(pForm);
            foreach (Control ctl in controls)
            {
                if (ctl is Button)
                {
                    ((Button)ctl).FlatStyle = FlatStyle.Standard;
                    ((Button)ctl).ForeColor = Color.Black;
                    ((Button)ctl).BackColor = Color.Transparent;
                }
                if (ctl is FarPoint.Win.Spread.FpSpread)
                {
                    SetSpreadSkinOffice((FarPoint.Win.Spread.FpSpread)ctl);
                    SetSpreadSheetSkinColor(((FarPoint.Win.Spread.FpSpread)ctl).Sheets[0], Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(236)))), ((int)(((byte)(247))))), Color.Black);
                }
                if (ctl is Label)
                {
                    if (VB.Left(((Label)ctl).Name.Trim(), 8) == "lblTitle")
                    {
                        ((Label)ctl).ForeColor = Color.Black;
                    }
                    if (VB.Left(((Label)ctl).Name.Trim(), 11) == "lblTitleSub")
                    {
                        ((Label)ctl).ForeColor = Color.White;
                    }
                }
                if (ctl is Panel)
                {
                    if (VB.Left(((Panel)ctl).Name.Trim(), 8) == "panTitle")
                    {
                        ((Panel)ctl).BackColor = Color.White;
                    }
                    if (VB.Left(((Panel)ctl).Name.Trim(), 11) == "panTitleSub")
                    {
                        ((Panel)ctl).BackColor = Color.RoyalBlue;
                    }
                }
            }
        }

        /// <summary>
        /// Spread Skin : Default
        /// </summary>
        /// <param name="Spread"></param>
        public static void SetSpreadSkinDefault(FarPoint.Win.Spread.FpSpread Spread)
        {
            Spread.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Default;

            //FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer enhancedRowHeaderRenderer1 = new FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer();
            //// 
            //// SetSpread
            //// 
            ////Spread.AccessibleDescription = "SetSpread, Sheet1, Row 0, Column 0, ";
            ////Spread.Dock = System.Windows.Forms.DockStyle.Fill;
            ////Spread.Location = new System.Drawing.Point(0, 102);
            ////Spread.Name = "SetSpread";
            ////Spread.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            ////Spread.Sheets[0]});
            ////Spread.Size = new System.Drawing.Size(520, 397);
            ////Spread.TabIndex = 111;
            //// 
            //// SetSpread_Sheet1
            //// 
            ////Spread.Sheets[0].Reset();
            ////Spread.Sheets[0].SheetName = "Sheet1";
            //// Formulas and custom names must be loaded with R1C1 reference style
            //Spread.Sheets[0].ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            //Spread.Sheets[0].FilterBarHeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(236)))), ((int)(((byte)(247)))));
            //Spread.Sheets[0].FilterBarHeaderStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            //Spread.Sheets[0].FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //Spread.Sheets[0].FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            //Spread.Sheets[0].FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            //enhancedRowHeaderRenderer1.BackColor = System.Drawing.SystemColors.Control;
            //enhancedRowHeaderRenderer1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            //enhancedRowHeaderRenderer1.ForeColor = System.Drawing.SystemColors.ControlText;
            //enhancedRowHeaderRenderer1.Name = "";
            //enhancedRowHeaderRenderer1.PictureZoomEffect = false;
            //enhancedRowHeaderRenderer1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            //enhancedRowHeaderRenderer1.TextRotationAngle = 0D;
            //enhancedRowHeaderRenderer1.ZoomFactor = 1F;
            //Spread.Sheets[0].FilterBarHeaderStyle.Renderer = enhancedRowHeaderRenderer1;
            //Spread.Sheets[0].FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //Spread.Sheets[0].FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            //Spread.Sheets[0].RowHeader.Columns.Default.Resizable = false;
            //Spread.Sheets[0].SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            //Spread.Sheets[0].SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            //Spread.Sheets[0].SheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            //Spread.Sheets[0].SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            //Spread.Sheets[0].ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
        }

        /// <summary>
        /// Spread Skin : Office 2013
        /// </summary>
        /// <param name="Spread"></param>
        public static void SetSpreadSkinOffice(FarPoint.Win.Spread.FpSpread Spread)
        {
            Spread.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;
            //FarPoint.Win.Spread.EnhancedFocusIndicatorRenderer enhancedFocusIndicatorRenderer1 = new FarPoint.Win.Spread.EnhancedFocusIndicatorRenderer();
            //FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer1 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            //FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer2 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            //FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer enhancedRowHeaderRenderer1 = new FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer();
            //// 
            //// SetSpread
            //// 
            ////Spread.AccessibleDescription = "SetSpread, Sheet1, Row 0, Column 0, ";
            ////Spread.Dock = System.Windows.Forms.DockStyle.Fill;
            //Spread.FocusRenderer = enhancedFocusIndicatorRenderer1;
            //Spread.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            //Spread.HorizontalScrollBar.Name = "";
            //Spread.HorizontalScrollBar.Renderer = enhancedScrollBarRenderer1;
            //Spread.HorizontalScrollBar.TabIndex = 3;
            ////Spread.Location = new System.Drawing.Point(0, 102);
            ////Spread.Name = "SetSpread";
            ////Spread.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            ////Spread.Sheets[0]});
            ////Spread.Size = new System.Drawing.Size(520, 397);
            //Spread.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;
            ////Spread.TabIndex = 111;
            //Spread.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            //Spread.VerticalScrollBar.Name = "";
            //Spread.VerticalScrollBar.Renderer = enhancedScrollBarRenderer2;
            //Spread.VerticalScrollBar.TabIndex = 4;
            //// 
            //// Spread.Sheet[0]
            //// 
            //Spread.Sheets[0].Reset();
            //Spread.Sheets[0].SheetName = "Sheet1";
            //// Formulas and custom names must be loaded with R1C1 reference style
            //Spread.Sheets[0].ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            //Spread.Sheets[0].ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            //Spread.Sheets[0].ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            //Spread.Sheets[0].ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            //Spread.Sheets[0].ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            //Spread.Sheets[0].ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            //Spread.Sheets[0].ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            //Spread.Sheets[0].ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            //Spread.Sheets[0].ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            //Spread.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            //Spread.Sheets[0].ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            //Spread.Sheets[0].ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            //Spread.Sheets[0].ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            //Spread.Sheets[0].FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            //Spread.Sheets[0].FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            //Spread.Sheets[0].FilterBar.DefaultStyle.Parent = "FilterBarEnhanced";
            //Spread.Sheets[0].FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            //Spread.Sheets[0].FilterBarHeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(236)))), ((int)(((byte)(247)))));
            //Spread.Sheets[0].FilterBarHeaderStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            //Spread.Sheets[0].FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //Spread.Sheets[0].FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            //enhancedRowHeaderRenderer1.Name = "";
            //enhancedRowHeaderRenderer1.PictureZoomEffect = false;
            //enhancedRowHeaderRenderer1.TextRotationAngle = 0D;
            //enhancedRowHeaderRenderer1.ZoomFactor = 1F;
            //Spread.Sheets[0].FilterBarHeaderStyle.Renderer = enhancedRowHeaderRenderer1;
            //Spread.Sheets[0].FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //Spread.Sheets[0].FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            //Spread.Sheets[0].RowHeader.Columns.Default.Resizable = false;
            //Spread.Sheets[0].RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            //Spread.Sheets[0].RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            //Spread.Sheets[0].RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            //Spread.Sheets[0].RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            //Spread.Sheets[0].ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
        }

        /// <summary>
        /// Spread Sheet Skin : Default
        /// </summary>
        /// <param name="SpdSheet"></param>
        public static void SetSpreadSheetSkinDefault(FarPoint.Win.Spread.SheetView SpdSheet)
        {
            //FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer enhancedRowHeaderRenderer1 = new FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer();
            SpdSheet.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Default;
        }

        /// <summary>
        /// Spread Sheet Skin : Office 2013
        /// </summary>
        /// <param name="SpdSheet"></param>
        public static void SetSpreadSheetSkinOffice(FarPoint.Win.Spread.SheetView SpdSheet)
        {
            //FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer enhancedRowHeaderRenderer1 = new FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer();
            SpdSheet.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Default;
            SpdSheet.GrayAreaBackColor = Color.White;
        }

        /// <summary>
        /// 스프래드 해드 색을 칠한다
        /// </summary>
        /// <param name="SpdSheet"></param>
        /// <param name="pColor"></param>
        public static void SetSpreadSheetSkinColor(FarPoint.Win.Spread.SheetView SpdSheet, Color pColor, Color pFontColor)
        {
            SpdSheet.GrayAreaBackColor = Color.White;
            for (int i = 0; i < SpdSheet.Columns.Count; i++)
            {
                for (int j = 0; j < SpdSheet.ColumnHeader.RowCount; j++)
                {
                    SpdSheet.ColumnHeader.Cells[j, i].BackColor = pColor;
                    SpdSheet.ColumnHeader.Cells[j, i].ForeColor = pFontColor;
                }
            }
            for (int i = 0; i < SpdSheet.Rows.Count; i++)
            {
                for (int j = 0; j < SpdSheet.RowHeader.ColumnCount; j++)
                {
                    SpdSheet.RowHeader.Cells[i, j].BackColor = pColor;
                    SpdSheet.RowHeader.Cells[i, j].ForeColor = pFontColor;
                }
            }
        }

        /// <summary>
        /// 문자열에 0을 채운다
        /// </summary>
        /// <param name="strNumber"></param>
        /// <param name="intAllCnt"></param>
        /// <returns></returns>
        public static string SetAutoZero(string strNumber, int intAllCnt)
        {
            string rtnVal = "";
            if (VB.IsNumeric(strNumber))
            {
                rtnVal = strNumber.PadLeft(intAllCnt, '0');
            }
            else
            {
                rtnVal = strNumber;
            }
            return rtnVal;
        }


        /// <summary>
        /// 문자열을 strFillString로 채운다
        /// </summary>
        /// <param name="strString"></param>
        /// <param name="strLR"></param>
        /// <param name="intAllCnt"></param>
        /// <param name="strFillString"></param>
        /// <returns></returns>
        public static string SetFillString(string strString, string strLR, int intAllCnt, string strFillString)
        {
            string rtnVal = "";
            int intLen = 0;
            int i = 0;

            intLen = VB.Len(strString);

            for (i = 0; i < intAllCnt - intLen; i++)
            {
                rtnVal = rtnVal + strFillString;
            }

            if (strLR == "R")
            {
                rtnVal = strString + rtnVal;
            }
            else
            {
                rtnVal = rtnVal + strString;
            }
            return rtnVal;
        }

        /// <summary>
        /// VB의 Format함수 대용
        /// </summary>
        /// <param name="strChr"></param>
        /// <param name="strFormat"></param>
        /// <returns></returns>
        public static string vbFormat(string strChr, string strFormat)
        {
            string rtnVal = "";
            if (strChr.Trim() == "")
            {
                return rtnVal;
            }
            rtnVal = VB.Val(strChr).ToString(strFormat);
            return rtnVal;
        }
        /// <summary>
        /// 문자를 날짜 형식으로 변환한다
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="strType"></param>
        /// <returns></returns>
        public static string FormatStrToDate(string strDate, string strType)
        {
            string rtnVal = null;
            rtnVal = "";
            if (VB.Len(strDate) == 0 | string.IsNullOrEmpty(VB.Trim(strDate)))
            {
                return rtnVal;
            }
            switch (strType)
            {
                case "D":
                    rtnVal = VB.Val(strDate).ToString("####-##-##");
                    break;
                case "DS":
                    rtnVal = VB.Mid(strDate, 3, 2) + "/" + VB.Mid(strDate, 5, 2) + "/" + VB.Mid(strDate, 7, 2);
                    break;
                case "DK":
                    rtnVal = VB.Mid(strDate, 1, 4) + "년 " + VB.Mid(strDate, 5, 2) + "월 " + VB.Mid(strDate, 7, 2) + "일";
                    break;
                case "DM":
                    rtnVal = VB.Val(VB.Left(strDate, 8)).ToString("####-##-##") + " " + VB.Left(VB.Right(strDate, 6), 2) + ":" + VB.Mid(VB.Right(strDate, 6), 3, 2);
                    break;
                case "A":
                    rtnVal = VB.Val(VB.Left(strDate, 8)).ToString("####-##-##") + " " + VB.Left(VB.Right(strDate, 6), 2) + ":" + VB.Mid(VB.Right(strDate, 6), 3, 2) + ":" + VB.Mid(VB.Right(strDate, 6), 5, 2);
                    break;
                case "T":
                    if (VB.Len(strDate) < 6)
                    {
                        strDate = strDate.PadRight(6, '0');
                    }
                    rtnVal = VB.Left(strDate, 2) + ":" + VB.Mid(strDate, 3, 2) + ":" + VB.Right(strDate, 2);
                    break;
                case "M":
                    strDate = VB.Replace(strDate, ":", "");
                    rtnVal = VB.Left(strDate, 2) + ":" + VB.Mid(strDate, 3, 2);
                    break;
                case "MK":
                    strDate = VB.Replace(strDate, ":", "");
                    rtnVal = VB.Left(strDate, 2) + "시" + VB.Mid(strDate, 3, 2) + "분";
                    break;
                case "YM":
                    rtnVal = VB.Val(strDate).ToString("####-##");
                    break;
                default:
                    return rtnVal;
            }
            return rtnVal;
        }

        /// <summary>
        /// 문자를 날짜 형식으로 변환한다
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="strType"></param>
        /// <param name="strSplit"></param>
        /// <returns></returns>
        public static string FormatStrToDateEx(string strDate, string strType, string strSplit)
        {
            string rtnVal = null;
            rtnVal = "";
            if (VB.Len(strDate) == 0 | string.IsNullOrEmpty(VB.Trim(strDate)))
            {
                return rtnVal;
            }
            switch (strType)
            {
                case "D":
                    rtnVal = VB.Left(strDate, 4) + strSplit + VB.Mid(strDate, 5, 2) + strSplit + VB.Mid(strDate, 7, 2);
                    break;
                case "A":
                    rtnVal = VB.Left(strDate, 4) + strSplit + VB.Mid(strDate, 5, 2) + strSplit + VB.Mid(strDate, 7, 2) + " " + VB.Left(VB.Right(strDate, 6), 2) + ":" + VB.Mid(VB.Right(strDate, 6), 3, 2) + ":" + VB.Mid(VB.Right(strDate, 6), 5, 2);
                    break;
                case "T":
                    if (VB.Len(strDate) < 6)
                    {
                        strDate = strDate.PadRight(6, '0');
                    }
                    rtnVal = VB.Left(strDate, 2) + strSplit + VB.Mid(strDate, 3, 2) + strSplit + VB.Right(strDate, 2);
                    break;
                case "M":
                    strDate = VB.Replace(strDate, strSplit, "");
                    rtnVal = VB.Left(strDate, 2) + strSplit + VB.Mid(strDate, 3, 2);
                    break;
                case "YM":
                    rtnVal = VB.Left(strDate, 4) + strSplit + VB.Mid(strDate, 5, 2);
                    break;
                default:
                    return rtnVal;
            }
            return rtnVal;
        }

        /// <summary>
        /// 문자를 날짜 형식으로 변환한다 : Oracle용
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="strType"></param>
        /// <returns></returns>
        public static string FormatStrToDateTime(string strDate, string strType)
        {
            string rtnVal = "";
            if (VB.Len(strDate) == 0 | string.IsNullOrEmpty(VB.Trim(strDate)))
            {
                return rtnVal;
            }

            try
            {
                switch (strType)
                {
                    case "D":
                        rtnVal = VB.Format(Convert.ToDateTime(strDate), "yyyy-MM-dd");
                        break;
                    case "DS":
                        rtnVal = VB.Mid(strDate, 3, 2) + "/" + VB.Mid(strDate, 5, 2) + "/" + VB.Mid(strDate, 7, 2);
                        break;
                    case "DK":
                        rtnVal = VB.Format(Convert.ToDateTime(strDate), "yyyy년 MM월 dd일");
                        break;
                    case "A":
                        rtnVal = VB.Format(Convert.ToDateTime(strDate), "yyyy-MM-dd hh:mm:ss");
                        break;
                    case "T":
                        if (VB.Len(strDate) < 6)
                        {
                            strDate = strDate.PadRight(6, '0');
                        }
                        rtnVal = VB.Left(strDate, 2) + ":" + VB.Mid(strDate, 3, 2) + ":" + VB.Right(strDate, 2);
                        break;
                    case "M":
                        strDate = VB.Replace(strDate, ":", "");
                        rtnVal = VB.Left(strDate, 2) + ":" + VB.Mid(strDate, 3, 2);
                        break;
                    case "MK":
                        strDate = VB.Replace(strDate, ":", "");
                        rtnVal = VB.Left(strDate, 2) + "시" + VB.Mid(strDate, 3, 2) + "분";
                        break;
                    case "YM":
                        rtnVal = VB.Format(Convert.ToDateTime(strDate), "yyyy-MM");
                        break;
                    default:
                        return rtnVal;
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// center it on the screen
        /// </summary>
        /// <param name="FormName"></param>
        public static void Form_Center(Form FormName)
        {

            FormName.Top = (Screen.PrimaryScreen.Bounds.Height - FormName.Height - 400) / 2;
            FormName.Left = (Screen.PrimaryScreen.Bounds.Height - FormName.Width) / 2;

        }

        /// <summary>
        /// 날짜 형식으로 변환한다
        /// </summary>
        /// <param name="xDate"></param>
        /// <param name="sFlag"></param>
        /// <returns></returns>
        public static string ConStrDate(string xDate, string sFlag)
        {
            string strDate = null;
            try
            {
                strDate = VB.Replace(xDate, "-", "");
                strDate = VB.Replace(strDate, ".", "");
                strDate = VB.Replace(strDate, "/", "");

                strDate = VB.Left(strDate, 4) + sFlag + VB.Mid(strDate, 5, 2) + sFlag + VB.Right(strDate, 2);
                return strDate;
            }
            catch (Exception ex)
            {
                return xDate;
                //ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// 소숫점 아래를 없앤다
        /// </summary>
        /// <param name="dblNum"></param>
        /// <returns></returns>
        public static double TruncToDbl(double dblNum)
        {
            //Math.Ceiling(dblNum);// 올림
            //Math.Round(dblNum); // 반올림
            //Math.Truncate(dblNum);// 버림
            double rtnVal = 0;

            rtnVal = Math.Truncate(dblNum);

            return rtnVal;
        }

        /// <summary>
        /// 소숫점 아래를 없앤다
        /// </summary>
        /// <param name="dblNum"></param>
        /// <returns></returns>
        public static string TruncToString(double dblNum)
        {
            string rtnVal = "0";

            rtnVal = Convert.ToString(Math.Truncate(dblNum));

            return rtnVal;
        }

        /// <summary>
        /// 소숫점 아래 0 을 제거한다.
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string RmZero(string strValue)
        {
            int i = 0;
            int j = 0;
            int intLen = 0;
            string strTmp = "";
            string strTmpVal = "";

            if (VB.IsNumeric(strValue) == false)
            {
                //ComFunc.MsgBox(""222");  
                return strValue;
            }
            intLen = strValue.Length;

            if (VB.InStr(strValue, ".") == 0)
            {
                return strValue;
            }

            for (i = 0; i < intLen; i++)
            {
                //strTmpVal = VB.Mid(strValue, intLen - j, 1);
                strTmpVal = VB.Mid(strValue, intLen - i, 1);
                if (strTmpVal == "0")
                {
                    j = j + 1;
                }
                else
                {
                    if (strTmpVal == ".")
                    {
                        j = j + 1;
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (j == 0)
            {
                return strValue;
            }

            strTmp = VB.Left(strValue, intLen - j);
            return strTmp;
        }
        /// <summary>
        /// 소숫점 아래 자리수를 정한다.(소숫점 4자리까지만)
        /// </summary>
        /// <param name="dblNum">입력변수</param>
        /// <param name="intQut">자릿수</param>
        /// <param name="intFlag">0:버림, 1:올림, 2:버림</param>
        /// <returns></returns>
        public static double SetQutDtD(double dblNum, int intQut, int intFlag)
        {
            double rtnVal = 0;
            int i = 0;
            string strQutNum = "0.";
            string strQutFormat = "#.";
            if (dblNum == 0)
            {
                return rtnVal;
            }
            for (i = 1; i <= intQut; i++)
            {
                strQutNum = strQutNum + "0";
                strQutFormat = strQutFormat + "0";
            }
            strQutNum = strQutNum + "5";

            switch (intFlag)
            {
                case 0:
                    dblNum = Convert.ToDouble(VB.Format(dblNum - Convert.ToDouble(strQutNum), strQutFormat));
                    break;
                case 1:
                    dblNum = Convert.ToDouble(VB.Format(dblNum - Convert.ToDouble(strQutNum), strQutFormat));
                    break;
                case 2:
                    dblNum = Convert.ToDouble(VB.Format(dblNum - Convert.ToDouble(strQutNum), strQutFormat));
                    break;
            }

            rtnVal = dblNum;

            return rtnVal;
        }

        /// <summary>
        /// 소숫점 아래 자리수를 정한다.(소숫점 4자리까지만)
        /// </summary>
        /// <param name="dblNum">입력변수</param>
        /// <param name="intQut">자릿수</param>
        /// <param name="intFlag">0:버림, 1:올림, 2:버림</param>
        /// <returns></returns>
        public static string SetQutStS(string strNum, int intQut, int intFlag)
        {
            string rtnVal = "0";
            int i = 0;
            string strQutNum = "0.";
            string strQutFormat = "#.";

            if (VB.IsNumeric(strNum) == false)
            {
                return strNum;
            }

            double dblNum = VB.Val(strNum);

            if (dblNum == 0)
            {
                return rtnVal;
            }

            for (i = 1; i <= intQut; i++)
            {
                strQutNum = strQutNum + "0";
                strQutFormat = strQutFormat + "0";
            }
            strQutNum = strQutNum + "5";

            switch (intFlag)
            {
                case 0:
                    dblNum = Convert.ToDouble(VB.Format(dblNum - Convert.ToDouble(strQutNum), strQutFormat));
                    break;
                case 1:
                    dblNum = Convert.ToDouble(VB.Format(dblNum - Convert.ToDouble(strQutNum), strQutFormat));
                    break;
                case 2:
                    dblNum = Convert.ToDouble(VB.Format(dblNum - Convert.ToDouble(strQutNum), strQutFormat));
                    break;
            }

            rtnVal = Convert.ToString(dblNum);

            return rtnVal;
        }

        /// <summary>
        /// 텍스트박스 반전
        /// </summary>
        /// <param name="txtObj"></param>
        public static void StartLen(System.Windows.Forms.TextBox txtObj)
        {
            txtObj.SelectionStart = 0;
            txtObj.SelectionLength = VB.Len(txtObj.Text);
        }

        /// <summary>
        /// 한글 2BYTE로 계산해서 문자개수 리턴
        /// </summary>
        /// <param name="strExp"></param>
        /// <returns></returns>
        public static long GetWordByByte(string strExp)
        {
            System.Text.Encoding Enchar = null;
            Enchar = System.Text.Encoding.GetEncoding("ks_c_5601-1987");

            byte[] buf = Enchar.GetBytes(strExp);

            return buf.Length;
        }

        /// <summary>
        /// BYTE용 MID 함수
        /// </summary>
        /// <param name="strExp"></param>
        /// <param name="intLR"></param>
        /// <param name="intLen"></param>
        /// <returns></returns>
        public static string GetMidStr(string strExp, int intLR, int intLen)
        {
            System.Text.Encoding Enchar = null;
            Enchar = System.Text.Encoding.GetEncoding("ks_c_5601-1987");
            byte[] buf = Enchar.GetBytes(strExp);

            ////Right
            //if (intLR > 0)
            //{
            //    intLR = buf.Length - intLen;
            //}

            return Enchar.GetString(buf, intLR, intLen);
        }

        /// <summary>
        /// BYTE 형태 문자열 처리
        /// intLen길이 만큼 strconv로 오른쪽 채우기
        /// </summary>
        /// <param name="strExp"></param>
        /// <param name="intLen"></param>
        /// <param name="strConv"></param>
        /// <returns></returns>
        public static string RPAD(string strExp, int intLen, string strConv)
        {
            string strConvRet = "";
            int i = 0;

            for (i = 1; i <= intLen - GetWordByByte(strExp); i++)
            {
                strConvRet = strConvRet + strConv;
            }
            return strExp + strConvRet;
        }

        /// <summary>
        /// intLen길이 만큼 strconv로 왼쪽 채우기
        /// </summary>
        /// <param name="strExp"></param>
        /// <param name="intLen"></param>
        /// <param name="strConv"></param>
        /// <returns></returns>
        public static string LPAD(string strExp, int intLen, string strConv)
        {
            string strConvRet = "";
            int i = 0;

            for (i = 1; i <= intLen - GetWordByByte(strExp); i++)
            {
                strConvRet = strConvRet + strConv;
            }
            return strConvRet + strExp;
        }

        /// <summary>
        /// BYTE용 MID 함수
        /// </summary>
        /// <param name="strExp">문자열</param>
        /// <param name="intLR">인덱스 시작은 1부터</param>
        /// <param name="intLen">바이트수</param>
        /// <returns></returns>
        public static string MidH(string strExp, int intLR, int intLen)
        {
            string rtnVal = "";

            if (strExp == "") return rtnVal;
            if (intLR > 0)
            {
                intLR = intLR - 1;
            }

            try
            {
                System.Text.Encoding Enchar = null;
                Enchar = System.Text.Encoding.GetEncoding("EUC-KR");
                byte[] buf = Enchar.GetBytes(strExp);

                if (buf.Length < intLR + intLen)
                {
                    rtnVal = Enchar.GetString(buf, intLR, buf.Length - intLR);
                }
                else
                {
                    rtnVal = Enchar.GetString(buf, intLR, intLen);
                }

                return rtnVal;
            }
            catch (Exception e)
            {
                return rtnVal;
            }
        }

        //---------------( 한글용 문자열 처리 함수들 )--------------//

        /// <summary>
        /// 2Bite 문자열 길이
        /// </summary>
        /// <param name="strString"></param>
        /// <returns></returns>
        public static long LenH(string strString)
        {
            if (strString != null)
            {
                return GetWordByByte(strString);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 2Bite Left
        /// </summary>
        /// <param name="strString"></param>
        /// <param name="lngLength"></param>
        /// <returns></returns>
        public static string LeftH(string strString, int lngLength)
        {
            return MidH(strString, 0, lngLength);
        }

        /// <summary>
        /// 2Bite Right
        /// </summary>
        /// <param name="strString"></param>
        /// <param name="lngLength"></param>
        /// <returns></returns>
        /// ** 절대 사용 금지 에러남 **
        /// ** 절대 사용 금지 에러남 **
        /// ** 절대 사용 금지 에러남 **
        public static string RightH(string strString, int lngLength)
        {
            //절대 사용 금지 에러남
            return MidH(strString, 1, lngLength);
        }

        /// <summary>
        /// 컨트롤에 포함된 모든 컨트롤를 반환한다. GetAllControls와 동일
        /// </summary>
        /// <param name="containerControl"></param>
        /// <returns></returns>
        public static Control[] GetAllControlsUsingRecursive(Control containerControl)
        {
            List<Control> allControls = new List<Control>();

            foreach (Control control in containerControl.Controls)
            {
                allControls.Add(control);
                if (control.Controls.Count > 0)
                {
                    allControls.AddRange(GetAllControlsUsingRecursive(control));
                }
            }
            return allControls.ToArray();
        }

        /// <summary>
        /// 컨트롤에 포함된 모든 컨트롤를 반환한다. GetAllControlsUsingRecursive와 동일
        /// </summary>
        /// <param name="containerControl"></param>
        /// <returns></returns>
        public static Control[] GetAllControls(Control containerControl)
        {
            List<Control> allControls = new List<Control>();

            Queue<Control.ControlCollection> queue = new Queue<Control.ControlCollection>();
            queue.Enqueue(containerControl.Controls);

            while (queue.Count > 0)
            {
                Control.ControlCollection controls
                            = (Control.ControlCollection)queue.Dequeue();
                if (controls == null || controls.Count == 0)
                    continue;

                foreach (Control control in controls)
                {
                    allControls.Add(control);
                    queue.Enqueue(control.Controls);
                }
            }

            return allControls.ToArray();
        }

        /// <summary>
        /// 폼안에 컨트롤을 찾아서 Visible 속성을 변경한다
        /// </summary>
        /// <param name="objParent"></param>
        /// <param name="strName"></param>
        /// <param name="bolVisible"></param>
        /// <param name="bolDtpCheckedDefault"></param>
        public static void SetControlVisible(Control objParent, string strName, bool bolVisible, string strText, bool bolDtpCheckedDefault = true)
        {
            Control[] controls = GetAllControls(objParent);

            foreach (Control control in controls)
            {
                if (control.Name == strName)
                {
                    control.Visible = bolVisible;
                    if (strText != "")
                    {
                        control.Text = strText;
                    }
                }
            }
        }

        /// <summary>
        /// 폼 컨트롤에 색을 칠한다.
        /// </summary>
        /// <param name="objParent"></param>
        /// <param name="strName"></param>
        /// <param name="strFlag"></param>
        /// <param name="cColor"></param>
        public static void SetControlColor(Control objParent, string strName, string strFlag, Color cColor)
        {
            Control[] controls = GetAllControls(objParent);

            foreach (Control control in controls)
            {
                if (control.Name == strName)
                {
                    if (strFlag == "ForeColor")
                    {
                        control.ForeColor = cColor;
                    }
                    else
                    {
                        control.BackColor = cColor;
                    }
                }
            }
        }


        /// <summary>
        /// 폼안에 TextBox컨트롤을 찾아서 값을 입력한다.
        /// </summary>
        /// <param name="objParent"></param>
        /// <param name="strConName"></param>
        /// <param name="strText"></param>
        /// <param name="bolDtpCheckedDefault"></param>
        public static void SetTextToValue(Control objParent, string strConName, string strText, bool bolDtpCheckedDefault = true)
        {
            Control[] controls = GetAllControls(objParent);

            foreach (Control control in controls)
            {
                if (control.Name.ToUpper() == strConName.ToUpper())
                {
                    control.Text = strText;
                }
            }
        }

        /// <summary>
        /// 호출하는 컨트롤의 하부 컨트롤을 초기화 한다
        /// </summary>
        /// <param name="objParent">컨테이너</param>
        /// <param name="bolDtpCheckedDefault">true</param>
        public static void SetAllControlClear(Control objParent, bool bolDtpCheckedDefault = true)
        {
            Control[] controls = GetAllControls(objParent);

            foreach (Control ctl in controls)
            {
                if (ctl is TextBox)
                {
                    ctl.Text = "";
                }
                else if (ctl is CheckBox)
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl is ComboBox)
                {
                    ((ComboBox)ctl).SelectedIndex = -1;
                }
                else if (ctl is DateTimePicker)
                {
                    ((DateTimePicker)ctl).Value = VB.Now();
                    ((DateTimePicker)ctl).Checked = bolDtpCheckedDefault;
                }
                else if (ctl is FarPoint.Win.Spread.FpSpread)
                {
                    ((FarPoint.Win.Spread.FpSpread)ctl).ActiveSheet.RowCount = 0;
                }
            }
        }

        /// <summary>
        /// 호출하는 컨트롤의 하부 컨트롤을 초기화 한다
        /// </summary>
        /// <param name="objParent">컨테이너</param>
        /// <param name="bolDtpCheckedDefault">true</param>
        public static void SetAllControlClearEx(Control objParent, bool bolDtpCheckedDefault = true)
        {
            Control[] controls = GetAllControls(objParent);

            foreach (Control ctl in controls)
            {
                if (ctl is TextBox)
                {
                    ctl.Text = "";
                }
                else if (ctl is CheckBox)
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl is RadioButton)
                {
                    ((RadioButton)ctl).Checked = false;
                }
                else if (ctl is ComboBox)
                {
                    ((ComboBox)ctl).SelectedIndex = -1;
                }
                else if (ctl is DateTimePicker)
                {
                    ((DateTimePicker)ctl).Value = VB.Now();
                    ((DateTimePicker)ctl).Checked = bolDtpCheckedDefault;
                }
                else if (ctl is FarPoint.Win.Spread.FpSpread)
                {
                    ((FarPoint.Win.Spread.FpSpread)ctl).ActiveSheet.RowCount = 0;
                    ((FarPoint.Win.Spread.FpSpread)ctl).DataSource = null;
                }
                else if (ctl is NumericUpDown)
                {
                    ((NumericUpDown)ctl).Value = 0;
                }
            }
        }

        /// <summary>
        /// 콤보박스에 셋팅된 데이터를 Display 해준다.
        /// </summary>
        /// <param name="comboname"></param>
        /// <param name="strLR">L,R,T</param>
        /// <param name="intLen"></param>
        /// <param name="strFind"></param>
        public static void ComboFind(System.Windows.Forms.ComboBox comboname, string strLR, int intLen, string strFind)
        {
            int i;

            if (strFind == "")
            {
                return;
            }

            for (i = 0; i <= comboname.Items.Count - 1; i++)
            {
                if (strLR == "L")
                {
                    if (strFind.Trim() == VB.Left(comboname.Items[i].ToString(), intLen).Trim())
                    {
                        comboname.SelectedIndex = i;
                        break;
                    }

                }
                else if (strLR == "R")
                {
                    if (strFind.Trim() == VB.Right(comboname.Items[i].ToString(), intLen).Trim())
                    {
                        comboname.SelectedIndex = i;
                        break;
                    }

                }
                else
                {
                    if (strFind.Trim() == comboname.Items[i].ToString().Trim())
                    {
                        comboname.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 생일이 날짜 형식인지 체크 한다.
        /// </summary>
        /// <returns></returns>
        public static bool CheckBirthDay(string strBirthDate)
        {
            bool rtnVal = false;
            DateTime dtBirthDay;

            try
            {
                dtBirthDay = Convert.ToDateTime(strBirthDate);
                rtnVal = true;
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        /// <summary>
        /// 만나이계산(주민번호는 13자리)
        /// </summary>
        /// <param name="strJuminNo"></param>
        public static int AgeCalc(PsmhDb pDbCon, string strJuminNo)
        {
            string strBirthDate = "";
            string strSex = "";
            int rtnVal = 0;
            string strCurrentDate = "";

            try
            {
                if (VB.Len(VB.Trim(strJuminNo)) != 13)
                {
                    //20170518 박병규 : 주민번호가 오류인경우 10살로 처리
                    return rtnVal = 10;
                }

                strSex = VB.Mid(strJuminNo, 7, 1);

                switch (strSex)
                {
                    case "1":
                    case "2":
                        strBirthDate = "19";
                        break;

                    case "3":
                    case "4":
                        strBirthDate = "20";
                        break;

                    case "5":
                    case "6":
                        strBirthDate = "19";
                        break;

                    case "7": //외국인 2000년 이후 출생자
                    case "8":
                        strBirthDate = "20";
                        break;

                    case "0":
                    case "9":
                        strBirthDate = "18";
                        break;

                    default:
                        strBirthDate = "19";
                        break;
                }

                strBirthDate = strBirthDate + VB.Left(strJuminNo, 2) + "-" + VB.Mid(strJuminNo, 3, 2) + "-" + VB.Mid(strJuminNo, 5, 2);

                //20170518 박병규 : 나자렛집 주민번호 나이계산(생일이 이상하면 강제로 2월2일로 함)
                if (VB.Mid(strJuminNo, 3, 1) == "5" || VB.Mid(strJuminNo, 3, 1) == "6")
                {
                    rtnVal = 0;

                    strBirthDate = VB.Left(strBirthDate, 4) + "-02-02";
                }

                if (VB.Right(strBirthDate, 5) == "02-30")        //주민번호가 2월 30일일 경우 2월 28일로 변경(10890382환자) 자주 발생할 경우 생일 읽는 부분 추가 예정
                {
                    strBirthDate = VB.Left(strBirthDate, 4) + "-02-28";
                }

                if (CheckBirthDay(strBirthDate) == false)
                {
                    return 0;
                }
                strCurrentDate = VB.Val(ComQuery.CurrentDateTime(pDbCon, "D")).ToString("####-##-##");

                //기준일자가 생년월일보다 적으면 0살로 처리
                if (Convert.ToDateTime(strBirthDate) >= Convert.ToDateTime(strCurrentDate))
                {
                    return rtnVal;
                }

                //if (Convert.ToDateTime(VB.Format(Convert.ToDateTime(strBirthDate), "MM-dd")) > Convert.ToDateTime(VB.Format(Convert.ToDateTime(strCurrentDate), "MM-dd")))
                if (Convert.ToInt32(VB.Right(VB.Replace(strBirthDate, "-", ""), 4)) > Convert.ToInt32(VB.Right(VB.Replace(strCurrentDate, "-", ""), 4)))
                {
                    rtnVal = Convert.ToInt32(VB.DateDiff("yyyy", Convert.ToDateTime(strBirthDate), Convert.ToDateTime(strCurrentDate))) - 1;
                }
                else
                {
                    rtnVal = Convert.ToInt32(VB.DateDiff("yyyy", Convert.ToDateTime(strBirthDate), Convert.ToDateTime(strCurrentDate)));
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }

        }

        /// <summary>
        /// 만나이계산(주민번호는 13자리)
        /// Modify : 2017.08.22 박병규
        /// </summary>
        /// <param name="strJuminNo"></param>
        /// <seealso cref="AGE_YEAR_GESAN"/>
        public static int AgeCalcEx(string strJuminNo, string strCurrentDate)
        {
            string strBirthDate = "";
            string strSex = "";
            int rtnVal = 0;

            try
            {
                if (VB.Len(VB.Trim(strJuminNo)) != 13)
                {
                    //20170518 박병규 : 주민번호가 오류인경우 10살로 처리
                    return rtnVal = 10;
                }

                strSex = VB.Mid(strJuminNo, 7, 1);

                switch (strSex)
                {
                    case "1":
                    case "2":
                        strBirthDate = "19";
                        break;

                    case "3":
                    case "4":
                        strBirthDate = "20";
                        break;

                    case "5":
                    case "6":
                        strBirthDate = "19";
                        break;

                    case "7": //외국인 2000년 이후 출생자
                    case "8":
                        strBirthDate = "20";
                        break;

                    case "0":
                    case "9":
                        strBirthDate = "18";
                        break;

                    default:
                        strBirthDate = "19";
                        break;
                }

                strBirthDate = strBirthDate + VB.Left(strJuminNo, 2) + "-" + VB.Mid(strJuminNo, 3, 2) + "-" + VB.Mid(strJuminNo, 5, 2);

                //20170518 박병규 : 나자렛집 주민번호 나이계산(생일이 이상하면 강제로 2월2일로 함)
                if (VB.Mid(strJuminNo, 3, 1) == "5" || VB.Mid(strJuminNo, 3, 1) == "6")
                {
                    rtnVal = 0;

                    strBirthDate = VB.Left(strBirthDate, 4) + "-02-02";
                }

                if (VB.Right(strBirthDate, 5) == "02-30")        //주민번호가 2월 30일일 경우 2월 28일로 변경(10890382환자) 자주 발생할 경우 생일 읽는 부분 추가 예정
                {
                    strBirthDate = VB.Left(strBirthDate, 4) + "-02-28";
                }

                if (VB.Right(strBirthDate, 5) == "00-00")        //외국인 주민번호이면 01-01셋팅
                {
                    strBirthDate = VB.Left(strBirthDate, 4) + "-01-01";
                }

                if (CheckBirthDay(strBirthDate) == false)
                {
                    return 0;
                }

                if (strCurrentDate.Equals("") || strCurrentDate.Equals(null))
                {
                    strCurrentDate = clsPublic.GstrSysDate;
                }

                //기준일자가 생년월일보다 적으면 0살로 처리
                if (DateTime.Parse(strBirthDate) >= DateTime.Parse(strCurrentDate))
                {
                    return rtnVal;
                }

                //if (Convert.ToDateTime(VB.Format(Convert.ToDateTime(strBirthDate), "MM-dd")) > Convert.ToDateTime(VB.Format(Convert.ToDateTime(strCurrentDate), "MM-dd")))
                if (Convert.ToInt32(VB.Right(VB.Replace(strBirthDate, "-", ""), 4)) > Convert.ToInt32(VB.Right(VB.Replace(strCurrentDate, "-", ""), 4)))
                {
                    rtnVal = Convert.ToInt32(VB.DateDiff("yyyy", Convert.ToDateTime(strBirthDate), Convert.ToDateTime(strCurrentDate))) - 1;
                }
                else
                {
                    rtnVal = Convert.ToInt32(VB.DateDiff("yyyy", Convert.ToDateTime(strBirthDate), Convert.ToDateTime(strCurrentDate)));
                }

                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }

        }

        /// <summary>
        /// 0살인경우 나이, 개월을 소숫점으로 결과값리턴됨.
        /// Author : 박병규
        /// Create Date : 2017.10.23
        /// </summary>
        /// <param name="strJuminNo"></param>
        /// <seealso cref="vbfunc.bas:AGE_YEAR_Gesan2"/>
        /// <seealso cref="vbfunc.bas:AGE_YEAR_Gesan3"/>
        public static double AgeCalcEx_Zero(string strJuminNo, string strCurrentDate)
        {
            DataTable DtFunc = new DataTable();
            string SQL = String.Empty;
            string SqlErr = String.Empty;

            int nYear1 = 0; //기준년도
            string strMMDD1 = ""; //기준월일
            int nYear2 = 0; //출생년도
            string strMMDD2 = ""; //출생월일
            double rtnVal = 0;

            if (strCurrentDate.Length == 10)
            {
                nYear1 = Convert.ToInt32(VB.Left(strCurrentDate, 4));
                strMMDD1 = VB.Mid(strCurrentDate, 6, 2) + VB.Mid(strCurrentDate, 9, 2);
            }
            else
            {
                nYear1 = Convert.ToInt32(VB.Left(clsPublic.GstrSysDate, 4));
                strMMDD1 = VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Mid(clsPublic.GstrSysDate, 9, 2);
            }

            if (strJuminNo != "")
            {
                nYear2 = Convert.ToInt32(VB.Left(strJuminNo, 2));
                switch (VB.Mid(strJuminNo, 7, 1))
                {
                    case "1":
                    case "2":
                        nYear2 += 1900;
                        break;

                    case "3":
                    case "4":
                        nYear2 += 2000;
                        break;

                    case "5":
                    case "6":
                        nYear2 += 1900;
                        break;

                    case "7": //외국인 2000년 이후 출생자
                    case "8":
                        nYear2 += 2000;
                        break;

                    case "0":
                    case "9":
                        nYear2 += 1800;
                        break;

                    default:
                        nYear2 += 1900;
                        break;

                }
            }

            //나이를 계산함(기준년도 - 출생년도)
            rtnVal = nYear1 - nYear2;

            //월일을 비교하여 출생월일이 기준월일보다 크면 나이에서 (-1)처리
            strMMDD2 = VB.Mid(strJuminNo.Trim(), 3, 4);
            if (string.Compare(strMMDD2, strMMDD1) > 0)
                rtnVal = rtnVal - 1;

            //나이가 110살보다 크거나 0보다 적으면 10살로 처리함
            if (rtnVal > 120 || rtnVal < 0)
                rtnVal = 10;

            if (rtnVal == 0)
            {
                string strBirth = nYear2 + "-" + VB.Mid(strJuminNo, 3, 2) + "-" + VB.Mid(strJuminNo, 5, 2);

                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  TO_DATE('" + strCurrentDate + "','YYYY-MM-DD') - ";
                SQL += ComNum.VBLF + "  TO_DATE('" + strBirth + "','YYYY-MM-DD') Gigan";
                SQL += ComNum.VBLF + "FROM DUAL";

                SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                }

                if (DtFunc.Rows.Count == 1)
                {
                    rtnVal = Convert.ToInt32(DtFunc.Rows[0]["Gigan"].ToString().Trim());
                }

                DtFunc.Dispose();
                DtFunc = null;
            }

            return rtnVal;

        }

        /// <summary>
        /// 0살인경우 나이, 개월을 소숫점으로 결과값리턴됨.
        /// Author : 박병규
        /// Create Date : 2018.06.19
        /// </summary>
        /// <param name="strJuminNo"></param>
        /// <seealso cref="vbfunc.bas:AGE_YEAR_Gesan2"/>
        public static double AgeCalcEx_Zero2(string strJuminNo, string strCurrentDate)
        {
            DataTable DtFunc = new DataTable();
            string SQL = String.Empty;
            string SqlErr = String.Empty;

            int nYear1 = 0; //기준년도
            string strMMDD1 = ""; //기준월일
            int nYear2 = 0; //출생년도
            string strMMDD2 = ""; //출생월일
            double rtnVal = 0;

            if (strCurrentDate.Length == 10)
            {
                nYear1 = Convert.ToInt32(VB.Left(strCurrentDate, 4));
                strMMDD1 = VB.Mid(strCurrentDate, 6, 2) + VB.Mid(strCurrentDate, 9, 2);
            }
            else
            {
                nYear1 = Convert.ToInt32(VB.Left(clsPublic.GstrSysDate, 4));
                strMMDD1 = VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Mid(clsPublic.GstrSysDate, 9, 2);
            }

            if (strJuminNo != "")
            {
                nYear2 = Convert.ToInt32(VB.Left(strJuminNo, 2));
                switch (VB.Mid(strJuminNo, 7, 1))
                {
                    case "1":
                    case "2":
                        nYear2 += 1900;
                        break;

                    case "3":
                    case "4":
                        nYear2 += 2000;
                        break;

                    case "5":
                    case "6":
                        nYear2 += 1900;
                        break;

                    case "7": //외국인 2000년 이후 출생자
                    case "8":
                        nYear2 += 2000;
                        break;

                    case "0":
                    case "9":
                        nYear2 += 1800;
                        break;

                    default:
                        nYear2 += 1900;
                        break;

                }
            }

            //나이를 계산함(기준년도 - 출생년도)
            rtnVal = nYear1 - nYear2;

            //월일을 비교하여 출생월일이 기준월일보다 크면 나이에서 (-1)처리
            strMMDD2 = VB.Mid(strJuminNo.Trim(), 3, 4);
            if (string.Compare(strMMDD2, strMMDD1) > 0)
                rtnVal = rtnVal - 1;

            //나이가 110살보다 크거나 0보다 적으면 10살로 처리함
            if (rtnVal > 120 || rtnVal < 0)
                rtnVal = 10;

            if (rtnVal == 0)
            {
                rtnVal = ((VB.Val(VB.Left(strMMDD1, 2)) - VB.Val(VB.Left(strMMDD2, 2))) + ((nYear1 - nYear2) * 12)) / 10;

            }

            return rtnVal;

        }

        /// <summary>
        /// 만나이계산(주민번호는 13자리)
        /// </summary>
        /// <param name="strJuminNo"></param>
        public static string AgeCalcX1(string strJuminNo, string strCurrentDate)
        {
            string strBirthDate = null;
            string strSex = null;
            string rtnVal = "X";

            try
            {
                if (VB.Len(VB.Trim(strJuminNo)) != 13)
                {
                    return rtnVal;
                }

                strSex = VB.Mid(strJuminNo, 7, 1);

                switch (strSex)
                {
                    case "1":
                    case "2":
                        strBirthDate = "19";
                        break;

                    case "3":
                    case "4":
                        strBirthDate = "20";
                        break;

                    case "5":
                    case "6":
                        strBirthDate = "19";
                        break;

                    case "7": //외국인 2000년 이후 출생자
                    case "8":
                        strBirthDate = "20";
                        break;

                    case "0":
                    case "9":
                        strBirthDate = "18";
                        break;

                    default:
                        strBirthDate = "19";
                        break;
                }

                strBirthDate = strBirthDate + VB.Left(strJuminNo, 2) + "-" + VB.Mid(strJuminNo, 3, 2) + "-" + VB.Mid(strJuminNo, 5, 2);

                if (CheckBirthDay(strBirthDate) == false)
                {
                    return "0";
                }

                strCurrentDate = VB.Val(strCurrentDate).ToString("####-##-##");

                if (Convert.ToDateTime(strBirthDate) >= Convert.ToDateTime(strCurrentDate))
                {
                    return rtnVal;
                }

                //if (Convert.ToDateTime(VB.Format(Convert.ToDateTime(strBirthDate), "MM-dd")) > Convert.ToDateTime(VB.Format(Convert.ToDateTime(strCurrentDate), "MM-dd")))
                if (Convert.ToInt32(VB.Right(VB.Replace(strBirthDate, "-", ""), 4)) > Convert.ToInt32(VB.Right(VB.Replace(strCurrentDate, "-", ""), 4)))
                {
                    rtnVal = Convert.ToString(Convert.ToInt32(VB.DateDiff("yyyy", Convert.ToDateTime(strBirthDate), Convert.ToDateTime(strCurrentDate))) - 1);
                }
                else
                {
                    rtnVal = Convert.ToString(VB.DateDiff("yyyy", Convert.ToDateTime(strBirthDate), Convert.ToDateTime(strCurrentDate)));
                }

                if (VB.Val(rtnVal) == 0)
                {
                    rtnVal = Convert.ToString(VB.DateDiff("m", Convert.ToDateTime(strBirthDate), Convert.ToDateTime(strCurrentDate)));
                    rtnVal = rtnVal + "개월";
                }
                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }

        }

        /// <summary>
        /// 성별체크
        /// </summary>
        /// <param name="strJuminNo"></param>
        public static string SexCheck(string strJuminNo, string strSexType)
        {
            string strSex = null;
            string SexCheck = null;

            SexCheck = "";

            if (VB.Len(VB.Trim(strJuminNo)) != 13)
            {
                return SexCheck;
            }

            strSex = VB.Mid(strJuminNo, 7, 1);

            switch (strSex)
            {
                case "1":
                case "3":
                case "5":
                case "7":
                    switch (strSexType)
                    {
                        case "1":
                            SexCheck = "남";
                            break;
                        case "2":
                            SexCheck = "M";
                            break;
                        case "3":
                            SexCheck = "O";
                            break;
                        case "4":
                            SexCheck = "남자";
                            break;
                    }
                    break;
                case "2":
                case "4":
                case "6":
                case "8":
                    switch (strSexType)
                    {
                        case "1":
                            SexCheck = "여";
                            break;
                        case "2":
                            SexCheck = "F";
                            break;
                        case "3":
                            SexCheck = "1";
                            break;
                        case "4":
                            SexCheck = "여자";
                            break;
                    }
                    break;
            }

            return SexCheck;

        }

        /// <summary>
        /// 주민번호로 생일 년월일을 만든다.
        /// </summary>
        /// <param name="ArgJumin1"></param>
        /// <param name="ArgJumin2"></param>
        /// <param name="ArgOpt">Format Option</param>
        /// UPDATE :  2018.01.02 박병규
        /// <seealso cref="vbfunc.BAS:Date_Format_BirthDate"/>
        public static string GetBirthDate(string ArgJumin1, string ArgJumin2, string ArgOpt)
        {
            string strBirthDate = null;
            string retVal = null;

            if (VB.Left(ArgJumin2, 1) == "1" || VB.Left(ArgJumin2, 1) == "2" || VB.Left(ArgJumin2, 1) == "5" || VB.Left(ArgJumin2, 1) == "6")
            {
                strBirthDate = "19";
            }
            else if (VB.Left(ArgJumin2, 1) == "3" || VB.Left(ArgJumin2, 1) == "4" || VB.Left(ArgJumin2, 1) == "7" || VB.Left(ArgJumin2, 1) == "8")
            {
                strBirthDate = "20";
            }
            else if (VB.Left(ArgJumin2, 1) == "0" || VB.Left(ArgJumin2, 1) == "9")
            {
                strBirthDate = "18";
            }

            if (ArgOpt == "")
                retVal = strBirthDate + VB.Mid(ArgJumin1, 1, 2) + VB.Mid(ArgJumin1, 3, 2) + VB.Mid(ArgJumin1, 5, 2);
            else if (ArgOpt == "-")
                retVal = strBirthDate + VB.Mid(ArgJumin1, 1, 2) + "-" + VB.Mid(ArgJumin1, 3, 2) + "-" + VB.Mid(ArgJumin1, 5, 2);

            return retVal;
        }


        /// <summary>
        /// SQL 문에 "'"를 "`"로 변경을 한다.
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string QuotConv(string strText)
        {
            string retVal = VB.Replace(strText, "'", "`");
            return retVal;
        }

        /// <summary>
        /// VB IsNumeric 
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static bool vbNumeric(string strText)
        {
            bool rtnVal = false;

            try
            {
                rtnVal = VB.IsNumeric(strText);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        /// <summary>
        /// 날짜 형식으로 만들어 준다.
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static string ChgDate(string strDate)
        {
            string rtnVal = "";

            strDate = strDate.Trim();

            if (strDate.Trim() == "")
            {
                return rtnVal;
            }

            strDate = VB.Replace(strDate, "-", "");

            if (strDate.Length != 8)
            {
                return rtnVal;
            }

            strDate = ComFunc.FormatStrToDate(strDate, "D");

            DateTime dtDate;

            try
            {
                dtDate = Convert.ToDateTime(strDate);
                rtnVal = strDate;
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        /// <summary>
        /// 한글이 포함되어 있는지 확인(문자열 길이 때문에)
        /// </summary>
        /// <param name="ArgData"></param>
        /// <returns></returns>
        public static bool isHangul(string ArgData)
        {
            bool rtnVal;
            short i;

            rtnVal = false;
            if (ArgData.Length == 0)
            {
                return rtnVal;
            }
            for (i = 0; i < ArgData.Length; i++)
            {
                if (VB.Asc(ArgData.Substring(i, 1)) < 0)
                {
                    rtnVal = true;
                    return rtnVal;
                }
            }
            return rtnVal;
        }

        /// <summary>
        /// 타임 형식이 맞는지 검증한다.
        /// </summary>
        /// <param name="strTime"></param>
        /// <returns></returns>
        public static bool CheckTime(string strTime)
        {
            DateTime dtTime;
            string strDate = "2012-01-01 ";

            try
            {
                dtTime = Convert.ToDateTime(strDate + strTime);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 문자열을 짤라서 필요한 위치의 것을 반환한다
        /// </summary>
        /// <param name="strInNm"></param>
        /// <param name="intSeq"></param>
        /// <param name="strDelimiter"></param>
        /// <returns></returns>
        public static string SptChar(string strInNm, int intSeq, string strDelimiter)
        {
            string[] strArrChr = VB.Split(strInNm.Trim(), strDelimiter);
            try
            {
                if (strArrChr.Length <= intSeq)
                {
                    return "";
                }
                return strArrChr[intSeq];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 사용하지 마시용 : 프로젝트 단위별로 필요시 클래스(clsFormMap) 만들어 사용하시요.
        /// </summary>
        /// <param name="strForm"></param>
        /// <returns></returns>
        //public static Form FormMapping(string strForm)
        //{
        //    try
        //    {
        //        Type type = Type.GetType(strForm);  // MYPROJECT 네임스페이스의 Form1클래스 폼 
        //        object obj = Activator.CreateInstance(type);
        //        Form rtnForm = obj as Form;
        //        return rtnForm;
        //    }
        //    catch
        //    {
        //        return null;
        //    }

        //    ////Type type = Type.GetType("MYPROJECT.Form1");  // MYPROJECT 네임스페이스의 Form1클래스 폼 
        //    ////object obj = Activator.CreateInstance(type); 
        //    ////Form form = obj as Form; form.Show(); 

        //    //Form rtnForm = null;
        //    //Type formInstanceType = null;
        //    ////string strMode = "";

        //    //try
        //    //{
        //    //    formInstanceType = Type.GetType(strForm);
        //    //    rtnForm = (Form)Activator.CreateInstance(formInstanceType); 
        //    //    return rtnForm;
        //    //}
        //    //catch
        //    //{
        //    //    return rtnForm;
        //    //}
        //}

        /// <summary>
        /// 호출하는 컨트롤의 위치를 조정한다.
        /// </summary>
        /// <param name="CallControl"></param>
        /// <param name="ChildControl"></param>
        public static void SetChildPosition(Control CallControl, Control ChildControl)
        {

            int intTop = 0;
            int intTopHeight = 0;

            ChildControl.Left = SetAdjustPosition(CallControl, "LEFT");

            intTop = SetAdjustPosition(CallControl, "TOP");
            intTopHeight = intTop + CallControl.Height;

            if (intTopHeight >= 796)
            {
                ChildControl.Top = intTop - ChildControl.Height;
            }
            else
            {
                ChildControl.Top = intTop + CallControl.Height;
            }
        }

        /// <summary>
        /// 사이즈에 따라서 재조정한다.
        /// </summary>
        /// <param name="cControl"></param>
        /// <param name="strOption"></param>
        /// <returns></returns>
        private static int SetAdjustPosition(Control cControl, string strOption)
        {
            Control pControl = null;

            int rtnVal = 0;

            if (strOption == "LEFT")
            {
                rtnVal = cControl.Left;
            }
            else
            {
                rtnVal = cControl.Top;
            }

            if (cControl.Parent == null)
            {
                return rtnVal;
            }
            if (cControl.Parent is Form)
            {
                return rtnVal;
            }

            pControl = cControl.Parent;
            rtnVal = rtnVal + SetAdjustPosition(pControl, strOption);
            return rtnVal;
        }

        /// <summary>
        /// 선택된 스프래드 셀 색상 변경
        /// </summary>
        public static void SelectRowColor(FarPoint.Win.Spread.SheetView Spd, int Row)
        {
            int i = 0;

            if (Spd.RowCount <= 0)
                return;

            for (i = 0; i < Spd.RowCount; i++)
            {
                Spd.Cells[i, 0, i, Spd.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            }
            Spd.Cells[Row, 0, Row, Spd.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
        }

        /// <summary>
        /// JuminNo_Check_New
        /// </summary>
        /// <param name="strSsNo1"></param>
        /// <param name="strSsNo2"></param>
        /// <returns></returns>
        public static string JuminNoCheck(PsmhDb pDbCon, string strJuminNo1, string strJuminNo2)
        {
            string returnValue = "";
            int i;
            int j;
            int intMM;
            int intDD;
            int intCheckDigit;
            int intCheckTotal;
            int intCheckCount;

            strJuminNo1 = VB.Replace(strJuminNo1, "-", "");
            strJuminNo2 = VB.Replace(strJuminNo2, "-", "");
            if (VB.Len(strJuminNo1) != 6 || VB.Len(strJuminNo2) != 7)
            {
                returnValue = "주민번호 자리수 오류";
                return returnValue;
            }

            //주민번호 앞자리 체크
            returnValue = "주민번호 앞6자리 오류";

            //5,6,7,8 은 외국이거나, 보호시설 대상자이므로 체크를 하지 않는다.  '7, 8 추가 ggod(20090903)
            if (strJuminNo2.Substring(0, 1) == "5" || strJuminNo2.Substring(0, 1) == "6" || strJuminNo2.Substring(0, 1) == "7" || strJuminNo2.Substring(0, 1) == "8")
            {
                returnValue = "";
                return returnValue;
            }

            if (strJuminNo1 != "" && !VB.IsDate(strJuminNo1.Substring(0, 2) + "-" + strJuminNo1.Substring(2, 2) + "-" + strJuminNo1.Substring(4, 2)))
            {
                return returnValue;
            }

            if (strJuminNo1 == "")
            {
                return returnValue;
            }

            intMM = int.Parse(strJuminNo1.Substring(2, 2));
            intDD = int.Parse(strJuminNo1.Substring(4, 2));

            if (intMM < 1 || intMM > 12)
            {
                return returnValue;
            }
            else if (intDD < 1 || intDD > 31)
            {
                return returnValue;
            }

            //주민번호 뒤자리 체크
            returnValue = "주민번호 뒤7자리 오류";

            if (strJuminNo2 == "")
            {
                return returnValue;
            }

            strJuminNo2 = VB.Mid((string)(strJuminNo2 + "000000"), 1, 7);

            if (strJuminNo1.Substring(2, 2) == "00" || strJuminNo1.Substring(4, 2) == "00")
            {
                returnValue = BirthdateCheck(pDbCon, (string)(strJuminNo1.Substring(0, 2) + "0101"), strJuminNo2);
            }
            else
            {
                returnValue = BirthdateCheck(pDbCon, strJuminNo1, strJuminNo2);
            }

            //주민번호 Check
            if (strJuminNo2.Substring(1, 6) == "000000")
            {
                return returnValue; //신생아제외
            }

            intCheckTotal = 0;

            for (i = 1; i <= 12; i++)
            {
                j = i + 1;

                if (j > 9)
                {
                    j = j - 8;
                }

                if (i >= 1 && i <= 6)
                {
                    intCheckDigit = (int.Parse(strJuminNo1.Substring(i - 1, 1))) * j;
                }
                else
                {
                    intCheckDigit = (int.Parse(strJuminNo2.Substring(i - 6 - 1, 1))) * j;
                }

                intCheckTotal = intCheckTotal + intCheckDigit;
            }

            intCheckDigit = VB.Int(intCheckTotal / 11);
            intCheckDigit = VB.Int(intCheckDigit * 11);
            intCheckCount = intCheckTotal - intCheckDigit;

            switch (intCheckCount)
            {
                case 0:
                    intCheckDigit = 1;
                    break;
                case 1:
                    intCheckDigit = 0;
                    break;
                default:
                    intCheckDigit = System.Convert.ToInt32(11 - intCheckCount);
                    break;
            }

            if (intCheckTotal < 20)
            {
                return returnValue;
            }

            if (intCheckDigit != VB.Val(strJuminNo2.Substring(6, 1)))
            {
                returnValue = "주민번호 체크 오류!";
                return returnValue;
            }

            return returnValue;
        }

        /// <summary>
        /// 생년월일체크
        /// </summary>
        /// <param name="strJuminNo1"></param>
        /// <param name="strJuminNo2"></param>
        /// <returns></returns>
        public static string BirthdateCheck(PsmhDb pDbCon, string strJuminNo1, string strJuminNo2)
        {
            string returnValue;
            switch (strJuminNo2.Substring(0, 1))
            {
                case "0":
                case "9":
                    if (VB.Val(ComQuery.CurrentDateTime(pDbCon, "D")) < VB.Val("18" + strJuminNo1))
                    {
                        returnValue = "현재일자 보다 생년월일이 큽니다.";
                        return returnValue;
                    }
                    break;
                case "1":
                case "2":
                    if (VB.Val(ComQuery.CurrentDateTime(pDbCon, "D")) < VB.Val("19" + strJuminNo1))
                    {
                        returnValue = "현재일자 보다 생년월일이 큽니다.";
                        return returnValue;
                    }
                    break;
                case "3":
                case "4":
                    if (VB.Val(ComQuery.CurrentDateTime(pDbCon, "D")) < VB.Val("20" + strJuminNo1))
                    {
                        returnValue = "현재일자 보다 생년월일이 큽니다.";
                        return returnValue;
                    }
                    break;
                case "5": //외국인
                case "6":
                    if (VB.Val(ComQuery.CurrentDateTime(pDbCon, "D")) < VB.Val("19" + strJuminNo1))
                    {
                        returnValue = "현재일자 보다 생년월일이 큽니다.";
                        return returnValue;
                    }
                    break;
                default:
                    if (VB.Val(ComQuery.CurrentDateTime(pDbCon, "D")) < VB.Val("20" + strJuminNo1))
                    {
                        returnValue = "현재일자 보다 생년월일이 큽니다.";
                        return returnValue;
                    }
                    break;
            }

            if (VB.Val(strJuminNo1.Substring(2, 2)) >= 1 && VB.Val(strJuminNo1.Substring(2, 2)) <= 12)
            {
            }
            else
            {
                returnValue = "주민번호 앞6자리 오류";
                return returnValue;
            }

            if (VB.Val(strJuminNo1.Substring(strJuminNo1.Length - 2, 2)) >= 1 && VB.Val(strJuminNo1.Substring(strJuminNo1.Length - 2, 2)) <= 31)
            {
            }
            else
            {
                returnValue = "주민번호 앞6자리 오류";
                return returnValue;
            }

            returnValue = "";
            return returnValue;
        }

        /// <summary>
        /// 날짜 타입이 맞지 않으면 에러가 남으로
        /// </summary>
        /// <param name="dtp"></param>
        /// <param name="strBirth"></param>
        public static void SetDtPickerDate(DateTimePicker dtp, string strBirth)
        {
            try
            {
                if (strBirth != "")
                {
                    dtp.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(strBirth, "D"));
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 일주일중에 요일을 1~7로 변경
        /// </summary>
        /// <param name="dt">날짜</param>
        /// <param name="startOfWeek"></param>
        /// <returns></returns>
        public static int Weekday(DateTime dt, DayOfWeek startOfWeek)
        {
            return (dt.DayOfWeek - startOfWeek + 7) % 7;
        }

        /// <summary>
        /// PACS 프로그램을 연동한다.
        /// </summary>
        /// <param name="strFlag"></param>
        /// <param name="strPtno"></param>
        public static void LoadPacs(string strFlag, string strPtno)
        {
            if (strFlag == "MS")
            {

            }
            else if (strFlag == "TECH")
            {
                LoadPacsTech(strPtno);
            }
            else //INFINITI
            {
                LoadPacsInfiniti(strPtno);
            }
        }

        public static void LoadPacsTech(string strPtno)
        {
            try
            {
                //VB.Shell(@"C:\Program Files\TechHeim\ViewRex\bin\PACS_Bridge.exe " + clsType.gUseInfo.strUseId + " " + strPtno + ", /OCS", "NormalFocus");
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        public static void LoadPacsInfiniti(string strPtno)
        {
            try
            {
                if (strPtno.Trim() == null)
                {
                    VB.Shell(@"C:\Program Files\Internet Explorer\iexplore.exe" + " " + "http://192.168.104.204/pkg_pacs/external_interface.aspx?TYPE=L&LID=med1&LPW=med&PID=" + "", "NormalFocus");
                }
                else
                {
                    VB.Shell(@"C:\Program Files\Internet Explorer\iexplore.exe" + " " + "http://192.168.104.204/pkg_pacs/external_interface.aspx?TYPE=L&LID=med1&LPW=med&PID=" + strPtno.Trim(), "NormalFocus");
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }


        /// <summary>
        /// Oracle에서 date type 변경
        /// </summary>
        /// <param name="strDate"></param> date
        /// <param name="strFlag"></param> 유형
        /// ex) clsComFunc.ConvOraToDate(strDate, "D");
        public static string ConvOraToDate(string strDate, string strFlag)
        {
            string rtnVal = "";

            if (strDate == "")
            {
                rtnVal = "Null";
                return rtnVal;
            }

            switch (strFlag)
            {
                case "A":
                    rtnVal = "TO_DATE('" + VB.Format(Convert.ToDateTime(strDate), "yyyy-MM-dd HH:mm:ss") + "', 'yyyy-MM-dd hh24:MI:ss')";
                    break;
                case "D":
                    rtnVal = "TO_DATE('" + VB.Format(Convert.ToDateTime(strDate), "yyyy-MM-dd") + "', 'yyyy-MM-dd')";
                    break;
                case "T": //time 12시간일경우 : hh12 / 24시간일경우 : hh24
                    rtnVal = "TO_DATE('" + VB.Format(Convert.ToDateTime(strDate), "yyyy-MM-dd") + "', hh24MIss')";
                    break;
            }

            return rtnVal;
            //오라클 분 : MI
            //MSSQL 분 : MM
        }

        public static string ConvOraToDate(DateTime strDate, string strFlag)
        {
            string rtnVal = "";

            switch (strFlag)
            {
                case "A":
                    rtnVal = "TO_DATE('" + VB.Format(strDate, "yyyy-MM-dd HH:mm:ss") + "', 'yyyy-MM-dd hh24:MI:ss')";
                    break;
                case "D":
                    rtnVal = "TO_DATE('" + VB.Format(strDate, "yyyy-MM-dd") + "', 'yyyy-MM-dd')";
                    break;
                case "T":
                    rtnVal = "TO_DATE('" + VB.Format(strDate, "yyyy-MM-dd") + "', hh24MIss')";
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// TopMost로 메세지 박스를 띄움
        /// </summary>
        /// <param name="strMsg"></param>
        /// <param name="strCation"></param>
        public static void MsgBox(string strMsg, string strCation = "")
        { 
            if (strCation == "") strCation = "PSMH";

            MessageBox.Show(new Form() { TopMost = true, StartPosition = FormStartPosition.CenterParent }, strMsg, strCation, MessageBoxButtons.OK, MessageBoxIcon.Information);

            //frmShowMessage.Show(strMsg, strCation, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 메세지 박스의 Owner를 설정할 수 있다
        /// </summary>
        /// <param name="ownerForm"></param>
        /// <param name="strMsg"></param>
        /// <param name="strCation"></param>
        public static void MsgBoxEx(Form ownerForm, string strMsg, string strCation = "")
        {
            if (strCation == "") strCation = "PSMH";

            if (ownerForm == null)
            {
                MsgBox(strMsg, strCation);
                return;
            }

            if (ownerForm.IsDisposed)
            {
                MsgBox(strMsg, strCation);
                return;
            }

            ////일부 컴퓨터는 에러가 난다 : 원인 모름
            //////MessageBox.Show(ownerForm, strMsg, strCation, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

            Screen screen = Screen.FromControl(ownerForm);
            MessageBox.Show(
                new Form()
                {
                    TopMost = true,
                    StartPosition = FormStartPosition.Manual,
                    Location = new Point(screen.WorkingArea.Right / 2, screen.WorkingArea.Height / 2)
                },
                strMsg, strCation,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);


            //MessageBox.Show(ownerForm, strMsg, strCation, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            //MessageBox.Show(new Form() {Parent = ownerForm, TopMost = true }, strMsg, strCation, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            //frmShowMessage.Show(ownerForm, strMsg, strCation, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// TopMost로 메세지 박스를 띄움
        /// </summary>
        /// <param name="strMsg"></param>
        /// <param name="strCation"></param>
        /// <param name="btn2"></param>
        /// <returns></returns>
        public static DialogResult MsgBoxQ(string strMsg, string strCation = "", MessageBoxDefaultButton btn2 = MessageBoxDefaultButton.Button2)
        {
            if (strCation == "") strCation = "PSMH";

            DialogResult rtnVal = MessageBox.Show(new Form() { TopMost = true, StartPosition = FormStartPosition.CenterParent }, strMsg, strCation, MessageBoxButtons.YesNo, MessageBoxIcon.Question, btn2);

            //DialogResult rtnVal = frmShowMessage.Show(strMsg, strCation, MessageBoxButtons.YesNo, MessageBoxIcon.Question, btn2);
            return rtnVal;
        }

        /// <summary>
        /// 메세지 박스의 Owner를 설정할 수 있다
        /// </summary>
        /// <param name="strMsg"></param>
        /// <param name="strCation"></param>
        /// <param name="btn2"></param>
        /// <returns></returns>
        public static DialogResult MsgBoxQEx(Form ownerForm, string strMsg, string strCation = "", MessageBoxDefaultButton btn2 = MessageBoxDefaultButton.Button2)
        {
            if (strCation == "") strCation = "PSMH";

            if (ownerForm == null)
            {
                return MsgBoxQ(strMsg, strCation);
            }

            if (ownerForm.IsDisposed)
            {
                return MsgBoxQ(strMsg, strCation);
            }
            //일부 컴퓨터는 에러가 난다 : 원인 모름
            ////DialogResult rtnVal = MessageBox.Show(ownerForm, strMsg, strCation, MessageBoxButtons.YesNo, MessageBoxIcon.Question, btn2, (MessageBoxOptions)0x40000);

            //Screen screen = Screen.FromControl(ownerForm);
            //DialogResult rtnVal = MessageBox.Show(
            //    new Form()
            //    {
            //        TopMost = true,
            //        StartPosition = FormStartPosition.Manual,
            //        Location = new Point(screen.WorkingArea.Right / 2, screen.WorkingArea.Height / 2)
            //    },
            //    strMsg, strCation,
            //    MessageBoxButtons.YesNo,
            //    MessageBoxIcon.Question,
            //    btn2);

            DialogResult rtnVal = MessageBox.Show(ownerForm, strMsg, strCation, MessageBoxButtons.YesNo, MessageBoxIcon.Question, btn2);
            //DialogResult rtnVal = MessageBox.Show( new Form() {  TopMost = true },  strMsg, strCation,   MessageBoxButtons.YesNo,    MessageBoxIcon.Question,  btn2);

            //DialogResult rtnVal = frmShowMessage.Show(ownerForm, strMsg, strCation, MessageBoxButtons.YesNo, MessageBoxIcon.Question, btn2);

            return rtnVal;
        }

        /// <summary>
        /// Form에 아이콘을 세팅한다.
        /// </summary>
        public static void SetFormIcon(Form frm)
        {

        }

        /// <summary>
        /// 폼스킨 적용(통합버젼)
        /// </summary>
        /// <param name="objParent"></param>
        //public static void SetFormSkin(Control objParent)
        //{

        //}

        /// <summary>
        /// 폴더에 있는 파일을 삭제한다.
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static bool DeleteFoldAll(string strPath)
        {
            try
            {
                if (Directory.Exists(strPath) == true)
                {
                    DirectoryInfo diSource = new DirectoryInfo(strPath);
                    foreach (FileInfo fi in diSource.GetFiles())
                    {
                        fi.Delete();
                    }
                    diSource = null;
                }
                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// 문자를 바이트로 변환
        /// </summary>
        /// <param name="str"></param>
        /// <param name="nlen"></param>
        /// <returns></returns>
        public static string StrCpyByte(string str, int nlen)
        {
            string rtnVal = "";
            int i = 0;
            int length = 0;

            for (i = 0; i < VB.Len(str); i++)
            {
                if (VB.Asc(VB.Mid(str, i, 1)) < 0)
                {
                    length = length + 2;
                }
                else
                {
                    length = length + 1;
                }
                rtnVal = rtnVal + VB.Mid(str, i + 1, 1);
                if (length > nlen)
                {
                    break;
                }
            }
            return rtnVal;
        }

        /// <summary>
        /// delete folder and file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="recursive"></param>
        public static void DeleteDirectory(string path, bool recursive = false)
        {
            // Delete all files from the folder 'c:\Games', but
            // keep all sub-folders and its files
            //DeleteDirectory(@"c:\Games");

            // Delete the folder 'c:\Projects' and all of its content
            //DeleteDirectory(@"c:\Projects", true);

            // Delete all files from the folder 'c:\Software', but
            // keep all sub-folders and its files
            //DeleteDirectory(@"c:\Software", false);
            if (recursive)
            {
                var subfolders = Directory.GetDirectories(path);
                foreach (var s in subfolders)
                {
                    DeleteDirectory(s, recursive);
                }
            }

            var files = Directory.GetFiles(path);
            foreach (var f in files)
            {
                var attr = File.GetAttributes(f);

                if ((attr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    File.SetAttributes(f, attr ^ FileAttributes.ReadOnly);
                }

                File.Delete(f);
            }

            Directory.Delete(path);
        }

        /// <summary>
        /// 숨김패널 안에 있는 컨트롤은 저장하지 않는다.
        /// </summary>
        /// <param name="cControl"></param>
        /// <returns></returns>
        public static bool IsVisible(Control cControl, bool isSpcPanel = false, Control pControl = null)
        {
            bool rtnVal = true;

            Control ctlControl;
            Control ctlControlCon;

            ctlControl = cControl;

            if (ctlControl.Visible == true)
            {
                rtnVal = true;
            }
            else
            {
                rtnVal = false;
            }

        GoBack:

            if (ctlControl.Parent == null)
            {
                return rtnVal;
            }

            if (isSpcPanel == true)
            {
                if (ctlControl.Parent == pControl)
                {
                    return rtnVal;
                }
            }

            if (ctlControl.Parent is Form)
            {
                return rtnVal;
            }

            ctlControlCon = ctlControl.Parent;
            ctlControl = null;
            ctlControl = ctlControlCon;
            ctlControlCon = null;
            if (ctlControl.Visible == true)
            {
                rtnVal = true;
            }
            else
            {
                rtnVal = false;
                return rtnVal;
            }

            goto GoBack;
        }

        /// <summary>
        /// 주민등록 번호를 받아서 생년월일을 가지고 온다. 형식 (yyyy-MM-dd)
        /// </summary>
        /// <param name="strJuMin1">주민 등록번호 앞에 것</param>
        /// <param name="strJuMin2">주민 등록번호 뒤에 것</param>
        /// <returns></returns>
        public static string GetBirthday(string strJuMin1, string strJuMin2)
        {
            string rtnVal = "";
            if (strJuMin1 == "" || strJuMin2 == "")
            {
                return "";
            }

            if (strJuMin1.Length != 6)
            {
                return "";
            }

            if (strJuMin2.Length != 7)
            {
                return "";
            }

            if (VB.IsNumeric(strJuMin1) == false || VB.IsNumeric(strJuMin2) == false)
            {
                return "";
            }

            switch (VB.Left(strJuMin2, 1))
            {
                case "1":
                case "2":
                    rtnVal = "19";
                    break;
                case "3":
                case "4":
                    rtnVal = "20";
                    break;
                case "5":
                case "6":
                    rtnVal = "19";
                    break;
                case "7":
                case "8":
                    rtnVal = "20";
                    break;
            }

            rtnVal = rtnVal + VB.Left(strJuMin1, 2) + "-" + VB.Mid(strJuMin1, 3, 2) + "-" + VB.Mid(strJuMin1, 5, 2);

            return rtnVal;
        }

        /// <summary>
        /// 두 날짜(시간) 차이 구하기. 형식 (00:00)
        /// </summary>
        /// <param name="strFrDate">날짜(시간) 작은 값</param>
        /// <param name="strEndDate">날짜(시간) 큰 값</param>
        /// <returns></returns>
        public static string TimeDiff(string strFrDate, string strEndDate)
        {
            string rtnVal = "";

            double dblFr = 0;
            double dblEnd = 0;
            double dblTimeGap = 0;

            int Hour = 0;
            int Minute = 0;

            if (VB.IsDate(strFrDate) == false || VB.IsDate(strEndDate) == false)
                return rtnVal;

            dblFr = Convert.ToDateTime(strFrDate).Ticks;
            dblEnd = Convert.ToDateTime(strEndDate).Ticks;

            dblTimeGap = (dblEnd - dblFr) / 10000.0f;

            if (dblTimeGap < 0)
                return rtnVal;

            //시간 6000
            if ((dblTimeGap / 3600000) >= 1)
            {
                Hour = Convert.ToInt32(Math.Truncate(dblTimeGap / 3600000));
                dblTimeGap = (dblTimeGap % 3600000);

                rtnVal = VB.Format(Hour, "00");
            }
            else
            {
                rtnVal = "00";
            }

            //분 600
            if ((dblTimeGap / 60000) >= 1)
            {
                Minute = Convert.ToInt32(dblTimeGap / 60000);

                if (rtnVal.Trim() != "")
                {
                    rtnVal = rtnVal + ":" + VB.Format(Minute, "00");
                }
                else
                {
                    rtnVal = VB.Format(Minute, "00");
                }
            }
            else
            {
                rtnVal = rtnVal + ":00";
            }

            return rtnVal;
        }

        /// <summary>
        /// 두 날짜(시간) 차이 구하기. 형식 분
        /// </summary>
        /// <param name="strFrDate">날짜(시간) 작은 값</param>
        /// <param name="strEndDate">날짜(시간) 큰 값</param>
        /// <returns>00</returns>
        public static string TimeDiffMin(string strFrDate, string strEndDate)
        {
            string rtnVal = "";

            double dblFr = 0;
            double dblEnd = 0;
            double dblTimeGap = 0;

            int Minute = 0;

            if (VB.IsDate(strFrDate) == false || VB.IsDate(strEndDate) == false)
                return rtnVal;

            dblFr = Convert.ToDateTime(strFrDate).Ticks;
            dblEnd = Convert.ToDateTime(strEndDate).Ticks;

            dblTimeGap = (dblEnd - dblFr) / 10000.0f;

            if (dblTimeGap <= 0)
                return dblTimeGap.ToString();

            //분 600
            if ((dblTimeGap / 60000) >= 1)
            {
                Minute = Convert.ToInt32(dblTimeGap / 60000);

                rtnVal = Minute.ToString();
            }

            return rtnVal;
        }

        /// <summary>
        /// 두 날짜(시간) 차이 구하기. 형식 분
        /// </summary>
        /// <param name="strFrDate">날짜(시간) 작은 값</param>
        /// <param name="strEndDate">날짜(시간) 큰 값</param>
        /// <returns>00:00</returns>
        public static string TimeDiffHourMin(string strFrDate, string strEndDate)
        {
            string rtnVal = "";

            double dblFr = 0;
            double dblEnd = 0;
            double dblTimeGap = 0;

            int Hour = 0;
            int Minute = 0;

            if (VB.IsDate(strFrDate) == false || VB.IsDate(strEndDate) == false)
                return rtnVal;

            dblFr = Convert.ToDateTime(strFrDate).Ticks;
            dblEnd = Convert.ToDateTime(strEndDate).Ticks;

            dblTimeGap = (dblEnd - dblFr) / 10000.0f;

            if (dblTimeGap < 0)
                return rtnVal;

            //분 600
            if ((dblTimeGap / 60000) >= 1)
            {
                Minute = Convert.ToInt32(dblTimeGap / 60000);
            }

            if (Minute >= 1)
            {
                rtnVal = Convert.ToInt32(Minute / 60).ToString("00") + ":" + Convert.ToInt32(Minute % 60).ToString("00");
            }

            return rtnVal;
        }

        /// <summary>
        ///  스프레드의 NameBox 를 불러온다. 최대 줄 수는 650 까지
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string GetNameBox(int i)
        {
            string strReturn = "";

            i = i + 1;

            if (i / 26 > 26)
            {
                return strReturn;
            }


            if (i / 26 > 0)
            {
                strReturn = Convert.ToString(Convert.ToChar(64 + ((i / 26))) + Convert.ToChar(64 + ((i % 26) == 0 ? 26 : (i % 26))));
            }
            else
            {
                strReturn = Convert.ToString(Convert.ToChar(64 + ((i % 26) == 0 ? 26 : (i % 26))));
            }

            return strReturn;
        }

        /// <summary>2017.06.07.김홍록 : 컬럼 데이트형 쿼리 문장 작성</summary>
        /// <param name="date">일시</param>
        /// <param name="sqlForm">변환하고자 하는 형태</param>
        /// <param name="IsComma">콤마가 필요하면 True</param>
        /// <returns></returns>
        public static string covSqlDate(string date, string sqlForm, bool IsComma) // 쿼리구문의 데이트 형 문자열 변환 (예: TO_DATE('변수','YYYY-MM-DD')
        {


            string strComma = "";

            if (IsComma == true)
            {
                strComma = ",";
            }

            date = strComma + " TO_DATE('" + date + "','" + sqlForm + "') \r\n";
            return date;

        }

        /// <summary>컬럼 데이트형 쿼리 문장 작성
        /// </summary>
        /// <param name="date">데이트형 컬럼</param>
        /// <param name="IsComma">콤마가 필요하면 True</param>
        /// <returns>TO_DATE('변수','YYYY-MM-DD')</returns>
        public static string covSqlDate(string date, bool IsComma) // 쿼리구문의 데이트 형 문자열 변환 (예: TO_DATE('변수','YYYY-MM-DD')
        {
            DateTime dt;
            date = date.Replace("-", "").Replace(".", "");
            string strComma = null;
            if (IsComma == true)
            {
                strComma = ",";
            }


            if (VB.IsNumeric(date))
            {
                if (date.Length == 10 || date.Length == 8)
                {
                    dt = new DateTime(int.Parse(date.Substring(0, 4)), int.Parse(date.Substring(4, 2)), int.Parse(date.Substring(6, 2)));
                    date = string.Format("{0:yyyy-MM-dd}", dt.ToShortDateString());
                    date = strComma + "TO_DATE('" + date + "','YYYY-MM-DD') \r\n";
                    return date;
                }
            }
            return null;
        }

        /// <summary>컬럼 문자형 쿼리 문장 작성
        /// </summary>
        /// <param name="strColumn">대상 컬럼</param>
        /// <param name="IsComma">콤마가 필요하면 True</param>
        /// <returns> 'abc' 또는 'abc', </returns>        
        public static string covSqlstr(string strColumn, bool IsComma) //쿼리구문의 문자열 변환(예 : 'abc')
        {

            strColumn = strColumn == null ? "" : strColumn;
            if (IsComma == true)
            {
                strColumn = ", '" + strColumn.Replace("'", "\'") + "' \r\n";
            }
            else
            {
                strColumn = "'" + strColumn.Replace("'", "\'") + "' \r\n";
            }


            return strColumn;
        }

        /// <summary>컨트롤 툴팁</summary>
        /// <author>김홍록</author>
        /// <createdate>2017-07-26</createdate>
        /// <param name="c">컨트롤</param>
        /// <param name="s">메시지</param>
        public static void setToolTip(Control c, string s)
        {
            ToolTip tp1 = new ToolTip();
            tp1.AutoPopDelay = 3000;//지연시간 기본5000
            tp1.InitialDelay = 500;// 표시까지시간
            tp1.ReshowDelay = 500;//다음콘트롤 표시시간
            tp1.ShowAlways = true;//부모 화설화여부

            tp1.SetToolTip(c, s);
        }

        /// <summary>
        /// 파일을 읽어서 이미지로 변환한다
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="pWidth">너비</param>
        /// <param name="pHeight">높이</param>
        /// <returns></returns>
        public static Image FileToImage(string strFilePath, int pWidth = -1, int pHeight = -1)
        {
            int intWidth = 0;
            int intHeight = 0;

            try
            {
                Bitmap image1 = (Bitmap)Image.FromFile(strFilePath, true);

                if (pWidth == -1)
                    intWidth = image1.Width;
                else
                    intWidth = pWidth;

                if (pHeight == -1)
                    intHeight = image1.Height;
                else
                    intHeight = pHeight;

                Bitmap newImage = new Bitmap(intWidth, intHeight, PixelFormat.Format64bppArgb);
                Graphics graphics_1 = Graphics.FromImage(newImage);
                graphics_1.CompositingQuality = CompositingQuality.HighQuality;  //HighQuality
                graphics_1.InterpolationMode = InterpolationMode.HighQualityBicubic;    //HighQualityBicubic
                graphics_1.SmoothingMode = SmoothingMode.HighQuality;   //HighQuality
                graphics_1.DrawImage(image1, 0, 0, intWidth, intHeight);

                image1.Dispose();
                image1 = null;

                return newImage;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 공통라이브러리에 있는 폼을 로드 한다.
        /// </summary>
        /// <param name="strClassNm">라이브러리 명</param>
        /// <param name="strFormNm">폼명</param>
        /// <returns></returns>
        public static Form FormMapping(string strClassNm, string strFormNm)
        {
            try
            {
                Assembly assem = Assembly.GetExecutingAssembly();
                Form objForm = null;

                Type t = assem.GetType(strClassNm + "." + strFormNm);
                objForm = (Form)Activator.CreateInstance(t);
                return objForm;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// INI File을 읽어서 컴퓨터 세팅값에 저장한다.
        /// 작성자 : 박웅규 2018-05-01
        /// ComQuery CheckINIandSaveConfigExcute 참고
        /// </summary>
        public static void CheckINIandSaveConfig()
        {
            #region //C:\CMC\ocs_deptset.INI : 외래COS 간호사 로그인에서 사용
            string strFIleNm = "";
            strFIleNm = @"C:\CMC\ocs_deptset.INI";

            FileInfo pRevEmrFile = new FileInfo(strFIleNm);
            if (pRevEmrFile.Exists == true)
            {
                string[] fileReader = File.ReadAllLines(strFIleNm);

                string strDept = "";
                string strDrCd = "";
                string strSabun = "";

                for (int i = 0; i < fileReader.Length; i++)
                {
                    if (i == 0)
                    {
                        strDept = SptChar(SptChar(fileReader[i].Trim(), 1, ":"), 0, ".");
                    }
                    else if (i == 1)
                    {
                        strDrCd = SptChar(SptChar(fileReader[i].Trim(), 1, ":"), 0, ".");
                        strSabun = (SptChar(SptChar(SptChar(fileReader[i].Trim(), 1, ":"), 1, "."), 1, "(")).Replace(")", "");
                    }
                }
                fileReader = null;
                if (strDept != "" && strDrCd != "" && strSabun != "")
                {
                    ComQuery.CheckINIandSaveConfigExcute("간호사_DeptCode", strDept);
                    ComQuery.CheckINIandSaveConfigExcute("간호사_DrCode", strDrCd);
                    ComQuery.CheckINIandSaveConfigExcute("간호사_Sabun", strSabun);
                }
            }
            #endregion 
        }

        /// <summary>
        /// 프로그램에 필요한 폴더를 체크하고 만들어 준다
        /// </summary>
        public static void CheckAndCreateFold()
        {
            string strMentorsoft = @"C:\PSMHEXE";

            try
            {

                if (Directory.Exists(strMentorsoft + @"\exe") == false)
                {
                    Directory.CreateDirectory(strMentorsoft + @"\exe");
                }
                if (Directory.Exists(strMentorsoft + @"\exenet") == false)
                {
                    Directory.CreateDirectory(strMentorsoft + @"\exenet");
                }
                if (Directory.Exists(strMentorsoft + @"\tessdata") == false)
                {
                    Directory.CreateDirectory(strMentorsoft + @"\tessdata");
                }
                if (Directory.Exists(strMentorsoft + @"\icon") == false)
                {
                    Directory.CreateDirectory(strMentorsoft + @"\ItemValue");
                }
                if (Directory.Exists(strMentorsoft + @"\EmrImageTmp") == false)
                {
                    Directory.CreateDirectory(strMentorsoft + @"\EmrImageTmp");
                }
                if (Directory.Exists(strMentorsoft + @"\FormToImage") == false)
                {
                    Directory.CreateDirectory(strMentorsoft + @"\FormToImage");
                }
                if (Directory.Exists(strMentorsoft + @"\TabletImage") == false)
                {
                    Directory.CreateDirectory(strMentorsoft + @"\TabletImage");
                }
                if (Directory.Exists(strMentorsoft + @"\ScanData") == false)
                {
                    Directory.CreateDirectory(strMentorsoft + @"\ScanData");
                }
                if (Directory.Exists(strMentorsoft + @"\ScanTmp") == false)
                {
                    Directory.CreateDirectory(strMentorsoft + @"\ScanTmp");
                }
                if (Directory.Exists(strMentorsoft + @"\TextEmr") == false)
                {
                    Directory.CreateDirectory(strMentorsoft + @"\TextEmr");
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// 양력을 음력으로 변환
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="intFomat"></param>
        /// <returns></returns>
        public static string ToLunar(string strDate, int intFomat = 0)
        {
            string rtnVal = ""; //반환값
            string LeapDate;

            DateTime date = DateTime.Parse(strDate);
            KoreanLunisolarCalendar ksc = new KoreanLunisolarCalendar();

            int year = ksc.GetYear(date);
            int month = ksc.GetMonth(date);
            int day = ksc.GetDayOfMonth(date);
            bool isLeapMonth;

            //윤달이 끼어 있으면
            if (ksc.GetMonthsInYear(year) > 12)
            {
                int leapMonth = ksc.GetLeapMonth(year);
                if (month >= leapMonth)
                {
                    isLeapMonth = (month == leapMonth);
                    month--;
                }
            }

            LeapDate = string.Format("{0}-{1}-{2}", year, month, day); //yyyy-MM-dd

            switch (intFomat)
            {
                case 1: //CLDF_YMD_Full_Dash
                    rtnVal = DateTime.Parse(LeapDate).ToString("yyyy-MM-dd");
                    break;
                case 2: //CLDF_YMD_Full_Slash
                    rtnVal = DateTime.Parse(LeapDate).ToString("yyyy/MM/dd");
                    break;
                case 4: //CLDF_YMD_Full_Dot
                    rtnVal = DateTime.Parse(LeapDate).ToString("yyyy.MM.dd");
                    break;
                case 8: //CLDF_YMD_Full_None
                    rtnVal = DateTime.Parse(LeapDate).ToString("yyyyMMdd");
                    break;
                default:
                    rtnVal = DateTime.Parse(LeapDate).ToString("yyyy-MM-dd");
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// 음력을 양력으로 변환
        /// </summary>
        /// <param name="lunarDate"></param>
        /// <returns></returns>
        public static string ToSolar(string strDate, int intFomat = 0)
        {
            string rtnVal = ""; //반환값
            DateTime returnDate = new DateTime();

            DateTime date = DateTime.Parse(strDate);
            KoreanLunisolarCalendar ksc = new KoreanLunisolarCalendar();

            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            if (ksc.GetMonthsInYear(year) > 12)
            {
                int leapMonth = ksc.GetLeapMonth(year);

                if (month >= leapMonth - 1)
                {
                    returnDate = ksc.ToDateTime(year, month + 1, day, 0, 0, 0, 0);
                }
                else
                {
                    returnDate = ksc.ToDateTime(year, month, day, 0, 0, 0, 0);
                }
            }
            else
            {
                returnDate = ksc.ToDateTime(year, month, day, 0, 0, 0, 0);
            }
            switch (intFomat)
            {
                case 1:
                    rtnVal = returnDate.ToString("yyyy-MM-dd");
                    break;
                case 2:
                    rtnVal = returnDate.ToString("yyyy/MM/dd");
                    break;
                default:
                    rtnVal = returnDate.ToString("yyyy-MM-dd");
                    break;
            }
            return rtnVal;
        }

        /// <summary>
        /// Unicode => Utf8
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string EncodeUTF8(string strText)
        {
            string rtnVal = "";

            byte[] bytes = Encoding.Default.GetBytes(strText);
            rtnVal = Encoding.UTF8.GetString(bytes);

            return rtnVal;
        }

        /// <summary>
        /// Unicode => Utf8
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string EncodeUTF8(byte[] bytes)
        {
            string rtnVal = "";

            rtnVal = Encoding.UTF8.GetString(bytes);

            return rtnVal;
        }

        /// <summary>
        /// Utf8 => Unicode
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string DecodeUTF8(string strText)
        {
            string rtnVal = "";

            byte[] bytes = Encoding.Unicode.GetBytes(strText);
            rtnVal = Encoding.Unicode.GetString(bytes);

            return rtnVal;
        }

        /// <summary>
        /// Utf8 => Unicode
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string DecodeUTF8(byte[] bytes)
        {
            string rtnVal = "";

            rtnVal = Encoding.Unicode.GetString(bytes);

            return rtnVal;
        }

        /// <summary>
        /// Description : 부서명을 읽어온다.
        /// Author : 박병규
        /// Create Date : 2017.05.26
        /// <param name="strCode"></param>
        /// </summary>
        public string Read_BuseName(PsmhDb pDbCon, string strCode)
        {
            DataTable DtFunc = new DataTable();
            string SQL = String.Empty;
            string SqlErr = String.Empty;
            string strVal = String.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT SNAME";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BUSE ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "    AND BuCode = '" + strCode + "' ";
                SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, pDbCon);

                if (DtFunc.Rows.Count == 0)
                {
                    DtFunc.Dispose();
                    DtFunc = null;
                    return strVal;
                }

                if (DtFunc.Rows.Count == 1)
                    strVal = DtFunc.Rows[0]["Sname"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return strVal;
            }
        }

        /// <summary>
        /// Description : 상병명을 읽어온다.
        /// Author : 박병규
        /// Create Date : 2017.07.03
        /// <param name="ArgCode"></param>
        /// <param name="ArgGubun">
        /// 1:한글명
        /// 2.영문명
        /// </param>
        /// </summary>
        /// <seealso cref="frm의료급여수동승인 : READ_BAS_ILL"/> 
        /// <seealso cref="frmCsinfoView.frm : READ_MSYM_NAME"/> 
        public string Read_IllsName(PsmhDb pDbCon, string ArgCode, string ArgGubun)
        {
            DataTable DtFunc = new DataTable();
            string SQL = String.Empty;
            string SqlErr = String.Empty;
            string strVal = String.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT IllNameK, IllNameE ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ILLS";
                SQL += ComNum.VBLF + "  WHERE IllCode = '" + ArgCode + "'";
                SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

                if (DtFunc.Rows.Count == 0)
                {
                    DtFunc.Dispose();
                    DtFunc = null;
                    return strVal;
                }

                if (DtFunc.Rows.Count == 1)
                    switch (ArgGubun)
                    {
                        case "1":
                            strVal = DtFunc.Rows[0]["IllNameK"].ToString().Trim();
                            break;

                        case "2":
                            strVal = DtFunc.Rows[0]["IllNameE"].ToString().Trim();
                            break;
                    }
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return strVal;
            }
        }

        /// <summary>
        /// Description :직원성명을 읽어온다.
        /// Author : 박병규
        /// Create Date : 2017.05.29
        /// <param name="strCode"></param>
        /// 2017-05-29 박병규
        /// </summary>
        /// <seealso cref="VbFunction.bas : READ_PASSNAME"/> 
        public string Read_SabunName(PsmhDb pDbCon, string strCode)
        {
            DataTable DtFunc = new DataTable();
            string SQL = String.Empty;
            string SqlErr = String.Empty;
            string strVal = String.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT USERNAME";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_USER ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "    AND IDNUMBER = '" + strCode + "' ";
                SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

                if (DtFunc.Rows.Count == 0)
                {
                    DtFunc.Dispose();
                    DtFunc = null;
                    return strVal;
                }

                if (DtFunc.Rows.Count == 1)
                    strVal = DtFunc.Rows[0]["USERNAME"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strVal;
            }
        }

        /// <summary>
        /// Description : 환자정보중 필요한 항목을 읽어온다.
        /// Author : 박병규
        /// Create Date : 2017.06.05
        /// <param name="strPtno"></param>
        /// <param name="strType">
        /// 1 :등록번호
        /// 2 :환자성명
        /// 3 :휴대폰번호
        /// </param>
        /// </summary>
        /// <seealso cref="Vbfunc.bas : READ_PATIENTNAME"/>  
        public string Read_Patient(PsmhDb pDbCon, string strPtno, string strType)
        {
            DataTable DtFunc = new DataTable();
            string SQL = String.Empty;
            string SqlErr = String.Empty;
            string strVal = String.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT * ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "    AND PANO = '" + strPtno + "' ";
                SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, pDbCon);

                if (DtFunc.Rows.Count == 0)
                {
                    DtFunc.Dispose();
                    DtFunc = null;
                    return strVal;
                }

                if (DtFunc.Rows.Count == 1)
                    switch (strType)
                    {
                        case "1":
                            strVal = DtFunc.Rows[0]["PANO"].ToString().Trim();
                            break;
                        case "2":
                            strVal = DtFunc.Rows[0]["SNAME"].ToString().Trim();
                            break;
                        case "3":
                            strVal = DtFunc.Rows[0]["HPHONE"].ToString().Trim();
                            break;
                        case "4":
                            strVal = DtFunc.Rows[0]["GBSMS"].ToString().Trim();
                            break;
                        case "5":
                            strVal = DtFunc.Rows[0]["JUMIN3"].ToString().Trim();
                            break;
                    }
                else
                {
                    strVal = "";
                }

                DtFunc.Dispose();
                DtFunc = null;

                return strVal;
            }
            catch (Exception ex)
            {
                strVal = "";
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strVal;
            }
        }





        /// <summary>
        /// 월의 마지막일을 읽어온다.
        /// <param name="ArDate"></param>
        /// 2017-06-05 안정수
        /// </summary>
        /// <returns></returns>
        public string READ_LASTDAY(PsmhDb pDbCon, string ArDate)
        {
            DataTable dt = null;
            string SQL = "";
            string rtnVal = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  TO_CHAR(LAST_DAY(TO_DATE('" + ArDate + "','YYYY-MM-DD')),'YYYY-MM-DD') Lday ";
                SQL += ComNum.VBLF + "FROM DUAL";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count == 1)
                {
                    rtnVal = dt.Rows[0]["LDay"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
                }

                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }


        /// <summary>
        /// Description :Registry 쓰기
        /// Author : 박병규
        /// Create Date : 2017.06.06
        /// <param name="Substr1"></param>
        /// <param name="Substr2"></param>
        /// <param name="Text"></param>
        /// </summary>
        public void Reg_Save_Setting(string Substr1, string Substr2, string Text)
        {
            //CreateSubKey:새 하위키를 만들거나 기존 하위 키를 연다.
            RegistryKey rk = Registry.CurrentUser.CreateSubKey(Substr1, RegistryKeyPermissionCheck.ReadWriteSubTree);
            //SetValue:값 쓰기
            rk.SetValue(Substr2, Text);
            rk.Close();
        }

        /// <summary>
        /// Description : Registry 읽기
        /// Author : 박병규
        /// Create Date : 2017.06.06
        /// <param name="Substr"></param>
        /// <param name="Text"></param>
        /// </summary>
        public string Reg_Get_Setting(string Substr, string Text)
        {
            string strKey = "";

            //OpenSubKey:지정된 하위 키를 검색한다.
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(Substr, true);
            if (rk != null)
            {
                //GetValue:값 가져오기
                strKey = rk.GetValue(Text, false).ToString();

                rk.Close();
            }

            return strKey;
        }

        /// <summary>데이터 테이블 값의 널 여부</summary>
        /// <param name="dt">대상테이블</param>
        /// <returns>Null 일경우 True</returns>
        public static bool isDataTableNull(DataTable dt)
        {
            bool b = false;

            if (dt == null || dt.Rows.Count == 0)
            {
                b = true;
            }

            return b;
        }

        /// <summary>데이터 셋 값 널 여부</summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static bool isDataSetNull(DataSet ds)
        {
            bool b = false;

            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                b = true;
            }


            return b;
        }

        /// <summary>
        /// 사번 -> 이름을 읽어옴
        /// <param name="argSABUN"></param>
        /// 2017-06-10 안정수
        /// /// </summary>
        /// <returns></returns>
        /// 
        #region 테이블 변경으로 인한 주석처리, Read_SabunName 사용하시기 바랍니다.
        //public static string READ_INSA_Name(string argSABUN)
        //{
        //    string rtnVal = "";

        //    string ArgSabun1 = "";
        //    string SQL = "";
        //    DataTable dt = null;
        //    string SqlErr = ""; //에러문 받는 변수


        //    if (Convert.ToInt16(argSABUN) == 0)
        //    {
        //        return rtnVal;
        //    }

        //    ArgSabun1 = argSABUN;

        //    try
        //    {
        //        SQL = "";
        //        SQL = SQL + ComNum.VBLF + "SELECT";
        //        SQL = SQL + ComNum.VBLF + "     KorName , buse";
        //        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST";
        //        SQL = SQL + ComNum.VBLF + "WHERE Sabun='" + ArgSabun1 + "'";
        //        SQL = SQL + ComNum.VBLF + "AND ( TOIDAY IS NULL OR TOIDAY < TRUNC(SYSDATE) )";

        //        SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

        //        if(SqlErr != "")
        //        {
        //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
        //            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
        //            return rtnVal;
        //        }
        //        if (dt.Rows.Count == 0)
        //        {
        //            dt.Dispose();
        //            dt = null;
        //            ComFunc.MsgBox("자료가 등록되지 않았습니다.");
        //        }

        //        if(dt.Rows.Count > 0)
        //        {
        //            rtnVal = dt.Rows[0]["KorName"].ToString().Trim();

        //            switch(rtnVal)
        //            {
        //                case "4444":
        //                    rtnVal = "영양팀";
        //                    break;
        //                case "111":
        //                    rtnVal = "영양팀";
        //                    break;
        //                case "222":
        //                    rtnVal = "영양팀";
        //                    break;
        //                case "333":
        //                    rtnVal = "영양팀";
        //                    break;
        //                case "555":
        //                    rtnVal = "영양팀";
        //                    break;
        //                case "2222":
        //                    rtnVal = "영양팀";
        //                    break;
        //                case "123":
        //                    rtnVal = "영양팀";
        //                    break;
        //                case "500":
        //                    rtnVal = "외래상담용";
        //                    break;
        //                case "4349":
        //                    rtnVal = "전산정보팀";
        //                    break;
        //                case "04349":
        //                    rtnVal = "전산정보팀";
        //                    break;
        //                case "6666":
        //                    rtnVal = "진료의뢰";
        //                    break;
        //                default:
        //                    rtnVal = "";
        //                    break;
        //            }
        //        }

        //        dt.Dispose();
        //        dt = null;
        //        return rtnVal;

        //    }
        //    catch (Exception ex)
        //    {
        //        ComFunc.MsgBox(ex.Message);
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
        //        return rtnVal;
        //    }
        //}
        #endregion

        /// <summary>
        /// Description : 콤보박스에 년도를 세팅한다.
        /// Author : 박병규
        /// Create Date : 2017.06.20
        /// </summary>
        /// <param name="cboVal"></param>
        /// <param name="intCboLen"></param>
        /// <param name="strGubun">
        /// 1 : yyyy
        /// 2 : yyyy년도
        /// </param>
        /// </summary>
        public void SetComboBoxYear(PsmhDb pDbCon, ComboBox o, int intCboLen, string strGubun)
        {
            DateTime dateVal;
            int i = 0;
            int intYY = 0;

            o.DropDownStyle = ComboBoxStyle.DropDownList;
            o.Items.Clear();
            dateVal = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(pDbCon, "D"), "D"));

            intYY = dateVal.Year;

            for (i = 0; i < intCboLen; i++)
            {
                if (strGubun == "1")
                {
                    o.Items.Add((intYY - i).ToString("0000"));
                }
                else if (strGubun == "2")
                {
                    o.Items.Add((intYY - 1).ToString("0000") + "년도");
                }
            }
            o.SelectedIndex = 0;
        }

        /// <summary>
        /// Description : 콤보박스에 년도를 세팅한다.
        /// Author : 김욱동
        /// Create Date : 2019.09.11
        /// </summary>
        /// <param name="cboVal"></param>
        /// <param name="intCboLen"></param>
        /// <param name="strGubun">
        /// 1 : yyyy
        /// 2 : yyyy년도
        /// </param>
        /// </summary>
        public void SetComboBoxYear2(PsmhDb pDbCon, ComboBox o, int intCboLen, string strGubun)
        {
            DateTime dateVal;
            int i = 0;
            int intYY = 0;

            o.DropDownStyle = ComboBoxStyle.DropDownList;
            o.Items.Clear();
            dateVal = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(pDbCon, "D"), "D"));

            intYY = dateVal.Year;

            for (i = 1; i < intCboLen; i++)
            {
                if (strGubun == "1")
                {
                    o.Items.Add((intYY - i).ToString("0000"));
                }
                else if (strGubun == "2")
                {
                    o.Items.Add((intYY - i).ToString("0000") + "년도");
                }
            }
            o.SelectedIndex = 0;
        }


        /// <summary>
        /// Description : 계약처 목록중 계약처명을 가져온다.
        /// Author : 박병규
        /// Create Date : 2017.06.22
        /// <param name="ArgGelCode"></param>
        /// <param name="ArgClass">MIACLASS 조건 사용여부</param>
        /// </summary>
        /// <seealso cref="READ_BAS_MIA" 사용 시 ArgClass = false />

        public string Read_MiaName(PsmhDb pDbCon, string ArgGelCode, bool ArgClass)
        {
            DataTable DtFunc = new DataTable();
            string SQL = String.Empty;
            string SqlErr = String.Empty;
            string strVal = String.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT MIANAME ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_MIA ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "    AND MIACODE = '" + ArgGelCode + "' ";

                if (ArgClass == true)
                    SQL += ComNum.VBLF + "    AND MIACLASS = '90' ";

                SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

                if (DtFunc.Rows.Count == 0)
                {
                    DtFunc.Dispose();
                    DtFunc = null;
                    return strVal;
                }

                if (DtFunc.Rows.Count == 1)
                    strVal = DtFunc.Rows[0]["MIANAME"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strVal;
            }
        }


        /// <summary>
        /// Description : BAS_BCODE TABLE 코드명칭 읽기
        /// Author : 박병규
        /// Create Date : 2017.06.27
        /// <param name="ArgGubun"></param>
        /// <param name="ArgCode"></param>
        /// <param name="ArgGubun2">
        /// 감액자격(J), 감액CASE(C)
        /// </param>
        /// </summary>
        /// <HISTORY>
        /// </HISTORY>
        /// <seealso cref="frmMasterCsinfo.frm:READ_VIP_GUBUN"/> 
        /// <seealso cref="vbBasCode.bas:Read_Cancer"/> 
        /// <seealso cref="vbBasCode.bas:Read_MCodeName"/> 
        /// <seealso cref="OUMSAD.bas:Bas_감액_Name"/> 
        /// <seealso cref="OUMSAD.bas:READ_Bunup_Name"/> 
        /// <seealso cref="vbfunc.bas:Read_환자장애구분"/> 
        /// <seealso cref="vbBasCode.bas:Read_JinDtlName"/> 
        public string Read_Bcode_Name(PsmhDb pDbCon, string ArgGubun, string ArgCode, string ArgGubun2 = "")
        {
            DataTable DtFunc = new DataTable();
            string SQL = String.Empty;
            string SqlErr = String.Empty;
            string strVal = String.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT NAME ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "    AND GUBUN = '" + ArgGubun + "' ";
                SQL += ComNum.VBLF + "    AND CODE  = '" + ArgCode + "' ";

                if (ArgGubun2 != "")
                    SQL += ComNum.VBLF + "    AND GUBUN2  = '" + ArgGubun2 + "' ";

                SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strVal;
            }
        }
        public string Read_Dept_Info(PsmhDb pDbCon, string ArgCode)
        {
            DataTable DtFunc = new DataTable();
            string SQL = String.Empty;
            string SqlErr = String.Empty;
            string strVal = String.Empty;

            try
            {

                SQL = "";
                SQL += ComNum.VBLF + " SELECT count(*) CNT";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_DEPTJEPSU A";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "    AND A.ACTDATE =trunc(sysdate)  ";
                SQL += ComNum.VBLF + "    AND A.DRCODE  = '" + ArgCode + "' ";
                SQL += ComNum.VBLF + "    AND A.JINTIME IS NULL  ";
                SQL += ComNum.VBLF + "    AND (A.SEQ_NO IS NULL OR A.SEQ_NO < 'B')  ";
                SQL += ComNum.VBLF + "    AND A.DEPTJTIME IS NOT NULL  ";


                SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

                if (DtFunc.Rows.Count == 0)
                {
                    DtFunc.Dispose();
                    DtFunc = null;
                    return strVal;
                }

                if (DtFunc.Rows.Count >= 1)
                    strVal = "대기자" + DtFunc.Rows[0]["cnt"].ToString().Trim() + "명";
                else
                    strVal = "대기자 0명";

                DtFunc.Dispose();
                DtFunc = null;

                return strVal;
            }
            catch (Exception ex)
            {
                strVal = "";
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strVal;
            }
        }




        /// <summary>
        /// Description : 환자종류 코드명칭 읽기
        /// Author : 박병규
        /// Create Date : 2017.07.12
        /// <param name="ArgCode"></param>
        /// <param name="ArgType">
        /// 1. 기존명칭
        /// 2. 변경명칭
        /// </param>
        /// </summary>
        /// <seealso cref="vbFunction.bas : READ_Bi_Name"/> 
        /// 
        public string Read_Bi_Name(PsmhDb pDbCon, string ArgCode, string ArgType)
        {
            DataTable DtFunc = new DataTable();
            string SQL = String.Empty;
            string SqlErr = String.Empty;
            string strVal = String.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT NAME, GUBUN2 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "    AND GUBUN = 'BAS_환자종류' ";
                SQL += ComNum.VBLF + "    AND CODE  = '" + ArgCode + "' ";
                SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

                if (DtFunc.Rows.Count == 0)
                {
                    DtFunc.Dispose();
                    DtFunc = null;
                    return strVal;
                }

                if (DtFunc.Rows.Count == 1)
                {
                    if (ArgType == "1")
                        strVal = DtFunc.Rows[0]["NAME"].ToString().Trim();
                    else if (ArgType == "2")
                        strVal = DtFunc.Rows[0]["GUBUN2"].ToString().Trim();
                }
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strVal;
            }
        }
        public int AGE_MONTH_GESAN(PsmhDb pDbCon, string strJuminNo, string strCurrentDate)
        {
            string strBirthDate = "";
            string strSex = "";
            int rtnVal = 120;

            try
            {
                if (VB.Len(VB.Trim(strJuminNo)) != 13)
                {
                    return rtnVal;
                }

                strSex = VB.Mid(strJuminNo, 7, 1);

                switch (strSex)
                {
                    case "1":
                    case "2":
                        strBirthDate = "19";
                        break;

                    case "3":
                    case "4":
                        strBirthDate = "20";
                        break;

                    case "5":
                    case "6":
                        strBirthDate = "19";
                        break;

                    case "7": //외국인 2000년 이후 출생자
                    case "8":
                        strBirthDate = "20";
                        break;

                    case "0":
                    case "9":
                        strBirthDate = "18";
                        break;

                    default:
                        strBirthDate = "19";
                        break;
                }


                strBirthDate = strBirthDate + VB.Left(strJuminNo, 2) + "-" + VB.Mid(strJuminNo, 3, 2) + "-" + VB.Mid(strJuminNo, 5, 2);

                if (ComFunc.CheckBirthDay(strBirthDate) == false)
                {
                    return rtnVal;
                }

                if (strCurrentDate.Equals("") || strCurrentDate.Equals(null))
                {
                    strCurrentDate = clsPublic.GstrSysDate;
                }

                //기준일자가 생년월일보다 적으면 999일로 처리
                if (Convert.ToDateTime(strBirthDate) > Convert.ToDateTime(strCurrentDate))
                {
                    return rtnVal;
                }

                //rtnVal = ComFunc.DATE_ILSU(pDbCon, strCurrentDate, strBirthDate, "");

                if (Convert.ToInt32(VB.Right(VB.Replace(strBirthDate, "-", ""), 4)) > Convert.ToInt32(VB.Right(VB.Replace(strCurrentDate, "-", ""), 4)))
                {
                    //rtnVal = Convert.ToInt32(VB.DateDiff("d", Convert.ToDateTime(strBirthDate), Convert.ToDateTime(strCurrentDate))) ;

                    //rtnVal = 12;

                    rtnVal = Convert.ToInt32(VB.DateDiff("m", Convert.ToDateTime(strBirthDate), Convert.ToDateTime(strCurrentDate)));//2019-01-14, 김해수, 나이 개월 안맞는 부분 수정작업   
                }
                else
                {
                    rtnVal = Convert.ToInt32(VB.DateDiff("m", Convert.ToDateTime(strBirthDate), Convert.ToDateTime(strCurrentDate)));
                }

                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }

        }

        public string Read_AgeSex(string ArginDate, string ArgJumin1, string ArgJumin2)
        {
            string strJindate1 = VB.Left(ArginDate, 4) + VB.Mid(ArginDate, 6, 2) + VB.Right(ArginDate, 2);
            string rtnVal = "";
            int nAge = 0;
            string AgeMonth = "";
            string AgeIlsu = "";


            if (VB.Len(strJindate1) == 8)
            {
                nAge = Convert.ToInt32(ComFunc.AgeCalcEx(ArgJumin1 + ArgJumin2, ArginDate).ToString());
                AgeMonth = AGE_MONTH_GESAN(clsDB.DbCon, ArgJumin1 + ArgJumin2, ArginDate).ToString();

            }

            if (AgeMonth == "120")
            {
                rtnVal = "";
                return rtnVal;
            }
            if (VB.Val(AgeMonth) - (nAge * 12) == 12)  //2019-01-14 김해수 나이계산 로직 재 작업 진행    
            {
                nAge = nAge + 1;
                AgeMonth = "0";
                rtnVal = nAge + "년" + AgeMonth + "개월";
            }
            else
            {
                rtnVal = nAge + "년" + (VB.Val(AgeMonth) - (nAge * 12)) + "개월";
            }



            switch (VB.Mid(ArgJumin2, 1, 1))
            {
                case "2":
                    rtnVal = rtnVal + "/F";
                    break;
                case "4":
                    rtnVal = rtnVal + "/F";
                    break;
                case "6":
                    rtnVal = rtnVal + "/F";
                    break;
                case "8":
                    rtnVal = rtnVal + "/F";
                    break;
                default:
                    rtnVal = rtnVal + "/M";
                    break;
            }

            return rtnVal;
        }


        /// <summary>
        /// Description : ETC_CSINFO_CODE TABLE 코드명칭 읽기
        /// Author : 박병규
        /// Create Date : 2017.07.11
        /// <param name="ArgGubun"></param>
        /// <param name="ArgCode"></param>
        /// </summary>
        /// <seealso cref="vbFunction.bas : READ_JIKUP_NAME"/> 
        /// <seealso cref="Csinfo00.bas : READ_CSGubun_Name"/> 
        public string Read_Csinfo_Name(PsmhDb pDbCon, string ArgGubun = "", string ArgCode = "")
        {
            DataTable DtFunc = new DataTable();
            string SQL = String.Empty;
            string SqlErr = String.Empty;
            string strVal = String.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT NAME ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_CSINFO_CODE ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";

                if (ArgGubun != "")
                {
                    SQL += ComNum.VBLF + "    AND GUBUN = '" + ArgGubun + "' ";
                }

                if (ArgCode != "")
                {
                    SQL += ComNum.VBLF + "    AND CODE  = '" + ArgCode + "' ";
                }

                SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strVal;
            }
        }

        /// <summary>
        /// Description : 회사명 코드명칭 읽기
        /// Author : 박병규
        /// Create Date : 2017.07.12
        /// <param name="ArgCode"></param>
        /// </summary>
        public string Read_Ltd_Name(PsmhDb pDbCon, string ArgCode)
        {
            DataTable DtFunc = new DataTable();
            string SQL = String.Empty;
            string SqlErr = String.Empty;
            string strVal = String.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT NAME ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "HIC_LTD ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "    AND CODE  = '" + ArgCode + "' ";
                SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

                if (DtFunc.Rows.Count == 0)
                {
                    DtFunc.Dispose();
                    DtFunc = null;
                    return strVal;
                }

                if (DtFunc.Rows.Count == 1)
                {
                    strVal = DtFunc.Rows[0]["NAME"].ToString().Trim();
                }
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strVal;
            }
        }


        /// <summary>
        /// Description : ComboBox에 접수가능한 진료과목 셋팅
        /// Author : 박병규
        /// Create Date : 2017.07.19
        /// Modify Date : 2017.08.17
        /// <param name="ArgCode"></param>
        /// <param name="ArgAll"></param>
        /// <param name="ArgType"></param>
        /// </summary>
        /// <seealso cref="vbFunction.bas : ComboDept_SET"/> 
        public void COMBO_DEPT_SET(PsmhDb pDbCon, ComboBox ArgCombo, string ArgAll = "", string ArgType = "1")
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";

            if (ArgAll == "")
                ArgAll = "1";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT DEPTCODE, DEPTNAMEK";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND GBJUPSU   = '1' ";
            SQL += ComNum.VBLF + "  ORDER BY PRINTRANKING ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            ArgCombo.Items.Clear();

            if (ArgAll == "1")
            {
                switch (ArgType)
                {
                    case "1":
                        ArgCombo.Items.Add("**.전체");
                        break;
                    case "2":
                        ArgCombo.Items.Add("**");
                        break;
                    case "3":
                        ArgCombo.Items.Add("전체");
                        break;
                }
            }

            for (int i = 0; i < DtFunc.Rows.Count; i++)
            {
                switch (ArgType)
                {
                    case "1":
                        ArgCombo.Items.Add(DtFunc.Rows[i]["DeptCode"].ToString().Trim() + "." + DtFunc.Rows[i]["DeptNameK"].ToString().Trim());
                        break;
                    case "2":
                        ArgCombo.Items.Add(DtFunc.Rows[i]["DeptCode"].ToString().Trim());
                        break;
                    case "3":
                        ArgCombo.Items.Add(DtFunc.Rows[i]["DeptNamek"].ToString().Trim());
                        break;
                }
            }

            ArgCombo.SelectedIndex = 0;

            DtFunc.Dispose();
            DtFunc = null;
        }


        /// <summary>
        /// Description : ComboBox에 진료과목의 해당 의사 목록 셋팅
        /// Author : 박병규
        /// Create Date : 2017.07.27
        /// <param name="ArgCode"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgAll"></param>
        /// <param name="ArgType"></param>
        /// </summary>
        /// <seealso cref="vbFunction.bas : ComboDept_SET"/> 
        public void ComboDr_Set(PsmhDb pDbCon, ComboBox ArgCombo, string ArgDept, string ArgAll = "", string ArgType = "1")
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";

            if (ArgAll == "")
            {
                ArgAll = "1";
            }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT * ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND DRDEPT1   = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND TOUR      = 'N' ";
            SQL += ComNum.VBLF + "  ORDER BY DRCODE ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            ArgCombo.Items.Clear();

            if (ArgAll == "1")
            {
                switch (ArgType)
                {
                    case "1":
                        ArgCombo.Items.Add("**.전체");
                        break;
                    case "2":
                        ArgCombo.Items.Add("**");
                        break;
                    case "3":
                        ArgCombo.Items.Add("전체");
                        break;
                }
            }

            for (int i = 0; i < DtFunc.Rows.Count; i++)
            {
                switch (ArgType)
                {
                    case "1":
                        ArgCombo.Items.Add(DtFunc.Rows[i]["DRCODE"].ToString().Trim() + "." + DtFunc.Rows[i]["DRNAME"].ToString().Trim());
                        break;
                    case "2":
                        ArgCombo.Items.Add(DtFunc.Rows[i]["DRCODE"].ToString().Trim());
                        break;
                    case "3":
                        ArgCombo.Items.Add(DtFunc.Rows[i]["DRNAME"].ToString().Trim());
                        break;
                }
            }

            ArgCombo.SelectedIndex = 0;

            DtFunc.Dispose();
            DtFunc = null;
        }


        /// <summary>
        /// Description : 바코드 스캐너
        /// Author : 박병규
        /// Create Date : 2017.07.25
        /// <param name="ArgBarCode"></param>
        /// </summary>
        /// <seealso cref="vbFunction.bas : READ_BARCODE"/> 
        public bool READ_BARCODE(string ArgBarCode)
        {
            bool rtnVal = false;

            clsPublic.GstrBarPano = "";
            clsPublic.GstrBarDept = "";

            if (ArgBarCode.Length == 12)
            {
                clsPublic.GstrBarPano = string.Format("{0:D8}", Convert.ToInt32(VB.Left(ArgBarCode, 8)));

                char character1 = (char)Convert.ToInt32(VB.Mid(ArgBarCode, 9, 2));
                char character2 = (char)Convert.ToInt32(VB.Right(ArgBarCode, 2));
                clsPublic.GstrBarDept = character1.ToString() + character2.ToString();


                if (clsPublic.GstrBarDept == "RA")
                    clsPublic.GstrBarDept = "MD";

                rtnVal = true;
            }
            else
            {
                rtnVal = false;
            }

            return rtnVal;
        }


        /// <summary>
        /// Description : 진료과명 가져오기
        /// Author : 박병규
        /// Create Date : 2017.07.26
        /// <param name="ArgCode">진료과목코드</param>
        /// </summary>
        /// <seealso cref="vbFunction.bas : READ_BAS_ClinicDeptNameK"/> 
        /// <seealso cref="IUMENT1.bas : READ_BAS_DeptName"/> 
        public string READ_DEPTNAMEK(PsmhDb pDbCon, string ArgCode)
        {
            DataTable DtFunc = null;
            string rtnVal = string.Empty;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            if (ArgCode == "")
            {
                return rtnVal;
            }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT DeptNameK";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "    AND DEPTCODE = '" + ArgCode + "' ";
                SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                }

                if (DtFunc.Rows.Count > 0)
                    rtnVal = DtFunc.Rows[0]["DeptNameK"].ToString().Trim();
                else
                    return rtnVal;

                DtFunc.Dispose();
                DtFunc = null;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }

            return rtnVal;
        }

        /// <summary>
        /// Description : 진료과명 가져오기
        /// Author : 박웅규
        /// Create Date : 2017.07.26
        /// <param name="ArgCode">의사코드</param>
        /// </summary>
        /// <seealso cref="vbFunction.bas : READ_BAS_Doctor"/> 
        public string ReadBASDoctor(PsmhDb pDbCon, string argDrCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";
            DataTable dt = null;

            if (argDrCode.Trim() == "")
            {
                rtnVal = "";
            }
            try
            {

                SQL = "";
                SQL += ComNum.VBLF + "SELECT DrName FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL += ComNum.VBLF + " WHERE DrCode='" + argDrCode + "' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["DrName"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        /// <summary>
        /// ROWID를 찾아 등록 및 수정 한다.
        /// 2017-06-26 안정수
        /// </summary>
        /// <param name="ArgCode"></param>
        /// <param name="ArgName"></param>
        public void EMR_CLINIC_RTN(PsmhDb pDbCon, string ArgCode, string ArgName)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            if (ArgCode.Trim() == "")
            {
                return;
            }

            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  ROWID";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMR_CLINICT";
                SQL += ComNum.VBLF + "WHERE CLINCODE = '" + ArgCode + "'";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "UPDATE ";
                    SQL += ComNum.VBLF + ComNum.DB_EMR + "EMR_CLINICT SET";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMR_CLINICT";
                    SQL += ComNum.VBLF + "NAME = '" + ArgName.Trim() + "'";
                    SQL += ComNum.VBLF + "WHERE ROWID = '" + dt.Rows[0]["ROWID"].ToString().Trim() + "' ";
                }
                else
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "INSERT INTO ";
                    SQL += ComNum.VBLF + ComNum.DB_EMR + "EMR_CLINICT";
                    SQL += ComNum.VBLF + "(CLINCODE, NAME, OCRCODE, ORDERBY, ACTIVE,CONSYN)";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "DEPTCODE, DEPTNAMEK, DEPTCODE, '9999', '1','N' ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
                    SQL += ComNum.VBLF + "WHERE DEPTCODE = '" + ArgCode + "' ";
                    SQL += ComNum.VBLF + "UNION ALL";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  BUCODE, NAME, BUCODE, '9999', '1','N' ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BUSE";
                    SQL += ComNum.VBLF + "WHERE INSA= '*' ";
                    SQL += ComNum.VBLF + "  AND BUCODE = '" + ArgCode + "' ";
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                dt.Dispose();
                dt = null;

                clsDB.setCommitTran(pDbCon);
            }

            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        /// <summary>
        /// 주소를 읽어 온다.
        /// 2017-06-26 안정수
        /// </summary>
        /// <param name="ArgCode"></param>
        /// <returns></returns>
        public string READ_BAS_Mail(PsmhDb pDbCon, string ArgCode)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (ArgCode.Trim() == "" || ArgCode == "000000")
            {
                return rtnVal;
            }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  MailJuso";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_MAILNEW";
                ;
                SQL += ComNum.VBLF + "WHERE MailCode='" + ArgCode + "'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["MailJuso"].ToString().Trim();
                }
                else
                {
                    return rtnVal;
                }

                dt.Dispose();
                dt = null;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }

            return rtnVal;
        }

        /// <summary>
        /// 수가이름을 읽어온다
        /// 2017-06-26 안정수
        /// </summary>
        /// <param name="ArgCode"></param>
        /// <returns></returns>
        public string READ_SugaName(PsmhDb pDbCon, string ArgCode)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  SuNameK";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN";
                SQL += ComNum.VBLF + "WHERE SuNext = '" + ArgCode + "' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["SuNameK"].ToString().Trim();
                }
                else
                {
                    return rtnVal;
                }

                dt.Dispose();
                dt = null;

            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }

            return rtnVal;
        }

        /// <summary>
        /// 2017-06-26 안정수
        /// </summary>
        /// <param name="ArDate"></param>
        /// <param name="ArgIlsu"></param>
        /// <returns></returns>
        public string DATE_ADD(PsmhDb pDbCon, string ArDate, int ArgIlsu)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            if (VB.Len(ArDate) != 10)
            {
                return rtnVal;
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(TO_DATE('" + ArDate + "','YYYY-MM-DD')";
                if (ArgIlsu < 0)
                {
                    SQL = SQL + ComNum.VBLF + "-" + ArgIlsu * -1;
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "+" + ArgIlsu;
                }
                SQL = SQL + ComNum.VBLF + ",'YYYY-MM-DD') AddDate";
                SQL = SQL + ComNum.VBLF + "FROM DUAL";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                }

                if (dt.Rows.Count == 1)
                {
                    rtnVal = dt.Rows[0]["AddDate"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
                }

                dt.Dispose();
                dt = null;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
            }

            return rtnVal;
        }


        /// <summary>
        /// 폼 로딩 사용 빈도
        /// 2017-06-30 안정수
        /// </summary>
        /// <param name="argFormName"></param>
        /// <param name="argFormTitle"></param>
        /// <param name="GstrIpAddress"></param>
        /// <param name="GstrJobSabun"></param>
        /// <param name="GstrJobPart"></param>
        public void FormInfo_History(PsmhDb pDbCon, string argFormName, string argFormTitle, string GstrIpAddress, string GstrJobSabun, string GstrJobPart)
        {
            if (GstrIpAddress == "" || GstrJobSabun == "" || GstrJobPart == "")
            {
                return;
            }

            string strEXEName = @"C:\PSMHEXE";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "INSERT INTO";
                SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_FormInfo";
                SQL += ComNum.VBLF + "(SAbun, ChulTime, ExeFile, FormName, FormTitle, IPADDR)";
                SQL += ComNum.VBLF + "VALUES(";
                SQL += ComNum.VBLF + "  '" + GstrJobPart + "',  ";
                SQL += ComNum.VBLF + "      SYSDATE,            ";
                SQL += ComNum.VBLF + "  '" + strEXEName + "',   ";
                SQL += ComNum.VBLF + "  '" + argFormName + "',  ";
                SQL += ComNum.VBLF + "  '" + argFormTitle + "', ";
                SQL += ComNum.VBLF + "  '" + GstrIpAddress + "' ";
                SQL += ComNum.VBLF + ")";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }

                clsDB.setCommitTran(pDbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }

            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// DATE_TIME(종료시각, 시작시각)
        /// 2017-06-30 안정수
        /// </summary>
        /// <param name="ArgFtime"></param>
        /// <param name="ArgTtime"></param>
        /// <returns></returns>
        public int DATE_TIME(PsmhDb pDbCon, string ArgFtime, string ArgTtime)
        {
            int rtnVal = 0;
            int ArgIlsu = 0;
            int ArgTime = 0;

            //반드시 시각이 "YYYY-MM-DD HH24:MI" 형태이어야 함

            if (VB.Len(ArgFtime.Trim()) != 16 || VB.Len(ArgTtime.Trim()) != 16)
            {
                return rtnVal;
            }

            try
            {
                //일수를 계산
                ArgIlsu = DATE_ILSU(pDbCon, VB.Left(ArgTtime, 10), VB.Left(ArgFtime, 10));
                ArgTime = ArgIlsu * 1440; // 1일은 1440분(24h * 60)
                ArgTime += (Convert.ToInt32(VB.Mid(ArgTtime, 12, 2)) * 60) + Convert.ToInt32(VB.Right(ArgTtime, 2)); // 종료시각
                ArgTime = ArgTime - (Convert.ToInt32(VB.Mid(ArgFtime, 12, 2)) * 60) - (Convert.ToInt32(VB.Right(ArgFtime, 2))); // 시작시각

                rtnVal = ArgTime;
            }
            catch
            {
            }

            return rtnVal;
        }

        /// <summary>
        /// 2017-06-30 안정수
        /// </summary>
        /// <param name="ArgTdate"></param>
        /// <param name="ArgFdate"></param>
        /// <param name="ArgGb"></param>
        /// <returns></returns>
        public int DATE_ILSU(PsmhDb pDbCon, string ArgTdate, string ArgFdate, string ArgGb = "")
        {
            int rtnVal = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (VB.Len(ArgFdate.Trim()) != 10 || VB.IsDate(ArgFdate) == false
                                              || VB.Len(ArgTdate.Trim()) != 10 || VB.IsDate(ArgFdate) == false)
            {
                return rtnVal;
            }

            if (String.Compare(ArgFdate, ArgTdate) > 0)
            {
                return rtnVal;
            }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  TO_DATE('" + ArgTdate + "','YYYY-MM-DD') - ";
                SQL += ComNum.VBLF + "  TO_DATE('" + ArgFdate + "','YYYY-MM-DD') Gigan";
                SQL += ComNum.VBLF + "FROM DUAL";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                }

                if (dt.Rows.Count == 1)
                {
                    rtnVal = Convert.ToInt32(dt.Rows[0]["Gigan"].ToString().Trim());
                }

                if (ArgGb != "ALL")
                {
                    if (rtnVal >= 1000) // 일수 계산 제한 옵션으로 풀도록 함수 수정
                    {
                        rtnVal = 999;
                    }
                }

                dt.Dispose();
                dt = null;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }


            return rtnVal;
        }

        /// <summary>
        /// 2017-07-10 안정수, Password 관리 테이블에서 사용자이름을 읽어온다.
        /// </summary>
        /// <param name="ArgSabun"></param>
        /// <returns></returns>
        public string READ_PassName(PsmhDb pDbCon, string ArgSabun)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (ArgSabun == "")
            {
                return rtnVal;
            }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  Name";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PASS";
                SQL += ComNum.VBLF + "  WHERE 1=1";
                SQL += ComNum.VBLF + "      AND IDnumber= " + Convert.ToInt32(ArgSabun) + " ";
                SQL += ComNum.VBLF + "      AND PrograMid = ' '  ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["Name"].ToString().Trim();
                }
                else
                {
                    return rtnVal;
                }

                dt.Dispose();
                dt = null;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }


            return rtnVal;
        }

        //외부판독의사 면허번호 가져오기
        public string READ_OutPanDoctorDRNO(PsmhDb pDbCon, string ArgSabun)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (ArgSabun == "")
            {
                return rtnVal;
            }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  GUBUN2";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL += ComNum.VBLF + "  WHERE 1=1";
                SQL += ComNum.VBLF + "      AND CODE= " + Convert.ToInt32(ArgSabun) + " ";
                SQL += ComNum.VBLF + "      AND GUBUN = 'XRAY_외주판독의사'  ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["GUBUN2"].ToString().Trim();
                }
                else
                {
                    return rtnVal;
                }

                dt.Dispose();
                dt = null;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }


            return rtnVal;
        }
        /// <summary>
        /// 2017-07-10 안정수, 의사명을 읽어온다
        /// </summary>
        /// <param name="ArgCode"></param>
        /// <returns></returns>
        public string READ_DrName(PsmhDb pDbCon, string ArgCode)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable DtFunc = null;

            if (ArgCode == "")
            {
                return rtnVal;
            }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  DrName";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_DOCTOR";
                SQL += ComNum.VBLF + "  WHERE 1=1";
                SQL += ComNum.VBLF + "      AND DrCode= " + ArgCode + " ";

                SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (DtFunc.Rows.Count > 0)
                {
                    rtnVal = DtFunc.Rows[0]["DrName"].ToString().Trim();
                }

                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }


            return rtnVal;
        }

        /// <summary>
        /// 2017-07-11 안정수, 콤보박스 아이템 추가(조회년월)
        /// </summary>
        /// <param name="cbo"></param>
        /// <param name="ArgMonthCNT"></param>
        public void ComboMonth_Set(ComboBox Argcbo, int ArgMonthCNT)
        {
            int i = 0;
            int ArgYY = 0;
            int ArgMM = 0;

            ArgYY = Convert.ToInt16(DateTime.Now.ToString("yyyy"));
            ArgMM = Convert.ToInt16(DateTime.Now.ToString("MM"));
            Argcbo.Items.Clear();

            for (i = 1; i <= ArgMonthCNT; i++)
            {
                Argcbo.Items.Add(ArgYY + "년 " + ComFunc.SetAutoZero(ArgMM.ToString(), 2) + "월분");

                ArgMM -= 1;
                if (ArgMM == 0)
                {
                    ArgMM = 12;
                    ArgYY -= 1;
                }
            }
            Argcbo.SelectedIndex = 0;
        }

        /// <summary>
        /// 2017-07-13 안정수, 콤보박스 아이템 추가(조회년월)
        /// </summary>
        /// <param name="ArgCombo"></param>
        /// <param name="ArgMonthCNT"></param>
        public void ComboMonth_Set1(ComboBox ArgCombo, int ArgMonthCNT)
        {
            int i = 0;
            int ArgYY = 0;
            int ArgMM = 0;


            ArgYY = Convert.ToInt16(DateTime.Now.ToString("yyyy"));
            ArgMM = Convert.ToInt16(DateTime.Now.ToString("MM"));
            ArgCombo.Items.Clear();

            for (i = 1; i < ArgMonthCNT; i++)
            {
                ArgCombo.Items.Add(ArgYY + "-" + ComFunc.SetAutoZero(ArgMM.ToString(), 2));
                ArgMM -= 1;
                if (ArgMM == 0)
                {
                    ArgMM = 12;
                    ArgYY -= 1;
                }
            }

            ArgCombo.SelectedIndex = 0;

        }

        /// <summary>
        /// 2017-07-13 안정수, 콤보박스 아이템(조회년월)을 추가
        /// </summary>
        /// <param name="ArgCombo"></param>
        /// <param name="ArgMonthCNT"></param>
        /// <param name="ArgYYMM"></param>
        public void ComboMonth_Set2(ComboBox ArgCombo, int ArgMonthCNT, string ArgYYMM = "")
        {
            int i = 0;
            int ArgYY = 0;
            int ArgMM = 0;


            ArgYY = Convert.ToInt16(DateTime.Now.ToString("yyyy"));
            ArgMM = Convert.ToInt16(DateTime.Now.ToString("MM"));
            ArgCombo.Items.Clear();

            for (i = 1; i < ArgMonthCNT; i++)
            {
                ArgCombo.Items.Add(ArgYY + "년 " + ComFunc.SetAutoZero(ArgMM.ToString(), 2) + "월분");
                ArgMM -= 1;
                if (ArgMM == 0)
                {
                    ArgMM = 12;
                    ArgYY -= 1;
                }
            }

            ArgCombo.SelectedIndex = 0;
        }

        /// <summary>
        /// 지정 년, 월부터 ArgMonthCNT 까지 1씩 증가하면서 ComboBox 세팅
        /// </summary>
        /// <param name="ArgCombo">대상 ComboBox Control</param>
        /// <param name="ArgMonthCNT">표시할 개월 수</param>
        /// <param name="argYear">지정 년</param>
        /// <param name="argMonth">지정 월</param>
        public void ComboMonth_Set3(ComboBox ArgCombo, int ArgMonthCNT, int argYear, int argMonth)
        {
            int i = 0;
            int ArgYY = 0;
            int ArgMM = 0;


            ArgYY = argYear;
            ArgMM = argMonth;
            ArgCombo.Items.Clear();

            for (i = 1; i < ArgMonthCNT; i++)
            {
                ArgCombo.Items.Add(ArgYY + "-" + ComFunc.SetAutoZero(ArgMM.ToString(), 2));
                ArgMM += 1;
                if (ArgMM == 13)
                {
                    ArgMM = 1;
                    ArgYY += 1;
                }
            }

            ArgCombo.SelectedIndex = 0;
        }

        /// <summary>
        /// 2017-07-13 박웅규, 해당프로그램을 강제로 죽인다.
        /// </summary>
        /// <param name="strEXE">프로그램명</param>
        public static void KillProc(string strEXE)
        {
            strEXE = strEXE.ToUpper().Replace(".EXE", "");

            System.Diagnostics.Process[] ProcessEx = System.Diagnostics.Process.GetProcessesByName(strEXE);
            if (ProcessEx.Length > 0)
            {
                System.Diagnostics.Process[] Pro1 = System.Diagnostics.Process.GetProcessesByName(strEXE);
                System.Diagnostics.Process CurPro = System.Diagnostics.Process.GetCurrentProcess();
                foreach (System.Diagnostics.Process Proc in Pro1)
                {
                    if (Proc.Id != CurPro.Id)
                    {
                        Proc.Kill();
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(500);
                        Application.DoEvents();
                        Application.DoEvents();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 2017-07-13 박웅규, 해당프로그램을 실행한다
        /// </summary>
        /// <param name="strExePath">프로그램명(경로)</param>
        /// <param name="strGb">구분:NET, COM</param>
        /// <param name="strFocus">프로그램명</param>
        /// <param name="arguments">파라메타</param>
        public static void StartProc(string strExePath, string strGb = "NET", string strFocus = "", string arguments = "")
        {
            if (strGb == "COM")
            {
                System.Diagnostics.Process program = System.Diagnostics.Process.Start(strExePath, arguments);
            }
            else
            {
                System.Diagnostics.Process program = System.Diagnostics.Process.Start(strExePath, arguments);
            }
        }


        /// <summary>
        /// /// Description : 선택진료 기본마스터정보읽기
        /// Create Date : 2017.07.19
        /// Author : 김효성
        /// Update Date : 2017.08.07 박병규
        /// </summary>
        /// <param name="ArgIO"></param>
        /// <param name="ArgDrCode"></param>
        /// <param name="ArgBdate"></param>
        /// <seealso cref="Vb선택진료 : Vb선택진료"/>
        /// <seealso cref="Vb선택진료 : Read_SELECT_DOCTOR_CHK"/>
        /// <returns>
        /// </returns>
        public static string READ_SELECT_DOCTOR_CHK(PsmhDb pDbCon, string ArgIO, string ArgDrCode, string ArgBdate = "")
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable DtFunc = null;

            ComFunc.ReadSysDate(pDbCon);

            if (ArgBdate == "")
                ArgBdate = clsPublic.GstrSysDate;

            //2014-02-01 부로 외과 하동엽과장 선택진료 실시하므로 그날 이전 기준은 선택안물리게 함.
            if (string.Compare(ArgBdate, "2014-02-01") < 0)
            {
                if (ArgDrCode == "2119")
                    return rtnVal;
            }

            //2016-05-23 외래 산부인과 김도균과장 토요일은 선택진료 안물림
            if (string.Compare(ArgBdate, "2016-06-08") >= 0)
            {
                if (ArgIO == "0" && ArgDrCode == "3111" && clsVbfunc.GetYoIl(ArgBdate) == "토요일")
                    return rtnVal;
            }

            //선택진료 기본마스터 정보 읽기
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT ROWID ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND DRCODE    = '" + ArgDrCode + "' ";
                SQL += ComNum.VBLF + "    AND GBCHOICE  = 'Y' ";
                SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                    DtFunc.Dispose();
                    DtFunc = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (DtFunc.Rows.Count > 0)
                    rtnVal = "OK";

                DtFunc.Dispose();
                DtFunc = null;
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 2017-07-19 김효성
        /// </summary>
        /// <param name="ArgPano"></param>
        /// <param name="ArgIO"></param>
        /// <param name="ArgDrCode"></param>
        /// <param name="argBDATE"></param>
        /// <param name="ArgIpdNo"></param>
        /// <seealso cref="Vb선택진료 : Vb선택진료"/>
        /// <seealso cref="Vb선택진료 : Read_Pano_SELECT_MST"/>
        /// <returns></returns>
        public static string Read_Pano_SELECT_MST(PsmhDb pDbCon, string ArgPano, string ArgIO, string ArgDrCode, string argBDATE, long ArgIpdNo = 0)
        {
            string strrtn = "";
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            if (ArgIO == "I")
            {
                if (READ_IPD_NEW_MASTER_INDATE_CHK(pDbCon, ArgIpdNo) != "OK")
                {
                    strrtn = "OK";
                    return strrtn;
                }
            }

            try
            {
                SQL = " SELECT Pano, ";
                SQL += ComNum.VBLF + " TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, ";
                SQL += ComNum.VBLF + " TO_CHAR(EDATE,'YYYY-MM-DD') EDATE ";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_SELECT_MST";
                SQL += ComNum.VBLF + "  WHERE PANO ='" + ArgPano + "' ";
                SQL += ComNum.VBLF + "   AND DRCODE ='" + ArgDrCode + "' ";
                SQL += ComNum.VBLF + "   AND GUBUN ='" + ArgIO + "' ";
                SQL += ComNum.VBLF + "   AND SDate <=TO_DATE('" + argBDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND (DelDate IS NULL OR DelDate ='') ";
                SQL += ComNum.VBLF + "  ORDER BY SDate DESC     ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생하였습니다.");
                    return strrtn;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["EDATE"].ToString().Trim() == "")
                    {
                        strrtn = "OK";
                        return strrtn;
                    }
                    else if (Convert.ToDateTime(argBDATE) <= Convert.ToDateTime(dt.Rows[0]["EDATE"].ToString().Trim()))
                    {
                        strrtn = "OK";
                        return strrtn;
                    }
                    else
                    {
                        strrtn = "";
                        return strrtn;
                    }

                    strrtn = "OK";
                    return strrtn;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return strrtn;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return strrtn;
            }
        }

        /// <summary>
        /// 2017-07-19 김효성
        /// </summary>
        /// <param name="ArgIpdNo"></param>
        /// <seealso cref="Vb선택진료 : Vb선택진료"/>
        /// <seealso cref="Vb선택진료 : READ_IPD_NEW_MASTER_INDATE_CHK"/>
        /// <returns></returns>
        public static string READ_IPD_NEW_MASTER_INDATE_CHK(PsmhDb pDbCon, long ArgIpdNo)
        {
            string strrtn = "";
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                SQL = "SELECT Pano,IPDNO ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL += ComNum.VBLF + " WHERE IPDNO=" + ArgIpdNo + " ";
                SQL += ComNum.VBLF + "  AND INDATE >=TO_DATE('2011-06-01 00:01','YYYY-MM-DD HH24:MI')  ";
                SQL += ComNum.VBLF + "  AND GBSTS NOT IN ('9') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생하였습니다.");
                    return strrtn;
                }

                if (dt.Rows.Count > 0)
                {
                    strrtn = "OK";
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return strrtn;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return strrtn;
            }
        }

        /// <summary>
        /// Create Date : 2017.07.20
        /// Author : 김효성
        /// Update Date : 2017.08.07 박병규
        /// </summary>
        /// <param name="ArgPan"></param>
        /// <param name="ArgIO"></param>
        /// <param name="ArgDrCode"></param>
        /// <param name="ArgBDate"></param>
        /// <seealso cref="Vb선택진료 : Vb선택진료"/>
        /// <seealso cref="Vb선택진료 : Read_Pano_SELECT_MST_BDate"/>
        /// <returns></returns>
        public static string READ_PANO_SELECT_MST_BDATE(PsmhDb pDbCon, string ArgPano, string ArgIO, string ArgDrCode, string ArgBDate)
        {
            string SQL = "";
            string rtnVal = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable DtFunc = null;

            //'선택진료 기본마스터 정보 읽기
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, ";
                SQL += ComNum.VBLF + "        TO_CHAR(EDATE,'YYYY-MM-DD') EDATE ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SELECT_MST";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPano + "' ";
                SQL += ComNum.VBLF + "    AND DRCODE    = '" + ArgDrCode + "' ";
                SQL += ComNum.VBLF + "    AND GUBUN     = '" + ArgIO + "' ";
                SQL += ComNum.VBLF + "    AND SDate     <= TO_DATE('" + ArgBDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND (DelDate IS NULL OR DelDate ='') ";
                SQL += ComNum.VBLF + "  ORDER BY SDate DESC     ";
                SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    DtFunc.Dispose();
                    DtFunc = null;
                    return rtnVal;
                }

                if (DtFunc.Rows.Count > 0)
                {
                    if (DtFunc.Rows[0]["EDATE"].ToString().Trim() == "")
                    {
                        rtnVal = DtFunc.Rows[0]["SDATE"].ToString().Trim();
                        return rtnVal;
                    }
                    else if (string.Compare(ArgBDate, DtFunc.Rows[0]["SDATE"].ToString().Trim()) <= 0)
                    {
                        rtnVal = DtFunc.Rows[0]["SDATE"].ToString().Trim();
                        return rtnVal;
                    }
                    else
                    {
                        rtnVal = "";
                        return rtnVal;
                    }
                }
                DtFunc.Dispose();
                DtFunc = null;
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 2017-07-21 안정수, 콤보박스에 BCode 관련 항목을 추가한다.
        /// </summary>
        /// <param name="ArgCombobox"></param>
        /// <param name="argGubun"></param>
        /// <param name="ArgClear"></param>
        /// <param name="ArgTYPE"></param>
        /// <param name="ArgNULL"></param>
        public void Combo_BCode_SET(PsmhDb pDbCon, ComboBox ArgCombobox, string argGubun, bool ArgClear, int ArgTYPE, string ArgNULL = "")
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            if (ArgClear == true)
            {
                ArgCombobox.Items.Clear();
            }

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT                                               ";
            SQL = SQL + ComNum.VBLF + "     Sort,Code,Name                                  ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE                 ";
            SQL = SQL + ComNum.VBLF + "     WHERE 1=1                                       ";
            SQL = SQL + ComNum.VBLF + "   AND Gubun = '" + argGubun + "'                    ";
            SQL = SQL + ComNum.VBLF + "   AND (DelDate IS NULL OR DelDate > TRUNC(SYSDATE)) ";
            SQL = SQL + ComNum.VBLF + "ORDER BY Sort,Code                                   ";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
            }

            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (ArgNULL != "N")
            {
                ArgCombobox.Items.Add(" ");
            }

            if (dt.Rows.Count > 0)
            {

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (ArgTYPE == 1)
                    {
                        ArgCombobox.Items.Add(dt.Rows[i]["Code"].ToString().Trim() + "." + dt.Rows[i]["Name"].ToString().Trim());
                    }

                    else if (ArgTYPE == 2)
                    {
                        ArgCombobox.Items.Add(dt.Rows[i]["Code"].ToString().Trim());
                    }

                    else if (ArgTYPE == 3)
                    {
                        ArgCombobox.Items.Add(dt.Rows[i]["Name"].ToString().Trim());
                    }
                }
            }
            else
            {
                ComFunc.MsgBox("조회중 문제가 발생하였습니다.");
                return;
            }
            dt.Dispose();
            dt = null;


        }

        /// <summary>
        /// 콤보박스에 BCode 관련 항목을 추가한다. Override
        /// Gubun2 컬럼 조건추가  2018-01-02  KMC
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgCombobox"></param>
        /// <param name="argGubun"></param>
        /// <param name="ArgClear"></param>
        /// <param name="ArgTYPE"></param>
        /// <param name="ArgGubun2"></param>
        /// <param name="ArgNULL"></param>
        public void Combo_BCode_SET(PsmhDb pDbCon, ComboBox ArgCombobox, string argGubun, bool ArgClear, int ArgTYPE, string ArgGubun2, string ArgNULL = "")
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            if (ArgClear == true)
            {
                ArgCombobox.Items.Clear();
            }

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT                                               ";
            SQL = SQL + ComNum.VBLF + "     Sort,Code,Name                                  ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE                 ";
            SQL = SQL + ComNum.VBLF + "     WHERE 1=1                                       ";
            SQL = SQL + ComNum.VBLF + "   AND Gubun = '" + argGubun + "'                    ";
            SQL = SQL + ComNum.VBLF + "   AND (DelDate IS NULL OR DelDate > TRUNC(SYSDATE)) ";
            SQL = SQL + ComNum.VBLF + "ORDER BY Sort,Code                                   ";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
            }

            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (ArgNULL != "N")
            {
                ArgCombobox.Items.Add(" ");
            }

            if (dt.Rows.Count > 0)
            {

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (ArgTYPE == 1)
                    {
                        ArgCombobox.Items.Add(dt.Rows[i]["Code"].ToString().Trim() + "." + dt.Rows[i]["Name"].ToString().Trim());
                    }

                    else if (ArgTYPE == 2)
                    {
                        ArgCombobox.Items.Add(dt.Rows[i]["Code"].ToString().Trim());
                    }

                    else if (ArgTYPE == 3)
                    {
                        ArgCombobox.Items.Add(dt.Rows[i]["Name"].ToString().Trim());
                    }
                }
            }
            else
            {
                ComFunc.MsgBox("조회중 문제가 발생하였습니다.");
                return;
            }
            dt.Dispose();
            dt = null;


        }

        /// <summary>
        /// 2017-10-13 박병규, 콤보박스에 BCode중 국적코드을 추가한다.
        /// </summary>
        /// <param name="ArgCombobox"></param>
        /// <param name="argGubun"></param>
        /// <param name="ArgClear"></param>
        /// <param name="ArgTYPE"></param>
        /// <param name="ArgNULL"></param>
        public void Combo_Nat_BCode_SET(PsmhDb pDbCon, ComboBox ArgCombobox, string argGubun, bool ArgClear, int ArgTYPE, string ArgNULL = "")
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            if (ArgClear == true)
            {
                ArgCombobox.Items.Clear();
            }

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT Sort,Code,Name ";
            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL = SQL + ComNum.VBLF + "  WHERE 1        = 1 ";
            SQL = SQL + ComNum.VBLF + "    AND Gubun    = '" + argGubun + "' ";
            SQL = SQL + ComNum.VBLF + "    AND (DelDate IS NULL OR DelDate > TRUNC(SYSDATE)) ";
            SQL = SQL + ComNum.VBLF + "ORDER BY decode(code,'KR',1,2) ";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
            }

            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (ArgNULL != "N")
            {
                ArgCombobox.Items.Add(" ");
            }

            if (dt.Rows.Count > 0)
            {

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (ArgTYPE == 1)
                    {
                        ArgCombobox.Items.Add(dt.Rows[i]["Code"].ToString().Trim() + "." + dt.Rows[i]["Name"].ToString().Trim());
                    }

                    else if (ArgTYPE == 2)
                    {
                        ArgCombobox.Items.Add(dt.Rows[i]["Code"].ToString().Trim());
                    }

                    else if (ArgTYPE == 3)
                    {
                        ArgCombobox.Items.Add(dt.Rows[i]["Name"].ToString().Trim());
                    }
                }
            }
            else
            {
                ComFunc.MsgBox("조회중 문제가 발생하였습니다.");
                return;
            }
            dt.Dispose();
            dt = null;


        }

        /// <summary>
        /// 2017-10-13 박병규, 콤보박스에 응급이송차량을 추가한다.
        /// </summary>
        /// <param name="ArgCombobox"></param>
        /// <param name="ArgClear"></param>
        /// <param name="ArgTYPE"></param>
        /// <param name="ArgNULL"></param>
        public void Combo_ErCar_Set(PsmhDb pDbCon, ComboBox ArgCombobox, bool ArgClear, int ArgTYPE, string ArgNULL = "")
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            if (ArgClear == true)
            {
                ArgCombobox.Items.Clear();
            }

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT Code,Name ";
            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_ER_CAR ";
            SQL = SQL + ComNum.VBLF + "  WHERE 1        = 1 ";
            SQL = SQL + ComNum.VBLF + "    AND (DelDate IS NULL OR DelDate > TRUNC(SYSDATE)) ";
            SQL = SQL + ComNum.VBLF + "ORDER BY Code ";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
            }

            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (ArgNULL != "N")
            {
                ArgCombobox.Items.Add(" ");
            }

            if (dt.Rows.Count > 0)
            {

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (ArgTYPE == 1)
                    {
                        ArgCombobox.Items.Add(dt.Rows[i]["Code"].ToString().Trim() + "." + dt.Rows[i]["Name"].ToString().Trim());
                    }

                    else if (ArgTYPE == 2)
                    {
                        ArgCombobox.Items.Add(dt.Rows[i]["Code"].ToString().Trim());
                    }

                    else if (ArgTYPE == 3)
                    {
                        ArgCombobox.Items.Add(dt.Rows[i]["Name"].ToString().Trim());
                    }
                }
            }
            else
            {
                ComFunc.MsgBox("조회중 문제가 발생하였습니다.");
                return;
            }
            dt.Dispose();
            dt = null;


        }


        /// <summary>
        /// Description : 콤보박스에 BCODE중 감액 관련 항목을 추가한다.
        /// Author : 박병규
        /// Create Date : 2017.08.17
        /// </summary>
        /// <param name="o"></param>
        /// <param name="ArgGubun"></param>
        /// <param name="ArgGubun2"></param>
        /// <param name="ArgClear"></param>
        /// <param name="ArgType"></param>
        /// <param name="ArgNull"></param>
        public void COMBO_GAMEK_BCODE_SET(PsmhDb pDbCon, ComboBox o, string ArgGubun, string ArgGubun2, bool ArgClear, int ArgType, string ArgNull = "")
        {
            DataTable DtFunc = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            if (ArgClear == true)
                o.Items.Clear();

            SQL = "";
            SQL += ComNum.VBLF + " SELECT * ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL += ComNum.VBLF + "  WHERE 1      = 1 ";
            SQL += ComNum.VBLF + "    AND GUBUN  = '" + ArgGubun + "' ";

            if (ArgGubun2.Trim() != "")
                SQL += ComNum.VBLF + "    AND GUBUN2 = '" + ArgGubun2 + "' ";

            SQL += ComNum.VBLF + "    AND CODE NOT IN ('53','54') "; //협진,PET-CT는 진료구분에서 함.
            SQL += ComNum.VBLF + "    AND (DELDATE IS NULL OR DELDATE > TRUNC(SYSDATE)) ";
            SQL += ComNum.VBLF + "  ORDER BY Sort, Code  ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                return;
            }

            if (DtFunc.Rows.Count == 0)
            {
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                DtFunc.Dispose();
                DtFunc = null;
                return;
            }

            if (ArgNull == "")
                o.Items.Add("");


            for (int i = 0; i < DtFunc.Rows.Count; i++)
            {
                if (ArgType == 1)
                    o.Items.Add(DtFunc.Rows[i]["Code"].ToString().Trim() + "." + DtFunc.Rows[i]["Name"].ToString().Trim());
                else if (ArgType == 2)
                    o.Items.Add(DtFunc.Rows[i]["Code"].ToString().Trim());
                else if (ArgType == 3)
                    o.Items.Add(DtFunc.Rows[i]["Name"].ToString().Trim());
            }

            DtFunc.Dispose();
            DtFunc = null;
        }

        /// <summary>
        /// Description : 콤보박스에 진찰료수납(접수구분) 관련 항목을 추가한다.
        /// Author : 박병규
        /// Create Date : 2017.08.17
        /// </summary>
        /// <param name="o"></param>
        /// <param name="ArgClear"></param>
        /// <param name="ArgType"></param>
        /// <param name="ArgNull"></param>
        public void COMBO_OPDJIN_SET(PsmhDb pDbCon, ComboBox o, bool ArgClear, int ArgType, string ArgNull = "")
        {
            DataTable DtFunc = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            if (ArgClear == true)
                o.Items.Clear();

            SQL = "";
            SQL += ComNum.VBLF + " SELECT * ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_OPDJIN ";
            SQL += ComNum.VBLF + "  WHERE 1      = 1 ";
            SQL += ComNum.VBLF + "    AND (DELDATE IS NULL OR DELDATE > TRUNC(SYSDATE)) ";
            SQL += ComNum.VBLF + "  ORDER BY CODE  ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                return;
            }

            if (DtFunc.Rows.Count == 0)
            {
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                DtFunc.Dispose();
                DtFunc = null;
                return;
            }

            if (ArgNull == "")
                o.Items.Add("");

            for (int i = 0; i < DtFunc.Rows.Count; i++)
            {
                if (ArgType == 1)
                    o.Items.Add(DtFunc.Rows[i]["Code"].ToString().Trim() + "." + DtFunc.Rows[i]["Name"].ToString().Trim());
                else if (ArgType == 2)
                    o.Items.Add(DtFunc.Rows[i]["Code"].ToString().Trim());
                else if (ArgType == 3)
                    o.Items.Add(DtFunc.Rows[i]["Name"].ToString().Trim());
            }

            DtFunc.Dispose();
            DtFunc = null;
        }

        /// <summary>
        /// Description : 콤보박스에 BAS_BUN 관련 항목을 추가한다.
        /// Author : 박병규
        /// Create Date : 2017.08.16
        /// </summary>
        /// <param name="o"></param>
        /// <param name="ArgJong"></param>
        /// <param name="ArgClear"></param>
        /// <param name="ArgType"></param>
        /// <param name="ArgNull"></param>
        public void COMBO_BASBUN_SET(PsmhDb pDbCon, ComboBox o, string ArgJong, bool ArgClear, int ArgType, string ArgNull = "")
        {
            DataTable DtFunc = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            if (ArgClear == true)
                o.Items.Clear();

            SQL = "";
            SQL += ComNum.VBLF + "SELECT * ";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_BUN ";
            SQL += ComNum.VBLF + " WHERE 1      = 1 ";
            SQL += ComNum.VBLF + "   AND JONG   = '" + ArgJong + "' ";
            SQL += ComNum.VBLF + "   AND (DELDATE IS NULL OR DELDATE > TRUNC(SYSDATE)) ";
            SQL += ComNum.VBLF + "ORDER BY CODE ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                return;
            }

            if (DtFunc.Rows.Count == 0)
            {
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                DtFunc.Dispose();
                DtFunc = null;
                return;
            }

            if (ArgNull == "")
                o.Items.Add("");


            for (int i = 0; i < DtFunc.Rows.Count; i++)
            {
                if (ArgType == 1)
                    o.Items.Add(DtFunc.Rows[i]["Code"].ToString().Trim() + "." + DtFunc.Rows[i]["Name"].ToString().Trim());
                else if (ArgType == 2)
                    o.Items.Add(DtFunc.Rows[i]["Code"].ToString().Trim());
                else if (ArgType == 3)
                    o.Items.Add(DtFunc.Rows[i]["Name"].ToString().Trim());
            }

            DtFunc.Dispose();
            DtFunc = null;
        }



        /// <summary>
        /// 2017-08-07 안정수, 콤보박스에 항목을 추가한다.
        /// </summary>
        /// <param name="cbo"></param>
        public void ComboGubun_ADDITEM(PsmhDb pDbCon, ComboBox cbo)
        {
            int i = 0;
            string strList = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT                                                           ";
            SQL = SQL + ComNum.VBLF + " Code,Name                                                       ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_CSINFO_CODE                       ";
            SQL = SQL + ComNum.VBLF + "WHERE 1=1                                                        ";
            SQL = SQL + ComNum.VBLF + "   AND Gubun='1'                                                 ";
            SQL = SQL + ComNum.VBLF + "ORDER BY SORT,Code                                               ";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strList = dt.Rows[i]["Code"].ToString().Trim() + ".";
                    strList += dt.Rows[i]["Name"].ToString().Trim();
                }

                cbo.Items.Add(strList);

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 2017-08-07 안정수, 직업이름을 읽어온다.
        /// </summary>
        /// <param name="ArgCode"></param>
        /// <returns></returns>
        public string READ_Jikup_Name(PsmhDb pDbCon, string ArgCode)
        {
            int i = 0;
            string rtnVal = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT                                                           ";
            SQL = SQL + ComNum.VBLF + " Name                                                            ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_CSINFO_CODE                       ";
            SQL = SQL + ComNum.VBLF + "WHERE 1=1                                                        ";
            SQL = SQL + ComNum.VBLF + "   AND Gubun='4'                                                 "; // 직업
            SQL = SQL + ComNum.VBLF + "   AND Code='" + ArgCode + "'                                     ";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["Name"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : SEQ 번호 가져오기
        /// Author : 박병규
        /// Create Date : 2017.07.04
        /// </summary>
        /// <seealso cref="Oumsad.bas : READ_NEXT_NHICNO"/> 
        public long GET_NEXT_NHICNO(PsmhDb pDbCon)
        {
            DataTable DtPf = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            long rtnVal = 0;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT KOSMOS_PMPA.SEQ_OPD_NHIC.NEXTVAL WRTNO ";
            SQL += ComNum.VBLF + "   FROM DUAL";
            SqlErr = clsDB.GetDataTableEx(ref DtPf, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                DtPf.Dispose();
                DtPf = null;
                return rtnVal;
            }

            rtnVal = long.Parse(DtPf.Rows[0]["WRTNO"].ToString().Trim());

            DtPf.Dispose();
            DtPf = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 의사 번호 가져오기
        /// Author : 안정수
        /// Create Date : 2017.08.11
        /// </summary>
        /// <param name="ArgDrCode"></param>
        /// <seealso cref="clsVbfunc.bas : READ_OCS_Doctor3_DrBunho"/>
        /// <returns></returns>
        public string READ_OCS_Doctor3_DrBunho(PsmhDb pDbCon, string ArgDrCode)
        {
            int i = 0;
            string rtnVal = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            if (ArgDrCode == "")
            {
                return rtnVal;
            }

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT                                                           ";
            SQL = SQL + ComNum.VBLF + " DrBunho                                                         ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR                             ";
            SQL = SQL + ComNum.VBLF + "WHERE 1=1                                                        ";
            SQL = SQL + ComNum.VBLF + "   AND DrCode='" + ArgDrCode + "'                                ";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["DrBunho"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 진료비영수증 관련 인쇄 허용 - 기본5년
        /// Author : 안정수
        /// Create Date : 2017.08.11
        /// </summary>
        /// <param name="ArgSabun"></param>
        /// <seealso cref="clsVbfunc.bas : JIN_AMT_PRINT_CHK"/>
        /// <returns></returns>
        public string JIN_AMT_PRINT_CHK(PsmhDb pDbCon, string ArgSabun)
        {
            int i = 0;
            string rtnVal = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT                                                           ";
            SQL = SQL + ComNum.VBLF + " Code                                                            ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE                             ";
            SQL = SQL + ComNum.VBLF + "WHERE 1=1                                                        ";
            SQL = SQL + ComNum.VBLF + "   AND GUBUN ='외부진료영수증인쇄제한'                           ";
            SQL = SQL + ComNum.VBLF + "   AND TRIM(CODE) ='" + ArgSabun + "'                            ";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = "OK";
                }
                else
                {
                    rtnVal = "";
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// Author : 안정수
        /// Create Date : 2017.08.11
        /// </summary>
        /// <param name="ArgSuNext"></param>
        /// <seealso cref="OVIEWA.bas : READ_BAS_Sun_S항"/>
        /// <returns></returns>
        public string READ_BAS_Sun_S(PsmhDb pDbCon, string ArgSuNext)
        {
            int i = 0;
            string rtnVal = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            if (ArgSuNext == "")
            {
                return rtnVal;
            }

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT                                                           ";
            SQL = SQL + ComNum.VBLF + " SugbS                                                           ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN                               ";
            SQL = SQL + ComNum.VBLF + "WHERE 1=1                                                        ";
            SQL = SQL + ComNum.VBLF + "   AND SuNext ='" + ArgSuNext + "'                               ";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["SugbS"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 해당 요일을 읽어온다.
        /// Author : 안정수
        /// Create Date : 2017.08.14
        /// </summary>
        /// <param name="ArDate"></param>
        /// <seealso cref="vbfunc.bas : READ_YOIL"/>
        /// <returns></returns>
        public string READ_YOIL(PsmhDb pDbCon, string ArDate)
        {
            int i = 0;
            string rtnVal = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT                                                               ";
            SQL = SQL + ComNum.VBLF + " TO_CHAR(TO_DATE('" + ArDate + "','YYYY-MM-DD'),'DY') Yoil FROM DUAL ";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                }

                if (dt.Rows.Count == 1)
                {
                    switch (dt.Rows[0]["Yoil"].ToString().Trim().ToUpper())
                    {
                        case "SUN":
                        case "일":
                            rtnVal = "일요일";
                            break;

                        case "MON":
                        case "월":
                            rtnVal = "월요일";
                            break;

                        case "TUE":
                        case "화":
                            rtnVal = "화요일";
                            break;

                        case "WED":
                        case "수":
                            rtnVal = "수요일";
                            break;

                        case "THU":
                        case "목":
                            rtnVal = "목요일";
                            break;

                        case "FRI":
                        case "금":
                            rtnVal = "금요일";
                            break;

                        case "SAT":
                        case "토":
                            rtnVal = "토요일";
                            break;

                        default:
                            rtnVal = dt.Rows[0]["Yoil"].ToString().Trim();
                            break;
                    }
                }
                else
                {
                    rtnVal = "";
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// /// Description : READ_ERCAR_NAME
        /// Author : 안정수
        /// Create Date : 2017.08.17
        /// </summary>
        /// <param name="ArgCode"></param>
        /// <returns></returns>
        /// <seealso cref="VbFunction.bas : READ_ERCAR_NAME"/>
        public string READ_ERCAR_NAME(PsmhDb pDbCon, string ArgCode)
        {
            int i = 0;
            string rtnVal = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT                                                           ";
            SQL = SQL + ComNum.VBLF + " Name                                                            ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_ER_CAR                            ";
            SQL = SQL + ComNum.VBLF + "WHERE 1=1                                                        ";
            SQL = SQL + ComNum.VBLF + "   AND ((CODE ='" + ArgCode + "' ) or  (NCODE ='" + ArgCode + "'))                                 ";
            SQL = SQL + ComNum.VBLF + "   order by   DELDATE desc                             ";
            //SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL                                           ";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["NAME"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 외래접수 진찰료 수납구분명
        /// Author : 박병규
        /// Create Date : 2017.08.22
        /// <param name="ArgCode"></param>
        /// </summary>
        /// <seealso cref="vbBasCode.bas:Read_Jin"/>
        public string READ_JIN(PsmhDb pDbCon, string ArgCode)
        {
            DataTable DtFunc = new DataTable();
            string SQL = String.Empty;
            string SqlErr = String.Empty;
            string strVal = String.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT NAME ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_OPDJIN ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "    AND CODE  = '" + ArgCode.Trim() + "' ";
                SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strVal;
            }
        }

        /// <summary>
        /// Description : 10:00식으로 => 분으로
        /// Author : 박병규
        /// Create Date : 2017.08.22
        /// <param name="ArgData"></param>
        /// </summary>
        public int TIME_MI(string ArgData)
        {
            int nHH = 0;
            int nMM = 0;
            int rtnVal = 0;

            if (ArgData == "") return rtnVal;

            nHH = Convert.ToInt32(VB.Left(ArgData, 2));
            nMM = Convert.ToInt32(VB.Right(ArgData, 2));
            rtnVal = (nHH * 60) + nMM;

            return rtnVal;
        }

        /// <summary>
        /// Description : 분을 시간으로 10:30
        /// Author : 박병규
        /// Create Date : 2017.08.22
        /// <param name="ArgData"></param>
        /// </summary>
        public string TIME_MI_TIME(int ArgData)
        {
            int nHH = 0;
            int nMM = 0;
            string rtnVal = "";

            nHH = VB.Int(ArgData / 60);
            nMM = ArgData % 60;
            rtnVal = string.Format("{0:00}", nHH) + ":" + string.Format("{0:00}", nMM);

            return rtnVal;
        }


        /// <summary>
        /// Description : 해당 날짜가 휴일인지 Check 함(True: 휴일  False: 휴일이 아님)
        /// Author : 박병규
        /// Create Date : 2017.08.22
        /// <param name="ArgDate"></param>
        /// </summary>
        /// <seealso cref="vbfunc.bas:DATE_HUIL_Check"/>
        public bool DATE_HUIL_CHECK(PsmhDb pDbCon, string ArgDate)
        {
            DataTable DtFunc = new DataTable();
            string SQL = String.Empty;
            string SqlErr = String.Empty;
            bool rtnVal = false;

            string strDate = string.Empty;

            if (ArgDate.Trim() == "")
            {
                return false;
            }
            else
            {
                strDate = VB.Left(ArgDate, 10);
            }

            clsPublic.GstrHoliday = "";
            clsPublic.GstrTempHoliday = "";

            if (ArgDate == "")
            {
                return rtnVal;
            }

            try
            {

                SQL = "";
                SQL += ComNum.VBLF + " SELECT HOLYDAY, TEMPHOLYDAY ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_JOB ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "    AND JOBDATE  = TO_DATE('" + strDate + "', 'YYYY-MM-DD') ";
                SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

                if (DtFunc.Rows.Count == 0)
                {
                    DtFunc.Dispose();
                    DtFunc = null;
                    return rtnVal;
                }

                if (DtFunc.Rows.Count > 0)
                {
                    if (DtFunc.Rows[0]["HOLYDAY"].ToString().Trim() == "*")
                    {
                        clsPublic.GstrHoliday = "*";
                        rtnVal = true;
                    }

                    if (DtFunc.Rows[0]["TEMPHOLYDAY"].ToString().Trim() == "*")
                    {
                        clsPublic.GstrTempHoliday = "*";
                        rtnVal = true;
                    }
                }

                DtFunc.Dispose();
                DtFunc = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                rtnVal = false;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// Description : 전산정보팀에서 DB점검시 일시적으로 OCS, 원무행정 업무 중지시 사용
        /// Author : 박병규
        /// Create Date : 2017.08.30
        /// </summary>
        /// <seealso cref="vbfunc.bas:READ_JOBSTOP_TIME"/>
        public string READ_JOBSTOP_TIME(PsmhDb pDbCon)
        {
            DataTable DtFunc = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT Remark ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_JOBSTOP_TIME ";
            SQL += ComNum.VBLF + "  WHERE JobStop_FROM  <= SYSDATE ";
            SQL += ComNum.VBLF + "    AND JobStop_To    >= SYSDATE ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (DtFunc.Rows.Count == 0)
            {
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = DtFunc.Rows[0]["Remark"].ToString().Trim();

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : DateTimePicker 값 Clear
        /// Author : 김민철
        /// Create Date : 2017.08.30
        /// </summary>
        /// <param name="dtp"></param>
        public void dtpClear(DateTimePicker dtp)
        {
            DateTime date = new DateTime(1900, 1, 1, 00, 00, 00);

            dtp.Format = DateTimePickerFormat.Custom;
            dtp.Value = date;
            dtp.CustomFormat = " ";

        }

        /// <summary>
        /// Description : DateTimePicker 값 Setting
        /// 사용예 : 이벤트 헨들러 사용 폼 코딩시 =>  this.dtpBirth.ValueChanged += new EventHandler(ComFunc.eDtpFormatSet);
        /// Author : 김민철
        /// Create Date : 2017.08.30
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void eDtpFormatSet(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            DateTime date = new DateTime(1900, 1, 1, 00, 00, 00);

            if (dtp.Value == date)
            {
                return;
            }

            dtp.Format = DateTimePickerFormat.Short;

        }

        /// <summary>
        /// Delay 함수
        /// </summary>
        /// <param name="intMicroSecond"></param>
        /// <returns></returns>
        public static DateTime Delay(int intMicroSecond)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, intMicroSecond);
            DateTime AfterWards = ThisMoment.Add(duration);

            while (AfterWards >= ThisMoment)
            {
                Application.DoEvents();
                ThisMoment = DateTime.Now;
            }

            return DateTime.Now;
        }

        /// <summary>
        /// INI file에서 필요한 값을 불러옴
        /// </summary>
        /// <param name="strSettingIniFile"></param>
        /// <param name="strSectionName"></param>
        /// <param name="strSetting"></param>
        /// <returns></returns>
        public static string ReadINI(string strPath, string strSectionName, string strSetting)
        {
            string rtnVal = "";

            FileInfo patfile = new FileInfo(strPath);
            if (patfile.Exists == false)
            {
                rtnVal = "";
                return rtnVal;
            }
            // INI 파일을 읽어서 서버 정보와 비교를 한다
            clsIniFile myIniFile = new clsIniFile(strPath);
            rtnVal = myIniFile.ReadValue(strSectionName, strSetting, "");

            return rtnVal;
        }

        /// <summary>
        /// INI file에서 값을 저장함
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="strSectionName"></param>
        /// <param name="strSetting"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool WriteINI(string strPath, string strSectionName, string strSetting, string strValue)
        {
            bool rtnVal = false;
            // INI 파일을 읽어서 서버 정보와 비교를 한다
            clsIniFile myIniFile = new clsIniFile(strPath);
            rtnVal = DirectIniFile.WriteProfileValue(strPath, strSectionName, strSetting, strValue);
            return rtnVal;
        }

        /// <summary>
        /// Description : 성별 다시 확인
        /// Author : 안정수
        /// Create Date : 2017.10.10
        /// </summary>
        /// <param name="ArgJumin2"></param>
        /// <returns></returns>
        public string SEX_SEARCH(string ArgJumin2)
        {
            string rtnVal = "";

            //성별구분으로 세기를 판단함

            switch (VB.Left(ArgJumin2, 1))
            {
                case "1":
                case "3":
                case "5":
                case "7":
                case "9":
                    rtnVal = "M";
                    break;
                default:
                    rtnVal = "F";
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// VB의 FIX문의 오류로 소수점이하를 절사하는 함수
        /// author : 안정수
        /// Create Date : 2017-10-12
        /// <seealso cref="vbfunc.bas : FIX_N"/>
        /// </summary>
        /// <param name="ArgNumber"></param>
        /// <returns></returns>
        public long FIX_N(double ArgNumber)
        {
            string ArgStrNum = "";
            long rtnVal = 0;

            ArgStrNum = String.Format("{0:##############0.00}", ArgNumber);
            rtnVal = Convert.ToInt64(VB.Val(VB.Left(ArgStrNum, VB.Len(ArgStrNum) - 3)));
            return rtnVal;
        }

        /// <summary>
        /// Quotation 문자를 "`"로 변경
        /// author : 안정수
        /// Create Date : 2017-11-27
        /// <seealso cref="vbfunc.bas : Quotation_Change"/>
        /// </summary>
        /// <param name="ArgString"></param>
        /// <returns></returns>
        public string Quotation_Change(string ArgString)
        {
            string rtnVal = "";

            int i = 0;
            int nLen = 0;

            string ArgReturn = "";
            string ArgChar = "";

            nLen = ArgString.Length;

            if (nLen == 0)
            {
                rtnVal = "";
                return rtnVal;
            }

            for (i = 1; i <= nLen; i++)
            {
                ArgChar = VB.Mid(ArgString, i, 1);
                if (ArgChar == "'")
                {
                    ArgReturn += "`";
                }

                else if (ArgChar == "·")
                {
                    ArgReturn += ".";
                }

                else
                {
                    ArgReturn += ArgChar;
                }
            }

            rtnVal = ArgReturn;

            return rtnVal;
        }

        /// <summary>
        /// 이미지를 Byte로 변환
        /// author : 박웅규
        /// Create Date : 2017-12-26
        /// </summary>
        /// <param name="imageIn"></param>
        /// <returns></returns>
        public static byte[] imageToByteArray(Image imageIn)
        {
            // 이미지 포멧을 가져옵니다.

            ImageFormat imageFormat = imageIn.RawFormat;

            using (MemoryStream memoryStream = new MemoryStream())

            {
                using(Image tmpImg = new Bitmap(imageIn))
                {
                    tmpImg.Save(memoryStream, ImageFormat.Jpeg);

                }

               

                // 배열을 선언합니다.
                byte[] buffer = new byte[memoryStream.Length];

                // 배열을 체웁니다.
                memoryStream.Seek(0, SeekOrigin.Begin);
                memoryStream.Read(buffer, 0, buffer.Length);


                return buffer;
            }
        }

        /// <summary>
        /// 이미지를 문자로 변환
        /// author : 박웅규
        /// Create Date : 2017-12-26
        /// </summary>
        /// <param name="imageIn"></param>
        /// <returns></returns>
        public static string ConvertImageToString(Image pImage)
        {
            byte[] imageArray = imageToByteArray(pImage);
            string strImage = Convert.ToBase64String(imageArray);
            return strImage;
        }

        /// <summary>
        /// Byte를 이미지로 변환
        /// author : 박웅규
        /// Create Date : 2017-12-26
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Image ByteArrayToImage(byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes);
            Image recImg = Image.FromStream(ms);
            return recImg;
        }

        /// <summary>
        /// String를 이미지로 변환
        /// author : 박웅규
        /// Create Date : 2017-12-26
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Image StringToImage(string strImage)
        {
            if (strImage == "")
                return null;

            byte[] b = Convert.FromBase64String(strImage);
            using (MemoryStream stream = new MemoryStream(b))
            {
                return Image.FromStream(stream);
            }
        }

        /// <summary>
        /// 일자을 가지고 개월 단위 계산
        /// author : 박병규
        /// Create Date : 2018-01-02
        /// </summary>
        /// <param name="ArgDate"></param>
        /// <param name="ArgMonth"></param>
        /// <returns></returns>
        public string Month_Add(PsmhDb pDbCon, string ArgDate, int ArgMonth)
        {
            DataTable DtFunc = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(ADD_MONTHS(TO_DATE('" + ArgDate + "','YYYY-MM-DD'), ";

            if (ArgMonth < 0)
                SQL += ComNum.VBLF + ArgMonth;
            else
                SQL += ComNum.VBLF + "+" + ArgMonth;

            SQL += ComNum.VBLF + "  ),'YYYY-MM-DD') AddMONTH FROM DUAL ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (DtFunc.Rows.Count == 0)
            {
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = DtFunc.Rows[0]["AddMONTH"].ToString().Trim();
            else
                rtnVal = "";

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 내과 인증전문의 체크
        /// Author : 박창욱
        /// Create Date : 2018-01-03
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strDrCode"></param>
        /// <param name="strDate"></param>
        /// <seealso cref="oumsad_chk.bas:CHK_내과분과인증전문의"/>
        /// <returns></returns>
        public bool Check_Dept_Certified(PsmhDb pDbCon, string strDrCode, string strDate)
        {
            //내과세부분과 전문의 인증여부 확인 (진찰료 산정관련)
            bool rtnVar = false;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT c.DRCODE ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "INSA_MST a, " + ComNum.DB_ERP + "INSA_MSTL b,";
                SQL = SQL + ComNum.VBLF + "       " + ComNum.DB_MED + "OCS_DOCTOR c ";
                SQL = SQL + ComNum.VBLF + " WHERE a.TOIDAY IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND a.SABUN =b.SABUN(+) ";
                SQL = SQL + ComNum.VBLF + "   AND a.SABUN =c.SABUN(+) ";
                SQL = SQL + ComNum.VBLF + "   AND a.BUSE in  ( ";
                SQL = SQL + ComNum.VBLF + "       SELECT BUCODE FROM " + ComNum.DB_PMPA + "BAS_BUSE WHERE NAME LIKE '%내과%' ) ";
                SQL = SQL + ComNum.VBLF + "   AND (b.NAME like '%내과분과%' OR b.bun ='1' ) ";
                SQL = SQL + ComNum.VBLF + "   AND b.FDATE <= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND b.TDATE >= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND c.DRCODE = '" + strDrCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = true;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                return rtnVar;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVar;
            }
        }
        public bool Check_Jinchal_Duplicate_Sunap(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgDate)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = true;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(SUM(QTY*NAL), 0) CNT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND SUNEXT IN (SELECT SUNEXT FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT_CONV ";
            SQL += ComNum.VBLF + "                    WHERE GUBUN   = '1' ";
            SQL += ComNum.VBLF + "                      AND SDATE   <= TO_DATE('" + ArgDate + "','YYYY-MM-DD')) ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("의료급여_진찰료중복체크 조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
            {
                if (Convert.ToInt32(DtQ.Rows[0]["CNT"].ToString()) > 0)
                    rtnVal = false;
            }

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 내과세부분과 비인증 전문의 진료비가 있는지 체크함
        /// Author : 박창욱
        /// Create Date : 2018-01-04
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPano"></param>
        /// <param name="strDate"></param>
        /// <param name="strGb"></param>
        /// <param name="strDept"></param>
        /// <param name="strDrcode"></param>
        /// <param name="dblJinAmt"></param>
        /// <seealso cref="oumsad_chk.bas:RTN_내과접수내역"/>
        /// <returns></returns>
        /// 
        public string Check_Dept_ReceiveHis(PsmhDb pDbCon, string strPano, string strDate, string strGb, string strDept, string argDrCode, double dblJinAmt)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            double nAmt = 0;
            string strDrCode = "";
            string strMsg = "";

            string rtnVar = "";
            ComFunc CF = new ComBase.ComFunc();


            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT DEPTCODE,DRCODE, AMT1";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE=TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND SUBSTR(DEPTCODE,1,1) = 'M' ";          //내과만
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE  <> '" + strDept + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DRCODE  <> '" + argDrCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nAmt = VB.Val(dt.Rows[i]["AMT1"].ToString().Trim());
                        strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                        strDrCode = dt.Rows[i]["DrCode"].ToString().Trim();

                        if (strGb == "인증")
                        {

                            if ((CF.Check_Dept_Certified(clsDB.DbCon, strDrCode, strDate) == true && nAmt > 0) || (CF.Check_Dept_Certified(clsDB.DbCon, strDrCode, strDate) == true && Check_Jinchal_Duplicate_Sunap(pDbCon, strPano, strDept, strDate) == false))
                            {
                                //인증전문의
                                strMsg = strDept + "^^" + clsVbfunc.GetBASDoctorName(clsDB.DbCon, strDrCode);
                                break;
                            }
                        }
                        else
                        {
                            if ((CF.Check_Dept_Certified(clsDB.DbCon, strDrCode, strDate) == false && nAmt > 0) || (CF.Check_Dept_Certified(clsDB.DbCon, strDrCode, strDate) == false && Check_Jinchal_Duplicate_Sunap(pDbCon, strPano, strDept, strDate) == false))
                            {
                                //비인증전문의
                                strMsg = strDept + "^^" + clsVbfunc.GetBASDoctorName(clsDB.DbCon, strDrCode);
                                break;
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                if (strMsg != "")
                {
                    if (strGb == "인증")
                    {
                        if ((nAmt > 0 && dblJinAmt > 0) || (Check_Jinchal_Duplicate_Sunap(pDbCon, strPano, strDept, strDate) == false && dblJinAmt > 0))
                        {
                            if (clsType.User.Sabun != "5001" && clsType.User.Sabun != "5002" && clsType.User.Sabun != "5003" && clsType.User.Sabun != "5004" && clsType.User.Sabun != "5005" && clsType.User.Sabun != "5006" && clsType.User.Sabun != "5050")
                            {
                                clsPublic.GstrMsgTitle = "접수비 1회만 산정";
                                clsPublic.GstrMsgList = "비인증 전문의입니다." + ComNum.VBLF + ComNum.VBLF;
                                clsPublic.GstrMsgList += "이미 당일 세부내과 인증전문의 진료(진료비)내역이 있습니다." + ComNum.VBLF;
                                clsPublic.GstrMsgList += "진료과: " + VB.Pstr(strMsg, "^^", 1) + " / " + VB.Pstr(strMsg, "^^", 2) + ComNum.VBLF;
                                clsPublic.GstrMsgList += "현재과를 접수2(접수비 발생안됨)로 접수수정하여 주시길 바랍니다.";
                                clsPublic.GstrMsgList += "그래도 수납하시겠습니까?";
                            }
                        }
                    }
                    else
                    {
                        if ((nAmt > 0 && dblJinAmt > 0) || (Check_Jinchal_Duplicate_Sunap(pDbCon, strPano, strDept, strDate) == false && dblJinAmt > 0))
                        {
                            if (clsType.User.Sabun != "5001" && clsType.User.Sabun != "5002" && clsType.User.Sabun != "5003" && clsType.User.Sabun != "5004" && clsType.User.Sabun != "5005" && clsType.User.Sabun != "5006" && clsType.User.Sabun != "5050")
                            {
                                clsPublic.GstrMsgTitle = "접수비 1회만 산정";
                                clsPublic.GstrMsgList = "비인증 전문의입니다." + ComNum.VBLF + ComNum.VBLF;
                                clsPublic.GstrMsgList += "이미 당일 세부내과 진료(진료비)내역이 있습니다." + ComNum.VBLF;
                                clsPublic.GstrMsgList += "진료과: " + VB.Pstr(strMsg, "^^", 1) + " / " + VB.Pstr(strMsg, "^^", 2) + ComNum.VBLF;
                                clsPublic.GstrMsgList += "접수2(접수비 발생안됨)로 접수하여 주시길 바랍니다.";
                                clsPublic.GstrMsgList += "그래도 수납하시겠습니까?";
                            }
                        }

                    }
                }

                rtnVar = strMsg;
                return rtnVar;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVar;
            }
        }

        /// <summary>
        /// VB - CHK_B항_수가항목
        /// 2018-01-04 박창욱
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argTable"></param>
        /// <param name="argPano"></param>
        /// <param name="argDept"></param>
        /// <returns></returns>
        public string Check_SugbB(PsmhDb pDbCon, string argTable, string argPano, string argDept)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strSysDate = "";
            string rtnVar = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Pano,SuCode";
                if (argTable == "ETC_B_SUCHK")
                {
                    SQL = SQL + ComNum.VBLF + ",MESSAGE,GUBUN ";
                }
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + argTable + "";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + argPano + "'";
                SQL = SQL + ComNum.VBLF + "   AND DeptCode ='" + argDept + "'";
                SQL = SQL + ComNum.VBLF + "   AND BDATE =TO_DATE('" + strSysDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND ( CHK IS NULL OR CHK <>'Y')";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = dt.Rows[0]["SuCode"].ToString().Trim();
                    if (argTable == "ETC_B_SUCHK")
                    {
                        rtnVar = rtnVar + "^^" + dt.Rows[0]["GUBUN"].ToString().Trim() + "^^" + dt.Rows[0]["MESSAGE"].ToString().Trim();
                    }
                    else
                    {
                        rtnVar = rtnVar + "^^" + dt.Rows[0]["SuCode"].ToString().Trim() + "^^";
                    }
                }

                dt.Dispose();
                dt = null;

                return rtnVar;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVar;
            }
        }

        /// <summary>
        /// VB - CHK_진료의사본과접수
        /// 2018-01-04 박창욱
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argPano"></param>
        /// <param name="argDrCode"></param>
        /// <returns></returns>
        public static bool CHK_Practitioner_RegularDeptReceive(PsmhDb pDbCon, string argPano, string argDrCode)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            bool rtnVar = false;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT c.Pano ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_DOCTOR a, " + ComNum.DB_MED + "OCS_DOCTOR b,";
                SQL = SQL + ComNum.VBLF + "       " + ComNum.DB_ERP + "INSA_MST c ";
                SQL = SQL + ComNum.VBLF + " Where a.DrCode ='" + argDrCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.DrCode = b.DrCode(+) ";
                SQL = SQL + ComNum.VBLF + "   AND b.Sabun = c.Sabun ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Pano"].ToString().Trim() == argPano)
                    {
                        rtnVar = true;
                    }
                }

                dt.Dispose();
                dt = null;

                return rtnVar;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVar;
            }
        }

        /// <summary>
        /// VB - CHK_만성질환상병진료대상자
        /// 재진진찰료 산정 만성질환 상병인지 체크
        /// 2018-01-05 박창욱
        /// </summary>
        /// <param name="pDbcon"></param>
        /// <param name="argPano"></param>
        /// <param name="argDate"></param>
        /// <param name="argDept"></param>
        /// <param name="GnDrugNal"></param>
        /// <returns></returns>
        public string CHK_ChronicIll(PsmhDb pDbcon, string argPano, string argDate, string argDept, ref int GnDrugNal)
        {
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVar = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ILLCODE";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OILLS ";
                SQL = SQL + ComNum.VBLF + " WHERE Ptno='" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDate=TO_DATE('" + argDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DeptCode= '" + argDept + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY SEQNO ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }

                if (dt.Rows.Count > 0)
                {
                    if (READ_ChronicIllCode(VB.Left(dt.Rows[0]["ILLCODE"].ToString().Trim(), 3)) != "")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT ILLCODE,TO_CHAR(BDate,'YYYY-MM-DD') BDate ";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OILLS ";
                        SQL = SQL + ComNum.VBLF + " WHERE Ptno='" + argPano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND BDate<TO_DATE('" + argDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND DeptCode= '" + argDept + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND SUBSTR(ILLCODE,1,3) = '" + VB.Left(dt.Rows[0]["ILLCODE"].ToString().Trim(), 3) + "' ";
                        SQL = SQL + ComNum.VBLF + " ORDER By BDate DESC ";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return rtnVar;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            GnDrugNal = 0;

                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "SELECT SUCODE,SUM(NAL) SNAL";
                            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER ";
                            SQL = SQL + ComNum.VBLF + " WHERE Ptno='" + argPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND BDate=TO_DATE('" + dt1.Rows[0]["BDate"].ToString().Trim() + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND DeptCode= '" + argDept + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND BUN IN ('11','12') ";
                            SQL = SQL + ComNum.VBLF + " GROUP BY SUCODE,Nal ";
                            SQL = SQL + ComNum.VBLF + " ORDER By Nal DESC ";

                            SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return rtnVar;
                            }

                            if (dt2.Rows.Count > 0)
                            {
                                GnDrugNal = (int)VB.Val(dt2.Rows[0]["SNAL"].ToString().Trim());
                                rtnVar = dt1.Rows[0]["BDate"].ToString().Trim() + "@@" + dt.Rows[0]["ILLCODE"].ToString().Trim();
                            }

                            dt2.Dispose();
                            dt2 = null;
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }
                }

                dt.Dispose();
                dt = null;

                return rtnVar;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVar;
            }
        }

        /// <summary>
        /// VB - READ_만성질환상병코드
        /// 2018-01-05 박창욱
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public static string READ_ChronicIllCode(string argCode)
        {
            string rtnVar = "";

            if (argCode == "")
            {
                return rtnVar;
            }

            if (string.Compare(argCode, "I10") >= 0 && string.Compare(argCode, "I12") <= 0)
            {
                rtnVar = "고혈압";
            }
            if (argCode == "I15")
            {
                rtnVar = "고혈압";
            }
            if (string.Compare(argCode, "E10") >= 0 && string.Compare(argCode, "E14") <= 0)
            {
                rtnVar = "당뇨병";
            }
            if (string.Compare(argCode, "F00") >= 0 && string.Compare(argCode, "F99") <= 0)
            {
                rtnVar = "정신 및 행동장애";
            }
            if (string.Compare(argCode, "G40") >= 0 && string.Compare(argCode, "G41") <= 0)
            {
                rtnVar = "정신 및 행동장애";
            }
            if (string.Compare(argCode, "A15") >= 0 && string.Compare(argCode, "A16") <= 0)
            {
                rtnVar = "호흡기결핵";
            }
            if (argCode == "A19")
            {
                rtnVar = "호흡기결핵";
            }
            if (string.Compare(argCode, "I05") >= 0 && string.Compare(argCode, "I09") <= 0)
            {
                rtnVar = "심장질환";
            }
            if (string.Compare(argCode, "I20") >= 0 && string.Compare(argCode, "I27") <= 0)
            {
                rtnVar = "심장질환";
            }
            if (string.Compare(argCode, "I30") >= 0 && string.Compare(argCode, "I52") <= 0)
            {
                rtnVar = "심장질환";
            }
            if (string.Compare(argCode, "I60") >= 0 && string.Compare(argCode, "I69") <= 0)
            {
                rtnVar = "대뇌혈괄질환";
            }
            if (string.Compare(argCode, "G00") >= 0 && string.Compare(argCode, "G37") <= 0)
            {
                rtnVar = "신경계질환";
            }
            if (string.Compare(argCode, "G43") >= 0 && string.Compare(argCode, "G83") <= 0)
            {
                rtnVar = "신경계질환";
            }
            if (string.Compare(argCode, "C00") >= 0 && string.Compare(argCode, "C97") <= 0)
            {
                rtnVar = "악성신생물";
            }
            if (string.Compare(argCode, "D00") >= 0 && string.Compare(argCode, "D09") <= 0)
            {
                rtnVar = "악성신생물";
            }
            if (string.Compare(argCode, "E00") >= 0 && string.Compare(argCode, "E07") <= 0)
            {
                rtnVar = "갑상선의장애";
            }
            if (argCode == "B18")
            {
                rtnVar = "간의질환";
            }
            if (argCode == "B19")
            {
                rtnVar = "간의질환";
            }
            if (string.Compare(argCode, "K70") >= 0 && string.Compare(argCode, "K77") < 0)
            {
                rtnVar = "간의질환";
            }
            if (argCode == "N18")
            {
                rtnVar = "만성신부전증";
            }

            return rtnVar;
        }

        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2017.07.27
        /// <param name="ArgCode"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgAll"></param>
        /// <param name="ArgType"></param>
        /// </summary>
        /// <seealso cref="vbbascode.bas : Read_약전송구분읽기"/> 
        public void Read_DrugSend_Set(PsmhDb pDbCon, ComboBox ArgCombo)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT CODE, NAME ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCode ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND GUBUN     = '원무약수정' ";
            SQL += ComNum.VBLF + "    AND (DELDATE IS NULL OR DELDATE = '') ";
            SQL += ComNum.VBLF + "  ORDER BY CODE ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                DtFunc.Dispose();
                DtFunc = null;
                return;
            }

            if (DtFunc.Rows.Count > 0)
            {
                ArgCombo.Items.Clear();

                for (int i = 0; i < DtFunc.Rows.Count; i++)
                    ArgCombo.Items.Add(DtFunc.Rows[i]["CODE"].ToString().Trim() + "." + DtFunc.Rows[i]["NAME"].ToString().Trim());
            }

            ArgCombo.SelectedIndex = 0;

            DtFunc.Dispose();
            DtFunc = null;
        }

        /// <summary>
        /// 하부 컨트롤의 IME MODE를 변경한다
        /// 박웅규 : 2018-05-25
        /// </summary>
        /// <param name="objParent">최상위 컨트롤(폼,패널만 가능)</param>
        /// <param name="pOption">K:한글, E:영어</param>
        public static void SetIMEMODE(Control objParent, string pOption)
        {
            Control[] controls = GetAllControls(objParent);

            foreach (Control ctl in controls)
            {
                if (ctl is TextBox)
                {
                    if (pOption == "K")
                    {
                        ((TextBox)ctl).ImeMode = ImeMode.Hangul;
                    }
                    if (pOption == "E")
                    {
                        ((TextBox)ctl).ImeMode = ImeMode.Alpha;
                    }
                }
            }
        }

        /// <summary>
        /// EMR Viewer 가 실행중인지 확인한다
        /// </summary>
        /// <returns></returns>
        public static bool CheckExecEmrViewOld()
        {
            bool ActiveProc = false;
            System.Diagnostics.Process[] ProcessEx = System.Diagnostics.Process.GetProcessesByName("mhemrviewer");
            if (ProcessEx.Length > 0)
            {
                System.Diagnostics.Process[] Pro1 = System.Diagnostics.Process.GetProcessesByName("mhemrviewer");
                System.Diagnostics.Process CurPro = System.Diagnostics.Process.GetCurrentProcess();
                foreach (System.Diagnostics.Process Proc in Pro1)
                {
                    if (Proc.Id != CurPro.Id)
                    {
                        ActiveProc = true;
                    }
                }
            }
            return ActiveProc;
        }

        /// <summary>
        /// 2019.09.24 (vbfunc.bas TextBox_2_MultiLine)
        /// </summary>
        /// <param name="argString"></param>
        /// <param name="nLen"></param>
        /// <returns></returns>
        public string TextBox_2_MultiLine(string argString, int nLen)
        {
            string rtnVal = "";

            string strString = "";
            string ArgReturn = "";
            string ArgChar = "";
            string ArgREC = "";

            //마지막 공란 및 CrLf를 제거
            strString = VB.RTrim(argString);
            if (VB.Right(strString, 2) == Environment.NewLine)
            {
                strString = VB.Left(strString, strString.Length - 2);
            }

            for (int i = 1; i <= VB.Len(strString); i++)
            {
                ArgChar = VB.Mid(strString, i, 1);
                if (VB.Mid(strString, i, 2) == "\r\n")
                {
                    ArgReturn += ArgREC + "{{@}}";
                    ArgREC = "";
                    i += 1;
                }
                else if (VB.Mid(strString, i, 1) == "\n")
                {
                    ArgReturn += ArgREC + "{{@}}";
                    ArgREC = "";
                }
                //else if ((ArgREC + ArgChar).Length > nLen)
                else if (LenH(ArgREC + ArgChar) > nLen)
                {
                    ArgReturn += ArgREC + "{{@}}";
                    ArgREC = ArgChar;
                }
                else
                {
                    ArgREC += ArgChar;
                }
            }

            //마지막 {{#}}를 제거함
            ArgReturn += VB.RTrim(ArgREC);
            if (VB.Right(ArgReturn, 5) == "{{@}}")
            {
                ArgReturn = VB.RTrim(VB.Left(ArgReturn, ArgReturn.Length - 5).Trim());
                if (VB.Right(ArgReturn, 5) == "{{@}}")
                {
                    ArgReturn = VB.RTrim(VB.Left(ArgReturn, ArgReturn.Length - 5).Trim());
                    if (VB.Right(ArgReturn, 5) == "{{@}}")
                    {
                        ArgReturn = VB.RTrim(VB.Left(ArgReturn, ArgReturn.Length - 5).Trim());
                        if (VB.Right(ArgReturn, 5) == "{{@}}")
                        {
                            ArgReturn = VB.RTrim(VB.Left(ArgReturn, ArgReturn.Length - 5).Trim());
                            if (VB.Right(ArgReturn, 5) == "{{@}}")
                            {
                                ArgReturn = VB.RTrim(VB.Left(ArgReturn, ArgReturn.Length - 5).Trim());
                            }
                        }
                    }
                }
            }

            rtnVal = ArgReturn;
            return rtnVal;
        }

        /// <summary>
        /// BSA 계산로직. 간호 ComNurLibB -> ComLibB로 이관
        /// 2020-12-24 KMC
        /// </summary>
        /// <param name="argHeight"></param>
        /// <param name="argWeight"></param>
        /// <returns></returns>
        public static string GetBSA(string argHeight, string argWeight)
        {
            double nHe = 0;
            double nWt = 0;

            double nBSA0 = 0;
            double nBSA1 = 0;
            double nBSA2 = 0;

            try
            {
                nHe = Convert.ToDouble(argHeight.Replace("cm", ""));
                nWt = Convert.ToDouble(argWeight.Replace("kg", ""));

                nBSA0 = (nHe * nWt) / 3600;
                nBSA1 = Math.Pow(nBSA0, 0.5);
                nBSA2 = Fixed(nBSA1);

                return nBSA2.ToString();
            }
            catch
            {
                return "Error";
            }
        }

        /// <summary>
        /// 챠트에서 최근 3일이내 최신 키(height)값 가져오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argPTNO"></param>
        /// <param name="argINDATE"></param>
        /// <seealso cref="READ_IPD_HEIGHT"/>
        /// <returns> 키(height) </returns>
        public static string READ_IPD_HEIGHT(PsmhDb pDbCon, string argPTNO, string argINDATE)
        {
            string RtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT TRANSLATE (ITEMVALUE, ";
                SQL += ComNum.VBLF + "                  '0' || TRANSLATE (ITEMVALUE, 'x0123456789.', 'x'), ";
                SQL += ComNum.VBLF + "                  '0') ";
                SQL += ComNum.VBLF + "          HEIGHT ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRCHARTROW ";
                SQL += ComNum.VBLF + " WHERE EMRNO = ";
                SQL += ComNum.VBLF + "          (SELECT MAX (EMRNO) EMRNO ";
                SQL += ComNum.VBLF + "             FROM KOSMOS_EMR.AEMRCHARTMST S1 ";
                SQL += ComNum.VBLF + "            WHERE PTNO = '" + argPTNO + "' ";
                SQL += ComNum.VBLF + "                  AND (CHARTDATE = '" + argINDATE.Replace("-", "") + "' ";
                SQL += ComNum.VBLF + "                       OR (CHARTDATE >= TO_CHAR(TRUNC(SYSDATE - 3),'YYYYMMDD') AND CHARTDATE <= TO_CHAR(TRUNC(SYSDATE),'YYYYMMDD'))) ";
                SQL += ComNum.VBLF + "                  AND EXISTS ";
                SQL += ComNum.VBLF + "                         (SELECT * ";
                SQL += ComNum.VBLF + "                            FROM KOSMOS_EMR.AEMRCHARTROW S2 ";
                SQL += ComNum.VBLF + "                           WHERE S1.EMRNO = S2.EMRNO ";
                SQL += ComNum.VBLF + "                                 AND S2.ITEMCD IN ('I0000000562', 'I0000000002') ";
                SQL += ComNum.VBLF + "                                 AND ITEMVALUE IS NOT NULL)) ";
                SQL += ComNum.VBLF + "       AND ITEMCD IN ('I0000000562', 'I0000000002') ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = dt.Rows[0]["HEIGHT"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
            return RtnVal;
        }

        /// <summary>
        /// 챠트에서 최근 3일이내 최신 몸무게(weight)값 가져오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argPTNO"></param>
        /// <seealso cref="READ_IPD_WEIGHT"/>
        /// <returns> 몸무게 값 </returns>
        public static string READ_IPD_WEIGHT(PsmhDb pDbCon, string argPTNO, string argINDATE)
        {
            string RtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT TRANSLATE (ITEMVALUE, ";
                SQL += ComNum.VBLF + "                  '0' || TRANSLATE (ITEMVALUE, 'x0123456789.', 'x'), ";
                SQL += ComNum.VBLF + "                  '0') ";
                SQL += ComNum.VBLF + "          WEIGHT ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRCHARTROW ";
                SQL += ComNum.VBLF + " WHERE EMRNO = ";
                SQL += ComNum.VBLF + "          (SELECT MAX (EMRNO) EMRNO ";
                SQL += ComNum.VBLF + "             FROM KOSMOS_EMR.AEMRCHARTMST S1 ";
                SQL += ComNum.VBLF + "            WHERE PTNO = '" + argPTNO + "' ";
                SQL += ComNum.VBLF + "                  AND (CHARTDATE = '" + argINDATE.Replace("-", "") + "' ";
                SQL += ComNum.VBLF + "                       OR (CHARTDATE >= TO_CHAR(TRUNC(SYSDATE - 3),'YYYYMMDD') AND CHARTDATE <= TO_CHAR(TRUNC(SYSDATE),'YYYYMMDD'))) ";
                SQL += ComNum.VBLF + "                  AND EXISTS ";
                SQL += ComNum.VBLF + "                         (SELECT * ";
                SQL += ComNum.VBLF + "                            FROM KOSMOS_EMR.AEMRCHARTROW S2 ";
                SQL += ComNum.VBLF + "                           WHERE S1.EMRNO = S2.EMRNO ";
                SQL += ComNum.VBLF + "                                 AND S2.ITEMCD = 'I0000000418' ";
                SQL += ComNum.VBLF + "                                 AND ITEMVALUE IS NOT NULL)) ";
                SQL += ComNum.VBLF + "       AND ITEMCD = 'I0000000418' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = dt.Rows[0]["WEIGHT"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
            return RtnVal;
        }

        public static double Fixed(double arg)
        {
            string strTmp = "";

            strTmp = arg.ToString("N3");

            return double.Parse(strTmp);
        }

        /// <summary>
        /// 외래 Vital 신장/체중값 가져와서 BSA 계산하기 KMC 2020-12-23
        /// </summary>
        /// <param name="argPtno"></param>
        /// <param name="argBDate"></param>
        /// <returns></returns>
        public static string Get_Opd_BSA(string argPtno, string argBDate)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strHeight = "";
            string strWeight = "";
            string rtnVal = "";

            try
            {
                SQL = " SELECT  "; 
                SQL = SQL + ComNum.VBLF + "  (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000000418') AS WEIGHT,  ";  //체중
                SQL = SQL + ComNum.VBLF + "  (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000000002') AS HEIGHT   ";  //신장
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRCHARTMST A  ";
                SQL = SQL + ComNum.VBLF + " WHERE A.FORMNO = 1562 ";
                SQL = SQL + ComNum.VBLF + "   AND A.PTNO = '" + argPtno + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE >= '" + VB.Replace(argBDate, "-", "").Trim() + "'";
                SQL = SQL + ComNum.VBLF + "   AND TRIM((SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000000418')) IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM((SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000000002')) IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.CHARTDATE DESC, A.CHARTTIME DESC";

                //(SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000000418') AS COL10,
                //(SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000000002') AS COL11


                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    
                    return "";
                }
                else
                {
                    strHeight = (dt.Rows[0]["HEIGHT"].ToString() + "").Trim();
                    strWeight = (dt.Rows[0]["WEIGHT"].ToString() + "").Trim();
                }

                rtnVal = GetBSA(strHeight, strWeight);

                dt.Dispose();
                dt = null;

                if (rtnVal != "")
                {
                    rtnVal += "㎡";
                }

                return rtnVal;
            }
            catch
            {
                return "Error";
            }
        }

        public static string Get_Ipd_BSA(string argPtno, string argBDate)
        {
            string rtnVal = "";
            string strHeight = "";
            string strWeight = "";

            try
            {
                strHeight = READ_IPD_HEIGHT(clsDB.DbCon, argPtno, argBDate);
                strWeight = READ_IPD_WEIGHT(clsDB.DbCon, argPtno, argBDate);

                rtnVal = GetBSA(strHeight, strWeight);

                if (rtnVal != "")
                {
                    rtnVal += "㎡";
                }

                return rtnVal;
            }
            catch
            {
                return "Error";
            }
        }

        public static string Get_Opd_Body(string argPtno, string argBDate)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strHeight = "";
            string strWeight = "";
            string rtnVal = "";

            try
            {
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "  (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000000418') AS WEIGHT,  ";  //체중
                SQL = SQL + ComNum.VBLF + "  (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000000002') AS HEIGHT   ";  //신장
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRCHARTMST A  ";
                SQL = SQL + ComNum.VBLF + " WHERE A.FORMNO = 1562 ";
                SQL = SQL + ComNum.VBLF + "   AND A.PTNO = '" + argPtno + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE >= '" + VB.Replace(argBDate, "-", "").Trim() + "'";
                SQL = SQL + ComNum.VBLF + "   AND TRIM((SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000000418')) IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM((SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000000002')) IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.CHARTDATE DESC, A.CHARTTIME DESC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;

                    return "";
                }
                else
                {
                    strHeight = (dt.Rows[0]["HEIGHT"].ToString() + "").Trim();
                    strWeight = (dt.Rows[0]["WEIGHT"].ToString() + "").Trim();
                }

                rtnVal = strHeight + "^^" + strWeight;

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        public static string Get_Ipd_Body(string argPtno, string argInDate)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strHeight = "";
            string strWeight = "";
            string rtnVal = "";

            try
            {
                SQL = " WITH TEMP_DATA AS ( ";
                SQL = SQL + ComNum.VBLF + " SELECT   A.CHARTDATE, R.ITEMVALUE AS HEIGHT, R2.ITEMVALUE AS WEIGHT ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRCHARTMST A         ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_EMR.AEMRCHARTROW R    ";
                SQL = SQL + ComNum.VBLF + "    ON R.EMRNO = A.EMRNO                 ";
                SQL = SQL + ComNum.VBLF + "   AND R.EMRNOHIS = A.EMRNOHIS           ";
                SQL = SQL + ComNum.VBLF + "   AND R.ITEMCD = 'I0000000002' --신장   ";
                SQL = SQL + ComNum.VBLF + "   AND R.ITEMVALUE IS NOT NULL           ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_EMR.AEMRCHARTROW R2   ";
                SQL = SQL + ComNum.VBLF + "    ON R2.EMRNO = A.EMRNO                ";
                SQL = SQL + ComNum.VBLF + "   AND R2.EMRNOHIS = A.EMRNOHIS                  ";
                SQL = SQL + ComNum.VBLF + "   AND R2.ITEMCD = 'I0000000418' --체중          ";
                SQL = SQL + ComNum.VBLF + "   AND R2.ITEMVALUE IS NOT NULL                  ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPtno + "' -- 등록번호      ";
                SQL = SQL + ComNum.VBLF + "   AND MEDFRDATE >= '" + argInDate + "' --입원일자 ";
                SQL = SQL + ComNum.VBLF + "   AND FORMNO = 3150                     ";
                SQL = SQL + ComNum.VBLF + " )                                       ";
                SQL = SQL + ComNum.VBLF + " SELECT HEIGHT, WEIGHT                   ";
                SQL = SQL + ComNum.VBLF + "   FROM TEMP_DATA                        ";
                SQL = SQL + ComNum.VBLF + "  WHERE CHARTDATE = (SELECT MAX(CHARTDATE) FROM TEMP_DATA) ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;

                    return "";
                }
                else
                {
                    strHeight = (dt.Rows[0]["HEIGHT"].ToString() + "").Trim();
                    strWeight = (dt.Rows[0]["WEIGHT"].ToString() + "").Trim();
                }

                rtnVal = strHeight + "^^" + strWeight;

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        /// <summary>
        /// BAS_PATIENT 수정하기 전 대상 컬럼간 비교하여 이전 데이터 백업. 2021-09-07
        /// 딕셔너리 키는 => PANO, SNAME, SEX, BIRTH, JUMIN1, JUMIN2, JUMIN3, ZIPCODE1, ZIPCODE2, ZIPCODE3, JUSO, TEL, HPHONE, BUILDNO
        /// </summary>
        /// <param name="strPANO">등록번호</param>
        /// <param name="dict">키, 값</param>
        /// <returns></returns>
        public void INSERT_BAS_PATIENT_HIS(string strPANO, Dictionary<string, string> dict)
        {
            #region 사용 시 참고하세요.
            // 1) 딕셔너리 키로 사용되는 키값입니다. 다른 키값은 체크 안합니다.
            // PANO, SNAME, SEX, BIRTH, JUMIN1, JUMIN2, JUMIN3, ZIPCODE1, ZIPCODE2, ZIPCODE3, JUSO, TEL, HPHONE, BUILDNO, ROADDETAIL
            // 2) BAS_PATIENT 테이블 업데이트 쿼리 전에 사용됩니다.
            // 3) 기존 BAS_PATIENT 및 변경한  COLUMN의 값 2개의 로우가 저장이 됩니다. 
            #endregion

            DataTable dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strSNAME = "";
            string strSEX = "";
            string strBIRTH = "";
            string strJUMIN1 = "";
            string strJUMIN2 = "";
            string strJUMIN3 = "";
            string strZIPCODE1 = "";
            string strZIPCODE2 = "";
            string strZIPCODE3 = "";
            string strJUSO = "";
            string strTEL = "";
            string strHPHONE = "";
            string strBUILDNO = "";
            string strROADDETAIL = "";

            string strCHANGE = "N";

            // 주소에 띄워쓰기도 변경내역으로 저장되어 공백 제거 후 비교하기 위함
            //2021-09-14
            string strJUSO_Dict = "";       //딕셔너리 주소
            string strJUSO_Patient = "";    //BAS_PATIENT 주소

            
            SQL = " SELECT CODE ";
            SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_BCODE ";
            SQL += ComNum.VBLF + " WHERE GUBUN = 'BAS_환자인적정보로그' ";
            SQL += ComNum.VBLF + "   AND CODE = 'USE' ";
            SQL += ComNum.VBLF + "   AND NAME = 'Y' ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                dt.Dispose();
                dt = null;
            }
            else
            {
                dt.Dispose();
                dt = null;
                return;
            }

            SQL = " SELECT PANO, SNAME, SEX, TO_CHAR(BIRTH,'YYYY-MM-DD') BIRTH, ";
            SQL += ComNum.VBLF + " JUMIN1, JUMIN2, JUMIN3, ZIPCODE1, ";
            SQL += ComNum.VBLF + " ZIPCODE2, ZIPCODE3, JUSO, TEL, ";
            SQL += ComNum.VBLF + " HPHONE, BUILDNO, ROADDETAIL ";
            SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_PATIENT ";
            SQL += ComNum.VBLF + " WHERE PANO = '" + strPANO + "' ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                strSNAME = dt.Rows[0]["SNAME"].ToString().Trim();
                if (dict.ContainsKey("SNAME"))
                {
                    if (dict["SNAME"].Trim() != strSNAME) strCHANGE = "Y";
                }

                strSEX = dt.Rows[0]["SEX"].ToString().Trim();
                if (dict.ContainsKey("SEX"))
                {
                    if (dict["SEX"].Trim() != strSEX) strCHANGE = "Y";
                }

                strBIRTH = dt.Rows[0]["BIRTH"].ToString().Trim();
                if (dict.ContainsKey("BIRTH"))
                {
                    if (dict["BIRTH"].Trim() != strBIRTH) strCHANGE = "Y";
                }

                strJUMIN1 = dt.Rows[0]["JUMIN1"].ToString().Trim();
                if (dict.ContainsKey("JUMIN1"))
                {
                    if (dict["JUMIN1"].Trim() != strJUMIN1) strCHANGE = "Y";
                }

                strJUMIN2 = dt.Rows[0]["JUMIN2"].ToString().Trim();
                if (dict.ContainsKey("JUMIN2"))
                {
                    if (dict["JUMIN2"].Trim() != strJUMIN2) strCHANGE = "Y";
                }

                strJUMIN3 = dt.Rows[0]["JUMIN3"].ToString().Trim();
                if (dict.ContainsKey("JUMIN3"))
                {
                    if (dict["JUMIN3"].Trim() != strJUMIN3) strCHANGE = "Y";
                }

                strZIPCODE1 = dt.Rows[0]["ZIPCODE1"].ToString().Trim();
                if (dict.ContainsKey("ZIPCODE1"))
                {
                    if (dict["ZIPCODE1"].Trim() != strZIPCODE1) strCHANGE = "Y";
                }

                strZIPCODE2 = dt.Rows[0]["ZIPCODE2"].ToString().Trim();
                if (dict.ContainsKey("ZIPCODE2"))
                {
                    if (dict["ZIPCODE2"].Trim() != strZIPCODE2) strCHANGE = "Y";
                }

                strZIPCODE3 = dt.Rows[0]["ZIPCODE3"].ToString().Trim();
                if (dict.ContainsKey("ZIPCODE3"))
                {
                    if (dict["ZIPCODE3"].Trim() != strZIPCODE3) strCHANGE = "Y";
                }

                strJUSO = dt.Rows[0]["JUSO"].ToString().Trim();
                if (dict.ContainsKey("JUSO"))
                {
                    //if (dict["JUSO"].Trim() != strJUSO) strCHANGE = "Y";
                    strJUSO_Dict = dict["JUSO"].Trim().Replace(" ", "");
                    strJUSO_Patient = strJUSO.Trim().Replace(" ", "");
                    if (strJUSO_Dict != strJUSO_Patient)
                    {
                        //공백 제거하고 비교.
                        strCHANGE = "Y";
                    }
                }
                

                strTEL = dt.Rows[0]["TEL"].ToString().Trim();
                if (dict.ContainsKey("TEL"))
                {
                    if (dict["TEL"].Trim() != strTEL) strCHANGE = "Y";
                }

                strHPHONE = dt.Rows[0]["HPHONE"].ToString().Trim();
                if (dict.ContainsKey("HPHONE"))
                {
                    if (dict["HPHONE"].Trim() != strHPHONE) strCHANGE = "Y";
                }

                strBUILDNO = dt.Rows[0]["BUILDNO"].ToString().Trim();
                if (dict.ContainsKey("BUILDNO"))
                {
                    if (dict["BUILDNO"].Trim() != strBUILDNO) strCHANGE = "Y";
                }

                strROADDETAIL = dt.Rows[0]["ROADDETAIL"].ToString().Trim();
                if (dict.ContainsKey("ROADDETAIL"))
                {
                    if (dict["ROADDETAIL"].Trim() != strROADDETAIL) strCHANGE = "Y";
                }

                if (strCHANGE == "Y")
                {
                    SQL = " INSERT INTO KOSMOS_PMPA.BAS_PATIENT_HIS( ";
                    SQL += ComNum.VBLF + " PANO, SNAME, SEX, BIRTH, ";
                    SQL += ComNum.VBLF + " JUMIN1, JUMIN2, JUMIN3, ZIPCODE1, ";
                    SQL += ComNum.VBLF + " ZIPCODE2, ZIPCODE3, JUSO, TEL, ";
                    SQL += ComNum.VBLF + " HPHONE, BUILDNO, WEBSEND, CHGDATE, ";
                    SQL += ComNum.VBLF + " ROADDETAIL, CHGIDNUMBER ";
                    SQL += ComNum.VBLF + " ) VALUES ( ";
                    SQL += ComNum.VBLF + "'" + strPANO + "','" + strSNAME + "','" + strSEX + "',TO_DATE('" + strBIRTH + "','YYYY-MM-DD'),";
                    SQL += ComNum.VBLF + "'" + strJUMIN1 + "','" + strJUMIN2 + "','" + strJUMIN3 + "','" + strZIPCODE1 + "',";
                    SQL += ComNum.VBLF + "'" + strZIPCODE2 + "','" + strZIPCODE3 + "','" + strJUSO + "','" + strTEL + "',";
                    SQL += ComNum.VBLF + "'" + strHPHONE + "','" + strBUILDNO + "','Y', SYSDATE, ";
                    SQL += ComNum.VBLF + "'" + strROADDETAIL + "','" + VB.Right(clsType.User.Sabun.Trim(), 5) + "'";
                    SQL += ComNum.VBLF + ")";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                }
                else
                {
                    return;
                }




                #region 수정한 내용의 컬럼만 저장
                if (dict.ContainsKey("SNAME"))
                {
                    if (dict["SNAME"].Trim() != strSNAME)
                    {
                        strSNAME = dict["SNAME"].Trim();
                    }
                    else
                    {
                        strSNAME = "";
                    }
                }
                else
                {
                    strSNAME = "";
                }

                if (dict.ContainsKey("SEX"))
                {
                    if (dict["SEX"].Trim() != strSEX)
                    {
                        strSEX = dict["SEX"].Trim();
                    }
                    else
                    {
                        strSEX = "";
                    }
                }
                else
                {
                    strSEX = "";
                }

                if (dict.ContainsKey("BIRTH"))
                {
                    if (dict["BIRTH"].Trim() != strBIRTH)
                    {
                        strBIRTH = dict["BIRTH"].Trim();
                    }
                    else
                    {
                        strBIRTH = "";
                    }
                }
                else
                {
                    strBIRTH = "";
                }

                if (dict.ContainsKey("JUMIN1"))
                {
                    if (dict["JUMIN1"].Trim() != strJUMIN1)
                    {
                        strJUMIN1 = dict["JUMIN1"].Trim();
                    }
                    else
                    {
                        strJUMIN1 = "";
                    }
                }
                else
                {
                    strJUMIN1 = "";
                }

                if (dict.ContainsKey("JUMIN2"))
                {
                    if (dict["JUMIN2"].Trim() != strJUMIN2)
                    {
                        strJUMIN2 = dict["JUMIN2"].Trim();
                    }
                    else
                    {
                        strJUMIN2 = "";
                    }
                }
                else
                {
                    strJUMIN2 = "";
                }

                if (dict.ContainsKey("JUMIN3"))
                {
                    if (dict["JUMIN3"].Trim() != strJUMIN3)
                    {
                        strJUMIN3 = dict["JUMIN3"].Trim();
                    }
                    else
                    {
                        strJUMIN3 = "";
                    }
                }
                else
                {
                    strJUMIN3 = "";
                }

                if (dict.ContainsKey("ZIPCODE1"))
                {
                    if (dict["ZIPCODE1"].Trim() != strZIPCODE1)
                    {
                        strZIPCODE1 = dict["ZIPCODE1"].Trim();
                    }
                    else
                    {
                        strZIPCODE1 = "";
                    }
                }
                else
                {
                    strZIPCODE1 = "";
                }

                if (dict.ContainsKey("ZIPCODE2"))
                {
                    if (dict["ZIPCODE2"].Trim() != strZIPCODE2)
                    {
                        strZIPCODE2 = dict["ZIPCODE2"].Trim();
                    }
                    else
                    {
                        strZIPCODE2 = "";
                    }
                }
                else
                {
                    strZIPCODE2 = "";
                }

                if (dict.ContainsKey("ZIPCODE3"))
                {
                    if (dict["ZIPCODE3"].Trim() != strZIPCODE3)
                    {
                        strZIPCODE3 = dict["ZIPCODE3"].Trim();
                    }
                    else
                    {
                        strZIPCODE3 = "";
                    }
                }
                else
                {
                    strZIPCODE3 = "";
                }

                if (dict.ContainsKey("JUSO"))
                {
                    //if (dict["JUSO"].Trim() != strJUSO)
                    //{
                    //    strJUSO = dict["JUSO"].Trim();
                    //}
                    strJUSO_Dict = dict["JUSO"].Trim().Replace(" ", "");
                    strJUSO_Patient = strJUSO.Trim().Replace(" ", "");
                    if (strJUSO_Dict != strJUSO_Patient)
                    {
                        strJUSO = dict["JUSO"].Trim();
                    }
                    else
                    {
                        strJUSO = "";
                    }
                }
                else
                {
                    strJUSO = "";
                }

                if (dict.ContainsKey("TEL"))
                {
                    if (dict["TEL"].Trim() != strTEL)
                    {
                        strTEL = dict["TEL"].Trim();
                    }
                    else
                    {
                        strTEL = "";
                    }
                }
                else
                {
                    strTEL = "";
                }

                if (dict.ContainsKey("HPHONE"))
                {
                    if (dict["HPHONE"].Trim() != strHPHONE)
                    {
                        strHPHONE = dict["HPHONE"].Trim();
                    }
                    else
                    {
                        strHPHONE = "";
                    }
                }
                else
                {
                    strHPHONE = "";
                }

                if (dict.ContainsKey("BUILDNO"))
                {
                    if (dict["BUILDNO"].Trim() != strBUILDNO)
                    {
                        strBUILDNO = dict["BUILDNO"].Trim();
                    }
                    else
                    {
                        strBUILDNO = "";
                    }
                }
                else
                {
                    strBUILDNO = "";
                }

                if (dict.ContainsKey("ROADDETAIL"))
                {
                    if (dict["ROADDETAIL"].Trim() != strROADDETAIL)
                    {
                        strROADDETAIL = dict["ROADDETAIL"].Trim();
                    }
                    else
                    {
                        strROADDETAIL = "";
                    }
                }
                else
                {
                    strROADDETAIL = "";
                }

                SQL = " INSERT INTO KOSMOS_PMPA.BAS_PATIENT_HIS( ";
                SQL += ComNum.VBLF + " PANO, SNAME, SEX, BIRTH, ";
                SQL += ComNum.VBLF + " JUMIN1, JUMIN2, JUMIN3, ZIPCODE1, ";
                SQL += ComNum.VBLF + " ZIPCODE2, ZIPCODE3, JUSO, TEL, ";
                SQL += ComNum.VBLF + " HPHONE, BUILDNO, WEBSEND, CHGDATE, ";
                SQL += ComNum.VBLF + " ROADDETAIL, CHGIDNUMBER ";
                SQL += ComNum.VBLF + " ) VALUES ( ";
                SQL += ComNum.VBLF + "'" + strPANO + "','" + strSNAME + "','" + strSEX + "',TO_DATE('" + strBIRTH + "','YYYY-MM-DD'),";
                SQL += ComNum.VBLF + "'" + strJUMIN1 + "','" + strJUMIN2 + "','" + strJUMIN3 + "','" + strZIPCODE1 + "',";
                SQL += ComNum.VBLF + "'" + strZIPCODE2 + "','" + strZIPCODE3 + "','" + strJUSO + "','" + strTEL + "',";
                SQL += ComNum.VBLF + "'" + strHPHONE + "','" + strBUILDNO + "','O', SYSDATE, ";
                SQL += ComNum.VBLF + "'" + strROADDETAIL + "','" + VB.Right(clsType.User.Sabun.Trim(), 5) + "'";     //수정한 컬럼의 내용만 저장
                SQL += ComNum.VBLF + ")";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon); 
                #endregion

            }

            dt.Dispose();
            dt = null;
        }

        public void FUNC_GBSMS_HISTORY(PsmhDb pDbCon, string ArgPtno, string ArgGBSMS)
        {
            //2021-11-25
            //개인정보 동의 여부 변경 내역 남기는 기능(원무팀 전용입니다. 필요시 사용하셔도 됩니다~)

            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strGBSMS = "";

            SQL = " SELECT GBSMS ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
            SQL += ComNum.VBLF + "  WHERE PANO         = '" + ArgPtno + "' ";
            SqlErr = clsDB.GetDataTableEx(ref DtQ, SQL, pDbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                DtQ.Dispose();
                DtQ = null;
                return;
            }
            if (DtQ.Rows.Count > 0)
            {
                strGBSMS = DtQ.Rows[0]["GBSMS"].ToString().Trim();
            }

            DtQ.Dispose();
            DtQ = null;

            if (strGBSMS != ArgGBSMS)
            {
                //History에 INSERT
                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO KOSMOS_PMPA.BAS_SMS_APPROVE(Pano, AppGbn, Tel, HPhone, Email, EntDate, EntSabun, Gubun) ";
                SQL = SQL + ComNum.VBLF + " SELECT ";
                SQL = SQL + ComNum.VBLF + "     Pano, ";
                SQL = SQL + ComNum.VBLF + "     '" + ArgGBSMS + "', ";
                SQL = SQL + ComNum.VBLF + "     Tel, ";
                SQL = SQL + ComNum.VBLF + "     Hphone, ";
                SQL = SQL + ComNum.VBLF + "     Email, ";
                SQL = SQL + ComNum.VBLF + "     SYSDATE, ";
                SQL = SQL + ComNum.VBLF + "     '" + clsType.User.Sabun + "', ";
                SQL = SQL + ComNum.VBLF + "     '0' ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + ArgPtno + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장}
                }

            }

        }

    }
}
