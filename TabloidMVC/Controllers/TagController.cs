using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Repositories;
using TabloidMVC.Models;

namespace TabloidMVC.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepo;

        public TagController(ITagRepository tagRepository)
        {
            _tagRepo = tagRepository;
        }
        public ActionResult Index()
        {
            List<Tag> tags = _tagRepo.GetAllTags();
            return View(tags);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tag newTag)
        {
            try
            {
                _tagRepo.AddTag(newTag);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(newTag);
            }
        }
    }
}
