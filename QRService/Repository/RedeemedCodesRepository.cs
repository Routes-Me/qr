using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QRService.Abstraction;
using QRService.Models;
using QRService.Models.DBModels;
using QRService.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QRService.Repository
{
    public class RedeemedCodesRepository : IRedeemedCodesRepository
    {
        private readonly qrserviceContext _context;
        public RedeemedCodesRepository(qrserviceContext context)
        {
            _context = context;
        }
        public dynamic DeleteRedeemedCodes(int id)
        {
            try
            {
                var redeemedcode = _context.Redeemedcodes.Where(x => x.RedeemedCodeId == id).FirstOrDefault();
                if (redeemedcode == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.RedeemedCodesNotFound, StatusCodes.Status404NotFound);

                _context.Redeemedcodes.Remove(redeemedcode);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.RedeemedCodesDelete, true);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public dynamic GetRedeemedCodes(int redeemedCodesId, Pagination pageInfo)
        {
            RedeemedcodesGetResponse response = new RedeemedcodesGetResponse();
            int totalCount = 0;
            try
            {
                List<RedeemedcodesModel> redeemedcodesModelList = new List<RedeemedcodesModel>();

                if (redeemedCodesId == 0)
                {
                    redeemedcodesModelList = (from redeemedcode in _context.Redeemedcodes
                                             select new RedeemedcodesModel()
                                             {
                                                 CreatedAt = redeemedcode.CreatedAt,
                                                 QrCodeId = redeemedcode.QrCodeId,
                                                 UserId = redeemedcode.UserId,
                                                 RedeemedCodeId = redeemedcode.RedeemedCodeId
                                             }).OrderBy(a => a.RedeemedCodeId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Redeemedcodes.ToList().Count();
                }
                else
                {

                    redeemedcodesModelList = (from redeemedcode in _context.Redeemedcodes
                                              where redeemedcode.RedeemedCodeId == redeemedCodesId
                                              select new RedeemedcodesModel()
                                              {
                                                  CreatedAt = redeemedcode.CreatedAt,
                                                  QrCodeId = redeemedcode.QrCodeId,
                                                  UserId = redeemedcode.UserId,
                                                  RedeemedCodeId = redeemedcode.RedeemedCodeId
                                              }).OrderBy(a => a.RedeemedCodeId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Redeemedcodes.Where(x => x.RedeemedCodeId == redeemedCodesId).ToList().Count();
                }

                if (redeemedcodesModelList == null || redeemedcodesModelList.Count == 0)
                    return ReturnResponse.ErrorResponse(CommonMessage.RedeemedCodesNotFound, StatusCodes.Status404NotFound);
                
                var page = new Pagination
                {
                    offset = pageInfo.offset,
                    limit = pageInfo.limit,
                    total = totalCount
                };

                response.status = true;
                response.message = CommonMessage.RedeemedCodesRetrived;
                response.pagination = page;
                response.data = redeemedcodesModelList;
                response.statusCode = StatusCodes.Status200OK;
                return response;
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public dynamic InsertRedeemedCodes(RedeemedcodesModel model)
        {
            try
            {
                if (model == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.BadRequest, StatusCodes.Status400BadRequest);

                var qrcodes = _context.Qrcodes.Where(x => x.QrCodeId == model.QrCodeId).FirstOrDefault();
                if (qrcodes == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.QRCodeNotFound, StatusCodes.Status404NotFound);

                Redeemedcodes redeemedcodes = new Redeemedcodes()
                {
                    QrCodeId = model.QrCodeId,
                    UserId = model.UserId,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Redeemedcodes.Add(redeemedcodes);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.RedeemedCodesInsert, true);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public dynamic UpdateRedeemedCodes(RedeemedcodesModel model)
        {
            try
            {
                if (model == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.BadRequest, StatusCodes.Status400BadRequest);

                var redeemedcodes = _context.Redeemedcodes.Include(x => x.QrCode).Where(x => x.RedeemedCodeId == model.RedeemedCodeId).FirstOrDefault();
                if (redeemedcodes == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.RedeemedCodesNotFound, StatusCodes.Status404NotFound);
                
                if (redeemedcodes.QrCode == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.QRCodeNotFound, StatusCodes.Status404NotFound);

                redeemedcodes.QrCodeId = model.QrCodeId;
                redeemedcodes.UserId = model.UserId;
                redeemedcodes.CreatedAt = model.CreatedAt;
                _context.Redeemedcodes.Update(redeemedcodes);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.RedeemedCodesUpdate, false);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }
    }
}
