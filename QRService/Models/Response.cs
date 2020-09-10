using Microsoft.AspNetCore.Http;
using QRService.Models.ResponseModel;
using System;
using System.Collections.Generic;

namespace QRService.Models
{
    public class Response
    {
        public bool status { get; set; }
        public string message { get; set; }
        public int statusCode { get; set; }
    }

    public class ReturnResponse
    {
        public static dynamic ExceptionResponse(Exception ex)
        {
            Response response = new Response();
            response.status = false;
            response.message = CommonMessage.ExceptionMessage + ex.Message;
            response.statusCode = StatusCodes.Status500InternalServerError;
            return response;
        }

        public static dynamic SuccessResponse(string message, bool isCreated)
        {
            Response response = new Response();
            response.status = true;
            response.message = message;
            if (isCreated)
                response.statusCode = StatusCodes.Status201Created;
            else
                response.statusCode = StatusCodes.Status200OK;
            return response;
        }

        public static dynamic ErrorResponse(string message, int statusCode)
        {
            Response response = new Response();
            response.status = true;
            response.message = message;
            response.statusCode = statusCode;
            return response;
        }
    }

    #region QRCode Reponse
    public class QrcodesResponse : Response { }

    public class QrcodesGetResponse : Response
    {
        public Pagination pagination { get; set; }
        public List<QrcodesModel> data { get; set; }
    }

    public class AdvertisementsQrcodesGetResponse : Response
    {
        public Pagination pagination { get; set; }
        public List<GetQrcodesModel> data { get; set; }
    }
    #endregion


    #region QRCodeAdvertisments Reponse
    public class QrcodesadvertismentsResponse : Response { }

    public class QrcodesadvertismentsGetResponse : Response
    {
        public Pagination pagination { get; set; }
        public List<QrcodesadvertismentsModel> data { get; set; }
    }
    #endregion

    #region QRCodeUsers Reponse
    public class QrcodesusersResponse : Response { }

    public class QrcodesusersGetResponse : Response
    {
        public Pagination pagination { get; set; }
        public List<QrcodesusersModel> data { get; set; }
    }
    #endregion

    #region RedeemAuthorities Reponse
    public class RedeemauthoritiesResponse : Response { }

    public class RedeemauthoritiesGetResponse : Response
    {
        public Pagination pagination { get; set; }
        public List<RedeemauthoritiesModel> data { get; set; }
    }
    #endregion

    #region RedeemAuthorities Reponse
    public class RedeemedcodesResponse : Response { }

    public class RedeemedcodesGetResponse : Response
    {
        public Pagination pagination { get; set; }
        public List<RedeemedcodesModel> data { get; set; }
    }
    #endregion
}
