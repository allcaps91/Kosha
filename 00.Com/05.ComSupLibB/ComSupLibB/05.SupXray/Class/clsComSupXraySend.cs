using System;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using System.Data;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : clsComSupXraySend.cs
    /// Description     : 영상의학과 PACS HL7 SEND
    /// Author          : 윤조연
    /// Create Date     : 2017-07-12
    /// Update History  : 
    /// </summary>
    /// <history>  
    ///  
    /// </history>    
    public class clsComSupXraySend
    {

        ComFunc cfun = new ComFunc();  
        clsComSup sup = new clsComSup();
         
        string SQL = "";
        string SqlErr = ""; //에러문 받는 변수 
        int intRowAffected = 0;

        public static string GstrPacsSend = "";
        public static string GstrMSH = "";
        public static string GstrPID = "";
        public static string GstrPV1 = "";
        public static string GstrORC = "";
        public static string GstrOBR = "";
        public static string GstrOBX = "";

        public static string gstrTime = "";
                
        /// <summary> PACS 전송 관련  </summary>
        public class cXrayPacsSend
        {
            public string Job = "";
            public bool STS = false;
            public string EntDate = "";
            public string sysdate = "";
            public string systime = "";
            public string PacsNo = "";
            public string SendGbn = "";
            public string Pano = "";
            public string SName = "";
            public string SName2 = "";
            public string EName = "";
            public string EName2 = "";
            public string IpdOpd = "";
            public string Jumin1 = "";
            public string Jumin2 = "";
            public string Birth = "";
            public string Sex = "";
            public string Age = "";
            public string DeptCode = "";
            public string DrCode = "";
            public string DrName = "";
            public string DrSabun = "";
            public string DrEName = "";
            public string WardCode = "";
            public string RoomCode = "";
            public string OrderCode = "";
            public string OrderName = "";
            public string Remark = "";
            public string SeekDate = "";
            public string XJong = "";
            public string XSubCode = "";
            public string XrayRoom = "";
            public string MsgControlid = "";
            public string Resource = "";
            public string Modality = "";
            public string ReadNo = "";
            public long ReadDrSabun = 0;
            public string ReadDrName = "";
            public string DrRemark = "";
            public long ExID = 0;
            public string EndoChk = "";
            public string Emergency = "";
            public string QUEUEID = "";
            public string ROWID = "";
            public string Buse = ""; //'부서코드(6자리)
            public string XCode = ""; //'검사코드 2011-03-16
            public string Operator = ""; //'촬영자
            public string Disease = ""; //'질병명 2014-03-14
            public string PACS_Code = ""; //'2014-05-01  

            public string ExCode = "";
            public string ExName = "";
            
            public string Result = ""; //판독결과 값
            public string ResultDate = "";  //판독결과일자
            public string BDate = "";
            public string InDate = "";

            public string INPS = "";
            public string INPT_DT = "";
            public string UPPS = "";
            public string UP_DT = "";

        }

        //clsSpread cspd = new clsSpread();
        clsComSupXray cxray = new clsComSupXray();
        clsComSupXraySQL cxraySql = new clsComSupXraySQL();
        //cXrayPacsSend TPS = null;        

        #region //팍스 워크리스트에서 사용하는 Pacs_Ftp_Send

        public bool work_Pacs_Ftp_Send(PsmhDb pDbCon,cXrayPacsSend argCls)
        {
            if (work_HL7_Ftp_Send(pDbCon,argCls) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public bool work_HL7_Ftp_Send(PsmhDb pDbCon, cXrayPacsSend argCls)
        {

            string strSname = "";
            string strBaby = "";


            #region //영문 성명 및 생년월일 나이 Setting

            //'--------------------------------------
            //'  영문 성명 및 생년월일 Setting      
            //'--------------------------------------

            strSname = argCls.SName.Trim();
            strBaby = "";

            if (VB.Right(strSname, 2) == "애기")
            {
                strBaby = "(B)";
                strSname = VB.Left(strSname, VB.Len(strSname) - 2);
            }
            else if (VB.Right(strSname, 2) == "애1")
            {
                strBaby = "(B1)";
                strSname = VB.Left(strSname, VB.Len(strSname) - 2);
            }
            else if (VB.Right(strSname, 2) == "애2")
            {
                strBaby = "(B2)";
                strSname = VB.Left(strSname, VB.Len(strSname) - 2);
            }
            else if (VB.Right(strSname, 2) == "B1")
            {
                strBaby = "(B1)";
                strSname = VB.Left(strSname, VB.Len(strSname) - 2);
            }
            else if (VB.Right(strSname, 2) == "B2")
            {
                strBaby = "(B2)";
                strSname = VB.Left(strSname, VB.Len(strSname) - 2);
            }

            if (argCls.EName2.Trim() != "")
            {
                argCls.EName = argCls.EName2.Trim();
            }
            else
            {
                argCls.EName = HanName_TO_EngName(pDbCon, strSname) + strBaby.Trim();
            }
            argCls.SName = strSname + strBaby;

            argCls.SName2 = strSname;//이름만
            if (argCls.SName2.Trim() == "") argCls.SName2 = argCls.SName;


            //생년월일 및 주민번호 세팅
            work_Birth_Date_SET(pDbCon, argCls);

            if (argCls.Age == "" || argCls.Age == "0" || argCls.Age == "10")
            {
                //나이재계산 변경함
                argCls.Age = sup.read_Age(pDbCon, argCls.Pano, argCls.sysdate);

                #region 이전꺼 주석
                //if (argCls.Birth == "")
                //{
                //    argCls.Age = ComFunc.AgeCalcX1(argCls.Jumin1 + argCls.Jumin2, argCls.sysdate);
                //}
                //else
                //{
                //    argCls.Age = ComFunc.AgeCalcX1(argCls.Jumin1 + argCls.Jumin2, argCls.sysdate); ; //TODO 윤조연 TPS.Age = AGE_YEAR_Birth((TPS.Birth), GstrSysDate)
                //} 
                #endregion
            }
            

            #endregion

            #region //HL7 Message Setting      

            //'--------------------------------------
            //'      HL7 Message Setting      
            //'--------------------------------------
            if (argCls.DeptCode == "EM" || argCls.DeptCode == "ER") argCls.IpdOpd = "E"; //응급실

            //의사명 및 의사영문명
            argCls.DrName = "";
            argCls.DrEName = "";
            work_set_basDrNameE(pDbCon, argCls);

            argCls.OrderName = cxray.OCS_XNAME_READ(pDbCon, argCls.OrderCode,false, true);

            // 모달리티. 리소스 세팅
            work_set_Modality(argCls);

            argCls.STS  = false;

            work_PACS_ORDER_SEND(pDbCon,argCls);

            if (argCls.STS != true)
            {
                //전송실패
                return false;
            }

            #endregion


            return true;

        }

        public bool work_Birth_Date_SET(PsmhDb pDbCon, cXrayPacsSend argCls)
        {
            DataTable dt = null;

            #region //생년월일
            dt = sel_basPatient(pDbCon, argCls.Pano);

            if (dt.Rows.Count > 0)
            {
                argCls.SName = dt.Rows[0]["SName"].ToString().Trim();
                argCls.EName2 = dt.Rows[0]["EName"].ToString().Trim();
                argCls.Sex = dt.Rows[0]["Sex"].ToString().Trim();
                argCls.Jumin1 = dt.Rows[0]["Jumin1"].ToString().Trim();
                argCls.Jumin2 = dt.Rows[0]["Jumin2"].ToString().Trim();
                argCls.Birth = dt.Rows[0]["Birth"].ToString().Trim();

                if (argCls.Birth == "")
                {
                    if (VB.Left(argCls.Jumin2, 1) == "1" || VB.Left(argCls.Jumin2, 1) == "1")
                    {
                        argCls.Birth = "19" + argCls.Jumin1;
                    }
                    else if (VB.Left(argCls.Jumin2, 1) == "3" || VB.Left(argCls.Jumin2, 1) == "4")
                    {
                        argCls.Birth = "20" + argCls.Jumin1;
                    }
                    else if (VB.Left(argCls.Jumin2, 1) == "5" || VB.Left(argCls.Jumin2, 1) == "6")
                    {
                        argCls.Birth = "19" + argCls.Jumin1;
                    }
                    else if (VB.Left(argCls.Jumin2, 1) == "7" || VB.Left(argCls.Jumin2, 1) == "8")
                    {
                        if (("20" + VB.Left(argCls.Jumin1, 2)).CompareTo(VB.Left(clsPublic.GstrSysDate, 4)) > 0)
                        {
                            argCls.Birth = argCls.Birth = "19" + argCls.Jumin1;
                        }
                        else
                        {
                            argCls.Birth = argCls.Birth = "20" + argCls.Jumin1;
                        }
                    }
                    else if (VB.Left(argCls.Jumin2, 1) == "9" || VB.Left(argCls.Jumin2, 1) == "0")
                    {
                        argCls.Birth = "10" + argCls.Jumin1;
                    }
                    else
                    {
                        argCls.Birth = "";
                    }

                }

                return true;
            }
            else
            {
                argCls.Jumin1 = "";
                argCls.Jumin2 = "";

                return false;             
            }

            #endregion
        }

        public void work_set_Modality(cXrayPacsSend argCls)
        {

            #region modality set

            if (argCls.XJong == "1")
            {
                argCls.Modality = "CR";  //'일반촬영
            }
            else if (argCls.XJong == "2")
            {
                argCls.Modality = "DR";  //'특수촬영
            }
            else if (argCls.XJong == "3")
            {
                argCls.Modality = "US";  //'SONO(방사선과)
            }
            else if (argCls.XJong == "4")
            {
                argCls.Modality = "CT";  //'CT
            }
            else if (argCls.XJong == "5")
            {
                argCls.Modality = "MR";  //'MRI
            }
            else if (argCls.XJong == "6")
            {
                argCls.Modality = "NM";  //'RI
            }
            else if (argCls.XJong == "7")
            {
                argCls.Modality = "OT";  //'BMD
            }
            else if (argCls.XJong == "8")
            {
                argCls.Modality = "CT";  //'PET-CT
            }
            else if (argCls.XJong == "A")
            {
                argCls.Modality = "US";  //'UR초음파
            }
            else if (argCls.XJong == "B")
            {
                argCls.Modality = "US";  //'GS초음파
            }
            else if (argCls.XJong == "C")
            {
                argCls.Modality = "US";  //'심장초음파
            }
            else if (argCls.XJong == "D")
            {
                argCls.Modality = "ES";  //'내시경
            }
            else if (argCls.XJong == "F")
            {
                argCls.Modality = "OT";  //'안압,안저
            }
            else if (argCls.XJong == "G")
            {
                argCls.Modality = "US";  //'OG초음파
            }
            else if (argCls.XJong == "Q")
            {
                argCls.Modality = "RF";  //'ANGIO
            }
            else if (argCls.XJong == "H")
            {
                argCls.Modality = "US";  //'ENT초음파
            }
            else if (argCls.XJong == "K")
            {
                argCls.Modality = "OT";  //'적외선체온열
            }
            else if (argCls.XJong == "L")
            {
                argCls.Modality = "ES";  //'이비인후과 검사
            }
            else if (argCls.XJong == "M")
            {
                argCls.Modality = "ES";  //'GS직장경 검사
            }
            else if (argCls.XJong == "N")
            {
                argCls.Modality = "OT";  //'과 안저,시신경
            }

            #endregion

            #region resource set

            //촬영장비 SET  , CT,MR,RI,BMD는 무조건 다시 Setting함
            if (argCls.Resource.Trim() == "" || (argCls.XJong.CompareTo("2") > 0))
            {
                if (argCls.XJong == "1")
                {
                    argCls.Resource = "PRID1"; //일반촬영
                }
                else if (argCls.XJong == "2")
                {
                    argCls.Resource = "DR"; //특수촬영
                }
                else if (argCls.XJong == "3")
                {
                    argCls.Resource = "US1"; //SONO(방사선과)
                }
                else if (argCls.XJong == "4")
                {
                    argCls.Resource = "CT"; //CT
                }
                else if (argCls.XJong == "5")
                {
                    argCls.Resource = "MR"; //MRI
                }
                else if (argCls.XJong == "6")
                {
                    argCls.Resource = "NM"; //RI
                }
                else if (argCls.XJong == "7")
                {
                    argCls.Resource = "BMD"; //BMD
                }
                else if (argCls.XJong == "8")
                {
                    argCls.Resource = "PETCT"; //PET-CT
                }
                else if (argCls.XJong == "A")
                {
                    argCls.Resource = "US3"; //UR초음파
                }
                else if (argCls.XJong == "B")
                {
                    argCls.Resource = "US4"; //GS초음파
                }
                else if (argCls.XJong == "C")
                {
                    argCls.Resource = "US5"; //심장초음파
                }
                else if (argCls.XJong == "D")
                {
                    argCls.Resource = "ES1"; //내시경
                }
                else if (argCls.XJong == "F")
                {
                    argCls.Resource = "OT1"; //안저촬영
                }
                else if (argCls.XJong == "G")
                {
                    argCls.Resource = "US7"; //OG초음파
                }
                else if (argCls.XJong == "Q")
                {
                    argCls.Resource = "AN"; //ANGIO
                }
                else if (argCls.XJong == "K")
                {
                    argCls.Resource = "IRISXP"; //적외선체온
                }
                else if (argCls.XJong == "L")
                {
                    argCls.Resource = "ES1"; //이비인후과
                }
                else if (argCls.XJong == "M")
                {
                    argCls.Resource = "ES1"; //GS직장경
                }
                else if (argCls.XJong == "N")
                {
                    argCls.Resource = "OT2"; //과 안저,시신경
                }

            }
            
            #endregion

            #region 종검,건진,내시경 기타 예외 modality, resource 재설정

            //종검,건진 재설정
            if (argCls.XrayRoom == "T" || (argCls.XJong == "D" && argCls.DeptCode == "TO") || (argCls.XJong == "D" && argCls.DeptCode == "HR" && argCls.EndoChk == "Y"))
            {
                if (argCls.XJong == "1")
                {
                    argCls.Resource = "INNO4"; //일반촬영
                }
                else if (argCls.XJong == "2")
                {
                    argCls.Resource = "DR2"; //특수촬영(건진)
                }
                else if (argCls.XJong == "3")
                {
                    argCls.Resource = "US2"; //SONO(건진)
                }
                else if (argCls.XJong == "D")
                {
                    argCls.Resource = "ES2"; //종검내시경
                }
            }


            //내시경 체크
            if (argCls.XJong == "D")
            {
                if (argCls.Buse == "056104")
                {
                    argCls.Resource = "ES1"; //내시경실
                }
                else if (argCls.Buse == "044500")
                {
                    argCls.Resource = "ES2"; //종검내시경
                }
            }


            if (argCls.XJong == "F" && argCls.DeptCode == "OT")
            {
                argCls.Resource = "OT2"; //안저촬영
            }
            else if (argCls.XJong == "F" && argCls.IpdOpd == "I")
            {
                argCls.Resource = "OT2"; //안저촬영
            }

            if (argCls.XJong == "2" && argCls.XCode == "N003100A")
            {
                argCls.Modality = "ES";
                argCls.Resource = "ES1";
            }

            #endregion

        }

        public void work_set_basDrNameE(PsmhDb pDbCon, cXrayPacsSend argCls)
        {

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            argCls.DrName = "";
            argCls.DrEName = "";

            if (argCls.DrCode.Trim() == "") return;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT DrName,DrEName FROM KOSMOS_PMPA.BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE DRCODE ='" + argCls.DrCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    argCls.DrName = dt.Rows[0]["DRNAME"].ToString().Trim();
                    argCls.DrEName = dt.Rows[0]["DrEName"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return;
        }

        public void work_PACS_ORDER_SEND(PsmhDb pDbCon, cXrayPacsSend argCls)
        {
            //1.신규오더 2.취소 3.수정
            if (argCls.SendGbn == "4")
            {
                argCls.STS = true;
                return;
            }

            //long nDrSabun = 0;
            string strQUEUEID = "";
            string strExamCode = "";
            string strExamName = EDIT_OrderName(argCls.OrderName);
            string strRemark = EDIT_OrderName(argCls.Remark);
            if (strRemark != "") strExamName += " " + strRemark;



            if (strExamName.Trim() != "")
            {
                strExamCode = "";
                strExamName = work_ExamName_Convert(pDbCon, argCls.OrderCode, "", argCls.Remark);

                cXrayPacsExam cXrayPacsExam = new cXrayPacsExam();
                cXrayPacsExam.Job = "01";
                cXrayPacsExam.Modality = argCls.Modality;
                cXrayPacsExam.XCode = argCls.OrderCode;
                cXrayPacsExam.ExamName = strExamName;
                cXrayPacsExam.INPS = clsType.User.IdNumber;
                
                
                cXrayPacsExam cXrayPacsExam2 = new cXrayPacsExam();
                cXrayPacsExam2.Job = "00";
                cXrayPacsExam2.Modality = argCls.Modality;
                cXrayPacsExam2.XCode = argCls.OrderCode;
                cXrayPacsExam2.ExamName = strExamName;
                cXrayPacsExam2.INPS = clsType.User.IdNumber;
                
                strExamCode = work_ExamCode_Search(pDbCon, cXrayPacsExam, cXrayPacsExam2); //영상팍스의 코드매칭 부분
            }

            argCls.ExName = strExamName;
            argCls.ExCode = strExamCode;
            
            strQUEUEID = argCls.sysdate.Replace("-", "") + VB.Left(argCls.systime.Replace(":", ""), 4);
            strQUEUEID += VB.Right(DateTime.Now.ToString("HHmmss"), 2) + VB.Right(argCls.PacsNo, 4).Trim();

            argCls.QUEUEID = strQUEUEID;

            argCls.STS = true;

            if (argCls.STS == true)
            {
                //
                work_XRAY_PACS_ADT_INSERT(pDbCon, argCls);
            }

            if (argCls.STS == true)
            {
                //                
                work_XRAY_PACS_ORDER_INSERT(pDbCon, argCls);
            }

        }

        void work_XRAY_PACS_ADT_INSERT(PsmhDb pDbCon, cXrayPacsSend argCls)
        {
            string strEPatName = "";
            int intRowAffected = 0; //변경된 Row 받는 변수                       
            DataTable dtx = null;

            dtx = sel_mPatient(pDbCon, argCls.Pano);
            if (dtx != null && dtx.Rows.Count > 0)
            {
                strEPatName = dtx.Rows[0]["EPatName"].ToString().Trim().Replace("^", " ");
            }

            dtx = null;

            dtx = sel_xrayPacsADT(pDbCon, argCls.Pano);
            if (dtx != null && dtx.Rows.Count > 0)
            {
                if (strEPatName != "" && argCls.EName2 != "")
                {
                    if (argCls.EName2 == strEPatName)
                    {
                        argCls.STS = true;
                        return;
                    }
                }

            }

            #region //변수설정
            clsComSupXraySend.cXrayPacsADT cXrayPacsADT = new cXrayPacsADT();
            cXrayPacsADT.Patid = argCls.Pano;
            cXrayPacsADT.PatName = argCls.SName;
            cXrayPacsADT.QUEUEID = argCls.QUEUEID;
            cXrayPacsADT.EventType = "A04";
            if (argCls.IpdOpd =="I")
            {
                cXrayPacsADT.EventType = "A01";
            }
            cXrayPacsADT.BirthDay = argCls.Birth;
            cXrayPacsADT.Dept = argCls.DeptCode;
            cXrayPacsADT.AttendDoct1 = argCls.DrCode;
            cXrayPacsADT.PatType = argCls.IpdOpd;
            cXrayPacsADT.PersonalID = argCls.Jumin1 + argCls.Jumin2;
            cXrayPacsADT.Sex = argCls.Sex;
            cXrayPacsADT.EPatName = argCls.EName2.Trim();
            #endregion

            //생성
            SqlErr = ins_XRAY_PACS_ADT(pDbCon, cXrayPacsADT, ref intRowAffected);
                        
            if (SqlErr != "")
            {
                argCls.STS = false;               
                return;
            }
            else
            {
                argCls.STS = true;
            }           
            
        }
        
        void work_XRAY_PACS_ORDER_INSERT(PsmhDb pDbCon, cXrayPacsSend argCls)
        {
            int intRowAffected = 0; //변경된 Row 받는 변수                       
                      
            //생성
            //1.접수  2.접수취소 3.접수변경 4.촬영완료 5.판독완료 6.판독수정 7.판독삭제
            if (argCls.SendGbn.CompareTo("3") <= 0)
            {
                #region //변수설정
                clsComSupXraySend.cXrayPacsOrder cXrayPacsOrder = new cXrayPacsOrder();
                cXrayPacsOrder.Patid = argCls.Pano;
                cXrayPacsOrder.IPDOPD = argCls.IpdOpd;
                cXrayPacsOrder.QUEUEID = argCls.QUEUEID;
                cXrayPacsOrder.PacsNo = argCls.PacsNo;
                cXrayPacsOrder.EventType = "";
                if (argCls.SendGbn =="1")
                {
                    cXrayPacsOrder.EventType = "NW";
                }
                else if (argCls.SendGbn == "2")
                {
                    cXrayPacsOrder.EventType = "CA";
                }
                else if (argCls.SendGbn == "3")
                {
                    cXrayPacsOrder.EventType = "NW";
                }
                cXrayPacsOrder.ExamDate = VB.Left(argCls.SeekDate, 8);
                if (VB.Len(argCls.SeekDate) > 8)
                {
                    cXrayPacsOrder.ExamTime = VB.Right(argCls.SeekDate, 4);
                }
                else
                {
                    cXrayPacsOrder.ExamTime = "";
                }

                cXrayPacsOrder.ExamRoom = argCls.Resource;
                cXrayPacsOrder.ExamCode = argCls.ExCode;
                cXrayPacsOrder.ExamName = argCls.ExName;
                cXrayPacsOrder.DrCode = argCls.DrSabun;
                cXrayPacsOrder.ORDERDOC = argCls.DrSabun;
                cXrayPacsOrder.DeptCode = argCls.DeptCode;
                cXrayPacsOrder.SName = argCls.SName2;
                cXrayPacsOrder.BirthDay = argCls.Birth;
                cXrayPacsOrder.Sex = argCls.Sex;
                cXrayPacsOrder.PatType = argCls.IpdOpd;
                cXrayPacsOrder.PacsNo = argCls.PacsNo;
                cXrayPacsOrder.WardCode = argCls.WardCode;
                cXrayPacsOrder.RoomCode = argCls.RoomCode;
                cXrayPacsOrder.RequestMemo = argCls.DrRemark;
                cXrayPacsOrder.Emergency = argCls.Emergency;
                cXrayPacsOrder.Operator = argCls.Operator;
                cXrayPacsOrder.DISEASE = argCls.Disease;
                cXrayPacsOrder.INPS = argCls.INPS;

                #endregion

                SqlErr = ins_XRAY_PACS_ORDER(pDbCon, cXrayPacsOrder, ref intRowAffected);

                if (SqlErr != "")
                {
                    argCls.STS = false;
                    return;
                }
                
            }
            else
            {
                if (argCls.XJong == "D")
                {
                    work_ENDO_Reading_SET(pDbCon,argCls);
                }
                else
                {
                    work_XRAY_Reading_SET(pDbCon,argCls);
                }

                argCls.Result = argCls.Result.Replace("$", "\r\n");
                
                #region //변수설정
                clsComSupXraySend.cXrayPacsReport cXrayPacsReport = new cXrayPacsReport();
                cXrayPacsReport.QUEUEID = argCls.QUEUEID;
                cXrayPacsReport.FLAG = "N";
                if (argCls.SendGbn =="5" || argCls.SendGbn == "6")
                {
                    cXrayPacsReport.READSTAT = "C";
                }
                else if (argCls.SendGbn == "7" )
                {
                    cXrayPacsReport.READSTAT = "X";
                }
                else
                {
                    cXrayPacsReport.READSTAT = "C";
                }
                cXrayPacsReport.HISORDERID = argCls.PacsNo;
                cXrayPacsReport.PATID = argCls.Pano;
                cXrayPacsReport.CONCLUSION = argCls.Result;
                cXrayPacsReport.REPORTDATE = VB.Left(argCls.ResultDate, 8);
                if (VB.Len(argCls.ResultDate) > 8)
                {
                    cXrayPacsReport.REPORTTIME = VB.Right(argCls.ResultDate, 4);
                }
                else
                {
                    cXrayPacsReport.REPORTTIME = "";
                }
                cXrayPacsReport.CONFDR1 = argCls.DrSabun.ToString();
                #endregion

                SqlErr = ins_XRAY_PACS_REPORT(pDbCon, cXrayPacsReport, ref intRowAffected);
                if (SqlErr != "")
                {
                    argCls.STS = false;
                    return;
                }
                                
            }       

        }

        void work_XRAY_Reading_SET(PsmhDb pDbCon, cXrayPacsSend argCls)
        {
            DataTable dt = null;

            string strTemp = "";

            if (Convert.ToInt32(argCls.ReadNo) == 0)
            {
                dt = sel_XrayDetail(pDbCon, argCls.Pano, argCls.PacsNo);
                if (dt != null && dt.Rows.Count > 0)
                {
                    argCls.ReadNo = dt.Rows[0]["ExInfo"].ToString().Trim();
                }
            }


            dt = null;
            dt = sel_resultNew(pDbCon, argCls.ReadNo);
            if (dt != null && dt.Rows.Count > 0)
            {
                argCls.ResultDate = dt.Rows[0]["ReadDate"].ToString().Trim();
                strTemp = dt.Rows[0]["Result"].ToString().Trim();
                strTemp += dt.Rows[0]["Result1"].ToString().Trim();

                argCls.ReadDrSabun = Convert.ToInt32(dt.Rows[0]["XDrCode1"].ToString().Trim());

            }

            argCls.Result = strTemp.Replace("\r\n", "$");

            //판독삭제
            if (argCls.SendGbn == "7") argCls.Result = "";

            argCls.ReadDrName = clsVbfunc.GetInSaName(pDbCon, argCls.ReadDrSabun.ToString()); //TODO 윤조연 BAS_USER 사용요망

        }

        void work_ENDO_Reading_SET(PsmhDb pDbCon, cXrayPacsSend argCls)
        {
            DataTable dt = null;

            string strGbJob = "";
            string strGbNEW = "";

            string[] strResult = new string[5];

            string strTemp = "";

            dt = sel_endoJupmst(pDbCon, argCls.Pano, argCls.ReadNo);
            if (dt != null && dt.Rows.Count > 0)
            {
                strGbJob = dt.Rows[0]["GbJob"].ToString().Trim();
                strGbNEW = dt.Rows[0]["GbNew"].ToString().Trim();

                argCls.ReadNo = dt.Rows[0]["SeqNo"].ToString().Trim();
                argCls.ReadDrSabun = Convert.ToInt32(dt.Rows[0]["ResultDrCode"].ToString().Trim());
                argCls.ResultDate = dt.Rows[0]["ResultDate"].ToString().Trim();
            }

            dt = null;
            dt = sel_endoResult(pDbCon, argCls.ReadNo);
            if (dt != null && dt.Rows.Count > 0)
            {
                strResult[0] = dt.Rows[0]["Remark1"].ToString().Trim();
                strResult[1] = dt.Rows[0]["Remark2"].ToString().Trim();
                strResult[2] = dt.Rows[0]["Remark3"].ToString().Trim();
                strResult[3] = dt.Rows[0]["Remark4"].ToString().Trim();
                strResult[4] = dt.Rows[0]["Remark5"].ToString().Trim();

                if (strGbJob == "1")
                {
                    //기관지
                    strTemp = "▶Vocal Cord:" + "\r\n" + strResult[0] + "\r\n" + "\r\n";
                    strTemp += "▶Carina:" + "\r\n" + strResult[1] + "\r\n" + "\r\n";
                    strTemp += "▶Bronchi:" + "\r\n" + strResult[2] + "\r\n" + "\r\n";
                    strTemp += "▶EndoScopic Procedure:" + "\r\n" + strResult[3];

                }
                else if (strGbJob == "2")
                {
                    //위
                    strTemp = "▶Esophagus:" + "\r\n" + strResult[0] + "\r\n" + "\r\n";
                    strTemp += "▶Stomach:" + "\r\n" + strResult[1] + "\r\n" + "\r\n";
                    strTemp += "▶Duodenum:" + "\r\n" + strResult[2] + "\r\n" + "\r\n";
                    strTemp += "▶Endoscopic Diagnosis:" + "\r\n" + strResult[3] + "\r\n" + "\r\n";
                    strTemp += "▶Endoscopic Procedure:" + "\r\n" + strResult[4];

                }
                else if (strGbJob == "3")
                {
                    //장
                    if (strGbNEW == "Y")
                    {
                        strTemp = "▶Endoscopic Findings " + "\r\n" + "small Intestinal:" + strResult[0] + "\r\n" + "\r\n";
                        strTemp += "large Intestinal:" + strResult[3] + "\r\n" + "\r\n";
                        strTemp += "rectum:" + strResult[4] + "\r\n" + "\r\n";

                        strTemp += "▶Endoscopic Diagnosis:" + "\r\n" + strResult[1] + "\r\n" + "\r\n";
                        strTemp += "▶Endoscopic Procedure:" + "\r\n" + strResult[2];
                    }
                    else
                    {
                        strTemp = "▶Endoscopic Findings:" + "\r\n" + strResult[0] + "\r\n" + "\r\n";
                        strTemp += "▶Endoscopic Diagnosis:" + "\r\n" + strResult[1] + "\r\n" + "\r\n";
                        strTemp += "▶Endoscopic Procedure:" + "\r\n" + strResult[2];

                    }

                }
                else if (strGbJob == "4")
                {
                    //ERCP
                    strTemp = "▶ERCP Finding:" + "\r\n" + strResult[0] + "\r\n" + "\r\n";
                    strTemp += "▶Diagnosis:" + "\r\n" + strResult[1] + "\r\n" + "\r\n";
                    strTemp += "▶Plan & Tx:" + "\r\n" + strResult[2] + "\r\n" + "\r\n";
                    strTemp += "▶EndoScopic Procedure:" + "\r\n" + strResult[3];

                }

            }

            argCls.Result = strTemp.Replace("\r\n", "$");

            //판독삭제
            if (argCls.SendGbn == "7") argCls.Result = "";

            argCls.ReadDrName = clsVbfunc.GetInSaName(pDbCon, argCls.ReadDrSabun.ToString()); //TODO 윤조연 BAS_USER 사용요망

        }

        public string work_ExamName_Convert(PsmhDb pDbCon,string argCode,string argXCode,string argRemark)
        {
            DataTable dt = null;
            string strName = string.Empty;
            string s = string.Empty;
            string strRemark = string.Empty;
                       

            if (argCode !="")
            {
                dt = sel_OCS_ORDERCODE(pDbCon, argCode);
                if (ComFunc.isDataTableNull(dt) == false)
                {
                    if (dt.Rows[0]["DispHeader"].ToString().Trim()!="")
                    {
                        strName = dt.Rows[0]["DispHeader"].ToString().Trim();
                    }
                    else
                    {
                        strName = dt.Rows[0]["OrderName"].ToString().Trim();
                    }                    
                }
            }
            else if (argXCode !="")
            {
                dt = sel_XRAY_CODE(pDbCon, argXCode);
                if (ComFunc.isDataTableNull(dt) == false)
                {                    
                    strName = dt.Rows[0]["XName"].ToString().Trim();                    
                }
            }

            if (strName =="")
            {
                return "";
            }
            else
            {
                
                strName = strName.Replace("[CR]", "");
                strName = strName.Replace("(CR)", "");
                strName = strName.Replace("[CR)", "");
                strName = strName.Replace("()", "");
                                
                strRemark = argRemark.Trim();

                if (strRemark !="")
                {
                    strRemark = strRemark.Replace("[CR]", "");
                    strRemark = strRemark.Replace("(CR)", "");
                    strRemark = strRemark.Replace("[CR)", "");
                    strRemark = strRemark.Replace("()", "");
                    strRemark = strRemark.Replace("Right", "(R)");
                }

                if (strName !="")
                {
                    s = strName.Trim();
                    if (strRemark !="")
                    {
                        s += " " + strRemark;
                    }
                }

                return s;
            }
                      

        }

        public string work_ExamCode_Search(PsmhDb pDbCon, cXrayPacsExam argCls, cXrayPacsExam argCls2)
        {
            DataTable dt = null;
            long nSeqno = 0;
            string strCode = "";
            int intRowAffected = 0;

            //자료유무체크 구분01
            dt = sel_XRAY_PACS_EXAM(pDbCon, argCls);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                return dt.Rows[0]["ExamCode"].ToString().Trim();
            }
            else
            {
                //신규 구분00
                dt = sel_XRAY_PACS_EXAM(pDbCon, argCls2);
                if (ComFunc.isDataTableNull(dt) == false)
                {
                    if (dt.Rows[0]["ExamCode"].ToString().Trim() != "")
                    {
                        nSeqno = Convert.ToInt32(VB.Right(dt.Rows[0]["ExamCode"].ToString().Trim(), 3));
                    }

                    nSeqno++;

                    if (argCls2.ExamCode != "")
                    {
                        strCode = argCls2.ExamCode + "-" + ComFunc.SetAutoZero(nSeqno.ToString(), 3);
                    }
                    else
                    {
                        strCode = argCls2.XCode + "-" + ComFunc.SetAutoZero(nSeqno.ToString(), 3);
                    }
                    //신규일경우 변수 재설정
                    argCls2.ExamCode = strCode;

                }

                SqlErr = ins_XRAY_PACS_EXAM(pDbCon, argCls2, ref intRowAffected);
                if (SqlErr == "")
                {
                    return strCode;
                }
                else
                {
                    return "";
                }

            }


        }
        #endregion

        #region //팍스샌드(HL7) xusend에서 사용               

        public class cComSupXraySendInfo
        {
            public string Job = "";
            public string Pano = "";
            public string SName = "";
            public string SendGbn = "";
            public string SendTime = "";
            public string PacsNo = "";
            public string OrderCode = "";
            public string OrderName = "";

            public string sysdate = "";
            public string sysdateTime = "";
            public string JepDate = "";
            public string strExCode = "";
            public long nWRTNO = 0;
            public long nHic_Pano = 0;
            public string strGjJong = "";
            public string strBunjin = "";
            public string Ptno = "";            
            public string strSex = "";
            public string strLtdCode = "";
            public string strLtdName = "";
            public int nAge = 0;
            public long nSabun = 0;
            
        }

        public enum enmEtcSupXraySend
        {
            ETCSONO, XRAY, ENDO, LaparoSCOPY, ENDO_RESULT
           , PACSEND_SET, GBSTS_AUTO, HIC_XRAY_RESULT, IPD_STS_ADT, EDPS_TEST
        }

        public class cXray_ResultNew
        {
            public long WRTNO =0;
            public string Pano = "";
            public string strSeekDate = "";
            public string ReadDate = "";
            public string ReadTime = "";
            public string SeekDate = "";
            public string XJong = "";
            public string SName = "";
            public string Sex = "";
            public string Age = "";
            public string IpdOpd = "";
            public string DeptCode = "";
            public string DrCode = "";
            public string WardCode = "";
            public string RoomCode = "";
            public int XDrCode1 =0;
            public int XDrCode2 = 0;
            public int XDrCode3 = 0;
            public string IllCode1 = "";
            public string IllCode2 = "";
            public string IllCode3 = "";
            public string XCode = "";
            public string XName = "";
            public string Result = "";
            public string Result1 = "";
            public string EntDate = "";
            public string Approve = "";
            public string STime = "";
            public string ETime = "";

            public string ADDENDUM = "";

        }

        public string[] GstrXuSend_CHK = null;

        public string READ_ASIS_SEND_CHK(PsmhDb pDbCon)
        {
            string s = string.Empty;

            GstrXuSend_CHK = new string[Enum.GetValues(typeof(enmEtcSupXraySend)).Length];

            for (int i = 0; i < GstrXuSend_CHK.Length; i++)
            {
                GstrXuSend_CHK[i] = "";
            }

            GstrXuSend_CHK[(int)enmEtcSupXraySend.ETCSONO] = cfun.Read_Bcode_Name(pDbCon, "XRAY_SEND_구분", "00.ETCSONO");
            GstrXuSend_CHK[(int)enmEtcSupXraySend.XRAY] = cfun.Read_Bcode_Name(pDbCon, "XRAY_SEND_구분", "01.XRAY");
            GstrXuSend_CHK[(int)enmEtcSupXraySend.ENDO] = cfun.Read_Bcode_Name(pDbCon, "XRAY_SEND_구분", "02.ENDO");
            GstrXuSend_CHK[(int)enmEtcSupXraySend.LaparoSCOPY] = cfun.Read_Bcode_Name(pDbCon, "XRAY_SEND_구분", "03.복강경");
            GstrXuSend_CHK[(int)enmEtcSupXraySend.ENDO_RESULT] = cfun.Read_Bcode_Name(pDbCon, "XRAY_SEND_구분", "04.ENDO_RESULT");
            GstrXuSend_CHK[(int)enmEtcSupXraySend.PACSEND_SET] = cfun.Read_Bcode_Name(pDbCon, "XRAY_SEND_구분", "05.PACSEND_SET");
            GstrXuSend_CHK[(int)enmEtcSupXraySend.GBSTS_AUTO] = cfun.Read_Bcode_Name(pDbCon, "XRAY_SEND_구분", "06.GBSTS_AUTO");
            GstrXuSend_CHK[(int)enmEtcSupXraySend.HIC_XRAY_RESULT] = cfun.Read_Bcode_Name(pDbCon, "XRAY_SEND_구분", "07.HIC_XRAY_RESULT");
            GstrXuSend_CHK[(int)enmEtcSupXraySend.IPD_STS_ADT] = cfun.Read_Bcode_Name(pDbCon, "XRAY_SEND_구분", "08.IPD_STS_ADT");

            GstrXuSend_CHK[(int)enmEtcSupXraySend.EDPS_TEST] = cfun.Read_Bcode_Name(pDbCon, "XRAY_SEND_구분", "19.전산연습제외");

            if (GstrXuSend_CHK[(int)enmEtcSupXraySend.ETCSONO] == "Y") s += "00.ETCSONO,";
            if (GstrXuSend_CHK[(int)enmEtcSupXraySend.XRAY] == "Y") s += "01.XRAY,";
            if (GstrXuSend_CHK[(int)enmEtcSupXraySend.ENDO] == "Y") s += "02.ENDO,";
            if (GstrXuSend_CHK[(int)enmEtcSupXraySend.LaparoSCOPY] == "Y") s += "03.복강경,";
            if (GstrXuSend_CHK[(int)enmEtcSupXraySend.ENDO_RESULT] == "Y") s += "04.ENDO_RESULT,";
            if (GstrXuSend_CHK[(int)enmEtcSupXraySend.PACSEND_SET] == "Y") s += "05.PACSEND_SET,";
            if (GstrXuSend_CHK[(int)enmEtcSupXraySend.GBSTS_AUTO] == "Y") s += "06.GBSTS_AUTO,";
            if (GstrXuSend_CHK[(int)enmEtcSupXraySend.HIC_XRAY_RESULT] == "Y") s += "07.HIC_XRAY_RESULT,";
            if (GstrXuSend_CHK[(int)enmEtcSupXraySend.IPD_STS_ADT] == "Y") s += "08.IPD_STS_ADT,";


            return s;

        }

        public void sheet_add(FarPoint.Win.Spread.FpSpread Spd, cComSupXraySendInfo argCls)
        {
            Spd.ActiveSheet.AddRows(0, 1);  // 시작 ROW,추가row건수

            Spd.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmSpdXSend.SendTime].Text = argCls.SendTime;
            Spd.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmSpdXSend.PacsNo].Text = argCls.PacsNo;
            Spd.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmSpdXSend.Pano].Text = argCls.Pano;
            Spd.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmSpdXSend.SName].Text = argCls.SName;            
            Spd.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmSpdXSend.SendGbn].Text = argCls.SendGbn;
            Spd.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmSpdXSend.OrderCode].Text = argCls.OrderCode;
            Spd.ActiveSheet.Cells[0, (int)clsComSupXraySpd.enmSpdXSend.OrderName].Text = argCls.OrderName;

            if (Spd.ActiveSheet.RowCount > 100)
            {
                Spd.ActiveSheet.RowCount = 100; //최대 100건만 표시
            }
        }

        DataTable sel_HL7_Send_EtcSono(PsmhDb pDbCon, string[] argGubun)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                                      \r\n";
            SQL += "  TO_CHAR(EnterDate,'YYYY-MM-DD HH24:MI') EnterDate                         \r\n";
            SQL += "  ,Pano,SName,OrderCode,OrderName                                           \r\n";
            SQL += "  ,XJong,Qty, XCODE,Exid,ROWID                                              \r\n";
            SQL += "  ,KOSMOS_OCS.FC_XRAY_CODE_NM(XCode) FC_XName                               \r\n"; //코드명칭       
            SQL += "  ,KOSMOS_OCS.FC_OCS_ORDERCODE_NAME2(OrderCode) FC_OrderName                \r\n"; //오더명칭       
            SQL += " FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                     \r\n";
            SQL += "  WHERE 1=1                                                                 \r\n";
            SQL += "   AND SeekDate >=TRUNC(SYSDATE)                                            \r\n";
            SQL += "   AND (XJong >= 'A'  AND XJONG NOT IN  ('Q'))                              \r\n";
            SQL += "   AND XCODE NOT IN  ('US23','US22')                                        \r\n";
            SQL += "   AND PacsNo IS NULL                                                       \r\n";
            if (argGubun[(int)clsComSupXraySend.enmEtcSupXraySend.EDPS_TEST] != "Y")
            {
                SQL += "  AND Pano NOT IN ('81000000','81000001','81000002','81000003','81000004','81000005','81000006','81000007','81000008','81000009')   \r\n";
            }
            else
            {
                SQL += "  AND Pano IN ('81000000','81000001','81000002','81000003','81000004','81000005','81000006','81000007','81000008','81000009')       \r\n";
            }
            SQL += "  ORDER BY EnterDate,PacsNo                             \r\n";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_HL7_Xray(PsmhDb pDbCon, string[] argGubun)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                                      \r\n";
            SQL += "  TO_CHAR(a.ENTDATE,'YYYYMMDDHH24MI') EntDate,a.PacsNo,a.SendGbn            \r\n";
            SQL += "  ,a.Pano,a.SName,a.EName,b.EName EName2,a.Sex,a.Age                        \r\n";
            SQL += "  ,a.DeptCode,a.DrCode,a.IPDOPD,a.WardCode,a.RoomCode                       \r\n";
            SQL += "  ,a.XJONG,a.XSUBCODE,a.XCODE,a.OrderCode,a.XRayName,a.operator             \r\n";
            SQL += "  ,TO_CHAR(a.SeekDate,'YYYYMMDDHH24MI') SeekDate                            \r\n";      
            SQL += "  ,a.Remark,a.XrayRoom,a.ReadNo,a.DrRemark,a.ROWID                          \r\n";      
            SQL += "  ,KOSMOS_OCS.FC_XRAY_DETAIL_GBREAD(a.Pano,a.PacsNo) FC_GBREAD              \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "XRAY_PACSSEND a                                 \r\n";
            SQL += "   ,  " + ComNum.DB_PMPA + "BAS_PATIENT b                                   \r\n";
            SQL += "  WHERE 1=1                                                                 \r\n";
            SQL += "   AND a.Pano=b.Pano(+)                                                     \r\n";
            SQL += "   AND a.SendTime IS NULL                                                   \r\n";
            SQL += "   AND a.EntDate>=TRUNC(SYSDATE-2)                                          \r\n";
            SQL += "   AND ROWNUM <= 40                                                         \r\n";            
            if (argGubun[(int)clsComSupXraySend.enmEtcSupXraySend.EDPS_TEST] != "Y")
            {
                SQL += "  AND a.Pano NOT IN ('81000000','81000001','81000002','81000003','81000004','81000005','81000006','81000007','81000008','81000009')   \r\n";
            }
            else
            {
                SQL += "  AND a.Pano IN ('81000000','81000001','81000002','81000003','81000004','81000005','81000006','81000007','81000008','81000009')       \r\n";
            }
            SQL += "  ORDER BY a.EntDate,a.PacsNo                                               \r\n";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_HL7_Endo(PsmhDb pDbCon, string[] argGubun)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                                      \r\n";
            SQL += "  TO_CHAR(a.ENTDATE,'YYYYMMDDHH24MI') EntDate,a.PacsNo,'' SendGbn,a.GbSunap \r\n";
            SQL += "  ,a.Ptno Pano,b.SName,b.EName EName2,a.Sex,b.Jumin1,b.Jumin2,b.Jumin3      \r\n";
            SQL += "  ,a.DeptCode,a.DrCode,a.GBIO IPDOPD,a.WardCode,a.RoomCode                  \r\n";
            SQL += "  ,a.OrderCode,a.EndoChk, a.BUSE                                            \r\n";
            SQL += "  ,TO_CHAR(a.RDate,'YYYYMMDD') RDate                                        \r\n"; //
            SQL += "  ,TO_CHAR(a.BDate,'YYYY-MM-DD') BDate                                      \r\n";
            SQL += "  ,TO_CHAR(a.SeekDate,'YYYYMMDDHH24MI') SeekDate                            \r\n";
            SQL += "  ,KOSMOS_OCS.KOSMOS_OCS.FC_GET_AGE2(a.Ptno,SYSDATE) FC_AGE                 \r\n";
            SQL += "  ,a.Remark,a.PacsUID,a.PacsSend,a.ROWID                                    \r\n";            
            SQL += " FROM " + ComNum.DB_MED + "ENDO_JUPMST a                                    \r\n";
            SQL += "   ,  " + ComNum.DB_PMPA + "BAS_PATIENT b                                   \r\n";
            SQL += "  WHERE 1=1                                                                 \r\n";
            SQL += "   AND a.Ptno=b.Pano(+)                                                     \r\n";
            SQL += "   AND a.RDate >= TRUNC(SYSDATE-3)                                          \r\n";
            SQL += "   AND a.PacsSend = '*'                                                     \r\n";
            SQL += "   AND a.JupsuName <> '$$'                                                  \r\n";
            SQL += "   AND ROWNUM <= 30                                                         \r\n";
            if (argGubun[(int)clsComSupXraySend.enmEtcSupXraySend.EDPS_TEST] != "Y")
            {
                SQL += "  AND a.Ptno NOT IN ('81000000','81000001','81000002','81000003','81000004','81000005','81000006','81000007','81000008','81000009')   \r\n";
            }
            else
            {
                SQL += "  AND a.Ptno IN ('81000000','81000001','81000002','81000003','81000004','81000005','81000006','81000007','81000008','81000009')       \r\n";
            }
            SQL += "  ORDER BY a.RDate                                                          \r\n";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_HL7_Endo_Result(PsmhDb pDbCon, string[] argGubun)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                                      \r\n";
            SQL += "  TO_CHAR(a.ENTDATE,'YYYYMMDDHH24MI') EntDate,a.PacsNo,'' SendGbn,a.GbSunap \r\n";
            SQL += "  ,a.Ptno Pano,b.SName,b.EName EName2,a.Sex,b.Jumin1,b.Jumin2,b.Jumin3      \r\n";
            SQL += "  ,a.DeptCode,a.DrCode,a.GBIO IPDOPD,a.WardCode,a.RoomCode                  \r\n";
            SQL += "  ,a.OrderCode,a.EndoChk, a.BUSE                                            \r\n";
            SQL += "  ,TO_CHAR(a.RDate,'YYYYMMDD') RDate                                        \r\n"; //
            SQL += "  ,TO_CHAR(a.BDate,'YYYYMMDD') BDate                                        \r\n";
            SQL += "  ,TO_CHAR(a.SeekDate,'YYYYMMDDHH24MI') SeekDate                            \r\n";
            SQL += "  ,TO_CHAR(a.ResultDATE,'YYYYMMDDHH24MI') ResultDATE                        \r\n";
            SQL += "  ,KOSMOS_OCS.KOSMOS_OCS.FC_GET_AGE2(a.Ptno,SYSDATE) FC_AGE                 \r\n";
            SQL += "  ,a.Remark,a.PacsUID,a.PacsSend,a.ROWID                                    \r\n";
            SQL += " FROM " + ComNum.DB_MED + "ENDO_JUPMST a                                    \r\n";
            SQL += "   ,  " + ComNum.DB_PMPA + "BAS_PATIENT b                                   \r\n";
            SQL += "  WHERE 1=1                                                                 \r\n";
            SQL += "   AND a.Ptno=b.Pano(+)                                                     \r\n";
            SQL += "   AND a.ResultDate >= TRUNC(SYSDATE-5)                                     \r\n";
            SQL += "   AND a.ResultSend = '*'                                                   \r\n";
            SQL += "   AND a.PacsUID IS NOT NULL                                                \r\n";
            SQL += "   AND ROWNUM <= 14                                                         \r\n";
            if (argGubun[(int)clsComSupXraySend.enmEtcSupXraySend.EDPS_TEST] != "Y")
            {
                SQL += "  AND a.Ptno NOT IN ('81000000','81000001','81000002','81000003','81000004','81000005','81000006','81000007','81000008','81000009')   \r\n";
            }
            else
            {
                SQL += "  AND a.Ptno IN ('81000000','81000001','81000002','81000003','81000004','81000005','81000006','81000007','81000008','81000009')       \r\n";
            }
            SQL += "  ORDER BY a.ResultDate                                                     \r\n";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_HL7_Laparoscopy(PsmhDb pDbCon, string[] argGubun)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                                      \r\n";
            SQL += "  TO_CHAR(EnterDate,'YYYY-MM-DD HH24:MI') EnterDate                         \r\n";
            SQL += "  ,Pano,SName,OrderCode,OrderName                                           \r\n";
            SQL += "  ,XJong,Qty, XCODE,Exid,ROWID                                              \r\n";
            SQL += "  ,KOSMOS_OCS.FC_XRAY_CODE_NM(XCode) FC_XName                               \r\n"; //코드명칭       
            SQL += "  ,KOSMOS_OCS.FC_OCS_ORDERCODE_NAME2(OrderCode) FC_OrderName                \r\n"; //오더명칭       
            SQL += " FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                     \r\n";
            SQL += "  WHERE 1=1                                                                 \r\n";
            SQL += "   AND SeekDate >=TRUNC(SYSDATE)                                            \r\n";
            SQL += "   AND XJong = '2'                                                          \r\n";
            SQL += "   AND XCODE IN  ('N003100A','N003100B','N003100T')                         \r\n";
            SQL += "   AND PacsNo IS NULL                                                       \r\n";
            SQL += "   AND GbReserved ='1'                                                      \r\n"; //ocs 자동전송된것
            if (argGubun[(int)clsComSupXraySend.enmEtcSupXraySend.EDPS_TEST] != "Y")
            {
                SQL += "  AND Pano NOT IN ('81000000','81000001','81000002','81000003','81000004','81000005','81000006','81000007','81000008','81000009')   \r\n";
            }
            else
            {
                SQL += "  AND Pano IN ('81000000','81000001','81000002','81000003','81000004','81000005','81000006','81000007','81000008','81000009')       \r\n";
            }
            SQL += "  ORDER BY EnterDate,PacsNo                             \r\n";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_HL7_GbSTS_Auto_Set_Part1(PsmhDb pDbCon, string[] argGubun)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                                      \r\n";
            SQL += "  TO_CHAR(EnterDate,'YYYY-MM-DD HH24:MI') EnterDate                         \r\n";
            SQL += "  ,Pano,SName,OrderCode,OrderName                                           \r\n";
            SQL += "  ,XJong,Qty, XCODE,Exid,ROWID                                              \r\n";
            SQL += "  ,KOSMOS_OCS.FC_XRAY_CODE_NM(XCode) FC_XName                               \r\n"; //코드명칭       
            SQL += "  ,KOSMOS_OCS.FC_OCS_ORDERCODE_NAME2(OrderCode) FC_OrderName                \r\n"; //오더명칭       
            SQL += " FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                     \r\n";
            SQL += "  WHERE 1=1                                                                 \r\n";
            SQL += "   AND SeekDate >=TRUNC(SYSDATE-1)                                          \r\n";
            SQL += "   AND TRIM(XCode) IN (                                                     \r\n";
            SQL += "                       SELECT TRIM(Code)                                    \r\n";
            SQL += "                        FROM " + ComNum.DB_PMPA + "BAS_BCODE                \r\n";
            SQL += "                         WHERE GUBUN ='XRAY_자동상태완료수가'               \r\n";
            SQL += "                          AND (DELDATE IS NULL OR DELDATE ='')              \r\n";
            SQL += "                       )                                                    \r\n";
            SQL += "   AND (CDate IS NULL OR CDate ='')                                         \r\n";
            SQL += "   AND (DelDate IS NULL OR DelDate ='')                                     \r\n";
            SQL += "   AND (GbSTS IS NULL OR GbSTS <>'7')                                       \r\n";            
            if (argGubun[(int)clsComSupXraySend.enmEtcSupXraySend.EDPS_TEST] != "Y")
            {
                SQL += "  AND Pano NOT IN ('81000000','81000001','81000002','81000003','81000004','81000005','81000006','81000007','81000008','81000009')   \r\n";
            }
            else
            {
                SQL += "  AND Pano IN ('81000000','81000001','81000002','81000003','81000004','81000005','81000006','81000007','81000008','81000009')       \r\n";
            }
            SQL += "  ORDER BY EnterDate,PacsNo                             \r\n";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_HL7_GbSTS_Auto_Set_Part2(PsmhDb pDbCon, string[] argGubun)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                                      \r\n";
            SQL += "  TO_CHAR(EnterDate,'YYYY-MM-DD HH24:MI') EnterDate                         \r\n";
            SQL += "  ,Pano,SName,OrderCode,OrderName                                           \r\n";
            SQL += "  ,XJong,Qty, XCODE,Exid,ROWID                                              \r\n";
            SQL += "  ,KOSMOS_OCS.FC_XRAY_CODE_NM(XCode) FC_XName                               \r\n"; //코드명칭       
            SQL += "  ,KOSMOS_OCS.FC_OCS_ORDERCODE_NAME2(OrderCode) FC_OrderName                \r\n"; //오더명칭       
            SQL += " FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                     \r\n";
            SQL += "  WHERE 1=1                                                                 \r\n";
            SQL += "   AND SeekDate >=TRUNC(SYSDATE-1)                                          \r\n";
            SQL += "   AND DeptCode IN ('HR','TO','R6')                                         \r\n";            
            SQL += "   AND (CDate IS NULL OR CDate ='')                                         \r\n";
            SQL += "   AND (DelDate IS NULL OR DelDate ='')                                     \r\n";
            SQL += "   AND (GbSTS IS NULL OR GbSTS <>'7')                                       \r\n";            
            if (argGubun[(int)clsComSupXraySend.enmEtcSupXraySend.EDPS_TEST] != "Y")
            {
                SQL += "  AND Pano NOT IN ('81000000','81000001','81000002','81000003','81000004','81000005','81000006','81000007','81000008','81000009')   \r\n";
            }
            else
            {
                SQL += "  AND Pano IN ('81000000','81000001','81000002','81000003','81000004','81000005','81000006','81000007','81000008','81000009')       \r\n";
            }
            SQL += "  ORDER BY EnterDate,PacsNo                             \r\n";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_HL7_GbSTS_Auto_Set_Part3(PsmhDb pDbCon, string[] argGubun)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                                      \r\n";
            SQL += "  TO_CHAR(EnterDate,'YYYY-MM-DD HH24:MI') EnterDate                         \r\n";
            SQL += "  ,Pano,SName,OrderCode,OrderName                                           \r\n";
            SQL += "  ,XJong,Qty, XCODE,Exid,ROWID                                              \r\n";
            SQL += "  ,KOSMOS_OCS.FC_XRAY_CODE_NM(XCode) FC_XName                               \r\n"; //코드명칭       
            SQL += "  ,KOSMOS_OCS.FC_OCS_ORDERCODE_NAME2(OrderCode) FC_OrderName                \r\n"; //오더명칭       
            SQL += " FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                     \r\n";
            SQL += "  WHERE 1=1                                                                 \r\n";
            SQL += "   AND SeekDate >=TRUNC(SYSDATE-2)                                          \r\n";
            SQL += "   AND (XCode IN ( 'G0400','G0400A','G04009','GR9701' )                     \r\n";
            SQL += "         OR ( XCode ='US15' AND DeptCode <>'TO' ) )                         \r\n";
            SQL += "   AND (GbEnd IS NULL OR GbEnd ='')                                         \r\n";                        
            if (argGubun[(int)clsComSupXraySend.enmEtcSupXraySend.EDPS_TEST] != "Y")
            {
                SQL += "  AND Pano NOT IN ('81000000','81000001','81000002','81000003','81000004','81000005','81000006','81000007','81000008','81000009')   \r\n";
            }
            else
            {
                SQL += "  AND Pano IN ('81000000','81000001','81000002','81000003','81000004','81000005','81000006','81000007','81000008','81000009')       \r\n";
            }
            SQL += "  ORDER BY EnterDate,PacsNo                             \r\n";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_HL7_Xray_Pacs_SReport(PsmhDb pDbCon, string[] argGubun)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                                      \r\n";
            SQL += "   QUEUEID,HISORDERID,PATID,READSTAT                                        \r\n";
            SQL += "   ,CONCLUSION,CONFDR1,ROWID, ADDENDUM                                      \r\n";
            SQL += " FROM " + ComNum.AES_PASS + "XRAY_PACS_SREPORT                              \r\n";            
            SQL += "  WHERE 1=1                                                                 \r\n";
            SQL += "   AND WORKTIME >=TRUNC(SYSDATE-1)                                          \r\n";
            SQL += "   AND FLAG = 'N'                                                           \r\n";            
            if (argGubun[(int)clsComSupXraySend.enmEtcSupXraySend.EDPS_TEST] != "Y")
            {
                SQL += "  AND a.PATID NOT IN ('81000000','81000001','81000002','81000003','81000004','81000005','81000006','81000007','81000008','81000009')   \r\n";
            }
            else
            {
                SQL += "  AND a.PATID IN ('81000000','81000001','81000002','81000003','81000004','81000005','81000006','81000007','81000008','81000009')       \r\n";
            }
            SQL += "  ORDER BY QUEUEID                                                          \r\n";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_HL7_ipdNewMst(PsmhDb pDbCon, string Job, string[] argGubun, string argPano)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                              \r\n";
            if (Job == "3") SQL += " /*+index ( IPD_NEW_MASTER INDEX_IPDNEWMST5 )*/     \r\n";
            SQL += "  TO_CHAR(INDATE,'YYYY-MM-DD') INDATE,                              \r\n";
            SQL += "  TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE,                            \r\n";
            SQL += "  TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE,                            \r\n";
            SQL += "  PANO,IPDNO,SNAME,GBSTS,DEPTCODE,DRCODE,                           \r\n";
            SQL += "  WARDCODE,ROOMCODE,PACS_ADT,ROWID                                  \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                          \r\n";
            SQL += "  WHERE 1=1                                                         \r\n";
            if (Job == "1")
            {
                SQL += "   AND Pano ='" + argPano + "'                                  \r\n";
                SQL += "   AND ActDate IS NULL                                          \r\n";                
            }
            else if (Job == "2") //입원자
            {
                SQL += "   AND INDATE >=TRUNC(SYSDATE-1)                                \r\n";
                SQL += "   AND PACS_ADT IS NULL                                         \r\n";
            }
            else if (Job == "3") //퇴원자
            {
                SQL += "   AND ActDate >=TRUNC(SYSDATE-1)                               \r\n";
                SQL += "   AND GbSTS ='7'                                               \r\n";
                SQL += "   AND PACS_ADT ='II'                                           \r\n";
            }
            else if (Job == "4") //입원취소
            {
                SQL += "   AND InDate >=TRUNC(SYSDATE-1)                                \r\n";
                SQL += "   AND ActDate >=TRUNC(SYSDATE-1)                               \r\n";
                SQL += "   AND GbSTS ='9'                                               \r\n";
                SQL += "   AND PACS_ADT ='II'                                           \r\n";
            }
            else if (Job == "5") //퇴원취소
            {
                SQL += "   AND InDate >=TRUNC(SYSDATE-1)                                \r\n";
                SQL += "   AND ActDate >=TRUNC(SYSDATE-1)                               \r\n";
                SQL += "   AND GbSTS NOT IN ('7', '9')                                  \r\n";
                SQL += "   AND PACS_ADT IN ('II','IC','TT')                             \r\n";
            }
            if (argGubun[(int)clsComSupXraySend.enmEtcSupXraySend.EDPS_TEST] != "Y")
            {
                SQL += "  AND Pano NOT IN ('81000000','81000001','81000002','81000003','81000004','81000005','81000006','81000007','81000008','81000009')   \r\n";
            }
            else
            {
                SQL += "  AND Pano IN ('81000000','81000001','81000002','81000003','81000004','81000005','81000006','81000007','81000008','81000009')       \r\n";
            }
            if (Job == "1")
            {                
                SQL += "  ORDER BY InDate DESC                                          \r\n";
            }

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_HL7_ipdNewMstTrs(PsmhDb pDbCon, string[] argGubun)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                              \r\n";
            SQL += "  TO_CHAR(b.INDATE,'YYYY-MM-DD') INDATE,                            \r\n";
            SQL += "  TO_CHAR(b.OUTDATE,'YYYY-MM-DD') OUTDATE,                          \r\n";
            SQL += "  TO_CHAR(b.ACTDATE,'YYYY-MM-DD') ACTDATE,                          \r\n";
            SQL += "  b.PANO,b.IPDNO,b.SNAME,b.GBSTS,b.DEPTCODE,b.DRCODE,               \r\n";
            SQL += "  b.WARDCODE,b.ROOMCODE,b.PACS_ADT,a.ROWID                          \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "IPD_TRANSFOR a  ,                       \r\n";
            SQL += "    " + ComNum.DB_PMPA + "IPD_NEW_MASTER b                          \r\n";
            SQL += "  WHERE 1=1                                                         \r\n";
            SQL += "   AND a.IPDNO=b.IPDNO                                              \r\n";
            SQL += "   AND a.IPDNO IN (                                                 \r\n";
            SQL += "                   SELECT IPDNO                                     \r\n";
            SQL += "                      FROM KOSMOS_PMPA.IPD_NEW_MASTER               \r\n";
            SQL += "                        WHERE INDATE >=TRUNC(SYSDATE-1)             \r\n";
            SQL += "                         AND PACS_ADT IN ('II','TT','IC','TC')      \r\n";
            SQL += "                    )                                               \r\n";
            SQL += "   AND a.TRSDATE >=TRUNC(SYSDATE-3)                                 \r\n";
            SQL += "   AND ( a.PACS_ADT IS NULL OR a.PACS_ADT ='' )                     \r\n";
            if (argGubun[(int)clsComSupXraySend.enmEtcSupXraySend.EDPS_TEST] != "Y")
            {
                SQL += "  AND a.Pano NOT IN ('81000000','81000001','81000002','81000003','81000004','81000005','81000006','81000007','81000008','81000009') \r\n";
            }
            else
            {
                SQL += "  AND a.Pano IN ('81000000','81000001','81000002','81000003','81000004','81000005','81000006','81000007','81000008','81000009')     \r\n";
            }

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_HL7_Hic_Result(PsmhDb pDbCon, string[] argGubun)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                                  \r\n";
            SQL += "  a.Pano,a.PacsNo,a.XCode,a.HIC_WRTNO,a.HIC_CODE,a.ROWID                \r\n";
            SQL += "  ,a.SName,b.Pano Hic_Pano                                                      \r\n";
            SQL += "  ,TO_CHAR(a.EnterDate,'YYYY-MM-DD') JepDate                            \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a                               \r\n";
            SQL += "    , " + ComNum.DB_PMPA + "HIC_PATIENT b                               \r\n";
            SQL += "  WHERE 1=1                                                             \r\n";
            SQL += "   AND a.Pano =b.Ptno(+)                                                \r\n";
            SQL += "   AND a.SeekDate >= TRUNC(SYSDATE-5)                                   \r\n";
            SQL += "   AND a.DeptCode='HR'                                                  \r\n";
            SQL += "   AND a.PacsNo IS NOT NULL                                             \r\n";
            SQL += "   AND a.GbEnd='1'                                                      \r\n";
            SQL += "   AND a.XJong='1'                                                      \r\n";
            SQL += "   AND a.XSubCode='01'                                                  \r\n";
            SQL += "   AND a.ExInfo < 1000                                                  \r\n";
            SQL += "   AND a.GbHIC IS NULL                                                  \r\n";

            if (argGubun[(int)clsComSupXraySend.enmEtcSupXraySend.EDPS_TEST] != "Y")
            {
                SQL += "  AND a.Pano NOT IN ('81000000','81000001','81000002','81000003','81000004','81000005','81000006','81000007','81000008','81000009') \r\n";
            }
            else
            {
                SQL += "  AND a.Pano IN ('81000000','81000001','81000002','81000003','81000004','81000005','81000006','81000007','81000008','81000009')     \r\n";
            }

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_HL7_Hic_Jepsu_Result(PsmhDb pDbCon,long  argHPano,string argJepDate)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                                  \r\n";
            SQL += "  a.WRTNO,a.GjJong,b.ExCode                                             \r\n";            
            SQL += " FROM " + ComNum.DB_PMPA + "HIC_JEPSU a                                 \r\n";
            SQL += "    , " + ComNum.DB_PMPA + "HIC_RESULT b                                \r\n";
            SQL += "  WHERE 1=1                                                             \r\n";
            SQL += "   AND a.WRTNO=b.WRTNO(+)                                               \r\n";            
            SQL += "   AND a.Pano =" + argHPano + "                                         \r\n";
            SQL += "   AND a.JepDate = TO_DATE('" + argJepDate + "','YYYY-MM-DD')           \r\n";
            SQL += "   AND a.DelDate IS NULL                                                \r\n";
            SQL += "   AND b.ExCode IN ('A142','TX13','TX14','TX11','A213','TX16','A211')   \r\n";
            SQL += "  ORDER BY a.WRTNO DESC                                                 \r\n";
            
            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_HL7_Hic_Jepsu(PsmhDb pDbCon, long argWRTNO)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                                  \r\n";
            SQL += "  a.GjJong,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate                      \r\n";
            SQL += "  ,b.LtdCode,a.Pano,b.PTno,a.JobSabun,a.SName,a.Sex,a.Age               \r\n"; 
            SQL += "  ,KOSMOS_OCS.FC_HIC_LTD_NM(b.LtdCode) FC_LtdName                       \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "HIC_JEPSU a                                 \r\n";
            SQL += "    , " + ComNum.DB_PMPA + "HIC_PATIENT b                               \r\n";
            SQL += "  WHERE 1=1                                                             \r\n";
            SQL += "   AND a.Pano=b.Pano(+)                                                 \r\n";
            SQL += "   AND a.WRTNO =" + argWRTNO + "                                        \r\n";
            
            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_HL7_Hic_SunapDtl(PsmhDb pDbCon, long argWRTNO)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                                  \r\n";
            SQL += "  Code                                                                  \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "HIC_SUNAPDTL                                \r\n";
            SQL += "  WHERE 1=1                                                             \r\n";            
            SQL += "   AND WRTNO =" + argWRTNO + "                                          \r\n";
            SQL += "   AND Code IN (                                                        \r\n";
            SQL += "                 SELECT CODE                                            \r\n";
            SQL += "                  FROM " + ComNum.DB_PMPA + "HIC_CODE                   \r\n";
            SQL += "                  WHERE GUBUN ='98' )                                   \r\n";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_HL7_Hic_Xray_Result(PsmhDb pDbCon, long argHPano, string argJepDate)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                                  \r\n";
            SQL += "  ROWID                                                                 \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "HIC_XRAY_RESULT                             \r\n";
            SQL += "  WHERE 1=1                                                             \r\n";
            SQL += "   AND Pano =" + argHPano + "                                           \r\n";
            SQL += "   AND JepDate=TO_DATE('" + argJepDate + "','YYYY-MM-DD')               \r\n";
            SQL += "   AND DelDate IS NULL                                                  \r\n";
            SQL += "   AND GbPACS  IS NULL                                                  \r\n";            

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_HL7_Ocsills(PsmhDb pDbCon, cXrayPacsSend argHL7)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                                          \r\n";
            SQL += "  b.iLLNameK,a.iLLCode                                                          \r\n";
            if (argHL7.IpdOpd == "O")
            {
                SQL += " FROM " + ComNum.DB_MED + "OCS_OILLS a, " + ComNum.DB_PMPA + "BAS_ILLS b    \r\n";
            }
            else
            {
                SQL += " FROM " + ComNum.DB_MED + "OCS_iILLS a, " + ComNum.DB_PMPA + "BAS_ILLS b    \r\n";
            }

            SQL += "  WHERE 1=1                                                                     \r\n";
            SQL += "   AND a.iLLCode=b.iLLCode                                                      \r\n";
            SQL += "   AND Ptno ='" + argHL7.Pano + "'                                              \r\n";
            
            if (argHL7.IpdOpd == "O")
            {
                SQL += "   AND DeptCode ='" + argHL7.DeptCode + "'                                  \r\n";
                SQL += "   AND BDate =TO_DATE('" + argHL7.BDate + "','YYYY-MM-DD')                  \r\n";
            }
            else
            {
                if (argHL7.InDate == "")
                {
                    SQL += "   AND BDate =TO_DATE('" + argHL7.InDate + "','YYYY-MM-DD')             \r\n";
                }
                else
                {
                    SQL += "   AND BDate >=TO_DATE('" + argHL7.BDate + "','YYYY-MM-DD')             \r\n";
                    SQL += "   AND BDate <=TRUNC(SYSDATE)                                           \r\n";
                }
            }
            SQL += "  ORDER BY a.SeqNo                                                              \r\n";
               

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_HL7_Hic_Xray_Detail(PsmhDb pDbCon, string argPano, string argPacsNo)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                                  \r\n";
            SQL += "  IPDOPD,TO_CHAR(SEEKDATE,'YYYY-MM-DD') SeekDate                        \r\n";
            SQL += "  ,PANO,SNAME,SEX,AGE,DEPTCODE,DRCODE                                   \r\n";
            SQL += "  ,WARDCODE,ROOMCODE,XJONG,XSUBCODE,XCODE,EXINFO,QTY,EXMORE,EXID        \r\n";
            SQL += "  ,GBEND,MGRNO,GBPORTABLE,GbRead,ROWID                                  \r\n";
            SQL += "  ,REMARK,XRAYROOM,GBNGT,DRREMARK,ORDERNO,ORDERCODE,PACSNO,ORDERNAME    \r\n"; 
            SQL += "  ,KOSMOS_OCS.FC_XRAY_CODE_NM(XCode) FC_XName                           \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                 \r\n";
            SQL += "  WHERE 1=1                                                             \r\n";
            SQL += "   AND Pano ='" + argPano + "'                                          \r\n";
            SQL += "   AND PacsNo='" + argPacsNo + "'                                       \r\n";
                        
            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_HL7_Endo_JupMst(PsmhDb pDbCon, string argPtno, string argPacsNo)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                                  \r\n";
            SQL += "  Ptno,PacsNo,ROWID                                                     \r\n";            
            SQL += " FROM " + ComNum.DB_MED + "ENDO_JUPMST                                  \r\n";
            SQL += "  WHERE 1=1                                                             \r\n";
            SQL += "   AND Ptno ='" + argPtno + "'                                          \r\n";
            SQL += "   AND PacsNo='" + argPacsNo + "'                                       \r\n";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }


        public string HL7_Send_EtcSono(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd,  string[] argGubun , cComSupXraySendInfo argCls)
        {
            DataTable dt = null;
            
            string strPacsNo = "";
            string strROWID = "";
            string strOperator = "";

            //쿼리실행      
            dt = sel_HL7_Send_EtcSono(pDbCon,argGubun);

            #region //데이터셋 읽은후 작업

            if ( dt != null && dt.Rows.Count > 0)
            {
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strROWID = dt.Rows[i]["ROWID"].ToString().Trim();
                    strOperator = dt.Rows[i]["Exid"].ToString().Trim();

                    if (dt.Rows[i]["Qty"].ToString().Trim() != "" && Convert.ToInt16(dt.Rows[i]["Qty"].ToString().Trim()) > 0)
                    {
                        //팍스넘버 날짜(YYYYMMDD+0000)
                        strPacsNo = argCls.sysdate + ComFunc.SetAutoZero(ComQuery.GetSequencesNo(pDbCon,"KOSMOS_PMPA", "SEQ_PACSNO").ToString(), 4);
                        
                        //갱신
                        SqlErr = up_HL7_XRAY_DETAIL_STS(pDbCon, "00", strPacsNo, strROWID, ref intRowAffected);
                        if (SqlErr != "") 
                        {                                                    
                            return "up_HL7_XRAY_DETAIL_STS 오류";
                        }
                        else
                        {
                            //추가
                            SqlErr = ins_HL7_XRAY_PACSSEND(pDbCon, "00", strROWID, strOperator, ref intRowAffected);
                            if (SqlErr != "")
                            {                                                   
                                return "ins_HL7_XRAY_PACSSEND 오류";
                            }
                            else
                            {

                                argCls.PacsNo = strPacsNo;
                                argCls.Pano = dt.Rows[i]["Pano"].ToString().Trim();
                                argCls.SName = dt.Rows[i]["SName"].ToString().Trim();
                                argCls.OrderCode = dt.Rows[i]["OrderCode"].ToString().Trim() + "(" +  dt.Rows[i]["XCode"].ToString().Trim() + ")";
                                argCls.OrderName = dt.Rows[i]["FC_OrderName"].ToString().Trim();

                                sheet_add(Spd, argCls);
                            }
                        }
                        
                    }
                }                
            }

            dt.Dispose();
            dt = null;

            #endregion


            return SqlErr;

        }

        public string HL7_Send_Xray(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string[] argGubun, cComSupXraySendInfo argCls, cXrayPacsSend argHL7)
        {
            DataTable dt = null;            

            //쿼리실행      
            dt = sel_HL7_Xray(pDbCon, argGubun);

            #region //데이터셋 읽은후 작업            

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    
                    #region 1건의 정보 설정
                    argHL7.EntDate = dt.Rows[i]["EntDate"].ToString().Trim();
                    argHL7.PacsNo = dt.Rows[i]["PacsNo"].ToString().Trim();
                    argHL7.SendGbn = dt.Rows[i]["SendGbn"].ToString().Trim();
                    argHL7.Pano = dt.Rows[i]["Pano"].ToString().Trim();
                    argHL7.SName = dt.Rows[i]["SName"].ToString().Trim();
                    argHL7.EName = dt.Rows[i]["EName"].ToString().Trim();
                    argHL7.EName2 = dt.Rows[i]["EName2"].ToString().Trim();
                    argHL7.Sex = dt.Rows[i]["Sex"].ToString().Trim();
                    argHL7.Age = dt.Rows[i]["Age"].ToString().Trim();
                    argHL7.DeptCode = dt.Rows[i]["DeptCode"].ToString().Trim();
                    argHL7.DrCode = dt.Rows[i]["DrCode"].ToString().Trim();
                    argHL7.IpdOpd = dt.Rows[i]["IpdOpd"].ToString().Trim();
                    argHL7.WardCode = dt.Rows[i]["WardCode"].ToString().Trim();
                    argHL7.RoomCode = dt.Rows[i]["RoomCode"].ToString().Trim();
                    argHL7.OrderCode = dt.Rows[i]["OrderCode"].ToString().Trim();
                    argHL7.OrderName = dt.Rows[i]["XRayName"].ToString().Trim();
                    argHL7.SeekDate = dt.Rows[i]["SeekDate"].ToString().Trim();
                    argHL7.Remark = dt.Rows[i]["Remark"].ToString().Trim();
                    argHL7.XrayRoom = dt.Rows[i]["XRayRoom"].ToString().Trim();
                    argHL7.XJong = dt.Rows[i]["XJong"].ToString().Trim();
                    argHL7.XCode = dt.Rows[i]["XCode"].ToString().Trim();
                    argHL7.ReadNo = dt.Rows[i]["ReadNo"].ToString().Trim();
                    argHL7.DrRemark = dt.Rows[i]["DrRemark"].ToString().Trim();
                    argHL7.ROWID = dt.Rows[i]["ROWID"].ToString().Trim();

                    //argHL7.MsgControlid = argCls.sysdateTime + ComFunc.SetAutoZero(i.ToString(), 2);
                    argHL7.Modality = "";
                    argHL7.Resource = "";
                    argHL7.Emergency = dt.Rows[i]["FC_GBREAD"].ToString().Trim(); //함수필요 
                    argHL7.XCode = "";
                    argHL7.BDate = "";
                    argHL7.Disease = HL7_read_Xray_DISEASE_set(pDbCon,argHL7);

                    #endregion

                    //HL7_Ftp_Send
                    argHL7.STS = true; //전송 상태 플래그
                    argHL7.STS = work_Pacs_Ftp_Send(pDbCon, argHL7);

                    if (argHL7.STS ==true)
                    {
                        //갱신
                        SqlErr = up_HL7_XRAY_PACSSEND(pDbCon, "", argHL7.ROWID, argHL7.OrderName, ref intRowAffected);
                        if (SqlErr != "")
                        {
                            return "up_XRAY_PACSSEND 오류";
                        }
                        else
                        {
                            //갱신
                            SqlErr = up_HL7_XRAY_DETAIL(pDbCon, "01", argHL7.Pano, argHL7.PacsNo, argHL7.OrderName, argHL7.Remark,"","", ref intRowAffected);
                            if (SqlErr != "")
                            {
                                return "up_XRAY_DETAIL 오류";
                            }
                            else
                            {
                                #region //1건 내역 표시
                                argCls.PacsNo = argHL7.PacsNo;
                                argCls.Pano = argHL7.Pano;
                                argCls.SName = argHL7.SName;
                                argCls.OrderCode = argHL7.OrderCode + "(" + argHL7.XCode + ")";
                                argCls.OrderName = argHL7.OrderName;

                                sheet_add(Spd, argCls); 
                                #endregion
                            }
                        }
                    }
                    

                }

            }

            dt.Dispose();
            dt = null;

            #endregion


            return SqlErr;
        }

        public string HL7_Send_Endo(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string[] argGubun, cComSupXraySendInfo argCls, cXrayPacsSend argHL7)
        {
            DataTable dt = null;                                

            //쿼리실행      
            dt = sel_HL7_Endo(pDbCon, argGubun);

            #region //데이터셋 읽은후 작업            

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region 1건의 정보 설정
                    argHL7.SendGbn = "3";//기본수정
                    argHL7.EntDate = dt.Rows[i]["EntDate"].ToString().Trim();
                    argHL7.EndoChk = dt.Rows[i]["EndoChk"].ToString().Trim();
                    argHL7.PacsNo = dt.Rows[i]["PacsNo"].ToString().Trim();
                    argHL7.ReadNo = "";
                    if (argHL7.PacsNo =="")
                    {
                        argHL7.SendGbn = "1";//신규
                        argHL7.PacsNo = sup.NewPacsNo(pDbCon, argHL7.sysdate);  //dt.Rows[i]["PacsNo"].ToString().Trim();//체크
                        
                        
                    }
                    if (dt.Rows[i]["GbSunap"].ToString().Trim() =="*")
                    {
                        argHL7.SendGbn = "2"; //접수취소
                    }
                    
                    argHL7.Pano = dt.Rows[i]["Pano"].ToString().Trim();
                    argHL7.SName = dt.Rows[i]["SName"].ToString().Trim();
                    argHL7.EName = ""; // dt.Rows[i]["EName"].ToString().Trim();
                    argHL7.EName2 = dt.Rows[i]["EName"].ToString().Trim();
                    argHL7.Sex = dt.Rows[i]["Sex"].ToString().Trim();
                    argHL7.Age = dt.Rows[i]["FC_AGE"].ToString().Trim(); 
                    argHL7.DeptCode = dt.Rows[i]["DeptCode"].ToString().Trim();
                    argHL7.DrCode = dt.Rows[i]["DrCode"].ToString().Trim();
                    argHL7.IpdOpd = dt.Rows[i]["IpdOpd"].ToString().Trim();
                    argHL7.WardCode = dt.Rows[i]["WardCode"].ToString().Trim();
                    argHL7.RoomCode = dt.Rows[i]["RoomCode"].ToString().Trim();
                    argHL7.OrderCode = dt.Rows[i]["OrderCode"].ToString().Trim();
                    argHL7.OrderName = dt.Rows[i]["XRayName"].ToString().Trim();
                    argHL7.SeekDate = dt.Rows[i]["SeekDate"].ToString().Trim();
                    argHL7.Remark = ""; // dt.Rows[i]["Remark"].ToString().Trim();
                    argHL7.XrayRoom = ""; // dt.Rows[i]["XRayRoom"].ToString().Trim();
                    argHL7.XJong = "D"; //내시경
                    argHL7.XCode = "";
                    argHL7.Buse = dt.Rows[i]["BUSE"].ToString().Trim();
                    argHL7.DrRemark = ""; //dt.Rows[i]["DrRemark"].ToString().Trim();
                    argHL7.ROWID = dt.Rows[i]["ROWID"].ToString().Trim();

                    //argHL7.MsgControlid = argCls.sysdateTime + ComFunc.SetAutoZero(i.ToString(), 2);
                    argHL7.Modality = "";
                    argHL7.Resource = "";
                    argHL7.Emergency = "";
                    argHL7.XCode = "";
                    argHL7.BDate = dt.Rows[i]["BDate"].ToString().Trim();
                    argHL7.Disease = HL7_read_Xray_DISEASE_set(pDbCon, argHL7);

                    #endregion

                    //HL7_Ftp_Send
                    argHL7.STS = true; //전송 상태 플래그
                    argHL7.STS = work_Pacs_Ftp_Send(pDbCon, argHL7);

                    if (argHL7.STS == true)
                    {
                        //갱신
                        if (argHL7.SendGbn =="2")
                        {
                            SqlErr = up_HL7_ENDO_JUPMST(pDbCon, "CAN", argHL7.ROWID, argHL7.PacsNo,"", ref intRowAffected);
                        }
                        else
                        {
                            SqlErr = up_HL7_ENDO_JUPMST(pDbCon, "UP", argHL7.ROWID, argHL7.PacsNo,"", ref intRowAffected);
                        }
                        
                        if (SqlErr != "")
                        {
                            return "up_ENDO_JUPMST 오류";
                        }
                        else
                        {                           
                            #region //1건 내역 표시
                            argCls.PacsNo = argHL7.PacsNo;
                            argCls.Pano = argHL7.Pano;
                            argCls.SName = argHL7.SName;
                            argCls.OrderCode = argHL7.OrderCode + "(" + argHL7.XCode + ")";
                            argCls.OrderName = argHL7.OrderName;

                            sheet_add(Spd, argCls);
                            #endregion
                            
                        }
                    }
                    
                }

            }

            dt.Dispose();
            dt = null;

            #endregion


            return SqlErr;
        }

        public string HL7_Send_Endo_Result(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string[] argGubun, cComSupXraySendInfo argCls, cXrayPacsSend argHL7)
        {
            DataTable dt = null;

            //쿼리실행      
            dt = sel_HL7_Endo_Result(pDbCon, argGubun);

            #region //데이터셋 읽은후 작업            

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region 1건의 정보 설정
                    argHL7.SendGbn = "5";//기본수정
                    argHL7.EntDate =  dt.Rows[i]["ResultDate"].ToString().Trim();
                    argHL7.EndoChk = ""; //dt.Rows[i]["EndoChk"].ToString().Trim();
                    argHL7.PacsNo = dt.Rows[i]["PacsNo"].ToString().Trim();
                    argHL7.ReadNo = "";

                    argHL7.Pano = dt.Rows[i]["Pano"].ToString().Trim();
                    argHL7.SName = dt.Rows[i]["SName"].ToString().Trim();
                    argHL7.EName = ""; // dt.Rows[i]["EName"].ToString().Trim();
                    argHL7.EName2 = dt.Rows[i]["EName"].ToString().Trim();
                    argHL7.Sex = dt.Rows[i]["Sex"].ToString().Trim();
                    argHL7.Age = dt.Rows[i]["FC_AGE"].ToString().Trim();
                    argHL7.DeptCode = dt.Rows[i]["DeptCode"].ToString().Trim();
                    argHL7.DrCode = dt.Rows[i]["DrCode"].ToString().Trim();
                    argHL7.IpdOpd = dt.Rows[i]["IpdOpd"].ToString().Trim();
                    argHL7.WardCode = dt.Rows[i]["WardCode"].ToString().Trim();
                    argHL7.RoomCode = dt.Rows[i]["RoomCode"].ToString().Trim();
                    argHL7.OrderCode = dt.Rows[i]["OrderCode"].ToString().Trim();
                    argHL7.OrderName = dt.Rows[i]["XRayName"].ToString().Trim();
                    argHL7.SeekDate = dt.Rows[i]["RDate"].ToString().Trim();
                    argHL7.Remark = ""; // dt.Rows[i]["Remark"].ToString().Trim();
                    argHL7.XrayRoom = ""; // dt.Rows[i]["XRayRoom"].ToString().Trim();
                    argHL7.XJong = "D"; //내시경
                    argHL7.XCode = "";
                    argHL7.Buse = dt.Rows[i]["BUSE"].ToString().Trim();
                    argHL7.DrRemark = ""; //dt.Rows[i]["DrRemark"].ToString().Trim();
                    argHL7.ROWID = dt.Rows[i]["ROWID"].ToString().Trim();

                    //argHL7.MsgControlid = argCls.sysdateTime + ComFunc.SetAutoZero(i.ToString(), 2); //체크
                    //cXrayPacsSend.MsgControlid = dt.Rows[i]["PacsNo"].ToString().Trim() + VB.Mid(DateTime.Now.ToString("hh:mm:ss"), 4, 2) + VB.Right(DateTime.Now.ToString("hh:mm:ss"), 2);
                    argHL7.Modality = "";
                    argHL7.Resource = "";
                    argHL7.Emergency = "";
                    argHL7.XCode = "";
                    argHL7.BDate = dt.Rows[i]["BDate"].ToString().Trim();
                    argHL7.Disease = HL7_read_Xray_DISEASE_set(pDbCon, argHL7);

                    #endregion

                    //HL7_Ftp_Send
                    argHL7.STS = true; //전송 상태 플래그
                    argHL7.STS = work_Pacs_Ftp_Send(pDbCon, argHL7);

                    if (argHL7.STS == true)
                    {
                        //갱신
                        SqlErr = up_HL7_ENDO_JUPMST(pDbCon, "UP2", argHL7.ROWID, argHL7.PacsNo,"", ref intRowAffected);                       
                        if (SqlErr != "")
                        {
                            return "up_ENDO_JUPMST 오류";
                        }
                        else
                        {
                            #region //1건 내역 표시
                            argCls.PacsNo = argHL7.PacsNo;
                            argCls.Pano = argHL7.Pano;
                            argCls.SName = argHL7.SName;
                            argCls.OrderCode = argHL7.OrderCode + "(" + argHL7.XCode + ")";
                            argCls.OrderName = argHL7.OrderName;

                            sheet_add(Spd, argCls);
                            #endregion

                        }
                    }

                }

            }

            dt.Dispose();
            dt = null;

            #endregion


            return SqlErr;
        }

        public string HL7_Send_Laparoscopy(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string[] argGubun, cComSupXraySendInfo argCls)
        {
            DataTable dt = null;
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strPacsNo = "";
            string strROWID = "";

            //쿼리실행      
            dt = sel_HL7_Laparoscopy(pDbCon,argGubun);

            #region //데이터셋 읽은후 작업            

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strROWID = dt.Rows[i]["ROWID"].ToString().Trim();

                    if (dt.Rows[i]["Qty"].ToString().Trim() != "" && Convert.ToInt16(dt.Rows[i]["Qty"].ToString().Trim()) > 0)
                    {
                        //팍스넘버 날짜(YYYYMMDD+0000)
                        strPacsNo = argCls.sysdate + ComFunc.SetAutoZero(ComQuery.GetSequencesNo(clsDB.DbCon, "KOSMOS_PMPA", "SEQ_PACSNO").ToString(), 4);

                        //갱신
                        SqlErr = up_HL7_XRAY_DETAIL_STS(pDbCon, "03", strPacsNo, strROWID, ref intRowAffected);
                        if (SqlErr != "")
                        {
                            return "up_HL7_XRAY_DETAIL_STS 오류";
                        }
                        else
                        {
                            //추가
                            SqlErr = ins_HL7_XRAY_PACSSEND(pDbCon, "03", strROWID, "", ref intRowAffected);
                            if (SqlErr != "")
                            {
                                return "ins_HL7_XRAY_PACSSEND 오류";
                            }
                            else
                            {
                                #region //1건 내역 표시
                                argCls.PacsNo = strPacsNo;
                                argCls.Pano = dt.Rows[i]["Pano"].ToString().Trim();
                                argCls.SName = dt.Rows[i]["SName"].ToString().Trim();
                                argCls.OrderCode = dt.Rows[i]["OrderCode"].ToString().Trim() + "(" + dt.Rows[i]["XCode"].ToString().Trim() + ")";
                                argCls.OrderName = dt.Rows[i]["FC_OrderName"].ToString().Trim();

                                sheet_add(Spd, argCls);
                                #endregion
                            }
                        }
                    }
                }

            }

            dt.Dispose();
            dt = null;

            #endregion


            return SqlErr;
        }

        public string HL7_Send_GbSTS_AUTO_SET_PART1(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string[] argGubun, cComSupXraySendInfo argCls)
        {
            DataTable dt = null;
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strPacsNo = "";
            string strROWID = "";

            //쿼리실행      
            dt = sel_HL7_GbSTS_Auto_Set_Part1(pDbCon, argGubun);

            #region //데이터셋 읽은후 작업            

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strROWID = dt.Rows[i]["ROWID"].ToString().Trim();

                    if (dt.Rows[i]["Qty"].ToString().Trim() != "" && Convert.ToInt16(dt.Rows[i]["Qty"].ToString().Trim()) > 0)
                    {
                        
                        //갱신
                        SqlErr = up_HL7_XRAY_DETAIL_STS(pDbCon, "06_01", strPacsNo, strROWID, ref intRowAffected);
                        if (SqlErr != "")
                        {
                            return "up_HL7_Send_GbSTS_AUTO_SET_PART1 오류";
                        }
                        else
                        {
                            #region //1건 내역 표시
                            argCls.PacsNo = strPacsNo;
                            argCls.Pano = dt.Rows[i]["Pano"].ToString().Trim();
                            argCls.SName = dt.Rows[i]["SName"].ToString().Trim();
                            argCls.OrderCode = dt.Rows[i]["OrderCode"].ToString().Trim() + "(" + dt.Rows[i]["XCode"].ToString().Trim() + ")";
                            argCls.OrderName = dt.Rows[i]["FC_OrderName"].ToString().Trim();

                            sheet_add(Spd, argCls);
                            #endregion
                        }
                    }
                }

            }

            dt.Dispose();
            dt = null;

            #endregion


            return SqlErr;
        }

        public string HL7_Send_GbSTS_AUTO_SET_PART2(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string[] argGubun, cComSupXraySendInfo argCls)
        {
            DataTable dt = null;
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strPacsNo = "";
            string strROWID = "";

            //쿼리실행      
            dt = sel_HL7_GbSTS_Auto_Set_Part2(pDbCon, argGubun);

            #region //데이터셋 읽은후 작업            

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strROWID = dt.Rows[i]["ROWID"].ToString().Trim();

                    if (dt.Rows[i]["Qty"].ToString().Trim() != "" && Convert.ToInt16(dt.Rows[i]["Qty"].ToString().Trim()) > 0)
                    {

                        //갱신
                        SqlErr = up_HL7_XRAY_DETAIL_STS(pDbCon, "06_02", strPacsNo, strROWID, ref intRowAffected);
                        if (SqlErr != "")
                        {
                            return "up_HL7_Send_GbSTS_AUTO_SET_PART2 오류";
                        }
                        else
                        {
                            #region //1건 내역 표시
                            argCls.PacsNo = strPacsNo;
                            argCls.Pano = dt.Rows[i]["Pano"].ToString().Trim();
                            argCls.SName = dt.Rows[i]["SName"].ToString().Trim();
                            argCls.OrderCode = dt.Rows[i]["OrderCode"].ToString().Trim() + "(" + dt.Rows[i]["XCode"].ToString().Trim() + ")";
                            argCls.OrderName = dt.Rows[i]["FC_OrderName"].ToString().Trim();

                            sheet_add(Spd, argCls);
                            #endregion
                        }
                    }
                }

            }

            dt.Dispose();
            dt = null;

            #endregion


            return SqlErr;
        }

        public string HL7_Send_GbSTS_AUTO_SET_PART3(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string[] argGubun, cComSupXraySendInfo argCls)
        {
            DataTable dt = null;
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strPacsNo = "";
            string strROWID = "";

            //쿼리실행      
            dt = sel_HL7_GbSTS_Auto_Set_Part3(pDbCon, argGubun);

            #region //데이터셋 읽은후 작업            

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strROWID = dt.Rows[i]["ROWID"].ToString().Trim();

                    if (dt.Rows[i]["Qty"].ToString().Trim() != "" && Convert.ToInt16(dt.Rows[i]["Qty"].ToString().Trim()) > 0)
                    {

                        //갱신
                        SqlErr = up_HL7_XRAY_DETAIL_STS(pDbCon, "06_03", strPacsNo, strROWID, ref intRowAffected);
                        if (SqlErr != "")
                        {
                            return "up_HL7_Send_GbSTS_AUTO_SET_PART3 오류";
                        }
                        else
                        {
                            #region //1건 내역 표시
                            argCls.PacsNo = strPacsNo;
                            argCls.Pano = dt.Rows[i]["Pano"].ToString().Trim();
                            argCls.SName = dt.Rows[i]["SName"].ToString().Trim();
                            argCls.OrderCode = dt.Rows[i]["OrderCode"].ToString().Trim() + "(" + dt.Rows[i]["XCode"].ToString().Trim() + ")";
                            argCls.OrderName = dt.Rows[i]["FC_OrderName"].ToString().Trim();

                            sheet_add(Spd, argCls);
                            #endregion

                        }
                    }
                }

            }

            dt.Dispose();
            dt = null;

            #endregion


            return SqlErr;
        }

        public string HL7_Send_IPD_STS_ADT_A01(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string[] argGubun, cComSupXraySendInfo argCls)
        {
            DataTable dt = null;
            string strQUEUEID = "";

            #region 입원자 체크
            //입원자 체크
            dt = sel_HL7_ipdNewMst(pDbCon, "2", argGubun, "");
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //팍스넘버 날짜(YYYYMMDD+0000)
                    strQUEUEID = argCls.sysdateTime + ComFunc.SetAutoZero(ComQuery.GetSequencesNo(pDbCon, "KOSMOS_PMPA", "SEQ_PACSNO").ToString(), 4);

                    if (HL7_PACS_IPD_ADT_SEND(pDbCon, "A01", dt.Rows[i]["Pano"].ToString().Trim(), dt.Rows[i]["DeptCode"].ToString().Trim(), dt.Rows[i]["DrCode"].ToString().Trim(), dt.Rows[i]["WardCode"].ToString().Trim(), dt.Rows[i]["RoomCode"].ToString().Trim(), strQUEUEID, "HL7") == true)
                    {
                        SqlErr = up_HL7_ipdNewMst(pDbCon, "II", dt.Rows[i]["ROWID"].ToString().Trim(), ref intRowAffected);

                        #region //1건 내역 표시
                        argCls.PacsNo = "";
                        argCls.Pano = dt.Rows[i]["Pano"].ToString().Trim();
                        argCls.SName = dt.Rows[i]["SName"].ToString().Trim();
                        argCls.OrderCode = "";
                        argCls.OrderName = dt.Rows[i]["WardCode"].ToString().Trim() + "("+ dt.Rows[i]["RoomCode"].ToString().Trim() + ")";

                        sheet_add(Spd, argCls);
                        #endregion
                    }
                    else
                    {
                        return "HL7_Send_IPD_STS_ADT_A01";
                    }
                }

            }

            #endregion

            return SqlErr;

        }

        public string HL7_Send_IPD_STS_ADT_A03(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string[] argGubun, cComSupXraySendInfo argCls)
        {
            DataTable dt = null;
            string strQUEUEID = "";

            #region 퇴원자 체크
            //퇴원자 체크
            dt = sel_HL7_ipdNewMst(pDbCon, "3", argGubun, "");
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //팍스넘버 날짜(YYYYMMDD+0000)
                    strQUEUEID = argCls.sysdateTime + ComFunc.SetAutoZero(ComQuery.GetSequencesNo(pDbCon, "KOSMOS_PMPA", "SEQ_PACSNO").ToString(), 4);

                    if (HL7_PACS_IPD_ADT_SEND(pDbCon, "A03", dt.Rows[i]["Pano"].ToString().Trim(), dt.Rows[i]["DeptCode"].ToString().Trim(), dt.Rows[i]["DrCode"].ToString().Trim(), dt.Rows[i]["WardCode"].ToString().Trim(), dt.Rows[i]["RoomCode"].ToString().Trim(), strQUEUEID, "HL7") == true)
                    {
                        SqlErr = up_HL7_ipdNewMst(pDbCon, "TT", dt.Rows[i]["ROWID"].ToString().Trim(), ref intRowAffected);

                        #region //1건 내역 표시
                        argCls.PacsNo = "";
                        argCls.Pano = dt.Rows[i]["Pano"].ToString().Trim();
                        argCls.SName = dt.Rows[i]["SName"].ToString().Trim();
                        argCls.OrderCode = "";
                        argCls.OrderName = dt.Rows[i]["WardCode"].ToString().Trim() + "(" + dt.Rows[i]["RoomCode"].ToString().Trim() + ")";

                        sheet_add(Spd, argCls);
                        #endregion

                    }
                    else
                    {
                        return "HL7_Send_IPD_STS_ADT_A03";
                    }
                }
            }

            #endregion                       

            return SqlErr;

        }

        public string HL7_Send_IPD_STS_ADT_A11(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string[] argGubun, cComSupXraySendInfo argCls)
        {
            DataTable dt = null;
            string strQUEUEID = "";

            #region 입원취소 체크
            //입원취소 체크
            dt = sel_HL7_ipdNewMst(pDbCon, "4", argGubun, "");
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //팍스넘버 날짜(YYYYMMDD+0000)
                    strQUEUEID = argCls.sysdateTime + ComFunc.SetAutoZero(ComQuery.GetSequencesNo(pDbCon, "KOSMOS_PMPA", "SEQ_PACSNO").ToString(), 4);

                    if (HL7_PACS_IPD_ADT_SEND(pDbCon, "A11", dt.Rows[i]["Pano"].ToString().Trim(), dt.Rows[i]["DeptCode"].ToString().Trim(), dt.Rows[i]["DrCode"].ToString().Trim(), dt.Rows[i]["WardCode"].ToString().Trim(), dt.Rows[i]["RoomCode"].ToString().Trim(), strQUEUEID, "HL7") == true)
                    {
                        SqlErr = up_HL7_ipdNewMst(pDbCon, "IC", dt.Rows[i]["ROWID"].ToString().Trim(), ref intRowAffected);

                        #region //1건 내역 표시
                        argCls.PacsNo = "";
                        argCls.Pano = dt.Rows[i]["Pano"].ToString().Trim();
                        argCls.SName = dt.Rows[i]["SName"].ToString().Trim();
                        argCls.OrderCode = "";
                        argCls.OrderName = dt.Rows[i]["WardCode"].ToString().Trim() + "(" + dt.Rows[i]["RoomCode"].ToString().Trim() + ")";

                        sheet_add(Spd, argCls);
                        #endregion

                    }
                    else
                    {
                        return "HL7_Send_IPD_STS_ADT_A11";
                    }

                }
            }

            #endregion                        

            return SqlErr;

        }

        public string HL7_Send_IPD_STS_ADT_A13(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string[] argGubun, cComSupXraySendInfo argCls)
        {
            DataTable dt = null;
            string strQUEUEID = "";

            #region 퇴원취소 체크
            //퇴원취소 체크
            dt = sel_HL7_ipdNewMst(pDbCon, "5", argGubun, "");
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //팍스넘버 날짜(YYYYMMDD+0000)
                    strQUEUEID = argCls.sysdateTime + ComFunc.SetAutoZero(ComQuery.GetSequencesNo(pDbCon, "KOSMOS_PMPA", "SEQ_PACSNO").ToString(), 4);

                    if (HL7_PACS_IPD_ADT_SEND(pDbCon, "A13", dt.Rows[i]["Pano"].ToString().Trim(), dt.Rows[i]["DeptCode"].ToString().Trim(), dt.Rows[i]["DrCode"].ToString().Trim(), dt.Rows[i]["WardCode"].ToString().Trim(), dt.Rows[i]["RoomCode"].ToString().Trim(), strQUEUEID, "HL7") == true)
                    {
                        SqlErr = up_HL7_ipdNewMst(pDbCon, "TC", dt.Rows[i]["ROWID"].ToString().Trim(), ref intRowAffected);

                        #region //1건 내역 표시
                        argCls.PacsNo = "";
                        argCls.Pano = dt.Rows[i]["Pano"].ToString().Trim();
                        argCls.SName = dt.Rows[i]["SName"].ToString().Trim();
                        argCls.OrderCode = "";
                        argCls.OrderName = dt.Rows[i]["WardCode"].ToString().Trim() + "(" + dt.Rows[i]["RoomCode"].ToString().Trim() + ")";

                        sheet_add(Spd, argCls);
                        #endregion

                    }
                    else
                    {
                        return "HL7_Send_IPD_STS_ADT_A13";
                    }
                }
            }

            #endregion                       

            return SqlErr;

        }

        public string HL7_Send_IPD_STS_ADT_A02(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string[] argGubun, cComSupXraySendInfo argCls)
        {
            DataTable dt = null;
            string strQUEUEID = "";

            #region 전실전과 체크
            //전실전과 체크
            dt = sel_HL7_ipdNewMstTrs(pDbCon, argGubun);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //팍스넘버 날짜(YYYYMMDD+0000)
                    strQUEUEID = argCls.sysdateTime + ComFunc.SetAutoZero(ComQuery.GetSequencesNo(pDbCon, "KOSMOS_PMPA", "SEQ_PACSNO").ToString(), 4);

                    if (HL7_PACS_IPD_ADT_SEND(pDbCon, "A02", dt.Rows[i]["Pano"].ToString().Trim(), dt.Rows[i]["DeptCode"].ToString().Trim(), dt.Rows[i]["DrCode"].ToString().Trim(), dt.Rows[i]["WardCode"].ToString().Trim(), dt.Rows[i]["RoomCode"].ToString().Trim(), strQUEUEID, "HL7") == true)
                    {
                        SqlErr = up_HL7_ipdTransfor(pDbCon, "*", dt.Rows[i]["ROWID"].ToString().Trim(), ref intRowAffected);

                        #region //1건 내역 표시
                        argCls.PacsNo = "";
                        argCls.Pano = dt.Rows[i]["Pano"].ToString().Trim();
                        argCls.SName = dt.Rows[i]["SName"].ToString().Trim();
                        argCls.OrderCode = "";
                        argCls.OrderName = dt.Rows[i]["WardCode"].ToString().Trim() + "(" + dt.Rows[i]["RoomCode"].ToString().Trim() + ")";

                        sheet_add(Spd, argCls);
                        #endregion

                    }
                    else
                    {
                        return "HL7_Send_IPD_STS_ADT_A02";
                    }

                }
            }

            #endregion

            return SqlErr;

        }

        public string HL7_Send_PacsEnd_Set(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string[] argGubun, cComSupXraySendInfo argCls)
        {

            #region //변수선언..
            DataTable dt = null;
            DataTable dt1 = null;
            //DataTable dt2 = null;

            string strPano = "";
            string strPacsNo = "";
            string strUID = "";
            string strFLAG = "";
            string strCONCLUSION = "";
            string strCONFDR1 = "";
            string strADDENDUM = "";
            string strXJong = "";
            string strGbRead = "";
            string strROWID = "";

            long nPano = 0;

            bool bOK = false;
            bool bSend_OK = false; 
            #endregion

            //쿼리실행      
            dt = sel_HL7_Xray_Pacs_SReport(pDbCon, argGubun);

            #region //데이터셋 읽은후 작업            

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region //변수설정
                    strPano = dt.Rows[i]["Patid"].ToString().Trim();
                    strPacsNo = dt.Rows[i]["HISORDERID"].ToString().Trim();
                    strUID = dt.Rows[i]["QUEUEID"].ToString().Trim();
                    strFLAG = dt.Rows[i]["READSTAT"].ToString().Trim();
                    strCONCLUSION = dt.Rows[i]["CONCLUSION"].ToString().Trim();
                    strCONFDR1 = dt.Rows[i]["CONFDR1"].ToString().Trim();
                    strADDENDUM = dt.Rows[i]["ADDENDUM"].ToString().Trim();

                    strXJong = "";
                    strROWID = "";
                    strGbRead = "";

                    #endregion

                    bOK = true;
                    bSend_OK = false;
                    //---------( 판독결과 PACS에서 OCS로 전송)----------------
                    if (strCONCLUSION !="" && strCONFDR1 !="" && strFLAG =="C")
                    {
                        #region //HL7_PACS_SRESULT_UPDATE
                        if (HL7_PACS_SRESULT_UPDATE(pDbCon, strPano, strPacsNo, strCONCLUSION, strCONFDR1, strADDENDUM) == false)
                        {
                            bOK = false;
                        }
                        else
                        {
                            bSend_OK = true;
                        } 
                        #endregion
                    }
                    else if (VB.Left(strPano,1) =="H")
                    {
                        #region //일반검진
                        //일반검진
                        nPano = 0;
                        nPano = Convert.ToInt32(strPano.Replace("H", ""));
                        SqlErr = up_HL7_HIC_XRAY_RESULT(pDbCon, "HIC_XRAY_RESULT_02", strPano, strPacsNo, nPano, "", ref intRowAffected);

                        bSend_OK = true; 
                        #endregion
                    }
                    else
                    {
                        #region //건진 촬영완료 SET
                        //건진 촬영완료 SET
                        SqlErr = up_HL7_HIC_XRAY_RESULT(pDbCon, "HIC_XRAY_RESULT_03", strPano, strPacsNo, 0, "", ref intRowAffected);
                        if (intRowAffected > 0)
                        {
                            bSend_OK = true;
                        } 
                        #endregion
                    }                                       

                    if (bSend_OK == false)
                    {
                        dt1 = sel_HL7_Hic_Xray_Detail(pDbCon, strPano, strPacsNo);
                        if (dt1 != null && dt1.Rows.Count > 0)
                        {
                            #region //xray_detail 체크
                            strXJong = dt1.Rows[0]["XJong"].ToString().Trim();
                            strGbRead = dt1.Rows[0]["GbRead"].ToString().Trim();
                            strROWID = dt1.Rows[0]["ROWID"].ToString().Trim();

                            if (strGbRead != "Y")
                            {
                                if (dt1.Rows[0]["DrRemark"].ToString().Trim().Contains("판독") == true)
                                {
                                    strGbRead = "Y";
                                }
                            }

                            dt1.Dispose();
                            dt1 = null; 
                            #endregion
                        }
                        else
                        {
                            #region //endo_jupmst 체크
                            dt1 = sel_HL7_Endo_JupMst(pDbCon, strPano, strPacsNo);
                            if (dt1 != null && dt1.Rows.Count > 0)
                            {
                                strXJong = "ENDO";
                                strROWID = dt1.Rows[0]["ROWID"].ToString().Trim();

                                dt1.Dispose();
                                dt1 = null;
                            } 
                            #endregion
                        }

                        if (strXJong == "ENDO")
                        {
                            #region //내시경 갱신
                            if (strFLAG == "M")
                            {
                                SqlErr = up_HL7_ENDO_JUPMST(pDbCon, "UP3_01", strROWID, strPacsNo, strUID, ref intRowAffected);
                            }
                            else
                            {
                                SqlErr = up_HL7_ENDO_JUPMST(pDbCon, "UP3_02", strROWID, strPacsNo, strUID, ref intRowAffected);
                            }
                            if (SqlErr != "")
                            {
                                return "up_ENDO_JUPMST 오류";
                            } 
                            #endregion
                        }
                        else
                        {
                            #region //xray_detail 갱신
                            //PACS 전송용 Table에 INSERT(촬영완료)
                            //특수촬영은 특수촬영후 CR로 촬영하기 위하여 촬영완료를 않함
                            //(방사선접수에서 재료입력을 해야만 WorkList에서 없어짐)
                            if (strXJong != "2" && strFLAG == "M")
                            {
                                SqlErr = ins_HL7_XRAY_PACSSEND(pDbCon, "PACSEND_SET", strROWID, "", ref intRowAffected);
                            }

                            //XRAY_DETAIL에 촬영완료 UPDATE
                            //단순촬영은 영상이 전송되면 바로 재료입력 처리함
                            //촬영완료시간(영상전송시간)을 Update
                            //2006-06-30 안저촬영 자용 재료입력 처리(이양재)
                            //2008-07-08 BMD 자동 재료입력
                            if (strFLAG == "M")
                            {
                                if (strXJong == "1" || strXJong == "F" || strXJong == "7")
                                {
                                    SqlErr = up_HL7_XRAY_DETAIL(pDbCon, "PACSEND_SET_01", strPano, strPacsNo, "", "", strGbRead, strROWID, ref intRowAffected);
                                }
                                else
                                {
                                    SqlErr = up_HL7_XRAY_DETAIL(pDbCon, "PACSEND_SET_02", strPano, strPacsNo, "", "", strGbRead, strROWID, ref intRowAffected);
                                }
                            }
                            else
                            {
                                SqlErr = up_HL7_XRAY_DETAIL(pDbCon, "PACSEND_SET_03", strPano, strPacsNo, "", "", strGbRead, strROWID, ref intRowAffected);
                            }
                            if (SqlErr != "")
                            {
                                return "up_XRAY_DETAIL 오류";
                            } 
                            #endregion
                        }

                        //처리완료 처리
                        SqlErr = up_HL7_XRAY_PACS_SREPORT(pDbCon, strROWID,ref intRowAffected);
                                                
                        if (SqlErr != "")
                        {
                            return "up_ENDO_JUPMST 오류";
                        }
                        else
                        {
                            #region //1건 내역 표시
                            argCls.PacsNo = strPacsNo;
                            argCls.Pano = strPano;
                            argCls.SName = "";
                            argCls.OrderCode = "";
                            argCls.OrderName = "";

                            sheet_add(Spd, argCls);

                            #endregion
                        }
                    }
                }

            }

            dt.Dispose();
            dt = null;

            #endregion

            return SqlErr;
        }
        
        bool HL7_PACS_IPD_ADT_SEND(PsmhDb pDbCon, string argGubun, string argPano, string argDept, string argDrCode, string argWard, string argRoom, string argQUEUEID,string argSabun)
        {
            DataTable dt = null;
            string SName = "";
            string EName2 = "";
            string Sex = "";
            string Birth = "";

            dt = sel_basPatient(pDbCon, argPano);

            if (dt.Rows.Count > 0)
            {
                SName = dt.Rows[0]["SName"].ToString().Trim();
                EName2 = dt.Rows[0]["EName"].ToString().Trim();
                Sex = dt.Rows[0]["Sex"].ToString().Trim();
                Birth = dt.Rows[0]["Birth"].ToString().Trim();

                //생성
                SQL = " INSERT INTO " + ComNum.DB_PACS + "XRAY_PACS_ADT                                         \r\n";
                SQL += "  (QUEUEID,FLAG,WORKTIME,PATID,EVENTTYPE,BIRTHDAY,DEPT,ATTENDDOCT1,PATNAME,             \r\n";
                SQL += "   PATTYPE,PERSONALID,Sex,Ward,RoomNo,INPS,INPT_DT )   VALUES                           \r\n";
                SQL += "  ( '" + argQUEUEID + "','N',SYSDATE,                                                   \r\n";
                SQL += "  '" + argPano + "','" + argGubun + "','" + Birth + "',                                 \r\n";
                SQL += "  '" + argDept + "','" + argDrCode + "','" + SName + "','I','',                         \r\n";
                SQL += "  '" + Sex + "','" + argWard + "','" + argRoom + "','" + argSabun +"',SYSDATE           \r\n";
                SQL += "  )                                                                                     \r\n";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            else
            {
                return false;
            }

        }

        public string HL7_Send_XRAY_RESULT(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string[] argGubun, cComSupXraySendInfo argCls)
        {
            DataTable dt = null;
            DataTable dt2 = null;

            string strROWID = "";
            bool bOK = false;
            bool bUpChk = false;

            #region 일반건진 HIC_XRAY_RESULT에 복사

            //전실전과 체크
            dt = sel_HL7_Hic_Result(pDbCon, argGubun);
            if (dt != null && dt.Rows.Count > 0)
            {
                #region for 영역
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bUpChk = false;                    

                    argCls.nWRTNO = 0;
                    argCls.Ptno = dt.Rows[0]["Pano"].ToString().Trim();
                    if (dt.Rows[i]["HIC_WRTNO"].ToString().Trim() != "")
                    {
                        argCls.nWRTNO = Convert.ToInt32(dt.Rows[i]["HIC_WRTNO"].ToString().Trim());
                    }
                    argCls.nHic_Pano = 0;
                    if (dt.Rows[i]["Hic_Pano"].ToString().Trim() != "")
                    {
                        argCls.nHic_Pano = Convert.ToInt32(dt.Rows[i]["Hic_Pano"].ToString().Trim());
                    }
                    argCls.JepDate = dt.Rows[i]["JepDate"].ToString().Trim();
                    argCls.strExCode = dt.Rows[i]["HIC_CODE"].ToString().Trim();
                    argCls.PacsNo = dt.Rows[i]["PacsNo"].ToString().Trim();
                    strROWID = dt.Rows[i]["ROWID"].ToString().Trim();

                    bOK = false;
                    if (dt.Rows[i]["XCode"].ToString().Trim() == "GR2101A")
                    {
                        bOK = true;
                    }

                    #region 검진 접수번호가 없으면 접수번호를 찾음
                    if (argCls.nWRTNO == 0 && argCls.nHic_Pano > 0)
                    {
                        dt2 = sel_HL7_Hic_Jepsu_Result(pDbCon, argCls.nHic_Pano, argCls.JepDate);

                        if (dt2 != null && dt2.Rows.Count > 0)
                        {
                            if (dt2.Rows[0]["WRTNO"].ToString().Trim() != "")
                            {
                                argCls.nWRTNO = Convert.ToInt32(dt2.Rows[0]["WRTNO"].ToString().Trim());
                            }
                            if (argCls.strExCode == "")
                            {
                                argCls.strExCode = dt2.Rows[0]["ExCode"].ToString().Trim();
                            }
                        }
                    }
                    #endregion
                                        
                    #region 검진 종류를 읽음
                    argCls.JepDate = "";
                    argCls.strGjJong = "";

                    dt2 = sel_HL7_Hic_Jepsu(pDbCon, argCls.nWRTNO);

                    if (dt2 != null && dt2.Rows.Count > 0)
                    {
                        argCls.strGjJong = dt2.Rows[0]["GjJong"].ToString().Trim();
                        argCls.JepDate = dt2.Rows[0]["JepDate"].ToString().Trim();
                    }
                    #endregion

                    #region 예외대상 체크
                    if (bOK == false && argCls.nWRTNO > 0 && argCls.strGjJong != "")
                    {
                        if (argCls.strGjJong == "21" || argCls.strGjJong == "22" || argCls.strGjJong == "24" || argCls.strGjJong == "27" || argCls.strGjJong == "29" || argCls.strGjJong == "30" || argCls.strGjJong == "49" || argCls.strGjJong == "32" || argCls.strGjJong == "54")
                        {
                            dt2 = sel_HL7_Hic_SunapDtl(pDbCon, argCls.nWRTNO);

                            if (dt2 != null && dt2.Rows.Count > 0)
                            {
                                bOK = true;
                            }
                        }
                        else
                        {
                            bOK = true;
                        }
                    }
                    #endregion

                    #region 분진 체크 
                    argCls.strBunjin = "1";
                    if (argCls.nWRTNO > 0)
                    {
                        dt2 = sel_HL7_Hic_SunapDtl(pDbCon, argCls.nWRTNO);

                        if (dt2 != null && dt2.Rows.Count > 0)
                        {
                            argCls.strBunjin = "2";
                        }
                    } 
                    #endregion

                    //검진 접수번호 찾지 못하였으면
                    if (argCls.nWRTNO == 0)
                    {
                        bOK = false;
                    }

                    #region 이미 HIC_XRAY_RESULT에 자료가 있는지 점검 후 컬럼 갱신
                    if (bOK == true)
                    {
                        dt2 = sel_HL7_Hic_Xray_Result(pDbCon, argCls.nHic_Pano, argCls.JepDate);

                        if (dt2 != null && dt2.Rows.Count > 0)
                        {

                            SqlErr = up_HL7_HIC_XRAY_RESULT(pDbCon, "HIC_XRAY_RESULT_01",argCls.Ptno,  argCls.PacsNo, argCls.nHic_Pano, argCls.JepDate, ref intRowAffected);
                            if (SqlErr == "")
                            {
                                SqlErr = up_HL7_XRAY_DETAIL_STS(pDbCon, "HIC_XRAY_RESULT_01", argCls.PacsNo, strROWID, ref intRowAffected);

                                if (SqlErr == "")
                                {
                                    bOK = false;
                                    bUpChk = true;
                                }
                            }

                        }
                    }
                    #endregion

                    #region 접수마스타에서 이름,성별,나이,등록번호,접수일자를 읽음
                    if (bOK == true)
                    {
                        dt2 = sel_HL7_Hic_Jepsu(pDbCon, argCls.nWRTNO);

                        if (dt2 != null && dt2.Rows.Count > 0)
                        {
                            argCls.Ptno = dt2.Rows[0]["Ptno"].ToString().Trim();
                            if (dt2.Rows[0]["Pano"].ToString().Trim() != "")
                            {
                                argCls.nHic_Pano = Convert.ToInt32(dt2.Rows[0]["Pano"].ToString().Trim());
                            }
                            argCls.SName = dt2.Rows[0]["SName"].ToString().Trim();
                            argCls.strSex = dt2.Rows[0]["Sex"].ToString().Trim();
                            argCls.strLtdCode = dt2.Rows[0]["LtdCode"].ToString().Trim();
                            argCls.strLtdName = dt2.Rows[0]["FC_LtdName"].ToString().Trim();
                            if (argCls.strLtdCode == "")
                            {
                                argCls.strLtdCode = "0000";
                                argCls.strLtdName = "획사공란";
                            }
                            argCls.nAge = 0;
                            if (dt2.Rows[0]["Age"].ToString().Trim() != "")
                            {
                                argCls.nAge = Convert.ToInt16(dt2.Rows[0]["Age"].ToString().Trim());
                            }
                            argCls.nSabun = 0;
                            if (dt2.Rows[0]["JobSabun"].ToString().Trim() != "")
                            {
                                argCls.nSabun = Convert.ToInt32(dt2.Rows[0]["JobSabun"].ToString().Trim());
                            }

                        }
                        else
                        {
                            bOK = false;
                        }
                    }
                    #endregion

                    #region HIC_XRAY_RESULT에 자료를 INSERT
                    if (bOK == true)
                    {
                        SqlErr = ins_HL7_HIC_XRAY_RESULT(pDbCon, argCls, ref intRowAffected);
                        if (SqlErr == "")
                        {
                            SqlErr = up_HL7_XRAY_DETAIL_STS(pDbCon, "HIC_XRAY_RESULT_02", "", strROWID, ref intRowAffected);
                            if (SqlErr == "")
                            {
                                //
                                SqlErr = up_HL7_HIC_JEPSU(pDbCon, "HIC_JEPSU_01", argCls.PacsNo, argCls.nWRTNO, ref intRowAffected);
                            }
                        }
                    }
                    else
                    {
                        if (bUpChk == false)
                        {
                            SqlErr = up_HL7_XRAY_DETAIL_STS(pDbCon, "HIC_XRAY_RESULT_03", "", strROWID, ref intRowAffected);
                        }
                    }
                    #endregion

                    if (SqlErr == "")
                    {
                        //argCls.PacsNo = "";
                        argCls.Pano = dt.Rows[i]["Pano"].ToString().Trim();
                        argCls.SName = dt.Rows[i]["SName"].ToString().Trim();
                        argCls.OrderCode = argCls.strExCode;
                        argCls.OrderName = "HIC_XRAY_RESULT";

                        sheet_add(Spd, argCls);
                    }

                } 
                #endregion
            }

            #endregion

            return SqlErr;

        }

        public void HL7_Send_XRAY_Result_INSERT(PsmhDb pDbCon)
        {
            DataTable dtx = null;
            DataTable dtx2 = null;
            int intRowAffected = 0;

            string strPano = "";
            string strJepDate = "";
            string strPacsNo = "";
            string strExCode = "";
            string strROWID = "";
            long nWRTNO = 0;
            long nHIC_Pano = 0;
            string strGjJong = "";
            string strBunjin = "";
            bool bHIC_Update = false;
            bool bOK = false;

            string strPTno = "";
            string strSname = "";
            string strSex = "";
            string strDate = "";
            string strLtdCode = "";
            string strLtdName = "";
            int nAge = 0;
            long nSabun = 0;


            // clsTrans DT = new clsTrans();

            dtx = sel_XrayDetail_batch(pDbCon, "4");//일반건진 HIC_XRAY_RESULT에 복사
            if (dtx != null && dtx.Rows.Count > 0)
            {
                for (int i = 0; i < dtx.Rows.Count; i++)
                {

                    strPano = dtx.Rows[i]["Pano"].ToString().Trim();
                    strJepDate = dtx.Rows[i]["JepDate"].ToString().Trim();
                    strPacsNo = dtx.Rows[i]["PacsNo"].ToString().Trim();
                    strExCode = dtx.Rows[i]["HIC_CODE"].ToString().Trim();
                    strROWID = dtx.Rows[i]["ROWID"].ToString().Trim();
                    nWRTNO = Convert.ToInt32(dtx.Rows[i]["HIC_WRTNO"].ToString().Trim());
                    nHIC_Pano = 0;
                    strGjJong = "";
                    bHIC_Update = false;

                    //검진 접수번호를 찾음
                    if (nHIC_Pano == 0)
                    {
                        dtx2 = sel_hicPatient(pDbCon, strPano);
                        if (dtx2 != null && dtx2.Rows.Count > 0)
                        {
                            nHIC_Pano = Convert.ToInt32(dtx2.Rows[0]["Pano"].ToString().Trim());
                        }
                    }

                    //검진 접수번호가 없으면 접수번호를 찾음
                    if (nWRTNO == 0 && nHIC_Pano > 0)
                    {
                        dtx2 = sel_hicJepsuResult(pDbCon, nHIC_Pano, strJepDate);
                        if (dtx2 != null && dtx2.Rows.Count > 0)
                        {
                            nWRTNO = Convert.ToInt32(dtx2.Rows[0]["WRTNO"].ToString().Trim());
                            if (strExCode == "") strExCode = dtx2.Rows[0]["WRTNO"].ToString().Trim();
                        }
                    }

                    //검진 종류를 읽음
                    strGjJong = "";
                    strJepDate = "";
                    if (nWRTNO > 0)
                    {
                        dtx2 = sel_hicJepsu(pDbCon, nWRTNO);
                        if (dtx2 != null && dtx2.Rows.Count > 0)
                        {
                            strGjJong = dtx2.Rows[0]["GjJong"].ToString().Trim();
                            strJepDate = dtx2.Rows[0]["JepDate"].ToString().Trim();
                        }
                    }

                    bOK = false;
                    if (dtx.Rows[i]["Pano"].ToString().Trim() == "GR2101A") bOK = true;

                    if (bOK == false && nWRTNO > 0 && strGjJong != "")
                    {
                        if (strGjJong == "21" && strGjJong == "22" && strGjJong == "24" && strGjJong == "27" && strGjJong == "29" && strGjJong == "30" && strGjJong == "32" && strGjJong == "49" && strGjJong == "54") //분진
                        {
                            dtx2 = sel_hicSunapDtl(pDbCon, "1", nWRTNO);
                            if (dtx2 != null && dtx2.Rows.Count > 0)
                            {
                                bOK = true;
                            }
                        }
                        else
                        {
                            bOK = true;
                        }
                    }
                    //분진여부 설정
                    strBunjin = "1";
                    if (nWRTNO > 0)
                    {
                        dtx2 = sel_hicSunapDtl(pDbCon, "1", nWRTNO);
                        if (dtx2 != null && dtx2.Rows.Count > 0)
                        {
                            strBunjin = "2";
                        }
                    }

                    //건진 접수번호를 찾지 못하였으면
                    if (nWRTNO == 0) bOK = false;

                    //이미 HIC_XRAY_RESULT에 자료가 있는지 점검
                    if (bOK == true)
                    {
                        dtx2 = sel_hicXrayResult(pDbCon, nHIC_Pano, strJepDate);
                        if (dtx2 != null && dtx2.Rows.Count > 0)
                        {

                            clsDB.setBeginTran(pDbCon);

                            SqlErr = up_hicXrayResult(pDbCon, "1", nHIC_Pano, strJepDate, strPacsNo, ref intRowAffected);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                                return;
                            }

                            SqlErr = up_XRAY_DETAIL_STS(pDbCon, "3", strROWID, ref intRowAffected);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                                return;
                            }

                            clsDB.setCommitTran(pDbCon);

                            bOK = false;
                            bHIC_Update = true;

                        }
                    }

                    //접수마스타에서 이름,성별,나이,등록번호,접수일자를 읽음
                    if (bOK == true)
                    {
                        dtx2 = sel_hicJepsu(pDbCon, nWRTNO);
                        if (dtx2 != null && dtx2.Rows.Count > 0)
                        {
                            strPTno = dtx2.Rows[0]["PTno"].ToString().Trim();
                            strSname = dtx2.Rows[0]["SName"].ToString().Trim();
                            strSex = dtx2.Rows[0]["Sex"].ToString().Trim();
                            strPano = dtx2.Rows[0]["Pano"].ToString().Trim();
                            strDate = clsPublic.GstrSysDate;
                            strLtdCode = dtx2.Rows[0]["LtdCode"].ToString().Trim();
                            strLtdName = Read_Hic_LtdName(pDbCon, strLtdCode);
                            if (strLtdCode == "") strLtdCode = "0000";
                            if (strLtdName == "") strLtdName = "회사공란";
                            nAge = Convert.ToInt16(dtx2.Rows[0]["Age"].ToString().Trim());
                            nSabun = Convert.ToInt32(dtx2.Rows[0]["JobSabun"].ToString().Trim());
                        }
                        else
                        {
                            bOK = false;
                        }

                    }

                    //HIC_XRAY_RESULT에 자료를 INSERT                    
                    if (bOK == true)
                    {
                        clsDB.setBeginTran(pDbCon);

                        SQL = " INSERT INTO " + ComNum.DB_PMPA + "HIC_XRAY_RESULT                                                           \r\n";
                        SQL += "   (JepDate, XrayNo, Pano,PTno, Sname, Sex, Age, GjJong, GbChul,                                            \r\n";
                        SQL += "   LtdCode, XCode, GbRead, GbSts, EntSabun, EntTime,                                                        \r\n";
                        SQL += "    GbOrder_Send,GbPacs,GbConv)  VALUES (                                                                   \r\n";
                        SQL += "  TO_DATE('" + strJepDate + "','YYYY-MM-DD'), '" + strPacsNo + "',                                          \r\n";
                        SQL += "  '" + strPano + "','" + strPTno + "','" + strSname + "', '" + strSex + "',                                 \r\n";
                        SQL += "  " + nAge + " ,'" + strGjJong + "','N' , '" + strLtdCode + "' ,                                            \r\n";
                        SQL += "  '" + strExCode + "' ,'" + strBunjin + "','0',                                                             \r\n";
                        SQL += "  " + nSabun + " ,                                                                                          \r\n";
                        SQL += "  TO_DATE('" + clsPublic.GstrSysDate + " " + VB.Left(clsPublic.GstrSysTime, 5) + "','YYYY-MM-DD HH24:MI'),  \r\n";
                        SQL += "   'Y','Y','Y')                                                                                             \r\n";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                            return;
                        }

                        //XRAY_DETAIL에 처리완료
                        SqlErr = up_XRAY_DETAIL_STS(pDbCon, "4", strROWID, ref intRowAffected);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                            return;
                        }

                        //HIC_JEPSU에 촬영번호세팅
                        SQL = " UPDATE " + ComNum.DB_PMPA + "HIC_JEPSU  SET         \r\n";
                        SQL += "   XRAYNO ='" + strPacsNo + "'                      \r\n";
                        SQL += "  WHERE 1=1                                         \r\n";
                        SQL += "    AND WRTNO = " + nWRTNO + "                      \r\n";
                        SQL += "    AND XRAYNO IS NULL                              \r\n";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                            return;
                        }

                        clsDB.setCommitTran(pDbCon);
                    }
                    else
                    {
                        if (bHIC_Update == false)
                        {
                            clsDB.setBeginTran(pDbCon);

                            //XRAY_DETAIL에 처리완료
                            SqlErr = up_XRAY_DETAIL_STS(pDbCon, "5", strROWID, ref intRowAffected);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                                return;
                            }

                            clsDB.setCommitTran(pDbCon);
                        }
                    }

                }
            }

        }
        
        bool HL7_PACS_SRESULT_UPDATE(PsmhDb pDbCon, string argPano,string argPacsNo,string argCONCLUSION, string argCONFDR, string argADDENDUM)
        {
            DataTable dt = null;
            cXray_ResultNew cXray_ResultNew = new cXray_ResultNew();                             

            dt = sel_HL7_Hic_Xray_Detail(pDbCon, argPano, argPacsNo);
            if (dt == null) return false;

            if (dt.Rows.Count > 0)
            {

                #region //값 설정
                cXray_ResultNew.strSeekDate = dt.Rows[0]["SeekDate"].ToString().Trim();
                cXray_ResultNew.Pano = argPano;
                cXray_ResultNew.SName = dt.Rows[0]["SName"].ToString().Trim();
                cXray_ResultNew.DeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                cXray_ResultNew.DrCode = dt.Rows[0]["DRCODE"].ToString().Trim();
                cXray_ResultNew.IpdOpd = dt.Rows[0]["IPDOPD"].ToString().Trim();
                cXray_ResultNew.WardCode = dt.Rows[0]["WARDCODE"].ToString().Trim();
                cXray_ResultNew.RoomCode = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                cXray_ResultNew.XJong = dt.Rows[0]["XJONG"].ToString().Trim();
                cXray_ResultNew.XCode = dt.Rows[0]["XCODE"].ToString().Trim();
                cXray_ResultNew.Sex = dt.Rows[0]["SEX"].ToString().Trim();
                cXray_ResultNew.Age = dt.Rows[0]["AGE"].ToString().Trim();
                cXray_ResultNew.WRTNO = 0;
                if (dt.Rows[0]["AGE"].ToString().Trim() != "")
                {
                    cXray_ResultNew.WRTNO = Convert.ToInt32(dt.Rows[0]["EXINFO"].ToString().Trim());
                }
                if (cXray_ResultNew.WRTNO < 1000)
                {
                    cXray_ResultNew.WRTNO = 0;
                }

                if (cXray_ResultNew.XJong == "4" || cXray_ResultNew.XJong == "5")
                {
                    cXray_ResultNew.XName = dt.Rows[0]["ORDERNAME"].ToString().Trim() + " " + dt.Rows[0]["REMARK"].ToString().Trim();
                }
                else
                {
                    cXray_ResultNew.XName = dt.Rows[0]["FC_XName"].ToString().Trim();
                } 
                #endregion

            }
            else
            {
                return false;
            }

            if (dt !=null)
            {
                dt.Dispose();
                dt = null;
            }
            
            #region //결과값 2000 자르기
            int nMaxLen = 2000; //결과값을 2000자 분리 처리(한글 byte 처리됨)

            string s = ComFunc.QuotConv(VB.RTrim((argCONCLUSION)));

            int intLenTot = (int)ComFunc.GetWordByByte(s);

            if (intLenTot <= nMaxLen)
            {
                cXray_ResultNew.Result = s;
            }
            else if (intLenTot > nMaxLen)
            {
                cXray_ResultNew.Result = ComFunc.GetMidStr(s, 0, nMaxLen);
                cXray_ResultNew.Result1 = ComFunc.GetMidStr(s, nMaxLen, intLenTot - nMaxLen);
            }
            #endregion

            cXray_ResultNew.XName = ComFunc.QuotConv(cXray_ResultNew.XName);

            if (VB.IsNumeric(argCONFDR) ==false)
            {
                cXray_ResultNew.XDrCode1 = 1000;
            }
            else
            {
                cXray_ResultNew.XDrCode1 = Convert.ToInt32(argCONFDR);
            }

            if (cXray_ResultNew.WRTNO <= 1000)
            {
                SqlErr = ins_HL7_XRAY_RESULTNEW(pDbCon, cXray_ResultNew, ref intRowAffected);
            }
            else
            {
                SqlErr = up_HL7_XRAY_RESULTNEW(pDbCon, cXray_ResultNew, ref intRowAffected);
            }
            if (SqlErr != "")
            {
                return false;
            }            

            return true;
        }

        string up_HL7_ipdNewMst(PsmhDb pDbCon, string Job, string strROWID, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_NEW_MASTER SET    \r\n";
            if (Job == "II")
            {
                SQL += "     PACS_ADT ='II'                             \r\n";//입원자
            }
            else if (Job == "TT")
            {
                SQL += "     PACS_ADT ='TT'                             \r\n";//퇴원자
            }
            else if (Job == "IC")
            {
                SQL += "     PACS_ADT ='IC'                             \r\n";//입원취소
            }
            else if (Job == "TC")
            {
                SQL += "     PACS_ADT ='TC'                             \r\n";//퇴원취소
            }
            else if (Job == "*")
            {
                SQL += "     PACS_ADT ='*'                              \r\n"; //전실전과
            }
            SQL += "  WHERE 1=1                                         \r\n";
            SQL += "    AND ROWID = '" + strROWID + "'                  \r\n";


            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        string up_HL7_ipdTransfor(PsmhDb pDbCon, string Job, string strROWID, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            if (Job == "")
            {
                return "오류";
            }

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANSFOR SET    \r\n";
            if (Job == "*")
            {
                SQL += "     PACS_ADT ='*'                              \r\n"; //전실전과
            }
            SQL += "  WHERE 1=1                                         \r\n";
            SQL += "    AND ROWID = '" + strROWID + "'                  \r\n";


            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_HL7_XRAY_DETAIL_STS(PsmhDb pDbCon, string Job, string argPacsNo, string argROWID, ref int intRowAffected)
        {
            string SqlErr = string.Empty;
            if (Job == "")
            {
                return "오류";
            }

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_DETAIL SET   \r\n";
            if (Job == "00")
            {
                SQL += "     PacsNo ='" + argPacsNo + "'            \r\n";
                SQL += "     ,GbReserved ='7'                       \r\n";
                SQL += "     ,XSENDDATE =  SYSDATE                  \r\n";
                SQL += "     ,UPPS ='HL7'                           \r\n";
                SQL += "     ,UP_DT =  SYSDATE                      \r\n";
            }
            else if (Job == "03")
            {
                SQL += "     PacsNo ='" + argPacsNo + "'            \r\n";
                SQL += "     ,GbReserved ='7'                       \r\n";
                SQL += "     ,GbEnd ='1'                            \r\n";
                SQL += "     ,XSENDDATE =  SYSDATE                  \r\n";
                SQL += "     ,UPPS ='HL7'                           \r\n";
                SQL += "     ,UP_DT =  SYSDATE                      \r\n";
            }
            else if (Job == "06_01" || Job == "06_02")
            {                
                SQL += "     GBSTS ='7'                             \r\n";                
                SQL += "     ,CDATE =  SYSDATE                      \r\n";
                SQL += "     ,UPPS ='HL7'                           \r\n";
                SQL += "     ,UP_DT =  SYSDATE                      \r\n";
            }
            else if (Job == "06_03")
            {
                SQL += "     GbEnd ='1'                             \r\n";                
                SQL += "     ,UPPS ='HL7'                           \r\n";
                SQL += "     ,UP_DT =  SYSDATE                      \r\n";
            }
            else if (Job == "HIC_XRAY_RESULT_01")
            {
                SQL += "     GbHIC ='Y'                             \r\n";
                SQL += "     ,GbSTS ='7'                            \r\n";
                SQL += "     ,UPPS ='HL7'                           \r\n";
                SQL += "     ,UP_DT =  SYSDATE                      \r\n";
            }
            else if (Job == "HIC_XRAY_RESULT_02")
            {
                SQL += "     GbHIC ='Y'                             \r\n";
                SQL += "     ,GbSTS ='7'                            \r\n";
                SQL += "     ,CDate =  SYSDATE                      \r\n";
                SQL += "     ,UPPS ='HL7'                           \r\n";
                SQL += "     ,UP_DT =  SYSDATE                      \r\n";
            }
            else if (Job == "HIC_XRAY_RESULT_03")
            {
                SQL += "     GbHIC ='N'                             \r\n";                
                SQL += "     ,UPPS ='HL7'                           \r\n";
                SQL += "     ,UP_DT =  SYSDATE                      \r\n";
            }
            SQL += "  WHERE 1=1                                     \r\n";
            SQL += "    AND ROWID = '" + argROWID + "'              \r\n";


            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        string up_HL7_XRAY_DETAIL(PsmhDb pDbCon, string Job, string argPano, string argPacsNo,  string argXrayName, string argRemark, string argRead, string argROWID, ref int intRowAffected)
        {
            string SqlErr = string.Empty;
            if (Job =="")
            {
                return "job null";
            }
            if (Job =="01" && argPacsNo =="")
            {
                return "pacsno";
            }

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_DETAIL SET                   \r\n";
            if (Job =="01")
            {
                SQL += "        SENDDATE = SYSDATE                                  \r\n";
                SQL += "        ,OrderName = '" + argXrayName + "'                  \r\n";
                SQL += "        ,Remark = '" + argRemark + "'                       \r\n";
            }
            else if (Job =="PACSEND_SET_01")
            {
                SQL += "        XSENDDATE = SYSDATE                                 \r\n";
                SQL += "        ,CDate = SYSDATE                                    \r\n";
                SQL += "        ,PacsStudyID = '" + argPacsNo + "'                  \r\n";
                SQL += "        ,GbRead = '" + argRead + "'                         \r\n";
                SQL += "        ,PACS_END ='Y'                                      \r\n";
                SQL += "        ,GbEnd ='1'                                         \r\n";
                SQL += "        ,MgrNo =''                                          \r\n";
                SQL += "        ,GbSTS ='7'                                         \r\n";                                
            }
            else if (Job == "PACSEND_SET_02")
            {
                SQL += "        XSENDDATE = SYSDATE                                 \r\n";
                SQL += "        ,CDate = SYSDATE                                    \r\n";                
                SQL += "        ,PacsStudyID = '" + argPacsNo + "'                  \r\n";
                SQL += "        ,GbRead = '" + argRead + "'                         \r\n";
                SQL += "        ,PACS_END ='Y'                                      \r\n";                               
                SQL += "        ,GbSTS ='7'                                         \r\n";
                SQL += "        ,cSabun = 999                                       \r\n";
                SQL += "        ,gbreserved = '7'                                   \r\n";                
            }
            else if (Job == "PACSEND_SET_03")
            {
                SQL += "        XSENDDATE = ''                                      \r\n";
                SQL += "        ,CDate = ''                                         \r\n";
                SQL += "        ,PacsStudyID = ''                                   \r\n";                
                SQL += "        ,PACS_END =''                                       \r\n";
                SQL += "        ,GbEND =''                                          \r\n";
                SQL += "        ,GbSTS ='0'                                         \r\n";                
            }
            else
            {
                SQL += "        XSENDDATE = SYSDATE                                 \r\n";
            }
            SQL += "  WHERE 1=1                                                     \r\n";
            if (Job == "01")
            {
                SQL += "    AND Pano = '" + argPano + "'                            \r\n";
                SQL += "    AND PacsNo = '" + argPacsNo + "'                        \r\n";
            }
            else if (Job =="PACSEND_SET_01" || Job == "PACSEND_SET_02")
            {
                SQL += "    AND ROWID = '" + argROWID + "'                          \r\n";
                SQL += "    AND PacsStudyID IS NULL                                 \r\n";
            }
            else if (Job == "PACSEND_SET_03")
            {
                SQL += "    AND ROWID = '" + argROWID + "'                          \r\n";
                SQL += "    AND PacsStudyID IS NOT NULL                             \r\n";
            }
            else
            {
                SQL += "    AND ROWID = '" + argROWID + "'                          \r\n";
                SQL += "    AND Pano = '" + argPano + "'                            \r\n";
            }

            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        string ins_HL7_XRAY_PACSSEND(PsmhDb pDbCon, string argJob, string argROWID, string argOperator, ref int intRowAffected)
        {
            string SqlErr = string.Empty;
            if (argJob == "")
            {
                return "오류";
            }

            SQL = "";
            if (argJob == "00" || argJob == "03")
            {
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_PACSSEND  (                     \r\n";
                SQL += "    EntDate,PacsNo,SendGbn,Pano,SName                                   \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark,Operator,Gbinfo   \r\n";
                SQL += "   ,INPS,INPT_DT )                                                      \r\n";
                SQL += "  SELECT SYSDATE,PacsNo,'1',Pano,SName                                  \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark                   \r\n";
                SQL += "   ,'" + argOperator + "',Gbinfo,'HL7',SYSDATE                          \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                \r\n";
                SQL += "   WHERE 1=1                                                            \r\n";
                SQL += "    AND ROWID = '" + argROWID + "'                                      \r\n";

            }
            else if (argJob == "PACSEND_SET")
            {
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_PACSSEND  (                     \r\n";
                SQL += "    EntDate,PacsNo,SendGbn,Pano,SName                                   \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark,Gbinfo            \r\n";
                SQL += "   ,INPS,INPT_DT )                                                      \r\n";
                SQL += "  SELECT SYSDATE,PacsNo,'1',Pano,SName                                  \r\n";
                SQL += "   ,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode     \r\n";
                SQL += "   ,XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark                   \r\n";
                SQL += "   ,Gbinfo,'HL7',SYSDATE                                                \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                \r\n";
                SQL += "   WHERE 1=1                                                            \r\n";
                SQL += "    AND ROWID = '" + argROWID + "'                                      \r\n";
                SQL += "    AND PacsNo IS NOT NULL                                              \r\n";
                SQL += "    AND PacsStudyID IS NULL                                             \r\n";

            }

            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        string up_HL7_XRAY_PACSSEND(PsmhDb pDbCon, string Job, string argROWID,  string argXrayName, ref int intRowAffected)
        {
            string SqlErr = string.Empty;    

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_PACSSEND SET             \r\n";
            SQL += "        SendTime = SYSDATE                                  \r\n";
            SQL += "        ,XRayName = '" + argXrayName + "'                   \r\n";
            SQL += "  WHERE 1=1                                                 \r\n";            
            SQL += "    AND ROWID = '" + argROWID + "'                          \r\n";            

            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        string up_HL7_ENDO_JUPMST(PsmhDb pDbCon, string Job, string argROWID, string argPacsNo, string argUID, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            if (Job != "UP" && Job !="CAN" && Job != "UP3_01" && Job != "UP3_02")
            {
                return "ENDO_JUPMST error";
            }

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_MED + "ENDO_JUPMST SET                \r\n";
            if (Job =="CAN")
            {
                SQL += "        XSENDDATE = ''                                  \r\n";
                SQL += "        ,PacsSend = ''                                  \r\n";
                SQL += "        ,PacsNo = ''                                    \r\n";
            }
            else if (Job == "UP")
            {
                SQL += "        XSENDDATE = SYSDATE                             \r\n";
                SQL += "        ,PacsSend = 'Y'                                 \r\n";
                SQL += "        ,PacsNo = '" + argPacsNo + "'                   \r\n";
            }
            else if (Job == "UP2")
            {
                SQL += "        XSENDDATE = SYSDATE                             \r\n";
                SQL += "        ,ResultSend = 'Y'                               \r\n";                
            }
            else if (Job == "UP3_01")
            {
                SQL += "        XSENDDATE = SYSDATE                             \r\n";
                SQL += "        ,PacsUID = '" + argUID + "'                     \r\n";
            }
            else if (Job == "UP3_02")
            {
                SQL += "        XSENDDATE = ''                                  \r\n";
                SQL += "        ,PacsUID = ''                                   \r\n";
            }
            SQL += "  WHERE 1=1                                                 \r\n";
            SQL += "    AND ROWID = '" + argROWID + "'                          \r\n";
            if (Job == "UP3_01")
            {
                SQL += "    AND PacsUID IS NULL                                 \r\n";
            }
            else if (Job == "UP3_02")
            {
                SQL += "    AND PacsUID IS NOT NULL                             \r\n";
            }

            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_HL7_HIC_XRAY_RESULT(PsmhDb pDbCon, string Job, string argPtno, string argPacsNo, long argHPano, string argJepDate, ref int intRowAffected)
        {
            string SqlErr = string.Empty;
            if (Job == "")
            {
                return "오류";
            }

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "HIC_XRAY_RESULT SET                   \r\n";
            if (Job == "HIC_XRAY_RESULT_01")
            {
                SQL += "     XRAYNO ='" + argPacsNo + "'                                \r\n";
                SQL += "     ,GbPACS ='Y'                                               \r\n";                
                SQL += "     ,UPPS ='HL7'                                               \r\n";
                SQL += "     ,UP_DT =  SYSDATE                                          \r\n";
            }
            else if (Job == "HIC_XRAY_RESULT_02")
            {                
                SQL += "     ,GbPACS ='Y'                                               \r\n";
                SQL += "     ,UPPS ='HL7'                                               \r\n";
                SQL += "     ,UP_DT =  SYSDATE                                          \r\n";
            }
            else if (Job == "HIC_XRAY_RESULT_03")
            {
                SQL += "     ,GbPACS ='Y'                                               \r\n";
                SQL += "     ,UPPS ='HL7'                                               \r\n";
                SQL += "     ,UP_DT =  SYSDATE                                          \r\n";
            }
            else
            {
                SQL += "     ,UPPS ='HL7'                                               \r\n";
                SQL += "     ,UP_DT =  SYSDATE                                          \r\n";
            }
            
            SQL += "  WHERE 1=1                                                         \r\n";
            if (Job == "HIC_XRAY_RESULT_01")
            {
                SQL += "    AND Pano = " + argHPano + "                                 \r\n";
                SQL += "    AND JepDate = TO_DATE('" + argJepDate + "','YYYY-MM-DD')    \r\n";
                SQL += "    AND DelDate IS NULL                                         \r\n";
                SQL += "    AND GbPACS  IS NULL                                         \r\n";
            }
            else if (Job == "HIC_XRAY_RESULT_02")
            {
                SQL += "    AND Pano = " + argHPano + "                                 \r\n";
                SQL += "    AND XRayNo = '" + argPacsNo + "'                            \r\n";
            }
            else if (Job == "HIC_XRAY_RESULT_03")
            {
                SQL += "    AND Ptno = '" + argPtno + "'                                 \r\n";
                SQL += "    AND XRayNo = '" + argPacsNo + "'                            \r\n";
            }
            else
            {
                SQL += "    AND XRAYNO ='" + argPacsNo + "'                             \r\n";
            }
                

            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_HL7_HIC_XRAY_RESULT(PsmhDb pDbCon, cComSupXraySendInfo argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";

            SQL = " INSERT INTO " + ComNum.DB_PMPA + "HIC_XRAY_RESULT                               \r\n";
            SQL += "  (JepDate, XrayNo, Pano,PTno, Sname, Sex, Age, GjJong, GbChul ,LtdCode         \r\n";
            SQL += "   ,XCode, GbRead, GbSts, EntSabun, EntTime                                     \r\n";
            SQL += "   ,GbOrder_Send,GbPacs,GbConv,UpPs,Up_Dt) VALUES                               \r\n";
            SQL += "  ( TO_DATE('" + argCls.JepDate + "','YYYY-MM-DD')                              \r\n";
            SQL += "  ,'" + argCls.PacsNo + "'                                                      \r\n";
            SQL += "  ,'" + argCls.nHic_Pano + "'                                                   \r\n";
            SQL += "  ,'" + argCls.Ptno + "'                                                        \r\n";
            SQL += "  ,'" + argCls.SName + "'                                                       \r\n";
            SQL += "  ,'" + argCls.strSex + "'                                                      \r\n";
            SQL += "  ," + argCls.nAge + "                                                          \r\n";
            SQL += "  ,'" + argCls.strGjJong + "'                                                   \r\n";
            SQL += "  ,'N'                                                                          \r\n";
            SQL += "  ,'" + argCls.strLtdCode + "'                                                  \r\n";
            SQL += "  ,'" + argCls.strExCode + "'                                                   \r\n";
            SQL += "  ,'" + argCls.strBunjin + "'                                                   \r\n";
            SQL += "  ,'0'                                                                          \r\n";
            SQL += "  ," + argCls.nSabun + "                                                        \r\n";
            SQL += "  ,SYSDATE,'Y','Y','Y','HL7',SYSDATE                                            \r\n";
            SQL += "  )                                                                             \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_HL7_HIC_JEPSU(PsmhDb pDbCon, string Job, string argPacsNo, long argWRTNO, ref int intRowAffected)
        {
            string SqlErr = string.Empty;
            if (Job == "")
            {
                return "오류";
            }

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "HIC_JEPSU SET     \r\n";
            if (Job == "HIC_JEPSU_01")
            {
                SQL += "     XRAYNO ='" + argPacsNo + "'            \r\n";                
            }
            
            SQL += "  WHERE 1=1                                     \r\n";
            SQL += "    AND WRTNO = " + argWRTNO + "                \r\n";
            if (Job == "HIC_JEPSU_01")
            {
                SQL += "    AND XRAYNO IS NULL                      \r\n";
            }            

            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string HL7_read_Xray_DISEASE_set(PsmhDb pDbCon, cXrayPacsSend argHL7)
        {
            DataTable dt = null;            
            
            string strTemp = string.Empty;

            #region 처방일자체크
            if (argHL7.BDate == "")
            {
                dt = sel_XrayDetail(pDbCon, argHL7.Pano, argHL7.PacsNo);
                if (dt != null && dt.Rows.Count > 0)
                {
                    argHL7.BDate = dt.Rows[0]["BDate"].ToString().Trim();
                }
            } 
            #endregion

            if (argHL7.IpdOpd == "O")
            {
                #region 외래상병체크
                dt = null;
                dt = sel_HL7_Ocsills(pDbCon, argHL7);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == 2) break;

                        strTemp += "[" + dt.Rows[i]["iLLCode"].ToString().Trim() + " " + dt.Rows[i]["iLLNameK"].ToString().Trim() + "]";
                    }
                } 
                #endregion

                return ComFunc.QuotConv(strTemp);
            }
            else
            {
                #region 입원상병체크
                dt = null;
                dt = sel_HL7_ipdNewMst(pDbCon, "1", null, argHL7.Pano);
                if (dt != null && dt.Rows.Count > 0)
                {
                    argHL7.InDate = dt.Rows[0]["INDATE"].ToString().Trim();
                }

                dt = null;
                dt = sel_HL7_Ocsills(pDbCon, argHL7);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == 2) break;

                        strTemp += "[" + dt.Rows[i]["iLLCode"].ToString().Trim() + " " + dt.Rows[i]["iLLNameK"].ToString().Trim() + "]";
                    }
                } 
                #endregion

                return ComFunc.QuotConv(strTemp);
            }

        }

        string ins_HL7_XRAY_RESULTNEW(PsmhDb pDbCon, cXray_ResultNew argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";

            SQL = " INSERT INTO " + ComNum.DB_PMPA + "XRAY_RESULTNEW                                        \r\n";
            SQL += "  (WRTNO,Pano,ReadDate,ReadTime,SeekDate,XJong,SName,Sex,Age                            \r\n";
            SQL += "   ,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XDrCode1,XDrCode2,XDrCode3                 \r\n";
            SQL += "   ,IllCode1,IllCode2,IllCode3,XCode,XName,Result,Result1,EntDate                       \r\n";
            SQL += "   ,Approve, STime, ETime,inps, inpt_dt) VALUES                                         \r\n";
            SQL += "  ( " + argCls.WRTNO + "                                                                \r\n";
            SQL += "  ,'" + argCls.Pano + "'                                                                \r\n";
            SQL += "  ,TO_DATE('" + argCls.ReadDate + "','YYYY-MM-DD')                                      \r\n";
            SQL += "  ,TO_DATE('" + argCls.ReadDate + " " + argCls.ReadTime +"','YYYY-MM-DD HH24:MI')       \r\n";
            SQL += "  ,TO_DATE('" + argCls.SeekDate + "','YYYY-MM-DD')                                      \r\n";
            SQL += "  ,'" + argCls.XJong + "'                                                               \r\n";
            SQL += "  ,'" + argCls.SName + "'                                                               \r\n";
            SQL += "  ,'" + argCls.Sex + "'                                                                 \r\n";
            SQL += "  ,'" + argCls.Age + "'                                                                 \r\n";
            SQL += "  ,'" + argCls.IpdOpd + "'                                                              \r\n";
            SQL += "  ,'" + argCls.DeptCode + "'                                                            \r\n";
            SQL += "  ,'" + argCls.DrCode + "'                                                              \r\n";
            SQL += "  ,'" + argCls.WardCode + "'                                                            \r\n";
            SQL += "  ,'" + argCls.RoomCode + "'                                                            \r\n";
            SQL += "  ," + argCls.XDrCode1 + "                                                              \r\n";
            SQL += "  ," + argCls.XDrCode2 + "                                                              \r\n";
            SQL += "  ," + argCls.XDrCode3 + "                                                              \r\n";
            SQL += "  ,'" + argCls.IllCode1 + "'                                                            \r\n";
            SQL += "  ,'" + argCls.IllCode2 + "'                                                            \r\n";
            SQL += "  ,'" + argCls.IllCode3 + "'                                                            \r\n";
            SQL += "  ,'" + argCls.XCode + "'                                                               \r\n";
            SQL += "  ,'" + argCls.XName + "'                                                               \r\n";
            SQL += "  ,'" + argCls.Result + "'                                                              \r\n";
            SQL += "  ,'" + argCls.Result1 + "'                                                             \r\n";
            SQL += "  ,SYSDATE                                                                              \r\n";
            SQL += "  ,'Y'                                                                                  \r\n";
            SQL += "  ,TO_DATE('" + argCls.STime +"','YYYY-MM-DD HH24:MI')                                  \r\n";
            SQL += "  ,SYSDATE                                                                              \r\n";
            SQL += "  ,123                                                                                  \r\n";
            SQL += "  ,SYSDATE                                                                              \r\n";
            SQL += "  )                                                                                     \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        string up_HL7_XRAY_RESULTNEW(PsmhDb pDbCon, cXray_ResultNew argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";

            SQL = " UPDATE " + ComNum.DB_PMPA + "XRAY_RESULTNEW  SET                                                        \r\n";
            SQL += "   XDrCode2 = " + argCls.XDrCode2 + "                                                                   \r\n";
            SQL += "  ,XDrCode3 = " + argCls.XDrCode3 + "                                                                   \r\n";
            SQL += "  ,IllCode1 = '" + argCls.IllCode1 + "'                                                                 \r\n";
            SQL += "  ,IllCode2 = '" + argCls.IllCode2 + "'                                                                 \r\n";
            SQL += "  ,IllCode3 = '" + argCls.IllCode3 + "'                                                                 \r\n";
            SQL += "  ,XCode = '" + argCls.XCode + "'                                                                       \r\n";
            SQL += "  ,XName = '" + argCls.XName + "'                                                                       \r\n";
            if (argCls.ADDENDUM == "A")
            {
                SQL += "  ,ADDENDUM1 = '" + argCls.Result + "'                                                              \r\n";
                SQL += "  ,ADDENDUM2 = '" + argCls.Result1 + "'                                                             \r\n";
                SQL += "  ,ADDDATE = SYSDATE                                                                                \r\n";
                SQL += "  ,ADDDrCode = " + argCls.XDrCode1 + "                                                              \r\n";
            }
            else
            {
                SQL += "  ,ReadDate = TO_DATE('" + argCls.ReadDate + "','YYYY-MM-DD')                                       \r\n";
                SQL += "  ,ReadTime = TO_DATE('" + argCls.ReadDate + " " + argCls.ReadTime + "','YYYY-MM-DD HH24:MI')       \r\n";
                SQL += "  ,Result = '" + argCls.Result + "'                                                                 \r\n";
                SQL += "  ,Result1 = '" + argCls.Result1 + "'                                                               \r\n";
            }
            SQL += "  ,Approve = 'Y'                                                                                        \r\n";
            SQL += "  ,EntDate = SYSDATE                                                                                    \r\n";
            SQL += "  ,STime = TO_DATE('" + argCls.STime + "','YYYY-MM-DD HH24:MI')                                         \r\n";            
            SQL += "  ,ETIME = SYSYDATE                                                                                     \r\n";
            SQL += "  ,SENDEMR = ''                                                                                         \r\n";
            SQL += " WHERE 1=1                                                                                              \r\n";
            SQL += "    AND WRTNO = " + argCls.WRTNO + "                                                                    \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_HL7_XRAY_PACS_SREPORT(PsmhDb pDbCon, string argROWID, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL = " UPDATE " + ComNum.DB_PACS + "XRAY_PACS_SREPORT  SET     \r\n";
            SQL += "   FLAG = 'Y'                                           \r\n";
            SQL += " WHERE 1=1                                              \r\n";
            SQL += "    AND ROWID = " + argROWID + "                        \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }


        #region //방사선,내시경 HL7 Order SEND 작업중??
        //방사선 HL7 Order SEND
        //public bool Send_Xray(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread spd)
        //{

        //    DataTable dt = null;
        //    DataTable dt2 = null;
        //    int intRowAffected = 0; //변경된 Row 받는 변수                       

        //    //쿼리실행      
        //    dt = sel_XrayPacsSend(pDbCon);

        //    TPS = null;

        //    #region //데이터셋 읽은후 작업

        //    if (dt == null) return false;


        //    if (dt.Rows.Count > 0)
        //    {

        //        // clsTrans DT = new clsTrans();
        //        clsDB.setBeginTran(pDbCon);

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {

        //            #region class 초기화 및 변수세팅
        //            //class 초기화
        //            TPS = new cXrayPacsSend();

        //            clsComSupXraySend.GstrPacsSend = "";//상태구분자

        //            TPS.EntDate = dt.Rows[i]["EntDate"].ToString().Trim();
        //            TPS.PacsNo = dt.Rows[i]["PacsNo"].ToString().Trim();
        //            TPS.SendGbn = dt.Rows[i]["SendGbn"].ToString().Trim();
        //            TPS.Pano = dt.Rows[i]["Pano"].ToString().Trim();
        //            TPS.SName = dt.Rows[i]["SName"].ToString().Trim();
        //            TPS.EName = dt.Rows[i]["EName"].ToString().Trim();
        //            TPS.EName2 = dt.Rows[i]["EName2"].ToString().Trim();
        //            TPS.Sex = dt.Rows[i]["Sex"].ToString().Trim();
        //            TPS.Age = dt.Rows[i]["Age"].ToString().Trim();
        //            TPS.DeptCode = dt.Rows[i]["DeptCode"].ToString().Trim();
        //            TPS.DrCode = dt.Rows[i]["DrCode"].ToString().Trim();
        //            TPS.IpdOpd = dt.Rows[i]["IpdOpd"].ToString().Trim();
        //            TPS.WardCode = dt.Rows[i]["WardCode"].ToString().Trim();
        //            TPS.RoomCode = dt.Rows[i]["RoomCode"].ToString().Trim();
        //            TPS.OrderCode = dt.Rows[i]["OrderCode"].ToString().Trim();
        //            TPS.OrderName = dt.Rows[i]["XRayName"].ToString().Trim();
        //            TPS.SeekDate = dt.Rows[i]["SeekDate"].ToString().Trim();
        //            TPS.Remark = EDIT_OrderName(dt.Rows[i]["Remark"].ToString().Trim());
        //            TPS.XrayRoom = dt.Rows[i]["XRayRoom"].ToString().Trim();
        //            TPS.XJong = dt.Rows[i]["XJong"].ToString().Trim();
        //            TPS.XCode = dt.Rows[i]["XCode"].ToString().Trim();
        //            TPS.ReadNo = dt.Rows[i]["ReadNo"].ToString().Trim();
        //            TPS.DrRemark = dt.Rows[i]["DrRemark"].ToString().Trim();
        //            TPS.ROWID = dt.Rows[i]["ROWID"].ToString().Trim();
        //            TPS.MsgControlid = clsComSupXraySend.gstrTime + ComFunc.SetAutoZero(i.ToString(), 2);
        //            TPS.Modality = "";
        //            TPS.Resource = "";
        //            TPS.Operator = dt.Rows[i]["operator"].ToString().Trim();
        //            TPS.Disease = read_Xray_DISEASE_set(pDbCon, "", "");
        //            TPS.PACS_Code = "";

        //            TPS.Result = ""; //판독결과값
        //            TPS.ResultDate = "";//판독결과일자


        //            dt2 = sel_XrayDetail(pDbCon, TPS.Pano, TPS.PacsNo);
        //            if (dt2.Rows.Count > 0)
        //            {
        //                if (dt2.Rows[0]["GbRead"].ToString().Trim() == "Y") TPS.Emergency = "Y";
        //            }

        //            #endregion

        //            //전송정보 체크 및 정리후 팍스에 넘김
        //            HL7_Send(pDbCon);

        //            if (clsComSupXraySend.GstrPacsSend == "OK")
        //            {

        //                //갱신
        //                SQL = " UPDATE " + ComNum.DB_PMPA + "XRAY_PACSSEND  SET     \r\n";
        //                SQL += "    XRayName='" + TPS.OrderName.Trim() + "'         \r\n";
        //                SQL += "   ,SendTime = SYSDATE                              \r\n";
        //                SQL += " WHERE 1=1                                          \r\n";
        //                SQL += "  AND ROWID='" + TPS.ROWID + "'                     \r\n";

        //                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
        //                if (SqlErr != "")
        //                {
        //                    clsDB.setRollbackTran(pDbCon);
        //                    ComFunc.MsgBox(SqlErr);
        //                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
        //                    return false;
        //                }

        //                //갱신
        //                if (TPS.PacsNo.Trim() != "")
        //                {
        //                    SQL = " UPDATE " + ComNum.DB_PMPA + "XRAY_DETAIL  SET   \r\n";
        //                    SQL += "    OrderName='" + TPS.OrderName.Trim() + "'    \r\n";
        //                    SQL += "    Remark='" + TPS.Remark.Trim() + "'          \r\n";
        //                    SQL += "   ,SENDDATE = SYSDATE                          \r\n";
        //                    SQL += " WHERE 1=1                                      \r\n";
        //                    SQL += "  AND Pano='" + TPS.Pano + "'                   \r\n";
        //                    SQL += "  AND PacsNo='" + TPS.PacsNo + "'               \r\n";

        //                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
        //                    if (SqlErr != "")
        //                    {
        //                        clsDB.setRollbackTran(pDbCon);
        //                        ComFunc.MsgBox(SqlErr);
        //                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
        //                        return false;
        //                    }
        //                }

        //                //시트표시
        //                spd.ActiveSheet.AddRows(0, 1);
        //                if (spd.ActiveSheet.RowCount > 500) spd.ActiveSheet.RowCount = 500;

        //                spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmSpdXSend.SendTime].Text = VB.Mid(TPS.MsgControlid, 3, 6) + "-" + VB.Mid(TPS.MsgControlid, 9, 4);
        //                spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmSpdXSend.PacsNo].Text = TPS.PacsNo;
        //                spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmSpdXSend.Pano].Text = TPS.Pano;
        //                spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmSpdXSend.SName].Text = TPS.SName;
        //                spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmSpdXSend.SendGbn].Text = TPS.SendGbn;
        //                spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmSpdXSend.OrderCode].Text = TPS.OrderCode;
        //                spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmSpdXSend.OrderName].Text = TPS.Remark != "" ? (TPS.OrderName + "^" + TPS.Remark) : TPS.OrderName;


        //            }

        //        }

        //        clsDB.setCommitTran(pDbCon);
        //    }

        //    dt.Dispose();
        //    dt = null;

        //    #endregion


        //    return true;

        //}

        //방사선 HL7 Order 쿼리
        //DataTable sel_XrayPacsSend(PsmhDb pDbCon)
        //{
        //    DataTable dt = null;

        //    SQL = "";
        //    SQL += "SELECT                                                                                                  \r\n";
        //    SQL += "  TO_CHAR(a.ENTDATE,'YYYYMMDDHH24MI') EntDate,a.PacsNo,a.SendGbn,                                       \r\n";
        //    SQL += "  a.Pano,a.SName,a.EName,b.EName EName2,a.Sex,a.Age,a.DeptCode,a.DrCode,a.IPDOPD,a.WardCode,a.RoomCode, \r\n";
        //    SQL += "  a.XJONG,a.XSUBCODE,a.XCODE,a.OrderCode,a.XRayName,a.operator,                                         \r\n";
        //    SQL += "  TO_CHAR(a.SeekDate,'YYYYMMDDHH24MI') SeekDate,                                                        \r\n";
        //    SQL += "  a.Remark,a.XrayRoom,a.ReadNo,a.DrRemark,a.ROWID                                                       \r\n";
        //    SQL += " FROM " + ComNum.DB_PMPA + "XRAY_PACSSEND a, " + ComNum.DB_PMPA + "BAS_PATIENT b                        \r\n";
        //    SQL += "  WHERE 1=1                                                                                             \r\n";
        //    SQL += "   AND a.Pano=b.Pano(+)                                                                                 \r\n";
        //    SQL += "   AND a.SendTime IS NULL                                                                               \r\n";
        //    SQL += "   AND a.EntDate>=TRUNC(SYSDATE-2)                                                                      \r\n";
        //    SQL += "   AND ROWNUM <= 40                                                                                     \r\n";
        //    SQL += "  ORDER BY a.EntDate,a.PacsNo                                                                           \r\n";

        //    try
        //    {
        //        SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

        //        if (SqlErr != "")
        //        {
        //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
        //            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
        //            return null;
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
        //        return null;
        //    }

        //    return dt;

        //}

        /// <summary>
        /// 내시경 팍스전송
        /// </summary>
        /// <param name="spd"></param>
        /// <param name="bResult"></param>
        /// <returns></returns>
        //내시경 HL7 Order SEND
        //public bool Send_Endo(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread spd, bool bResult = false)
        //{

        //    DataTable dt = null;
        //    int intRowAffected = 0; //변경된 Row 받는 변수                       

        //    //쿼리실행      
        //    dt = sel_EndoPacsSend(pDbCon, bResult);

        //    TPS = null;

        //    #region //데이터셋 읽은후 작업

        //    if (dt == null) return false;


        //    if (dt.Rows.Count > 0)
        //    {

        //        // clsTrans DT = new clsTrans();
        //        clsDB.setBeginTran(pDbCon);

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {

        //            #region class 초기화 및 변수세팅
        //            //class 초기화
        //            TPS = new cXrayPacsSend();

        //            clsComSupXraySend.GstrPacsSend = "";//상태구분자

        //            if (bResult == true)
        //            {
        //                TPS.SendGbn = "5"; //판독

        //                TPS.EntDate = dt.Rows[i]["ResultDate"].ToString().Trim();
        //                TPS.MsgControlid = clsComSupXraySend.gstrTime + ComFunc.SetAutoZero((i + 85).ToString(), 2);
        //            }
        //            else
        //            {
        //                TPS.SendGbn = "3"; //기본은 수정    
        //                TPS.PacsNo = dt.Rows[i]["PacsNo"].ToString().Trim();
        //                if (TPS.PacsNo.Trim() == "")
        //                {
        //                    //팍스넘버 날짜(YYYYMMDD+0000)
        //                    TPS.PacsNo = clsPublic.GstrSysDate.Replace("-", "").Trim() + ComFunc.SetAutoZero(ComQuery.GetSequencesNo(pDbCon, "KOSMOS_PMPA", "SEQ_PACSNO").ToString(), 4);
        //                }

        //                if (dt.Rows[i]["GbSunap"].ToString().Trim() == "*") TPS.SendGbn = "2"; //접수취소건

        //                TPS.EntDate = dt.Rows[i]["EntDate"].ToString().Trim();
        //                TPS.MsgControlid = clsComSupXraySend.gstrTime + ComFunc.SetAutoZero(i.ToString(), 2);
        //            }

        //            TPS.EName = "";
        //            TPS.Age = "0";//뒤에서 처리
        //            TPS.OrderName = "";
        //            TPS.XrayRoom = "";
        //            TPS.XJong = "D"; //'내시경
        //            TPS.ReadNo = "0";
        //            TPS.DrRemark = "";

        //            TPS.EndoChk = dt.Rows[i]["EndoChk"].ToString().Trim();

        //            TPS.Pano = dt.Rows[i]["Pano"].ToString().Trim();
        //            TPS.SName = dt.Rows[i]["SName"].ToString().Trim();
        //            TPS.EName2 = dt.Rows[i]["EName"].ToString().Trim();
        //            TPS.Sex = dt.Rows[i]["Sex"].ToString().Trim();
        //            TPS.DeptCode = dt.Rows[i]["DeptCode"].ToString().Trim();
        //            TPS.DrCode = dt.Rows[i]["DrCode"].ToString().Trim();
        //            TPS.IpdOpd = dt.Rows[i]["IpdOpd"].ToString().Trim();
        //            TPS.WardCode = dt.Rows[i]["WardCode"].ToString().Trim();
        //            TPS.RoomCode = dt.Rows[i]["RoomCode"].ToString().Trim();
        //            TPS.OrderCode = dt.Rows[i]["OrderCode"].ToString().Trim();
        //            TPS.SeekDate = dt.Rows[i]["RDate"].ToString().Trim();
        //            TPS.Remark = "";
        //            TPS.ROWID = dt.Rows[i]["ROWID"].ToString().Trim();
        //            TPS.Buse = dt.Rows[i]["Buse"].ToString().Trim();

        //            TPS.Modality = "";
        //            TPS.Resource = "";
        //            TPS.Emergency = "";
        //            TPS.XCode = "";
        //            TPS.Disease = "";
        //            TPS.Disease = read_Xray_DISEASE_set(pDbCon, "2", dt.Rows[i]["BDate2"].ToString().Trim());
        //            TPS.PACS_Code = "";

        //            TPS.Result = ""; //판독결과값
        //            TPS.ResultDate = "";//판독결과일자


        //            #endregion

        //            //전송정보 체크 및 정리후 팍스에 넘김
        //            HL7_Send(pDbCon);

        //            if (clsComSupXraySend.GstrPacsSend == "OK")
        //            {
        //                //갱신
        //                SQL = " UPDATE " + ComNum.DB_MED + "ENDO_JUPMST  SET        \r\n";

        //                if (bResult == true)
        //                {
        //                    SQL += " ResultSend = 'Y',                              \r\n";
        //                    SQL += " XSENDDATE = SYSDATE                            \r\n";
        //                }
        //                else
        //                {
        //                    if (TPS.SendGbn == "2")
        //                    {
        //                        SQL += "  PacsSend='',PacsNo='',XSENDDATE =''       \r\n";
        //                    }
        //                    else
        //                    {
        //                        SQL += " PacsSend = 'Y',                            \r\n";
        //                        SQL += " PacsNo = '" + TPS.PacsNo + "',             \r\n";
        //                        SQL += " XSENDDATE = SYSDATE                        \r\n";
        //                    }
        //                }

        //                SQL += " WHERE 1=1                                          \r\n";
        //                SQL += "  AND ROWID='" + TPS.ROWID + "'                     \r\n";

        //                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
        //                if (SqlErr != "")
        //                {
        //                    clsDB.setRollbackTran(pDbCon);
        //                    ComFunc.MsgBox(SqlErr);
        //                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
        //                    return false;
        //                }


        //                //시트표시
        //                spd.ActiveSheet.AddRows(0, 1);
        //                if (spd.ActiveSheet.RowCount > 500) spd.ActiveSheet.RowCount = 500;

        //                spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmSpdXSend.SendTime].Text = VB.Mid(TPS.MsgControlid, 3, 6) + "-" + VB.Mid(TPS.MsgControlid, 9, 4);
        //                spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmSpdXSend.PacsNo].Text = TPS.PacsNo;
        //                spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmSpdXSend.Pano].Text = TPS.Pano;
        //                spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmSpdXSend.SName].Text = TPS.SName;
        //                spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmSpdXSend.SendGbn].Text = TPS.SendGbn;
        //                spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmSpdXSend.OrderCode].Text = TPS.OrderCode;
        //                spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmSpdXSend.OrderName].Text = TPS.Remark != "" ? (TPS.OrderName + "^" + TPS.Remark) : TPS.OrderName;


        //            }

        //        }

        //        clsDB.setCommitTran(pDbCon);
        //    }

        //    dt.Dispose();
        //    dt = null;

        //    #endregion


        //    return true;

        //}

        //내시경 HL7 Order 쿼리
        //DataTable sel_EndoPacsSend(PsmhDb pDbCon, bool bResult = false)
        //{
        //    DataTable dt = null;

        //    SQL = "";
        //    SQL += "SELECT                                                                                          \r\n";
        //    SQL += "  TO_CHAR(a.ENTDATE,'YYYYMMDDHH24MI') EntDate,a.PacsNo,'' SendGbn,a.GbSunap,                    \r\n";
        //    SQL += "  a.PTno Pano,b.SName,b.EName,a.Sex,b.Jumin1,b.Jumin2,b.Jumin3,a.DeptCode,a.DrCode,             \r\n";
        //    SQL += "  a.GBIO IPDOPD,a.WardCode,a.RoomCode,a.OrderCode,a.EndoChk, a.BUSE,                            \r\n";
        //    SQL += "  TO_CHAR(a.RDate,'YYYYMMDD') RDate,TO_CHAR(a.BDate,'YYYYMMDD') BDate,                          \r\n";
        //    SQL += "  TO_CHAR(a.BDate,'YYYY-MM-DD') BDate2,                                                         \r\n";
        //    SQL += "  TO_CHAR(a.ResultDATE,'YYYYMMDDHH24MI') ResultDate,                                            \r\n";
        //    SQL += "  a.Remark,a.PacsUID,a.PacsSend,a.ROWID                                                         \r\n";
        //    SQL += " FROM " + ComNum.DB_MED + "ENDO_JUPMST a, " + ComNum.DB_PMPA + "BAS_PATIENT b                   \r\n";
        //    SQL += "  WHERE 1=1                                                                                     \r\n";
        //    SQL += "   AND a.Ptno=b.Pano(+)                                                                         \r\n";
        //    if (bResult == true)
        //    {
        //        SQL += "   AND a.ResultDate >= TRUNC(SYSDATE-5)                                                     \r\n";
        //        SQL += "   AND a.ResultSend = '*'                                                                   \r\n";
        //        SQL += "   AND ROWNUM <= 10                                                                         \r\n";
        //        SQL += "  ORDER BY a.ResultDate                                                                     \r\n";
        //    }
        //    else
        //    {
        //        SQL += "   AND a.JupsuName <> '$$'                                                                  \r\n";
        //        SQL += "   AND a.RDate >= TRUNC(SYSDATE-3)                                                          \r\n";
        //        SQL += "   AND a.PacsSend = '*'                                                                     \r\n";
        //        SQL += "   AND ROWNUM <= 30                                                                         \r\n";
        //        SQL += "  ORDER BY a.RDate                                                                          \r\n";
        //    }

        //    try
        //    {
        //        SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

        //        if (SqlErr != "")
        //        {
        //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
        //            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
        //            return null;
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
        //        return null;
        //    }

        //    return dt;

        //}

        //촬영완료/미완료처리
        //public bool Send_PacsEnd_Set(PsmhDb pDbCon)
        //{
        //    DataTable dt = null;
        //    DataTable dt2 = null;

        //    string strPano = "";
        //    string strPacsNo = "";
        //    string strUID = "";
        //    string strFLAG = "";
        //    string strCONCLUSION = "";
        //    string strCONFDR1 = "";
        //    string strADDENDUM = "";

        //    string strXJong = "";
        //    string strGbRead = "";
        //    string strROWID = "";

        //    bool bSend_OK = false;


        //    int intRowAffected = 0; //변경된 Row 받는 변수                       

        //    //쿼리실행      
        //    dt = sel_xrayPacsSReport(pDbCon);

        //    if (dt == null) return false;

        //    if (dt.Rows.Count > 0)
        //    {

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            // clsTrans DT = new clsTrans();
        //            clsDB.setBeginTran(pDbCon);

        //            strPano = dt.Rows[i]["PATID"].ToString().Trim();
        //            strPacsNo = dt.Rows[i]["HISORDERID"].ToString().Trim();
        //            strUID = dt.Rows[i]["QUEUEID"].ToString().Trim();
        //            strFLAG = dt.Rows[i]["READSTAT"].ToString().Trim();
        //            strCONCLUSION = dt.Rows[i]["CONCLUSION"].ToString().Trim();
        //            strCONFDR1 = dt.Rows[i]["CONFDR1"].ToString().Trim();
        //            strADDENDUM = dt.Rows[i]["ADDENDUM"].ToString().Trim();

        //            //---------( 판독결과 PACS에서 OCS로 전송)----------------

        //            bSend_OK = false;

        //            if (strCONCLUSION != "" && strCONFDR1 != "" && strFLAG == "C")
        //            {
        //                if (PACS_SRESULT_UPDATE(pDbCon, strPano, strPacsNo, strCONCLUSION, strCONFDR1, strADDENDUM) == false)
        //                {
        //                    bSend_OK = true;
        //                    return false;
        //                }
        //                bSend_OK = true;
        //            }
        //            else if (VB.Left(strPano, 1) == "H")
        //            {
        //                //일반검진
        //                SQL = " UPDATE " + ComNum.DB_PMPA + "HIC_XRAY_RESULT  SET               \r\n";
        //                SQL += "    GbPacs='Y'                                                  \r\n";
        //                SQL += " WHERE 1=1                                                      \r\n";
        //                SQL += "  AND Pano=" + Convert.ToInt32(TPS.Pano.Replace("H", "")) + "    \r\n";
        //                SQL += "  AND XrayNo='" + TPS.PacsNo + "'                               \r\n";

        //                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
        //                if (SqlErr != "")
        //                {
        //                    bSend_OK = false;
        //                    clsDB.setRollbackTran(pDbCon);
        //                    ComFunc.MsgBox(SqlErr);
        //                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
        //                    return false;
        //                }

        //                bSend_OK = true;
        //            }
        //            else
        //            {
        //                //건진 촬영완료 SET
        //                SQL = " UPDATE " + ComNum.DB_PMPA + "HIC_XRAY_RESULT  SET   \r\n";
        //                SQL += "    GbPacs='Y'                                      \r\n";
        //                SQL += " WHERE 1=1                                          \r\n";
        //                SQL += "  AND Ptno='" + TPS.Pano + "'                       \r\n";
        //                SQL += "  AND PacsNo='" + TPS.PacsNo + "'                   \r\n";

        //                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
        //                if (SqlErr != "")
        //                {
        //                    bSend_OK = false;
        //                    clsDB.setRollbackTran(pDbCon);
        //                    ComFunc.MsgBox(SqlErr);
        //                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
        //                    return false;
        //                }

        //                if (intRowAffected > 0)
        //                {
        //                    bSend_OK = true;
        //                }

        //            }

        //            if (bSend_OK == false)
        //            {
        //                //쿼리실행      
        //                dt2 = sel_XrayDetail(pDbCon, strPano, strPacsNo);
        //                if (dt2 != null && dt2.Rows.Count > 0)
        //                {
        //                    strXJong = dt2.Rows[0]["XJong"].ToString().Trim();
        //                    strGbRead = dt2.Rows[0]["GbRead"].ToString().Trim();
        //                    strROWID = dt2.Rows[0]["ROWID"].ToString().Trim();

        //                    if (strGbRead != "Y")
        //                    {
        //                        if (dt2.Rows[0]["DrRemark"].ToString().Trim().Contains("판독") == true)
        //                        {
        //                            strGbRead = "Y";
        //                        }
        //                    }

        //                }
        //                else
        //                {
        //                    dt2 = sel_endoJupmst(pDbCon, strPano, strPacsNo);
        //                    if (dt2 != null && dt2.Rows.Count > 0)
        //                    {
        //                        strXJong = "ENDO";
        //                        strROWID = dt2.Rows[0]["ROWID"].ToString().Trim();
        //                    }

        //                }

        //                //내시경경우
        //                if (strXJong == "ENDO")
        //                {
        //                    if (strFLAG == "M")
        //                    {
        //                        SQL = " UPDATE " + ComNum.DB_MED + "ENDO_JUPMST  SET            \r\n";
        //                        SQL += "   PacsUID='" + strUID + "' , XSENDDATE = SYSDATE       \r\n";
        //                        SQL += " WHERE 1=1                                              \r\n";
        //                        SQL += "  AND ROWID='" + strROWID + "'                          \r\n";
        //                        SQL += "  AND PacsUID IS NULL                                   \r\n";
        //                    }
        //                    else
        //                    {
        //                        SQL = " UPDATE " + ComNum.DB_MED + "ENDO_JUPMST  SET            \r\n";
        //                        SQL += "   PacsUID='' , XSENDDATE = ''                          \r\n";
        //                        SQL += " WHERE 1=1                                              \r\n";
        //                        SQL += "  AND ROWID='" + strROWID + "'                          \r\n";
        //                        SQL += "  AND PacsUID IS NOT NULL                               \r\n";
        //                    }

        //                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
        //                    if (SqlErr != "")
        //                    {
        //                        clsDB.setRollbackTran(pDbCon);
        //                        ComFunc.MsgBox(SqlErr);
        //                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
        //                        return false;
        //                    }

        //                }
        //                else
        //                {
        //                    //PACS 전송용 Table에 INSERT(촬영완료)
        //                    //'특수촬영은 특수촬영후 CR로 촬영하기 위하여 촬영완료를 않함
        //                    //'(방사선접수에서 재료입력을 해야만 WorkList에서 없어짐)
        //                    if (strXJong != "2" && strFLAG == "M")
        //                    {
        //                        SQL = " INSERT INTO " + ComNum.DB_PMPA + "XRAY_PACSSEND                         \r\n";
        //                        SQL += "   (EntDate,PacsNo,SendGbn,Pano,SName,                                  \r\n";
        //                        SQL += "    Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode,    \r\n";
        //                        SQL += "    XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark,Gbinfo)           \r\n";
        //                        SQL += "  SELECT SYSDATE,PacsNo,'4',Pano,SName,Sex,Age,IpdOpd,DeptCode,DrCode,  \r\n";
        //                        SQL += "         WardCode,RoomCode,XJong,XSubCode,XCode,OrderCode,SeekDate,     \r\n";
        //                        SQL += "         Remark,XRayRoom,DrRemark,Gbinfo                                \r\n";
        //                        SQL += "    FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                               \r\n";
        //                        SQL += "     WHERE 1=1                                                          \r\n";
        //                        SQL += "      AND ROWID='" + strROWID + "'                                      \r\n";
        //                        SQL += "      AND PacsNo IS NOT NULL                                            \r\n";
        //                        SQL += "      AND PacsStudyID IS NOT NULL                                       \r\n";

        //                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
        //                        if (SqlErr != "")
        //                        {
        //                            clsDB.setRollbackTran(pDbCon);
        //                            ComFunc.MsgBox(SqlErr);
        //                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
        //                            return false;
        //                        }

        //                    }

        //                    //'XRAY_DETAIL에 촬영완료 UPDATE
        //                    //'단순촬영은 영상이 전송되면 바로 재료입력 처리함
        //                    //'촬영완료시간(영상전송시간)을 Update
        //                    //'2006-06-30 안저촬영 자용 재료입력 처리(이양재)
        //                    //'2008-07-08 BMD 자동 재료입력

        //                    if (strFLAG == "M")
        //                    {
        //                        if (strXJong == "1" || strXJong == "F" || strXJong == "7")
        //                        {
        //                            //
        //                            SQL = " UPDATE " + ComNum.DB_PMPA + "XRAY_DETAIL  SET       \r\n";
        //                            SQL += "    PacsStudyID='" + strPacsNo + "',                \r\n";
        //                            SQL += "    PACS_END='Y',GbEnd='1',MgrNo='',GbSTS ='7',     \r\n";
        //                            SQL += "     CDate =SYSDATE,GbRead='" + strGbRead + "',     \r\n";
        //                            SQL += "     XSENDDATE = SYSDATE                            \r\n";
        //                            SQL += " WHERE 1=1                                          \r\n";
        //                            SQL += "  AND ROWID='" + strROWID + "'                      \r\n";
        //                            SQL += "  AND PacsStudyID IS NULL                           \r\n";
        //                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

        //                        }
        //                        else
        //                        {
        //                            //
        //                            SQL = " UPDATE " + ComNum.DB_PMPA + "XRAY_DETAIL  SET       \r\n";
        //                            SQL += "    PacsStudyID='" + strPacsNo + "',                \r\n";
        //                            SQL += "    PACS_END='Y',GbSTS ='7',cSabun=999,             \r\n";
        //                            SQL += "     CDate =SYSDATE,GbRead='" + strGbRead + "',     \r\n";
        //                            SQL += "     XSENDDATE = SYSDATE                            \r\n";
        //                            SQL += " WHERE 1=1                                          \r\n";
        //                            SQL += "  AND ROWID='" + strROWID + "'                      \r\n";
        //                            SQL += "  AND PacsStudyID IS NULL                           \r\n";
        //                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //
        //                        SQL = " UPDATE " + ComNum.DB_PMPA + "XRAY_DETAIL  SET       \r\n";
        //                        SQL += "    PacsStudyID='',                                 \r\n";
        //                        SQL += "    PACS_END='',GbEnd ='',GbSTS ='0',               \r\n";
        //                        SQL += "     CDate ='',                                     \r\n";
        //                        SQL += "     XSENDDATE = ''                                 \r\n";
        //                        SQL += " WHERE 1=1                                          \r\n";
        //                        SQL += "  AND ROWID='" + strROWID + "'                      \r\n";
        //                        SQL += "  AND PacsStudyID IS NOT NULL                       \r\n";
        //                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
        //                    }

        //                    if (SqlErr != "")
        //                    {
        //                        clsDB.setRollbackTran(pDbCon);
        //                        ComFunc.MsgBox(SqlErr);
        //                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
        //                        return false;
        //                    }
        //                }

        //            }

        //            //
        //            SQL = " UPDATE " + ComNum.DB_PACS + "XRAY_PACS_SREPORT  SET         \r\n";
        //            SQL += "    FLAG='Y'                                                \r\n";
        //            SQL += " WHERE 1=1                                                  \r\n";
        //            SQL += "  AND ROWID='" + dt.Rows[i]["ROWID"].ToString().Trim() + "' \r\n";

        //            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
        //            if (SqlErr != "")
        //            {
        //                clsDB.setRollbackTran(pDbCon);
        //                ComFunc.MsgBox(SqlErr);
        //                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
        //                return false;
        //            }

        //            clsDB.setCommitTran(pDbCon);
        //        }


        //    }

        //    dt.Dispose();
        //    dt = null;


        //    return true;

        //}

        //DataTable sel_xrayPacsSReport(PsmhDb pDbCon)
        //{
        //    DataTable dt = null;

        //    SQL = "";
        //    SQL += "SELECT                                          \r\n";
        //    SQL += "  QUEUEID,HISORDERID,PATID,READSTAT,            \r\n";
        //    SQL += "  CONCLUSION,CONFDR1,ROWID, ADDENDUM            \r\n";
        //    SQL += " FROM " + ComNum.DB_PACS + "XRAY_PACS_SREPORT   \r\n";
        //    SQL += "  WHERE 1=1                                     \r\n";
        //    SQL += "   AND WORKTIME >=TRUNC(SYSDATE-1)              \r\n";
        //    SQL += "   AND FLAG = 'N'                               \r\n";
        //    SQL += "  ORDER BY QUEUEID                              \r\n";

        //    try
        //    {
        //        SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

        //        if (SqlErr != "")
        //        {
        //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
        //            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
        //            return null;
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
        //        return null;
        //    }

        //    return dt;

        //}

        //public string HL7_Send(PsmhDb pDbCon)
        //{

        //    string strSname = "";
        //    string strBaby = "";


        //    #region //영문 성명 및 생년월일 나이 Setting

        //    //'--------------------------------------
        //    //'  영문 성명 및 생년월일 Setting      
        //    //'--------------------------------------

        //    strSname = TPS.SName.Trim();
        //    strBaby = "";

        //    if (VB.Right(strSname, 2) == "애기")
        //    {
        //        strBaby = "(B)";
        //        strSname = VB.Left(strSname, VB.Len(strSname) - 2);
        //    }
        //    else if (VB.Right(strSname, 2) == "애1")
        //    {
        //        strBaby = "(B1)";
        //        strSname = VB.Left(strSname, VB.Len(strSname) - 2);
        //    }
        //    else if (VB.Right(strSname, 2) == "애2")
        //    {
        //        strBaby = "(B2)";
        //        strSname = VB.Left(strSname, VB.Len(strSname) - 2);
        //    }
        //    else if (VB.Right(strSname, 2) == "B1")
        //    {
        //        strBaby = "(B1)";
        //        strSname = VB.Left(strSname, VB.Len(strSname) - 2);
        //    }
        //    else if (VB.Right(strSname, 2) == "B2")
        //    {
        //        strBaby = "(B2)";
        //        strSname = VB.Left(strSname, VB.Len(strSname) - 2);
        //    }

        //    if (TPS.EName2.Trim() != "")
        //    {
        //        TPS.EName = TPS.EName2.Trim();
        //    }
        //    else
        //    {
        //        TPS.EName = HanName_TO_EngName(pDbCon, strSname) + strBaby.Trim();
        //    }
        //    TPS.SName = strSname + strBaby;

        //    TPS.SName2 = strSname;//이름만
        //    if (TPS.SName2.Trim() == "") TPS.SName2 = TPS.SName;


        //    //생년월일 및 주민번호 세팅
        //    Birth_Date_SET(pDbCon, TPS.Pano);

        //    if (TPS.Age == "" || TPS.Age == "0" || TPS.Age == "10")
        //    {
        //        if (TPS.Birth == "")
        //        {
        //            TPS.Age = ComFunc.AgeCalcX1(TPS.Jumin1 + TPS.Jumin2, clsPublic.GstrSysDate);
        //        }
        //        else
        //        {
        //            TPS.Age = ComFunc.AgeCalcX1(TPS.Jumin1 + TPS.Jumin2, clsPublic.GstrSysDate); ; //TODO 윤조연 TPS.Age = AGE_YEAR_Birth((TPS.Birth), GstrSysDate)
        //        }
        //    }


        //    #endregion

        //    #region //HL7 Message Setting      

        //    //'--------------------------------------
        //    //'      HL7 Message Setting      
        //    //'--------------------------------------
        //    if (TPS.DeptCode == "EM" || TPS.DeptCode == "ER") TPS.IpdOpd = "E"; //응급실

        //    //의사명 및 의사영문명
        //    TPS.DrName = "";
        //    TPS.DrEName = "";
        //    set_basDrNameE(pDbCon, TPS.DrCode);

        //    TPS.OrderName = cxray.OCS_XNAME_READ(pDbCon, TPS.OrderCode, false, true);

        //    // 모달리티. 리소스 세팅
        //    set_Modality();


        //    clsComSupXraySend.GstrPacsSend = "";

        //    PACS_ORDER_SEND(pDbCon);

        //    if (clsComSupXraySend.GstrPacsSend == "")
        //    {
        //        //전송실패
        //        return "";
        //    }

        //    #endregion


        //    return "OK";
        //}

        #endregion

        #endregion

        //상태자동 갱신처리
        //public void Send_GbSTS_AUTO_Set(PsmhDb pDbCon)
        //{
        //    DataTable dtx = null;
        //    string strROWID = "";
        //    int intRowAffected = 0;

        //    dtx = sel_XrayDetail_batch(pDbCon, "1");//수가기준 자동처리
        //    if (dtx != null && dtx.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dtx.Rows.Count; i++)
        //        {
        //            clsTrans DT = new clsTrans();
        //            clsDB.setBeginTran(pDbCon);

        //            strROWID = dtx.Rows[i]["ROWID"].ToString().Trim();

        //            if (Convert.ToInt16(dtx.Rows[i]["Qty"].ToString().Trim()) > 0)
        //            {

        //                SqlErr = up_XRAY_DETAIL_STS(pDbCon, "1", strROWID, ref intRowAffected);
        //                if (SqlErr != "")
        //                {
        //                    clsDB.setRollbackTran(pDbCon);
        //                    ComFunc.MsgBox(SqlErr);
        //                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
        //                    return;
        //                }
        //            }

        //            clsDB.setCommitTran(pDbCon);
        //        }
        //    }

        //    dtx = null;

        //    dtx = sel_XrayDetail_batch(pDbCon, "2");//특정과 자동처리
        //    if (dtx != null && dtx.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dtx.Rows.Count; i++)
        //        {
        //            clsTrans DT = new clsTrans();
        //            clsDB.setBeginTran(pDbCon);

        //            strROWID = dtx.Rows[i]["ROWID"].ToString().Trim();

        //            if (Convert.ToInt16(dtx.Rows[i]["Qty"].ToString().Trim()) > 0)
        //            {

        //                SqlErr = up_XRAY_DETAIL_STS(pDbCon, "1", strROWID, ref intRowAffected);
        //                if (SqlErr != "")
        //                {
        //                    clsDB.setRollbackTran(pDbCon);
        //                    ComFunc.MsgBox(SqlErr);
        //                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
        //                    return;
        //                }
        //            }

        //            clsDB.setCommitTran(pDbCon);
        //        }
        //    }


        //    dtx = null;

        //    dtx = sel_XrayDetail_batch(pDbCon, "3");//특정수가 자동처리 2012-12-24 -의뢰
        //    if (dtx != null && dtx.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dtx.Rows.Count; i++)
        //        {
        //            clsTrans DT = new clsTrans();
        //            clsDB.setBeginTran(pDbCon);

        //            strROWID = dtx.Rows[i]["ROWID"].ToString().Trim();

        //            if (Convert.ToInt16(dtx.Rows[i]["Qty"].ToString().Trim()) > 0)
        //            {

        //                SqlErr = up_XRAY_DETAIL_STS(pDbCon, "2", strROWID, ref intRowAffected);
        //                if (SqlErr != "")
        //                {
        //                    clsDB.setRollbackTran(pDbCon);
        //                    ComFunc.MsgBox(SqlErr);
        //                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
        //                    return;
        //                }
        //            }

        //            clsDB.setCommitTran(pDbCon);
        //        }
        //    }


        //}

        //인피니트 PACS에서 저장한 판독결과를 OCS에 UpDate
        //bool PACS_SRESULT_UPDATE(PsmhDb pDbCon,string argPano, string argPacsNo, string argCONCLUSION, string argCONFDR, string ArgADDENDUM)
        //{
        //    //string strResult   ="";
        //    string strResult1 = "";
        //    string strResult2 = "";
        //    string strXCode = "";
        //    string strXName = "";
        //    long nDrCode1 = 0;
        //    long nDrCode2 = 0;
        //    long nDrCode3 = 0;
        //    string strMsym1 = "";
        //    string strMsym2 = "";
        //    string strMsym3 = "";
        //    //string strROWID = "";

        //    string strPano = "";
        //    string strIpdOpd = "";
        //    string strSeekDate = "";
        //    string strSname = "";
        //    string strSex = "";
        //    string strAge = "";
        //    string strDeptCode = "";
        //    string strDrCode = "";
        //    string strWardCode = "";
        //    string strRoomCode = "";
        //    string strXJong = "";
        //    long nWRTNO = 0;

        //    DataTable dtx = null;

        //    int intRowAffected = 0; //변경된 Row 받는 변수                       



        //    dtx = sel_XrayDetail(pDbCon, argPano, argPacsNo);

        //    if (dtx != null && dtx.Rows.Count > 0)
        //    {
        //        TPS.ReadNo = dtx.Rows[0]["ExInfo"].ToString().Trim();

        //        strSeekDate = dtx.Rows[0]["SeekDate"].ToString().Trim();
        //        strPano = argPano;
        //        strSname = dtx.Rows[0]["SName"].ToString().Trim();
        //        strDeptCode = dtx.Rows[0]["DEPTCODE"].ToString().Trim();
        //        strDrCode = dtx.Rows[0]["DRCODE"].ToString().Trim();
        //        strIpdOpd = dtx.Rows[0]["IPDOPD"].ToString().Trim();
        //        strWardCode = dtx.Rows[0]["WARDCODE"].ToString().Trim();
        //        strRoomCode = dtx.Rows[0]["ROOMCODE"].ToString().Trim();
        //        strXJong = dtx.Rows[0]["XJONG"].ToString().Trim();
        //        strXCode = dtx.Rows[0]["XCODE"].ToString().Trim();
        //        strXName = cxray.OCS_XNAME_READ(pDbCon, dtx.Rows[0]["OrderCode"].ToString().Trim(),false, true);
        //        strSex = dtx.Rows[0]["SEX"].ToString().Trim();
        //        strAge = dtx.Rows[0]["AGE"].ToString().Trim();
        //        nWRTNO = Convert.ToInt32(dtx.Rows[0]["EXINFO"].ToString());
        //        if (nWRTNO < 1000) nWRTNO = 0;

        //    }
        //    else
        //    {

        //        return false;
        //    }


        //    strXName = ComFunc.QuotConv(strXName);

        //    nDrCode1 = Convert.ToInt32(argCONFDR);

        //    nDrCode2 = 0;
        //    nDrCode3 = 0;
        //    strMsym1 = "";
        //    strMsym2 = "";
        //    strMsym3 = "";

        //    // clsTrans DT = new clsTrans();
        //    clsDB.setBeginTran(pDbCon);

        //    if (nWRTNO <= 1000)
        //    {
        //        //신규
        //        //새로운 WRTNO를 부여함
        //        nWRTNO = Convert.ToInt32(ComQuery.GetSequencesNo(pDbCon, "KOSMOS_PMPA", "SEQ_XRAYREAD"));

        //        SQL = " INSERT INTO " + ComNum.DB_PMPA + "XRAY_RESULTNEW                                                            \r\n";
        //        SQL += "  (WRTNO,Pano,ReadDate,ReadTime,SeekDate,XJong,SName,Sex,Age,                                               \r\n";
        //        SQL += "   IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XDrCode1,XDrCode2,XDrCode3,                                     \r\n";
        //        SQL += "   IllCode1,IllCode2,IllCode3,XCode,XName,Result,Result1,                                                   \r\n";
        //        SQL += "   EntDate,Approve, STime, ETime  ) VALUES                                                                  \r\n";
        //        SQL += "  ( " + nWRTNO + ",'" + argPano + "',                                                                       \r\n";
        //        SQL += "  TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD'),                                                    \r\n";
        //        SQL += "  TO_DATE('" + clsPublic.GstrSysDate + " " + VB.Left(clsPublic.GstrSysTime, 5) + "','YYYY-MM-DD HH24:MI'),  \r\n";
        //        SQL += "  TO_DATE('" + strSeekDate + "','YYYY-MM-DD'),                                                              \r\n";
        //        SQL += "  '" + strXJong + "','" + strSname.Trim() + "',                                                             \r\n";
        //        SQL += "  '" + strSex + "','" + strAge + "', '" + strIpdOpd + "',                                                    \r\n";
        //        SQL += "  '" + strDeptCode + "','" + strDrCode + "', '" + strWardCode + "',                                         \r\n";
        //        SQL += "  '" + strRoomCode + "','" + nDrCode1 + "', 0 , 0 ,                                                         \r\n";
        //        SQL += "  '" + strMsym1 + "','" + strMsym2 + "', '" + strMsym3 + "','" + strXCode + "' ,                            \r\n";
        //        SQL += "  '" + strXName + "','" + strResult1 + "', '" + strResult2 + "', SYSDATE, 'Y',                              \r\n";
        //        SQL += "   SYSDATE, SYSDATE                                                                                         \r\n";
        //        SQL += "  )                                                                                                         \r\n";

        //        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
        //        if (SqlErr != "")
        //        {
        //            clsComSupXraySend.GstrPacsSend = "";

        //            clsDB.setRollbackTran(pDbCon);
        //            ComFunc.MsgBox(SqlErr);
        //            clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장                            
        //            return false;
        //        }


        //    }
        //    else
        //    {
        //        //갱신
        //        SQL = " UPDATE " + ComNum.DB_PMPA + "XRAY_RESULTNEW  SET                                                                            \r\n";
        //        SQL += " XDrCode2 = " + nDrCode2 + ",                                                                                               \r\n";
        //        SQL += " XDrCode3=" + nDrCode3 + ",                                                                                                 \r\n";
        //        SQL += " IllCode1='" + strMsym1 + "',                                                                                               \r\n";
        //        SQL += " IllCode2='" + strMsym2 + "',                                                                                               \r\n";
        //        SQL += " IllCode3='" + strMsym3 + "',                                                                                               \r\n";
        //        SQL += " XCode='" + strXCode + "',                                                                                                  \r\n";
        //        SQL += " XName='" + strXName + "',                                                                                                  \r\n";
        //        if (ArgADDENDUM == "A")
        //        {
        //            SQL += " ADDENDUM1='" + strResult1 + "',                                                                                        \r\n";
        //            SQL += " ADDENDUM2='" + strResult2 + "',                                                                                        \r\n";
        //            SQL += " ADDDATE = TO_DATE('" + clsPublic.GstrSysDate + " " + VB.Left(clsPublic.GstrSysTime, 5) + "','YYYY-MM-DD HH24:MI') ,     \r\n";
        //            SQL += " ADDDrCode=" + nDrCode1 + ",                                                                                            \r\n";
        //        }
        //        else
        //        {
        //            SQL += " ReadDate=TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD'),                                                        \r\n";
        //            SQL += " ReadTime =TO_DATE('" + clsPublic.GstrSysDate + " " + VB.Left(clsPublic.GstrSysTime, 5) + "','YYYY-MM-DD HH24:MI'),      \r\n";
        //            SQL += " Result='" + strResult1 + "',                                                                                           \r\n";
        //            SQL += " Result1='" + strResult2 + "',                                                                                          \r\n";
        //        }
        //        SQL += " Approve='Y',                                                                                                               \r\n";
        //        SQL += " EntDate=SYSDATE,                                                                                                           \r\n";
        //        SQL += " STIME = SYSDATE ,                                                                                                          \r\n";
        //        SQL += " ETIME = SYSDATE,                                                                                                           \r\n";
        //        SQL += " SENDEMR = ''                                                                                                               \r\n";

        //        SQL += "  WHERE WRTNO  =" + nWRTNO + "                                                                                              \r\n";

        //        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
        //        if (SqlErr != "")
        //        {
        //            clsComSupXraySend.GstrPacsSend = "";

        //            clsDB.setRollbackTran(pDbCon);
        //            ComFunc.MsgBox(SqlErr);
        //            clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장                            
        //            return false;
        //        }

        //    }


        //    return true;
        //}


        //명칭 정리(오더명칭,참고사항)
        public string EDIT_OrderName(string str, bool bright = false)
        {
            string strTemp = str.Trim();

            if (strTemp == "") return "";

            strTemp = strTemp.Replace("  ", " ");//연속공란을 공란1칸으로
            strTemp = strTemp.Replace("  ", " ");//연속공란을 공란1칸으로

            strTemp = strTemp.Replace("[CR]", "");//[CR] 제거
            strTemp = strTemp.Replace("[CR)", "");//[CR) 제거
            strTemp = strTemp.Replace("(CR)", "");//(CR) 제거
            strTemp = strTemp.Replace("(R-A)", "");//(R-A) 제거
            strTemp = strTemp.Replace("(RA)", "");//(RA) 제거

            strTemp = strTemp.Replace("(Both)", "(B)");//(Both) => (B)
            strTemp = strTemp.Replace("(Both )", "(B)");//(Both) => (B)
            strTemp = strTemp.Replace("(BOTH )", "(B)");//(BOTH) => (B)

            strTemp = strTemp.Replace(" (", "(");// ( >(
            strTemp = strTemp.Replace(", ", ",");// ,  >,
            strTemp = strTemp.Replace("()", "");// ()제거

            if (bright)
            {
                strTemp = strTemp.Replace("Right", "(R)");// 참고사항 문구제거용
            }

            return strTemp;
        }

        public string HanName_TO_EngName(PsmhDb pDbCon,string argSName)
        {
            int i = 0;
            string strData = "";
            string strCode = "";
            string strReturn = "";

            for (i = 1; i <= VB.Len(argSName); i++)
            {
                if (VB.Mid(argSName, i, 1) != " ") strData += VB.Mid(argSName, i, 1);

            }
            strData = VB.Left(strData + VB.Space(10), 10);

            for (i = 1; i <= 5; i++)
            {
                strCode = VB.Mid(strData, i, 1);
                if (strCode != "") strReturn += READ_EngName(pDbCon, strCode) + " ";
            }


            return strReturn;
        }

        //BAS_Z300FONT에서 한글을 영문으로 변환(성)
        public string READ_EngName(PsmhDb pDbCon,string str)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                  \r\n";
            SQL += "  EngName                                               \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "BAS_Z300FONT                \r\n";
            SQL += "  WHERE 1=1                                             \r\n";
            SQL += "   AND Z300Code ='" + str + "'                          \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL,pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return "";
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL,pDbCon); //에러로그 저장
                return "";
            }

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["EngName"].ToString().Trim();
            }
            else
            {
                return "";
            }
        }

        //환자정보(성명,주민,생일,영문명,성별) 세팅
        //public void Birth_Date_SET(PsmhDb pDbCon,string argPano)
        //{
        //    DataTable dt = null;

        //    dt = sel_basPatient(pDbCon, argPano);

        //    if (dt.Rows.Count > 0)
        //    {
        //        TPS.SName = dt.Rows[0]["SName"].ToString().Trim();
        //        TPS.EName2 = dt.Rows[0]["EName"].ToString().Trim();
        //        TPS.Sex = dt.Rows[0]["Sex"].ToString().Trim();
        //        TPS.Jumin1 = dt.Rows[0]["Jumin1"].ToString().Trim();
        //        TPS.Jumin2 = dt.Rows[0]["Jumin2"].ToString().Trim();
        //        TPS.Birth = dt.Rows[0]["Birth"].ToString().Trim();

        //        if (TPS.Birth == "")
        //        {
        //            if (VB.Left(TPS.Jumin2, 1) == "1" || VB.Left(TPS.Jumin2, 1) == "1")
        //            {
        //                TPS.Birth = "19" + TPS.Jumin1;
        //            }
        //            else if (VB.Left(TPS.Jumin2, 1) == "3" || VB.Left(TPS.Jumin2, 1) == "4")
        //            {
        //                TPS.Birth = "20" + TPS.Jumin1;
        //            }
        //            else if (VB.Left(TPS.Jumin2, 1) == "5" || VB.Left(TPS.Jumin2, 1) == "6")
        //            {
        //                TPS.Birth = "19" + TPS.Jumin1;
        //            }
        //            else if (VB.Left(TPS.Jumin2, 1) == "7" || VB.Left(TPS.Jumin2, 1) == "8")
        //            {
        //                if (("20" + VB.Left(TPS.Jumin1, 2)).CompareTo(VB.Left(clsPublic.GstrSysDate, 4)) > 0)
        //                {
        //                    TPS.Birth = TPS.Birth = "19" + TPS.Jumin1;
        //                }
        //                else
        //                {
        //                    TPS.Birth = TPS.Birth = "20" + TPS.Jumin1;
        //                }
        //            }
        //            else if (VB.Left(TPS.Jumin2, 1) == "9" || VB.Left(TPS.Jumin2, 1) == "0")
        //            {
        //                TPS.Birth = "10" + TPS.Jumin1;
        //            }
        //            else
        //            {
        //                TPS.Birth = "";
        //            }

        //        }

        //    }
        //    else
        //    {
        //        TPS.Jumin1 = "";
        //        TPS.Jumin2 = "";
        //        clsComSupXraySend.GstrPacsSend = "";
        //    }
        //}

        //public void set_basDrNameE(PsmhDb pDbCon,string strDrCode)
        //{

        //    DataTable dt = null;
        //    string SQL = "";
        //    string SqlErr = ""; //에러문 받는 변수

        //    TPS.DrName = "";
        //    TPS.DrEName = "";

        //    if (strDrCode.Trim() == "") return;

        //    try
        //    {
        //        SQL = "";
        //        SQL = SQL + ComNum.VBLF + "SELECT DrName,DrEName FROM KOSMOS_PMPA.BAS_DOCTOR ";
        //        SQL = SQL + ComNum.VBLF + " WHERE DRCODE ='" + strDrCode + "' ";

        //        SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

        //        if (SqlErr != "")
        //        {
        //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
        //            clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
        //            return;
        //        }

        //        if (dt.Rows.Count > 0)
        //        {
        //            TPS.DrName = dt.Rows[0]["DRNAME"].ToString().Trim();
        //            TPS.DrEName = dt.Rows[0]["DrEName"].ToString().Trim();
        //        }

        //        dt.Dispose();
        //        dt = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (dt != null)
        //        {
        //            dt.Dispose();
        //            dt = null;
        //        }
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

        //    }

        //    return;
        //}

        //모달리티, 리소스 재정의
        //public void set_Modality()
        //{

        //    #region modality set

        //    if (TPS.XJong == "1")
        //    {
        //        TPS.Modality = "CR";  //'일반촬영
        //    }
        //    else if (TPS.XJong == "2")
        //    {
        //        TPS.Modality = "DR";  //'특수촬영
        //    }
        //    else if (TPS.XJong == "3")
        //    {
        //        TPS.Modality = "US";  //'SONO(방사선과)
        //    }
        //    else if (TPS.XJong == "4")
        //    {
        //        TPS.Modality = "CT";  //'CT
        //    }
        //    else if (TPS.XJong == "5")
        //    {
        //        TPS.Modality = "MR";  //'MRI
        //    }
        //    else if (TPS.XJong == "6")
        //    {
        //        TPS.Modality = "NM";  //'RI
        //    }
        //    else if (TPS.XJong == "7")
        //    {
        //        TPS.Modality = "OT";  //'BMD
        //    }
        //    else if (TPS.XJong == "8")
        //    {
        //        TPS.Modality = "CT";  //'PET-CT
        //    }
        //    else if (TPS.XJong == "A")
        //    {
        //        TPS.Modality = "US";  //'UR초음파
        //    }
        //    else if (TPS.XJong == "B")
        //    {
        //        TPS.Modality = "US";  //'GS초음파
        //    }
        //    else if (TPS.XJong == "C")
        //    {
        //        TPS.Modality = "US";  //'심장초음파
        //    }
        //    else if (TPS.XJong == "D")
        //    {
        //        TPS.Modality = "ES";  //'내시경
        //    }
        //    else if (TPS.XJong == "F")
        //    {
        //        TPS.Modality = "OT";  //'안압,안저
        //    }
        //    else if (TPS.XJong == "G")
        //    {
        //        TPS.Modality = "US";  //'OG초음파
        //    }
        //    else if (TPS.XJong == "Q")
        //    {
        //        TPS.Modality = "RF";  //'ANGIO
        //    }
        //    else if (TPS.XJong == "H")
        //    {
        //        TPS.Modality = "US";  //'ENT초음파
        //    }
        //    else if (TPS.XJong == "K")
        //    {
        //        TPS.Modality = "OT";  //'적외선체온열
        //    }
        //    else if (TPS.XJong == "L")
        //    {
        //        TPS.Modality = "ES";  //'이비인후과 검사
        //    }
        //    else if (TPS.XJong == "M")
        //    {
        //        TPS.Modality = "ES";  //'GS직장경 검사
        //    }
        //    else if (TPS.XJong == "N")
        //    {
        //        TPS.Modality = "OT";  //'과 안저,시신경
        //    }

        //    #endregion

        //    #region resource set

        //    //촬영장비 SET  , CT,MR,RI,BMD는 무조건 다시 Setting함
        //    if (TPS.Resource.Trim() == "" || (TPS.XJong.CompareTo("2") > 0))
        //    {
        //        if (TPS.XJong == "1")
        //        {
        //            TPS.Resource = "PRID1"; //일반촬영
        //        }
        //        else if (TPS.XJong == "2")
        //        {
        //            TPS.Resource = "DR"; //특수촬영
        //        }
        //        else if (TPS.XJong == "3")
        //        {
        //            TPS.Resource = "US1"; //SONO(방사선과)
        //        }
        //        else if (TPS.XJong == "4")
        //        {
        //            TPS.Resource = "CT"; //CT
        //        }
        //        else if (TPS.XJong == "5")
        //        {
        //            TPS.Resource = "MR"; //MRI
        //        }
        //        else if (TPS.XJong == "6")
        //        {
        //            TPS.Resource = "NM"; //RI
        //        }
        //        else if (TPS.XJong == "7")
        //        {
        //            TPS.Resource = "BMD"; //BMD
        //        }
        //        else if (TPS.XJong == "8")
        //        {
        //            TPS.Resource = "PETCT"; //PET-CT
        //        }
        //        else if (TPS.XJong == "A")
        //        {
        //            TPS.Resource = "US3"; //UR초음파
        //        }
        //        else if (TPS.XJong == "B")
        //        {
        //            TPS.Resource = "US4"; //GS초음파
        //        }
        //        else if (TPS.XJong == "C")
        //        {
        //            TPS.Resource = "US5"; //심장초음파
        //        }
        //        else if (TPS.XJong == "D")
        //        {
        //            TPS.Resource = "ES1"; //내시경
        //        }
        //        else if (TPS.XJong == "F")
        //        {
        //            TPS.Resource = "OT1"; //안저촬영
        //        }
        //        else if (TPS.XJong == "G")
        //        {
        //            TPS.Resource = "US7"; //OG초음파
        //        }
        //        else if (TPS.XJong == "Q")
        //        {
        //            TPS.Resource = "AN"; //ANGIO
        //        }
        //        else if (TPS.XJong == "K")
        //        {
        //            TPS.Resource = "IRISXP"; //적외선체온
        //        }
        //        else if (TPS.XJong == "L")
        //        {
        //            TPS.Resource = "ES1"; //이비인후과
        //        }
        //        else if (TPS.XJong == "M")
        //        {
        //            TPS.Resource = "ES1"; //GS직장경
        //        }
        //        else if (TPS.XJong == "N")
        //        {
        //            TPS.Resource = "OT2"; //과 안저,시신경
        //        }

        //    }


        //    #endregion

        //    #region 종검,건진,내시경 기타 예외 modality, resource 재설정

        //    //종검,건진 재설정
        //    if (TPS.XrayRoom == "T" || (TPS.XJong == "D" && TPS.DeptCode == "TO") || (TPS.XJong == "D" && TPS.DeptCode == "HR" && TPS.EndoChk == "Y"))
        //    {
        //        if (TPS.XJong == "1")
        //        {
        //            TPS.Resource = "INNO4"; //일반촬영
        //        }
        //        else if (TPS.XJong == "2")
        //        {
        //            TPS.Resource = "DR2"; //특수촬영(건진)
        //        }
        //        else if (TPS.XJong == "3")
        //        {
        //            TPS.Resource = "US2"; //SONO(건진)
        //        }
        //        else if (TPS.XJong == "D")
        //        {
        //            TPS.Resource = "ES2"; //종검내시경
        //        }
        //    }


        //    //내시경 체크
        //    if (TPS.XJong == "D")
        //    {
        //        if (TPS.Buse == "056104")
        //        {
        //            TPS.Resource = "ES1"; //내시경실
        //        }
        //        else if (TPS.Buse == "044500")
        //        {
        //            TPS.Resource = "ES2"; //종검내시경
        //        }
        //    }


        //    if (TPS.XJong == "F" && TPS.DeptCode == "OT")
        //    {
        //        TPS.Resource = "OT2"; //안저촬영
        //    }
        //    else if (TPS.XJong == "F" && TPS.IpdOpd == "I")
        //    {
        //        TPS.Resource = "OT2"; //안저촬영
        //    }

        //    if (TPS.XJong == "2" && TPS.XCode == "N003100A")
        //    {
        //        TPS.Modality = "ES";
        //        TPS.Resource = "ES1";
        //    }

        //    #endregion

        //}

        // 인피니트 PACS용 오더정보에 자료를 INSERT
        //void PACS_ORDER_SEND(PsmhDb pDbCon)
        //{

        //    if (TPS.SendGbn == "4")
        //    {
        //        clsComSupXraySend.GstrPacsSend = "OK";
        //        return;
        //    }

        //    long nDrSabun = 0;
        //    string strQUEUEID = "";
        //    string strExamCode = "";
        //    string strExamName = EDIT_OrderName(TPS.OrderName);
        //    string strRemark = EDIT_OrderName(TPS.Remark);
        //    if (strRemark != "") strExamName += " " + strRemark;

        //    if (strExamName.Trim() != "")
        //    {
        //        strExamCode = "";
        //    }

        //    nDrSabun = Convert.ToInt32(clsVbfunc.GetOCSDrCodeSabun(pDbCon, TPS.DrCode));

        //    strQUEUEID = clsPublic.GstrSysDate.Replace("-", "") + VB.Left(clsPublic.GstrSysTime.Replace(":", ""), 4);
        //    strQUEUEID += VB.Right(DateTime.Now.ToString("HHmmss"), 2) + VB.Right(TPS.PacsNo, 4).Trim();

        //    clsComSupXraySend.GstrPacsSend = "OK";

        //    if (clsComSupXraySend.GstrPacsSend == "OK")
        //    {
        //        //
        //        XRAY_PACS_ADT_INSERT(pDbCon, strQUEUEID);
        //    }

        //    if (clsComSupXraySend.GstrPacsSend == "OK")
        //    {
        //        //
        //        XRAY_PACS_ORDER_INSERT(pDbCon, strQUEUEID, strExamCode, strExamName, nDrSabun);
        //    }

        //}

        //팍스 환자정보 갱신 추가 여부 체크 및 XRAY_PACS_ADT생성
        //void XRAY_PACS_ADT_INSERT(PsmhDb pDbCon,string argQUEUEID)
        //{
        //    string strEPatName = "";
        //    int intRowAffected = 0; //변경된 Row 받는 변수                       
        //    DataTable dtx = null;

        //    dtx = sel_mPatient(pDbCon, TPS.Pano);
        //    if (dtx != null && dtx.Rows.Count > 0)
        //    {
        //        strEPatName = dtx.Rows[0]["EPatName"].ToString().Trim().Replace("^", " ");
        //    }

        //    dtx = null;

        //    dtx = sel_xrayPacsADT(pDbCon, TPS.Pano);
        //    if (dtx != null && dtx.Rows.Count > 0)
        //    {
        //        if (strEPatName != "" && TPS.EName2 != "")
        //        {
        //            if (TPS.EName2 == strEPatName)
        //            {
        //                clsComSupXraySend.GstrPacsSend = "OK";
        //                return;
        //            }
        //        }

        //    }

        //    // clsTrans DT = new clsTrans();
        //    clsDB.setBeginTran(pDbCon);

        //    //생성
        //    SQL = " INSERT INTO " + ComNum.DB_PACS + "XRAY_PACS_ADT                                         \r\n";
        //    SQL += "  (QUEUEID,FLAG,WORKTIME,BIRTHDAY,DEPT,ATTENDDOCT1,PATNAME,                             \r\n";
        //    SQL += "   PATTYPE,PATID,EVENTTYPE,PERSONALID,Sex,EPatName )   VALUES                           \r\n";
        //    SQL += "  ( '" + argQUEUEID + "','N',SYSDATE,                                                   \r\n";
        //    SQL += "  '" + TPS.Birth + "','" + TPS.DeptCode + "','" + TPS.DrCode + "',                         \r\n";
        //    SQL += "  '" + TPS.SName + "','" + TPS.IpdOpd + "','" + TPS.Pano + "',                          \r\n";

        //    if (TPS.IpdOpd == "I")
        //    {
        //        SQL += " 'A01',                                                                             \r\n";
        //    }
        //    else
        //    {
        //        SQL += " 'A04',                                                                             \r\n";
        //    }

        //    SQL += "  '" + TPS.Jumin1 + TPS.Jumin2 + "','" + TPS.Sex + "','" + TPS.EName2.Trim() + "'       \r\n";

        //    SQL += "  )                                                                                     \r\n";

        //    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected,pDbCon
        //        );
        //    if (SqlErr != "")
        //    {
        //        clsComSupXraySend.GstrPacsSend = "";

        //        clsDB.setRollbackTran(pDbCon);
        //        ComFunc.MsgBox(SqlErr);
        //        clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장     

        //        return;
        //    }

        //    clsDB.setCommitTran(pDbCon);

        //    clsComSupXraySend.GstrPacsSend = "OK";

        //}                

        public bool HIC_XRAY_PACS_ADT_ORDER_INSERT(PsmhDb pDbCon, string argDate,string argQUEUEID, string argPacsNo,string argCode, long argHPano, string argPtno,string ExamRoom)
        {            
            int intRowAffected = 0; //변경된 Row 받는 변수                       
            DataTable dt = null;
            DataTable dtx = null;

            string strPano = "";
            string strSName = "";
            string strSex = "";
            string strJumin = "";
            string strJumin1 = "";
            string strJumin2 = "";
            string strJumin3 = "";
            string strBirth = ""; //comfunc GetBirthDate                      
            string strExamCode = "";
            string strExamName = "";
            
            strPano = "H" + argHPano.ToString();
            if (argPtno !="")
            {
                strPano = argPtno;
            }

            #region HIC_PATIENT 체크
            dtx = sel_HicPatient(pDbCon, argHPano);
            if (dtx != null && dtx.Rows.Count > 0)
            {
                strSName = dtx.Rows[0]["SName"].ToString().Trim();
                strSex = dtx.Rows[0]["Sex"].ToString().Trim();
                strJumin = clsAES.DeAES(dtx.Rows[0]["Jumin2"].ToString().Trim());
                if (strJumin.Length ==13)
                {
                    strJumin1 = VB.Left(strJumin, 6);
                    strJumin2 = VB.Right(strJumin, 7);
                    strJumin3 = strJumin1 + "-" + VB.Left(strJumin2,3) + "****";
                    strBirth = ComFunc.GetBirthDate(strJumin1, strJumin2,"");
                }  

            }

            dtx = null;
            #endregion

            if (strBirth =="")
            {
                strBirth = "20000101";
            }

            dtx = sel_xrayPacsADT(pDbCon, strPano);
            if ( dtx.Rows.Count == 0)
            {
                //ADT
                #region 변수설정
                cXrayPacsADT cXrayPacsADT = new cXrayPacsADT();
                cXrayPacsADT.Patid = strPano;
                cXrayPacsADT.QUEUEID = argQUEUEID;
                cXrayPacsADT.EventType = "A04";
                cXrayPacsADT.BirthDay = strBirth;
                cXrayPacsADT.Dept = "HR";
                cXrayPacsADT.PatType = "O";
                cXrayPacsADT.PersonalID = strJumin3;
                cXrayPacsADT.Sex = strSex;
                #endregion
                SqlErr = ins_XRAY_PACS_ADT(pDbCon, cXrayPacsADT, ref intRowAffected);
                if (SqlErr != "")
                {
                    return false;
                }
            }

            #region 검사코드 체크
            dt = cxraySql.sel_HIC_EXCODE(pDbCon, argCode);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                strExamName = dt.Rows[0]["EName"].ToString().Trim() + " " + dt.Rows[0]["YName"].ToString().Trim();

                if (strExamName !="")
                {
                    cXrayPacsExam cXrayPacsExam = new cXrayPacsExam();
                    cXrayPacsExam.Job = "01";
                    cXrayPacsExam.Modality = "DX";
                    cXrayPacsExam.XCode = argCode;


                    cXrayPacsExam cXrayPacsExam2 = new cXrayPacsExam();
                    cXrayPacsExam2.Job = "00";
                    cXrayPacsExam2.Modality = "DX";
                    cXrayPacsExam2.XCode = argCode;
                    cXrayPacsExam2.ExamName = strExamName;


                    strExamCode = XRAY_Send_ExamCode_Search(pDbCon, cXrayPacsExam, cXrayPacsExam2);
                }
            }
            #endregion

            //XARY_PACS_ORDER
            #region 변수설정
            cXrayPacsOrder cXrayPacsOrder = new cXrayPacsOrder();
            cXrayPacsOrder.QUEUEID = argQUEUEID;
            cXrayPacsOrder.PacsNo = argPacsNo;
            cXrayPacsOrder.Patid = strPano;
            cXrayPacsOrder.JepDate = argDate;
            cXrayPacsOrder.DeptCode = "HR";
            cXrayPacsOrder.SName = strSName;
            cXrayPacsOrder.Sex = strSex;
            cXrayPacsOrder.Job = "NW";            
            cXrayPacsOrder.IPDOPD = "O";
            cXrayPacsOrder.Modality = "DX";
            cXrayPacsOrder.ExamRoom = ExamRoom;
            cXrayPacsOrder.ORDERDOC = "";
            cXrayPacsOrder.ExamCode = strExamCode;
            cXrayPacsOrder.ExamName = strExamName;
            cXrayPacsOrder.BirthDay = strBirth;


            #endregion
            SqlErr = ins_HIC_XRAY_PACS_ORDER(pDbCon, cXrayPacsOrder, ref intRowAffected);
            if (SqlErr != "")
            {
                return false;
            }

            return true;
        }         
        
        //팍스 XRAY_PACS_ORDER 생성
        //void XRAY_PACS_ORDER_INSERT(PsmhDb pDbCon,string argQUEUEID, string argExCode, string argExName, long argDrSabun)
        //{

        //    int intRowAffected = 0; //변경된 Row 받는 변수                       


        //    // clsTrans DT = new clsTrans();
        //    clsDB.setBeginTran(pDbCon);

        //    //생성
        //    //1.접수  2.접수취소 3.접수변경 4.촬영완료 5.판독완료 6.판독수정 7.판독삭제
        //    if (TPS.SendGbn.CompareTo("3") <= 0)
        //    {
        //        SQL = " INSERT INTO " + ComNum.DB_PACS + "XRAY_PACS_ORDER                                       \r\n";
        //        SQL += "  (QUEUEID,FALG,WORKTIME,PATID,ACDESSIONNO,                                             \r\n";
        //        SQL += "   EVENTTYPE,EXAMDATE,EXAMTIME,ExamRoom,EXAMCODE,EXAMNAME,                              \r\n";
        //        SQL += "   ORDERDOC,ORDERFROM,PATNAME,PATBIRTHDAY,PATSEX,PATDEPT,PATTYPE,                       \r\n";
        //        SQL += "   HISORDERID,WARD,ROOM,RequestMemo,Emergency,operator,DISEASE  ) VALUES                \r\n";
        //        SQL += "  ( '" + argQUEUEID + "','N',SYSDATE,                                                   \r\n";
        //        SQL += "  '" + TPS.Pano + "','" + TPS.PacsNo + "',                       \r\n";
        //        if (TPS.SendGbn == "1")
        //        {
        //            SQL += " 'NW',                                                                              \r\n";
        //        }
        //        else if (TPS.SendGbn == "2")
        //        {
        //            SQL += " 'CA',                                                                              \r\n";
        //        }
        //        else if (TPS.SendGbn == "3")
        //        {
        //            SQL += " 'NW',                                                                              \r\n";
        //        }
        //        SQL += "  '" + VB.Left(TPS.SeekDate, 8) + "',                                                    \r\n";
        //        if (VB.Len(TPS.SeekDate) > 8)
        //        {
        //            SQL += " '" + VB.Right(TPS.SeekDate, 4) + "',                                                \r\n";
        //        }
        //        else
        //        {
        //            SQL += " '', ";
        //        }

        //        SQL += "  '" + TPS.Resource + "','" + argExCode + "','" + argExName + "',    \r\n";
        //        SQL += "  '" + argDrSabun.ToString() + "','" + TPS.DeptCode + "','" + TPS.SName2 + "',    \r\n";
        //        SQL += "  '" + TPS.Birth + "','" + TPS.Sex + "','" + TPS.DeptCode + "',    \r\n";
        //        SQL += "  '" + TPS.IpdOpd + "','" + TPS.PacsNo + "','" + TPS.WardCode + "',    \r\n";

        //        SQL += "  '" + TPS.RoomCode + "','" + TPS.DrRemark + "','" + TPS.Emergency + "',    \r\n";
        //        SQL += "  '" + TPS.Operator + "','" + TPS.Disease + "'    \r\n";

        //        SQL += "  )                                                                                     \r\n";

        //        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
        //        if (SqlErr != "")
        //        {
        //            clsComSupXraySend.GstrPacsSend = "";

        //            clsDB.setRollbackTran(pDbCon);
        //            ComFunc.MsgBox(SqlErr);
        //            clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장                            
        //            return;
        //        }


        //    }
        //    else
        //    {
        //        if (TPS.XJong == "D")
        //        {
        //            ENDO_Reading_SET(pDbCon);
        //        }
        //        else
        //        {
        //            XRAY_Reading_SET(pDbCon);
        //        }


        //        TPS.Result = TPS.Result.Replace("$", "\r\n");


        //        SQL = " INSERT INTO " + ComNum.DB_PACS + "XRAY_PACS_REPORT                                       \r\n";
        //        SQL += "  (QUEUEID,FLAG,READSTAT,HISORDERID,PATID,CONCLUSION,                                            \r\n";
        //        SQL += "   REPORTDATE,REPORTTIME,CONFDR1  ) VALUES                \r\n";
        //        SQL += "  ( '" + argQUEUEID + "','N',                                                   \r\n";

        //        if (TPS.SendGbn == "5" || TPS.SendGbn == "6")
        //        {
        //            SQL += " 'C',                                                                              \r\n";
        //        }
        //        else if (TPS.SendGbn == "7")
        //        {
        //            SQL += " 'X',                                                                              \r\n";
        //        }
        //        else
        //        {
        //            SQL += " 'C',                                                                              \r\n";
        //        }

        //        SQL += "  '" + TPS.PacsNo + "','" + TPS.Pano + "','" + TPS.Result + "',                       \r\n";
        //        SQL += "  '" + VB.Left(TPS.ResultDate, 8) + "',                                                    \r\n";
        //        if (VB.Len(TPS.ResultDate) > 8)
        //        {
        //            SQL += " '" + VB.Right(TPS.ResultDate, 4) + "',                                                \r\n";
        //        }
        //        else
        //        {
        //            SQL += " '', ";
        //        }

        //        SQL += "  '" + argDrSabun.ToString() + "' \r\n";
        //        SQL += "  )                                                                                     \r\n";

        //        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
        //        if (SqlErr != "")
        //        {
        //            clsComSupXraySend.GstrPacsSend = "";

        //            clsDB.setRollbackTran(pDbCon);
        //            ComFunc.MsgBox(SqlErr);
        //            clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장                            
        //            return;
        //        }



        //    }

        //    clsDB.setCommitTran(pDbCon);

        //    clsComSupXraySend.GstrPacsSend = "OK";

        //}
        
        public string XRAY_Send_ExamCode_Search(PsmhDb pDbCon,cXrayPacsExam argCls, cXrayPacsExam argCls2)
        {
            DataTable dt = null;
            long nSeqno = 0;
            string strCode = "";
            int intRowAffected = 0;

            //자료유무체크 구분01
            dt = sel_XRAY_PACS_EXAM(pDbCon, argCls);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                return dt.Rows[0]["ExamCode"].ToString().Trim();                
            }
            else
            {
                //신규 구분00
                dt = sel_XRAY_PACS_EXAM(pDbCon, argCls2);
                if (ComFunc.isDataTableNull(dt) == false)
                {
                    if (dt.Rows[0]["ExamCode"].ToString().Trim()!="")
                    {
                        nSeqno =  Convert.ToInt32(VB.Right(dt.Rows[0]["ExamCode"].ToString().Trim(),3));                 
                    }                    

                    nSeqno++;

                    if (argCls2.ExamCode != "")
                    {
                        strCode = argCls2.ExamCode + "-" + ComFunc.SetAutoZero(nSeqno.ToString(), 3);
                    }
                    else
                    {
                        strCode = argCls2.XCode + "-" + ComFunc.SetAutoZero(nSeqno.ToString(), 3);
                    }
                    //변수재설정
                    argCls2.ExamCode = strCode;

                }

                SqlErr = ins_XRAY_PACS_EXAM(pDbCon, argCls2, ref intRowAffected);
                if (SqlErr == "" )
                {
                    return strCode;
                }
                else
                {
                    return "";
                }
                
            }
            
            
        }     

        //void XRAY_Reading_SET(PsmhDb pDbCon)
        //{
        //    DataTable dt = null;

        //    string strTemp = "";

        //    if (Convert.ToInt32(TPS.ReadNo) == 0)
        //    {
        //        dt = sel_XrayDetail(pDbCon, TPS.Pano, TPS.PacsNo);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            TPS.ReadNo = dt.Rows[0]["ExInfo"].ToString().Trim();
        //        }
        //    }


        //    dt = null;
        //    dt = sel_resultNew(pDbCon, TPS.ReadNo);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        TPS.ResultDate = dt.Rows[0]["ReadDate"].ToString().Trim();
        //        strTemp = dt.Rows[0]["Result"].ToString().Trim();
        //        strTemp += dt.Rows[0]["Result1"].ToString().Trim();

        //        TPS.ReadDrSabun = Convert.ToInt32(dt.Rows[0]["XDrCode1"].ToString().Trim());

        //    }

        //    TPS.Result = strTemp.Replace("\r\n", "$");

        //    //판독삭제
        //    if (TPS.SendGbn == "7") TPS.Result = "";

        //    TPS.ReadDrName = clsVbfunc.GetInSaName(pDbCon, TPS.ReadDrSabun.ToString()); //TODO 윤조연 BAS_USER 사용요망

        //}

        //void ENDO_Reading_SET(PsmhDb pDbCon)
        //{
        //    DataTable dt = null;

        //    string strGbJob = "";
        //    string strGbNEW = "";

        //    string[] strResult = new string[5];

        //    string strTemp = "";

        //    dt = sel_endoJupmst(pDbCon, TPS.Pano, TPS.ReadNo);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        strGbJob = dt.Rows[0]["GbJob"].ToString().Trim();
        //        strGbNEW = dt.Rows[0]["GbNew"].ToString().Trim();

        //        TPS.ReadNo = dt.Rows[0]["SeqNo"].ToString().Trim();
        //        TPS.ReadDrSabun = Convert.ToInt32(dt.Rows[0]["ResultDrCode"].ToString().Trim());
        //        TPS.ResultDate = dt.Rows[0]["ResultDate"].ToString().Trim();
        //    }

        //    dt = null;
        //    dt = sel_endoResult(pDbCon, TPS.ReadNo);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        strResult[0] = dt.Rows[0]["Remark1"].ToString().Trim();
        //        strResult[1] = dt.Rows[0]["Remark2"].ToString().Trim();
        //        strResult[2] = dt.Rows[0]["Remark3"].ToString().Trim();
        //        strResult[3] = dt.Rows[0]["Remark4"].ToString().Trim();
        //        strResult[4] = dt.Rows[0]["Remark5"].ToString().Trim();

        //        if (strGbJob == "1")
        //        {
        //            //기관지
        //            strTemp = "▶Vocal Cord:" + "\r\n" + strResult[0] + "\r\n" + "\r\n";
        //            strTemp += "▶Carina:" + "\r\n" + strResult[1] + "\r\n" + "\r\n";
        //            strTemp += "▶Bronchi:" + "\r\n" + strResult[2] + "\r\n" + "\r\n";
        //            strTemp += "▶EndoScopic Procedure:" + "\r\n" + strResult[3];

        //        }
        //        else if (strGbJob == "2")
        //        {
        //            //위
        //            strTemp = "▶Esophagus:" + "\r\n" + strResult[0] + "\r\n" + "\r\n";
        //            strTemp += "▶Stomach:" + "\r\n" + strResult[1] + "\r\n" + "\r\n";
        //            strTemp += "▶Duodenum:" + "\r\n" + strResult[2] + "\r\n" + "\r\n";
        //            strTemp += "▶Endoscopic Diagnosis:" + "\r\n" + strResult[3] + "\r\n" + "\r\n";
        //            strTemp += "▶Endoscopic Procedure:" + "\r\n" + strResult[4];

        //        }
        //        else if (strGbJob == "3")
        //        {
        //            //장
        //            if (strGbNEW == "Y")
        //            {
        //                strTemp = "▶Endoscopic Findings " + "\r\n" + "small Intestinal:" + strResult[0] + "\r\n" + "\r\n";
        //                strTemp += "large Intestinal:" + strResult[3] + "\r\n" + "\r\n";
        //                strTemp += "rectum:" + strResult[4] + "\r\n" + "\r\n";

        //                strTemp += "▶Endoscopic Diagnosis:" + "\r\n" + strResult[1] + "\r\n" + "\r\n";
        //                strTemp += "▶Endoscopic Procedure:" + "\r\n" + strResult[2];
        //            }
        //            else
        //            {
        //                strTemp = "▶Endoscopic Findings:" + "\r\n" + strResult[0] + "\r\n" + "\r\n";
        //                strTemp += "▶Endoscopic Diagnosis:" + "\r\n" + strResult[1] + "\r\n" + "\r\n";
        //                strTemp += "▶Endoscopic Procedure:" + "\r\n" + strResult[2];

        //            }

        //        }
        //        else if (strGbJob == "4")
        //        {
        //            //ERCP
        //            strTemp = "▶ERCP Finding:" + "\r\n" + strResult[0] + "\r\n" + "\r\n";
        //            strTemp += "▶Diagnosis:" + "\r\n" + strResult[1] + "\r\n" + "\r\n";
        //            strTemp += "▶Plan & Tx:" + "\r\n" + strResult[2] + "\r\n" + "\r\n";
        //            strTemp += "▶EndoScopic Procedure:" + "\r\n" + strResult[3];

        //        }

        //    }

        //    TPS.Result = strTemp.Replace("\r\n", "$");

        //    //판독삭제
        //    if (TPS.SendGbn == "7") TPS.Result = "";

        //    TPS.ReadDrName = clsVbfunc.GetInSaName(pDbCon, TPS.ReadDrSabun.ToString()); //TODO 윤조연 BAS_USER 사용요망

        //}

        public string Read_Hic_LtdName(PsmhDb pDbCon,string argCode)
        {         
            DataTable  dt = sel_HIC_LTD(pDbCon,"", argCode);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                return dt.Rows[0]["Name"].ToString().Trim();
                
            }
            else
            {
                return "";
            }

        }
        

        #region NON 트랜잭션 SQL 쿼리

        DataTable sel_XrayDetail(PsmhDb pDbCon,string argPano, string argPacsNo, long argOrderNo = 0, string argRowid = "")
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                  \r\n";
            SQL += "  TO_CHAR(EnterDate,'YYYY-MM-DD HH24:MI') EnterDate,    \r\n";
            SQL += "  TO_CHAR(BDate,'YYYY-MM-DD') BDate,                    \r\n";
            SQL += "  IPDOPD,TO_CHAR(SEEKDATE,'YYYY-MM-DD') SeekDate,       \r\n";
            SQL += "  PANO,SNAME,SEX,AGE,DEPTCODE,DRCODE,                   \r\n";
            SQL += "  WARDCODE,ROOMCODE,XSUBCODE,EXMORE,                    \r\n";
            SQL += "  GBEND,MGRNO,GBPORTABLE,                               \r\n";
            SQL += "  REMARK,XRAYROOM,GBNGT,DRREMARK,                       \r\n";
            SQL += "  ORDERNO,ORDERCODE,PACSNO,ORDERNAME,                   \r\n";
            SQL += "  XJong,Qty, XCODE,Exid,GbREAD,ExInfo,ROWID             \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                 \r\n";
            SQL += "  WHERE 1=1                                             \r\n";
            SQL += "   AND Pano ='" + argPano + "'                          \r\n";
            SQL += "   AND PacsNo ='" + argPacsNo + "'                      \r\n";

            if (argOrderNo != 0) SQL += "   AND OrderNo = " + argOrderNo + "  \r\n";
            if (argRowid != "") SQL += "   AND ROWID = '" + argRowid + "'   \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_XrayDetail_batch(PsmhDb pDbCon,string Job)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                  \r\n";
            SQL += "  Pano,PacsNo,HIC_WRTNO,HIC_CODE,                       \r\n";
            SQL += "  TO_CHAR(EnterDate,'YYYY-MM-DD') JepDate,              \r\n";
            SQL += "  ROWID,XJong,Qty,XCODE                                 \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                 \r\n";
            SQL += "  WHERE 1=1                                             \r\n";
            if (Job == "1")
            {
                //수가기준 자동처리
                SQL += "   AND SeekDate >=TRUNC(SYSDATE-1)                  \r\n";
                SQL += "   AND TRIM(XCode) IN (                             \r\n";
                SQL += "     SELECT TRIM(Code)                              \r\n";
                SQL += "      FROM " + ComNum.DB_PMPA + "BAS_BCODE          \r\n";
                SQL += "       WHERE GUBUN ='XRAY_자동상태완료수가'         \r\n";
                SQL += "       AND (DELDATE IS NULL OR DELDATE ='') )       \r\n";
                SQL += "   AND (CDate IS NULL OR CDate ='')                 \r\n";
                SQL += "   AND (DelDate IS NULL OR DelDate ='')             \r\n";
                SQL += "   AND (GbSTS IS NULL OR GbSTS <>'7')               \r\n";
                SQL += "   ORDER BY EnterDate,PacsNo                        \r\n";
            }
            else if (Job == "2")
            {
                //특정과 자동처리
                SQL += "   AND SeekDate >=TRUNC(SYSDATE-1)                  \r\n";
                SQL += "   AND DeptCode IN ('HR','TO','R6')                 \r\n";
                SQL += "   AND (CDate IS NULL OR CDate ='')                 \r\n";
                SQL += "   AND (DelDate IS NULL OR DelDate ='')             \r\n";
                SQL += "   AND (GbSTS IS NULL OR GbSTS <>'7')               \r\n";
                SQL += "   ORDER BY EnterDate,PacsNo                        \r\n";
            }
            else if (Job == "3")
            {
                //특정수가 자동처리 2012-12-24 -의뢰
                SQL += "   AND SeekDate >=TRUNC(SYSDATE-2)                  \r\n";
                SQL += "   AND (XCode IN                                    \r\n";
                SQL += "      ( 'G0400','G0400A','G04009','GR9701' )        \r\n";
                SQL += "       OR ( XCode ='US15' AND DeptCode <>'TO' ) )   \r\n";
                SQL += "   AND (GbEnd IS NULL OR GbEnd ='')                 \r\n";
                SQL += "   ORDER BY EnterDate,PacsNo                        \r\n";
            }
            else if (Job == "4")
            {
                //일반건진 HIC_XRAY_RESULT
                SQL += "   AND SeekDate >= TRUNC(SYSDATE-5)                 \r\n";
                SQL += "   AND DeptCode='HR'                                \r\n";
                SQL += "   AND PacsNo IS NOT NULL                           \r\n";
                SQL += "   AND GbEnd='1'                                    \r\n";
                SQL += "   AND XJong='1'                                    \r\n";//단순
                SQL += "   AND XSubCode='01'                                \r\n";//흉부
                SQL += "   AND ExInfo < 1000                                \r\n";
                SQL += "   AND GbHIC IS NULL                                \r\n";
                SQL += "   ORDER BY PacsNo                                  \r\n";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_resultNew(PsmhDb pDbCon,string argWRTNO)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                  \r\n";
            SQL += " TO_CHAR(ReadDate,'YYYYMMDD') ReadDate,                 \r\n";
            SQL += "  XDrCode1,Result,Result1,ROWID                         \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                 \r\n";
            SQL += "  WHERE 1=1                                             \r\n";
            SQL += "   AND WRTNO =" + Convert.ToInt32(argWRTNO) + "                          \r\n";


            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_basPatient(PsmhDb pDbCon,string argPano)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                  \r\n";
            SQL += "   SName,EName,Jumin1,Jumin2,Jumin3,Sex,                \r\n";
            SQL += "   TO_CHAR(Birth,'YYYYMMDD') Birth                      \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "BAS_PATIENT                 \r\n";
            SQL += "  WHERE 1=1                                             \r\n";
            SQL += "   AND Pano ='" + argPano + "'                          \r\n";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_hicPatient(PsmhDb pDbCon,string argPtno, long argPano = 0)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                  \r\n";
            SQL += "   SName,Pano,Ptno                                      \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "HIC_PATIENT                 \r\n";
            SQL += "  WHERE 1=1                                             \r\n";
            if (argPano == 0)
            {
                SQL += "   AND Ptno ='" + argPtno + "'                      \r\n";
            }
            else
            {
                SQL += "   AND Pano =" + argPano + "                        \r\n";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_mPatient(PsmhDb pDbCon,string argPano)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                  \r\n";
            SQL += "  PATID,EPatName                                        \r\n";
            SQL += " FROM " + ComNum.DB_PACS + "MPatient                    \r\n";
            SQL += "  WHERE 1=1                                             \r\n";
            SQL += "   AND PATID ='" + argPano + "'                         \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_HicPatient(PsmhDb pDbCon, long argPano)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                  \r\n";
            SQL += "  SName,Jumin2,Sex                                      \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "HIC_PATIENT                 \r\n";
            SQL += "  WHERE 1=1                                             \r\n";
            SQL += "   AND Pano ='" + argPano + "'                          \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_xrayPacsADT(PsmhDb pDbCon,string argPano)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                  \r\n";
            SQL += "  PATID                                                 \r\n";
            SQL += " FROM " + ComNum.DB_PACS + "XRAY_PACS_ADT               \r\n";
            SQL += "  WHERE 1=1                                             \r\n";
            SQL += "   AND PATID ='" + argPano + "'                         \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_XRAY_PACS_EXAM(PsmhDb pDbCon, cXrayPacsExam argCls)
        {
            DataTable dt = null;

            SQL = "";
            
            if (argCls.Job =="00")
            {
                SQL += "SELECT                                                  \r\n";
                SQL += "  MAX(ExamCode) ExamCode                                \r\n";
                SQL += " FROM " + ComNum.DB_PACS + "XRAY_PACS_EXAM              \r\n";
                SQL += "  WHERE 1=1                                             \r\n";
                if (argCls.ExamCode !="")
                {
                    SQL += "   AND ExamCode LIKE '" + argCls.ExamCode + "-%'    \r\n";
                }
                else
                {
                    SQL += "   AND ExamCode LIKE '" + argCls.XCode + "-%'       \r\n";
                }
                
                
            }
            else if (argCls.Job == "01")
            {
                SQL += "SELECT                                                  \r\n";
                SQL += "  ExamCode                                              \r\n";
                SQL += " FROM " + ComNum.DB_PACS + "XRAY_PACS_EXAM              \r\n";
                SQL += "  WHERE 1=1                                             \r\n";
                if (argCls.ExamCode != "")
                {
                    SQL += "   AND ExamCode LIKE '" + argCls.ExamCode + "-%'    \r\n";
                }
                if (argCls.XCode != "")
                {
                    SQL += "   AND ExamCode LIKE '" + argCls.XCode + "-%'       \r\n";
                }
                if (argCls.ExamName != "")
                {
                    SQL += "   AND ExamName = '" + argCls.ExamName + "'         \r\n";
                }
            }            
            

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_endoJupmst(PsmhDb pDbCon,string argPano, string argPacsNo)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                  \r\n";
            SQL += "  GbJob,SeqNo,ResultDrCode,GbNew,ROWID,                 \r\n";
            SQL += " TO_CHAR(ResultDate,'YYYYMMDD HH24:MI') ResultDate      \r\n";
            SQL += " FROM " + ComNum.DB_MED + "ENDO_JUPMST                  \r\n";
            SQL += "  WHERE 1=1                                             \r\n";
            SQL += "   AND PTno ='" + argPano + "'                          \r\n";
            SQL += "   AND PacsNo ='" + argPacsNo + "'                      \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_endoResult(PsmhDb pDbCon,string argReadNo)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                  \r\n";
            SQL += "  Remark1,Remark2,Remark3,Remark4,Remark5               \r\n";
            SQL += " FROM " + ComNum.DB_MED + "ENDO_RESULT                  \r\n";
            SQL += "  WHERE 1=1                                             \r\n";
            SQL += "   AND SeqNo =" + Convert.ToInt32(argReadNo) + "        \r\n";


            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_OCS_ORDERCODE(PsmhDb pDbCon, string argCode)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                  \r\n";
            SQL += "  OrderName,DispHeader                                  \r\n";
            SQL += " FROM " + ComNum.DB_MED + "OCS_ORDERCODE                \r\n";
            SQL += "  WHERE 1=1                                             \r\n";
            SQL += "   AND OrderCode ='" + argCode + "'                     \r\n";
            
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_XRAY_CODE(PsmhDb pDbCon, string argCode)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                  \r\n";
            SQL += "  XName                                                 \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "XRAY_CODE                   \r\n";
            SQL += "  WHERE 1=1                                             \r\n";
            SQL += "   AND XCode ='" + argCode + "'                         \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        public DataTable sel_HIC_LTD(PsmhDb pDbCon, string argJob, string argCode)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                  \r\n";
            SQL += "  Code,Name,SangHo                                      \r\n";
            SQL += "  ,ROWID                                                \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "HIC_LTD                     \r\n";
            SQL += "  WHERE 1=1                                             \r\n";
            if (argCode !="")
            {
                SQL += "   AND Code =" + argCode + "                        \r\n";
            }
            SQL += "  ORDER BY Code,Name                                    \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_hicJepsuResult(PsmhDb pDbCon,long argPano, string argJDate)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                                      \r\n";
            SQL += "  a.WRTNO,a.GjJong,b.ExCode                                                 \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "HIC_JEPSU a, " + ComNum.DB_PMPA + "HIC_RESULT b \r\n";
            SQL += "  WHERE 1=1                                                                 \r\n";
            SQL += "   AND a.Pano=" + argPano + "                                               \r\n";
            SQL += "   AND a.JepDate=TO_DATE('" + argJDate + "','YYYY-MM-DD')                   \r\n";
            SQL += "   AND a.DelDate IS NULL                                                    \r\n";
            SQL += "   AND a.WRTNO=b.WRTNO(+)                                                   \r\n";
            SQL += "   AND b.ExCode IN ('A142','TX13','TX14','TX11','A213','TX16','A211')       \r\n"; //흉부            
            SQL += "   ORDER BY a.WRTNO DESC                                                    \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_hicJepsu(PsmhDb pDbCon,long argWRTNO)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                  \r\n";
            SQL += "  a.GjJong,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,     \r\n";
            SQL += "  a.SName,a.Sex,a.Age,b.Pano,                           \r\n";
            SQL += "  b.LtdCode,b.PTno,a.JobSabun                           \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "HIC_JEPSU a  ,              \r\n";
            SQL += "  " + ComNum.DB_PMPA + "HIC_PATIENT b                   \r\n";
            SQL += "  WHERE 1=1                                             \r\n";
            SQL += "   AND WRTNO=" + argWRTNO + "                           \r\n";
            SQL += "   AND a.Pano=b.Pano(+)                                 \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_hicXrayResult(PsmhDb pDbCon,long argPano, string argJDate, string argPtno = "")
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                      \r\n";
            SQL += "  ROWID                                                     \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "HIC_XRAY_RESULT                 \r\n";
            SQL += "  WHERE 1=1                                                 \r\n";
            SQL += "   AND Pano=" + argPano + "                                 \r\n";
            SQL += "   AND JepDate=TO_DATE('" + argJDate + "' ,'YYYY-MM-DD')    \r\n";
            SQL += "   AND DelDate IS NULL                                      \r\n";
            SQL += "   AND GbPACS  IS NULL                                      \r\n";
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_hicSunapDtl(PsmhDb pDbCon,string Job, long argWRTNO)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                              \r\n";
            SQL += "  Code                                                              \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "HIC_SUNAPDTL                            \r\n";
            SQL += "  WHERE 1=1                                                         \r\n";
            SQL += "   AND WRTNO=" + argWRTNO + "                                       \r\n";
            if (Job == "1")
            {
                SQL += "   AND Code IN ( SELECT CODE FROM HIC_CODE WHERE GUBUN ='98' )  \r\n";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }


                

        #endregion

        #region 트랜잭션 쿼리 + enum INSERT, UPDATE,DELETE .... 

        /// <summary>
        /// 영상 테이블 상태 갱신
        /// </summary>
        /// <param name="Job"></param>
        /// <param name="strROWID"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string up_XRAY_DETAIL_STS(PsmhDb pDbCon, string Job, string strROWID, ref int intRowAffected)
        {
            string SqlErr = string.Empty;
            if (Job == "" )
            {
                return "오류";
            }

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_DETAIL SET   \r\n";
            if (Job == "1")
            {
                SQL += "     GbSTS ='7', CDate =  SYSDATE           \r\n";
            }
            else if (Job == "2")
            {
                SQL += "     GbEnd='1'                              \r\n";
            }
            else if (Job == "3")//검진
            {
                SQL += "     GbHic='Y',GbSTS ='7'                   \r\n";
            }
            else if (Job == "4")//검진2
            {
                SQL += "     GbHic='Y',GbSTS ='7',                  \r\n";
                SQL += "     CDate =SYSDATE                         \r\n";

            }
            else if (Job == "5")//검진333333
            {
                SQL += "     GbHic='N'                              \r\n";

            }

            SQL += "  WHERE 1=1                                     \r\n";
            SQL += "    AND ROWID = '" + strROWID + "'              \r\n";


            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
                

        /// <summary>
        /// 영상메인 테이블 상태 갱신에 사용 - up_XRAY_DETAIL_STS
        /// </summary>
        public enum enum_XrayDetail_STS { STS, Gubun, Pano, SeekDate, GbReserved, PacsNo, BDate, OrderNo, jobsabun, ROWID }

        /// <summary>
        /// 기능검사 접수/미접수시 영상 테이블 상태 갱신 - 기능검사 접수연동
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string up_XRAY_DETAIL_FnEx(PsmhDb pDbCon, string[] arg, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_DETAIL SET                                                       \r\n";
            if (arg[(int)enum_XrayDetail_STS.STS] == "A")
            {
                SQL += "     GBRESERVED ='7',GbSTS ='7', CDate =  SYSDATE                                               \r\n";
                if (arg[(int)enum_XrayDetail_STS.Gubun] == "3")
                {
                    SQL += "     ,PacsNo ='" + arg[(int)enum_XrayDetail_STS.PacsNo] + "'                                \r\n";
                }
                SQL += "     ,CSabun ='" + arg[(int)enum_XrayDetail_STS.jobsabun] + "'                                  \r\n";
                SQL += "     ,SeekDate =TO_DATE('" + arg[(int)enum_XrayDetail_STS.SeekDate] + "','YYYY-MM-DD HH24:MI')  \r\n";
            }
            else if (arg[(int)enum_XrayDetail_STS.STS] == "B")
            {
                SQL += "     GBRESERVED ='1',GbSTS ='0', CDate =  ''                                                    \r\n";
                SQL += "     ,RDate ='', Pacs_End =''                                                                   \r\n";
                SQL += "     ,SeekDate = EnterDate                                                                      \r\n";
            }

            SQL += "  WHERE 1=1                                                                                         \r\n";
            SQL += "    AND Pano = '" + arg[(int)enum_XrayDetail_STS.Pano] + "'                                         \r\n";
            SQL += "    AND OrderNo = " + Convert.ToInt32(arg[(int)enum_XrayDetail_STS.OrderNo]) + "                    \r\n";
            SQL += "    AND BDate = TO_DATE('" + arg[(int)enum_XrayDetail_STS.BDate] + "','YYYY-MM-DD')                 \r\n";

            if (arg[(int)enum_XrayDetail_STS.STS] == "A")
            {
                if (arg[(int)enum_XrayDetail_STS.Gubun] == "3") SQL += "    AND XJong ='C'                              \r\n";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;

        }

        /// <summary>
        /// 기능검사 접수,미접수 작업시 팍스 연동 관련
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string ins_XRAY_PACSSEND_FnEx(PsmhDb pDbCon, string[] arg, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            if (arg[(int)enum_XrayDetail_STS.Gubun] != "3" && arg[(int)enum_XrayDetail_STS.Gubun] != "22") return "";

            SQL = " INSERT INTO " + ComNum.DB_PMPA + "XRAY_PACSSEND                                         \r\n";
            SQL += "   (EntDate,PacsNo,SendGbn,Pano,SName,                                                  \r\n";
            SQL += "    Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode,                    \r\n";
            SQL += "    XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark,Operator,Gbinfo)                  \r\n";
            if (arg[(int)enum_XrayDetail_STS.STS] == "A")
            {
                SQL += "  SELECT SYSDATE,PacsNo,'1',Pano,SName,                                             \r\n";
            }
            else if (arg[(int)enum_XrayDetail_STS.STS] == "B")
            {
                SQL += "  SELECT SYSDATE,PacsNo,'2',Pano,SName,                                             \r\n";
            }

            SQL += "         Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XJong,XSubCode,               \r\n";
            SQL += "         XCode,OrderCode,SeekDate,Remark,XRayRoom,DrRemark,EXID, Gbinfo                 \r\n";
            SQL += "    FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                              \r\n";
            SQL += "     WHERE 1=1                                                                          \r\n";
            SQL += "      AND Pano='" + arg[(int)enum_XrayDetail_STS.Pano] + "'                             \r\n";
            SQL += "      AND BDATE =TO_DATE('" + arg[(int)enum_XrayDetail_STS.BDate] + "','YYYY-MM-DD')    \r\n";
            SQL += "      AND OrderNo = " + Convert.ToInt32(arg[(int)enum_XrayDetail_STS.OrderNo]) + "      \r\n";
            SQL += "      AND XJONG ='C'                                                                    \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
        
        public string up_hicXrayResult(PsmhDb pDbCon, string Job, long argPano, string argJDate, string argPacsNo, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "HIC_XRAY_RESULT               \r\n";
            if (Job == "1")
            {
                SQL += "    GbPACS = 'Y',                                       \r\n";
                SQL += "    XRAYNO='" + argPacsNo + "'                          \r\n";
            }
            SQL += "  WHERE 1=1                                 \r\n";
            SQL += "   AND Pano=" + argPano + "                                 \r\n";
            SQL += "   AND JepDate=TO_DATE('" + argJDate + "' ,'YYYY-MM-DD')    \r\n";
            SQL += "   AND DelDate IS NULL                                      \r\n";
            SQL += "   AND GbPACS  IS NULL                                      \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        

        public class cXrayPacsADT
        {
            public string QUEUEID = "";
            public string Patid = "";
            public string Flag = "";
            public string WorkTime = "";
            public string EventType = "";
            public string BirthDay = "";
            public string Dept = "";
            public string AttendDoct1 = "";
            public string PatName = "";
            public string PatType = "";
            public string PersonalID = "";
            public string Sex = "";
            public string EPatName = "";


        }

        public class cXrayPacsExam
        {
            public string Job = "";
            public string ExamCode = "";
            public string XCode = "";
            public string ExamName= "";
            public string ShortName = "";
            public string Modality = "";
            public string SectCode = "";
            public string INPS = "";
            public string INPT_DT = "";
            public string UPPS = "";
            public string UP_DT = "";

        }

        public class cXrayPacsOrder
        {
            public string Job = "";
            public string Flag = "";
            public string SName = "";
            public string IPDOPD = "";
            public long Pano = 0;
            public string HPano = "";
            //public string Ptno = "";
            public string Sex = "";
            public string JepDate = "";
            public string ExamRoom = "";
            public string Modality = "";
            public string JepTime = "";
            public string QUEUEID = "";
            public string Patid = "";
            public string PatType = "";          
            public string WorkTime = "";
            public string EventType = "";
            public string PacsNo = "";
            public string ExamCode = "";
            public string ExamName = "";
            public string ExamDate = "";
            public string ExamTime = "";
            public string ORDERDOC = "";
            public string DeptCode = "";
            public string DrCode = "";
            public string WardCode = "";
            public string RoomCode = "";
            public string BirthDay = "";
            public string RequestMemo = "";
            public string Emergency = "";
            public string Operator = "";
            public string DISEASE = "";
            public string INPS = "";
            public string INPT_DT = "";
            public string UPPS = "";
            public string UP_DT = "";

        }

        public class cXrayPacsReport
        {
            public string Job = "";
            public string QUEUEID = "";
            public string FLAG = "";
            public string READSTAT = "";
            public string HISORDERID = "";
            public string PATID = "";
            public string CONCLUSION = "";
            public string REPORTDATE = "";
            public string REPORTTIME = "";
            public string CONFDR1 = "";
        }

        public string ins_XRAY_PACS_ADT(PsmhDb pDbCon, cXrayPacsADT argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";

            SQL = " INSERT INTO " + ComNum.DB_PACS + "XRAY_PACS_ADT                                 \r\n";
            SQL += "  (QUEUEID,FLAG,WORKTIME,PATID,EVENTTYPE,BIRTHDAY                               \r\n";
            SQL += "   ,DEPT,ATTENDDOCT1,PATNAME,PATTYPE,PERSONALID,Sex,EPatName) VALUES            \r\n";
            SQL += "  ( '" + argCls.QUEUEID + "'                                                    \r\n";
            SQL += "  ,'N'                                                                          \r\n";
            SQL += "  ,SYSDATE                                                                      \r\n";
            SQL += "  ,'" + argCls.Patid + "'                                                       \r\n";
            SQL += "  ,'" + argCls.EventType + "'                                                   \r\n";
            SQL += "  ,'" + argCls.BirthDay + "'                                                    \r\n";
            SQL += "  ,'" + argCls.Dept + "'                                                        \r\n";
            SQL += "  ,'" + argCls.AttendDoct1 + "'                                                 \r\n";
            SQL += "  ,'" + argCls.PatName + "'                                                     \r\n";
            SQL += "  ,'" + argCls.PatType + "'                                                     \r\n";
            SQL += "  ,'" + argCls.PersonalID + "'                                                  \r\n";
            SQL += "  ,'" + argCls.Sex + "'                                                         \r\n";
            SQL += "  ,'" + argCls.EPatName + "'                                                    \r\n";
            SQL += "  )                                                                             \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_XRAY_PACS_EXAM(PsmhDb pDbCon, cXrayPacsExam argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";

            SQL = " INSERT INTO " + ComNum.DB_PACS + "XRAY_PACS_EXAM                                \r\n";
            SQL += "  (ExamCode,ExamName,ShortName,Modality,SectCode                                \r\n";
            SQL += "  ,INPS,INPT_DT)  VALUES                                                        \r\n";
            SQL += "  ( '" + argCls.ExamCode + "'                                                   \r\n";           
            SQL += "  ,'" + argCls.ExamName + "'                                                    \r\n";
            SQL += "  ,'" + argCls.ShortName + "'                                                   \r\n";
            SQL += "  ,'" + argCls.Modality + "'                                                    \r\n";
            SQL += "  ,'" + argCls.SectCode + "'                                                    \r\n";
            SQL += "  ,'" + argCls.INPS + "'                                                        \r\n";
            SQL += "  ,SYSDATE                                                                      \r\n";            
            SQL += "  )                                                                             \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_XRAY_PACS_ORDER(PsmhDb pDbCon, cXrayPacsOrder argCls, ref int intRowAffected)
        {

            string SqlErr = string.Empty;

            SQL = "";

            SQL = " INSERT INTO " + ComNum.DB_PACS + "XRAY_PACS_ORDER                               \r\n";
            SQL += "  (QUEUEID,FALG,WORKTIME,PATID,ACDESSIONNO                                      \r\n";
            SQL += "   ,EVENTTYPE,EXAMDATE,EXAMTIME,ExamRoom,EXAMCODE,EXAMNAME                      \r\n";
            SQL += "   ,ORDERDOC,ORDERFROM,PATNAME,PATBIRTHDAY                                      \r\n";
            SQL += "   ,PATSEX,PATDEPT,PATTYPE,HISORDERID,WARD,ROOM                                 \r\n";
            SQL += "   ,RequestMemo,Emergency,Operator,DISEASE                                      \r\n";
            SQL += "  ,INPS,INPT_DT)  VALUES                                                        \r\n";
            SQL += "  ( '" + argCls.QUEUEID + "'                                                    \r\n";
            SQL += "  ,'N'                                                                          \r\n";
            SQL += "  ,SYSDATE                                                                      \r\n";
            SQL += "  ,'" + argCls.Patid + "'                                                       \r\n";
            SQL += "  ,'" + argCls.PacsNo + "'                                                      \r\n";
            SQL += "  ,'" + argCls.EventType + "'                                                   \r\n";
            SQL += "  ,'" + argCls.ExamDate + "'                                                    \r\n";
            SQL += "  ,'" + argCls.ExamTime + "'                                                    \r\n";
            SQL += "  ,'" + argCls.ExamRoom + "'                                                    \r\n";
            SQL += "  ,'" + argCls.ExamCode + "'                                                    \r\n";
            SQL += "  ,'" + argCls.ExamName + "'                                                    \r\n";
            SQL += "  ,'" + argCls.ORDERDOC + "'                                                    \r\n";
            SQL += "  ,'" + argCls.DeptCode + "'                                                    \r\n";
            SQL += "  ,'" + argCls.SName + "'                                                       \r\n";
            SQL += "  ,'" + argCls.BirthDay + "'                                                    \r\n";
            SQL += "  ,'" + argCls.Sex + "'                                                         \r\n";
            SQL += "  ,'" + argCls.DeptCode + "'                                                    \r\n";
            SQL += "  ,'" + argCls.IPDOPD + "'                                                      \r\n";
            SQL += "  ,'" + argCls.PacsNo + "'                                                      \r\n";
            SQL += "  ,'" + argCls.WardCode + "'                                                    \r\n";
            SQL += "  ,'" + argCls.RoomCode + "'                                                    \r\n";
            SQL += "  ,'" + argCls.RequestMemo + "'                                                 \r\n";
            SQL += "  ,'" + argCls.Emergency + "'                                                   \r\n";
            SQL += "  ,'" + argCls.Operator + "'                                                    \r\n";
            SQL += "  ,'" + argCls.DISEASE + "'                                                     \r\n";
            SQL += "  ,'" + argCls.INPS + "'                                                        \r\n";
            SQL += "  ,SYSDATE                                                                      \r\n";
            SQL += "  )                                                                             \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_HIC_XRAY_PACS_ORDER(PsmhDb pDbCon, cXrayPacsOrder argCls, ref int intRowAffected)
        {

            string SqlErr = string.Empty;

            SQL = "";

            SQL = " INSERT INTO " + ComNum.DB_PACS + "XRAY_PACS_ORDER                               \r\n";
            SQL += "  (QUEUEID,FALG,WORKTIME,PATID,ACDESSIONNO                                      \r\n";
            SQL += "   ,EVENTTYPE,EXAMDATE,EXAMTIME,ExamRoom,EXAMCODE,EXAMNAME                      \r\n";
            SQL += "   ,ORDERDOC,ORDERFROM,PATNAME,PATBIRTHDAY                                      \r\n";
            SQL += "   ,PATSEX,PATDEPT,PATTYPE,HISORDERID,WARD,ROOM ) VALUES                        \r\n";
            SQL += "  ( '" + argCls.QUEUEID + "'                                                    \r\n";
            SQL += "  ,'N'                                                                          \r\n";
            SQL += "  ,SYSDATE                                                                      \r\n";
            SQL += "  ,'" + argCls.Patid + "'                                                       \r\n";
            SQL += "  ,'" + argCls.PacsNo + "'                                                      \r\n";
            SQL += "  ,'" + argCls.Job + "'                                                         \r\n";
            SQL += "  ,'" + argCls.JepDate + "'                                                     \r\n";
            SQL += "  ,'" + argCls.JepTime + "'                                                     \r\n";
            SQL += "  ,'" + argCls.ExamRoom + "'                                                    \r\n";
            SQL += "  ,'" + argCls.ExamCode + "'                                                    \r\n";
            SQL += "  ,'" + argCls.ExamName + "'                                                    \r\n";
            SQL += "  ," + argCls.ORDERDOC + "                                                       \r\n";
            SQL += "  ,'" + argCls.DeptCode + "'                                                    \r\n";
            SQL += "  ,'" + argCls.SName + "'                                                       \r\n";
            SQL += "  ,'" + argCls.BirthDay + "'                                                    \r\n";
            SQL += "  ,'" + argCls.Sex + "'                                                         \r\n";
            SQL += "  ,'" + argCls.DeptCode + "'                                                    \r\n";
            SQL += "  ,'" + argCls.IPDOPD + "'                                                        \r\n";
            SQL += "  ,'" + argCls.PacsNo + "'                                                      \r\n";
            SQL += "  ,'" + argCls.WardCode + "'                                                    \r\n";
            SQL += "  ,'" + argCls.RoomCode + "'                                                    \r\n";
            SQL += "  )                                                                             \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_XRAY_PACS_REPORT(PsmhDb pDbCon, cXrayPacsReport argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";

            
            SQL = " INSERT INTO " + ComNum.DB_PACS + "XRAY_PACS_REPORT                  \r\n";
            SQL += "  (QUEUEID,FLAG,READSTAT,HISORDERID,PATID,CONCLUSION                \r\n";
            SQL += "   ,REPORTDATE,REPORTTIME,CONFDR1) VALUES                           \r\n";
            SQL += "  ( '" + argCls.QUEUEID + "'                                        \r\n";                        
            SQL += "  ,'" + argCls.FLAG + "'                                            \r\n";
            SQL += "  ,'" + argCls.READSTAT + "'                                        \r\n";
            SQL += "  ,'" + argCls.HISORDERID + "'                                      \r\n";
            SQL += "  ,'" + argCls.PATID + "'                                           \r\n";
            SQL += "  ,'" + argCls.CONCLUSION + "'                                      \r\n";
            SQL += "  ,'" + argCls.REPORTDATE + "'                                      \r\n";
            SQL += "  ,'" + argCls.REPORTTIME + "'                                      \r\n";
            SQL += "  ,'" + argCls.CONFDR1 + "'                                         \r\n";            
            SQL += "  )                                                                 \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
                
        #endregion

    }
}
