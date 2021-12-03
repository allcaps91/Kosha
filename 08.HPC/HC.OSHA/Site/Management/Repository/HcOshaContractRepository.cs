namespace HC.OSHA.Site.Management.Repository
{
    using ComBase.Mvc;
    using HC.OSHA.Site.Management.Dto;
    using HC.Core.Common.Service;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaContractRepository : BaseRepository
    {
        public HC_OSHA_CONTRACT FindByEstimateId(long estimateId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HC_OSHA_CONTRACT    ");
            parameter.AppendSql("WHERE ESTIMATE_ID = :ESTIMATE_ID");
            parameter.Add("ESTIMATE_ID", estimateId);

            HC_OSHA_CONTRACT dto = ExecuteReaderSingle<HC_OSHA_CONTRACT>(parameter);
            return dto;

        }
        public HC_OSHA_CONTRACT Insert(HC_OSHA_CONTRACT dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HC_OSHA_CONTRACT                                                ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  ESTIMATE_ID,                                                              ");
            parameter.AppendSql("  OSHA_SITE_ID,                                                             ");
            parameter.AppendSql("  CONTRACTDATE,                                                             ");
            parameter.AppendSql("  SPECIALCONTRACT,                                                             ");
            parameter.AppendSql("  WORKERTOTALCOUNT,                                                         ");
            parameter.AppendSql("  WORKERWHITEMALECOUNT,                                                     ");
            parameter.AppendSql("  WORKERWHITEFEMALECOUNT,                                                   ");
            parameter.AppendSql("  WORKERBLUEMALECOUNT,                                                      ");
            parameter.AppendSql("  WORKERBLUEFEMALECOUNT,                                                    ");
            parameter.AppendSql("  MANAGEWORKERCOUNT,                                                        ");
            parameter.AppendSql("  MANAGEDOCTOR,                                                             ");
            parameter.AppendSql("  MANAGEDOCTORSTARTDATE,                                                    ");
            parameter.AppendSql("  MANAGEDOCTORCOUNT,                                                        ");
            parameter.AppendSql("  MANAGENURSE,                                                              ");
            parameter.AppendSql("  MANAGENURSESTARTDATE,                                                     ");
            parameter.AppendSql("  MANAGENURSECOUNT,                                                         ");
            parameter.AppendSql("  MANAGEENGINEER,                                                           ");
            parameter.AppendSql("  MANAGEENGINEERSTARTDATE,                                                  ");
            parameter.AppendSql("  MANAGEENGINEERCOUNT,                                                      ");
            parameter.AppendSql("  VISITWEEK,                                                                ");
            parameter.AppendSql("  VISITDAY,                                                                 ");
            parameter.AppendSql("  COMMISSION,                                                               ");
            parameter.AppendSql("  DECLAREDAY,                                                               ");
            parameter.AppendSql("  CONTRACTSTARTDATE,                                                        ");
            parameter.AppendSql("  CONTRACTENDDATE,                                                          ");
            parameter.AppendSql("  POSITION,                                                                 ");
            parameter.AppendSql("  ISROTATION,                                                               ");
            parameter.AppendSql("  ISPRODUCTTYPE,                                                            ");
            parameter.AppendSql("  ISLABOR,                                                                  ");
            parameter.AppendSql("  BuildingType,                                                                  ");
            
            parameter.AppendSql("  WORKSTARTTIME,                                                            ");
            parameter.AppendSql("  WORKENDTIME,                                                              ");
            parameter.AppendSql("  WORKMEETTIME,                                                             ");
            parameter.AppendSql("  WORKROTATIONTIME,                                                         ");
            parameter.AppendSql("  WORKLUANCHTIME,                                                           ");
            parameter.AppendSql("  WORKRESTTIME,                                                             ");
            parameter.AppendSql("  WORKEDUTIME,                                                              ");
            parameter.AppendSql("  WORKETCTIME,                                                              ");
            parameter.AppendSql("  ISCONTRACT,                                                               ");
            parameter.AppendSql("  ISWEM,                                                                    ");
            parameter.AppendSql("  ISWEMDATA,                                                                ");
            parameter.AppendSql("  ISCOMMITTEE,                                                              ");
            parameter.AppendSql("  ISSKELETON,                                                               ");
            parameter.AppendSql("  ISSKELETONDATE,                                                           ");
            parameter.AppendSql("  ISSPACEPROGRAM,                                                           ");
            parameter.AppendSql("  ISSPACEPROGRAMDATE,                                                       ");
            parameter.AppendSql("  ISEARPROGRAM,                                                             ");
            parameter.AppendSql("  ISEARPROGRAMDATE,                                                         ");
            parameter.AppendSql("  ISSTRESS,                                                                 ");
            parameter.AppendSql("  ISSTRESSDATE,                                                             ");
            parameter.AppendSql("  ISBRAINTEST,                                                              ");
            parameter.AppendSql("  ISBRAINTESTDATE,                                                          ");
            parameter.AppendSql("  ISSPECIAL,                                                                ");
            parameter.AppendSql("  ISSPECIALDATA,                                                            ");
            parameter.AppendSql("  ISDELETED,                                                                ");
            parameter.AppendSql("  MODIFIED,                                                                 ");
            parameter.AppendSql("  MODIFIEDUSER,                                                             ");
            parameter.AppendSql("  CREATED,                                                                  ");
            parameter.AppendSql("  CREATEDUSER                                                               ");

            parameter.AppendSql(")                                                                           ");
            parameter.AppendSql("VALUES                                                                      ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  :ESTIMATE_ID,                                                             ");
            parameter.AppendSql("  :OSHA_SITE_ID,                                                            ");
            parameter.AppendSql("  :CONTRACTDATE,                                                            ");
            parameter.AppendSql("  :SPECIALCONTRACT,                                                            ");
            parameter.AppendSql("  :WORKERTOTALCOUNT,                                                        ");
            parameter.AppendSql("  :WORKERWHITEMALECOUNT,                                                    ");
            parameter.AppendSql("  :WORKERWHITEFEMALECOUNT,                                                  ");
            parameter.AppendSql("  :WORKERBLUEMALECOUNT,                                                     ");
            parameter.AppendSql("  :WORKERBLUEFEMALECOUNT,                                                   ");
            parameter.AppendSql("  :MANAGEWORKERCOUNT,                                                       ");
            parameter.AppendSql("  :MANAGEDOCTOR,                                                            ");
            parameter.AppendSql("  :MANAGEDOCTORSTARTDATE,                                                   ");
            parameter.AppendSql("  :MANAGEDOCTORCOUNT,                                                       ");
            parameter.AppendSql("  :MANAGENURSE,                                                             ");
            parameter.AppendSql("  :MANAGENURSESTARTDATE,                                                    ");
            parameter.AppendSql("  :MANAGENURSECOUNT,                                                        ");
            parameter.AppendSql("  :MANAGEENGINEER,                                                          ");
            parameter.AppendSql("  :MANAGEENGINEERSTARTDATE,                                                 ");
            parameter.AppendSql("  :MANAGEENGINEERCOUNT,                                                     ");
            parameter.AppendSql("  :VISITWEEK,                                                               ");
            parameter.AppendSql("  :VISITDAY,                                                                ");
            parameter.AppendSql("  :COMMISSION,                                                              ");
            parameter.AppendSql("  :DECLAREDAY,                                                              ");
            parameter.AppendSql("  :CONTRACTSTARTDATE,                                                       ");
            parameter.AppendSql("  :CONTRACTENDDATE,                                                         ");
            parameter.AppendSql("  :POSITION,                                                                ");
            parameter.AppendSql("  :ISROTATION,                                                              ");
            parameter.AppendSql("  :ISPRODUCTTYPE,                                                           ");
            parameter.AppendSql("  :ISLABOR,                                                                 ");
            parameter.AppendSql("  :BUILDINGTYPE,                                                             ");
            parameter.AppendSql("  :WORKSTARTTIME,                                                           ");
            parameter.AppendSql("  :WORKENDTIME,                                                             ");
            parameter.AppendSql("  :WORKMEETTIME,                                                            ");
            parameter.AppendSql("  :WORKROTATIONTIME,                                                        ");
            parameter.AppendSql("  :WORKLUANCHTIME,                                                          ");
            parameter.AppendSql("  :WORKRESTTIME,                                                            ");
            parameter.AppendSql("  :WORKEDUTIME,                                                             ");
            parameter.AppendSql("  :WORKETCTIME,                                                             ");
            parameter.AppendSql("  :ISCONTRACT,                                                              ");
            parameter.AppendSql("  :ISWEM,                                                                   ");
            parameter.AppendSql("  :ISWEMDATA,                                                               ");
            parameter.AppendSql("  :ISCOMMITTEE,                                                             ");
            parameter.AppendSql("  :ISSKELETON,                                                              ");
            parameter.AppendSql("  :ISSKELETONDATE,                                                          ");
            parameter.AppendSql("  :ISSPACEPROGRAM,                                                          ");
            parameter.AppendSql("  :ISSPACEPROGRAMDATE,                                                      ");
            parameter.AppendSql("  :ISEARPROGRAM,                                                            ");
            parameter.AppendSql("  :ISEARPROGRAMDATE,                                                        ");
            parameter.AppendSql("  :ISSTRESS,                                                                ");
            parameter.AppendSql("  :ISSTRESSDATE,                                                            ");
            parameter.AppendSql("  :ISBRAINTEST,                                                             ");
            parameter.AppendSql("  :ISBRAINTESTDATE,                                                         ");
            parameter.AppendSql("  :ISSPECIAL,                                                               ");
            parameter.AppendSql("  :ISSPECIALDATA,                                                           ");
            parameter.AppendSql("  'N',                                                                      ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                             ");
            parameter.AppendSql("  :MODIFIEDUSER,                                                            ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                             ");
            parameter.AppendSql("  :CREATEDUSER                                                              ");         
            parameter.AppendSql(")                                                                           ");
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("OSHA_SITE_ID", dto.OSHA_SITE_ID);
            parameter.Add("CONTRACTDATE", dto.CONTRACTDATE);
            parameter.Add("SPECIALCONTRACT", dto.SPECIALCONTRACT);
            
            parameter.Add("WORKERTOTALCOUNT", dto.WORKERTOTALCOUNT);
            parameter.Add("WORKERWHITEMALECOUNT", dto.WORKERWHITEMALECOUNT);
            parameter.Add("WORKERWHITEFEMALECOUNT", dto.WORKERWHITEFEMALECOUNT);
            parameter.Add("WORKERBLUEMALECOUNT", dto.WORKERBLUEMALECOUNT);
            parameter.Add("WORKERBLUEFEMALECOUNT", dto.WORKERBLUEFEMALECOUNT);
            parameter.Add("MANAGEWORKERCOUNT", dto.MANAGEWORKERCOUNT);
            parameter.Add("MANAGEDOCTOR", dto.MANAGEDOCTOR);
            parameter.Add("MANAGEDOCTORSTARTDATE", dto.MANAGEDOCTORSTARTDATE);
            parameter.Add("MANAGEDOCTORCOUNT", dto.MANAGEDOCTORCOUNT);
            parameter.Add("MANAGENURSE", dto.MANAGENURSE);
            parameter.Add("MANAGENURSESTARTDATE", dto.MANAGENURSESTARTDATE);
            parameter.Add("MANAGENURSECOUNT", dto.MANAGENURSECOUNT);
            parameter.Add("MANAGEENGINEER", dto.MANAGEENGINEER);
            parameter.Add("MANAGEENGINEERSTARTDATE", dto.MANAGEENGINEERSTARTDATE);
            parameter.Add("MANAGEENGINEERCOUNT", dto.MANAGEENGINEERCOUNT);
            parameter.Add("VISITWEEK", dto.VISITWEEK);
            parameter.Add("VISITDAY", dto.VISITDAY);
            parameter.Add("COMMISSION", dto.COMMISSION);
            parameter.Add("DECLAREDAY", dto.DECLAREDAY);
            parameter.Add("CONTRACTSTARTDATE", dto.CONTRACTSTARTDATE);
            parameter.Add("CONTRACTENDDATE", dto.CONTRACTENDDATE);
            parameter.Add("POSITION", dto.POSITION);
            parameter.Add("ISROTATION", dto.ISROTATION);
            parameter.Add("ISPRODUCTTYPE", dto.ISPRODUCTTYPE);
            parameter.Add("ISLABOR", dto.ISLABOR);
            parameter.Add("BUILDINGTYPE", dto.BUILDINGTYPE);
            parameter.Add("WORKSTARTTIME", dto.WORKSTARTTIME);
            parameter.Add("WORKENDTIME", dto.WORKENDTIME);
            parameter.Add("WORKMEETTIME", dto.WORKMEETTIME);
            parameter.Add("WORKROTATIONTIME", dto.WORKROTATIONTIME);
            parameter.Add("WORKLUANCHTIME", dto.WORKLUANCHTIME);
            parameter.Add("WORKRESTTIME", dto.WORKRESTTIME);
            parameter.Add("WORKEDUTIME", dto.WORKEDUTIME);
            parameter.Add("WORKETCTIME", dto.WORKETCTIME);
            parameter.Add("ISCONTRACT", dto.ISCONTRACT);
            parameter.Add("ISWEM", dto.ISWEM);
            parameter.Add("ISWEMDATA", dto.ISWEMDATA);
            parameter.Add("ISCOMMITTEE", dto.ISCOMMITTEE);
            parameter.Add("ISSKELETON", dto.ISSKELETON);
            parameter.Add("ISSKELETONDATE", dto.ISSKELETONDATE);
            parameter.Add("ISSPACEPROGRAM", dto.ISSPACEPROGRAM);
            parameter.Add("ISSPACEPROGRAMDATE", dto.ISSPACEPROGRAMDATE);
            parameter.Add("ISEARPROGRAM", dto.ISEARPROGRAM);
            parameter.Add("ISEARPROGRAMDATE", dto.ISEARPROGRAMDATE);
            parameter.Add("ISSTRESS", dto.ISSTRESS);
            parameter.Add("ISSTRESSDATE", dto.ISSTRESSDATE);
            parameter.Add("ISBRAINTEST", dto.ISBRAINTEST);
            parameter.Add("ISBRAINTESTDATE", dto.ISBRAINTESTDATE);
            parameter.Add("ISSPECIAL", dto.ISSPECIAL);
            parameter.Add("ISSPECIALDATA", dto.ISSPECIALDATA);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);


            return FindByEstimateId(dto.ESTIMATE_ID);
        }

        public HC_OSHA_CONTRACT Update(HC_OSHA_CONTRACT dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HC_OSHA_CONTRACT                                                     ");
            parameter.AppendSql("SET                                                                         ");
            parameter.AppendSql("  CONTRACTDATE = :CONTRACTDATE,                                             ");
            parameter.AppendSql("  SPECIALCONTRACT = :SPECIALCONTRACT,                                             ");
            parameter.AppendSql("  WORKERTOTALCOUNT = :WORKERTOTALCOUNT,                                     ");
            parameter.AppendSql("  WORKERWHITEMALECOUNT = :WORKERWHITEMALECOUNT,                             ");
            parameter.AppendSql("  WORKERWHITEFEMALECOUNT = :WORKERWHITEFEMALECOUNT,                         ");
            parameter.AppendSql("  WORKERBLUEMALECOUNT = :WORKERBLUEMALECOUNT,                               ");
            parameter.AppendSql("  WORKERBLUEFEMALECOUNT = :WORKERBLUEFEMALECOUNT,                           ");
            parameter.AppendSql("  MANAGEWORKERCOUNT = :MANAGEWORKERCOUNT,                                   ");
            parameter.AppendSql("  MANAGEDOCTOR = :MANAGEDOCTOR,                                             ");
            parameter.AppendSql("  MANAGEDOCTORSTARTDATE = :MANAGEDOCTORSTARTDATE,                           ");
            parameter.AppendSql("  MANAGEDOCTORCOUNT = :MANAGEDOCTORCOUNT,                                   ");
            parameter.AppendSql("  MANAGENURSE = :MANAGENURSE,                                               ");
            parameter.AppendSql("  MANAGENURSESTARTDATE = :MANAGENURSESTARTDATE,                             ");
            parameter.AppendSql("  MANAGENURSECOUNT = :MANAGENURSECOUNT,                                     ");
            parameter.AppendSql("  MANAGEENGINEER = :MANAGEENGINEER,                                         ");
            parameter.AppendSql("  MANAGEENGINEERSTARTDATE = :MANAGEENGINEERSTARTDATE,                       ");
            parameter.AppendSql("  MANAGEENGINEERCOUNT = :MANAGEENGINEERCOUNT,                               ");
            parameter.AppendSql("  VISITWEEK = :VISITWEEK,                                                   ");
            parameter.AppendSql("  VISITDAY = :VISITDAY,                                                     ");
            parameter.AppendSql("  COMMISSION = :COMMISSION,                                                 ");
            parameter.AppendSql("  DECLAREDAY = :DECLAREDAY,                                                 ");
            parameter.AppendSql("  CONTRACTSTARTDATE = :CONTRACTSTARTDATE,                                   ");
            parameter.AppendSql("  CONTRACTENDDATE = :CONTRACTENDDATE,                                       ");
            parameter.AppendSql("  POSITION = :POSITION,                                                     ");
            parameter.AppendSql("  ISROTATION = :ISROTATION,                                                 ");
            parameter.AppendSql("  ISPRODUCTTYPE = :ISPRODUCTTYPE,                                           ");
            parameter.AppendSql("  ISLABOR = :ISLABOR,                                                       ");
            parameter.AppendSql("  BUILDINGTYPE = :BUILDINGTYPE,                                                                  ");
            parameter.AppendSql("  WORKSTARTTIME = :WORKSTARTTIME,                                           ");
            parameter.AppendSql("  WORKENDTIME = :WORKENDTIME,                                               ");
            parameter.AppendSql("  WORKMEETTIME = :WORKMEETTIME,                                             ");
            parameter.AppendSql("  WORKROTATIONTIME = :WORKROTATIONTIME,                                     ");
            parameter.AppendSql("  WORKLUANCHTIME = :WORKLUANCHTIME,                                         ");
            parameter.AppendSql("  WORKRESTTIME = :WORKRESTTIME,                                             ");
            parameter.AppendSql("  WORKEDUTIME = :WORKEDUTIME,                                               ");
            parameter.AppendSql("  WORKETCTIME = :WORKETCTIME,                                               ");
            parameter.AppendSql("  ISCONTRACT = :ISCONTRACT,                                                 ");
            parameter.AppendSql("  ISWEM = :ISWEM,                                                           ");
            parameter.AppendSql("  ISWEMDATA = :ISWEMDATA,                                                   ");
            parameter.AppendSql("  ISCOMMITTEE = :ISCOMMITTEE,                                               ");
            parameter.AppendSql("  ISSKELETON = :ISSKELETON,                                                 ");
            parameter.AppendSql("  ISSKELETONDATE = :ISSKELETONDATE,                                         ");
            parameter.AppendSql("  ISSPACEPROGRAM = :ISSPACEPROGRAM,                                         ");
            parameter.AppendSql("  ISSPACEPROGRAMDATE = :ISSPACEPROGRAMDATE,                                 ");
            parameter.AppendSql("  ISEARPROGRAM = :ISEARPROGRAM,                                             ");
            parameter.AppendSql("  ISEARPROGRAMDATE = :ISEARPROGRAMDATE,                                     ");
            parameter.AppendSql("  ISSTRESS = :ISSTRESS,                                                     ");
            parameter.AppendSql("  ISSTRESSDATE = :ISSTRESSDATE,                                             ");
            parameter.AppendSql("  ISBRAINTEST = :ISBRAINTEST,                                               ");
            parameter.AppendSql("  ISBRAINTESTDATE = :ISBRAINTESTDATE,                                       ");
            parameter.AppendSql("  ISSPECIAL = :ISSPECIAL,                                                   ");
            parameter.AppendSql("  ISSPECIALDATA = :ISSPECIALDATA,                                           ");
            parameter.AppendSql("  ISDELETED = :ISDELETED,                                                   ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                                                  ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                                              ");
            
            parameter.AppendSql("WHERE ESTIMATE_ID = :ESTIMATE_ID                                                              ");

            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("CONTRACTDATE", dto.CONTRACTDATE);
            parameter.Add("SPECIALCONTRACT", dto.SPECIALCONTRACT);
            parameter.Add("WORKERTOTALCOUNT", dto.WORKERTOTALCOUNT);
            parameter.Add("WORKERWHITEMALECOUNT", dto.WORKERWHITEMALECOUNT);
            parameter.Add("WORKERWHITEFEMALECOUNT", dto.WORKERWHITEFEMALECOUNT);
            parameter.Add("WORKERBLUEMALECOUNT", dto.WORKERBLUEMALECOUNT);
            parameter.Add("WORKERBLUEFEMALECOUNT", dto.WORKERBLUEFEMALECOUNT);
            parameter.Add("MANAGEWORKERCOUNT", dto.MANAGEWORKERCOUNT);
            parameter.Add("MANAGEDOCTOR", dto.MANAGEDOCTOR);
            parameter.Add("MANAGEDOCTORSTARTDATE", dto.MANAGEDOCTORSTARTDATE);
            parameter.Add("MANAGEDOCTORCOUNT", dto.MANAGEDOCTORCOUNT);
            parameter.Add("MANAGENURSE", dto.MANAGENURSE);
            parameter.Add("MANAGENURSESTARTDATE", dto.MANAGENURSESTARTDATE);
            parameter.Add("MANAGENURSECOUNT", dto.MANAGENURSECOUNT);
            parameter.Add("MANAGEENGINEER", dto.MANAGEENGINEER);
            parameter.Add("MANAGEENGINEERSTARTDATE", dto.MANAGEENGINEERSTARTDATE);
            parameter.Add("MANAGEENGINEERCOUNT", dto.MANAGEENGINEERCOUNT);
            parameter.Add("VISITWEEK", dto.VISITWEEK);
            parameter.Add("VISITDAY", dto.VISITDAY);
            parameter.Add("COMMISSION", dto.COMMISSION);
            parameter.Add("DECLAREDAY", dto.DECLAREDAY);
            parameter.Add("CONTRACTSTARTDATE", dto.CONTRACTSTARTDATE);
            parameter.Add("CONTRACTENDDATE", dto.CONTRACTENDDATE);
            parameter.Add("POSITION", dto.POSITION);
            parameter.Add("ISROTATION", dto.ISROTATION);
            parameter.Add("ISPRODUCTTYPE", dto.ISPRODUCTTYPE);
            parameter.Add("ISLABOR", dto.ISLABOR);
            parameter.Add("BUILDINGTYPE", dto.BUILDINGTYPE);
            parameter.Add("WORKSTARTTIME", dto.WORKSTARTTIME);
            parameter.Add("WORKENDTIME", dto.WORKENDTIME);
            parameter.Add("WORKMEETTIME", dto.WORKMEETTIME);
            parameter.Add("WORKROTATIONTIME", dto.WORKROTATIONTIME);
            parameter.Add("WORKLUANCHTIME", dto.WORKLUANCHTIME);
            parameter.Add("WORKRESTTIME", dto.WORKRESTTIME);
            parameter.Add("WORKEDUTIME", dto.WORKEDUTIME);
            parameter.Add("WORKETCTIME", dto.WORKETCTIME);
            parameter.Add("ISCONTRACT", dto.ISCONTRACT);
            parameter.Add("ISWEM", dto.ISWEM);
            parameter.Add("ISWEMDATA", dto.ISWEMDATA);
            parameter.Add("ISCOMMITTEE", dto.ISCOMMITTEE);
            parameter.Add("ISSKELETON", dto.ISSKELETON);
            parameter.Add("ISSKELETONDATE", dto.ISSKELETONDATE);
            parameter.Add("ISSPACEPROGRAM", dto.ISSPACEPROGRAM);
            parameter.Add("ISSPACEPROGRAMDATE", dto.ISSPACEPROGRAMDATE);
            parameter.Add("ISEARPROGRAM", dto.ISEARPROGRAM);
            parameter.Add("ISEARPROGRAMDATE", dto.ISEARPROGRAMDATE);
            parameter.Add("ISSTRESS", dto.ISSTRESS);
            parameter.Add("ISSTRESSDATE", dto.ISSTRESSDATE);
            parameter.Add("ISBRAINTEST", dto.ISBRAINTEST);
            parameter.Add("ISBRAINTESTDATE", dto.ISBRAINTESTDATE);
            parameter.Add("ISSPECIAL", dto.ISSPECIAL);
            parameter.Add("ISSPECIALDATA", dto.ISSPECIALDATA);
            parameter.Add("ISDELETED", dto.ISDELETED);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);

            return FindByEstimateId(dto.ESTIMATE_ID);
        }

        public void Delete(long estimateId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HC_OSHA_CONTRACT                                               ");
            parameter.AppendSql("SET ISDELETED = 'Y'                                                        ");
            parameter.AppendSql("WHERE ESTIMATE_ID = :ESTIMATE_ID                                           ");  
            parameter.Add("ESTIMATE_ID", estimateId);
            ExecuteNonQuery(parameter);

        }
    }
}
