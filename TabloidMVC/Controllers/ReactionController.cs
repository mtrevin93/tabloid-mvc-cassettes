using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;
using TabloidMVC.Models;
using System.Security.Claims;

namespace TabloidMVC.Controllers
{
    public class ReactionController : Controller
    {

        private readonly IReactionRepository _reactionRepository;

        public ReactionController(IReactionRepository reactionRepository)
        {
            _reactionRepository = reactionRepository;
        }
        // GET: ReactionController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ReactionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReactionController/Create
        public ActionResult Create(int postId, int reactionId)
        {
            int userProfileId = GetCurrentUserProfileId();

            _reactionRepository.Add(postId, reactionId, userProfileId);

            return RedirectToAction("Details", "Post", new  { id = postId });
        }

        // GET: ReactionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReactionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReactionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReactionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
