using ApiTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Results;
using static ApiTest.Models.Services;

namespace ApiTest.Controllers
{
    public class PhotosController : ControllerBase
    {
        [Authorize]
        [HttpPost]
        [Produces("application/json")]
        [Route("getAlbumDetails")]
        public IActionResult GetAlbumDetails(string userId = "")
        {
            try
            {
                List<Response> objRespone = new List<Response>();
                Services objServices = new Services();
                var photosResult = objServices.GetPhotos();
                var albumsResult = objServices.GetAlbums();

                if (string.IsNullOrEmpty(userId))
                {
                    objRespone = GetAllDatas(photosResult, albumsResult);
                }
                else
                {
                    objRespone = GetAllDatasByUserId(photosResult, albumsResult, userId);
                }
                return Ok(objRespone);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /*Convert List to Datatable*/
        public DataTable ToDataTable<T>(IList<T> data)
        {
            try
            {
                PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
                DataTable table = new DataTable();
                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    table.Columns.Add(prop.Name, prop.PropertyType);
                }
                object[] values = new object[props.Count];
                foreach (T item in data)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }
                    table.Rows.Add(values);
                }
                return table;
            }
            catch (Exception ex)
            {
                throw new Exception("Service was unable to process request");
            }
        }


        /*Get all datas by combining both endpoints*/
        public List<Response> GetAllDatas(string photosResult, string albumsResult)
        {
            try
            {
                List<photos> photosList = JsonConvert.DeserializeObject<List<photos>>(photosResult.ToString());
                List<albums> albumsList = JsonConvert.DeserializeObject<List<albums>>(albumsResult.ToString());

                var dtPhotos = ToDataTable(photosList);
                var dtAlbums = ToDataTable(albumsList);

                var JoinResult = (from p in dtPhotos.AsEnumerable()
                                  join a in dtAlbums.AsEnumerable()
                                  on p.Field<string>("albumId") equals a.Field<string>("id")
                                  select new Response
                                  {
                                      userId = a.Field<string>("userId"),
                                      albumId = p.Field<string>("albumId"),
                                      url = p.Field<string>("url"),
                                      thumbnailUrl = p.Field<string>("thumbnailUrl")

                                  }).ToList();
                return JoinResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Service was unable to process request");
            }
        }


        /*Get all data relating to UserId in request*/
        public List<Response> GetAllDatasByUserId(string photosResult, string albumsResult, string userId)
        {
            try
            {
                List<photos> photosList = JsonConvert.DeserializeObject<List<photos>>(photosResult.ToString());
                List<albums> albumsList = JsonConvert.DeserializeObject<List<albums>>(albumsResult.ToString());

                var dtPhotos = ToDataTable(photosList);
                var dtAlbums = ToDataTable(albumsList);

                var JoinResult = (from p in dtPhotos.AsEnumerable()
                                  join a in dtAlbums.AsEnumerable()
                                  on p.Field<string>("albumId") equals a.Field<string>("id")
                                  where a.Field<string>("userId") == userId
                                  select new Response
                                  {
                                      userId = a.Field<string>("userId"),
                                      albumId = p.Field<string>("albumId"),
                                      url = p.Field<string>("url"),
                                      thumbnailUrl = p.Field<string>("thumbnailUrl")

                                  }).ToList();
                return JoinResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Service was unable to process request");
            }
        }

    }
}
