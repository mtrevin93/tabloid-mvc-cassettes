using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Repositories;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;

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

        public ActionResult DIndex()
        {
            List<UserProfile> userProfiles = _userProfileRepository.GetDeactivatedUsers();
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

        // GET: UserProfileController/Edit/5
        public ActionResult Edit(int id)
        {

            UserProfile user = _userProfileRepository.GetUserProfileById(id);
            List<UserType> userTypes = _userProfileRepository.GetAllUserTypes();
            UserTypeFormViewModel vm = new UserTypeFormViewModel
            {
                UserProfile = user,
                UserTypes = userTypes
            };
            return View(vm);
        }

        // POST: UserProfile/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UserProfile userProfile)
        {
            try
            {
                _userProfileRepository.Update(userProfile);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(userProfile);
            }
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
        public ActionResult Reactivate (int id)
        {
            UserProfile userProfile = _userProfileRepository.GetUserProfileById(id);
            try
            {
                if (userProfile.UserTypeId == 4)
                {
                    _userProfileRepository.ReactivateAuthorProfile(userProfile);
                    return RedirectToAction("DIndex");
                }
                if (userProfile.UserTypeId == 3)
                {
                    _userProfileRepository.ReactivateAdminProfile(userProfile);
                    return RedirectToAction("DIndex");
                }
                return StatusCode(404);
            }
            catch (Exception ex)
            {

                return View(userProfile);
            }
        }

        

        // GET: UserProfileController/Delete/5
        public ActionResult Delete(int id)
        {
            UserProfile userProfile = _userProfileRepository.GetUserProfileById(id);
            try
            {
                if (userProfile.UserTypeId == 2)
                {
                    _userProfileRepository.DeactivateAuthorProfile(userProfile);
                    return RedirectToAction("Index");
                }
                if (userProfile.UserTypeId == 1)
                {
                    _userProfileRepository.DeactivateAdminProfile(userProfile);
                    return RedirectToAction("Index");
                }
                return StatusCode(404);
            }
            catch (Exception ex)
            {

                return View(userProfile);
            }





        }




    }
}
