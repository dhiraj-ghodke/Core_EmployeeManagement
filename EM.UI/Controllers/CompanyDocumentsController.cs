using EM.Core.Models;
using EM.Core.Models.Entities;
using EM.Models;
using EM.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EM.UI.Controllers
{
    public class CompanyDocumentsController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ICompanyDocumentService _companyDocumentService;
        private readonly IGenericService<UserModel, AspNetUser> _gUserService;
        private readonly IGenericService<CompanyDocumentsTypeMetadata, EM.Core.Models.Entities.CompanyDocumentsType> _gCDocumentTypeService;
        private readonly IGenericService<CompanyMetadata, EM.Core.Models.Entities.Company> _gCompanyService;
        private readonly IGenericService<CompanyDocumentMetadata, EM.Core.Models.Entities.CompanyDocument> _gCDocumentService;

        public CompanyDocumentsController(IWebHostEnvironment env, ICompanyDocumentService companyDocumentService, IGenericService<UserModel, AspNetUser> gUserService, IGenericService<CompanyDocumentsTypeMetadata, CompanyDocumentsType> gCDocumentTypeService, IGenericService<CompanyMetadata, Core.Models.Entities.Company> gCompanyService, IGenericService<CompanyDocumentMetadata, Core.Models.Entities.CompanyDocument> gCDocumentService)
        {
            _env = env;
            _companyDocumentService = companyDocumentService;
            _gUserService = gUserService;
            _gCDocumentTypeService = gCDocumentTypeService;
            _gCompanyService = gCompanyService;
            _gCDocumentService = gCDocumentService;
        }

        // GET: CompanyDocuments
        public async Task<ActionResult> Index()
        {
            //var companyDocuments = _employeeDBContext.CompanyDocuments.Include(c => c.CreatedByNavigation)
            //    .Include(c1 => c1.Company).Include(c2 => c2.CompanyDocumentsType);
            IEnumerable<CompanyDocumentMetadata> companyDocuments = await _companyDocumentService.GetCompanyDocumentWithAllDetails();
            return View(companyDocuments.ToList());

        }


        // GET: CompanyDocuments/Create
        public async Task<ActionResult> Create()
        {
            string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            IEnumerable<UserModel> users = await _gUserService.GetAllAsync();
            ViewBag.CreatedBy = new SelectList
                (users.Where(u => u.Id.Equals(loggedInUserId)), "Id", "Email");

            IEnumerable<CompanyDocumentsTypeMetadata> companyDocumentsTypes = await _gCDocumentTypeService.GetAllAsync();
            ViewBag.DocumentType = companyDocumentsTypes.ToList().Select(cd =>
            new SelectListItem()
            {
                Value = cd.DocumentTypeId.ToString(),
                Text = cd.DocumentTypeName
            }).ToList();

            //ViewBag.DocumentType = new List<SelectListItem>()
            //{
            //    new SelectListItem(){ Text = "Offer Letter Format", Value = "OfferLetter" },
            //    new SelectListItem(){ Text = "Increment Letter Format", Value = "IncrementLetter" },
            //    new SelectListItem(){ Text = "Salary Slip Format", Value = "SalarySlip" },
            //    new SelectListItem(){ Text = "Reliving Letter Format", Value = "RelivingLetter" },
            //    new SelectListItem(){ Text = "Experience Letter Format", Value = "ExperienceLetter" }
            //};
            
            IEnumerable<CompanyMetadata> companies = await _gCompanyService.GetAllAsync();
            ViewBag.CompanyIdList = new SelectList
                (companies.ToList() , "CompanyId", "CompanyName");

            return View();
        }

        // POST: CompanyDocuments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CompanyDocumentMetadata companyDocument)
        {
            if (ModelState.IsValid)
            {
                CompanyMetadata company = await _gCompanyService.GetByUserIdAsync(u => u.CompanyId == companyDocument.CompanyId);
                string companyName = company.CompanyName;

                string fileName = Path.GetFileNameWithoutExtension(companyDocument.DocumentFile.FileName);
                string fileExtension = Path.GetExtension(companyDocument.DocumentFile.FileName);

                fileName = fileName + DateTime.Now.ToString("yymm") + fileExtension;

                //System.IO.Directory.CreateDirectory(Server.MapPath("~/documents/"));
                string documentsPath = Path.Combine(_env.ContentRootPath, "documents");

                if (!Directory.Exists(documentsPath))
                {
                    Directory.CreateDirectory(documentsPath);
                }


                //System.IO.Directory.CreateDirectory(Server.MapPath("~/documents/" + companyName));
                string documentsOfCompanyPath = Path.Combine(documentsPath,companyName);

                if (!Directory.Exists(documentsOfCompanyPath))
                {
                    Directory.CreateDirectory(documentsOfCompanyPath);
                }

                //companyDocument.DocumentPath = "~/documents/" + companyName + "/" + fileName;
                //fileName = Path.Combine(Server.MapPath("~/documents/" + companyName), fileName);
                fileName = Path.Combine(documentsOfCompanyPath, fileName);

                //companyDocument.DocumentFile.SaveAs(fileName);
                using (var stream = new FileStream(fileName, FileMode.Create))
                {
                    await companyDocument.DocumentFile.CopyToAsync(stream);
                }

                companyDocument.DocumentType = fileExtension.TrimStart(new char[] { '.' });

                companyDocument.DocumentPath = System.IO.File.ReadAllBytes(fileName);
                companyDocument.CompanyDocumentsTypeId = int.Parse(companyDocument.DocumentName);
                companyDocument.CreatedDate = DateTime.Now;

                //db.CompanyDocuments.Add(companyDocument);
                //db.SaveChanges();
                await _gCDocumentService.AddAsync(companyDocument);
                return RedirectToAction("Index");
            }

            string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //ViewBag.CreatedBy = new SelectList
            //    (db.AspNetUsers.Where(u => u.Id.Equals(loggedInUserId)), "Id", "Email");

            IEnumerable<UserModel> users = await _gUserService.GetAllAsync();
            ViewBag.CreatedBy = new SelectList
                (users.Where(u => u.Id.Equals(loggedInUserId)), "Id", "Email");

            //ViewBag.DocumentType = db.CompanyDocumentsTypes.ToList().Select(cd =>
            //new SelectListItem()
            //{
            //    Value = cd.DocumentTypeId.ToString(),
            //    Text = cd.DocumentTypeName
            //}).ToList();

            IEnumerable<CompanyDocumentsTypeMetadata> companyDocumentsTypes = await _gCDocumentTypeService.GetAllAsync();
            ViewBag.DocumentType = companyDocumentsTypes.ToList().Select(cd =>
            new SelectListItem()
            {
                Value = cd.DocumentTypeId.ToString(),
                Text = cd.DocumentTypeName
            }).ToList();


            //ViewBag.DocumentType = new List<SelectListItem>()
            //{
            //    new SelectListItem(){ Text = "Offer Letter Format", Value = "OfferLetter" },
            //    new SelectListItem(){ Text = "Increment Letter Format", Value = "IncrementLetter" },
            //    new SelectListItem(){ Text = "Salary Slip Format", Value = "SalarySlip" },
            //    new SelectListItem(){ Text = "Reliving Letter Format", Value = "RelivingLetter" },
            //    new SelectListItem(){ Text = "Experience Letter Format", Value = "ExperienceLetter" }
            //};

            //ViewBag.CompanyIdList = new SelectList
            //    (db.Companies, "CompanyId", "CompanyName");
            
            IEnumerable<CompanyMetadata> companies = await _gCompanyService.GetAllAsync();
            ViewBag.CompanyIdList = new SelectList
                (companies.ToList(), "CompanyId", "CompanyName");

            return View(companyDocument);
        }


        // GET: CompanyDocuments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            IEnumerable<CompanyDocumentMetadata> companyDocuments = await _companyDocumentService.GetCompanyDocumentWithAllDetails();
            CompanyDocumentMetadata companyDocument = companyDocuments.Where(u => u.Id == id).First();
            if (companyDocument == null)
            {
                return NotFound();
            }

            //serializing complex type containing another complex type
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = false
            };

            TempData["document"] = JsonSerializer.Serialize(companyDocument,options);
            //UserModel user = await _gUserService.GetByIdAsync(companyDocument.CreatedBy);
            //ViewBag.CompanyDocumentCreator = user.Email;

            return View(companyDocument);
        }


        // GET: CompanyDocuments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            CompanyDocumentMetadata companyDocument = await _gCDocumentService.GetByIdAsync(id);
            if (companyDocument == null)
            {
                return NotFound();
            }

            string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<CompanyDocumentsTypeMetadata> companyDocumentsTypes = await _gCDocumentTypeService.GetAllAsync();
            ViewBag.DocumentType = companyDocumentsTypes.ToList().Select(cd =>
            new SelectListItem()
            {
                Value = cd.DocumentTypeId.ToString(),
                Text = cd.DocumentTypeName
            }).ToList();

            //IEnumerable<CompanyDocumentsTypeMetadata> documentTypes1 = await _gCDocumentTypeService.GetAllAsync();
            //var documentTypes = documentTypes1.ToList().Select(cd =>
            //new SelectListItem()
            //{
            //    Value = cd.DocumentTypeId.ToString(),
            //    Text = cd.DocumentTypeName
            //}).ToList();

            //var documentTypes = new List<SelectListItem>()
            //{
            //    new SelectListItem(){ Text = "Offer Letter Format", Value = "OfferLetter" },
            //    new SelectListItem(){ Text = "Increment Letter Format", Value = "IncrementLetter" },
            //    new SelectListItem(){ Text = "Salary Slip Format", Value = "SalarySlip" },
            //    new SelectListItem(){ Text = "Reliving Letter Format", Value = "RelivingLetter" },
            //    new SelectListItem(){ Text = "Experience Letter Format", Value = "ExperienceLetter" }
            //};

            //ViewBag.DocumentType = documentTypes.Select(bg =>
            //new SelectListItem()
            //{ Text = bg.Text, Value = bg.Value, Selected = (int.Parse(bg.Value).Equals(companyDocument.CompanyDocumentsTypeId)) }
            //).ToList();
            IEnumerable<CompanyMetadata> companies = await _gCompanyService.GetAllAsync();
            ViewBag.CompanyIdList = new SelectList
                (companies.ToList(), "CompanyId", "CompanyName", companyDocument.CompanyId);

            if (id == null)
            {
                return BadRequest();
            }
            var json = JsonSerializer.Serialize(companyDocument) as string;
            TempData["document"] = json;

            IEnumerable<UserModel> user = await _gUserService.GetAllAsync();
            ViewBag.CreatedBy = new SelectList(user.ToList().Where(u => u.Id == loggedInUserId) , "Id", "Email", companyDocument.CreatedBy);
            return View(companyDocument);
        }

        // POST: CompanyDocuments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CompanyDocumentMetadata companyDocument)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<CompanyMetadata> company = await _gCompanyService.GetAllAsync();
                string companyName = company.FirstOrDefault(u => u.CompanyId == companyDocument.CompanyId).CompanyName;

                if (companyDocument.DocumentFile != null && !string.IsNullOrEmpty(companyDocument.DocumentFile.FileName))
                {
                    string fileName = Path.GetFileNameWithoutExtension(companyDocument.DocumentFile.FileName);
                    string fileExtension = Path.GetExtension(companyDocument.DocumentFile.FileName);

                    fileName = fileName + DateTime.Now.ToString("yymm") + fileExtension;

                    string documentsPath = Path.Combine(_env.ContentRootPath, "documents");

                    if (!Directory.Exists(documentsPath))
                    {
                        Directory.CreateDirectory(documentsPath);
                    }


                    string documentsOfCompanyPath = Path.Combine(documentsPath, companyName);

                    if (!Directory.Exists(documentsOfCompanyPath))
                    {
                        Directory.CreateDirectory(documentsOfCompanyPath);
                    }

                    fileName = Path.Combine(documentsOfCompanyPath, fileName);

                    using (var stream = new FileStream(fileName, FileMode.Create))
                    {
                        await companyDocument.DocumentFile.CopyToAsync(stream);
                    }

                    companyDocument.DocumentType = fileExtension.TrimStart(new char[] { '.' });
                    companyDocument.DocumentPath = System.IO.File.ReadAllBytes(fileName);

                }

                //CompanyDocumentMetadata existingCompanyDocument = await _gCDocumentService.GetByIdAsync(companyDocument.Id);

                //existingCompanyDocument.CompanyId = companyDocument.CompanyId;
                //existingCompanyDocument.DocumentName = companyDocument.DocumentName;
                //existingCompanyDocument.CreatedBy = companyDocument.CreatedBy;
                //existingCompanyDocument.CreatedDate = DateTime.Now;
                companyDocument.CreatedDate = DateTime.Now;


                //if (!string.IsNullOrEmpty(companyDocument.DocumentPath))
                //{
                //    existingCompanyDocument.DocumentPath = companyDocument.DocumentPath;
                //    existingCompanyDocument.DocumentType = companyDocument.DocumentType;
                //}

                //db.Entry(companyDocument).State = EntityState.Modified;
                companyDocument.CompanyDocumentsTypeId = int.Parse(companyDocument.DocumentName);
                await _gCDocumentService.UpdateAsync(companyDocument.Id, companyDocument);
                
                //serializing complex type containing another complex type
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = false
                };

                TempData["document"] = JsonSerializer.Serialize(companyDocument, options);
                return RedirectToAction("Index");
            }
            CompanyDocumentMetadata existingCompanyDocument = await _gCDocumentService.GetByIdAsync(companyDocument.Id);
            companyDocument.DocumentPath = existingCompanyDocument.DocumentPath;
            //ViewBag.CreatedBy = new SelectList(await _gUserService.GetAllAsync(), "Id", "Email", companyDocument.CreatedBy);
            await _gCDocumentService.UpdateAsync(companyDocument.Id, companyDocument);
            return View(companyDocument);
        }


        // GET: CompanyDocuments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            CompanyDocumentMetadata companyDocument = await _gCDocumentService.GetByIdAsync(id);
            if (companyDocument == null)
            {
                return NotFound();
            }
            return View(companyDocument);
        }

        // POST: CompanyDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _gCDocumentService.DeleteAsync(id);
            
            return RedirectToAction("Index");
        }


        //public void ShowDocument(CompanyDocument document)
        public async Task<ActionResult> ShowDocument()
        {

            //CompanyDocument document = (CompanyDocument)Session["document"];
            CompanyDocumentMetadata document = JsonSerializer.Deserialize<CompanyDocumentMetadata>(TempData["document"].ToString(), new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });

            //Split the string by character . to get file extension type  
            // string[] stringParts = document.DocumentName.Split(new char[] { '.' });


            if (document == null || document.DocumentPath == null)
                return NotFound();

            var documentType = await _gCDocumentTypeService.GetByIdAsync(document.CompanyDocumentsTypeId);
            var documentName = documentType?.DocumentTypeName ?? "Document";
            var fileExtension = document.DocumentType ?? "bin";

            // Set MIME type based on file extension
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType($"file.{fileExtension}", out string contentType))
            {
                contentType = "application/octet-stream"; // Default fallback
            }

            var fileName = $"{documentName}.{fileExtension}";

            return File(document.DocumentPath, contentType, fileName);

        }
    }
}
