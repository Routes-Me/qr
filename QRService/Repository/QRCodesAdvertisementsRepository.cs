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
    public class QRCodesAdvertisementsRepository : IQRCodesAdvertisementsRepository
    {
        private readonly qrserviceContext _context;
        public QRCodesAdvertisementsRepository(qrserviceContext context)
        {
            _context = context;
        }

        public dynamic DeleteQrcodesAdvertisments(int id)
        {
            try
            {
                var qrcodesadvertisments = _context.Qrcodesadvertisments.Where(x => x.QrCodesAdvertismentsId == id).FirstOrDefault();
                if (qrcodesadvertisments == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.QRCodeAdvertisementsNotFound, StatusCodes.Status404NotFound);

                _context.Qrcodesadvertisments.Remove(qrcodesadvertisments);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.QRCodeAdvertisementsDelete, false);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public dynamic GetQrcodesAdvertisments(int qrCodesAdvertisementsId, Pagination pageInfo)
        {
            QrcodesadvertismentsGetResponse response = new QrcodesadvertismentsGetResponse();
            int totalCount = 0;
            try
            {
                List<QrcodesadvertismentsModel> qrcodesadvertismentsModelList = new List<QrcodesadvertismentsModel>();

                if (qrCodesAdvertisementsId == 0)
                {
                    qrcodesadvertismentsModelList = (from qrcodesadvertisment in _context.Qrcodesadvertisments
                                                     select new QrcodesadvertismentsModel()
                                                     {
                                                         QrCodesAdvertismentsId = qrcodesadvertisment.QrCodesAdvertismentsId,
                                                         AdvertismentId = qrcodesadvertisment.AdvertismentId,
                                                         QrCodeId = qrcodesadvertisment.QrCodeId
                                                     }).OrderBy(a => a.QrCodesAdvertismentsId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Qrcodesadvertisments.ToList().Count();
                }
                else
                {
                    qrcodesadvertismentsModelList = (from qrcodesadvertisment in _context.Qrcodesadvertisments
                                                     where qrcodesadvertisment.QrCodesAdvertismentsId == qrCodesAdvertisementsId
                                                     select new QrcodesadvertismentsModel()
                                                     {
                                                         QrCodesAdvertismentsId = qrcodesadvertisment.QrCodesAdvertismentsId,
                                                         AdvertismentId = qrcodesadvertisment.AdvertismentId,
                                                         QrCodeId = qrcodesadvertisment.QrCodeId
                                                     }).OrderBy(a => a.QrCodesAdvertismentsId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Qrcodesadvertisments.Where(x => x.QrCodesAdvertismentsId == qrCodesAdvertisementsId).ToList().Count();
                }

                if (qrcodesadvertismentsModelList == null || qrcodesadvertismentsModelList.Count == 0)
                    return ReturnResponse.ErrorResponse(CommonMessage.QRCodeAdvertisementsNotFound, StatusCodes.Status404NotFound);
              
                var page = new Pagination
                {
                    offset = pageInfo.offset,
                    limit = pageInfo.limit,
                    total = totalCount
                };

                response.status = true;
                response.message = CommonMessage.QRCodeAdvertisementsRetrived;
                response.pagination = page;
                response.data = qrcodesadvertismentsModelList;
                response.statusCode = StatusCodes.Status200OK;
                return response;
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public dynamic InsertQrcodesAdvertisments(QrcodesadvertismentsModel model)
        {
            try
            {
                if (model == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.BadRequest, StatusCodes.Status400BadRequest);
                
                var qrcodes = _context.Qrcodes.Where(x => x.QrCodeId == model.QrCodeId).FirstOrDefault();
                if (qrcodes == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.QRCodeNotFound, StatusCodes.Status404NotFound);

                Qrcodesadvertisments qrcodesadvertisments = new Qrcodesadvertisments()
                {
                    QrCodeId = model.QrCodeId,
                    AdvertismentId = model.AdvertismentId
                };
                _context.Qrcodesadvertisments.Add(qrcodesadvertisments);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.QRCodeAdvertisementsInsert, true);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public dynamic UpdateQrcodesAdvertisments(QrcodesadvertismentsModel model)
        {
            try
            {
                if (model == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.BadRequest, StatusCodes.Status400BadRequest);

                var qrcodesadvertisments = _context.Qrcodesadvertisments.Where(x => x.QrCodeId == model.QrCodeId).FirstOrDefault();
                if (qrcodesadvertisments == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.QRCodeAdvertisementsNotFound, StatusCodes.Status404NotFound);

                var qrcodes = _context.Qrcodes.Where(x => x.QrCodeId == model.QrCodeId).FirstOrDefault();
                if (qrcodes == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.QRCodeNotFound, StatusCodes.Status404NotFound);

                qrcodesadvertisments.QrCodeId = model.QrCodeId;
                qrcodesadvertisments.AdvertismentId = model.AdvertismentId;
                _context.Qrcodesadvertisments.Update(qrcodesadvertisments);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.QRCodeAdvertisementsUpdate, false);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }
    }
}
