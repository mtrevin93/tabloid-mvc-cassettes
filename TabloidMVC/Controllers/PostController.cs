using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;
using TabloidMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IPostTagRepository _postTagRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository, ICommentRepository commentRepository, IPostTagRepository postTagRepository, ISubscriptionRepository subscriptionRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
            _postTagRepository = postTagRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        public IActionResult Index()
        {
            //Use postlistviewmodel to pass current user's id
            var vm = new PostListViewModel() { UserId = GetCurrentUserProfileId() };

            vm.Posts = _postRepository.GetAllPublishedPosts();

            return View(vm);
        }

        public IActionResult MyIndex()
        {
            //Use postlistviewmodel to pass current user's id
            int userId = GetCurrentUserProfileId();

            var myPosts = _postRepository.GetMyPosts(userId);

            var vm = new PostListViewModel() { Posts = myPosts, UserId = userId };

            return View(vm);
        }

        public IActionResult Details(int id)
        {
            int userId = GetCurrentUserProfileId();
            
            
            //Gets published or unpublished post by user
            var post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }

            List<Subscription> subscriptions = _subscriptionRepository.GetActiveSubscriptions(userId);

            var subscription = subscriptions.FirstOrDefault(s => s.ProviderUserProfileId == post.UserProfileId);
            //Use postdetails view model to pass current user id

            PostDetailsViewModel vm = new PostDetailsViewModel { 
                Post = post, 
                CurrentUserId = userId,
                PostId = id,
                Category = _categoryRepository.GetCategoryById(post.CategoryId),
                PostTag = _postTagRepository.GetAllPostTags(id),
                Subscription = subscription
            };

            return View(vm);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
        } 
            catch(Exception ex)
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
    }
}
        public IActionResult Delete(Post post)
        {
            try
            {
                _postRepository.Delete(post);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Details", new { id = post.Id });
            }
        }
        public IActionResult Edit(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);

            post.Category = _categoryRepository.GetCategoryById(post.CategoryId);
            var categories = _categoryRepository.GetAll();

            var vm = new PostDetailsViewModel();

            vm.Categories = categories;
            vm.Post = post;

            return View(vm);
        }
        [HttpPost]
        public IActionResult Edit(PostDetailsViewModel postDetailsViewModel)
        {
            var post = postDetailsViewModel.Post;

            try
            {
                _postRepository.Update(post);

                return RedirectToAction("Details", new { id = post.Id } );
            }
            catch (Exception ex)
            {
                return View(postDetailsViewModel);
            }
        }


        public ActionResult Subscribe(PostDetailsViewModel pd)
        {
            int subscriber = GetCurrentUserProfileId();
            Post post = _postRepository.GetPublishedPostById(pd.PostId);
            try
            {
                Subscription subscription = new Subscription
                {
                    ProviderUserProfileId = post.UserProfileId,
                    SubscriberUserProfileId = subscriber,
                    BeginDateTime = DateTime.Now
                };
                _subscriptionRepository.AddSubscription(subscription);
                return RedirectToAction("Details", new { id = post.Id });
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Unsubscribe(int subscriptionId, int postId)
        {
            int subscriber = GetCurrentUserProfileId();
/*            Post post = _postRepository.GetPublishedPostById(pd.PostId);

            Subscription subscription = _subscriptionRepository.GetSubscriptionById(subscriber, post.UserProfileId);*/
            _subscriptionRepository.Unsubscribe(subscriptionId);
            return RedirectToAction("Details", new { id = postId });
        }


        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
