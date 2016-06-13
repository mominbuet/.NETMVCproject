using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using System.Web.Mvc;

using OG_SLN.Models;
using System.Data.Objects;

namespace OG_SLN.Controllers
{
    public class WebRequestsController : ApiController
    {
        private OGDBEntities db = new OGDBEntities();

        [HttpGet]
        [ActionName("departments")]
        public HttpResponseMessage DepartmentsByCompany(long? id = 0)
        {
            //var departments = db.SET_DEPARTMENT.Where(d => d.COMP_NO == company).ToList();

            var departments = from dep in db.SET_DEPARTMENT
                              where dep.COMP_NO == id
                              select new
                              {
                                  deptno = (int) (dep.DEPT_NO),
                                  deptname = dep.DEPT_NAME
                              };

            if (departments == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, departments);
        }

        [HttpPost]
        [ActionName("authority")]
        public HttpResponseMessage ReportingAuthorityForUserType(long? id = 0)
        {
            decimal auth_user_type = 0;
            if (id == (long)EUserTypes.Agent)
            {
                auth_user_type = (decimal)EUserTypes.ZonalManager;
            }
            else if (id == (long)EUserTypes.ZonalManager)
            {
                auth_user_type = (decimal)EUserTypes.AreaManager;
            }
            else if (id == (long)EUserTypes.AreaManager)
            {
                auth_user_type = (decimal)EUserTypes.DivisionalManager;
            }
            else if (id == (long)EUserTypes.DivisionalManager)
            {
                auth_user_type = (decimal)EUserTypes.SeniorManager;
            }
            else if (id == (long)EUserTypes.SeniorManager)
            {
                auth_user_type = (decimal)EUserTypes.MarketingHead;
            }
            else if (id == (long)EUserTypes.MarketingHead)
            {
                auth_user_type = (decimal)EUserTypes.Administrator;
            }

            var authority = from user in db.SEC_USERS
                              where user.USER_TYPE_NO == auth_user_type
                              select new
                              {
                                  authno = (int)(user.USER_NO),
                                  authname = user.USER_FULL_NAME,
                                  authhrid = user.HR_EMP_ID,
                                  authmobile = user.USER_MOBILE,
                                  authemail = user.USER_EMAIL
                              };

            if (authority == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, authority);
        }

        [HttpPost]
        [ActionName("search")]
        public HttpResponseMessage SearchUser([FromBody] UserSearchViewModel userdata)
        {
            decimal? active;
            if (userdata.active != null)
            {
                active = userdata.active == true ? 1 : 0;
            }
            else 
            {
                active = null;
            }

            ObjectResult<SEC_USERS_GET_Result> users = db.SEC_USERS_GET(userdata.mobile, null, userdata.email,
                   userdata.username, null, active, null, null, 
                   userdata.usertype, userdata.deptno, userdata.designo, userdata.hrid, 
                   userdata.fullname, null, null, null, 1, 10);
            
            var result = users.Select(u => new { 
                userno = (int) u.USER_NO,
                usertype = (int) u.USER_TYPE_NO,
                compno = u.COMP_NO,
                deptno = u.DEPT_NO,
                designo = u.DESIG_NO,
                hrid = u.HR_EMP_ID,
                fullname = u.USER_FULL_NAME,
                mobile = u.USER_MOBILE,
                email = u.USER_EMAIL,
                username = u.USER_NAME,
                active = u.IS_ACTIVE,

                nickname = u.USER_NICK_NAME,
                word = u.USER_PWD,
                contact = u.USER_CONTACT,
                address = u.USER_ADDR,
                activefrom = u.ACTIVE_FROM.ToString(),
                activeto = u.ACTIVE_TO.ToString()
            });

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [ActionName("Institute")]
        public HttpResponseMessage GetInstitute([FromBody] InstituteSearchViewModel insdata)
        {
            IQueryable<SET_INSTITUTE> institutes = db.SET_INSTITUTE.AsQueryable();

            if (insdata.Institute_Type.HasValue)
            {
                institutes = institutes.Where(i => i.INST_TYPE_NO == insdata.Institute_Type);
            }

            if (!string.IsNullOrEmpty(insdata.Institute_Dbid))
            {
                institutes = institutes.Where(i => i.F_INSTITUTION_DB_ID == insdata.Institute_Dbid);
            }

            if (!string.IsNullOrEmpty(insdata.Institute_Name))
            {
                institutes = institutes.Where(i => i.INSTITUTE_NAME.ToLower()
                    .Contains(insdata.Institute_Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(insdata.Institute_Eiin))
            {
                institutes = institutes.Where(i => i.EIIN_NUMBER == insdata.Institute_Eiin);
            }

            if (insdata.THANA_NO.HasValue)
            {
                institutes = institutes.Where(i => i.THANA_NO == insdata.THANA_NO);
            }
            else if (insdata.ZILLA_NO.HasValue)
            {
                var thanalist = from thana in db.SET_THANA
                                where thana.ZILLA_NO == insdata.ZILLA_NO
                                select thana.THANA_NO;
                institutes = institutes.Where(i => thanalist.Contains(i.THANA_NO));
            }
            else if (insdata.DIVISION_NO.HasValue)
            {
                var zillalist = from zilla in db.SET_ZILLA
                                where zilla.DIVISION_NO == insdata.DIVISION_NO
                                select zilla.ZILLA_NO;

                var thanalist = from thana in db.SET_THANA
                                where zillalist.Contains(thana.ZILLA_NO)
                                select thana.THANA_NO;
                institutes = institutes.Where(i => thanalist.Contains(i.THANA_NO));
            }

            var result = institutes.Select(i => new { 
                        insno = i.INSTITUTE_NO,
                        instype = i.SET_INST_TYPE.INST_TYPE_NAME,
                        insname = i.INSTITUTE_NAME,
                        inseiin = i.EIIN_NUMBER,
                        insdbid = i.F_INSTITUTION_DB_ID
                    });
            
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [ActionName("AssignedInstitute")]
        public HttpResponseMessage GetAssignedInstitute([FromBody] InstituteSearchViewModel insdata)
        {
            //IQueryable<SET_INSTITUTE> institutes = db.SET_INSTITUTE.AsQueryable();

            ObjectResult<TPR_INSTITUTE_ASSIGNMENTS_GET_Result> institutes =
                db.TPR_INSTITUTE_ASSIGNMENTS_GET(insdata.PROJECT_NO, insdata.USER_NO, insdata.Institute_Name,
                    insdata.Institute_Code, insdata.THANA_NO, insdata.ZILLA_NO, insdata.DIVISION_NO);

            var result = institutes.Select(i => new
            {
                insno = i.INSTITUTE_NO,
                insname = i.INSTITUTE_NAME,
                inseiin = i.EIIN_NUMBER,
                insdbid = i.F_INSTITUTION_DB_ID,
                thana = i.THANA_NAME,
                zilla = i.ZILLA_NAME,
                division = i.DIVISION_NAME
            });

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [ActionName("ZonalManager")]
        public HttpResponseMessage GetZonalManager([FromBody] ZonalManagerSearchViewModel za)
        {
            ObjectResult<SEC_USERS_GET_ZONAL_AGENT_Result> res = db.SEC_USERS_GET_ZONAL_AGENT(za.Zonal_Name,
                za.Zonal_Hrid, za.Zonal_Username, za.Zonal_Mobile, za.Zonal_Email, 
                za.Zonal_Dept, za.Zonal_Desig, za.Zonal_Parent, za.Zonal_User_Type, 
                za.Zonal_Thana, za.Zilla_No, za.Division_No);
                    
            var result = res.Select(z => new
                    {
                        zonalno = z.USER_NO,
                        zonalname = z.USER_FULL_NAME,
                        zonalmobile = z.USER_MOBILE,
                        zonalemail = z.USER_EMAIL,
                        zonalhremp = z.HR_EMP_ID
                    });

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [ActionName("Approve")]
        public HttpResponseMessage ApproveTeacher([FromUri] List<decimal> idList)
        {
            decimal approved = (decimal) ApproveType.Approved;
            decimal USER_NO = idList.ElementAt(0);
            decimal LOGON_NO = idList.ElementAt(1);

            idList.RemoveRange(0, 2);

            foreach (decimal teacher_no in idList)
            {
                db.SET_TEACHER_INFO_APPROVE(teacher_no, approved, USER_NO, LOGON_NO);
            }

            string status = "success";
            return Request.CreateResponse(HttpStatusCode.OK, status);
        }

        [HttpGet]
        [ActionName("Reject")]
        public HttpResponseMessage RejectTeacher([FromUri] List<decimal> idList)
        {
            decimal rejected = (decimal)ApproveType.Rejected;
            decimal USER_NO = idList.ElementAt(0);
            decimal LOGON_NO = idList.ElementAt(1);

            idList.RemoveRange(0, 2);

            foreach (decimal teacher_no in idList)
            {
                db.SET_TEACHER_INFO_APPROVE(teacher_no, rejected, USER_NO, LOGON_NO);
            }

            string status = "success";
            return Request.CreateResponse(HttpStatusCode.OK, status);
        }

        [HttpGet]
        [ActionName("zilla")]
        public HttpResponseMessage ZillasByDivision(long? id = 0)
        {
            var zillas = from zil in db.SET_ZILLA
                         where zil.DIVISION_NO == id
                              select new
                              {
                                  zillano = (int)(zil.ZILLA_NO),
                                  zillaname = zil.ZILLA_NAME
                              };

            if (zillas == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, zillas);
        }

        [HttpGet]
        [ActionName("thana")]
        public HttpResponseMessage ThanasByZilla(long? id = 0)
        {
            var thanas = from tha in db.SET_THANA
                         where tha.ZILLA_NO == id
                              select new
                              {
                                  thanano = (int)(tha.THANA_NO),
                                  thananame = tha.THANA_NAME
                              };

            if (thanas == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, thanas);
        }
    }
}
