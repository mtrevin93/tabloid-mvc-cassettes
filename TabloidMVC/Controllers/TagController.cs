using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Repositories;
using TabloidMVC.Models;
using System.Security.Claims;

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
            return View(tags.OrderBy(t => t.Name));
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

        public ActionResult Delete(Tag tag)
        {
            try
            {
                _tagRepo.DeleteTag(tag.Id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Index");
            }
        }

        public ActionResult Edit(int id)
        {
            Tag tag = _tagRepo.GetTagById(id);

            return View(tag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Tag tag)
        {
            try
            {
                _tagRepo.UpdateTag(tag);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(tag);
            }
        }

        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
