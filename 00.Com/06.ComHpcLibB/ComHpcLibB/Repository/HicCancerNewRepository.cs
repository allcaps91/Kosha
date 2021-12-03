namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicCancerNewRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicCancerNewRepository()
        {
        }

        public HIC_CANCER_NEW GetItemByWrtno(long argWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT WRTNO, STOMACH_S, STOMACH_B, STOMACH_P, STOMACH_PETC, STOMACH_SENDO                            ");
            parameter.AppendSql("      , STOMACH_BENDO, STOMACH_PENDO, STOMACH_ENDOETC, S_ENDOGBN, S_ANATGBN, S_ANAT, S_ANATETC         ");
            parameter.AppendSql("      , S_PANJENG, S_MONTH, S_JILETC, S_PLACE, COLON_RESULT, COLONGBN, COLON_S, COLON_B, COLON_P       ");
            parameter.AppendSql("      , COLON_ENDOGBN, COLON_SENDO, COLON_BENDO, COLON_PENDO, COLON_ENDOETC, C_ENDOGBN                 ");
            parameter.AppendSql("      , C_ANATGBN, C_ANAT, C_ANATETC, C_PANJENG, C_MONTH, C_JILETC, C_PLACE, LIVER_S, LIVER_B          ");
            parameter.AppendSql("      , LIVER_P, LIVER_SIZE, LIVER_LSTYLE, LIVER_VIOLATE, LIVER_DISEASSE, LIVER_ETC, LIVER_PANJENG     ");
            parameter.AppendSql("      , LIVER_JILETC, LIVER_PLACE, BREAST_S, BREAST_P, BREAST_ETC, B_ANATGBN, B_ANAT, B_ANATETC        ");
            parameter.AppendSql("      , B_PANJENG, B_MONTH, B_JILETC, B_PLACE, HEIGHT, WEIGHT, GBSTOMACH, GBLIVER, GBRECTUM            ");
            parameter.AppendSql("      , GBBREAST, SICK11, SICK12, SICK21, SICK22, SICK31, SICK32, SICK41, SICK42, SICK51, SICK52       ");
            parameter.AppendSql("      , SICK61, SICK62, SICK71, SICK72, SICK81, SICK82, JUNGSANG01, JUNGSANG02, JUNGSANG03             ");
            parameter.AppendSql("      , JUNGSANG04, JUNGSANG05, JUNGSANG06, JUNGSANG07, JUNGSANG08, JUNGSANG09, JUNGSANG10             ");
            parameter.AppendSql("      , JUNGSANG11, JUNGSANG12, JUNGSANG13, JUNGSANG14, JUNGSANG15, GAJOK1, GAJOKETC, DRINK1           ");
            parameter.AppendSql("      , DRINK2, SMOKING1, SMOKING2, WOMAN1, WOMAN2, WOMAN3, WOMAN4, WOMAN5, WOMAN6, WOMAN7, WOMAN8     ");
            parameter.AppendSql("      , WOMAN9, WOMAN10, TONGBOGBN, TONGBODATE, PANJENGDRNO, SOGEN, GUNDATE, JINCHALGBN, DRNAME        ");
            parameter.AppendSql("      , LIVER_RPHA, LIVER_EIA, GBPRINT, COLON_PETC, WOMB01, WOMB02, WOMB03, WOMB04, WOMB05             ");
            parameter.AppendSql("      , WOMB06, WOMB07, WOMB08, WOMB09, WOMB10, WOMB11, GBWOMB, WOMB_PLACE, WOMAN11, WOMAN12           ");
            parameter.AppendSql("      , WOMAN13, SICK91, SICK92, LIVER_NEW_ALT, LIVER_NEW_B, LIVER_NEW_BRESULT, LIVER_NEW_C            ");
            parameter.AppendSql("      , LIVER_NEW_CRESULT, NEW_SICK01, NEW_SICK02, NEW_SICK03, NEW_SICK04, NEW_SICK06                  ");
            parameter.AppendSql("      , NEW_SICK07, NEW_SICK08, NEW_SICK09, NEW_SICK11, NEW_SICK12, NEW_SICK13, NEW_SICK14             ");
            parameter.AppendSql("      , NEW_SICK16, NEW_SICK17, NEW_SICK18, NEW_SICK19, NEW_SICK20, NEW_SICK21, NEW_SICK22             ");
            parameter.AppendSql("      , NEW_SICK23, NEW_SICK24, NEW_SICK25, NEW_SICK26, NEW_SICK27, NEW_SICK28, NEW_SICK29             ");
            parameter.AppendSql("      , NEW_SICK30, NEW_SICK31, NEW_SICK32, NEW_SICK33, NEW_SICK34, NEW_SICK36, NEW_SICK37             ");
            parameter.AppendSql("      , NEW_SICK38, NEW_SICK39, NEW_SICK41, NEW_SICK42, NEW_SICK43, NEW_SICK44, NEW_SICK46             ");
            parameter.AppendSql("      , NEW_SICK47, NEW_SICK48, NEW_SICK49, NEW_SICK51, NEW_SICK52, NEW_SICK53, NEW_SICK54             ");
            parameter.AppendSql("      , NEW_SICK56, NEW_SICK57, NEW_SICK58, NEW_SICK59, NEW_SICK61, NEW_SICK62, NEW_SICK63             ");
            parameter.AppendSql("      , NEW_SICK64, NEW_SICK66, NEW_SICK67, NEW_SICK68, NEW_SICK69, NEW_SICK71, NEW_SICK72             ");
            parameter.AppendSql("      , NEW_SICK73, NEW_SICK74, NEW_B_SICK01, NEW_B_SICK02, NEW_B_SICK03, NEW_B_SICK04                 ");
            parameter.AppendSql("      , NEW_B_SICK05, NEW_B_SICK06, NEW_N_SICK01, NEW_N_SICK02, NEW_N_SICK03, NEW_S_SICK01             ");
            parameter.AppendSql("      , NEW_S_SICK02, NEW_S_SICK03, NEW_S_SICK04, NEW_CAN_01, NEW_CAN_02, NEW_CAN_03                   ");
            parameter.AppendSql("      , NEW_CAN_04, NEW_CAN_06, NEW_CAN_07, NEW_CAN_08, NEW_CAN_09, NEW_CAN_11, NEW_CAN_12             ");
            parameter.AppendSql("      , NEW_CAN_13, NEW_CAN_14, NEW_CAN_16, NEW_CAN_17, NEW_CAN_18, NEW_CAN_19, NEW_CAN_21             ");
            parameter.AppendSql("      , NEW_CAN_22, NEW_CAN_23, NEW_CAN_24, NEW_CAN_26, NEW_CAN_27, NEW_CAN_28, NEW_CAN_29             ");
            parameter.AppendSql("      , NEW_HARD, NEW_MARRIED, NEW_SCHOOL, NEW_WORK01, NEW_WORK02, NEW_SMOKE01, NEW_SMOKE02            ");
            parameter.AppendSql("      , NEW_SMOKE03, NEW_SMOKE04, NEW_SMOKE05, NEW_DRINK01, NEW_DRINK02, NEW_DRINK03                   ");
            parameter.AppendSql("      , NEW_DRINK04, NEW_DRINK05, NEW_DRINK06, NEW_DRINK07, NEW_DRINK08, NEW_DRINK09                   ");
            parameter.AppendSql("      , NEW_WOMAN01, NEW_WOMAN02, NEW_WOMAN03, NEW_WOMAN11, NEW_WOMAN12, NEW_WOMAN13                   ");
            parameter.AppendSql("      , NEW_WOMAN14, NEW_WOMAN15, NEW_WOMAN16, NEW_WOMAN17, NEW_WOMAN18, NEW_WOMAN19                   ");
            parameter.AppendSql("      , NEW_WOMAN20, NEW_WOMAN21, NEW_WOMAN22, NEW_WOMAN23, NEW_WOMAN24, NEW_WOMAN25                   ");
            parameter.AppendSql("      , NEW_WOMAN26, NEW_WOMAN27, NEW_WOMAN31, NEW_WOMAN32, NEW_WOMAN33, NEW_WOMAN34                   ");
            parameter.AppendSql("      , NEW_WOMAN35, NEW_WOMAN36, NEW_WOMAN41, NEW_WOMAN42, NEW_WOMAN43, NEW_CAN_WOMAN01               ");
            parameter.AppendSql("      , NEW_CAN_WOMAN02, NEW_CAN_WOMAN03, NEW_CAN_WOMAN04, NEW_CAN_WOMAN06, NEW_CAN_WOMAN07            ");
            parameter.AppendSql("      , NEW_CAN_WOMAN08, NEW_CAN_WOMAN09, NEW_CAN_WOMAN11, NEW_CAN_WOMAN12, NEW_CAN_WOMAN13            ");
            parameter.AppendSql("      , NEW_CAN_WOMAN14, NEW_CAN_WOMAN16, NEW_CAN_WOMAN17, NEW_CAN_WOMAN18, NEW_CAN_WOMAN19            ");
            parameter.AppendSql("      , CAN_MIRGBN, S_SOGEN, C_SOGEN, L_SOGEN, B_SOGEN, W_SOGEN, S_PANJENGDATE, C_PANJENGDATE          ");       
            parameter.AppendSql("      , L_PANJENGDATE, B_PANJENGDATE, W_PANJENGDATE, S_SOGEN2, C_SOGEN2, C_SOGEN3, SANGDAM             ");
            parameter.AppendSql("      , PRTSABUN, BREAST_ETC2, BREAST_ETC3, B_ANATETC3, B_ANATETC2, PANJENGYN, WOMB12, RESULT0001      ");
            parameter.AppendSql("      , RESULT0002, RESULT0003, RESULT0004, RESULT0005, RESULT0006, RESULT0007, RESULT0008             ");
            parameter.AppendSql("      , RESULT0009, RESULT0010, RESULT0011, RESULT0012, RESULT0013, RESULT0014, RESULT0015             ");
            parameter.AppendSql("      , RESULT0016, PANJENGDRNO1, PANJENGDRNO2, PANJENGDRNO3, PANJENGDRNO4, PANJENGDRNO5               ");
            parameter.AppendSql("      , PANJENGDRNO6, PANJENGDRNO7, PANJENGDRNO8, PANJENGDRNO9, PANJENGDRNO10, PANJENGDRNO11           ");
            parameter.AppendSql("      , NEW_SICK75, NEW_SICK76, NEW_SICK77, NEW_SICK78, GBLUNG, L_PANJENGDATE1, LUNG_RESULT001         ");
            parameter.AppendSql("      , LUNG_RESULT002, LUNG_RESULT003, LUNG_RESULT004, LUNG_RESULT005, LUNG_RESULT006                 ");
            parameter.AppendSql("      , LUNG_RESULT007, LUNG_RESULT008, LUNG_RESULT009, LUNG_RESULT010, LUNG_RESULT011                 ");
            parameter.AppendSql("      , LUNG_RESULT012, LUNG_RESULT013, LUNG_RESULT014, LUNG_RESULT015, LUNG_RESULT016                 ");
            parameter.AppendSql("      , LUNG_RESULT017, LUNG_RESULT018, LUNG_RESULT019, LUNG_RESULT020, LUNG_RESULT021                 ");
            parameter.AppendSql("      , LUNG_RESULT022, LUNG_RESULT023, LUNG_RESULT024, LUNG_RESULT025, LUNG_RESULT026                 ");
            parameter.AppendSql("      , LUNG_RESULT027, LUNG_RESULT028, LUNG_RESULT029, LUNG_RESULT030, LUNG_RESULT031                 ");
            parameter.AppendSql("      , LUNG_RESULT032, LUNG_RESULT033, LUNG_RESULT034, LUNG_RESULT035, LUNG_RESULT036                 ");
            parameter.AppendSql("      , LUNG_RESULT037, LUNG_RESULT038, LUNG_RESULT039, LUNG_RESULT040, LUNG_RESULT041                 ");
            parameter.AppendSql("      , LUNG_RESULT042, LUNG_RESULT043, LUNG_RESULT044, LUNG_RESULT045, LUNG_RESULT046                 ");
            parameter.AppendSql("      , LUNG_RESULT047, LUNG_RESULT048, LUNG_RESULT049, LUNG_RESULT050, LUNG_RESULT051                 ");
            parameter.AppendSql("      , LUNG_RESULT052, LUNG_RESULT053, LUNG_RESULT054, LUNG_RESULT055, LUNG_RESULT056                 ");
            parameter.AppendSql("      , LUNG_RESULT057, LUNG_RESULT058, LUNG_RESULT059, LUNG_RESULT060, LUNG_RESULT061                 ");
            parameter.AppendSql("      , LUNG_RESULT062, LUNG_RESULT063, LUNG_RESULT064, LUNG_RESULT065, LUNG_RESULT066                 ");
            parameter.AppendSql("      , LUNG_RESULT067, LUNG_RESULT068, LUNG_RESULT069, LUNG_RESULT070, LUNG_RESULT071                 ");
            parameter.AppendSql("      , LUNG_RESULT072, LUNG_RESULT073, LUNG_RESULT074, LUNG_RESULT075, LUNG_RESULT076                 ");
            parameter.AppendSql("      , LUNG_RESULT077, LUNG_RESULT078, LUNG_PLACE, NEW_WOMAN37, LUNG_RESULT079, LUNG_RESULT080        ");
            parameter.AppendSql("      , LUNG_SANGDAM1, LUNG_SANGDAM2, LUNG_SANGDAM3, LUNG_SANGDAM4                                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_NEW                                                                      ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                                                                 ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReaderSingle<HIC_CANCER_NEW>(parameter);
        }

        public HIC_CANCER_NEW GetItembyWrtNo(long wRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT GBSTOMACH,GBLIVER,GBRECTUM,GBBREAST,GbWomb ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_NEW                  ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                             ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteReaderSingle<HIC_CANCER_NEW>(parameter);
        }

        public HIC_CANCER_NEW GetGunDatebyWrtNo(long wRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT TO_CHAR(GunDate,'YYYY-MM-DD') GUNDATE      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_NEW                  ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                             ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteReaderSingle<HIC_CANCER_NEW>(parameter);
        }

        public int UpdateCancerPanjengbyRowId(HIC_CANCER_NEW item, COMHPC item2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_CANCER_NEW SET                              ");
            parameter.AppendSql("       S_PLACE = :S_PLACE                                          ");
            parameter.AppendSql("     , C_PLACE = :C_PLACE                                          ");
            parameter.AppendSql("     , LIVER_PLACE = :LIVER_PLACE                                  ");
            parameter.AppendSql("     , B_PLACE = :B_PLACE                                          ");
            parameter.AppendSql("     , WOMB_PLACE = :WOMB_PLACE                                    ");
            parameter.AppendSql("     , LUNG_PLACE = :LUNG_PLACE                                    ");
            parameter.AppendSql("     , GBSTOMACH = :GBSTOMACH                                      ");
            parameter.AppendSql("     , GBLIVER = :GBLIVER                                          ");
            parameter.AppendSql("     , GBRECTUM = :GBRECTUM                                        ");
            parameter.AppendSql("     , GBBREAST = :GBBREAST                                        ");
            parameter.AppendSql("     , GBWOMB = :GBWOMB                                            ");
            parameter.AppendSql("     , GBLUNG = :GBLUNG                                            ");
            parameter.AppendSql("     , NEW_WOMAN03 = :NEW_WOMAN03                                  ");
            parameter.AppendSql("     , TONGBOGBN = :TONGBOGBN                                      ");
            parameter.AppendSql("     , CAN_MIRGBN = :CAN_MIRGBN                                    ");
            parameter.AppendSql("     , GUNDATE = TO_DATE(:GUNDATE, 'YYYY-MM-DD')                   ");

            //판정일자 / 판정의사면허번호
            if (item2.CANCERKIND1 == "Y" && item2.PANJENGDATE1 == "Y")
            {
                parameter.AppendSql("     , S_PANJENGDATE = TO_DATE(:S_PANJENGDATE, 'YYYY-MM-DD')   ");
                parameter.AppendSql("     , NEW_WOMAN32 = :NEW_WOMAN32                              ");
            }
            if (item2.CANCERKIND2 == "Y" && item2.PANJENGDATE2 == "Y")
            {
                parameter.AppendSql("     , C_PANJENGDATE = TO_DATE(:C_PANJENGDATE, 'YYYY-MM-DD')   ");
                parameter.AppendSql("     , NEW_WOMAN33 = :NEW_WOMAN33                              ");
            }
            if (item2.CANCERKIND3 == "Y" && item2.PANJENGDATE3 == "Y")
            {
                parameter.AppendSql("     , L_PANJENGDATE = TO_DATE(:L_PANJENGDATE, 'YYYY-MM-DD')   ");
                parameter.AppendSql("     , NEW_WOMAN34 = :NEW_WOMAN34                              ");

            }
            if (item2.CANCERKIND4 == "Y" && item2.PANJENGDATE4 == "Y")
            {
                parameter.AppendSql("     , B_PANJENGDATE = TO_DATE(:B_PANJENGDATE, 'YYYY-MM-DD')   ");
                parameter.AppendSql("     , NEW_WOMAN35 = :NEW_WOMAN35                              ");
            }
            if (item2.CANCERKIND5 == "Y" && item2.PANJENGDATE5 == "Y")
            {
                parameter.AppendSql("     , W_PANJENGDATE = TO_DATE(:W_PANJENGDATE, 'YYYY-MM-DD')   ");
                parameter.AppendSql("     , NEW_WOMAN36 = :NEW_WOMAN36                              ");
            }
            if (item2.CANCERKIND6 == "Y" && item2.PANJENGDATE6 == "Y")
            {
                parameter.AppendSql("     , L_PANJENGDATE1 = TO_DATE(:L_PANJENGDATE1, 'YYYY-MM-DD') ");
                parameter.AppendSql("     , NEW_WOMAN37 = :NEW_WOMAN37                              ");
            }

            if (item2.GBCANCER1 == "Y") //위암판정
            {
                if (item2.STOMACH1 == "Y")
                {
                    parameter.AppendSql("     , STOMACH_S = :STOMACH_S                              ");
                    parameter.AppendSql("     , STOMACH_B = :STOMACH_B                              ");
                    parameter.AppendSql("     , STOMACH_P = :STOMACH_P                              ");
                    parameter.AppendSql("     , STOMACH_PETC = :STOMACH_PETC                        ");
                    parameter.AppendSql("     , S_ENDOGBN = :S_ENDOGBN                              ");
                }
                if (item2.STOMACH2 == "Y")
                {
                    parameter.AppendSql("     , STOMACH_SENDO = :STOMACH_SENDO                      ");
                    parameter.AppendSql("     , STOMACH_BENDO = :STOMACH_BENDO                      ");
                    parameter.AppendSql("     , STOMACH_PENDO = :STOMACH_PENDO                      ");
                    parameter.AppendSql("     , STOMACH_ENDOETC = :STOMACH_ENDOETC                  ");
                    parameter.AppendSql("     , S_ANATGBN = :S_ANATGBN                              ");
                    parameter.AppendSql("     , RESULT0001 = :RESULT0001                            "); //위조직검사
                    parameter.AppendSql("     , RESULT0016 = :RESULT0016                            "); //이물제거술
                    parameter.AppendSql("     , NEW_SICK54 = :NEW_SICK54                            ");
                    parameter.AppendSql("     , NEW_SICK63 = :NEW_SICK63                            ");
                    parameter.AppendSql("     , NEW_SICK66 = :NEW_SICK66                            ");
                    parameter.AppendSql("     , NEW_SICK67 = :NEW_SICK67                            ");
                    parameter.AppendSql("     , NEW_SICK68 = :NEW_SICK68                            ");
                }
                parameter.AppendSql("     , S_PANJENG = :S_PANJENG                                  ");
                parameter.AppendSql("     , S_JILETC = :S_JILETC                                    ");
                parameter.AppendSql("     , PANJENGDRNO1 = :PANJENGDRNO1                            ");
                parameter.AppendSql("     , PANJENGDRNO2 = :PANJENGDRNO2                            ");
                parameter.AppendSql("     , PANJENGDRNO3 = :PANJENGDRNO3                            ");
            }

            if (item2.GBCANCER2 == "Y") //대장암판정
            {
                if (item2.COLON1 == "Y")
                {
                    parameter.AppendSql("     , COLON_RESULT = :COLON_RESULT                        ");
                }
                else if (item2.COLON2 == "Y")
                {
                    parameter.AppendSql("     , COLON_S = :COLON_S                                  ");
                    parameter.AppendSql("     , COLON_B = :COLON_B                                  ");
                    parameter.AppendSql("     , COLON_P = :COLON_P                                  ");
                    parameter.AppendSql("     , COLON_PETC = :COLON_PETC                            ");
                    parameter.AppendSql("     , NEW_SICK33 = :NEW_SICK33                            ");
                    parameter.AppendSql("     , RESULT0002 = :RESULT0002                            ");
                    parameter.AppendSql("     , RESULT0003 = :RESULT0003                            ");
                }
                else if (item2.COLON3 == "Y")
                {
                    parameter.AppendSql("     , COLON_SENDO = :COLON_SENDO                          ");
                    parameter.AppendSql("     , COLON_BENDO = :COLON_BENDO                          ");
                    parameter.AppendSql("     , COLON_PENDO = :COLON_PENDO                          ");
                    parameter.AppendSql("     , COLON_ENDOETC = :COLON_ENDOETC                      ");
                    parameter.AppendSql("     , C_ANATGBN = :C_ANATGBN                              ");
                    parameter.AppendSql("     , NEW_SICK71 = :NEW_SICK71                            ");
                    parameter.AppendSql("     , NEW_SICK69 = :NEW_SICK69                            ");
                    parameter.AppendSql("     , NEW_SICK72 = :NEW_SICK72                            ");
                    parameter.AppendSql("     , NEW_SICK73 = :NEW_SICK73                            ");
                    parameter.AppendSql("     , NEW_SICK74 = :NEW_SICK74                            ");
                    parameter.AppendSql("     , NEW_SICK34 = :NEW_SICK34                            ");
                    parameter.AppendSql("     , NEW_SICK38 = :NEW_SICK38                            ");
                    parameter.AppendSql("     , NEW_SICK59 = :NEW_SICK59                            ");
                    parameter.AppendSql("     , PANJENGDRNO4 = :PANJENGDRNO4                        ");
                    parameter.AppendSql("     , PANJENGDRNO5 = :PANJENGDRNO5                        ");
                    parameter.AppendSql("     , PANJENGDRNO6 = :PANJENGDRNO6                        ");
                }
                parameter.AppendSql("     , C_PANJENG = :C_PANJENG                                  ");
                parameter.AppendSql("     , C_JILETC = :C_JILETC                                    ");
            }

            if (item2.GBCANCER3 == "Y") //간암판정
            {
                if (item2.LIVER1 == "Y")
                {
                    parameter.AppendSql("     , RESULT0004 = :RESULT0004                            ");
                    parameter.AppendSql("     , RESULT0005 = :RESULT0005                            ");
                    parameter.AppendSql("     , RESULT0006 = :RESULT0006                            ");
                    parameter.AppendSql("     , RESULT0007 = :RESULT0007                            ");
                    parameter.AppendSql("     , RESULT0008 = :RESULT0008                            ");
                    parameter.AppendSql("     , RESULT0009 = :RESULT0009                            ");
                    parameter.AppendSql("     , RESULT0010 = :RESULT0010                            "); //간초음파소견-기타
                    parameter.AppendSql("     , RESULT0011 = :RESULT0011                            "); //간초음파소견-기타
                    parameter.AppendSql("     , RESULT0012 = :RESULT0012                            "); //간초음파소견-기타
                    parameter.AppendSql("     , RESULT0015 = :RESULT0015                            "); //간낭종
                }
                if (item2.LIVER2 == "Y")
                {
                    parameter.AppendSql("     , LIVER_RPHA = :LIVER_RPHA                            ");
                    parameter.AppendSql("     , LIVER_EIA = :LIVER_EIA                              ");
                }
                if (item2.LIVER3 == "Y")
                {
                    parameter.AppendSql("     , LIVER_NEW_ALT = :LIVER_NEW_ALT                      ");
                    parameter.AppendSql("     , LIVER_NEW_B = :LIVER_NEW_B                          ");
                    parameter.AppendSql("     , LIVER_NEW_BRESULT = :LIVER_NEW_BRESULT              ");
                    parameter.AppendSql("     , LIVER_NEW_C = :LIVER_NEW_C                          ");
                    parameter.AppendSql("     , LIVER_NEW_CRESULT = :LIVER_NEW_CRESULT              ");
                }
                parameter.AppendSql("     , LIVER_PANJENG = :LIVER_PANJENG                          ");
                parameter.AppendSql("     , LIVER_JILETC = :LIVER_JILETC                            ");
                parameter.AppendSql("     , PANJENGDRNO7 = :PANJENGDRNO7                            ");
            }

            if (item2.GBCANCER4 == "Y") //유방암 판정
            {
                parameter.AppendSql("     , RESULT0013 = :RESULT0013                                ");
                parameter.AppendSql("     , B_ANATGBN = :B_ANATGBN                                  ");
                parameter.AppendSql("     , BREAST_S = :BREAST_S                                    ");
                parameter.AppendSql("     , NEW_WOMAN17 = :NEW_WOMAN17                              ");
                parameter.AppendSql("     , BREAST_P = :BREAST_P                                    ");
                parameter.AppendSql("     , BREAST_ETC = :BREAST_ETC                                ");
                parameter.AppendSql("     , B_ANATETC = :B_ANATETC                                  ");
                parameter.AppendSql("     , BREAST_ETC2 = :BREAST_ETC2                              ");
                parameter.AppendSql("     , B_ANATETC2 = :B_ANATETC2                                ");
                parameter.AppendSql("     , BREAST_ETC3 = :BREAST_ETC3                              ");
                parameter.AppendSql("     , B_ANATETC3 = :B_ANATETC3                                ");
                parameter.AppendSql("     , B_PANJENG = :B_PANJENG                                  ");
                parameter.AppendSql("     , B_JILETC = :B_JILETC                                    ");
                parameter.AppendSql("     , PANJENGDRNO8 = :PANJENGDRNO8                            ");
            }

            if (item2.GBCANCER5 == "Y")    //자궁암 판정
            {
                parameter.AppendSql("     , RESULT0014 = :RESULT0014                                "); //자궁-비정상 선상피세표 (1.일반 2.종양성)
                parameter.AppendSql("     , WOMB01 = :WOMB01                                        ");
                parameter.AppendSql("     , WOMB02 = :WOMB02                                        ");
                parameter.AppendSql("     , WOMB03 = :WOMB03                                        ");
                parameter.AppendSql("     , WOMB04 = :WOMB04                                        ");
                parameter.AppendSql("     , WOMB05 = :WOMB05                                        ");
                parameter.AppendSql("     , WOMB06 = :WOMB06                                        ");
                parameter.AppendSql("     , WOMB07 = :WOMB07                                        ");
                parameter.AppendSql("     , WOMB08 = :WOMB08                                        ");
                parameter.AppendSql("     , WOMB09 = :WOMB09                                        ");
                parameter.AppendSql("     , WOMB10 = :WOMB10                                        ");
                parameter.AppendSql("     , WOMB11 = :WOMB11                                        ");     
                parameter.AppendSql("     , WOMAN12 = :WOMAN12                                      ");
                parameter.AppendSql("     , PANJENGDRNO9 = :PANJENGDRNO9                            ");
                parameter.AppendSql("     , PANJENGDRNO10 = :PANJENGDRNO10                          ");
            }

            if (item2.GBCANCER6 == "Y") //폐암 판정
            {
                parameter.AppendSql("     , LUNG_RESULT001 = :LUNG_RESULT001                        ");
                parameter.AppendSql("     , LUNG_RESULT002 = :LUNG_RESULT002                        ");
                parameter.AppendSql("     , LUNG_RESULT003 = :LUNG_RESULT003                        ");
                parameter.AppendSql("     , LUNG_RESULT004 = :LUNG_RESULT004                        ");
                if (item2.LUNG1 == "Y")
                {
                    parameter.AppendSql("     , LUNG_RESULT005 = :LUNG_RESULT005                    ");
                    parameter.AppendSql("     , LUNG_RESULT006 = :LUNG_RESULT006                    ");
                    parameter.AppendSql("     , LUNG_RESULT007 = :LUNG_RESULT007                    ");
                    parameter.AppendSql("     , LUNG_RESULT008 = :LUNG_RESULT008                    ");
                    parameter.AppendSql("     , LUNG_RESULT009 = :LUNG_RESULT009                    ");
                    parameter.AppendSql("     , LUNG_RESULT010 = :LUNG_RESULT010                    ");
                    parameter.AppendSql("     , LUNG_RESULT011 = :LUNG_RESULT011                    ");
                    parameter.AppendSql("     , LUNG_RESULT012 = :LUNG_RESULT012                    ");
                }
                if (item2.LUNG2 == "Y")
                {
                    parameter.AppendSql("     , LUNG_RESULT013 = :LUNG_RESULT013                    ");
                    parameter.AppendSql("     , LUNG_RESULT014 = :LUNG_RESULT014                    ");
                    parameter.AppendSql("     , LUNG_RESULT015 = :LUNG_RESULT015                    ");
                    parameter.AppendSql("     , LUNG_RESULT016 = :LUNG_RESULT016                    ");
                    parameter.AppendSql("     , LUNG_RESULT017 = :LUNG_RESULT017                    ");
                    parameter.AppendSql("     , LUNG_RESULT018 = :LUNG_RESULT018                    ");
                    parameter.AppendSql("     , LUNG_RESULT019 = :LUNG_RESULT019                    ");
                    parameter.AppendSql("     , LUNG_RESULT020 = :LUNG_RESULT020                    ");
                }
                if (item2.LUNG3 == "Y")
                {
                    parameter.AppendSql("     , LUNG_RESULT021 = :LUNG_RESULT021                    ");
                    parameter.AppendSql("     , LUNG_RESULT022 = :LUNG_RESULT022                    ");
                    parameter.AppendSql("     , LUNG_RESULT023 = :LUNG_RESULT023                    ");
                    parameter.AppendSql("     , LUNG_RESULT024 = :LUNG_RESULT024                    ");
                    parameter.AppendSql("     , LUNG_RESULT025 = :LUNG_RESULT025                    ");
                    parameter.AppendSql("     , LUNG_RESULT026 = :LUNG_RESULT026                    ");
                    parameter.AppendSql("     , LUNG_RESULT027 = :LUNG_RESULT027                    ");
                    parameter.AppendSql("     , LUNG_RESULT028 = :LUNG_RESULT028                    ");
                }
                if (item2.LUNG4 == "Y")
                {
                    parameter.AppendSql("     , LUNG_RESULT029 = :LUNG_RESULT029                    ");
                    parameter.AppendSql("     , LUNG_RESULT030 = :LUNG_RESULT030                    ");
                    parameter.AppendSql("     , LUNG_RESULT031 = :LUNG_RESULT031                    ");
                    parameter.AppendSql("     , LUNG_RESULT032 = :LUNG_RESULT032                    ");
                    parameter.AppendSql("     , LUNG_RESULT033 = :LUNG_RESULT033                    ");
                    parameter.AppendSql("     , LUNG_RESULT034 = :LUNG_RESULT034                    ");
                    parameter.AppendSql("     , LUNG_RESULT035 = :LUNG_RESULT035                    ");
                    parameter.AppendSql("     , LUNG_RESULT036 = :LUNG_RESULT036                    ");
                }
                if (item2.LUNG5 == "Y")
                {
                    parameter.AppendSql("     , LUNG_RESULT037 = :LUNG_RESULT037                    ");
                    parameter.AppendSql("     , LUNG_RESULT038 = :LUNG_RESULT038                    ");
                    parameter.AppendSql("     , LUNG_RESULT039 = :LUNG_RESULT039                    ");
                    parameter.AppendSql("     , LUNG_RESULT040 = :LUNG_RESULT040                    ");
                    parameter.AppendSql("     , LUNG_RESULT041 = :LUNG_RESULT041                    ");
                    parameter.AppendSql("     , LUNG_RESULT042 = :LUNG_RESULT042                    ");
                    parameter.AppendSql("     , LUNG_RESULT043 = :LUNG_RESULT043                    ");
                    parameter.AppendSql("     , LUNG_RESULT044 = :LUNG_RESULT044                    ");
                }
                if (item2.LUNG6 == "Y")
                {
                    parameter.AppendSql("     , LUNG_RESULT045 = :LUNG_RESULT045                    ");
                    parameter.AppendSql("     , LUNG_RESULT046 = :LUNG_RESULT046                    ");
                    parameter.AppendSql("     , LUNG_RESULT047 = :LUNG_RESULT047                    ");
                    parameter.AppendSql("     , LUNG_RESULT048 = :LUNG_RESULT048                    ");
                    parameter.AppendSql("     , LUNG_RESULT049 = :LUNG_RESULT049                    ");
                    parameter.AppendSql("     , LUNG_RESULT050 = :LUNG_RESULT050                    ");
                    parameter.AppendSql("     , LUNG_RESULT051 = :LUNG_RESULT051                    ");
                    parameter.AppendSql("     , LUNG_RESULT052 = :LUNG_RESULT052                    ");
                }

                parameter.AppendSql("     , LUNG_RESULT053 = :LUNG_RESULT053                        ");
                parameter.AppendSql("     , LUNG_RESULT054 = :LUNG_RESULT054                        ");
                parameter.AppendSql("     , LUNG_RESULT055 = :LUNG_RESULT055                        ");
                parameter.AppendSql("     , LUNG_RESULT056 = :LUNG_RESULT056                        ");
                parameter.AppendSql("     , LUNG_RESULT057 = :LUNG_RESULT057                        ");
                parameter.AppendSql("     , LUNG_RESULT058 = :LUNG_RESULT058                        ");
                parameter.AppendSql("     , LUNG_RESULT059 = :LUNG_RESULT059                        ");
                parameter.AppendSql("     , LUNG_RESULT060 = :LUNG_RESULT060                        ");
                parameter.AppendSql("     , LUNG_RESULT061 = :LUNG_RESULT061                        ");
                parameter.AppendSql("     , LUNG_RESULT062 = :LUNG_RESULT062                        ");
                parameter.AppendSql("     , LUNG_RESULT063 = :LUNG_RESULT063                        ");
                parameter.AppendSql("     , LUNG_RESULT064 = :LUNG_RESULT064                        ");
                parameter.AppendSql("     , LUNG_RESULT065 = :LUNG_RESULT065                        ");
                parameter.AppendSql("     , LUNG_RESULT066 = :LUNG_RESULT066                        ");
                parameter.AppendSql("     , LUNG_RESULT067 = :LUNG_RESULT067                        ");
                parameter.AppendSql("     , LUNG_RESULT068 = :LUNG_RESULT068                        ");
                parameter.AppendSql("     , LUNG_RESULT069 = :LUNG_RESULT069                        ");
                parameter.AppendSql("     , LUNG_RESULT070 = :LUNG_RESULT070                        ");
                parameter.AppendSql("     , LUNG_RESULT071 = :LUNG_RESULT071                        ");
                parameter.AppendSql("     , LUNG_RESULT072 = :LUNG_RESULT072                        ");
                parameter.AppendSql("     , LUNG_RESULT073 = :LUNG_RESULT073                        ");
                parameter.AppendSql("     , LUNG_RESULT074 = :LUNG_RESULT074                        ");
                parameter.AppendSql("     , LUNG_RESULT075 = :LUNG_RESULT075                        ");
                parameter.AppendSql("     , LUNG_RESULT076 = :LUNG_RESULT076                        ");
                parameter.AppendSql("     , LUNG_RESULT077 = :LUNG_RESULT077                        ");
                parameter.AppendSql("     , LUNG_RESULT078 = :LUNG_RESULT078                        ");
                if (item2.LUNGOK.IsNullOrEmpty())
                {
                    parameter.AppendSql("     , LUNG_RESULT079 = :LUNG_RESULT079                    ");
                }
                parameter.AppendSql("     , LUNG_RESULT080 = :LUNG_RESULT080                        ");
                parameter.AppendSql("     , PANJENGDRNO11 = :PANJENGDRNO11                          ");
            }

            //판정소견
            parameter.AppendSql("     , S_SOGEN = :S_SOGEN                                          ");
            parameter.AppendSql("     , S_SOGEN2 = :S_SOGEN2                                        ");
            parameter.AppendSql("     , C_SOGEN = :C_SOGEN                                          ");
            parameter.AppendSql("     , C_SOGEN2 = :C_SOGEN2                                        ");
            parameter.AppendSql("     , C_SOGEN3 = :C_SOGEN3                                        ");
            parameter.AppendSql("     , L_SOGEN = :L_SOGEN                                          ");
            parameter.AppendSql("     , B_SOGEN = :B_SOGEN                                          ");
            parameter.AppendSql("     , W_SOGEN = :W_SOGEN                                          ");

            

            
            parameter.AppendSql(" WHERE ROWID   = :RID                                              ");

            #region 변수매칭
            parameter.Add("RID", item.RID);

            parameter.Add("S_PLACE", item.S_PLACE);
            parameter.Add("C_PLACE", item.C_PLACE);
            parameter.Add("LIVER_PLACE", item.LIVER_PLACE);
            parameter.Add("B_PLACE", item.B_PLACE);
            parameter.Add("WOMB_PLACE", item.WOMB_PLACE);
            parameter.Add("LUNG_PLACE", item.LUNG_PLACE);
            parameter.Add("GBSTOMACH", item.GBSTOMACH);
            parameter.Add("GBLIVER", item.GBLIVER);
            parameter.Add("GBRECTUM", item.GBRECTUM);
            parameter.Add("GBBREAST", item.GBBREAST);
            parameter.Add("GBWOMB", item.GBWOMB);
            parameter.Add("GBLUNG", item.GBLUNG);
            parameter.Add("NEW_WOMAN03", item.NEW_WOMAN03);
            parameter.Add("TONGBOGBN", item.TONGBOGBN);
            parameter.Add("CAN_MIRGBN", item.CAN_MIRGBN);
            parameter.Add("GUNDATE", item.GUNDATE);

            //판정일자 / 판정의사면허번호
            if (item2.CANCERKIND1 == "Y" && item2.PANJENGDATE1 == "Y")
            {
                parameter.Add("S_PANJENGDATE", item.S_PANJENGDATE);
                parameter.Add("NEW_WOMAN32", item.NEW_WOMAN32);
            }
            if (item2.CANCERKIND2 == "Y" && item2.PANJENGDATE2 == "Y")
            {
                parameter.Add("C_PANJENGDATE", item.C_PANJENGDATE);
                parameter.Add("NEW_WOMAN33", item.NEW_WOMAN33);
            }
            if (item2.CANCERKIND3 == "Y" && item2.PANJENGDATE3 == "Y")
            {
                parameter.Add("L_PANJENGDATE", item.L_PANJENGDATE);
                parameter.Add("NEW_WOMAN34", item.NEW_WOMAN34);

            }
            if (item2.CANCERKIND4 == "Y" && item2.PANJENGDATE4 == "Y")
            {
                parameter.Add("B_PANJENGDATE", item.B_PANJENGDATE);
                parameter.Add("NEW_WOMAN35", item.NEW_WOMAN35);
            }
            if (item2.CANCERKIND5 == "Y" && item2.PANJENGDATE5 == "Y")
            {
                parameter.Add("W_PANJENGDATE", item.W_PANJENGDATE);
                parameter.Add("NEW_WOMAN36", item.NEW_WOMAN36);
            }
            if (item2.CANCERKIND6 == "Y" && item2.PANJENGDATE6 == "Y")
            {
                parameter.Add("L_PANJENGDATE1", item.L_PANJENGDATE1);
                parameter.Add("NEW_WOMAN37", item.NEW_WOMAN37);
            }


            if (item2.GBCANCER1 == "Y") //위암판정
            {
                if (item2.STOMACH1 == "Y")
                {
                    parameter.Add("STOMACH_S", item.STOMACH_S);
                    parameter.Add("STOMACH_B", item.STOMACH_B);
                    parameter.Add("STOMACH_P", item.STOMACH_P);
                    parameter.Add("STOMACH_PETC", item.STOMACH_PETC);
                    parameter.Add("S_ENDOGBN", item.S_ENDOGBN);
                }
                if (item2.STOMACH2 == "Y")
                {
                    parameter.Add("STOMACH_SENDO", item.STOMACH_SENDO);
                    parameter.Add("STOMACH_BENDO", item.STOMACH_BENDO);
                    parameter.Add("STOMACH_PENDO", item.STOMACH_PENDO);
                    parameter.Add("STOMACH_ENDOETC", item.STOMACH_ENDOETC);
                    parameter.Add("S_ANATGBN", item.S_ANATGBN);
                    parameter.Add("RESULT0001", item.RESULT0001); //위조직검사
                    parameter.Add("RESULT0016", item.RESULT0016); //이물제거술
                    parameter.Add("NEW_SICK54", item.NEW_SICK54);
                    parameter.Add("NEW_SICK63", item.NEW_SICK63);
                    parameter.Add("NEW_SICK66", item.NEW_SICK66);
                    parameter.Add("NEW_SICK67", item.NEW_SICK67);
                    parameter.Add("NEW_SICK68", item.NEW_SICK68);
                }
                parameter.Add("S_PANJENG", item.S_PANJENG);
                parameter.Add("S_JILETC", item.S_JILETC);
                parameter.Add("PANJENGDRNO1", item.PANJENGDRNO1);
                parameter.Add("PANJENGDRNO2", item.PANJENGDRNO2);
                parameter.Add("PANJENGDRNO3", item.PANJENGDRNO3);
            }

            if (item2.GBCANCER2 == "Y") //대장암판정
            {
                if (item2.COLON1 == "Y")
                {
                    parameter.Add("COLON_RESULT", item.COLON_RESULT);
                }
                else if (item2.COLON2 == "Y")
                {
                    parameter.Add("COLON_S", item.COLON_S);
                    parameter.Add("COLON_B", item.COLON_B);
                    parameter.Add("COLON_P", item.COLON_P);
                    parameter.Add("COLON_PETC", item.COLON_PETC);
                    parameter.Add("NEW_SICK33", item.NEW_SICK33);
                    parameter.Add("RESULT0002", item.RESULT0002);
                    parameter.Add("RESULT0003", item.RESULT0003);
                }
                else if (item2.COLON3 == "Y")
                {
                    parameter.Add("COLON_SENDO", item.COLON_SENDO);
                    parameter.Add("COLON_BENDO", item.COLON_BENDO);
                    parameter.Add("COLON_PENDO", item.COLON_PENDO);
                    parameter.Add("COLON_ENDOETC", item.COLON_ENDOETC);
                    parameter.Add("C_ANATGBN", item.C_ANATGBN);
                    parameter.Add("NEW_SICK71", item.NEW_SICK71);
                    parameter.Add("NEW_SICK69", item.NEW_SICK69);
                    parameter.Add("NEW_SICK72", item.NEW_SICK72);
                    parameter.Add("NEW_SICK73", item.NEW_SICK73);
                    parameter.Add("NEW_SICK74", item.NEW_SICK74);
                    parameter.Add("NEW_SICK34", item.NEW_SICK34);
                    parameter.Add("NEW_SICK38", item.NEW_SICK38);
                    parameter.Add("NEW_SICK59", item.NEW_SICK59);
                    parameter.Add("PANJENGDRNO4", item.PANJENGDRNO4);
                    parameter.Add("PANJENGDRNO5", item.PANJENGDRNO5);
                    parameter.Add("PANJENGDRNO6", item.PANJENGDRNO6);
                }
                parameter.Add("C_PANJENG", item.C_PANJENG);
                parameter.Add("C_JILETC", item.C_JILETC);
            }

            if (item2.GBCANCER3 == "Y") //간암판정
            {
                if (item2.LIVER1 == "Y")
                {
                    parameter.Add("RESULT0004", item.RESULT0004);
                    parameter.Add("RESULT0005", item.RESULT0005);
                    parameter.Add("RESULT0006", item.RESULT0006);
                    parameter.Add("RESULT0007", item.RESULT0007);
                    parameter.Add("RESULT0008", item.RESULT0008);
                    parameter.Add("RESULT0009", item.RESULT0009);
                    parameter.Add("RESULT0010", item.RESULT0010); //간초음파소견-기타
                    parameter.Add("RESULT0011", item.RESULT0011); //간초음파소견-기타
                    parameter.Add("RESULT0012", item.RESULT0012); //간초음파소견-기타
                    parameter.Add("RESULT0015", item.RESULT0015); //간낭종
                }
                if (item2.LIVER2 == "Y")
                {
                    parameter.Add("LIVER_RPHA", item.LIVER_RPHA);
                    parameter.Add("LIVER_EIA", item.LIVER_EIA);
                }
                if (item2.LIVER3 == "Y")
                {
                    parameter.Add("LIVER_NEW_ALT", item.LIVER_NEW_ALT);
                    parameter.Add("LIVER_NEW_B", item.LIVER_NEW_B);
                    parameter.Add("LIVER_NEW_BRESULT", item.LIVER_NEW_BRESULT);
                    parameter.Add("LIVER_NEW_C", item.LIVER_NEW_C);
                    parameter.Add("LIVER_NEW_CRESULT", item.LIVER_NEW_CRESULT);
                }
                parameter.Add("LIVER_PANJENG", item.LIVER_PANJENG);
                parameter.Add("LIVER_JILETC", item.LIVER_JILETC);
                parameter.Add("PANJENGDRNO7", item.PANJENGDRNO7);
            }

            if (item2.GBCANCER4 == "Y") //유방암 판정
            {
                parameter.Add("RESULT0013", item.RESULT0013);
                parameter.Add("B_ANATGBN", item.B_ANATGBN);
                parameter.Add("BREAST_S", item.BREAST_S);
                parameter.Add("NEW_WOMAN17", item.NEW_WOMAN17);
                parameter.Add("BREAST_P", item.BREAST_P);
                parameter.Add("BREAST_ETC", item.BREAST_ETC);
                parameter.Add("B_ANATETC", item.B_ANATETC);
                parameter.Add("BREAST_ETC2", item.BREAST_ETC2);
                parameter.Add("B_ANATETC2", item.B_ANATETC2);
                parameter.Add("BREAST_ETC3", item.BREAST_ETC3);
                parameter.Add("B_ANATETC3", item.B_ANATETC3);
                parameter.Add("B_PANJENG", item.B_PANJENG);
                parameter.Add("B_JILETC", item.B_JILETC);
                parameter.Add("PANJENGDRNO8", item.PANJENGDRNO8);
            }

            if (item2.GBCANCER5 == "Y")    //자궁암 판정
            {
                parameter.Add("RESULT0014", item.RESULT0014); //자궁-비정상 선상피세표 (1.일반 2.종양성)
                parameter.Add("WOMB01", item.WOMB01);
                parameter.Add("WOMB02", item.WOMB02);
                parameter.Add("WOMB03", item.WOMB03);
                parameter.Add("WOMB04", item.WOMB04);
                parameter.Add("WOMB05", item.WOMB05);
                parameter.Add("WOMB06", item.WOMB06);
                parameter.Add("WOMB07", item.WOMB07);
                parameter.Add("WOMB08", item.WOMB08);
                parameter.Add("WOMB09", item.WOMB09);
                parameter.Add("WOMB10", item.WOMB10);
                parameter.Add("WOMB11", item.WOMB11);
                parameter.Add("WOMAN12", item.WOMAN12);
                parameter.Add("PANJENGDRNO9", item.PANJENGDRNO9);
                parameter.Add("PANJENGDRNO10", item.PANJENGDRNO10);
            }

            if (item2.GBCANCER6 == "Y") //폐암 판정
            {
                parameter.Add("LUNG_RESULT001", item.LUNG_RESULT001);
                parameter.Add("LUNG_RESULT002", item.LUNG_RESULT002);
                parameter.Add("LUNG_RESULT003", item.LUNG_RESULT003);
                parameter.Add("LUNG_RESULT004", item.LUNG_RESULT004);
                if (item2.LUNG1 == "Y")
                {
                    parameter.Add("LUNG_RESULT005", item.LUNG_RESULT005);
                    parameter.Add("LUNG_RESULT006", item.LUNG_RESULT006);
                    parameter.Add("LUNG_RESULT007", item.LUNG_RESULT007);
                    parameter.Add("LUNG_RESULT008", item.LUNG_RESULT008);
                    parameter.Add("LUNG_RESULT009", item.LUNG_RESULT009);
                    parameter.Add("LUNG_RESULT010", item.LUNG_RESULT010);
                    parameter.Add("LUNG_RESULT011", item.LUNG_RESULT011);
                    parameter.Add("LUNG_RESULT012", item.LUNG_RESULT012);
                }
                if (item2.LUNG2 == "Y")
                {
                    parameter.Add("LUNG_RESULT013", item.LUNG_RESULT013);
                    parameter.Add("LUNG_RESULT014", item.LUNG_RESULT014);
                    parameter.Add("LUNG_RESULT015", item.LUNG_RESULT015);
                    parameter.Add("LUNG_RESULT016", item.LUNG_RESULT016);
                    parameter.Add("LUNG_RESULT017", item.LUNG_RESULT017);
                    parameter.Add("LUNG_RESULT018", item.LUNG_RESULT018);
                    parameter.Add("LUNG_RESULT019", item.LUNG_RESULT019);
                    parameter.Add("LUNG_RESULT020", item.LUNG_RESULT020);
                }
                if (item2.LUNG3 == "Y")
                {
                    parameter.Add("LUNG_RESULT021", item.LUNG_RESULT021);
                    parameter.Add("LUNG_RESULT022", item.LUNG_RESULT022);
                    parameter.Add("LUNG_RESULT023", item.LUNG_RESULT023);
                    parameter.Add("LUNG_RESULT024", item.LUNG_RESULT024);
                    parameter.Add("LUNG_RESULT025", item.LUNG_RESULT025);
                    parameter.Add("LUNG_RESULT026", item.LUNG_RESULT026);
                    parameter.Add("LUNG_RESULT027", item.LUNG_RESULT027);
                    parameter.Add("LUNG_RESULT028", item.LUNG_RESULT028);
                }
                if (item2.LUNG4 == "Y")
                {
                    parameter.Add("LUNG_RESULT029", item.LUNG_RESULT029);
                    parameter.Add("LUNG_RESULT030", item.LUNG_RESULT030);
                    parameter.Add("LUNG_RESULT031", item.LUNG_RESULT031);
                    parameter.Add("LUNG_RESULT032", item.LUNG_RESULT032);
                    parameter.Add("LUNG_RESULT033", item.LUNG_RESULT033);
                    parameter.Add("LUNG_RESULT034", item.LUNG_RESULT034);
                    parameter.Add("LUNG_RESULT035", item.LUNG_RESULT035);
                    parameter.Add("LUNG_RESULT036", item.LUNG_RESULT036);
                }
                if (item2.LUNG5 == "Y")
                {
                    parameter.Add("LUNG_RESULT037", item.LUNG_RESULT037);
                    parameter.Add("LUNG_RESULT038", item.LUNG_RESULT038);
                    parameter.Add("LUNG_RESULT039", item.LUNG_RESULT039);
                    parameter.Add("LUNG_RESULT040", item.LUNG_RESULT040);
                    parameter.Add("LUNG_RESULT041", item.LUNG_RESULT041);
                    parameter.Add("LUNG_RESULT042", item.LUNG_RESULT042);
                    parameter.Add("LUNG_RESULT043", item.LUNG_RESULT043);
                    parameter.Add("LUNG_RESULT044", item.LUNG_RESULT044);
                }
                if (item2.LUNG6 == "Y")
                {
                    parameter.Add("LUNG_RESULT045", item.LUNG_RESULT045);
                    parameter.Add("LUNG_RESULT046", item.LUNG_RESULT046);
                    parameter.Add("LUNG_RESULT047", item.LUNG_RESULT047);
                    parameter.Add("LUNG_RESULT048", item.LUNG_RESULT048);
                    parameter.Add("LUNG_RESULT049", item.LUNG_RESULT049);
                    parameter.Add("LUNG_RESULT050", item.LUNG_RESULT050);
                    parameter.Add("LUNG_RESULT051", item.LUNG_RESULT051);
                    parameter.Add("LUNG_RESULT052", item.LUNG_RESULT052);
                }

                parameter.Add("LUNG_RESULT053", item.LUNG_RESULT053);
                parameter.Add("LUNG_RESULT054", item.LUNG_RESULT054);
                parameter.Add("LUNG_RESULT055", item.LUNG_RESULT055);
                parameter.Add("LUNG_RESULT056", item.LUNG_RESULT056);
                parameter.Add("LUNG_RESULT057", item.LUNG_RESULT057);
                parameter.Add("LUNG_RESULT058", item.LUNG_RESULT058);
                parameter.Add("LUNG_RESULT059", item.LUNG_RESULT059);
                parameter.Add("LUNG_RESULT060", item.LUNG_RESULT060);
                parameter.Add("LUNG_RESULT061", item.LUNG_RESULT061);
                parameter.Add("LUNG_RESULT062", item.LUNG_RESULT062);
                parameter.Add("LUNG_RESULT063", item.LUNG_RESULT063);
                parameter.Add("LUNG_RESULT064", item.LUNG_RESULT064);
                parameter.Add("LUNG_RESULT065", item.LUNG_RESULT065);
                parameter.Add("LUNG_RESULT066", item.LUNG_RESULT066);
                parameter.Add("LUNG_RESULT067", item.LUNG_RESULT067);
                parameter.Add("LUNG_RESULT068", item.LUNG_RESULT068);
                parameter.Add("LUNG_RESULT069", item.LUNG_RESULT069);
                parameter.Add("LUNG_RESULT070", item.LUNG_RESULT070);
                parameter.Add("LUNG_RESULT071", item.LUNG_RESULT071);
                parameter.Add("LUNG_RESULT072", item.LUNG_RESULT072);
                parameter.Add("LUNG_RESULT073", item.LUNG_RESULT073);
                parameter.Add("LUNG_RESULT074", item.LUNG_RESULT074);
                parameter.Add("LUNG_RESULT075", item.LUNG_RESULT075);
                parameter.Add("LUNG_RESULT076", item.LUNG_RESULT076);
                parameter.Add("LUNG_RESULT077", item.LUNG_RESULT077);
                parameter.Add("LUNG_RESULT078", item.LUNG_RESULT078);
                if (item2.LUNGOK.IsNullOrEmpty())
                {
                    parameter.Add("LUNG_RESULT079", item.LUNG_RESULT079);
                }
                parameter.Add("LUNG_RESULT080", item.LUNG_RESULT080);
                parameter.Add("PANJENGDRNO11", item.PANJENGDRNO11);
            }
            //판정소견
            parameter.Add("S_SOGEN", item.S_SOGEN);
            parameter.Add("S_SOGEN2", item.S_SOGEN2);
            parameter.Add("C_SOGEN", item.C_SOGEN);
            parameter.Add("C_SOGEN2", item.C_SOGEN2);
            parameter.Add("C_SOGEN3", item.C_SOGEN3);
            parameter.Add("L_SOGEN", item.L_SOGEN);
            parameter.Add("B_SOGEN", item.B_SOGEN);
            parameter.Add("W_SOGEN", item.W_SOGEN);

            
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public int UpdatePanjengYNbyRowId(string strTemp, string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_CANCER_NEW SET                          ");
            parameter.AppendSql("       PANJENGYN = :PANJENGYN                                  ");
            parameter.AppendSql(" WHERE ROWID     = :RID                                        ");

            parameter.Add("PANJENGYN", strTemp);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public string GetGbPrintbyWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT GBPRINT                    ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_CANCER_NEW ");
            parameter.AppendSql("  WHERE WRTNO  = :WRTNO            ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_CANCER_NEW GetRowIdbyWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT ROWID RID, WRTNO           ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_CANCER_NEW ");
            parameter.AppendSql("  WHERE WRTNO  = :WRTNO            ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReaderSingle<HIC_CANCER_NEW>(parameter);
        }

        public void UpdateTongBoInfobyWrtNo(long fnWRTNO, string strDate, string strGbn)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_CANCER_NEW SET                      ");
            if (!strDate.IsNullOrEmpty())
            {
                parameter.AppendSql("       TONGBODATE  = TO_DATE(:TONGBODATE, 'YYYY-MM-DD')");
                parameter.AppendSql("     , TONGBOGBN  = :TONGBOGBN                         ");
            }
            else
            {
                parameter.AppendSql("       TONGBODATE  = ''                                ");
                parameter.AppendSql("     , TONGBOGBN  = ''                                 ");
                parameter.AppendSql("     , GBPRINT = ''                                    ");
                parameter.AppendSql("     , PRTSABUN  = ''                                  ");
            }

            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                     ");

            if (!strDate.IsNullOrEmpty())
            {
                parameter.Add("TONGBODATE", strDate);
                parameter.Add("TONGBOGBN", strGbn);
            }

            parameter.Add("WRTNO", fnWRTNO);

            ExecuteNonQuery(parameter);
        }

        public void InsertSelect(long argWrtno, long nCanWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_CANCER_NEW (                                                               ");
            parameter.AppendSql("       WRTNO,WEIGHT,NEW_SICK19,NEW_SICK20,NEW_SICK01,NEW_SICK03,NEW_SICK06,NEW_SICK08,NEW_SICK11       ");
            parameter.AppendSql("      ,NEW_SICK13,NEW_SICK16,NEW_SICK18,NEW_SICK21,NEW_SICK23,NEW_SICK26,NEW_SICK28,NEW_SICK30         ");
            parameter.AppendSql("      ,NEW_SICK25,NEW_CAN_WOMAN01,NEW_CAN_WOMAN04,NEW_CAN_WOMAN06,NEW_CAN_WOMAN09,NEW_CAN_WOMAN11      ");
            parameter.AppendSql("      ,NEW_CAN_WOMAN14,NEW_CAN_WOMAN16,NEW_CAN_WOMAN19,NEW_SICK61,NEW_SICK62,NEW_WOMAN01,NEW_WOMAN02   ");
            parameter.AppendSql("      ,NEW_WOMAN11,NEW_WOMAN14,NEW_WOMAN18,NEW_WOMAN21,NEW_WOMAN41,NEW_WOMAN27,NEW_WOMAN31 )           ");
            parameter.AppendSql("SELECT :WRTNO,WEIGHT,NEW_SICK19,NEW_SICK20,NEW_SICK01,NEW_SICK03,NEW_SICK06,NEW_SICK08,NEW_SICK11      ");
            parameter.AppendSql("      ,NEW_SICK13,NEW_SICK16,NEW_SICK18,NEW_SICK21,NEW_SICK23,NEW_SICK26,NEW_SICK28,NEW_SICK30         ");
            parameter.AppendSql("      ,NEW_SICK25,NEW_CAN_WOMAN01,NEW_CAN_WOMAN04,NEW_CAN_WOMAN06,NEW_CAN_WOMAN09,NEW_CAN_WOMAN11      ");
            parameter.AppendSql("      ,NEW_CAN_WOMAN14,NEW_CAN_WOMAN16,NEW_CAN_WOMAN19,NEW_SICK61,NEW_SICK62,NEW_WOMAN01,NEW_WOMAN02   ");
            parameter.AppendSql("      ,NEW_WOMAN11,NEW_WOMAN14,NEW_WOMAN18,NEW_WOMAN21,NEW_WOMAN41,NEW_WOMAN27,NEW_WOMAN31             ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HIC_CANCER_NEW                                                                       ");
            parameter.AppendSql("WHERE WRTNO =:NEWWRTNO                                                                                 ");

            parameter.Add("WRTNO", argWrtno);
            parameter.Add("NEWWRTNO", nCanWRTNO);

            ExecuteNonQuery(parameter);
        }

        public int DeletebyWrtNo(long argWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_CANCER_NEW  ");
            parameter.AppendSql(" WHERE WRTNO  =:WRTNO              ");

            parameter.Add("WRTNO", argWrtno);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateLungSangdambyWrtNo(string strResult1, string strResult2, string gstrSysDate, long nDrNo, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_CANCER_NEW SET                          ");
            parameter.AppendSql("       LUNG_SANGDAM1 = :LUNG_SANGDAM1                          ");
            parameter.AppendSql("     , LUNG_SANGDAM2 = :LUNG_SANGDAM2                          ");
            parameter.AppendSql("     , LUNG_SANGDAM3 = TO_DATE(:LUNG_SANGDAM3, 'YYYY-MM-DD')   ");
            parameter.AppendSql("     , LUNG_SANGDAM4 = :LUNG_SANGDAM4                          ");
            parameter.AppendSql("     , GUNDATE       = TO_DATE(:GUNDATE, 'YYYY-MM-DD')         ");
            parameter.AppendSql(" WHERE WRTNO         = :WRTNO                                  ");

            parameter.Add("LUNG_SANGDAM1", strResult1);
            parameter.Add("LUNG_SANGDAM2", strResult2);
            parameter.Add("LUNG_SANGDAM3", gstrSysDate);
            parameter.Add("LUNG_SANGDAM4", nDrNo);
            parameter.Add("GUNDATE", gstrSysDate); 
            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public HIC_CANCER_NEW GetLungbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT LUNG_SANGDAM1, LUNG_SANGDAM2                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_NEW                              ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                         ");
            parameter.AppendSql(" AND (LUNG_SANGDAM1 IS NOT NULL AND LUNG_SANGDAM2 IS NOT NULL) ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReaderSingle<HIC_CANCER_NEW>(parameter);
        }

        public int Update(string strTemp, long nWrtNo, string strJob)
        {
            MParameter parameter = CreateParameter();
                                  
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_CANCER_NEW SET              ");
            switch (strJob)
            {
                case "1":
                    parameter.AppendSql("       S_SOGEN = :TEMP                     ");
                    break;
                case "2":
                    parameter.AppendSql("       S_SOGEN2 = :TEMP                    ");
                    break;
                case "3":
                    parameter.AppendSql("       C_SOGEN = :TEMP                     ");
                    break;
                case "4":
                    parameter.AppendSql("       C_SOGEN2 = :TEMP                    ");
                    break;
                case "5":
                    parameter.AppendSql("       C_SOGEN3 = :TEMP                    ");
                    break;
                case "6":
                    parameter.AppendSql("       L_SOGEN = :TEMP                     ");
                    break;
                case "7":
                    parameter.AppendSql("       B_SOGEN = :TEMP                     ");
                    break;
                case "8":
                    parameter.AppendSql("       W_SOGEN = :TEMP                     ");
                    break;
                default:
                    break;
            }
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");

            parameter.Add("TEMP", strTemp);
            parameter.Add("WRTNO", nWrtNo); 

            return ExecuteNonQuery(parameter);
        }

        public long GetPanjengDrNobyWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT PANJENGDRNO                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_NEW      ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                 ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteScalar<long>(parameter);
        }

        public int GetCountbyWrtNo(long argWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT COUNT('X') CNT                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_NEW      ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                 ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public HIC_CANCER_NEW GetPanjengDateByWrtno(long argWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT                                                ");
            parameter.AppendSql(" TO_CHAR(S_PanjengDate,'YYYY-MM-DD') S_PanjengDate     ");
            parameter.AppendSql(" ,TO_CHAR(C_PanjengDate,'YYYY-MM-DD') C_PanjengDate    ");
            parameter.AppendSql(" ,TO_CHAR(L_PanjengDate,'YYYY-MM-DD') L_PanjengDate    ");
            parameter.AppendSql(" ,TO_CHAR(B_PanjengDate,'YYYY-MM-DD') B_PanjengDate    ");
            parameter.AppendSql(" ,TO_CHAR(W_PanjengDate,'YYYY-MM-DD') W_PanjengDate    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_NEW                      ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                 ");

            parameter.Add("WRTNO", argWrtno);

            return ExecuteReaderSingle<HIC_CANCER_NEW>(parameter);
        } 
    }
}
