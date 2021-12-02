using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanMain_Total.cs
/// Description     : 종합검진 판정메인
/// Author          : 김민철
/// Create Date     : 2021-03-10
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "MDIForm1.frm(MDIForm1)" />

namespace HC_Pan
{
    public partial class frmHcPanMain_Total : Form, MainFormMessage
    {
        string mPara1 = "";

        clsHaBase cHB = null;
        clsHcFunc cHF = null;
        ComFunc CF = null;

        HicBcodeService hicBcodeService = null;
        HeaResultService heaResultService = null;
        XrayResultnewService xrayResultnewService = null;
        ComHpcLibBService comHpcLibBService = null;
        InsaMstService insaMstService = null;
        HicResultService hicResultService = null;
        EtcJupmstService etcJupmstService = null;
        XrayResultnewDrService xrayResultnewDrService = null;
        HeaJepsuService heaJepsuService = null;
        ExamAnatmstService examAnatmstService = null;
        BasBcodeService basBcodeService = null;
        HicXrayResultService hicXrayResultService = null;

        frmHaExamResultReg fHaExamResultReg = null;
        frmHaExamResultReg_New fHaExamResultReg_New = null;
        frmHaHyangjoengApproval FrmHaHyangjoengApproval = null;

        string FstrXRayCodeList;

        #region //MainFormMessage
        public MainFormMessage mCallForm = null;

        public void MsgActivedForm(Form frm)
        {
        }
        public void MsgUnloadForm(Form frm)
        {
            frm.Dispose();
            frm = null;
        }
        public void MsgFormClear()
        {
        }
        public void MsgSendPara(string strPara)
        {
        }
        #endregion //MainFormMessage

        public frmHcPanMain_Total()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcPanMain_Total(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
            SetEvent();
            SetControl();
        }

        public frmHcPanMain_Total(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.menuExit.Click += new EventHandler(eMenuClick);
            this.menuPanjeng.Click += new EventHandler(eMenuClick);
            this.menuSetReg.Click += new EventHandler(eMenuClick);
            this.menuResultReg.Click += new EventHandler(eMenuClick);
            this.menuEKG.Click += new EventHandler(eMenuClick);
            this.menuHyang.Click += new EventHandler(eMenuClick);
            this.menuEndoSend.Click += new EventHandler(eMenuClick);
            this.menuConsent00.Click += new EventHandler(eMenuClick);
            this.MenuMail.Click += new EventHandler(eMenuClick);
            this.MenuXSend.Click += new EventHandler(eMenuClick);
            this.MenuEndoSend2.Click += new EventHandler(eMenuClick);
            this.MenuEtcSend.Click += new EventHandler(eMenuClick);
            this.MenuEtcView.Click += new EventHandler(eMenuClick);
            
        }

        void SetControl()
        {
            cHB = new clsHaBase();
            cHF = new clsHcFunc();
            CF = new ComFunc();

            hicBcodeService = new HicBcodeService();
            heaResultService = new HeaResultService();
            xrayResultnewService = new XrayResultnewService();
            comHpcLibBService = new ComHpcLibBService();
            insaMstService = new InsaMstService();
            hicResultService = new HicResultService();
            etcJupmstService = new EtcJupmstService();
            xrayResultnewDrService = new XrayResultnewDrService();
            heaJepsuService = new HeaJepsuService();
            examAnatmstService = new ExamAnatmstService();
            basBcodeService = new BasBcodeService();
            hicXrayResultService = new HicXrayResultService();

            fHaExamResultReg = new frmHaExamResultReg();
            fHaExamResultReg_New = new frmHaExamResultReg_New();
        }

        private void eMenuClick(object sender, EventArgs e)
        {
            if (sender == menuExit)
            {
                this.Close();
                return;
            }
            else if (sender == menuPanjeng) 
            {
                if (fHaExamResultReg_New == null)
                {
                    themTabForm(fHaExamResultReg_New, this.panMain);
                }
                else
                {
                    if (FormIsExist(fHaExamResultReg_New) == true)
                    {
                        FormVisiable(fHaExamResultReg_New);
                    }
                    else
                    {
                        fHaExamResultReg_New = new frmHaExamResultReg_New();
                        themTabForm(fHaExamResultReg_New, this.panMain);
                        FormVisiable(fHaExamResultReg_New);
                    }
                }
            } //종검판정
            else if (sender == menuSetReg)
            {
                frmHaUseWard_New f = new frmHaUseWard_New();
                f.ShowDialog(this);
            }
            //결과등록
            else if (sender == menuResultReg)       
            {
                if (fHaExamResultReg == null)
                {
                    fHaExamResultReg = new frmHaExamResultReg();
                    themTabForm(fHaExamResultReg, this.panMain);
                }
                else
                {
                    if (FormIsExist(fHaExamResultReg) == true)
                    {
                        FormVisiable(fHaExamResultReg);
                    }
                    else
                    {
                        fHaExamResultReg = new frmHaExamResultReg();
                        themTabForm(fHaExamResultReg, this.panMain);
                        FormVisiable(fHaExamResultReg);
                    }
                }

            }   //결과입력
            else if (sender == menuEKG)
            {
                frmHaResultEKG f = new frmHaResultEKG();
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
            }
            else if (sender == menuHyang)
            {
                FrmHaHyangjoengApproval = new frmHaHyangjoengApproval();
                FrmHaHyangjoengApproval.ShowDialog(this);
            }
            else if (sender == menuEndoSend)
            {
                frmHcSangEndoOrderClose f = new frmHcSangEndoOrderClose();
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
            }
            else if (sender == menuConsent01)
            {
                frmHcEmrConset_Rec frm = new frmHcEmrConset_Rec(0, "DOCTOR");
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.Show();
            }
            else if (sender == menuConsent02)
            {
                frmHcEmrConsentView frm = new frmHcEmrConsentView();
                frm.ShowDialog();
                cHF.fn_ClearMemory(frm);
            }
            else if (sender == MenuMail)
            {
                frmRegisteredMailFileCreate f = new frmRegisteredMailFileCreate();
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }
            else if (sender == MenuXSend)
            {
                int nREAD = 0;
                string strDate ="";
                string strPano = "";
                string strSEEKDATE = "";
                string strREADDATE = "";
                string strXJong = "";
                string strXCode = "";
                string strResult = "";
                string strResult4 = "";


                if (clsHcVariable.B01_JONGGUM_SABUN == false)
                {
                    MessageBox.Show("종검 직원만 가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //방사선과 코드를 건진코드로 변환 테이블 설정
                XRayCode_2_HicCode_SET();

                if (clsHcVariable.GstrTimerDate != "")
                {
                    strDate = clsHcVariable.GstrTimerDate;
                }
                else
                {
                    strDate = VB.InputBox("전송하실 일자는(결과가 입력되어있으면 전송안됨) ? (YYYY-MM-DD) 예)2008-05-01 : ");
                }

                if (VB.IsDate(strDate) == false) { return; }

                Cursor.Current = Cursors.WaitCursor;

                //분진(Chest-Dust) 판독 결과
                List<HIC_XRAY_RESULT> list4 = hicXrayResultService.GetListItemByHeaPtno(strDate);
                nREAD = list4.Count;

                for (int i = 0; i < nREAD; i++)
                {
                    strPano = list4[i].PTNO.To<string>("");
                    strSEEKDATE = list4[i].JEPDATE.To<string>("").Trim();
                    strXJong = "1";
                    strXCode = "GR2101A";
                    if (list4[i].RESULT1.To<string>("").Trim() == "")
                    {
                        strResult = "";
                    }
                    else
                    {
                        if (list4[i].READDOCT1 > 99001)
                        {
                            BAS_BCODE item = basBcodeService.GetAllByGubunCode("XRAY_외주판독의사", list4[i].READDOCT1.ToString());
                            if (!item.IsNullOrEmpty())
                            {
                                strResult = "[" + item.NAME + "] ☞ ";
                                strResult += "판독분류: " + list4[i].RESULT2.To<string>("") + "/";
                                strResult += "판독분류명: " + list4[i].RESULT3.To<string>("") + " / ";

                                strResult4 = VB.Pstr(list4[i].RESULT4.To<string>(""),".",2);
                                strResult4 = VB.Pstr(strResult4, "DR :", 1);
                                if(list4[i].RESULT3.To<string>("") == "정상")
                                {
                                    strResult += "판독소견: //" ;
                                }
                                else
                                {
                                    strResult += "판독소견: " + strResult4.Trim();
                                }
                            }
                        }
                        else
                        {
                            strResult = "[" + CF.Read_SabunName(clsDB.DbCon, list4[i].READDOCT1.To<string>("")) + "] ☞ ";
                            strResult += "판독분류: " + list4[i].RESULT2.To<string>("") + "/";
                            strResult += "판독분류명: " + list4[i].RESULT3.To<string>("") + " / ";
                            strResult += "판독소견: " + list4[i].RESULT4.To<string>("") + " // ";
                        }
                    }

                    fn_Med_Result_Send(strPano, strSEEKDATE, strXJong, strXCode, strResult, "");
                }


                //방사선 판독 결과
                List<XRAY_RESULTNEW> list = xrayResultnewService.GetItembySeekDate(strDate);

                nREAD = list.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    strResult = "";
                    strPano = list[i].PANO;
                    strSEEKDATE = list[i].SEEKDATE.To<string>("").Trim();
                    strREADDATE = list[i].READDATE.To<string>("").Trim();
                    strXJong = list[i].XJONG.To<string>("").Trim();
                    strXCode = list[i].XCODE.To<string>("").Trim();
                    strResult = list[i].RESULT.To<string>("").Trim() + list[i].RESULT1.To<string>("").Trim();
                    strResult = fn_BlankLine_Delete(strResult);

                    if (strResult.Trim() != "")
                    {
                        fn_Med_Result_Send(strPano, strSEEKDATE, strXJong, strXCode, strResult, strREADDATE);
                    }
                }


                Cursor.Current = Cursors.Default;

            }
            else if (sender == MenuEndoSend2)
            {
                int nREAD = 0;
                string strDate = "";
                string strPano = "";
                string strResult = "";
                string strGbJob = "";
                string strResult1 = "";
                string strResult2 = "";
                string strResult3 = "";
                string strResult4 = "";
                string strResult5 = "";
                string strResult6 = "";
                string strResult6_2 = "";
                string strResult6_3 = "";
                string strRemark = "";
                string strNew = "";

                string strInfo = "";
                string strRESULTDATE = "";
                string strResultDrCode = "";
                string strKorName = "";

                if (clsHcVariable.B01_JONGGUM_SABUN == false)
                {
                    MessageBox.Show("종검 직원만 가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ComFunc.ReadSysDate(clsDB.DbCon);
                strDate = VB.InputBox("전송하실 일자는? (YYYY-MM-DD): ");

                if (VB.IsDate(strDate) == false) { return; }

                if (string.Compare(strDate, clsPublic.GstrSysDate) >= 0)
                {
                    MessageBox.Show("어제까지 내시경 결과만 전송이 가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                nREAD = 0;

                List<ENDO_RESULT_JUPMST> list = comHpcLibBService.GetItembyJDate(strDate);

                Cursor.Current = Cursors.WaitCursor;

                nREAD = list.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    strPano = list[i].PTNO;
                    strDate = list[i].JDATE;
                    strGbJob = list[i].GBJOB;
                    strNew = list[i].GBNEW.To<string>("").Trim();
                    strResultDrCode = list[i].RESULTDRCODE.To<string>("").Trim();

                    //결과입력의사명
                    if (!strResultDrCode.IsNullOrEmpty())
                    {
                        strKorName = insaMstService.GetKornameBySabun(strResultDrCode);
                    }

                    strResult1 = list[i].REMARK1.To<string>("").Trim();
                    strResult2 = list[i].REMARK2.To<string>("").Trim();
                    strResult3 = list[i].REMARK3.To<string>("").Trim();
                    strResult4 = list[i].REMARK4.To<string>("").Trim();
                    strResult5 = list[i].REMARK5.To<string>("").Trim();
                    strResult6 = list[i].REMARK6.To<string>("").Trim(); //Biposy

                    if (strNew == "Y")
                    {
                        strResult6_2 = list[i].REMARK6_2.To<string>("").Trim();
                        strResult6_3 = list[i].REMARK6_3.To<string>("").Trim();
                        strRemark = list[i].REMARK.To<string>("").Trim();
                    }

                    //Premedication--------------------------------------------------------------------------
                    strInfo = "▶Premedication:" + "\r\n";
                    strInfo += list[i].GBPRE_1.To<string>("").Trim() == "Y" ? "None" : "";
                    strInfo += list[i].GBPRE_2.To<string>("").Trim() == "Y" ? "Aigiron " : "";

                    if (list[i].GBPRE_21.To<string>("").Trim() != "")
                    {
                        strInfo += list[i].GBPRE_21 + "mg " + list[i].GBPRE_22 + ", ";
                    }

                    //Conscious Sedation---------------------------------------------------------------------
                    strInfo += "\r\n" + "▶Conscious Sedation:" + "\r\n";
                    strInfo += list[i].GBCON_1.To<string>("").Trim() == "Y" ? "None" : "";
                    strInfo += list[i].GBCON_2.To<string>("").Trim() == "Y" ? "Mediazolam " : "";

                    if (list[i].GBCON_21.To<string>("").Trim() != "")
                    {
                        strInfo += list[i].GBCON_21 + "mg " + list[i].GBCON_22 + ", ";
                    }

                    strInfo += list[i].GBCON_3.To<string>("").Trim() == "Y" ? "Anepol " : "";

                    if (list[i].GBCON_31.To<string>("").Trim() != "")
                    {
                        strInfo += list[i].GBCON_31 + "mg " + list[i].GBCON_32 + ", ";
                    }

                    strInfo += list[i].GBCON_4.To<string>("").Trim() == "Y" ? "Pathidine " : "";

                    if (list[i].GBCON_41.To<string>("").Trim() != "")
                    {
                        strInfo += list[i].GBCON_41 + "mg " + list[i].GBCON_42 + ", ";
                    }

                    switch (strGbJob)
                    {
                        case "1":   //기관지
                            strResult = "▶Vocal Cord:" + Environment.NewLine + strResult1 + Environment.NewLine;
                            strResult += "▶Carina:" + Environment.NewLine + strResult2 + Environment.NewLine;
                            strResult += "▶Bronchi:" + Environment.NewLine + strResult3 + Environment.NewLine;
                            strResult += "▶EndoScopic Procedure:" + Environment.NewLine + strResult4;
                            strResult += "▶EndoScopic Biopsy:" + Environment.NewLine + strResult6;
                            if (strNew == "Y")
                            {
                                strResult = strResult + Environment.NewLine + strResult6_2;
                                strResult = strResult + Environment.NewLine + strResult6_3 + Environment.NewLine;
                                strResult = strResult + "처치의사 : " + strResultDrCode + "  " + strKorName;
                            }
                            break;
                        case "2":   //위
                            if (strNew == "Y")
                            {
                                if (list[i].REMARK6.To<string>("").Trim() != "")
                                {
                                    strResult6 = "Esophagus:" + list[i].REMARK6;
                                }
                                if (list[i].REMARK6_2.To<string>("").Trim() != "")
                                {
                                    strResult6 += Environment.NewLine + "Stomach:" + list[i].REMARK6_2;
                                }
                                if (list[i].REMARK6_3.To<string>("").Trim() != "")
                                {
                                    strResult6 += Environment.NewLine + "Duodenum:" + list[i].REMARK6_3;
                                }
                            }

                            if (strResult1 != "")
                            {
                                strResult = "▶Esophagus:" + Environment.NewLine + strResult1 + Environment.NewLine;
                            }
                            else
                            {
                                strResult = "▶Esophagus:" + Environment.NewLine + strResult1;
                            }
                            if (strResult2 != "")
                            {
                                strResult = strResult + "▶Stomach:" + Environment.NewLine + strResult2 + Environment.NewLine;
                            }
                            else
                            {
                                strResult = strResult + "▶Stomach:" + Environment.NewLine + strResult2;
                            }
                            if (strResult3 != "")
                            {
                                strResult = strResult + "▶Duodenum:" + Environment.NewLine + strResult3 + Environment.NewLine;
                            }
                            else
                            {
                                strResult = strResult + "▶Duodenum:" + Environment.NewLine + strResult3;
                            }
                            if (strResult4 != "")
                            {
                                strResult = strResult + "▶Endoscopic Diagnosis:" + Environment.NewLine + strResult4 + Environment.NewLine;
                            }
                            else
                            {
                                strResult = strResult + "▶Endoscopic Diagnosis:" + Environment.NewLine + strResult4;
                            }
                            strResult = strResult + strInfo + Environment.NewLine; //add


                            if (list[i].PRO_RUT.To<string>("").Trim() == "Y")
                            {
                                if (strResult5 != "")
                                {
                                    strResult = strResult + "▶Endoscopic Procedure:" + Environment.NewLine + "Rapid Urease Test, " + Environment.NewLine + strResult5 + Environment.NewLine;
                                }
                                else
                                {
                                    strResult = strResult + "▶Endoscopic Procedure:" + Environment.NewLine + "Rapid Urease Test, " + Environment.NewLine + strResult5;
                                }
                            }
                            else
                            {
                                if (strResult5 != "")
                                {
                                    strResult = strResult + "▶Endoscopic Procedure:" + Environment.NewLine + strResult5 + Environment.NewLine;
                                }
                                else
                                {
                                    strResult = strResult + "▶Endoscopic Procedure:" + Environment.NewLine + strResult5;
                                }
                            }
                            strResult = strResult + "▶EndoScopic Biopsy:" + Environment.NewLine + strResult6 + Environment.NewLine;


                            if (strNew == "Y")
                            {
                                //참고사항
                                if (strRemark != "")
                                {
                                    strResult = strResult + "▶Remark:" + Environment.NewLine + strRemark + Environment.NewLine;
                                }
                                else
                                {
                                    strResult = strResult + "▶Remark:" + Environment.NewLine + strRemark + Environment.NewLine;
                                }
                                strResult = strResult + "처치의사 : " + strResultDrCode + "  " + strKorName;
                            }
                            break;
                        case "3":   //장
                            if (strNew == "Y")
                            {
                                strResult6 = "";
                                if (list[i].REMARK6.To<string>("").Trim() != "")
                                {
                                    strResult6 = "small Intestinal:" + list[i].REMARK6;
                                }
                                if (list[i].REMARK6_2.To<string>("").Trim() != "")
                                {
                                    strResult6 += Environment.NewLine + "large Intestinal:" + list[i].REMARK6_2;
                                }
                                if (list[i].REMARK6_3.To<string>("").Trim() != "")
                                {
                                    strResult6 += Environment.NewLine + "rectum:" + list[i].REMARK6_3;
                                }

                                if (strResult1 != "")
                                {
                                    strResult = strResult + "▶small Intestinal:" + Environment.NewLine + strResult1 + Environment.NewLine;
                                }
                                else
                                {
                                    strResult = strResult + "▶small Intestinal:" + Environment.NewLine + strResult1;
                                }
                                if (strResult4 != "")
                                {
                                    strResult = strResult + "▶large Intestinal:" + Environment.NewLine + strResult4 + Environment.NewLine;
                                }
                                else
                                {
                                    strResult = strResult + "▶large Intestinal:" + Environment.NewLine + strResult4;
                                }
                                if (strResult5 != "")
                                {
                                    strResult = strResult + "▶rectum:" + Environment.NewLine + strResult5 + Environment.NewLine;
                                }
                                else
                                {
                                    strResult = strResult + "▶rectum:" + Environment.NewLine + strResult5;
                                }
                                if (strResult2 != "")
                                {
                                    strResult = strResult + "▶Endoscopic Diagnosis:" + Environment.NewLine + strResult2 + Environment.NewLine;
                                }
                                else
                                {
                                    strResult = strResult + "▶Endoscopic Diagnosis:" + Environment.NewLine + strResult2;
                                }
                                if (list[i].GB_CLEAN.To<string>("").Trim() != "")
                                {
                                    strResult = strResult + "▶장정결도:" + Environment.NewLine + list[i].GB_CLEAN + Environment.NewLine;   //2013-06-17
                                }
                                else
                                {
                                    strResult = strResult + "▶장정결도:" + Environment.NewLine + list[i].GB_CLEAN;    //2013-06-17
                                }
                                strResult = strResult + strInfo + Environment.NewLine;   //add
                                if (strResult3 != "")
                                {
                                    strResult = strResult + "▶Endoscopic Procedure:" + Environment.NewLine + strResult3 + Environment.NewLine;
                                }
                                else
                                {
                                    strResult = strResult + "▶Endoscopic Procedure:" + Environment.NewLine + strResult3;
                                }
                                if (strResult6 != "")
                                {
                                    strResult = strResult + "▶Endoscopic Biopsy:" + Environment.NewLine + strResult6 + Environment.NewLine;
                                }
                                else
                                {
                                    strResult = strResult + "▶Endoscopic Biopsy:" + Environment.NewLine + strResult6;
                                }
                                if (strNew == "Y")
                                {
                                    //참고사항
                                    if (strRemark != "")
                                    {
                                        strResult = strResult + "▶Remark:" + Environment.NewLine + strRemark + Environment.NewLine;
                                    }
                                    else
                                    {
                                        strResult = strResult + "▶Remark:" + Environment.NewLine + strRemark + Environment.NewLine;
                                    }
                                    strResult = strResult + "처치의사 : " + strResultDrCode + "  " + strKorName;
                                }
                            }
                            break;
                        case "4":   // ERCP
                            strResult = "▶ERCP Finding:" + Environment.NewLine + strResult1 + Environment.NewLine;
                            strResult += "▶Diagnosis:" + Environment.NewLine + strResult2 + Environment.NewLine;
                            strResult += "▶Plan + Tx:" + Environment.NewLine + strResult3 + Environment.NewLine;
                            strResult += "▶EndoScopic Procedure:" + Environment.NewLine + strResult4;
                            strResult += "▶EndoScopic Biopsy:" + Environment.NewLine + strResult6;
                            if (strNew == "Y")
                            {
                                strResult += Environment.NewLine + strResult6_2;
                                strResult += Environment.NewLine + strResult6_3 + Environment.NewLine;
                                strResult += "처치의사 : " + strResultDrCode + "  " + strKorName;
                            }
                            break;
                        default:
                            break;
                    }

                    strResult = fn_BlankLine_Delete(strResult);
                    fn_Med_Endo_Send(strPano, strDate, strResult, strGbJob, strRESULTDATE);

                    strResult = "";
                    strRESULTDATE = "";
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("작업완료", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (sender == MenuEtcSend)
            {
                int nREAD = 0;
                string strPano = "";
                string StrRDate = "";
                string strResult = "";
                string strTemp = "";
                string strROWID = "";
                string strOLD = "";
                string strNew = "";
                string strTempResult = "";
                string strTemp1 = "";

                if (clsHcVariable.B01_JONGGUM_SABUN == false)
                {
                    MessageBox.Show("종검 직원만 가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //=========================
                // 1.스트레스검사 결과 전송
                //=========================
                List<ETC_JUPMST> list = etcJupmstService.GetItemStress();
                Cursor.Current = Cursors.WaitCursor;
                nREAD = list.Count;

                for (int i = 0; i < nREAD; i++)
                {
                    strPano = list[i].PTNO;
                    strResult = list[i].STRESS_SOGEN.To<string>("").Trim();

                    //결과가 있으면 전송을 안함
                    if (strResult != "")
                    {
                        HIC_RESULT list2 = hicResultService.GetResultbyJepsu(strPano, "TX87", 5);

                        if (!list2.IsNullOrEmpty())
                        {
                            if (list2.RESULT.To<string>("").Trim() == "")
                            {
                                strResult = strResult.Replace("1. ", Environment.NewLine + "1. ");
                                strResult = strResult.Replace("2. ", Environment.NewLine + "2. ");
                                strResult = strResult.Replace("3. ", Environment.NewLine + "3. ");
                                strResult = strResult.Replace("1) ", Environment.NewLine + "1) ");
                                strResult = strResult.Replace("2) ", Environment.NewLine + "2) ");
                                strResult = strResult.Replace("3) ", Environment.NewLine + "3) ");
                                strResult = strResult.Replace("4) ", Environment.NewLine + "4) ");
                                strResult = strResult.Replace("5) ", Environment.NewLine + "5) ");
                                strResult = strResult.Replace("6) ", Environment.NewLine + "6) ");
                                strResult = strResult.Replace("7) ", Environment.NewLine + "7) ");
                                strResult = strResult.Replace("8) ", Environment.NewLine + "8) ");
                                strResult = strResult.Replace("9) ", Environment.NewLine + "9) ");

                                int result = hicResultService.UpdatebyRowId(strResult, list2.RID);

                                if (result < 0)
                                {
                                    MessageBox.Show("스트레스검사 UPDATE 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                            }
                        }
                    }
                }

                #region 뇌혈류초음파 별도 전송안함
                ////=================================
                //// 2.TCD(TZ16) 뇌혈류초음파 결과전송
                ////=================================
                //List<ETC_JUPMST> list3 = etcJupmstService.GetItembyRdate();

                //nREAD = list3.Count;
                //for (int i = 0; i < nREAD; i++)
                //{
                //    strPano = list3[i].PTNO;
                //    StrRDate = list3[i].RDATE;

                //    //종검에 결과가 전송된것은 제외
                //    HIC_RESULT list4 = hicResultService.GetResultbyJepsu(strPano, "TZ16", 7);

                //    if (!list4.IsNullOrEmpty())
                //    {
                //        strResult = list4.RESULT.To<string>("").Trim();
                //        strROWID = list4.RID.To<string>("").Trim();
                //    }

                //    //결과가 있으면 전송을 안함
                //    if (strResult.IsNullOrEmpty() && !strROWID.IsNullOrEmpty())
                //    {
                //        ///TODO : 이상훈 (2019.09.26) EMR 확인 필요
                //        //SQL = "SELECT EMRNO FROM KOSMOS_EMR.EMRXML "
                //        //SQL = SQL & "WHERE PTno='" & strPano & "' "
                //        //SQL = SQL & "  AND FormNo=2085 "
                //        //SQL = SQL & "  AND MedDeptCd='TO' "
                //        //SQL = SQL & "  AND CHARTDATE='" & Replace(StrRDate, "-", "") & "' "
                //        //Call AdoOpenSet(rs1, SQL)
                //        //strEmrNo = AdoGetString(rs1, "EmrNo", 0)
                //        //Call AdoCloseSet(rs1)
                //        //strXmlData = ""
                //        //strResult = ""
                //        //If strEmrNo<> "" Then
                //        //   strXmlData = GetXMLData(strEmrNo)
                //        //    If strXmlData<> "" Then
                //        //       strResult = TCD_Result_Edit(strXmlData)
                //        //    End If
                //        //End If

                //        //If strResult<> "" Then
                //        //   SQL = "UPDATE HEA_RESULT SET Result='" & strResult & "' "
                //        //    SQL = SQL & "WHERE ROWID='" & strROWID & "' "
                //        //    Result = AdoExecute(SQL)
                //        //    If Result<> 0 Then
                //        //        adoConnect.RollbackTrans
                //        //        MsgBox "TCD 검사 UPDATE 오류", , "확인"
                //        //    End If
                //        //End If
                //    }

                //    //일반건진 결과가 전송된것은 제외
                //    string[] strCodes = new string[] { "TZ16" };
                //    HIC_RESULT list5 = hicResultService.GetResultRowIdbyPtNo(strPano, strCodes, 7, "HC");

                //    if (!list5.IsNullOrEmpty())
                //    {
                //        strResult = list5.RESULT;
                //        strROWID = list5.RID;
                //    }

                //    //결과가 있으면 전송을 안함
                //    if (strResult.IsNullOrEmpty() && !strROWID.IsNullOrEmpty())
                //    {
                //        ///TODO : 이상훈 (2019.09.26) EMR 확인 필요
                //        //SQL = "SELECT EMRNO FROM KOSMOS_EMR.EMRXML "
                //        //SQL = SQL & "WHERE PTno='" & strPano & "' "
                //        //SQL = SQL & "  AND FormNo=2085 "
                //        //SQL = SQL & "  AND CHARTDATE='" & Replace(StrRDate, "-", "") & "' "
                //        //Call AdoOpenSet(rs1, SQL)
                //        //strEmrNo = AdoGetString(rs1, "EmrNo", 0)
                //        //Call AdoCloseSet(rs1)
                //        //strXmlData = ""
                //        //strResult = ""
                //        //If strEmrNo<> "" Then
                //        //   strXmlData = GetXMLData(strEmrNo)
                //        //    If strXmlData<> "" Then
                //        //       strResult = TCD_Result_Edit(strXmlData)
                //        //    End If
                //        //End If

                //        //If strResult<> "" Then
                //        //   SQL = "UPDATE HIC_RESULT SET Result='" & strResult & "' "
                //        //    SQL = SQL & "WHERE ROWID='" & strROWID & "' "
                //        //    Result = AdoExecute(SQL)
                //        //    If Result<> 0 Then
                //        //        adoConnect.RollbackTrans
                //        //        MsgBox "TCD 검사 UPDATE 오류", , "확인"
                //        //    End If
                //        //End If
                //    }
                //}
                #endregion

                //===================================
                //3.GY-SONO(TX98) 산부인과 초음파
                //===================================
                List<XRAY_RESULTNEW_DR> list6 = xrayResultnewDrService.GetItembyXCodeDeptCode("US24", "TO", 7);
                nREAD = list6.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    strPano = list6[i].PANO;
                    StrRDate = list6[i].READDATE.ToString();

                    //종검에 결과가 전송된것은 제외
                    HIC_RESULT list7 = hicResultService.GetResultbyJepsu(strPano, "TX98", 7);
                    if (!list7.IsNullOrEmpty())
                    {
                        strResult = list7.RESULT.To<string>("").Trim();
                        strROWID = list7.RID.To<string>("").Trim();
                    }

                    //결과가 있으면 전송을 안함
                    if ((strResult.IsNullOrEmpty() || strResult == "") && !strROWID.IsNullOrEmpty())
                    {
                        strResult = list6[i].RESULT.To<string>("").Trim() + list6[i].RESULT1.To<string>("").Trim();

                        if (strResult != "")
                        {
                            int result = hicResultService.UpdatebyRowId(strResult, strROWID);

                            if (result < 0)
                            {
                                MessageBox.Show("TCD 검사 UPDATE 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                    }
                }

                //=================================
                // 4.위조직검사 결과 가져오기
                //=================================
                List<EXAM_ANATMST> list8 = examAnatmstService.GetItembyOrderCode();

                nREAD = list8.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    strNew = list8[i].BDATE.To<string>("").Trim() + "{}" + list8[i].PTNO;
                    strTempResult = list8[i].RESULT1.To<string>("").Trim() + list8[i].RESULT2.To<string>("").Trim();

                    if (strOLD == strNew)
                    {
                        strResult += "{@}" + strTempResult;
                    }
                    else
                    {
                        strPano = VB.Pstr(strOLD, "{}", 2);
                        StrRDate = VB.Pstr(strOLD, "{}", 1);
                        strROWID = "";
                        strTemp = "";

                        //종검에 결과가 전송된것은 제외
                        if (strOLD != "")
                        {
                            string[] strCodes = new string[] { "TX20", "TX23" };
                            HIC_RESULT list9 = hicResultService.GetResultRowIdbyPtNo(strPano, strCodes, 7, "HA");
                            if (!list9.IsNullOrEmpty())
                            {
                                strTemp = list9.RESULT.To<string>("").Trim();
                                strROWID = list9.RID.To<string>("").Trim();
                                if (VB.InStr(strTemp, "▶Biopsy Diagnosis:") > 0) { strTemp = ""; }
                            }
                        }

                        //결과가 있으면 전송을 안함
                        if (strTemp != "" && strROWID != "")
                        {
                            strResult = Jojik_Result_Edit(strResult);
                            if (strResult != "")
                            {
                                //내시경결과 형식에 오류가 있으면 처리을 안함
                                if (VB.InStr(strTemp, "▶EndoScopic Biopsy:") == 0 || VB.InStr(strTemp, "▶Remark:") == 0)
                                {
                                    strResult = "";
                                }
                                else
                                {
                                    strTemp1 = VB.STRCUT(strTemp, "", "▶EndoScopic Biopsy:") + strResult + Environment.NewLine;
                                    strTemp1 += "▶Remark:" + VB.STRCUT(strTemp, "▶Remark:", "");
                                    strResult = strTemp1;
                                }
                            }

                            if (strResult != "")
                            {
                                int result = hicResultService.UpdatebyRowId(strResult, strROWID);

                                if (result < 0)
                                {
                                    MessageBox.Show("위내시경 조직검사 UPDATE 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                            }
                        }
                        strResult = strTempResult;
                        strOLD = strNew;
                    }
                }

                strPano = VB.Pstr(strOLD, "{}", 2);
                StrRDate = VB.Pstr(strOLD, "{}", 1);
                strROWID = "";
                strTemp = "";

                //종검에 결과가 전송된것은 제외
                if (strOLD != "")
                {
                    string[] strCodes = new string[] { "TX20", "TX23" };    //위내시경
                    HIC_RESULT list10 = hicResultService.GetResultRowIdbyPtNo(strPano, strCodes, 7, "HA");

                    if (!list10.IsNullOrEmpty())
                    {
                        strTemp = list10.RESULT.To<string>("").Trim();
                        strROWID = list10.RID.To<string>("").Trim();
                    }

                    if (VB.InStr(strTemp, "▶Biopsy Diagnosis:") > 0)
                    {
                        strTemp = "";
                    }
                }

                //결과가 있으면 전송을 안함
                if (strTemp != "" && strROWID != "")
                {
                    strResult = Jojik_Result_Edit(strResult);
                    if (strResult != "")
                    {
                        //내시경결과 형식에 오류가 있으면 처리을 안함
                        if (VB.InStr(strTemp, "▶EndoScopic Biopsy:") == 0 || VB.InStr(strTemp, "▶Remark:") == 0)
                        {
                            strResult = "";
                        }
                        else
                        {
                            strTemp1 = VB.STRCUT(strTemp, "", "▶EndoScopic Biopsy:") + strResult + Environment.NewLine;
                            strTemp1 += "▶Remark:" + VB.STRCUT(strTemp, "▶Remark:", "");
                            strResult = strTemp1;
                        }
                    }

                    if (strResult != "")
                    {
                        int result = hicResultService.UpdatebyRowId(strResult, strROWID);

                        if (result < 0)
                        {
                            MessageBox.Show("위내시경 조직검사 UPDATE 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                //===============================
                // 5.대장조직검사 결과 가져오기
                //===============================
                List<EXAM_ANATMST> list11 = examAnatmstService.GetItembyOrderCodeGbJob();

                nREAD = list11.Count;
                strOLD = "";
                strResult = "";
                for (int i = 0; i < nREAD; i++)
                {
                    strNew = list11[i].BDATE.ToString() + "{}" + list11[i].PTNO;
                    strTempResult = list11[i].RESULT1.To<string>("").Trim() + list11[i].RESULT2.To<string>("").Trim();
                    if (strOLD == strNew)
                    {
                        strResult += "{@}" + strTempResult;
                    }
                    else
                    {
                        strPano = VB.Pstr(strOLD, "{}", 2);
                        StrRDate = VB.Pstr(strOLD, "{}", 1);
                        strROWID = "";
                        strTemp = "";

                        //종검에 결과가 전송된것은 제외
                        if (strOLD != "")
                        {
                            string[] strCodes = new string[] { "TX64", "TX32" };
                            HIC_RESULT list12 = hicResultService.GetResultRowIdbyPtNo(strPano, strCodes, 7, "HA");    //대장내시경
                            if (!list12.IsNullOrEmpty())
                            {
                                strTemp = list12.RESULT.To<string>("").Trim();
                                strROWID = list12.RID.To<string>("").Trim();
                                if (VB.InStr(strTemp, "▶Biopsy Diagnosis:") > 0)
                                {
                                    strTemp = "";
                                }
                            }

                        }

                        //결과가 있으면 전송을 안함
                        if (strTemp != "" && strROWID != "")
                        {
                            strResult = Jojik_Result_Edit(strResult);
                            if (strResult != "")
                            {
                                //내시경결과 형식에 오류가 있으면 처리을 안함
                                if (VB.InStr(strTemp, "▶Endoscopic Biopsy:") == 0 || VB.InStr(strTemp, "▶Remark:") == 0)
                                {
                                    strResult = "";
                                }
                                else
                                {
                                    strTemp1 = VB.STRCUT(strTemp, "", "▶EndoScopic Biopsy:") + strResult + Environment.NewLine;
                                    strTemp1 += "▶Remark:" + VB.STRCUT(strTemp, "▶Remark:", "");
                                    strResult = strTemp1;
                                }
                                if (strResult != "")
                                {
                                    int result = hicResultService.UpdatebyRowId(strResult, strROWID);

                                    if (result < 0)
                                    {
                                        MessageBox.Show("대장내시경 조직검사 UPDATE 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                }
                            }
                        }
                        strResult = strTempResult;
                        strOLD = strNew;
                    }
                }

                strPano = VB.Pstr(strOLD, "{}", 2);
                StrRDate = VB.Pstr(strOLD, "{}", 1);
                strROWID = "";
                strTemp = "";

                //종검에 결과가 전송된것은 제외
                if (strOLD != "")
                {
                    string[] strCodes = new string[] { "TX64","TX32" };    //대장내시경
                    HIC_RESULT list13 = hicResultService.GetResultRowIdbyPtNo(strPano, strCodes, 7, "HA");
                    if (!list13.IsNullOrEmpty())
                    {
                        strTemp = list13.RESULT.To<string>("").Trim();
                        strROWID = list13.RID.To<string>("").Trim();
                        if (VB.InStr(strTemp, "▶Biopsy Diagnosis:") > 0)
                        {
                            strTemp = "";
                        }
                    }

                }

                //결과가 있으면 전송을 안함
                if (strTemp != "" && strROWID != "")
                {
                    strResult = Jojik_Result_Edit(strResult);
                    if (strResult != "")
                    {
                        if (VB.InStr(strTemp, "▶Endoscopic Biopsy:") == 0 || VB.InStr(strTemp, "▶Remark:") == 0)
                        {
                            strResult = "";
                        }
                        else
                        {
                            strTemp1 = VB.STRCUT(strTemp, "", "▶EndoScopic Biopsy:") + strResult + Environment.NewLine;
                            strTemp1 += "▶Remark:" + VB.STRCUT(strTemp, "▶Remark:", "");
                            strResult = strTemp1;
                        }
                    }
                    if (strResult != "")
                    {
                        int result = hicResultService.UpdatebyRowId(strResult, strROWID);

                        if (result < 0)
                        {
                            MessageBox.Show("대장내시경 조직검사 UPDATE 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("작업완료", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else if (sender == MenuEtcView)
            {
                frmHaSupReadCalender frm = new frmHaSupReadCalender();
                frm.ShowDialog();
            }
        }

        void fn_Med_Result_Send(string ArgPano, string ArgSeekDate, string ArgXJong, string ArgXCode, string ArgResult, string ArgReadDate)
        {
            long nWRTNO = 0;
            string strNewResult = "";
            string strNewResult1 = "";
            string strSDate = "";
            string strEdate = "";
            string strLF = "";
            string strLF2 = "";

            List<string> strExcode = new List<string>();

            strLF = "\n";
            strLF2 = strLF + strLF;

            strSDate = DateTime.Parse(ArgSeekDate).AddDays(-7).ToShortDateString();
            strEdate = DateTime.Parse(ArgSeekDate).AddDays(7).ToShortDateString();

            //======================================
            // 방사선코드를 건진코드로 변환정보 찾기
            //======================================

            strExcode.Clear();
            string strExCodes = VB.STRCUT(FstrXRayCodeList, "{@}" + ArgXCode + "{}", "{@}").Trim();

            if (strExCodes == "")
            {
                strExCodes = VB.STRCUT(FstrXRayCodeList, "{@}[" + ArgXJong + "]{}", "{@}").Trim();
            }

            if (strExCodes != "")
            {
                if (VB.L(strExCodes, ",") > 1)
                {
                    for (int i = 1; i <= VB.L(strExCodes, ","); i++)
                    {
                        strExcode.Add(VB.Pstr(strExCodes, ",", i).Trim());
                    }
                }
                else
                {
                    strExcode.Add(strExCodes);
                }
            }

            if (strExcode.Count == 0) { return; }

            //종합검진 번호를 찾기
            HEA_JEPSU list = heaJepsuService.GetWrtNobySDatebyPtNo(ArgPano, strSDate, strEdate);

            nWRTNO = 0;
            if (list != null)
            {
                nWRTNO = list.WRTNO;
                strSDate = list.SDATE.ToString();
            }

            if (nWRTNO == 0) { return; }

            strNewResult = ArgResult.Trim();

            for (int i = 1; i <= 10; i++)
            {
                strNewResult1 = strNewResult.Replace(strLF2, strLF);
                strNewResult1 = strNewResult1.Replace("  ", " ");
            }


            //안저검사일때 판독문 결과만(정상만 표시)
            if (ArgXJong == "F")
            {
                strNewResult1 = VB.Pstr(VB.UCase(strNewResult), "(BOTH)", 2);
                if (strNewResult1 != "정상") { strNewResult1 = ""; }
            }

            if (strNewResult1 != "")
            {
                HEA_RESULT list2 = heaResultService.GetResultBYWrtnoExCodeIN(nWRTNO, strExcode);

                if (!list2.IsNullOrEmpty())
                {
                    //이미 결과가 있으면
                    if (list2.RESULT.To<string>("").Trim() == "" || list2.RESULT.To<string>("").Trim() == "NORMAL")
                    {
                        //자료를 UPDATE
                        HEA_RESULT item = new HEA_RESULT
                        {
                            RESULT = strNewResult1,
                            ENTSABUN = long.Parse(clsType.User.IdNumber),
                            WRTNO = nWRTNO,
                            READTIME = ArgReadDate,
                            ACTIVE = "Y"
                        };

                        int result = heaResultService.UpDateResultReadTimeByItemExCodeIN(item, strExcode);

                        if (result < 0)
                        {
                            MessageBox.Show("결과 저장중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }
        }

        void fn_Med_Endo_Send(string ArgPano, string argDate, string ArgResult, string ArgGbn, string ArgReadTime)
        {
            long nWRTNO = 0;

            nWRTNO = heaJepsuService.GetWrtNobySDatePano(ArgPano, argDate);

            if (nWRTNO != 0)
            {
                if (ArgGbn == "3")
                {
                    List<string> lstCodes = new List<string> { "TX32", "TX64" };
                    HEA_RESULT list = heaResultService.GetResultBYWrtnoExCodeIN(nWRTNO, lstCodes);

                    if (!list.IsNullOrEmpty())
                    {
                        if (list.RESULT.IsNullOrEmpty() && list.RESULT.To<string>("").Trim() == "")
                        {
                            HEA_RESULT item = new HEA_RESULT
                            {
                                RESULT = ArgResult,
                                ENTSABUN = long.Parse(clsType.User.IdNumber),
                                WRTNO = nWRTNO,
                                READTIME = ArgReadTime,
                                ACTIVE = "Y"
                            };

                            int result = heaResultService.UpDateResultReadTimeByItemExCodeIN(item, lstCodes);

                            if (result < 0)
                            {
                                MessageBox.Show("결과 입력 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    List<string> lstCodes = new List<string> { "TX20", "TX23" };
                    HEA_RESULT list = heaResultService.GetResultBYWrtnoExCodeIN(nWRTNO, lstCodes);

                    if (!list.IsNullOrEmpty())
                    {
                        if (list.RESULT.IsNullOrEmpty() && list.RESULT.To<string>("").Trim() == "")
                        {
                            HEA_RESULT item = new HEA_RESULT
                            {
                                RESULT = ArgResult,
                                ENTSABUN = long.Parse(clsType.User.IdNumber),
                                WRTNO = nWRTNO,
                                READTIME = ArgReadTime,
                                ACTIVE = "Y"
                            };

                            int result = heaResultService.UpDateResultReadTimeByItemExCodeIN(item, lstCodes);

                            if (result < 0)
                            {
                                MessageBox.Show("결과 입력 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 조직검사 검사결과 편집
        /// </summary>
        /// <param name="argResult"></param>
        /// <returns></returns>
        string Jojik_Result_Edit(string argResult)
        {
            string rtnVal = "";

            string strCUT1 = "";
            string strResTemp = "";
            string strTemp = "";
            string strResult = "";
            string strFollow = "";
            long nResCnt = 0;
            long nCNT = 0;
            string strREC = "";

            strResult = "";
            nResCnt = VB.L(argResult, "{@}");

            for (int inx = 1; inx <= nResCnt; inx++)
            {
                strResTemp = VB.Pstr(argResult, "{@}", inx);
                strCUT1 = "DIAGNOSIS:";
                strTemp = VB.STRCUT(strResTemp, strCUT1, "");
                strTemp = fn_BlankLine_Delete(strTemp);

                nCNT = VB.L(strTemp, Environment.NewLine);

                for (int i = 1; i <= nCNT; i++)
                {
                    strREC = VB.Pstr(strTemp, Environment.NewLine, i).Trim();
                    if (strREC != "")
                    {
                        if (VB.Right(strREC, 1) == ":")
                        {
                            if (strResult != "") { strResult += "\r\n"; }
                            strResult += strREC.Trim();
                        }
                        else
                        {
                            strResult += " " + strREC.Trim();
                        }
                    }
                }
            }

            strFollow = "";

            nResCnt = VB.L(argResult, "{@}");

            for (int inx = 1; inx <= nResCnt; inx++)
            {
                strResTemp = VB.Pstr(argResult, "{@}", inx);

                strCUT1 = "DIAGNOSIS:";
                strTemp = VB.STRCUT(strResTemp, " * ", strCUT1);
                if (strTemp != "") { strTemp = fn_BlankLine_Delete("* " + strTemp); }

                if (strFollow == "")
                {
                    strFollow = strTemp;
                }
                else
                {
                    strFollow = strFollow + "\r\n" + strTemp;
                }
            }

            if (strFollow != "")
            {
                strFollow = strFollow.Replace("\r\n", " ");
                strFollow = strFollow.Replace("  ", " ");
                strResult += "\r\n" + strFollow;
            }

            rtnVal = "▶Biopsy Diagnosis:" + "\r\n" + strResult.Trim();

            return rtnVal;
        }

        /// <summary>
        /// 방사선과 코드와 건강증진센타 코드 변환테이블 설정
        /// </summary>
        void XRayCode_2_HicCode_SET()
        {
            int nRead = 0;
            long nCnt = 0;
            string strCode = "";
            string strName = "";
            string strResult = "";

            List<BAS_BCODE> list = basBcodeService.GetItembyGubun("XRAY_건진코드_변환테이블");

            nRead = list.Count;
            FstrXRayCodeList = "{@}";
            for (int i = 0; i < nRead; i++)
            {
                strCode = list[i].CODE;
                strName = list[i].NAME;
                nCnt = VB.L(strName, ",");
                strResult = "";
                for (int j = 1; j <= nCnt; j++)
                {
                    if (VB.Pstr(strName, ",", j).Trim() != "")
                    {
                        strResult += VB.Pstr(strName, ",", j).Trim() + ",";
                    }
                }
                if (VB.Right(strResult, 1) == ",")
                {
                    strResult = VB.Left(strResult, strResult.Length - 1);
                    if (strResult != "")
                    {
                        FstrXRayCodeList += strCode + "{}" + strResult + "{@}";
                    }
                }
            }
        }

        string fn_BlankLine_Delete(string argData)
        {
            string rtnVal = "";
            string strResult = "";
            int nPos1 = 0;
            int nPos2 = 0;
            int nPos3 = 0;
            int nPos4 = 0;
            int nPos5 = 0;

            strResult = argData;

            do
            {
                nPos1 = VB.InStr(strResult, Keys.Tab.ToString());
                if (nPos1 > 0) strResult = strResult.Replace(Keys.Tab.ToString(), " ");
                nPos2 = VB.InStr(strResult, Environment.NewLine + Environment.NewLine);
                if (nPos2 > 0) strResult = strResult.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
                nPos3 = VB.InStr(strResult, Environment.NewLine + Environment.NewLine);
                if (nPos3 > 0) strResult = strResult.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
                nPos4 = VB.InStr(strResult, Environment.NewLine + " " + Environment.NewLine);
                if (nPos4 > 0) strResult = strResult.Replace(Environment.NewLine + " " + Environment.NewLine, Environment.NewLine);
                nPos5 = VB.InStr(strResult, "  ");
                if (nPos5 > 0) strResult = strResult.Replace("  ", " ");
            }
            while (nPos1 != 0 && nPos2 != 0 && nPos3 != 0 && nPos4 != 0 && nPos5 != 0);

            rtnVal = strResult;

            return rtnVal;
        }

        void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            clsCompuInfo.SetComputerInfo();

            clsQuery.READ_PC_CONFIG(clsDB.DbCon);                   //PC에 설정된 값을 READ

            cHB.READ_HIC_Doctor(clsType.User.IdNumber.To<long>());   //판정의사 여부를 읽음
            cHF.SET_자료사전_VALUE();                               //자료사전의 값을 공용변수에 로딩

            if (mPara1 == "PAN")
            {
                if (fHaExamResultReg_New == null)
                {
                    fHaExamResultReg_New = new frmHaExamResultReg_New();
                    themTabForm(fHaExamResultReg_New, this.panMain);
                }
                else
                {
                    if (FormIsExist(fHaExamResultReg_New) == true)
                    {
                        FormVisiable(fHaExamResultReg_New);
                    }
                    else
                    {
                        if (fHaExamResultReg_New == null)
                        {
                            fHaExamResultReg_New = new frmHaExamResultReg_New();
                        }
                        themTabForm(fHaExamResultReg_New, this.panMain);
                    }
                }
                menuPanjeng.Text = "종검판정 (F6)";

                //의사제외처리
                MenuMail.Visible = false;
                MenuXSend.Visible = false;
                MenuEndoSend2.Visible = false;
                MenuEtcSend.Visible = false;
                MenuEtcView.Visible = false;
            }
            else
            {
                menuPanjeng.Text = "종검가판정 (F6)";
                menuHyang.Visible = false;
                menuEndoSend.Visible = false;
                menuConsent00.Visible = false;

                if (fHaExamResultReg == null)
                {
                    fHaExamResultReg = new frmHaExamResultReg();
                    themTabForm(fHaExamResultReg, this.panMain);
                }
                else
                {
                    if (FormIsExist(fHaExamResultReg) == true)
                    {
                        FormVisiable(fHaExamResultReg);
                    }
                    else
                    {
                        if (fHaExamResultReg == null)
                        {
                            fHaExamResultReg = new frmHaExamResultReg();
                        }
                        themTabForm(fHaExamResultReg, this.panMain);
                    }
                }

                if (fHaExamResultReg_New == null)
                {
                    fHaExamResultReg_New = new frmHaExamResultReg_New();
                    themTabForm(fHaExamResultReg_New, this.panMain);
                }
                else
                {
                    if (FormIsExist(fHaExamResultReg_New) == true)
                    {
                        FormVisiable(fHaExamResultReg_New);
                    }
                    else
                    {
                        if (fHaExamResultReg_New == null)
                        {
                            fHaExamResultReg_New = new frmHaExamResultReg_New();
                        }
                        themTabForm(fHaExamResultReg_New, this.panMain);
                    }
                }

                fHaExamResultReg.Visible = false;
                fHaExamResultReg_New.Visible = false;
            }

            if (hicBcodeService.GetCountbyGubunCodeName("ETC_향정코드변경", "USE") > 0)
            {
                clsHcVariable.GstrUSE = "OK";
            }

            
            menuEKG.Visible = false;

            if (clsHcVariable.GnHicLicense > 0)
            {
                menuResultReg.Visible = false;
                menuEKG.Visible = true;
            }
            else
            {


            }


            //Call Kill("c:\ECG_*.ecg")
            string strDir = @"c:\";

            DirectoryInfo Dir = new DirectoryInfo(strDir);

            if (Dir.Exists == false)
            {
                FileInfo[] File = Dir.GetFiles("ECG_*.ecg", SearchOption.AllDirectories);

                foreach (FileInfo file in File)
                {
                    file.Delete();
                }
            }
        }

        private void FormVisiable(Form frm)
        {
            frm.Visible = false;
            frm.Visible = true;
            frm.BringToFront();
        }

        /// <summary>
        /// Main 폼에서 폼이 로드된 경우
        /// </summary>
        /// <param name="frm"></param>
        /// <returns></returns>
        private bool FormIsExist(Form frm)
        {
            foreach (Control ctl in this.panMain.Controls)
            {
                if (ctl is Form)
                {
                    if (ctl.Name == frm.Name)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void themTabForm(Form frm, Control pControl)
        {
            frm.Owner = this;
            frm.TopLevel = false;
            this.Controls.Add(frm);
            frm.Parent = pControl;
            frm.ControlBox = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Top = 0;
            frm.Left = 0;
            frm.WindowState = FormWindowState.Normal;
            frm.Height = pControl.Height;
            frm.Width = pControl.Width;
            frm.Dock = DockStyle.Fill;
            frm.Show();
        }

    }
}
