using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Repositories;
using TabloidMVC.Models;

namespace TabloidMVC.Controllers
{
    public class UserProfileController : Controller
    {

        private readonly IUserProfileRepository _userProfileRepository;

        public UserProfileController(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }



        // GET: UserProfileController
        public ActionResult Index()
        {
            List<UserProfile> userProfiles = _userProfileRepository.GetAllUsers();
            return View(userProfiles);
        }

        // GET: UserProfileController/Details/5
        public ActionResult Details(int id)
        {
            UserProfile user = _userProfileRepository.GetUserProfileById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // GET: UserProfileController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserProfileController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: UserProfileController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserProfileController/Edit/5
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

        // GET: UserProfileController/Delete/5
        public ActionResult Delete(UserProfile userProfile)
        {
            try
            {
                _userProfileRepository.DeactivateUserProfile(userProfile);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                return View(userProfile);
            }
        }

        


    }
}
