using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QRCoder;
using QRService.Abstraction;
using QRService.Helper;
using QRService.Helper.Models;
using QRService.Models;
using QRService.Models.DBModels;
using QRService.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QRService.Repository
{
    public class QRCodesRepository : IQRCodesRepository
    {
        private readonly AzureStorageBlobConfig _config;
        private readonly qrserviceContext _context;
        public QRCodesRepository(IOptions<AzureStorageBlobConfig> config, qrserviceContext context)
        {
            _config = config.Value;
            _context = context;
        }
        
        public async Task<dynamic> DeleteQRCode(int id)
        {
            try
            {
                var qrcodes = _context.Qrcodes.Include(x => x.Qrcodesadvertisments).Include(x => x.Qrcodesusers).Include(x => x.Redeemedcodes).Where(x => x.QrCodeId == id).FirstOrDefault();
                if (qrcodes == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.QRCodeNotFound, StatusCodes.Status404NotFound);

                if (qrcodes.Qrcodesadvertisments != null)
                    _context.Qrcodesadvertisments.RemoveRange(qrcodes.Qrcodesadvertisments);

                if (qrcodes.Qrcodesusers != null)
                    _context.Qrcodesusers.RemoveRange(qrcodes.Qrcodesusers);

                if (qrcodes.Redeemedcodes != null)
                    _context.Redeemedcodes.RemoveRange(qrcodes.Redeemedcodes);
                _context.SaveChanges();

                var existingMediaReferenceName = qrcodes.ImageUrl.Split('/');
                if (CloudStorageAccount.TryParse(_config.StorageConnection, out CloudStorageAccount storageAccount))
                {
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobClient.GetContainerReference(_config.Container);
                    if (await container.ExistsAsync())
                    {
                        CloudBlob file = container.GetBlobReference(existingMediaReferenceName.LastOrDefault());
                        if (await file.ExistsAsync())
                            await file.DeleteAsync();
                    }
                }

                _context.Qrcodes.Remove(qrcodes);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.QRCodeDelete, false);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }
        public async Task<dynamic> InsertQRCode(QrcodesModel model)
        {
            string blobUrl = string.Empty;
            try
            {
                if (model == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.BadRequest, StatusCodes.Status400BadRequest);

                QRCodeGenerator _qrCode = new QRCodeGenerator();
                QRCodeData _qrCodeData = _qrCode.CreateQrCode(model.ResourceName, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(_qrCodeData);
                Bitmap qrCodeBitmap = qrCode.GetGraphic(20);
                var qrCodeBytes = Common.BitmapToBytesCode(qrCodeBitmap);

                if (CloudStorageAccount.TryParse(_config.StorageConnection, out CloudStorageAccount storageAccount))
                {
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobClient.GetContainerReference(_config.Container);
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(model.ResourceName + "_" + DateTime.UtcNow.Ticks.ToString() + ".jpg");
                    await blockBlob.UploadFromStreamAsync(new MemoryStream(qrCodeBytes));
                    blobUrl = blockBlob.Uri.AbsoluteUri;
                }

                if (string.IsNullOrEmpty(blobUrl))
                    throw new Exception(CommonMessage.ErrorUploading);

                Qrcodes qrcodes = new Qrcodes()
                {
                    ResourceName = model.ResourceName,
                    Details = model.Details,
                    InstitutionId = model.InstitutionId,
                    RedeemLimit = model.RedeemLimit,
                    ExpieryDate = model.ExpieryDate,
                    ImageUrl = blobUrl
                };

                _context.Qrcodes.Add(qrcodes);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.QRCodeInsert, true);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }
        public async Task<dynamic> UpdateQRCode(QrcodesModel model)
        {
            string blobUrl = string.Empty, qrCodeReference = string.Empty;
            try
            {
                if (model == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.BadRequest, StatusCodes.Status400BadRequest);
                
                var qrcodes = _context.Qrcodes.Where(x => x.QrCodeId == model.QrCodeId).FirstOrDefault();
                if (qrcodes == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.QRCodeNotFound, StatusCodes.Status404NotFound);

                if (!string.IsNullOrEmpty(qrcodes.ImageUrl))
                {
                    var existingMediaReferenceName = qrcodes.ImageUrl.Split('/');
                    if (CloudStorageAccount.TryParse(_config.StorageConnection, out CloudStorageAccount storageAccount))
                    {
                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                        CloudBlobContainer container = blobClient.GetContainerReference(_config.Container);
                        if (await container.ExistsAsync())
                        {
                            CloudBlob file = container.GetBlobReference(existingMediaReferenceName.LastOrDefault());
                            if (await file.ExistsAsync())
                                await file.DeleteAsync();
                        }

                        QRCodeGenerator _qrCode = new QRCodeGenerator();
                        QRCodeData _qrCodeData = _qrCode.CreateQrCode(model.ResourceName, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(_qrCodeData);
                        Bitmap qrCodeBitmap = qrCode.GetGraphic(20);
                        var qrCodeBytes = Common.BitmapToBytesCode(qrCodeBitmap);

                        CloudBlockBlob blockBlob = container.GetBlockBlobReference(model.ResourceName + "_" + DateTime.UtcNow.Ticks.ToString() + ".jpg");
                        await blockBlob.UploadFromStreamAsync(new MemoryStream(qrCodeBytes));
                        blobUrl = blockBlob.Uri.AbsoluteUri;
                    }
                }
                else
                {
                    if (CloudStorageAccount.TryParse(_config.StorageConnection, out CloudStorageAccount storageAccount))
                    {
                        QRCodeGenerator _qrCode = new QRCodeGenerator();
                        QRCodeData _qrCodeData = _qrCode.CreateQrCode(model.ResourceName, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(_qrCodeData);
                        Bitmap qrCodeBitmap = qrCode.GetGraphic(20);
                        var qrCodeBytes = Common.BitmapToBytesCode(qrCodeBitmap);

                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                        CloudBlobContainer container = blobClient.GetContainerReference(_config.Container);
                        CloudBlockBlob blockBlob = container.GetBlockBlobReference(model.ResourceName + "_" + DateTime.UtcNow.Ticks.ToString() + ".jpg");
                        await blockBlob.UploadFromStreamAsync(new MemoryStream(qrCodeBytes));
                        blobUrl = blockBlob.Uri.AbsoluteUri;
                    }
                }

                qrcodes.Details = model.Details;
                qrcodes.ExpieryDate = model.ExpieryDate;
                qrcodes.InstitutionId = model.InstitutionId;
                qrcodes.ResourceName = model.ResourceName;
                qrcodes.RedeemLimit = model.RedeemLimit;
                qrcodes.ImageUrl = blobUrl;
                _context.Qrcodes.Update(qrcodes);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.QRCodeUpdate, false);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }
        public dynamic GetQRCode(int qrCodeId, Pagination pageInfo)
        {
            QrcodesGetResponse response = new QrcodesGetResponse();
            int totalCount = 0;
            try
            {
                List<QrcodesModel> qrcodesModelList = new List<QrcodesModel>();
                if (qrCodeId == 0)
                {
                    qrcodesModelList = (from qrcode in _context.Qrcodes
                                        select new QrcodesModel()
                                        {
                                            QrCodeId = qrcode.QrCodeId,
                                            ResourceName = qrcode.ResourceName,
                                            Details = qrcode.Details,
                                            InstitutionId = qrcode.InstitutionId,
                                            RedeemLimit = qrcode.RedeemLimit,
                                            ExpieryDate = qrcode.ExpieryDate,
                                            ImageUrl = qrcode.ImageUrl
                                        }).OrderBy(a => a.QrCodeId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Qrcodes.ToList().Count();
                }
                else
                {
                    qrcodesModelList = (from qrcode in _context.Qrcodes
                                        where qrcode.QrCodeId == qrCodeId
                                        select new QrcodesModel()
                                        {
                                            QrCodeId = qrcode.QrCodeId,
                                            ResourceName = qrcode.ResourceName,
                                            Details = qrcode.Details,
                                            InstitutionId = qrcode.InstitutionId,
                                            RedeemLimit = qrcode.RedeemLimit,
                                            ExpieryDate = qrcode.ExpieryDate,
                                            ImageUrl = qrcode.ImageUrl
                                        }).OrderBy(a => a.QrCodeId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = totalCount = _context.Qrcodes.Where(x => x.QrCodeId == qrCodeId).ToList().Count();
                }

                if (qrcodesModelList == null || qrcodesModelList.Count == 0)
                    return ReturnResponse.ErrorResponse(CommonMessage.QRCodeNotFound, StatusCodes.Status404NotFound);

                var page = new Pagination
                {
                    offset = pageInfo.offset,
                    limit = pageInfo.limit,
                    total = totalCount
                };

                response.status = true;
                response.message = CommonMessage.QRCodeRetrived;
                response.pagination = page;
                response.data = qrcodesModelList;
                response.statusCode = StatusCodes.Status200OK;
                return response;
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }
        public dynamic GetAdvertisementQRCode(int advertisementId, Pagination pageInfo)
        {
            AdvertisementsQrcodesGetResponse response = new AdvertisementsQrcodesGetResponse();
            int totalCount = 0;
            try
            {
                List<GetQrcodesModel> qrcodesModelList = new List<GetQrcodesModel>();

                if (advertisementId == 0)
                    return ReturnResponse.ErrorResponse(CommonMessage.QRCodeNotFound, StatusCodes.Status404NotFound);

                qrcodesModelList = (from qrcode in _context.Qrcodes
                                    join qradvertisement in _context.Qrcodesadvertisments on qrcode.QrCodeId equals qradvertisement.QrCodeId
                                    where qradvertisement.AdvertismentId == advertisementId
                                    select new GetQrcodesModel()
                                    {
                                        AdvertisementId = qradvertisement.AdvertismentId,
                                        Details = qrcode.Details,
                                        ImageUrl = qrcode.ImageUrl
                                    }).OrderBy(a => a.Details).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                totalCount = _context.Qrcodes.ToList().Count();

                if (qrcodesModelList == null || qrcodesModelList.Count == 0)
                    return ReturnResponse.ErrorResponse(CommonMessage.QRCodeNotFound, StatusCodes.Status404NotFound);

                var page = new Pagination
                {
                    offset = pageInfo.offset,
                    limit = pageInfo.limit,
                    total = totalCount
                };

                response.status = true;
                response.message = CommonMessage.QRCodeRetrived;
                response.pagination = page;
                response.data = qrcodesModelList;
                response.statusCode = StatusCodes.Status200OK;
                return response;
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }
    }
}
