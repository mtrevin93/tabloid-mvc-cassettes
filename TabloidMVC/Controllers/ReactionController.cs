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

        // GET: ReactionController/Create
        public ActionResult Create(int postId, int reactionId)
        {
            int userProfileId = GetCurrentUserProfileId();

            _reactionRepository.Add(postId, reactionId, userProfileId);

            return RedirectToAction("Details", "Post", new  { id = postId });
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
