using Microsoft.AspNetCore.Http;
using QRService.Abstraction;
using QRService.Models;
using QRService.Models.DBModels;
using QRService.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QRService.Repository
{
    public class QRCodesUsersRepository : IQRCodesUsersRepository
    {
        private readonly qrserviceContext _context;
        public QRCodesUsersRepository(qrserviceContext context)
        {
            _context = context;
        }
        public dynamic DeleteQrcodesUsers(int id)
        {
            try
            {
                var qrcodesusers = _context.Qrcodesusers.Where(x => x.QrCodesUsersId == id).FirstOrDefault();
                if (qrcodesusers == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.QRCodeUsersNotFound, StatusCodes.Status404NotFound);

                _context.Qrcodesusers.Remove(qrcodesusers);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.QRCodeUsersDelete, false);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public dynamic GetQrcodesUsers(int qrCodesUsersId, Pagination pageInfo)
        {
            QrcodesusersGetResponse response = new QrcodesusersGetResponse();
            int totalCount = 0;
            try
            {
                List<QrcodesusersModel> qrcodesusersModelList = new List<QrcodesusersModel>();

                if (qrCodesUsersId == 0)
                {
                    qrcodesusersModelList = (from qrcodesuser in _context.Qrcodesusers
                                                     select new QrcodesusersModel()
                                                     {
                                                         QrCodesUsersId = qrcodesuser.QrCodesUsersId,
                                                         QrCodeId = qrcodesuser.QrCodeId,
                                                         UserId = qrcodesuser.UserId
                                                     }).OrderBy(a => a.QrCodesUsersId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Qrcodesusers.ToList().Count();
                }
                else
                {
                    qrcodesusersModelList = (from qrcodesuser in _context.Qrcodesusers
                                             where qrcodesuser.QrCodesUsersId == qrCodesUsersId
                                             select new QrcodesusersModel()
                                             {
                                                 QrCodesUsersId = qrcodesuser.QrCodesUsersId,
                                                 QrCodeId = qrcodesuser.QrCodeId,
                                                 UserId = qrcodesuser.UserId
                                             }).OrderBy(a => a.QrCodesUsersId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Qrcodesusers.Where(x => x.QrCodesUsersId == qrCodesUsersId).ToList().Count();
                }

                if (qrcodesusersModelList == null || qrcodesusersModelList.Count == 0)
                    return ReturnResponse.ErrorResponse(CommonMessage.QRCodeUsersNotFound, StatusCodes.Status404NotFound);

                var page = new Pagination
                {
                    offset = pageInfo.offset,
                    limit = pageInfo.limit,
                    total = totalCount
                };

                response.status = true;
                response.message = CommonMessage.QRCodeUsersRetrived;
                response.pagination = page;
                response.data = qrcodesusersModelList;
                response.statusCode = StatusCodes.Status200OK;
                return response;
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public dynamic InsertQrcodesUsers(QrcodesusersModel model)
        {
            try
            {
                if (model == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.BadRequest, StatusCodes.Status400BadRequest);

                var qrcodes = _context.Qrcodes.Where(x => x.QrCodeId == model.QrCodeId).FirstOrDefault();
                if (qrcodes == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.QRCodeNotFound, StatusCodes.Status404NotFound);

                Qrcodesusers qrcodesusers = new Qrcodesusers()
                {
                    QrCodeId = model.QrCodeId,
                    UserId = model.UserId
                };
                _context.Qrcodesusers.Add(qrcodesusers);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.QRCodeInsert, true);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public dynamic UpdateQrcodesUsers(QrcodesusersModel model)
        {
            try
            {
                if (model == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.BadRequest, StatusCodes.Status400BadRequest);

                var qrcodesuser = _context.Qrcodesusers.Where(x => x.QrCodeId == model.QrCodeId).FirstOrDefault();
                if (qrcodesuser == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.QRCodeUsersNotFound, StatusCodes.Status404NotFound);

                var qrcodes = _context.Qrcodes.Where(x => x.QrCodeId == model.QrCodeId).FirstOrDefault();
                if (qrcodes == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.QRCodeNotFound, StatusCodes.Status404NotFound);

                qrcodesuser.QrCodeId = model.QrCodeId;
                qrcodesuser.UserId = model.UserId;
                _context.Qrcodesusers.Update(qrcodesuser);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.QRCodeUsersUpdate, false);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }
    }
}
