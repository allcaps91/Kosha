namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicDoctorRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicDoctorRepository()
        {
        }        
        
        public string Read_License_DrName(long argLicence)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT DrName                  ");
            parameter.AppendSql("  FROM ADMIN.HIC_DOCTOR  ");
            parameter.AppendSql(" WHERE LICENCE = :LICENCE      ");

            parameter.Add("LICENCE", argLicence);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_DOCTOR Read_Hic_DrCode(long argSabun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT DRNAME, LICENCE         ");
            parameter.AppendSql("  FROM ADMIN.HIC_DOCTOR  ");
            parameter.AppendSql(" WHERE SABUN = :SABUN          ");

            parameter.Add("SABUN", argSabun);

            return ExecuteReaderSingle<HIC_DOCTOR>(parameter);
        }

        public List<HIC_DOCTOR> Read_Combo_HisDoctor(string strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT LICENCE, DRNAME, SABUN          ");
            parameter.AppendSql("  FROM ADMIN.HIC_DOCTOR          ");
            if (strGubun == "")
            {
                parameter.AppendSql(" WHERE REDAY IS NULL               ");
            }
            else
            {
                parameter.AppendSql(" WHERE REDAY >=TRUNC(SYSDATE-365)  ");
            }
            
            parameter.AppendSql("   AND PAN = '1'                       ");
            parameter.AppendSql(" ORDER BY DrName                       ");

            return ExecuteReader<HIC_DOCTOR>(parameter);
        }

        public List<HIC_DOCTOR> GetSabunbyRoom(List<string> strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SABUN                   ");
            parameter.AppendSql("  FROM ADMIN.HIC_DOCTOR  ");
            parameter.AppendSql(" WHERE ROOM IN (:ROOM)         ");
            parameter.AppendSql("   AND ReDay IS NULL           ");
            parameter.AppendSql(" ORDER BY DrName               ");

            parameter.AddInStatement("ROOM", strGubun,  Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_DOCTOR>(parameter);
        }

        public HIC_DOCTOR Read_Hic_DrCode3(long argSabun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT DrName,DRBUNHO,DRCODE   ");
            parameter.AppendSql("  FROM ADMIN.OCS_DOCTOR   ");
            parameter.AppendSql(" WHERE DRBUNHO = :DRBUNHO      ");

            parameter.Add("DRBUNHO", argSabun);

            return ExecuteReaderSingle<HIC_DOCTOR>(parameter);
        }

        public int Read_Doctor_Hea(long nDrSabun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT          ");
            parameter.AppendSql("  FROM ADMIN.HIC_DOCTOR  ");
            parameter.AppendSql(" WHERE SABUN = :SABUN          ");
            parameter.AppendSql("   AND REDAY IS NULL           ");
            parameter.AppendSql("   AND GBHEA = 'Y'             ");

            parameter.Add("SABUN", nDrSabun);

            return ExecuteScalar<int>(parameter);
        }

        public string Chk_Hea_Doct(long nDrSabun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GBHEA                   ");
            parameter.AppendSql("  FROM ADMIN.HIC_DOCTOR  ");
            parameter.AppendSql(" WHERE SABUN = :SABUN          ");
            parameter.AppendSql("   AND GBHEA = 'Y'             ");

            parameter.Add("SABUN", nDrSabun);

            return ExecuteScalar<string>(parameter);
        }

        public string GetLicensebySabun(long nDrSabun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT LICENCE                 ");
            parameter.AppendSql("  FROM ADMIN.HIC_DOCTOR  ");
            parameter.AppendSql(" WHERE SABUN = :SABUN          ");

            parameter.Add("SABUN", nDrSabun);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_DOCTOR GetIDrNameLicencebyDrSabun(string strDRSABUN)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT DRNAME, LICENCE         ");
            parameter.AppendSql("  FROM ADMIN.HIC_DOCTOR  ");
            parameter.AppendSql(" WHERE SABUN = :SABUN          ");

            parameter.Add("SABUN", strDRSABUN);

            return ExecuteReaderSingle<HIC_DOCTOR>(parameter);
        }

        public List<HIC_DOCTOR> GetIDrCode()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT DRCODE, ROOM            ");
            parameter.AppendSql("  FROM ADMIN.HIC_DOCTOR  ");
            parameter.AppendSql(" WHERE REDAY IS NULL           ");
            parameter.AppendSql("   AND PAN IS NOT NULL         ");
            parameter.AppendSql("   AND ROOM IS NOT NULL        ");

            return ExecuteReader<HIC_DOCTOR>(parameter);
        }

        public List<HIC_DOCTOR> GetItemAll()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SABUN, DRNAME, IPSADAY, REDAY               ");
            parameter.AppendSql("     , LICENCE, ENTDATE, ENTSABUN                  ");
            parameter.AppendSql("     , SCHE, ROOM, PAN, DRCODE, GBDENT, GBHEA      ");
            parameter.AppendSql("  FROM ADMIN.HIC_DOCTOR                      ");
            parameter.AppendSql(" WHERE ROOM >= '01'                                ");
            parameter.AppendSql("   AND (REDAY IS NULL OR ReDay > TRUNC(SYSDATE))   ");

            return ExecuteReader<HIC_DOCTOR>(parameter);
        }

        public long Read_Hic_DrSabun(string argDrLicence)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SABUN                   ");
            parameter.AppendSql("  FROM ADMIN.HIC_DOCTOR  ");
            parameter.AppendSql(" WHERE LICENCE = :LICENCE      ");
            parameter.AppendSql(" ORDER BY Sabun DESC           ");

            parameter.Add("LICENCE", argDrLicence);

            return ExecuteScalar<long>(parameter);
        }

        public string Read_Hic_OcsDrcode(long argSabun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT DRCODE                  ");
            parameter.AppendSql("  FROM ADMIN.OCS_DOCTOR   ");
            parameter.AppendSql(" WHERE SABUN = :SABUN          ");

            parameter.Add("SABUN", string.Format("0:#00000", argSabun));

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_DOCTOR> GetListbyReday(string strREDAY)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SABUN,DRNAME,LICENCE,GBDENT,DRCODE                      ");
            parameter.AppendSql(" FROM ADMIN.HIC_DOCTOR                                   ");
            parameter.AppendSql(" WHERE 1 = 1                                                   ");
            parameter.AppendSql(" AND (REDAY IS NULL OR REDAY >= TO_DATE(:REDAY,'YYYY-MM-DD'))  ");
            parameter.AppendSql(" AND DRCODE IS NOT NULL                                        ");
            parameter.AppendSql(" ORDER BY DRNAME                                               ");


            parameter.Add("REDAY", strREDAY);

            return ExecuteReader<HIC_DOCTOR>(parameter);
        }
    }
}
