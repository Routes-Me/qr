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
    public class RedeemauthoritiesRepository : IRedeemauthoritiesRepository
    {
        private readonly qrserviceContext _context;
        public RedeemauthoritiesRepository(qrserviceContext context)
        {
            _context = context;
        }
        public dynamic DeleteRedeemAuthorities(int id)
        {
            try
            {
                var redeemauthorities = _context.Redeemauthorities.Where(x => x.RedeemAuthoritiesId == id).FirstOrDefault();
                if (redeemauthorities == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.RedeemAuthoritiesNotFound, StatusCodes.Status404NotFound);

                _context.Redeemauthorities.Remove(redeemauthorities);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.RedeemAuthoritiesDelete, true);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public dynamic GetRedeemAuthorities(int redeemAuthoritiesId, Pagination pageInfo)
        {
            RedeemauthoritiesGetResponse response = new RedeemauthoritiesGetResponse();
            int totalCount = 0;
            try
            {
                List<RedeemauthoritiesModel> redeemauthoritiesModelList = new List<RedeemauthoritiesModel>();

                if (redeemAuthoritiesId == 0)
                {
                    redeemauthoritiesModelList = (from redeemauthority in _context.Redeemauthorities
                                              select new RedeemauthoritiesModel()
                                              {
                                                  RedeemAuthoritiesId = redeemauthority.RedeemAuthoritiesId,
                                                  AuthorityId = redeemauthority.AuthorityId,
                                                  InstitutionId = redeemauthority.InstitutionId,
                                                  Pin = redeemauthority.Pin
                                              }).OrderBy(a => a.RedeemAuthoritiesId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Redeemauthorities.ToList().Count();
                }
                else
                {

                    redeemauthoritiesModelList = (from redeemauthority in _context.Redeemauthorities
                                                  where redeemauthority.RedeemAuthoritiesId == redeemAuthoritiesId
                                                  select new RedeemauthoritiesModel()
                                                  {
                                                      RedeemAuthoritiesId = redeemauthority.RedeemAuthoritiesId,
                                                      AuthorityId = redeemauthority.AuthorityId,
                                                      InstitutionId = redeemauthority.InstitutionId,
                                                      Pin = redeemauthority.Pin
                                                  })
                                              .OrderBy(a => a.RedeemAuthoritiesId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Redeemauthorities.Where(x => x.RedeemAuthoritiesId == redeemAuthoritiesId).ToList().Count();
                }

                if (redeemauthoritiesModelList == null || redeemauthoritiesModelList.Count == 0)
                    return ReturnResponse.ErrorResponse(CommonMessage.RedeemAuthoritiesNotFound, StatusCodes.Status404NotFound);
                
                var page = new Pagination
                {
                    offset = pageInfo.offset,
                    limit = pageInfo.limit,
                    total = totalCount
                };

                response.status = true;
                response.message = CommonMessage.RedeemAuthoritiesRetrived;
                response.pagination = page;
                response.data = redeemauthoritiesModelList;
                response.statusCode = StatusCodes.Status200OK;
                return response;
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public dynamic InsertRedeemAuthorities(RedeemauthoritiesModel model)
        {
            try
            {
                if (model == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.BadRequest, StatusCodes.Status400BadRequest);

                Redeemauthorities redeemauthorities = new Redeemauthorities()
                {
                    AuthorityId = model.AuthorityId,
                    InstitutionId = model.InstitutionId,
                    Pin = model.Pin
                };
                _context.Redeemauthorities.Add(redeemauthorities);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.RedeemAuthoritiesInsert, true);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public dynamic UpdateRedeemAuthorities(RedeemauthoritiesModel model)
        {
            try
            {
                if (model == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.BadRequest, StatusCodes.Status400BadRequest);

                var redeemauthorities = _context.Redeemauthorities.Where(x => x.RedeemAuthoritiesId == model.RedeemAuthoritiesId).FirstOrDefault();
                if (redeemauthorities == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.RedeemAuthoritiesNotFound, StatusCodes.Status404NotFound);

                redeemauthorities.AuthorityId = model.AuthorityId;
                redeemauthorities.InstitutionId = model.InstitutionId;
                redeemauthorities.Pin = model.Pin;
                _context.Redeemauthorities.Update(redeemauthorities);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.RedeemAuthoritiesUpdate, false);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }
    }
}