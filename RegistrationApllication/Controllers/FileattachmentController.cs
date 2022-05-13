using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using RegistrationApllication.Data;
using RegistrationApllication.Modal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RegistrationApllication.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]

    public class FileattachmentController : ControllerBase
    {
        public readonly UserDbContext dataModel;
        public FileattachmentController(UserDbContext userData)
        {
            dataModel = userData;

        }

        [HttpPost("Attachment"), DisableRequestSizeLimit]
        
        public IActionResult UploadFileAttachment(IFormFile files, string fileType)
        {
            try
            {
                var file = Request.Form.Files[0];
                fileType = Request.Form["fileType"];
                var date = DateTime.Now.Date.Month.ToString() + " " + DateTime.Now.Date.Year.ToString() + " " + DateTime.Now.Day.ToString();
                var folderName = Path.Combine("Resource", "Images", date);
                var pathtoSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    Directory.CreateDirectory(pathtoSave);
                    var fileName = file.FileName.Trim('"');
                    var fullPath = Path.Combine(pathtoSave, fileName).ToString();
                    var fileExtension = Path.GetExtension(fileName);
                    var dbpath = Path.Combine(folderName, fileName);
                    var filePathAttachment = Path.Combine(folderName, fileName).ToString();
                    using (var stream = new FileStream(fullPath, FileMode.Append))
                    {
                        file.CopyTo(stream);
                    }
                    var fileDetails = SaveFileToDB(fileName, fileType, filePathAttachment);

                    return Ok(fileDetails);

                }

                return BadRequest();
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        private FileAttachmentModelClass SaveFileToDB(string fileName, string fileType, string filePathAttachment)
        {
            var objFiles = new FileAttachmentModelClass()
            {
                AttachmentId = 0,
                AttachmentName = fileName,
                AttachmentType = fileType,
                AttachmentPath = filePathAttachment
            };

            dataModel.FileAttachment.Add(objFiles);
            dataModel.SaveChanges();
            return objFiles;
        }


        [HttpGet("atttchmentFile")]
        
        public IActionResult GetAttachmentPath()
        {
            var user = dataModel.FileAttachment.AsQueryable();
            return Ok(user);
        }

        [HttpGet("data")]
        
        public IActionResult DownloadFileAttachment(int id)
        {
            var file = dataModel.FileAttachment.Where(n => n.AttachmentId == id).FirstOrDefault();
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), file.AttachmentPath);
            //var fileName = file.AttachmentName;

            if (!System.IO.File.Exists(filepath))
                return NotFound();

            var memory = new MemoryStream();

            using (var stream = new FileStream(filepath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }

            memory.Position = 0;

            return File(memory, GetContentType(filepath), filepath);
        }

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;

            if (!provider.TryGetContentType(path, out contentType))
            {
                
                contentType = "application/octet-stream";
            }

            return contentType;
        }

        [HttpGet("GetAttachmentDetails")]
        
        public IActionResult GetAttachmentDetails(int candidateId)
        {
            var userData = dataModel.RegistrationDetail.Where(a => a.CandidateId == candidateId)
                .FirstOrDefault();
            var attachmentList = new List<FileAttachmentModelClass>();
            if (userData != null)
            {
                var attamenctIds = userData.AttachmentIds.Split(',');

                if (attamenctIds.Any())
                {
                    foreach (var attamenctId in attamenctIds)
                    {
                        var attachment = dataModel.FileAttachment.Where(n => n.AttachmentId.ToString() == attamenctId).FirstOrDefault();
                        attachmentList.Add(attachment);

                    }

                }
            }
            return Ok(attachmentList);
        }

    }
}