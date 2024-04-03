using Ambit_CRM.Controllers;
using AmbitCRM.BO.DAL;
using AmbitCRM_Core.Interface;
using AmbitCRM_Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Web.Http.Controllers;


namespace Ambit_CRM.Handler
{
    public class ReadHeaders : IActionFilter
    {

        LoginDetails loginDetails = new LoginDetails();
        LoginDetails loginDetailsNew = new LoginDetails();
        ResponseModel response = new ResponseModel();
        DALCommon daLCommon = new DALCommon();
        public void OnActionExecuting(ActionExecutingContext context)
        {
           // var jwtToken = context.HttpContext.Request.Headers["jwtToken"].ToString();
            
            
            
            
            
            HeadersKeyDetails headersKeyDetails = new HeadersKeyDetails();
            headersKeyDetails.jwtToken = context.HttpContext.Request.Headers["jwtToken"].ToString();
            headersKeyDetails.empEmail = context.HttpContext.Request.Headers["empEmail"].ToString();
            headersKeyDetails.deviceId = context.HttpContext.Request.Headers["deviceId"].ToString();

            if (string.IsNullOrEmpty(headersKeyDetails.jwtToken) && string.IsNullOrEmpty(headersKeyDetails.empEmail) && string.IsNullOrEmpty(headersKeyDetails.deviceId))
            {
                //Do Nothing 
                //var req = HttpContext.Current.Request;
                //string a = "";         
            }
            else
            {
                loginDetails = daLCommon.GetLoginDetails(headersKeyDetails.jwtToken, headersKeyDetails.empEmail, headersKeyDetails.deviceId);

                if (loginDetails.id > 0)
                {
                    loginDetailsNew = CommonController.IsTokenActive(loginDetails);

                    if (loginDetailsNew.is_active)
                    {
                        response = daLCommon.UpdateLoginDetails(loginDetailsNew.id, loginDetailsNew.end_date);
                    }
                    else
                    {
                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        //if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                        //{
                            // Modify the response if unauthorized
                            context.Result = new ContentResult
                            {
                                StatusCode = (int)HttpStatusCode.Unauthorized,
                                Content = "Unauthorized access."
                            };
                       // }
                    }
                }
                else
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    //if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                    //{
                    // Modify the response if unauthorized
                    context.Result = new ContentResult
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized,
                        Content = "Unauthorized access."
                    };
                    // }
                }
            }
            // Continue with the action execution
            //  base.OnActionExecuting(actionContext);
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
