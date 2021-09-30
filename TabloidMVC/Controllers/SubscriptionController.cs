using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionRepository _subscriptionRepo;

            public SubscriptionController(ISubscriptionRepository subscriptionRepository)
            {
                _subscriptionRepo = subscriptionRepository;
            }
                
        // GET: SubscriptionController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SubscriptionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        public ActionResult Create(int postUserId)
        {
            int subscriber = GetCurrentUserId();
            try
            {
                Subscription subscription = new Subscription
                {
                    ProviderUserProfileId = postUserId,
                    SubscriberUserProfileId = subscriber,
                    BeginDateTime = DateTime.Now
                };
                _subscriptionRepo.AddSubscription(subscription);
                return RedirectToAction("Details", "Post", new { id = postUserId });
            }
            catch
            {
                return View("Index", "Home");
            }
        }

        // GET: SubscriptionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SubscriptionController/Edit/5
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

        // GET: SubscriptionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SubscriptionController/Delete/5
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

        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
