using System;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using System.Data;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray 
    /// File Name       : clsComSupXrayRead.cs
    /// Description     : 진료지원 공통 영상의학과 판독관련 class
    /// Author          : 윤조연
    /// Create Date     : 2017-06-20
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>    
    public class clsComSupXrayRead
    {
        clsQuery Query = new clsQuery();
        clsComSupXraySQL cxraySql = new clsComSupXraySQL();

        string SQL = "";
        string SqlErr = ""; //에러문 받는 변수
        public string[] argstr = null; //쿼리에 사용될 변수 배열값 

        #region //함수 및 기타

        public class cXray_Read
        {
            public string Job = "";
            public string Gubun1 = ""; //1.촬영일자별 2.등록번호별
            public string Gubun2 = ""; //
            public string Gubun3 = ""; //심초음파 자료가져오기 구분 NULL, E
            public string STS01 = ""; //판독상태
            public string ReadDrSabun = "";
            public string ReadDrName = "";

            public string Pano = "";
            public string SName = "";
            public string Age = "";
            public string Sex = "";
            public string Jumin = "";
            public string SeekDate = "";
            public string ReadDate = "";
            public long WRTNO = 0;
            public long EMGWRTNO = 0;
            public long HPANO = 0;
            public string DeptCode = "";
            public string DrCode = "";
            public string XJong = "";
            public string XCode = "";
            public string XName = "";
            public string Approve = "";
            public int PrintCnt = 0;
            public string DrName = "";
            public string GbIO = "";
            public string WardCode = "";
            public string RoomCode = "";
            public int SelCnt = 0;
            public string PacsNo = "";
            public string Uid = "";
            public string Remark = "";
            public string DrRemark = "";
            public string ROWID_detail = "";
            public string ROWID_resultnew = "";
            public string SelROWID = "";            

        }

        public class cXray_Read_Delegate
        {
            public string Job = "";
            public string STS = "";
            public long Row = 0;
            public string sPos = "";
            public long nPos = 0;
            public string Bun = "";
            public string BunName = "";
            public string Sogen = "";
            public string ResultEC = "";            

        }

        public cXray_Read Set_Xray_Read(string Job)
        {
            //11.촬영일자별 + 판독권한 12.등록번호별+판독권한 
            //21.촬영일자별별 + VIEW 22.등록번호별 + VIEW

            cXray_Read c = new cXray_Read();
            c.Job = Job;
            //if ( VB.Left(Job,1) == "1" )
            //{
            //    c.Gubun1 = "1";
            //}
            //else if (VB.Left(Job, 1) == "2")
            //{
            //    c.Gubun1 = "2";
            //}
            //else
            //{
            //    c.Gubun1 = "";
            //}
            c.Gubun1 = "1"; //기본 촬영일자별 조회구분
            c.Gubun2 = Job; //파트

            c.ReadDrName = clsType.User.UserName;
            c.ReadDrSabun = clsType.User.Sabun;

            return c;
        }

        public cXray_Read Set_Xray_Read_Hic(string Job)
        {            
            //HIC, HEA

            cXray_Read c = new cXray_Read();
            c.Job = Job;            
            c.ReadDrName = clsType.User.UserName;
            c.ReadDrSabun = clsType.User.Sabun;

            return c;
        }

        public cXray_Read Set_Xray_Read_Dept(string Job)
        {
            //  EC,UR,..
           
            cXray_Read c = new cXray_Read();
            c.Job = Job;
            //c.Gubun1 = clsComSup.setP(Job.Trim(), ".", 1).Trim(); //조회구분
            //c.Gubun2 = clsComSup.setP(Job.Trim(), ".", 2).Trim(); //파트
            c.Gubun1 = "1"; //기본 촬영일자별 조회구분
            c.Gubun2 = Job; //파트

            c.ReadDrName = clsType.User.UserName;
            c.ReadDrSabun = clsType.User.Sabun;

            return c;
        }

        public class XrayReadSet
        {
            public bool Link_YN= false;     //팍스연동 여부
            public bool Link_Set1 = false;  //연동 set1
            public bool Link_Set2 = false;  //연동 set2
            public bool Link_Set3 = false;  //연동 set3

        }

        public bool XRAY_RePanjeng_Check(PsmhDb pDbCon, string argPano)
        {
            DataTable dt = Query.Get_BasBcode(pDbCon, "XRAY_재판정대상자", argPano, " Code,JDate ", " AND JDate >=TRUNC(SYSDATE) ");
            if (ComFunc.isDataTableNull(dt) == false)
            {
                return true;
            }
            else
            {
                return false;
            }                        
        }

        public bool XRAY_Panjeng_Check(PsmhDb pDbCon,string argDate,string argPano, string argJong)
        {
            DataTable dt = null;
            if (argJong =="11" || argJong == "12" || argJong == "13" || argJong == "14" || argJong == "41" || argJong == "42" || argJong == "43")
            {
                dt = sel_HIC_JEPSU(pDbCon, "HIC_RES_BOHUM1", argDate, argPano);
                if (ComFunc.isDataTableNull(dt) == false)
                {
                    if (dt.Rows[0]["BPANJENGDRNO"].ToString().Trim() != "")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    
                }
                else
                {
                    return false;
                }
            }
            else if (argJong == "21")
            {
                dt = sel_HIC_JEPSU(pDbCon, "HIC_RES_SPECIAL", argDate, argPano);
                if (ComFunc.isDataTableNull(dt) == false)
                {
                    if (dt.Rows[0]["BPANJENGDRNO"].ToString().Trim() != "")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }
            }
            else if (argJong == "56")
            {
                dt = sel_HIC_JEPSU(pDbCon, "HIC_SCHOOL_NEW", argDate, argPano);
                if (ComFunc.isDataTableNull(dt) == false)
                {
                    if (dt.Rows[0]["BPANJENGDRNO"].ToString().Trim() != "")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }
            }
            else if (argJong == "83")
            {
                dt = sel_HEA_JEPSU(pDbCon, argDate, argPano);
                if (ComFunc.isDataTableNull(dt) == false)
                {
                    if (dt.Rows[0]["BPANJENGDRNO"].ToString().Trim() != "")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            
        }

        public bool XRAY_Hic_Send_Check(PsmhDb pDbCon, string argDept, string argPano, string argSeek, string argJong,string argXCode,string xrayCodes)
        {
            if (argDept !="TO" && argDept !="HR")
            {
                return false;
            }
            else
            {

                DataTable dt = null;
                string strDate1 = Convert.ToDateTime(argSeek).AddDays(-20).ToShortDateString();
                string strDate2 = argSeek;
                string strExCode = VB.STRCUT(xrayCodes,"{@}"+ argXCode + "{}","{@}");
                if (strExCode =="")
                {
                    strExCode = VB.STRCUT(xrayCodes, "{@}[" + argJong + "]{}", "{@}");
                }

                if (strExCode =="")
                {
                    return false;
                }

                if (argDept =="TO")
                {
                    dt = sel_HEA_RESULT(pDbCon, argPano, strDate1, strDate2, strExCode);

                }
                else
                {
                    dt = sel_HIC_RESULT(pDbCon, argPano, strDate1, strDate2, strExCode);
                }

                if (ComFunc.isDataTableNull(dt) == false)
                {
                    string s = string.Empty;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        s += dt.Rows[0]["Result"].ToString().Trim(); ;
                    }

                    if (s !="")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    
                }
                else
                {
                    return false;
                }


                
            }
        }

        public string Xray_Read_EC(string argResult,string argDrRemark)
        {
            string[] s2 = new string[41];
            int j = 1;
            int k = 1;
            int l = 0;

            string s = argResult.Trim();

            #region //텍스값 분리
            for (int i = 1; i <= s.Length; i++)
            {
                if (VB.Mid(s, i, 2) == "#$")
                {
                    s2[k - 1] = VB.Mid(s, j, l) == "#$" ? "" : VB.Mid(s, j, l);
                    

                    i = i + 2;
                    j = i;
                    k++;
                    l = 0;

                }
                else if (VB.Mid(s, i, 2) == "$%")
                {
                    l = s.Length;
                }

                l++;
            }
            #endregion
             
            #region // 결과 문구 형성
            s = "";
            s += "※신장 / 체중 : " + s2[0] + " / " + s2[1] + "    ※ BSA : " + s2[37] + "    ※ 혈압 : " + s2[38] + "   ※ 맥박 : " + s2[39] + "\r\n" + "\r\n";
            s += "※Clinical information ; " + "\r\n" + "  " +  argDrRemark + "\r\n" + "\r\n";
            //s += VB.Left("※2D - IVS/LVPW : " + s2[2] + "㎜" + VB.Space(40), 37) + VB.Left("※M-Mode - IVS/LVPW : " + s2[9] + "㎜" + VB.Space(40), 37) + "\r\n";
            //s += VB.Left("       LVEDD/LVESD : " + s2[3] + "㎜" + VB.Space(40), 37) + VB.Left("            LVEDD/LVESD : " + s2[10] + "㎜" + VB.Space(40), 37) + "\r\n";
            //s += VB.Left("       Aorta/LAD : " + s2[4] + "㎜" + VB.Space(40), 37) + VB.Left("            Aorta/LAD : " + s2[11] + "㎜" + VB.Space(40), 37) + "\r\n";
            //s += VB.Left("       LVEDV : " + s2[5] + "㎖" + VB.Space(40), 37) + VB.Left("              EF : " + s2[12] + "%" + VB.Space(40), 37) + "\r\n";

            //2021-09-02
            //s += VB.Left("※2D - IVS/LVPW : " + s2[2] + "㎜" + VB.Space(40), 37) + VB.Left("※M-Mode - LVOT diameter : " + s2[9] + "㎜" + VB.Space(40), 37) + "\r\n";
            //s += VB.Left("       LVEDD/LVESD : " + s2[3] + "㎜" + VB.Space(40), 37) + VB.Left("            Ascending AO : " + s2[10] + "㎜" + VB.Space(40), 37) + "\r\n";
            //s += VB.Left("       Aorta/LAD : " + s2[4] + "㎜" + VB.Space(40), 37) + "\r\n";
            //s += VB.Left("       LVEDV : " + s2[5] + "㎖" + VB.Space(40), 37) + "\r\n";
            //s += VB.Left("       LVESV : " + s2[6] + "㎖" + VB.Space(40), 37) + "\r\n";
            //s += VB.Left("       EF(Simpson's Method) : " + s2[7] + "%" + VB.Space(40), 37) + "\r\n";
            //s += VB.Left("       LAVolume : " + s2[8] + "㎖" + VB.Space(40), 37) + "\r\n" + "\r\n";

            s += VB.Left("※2D/M-Mode" + VB.Space(40), 33) + "\r\n";
            s += VB.Left("    IVS/LVPW : " + s2[2] + "㎜" + VB.Space(40), 33) + VB.Left("    LVEDV : " + s2[5] + "㎖" + VB.Space(40), 33) + "\r\n";
            s += VB.Left("    LVEDD/LVESD : " + s2[3] + "㎜" + VB.Space(40), 33) + VB.Left("    LVESV : " + s2[6] + "㎖" + VB.Space(40), 33) + "\r\n";
            s += VB.Left("    Aorta/LAD : " + s2[4] + "㎜" + VB.Space(40), 33) + VB.Left("    EF(Simpson's Method) : " + s2[7] + "%" + VB.Space(40), 33) + "\r\n";
            s += VB.Left("    Ascending AO : " + s2[10] + "㎜" + VB.Space(40), 33) + VB.Left("    LAVolume : " + s2[8] + "㎖" + VB.Space(40), 33) + "\r\n" + "\r\n";

            s += "※Diastolic functione - E/A Ratio : " + s2[13] + "" + "\r\n";
            s += "                      - DT : " + s2[14] + "sec" + "\r\n";
            s += "                      - E/E` : " + s2[15] + "" + "\r\n" + "\r\n";
            s += "※Echocadiographic Finding ; " + "\r\n" + s2[16] + "" + "\r\n" + "\r\n";
            s += "※Conclusion ; " + "\r\n" + s2[36] + "\r\n";
            #endregion
            
            return s;
        }

        public string Xray_Read_EC2(string[] argResult, string argDrRemark)
        {
            int i = 0;
            string[] s2 = new string[8] ;            
            string s = string.Empty;

            #region //텍스값 체크
            if ( clsComSup.setP(argResult[0],"^&^",1).Trim() == "Y")
            {
                s2[0] += "■DM ";
            }
            else
            {
                s2[0] += "□DM ";
            }
            if (clsComSup.setP(argResult[0], "^&^", 2).Trim() == "Y")
            {
                s2[0] += "■HTN ";
            }
            else
            {
                s2[0] += "□HTN ";
            }
            if (clsComSup.setP(argResult[0], "^&^", 3).Trim() == "Y")
            {
                s2[0] += "■IHD ";
            }
            else
            {
                s2[0] += "□IHD ";
            }
            if (clsComSup.setP(argResult[0], "^&^", 4).Trim() == "Y")
            {
                s2[0] += "■CVD ";
            }
            else
            {
                s2[0] += "□CVD ";
            }
            if (clsComSup.setP(argResult[0], "^&^", 5).Trim() == "Y")
            {
                s2[0] += "■SMOKING ";
            }
            else
            {
                s2[0] += "□SMOKING ";
            }
            if (clsComSup.setP(argResult[0], "^&^", 6).Trim() == "Y")
            {
                s2[0] += "■ALCOHOL ";
            }
            else
            {
                s2[0] += "□ALCOHOL ";
            }

            for ( i =1; i < 8; i++)
            {
                s2[i] = argResult[i];
            }
            


            #endregion

            #region // 결과 문구 형성
            s = "";
            s += "위험인자" + "\r\n";
            s += s2[0] + "\r\n";
            s += "-----------------------------------------------------------------------------" + "\r\n";
            s += VB.Space(20) + "Rt Carotid Artery" + VB.Space(5) + "Lt Carotid Artery" + "\r\n";
            s += VB.Space(20) + "CCA" + VB.Space(3) + "ICA" + VB.Space(3) + "ECA" + VB.Space(7) + "CCA" + VB.Space(3) +"ICA" + VB.Space(3) + "ECA" + VB.Space(3) + "\r\n";
            s += "-----------------------------------------------------------------------------" + "\r\n";
            s += "  IMT       Max  " + VB.Space(3) +  VB.Left(clsComSup.setP(s2[1],"^&^",1).Trim() + VB.Space(7),7) + VB.Left(clsComSup.setP(s2[1], "^&^", 2).Trim() + VB.Space(7), 7) + VB.Left(clsComSup.setP(s2[1], "^&^", 3).Trim() + VB.Space(7), 7) + VB.Left(clsComSup.setP(s2[1], "^&^", 4).Trim() + VB.Space(7), 7)+ VB.Left(clsComSup.setP(s2[1], "^&^", 5).Trim() + VB.Space(7), 7) + VB.Left(clsComSup.setP(s2[1], "^&^", 6).Trim() + VB.Space(7), 7) + "\r\n";

            s += "  (mm)      Mean " + VB.Space(3) + VB.Left(clsComSup.setP(s2[2], "^&^", 1).Trim() + VB.Space(7), 7) + VB.Left(clsComSup.setP(s2[2], "^&^", 2).Trim() + VB.Space(7), 7) + VB.Left(clsComSup.setP(s2[2], "^&^", 3).Trim() + VB.Space(7), 7) + VB.Left(clsComSup.setP(s2[2], "^&^", 4).Trim() + VB.Space(7), 7) + VB.Left(clsComSup.setP(s2[2], "^&^", 5).Trim() + VB.Space(7), 7) + VB.Left(clsComSup.setP(s2[2], "^&^", 6).Trim() + VB.Space(7), 7) + "\r\n";

            s += "Velocity    PSV  " + VB.Space(3) + VB.Left(clsComSup.setP(s2[3], "^&^", 1).Trim() + VB.Space(7), 7) + VB.Left(clsComSup.setP(s2[3], "^&^", 2).Trim() + VB.Space(7), 7) + VB.Left(clsComSup.setP(s2[3], "^&^", 3).Trim() + VB.Space(7), 7) + VB.Left(clsComSup.setP(s2[3], "^&^", 4).Trim() + VB.Space(7), 7) + VB.Left(clsComSup.setP(s2[3], "^&^", 5).Trim() + VB.Space(7), 7) + VB.Left(clsComSup.setP(s2[3], "^&^", 6).Trim() + VB.Space(7), 7) + "\r\n";

            s += "(cm/sec)    EDV  " + VB.Space(3) + VB.Left(clsComSup.setP(s2[4], "^&^", 1).Trim() + VB.Space(7), 7) + VB.Left(clsComSup.setP(s2[4], "^&^", 2).Trim() + VB.Space(7), 7) + VB.Left(clsComSup.setP(s2[4], "^&^", 3).Trim() + VB.Space(7), 7) + VB.Left(clsComSup.setP(s2[4], "^&^", 4).Trim() + VB.Space(7), 7) + VB.Left(clsComSup.setP(s2[4], "^&^", 5).Trim() + VB.Space(7), 7) + VB.Left(clsComSup.setP(s2[4], "^&^", 6).Trim() + VB.Space(7), 7) + "\r\n";

            s += "Plaque     유/무 " + VB.Space(3) + VB.Left(clsComSup.setP(s2[5], "^&^", 1).Trim() + VB.Space(7), 7) + VB.Left(clsComSup.setP(s2[5], "^&^", 2).Trim() + VB.Space(7), 7) + VB.Left(clsComSup.setP(s2[5], "^&^", 3).Trim() + VB.Space(7), 7) + VB.Left(clsComSup.setP(s2[5], "^&^", 4).Trim() + VB.Space(7), 7) + VB.Left(clsComSup.setP(s2[5], "^&^", 5).Trim() + VB.Space(7), 7) + VB.Left(clsComSup.setP(s2[5], "^&^", 6).Trim() + VB.Space(7), 7) + "\r\n";

            s += "-----------------------------------------------------------------------------" ;
            s += "\r\n" + "\r\n" + "COMMENTS" + "\r\n";
            s += s2[7].Replace("^&^","") + "\r\n";

            #endregion

            return s;
        }

        public string Xray_Read_EC3(string[] argResult, string argDrRemark)
        {
            //int i = 0;
            string[] s2 = argResult;
            string s = string.Empty;

            #region //텍스값 체크

            s2[0] = s2[0].Replace(";", "");
            s2[1] = s2[1].Replace(";", "");
            s2[2] = s2[2].Replace(";", "");
            s2[3] = s2[3].Replace(";", "");
            s2[4] = s2[4].Replace(";", "");
            s2[5] = s2[5].Replace(";", "");
            s2[6] = s2[6].Replace(";", "");
            s2[7] = s2[7].Replace(";", "");
            s2[8] = s2[8].Replace(";", "");
            s2[9] = s2[9].Replace(";", "");


            #endregion

            #region // 결과 문구 형성
            //s = "";
            //s += "Head-up Tilt Test" + "\r\n";
            //s += "-----------------------------------------------------------------------------" + "\r\n";
            //s +=  VB.Space(1) + "Procedure" + VB.Space(10) + "Time" + VB.Space(5) + "SBP" + VB.Space(5) + "DBP" + VB.Space(5) + "HR(bpm)" + VB.Space(5) + "Sx" + "\r\n";
            //s += "-----------------------------------------------------------------------------" + "\r\n";
            //s += VB.Space(1) + "Baseline Supine" + VB.Space(14) + VB.Left(clsComSup.setP(s2[0],"^&^",1).Trim() + VB.Space(8),8) + VB.Left(clsComSup.setP(s2[0], "^&^", 2).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[0], "^&^", 3).Trim() + VB.Space(10), 10) + "\r\n";
            //s += "-----------------------------------------------------------------------------" + "\r\n";
            //s += VB.Space(19) + "0  min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[1], "^&^", 1).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[1], "^&^", 2).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[1], "^&^", 3).Trim() + VB.Space(10), 10) + "\r\n";
            //s += VB.Space(19) + "5  min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[1], "^&^",5).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[1], "^&^", 6).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[1], "^&^", 7).Trim() + VB.Space(10), 10) + "\r\n";
            //s += VB.Space(3) + "HUT 60" + VB.Space(10) + "10 min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[1], "^&^", 9).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[1], "^&^", 10).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[1], "^&^", 11).Trim() + VB.Space(10), 10) + "\r\n";
            //s += VB.Space(19) + "15 min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[1], "^&^", 13).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[1], "^&^", 14).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[1], "^&^", 15).Trim() + VB.Space(10), 10) + "\r\n";
            //s += VB.Space(19) + "20 min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[1], "^&^", 17).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[1], "^&^", 18).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[1], "^&^", 19).Trim() + VB.Space(10), 10) + "\r\n";
            //s += "-----------------------------------------------------------------------------" + "\r\n";
            //s += VB.Space(19) + "0  min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[2], "^&^", 1).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[2], "^&^", 2).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[2], "^&^", 3).Trim() + VB.Space(10), 10) + "\r\n";
            //s += VB.Space(3) + "HUT 70" + VB.Space(10) + "5  min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[2], "^&^", 5).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[2], "^&^", 6).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[2], "^&^", 7).Trim() + VB.Space(10), 10) + "\r\n";
            //s += VB.Space(19) + "10 min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[2], "^&^", 9).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[2], "^&^", 10).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[2], "^&^", 11).Trim() + VB.Space(10), 10) + "\r\n";
            //s += "-----------------------------------------------------------------------------" + "\r\n";
            //s += VB.Space(19) + "0  min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[3], "^&^", 1).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[3], "^&^", 2).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[3], "^&^", 3).Trim() + VB.Space(10), 10) + "\r\n";
            //s += VB.Space(3) + "supine" + VB.Space(10) + "5  min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[3], "^&^", 5).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[3], "^&^", 6).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[3], "^&^", 7).Trim() + VB.Space(10), 10) + "\r\n";
            //s += VB.Space(19) + "10 min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[3], "^&^", 9).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[3], "^&^", 10).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[3], "^&^", 11).Trim() + VB.Space(10), 10) + "\r\n";
            //s += "-----------------------------------------------------------------------------" + "\r\n";
            //s += VB.Space(19) + "0  min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[4], "^&^", 1).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[4], "^&^", 2).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[4], "^&^", 3).Trim() + VB.Space(10), 10) + "\r\n";
            //s += VB.Space(19) + "2  min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[4], "^&^", 5).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[4], "^&^", 6).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[4], "^&^", 7).Trim() + VB.Space(10), 10) + "\r\n";
            //s += VB.Space(3) + "NIG 1/2T" + VB.Space(8) + "4  min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[4], "^&^", 9).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[4], "^&^", 10).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[4], "^&^", 11).Trim() + VB.Space(10), 10) + "\r\n";
            //s += VB.Space(19) + "6  min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[4], "^&^", 13).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[4], "^&^", 14).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[4], "^&^", 15).Trim() + VB.Space(10), 10) + "\r\n";
            //s += VB.Space(19) + "8  min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[4], "^&^", 17).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[4], "^&^", 18).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[4], "^&^", 19).Trim() + VB.Space(10), 10) + "\r\n";
            //s += VB.Space(19) + "10 min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[4], "^&^", 17).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[4], "^&^", 18).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[4], "^&^", 19).Trim() + VB.Space(10), 10) + "\r\n";
            //s += "-----------------------------------------------------------------------------" + "\r\n";
            //s += " impression" + "\r\n";
            s = "";
            s += "Head-up Tilt Test" + "\r\n";
            s += "-----------------------------------------------------------------------------" + "\r\n";
            s += VB.Space(1) + "Procedure" + VB.Space(10) + "Time" + VB.Space(5) + "SBP" + VB.Space(5) + "DBP" + VB.Space(5) + "HR(bpm)" + VB.Space(5) + "Sx" + "\r\n";
            s += "-----------------------------------------------------------------------------" + "\r\n";
            s += VB.Space(1) + "Baseline Supine" + VB.Space(14) + VB.Left(clsComSup.setP(s2[0], "^&^", 1).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[0], "^&^", 2).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[0], "^&^", 3).Trim() + VB.Space(10), 10) + VB.Left(clsComSup.setP(s2[0], "^&^", 4).Trim() + VB.Space(11), (clsComSup.setP(s2[0], "^&^", 4).Trim() + VB.Space(11)).Length) + "\r\n";
            s += "-----------------------------------------------------------------------------" + "\r\n";
            s += VB.Space(19) + "0  min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[1], "^&^", 1).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[1], "^&^", 2).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[1], "^&^", 3).Trim() + VB.Space(10), 10) + VB.Left(clsComSup.setP(s2[1], "^&^", 4).Trim() + VB.Space(11), (clsComSup.setP(s2[1], "^&^", 4).Trim() + VB.Space(11)).Length) + "\r\n";
            s += VB.Space(19) + "5  min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[1], "^&^", 5).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[1], "^&^", 6).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[1], "^&^", 7).Trim() + VB.Space(10), 10) + VB.Left(clsComSup.setP(s2[1], "^&^", 8).Trim() + VB.Space(11), (clsComSup.setP(s2[1], "^&^", 8).Trim() + VB.Space(11)).Length) + "\r\n";
            s += VB.Space(3) + "HUT 60" + VB.Space(10) + "10 min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[1], "^&^", 9).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[1], "^&^", 10).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[1], "^&^", 11).Trim() + VB.Space(10), 10) + VB.Left(clsComSup.setP(s2[1], "^&^", 12).Trim() + VB.Space(11), (clsComSup.setP(s2[1], "^&^", 12).Trim() + VB.Space(11)).Length) + "\r\n";
            s += VB.Space(19) + "15 min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[1], "^&^", 13).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[1], "^&^", 14).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[1], "^&^", 15).Trim() + VB.Space(10), 10) + VB.Left(clsComSup.setP(s2[1], "^&^", 16).Trim() + VB.Space(11), (clsComSup.setP(s2[1], "^&^", 16).Trim() + VB.Space(11)).Length) + "\r\n";
            s += VB.Space(19) + "20 min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[1], "^&^", 17).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[1], "^&^", 18).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[1], "^&^", 19).Trim() + VB.Space(10), 10) + VB.Left(clsComSup.setP(s2[1], "^&^", 20).Trim() + VB.Space(11), (clsComSup.setP(s2[1], "^&^", 20).Trim() + VB.Space(11)).Length) + "\r\n";
            s += "-----------------------------------------------------------------------------" + "\r\n";
            s += VB.Space(19) + "0  min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[2], "^&^", 1).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[2], "^&^", 2).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[2], "^&^", 3).Trim() + VB.Space(10), 10) + VB.Left(clsComSup.setP(s2[2], "^&^", 4).Trim() + VB.Space(11), (clsComSup.setP(s2[2], "^&^", 4).Trim() + VB.Space(11)).Length) + "\r\n";
            s += VB.Space(3) + "HUT 70" + VB.Space(10) + "5  min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[2], "^&^", 5).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[2], "^&^", 6).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[2], "^&^", 7).Trim() + VB.Space(10), 10) + VB.Left(clsComSup.setP(s2[2], "^&^", 8).Trim() + VB.Space(11), (clsComSup.setP(s2[2], "^&^", 8).Trim() + VB.Space(11)).Length) + "\r\n";
            s += VB.Space(19) + "10 min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[2], "^&^", 9).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[2], "^&^", 10).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[2], "^&^", 11).Trim() + VB.Space(10), 10) + VB.Left(clsComSup.setP(s2[2], "^&^", 12).Trim() + VB.Space(11), (clsComSup.setP(s2[2], "^&^", 12).Trim() + VB.Space(11)).Length) + "\r\n";
            s += "-----------------------------------------------------------------------------" + "\r\n";
            s += VB.Space(19) + "0  min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[3], "^&^", 1).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[3], "^&^", 2).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[3], "^&^", 3).Trim() + VB.Space(10), 10) + VB.Left(clsComSup.setP(s2[3], "^&^", 4).Trim() + VB.Space(11), (clsComSup.setP(s2[3], "^&^", 4).Trim() + VB.Space(11)).Length) + "\r\n";
            s += VB.Space(3) + "supine" + VB.Space(10) + "5  min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[3], "^&^", 5).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[3], "^&^", 6).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[3], "^&^", 7).Trim() + VB.Space(10), 10) + VB.Left(clsComSup.setP(s2[3], "^&^", 8).Trim() + VB.Space(11), (clsComSup.setP(s2[3], "^&^", 8).Trim() + VB.Space(11)).Length) + "\r\n";
            s += VB.Space(19) + "10 min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[3], "^&^", 9).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[3], "^&^", 10).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[3], "^&^", 11).Trim() + VB.Space(10), 10) + VB.Left(clsComSup.setP(s2[3], "^&^", 12).Trim() + VB.Space(11), (clsComSup.setP(s2[3], "^&^", 12).Trim() + VB.Space(11)).Length) + "\r\n";
            s += "-----------------------------------------------------------------------------" + "\r\n";
            s += VB.Space(19) + "0  min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[4], "^&^", 1).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[4], "^&^", 2).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[4], "^&^", 3).Trim() + VB.Space(10), 10) + VB.Left(clsComSup.setP(s2[4], "^&^", 4).Trim() + VB.Space(11), (clsComSup.setP(s2[4], "^&^", 4).Trim() + VB.Space(11)).Length) + "\r\n";
            s += VB.Space(19) + "2  min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[4], "^&^", 5).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[4], "^&^", 6).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[4], "^&^", 7).Trim() + VB.Space(10), 10) + VB.Left(clsComSup.setP(s2[4], "^&^", 8).Trim() + VB.Space(11), (clsComSup.setP(s2[4], "^&^", 8).Trim() + VB.Space(11)).Length) + "\r\n";
            s += VB.Space(3) + "NIG 1/2T" + VB.Space(8) + "4  min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[4], "^&^", 9).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[4], "^&^", 10).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[4], "^&^", 11).Trim() + VB.Space(10), 10) + VB.Left(clsComSup.setP(s2[4], "^&^", 12).Trim() + VB.Space(11), (clsComSup.setP(s2[4], "^&^", 12).Trim() + VB.Space(11)).Length) + "\r\n";
            s += VB.Space(19) + "6  min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[4], "^&^", 13).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[4], "^&^", 14).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[4], "^&^", 15).Trim() + VB.Space(10), 10) + VB.Left(clsComSup.setP(s2[4], "^&^", 16).Trim() + VB.Space(11), (clsComSup.setP(s2[4], "^&^", 16).Trim() + VB.Space(11)).Length) + "\r\n";
            s += VB.Space(19) + "8  min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[4], "^&^", 17).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[4], "^&^", 18).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[4], "^&^", 19).Trim() + VB.Space(10), 10) + VB.Left(clsComSup.setP(s2[4], "^&^", 20).Trim() + VB.Space(11), (clsComSup.setP(s2[4], "^&^", 20).Trim() + VB.Space(11)).Length) + "\r\n";
            s += VB.Space(19) + "10 min" + VB.Space(5) + VB.Left(clsComSup.setP(s2[4], "^&^", 21).Trim() + VB.Space(8), 8) + VB.Left(clsComSup.setP(s2[4], "^&^", 22).Trim() + VB.Space(9), 9) + VB.Left(clsComSup.setP(s2[4], "^&^", 23).Trim() + VB.Space(10), 10) + VB.Left(clsComSup.setP(s2[4], "^&^", 24).Trim() + VB.Space(11), (clsComSup.setP(s2[4], "^&^", 24).Trim() + VB.Space(11)).Length) + "\r\n";
            s += "-----------------------------------------------------------------------------" + "\r\n";
            s += " impression" + "\r\n";
            if (clsComSup.setP(s2[5], "^&^", 1).Trim()=="True")
            {
                s += " ■ Negative Study" + "\r\n";
            }
            else
            {
                s += " □ Negative Study" + "\r\n";
            }
            if (clsComSup.setP(s2[5], "^&^", 2).Trim() == "True")
            {
                s += " ■ Positive Study(neurocardiogenic syncope, vasovagal syncope)" + "\r\n";
            }
            else
            {
                s += " □ Positive Study(neurocardiogenic syncope, vasovagal syncope)" + "\r\n"; 
            }
            if (clsComSup.setP(s2[6], "^&^", 1).Trim() == "True")
            {
                s += " ■ Type1 : Mixed Type" + "\r\n";
            }
            else
            {
                s += " □ Type1 : Mixed Type" + "\r\n";
            }
            s += " (HR falls at the time ofsyncope, not to < 40bpm, " + "\r\n";
            s += "  or falls to < 40 bpm for < 10 sec. BP falls before the HR falls.)" + "\r\n";
            if (clsComSup.setP(s2[7], "^&^", 1).Trim() == "True")
            {
                s += " ■ Type2 : Cardioinhibitory" + "\r\n";
            }
            else
            {
                s += " □ Type2 : Cardioinhibitory" + "\r\n";
            }
            s += " (HR falls to < 40 bpm for > 10 sec. BP falls before the HR falls.)" + "\r\n";
            if (clsComSup.setP(s2[8], "^&^", 1).Trim() == "True")
            {
                s += " ■ Type3 : Vasodepressor Type" + "\r\n";
            }
            else
            {
                s += " □ Type3 : Vasodepressor Type" + "\r\n";
            }
            s += " (HR does not fall > 10% of its peak time of syncope)" + "\r\n";
            s += "▶Conclusion" + "\r\n"; 
            s += clsComSup.setP(s2[9], "^&^", 1).Trim() + "\r\n";
            s += "▶Comments" + "\r\n";
            s += clsComSup.setP(s2[9], "^&^", 2).Trim() + "\r\n";


            #endregion

            return s;
        }

        public string Xray_Detail_DrRemark(PsmhDb pDbCon,clsComSupXraySQL.cXrayDetail argCls)
        {
            string s = string.Empty;
            DataTable dt = null;
            
            dt = cxraySql.sel_XrayDetail(pDbCon, argCls, " DrRemark ");
                        
            if (dt != null && dt.Rows.Count == 1)
            {
                s = dt.Rows[0]["DrRemark"].ToString().Trim();
            }
            else
            {
                s = "내용없음.";
            }

            return s;
        }

        #endregion

        #region NON 트랜잭션 SQL 쿼리

        public class cXray_ResultNew
        {
            public string Job = "";   //01.미판독,02.판독 03.임시 04.의뢰           

            public string Gubun1 = "";//1.촬영일자  2.등록번호
            public string Gubun2 = "";//파트(OG,EC...
            public string Gubun3 = "";//검색조건(A,B...
            public string GubunSel = ""; //00.전체, 01.미판독 02.판독 03.임시저장 >> 과별 판독 명단에 사용
            public string DrCodeSel = "";
            public string JepChk = ""; //EC 접수체크
            public string OTChk = ""; //OT 종검,건진구분
            public string XSort = "";

            public long WRTNO = 0;
            public string Pano = "";
            public string READDATE = "";
            public string SEEKDATE = "";            
            public string SName = "";
            public string SEX = "";
            public int AGE = 0;
            public string IPDOPD = "";
            public string DEPTCODE = "";
            public string DRCODE = "";
            public string WARDCODE = "";
            public string ROOMCODE = "";
            public long XDrCode1 = 0;
            public long XDrCode2 = 0;
            public long XDrCode3 = 0;
            public string ILLCODE1 = "";
            public string ILLCODE2 = "";
            public string ILLCODE3 = "";
            public string XCODE = "";
            public string XNAME = "";
            public string RESULT = "";
            public string RESULT1 = "";
            public int PRTCNT = 0;
            public int VIEWCNT = 0;
            public string ENTDATE = "";
            public string APPROVE = "";
            public string GBOUT = "";
            public string STIME = "";
            public string ETIME = "";
            public string RESULTEC = "";
            public string RESULTEC1 = "";
            public string GBANAT = "";
            public string SENDEMR = "";
            public long SENDEMRNO = 0;            
            public long PART = 0;
            public long PART2 = 0;
            public string ADDENDUM1 = "";
            public string ADDENDUM2 = "";
            public string ADDDATE = "";
            public string ADDDRCODE = "";
            public string CERTNO = "";
            public string PANHIC = "";
            public string EXID = "";
            public string READTIME = "";
            
            public string TEMP = "";
            public string INPS = "";
            public string INPT_DT = "";
            public string UPPS = "";
            public string UP_DT = "";

            public string XJong = "";
            public string XJong2 = "";
            public string SDate = "";
            public string TDate = "";            
            
            public string DrName = "";
            public string ReadDrName = "";
            public string GbSPC = "";
            public string GbRead = "";
            
            public string Search = "";
            public string NotTO = "";
            public string NotCode = "";
            public string AnatChk = "";
            public string MyChk = "";
            public string XrayBuse = "";
            public string ResultOLD = "";
                        
            public string ROWID_detail = "";
            public string ROWID = "";

            public string strPanChk = "";          

        }

        public class cXrayViewList
        {
            public string optJob1 = "";
            public string optJob2 = "";
            public string XJong = "";
            public string SDate = "";
            public string TDate = ""; 
            public string SName = "";
            public string Pano = "";
            public string DeptCode = "";
            public string DrCode = "";
        }


        /// <summary>
        /// 판독실 전용 판독 대상자 조회 쿼리
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argCls"></param>
        /// <returns></returns>
        public DataTable sel_XrayRead(PsmhDb pDbCon, cXray_ResultNew argCls,bool bLog) 
        {
            DataTable dt = null;

            //미판독
            if (argCls.Job == "01")
            {
                //xray_detail
                SQL = "";
                SQL += " SELECT                                                                                                             \r\n";
                SQL += "  a.Pano,a.Sname,a.XJong,a.XCode                                                                                    \r\n";
                SQL += "  ,TO_CHAR(a.SeekDate,'YYYY-MM-DD') SeekDate                                                                        \r\n";
                SQL += "  ,TO_CHAR(a.SeekDate,'YYYY-MM-DD HH24:MI') SeekDateTime                                                            \r\n";
                SQL += "  ,TO_CHAR(a.BDate,'YYYY-MM-DD') BDate                                                                              \r\n";
                SQL += "  ,a.DeptCode,a.DrCode,a.IpdOpd,a.WardCode,a.RoomCode,b.OrderName OrderName2                                        \r\n";
                SQL += "  ,DECODE(b.DispHeader,'',b.OrderName,b.DispHeader) OrderName3                                                      \r\n";
                SQL += "  ,a.PacsNo,a.PacsStudyID,a.OrderCode,a.OrderName,a.Remark,a.Exid,a.Sex,a.Age,a.GbSPC                               \r\n";
                SQL += "  ,a.GbRead,a.DrRemark,a.CVR,a.Exinfo,0 XDrCode1, c.Jumin1,c.Jumin2,'' Approve,a.ROWID,'' ROWID_read                \r\n";
                //SQL += "  ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Pano,a.BDate) FC_infect                                                   \r\n"; //감염체크
                SQL += "  ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(a.Pano,a.BDate) FC_infect_EX                                                   \r\n"; //감염체크
                SQL += "  ,KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(a.DrCode) FC_DrName                                                              \r\n"; //의사명
                SQL += "  ,KOSMOS_OCS.FC_XRAY_DETAIL_HEALTH_CHK('HR',a.Pano,a.SeekDate,a.PacsNo) FC_HR                                      \r\n"; //검진체크
                SQL += "  ,KOSMOS_OCS.FC_XRAY_DETAIL_HEALTH_CHK('TO',a.Pano,a.SeekDate,a.PacsNo) FC_TO                                      \r\n"; //검진체크
                SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a                                                                          \r\n";
                SQL += "     , " + ComNum.DB_MED + "OCS_ORDERCODE b                                                                         \r\n";
                SQL += "     , " + ComNum.DB_PMPA + "BAS_PATIENT c                                                                          \r\n";
                SQL += "    WHERE 1=1                                                                                                       \r\n";
                SQL += "    AND a.OrderCode = b.OrderCode(+)                                                                                \r\n";
                SQL += "    AND a.Pano = c.Pano(+)                                                                                          \r\n";
                if (argCls.Job == "02")
                {
                    SQL += "  AND a.SeekDate >= TRUNC(SYSDATE-180)                                                                          \r\n";
                    SQL += "  AND a.SeekDate < TRUNC(SYSDATE+1)                                                                             \r\n";
                    SQL += "  AND a.GbRead ='Y'                                                                                             \r\n";
                }
                else
                {
                    if (argCls.Gubun1 == "1")
                    {
                        SQL += "  AND a.SeekDate >= TO_DATE('" + argCls.SDate + "', 'YYYY-MM-DD')                                           \r\n";
                        SQL += "  AND a.SeekDate <=  TO_DATE('" + argCls.TDate + " 23:59','YYYY-MM-DD HH24:MI')                             \r\n";
                    }
                }

                SQL += "  AND (a.ExInfo < 2 OR a.ExInfo IS NULL)                                                                            \r\n";
                SQL += "  AND a.READ_SEND IS NULL                                                                                           \r\n"; //판독의뢰는 제외
                if (argCls.Gubun2 == "") //판독실전용일때
                {
                    SQL += "  AND a.XCODE NOT IN (" + argCls.NotCode + " )                                                                  \r\n";
                }
                if (argCls.Search != "")
                {
                    if (argCls.Gubun1 == "1")
                    {
                        SQL += "  AND a.SName LIKE '%" + argCls.Search + "%'                                                                \r\n";
                    }
                    else if (argCls.Gubun1 == "2")
                    {
                        if (argCls.Gubun3 == "A")
                        {
                            SQL += "  AND a.Pano = '" + argCls.Search + "'                                                                  \r\n";
                        }
                        else if (argCls.Gubun3 == "B")
                        {
                            SQL += "  AND a.SName LIKE '%" + argCls.Search + "%'                                                            \r\n";
                            SQL += "  AND a.SeekDate >= TO_DATE('" + Convert.ToDateTime(argCls.SDate).AddDays(-20).ToShortDateString() + "', 'YYYY-MM-DD')     \r\n";
                        }
                        else if (argCls.Gubun3 == "C")
                        {
                            SQL += "  AND a.SName LIKE '%" + argCls.Search + "%'                                                            \r\n";
                        }
                    }
                }
                if (argCls.GbSPC != "")
                {
                    SQL += "  AND a.GbSPC ='1'                                                                                              \r\n";
                }
                if (argCls.Gubun2 != "")
                {
                    SQL += "  AND a.GbReserved IN ('1','6','7','8')                                                                         \r\n"; //과별리스트는 대기자포함
                    SQL += "  AND a.XJong IN (" + argCls.XJong + ")                                                                         \r\n";
                    if (argCls.Gubun2 == "UR")
                    {
                        SQL += "  AND a.DeptCode IN ('UR')                                                                                  \r\n";
                    }
                    else if (argCls.Gubun2 == "OT")
                    {
                        SQL += "  AND (a.XJong in ('F') OR (a.XJong='N' AND (a.DeptCode ='TO' OR a.DeptCode ='HR') ))                       \r\n";
                    }
                }
                else
                {
                    SQL += "  AND a.GbReserved IN ('6','7','8')                                                                             \r\n";

                    if (argCls.XJong != "Q")
                    {
                        SQL += "  AND a.XJong <= '9'                                                                                        \r\n";
                    }
                    if (argCls.XJong == "HR")
                    {
                        SQL += "  AND a.DeptCode ='HR'                                                                                      \r\n";
                    }
                    else if (argCls.XJong == "TO")
                    {
                        SQL += "  AND a.DeptCode ='TO'                                                                                      \r\n";
                    }
                    else if (argCls.XJong == "SONO")
                    {
                        SQL += "  AND a.XCODE = 'US04'                                                                                      \r\n"; //SONO BREAST
                    }
                    else
                    {
                        if (argCls.NotTO == "True")
                        {
                            SQL += "  AND a.DeptCode <> 'TO'                                                                                \r\n";
                            SQL += "  AND a.XJong NOT IN ('6','7')                                                                          \r\n";
                        }
                        if (argCls.XJong != "*")
                        {
                            SQL += "  AND a.XJong ='" + argCls.XJong + "'                                                                   \r\n";
                        }
                    }
                }

            }
            else if (argCls.Job == "04")
            {
                //xray_read_req                    

                SQL += " SELECT                                                                                                             \r\n";
                SQL += "  a.Pano,b.SName,a.XJong,a.XCode                                                                                    \r\n";
                SQL += "  ,TO_CHAR(a.SeekDate,'YYYY-MM-DD') SeekDate                                                                        \r\n";
                SQL += "  ,TO_CHAR(a.SeekDate,'YYYY-MM-DD HH24:MI') SeekDateTime                                                            \r\n";
                SQL += "  ,TO_CHAR(a.BDate,'YYYY-MM-DD') BDate                                                                              \r\n";
                SQL += "  ,a.DeptCode,a.DrCode,a.IpdOpd, a.WardCode,a.RoomCode                                                              \r\n";
                SQL += "  ,a.PacsNo,a.PacsStudyID,a.OrderCode,a.OrderName, '' Remark                                                        \r\n";
                SQL += "  ,a.Exid,a.Sex,a.Age,a.GbSpc,'Y' GbRead, a.DrRemark,a.CVR,a.Exinfo                                                 \r\n";
                SQL += "  ,0 xDrCode1                                                                                                       \r\n";
                SQL += "  ,b.Jumin1,b.Jumin2,'' Approve,a.ROWID,'' ROWID_read                                                               \r\n";
                //SQL += "  ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Pano,a.BDate) FC_infect                                                   \r\n"; //감염체크
                SQL += "  ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(a.Pano,a.BDate) FC_infect_EX                                                   \r\n"; //감염체크
                SQL += "  ,KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(a.DrCode) FC_DrName                                                              \r\n"; //의사명
                SQL += "  ,KOSMOS_OCS.FC_XRAY_DETAIL_HEALTH_CHK('HR',a.Pano,a.SeekDate,a.PacsNo) FC_HR                                      \r\n"; //검진체크
                SQL += "  ,KOSMOS_OCS.FC_XRAY_DETAIL_HEALTH_CHK('TO',a.Pano,a.SeekDate,a.PacsNo) FC_TO                                      \r\n"; //검진체크
                SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a , " + ComNum.DB_PMPA + "BAS_PATIENT b                                    \r\n";
                SQL += "   ," + ComNum.DB_PMPA + "BAS_DOCTOR c , " + ComNum.DB_PMPA + "XRAY_CODE d,                                         \r\n";
                SQL += "   " + ComNum.DB_PMPA + "XRAY_READ_REQ e                                                                            \r\n";
                SQL += "    WHERE 1=1                                                                                                       \r\n";
                if (argCls.Gubun1 == "1")
                {
                    SQL += "  AND e.BDate >= TO_DATE('" + argCls.SDate + "', 'YYYY-MM-DD')                                                  \r\n";
                    SQL += "  AND e.BDate <=  TO_DATE('" + argCls.TDate + "','YYYY-MM-DD HH24:MI')                                          \r\n";
                }
                SQL += "  AND a.SeekDate >= e.SeekDate                                                                                      \r\n";
                SQL += "  AND a.PacsStudyID > ' '                                                                                           \r\n";
                SQL += "  AND (a.ExInfo < 1000 OR a.ExInfo IS NULL)                                                                         \r\n";
                SQL += "  AND a.Read_Send IS NULL                                                                                           \r\n";
                SQL += "  AND a.Pano=b.Pano(+)                                                                                              \r\n";

                SQL += "  AND a.Pano=e.Pano                                                                                                 \r\n";
                SQL += "  AND a.PacsNo=e.PacsNo                                                                                             \r\n";
                SQL += "  AND a.DrCode=c.DrCode(+)                                                                                          \r\n";

                SQL += "  AND a.XCode=d.XCode(+)                                                                                            \r\n";
                SQL += "  AND ( e.GbPrint IS NULL OR e.GbPrint ='' )                                                                        \r\n";
                SQL += "  AND ( e.DelDate IS NULL OR e.DelDate ='' )                                                                        \r\n";
                SQL += "  AND a.Read_Receive IS NULL                                                                                        \r\n";

                if (argCls.Search != "")
                {
                    if (argCls.Gubun1 == "1")
                    {
                        SQL += "  AND a.SName LIKE '%" + argCls.Search + "%'                                                                \r\n";
                    }
                    else if (argCls.Gubun1 == "2")
                    {
                        if (argCls.Gubun3 == "A")
                        {
                            SQL += "  AND a.Pano = '" + argCls.Search + "'                                                                  \r\n";
                        }
                        else if (argCls.Gubun3 == "B")
                        {
                            SQL += "  AND a.SName LIKE '%" + argCls.Search + "%'                                                             \r\n";
                            SQL += "  AND e.BDate >= TO_DATE('" + Convert.ToDateTime(argCls.SDate).AddDays(-60).ToShortDateString() + "', 'YYYY-MM-DD')     \r\n";
                        }
                        else if (argCls.Gubun3 == "C")
                        {
                            SQL += "  AND a.SName LIKE '%" + argCls.Search + "%'                                                            \r\n";
                        }
                    }
                }
                if (argCls.GbSPC != "")
                {
                    SQL += "  AND a.GbSPC ='1'                                                                                              \r\n";
                }

            }
            else if (argCls.Job == "02" || argCls.Job == "03")
            {
                //xray_resultnew
                SQL = "";
                SQL += " SELECT                                                                                                             \r\n";
                SQL += "  a.Pano,a.Sname,a.XJong,a.XCode                                                                                    \r\n";
                SQL += "  ,TO_CHAR(a.SeekDate,'YYYY-MM-DD') SeekDate                                                                        \r\n";
                SQL += "  ,TO_CHAR(c.SeekDate,'YYYY-MM-DD HH24:MI') SeekDateTime                                                            \r\n";
                SQL += "  ,TO_CHAR(c.BDate,'YYYY-MM-DD') BDate                                                                              \r\n";
                SQL += "  ,a.DeptCode,a.DrCode,a.IpdOpd,a.WardCode,a.RoomCode                                                               \r\n";
                SQL += "  ,c.PacsNo,c.PacsStudyID,c.OrderCode,a.XName OrderName                                                             \r\n";
                SQL += "  ,c.Remark,c.Exid,a.Sex,a.Age,a.GbSPC                                                                              \r\n";
                SQL += "  ,'' GbRead,c.DrRemark,c.CVR,a.WRTNO Exinfo                                                                        \r\n";
                SQL += "  ,a.XDrCode1                                                                                                       \r\n";
                SQL += "  ,b.Jumin1,b.Jumin2,a.Approve, c.ROWID,a.ROWID ROWID_read, a.PANHIC                                                \r\n";
                //SQL += "  ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Pano,c.BDate) FC_infect                                                   \r\n"; //감염체크         
                SQL += "  ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(a.Pano,c.BDate) FC_infect_EX                                                   \r\n"; //감염체크
                SQL += "  ,KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(a.DrCode) FC_DrName                                                              \r\n"; //의사명
                SQL += "  ,KOSMOS_OCS.FC_XRAY_DETAIL_HEALTH_CHK('HR',a.Pano,c.SeekDate,c.PacsNo) FC_HR                                      \r\n"; //검진체크
                SQL += "  ,KOSMOS_OCS.FC_XRAY_DETAIL_HEALTH_CHK('TO',a.Pano,c.SeekDate,c.PacsNo) FC_TO                                      \r\n"; //검진체크
                SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_RESULTNEW a                                                                       \r\n";
                SQL += "     , " + ComNum.DB_PMPA + "BAS_PATIENT b                                                                          \r\n";
                SQL += "     , " + ComNum.DB_PMPA + "XRAY_DETAIL c                                                                          \r\n";
                SQL += "    WHERE 1=1                                                                                                       \r\n";
                SQL += "      AND a.Pano=b.Pano(+)                                                                                          \r\n";
                SQL += "      AND a.Pano=c.Pano(+)                                                                                          \r\n";
                SQL += "      AND a.WRTNO=c.Exinfo(+)                                                                                       \r\n";
                if (argCls.Gubun1 == "1")
                {
                    SQL += "  AND a.ReadDate >= TO_DATE('" + argCls.SDate + "', 'YYYY-MM-DD')                                               \r\n";
                    SQL += "  AND a.ReadDate <=  TO_DATE('" + argCls.TDate + " 23:59','YYYY-MM-DD HH24:MI')                                 \r\n";
                }
                if (argCls.Search != "")
                {
                    if (argCls.Gubun1 == "1")
                    {
                        SQL += "  AND a.SName LIKE '%" + argCls.Search + "%'                                                                \r\n";
                    }
                    else if (argCls.Gubun1 == "2")
                    {
                        if (argCls.Gubun3 == "A")
                        {
                            SQL += "  AND a.Pano = '" + argCls.Search + "'                                                                  \r\n";
                        }
                        else if (argCls.Gubun3 == "B")
                        {
                            SQL += "  AND a.SName LIKE '%" + argCls.Search + "%'                                                            \r\n";
                            SQL += "  AND a.ReadDate >= TO_DATE('" + Convert.ToDateTime(argCls.SDate).AddDays(-60).ToShortDateString() + "', 'YYYY-MM-DD')     \r\n";
                        }
                        else if (argCls.Gubun3 == "C")
                        {
                            SQL += "  AND a.SName LIKE '%" + argCls.Search + "%'                                                            \r\n";
                        }
                    }

                }

                //2019-05-15 안정수 추가, 박은지s 요청으로 공단검진판정 누락자 체크
                if(argCls.strPanChk != "")
                {
                    SQL += "  AND a.PANHIC IS NULL                                                                                          \r\n";
                    SQL += "  AND a.XCODE = 'GR2101'                                                                                        \r\n";
                }
                if (argCls.GbSPC != "")
                {
                    SQL += "  AND a.GbSPC ='1'                                                                                              \r\n";
                }

                if (argCls.Gubun2 != "")
                {
                    SQL += "  AND a.XJong IN ( " + argCls.XJong + " )                                                                       \r\n";
                    if (argCls.Gubun2 == "UR")
                    {
                        SQL += "  AND a.DeptCode IN ('UR')                                                                                  \r\n";
                    }
                    else if (argCls.Gubun2 == "OT")
                    {
                        SQL += "  AND (a.XJong in ('F') OR (a.XJong='N' AND (a.DeptCode ='TO' OR a.DeptCode ='HR') ))                       \r\n";
                    }
                }
                else
                {
                    if (argCls.Job == "02")
                    {
                        SQL += "  AND a.APPROVE='Y'                                                                                         \r\n";
                    }
                    else
                    {
                        SQL += "  AND (a.APPROVE='N' OR a.APPROVE IS NULL)                                                                  \r\n";
                    }

                    if (argCls.XJong == "HR")
                    {
                        SQL += "  AND a.DeptCode ='HR'                                                                                      \r\n";
                    }
                    else if (argCls.XJong == "TO")
                    {
                        SQL += "  AND a.DeptCode ='TO'                                                                                      \r\n";
                    }
                    else if (argCls.XJong == "SONO")
                    {
                        //2019-10-31 안정수 조건 변경 
                        //SQL += "  AND a.XCODE = 'US04'                                                                                      \r\n"; //SONO BREAST
                        SQL += "  AND a.XCODE IN ('US04', 'EB421')                                                                          \r\n"; //SONO BREAST
                    }
                    else
                    {
                        if (argCls.NotTO == "True")
                        {
                            SQL += "  AND a.DeptCode <> 'TO'                                                                                \r\n";
                            SQL += "  AND a.XJong NOT IN ('6','7')                                                                          \r\n";
                            //2019-10-31 안정수 조건 변경 
                            //SQL += "  AND a.XCode NOT IN ('G2702','G2702B')                                                                 \r\n";
                            SQL += "  AND a.XCode NOT IN ('G2702','G2702B','EB421')                                                         \r\n";
                        }
                        if (argCls.XJong != "*")
                        {
                            SQL += "  AND a.XJong ='" + argCls.XJong + "'                                                                   \r\n";
                        }

                        if (argCls.XDrCode1 > 0)
                        {
                            SQL += "  AND a.XDrCode1 =" + argCls.XDrCode1 + "                                                               \r\n";
                        }

                    }
                }

            }
            else
            {

            }
            if (argCls.XSort !="")
            {
                if (argCls.XSort =="PANO")
                {
                    SQL += " ORDER BY 1,2,3,4                                                                                               \r\n";
                }
                else if (argCls.XSort == "SNAME")
                {
                    SQL += " ORDER BY 2,1,3,4                                                                                               \r\n";
                }
                else if (argCls.XSort == "DEPT")
                {
                    SQL += " ORDER BY 8,1,2,3,4                                                                                             \r\n";
                }
                else if (argCls.XSort == "XCODE")
                {
                    SQL += " ORDER BY 4,1,2,3                                                                                               \r\n";
                }
                else if (argCls.XSort == "XNAME")
                {
                    if (argCls.Job== "01")
                    {
                        SQL += " ORDER BY 14,19,1,2,3,4                                                                                     \r\n";
                    }
                    else if (argCls.Job == "02" || argCls.Job == "03")
                    {
                        SQL += " ORDER BY 16,1,2,3,4                                                                                        \r\n";
                    }
                    else
                    {
                        SQL += " ORDER BY 16,1,2,3,4                                                                                        \r\n";
                    }                    
                }
                else if (argCls.XSort == "SEEK")
                {
                    SQL += " ORDER BY 6,2,1,3,4                                                                                             \r\n";
                }
            }
            else
            {
                SQL += " ORDER BY 2,1,3,4                                                                                                   \r\n";
            }
            


            try
            {                
                if (bLog == true)
                {
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                }
                else 
                {
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                }

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

        public DataTable sel_XrayDetail(PsmhDb pDbCon, string Job, string argPano, string argDept, long argExinfo, string argDate = "", int argSeq = 0, string argIO = "")
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                         \r\n";
            SQL += "  Pano,SName,DeptCode,DrCode,ExID,IpdOpd,WardCode                               \r\n";
            SQL += "  ,RoomCode,XJong,XCode,Remark,Sex,Age,ExInfo                                   \r\n";
            SQL += "  ,PacsNo,PacsStudyID,OrderName                                                 \r\n";
            SQL += "  ,TO_CHAR(SeekDate,'YYYY-MM-DD') SeekDate                                      \r\n";
            SQL += "  ,TO_CHAR(SeekDate,'YYYY-MM-DD HH24:MI') SeekDateTime                          \r\n";
            SQL += "  ,TO_CHAR(BDate,'YYYY-MM-DD') BDate                                            \r\n";
            SQL += "  ,ROWID                                                                        \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                       \r\n";
            SQL += "  WHERE 1 = 1                                                                   \r\n";
            if (Job == "V1")
            {
                SQL += "   AND Pano ='" + argPano + "'                                              \r\n";
                SQL += "   AND DeptCode ='" + argDept + "'                                          \r\n";
                SQL += "   AND ExInfo =" + argExinfo + "                                            \r\n";
                SQL += "   AND PacsStudyID IS NOT NULL                                              \r\n";
            }
            else if (Job == "T1")
            {
                SQL += "    AND SeekDate >= TO_DATE('" + argDate + "', 'YYYY-MM-DD')                \r\n";
                SQL += "    AND SeekDate <= TO_DATE('" + argDate + " 23:59', 'YYYY-MM-DD HH24:MI')  \r\n";
                SQL += "    AND MgrNo = " + argSeq + "                                              \r\n";
                if (argIO != "") SQL += "    AND a.IpdOpd ='" + argIO + "'                          \r\n";
                SQL += "    AND GbEnd = '1'                                                         \r\n";
                SQL += "    AND (GbHIC IS NULL OR GbHIC <> 'Y')                                     \r\n";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
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



        public DataTable sel_HIC_AlreadyChkX(PsmhDb pDbCon, string argPano, string argSeekdate, string argXcode)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT * FROM KOSMOS_PMPA.XRAY_RESULTNEW                                       \r\n";
            SQL += "  WHERE 1 = 1                                                                   \r\n";
            SQL += "   AND Pano ='" + argPano + "'                                                  \r\n";
            SQL += "   AND SEEKDATE = TO_DATE('" + argSeekdate + "', 'YYYY-MM-DD')                                          \r\n";
            SQL += "   AND XCODE ='" + argXcode + "'                                          \r\n";
            SQL += "   AND SENDDATE IS NOT NULL                                                     \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
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

        public DataTable sel_XrayDetail_LINK(PsmhDb pDbCon, string Job, string argPano, string argPacsNo)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                         \r\n";
            SQL += "  Pano,SName,DeptCode,DrCode,ExID,IpdOpd,WardCode                               \r\n";
            SQL += "  ,RoomCode,XJong,XCode,Remark,DRREMARK,Sex,Age,ExInfo                          \r\n";
            SQL += "  ,PacsNo,PacsStudyID,OrderCode,OrderName                                       \r\n";
            SQL += "  ,TO_CHAR(SeekDate,'YYYY-MM-DD') SeekDate                                      \r\n";
            SQL += "  ,TO_CHAR(SeekDate,'YYYY-MM-DD HH24:MI') SeekDateTime                          \r\n";
            SQL += "  ,TO_CHAR(BDate,'YYYY-MM-DD') BDate                                            \r\n";
            SQL += "  ,ROWID                                                                        \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                       \r\n";
            SQL += "  WHERE 1 = 1                                                                   \r\n";
            if (Job =="LINK1")
            {
                SQL += "   AND Pano ='" + argPano + "'                                              \r\n";
                SQL += "   AND PacsNo ='" + argPacsNo + "'                                          \r\n";
                SQL += "   AND GbReserved IN ('1','6','7','8')                                      \r\n";
                SQL += "   AND XJong < 'A'                                                          \r\n";
                SQL += "   AND  XCODE NOT IN ('US24','G201','F08','F09'                             \r\n";
                SQL += "                       ,'F10','F11','F12','F13','G9101','G2000')            \r\n";
                SQL += "   AND ROWNUM <= 1                                                          \r\n";
            }
            else
            {
                SQL += "   AND Pano ='" + argPano + "'                                              \r\n";
                SQL += "   AND PacsNo ='" + argPacsNo + "'                                          \r\n";
                SQL += "   AND GbReserved IN ('1','6','7','8')                                      \r\n";
            }
                        
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
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

        /// <summary>
        /// 과전용 판독 대상자 쿼리
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argCls"></param>
        /// <returns></returns>
        public DataTable sel_XrayRead_Dept(PsmhDb pDbCon, cXray_ResultNew argCls)
        {
            DataTable dt = null;                        
                        
            #region //미판독 쿼리
            if (argCls.Job == "01")
            {
                
                if (argCls.Gubun2 != "EEG")
                {
                    #region //xray_detail 기준테이블
                    SQL = "";
                    SQL += " SELECT                                                                                                             \r\n";
                    SQL += "  a.Pano,a.Sname,a.XJong,a.XCode                                                                                    \r\n";
                    SQL += "  ,TO_CHAR(a.SeekDate,'YYYY-MM-DD') SeekDate                                                                        \r\n";
                    SQL += "  ,TO_CHAR(a.SeekDate,'YYYY-MM-DD HH24:MI') SeekDateTime                                                            \r\n";
                    SQL += "  ,TO_CHAR(a.BDate,'YYYY-MM-DD') BDate                                                                              \r\n";
                    SQL += "  ,'' ReadDate                                                                                                      \r\n";
                    SQL += "  ,a.DeptCode,a.DrCode,a.IpdOpd,a.WardCode,a.RoomCode,b.OrderName OrderName2                                        \r\n";                    
                    SQL += "  ,a.PacsNo,a.PacsStudyID,a.OrderNo,a.OrderCode,a.OrderName,a.Remark                                                \r\n";
                    SQL += "  ,a.GbRead,a.DrRemark,a.CVR,a.Exinfo,a.EMGWRTNO,a.Exid,a.Sex,a.Age,a.GbSPC                                         \r\n";
                    SQL += "  ,c.Jumin1,c.Jumin2,'' Approve, a.ROWID,'' ROWID_read                                                              \r\n";
                    SQL += "  ,KOSMOS_OCS.FC_XRAY_CODE_NM(a.XCode) FC_XName                                                                     \r\n"; //코드명칭
                    SQL += "  ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Pano,a.BDate) FC_infect                                                   \r\n"; //감염체크
                    SQL += "  ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(a.Pano,a.BDate) FC_infect_EX                                             \r\n"; //감염체크
                    SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a                                                                          \r\n";
                    SQL += "     , " + ComNum.DB_MED + "OCS_ORDERCODE b                                                                         \r\n";
                    SQL += "     , " + ComNum.DB_PMPA + "BAS_PATIENT c                                                                          \r\n";
                    SQL += "    WHERE 1=1                                                                                                       \r\n";
                    SQL += "    AND a.OrderCode = b.OrderCode(+)                                                                                \r\n";
                    SQL += "    AND a.Pano = c.Pano(+)                                                                                          \r\n";
                    if (argCls.Gubun1 == "1")
                    {
                        if (argCls.Gubun2 == "BMD")
                        {
                            SQL += "  AND a.SeekDate >= TO_DATE('" + argCls.SDate + "', 'YYYY-MM-DD')                                           \r\n";
                            SQL += "  AND a.SeekDate <=  TO_DATE('" + argCls.TDate + " 23:59','YYYY-MM-DD HH24:MI')                             \r\n";
                        }
                        else
                        {
                            SQL += "  AND a.SeekDate >= TO_DATE('" + argCls.SDate + "', 'YYYY-MM-DD')                                           \r\n";
                            SQL += "  AND a.SeekDate <=  TO_DATE('" + argCls.TDate + " 23:59','YYYY-MM-DD HH24:MI')                             \r\n";
                        }
                    }

                    if (argCls.GubunSel == "01")//미판독
                    {
                        SQL += "  AND (a.ExInfo < 2 OR a.ExInfo IS NULL)                                                                        \r\n";
                    }
                    else if (argCls.GubunSel == "02") //판독
                    {
                        SQL += "  AND a.ExInfo > 2                                                                                              \r\n";
                    }
                    else if (argCls.GubunSel == "03") //임시
                    {
                        SQL += "  AND EXISTS ( SELECT TEMP                                                                                      \r\n";
                        SQL += "                FROM " + ComNum.DB_PMPA + "XRAY_RESULTNEW b                                                     \r\n";
                        SQL += "                 WHERE 1=1                                                                                      \r\n";
                        SQL += "                   AND b.WRTNO= a.ExInfo                                                                        \r\n";
                        SQL += "                   AND b.TEMP ='Y' )                                                                            \r\n";
                    }

                    if (argCls.Gubun2 == "") //판독실전용일때
                    {
                        SQL += "  AND a.XCODE NOT IN (" + argCls.NotCode + " )                                                                  \r\n";
                    }
                    if (argCls.Search != "")
                    {
                        if (argCls.Gubun1 == "1")
                        {
                            SQL += "  AND a.SName LIKE '" + argCls.Search + "%'                                                                 \r\n";
                        }
                        else if (argCls.Gubun1 == "2")
                        {
                            if (argCls.Gubun3 == "A")
                            {
                                SQL += "  AND a.Pano = '" + argCls.Search + "'                                                                  \r\n";
                            }
                            else if (argCls.Gubun3 == "B")
                            {
                                SQL += "  AND a.SName LIKE '" + argCls.Search + "%'                                                             \r\n";
                                SQL += "  AND a.SeekDate >= TO_DATE('" + Convert.ToDateTime(argCls.SDate).AddDays(-20).ToShortDateString() + "', 'YYYY-MM-DD')     \r\n";
                            }
                            else if (argCls.Gubun3 == "C")
                            {
                                SQL += "  AND a.SName LIKE '" + argCls.Search + "%'                                                             \r\n";
                            }
                        }
                    }
                    if (argCls.IPDOPD != "")
                    {
                        SQL += "  AND a.IPDOPD = '" + argCls.IPDOPD + "'                                                                        \r\n";
                    }
                    if (argCls.Gubun2 != "")
                    {
                        SQL += "  AND a.GbReserved IN ('1','6','7','8')                                                                         \r\n";
                        if (argCls.Gubun2 != "OT" && argCls.Gubun2 != "EC" && argCls.Gubun2 != "OG")
                        {
                            SQL += "  AND a.XJong IN (" + argCls.XJong + ")                                                                     \r\n";
                        }                           
                        
                        if (argCls.Gubun2 == "UR")
                        {
                            SQL += "  AND a.DeptCode IN ('UR')                                                                                  \r\n";
                            //2019-08-27 안정수, 비뇨기과 초음파코드 추가 
                            SQL += "  AND a.XCODE IN ('EB451', 'EB451A', 'EB454A', 'US15', 'US06U')                                             \r\n";
                        }
                        else if (argCls.Gubun2 == "OT")
                        {
                            SQL += "  AND (a.XJong in ('F', '3') OR (a.XJong='N' AND (a.DeptCode ='TO' OR a.DeptCode ='HR') ))                       \r\n";
                            if (argCls.OTChk != "")
                            {
                                SQL += "  AND a.DeptCode ='" + argCls.OTChk + "'                                                                \r\n";
                            }
                        }
                        else if (argCls.Gubun2 == "EC")
                        {
                            //2019-07-03 안정수 신규코드 추가 ('EB521A', 'EB561')
                            //2019-08-30 안정수 신규코드 추가('EB610A', 'US-TEE40', 'EB611')
                            SQL += "  AND (a.XJong = 'C' OR a.XCode IN ('US-TEE', 'US-TEER', 'US-CADU1','US-CADU2', 'US-CADUR', 'EB521A', 'EB561', 'EB610A', 'US-TEE40', 'EB611'))           \r\n";
                            if (argCls.DrCodeSel != "****")
                            {
                                SQL += "  AND a.DrCode ='" + argCls.DrCodeSel + "'                                                              \r\n";
                            }
                            //구소정C 건진 예약일자별 검색 2021-02-10 요청
                            SQL += "  AND ( ( ('TO' = A.DEPTCODE) AND ( (a.Bdate >= TO_DATE('" + argCls.SDate + "', 'YYYY-MM-DD') AND a.Bdate <=  TO_DATE('" + argCls.TDate + " 23:59','YYYY-MM-DD HH24:MI'))) )                             \r\n";
                            SQL += "  OR ( ('TO' != A.DEPTCODE) AND ( 1=1 ) ) )                             \r\n";
                        }
                        //2020-01-14 안정수, OG 신규코드 추가 
                        else if (argCls.Gubun2 == "OG")
                        {
                            SQL += "  AND a.XCODE IN ('EB455', 'EB457', 'EB455001', 'EB457001')                                                 \r\n";
                            SQL += "  AND a.DEPTCODE = 'OG'                                                                                     \r\n";
                            SQL += "  AND a.DrCode = '" + argCls.DRCODE + "'                                                                    \r\n";
                        }
                    }
                    else
                    {
                        SQL += "  AND a.GbReserved IN ('6','7','8')                                                                             \r\n";

                        if (argCls.XJong != "Q")
                        {
                            SQL += "  AND a.XJong <= '9'                                                                                        \r\n";
                        }
                        if (argCls.XJong == "HR")
                        {
                            SQL += "  AND a.DeptCode ='HR'                                                                                      \r\n";
                        }
                        else if (argCls.XJong == "TO")
                        {
                            SQL += "  AND a.DeptCode ='TO'                                                                                      \r\n";
                        }
                        else if (argCls.XJong == "SONO")
                        {
                            SQL += "  AND a.XCODE = 'US04'                                                                                      \r\n"; //SONO BREAST
                        }
                        else
                        {
                            if (argCls.NotTO == "True")
                            {
                                SQL += "  AND a.DeptCode <> 'TO'                                                                                \r\n";
                                SQL += "  AND a.XJong NOT IN ('6','7')                                                                          \r\n";
                            }
                            if (argCls.XJong != "*")
                            {
                                SQL += "  AND a.XJong ='" + argCls.XJong + "'                                                                   \r\n";
                            }
                        }
                    }
                    #endregion
                }
                
                else if (argCls.Gubun2 == "EEG")
                {
                    #region //etc_jupmst 기준테이블 EEG
                    SQL = "";
                    SQL += " SELECT                                                                                                             \r\n";
                    SQL += "  a.Ptno Pano,a.Sname,'P' XJong,a.OrderCode XCode                                                                   \r\n";
                    SQL += "  ,TO_CHAR(a.RDate,'YYYY-MM-DD') SeekDate                                                                           \r\n";
                    SQL += "  ,TO_CHAR(a.RDate,'YYYY-MM-DD HH24:MI') SeekDateTime                                                               \r\n";
                    SQL += "  ,TO_CHAR(a.BDate,'YYYY-MM-DD') BDate                                                                              \r\n";
                    SQL += "  ,'' ReadDate                                                                                                      \r\n";
                    SQL += "  ,a.DeptCode,a.DrCode,a.GbIO IpdOpd,'' WardCode,a.RoomCode,b.OrderName OrderName2                                  \r\n";
                    SQL += "  ,'' PacsNo,'' PacsStudyID,a.OrderNo,a.OrderCode,'' OrderName,a.Remark                                             \r\n";
                    SQL += "  ,'' GbRead,'' DrRemark,'' CVR,a.Read_Wrtno Exinfo,0 EMGWRTNO,0 Exid,a.Sex,a.Age,'' GbSPC                          \r\n";
                    SQL += "  ,c.Jumin1,c.Jumin2,'' Approve, a.ROWID,'' ROWID_read                                                              \r\n";
                    SQL += "  ,KOSMOS_OCS.FC_OCS_ORDERCODE_NAME2(a.OrderCode) FC_XName                                                          \r\n"; //코드명칭
                    SQL += "  ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Ptno,a.BDate) FC_infect                                                   \r\n"; //감염체크
                    SQL += "  ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(a.Ptno,a.BDate) FC_infect_EX                                             \r\n"; //감염체크
                    SQL += "  FROM " + ComNum.DB_MED + "ETC_JUPMST a                                                                            \r\n";
                    SQL += "     , " + ComNum.DB_MED + "OCS_ORDERCODE b                                                                         \r\n";
                    SQL += "     , " + ComNum.DB_PMPA + "BAS_PATIENT c                                                                          \r\n";
                    SQL += "    WHERE 1=1                                                                                                       \r\n";
                    SQL += "    AND a.OrderCode = b.OrderCode(+)                                                                                \r\n";
                    SQL += "    AND a.Ptno = c.Pano(+)                                                                                          \r\n";
                    SQL += "    AND a.Gubun IN  ('2')                                                                                           \r\n"; //EEG
                    SQL += "    AND a.GbJob NOT IN  ('9')                                                                                       \r\n"; //EEG 취소제외
                    if (argCls.Gubun1 == "1")
                    {                        
                        SQL += "  AND a.RDate >= TO_DATE('" + argCls.SDate + "', 'YYYY-MM-DD')                                                  \r\n";
                        SQL += "  AND a.RDate <=  TO_DATE('" + argCls.TDate + " 23:59','YYYY-MM-DD HH24:MI')                                    \r\n";                        
                    }

                    if (argCls.GubunSel == "01")//미판독
                    {
                        SQL += "  AND (a.Read_Wrtno < 2 OR a.Read_Wrtno IS NULL)                                                                \r\n";
                    }
                    else if (argCls.GubunSel == "02") //판독
                    {
                        SQL += "  AND a.Read_Wrtno > 2                                                                                          \r\n";
                    }
                    if (argCls.Search != "")
                    {
                        if (argCls.Gubun1 == "1")
                        {
                            SQL += "  AND a.SName LIKE '" + argCls.Search + "%'                                                                 \r\n";
                        }
                        else if (argCls.Gubun1 == "2")
                        {
                            if (argCls.Gubun3 == "A")
                            {
                                SQL += "  AND a.Ptno = '" + argCls.Search + "'                                                                  \r\n";
                            }
                            else if (argCls.Gubun3 == "B")
                            {
                                SQL += "  AND a.SName LIKE '" + argCls.Search + "%'                                                             \r\n";
                                SQL += "  AND a.RDate >= TO_DATE('" + Convert.ToDateTime(argCls.SDate).AddDays(-20).ToShortDateString() + "', 'YYYY-MM-DD')     \r\n";
                            }
                            else if (argCls.Gubun3 == "C")
                            {
                                SQL += "  AND a.SName LIKE '" + argCls.Search + "%'                                                             \r\n";
                            }
                        }
                    }
                    if (argCls.IPDOPD != "")
                    {
                        SQL += "  AND a.GbIO = '" + argCls.IPDOPD + "'                                                                          \r\n";
                    }
                    #endregion
                }

            }
            #endregion

            #region //판독 쿼리
            else if (argCls.Job == "02" || argCls.Job == "03") 
            {               
                if (argCls.Gubun2 != "EEG")
                {
                    #region //xray_resultnew 기준테이블
                    SQL = "";
                    SQL += " SELECT                                                                                                                  \r\n";
                    SQL += "  a.Pano,a.Sname,a.XJong,a.XCode                                                                                        \r\n";
                    SQL += "  ,TO_CHAR(a.SeekDate,'YYYY-MM-DD') SeekDate                                                                            \r\n";
                    SQL += "  ,TO_CHAR(c.SeekDate,'YYYY-MM-DD HH24:MI') SeekDateTime                                                                \r\n";
                    SQL += "  ,TO_CHAR(c.BDate,'YYYY-MM-DD') BDate                                                                                  \r\n";
                    SQL += "  ,TO_CHAR(a.ReadDate,'YYYY-MM-DD') ReadDate                                                                            \r\n";
                    SQL += "  ,a.DeptCode,a.DrCode,a.IpdOpd,a.WardCode,a.RoomCode                                                                   \r\n";
                    //SQL += "  ,0 PacsNo,'' PacsStudyID,c.OrderNo,c.OrderCode,a.XName OrderName                                                      \r\n";

                    //2018-10-25 안정수, 윤만식t 요청으로 BMD 판독시 영상칼럼에 ▦ 표시되도록 수정함
                    if (argCls.Gubun2 == "BMD" || argCls.Gubun2 == "OG")
                    {
                        SQL += "  ,0 PacsNo, c.PacsStudyID,c.OrderNo,c.OrderCode,a.XName OrderName                                                  \r\n";
                    }
                    else
                    {
                        SQL += "  ,0 PacsNo,'' PacsStudyID,c.OrderNo,c.OrderCode,a.XName OrderName                                                  \r\n";
                        
                    }
                    //SQL += "  ,'' Remark,0 Exid,a.Sex,a.Age,a.GbSPC,a.Part,a.Part2                                                                  \r\n";
                    SQL += "  ,'' Remark,c.Exid,a.Sex,a.Age,a.GbSPC,a.Part,a.Part2                                                                  \r\n";
                    SQL += "  ,'' GbRead,'' DrRemark,'' CVR,a.WRTNO Exinfo,c.EMGWRTNO                                                               \r\n";
                    SQL += "  ,b.Jumin1,b.Jumin2,a.Approve,a.Result, c.ROWID,a.ROWID ROWID_read                                                     \r\n";
                    SQL += "  ,KOSMOS_OCS.FC_XRAY_CODE_NM(c.XCode) FC_XName                                                                         \r\n"; //코드명칭
                    SQL += "  ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Pano,c.BDate) FC_infect                                                       \r\n"; //감염체크
                    SQL += "  ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(a.Pano,c.BDate) FC_infect_EX                                                 \r\n"; //감염체크
                    SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_RESULTNEW a                                                                           \r\n";
                    SQL += "     , " + ComNum.DB_PMPA + "BAS_PATIENT b                                                                              \r\n";
                    SQL += "     , " + ComNum.DB_PMPA + "XRAY_DETAIL c                                                                              \r\n";
                    SQL += "    WHERE 1=1                                                                                                           \r\n";
                    SQL += "      AND a.Pano=b.Pano(+)                                                                                              \r\n";
                    SQL += "      AND a.Pano=c.Pano(+)                                                                                              \r\n";
                    SQL += "      AND a.WRTNO=c.Exinfo(+)                                                                                           \r\n";
                    if (argCls.Gubun1 == "1")
                    {
                        SQL += "  AND a.ReadDate >= TO_DATE('" + argCls.SDate + "', 'YYYY-MM-DD')                                                   \r\n";
                        SQL += "  AND a.ReadDate <=  TO_DATE('" + argCls.TDate + " 23:59','YYYY-MM-DD HH24:MI')                                     \r\n";
                    }

                    if (argCls.GubunSel == "03") //임시
                    {
                        SQL += "  AND EXISTS ( SELECT TEMP                                                                                      \r\n";
                        SQL += "                FROM " + ComNum.DB_PMPA + "XRAY_RESULTNEW b                                                     \r\n";
                        SQL += "                 WHERE 1=1                                                                                      \r\n";
                        SQL += "                   AND b.WRTNO= a.WRTNO                                                                        \r\n";
                        SQL += "                   AND b.TEMP ='Y' )                                                                            \r\n";
                    }

                    if (argCls.Search != "")
                    {
                        if (argCls.Gubun1 == "1")
                        {
                            SQL += "  AND a.SName LIKE '" + argCls.Search + "%'                                                                     \r\n";
                        }
                        else if (argCls.Gubun1 == "2")
                        {
                            if (argCls.Gubun3 == "A")
                            {
                                SQL += "  AND a.Pano = '" + argCls.Search + "'                                                                      \r\n";
                            }
                            else if (argCls.Gubun3 == "B")
                            {
                                SQL += "  AND a.SName LIKE '" + argCls.Search + "%'                                                                 \r\n";
                                SQL += "  AND a.ReadDate >= TO_DATE('" + Convert.ToDateTime(argCls.SDate).AddDays(-60).ToShortDateString() + "',     'YYYY-MM-DD')     \r\n";
                            }
                            else if (argCls.Gubun3 == "C")
                            {
                                SQL += "  AND a.SName LIKE '" + argCls.Search + "%'                                                                 \r\n";
                            }
                        }
                    }
                    if (argCls.GbSPC != "")
                    {
                        SQL += "  AND a.GbSPC ='1'                                                                                                  \r\n";
                    }
                    if (argCls.IPDOPD != "")
                    {
                        SQL += "  AND c.IPDOPD = '" + argCls.IPDOPD + "'                                                                            \r\n";
                    }

                    if (argCls.Gubun2 != "")
                    {                        
                        if (argCls.Gubun2 != "OT" && argCls.Gubun2 != "EC" && argCls.Gubun2 != "OG")
                        {
                            SQL += "  AND a.XJong IN ( " + argCls.XJong + " )                                                                       \r\n";
                        }

                        if (argCls.Gubun2 == "UR")
                        {
                            SQL += "  AND a.DeptCode IN ('UR')                                                                                      \r\n";
                            //2019-08-27 안정수, 비뇨기과 초음파코드 추가 
                            SQL += "  AND a.XCODE IN ('EB451', 'EB451A', 'EB454A', 'US15', 'US06U')                                             \r\n";                            
                        }
                        else if (argCls.Gubun2 == "OT")
                        {
                            SQL += "  AND (a.XJong in ('F') OR (a.XJong='N' AND (a.DeptCode ='TO' OR a.DeptCode ='HR') ))                           \r\n";
                            if (argCls.OTChk != "")
                            {
                                SQL += "  AND a.DeptCode ='" + argCls.OTChk + "'                                                                    \r\n";
                            }
                        }
                        else if (argCls.Gubun2 == "EC")
                        {
                            //2019-07-03 안정수 신규코드 추가 ('EB521A', 'EB561')
                            //2019-08-30 안정수 신규코드 추가('EB610A', 'US-TEE40', 'EB611')
                            SQL += "  AND (a.XJong = 'C' OR a.XCode IN ('US-TEE', 'US-TEER', 'US-CADU1','US-CADU2', 'US-CADUR', 'EB521A', 'EB561', 'EB610A', 'US-TEE40', 'EB611'))           \r\n";
                            //SQL += "  AND (a.XJong = 'C' OR a.XCode IN ('US-TEE', 'US-TEER', 'US-CADU1','US-CADU2', 'US-CADUR', 'EB521A', 'EB561')) \r\n";
                            //SQL += "  AND (a.XJong = 'C' OR a.XCode IN ('US-TEE', 'US-TEER', 'US-CADU1','US-CADU2', 'US-CADUR'))                    \r\n";
                            if (argCls.DrCodeSel != "****")
                            {
                                SQL += "  AND a.DrCode ='" + argCls.DrCodeSel + "'                                                                  \r\n";
                            }
                        }
                        //2020-01-14 안정수, OG 신규코드 추가 
                        else if (argCls.Gubun2 == "OG")
                        {
                            SQL += "  AND a.XCODE IN ('EB455', 'EB457', 'EB455001', 'EB457001')                                                 \r\n";
                            SQL += "  AND a.DEPTCODE = 'OG'                                                                                     \r\n";
                            SQL += "  AND a.DrCode = '" + argCls.DRCODE + "'                                                                    \r\n";
                        }
                    }
                    else
                    {
                        if (argCls.Job == "02")
                        {
                            SQL += "  AND a.APPROVE='Y'                                                                                             \r\n";
                        }
                        else
                        {
                            SQL += "  AND (a.APPROVE='N' OR a.APPROVE IS NULL)                                                                      \r\n";
                        }

                        if (argCls.XJong == "HR")
                        {
                            SQL += "  AND a.DeptCode ='HR'                                                                                          \r\n";
                        }
                        else if (argCls.XJong == "TO")
                        {
                            SQL += "  AND a.DeptCode ='TO'                                                                                          \r\n";
                        }
                        else if (argCls.XJong == "SONO")
                        {
                            SQL += "  AND a.XCODE = 'US04'                                                                                          \r\n"; //SONO BREAST
                        }
                        else
                        {
                            if (argCls.NotTO == "True")
                            {
                                SQL += "  AND a.DeptCode <> 'TO'                                                                                    \r\n";
                                SQL += "  AND a.XJong NOT IN ('6','7')                                                                              \r\n";
                                SQL += "  AND a.XCode NOT IN ('G2702','G2702B')                                                                     \r\n";
                            }
                            if (argCls.XJong != "*")
                            {
                                SQL += "  AND a.XJong ='" + argCls.XJong + "'                                                                       \r\n";
                            }

                            if (argCls.XDrCode1 > 0)
                            {
                                SQL += "  AND a.XDrCode1 =" + argCls.XDrCode1 + "                                                                   \r\n";
                            }
                        } 
                    } 
                    #endregion
                }
                else if (argCls.Gubun2 == "EEG")
                {
                    #region //etc_jupmst 기준테이블
                    SQL = "";
                    SQL += " SELECT                                                                                                             \r\n";
                    SQL += "  a.Pano,a.Sname,a.XJong,a.XCode                                                                                    \r\n";
                    SQL += "  ,TO_CHAR(c.RDate,'YYYY-MM-DD') SeekDate                                                                           \r\n";
                    SQL += "  ,TO_CHAR(c.RDate,'YYYY-MM-DD HH24:MI') SeekDateTime                                                               \r\n";
                    SQL += "  ,TO_CHAR(c.BDate,'YYYY-MM-DD') BDate                                                                              \r\n";
                    SQL += "  ,TO_CHAR(a.ReadDate,'YYYY-MM-DD') ReadDate                                                                        \r\n";
                    SQL += "  ,a.DeptCode,a.DrCode,a.IpdOpd,a.WardCode,a.RoomCode                                                               \r\n";
                    SQL += "  ,0 PacsNo,'' PacsStudyID,c.OrderNo,c.OrderCode,a.XName OrderName                                                  \r\n";
                    SQL += "  ,'' Remark,0 Exid,a.Sex,a.Age,a.GbSPC                                                                             \r\n";
                    SQL += "  ,'' GbRead,'' DrRemark,'' CVR,a.WRTNO Exinfo,0 EMGWRTNO                                                           \r\n";
                    SQL += "  ,b.Jumin1,b.Jumin2,a.Approve,a.Result, c.ROWID,a.ROWID ROWID_read                                                 \r\n";
                    SQL += "  ,KOSMOS_OCS.FC_OCS_ORDERCODE_NAME2(c.OrderCode) FC_XName                                                          \r\n"; //코드명칭
                    SQL += "  ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Pano,c.BDate) FC_infect                                                   \r\n"; //감염체크
                    SQL += "  ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(a.Pano,c.BDate) FC_infect_EX                                             \r\n"; //감염체크
                    SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_RESULTNEW a                                                                       \r\n";
                    SQL += "     , " + ComNum.DB_PMPA + "BAS_PATIENT b                                                                          \r\n";
                    SQL += "     , " + ComNum.DB_MED + "ETC_JUPMST c                                                                            \r\n";
                    SQL += "    WHERE 1=1                                                                                                       \r\n";
                    SQL += "      AND a.Pano=b.Pano(+)                                                                                          \r\n";
                    SQL += "      AND a.Pano=c.Ptno(+)                                                                                          \r\n";
                    SQL += "      AND a.WRTNO=c.Read_WRTNO(+)                                                                                   \r\n";
                    SQL += "      AND c.Gubun IN  ('2')                                                                                         \r\n"; //EEG
                    SQL += "      AND c.GbJob NOT IN  ('9')                                                                                     \r\n"; //EEG 취소제외
                    if (argCls.Gubun1 == "1")
                    {
                        SQL += "  AND a.ReadDate >= TO_DATE('" + argCls.SDate + "', 'YYYY-MM-DD')                                               \r\n";
                        SQL += "  AND a.ReadDate <=  TO_DATE('" + argCls.TDate + " 23:59','YYYY-MM-DD HH24:MI')                                 \r\n";
                    }
                    if (argCls.Search != "")
                    {
                        if (argCls.Gubun1 == "1")
                        {
                            SQL += "  AND a.SName LIKE '" + argCls.Search + "%'                                                                 \r\n";
                        }
                        else if (argCls.Gubun1 == "2")
                        {
                            if (argCls.Gubun3 == "A")
                            {
                                SQL += "  AND a.Pano = '" + argCls.Search + "'                                                                  \r\n";
                            }
                            else if (argCls.Gubun3 == "B")
                            {
                                SQL += "  AND a.SName LIKE '" + argCls.Search + "%'                                                             \r\n";
                                SQL += "  AND a.ReadDate >= TO_DATE('" + Convert.ToDateTime(argCls.SDate).AddDays(-60).ToShortDateString() + "',     'YYYY-MM-DD')     \r\n";
                            }
                            else if (argCls.Gubun3 == "C")
                            {
                                SQL += "  AND a.SName LIKE '" + argCls.Search + "%'                                                             \r\n";
                            }
                        }
                    }
                    if (argCls.GbSPC != "")
                    {
                        SQL += "  AND a.GbSPC ='1'                                                                                              \r\n";
                    }
                    if (argCls.IPDOPD != "")
                    {
                        SQL += "  AND c.GBIO = '" + argCls.IPDOPD + "'                                                                          \r\n";
                    }

                    if (argCls.Gubun2 != "")
                    {
                        SQL += "  AND a.XJong IN ( " + argCls.XJong + " )                                                                       \r\n";                        
                    }
                    else
                    {
                        if (argCls.Job == "02")
                        {
                            SQL += "  AND a.APPROVE='Y'                                                                                         \r\n";
                        }
                        else
                        {
                            SQL += "  AND (a.APPROVE='N' OR a.APPROVE IS NULL)                                                                  \r\n";
                        }

                        if (argCls.XJong == "HR")
                        {
                            SQL += "  AND a.DeptCode ='HR'                                                                                      \r\n";
                        }
                        else if (argCls.XJong == "TO")
                        {
                            SQL += "  AND a.DeptCode ='TO'                                                                                      \r\n";
                        }
                        else if (argCls.XJong == "SONO")
                        {
                            SQL += "  AND a.XCODE = 'US04'                                                                                      \r\n"; //SONO BREAST
                        }
                        else
                        {
                            if (argCls.NotTO == "True")
                            {
                                SQL += "  AND a.DeptCode <> 'TO'                                                                                \r\n";
                                SQL += "  AND a.XJong NOT IN ('6','7')                                                                          \r\n";
                                SQL += "  AND a.XCode NOT IN ('G2702','G2702B')                                                                 \r\n";
                            }
                            if (argCls.XJong != "*")
                            {
                                SQL += "  AND a.XJong ='" + argCls.XJong + "'                                                                   \r\n";
                            }

                            if (argCls.XDrCode1 > 0)
                            {
                                SQL += "  AND a.XDrCode1 =" + argCls.XDrCode1 + "                                                               \r\n";
                            }

                        }
                    }
                    #endregion
                }
            }          
            #endregion
                    

            SQL += " ORDER BY 2,1,3,4                                                                                                           \r\n";
             
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

        public DataTable sel_XrayRead_EtcJupmst(PsmhDb pDbCon, string argROWID)
        {
            DataTable dt = null;
            
            SQL = "";
            SQL += " SELECT                                                                                                             \r\n";
            SQL += "  a.Ptno Pano,a.Sname,'P' XJong,a.OrderCode XCode                                                                   \r\n";
            SQL += "  ,TO_CHAR(a.RDate,'YYYY-MM-DD') SeekDate                                                                           \r\n";
            SQL += "  ,TO_CHAR(a.RDate,'YYYY-MM-DD HH24:MI') SeekDateTime                                                               \r\n";
            SQL += "  ,TO_CHAR(a.BDate,'YYYY-MM-DD') BDate                                                                              \r\n";
            SQL += "  ,'' ReadDate                                                                                                      \r\n";
            SQL += "  ,a.DeptCode,a.DrCode,a.GbIO IpdOpd,'' WardCode,a.RoomCode,b.OrderName OrderName2                                  \r\n";
            SQL += "  ,'' PacsNo,'' PacsStudyID,a.OrderNo,a.OrderCode,'' OrderName,a.Remark                                             \r\n";
            SQL += "  ,'' GbRead,'' DrRemark,'' CVR,a.Read_Wrtno Exinfo,0 EMGWRTNO,0 Exid,a.Sex,a.Age,'' GbSPC                          \r\n";
            SQL += "  ,c.Jumin1,c.Jumin2,'' Approve, a.ROWID,'' ROWID_read                                                              \r\n";
            SQL += "  ,KOSMOS_OCS.FC_OCS_ORDERCODE_NAME2(a.OrderCode) FC_XName                                                          \r\n"; //코드명칭
            SQL += "  ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(a.Ptno,a.BDate) FC_infect                                                   \r\n"; //감염체크
            SQL += "  ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(a.Ptno,a.BDate) FC_infect_EX                                             \r\n"; //감염체크
            SQL += "  FROM " + ComNum.DB_MED + "ETC_JUPMST a                                                                            \r\n";
            SQL += "     , " + ComNum.DB_MED + "OCS_ORDERCODE b                                                                         \r\n";
            SQL += "     , " + ComNum.DB_PMPA + "BAS_PATIENT c                                                                          \r\n";
            SQL += "    WHERE 1=1                                                                                                       \r\n";
            SQL += "    AND a.OrderCode = b.OrderCode(+)                                                                                \r\n";
            SQL += "    AND a.Ptno = c.Pano(+)                                                                                          \r\n";
            SQL += "    AND a.Gubun IN  ('2')                                                                                           \r\n"; //EEG
            SQL += "    AND a.GbJob NOT IN  ('9')                                                                                       \r\n"; //EEG 취소제외            
            SQL += "    AND a.ROWID IN ( " + argROWID + " )                                                                             \r\n";                     

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

        /// <summary>
        /// 판독 미판독 관련 조회용 쿼리
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public DataTable sel_XrayDetailView(PsmhDb pDbCon, cXrayViewList argCls)
        {
            DataTable dt = null;

            //미판독, 판독의뢰
            if (argCls.optJob1 == "True")
            {
                //xray_detail
                SQL = "";
                SQL += " SELECT                                                                         \r\n";
                SQL += "  Pano,Sname,DeptCode,DrCode,IpdOpd,WardCode,OrderCode,RoomCode,XJong,          \r\n";
                SQL += "  TO_CHAR(SeekDate,'YYYY-MM-DD') SeekDate,                                      \r\n";
                SQL += "  XCode,Remark,Exid,Sex,Age,XCode XName,Exinfo WRTNO,'' Result,ROWID            \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                        \r\n";
                SQL += "    WHERE 1=1                                                                   \r\n";
                SQL += "  AND SeekDate >= TO_DATE('" + argCls.SDate + "', 'YYYY-MM-DD')                 \r\n";
                SQL += "  AND SeekDate <= TO_DATE('" + argCls.TDate + " 23:59', 'YYYY-MM-DD HH24:MI')   \r\n";
                SQL += "  AND GbReserved IN ('1','6','7','8')                                           \r\n";
                SQL += "  AND (ExInfo < 2 OR ExInfo IS NULL)                                            \r\n";

                if (argCls.SName != "")
                {
                    SQL += "  AND SName LIKE '" + argCls.SName + "%'                                    \r\n";
                }
                if (argCls.Pano != "")
                {
                    SQL += "  AND Pano = '" + argCls.Pano + "'                                          \r\n";
                }
                if (argCls.DeptCode != "**")
                {
                    SQL += "  AND DeptCode = '" + argCls.DeptCode + "'                                  \r\n";
                }
                if (argCls.DrCode != "****")
                {
                    SQL += "  AND DrCode = '" + argCls.DrCode + "'                                      \r\n";
                }
                if (argCls.XJong != "*")
                {
                    SQL += "  AND XJong = '" + argCls.XJong + "'                                        \r\n";
                }

            }
            else
            {
                //xray_resultnew
                SQL = "";
                SQL += " SELECT                                                                         \r\n";
                SQL += "  Pano,Sname,DeptCode,DrCode,IpdOpd,WardCode,'' OrderCode,RoomCode,XJong,       \r\n";
                SQL += "  TO_CHAR(SeekDate,'YYYY-MM-DD') SeekDate,                                      \r\n";
                SQL += "  XCode,'' Remark,0 Exid,Sex,Age,XName,WRTNO,Result,ROWID                       \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_RESULTNEW                                     \r\n";
                SQL += "    WHERE 1=1                                                                   \r\n";
                SQL += "  AND ReadDate >= TO_DATE('" + argCls.SDate + "', 'YYYY-MM-DD')                 \r\n";
                SQL += "  AND ReadDate <= TO_DATE('" + argCls.TDate + " 23:59', 'YYYY-MM-DD HH24:MI')   \r\n";

                if (argCls.SName != "")
                {
                    SQL += "  AND SName LIKE '" + argCls.SName + "%'                                    \r\n";
                }
                if (argCls.Pano != "")
                {
                    SQL += "  AND Pano = '" + argCls.Pano + "'                                          \r\n";
                }
                if (argCls.DeptCode != "**")
                {
                    SQL += "  AND DeptCode = '" + argCls.DeptCode + "'                                  \r\n";
                }
                if (argCls.DrCode != "****")
                {
                    SQL += "  AND DrCode = '" + argCls.DrCode + "'                                      \r\n";
                }
                if (argCls.XJong != "*")
                {
                    SQL += "  AND XJong = '" + argCls.XJong + "'                                        \r\n";
                }

            }

            SQL += " ORDER BY 1,2,3                                                                     \r\n";


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

        /// <summary>
        /// 판독대상코드에서 제외되는 쿼리
        /// </summary>
        /// <returns></returns>
        public string read_XrayResultNotCode(PsmhDb pDbCon)
        {
            string strVal = "";

            DataTable dt = sel_BasBCode(pDbCon, "1", "XRAY_판독제외코드");

            if (dt == null) return "";
            if (dt.Rows.Count == 0) return "";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strVal += "'" + dt.Rows[i]["Code"].ToString().Trim() + "',";
            }

            if (strVal != "")
            {
                strVal = VB.Mid(strVal, 1, VB.Len(strVal) - 1);
            }

            return strVal;


        }

        /// <summary>
        /// 판독명단 조회에 사용
        /// </summary>
        /// <param name="Job"></param>
        /// <param name="argPano"></param>
        /// <param name="argDept"></param>
        /// <param name="argExinfo"></param>
        /// <param name="argDate"></param>
        /// <param name="argSeq"></param>
        /// <param name="argIO"></param>
        /// <returns></returns>
        
        public DataTable sel_XRAY_RESULTNEW(PsmhDb pDbCon, cXray_ResultNew argCls,bool bLog)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                             \r\n";
            SQL += "   Pano,GbAnat,WRTNO,ROWID                                                          \r\n";
            SQL += "  ,XJong,XCode,XName,Result,Result1,XName OrderName                                 \r\n";
            SQL += "  ,ResultEC,ResultEC1,0 OrderNo                                                     \r\n";
            SQL += "  ,XDrCode1,XDrCode2,XDrCode3,Approve                                               \r\n";
            SQL += "  ,Part,Part2,Temp                                                                  \r\n"; //2018-07-12 안정수 Temp 칼럼 추가 
            SQL += "  ,ADDENDUM1,ADDENDUM2,IllCode1,IllCode2,IllCode3                                   \r\n";
            SQL += "  ,TO_CHAR(SeekDate,'YYYY-MM-DD') SeekDate                                          \r\n";
            SQL += "  ,TO_CHAR(ReadDate,'YYYY-MM-DD') ReadDate                                          \r\n";
            SQL += "  ,PanHic                                                                           \r\n"; //2018-09-07 안정수 PanHic 칼럼 추가
            SQL += "  ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('C#_XRAY_접수종류',TRIM(XJong)) FC_XJong2       \r\n"; //종류체크
            SQL += "  ,KOSMOS_OCS.FC_XRAY_CODE_NM(XCode) FC_XName                                       \r\n"; //코드명칭             
            SQL += "  ,KOSMOS_OCS.FC_XRAY_PACSUID_CHK(ROWID) FC_UID                                     \r\n"; //UID             
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_RESULTNEW                                        \r\n";
            SQL += "  WHERE 1 = 1                                                                       \r\n";
            if (argCls.Job == "00")
            {
                SQL += "  AND Pano = '" + argCls.Pano + "'                                              \r\n";
            }
            else if (argCls.Job == "01")
            {
                SQL += "  AND ROWID = '" + argCls.ROWID + "'                                            \r\n";
            }
            else if (argCls.Job == "02")
            {
                SQL += "  AND WRTNO = " + argCls.WRTNO + "                                              \r\n";
            }
            else if (argCls.Job == "03")
            {
                SQL += "  AND Pano = '" + argCls.Pano + "'                                              \r\n";
                SQL += "  AND ROWNUM <= 100                                                             \r\n";
            }
            else if (argCls.Job == "04")
            {
                SQL += "  AND Pano = '" + argCls.Pano + "'                                              \r\n";
                SQL += "  AND SeekDate >= TO_DATE('" + argCls.SDate + "','YYYY-MM-DD')                  \r\n";
                SQL += "  AND SeekDate <= TO_DATE('" + argCls.TDate + " 23:59','YYYY-MM-DD HH24:MI')    \r\n";
                if (argCls.AnatChk == "Y")
                {
                    SQL += "  AND (GbAnat IS NULL OR GbAnat <> 'Y')                                     \r\n";
                }
                if (argCls.MyChk == "Y")
                {
                    SQL += "  AND XDrCode1 = " + argCls.XDrCode1 + "                                   \r\n";
                }
            }
            else
            {

            }
            if (argCls.Job == "00")
            {
                SQL += "  ORDER BY ReadDate DESC,XJong,XCode                                            \r\n";
            }
            else if (argCls.Job == "03")
            {
                SQL += "  ORDER BY ReadDate DESC,XJong,XCode                                            \r\n";
            }
            else if (argCls.Job == "04")
            {
                SQL += "  ORDER BY ReadDate DESC                                            \r\n";
            }

            try
            {                
                if (bLog == true)
                {
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                }
                else
                {
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                }

                if (SqlErr != "")
                {
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

        public DataTable sel_XRAY_RESULTNEW(PsmhDb pDbCon, string argJob, string argPano, string argPacsNo, long ReadNo)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                     \r\n";
            SQL += "  Pano,Temp,Approve,Result,Result1                          \r\n";
            SQL += "  ,ROWID                                                    \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_RESULTNEW                \r\n";
            SQL += "  WHERE 1 = 1                                               \r\n";
            if (argJob == "00")
            {
                SQL += "   AND Pano ='" + argPano + "'                          \r\n";
            }
            else if (argJob == "01")
            {
                if (argPano != "")
                {
                    SQL += "   AND Pano ='" + argPano + "'                      \r\n";
                }
                if (argPacsNo != "")
                {
                    SQL += "   AND PacsNo ='" + argPacsNo + "'                  \r\n";
                }
            }
            else if (argJob == "02")
            {
                SQL += "   AND Pano ='" + argPano + "'                          \r\n";
                SQL += "   AND WRTNO =" + ReadNo + "                            \r\n";
            }
            else if (argJob == "03")
            {
                SQL += "   AND WRTNO =" + ReadNo + "                            \r\n";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
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

        public DataTable sel_XRAY_RESULTNEW(PsmhDb pDbCon, string argDate)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                     \r\n";
            SQL += "  DECODE(XDrCode1,'27069','33781','23696','33852',XDrCode1) XDrCode1                        \r\n";
            SQL += "  ,SUM(DECODE(XJong,'1',1,0)) IlbanCnt                                                      \r\n";
            SQL += "  ,SUM(DECODE(XJong,'2',1,0)) TuksuCnt                                                      \r\n";
            SQL += "  ,SUM(DECODE(XJong,'3',1,0)) SonoCnt                                                       \r\n";
            SQL += "  ,SUM(DECODE(XJong,'4',1,0)) CtCnt                                                         \r\n";
            SQL += "  ,SUM(DECODE(XJong,'5',1,0)) MriCnt                                                        \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_RESULTNEW                                                \r\n";
            SQL += "  WHERE 1 = 1                                                                               \r\n";
            SQL += "   AND ReadDate = TO_DATE('" + argDate + "','YYYY-MM-DD')                                   \r\n";
            SQL += "   AND XDRCODE1 IN ('23696','27069','22115','28039'                                         \r\n";
            SQL += "                    ,'29377','32111','99001','33781','33852')                               \r\n";
            SQL += "   GROUP BY DECODE(XDrCode1,'27069','33781','23696','33852',XDrCode1)                       \r\n";
            SQL += "    ORDER BY XDrCode1                                                                       \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
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

        public DataTable sel_XRAY_RESULTNEW_detail(PsmhDb pDbCon, cXray_ResultNew argCls)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                             \r\n";
            SQL += "   a.Pano,a.SName,a.DeptCode,a.DrCode,a.IpdOpd,a.WardCode                           \r\n";
            SQL += "  ,a.XName,b.Remark,b.DrRemark,b.OrderName,b.ROWID                                  \r\n";
            SQL += "  ,a.RoomCode,a.XJong,a.XCode,a.Sex,a.Age,a.WRTNO,b.EMGWRTNO                        \r\n";
            SQL += "  ,b.OrderNo                                                                        \r\n";
            SQL += "  ,TO_CHAR(b.BDate,'YYYY-MM-DD') BDate                                              \r\n";
            SQL += "  ,TO_CHAR(a.SeekDate,'YYYY-MM-DD') SeekDate                                        \r\n";
            SQL += "  ,TO_CHAR(a.ReadDate,'YYYY-MM-DD') ReadDate                                        \r\n";
            SQL += "  ,KOSMOS_OCS.FC_ETC_RESULT_CNT(b.EMGWRTNO) FC_EMGCNT                               \r\n";  //EMG영상건수
            SQL += "  ,KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(a.DrCode) FC_DrName                              \r\n";  //의사명
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_RESULTNEW a                                      \r\n";
            SQL += "      , " + ComNum.DB_PMPA + "XRAY_DETAIL b                                         \r\n";
            SQL += "  WHERE 1 = 1                                                                       \r\n";
            SQL += "  AND a.Pano=b.Pano                                                                 \r\n";
            SQL += "  AND a.XJong =b.XJong                                                              \r\n";
            SQL += "  AND a.XCode =b.XCode                                                              \r\n";
            SQL += "  AND a.WRTNO =b.EXINFO                                                             \r\n";
            if (argCls.Job == "00")
            {
                SQL += "  AND ReadDate >= TO_DATE('" + argCls.SDate + "','YYYY-MM-DD')                  \r\n";
                SQL += "  AND ReadDate <= TO_DATE('" + argCls.TDate + "','YYYY-MM-DD')                  \r\n";
                SQL += "  AND a.XJong = 'E'                                                             \r\n";
                if (argCls.Gubun1 == "IPD")
                {
                    SQL += "  AND b.IPDOPD='I'                                                          \r\n";
                    SQL += "  AND a.DRCODE <>'2601'                                                     \r\n";
                    SQL += "  AND a.XCODE NOT IN ('FA183')                                              \r\n";
                }
                else if (argCls.Gubun1 == "OPD")
                {
                    SQL += "  AND b.IPDOPD='O'                                                          \r\n";
                }
                else if (argCls.Gubun1 == "ER")
                {
                    SQL += "  AND b.DeptCode ='ER'                                                      \r\n";
                }
            }
            else
            {

            }
            if (argCls.Job == "00")
            {
                SQL += "  ORDER BY a.SName,a.Pano,a.XJong,a.XCode                                       \r\n";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
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
        
        public DataTable sel_XRAY_RESULTSET(PsmhDb pDbCon, string argJob, long argSabun)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                     \r\n";
            SQL += "  XJong,SetName,XName,Result1,Result2,ROWID                 \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_RESULTSET                \r\n";
            SQL += "  WHERE 1 = 1                                               \r\n";
            if (argJob == "00")
            {
                SQL += "   AND Sabun = " + argSabun + "                         \r\n";
            }
            else if (argJob == "01")
            {

                SQL += "   AND Sabun = " + argSabun + "                         \r\n";
                SQL += "   AND XJong = '2'                                      \r\n";
                SQL += "   AND SetName LIKE 'MM-%'                              \r\n";
            }
            else if (argJob == "02")
            {

                SQL += "   AND Sabun = " + argSabun + "                         \r\n";
                SQL += "   AND XJong = '2'                                      \r\n";
                SQL += "   AND SetName LIKE 'MMM-%'                             \r\n";
            }

            if (argJob == "01" || argJob == "02")
            {
                SQL += "   ORDER BY XJong,SetName                               \r\n";
            }
            else
            {

            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
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

        /// <summary>
        ///  영상의 판독 상용결과 쿼리
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public DataTable sel_Xray_ResultSet(PsmhDb pDbCon, long argSabun, string argPart, string argXJong, string argSearch = "")
        {
            DataTable dt = null;

            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT                                                                             \r\n";
                SQL += "   XJong,SetName,XName,Result1,Result2                                              \r\n";
                SQL += "   ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('XRAY_방사선종류',TRIM(XJong)) FC_XJONG        \r\n";
                SQL += "   ,ROWID                                        \r\n";
                if (argPart == "HIC")
                {
                    SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_ResultSet_Hic                                \r\n";
                }
                else
                {
                    SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_ResultSet                                    \r\n";
                }
                SQL += "  WHERE 1 = 1                                                                       \r\n";
                if (argSabun != 0)
                {
                    SQL += "   AND Sabun = " + argSabun + "                                                 \r\n";
                }
                if (argPart != "")
                {
                    if (argXJong == "HIC" || argXJong == "ALL")
                    {

                    }
                    else
                    {
                        if (argXJong != "")
                        {
                            SQL += "   AND XJong = '" + argXJong + "'                                           \r\n";
                        }

                    }

                }
                if (argSearch != "")
                {
                    SQL += "   AND (                                                                        \r\n";
                    SQL += "     UPPER(SetName) LIKE '%" + argSearch.ToUpper() + "%'                        \r\n";
                    SQL += "     OR  UPPER(XName) LIKE '%" + argSearch.ToUpper() + "%'                      \r\n";
                    SQL += "     OR  UPPER(Result1) LIKE '%" + argSearch.ToUpper() + "%'                    \r\n";
                    SQL += "      )                                                                         \r\n";
                }
                SQL += "   ORDER BY XJong,SetName                                                           \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }

            return dt;

        }

        public class cXrayPatient
        {
            public string Job = "";
            public string Pano = "";
            public string SName = "";
            public string XCode = "";
            public string XName = "";            
            public long WRTNO = 0;
            public string PacsNo = "";
            public string ROWID = "";
        }

        public DataTable sel_XRAY_RESULTNEW_Sup(PsmhDb pDbCon, cHic_Xray_Result argCls)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                         \r\n";
            SQL += "  a.Pano,a.SName,a.XJong,a.SName,a.Sex,a.Age,a.IpdOpd,a.DeptCode,a.DrCode       \r\n";
            SQL += " ,a.WardCode,a.RoomCode,XDrCode1,XDrCode2                                           \r\n";
            SQL += " ,XDrCode3,IllCode1,IllCode2,IllCode3,a.XCode,XName                               \r\n";
            //2020-01-10 안정수, READTIME, ENTDATE 추가 
            SQL += " ,Result,Result1,Approve, 0 DRWRTNO,ADDENDUM1, ADDENDUM2, a.ROWID         \r\n";
            SQL += " ,TO_CHAR(enterdate,'YYYY-MM-DD HH24:MI') ENTDATE                               \r\n";
            SQL += " ,TO_CHAR(READTIME,'YYYY-MM-DD HH24:MI') READTIME                                      \r\n";
            SQL += " ,TO_CHAR(ReadDate,'YYYY-MM-DD HH24:MI') ReadDate                                       \r\n";
            SQL += " ,TO_CHAR(b.SeekDate,'YYYY-MM-DD HH24:MI') SeekDate                                       \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_RESULTNEW a, " + ComNum.DB_PMPA + "xray_detail b        \r\n";
            SQL += "  WHERE 1 = 1                                                                   \r\n";
            SQL += "  and a.pano =b.pano                                                                   \r\n";
            SQL += "  and a.wrtno =b.exinfo(+)                                                               \r\n";
            if (argCls.Job == "00")
            {
                SQL += "   AND ROWID ='" + argCls.ROWID + "'                                        \r\n";
            }
            else if (argCls.Job == "01")
            {
                SQL += "   AND Pano ='" + argCls.PTNO + "'                                          \r\n";
            }
            else if (argCls.Job == "02")
            {
                SQL += "   AND WRTNO =" + argCls.WRTNO + "                                          \r\n";
            }
            else
            {
                SQL += "   AND Pano ='" + argCls.PTNO + "'                                          \r\n";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
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

        /// <summary>
        /// 영상 판독 쿼리
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argCls"></param>
        /// <returns></returns>
        public DataTable sel_XRAY_RESULTNEW(PsmhDb pDbCon, cHic_Xray_Result argCls)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                         \r\n";
            SQL += "  Pano,SName,XJong,SName,Sex,Age,IpdOpd,DeptCode,DrCode                         \r\n";
            SQL += " ,WardCode,RoomCode,XDrCode1,XDrCode2                                           \r\n";
            SQL += " ,XDrCode3,IllCode1,IllCode2,IllCode3,XCode,XName                               \r\n";
            //2020-01-10 안정수, READTIME, ENTDATE 추가 
            SQL += " ,Result,Result1,Approve, 0 DRWRTNO,ADDENDUM1, ADDENDUM2,ENTDATE, ROWID         \r\n";
            SQL += " ,TO_CHAR(READTIME,'YYYY-MM-DD HH24:MI') READTIME                               \r\n";
            SQL += " ,TO_CHAR(ReadDate,'YYYY-MM-DD') ReadDate                                       \r\n";
            SQL += " ,TO_CHAR(SeekDate,'YYYY-MM-DD') SeekDate                                       \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_RESULTNEW                                    \r\n";
            SQL += "  WHERE 1 = 1                                                                   \r\n";
            if (argCls.Job == "00")
            {
                SQL += "   AND ROWID ='" + argCls.ROWID + "'                                        \r\n";
            }
            else if (argCls.Job == "01")
            {
                SQL += "   AND Pano ='" + argCls.PTNO + "'                                          \r\n";
            }
            else if (argCls.Job == "02")
            {
                SQL += "   AND WRTNO =" + argCls.WRTNO + "                                          \r\n";
            }
            else
            {
                SQL += "   AND Pano ='" + argCls.PTNO + "'                                          \r\n";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
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
        
        /// <summary>
        /// 처방의 판독 쿼리
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argCls"></param>
        /// <returns></returns>
        public DataTable sel_XRAY_RESULTNEW_DR(PsmhDb pDbCon, cHic_Xray_Result argCls)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                         \r\n";
            SQL += "  Pano,SName,XJong,SName,Sex,Age,IpdOpd,DeptCode,DrCode                         \r\n";
            SQL += " ,WardCode,RoomCode,XDrCode1,XDrCode2                                           \r\n";
            SQL += " ,XDrCode3,IllCode1,IllCode2,IllCode3,XCode,XName                               \r\n";
            SQL += " ,Result,Result1,Approve, DRWRTNO,ROWID                                         \r\n";
            SQL += " ,TO_CHAR(ReadDate,'YYYY-MM-DD') ReadDate                                       \r\n";
            SQL += " ,TO_CHAR(SeekDate,'YYYY-MM-DD') SeekDate                                       \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_RESULTNEW_DR                                 \r\n";
            SQL += "  WHERE 1 = 1                                                                   \r\n";
            if (argCls.Job == "00")
            {
                SQL += "   AND ROWID ='" + argCls.ROWID + "'                                        \r\n";
            }
            else if (argCls.Job == "01")
            {
                SQL += "   AND Pano ='" + argCls.PTNO + "'                                          \r\n";
            }
            else if (argCls.Job == "02")
            {
                SQL += "   AND DRWRTNO =" + argCls.WRTNO + "                                        \r\n";
            }
            else
            {
                SQL += "   AND Pano ='" + argCls.PTNO + "'                                          \r\n";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
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

        /// <summary>
        /// 건진 Chest영상 판독 쿼리
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argCls"></param>
        /// <returns></returns>
        public DataTable sel_HIC_XRAY_RESULT(PsmhDb pDbCon, cHic_Xray_Result argCls)
        {
            DataTable dt = null;


            SQL = "";
            SQL += " SELECT                                                                             \r\n";
            SQL += "  Pano,Ptno,XRAYNO,Sex,Age                                                          \r\n";
            SQL += "  ,GjJong,LtdCode,SName                                                             \r\n";
            SQL += "  ,ReadDoct1,ReadDoct2,Result1,Result1_1,Result2,Result2_1,Result3,Result3_1        \r\n";
            SQL += "  ,Result4,Result4_1,GbSTS,XCode,GbRead,GbPacs                                      \r\n";            
            SQL += "  ,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate                                            \r\n";
            SQL += "  ,TO_CHAR(JepDate,'YYYYMMDD') JepDate2                                             \r\n";
            SQL += "  ,TO_CHAR(ReadTime1,'YYYY-MM-DD') ReadDate                                         \r\n";
            SQL += "  ,ROWID,EXID                                                                       \r\n";
            SQL += "  ,KOSMOS_OCS.FC_HIC_EXJONG_NM(GjJong) FC_ExJongName                                \r\n";
            SQL += "  ,KOSMOS_OCS.FC_HIC_EXCODE_NM(XCode) FC_ExName                                     \r\n";
            SQL += "  ,KOSMOS_OCS.FC_HIC_LTD_NM(LtdCode) FC_LtdName                                     \r\n";
            SQL += "  ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(Ptno,JepDate) FC_infect                     \r\n"; //감염체크
            SQL += "  ,KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(Ptno,JepDate) FC_infect_EX               \r\n"; //감염체크
            SQL += "  ,KOSMOS_OCS.FC_BAS_PATIENT_JUMINNO(Ptno) FC_JUMIN                                 \r\n"; //주민번호            
            SQL += "   FROM " + ComNum.DB_PMPA + "HIC_XRAY_RESULT                                       \r\n";
            SQL += "  WHERE 1 = 1                                                                       \r\n";
           
            if (argCls.Job == "00")
            {
                SQL += "   AND Ptno ='" + argCls.PTNO + "'                                              \r\n";
                SQL += "   AND JepDate >= TO_DATE('" + argCls.JEPDATE + "','YYYY-MM-DD')                \r\n";
                SQL += "   AND XCODE = 'A142'                                                           \r\n";
                SQL += "   AND GJJONG IN ('11','83')                                                    \r\n";
            }
            else if (argCls.Job == "01")
            {
                SQL += "   AND Pano =" + argCls.HPANO + "                                               \r\n";
                SQL += "   AND XRayNo = '" + argCls.XRAYNO + "'                                         \r\n";
                SQL += "   AND DelDate IS NULL                                                          \r\n";
            }
            else if (argCls.Job == "02") //검진판독 쿼리
            {
                SQL += "   AND JepDate >= TO_DATE('" + argCls.Date1 + "','YYYY-MM-DD')                  \r\n";
                SQL += "   AND JepDate <= TO_DATE('" + argCls.Date2 + "','YYYY-MM-DD')                  \r\n";
                if (argCls.PTNO != "")
                {
                    SQL += "   AND Ptno ='" + argCls.PTNO + "'                                          \r\n";
                }
                if (argCls.GJJONG != "**")
                {
                    //SQL += "   AND GJJONG = '" + argCls.GJJONG + "'                                     \r\n";
                    SQL += "   AND GJJONG IN ('" + argCls.GJJONG + "')                                   \r\n";

                    //2019-07-30 안정수, 폐암 판독 관련 조건 추가
                    if (argCls.GJJONG == "31")
                    {
                        SQL += "   AND XCODE = 'TY10'                                                   \r\n";
                    }
                }
                else
                {
                    SQL += "   AND XCODE NOT IN ('TY10')                                                \r\n";
                }
                if (argCls.LTDCODE != "")
                {
                    SQL += "   AND LtdCode ='" + argCls.LTDCODE + "'                                    \r\n";
                }

                if (argCls.GubunA == "미판독")
                {
                    SQL += "   AND GbSTS IN  ('0','1')                                                  \r\n";
                }
                else if (argCls.GubunA == "판독")
                {
                    SQL += "   AND GbSTS IN  ('2')                                                      \r\n";
                }
                else if (argCls.GubunA == "보류")
                {
                    SQL += "   AND GbSTS IN  ('P')                                                      \r\n";
                }

                if (argCls.GBREAD != "*")
                {
                    if (argCls.GBREAD == "1")
                    {
                        SQL += "   AND (GbRead = '" + argCls.GBREAD + "' OR GbRead IS NULL)             \r\n";
                    }
                    else
                    {
                        SQL += "   AND GbRead = '" + argCls.GBREAD + "'                                     \r\n";
                    }
                }

                if (argCls.GBCHUL != "*")
                {
                    SQL += "   AND GbChul = '" + argCls.GBCHUL + "'                                     \r\n";
                }

                SQL += "   AND GbOrder_Send ='Y'                                                        \r\n";
                SQL += "   AND GbPacs ='Y'                                                              \r\n";

            }
            else if (argCls.Job == "03")
            {
                SQL += "   AND Ptno ='" + argCls.PTNO + "'                                              \r\n";
            }
            else if (argCls.Job == "04")
            {
                SQL += "   AND ROWID ='" + argCls.ROWID + "'                                            \r\n";
            }
            else
            {
                SQL += "   AND Ptno ='" + argCls.PTNO + "'                                              \r\n";
            }

            SQL += "   AND (DelDate IS NULL OR DelDate ='')                                             \r\n";

            if (argCls.Job == "02")
            {
                SQL += "   ORDER BY JepDate, XrayNo                                                     \r\n";
            }
            else if (argCls.Job == "03")
            {
                SQL += "   ORDER BY JepDate DESC                                                        \r\n";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
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

        public DataTable sel_HIC_JEPSU(PsmhDb pDbCon,string argTable, string argDate, string argPano)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                             \r\n";
            if (argTable == "HIC_SCHOOL_NEW")
            {
                SQL += "  A.JEPDATE, C.JEPDATE, A.PANO                                                                  \r\n";
                SQL += "  , A.WRTNO, A.SEX, A.AGE, A.SNAME, B.PPANDRNO BPANJENGDRNO, B.RDATE BRDATE                     \r\n";
            }
            else
            {
                SQL += "  A.JEPDATE, C.JEPDATE, A.PANO                                                                  \r\n";
                SQL += "  , A.WRTNO, A.SEX, A.AGE, A.SNAME, B.PANJENGDRNO BPANJENGDRNO, B.PANJENGDATE BPANJENGDATE      \r\n";
            }
            
            SQL += "   FROM " + ComNum.DB_PMPA + "HIC_JEPSU a                                                           \r\n";            
            SQL += "      , " + ComNum.DB_PMPA + argTable  + " b                                                        \r\n";
            SQL += "      , " + ComNum.DB_PMPA + "HIC_XRAY_RESULT c                                                     \r\n";
            SQL += "  WHERE 1 = 1                                                                                       \r\n";
            SQL += "   AND a.WRTNO = b.WRTNO                                                                            \r\n";
            SQL += "   AND a.PANO = c.PANO                                                                              \r\n";
            
            if (argTable == "HIC_SCHOOL_NEW")
            {
                SQL += "   AND b.PPANDRNO > 0                                                                            \r\n";
            }
            else
            {
                SQL += "   AND b.PANJENGDRNO > 0                                                                        \r\n";
            }
            SQL += "   AND a.DELDATE IS NULL                                                                            \r\n";
            SQL += "   AND c.DELDATE IS NULL                                                                            \r\n";
            SQL += "   AND a.JEPDATE = TO_DATE('" + argDate + "', 'YYYY-MM-DD')                                         \r\n";
            SQL += "   AND c.JEPDATE = TO_DATE('" + argDate + "', 'YYYY-MM-DD')                                         \r\n";
            SQL += "   AND a.Pano =" + argPano + "                                                                      \r\n";
            if (argTable == "HIC_SCHOOL_NEW")
            {
                SQL += "  GROUP BY A.JEPDATE, C.JEPDATE, A.PANO                                                         \r\n";
                SQL += "          ,A.WRTNO, A.SEX, A.AGE, A.SNAME, B.PPANDRNO , B.RDATE                                 \r\n";
            }
            else
            {
                SQL += "  GROUP BY A.JEPDATE, C.JEPDATE, A.PANO                                                         \r\n";
                SQL += "          ,A.WRTNO, A.SEX, A.AGE, A.SNAME, B.PANJENGDRNO, B.PANJENGDATE                         \r\n";
            }
            

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
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

        public DataTable sel_HEA_JEPSU(PsmhDb pDbCon, string argDate, string argPano)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                             \r\n";            
            SQL += "  A.PTNO, A.JEPDATE, A.SNAME, A.GBSTS, A.DRSABUN BPANJENGDRNO                                       \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "HEA_JEPSU a                                                           \r\n";            
            SQL += "      , " + ComNum.DB_PMPA + "HIC_XRAY_RESULT b                                                     \r\n";
            SQL += "  WHERE 1 = 1                                                                                       \r\n";            
            SQL += "   AND a.PtNO = b.PtNO                                                                              \r\n";
            SQL += "   AND a.GBSTS = '9'                                                                                \r\n";
            SQL += "   AND a.DELDATE IS NULL                                                                            \r\n";
            SQL += "   AND b.DELDATE IS NULL                                                                            \r\n";
            SQL += "   AND a.sDATE = TO_DATE('" + argDate + "', 'YYYY-MM-DD')                                           \r\n";
            SQL += "   AND b.JEPDATE = TO_DATE('" + argDate + "', 'YYYY-MM-DD')                                         \r\n";
            SQL += "   AND a.Ptno =" + argPano + "                                                                      \r\n";            
            
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
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

        public DataTable sel_HIC_RESULT(PsmhDb pDbCon, string argPano,string argDate1, string argDate2, string argExCode)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                 \r\n";
            SQL += "  ExCode,Result                                                                         \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "HIC_RESULT                                                \r\n";            
            SQL += "  WHERE 1 = 1                                                                           \r\n";
            SQL += "   AND ExCode IN (" + argExCode + " )                                                   \r\n";
            SQL += "   AND WRTNO IN ( SELECT WRTNO                                                          \r\n";
            SQL += "                    FROM " + ComNum.DB_PMPA + "HIC_JEPSU                                \r\n";
            SQL += "                      WHERE 1 = 1                                                       \r\n";
            SQL += "                       AND PTNO = '" + argPano + "'                                     \r\n";
            SQL += "                        AND JepDate >=TO_DATE('" + argDate1 + "','YYYY-MM-DD')          \r\n";
            SQL += "                        AND JepDate <=TO_DATE('" + argDate2 + "','YYYY-MM-DD')          \r\n";
            SQL += "                        AND DelDate IS NULL )                                           \r\n";
            
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
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

        public DataTable sel_HEA_RESULT(PsmhDb pDbCon, string argPano, string argDate1, string argDate2, string argExCode)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                 \r\n";
            SQL += "  ExCode,Result                                                                         \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "HEA_RESULT                                                \r\n";
            SQL += "  WHERE 1 = 1                                                                           \r\n";
            SQL += "   AND ExCode IN (" + argExCode + " )                                                   \r\n";
            SQL += "   AND WRTNO IN ( SELECT WRTNO                                                          \r\n";
            SQL += "                    FROM " + ComNum.DB_PMPA + "HEA_JEPSU                                \r\n";
            SQL += "                      WHERE 1 = 1                                                       \r\n";
            SQL += "                       AND PTNO = '" + argPano + "'                                     \r\n";
            SQL += "                        AND SDate >=TO_DATE('" + argDate1 + "','YYYY-MM-DD')            \r\n";
            SQL += "                        AND SDate <=TO_DATE('" + argDate2 + "','YYYY-MM-DD')            \r\n";
            SQL += "                        AND DelDate IS NULL )                                           \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
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

        /// <summary>
        /// 판독 제외 코드 쿼리시 사용 bas_bcode
        /// </summary>
        /// <param name="strFlag"></param>
        /// <param name="strGubun"></param>
        /// <param name="strData"></param>
        /// <returns></returns>
        public DataTable sel_BasBCode(PsmhDb pDbCon, string strFlag, string strGubun, string strData = "")
        {
            DataTable dt = null;

            if (strFlag.Trim() != "1" && strFlag.Trim() != "2") return null;

            SQL = "";
            SQL += "SELECT " + (strFlag.Trim() == "1" ? "CODE" : "NAME") + " ";
            SQL += " FROM KOSMOS_PMPA.BAS_BCODE ";
            SQL += " WHERE 1=1 ";
            SQL += "  AND GUBUN = '" + strGubun + "' ";
            if (strData != "") SQL += "   AND " + (strFlag.Trim() == "1" ? "NAME" : "CODE") + " ='" + strData.Trim() + "' ";

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

        public DataTable sel_BAS_PASS(PsmhDb pDbCon, string argJob, string argProgid, string argSabun)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                         \r\n";
            SQL += "  IDnumber || '.' || Name AS Names, IDnumber,Name                               \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_PASS                                          \r\n";            
            SQL += "  WHERE 1 = 1                                                                   \r\n";
            if (argJob =="00")
            {
                if (argProgid !="")
                {
                    SQL += "   AND ProgramID = '" + argProgid + "'                                  \r\n";
                }
                SQL += "   AND IDnumber > 0                                                         \r\n";
                SQL += "   AND IDnumber NOT IN (                                                    \r\n";
                SQL += "                         SELECT TRIM(CODE)                                  \r\n";
                SQL += "                          FROM " + ComNum.DB_PMPA + "BAS_BCODE              \r\n";
                SQL += "                           WHERE 1=1                                        \r\n";
                SQL += "                             AND  GUBUN ='C#_Xray_판독사번제외'             \r\n";
                SQL += "                             AND  (DelDate IS NULL OR DelDate ='')          \r\n";
                SQL += "                       )                                                    \r\n";
            }
            else if (argJob == "01")
            {
                if (argProgid != "")
                {
                    SQL += "   AND ProgramID = '" + argProgid + "'                                  \r\n";
                }
                if (argSabun !="")
                {
                    SQL += "   AND IDnumber = " + Convert.ToInt32(argSabun) + "                     \r\n";
                }
            }
            #region 2018-07-07 안정수, 심초음파 판독일 경우, 심장내과 과장만 들어가도록 추가
            else if (argJob == "02")
            {
                if (argProgid != "")
                {
                    SQL += "   AND ProgramID = '" + argProgid + "'                                  \r\n";
                }
                SQL += "   AND IDnumber > 0                                                         \r\n";
                SQL += "   AND IDnumber IN ('31544', '31553', '41460', '37388')                     \r\n";
            }
            #endregion

            if (argJob =="00" || argJob == "02")
            {
                SQL += "  ORDER BY IDnumber                                         \r\n";
            }
            
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
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

        public DataTable sel_HIC_XRAY_RESULT2(PsmhDb pDbCon, clsComSupXrayRead.cHic_Xray_Result argC)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                             \r\n";
            SQL += "  RESULT4, RESULT4_1, PASTCT, PASTCANCER, CTDOSE,                                                                   \r\n";
            SQL += "  NUDOYN_1, NUDOICON_1, NUDOSITE_1, NUDOSIZE1_1, NUDOSIZE2_1, NUDOPOSITIVE_1, NUDOTRACECHK_1, NUDOTRACECHK2_1,      \r\n";
            SQL += "  NUDOYN_2, NUDOICON_2, NUDOSITE_2, NUDOSIZE1_2, NUDOSIZE2_2, NUDOPOSITIVE_2, NUDOTRACECHK_2, NUDOTRACECHK2_2,      \r\n";
            SQL += "  NUDOYN_3, NUDOICON_3, NUDOSITE_3, NUDOSIZE1_3, NUDOSIZE2_3, NUDOPOSITIVE_3, NUDOTRACECHK_3, NUDOTRACECHK2_3,      \r\n";
            SQL += "  NUDOYN_4, NUDOICON_4, NUDOSITE_4, NUDOSIZE1_4, NUDOSIZE2_4, NUDOPOSITIVE_4, NUDOTRACECHK_4, NUDOTRACECHK2_4,      \r\n";
            SQL += "  NUDOYN_5, NUDOICON_5, NUDOSITE_5, NUDOSIZE1_5, NUDOSIZE2_5, NUDOPOSITIVE_5, NUDOTRACECHK_5, NUDOTRACECHK2_5,      \r\n";
            SQL += "  NUDOYN_6, NUDOICON_6, NUDOSITE_6, NUDOSIZE1_6, NUDOSIZE2_6, NUDOPOSITIVE_6, NUDOTRACECHK_6, NUDOTRACECHK2_6,      \r\n";
            SQL += "  INDICATIOCHK, INDICATIOETC, SISACHK, SISAETC, NUDOMEAN1, NUDOMEAN2, NUDOMEAN2_1, NUDOMEAN3, NUDOMEAN4,            \r\n";
            SQL += "  NUDOMEAN5, NUDOMEAN6, NUDOMEAN7, NUDOMEAN8, NUDOMEAN9, NUDOMEAN9_9, NUDOUNACTIVE, NUDOPANGU, NUDOPANGU2,          \r\n";
            SQL += "  SIZE1_1, SIZE1_2, GOSIZE1_1, GOSIZE1_2, IMAGENO_1, CATEGORY1,                                                     \r\n";
            SQL += "  SIZE2_1, SIZE2_2, GOSIZE2_1, GOSIZE2_2, IMAGENO_2, CATEGORY2,                                                     \r\n";
            SQL += "  SIZE3_1, SIZE3_2, GOSIZE3_1, GOSIZE3_2, IMAGENO_3, CATEGORY3,                                                     \r\n";
            SQL += "  SIZE4_1, SIZE4_2, GOSIZE4_1, GOSIZE4_2, IMAGENO_4, CATEGORY4,                                                     \r\n";
            SQL += "  SIZE5_1, SIZE5_2, GOSIZE5_1, GOSIZE5_2, IMAGENO_5, CATEGORY5,                                                     \r\n";
            SQL += "  SIZE6_1, SIZE6_2, GOSIZE6_1, GOSIZE6_2, IMAGENO_6, CATEGORY6,                                                     \r\n";
            SQL += "  NUDOUNACTCHKETC, NUDOUNACTETCSOGEN, NUDOMAXRESULT                                                                 \r\n";

            SQL += "   FROM " + ComNum.DB_PMPA + "HIC_XRAY_RESULT                                                                       \r\n";
            SQL += "  WHERE 1 = 1                                                                                                       \r\n";
            SQL += "   AND ROWID = '" + argC.ROWID + "'                                                                                 \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
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

        public DataTable sel_HIC_CHK_DATA(PsmhDb pDbCon, clsComSupXrayRead.cHic_Xray_Result argC)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                             \r\n";
            SQL += "  SUBSTR(NEW_SICK76, 1, 1) AS PAN, NEW_WOMAN37                                                                      \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "HIC_JEPSU A, " + ComNum.DB_PMPA + "HIC_CANCER_NEW B                                   \r\n";
            SQL += "  WHERE 1 = 1                                                                                                       \r\n";
            SQL += "   AND A.WRTNO = B.WRTNO                                                                                            \r\n";
            SQL += "   AND A.JEPDATE = TO_DATE('" + argC.JEPDATE + "', 'YYYY-MM-DD')                                                    \r\n";
            SQL += "   AND A.PTNO = '" + argC.PTNO + "'                                                                                 \r\n";
            SQL += "   AND A.GJJONG = '31'                                                                                              \r\n";
            SQL += "   AND A.DELDATE IS NULL                                                                                            \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
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



        #endregion

        #region 트랜잭션 쿼리 INSERT, UPDATE,DELETE ....   

        public string ins_Xray_ResultNew(PsmhDb pDbCon, cXray_ResultNew argCls, ref int intRowAffected) 
        {
            string SqlErr = string.Empty;

            SQL = "";
            if (argCls.Job =="00")
            {
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_RESULTNEW                   \r\n";
                SQL += "  (WRTNO,Pano,ReadDate,ReadTime,SeekDate,XJong,SName,Sex,Age        \r\n";
                SQL += "  ,IpdOpd,DeptCode,DrCode,WardCode,RoomCode                         \r\n";
                SQL += "  ,XDrCode1,XDrCode2,XDrCode3,IllCode1,IllCode2,IllCode3            \r\n";
                SQL += "  ,XCode,XName,Result,Result1,EntDate,Approve                       \r\n";
                SQL += "  ,STime,ETime,GbSPC,PanHic, INPS,INPT_DT )  VALUES                 \r\n";
                SQL += "   (                                                                \r\n";
                SQL += "  " + argCls.WRTNO + "                                              \r\n";
                SQL += "  ,'" + argCls.Pano + "'                                            \r\n";
                SQL += "  ,TO_DATE('" + argCls.READDATE + "','YYYY-MM-DD')                  \r\n";
                SQL += "  ,TO_DATE('" + argCls.READTIME + "','YYYY-MM-DD HH24:MI')          \r\n";//체크
                SQL += "  ,TO_DATE('" + argCls.SEEKDATE + "','YYYY-MM-DD')                  \r\n";
                SQL += "  ,'" + argCls.XJong + "'                                           \r\n";
                SQL += "  ,'" + argCls.SName + "'                                           \r\n";
                SQL += "  ,'" + argCls.SEX + "'                                             \r\n";
                SQL += "  ,'" + argCls.AGE + "'                                             \r\n";
                SQL += "  ,'" + argCls.IPDOPD + "'                                          \r\n"; 
                SQL += "  ,'" + argCls.DEPTCODE + "'                                        \r\n";
                SQL += "  ,'" + argCls.DRCODE + "'                                          \r\n";
                SQL += "  ,'" + argCls.WARDCODE + "'                                        \r\n";
                SQL += "  ,'" + argCls.ROOMCODE + "'                                        \r\n";
                SQL += "  ," + argCls.XDrCode1 + "                                          \r\n";
                SQL += "  ," + argCls.XDrCode2 + "                                          \r\n";
                SQL += "  ," + argCls.XDrCode3 + "                                          \r\n";
                SQL += "  ,'" + argCls.ILLCODE1 + "'                                        \r\n";
                SQL += "  ,'" + argCls.ILLCODE2 + "'                                        \r\n";
                SQL += "  ,'" + argCls.ILLCODE3 + "'                                        \r\n";
                SQL += "  ,'" + argCls.XCODE + "'                                           \r\n";
                SQL += "  ,'" + argCls.XNAME + "'                                           \r\n";
                SQL += "  ,'" + argCls.RESULT + "'                                          \r\n";
                SQL += "  ,'" + argCls.RESULT1 + "'                                         \r\n";
                SQL += "  ,SYSDATE                                                          \r\n";
                SQL += "  ,'" + argCls.APPROVE + "'                                         \r\n"; //'Y'
                SQL += "  ,TO_DATE('" + argCls.STIME + "'   ,'YYYY-MM-DD HH24:MI')          \r\n";//체크
                SQL += "  ,SYSDATE                                                          \r\n";
                SQL += "  ,'" + argCls.GbSPC + "'                                           \r\n"; // 'N'
                SQL += "  ,'" + argCls.PANHIC + "'                                          \r\n";
                SQL += "  ,'" + argCls.INPS + "'                                            \r\n";
                SQL += "  ,SYSDATE                                                          \r\n";
                SQL += "   )                                                                \r\n";
            }
            else if (argCls.Job == "01")
            {
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_RESULTNEW                   \r\n";
                SQL += "  (WRTNO,Pano,ReadDate,ReadTime,SeekDate,XJong,SName,Sex,Age        \r\n";
                SQL += "  ,IpdOpd,DeptCode,DrCode,WardCode,RoomCode                         \r\n";
                SQL += "  ,XDrCode1,XDrCode2,XDrCode3,IllCode1,IllCode2,IllCode3            \r\n";
                SQL += "  ,XCode,XName,Result,Result1,ResultEC,ResultEC1                    \r\n";
                SQL += "  ,Part,Part2,EntDate,Approve,Temp                                  \r\n";
                SQL += "  ,STime,ETime,GbSPC,PanHic, INPS,INPT_DT )  VALUES                 \r\n";
                SQL += "   (                                                                \r\n";
                SQL += "  " + argCls.WRTNO + "                                              \r\n";
                SQL += "  ,'" + argCls.Pano + "'                                            \r\n";
                SQL += "  ,TO_DATE('" + argCls.READDATE + "','YYYY-MM-DD')                  \r\n";
                SQL += "  ,TO_DATE('" + argCls.READTIME + "','YYYY-MM-DD HH24:MI')          \r\n";//체크
                SQL += "  ,TO_DATE('" + argCls.SEEKDATE + "','YYYY-MM-DD')                  \r\n";
                SQL += "  ,'" + argCls.XJong + "'                                           \r\n";
                SQL += "  ,'" + argCls.SName + "'                                           \r\n";
                SQL += "  ,'" + argCls.SEX + "'                                             \r\n";
                SQL += "  ,'" + argCls.AGE + "'                                             \r\n";
                SQL += "  ,'" + argCls.IPDOPD + "'                                          \r\n";
                SQL += "  ,'" + argCls.DEPTCODE + "'                                        \r\n";
                SQL += "  ,'" + argCls.DRCODE + "'                                          \r\n";
                SQL += "  ,'" + argCls.WARDCODE + "'                                        \r\n";
                SQL += "  ,'" + argCls.ROOMCODE + "'                                        \r\n";
                SQL += "  ," + argCls.XDrCode1 + "                                          \r\n";
                SQL += "  ," + argCls.XDrCode2 + "                                          \r\n";
                SQL += "  ," + argCls.XDrCode3 + "                                          \r\n";
                SQL += "  ,'" + argCls.ILLCODE1 + "'                                        \r\n";
                SQL += "  ,'" + argCls.ILLCODE2 + "'                                        \r\n";
                SQL += "  ,'" + argCls.ILLCODE3 + "'                                        \r\n";
                SQL += "  ,'" + argCls.XCODE + "'                                           \r\n";
                SQL += "  ,'" + argCls.XNAME + "'                                           \r\n";
                SQL += "  ,'" + argCls.RESULT + "'                                          \r\n";
                SQL += "  ,'" + argCls.RESULT1 + "'                                         \r\n";
                SQL += "  ,'" + argCls.RESULTEC + "'                                        \r\n";
                SQL += "  ,'" + argCls.RESULTEC1 + "'                                       \r\n";
                SQL += "  ," + argCls.PART + "                                              \r\n";
                SQL += "  ," + argCls.PART2 + "                                             \r\n";
                SQL += "  ,SYSDATE                                                          \r\n";
                SQL += "  ,'" + argCls.APPROVE + "'                                         \r\n";
                SQL += "  ,'" + argCls.TEMP + "'                                            \r\n";
                SQL += "  ,TO_DATE('" + argCls.STIME + "'   ,'YYYY-MM-DD HH24:MI')          \r\n";
                SQL += "  ,SYSDATE                                                          \r\n";
                SQL += "  ,'" + argCls.GbSPC + "'                                           \r\n"; // 'N'
                SQL += "  ,'" + argCls.PANHIC + "'                                          \r\n";
                SQL += "  ,'" + argCls.INPS + "'                                            \r\n";
                SQL += "  ,SYSDATE                                                          \r\n";
                SQL += "   )                                                                \r\n";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_Xray_ResultNew(PsmhDb pDbCon, cXray_ResultNew argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            
            if (argCls.Job =="00")
            {
                SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_RESULTNEW  SET                               \r\n";
                SQL += "    XCode = '" + argCls.XCODE + "'                                              \r\n";
                SQL += "   ,XName = '" + argCls.XNAME + "'                                              \r\n";
                SQL += "   ,XDrCode2 = " + argCls.XDrCode2 + "                                          \r\n";
                SQL += "   ,XDrCode3 = " + argCls.XDrCode3 + "                                          \r\n";
                SQL += "   ,ReadDate = TO_DATE('" + argCls.READDATE + "','YYYY-MM-DD')                  \r\n";
                SQL += "   ,ReadTime = TO_DATE('" + argCls.READDATE + "','YYYY-MM-DD HH24:MI')          \r\n"; 
                SQL += "   ,Result = '" + argCls.RESULT + "'                                            \r\n";
                SQL += "   ,Result1 = '" + argCls.RESULT1 + "'                                          \r\n";
                SQL += "   ,PanHic = '" + argCls.PANHIC + "'                                            \r\n";
                SQL += "   ,Approve = '" + argCls.APPROVE + "'                                          \r\n"; 
                SQL += "   ,IllCode1 = '" + argCls.ILLCODE1 + "'                                        \r\n";
                SQL += "   ,IllCode2 = '" + argCls.ILLCODE2 + "'                                        \r\n";
                SQL += "   ,IllCode3 = '" + argCls.ILLCODE3 + "'                                        \r\n";
                SQL += "   ,STIME = TO_DATE('" + argCls.STIME + "','YYYY-MM-DD HH24:MI')                \r\n"; 
                SQL += "   ,ETIME = SYSDATE                                                             \r\n";
                SQL += "   ,EntDate = SYSDATE                                                           \r\n";
                SQL += "   ,SENDEMR = NULL                                                              \r\n";
                SQL += "   ,UPPS = '" + argCls.UPPS + "'                                                \r\n";
                SQL += "   ,UP_DT = SYSDATE                                                             \r\n";
                SQL += "  WHERE 1=1                                                                     \r\n";
                SQL += "    AND WRTNO = " + argCls.WRTNO + "                                            \r\n";
            }
            else if (argCls.Job == "01")
            {
                SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_RESULTNEW  SET                               \r\n";
                SQL += "    XCode = '" + argCls.XCODE + "'                                              \r\n";
                SQL += "   ,XName = '" + argCls.XNAME + "'                                              \r\n";
                SQL += "   ,ReadDate = TO_DATE('" + argCls.READDATE + "','YYYY-MM-DD')                  \r\n";
                SQL += "   ,ReadTime = TO_DATE('" + argCls.READDATE + "','YYYY-MM-DD HH24:MI')          \r\n";
                SQL += "   ,Result = '" + argCls.RESULT + "'                                            \r\n";
                SQL += "   ,Result1 = '" + argCls.RESULT1 + "'                                          \r\n";
                SQL += "   ,ResultEC = '" + argCls.RESULTEC + "'                                        \r\n";
                SQL += "   ,ResultEC1 = '" + argCls.RESULTEC1 + "'                                      \r\n";
                SQL += "   ,Part = " + argCls.PART + "                                                  \r\n";
                SQL += "   ,Part2 = " + argCls.PART2 + "                                                \r\n";
                SQL += "   ,STIME = TO_DATE('" + argCls.STIME + "','YYYY-MM-DD HH24:MI')                \r\n";
                SQL += "   ,ETIME = SYSDATE                                                             \r\n";
                SQL += "   ,EntDate = SYSDATE                                                           \r\n";
                SQL += "   ,SENDEMR = NULL                                                              \r\n";
                //2018-09-14 안정수, TEMP값 추가                
                SQL += "   ,TEMP = '" + argCls.TEMP + "'                                                \r\n";                
                SQL += "   ,UPPS = '" + argCls.UPPS + "'                                                \r\n";
                SQL += "   ,UP_DT = SYSDATE                                                             \r\n";
                SQL += "  WHERE 1=1                                                                     \r\n";
                SQL += "    AND WRTNO = " + argCls.WRTNO + "                                            \r\n";
            }   
            else if (argCls.Job == "02")
            {
                SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_RESULTNEW  SET                               \r\n";
                SQL += "    PANHIC = '" + argCls.PANHIC + "'                                            \r\n";
                SQL += "  WHERE 1=1                                                                     \r\n";
                SQL += "    AND WRTNO = " + argCls.WRTNO + "                                            \r\n";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// XRAY_RESULTNEW 특정컬럼 갱신
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string up_XRAY_RESULTNEW(PsmhDb pDbCon, string argROWID, string argPtno, string argUpCols, string argWhere, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            if (argPtno == "" && argROWID == "")
            {
                return "자료갱신 오류!!";
            }

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_RESULTNEW  SET       \r\n";

            SQL += "    " + argUpCols + "                                   \r\n";

            SQL += "  WHERE 1=1                                             \r\n";
            if (argROWID != "")
            {
                SQL += "    AND ROWID = '" + argROWID + "'                  \r\n";
            }
            if (argPtno != "")
            {
                SQL += "    AND Pano = '" + argPtno + "'                    \r\n";
            }
            if (argWhere != "")
            {
                SQL += "      " + argWhere + "                              \r\n";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string del_XRAY_RESULTNEW(PsmhDb pDbCon, string argROWID, long argWRTNO,  string argWhere, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            if (argWRTNO == 0 && argROWID == "")
            {
                return "삭제 오류!!";
            }

            SQL = "";
            SQL += " DELETE FROM " + ComNum.DB_PMPA + "XRAY_RESULTNEW       \r\n";            
            SQL += "  WHERE 1=1                                             \r\n";
            if (argROWID != "")
            {
                SQL += "    AND ROWID = '" + argROWID + "'                  \r\n";
            }
            if (argWRTNO > 0)
            {
                SQL += "    AND WRTNO = " + argWRTNO + "                    \r\n";
            }
            if (argWhere != "")
            {
                SQL += "      " + argWhere + "                              \r\n";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public class cHic_Xray_Result
        {
            public string Job = "";
            public string GubunA = ""; //판독/미판독  
            public long Row = 0;             
            public string Date1 = "";
            public string Date2 = "";

            public string JEPDATE = "";
            public long WRTNO = 0;
            public string XRAYNO = "";
            public long HPANO = 0;
            public string SNAME = "";
            public string SEX = "";
            public int AGE = 0;
            public string GJJONG = "";
            public string JongName = "";
            public string GBCHUL = "";
            public string LTDCODE = "";
            public string XCODE = "";
            public string XName = "";
            public string GBREAD = "";
            public string GBSTS = "";
            public string RESULT1 = "";
            public string RESULT2 = "";
            public string RESULT3 = "";
            public string RESULT4 = "";
            public string READTIME1 = "";
            public long READDOCT1 = 0;
            public string READTIME2 = "";
            public long READDOCT2 = 0;
            public string GBPRINT = "";
            public string GBRESULTSEND = "";
            public string DELDATE = "";
            public long ENTSABUN = 0;
            public string ENTTIME = "";
            public string READDATE = "";
            public string RESULT1_1 = "";
            public string RESULT2_1 = "";
            public string RESULT3_1 = "";
            public string RESULT4_1 = "";
            public string GBORDER_SEND = "";
            public string GBPACS = "";
            public string PTNO = "";
            public string GBCONV = "";
            public string XRAYNO2 = "";
            public string LTDCODE2 = "";

            public string INPS = "";
            public string INPT_DT = "";
            public string UPPS = "";
            public string UP_DT = "";

            public string ROWID = "";
            public string EXID = "";
            public string JUMIN = "";



            //2019-08-01 안정수 추가

            public string PASTCT 	      = "";     //이전CT 유무
            public string PASTCTYYYY      = "";     //이전CT 촬영일자 년
            public string PASTCTMM        = "";     //이전CT 촬영일자 월

            public string NUDOYN_1        = "";     //양성결절 유무, 1.무 2.유 3.석회화 또는 지방포함결절
            public string NUDOICON_1 	  = "";     //결절성상 1.고형 2.부분고형 3.간유리(비고형)
            public string NUDOSITE_1 	  = "";     //결절위치 1.우상엽 2.우중엽 3.우하엽 4.좌상엽 5.좌하엽
            public string NUDOSIZE1_1 	  = "";     //결절크기 평균
            public string NUDOSIZE2_1 	  = "";     //고형크기평균
            public string NUDOPOSITIVE_1  = "";     //1.폐암 시사소견, 2.양성결절 시사소견(2b), 3.해당없음
            public string NUDOTRACECHK_1  = "";     //추척검사  1.변화없음 2.변화있음 3.해당없음
            public string NUDOTRACECHK2_1 = "";     //추적검사  1.새로생김 2.커짐

            public string NUDOYN_2        = "";
            public string NUDOICON_2      = "";
            public string NUDOSITE_2      = "";
            public string NUDOSIZE1_2     = "";
            public string NUDOSIZE2_2     = "";
            public string NUDOPOSITIVE_2  = "";
            public string NUDOTRACECHK_2  = "";
            public string NUDOTRACECHK2_2 = "";

            public string NUDOYN_3        = "";
            public string NUDOICON_3      = "";
            public string NUDOSITE_3      = "";
            public string NUDOSIZE1_3     = "";
            public string NUDOSIZE2_3     = "";
            public string NUDOPOSITIVE_3  = "";
            public string NUDOTRACECHK_3  = "";
            public string NUDOTRACECHK2_3 = "";

            public string NUDOYN_4        = "";
            public string NUDOICON_4      = "";
            public string NUDOSITE_4      = "";
            public string NUDOSIZE1_4     = "";
            public string NUDOSIZE2_4     = "";
            public string NUDOPOSITIVE_4  = "";
            public string NUDOTRACECHK_4  = "";
            public string NUDOTRACECHK2_4 = "";

            public string NUDOYN_5        = "";
            public string NUDOICON_5      = "";
            public string NUDOSITE_5      = "";
            public string NUDOSIZE1_5     = "";
            public string NUDOSIZE2_5     = "";
            public string NUDOPOSITIVE_5  = "";
            public string NUDOTRACECHK_5  = "";
            public string NUDOTRACECHK2_5 = "";

            public string NUDOYN_6        = "";
            public string NUDOICON_6      = "";
            public string NUDOSITE_6      = "";
            public string NUDOSIZE1_6     = "";
            public string NUDOSIZE2_6     = "";
            public string NUDOPOSITIVE_6  = "";
            public string NUDOTRACECHK_6  = "";
            public string NUDOTRACECHK2_6 = "";

            public string INDICATIOCHK    = "";   //기관지내 병변 1.있음 2.없음
            public string INDICATIOETC    = "";   //소견

            public string SISACHK         = "";   //폐임시사 소견   1.해당없음 2.폐경화 3.무기폐 4.림프절비대 5.기타
            public string SISAETC         = "";   //기타소견, 입력하지 않을 경우 '성적발생'

            public string NUDOMEAN1 = "";
            public string NUDOMEAN2 = "";
            public string NUDOMEAN2_1 = "";
            public string NUDOMEAN3	  = "";
            public string NUDOMEAN4	  = "";
            public string NUDOMEAN5	  = "";
            public string NUDOMEAN6	  = "";
            public string NUDOMEAN7	  = "";
            public string NUDOMEAN8	  = "";
            public string NUDOMEAN9	  = "";
            public string NUDOMEAN9_9 = "";

            public string NUDOUNACTIVE = "";
            public string NUDOPANGU  = "";
            public string NUDOPANGU2 = "";

            public string SIZE1_1 = "";             //사이즈1
            public string SIZE1_2      = "";        //사이즈2
            public string GOSIZE1_1    = "";        //고형사이즈1
            public string GOSIZE1_2    = "";        //고형사이즈2
            public string IMAGENO_1    = "";        //영상넘버            
            public string CATEGORY1    = "";        //범주

            public string SIZE2_1      = "";
            public string SIZE2_2      = "";
            public string GOSIZE2_1    = "";
            public string GOSIZE2_2    = "";
            public string IMAGENO_2    = "";            
            public string CATEGORY2    = "";

            public string SIZE3_1      = "";
            public string SIZE3_2      = "";
            public string GOSIZE3_1    = "";
            public string GOSIZE3_2    = "";
            public string IMAGENO_3    = "";            
            public string CATEGORY3    = "";

            public string SIZE4_1      = "";
            public string SIZE4_2      = "";
            public string GOSIZE4_1    = "";
            public string GOSIZE4_2    = "";
            public string IMAGENO_4    = "";            
            public string CATEGORY4    = "";

            public string SIZE5_1      = "";
            public string SIZE5_2      = "";
            public string GOSIZE5_1    = "";
            public string GOSIZE5_2    = "";
            public string IMAGENO_5    = "";            
            public string CATEGORY5    = "";

            public string SIZE6_1      = "";
            public string SIZE6_2      = "";
            public string GOSIZE6_1    = "";
            public string GOSIZE6_2    = "";
            public string IMAGENO_6    = "";            
            public string CATEGORY6    = "";

            public string strResult1 = "";  //입력결과값
            public string strResult2 = "";  //입력결과값

            public string sResult1 = "";    //DB상 데이터
            public string sResult2 = "";    //DB상 데이터

            public string NUDOUNACTCHKETC = "";     //기타소견 체크 0:없음 1:있음
            public string NUDOUNACTETCSOGEN = "";

            public string PASTCANCER = "";  //과거폐암병력여부

            public string NUDO4X_1 = "";
            public string NUDO4XETC_1 = "";
            public string NUDO4X_2 = "";
            public string NUDO4XETC_2 = "";
            public string NUDO4X_3 = "";
            public string NUDO4XETC_3 = "";
            public string NUDO4X_4 = "";
            public string NUDO4XETC_4 = "";
            public string NUDO4X_5 = "";
            public string NUDO4XETC_5 = "";
            public string NUDO4X_6 = "";
            public string NUDO4XETC_6 = "";

            public string NUDOMAXRESULT = "";   //최대결절값

            public double NODUCTDOSE = 0;
            public string PANCHK = "";      //판정여부 체크

            //2020-01-10 안정수, 추가
            public string DeptCode = "";
            public string DeptName = "";
            public string DRName = "";
            public string PANDRName = "";
            public string PANDRCode = "";
            public string DRCode = "";
            public string SeekDate = "";
            public string RoomCode = "";
            public string WardCode = "";
        }                

        /// <summary>
        /// HIC_XRAY_RESULT 특정컬럼 갱신
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string up_HIC_XRAY_RESULT(PsmhDb pDbCon, string argROWID, string argPtno, string argUpCols, string argWhere, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            if (argPtno == "" && argROWID == "")
            {
                return "자료갱신 오류!!";
            }

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "HIC_XRAY_RESULT  SET      \r\n";

            SQL += "    " + argUpCols + "                                   \r\n";

            SQL += "  WHERE 1=1                                             \r\n";
            if (argROWID != "")
            {
                SQL += "    AND ROWID = '" + argROWID + "'                  \r\n";
            }
            if (argPtno != "")
            {
                SQL += "    AND Ptno = '" + argPtno + "'                    \r\n";
            }
            if (argWhere != "")
            {
                SQL += "      " + argWhere + "                              \r\n";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_HIC_XRAY_RESULT(PsmhDb pDbCon, cHic_Xray_Result argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            if (argCls.ROWID =="")
            {
                return "작업오류!!";
            }

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "HIC_XRAY_RESULT  SET          \r\n";

            SQL += "    GbSTS = '" + argCls.GBSTS + "'                          \r\n";

            SQL += "   ,Result1 = '" + argCls.RESULT1 + "'                      \r\n";
            SQL += "   ,Result2 = '" + argCls.RESULT2 + "'                      \r\n";
            SQL += "   ,Result3 = '" + argCls.RESULT3 + "'                      \r\n";
            SQL += "   ,Result4 = '" + argCls.RESULT4 + "'                      \r\n";

            if (argCls.Job =="삭제")
            {
                SQL += "   ,ReadDoct1 = ''                                      \r\n";
                SQL += "   ,ReadDate = ''                                       \r\n";
                SQL += "   ,ReadTime1 = ''                                      \r\n";
            }
            else
            {
                SQL += "   ,ReadDoct1 = '" + argCls.ENTSABUN + "'               \r\n";
                SQL += "   ,ReadDate = TRUNC(SYSDATE)                           \r\n";
                SQL += "   ,ReadTime1 = SYSDATE                                 \r\n";
            }            

            SQL += "   ,EntSabun = '" + argCls.ENTSABUN + "'                    \r\n";
            SQL += "   ,EntTime = SYSDATE                                       \r\n";
            SQL += "   ,UPPS = '" + argCls.UPPS + "'                            \r\n";
            SQL += "   ,UP_DT = SYSDATE                                         \r\n";
            SQL += "  WHERE 1=1                                                 \r\n";
            SQL += "    AND ROWID = '" + argCls.ROWID + "'                      \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        //public string ins_HIC_XRAY_RESULT(PsmhDb pDbCon, cHic_Xray_Result argCls, ref int intRowAffected)
        //{
        //    string SqlErr = string.Empty;

        //    SQL = "";
        //    SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_RESULTSET           \r\n";
        //    SQL += "  (Sabun,XJong,SetName,XName,Result1,Result2)  VALUES       \r\n";
        //    SQL += "   (                                                        \r\n";
        //    SQL += "  " + argCls.Sabun + "                                      \r\n";
        //    SQL += "  ,'" + argCls.XJong + "'                                   \r\n";
        //    SQL += "  ,'" + argCls.SetName + "'                                 \r\n";
        //    SQL += "  ,'" + argCls.XName + "'                                   \r\n";
        //    SQL += "  ,'" + argCls.Result1 + "'                                 \r\n";
        //    SQL += "  ,'" + argCls.Result2 + "'                                 \r\n";
        //    SQL += "   )                                                        \r\n";

        //    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

        //    return SqlErr;
        //}

        public class cXray_ResultSet
        {
            public string Job = "";
            public long Sabun = 0;
            public string Part = "";
            public string XJong = "";
            public string SetName = "";
            public string XName = "";
            public string Result1 = "";
            public string Result2 = "";
            public string ROWID = "";
        }

        /// <summary>
        /// 영상의학과 기초코드 관련
        /// </summary>
        /// <param name="argCode"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string del_Xray_ResultSet(PsmhDb pDbCon, cXray_ResultSet argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            if (argCls.Part == "HIC")
            {
                SQL += " DELETE FROM " + ComNum.DB_PMPA + "XRAY_ResultSet_Hic       \r\n";
            }
            else
            {
                SQL += " DELETE FROM " + ComNum.DB_PMPA + "XRAY_RESULTSET           \r\n";
            }
            
            SQL += "  WHERE 1=1                                                     \r\n";
            SQL += "    AND ROWID = '" + argCls.ROWID + "'                          \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_Xray_ResultSet(PsmhDb pDbCon, cXray_ResultSet argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            if (argCls.Part == "HIC")
            {
                SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_ResultSet_Hic  SET   \r\n";
            }
            else
            {
                SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_RESULTSET  SET       \r\n";
            }            
            SQL += "    XJong = '" + argCls.XJong + "'                          \r\n";
            SQL += "   ,SetName = '" + argCls.SetName + "'                      \r\n";
            SQL += "   ,XName = '" + argCls.XName + "'                          \r\n";
            SQL += "   ,Result1 = '" + argCls.Result1 + "'                      \r\n";
            SQL += "   ,Result2 = '" + argCls.Result2 + "'                      \r\n";
            SQL += "  WHERE 1=1                                                 \r\n";
            SQL += "    AND ROWID = '" + argCls.ROWID + "'                      \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_Xray_AllPanJeong(PsmhDb pDbCon, string argROWID, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "HIC_XRAY_RESULT  SET          \r\n";
            SQL += "    GBSTS = '2'                                               \r\n";
            SQL += "    ,RESULT1 = '01'                                            \r\n";
            SQL += "    ,RESULT2 = 'A'                                             \r\n";
            SQL += "    ,RESULT3 = '(정상)'                                        \r\n";
            SQL += "    ,READDOCT1 = '" + clsType.User.Sabun +   "'              \r\n";
            SQL += "    ,READDATE = TRUNC(SYSDATE)                               \r\n";
            SQL += "    ,READTIME1 = SYSDATE                                     \r\n";
            SQL += "    ,ENTSABUN = '" + clsType.User.Sabun + "'                 \r\n";
            SQL += "    ,ENTTIME = SYSDATE                                       \r\n";
            SQL += "    ,UPPS = '" + clsType.User.Sabun + "'                     \r\n";
            SQL += "    ,UP_DT = SYSDATE                                         \r\n";
            SQL += "  WHERE 1=1                                                 \r\n";
            SQL += "    AND ROWID = '" + argROWID + "'                      \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_Xray_ResultSet(PsmhDb pDbCon, cXray_ResultSet argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            if (argCls.Part == "HIC")
            {
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_ResultSet_Hic   \r\n";
            }
            else
            {
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "XRAY_RESULTSET       \r\n";
            }            
            SQL += "  (Sabun,XJong,SetName,XName,Result1,Result2)  VALUES       \r\n";
            SQL += "   (                                                        \r\n";
            SQL += "  " + argCls.Sabun + "                                      \r\n";            
            SQL += "  ,'" + argCls.XJong + "'                                   \r\n";                            
            SQL += "  ,'" + argCls.SetName + "'                                 \r\n";
            SQL += "  ,'" + argCls.XName + "'                                   \r\n";
            SQL += "  ,'" + argCls.Result1 + "'                                 \r\n";
            SQL += "  ,'" + argCls.Result2 + "'                                 \r\n";
            SQL += "   )                                                        \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
                

        #endregion

    }
}
