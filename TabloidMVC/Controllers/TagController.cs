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
        public ActionResult Create(Tag tag)
        {
            List<Tag> tags = _tagRepo.GetAllTags();

            if (tags.Any(t => t.Name == tag.Name))
            {
                ModelState.AddModelError("", "Tag already exists.");
                return View(tag);
            }
            else
            {
                try
                {
                    _tagRepo.AddTag(tag);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return View(tag);
                }
            }
        }

        public ActionResult Delete()
        {
            return View("Index");
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Tag tag)
        {
            try
            {
                _tagRepo.DeleteTag(tag);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Index");
            }
        }
    }
}
